using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using static System.Collections.Specialized.BitVector32;
using System.Diagnostics;

namespace netfx_test
{
    internal class Program
    {

        static void Main(string[] args)
        {

            RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");
            string pathName = (string)registryKey.GetValue("productName");
            


            Console.WriteLine(pathName);
            Console.ReadKey();


            string netfx10Dir = @"F:\sources\sxs\";
            string sourcePath = "";



            if (pathName.Contains("8"))
            {
            }
            else if (pathName.Contains("10"))
            {
                Console.WriteLine("here");
                sourcePath = netfx10Dir;
            }
            else if (pathName.Contains("11"))
            {
            }

            if (sourcePath != null)
            {
                ProcessStartInfo netfxInfo = new ProcessStartInfo();
                netfxInfo.FileName = "dism.exe";
                netfxInfo.Arguments = $"/online /enable-feature /featurename:NetFX3 /All /Source:{sourcePath} /LimitAccess";
                netfxInfo.Verb = "runas";
                netfxInfo.UseShellExecute = true;
                try
                {
                    Process.Start(netfxInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else
            {
            }

            Console.ReadKey();

        }
    }
}
