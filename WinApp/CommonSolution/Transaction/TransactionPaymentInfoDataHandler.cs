/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TransactionPaymentInfoDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3      03-Jun-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the TransactionPaymentInfoDataHandler data object class. Handles insert, update and select of  TrxPaymentsInfo object
    /// </summary>
    public class TransactionPaymentInfoDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxPaymentsInfo AS tpi";
        /// <summary>
        /// Dictionary for searching Parameters for the TrxPaymentsInfo object.
        /// </summary>
        private static readonly Dictionary<TransactionPaymentInfoDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionPaymentInfoDTO.SearchByParameters, string>
        {
            { TransactionPaymentInfoDTO.SearchByParameters.ID,"tpi.Id"},
            { TransactionPaymentInfoDTO.SearchByParameters.ID_LIST,"tpi.Id"},
            { TransactionPaymentInfoDTO.SearchByParameters.PAYMENT_ID,"tpi.PaymentId"},
            { TransactionPaymentInfoDTO.SearchByParameters.SITE_ID,"tpi.site_id"},
            { TransactionPaymentInfoDTO.SearchByParameters.MASTER_ENTITY_ID,"tpi.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TransactionPaymentInfoDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TransactionPaymentInfoDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating trxPaymentsInfo Record.
        /// </summary>
        /// <param name="trxPaymentsInfoDTO">TransactionPaymentInfoDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TransactionPaymentInfoDTO trxPaymentInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPaymentInfoDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", trxPaymentInfoDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@PaymentId", trxPaymentInfoDTO.PaymentId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxPaymentInfoDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to TransactionPaymentInfoDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TransactionPaymentInfoDTO</returns>
        private TransactionPaymentInfoDTO GetTrxPaymentsInfoDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionPaymentInfoDTO trxPaymentsInfoDTO = new TransactionPaymentInfoDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                          dataRow["PaymentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentId"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                          );
            log.LogMethodExit(trxPaymentsInfoDTO);
            return trxPaymentsInfoDTO;
        }

        /// <summary>
        /// Gets the TrxPaymentsInfo data of passed id 
        /// </summary>
        /// <param name="id">id of TrxPaymentsInfo is passed as parameter</param>
        /// <returns>Returns TransactionPaymentInfoDTO</returns>
        public TransactionPaymentInfoDTO GetTrxPaymentsInfoDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionPaymentInfoDTO result = null;
            string query = SELECT_QUERY + @" WHERE tpi.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxPaymentsInfoDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TrxPaymentsInfo record
        /// </summary>
        /// <param name="trxPaymentsInfoDTO">TransactionPaymentInfoDTO is passed as parameter</param>
        internal void Delete(TransactionPaymentInfoDTO trxPaymentsInfoDTO)
        {
            log.LogMethodEntry(trxPaymentsInfoDTO);
            string query = @"DELETE  
                             FROM TrxPaymentsInfo
                             WHERE TrxPaymentsInfo.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", trxPaymentsInfoDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            trxPaymentsInfoDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TrxPaymentsInfo Table.
        /// </summary>
        /// <param name="trxPaymentsInfoDTO">TransactionPaymentInfoDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the TransactionPaymentInfoDTO </returns>
        public TransactionPaymentInfoDTO Insert(TransactionPaymentInfoDTO trxPaymentsInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPaymentsInfoDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TrxPaymentsInfo]
                           (PaymentId,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@PaymentId,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE())
                                    SELECT * FROM TrxPaymentsInfo WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxPaymentsInfoDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxPaymentsInfoDTO(trxPaymentsInfoDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TrxPaymentsInfo ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxPaymentsInfoDTO);
            return trxPaymentsInfoDTO;
        }

        /// <summary>
        ///  Updates the record to the TrxPaymentsInfo Table.
        /// </summary>
        /// <param name="trxPaymentsInfoDTO">TransactionPaymentInfoDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the TransactionPaymentInfoDTO </returns>s
        public TransactionPaymentInfoDTO Update(TransactionPaymentInfoDTO trxPaymentsInfoDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPaymentsInfoDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxPaymentsInfo]
                           SET
                            PaymentId      = @PaymentId,
                            MasterEntityId = @MasterEntityId,
                            LastUpdatedBy  = @LastUpdatedBy,
                            LastUpdateDate = GETDATE()
                            where Id  = @Id
                            SELECT * FROM TrxPaymentsInfo WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxPaymentsInfoDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxPaymentsInfoDTO(trxPaymentsInfoDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TrxPaymentsInfo ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxPaymentsInfoDTO);
            return trxPaymentsInfoDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxPaymentsInfoDTO">TransactionPaymentInfoDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
       
        private void RefreshTrxPaymentsInfoDTO(TransactionPaymentInfoDTO trxPaymentsInfoDTO, DataTable dt)
        {
            log.LogMethodEntry(trxPaymentsInfoDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxPaymentsInfoDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                trxPaymentsInfoDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                trxPaymentsInfoDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxPaymentsInfoDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxPaymentsInfoDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxPaymentsInfoDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxPaymentsInfoDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TransactionPaymentInfoDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionPaymentInfoDTO</returns>
        public List<TransactionPaymentInfoDTO> GetTrxPaymentsInfoDTOList(List<KeyValuePair<TransactionPaymentInfoDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionPaymentInfoDTO> trxPaymentsInfoDTOList = new List<TransactionPaymentInfoDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionPaymentInfoDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionPaymentInfoDTO.SearchByParameters.ID
                            || searchParameter.Key == TransactionPaymentInfoDTO.SearchByParameters.PAYMENT_ID
                            || searchParameter.Key == TransactionPaymentInfoDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionPaymentInfoDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionPaymentInfoDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
                    TransactionPaymentInfoDTO trxPaymentsInfoDTO = GetTrxPaymentsInfoDTO(dataRow);
                    trxPaymentsInfoDTOList.Add(trxPaymentsInfoDTO);
                }
            }
            log.LogMethodExit(trxPaymentsInfoDTOList);
            return trxPaymentsInfoDTOList;
        }
    }
}
