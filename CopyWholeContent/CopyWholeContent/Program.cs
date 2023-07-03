using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace CopyWholeContent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string installDirectory = "d:\\temp";
                string targetDirectory = installDirectory+"\\moeintempfile";
                List<string> sourceNames = new List<string>();
                //todo: get the list from a file
                using (StreamReader reader = new StreamReader(Path.Combine(installDirectory, "permanent.txt")))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if(!string.IsNullOrEmpty(line) )
                        {
                            sourceNames.Add(line);
                            Console.WriteLine($"{line} added!");
                        }
                    }
                    Console.ReadKey();
                }


                List<string> sourcedirectories = new List<string>();
                Directory.CreateDirectory(targetDirectory);
                foreach (string sourceName in sourceNames)
                {
                    string sourceFolderPath = Path.Combine(installDirectory, sourceName);
                    if (Path.HasExtension(sourceFolderPath) && File.Exists(sourceFolderPath))
                    {
                        Console.WriteLine($"{sourceFolderPath} has extention!");
                        
                        File.Copy(sourceFolderPath, Path.Combine(targetDirectory, sourceName) , true);
                    }
                    else if (Directory.Exists(sourceFolderPath))
                    {

                        Console.WriteLine($"{sourceFolderPath} is a directory!");
                        CopyFolder(sourceFolderPath, Path.Combine(targetDirectory, sourceName));
                    }
                }

                Console.ReadKey();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
        private static void CopyFolder(string sourceFolder, string destinationFolder)
        {

            DirectoryInfo directorySource = new DirectoryInfo(sourceFolder);
            DirectoryInfo directoryDestination = new DirectoryInfo(destinationFolder);
            CopyAll(directorySource, directoryDestination);

        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
