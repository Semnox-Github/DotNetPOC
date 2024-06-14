/* Project Name - ReservationCoreDatahnadler Programs 
* Description  - Data handler for bookings
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith                Created 
*2.50.0      12-Dec-2018    Mathew Ninan            Deprecating StaticDataExchange. Using 
*                                                   new Transaction Payment object
*2.50.0      28-Jan-2019    Guru S A                Booking changes  
*2.70        14-Mar-2019    Guru S A                Booking phase 2 enhancement changes                  
*2.70.2        26-Sep-2019    Guru S A                Waiver phase 2 enhancement changes 
*2.70.2        10-Dec-2019   Jinto Thomas             Removed siteid from update query
*2.110       25-Nov-2020      Girish Kundar          Modified:  Paymemnt link enhancement
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Semnox.Parafait.User;
using System.Data;
using System.Data.SqlClient;
using Semnox.Parafait.Product;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.CardCore;
using System.Globalization;

namespace Semnox.Parafait.Transaction
{
    public class ReservationDatahandler
    {

        DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SqlTransaction sqlTransaction;

        string connstring;

        /// <summary>
        /// Default constructor of  TransactionCoreDatahandler class
        /// </summary>
        public ReservationDatahandler(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            connstring = dataAccessHandler.ConnectionString;
            log.LogMethodExit();
        }

        ////<summary>
        ////For search parameter Specified
        ////</summary>
        private static readonly Dictionary<ReservationDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<ReservationDTO.SearchByParameters, string>
        {
            {ReservationDTO.SearchByParameters.TRX_ID, "rr.TrxId"},
            {ReservationDTO.SearchByParameters.BOOKING_ID, "rr.BookingId"},
            {ReservationDTO.SearchByParameters.STATUS, "rr.Status"},
            {ReservationDTO.SearchByParameters.MASTER_ENTITY_ID, "rr.MasterEntityId"},
            {ReservationDTO.SearchByParameters.BOOKING_PRODUCT_ID, "rr.bookingProductId"},
            {ReservationDTO.SearchByParameters.CARD_NUMBER_LIKE, "cc.card_number"},
            {ReservationDTO.SearchByParameters.CUSTOMER_NAME_LIKE, ""},
            {ReservationDTO.SearchByParameters.FACILITY_MAP_ID, ""},
            {ReservationDTO.SearchByParameters.STATUS_LIST_IN, "rr.Status"},
            {ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, "rr.Status"},
            {ReservationDTO.SearchByParameters.RESERVATION_CODE_LIKE, "rr.ReservationCode"},
            {ReservationDTO.SearchByParameters.RESERVATION_CODE_EXACT, "rr.ReservationCode"},
            {ReservationDTO.SearchByParameters.TRANSACTION_GUID, "th.Guid"},
            {ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, ""},
            {ReservationDTO.SearchByParameters.RESERVATION_TO_DATE, ""},
            {ReservationDTO.SearchByParameters.SITE_ID, "rr.site_id"},
            {ReservationDTO.SearchByParameters.CHECKLIST_TASK_ASSIGNEE_ID, "uji.AssignedUserId"}
        };


        private string selectQry = @"SELECT rr.BookingId, rr.BookingName, rr.FromDate as FromDate ,  rr.ToDate as ToDate, rr.status, 
                                               case rr.recur_flag when 'Y' then 'Yes' else 'No' end as recur_flag,
                                               case rr.recur_frequency when 'D' then 'Daily' when 'W' then 'Weekly' else '' end as recur_frequency,
                                               rr.recur_end_date , rr.Remarks, rr.TrxId, rr.Quantity, rr.ReservationCode, cc.card_number as CardNumber,  
                                               rr.bookingClassId, cc.card_id as cardid,
											   rr.expiryTime, rr.channel, rr.createdBy, rr.creationDate, 
                                               rr.lastUpdatedBy, rr.LastUpdatedDate, rr.guid, rr.synchStatus, rr.site_Id,
											    rr.isEmailSent,  rr.age,
                                               ISNULL(rr.bookingProductId,((SELECT top 1 p.product_id 
                                                                              from products p, product_type pt, trx_lines tl
                                                                             where p.product_type_id=pt.product_type_id  
                                                                               and pt.product_type = 'BOOKINGS'
                                                                               and tl.product_id = p.product_id
                                                                               and tl.TrxId = rr.TrxId
                                                                             order by ISNULL(tl.CancelledTime, getdate()+100) desc, tl.TrxId, tl.LineId) ) ) as bookingProductId, 
                                                (Select top 1 trs.SchedulesId
                                                   from TrxReservationSchedule trs 
                                                  where trs.TrxId = rr.TrxId
                                               order by trs.Cancelled, trs.TrxId, trs.LineId )  as attractionScheduleId, 
                                               rr.extraGuests, rr.masterEntityId, 
											   th.trx_no, th.Status TrxStatus, th.TrxNetAmount, 
                                                stuff((Select ', '+fac.FacilityName
                                                   from FacilityMapDetails fmd,
                                                        CheckInFacility fac ,
														TrxReservationSchedule trs
                                                  where trs.TrxId  = rr.TrxId 
												    and trs.Cancelled = 0
												    and fmd.FacilityMapId = trs.FacilityMapId
                                                    and fmd.FacilityId = fac.FacilityId FOR XML PATH('')),1,1,'')  as FacilityName, 
                                                (Select top 1 trs.FacilityMapId
                                                   from TrxReservationSchedule trs 
                                                  where trs.TrxId = rr.TrxId 
                                               order by trs.Cancelled, trs.TrxId, trs.LineId ) as FacilityMapId, 
                                                (Select top 1 fm.FacilityMapName
                                                   from TrxReservationSchedule trs,
                                                        FacilityMap fm 
                                                  where trs.TrxId = rr.TrxId
                                                    and trs.FacilityMapId = fm.FacilityMapId
                                               order by trs.Cancelled, trs.TrxId, trs.LineId ) as FacilityMapName, 
                                               (SELECT top 1 p.product_name 
                                                  from products p, product_type pt, trx_lines tl
                                                 where p.product_type_id=pt.product_type_id  
                                                   and pt.product_type = 'BOOKINGS'
                                                   and tl.product_id = p.product_id
                                                   and tl.TrxId = rr.TrxId
                                                 order by ISNULL(tl.CancelledTime, getdate()+100) desc, tl.TrxId, tl.LineId) as bookingProductName, 
                                                 rr.ServiceChargePercentage,
                                                 rr.ServiceChargeAmount,																							    
											   th.CustomerId as customerId,
											   '' as contactNo, 
                                               '' as alternateContactNo,
											   '' as email,
                                               '' as customerName, 
											   '' as gender,
											   '' as postalAddress
                                          from Bookings rr 
                                               left outer join  trx_header th on th.trxId = rr.TrxId 
                                               left outer join cards cc on cc.card_id = rr.CardId  
            ";

        /// <summary>
        /// Insert bookings data
        /// </summary>
        /// <param name="reservationDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ReservationDTO InsertReservationDTO(ReservationDTO reservationDTO, string userId, int siteId)
        {
            log.LogMethodEntry(reservationDTO, userId, siteId);
            try
            {
                string insertBookingsQuery = @"insert into bookings 
                                                        (  
                                                       BookingClassId
                                                       ,BookingName
                                                       ,FromDate
                                                       ,recur_flag
                                                       ,recur_frequency
                                                       ,recur_end_date
                                                       ,Quantity
                                                       ,ReservationCode
                                                       ,Status
                                                       ,CardId
                                                       ,CardNumber
                                                       ,CustomerId
                                                       ,CustomerName
                                                       ,ExpiryTime
                                                       ,Channel
                                                       ,Remarks
                                                       ,CreatedBy
                                                       ,CreationDate
                                                       ,LastUpdatedBy
                                                       ,LastUpdatedDate
                                                       ,Guid
                                                       ,SynchStatus
                                                       ,site_id
                                                       ,ContactNo
                                                       ,AlternateContactNo
                                                       ,Email
                                                       ,isEmailSent
                                                       ,ToDate
                                                       ,TrxId
                                                       ,Age
                                                       ,Gender
                                                       ,PostalAddress
                                                       ,BookingProductId
                                                       ,AttractionScheduleId
                                                       ,MasterEntityId
                                                       ,ExtraGuests
                                                       --,EventHostUserId
                                                       --,ChecklistTaskGroupId
                                                       ,ServiceChargeAmount
                                                       ,ServiceChargePercentage
                                                        ) 
                                                values 
                                                        (
                                                        @BookingClassId
                                                       ,@BookingName
                                                       ,@FromDate
                                                       ,@recur_flag
                                                       ,@recur_frequency
                                                       ,@recur_end_date
                                                       ,@Quantity
                                                       ,@ReservationCode
                                                       ,@Status
                                                       ,@CardId
                                                       ,@CardNumber
                                                       ,@CustomerId
                                                       ,@CustomerName
                                                       ,@ExpiryTime
                                                       ,@Channel
                                                       ,@Remarks
                                                       ,@CreatedBy
                                                       ,GETDATE()
                                                       ,@LastUpdatedBy
                                                       ,GETDATE()
                                                       ,NEWID()
                                                       ,@SynchStatus
                                                       ,@site_id
                                                       ,@ContactNo
                                                       ,@AlternateContactNo
                                                       ,@Email
                                                       ,@isEmailSent
                                                       ,@ToDate
                                                       ,@TrxId
                                                       ,@Age
                                                       ,@Gender
                                                       ,@PostalAddress
                                                       ,@BookingProductId
                                                       ,@AttractionScheduleId
                                                       ,@MasterEntityId
                                                       ,@ExtraGuests
                                                       --,@EventHostId
                                                       --,@CheckListTaskGroupId 
                                                       ,@ServiceChargeAmount
                                                       ,@ServiceChargePercentage
                                                         )
                                         SELECT * FROM bookings WHERE bookingId = scope_identity()";

                List<SqlParameter> insertBookingParameters = new List<SqlParameter>();

                insertBookingParameters.Add(new SqlParameter("@BookingClassId", reservationDTO.BookingClassId == -1 ? DBNull.Value : (object)reservationDTO.BookingClassId));
                insertBookingParameters.Add(new SqlParameter("@BookingName", string.IsNullOrEmpty(reservationDTO.BookingName) ? DBNull.Value : (object)reservationDTO.BookingName));
                insertBookingParameters.Add(new SqlParameter("@FromDate", reservationDTO.FromDate.Year <= 1900 ? DBNull.Value : (object)reservationDTO.FromDate));
                insertBookingParameters.Add(new SqlParameter("@recur_flag", string.IsNullOrEmpty(reservationDTO.RecurFlag) ? "N" : (reservationDTO.RecurFlag.ToUpper() == "YES" ? "Y" : "N")));
                insertBookingParameters.Add(new SqlParameter("@recur_frequency", string.IsNullOrEmpty(reservationDTO.RecurFrequency) ? DBNull.Value : (object)reservationDTO.RecurFrequency.ToUpper().Substring(0, 1)));
                insertBookingParameters.Add(new SqlParameter("@recur_end_date", (reservationDTO.RecurEndDate == null ? DBNull.Value : (((DateTime)reservationDTO.RecurEndDate).Year < 1900 ? DBNull.Value : (object)reservationDTO.RecurEndDate))));
                insertBookingParameters.Add(new SqlParameter("@Quantity", reservationDTO.Quantity == -1 ? DBNull.Value : (object)reservationDTO.Quantity));
                insertBookingParameters.Add(new SqlParameter("@ReservationCode", string.IsNullOrEmpty(reservationDTO.ReservationCode) ? DBNull.Value : (object)reservationDTO.ReservationCode));
                insertBookingParameters.Add(new SqlParameter("@Status", string.IsNullOrEmpty(reservationDTO.Status) ? DBNull.Value : (object)reservationDTO.Status));
                insertBookingParameters.Add(new SqlParameter("@CardId", reservationDTO.CardId == -1 ? DBNull.Value : (object)reservationDTO.CardId));
                insertBookingParameters.Add(new SqlParameter("@CardNumber", string.IsNullOrEmpty(reservationDTO.CardNumber) ? DBNull.Value : (object)reservationDTO.CardNumber));
                insertBookingParameters.Add(new SqlParameter("@CustomerId", reservationDTO.CustomerId == -1 ? DBNull.Value : (object)reservationDTO.CustomerId));
                insertBookingParameters.Add(new SqlParameter("@CustomerName", string.IsNullOrEmpty(reservationDTO.CustomerName) ? DBNull.Value : (object)reservationDTO.CustomerName));
                insertBookingParameters.Add(new SqlParameter("@ExpiryTime", (reservationDTO.ExpiryTime == null ? DBNull.Value : (((DateTime)reservationDTO.ExpiryTime).Year < 1900 ? DBNull.Value : (object)reservationDTO.ExpiryTime))));
                insertBookingParameters.Add(new SqlParameter("@Channel", string.IsNullOrEmpty(reservationDTO.Channel) ? DBNull.Value : (object)reservationDTO.Channel));
                insertBookingParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(reservationDTO.Remarks) ? DBNull.Value : (object)reservationDTO.Remarks));
                insertBookingParameters.Add(new SqlParameter("@CreatedBy", userId));
                insertBookingParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
                insertBookingParameters.Add(new SqlParameter("@synchStatus", (reservationDTO.SynchStatus == true ? (object)reservationDTO.SynchStatus : DBNull.Value)));
                insertBookingParameters.Add(new SqlParameter("@site_id", reservationDTO.SiteId == -1 ? DBNull.Value : (object)reservationDTO.SiteId));
                insertBookingParameters.Add(new SqlParameter("@ContactNo", string.IsNullOrEmpty(reservationDTO.ContactNo) ? DBNull.Value : (object)reservationDTO.ContactNo));
                insertBookingParameters.Add(new SqlParameter("@AlternateContactNo", string.IsNullOrEmpty(reservationDTO.AlternateContactNo) ? DBNull.Value : (object)reservationDTO.AlternateContactNo));
                insertBookingParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(reservationDTO.Email) ? DBNull.Value : (object)reservationDTO.Email));
                insertBookingParameters.Add(new SqlParameter("@isEmailSent", reservationDTO.IsEmailSent == -1 ? DBNull.Value : (object)reservationDTO.IsEmailSent));
                insertBookingParameters.Add(new SqlParameter("@ToDate", reservationDTO.ToDate.Year <= 1900 ? DBNull.Value : (object)reservationDTO.ToDate));
                insertBookingParameters.Add(new SqlParameter("@TrxId", reservationDTO.TrxId == -1 ? DBNull.Value : (object)reservationDTO.TrxId));
                insertBookingParameters.Add(new SqlParameter("@Age", reservationDTO.Age == -1 ? DBNull.Value : (object)reservationDTO.Age));
                insertBookingParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(reservationDTO.Gender) ? DBNull.Value : (object)reservationDTO.Gender));
                insertBookingParameters.Add(new SqlParameter("@PostalAddress", string.IsNullOrEmpty(reservationDTO.PostalAddress) ? DBNull.Value : (object)reservationDTO.PostalAddress));
                insertBookingParameters.Add(new SqlParameter("@BookingProductId", reservationDTO.BookingProductId == -1 ? DBNull.Value : (object)reservationDTO.BookingProductId));
                insertBookingParameters.Add(new SqlParameter("@AttractionScheduleId", reservationDTO.AttractionScheduleId == -1 ? DBNull.Value : (object)reservationDTO.AttractionScheduleId));
                insertBookingParameters.Add(new SqlParameter("@MasterEntityId", reservationDTO.MasterEntityId == -1 ? DBNull.Value : (object)reservationDTO.MasterEntityId));
                insertBookingParameters.Add(new SqlParameter("@ExtraGuests", reservationDTO.ExtraGuests == -1 ? DBNull.Value : (object)reservationDTO.ExtraGuests));
                //insertBookingParameters.Add(new SqlParameter("@EventHostId", reservationDTO.EventHostId == -1 ? DBNull.Value : (object)reservationDTO.EventHostId));
                //insertBookingParameters.Add(new SqlParameter("@CheckListTaskGroupId", reservationDTO.CheckListTaskGroupId == -1 ? DBNull.Value : (object)reservationDTO.CheckListTaskGroupId));
                insertBookingParameters.Add(new SqlParameter("@ServiceChargePercentage", reservationDTO.ServiceChargePercentage == 0 ? DBNull.Value : (object)reservationDTO.ServiceChargePercentage));
                insertBookingParameters.Add(new SqlParameter("@ServiceChargeAmount", reservationDTO.ServiceChargeAmount == 0 ? DBNull.Value : (object)reservationDTO.ServiceChargeAmount));

                //int idOfRowInserted = dataAccessHandler.executeInsertQuery(insertBookingsQuery, insertBookingParameters.ToArray(), sqlTransaction);
                DataTable dt = dataAccessHandler.executeSelectQuery(insertBookingsQuery, insertBookingParameters.ToArray(), sqlTransaction);
                RefreshReservationDTO(reservationDTO, dt);

                log.LogMethodExit(reservationDTO);
                return reservationDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }

        }


        /// <summary>
        /// Update bookings entry
        /// </summary>
        /// <param name="reservationDTO"></param>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public ReservationDTO UpdateReservationDTO(ReservationDTO reservationDTO, string userId, int siteId)
        {
            log.LogMethodEntry(reservationDTO, userId, siteId);
            string updateBookingQuery = @"update dbo.Bookings
                                                        SET BookingClassId = @BookingClassId,  
                                                        BookingName = @BookingName, 
                                                        FromDate = @FromDate, 
                                                        ToDate = @ToDate, 
                                                        recur_flag = @recur_flag, 
                                                        recur_frequency = @recur_frequency, 
                                                        recur_end_date = @recur_end_date, 
                                                        Quantity = @Quantity, 
                                                        ReservationCode = @ReservationCode, 
                                                        Status = @Status, 
                                                        CardId = @CardId, 
                                                        CardNumber = @CardNumber,
                                                        CustomerId = @CustomerId, 
                                                        CustomerName = @CustomerName,
                                                        ExpiryTime = @ExpiryTime, 
                                                        Channel = @Channel,
                                                        Remarks = @Remarks,
                                                        LastUpdatedBy = @LastUpdatedBy, 
                                                        LastUpdatedDate = getdate(), 
                                                        SynchStatus = @SynchStatus,
                                                        -- site_id = @site_id, 
                                                        ContactNo = @ContactNo,
                                                        AlternateContactNo = @AlternateContactNo,
                                                        Email = @Email, 
                                                        isEmailSent = @isEmailSent, 
                                                        TrxId = @TrxId, 
                                                        Age = @Age, 
                                                        Gender = @Gender, 
                                                        PostalAddress = @PostalAddress,
                                                        BookingProductId = @BookingProductId, 
                                                        AttractionScheduleId = @AttractionScheduleId, 
                                                        MasterEntityId = @MasterEntityId, 
                                                        ExtraGuests = @ExtraGuests,
                                                       -- EventHostUserId = @EventHostId,
                                                       -- ChecklistTaskGroupId = @CheckListTaskGroupId,
                                                        ServiceChargeAmount = @ServiceChargeAmount,
                                                       ServiceChargePercentage = @ServiceChargePercentage
                                                  where BookingId=@BookingId;
                                          SELECT * FROM Bookings WHERE BookingId = @BookingId ";
            
            List<SqlParameter> updateBookingParameters = new List<SqlParameter>();
            updateBookingParameters.Add(new SqlParameter("@BookingId", reservationDTO.BookingId == -1 ? DBNull.Value : (object)reservationDTO.BookingId));
            updateBookingParameters.Add(new SqlParameter("@BookingClassId", reservationDTO.BookingClassId == -1 ? DBNull.Value : (object)reservationDTO.BookingClassId));
            updateBookingParameters.Add(new SqlParameter("@BookingName", string.IsNullOrEmpty(reservationDTO.BookingName) ? DBNull.Value : (object)reservationDTO.BookingName));
            updateBookingParameters.Add(new SqlParameter("@FromDate", reservationDTO.FromDate.Year <= 1900 ? DBNull.Value : (object)reservationDTO.FromDate));
            updateBookingParameters.Add(new SqlParameter("@recur_flag", string.IsNullOrEmpty(reservationDTO.RecurFlag) ? "N" : (reservationDTO.RecurFlag.ToUpper() == "YES" ? "Y" : "N")));
            updateBookingParameters.Add(new SqlParameter("@recur_frequency", string.IsNullOrEmpty(reservationDTO.RecurFrequency) ? DBNull.Value : (object)reservationDTO.RecurFrequency.ToUpper().Substring(0, 1)));
            updateBookingParameters.Add(new SqlParameter("@recur_end_date", (reservationDTO.RecurEndDate == null ? DBNull.Value : (((DateTime)reservationDTO.RecurEndDate).Year < 1900 ? DBNull.Value : (object)reservationDTO.RecurEndDate))));
            updateBookingParameters.Add(new SqlParameter("@Quantity", reservationDTO.Quantity == -1 ? DBNull.Value : (object)reservationDTO.Quantity));
            updateBookingParameters.Add(new SqlParameter("@ReservationCode", string.IsNullOrEmpty(reservationDTO.ReservationCode) ? DBNull.Value : (object)reservationDTO.ReservationCode));
            updateBookingParameters.Add(new SqlParameter("@Status", string.IsNullOrEmpty(reservationDTO.Status) ? DBNull.Value : (object)reservationDTO.Status));
            updateBookingParameters.Add(new SqlParameter("@CardId", reservationDTO.CardId == -1 ? DBNull.Value : (object)reservationDTO.CardId));
            updateBookingParameters.Add(new SqlParameter("@CardNumber", string.IsNullOrEmpty(reservationDTO.CardNumber) ? DBNull.Value : (object)reservationDTO.CardNumber));
            updateBookingParameters.Add(new SqlParameter("@CustomerId", reservationDTO.CustomerId == -1 ? DBNull.Value : (object)reservationDTO.CustomerId));
            updateBookingParameters.Add(new SqlParameter("@CustomerName", string.IsNullOrEmpty(reservationDTO.CustomerName) ? DBNull.Value : (object)reservationDTO.CustomerName));
            updateBookingParameters.Add(new SqlParameter("@ExpiryTime", (reservationDTO.ExpiryTime == null ? DBNull.Value : (((DateTime)reservationDTO.ExpiryTime).Year < 1900 ? DBNull.Value : (object)reservationDTO.ExpiryTime))));
            updateBookingParameters.Add(new SqlParameter("@Channel", string.IsNullOrEmpty(reservationDTO.Channel) ? DBNull.Value : (object)reservationDTO.Channel));
            updateBookingParameters.Add(new SqlParameter("@Remarks", string.IsNullOrEmpty(reservationDTO.Remarks) ? DBNull.Value : (object)reservationDTO.Remarks));
            updateBookingParameters.Add(new SqlParameter("@CreatedBy", userId));
            updateBookingParameters.Add(new SqlParameter("@LastUpdatedBy", userId));
            updateBookingParameters.Add(new SqlParameter("@synchStatus", (reservationDTO.SynchStatus == true ? (object)reservationDTO.SynchStatus : DBNull.Value)));
            updateBookingParameters.Add(new SqlParameter("@site_id", reservationDTO.SiteId == -1 ? DBNull.Value : (object)reservationDTO.SiteId));
            updateBookingParameters.Add(new SqlParameter("@ContactNo", string.IsNullOrEmpty(reservationDTO.ContactNo) ? DBNull.Value : (object)reservationDTO.ContactNo));
            updateBookingParameters.Add(new SqlParameter("@AlternateContactNo", string.IsNullOrEmpty(reservationDTO.AlternateContactNo) ? DBNull.Value : (object)reservationDTO.AlternateContactNo));
            updateBookingParameters.Add(new SqlParameter("@Email", string.IsNullOrEmpty(reservationDTO.Email) ? DBNull.Value : (object)reservationDTO.Email));
            updateBookingParameters.Add(new SqlParameter("@isEmailSent", reservationDTO.IsEmailSent == -1 ? DBNull.Value : (object)reservationDTO.IsEmailSent));
            updateBookingParameters.Add(new SqlParameter("@ToDate", reservationDTO.ToDate.Year <= 1900 ? DBNull.Value : (object)reservationDTO.ToDate));
            updateBookingParameters.Add(new SqlParameter("@TrxId", reservationDTO.TrxId == -1 ? DBNull.Value : (object)reservationDTO.TrxId));
            updateBookingParameters.Add(new SqlParameter("@Age", reservationDTO.Age == -1 ? DBNull.Value : (object)reservationDTO.Age));
            updateBookingParameters.Add(new SqlParameter("@Gender", string.IsNullOrEmpty(reservationDTO.Gender) ? DBNull.Value : (object)reservationDTO.Gender));
            updateBookingParameters.Add(new SqlParameter("@PostalAddress", string.IsNullOrEmpty(reservationDTO.PostalAddress) ? DBNull.Value : (object)reservationDTO.PostalAddress));
            updateBookingParameters.Add(new SqlParameter("@BookingProductId", reservationDTO.BookingProductId == -1 ? DBNull.Value : (object)reservationDTO.BookingProductId));
            updateBookingParameters.Add(new SqlParameter("@AttractionScheduleId", reservationDTO.AttractionScheduleId == -1 ? DBNull.Value : (object)reservationDTO.AttractionScheduleId));
            updateBookingParameters.Add(new SqlParameter("@MasterEntityId", reservationDTO.MasterEntityId == -1 ? DBNull.Value : (object)reservationDTO.MasterEntityId));
            updateBookingParameters.Add(new SqlParameter("@ExtraGuests", reservationDTO.ExtraGuests == -1 ? DBNull.Value : (object)reservationDTO.ExtraGuests));
            //updateBookingParameters.Add(new SqlParameter("@EventHostId", reservationDTO.EventHostId == -1 ? DBNull.Value : (object)reservationDTO.EventHostId));
            //updateBookingParameters.Add(new SqlParameter("@CheckListTaskGroupId", reservationDTO.CheckListTaskGroupId == -1 ? DBNull.Value : (object)reservationDTO.CheckListTaskGroupId));
            updateBookingParameters.Add(new SqlParameter("@ServiceChargePercentage", reservationDTO.ServiceChargePercentage == 0 ? DBNull.Value : (object)reservationDTO.ServiceChargePercentage));
            updateBookingParameters.Add(new SqlParameter("@ServiceChargeAmount", reservationDTO.ServiceChargeAmount == 0 ? DBNull.Value : (object)reservationDTO.ServiceChargeAmount));

            //int rowsUpdated = dataAccessHandler.executeUpdateQuery(updateBookingQuery, updateBookingParameters.ToArray(), sqlTransaction);
            DataTable dt = dataAccessHandler.executeSelectQuery(updateBookingQuery, updateBookingParameters.ToArray(), sqlTransaction);
            RefreshReservationDTO(reservationDTO, dt);
            log.LogMethodExit(reservationDTO);
            return reservationDTO;

        }

        private void RefreshReservationDTO(ReservationDTO reservationDTO, DataTable dt)
        {
            log.LogMethodEntry(reservationDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                reservationDTO.BookingId = Convert.ToInt32(dt.Rows[0]["BookingId"]);
                reservationDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                reservationDTO.LastupdateDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                reservationDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString();
                reservationDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : dataRow["LastUpdatedBy"].ToString();
                reservationDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : dataRow["CreatedBy"].ToString();
                reservationDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ReservationDTO class type
        /// </summary>
        /// <param name="reservationDTODataRow"></param>
        /// <returns></returns>
        private ReservationDTO GetReservationDTO(DataRow reservationDTODataRow)
        {

            log.LogMethodEntry(reservationDTODataRow);
            ReservationDTO reservationDTO = new ReservationDTO(
            reservationDTODataRow["BookingId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["BookingId"]),
            reservationDTODataRow["BookingClassId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["BookingClassId"]),
            reservationDTODataRow["BookingName"].ToString(),
            reservationDTODataRow["FromDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reservationDTODataRow["FromDate"]),
            reservationDTODataRow["recur_flag"].ToString(),
            reservationDTODataRow["recur_frequency"].ToString(),
            reservationDTODataRow["recur_end_date"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reservationDTODataRow["recur_end_date"]),
            reservationDTODataRow["Quantity"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["Quantity"]),
            reservationDTODataRow["ReservationCode"].ToString(),
            reservationDTODataRow["Status"].ToString(),
            reservationDTODataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["CardId"]),
            reservationDTODataRow["CardNumber"].ToString(),
            reservationDTODataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["CustomerId"]),
            reservationDTODataRow["CustomerName"].ToString(),
            reservationDTODataRow["ExpiryTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reservationDTODataRow["ExpiryTime"]),
            reservationDTODataRow["Channel"].ToString(),
            reservationDTODataRow["Remarks"].ToString(),
            reservationDTODataRow["CreatedBy"].ToString(),
            reservationDTODataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reservationDTODataRow["CreationDate"]),
            reservationDTODataRow["LastUpdatedBy"].ToString(),
            reservationDTODataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reservationDTODataRow["LastUpdatedDate"]),
            reservationDTODataRow["Guid"].ToString(),
            reservationDTODataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(reservationDTODataRow["SynchStatus"]),
            reservationDTODataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["site_id"]),
            reservationDTODataRow["ContactNo"].ToString(),
            reservationDTODataRow["AlternateContactNo"].ToString(),
            reservationDTODataRow["Email"].ToString(),
            reservationDTODataRow["isEmailSent"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["isEmailSent"]),
            reservationDTODataRow["ToDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reservationDTODataRow["ToDate"]),
            reservationDTODataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["TrxId"]),
            reservationDTODataRow["Age"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["Age"]),
            reservationDTODataRow["Gender"].ToString(),
            reservationDTODataRow["PostalAddress"].ToString(),
            reservationDTODataRow["BookingProductId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["BookingProductId"]),
            reservationDTODataRow["AttractionScheduleId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["AttractionScheduleId"]),
            reservationDTODataRow["ExtraGuests"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["ExtraGuests"]),
            reservationDTODataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["MasterEntityId"]),
            reservationDTODataRow["trx_no"].ToString(),
            reservationDTODataRow["TrxStatus"].ToString(),
            reservationDTODataRow["TrxNetAmount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reservationDTODataRow["TrxNetAmount"]),
            reservationDTODataRow["BookingProductName"].ToString(),
            //reservationDTODataRow["EventHostUserId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["EventHostUserId"]),
            //reservationDTODataRow["EventHostName"].ToString(),
            //reservationDTODataRow["ChecklistTaskGroupId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["ChecklistTaskGroupId"]),
            //reservationDTODataRow["CheckListTaskGroupName"].ToString(),
            reservationDTODataRow["ServiceChargeAmount"] == DBNull.Value ? 0 : Convert.ToDouble(reservationDTODataRow["ServiceChargeAmount"]),
            reservationDTODataRow["ServiceChargePercentage"] == DBNull.Value ? 0 : Convert.ToDouble(reservationDTODataRow["ServiceChargePercentage"]),
            reservationDTODataRow["FacilityMapId"] == DBNull.Value ? -1 : Convert.ToInt32(reservationDTODataRow["FacilityMapId"]),
            reservationDTODataRow["FacilityMapName"].ToString(),
            reservationDTODataRow["FacilityName"].ToString()
            );
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }


        /// <summary>
        ///  Gets the ReservationDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<ReservationDTO> GetReservationDTOList(List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters, ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string PassPhrase = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");

            string selectReportQuery = selectQry;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<ReservationDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperartor = (count == 0) ? " " : " and ";

                        if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + " OR -1 =" + searchParameter.Value + ") ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.TRX_ID) ||
                             (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.BOOKING_ID)) ||
                             (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.BOOKING_PRODUCT_ID) ||
                              (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.MASTER_ENTITY_ID)))
                              )
                        {
                            query.Append(joinOperartor + "(" + DBSearchParameters[searchParameter.Key] + " =  " + searchParameter.Value + ") ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.FACILITY_MAP_ID))
                        {
                            query.Append(joinOperartor + " exists (SELECT 1 from TrxReservationSchedule trs, trx_lines tl " +
                                                         " where trs.TrxId = tl.TrxId and trs.FacilityMapId = " + searchParameter.Value +
                                                           " and trs.LineId = tl.LineId and tl.TrxId = th.TrxId and tl.CancelledTime is null)  ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.CHECKLIST_TASK_ASSIGNEE_ID))
                        {
                            query.Append(joinOperartor + @" exists (select 1 from Maint_ChecklistDetails uji, Maint_SchAssetTasks ujt
                                                                     where uji.AssignedUserId = " + searchParameter.Value +
                                                                     " and uji.MaintschAssetTaskId = ujt.MaintSchAssetTaskId and ujt.BookingId = rr.BookingId)  ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.STATUS))
                        {
                            query.Append(joinOperartor + " " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.TRANSACTION_GUID))
                        {
                            query.Append(joinOperartor + " " + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.STATUS_LIST_IN))
                        {
                            query.Append(joinOperartor + " " + DBSearchParameters[searchParameter.Key] + " in ( " + searchParameter.Value + " ) ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN))
                        {
                            query.Append(joinOperartor + " " + DBSearchParameters[searchParameter.Key] + " not in ( " + searchParameter.Value + " ) ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.CARD_NUMBER_LIKE) ||
                                 searchParameter.Key.Equals(ReservationDTO.SearchByParameters.RESERVATION_CODE_LIKE))
                        {
                            query.Append(joinOperartor + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') like N'%" + searchParameter.Value + "%' ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.RESERVATION_CODE_EXACT))
                        {
                            query.Append(joinOperartor + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') = N'" + searchParameter.Value + "' ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.CUSTOMER_NAME_LIKE))
                        {
                            //query.Append(joinOperartor + " (rr.CustomerName like N'%" + searchParameter.Value + "%' or  ISNULL(c.customer_name,'') +' ' + isnull(c.last_name, '') like N'%" + searchParameter.Value + "%' )");
                            query.Append(joinOperartor + @"(exists (SELECT 1 
										                              FROM CustomerView(@PassPhrase) c 
													                 WHERE c.customer_id = th.CustomerId 
													                   AND ISNULL(c.customer_name,'') +' ' + isnull(c.last_name, '') like N'%" + searchParameter.Value + @"%'
                                                                     ) OR rr.CustomerName like N'%" + searchParameter.Value + "%')");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE))
                        {
                            query.Append(joinOperartor + "  ( rr.FromDate >= '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy")
                                                             + "' or(rr.recur_flag = 'Y' and rr.recur_end_date >= '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy") + "' ) )   ");
                        }
                        else if (searchParameter.Key.Equals(ReservationDTO.SearchByParameters.RESERVATION_TO_DATE))
                        {
                            query.Append(joinOperartor + "  ( rr.FromDate < '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy") +
                                                         "'  or(rr.recur_flag = 'Y' and(recur_end_date < '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy")
                                                                                      + "' or(rr.recur_end_date > '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy") + "' and rr.FromDate < '" + Convert.ToDateTime(searchParameter.Value, CultureInfo.InvariantCulture).ToString("MM-dd-yyyy") + "' ))))  ");
                        }
                        else
                        {
                            query.Append(joinOperartor + " ISNULL(" + DBSearchParameters[searchParameter.Key] + ",'') like N'%" + searchParameter.Value + "%' ");
                        }

                        count++;
                    }
                    else
                    {
                        log.Debug("Ends-GetReservationDTOList(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception("The query parameter does not exist " + searchParameter.Key);
                    }
                }

                if (searchParameters.Count > 0)
                {
                    selectReportQuery = selectReportQuery + query;
                }
                selectReportQuery = selectReportQuery + " Order by rr.FromDate, rr.ToDate";
            }
            //dataAccessHandler.CommandTimeOut = 180;
            DataTable dtReservationDTO = dataAccessHandler.executeSelectQuery(selectReportQuery, new SqlParameter[] { new SqlParameter("@PassPhrase", PassPhrase) }, sqlTransaction);
            List<ReservationDTO> reservationDTOList = new List<ReservationDTO>();
            if (dtReservationDTO.Rows.Count > 0)
            {
                DataTable dtCustomerDto = GetCustomerData(dtReservationDTO, PassPhrase);
                Dictionary<int, DataRow> customerData = new Dictionary<int, DataRow>();
                foreach (DataRow bookingDataRow in dtReservationDTO.Rows)
                {
                    ReservationDTO reservationDTO = GetReservationDTO(bookingDataRow);
                    reservationDTO = MergeCustomerDataIntoReservationData(reservationDTO, dtCustomerDto, customerData);
                    reservationDTOList.Add(reservationDTO);
                }
            }
            log.LogMethodExit(reservationDTOList);
            return reservationDTOList;
        }


        /// <summary>
        /// Gets bookings data of passed bookingId
        /// </summary>
        /// <param name="machineUserContext"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public ReservationDTO GetReservationDTO(ExecutionContext machineUserContext, int bookingId)
        {
            log.LogMethodEntry(machineUserContext, bookingId);
            string PassPhrase = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            //string selectBookingQuery = @"SELECT rr.BookingId, rr.BookingName, rr.FromDate , rr.ToDate, rr.status, 
            //                                    case rr.recur_flag when 'Y' then 'Yes' else 'No' end as recur_flag,
            //                                    case rr.recur_frequency when 'D' then 'Daily' when 'W' then 'Weekly' else '' end as recur_frequency,
            //                                    rr.recur_end_date , rr.Remarks, rr.TrxId, rr.Quantity, rr.ReservationCode, rr.CardNumber,  
            //                                    rr.bookingClassId, rr.cardid, rr.customerId, rr.expiryTime,rr.channel, rr.createdBy, rr.creationDate, rr.lastUpdatedBy, 
            //                                    rr.LastUpdatedDate, 
            //                                    rr.guid, rr.synchStatus, rr.site_Id, rr.contactNo, rr.alternateContactNo, rr.email, rr.isEmailSent,  rr.age,
            //                                    rr.gender, rr.postalAddress, rr.bookingProductId, rr.attractionScheduleId, rr.extraGuests, rr.masterEntityId, 
            //                                   isnull(c.customer_name + isnull(c.last_name, ''), customerName) customerName, th.trx_no, th.Status TrxStatus, th.TrxNetAmount,
            //                                   fac.FacilityId, fac.FacilityName, p.product_name as bookingProductName,
            //                                    rr.EventHostUserId,
            //                                    u.username as EventHostName,
            //                                    rr.ChecklistTaskGroupId,
            //                                    taskgroup.TaskGroupName as CheckListTaskGroupName
            //                              from Bookings rr
            //                                   left outer join CustomerView(@PassPhrase) c on c.customer_id = rr.CustomerId
            //                                   left outer join AttractionSchedules asch on asch.AttractionScheduleId = rr.AttractionScheduleId
            //                                   left outer join CheckInFacility fac on fac.FacilityId = asch.FacilityId
            //                                   left outer join  trx_header th on th.trxId = rr.TrxId
            //                                   left outer join products p on p.product_id = rr.bookingProductId
            //                                   left outer join users u on u.user_id = rr.EventHostUserId
            //                                   left outer join Maint_TaskGroups taskgroup on rr.ChecklistTaskGroupId = taskgroup.MaintTaskGroupId 
            //                              where rr.bookingId = @BookingId";
            string selectBookingQuery = selectQry + " where rr.bookingId = @BookingId";
            SqlParameter[] selectBookingParameters = new SqlParameter[2];
            selectBookingParameters[0] = new SqlParameter("@BookingId", bookingId);
            selectBookingParameters[1] = new SqlParameter("@PassPhrase", PassPhrase);
            DataTable dtBooking = dataAccessHandler.executeSelectQuery(selectBookingQuery, selectBookingParameters, sqlTransaction);
            ReservationDTO reservationDTO = new ReservationDTO();
            if (dtBooking.Rows.Count > 0)
            {
                DataTable dtCustomerDto = GetCustomerData(dtBooking, PassPhrase);
                Dictionary<int, DataRow> customerData = new Dictionary<int, DataRow>();
                // dtBooking = MergeCustomerDataIntoReservationData(dtBooking, dtCustomerDto, customerData);
                DataRow reportRow = dtBooking.Rows[0];
                reservationDTO = GetReservationDTO(reportRow);
                reservationDTO = MergeCustomerDataIntoReservationData(reservationDTO, dtCustomerDto, customerData);
            }
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }
        /// <summary>
        /// Gets bookings data of passed reservation code
        /// </summary>
        /// <param name="machineUserContext"></param>
        /// <param name="reservationCode"></param>
        /// <returns></returns>
        public ReservationDTO GetReservationDTO(ExecutionContext machineUserContext, string reservationCode)
        {
            log.LogMethodEntry(machineUserContext, reservationCode);
            string PassPhrase = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE");
            //string selectBookingQuery = @"SELECT rr.BookingId, rr.BookingName, rr.FromDate , rr.ToDate, rr.status, 
            //                                    case rr.recur_flag when 'Y' then 'Yes' else 'No' end as recur_flag,
            //                                    case rr.recur_frequency when 'D' then 'Daily' when 'W' then 'Weekly' else '' end as recur_frequency,
            //                                    rr.recur_end_date , rr.Remarks, rr.TrxId, rr.Quantity, rr.ReservationCode, rr.CardNumber,  
            //                                    rr.bookingClassId, rr.cardid, rr.customerId, rr.expiryTime,rr.channel, rr.createdBy, rr.creationDate, rr.lastUpdatedBy, 
            //                                    rr.LastUpdatedDate, 
            //                                    rr.guid, rr.synchStatus, rr.site_Id, rr.contactNo, rr.alternateContactNo, rr.email, rr.isEmailSent,  rr.age,
            //                                    rr.gender, rr.postalAddress, rr.bookingProductId, rr.attractionScheduleId, rr.extraGuests, rr.masterEntityId, 
            //                                   isnull(c.customer_name + isnull(c.last_name, ''), customerName) customerName, th.trx_no, th.Status TrxStatus, th.TrxNetAmount,
            //                                   fac.FacilityId, fac.FacilityName, p.product_name as bookingProductName,
            //                                    rr.EventHostUserId,
            //                                    u.username as EventHostName,
            //                                    rr.ChecklistTaskGroupId,
            //                                    taskgroup.TaskGroupName as CheckListTaskGroupName
            //                              from Bookings rr
            //                                   left outer join CustomerView(@PassPhrase) c on c.customer_id = rr.CustomerId
            //                                   left outer join AttractionSchedules asch on asch.AttractionScheduleId = rr.AttractionScheduleId
            //                                   left outer join CheckInFacility fac on fac.FacilityId = asch.FacilityId
            //                                   left outer join  trx_header th on th.trxId = rr.TrxId
            //                                   left outer join products p on p.product_id = rr.bookingProductId
            //                                   left outer join users u on u.user_id = rr.EventHostUserId
            //                                   left outer join Maint_TaskGroups taskgroup on rr.ChecklistTaskGroupId = taskgroup.MaintTaskGroupId 
            //                              where rr.ReservationCode = @reservationCode";
            string selectBookingQuery = selectQry + " where rr.ReservationCode = @reservationCode";
            SqlParameter[] selectBookingParameters = new SqlParameter[2];
            selectBookingParameters[0] = new SqlParameter("@reservationCode", reservationCode);
            selectBookingParameters[1] = new SqlParameter("@PassPhrase", PassPhrase);
            DataTable dtBooking = dataAccessHandler.executeSelectQuery(selectBookingQuery, selectBookingParameters, sqlTransaction);
            ReservationDTO reservationDTO = new ReservationDTO();
            if (dtBooking.Rows.Count > 0)
            {
                DataTable dtCustomerDto = GetCustomerData(dtBooking, PassPhrase);
                Dictionary<int, DataRow> customerData = new Dictionary<int, DataRow>();
                DataRow reportRow = dtBooking.Rows[0];
                reservationDTO = GetReservationDTO(reportRow);
                reservationDTO = MergeCustomerDataIntoReservationData(reservationDTO, dtCustomerDto, customerData);
            }
            log.LogMethodExit(reservationDTO);
            return reservationDTO;
        }

        /// <summary>
        /// ValidateLogin
        /// </summary>
        /// <param name="parafaitUtility"></param>
        /// <param name="LoginId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        public UsersDTO ValidateLogin(Utilities parafaitUtility, string LoginId, int SiteId = -1)
        {
            if (LoginId.ToString() == null && LoginId.ToString() == "")
            {
                throw new Exception("Login Id is Invalid!");
            }

            UsersDTO userDTO;
            if (parafaitUtility.ParafaitEnv.IsCorporate)
            {
                userDTO = new Users(parafaitUtility.ExecutionContext, LoginId, SiteId).UserDTO;
            }
            else
            {
                userDTO = new Users(parafaitUtility.ExecutionContext, LoginId).UserDTO;
            }

            if (userDTO == null || userDTO.UserId <= 0)
            {
                throw new Exception("Login Id does not exist!");
            }

            return userDTO;

        }

        /// <summary>
        /// checkIsCorporate
        /// </summary>
        /// <returns></returns>
        private bool checkIsCorporate()
        {
            string getSiteQuery = @"select count(*) sitecount from site";
            DataTable dtSites = dataAccessHandler.executeSelectQuery(getSiteQuery, null);

            if (Convert.ToInt32(dtSites.Rows[0]["sitecount"]) > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private DataTable GetCustomerData(DataTable dtReservationDTO, string passPhrase)
        {
            log.LogMethodEntry();
            DataTable customerDT = null;
            if (dtReservationDTO != null && dtReservationDTO.Rows.Count > 0)
            {
                List<int> customerIdList = new List<int>();
                foreach (DataRow bookingDataRow in dtReservationDTO.Rows)
                {
                    int customerId = bookingDataRow["CustomerId"] == DBNull.Value ? -1 : Convert.ToInt32(bookingDataRow["CustomerId"]);
                    if (customerId > -1 && customerIdList.Exists(id => id == customerId) == false)
                    {
                        customerIdList.Add(customerId);
                    }
                }
                if (customerIdList != null && customerIdList.Any())
                {
                    string custQry = @"SELECT customers.customer_id as CustomerId, 
                                              ISNULL( Profile.FirstName,'') +' ' + isnull(Profile.LastName, '')  as CustomerName, 
	                                          Profile.Gender gender, 
	                                          CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase, Profile.Line1)) PostalAddress, 
	                                          CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.email)) Email, 
	                                          CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.contact_phone1)) ContactNo,
	                                          CONVERT(NVARCHAR(MAX),DECRYPTBYPASSPHRASE(@PassPhrase,Profile.contact_phone2)) AlternateContactNo, 
	                                          Profile.id as ProfileId,
	                                          customers.CustomerType
                                        FROM (select c.* from Customers c , @CustIdList List WHERE c.customer_id = List.Id) as Customers,
                                             (SELECT Profile.id, Profile.FirstName, Profile.LastName,   Profile.Gender,
	                                                 Address.Line1,  
	                                                 ContactEmail.Attribute1 email, 
	                                                 ContactPhone1.Attribute1 contact_phone1, 
	                                                 ContactPhone2.Attribute1 contact_phone2 
                                                FROM Profile LEFT OUTER JOIN( SELECT Address.* ,
                                                                                     ROW_NUMBER() OVER(PARTITION BY Address.ProfileId ORDER BY Address.LastUpdateDate DESC) rnk
				                                                                FROM Address
				                                                               WHERE Address.IsActive = 1) Address ON Address.ProfileId = Profile.Id AND Address.rnk = 1
                                                            LEFT OUTER JOIN (SELECT Contact.* , 
                                                                                    DENSE_RANK() OVER(PARTITION BY Contact.ProfileId ORDER BY Contact.LastUpdateDate DESC, Contact.Id DESC) rnk
				                                                               FROM Contact
				                                                              INNER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
				                                                              WHERE Contact.IsActive = 1 AND ContactType.Name = 'EMAIL') ContactEmail ON ContactEmail.ProfileId = Profile.Id 
                                                                                AND ContactEmail.rnk = 1
                                                            LEFT OUTER JOIN (SELECT Contact.* , DENSE_RANK() OVER(PARTITION BY Contact.ProfileId ORDER BY Contact.LastUpdateDate DESC, Contact.Id DESC) rnk
				                                                               FROM Contact
				                                                              INNER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
				                                                              WHERE Contact.IsActive = 1 AND ContactType.Name = 'PHONE') ContactPhone1 ON ContactPhone1.ProfileId = Profile.Id 
                                                                                AND ContactPhone1.rnk = 1
                                                            LEFT OUTER JOIN (SELECT Contact.* , DENSE_RANK() OVER(PARTITION BY Contact.ProfileId ORDER BY Contact.LastUpdateDate DESC, Contact.Id DESC) rnk
				                                                               FROM Contact
				                                                              INNER JOIN ContactType ON ContactType.Id = Contact.ContactTypeId
				                                                              WHERE Contact.IsActive = 1 AND ContactType.Name = 'PHONE') ContactPhone2 ON ContactPhone2.ProfileId = Profile.Id 
				                                                                AND ContactPhone2.rnk = 2) Profile 
                                        WHERE customers.ProfileId = Profile.Id";
                    customerDT = dataAccessHandler.BatchSelect(custQry, "@CustIdList", customerIdList, new SqlParameter[] { new SqlParameter("@PassPhrase", passPhrase) }, sqlTransaction);
                }
            }
            log.LogMethodExit();
            return customerDT;

        }

        private ReservationDTO MergeCustomerDataIntoReservationData(ReservationDTO reservationDTO, DataTable dtCustomerDto, Dictionary<int, DataRow> customerData)
        {
            log.LogMethodEntry();
            if (dtCustomerDto != null && dtCustomerDto.Rows.Count > 0)
            {
                //foreach (DataRow bookingDataRow in reservationDTO.Rows)
                // { 
                if (reservationDTO.CustomerId > -1)
                {
                    DataRow custDataRow = GetDataRow(customerData, dtCustomerDto, reservationDTO.CustomerId);
                    if (custDataRow != null)
                    {
                        reservationDTO.CustomerName = custDataRow["CustomerName"].ToString();
                        reservationDTO.ContactNo = custDataRow["ContactNo"].ToString();
                        reservationDTO.AlternateContactNo = custDataRow["AlternateContactNo"].ToString();
                        reservationDTO.Email = custDataRow["Email"].ToString();
                        reservationDTO.Gender = custDataRow["Gender"].ToString();
                        reservationDTO.PostalAddress = custDataRow["PostalAddress"].ToString();
                    }
                }
                // }
            }
            log.LogMethodExit();
            return reservationDTO;
        }

        private DataRow GetDataRow(Dictionary<int, DataRow> customerData, DataTable dtCustomerDto, int reservationCustomerId)
        {
            log.LogMethodEntry();
            DataRow custDataRowValue = null;
            if (customerData.ContainsKey(reservationCustomerId) == false)
            {
                foreach (DataRow custDataRow in dtCustomerDto.Rows)
                {
                    int customerId = Convert.ToInt32(custDataRow["CustomerId"]);
                    if (customerData.ContainsKey(customerId) == false)
                    {
                        customerData.Add(customerId, custDataRow); 
                    }
                }
            }
            try
            {
                custDataRowValue = customerData[reservationCustomerId];
            }
            catch (Exception ex)
            {
                log.Error("Unable to fetch customer record for customer id: " + reservationCustomerId, ex);
            }
            log.LogMethodExit();
            return custDataRowValue;
        }
    }
}
