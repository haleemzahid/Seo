using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seo.BL;
using Seo.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Seo.ViewModel
{
  public  class MainViewModel:ViewModelBase
    {
        public RelayCommand<string> command{ get; set; }
        private UserControl _currentView;

        public UserControl CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; }
        }
        private DashbordUC _dashbordUC;

        public DashbordUC DashbordUC
        {
            get { return _dashbordUC; }
            set { _dashbordUC = value; }

            
        }
         

        public MainViewModel()
        {
            //var a = Helper.GetTables();
            command = new RelayCommand<string>(PerformAction);
            DashbordUC = new DashbordUC();
            CurrentView = new UserControl();
            CurrentView = DashbordUC;
        }

        private void PerformAction(string obj)
        {
            switch (obj)
            {
                case "Setting":
                    CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().PerformAction(obj);
                    break;


                case "Dashboard":
                    CurrentView = DashbordUC;
                    break;



                default:
                    break;
            }
        }
    }
}
