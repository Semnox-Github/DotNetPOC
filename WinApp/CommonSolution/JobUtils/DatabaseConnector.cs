/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - DB Connector Class Logic for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       14-May-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *********************************************************************************************/
using System;
using System.Configuration;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Site;
using System.Windows.Forms;
using System.Linq;


namespace Semnox.Parafait.JobUtils
{
    public class DatabaseConnector
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DatabaseConnectorDTO databaseConnectorDTO = new DatabaseConnectorDTO();
        string connectionString = string.Empty;
        //private const string MASTER_SITE_NAME = "master";


        /// <summary>  
        /// GetDatabaseConnectorDTO method
        /// </summary>
        /// <returns>returns databaseConnectorDTO</returns>
        public DatabaseConnectorDTO GetDatabaseConnectorDTO()
        {
            log.LogMethodEntry();
            try
            {
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ToString();
                }
                catch (Exception e)
                {
                    log.Info("Parafait connection string is not specified");
                }
                if (!string.IsNullOrEmpty(connectionString))
                {
                    Utilities utilities = new Utilities(connectionString);
                    if (IsConncectionValid(connectionString))
                    {
                        try
                        {
                            if (utilities.ParafaitEnv.SiteId != -1)
                            {
                                connectionString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaiCentraltConnectionString"].ToString();
                                if (IsConncectionValid(connectionString))
                                {
                                    databaseConnectorDTO.ConnectionString = connectionString;
                                    databaseConnectorDTO.IsMultiDb = true;
                                }
                            }
                            else
                            {
                                databaseConnectorDTO.ConnectionString = connectionString;
                                databaseConnectorDTO.IsMultiDb = false;
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(" GetConnectionString Failed  with exception " + ex.Message);
                            databaseConnectorDTO.Message += ex.Message;
                        }
                    }
                    else
                    {
                        log.Error("ParafaitConnectionString connection failed");
                        databaseConnectorDTO.Message = "ParafaitConnectionString connection failed";
                    }
                }
                else
                {
                    try
                    {
                        log.Debug(" Checking CentralConnectionString ");
                        string centralConnectionString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaiCentraltConnectionString"].ToString();
                        connectionString = centralConnectionString;
                        if (IsConncectionValid(centralConnectionString))
                        {
                            databaseConnectorDTO.ConnectionString = centralConnectionString;
                            databaseConnectorDTO.IsMultiDb = true;
                            log.Info(" CentralConnectionString is valid ");
                        }
                    }
                    catch
                    {
                        log.Error("unable to get CentralConnectionString config..");
                    }
                }
                log.Debug("Ends GetConnectionString(ref IsMultiDb, ref  ConnectionString, ref message) method");
            }
            catch (Exception ex)
            {
                log.Error("Ends GetConnectionString(ref IsMultiDb, ref  ConnectionString, ref message) method with exception " + ex.Message);
                databaseConnectorDTO.Message += ex.Message;
            }
            log.LogMethodExit(databaseConnectorDTO);
            return databaseConnectorDTO;
        }

