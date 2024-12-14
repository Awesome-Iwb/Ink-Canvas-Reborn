using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dubi906w.InkCanvasReborn.Wpf.Models {

    public class Settings : ObservableObject {

        #region Toolbar

        private double _toolbarZoom = 1.0;
        private bool toolbarStrictInWorkArea = false;

        /// <summary>
        /// <para>工具栏缩放</para>
        /// <para>0.7407407 为真1x缩放，1.0为ic看起来差不多的缩放。</para>
        /// </summary>
        [JsonPropertyName("toolbarZoom")]
        public double ToolbarZoom {
            get => _toolbarZoom;
            set {
                if (Math.Abs(_toolbarZoom - value) < 0.00001) return;
                _toolbarZoom = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 始终让工具栏显示在屏幕可见范围之内。
        /// </summary>
        [JsonPropertyName("toolbarInWorkArea")]
        public bool IsToolbarStrictInWorkArea {
            get => toolbarStrictInWorkArea;
            set {
                if (toolbarStrictInWorkArea == value) return;
                toolbarStrictInWorkArea = value;
                OnPropertyChanged();
            }
        }

        #endregion Toolbar

        private bool _isEnableEdgeGesturesBlocker = true;

        /// <summary>
        /// 是否启用边缘手势屏蔽器
        /// </summary>
        [JsonPropertyName("enableEdgeGesturesBlocker")]
        public bool IsEnableEdgeGesturesBlocker {
            get => _isEnableEdgeGesturesBlocker;
            set {
                if (_isEnableEdgeGesturesBlocker == value) return;
                _isEnableEdgeGesturesBlocker = value;
                OnPropertyChanged();
            }
        }
    }
}