using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace RunCmdService
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
            : this("RunCmdService")
        {
            InitializeComponent();
        }

        public Installer1(string serviceName)
        {
            ServiceProcessInstaller spi = new ServiceProcessInstaller();
            spi.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            spi.Username = null;
            spi.Password = null;

            ServiceInstaller si = new ServiceInstaller();
            si.ServiceName = serviceName;
            si.Description = "RunCmdService For Thomas";
            si.StartType = ServiceStartMode.Automatic;


            this.Installers.Add(spi);
            this.Installers.Add(si);


            InitializeComponent();
        }
    }
}
