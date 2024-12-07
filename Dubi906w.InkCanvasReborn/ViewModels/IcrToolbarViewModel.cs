using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Dubi906w.InkCanvasReborn.Interfaces;
using Dubi906w.InkCanvasReborn.Models.Toolbar;
using Dubi906w.InkCanvasReborn.Views;

namespace Dubi906w.InkCanvasReborn.ViewModels {
    public partial class IcrToolbarViewModel : ObservableObject {

        /// <summary>
        /// 在默认模式下的工具栏图标
        /// </summary>
        public ObservableCollection<IToolbarItem> ToolbarDefaultModeItems { get; set; } = new ();

        private bool _canClearCanvas = true;

        [ObservableProperty]
        public double scalingFactor = 1.0;

        /// <summary>
        /// 图标字典，用于工具栏按钮的图标显示
        /// </summary>
        private ResourceDictionary iconsDictionary { get; set; } = (ResourceDictionary)new ResourceDictionary() {
            Source = new Uri("../Resources/Dictionaries/FluentIconsDictionary.xaml", UriKind.Relative)
        };

        /// <summary>
        /// 切换到墨迹书写模式
        /// </summary>
        [RelayCommand]
        private void SwitchToInkMode() {
            // MessageBox.Show("SwitchToInkMode!");
            _canClearCanvas = !_canClearCanvas;
            ClearCanvasCommand.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// 清屏
        /// </summary>
        [RelayCommand(CanExecute = nameof(CanClearCanvas))]
        private void ClearCanvas() {
            //MessageBox.Show("ClearCanvas!");
            ScalingFactor = 1.25;
        }

        public bool CanClearCanvas() {
            return _canClearCanvas;
        }

        public IcrToolbarViewModel() {
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("批注",iconsDictionary["PenIconToolbar"] as DrawingImage, SwitchToInkModeCommand));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("清屏",iconsDictionary["ClearIconToolbar"] as DrawingImage, ClearCanvasCommand));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("截图",iconsDictionary["ScreenshotIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("白板",iconsDictionary["WhiteboardModeIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("快捷面板",iconsDictionary["QuickPanelIconToolbar"] as DrawingImage));
            ToolbarDefaultModeItems.Add(new ToolbarBasicButton("设置",iconsDictionary["SettingsIconToolbar"] as DrawingImage));
        }
    }
}
