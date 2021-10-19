using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using CsvHelper.Configuration;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seo.Model;

namespace Seo.ViewModel
{
 public   class MainViewModel:ViewModelBase
    {
        public RelayCommand<string> command { get; set; }
        public MainViewModel()
        {
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
