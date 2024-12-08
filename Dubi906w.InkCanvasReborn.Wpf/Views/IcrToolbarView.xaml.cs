using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Dubi906w.InkCanvasReborn.Wpf.ViewModels;

namespace Dubi906w.InkCanvasReborn.Wpf.Views {

    public partial class IcrToolbarView : UserControl {
        public IcrToolbarViewModel ViewModel { get; }

        private bool _isToolbarBtnsVisible = true;

        public IcrToolbarView() {
            InitializeComponent();

            DataContext = new IcrToolbarViewModel();

            WeakReferenceMessenger.Default.Register<ChangeToolbarVisibilityMessage>(this, (r, m) => {
                _isToolbarBtnsVisible = m.IsSwitch ? !_isToolbarBtnsVisible : m.IsVisible;
                ToolbarBtnsBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, new DoubleAnimation(_isToolbarBtnsVisible ? 1 : 0, TimeSpan.FromMilliseconds(m.IsAnimated ? 100 : 0)) {
                    EasingFunction = new PowerEase() { Power = 4, EasingMode = EasingMode.EaseOut },
                });
                ToolbarBtnsBorder.BeginAnimation(OpacityProperty, new DoubleAnimation(_isToolbarBtnsVisible ? 1 : 0, TimeSpan.FromMilliseconds(m.IsAnimated ? 100 : 0)));
            });
        }
    }

    public class ChangeToolbarVisibilityMessage {
        public bool IsVisible = true;
        public bool IsSwitch = false;
        public bool IsAnimated = true;
    }
}