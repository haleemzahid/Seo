using Seo.ViewModel;
using CefSharp.WinForms;
using CefSharp;
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

         
        }

        private void AddressChanged(object sender, CefSharp.AddressChangedEventArgs e)
        {

        }

        private void Changed(object sender, TextChangedEventArgs e)
        {
            LoadUrl((sender as TextBox).Text);
        }




        //private void Changed(object sender, TextChangedEventArgs e)
        //{
        //    Browser.Load((sender as TextBox).Text);
        //}

        //private void Changes(object sender, CefSharp.AddressChangedEventArgs e)
        //{
        //    CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().LinkssSelectedData.SourceURL = e.NewValue.ToString();


        //    txtUrl.Text = e.NewValue.ToString();

        //}

        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                var a = wfhSample.Child;

                (a as ChromiumWebBrowser).Load(url);
            }
        }

        private void LoadStateIsChanged(object sender, LoadingStateChangedEventArgs e)
        {
            
            if (!e.IsLoading)
            {
                CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ControlVisibility = Visibility.Collapsed;
            }
            else
            {

                CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ControlVisibility = Visibility.Visible;
                CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().RaisePropertyChanged("ControlVisibility"); 
                //IsLoading = true;


            }
        }
    }
}
