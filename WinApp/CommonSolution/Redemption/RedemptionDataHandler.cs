/********************************************************************************************
 * Project Name - Redemption Data Handler
 * Description  - Data handler of the Redemption class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        12-May-2017   Lakshminarayana     Created 
 *2.3.0       03-Jul-2018   Guru S A            Date param changes to insert and update. Updated 
 *                                              get dto queries to include required fields
 *2.4.0       3-Sep-2018    Archana/Guru S A    Modified for pos redemption reversal and RDS changes 
 *2.70.2        19-Jul-2019   Deeksha             Modifications as per three tier standard.
 *2.70.2        10-Dec-2019   Jinto Thomas        Removed siteid from update query
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    ///  Redemption Data Handler - Handles insert, update and select of  Redemption objects
    /// </summary>
    public class RedemptionDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<RedemptionDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<RedemptionDTO.SearchByParameters, string>
            {
                {RedemptionDTO.SearchByParameters.REDEPTION_ID, "r.redemption_id"},
                {RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, "r.redeemed_date"},
                {RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, "r.redeemed_date"},
                {RedemptionDTO.SearchByParameters.MASTER_ENTITY_ID,"r.MasterEntityId"},
                {RedemptionDTO.SearchByParameters.SITE_ID, "r.site_id"},
                {RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO, "r.RedemptionOrderNo"},
                {RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO_LIKE, "r.RedemptionOrderNo"},
                {RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, "r.RedemptionStatus"},
                {RedemptionDTO.SearchByParameters.REDEMPTION_STATUS_NOT_IN, "r.RedemptionStatus"},
                {RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE, ""},
                {RedemptionDTO.SearchByParameters.CARD_NUMBER, ""},
                {RedemptionDTO.SearchByParameters.PRIMARY_CARD, "r.primary_card_number"},
                {RedemptionDTO.SearchByParameters.CUSTOMER_NAME, "(ISNULL(pf.FirstName,'') + ' ' + ISNULL(pf.lastName,''))"} ,
                {RedemptionDTO.SearchByParameters.FETCH_GIFT_REDEMPTIONS_ONLY, ""} ,
                {RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, ""},
                {RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_DELIVERED_DATE, "r.OrderDeliveredDate"},
                {RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_DELIVERED_DATE, "r.OrderDeliveredDate"},
                {RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_COMPLETED_DATE, "r.OrderCompletedDate"},
                {RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_COMPLETED_DATE, "r.OrderCompletedDate"},
                {RedemptionDTO.SearchByParameters.POS_MACHINE_ID, "r.POSMachineId"},
                {RedemptionDTO.SearchByParameters.CUSTOMER_ID, "r.CustomerId"}
            };

        private static readonly string RedemptionSelectQry = @"SELECT r.*, ( isnull(pf.FirstName,'' ) + ' ' +  isnull(pf.lastName,'' )) as CustomerName, pos.POSName as PosMachineName
                                                                     ,orginalR.redemptionOrderNo OriginalRedemptionOrderNo
                                                                FROM Redemption r left outer join cards c on r.card_id = c.card_id 
                                                                     left outer join customers cu on cu.customer_id = isnull(r.customerId,c.customer_id )                                 
                                                                     left outer join Profile pf on pf.Id = cu.ProfileId
                                                                     left outer join PosMachines pos on pos.POSMachineId = r.POSMachineId 
                                                                     left outer join redemption orginalR on orginalR.redemption_Id = r.origRedemptionId "; 

        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of RedemptionDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public RedemptionDataHandler(SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        private List<SqlParameter> PassParametersHelper( RedemptionDTO redemptionDTO, string userId, int siteId)
        {
            log.LogMethodEntry( redemptionDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@redemption_id", redemptionDTO.RedemptionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@primary_card_number", redemptionDTO.PrimaryCardNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@manual_tickets", redemptionDTO.ManualTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@eTickets", redemptionDTO.ETickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@redeemed_date", redemptionDTO.RedeemedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@card_id", redemptionDTO.CardId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@OrigRedemptionId", redemptionDTO.OrigRedemptionId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", redemptionDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@GraceTickets", redemptionDTO.GraceTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ReceiptTickets", redemptionDTO.ReceiptTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CurrencyTickets", redemptionDTO.CurrencyTickets));
            parameters.Add(dataAccessHandler.GetSQLParameter("@MasterEntityId", redemptionDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionOrderNo", redemptionDTO.RedemptionOrderNo));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Source", redemptionDTO.Source));
			if (redemptionDTO.OrderCompletedDate == DateTime.MinValue || redemptionDTO.OrderCompletedDate == null)
			{
                parameters.Add(dataAccessHandler.GetSQLParameter("@OrderCompletedDate", DBNull.Value));
			}
			else
			{
                parameters.Add(dataAccessHandler.GetSQLParameter("@OrderCompletedDate", redemptionDTO.OrderCompletedDate));
			}

			if (redemptionDTO.OrderDeliveredDate == null || redemptionDTO.OrderDeliveredDate == DateTime.MinValue)
			{
                parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDeliveredDate", DBNull.Value));
			}
			else
			{
                parameters.Add(dataAccessHandler.GetSQLParameter("@OrderDeliveredDate", redemptionDTO.OrderDeliveredDate));
			}


            parameters.Add(dataAccessHandler.GetSQLParameter("@RedemptionStatus", redemptionDTO.RedemptionStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CreatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@POSMachineId", redemptionDTO.POSMachineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@CustomerId", redemptionDTO.CustomerId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the Redemption record to the database
        /// </summary>
        /// <param name="redemptionDTO">RedemptionDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns inserted record id</returns>
        public RedemptionDTO InsertRedemption(RedemptionDTO redemptionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionDTO, loginId, siteId);
            string query = @"INSERT INTO[dbo].[Redemption]
                                        ( 
                                            primary_card_number,
                                            manual_tickets,
                                            eTickets,
                                            redeemed_date,
                                            LastUpdatedBy,
                                            card_id,
                                            OrigRedemptionId,
                                            Remarks,
                                            site_id,
                                            Guid,
                                            GraceTickets,
                                            ReceiptTickets,
                                            CurrencyTickets,
                                            MasterEntityId,
                                            Source,
                                            RedemptionOrderNo,
                                            LastUpdateDate,
                                            OrderCompletedDate,
                                            OrderDeliveredDate,
                                            RedemptionStatus,
                                            CreationDate,
                                            CreatedBy,
                                            POSMachineId,
                                            CustomerId
                                        ) 
                                VALUES 
                                        (
                                            @primary_card_number,
                                            @manual_tickets,
                                            @eTickets,
                                            @redeemed_date,
                                            @LastUpdatedBy,
                                            @card_id,
                                            @OrigRedemptionId,
                                            @Remarks,
                                            @site_id,
                                            NEWID(),
                                            @GraceTickets,
                                            @ReceiptTickets,
                                            @CurrencyTickets,
                                            @MasterEntityId,
                                            @Source,
                                            @RedemptionOrderNo,
                                            GETDATE(),
                                            @OrderCompletedDate,
                                            @OrderDeliveredDate,
                                            @RedemptionStatus,
                                            GETDATE(),
                                            @CreatedBy,
                                            @POSMachineId,
                                            @CustomerId
                                        ) SELECT* FROM Redemption WHERE redemption_id = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionDTO(redemptionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting redemptionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);

                throw;
            }
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        }

        /// <summary>
        /// Updates the Redemption record
        /// </summary>
        /// <param name="redemptionDTO">RedemptionDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx"></param>
        /// <returns>Returns the count of updated rows</returns>
        public RedemptionDTO UpdateRedemption(RedemptionDTO redemptionDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(redemptionDTO, loginId, siteId);
            string query = @"UPDATE  [dbo].[Redemption]
                               SET 
                                 primary_card_number=@primary_card_number,
                                 manual_tickets=@manual_tickets,
                                 eTickets=@eTickets,
                                 redeemed_date=@redeemed_date,
                                 LastUpdatedBy=@LastUpdatedBy,  
                                 card_id=@card_id,
                                 OrigRedemptionId=@OrigRedemptionId,   
                                 Remarks=@Remarks,
                                 -- site_id=@site_id,
                                 GraceTickets = @GraceTickets,
                                 ReceiptTickets=@ReceiptTickets,
                                 CurrencyTickets=@CurrencyTickets,
                                 Source=@Source,
                                 RedemptionOrderNo=@RedemptionOrderNo,
                                 LastUpdateDate=GETDATE(),
                                 OrderCompletedDate=@OrderCompletedDate,
                                 OrderDeliveredDate=@OrderDeliveredDate,
                                 RedemptionStatus=@RedemptionStatus,
                                 --POSMachineId=@POSMachineId,
                                 CustomerId=@CustomerId
                             WHERE redemption_id = @redemption_id
                            SELECT * FROM Redemption WHERE redemption_id = @redemption_id";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(query, PassParametersHelper(redemptionDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshRedemptionDTO(redemptionDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating redemptionDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        }


        /// <summary>
        /// Converts the Data row object to RedemptionDTO class type
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <returns>Returns RedemptionDTO</returns>
        private RedemptionDTO GetRedemptionDTO(DataRow dataRow)
        {
            log.LogMethodEntry(dataRow);
            RedemptionDTO redemptionDTO = new RedemptionDTO(Convert.ToInt32(dataRow["redemption_id"]),
                                            dataRow["primary_card_number"] == DBNull.Value ? string.Empty : dataRow["primary_card_number"].ToString(),
                                            dataRow["manual_tickets"] == DBNull.Value ? (int?) null : Convert.ToInt32(dataRow["manual_tickets"]),
                                            dataRow["eTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["eTickets"]),
                                            dataRow["redeemed_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["redeemed_date"]),
                                            dataRow["card_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["card_id"]),
                                            dataRow["OrigRedemptionId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrigRedemptionId"]),
                                            dataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Remarks"]),
                                            dataRow["GraceTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["GraceTickets"]),
                                            dataRow["ReceiptTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ReceiptTickets"]),
                                            dataRow["CurrencyTickets"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["CurrencyTickets"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["Source"] == DBNull.Value ? string.Empty : dataRow["Source"].ToString(),
                                            dataRow["RedemptionOrderNo"] == DBNull.Value ? string.Empty : dataRow["RedemptionOrderNo"].ToString(),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["OrderCompletedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["OrderCompletedDate"]),
                                            dataRow["OrderDeliveredDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["OrderDeliveredDate"]),
                                            dataRow["RedemptionStatus"] == DBNull.Value ? string.Empty : dataRow["RedemptionStatus"].ToString(),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString(),
                                            new List<RedemptionGiftsDTO>(),
                                            new List<RedemptionCardsDTO>(),
                                            new List<TicketReceiptDTO>(),
                                            new List<RedemptionTicketAllocationDTO>(),
                                            dataRow["CustomerName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CustomerName"]),
                                            dataRow["POSMachineId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["POSMachineId"]),
                                            dataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CustomerId"]),
                                            dataRow["PosMachineName"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["PosMachineName"]),
                                            dataRow["OriginalRedemptionOrderNo"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["OriginalRedemptionOrderNo"])
                                            );
            log.LogMethodExit(redemptionDTO);
            return redemptionDTO;
        }

        /// <summary>
        /// Gets the Redemption data of passed Redemption Id
        /// </summary>
        /// <param name="redemptionId">integer type parameter</param>
        /// <returns>Returns RedemptionDTO</returns>
        public RedemptionDTO GetRedemptionDTO(int redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            RedemptionDTO returnValue = null;
            string query = RedemptionSelectQry +  " WHERE r.redemption_id = @redemption_id ";
            SqlParameter parameter = new SqlParameter("@redemption_id", redemptionId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter },sqlTransaction);
            if(dataTable.Rows.Count > 0)
                returnValue = GetRedemptionDTO(dataTable.Rows[0]);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Delete the record from the redemption_id database based on POSMachineId
        /// </summary>
        /// <param name="redemptionId">integer type parameter</param>
        /// <returns>return the int </returns>
        internal int Delete(int redemption_id)
        {
            log.LogMethodEntry(redemption_id);
            string query = @"DELETE  
                             FROM Redemption
                             WHERE Redemption.redemption_id = @redemption_id";
            SqlParameter parameter = new SqlParameter("@redemption_id", redemption_id);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="redemptionDTO">redemptionDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshRedemptionDTO(RedemptionDTO redemptionDTO, DataTable dt)
        {
            log.LogMethodEntry(redemptionDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                redemptionDTO.RedemptionId = Convert.ToInt32(dt.Rows[0]["redemption_id"]);
                redemptionDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                redemptionDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                redemptionDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                redemptionDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                redemptionDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                redemptionDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
             }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Next Sequence No
        /// </summary>
        /// <param name="POSMachineId">integer type parameter</param>
        /// <param name="SQLTrx">SqlTransaction type parameter</param>
        /// <returns>Returns RedemptionDTO</returns>
        public string GetNextRedemptionOrderNo(int POSMachineId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(POSMachineId, SQLTrx);

            //SqlCommand cmd = u.getCommand(SQLTrx);
            string returnValue = null;

            string query = @"declare @value varchar(20)
                                exec GetNextSeqValue N'RedemptionOrder', @value out, " + POSMachineId.ToString() + @"
                                select @value";
            try
            {
                DataTable dataTable = dataAccessHandler.executeSelectQuery(query,null, SQLTrx);
                if (dataTable.Rows.Count > 0)
                {
                    returnValue = dataTable.Rows[0][0].ToString();
                    log.LogMethodExit(returnValue);
                }
                else
                {
                    log.LogMethodExit("-1");
                    return "-1";
                }
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            catch (Exception ex)
            {
                log.Error("Unable to execute query on RedemptionOrder! ", ex);
                log.LogMethodExit("-1");
                return "-1";
            }
        }


        /// <summary>
        /// Gets the MediaDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of RedemptionDTO matching the search criteria</returns>
        public List<RedemptionDTO> GetRedemptionDTOList(List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<RedemptionDTO> list = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            int count = 0;
            bool doCompleteLoad = false;
            string selectQuery = RedemptionSelectQry;
           
            if((searchParameters != null) && (searchParameters.Count > 0))
            {   

                string joiner ;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<RedemptionDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if(DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? string.Empty : " and ";
                        if(searchParameter.Key == RedemptionDTO.SearchByParameters.REDEPTION_ID ||
                            searchParameter.Key == RedemptionDTO.SearchByParameters.POS_MACHINE_ID ||
                            searchParameter.Key == RedemptionDTO.SearchByParameters.CUSTOMER_ID ||
                            searchParameter.Key == RedemptionDTO.SearchByParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if(searchParameter.Key == RedemptionDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE
                                || searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_DELIVERED_DATE
                                || searchParameter.Key == RedemptionDTO.SearchByParameters.FROM_REDEMPTION_ORDER_COMPLETED_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if(searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE
                                || searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_DELIVERED_DATE
                                || searchParameter.Key == RedemptionDTO.SearchByParameters.TO_REDEMPTION_ORDER_COMPLETED_DATE)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + dataAccessHandler.GetParameterName(searchParameter.Key) + "");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO_LIKE||
                                 searchParameter.Key == RedemptionDTO.SearchByParameters.PRIMARY_CARD||
                                 searchParameter.Key == RedemptionDTO.SearchByParameters.CUSTOMER_NAME)
                        {

                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));

                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.CARD_NUMBER)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                                 from redemption_cards rcards 
                                                                where rcards.redemption_id = r.redemption_id
                                                                   and rcards.card_number  like N'%" + searchParameter.Value + "%' )");
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.REDEMPTION_STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.REDEMPTION_STATUS_NOT_IN)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " NOT IN (" + dataAccessHandler.GetInClauseParameterName(searchParameter.Key, searchParameter.Value) + ")");
                            parameters.AddRange(dataAccessHandler.GetSqlParametersForInClause(searchParameter.Key, searchParameter.Value));
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE)
                        {
                            query.Append(joiner + @" EXISTS (SELECT 1 
                                                                 from redemption_gifts g ,
                                                                      product p left outer join productBarcode pb on p.productId = pb.productId and pb.isactive = 'Y'
                                                                where g.redemption_id = r.redemption_id
                                                                   and p.productId = g.productId
                                                                   and (p.Code like N'%" + searchParameter.Value + "%' or p.description like N'%" + searchParameter.Value + "%' or pb.BarCode like N'%" + searchParameter.Value + "%' ) )");
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.FETCH_GIFT_REDEMPTIONS_ONLY)
                        {
                            if (searchParameter.Value == "Y")
                            {
                                query.Append(joiner + @" EXISTS (SELECT 1 
                                                                 from redemption_gifts g  
                                                                where g.redemption_id = r.redemption_id ) ");
                            }
                        }
                        else if (searchParameter.Key == RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS)
                        {
                            if (searchParameter.Value == "Y")
                            { doCompleteLoad = true; }
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
                }
                selectQuery = selectQuery + query;
            }
            log.Info("Search query: " + selectQuery);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                list = new List<RedemptionDTO>();
                ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
                string siteId = executionContext.GetSiteId().ToString();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    RedemptionDTO redemptionDTO = GetRedemptionDTO(dataRow);
                    if (doCompleteLoad)
                    {
                        RedemptionGiftsListBL redemptionGiftListBl = new RedemptionGiftsListBL(executionContext);
                        List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> searchParamRg = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>();
                        searchParamRg.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
                        searchParamRg.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, siteId));
                        List<RedemptionGiftsDTO> redemptionGiftsDTOList = redemptionGiftListBl.GetRedemptionGiftsDTOList(searchParamRg);
                        if(redemptionGiftsDTOList != null && redemptionGiftsDTOList.Count > 0)
                        {
                            redemptionDTO.RedemptionGiftsListDTO = redemptionGiftsDTOList;
                        }
                        RedemptionCardsListBL redemptionCardsListBl = new RedemptionCardsListBL(executionContext);
                        List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>> searchParamRedemptionCards = new List<KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>>();
                        searchParamRedemptionCards.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
                        searchParamRedemptionCards.Add(new KeyValuePair<RedemptionCardsDTO.SearchByParameters, string>(RedemptionCardsDTO.SearchByParameters.SITE_ID, siteId));
                        List<RedemptionCardsDTO> redemptionCardsDTOLst = redemptionCardsListBl.GetRedemptionCardsDTOList(searchParamRedemptionCards);
                        if (redemptionCardsDTOLst != null && redemptionCardsDTOLst.Count > 0)
                        {
                            redemptionDTO.RedemptionCardsListDTO = redemptionCardsDTOLst;
                        }
                        RedemptionTicketAllocationListBL redemptionTicketAllocationListBl = new RedemptionTicketAllocationListBL(executionContext);
                        List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>> searchParamRta = new List<KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>>();
                        searchParamRta.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
                        searchParamRta.Add(new KeyValuePair<RedemptionTicketAllocationDTO.SearchByParameters, string>(RedemptionTicketAllocationDTO.SearchByParameters.SITE_ID, siteId));
                        List<RedemptionTicketAllocationDTO> redemptionTicketAllocationDTOList = redemptionTicketAllocationListBl.GetRedemptionTicketAllocationDTOList(searchParamRta);
                        if (redemptionTicketAllocationDTOList != null && redemptionTicketAllocationDTOList.Count > 0)
                        {
                            redemptionDTO.RedemptionTicketAllocationListDTO = redemptionTicketAllocationDTOList;
                        }
                        TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                        List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParamtkt = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                        searchParamtkt.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID, redemptionDTO.RedemptionId.ToString()));
                        searchParamtkt.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, siteId));
                        List<TicketReceiptDTO> ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParamtkt);
                        if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                        {
                            redemptionDTO.TicketReceiptListDTO = ticketReceiptDTOList;
                        }
                    }
                    list.Add(redemptionDTO); 
                }
            }
            log.LogMethodExit(list);
            return list;
        }
    }
}
