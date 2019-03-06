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
            //Monitor monitor = new Monitor();
            int id = 0;
            if(args.Length > 0)
            {
                id = Convert.ToInt32(args[0]);
            }
            FolderMonitor monitor = new FolderMonitor(@"C:\Users\andre\Documents\NetBeansProjects\GitHub\dashboards",new Dashboards(id));
            monitor.Start();
			Console.ReadKey();
		}
	}
}
