/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80       18-Sep-2019            Dakshakh raj                Modified : Added logs                           
*********************************************************************************************/
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Semnox.Parafait.Report.Reports
{
    static class Program
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            log.LogMethodEntry(argv);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Boolean duplicateApplicationDoesNotExist = false;
            //Boolean duplicateServerDoesNotExist = false;
            System.Threading.Mutex mutexApp = new System.Threading.Mutex(true, "ParafaitReports", out duplicateApplicationDoesNotExist);
            //System.Threading.Mutex mutexServer = new System.Threading.Mutex(true, "ParafaitReportServer", out duplicateServerDoesNotExist);
            //updateTelerikConfig(); //Telerik config updates are automized during MSI Installation
            try
            {
                if (argv.Length > 0)
                {
                    //if (argv[0] == "B") // background mode
                    //{
                    //    if (!duplicateServerDoesNotExist)
                    //    {
                    //        string message =
                    //            "There is another instance of Parafait Report Server running on this machine." +
                    //            Environment.NewLine +
                    //            "This new instance must close." +
                    //            Environment.NewLine +
                    //            "Only 1 instance of Parafait Report Server can exist on a machine.";
                    //        MessageBox.Show(message, "Parafait Report Server - Duplicate Process");

                    //        Environment.Exit(0);
                    //    }
                    //    Application.Run(new ReportServerLaunch());
                    //}
                    //else 
                    if (argv[0] == "M") // online mode, called from management studio
                    {
                        if (duplicateApplicationDoesNotExist)
                        {
                            Application.Run(new MainMDI("M", argv[1], argv[2], argv[3]));
                        }
                    }
                }
                else // online mode, from command line (needs authentication)
                {
                    if (duplicateApplicationDoesNotExist)
                        Application.Run(new MainMDI("U", "", "", ""));
                }

                log.LogMethodExit();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message); 
            }
        }


        /// <summary>
        /// updateTelerikConfig
        /// </summary>
        static void updateTelerikConfig()
        {
            log.LogMethodEntry();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                foreach (XmlElement element in xmlDoc.DocumentElement.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment))
                {
                    if (element.NodeType != XmlNodeType.Comment)
                    {
                        if (element.Name.Equals("Telerik.Reporting"))
                        {
                            foreach (XmlNode node in element.ChildNodes)
                            {
                                if (node.NodeType != XmlNodeType.Comment)
                                {
                                    if (node.Name.Equals("AssemblyReferences"))
                                    {
                                        foreach (XmlNode childnode in node.ChildNodes)
                                        {
                                            if (childnode.NodeType != XmlNodeType.Comment)
                                            {
                                                if (childnode.Attributes[0].Value.Contains("ReportsBL"))
                                                {
                                                    childnode.Attributes[0].Value = childnode.Attributes[0].Value.Replace("ReportsBL", "Reports");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

                ConfigurationManager.RefreshSection("Telerik.Reporting");
                log.LogMethodExit();
            }

            catch (Exception e)
            {
                log.Error("Exception while reading the web.config file:" + e.Message);
            }
            log.LogMethodExit();
        }
    }
}
