/********************************************************************************************
 * Project Name - PayConfigurationDetailsDataHandler
 * Description  - data handler file for Pay Configuration Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90.0      03-Jul-2020   Akshay Gulaganji        Created 
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
    /// Pay Configuration Details Data Handler - Handles insert, update and select of Pay Configuration Details objects
    /// </summary>
    public class PayConfigurationDetailsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM PayConfigurationDetails as pcd ";

        /// <summary>
        /// Dictionary for searching Parameters for the PayConfigurationDetails object
        /// </summary>
        private static readonly Dictionary<PayConfigurationDetailsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<PayConfigurationDetailsDTO.SearchByParameters, string>
        {
            { PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_DETAIL_ID,"pcd.PayConfigurationDetailId"},
            { PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID,"pcd.PayConfigurationId"},
            { PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID_LIST,"pcd.PayConfigurationId"},
            { PayConfigurationDetailsDTO.SearchByParameters.EFFECTIVE_DATE_GREATER_THAN,"pcd.EffecitveDate"},
            { PayConfigurationDetailsDTO.SearchByParameters.END_DATE_LESS_THAN_OR_EQUALS,"pcd.EndDate"},
            { PayConfigurationDetailsDTO.SearchByParameters.IS_ACTIVE,"pcd.IsActive"},
            { PayConfigurationDetailsDTO.SearchByParameters.SITE_ID,"pcd.site_id"},
            { PayConfigurationDetailsDTO.SearchByParameters.MASTER_ENTITY_ID,"pcd.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for PayConfigurationDetailsDataHandler
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction Object</param>
        public PayConfigurationDetailsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating PayConfigurationDetails Record
        /// </summary>
        /// <param name="payConfigurationDetailsDTO">PayConfigurationDetailsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(PayConfigurationDetailsDTO payConfigurationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationDetailsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationDetailId", payConfigurationDetailsDTO.PayConfigurationDetailId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@payConfigurationId", payConfigurationDetailsDTO.PayConfigurationId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@regularPayRate", payConfigurationDetailsDTO.RegularPayRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@overtimePayRate", payConfigurationDetailsDTO.OvertimePayRate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@effectiveDate", payConfigurationDetailsDTO.EffectiveDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@endDate", payConfigurationDetailsDTO.EndDate, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", payConfigurationDetailsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", payConfigurationDetailsDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Converts the Data row object to PayConfigurationDetailsDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the object of payConfigurationDetailsDTO</returns>
        private PayConfigurationDetailsDTO GetPayConfigurationDetailsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            PayConfigurationDetailsDTO payConfigurationDetailsDTO = new PayConfigurationDetailsDTO(
                                                dataRow["PayConfigurationDetailId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayConfigurationDetailId"]),
                                                dataRow["PayConfigurationId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PayConfigurationId"]),
                                                dataRow["RegularPayRate"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["RegularPayRate"]),
                                                dataRow["OvertimePayRate"] == DBNull.Value ? -1 : Convert.ToDecimal(dataRow["OvertimePayRate"]),
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
            log.LogMethodExit(payConfigurationDetailsDTO);
            return payConfigurationDetailsDTO;
        }

        /// <summary>
        /// Gets the PayConfigurationDetails data of passed payConfigurationDetail Id
        /// </summary>
        /// <param name="payConfigurationDetailId">payConfigurationDetailId is passed as Parameter</param>
        /// <returns>Returns payConfigurationDetailsDTO</returns>
        public PayConfigurationDetailsDTO GetPayConfigurationDetailsDTO(int payConfigurationDetailId)
        {
            log.LogMethodEntry(payConfigurationDetailId);
            PayConfigurationDetailsDTO payConfigurationDetailsDTO = null;
            string query = SELECT_QUERY + @" WHERE pcd.PayConfigurationDetailId = @payConfigurationDetailId";
            SqlParameter parameter = new SqlParameter("@payConfigurationDetailId", payConfigurationDetailId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                payConfigurationDetailsDTO = GetPayConfigurationDetailsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(payConfigurationDetailsDTO);
            return payConfigurationDetailsDTO;
        }


        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="payConfigurationDetailsDTO">payConfigurationDetailsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        private void RefreshPayConfigurationDetailsDTO(PayConfigurationDetailsDTO payConfigurationDetailsDTO, DataTable dt)
        {
            log.LogMethodEntry(payConfigurationDetailsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                payConfigurationDetailsDTO.PayConfigurationDetailId = Convert.ToInt32(dt.Rows[0]["PayConfigurationDetailId"]);
                payConfigurationDetailsDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                payConfigurationDetailsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                payConfigurationDetailsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                payConfigurationDetailsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                payConfigurationDetailsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                payConfigurationDetailsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the PayConfigurationDetails Table. 
        /// </summary>
        /// <param name="payConfigurationDetailsDTO">PayConfigurationDetailsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public PayConfigurationDetailsDTO Insert(PayConfigurationDetailsDTO payConfigurationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationDetailsDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[PayConfigurationDetails]
                            (
                            PayConfigurationId,
                            RegularPayRate,
                            OvertimePayRate,
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
                            @payConfigurationId,
                            @regularPayRate,
                            @overtimePayRate,
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
                            SELECT * FROM PayConfigurationDetails WHERE PayConfigurationDetailId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationDetailsDTO(payConfigurationDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting PayConfigurationDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationDetailsDTO);
            return payConfigurationDetailsDTO;
        }

        /// <summary>
        /// Update the record in the PayConfigurationDetails Table. 
        /// </summary>
        /// <param name="payConfigurationDetailsDTO">PayConfigurationDetailsDTO object is passed as Parameter</param>
        /// <param name="loginId">login id of user </param>
        /// <param name="siteId">site id of user</param>
        /// <returns>Returns updated PayConfigurationDetailsDTO</returns>
        public PayConfigurationDetailsDTO Update(PayConfigurationDetailsDTO payConfigurationDetailsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(payConfigurationDetailsDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[PayConfigurationDetails]
                             SET
                             PayConfigurationId = @payConfigurationId,
                             RegularPayRate = @regularPayRate,
                             OvertimePayRate = @overtimePayRate,
                             EffectiveDate = @effectiveDate,
                             EndDate = @endDate,
                             IsActive = @isActive,
                             LastUpdatedDate = GETDATE(),
                             LastUpdatedBy = @lastUpdatedBy        
                             WHERE PayConfigurationDetailId = @payConfigurationDetailId
                            SELECT * FROM PayConfigurationDetails WHERE PayConfigurationDetailId = @payConfigurationDetailId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(payConfigurationDetailsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPayConfigurationDetailsDTO(payConfigurationDetailsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating PayConfigurationDetailsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(payConfigurationDetailsDTO);
            return payConfigurationDetailsDTO;
        }

        /// <summary>
        /// Returns the List of PayConfigurationDetailsDTO based on the search parameters
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of PayConfigurationDetailsDTO</returns>
        public List<PayConfigurationDetailsDTO> GetPayConfigurationDetailsDTOList(List<KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<PayConfigurationDetailsDTO> payConfigurationDetailsDTOList = new List<PayConfigurationDetailsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<PayConfigurationDetailsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_DETAIL_ID ||
                            searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID ||
                            searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'1')= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.PAY_CONFIGURATION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.EFFECTIVE_DATE_GREATER_THAN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == PayConfigurationDetailsDTO.SearchByParameters.END_DATE_LESS_THAN_OR_EQUALS)
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
                    PayConfigurationDetailsDTO payConfigurationDetailsDTO = GetPayConfigurationDetailsDTO(dataRow);
                    payConfigurationDetailsDTOList.Add(payConfigurationDetailsDTO);
                }
            }
            log.LogMethodExit(payConfigurationDetailsDTOList);
            return payConfigurationDetailsDTOList;
        }
    }
}
