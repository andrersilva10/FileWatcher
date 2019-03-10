using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
	class Program
	{
		static void Main(string[] args)
		{
            var id = 0;
            if(args.Length > 0)
            {
                id = Convert.ToInt32(args[0]);
            }
            var monitor = new FolderMonitor(Utils.DashboardPath,new Dashboards(id));
            monitor.Start();
			Console.ReadKey();
		}
	}
}
