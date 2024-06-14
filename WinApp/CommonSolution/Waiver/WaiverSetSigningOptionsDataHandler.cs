/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data handler - WaiverSetSigningOptionsDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        01-Jul-2019   Girish Kundar     Modified : For SQL Injection Issue.   
 *2.70.2        15-Oct-2019    GUru S A         Waiver phase 2 changes
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.70.2      06-Feb-2020      Divya A          Changes for WMS 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Waiver
{
    /// <summary>
    ///  WaiverSetSigningOptions Data Handler - Handles insert, update and select of  WaiverSetSigningOptions objects
    /// </summary>
    public class WaiverSetSigningOptionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<WaiverSetSigningOptionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<WaiverSetSigningOptionsDTO.SearchByParameters, string>
        {
                {WaiverSetSigningOptionsDTO.SearchByParameters.ID, "wsso.Id"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID, "wsso.WaiverSetId"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_ID_LIST, "wsso.WaiverSetId"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID, "wsso.LookupValueId"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.MASTER_ENTITY_ID, "wsso.MasterEntityId"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID, "wsso.site_Id"},
                {WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_SIGNING_OPTIONS_LIST, "lp.Description"},

        };
        private const string SELECT_QUERY = @"SELECT wsso.*, lp.Description as OptionDescription, lp.LookupValue as OptionName
                                                FROM WaiverSetSigningOptions As wsso 
                                                     left outer join LookupValues lp on wsso.LookupValueId = lp.LookupValueId ";
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of WaiverSignedDataHandler class
        /// </summary>
        public WaiverSetSigningOptionsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating  WaiverSetSigningOptions Record.
        /// </summary>
        /// <param name="waiverSetSigningOptionsDTO"> WaiverSetSigningOptionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetSigningOptionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@Id", waiverSetSigningOptionsDTO.Id, true);
            ParametersHelper.ParameterHelper(parameters, "@waiverSetId", waiverSetSigningOptionsDTO.WaiverSetId, true);
            ParametersHelper.ParameterHelper(parameters, "@lookupValueId", waiverSetSigningOptionsDTO.LookupValueId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@guid", waiverSetSigningOptionsDTO.Guid);
			ParametersHelper.ParameterHelper(parameters, "@masterEntityId", waiverSetSigningOptionsDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the WaiverSetSigningOptions record to the database
        /// </summary>
        /// <param name="waiverSetSigningOptionsDTO">WaiverSetSigningOptionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetSigningOptionsDTO</returns>
        public WaiverSetSigningOptionsDTO InsertWaiverSetSigningOptions(WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetSigningOptionsDTO, loginId, siteId);
            string query = @"INSERT INTO WaiverSetSigningOptions 
                                        ( 
                                            WaiverSetId,
                                            LookupValueId,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedDate,
                                            LastUpdatedBy,
                                            Site_id,
                                            Guid,
                                            MasterEntityId
                                        ) 
                                VALUES 
                                        (
                                            @waiverSetId,
                                            @lookupValueId,
                                            @createdBy,
                                            GetDate(),
                                            GetDate(),
                                            @lastUpdatedBy,
                                            @site_id,
                                            NEWID(),
                                            @masterEntityId
                                            ) SELECT  * from WaiverSetSigningOptions where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetSigningOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetSigningOptionsDTO(waiverSetSigningOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WaiverSetSigningOptionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetSigningOptionsDTO);
            return waiverSetSigningOptionsDTO;
        }

        /// <summary>
        /// Updates the WaiverSetSigningOptions record
        /// </summary>
        /// <param name="waiverSetSigningOptionsDTO">WaiverSetSigningOptionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the WaiverSetSigningOptionsDTO</returns>
        public WaiverSetSigningOptionsDTO UpdateWaiverSetSigningOptions(WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetSigningOptionsDTO, loginId, siteId);
            string query = @"UPDATE WaiverSetSigningOptions
                             SET 
                                            WaiverSetId = @waiverSetId,
                                            LookupValueId = @lookupValueId,
                                            CreatedBy = @createdBy,
                                            LastUpdatedDate = GETDATE(),
                                            LastUpdatedBy = @lastUpdatedBy,
                                            -- Site_id = @site_id,
                                            MasterEntityId = @masterEntityId
                             WHERE Id = @Id
                             SELECT  * from WaiverSetSigningOptions where Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetSigningOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetSigningOptionsDTO(waiverSetSigningOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WaiverSetSigningOptionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetSigningOptionsDTO);
            return waiverSetSigningOptionsDTO;
        }
        /// <summary>
        /// Converts the Data row object to WaiverSetSigningOptionsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns WaiverSetSigningOptionsDTO</returns>
        private WaiverSetSigningOptionsDTO GetWaiverSetSigningOptionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO = new WaiverSetSigningOptionsDTO(Convert.ToInt32(dataRow["Id"]),
                                                            dataRow["WaiverSetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WaiverSetId"]),
                                                            dataRow["LookupValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LookupValueId"]),
                                                            dataRow["CreatedBy"].ToString(),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                            dataRow["LastUpdatedBy"].ToString(),
                                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                                            dataRow["Guid"].ToString(),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                            dataRow["OptionName"] == DBNull.Value ? string.Empty : dataRow["OptionName"].ToString(),
                                                            dataRow["OptionDescription"] == DBNull.Value ? string.Empty : dataRow["OptionDescription"].ToString()
                                                            );
            log.LogMethodExit(waiverSetSigningOptionsDTO);
            return waiverSetSigningOptionsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="waiverSetSigningOptionsDTO">WaiverSetDetailDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshWaiverSetSigningOptionsDTO(WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO, DataTable dt)
        {
            log.LogMethodEntry(waiverSetSigningOptionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                waiverSetSigningOptionsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                waiverSetSigningOptionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                waiverSetSigningOptionsDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the WaiverSetSigningOptions data of passed WaiverSetSigningOptions Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns WaiverSetSigningOptionsDTO</returns>
        public WaiverSetSigningOptionsDTO GetWaiverSetSigningOptionsDTO(int id, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(id);
            WaiverSetSigningOptionsDTO returnValue = null;
            string query = SELECT_QUERY + "   WHERE wsso.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetWaiverSetSigningOptionsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the WaiverSetSigningOptionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of WaiverSetSigningOptionsDTO matching the search criteria</returns>
        public List<WaiverSetSigningOptionsDTO> GetWaiverSetSigningOptionsList(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<WaiverSetSigningOptionsDTO> list = new List<WaiverSetSigningOptionsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.ID 
                            || searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID 
                            || searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.MASTER_ENTITY_ID 
                            || searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_SIGNING_OPTIONS_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == WaiverSetSigningOptionsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
            list = new List<WaiverSetSigningOptionsDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WaiverSetSigningOptionsDTO waiverSetSigningOptionsDTO = GetWaiverSetSigningOptionsDTO(dataRow);
                    list.Add(waiverSetSigningOptionsDTO);
                }
                log.LogMethodExit();
            }
            log.LogMethodExit(list);
            return list;
        }

        /// <summary>
        /// DeleteWaiverSetSigningOptions method 
        /// </summary>
        /// <param name="paymentModeId">waiverSetSigningOptionsId</param>
        /// <returns>return delete status</returns>
        public int DeleteWaiverSetSigningOptions(int waiverSetSigningOptionsId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(waiverSetSigningOptionsId, sqlTransaction);
            try
            {
                string paymentChannelQuery = @"delete  
                                          from WaiverSetSigningOptions
                                          where Id = @waiverSetSigningOptionsId";

                SqlParameter[] paymentChannelParameters = new SqlParameter[1];
                paymentChannelParameters[0] = new SqlParameter("@waiverSetSigningOptionsId", waiverSetSigningOptionsId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(paymentChannelQuery, paymentChannelParameters);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                log.Error("Error occurred while Deleting  WaiverSetSigningOptions", expn);
                log.LogMethodExit(null, "Throwing exception - " + expn.Message);
                throw;
                
            }
        }


    }
}
