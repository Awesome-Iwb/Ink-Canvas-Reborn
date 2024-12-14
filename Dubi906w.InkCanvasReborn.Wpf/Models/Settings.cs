using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Dubi906w.InkCanvasReborn.Wpf.Models {

    public class Settings : ObservableObject {
        private double _toolbarZoom = 1.0;

        [JsonPropertyName("toolbarZoom")]
        public double ToolbarZoom {
            get => _toolbarZoom;
            set {
                if (Math.Abs(_toolbarZoom - value) < 0.00001) return;
                _toolbarZoom = value;
                OnPropertyChanged();
            }
        }

        private bool _isEnableEdgeGesturesBlocker = true;

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