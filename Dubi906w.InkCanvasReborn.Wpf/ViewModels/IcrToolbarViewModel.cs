using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;
using Dubi906w.InkCanvasReborn.Wpf.Interfaces;
using Dubi906w.InkCanvasReborn.Wpf.Models;
using Dubi906w.InkCanvasReborn.Wpf.Models.Toolbar;
using Dubi906w.InkCanvasReborn.Wpf.Services;
using Dubi906w.InkCanvasReborn.Wpf.Views;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn.Wpf.ViewModels {
    public partial class IcrToolbarViewModel : ObservableObject {
        private readonly ILoggerFactory? loggerFactory;
        private readonly SettingsService? settings;
        private ILogger? logger;

        /// <summary>
        /// 在默认模式下的工具栏图标
        /// </summary>
        public ObservableCollection<IToolbarItem> ToolbarDefaultModeItems { get; set; } = new();

        private bool _canClearCanvas = true;

        [ObservableProperty] private double scalingFactor = 1.35;

        [ObservableProperty] private bool? strictInWorkArea = false;

        /// <summary>
        /// 图标字典，用于工具栏按钮的图标显示
        /// </summary>
        private ResourceDictionary iconsDictionary { get; set; } = (ResourceDictionary)new ResourceDictionary() {
            Source = new Uri("../Resources/Dictionaries/FluentIconsDictionary.xaml", UriKind.Relative)
        };

        /// <summary>
        /// 切换到墨迹书写模式
        /// </summary>
        [RelayCommand]
        private void SwitchToInkMode() {
            // MessageBox.Show("SwitchToInkMode!");
            _canClearCanvas = !_canClearCanvas;
            ClearCanvasCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 清屏
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanClearCanvas))]
        private void ClearCanvas() {
            settings.Settings.IsToolbarStrictInWorkArea = !settings.Settings.IsToolbarStrictInWorkArea;
        }

        public bool CanClearCanvas() {
            return _canClearCanvas;
        }

        private void InitSettings() {
            if (settings == null) return;

            ScalingFactor = 1.35 * settings.Settings.ToolbarZoom;
            StrictInWorkArea = settings.Settings.IsToolbarStrictInWorkArea;

            settings.Settings.PropertyChanged += (sender, args) => {
                if (args.PropertyName == nameof(settings.Settings.ToolbarZoom))
                    ScalingFactor = settings.Settings.ToolbarZoom;
                if (args.PropertyName == nameof(settings.Settings.IsToolbarStrictInWorkArea))
                    StrictInWorkArea = settings.Settings.IsToolbarStrictInWorkArea;
            };
        }

        public IcrToolbarViewModel(ILoggerFactory factory, SettingsService settings) {
            loggerFactory = factory;
            this.settings = settings;

            // create logger
            logger = loggerFactory?.CreateLogger<IcrToolbarViewModel>();
            logger?.Log(LogLevel.Information, "IcrToolbarViewModel Created.");

            InitSettings();

            _emojiIconImageSource = iconsDictionary["EmojiIconToolbar"] as DrawingImage;

            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("批注", iconsDictionary["PenIconToolbar"] as DrawingImage,
                SwitchToInkModeCommand));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("清屏",
                iconsDictionary["ClearIconToolbar"] as DrawingImage, ClearCanvasCommand));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("截图",
                iconsDictionary["ScreenshotIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("白板",
                iconsDictionary["WhiteboardModeIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("快捷面板",
                iconsDictionary["QuickPanelIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("设置",
                iconsDictionary["SettingsIconToolbar"] as DrawingImage));
        }

        #region 笑脸按钮

        [ObservableProperty] private ImageSource _emojiIconImageSource;

        private Point _mouseDownPoint;
        private Point _mouseDownDeltaPoint;
        private Point _mouseMovePoint;
        private bool _isEmojiBtnMouseDown = false; // 判断鼠标是否被按下
        private readonly double _dpiVal = DpiUtilities.GetWPFDPIScaling();
        private bool _isEmojiIconTriggerMoving = false; // 是否触发了工具栏的移动

        [RelayCommand]
        private void EmojiButtonMouseDown(MouseButtonEventArgs e) {
            if (_isEmojiBtnMouseDown) return;
            EmojiIconImageSource = iconsDictionary["Emoji2IconToolbar"] as DrawingImage;
            _mouseDownPoint = ((Visual)e.Source).PointToScreen(e.GetPosition((Border)e.Source));
            var borderLtPoint = ((Border)e.Source).PointToScreen(new Point(0, 0));
            _mouseDownDeltaPoint = new Point(_mouseDownPoint.X - borderLtPoint.X, _mouseDownPoint.Y - borderLtPoint.Y);
            var border = e.Source as Border;
            border.CaptureMouse();
            _isEmojiBtnMouseDown = true;
            _isEmojiIconTriggerMoving = false;
        }

        [RelayCommand]
        private void EmojiButtonMouseMove(MouseEventArgs e) {
            if (!_isEmojiBtnMouseDown) return;
            _mouseMovePoint = ((Visual)e.Source).PointToScreen(e.GetPosition((Border)e.Source));
            if (Math.Sqrt(Math.Pow(Math.Abs(_mouseDownPoint.X - _mouseMovePoint.X), 2) +
                          Math.Pow(Math.Abs(_mouseDownPoint.Y - _mouseMovePoint.Y), 2)) >= 16)
                _isEmojiIconTriggerMoving = true;
            if (_isEmojiIconTriggerMoving)
                WeakReferenceMessenger.Default.Send(new MoveToolbarWindowMessage() {
                    X = _mouseMovePoint.X / _dpiVal - _mouseDownDeltaPoint.X / _dpiVal,
                    Y = _mouseMovePoint.Y / _dpiVal - _mouseDownDeltaPoint.Y / _dpiVal
                });
        }

        [RelayCommand]
        private void EmojiButtonMouseUp(MouseButtonEventArgs e) {
            if (!_isEmojiBtnMouseDown) return;
            EmojiIconImageSource = iconsDictionary["EmojiIconToolbar"] as DrawingImage;
            var border = e.Source as Border;
            border.ReleaseMouseCapture();
            _isEmojiBtnMouseDown = false;
            if (_isEmojiIconTriggerMoving == false)
                WeakReferenceMessenger.Default.Send(new ChangeToolbarVisibilityMessage() {
                    IsSwitch = true
                });
            _isEmojiIconTriggerMoving = false;
        }

        #endregion 笑脸按钮
    }
}