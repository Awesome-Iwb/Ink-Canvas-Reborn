using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;
using Dubi906w.InkCanvasReborn.Wpf.Models;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn.Wpf.Services {
    /// <summary>
    /// 提供应用程序的设置管理功能。
    /// 包括加载、保存设置和监听配置文件变更。
    /// 
    /// Provides settings management for the application,
    /// including loading, saving, and monitoring configuration file changes.
    /// </summary>
    public class SettingsService : INotifyPropertyChanged {
        private readonly ILogger? _logger;
        private readonly object _fileSystemWatcherLocker = new();
        private Settings _settings = new();
        private static FileSystemWatcher _watcher;

        /// <summary>
        /// 当前应用的设置实例。
        /// 
        /// The current application settings instance.
        /// </summary>
        public Settings Settings {
            get => _settings;
            set => SetField(ref _settings, value);
        }

        /// <summary>
        /// 当属性值发生变化时触发的事件。
        /// 
        /// Event triggered when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 初始化 SettingsService 类的新实例。
        /// 
        /// Initializes a new instance of the <see cref="SettingsService"/> class.
        /// </summary>
        /// <param name="loggerFactory">用于创建日志记录器的工厂。</param>
        /// <param name="loggerFactory">The factory for creating loggers.</param>
        public SettingsService(ILoggerFactory? loggerFactory) {
            _logger = loggerFactory?.CreateLogger<SettingsService>();
            _logger?.LogInformation("SettingsService initialized.");
            InitFileSystemWatcher();
        }

        /// <summary>
        /// 加载配置文件中的设置到内存。
        /// 
        /// Loads settings from the configuration file into memory.
        /// </summary>
        public void LoadSettings() {
            try {
                if (File.Exists("./Settings.json")) {
                    _logger?.LogInformation("Configuration file found, loading settings...");
                    _settings = ConfigureFileHelper.LoadConfig<Settings>("./Settings.json");
                } else {
                    _logger?.LogInformation("Configuration file not found, initializing new settings...");
                }

                Settings.PropertyChanged += OnSettingsPropertyChanged;
            }
            catch (Exception ex) {
                _logger?.LogError(ex, "Failed to load configuration file.");
            }
        }

        /// <summary>
        /// 将当前设置保存到配置文件。
        /// 
        /// Saves the current settings to the configuration file.
        /// </summary>
        /// <param name="note">保存操作的注释信息。此处一般传入参数名称。</param>
        /// <param name="note">Optional note describing the save operation.</param>
        public void SaveSettings(string? note = "-") {
            _logger?.LogInformation($"Saving configuration file. Note: {note}");

            lock (_fileSystemWatcherLocker) {
                DisableFileWatcher();

                try {
                    ConfigureFileHelper.SaveConfig("./Settings.json", Settings);
                }
                catch (Exception ex) {
                    _logger?.LogError(ex, "Failed to save configuration file.");
                }
                finally {
                    EnableFileWatcher();
                }
            }
        }

        #region FileSystemWatcher

        /// <summary>
        /// 初始化文件系统监视器以监控配置文件的变更。
        /// 
        /// Initializes the file system watcher to monitor configuration file changes.
        /// </summary>
        private void InitFileSystemWatcher() {
            _watcher = new FileSystemWatcher {
                Path = AppDomain.CurrentDomain.BaseDirectory,
                IncludeSubdirectories = true,
                Filter = "Settings.json",
                NotifyFilter = NotifyFilters.LastWrite
            };

            _watcher.Changed += ConfigureFileChanged;
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 处理文件变更事件，重新加载配置文件。
        /// 
        /// Handles file change events to reload the configuration file.
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void ConfigureFileChanged(object sender, FileSystemEventArgs e) {
            lock (_fileSystemWatcherLocker) {
                try {
                    _logger?.LogWarning("Configuration file changed. Reloading settings...");
                    var newSettings = ConfigureFileHelper.ReadConfig<Settings>("./Settings.json");

                    SilentUpdateSettings(newSettings);
                    OnPropertyChanged(nameof(Settings));
                }
                catch (IOException ex) {
                    _logger?.LogError(ex, "Failed to reload configuration file.");
                }
            }
        }

        /// <summary>
        /// 静默更新当前设置，不触发事件。
        /// 
        /// Silently updates the current settings without triggering events.
        /// </summary>
        /// <param name="newSettings">新的设置对象。</param>
        /// <param name="newSettings">The new settings object.</param>
        private void SilentUpdateSettings(Settings newSettings) {
            Settings.SetSilentUpdate(Settings, true);

            foreach (var property in typeof(Settings).GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                property.SetValue(Settings, property.GetValue(newSettings));
            }

            Settings.SetSilentUpdate(Settings, false);
        }

        /// <summary>
        /// 禁用文件系统监视器。
        /// 
        /// Disables the file system watcher.
        /// </summary>
        private void DisableFileWatcher() => _watcher.EnableRaisingEvents = false;

        /// <summary>
        /// 启用文件系统监视器。
        /// 
        /// Enables the file system watcher.
        /// </summary>
        private void EnableFileWatcher() => _watcher.EnableRaisingEvents = true;

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// 触发属性变更事件。
        /// 
        /// Triggers the property changed event.
        /// </summary>
        /// <param name="propertyName">变更的属性名称。</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 设置字段值并触发属性变更通知。
        /// 
        /// Sets a field value and triggers property change notifications.
        /// </summary>
        /// <typeparam name="T">字段类型。</typeparam>
        /// <param name="field">字段引用。</param>
        /// <param name="value">新值。</param>
        /// <param name="propertyName">属性名称。</param>
        /// <returns>如果字段值已更改，则返回 true；否则返回 false。</returns>
        /// <returns>True if the field value changed, otherwise false.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 当设置属性变更时的回调方法。
        /// 
        /// Callback method when a settings property changes.
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e) {
            _logger?.LogWarning($"Settings property changed: {e.PropertyName}");
            SaveSettings(e.PropertyName);
        }

        #endregion
    }
}
