using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Dubi906w.InkCanvasReborn.Wpf.Models;
using Dubi906w.InkCanvasReborn.Wpf.ViewModels;
using Dubi906w.InkCanvasReborn.Wpf.Services;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn.Wpf.Views {

    public partial class IcrToolbarView : UserControl {
        private bool isToolbarBtnsVisible = true;
        private SettingsService? settingsService;
        private readonly ILoggerFactory? loggerFactory;
        private readonly ILogger? logger;

        public IcrToolbarView(ILoggerFactory? factory, SettingsService? settings) {
            InitializeComponent();

            settingsService = settings;
            loggerFactory = factory;

            // create logger
            logger = loggerFactory?.CreateLogger<IcrToolbarViewModel>();
            logger?.Log(LogLevel.Information, "IcrToolbarView Created.");

            if (factory is not null && settings is not null)
                DataContext = new IcrToolbarViewModel(factory, settings);

            WeakReferenceMessenger.Default.Register<ChangeToolbarVisibilityMessage>(this, (r, m) => {
                isToolbarBtnsVisible = m.IsSwitch ? !isToolbarBtnsVisible : m.IsVisible;
                if (isToolbarBtnsVisible) ToolbarBtnsBorder.Visibility = Visibility.Visible;
                var ani1 = new DoubleAnimation(isToolbarBtnsVisible ? 1 : 0,
                    TimeSpan.FromMilliseconds(m.IsAnimated ? 100 : 0)) {
                    EasingFunction = new PowerEase() { Power = 4, EasingMode = EasingMode.EaseOut },
                };
                ani1.Completed += (s, e) => {
                    if (!isToolbarBtnsVisible) ToolbarBtnsBorder.Visibility = Visibility.Collapsed;
                };
                ToolbarBtnsBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ani1);

                ToolbarBtnsBorder.BeginAnimation(OpacityProperty,
                    new DoubleAnimation(isToolbarBtnsVisible ? 1 : 0,
                        TimeSpan.FromMilliseconds(m.IsAnimated ? 100 : 0)));
            });
        }
    }

    public class ChangeToolbarVisibilityMessage {
        public bool IsVisible = true;
        public bool IsSwitch = false;
        public bool IsAnimated = true;
    }
}