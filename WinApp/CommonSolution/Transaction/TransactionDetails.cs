/********************************************************************************************
 * Project Name - TransactionDetails Programs
 * Description  - TransactionDetails object of TransactionDetails
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *1.00        15-June-2016  Rakshith          Created 
 *2.70        25-Mar-2019   Guru S A          Booking Phase 2 enhancements
 *            30-Jul-2019   Jeevan            Booking Phase 2 enhancements included Remarks 
  *2.70.3      30-Mar-2020   Jeevan            Booking attende fixes , added attendee list property
 ********************************************************************************************/
using System;
using System.Collections.Generic; 

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///   TransactionDetails Class
    /// </summary>
    public class TransactionDetails
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private List<LinkedPurchasedProducts> orderedProducts;
        private TransactionUser transactionUser;
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
        private double advancePaid;
        private string discountSummary;
        private DateTime creationDate;
        private string waiverSignedSummary;
        private string taxSummary;
        private string transactionStatus;
        private string transactionOTP;
        private ReservationDTO reservationDTO;
        private string remarks;
        private List<BookingAttendeeDTO> bookingAttendeeList;
        private string lastFourDigitOfCC;
        private string creditCardType;
        private DateTime trxPaymentDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionDetails()
        {
            log.LogMethodEntry();
            orderedProducts = null; 
            reservationDTO = new ReservationDTO();
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
			transactionStatus = "";
			advancePaid = 0.0;
            this.discountSummary = "";
            this.waiverSignedSummary = "";
            this.taxSummary = "";
            this.transactionOTP ="";
            this.remarks = "";
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with  parameter
        /// </summary>
        public TransactionDetails(List<LinkedPurchasedProducts> selectedProducts)
        {
            log.LogMethodEntry();
            orderedProducts = selectedProducts;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Constructor with  parameter
        /// </summary>
        public TransactionDetails(int transactionId, DateTime transactionDate)
        {
            log.LogMethodEntry();
            this.transactionId = transactionId;
            this.transactionDate = transactionDate;
            log.LogMethodExit();

        }

        /// <summary>
        ///  Constructor with  parameter
        /// </summary>
        public TransactionDetails(int transactionId, DateTime transactionDate, double transactionTotal, double transactionTax) : this(transactionId, transactionDate)
        {
            log.LogMethodEntry();
            this.transactionId = transactionId;
            this.transactionDate = transactionDate;
            this.transactionTotal = transactionTotal;
            this.transactionTax = transactionTax; 
            log.LogMethodExit();

        }

        /// <summary>
        /// ProductLineLinker Class
        /// </summary>
        private class ProductLineLinker
        {
            int lineId;
            string productLineIdentifier;

            /// <summary>
            /// Default constructor
            /// </summary>
            public ProductLineLinker()
            {
                lineId = -1;
                productLineIdentifier = "";
            }

            /// <summary>
            ///  Constructor with  parameter
            /// </summary>
            public ProductLineLinker(int lineId, string productLineIdentifier)
            {
                this.lineId = lineId;
                this.productLineIdentifier = productLineIdentifier;
            }

            /// <summary>
            /// Get/Set method of the LineId field
            /// </summary>
            public int LineId { get { return lineId; } }

            /// <summary>
            /// Get/Set method of the ProductLineIdentifier field
            /// </summary>
            public string ProductLineIdentifier { get { return productLineIdentifier; } }

        }

        /// <summary>
        /// AddProduct method
        /// </summary>
        public void AddProduct(LinkedPurchasedProducts product)
        {
            if (orderedProducts == null)
                orderedProducts = new List<LinkedPurchasedProducts>();
            orderedProducts.Add(product);
        }
        /// <summary>
        /// Get/Set method of the TransactionId field
        /// </summary>
        public int TransactionId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the TransactionTotal field
        /// </summary>
        public double TransactionTotal { get { return transactionTotal; } set { transactionTotal = value; } }

        /// <summary>
        /// Get/Set method of the TransactionTax field
        /// </summary>
        public double TransactionTax { get { return transactionTax; } set { transactionTax = value; } }

        /// <summary>
        /// Get/Set method of the TransactionGrandTotal field
        /// </summary>
        public double TransactionGrandTotal { get { return transactionGrandTotal; } set { transactionGrandTotal = value; } }

        /// <summary>
        /// Get/Set method of the TransactionTotalWithoutTax field
        /// </summary>
        public double TransactionTotalWithoutTax { get { return transactionGrandTotal - transactionTax; }   }

        /// <summary>
        /// Get/Set method of the TransactionRoundOffAmount field
        /// </summary>
        public double TransactionRoundOffAmount { get { return transactionRoundOffTotal; } set { transactionRoundOffTotal = value; } }

        /// <summary>
        /// Get/Set method of the DiscountAmount field
        /// </summary>
        public double DiscountAmount { get { return discountAmount; } set { discountAmount = value; } }

        /// <summary>
        /// Get/Set method of the TransactionDate field
        /// </summary>
        public DateTime TransactionDate { get { return transactionDate; } set { transactionDate = value; } }

        /// <summary>
        /// Get/Set method of the TransactionReference field
        /// </summary>
        public string TransactionReference { get { return transactionReference; } set { transactionReference = value; } }

        /// <summary>
        /// Get/Set method of the Products field
        /// </summary>
        public List<LinkedPurchasedProducts> Products { get { return orderedProducts; } set { orderedProducts = value; } }

        /// <summary>
        /// Get/Set method of the LineId field
        /// </summary>
        public int LineId { get { return lineId; } set { lineId = value; } }

        /// <summary>
        /// Get/Set method of the PaymentMode field
        /// </summary>
        public string PaymentMode { get { return paymentMode; } set { paymentMode = value; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        public int CustomerId { get { return customerId; } set { customerId = value; } }

         /// <summary>
        /// Get/Set method of the Original_System_Reference field
        /// </summary>
        public string Original_System_Reference { get { return original_System_Reference; } set { original_System_Reference = value; } }

        /// Get/Set method of the transactionType field
        /// </summary>
        public string TransactionType { get { return transactionType; } set { transactionType = value; } }

        /// Get/Set method of the BookingId field
        /// </summary>
        public int BookingId { get { return bookingId; } set { bookingId = value; } }

        /// Get/Set method of the ReservationCode field
        /// </summary>
        public string ReservationCode { get { return reservationCode; } set { reservationCode = value; } }

        /// Get/Set method of the ReservationDateTime field
        /// </summary>
        public string ReservationDateTime { get { return reservationDateTime; } set { reservationDateTime = value; } }

        /// Get/Set method of the transactionType field
        /// </summary>
        public string Status { get { return status; } set { status = value; } }

        /// Get/Set method of the EditStatus field
        /// </summary>
        public string EditStatus { get { return editStatus; } set { editStatus = value; } }

		/// Get/Set method of the TransactionStatus field
		/// </summary>
		public string TransactionStatus { get { return transactionStatus; } set { transactionStatus = value; } }

		/// <summary>
		/// Get/Set method of the SiteId field
		/// </summary>
		public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the reservationDTO field
        /// </summary> 
        public ReservationDTO ReservationDTO { get { return reservationDTO; } set { reservationDTO = value; } }
        

        
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public TransactionUser TransactionUserDTO { get { return transactionUser; } set { transactionUser = value; } }

        /// <summary>
        /// Get/Set method of the AdvancePaid field
        /// </summary>
        public double AdvancePaid { get { return advancePaid; } set { advancePaid = value; } }

        /// <summary>
        /// Get/Set method of the DiscountSummary field
        /// </summary>
        public string DiscountSummary { get { return discountSummary; } set { discountSummary = value; } }

        /// <summary>
        ///// Get/Set method of the WaiverSignedSummary field
        ///// </summary>
        public string WaiverSignedSummary { get { return waiverSignedSummary; } set { waiverSignedSummary = value; } }

        /// <summary>
        /// Get/Set method of the TaxSummary field
        /// </summary>
        public string TaxSummary { get { return taxSummary; } set { taxSummary = value; } }


        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        
        /// <summary>
        /// Get/Set method of the TransactionOTP field
        /// </summary>
        public string TransactionOTP { get { return transactionOTP; } set { transactionOTP = value; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        public string Remarks { get { return remarks; } set { remarks = value; } }

        /// <summary>
        /// BookingAttendeeList
        /// </summary>
        public List<BookingAttendeeDTO> BookingAttendeeList { get { return bookingAttendeeList; } set { bookingAttendeeList = value; } }

        /// <summary>
        /// Get/Set method of the LastFourDigitOfCC field
        /// </summary>
        public string LastFourDigitOfCC { get { return lastFourDigitOfCC; } set { lastFourDigitOfCC = value; } }

        /// <summary>
        /// Get/Set method of the CreditCardType field
        /// </summary>
        public string CreditCardType { get { return creditCardType; } set { creditCardType = value; } }

        /// <summary>
        /// Get/Set method of the TrxPaymentDate field
        /// </summary>
        public DateTime TrxPaymentDate { get { return trxPaymentDate; } set { trxPaymentDate = value; } }
    }




    /// <summary>
    /// TransactionUser class. 
    /// </summary>
    public class TransactionUser
    {
        private int userid;
        private string loginid;
        private string username;
        private string mobileNo;
        private string isAgent ;
        private string email;

        /// <summary>
        /// Default Constructor 
        /// </summary>
        public TransactionUser()
        {
            this.userid = -1;
            this.IsAgent = "N";
            this.loginid = "";
            this.email = "";
        }


        /// <summary>
        ///   Constructor With parameter
        /// </summary>
        public TransactionUser(int userid, string username, string loginid, string mobileNo, string IsAgent, string email)
        {
            this.userid = userid;
            this.username = username;
            this.loginid = loginid;
            this.mobileNo = mobileNo;
            this.IsAgent = IsAgent;
            this.email = email;
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        public int UserId { get { return userid; } set { userid = value; } }


        /// <summary>
        /// Get/Set method of the Username field
        /// </summary>
        public string Username { get { return username; } set { username = value; } }


        // <summary>
        /// Get/Set method of the loginid field
        /// </summary>
        public string LoginId { get { return loginid; } set { loginid = value; } }


        /// <summary>
        /// Get/Set method of the MobileNo field
        /// </summary>
        public string MobileNo { get { return mobileNo; } set { mobileNo = value; } }


        /// <summary>
        /// Get/Set method of the IsAgent field
        /// </summary>
        public string IsAgent { get { return isAgent; } set { isAgent = value; } }


        /// <summary>
        /// Get/Set method of the Email field
        /// </summary>
        public string Email { get { return email; } set { email = value; } }



    }




    /// <summary>
    /// TransactionException Class
    /// </summary>
    public class TransactionException : Exception
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TransactionException()
        {
        }

        /// <summary>
        ///  constructor with Parameter
        /// </summary>
        public TransactionException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }
       
    }

}
