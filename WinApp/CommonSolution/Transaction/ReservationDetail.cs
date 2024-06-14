/********************************************************************************************
 * Class Name - Transaction                                                                        
 * Description - ReservationDetail 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Transaction
{

    public class ReservationDetail
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int transactionId;
        private DateTime transactionDate;
        private double transactionTotal;
        private double transactionTax;
        private double transactionGrandTotal;
        private double transactionRoundOffTotal;
        private double discountAmount;
        private string transactionReference;
        private int lineId;
        private string paymentMode;
        private string original_System_Reference;
        private string transactionType;
        private int customerId;
        private int bookingId;
        private string reservationCode;
        private string reservationDateTime;
        private string status;
        private int siteId;
        private string editStatus;




        /// <summary>
        /// Default constructor
        /// </summary>
        public ReservationDetail()
        {
            log.LogMethodEntry();
            transactionTotal = 0;
            transactionTax = 0;
            transactionGrandTotal = 0;
            transactionRoundOffTotal = 0;
            transactionId = -1;
            transactionReference = "";
            lineId = -1;
            paymentMode = "";
            customerId = -1;
            transactionType = "";
            bookingId = -1;
            reservationCode = "";
            reservationDateTime = "";
            status = "";
            siteId = -1;
            editStatus = "";
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set For TransactionId
        /// </summary>
        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set For TransactionDate
        /// </summary>
        public DateTime TransactionDate { get { return transactionDate; } set { transactionDate = value; } }

        /// <summary>
        /// Get/Set For transactionTotal
        /// </summary>
        public double TransactionTotal { get { return transactionTotal; } set { transactionTotal = value; } }

        /// <summary>
        /// Get/Set For transactionTax
        /// </summary>
        public double TransactionTax { get { return transactionTax; } set { transactionTax = value; } }

        /// <summary>
        /// Get/Set For transactionGrandTotal
        /// </summary>
        public double TransactionGrandTotal { get { return transactionGrandTotal; } set { transactionGrandTotal = value; } }

        /// <summary>
        /// Get/Set For transactionRoundOffTotal
        /// </summary>
        public double TransactionRoundOffTotal { get { return transactionRoundOffTotal; } set { transactionRoundOffTotal = value; } }

        /// <summary>
        /// Get/Set For discountAmount
        /// </summary>
        public double DiscountAmount { get { return discountAmount; } set { discountAmount = value; } }

        /// <summary>
        /// Get/Set For transactionReference
        /// </summary>
        public string TransactionReference { get { return transactionReference; } set { transactionReference = value; } }

        /// <summary>
        /// Get/Set For lineId
        /// </summary>
        public int LineId { get { return lineId; } set { lineId = value; } }

        /// <summary>
        /// Get/Set For paymentMode
        /// </summary>
        public string PaymentMode { get { return paymentMode; } set { paymentMode = value; } }


        /// <summary>
        /// Get/Set For original_System_Reference
        /// </summary>
        public string Original_System_Reference { get { return original_System_Reference; } set { original_System_Reference = value; } }


        /// <summary>
        /// Get/Set For transactionType
        /// </summary>
        public string TransactionType { get { return transactionType; } set { transactionType = value; } }

        /// <summary>
        /// Get/Set For customerId
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }

        /// <summary>
        /// Get/Set For bookingId
        /// </summary>
        public int BookingId { get { return bookingId; } set { bookingId = value; } }

        /// <summary>
        /// Get/Set For reservationCode
        /// </summary>
        public string ReservationCode { get { return reservationCode; } set { reservationCode = value; } }

        /// <summary>
        /// Get/Set For reservationDateTime
        /// </summary>
        public string ReservationDateTime { get { return reservationDateTime; } set { reservationDateTime = value; } }

        /// <summary>
        /// Get/Set For status
        /// </summary>
        public string Status { get { return status; } set { status = value; } }

        /// <summary>
        /// Get/Set For siteId
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set For editStatus
        /// </summary>
        public string EditStatus { get { return editStatus; } set { editStatus = value; } }

    }


}
