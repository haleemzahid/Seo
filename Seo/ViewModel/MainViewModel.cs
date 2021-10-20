using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CsvHelper;
using CsvHelper.Configuration;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seo.Model;
using Seo.Views;
using Seo.Views.UserControls;

namespace Seo.ViewModel
{
 public   class MainViewModel:ViewModelBase
    {



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

        private Server _server;

        public Server server
        {
            get { return _server; }
            set { _server = value;RaisePropertyChanged("server"); }
        }
        string serverCon = @"DESKTOP-32GMQSN\SQLEXPRESS";
        public RelayCommand<string> command { get; set; }
        public MainViewModel()
        {
            DashbordUC = new DashbordUC();
            CurrentView = new UserControl();
            CurrentView = DashbordUC;
           // string str = @"Server=DESKTOP-32GMQSN\SQLEXPRESS;User Id=sa;Password=123456;";
           // SqlConnection sqlConnection = new SqlConnection(str);
           // //sqlConnection.Open();

           // var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
           // SqlServerDailog s = new SqlServerDailog();
           //// s.ShowDialog();
           // var entry = config.AppSettings.Settings["connectionString"].Value;
           // if (entry==""||entry==null)
           // {


           // }
           // config.AppSettings.Settings["connectionString"].Value="asdas";
          
           // config.Save(ConfigurationSaveMode.Modified);







            WebDatasList = new ObservableCollection<WebData>();
            LinkssList = new ObservableCollection<Links>();
            WebDatasSelectedData = new WebData();
            LinkssSelectedData = new Links();
            CsvConfiguration csvcon = new CsvConfiguration(CultureInfo.CurrentCulture);
            csvcon.MissingFieldFound = null;
            using (var reader = new StreamReader("tranco_W5W9.csv"))
            using (CsvReader csv = new CsvReader(reader, csvcon))
            {
                int a = 0;
                while (csv.Read())
                {
                    if (a != 0)
                        LinkssList.Add(new Links() { Id = int.Parse(csv[0]), WebsiteLink = csv[1] });
                    a = 1;

                    command = new RelayCommand<string>(PerformAction);
                }
            }
            LinkssSelectedData = LinkssList[LinkIndex];
        }
        private ObservableCollection<WebData> _webDatasList;

        public ObservableCollection<WebData> WebDatasList
        {
            get { return _webDatasList; }
            set { _webDatasList = value;RaisePropertyChanged("WebDatasList"); }
        }
          private WebData _webDatasSelectedData;

        public WebData WebDatasSelectedData
        {
            get { return _webDatasSelectedData; }
            set { _webDatasSelectedData = value;RaisePropertyChanged("WebDatasSelectedData"); }
        }
          private ObservableCollection<Links> _LinkssList;

        public ObservableCollection<Links> LinkssList
        {
            get { return _LinkssList; }
            set { _LinkssList = value;RaisePropertyChanged("LinkssList"); }
        }
          private Links _LinkssSelectedData;

        public Links LinkssSelectedData
        {
            get { return _LinkssSelectedData; }
            set { _LinkssSelectedData = value;RaisePropertyChanged("LinkssSelectedData"); }
        }

        private Visibility _controlVisibility=Visibility.Collapsed;

        public Visibility ControlVisibility
        {
            get { return _controlVisibility; }
            set { _controlVisibility = value; RaisePropertyChanged("ControlVisibility"); }
        }
        public int LinkIndex { get; set; } = 0;
     

        private void PerformAction(string obj)
        {
            switch (obj)
            {
                case "PageLoading":
                    break;
                case "Next":
                    LinkIndex++;
                    LinkssSelectedData = LinkssList[LinkIndex];
                    break;
                case "Bad":
                    LinkIndex++;
                    LinkssSelectedData = LinkssList[LinkIndex];
                    break;
                case "PageLoaded":
                    break;
                default:
                    break;
            }
        }
    }
}
