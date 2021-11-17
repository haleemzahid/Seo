using Seo.BL;
using Seo.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlockingCollection
{
    public static class DataManager
    {

        public static BlockingCollection<string> data { get; set; }


        public static void Start()
        {
            data = new BlockingCollection<string>();
            Task.Factory.StartNew(() =>
            {
                foreach (string p in data.GetConsumingEnumerable())
                {

                    Helper.ExecuteQuery(p, Helper.GetSqlConnection());
                }

            });
        }

        public static bool UpdateStatus(Links l, bool IsFinalUrl = false)
        {
            l = l == null ? new Links() : l;

            if (data == null) return true;


            try
            {
                var tables = Helper.GetTables();
                tables.Add(new Project() { Name = "tblMaster" });
                foreach (var item in tables)
                {
                    string query = "";
                    
                    if (!IsFinalUrl)
                    {
                        l.URLStatus = "Bad";

                        query = Helper.GetLinkupdateQuery(l, item.Name);
                    }
                    else
                    {
                        query = Helper.UpdateFinalURLQuery(l, item.Name);

                    }
                    data.Add(query);



                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show(ex.Message.ToString());

            }

          
            return true;
        }



    }
}


