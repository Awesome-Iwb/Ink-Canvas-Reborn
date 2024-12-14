using CommunityToolkit.Mvvm.Messaging;
using Dubi906w.InkCanvasReborn.Wpf.Services;
using Dubi906w.InkCanvasReborn.Wpf.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Dubi906w.InkCanvasReborn.Wpf.Helpers;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn.Wpf {

    public partial class App : Application {
        public static IHost AppHost;

        public void Application_Startup(object sender, StartupEventArgs e) {
            // 判断配置文件是否存在
            if (!File.Exists($"{AppContext.BaseDirectory}icr.json")) {
                File.WriteAllText($"{AppContext.BaseDirectory}icr.json", "{}");
            }

            // 创建 Host
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, builder) => {
                    builder.AddCommandLine(Environment.GetCommandLineArgs());
                    builder.AddInMemoryCollection(new[] {
                        new KeyValuePair<string, string>("instanceID", Guid.NewGuid().ToString())
                    });
                })
                .ConfigureServices((context, collection) => {
                    collection.AddSingleton<GCTimer>();
                    collection.AddSingleton<SettingsService>();
                    collection.AddSingleton<EdgeGesturesBlockerService>();

                    collection.AddSingleton(provider => {
                        var w = new IcrToolbarWindow {
                            Content = new Grid() {
                                Children = {
                                    new IcrToolbarView(provider.GetService<ILoggerFactory>(),
                                        provider.GetService<SettingsService>())
                                }
                            }
                        };
                        return w;
                    });
                })
                .Build();

            // 启动
            AppHost.Start();

            AppHost.Services.GetService<GCTimer>()?.SetTimer();
            AppHost.Services.GetService<EdgeGesturesBlockerService>()
                ?.InitEdgeGesturesBlockerService()
                ?.EnableEdgeGesturesBlocker();
            AppHost.Services.GetService<SettingsService>()?.LoadSettings();
            AppHost.Services.GetService<IcrToolbarWindow>()?.Show();
        }
    }
}