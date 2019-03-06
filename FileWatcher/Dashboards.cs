using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ForLogic.Persistence;
namespace FileWatcher
{
    class Dashboards : Base,IFileWatcherActions
    {
        public StreamReader Reader { get; set; }

        public Dashboards()
        {
            DbUtils.SetDbParameters();
        }
        public Dashboards(int id)
            :base(id)
        {
            DbUtils.SetDbParameters();
        }
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            if (FolderMonitor.IsFileLocked(new FileInfo(e.FullPath))) return;
            using (Reader = new StreamReader(e.FullPath))
            {
                var content = Reader.ReadToEnd();
                var column = getDbColumnByFileName(e.Name);
                var id = this.Id != 0 ? this.Id : getDashboardIdByFolderName(e.Name);
                new Db().Execute(new Sql($"update dashboards_panels set {column} = ? where id = ? ",content,id));
                Console.WriteLine($"Updated {e.FullPath} at {DateTime.Now}");
            }
            
        }

        public void OnCreated(object source, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnDeleted(object source, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnRenamed(object source, FileSystemEventArgs e)
        {
            throw new NotImplementedException();
        }

        private string getDbColumnByFileName(String fileName)
        {
            var exp = @"^[A-Za-z_]+\\(?<file>(?<name>view|config)\.(?<format>js|sql|html))$";
            var regex = new Regex(exp);
            foreach(Match match in regex.Matches(fileName))
            {
                return match.Groups["name"] + "_" + match.Groups["format"];
            }
            return null;
        }
        private int getDashboardIdByFolderName(String fileName)
        {
            var exp = @"DashboardPanel_(?<dashboardname>[A-Za-z0-9_]+)";
            var regex = new Regex(exp);
            var match = regex.Match(fileName);
             if (match.Success)
            {
                int id = Convert.ToInt32(new Db().ExecuteScalar(new Sql($"select id from dbo.dashboards_panels where name like '%{match.Groups["dashboardname"].Value.Replace('_', ' ')}%' COLLATE Latin1_general_CI_AI")));
                return id;
            }
            return 0;
        }
    }
}
