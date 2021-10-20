using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seo.BL
{
    static public class BusinessLogic
    {
        public static bool Authenticate(string serverName,string userName,string password)
        {
            string str = @"Server="+serverName+";User Id="+userName+";Password="+password+";";
            SqlConnection sqlConnection = new SqlConnection(str);
            sqlConnection.Open();
            return true;
        }
    }
}
