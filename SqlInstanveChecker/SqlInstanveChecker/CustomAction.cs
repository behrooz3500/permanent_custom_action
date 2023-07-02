using Microsoft.Deployment.WindowsInstaller;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlInstanveChecker
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            session.Log("Begin CustomAction1");

            string moeinInstance = "Moein";
            int count = InstanceFinder();

            if (count > 0) {
                moeinInstance = $"Moein{count}";
            }

            string sqlInstallCommand = $"/norebootchk /qb SECURITYMODE=SQL DISABLENETWORKPROTOCOLS=0 SAPWD=\"arta0@\" INSTANCENAME=\"{moeinInstance}\" ADDLOCAL=SQL_Engine,SQL_Data_Files,SQL_Replication,Client_Components,Connectivity";
            session["SQL_INSTALL_COMMAND"] = sqlInstallCommand;
            session["SQL_INSTANCE"] = moeinInstance.ToString();



            return ActionResult.Success;
        }

        private static int InstanceFinder()
        {
            RegistryKey sqlInstanceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names");
            int moeinInstances = 0;

            foreach (string subKey in sqlInstanceKey.GetSubKeyNames())
            {
                RegistryKey subRegistryKey = sqlInstanceKey.OpenSubKey(subKey);
                string[] namesList = subRegistryKey.GetValueNames();
                foreach (string name in namesList)
                {
                    if (name.ToLower().Contains("moein"))
                    {
                        moeinInstances++;
                    }
                }
            }
            return moeinInstances; 
        }
    }
}
