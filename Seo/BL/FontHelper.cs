using System;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Documents;
using GalaSoft.MvvmLight;

namespace Seo
{
 
  public   class FontHelper:ViewModelBase
    {


        private UserControl _userControl;

        public UserControl userControl
        {
            get { return _userControl; }
            set { _userControl = value; RaisePropertyChanged("userControl"); }
        }


       

        private Window _Window;

        public Window Window
        {
            get { return _Window; }
            set { _Window = value; RaisePropertyChanged("Window"); }
        }

        public FontHelper()
        {
            userControl = new UserControl();
            Window = new Window();
        }



        public  IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null)
                yield return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                if (child != null && child is T)
                    yield return (T)child;

                foreach (T childOfChild in FindVisualChildren<T>(child))
                    yield return childOfChild;
            }
        }

        public  Window  GetResponsive(Window wind)
        {
           
           var TextBlock= FindVisualChildren<TextBlock>(wind);
           var Button= FindVisualChildren<Button>(wind);
           var TextBox= FindVisualChildren<TextBox>(wind);
           var labels= FindVisualChildren<Label>(wind);
         

            foreach (var item in TextBox)
            {
                item.FontSize = GetFont(item.FontSize, wind);
            }
            foreach (var item in labels)
            {
                item.FontSize = GetFont(item.FontSize, wind);
            }
            foreach (var item in TextBlock)
            {
                item.FontSize = GetFont(item.FontSize, wind);
            }
            foreach (var item in Button)
            {
                item.FontSize = GetFont(item.FontSize,wind);
                item.Width = GetFont(item.Width, wind);
                item.Height = GetFont(item.Height, wind);
            }
            return wind;
        }
        public  UserControl GetResponsive(UserControl wind)
        {
            try
            {
                userControl = wind;
                var TextBlock = FindVisualChildren<TextBlock>(userControl);
                var Button = FindVisualChildren<Button>(userControl);
                var TextBox = FindVisualChildren<TextBox>(userControl);
                var mt = FindVisualChildren<PackIcon>(userControl);
                var Pic = FindVisualChildren<Image>(userControl);
                //var runs = FindVisualChildren<Run>(userControl);
                //var templColumn = FindVisualChildren<DataGridTemplateColumn>(userControl);
                //var Datagrid = FindVisualChildren<DataGrid>(wind);

                //foreach (var item in Datagrid)
                //{
                //    foreach (DataGridColumn column in item.Columns)
                //    {

                //        column.Width = GetFont(Convert.ToDouble(column.ActualWidth), wind);
                //    }
                //}
                //foreach (var item in templColumn)
                //{
                //  //  item.Width = (DataGridLength)(GetFont(item.Width, userControl));
                //}

                foreach (var item in mt)
                {
                    item.Width = GetFont(item.Width, userControl);
                    item.Height = GetFont(item.Height, userControl);
                }
                //foreach (var item in runs)
                //{
                //    item.FontSize = GetFont(item.FontSize, userControl);

                //}
                foreach (var item in Pic)
                {
                    item.Width = GetFont(item.Width, userControl);
                    item.Height = GetFont(item.Height, userControl);
                }
                foreach (var item in TextBox)
                {
                    item.FontSize = GetFont(item.FontSize, userControl);
                }
                foreach (var item2 in TextBlock)
                {
                    item2.FontSize = GetFont(item2.FontSize, userControl);
                }
                foreach (var item3 in Button)
                {
                    item3.FontSize = GetFont(item3.FontSize, userControl);
                    item3.Width = GetFont(item3.Width, userControl);
                    item3.Height = GetFont(item3.Height, userControl);
                }
                a = userControl.ActualHeight;
                b = userControl.ActualWidth;
            }
            catch(Exception ex)
            {
               
            }
            return userControl;
        }
        public  double GetFont(double DesiredFontSize,Window w)
        {
            var multiple = w.ActualHeight * w.ActualWidth * DesiredFontSize;
            var devide = 1300 * 700;

            return multiple / devide;
        }
        double a = 0;
        double b = 0;
        public  double GetFont(double DesiredFontSize, UserControl w)
        {
          
            if (a==0&&b==0)
            {
                a = 1300;
                b = 700;
            }
            double multiple = w.ActualHeight* w.ActualWidth * DesiredFontSize;
            double devide = b * a;
            double ans = multiple / devide;
            
            return ans;
        }

    }
}
