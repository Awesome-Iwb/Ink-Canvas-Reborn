using CommunityToolkit.Mvvm.Messaging;
using System.Windows;

namespace Dubi906w.InkCanvasReborn.Wpf.Views {

    /// <summary>
    /// IcrToolbarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class IcrToolbarWindow : Window {

        public IcrToolbarWindow() {
            InitializeComponent();

            WeakReferenceMessenger.Default.Register<MoveToolbarWindowMessage>(this, (r, m) => {
                Left = m.X;
                Top = m.Y;
            });
        }
    }

    public class MoveToolbarWindowMessage {
        public double X;
        public double Y;
    }
}