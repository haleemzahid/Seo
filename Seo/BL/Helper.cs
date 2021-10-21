using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.Win32;
using Seo.Model;
using Seo.ViewModel;
using System;
using System.Collections.Generic;
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
            return "update "+tableName+" set URLStatus='" + l.URLStatus + "' where CONVERT(NVARCHAR(MAX), Guid)='" + l.Guidstr+"';";
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

            ExecuteQuery(tableQuery, con);
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

        public static List<Links> GetLinksFromDB(string dbName)
        {
                List<Links> l = new List<Links>();
            try
            {
                string query = "SELECT * FROM "+dbName+ " where CONVERT(NVARCHAR,URLStatus)!='Bad'";
                var con = GetSqlConnection();
                con.Open();
                using (var command = new SqlCommand(query, con))
                {
                    using (SqlDataReader result = command.ExecuteReader())
                    {
                        while (result.Read())
                        {


                            l.Add(new Links() { Id = result.GetInt32(0),Guidstr= (result.GetString(1)), SourceTitle = result.GetString(2), AnchorURL = result.GetString(3), AnchorText = result.GetString(4), SourceURL = result.GetString(5), URLStatus = result.GetString(6), Catogery = result.GetString(8), FinalURL = result.GetString(7) });

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
        public static List<Project> projects = GetTables();
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


        public static List<Links> ReadCSV(string fileName)
        {


            List<Links> listB = new List<Links>();
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                int a = 0;
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    
                    while (reader.Read())
                    {
                        if (a != 0)
                            
                       
                        {

                        var l = new Links();
                        if(!reader.IsDBNull(3))
                        l.AnchorText = reader[3].ToString();
                        if(!reader.IsDBNull(2))
                        l.AnchorURL = reader[2].ToString();
                        if(!reader.IsDBNull(0))
                        l.SourceURL = reader[0].ToString();
                        if(!reader.IsDBNull(1))
                        l.SourceTitle = reader[1].ToString();
                        listB.Add(l);
                        }
                        a++;

                    }
                }
            }
            //try
            //{
            //    using (var reader = new StreamReader(fileName))
            //    {
            //        int a = 0;
            //        while (!reader.EndOfStream)
            //        {
            //            if (a != 0)
            //            {

            //                var line = reader.ReadLine();
            //                var values = line.Split(',');
            //                var l = new Links();
            //                l.AnchorText = values[3];
            //                l.AnchorURL = values[2];
            //                l.SourceURL = values[0];
            //                l.SourceTitle = values[1];
            //                listB.Add(l);
            //            }
            //            else
            //            {

            //                a++;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //}
            return listB;

        }
        public static List<Links> GetListOfLinksFromCSVFile(string fileName)
        {
            return ReadCSV(fileName);

            //foreach (var item in list)
            //{

            //}
            //List<Links> l = new List<Links>();
            //try { 
            //CsvConfiguration csvcon = new CsvConfiguration(CultureInfo.CurrentCulture);
            //using (var reader = new StreamReader(fileName))
            //using (CsvReader csv = new CsvReader(reader, csvcon))
            //{
            //    int a = 0;
            //    while (csv.Read())
            //    {
            //        if (a != 0)
            //            l.Add(new Links() { SourceTitle = csv.GetField(1), AnchorURL = csv.GetField(2), AnchorText = csv.GetField(3), SourceURL = csv.GetField(0) });
            //        a = 1;

            //        //var s = Date.Now;csv[4]
            //        //var a =csv.GetRecords<Prayer>();
            //    }


         
        }

        public  static void UpdateStatus(Links l)
        {
            try {
                l.URLStatus = "Bad";
            var tables = GetTables();
                tables.Add(new Project() { Name="tblMaster"});
            foreach (var item in tables)
            {
                var con= GetSqlConnection();
                var query = GetLinkupdateQuery(l,item.Name);
                ExecuteQuery(query,con);



            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());

            }
        }
        public static List<Project> GetTables()
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
                    return ProjectNames.Where(x => x.Name != "tblMaster").ToList();
                }
            }
              
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return new List<Project>();
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
            projects = GetTables();
            CommonServiceLocator.ServiceLocator.Current.GetInstance<SettingViewModel>().ProjectList = projects;
            CommonServiceLocator.ServiceLocator.Current.GetInstance<DashbordViewModel>().ProjectList= projects;
                   }

    }
}
