using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace keepfiles
{
    public class CustomActions
    {

        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            session.Log("Begin CustomAction1");

            try
            {
                string installDirectory = session["APPDIR"]+"\\MoeinSoft";
                string targetDirectory = installDirectory+"\\moein";
                session.Log($"come here {installDirectory}");

                List<string> sourceNames = new List<string>();

                using(StreamReader reader = new StreamReader(Path.Combine(installDirectory, "permanent.txt")))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (!string.IsNullOrEmpty(line))
                        {
                            sourceNames.Add(line);
                        }
                    }
                }
                List<string> sourcedirectories = new List<string>();
                Directory.CreateDirectory(targetDirectory);
                foreach (string sourceName in sourceNames)
                {
                    string sourceFolderPath = Path.Combine(installDirectory, sourceName);
                    
                    if (Path.HasExtension(sourceFolderPath) && File.Exists(sourceFolderPath))
                    {

                        File.Copy(sourceFolderPath, Path.Combine(targetDirectory, sourceName), true);
                        session.Log($"keepfiles: {sourceFolderPath} file will be kept!");

                    }
                    else if (Directory.Exists(sourceFolderPath))
                    {
                        CopyFolder(sourceFolderPath, Path.Combine(targetDirectory, sourceName));
                        session.Log($"keepfiles: {sourceFolderPath} directory will be kept!");
                    }
                }


                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("keepfiles: Exception: "+ ex.Message);
                return ActionResult.Failure;
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