        /// <summary>
        /// IsConncectionValid(string connectionString) method
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        private bool IsConncectionValid(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            try
            {
                using (Utilities siteUtilities = new Utilities(connectionString))
                {
                    try
                    {
                        using (SqlConnection db = new SqlConnection(siteUtilities.DBUtilities.sqlConnection.ConnectionString))
                        {
                            db.Open();
                            db.Close();
                            log.Debug("Connection established successfully for" + connectionString);
                            log.LogMethodExit(true);
                            return true;

                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Ends IsConncectionValid(string connectionString) with exception 1" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends IsConncectionValid(string connectionString) with exception 2" + ex.Message);
            }
            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// GetConnectionStringList
        /// </summary>
        /// <param name="IsMutiDb">IsMutiDb</param>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        public List<DatabaseConnectorDTO> GetConnectionStringList()
        {
            log.LogMethodEntry();
            List<DatabaseConnectorDTO> databaseConnectorDTOList = new List<DatabaseConnectorDTO>();
            if (databaseConnectorDTO.IsMultiDb)
            {
                databaseConnectorDTOList = GetConnectionStringsForMultiDB();
            }
            else
            {
                databaseConnectorDTOList = GetConnectionStringForLocal();
            }
            log.LogMethodExit(databaseConnectorDTOList);
            return databaseConnectorDTOList;
        }


        private List<DatabaseConnectorDTO> GetConnectionStringsForMultiDB()
        {
            log.LogMethodEntry();
            List<DatabaseConnectorDTO> databaseConnectorDTOList = new List<DatabaseConnectorDTO>();
            try
            {
                List<CentralCompanyDTO> siteList = GetDbList();
                string siteConnectionString = string.Empty;
                if (siteList == null || siteList.Count < 0)
                {
                    log.Error("Empty Site List in HQ ");
                    return databaseConnectorDTOList;
                }
                foreach (CentralCompanyDTO centralSite in siteList)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(centralSite.DBName))
                        {
                            continue;
                        }
                        DatabaseConnectorDTO databaseConnectorDTO = new DatabaseConnectorDTO();
                        siteConnectionString = ChangeConnectionString(connectionString, centralSite.DBName);

                        if (!string.IsNullOrEmpty(siteConnectionString) &&
                            IsConncectionValid(siteConnectionString))
                        {
                            if (IsGreaterVersion(GetSiteVersion(siteConnectionString, true))) //&& IsConcurrentProgramEnabled(siteId))
                            {

                                int masterSiteId = GetMasterSiteId(siteConnectionString, true);
                                if (IsConcurrentManagerEnabled(masterSiteId, siteConnectionString))
                                {
                                    if (!string.IsNullOrEmpty(centralSite.ApplicationFolderPath))
                                    {
                                        databaseConnectorDTO.ApplicationFolderPath = centralSite.ApplicationFolderPath;
                                    }
                                    databaseConnectorDTO.ConnectionString = siteConnectionString;
                                    databaseConnectorDTOList.Add(databaseConnectorDTO);
                                }
                                else
                                {
                                    log.Error("CONCURRENT_MANAGER_ENABLED is disabled");
                                }
                            }
                            else
                            {
                                log.Error("Site Version is older for following connection." + siteConnectionString +
                                             "Concurrent Manager will not be triggered");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Ends GetConnectionStringList() with exception" + ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                log.Error(" Ends GetConnectionStringList() with exception" + ex.Message);
            }

            log.LogMethodExit(databaseConnectorDTOList);
            return databaseConnectorDTOList;
        }

        /// <summary>
        /// getParafaitDefaults
        /// </summary>
        /// <param name="default_value_name"></param>
        /// <returns>ParafaitDefaults vlaue</returns>
        private bool IsConcurrentManagerEnabled(int siteId, string connectionString)
        {
            log.LogMethodEntry(siteId);
            bool concurrentManagerEnabled = false;
            DataAccessHandler dataAccessHandler = new DataAccessHandler(connectionString);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@default_value_name", "CONCURRENT_MANAGER_ENABLED"));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId));
            string query = @"select  default_value value 
                                                        from parafait_defaults  
                                                        where active_flag = 'Y' 
                                                        and default_value_name = @default_value_name 
                                                        and (site_id = @site_id or @site_id = -1)";
            object returnValue = dataAccessHandler.executeScalar(query, parameters.ToArray(), null);
            //object returnValue = utilities.executeScalar(@"select  default_value value 
            //                                            from parafait_defaults  
            //                                            where active_flag = 'Y' 
            //                                            and default_value_name = @default_value_name 
            //                                            and (site_id = @site_id or @site_id = -1)",
            //                                            new SqlParameter("@default_value_name", "CONCURRENT_MANAGER_ENABLED"),
            //                                            new SqlParameter("@site_id", siteId)
            //                                            );

            log.LogVariableState("returnValue", returnValue);
            if (returnValue.Equals("Y"))
                concurrentManagerEnabled =  true;
            return concurrentManagerEnabled;
        }

        private List<DatabaseConnectorDTO> GetConnectionStringForLocal()
        {
            log.LogMethodEntry();
            List<DatabaseConnectorDTO> databaseConnectorDTOList = new List<DatabaseConnectorDTO>();
            try
            {
                DatabaseConnectorDTO databaseConnectorDTO = new DatabaseConnectorDTO();
                if (!string.IsNullOrEmpty(connectionString)) // should we check for Is concurrent mgr enabled?
                {
                    if (IsGreaterVersion(GetSiteVersion(connectionString, false)))// && IsConcurrentProgramEnabled(siteId))
                    {
                        int masterSiteId = GetMasterSiteId(connectionString, false);
                        if (IsConcurrentManagerEnabled(masterSiteId, connectionString))
                        {
                            databaseConnectorDTO.ConnectionString = connectionString;
                            databaseConnectorDTO.ApplicationFolderPath = Application.StartupPath.ToString();
                            databaseConnectorDTOList.Add(databaseConnectorDTO);
                        }
                        else
                        {
                            log.Error("CONCURRENT_MANAGER_ENABLED is disabled");
                        }
                    }
                    else
                    {
                        log.Error("Site Version is older for following connection." + connectionString +
                            "Concurrent Manager will not be triggered");
                    }
                }
                log.Debug("Ends GetConnectionStringList() local ");
            }
            catch (Exception ex)
            {
                log.Error("Ends GetConnectionStringList() local  with exception " + ex.Message);
            }
            log.LogMethodExit(databaseConnectorDTOList);
            return databaseConnectorDTOList;
        }

        /// <summary>
        /// changeConnectionString  method
        /// </summary>
        /// <param name="conStr">conStr</param>
        /// <param name="dbName">dbName</param>
        /// <returns>returns string</returns>
        private static string ChangeConnectionString(string conStr, string dbName)
        {
            log.LogMethodEntry(conStr, dbName);
            int pos1 = conStr.IndexOf("Database");
            if (pos1 < 0)
            {
                pos1 = conStr.IndexOf("Initial Catalog");
                int pos2 = conStr.IndexOf(";", pos1);
                if (pos2 > 0)
                    conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName + conStr.Substring(pos2);
                else
                    conStr = conStr.Substring(0, pos1) + "Initial Catalog=" + dbName;
            }
            else if (pos1 > 0)
            {
                int pos2 = conStr.IndexOf(";", pos1);
                if (pos2 > 0)
                    conStr = conStr.Substring(0, pos1) + "Database=" + dbName + conStr.Substring(pos2);
                else
                    conStr = conStr.Substring(0, pos1) + "Database=" + dbName;
            }
            log.LogMethodExit(conStr);
            return conStr;
        }

        /// <summary>
        /// GetDbList
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        private List<CentralCompanyDTO> GetDbList()
        {
            log.LogMethodEntry();
            List<CentralCompanyDTO> siteDTOList = null;
            try
            {
                using (ParafaitDBTransaction dbtrx = new ParafaitDBTransaction(connectionString))
                {
                    try
                    {
                        dbtrx.BeginTransaction();
                        CentralCompanyList companySiteList = new CentralCompanyList();
                        List<KeyValuePair<CentralCompanyDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<CentralCompanyDTO.SearchByParameters, string>>();
                        searchByParams.Add(new KeyValuePair<CentralCompanyDTO.SearchByParameters, string>(CentralCompanyDTO.SearchByParameters.IS_ACTIVE, "1"));
                        siteDTOList = companySiteList.GetAllCompanies(searchByParams, connectionString);
                        dbtrx.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        dbtrx.RollBack();
                        log.Error(ex);
                    }
                }
                    

            }
            catch (Exception ex)
            {
                log.Error("Ends IsConncectionValid(string connectionString) with exception 1" + ex.Message);
            }
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
        }

        /// <summary>
        /// IsGreaterVersion
        /// </summary>
        /// <param name="version">version</param>
        /// <returns></returns>
        private bool IsGreaterVersion(string version)
        {
            log.LogMethodEntry(version);
            bool isGreaterVersion = false;
            try
            {
                string[] arr;

                string[] separators = { "." };
                arr = version.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length > 0)
                {
                    if (Convert.ToInt32(arr[0]) >= 2)
                    {
                        if (Convert.ToInt32(arr[1]) == 120 && Convert.ToInt32(arr[2]) == 0)
                        {
                            isGreaterVersion = false;
                        }
                        else if (Convert.ToInt32(arr[1]) >= 120)
                        {
                            isGreaterVersion = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-IsGreaterVersion method with exception" + ex.Message);
            }
            log.LogMethodExit(isGreaterVersion);
            return isGreaterVersion;
        }

        /// <summary>
        /// GetSiteVersion() method
        /// </summary>
        /// <returns></returns>
        private string GetSiteVersion(string siteConnectionString, bool multiDB)
        {
            log.LogMethodEntry();
            string siteVersion = string.Empty;
            SiteList siteList = new SiteList();
            try
            {
                using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction(siteConnectionString))
                {
                    try
                    {
                        dbTrx.BeginTransaction();
                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                        if (multiDB)
                        {
                            searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SHOW_ONLY_MASTER_SITE, ""));
                        }
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters, siteConnectionString);
                        dbTrx.EndTransaction();
                        if (siteDTOList.Count == 1)
                        {
                            siteVersion = siteDTOList[0].Version;
                        }
                        else if (siteDTOList.Count > 1)
                        {
                            for (int i = 0; i < siteDTOList.Count; i++)
                            {
                                if (siteDTOList[i].IsMasterSite)
                                {
                                    siteVersion = siteDTOList[i].Version;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTrx.RollBack();
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends IsConncectionValid(string connectionString) with exception" + ex.Message);
            }
            log.LogMethodExit(siteVersion);
            return siteVersion;
        }

        /// <summary>
        /// GetMasterSiteId() method
        /// </summary>
        /// <returns></returns>
        private int GetMasterSiteId(string siteConnectionString, bool multiDB)
        {
            log.LogMethodEntry();
            int siteId = -1;
            SiteList siteList = new SiteList();
            try
            {
                using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction(siteConnectionString))
                {
                    try
                    {
                        dbTrx.BeginTransaction();
                        List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                        searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
                        if (multiDB)
                        {
                            searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SHOW_ONLY_MASTER_SITE, ""));
                        }
                        List<SiteDTO> siteDTOList = siteList.GetAllSites(searchParameters, siteConnectionString);
                        dbTrx.EndTransaction();
                        if (multiDB && siteDTOList.Count == 1)
                        {
                            siteId = siteDTOList[0].SiteId;
                        }
                        else if (siteDTOList.Count > 1)
                        {
                            for (int i = 0; i < siteDTOList.Count; i++)
                            {
                                if (siteDTOList[i].IsMasterSite)
                                {
                                    siteId = siteDTOList[i].SiteId;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTrx.RollBack();
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends IsConncectionValid(string connectionString) with exception" + ex.Message);
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
    }
}


