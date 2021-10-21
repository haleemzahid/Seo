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

namespace Seo.Views.UserControls
{
    /// <summary>
    /// Interaction logic for DashbordUC.xaml
    /// </summary>
    public partial class DashbordUC : UserControl
    {
        public DashbordUC()
        {
            InitializeComponent();
        }

        private void LoadStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {

            if (!e.IsLoading)
            {
                CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ControlVisibility = Visibility.Collapsed;
            }
            else
            {
                CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ControlVisibility = Visibility.Visible;
                //IsLoading = true;


            }
        }
    }
}
