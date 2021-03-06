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
using BlockingCollection;
using CefSharp;
using CefSharp.WinForms;
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
        private ChromiumWebBrowser _cefBrowser;

        public ChromiumWebBrowser CefBrowser
        {
            get { return _cefBrowser ; }
            set { _cefBrowser = value; }
        }


        private ObservableCollection<Data> _DataList;
        public ObservableCollection<Data> DataList
        {
            get { return _DataList = _DataList ?? new ObservableCollection<Data>(); }
            set { _DataList = value; RaisePropertyChanged("DataList"); }
        }
        private Data _DataSelectedData;
        public Data DataSelectedData
        {
            get { return _DataSelectedData = _DataSelectedData ?? new Data(); }
            set { _DataSelectedData = value; RaisePropertyChanged("DataSelectedData"); }
        }

        private Filter _filterData;
        public  Filter FilterData
        {
            get { return _filterData=_filterData??new Filter(); }
            set
            {
                _filterData = value;
             //   GetFilteredList(value);
                RaisePropertyChanged("FilterData");
            }
        }
        private Filter _filterData2;
        public  Filter FilterData2
        {
            get { return _filterData2=_filterData2??new Filter(); }
            set
            {
                _filterData2 = value;
                //GetFilteredList(value);
                RaisePropertyChanged("FilterData2");
            }
        }
        private ObservableCollection<Filter> _filterList;
        public  ObservableCollection<Filter> FilterList
        {
            get { return _filterList=_filterList??new ObservableCollection<Filter>(); }
            set { _filterList = value;RaisePropertyChanged("FilterList"); }
        }
        private ObservableCollection<Filter> _filterList2;
        public  ObservableCollection<Filter> FilterList2
        {
            get { return _filterList2=_filterList2??new ObservableCollection<Filter>(); }
            set { _filterList2 = value;RaisePropertyChanged("FilterList2"); }
        }
        private Server _server;
        public  Server server
        {
            get { return _server=_server??new Server(); }
            set { _server = value;RaisePropertyChanged("server"); }
        }
        public  RelayCommand<string> command { get; set; }
        private ObservableCollection<Project> _projectsList;
        public ObservableCollection<Project> ProjectList
        {
            get { return _projectsList = _projectsList ?? new ObservableCollection<Project>(); }
            set { _projectsList = value; RaisePropertyChanged("ProjectList"); }
        }
        private Project _projectsSelectedData;
        public Project ProjectSelectedData
        {
            get { return _projectsSelectedData = _projectsSelectedData ?? new Project(); }
            set
            {
                _projectsSelectedData = value;
                RefreshData(value);

                //value = new Project();
                RaisePropertyChanged("ProjectSelectedData");
            }
        }
        private SettingDialog _settingDialog;
        public SettingDialog SettingDialog
        {
            get { return _settingDialog = _settingDialog ?? new SettingDialog(); }
            set { _settingDialog = value; }
        }
        private ObservableCollection<Links> _LinkssList;
        public ObservableCollection<Links> LinkssList
        {
            get { return _LinkssList = _LinkssList ?? new ObservableCollection<Links>(); }
            set { _LinkssList = value; RaisePropertyChanged("LinkssList"); }
        }
        private Links _LinkssSelectedData;
        public Links LinkssSelectedData
        {
            get { return _LinkssSelectedData = _LinkssSelectedData ?? new Links(); }
            set { _LinkssSelectedData = value; RaisePropertyChanged("LinkssSelectedData"); }
        }
        private Visibility _controlVisibility = Visibility.Collapsed;
        public Visibility ControlVisibility
        {
            get { return _controlVisibility; }
            set { _controlVisibility = value; RaisePropertyChanged("ControlVisibility"); }
        }
        private Visibility _controlVisibility2 = Visibility.Visible;
        public Visibility ControlVisibility2
        {
            get { return _controlVisibility2; }
            set { _controlVisibility2 = value; RaisePropertyChanged("ControlVisibility2"); }
        }
        public int LinkIndex { get; set; } = 0;
        private Visibility _visibility = Visibility.Collapsed;
        public Visibility visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value

                      ; RaisePropertyChanged("visibility");
            }
        }
        public Window currentWindow { get; set; }

        private string _SourceURL;

        public string SourceURL
        {
            get { return _SourceURL; }
            set { _SourceURL = value; RaisePropertyChanged("SourceURL"); }
        }


        public DashbordViewModel()
        {
            command = new RelayCommand<string>(PerformAction);
            currentWindow = new SqlServerDailog(this);
            if (!Helper.CheckForConnectionString())
                currentWindow.ShowDialog();
            SettingDialog = new SettingDialog();

            ProjectList = Helper.GetTables();
            if (ProjectList.Count > 0)
            {

                ProjectSelectedData = ProjectList.First();
                LinkIndex = -1;
                PerformAction("Next");

            }
         

            //CefBrowser.ShowDevTools();
        }
        private void LoadUrl(string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                CefBrowser.Load(url);
            }
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            CefBrowser.ShowDevTools();
        }


        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RefreshData(Project value)
        {
            if (value != null)
            {

                if (LinkssList == null)
                    LinkssList = new ObservableCollection<Links>();
                if (value.Name != "" && value != null && value.Name != null)
                {
                    DataList = new ObservableCollection<Data>(Helper.GetSiteData(ProjectSelectedData.Name));
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
        private void GetFilteredList()
        {
           

            if (FilterData2.AnchorText == null)
                FilterData2.AnchorText = "";
            if (FilterData.Category == null)
                FilterData.Category = "";
            if (FilterData2.AnchorText == "" && FilterData.Category != "")
                LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name,Category:FilterData.Category));
            else if (FilterData2.AnchorText == "" && FilterData.Category == "")
                LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name));
            else if (FilterData2.AnchorText != "" && FilterData.Category == "")
                LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name,AnchorText: FilterData2.AnchorText));
            else if (FilterData2.AnchorText != "" && FilterData.Category != "")
                LinkssList = new ObservableCollection<Links>(Helper.GetLinksFromDB(ProjectSelectedData.Name,FilterData.Category,FilterData2.AnchorText));
            if (LinkssList.Count <= 0)
                LinkssSelectedData = new Links();
            else
            {
                LinkIndex = -1;

                PerformAction("Next");
            }
        }
        private void GetFilter()
        {
            FilterList = new ObservableCollection<Filter>();
            FilterList2 = new ObservableCollection<Filter>();
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
            if (FilterList.Count>0)
            {
                FilterData = FilterList[0];
            }
            if (FilterList2.Count > 0)
            {
                FilterData2 = FilterList[0];
            }

        }
        public void PerformAction(string obj)
        {
            try
            {
                switch (obj)
                {
                    case "TitleCopy":
                        Clipboard.SetText(DataSelectedData.Title);
                        break;
                    case "URLCopy":
                        Clipboard.SetText(DataSelectedData.URL);
                        break;
                    case "DescriptionCopy":
                        Clipboard.SetText(DataSelectedData.Description);
                        break;
                    case "Delete":
                        Helper.DeleteSiteDate(DataSelectedData);
                        DataList.Remove(DataSelectedData);
                        break;

                    case "SiteDataInsert":
                        DataList.Insert(0,new Data());
                        break;
                    case "SiteDataSave":
                        var data = DataList.Where(x => x.Id == 0).ToList();
                        foreach (var item in data)
                        {
                            item.ProjectName = ProjectSelectedData.Name;
                            Helper.InsertSiteData(item);
                            DataList = new ObservableCollection<Data>(Helper.GetSiteData(ProjectSelectedData.Name));


                        }

                        break;

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
                        DataManager.UpdateStatus(LinkssSelectedData,true);

                        if (LinkIndex < LinkssList.Count)
                            LinkssSelectedData = LinkssList[LinkIndex];
                        else 
                        {
                          
                            MessageBox.Show("Reached to the end!");
                        }
                        SourceURL = LinkssSelectedData.SourceURL;
                        break;
                    case "Bad":
                        if (LinkssList.Count <= 0)
                            return;
                        var l = LinkssList.ToList();
                        l.RemoveAll(x => x.SourceURL.Contains(LinkssSelectedData.SourceURL.Substring(0, 15)));
                        LinkssList = new ObservableCollection<Links>(l);
                        DataManager.UpdateStatus(LinkssSelectedData);
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
            //    MessageBox.Show(ex.Message.ToString());
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
             //   MessageBox.Show(ex.Message.ToString());
            }
            return true;
        }
    }
}
