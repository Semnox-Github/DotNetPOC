/********************************************************************************************
 * Project Name - POSPrinterOverrideRules Datahandler
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
using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// POS Printer Override Rules DataHandler
    /// </summary>
    public class POSPrinterOverrideRulesDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM POSPrinterOverrideRules AS ppor ";

        private static readonly Dictionary<POSPrinterOverrideRulesDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<POSPrinterOverrideRulesDTO.SearchByParameters, string>
            {
                {POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID, "ppor.POSPrinterOverrideRuleId"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID, "ppor.POSPrinterId"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID, "ppor.POSPrinterOverrideOptionID"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE, "ppor.OptionItemCode"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE, "ppor.IsActive"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.MASTER_ENTITY_ID, "ppor.MasterEntityId"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID, "ppor.site_id"},
                {POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID_LIST, "ppor.POSPrinterOverrideRuleId"},
            };

        /// <summary>
        /// Parameterized Constructor for POS Printer Override Rules Datahandler.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction Object</param>
        public POSPrinterOverrideRulesDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get SQL Parameters
        /// </summary>
        /// <param name="POSPrinterOverrideRulesDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private List<SqlParameter> GetSQLParameters(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterOverrideRuleId", pOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterId", pOSPrinterOverrideRulesDTO.POSPrinterId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSPrinterOverrideOptionId", pOSPrinterOverrideRulesDTO.POSPrinterOverrideOptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OptionItemCode", pOSPrinterOverrideRulesDTO.OptionItemCode));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ItemSourceColumnGuid", pOSPrinterOverrideRulesDTO.ItemSourceColumnGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DefaultOption", pOSPrinterOverrideRulesDTO.DefaultOption));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IsActive", pOSPrinterOverrideRulesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SiteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", pOSPrinterOverrideRulesDTO.MasterEntityId, true));
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
        private POSPrinterOverrideRulesDTO GetPOSPrinterOverrideRulesDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO = new POSPrinterOverrideRulesDTO(
                dataRow["POSPrinterOverrideRuleId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterOverrideRuleId"]),
                dataRow["POSPrinterId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterId"]),
                dataRow["POSPrinterOverrideOptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSPrinterOverrideOptionId"]),
                dataRow["OptionItemCode"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OptionItemCode"]),
                dataRow["ItemSourceColumnGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ItemSourceColumnGuid"]),
                dataRow["DefaultOption"] == DBNull.Value ? true : Convert.ToBoolean(dataRow["DefaultOption"]),
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
            log.LogMethodExit(pOSPrinterOverrideRulesDTO);
            return pOSPrinterOverrideRulesDTO;
        }

        internal POSPrinterOverrideRulesDTO GetPOSPrinterOverrideRulesDTO(int pOSPrinterOverrideRuleId)
        {
            log.LogMethodEntry(pOSPrinterOverrideRuleId);
            POSPrinterOverrideRulesDTO result = null;
            string query = SELECT_QUERY + @" WHERE ppor.POSPrinterOverrideRuleId = @POSPrinterOverrideRuleId";
            SqlParameter parameter = new SqlParameter("@POSPrinterOverrideRuleId", pOSPrinterOverrideRuleId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetPOSPrinterOverrideRulesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the List of POSPrinterOverrideRulesDTO based on the POSPrinterOverrideRules Id List
        /// </summary>
        /// <param name="pOSPrinterOverrideRulesIdList">List of POSPrinterOverrideRules Ids </param>
        /// <param name="activeRecords">activeRecords </param>
        /// <returns>returns the POSPrinterOverrideRulesDTO List</returns>
        internal List<POSPrinterOverrideRulesDTO> GetPOSPrinterOverrideRulesDTOList(List<int> pOSPrinterOverrideRulesIdList, bool activeRecords)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesIdList, activeRecords);
            List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
            string query = @"SELECT *
                            FROM POSPrinterOverrideRules, @POSPrinterOverrideRuleId List
                            WHERE POSPrinterOverrideRuleId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@POSPrinterOverrideRuleId", pOSPrinterOverrideRulesIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                pOSPrinterOverrideRulesDTOList = table.Rows.Cast<DataRow>().Select(x => GetPOSPrinterOverrideRulesDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
            return pOSPrinterOverrideRulesDTOList;
        }
        internal void Delete(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTO);
            string query = @"DELETE  
                             FROM POSPrinterOverrideRules
                             WHERE POSPrinterOverrideRuleId = @POSPrinterOverrideRuleId";
            SqlParameter parameter = new SqlParameter("@POSPrinterOverrideRuleId", pOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void RefreshPOSPrinterOverrideRulesDTO(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO, DataTable dt)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                pOSPrinterOverrideRulesDTO.POSPrinterOverrideRuleId = Convert.ToInt32(dt.Rows[0]["POSPrinterOverrideRuleId"]);
                pOSPrinterOverrideRulesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                pOSPrinterOverrideRulesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                pOSPrinterOverrideRulesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                pOSPrinterOverrideRulesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                pOSPrinterOverrideRulesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                pOSPrinterOverrideRulesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        internal POSPrinterOverrideRulesDTO Insert(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[POSPrinterOverrideRules]
                               ([POSPrinterId]
                               ,[POSPrinterOverrideOptionID]
                               ,[OptionItemCode]
                               ,[ItemSourceColumnGuid]
                               ,[DefaultOption]
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
                                @POSPrinterId,
                                @POSPrinterOverrideOptionID,
                                @OptionItemCode,
                                @ItemSourceColumnGuid,
                                @DefaultOption,
                                @IsActive,
                                @CreatedBy,
                                GETDATE(),
                                @LastUpdatedBy,
                                GETDATE(),
                                NEWID(), 
                                @SiteId,
                                @MasterEntityId
                                 )
                                SELECT * FROM POSPrinterOverrideRules WHERE POSPrinterOverrideRuleId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterOverrideRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterOverrideRulesDTO(pOSPrinterOverrideRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterOverrideRulesDTO);
            return pOSPrinterOverrideRulesDTO;
        }
        internal POSPrinterOverrideRulesDTO Update(POSPrinterOverrideRulesDTO pOSPrinterOverrideRulesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(pOSPrinterOverrideRulesDTO, loginId, siteId);
            string query = @"UPDATE [dbo].[POSPrinterOverrideRules] set 
                                    [POSPrinterId]                           = @POSPrinterId,
                                    [POSPrinterOverrideOptionID]             = @POSPrinterOverrideOptionID,
                                    [OptionItemCode]                         = @OptionItemCode,
                                    [ItemSourceColumnGuid]                   = @ItemSourceColumnGuid,
                                    [DefaultOption]                          = @DefaultOption,
                                    [IsActive]                               = @IsActive,
                                    [MasterEntityId]                         = @MasterEntityId,
                                    [LastUpdatedBy]                          = @LastUpdatedBy,
                                    [LastUpdatedDate]                        = GETDATE()
                                    where POSPrinterOverrideRuleId = @POSPrinterOverrideRuleId
                                    SELECT * FROM POSPrinterOverrideRules WHERE POSPrinterOverrideRuleId = @POSPrinterOverrideRuleId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(pOSPrinterOverrideRulesDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshPOSPrinterOverrideRulesDTO(pOSPrinterOverrideRulesDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(pOSPrinterOverrideRulesDTO);
            return pOSPrinterOverrideRulesDTO;
        }

        internal List<POSPrinterOverrideRulesDTO> GetPOSPrinterOverrideRulesDTOList(List<KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<SqlParameter> parameters = new List<SqlParameter>();
            List<POSPrinterOverrideRulesDTO> pOSPrinterOverrideRulesDTOList = new List<POSPrinterOverrideRulesDTO>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner = string.Empty;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<POSPrinterOverrideRulesDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID ||
                            searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_ID ||
                            searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_OPTION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(POSPrinterOverrideRulesDTO.SearchByParameters.OPTION_ITEM_CODE))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.IS_ACTIVE)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ((searchParameter.Value == "1" || searchParameter.Value == "Y") ? "1" : "0")));
                        }
                        else if (searchParameter.Key == POSPrinterOverrideRulesDTO.SearchByParameters.POS_PRINTER_OVERRIDE_RULE_ID_LIST)
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
                pOSPrinterOverrideRulesDTOList = dataTable.Rows.Cast<DataRow>().Select(x => GetPOSPrinterOverrideRulesDTO(x)).ToList();
            }
            log.LogMethodExit(pOSPrinterOverrideRulesDTOList);
            return pOSPrinterOverrideRulesDTOList;
        }

    }


}
