using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Dubi906w.InkCanvasReborn.Wpf.Interfaces;

namespace Dubi906w.InkCanvasReborn.Wpf.Models.Toolbar {

    public class ToolbarBasicButton : IToolbarItem {
        private DrawingImage _icon;
        private string _name = "Empty Name";
        private ICommand _command;

        public string Name {
            get => _name;
            set {
                if (_name != value) {
                    _name = value;
                    Element = CreateButtonElement(Name, Icon, Command);
                }
            }
        }

        public DrawingImage Icon {
            get => _icon;
            set {
                if (_icon == null || !_icon.Equals(value)) {
                    _icon = value;
                    Element = CreateButtonElement(Name, Icon, Command);
                }
            }
        }

        public ICommand Command {
            get => _command;
            set {
                if (_command == null || !_command.Equals(value)) {
                    _command = value;
                    Element = CreateButtonElement(Name, Icon, Command);
                }
            }
        }

        public FrameworkElement Element { get; private set; }

        public ToolbarBasicButton(string name, DrawingImage icon) {
            Name = name;
            Icon = icon;
            Element = CreateButtonElement(Name, Icon, Command);
        }

        public ToolbarBasicButton(string name, DrawingImage icon, ICommand command) {
            Name = name;
            Icon = icon;
            Command = command;
            Element = CreateButtonElement(Name, Icon, Command);
        }

        public FrameworkElement CreateButtonElement(string name, DrawingImage icon, ICommand command) {
            var button = new Button();
            button.ToolTip = name;
            button.Command = command;

            var innerGrid = new Grid();
            innerGrid.Width = 31D;
            innerGrid.IsHitTestVisible = false;

            var iconImage = new Image();
            iconImage.Height = 22D;
            iconImage.Width = 22D;
            iconImage.HorizontalAlignment = HorizontalAlignment.Center;
            iconImage.VerticalAlignment = VerticalAlignment.Center;
            iconImage.Source = icon;

            innerGrid.Children.Add(iconImage);
            button.Content = innerGrid;
            return button;
        }
    }
}