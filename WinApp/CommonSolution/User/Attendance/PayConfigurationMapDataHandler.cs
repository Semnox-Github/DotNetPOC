/********************************************************************************************
 * Project Name - PayConfigurationMapDataHandler
 * Description  - data handler file for  Pay Configuration Map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      03-Jul-2020   Akshay Gulaganji        Created 
 *2.100.0     22-Aug-2020   Vikas Dwivedi           Resolved issues in CURD operations
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Pay Configuration Map Data Handler - Handles insert, update and select of Pay Configuration Map objects
    /// </summary>
    public class PayConfigurationMapDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PayConfigurationMap as pcm ";

        /// <summary>
        /// Dictionary for searching Parameters for the PayConfigurationDetails object
        /// </summary>
        private static readonly Dictionary<PayConfigurationMapDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PayConfigurationMapDTO.SearchByParameters, string>
        {
            { PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_MAP_ID,"pcm.PayConfigurationMapId"},
            { PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID,"pcm.UserRoleId"},
            { PayConfigurationMapDTO.SearchByParameters.USER_ID,"pcm.UserId"},
            { PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID,"pcm.PayConfigurationId"},
            { PayConfigurationMapDTO.SearchByParameters.EFFECTIVE_DATE_GREATER_THAN,"pcm.EffectiveDate"},
            { PayConfigurationMapDTO.SearchByParameters.END_DATE_LESS_THAN_OR_EQUALS,"pcm.EndDate"},
            { PayConfigurationMapDTO.SearchByParameters.IS_ACTIVE,"pcm.IsActive"},
            { PayConfigurationMapDTO.SearchByParameters.SITE_ID,"pcm.site_id"},
            { PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID_LIST,"pcm.UserRoleId"},
            { PayConfigurationMapDTO.SearchByParameters.USER_ID_LIST,"pcm.UserId"},
            { PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID_LIST,"pcm.PayConfigurationId"}
        };

        /// <summary>
        /// Parameterized Constructor for PayConfigurationMapDataHandler
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public PayConfigurationMapDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating Pay Configuration Map Record
        /// </summary>
        /// <param name="payConfigurationMapDTO">payConfigurationMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PayConfigurationMapDTO payConfigurationMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationMapDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationMapId", payConfigurationMapDTO.PayConfigurationMapId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userRoleId", payConfigurationMapDTO.UserRoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", payConfigurationMapDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationId", payConfigurationMapDTO.PayConfigurationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveDate", payConfigurationMapDTO.EffectiveDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate", payConfigurationMapDTO.EndDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", payConfigurationMapDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", payConfigurationMapDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PayConfigurationMapDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of payConfigurationMapDTO</returns>
        private PayConfigurationMapDTO GetPayConfigurationMapDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PayConfigurationMapDTO payConfigurationMapDTO = new PayConfigurationMapDTO(
                                               dataRow["PayConfigurationMapId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayConfigurationMapId"]),
                                                dataRow["UserRoleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserRoleId"]),
                                                dataRow["UserId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UserId"]),
                                                dataRow["PayConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayConfigurationId"]),
                                                dataRow["EffectiveDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["EffectiveDate"]),
                                                dataRow["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]),
                                                dataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["IsActive"].ToString()),
                                                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                                                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                               );
            log.LogMethodExit(payConfigurationMapDTO);
            return payConfigurationMapDTO;
        }

        /// <summary>
        /// Gets the PayConfigurationMap data of passed Pay Configuration Map Id
        /// </summary>
        /// <param name="payConfigurationMapId">payConfigurationMapId is passed as Parameter</param>
        /// <returns>Returns payConfigurationDetailsDTO</returns>
        public PayConfigurationMapDTO GetPayConfigurationMapDTO(int payConfigurationMapId)
        {
            log.LogMethodEntry(payConfigurationMapId);
            PayConfigurationMapDTO payConfigurationMapDTO = null;
            string query = SELECT_QUERY + @" WHERE pcm.PayConfigurationMapId = @payConfigurationMapId";
            SqlParameter parameter = new SqlParameter("@payConfigurationMapId", payConfigurationMapId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                payConfigurationMapDTO = GetPayConfigurationMapDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(payConfigurationMapDTO);
            return payConfigurationMapDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="payConfigurationMapDTO">PayConfigurationMapDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshPayConfigurationMapDTO(PayConfigurationMapDTO payConfigurationMapDTO, DataTable dt)
        {
            log.LogMethodEntry(payConfigurationMapDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payConfigurationMapDTO.PayConfigurationMapId = Convert.ToInt32(dt.Rows[0]["PayConfigurationMapId"]);
                payConfigurationMapDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                payConfigurationMapDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                payConfigurationMapDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                payConfigurationMapDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                payConfigurationMapDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                payConfigurationMapDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PayConfigurationMap Table. 
        /// </summary>
        /// <param name="PayConfigurationMapDTO">PayConfigurationMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public PayConfigurationMapDTO Insert(PayConfigurationMapDTO payConfigurationMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationMapDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PayConfigurationMap]
                            (
                            UserRoleId,
                            UserId,
                            PayConfigurationId,
                            EffectiveDate,
                            EndDate,
                            IsActive,
                            Guid,
                            site_id,
                            MasterEntityId,
                            LastUpdatedDate,
                            LastUpdatedBy,
                            CreatedBy,
                            CreationDate
                            )
                            VALUES
                            (
                            @userRoleId,
                            @userId,
                            @payConfigurationId,
                            @effectiveDate,
                            @endDate,
                            @isActive,
                            NEWID(),
                            @siteId,
                            @masterEntityId,
                            GETDATE(),
                            @lastUpdatedBy,
                            @createdBy,
                            GETDATE()                      
                            )
                            SELECT * FROM PayConfigurationMap pcm WHERE pcm.PayConfigurationMapId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationMapDTO(payConfigurationMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PayConfigurationMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationMapDTO);
            return payConfigurationMapDTO;
        }

        /// <summary>
        /// Update the record in the PayConfigurationMap Table. 
        /// </summary>
        /// <param name="PayConfigurationMapDTO">PayConfigurationMapDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public PayConfigurationMapDTO Update(PayConfigurationMapDTO payConfigurationMapDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationMapDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PayConfigurationMap]
                             SET
                             UserRoleId = @userRoleId,
                             UserId = @userId,
                             PayConfigurationId = @payConfigurationId,
                             EffectiveDate = @effectiveDate,
                             EndDate = @endDate,
                             IsActive = @isActive,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @lastUpdatedBy        
                             WHERE PayConfigurationMapId = @payConfigurationMapId
                            SELECT * FROM PayConfigurationMap pcm WHERE pcm.PayConfigurationMapId = @payConfigurationMapId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationMapDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationMapDTO(payConfigurationMapDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PayConfigurationMapDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationMapDTO);
            return payConfigurationMapDTO;
        }

        /// <summary>
        /// Returns the List of PayConfigurationMapDTO based on the search parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PayConfigurationMapDTO</returns>
        public List<PayConfigurationMapDTO> GetPayConfigurationMapDTOList(List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PayConfigurationMapDTO> payConfigurationMapDTOList = new List<PayConfigurationMapDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_MAP_ID ||
                            searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID ||
                            searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.USER_ID ||
                            searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID ||
                            searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID_LIST ||
                                searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID_LIST ||
                                searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.USER_ID_LIST
                                )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.EFFECTIVE_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PayConfigurationMapDTO.SearchByParameters.END_DATE_LESS_THAN_OR_EQUALS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    PayConfigurationMapDTO payConfigurationMapDTO = GetPayConfigurationMapDTO(dataRow);
                    payConfigurationMapDTOList.Add(payConfigurationMapDTO);
                }
            }
            log.LogMethodExit(payConfigurationMapDTOList);
            return payConfigurationMapDTOList;
        }
    }
}
