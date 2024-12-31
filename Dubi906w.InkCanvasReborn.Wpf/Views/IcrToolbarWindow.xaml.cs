using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows;
using System.Windows.Interop;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;
using Dubi906w.InkCanvasReborn.Wpf.Services;
using WindowsDesktop;
using Rect = System.Windows.Rect;
using Microsoft.Extensions.Logging;
using Dubi906w.InkCanvasReborn.Wpf.ViewModels;

namespace Dubi906w.InkCanvasReborn.Wpf.Views {
    /// <summary>
    /// IcrToolbarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IcrToolbarWindow : Window {
        private bool isToolbarStrictInScreen = false;
        private bool isToolbarWinHiddenFromAltTab = false;
        private readonly SettingsService? settingsService;
        private bool isToolbarAlwaysPinOnVirtualDesktops = false;

        private readonly ILoggerFactory? loggerFactory;
        private ILogger? logger;

        // 此处判断窗口是否已经显示出来，显示出来了才能操作VirtualDesktop。
        private bool isContentFullyRendered = false;

        public IcrToolbarWindow(SettingsService? settings, ILoggerFactory? factory) {
            InitializeComponent();

            settingsService = settings;
            loggerFactory = factory;
            // create logger
            logger = loggerFactory?.CreateLogger<IcrToolbarViewModel>();
            logger?.Log(LogLevel.Information, "IcrToolbarWindow Created.");

            if (settingsService != null) {
                LoadSettings();

                settingsService.Settings.PropertyChanged += (sender, args) => {
                    if (args.PropertyName is nameof(settingsService.Settings.IsToolbarStrictInWorkArea)
                        or nameof(settingsService.Settings.IsToolbarAlwaysPinOnVirtualDesktops)
                        or nameof(settingsService.Settings.IsToolbarWinHiddenFromAltTab))
                        LoadSettings();
                };

                settingsService.PropertyChanged += (sender, args) => {
                    if (args.PropertyName == nameof(settings.Settings)) Dispatcher.Invoke(LoadSettings);
                };
            }

            // 接收来自ViewModel的移动窗口消息
            WeakReferenceMessenger.Default.Register<MoveToolbarWindowMessage>(this, (r, m) => {
                if (isToolbarStrictInScreen) {
                    ReLocateToolbarWindow(m.X, m.Y);
                } else {
                    Left = m.X;
                    Top = m.Y;
                }
            });
        }

        private void LoadSettings() {
            if (settingsService != null) {
                isToolbarStrictInScreen = settingsService.Settings.IsToolbarStrictInWorkArea;
                isToolbarWinHiddenFromAltTab = settingsService.Settings.IsToolbarWinHiddenFromAltTab;
                isToolbarAlwaysPinOnVirtualDesktops = settingsService.Settings.IsToolbarAlwaysPinOnVirtualDesktops;
                if (isToolbarStrictInScreen) ReLocateToolbarWindow(Left, Top);
                if (isContentFullyRendered) {
                    switch (isToolbarWinHiddenFromAltTab) {
                        case false when isToolbarAlwaysPinOnVirtualDesktops:
                            PinToolbarOnVirtualDesktops(isToolbarAlwaysPinOnVirtualDesktops);
                            break;
                        case true when !isToolbarAlwaysPinOnVirtualDesktops:
                            UpdateAltTabVisibility(isToolbarWinHiddenFromAltTab);
                            break;
                    }

                    if (isToolbarWinHiddenFromAltTab && isToolbarAlwaysPinOnVirtualDesktops)
                        AlertAltTabHHiddenAndVirtualDesktopsConflict();
                }
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            Task.Run(() => {
                Task.Delay(50);
                Dispatcher.InvokeAsync(LoadSettings);
            });
        }

        private void ReLocateToolbarWindow(double left, double top) {
            var winRect = new Rect(left, top, Width, Height);
            var overflowing = SystemParameters.WorkArea.IsWithinBoundary(winRect);

            // 只有两边超出，如果有三边超出那就不对劲了（屏幕大小无论如何都比窗口大小小或窗口大小太大）（不考虑同侧两边都超出）
            if (overflowing[0] == 1) {
                Top = SystemParameters.WorkArea.Top;
            } else if (overflowing[1] == 1) {
                Top = SystemParameters.WorkArea.Bottom - Height;
            } else {
                Top = top;
            }

            if (overflowing[2] == 1) {
                Left = SystemParameters.WorkArea.Left;
            } else if (overflowing[3] == 1) {
                Left = SystemParameters.WorkArea.Right - Width;
            } else {
                Left = left;
            }
        }

        private void PinToolbarOnVirtualDesktops(bool isPinned) {
            if (!this.IsWindowVisibleInTaskView()) {
                this.SetAltTabWinVisibility(true);
            }

            if (isPinned) {
                VirtualDesktop.PinWindow(new WindowInteropHelper(this).Handle);
            } else {
                VirtualDesktop.UnpinWindow(new WindowInteropHelper(this).Handle);
            }
        }

        private void OnContentRendered(object sender, EventArgs e) {
            isContentFullyRendered = true;
            switch (isToolbarWinHiddenFromAltTab) {
                case false when isToolbarAlwaysPinOnVirtualDesktops:
                    PinToolbarOnVirtualDesktops(isToolbarAlwaysPinOnVirtualDesktops);
                    break;
                case true when !isToolbarAlwaysPinOnVirtualDesktops:
                    UpdateAltTabVisibility(isToolbarWinHiddenFromAltTab);
                    break;
            }

            if (isToolbarWinHiddenFromAltTab && isToolbarAlwaysPinOnVirtualDesktops)
                AlertAltTabHHiddenAndVirtualDesktopsConflict();
        }

        private void AlertAltTabHHiddenAndVirtualDesktopsConflict() {
            logger?.Log(LogLevel.Warning,
                "配置文件中的 toolbarWindowHiddenFromAltTab 和 toolbarAlwaysPinOnVirtualDesktops 同时开启会产生冲突，因此已经忽略这两个选项。请确保只有其中一个选项开启！");
        }

        private void UpdateAltTabVisibility(bool isHidden) {
            this.SetAltTabWinVisibility(!isHidden);
        }
    }

    public class MoveToolbarWindowMessage {
        public double X;
        public double Y;
    }
}