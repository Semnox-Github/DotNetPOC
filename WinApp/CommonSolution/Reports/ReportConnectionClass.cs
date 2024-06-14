/********************************************************************************************
 * Project Name - ReportConnectionClass
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019     Dakshakh raj       Modified : added Logs
 *2.110         4-Jan-2021      Laster Menezes     Added new method GetDatabaseName
 ********************************************************************************************/
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Reports
{
    /// <summary>
    /// ReportConnectionClass Class
    /// </summary>
    public class ReportConnectionClass
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>  
        /// GetConnectionString(ReportServerCommon reportServerCommon) method
        /// </summary>
        /// <returns>returns ReportServerCommon</returns>
        public ReportServerCommon GetConnectionString()
        {
            ReportServerCommon reportServerCommon = new ReportServerCommon();
            log.LogMethodEntry();

            if (reportServerCommon == null)
                reportServerCommon = new ReportServerCommon();
            try
            {
                string connectionString = "";
                log.Debug(" Checking ParafaitConnectionString ");
                try
                {
                    connectionString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaitConnectionString"].ToString();
                }
                catch { }
                if (!string.IsNullOrEmpty(connectionString))
                {
                    if (IsConncectionValid(connectionString))
                    {
                        //Check having multiple Site
                        try
                        {

                            ReportServerGenericClass reportServerGenericClass = new ReportServerGenericClass(connectionString);
                            log.Debug("IsMultiSite Valid Site Id.." + reportServerGenericClass.GetSiteId());
                            log.Debug("IsMultiSite Valid Conn  .." + connectionString);

                            if (reportServerGenericClass.GetSiteId() != -1)
                            {
                                connectionString = GetConnectionString(connectionString, "ParafaitCentral");
                                reportServerCommon.ConnectionString = connectionString;
                                reportServerCommon.IsMultiDb = true;
                                log.Debug("IsMultiSite Valid config..");
                            }
                            else
                            {
                                reportServerCommon.ConnectionString = connectionString;
                                reportServerCommon.IsMultiDb = false;
                                log.Debug("Single Site Valid config..");
                            }

                        }
                        catch (Exception ex)
                        {
                            log.Error(" GetConnectionString Failed  with exception " + ex.Message);
                            reportServerCommon.Message += ex.Message;
                        }
                    }
                    else
                    {
                        reportServerCommon.Message = "ParafaitConnectionString coneection failed";
                    }
                }
                else
                {
                    try
                    {
                        log.Debug(" Checking CentralConnectionString ");
                        string CentralConnectionString = ConfigurationManager.ConnectionStrings["ParafaitUtils.Properties.Settings.ParafaiCentraltConnectionString"].ToString();

                        if (IsConncectionValid(CentralConnectionString))
                        {
                            reportServerCommon.ConnectionString = CentralConnectionString;
                            reportServerCommon.IsMultiDb = true;
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
                reportServerCommon.Message += ex.Message;
            }
            log.LogMethodExit(reportServerCommon);
            return reportServerCommon;
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
        /// GetConnectionString
        /// </summary>
        /// <param name="conStr">conStr</param>
        /// <param name="dbName">dbName</param>
        /// <returns></returns>
        private string GetConnectionString(string conStr, string dbName)
        {
            log.LogMethodEntry(conStr, dbName);
            try

            {
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
                log.Debug("Ends- GetConnectionString(string conStr, string dbName) Method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends- GetConnectionString(string conStr, string dbName) Method with exception." + ex.Message);
            }
            log.LogMethodExit(conStr);
            return conStr;


        }

        /// <summary>
        /// IsMultiSite
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <param name="isMultiSite">isMultiSite</param>
        /// <returns></returns>
        private bool IsMultiSite(string connectionString, ref bool isMultiSite)
        {
            log.LogMethodEntry(connectionString, isMultiSite);
            try
            {
                log.Debug("Begin IsMultiSite(string connectionString, ref bool isMultiSite) method.");
                using (Utilities siteUtilities = new Utilities(connectionString))
                {
                    DataTable dtSites = siteUtilities.DBUtilities.executeDataTable("select * from site ");
                    if (dtSites == null || dtSites.Rows.Count == 0)
                    {
                        isMultiSite = false;
                        log.LogMethodExit(false);
                        return false;
                    }
                    else if (dtSites.Rows.Count == 1)
                    {
                        isMultiSite = false;
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        isMultiSite = true;
                        log.LogMethodExit(true);
                        return true;

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends IsMultiSite(string connectionString, ref bool isMultiSite) method with Exception." + ex.Message);
            }

            log.LogMethodExit(false);
            return false;
        }

        /// <summary>
        /// GetDatabaseName
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>The InitialCatalog in the connectionstring </returns>
        public string GetDatabaseName(string connectionString)
        {
            log.LogMethodEntry(connectionString);
            string dbName = string.Empty;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            dbName = builder.InitialCatalog;
            return dbName;
        }

    }
}

