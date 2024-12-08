using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Console = Colorful.Console;

namespace Dubi906w.InkCanvasReborn.Helpers {

    internal static class ConsoleHelper {
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        /// <summary>
        /// 隐藏控制台
        /// </summary>
        public static void HideConsoleWindow() {
            ShowWindow(GetConsoleWindow(), SW_HIDE);
        }

        /// <summary>
        /// 显示控制台
        /// </summary>
        public static void ShowConsoleWindow(string ConsoleTitle = "") {
            ShowWindow(GetConsoleWindow(), SW_SHOW);
        }

        public static void ConsoleWrite_InkCanvasRebornASCIIText() {
            Console.Write(@"  _____       _       _____                            _____      _
 |_   _|     | |     / ____|                          |  __ \    | |
   | |  _ __ | | __ | |     __ _ _ ____   ____ _ ___  | |__) |___| |__   ___  _ __ _ __
   | | | '_ \| |/ / | |    / _` | '_ \ \ / / _` / __| |  _  // _ \ '_ \ / _ \| '__| '_ \
  _| |_| | | |   <  | |___| (_| | | | \ V / (_| \__ \ | | \ \  __/ |_) | (_) | |  | | | |
 |_____|_| |_|_|\_\  \_____\__,_|_| |_|\_/ \__,_|___/ |_|  \_\___|_.__/ \___/|_|  |_| |_|

", Color.FromArgb(59, 130, 246));
        }

        public static void ConsoleWrite_Copyright() {
            Console.WriteLine("Ink Canvas Reborn  (c) Dubi906w, WXRIW 2021-Present. ", Color.FromArgb(59, 130, 246));
        }
    }
}