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
using Seo.BL;
using Seo.Model;
using Seo.Views;
using Seo.Views.Dialogs;
using Seo.Views.UserControls;

namespace Seo.ViewModel
{
 public   class DashbordViewModel:ViewModelBase
    {



      
        private Server _server;

        public Server server
        {
            get { return _server; }
            set { _server = value;RaisePropertyChanged("server"); }
        }
       
        public RelayCommand<string> command { get; set; }
        public DashbordViewModel()
        {
            command = new RelayCommand<string>(PerformAction);
            server = new Server();

            ProjectSelectedData = new Project();
            ProjectList = new List<Project>();
            currentWindow = new SqlServerDailog(this);
            if (!Helper.CheckForConnectionString())
                currentWindow.ShowDialog();
            SettingDialog = new SettingDialog();
            WebDatasList = new ObservableCollection<WebData>();
            LinkssList = new ObservableCollection<Links>();
            WebDatasSelectedData = new WebData();
            LinkssSelectedData = new Links();
            ProjectList = Helper.GetTables();
       
          
            


            


       

            
            




            //CsvConfiguration csvcon = new CsvConfiguration(CultureInfo.CurrentCulture);
            //csvcon.MissingFieldFound = null;
            //using (var reader = new StreamReader("tranco_W5W9.csv"))
            //using (CsvReader csv = new CsvReader(reader, csvcon))
            //{
            //    int a = 0;
            //    while (csv.Read())
            //    {
            //        if (a != 0)
            //            LinkssList.Add(new Links() { Id = int.Parse(csv[0]), WebsiteLink = csv[1] });
            //        a = 1;

            //    }
            
            //LinkssSelectedData = LinkssList[LinkIndex];
        }
        private List<Project> _projectsList;

        public List<Project> ProjectList
        {
            get { return _projectsList; }
            set { _projectsList = value; RaisePropertyChanged("ProjectsList"); }
        }
        private Project _projectsSelectedData;

        public Project ProjectSelectedData
        {
            get { return _projectsSelectedData; }
            set { _projectsSelectedData = value; RaisePropertyChanged("ProjectsSelectedData"); }
        }
        private SettingDialog  _settingDialog;

        public SettingDialog SettingDialog
        {
            get { return _settingDialog; }
            set { _settingDialog = value; }
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
        private Visibility _visibility=Visibility.Collapsed;

        public Visibility visibility
        {
            get { return _visibility; }
            set {
                _visibility=value

                      ; RaisePropertyChanged("visibility"); }
        }
      

        public Window currentWindow { get; set; }
        public void PerformAction(string obj)
        {
            switch (obj)
            {
                case "Setting":
                    currentWindow = new Window();
                    currentWindow = SettingDialog;
                    currentWindow.ShowDialog();
                    break;
                case "Authenticate":
                    if (authenticate())
                        currentWindow.Close();
                    
                    break;

                case "PageLoading":
                    break;
                case "Next":
                    LinkIndex++;
                    LinkssSelectedData = LinkssList[LinkIndex];
                    break;
                case "Bad":
                    LinkIndex++;
                    LinkssSelectedData.URLStatus = "Bad";
                    Helper.UpdateStatus(LinkssSelectedData);
                    LinkssSelectedData = LinkssList[LinkIndex];
                    break;
                case "PageLoaded":
                    break;
                default:
                    break;
            }
        }
        public  bool authenticate()
        {
            visibility = Visibility.Visible;
            if (string.IsNullOrEmpty(server.Password )||string.IsNullOrEmpty(server.SqlServerName) || string.IsNullOrEmpty(server.UserName))
            {
                visibility = Visibility.Collapsed;
                MessageBox.Show("You must fill in all the fields");
                return false;
            }

            Helper.SaveConDataToConfig(server);
            var conStr = Helper.GetConnectionString(false);
           var res= Helper.Authenticate(conStr);


            if (!res)
            {

                visibility = Visibility.Collapsed;
                MessageBox.Show("Invalid credentials");
                return false;
            }
            //var GetSqlConnection();
            visibility = Visibility.Collapsed;
            Helper.CreateDB();
            return true;
        }
    }
}
