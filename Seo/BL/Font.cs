using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Seo
{
   public class Font:ViewModelBase
    {
      

      
        public Font()
        {
            //CurrentView.SizeChanged += new SizeChangedEventHandler(Window_SizeChanged);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           
        }

        public double DesiredFontSize { get; set; }

        

        private double _UIElementFont;
        public double UIElementFont
        {


            get { return _UIElementFont; }
            set { _UIElementFont = value;RaisePropertyChanged("UIElementFont"); }
        }
       





    }
}