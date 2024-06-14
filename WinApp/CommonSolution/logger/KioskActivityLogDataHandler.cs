/********************************************************************************************
 * Project Name - Kiosk activity log Data Handler
 * Description  - Data handler of the Kiosk activity log  data handler class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        16-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.logger
{
    /// <summary>
    ///  KioskActivityLog Data Handler - Handles insert, update and select of  KioskActivityLog objects
    /// </summary>
    public class KioskActivityLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM KioskActivityLog AS kal ";

        /// <summary>
        /// Default constructor of KioskActivityLogDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public KioskActivityLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// For search parameter Specified
        /// </summary>
        private static readonly Dictionary<KioskActivityLogDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<KioskActivityLogDTO.SearchByParameters, string>
        {
            {KioskActivityLogDTO.SearchByParameters.CARD_NUMBER,"kal.CardNumber"},
            {KioskActivityLogDTO.SearchByParameters.KIOSK_ACTIVITY_LOG_ID,"kal.KioskActivityLogId"},
            {KioskActivityLogDTO.SearchByParameters.KIOSK_ID,"kal.KioskId"},
            {KioskActivityLogDTO.SearchByParameters.SITE_ID,"kal.site_id"},
            {KioskActivityLogDTO.SearchByParameters.KIOSK_NAME,"kal.KioskName"},
            {KioskActivityLogDTO.SearchByParameters.ACTIVITY,"kal.Activity"},
            {KioskActivityLogDTO.SearchByParameters.FROM_TIME_STAMP,"kal.TimeStamp"},
            {KioskActivityLogDTO.SearchByParameters.TO_TIME_STAMP,"kal.TimeStamp"},
        };

        /// <summary>
        /// ParameterHelper
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterName">parameterName</param>
        /// <param name="value">value</param>
        /// <param name="negetiveValueNull">negetiveValueNull</param>
        private void ParameterHelper(List<SqlParameter> parameters, string parameterName, object value, bool negetiveValueNull = false)
        {
            log.LogMethodEntry();
            if (parameters != null && !string.IsNullOrEmpty(parameterName))
            {
                if (value is int)
                {
                    if (negetiveValueNull && ((int)value) < 0)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else if (value is string)
                {
                    if (string.IsNullOrEmpty(value as string))
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
                else
                {
                    if (value == null)
                    {
                        parameters.Add(new SqlParameter(parameterName, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(parameterName, value));
                    }
                }
            }
            log.LogMethodExit();
        }

        private void PassParametersHelper(List<SqlParameter> parameters, KioskActivityLogDTO kioskActivityLogDTO, string userId, int siteId)
        {
            log.LogMethodEntry(parameters, kioskActivityLogDTO, userId, siteId);
            ParameterHelper(parameters, "@NoteCoinFlag", kioskActivityLogDTO.NoteCoinFlag);
            ParameterHelper(parameters, "@Value", kioskActivityLogDTO.Value);
            ParameterHelper(parameters, "@Activity", kioskActivityLogDTO.Activity);
            ParameterHelper(parameters, "@CardNumber", kioskActivityLogDTO.CardNumber);
            ParameterHelper(parameters, "@Message", kioskActivityLogDTO.Message);
            ParameterHelper(parameters, "@KioskId", kioskActivityLogDTO.KioskId, true);
            ParameterHelper(parameters, "@KioskName", kioskActivityLogDTO.KioskName);
            ParameterHelper(parameters, "@site_id", siteId, true);
            ParameterHelper(parameters, "@TrxId", kioskActivityLogDTO.TrxId, true);
            ParameterHelper(parameters, "@KioskTrxId", kioskActivityLogDTO.TrxId, true);
            ParameterHelper(parameters, "@MasterEntityId", kioskActivityLogDTO.MasterEntityId, true);
            ParameterHelper(parameters, "@CreatedBy", userId);
            ParameterHelper(parameters, "@LastUpdatedBy", userId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the KioskActivityLog record to the database
        /// </summary>
        /// <param name="kioskActivityLogDTO">KioskActivityLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public KioskActivityLogDTO InsertKioskActivityLog(KioskActivityLogDTO kioskActivityLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(kioskActivityLogDTO, loginId, siteId);
            string query = @"INSERT INTO KioskActivityLog 
                                        (
                                            TimeStamp,
                                            NoteCoinFlag,
                                            Value,
                                            Activity,
                                            CardNumber,
                                            Message,
                                            KioskId,
                                            KioskName,                                            
                                            site_id,
                                            TrxId,
                                            KioskTrxId,
                                            MasterEntityId,
                                            LastUpdatedBy,
                                            LastUpdateDate,
                                            CreatedBy,
                                            CreationDate
                                        )             
                                VALUES 
                                        (                                            
                                            GETDATE(),
                                            @NoteCoinFlag,
                                            @Value,
                                            @Activity,
                                            @CardNumber,
                                            @Message,
                                            @KioskId,
                                            @KioskName,                                            
                                            @site_id,
                                            @TrxId,
                                            @KioskTrxId,
                                            @MasterEntityId,
                                            @LastUpdatedBy,
                                            getdate(),
                                            @CreatedBy,
                                            getdate()
                                        )SELECT * FROM KioskActivityLog WHERE KioskActivityLogId = scope_identity()"; 
            List<SqlParameter> parameters = new List<SqlParameter>();
            PassParametersHelper(parameters, kioskActivityLogDTO, loginId, siteId);
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                RefreshLocationTypeDTO(kioskActivityLogDTO, dt);

            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating KioskActivityLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }


            log.LogMethodExit(kioskActivityLogDTO);
            return kioskActivityLogDTO;
        }
        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="kioskActivityLogDTO"></param>
        /// <param name="dt"></param>
        private void RefreshLocationTypeDTO(KioskActivityLogDTO kioskActivityLogDTO, DataTable dt)
        {
            log.LogMethodEntry(kioskActivityLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                kioskActivityLogDTO.KioskActivityLogId = Convert.ToInt32(dt.Rows[0]["KioskActivityLogId"]);
                kioskActivityLogDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                kioskActivityLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                kioskActivityLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                kioskActivityLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                kioskActivityLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                kioskActivityLogDTO.site_id = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// return the record from the database
        /// Convert the datarow to KioskActivityLogDTO object
        /// </summary>
        /// <returns>return the KioskActivityLogDTO object</returns>
        private KioskActivityLogDTO GetKioskActivityLogDTO(DataRow paymentDataRow)
        {
            log.LogMethodEntry(paymentDataRow);

            KioskActivityLogDTO kioskActivityLogDTO = new KioskActivityLogDTO(
                                                    paymentDataRow["KioskActivityLogId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["KioskActivityLogId"]),
                                                    paymentDataRow["NoteCoinFlag"] == DBNull.Value ? string.Empty : paymentDataRow["NoteCoinFlag"].ToString(),
                                                    paymentDataRow["TimeStamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentDataRow["TimeStamp"]),
                                                    paymentDataRow["Activity"] == DBNull.Value ? string.Empty : paymentDataRow["Activity"].ToString(),
                                                    paymentDataRow["Value"] == DBNull.Value ? (double?)null : Convert.ToDouble(paymentDataRow["Value"]),
                                                    paymentDataRow["CardNumber"] == DBNull.Value ? string.Empty : paymentDataRow["CardNumber"].ToString(),
                                                    paymentDataRow["Message"] == DBNull.Value ? string.Empty : paymentDataRow["Message"].ToString(),
                                                    paymentDataRow["KioskId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["KioskId"]),
                                                    paymentDataRow["KioskName"] == DBNull.Value ? string.Empty : paymentDataRow["KioskName"].ToString(),
                                                    paymentDataRow["Guid"].ToString(),
                                                    paymentDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(paymentDataRow["SynchStatus"]),
                                                    paymentDataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["TrxId"]),
                                                    paymentDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["site_id"]),
                                                    paymentDataRow["KioskTrxId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["KioskTrxId"]),
                                                    paymentDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(paymentDataRow["MasterEntityId"]),
                                                    paymentDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["CreatedBy"]),
                                                    paymentDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentDataRow["CreationDate"]),
                                                    paymentDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(paymentDataRow["LastUpdatedBy"]),
                                                    paymentDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(paymentDataRow["LastUpdateDate"])

                                                 );
            log.LogMethodExit(kioskActivityLogDTO);
            return kioskActivityLogDTO;

        }


        /// <summary>
        /// Gets the KioskActivityLogDTO  matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of  KioskActivityLogDTO matching the search criteria</returns>
        internal List<KioskActivityLogDTO> GetKioskActivityLogList(List<KeyValuePair<KioskActivityLogDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectKioskActivityLogsQuery = SELECT_QUERY;
            List<KioskActivityLogDTO> kioskActivityLogDTOList = new List<KioskActivityLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<KioskActivityLogDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? string.Empty : "  and ";

                        if (searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.CARD_NUMBER) ||
                            searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.KIOSK_NAME) ||
                            searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.ACTIVITY))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.KIOSK_ID)
                        || searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.KIOSK_ACTIVITY_LOG_ID))
                        {
                            query.Append(joinOperartor + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.FROM_TIME_STAMP))
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(KioskActivityLogDTO.SearchByParameters.TO_TIME_STAMP))
                        {
                            query.Append(joinOperartor + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) =< " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else
                        {
                            query.Append(joinOperartor + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectKioskActivityLogsQuery = selectKioskActivityLogsQuery + query;
            }
            DataTable dtKioskActivityLogsDTO = dataAccessHandler.executeSelectQuery(selectKioskActivityLogsQuery, parameters.ToArray(), sqlTransaction);
            if (dtKioskActivityLogsDTO.Rows.Count > 0)
            {
                foreach (DataRow paymentRow in dtKioskActivityLogsDTO.Rows)
                {
                    KioskActivityLogDTO kioskActivityLogDTO = GetKioskActivityLogDTO(paymentRow);
                    kioskActivityLogDTOList.Add(kioskActivityLogDTO);
                }
            }
            log.LogMethodExit();
            return kioskActivityLogDTOList;
        }


        internal int GetMaxKioskTrxId(int siteId)
        {
            log.LogMethodEntry(siteId);
            int maxKioskTrxId = 0;

            try
            {
                string query = @" Select isnull(max(KioskTrxId), 0) as KioskActivityLogId
                                    from KioskActivityLog ";
                string whereClause = "  Where site_id = @siteId";
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (siteId > -1)
                {
                    parameters.Add(new SqlParameter("@site_id", siteId));
                    query = query + whereClause;
                }
                DataTable dt = dataAccessHandler.executeSelectQuery(query, parameters.ToArray(), sqlTransaction);
                if (dt.Rows.Count > 0)
                {
                    DataRow dataRow = dt.Rows[0];
                    maxKioskTrxId = Convert.ToInt32(dt.Rows[0]["KioskActivityLogId"]);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while fetching maxKioskTrxId", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(maxKioskTrxId);
            return maxKioskTrxId;
        }
    }
}
