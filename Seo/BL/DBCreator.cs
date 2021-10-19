


using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seo
{
  public static  class DBCreator
    {
        public const string pass= "masjid2525";
        public static string DBDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Masjid-e-Anwar-e-Mustafa");
        public static string DBPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Masjid-e-Anwar-e-Mustafa\\Masjid.db");
        public static SQLiteConnection GetSQLiteInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", DBPath)
            );
            db.Open();

            return db;
        }

        public static void CheckDBIfExist()
        {
            if (!Directory.Exists(DBDir))
                Directory.CreateDirectory(DBDir);
            if (!File.Exists(DBPath))
            {
                CreateSQLiteDB();
            }
        }
        public static void CreateSQLiteDB()
        {
            SQLiteConnection.CreateFile(DBPath);
            //Establezco la contraseña
            using (SQLiteConnection conn = new SQLiteConnection(string.Format("Data Source={0};Version=3;", DBPath)))
            {
               // conn.SetPassword(pass);
                conn.Open();
                string PrayerTableQuery = "CREATE TABLE Prayer(ID INTEGER PRIMARY KEY AUTOINCREMENT, SalatTime TEXT,JamaatTime TEXT,EndTime TEXT,PrayerType TEXT,Date TEXT);";
                string DuhaTableQuery = "CREATE TABLE Duha(ID INTEGER PRIMARY KEY AUTOINCREMENT, Duhaa TEXT,StartTime INTEGER,EndTime INTEGER,Date TEXT);";
                string AnnouncmentTableQuery = "CREATE TABLE Announcement(ID INTEGER PRIMARY KEY AUTOINCREMENT, announcement TEXT);";
                 using (var command = new SQLiteCommand(PrayerTableQuery, conn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(DuhaTableQuery, conn))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SQLiteCommand(AnnouncmentTableQuery, conn))
                {
                    command.ExecuteNonQuery();
                }
            }
            string installationDirectory = Directory.GetCurrentDirectory();
         
        }
      }
}
