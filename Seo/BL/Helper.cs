using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.Win32;
using Seo.Model;
using Seo.ViewModel;
using Seo.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

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
                sqlConnection.Close();
                        res = true;
                    }
                    catch (Exception ex)
                    {
                MessageBox.Show(ex.Message.ToString());
                       
                    }

             


        
          return res;
        }
        #region Site Data
        public static bool InsertSiteData(Data l)
        {

         
            try
            {


               
                         l.Description = CheckforNull(l.Description);
                        l.Title = CheckforNull(l.Title);
                        l.URL = CheckforNull(l.URL);
                       

                       string query= ("insert into tblSiteData (URL,Description,Title,ProjectName) values ('"+ l.URL + "','" + l.Description + "','" +  l.Title + "','" +l.ProjectName+ "')");
                ExecuteQuery(query,GetSqlConnection());
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());

                return false;
            }
            return true;
        }
        public static bool DeleteSiteDate(Data d)
        {
            try
            {






                string query = "delete from tblSiteData where Id=" + d.Id;
                ExecuteQuery(query, GetSqlConnection());

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());

            }
            return false;
        }
        #endregion
        #region Links
        public static string GetLinkInsertQuery(Links l)
        {
            if (l == null)
                l = new Links();
            
            l.SourceTitle = CheckforNull(l.SourceTitle);
            l.AnchorURL = CheckforNull(l.AnchorURL);
            l.AnchorText = CheckforNull(l.AnchorText);
            l.SourceURL = CheckforNull(l.SourceURL);
            l.FinalURL = CheckforNull(l.FinalURL);
            l.Catogery = CheckforNull(l.Catogery);
            l.SourceTitle = CheckforNull(l.SourceTitle);
            if (l.Guidstr == "" || l.Guidstr == null)
                l.Guidstr = l.Guid.ToString();
            return "insert into tblMaster (SourceTitle,AnchorURL,AnchorText,SourceURL,URLStatus,FinalURL,Category,Guid) values ('"
                + l.SourceTitle + "','" + l.AnchorURL + "','" + l.AnchorText + "','"
                + l.SourceURL + "','" + l.URLStatus + "','"
                + l.FinalURL + "','" + l.Catogery +"','"+l.Guidstr+ "')";
        }

        internal static void DeleteTable(string name)
        {
            try
            {
                var con = GetSqlConnection();
                var query = "drop table " + name;
                ExecuteQuery(query, con);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
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
                        l.SourceTitle = CheckforNull(l.SourceTitle);
                        l.AnchorURL = CheckforNull(l.AnchorURL);
                        l.AnchorText = CheckforNull(l.AnchorText);
                        l.SourceURL = CheckforNull(l.SourceURL);
                        l.FinalURL = CheckforNull(l.FinalURL);
                        l.Catogery = CheckforNull(l.Catogery);
                        l.SourceTitle = CheckforNull(l.SourceTitle);
                        if (l.Guidstr == "" || l.Guidstr == null)
                            l.Guidstr = l.Guid.ToString();

                        lStrin.Add("insert into "+tableName+ " (SourceTitle,AnchorURL,AnchorText,SourceURL,URLStatus,FinalURL,Category,Guid) values ('"
                + l.SourceTitle + "','" + l.AnchorURL + "','" + l.AnchorText + "','"
                + l.SourceURL +  "','" + l.URLStatus + "','"
                + l.FinalURL + "','" + l.Catogery + "','"+l.Guidstr +"')");
                }
            }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message.ToString());
            }
            return lStrin;
        }
        public static string CheckforNull(string value) 
        {
            if (value==null||value=="")
            {
                value = "Null";
            }
            value = value.Replace("'","");

            return value;
        }
        public static string GetLinkupdateQuery(Links l,string tableName)
        {
            if (l == null)
                l = new Links();
            var a = l.SourceURL.Substring(0, 18);
            return "update "+tableName+" set URLStatus='" + l.URLStatus + "' where SourceURL LIKE '%" + l.SourceURL.Substring(0,25)+"%';";
        }

        internal static List<Data> GetSiteData(string name)
        {
            List<Data> l = new List<Data>();
            try
            {

                string query = "SELECT * FROM tblSiteData where CONVERT(NVARCHAR,ProjectName)='"+name+"'";

                var con = GetSqlConnection();
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (SqlDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {


                            l.Add(new Data() { Id = result.GetInt32(0), Title = (result.GetString(1)), Description = result.GetString(2), URL = result.GetString(3), ProjectName = result.GetString(4) });

                        }
                    }
                    con.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return l;

        }

        public static string UpdateFinalURLQuery(Links l, string tableName)
        {
            if (l == null)
                l = new Links();
            return "update " + tableName + " set FinalURL='" + l.SourceURL + "' where CONVERT(NVARCHAR(MAX), Guid)='" + l.Guidstr + "';";
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
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            return false;
            }
            return true;

        }
        public static bool ExecuteQuery(List<string> query,SqlConnection con)
        {
                string itemsa = "";
            try
            {
                int a = 0;
                using (SqlConnection conn = con)
                {
                    con.Open();
                    foreach (var item in query)
                    {
                        itemsa = item;
                    using (var command = new SqlCommand(item, conn))
                    {
                        command.ExecuteNonQuery();
                            a++;
                    }
                    }
                }
            }

            
            catch (Exception ex)
            {
                var a = itemsa;
                MessageBox.Show(ex.Message.ToString());
                return false;


            }
            return true;

        }
        public static void CreateDB()
        {
            try { 
            var con = GetSqlConnection(false);
            string query = "create database MasterDB";
            ExecuteQuery(query,con);
            CreateTable("tblMaster");
            string tableQuery= "create table tblSiteData (Id INTEGER Identity(1,1) PRIMARY KEY,Title TEXT, Description TEXT,URL TEXT,ProjectName TEXT)";
                ExecuteQuery(tableQuery,GetSqlConnection());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }



        public static void CreateTable(string tableName)
        {
            try { 
           var con = GetSqlConnection();
            string tableQuery = "Create table "+tableName+ " (Id INTEGER Identity(1,1) PRIMARY KEY,Guid TEXT, SourceTitle TEXT,AnchorURL TEXT,AnchorText TEXT,SourceURL TEXT,URLStatus NVARCHAR(50),FinalURL TEXT,Category NVARCHAR(50))";

            if(ExecuteQuery(tableQuery, con))
                {
                    if(tableName!="tblMaster")
                        
                        CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ProjectList.Add(new Project() { Name=tableName});
                        CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().RaisePropertyChanged("ProjectList");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
        public static string conStr = "";
        internal static string GetConnectionString(bool IsDB = true)
        {
            try
            {
                if (IsDB)
                {
                    if(conStr=="")
                        conStr= string.Concat("Server=", GetAppConfigVariable("serverName"), "Database=MasterDB;", GetAppConfigVariable("userNamePassword"));
                    return conStr;
                }
                return string.Concat("Server=", GetAppConfigVariable("serverName"), GetAppConfigVariable("userNamePassword"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            return null;
            }
        
}
        public static string GetAppConfigVariable(string str)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var a = config.AppSettings.Settings[str].Value;
                return a;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return "";

            }
        }

        internal static void SaveConDataToConfig(Server server)
        {
            try { 
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["serverName"].Value = server.SqlServerName + ";";
            config.AppSettings.Settings["userNamePassword"].Value += string.Concat("User Id=", server.UserName, ";", "Password=", server.Password,";");

            config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }

        internal  static bool CheckForConnectionString()
        {
            try
            {
                if (BL.Helper.GetAppConfigVariable("userNamePassword") == string.Empty)
                    return false;
                else
                {
                    return Authenticate(GetConnectionString(false));
                }
            }  
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return false;
            }

}


   

      
        public static List<Links> GetLinksFromDB(string dbName,string Category="",string AnchorText="")
        {
            List<Links> l = new List<Links>();
            try
            {

                string query = "";
                if(Category==""&&AnchorText=="")
                query = "SELECT * FROM " + dbName + " where CONVERT(NVARCHAR,URLStatus)!='Bad'";
              else  if (Category != "" && AnchorText == "")
                    query = "SELECT * FROM " + dbName + " where CONVERT(NVARCHAR,URLStatus)!='Bad' AND CONVERT(NVARCHAR,AnchorText)='"+AnchorText+"'";
                else if (Category == "" && AnchorText != "")
                    query = "SELECT * FROM " + dbName + " where CONVERT(NVARCHAR,URLStatus)!='Bad' AND CONVERT(NVARCHAR,Category)='" + Category + "'";
                else if (Category != "" && AnchorText != "")
                    query = "SELECT * FROM " + dbName + " where CONVERT(NVARCHAR,URLStatus)!='Bad' AND CONVERT(NVARCHAR,Category)='" + Category + "' AND CONVERT(NVARCHAR,AnchorText)='" + AnchorText + "'";

                var con = GetSqlConnection();
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (SqlDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {


                            l.Add(new Links() { Id = result.GetInt32(0), Guidstr = (result.GetString(1)), SourceTitle = result.GetString(2), AnchorURL = result.GetString(3), AnchorText = result.GetString(4), SourceURL = result.GetString(5), URLStatus = result.GetString(6), Catogery = result.GetString(8), FinalURL = result.GetString(7) });

                        }
                    }
                    con.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return l;

        }
        public static List<Project> projects = new List<Project>();
        public static string GetFilePath()
        {

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Excel Workbook|*.xlsx";

            if (dialog.ShowDialog() !=System.Windows.Forms.DialogResult.OK)
            {
                return null;

            }
            return dialog.FileName;

        }


        public static List<Links> ReadExcel(string fileName)
        {


            List<Links> listB = new List<Links>();
            try
            {
                using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
                {
                    string catName = "";
                    int a = 0;
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        if (reader.RowCount > 0)
                        {
                            var dashv = CommonServiceLocator.ServiceLocator.Current.GetInstance<SettingViewModel>();
                            CreateNewProject c = new CreateNewProject(dashv);
                            catName = dashv.ProjectSelectedData.Name = "";
                            dashv.win = new Window();
                            dashv.win = c;
                            c.btnSave.CommandParameter = "Close";
                            dashv.HintText = "Enter category name";
                            c.ShowDialog();
                            catName = dashv.ProjectSelectedData.Name;
                            dashv.HintText = "Enter project name";
                            if (catName == "")
                                catName = "Default";




                        }
                        while (reader.Read())
                        {
                            if (a != 0)


                            {



                                var l = new Links();
                                if (!reader.IsDBNull(3))
                                    l.AnchorText = reader[3].ToString();
                                if (!reader.IsDBNull(2))
                                    l.AnchorURL = reader[2].ToString();
                                if (!reader.IsDBNull(0))
                                    l.SourceURL = reader[0].ToString();
                                if (!reader.IsDBNull(1))
                                    l.SourceTitle = reader[1].ToString();
                                l.Catogery = catName;
                                listB.Add(l);
                            }
                            a++;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            return listB;

        }
        public static List<Links> ReadExcelFile(string fileName)
        {
            return ReadExcel(fileName);

          

         
        }

        public  static void UpdateStatus(Links l,bool IsFinalUrl=false)
        {
            try {
                l.URLStatus = "Bad";
            var tables = GetTables();
                tables.Add(new Project() { Name="tblMaster"});
            foreach (var item in tables)
            {
                    string query = "";
                var con= GetSqlConnection();
                    if(!IsFinalUrl)
                    {

                query = GetLinkupdateQuery(l,item.Name);
                    }
                    else
                    {
                query = UpdateFinalURLQuery(l,item.Name);

                    }
                ExecuteQuery(query,con);



            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
        public static ObservableCollection<Project> GetTables()
        {
            try
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
                    connection.Close();
                    return new ObservableCollection<Project>(ProjectNames.Where(x => x.Name != "tblMaster"&&x.Name!="tblSiteData").ToList());
                }
            }
              
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return new ObservableCollection<Project>();
            }
}

      
        public static void CreateNewProject(string name)
        {
            try
            {
                name = name.Replace(" ","_");
                CreateTable(name);
                var dateFromMaster = GetLinksFromDB("tblMaster");
                var quiries = GetLinkInsertQuery(dateFromMaster, name);
                var con = GetSqlConnection();
                ExecuteQuery(quiries, con);
                RefreshData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }

        }
        public static void RefreshData()
        {
            projects = GetTables().ToList();
            CommonServiceLocator.ServiceLocator.Current.GetInstance<SettingViewModel>().ProjectList = projects;
                   }

    }
}
