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
using System.Windows.Shapes;

namespace WnekoTrigger.Views
{
    /// <summary>
    /// Logika interakcji dla klasy HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            helpWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (helpWindow.WindowState == WindowState.Normal)
            {
                helpWindow.WindowState = WindowState.Maximized;
                helpWindow.maximizeButton.Content = " o ";
            }
            else
            {
                helpWindow.WindowState = WindowState.Normal;
                helpWindow.maximizeButton.Content = " O ";
            }
        }
    }
}
