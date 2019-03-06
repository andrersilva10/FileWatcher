using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcher
{
    class FolderMonitor
    {
        private string path;
        public string Path {
            get { return path; }
            set {
                if (String.IsNullOrEmpty(value)) {
                    throw new Exception("Path can't be empty!");
                }
                path = value;
            }
        }
        public FileSystemWatcher watcher { get; set; }
        public IFileWatcherActions actions { get; set; }
        public FolderMonitor(string path)
        {
            this.Path = path;
        }
        public FolderMonitor(string path,IFileWatcherActions actions):this(path)
        {
            this.actions = actions;
        }
        public void Start()
        {

            // Create a new FileSystemWatcher and set its properties.
            using (watcher = new FileSystemWatcher())
            {
                watcher.Path = path;
                watcher.IncludeSubdirectories = true;
                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.*";

                // Add event handlers.
                watcher.Changed += actions.OnChanged;
                watcher.Created += actions.OnCreated;
                watcher.Deleted += actions.OnDeleted;
                watcher.Renamed += actions.OnRenamed;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            catch (Exception)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
