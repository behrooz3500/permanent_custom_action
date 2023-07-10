using Microsoft.Deployment.WindowsInstaller;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
                string installDirectory = session["APPDIR"]+"\\MoeinSoft";
                string targetDirectory = installDirectory + "\\moein";
                string launcherDirectory = installDirectory + "\\Launcher";

                string removeLauncherCondition = session["REMOVE_LAUNCHER"].ToLower();

                session.Log($"restoreFiles: Restoring started from {installDirectory}");
                CopyFolder(targetDirectory, Path.Combine(installDirectory));
                session.Log(msg:"restoreFiles: Restoring files finished successfully!");

                Directory.Delete(targetDirectory, true );
                session.Log(msg: "restoreFiles: Temp directory deleted.");

                if (removeLauncherCondition.Contains("yes"))
                {
                    Directory.Delete(launcherDirectory, recursive: true);
                    session.Log(msg: "restoreFiles: Launcher directory deleted.");
                }

                return ActionResult.Success;
            }
            catch (Exception ex)
            {
                session.Log(msg: "restoreFiles: Exception: " + ex.Message);
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
