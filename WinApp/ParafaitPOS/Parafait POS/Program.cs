/*******************************************************************************
 * Project Name - Program.cs
 * Description  - Base form of framework
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.70.2        04-Feb-2020      Nitin Pai     Changed to log unhandled exception
*2.100.0       16-Oct-2020      Amitha Joy    Changed to add new configs for POS redesign
/*******************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static string appGuid = "8C18C320-C7B7-40E4-9940-EC762BC93829";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                UpdateConfigs();
                //Application.Run(new POS());
                //if (checkForDuplicateProcess("Parafait POS - V"))
                //{
                //    string message =
                //                "There is another instance of Parafait POS running on this machine." +
                //                Environment.NewLine +
                //                "This new instance must close." +
                //                Environment.NewLine +
                //                "Only 1 instance of Parafait POS can exist on a machine.";
                //    MessageBox.Show(message, "Parafait POS - Duplicate Process");

                //    Environment.Exit(0);
                //}
                //Application.Run(new POS());


                using (Mutex mutex = new Mutex(false, appGuid))
                {
                    bool isHandled = false;
                    try
                    {
                        try
                        {
                            isHandled = mutex.WaitOne(5000, true);
                            if (isHandled == false)
                            {
                                string message =
                                                "There is another instance of Parafait POS running on this machine." +
                                                Environment.NewLine +
                                                "This new instance must close." +
                                                Environment.NewLine +
                                                "Only 1 instance of Parafait POS can exist on a machine.";
                                MessageBox.Show(message, "Parafait POS - Duplicate Process");
                            }
                            else
                            {
                                Application.Run(new POS());
                            }
                        }
                        catch (AbandonedMutexException)
                        {
                            isHandled = true;
                        }
                    }
                    finally
                    {
                        if (isHandled)
                            mutex.ReleaseMutex();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message); 
                log.Fatal("Unhandled exception in application: " + ex + " : "+ ex.StackTrace);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public static void UpdateConfigs()
        {
            //log.LogMethodEntry();
            bool isRuntimeConfigExist = false;
            string runtimeNewtonsoftConfig = "<runtime>" +
                                           "<assemblyBinding xmlns='urn:schemas-microsoft-com:asm.v1'>" +
                                            "<dependentAssembly>" +
                                            "<assemblyIdentity name='Newtonsoft.Json' publicKeyToken='30ad4fe6b2a6aeed' culture='neutral' />" +
                                            "<bindingRedirect oldVersion='0.0.0.0 - 12.0.0.0' newVersion='12.0.0.0' /> " +
                                            "</dependentAssembly>" +
                                            "</assemblyBinding>" +
                                            "</runtime >";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                foreach (XmlElement element in xmlDoc.DocumentElement.Cast<XmlNode>().Where(n => n.NodeType != XmlNodeType.Comment))
                {
                    if (element.Name == "runtime")
                    {
                        //if (element.InnerXml.Contains("Newtonsoft.Json"))
                        //{
                        //    isRuntimeConfigExist = true;
                        //}
                        if (element.InnerXml.Contains("Newtonsoft.Json"))
                        {
                            bool isNewtonSoftVersionUpdated = false;
                            isRuntimeConfigExist = true;
                            foreach (XmlNode xNode in element.ChildNodes)
                            {
                                if (xNode.HasChildNodes && xNode.Name == "assemblyBinding")
                                {
                                    foreach (XmlNode x1Node in xNode.FirstChild.ChildNodes)
                                    {
                                        if (x1Node.Name == "assemblyIdentity"
                                            && x1Node.Attributes[0].Name == "name"
                                            && x1Node.Attributes[0].Value != "Newtonsoft.Json")
                                            continue;
                                        if (x1Node.Attributes[0].Name == "oldVersion"
                                            && x1Node.Attributes[0].Value == "0.0.0.0 - 10.0.0.0")
                                        {
                                            x1Node.Attributes[0].Value = "0.0.0.0 - 12.0.0.0";
                                            isNewtonSoftVersionUpdated = true;
                                        }
                                        if (x1Node.Attributes[1].Name == "newVersion"
                                           && x1Node.Attributes[1].Value == "10.0.0.0")
                                        {
                                            x1Node.Attributes[1].Value = "12.0.0.0";
                                            isNewtonSoftVersionUpdated = true;
                                        }
                                    }
                                    break;
                                }
                            }
                            if (isNewtonSoftVersionUpdated)
                            {
                                xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                                ConfigurationManager.RefreshSection("runtime");
                            }
                        }
                    }
                }

                //create a new element if runtime config element doesn't exists
                if (!isRuntimeConfigExist)
                {
                    XmlDocumentFragment xfrag = xmlDoc.CreateDocumentFragment();
                    xfrag.InnerXml = runtimeNewtonsoftConfig;
                    xmlDoc.DocumentElement.AppendChild(xfrag);
                }

                if (!(isRuntimeConfigExist))
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("runtime");
                }


                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                if(appSettingsNode == null)
                {
                    appSettingsNode = CreateAppSettingsNode(xmlDoc);
                }
                bool updated = false;
                if (IsAppSettingExists(appSettingsNode, "WEB_API_ORIGIN_KEY") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "WEB_API_ORIGIN_KEY", @"ParafaitPOS");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "WEB_API_URL") == false)
                {
                    string webApiUrl = ParafaitDefaultContainerList.GetParafaitDefault(-1, "WEB_API_URL");
                    if(string.IsNullOrWhiteSpace(webApiUrl))
                    {
                        webApiUrl = @"http://localhost:60875/";
                    }
                    AddToAppSettings(xmlDoc, appSettingsNode, "WEB_API_URL", webApiUrl);
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "EXECUTION_MODE") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "EXECUTION_MODE", @"Local");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "SYSTEM_USER_LOGIN_ID") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "SYSTEM_USER_LOGIN_ID", @"ParafaitPOS");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "SYSTEM_USER_PASSWORD") == false)
                {
                    SystemUserEncryptedPassword semnoxPassword = new SystemUserEncryptedPassword("zKYh1RgsAEsPCIO9p5de9w==");
                    SystemUserEncryptedPassword systemUserEncryptedPassword = new SystemUserEncryptedPassword(semnoxPassword.GetPlainTextPassword("MLR-LT"), Environment.MachineName);
                    AddToAppSettings(xmlDoc, appSettingsNode, "SYSTEM_USER_PASSWORD", systemUserEncryptedPassword.GetEncryptedBase64Password());
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "SITE_ID") == false)
                {
                    List<SiteContainerDTO> siteContainerDTOList = SiteContainerList.GetSiteContainerDTOList();
                    if (siteContainerDTOList != null && siteContainerDTOList.Count == 1)
                    {
                        AddToAppSettings(xmlDoc, appSettingsNode, "SITE_ID", siteContainerDTOList[0].SiteId.ToString());
                        updated = true;
                    }

                }
                if (IsAppSettingExists(appSettingsNode, "InterMediateXml") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "InterMediateXml", @"InterMediateXML.xml");
                    updated = true;
                }                
                if (IsAppSettingExists(appSettingsNode, "CREDITPLUS_THRESHOLD_COUNT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "CREDITPLUS_THRESHOLD_COUNT", "1000");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "SMARTRO_TIMEOUT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "SMARTRO_TIMEOUT", "60");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "CREDITPLUS_THRESHOLD_COUNT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "CREDITPLUS_THRESHOLD_COUNT", "1000");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "MashreqTransactionTimeout") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "MashreqTransactionTimeout", "180000");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "MashreqInitialResponseWaitPeriod") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "MashreqInitialResponseWaitPeriod", "15000");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "AdyenTransactionTimeout") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "AdyenTransactionTimeout", "150000");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "EnableLog") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "EnableLog", "0");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "LOGPATH") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "LOGPATH", "");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "GeideaDeviceStatusWaitPeriod") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "GeideaDeviceStatusWaitPeriod", "4000");
                    updated = true;
                }
                if (updated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                //log.LogMethodExit();
            }
            catch (Exception ex)
            {
                //log.Error(ex);
                //log.LogMethodExit();
                throw new Exception(ex.Message);
            }
        }

        private static XmlNode CreateAppSettingsNode(XmlDocument xmlDoc)
        {
            log.LogMethodEntry(xmlDoc);
            XmlElement appSettingsElement = xmlDoc.CreateElement("appSettings");
            xmlDoc.DocumentElement.AppendChild(appSettingsElement);
            log.LogMethodExit(xmlDoc);
            return appSettingsElement;
        }
        private static bool IsAppSettingExists(XmlNode appSettingsNode, string key)
        {
            log.LogMethodEntry(appSettingsNode, key);
            if (appSettingsNode.SelectSingleNode(@"add[@key='" + key + "']") != null)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private static void AddToAppSettings(XmlDocument xmlDoc, XmlNode appSettingsNode, string key, string value)
        {
            log.LogMethodEntry(xmlDoc, appSettingsNode, key, value);
            XmlElement exeMode = xmlDoc.CreateElement("add");
            exeMode.SetAttribute("key", key);
            exeMode.SetAttribute("value", value);
            appSettingsNode.AppendChild(exeMode);
            log.LogMethodExit();
        }

        static private bool checkForDuplicateProcess(string processName, bool Activate = false)
        {
            Process[] procs;
            procs = Process.GetProcesses();

            int count = 0;
            foreach (Process p in procs)
            {
                if (p.MainWindowTitle.Contains(processName))
                {
                    count++;
                    if (Activate)
                        SetForegroundWindow(p.MainWindowHandle);
                }
            }
            if (count > 0)
                return true;
            else
                return false;
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            log.LogMethodEntry();
            log.Fatal("Unhandled exception occured" + e.Exception);
            log.LogMethodExit();
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ExceptionObject is Exception)
            {
                log.Fatal("Unhandled exception occured" + (e.ExceptionObject as Exception));
            }
            else
            {
                log.Fatal("Unhandled exception occured" + e.ExceptionObject.ToString());
            }
            log.LogMethodExit();
        }
    }
}
