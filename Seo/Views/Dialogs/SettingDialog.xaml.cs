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

namespace Seo.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SettingDialog.xaml
    /// </summary>
    public partial class SettingDialog : Window
    {
        public SettingDialog()
        {
            InitializeComponent();
        }
        private void Minimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
            {
                WindowState = WindowState.Normal;

            }
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        private void Down(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


    }
}
