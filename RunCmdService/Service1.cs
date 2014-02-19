using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RunCmdService
{
    public partial class Service1 : ServiceBase
    {
        private XRun xrun;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            xrun = new XRun();
            xrun.Start();
        }

        protected override void OnStop()
        {
            xrun.Stop();
        }

        public void Test()
        {
            OnStart(null);
        }
    }
}
