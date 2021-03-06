using BlockingCollection;
using Seo.ViewModel;
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

namespace Seo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // public bool IsLoading { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            DataManager.Start();
           
            

        }

        private void AddressChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void start(object sender, CefSharp.FrameLoadStartEventArgs e)
        {
           
        }

        private void end(object sender, CefSharp.FrameLoadEndEventArgs e)
        {

        }

       

        private void LoadStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {

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
            Application.Current.Shutdown();

        }

        private void down(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
