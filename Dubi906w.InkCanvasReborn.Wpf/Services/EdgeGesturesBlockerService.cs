using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;

namespace Dubi906w.InkCanvasReborn.Wpf.Services {

    public class EdgeGesturesBlockerService {
        private readonly Window egbWin = new();

        private readonly ILoggerFactory? loggerFactory;
        private ILogger? logger;
        private readonly SettingsService? settings;

        public EdgeGesturesBlockerService(ILoggerFactory? factory, SettingsService? settings) {
            loggerFactory = factory;
            this.settings = settings;

            // create logger
            logger = loggerFactory?.CreateLogger<EdgeGesturesBlockerService>();
            logger?.Log(LogLevel.Information, "EdgeGesturesBlockerService Created.");
        }

        public EdgeGesturesBlockerService InitEdgeGesturesBlockerService() {
            logger?.Log(LogLevel.Information, $"配置文件中：enableEdgeGesturesBlocker 为 {settings?.Settings.IsEnableEdgeGesturesBlocker}");
            // create window
            egbWin.AllowsTransparency = true;
            egbWin.ResizeMode = ResizeMode.NoResize;
            egbWin.WindowStyle = WindowStyle.None;
            egbWin.Topmost = true;
            egbWin.Width = 2;
            egbWin.Height = 2;
            egbWin.Background = Brushes.Transparent;
            egbWin.Left = 0;
            egbWin.Top = 0;
            egbWin.Show();

            return this;
        }

        public EdgeGesturesBlockerService EnableEdgeGesturesBlocker() {
            EdgeGesturesUtil.DisableEdgeGestures(new WindowInteropHelper(egbWin).Handle, true);
            logger?.Log(LogLevel.Information, "EnableEdgeGesturesBlockerMessage Received.");
            return this;
        }

        public EdgeGesturesBlockerService DisableEdgeGesturesBlocker() {
            EdgeGesturesUtil.DisableEdgeGestures(new WindowInteropHelper(egbWin).Handle, false);
            logger?.Log(LogLevel.Information, "DisableEdgeGesturesBlockerMessage Received.");
            return this;
        }
    }
}