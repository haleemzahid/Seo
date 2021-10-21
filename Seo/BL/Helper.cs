using Seo.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Seo.BL
{
    static public class Helper
    {
        public  static bool Authenticate(string ConnectionString)
        {
            bool res = false;

          
              
                    try
                    {

                    SqlConnection sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
                        res = true;
                    }
                    catch (Exception ex)
                    {

                       
                    }

             


        
          return res;
        }
        #region Links
        public static string GetLinkInsertQuery(Links l)
        {
            if (l == null)
                l = new Links();
            return "insert into tblMaster (SourceTitle,AnchorURL,AnchorText,SourceURL,URLStatus,FinalURL,Catogery) values ('"
                + l.SourceTitle + "','" + l.AnchorURL + "','" + l.AnchorText + "','"
                + l.SourceURL + "','" + l.SourceURL + "','" + l.URLStatus + "','"
                + l.FinalURL + "','" + l.Catogery + "')";
        }
        public static List<string> GetLinkInsertQuery(List<Links> _list)
        {
            List<string> lStrin = new List<string>();
            try
            {

          
            foreach (var l in _list)
            {

            if (l != null)
                {


            lStrin.Add("insert into tblMaster (SourceTitle,AnchorURL,AnchorText,SourceURL,URLStatus,FinalURL,Catogery) values ('"
                + l.SourceTitle + "','" + l.AnchorURL + "','" + l.AnchorText + "','"
                + l.SourceURL + "','" + l.SourceURL + "','" + l.URLStatus + "','"
                + l.FinalURL + "','" + l.Catogery + "')");
                }
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
            return lStrin;
        }
        public static string GetAnouncementupdateQuery(Links l)
        {
            if (l == null)
                l = new Links();
            return "update tblMaster set URLStatus='" + l.URLStatus + "' where Id=" + l.Id;
        }

        #endregion


        public static SqlConnection GetSqlConnection(bool IsDB=true)
        {
            SqlConnection con = new SqlConnection();
            try { 
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var conString = Helper.GetConnectionString(IsDB);
                con = new SqlConnection(conString);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
            return con;
        }
    

        public static bool ExecuteQuery(string query, SqlConnection con)
        {
            try
            {

                using (SqlConnection conn = con)
                {
                    conn.Open();
                    using (var command = new SqlCommand(query, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }



            catch (Exception ex)
            {
                //    CustomMessageBox m = new CustomMessageBox(ex.Message.ToString(), _IsError: true); m.ShowDialog();
                return false;
            }
            return true;

        }

        public static bool ExecuteQuery(List<string> query,SqlConnection con)
        {
            try
            {
                using (SqlConnection conn = con)
                {

                    foreach (var item in query)
                    {

                    using (var command = new SqlCommand(item, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    }
                }
            }

            catch (Exception ex) { return false; }
            return true;

        }

        public static void CreateDB()
        {
            var con = GetSqlConnection(false);
            string query = "create database MasterDB";
            ExecuteQuery(query,con);
            con = GetSqlConnection(true);
            string tableQuery = "Create table tblMaster (Id INTEGER Identity(1,1) PRIMARY KEY, SourceTitle TEXT,AnchorText TEXT,SourceURL TEXT,URLStatus NVARCHAR(50),FinalURL TEXT,Category NVARCHAR(50))";
            
            ExecuteQuery(tableQuery,con);

        }
        internal static string GetConnectionString(bool IsDB=true)
        {
            if(IsDB)
            {

            return string.Concat("Server=",GetAppConfigVariable("serverName"),"Database=MasterDB;", GetAppConfigVariable("userNamePassword"));
            
            }
            return string.Concat("Server=",GetAppConfigVariable("serverName"), GetAppConfigVariable("userNamePassword"));
        }
        public static string GetAppConfigVariable(string str)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var a = config.AppSettings.Settings[str].Value;
            return a;
        }

        internal static void SaveConDataToConfig(Server server)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["serverName"].Value = server.SqlServerName + ";";
            config.AppSettings.Settings["userNamePassword"].Value += string.Concat("User Id=", server.UserName, ";", "Password=", server.Password,";");

            config.Save(ConfigurationSaveMode.Modified);
        }

        internal  static bool CheckForConnectionString()
        {
         
           
            if (BL.Helper.GetAppConfigVariable("userNamePassword") == string.Empty)
                return false;
            else
            {
                return Authenticate(GetConnectionString());
            }
            
        }
        public static List<Links> GetLinksFromDB()
        {
                List<Links> l = new List<Links>();
            try
            {
                string query = "SELECT * FROM Prayer";
                using (var command = new SqlCommand(query, GetSqlConnection()))
                {
                    using (SqlDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {


                            l.Add(new Links() { Id = result.GetInt32(0), SourceTitle = result.GetString(1), AnchorURL = result.GetString(2), AnchorText = result.GetString(3), SourceURL = result.GetString(4), URLStatus = result.GetString(5), Catogery = result.GetString(7), FinalURL = result.GetString(6) });

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
                    return l;

        }

    }
}
