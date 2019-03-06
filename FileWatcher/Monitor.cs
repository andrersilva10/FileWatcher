using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcher
{
	class Monitor
	{
		private FileSystemWatcher fileSystemWatcher;
		private string directory = @"C:\Users\andre\Downloads";
		private string targetDirectory = @"C:\Users\andre\Downloads\Teste\";

		private Dictionary<String, String> map = new Dictionary<string, string>()
		{
			{ ".txt" ,  @"C:\Users\andre\Downloads\txts"},
			{ ".exe" ,  @"C:\Users\andre\Downloads\exes"},
			{ ".docx" ,  @"C:\Users\andre\Downloads\docs"},
            { ".doc" ,  @"C:\Users\andre\Downloads\docs"},
            { ".pdfs" ,  @"C:\Users\andre\Downloads\pdfs"}
		};
		public Monitor()
		{

			fileSystemWatcher = new FileSystemWatcher(directory);
			fileSystemWatcher.EnableRaisingEvents = true;

			// Instruct the file system watcher to call the FileCreated method
			// when there are files created at the folder.
			fileSystemWatcher.Created += new FileSystemEventHandler(FileCreated);
			//fileSystemWatcher.Changed += new FileSystemEventHandler(FileCreated);
		} 

		private void FileCreated(Object sender, FileSystemEventArgs e)
		{
			if (IsFileLocked(new FileInfo(e.FullPath))) return;
			
			if (MoveFile(e.FullPath))
			{
				Console.WriteLine(@"File " + e.FullPath + " moved!");
            }
            else
            {
                Console.WriteLine("There is a file with same name in this folder");
            }
			//ProcessFile(e.FullPath);
		}
		
		private Boolean MoveFile(string file)
		{
            int tries = 0;
			while (true)
			{
				try
				{
					string target = GetTarget(Path.GetExtension(file));
					string to = target + "\\" + file.Substring(file.LastIndexOf('\\') + 1);
					File.Move(file,to);
					return true;
				}
				catch (Exception err)
				{
                    Console.WriteLine(err.Message);
                    if (err.Message == "Não é possível criar um arquivo já existente.\r\n") return false;
					Thread.Sleep(1000);
				}
			}

		}
		private bool IsFileLocked(FileInfo file)
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
		private string GetTarget(string format)
		{
			if (map.ContainsKey(format))
			{
				return map[format];
			}
			throw new Exception("This format doesn't exist");
			
		}


		private void ProcessFile(String fileName)
		{
			FileStream inputFileStream;
			while (true)
			{
				try
				{
					inputFileStream = new FileStream(fileName,
						FileMode.Open, FileAccess.ReadWrite);
					StreamReader reader = new StreamReader(inputFileStream);
					Console.WriteLine(reader.ReadToEnd());
					// Break out from the endless loop
					break;
				}
				catch (IOException)
				{
					// Sleep for 3 seconds before trying
					Thread.Sleep(3000);
				} // end try
			} // end while(true)
		}
	}
}
