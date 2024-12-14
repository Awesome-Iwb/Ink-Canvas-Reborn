using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {
    public static class RectUtils {
        public static int[] IsWithinBoundary(this Rect rect, Rect smallRect) {
            var workRect = rect;

            workRect.Intersect(smallRect);
            var h = Math.Round(workRect.Size.Height, 2);
            var w = Math.Round(workRect.Size.Width, 2);
            var wh = Math.Round(smallRect.Size.Height, 2);
            var ww = Math.Round(smallRect.Size.Width, 2);

            int topOverflowed = 0;
            int bottomOverflowed = 0;
            int leftOverflowed = 0;
            int rightOverflowed = 0;
            Trace.WriteLine(workRect.Left);

            // 超出工作区
            if (Math.Abs(h - wh) > 0.0001 || Math.Abs(w - ww) > 0.0001) {
                if (Math.Abs(h - wh) > 0.0001) {
                    if (workRect.Top <= rect.Top) topOverflowed = 1;
                    if (workRect.Bottom >= rect.Height) bottomOverflowed = 1;
                }

                if (Math.Abs(w - ww) > 0.0001) {
                    if (workRect.Left <= rect.Left) leftOverflowed = 1;
                    if (workRect.Right >= rect.Width) rightOverflowed = 1;
                }
            }

            return new[] { topOverflowed, bottomOverflowed, leftOverflowed, rightOverflowed };
        }
    }
}
