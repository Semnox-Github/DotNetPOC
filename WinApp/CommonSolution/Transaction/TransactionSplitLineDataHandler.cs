/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TransactionSplitLineDataHandler
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
    /// This is the TransactionSplitLineDataHandler data object class.  Handles insert, update and select of  TrxSplitLines object
    /// </summary>
    public class TransactionSplitLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxSplitLines AS tspl ";
        /// <summary>
        /// Dictionary for searching Parameters for the TrxSplitLine object.
        /// </summary>
        private static readonly Dictionary<TransactionSplitLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionSplitLineDTO.SearchByParameters, string>
        {
            { TransactionSplitLineDTO.SearchByParameters.ID,"tspl.Id"},
            { TransactionSplitLineDTO.SearchByParameters.ID_LIST,"tspl.Id"},
            { TransactionSplitLineDTO.SearchByParameters.SPLIT_ID,"tspl.SplitId"},
            { TransactionSplitLineDTO.SearchByParameters.LINE_ID,"tspl.LineId"},
            { TransactionSplitLineDTO.SearchByParameters.SITE_ID,"tspl.site_id"},
            { TransactionSplitLineDTO.SearchByParameters.MASTER_ENTITY_ID,"tspl.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TransactionSplitLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TransactionSplitLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }


        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TrxSplitLines Record.
        /// </summary>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId"> site Id</param>
        /// <returns>Returns the List of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TransactionSplitLineDTO trxSplitLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxSplitLineDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", trxSplitLineDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SplitId", trxSplitLineDTO.SplitId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", trxSplitLineDTO.LineId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxSplitLineDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to TransactionSplitLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object </param>
        /// <returns>Returns the TransactionSplitLineDTO</returns>
        private TransactionSplitLineDTO GetTrxSplitLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionSplitLineDTO trxSplitLineDTO = new TransactionSplitLineDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                          dataRow["SplitId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SplitId"]),
                                          dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                          );
            log.LogMethodExit(trxSplitLineDTO);
            return trxSplitLineDTO;
        }

        /// <summary>
        /// Gets the TrxSplitLine data of passed id 
        /// </summary>
        /// <param name="id">id of TrxSplitLine is passed as parameter</param>
        /// <returns>Returns TransactionSplitLineDTO</returns>
        public TransactionSplitLineDTO GetTrxSplitLineDTO(int id)
        {
            log.LogMethodEntry(id);
            TransactionSplitLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE tspl.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxSplitLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Deletes the TrxSplitLine record
        /// </summary>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO is passed as parameter</param>
        internal void Delete(TransactionSplitLineDTO trxSplitLineDTO)
        {
            log.LogMethodEntry(trxSplitLineDTO);
            string query = @"DELETE  
                             FROM TrxSplitLines
                             WHERE TrxSplitLines.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", trxSplitLineDTO.Id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            trxSplitLineDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Inserts the record to the TrxSplitLines Table.
        /// </summary>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId"> site Id</param>
        /// <returns>Returns the  TransactionSplitLineDTO</returns>
        public TransactionSplitLineDTO Insert(TransactionSplitLineDTO trxSplitLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxSplitLineDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TrxSplitLines]
                           (SplitId,
                            LineId,
                            Guid,
                            site_id,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (
                            @SplitId,
                            @LineId,
                            NEWID(),
                            @site_id,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE()
                            )SELECT * FROM TrxSplitLines WHERE Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxSplitLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxSplitLineDTO(trxSplitLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TrxSplitLines ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxSplitLineDTO);
            return trxSplitLineDTO;
        }

        /// <summary>
        ///  Updates the record to the TrxSplitLines Table.
        /// </summary>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId"> site Id</param>
        /// <returns>Returns the  TransactionSplitLineDTO</returns>
        public TransactionSplitLineDTO Update(TransactionSplitLineDTO trxSplitLineDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxSplitLineDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxSplitLines]
                             SET
                                SplitId         =   @SplitId,
                                LineId          =   @LineId,
                                MasterEntityId  =   @MasterEntityId,
                                LastUpdatedBy   =   @LastUpdatedBy,
                                LastUpdateDate  =   GETDATE() 
                                WHERE Id = @Id 
                                SELECT * FROM TrxSplitLines WHERE Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxSplitLineDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxSplitLineDTO(trxSplitLineDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TrxSplitLines ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxSplitLineDTO);
            return trxSplitLineDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxSplitLineDTO">TransactionSplitLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
     
        private void RefreshTrxSplitLineDTO(TransactionSplitLineDTO trxSplitLineDTO, DataTable dt)
        {
            log.LogMethodEntry(trxSplitLineDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxSplitLineDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                trxSplitLineDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                trxSplitLineDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxSplitLineDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxSplitLineDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxSplitLineDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxSplitLineDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TransactionSplitLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionSplitLineDTO</returns>
        public List<TransactionSplitLineDTO> GetTrxSplitLineDTOList(List<KeyValuePair<TransactionSplitLineDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionSplitLineDTO> trxSplitLineDTOList = new List<TransactionSplitLineDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionSplitLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.ID
                            || searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.SPLIT_ID
                            || searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionSplitLineDTO.SearchByParameters.SITE_ID)
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
                    TransactionSplitLineDTO trxSplitLineDTO = GetTrxSplitLineDTO(dataRow);
                    trxSplitLineDTOList.Add(trxSplitLineDTO);
                }
            }
            log.LogMethodExit(trxSplitLineDTOList);
            return trxSplitLineDTOList;
        }
    }
}
