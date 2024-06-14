/********************************************************************************************
 * Project Name - Ad Broadcast Data Handler
 * Description  - Data handler of the Ad Broadcast handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        17-May-2019   Jagan Mohana Rao        Created 
 *2.70.2      26-Jan-2020    Girish Kundar           Modified : Changed to Standard format 
 *2.90       20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API. 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.ServerCore
{
    public class AdBroadcastDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM AdBroadcast adb ";
        private static readonly Dictionary<AdBroadcastDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<AdBroadcastDTO.SearchByParameters, string>
               {
                    {AdBroadcastDTO.SearchByParameters.ID, "adb.Id"},
                    {AdBroadcastDTO.SearchByParameters.BROADCAST_FILE_NAME, "adb.BroadcastFileName"},
                    {AdBroadcastDTO.SearchByParameters.AD_ID, "adb.AdId"},
                    {AdBroadcastDTO.SearchByParameters.AD_ID_LIST, "adb.AdId"},
                    {AdBroadcastDTO.SearchByParameters.MASTER_ENTITY_ID, "adb.MasterEntityId"},
                    {AdBroadcastDTO.SearchByParameters.SITE_ID,"adb.site_id"},
                    {AdBroadcastDTO.SearchByParameters.ISACTIVE,"adb.IsActive"}
               };
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of AdBroadcastDataHandler class
        /// </summary>
        public AdBroadcastDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating AdBroadcast Record.
        /// </summary>
        /// <param name="adBroadcastDTO">AdBroadcastDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(AdBroadcastDTO adBroadcastDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(adBroadcastDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", adBroadcastDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@broadCastFileName", adBroadcastDTO.BroadCastFileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@broadcastBeginTime", adBroadcastDTO.BroadcastBeginTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@broadcastEndTime", adBroadcastDTO.BroadcastEndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@adId", adBroadcastDTO.AdId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@machineId", adBroadcastDTO.MachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", adBroadcastDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", adBroadcastDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", adBroadcastDTO.IsActive));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the AdBroadcast record to the database
        /// </summary>
        /// <param name="adBroadcastDTO">AdBroadcastDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public AdBroadcastDTO Insert(AdBroadcastDTO adBroadcastDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(adBroadcastDTO, loginId, siteId);
            string insertAdBroadcastQuery = @"insert into AdBroadcast 
                                                        (
                                                        BroadcastFileName,
                                                        BroadcastBeginTime,
                                                        BroadcastEndTime,
                                                        AdId,
                                                        MachineId,
                                                        Guid,
                                                        site_id,
                                                       -- SynchStatus,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        IsActive
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @broadcastFileName,
                                                        @broadcastBeginTime,
                                                        @broadcastEndTime,
                                                        @adId,
                                                        @machineId,
                                                        NewId(),
                                                        @site_id,
                                                      --  @synchStatus,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        getDate(),
                                                        @lastUpdatedBy,
                                                        getDate(),
                                                        @isActive
                                                        )
                                            SELECT * FROM AdBroadcast WHERE Id = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertAdBroadcastQuery, GetSQLParameters(adBroadcastDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAdBroadcastDTO(adBroadcastDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting adBroadcastDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(adBroadcastDTO);
            return adBroadcastDTO;
        }
        /// <summary>
        /// Updates the AdBroadcast record
        /// </summary>
        /// <param name="adBroadcastDTO">AdBroadcastDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public AdBroadcastDTO Update(AdBroadcastDTO adBroadcastDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(adBroadcastDTO, loginId, siteId);
            string updateAdBroadcastsQuery = @"update AdBroadcast 
                                         set BroadcastFileName=@broadcastFileName,
                                             BroadcastBeginTime=@broadcastBeginTime,
                                             BroadcastEndTime=@broadcastEndTime,
                                             AdId=@adId,
                                             MachineId = @machineId,                                             
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastUpdateDate = Getdate(),
                                             IsActive = @isActive
                                       where Id = @id 
                                     SELECT * FROM AdBroadcast WHERE Id = @id  ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateAdBroadcastsQuery, GetSQLParameters(adBroadcastDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshAdBroadcastDTO(adBroadcastDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating adBroadcastDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(adBroadcastDTO);
            return adBroadcastDTO;
        }

        private void RefreshAdBroadcastDTO(AdBroadcastDTO adBroadcastDTO, DataTable dt)
        {
            log.LogMethodEntry(adBroadcastDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                adBroadcastDTO.AdId = Convert.ToInt32(dt.Rows[0]["Id"]);
                adBroadcastDTO.LastupdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                adBroadcastDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                adBroadcastDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                adBroadcastDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                adBroadcastDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                adBroadcastDTO.Siteid = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Based on the id, appropriate ads record will be deleted
        /// This is for hard deletion. In future, when we implement soft deletion this method may not be required
        /// </summary>
        /// <param name="id">primary key of adId </param>
        /// <returns>return the int </returns>
        public int Delete(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            try
            {
                string adBroadcastQuery = @"delete from AdBroadcast where Id = @id";
                SqlParameter[] adBroadcastParameters = new SqlParameter[1];
                adBroadcastParameters[0] = new SqlParameter("@id", id);
                int deleteStatus = dataAccessHandler.executeUpdateQuery(adBroadcastQuery, adBroadcastParameters, sqlTransaction);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                System.Console.Write(expn.Message.ToString());
                throw new System.Exception(expn.Message.ToString());
            }
        }
        /// <summary>
        /// Converts the Data row object to AdBroadcastDTO class type
        /// </summary>
        /// <param name="adBroadcastDataRow">AdBroadcastDTO DataRow</param>
        /// <returns>Returns AdBroadcastDTO</returns>
        private AdBroadcastDTO GetAdBroadcastDTO(DataRow adBroadcastDataRow)
        {
            log.LogMethodEntry(adBroadcastDataRow);
            AdBroadcastDTO adBroadcastDataObject = new AdBroadcastDTO(Convert.ToInt32(adBroadcastDataRow["Id"]),
                                            adBroadcastDataRow["BroadcastFileName"] == DBNull.Value ? string.Empty : Convert.ToString(adBroadcastDataRow["BroadcastFileName"]),
                                            adBroadcastDataRow["BroadcastBeginTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adBroadcastDataRow["BroadcastBeginTime"]),
                                            adBroadcastDataRow["BroadcastEndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adBroadcastDataRow["BroadcastEndTime"]),
                                            adBroadcastDataRow["AdId"] == DBNull.Value ? -1 : Convert.ToInt32(adBroadcastDataRow["AdId"]),
                                            adBroadcastDataRow["MachineId"] == DBNull.Value ? -1 : Convert.ToInt32(adBroadcastDataRow["MachineId"]),
                                            adBroadcastDataRow["Guid"].ToString(),
                                            adBroadcastDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(adBroadcastDataRow["site_id"]),
                                            adBroadcastDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(adBroadcastDataRow["SynchStatus"]),
                                            adBroadcastDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(adBroadcastDataRow["MasterEntityId"]),
                                            adBroadcastDataRow["CreatedBy"].ToString(),
                                            adBroadcastDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adBroadcastDataRow["CreationDate"]),
                                            adBroadcastDataRow["LastUpdatedBy"].ToString(),
                                            adBroadcastDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(adBroadcastDataRow["LastUpdateDate"]),
                                           adBroadcastDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(adBroadcastDataRow["IsActive"])
                                            );
            log.LogMethodExit(adBroadcastDataObject);
            return adBroadcastDataObject;
        }

        /// <summary>
        /// Gets the Ads data of passed adId id
        /// </summary>
        /// <param name="adId">integer type parameter</param>
        /// <returns>Returns AdBroadcastDTO</returns>
        public AdBroadcastDTO GetAdBroadcast(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            string selectAdBroadcastQuery = SELECT_QUERY + " where Id = @id";
            AdBroadcastDTO adBroadcastDataObject = new AdBroadcastDTO();
            SqlParameter[] selectAdBroadcastParameters = new SqlParameter[1];
            selectAdBroadcastParameters[0] = new SqlParameter("@id", id);
            DataTable adBroadcast = dataAccessHandler.executeSelectQuery(selectAdBroadcastQuery, selectAdBroadcastParameters.ToArray(), sqlTransaction);
            if (adBroadcast.Rows.Count > 0)
            {
                DataRow adBroadcastRow = adBroadcast.Rows[0];
                adBroadcastDataObject = GetAdBroadcastDTO(adBroadcastRow);
            }
            log.LogMethodExit(adBroadcastDataObject);
            return adBroadcastDataObject;
        }

        /// <summary>
        /// Gets the AdBroadcastDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AdBroadcastDTO matching the search criteria</returns>
        public List<AdBroadcastDTO> GetAdBroadcastList(List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<AdBroadcastDTO> adBroadcastList = new List<AdBroadcastDTO>();
            string selectAdBroadcastQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<AdBroadcastDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == AdBroadcastDTO.SearchByParameters.ID
                            || searchParameter.Key == AdBroadcastDTO.SearchByParameters.AD_ID
                            || searchParameter.Key == AdBroadcastDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AdBroadcastDTO.SearchByParameters.AD_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == AdBroadcastDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == AdBroadcastDTO.SearchByParameters.ISACTIVE) // column to be added
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectAdBroadcastQuery = selectAdBroadcastQuery + query;
            }

            DataTable adBroadcastData = dataAccessHandler.executeSelectQuery(selectAdBroadcastQuery, parameters.ToArray(), sqlTransaction);
            if (adBroadcastData.Rows.Count > 0)
            {
                foreach (DataRow adBroadcastDataRow in adBroadcastData.Rows)
                {
                    AdBroadcastDTO adsDataObject = GetAdBroadcastDTO(adBroadcastDataRow);
                    adBroadcastList.Add(adsDataObject);
                }
            }
            log.LogMethodExit(adBroadcastList);
            return adBroadcastList;
        }

        /// <summary>
        /// Gets the AdBroadcastDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of AdBroadcastDTO matching the search criteria</returns>
        public List<AdBroadcastDTO> GetAllPopulateAdBroadcast(List<KeyValuePair<AdBroadcastDTO.SearchByParameters, string>> searchParameters,
                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            int site_Id = -1;
            int adId = -1;
            List<AdBroadcastDTO> adBroadcastList = new List<AdBroadcastDTO>();
            string selectAdBroadcastQuery = @"select AdImageFileSystem, null, null, AdId, machine_id, machines.site_id 
                                            from Ads, machines 
                                            where AdId = @id 
                                            and (machines.site_id = @site_id or @site_id = -1) 
                                            and not exists (select 1 from AdBroadcast adb where adb.AdId = Ads.AdId 
											and adb.machineId = machines.machine_id
                                                            and BroadcastFileName = AdImageFileSystem)";

            List<SqlParameter> parameters = new List<SqlParameter>();
            if (searchParameters.Where(m => m.Key.Equals(AdBroadcastDTO.SearchByParameters.SITE_ID)).FirstOrDefault().Value != string.Empty)
            {
                site_Id = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(AdBroadcastDTO.SearchByParameters.SITE_ID)).FirstOrDefault().Value);
            }
            if (searchParameters.Where(m => m.Key.Equals(AdBroadcastDTO.SearchByParameters.AD_ID)).FirstOrDefault().Value != string.Empty)
            {
                adId = Convert.ToInt32(searchParameters.Where(m => m.Key.Equals(AdBroadcastDTO.SearchByParameters.AD_ID)).FirstOrDefault().Value);
            }
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", site_Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", adId, true));

            DataTable adBroadcastData = dataAccessHandler.executeSelectQuery(selectAdBroadcastQuery, parameters.ToArray(), sqlTransaction);
            if (adBroadcastData.Rows.Count > 0)
            {
                foreach (DataRow adBroadcastDataRow in adBroadcastData.Rows)
                {
                    AdBroadcastDTO adsDataObject = GetAdBroadcastDTO(adBroadcastDataRow);
                    adBroadcastList.Add(adsDataObject);
                }
            }
            log.LogMethodExit(adBroadcastList);
            return adBroadcastList;
        }
    }
}