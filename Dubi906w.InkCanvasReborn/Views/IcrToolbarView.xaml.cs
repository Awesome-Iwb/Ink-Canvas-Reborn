using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Dubi906w.InkCanvasReborn.ViewModels;

namespace Dubi906w.InkCanvasReborn.Views {
    public partial class IcrToolbarView : UserControl {

        public IcrToolbarViewModel ViewModel { get; }

        public IcrToolbarView() {
            InitializeComponent();

            DataContext = new IcrToolbarViewModel();
        }
    }
}
