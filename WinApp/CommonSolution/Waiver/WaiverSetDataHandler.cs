/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data handler - WaiverSetDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        06-Feb-2017   Lakshminarayana   Created 
 *2.70        01-Jul-2019   Girish Kundar     Modified : For SQL Injection Issue.   
 *2.70.2        23-Sep-2019   Deeksha           Waiver phase 2 changes
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.130.0     28-Jul-2020   Mushahid Faizan    Modified : POS UI redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  WaiverSet Data Handler - Handles insert, update and select of  WaiverSet objects
    /// </summary>
    public class WaiverSetDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string SELECT_QUERY = @"SELECT * FROM WaiverSet AS ws ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        private static readonly Dictionary<WaiverSetDTO.SearchByWaiverParameters, string> DBSearchParameters = new Dictionary<WaiverSetDTO.SearchByWaiverParameters, string>
            {
                {WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID, "ws.WaiverSetId"},
                {WaiverSetDTO.SearchByWaiverParameters.NAME, "ws.Name"},
                {WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "ws.IsActive"},
                {WaiverSetDTO.SearchByWaiverParameters.LAST_UPDATED_DATE, "ws.LastUpdatedDate"},
                {WaiverSetDTO.SearchByWaiverParameters.LAST_UPDATED_BY, "ws.LastUpdatedBy"},
                {WaiverSetDTO.SearchByWaiverParameters.GUID , "ws.GUID"},
                {WaiverSetDTO.SearchByWaiverParameters.SITE_ID, "ws.Site_id"},
                {WaiverSetDTO.SearchByWaiverParameters.SYNCH_STATUS, "ws.SynchStatus"},
                {WaiverSetDTO.SearchByWaiverParameters.MASTER_ENTITY_ID, "ws.MasterEntityId"},
                {WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID_LIST, "ws.WaiverSetId"}
            };

        /// <summary>
        /// Default constructor of WaiverSetDataHandler class
        /// </summary>
        public WaiverSetDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();

        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating WaiverSet Record.
        /// </summary>
        /// <param name="waiverSetDTO">WaiverSetDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(WaiverSetDTO waiverSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@WaiverSetId", waiverSetDTO.WaiverSetId, true);
            ParametersHelper.ParameterHelper(parameters, "@Name", waiverSetDTO.Name);
            ParametersHelper.ParameterHelper(parameters, "@IsActive", waiverSetDTO.IsActive);
            ParametersHelper.ParameterHelper(parameters, "@LastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@Site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@MasterEntityId", waiverSetDTO.MasterEntityId, true);
            ParametersHelper.ParameterHelper(parameters, "@Description", waiverSetDTO.Description);
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the WaiverSet record to the database
        /// </summary>
        /// <param name="waiverSetDTO">WaiverSetDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetDTO object</returns>
        public WaiverSetDTO InsertWaiverSet(WaiverSetDTO waiverSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDTO, loginId, siteId);
            string query = @"INSERT INTO WaiverSet 
                                        ( 
                                            Name,
                                            IsActive,
                                            CreationDate,
                                            CreatedBy,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            GUID,
                                            Site_id,
                                            MasterEntityId,
                                            Description
                                        ) 
                                VALUES 
                                        (
                                            @Name,
                                            @IsActive,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            NewId(),
                                            @Site_id,
                                            @MasterEntityId,
                                            @Description
                                        )SELECT  * from WaiverSet where WaiverSetId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetDTO(waiverSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WaiverSetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDTO);
            return waiverSetDTO;
        }

        /// <summary>
        /// Updates the WaiverSet record
        /// </summary>
        /// <param name="waiverSetDTO">WaiverSetDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the WaiverSetDTO</returns>
        ///  //MasterEntityId = @MasterEntityId

        public WaiverSetDTO UpdateWaiverSet(WaiverSetDTO waiverSetDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDTO, loginId, siteId);
            string query = @"UPDATE WaiverSet 
                             SET 
                                            Name = @Name,
                                            IsActive = @IsActive,
                                            LastUpdatedDate =   GETDATE(),
                                            LastUpdatedBy = @LastUpdatedBy,
                                            -- Site_id = @Site_id,
                                            Description = @Description
                             WHERE WaiverSetId = @WaiverSetId
                             SELECT  * from WaiverSet where WaiverSetId = @WaiverSetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetDTO(waiverSetDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WaiverSetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDTO);
            return waiverSetDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="waiverSetDTO">WaiverSetDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshWaiverSetDTO(WaiverSetDTO waiverSetDTO, DataTable dt)
        {
            log.LogMethodEntry(waiverSetDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                waiverSetDTO.WaiverSetId = Convert.ToInt32(dt.Rows[0]["WaiverSetId"]);
                waiverSetDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                waiverSetDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                waiverSetDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                waiverSetDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                waiverSetDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                waiverSetDTO.Site_id = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to WaiverSetDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns WaiverSetDTO</returns>
        private WaiverSetDTO GetWaiverSetDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WaiverSetDTO waiverSetDTO = new WaiverSetDTO(Convert.ToInt32(dataRow["WaiverSetId"]),
                                            dataRow["Name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Name"]),
                                            dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"].ToString(),
                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["GUID"] == DBNull.Value ? "" : Convert.ToString(dataRow["GUID"]),
                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["Description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Description"])
                                            );
            log.LogMethodExit(waiverSetDTO);
            return waiverSetDTO;
        }
        /// <summary>
        /// Gets the WaiverSet data of passed WaiverSet Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns WaiverSetDTO</returns>
        public WaiverSetDTO GetWaiverSetDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            WaiverSetDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE ws.WaiverSetId = @WaiverSetId";
            SqlParameter parameter = new SqlParameter("@WaiverSetId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetWaiverSetDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// Gets the WaiverSetDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of WaiverSetDTO matching the search criteria</returns>
        public List<WaiverSetDTO> GetWaiverSetDTOList(List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<WaiverSetDTO> waiverSetDTOList = new List<WaiverSetDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID
                           || searchParameter.Key == WaiverSetDTO.SearchByWaiverParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == WaiverSetDTO.SearchByWaiverParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }

                        else if (searchParameter.Key == WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
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
                waiverSetDTOList = new List<WaiverSetDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WaiverSetDTO waiverSetDTO = GetWaiverSetDTO(dataRow);
                    waiverSetDTOList.Add(waiverSetDTO);
                }
            }
            log.LogMethodExit(waiverSetDTOList);
            return waiverSetDTOList;
        }

        /// <summary>
        /// Checks whether WaiverSet is in use.
        /// <param name="id">WaiverSet Id</param>
        /// </summary>
        /// <returns>Returns refrenceCount</returns>
        public int GetWaiverSetReferenceCount(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            int refrenceCount = 0;
            string query = @"select count(distinct pId) as ReferenceCount
                            from (
                            SELECT p.Product_Id as pId
                              FROM Products p
                             WHERE WaiverSetId = @WaiverSetId
                               AND active_flag = 'Y'
                            union all
                            select p.Product_Id as pId
                              from FacilityWaiver fw,
                                   FacilityMapDetails fmd,
                                   ProductsAllowedInFacility paf,
                                   Products p
                               where fw.WaiverSetId = @WaiverSetId
                                 and fw.FacilityId = fmd.FacilityId
                                 and fmd.IsActive = 1
                                 and fmd.FacilityMapId = paf.FacilityMapId
                                 and paf.IsActive = 1
                                 and paf.ProductsId = p.product_id
                                 and p.active_flag = 'Y'
								 and ISNULL(fw.EffectiveFrom,getdate()) >= getdate()
								 and isnull(fw.EffectiveTo, getdate()) <= getdate()
                            ) as waiverProducts";
            SqlParameter parameter = new SqlParameter("@WaiverSetId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                refrenceCount = Convert.ToInt32(dataTable.Rows[0]["ReferenceCount"]);
            }
            log.LogMethodExit(refrenceCount);
            return refrenceCount;
        }

        internal DateTime? GetWaiverSetLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"SELECT MAX(LastUpdatedDate) LastUpdatedDate 
                            FROM (
                            select max(LastUpdatedDate) LastUpdatedDate from WaiverSet WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from WaiverSetDetails WHERE (site_id = @siteId or @siteId = -1)
                            union all
                            select max(LastUpdatedDate) LastUpdatedDate from WaiverSetSigningOptions WHERE (site_id = @siteId or @siteId = -1)
                            )a";
            SqlParameter parameter = new SqlParameter("@siteId", siteId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            DateTime? result = null;
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0]["LastUpdatedDate"] != DBNull.Value)
            {
                result = Convert.ToDateTime(dataTable.Rows[0]["LastUpdatedDate"]);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
