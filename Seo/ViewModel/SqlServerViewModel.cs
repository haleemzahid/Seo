using GalaSoft.MvvmLight;
using Seo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seo.ViewModel
{
    class SqlServerViewModel:ViewModelBase
    {
        private Server _server;

        public Server server
        {
            get { return _server; }
            set { _server = value; RaisePropertyChanged("server"); }
        }
    }
}
