/********************************************************************************************
 * Project Name - Reservation
 * Description  - Business Logic to create and save Reservations
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.50.0      03-Dec-2018      Mathew Ninan   Remove staticDataExchange from calls as Staticdataexchange
 *                                            is deprecated
 *2.50.0      28-Jan-2019   Guru S A          Booking changes   
 *2.60.0      26-Mar-2019   Guru S A          Booking phase 2 changes   
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    public class Reservation
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //        public int BookingId = -1;
        //        public int CustomerId = -1;
        //        public string Status = ReservationDTO.ReservationStatus.BOOKED.ToString();//Added 2 get the advance amout before booking on May 13, 2015//
        //        public string ReservationCode;
        //        public string CardNumber;
        //        public Transaction Transaction;
        //        TaskProcs TaskProcs;
        //        EventLog audit;
        //        Semnox.Core.Utilities.Utilities Utilities;
        //        public double AdvanceRequired = 0;
        //        public double totalAdvanceRequired = 0;
        //        DateTime ReservationDate;
        //        Card currentCard;
        //        List<KeyValuePair<PurchasedProducts, int>> purchasedProductSelected = new List<KeyValuePair<PurchasedProducts, int>>();
        //        List<KeyValuePair<PurchasedProducts, int>> purchasedAdditionalProduct = new List<KeyValuePair<PurchasedProducts, int>>();

        //        public object site_id = DBNull.Value; //Jan-20-2017- Added variable site_id for updating Booking site_id field 

        //        public int OffSetDuration = 0;

        //        public string DiscountCouponCode;

        public class clsProductList
        {
            public object Id;
            public int ProductId;
            public object Quantity;
            public object Price;
            public object Remarks;
            public int discountId;
            public int transactionProfileId;
            public object productType;//Added additional parameter to get the product Type for additional products 
            public List<KeyValuePair<PurchasedProducts, int>> purchasedModifierLst;
            public List<KeyValuePair<AttractionBooking, int>> attractionProductLst;
            public int lineId;
        }

        //        public class clsAttendeeDetails
        //        {
        //            public int Id;
        //            public string Name;
        //            public int Age;
        //            public string Gender;
        //            public string PhoneNumber;
        //            public string Email;
        //            public string SpecialRequest;
        //            public string Remarks;
        //            public bool PartyInNameOf;
        //        }

        //Begin:Added to enable multiple package selection for reservation to send the package as a list on June 6, 2015//
        public class clsPackageList
        {
            public int bookingProductId;
            public int productId;
            public int guestQuantity;
            public int discountId;
            public int transactionProfileId;
            public string productType;
            public string priceInclusive;
            public double productPrice;
            public string remarks;
            public int priceInclusiveQuantity;

        }
        //end

        //Begin:Added to include combo product id alon with category products selected
        public class clsSelectedCategoryProducts
        {
            public int parentComboProductId;
            public int productId;
            public int quantity;
            public double productPrice;
        }
        //end

        //        //Begin: Added to send the booking details as a list
        //        public class clsBookingDetails
        //        {
        //            private CustomerDTO customerDTO;
        //            public int BookingId = -1; // Added by Jeevan
        //            public string Status;
        //            public string ReservationCode;
        //            public string BookingName;
        //            public int bookingProductId;
        //            public DateTime reservationFromDateTime;
        //            public int Quantity;
        //            public string CardNumber;
        //            public int CustomerId;
        //            public string Remarks;
        //            public string User;
        //            public object ExpiryTime;
        //            public bool recurFlag;
        //            public object RecurFrequency;
        //            public object RecurUntil;
        //            public int Age;
        //            public string Channel;
        //            public int facilityId;//Added to send facility id as parameter to get Resource availablity on Dec-7-2015//
        //            public int attractionScheduleId;//Added to save  attraction id bookings table on Dec-7-2015//
        //            public int ExtraGuests; //Added to save  extra guests  on Jan-9-2017//

        //            public CustomerDTO CustomerDTO
        //            {
        //                get
        //                {
        //                    return customerDTO;
        //                }

        //                set
        //                {
        //                    customerDTO = value;
        //                    CustomerId = customerDTO.Id;
        //                }

        //            }

        //            public string FacilityName { get; set; }



        //        }
        //        //End

        //        ReservationDTO reservationDTO;

        //        class ModifierProductInformation
        //        {
        //            internal int productId;
        //            internal KeyValuePair<PurchasedProducts, int> productModifierInformation;
        //            internal Transaction.TransactionLine productTrxLine;
        //        }

        //        class AdditionalAttractionProductInfo
        //        {
        //            internal int productId;
        //            internal KeyValuePair<AttractionBooking, int> AdditionalAttractionProductInformation;
        //            internal bool processed;
        //        };
        //        /// <summary>
        //        /// Default constructor of Bookings class
        //        /// </summary>
        //        public Reservation(Semnox.Core.Utilities.Utilities ParafaitUtilitie)
        //        {
        //            log.LogMethodEntry(ParafaitUtilitie);
        //            reservationDTO = null;
        //            this.Utilities = ParafaitUtilitie;
        //            log.LogMethodExit();
        //        }

        //        ///// <summary>
        //        ///// Parmaeterized constructor of Bookings class
        //        ///// </summary>
        //        //public Reservation(int bookingId, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        //        //{
        //        //    log.LogMethodEntry(bookingId, ParafaitUtilities);
        //        //    ReservationDatahandler reservationDataHandler = new ReservationDatahandler();
        //        //    reservationDTO = reservationDataHandler.GetReservationDTO(bookingId);
        //        //    this.Utilities = ParafaitUtilities;
        //        //    log.LogMethodExit();
        //        //}

        //        public Reservation(ReservationDTO reservationDTO, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        //        {
        //            log.LogMethodEntry(reservationDTO, ParafaitUtilities);
        //            this.reservationDTO = reservationDTO;
        //            this.Utilities = ParafaitUtilities;
        //            log.LogMethodExit();
        //        }



        //        public Reservation(string pReservationCode, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        //        {
        //            log.LogMethodEntry(pReservationCode, ParafaitUtilities);

        //            Utilities = ParafaitUtilities;
        //            object o = Utilities.executeScalar("select BookingId from Bookings where ReservationCode = @code", new SqlParameter("@code", pReservationCode));
        //            if (o != null)
        //            {
        //                BookingId = Convert.ToInt32(o);
        //                constructor();
        //            }

        //            log.LogMethodExit(null);
        //        }

        //        public Reservation(int pBookingId, Semnox.Core.Utilities.Utilities ParafaitUtilities)
        //        {
        //            log.LogMethodEntry(pBookingId, ParafaitUtilities);
        //            Utilities = ParafaitUtilities;
        //            BookingId = pBookingId;
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            constructor();
        //            reservationDTO = reservationDatahandler.GetReservationDTO(pBookingId);
        //            log.LogMethodExit();
        //        }

        //        //Modified the method to change the query that gets the booking details
        //        void constructor()
        //        {
        //            log.LogMethodEntry();
        //            if (BookingId != -1)
        //            {
        //                currentCard = new Card(Utilities);

        //                DataTable dt = Utilities.executeDataTable("Select trxId, ReservationCode, CardNumber, customerId, FromDate from Bookings where BookingId = @BookingId", new SqlParameter("@BookingID", BookingId));
        //                ReservationCode = dt.Rows[0][1].ToString();
        //                object trxId = dt.Rows[0][0];
        //                CardNumber = (dt.Rows[0][2] != DBNull.Value ? dt.Rows[0][2].ToString() : "");
        //                if (dt.Rows[0][3] != DBNull.Value)
        //                    CustomerId = Convert.ToInt32(dt.Rows[0][3]);
        //                ReservationDate = Convert.ToDateTime(dt.Rows[0]["FromDate"]);
        //                if (trxId != DBNull.Value)
        //                {
        //                    TransactionUtils trxUtils = new TransactionUtils(Utilities);
        //                    Transaction = trxUtils.CreateTransactionFromDB((int)trxId, Utilities);
        //                    TaskProcs = new TaskProcs(Utilities);
        //                    if (Transaction.DiscountApplicationHistoryDTOList.Where(x => x.CouponNumber != null && x.CouponNumber.Length > 0).Count() > 0)
        //                    {
        //                        DiscountCouponCode = Transaction.DiscountApplicationHistoryDTOList.Where(x => x.CouponNumber != null && x.CouponNumber.Length > 0).First().CouponNumber;
        //                    }

        //                }
        //                //Else is removed for Blocked status bookings
        //                //else // trxId could be indeed null
        //                //{
        //                //    log.LogMethodExit();
        //                //    return;
        //                //}
        //                //Begin: Commented since Trxid will be created when a reservation is booked, will never be null
        //                //else
        //                //{
        //                //    string message = "";
        //                //    Transaction = CreateReservationTransaction(ref message);
        //                //}
        //                //end//

        //                //Modified the query to get booking details//
        //                DataTable dtPackage = Utilities.executeDataTable(@"select p.AdvanceAmount, p.AdvancePercentage , ISNULL(p.Price,0) as Price
        //                                                                    from products p, bookings b
        //                                                                    where p.product_id = b.BookingProductId 
        //                                                                    and b.BookingId = @BookingId",
        //                                                             new SqlParameter("@BookingId", BookingId));

        //                log.LogVariableState("@BookingId", BookingId);

        //                if (dtPackage.Rows.Count > 0)
        //                {
        //                    AdvanceRequired = 0;
        //                    if (dtPackage.Rows[0]["AdvanceAmount"] != DBNull.Value)
        //                        AdvanceRequired = Convert.ToDouble(dtPackage.Rows[0]["AdvanceAmount"]);
        //                    else if (dtPackage.Rows[0]["AdvancePercentage"] != DBNull.Value)
        //                    {
        //                        double amount = Convert.ToDouble(Transaction != null ? Transaction.Net_Transaction_Amount : dtPackage.Rows[0]["Price"]);
        //                        AdvanceRequired = Convert.ToDouble(dtPackage.Rows[0]["AdvancePercentage"]) * amount / 100;
        //                    }

        //                    AdvanceRequired = (Transaction != null ? Math.Min(Transaction.Net_Transaction_Amount, AdvanceRequired) : AdvanceRequired);
        //                }
        //            }
        //            log.LogMethodExit(null);
        //        }

        //        public ReservationDTO GetReservationDTO { get { return reservationDTO; } }

        //        public void Save()
        //        {
        //            log.LogMethodEntry();
        //            ExecutionContext executionUserContext = ExecutionContext.GetExecutionContext();
        //            ReservationDatahandler reservationDataHandler = new ReservationDatahandler();

        //            if (reservationDTO.BookingId < 0)
        //            {
        //                if (String.IsNullOrEmpty(reservationDTO.ReservationCode))
        //                {
        //                    reservationDTO.ReservationCode = Utilities.GenerateRandomCardNumber(5);
        //                }
        //                int bookingId = reservationDataHandler.InsertReservationDTO(reservationDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
        //                reservationDTO.BookingId = bookingId;
        //            }
        //            else
        //            {
        //                if (reservationDTO.IsChanged == true)
        //                {
        //                    reservationDataHandler.UpdateReservationDTO(reservationDTO, executionUserContext.GetUserId(), executionUserContext.GetSiteId());
        //                    reservationDTO.AcceptChanges();
        //                }
        //            }
        //        }

        //        //Modified the query to add parameter FacilityId to get attrationScheduleId on Dec-7-2015//
        //        public DataTable getResourceAvailability(DateTime FromDate, DateTime ToDate, string ResourceType, string ResourceName, int BookingProductId, int facilityId)
        //        {
        //            log.LogMethodEntry(FromDate, ToDate, ResourceType, ResourceName, BookingProductId, facilityId);

        //            string query = @"SELECT GETDATE() Date, DATENAME(DW, GETDATE()) WeekDay,
        //                       Products.product_name ProductName,
        //                       tt.TimeFrom,
        //                       tt.TimeTo,
        //                       cc.Fixed,
        //                       Products.StartDate FromDate,
        //                       Products.ExpiryDate ToDate,
        //                       Products.active_flag,
        //                       Products.MinimumTime,
        //                       Products.MaximumTime,
        //                       isnull(Products.MinimumQuantity,0)MinimumQuantity,
        //                       Products.product_id,
        //	                   cc.attractionScheduleId,
        //                       CASE
        //                           WHEN ISNULL(Quantity, 0) = 0 THEN CASE
        //                                                                 WHEN ISNULL(Products.AvailableUnits, 0) = 0 THEN ISNULL(cc.Capacity,0)
        //                                                                 ELSE ISNULL(Products.AvailableUnits,0)
        //                                                             END
        //                           ELSE ISNULL(Quantity, 0)
        //                       END Quantity
        //                                FROM products,

        //                                  (SELECT aa.Quantity,
        //                                          aa.AttractionScheduleId,
        //                                          aa.Fixed,
        //                                          aa.Capacity
        //                                   FROM
        //                                     (SELECT ats.AttractionScheduleId,
        //                                         (SELECT top 1 Units
        //                                           FROM AttractionScheduleRules,

        //                                             (SELECT dateadd([mi], convert(int, ats.ScheduleTime)*60 + ats.ScheduleTime%1*100, @fromDate) schDate) v
        //                                           WHERE (v.schDate BETWEEN FromDate AND ToDate + 1
        //                                                  OR DATEPART(weekday, v.schDate) = DAY + 1
        //                                                  OR DATEPART(DAY, v.schDate) = DAY - 1000)
        //                                             AND AttractionScheduleId = ats.AttractionScheduleId
        //                                             AND ProductId = @BookingProductId) Quantity,
        //                                                                                   ats.Fixed,
        //                                                                                   cf.Capacity
        //                                      FROM AttractionSchedules ats ,
        //                                           CheckInFacility cf
        //                                      WHERE cf.FacilityId = ats.FacilityId
        //                                        AND ats.AttractionScheduleId = (
        //                                                                          (SELECT AttractionScheduleId
        //                                                                           FROM AttractionSchedules
        //                                                                           INNER JOIN products p ON p.product_id = @BookingProductId
        //                                                                           AND p.AttractionMasterScheduleId = AttractionSchedules.AttractionMasterScheduleId
        //                                                                           WHERE
        //                                                                           FacilityId = @facilityId
        //                                                                           AND @reservation >= DATEADD([mi], CONVERT(int, ScheduleTime) * 60 + ScheduleTime % 1 * 100, @fromDate) 
        //                                                                           AND  @reservation < CASE when ScheduleTime > ScheduleToTime
        //																				                    then DATEADD(DAY, 1, DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 100, @fromDate))
        //                                                                                                    else DATEADD([mi], CONVERT(int, isnull(ScheduleToTime,23.59)) * 60 + isnull(ScheduleToTime,23.59) % 1 * 100, @fromDate)
        //                                                                                                     end ) ))aa) cc,
        //                                  (SELECT TimeFrom,
        //                                          TimeTo
        //                                   FROM
        //                                     (SELECT MIN(DATEADD([mi], CONVERT(int, ats.ScheduleTime) * 60 + ats.ScheduleTime % 1 * 100, @fromDate)) TimeFrom,
        //                                            MAX(CASE WHEN ats.ScheduleTime is not null and ats.ScheduleToTime is not null and  ats.ScheduleTime > ats.ScheduleToTime 
        //                                                     THEN (DATEADD(DAY,1, DATEADD([mi], CONVERT(int, isnull(ats.ScheduleToTime,23.59)) * 60 + isnull(ats.ScheduleToTime,23.59) % 1 * 100, @fromDate)))
        //										             ELSE (DATEADD([mi], CONVERT(int, isnull(ats.ScheduleToTime,23.59)) * 60 + isnull(ats.ScheduleToTime,23.59) % 1 * 100, @fromDate)) 
        //                                                      END) TimeTo
        //                                      FROM AttractionSchedules ats,
        //                                           AttractionMasterSchedule ams,
        //                                           Products p
        //                                      WHERE ams.AttractionMasterScheduleId = ats.AttractionMasterScheduleId
        //                                        AND p.AttractionMasterScheduleId = ams.AttractionMasterScheduleId
        //                                        AND ats.AttractionScheduleId = AttractionScheduleId
        //                                        AND p.product_id = @BookingProductId ) t) tt
        //                                WHERE product_id = @BookingProductId
        //                                  AND active_flag ='Y'";

        //            object lclResourceName;
        //            if (string.IsNullOrEmpty(ResourceName))
        //                lclResourceName = DBNull.Value;
        //            else
        //                lclResourceName = ResourceName;

        //            object lclResourceType;
        //            if (string.IsNullOrEmpty(ResourceType))
        //                lclResourceType = DBNull.Value;
        //            else
        //                lclResourceType = ResourceType;

        //            DataTable dt = Utilities.executeDataTable(query, new SqlParameter[] { new SqlParameter("@fromDate", FromDate.Date),
        //                                                                                  new SqlParameter("@toDate", ToDate.Date),
        //                                                                                  new SqlParameter("@BookingProductId", BookingProductId),
        //                                                                                  new SqlParameter("@resourceType", lclResourceType),
        //                                                                                  new SqlParameter("@resourceName", lclResourceName),
        //                                                                                  new SqlParameter("@reservation", FromDate.AddSeconds(OffSetDuration)),
        //                                                                                  new SqlParameter("@facilityId", facilityId),
        //                                                                                  new SqlParameter("@fromDateAdd", FromDate.Date.AddDays(1).AddSeconds(OffSetDuration))});
        //            return dt;
        //        }
        //        //End//


        //        //Begin:Modified the query to get the bookingProducts//
        //        //Modified the method to add facilityId on Dec-7-2015//
        //        public bool ValidateRequest(int bookingProductId, DateTime ReservationFromDateTime,
        //                                    int Duration, int Quantity, int facilityId, ref string Message)
        //        {
        //            log.LogMethodEntry(bookingProductId, ReservationFromDateTime, Duration, Quantity, facilityId, Message);
        //            //Commented the query to implement new Booking changes
        //            //DataTable dtPackage = Utilities.executeDataTable("select * from BookingClassProducts where PackageId = @PackageId",
        //            //                                                 new SqlParameter("@PackageId", PackageId));
        //            DataTable dtBookingProduct = Utilities.executeDataTable("select * from products where product_id = @BookingProductId",
        //                                                             new SqlParameter("@BookingProductId", bookingProductId));

        //            log.LogVariableState("@BookingProductId", bookingProductId);

        //            int BookingProductId = (int)dtBookingProduct.Rows[0]["product_id"];
        //            DataTable dt = getResourceAvailability(ReservationFromDateTime, ReservationFromDateTime, null, null, BookingProductId, facilityId);

        //            // check if valid date and valid from and to time
        //            bool slotFound = false;

        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                DateTime fromDate, toDate;
        //                string Fixed = dr["Fixed"].ToString();
        //                if (dr["FromDate"] != DBNull.Value)
        //                    fromDate = Convert.ToDateTime(dr["FromDate"]);
        //                else
        //                    fromDate = DateTime.MinValue;

        //                if (dr["ToDate"] != DBNull.Value)
        //                    toDate = Convert.ToDateTime(dr["ToDate"]);
        //                else
        //                    toDate = DateTime.MaxValue;

        //                if (ReservationFromDateTime >= fromDate && ReservationFromDateTime <= toDate)
        //                {
        //                    double fromTime, toTime;
        //                    if (dr["TimeFrom"] != DBNull.Value)
        //                    {
        //                        fromTime = Convert.ToDateTime(dr["TimeFrom"]).Hour * 60 + Convert.ToDateTime(dr["TimeFrom"]).Minute;
        //                        fromTime = fromTime - (OffSetDuration / 60); // Modified for OffSet
        //                    }
        //                    else
        //                        fromTime = 0;

        //                    if (dr["TimeTo"] != DBNull.Value)
        //                    {
        //                        toTime = Convert.ToDateTime(dr["TimeTo"]).Hour * 60 + Convert.ToDateTime(dr["TimeTo"]).Minute;
        //                        toTime = toTime - (OffSetDuration / 60); // Modified for OffSet
        //                    }
        //                    else
        //                    {
        //                        toTime = 24 * 60;
        //                    }

        //                    if (toTime <= fromTime)
        //                        toTime += 24 * 60;

        //                    DateTime ReservationDate = ReservationFromDateTime.Date;
        //                    DateTime ReservationToDateTime = ReservationFromDateTime.AddMinutes(Duration);
        //                    if (ReservationFromDateTime >= ReservationDate.AddMinutes(fromTime) && ReservationFromDateTime <= ReservationDate.AddMinutes(toTime)
        //                        && ReservationToDateTime >= ReservationDate.AddMinutes(fromTime) && ReservationToDateTime <= ReservationDate.AddMinutes(toTime))
        //                    {
        //                        if (Fixed == "N")
        //                        {
        //                            if (dtBookingProduct.Rows[0]["MinimumTime"] != DBNull.Value)
        //                            {
        //                                if (Duration < Convert.ToInt32(dtBookingProduct.Rows[0]["MinimumTime"]))
        //                                {
        //                                    Message = Utilities.MessageUtils.getMessage(324, dtBookingProduct.Rows[0]["MinimumTime"]);

        //                                    log.LogVariableState("Message", Message);
        //                                    log.LogMethodExit(false);
        //                                    return false;
        //                                }
        //                            }
        //                            else if (dr["MinimumTime"] != DBNull.Value && Duration < Convert.ToInt32(dr["MinimumTime"]))
        //                            {
        //                                Message = Utilities.MessageUtils.getMessage(324, dr["MinimumTime"]);

        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }

        //                            if (dr["MaximumTime"] != DBNull.Value && Duration > Convert.ToInt32(dr["MaximumTime"]))
        //                            {
        //                                Message = Utilities.MessageUtils.getMessage(325, dr["MaximumTime"]);
        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }
        //                        }
        //                        int resQty = Convert.ToInt32(Utilities.executeScalar(@"select isnull(sum(quantity), 0) quantityBooked
        //                                                                                 from Bookings b,
        //                                                                                      AttractionSchedules ats
        //                                                                                where BookingProductId = @bookingProductId
        //                                                                                and BookingId != @BookingId
        //                                                                                and b.attractionScheduleId=ats.attractionScheduleId
        //                                                                                and ats.facilityId = @facilityId
        //                                                                                and ((@TimeFrom < FromDate and @TimeTo > ToDate)
        //                                                                                    or (@TimeFrom >= FromDate and @TimeFrom < ToDate)
        //                                                                                    or (@TimeTo > FromDate and @TimeTo <= ToDate))
        //                                                                                and ((status ='BOOKED' and (ExpiryTime is null or ExpiryTime > getdate()))
        //                                                                                or (status in ('WIP','BLOCKED') and (ExpiryTime is null or ExpiryTime > getdate()))
        //                                                                                    or status in ('COMPLETE', 'CONFIRMED'))",
        //                                                                new SqlParameter[] {new SqlParameter("@bookingProductId", bookingProductId),
        //                                                                                    new SqlParameter("@BookingId", BookingId),
        //                                                                                    new SqlParameter("@facilityId", facilityId),
        //                                                                                    new SqlParameter("@TimeFrom", ReservationFromDateTime),
        //                                                                                    new SqlParameter("@TimeTo", ReservationToDateTime)}));

        //                        log.LogVariableState("@bookingProductId", bookingProductId);
        //                        log.LogVariableState("@BookingId", BookingId);
        //                        log.LogVariableState("@facilityId", facilityId);
        //                        log.LogVariableState("@TimeFrom", ReservationFromDateTime);
        //                        log.LogVariableState("@TimeTo", ReservationToDateTime);

        //                        int maxQty = Convert.ToInt32(dr["quantity"]);
        //                        //Begin: Commneted the Condition on Dec-7-2015 to remove check for Units returned from getResource Availablity method//
        //                        //if (maxQty > 0)
        //                        //{
        //                        if (resQty + Quantity > maxQty)
        //                        {
        //                            Message = Utilities.MessageUtils.getMessage(326, Quantity, (maxQty - resQty));

        //                            log.LogVariableState("Message", Message);
        //                            log.LogMethodExit(false);
        //                            return false;
        //                        }
        //                        //}
        //                        //End: Commneted the Condition on Dec-7-2015 to remove check for Units returned from getResource Availablity method//

        //                        if (dr["MinimumQuantity"] != DBNull.Value)
        //                        {
        //                            int minQty = Convert.ToInt32(dr["MinimumQuantity"]);
        //                            if (minQty > 0 && Quantity < minQty)
        //                            {
        //                                Message = "Minimum quantity should be " + minQty.ToString();
        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }
        //                        }

        //                        //added 10-Oct
        //                        int bookedFacilityQty = getFacilityBookings(BookingId, Convert.ToInt32(dr["AttractionScheduleId"]), ReservationFromDateTime, ReservationToDateTime);
        //                        if (bookedFacilityQty > 0)
        //                        {
        //                            if (Utilities.getParafaitDefaults("ALLOW_MULTIPLE_BOOKINGS_WITHIN_SCHEDULE").Equals("N"))
        //                            {
        //                                Message = Utilities.MessageUtils.getMessage(1254);
        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }

        //                            int maxFacilityQty = Convert.ToInt32(Utilities.executeScalar(@"SELECT ISNULL((SELECT isnull(capacity, -1)
        //                                                                                                            FROM CheckInFacility
        //                                                                                                           WHERE FacilityId = @FacilityId
        //                                                                                                             AND active_flag = 'Y')
        //                                                                                                          ,-1)",
        //                                                                new SqlParameter[] { new SqlParameter("@facilityId", facilityId) }));

        //                            log.LogVariableState("@facilityId", facilityId);

        //                            if (maxFacilityQty >= 0)
        //                            {
        //                                if (bookedFacilityQty + Quantity > maxFacilityQty)
        //                                {
        //                                    Message = Utilities.MessageUtils.getMessage(1253, Quantity, (maxFacilityQty - bookedFacilityQty), maxFacilityQty);
        //                                    log.LogVariableState("Message", Message);
        //                                    log.LogMethodExit(false);
        //                                    return false;
        //                                }
        //                            }
        //                        }
        //                        //end 10-Oct
        //                        slotFound = true;
        //                        break;
        //                    }
        //                }
        //            }

        //            if (!slotFound)
        //            {
        //                Message = Utilities.MessageUtils.getMessage(327);
        //                log.LogVariableState("Message", Message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }
        //            log.LogVariableState("Message", Message);
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //        //End//

        //        /*Begin Modification-Dec-30-2015 -Added a new constructor for make reservation to send credit card, debit card , payment reference and payment card number from web transaction. When called from POS credit card, debit card will be 0 and payment reference and payment card number will be blank*/
        //        //Called from POS//
        //        public bool MakeReservation(List<Reservation.clsBookingDetails> bookingDetails, double bookingProductPrice,
        //                                    bool isEditedBooking, int editedBookingId,
        //                                    List<clsPackageList> lstPackageList, int Duration,
        //                                    List<clsProductList> lstBookingProducts, List<string> Cards,
        //                                    List<Reservation.clsSelectedCategoryProducts> CategoryProducts,
        //                                    List<DiscountsDTO> discounts,
        //                                    List<KeyValuePair<AttractionBooking, int>> lstAttractionProductList,
        //                                    List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductsList,
        //                                    ref string Message,
        //                                    List<KeyValuePair<PurchasedProducts, int>> purchasedProductSelected,
        //                                    List<KeyValuePair<PurchasedProducts, int>> purchasedAdditionalProduct)//Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //        {
        //            log.LogMethodEntry(bookingDetails, bookingProductPrice, isEditedBooking, editedBookingId, lstPackageList, Duration, lstBookingProducts, Cards,
        //                                    CategoryProducts, discounts, lstAttractionProductList, lstAdditionalAttractionProductsList, Message, purchasedProductSelected, purchasedAdditionalProduct);

        //            log.LogVariableState("Message", Message);

        //            bool returnValue = MakeReservation(bookingDetails, bookingProductPrice, isEditedBooking, editedBookingId, lstPackageList, Duration,
        //                                    lstBookingProducts, Cards, CategoryProducts, discounts, 0, 0, "", "", lstAttractionProductList,
        //                                    lstAdditionalAttractionProductsList, ref Message,
        //                                    purchasedProductSelected, purchasedAdditionalProduct); //Modification for F&B Restructuring of modifiers on 17-Oct-2018

        //            log.LogMethodExit(returnValue);

        //            return returnValue;
        //        }
        //        //End Modification-Dec-30-2015 -Added a new constructor for make reservation to send credit card, debit card , payment reference and payment card number from web transaction//

        //        //Begin Modification-Jan-20-2016- Constructor written for Website in order to pass Null for Attraction object//
        //        public bool MakeReservation(List<Reservation.clsBookingDetails> bookingDetails, double bookingProductPrice,
        //                                    bool isEditedBooking, int editedBookingId,
        //                                    List<clsPackageList> lstPackageList, int Duration,
        //                                    List<clsProductList> lstBookingProducts, List<string> Cards,
        //                                    List<Reservation.clsSelectedCategoryProducts> CategoryProducts,
        //                                    List<DiscountsDTO> discounts,
        //                                    double creditCardAmount, double debitCardAmount,
        //                                    string paymentCardNumber, string PaymentReference, ref string Message)
        //        {
        //            log.LogMethodEntry(bookingDetails, bookingProductPrice, isEditedBooking, editedBookingId,
        //                lstPackageList, Duration, lstBookingProducts, Cards, CategoryProducts, discounts,
        //                creditCardAmount, debitCardAmount, paymentCardNumber, PaymentReference, Message
        //                );

        //            bool returnValue = MakeReservation(bookingDetails, bookingProductPrice, isEditedBooking, editedBookingId, lstPackageList, Duration,
        //                                    lstBookingProducts, Cards, CategoryProducts, discounts, creditCardAmount, debitCardAmount, paymentCardNumber, PaymentReference,
        //                                    null, null, ref Message, null, null);
        //            log.LogMethodExit(returnValue);

        //            return returnValue;
        //        }
        //        //End Modification-Jan-20-2016- Constructor written for Website in order to pass Null for Attraction object//

        //        //Begin Booking Related changes.Changed the parameter to List<clsPackageList> lstPackageList to get the package list on June 5, 2015//
        //        //Added parameters BookingProductId,bookingProductPrice,isEditedBooking, editedBookingId, Cards,CategoryProducts
        //        /*List of CategoryProducts: Products which belong to the category selected by the combo product, List of cards : If card products were selected 
        //         bookingDetails is a list of all booking related parameters on 05-Sept-2015*/
        //        //Called from Website//
        //        public bool MakeReservation(List<Reservation.clsBookingDetails> bookingDetails, double bookingProductPrice,
        //                                    bool isEditedBooking, int editedBookingId,
        //                                    List<clsPackageList> lstPackageList, int Duration,
        //                                    List<clsProductList> lstBookingProducts, List<string> Cards,
        //                                    List<Reservation.clsSelectedCategoryProducts> CategoryProducts,
        //                                    List<DiscountsDTO> discounts,
        //                                    double creditCardAmount, double debitCardAmount,
        //                                    string paymentCardNumber, string PaymentReference,
        //                                    List<KeyValuePair<AttractionBooking, int>> lstAttractionProductList,
        //                                    List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductsList, ref string Message,
        //                                    List<KeyValuePair<PurchasedProducts, int>> purchasedProductSelected,
        //                                    List<KeyValuePair<PurchasedProducts, int>> purchasedAdditionalProduct)//Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //        {
        //            log.LogMethodEntry(bookingDetails, bookingProductPrice, isEditedBooking, editedBookingId, lstPackageList, Duration,
        //                                           lstBookingProducts, Cards, CategoryProducts, discounts, creditCardAmount, debitCardAmount,
        //                                           paymentCardNumber, PaymentReference, lstAttractionProductList, lstAdditionalAttractionProductsList, Message,
        //                                           purchasedProductSelected, purchasedAdditionalProduct);

        //            this.purchasedProductSelected = purchasedProductSelected;
        //            this.purchasedAdditionalProduct = purchasedAdditionalProduct;
        //            audit = new EventLog(Utilities);
        //            DataTable dtPackage = null;
        //            if (!ValidateRequest(bookingDetails[0].bookingProductId, bookingDetails[0].reservationFromDateTime, Duration, bookingDetails[0].Quantity, bookingDetails[0].facilityId, ref Message))
        //            {
        //                log.LogVariableState("Message", Message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }

        //            if (BookingId == -1)
        //            {
        //                if (bookingDetails[0].BookingId != -1)
        //                {
        //                    ReservationCode = bookingDetails[0].ReservationCode;
        //                }
        //                else
        //                {
        //                    ReservationCode = Utilities.GenerateRandomCardNumber(5);
        //                }
        //            }

        //            Card card = null;
        //            object lclCardId = DBNull.Value;
        //            if (bookingDetails[0].CardNumber.Trim() != "")
        //                card = new Card(bookingDetails[0].CardNumber.Trim(), bookingDetails[0].User, Utilities);//Changed the parameter cardnumber to list

        //            object lclCustomerId;
        //            if (bookingDetails[0].CustomerId == -1)
        //                lclCustomerId = DBNull.Value;
        //            else
        //                lclCustomerId = bookingDetails[0].CustomerId;

        //            SqlConnection cnn = Utilities.createConnection();
        //            SqlTransaction SQLTrx = cnn.BeginTransaction();

        //            CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, bookingDetails[0].CustomerDTO);
        //            if (Utilities.ParafaitEnv.IsCorporate && bookingDetails[0].CustomerDTO.SiteId <= 0)
        //            {
        //                bookingDetails[0].CustomerDTO.SiteId = Utilities.ParafaitEnv.SiteId;
        //            }

        //            customerBL.Save(SQLTrx);
        //            CustomerId = bookingDetails[0].CustomerDTO.Id;
        //            lclCustomerId = bookingDetails[0].CustomerDTO.Id;

        //            //End Modification-Dec-30-2015 - Added new parameters to customerDTO//

        //            if (card != null)
        //            {
        //                if (card.CardStatus == "NEW")
        //                {
        //                    card.customer_id = CustomerId;
        //                    card.createCard(SQLTrx);
        //                }
        //                lclCardId = card.card_id;
        //            }

        //            bool newBooking = false;
        //            try
        //            {
        //                //Begin Modification- Jan-20-2017- Added field ExtraGuests to Bookings and update for site_id //

        //                if (Utilities.ParafaitEnv.IsCorporate == false || Utilities.ParafaitEnv.SiteId <= 0)
        //                    site_id = DBNull.Value;
        //                else
        //                    site_id = Utilities.ParafaitEnv.SiteId;

        //                ReservationDate = bookingDetails[0].reservationFromDateTime;  // Added on Feb-14-2018 for the updation of Reservation FromDate

        //                if (BookingId == -1 && bookingDetails[0].BookingId != -1)
        //                {
        //                    newBooking = true;
        //                    BookingId = bookingDetails[0].BookingId;
        //                }
        //                if (BookingId == -1)
        //                {
        //                    newBooking = true;
        //                    //Added an additional field BookingProductId//
        //                    object id = Utilities.executeScalar(@"Insert into Bookings 
        //                                                           ([BookingName]
        //                                                           ,[FromDate]
        //                                                           ,[ToDate]
        //                                                           ,[Quantity]
        //                                                           ,[ReservationCode]
        //                                                           ,[Status]
        //                                                           ,[CardId]
        //                                                           ,[CardNumber]
        //                                                           ,[CustomerId]
        //                                                           ,[recur_flag]
        //                                                           ,[recur_frequency]
        //                                                           ,[recur_end_date]
        //                                                           ,[isEmailSent]
        //                                                           ,[ExpiryTime]
        //                                                           ,[Remarks]
        //                                                           ,[CreatedBy]
        //                                                           ,[CreationDate]
        //                                                           ,[LastUpdatedBy]
        //                                                           ,[LastUpdatedDate]
        //                                                           ,[Age]
        //                                                           ,[BookingProductId]
        //                                                           ,[AttractionScheduleId]
        //                                                           ,[ExtraGuests]
        //                                                           ,[site_id]
        //                                                            ,[Channel]
        //                                                            )
        //                                                    values 
        //                                                           (
        //                                                            @BookingName 
        //                                                           ,@FromDate
        //                                                           ,@ToDate
        //                                                           ,@Quantity
        //                                                           ,@ReservationCode
        //                                                           ,'BOOKED'
        //                                                           ,@CardId
        //                                                           ,@CardNumber
        //                                                           ,@CustomerId
        //                                                           ,@recur_flag
        //                                                           ,@recur_frequency
        //                                                           ,@recur_end_date
        //                                                           ,0
        //                                                           ,@ExpiryTime
        //                                                           ,@Remarks
        //                                                           ,@CreatedBy
        //                                                           ,getdate()
        //                                                           ,@LastUpdatedBy
        //                                                           ,getdate()
        //                                                           ,@Age
        //                                                           ,@BookingProductId
        //                                                           ,@AttractionScheduleId
        //                                                           ,@ExtraGuests
        //                                                           ,@site_id
        //                                                           ,@Channel

        //                                                            ); select @@identity", SQLTrx,

        //                                                new SqlParameter("@BookingName", bookingDetails[0].BookingName),
        //                                                new SqlParameter("@FromDate", bookingDetails[0].reservationFromDateTime),
        //                                                new SqlParameter("@ToDate", bookingDetails[0].reservationFromDateTime.AddMinutes(Duration)),
        //                                                new SqlParameter("@Quantity", bookingDetails[0].Quantity),
        //                                                new SqlParameter("@ReservationCode", ReservationCode),
        //                                                new SqlParameter("@CardId", lclCardId),
        //                                                new SqlParameter("@CardNumber", bookingDetails[0].CardNumber.Trim() == "" ? DBNull.Value : (object)bookingDetails[0].CardNumber.Trim()),
        //                                                new SqlParameter("@CustomerId", lclCustomerId),
        //                                                new SqlParameter("@recur_flag", bookingDetails[0].recurFlag ? 'Y' : 'N'),
        //                                                new SqlParameter("@recur_frequency", bookingDetails[0].recurFlag ? bookingDetails[0].RecurFrequency : DBNull.Value),
        //                                                new SqlParameter("@recur_end_date", bookingDetails[0].recurFlag ? bookingDetails[0].RecurUntil : DBNull.Value),
        //                                                new SqlParameter("@ExpiryTime", bookingDetails[0].ExpiryTime),
        //                                                new SqlParameter("@Remarks", bookingDetails[0].Remarks),
        //                                                new SqlParameter("@CreatedBy", bookingDetails[0].User),
        //                                                new SqlParameter("@LastUpdatedBy", bookingDetails[0].User),
        //                                                new SqlParameter("@Age", bookingDetails[0].Age),
        //                                                new SqlParameter("@Channel", bookingDetails[0].Channel),
        //                                                new SqlParameter("@BookingProductId", bookingDetails[0].bookingProductId),
        //                                                new SqlParameter("@AttractionScheduleId", bookingDetails[0].attractionScheduleId),
        //                                                new SqlParameter("@ExtraGuests", bookingDetails[0].ExtraGuests),
        //                                                new SqlParameter("@site_id", site_id)
        //                                                );


        //                    log.LogVariableState("@BookingName", bookingDetails[0].BookingName);
        //                    log.LogVariableState("@FromDate", bookingDetails[0].reservationFromDateTime);
        //                    log.LogVariableState("@ToDate", bookingDetails[0].reservationFromDateTime.AddMinutes(Duration));
        //                    log.LogVariableState("@Quantity", bookingDetails[0].Quantity);
        //                    log.LogVariableState("@ReservationCode", ReservationCode);
        //                    log.LogVariableState("@CardId", lclCardId);
        //                    log.LogVariableState("@CardNumber", bookingDetails[0].CardNumber.Trim() == "" ? DBNull.Value : (object)bookingDetails[0].CardNumber.Trim());
        //                    log.LogVariableState("@CustomerId", lclCustomerId);
        //                    log.LogVariableState("@recur_flag", bookingDetails[0].recurFlag ? 'Y' : 'N');
        //                    log.LogVariableState("@recur_frequency", bookingDetails[0].recurFlag ? bookingDetails[0].RecurFrequency : DBNull.Value);
        //                    log.LogVariableState("@recur_end_date", bookingDetails[0].recurFlag ? bookingDetails[0].RecurUntil : DBNull.Value);
        //                    log.LogVariableState("@ExpiryTime", bookingDetails[0].ExpiryTime);
        //                    log.LogVariableState("@Remarks", bookingDetails[0].Remarks);
        //                    log.LogVariableState("@CreatedBy", bookingDetails[0].User);
        //                    log.LogVariableState("@LastUpdatedBy", bookingDetails[0].User);
        //                    log.LogVariableState("@Age", bookingDetails[0].Age);
        //                    log.LogVariableState("@BookingProductId", bookingDetails[0].bookingProductId);
        //                    log.LogVariableState("@AttractionScheduleId", bookingDetails[0].attractionScheduleId);
        //                    log.LogVariableState("@ExtraGuests", bookingDetails[0].ExtraGuests);
        //                    log.LogVariableState("@site_id", site_id);
        //                    log.LogVariableState("@Channel", bookingDetails[0].Channel);

        //                    BookingId = Convert.ToInt32(id);
        //                    Status = ReservationDTO.ReservationStatus.BOOKED.ToString();//Changed the status to "WIP" to fetch the Advance amount before booking for online reservation on May 13, 2015//
        //                }

        //                else
        //                {
        //                    Utilities.executeScalar(@"update Bookings set 
        //                                                            [BookingName] = @BookingName
        //                                                           ,[FromDate] = @FromDate
        //                                                           ,[ToDate] = @ToDate
        //                                                           ,[Quantity] = @Quantity
        //                                                           ,[CardId] = @CardId
        //                                                           ,[CardNumber] = @CardNumber
        //                                                           ,[recur_flag] = @recur_flag
        //                                                           ,[recur_frequency] = @recur_frequency
        //                                                           ,[recur_end_date] = @recur_end_date
        //                                                           ,[Remarks] = @Remarks
        //                                                           ,[Status] = 'BOOKED'
        //                                                           ,[LastUpdatedBy] = @LastUpdatedBy
        //                                                           ,[LastUpdatedDate] = getdate()
        //                                                           ,[Age] = @Age
        //                                                           ,[BookingProductId] = @BookingProductId
        //                                                           ,[AttractionScheduleId] = @AttractionScheduleId
        //                                                           ,[ExpiryTime] = @ExpiryTime
        //                                                           ,[ExtraGuests] = @ExtraGuests
        //                                                            where BookingId = @BookingId", SQLTrx,

        //                                                new SqlParameter("@BookingName", bookingDetails[0].BookingName),
        //                                                new SqlParameter("@FromDate", bookingDetails[0].reservationFromDateTime),
        //                                                new SqlParameter("@ToDate", bookingDetails[0].reservationFromDateTime.AddMinutes(Duration)),
        //                                                new SqlParameter("@Quantity", bookingDetails[0].Quantity),
        //                                                new SqlParameter("@CardId", lclCardId),
        //                                                new SqlParameter("@CardNumber", bookingDetails[0].CardNumber.Trim() == "" ? DBNull.Value : (object)bookingDetails[0].CardNumber.Trim()),
        //                                                new SqlParameter("@recur_flag", bookingDetails[0].recurFlag ? 'Y' : 'N'),
        //                                                new SqlParameter("@recur_frequency", bookingDetails[0].recurFlag ? bookingDetails[0].RecurFrequency : DBNull.Value),
        //                                                new SqlParameter("@recur_end_date", bookingDetails[0].recurFlag ? bookingDetails[0].RecurUntil : DBNull.Value),
        //                                                new SqlParameter("@Remarks", bookingDetails[0].Remarks),
        //                                                new SqlParameter("@LastUpdatedBy", bookingDetails[0].User),
        //                                                new SqlParameter("@Age", bookingDetails[0].Age),
        //                                                new SqlParameter("@BookingProductId", bookingDetails[0].bookingProductId),
        //                                                new SqlParameter("@AttractionScheduleId", bookingDetails[0].attractionScheduleId),
        //                                                new SqlParameter("@ExpiryTime", DBNull.Value),
        //                                                new SqlParameter("@ExtraGuests", bookingDetails[0].ExtraGuests),
        //                                                new SqlParameter("@BookingId", BookingId));

        //                    log.LogVariableState("@BookingName", bookingDetails[0].BookingName);
        //                    log.LogVariableState("@FromDate", bookingDetails[0].reservationFromDateTime);
        //                    log.LogVariableState("@ToDate", bookingDetails[0].reservationFromDateTime.AddMinutes(Duration));
        //                    log.LogVariableState("@Quantity", bookingDetails[0].Quantity);
        //                    log.LogVariableState("@CardId", lclCardId);
        //                    log.LogVariableState("@CardNumber", bookingDetails[0].CardNumber.Trim() == "" ? DBNull.Value : (object)bookingDetails[0].CardNumber.Trim());
        //                    log.LogVariableState("@recur_flag", bookingDetails[0].recurFlag ? 'Y' : 'N');
        //                    log.LogVariableState("@recur_frequency", bookingDetails[0].recurFlag ? bookingDetails[0].RecurFrequency : DBNull.Value);
        //                    log.LogVariableState("@recur_end_date", bookingDetails[0].recurFlag ? bookingDetails[0].RecurUntil : DBNull.Value);
        //                    log.LogVariableState("@Remarks", bookingDetails[0].Remarks);
        //                    log.LogVariableState("@LastUpdatedBy", bookingDetails[0].User);
        //                    log.LogVariableState("@Age", bookingDetails[0].Age);
        //                    log.LogVariableState("@BookingProductId", bookingDetails[0].bookingProductId);
        //                    log.LogVariableState("@AttractionScheduleId", bookingDetails[0].attractionScheduleId);
        //                    log.LogVariableState("@ExpiryTime", DBNull.Value);
        //                    log.LogVariableState("@ExtraGuests", bookingDetails[0].ExtraGuests);
        //                    log.LogVariableState("@BookingId", BookingId);

        //                    Status = ReservationDTO.ReservationStatus.BOOKED.ToString();
        //                }
        //                //ENDS Modification- Jan-20-2017- Added field ExtraGuests to Bookings and update for site_id //



        //                //Begin: Save the Discount selected by the products

        //                ReservationDiscountsListBL reservationDiscountsListBL = new ReservationDiscountsListBL();
        //                List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchReservationDiscountsParams = new List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>>();
        //                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, BookingId.ToString()));
        //                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
        //                List<ReservationDiscountsDTO> existingReservationDiscountsDTOList = reservationDiscountsListBL.GetReservationDiscountsDTOList(searchReservationDiscountsParams);
        //                if (existingReservationDiscountsDTOList != null)
        //                {
        //                    foreach (var reservationDiscountsDTO in existingReservationDiscountsDTOList)
        //                    {
        //                        ReservationDiscountsBL reservationDiscountsBL = new ReservationDiscountsBL(reservationDiscountsDTO);
        //                        reservationDiscountsBL.Delete();
        //                    }
        //                }

        //                Dictionary<int, ReservationDiscountsDTO> reservationDiscountsDTODictionary = new Dictionary<int, ReservationDiscountsDTO>();
        //                foreach (var discountsDTO in discounts)
        //                {
        //                    if (reservationDiscountsDTODictionary.ContainsKey(discountsDTO.DiscountId) == false)
        //                    {
        //                        ReservationDiscountsDTO reservationDiscountsDTO = new ReservationDiscountsDTO();
        //                        reservationDiscountsDTO.BookingId = BookingId;
        //                        reservationDiscountsDTO.ReservationDiscountId = discountsDTO.DiscountId;
        //                        reservationDiscountsDTO.ReservationDiscountPecentage = discountsDTO.DiscountPercentage;
        //                        reservationDiscountsDTO.ReservationDiscountCategory = "Transaction Discount";
        //                        reservationDiscountsDTODictionary.Add(discountsDTO.DiscountId, reservationDiscountsDTO);
        //                    }
        //                    audit.logEvent("Reservation", 'D', bookingDetails[0].User, discountsDTO.DiscountName + " -Discount is applied", "Reservation Discounts Screen", 0, "", BookingId.ToString(), null);
        //                }

        //                for (int j = 0; j < lstPackageList.Count; j++)
        //                {

        //                    if (lstPackageList[j].discountId >= 1)
        //                    {
        //                        if (reservationDiscountsDTODictionary.ContainsKey(lstPackageList[j].discountId) == false)
        //                        {
        //                            ReservationDiscountsDTO reservationDiscountsDTO = new ReservationDiscountsDTO();
        //                            reservationDiscountsDTO.BookingId = BookingId;
        //                            reservationDiscountsDTO.ReservationDiscountId = lstPackageList[j].discountId;
        //                            reservationDiscountsDTO.ReservationDiscountCategory = "Package Discount";
        //                            reservationDiscountsDTO.ProductId = lstPackageList[j].productId;
        //                            reservationDiscountsDTODictionary.Add(lstPackageList[j].discountId, reservationDiscountsDTO);
        //                        }
        //                    }

        //                }
        //                //End//
        //                //Begin: Save the Discount selected by the additional products
        //                foreach (clsProductList bookingproducts in lstBookingProducts)
        //                {
        //                    if (bookingproducts.discountId >= 1)
        //                    {
        //                        if (reservationDiscountsDTODictionary.ContainsKey(bookingproducts.discountId) == false)
        //                        {
        //                            ReservationDiscountsDTO reservationDiscountsDTO = new ReservationDiscountsDTO();
        //                            reservationDiscountsDTO.BookingId = BookingId;
        //                            reservationDiscountsDTO.ReservationDiscountId = bookingproducts.discountId;
        //                            reservationDiscountsDTO.ReservationDiscountCategory = "Additional Product Discount";
        //                            reservationDiscountsDTO.ProductId = bookingproducts.ProductId;
        //                            reservationDiscountsDTODictionary.Add(bookingproducts.discountId, reservationDiscountsDTO);
        //                        }
        //                    }
        //                }
        //                //end

        //                foreach (var item in reservationDiscountsDTODictionary)
        //                {
        //                    ReservationDiscountsBL reservationDiscountsBL = new ReservationDiscountsBL(item.Value);
        //                    reservationDiscountsBL.Save();
        //                }


        //                if (isEditedBooking)
        //                    Transaction = null;
        //                if (Transaction == null || Transaction.TotalPaidAmount == 0)
        //                {
        //                    //Create the transaction
        //                    //Begin Modification -Jan-20-2016 -Modified the method to pass List of Attraction Object //
        //                    Transaction = CreateReservationTransaction(bookingDetails[0].bookingProductId,
        //                                                                bookingProductPrice, lstPackageList, lstBookingProducts, Cards, CategoryProducts,
        //                                                                lstAttractionProductList, lstAdditionalAttractionProductsList, ref Message,
        //                                                                purchasedProductSelected, purchasedAdditionalProduct, SQLTrx);//Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //                    //End Modification -Jan-20-2016 -Modified the method to pass List of Attraction Object //
        //                    if (Transaction == null)
        //                    {
        //                        SQLTrx.Rollback();
        //                        if (newBooking)
        //                            BookingId = -1;

        //                        log.LogVariableState("Message", Message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }

        //                    dtPackage = Utilities.executeDataTable("select * from products where product_id = @BookingProductId",
        //                                                         new SqlParameter("@BookingProductId", bookingDetails[0].bookingProductId));
        //                    AdvanceRequired = 0;
        //                    if (dtPackage.Rows[0]["AdvanceAmount"] != DBNull.Value)
        //                        AdvanceRequired += Convert.ToDouble(dtPackage.Rows[0]["AdvanceAmount"]);
        //                    else if (dtPackage.Rows[0]["AdvancePercentage"] != DBNull.Value)
        //                        AdvanceRequired = Convert.ToDouble(dtPackage.Rows[0]["AdvancePercentage"]) * Transaction.Net_Transaction_Amount / 100;

        //                    AdvanceRequired = Math.Min(Transaction.Net_Transaction_Amount, AdvanceRequired);
        //                    totalAdvanceRequired = AdvanceRequired;

        //                }

        //                //Status = "BOOKED";//Commented and added Status within conditions to return WIP and BOOKED status accordingly on May 13, 2015

        //                SQLTrx.Commit();

        //                //Begin Modification- Dec-30-2015-Insert into Trxpayments incase of website transation//
        //                if (creditCardAmount > 0)
        //                {
        //                    PaymentModeList paymentModeListBL = new PaymentModeList();
        //                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
        //                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
        //                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
        //                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
        //                    if (paymentModeDTOList != null)
        //                    {
        //                        TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, Transaction.Trx_id, paymentModeDTOList[0].PaymentModeId, creditCardAmount,
        //                                                                                          "", "", "", "", "", -1, "", -1, 0, -1, PaymentReference, "", false, -1, -1, "", Utilities.getServerTime(),
        //                                                                                          Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
        //                        trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
        //                        trxPaymentDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;
        //                        Transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
        //                    }
        //                }


        //                if (debitCardAmount > 0)
        //                {
        //                    Semnox.Parafait.Customer.Accounts.AccountBL CheckCard = new Customer.Accounts.AccountBL(Utilities.ExecutionContext, paymentCardNumber, false, false);
        //                    int cardId = CheckCard.GetAccountId();
        //                    PaymentModeList paymentModeListBL = new PaymentModeList();
        //                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
        //                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
        //                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
        //                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
        //                    if (paymentModeDTOList != null)
        //                    {
        //                        TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, Transaction.Trx_id, paymentModeDTOList[0].PaymentModeId, debitCardAmount,
        //                                                                                          "", "", "", "", "", cardId, "C", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
        //                                                                                          Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
        //                        trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
        //                        Transaction.PaymentCardNumber = paymentCardNumber;
        //                        //Transaction.GameCardId = cardId;
        //                        Transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);
        //                    }
        //                }
        //                //End Modification- Dec-30-2015-Insert into Trxpayments incase of website transation//

        //                //Begin: save the transaction
        //                DataTable dtReservation = Utilities.executeDataTable(@"select * from 
        //                                                                       Bookings 
        //                                                                       where BookingId = @BookingId
        //                                                                        and status != 'CANCELLED'
        //                                                                        and (ExpiryTime is null or ExpiryTime >= getdate())",
        //                                                                       new SqlParameter[] { new SqlParameter("@BookingId", BookingId) });

        //                if (dtReservation.Rows.Count == 0)
        //                {
        //                    Message = Utilities.MessageUtils.getMessage(328, ReservationCode);

        //                    log.LogVariableState("Message", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }

        //                if (!isEditedBooking)
        //                {
        //                    if (!cancelTransaction(dtReservation.Rows[0]["TrxId"], ref Message))
        //                    {
        //                        log.LogVariableState("Message ", Message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }
        //                }
        //                if (Transaction == null)
        //                {
        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        if (Transaction.SaveOrder(ref Message) != 0)
        //                        {
        //                            log.LogVariableState("Message ", Message);
        //                            log.LogMethodExit(false);
        //                            return false;
        //                        }
        //                        else
        //                        //If save is successful , then update the bookings table with trxid//
        //                        {
        //                            Utilities.executeNonQuery(@"update Bookings 
        //                                                                   set TrxId = @TrxId
        //                                                                   where BookingId = @BookingId",
        //                                                                        new SqlParameter("@TrxId", Transaction.Trx_id),
        //                                                                        new SqlParameter("@BookingId", BookingId));
        //                            Utilities.executeNonQuery(@"update trx_header set status = 'BOOKING' where trxId = @TrxId", new SqlParameter("@TrxId", Transaction.Trx_id));//Update the status of trx_header to BOOKING

        //                            log.LogVariableState("@TrxId", Transaction.Trx_id);
        //                            log.LogVariableState("@BookingId", BookingId);
        //                            log.LogVariableState("@TrxId", Transaction.Trx_id);
        //                        }

        //                        //begin:If booking is edited//
        //                        if (isEditedBooking)
        //                        {
        //                            //string message = "";
        //                            int transactionId = 0;
        //                            int newTransactionId = 0;
        //                            object id = Utilities.executeScalar(@"Select TrxId from Bookings where BookingId = @BookingId",
        //                                                                  new SqlParameter("@BookingId", editedBookingId));

        //                            log.LogVariableState("@BookingId", editedBookingId);

        //                            if (id != DBNull.Value)
        //                            {
        //                                transactionId = Convert.ToInt32(id);
        //                            }
        //                            newTransactionId = Transaction.Trx_id;
        //                            //Update the payments table with new trxid//
        //                            Utilities.executeScalar(@"update TrxPayments set TrxId = @NewTransactionId  where TrxId = @TransactionId",
        //                                               new SqlParameter("@NewTransactionId", newTransactionId),
        //                                               new SqlParameter("@TransactionId", transactionId));

        //                            log.LogVariableState("@NewTransactionId", newTransactionId);
        //                            log.LogVariableState("@TransactionId", transactionId);

        //                            bool cancelStatus = CancelOldBookingLines(transactionId, newTransactionId, ref Message);
        //                        }

        //                        Transaction.getTotalPaidAmount(null);
        //                        Transaction.updateAmounts();
        //                        //End: Edit Booking//
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Error("Error occured when updating Bookings", ex);
        //                        Message = ex.Message;
        //                        log.LogVariableState("Message ", Message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }
        //                }

        //                //end: save transaction
        //                log.LogMethodExit(true);
        //                return true;
        //            }

        //            catch (Exception ex)
        //            {
        //                log.Error("Unable to save Transaction", ex);

        //                SQLTrx.Rollback();
        //                Message = ex.Message;
        //                if (newBooking)
        //                    BookingId = -1;

        //                log.LogVariableState("newBooking", newBooking);
        //                log.LogVariableState("BookingId", BookingId);
        //                log.LogVariableState("Message ", Message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }

        //        }
        //        //End : Booking Related Modification 05-Sept-2015//

        //        /*Begin: Booking Related Modification.Added to edit reservation after confirming with payment on May 13, 2015
        //          Here a new record is inserted into Bookings table with the Booking deatails selected to modify and the Old booking status is updated to "CANCELLED
        //          If the Tranasction had card products the the transaction will be reversed 10-Sept-2015*/
        //        public bool EditReservation(ref string message)
        //        {
        //            log.LogMethodEntry(message);

        //            SqlConnection cnn = Utilities.createConnection();
        //            SqlTransaction SQLTrx = cnn.BeginTransaction();
        //            message = "";
        //            try
        //            {
        //                if (BookingId != -1)
        //                {
        //                    //Added an additional field BookingProductId//
        //                    object id = Utilities.executeScalar(@"Insert into Bookings 
        //                                                           (
        //                                                            [BookingName]
        //                                                           ,[CustomerName]
        //                                                           ,[FromDate]
        //                                                           ,[ToDate]
        //                                                           ,[TrxId]
        //                                                           ,[Quantity]
        //                                                           ,[ReservationCode]
        //                                                           ,[Status]
        //                                                           ,[CardId]
        //                                                           ,[CardNumber]
        //                                                           ,[CustomerId]
        //                                                           ,[ContactNo]
        //                                                           ,[AlternateContactNo]
        //                                                           ,[Email]
        //                                                           ,[Channel]
        //                                                           ,[recur_flag]
        //                                                           ,[recur_frequency]
        //                                                           ,[recur_end_date]
        //                                                           ,[isEmailSent]
        //                                                           ,[ExpiryTime]
        //                                                           ,[Remarks]
        //                                                           ,[CreatedBy]
        //                                                           ,[CreationDate]
        //                                                           ,[LastUpdatedBy]
        //                                                           ,[LastUpdatedDate]
        //                                                           ,[Age]
        //                                                           ,[Gender]
        //                                                           ,[PostalAddress]
        //                                                           ,[BookingProductId]
        //                                                           ,[AttractionScheduleId]
        //                                                           ,[Site_Id]
        //                                                             )
        //                                                    Select  

        //                                                            BookingName
        //                                                           ,CustomerName
        //                                                           ,FromDate
        //                                                           ,ToDate
        //                                                           ,TrxId
        //                                                           ,Quantity
        //                                                           ,ReservationCode
        //                                                           ,'WIP'
        //                                                           ,CardId
        //                                                           ,CardNumber
        //                                                           ,CustomerId
        //                                                           ,ContactNo
        //                                                           ,AlternateContactNo
        //                                                           ,Email
        //                                                           ,Channel
        //                                                           ,recur_flag
        //                                                           ,recur_frequency
        //                                                           ,recur_end_date
        //                                                           ,isEmailSent
        //                                                           ,@ExpiryTime
        //                                                           ,Remarks
        //                                                           ,CreatedBy
        //                                                           ,CreationDate
        //                                                           ,LastUpdatedBy
        //                                                           ,LastUpdatedDate
        //                                                           ,Age
        //                                                           ,Gender
        //                                                           ,PostalAddress
        //                                                           ,BookingProductId
        //                                                           ,AttractionScheduleId
        //                                                           ,Site_Id
        //                                                           from Bookings where BookingId = @BookingId
        //                                                           ; select @@identity", SQLTrx,
        //                                                 new SqlParameter("@BookingId", BookingId),
        //                                                 new SqlParameter("@ExpiryTime", DateTime.Now.AddMinutes(30))
        //                                                 );

        //                    Utilities.executeScalar(@"update Bookings set Status ='SYSTEMABANDONED', ReservationCode = NULL  where BookingId = @BookingId", SQLTrx,
        //                                                new SqlParameter("@BookingId", BookingId));

        //                    log.LogVariableState("@BookingId", BookingId);
        //                    log.LogVariableState("@ExpiryTime", DateTime.Now.AddMinutes(30));
        //                    log.LogVariableState("@BookingId", BookingId);

        //                    BookingId = Convert.ToInt32(id);

        //                }


        //                //To reverse card updates if reservation was edited//
        //                TransactionUtils trxUtils = new TransactionUtils(Utilities);
        //                TaskProcs = new TaskProcs(Utilities);
        //                currentCard = new Card(Utilities);
        //                //ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
        //                //ParafaitEnv.Initialize();
        //                int transactionId = -1;//Added to reverse Attraction Bookings on January 27-2016//

        //                //Get the card numbers, transaction id and Line id //
        //                //DataTable dtCardNumbers = Utilities.executeDataTable(@"Select distinct card_number, card_id, TrxId, LineId from trx_lines where trxid =(Select TrxId from   Bookings where BookingId = @BookingId)
        //                // and card_number is not null", SQLTrx, new SqlParameter("@BookingId", BookingId));
        //                //Begin Modification-Added to reverse Attraction Bookings on January 27-2016//
        //                object oTrxId = Utilities.executeScalar(@"Select TrxId from  trx_lines where trxid = 
        //                                          (Select TrxId from Bookings where BookingId = @BookingId)", SQLTrx, new SqlParameter("@BookingId", BookingId));
        //                if (oTrxId != null)
        //                {
        //                    transactionId = Convert.ToInt32(oTrxId);
        //                    //End-Added to reverse Attraction Bookings on January 27-2016//

        //                    //Get the card numbers, transaction id and Line id //
        //                    DataTable dtCardNumbers = Utilities.executeDataTable(@"Select distinct card_number, card_id, TrxId, LineId from trx_lines where trxid =(Select TrxId from   Bookings where BookingId = @BookingId)
        //                                                                    and card_number is not null", SQLTrx, new SqlParameter("@BookingId", BookingId));

        //                    //Begin: Modified For LoginId and userId Added on 10-Sep-2016
        //                    //get the login id and user id //
        //                    DataTable dtlogin = Utilities.executeDataTable(@"select th.user_id userId, u.loginid loginId from trx_header th ,users u
        //                                             where th.user_id = u.user_id
        //                                             and TrxId = @transactionId", SQLTrx, new SqlParameter("@transactionId", transactionId));
        //                    string loginId = dtlogin.Rows[0]["loginId"].ToString();
        //                    int userId = Convert.ToInt32(dtlogin.Rows[0]["userId"].ToString());
        //                    int reversalTrxId = 0;
        //                    //End: Modified For LoginId and userId Added on 10-Sep-2016

        //                    TransactionUtils reverseTransaction = new TransactionUtils(Utilities);
        //                    foreach (DataRow cards in dtCardNumbers.Rows)
        //                    {
        //                        //Get the card details//
        //                        currentCard.getCardDetails(cards["card_number"].ToString(), Convert.ToInt32(cards["card_id"].ToString()));

        //                        bool reverseCardUpdates = true;

        //                        //reverse the card updates incase of Edit booking//

        //                        if (!reverseTransaction.ReverseCard(Convert.ToInt32(cards["TrxId"].ToString()), Convert.ToInt32(cards["LineId"].ToString()), loginId, reverseCardUpdates, ref message, reversalTrxId, SQLTrx, cnn))
        //                        {
        //                            log.LogVariableState("message ", message);
        //                            log.LogMethodExit(false);
        //                            return false;
        //                        }//Added on 10-Sep-2016 
        //                    }
        //                    //reverseTransaction.reverseTransaction(transactionId, -1, false, Utilities.ParafaitEnv.POSMachine, loginId, userId, "ADMIN", "Reversed Card Updates for Edit Booking", ref message);//Added for reverse Booking on 10-Sep-2016

        //                    if (!reverseTransaction.ReverseTransactionEntity(transactionId, -1, loginId, userId, Utilities.ParafaitEnv.Username, "Reversed Card Updates for Edit Booking", ref message, ref reversalTrxId, SQLTrx, cnn))
        //                    {
        //                        log.LogVariableState("message ", message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }//Added for reverse Booking on 10-Sep-2016

        //                    reverseTransaction.reverseAttractionBookings(transactionId, -1, null);//Added to reverse Attraction Bookings on January 27-2016//

        //                    //Begin: Modified For Updating the TrxDate and Status of Transaction Header Added on 10-Sep-2016
        //                    object oTrxDate = Utilities.executeScalar(@"Select TrxDate from  trx_header where trxId = @TrxId", SQLTrx, new SqlParameter("@TrxId", transactionId));
        //                    reverseTransaction.SetStatusAsSystemAbandoned(transactionId, SQLTrx);
        //                    if (oTrxDate != DBNull.Value)
        //                    {
        //                        Utilities.executeNonQuery(@"UPDATE trx_header SET Status = 'CANCELLED',TrxDate = @trxDate,trxAmount = 0, TaxAmount = 0, trxNetAmount = 0, cashAmount = 0  WHERE trxId = @TrxId;",
        //                                                                SQLTrx, new SqlParameter("@TrxId", reversalTrxId)
        //                                                                , new SqlParameter("@trxDate", Convert.ToDateTime(oTrxDate)));//Added on 10-Sep-2016

        //                        log.LogVariableState("@TrxId", reversalTrxId);
        //                        log.LogVariableState("@trxDate", Convert.ToDateTime(oTrxDate));

        //                    }
        //                    //End: Modified For Updating the TrxDate and Status of Transaction Header Added on 10-Sep-2016
        //                }

        //                //}
        //                //End: Modified For Updating the TrxDate and Status of Transaction Header Added on 10-Sep-2016
        //                SQLTrx.Commit();

        //                log.LogVariableState("message ", message);
        //                log.LogMethodExit(true);
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Error occured when Updating the TrxDate and Status of Transaction Header", ex);
        //                SQLTrx.Rollback();
        //                message = ex.Message;

        //                log.LogVariableState("message ", message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }
        //        }
        //        //End: Edit Reservation. Booking related changes. 10-Sept-2015

        //        //Added 2 parameters to edit reservation after confirming with payment on May 18, 2015//
        //        public bool ConfirmReservation(ref string Message, bool isEditedBooking, int editedBookingId)
        //        {
        //            log.LogMethodEntry(Message, isEditedBooking, editedBookingId);

        //            bool returnValueNew = ConfirmReservation(0, 0, "", "", isEditedBooking, editedBookingId, ref Message);
        //            log.LogMethodExit(returnValueNew);
        //            return returnValueNew;
        //        }

        //        /*Added 2 parameters to edit reservation after confirming with payment on May 18, 2015
        //         if Edited booking then isEditedBooking isset to true andeditedBookingId is the BookingId selcted to modify 
        //         Modified the method and moved save transaction to MakeReservation*/

        //        public bool ConfirmReservation(double creditCardAmount, double debitCardAmount, string paymentCardNumber, string PaymentReference, bool isEditedBooking, int editedBookingId, ref string Message)
        //        {
        //            log.LogMethodEntry(creditCardAmount, debitCardAmount, paymentCardNumber, PaymentReference, isEditedBooking, editedBookingId, Message);

        //            try
        //            {
        //                DataTable dtReservation = Utilities.executeDataTable(@"select * from 
        //                                                                       Bookings 
        //                                                                       where BookingId = @BookingId
        //                                                                        and status != 'CANCELLED'
        //                                                                        and (ExpiryTime is null or ExpiryTime >= getdate())",
        //                                                                        new SqlParameter[] { new SqlParameter("@BookingId", BookingId) });

        //                log.LogVariableState("@BookingId", BookingId);

        //                if (dtReservation.Rows.Count == 0)
        //                {
        //                    Message = Utilities.MessageUtils.getMessage(328, ReservationCode);

        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }

        //                //Transaction = CreateReservationTransaction(ref Message);//Booking Related Changes.commented on August 30,2015 

        //                if (Transaction == null)
        //                {
        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        //if (Transaction.SaveOrder(ref Message) != 0)//commented and moved to MakeReservationmethod
        //                        //    return false;

        //                        DataTable dt = Utilities.executeDataTable("Select trxId, ReservationCode, CardNumber, customerId, FromDate from Bookings where BookingId = @BookingId", new SqlParameter("@BookingID", BookingId));
        //                        ReservationCode = dt.Rows[0][1].ToString();
        //                        object trxId = dt.Rows[0][0];
        //                        CardNumber = dt.Rows[0][2].ToString();
        //                        if (dt.Rows[0][3] != DBNull.Value)
        //                            CustomerId = Convert.ToInt32(dt.Rows[0][3]);
        //                        ReservationDate = Convert.ToDateTime(dt.Rows[0]["FromDate"]);
        //                        if (trxId != DBNull.Value)
        //                        {
        //                            int numRows = Utilities.executeNonQuery(@"update Bookings 
        //                                                                    set status = 'CONFIRMED', TrxId = @TrxId, CardId = @cardId
        //                                                                   where BookingId = @BookingId;
        //                                                                   update trx_header set status = 'RESERVED' where trxId = @TrxId;
        //                                                                   update cardGames set FromDate = @date where trxId = @TrxId and FromDate is null",
        //                                                                       new SqlParameter("@BookingId", dtReservation.Rows[0]["BookingId"]),
        //                                                                       new SqlParameter("@date", dtReservation.Rows[0]["FromDate"]),
        //                                                                       new SqlParameter("@TrxId", Transaction.Trx_id),
        //                                                                       new SqlParameter("@cardId", Transaction.PrimaryCard == null ? DBNull.Value : (object)Transaction.PrimaryCard.card_id)
        //                                                                       );

        //                            log.LogVariableState("@BookingId", dtReservation.Rows[0]["BookingId"]);
        //                            log.LogVariableState("@date", dtReservation.Rows[0]["FromDate"]);
        //                            log.LogVariableState("@TrxId", Transaction.Trx_id);
        //                            log.LogVariableState("@cardId", Transaction.PrimaryCard == null ? DBNull.Value : (object)Transaction.PrimaryCard.card_id);

        //                            if (numRows == 0)
        //                            {
        //                                Message = Utilities.MessageUtils.getMessage(330, ReservationCode);

        //                                log.LogVariableState("Message ", Message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }
        //                            Status = ReservationDTO.ReservationStatus.CONFIRMED.ToString();
        //                        }

        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        log.Error("Error occured when selecting the Booking details ", ex);
        //                        Message = ex.Message;
        //                        log.LogVariableState("Message ", Message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Error occured when selecting values from Bookings", ex);
        //                Message = ex.Message;
        //                log.LogVariableState("Message ", Message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }

        //            log.LogVariableState("Message ", Message);
        //            log.LogMethodExit(true);
        //            return true;
        //        }

        //        /*Begin: Booking related changes.Added to edit reservation after confirming with payment on May 18, 2015
        //         Here If any Payments where previously done it is upadted with new Transaction Id
        //         And further call cancelTransaction() to clear the Old booking details and delete Payments with edited Booking Transaction Id*/
        //        public bool CancelOldBookingLines(int transactionId, int newTransactionId, ref string message)
        //        {
        //            log.LogMethodEntry(transactionId, newTransactionId, message);

        //            try
        //            {
        //                Utilities.executeScalar(@"update TrxPayments set TrxId = @NewTransactionId  where TrxId = @TransactionId",
        //                                                new SqlParameter("@NewTransactionId", newTransactionId),
        //                                                new SqlParameter("@TransactionId", transactionId));
        //                bool cancelStatus = cancelTransaction(transactionId, ref message);
        //                if (cancelStatus)
        //                {
        //                    TransactionUtils transactionUtils = new TransactionUtils(Utilities);
        //                    transactionUtils.SetStatusAsSystemAbandoned(transactionId, null);
        //                }

        //                log.LogVariableState("@NewTransactionId", newTransactionId);
        //                log.LogVariableState("@TransactionId", transactionId);
        //                log.LogVariableState("message ", message);
        //                log.LogMethodExit(true);
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Error occured when Updating the TrxPayments or when Cancelling the transaction", ex);
        //                message = ex.Message;

        //                log.LogVariableState("message ", message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }
        //        }
        //        //End :Booking related changes. 18-May-2015

        //        //Begin: Booking Related Changes.Added to Apply manual discount to reservations at transaction level . To create the discount lines on July 2 2015//
        //        public bool CreateReservationDiscount(Transaction Trx)
        //        {
        //            log.LogMethodEntry(Trx);
        //            // bool ret = false;
        //            string message = "";
        //            try
        //            {
        //                List<ReservationDiscountsDTO> reservationDiscountsDTOList = null;
        //                ReservationDiscountsListBL reservationDiscountsListBL = new ReservationDiscountsListBL();
        //                List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>> searchReservationDiscountsParams = new List<KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>>();
        //                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.BOOKING_ID, BookingId.ToString()));
        //                searchReservationDiscountsParams.Add(new KeyValuePair<ReservationDiscountsDTO.SearchByParameters, string>(ReservationDiscountsDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
        //                reservationDiscountsDTOList = reservationDiscountsListBL.GetReservationDiscountsDTOList(searchReservationDiscountsParams);
        //                if (reservationDiscountsDTOList != null)
        //                {
        //                    foreach (var reservationDiscountsDTO in reservationDiscountsDTOList)
        //                    {
        //                        DiscountsBL discountsBL = Trx.GetDiscountsBL(reservationDiscountsDTO.ReservationDiscountId);
        //                        if (discountsBL != null && discountsBL.DiscountsDTO != null)
        //                        {
        //                            discountsBL.Apply(Trx);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Applying Reservation Discounts Unsuccessful! ", ex);
        //                message = ex.Message;
        //                log.LogMethodExit(false);
        //                return false;

        //            }
        //            log.LogMethodExit(true);
        //            return true;
        //        }
        //        //end : Booking related changes. discounts 02-july-2015//

        //        public bool ExecuteReservation(Dictionary<string, string> cardList, ref string message, bool completeTransaction = false)
        //        {
        //            log.LogMethodEntry(cardList, message, completeTransaction);

        //            SqlTransaction SQLTrx = Utilities.createConnection().BeginTransaction();
        //            SqlConnection cnn = SQLTrx.Connection;
        //            try
        //            {
        //                if (cardList.Count > 0)
        //                {
        //                    TaskProcs tp = new TaskProcs(Utilities);

        //                    foreach (KeyValuePair<string, string> keyv in cardList)
        //                    {
        //                        if (keyv.Key.Equals(keyv.Value))
        //                            continue;
        //                        Card card = new Card(keyv.Value, "", Utilities);
        //                        Card tempCard = new Card(keyv.Key, "", Utilities);
        //                        if (card.CardStatus == "ISSUED")
        //                        {
        //                            Card[] cards = new Card[2];
        //                            cards[0] = tempCard;
        //                            cards[1] = card;

        //                            if (!tp.Consolidate(cards, 2, "Execute Reservation", ref message, SQLTrx, true))
        //                            {
        //                                if (SQLTrx.Connection != null)
        //                                    SQLTrx.Rollback();

        //                                log.LogVariableState("message ", message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }

        //                            for (int i = 0; i < Transaction.TrxLines.Count; i++)
        //                            {
        //                                Transaction.TransactionLine tl = Transaction.TrxLines[i];
        //                                if (tempCard.CardNumber.Equals(tl.CardNumber)
        //                                   && tl.ProductID.Equals(Utilities.ParafaitEnv.CardDepositProductId))
        //                                {
        //                                    Transaction.cancelLine(i, SQLTrx);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (!tp.transferCard(tempCard, card, "Execute Reservation", ref message, SQLTrx))
        //                            {
        //                                if (SQLTrx.Connection != null)
        //                                    SQLTrx.Rollback();

        //                                log.LogVariableState("message ", message);
        //                                log.LogMethodExit(false);
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                }
        //                Utilities.executeNonQuery("update trx_header set status = 'OPEN' where trxId = @TrxId; update bookings set status = 'COMPLETE' where BookingId = @BookingId",
        //                                                                                SQLTrx,
        //                                                                                new SqlParameter("@BookingId", BookingId),
        //                                                                                new SqlParameter("@TrxId", Transaction.Trx_id));
        //                log.LogVariableState("@BookingId", BookingId);
        //                log.LogVariableState("@TrxId", Transaction.Trx_id);

        //                if (completeTransaction)
        //                {
        //                    if (Utilities.executeScalar("select top 1 1 from trx_lines where trxId = @trxId and card_number like 'T%'",
        //                                                 SQLTrx,
        //                                                 new SqlParameter("trxId", Transaction.Trx_id)) != null)
        //                    {
        //                        log.LogVariableState("trxId", Transaction.Trx_id);
        //                        completeTransaction = false;
        //                    }
        //                }

        //                if (completeTransaction)
        //                {
        //                    if (Transaction.CompleteTransaction(SQLTrx, ref message) == false)//Modification on 17-May-2016 for adding PosPlus 
        //                    {
        //                        message = Utilities.MessageUtils.getMessage(526);
        //                        if (SQLTrx.Connection != null)
        //                            SQLTrx.Rollback();

        //                        log.LogVariableState("message ", message);
        //                        log.LogMethodExit(false);
        //                        return false;
        //                    }
        //                }

        //                Status = ReservationDTO.ReservationStatus.COMPLETE.ToString();

        //                SQLTrx.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Unable to Commit the Transaction! ", ex);

        //                if (SQLTrx.Connection != null)
        //                    SQLTrx.Rollback();

        //                log.LogVariableState("message ", message);
        //                log.LogMethodExit(false);
        //                message = ex.Message;
        //                return false;
        //            }
        //            finally
        //            {
        //                cnn.Close();
        //            }

        //            log.LogVariableState("message ", message);
        //            log.LogMethodExit(true);
        //            return true;
        //        }

        //        public bool CancelReservation(string ReservationCode, ref string Message)
        //        {
        //            log.LogMethodEntry(ReservationCode, Message);

        //            try
        //            {
        //                DataTable dtReservation = Utilities.executeDataTable(@"select * from 
        //                                                                       Bookings 
        //                                                                       where ReservationCode = @ReservationCode 
        //                                                                        and status != 'CANCELLED'",
        //                                                                        new SqlParameter[] { new SqlParameter("@ReservationCode", ReservationCode) });

        //                log.LogVariableState("@ReservationCode", ReservationCode);

        //                if (dtReservation.Rows.Count == 0)
        //                {
        //                    Message = Utilities.MessageUtils.getMessage(328, ReservationCode);

        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }

        //                if (!cancelTransaction(dtReservation.Rows[0]["TrxId"], ref Message))
        //                {
        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }

        //                Utilities.executeScalar("update bookings set status = 'CANCELLED' where BookingId = @BookingId", new SqlParameter[] { new SqlParameter("@BookingId", dtReservation.Rows[0]["BookingId"]) });

        //                log.LogVariableState("@BookingId", dtReservation.Rows[0]["BookingId"]);
        //                Status = "CANCELLED";

        //                log.LogVariableState("Message ", Message);
        //                log.LogMethodExit(true);
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Unable to Cancel the Transaction! ", ex);
        //                Message = ex.Message;

        //                log.LogVariableState("Message ", Message);
        //                log.LogMethodExit(false);
        //                return false;
        //            }
        //        }

        //        //Get the Booking Products
        //        //Begin Modification- Dec-30-2015- Modifed to add  description column//
        //        public DataTable GetBookingProducts(int siteId, string posMachine = "", string loginId = "")
        //        {
        //            log.LogMethodEntry(siteId, posMachine, loginId);

        //            string BookingFilterQuery = "";
        //            string QueryJoiner = " where ";
        //            if (string.IsNullOrEmpty(posMachine))
        //            {
        //                BookingFilterQuery = "inner join PRODUCT_TYPE PT on p.product_type_id = PT.product_type_id AND P.active_flag='Y' ";

        //            }
        //            else
        //            {
        //                BookingFilterQuery = @"inner join PRODUCT_TYPE PT on p.product_type_id = PT.product_type_id AND P.active_flag='Y' 
        //                                            left outer join (SELECT * from (SELECT *, DENSE_RANK() over (partition by ProductId order by CreatedDate ASC) as D 
        //                                                             from ProductsDisplayGroup) T
        //                                                             WHERE T.D = 1) pd on pd.ProductId = p.product_id
        //                                            left outer join ProductDisplayGroupFormat pdgf on pdgf.Id = pd.DisplayGroupId
        //                                            where (pdgf.site_id = @siteid  or @siteid = -1) 
        //                                             and not exists (select 1 
        //                                                               from UserRoleDisplayGroupExclusions urdge , 
        //                                                                    users u
        //                                                              where urdge.ProductDisplayGroupId = pdgf.Id
        //                                                                and urdge.role_id = u.role_id
        //                                                                and u.loginId = @loginId )
        //                                            and pdgf.Id not in  (SELECT distinct ProductDisplayGroupFormatId 
        //							                                            from POSProductExclusions 
        //							                                                where POSMachineId in 
        //								                                            (select POSMachineId from POSMachines
        //								                                                where Computer_Name=@posMachine )) ";
        //                QueryJoiner = " and ";
        //            }


        //            DataTable dt = Utilities.executeDataTable(@"Select  product_id,product_name,p.ImageFileName,p.description, p.price , 
        //                                                                isnull(p.AdvanceAmount,0)AdvanceAmount,
        //                                                                isnull(p.MinimumQuantity,0)MinimumQuantity,
        //                                                                isnull(p.MinimumTime,0)MinimumTime,
        //                                                                isnull(p.MaximumTime,0)MaximumTime,
        //                                                                isnull(p.sort_order,0) sort_order,
        //                                                                isnull(p.AvailableUnits,0) AvailableUnits  
        //                                                         from products p " + BookingFilterQuery +
        //                                                        QueryJoiner +
        //                                                      @"p.product_type_id = pt.product_type_id
        //                                                        and pt.product_type ='BOOKINGS' 
        //                                                        and p.active_flag ='Y'
        //                                                        AND GETDATE() BETWEEN ISNULL(p.StartDate,getdate()) and ISNULL(p.ExpiryDate,getdate()+1)
        //                                                        AND (p.site_id = @siteid or @siteid = -1) 
        //                                                        order by isnull(p.sort_order,0) ",
        //                                                    new SqlParameter("@siteid", siteId),
        //                                                    new SqlParameter("@posMachine", posMachine),
        //                                                     new SqlParameter("@loginId", loginId));


        //            log.LogVariableState("@siteid", siteId);
        //            log.LogVariableState("@posMachine", posMachine);
        //            log.LogVariableState("@loginId", loginId);

        //            log.LogMethodExit(dt);
        //            return dt;
        //        }
        //        //End Modification- Dec-30-2015- Modifed to add  description column//
        //        //End Modification- Jan-20-2017- Modifed to add  MinimumQuantity.MinimumTime,MaximumTime,order column//

        //        //Begin:Get the contents of the booking product selected
        //        //Begin Modification- Dec-30-2015- Modifed to add  ImageFileName column//
        //        public DataTable GetBookingProductContents(int bookingProductId)
        //        {
        //            log.LogMethodEntry(bookingProductId);

        //            DataTable dtBookingContents = Utilities.executeDataTable(@"SELECT
        //                                                          cp.Product_Id ParentId,
        //                                                          ChildProductId ChildId,p.price,
        //                                                          PriceInclusive,
        //                                                          p.product_name,
        //							                              p.ImageFileName productImage,
        //                                                          ISNULL(Quantity, 0) Quantity,
        //                                                          cp.CategoryId,
        //                                                          pt.product_type ProductType  
        //                                                          FROM ComboProduct cp
        //                                                             INNER JOIN Products p
        //                                                               ON p.product_id = cp.ChildProductId,
        //                                                              product_type pt
        //                                                        WHERE isnull(AdditionalProduct,'N') != 'Y'
        //                                                        AND pt.product_type_id = p.product_type_id
        //                                                        AND cp.Product_Id = @bookingProductId 
        //                                                        ORDER BY  isnull(cp.SortOrder, 0) ", new SqlParameter("@bookingProductId", bookingProductId));

        //            log.LogVariableState("@bookingProductId", bookingProductId);

        //            log.LogMethodExit(dtBookingContents);
        //            return dtBookingContents;
        //        }
        //        //End:Booking Related changes//
        //        //End Modification- Dec-30-2015- Modifed to add  ImageFileName column//

        //        //Begin:Get the contents of the products incase the product is of type combo
        //        public DataTable GetPackageContents(int comboProductId)
        //        {
        //            log.LogMethodEntry(comboProductId);

        //            DataTable dtAdditionalProducts = Utilities.executeDataTable(@"SELECT
        //                                                                            product_name,pt.product_type,
        //                                                                            Name Category,
        //                                                                            Quantity,
        //                                                                            c.CategoryId
        //                                                                        FROM comboProduct cp
        //                                                                        LEFT OUTER JOIN products p
        //                                                                            ON p.Product_Id = cp.ChildProductId
        //                                                                        LEFT OUTER JOIN category c
        //                                                                            ON c.categoryId = cp.categoryId, product_type pt
        //                                                                        WHERE cp.Product_Id = @comboProductId
        //													                    and pt.product_type_id = p.product_type_id"
        //                                                                        , new SqlParameter("@comboProductId", comboProductId));

        //            log.LogVariableState("@comboProductId", comboProductId);

        //            log.LogMethodExit(dtAdditionalProducts);
        //            return dtAdditionalProducts;
        //        }
        //        //End:Booking Related changes//

        //        //Begin:Get the additional products
        //        //Begin Modification- Dec-30-2015- Modifed to add  productImage column//
        //        public DataTable GetAdditionalProducts(int bookingProductId)
        //        {
        //            log.LogMethodEntry(bookingProductId);

        //            DataTable dtAdditionalProducts = Utilities.executeDataTable(@"SELECT p.product_name,p.ImageFileName productImage,
        //                                                                         cp.Product_Id ParentId,
        //                                                                         cp.ChildProductId ChildId,
        //                                                                         isnull((select DisplayGroup from ProductDisplayGroupFormat where Id = cp.DisplayGroupId),'') display_group,
        //                                                                         cp.CategoryId,
        //                                                                         p.price, pt.product_type,
        //                                                                         isnull(p.description,'')  description ,
        //                                                                         isnull(p.MinimumQuantity,0) MinimumQuantity, 
        //                                                                         isnull(p.CardCount,0) MaximumQuantity
        //                                                                       FROM ComboProduct cp
        //                                                                            INNER JOIN Products p
        //                                                                            ON p.product_id = cp.ChildProductId,
        //                                                                            product_type pt
        //                                                                       WHERE cp.AdditionalProduct = 'Y'
        //                                                                            AND cp.Product_Id = @bookingProductId
        //                                                                            AND p.active_flag = 'Y'
        //                                                                            AND pt.product_type_id = p.product_type_id
        //",
        //                                                                        new SqlParameter("@bookingProductId", bookingProductId));

        //            log.LogVariableState("@bookingProductId", bookingProductId);

        //            log.LogMethodExit(dtAdditionalProducts);
        //            return dtAdditionalProducts;
        //        }
        //        //End:Booking Related changes//
        //        //End Modification- Dec-30-2015- Modifed to add  productImage column//

        //        public DataTable GetServiceChargeProduct(int bookingProductId)
        //        {
        //            log.LogMethodEntry(bookingProductId);

        //            DataTable dtServiceChargeProduct = Utilities.executeDataTable(@"select p.product_name, p.ImageFileName productImage,
        //                                                                         @bookingProductId as ParentId,
        //                                                                         p.product_id as ChildId,
        //                                                                          isnull(p.display_group,'') display_group,
        //                                                                          p.CategoryId,
        //                                                                          p.price,pt.product_type,
        //                                                                          ISNULL(p.description,'') as description,
        //                                                                          p.MinimumQuantity,
        //                                                                            isnull(p.CardCount,0) MaximumQuantity
        //                                                                         from products p, product_type pt
        //                                                                        where p.product_type_id = pt.product_type_id
        //                                                                        and pt.product_type = 'SERVICECHARGE'
        //",
        //                                                                        new SqlParameter("@bookingProductId", bookingProductId));

        //            log.LogVariableState("@bookingProductId", bookingProductId);

        //            log.LogMethodExit(dtServiceChargeProduct);
        //            return dtServiceChargeProduct;
        //        }
        //        //Begin:Get the dispaly groups of the additional products
        //        //Begin Modification- Dec-30-2015- Modifed to add  displayGroupImageName column//
        //        public DataTable GetAdditionalProductsDisplayGroups(int bookingProductId)
        //        {
        //            log.LogMethodEntry(bookingProductId);

        //            //Modified Query on 30-Dec-2016 for multiple displaygroup enhancement
        //            DataTable dtAdditionalProductsDisplayGroup = Utilities.executeDataTable(@"SELECT  DISTINCT  CASE when cp.DisplaygroupId is null and pd.DisplayGroupId is null then
        //		                                                                               'Other' else  pdgf.DisplayGroup end display_group,
        //		                                                                                '' productGroupDescription, --p.description
        //		                                                                                isnull(pdgf.imagefilename,'') displayGroupImageName,
        //		                                                                                pdgf.SortOrder
        //		                                                                                FROM products p
        //    	                                                                                inner join ComboProduct cp
        //		                                                                                on p.product_id = cp.ChildProductId
        //		                                                                                left join (SELECT * from (SELECT *, DENSE_RANK() over (partition by ProductId order by CreatedDate ASC) as D from ProductsDisplayGroup)T
        //						                                                                                WHERE T.D = 1) pd  
        //		                                                                                on p.product_id = pd.ProductId
        //		                                                                                left  join ProductDisplayGroupFormat pdgf
        //		                                                                                on pdgf.Id = ISNULL(cp.DisplaygroupId, isnull(pd.DisplayGroupId, (Select top 1 Id from ProductDisplayGroupFormat)))
        //                                                                                        WHERE cp.Product_Id = @BookingProductId
        //			                                                                            AND cp.AdditionalProduct = 'Y'
        //			                                                                            AND p.active_flag = 'Y'",
        //                                                                                        new SqlParameter("@BookingProductId", bookingProductId));

        //            log.LogVariableState("@BookingProductId", bookingProductId);

        //            log.LogMethodExit(dtAdditionalProductsDisplayGroup);
        //            return dtAdditionalProductsDisplayGroup;
        //        }
        //        //End:Booking Related changes//
        //        //End Modification- Dec-30-2015- Modifed to add  displayGroupImageName column//
        //        //End:Booking Related changes//

        //        //Begin:Get dditional products based on the display group
        //        public DataTable GetAdditionalProductsByDispalyGroup(int bookingProductId, string displayGroupName)
        //        {
        //            log.LogMethodExit(bookingProductId, displayGroupName);

        //            DataTable dtAdditionalProductsDisplayGroup = Utilities.executeDataTable(@"select p.product_name, p.product_id, isnull(p.price, 0) price, pt.product_type
        //                                                                                        from products p
        //                                                                                        inner join ComboProduct cp
        //                                                                                        on cp.ChildProductId = p.product_id
        //                                                                                        inner join product_type pt
        //                                                                                        on pt.product_type_id = p.product_type_id
        //                                                                                        left join (SELECT * from (SELECT *, DENSE_RANK() over (partition by ProductId order by CreatedDate ASC) as D from ProductsDisplayGroup)T
        //						                                                                                    WHERE T.D = 1) pd  
        //                                                                                        on pd.ProductId = p.product_id
        //                                                                                        left join ProductDisplayGroupFormat pdf
        //                                                                                        on pdf.Id = ISNULL(cp.DisplaygroupId, pd.DisplayGroupId)
        //                                                                                        where cp.Product_Id = @BookingProductId 
        //                                                                                        and cp.AdditionalProduct = 'Y' and p.active_flag = 'Y'
        //                                                                                        and isnull(pdf.DisplayGroup,'Other') = @displayGroup
        //                                                                                        order by 1",
        //                                                                                    new SqlParameter("@BookingProductId", bookingProductId),
        //                                                                                    new SqlParameter("@displayGroup", displayGroupName));

        //            log.LogVariableState("@BookingProductId", bookingProductId);
        //            log.LogVariableState("@displayGroup", displayGroupName);

        //            log.LogMethodExit(dtAdditionalProductsDisplayGroup);
        //            return dtAdditionalProductsDisplayGroup;
        //        }
        //        //End:Booking Related changes//

        //        public DataTable getResources(string ResourceType)
        //        {
        //            log.LogMethodEntry(ResourceType);

        //            string resourceId;
        //            string resourceName, tableName;
        //            string activeFlag = "";
        //            switch (ResourceType)
        //            {
        //                case "Machine":
        //                    resourceName = "machine_name";
        //                    resourceId = "guid";
        //                    tableName = "machines";
        //                    activeFlag = " where active_flag = 'Y'";
        //                    break;
        //                case "Game":
        //                    resourceName = "game_name";
        //                    resourceId = "guid";
        //                    tableName = "games";
        //                    break;
        //                case "Facility":
        //                    resourceName = "FacilityName";
        //                    resourceId = "guid";
        //                    tableName = "CheckInFacility";
        //                    activeFlag = " where active_flag = 'Y'";
        //                    break;
        //                default:
        //                    {
        //                        log.LogMethodExit(null);
        //                        return null;
        //                    }
        //            }

        //            DataTable dt = Utilities.executeDataTable("select " + resourceId + ", " + resourceName +
        //                                                                    " from " + tableName +
        //                                                                    activeFlag +
        //                                                                    " order by 2", new SqlParameter[] { });
        //            log.LogVariableState("resourceName", resourceName);
        //            log.LogVariableState("resourceId", resourceId);
        //            log.LogVariableState("tableName", tableName);
        //            log.LogVariableState("activeFlag", activeFlag);
        //            log.LogMethodExit(dt);
        //            return dt;
        //        }


        //        /*Modified the method to separately  create lines for combo product and Manual products
        //        Create CreateComboProduct method is called in case of combo product.
        //        createTransactionLine method is called for rest of the products in the list
        //        If there are card products within combo or within the booking package then T cards are created and saved to trx_lines on 21-Aug-2015*/
        //        //Modified parameter list on Jan-20-2016//
        //        Transaction CreateReservationTransaction(int BookingProductId, double bookingProductPrice,
        //                                                            List<clsPackageList> lstPackageList,
        //                                                            List<clsProductList> lstAddonProducts,
        //                                                            List<string> Cards, List<Reservation.clsSelectedCategoryProducts> CategoryProducts,
        //                                                            List<KeyValuePair<AttractionBooking, int>> lstAttractionProductList,
        //                                                            List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductsList,
        //                                                            ref string Message,
        //                                                            List<KeyValuePair<PurchasedProducts, int>> purchasedProductSelected,
        //                                                            List<KeyValuePair<PurchasedProducts, int>> purchasedAdditionalProduct,
        //                                                            SqlTransaction SQLTrx = null)
        //        {
        //            log.LogMethodEntry(BookingProductId, bookingProductPrice, lstPackageList, lstAddonProducts, Cards, CategoryProducts,
        //                               lstAttractionProductList, lstAdditionalAttractionProductsList, Message, purchasedProductSelected, purchasedAdditionalProduct, SQLTrx);

        //            bool savIsClientServer = Utilities.ParafaitEnv.IsClientServer;

        //            // double fixedCharge = 0.0;
        //            try
        //            {
        //                this.purchasedProductSelected = purchasedProductSelected;
        //                this.purchasedAdditionalProduct = purchasedAdditionalProduct;
        //                Utilities.ParafaitEnv.IsClientServer = false;
        //                DataTable dtReservation = Utilities.executeDataTable(@"select * 
        //                                                                   from Bookings where BookingId = @BookingId", SQLTrx,
        //                                                                       new SqlParameter("@BookingId", BookingId));

        //                DataTable dtProds = Utilities.executeDataTable(@"select p.product_id, pt.product_type
        //                                                            from products p, comboProduct cp, product_type pt
        //                                                            where cp.product_Id = @productId
        //                                                            and cp.ChildProductId = p.product_id
        //                                                            and p.product_type_id = pt.product_type_id
        //                                                            and pt.product_type in ('NEW', 'RECHARGE', 'CARDSALE', 'GAMETIME')",
        //                                           new SqlParameter("@productId", BookingProductId));

        //                log.LogVariableState("@BookingId", BookingId);
        //                log.LogVariableState("@productId", BookingProductId);

        //                if (dtReservation.Rows.Count == 0)
        //                {
        //                    Message = Utilities.MessageUtils.getMessage(328, BookingId);
        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(null);
        //                    return null;
        //                }

        //                string User = Utilities.ParafaitEnv.Username;

        //                object CardId = dtReservation.Rows[0]["CardId"];
        //                Card card = null;
        //                if (CardId != DBNull.Value)
        //                    card = new Card((int)CardId, User, Utilities);

        //                POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
        //                List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
        //                Transaction Trx = new Transaction(posPrintersDTOList, Utilities);
        //                Trx.TrxDate = ReservationDate;
        //                Trx.EntitlementReferenceDate = ReservationDate;//Added on 08-Feb-2018
        //                //Begin: Added for Customerid in TrxHeader on 21-Mar-2016
        //                if (CustomerId < 0)
        //                {
        //                    Trx.customerDTO = new CustomerDTO();
        //                }
        //                else
        //                {
        //                    Trx.customerDTO = (new CustomerBL(Utilities.ExecutionContext, CustomerId, true, true, SQLTrx)).CustomerDTO;
        //                }
        //                OrderHeaderDTO orderHeaderDTO = new OrderHeaderDTO();
        //                orderHeaderDTO.CustomerName = Trx.customerDTO.FirstName + " " + Trx.customerDTO.LastName;
        //                orderHeaderDTO.POSMachineId = Utilities.ExecutionContext.GetMachineId();
        //                orderHeaderDTO.UserId = Utilities.ExecutionContext.GetUserPKId();
        //                if (CardId != null && CardId != DBNull.Value)
        //                {
        //                    orderHeaderDTO.CardId = Convert.ToInt32(CardId);
        //                }

        //                OrderHeaderBL orderHeaderBL = new OrderHeaderBL(Utilities.ExecutionContext, orderHeaderDTO);
        //                orderHeaderBL.Save(SQLTrx);
        //                Trx.Order = orderHeaderBL;

        //                //End: Added for Customerid in TrxHeader on 21-Mar-2016
        //                /*Added to fecth the Parafait Environment variables, when booking is edited.*/
        //                // Environment variables were getting reset here
        //                //ParafaitEnv ParafaitEnv = Utilities.ParafaitEnv;
        //                //ParafaitEnv.Initialize();

        //                //Begin-Added to check if attraction product has Card Sale checked on Jan-28-2016//
        //                TransactionUtils trxUtils = new TransactionUtils(Utilities);
        //                //End-Added to check if attraction product has Card Sale checked on Jan-28-2016//

        //                //Begin: Added to create booking product transaction
        //                if (Trx.createTransactionLine(card, BookingProductId, -1, 1, ref Message) != 0)//Changed price to -1 on Dec-30-2015//
        //                {
        //                    log.LogVariableState("Message ", Message);
        //                    log.LogMethodExit(null);
        //                    return null;
        //                }
        //                //end

        //                int PackageProductId = -1;
        //                int PackageQty = 1;
        //                //Create the transaction line for products listed part of booking Product selected. lstPackageList will have list of Combo and Individual products//
        //                foreach (clsPackageList productlist in lstPackageList)
        //                {

        //                    int productId = productlist.productId;
        //                    int quantity = productlist.guestQuantity;
        //                    //Added 
        //                    int additionalQuantity = 0;
        //                    int quantityFromSetup = 0;
        //                    //End
        //                    double price = 0.0;

        //                    //Modified on Dec-23-2015-- These 2 statements were below moved it here//
        //                    PackageProductId = productId;
        //                    PackageQty = quantity;
        //                    //Modified on Dec-23-2015//

        //                    List<ModifierProductInformation> modifierProductInformationList = GetProductModifierInformationList(this.purchasedProductSelected, PackageProductId);

        //                    //Begin-Added to check if attraction product has Card Sale checked on Jan-28-2016//
        //                    DataTable dtPackageProduct = trxUtils.getProductDetails(productId, card);
        //                    //end -Added to check if attraction product has Card Sale checked on Jan-28-2016//

        //                    if (productlist.remarks == null)
        //                        Message = "";
        //                    else
        //                        Message = productlist.remarks.ToString();

        //                    //Begin: 18-Dec-2015 to get the setup quantity
        //                    if (productlist.priceInclusive == "Y")
        //                    {
        //                        object o = Utilities.executeScalar(@"select  ISNULL(Quantity, 0) Quantity 
        //                                                             from ComboProduct 
        //                                                             where ChildProductId = @productId 
        //                                                                and Product_Id = @BookingProductId 
        //                                                                and AdditionalProduct ='N' ",
        //                                           new SqlParameter("@productId", productId),
        //                                           new SqlParameter("@BookingProductId", productlist.bookingProductId));

        //                        log.LogVariableState("@productId", productId);
        //                        log.LogVariableState("@BookingProductId", productlist.bookingProductId);

        //                        if (o != null)
        //                            quantityFromSetup = Convert.ToInt32(o);
        //                        //Modified on Dec-23-2015- code was below moved it here//
        //                        //Begin: Added to check if Quantity is greater than the Quantity set up in the product set up on Dec-18-2015//
        //                        if (quantity > quantityFromSetup)
        //                        {
        //                            additionalQuantity = quantity - quantityFromSetup;
        //                            quantity = quantityFromSetup;
        //                        }
        //                        else if (quantity < quantityFromSetup)
        //                        {
        //                            additionalQuantity = 0;
        //                        }
        //                        price = 0;
        //                        //End: Added to check if Quantity is greater than the Quantity set up in the product set up on Dec-18-2015//
        //                        //Modified on Dec-23-2015//
        //                    }
        //                    //End
        //                    else
        //                        price = -1;

        //                    //Chekcs if list has products of type combo
        //                    if (productlist.productType == "COMBO")
        //                    {
        //                        Transaction.TransactionLine parentTrxLine = new Transaction.TransactionLine();
        //                        int ComboQuantity = quantity;

        //                        //Convert the  category products list to list ok key value pair
        //                        List<KeyValuePair<int, int>> CategorySelectedProducts = new List<KeyValuePair<int, int>>();

        //                        foreach (Reservation.clsSelectedCategoryProducts currSelectedProducts in CategoryProducts)
        //                        {
        //                            if (currSelectedProducts.parentComboProductId == productId)
        //                            {
        //                                CategorySelectedProducts.Add(new KeyValuePair<int, int>(currSelectedProducts.productId, currSelectedProducts.quantity));
        //                            }
        //                        }
        //                        //end//

        //                        //Create transaction for combo products

        //                        DataTable dtAttractionChild = Utilities.executeDataTable(@"select ChildProductId, cp.Quantity, cp.id, pt.cardSale, p.AutoGenerateCardNumber
        //                                                                         from ComboProduct cp, products p, product_type pt
        //                                                                         where cp.Product_id = @productId
        //                                                                         and p.product_id = ChildProductId
        //                                                                         and p.product_type_id = pt.product_type_id
        //                                                                         and cp.Quantity > 0
        //                                                                         and pt.product_type = 'ATTRACTION'",
        //                                                                         new SqlParameter("@productId", productId));

        //                        log.LogVariableState("@productId", productId);

        //                        DataTable dtCardProducts = Utilities.executeDataTable(@"select p.product_name, p.product_id, p.QuantityPrompt, isnull(cp.Quantity, 0) quantity, pt.product_type, p.AutoGenerateCardNumber
        //                                                                            from ComboProduct cp, products p, product_type pt
        //                                                                            where cp.Product_id = @productId
        //                                                                            and p.product_id = ChildProductId
        //                                                                            and p.product_type_id = pt.product_type_id
        //                                                                            and cp.Quantity > 0
        //                                                                            and pt.product_type in ('NEW', 'CARDSALE', 'RECHARGE', 'GAMETIME')
        //                                                                            order by case when pt.product_type = 'NEW' then 0 else 1 end",
        //                                                                            new SqlParameter("@productId", productId));

        //                        log.LogVariableState("@productId", productId);

        //                        List<Card> cardList = new List<Card>();
        //                        bool autoGenCardNumber = false;
        //                        if (dtAttractionChild.Rows.Count > 0)
        //                        {
        //                            bool cardSale = dtAttractionChild.Rows[0]["CardSale"].ToString().Equals("Y");
        //                            autoGenCardNumber = dtAttractionChild.Rows[0]["AutoGenerateCardNumber"].ToString().Equals("Y");

        //                            foreach (DataRow dr in dtAttractionChild.Rows)
        //                            {
        //                                getAttractionCards(productId, Convert.ToInt32(dr["Id"]), lstAttractionProductList, cardList, cardSale, autoGenCardNumber);
        //                            }
        //                        }

        //                        int qty = 0;
        //                        List<Transaction.ComboCardProduct> cardProductList = new List<Transaction.ComboCardProduct>();

        //                        int newCardCount = 0;
        //                        //Populate the cardProductList with child product details
        //                        foreach (DataRow dr in dtCardProducts.Rows)
        //                        {
        //                            qty = Convert.ToInt32(dr["quantity"]);
        //                            if (qty > 0)
        //                            {
        //                                Transaction.ComboCardProduct cpDetails = new Transaction.ComboCardProduct();
        //                                cpDetails.ChildProductId = Convert.ToInt32(dr["product_id"]);
        //                                cpDetails.ChildProductName = dr["product_name"].ToString();
        //                                cpDetails.ComboProductId = productId;
        //                                cpDetails.ChildProductType = dr["product_type"].ToString();
        //                                cpDetails.Quantity = qty;
        //                                cardProductList.Add(cpDetails);
        //                                bool cardAutoGen = dr["AutoGenerateCardNumber"].ToString().Equals("Y");
        //                                if (autoGenCardNumber == cardAutoGen)
        //                                {
        //                                    if (cpDetails.ChildProductType.Equals("NEW"))
        //                                    {
        //                                        if (newCardCount == 0) // one NEW card and attraction schedules will share same card
        //                                        {
        //                                            newCardCount++;
        //                                            if (cardList.Count > 0)
        //                                            {
        //                                                foreach (Card existingCard in cardList)
        //                                                {
        //                                                    cpDetails.CardNumbers.Add(existingCard.CardNumber);
        //                                                }
        //                                            }
        //                                            else
        //                                            {
        //                                                for (int j = 0; j < PackageQty; j++)
        //                                                {
        //                                                    string cardNumber = Utilities.GenerateRandomCardNumber();
        //                                                    if (!cardAutoGen)
        //                                                        cardNumber = "T" + cardNumber.Substring(1);
        //                                                    cpDetails.CardNumbers.Add(cardNumber);
        //                                                    cardList.Add(new Card(cardNumber, Utilities.ParafaitEnv.LoginID, Utilities));
        //                                                }
        //                                            }
        //                                        }
        //                                        else // other new cards will get new card numbers
        //                                        {
        //                                            for (int j = 0; j < PackageQty; j++)
        //                                            {
        //                                                string cardNumber = Utilities.GenerateRandomCardNumber();
        //                                                if (!cardAutoGen)
        //                                                    cardNumber = "T" + cardNumber.Substring(1);
        //                                                cpDetails.CardNumbers.Add(cardNumber);
        //                                                cardList.Add(new Card(cardNumber, Utilities.ParafaitEnv.LoginID, Utilities));
        //                                            }
        //                                        }
        //                                    } // use same cards for other card product types
        //                                    else
        //                                    {
        //                                        foreach (Card existingCard in cardList)
        //                                        {
        //                                            cpDetails.CardNumbers.Add(existingCard.CardNumber);
        //                                        }
        //                                    }
        //                                } // autogen is not matching, so create new card numbers
        //                                else
        //                                {
        //                                    for (int j = 0; j < PackageQty; j++)
        //                                    {
        //                                        string cardNumber = Utilities.GenerateRandomCardNumber();
        //                                        if (!cardAutoGen)
        //                                            cardNumber = "T" + cardNumber.Substring(1);
        //                                        cpDetails.CardNumbers.Add(cardNumber);
        //                                        cardList.Add(new Card(cardNumber, Utilities.ParafaitEnv.LoginID, Utilities));
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        foreach (Card c in cardList)
        //                            Cards.Add(c.CardNumber);

        //                        if (cardProductList.Count > 0)
        //                        {
        //                            foreach (Transaction.ComboCardProduct cpDetail in cardProductList)
        //                            {
        //                                if (cpDetail.ChildProductType.ToString().Equals("RECHARGE") && (newCardCount <= 0))//Added new card Count Check if New card and recharge product exists in combo 
        //                                {
        //                                    for (int cardIndex = 0; cardIndex < PackageQty; cardIndex++)
        //                                    {
        //                                        //Create a card Deposit line if combo child product is of type recharge//
        //                                        if (Trx.createTransactionLine(new Card(cpDetail.CardNumbers[cardIndex], Utilities.ParafaitEnv.LoginID, Utilities), Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref Message) != 0)
        //                                        {
        //                                            log.LogVariableState("Message ", Message);
        //                                            log.LogMethodExit(null);
        //                                            return null;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }

        //                        int comboLineId = -1;
        //                        if (Trx.CreateComboProduct(productId, price, ComboQuantity, ref Message, parentTrxLine, cardProductList, CategorySelectedProducts) == 0)
        //                        {
        //                            if (dtAttractionChild.Rows.Count > 0)
        //                            {
        //                                comboLineId = Trx.TrxLines.IndexOf(parentTrxLine);
        //                                createComboAttractionLine(Trx, dtAttractionChild, comboLineId, lstAttractionProductList, parentTrxLine, card, ComboQuantity, productId, 0);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            log.LogVariableState("Message", Message);
        //                            log.LogMethodExit(null);
        //                            return null;
        //                        }

        //                        parentTrxLine = new Transaction.TransactionLine();

        //                        if (additionalQuantity != 0)
        //                        {
        //                            foreach (Transaction.ComboCardProduct cardProduct in cardProductList)
        //                            {
        //                                cardProduct.CardNumbers.RemoveRange(0, ComboQuantity);
        //                            }

        //                            if (Trx.CreateComboProduct(productId, -1, additionalQuantity, ref Message, parentTrxLine, cardProductList, CategorySelectedProducts) == 0)
        //                            {
        //                                if (dtAttractionChild.Rows.Count > 0)
        //                                {
        //                                    comboLineId = Trx.TrxLines.IndexOf(parentTrxLine);
        //                                    createComboAttractionLine(Trx, dtAttractionChild, comboLineId, lstAttractionProductList, parentTrxLine, card, additionalQuantity, productId, 0);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                log.LogVariableState("Message ", Message);
        //                                log.LogMethodExit(null);
        //                                return null;
        //                            }
        //                        }
        //                    }
        //                    else if (productlist.productType == "ATTRACTION")
        //                    {
        //                        if (quantity > 0)
        //                        {
        //                            getAttractionCards(productId, -1, lstAttractionProductList, null,
        //                                dtPackageProduct.Rows[0]["CardSale"].ToString().Equals("Y"),
        //                                dtPackageProduct.Rows[0]["AutoGenerateCardnumber"].ToString().Equals("Y"));

        //                            int balQty = quantity;
        //                            foreach (KeyValuePair<AttractionBooking, int> attList in lstAttractionProductList)
        //                            {
        //                                if (attList.Value == productId && balQty > 0)
        //                                {
        //                                    int qtyToBook = Math.Min(attList.Key.AttractionBookingDTO.BookedUnits, balQty);
        //                                    balQty -= qtyToBook;
        //                                    if (Trx.CreateAttractionProduct(productId, price, qtyToBook, -1, attList.Key, attList.Key.cardList, ref Message) != 0)
        //                                    {
        //                                        log.LogVariableState("Message", Message);
        //                                        log.LogMethodExit(null);
        //                                        return null;
        //                                    }
        //                                    attList.Key.AttractionBookingDTO.BookedUnits -= qtyToBook;
        //                                    attList.Key.cardList.RemoveRange(0, qtyToBook);
        //                                }
        //                            }

        //                            if (additionalQuantity > 0)
        //                            {
        //                                balQty = additionalQuantity;
        //                                foreach (KeyValuePair<AttractionBooking, int> attList in lstAttractionProductList)
        //                                {
        //                                    if (attList.Value == productId && balQty > 0)
        //                                    {
        //                                        int qtyToBook = Math.Min(attList.Key.AttractionBookingDTO.BookedUnits, balQty);
        //                                        balQty -= qtyToBook;
        //                                        if (Trx.CreateAttractionProduct(productId, -1, qtyToBook, -1, attList.Key, attList.Key.cardList, ref Message) != 0)
        //                                        {
        //                                            log.LogVariableState("Message", Message);
        //                                            log.LogMethodExit(null);
        //                                            return null;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //End Modification- Jan-20-2016 - Added to create transaction line Attraction not part of combo//
        //                    else
        //                        //Create transaction for items other than combo within the list
        //                        if (productlist.productType != "NEW" && productlist.productType != "RECHARGE"
        //                            && productlist.productType != "GAMETIME" && productlist.productType != "CARDSALE")
        //                    {
        //                        //Begin Modification 18-Dec-2015- Create the Manual Product line with quantity to be charged//
        //                        if (additionalQuantity != 0)
        //                        {
        //                            while (additionalQuantity > 0)
        //                            {
        //                                //Added on 27-oct-2016 for creating trx line in loop
        //                                additionalQuantity--;
        //                                Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
        //                                //int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true, SqlTransaction sqlTrx = null)
        //                                // int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message)
        //                                if (Trx.createTransactionLine(card, productId, -1, 1, ref Message, outTrxLine) != 0)
        //                                {
        //                                    additionalQuantity = 0;
        //                                    log.LogVariableState("Message", Message);
        //                                    log.LogMethodExit(null);
        //                                    return null;
        //                                } //end
        //                                modifierProductInformationList = LinkTrxLineWithModifierProductInfoList(modifierProductInformationList, outTrxLine, true, PackageProductId);
        //                            }
        //                        }

        //                        //Begin Modification 18-Dec-2015-Create the Manual Product line with quantity with zero price//
        //                        while (quantity > 0)
        //                        {
        //                            quantity--;
        //                            Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
        //                            //int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true, SqlTransaction sqlTrx = null)
        //                            if (Trx.createTransactionLine(card, productId, price, 1, ref Message, outTrxLine) != 0)
        //                            {
        //                                quantity = 0;
        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(null);
        //                                return null;
        //                            }
        //                            modifierProductInformationList = LinkTrxLineWithModifierProductInfoList(modifierProductInformationList, outTrxLine, true, PackageProductId);
        //                        }

        //                        if (modifierProductInformationList != null && modifierProductInformationList.Count > 0)
        //                        {
        //                            //Starts - Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //                            //foreach (PurchasedProducts purchasedProduct in purchasedProductSelected)
        //                            //    CreateTransactionModifierLine(Trx, purchasedProduct);
        //                            //update here ******
        //                            foreach (ModifierProductInformation item in modifierProductInformationList)
        //                            {
        //                                if (item.productTrxLine != null)
        //                                {
        //                                    CreateTransactionModifierLine(Trx, item.productModifierInformation.Key, item.productTrxLine);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //if recharge/New/Gametime/ Card Sale products are selected create TCards here
        //                        int lclQty = PackageQty;

        //                        int newCardFlag = 0;//used for Checking Multiple New card Product  
        //                        int baseCardIndex = 0;


        //                        //Begin: Modified the New card checking in PackageList on 27-Sep-2016 
        //                        foreach (clsPackageList productlists in lstPackageList)
        //                        {
        //                            if (productlists.productType.Equals("NEW"))
        //                            {
        //                                newCardFlag++;
        //                            }
        //                        }
        //                        if (newCardFlag > 1)
        //                        {
        //                            Message = "Multiple new card products are not allowed";

        //                            log.LogVariableState("Message ", Message);
        //                            log.LogMethodExit(null);
        //                            return null;//error should not allow to use multiple new card products
        //                        }

        //                        List<Card> cardList = getCards(trxUtils.getProductDetails(productId, null).Rows[0]["AutogenerateCardNumber"].ToString().Equals("Y"), PackageQty);

        //                        foreach (DataRow dr in dtProds.Rows)
        //                        {
        //                            if (Convert.ToInt32(dr["product_id"]) == productId)
        //                            {
        //                                while (lclQty > 0)
        //                                {

        //                                    int ret = -1;
        //                                    Card lclCard = cardList[baseCardIndex++];

        //                                    if (dr["product_type"].ToString().Equals("RECHARGE") && !(newCardFlag > 0)) //Added new card Count Check if New card and recharge product exists 
        //                                    {
        //                                        ret = Trx.createTransactionLine(lclCard, Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref Message);
        //                                        if (ret != 0)
        //                                        {
        //                                            log.LogVariableState("Message", Message);
        //                                            log.LogMethodExit(null);
        //                                            return null;
        //                                        }
        //                                    }

        //                                    if (lclQty >= quantity)
        //                                        ret = Trx.createTransactionLine(lclCard, productId, -1, 1, ref Message);
        //                                    else
        //                                        ret = Trx.createTransactionLine(lclCard, productId, price, 1, ref Message);

        //                                    if (ret != 0)
        //                                    {
        //                                        log.LogVariableState("Message", Message);
        //                                        log.LogMethodExit(null);
        //                                        return null;
        //                                    }

        //                                    Trx.updateAmounts();
        //                                    lclQty--;
        //                                }
        //                            }
        //                        }
        //                        //End: Modified for Generate Card Number based on guest Count on 17-Jun-2016
        //                    }
        //                }
        //                //End:Create the transaction line for products listed part of booking Product selected. lstPackageList will have list of Combo and Individual products//
        //                List<AdditionalAttractionProductInfo> additionalAttractionProductInfo = GetAdditionalAttractionProductInfo(lstAdditionalAttractionProductsList);
        //                //Create the transaction line for Additional products  selected. lstBookingProducts will have list of additional products//
        //                foreach (clsProductList addonProduct in lstAddonProducts)
        //                {
        //                    int productId = addonProduct.ProductId;
        //                    int quantity = Convert.ToInt32(addonProduct.Quantity);
        //                    string productType = addonProduct.productType.ToString();
        //                    double price;
        //                    PackageQty = quantity;
        //                    if (addonProduct.Remarks == null)
        //                        Message = "";
        //                    else
        //                        Message = addonProduct.Remarks.ToString();
        //                    price = Convert.ToDouble(addonProduct.Price);

        //                    //begin-Added to check if Attraction prodcut has card sale checked-on Jan-28-2016//
        //                    DataTable dtAddonProduct = trxUtils.getProductDetails(productId, card);
        //                    List<ModifierProductInformation> modifierProductInformationList = GetProductModifierInformationList(this.purchasedAdditionalProduct, productId);

        //                    //Create transaction for additional products
        //                    if (addonProduct.productType.ToString() != "NEW" && addonProduct.productType.ToString() != "RECHARGE"
        //                            && addonProduct.productType.ToString() != "GAMETIME" && addonProduct.productType.ToString() != "CARDSALE"
        //                            && addonProduct.productType.ToString() != "ATTRACTION")
        //                    {
        //                        while (quantity > 0)
        //                        {
        //                            quantity--;
        //                            Transaction.TransactionLine outTrxLine = new Transaction.TransactionLine();
        //                            //int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true, SqlTransaction sqlTrx = null)
        //                            // int createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, ref string message)
        //                            if (Trx.createTransactionLine(card, productId, price, 1, ref Message, outTrxLine) != 0)
        //                            {
        //                                quantity = 0;

        //                                log.LogVariableState("Message", Message);
        //                                log.LogMethodExit(null);
        //                                return null;
        //                            }
        //                            modifierProductInformationList = LinkTrxLineWithModifierProductInfoList(modifierProductInformationList, outTrxLine, false, productId);
        //                        }

        //                        //Begin: Modification for adding product Modifiers in Transaction on 04-Oct-2016
        //                        //if (this.purchasedAdditionalProduct != null && this.purchasedAdditionalProduct.Count > 0)
        //                        //{
        //                        //Starts - Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //                        //foreach (PurchasedProducts additionalPurchasedProduct in purchasedAdditionalProduct)
        //                        //    CreateTransactionModifierLine(Trx, additionalPurchasedProduct);
        //                        if (modifierProductInformationList != null && modifierProductInformationList.Count > 0)
        //                        {
        //                            foreach (ModifierProductInformation item in modifierProductInformationList)
        //                            {
        //                                if (item.productTrxLine != null)
        //                                {
        //                                    CreateTransactionModifierLine(Trx, item.productModifierInformation.Key, item.productTrxLine);
        //                                }
        //                            }
        //                        }
        //                        //}
        //                        //End: Modification for adding product Modifiers in Transaction on 04-Oct-2016
        //                    }
        //                    //End Modification- Jan-20-2016 - Added to create transaction line for Attraction part of additional product List//
        //                    else if (addonProduct.productType.ToString() == "ATTRACTION")
        //                    {
        //                        getAttractionCards(productId, -1, lstAdditionalAttractionProductsList, null,
        //                            dtAddonProduct.Rows[0]["CardSale"].ToString().Equals("Y"),
        //                            dtAddonProduct.Rows[0]["AutoGenerateCardnumber"].ToString().Equals("Y"));

        //                        int balQty = quantity;
        //                        foreach (AdditionalAttractionProductInfo attProduct in additionalAttractionProductInfo)
        //                        {
        //                            if (attProduct.productId == productId && balQty > 0 && attProduct.processed == false)
        //                            {
        //                                int qtyToBook = Math.Min(attProduct.AdditionalAttractionProductInformation.Key.AttractionBookingDTO.BookedUnits, balQty);
        //                                balQty -= qtyToBook;
        //                                if (Trx.CreateAttractionProduct(productId, price, qtyToBook, -1, attProduct.AdditionalAttractionProductInformation.Key, attProduct.AdditionalAttractionProductInformation.Key.cardList, ref Message) != 0)
        //                                {
        //                                    log.LogVariableState("Message ", Message);
        //                                    log.LogMethodExit(null);
        //                                    return null;
        //                                }
        //                                attProduct.processed = true;
        //                            }
        //                        }
        //                    }
        //                    //End Modification- Jan-20-2016 - Added to create transaction line for Attraction part o additional product List//
        //                    //if recharge/New/Gametime/ Card Sale products are selected create TCards here
        //                    else
        //                    {
        //                        //Begin Modification -Jan-27-2016-Added to check if Auo generate Card Number is checked//
        //                        //TransactionUtils trxUtils = new TransactionUtils(Utilities);
        //                        //Begin: Updated for adding RECHARGE and CARD SALE to Existing Package Product on 28-Jul-2016
        //                        int newCardFlag = 0;//used for Checking Multiple New cards
        //                        int baseCardIndex = 0;

        //                        //Begin: Modification done for checking new card inside a Combo product on 12-Oct-2017                       
        //                        foreach (clsPackageList productlist in lstPackageList)
        //                        {
        //                            int cpProductId = productlist.productId;
        //                            if (productlist.productType == "COMBO")
        //                            {
        //                                DataTable dtCardProducts = Utilities.executeDataTable(@"select p.product_name, p.product_id, p.QuantityPrompt, isnull(cp.Quantity, 0) quantity, pt.product_type
        //                                                                            from ComboProduct cp, products p, product_type pt
        //                                                                            where cp.Product_id = @productId
        //                                                                            and p.product_id = ChildProductId
        //                                                                            and p.product_type_id = pt.product_type_id
        //                                                                            and cp.Quantity > 0
        //                                                                            and pt.product_type in ('NEW')",
        //                                                                            new SqlParameter("@productId", cpProductId));

        //                                log.LogVariableState("@productId", cpProductId);

        //                                foreach (DataRow dr in dtCardProducts.Rows)
        //                                {
        //                                    newCardFlag++;
        //                                }
        //                            }
        //                        }
        //                        //End: Modification done for checking new card inside a Combo product on 12-Oct-2017

        //                        foreach (DataRow dr in dtProds.Rows)
        //                        {
        //                            int lclQty = PackageQty;

        //                            if (dr["product_type"].ToString().Equals("NEW"))
        //                            {
        //                                newCardFlag++;
        //                            }

        //                            //Begin:Commented for allowing the new card in additional part on 27-Sep-2016 
        //                            //if (newCardFlag > 1)
        //                            //{
        //                            //    Message = "Multiple new card products are not allowed";
        //                            //    return null;//error should not allow to use multiple new card products
        //                            //}
        //                            //End:Commented for allowing the new card in additional part on 27-Sep-2016 

        //                            if (Convert.ToInt32(dr["product_id"]) == productId)
        //                            {
        //                                while (lclQty > 0)
        //                                {
        //                                    int ret = -1;

        //                                    if (dr["product_type"].ToString().Equals("NEW"))
        //                                    {
        //                                        string cardNumber = createNewCard(dtAddonProduct);

        //                                        Card lclCard = new Card(cardNumber, User, Utilities);

        //                                        ret = Trx.createTransactionLine(lclCard, (int)dr["Product_Id"], price, 1, ref Message);
        //                                        if (ret != 0)
        //                                        {
        //                                            log.LogVariableState("Message", Message);
        //                                            log.LogMethodExit(null);
        //                                            return null;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        //RECHARGE /CARD SALE products
        //                                        if (newCardFlag > 0 && Cards.Count != 0)
        //                                        {
        //                                            //Begin: check Additional Product RECHARGE /CARD SALE Quantity is more than guest count
        //                                            if (Cards.Count < PackageQty)
        //                                            {
        //                                                for (int j = 0; j < (PackageQty - Cards.Count); j++)
        //                                                {
        //                                                    string cardNumber = createNewCard(dtAddonProduct);
        //                                                    Cards.Add(cardNumber);

        //                                                    Card lclNewCard = new Card(cardNumber, User, Utilities);
        //                                                    ret = Trx.createTransactionLine(lclNewCard, Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref Message);
        //                                                    if (ret != 0)
        //                                                    {
        //                                                        log.LogVariableState("Message", Message);
        //                                                        log.LogMethodExit(null);
        //                                                        return null;
        //                                                    }
        //                                                }
        //                                            }
        //                                            //Ends: check Additional Product RECHARGE /CARD SALE Quantity is more than guest count

        //                                            Card lclCard = new Card(Cards[baseCardIndex++], User, Utilities);
        //                                            ret = Trx.createTransactionLine(lclCard, (int)dr["Product_Id"], -1, 1, ref Message);//Modified to show recharge/card sale product price on 31-Oct-2017
        //                                            if (ret != 0)
        //                                            {
        //                                                log.LogVariableState("Message ", Message);
        //                                                log.LogMethodExit(null);
        //                                                return null;
        //                                            }
        //                                        }
        //                                        else
        //                                        {
        //                                            //There are no NEW CARD products present in Package Products 
        //                                            string cardNumber = createNewCard(dtAddonProduct);

        //                                            Card lclCard = new Card(cardNumber, User, Utilities);
        //                                            if (dr["product_type"].ToString().Equals("RECHARGE"))
        //                                            {
        //                                                ret = Trx.createTransactionLine(lclCard, Utilities.ParafaitEnv.CardDepositProductId, 0, 1, ref Message);
        //                                                if (ret != 0)
        //                                                {
        //                                                    log.LogVariableState("Message ", Message);
        //                                                    log.LogMethodExit(null);
        //                                                    return null;
        //                                                }
        //                                            }

        //                                            ret = Trx.createTransactionLine(lclCard, (int)dr["Product_Id"], price, 1, ref Message);
        //                                            if (ret != 0)
        //                                            {
        //                                                log.LogVariableState("Message ", Message);
        //                                                log.LogMethodExit(null);
        //                                                return null;
        //                                            }
        //                                        }
        //                                    }

        //                                    Trx.updateAmounts();
        //                                    lclQty--;
        //                                }
        //                            }
        //                        }
        //                        //Ends: Updated for adding RECHARGE and CARD SALE to Existing Package Product on 28-Jul-2016
        //                    }
        //                }

        //                /*Added CreateReservationDiscount to apply manual discount to the Transaction. Added just before the method returns Trx, 
        //                to also include discounts if package has card products as package contents.*/
        //                log.LogVariableState("Message ", Message);
        //                // Apply Discount Coupons
        //                if (!String.IsNullOrEmpty(DiscountCouponCode))
        //                {
        //                    string[] discoupons = DiscountCouponCode.ToString().Split('|');
        //                    foreach (string coupon in discoupons)
        //                    {
        //                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(coupon, null);
        //                        if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
        //                        {
        //                            discountCouponsBL.ValidateCouponApplication();
        //                            Trx.ApplyCoupon(coupon);
        //                        }
        //                    }
        //                }
        //                // ENDS Apply Discount Coupons

        //                CreateReservationDiscount(Trx);
        //                return Trx;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Error occured while creating the reservation transaction", ex);
        //                log.LogMethodExit("Throwing Exception - " + ex);
        //                throw ex;
        //            }
        //            finally
        //            {
        //                Utilities.ParafaitEnv.IsClientServer = savIsClientServer;
        //            }
        //        }

        //        private List<AdditionalAttractionProductInfo> GetAdditionalAttractionProductInfo(List<KeyValuePair<AttractionBooking, int>> lstAdditionalAttractionProductsList)
        //        {
        //            log.LogMethodEntry();
        //            List<AdditionalAttractionProductInfo> additionalAttractionProductInfoList = new List<AdditionalAttractionProductInfo>();
        //            if (lstAdditionalAttractionProductsList != null)
        //            {
        //                foreach (KeyValuePair<AttractionBooking, int> item in lstAdditionalAttractionProductsList)
        //                {
        //                    AdditionalAttractionProductInfo additionalAttractionProductInfo = new AdditionalAttractionProductInfo();
        //                    additionalAttractionProductInfo.productId = item.Value;
        //                    additionalAttractionProductInfo.AdditionalAttractionProductInformation = item;
        //                    additionalAttractionProductInfo.processed = false;
        //                    additionalAttractionProductInfoList.Add(additionalAttractionProductInfo);
        //                }
        //            }
        //            log.LogMethodExit(additionalAttractionProductInfoList);
        //            return additionalAttractionProductInfoList;
        //        }

        //        private List<ModifierProductInformation> LinkTrxLineWithModifierProductInfoList(List<ModifierProductInformation> modifierProductInformationList, Transaction.TransactionLine outTrxLine, bool packageItem, int productId)
        //        {
        //            log.LogMethodEntry(modifierProductInformationList, outTrxLine, packageItem, productId);
        //            if (modifierProductInformationList != null)
        //            {
        //                foreach (ModifierProductInformation item in modifierProductInformationList)
        //                {
        //                    if (item.productTrxLine != null)
        //                    {
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        log.LogVariableState("item", item);
        //                        item.productTrxLine = outTrxLine;
        //                        if (packageItem)
        //                        {
        //                            RemoveEntryFromPackageModifierList(productId);
        //                        }
        //                        else
        //                        {
        //                            RemoveEntryFromAdditionProdModifierList(productId);
        //                        }
        //                        break;
        //                    }
        //                }
        //            }
        //            log.LogMethodExit();
        //            return modifierProductInformationList;
        //        }

        //        private void RemoveEntryFromAdditionProdModifierList(int productId)
        //        {
        //            log.LogMethodEntry(productId);
        //            for (int i = 0; i < this.purchasedAdditionalProduct.Count - 1; i++)
        //            {
        //                if (this.purchasedAdditionalProduct[i].Value == productId)
        //                {
        //                    this.purchasedAdditionalProduct.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //            log.LogMethodExit();
        //        }

        //        private void RemoveEntryFromPackageModifierList(int productId)
        //        {
        //            log.LogMethodEntry(productId);
        //            for (int i = 0; i < this.purchasedProductSelected.Count - 1; i++)
        //            {
        //                if (this.purchasedProductSelected[i].Value == productId)
        //                {
        //                    this.purchasedProductSelected.RemoveAt(i);
        //                    break;
        //                }
        //            }
        //            log.LogMethodExit();
        //        }

        //        private static List<ModifierProductInformation> GetProductModifierInformationList(List<KeyValuePair<PurchasedProducts, int>> purchasedProductSelected, int PackageProductId)
        //        {
        //            log.LogMethodEntry(purchasedProductSelected, PackageProductId);
        //            List<ModifierProductInformation> modifierProductInformationList = new List<ModifierProductInformation>();

        //            if (purchasedProductSelected != null)
        //            {
        //                List<KeyValuePair<PurchasedProducts, int>> modifierItemsForProduct = purchasedProductSelected.FindAll(prod => prod.Value == PackageProductId);
        //                if (modifierItemsForProduct != null)
        //                {
        //                    foreach (KeyValuePair<PurchasedProducts, int> item in modifierItemsForProduct)
        //                    {
        //                        ModifierProductInformation productmodifierInformation = new ModifierProductInformation
        //                        {
        //                            productId = PackageProductId,
        //                            productModifierInformation = item,
        //                            productTrxLine = null
        //                        };
        //                        modifierProductInformationList.Add(productmodifierInformation);
        //                    }
        //                }
        //            }
        //            log.LogMethodExit(modifierProductInformationList);
        //            return modifierProductInformationList;
        //        }



        //        //End: Booking Related changes on 21-Aug-2015

        //        //Starts - Modification for F&B Restructuring of modifiers on 17-Oct-2018
        //        private void CreateTransactionModifierLine(Transaction trx, PurchasedProducts purchasedSelectedProduct, Transaction.TransactionLine parentTrxLine)
        //        {
        //            log.LogMethodEntry(trx, purchasedSelectedProduct, parentTrxLine);
        //            if (purchasedSelectedProduct.PurchasedModifierSetDTOList != null &&
        //                purchasedSelectedProduct.PurchasedModifierSetDTOList.Count > 0)
        //            {
        //                int count = 0;

        //                foreach (PurchasedModifierSet modifierSet in purchasedSelectedProduct.PurchasedModifierSetDTOList)
        //                {
        //                    if (modifierSet != null && modifierSet.PurchasedProductsList != null && modifierSet.PurchasedProductsList.Count > 0)
        //                    {

        //                        if (modifierSet.FreeQuantity > 0 && modifierSet.PurchasedProductsList != null)
        //                        {
        //                            foreach (PurchasedProducts purchasedProducts in modifierSet.PurchasedProductsList)
        //                            {
        //                                if (count < modifierSet.FreeQuantity)
        //                                {
        //                                    purchasedProducts.Price = 0;
        //                                    count++;
        //                                }
        //                                else
        //                                    break;
        //                            }
        //                        }

        //                        foreach (PurchasedProducts localProductDTO in modifierSet.PurchasedProductsList)
        //                        {
        //                            int lclChildProduct = Convert.ToInt32(localProductDTO.ProductId);

        //                            Transaction.TransactionLine newModifierLine = new Transaction.TransactionLine();
        //                            newModifierLine.ModifierLine = true;
        //                            string message = "";
        //                            //createTransactionLine(Card inCard, int productId, double in_price, decimal in_quantity, TransactionLine ParentLine, ref string message, TransactionLine outTrxLine = null, bool CreateChildLines = true)
        //                            if (trx.createTransactionLine(null, lclChildProduct, (double)localProductDTO.Price, 1, parentTrxLine, ref message, newModifierLine) != 0)
        //                            {
        //                                log.Error("Error craeting the transaction line of Product : " + localProductDTO.ProductName);
        //                            }
        //                            else
        //                            {
        //                                if (localProductDTO.PurchasedModifierSetDTOList != null && localProductDTO.PurchasedModifierSetDTOList.Count > 0)
        //                                {
        //                                    CreateTransactionModifierLine(trx, localProductDTO, newModifierLine);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            log.LogMethodExit(null);
        //        }
        //        //Ends - Modification for F&B Restructuring of modifiers on 17-Oct-2018

        //        public bool cancelTransaction(object TrxId, ref string Message)
        //        {
        //            log.LogMethodEntry(TrxId, Message);

        //            if (TrxId != DBNull.Value)
        //            {
        //                TransactionUtils trxUtils = new TransactionUtils(Utilities);
        //                Transaction trx = trxUtils.CreateTransactionFromDB((int)TrxId, Utilities);

        //                foreach (Transaction.TransactionLine tl in trx.TrxLines)
        //                {
        //                    tl.ReceiptPrinted = false;
        //                    tl.AllowEdit = true;
        //                }
        //                if (!trx.cancelTransaction(ref Message))
        //                {
        //                    log.LogVariableState("Message", Message);
        //                    log.LogMethodExit(false);
        //                    return false;
        //                }
        //                else
        //                {
        //                    log.LogVariableState("Message", Message);
        //                    log.LogMethodExit(true);
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                log.LogVariableState("Message", Message);
        //                log.LogMethodExit(true);
        //                return true;
        //            }
        //        }

        //        //Begin Modification -Dec-30-2015- Added additional Parameters to customer Screen//
        //        public void UpdateReservation(string BookingName, string Remarks, string User, string Channel, CustomerDTO customerDTO)
        //        {
        //            log.LogMethodEntry(BookingName, Remarks, User, customerDTO);

        //            if (BookingId <= 0)
        //            {
        //                log.LogMethodExit(null);
        //                return;
        //            }

        //            using (SqlConnection sqlConnection = Utilities.createConnection())
        //            {
        //                SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
        //                try
        //                {
        //                    CustomerBL customerBL = new CustomerBL(Utilities.ExecutionContext, customerDTO);
        //                    customerBL.Save(sqlTransaction);

        //                    Utilities.executeScalar(@"update Bookings set 
        //                                         [BookingName] = @BookingName
        //                                        ,[CustomerId] = @CustomerId
        //                                        ,[CustomerName] = @CustomerName
        //                                        ,[Remarks] = @Remarks
        //                                        ,[Channel] = @Channel
        //                                        ,[LastUpdatedBy] = @LastUpdatedBy
        //                                        ,[LastUpdatedDate] = getdate()
        //                                        where BookingId = @BookingId", sqlTransaction,
        //                                            new SqlParameter("@BookingId", BookingId),
        //                                            new SqlParameter("@CustomerId", customerDTO.Id),
        //                                            new SqlParameter("@CustomerName", customerDTO.FirstName),
        //                                            new SqlParameter("@Channel", Channel),
        //                                            new SqlParameter("@BookingName", BookingName),
        //                                            new SqlParameter("@Remarks", Remarks),
        //                                            new SqlParameter("@LastUpdatedBy", User)
        //                                            );
        //                    //Begin: To update the customers table inacse the booking was edited//

        //                    log.LogVariableState("@BookingId", BookingId);
        //                    log.LogVariableState("@BookingName", BookingName);
        //                    log.LogVariableState("@Channel", Channel);
        //                    log.LogVariableState("@Remarks", Remarks);
        //                    log.LogVariableState("@LastUpdatedBy", User);
        //                    log.LogVariableState("@CustomerId", customerDTO.Id);
        //                    log.LogVariableState("@CustomerName", customerDTO.FirstName);
        //                    sqlTransaction.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    sqlTransaction.Rollback();
        //                    log.Error("Error occured while saving the customer", ex);
        //                    throw ex;
        //                }
        //                finally
        //                {
        //                    sqlConnection.Close();
        //                }
        //            }

        //            log.LogMethodExit();
        //        }

        //        void getAttractionCards(int productId, int comboChildId, List<KeyValuePair<AttractionBooking, int>> lstAttractionProducts, List<Card> existingCards, bool cardSale, bool autoGen)
        //        {
        //            if (cardSale)
        //            {
        //                // if combo has both attraction and card products, load them all onto same cards provided autogen attributes are matching

        //                int cardIndex = 0;
        //                foreach (KeyValuePair<AttractionBooking, int> keyVal in lstAttractionProducts)
        //                {
        //                    if (keyVal.Value == productId && keyVal.Key.AttractionBookingDTO.Identifier == comboChildId)
        //                    {
        //                        // card numbers not yet allocated
        //                        if (keyVal.Key.cardNumberList.Count == 0)
        //                        {
        //                            // check if existing cards exist to allocate
        //                            if (existingCards != null && existingCards.Count > 0 && existingCards.Count > cardIndex
        //                                && existingCards[0].CardNumber.StartsWith("T") == !autoGen)
        //                            {
        //                                for (int i = 0; i < keyVal.Key.AttractionBookingDTO.BookedUnits; i++)
        //                                {
        //                                    keyVal.Key.cardList.Add(existingCards[cardIndex]);
        //                                    keyVal.Key.cardNumberList.Add(existingCards[cardIndex++].CardNumber);
        //                                }
        //                            }
        //                            else // if not get new ones
        //                            {
        //                                keyVal.Key.cardList = getCards(autoGen, keyVal.Key.AttractionBookingDTO.BookedUnits);
        //                                foreach (Card card in keyVal.Key.cardList)
        //                                    keyVal.Key.cardNumberList.Add(card.CardNumber);

        //                                if (existingCards != null)
        //                                    existingCards.AddRange(keyVal.Key.cardList);
        //                            }
        //                        } // card numbers exist. create card object list
        //                        else
        //                        {
        //                            if (keyVal.Key.cardList.Count > 0) // object lsit exists. just add to existing list
        //                            {
        //                                if (existingCards != null)
        //                                {
        //                                    foreach (Card card in keyVal.Key.cardList)
        //                                    {
        //                                        if (existingCards.Contains(card) == false)
        //                                            existingCards.Add(card);
        //                                    }
        //                                }
        //                            }
        //                            else // create cards from card numbers
        //                            {
        //                                foreach (string cardNumber in keyVal.Key.cardNumberList)
        //                                {
        //                                    Card card = null;
        //                                    if (existingCards != null)
        //                                        card = existingCards.Find(x => x.CardNumber == cardNumber);
        //                                    if (card == null)
        //                                    {
        //                                        card = new Card(cardNumber, Utilities.ParafaitEnv.Username, Utilities);
        //                                        if (existingCards != null)
        //                                            existingCards.Add(card);
        //                                    }
        //                                    keyVal.Key.cardList.Add(card);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        List<Card> getCards(bool autoGen, int Quantity)
        //        {
        //            List<Card> cardList = new List<Card>();
        //            while (Quantity-- > 0)
        //            {
        //                string cardNumber = Utilities.GenerateRandomCardNumber();
        //                if (!autoGen)
        //                    cardNumber = "T" + cardNumber.Substring(1);

        //                Card card = new Card(cardNumber, Utilities.ParafaitEnv.Username, Utilities);
        //                cardList.Add(card);
        //            }

        //            return cardList;
        //        }

        //        void createComboAttractionLine(Transaction Trx, DataTable dtAttractionChild, int comboLineId,
        //                                   List<KeyValuePair<AttractionBooking, int>> lstAttractionProductList,
        //                                   Transaction.TransactionLine parentTrxLine, Card card, int Quantity, int productId, double Price)
        //        {
        //            log.LogMethodEntry(Trx, comboLineId, lstAttractionProductList, parentTrxLine, Quantity, productId, Price);

        //            int lineId = -1;
        //            string Message = "";

        //            foreach (DataRow dr in dtAttractionChild.Rows)
        //            {
        //                int balQuantity = Quantity;
        //                foreach (KeyValuePair<AttractionBooking, int> attList in lstAttractionProductList)
        //                {
        //                    if (attList.Value == productId && attList.Key.AttractionBookingDTO.Identifier == Convert.ToInt32(dr["Id"]) && attList.Key.AttractionBookingDTO.BookedUnits > 0 && balQuantity > 0)
        //                    {
        //                        int qtyToBook = Math.Min(balQuantity, attList.Key.AttractionBookingDTO.BookedUnits);
        //                        balQuantity -= qtyToBook;

        //                        lineId = Trx.TrxLines.Count - 1;
        //                        if (Trx.CreateAttractionProduct(Convert.ToInt32(dr["ChildProductId"]), Price, qtyToBook, comboLineId, attList.Key, attList.Key.cardList, ref Message) != 0)
        //                        {
        //                            while (lineId < Trx.TrxLines.Count)
        //                            {
        //                                if (Trx.TrxLines[lineId].DBLineId <= 0)
        //                                    Trx.TrxLines[lineId].LineValid = false;
        //                                lineId++;
        //                            }
        //                            Trx.updateAmounts();

        //                            log.LogMethodExit(null);
        //                            throw new ApplicationException(Message);
        //                        }
        //                        else
        //                        {
        //                            attList.Key.AttractionBookingDTO.BookedUnits -= qtyToBook;
        //                            attList.Key.cardList.RemoveRange(0, qtyToBook);

        //                            while (++lineId < Trx.TrxLines.Count)
        //                            {
        //                                Trx.TrxLines[lineId].UserPrice = false;
        //                                Trx.TrxLines[lineId].ComboChildLine = true;
        //                                Trx.TrxLines[lineId].AllowEdit = false;
        //                                Trx.TrxLines[lineId].AllowCancel = false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            log.LogMethodExit(null);
        //        }
        //        //End Modification-Jan-20-2016-Added to create transaction line for Attraction Product Part of combo//

        //        //Begin: Added for creating NEW CARD on 04-Aug-2016       
        //        public string createNewCard(DataTable AutoGenerateCardNumber)
        //        {
        //            log.LogMethodEntry(AutoGenerateCardNumber);

        //            string cardNumber = Utilities.GenerateRandomCardNumber();
        //            if (AutoGenerateCardNumber.Rows.Count > 0)
        //            {
        //                if (AutoGenerateCardNumber.Rows[0]["AutoGenerateCardNumber"].ToString() != "Y")
        //                {
        //                    cardNumber = "T" + cardNumber.Substring(1);
        //                }
        //            }

        //            log.LogMethodExit(CardNumber);
        //            return cardNumber;
        //        }
        //        //End: Added for creating NEW CARD on 04-Aug-2016

        //        public int getFacilityBookings(int BookingId, int AttractionScheduleId, DateTime ReservationFromDate, DateTime ReservationToDate)
        //        {
        //            log.LogMethodEntry(BookingId, AttractionScheduleId, ReservationFromDate, ReservationToDate);

        //            try
        //            {
        //                int bookedFacilityQty = Convert.ToInt32(Utilities.executeScalar(@"select isnull(sum(quantity),0)
        //                                                                     from bookings
        //                                                                     where AttractionScheduleId in (select ats.AttractionScheduleId 
        //                                                                                                      from AttractionSchedules  ats, attractionSchedules as1
        //                                                                                                     where ats.FacilityId = as1.FacilityId
        //								                                                                       and as1.AttractionScheduleId = @AttractionScheduleId
        //								                                                                   )
        //                                                                        and ((status in ( 'BOOKED','BLOCKED') and (ExpiryTime is null or ExpiryTime > getdate()))
        //                                                                            or status in ('CONFIRMED', 'COMPLETE'))
        //                                                                        and ((@TimeFrom < FromDate and @TimeTo > ToDate)
        //                                                                            or (@TimeFrom >= FromDate and @TimeFrom < ToDate)
        //                                                                            or (@TimeTo > FromDate and @TimeTo <= ToDate))
        //	                                                                    and bookingId != @BookingId",
        //                                                                new SqlParameter[] {new SqlParameter("@AttractionScheduleId", AttractionScheduleId),
        //                                                                                    new SqlParameter("@BookingId", BookingId),
        //                                                                                    new SqlParameter("@TimeFrom", ReservationFromDate),
        //                                                                                    new SqlParameter("@TimeTo", ReservationToDate)}));

        //                log.LogMethodExit(bookedFacilityQty);
        //                return bookedFacilityQty;
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error("Error occured while getting facility bookings", ex);
        //                log.LogMethodExit(-1);
        //                return -1;
        //            }
        //        }
        //        /// <summary>
        //        /// BlockReservationSchedule
        //        /// </summary>
        //        /// <param name="reservationDTO"></param>
        //        public void BlockReservationSchedule(ReservationDTO reservationDTO)
        //        {
        //            log.LogMethodEntry(reservationDTO);
        //            if (reservationDTO.BookingId == -1 || reservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString())
        //            {
        //                reservationDTO.ExpiryTime = Utilities.getServerTime().AddMinutes(15);
        //                reservationDTO.Status = ReservationDTO.ReservationStatus.BLOCKED.ToString();
        //                Save();
        //            }
        //            log.LogMethodExit();
        //        }
        //        /// <summary>
        //        /// GetPurchasedPackageProducts for the booking
        //        /// </summary>
        //        /// <param name="bookingId"></param>
        //        /// <returns></returns>
        //        public DataTable GetPurchasedPackageProducts(int bookingId)
        //        {
        //            log.LogMethodEntry(bookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            DataTable dtPurchasedProducts = reservationDatahandler.GetPurchasedPackageProducts(bookingId);
        //            log.LogMethodExit(dtPurchasedProducts);
        //            return dtPurchasedProducts;
        //        }

        //        /// <summary>
        //        /// GetServiceChargeProductDetails for the booking
        //        /// </summary>
        //        /// <param name="bookingId"></param>
        //        /// <returns></returns>
        //        public DataTable GetServiceChargeProductDetails(int bookingId)
        //        {
        //            log.LogMethodEntry(bookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            DataTable dt = reservationDatahandler.GetServiceChargeProductDetails(bookingId);
        //            log.LogMethodExit(dt);
        //            return dt;
        //        }

        //        /// <summary>
        //        /// GetSelectedAdditionalBookingProducts for the booking
        //        /// </summary>
        //        /// <param name="bookingId"></param>
        //        /// <returns></returns>
        //        public DataTable GetSelectedAdditionalBookingProducts(int bookingId)
        //        {
        //            log.LogMethodEntry(bookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            DataTable dt = reservationDatahandler.GetSelectedAdditionalBookingProducts(bookingId);
        //            log.LogMethodExit(dt);
        //            return dt;
        //        }

        //        /// <summary>
        //        /// GetSelectedPackageBookingProducts for the booking
        //        /// </summary>
        //        /// <param name="bookingId"></param>
        //        /// <returns></returns>
        //        public DataTable GetSelectedPackageBookingProducts(int bookingId)
        //        {
        //            log.LogMethodEntry(bookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            DataTable dt = reservationDatahandler.GetSelectedPackageBookingProducts(bookingId);
        //            log.LogMethodExit(dt);
        //            return dt;
        //        }
        //        /// <summary>
        //        /// UpdateBookingId details
        //        /// </summary>
        //        /// <param name="editedBookingId"></param>
        //        /// <param name="newBookingId"></param>
        //        public void UpdateBookingId(int editedBookingId, int newBookingId)
        //        {
        //            log.LogMethodEntry(editedBookingId, newBookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            reservationDatahandler.UpdateBookingId(editedBookingId, newBookingId);
        //            log.LogMethodExit();
        //        }

        //        /// <summary>
        //        /// GenerateAttractionProductlist
        //        /// </summary>
        //        /// <param name="packageProducts"></param>
        //        /// <param name="bookingId"></param>
        //        /// <returns></returns>
        //        public List<KeyValuePair<AttractionBooking, int>> GenerateAttractionProductlist(bool packageProducts, int bookingId)
        //        {
        //            log.LogMethodEntry();
        //            List<KeyValuePair<AttractionBooking, int>> lstAttractionProductslist = new List<KeyValuePair<AttractionBooking, int>>();
        //            if (BookingId != -1)
        //            {
        //                ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //                DataTable dtAttractionPackageProducts = reservationDatahandler.GetBookedAttractionProducts(bookingId, packageProducts);
        //                if (dtAttractionPackageProducts.Rows.Count > 0)
        //                {
        //                    DataTable dtTrxAttractionSchedules = reservationDatahandler.GetBookingAttractionSchedules(bookingId);
        //                    if (dtTrxAttractionSchedules.Rows.Count > 0)
        //                    {
        //                        List<KeyValuePair<string, string>> cardList = new List<KeyValuePair<string, string>>();
        //                        for (int trxAtLines = 0; trxAtLines < dtAttractionPackageProducts.Rows.Count; trxAtLines++)
        //                        {
        //                            string _sqlWhere = "ParentProductId = " + dtAttractionPackageProducts.Rows[trxAtLines]["ChildProductId"].ToString();
        //                            DataRow[] filteredRows = dtTrxAttractionSchedules.Select(_sqlWhere);
        //                            if (filteredRows != null && filteredRows.Count() > 0)
        //                            {
        //                                DataTable dtSelectedTrxAttractions = filteredRows.CopyToDataTable();
        //                                List<KeyValuePair<AttractionBooking, int>> atbKeyPairLst = BuildAttractionProductObject(dtSelectedTrxAttractions, cardList);
        //                                lstAttractionProductslist.AddRange(atbKeyPairLst);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            log.LogMethodExit(lstAttractionProductslist);
        //            return lstAttractionProductslist;
        //        }

        //        /// <summary>
        //        /// GenerateAttractionProductlist
        //        /// </summary>
        //        /// <param name="packageProducts"></param>
        //        /// <param name="bookingId"></param>
        //        /// <param name="parentProductId"></param>
        //        /// <param name="parentLineId"></param>
        //        /// <returns></returns>
        //        public List<KeyValuePair<AttractionBooking, int>> GenerateAttractionProductlist(bool packageProducts, int bookingId, int parentProductId, int parentLineId)
        //        {
        //            log.LogMethodEntry();
        //            List<KeyValuePair<AttractionBooking, int>> lstAttractionProductslist = new List<KeyValuePair<AttractionBooking, int>>();
        //            if (BookingId != -1)
        //            {
        //                ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //                DataTable dtAttractionPackageProducts = reservationDatahandler.GetBookedAttractionProducts(bookingId, packageProducts);
        //                if (dtAttractionPackageProducts.Rows.Count > 0)
        //                {
        //                    DataTable dtTrxAttractionSchedules = reservationDatahandler.GetBookingAttractionSchedules(bookingId);
        //                    if (dtTrxAttractionSchedules.Rows.Count > 0)
        //                    {
        //                        List<KeyValuePair<string, string>> cardList = new List<KeyValuePair<string, string>>();
        //                        for (int trxAtLines = 0; trxAtLines < dtAttractionPackageProducts.Rows.Count; trxAtLines++)
        //                        {
        //                            string _sqlWhere = "ParentProductId = " + parentProductId.ToString() + " and LineId = " + parentLineId.ToString();
        //                            DataRow[] filteredRows = dtTrxAttractionSchedules.Select(_sqlWhere);
        //                            if (filteredRows != null && filteredRows.Count() > 0)
        //                            {
        //                                DataTable dtSelectedTrxAttractions = filteredRows.CopyToDataTable();
        //                                List<KeyValuePair<AttractionBooking, int>> atbKeyPairLst = BuildAttractionProductObject(dtSelectedTrxAttractions, cardList);
        //                                lstAttractionProductslist.AddRange(atbKeyPairLst);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            log.LogMethodExit(lstAttractionProductslist);
        //            return lstAttractionProductslist;
        //        }

        //        private List<KeyValuePair<AttractionBooking, int>> BuildAttractionProductObject(DataTable dtSelectedTrxAttractions, List<KeyValuePair<string, string>> cardList)
        //        {
        //            log.LogMethodEntry(dtSelectedTrxAttractions);
        //            List<KeyValuePair<AttractionBooking, int>> lstAttractionProductslist = new List<KeyValuePair<AttractionBooking, int>>();
        //            for (int rowNum = 0; rowNum < dtSelectedTrxAttractions.Rows.Count; rowNum++)
        //            {

        //                AttractionBookingDTO attractionBookingDTO = new AttractionBookingDTO();
        //                List<string> cardNumberList = null;
        //                attractionBookingDTO.AttractionPlayId = (dtSelectedTrxAttractions.Rows[rowNum]["AttractionPlayId"] != DBNull.Value
        //                                               ? Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["AttractionPlayId"])
        //                                               : -1);
        //                attractionBookingDTO.AttractionPlayName = dtSelectedTrxAttractions.Rows[rowNum]["AttractionPlayName"].ToString();
        //                attractionBookingDTO.AttractionScheduleId = (dtSelectedTrxAttractions.Rows[rowNum]["AttractionScheduleId"] != DBNull.Value
        //                                               ? Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["AttractionScheduleId"])
        //                                               : -1);
        //                attractionBookingDTO.ScheduleTime = Convert.ToDateTime(dtSelectedTrxAttractions.Rows[rowNum]["ScheduleTime"]);
        //                attractionBookingDTO.BookedUnits = Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["BookedUnits"]);
        //                // atb.AvailableUnits = schedule.AvailableUnits;
        //                attractionBookingDTO.Price = Convert.ToDouble(dtSelectedTrxAttractions.Rows[rowNum]["tlPrice"]);
        //                attractionBookingDTO.ExpiryDate = DateTime.MinValue;
        //                attractionBookingDTO.PromotionId = (dtSelectedTrxAttractions.Rows[rowNum]["promotion_id"] != DBNull.Value
        //                                              ? Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["promotion_id"])
        //                                              : -1);
        //                if (Convert.ToBoolean(dtSelectedTrxAttractions.Rows[rowNum]["hasSeats"]) == true)
        //                {
        //                    //need tp create AttractionBookingSeats class and use the method from that class
        //                    SqlParameter[] seatBookingsParam = new SqlParameter[] { new SqlParameter("@SeatBookingId", dtSelectedTrxAttractions.Rows[rowNum]["SeatBookingId"]) };
        //                    DataTable dtTrxAttraactionSeating = Utilities.executeDataTable(@"SELECT  atbs.SeatId, facs.SeatName
        //                                                                                                             from AttractionBookingSeats atbs, FacilitySeats facs 
        //                                                                                                            where atbs.BookingId = @SeatBookingId
        //                                                                                                              and facs.SeatId = atbs.SeatId ", seatBookingsParam);
        //                    if (dtTrxAttraactionSeating.Rows.Count > 0)
        //                    {
        //                        //List<int> seatIdList = new List<int>();
        //                        //List<string> seatNameList = new List<string>();
        //                        for (int rowNums = 0; rowNums < dtTrxAttraactionSeating.Rows.Count; rowNums++)
        //                        {
        //                            AttractionBookingSeatsDTO attractionBookingSeatsDTO = new AttractionBookingSeatsDTO();
        //                            attractionBookingSeatsDTO.SeatId = Convert.ToInt32(dtTrxAttraactionSeating.Rows[rowNums]["SeatId"]);
        //                            attractionBookingSeatsDTO.SeatName = dtTrxAttraactionSeating.Rows[rowNums]["SeatName"].ToString();
        //                            attractionBookingDTO.AttractionBookingSeatsDTOList.Add(attractionBookingSeatsDTO);
        //                            //seatIdList.Add(Convert.ToInt32(dtTrxAttraactionSeating.Rows[rowNums]["SeatId"]));
        //                            //seatNameList.Add(dtTrxAttraactionSeating.Rows[rowNums]["SeatName"].ToString());
        //                        }
        //                        //atb.SelectedSeats = seatIdList;
        //                        //atb.SelectedSeatNames = seatNameList;
        //                    }
        //                }

        //                attractionBookingDTO.ScheduleFromTime = Convert.ToDecimal(dtSelectedTrxAttractions.Rows[rowNum]["ScheduleFromTime"]);
        //                attractionBookingDTO.ScheduleToTime = Convert.ToDecimal(dtSelectedTrxAttractions.Rows[rowNum]["ScheduleToTime"]);
        //                if (dtSelectedTrxAttractions.Rows[rowNum]["card_id"] != DBNull.Value)
        //                {
        //                    string cardNumber = "";
        //                    if (cardList != null && cardList.Exists(cardKey => cardKey.Key == dtSelectedTrxAttractions.Rows[rowNum]["card_number"].ToString()))
        //                    {
        //                        cardNumber = cardList.Find(cardKey => cardKey.Key == dtSelectedTrxAttractions.Rows[rowNum]["card_number"].ToString()).Value;
        //                    }
        //                    else
        //                    {
        //                        //AutoGenerateCardNumber
        //                        cardNumber = Utilities.GenerateRandomCardNumber();
        //                        if (dtSelectedTrxAttractions.Rows[rowNum]["AutoGenerateCardNumber"] != DBNull.Value
        //                             && dtSelectedTrxAttractions.Rows[rowNum]["AutoGenerateCardNumber"].ToString() != "Y")
        //                        {
        //                            cardNumber = "T" + cardNumber.Substring(1);
        //                        }
        //                        cardList.Add(new KeyValuePair<string, string>(dtSelectedTrxAttractions.Rows[rowNum]["card_number"].ToString(), cardNumber));
        //                    }
        //                    cardNumberList = new List<string>(0) { cardNumber };
        //                }
        //                attractionBookingDTO.Identifier = (dtSelectedTrxAttractions.Rows[rowNum]["Identifier"] != DBNull.Value ? Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["Identifier"]) : -1);

        //                AttractionBooking atb = new AttractionBooking(Utilities.ExecutionContext, attractionBookingDTO);
        //                if (cardNumberList != null)
        //                {
        //                    atb.cardNumberList = cardNumberList;
        //                }
        //                lstAttractionProductslist.Add(new KeyValuePair<AttractionBooking, int>(atb, Convert.ToInt32(dtSelectedTrxAttractions.Rows[rowNum]["ParentProductId"])));
        //            }
        //            log.LogMethodExit(lstAttractionProductslist);
        //            return lstAttractionProductslist;
        //        }
        //        /// <summary>
        //        ///  GeneratePurchasedProductlist for the booking
        //        /// </summary>
        //        /// <param name="bookingId"></param>
        //        /// <param name="packageProducts">bool</param>
        //        /// <returns></returns>
        //        public List<KeyValuePair<PurchasedProducts, int>> GeneratePurchasedProductlist(int bookingId, bool packageProducts)
        //        {
        //            log.LogMethodEntry(packageProducts, bookingId);
        //            List<KeyValuePair<PurchasedProducts, int>> purchasedProducts = new List<KeyValuePair<PurchasedProducts, int>>();
        //            if (bookingId != -1)
        //            {

        //                ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //                DataTable dtModifierProducts = reservationDatahandler.GetBookingProductsWithModifiers(bookingId, packageProducts);
        //                if (dtModifierProducts.Rows.Count > 0)
        //                {
        //                    for (int i = 0; i < dtModifierProducts.Rows.Count; i++)
        //                    {
        //                        int parentProductId = Convert.ToInt32(dtModifierProducts.Rows[i]["ChildProductId"]);
        //                        int parentLineId = Convert.ToInt32(dtModifierProducts.Rows[i]["LineId"]);
        //                        DataTable dtTrxModifierProductLines = reservationDatahandler.GetBookedModiferProducts(parentProductId, parentLineId, BookingId);
        //                        if (dtTrxModifierProductLines.Rows.Count > 0)
        //                        {

        //                            for (int j = 0; j < dtTrxModifierProductLines.Rows.Count; j++)
        //                            {
        //                                PurchasedProducts selectedProduct = SetPurchasedProductList(dtTrxModifierProductLines.Rows[j], parentProductId, BookingId);
        //                                purchasedProducts.Add(new KeyValuePair<PurchasedProducts, int>(selectedProduct, parentProductId));
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //            log.LogMethodExit(purchasedProducts);
        //            return purchasedProducts;
        //        }

        //        public List<KeyValuePair<PurchasedProducts, int>> GeneratePurchasedProductlist(int bookingId, bool packageProducts, int parentProductId, int parentLineId)
        //        {
        //            log.LogMethodEntry(packageProducts, bookingId);
        //            List<KeyValuePair<PurchasedProducts, int>> purchasedProducts = new List<KeyValuePair<PurchasedProducts, int>>();
        //            if (bookingId != -1)
        //            {
        //                ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //                DataTable dtModifierProducts = reservationDatahandler.GetBookingProductsWithModifiers(bookingId, packageProducts);
        //                if (dtModifierProducts.Rows.Count > 0)
        //                {
        //                    string sqlWhere = "ChildProductId = " + parentProductId.ToString() + "and LineId = " + parentLineId.ToString();
        //                    DataRow[] selectedProductModifiers = dtModifierProducts.Select(sqlWhere);
        //                    if (selectedProductModifiers != null && selectedProductModifiers.Count() > 0)
        //                    {
        //                        DataTable dtSelected = selectedProductModifiers.CopyToDataTable();
        //                        for (int i = 0; i < dtSelected.Rows.Count; i++)
        //                        {
        //                            DataTable dtTrxModifierProductLines = reservationDatahandler.GetBookedModiferProducts(parentProductId, parentLineId, BookingId);
        //                            if (dtTrxModifierProductLines.Rows.Count > 0)
        //                            {
        //                                for (int j = 0; j < dtTrxModifierProductLines.Rows.Count; j++)
        //                                {
        //                                    PurchasedProducts selectedProduct = SetPurchasedProductList(dtTrxModifierProductLines.Rows[j], parentProductId, BookingId);
        //                                    purchasedProducts.Add(new KeyValuePair<PurchasedProducts, int>(selectedProduct, parentProductId));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            log.LogMethodExit(purchasedProducts);
        //            return purchasedProducts;
        //        }

        //        private PurchasedProducts SetPurchasedProductList(DataRow selectedModifierRow, int parentProduct, int bookingId)
        //        {
        //            log.LogMethodEntry(selectedModifierRow, parentProduct, bookingId);
        //            ReservationDatahandler reservationDatahandler = new ReservationDatahandler();
        //            Products products = new Products(parentProduct);
        //            PurchasedProducts purchasedProducts = products.GetPurchasedProducts();
        //            if (purchasedProducts.PurchasedModifierSetDTOList != null && purchasedProducts.PurchasedModifierSetDTOList.Count > 0)
        //            {

        //                foreach (PurchasedModifierSet item in purchasedProducts.PurchasedModifierSetDTOList)
        //                {
        //                    if (selectedModifierRow["ModifierSetId"] != DBNull.Value
        //                       && Convert.ToInt32(selectedModifierRow["ModifierSetId"]) == item.ModifierSetId
        //                       && selectedModifierRow["SetName"] != DBNull.Value
        //                       && selectedModifierRow["SetName"].ToString() == item.SetName)
        //                    {
        //                        int productId = Convert.ToInt32(selectedModifierRow["product_id"]);
        //                        int productLineId = Convert.ToInt32(selectedModifierRow["LineId"]);
        //                        Products modifierProducts = new Products(productId);
        //                        PurchasedProducts purchasedModifierProducts = modifierProducts.GetPurchasedProducts();
        //                        purchasedModifierProducts.Amount = Convert.ToDouble(selectedModifierRow["amount"]);
        //                        purchasedModifierProducts.Price = Convert.ToDouble(selectedModifierRow["tlprice"]);
        //                        purchasedModifierProducts.Remarks = selectedModifierRow["Remarks"].ToString();

        //                        DataTable dtTrxModifierProductLines = reservationDatahandler.GetBookedModiferProducts(productId, productLineId, bookingId);
        //                        if (dtTrxModifierProductLines.Rows.Count > 0)
        //                        {

        //                            for (int j = 0; j < dtTrxModifierProductLines.Rows.Count; j++)
        //                            {
        //                                PurchasedProducts selectedProduct = SetPurchasedProductList(dtTrxModifierProductLines.Rows[j], productId, bookingId);
        //                                if (selectedProduct.PurchasedModifierSetDTOList != null)
        //                                {
        //                                    if (purchasedModifierProducts.PurchasedModifierSetDTOList == null)
        //                                    {
        //                                        purchasedModifierProducts.PurchasedModifierSetDTOList = new List<PurchasedModifierSet>();
        //                                    }
        //                                    purchasedModifierProducts.PurchasedModifierSetDTOList.AddRange(selectedProduct.PurchasedModifierSetDTOList);
        //                                }
        //                            }
        //                        }
        //                        List<PurchasedModifierSet> innerPurchasedModifierSet = new List<PurchasedModifierSet>();
        //                        foreach (PurchasedModifierSet innerItem in purchasedModifierProducts.PurchasedModifierSetDTOList)
        //                        {
        //                            if (innerItem.PurchasedProductsList != null)
        //                            {
        //                                innerPurchasedModifierSet.Add(innerItem);
        //                            }
        //                        }
        //                        purchasedModifierProducts.PurchasedModifierSetDTOList = innerPurchasedModifierSet;
        //                        if (item.PurchasedProductsList == null)
        //                        {
        //                            item.PurchasedProductsList = new List<PurchasedProducts>();
        //                        }
        //                        item.PurchasedProductsList.Add(purchasedModifierProducts);
        //                    }
        //                }
        //                List<PurchasedModifierSet> purchasedModifierSet = new List<PurchasedModifierSet>();
        //                foreach (PurchasedModifierSet item in purchasedProducts.PurchasedModifierSetDTOList)
        //                {
        //                    if (item.PurchasedProductsList != null)
        //                    {
        //                        purchasedModifierSet.Add(item);
        //                    }
        //                }
        //                purchasedProducts.PurchasedModifierSetDTOList = purchasedModifierSet;
        //            }
        //            log.LogMethodExit(purchasedProducts);
        //            return purchasedProducts;
        //        }


    }

    /// <summary>
    /// Manages the list of Bookings
    /// </summary>
    public class ReservationListBLDecommissioned
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ReservationListBLDecommissioned(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetReservationDTOList
        /// </summary>
        /// <param name="searchParameters"></param> 
        /// <returns></returns>
        public List<ReservationDTO> GetReservationDTOList(List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            ReservationDatahandler reservationDatahandler = new ReservationDatahandler(null);
            List<ReservationDTO> returnValue = reservationDatahandler.GetReservationDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
