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

namespace Dubi906w.InkCanvasReborn.Wpf.Views {

    /// <summary>
    /// IcrToolbarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IcrToolbarWindow : Window {
        private bool isToolbarStrictInScreen = false;
        private readonly SettingsService? settingsService;

        public IcrToolbarWindow(SettingsService? settings) {
            InitializeComponent();

            settingsService = settings;

            if (settingsService != null) {
                LoadSettings();

                settingsService.Settings.PropertyChanged += (sender, args) => {
                    if (args.PropertyName == nameof(settingsService.Settings.IsToolbarStrictInWorkArea)) LoadSettings();
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
            if (settingsService != null) isToolbarStrictInScreen = settingsService.Settings.IsToolbarStrictInWorkArea;
            if (isToolbarStrictInScreen) ReLocateToolbarWindow(Left, Top);
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

        private void OnContentRendered(object sender, EventArgs e) {
            VirtualDesktop.PinWindow(new WindowInteropHelper(this).Handle);
        }
    }

    public class MoveToolbarWindowMessage {
        public double X;
        public double Y;
    }
}