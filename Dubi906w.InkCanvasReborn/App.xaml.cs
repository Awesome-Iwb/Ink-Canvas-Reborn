using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using Dubi906w.InkCanvasReborn.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dubi906w.InkCanvasReborn {
    public partial class App : Application {

        public static IHost AppHost;

        public void Application_Startup(object sender, StartupEventArgs e) {

            // 判断配置文件是否存在
            if (!File.Exists($"{AppContext.BaseDirectory}icr.json")) {
                File.WriteAllText($"{AppContext.BaseDirectory}icr.json","{}");
            }

            // 创建 Host
            AppHost = Host.CreateDefaultBuilder()
                .UseContentRoot(AppContext.BaseDirectory)
                .ConfigureAppConfiguration((context, builder) => {

                    builder.AddCommandLine(Program.Args);
                    builder.AddJsonFile("icr.json",false,true);
                    builder.AddInMemoryCollection(new [] {
                        new KeyValuePair<string, string>("instanceID", Guid.NewGuid().ToString())
                    });

                })
                .ConfigureServices((context, collection) => {
                    collection.AddSingleton<MainWindow>();
                    collection.AddSingleton<EdgeGesturesBlockerService>();
                })
                .Build();

            // 启动
            AppHost.Start();

            // 显示 MainWindow
            AppHost.Services.GetService<MainWindow>().Show();

            // 启动 EdgeGesturesBlocker
            AppHost.Services.GetService<EdgeGesturesBlockerService>().InitEdgeGesturesBlockerService();
            WeakReferenceMessenger.Default.Send(new EnableEdgeGesturesBlockerMessage());

        }
    }
}
