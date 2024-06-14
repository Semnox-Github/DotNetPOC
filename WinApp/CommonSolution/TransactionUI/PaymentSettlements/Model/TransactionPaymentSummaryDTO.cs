/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - TransactionPaymentSummaryDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     17-Sep-2021    Fiona                  Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TransactionUI
{
    public class TransactionPaymentSummaryDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int transactionId;
        private string transactionNumber;
        private DateTime transactionDate;
        private string customer;
        private string paymentMode;
        private double paymentAmount;
        private double tipAmount;
        private string paymentReference;
        private bool settled;
        private string cardNumber;
        private double totalAmount;
        private int paymentId;
        private bool isTipUpdateAllowed;
        
        public TransactionPaymentSummaryDTO(int transactionId, string transactionNo, DateTime transactionDate, string customer, string paymentMode, double paymentAmount, double tipAmount, string paymentReference, bool settled, string cardNumber, double totalAmount, int paymentId, bool isTipUpdateAllowed)
        {
            log.LogMethodEntry(transactionId, transactionNo, transactionDate, customer, paymentMode, paymentAmount, tipAmount, paymentReference, settled, paymentId, isTipUpdateAllowed);
            this.transactionId = transactionId;
            this.transactionNumber = transactionNo;
            this.transactionDate = transactionDate;
            this.customer = customer;
            this.paymentMode = paymentMode;
            this.paymentAmount = paymentAmount;
            this.tipAmount = tipAmount;
            this.paymentReference = paymentReference;
            this.settled = settled;
            this.cardNumber = cardNumber;
            this.totalAmount = totalAmount;
            this.paymentId = paymentId;
            this.isTipUpdateAllowed = isTipUpdateAllowed;
            log.LogMethodExit();
        }

        public int TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        public string TransactionNumber
        {
            get { return transactionNumber; }
            set { transactionNumber = value; }
        }
        public DateTime TransactionDate
        {
            get { return transactionDate; }
            set { transactionDate = value; }
        }
        public string Customer
        {
            get { return customer; }
            set { customer = value; }
        }
        public string PaymentMode
        {
            get { return paymentMode; }
            set { paymentMode = value; }
        }
        public double PaymentAmount
        {
            get { return paymentAmount; }
            set { paymentAmount = value; }
        }
        public double TipAmount
        {
            get { return tipAmount; }
            set { tipAmount = value; }
        }
        public string PaymentReference
        {
            get { return paymentReference; }
            set { paymentReference = value; }
        }
        public bool Settled
        {
            get { return settled; }
            set { settled = value; }
        }
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }
        public double TotalAmount
        {
            get { return totalAmount; }
            set { totalAmount = value; }
        }
        public int PaymentId
        {
            get { return paymentId; }
            set { paymentId = value; }
        }
        public bool IsTipUpdateAllowed
        {
            get { return isTipUpdateAllowed; }
            set { isTipUpdateAllowed = value; }
        }

    }
}
