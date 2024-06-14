/* Project Name - ReservationCoreDTO Programs 
* Description  - Data object of the ReservationParams
* 
**************
**Version Log
**************
*Version     Date          Modified By        Remarks          
*********************************************************************************************
*1.00        7-Dec-2016    Jeevan             Created 
*2.50.0      28-Jan-2019   Guru S A          Booking changes
*2.70        25-Mar-2019      Guru S A       Booking Phase 2 enhancements
*2.70.2        12-Aug-2019   Deeksha           Added logger methods.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public class ReservationParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int trxId;
        int bookingId;
        int customerId;
        DateTime reservationDateTime;
        int duration;

        int userId;
        int site_id;
        string posIdentifier;
        string loginId;
        string status;
      

        double creditCardAmount;
        double debitCardAmount;
        string paymentCardNumber;
        string paymentReference;
        bool isEditedBooking;
        int editedBookingId;
        int paymentModeId;
        bool enableHistory;


        List<BookingProduct> bookingProductList; // used for Edit 
        List<AdditionalProduct> additionalProductList;
        List<BookingAttendeeDTO> bookingAttendeeList;
 
 
        /// <summary>
        /// Default constructor
        /// </summary>
        public ReservationParams()
        {
            log.LogMethodEntry();
            bookingProductList = new List<BookingProduct>();
            additionalProductList = new List<AdditionalProduct>();
            bookingAttendeeList = new List<BookingAttendeeDTO>();

            bookingId = -1;
            customerId = -1;
            userId = -1;
            this.site_id = -1;
            this.isEditedBooking = false;
            this.editedBookingId = -1;
            this.creditCardAmount = 0;
            this.debitCardAmount = 0;
            posIdentifier = "";
            loginId = "";
            status = "";
            trxId = -1;
            paymentModeId = -1;
            enableHistory = false;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("SiteId")]
        [DefaultValue(-1)]
        public int SiteId { get { return site_id; } set { site_id = value; } }

        /// <summary>
        /// Get/Set method of the SiteName field
        /// </summary>
        [DisplayName("SiteName")]
        public string SiteName { get; set; }


        /// <summary>
        /// Get/Set method of the TrxId field
        /// </summary>
        [DisplayName("TrxId")]
        public int TrxId { get { return trxId; } set { trxId = value; } }

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("BookingId")]
        public int BookingId { get { return bookingId; } set { bookingId = value; } }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        [DefaultValue(-1)]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set method of the PosIdentifier field
        /// </summary>
        [DisplayName("PosIdentifier")]
        public string PosIdentifier { get { return posIdentifier; } set { posIdentifier = value; } }

        /// <summary>
        /// Get/Set method of the Loginid field
        /// </summary>
        [DisplayName("LoginId")]
        public string LoginId { get { return loginId; } set { loginId = value; } }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
        [DefaultValue(-1)]
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        
        /// <summary>
        /// Get/Set method of the ReservationDateTime field
        /// </summary>
        [DisplayName("ReservationDateTime")]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime ReservationDateTime { get { return reservationDateTime; } set { reservationDateTime = value; } }

        /// <summary>
        /// Get/Set method of the Duration field
        /// </summary>
        [DisplayName("Duration")]
        [DefaultValue(-1)]
        public int Duration { get { return duration; } set { duration = value; } }


        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("CreditCardAmount")]
        [DefaultValue(0)]
        public double CreditCardAmount { get { return creditCardAmount; } set { creditCardAmount = value; } }


        /// <summary>
        /// Get/Set method of the DebitCardAmount field
        /// </summary>
        [DisplayName("DebitCardAmount")]
        [DefaultValue(0)]
        public double DebitCardAmount { get { return debitCardAmount; } set { debitCardAmount = value; } }


        /// <summary>
        /// Get/Set method of the PaymentCardNumber field
        /// </summary>
        [DisplayName("PaymentCardNumber")]
        public string PaymentCardNumber { get { return paymentCardNumber; } set { paymentCardNumber = value; } }


        /// <summary>
        /// Get/Set method of the DebitCardAmount field
        /// </summary>
        [DisplayName("PaymentReference")]
        public string PaymentReference { get { return paymentReference; } set { paymentReference = value; } }

        /// <summary>
        /// Get/Set method of the PaymentModeId field
        /// </summary>
        [DisplayName("PaymentModeId")]
        public int PaymentModeId { get { return paymentModeId; } set { paymentModeId = value; } }

        /// <summary>
        /// Get/Set method of the IsEditedBooking field
        /// </summary>
        [DisplayName("IsEditedBooking")]
        public bool IsEditedBooking { get { return isEditedBooking; } set { isEditedBooking = value; } }


        /// <summary>
        /// Get/Set method of the EditedBookingId field
        /// </summary>
        [DisplayName("EditedBookingId")]
        public int EditedBookingId { get { return editedBookingId; } set { editedBookingId = value; } }

        /// <summary>
        /// Get/Set method of the EnableHistory field
        /// </summary>
        [DisplayName("EnableHistory")]
        public bool EnableHistory { get { return enableHistory; } set { enableHistory = value; } }


        /// <summary>
        /// Get/Set method of the BookingProductList field
        /// </summary>
        [DisplayName("BookingProductList")]
        public List<BookingProduct> BookingProductList { get { return bookingProductList; } set { bookingProductList = value; } }


        /// <summary>
        /// Get/Set method of the AdditionalProductList field
        /// </summary>
        [DisplayName("AdditionalProductList")]
        public List<AdditionalProduct> AdditionalProductList { get { return additionalProductList; } set { additionalProductList = value; } }


        /// <summary>
        /// Get/Set method of the BookingAttendeeList field
        /// </summary>
        [DisplayName("BookingAttendeeList")]
        public List<BookingAttendeeDTO> BookingAttendeeList { get { return bookingAttendeeList; } set { bookingAttendeeList = value; } }

        
        /// <summary>
        /// Get/Set method of the DiscountCouponCode field
        /// </summary>
        [DisplayName("DiscountCouponCode")]
        public string DiscountCouponCode { get; set; }

    }
}
