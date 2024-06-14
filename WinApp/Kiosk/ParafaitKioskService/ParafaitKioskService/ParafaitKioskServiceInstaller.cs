using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Linq;
using System.Threading.Tasks;

namespace ParafaitKioskService
{
    [RunInstaller(true)]
    public partial class ParafaitKioskServiceInstaller : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        public ParafaitKioskServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller =
                                           new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;

            //# Service Information
            serviceInstaller.DisplayName = "Parafait Kiosk Service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;
            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of ParafaitWCFService.cs
            serviceInstaller.ServiceName = "Parafait Kiosk Service";
            serviceInstaller.Description = "Service to launch the Kiosk  Application, to ensure Kiosk is always running.";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
            this.Committed += new InstallEventHandler(serviceInstaller_Committed);
            this.AfterInstall += new InstallEventHandler(ServiceInstaller_AfterInstall);
        }

        void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                sc.Start();
                
            }
        }

        void serviceInstaller_Committed(object sender, InstallEventArgs e)
        {
            int exitCode;
            using (var process = new System.Diagnostics.Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 86400 reboot= \" Parafait Kiosk is not started. System will reboot in 1 minute. \" actions= restart/60000/restart/60000/reboot/60000", serviceInstaller.ServiceName);

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
                throw new InvalidOperationException();
        }
    }
}
