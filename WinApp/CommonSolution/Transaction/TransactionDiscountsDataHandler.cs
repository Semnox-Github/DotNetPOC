/********************************************************************************************
 * Project Name - TransactionDiscounts Data Handler
 * Description  - Data handler of the TransactionDiscounts class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        16-Jul-2017   Lakshminarayana     Created
 *2.80        31-May-2020   Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  TransactionDiscounts Data Handler - Handles insert, update and select of  TransactionDiscounts objects
    /// </summary>
    public class TransactionDiscountsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<TransactionDiscountsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionDiscountsDTO.SearchByParameters, string>
            {
                {TransactionDiscountsDTO.SearchByParameters.TRANSACTION_DISCOUNT_ID, "td.TrxDiscountId"},
                {TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID, "td.TrxId"},
                {TransactionDiscountsDTO.SearchByParameters.LINE_ID, "td.LineId"},
                {TransactionDiscountsDTO.SearchByParameters.DISCOUNT_ID, "td.DiscountId"},
                {TransactionDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID,"td.MasterEntityId"},
                {TransactionDiscountsDTO.SearchByParameters.SITE_ID, "td.site_id"}
            };
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT td.* 
                                              FROM TrxDiscounts AS td ";
        /// <summary>
        /// Default constructor of TransactionDiscountsDataHandler class
        /// </summary>
        public TransactionDiscountsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TransactionDiscounts Record.
        /// </summary>
        /// <param name="transactionDiscountsDTO">TransactionDiscountsDTO type object</param>
        /// <param name="loginId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSqlParameters(TransactionDiscountsDTO transactionDiscountsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionDiscountsDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@TrxDiscountId", transactionDiscountsDTO.TransactionDiscountId, true),
                dataAccessHandler.GetSQLParameter("@TrxId", transactionDiscountsDTO.TransactionId, true),
                dataAccessHandler.GetSQLParameter("@LineId", transactionDiscountsDTO.LineId, true),
                dataAccessHandler.GetSQLParameter("@DiscountId", transactionDiscountsDTO.DiscountId, true),
                dataAccessHandler.GetSQLParameter("@DiscountPercentage", transactionDiscountsDTO.DiscountPercentage),
                dataAccessHandler.GetSQLParameter("@DiscountAmount", transactionDiscountsDTO.DiscountAmount),
                dataAccessHandler.GetSQLParameter("@Remarks", transactionDiscountsDTO.Remarks),
                dataAccessHandler.GetSQLParameter("@ApprovedBy", transactionDiscountsDTO.ApprovedBy, true),
                dataAccessHandler.GetSQLParameter("@Applicability", DiscountApplicabilityConverter.ToString(transactionDiscountsDTO.Applicability)),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", transactionDiscountsDTO.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId),
                dataAccessHandler.GetSQLParameter("@CreatedBy", loginId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the TransactionDiscounts record to the database
        /// </summary>
        /// <param name="transactionDiscountsDTO">TransactionDiscountsDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the TransactionDiscountsDTO</returns>
        public TransactionDiscountsDTO InsertTransactionDiscounts(TransactionDiscountsDTO transactionDiscountsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionDiscountsDTO, loginId, siteId);
            string query = @"INSERT INTO TrxDiscounts 
                                        ( 
                                            TrxId,
                                            LineId,
                                            DiscountId,
                                            DiscountPercentage,
                                            DiscountAmount,
                                            Remarks,
                                            ApprovedBy,
                                            Applicability,
                                            site_id,
                                            MasterEntityId,
                                            SynchStatus,
                                            CreatedBy,
                                            CreationDate,
                                            LastUpdatedBy,
                                            LastUpdateDate
                                        ) 
                                        VALUES 
                                        (
                                            @TrxId,
                                            @LineId,
                                            @DiscountId,
                                            @DiscountPercentage,
                                            @DiscountAmount,
                                            @Remarks,
                                            @ApprovedBy,
                                            @Applicability,
                                            @site_id,
                                            @MasterEntityId,
                                            NULL,
                                            @CreatedBy,
                                            GETDATE(),
                                            @LastUpdatedBy,
                                            GETDATE()) " + SELECT_QUERY + " WHERE td.TrxDiscountId = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(transactionDiscountsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionDiscountsDTO(transactionDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionDiscountsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }

            log.LogMethodExit(transactionDiscountsDTO);
            return transactionDiscountsDTO;
        }

        /// <summary>
        /// Updates the TransactionDiscounts record
        /// </summary>
        /// <param name="transactionDiscountsDTO">TransactionDiscountsDTO type parameter</param>
        /// <param name="loginId">User updating the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the TransactionDiscountsDTO</returns>
        public TransactionDiscountsDTO UpdateTransactionDiscounts(TransactionDiscountsDTO transactionDiscountsDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(transactionDiscountsDTO, loginId, siteId);
            string query = @"UPDATE TrxDiscounts 
                             SET TrxId=@TrxId,
                                 LineId=@LineId,
                                 DiscountId=@DiscountId,
                                 DiscountPercentage=@DiscountPercentage,
                                 DiscountAmount=@DiscountAmount,
                                 Remarks=@Remarks,
                                 Applicability=@Applicability,
                                 ApprovedBy=@ApprovedBy,
                                 MasterEntityId=@MasterEntityId,
                                 SynchStatus=NULL,
                                 LastUpdatedBy=@LastUpdatedBy,
                                 LastUpdateDate=GETDATE()
                             WHERE TrxDiscountId = @TrxDiscountId " + SELECT_QUERY + " WHERE td.TrxDiscountId = @TrxDiscountId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(transactionDiscountsDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTransactionDiscountsDTO(transactionDiscountsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionDiscountsDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(transactionDiscountsDTO);
            return transactionDiscountsDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="transactionDiscountsDTO">TransactionDiscountsDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>

        private void RefreshTransactionDiscountsDTO(TransactionDiscountsDTO transactionDiscountsDTO, DataTable dt)
        {
            log.LogMethodEntry(transactionDiscountsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                transactionDiscountsDTO.TransactionDiscountId = Convert.ToInt32(dt.Rows[0]["TrxDiscountId"]);
                transactionDiscountsDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                transactionDiscountsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                transactionDiscountsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                transactionDiscountsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                transactionDiscountsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                transactionDiscountsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to TransactionDiscountsDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns TransactionDiscountsDTO</returns>
        private TransactionDiscountsDTO GetTransactionDiscountsDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionDiscountsDTO transactionDiscountsDTO = new TransactionDiscountsDTO(Convert.ToInt32(dataRow["TrxDiscountId"]),
                                            dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                            dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                            dataRow["DiscountId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DiscountId"]),
                                            dataRow["DiscountPercentage"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountPercentage"]),
                                            dataRow["DiscountAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["DiscountAmount"]),
                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                            dataRow["ApprovedBy"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ApprovedBy"]),
                                            DiscountApplicabilityConverter.FromString(dataRow["Applicability"] == DBNull.Value ? "T" : Convert.ToString(dataRow["Applicability"])),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["SynchStatus"] != DBNull.Value && Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["Guid"] == DBNull.Value ? "" : dataRow["Guid"].ToString(),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                            );
            log.LogMethodExit(transactionDiscountsDTO);
            return transactionDiscountsDTO;
        }

        /// <summary>
        /// Gets the TransactionDiscounts data of passed TransactionDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns TransactionDiscountsDTO</returns>
        public TransactionDiscountsDTO GetTransactionDiscountsDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionDiscountsDTO result = null;
            string query = SELECT_QUERY + @" WHERE td.TrxDiscountId = @TrxDiscountId";
            SqlParameter parameter = new SqlParameter("@TrxDiscountId", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTransactionDiscountsDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Deletes the TransactionDiscounts data of passed TransactionDiscounts Id
        /// </summary>
        /// <param name="id">integer type parameter</param>
        /// <returns>Returns no of rows deleted</returns>
        public int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE FROM TrxDiscounts
                            WHERE TrxDiscountId = @TrxDiscountId";
            SqlParameter parameter = new SqlParameter("@TrxDiscountId", id);
            int noOfRowsDeleted = dataAccessHandler.executeUpdateQuery(query, new[] { parameter }, sqlTransaction);
            log.LogMethodExit(noOfRowsDeleted);
            return noOfRowsDeleted;
        }

        /// <summary>
        /// Gets the TransactionDiscountsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of TransactionDiscountsDTO matching the search criteria</returns>
        public List<TransactionDiscountsDTO> GetTransactionDiscountsDTOList(List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TransactionDiscountsDTO> list = null;
            int count = 0;
            string selectQuery = SELECT_QUERY;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joiner = (count == 0) ? " " : " and ";
                        if (searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.TRANSACTION_DISCOUNT_ID ||
                            searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.DISCOUNT_ID ||
                            searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.LINE_ID ||
                            searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionDiscountsDTO.SearchByParameters.SITE_ID)
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
                        log.LogMethodExit();
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<TransactionDiscountsDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    TransactionDiscountsDTO transactionDiscountsDTO = GetTransactionDiscountsDTO(dataRow);
                    list.Add(transactionDiscountsDTO);
                }
                log.LogMethodExit();
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
