using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;
using Dubi906w.InkCanvasReborn.Wpf.Models;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn.Wpf.Services {

    public class SettingsService : INotifyPropertyChanged {
        private readonly ILoggerFactory? loggerFactory;
        private ILogger? logger;
        private Settings _settings = new Settings();

        public Settings Settings {
            get => _settings;
            set => SetField(ref _settings, value);
        }

        public SettingsService(ILoggerFactory? factory) {
            loggerFactory = factory;

            // create logger
            logger = loggerFactory?.CreateLogger<SettingsService>();
            logger?.Log(LogLevel.Information, "SettingsService Created.");
        }

        public void LoadSettings() {
            try {
                if (File.Exists("./Settings.json")) {
                    logger?.Log(LogLevel.Information, "已经找到配置文件，解析配置文件...");
                    var r = ConfigureFileHelper.LoadConfig<Settings>("./Settings.json");
                    _settings = r;
                } else {
                    logger?.Log(LogLevel.Information, "未找到配置文件，初始化配置文件中...");
                }
            }
            catch (Exception e) {
                logger?.LogError(e, "配置文件加载失败！已报错！");
            }

            Settings.PropertyChanged += (s, o) => {
                logger.Log(LogLevel.Warning, $"配置文件中的属性被修改：{o.PropertyName}");
                SaveSettings(o.PropertyName);
            };
        }

        public void SaveSettings(string? note = "-") {
            logger?.LogInformation("写入配置文件：" + note);
            ConfigureFileHelper.SaveConfig("./Settings.json", Settings);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}