using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Seo.BL;
using Seo.Model;
using Seo.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Seo.ViewModel
{
 public   class SettingViewModel:ViewModelBase
    {
        public RelayCommand<string> command { get; set; }
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
            set { _projectsSelectedData = value; RaisePropertyChanged("ProjectsSelectedData"); }
        }
        public SettingViewModel()
        {
            ProjectSelectedData = new Project();
            ProjectList = new List<Project>(Helper.GetTables());
            command = new RelayCommand<string>(PerformAction);
        }
        public Window win;
        private void PerformAction(string obj)
        {
            switch (obj)
            {
                case "Delete":
                    Helper.DeleteTable(ProjectSelectedData.Name);
                    Helper.RefreshData();
                    break;
                case "AddNew":
                    ProjectSelectedData = new Project();
                    win= new Window();
                    win = new CreateNewProject(this);
                    win.ShowDialog();

                    break;
                case "Close":
                    if(win!=null)
                    win.Close();
                    break;  
                case "CloseSetting":
                    CommonServiceLocator.ServiceLocator.Current.GetInstance<MainViewModel>().currentWindow.Close();
                    break;
                case "SaveProject":
                    if(ProjectSelectedData.Name=="")
                    {
                        MessageBox.Show("You must fill in all the fields");
                        return;
                    }
                    else if (ProjectList.Where(x => x.Name == ProjectSelectedData.Name).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Project with the name "+ProjectSelectedData.Name+" already exist.");
                        return;

                    }
                    Helper.CreateNewProject(ProjectSelectedData.Name);
                    Helper.RefreshData();
                    win.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
