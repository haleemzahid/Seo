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



        private Filter _filterData;

        public Filter FilterData
        {
            get { return _filterData; }
            set
            {
                _filterData = value;
             //   GetFilteredList(value);
                RaisePropertyChanged("FilterData");
            }
        }
        private Filter _filterData2;

        public Filter FilterData2
        {
            get { return _filterData2; }
            set
            {
                _filterData2 = value;
                //GetFilteredList(value);
                RaisePropertyChanged("FilterData2");
            }
        }

        private void GetFilteredList()
        {
          



            if (FilterData2.AnchorText == null)
                FilterData2.AnchorText = "";
            if (FilterData.Category == null)
                FilterData.Category = "";
            if(FilterData.AnchorText==""&&FilterData.Category!="")
            LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name).Where(x=>x.Catogery == FilterData.Category).ToList());
           else if(FilterData2.AnchorText!=""&&FilterData.Category=="")
            LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name).Where(x=>x.AnchorText== FilterData.AnchorText).ToList());
           else if(FilterData2.AnchorText!=""&&FilterData.Category!="")
            LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name).Where(x => x.AnchorText == FilterData.AnchorText && x.Catogery == FilterData.Category).ToList());
            if (LinkssList.Count <= 0)
                LinkssSelectedData = new Links();
            else
            {
                LinkIndex = 0;
                PerformAction("Next");
            }
        }

        private ObservableCollection<Filter> _filterList;

        public ObservableCollection<Filter> FilterList
        {
            get { return _filterList; }
            set { _filterList = value;RaisePropertyChanged("FilterList"); }
        }
             private ObservableCollection<Filter> _filterList2;

        public ObservableCollection<Filter> FilterList2
        {
            get { return _filterList2; }
            set { _filterList2 = value;RaisePropertyChanged("FilterList2"); }
        }

        private Server _server;

        public Server server
        {
            get { return _server; }
            set { _server = value;RaisePropertyChanged("server"); }
        }
       
        public RelayCommand<string> command { get; set; }
        public DashbordViewModel()
        {
            FilterData2 = new Filter();
            command = new RelayCommand<string>(PerformAction);
            server = new Server();
            ProjectSelectedData = new Project();
            ProjectList = new ObservableCollection<Project>();
            currentWindow = new SqlServerDailog(this);
            if (!Helper.CheckForConnectionString())
                currentWindow.ShowDialog();
            SettingDialog = new SettingDialog();
            LinkssList = new ObservableCollection<Links>();
           
            LinkssSelectedData = new Links();
            FilterList = new ObservableCollection<Filter>();
            FilterList2 = new ObservableCollection<Filter>();
           
            FilterData = new Filter();
            ProjectList = Helper.GetTables();
            if (ProjectList.Count > 0)
            {
                
                ProjectSelectedData = ProjectList.First();

            }

        }

        private void GetFilter()
        {
            var Catli = LinkssList.Select(x => x.Catogery).Distinct().ToList();
            var Anchorli = LinkssList.Select(x => x.AnchorText ).Distinct().ToList();
            foreach (var item in Catli)
            {
                FilterList.Add(new Filter() {Category=item });
            }
                        foreach (var item in Anchorli)
            {
                FilterList2.Add(new Filter() {AnchorText=item});
            }
                      
        }















        private ObservableCollection<Project> _projectsList;

        public ObservableCollection<Project> ProjectList
        {
            get { return _projectsList; }
            set { _projectsList = value; RaisePropertyChanged("ProjectList"); }
        }
        private Project _projectsSelectedData;

        public Project ProjectSelectedData
        {
            get { return _projectsSelectedData; }
            set
            {
                _projectsSelectedData = value;
                RefreshData(value);
                if(value==null)
                value = new Project();
                RaisePropertyChanged("ProjectSelectedData");
            }
        }

        private void RefreshData(Project value)
        {
            if (value != null)
            {

                if (LinkssList == null)
                    LinkssList = new ObservableCollection<Links>();
                if (value.Name != "" && value != null && value.Name != null)
                {

                    LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name));
                    LinkIndex = 0;
                }
                if (LinkssList.Count > 0)
                {
                    
                    LinkssSelectedData = LinkssList[0];
                
                }
                    GetFilter();
            }
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
                    case "FilterSelectionChanged":
                        GetFilteredList();
                        break;
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
                        Helper.UpdateStatus(LinkssSelectedData,true);

                        if (LinkIndex < LinkssList.Count)
                            LinkssSelectedData = LinkssList[LinkIndex];
                        break;
                    case "Bad":
                        var l = LinkssList.ToList();
                        l.RemoveAll(x => x.SourceURL.Contains(LinkssSelectedData.SourceURL.Substring(0, 15)));
                        LinkssList = new ObservableCollection<Links>(l);
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
