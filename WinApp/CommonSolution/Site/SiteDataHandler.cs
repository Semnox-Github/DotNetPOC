/********************************************************************************************
 * Project Name - Site Data Handler
 * Description  - Data handler of the site data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Feb-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        28-Jun-2016   Raghuveera          Modified
 *2.40        23-Oct-2018   Jagan Mohan         Created method GetAllSitesUserLogedIn and GetSiteListDTO
 *2.60        08-Mar-2019   Jagan Mohan         Created method InsertSite and UpdateSite
 *2.60        24-Apr-2019   Mushahid Faizan     Added GetSQLParameters() Method & Modified InsertSite/UpdateSite.
 *2.60        03-May-2019   Divya               SQL Injection
 *2.60.2      20-May-2019   Jagan Mohana        Created new method GetMaxSiteId();
 *2.60.2      08-May-2019   Nitin Pai           Guest App Changes
 *2.70.2      23-Jul-2019   Deeksha             Modifications as per three tier standard.
 *2.100.0     24-Sept-2020  Mushahid Faizan     Modified for Service Request Enhancement
 *2.110.0     01-Feb-2021   Girish Kundar       Modified : Urban Piper changes
 *2.120.1     26-May-2021   Deeksha             Modified as part of AWS Job Scheduler enhancements
 *2.140.0     21-Jun-2021   Fiona Lishal        Modified for Delivery Order enhancements for F&B and Urban Piper
 *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
using Semnox.Core.Utilities; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text; 

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Site Data Handler - Handles insert, update and select of site data objects
    /// </summary>
    public class SiteDataHandler
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<SiteDTO.SearchBySiteParameters, string> DBSearchParameters = new Dictionary<SiteDTO.SearchBySiteParameters, string>
               {
                    {SiteDTO.SearchBySiteParameters.SHOW_ONLY_MASTER_SITE, ""},
                    {SiteDTO.SearchBySiteParameters.SITE_ID, "site_id"},
                    {SiteDTO.SearchBySiteParameters.SITE_NAME, "site_name"},
                    {SiteDTO.SearchBySiteParameters.ORG_ID, "OrgId"},
                    {SiteDTO.SearchBySiteParameters.SITE_CODE, "SiteCode"},//Modification on 28-Jun-2016 for added site code for filter option.
                    {SiteDTO.SearchBySiteParameters.GUID, "Guid"},
                    {SiteDTO.SearchBySiteParameters.IS_ACTIVE, "active_flag"},
                    {SiteDTO.SearchBySiteParameters.ONLINE_ENABLED, "OnlineEnabled"},
                    {SiteDTO.SearchBySiteParameters.MASTER_ENTITY_ID, "MasterEntityId"},
                    {SiteDTO.SearchBySiteParameters.SITE_GUID, "site_guid"},
                    {SiteDTO.SearchBySiteParameters.LAST_UPDATED_DATE, "LastUpdateDate"}
               };

        /// <summary>
        /// Default constructor of SiteDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public SiteDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Default constructor of SiteDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public SiteDataHandler(string  connectionString)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler(connectionString);
            this.sqlTransaction = null;
            log.LogMethodExit();
        }



        /// <summary>
        /// Converts the Data row object to SiteDTO class type
        /// </summary>
        /// <param name="siteDataRow">SiteDTO DataRow</param>
        /// <returns>Returns SiteDTO</returns>
        public SiteDTO GetSiteDTO(DataRow siteDataRow)
        {
            log.LogMethodEntry(siteDataRow);
            SiteDTO siteDataObject = new SiteDTO(Convert.ToInt32(siteDataRow["site_id"]),
                                            siteDataRow["site_name"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["site_name"]),
                                            siteDataRow["site_address"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["site_address"]),
                                            siteDataRow["notes"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["notes"]),
                                            siteDataRow["site_guid"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["site_guid"]),
                                            siteDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["Guid"]),
                                            siteDataRow["Version"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["Version"]),
                                            siteDataRow["OrgId"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["OrgId"]),
                                            siteDataRow["last_upload_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["last_upload_time"]),
                                            siteDataRow["last_upload_server_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["last_upload_server_time"]),
                                            siteDataRow["last_upload_message"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["last_upload_message"]),
                                            siteDataRow["InitialLoadDone"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["InitialLoadDone"]),
                                            siteDataRow["MaxCards"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["MaxCards"]),
                                            siteDataRow["CustomerKey"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["CustomerKey"]),
                                            siteDataRow["Company_Id"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["Company_Id"]),
                                            siteDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(siteDataRow["SynchStatus"]),
                                            siteDataRow["SiteCode"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["SiteCode"]),
                                            siteDataRow.Table.Columns.Contains("isMasterSiteId")?siteDataRow["isMasterSiteId"] == DBNull.Value ? false : Convert.ToBoolean(siteDataRow["isMasterSiteId"]) :false,
											siteDataRow["siteShortName"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["siteShortName"]),
                                            siteDataRow["onlineEnabled"] == DBNull.Value ? string.Empty : Convert.ToString(siteDataRow["onlineEnabled"]),
                                            siteDataRow["AboutUs"] == DBNull.Value ? string.Empty : siteDataRow["AboutUs"].ToString(),
                                            siteDataRow["PinCode"] == DBNull.Value ? string.Empty : siteDataRow["PinCode"].ToString(),
                                            siteDataRow.Table.Columns.Contains("active_flag") ? siteDataRow["active_flag"] == DBNull.Value ? true : (siteDataRow["active_flag"].ToString().Equals("Y") || siteDataRow["active_flag"].ToString().Equals("1")) ? true : false : true,
                                            siteDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["CreationDate"]),
                                            siteDataRow["CreatedBy"] == DBNull.Value ? string.Empty : siteDataRow["CreatedBy"].ToString(),
                                            siteDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["LastUpdateDate"]),
                                            siteDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : siteDataRow["LastUpdatedBy"].ToString(),
                                            siteDataRow["Description"] == DBNull.Value ? string.Empty : siteDataRow["description"].ToString(),
                                            siteDataRow["OpenDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["OpenDate"]),
                                            siteDataRow["closureDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(siteDataRow["closureDate"]),
                                            siteDataRow["SiteURL"] == DBNull.Value ? string.Empty : siteDataRow["SiteURL"].ToString(),
                                            siteDataRow["Size"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["Size"]),
                                            siteDataRow["StoreType"] == DBNull.Value ? string.Empty : siteDataRow["StoreType"].ToString(),
                                            siteDataRow["City"] == DBNull.Value ? string.Empty : siteDataRow["City"].ToString(),
                                            siteDataRow["State"] == DBNull.Value ? string.Empty : siteDataRow["State"].ToString(),
                                            siteDataRow["Country"] == DBNull.Value ? string.Empty : siteDataRow["Country"].ToString(),
                                            siteDataRow["OpenTime"] == DBNull.Value ? 0.0M : Convert.ToDecimal(siteDataRow["OpenTime"].ToString()),
                                            siteDataRow["CloseTime"] == DBNull.Value ? 0.0M : Convert.ToDecimal(siteDataRow["CloseTime"].ToString()),
                                            siteDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["MasterEntityId"]),
                                            siteDataRow["logo"] == DBNull.Value ? null : siteDataRow["logo"] as byte[],
                                            siteDataRow["StoreRanking"] == DBNull.Value ? string.Empty : siteDataRow["StoreRanking"].ToString(),
                                            siteDataRow["ExternalSourceReference"] == DBNull.Value ? string.Empty : siteDataRow["ExternalSourceReference"].ToString(),
                                            siteDataRow["Email"] == DBNull.Value ? string.Empty : siteDataRow["Email"].ToString(),
                                            siteDataRow["PhoneNumber"] == DBNull.Value ? string.Empty : siteDataRow["PhoneNumber"].ToString(),
                                            siteDataRow["Latitude"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(siteDataRow["Latitude"]),
                                            siteDataRow["Longitude"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(siteDataRow["Longitude"]),
                                            siteDataRow["SiteConfigComplete"] == DBNull.Value ? "Y" : siteDataRow["SiteConfigComplete"].ToString()
                                            );

            log.LogMethodExit(siteDataObject);
            return siteDataObject;
        }
        /// <summary>
        /// select the site list from the site table based on the paramater value
        /// </summary>        
        /// <param name="siteId"> Site id to exclude from the list else -1</param>
        /// <param name="orgId">site organization id or -1</param>
        /// <param name="companyId"> site company id or -1</param>
        /// <returns></returns>
        public List<SiteDTO> GetAllSite(int siteId, int orgId, int companyId)
        {
            log.LogMethodEntry(siteId, orgId, companyId);
            string selectSiteQuery = @"select * , (select case when isnull(Master_Site_Id,-1)=site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId
                                  from site
                                  where (OrgId = @orgid or @orgid = -1) and (site_id != @siteid or @siteid = -1) and (Company_Id = @companyId or @companyId = -1)";
            SqlParameter[] selectSiteParameters = new SqlParameter[3];
            selectSiteParameters[0] = new SqlParameter("@siteid", siteId);
            selectSiteParameters[1] = new SqlParameter("@orgid", orgId);
            selectSiteParameters[2] = new SqlParameter("@companyId", companyId);
            DataTable siteData = dataAccessHandler.executeSelectQuery(selectSiteQuery, selectSiteParameters, sqlTransaction);
            if (siteData.Rows.Count > 0)
            {
                List<SiteDTO> siteList = new List<SiteDTO>();
                foreach (DataRow siteDataRow in siteData.Rows)
                {
                    SiteDTO siteDataObject = GetSiteDTO(siteDataRow);
                    siteList.Add(siteDataObject);
                }
                log.LogMethodExit(siteList);
                return siteList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }

        }

        /// <summary>
        /// Gets the site data of passed site id
        /// </summary>
        /// <param name="siteId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns SiteDTO</returns>
        public SiteDTO GetSite(int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            string selectSiteQuery = @"select *, (select case when isnull(Master_Site_Id,-1)=site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId
                                         from Site
                                        where site_id = @siteId";
            SqlParameter[] selectSiteParameters = new SqlParameter[1];
            selectSiteParameters[0] = new SqlParameter("@siteId", siteId);
            DataTable site = dataAccessHandler.executeSelectQuery(selectSiteQuery, selectSiteParameters, sqlTransaction);
            if (site.Rows.Count > 0)
            {
                DataRow siteRow = site.Rows[0];
                SiteDTO siteDataObject = GetSiteDTO(siteRow);
                log.LogMethodExit(siteDataObject);
                return siteDataObject;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// GetDbList
        /// </summary>
        /// <param name="connectionString">connectionString</param>
        /// <returns></returns>
        internal List<SiteDTO> GetSiteList()
        {
            log.LogMethodEntry();
            DataTable siteList = new DataTable();
            List<SiteDTO> siteDTOList = null;
            string selectSitesQuery = @"select * from site where active_flag ='Y'  ";
            DataTable dtSites = dataAccessHandler.executeSelectQuery(selectSitesQuery, null);
            if (dtSites.Rows.Count > 0)
            {
                siteDTOList = dtSites.Rows.Cast<DataRow>().Select(x => GetSiteDTO(x)).ToList();
            }
            return siteDTOList;
        }

        /// <summary>
        /// Gets the SiteDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        /// <returns>Returns the list of SiteDTO matching the search criteria</returns>
        public List<SiteDTO> GetSiteList(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            int count = 0;
            string selectSiteQuery = @"select *, (select case when isnull(Master_Site_Id,-1)=site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId
                                         from Site";
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");
                string joinOperartor;//Starts: Modification on 28-Jun-2016 for added site code for filter option.
                foreach (KeyValuePair<SiteDTO.SearchBySiteParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joinOperartor = (count == 0) ? string.Empty : " and ";

                        if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.SHOW_ONLY_MASTER_SITE))
                            query.Append(joinOperartor + " site_id in  ( select master_site_id from company where Master_Site_Id is not null ) "); //Ends: Modification to get master site option.
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.GUID)
                            || searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.SITE_GUID))
                        {
                            log.Debug("-GetSiteList(searchParameters) Method.guid:" + searchParameter.Value);
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.SITE_CODE)
                            || searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.MASTER_ENTITY_ID)
                            || searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.ORG_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));//Ends: Modification on 28-Jun-2016 for added site code for filter option.
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " OR -1 = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.IS_ACTIVE))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'Y') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.ONLINE_ENABLED))
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N') =" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "Y" : "N")));
                        }
                        else if (searchParameter.Key.Equals(SiteDTO.SearchBySiteParameters.LAST_UPDATED_DATE))
                        {
                            query.Append(joinOperartor + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectSiteQuery = selectSiteQuery + query;
                selectSiteQuery = selectSiteQuery + " order by site_name";
            }

            DataTable siteData = dataAccessHandler.executeSelectQuery(selectSiteQuery, parameters.ToArray(), sqlTransaction);
            if (siteData.Rows.Count > 0)
            {
                List<SiteDTO> siteList = new List<SiteDTO>();
                foreach (DataRow siteDataRow in siteData.Rows)
                {
                    SiteDTO siteDataObject = GetSiteDTO(siteDataRow);
                    siteList.Add(siteDataObject);
                }
                log.LogMethodExit(siteList);
                return siteList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Returns the roaming sites based on the current site id passed
        /// </summary>
        /// <param name="currentSiteId">current site id</param>
        /// <returns></returns>
        public List<SiteDTO> GetRoamingSites(int currentSiteId)
        {
            log.LogMethodEntry(currentSiteId);
            List<SiteDTO> siteDTOList = new List<SiteDTO>();
            DataTable dtOrg = dataAccessHandler.executeSelectQuery(@"WITH n(OrgId, ParentOrgId, OrgName, StructureId, level1) AS 
                                                                       (SELECT OrgId, ParentOrgId, OrgName, StructureId, 0
                                                                        FROM Organization
                                                                        WHERE OrgId = (select OrgId from Site where site_id = @site_id)
                                                                            UNION ALL
                                                                        SELECT nplus1.OrgId, nplus1.ParentOrgId, nplus1.OrgName, nplus1.StructureId, level1 + 1
                                                                        FROM Organization as nplus1, n
                                                                        WHERE nplus1.OrgId = n.ParentOrgId)
                                                                    SELECT OrgId, OrgName, ors.StructureName, isnull(ors.AutoRoam, 'N') AutoRoam, level1
                                                                      FROM n, OrgStructure ors
                                                                     where n.StructureId = ors.StructureId
                                                                     order by ors.AutoRoam desc, level1 desc", new SqlParameter[] { dataAccessHandler.GetSQLParameter("@site_id", currentSiteId) }, sqlTransaction);
            if (dtOrg.Rows.Count > 0 && dtOrg.Rows[0]["AutoRoam"].ToString().Equals("Y"))
            {
                dataAccessHandler = new DataAccessHandler();
                object orgId;
                orgId = dtOrg.Rows[0][0]; // highest level auto roam org
                DataTable siteData = dataAccessHandler.executeSelectQuery(@"WITH n(OrgId, OrgName) AS 
                                                                           (SELECT OrgId, OrgName
                                                                            FROM Organization
                                                                            WHERE OrgId = @OrgId
                                                                            UNION ALL
                                                                            SELECT nplus1.OrgId, nplus1.OrgName
                                                                            FROM Organization as nplus1, n
                                                                            WHERE n.OrgId = nplus1.ParentOrgId)
                                                                        SELECT s.*, (select case when isnull(Master_Site_Id,-1)=s.site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId
                                                                          FROM n, site s
                                                                         where s.OrgId = n.orgId AND (s.active_flag = 'Y' OR s.active_flag IS NULL)", new SqlParameter[] { dataAccessHandler.GetSQLParameter("@OrgId", dtOrg.Rows[0][0]), dataAccessHandler.GetSQLParameter("@site_id", currentSiteId) }, sqlTransaction);
                if (siteData.Rows.Count > 0)
                {
                    foreach (DataRow siteDataRow in siteData.Rows)
                    {
                        SiteDTO siteDTO = GetSiteDTO(siteDataRow);
                        siteDTOList.Add(siteDTO);
                    }
                }
            }
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
        }

        /// <summary>
        /// returns all the active sites except master site
        /// </summary>
        /// <returns></returns>
        public List<SiteDTO> GetAllSitesForRoaming(int currentSiteId)
        {
            log.LogMethodEntry(currentSiteId);
            List<SiteDTO> siteDTOList = new List<SiteDTO>();
            DataTable siteData = dataAccessHandler.executeSelectQuery(@"SELECT * , (select case when isnull(Master_Site_Id,-1)=site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId
                                  FROM SITE
                                 WHERE SITE_ID != (SELECT MASTER_SITE_ID FROM COMPANY)
                                   AND (ACTIVE_FLAG = 'Y' OR ACTIVE_FLAG IS NULL)", new SqlParameter[] { dataAccessHandler.GetSQLParameter("@site_id", currentSiteId) }, sqlTransaction);
            if (siteData.Rows.Count > 0)
            {
                foreach (DataRow siteDataRow in siteData.Rows)
                {
                    SiteDTO siteDTO = GetSiteDTO(siteDataRow);
                    siteDTOList.Add(siteDTO);
                }
            }
            log.LogMethodExit(siteDTOList);
            return siteDTOList;
        }

        /// <summary>
        /// Get the site list based on the user loginId
        /// </summary>        
        /// <param name="loginId">loginId</param>        
        /// <returns>sitelist</returns>
        public List<CommonLookupDTO> GetAllSitesUserLogedIn(string loginId)
        {
            log.LogMethodEntry(loginId);
            string selectSiteQuery = @"SELECT U.SITE_ID,ISNULL(S.SITE_NAME, (SELECT TOP 1 SITE_NAME FROM SITE)) as SITE_NAME, S.ORGID, (select case when isnull(Master_Site_Id,-1)=S.site_id then convert(bit,1) else convert(bit,0) end  from Company) as isMasterSiteId 
                                       FROM USERS U, SITE S WHERE (U.SITE_ID IS NULL OR U.SITE_ID = S.SITE_ID) AND LOGINID";
            selectSiteQuery = selectSiteQuery + "='" + loginId + "'";
            DataTable siteData = dataAccessHandler.executeSelectQuery(selectSiteQuery, null, sqlTransaction);
            if (siteData.Rows.Count > 0)
            {
                List<CommonLookupDTO> siteList = new List<CommonLookupDTO>();
                foreach (DataRow siteDataRow in siteData.Rows)
                {
                    CommonLookupDTO siteDataObject = GetSiteListDTO(siteDataRow);
                    siteList.Add(siteDataObject);
                }
                log.LogMethodExit(siteList);
                return siteList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Converts the Data row object to Sites and checking whether site belongs to IsMasterSite or not
        /// </summary>
        /// <param name="siteDataRow">SiteSelectionDTO DataRow</param>
        /// <returns>Returns SiteSelectionDTO</returns>
        public CommonLookupDTO GetSiteListDTO(DataRow siteDataRow)
        {
            log.LogMethodEntry(siteDataRow);
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            int orgid = siteDataRow["ORGID"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["ORGID"]);
            bool isMasterSite = siteDataRow["isMasterSiteId"] == DBNull.Value ? false : Convert.ToBoolean(siteDataRow["isMasterSiteId"]);
            if (isMasterSite)
            {
                keyValuePairs.Add("IsMasterSite", "1");
            }
            else
            {
                keyValuePairs.Add("IsMasterSite", "0");
            }
            int siteId = siteDataRow["SITE_ID"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["SITE_ID"]);
            CommonLookupDTO siteDataObject = new CommonLookupDTO(siteId.ToString(),
                                            siteDataRow["SITE_NAME"].ToString(),
                                            keyValuePairs
                                            );
            log.LogMethodExit(siteDataObject);
            return siteDataObject;
        }
        /// <summary>
        /// Converts the Data row object to Sites
        /// </summary>
        /// <param name="siteDataRow">SiteSelectionDTO DataRow</param>
        /// <returns>Returns SiteSelectionDTO</returns>
        public CommonLookupDTO GetSiteList(DataRow siteDataRow)
        {
            log.LogMethodEntry(siteDataRow);
            
            int siteId = siteDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(siteDataRow["site_id"]);
            CommonLookupDTO siteDataObject = new CommonLookupDTO(siteId.ToString(),
                                            siteDataRow["site_name"].ToString()                                            
                                            );
            log.LogMethodExit(siteDataObject);
            return siteDataObject;
        }
        /// <summary>
        /// Get max siteId
        /// </summary>
        /// <returns></returns>
        public int GetMaxSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            string selectSiteQuery = @"SELECT max(site_Id)+1 as site_id from site";
            DataTable siteData = dataAccessHandler.executeSelectQuery(selectSiteQuery, null, sqlTransaction);
            if (siteData.Rows.Count > 0)
            {
                DataRow siteDataRow = siteData.Rows[0];
                siteId = Convert.ToInt32(siteDataRow["site_id"]);
            }
            log.LogMethodExit(siteId);
            return siteId;
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating siteDTO Record.
        /// </summary>
        /// <param name="siteDTO">siteDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(SiteDTO siteDTO, string loginId)
        {
            log.LogMethodEntry(siteDTO, loginId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteDTO.SiteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_name", siteDTO.SiteName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteAddress", siteDTO.SiteAddress));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notes", siteDTO.Notes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteGuid", siteDTO.SiteGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrgId", siteDTO.OrgId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", siteDTO.MasterEntityId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUploadMessage", siteDTO.LastUploadMessage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InitialLoadDone", siteDTO.InitialLoadDone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MaxCards", siteDTO.MaxCards));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Version", siteDTO.Version));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerKey", siteDTO.CustomerKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CompanyId", siteDTO.CompanyId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteCode", siteDTO.SiteCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteShortName", siteDTO.SiteShortName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OnlineEnabled", siteDTO.OnlineEnabled));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Description", siteDTO.Description));
            parameters.Add(dataAccessHandler.GetSQLParameter("@City", siteDTO.City));
            parameters.Add(dataAccessHandler.GetSQLParameter("@State", siteDTO.State));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Country", siteDTO.Country));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PinCode", siteDTO.PinCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OpenTime", siteDTO.OpenTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CloseTime", siteDTO.CloseTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteURL", siteDTO.SiteURL));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Size", siteDTO.Size));
            parameters.Add(dataAccessHandler.GetSQLParameter("@StoreType", siteDTO.StoreType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@AboutUs", siteDTO.AboutUs));
            parameters.Add(dataAccessHandler.GetSQLParameter("@storeRanking", siteDTO.StoreRanking));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalSourceReference", siteDTO.ExternalSourceReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Email", siteDTO.Email));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PhoneNumber", siteDTO.PhoneNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@latitude", siteDTO.Latitude));
            parameters.Add(dataAccessHandler.GetSQLParameter("@longitude", siteDTO.Longitude));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteConfigComplete", siteDTO.SiteConfigComplete));
            parameters.Add(dataAccessHandler.GetSQLParameter("@active_flag", siteDTO.IsActive ? 'Y' : 'N'));
            /// The database field is Logo type so if image is null then need add the db type and send the db null value
            if (siteDTO.Logo == null || siteDTO.Logo == (byte[])null)
            {
                SqlParameter imageParameter = new SqlParameter("@Logo", SqlDbType.VarBinary)
                {
                    Value = DBNull.Value
                };
                parameters.Add(imageParameter);
            }
            else
            {
                parameters.Add(dataAccessHandler.GetSQLParameter("@Logo", siteDTO.Logo));
                
            }
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// Inserts the site record
        /// </summary>
        /// <param name="siteDTO">SiteDTO</param>
        /// <param name="loginId">User inserting the record</param>        
        /// <returns>Returns inserted record id</returns>
        public SiteDTO InsertSite(SiteDTO siteDTO, string loginId)
        {
            log.LogMethodEntry(siteDTO, loginId);
                string insertSiteQuery = @"insert into site 
                                                            ( site_id,
                                                              site_name,
                                                              site_address, 
                                                              notes,
                                                              site_guid,                                                              
                                                              Guid, 
                                                              OrgId,
                                                              last_upload_time, 
                                                              last_upload_server_time, 
                                                              last_upload_message,
                                                              InitialLoadDone,  
                                                              MaxCards,
                                                              Version,
                                                              CustomerKey,
                                                              Company_Id,
                                                              SiteCode,
                                                              CreatedBy,
                                                              CreationDate,
                                                              LastUpdatedBy,
                                                              LastUpdateDate,
                                                              SiteShortName,
                                                              OnlineEnabled,
                                                              Description,
                                                              OpenDate,
                                                              ClosureDate,
                                                              City,
                                                              State,
                                                              Country,
                                                              PinCode,
                                                              OpenTime,
                                                              CloseTime,
                                                              SiteURL,
                                                              Size,
                                                              StoreType,
                                                              AboutUs,
                                                              logo,
                                                              active_flag ,
                                                              StoreRanking,
                                                              ExternalSourceReference,
                                                              Email,
                                                              PhoneNumber,
                                                              Latitude,
                                                              Longitude,
                                                              SiteConfigComplete
                                                            ) 
                                                    values 
                                                            ( @SiteId,
                                                              @site_name,
                                                              @SiteAddress, 
                                                              @notes,
                                                              @SiteGuid,                                                              
                                                              NEWID(), 
                                                              @OrgId,
                                                              GETDATE(), 
                                                              GETDATE(), 
                                                              @LastUploadMessage,
                                                              @InitialLoadDone,    
                                                              @MaxCards,
                                                              @Version,
                                                              @CustomerKey,
                                                              @CompanyId,
                                                              @SiteCode,
                                                              @CreatedBy,
                                                              GETDATE(),
                                                              @LastUpdatedBy,
                                                              GETDATE(),
                                                              @SiteShortName,
                                                              @OnlineEnabled,
                                                              @Description,
                                                              GETDATE(),
                                                              GETDATE(),                                                             
                                                              @City,
                                                              @State,
                                                              @Country,
                                                              @PinCode,
                                                              @OpenTime,
                                                              @CloseTime,                                                              
                                                              @SiteURL,
                                                              @Size,
                                                              @StoreType,
                                                              @AboutUs,
                                                              @Logo,
                                                              @active_flag ,                                                             
                                                              @storeRanking,                                                              
                                                              @ExternalSourceReference,
                                                              @Email,
                                                              @PhoneNumber,
                                                              @latitude,
                                                              @longitude,
                                                              @SiteConfigComplete
                                                            )SELECT * FROM site WHERE site_id = @SiteId";


                try
                {
                    DataTable dt = dataAccessHandler.executeSelectQuery(insertSiteQuery, GetSQLParameters(siteDTO, loginId).ToArray(), sqlTransaction);
                    RefreshSiteDTO(siteDTO, dt);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while inserting siteDTO", ex);
                    log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                    throw;
                }
                log.LogMethodExit(siteDTO);
                return siteDTO;
            }



        /// Updates the site record
        /// </summary>
        /// <param name="site">siteDTO</param>
        /// <param name="loginId">User Updating the record</param>        
        /// <returns>Returns Updated record id</returns>
        public SiteDTO UpdateSite(SiteDTO siteDTO, string loginId)
        {
            log.LogMethodEntry(siteDTO, loginId);
              string updateSiteQuery = @"update site 
                                         set site_name = @site_name,
                                                              site_address = @SiteAddress, 
                                                              notes = @Notes,
                                                              site_guid = @SiteGuid,
                                                              OrgId = @OrgId,
                                                              -- last_upload_time = GETDATE(), 
                                                              -- last_upload_server_time = GETDATE(), 
                                                              last_upload_message = @LastUploadMessage,
                                                              InitialLoadDone = @InitialLoadDone,   
                                                              MaxCards = @MaxCards,
                                                              Version = @Version,
                                                              CustomerKey = @CustomerKey,
                                                              Company_Id = @CompanyId,
                                                              SiteCode = @SiteCode,                                                              
                                                              LastUpdatedBy = @lastUpdatedBy,
                                                              LastUpdateDate = GETDATE(),
                                                              SiteShortName = @SiteShortName,
                                                              OnlineEnabled = @OnlineEnabled,
                                                              Description = @Description,
                                                              -- OpenDate = getdate(),
                                                              -- ClosureDate = GETDATE(),
                                                              City = @City,
                                                              State = @State,
                                                              Country = @Country,
                                                              PinCode = @PinCode,
                                                              OpenTime = @OpenTime,
                                                              CloseTime = @CloseTime,
                                                              SiteURL = @SiteURL,
                                                              Size = @Size,
                                                              StoreType = @StoreType,
                                                              AboutUs = @AboutUs,
                                                              logo = @Logo,
                                                              StoreRanking = @storeRanking,
                                                              ExternalSourceReference = @ExternalSourceReference,
                                                              Email = @Email,
                                                              PhoneNumber = @PhoneNumber,
                                                              Latitude = @latitude,
                                                              Longitude = @longitude,
                                                              active_flag = @active_flag,
                                                              SiteConfigComplete=@SiteConfigComplete
                                       where site_id = @SiteId
                            SELECT * FROM site WHERE site_id = @SiteId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateSiteQuery, GetSQLParameters(siteDTO, loginId).ToArray(), sqlTransaction);
                RefreshSiteDTO(siteDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating siteDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(siteDTO);
            return siteDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="siteDTO">siteDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshSiteDTO(SiteDTO siteDTO, DataTable dt)
        {
            log.LogMethodEntry(siteDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                siteDTO.SiteId = Convert.ToInt32(dt.Rows[0]["site_id"]);
                siteDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                siteDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                siteDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                siteDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                siteDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the site data of passed site id
        /// </summary>
        /// <param name="siteId">integer type parameter</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        /// <returns>Returns SiteDTO</returns>
        public SiteDTO GetMasterSiteFromHQ()
        {
            log.LogMethodEntry();
            SiteDTO siteDataObject = null;
            string selectSiteQuery = @"select top 1 *
                                         from Site
                                        where site_id = (Select Top 1 master_site_id from company)";
            DataTable site = dataAccessHandler.executeSelectQuery(selectSiteQuery, new SqlParameter[0], sqlTransaction);
            if (site != null && site.Rows.Count > 0)
            {
                DataRow siteRow = site.Rows[0];
                siteDataObject = GetSiteDTO(siteRow);
            }
            log.LogMethodExit(siteDataObject);
            return siteDataObject;
        }

        internal DateTime? GetSiteLastUpdateTime()
        {
            log.LogMethodEntry();
            string query = @"select max(LastUpdateDate) LastUpdateDate from Site";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdateDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdateDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Inserts the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="functionalGuid">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void AddManagementFormAccess(string formName, string functionGuid, int siteId,bool isActive)
        {
            log.LogMethodEntry(formName, functionGuid, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Sites',@formName,'Data Access',@siteId,@functionGuid,@isActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", isActive));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Rename the managementFormAccess record to the database
        /// </summary>
        /// <param name="newFormName">string type object</param>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void RenameManagementFormAccess(string newFormName, string formName, int siteId, string functionGuid)
        {
            log.LogMethodEntry(newFormName, formName, siteId);
            string query = @"exec RenameManagementFormAccess @newFormName,'Sites',@formName,'Data Access',@siteId,@functionGuid";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@newFormName", newFormName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
        /// <summary>
        /// Update the managementFormAccess record to the database
        /// </summary>
        /// <param name="formName">string type object</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="updatedIsActive">Site to which the record belongs</param>
        /// <param name="functionGuid">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public void UpdateManagementFormAccess(string formName, int siteId, bool updatedIsActive, string functionGuid)
        {
            log.LogMethodEntry(formName, siteId);
            string query = @"exec InsertOrUpdateManagementFormAccess 'Sites',@formName,'Data Access',@siteId,@functionGuid,@updatedIsActive";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@formName", formName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@updatedIsActive", updatedIsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@functionGuid", functionGuid));
            dataAccessHandler.executeScalar(query, parameters.ToArray(), sqlTransaction);
            log.LogMethodExit();
        }
    }
}
