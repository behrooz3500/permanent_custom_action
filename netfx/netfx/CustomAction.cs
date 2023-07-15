using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace netfx
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult CustomAction1(Session session)
        {
            session.Log("netfx: Begin CustomAction netfx");

            string netfx8Dir = session["NETFX_8_DIR"];
            string netfx10Dir = session["NETFX_10_DIR"];
            string netfx11Dir = session["NETFX_11_DIR"];

            string sourcePath = "";

            session.Log(msg: netfx8Dir);
            session.Log(msg: netfx10Dir);
            session.Log(msg: netfx11Dir);

            

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            string pathName = (string)registryKey.GetValue(name:"productName");
            
            session.Log(msg: $"netfx: {pathName}");


            if (pathName.Contains("8"))
            {
                sourcePath = netfx8Dir;
            }
            else if (pathName.Contains("10"))
            {
                sourcePath = netfx10Dir;
            }
            else if (pathName.Contains("11"))
            {
                sourcePath = netfx11Dir;
            }

            if (sourcePath != null)
            {
                ProcessStartInfo netfxInfo = new ProcessStartInfo();
                netfxInfo.FileName = "dsim.exe";
                netfxInfo.Arguments = "/online /enable-feature /featurename:NetFX3 /All /Source:\"" + sourcePath +
                                      "\" /LimitAccess";
                netfxInfo.Verb = "runas";
                netfxInfo.UseShellExecute = true;
                session.Log(sourcePath);
                session.Log(netfxInfo.ToString());
                try
                {
                    Process.Start(netfxInfo);
                    session.Log(msg: "netfx: command ran...");
                }
                catch (Exception ex)
                {
                    session.Log(ex.Message);
                }

            }
            else
            {
                session.Log("no need for netfx");
            }

            return ActionResult.Success;
        }
    }
}
