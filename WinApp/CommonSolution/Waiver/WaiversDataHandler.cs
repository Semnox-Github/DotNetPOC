/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data handler - WaiverSetDetailDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        01-Jul-2019   Girish Kundar     Modified : For SQL Injection Issue.   
 *2.70.2      03-Oct-2019      Girish Kundar    Waiver phase 2 changes
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  WaiverSetDetail Data Handler - Handles insert, update and select of  WaiverSetDetail objects
    /// </summary>
    public class WaiversDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<WaiversDTO.SearchByWaivers, string> DBSearchParameters = new Dictionary<WaiversDTO.SearchByWaivers, string>
            {
                {WaiversDTO.SearchByWaivers.WAIVERSETDETAIL_ID, "wsd.WaiverSetDetailId"},
                 {WaiversDTO.SearchByWaivers.WAIVERSETDETAIL_ID_LIST, "wsd.WaiverSetDetailId"},
                {WaiversDTO.SearchByWaivers.WAIVERSET_ID, "wsd.WaiverSetId"},
                 {WaiversDTO.SearchByWaivers.WAIVERSET_ID_LIST, "wsd.WaiverSetId"},
                {WaiversDTO.SearchByWaivers.NAME, "wsd.Name"},
                {WaiversDTO.SearchByWaivers.WAIVER_FILENAME, "wsd.WaiverFileName"},
                {WaiversDTO.SearchByWaivers.VALID_FOR_DAYS, "wsd.ValidForDays"},
                {WaiversDTO.SearchByWaivers.EFFECTIVE_DATE, "wsd.EffectiveDate"},
                {WaiversDTO.SearchByWaivers.IS_ACTIVE, "wsd.IsActive"},
                {WaiversDTO.SearchByWaivers.LAST_UPDATED_DATE, "wsd.LastUpdatedDate"},
                {WaiversDTO.SearchByWaivers.LAST_UPDATED_BY, "wsd.LastUpdatedBy"},
                {WaiversDTO.SearchByWaivers.GUID, "wsd.GUID"},
                {WaiversDTO.SearchByWaivers.SITE_ID, "wsd.Site_id"},
                {WaiversDTO.SearchByWaivers.SYNCH_STATUS, "wsd.SynchStatus"},
                {WaiversDTO.SearchByWaivers.MASTER_ENTITY_ID, "wsd.MasterEntityId"},
                  {WaiversDTO.SearchByWaivers.SIGNING_OPTION_IS_SET, ""}
            };
        private const string SELECT_QUERY = @"SELECT * FROM WaiverSetDetails As wsd ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of WaiverSetDataHandler class
        /// </summary>
        public WaiversDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WaiverSetDetail Record.
        /// </summary>
        /// <param name="waiverSetDetailDTO">WaiverSetDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(WaiversDTO waiverSetDetailDTO, string loginId, int siteId)
        { 
            log.LogMethodEntry(waiverSetDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@WaiverSetDetailId", waiverSetDetailDTO.WaiverSetDetailId, true);
            ParametersHelper.ParameterHelper(parameters, "@WaiverSetId", waiverSetDetailDTO.WaiverSetId, true);
            ParametersHelper.ParameterHelper(parameters, "@Name", waiverSetDetailDTO.Name);
            ParametersHelper.ParameterHelper(parameters, "@WaiverFileName", waiverSetDetailDTO.WaiverFileName);
            ParametersHelper.ParameterHelper(parameters, "@ValidForDays", waiverSetDetailDTO.ValidForDays <= 0 ? -1 : waiverSetDetailDTO.ValidForDays, true);
            ParametersHelper.ParameterHelper(parameters, "@EffectiveDate", waiverSetDetailDTO.EffectiveDate);
            ParametersHelper.ParameterHelper(parameters, "@IsActive", waiverSetDetailDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@Site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@MasterEntityId", waiverSetDetailDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }


        /// <summary>
        /// Inserts the WaiverSetDetail record to the database
        /// </summary>
        /// <param name="waiverSetDetailDTO">WaiverSeDetailDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetDetailDTO</returns>
        public WaiversDTO InsertWaiverSetDetail(WaiversDTO waiverSetDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDetailDTO, loginId, siteId);
            string query = @"INSERT INTO WaiverSetDetails 
                                        ( 
                                            WaiverSetId,
                                            Name,
                                            WaiverFileName,
                                            ValidForDays,
                                            EffectiveDate,
                                            IsActive,
                                            CreationDate,
                                            CreatedBy,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            GUID,
                                            Site_id,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @WaiverSetId,
                                            @Name,
                                            @WaiverFileName,
                                            @ValidForDays,
                                            @EffectiveDate,
                                            @IsActive,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @Site_id,
                                            @MasterEntityId
                                           )SELECT  * from WaiverSetDetails where WaiverSetDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiversDTO(waiverSetDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WaiverSetDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDetailDTO);
            return waiverSetDetailDTO;
        }


        /// <summary>
        /// Updates the WaiverSetDetail record
        /// </summary>
        /// <param name="waiverSetDetailDTO">WaiverSetDetailDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetDetailDTO</returns>
        public WaiversDTO UpdateWaiverSetDetail(WaiversDTO waiverSetDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDetailDTO, loginId, siteId);
            string query = @"UPDATE WaiverSetDetails 
                             SET 
                                            WaiverSetId = @WaiverSetId,
                                            Name = @Name,
                                            WaiverFileName = @WaiverFileName,
                                            ValidForDays = @ValidForDays,
                                            EffectiveDate = @EffectiveDate,
                                            IsActive = @IsActive,
                                            LastUpdatedDate =   GETDATE(),
                                            LastUpdatedBy = @LastUpdatedBy
                                            -- Site_id = @Site_id
                             WHERE WaiverSetDetailId = @WaiverSetDetailId 
                             SELECT  * from WaiverSetDetails where WaiverSetDetailId = @WaiverSetDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiversDTO(waiverSetDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WaiverSetDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDetailDTO);
            return waiverSetDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="waiversDTO">waiversDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshWaiversDTO(WaiversDTO waiversDTO, DataTable dt)
        {
            log.LogMethodEntry(waiversDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                waiversDTO.WaiverSetDetailId = Convert.ToInt32(dt.Rows[0]["WaiverSetDetailId"]);
                waiversDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                waiversDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                waiversDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                waiversDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                waiversDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                waiversDTO.Site_id = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to WaiversDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns WaiverSetDetailDTO</returns>
        private WaiversDTO GetWaiversDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WaiversDTO waiversDTO = new WaiversDTO(Convert.ToInt32(dataRow["WaiverSetDetailId"]),
                                                dataRow["WaiverSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WaiverSetId"]),
                                                dataRow["Name"] == DBNull.Value ? "" : Convert.ToString(dataRow["Name"]),
                                                dataRow["WaiverFileName"] == DBNull.Value ? "" : Convert.ToString(dataRow["WaiverFileName"]),
                                                dataRow["ValidForDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ValidForDays"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"]),
                                                dataRow["EffectiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                dataRow["CreatedBy"].ToString(),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["GUID"] == DBNull.Value ? "" : Convert.ToString(dataRow["GUID"]),
                                                dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                );
            log.LogMethodExit(waiversDTO);
            return waiversDTO;
        }


        /// <summary>
        /// Gets the WaiverSetDetail data of passed WaiverSetDetail Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns WaiversDTO</returns>
        public WaiversDTO GetWaiversDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id, sqlTransaction);
            WaiversDTO returnValue = null;
            string query = @"SELECT *
                            FROM WaiverSetDetails
                            WHERE WaiverSetDetailId = @WaiverSetDetailId";
            SqlParameter parameter = new SqlParameter("@WaiverSetDetailId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetWaiversDTO(dataTable.Rows[0]); 
            }
            log.LogMethodExit(returnValue); 
            return returnValue;
        }


        /// <summary>
        /// Gets the WaiversDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of WaiversDTO matching the search criteria</returns>
        public List<WaiversDTO> GetWaiversDTOList(List<KeyValuePair<WaiversDTO.SearchByWaivers, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<WaiversDTO> list = null;
            int count = 0;
            //string selectQuery = @"SELECT * FROM WaiverSetDetails where WaiverSetId = " + waiverId;
            string selectQuery =SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<WaiversDTO.SearchByWaivers, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == WaiversDTO.SearchByWaivers.WAIVERSETDETAIL_ID
                            || searchParameter.Key == WaiversDTO.SearchByWaivers.WAIVERSET_ID 
                            || searchParameter.Key == WaiversDTO.SearchByWaivers.MASTER_ENTITY_ID
                            || searchParameter.Key == WaiversDTO.SearchByWaivers.VALID_FOR_DAYS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiversDTO.SearchByWaivers.WAIVERSET_ID_LIST 
                            || searchParameter.Key == WaiversDTO.SearchByWaivers.WAIVERSETDETAIL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value)); 
                        } 
                        else if (searchParameter.Key == WaiversDTO.SearchByWaivers.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiversDTO.SearchByWaivers.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == WaiversDTO.SearchByWaivers.EFFECTIVE_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",DATEADD(minute, -5,getdate())) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == WaiversDTO.SearchByWaivers.SIGNING_OPTION_IS_SET)
                        {
                            query.Append(joiner + @"ISNULL((select top 1 1 
                                                              from WaiverSetSigningOptions wsso
                                                             where wsso.WaiverSetId = wsd.WaiverSetId),0) = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        count++;
                    }
                    else
                    {
                        log.LogMethodExit(null, "throwing exception");
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<WaiversDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WaiversDTO waiversDTO = GetWaiversDTO(dataRow);
                    list.Add(waiversDTO);
                } 
            } 
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// Checks whether WaiverSetDetail is in use.
        /// <param name="id">WaiverSetDetail Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetWaiverSetDetailReferenceCount(int id)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"SELECT 
                            (
                            SELECT COUNT(1) 
                            FROM WaiverSetDetails
                            WHERE WaiverSetDetailId = @WaiverSetDetailId
                            AND IsActive = 'Y' 
                            )
                            
                            AS ReferenceCount";
            SqlParameter parameter = new SqlParameter("@WaiverSetDetailId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }




    }

}

