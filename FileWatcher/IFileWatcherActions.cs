using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    interface IFileWatcherActions
    {
        void OnCreated(object source, FileSystemEventArgs e);
        void OnDeleted(object source, FileSystemEventArgs e);
        void OnChanged(object source, FileSystemEventArgs e);
        void OnRenamed(object source, FileSystemEventArgs e);
    }
}
