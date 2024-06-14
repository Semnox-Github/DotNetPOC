/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler - KDSOrderLineDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70          16-May-2019   Girish Kundar     Created 
 *2.70.2        10-Dec-2019   Jinto Thomas      Removed siteid from update query
 *2.140.0       27-Jun-2021   Fiona Lishal      Modified for Delivery Order enhancements for F&B and Urban Piper
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
    /// KDSOrderLineDataHandler Data Handler - Handles insert, update and select of  KDSOrderLine objects
    /// </summary>
    public class KDSOrderLineDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private readonly DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT kds.*, tl.product_id, case when tl.ProductDescription is not null
											       then tl.ProductDescription
												   else p.product_name end as product_name, p.description, tl.remarks, tl.quantity, tl.ParentLineId, 
                                                    CASE WHEN tl.OriginalLineId is NULL THEN tl.CancelledTime ELSE null END CancelledTime, pt.product_type 
                                              FROM KDSOrderEntry AS kds 
                                              INNER JOIN trx_lines tl ON tl.TrxId = kds.TrxId AND tl.LineId = kds.LineId
                                              INNER JOIN products p ON tl.product_id =  p.product_id 
                                              INNER JOIN product_type pt ON p.product_type_id = pt.product_type_id ";
        private const string ORDER_BY_QUERY = @" ORDER BY kds.LineId ASC ";
        /// <summary>
        /// Dictionary for searching Parameters for the KDSOrderLine object.
        /// </summary>
        private static readonly Dictionary<KDSOrderLineDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<KDSOrderLineDTO.SearchByParameters, string>
        {
            { KDSOrderLineDTO.SearchByParameters.ID,"kds.Id"},
            { KDSOrderLineDTO.SearchByParameters.ID_LIST,"kds.Id"},
            { KDSOrderLineDTO.SearchByParameters.LINE_ID,"kds.LineId"},
            { KDSOrderLineDTO.SearchByParameters.TERMINAL_ID,"kds.TerminalId"},
            { KDSOrderLineDTO.SearchByParameters.DISPLAY_TEMPLATE_ID,"kds.DisplayTemplateId"},
            { KDSOrderLineDTO.SearchByParameters.DELIVERED_TIME,"kds.DeliveredTime"},
            { KDSOrderLineDTO.SearchByParameters.ORDERED_TIME,"kds.OrderedTime"},
            { KDSOrderLineDTO.SearchByParameters.TRX_ID,"kds.TrxId"},
            { KDSOrderLineDTO.SearchByParameters.DISPLAY_BATCH_ID,"kds.DisplayBatchId"},
            { KDSOrderLineDTO.SearchByParameters.SITE_ID,"kds.site_id"},
            { KDSOrderLineDTO.SearchByParameters.MASTER_ENTITY_ID,"kds.MasterEntityId"},
            { KDSOrderLineDTO.SearchByParameters.KDSKOT_ENTRY_TYPE,"kds.EntryType"}
        };

        /// <summary>
        /// Parameterized Constructor for KDSOrderLineDataHandler.
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction  object</param>
        public KDSOrderLineDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating KDSOrderLine Record.
        /// </summary>
        /// <param name="kdsOrderLineDto">KDSOrderLineDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the List of SQL parameter </returns>
        private List<SqlParameter> GetSqlParameters(KDSOrderLineDTO kdsOrderLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(kdsOrderLineDto, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                dataAccessHandler.GetSQLParameter("@Id", kdsOrderLineDto.Id, true),
                dataAccessHandler.GetSQLParameter("@TerminalId", kdsOrderLineDto.TerminalId, true),
                dataAccessHandler.GetSQLParameter("@TrxId", kdsOrderLineDto.TrxId, true),
                dataAccessHandler.GetSQLParameter("@DisplayTemplateId", kdsOrderLineDto.DisplayTemplateId, true),
                dataAccessHandler.GetSQLParameter("@DeliveredTime", kdsOrderLineDto.DeliveredTime),
                dataAccessHandler.GetSQLParameter("@DisplayBatchId", kdsOrderLineDto.DisplayBatchId),
                dataAccessHandler.GetSQLParameter("@LineId", kdsOrderLineDto.LineId),
                dataAccessHandler.GetSQLParameter("@OrderedTime", kdsOrderLineDto.OrderedTime),
                dataAccessHandler.GetSQLParameter("@PreparedTime", kdsOrderLineDto.PreparedTime),
                dataAccessHandler.GetSQLParameter("@PrepareStartTime", kdsOrderLineDto.PrepareStartTime),
                dataAccessHandler.GetSQLParameter("@ScheduleTime", kdsOrderLineDto.ScheduleTime),
                dataAccessHandler.GetSQLParameter("@EntryType", kdsOrderLineDto.EntryType.ToString()),
                dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId),
                dataAccessHandler.GetSQLParameter("@site_id", siteId, true),
                dataAccessHandler.GetSQLParameter("@MasterEntityId", kdsOrderLineDto.MasterEntityId, true),
                dataAccessHandler.GetSQLParameter("@CreatedBy", loginId)
            };
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        ///  Converts the Data row object to KDSOrderLineDTO class type
        /// </summary>
        /// <param name="dataRow">dataRow object</param>
        /// <returns>Returns the KDSOrderLineDTO</returns>
        private KDSOrderLineDTO GetKDSOrderLineDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            KDSOrderLineDTO kdsOrderLineDto = new KDSOrderLineDTO(dataRow["Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["Id"]),
                                                         dataRow["TerminalId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TerminalId"]),
                                                         dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                         dataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["LineId"]),
                                                         dataRow["OrderedTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["OrderedTime"]),
                                                         dataRow["DeliveredTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["DeliveredTime"]),
                                                         dataRow["DisplayTemplateId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayTemplateId"]),
                                                         dataRow["DisplayBatchId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["DisplayBatchId"]),
                                                         dataRow["PreparedTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["PreparedTime"]),
                                                         dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                         dataRow["SynchStatus"] != DBNull.Value && Convert.ToBoolean(dataRow["SynchStatus"]),
                                                         dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                         dataRow["PrepareStartTime"] == DBNull.Value ? (DateTime?) null: Convert.ToDateTime(dataRow["PrepareStartTime"]),
                                                         dataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["product_id"]),
                                                         dataRow["product_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["product_name"]),
                                                         dataRow["product_type"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["product_type"]),
                                                         dataRow["description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["description"]),
                                                         dataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Quantity"]),
                                                         dataRow["remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["remarks"]),
                                                         dataRow["ParentLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentLineId"]),
                                                         dataRow["CancelledTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["CancelledTime"]),
                                                         dataRow["ScheduleTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ScheduleTime"]),
                                                         dataRow["EntryType"] == DBNull.Value ? KDSOrderLineDTO.KDSKOTEntryType.KDS : KDSOrderLineDTO.GetKDSKOTEntryTypeFromString(Convert.ToString(dataRow["EntryType"])), 
                                                         dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                         dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                         dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                         dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                         dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"])
                                                        );
            log.LogMethodExit(kdsOrderLineDto);
            return kdsOrderLineDto;
        }
        /// <summary>
        /// Gets the KDSOrderLine data of passed id 
        /// </summary>
        /// <param name="id">id of KDSOrderLine is passed as parameter</param>
        /// <returns>Returns KDSOrderLineDTO</returns>
        public KDSOrderLineDTO GetKDSOrderLineDTO(int id)
        {
            log.LogMethodEntry(id);
            KDSOrderLineDTO result = null;
            string query = SELECT_QUERY + @" WHERE kds.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetKDSOrderLineDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        ///  Inserts the record to the CheckInDetail Table.
        /// </summary>
        /// <param name="kdsOrderLineDto">KDSOrderLineDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the KDSOrderLineDTO</returns>
        public KDSOrderLineDTO Insert(KDSOrderLineDTO kdsOrderLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(kdsOrderLineDto, loginId, siteId);
            string query = @"INSERT INTO [dbo].[KDSOrderEntry]
                           (TerminalId,
                            TrxId,
                            LineId,
                            OrderedTime,
                            DeliveredTime,
                            DisplayTemplateId,
                            DisplayBatchId,
                            PreparedTime,
                            Guid,
                            site_id,
                            PrepareStartTime,
                            ScheduleTime,
                            EntryType,
                            MasterEntityId,
                            CreatedBy,
                            CreationDate,
                            LastUpdatedBy,
                            LastUpdateDate)
                     VALUES
                           (@TerminalId,
                            @TrxId,
                            @LineId,
                            @OrderedTime,
                            @DeliveredTime,
                            @DisplayTemplateId,
                            @DisplayBatchId,
                            @PreparedTime,
                            NEWID(),
                            @site_id,
                            @PrepareStartTime,
                            @ScheduleTime,
                            @EntryType,
                            @MasterEntityId,
                            @CreatedBy,
                            GETDATE(),
                            @LastUpdatedBy,
                            GETDATE() ) " + SELECT_QUERY + " WHERE kds.Id = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(kdsOrderLineDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshKDSOrderLineDTO(kdsOrderLineDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting CheckInDetailDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(kdsOrderLineDto);
            return kdsOrderLineDto;
        }

        /// <summary>
        ///  Updates the record to the KDSOrderLine Table.
        /// </summary>
        /// <param name="kdsOrderLineDto">KDSOrderLineDTO object passed as parameter</param>
        /// <param name="loginId"> login id</param>
        /// <param name="siteId">site id</param>
        /// <returns>Returns the KDSOrderLineDTO</returns>
        public KDSOrderLineDTO Update(KDSOrderLineDTO kdsOrderLineDto, string loginId, int siteId)
        {
            log.LogMethodEntry(kdsOrderLineDto, loginId, siteId);
            string query = @"UPDATE  [dbo].[KDSOrderEntry]
                           SET 
                            TerminalId        = @TerminalId,
                            TrxId             = @TrxId,
                            LineId            = @LineId,
                            OrderedTime       = @OrderedTime,
                            DeliveredTime     = @DeliveredTime,
                            DisplayTemplateId = @DisplayTemplateId,
                            DisplayBatchId    = @DisplayBatchId,
                            PreparedTime      = @PreparedTime,
                            ScheduleTime      = @ScheduleTime,
                            -- site_id           = @site_id,
                            PrepareStartTime  = @PrepareStartTime,
                            EntryType         = @EntryType,
                            MasterEntityId    = @MasterEntityId,
                            LastUpdatedBy     = @LastUpdatedBy,
                            LastUpdateDate    = GETDATE() 
                           WHERE Id  = @Id " + SELECT_QUERY + " WHERE kds.Id = @Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSqlParameters(kdsOrderLineDto, loginId, siteId).ToArray(), sqlTransaction);
                RefreshKDSOrderLineDTO(kdsOrderLineDto, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting KDSOrderLineDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(kdsOrderLineDto);
            return kdsOrderLineDto;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="kdsOrderLineDto">KDSOrderLineDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
      
        private void RefreshKDSOrderLineDTO(KDSOrderLineDTO kdsOrderLineDto, DataTable dt)
        {
            log.LogMethodEntry(kdsOrderLineDto, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                kdsOrderLineDto.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                kdsOrderLineDto.LastUpdatedDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                kdsOrderLineDto.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                kdsOrderLineDto.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                kdsOrderLineDto.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                kdsOrderLineDto.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                kdsOrderLineDto.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
                kdsOrderLineDto.ProductId = dataRow["product_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["product_id"]);
                kdsOrderLineDto.ProductName = dataRow["product_name"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["product_name"]);
                kdsOrderLineDto.ProductDescription = dataRow["description"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["description"]);
                kdsOrderLineDto.Quantity = dataRow["Quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(dataRow["Quantity"]);
                kdsOrderLineDto.TransactionLineRemarks = dataRow["remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["remarks"]);
                kdsOrderLineDto.ParentLineId = dataRow["ParentLineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentLineId"]);
                kdsOrderLineDto.LineCancelledTime = dataRow["CancelledTime"] == DBNull.Value ? (DateTime?) null : Convert.ToDateTime(dataRow["CancelledTime"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the List of KDSOrderLineDTO based on the search parameters.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <returns>Returns the List of KDSOrderLineDTO</returns>
        public List<KDSOrderLineDTO> GetKDSOrderLineDTOList(List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<KDSOrderLineDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    string joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == KDSOrderLineDTO.SearchByParameters.ID
                            || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.DISPLAY_TEMPLATE_ID
                            || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.LINE_ID
                            || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.TERMINAL_ID
                            || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.TRX_ID
                            || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == KDSOrderLineDTO.SearchByParameters.SITE_ID ||
                                 searchParameter.Key == KDSOrderLineDTO.SearchByParameters.DISPLAY_BATCH_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == KDSOrderLineDTO.SearchByParameters.ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == KDSOrderLineDTO.SearchByParameters.ORDERED_TIME
                                || searchParameter.Key == KDSOrderLineDTO.SearchByParameters.DELIVERED_TIME)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) = " + dataAccessHandler.GetParameterName(searchParameter.Key));

                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == KDSOrderLineDTO.SearchByParameters.KDSKOT_ENTRY_TYPE)
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
                    counter++;
                }
                selectQuery = selectQuery + query + ORDER_BY_QUERY;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            List<KDSOrderLineDTO> kdsOrderLineDtoList = GetKDSOrderLineDTOList(dataTable);
            log.LogMethodExit(kdsOrderLineDtoList);
            return kdsOrderLineDtoList;
        }

        /// <summary>
        /// Returns the  List of KDSOrderLineDTO from the DataTable Object. 
        /// </summary>
        /// <param name="dataTable">dataTable object</param>
        /// <returns> List of KDSOrderLineDTO </returns>
        private List<KDSOrderLineDTO> GetKDSOrderLineDTOList(DataTable dataTable)
        {
            log.LogMethodEntry(dataTable);
            List<KDSOrderLineDTO> kdsOrderLineDTOList = new List<KDSOrderLineDTO>();
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    KDSOrderLineDTO kdsOrderLineDTO = GetKDSOrderLineDTO(dataRow);
                    kdsOrderLineDTOList.Add(kdsOrderLineDTO);
                }
            }
            log.LogMethodExit(kdsOrderLineDTOList);
            return kdsOrderLineDTOList;
        }

        /// <summary>
        /// Gets the List of KDSOrderLineDTO based on the displayBatchId List
        /// </summary>
        /// <param name="displayBatchIdSet">displayBatchIdSet is the list of displayBatch Ids</param>
        /// <returns>returns the KDSOrderLineDTO List</returns>
        public List<KDSOrderLineDTO> GetKDSOrderLineDTOList(List<int> displayBatchIdSet)
        {
            log.LogMethodEntry(displayBatchIdSet);
            string query = SELECT_QUERY + " INNER JOIN @DisplayBatchIdList List ON kds.DisplayBatchId = List.Id " + ORDER_BY_QUERY;
            DataTable dataTable = dataAccessHandler.BatchSelect(query, "@DisplayBatchIdList", displayBatchIdSet, null, sqlTransaction);
            List<KDSOrderLineDTO> kdsOrderLineDTOList = GetKDSOrderLineDTOList(dataTable);
            log.LogMethodExit(kdsOrderLineDTOList);
            return kdsOrderLineDTOList;
        }
    }

}
