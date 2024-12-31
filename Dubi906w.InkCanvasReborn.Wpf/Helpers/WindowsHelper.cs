using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {
    public static class WindowsHelper {
        #region Win32

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int WS_EX_APPWINDOW = 0x00040000;

        [DllImport("user32.dll")]
        static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [Flags]
        public enum GuiThreadInfoFlags {
            GUI_CARETBLINKING = 0x00000001,
            GUI_INMENUMODE = 0x00000004,
            GUI_INMOVESIZE = 0x00000002,
            GUI_POPUPMENUMODE = 0x00000010,
            GUI_SYSTEMMENUMODE = 0x00000008
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct GUITHREADINFO {
            public int cbSize;
            public GuiThreadInfoFlags flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public System.Drawing.Rectangle rcCaret;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr) {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        private static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4) {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            } else {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0)) {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        #endregion

        public static void SetAltTabWinVisibility(this Window win, bool isVisible = false) {
            var windowInterop = new WindowInteropHelper(win);
            int exStyle = (int)GetWindowLong(windowInterop.Handle, GWL_EXSTYLE);
            if (!isVisible) {
                exStyle |= WS_EX_TOOLWINDOW;
                exStyle |= 0x00000100;
                exStyle &= ~WS_EX_APPWINDOW;
            } else {
                exStyle &= ~WS_EX_TOOLWINDOW;
                exStyle &= ~0x00000100;
                exStyle |= WS_EX_APPWINDOW;
            }

            SetWindowLong(windowInterop.Handle, GWL_EXSTYLE, (IntPtr)exStyle);
        }

        public static bool IsWindowVisibleInTaskView(this Window win) {
            var windowInterop = new WindowInteropHelper(win);
            int exStyle = (int)GetWindowLong(windowInterop.Handle, GWL_EXSTYLE);

            bool isToolWindow = (exStyle & WS_EX_TOOLWINDOW) != 0;
            bool isAppWindow = (exStyle & WS_EX_APPWINDOW) != 0;

            return isAppWindow && !isToolWindow;
        }

        public static IntPtr GetLastFocusWindow() {
            GUITHREADINFO guiThreadInfo = new GUITHREADINFO();
            guiThreadInfo.cbSize = Marshal.SizeOf(guiThreadInfo);

            if (GetGUIThreadInfo(0, ref guiThreadInfo)) {
                return guiThreadInfo.hwndFocus;
            }

            return IntPtr.Zero;
        }
    }
}
