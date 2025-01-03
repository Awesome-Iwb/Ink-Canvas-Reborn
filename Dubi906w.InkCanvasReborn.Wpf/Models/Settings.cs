﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dubi906w.InkCanvasReborn.Wpf.Models {

    public class Settings : ObservableObject {
        public static void SetSilentUpdate(Settings obj, bool isEnabled) {
            obj.isSilentUpdateProperty = isEnabled;
        }

        #region Toolbar

        private double toolbarZoom = 1.0;
        private bool toolbarStrictInWorkArea = false;
        private bool toolbarAlwaysPinOnVirtualDesktops = false;
        private double toolbarOpacity = 1.0;
        private bool toolbarWinHiddenFromAltTab = false;
        private bool toolbarTopMost = true;

        private bool isSilentUpdateProperty = false;

        /// <summary>
        /// <para>工具栏缩放</para>
        /// <para>0.7407407 为真1x缩放，1.0为ic看起来差不多的缩放。</para>
        /// </summary>
        [JsonPropertyName("toolbarZoom")]
        public double ToolbarZoom {
            get => toolbarZoom;
            set {
                if (Math.Abs(toolbarZoom - value) < 0.00001) return;
                toolbarZoom = value;
                if (!isSilentUpdateProperty) OnPropertyChanged();
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
                if (!isSilentUpdateProperty) OnPropertyChanged();
            }
        }

        /// <summary>
        /// 让工具栏固定显示在每一个虚拟桌面上。
        /// </summary>
        [JsonPropertyName("toolbarAlwaysPinOnVirtualDesktops")]
        public bool IsToolbarAlwaysPinOnVirtualDesktops {
            get => toolbarAlwaysPinOnVirtualDesktops;
            set {
                if (toolbarAlwaysPinOnVirtualDesktops == value) return;
                toolbarAlwaysPinOnVirtualDesktops = value;
                if (!isSilentUpdateProperty) OnPropertyChanged();
            }
        }

        /// <summary>
        /// 工具栏置顶显示，包括强制置顶和UIAccess软置顶。
        /// </summary>
        [JsonPropertyName("toolbarTopMost")]
        public bool IsToolbarTopMost {
            get => toolbarTopMost;
            set {
                if (toolbarTopMost == value) return;
                toolbarTopMost = value;
                if (!isSilentUpdateProperty) OnPropertyChanged();
            }
        }

        /// <summary>
        /// 让工具栏从AltTab菜单中消失
        /// </summary>
        [JsonPropertyName("toolbarWindowHiddenFromAltTab")]
        public bool IsToolbarWinHiddenFromAltTab {
            get => toolbarWinHiddenFromAltTab;
            set {
                if (toolbarWinHiddenFromAltTab == value) return;
                toolbarWinHiddenFromAltTab = value;
                if (!isSilentUpdateProperty) OnPropertyChanged();
            }
        }

        /// <summary>
        /// 工具栏不透明度
        /// </summary>
        [JsonPropertyName("toolbarOpacity")]
        public double ToolbarOpacity {
            get => toolbarOpacity;
            set {
                if (Math.Abs(toolbarOpacity - value) < 0.00001) return;
                toolbarOpacity = value;
                if (!isSilentUpdateProperty) OnPropertyChanged();
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
                if (!isSilentUpdateProperty) OnPropertyChanged();
            }
        }
    }
}