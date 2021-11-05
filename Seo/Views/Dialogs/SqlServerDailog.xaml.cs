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

namespace Seo.Views
{
    /// <summary>
    /// Interaction logic for SqlServerDailog.xaml
    /// </summary>
    public partial class SqlServerDailog : Window
    {
        public SqlServerDailog(ViewModel.DashbordViewModel mainViewModel)
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Current.Shutdown();
            
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
