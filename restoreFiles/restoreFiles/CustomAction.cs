using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace restoreFiles
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            session.Log("Begin CustomAction1");

            try
            {
                string installDirectory = session["APPDIR"];
                string targetDirectory = installDirectory + "\\moein";

                session.Log($"come here {installDirectory}");
                CopyFolder(targetDirectory, Path.Combine(installDirectory));

                Directory.Delete(targetDirectory, true );
                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log("Exception: " + ex.Message);
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
