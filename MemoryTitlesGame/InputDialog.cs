using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;

namespace MemoryTitlesGame
{
    public class InputDialog : Window
    {
        private readonly TextBox textBox = new TextBox();
        private readonly Button okButton = new Button { Content = "OK" };
        private readonly Button cancelButton = new Button { Content = "Cancel" };
        public string Answer => textBox.Text;

        public InputDialog(string prompt)
        {
            Title = prompt;
            Width = 300;
            Height = 120;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            var stackPanel = new StackPanel();
            stackPanel.Children.Add(textBox);
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            buttonPanel.Children.Add(okButton);
            buttonPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonPanel);
            Content = stackPanel;
            okButton.Click += (sender, e) => DialogResult = true;
            cancelButton.Click += (sender, e) => DialogResult = false;
        }
    }
}
