/********************************************************************************************
 * Project Name - POS
 * Description  - Data Handler of ShiftLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        27-May-2019   Divya A                 Created 
 *2.70.2      10-Dec-2019   Jinto Thomas            Removed siteid from update query
 *2.90        26-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 *2.140.0     16-Aug-2021   Deeksha                 Modified : Provisional Shift changes
 *2.140.0     15-Feb-2022   Girish Kundar           Modified : added UpdateCashdrawerApproverDetails() to update the approverId and time
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.User
{
    class ShiftLogDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM ShiftLog as shiftlog ";


        private static readonly Dictionary<ShiftLogDTO.SearchByShiftParameters, string> DBSearchParameters = new Dictionary<ShiftLogDTO.SearchByShiftParameters, string>
        {
            {ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY, "shiftlog.shift_key"},
              {ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY_LIST, "shiftlog.shift_key"},
            {ShiftLogDTO.SearchByShiftParameters.SHIFT_ACTION, "shiftlog.shift_action"},
            {ShiftLogDTO.SearchByShiftParameters.SHIFT_LOG_ID, "shiftlog.shiftLog_Id"},
            {ShiftLogDTO.SearchByShiftParameters.SHIFT_FROM_TIME, "shiftlog.shift_time"},
            {ShiftLogDTO.SearchByShiftParameters.SHIFT_TO_TIME, "shiftlog.shift_time"},
            {ShiftLogDTO.SearchByShiftParameters.SITE_ID, "shiftlog.site_id"},
            {ShiftLogDTO.SearchByShiftParameters.MASTER_ENTITY_ID, "shiftlog.MasterEntityId"}
        };

        /// <summary>
        /// Default constructor of ShiftLogDataHandler class
        /// </summary>
        public ShiftLogDataHandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ShiftLog Record.
        /// </summary>
        /// <param name="shiftLogDTO">shiftLogDTO type object</param>
        /// <param name="loginId">userID to which the record belongs</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(ShiftLogDTO shiftLogDTO,string loginId, int siteId)
        {
            log.LogMethodEntry(shiftLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@shiftLog_Id", shiftLogDTO.ShiftLogId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_key", shiftLogDTO.ShiftKey, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_time", shiftLogDTO.ShiftTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_action", shiftLogDTO.ShiftAction));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_amount", shiftLogDTO.ShiftAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_count", shiftLogDTO.CardCount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_ticketnumber", shiftLogDTO.ShiftTicketNumber == null ?  DBNull.Value : (object)shiftLogDTO.ShiftTicketNumber.ToString()));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_remarks", shiftLogDTO.ShiftRemarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_amount", shiftLogDTO.ActualAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_cards", shiftLogDTO.ActualCards));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actual_tickets", shiftLogDTO.ActualTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GameCardamount", shiftLogDTO.GameCardAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreditCardamount", shiftLogDTO.CreditCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ChequeAmount", shiftLogDTO.ChequeAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CouponAmount", shiftLogDTO.CouponAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualGameCardamount", shiftLogDTO.ActualGameCardamount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualCreditCardamount", shiftLogDTO.ActualCreditCardAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualChequeAmount", shiftLogDTO.ActualChequeAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ActualCouponAmount", shiftLogDTO.ActualCouponAmount));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Guid", shiftLogDTO.Guid)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true)); 
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchStatus", shiftLogDTO.SynchStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", shiftLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreationDate", shiftLogDTO.CreationDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApproverID", shiftLogDTO.ApproverID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ApprovalTime", shiftLogDTO.ApprovalTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ShiftReason", shiftLogDTO.ShiftReason));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ExternalReference", shiftLogDTO.ExternalReference));
            parameters.Add(dataAccessHandler.GetSQLParameter("@shift_id", shiftLogDTO.ShiftId, true));
            log.LogMethodExit();
            return parameters;
        }


        /// <summary>
        /// Inserts the Shiftlog record to the database
        /// </summary>
        /// <param name="shiftDTO">ShiftDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns ShiftLogDTO</returns>
        public ShiftLogDTO Insert(ShiftLogDTO shiftLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftLogDTO, loginId, siteId);
            string query = @"insert into shiftlog
                                        (
                                        shift_key,
                                        shift_time,
                                        shift_action,
                                        shift_amount,
                                        card_count,
                                        shift_ticketnumber,
                                        shift_remarks,
                                        actual_amount,
                                        actual_cards,
                                        actual_tickets,
                                        GameCardamount,
                                        CreditCardamount,
                                        ChequeAmount,
                                        CouponAmount,
                                        ActualGameCardamount,
                                        ActualCreditCardamount,
                                        ActualChequeAmount,
                                        ActualCouponAmount,
                                        Guid,
                                        site_id,
                                        MasterEntityId,
                                        CreatedBy,
                                        CreationDate,
                                        LastUpdatedBy,
                                        LastUpdateDate,
                                        ApproverID,
                                        ApprovalTime,
                                        Reason,
                                        ExternalReference,
                                        ShiftId
                                        )
                                    values
                                        (
                                        @shift_key,
                                        GETDATE(),
                                        @shift_action,
                                        @shift_amount,
                                        @card_count,
                                        @shift_ticketnumber,
                                        @shift_remarks,
                                        @actual_amount,
                                        @actual_cards,
                                        @actual_tickets,
                                        @GameCardamount,
                                        @CreditCardamount,
                                        @ChequeAmount,
                                        @CouponAmount,
                                        @ActualGameCardamount,
                                        @ActualCreditCardamount,
                                        @ActualChequeAmount,
                                        @ActualCouponAmount,
                                        NEWID(),
                                        @site_id,
                                        @MasterEntityId,
                                        @CreatedBy,
                                        GETDATE(),
                                        @LastUpdatedBy,
                                        GETDATE(),
                                        @ApproverID,
                                        @ApprovalTime,
                                        @ShiftReason,                                        
                                        @ExternalReference,  
                                        @shift_id
                                        )
                          SELECT * FROM shiftlog WHERE shiftLog_Id = scope_identity()";

            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftLogDTO(shiftLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(shiftLogDTO);
            return shiftLogDTO;
        }

        /// <summary>
        /// Update the ShiftLogDTO record to the database
        /// </summary>
        /// <param name="shiftLogDTO">ShiftLogDTO type object</param>
        /// <param name="loginId"></param>
        /// <param name="siteId">data updated site id</param>
        /// <returns>Returns ShiftLogDTO</returns>
        public ShiftLogDTO Update(ShiftLogDTO shiftLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(shiftLogDTO, loginId, siteId);
            string query = @"UPDATE shiftlog SET 
                                    shift_key = @shift_key,
                                    shift_time = @shift_time,
                                    shift_action = @shift_action,
                                    shift_amount = @shift_amount,
                                    card_count = @card_count,
                                    shift_ticketnumber = @shift_ticketnumber,
                                    shift_remarks = @shift_remarks,
                                    actual_amount = @actual_amount,
                                    actual_cards = @actual_cards,
                                    actual_tickets = @actual_tickets,
                                    GameCardamount = @GameCardamount,
                                    CreditCardamount = @CreditCardamount,
                                    ChequeAmount = @ChequeAmount,
                                    CouponAmount = @CouponAmount,
                                    ActualGameCardamount = @ActualGameCardamount,
                                    ActualCreditCardamount = @ActualCreditCardamount,
                                    ActualChequeAmount = @ActualChequeAmount,
                                    ActualCouponAmount = @ActualCouponAmount,
                                    -- site_id = @site_id,
                                    MasterEntityId = @MasterEntityId,
                                    LastUpdatedBy = @LastUpdatedBy,
                                    LastUpdateDate = GETDATE(),
                                    ApproverID = @ApproverID,
                                    ApprovalTime = @ApprovalTime,
                                    Reason = @ShiftReason,
                                    ExternalReference = @ExternalReference,
                                    ShiftId = @shift_id
                                WHERE shiftLog_Id = @shiftLog_Id
                                SELECT * FROM ShiftLog WHERE shiftLog_Id = @shiftLog_Id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(shiftLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshShiftLogDTO(shiftLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "throwing exception");
                throw;
            }
            log.LogMethodExit(shiftLogDTO);
            return shiftLogDTO;
        }
        /// <summary>
        /// Converts the Data row object to ShiftLog class type
        /// </summary>
        /// <param name="dataRow">ShiftLog DataRow</param>
        /// <returns>Returns ShiftLogDTO</returns>
        private ShiftLogDTO GetShiftLogDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            ShiftLogDTO shiftLogDataObject = new ShiftLogDTO(dataRow["shiftLog_Id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["shiftLog_Id"]),
                                                    dataRow["shift_key"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["shift_key"]),
                                                    dataRow["shift_time"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["shift_time"]),
                                                    dataRow["shift_action"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["shift_action"]),
                                                    dataRow["shift_amount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["shift_amount"]),
                                                    dataRow["card_count"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["card_count"]),
                                                    dataRow["shift_ticketnumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["shift_ticketnumber"]),
                                                    dataRow["shift_remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["shift_remarks"]),
                                                    dataRow["actual_amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["actual_amount"]),
                                                    dataRow["actual_cards"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["actual_cards"]),
                                                    dataRow["actual_tickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["actual_tickets"]),
                                                    dataRow["GameCardamount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["GameCardamount"]),
                                                    dataRow["CreditCardamount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CreditCardamount"]),
                                                    dataRow["ChequeAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ChequeAmount"]),
                                                    dataRow["CouponAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["CouponAmount"]),
                                                    dataRow["ActualGameCardamount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualGameCardamount"]),
                                                    dataRow["ActualCreditCardamount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualCreditCardamount"]),
                                                    dataRow["ActualChequeAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualChequeAmount"]),
                                                    dataRow["ActualCouponAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(dataRow["ActualCouponAmount"]),
                                                    dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]),
                                                    dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                    dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                    dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                    dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                                    dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                                    dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                                    dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                                    dataRow["ApproverID"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ApproverID"]),
                                                    dataRow["ApprovalTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["ApprovalTime"]),
                                                    dataRow["Reason"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Reason"]),
                                                    dataRow["ExternalReference"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["ExternalReference"]),
                                                    dataRow["ShiftId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ShiftId"])
                                                    );
            log.LogMethodExit(shiftLogDataObject);
            return shiftLogDataObject;
        }


        /// <summary>
        /// Gets the ShiftLog data of passed shiftKey
        /// </summary>
        /// <param name="shiftLogId">integer type parameter</param>
        /// <returns>Returns ShiftLogDTO</returns>
        public ShiftLogDTO GetShiftLogDTO(int shiftLogId)
        {
            log.LogMethodEntry(shiftLogId);
            string selectQuery = @"select *
                                         from shiftLog
                                        where shiftLog_Id = @shiftLog_Id";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@shiftLog_Id", shiftLogId);
            DataTable shiftDataTable = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters);
            if (shiftDataTable.Rows.Count > 0)
            {
                DataRow shiftLogRow = shiftDataTable.Rows[0];
                ShiftLogDTO shiftDataObject = GetShiftLogDTO(shiftLogRow);
                log.LogMethodExit(shiftDataObject);
                return shiftDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }
        private void RefreshShiftLogDTO(ShiftLogDTO shiftLogDTO, DataTable dt)
        {
            log.LogMethodEntry(shiftLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                shiftLogDTO.ShiftLogId = Convert.ToInt32(dt.Rows[0]["shiftLog_Id"]);
                shiftLogDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                shiftLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                shiftLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                shiftLogDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                shiftLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                shiftLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the ShiftLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ShiftLogDTO matching the search criteria</returns>
        public List<ShiftLogDTO> GetShiftLogDTOList(List<KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string>> searchParameters, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<ShiftLogDTO> shiftLogDTOList = new List<ShiftLogDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ShiftLogDTO.SearchByShiftParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? "" : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.SHIFT_LOG_ID ||
                            searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY ||
                            searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.MASTER_ENTITY_ID )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.SHIFT_ACTION)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(ShiftLogDTO.SearchByShiftParameters.SHIFT_FROM_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(ShiftLogDTO.SearchByShiftParameters.SHIFT_TO_TIME))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ShiftLogDTO.SearchByShiftParameters.SHIFT_KEY_LIST)
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
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    ShiftLogDTO shiftLogDTO = GetShiftLogDTO(dataRow);
                    shiftLogDTOList.Add(shiftLogDTO);
                }
            }
            log.LogMethodExit(shiftLogDTOList);
            return shiftLogDTOList;
        }

        internal void UpdateCashdrawerApproverDetails(int shiftLogId , string approverLoginId)
        {
            log.LogMethodEntry(shiftLogId, approverLoginId);
            string selectQuery = @"UPDATE shiftLog SET ApproverID = @ApproverID , ApprovalTime = GetDate(), Reason ='Cashdrawer assignment/UnAssignment'
                                        where shiftLog_Id = @shiftLog_Id";
            SqlParameter[] selectParameters = new SqlParameter[2];
            selectParameters[0] = new SqlParameter("@shiftLog_Id", shiftLogId);
            selectParameters[1] = new SqlParameter("@ApproverID", approverLoginId);
            int success = dataAccessHandler.executeUpdateQuery(selectQuery, selectParameters,sqlTransaction);
            log.Debug("success" + success);
            log.LogMethodExit();
        }
    }
}
