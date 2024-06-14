/********************************************************************************************
 * Project Name - TicketReceipt
 * Description  - Ticket Station DataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date             	Modified By        Remarks          
 *********************************************************************************************
 *2.3.0       11-July-2018      Archana            Created 
 *2.7.0       08-Jul-2019       Archana            Modified: Redemption Receipt changes to show ticket allocation details
 *2.70.2        19-Jul-2019       Deeksha            Modifications as per three tier standard.
 *2.70.2        10-Dec-2019       Jinto Thomas       Removed siteid from update query
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Ticket Receipt Data Handler  - Handles insert, update and select of ticket receipt objects
    /// </summary>
    public class TicketReceiptDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private DataAccessHandler dataAccessHandler;
        private const string SELECT_QUERY = @"SELECT * FROM ManualTicketReceipts AS mtr ";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///  Dictionary for searching Parameters for the TicketReceipt object.
        /// </summary>
        private static readonly Dictionary<TicketReceiptDTO.SearchByTicketReceiptParameters, string> DBSearchParameters = new Dictionary<TicketReceiptDTO.SearchByTicketReceiptParameters, string>
            {
                {TicketReceiptDTO.SearchByTicketReceiptParameters.ID, "mtr.Id"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID, "mtr.Redemption_id"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED, "mtr.isSuspected"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS, "mtr.BalanceTickets"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_FROM, "mtr.BalanceTickets"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_TO, "mtr.BalanceTickets"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_FROM_TIME, "mtr.LastupdatedDate"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_TO_TIME, "mtr.LastupdatedDate"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, "mtr.ManualTicketReceiptNo"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.MASTER_ENTITY_ID,"mtr.MasterEntityId"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE,"mtr.IssueDate"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE,"mtr.IssueDate"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, "mtr.site_id"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_RECEIPT_IDS, "mtr.Id"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, "mtr.SourceRedemptionId"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO_LIKE, "mtr.ManualTicketReceiptNo"},
                {TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID_LIST, "mtr.SourceRedemptionId"}
            };

        /// <summary>
        /// Default constructor of TicketReceiptDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public TicketReceiptDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating TicketReceiptDataHandler Record.
        /// </summary>
        /// <param name="ticketReceiptDTO">TicketReceiptDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(TicketReceiptDTO ticketReceiptDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketReceiptDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@id", ticketReceiptDTO.Id, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemptionid", ticketReceiptDTO.RedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sourceRedemptionid", ticketReceiptDTO.SOurceRedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tickets", ticketReceiptDTO.Tickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@balanceTickets", ticketReceiptDTO.BalanceTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueDate", ticketReceiptDTO.IssueDate == DateTime.MinValue ? DBNull.Value :(object) ticketReceiptDTO.IssueDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isSuspected", ticketReceiptDTO.IsSuspected));
            parameters.Add(dataAccessHandler.GetSQLParameter("@manualTicketReceiptNo", string.IsNullOrEmpty(ticketReceiptDTO.ManualTicketReceiptNo) ? DBNull.Value : (object)ticketReceiptDTO.ManualTicketReceiptNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", ticketReceiptDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@reprintCount", ticketReceiptDTO.ReprintCount));
            log.LogMethodExit(parameters);
            return parameters;
        }

      
        
        /// <summary>
        /// Inserts the ticket receipt record to the database
        /// </summary>
        /// <param name="ticketReceiptDTO">TicketReceiptDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">SQL Transactions </param>
        /// <returns>Returns inserted record id</returns>
        public TicketReceiptDTO InsertTicketReceipt(TicketReceiptDTO ticketReceiptDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(ticketReceiptDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[ManualTicketReceipts]
                                                        (                                                         
                                                         Redemption_id
                                                        ,ManualTicketReceiptNo
                                                        ,Tickets
                                                        ,BalanceTickets
                                                        ,isSuspected
                                                        ,LastUpdatedBy
                                                        ,LastupdatedDate
                                                        ,Guid
                                                        ,site_id
                                                        ,MasterEntityId
                                                        ,SourceRedemptionId
                                                        ,IssueDate
                                                        ,CreatedBy
                                                        ,CreationDate
                                                        ,reprintCount
                                                        ) 
                                                values 
                                                        (                                                         
                                                         @redemptionid
                                                        ,@manualTicketReceiptNo
                                                        ,@tickets
                                                        ,@balanceTickets
                                                        ,@isSuspected
                                                        ,@lastUpdatedBy
                                                        ,getdate()
                                                        ,NewId()
                                                        ,@siteId
                                                        ,@masterEntityId
                                                        ,@sourceRedemptionid
                                                        ,@issueDate 
                                                        ,@createdBy
                                                        ,GETDATE()
                                                        ,@reprintCount)
                                            SELECT* FROM ManualTicketReceipts WHERE Id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketReceiptDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketReceiptDTO(ticketReceiptDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting ticketReceiptDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(ticketReceiptDTO);
            return ticketReceiptDTO;
        }

        /// <summary>
        /// Updates the Ticket Receipt record
        /// </summary>
        /// <param name="ticketReceiptDTO">TicketReceiptDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTrxn">SQL Transactions </param>
        /// <returns>Returns the count of updated rows</returns>
        public TicketReceiptDTO UpdateTicketReceipt(TicketReceiptDTO ticketReceiptDTO, string loginId, int siteId)
        {

            log.LogMethodEntry(ticketReceiptDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[ManualTicketReceipts]
                                    SET      Redemption_id = @redemptionid
                                             ,ManualTicketReceiptNo = @manualTicketReceiptNo
                                             ,Tickets = @tickets
                                             ,BalanceTickets = @balanceTickets
                                             ,isSuspected = @isSuspected,
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate(),
                                             -- site_id=@siteid,
                                             MasterEntityId=@masterEntityId,  
                                             --SourceRedemptionId = @sourceRedemptionId,
                                             --IssueDate = @issueDate,
                                             ReprintCount = @reprintCount
                                             where Id = @id
                                SELECT * FROM ManualTicketReceipts WHERE Id = @id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, GetSQLParameters(ticketReceiptDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshTicketReceiptDTO(ticketReceiptDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating ticketReceiptDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(ticketReceiptDTO);
            return ticketReceiptDTO;
        }

        /// <summary>
        /// Delete the record from the ManualTicketReceipts database based on ManualTicketReceiptsID
        /// </summary>
        /// <param name="id">dt is an object of DataTable </param>
        /// <returns>return the int </returns>
        internal int Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM ManualTicketReceipts
                             WHERE ManualTicketReceipts.Id = @id";
            SqlParameter parameter = new SqlParameter("@id", id);
            int id1 = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id1);
            return id1;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="ticketReceiptDTO">TicketReceiptDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshTicketReceiptDTO(TicketReceiptDTO ticketReceiptDTO, DataTable dt)
        {
            log.LogMethodEntry(ticketReceiptDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                ticketReceiptDTO.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                ticketReceiptDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                ticketReceiptDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                ticketReceiptDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                ticketReceiptDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                ticketReceiptDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                ticketReceiptDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to TicketReceiptDTO class type
        /// </summary>
        /// <param name="ticketReceiptDataRow">TicketReceipt DataRow</param>
        /// <returns>Returns TicketReceipt</returns>
        private TicketReceiptDTO GetTicketReceiptDTO(DataRow ticketReceiptDataRow)
        {
            log.LogMethodEntry(ticketReceiptDataRow);
            TicketReceiptDTO ticketReceiptDataObject = new TicketReceiptDTO(Convert.ToInt32(ticketReceiptDataRow["Id"]),
                                            ticketReceiptDataRow["Redemption_id"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["Redemption_id"]),
                                            ticketReceiptDataRow["ManualTicketReceiptNo"] == DBNull.Value ? string.Empty : Convert.ToString(ticketReceiptDataRow["ManualTicketReceiptNo"]),
                                            ticketReceiptDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["site_id"]),
                                            ticketReceiptDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(ticketReceiptDataRow["Guid"]),
                                            ticketReceiptDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(ticketReceiptDataRow["SynchStatus"]),
                                            ticketReceiptDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["MasterEntityId"]),
                                            ticketReceiptDataRow["Tickets"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["Tickets"]),
                                            ticketReceiptDataRow["balanceTickets"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["balanceTickets"]),
                                            ticketReceiptDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(ticketReceiptDataRow["LastUpdatedBy"]),
                                            ticketReceiptDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ticketReceiptDataRow["LastupdatedDate"]),
                                            ticketReceiptDataRow["isSuspected"] == DBNull.Value ? false : Convert.ToBoolean(ticketReceiptDataRow["isSuspected"]),
                                            ticketReceiptDataRow["SourceRedemptionid"] == DBNull.Value ? -1 : Convert.ToInt32(ticketReceiptDataRow["SourceRedemptionid"]),
                                            ticketReceiptDataRow["IssueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ticketReceiptDataRow["IssueDate"]),
                                            ticketReceiptDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(ticketReceiptDataRow["CreatedBy"]),
                                            ticketReceiptDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(ticketReceiptDataRow["CreationDate"]),
                                            ticketReceiptDataRow["ReprintCount"] == DBNull.Value ? 0 : Convert.ToInt32(ticketReceiptDataRow["ReprintCount"])




                                            );
            log.LogMethodExit(ticketReceiptDataObject);
            return ticketReceiptDataObject;
        }
        /// <summary>
        /// Gets the Ticket Receipt data of passed receipt no
        /// </summary>
        /// <param name="receiptNo">integer type parameter</param>
        /// <param name="sqltrxn">SqlTransaction object</param>
        /// <returns>Returns TicketReceiptDTO</returns>
        public TicketReceiptDTO GetTicketReceipt(string receiptNo, SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(receiptNo, sqltrxn);
            string selectTicketReceiptQuery = @"SELECT *
                                              FROM ManualTicketReceipts
                                             WHERE ManualTicketReceiptNo = @receiptNo";
            SqlParameter[] selectTicketReceiptParameters = new SqlParameter[1];
            selectTicketReceiptParameters[0] = new SqlParameter("@receiptNo", receiptNo);
            DataTable ticketReceipt = dataAccessHandler.executeSelectQuery(selectTicketReceiptQuery, selectTicketReceiptParameters, sqltrxn);
            if (ticketReceipt.Rows.Count > 0)
            {
                DataRow TicketReceiptRow = ticketReceipt.Rows[0];
                TicketReceiptDTO ticketReceiptDataObject = GetTicketReceiptDTO(TicketReceiptRow);
                log.LogMethodExit(ticketReceiptDataObject);
                return ticketReceiptDataObject;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

        /// <summary>
        /// Gets the ticket receipt data of passed ticket receipt id
        /// </summary>
        /// <param name="ticketReceiptId">integer type parameter</param>
        /// <param name="sqltrxn">SqlTransaction object</param>
        /// <returns>Returns TicketReceiptDTO</returns>
        public TicketReceiptDTO GetTicketReceipt(int ticketReceiptId, SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(ticketReceiptId, sqltrxn);
            TicketReceiptDTO result = null;
            string query = SELECT_QUERY + @" WHERE mtr.Id= @ticketReceiptId";
            SqlParameter parameter = new SqlParameter("@ticketReceiptId", ticketReceiptId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetTicketReceiptDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the TicketReceiptDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <param name="sqlTrxn">SqlTransaction object</param>
        /// <returns>Returns the list of TicketReceiptDTO matching the search criteria</returns>
        public List<TicketReceiptDTO> GetTicketReceiptList(List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<TicketReceiptDTO> ticketReceiptList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectTicketReceiptQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.ID
                            || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.MASTER_ENTITY_ID                           
                            || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID
                            || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS
                            || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if(searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_FROM)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_TO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_FROM_TIME
                                ||searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_TO_TIME
                            || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key),searchParameter.Value));
                         }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_RECEIPT_IDS
                              || searchParameter.Key == TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID_LIST)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " IN(" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ") ");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause<int>(searchParameter.Key, searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
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
                    count++;
                }
                if (searchParameters.Count > 0)
                    selectTicketReceiptQuery = selectTicketReceiptQuery + query;
            }
            DataTable ticketReceiptData = dataAccessHandler.executeSelectQuery(selectTicketReceiptQuery, parameters.ToArray(), sqlTransaction);
            if (ticketReceiptData.Rows.Count > 0)
            {
                ticketReceiptList = new List<TicketReceiptDTO>();
                foreach (DataRow ticketReceiptDataRow in ticketReceiptData.Rows)
                {
                    TicketReceiptDTO ticketReceiptDataObject = GetTicketReceiptDTO(ticketReceiptDataRow);
                    ticketReceiptList.Add(ticketReceiptDataObject);
                }
            }
            log.LogMethodExit(ticketReceiptList);
            return ticketReceiptList;
        }
    }
}
