using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Dubi906w.InkCanvasReborn.Wpf.Helpers {

    public class GCTimer {

        public void SetTimer() {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Interval = 5000;
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e) {
            GC.Collect();
        }
    }
}