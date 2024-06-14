using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    [Serializable]
    public class NetEpayBatchReportTransaction
    {
        private string isSaf;
        private string status;
        private string expDate;
        private string tranCode;
        private string cardType;
        private string accNo;
        private string accountSource;
        private string purchase;
        private string gratuity;
        private string authorize;
        private string authCode;
        private string refNo;
        private string transDateTime;
        private string invoiceNo;


        public string IsSaf
        {
            get
            {
                return isSaf;
            }
            set
            {
                isSaf = value;
            }
        }
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }
        public string ExpDate
        {
            get
            {
                return expDate;
            }
            set
            {
                expDate = value;
            }
        }
        public string TranCode
        {
            get
            {
                return tranCode;
            }
            set
            {
                tranCode = value;
            }
        }
        public string CardType
        {
            get
            {
                return cardType;
            }
            set
            {
                cardType = value;
            }
        }
        public string AccNo
        {
            get
            {
                return accNo;
            }
            set
            {
                accNo = value;
            }
        }
        public string AccountSource
        {
            get
            {
                return accountSource;
            }
            set
            {
                accountSource = value;
            }
        }
        public string Purchase
        {
            get
            {
                return purchase;
            }
            set
            {
                purchase = value;
            }
        }
        public string Gratuity
        {
            get
            {
                return gratuity;
            }
            set
            {
                gratuity = value;
            }
        }
        public string Authorize
        {
            get
            {
                return authorize;
            }
            set
            {
                authorize = value;
            }
        }
        public string AuthCode
        {
            get
            {
                return authCode;
            }
            set
            {
                authCode = value;
            }
        }
        public string RefNo
        {
            get
            {
                return refNo;
            }
            set
            {
                refNo = value;
            }
        }
        public string TransDateTime
        {
            get
            {
                return transDateTime;
            }
            set
            {
                transDateTime = value;
            }
        }
        public string InvoiceNo
        {
            get
            {
                return invoiceNo;
            }
            set
            {
                invoiceNo = value;
            }
        }
    }
}
