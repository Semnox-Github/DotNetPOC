using log4net.Config;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;

namespace Semnox.CommonAPI.Games
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            // This should be enabled if you want to catch first chance exceptions
            //AppDomain.CurrentDomain.FirstChanceException += (object source, FirstChanceExceptionEventArgs efc) =>
            //{
            //    try
            //    {
            //        if (efc.Exception != null)
            //        {
            //            switch (efc.Exception.GetType().ToString())
            //            {
            //             case "System.UnauthorizedAccessException":
            //             case "System.InvalidOperationException":
            //                    break;
            //             default:
            //                {
            //                    if (!efc.Exception.ToString().Contains("ApplicationInsights.Extensibility.PerfCounterCollector") &&
            //                        !efc.Exception.ToString().Contains("Access to the registry key 'Global' is denied") &&
            //                        !efc.Exception.ToString().Contains("Request is not available in this context"))
            //                    {
            //                        log.Debug("First chance exception caught");
            //                        log.Debug("Source " + source.ToString());
            //                        log.Debug("Exception " + efc.Exception.ToString());
            //                    }
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //};

            // Setting the GlobalJSONFormatter to replace
            // In CustomerDTO, the contacts are sent as ContactsDTO list in both CustomerDTO and ProfileDTO. Without the below change, the contacts are duplicated and appended
            // https://www.newtonsoft.com/json/help/html/DeserializeObjectCreationHandling.htm 
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings = new JsonSerializerSettings
            {
                ObjectCreationHandling = ObjectCreationHandling.Replace
            };

            XmlConfigurator.Configure();
            log.LogMethodEntry("Application_Start method() Start");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //Setting POSMachineId and POSMachineName to the HttpContext application level - modified on 26-Aug-2019
            Utilities utilities = new Utilities();
            ParafaitEnv parafaitEnv = utilities.ParafaitEnv;
            string ipAddress = string.Empty;

            int numberOfAttempts = 0;
            do
            {
                try
                {
                    log.Debug("Application Pool start attempt " + numberOfAttempts);
                    ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
                    log.Debug(ipAddress);
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        parafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
                        HttpContext.Current.Application["POSMachineId"] = parafaitEnv.POSMachineId;
                        HttpContext.Current.Application["POSMachineName"] = parafaitEnv.POSMachine;
                        log.LogMethodExit(parafaitEnv);
                    }
                    else
                    {
                        log.Error("IpAddress not found in Application_Start()");
                    }
                    CreateMissingAppSettings();
                    PreLoadContainers();

                    // Set the IsCorporate flag based on site container
                    HttpContext.Current.Application["IsCorporate"] = SiteViewContainerList.IsCorporate().ToString();
                    numberOfAttempts = 10;
                }
                catch (Exception ex)
                {
                    log.Fatal("Application Pool start attempt " + numberOfAttempts + " failed");
                    log.Fatal(ex.Message);
                    ++numberOfAttempts;
                    System.Threading.Thread.Sleep(5000);
                    // Start has failed for 10 times, do not start, retry
                    if (numberOfAttempts > 9)
                    {
                        log.Fatal("Application start failed, propagating error to IIS to trigger restart.");
                        throw;
                    }
                }
            } while (numberOfAttempts < 10);
            log.LogMethodExit("Application_Start method() Exit");
        }

        private void AppDomainUnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.Error("Unhandled exception caught");
                if (e != null && e.ExceptionObject != null)
                {
                    log.Error("Unhandled exception caught in global.asax");
                    log.Error("Unhandled exception caught " + sender.ToString());
                    log.Error("Unhandled exception caught " + e.ExceptionObject.ToString());
                }
            }
            catch (Exception ex)
            {

            }
            log.LogMethodExit();
        }

        private void PreLoadContainers()
        {
            log.LogMethodEntry();
            bool preLoadContainers = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PRE_LOAD_CONTAINERS"]) || ConfigurationManager.AppSettings["PRE_LOAD_CONTAINERS"].ToLower() != "false";
            if (SiteViewContainerList.IsCorporate() == false && preLoadContainers)
            {
                LanguageViewContainerList.Rebuild(-1);
                LocationTypeViewContainerList.Rebuild(-1);
                LocationViewContainerList.Rebuild(-1);
                MembershipViewContainerList.Rebuild(-1);
                MessageViewContainerList.Rebuild(-1, -1);
                MifareKeyViewContainerList.Rebuild(-1);
                if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Local")
                {
                    ParafaitDefaultContainerList.Rebuild(-1);
                }
                POSMachineViewContainerList.Rebuild(-1);
                ProductViewContainerList.Rebuild(-1, ManualProductType.SELLABLE.ToString());
                ProductViewContainerList.Rebuild(-1, ManualProductType.REDEEMABLE.ToString());
                RedemptionCurrencyRuleViewContainerList.Rebuild(-1);
                RedemptionCurrencyViewContainerList.Rebuild(-1);
                RedemptionPriceViewContainerList.Rebuild(-1);
                SystemOptionViewContainerList.Rebuild(-1);
                TicketStationViewContainerList.Rebuild(-1);
                UserRoleViewContainerList.Rebuild(-1);
                UserViewContainerList.Rebuild(-1);
            }
            else if (SiteViewContainerList.IsCorporate())
            {
                foreach (var siteContainerDTO in SiteViewContainerList.GetSiteContainerDTOList())
                {
                    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Local" &&
                        (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["PRE_LOAD_CONTAINERS_SITES"]) ||
                         ConfigurationManager.AppSettings["PRE_LOAD_CONTAINERS_SITES"].Contains(siteContainerDTO.SiteId.ToString()) == false))
                    {
                        continue;
                    }
                    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote" && ConfigurationManager.AppSettings["SITE_ID"] != siteContainerDTO.SiteId.ToString())
                    {
                        continue;
                    }
                    LanguageViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    LocationTypeViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    LocationViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    MembershipViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    MessageViewContainerList.Rebuild(siteContainerDTO.SiteId, -1);
                    MifareKeyViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Local")
                    {
                        ParafaitDefaultContainerList.Rebuild(siteContainerDTO.SiteId);
                    }
                    POSMachineViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    ProductViewContainerList.Rebuild(siteContainerDTO.SiteId, ManualProductType.SELLABLE.ToString());
                    ProductViewContainerList.Rebuild(siteContainerDTO.SiteId, ManualProductType.REDEEMABLE.ToString());
                    RedemptionCurrencyRuleViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    RedemptionCurrencyViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    RedemptionPriceViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    SystemOptionViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    TicketStationViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    UserRoleViewContainerList.Rebuild(siteContainerDTO.SiteId);
                    UserViewContainerList.Rebuild(siteContainerDTO.SiteId);
                }
            }
            log.LogMethodExit();
        }

        private void CreateMissingAppSettings()
        {
            log.LogMethodEntry();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                XmlNode appSettingsNode = xmlDoc.SelectSingleNode("/configuration/appSettings");
                if (appSettingsNode == null)
                {
                    appSettingsNode = CreateAppSettingsNode(xmlDoc);
                }
                bool updated = false;
                if (IsAppSettingExists(appSettingsNode, "EXECUTION_MODE") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "EXECUTION_MODE", @"Local");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "WEB_API_ORIGIN_KEY") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "WEB_API_ORIGIN_KEY", @"ParafaitPOS");
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
                if (IsAppSettingExists(appSettingsNode, "ENABLE_TOKEN_SITE_TRX_ENFORCEMENT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "ENABLE_TOKEN_SITE_TRX_ENFORCEMENT", "Y");
                    updated = true;
                }
                if (IsAppSettingExists(appSettingsNode, "ENABLE_VIRTUAL_SITE_TRX_ENFORCEMENT") == false)
                {
                    AddToAppSettings(xmlDoc, appSettingsNode, "ENABLE_VIRTUAL_SITE_TRX_ENFORCEMENT", "Y");
                    updated = true;
                }
                if (updated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                bool webServerUpdated = false;
                XmlNode systemWebServerNode = xmlDoc.SelectSingleNode("/configuration/system.webServer");
                if (systemWebServerNode == null)
                {
                    systemWebServerNode = CreateNode(xmlDoc, "system.webServer");
                }

                XmlNode staticContentNode = xmlDoc.SelectSingleNode("/configuration/system.webServer/staticContent");
                if (staticContentNode == null)
                {
                    staticContentNode = CreateSubNode(xmlDoc, systemWebServerNode, "staticContent");
                }
                if (IsStaticModulesExists(staticContentNode, "remove", ".apk") == false)
                {
                    AddToModuleSettings(xmlDoc, staticContentNode, "remove", "fileExtension", @".apk", string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    webServerUpdated = true;
                }
                if (IsStaticModulesExists(staticContentNode, "mimeMap", ".apk") == false)
                {
                    AddToModuleSettings(xmlDoc, staticContentNode, "mimeMap", "fileExtension", @".apk", "mimeType", @"application/vnd.android.package-archive", string.Empty,
                        string.Empty, string.Empty, string.Empty);
                    webServerUpdated = true;
                }

                XmlNode modulesNode = xmlDoc.SelectSingleNode("/configuration/system.webServer/modules");
                if (modulesNode == null)
                {
                    modulesNode = CreateModulesNode(xmlDoc, systemWebServerNode);
                }
                if (IsModulesExists(modulesNode, "add", "FileResourceModule") == false)
                {
                    AddToModuleSettings(xmlDoc, modulesNode, "add", "name", "FileResourceModule", "type", @"Semnox.Parafait.ViewContainer.FileResourceHttpModule, ViewContainer",
                          string.Empty, string.Empty, string.Empty, string.Empty);
                    webServerUpdated = true;
                }
                XmlNode securityNode = xmlDoc.SelectSingleNode("/configuration/system.webServer/security");
                if (securityNode == null)
                {
                    securityNode = CreateNode(xmlDoc, "security");
                }
                XmlNode requestFilteringNode = xmlDoc.SelectSingleNode("/configuration/system.webServer/security/requestFiltering");
                if (requestFilteringNode == null)
                {
                    requestFilteringNode = CreateSubNode(xmlDoc, securityNode, "requestFiltering");
                }
                XmlNode requestLimitsNode = xmlDoc.SelectSingleNode("/configuration/system.webServer/security/requestFiltering/requestLimits");
                if (requestLimitsNode == null)
                {
                    requestLimitsNode = CreateRequestLimitsNode(xmlDoc, requestFilteringNode);
                    webServerUpdated = true;
                }
                if (webServerUpdated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("systemWebServerNode");
                }
                bool webConfigUpdated = false;
                XmlNode configSectionsNode = xmlDoc.SelectSingleNode("/configuration/configSections");
                if (configSectionsNode == null)
                {
                    configSectionsNode = CreateNode(xmlDoc, "configSections");
                }
                if (IsModulesExists(configSectionsNode, "section", "Telerik.Reporting") == false)
                {
                    AddToModuleSettings(xmlDoc, configSectionsNode, "section", "name", "Telerik.Reporting", "type", @"Telerik.Reporting.Configuration.ReportingConfigurationSection, Telerik.Reporting, Version=14.1.20.513, Culture=neutral, PublicKeyToken=a9d7983dfcc261be",
                                "allowLocation", "true", "allowDefinition", "Everywhere");
                    webConfigUpdated = true;
                }

                XmlNode configurationNode = xmlDoc.SelectSingleNode("/configuration");
                if (configurationNode == null)
                {
                    configurationNode = CreateNode(xmlDoc, "configuration");
                }
                XmlNode requestTelerikNode = xmlDoc.SelectSingleNode("/configuration/Telerik.Reporting");
                if (requestTelerikNode == null)
                {
                    requestTelerikNode = CreateSubNode(xmlDoc, configurationNode, "Telerik.Reporting");
                }
                XmlNode assemblyReferencesNode = xmlDoc.SelectSingleNode("/configuration/Telerik.Reporting/AssemblyReferences");
                if (assemblyReferencesNode == null)
                {
                    assemblyReferencesNode = CreateSubNode(xmlDoc, requestTelerikNode, "AssemblyReferences");
                }
                if (IsModulesExists(assemblyReferencesNode, "add", "Reports") == false)
                {
                    AddToModuleSettings(xmlDoc, assemblyReferencesNode, "add", "name", "Reports", "version", @"1.0.0.0", string.Empty,
                        string.Empty, string.Empty, string.Empty);
                    webConfigUpdated = true;
                }
                if (IsModulesExists(assemblyReferencesNode, "add", "MessagesFunctions") == false)
                {
                    AddToModuleSettings(xmlDoc, assemblyReferencesNode, "add", "name", "MessagesFunctions", "version", @"1.0.0.0", string.Empty,
                        string.Empty, string.Empty, string.Empty);
                    webConfigUpdated = true;
                }             
                if (webConfigUpdated)
                {
                    xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("configSectionsNode");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while creating default app settings configuration entries", ex);
            }
            log.LogMethodExit();
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

        private static bool IsModulesExists(XmlNode modulesNode, string key, string name)
        {
            log.LogMethodEntry(modulesNode, key, name);
            if (modulesNode.SelectSingleNode(key+@"[@name='" + name + "']") != null)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private static bool IsStaticModulesExists(XmlNode modulesNode, string key, string name)
        {
            log.LogMethodEntry(modulesNode, key, name);
            if (modulesNode.SelectSingleNode(key + @"[@fileExtension='" + name + "']") != null)
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

        private static void AddToModuleSettings(XmlDocument xmlDoc, XmlNode modulesNode, string element, string key1, string value1,
                                                 string key2, string value2, string key3, string value3, string key4, string value4)
        {
            log.LogMethodEntry(xmlDoc, modulesNode, key1, value1, key2, value2, key3, value3, key4, value4);
            XmlElement exeMode = xmlDoc.CreateElement(element);
            if (!string.IsNullOrEmpty(key1))
            {
                exeMode.SetAttribute(key1, value1);
            }
            if (!string.IsNullOrEmpty(key2))
            {
                exeMode.SetAttribute(key2, value2);
            }
            if (!string.IsNullOrEmpty(key3))
            {
                exeMode.SetAttribute(key3, value3);
            }
            if (!string.IsNullOrEmpty(key4))
            {
                exeMode.SetAttribute(key4, value4);
            }
            modulesNode.AppendChild(exeMode);
            log.LogMethodExit();
        }

        private static XmlNode CreateNode(XmlDocument xmlDoc, string element)
        {
            log.LogMethodEntry(xmlDoc, element);
            XmlElement systemWebserverElement = xmlDoc.CreateElement(element);
            xmlDoc.DocumentElement.AppendChild(systemWebserverElement);
            log.LogMethodExit(xmlDoc);
            return systemWebserverElement;
        }

        private static XmlNode CreateModulesNode(XmlDocument xmlDoc, XmlNode systemWebserverNode)
        {
            log.LogMethodEntry(xmlDoc, systemWebserverNode);
            XmlElement modulesElement = xmlDoc.CreateElement("modules");
            modulesElement.SetAttribute("runAllManagedModulesForAllRequests", "true");
            systemWebserverNode.AppendChild(modulesElement);
            log.LogMethodExit(xmlDoc);
            return modulesElement;
        }

        private static XmlNode CreateRequestLimitsNode(XmlDocument xmlDoc, XmlNode requestFilteringNode)
        {
            log.LogMethodEntry(xmlDoc, requestFilteringNode);
            XmlElement requestLimitsElement = xmlDoc.CreateElement("requestLimits");
            requestLimitsElement.SetAttribute("maxAllowedContentLength", "64000000");
            requestFilteringNode.AppendChild(requestLimitsElement);
            log.LogMethodExit(xmlDoc);
            return requestLimitsElement;
        }

        private static XmlNode CreateSubNode(XmlDocument xmlDoc, XmlNode subNode, string element)
        {
            log.LogMethodEntry(xmlDoc, subNode, element);
            XmlElement requestFilteringElement = xmlDoc.CreateElement(element);
            subNode.AppendChild(requestFilteringElement);
            log.LogMethodExit(xmlDoc);
            return requestFilteringElement;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            try
            {
                log.LogMethodEntry();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.Fatal("Application_error is invoked ");
                if (sender != null)
                    log.Fatal("Application_error " + sender.ToString());

                if (e != null)
                    log.Fatal("Application_error " + e.ToString());

                Exception ex = Server.GetLastError();

                if (ex != null)
                    log.Fatal("Unhandled exception caught in Application_error " + ex.ToString());

                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (ctx.Request != null && ctx.Request.Url != null)
                        sb.Append(ctx.Request.Url.ToString());

                    if (ctx.Server != null && ctx.Server.GetLastError() != null)
                    {
                        sb.Append(System.Environment.NewLine + "Source: " + ctx.Server.GetLastError().Source.ToString());
                        sb.Append(System.Environment.NewLine + "Message: " + ctx.Server.GetLastError().Message.ToString());
                        sb.Append(System.Environment.NewLine + "Stack Trace: " + ctx.Server.GetLastError().StackTrace.ToString());
                    }
                    log.Fatal("Exception in Application_Error " + sb.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Exception in handling Application_Error " + ex);
            }
            log.LogMethodExit();
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                log.Fatal("Application_End is invoked ");
                if (sender != null)
                    log.Fatal("Application_End " + sender.ToString());

                if (e != null)
                    log.Fatal("Application_End " + e.ToString());
                log.Fatal(System.Web.Hosting.HostingEnvironment.ShutdownReason);

                HttpContext ctx = HttpContext.Current;
                if (ctx != null)
                {
                    StringBuilder sb = new StringBuilder();
                    if (ctx.Request != null && ctx.Request.Url != null)
                        sb.Append(ctx.Request.Url.ToString());

                    if (ctx.Server != null && ctx.Server.GetLastError() != null)
                    {
                        sb.Append(System.Environment.NewLine + "Source: " + ctx.Server.GetLastError().Source.ToString());
                        sb.Append(System.Environment.NewLine + "Message: " + ctx.Server.GetLastError().Message.ToString());
                        sb.Append(System.Environment.NewLine + "Stack Trace: " + ctx.Server.GetLastError().StackTrace.ToString());
                    }
                    log.Fatal("Exception in Application_End " + sb.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Exception in handling Application_Error " + ex);
            }
            log.LogMethodExit();
        }
    }
}