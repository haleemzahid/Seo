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
            LinkssList = new ObservableCollection<Links>();
           
            LinkssSelectedData = new Links();
            ProjectList = Helper.GetTables();
            if (ProjectList.Count > 0)
            {

            ProjectSelectedData = ProjectList.First();
          
            }
       
        }
          
            


            


       

            
            




         
        private List<Project> _projectsList;

        public List<Project> ProjectList
        {
            get { return _projectsList; }
            set { _projectsList = value; RaisePropertyChanged("ProjectList"); }
        }
        private Project _projectsSelectedData;

        public Project ProjectSelectedData
        {
            get { return _projectsSelectedData; }
            set { _projectsSelectedData = value;
                if(value.Name!=""&&value!=null&& value.Name != null)
                    LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name));
                RaisePropertyChanged("ProjectSelectedData"); }
        }
        private SettingDialog  _settingDialog;

        public SettingDialog SettingDialog
        {
            get { return _settingDialog; }
            set { _settingDialog = value; }
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
            try
            {
                switch (obj)
                {

                    case "Authenticate":
                        if (authenticate())
                            currentWindow.Close();

                        break;
                    case "Close":
                        currentWindow.Close();
                        break;

                    case "PageLoading":
                        break;
                    case "Next":
                        LinkIndex++;
                        if (LinkIndex <= LinkssList.Count)
                            LinkssSelectedData = LinkssList[LinkIndex];
                        break;
                    case "Bad":
                        Helper.UpdateStatus(LinkssSelectedData);
                        PerformAction("Next");

                        break;
                    case "PageLoaded":
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public  bool authenticate()
        {
            try
            {
                visibility = Visibility.Visible;
                if (string.IsNullOrEmpty(server.Password) || string.IsNullOrEmpty(server.SqlServerName) || string.IsNullOrEmpty(server.UserName))
                {
                    visibility = Visibility.Collapsed;
                    MessageBox.Show("You must fill in all the fields");
                    return false;
                }

                Helper.SaveConDataToConfig(server);
                var conStr = Helper.GetConnectionString(false);
                var res = Helper.Authenticate(conStr);


                if (!res)
                {

                    visibility = Visibility.Collapsed;
                    MessageBox.Show("Invalid credentials");
                    return false;
                }
                //var GetSqlConnection();
                visibility = Visibility.Collapsed;
                Helper.CreateDB();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return true;
        }
    }
}
