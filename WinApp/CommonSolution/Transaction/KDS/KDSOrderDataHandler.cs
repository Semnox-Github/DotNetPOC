/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - KDSOrderDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        16-May-2019   Girish Kundar           Created
 *2.140.0     27-Jun-2021    Fiona Lishal        Modified for Delivery Order enhancements for F&B and Urban Piper
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction.KDS
{
    /// <summary>
    /// KDSOrderDataHandler Data Handler - Handles insert, update and select of  KDSOrder objects
    /// </summary>
    public class KDSOrderDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @" SELECT DISTINCT kds.TrxId, kds.DisplayBatchId, kds.DisplayTemplateId, kds.TerminalId,
                                               oh.TableNumber, tp.ProfileName, h.trx_no, 
                                               (SELECT MAX(deliveredTime) 
                                                FROM KDSOrderEntry
                                                WHERE TrxId = kds.TrxId 
                                                AND DisplayBatchId = kds.DisplayBatchId 
                                                AND NOT EXISTS(SELECT 1  
                                                               FROM KDSOrderEntry, trx_lines 
                                                               WHERE KDSOrderEntry.TrxId = kds.TrxId 
                                                               AND trx_lines.TrxId = kds.TrxId 
                                                               AND KDSOrderEntry.LineId = trx_lines.LineId
                                                               AND trx_lines.CancelledTime IS NULL        
                                                               AND KDSOrderEntry.DisplayBatchId = kds.DisplayBatchId 
                                                               AND KDSOrderEntry.deliveredTime IS NULL) ) deliveredTime, 
                                               (SELECT MAX(PreparedTime) 
                                                FROM KDSOrderEntry 
                                                WHERE TrxId = kds.TrxId 
                                                AND DisplayBatchId = kds.DisplayBatchId 
                                                AND NOT EXISTS(SELECT 1  
                                                               FROM KDSOrderEntry, trx_lines 
                                                               WHERE KDSOrderEntry.TrxId = kds.TrxId 
                                                               AND trx_lines.TrxId = kds.TrxId 
                                                               AND KDSOrderEntry.LineId = trx_lines.LineId
                                                               AND trx_lines.CancelledTime IS NULL        
                                                               AND KDSOrderEntry.DisplayBatchId = kds.DisplayBatchId 
                                                               AND KDSOrderEntry.PreparedTime IS NULL)) PreparedTime,
                                               (SELECT min(OrderedTime) FROM KDSOrderEntry WHERE TrxId = kds.TrxId AND DisplayBatchId = kds.DisplayBatchId) OrderedTime
                                               FROM KDSOrderEntry kds
                                               INNER JOIN trx_header h ON kds.TrxId = h.TrxId
                                               LEFT OUTER JOIN OrderHeader oh ON oh.OrderId = h.OrderId
                                               LEFT OUTER JOIN TrxProfiles tp ON tp.TrxProfileId = h.TrxProfileId ";
        /// <summary>
        /// Dictionary for searching Parameters for the KDSOrder object.
        /// </summary>
        private static readonly Dictionary<KDSOrderDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<KDSOrderDTO.SearchByParameters, string>
        {
            { KDSOrderDTO.SearchByParameters.TERMINAL_ID,"kds.TerminalId"},
            { KDSOrderDTO.SearchByParameters.DISPLAY_TEMPLATE_ID,"kds.DisplayTemplateId"},
            { KDSOrderDTO.SearchByParameters.ORDERED_TIME_EQUAL_TO,"kds.OrderedTime"},
            { KDSOrderDTO.SearchByParameters.TRANSACTION_ID,"kds.TrxId"},
            { KDSOrderDTO.SearchByParameters.POS_MACHINE_ID,"h.POSMachineId"},
            { KDSOrderDTO.SearchByParameters.TABLE_NUMBER,"oh.TableNumber"},
            { KDSOrderDTO.SearchByParameters.DISPLAY_BATCH_ID,"kds.DisplayBatchId"},
            { KDSOrderDTO.SearchByParameters.DISPLAY_BATCH_ID_LIST,"kds.DisplayBatchId"},
            { KDSOrderDTO.SearchByParameters.PREPARED_TIME_GREATER_THAN,"kds.PreparedTime"},
            { KDSOrderDTO.SearchByParameters.DELIVERED_TIME_GREATER_THAN,"kds.DeliveredTime"},
            { KDSOrderDTO.SearchByParameters.PREPARED_TIME_NOT_NULL,"kds.PreparedTime"},
            { KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NOT_NULL,"kds.DeliveredTime"},
            { KDSOrderDTO.SearchByParameters.PREPARED_TIME_NULL,"kds.PreparedTime"},
            { KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NULL,"kds.DeliveredTime"},
            { KDSOrderDTO.SearchByParameters.SITE_ID,"kds.site_id"},
            { KDSOrderDTO.SearchByParameters.KDSKOT_ENTRY_TYPE,"kds.EntryType"},
            { KDSOrderDTO.SearchByParameters.HAS_SCHEDULED_TIME,"kds.ScheduleTime"}, 
            { KDSOrderDTO.SearchByParameters.SCHEDULED_TIME_GREATER_THAN,"kds.ScheduleTime"},
            { KDSOrderDTO.SearchByParameters.SCHEDULED_TIME_LESS_THAN,"kds.ScheduleTime"} 
        };

        /// <summary>
        /// Parameterized Constructor for KDSOrderDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction  object</param>
        public KDSOrderDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Converts the Data row object to KDSOrderDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the KDSOrderDTO</returns>
        private KDSOrderDTO GetKDSOrderDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            KDSOrderDTO kdsOrderDto = new KDSOrderDTO(dataRow["DisplayBatchId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayBatchId"]),
                                                         dataRow["TerminalId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TerminalId"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["OrderedTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["OrderedTime"]),
                                                         dataRow["DisplayTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayTemplateId"]),
                                                         dataRow["TableNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableNumber"]),
                                                         dataRow["ProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProfileName"]),
                                                         dataRow["trx_no"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["trx_no"]),
                                                         dataRow["DeliveredTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["DeliveredTime"]),
                                                         dataRow["PreparedTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["PreparedTime"])
                                                        );
            log.LogMethodExit(kdsOrderDto);
            return kdsOrderDto;
        }

        /// <summary>
        /// Gets the KDSOrder data of passed id 
        /// </summary>
        /// <param name="transactionId">transaction id</param>
        /// <param name="displayBatchId">displayBatchId of KDSOrder is passed as parameter</param>
        /// <returns>Returns KDSOrderDTO</returns>
        public KDSOrderDTO GetKDSOrderDTO(int transactionId, int displayBatchId)
        {
            log.LogMethodEntry(displayBatchId);
            KDSOrderDTO result = null;
            string query = SELECT_QUERY + @" WHERE (kds.DisplayBatchId = @DisplayBatchId OR @DisplayBatchId = -1) AND kds.TrxId = @TrxId ";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { new SqlParameter("@DisplayBatchId", displayBatchId), new SqlParameter("@TrxId", transactionId) }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetKDSOrderDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the List of KDSOrderDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of KDSOrderDTO</returns>
        public List<KDSOrderDTO> GetKDSOrderDTOList(List<KeyValuePair<KDSOrderDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<KDSOrderDTO> kdsOrderDtoList = new List<KDSOrderDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(@" WHERE EXISTS (select 1 
                                                                         from trx_lines 
                                                                         where trxId = kds.TrxId) 
                                                                 AND (NOT EXISTS (SELECT 1 
				                                                                  FROM trx_lines 
				                                                                  WHERE trx_lines.LineId = kds.LineId 
				                                                                  AND trx_lines.TrxId = kds.TrxId 
				                                                                  AND trx_lines.CancelledTime IS NOT NULL) 
		                                                                OR
		                                                                NOT EXISTS(SELECT 1 
				                                                                  FROM trx_lines, KDSOrderEntry 
				                                                                  WHERE trx_lines.LineId = KDSOrderEntry.LineId 
				                                                                  AND trx_lines.TrxId = KDSOrderEntry.TrxId 
				                                                                  AND trx_lines.CancelledTime IS NULL 
				                                                                  AND KDSOrderEntry.TrxId = kds.TrxId 
				                                                                  AND KDSOrderEntry.DisplayBatchId = kds.DisplayBatchId)) ");
                foreach (KeyValuePair<KDSOrderDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    string joiner = " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == KDSOrderDTO.SearchByParameters.DISPLAY_TEMPLATE_ID
                            || searchParameter.Key == KDSOrderDTO.SearchByParameters.TERMINAL_ID
                            || searchParameter.Key == KDSOrderDTO.SearchByParameters.TRANSACTION_ID
                            || searchParameter.Key == KDSOrderDTO.SearchByParameters.DISPLAY_BATCH_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == KDSOrderDTO.SearchByParameters.POS_MACHINE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.DISPLAY_BATCH_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.TABLE_NUMBER && 
                                 string.IsNullOrWhiteSpace(searchParameter.Value) == false)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.ORDERED_TIME_EQUAL_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key));

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.PREPARED_TIME_GREATER_THAN ||
                                 searchParameter.Key == KDSOrderDTO.SearchByParameters.DELIVERED_TIME_GREATER_THAN ||
                                 searchParameter.Key == KDSOrderDTO.SearchByParameters.SCHEDULED_TIME_GREATER_THAN  )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " > " + dataAccessHandler.GetParameterName(searchParameter.Key));

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.PREPARED_TIME_NOT_NULL ||
                                 searchParameter.Key == KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NOT_NULL )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IS NOT NULL ");
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.HAS_SCHEDULED_TIME)
                        {
                            query.Append(joiner + " CASE WHEN "+ DBSearchParameters[searchParameter.Key] + " IS NOT NULL THEN 1 ELSE 0 END = " 
                                                               + ((searchParameter.Value == "Y" || searchParameter.Value == "1") ? "1" : "0" ));
                        }
                        else if ( searchParameter.Key == KDSOrderDTO.SearchByParameters.SCHEDULED_TIME_LESS_THAN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " < " + dataAccessHandler.GetParameterName(searchParameter.Key));

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.PREPARED_TIME_NULL ||
                                 searchParameter.Key == KDSOrderDTO.SearchByParameters.DELIVERED_TIME_NULL )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IS NULL ");
                        }
                        else if (searchParameter.Key == KDSOrderDTO.SearchByParameters.KDSKOT_ENTRY_TYPE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'KDS') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
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
                }
                selectQuery = selectQuery + query;
                selectQuery += " order by deliveredTime desc, OrderedTime ";
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    KDSOrderDTO kdsOrderDto = GetKDSOrderDTO(dataRow);
                    kdsOrderDtoList.Add(kdsOrderDto);
                }
            }
            log.LogMethodExit(kdsOrderDtoList);
            return kdsOrderDtoList;
        }

        /// <summary>
        /// Refreshes the KDSOrderDTO with the values from the DB
        /// </summary>
        /// <param name="kdsOrderDto">kds Order Dto</param>
        /// <returns></returns>
        public KDSOrderDTO RefreshDTO(KDSOrderDTO kdsOrderDto)
        {
            log.LogMethodEntry(kdsOrderDto);
            if (kdsOrderDto.DisplayBatchId < 0)
            {
                log.LogMethodExit(kdsOrderDto, "Display batch id is empty.");
                return kdsOrderDto;
            }
            string query = SELECT_QUERY + @" WHERE kds.DisplayBatchId = @DisplayBatchId AND kds.TrxId = @TrxId";
            DataTable dt = dataAccessHandler.executeSelectQuery(query, new[] { new SqlParameter("@DisplayBatchId", kdsOrderDto.DisplayBatchId), new SqlParameter("@TrxId", kdsOrderDto.TransactionId) }, sqlTransaction);
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(kdsOrderDto, "Unable to find a KDS order with display batch id : " + kdsOrderDto.DisplayBatchId + ".");
                return kdsOrderDto;
            }
            DataRow dataRow = dt.Rows[0];
            kdsOrderDto.TerminalId = dataRow["TerminalId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TerminalId"]);
            kdsOrderDto.TransactionId = dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]);
            kdsOrderDto.OrderedTime = dataRow["OrderedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderedTime"]);
            kdsOrderDto.DisplayTemplateId = dataRow["DisplayTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayTemplateId"]);
            kdsOrderDto.TableNumber = dataRow["TableNumber"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["TableNumber"]);
            kdsOrderDto.TransactionProfileName = dataRow["ProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ProfileName"]);
            kdsOrderDto.TransactionNumber = dataRow["trx_no"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["trx_no"]);
            kdsOrderDto.DeliveredTime = dataRow["deliveredTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["deliveredTime"]);
            kdsOrderDto.PreparedTime = dataRow["PreparedTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["PreparedTime"]);
            log.LogMethodExit(kdsOrderDto);
            return kdsOrderDto;
        }
    }

}
