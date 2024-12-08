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
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration((context, builder) => {
                    builder.AddCommandLine(Environment.GetCommandLineArgs());
                    builder.AddJsonFile("icr.json", false, true);
                    builder.AddInMemoryCollection(new[] {
                        new KeyValuePair<string, string>("instanceID", Guid.NewGuid().ToString())
                    });
                })
                .ConfigureServices((context, collection) => {
                    collection.AddSingleton<IcrToolbarWindow>();
                    collection.AddSingleton<EdgeGesturesBlockerService>();
                })
                .Build();

            // 启动
            AppHost.Start();

            // 显示 IcrToolbarWindow
            AppHost.Services.GetService<IcrToolbarWindow>().Show();

            // 启动 EdgeGesturesBlocker
            AppHost.Services.GetService<EdgeGesturesBlockerService>().InitEdgeGesturesBlockerService();
            WeakReferenceMessenger.Default.Send(new EnableEdgeGesturesBlockerMessage());
        }
    }
}