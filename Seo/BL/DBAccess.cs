using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Seo
{
    public static class DBAccess
    {
      #region Announcement
        //public static string GetAnouncementInsertQuery(Anouncements a)
        //{
        //    if (a == null)
        //        a = new Anouncements();
        //    return "insert into Announcement (announcement) values ('" + a.Announcement+ "')";
        //}
        //public static string GetAnouncementupdateQuery(Anouncements a)
        //{
        //    if (a == null)
        //        a = new Anouncements();
        //    return "update Announcement set announcement='" + a.Announcement + "' where ID="+a.ID;
        //}
      
        #endregion


        

        public static bool ExecuteQuery(string query)
        {
            try
            {

                using (SQLiteConnection conn = DBCreator.GetSQLiteInstance())
                {

                    using (var command = new SQLiteCommand(query, conn))
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

        public static bool ExecuteQuery(List<string> query)
        {
            try
            {
                using (SQLiteConnection conn = DBCreator.GetSQLiteInstance())
                {

                    foreach (var item in query)
                    {

                    using (var command = new SQLiteCommand(item, conn))
                    {
                        command.ExecuteNonQuery();
                    }
                    }
                }
            }

            catch (Exception ex) { return false; }
            return true;

        }




    }
}