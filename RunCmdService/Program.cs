using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration.Install;
using System.Collections;
using System.Threading;

namespace RunCmdService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            string command;
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun = new ServiceBase[] { new Service1() };
                ServiceBase.Run(ServicesToRun);
                return;
            }
            else if (args.Length == 1)
            {
                command = args[0].ToLower();
                string serviceName = string.Empty;

                string[] cmdArray = command.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (cmdArray.Length == 2)
                {
                    command = cmdArray[0];
                    serviceName = cmdArray[1];
                }

                TransactedInstaller ti = new TransactedInstaller();
                Installer1 ii = null;
                if (serviceName == string.Empty)
                {
                    ii = new Installer1();
                }
                else
                {
                    ii = new Installer1(serviceName);
                }

                ti.Installers.Add(ii);

                string path = string.Format("/assemblypath={0}", System.Reflection.Assembly.GetExecutingAssembly().Location);
                string[] cmd = { path };
                InstallContext context = new InstallContext(string.Empty, cmd);
                ti.Context = context;

                if (command == "-install" || command == "-i")
                {
                    ti.Install(new Hashtable());
                    return;
                }
                else if (command == "-uninstall" || command == "-u")
                {
                    ti.Uninstall(null);
                    return;
                }
            }
            else
            {
                Service1 service = new Service1();
                service.Test();

                while (true)
                {
                    Thread.Sleep(500);
                }
            }

            Console.Write("Help:\r\n-console: Run in Console mode\r\n-i -install Install as Server\r\n-u -uninstall Uninstall Server\r\n");
        }
    }
}
