using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DeleteOldFiles
{
    // https://stackoverflow.com/questions/273313/randomize-a-listt
    static class NonGeneric
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try 
            {
                string strPath = ConfigurationManager.AppSettings["Path"];
                int i = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfDays"]);

                DirectoryInfo oDirectoryInfo = new DirectoryInfo(strPath);

                List<FileInfo> listFiles = oDirectoryInfo.GetFiles().Where(oFile => (DateTime.Now - oFile.LastWriteTime).TotalDays > i).ToList();

                foreach (FileInfo strFile in listFiles)
                {
                    Console.WriteLine(strFile);
                    File.Delete(strPath + "/" + strFile);
                }

                string strPathRestore = ConfigurationManager.AppSettings["PathRestore"];
                oDirectoryInfo = new DirectoryInfo(strPathRestore);
                listFiles = oDirectoryInfo.GetFiles().ToList();
                listFiles.Shuffle();

                int j = 0;

                foreach (FileInfo strFile in listFiles)
                {
                    File.Copy(strPathRestore + "/" + strFile, strPath + "/" + strFile, true);

                    j++;

                    if (j >= 6) break;
                }
            }
            catch (Exception oException)
            {
                Console.WriteLine(oException.ToString());
                Console.ReadLine();
            }
        }
    }
}
