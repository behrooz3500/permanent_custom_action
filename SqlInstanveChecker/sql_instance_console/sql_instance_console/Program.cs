using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;


namespace sql_instance_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RegistryKey sqlInstanceKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names");

            foreach (string subKey in sqlInstanceKey.GetSubKeyNames())
            {
                RegistryKey subRegistryKey = sqlInstanceKey.OpenSubKey(subKey);
                foreach (string name in subRegistryKey.GetValueNames())
                {
                    Console.WriteLine("Sql instance name:" + name);
                }

                string[] namesList = subRegistryKey.GetValueNames();
                int moeinInstances = 0;
                foreach (string name in namesList)
                {
                    if (name.ToLower().Contains("moein"))
                    {
                        moeinInstances++;
                    }
                }
                if (moeinInstances < 2)
                {
                    Console.WriteLine($"{moeinInstances} instance of moein is installed!");
                }
                else
                {
                    Console.WriteLine($"{moeinInstances} instances of moein are installed!");

                }

                if (moeinInstances == 0)
                {
                    Console.WriteLine($"The new instance should be names MOEIN!");
                }
                else
                {

                    Console.WriteLine($"The new instance should be names MOEIN{moeinInstances}!");
                }
            }

            Console.ReadKey();
        }

    }
}
