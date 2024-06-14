/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler -TransactionTaxLineDataHandler
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
    /// This is the TransactionTaxLineDataHandler data object class. Handles insert, update and select of  TrxTaxLine objects
    /// </summary>
    public class TransactionTaxLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxTaxLines AS ttl";
        /// <summary>
        /// Dictionary for searching Parameters for the TrxTaxLine object.
        /// </summary>
        private static readonly Dictionary<TransactionTaxLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionTaxLineDTO.SearchByParameters, string>
        {
            { TransactionTaxLineDTO.SearchByParameters.TRX_ID , "ttl.TrxId"},                         
            { TransactionTaxLineDTO.SearchByParameters.TRX_TAX_LINE_ID,"ttl.TrxTaxLineId"},               
            { TransactionTaxLineDTO.SearchByParameters.TRX_TAX_LINE_ID_LIST,"ttl.TrxTaxLineId"},               
            { TransactionTaxLineDTO.SearchByParameters.TAX_ID,"ttl.TaxId"},                        
            { TransactionTaxLineDTO.SearchByParameters.TAX_STRUCTURE_ID,"ttl.TaxStructureId"},               
            { TransactionTaxLineDTO.SearchByParameters.LINE_ID,"ttl.LineId"},                       
            { TransactionTaxLineDTO.SearchByParameters.SITE_ID,"ttl.site_id"},
            { TransactionTaxLineDTO.SearchByParameters.MASTER_ENTITY_ID,"ttl.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TransactionTaxLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public TransactionTaxLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TrxTaxLine Record.
        /// </summary>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TransactionTaxLineDTO trxTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxTaxLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxTaxLineId", trxTaxLineDTO.TrxTaxLineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", trxTaxLineDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxId", trxTaxLineDTO.TaxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TaxStructureId", trxTaxLineDTO.TaxStructureId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", trxTaxLineDTO.LineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Amount", trxTaxLineDTO.Amount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Percentage", trxTaxLineDTO.Percentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProductSplitAmount", trxTaxLineDTO.ProductSplitAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxTaxLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to TransactionTaxLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TransactionTaxLineDTO</returns>
        private TransactionTaxLineDTO GetTrxTaxLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionTaxLineDTO trxTaxLineDTO = new TransactionTaxLineDTO(dataRow["TrxTaxLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxTaxLineId"]),
                                          dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                          dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                          dataRow["TaxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaxId"]),
                                          dataRow["TaxStructureId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TaxStructureId"]),
                                          dataRow["Percentage"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Percentage"]),
                                          dataRow["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Amount"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["ProductSplitAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ProductSplitAmount"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                          );
            log.LogMethodExit(trxTaxLineDTO);
            return trxTaxLineDTO;
        }

        /// <summary>
        /// Gets the TrxTaxLine data of passed trxTaxLineId 
        /// </summary>
        /// <param name="trxTaxLineId">trxTaxLineId of  TrxTaxLine  passed as parameter</param>
        /// <returns>Returns TransactionTaxLineDTO</returns>
        public TransactionTaxLineDTO GetTrxTaxLineDTO(int trxTaxLineId)
        {
            log.LogMethodEntry(trxTaxLineId);
            TransactionTaxLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE ttl.TrxTaxLineId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", trxTaxLineId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxTaxLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TrxTaxLine record
        /// </summary>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO is passed as parameter</param>
        internal void Delete(TransactionTaxLineDTO trxTaxLineDTO)
        {
            log.LogMethodEntry(trxTaxLineDTO);
            string query = @"DELETE  
                             FROM TrxTaxLines
                             WHERE TrxTaxLines.TrxTaxLineId = @TrxTaxLineId";
            SqlParameter parameter = new SqlParameter("@TrxTaxLineId", trxTaxLineDTO.TrxTaxLineId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            trxTaxLineDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TrxTaxLine Table.
        /// </summary>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns> Returns the TransactionTaxLineDTO</returns>
        public TransactionTaxLineDTO Insert(TransactionTaxLineDTO trxTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxTaxLineDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TrxTaxLines]
                           (TrxId,
                            LineId,
                            TaxId,
                            TaxStructureId,
                            Percentage,
                            Amount,
                            Guid,
                            site_id,
                            ProductSplitAmount,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TrxId,
                            @LineId,
                            @TaxId,
                            @TaxStructureId,
                            @Percentage,
                            @Amount,
                            NEWID(),
                            @site_id,
                            @ProductSplitAmount,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() )
                                    SELECT * FROM TrxTaxLines WHERE TrxTaxLineId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxTaxLineDTO(trxTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TrxTaxLine ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxTaxLineDTO);
            return trxTaxLineDTO;
        }

        /// <summary>
        ///  Updates the record to the TrxTaxLines Table.
        /// </summary>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns> Returns the TransactionTaxLineDTO</returns>
        public TransactionTaxLineDTO Update(TransactionTaxLineDTO trxTaxLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxTaxLineDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxTaxLines]
                           SET 
                            TrxId               = @TrxId,
                            LineId              = @LineId,
                            TaxId               = @TaxId,
                            TaxStructureId      = @TaxStructureId,
                            Percentage          = @Percentage,
                            Amount              = @Amount,
                            ProductSplitAmount  = @ProductSplitAmount,
                            MasterEntityId      = @MasterEntityId,
                            LastUpdatedBy       = @LastUpdatedBy,
                            LastUpdateDate      = GETDATE() 
                           where TrxTaxLineId = @TrxTaxLineId 
                           SELECT * FROM TrxTaxLines WHERE TrxTaxLineId = @TrxTaxLineId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxTaxLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxTaxLineDTO(trxTaxLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TrxTaxLine ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxTaxLineDTO);
            return trxTaxLineDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxTaxLineDTO">TransactionTaxLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
       
        private void RefreshTrxTaxLineDTO(TransactionTaxLineDTO trxTaxLineDTO, DataTable dt)
        {
            log.LogMethodEntry(trxTaxLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxTaxLineDTO.TrxTaxLineId = Convert.ToInt32(dt.Rows[0]["TrxTaxLineId"]);
                trxTaxLineDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                trxTaxLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxTaxLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxTaxLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxTaxLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxTaxLineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TransactionTaxLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionTaxLineDTO</returns>
        public List<TransactionTaxLineDTO> GetTrxTaxLineDTOList(List<KeyValuePair<TransactionTaxLineDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionTaxLineDTO> trxTaxLineDTOList = new List<TransactionTaxLineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionTaxLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.TRX_TAX_LINE_ID
                            || searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.TAX_ID
                            || searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.TAX_STRUCTURE_ID
                            || searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.TRX_TAX_LINE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionTaxLineDTO.SearchByParameters.SITE_ID)
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
                    TransactionTaxLineDTO trxTaxLineDTO = GetTrxTaxLineDTO(dataRow);
                    trxTaxLineDTOList.Add(trxTaxLineDTO);
                }
            }
            log.LogMethodExit(trxTaxLineDTOList);
            return trxTaxLineDTOList;
        }
    }
}
