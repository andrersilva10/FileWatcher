using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ForLogic.Persistence;
namespace FileWatcher
{
    class Utils
    {
        public static string ConnectionString { get; set; } = @"Data Source=localhost; Initial Catalog=qualiex; Integrated Security=false; User Id=sa; Password=abc123##;";
        public static string DashboardPath {
            get
            {
                using (var sr = new StreamReader("config.json"))
                {
                    var content = sr.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    var dyn = serializer.Deserialize<Dictionary<string, object>>(content);
                    var DashboardsPath = dyn["DashboardsPath"].ToString();
                    return DashboardsPath;
                }
            }
        }
        public static void SetDbParameters()
        {
            using(var sr = new StreamReader("config.json"))
            {
                var content = sr.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                var dyn = serializer.Deserialize<Dictionary<string,object>>(content);
                ConnectionString = dyn["ConnectionString"].ToString();
            }
            Db.ShowConsoleSql = true;
            Db.ReplaceDbInstructions = false;
            Db.SetDbProvider(EDbProvider.SqlServer);
            Db.SetConnectionString(ConnectionString);
        }

    }
}
