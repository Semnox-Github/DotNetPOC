/********************************************************************************************
 * Project Name - PurchasesListParams Program
 * Description  - Data object of Purchases List Params
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-May-2016   Jeevan          Created 
 *2.70.2        12-Aug-2019   Deeksha         Added logger methods.
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// This is the Purchase Display Filter  object class. This acts as data holder for the Purchase display list filter 
    /// </summary>
    public class PurchasesListParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string loginId;
        private int userId;
        private int customerId;
        private DateTime fromDate;
        private DateTime toDate;
        private bool showTransactionLines;
        private int transactionId;
        private string email;
        private string reservationCode;
        private string transactionType;
        private bool showAllTransaction;
        private bool showDiscountSummary;


        /// <summary>
        /// Default constructor
        /// </summary>
        public PurchasesListParams( )
        {
            log.LogMethodEntry();
            this.loginId = "";
            this.userId = -1;
            this.customerId = -1;
            fromDate =new DateTime(1900,01,01);
            toDate = new DateTime(2100, 01, 01);
            this.showTransactionLines = false;
            this.transactionId = -1;
            this.email = "";
            this.reservationCode = "";
            this.transactionType = "";
            this.showAllTransaction = true;
            this.showDiscountSummary = false;
            log.LogMethodExit();
        }

    
        /// <summary>
        /// Get/Set method of the LoginId field
        /// </summary>
        [DisplayName("LoginId")]
        [DefaultValue("")]
        public string LoginId { get { return loginId; } set { loginId = value;} }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        [DefaultValue(-1)]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Get/Set method of the CustomerId field
        /// </summary>
        [DisplayName("CustomerId")]
        [DefaultValue(-1)]
        public int CustomerId { get { return customerId; } set { customerId = value;  } }

        /// <summary>
        /// Get/Set method of the TranscationId field
        /// </summary>
        [DisplayName("TranscationId")]
        [DefaultValue(-1)]
        public int TranscationId { get { return transactionId; } set { transactionId = value; } }

        /// <summary>
        /// Get/Set method of the ReservationCode field
        /// </summary>
        [DisplayName("Email")]
        [DefaultValue("")]
        public string Email { get { return email; } set { email = value; } }

        /// <summary>
        /// Get/Set method of the ReservationCode field
        /// </summary>
        [DisplayName("ReservationCode")]
        [DefaultValue("")]
        public string ReservationCode { get { return reservationCode; } set { reservationCode = value; } }


        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        [DisplayName("FromDate")]
        [DefaultValue(typeof(DateTime),"")]
        public DateTime FromDate {  
            get
            {
            if (fromDate.Year == 1)
            {
                fromDate = new DateTime(1900, 1, 1);
            }
        
            return fromDate; } set { fromDate = value;  } }

        /// <summary>
        /// Get/Set method of the ToDate field
        /// </summary>
        [DisplayName("ToDate")]
        [DefaultValue(typeof(DateTime), "")]
        public DateTime ToDate { get { 
            if (toDate.Year == 1)
            {
                toDate= new DateTime(2100, 12, 31);
            }
            return toDate;

        } set { toDate = value;  } }


        /// <summary>
        /// Get/Set method of the TransactionType field
        /// </summary>
        [DisplayName("TransactionType")]
        [DefaultValue("")]
        public string TransactionType { get { return transactionType; } set { transactionType = value; } }


        /// <summary>
        /// Get/Set method of the ShowTranscationLines field
        /// </summary>
        [DisplayName("ShowTranscationLines")]
        public bool ShowTransactionLines { get { return showTransactionLines; } set { showTransactionLines = value; } }


        /// <summary>
        /// Get/Set method of the ShowAllTransaction field
        /// </summary>
        [DisplayName("ShowAllTransaction")]
        public bool ShowAllTransaction { get { return showAllTransaction; } set { showAllTransaction = value; } }


        /// <summary>
        /// Get/Set method of the ShowDiscountSummary field
        /// </summary>
        [DisplayName("ShowDiscountSummary")]
        public bool ShowDiscountSummary { get { return showDiscountSummary; } set { showDiscountSummary = value; } }
    }

}
