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
using System.Windows.Shapes;

namespace Seo.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for CreateNewProject.xaml
    /// </summary>
    public partial class CreateNewProject : Window
    {
        public CreateNewProject(ViewModel.SettingViewModel settingViewModel)
        {
            InitializeComponent();
            this.DataContext = settingViewModel;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}
