using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace StartParafaitKioskApplication
{
    class Program
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            log.LogMethodEntry();
            log.Debug(args);
            if (args.Length != 1)
            {
                log.Error("Please pass one argument.");
                log.Error("Usage: StartParafaitKioskApplication <Kiosk Application Name> ");
                Environment.Exit(1);
            }
            else
            {
                System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo();
                Thread.Sleep(5000);
                process.FileName = System.IO.Path.GetFileNameWithoutExtension(args[0]);
                process.WorkingDirectory = System.IO.Path.GetDirectoryName(args[0]);
                process.WindowStyle = ProcessWindowStyle.Normal;
                //process.FileName = System.IO.Path.GetFileNameWithoutExtension(@"C:\Windows\system32\notepad.exe");
                //process.WorkingDirectory = System.IO.Path.GetDirectoryName(@"C:\Windows\system32\notepad.exe");
                log.Debug("Process File: " + process.FileName);
                log.Debug("Running Directory: " + process.WorkingDirectory);
                process.CreateNoWindow = true;
                process.Verb = "runas";
                process.UseShellExecute = false;
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(process);
                log.Debug("Process Started");
            }
        }
    }
}
