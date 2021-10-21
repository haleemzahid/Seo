using Seo.Model;
using Seo.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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

        internal static void DeleteTable(string name)
        {
            var con=GetSqlConnection();
            var query = "Delete table" + name;
            ExecuteQuery(query,con);
        }

        public static List<string> GetLinkInsertQuery(List<Links> _list,string tableName)
        {
            List<string> lStrin = new List<string>();
            try
            {

          
            foreach (var l in _list)
            {

            if (l != null)
                {


            lStrin.Add("insert into "+tableName+" (SourceTitle,AnchorURL,AnchorText,SourceURL,URLStatus,FinalURL,Catogery) values ('"
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
        public static string GetLinkupdateQuery(Links l,string tableName)
        {
            if (l == null)
                l = new Links();
            return "update "+tableName+" set URLStatus='" + l.URLStatus + "' where Guid=" + l.Id;
        }

        #endregion


        public static SqlConnection GetSqlConnection(bool IsDb = true)
        {
            SqlConnection con = new SqlConnection();
            try { 
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var conString = Helper.GetConnectionString(IsDb);
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
            var con = GetSqlConnection();
            string query = "create database MasterDB";
            ExecuteQuery(query,con);
            CreateTable("tblMaster");

        }
        public static void CreateTable(string tableName)
        {
           var con = GetSqlConnection();
            string tableQuery = "Create table "+tableName+ " (Id INTEGER Identity(1,1) PRIMARY KEY,Guid TEXT, SourceTitle TEXT,AnchorText TEXT,SourceURL TEXT,URLStatus NVARCHAR(50),FinalURL TEXT,Category NVARCHAR(50))";

            ExecuteQuery(tableQuery, con);
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
                return Authenticate(GetConnectionString(false));
            }
            
        }

        public static List<Links> GetLinksFromDB(string dbName)
        {
                List<Links> l = new List<Links>();
            try
            {
                string query = "SELECT * FROM "+dbName;
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
        public  static void UpdateStatus(Links l)
        {
            var tables = GetTables();
            foreach (var item in tables)
            {
                var con= GetSqlConnection();
                var query = GetLinkupdateQuery(l,item.Name);
                ExecuteQuery(query,con);



            }
        }
        public static List<Project> GetTables()
        {
            using (SqlConnection connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                DataTable schema = connection.GetSchema("Tables");
                List<Project> ProjectNames = new List<Project>();
                foreach (DataRow row in schema.Rows)
                {
                    ProjectNames.Add(new Project() { Name = (row[2].ToString()) });
                }
                return ProjectNames.Where(x=>x.Name!="tblMaster").ToList();
            }
        }

        public static void TranferDataFromMaster(string tableName)
        {
            var querys = GetLinkInsertQuery(GetLinksFromDB("tblMaster"),tableName);
            var con = GetSqlConnection();
            ExecuteQuery(querys,con);
        }
        public static void CreateNewProject(string name)
        {
            CreateTable(name);
            var dateFromMaster = GetLinksFromDB("tblMaster");
            var quiries = GetLinkInsertQuery(dateFromMaster,"tblMaster");
            var con = GetSqlConnection();
            ExecuteQuery(quiries,con);
            RefreshData();

        }
        public static void RefreshData()
        {
            CommonServiceLocator.ServiceLocator.Current.GetInstance<SettingViewModel>().ProjectList = GetTables();
            CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ProjectList = GetTables();
        }

    }
}
