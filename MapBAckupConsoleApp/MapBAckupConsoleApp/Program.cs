using System;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace MapBAckupConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
           while(true)
            {
                DateTime date1 = DateTime.Now;
                string sourceDirName = ConfigurationManager.AppSettings.Get("Source");
                string destDirName = ConfigurationManager.AppSettings.Get("Destination");
                DirectoryInfo dirTemp = new DirectoryInfo(destDirName + @"\temp");
                if (dirTemp.Exists)
                {
                    Directory.Delete(destDirName + @"\temp", true);
                }
                DirectoryCopy(sourceDirName, destDirName + @"\temp\", true);

                DirectoryInfo dir = new DirectoryInfo(destDirName + @"\" + date1.ToString("yyyy-MM-dd"));
                if (!dir.Exists)
                {
                    Directory.CreateDirectory(destDirName + @"\" + date1.ToString("yyyy-MM-dd"));
                }


                ZipFile.CreateFromDirectory(destDirName + @"\temp\", destDirName + @"\" + date1.ToString("yyyy-MM-dd") + @"\Map " + date1.ToString("HH-mm_ss") + @".zip", 0, false);
                Directory.Delete(destDirName + @"\temp", true);
                int czas = Int32.Parse(ConfigurationManager.AppSettings.Get("TimeInMinutes"));
                Console.WriteLine($"Oczekiwanie {czas}");
                Thread.Sleep(czas * 60 * 1000);
                
            }
            
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            
            // Get the subdirectories for the specified directory.
            try
            {

            
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Console.WriteLine(
                    "Source directory does not exist or could not be found: " + sourceDirName);
                return;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName );

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);

            }

        }
    }
}
