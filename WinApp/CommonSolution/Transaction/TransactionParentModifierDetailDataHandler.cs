/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TrxParentModifierDetaildataHandler
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
    /// This is the TrxParentModifierDetaildataHandler data object class. Handles insert, update and select of  TrxParentModifierDetails object
    /// </summary>
    public class TransactionParentModifierDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxParentModifierDetails AS tpmd";
        /// <summary>
        /// Dictionary for searching Parameters for the TrxParentModifierDetails object.
        /// </summary>
        private static readonly Dictionary<TransactionParentModifierDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionParentModifierDetailDTO.SearchByParameters, string>
        {
            { TransactionParentModifierDetailDTO.SearchByParameters.ID,"tpmd.Id"},
            { TransactionParentModifierDetailDTO.SearchByParameters.ID_LIST,"tpmd.Id"},
            { TransactionParentModifierDetailDTO.SearchByParameters.LINE_ID,"tpmd.LineId"},
            { TransactionParentModifierDetailDTO.SearchByParameters.PARENT_MODIFIER_ID,"tpmd.ParentModifierId"},
            { TransactionParentModifierDetailDTO.SearchByParameters.PARENT_PRODUCT_ID,"tpmd.ParentProductId"},
            { TransactionParentModifierDetailDTO.SearchByParameters.PARENT_PRODUCT_NAME,"tpmd.ParentProductName"},
            { TransactionParentModifierDetailDTO.SearchByParameters.TRX_ID,"tpmd.TrxId"},
            { TransactionParentModifierDetailDTO.SearchByParameters.SITE_ID,"tpmd.site_id"},
            { TransactionParentModifierDetailDTO.SearchByParameters.MASTER_ENTITY_ID,"tpmd.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TrxParentModifierDetaildataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        public TransactionParentModifierDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TrxParentModifierDetails Record.
        /// </summary>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(TransactionParentModifierDetailDTO trxParentModifierDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxParentModifierDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", trxParentModifierDetailDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", trxParentModifierDetailDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentModifierId", trxParentModifierDetailDTO.ParentModifierId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentProductId", trxParentModifierDetailDTO.ParentProductId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", trxParentModifierDetailDTO.LineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentPrice", trxParentModifierDetailDTO.ParentPrice));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ParentProductName", trxParentModifierDetailDTO.ParentProductName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxParentModifierDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to TransactionParentModifierDetailDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TransactionParentModifierDetailDTO</returns>
        private TransactionParentModifierDetailDTO GetTrxParentModifierDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionParentModifierDetailDTO trxParentModifierDetailDTO = new TransactionParentModifierDetailDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                          dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                          dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                          dataRow["ParentModifierId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentModifierId"]),
                                          dataRow["ParentProductId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentProductId"]),
                                          dataRow["ParentProductName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ParentProductName"]),
                                          dataRow["ParentPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["ParentPrice"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedTime"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"])
                                          );
            log.LogMethodExit(trxParentModifierDetailDTO);
            return trxParentModifierDetailDTO;
        }

        /// <summary>
        /// Gets the TrxParentModifierDetails data of passed id 
        /// </summary>
        /// <param name="id">id of TrxParentModifierDetails is passed as parameter</param>
        /// <returns>Returns TransactionParentModifierDetailDTO</returns>
        public TransactionParentModifierDetailDTO GetTrxParentModifierDetailDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionParentModifierDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE tpmd.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxParentModifierDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TrxParentModifierDetail record
        /// </summary>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO is passed as parameter</param>
        internal void Delete(TransactionParentModifierDetailDTO trxParentModifierDetailDTO)
        {
            log.LogMethodEntry(trxParentModifierDetailDTO);
            string query = @"DELETE  
                             FROM TrxParentModifierDetails
                             WHERE TrxParentModifierDetails.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", trxParentModifierDetailDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            trxParentModifierDetailDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TrxParentModifierDetail Table.
        /// </summary>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TransactionParentModifierDetailDTO</returns>
        public TransactionParentModifierDetailDTO Insert(TransactionParentModifierDetailDTO trxParentModifierDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxParentModifierDetailDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TrxParentModifierDetails]
                           (TrxId,
                            LineId,
                            ParentModifierId,
                            ParentProductId,
                            ParentProductName,
                            ParentPrice,
                            Guid,
                            LastUpdatedBy,
                            LastUpdatedTime,
                            MasterEntityId,
                            site_id,
                            CreatedBy,
                            CreationDate)
                     VALUES
                           (@TrxId,
                           @LineId,
                           @ParentModifierId,
                           @ParentProductId,
                           @ParentProductName,
                           @ParentPrice,
                           NEWID(),
                           @LastUpdatedBy,
                           GETDATE(),
                           @MasterEntityId,
                           @site_id,
                           @CreatedBy,
                           GETDATE())
                                    SELECT * FROM TrxParentModifierDetails WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxParentModifierDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTxParentModifierDetailDTO(trxParentModifierDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TrxParentModifierDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxParentModifierDetailDTO);
            return trxParentModifierDetailDTO;
        }

        /// <summary>
        ///  Updates the record to the TrxParentModifierDetails Table.
        /// </summary>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO object is passed as parameter.</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the TransactionParentModifierDetailDTO</returns>
        public TransactionParentModifierDetailDTO Update(TransactionParentModifierDetailDTO trxParentModifierDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxParentModifierDetailDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxParentModifierDetails]
                           SET                 
                            TrxId             =  @TrxId,
                            LineId            =  @LineId,
                            ParentModifierId  =  @ParentModifierId,
                            ParentProductId   =  @ParentProductId,
                            ParentProductName =  @ParentProductName,
                            ParentPrice       =  @ParentPrice,
                            LastUpdatedBy     =  @LastUpdatedBy,
                            LastUpdatedTime   =  GETDATE(),
                            MasterEntityId    =  @MasterEntityId
                           WHERE Id = @Id
                       SELECT * FROM TrxParentModifierDetails WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxParentModifierDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTxParentModifierDetailDTO(trxParentModifierDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TrxParentModifierDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxParentModifierDetailDTO);
            return trxParentModifierDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxParentModifierDetailDTO">TransactionParentModifierDetailDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
     
        private void RefreshTxParentModifierDetailDTO(TransactionParentModifierDetailDTO trxParentModifierDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(trxParentModifierDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxParentModifierDetailDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                trxParentModifierDetailDTO.LastUpdatedDate = dataRow["LastUpdatedTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedTime"]);
                trxParentModifierDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxParentModifierDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxParentModifierDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxParentModifierDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxParentModifierDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TransactionParentModifierDetailDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionParentModifierDetailDTO</returns>
        public List<TransactionParentModifierDetailDTO> GetTrxParentModifierDetailDTOList(List<KeyValuePair<TransactionParentModifierDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionParentModifierDetailDTO> trxParentModifierDetailDTOList = new List<TransactionParentModifierDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE  ");
                foreach (KeyValuePair<TransactionParentModifierDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.ID
                            || searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.PARENT_MODIFIER_ID
                            || searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.PARENT_PRODUCT_ID
                            || searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionParentModifierDetailDTO.SearchByParameters.PARENT_PRODUCT_NAME)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
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
                    TransactionParentModifierDetailDTO trxParentModifierDetailDTO = GetTrxParentModifierDetailDTO(dataRow);
                    trxParentModifierDetailDTOList.Add(trxParentModifierDetailDTO);
                }
            }
            log.LogMethodExit(trxParentModifierDetailDTOList);
            return trxParentModifierDetailDTOList;
        }
    }
}
