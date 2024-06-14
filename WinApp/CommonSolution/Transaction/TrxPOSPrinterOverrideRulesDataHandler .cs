/********************************************************************************************
 * Project Name - TrxPOSPrinterOverrideRules Datahandler
 * Description  - Datahandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *1.00        09-Dec-2020      Dakshakh Raj     Created for Peru Invoice Enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using Semnox.Parafait.POS;
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Semnox.Parafait.Transaction
{
    public class TrxPOSPrinterOverrideRulesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM TrxPOSPrinterOverrideRules AS por ";

        private static readonly Dictionary<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>
            {
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRX_POS_PRINTER_OVERRIDE_RULE_ID, "por.TrxPOSPrinterOverrideRuleID"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRANSACTION_ID, "por.TransactionId"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, "por.POSPrinterId"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_RULE_ID, "por.POSPrinterOverrideRuleId"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_OPTION_ID, "por.POSPrinterOverrideOptionID"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE, "por.OptionItemCode"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, "por.IsActive"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.MASTER_ENTITY_ID, "por.MasterEntityId"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, "por.site_id"},
                {TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRX_POS_PRINTER_OVERRIDE_RULE_ID_LIST, "ppor.TrxPOSPrinterOverrideRuleID"},
            };

        /// <summary>
        /// Parameterized Constructor for POS Printer Override Rules Datahandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public TrxPOSPrinterOverrideRulesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="TrxPOSPrinterOverrideRulesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@TrxPOSPrinterOverrideRuleID", trxPOSPrinterOverrideRulesDTO.TrxPOSPrinterOverrideRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TransactionId", trxPOSPrinterOverrideRulesDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterId", trxPOSPrinterOverrideRulesDTO.POSPrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterOverrideRuleId", trxPOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterOverrideOptionId", trxPOSPrinterOverrideRulesDTO.POSPrinterOverrideOptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionItemCode", trxPOSPrinterOverrideRulesDTO.OptionItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemSourceColumnGuid", (trxPOSPrinterOverrideRulesDTO.ItemSourceColumnGuid))); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", trxPOSPrinterOverrideRulesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", trxPOSPrinterOverrideRulesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastupdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        private TrxPOSPrinterOverrideRulesDTO GetTrxPOSPrinterOverrideRulesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO = new TrxPOSPrinterOverrideRulesDTO(
                dataRow["TrxPOSPrinterOverrideRuleID"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxPOSPrinterOverrideRuleID"]),
                dataRow["TransactionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TransactionId"]),
                dataRow["POSPrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterId"]),
                dataRow["POSPrinterOverrideRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterOverrideRuleId"]),
                dataRow["POSPrinterOverrideOptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterOverrideOptionId"]),
                dataRow["OptionItemCode"] == DBNull.Value ? POSPrinterOverrideOptionItemCode.NONE : (POSPrinterOverrideOptionItemCode)Enum.Parse(typeof(POSPrinterOverrideOptionItemCode),(dataRow["OptionItemCode"].ToString()),true),
                dataRow["ItemSourceColumnGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ItemSourceColumnGuid"]),
                dataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["IsActive"]),
                dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]),
                dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"])
                );
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTO);
            return trxPOSPrinterOverrideRulesDTO;
        }

        internal TrxPOSPrinterOverrideRulesDTO GetTrxPOSPrinterOverrideRulesDTO(int trxPOSPrinterOverrideRuleID)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRuleID);
            TrxPOSPrinterOverrideRulesDTO result = null;
            string query = SELECT_QUERY + @" WHERE por.TrxPOSPrinterOverrideRuleID = @TrxPOSPrinterOverrideRuleID";
            SqlParameter parameter = new SqlParameter("@TrxPOSPrinterOverrideRuleID", trxPOSPrinterOverrideRuleID);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTrxPOSPrinterOverrideRulesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the List of TrxPOSPrinterOverrideRulesDTO based on the TrxPOSPrinterOverrideRules Id List
        /// </summary>
        /// <param name="trxPOSPrinterOverrideRulesIdList">List of trxPOSPrinterOverrideRules Ids </param>
        /// <param name="activeRecords">activeRecords </param>
        /// <returns>returns the trxPOSPrinterOverrideRulesDTO List</returns>
        internal List<TrxPOSPrinterOverrideRulesDTO> GetTrxPOSPrinterOverrideRulesDTOList(List<int> trxPOSPrinterOverrideRules, bool activeRecords)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRules, activeRecords);
            List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = new List<TrxPOSPrinterOverrideRulesDTO>();
            string query = @"SELECT *
                            FROM TrxPOSPrinterOverrideRules, @TrxPOSPrinterOverrideRuleID List
                            WHERE TrxPOSPrinterOverrideRuleID = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSPrinterOverrideRuleId", trxPOSPrinterOverrideRules, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                trxPOSPrinterOverrideRulesDTOList = table.Rows.Cast<DataRow>().Select(x => GetTrxPOSPrinterOverrideRulesDTO(x)).ToList();
            }
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
            return trxPOSPrinterOverrideRulesDTOList;
        }
        internal void Delete(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTO);
            string query = @"DELETE  
                             FROM TrxPOSPrinterOverrideRules
                             WHERE TrxPOSPrinterOverrideRuleID = @TrxPOSPrinterOverrideRuleID";
            SqlParameter parameter = new SqlParameter("@TrxPOSPrinterOverrideRuleID",trxPOSPrinterOverrideRulesDTO.TrxPOSPrinterOverrideRuleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void RefreshTrxPOSPrinterOverrideRulesDTO(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, DataTable dt)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                trxPOSPrinterOverrideRulesDTO.TrxPOSPrinterOverrideRuleId = Convert.ToInt32(dt.Rows[0]["TrxPOSPrinterOverrideRuleId"]);
                trxPOSPrinterOverrideRulesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                trxPOSPrinterOverrideRulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                trxPOSPrinterOverrideRulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                trxPOSPrinterOverrideRulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                trxPOSPrinterOverrideRulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                trxPOSPrinterOverrideRulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal TrxPOSPrinterOverrideRulesDTO Insert(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[TrxPOSPrinterOverrideRules]
                               ([TransactionId]
                               ,[POSPrinterId]
                               ,[POSPrinterOverrideRuleId]
                               ,[POSPrinterOverrideOptionID]
                               ,[OptionItemCode]
                               ,[ItemSourceColumnGuid]
                               ,[IsActive]
                               ,[CreatedBy]
                               ,[CreationDate]
                               ,[LastUpdatedBy]
                               ,[LastUpdatedDate]
                               ,[Guid]
                               ,[site_id]
                               ,[MasterEntityId])
                               
                         VALUES
                               (
                                @TransactionId,
                                @POSPrinterId,
                                @POSPrinterOverrideRuleId,
                                @POSPrinterOverrideOptionID,
                                @OptionItemCode,
                                @ItemSourceColumnGuid,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId
                                 )
                                SELECT * FROM TrxPOSPrinterOverrideRules WHERE TrxPOSPrinterOverrideRuleID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxPOSPrinterOverrideRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxPOSPrinterOverrideRulesDTO(trxPOSPrinterOverrideRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTO);
            return trxPOSPrinterOverrideRulesDTO;
        }
        internal TrxPOSPrinterOverrideRulesDTO Update(TrxPOSPrinterOverrideRulesDTO trxPOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(trxPOSPrinterOverrideRulesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[TrxPOSPrinterOverrideRules] set 
                                    [TransactionId]                          = @TransactionId,
                                    [POSPrinterId]                           = @POSPrinterId,
                                    [POSPrinterOverrideRuleId]               = @POSPrinterOverrideRuleId,
                                    [POSPrinterOverrideOptionID]             = @POSPrinterOverrideOptionID,
                                    [OptionItemCode]                         = @OptionItemCode,
                                    [ItemSourceColumnGuid]                   = @ItemSourceColumnGuid,
                                    [IsActive]                               = @IsActive,
                                    [MasterEntityId]                         = @MasterEntityId,
                                    [LastUpdatedBy]                          = @LastUpdatedBy,
                                    [LastUpdatedDate]                        = GETDATE()
                                    where TrxPOSPrinterOverrideRuleID = @TrxPOSPrinterOverrideRuleID
                                    SELECT * FROM TrxPOSPrinterOverrideRules WHERE TrxPOSPrinterOverrideRuleID = @TrxPOSPrinterOverrideRuleID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(trxPOSPrinterOverrideRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTrxPOSPrinterOverrideRulesDTO(trxPOSPrinterOverrideRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTO);
            return trxPOSPrinterOverrideRulesDTO;
        }

        internal List<TrxPOSPrinterOverrideRulesDTO> GetTrxPOSPrinterOverrideRulesDTOList(List<KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<TrxPOSPrinterOverrideRulesDTO> trxPOSPrinterOverrideRulesDTOList = new List<TrxPOSPrinterOverrideRulesDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TrxPOSPrinterOverrideRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRX_POS_PRINTER_OVERRIDE_RULE_ID ||
                            searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID ||
                            searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRANSACTION_ID ||
                            searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_OPTION_ID ||
                            searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINETR_OVERRIDE_RULE_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(TrxPOSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1":"0")));
                        }
                        else if (searchParameter.Key == TrxPOSPrinterOverrideRulesDTO.SearchByParameters.TRX_POS_PRINTER_OVERRIDE_RULE_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " in (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
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
                trxPOSPrinterOverrideRulesDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetTrxPOSPrinterOverrideRulesDTO(x)).ToList();
            }
            log.LogMethodExit(trxPOSPrinterOverrideRulesDTOList);
            return trxPOSPrinterOverrideRulesDTOList;
        }

    }


}

