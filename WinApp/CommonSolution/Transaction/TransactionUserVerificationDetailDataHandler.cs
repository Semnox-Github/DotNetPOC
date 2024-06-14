/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - TransactionUserVerificationDetailDataHandler
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
    /// This is the TransactionUserVerificationDetailDataHandler data object class.  Handles insert, update and select of  TrxUserVerificationDetails object
    /// </summary>
    public class TransactionUserVerificationDetailDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxUserVerificationDetails AS tuvd";
        /// <summary>
        /// Dictionary for searching Parameters for the TrxTaxLines object.
        /// </summary>
        private static readonly Dictionary<TransactionUserVerificationDetailDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TransactionUserVerificationDetailDTO.SearchByParameters, string>
        {
            { TransactionUserVerificationDetailDTO.SearchByParameters.TRX_USR_VERFN_DETAIL_ID , "tuvd.TrxUserVerificationDetId"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.TRX_USR_VERFN_DETAIL_ID_LIST , "tuvd.TrxUserVerificationDetId"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.VERIFICATION_ID,"tuvd.VerificationId"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.TRX_ID,"tuvd.TrxId"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.LINE_ID,"tuvd.LineId"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.ACTIVE_FLAG,"tuvd.IsActive"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.SITE_ID,"tuvd.site_id"},
            { TransactionUserVerificationDetailDTO.SearchByParameters.MASTER_ENTITY_ID,"tuvd.MasterEntityId"}
        };

        /// <summary>
        /// Parameterized Constructor for TransactionUserVerificationDetailDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public TransactionUserVerificationDetailDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TrxUserVerificationDetails Record.
        /// </summary>
        /// <param name="trxUserVerificationDetailDTO">TransactionUserVerificationDetailDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the List of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxUserVerificationDetailDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxUserVerificationDetId", trxUserVerificationDetailDTO.TrxUserVerificationDetId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxId", trxUserVerificationDetailDTO.TrxId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VerificationId", trxUserVerificationDetailDTO.VerificationId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LineId", trxUserVerificationDetailDTO.LineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", trxUserVerificationDetailDTO.ActiveFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VerificationName", trxUserVerificationDetailDTO.VerificationName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@VerificationRemarks", trxUserVerificationDetailDTO.VerificationRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxUserVerificationDetailDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }
        /// <summary>
        ///  Converts the Data row object to TransactionUserVerificationDetailDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the TransactionUserVerificationDetailDTO</returns>
        private TransactionUserVerificationDetailDTO GetTrxUserVerificationDetailDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO = new TransactionUserVerificationDetailDTO(
                                          dataRow["TrxUserVerificationDetId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxUserVerificationDetId"]),
                                          dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                          dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                          dataRow["VerificationId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VerificationId"]),
                                          dataRow["VerificationName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VerificationName"]),
                                          dataRow["VerificationRemarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["VerificationRemarks"]),
                                          dataRow["IsActive"] == DBNull.Value ? "N" : Convert.ToString(dataRow["IsActive"]),
                                          dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                          dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                          dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                          dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                          dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                          dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                          dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                          dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"])
                                        
                                          );
            log.LogMethodExit(trxUserVerificationDetailDTO);
            return trxUserVerificationDetailDTO;
        }
        /// <summary>
        /// Gets the TrxUserVerificationDetail data of passed trxUserVerificationDetId 
        /// </summary>
        /// <param name="trxUserVerificationDetId">id of TrxUserVerificationDetail is passed as parameter</param>
        /// <returns>Returns TransactionUserVerificationDetailDTO</returns>
        public TransactionUserVerificationDetailDTO GetTrxUserVerificationDetailDTO(int trxUserVerificationDetId)
        {
            log.LogMethodEntry(trxUserVerificationDetId);
            TransactionUserVerificationDetailDTO result = null;
            string query = SELECT_QUERY + @" WHERE tuvd.TrxUserVerificationDetId = @Id";
            SqlParameter parameter = new SqlParameter("@Id", trxUserVerificationDetId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxUserVerificationDetailDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        ///  Inserts the record to the TrxUserVerificationDetails Table.
        /// </summary>
        /// <param name="trxUserVerificationDetailDTO">TransactionUserVerificationDetailDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns> Returns the TransactionUserVerificationDetailDTO</returns>
        public TransactionUserVerificationDetailDTO Insert(TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxUserVerificationDetailDTO, loginId, siteId);
            string query = @"INSERT INTO [dbo].[TrxUserVerificationDetails]
                           (TrxId,
                            LineId,
                            VerificationId,
                            VerificationName,
                            VerificationRemarks,
                            IsActive,
                            CreationDate,
                            CreatedBy,
                            LastUpdateDate,
                            LastUpdatedBy,
                            site_id,
                            Guid,
                            MasterEntityId)
                     VALUES
                           (@TrxId,
                            @LineId,
                            @VerificationId,
                            @VerificationName,
                            @VerificationRemarks,
                            @IsActive,
                            GETDATE(),
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            @site_id,
                            NEWID(),
                            @MasterEntityId)
                                    SELECT * FROM TrxUserVerificationDetails WHERE TrxUserVerificationDetId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxUserVerificationDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxUserVerificationDetailDTO(trxUserVerificationDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting TransactionUserVerificationDetailDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxUserVerificationDetailDTO);
            return trxUserVerificationDetailDTO;
        }
        /// <summary>
        ///  Updates the record to the TrxUserVerificationDetails Table.
        /// </summary>
        /// <param name="trxUserVerificationDetailDTO">TransactionUserVerificationDetailDTO object is passed as parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns> Returns the TransactionUserVerificationDetailDTO</returns>
        public TransactionUserVerificationDetailDTO Update(TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxUserVerificationDetailDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxUserVerificationDetails]
                           SET                
                            TrxId              = @TrxId,
                            LineId             = @LineId,
                            VerificationId     = @VerificationId,
                            VerificationName   = @VerificationName,
                            VerificationRemarks= @VerificationRemarks,
                            IsActive           = @IsActive,
                            LastUpdateDate     = GETDATE(),
                            LastUpdatedBy      = @LastUpdatedBy,
                            MasterEntityId     = @MasterEntityId
                           where  TrxUserVerificationDetId = @TrxUserVerificationDetId
                           SELECT * FROM TrxUserVerificationDetails WHERE TrxUserVerificationDetId = @TrxUserVerificationDetId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxUserVerificationDetailDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxUserVerificationDetailDTO(trxUserVerificationDetailDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating TransactionUserVerificationDetailDTO ", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxUserVerificationDetailDTO);
            return trxUserVerificationDetailDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="trxUserVerificationDetailDTO">TransactionUserVerificationDetailDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
     
        private void RefreshTrxUserVerificationDetailDTO(TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO, DataTable dt)
        {
            log.LogMethodEntry(trxUserVerificationDetailDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxUserVerificationDetailDTO.TrxUserVerificationDetId = Convert.ToInt32(dt.Rows[0]["TrxUserVerificationDetId"]);
                trxUserVerificationDetailDTO.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                trxUserVerificationDetailDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxUserVerificationDetailDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxUserVerificationDetailDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxUserVerificationDetailDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxUserVerificationDetailDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of TransactionTaxLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of TransactionTaxLineDTO</returns>
        public List<TransactionUserVerificationDetailDTO> GetTrxUserVerificationDetailDTOList(List<KeyValuePair<TransactionUserVerificationDetailDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<TransactionUserVerificationDetailDTO> trxUserVerificationDetailDTOList = new List<TransactionUserVerificationDetailDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TransactionUserVerificationDetailDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.TRX_USR_VERFN_DETAIL_ID
                            || searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.TRX_USR_VERFN_DETAIL_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TransactionUserVerificationDetailDTO.SearchByParameters.ACTIVE_FLAG) //string 
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'N')=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
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
                    TransactionUserVerificationDetailDTO trxUserVerificationDetailDTO = GetTrxUserVerificationDetailDTO(dataRow);
                    trxUserVerificationDetailDTOList.Add(trxUserVerificationDetailDTO);
                }
            }
            log.LogMethodExit(trxUserVerificationDetailDTOList);
            return trxUserVerificationDetailDTOList;
        }
    }
}
