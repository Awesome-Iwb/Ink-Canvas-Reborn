using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {

    public static class DpiUtilities {

        // you should always use this one and it will fallback if necessary
        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getdpiforwindow
        public static int GetDpiForWindow(IntPtr hwnd) {
            var h = LoadLibrary("user32.dll");
            var ptr = GetProcAddress(h, "GetDpiForWindow"); // Windows 10 1607
            if (ptr == IntPtr.Zero)
                return GetDpiForNearestMonitor(hwnd);

            return Marshal.GetDelegateForFunctionPointer<GetDpiForWindowFn>(ptr)(hwnd);
        }

        public static int GetDpiForNearestMonitor(IntPtr hwnd) => GetDpiForMonitor(GetNearestMonitorFromWindow(hwnd));

        public static int GetDpiForNearestMonitor(int x, int y) => GetDpiForMonitor(GetNearestMonitorFromPoint(x, y));

        public static int GetDpiForMonitor(IntPtr monitor, MonitorDpiType type = MonitorDpiType.Effective) {
            var h = LoadLibrary("shcore.dll");
            var ptr = GetProcAddress(h, "GetDpiForMonitor"); // Windows 8.1
            if (ptr == IntPtr.Zero)
                return GetDpiForDesktop();

            int hr = Marshal.GetDelegateForFunctionPointer<GetDpiForMonitorFn>(ptr)(monitor, type, out int x, out int y);
            if (hr < 0)
                return GetDpiForDesktop();

            return x;
        }

        public static int GetDpiForDesktop() {
            int hr = D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED, typeof(ID2D1Factory).GUID, IntPtr.Zero, out ID2D1Factory factory);
            if (hr < 0)
                return 96; // we really hit the ground, don't know what to do next!

            factory.GetDesktopDpi(out float x, out float y); // Windows 7
            Marshal.ReleaseComObject(factory);
            return (int)x;
        }

        public static IntPtr GetDesktopMonitor() => GetNearestMonitorFromWindow(GetDesktopWindow());

        public static IntPtr GetShellMonitor() => GetNearestMonitorFromWindow(GetShellWindow());

        public static IntPtr GetNearestMonitorFromWindow(IntPtr hwnd) => MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

        public static IntPtr GetNearestMonitorFromPoint(int x, int y) => MonitorFromPoint(new POINT { x = x, y = y }, MONITOR_DEFAULTTONEAREST);

        private delegate int GetDpiForWindowFn(IntPtr hwnd);

        private delegate int GetDpiForMonitorFn(IntPtr hmonitor, MonitorDpiType dpiType, out int dpiX, out int dpiY);

        private const int MONITOR_DEFAULTTONEAREST = 2;

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string lpLibFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("user32")]
        private static extern IntPtr MonitorFromPoint(POINT pt, int flags);

        [DllImport("user32")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, int flags);

        [DllImport("user32")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32")]
        private static extern IntPtr GetShellWindow();

        [StructLayout(LayoutKind.Sequential)]
        private partial struct POINT {
            public int x;
            public int y;
        }

        [DllImport("d2d1")]
        private static extern int D2D1CreateFactory(D2D1_FACTORY_TYPE factoryType, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, IntPtr pFactoryOptions, out ID2D1Factory ppIFactory);

        private enum D2D1_FACTORY_TYPE {
            D2D1_FACTORY_TYPE_SINGLE_THREADED = 0,
            D2D1_FACTORY_TYPE_MULTI_THREADED = 1,
        }

        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("06152247-6f50-465a-9245-118bfd3b6007")]
        private interface ID2D1Factory {

            int ReloadSystemMetrics();

            [PreserveSig]
            void GetDesktopDpi(out float dpiX, out float dpiY);

            // the rest is not implemented as we don't need it
        }

        public static double GetWPFDPIScaling() {
            int resHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;  // 1440
            double actualHeight = SystemParameters.PrimaryScreenHeight;  // 960
            double ratio = actualHeight / resHeight;
            double dpi = (double)resHeight / actualHeight;
            return dpi;
        }
    }

    public enum MonitorDpiType {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }
}