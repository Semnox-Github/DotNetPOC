/********************************************************************************************
 * Project Name - Waiver
 * Description  - Data handler - WaiverSetDetailSigningOptionsDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70        01-Jul-2019   Girish Kundar     Modified : Changed the structure and For SQL Injection Issue.
 *2.70.2        11-Dec-2019   Jinto Thomas      Removed siteid from update query
 ********************************************************************************************/
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
    ///  WaiverSetDetailSigningOptions Data Handler - Handles insert, update and select of  WaiverSetDetailSigningOptions objects
    /// </summary>
    public class WaiverSetDetailSigningOptionsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<WaiverSetDetailSigningOptionsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<WaiverSetDetailSigningOptionsDTO.SearchByParameters, string>
        {
                {WaiverSetDetailSigningOptionsDTO.SearchByParameters.ID, "wsds.Id"},
                {WaiverSetDetailSigningOptionsDTO.SearchByParameters.WAIVER_SET_DETAIL_ID, "wsds.WaiverSetDetailId"},
                {WaiverSetDetailSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID, "wsds.LookupValueId"},
                {WaiverSetDetailSigningOptionsDTO.SearchByParameters.SITE_ID, "wsds.site_Id"},

        };
        private const string SELECT_QUERY = @"SELECT * FROM WaiverSetDetailSigningOptions As wsds ";
        private DataAccessHandler dataAccessHandler;
        private  SqlTransaction sqlTransaction = null;
        /// <summary>
        /// Default constructor of WaiverSignedDataHandler class
        /// </summary>
        public WaiverSetDetailSigningOptionsDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating  WaiverSetDetailSigningOptions Record.
        /// </summary>
        /// <param name="waiverSetDetailSigningOptionsDTO"> WaiverSetDetailSigningOptionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> BuildSQLParameters(WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            ParametersHelper.ParameterHelper(parameters, "@Id", waiverSetDetailSigningOptionsDTO.Id, true);
            ParametersHelper.ParameterHelper(parameters, "@waiverSetDetailId", waiverSetDetailSigningOptionsDTO.WaiverSetDetailId, true);
            ParametersHelper.ParameterHelper(parameters, "@lookupValueId", waiverSetDetailSigningOptionsDTO.LookupValueId);
            ParametersHelper.ParameterHelper(parameters, "@createdBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@lastUpdatedBy", loginId);
            ParametersHelper.ParameterHelper(parameters, "@site_id", siteId, true);
            ParametersHelper.ParameterHelper(parameters, "@guid", waiverSetDetailSigningOptionsDTO.Guid);
            ParametersHelper.ParameterHelper(parameters, "@masterEntityId", waiverSetDetailSigningOptionsDTO.MasterEntityId, true);
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        /// Inserts the WaiverSetDetailSigningOptions record to the database
        /// </summary>
        /// <param name="waiverSetDetailSigningOptionsDTO">WaiverSetDetailSigningOptionsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetDetailSigningOptionsDTO</returns>
        public WaiverSetDetailSigningOptionsDTO InsertWaiverSetDetailSigningOptions(WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsDTO, loginId, siteId);
            string query = @"INSERT INTO WaiverSetDetailSigningOptions 
                                        ( 
                                            WaiverSetDetailId,
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
                                            @waiverSetDetailId,
                                            @lookupValueId,
                                            @createdBy,
                                            GetDate(),
                                            GetDate(),
                                            @lastUpdatedBy,
                                            @site_id,
                                            NEWID(),
                                            @masterEntityId
                                            )
                                            SELECT  * from WaiverSetDetailSigningOptions where Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDetailSigningOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetDetailSigningOptionsDTO(waiverSetDetailSigningOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting WaiverSetDetailSigningOptionsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDetailSigningOptionsDTO);
            return waiverSetDetailSigningOptionsDTO;
        }

        /// <summary>
        /// Updates the WaiverSetDetailSigningOptions record
        /// </summary>
        /// <param name="waiverSetDetailSigningOptionsDTO">WaiverSetDetailSigningOptionsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns WaiverSetDetailSigningOptionsDTO</returns>
        public WaiverSetDetailSigningOptionsDTO UpdateWaiverSetDetailSigningOptions(WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsDTO, loginId, siteId);
            string query = @"UPDATE WaiverSetDetailSigningOptions
                             SET 
                                            WaiverSetDetailId = @waiverSetDetailId,
                                            LookupValueId = @lookupValueId,
                                            CreatedBy = @createdBy,
                                            LastUpdatedDate = GETDATE(),
                                            LastUpdatedBy = @lastUpdatedBy,
                                            --Site_id = @site_id,
                                            MasterEntityId = @masterEntityId
                             WHERE Id = @Id
                             SELECT  * from WaiverSet WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, BuildSQLParameters(waiverSetDetailSigningOptionsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshWaiverSetDetailSigningOptionsDTO(waiverSetDetailSigningOptionsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating WaiverSetDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(waiverSetDetailSigningOptionsDTO);
            return waiverSetDetailSigningOptionsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="waiverSetDetailSigningOptionsDTO">WaiverSetDetailSigningOptionsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id </param>
        private void RefreshWaiverSetDetailSigningOptionsDTO(WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO, DataTable dt)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                waiverSetDetailSigningOptionsDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                waiverSetDetailSigningOptionsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                waiverSetDetailSigningOptionsDTO.SiteId = dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to WaiverSetDetailSigningOptionsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns WaiverSetDetailSigningOptionsDTO</returns>
        private WaiverSetDetailSigningOptionsDTO GetWaiverSetDetailSigningOptionsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO = new WaiverSetDetailSigningOptionsDTO(Convert.ToInt32(dataRow["Id"]),
                                                            dataRow["WaiverSetDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["WaiverSetDetailId"]),
                                                            dataRow["LookupValueId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LookupValueId"]),
                                                            dataRow["CreatedBy"].ToString(),
                                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                            dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                            dataRow["LastUpdatedBy"].ToString(),
                                                            dataRow["Site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Site_id"]),
                                                            dataRow["Guid"].ToString(),
                                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                                                            );
            log.LogMethodExit(waiverSetDetailSigningOptionsDTO);
            return waiverSetDetailSigningOptionsDTO;
        }


        /// <summary>
        /// Gets the WaiverSetDetailSigningOptions data of passed WaiverSetDetailSigningOptions Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns WaiverSetDetailSigningOptionsDTO</returns>
        public WaiverSetDetailSigningOptionsDTO GetWaiverSetDetailSigningOptionsDTO(int id)
        {
            log.LogMethodEntry(id);
            WaiverSetDetailSigningOptionsDTO returnValue = null;
            string query = SELECT_QUERY + " WHERE wsds.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                returnValue = GetWaiverSetDetailSigningOptionsDTO(dataTable.Rows[0]);
                log.LogMethodExit();
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Gets the WaiverSetDetailSigningOptionsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of WaiverSetDetailSigningOptionsDTO matching the search criteria</returns>
        public List<WaiverSetDetailSigningOptionsDTO> GetWaiverSetDetailSigningOptionsList(List<KeyValuePair<WaiverSetDetailSigningOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<WaiverSetDetailSigningOptionsDTO> list = new List<WaiverSetDetailSigningOptionsDTO>();
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<WaiverSetDetailSigningOptionsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if (searchParameter.Key == WaiverSetDetailSigningOptionsDTO.SearchByParameters.ID 
                            || searchParameter.Key == WaiverSetDetailSigningOptionsDTO.SearchByParameters.WAIVER_SET_DETAIL_ID 
                            || searchParameter.Key == WaiverSetDetailSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == WaiverSetDetailSigningOptionsDTO.SearchByParameters.SITE_ID)
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
            list = new List<WaiverSetDetailSigningOptionsDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    WaiverSetDetailSigningOptionsDTO waiverSetDetailSigningOptionsDTO = GetWaiverSetDetailSigningOptionsDTO(dataRow);
                    list.Add(waiverSetDetailSigningOptionsDTO);
                }
                log.LogMethodExit();
            }
            log.LogMethodExit(list);
            return list;
        }



        /// <summary>
        /// DeleteWaiverSetDetailSigningOptions method 
        /// </summary>
        /// <param name="paymentModeId">waiverSetDetailSigningOptionsId</param>
        /// <returns>return delete status</returns>
        public int DeleteWaiverSetDetailSigningOptions(int waiverSetDetailSigningOptionsId)
        {
            log.LogMethodEntry(waiverSetDetailSigningOptionsId);
            try
            {
                string paymentChannelQuery = @"delete  
                                          from WaiverSetDetailSigningOptions
                                          where Id = @waiverSetDetailSigningOptionsId";

                SqlParameter[] paymentChannelParameters = new SqlParameter[1];
                paymentChannelParameters[0] = new SqlParameter("@waiverSetDetailSigningOptionsId", waiverSetDetailSigningOptionsId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(paymentChannelQuery, paymentChannelParameters);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception expn)
            {
                throw new System.Exception("At DeleteWaiverSetDetailSigningOptions " + expn.Message.ToString());
            }
        }


    }
}
