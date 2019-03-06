using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForLogic.Persistence;
namespace FileWatcher
{
    class DbUtils
    {
        public static string ConnectionString { get; set; } = @"Data Source=localhost; Initial Catalog=qualiex; Integrated Security=false; User Id=sa; Password=abc123##;";
        public static void SetDbParameters()
        {
            Db.ShowConsoleSql = true;
            Db.ReplaceDbInstructions = false;
            Db.SetDbProvider(EDbProvider.SqlServer);
            Db.SetConnectionString(ConnectionString);
        }
    }
}
