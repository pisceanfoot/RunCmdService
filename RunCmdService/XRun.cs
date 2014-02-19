using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace RunCmdService
{
    public class XRun
    {
        private Thread thread;
        private bool bStart;
        private Queue<string> queue;
        private FileSystemWatcher fileSystemWatcher;
        private string logFile = null;

        public void Start()
        {
            queue = new Queue<string>();
            bStart = true;

            thread = new Thread(new ThreadStart(Run));
            thread.Name = "XRun_thomas";
            thread.IsBackground = true;
            thread.Start();

            string path = ConfigurationManager.AppSettings["path"];
            logFile = ConfigurationManager.AppSettings["log"];

            fileSystemWatcher = new FileSystemWatcher();
            fileSystemWatcher.Path = path ?? "d:\\run";
            fileSystemWatcher.Created += new FileSystemEventHandler(fileSystemWatcher_Created);
            fileSystemWatcher.Error += new ErrorEventHandler(fileSystemWatcher_Error);

            fileSystemWatcher.NotifyFilter = NotifyFilters.FileName;
            fileSystemWatcher.EnableRaisingEvents = true;

            WriteLog("Start");
        }

        public void Stop()
        {
            this.bStart = false;
        }

        void fileSystemWatcher_Error(object sender, ErrorEventArgs e)
        {
            WriteLog(e.GetException());
        }

        private void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            lock (queue)
            {
                queue.Enqueue(e.FullPath);
            }
        }

        private void Run()
        {
            while (bStart)
            {
                if (queue.Count > 0)
                {
                    lock (queue)
                    {
                        string fullPath = queue.Dequeue();
                        Thread.Sleep(1000);
                        Excute(fullPath);
                    }

                }

                Thread.Sleep(500);
            }

            fileSystemWatcher.EnableRaisingEvents = false;
            fileSystemWatcher.Dispose();
        }

        private void Excute(string fullPath)
        {
            try
            {
                Process p = new Process();

                ProcessStartInfo startInfo = new ProcessStartInfo(fullPath);
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                p.StartInfo = startInfo;

                p.Start();
                p.WaitForExit();

                WriteLog("done" + fullPath);
            }
            catch (Exception ex)
            {
                WriteLog(ex);
                WriteLog("error" + fullPath);
            }
        }

        private void WriteLog(Exception ex)
        {
            if (logFile != null)
            {
                try
                {
                    lock (logFile)
                    {
                        File.AppendAllText(logFile, DateTime.Now.ToString() + ":\r\n" + ex.ToString() + "\r\n");
                    }
                }
                catch { }
            }
        }

        private void WriteLog(string content)
        {
            if (logFile != null)
            {
                try
                {
                    lock (logFile)
                    {
                        File.AppendAllText(logFile, DateTime.Now.ToString() + ":\r\n" + content + "\r\n");
                    }
                }
                catch { }
            }
        }
    }
}
