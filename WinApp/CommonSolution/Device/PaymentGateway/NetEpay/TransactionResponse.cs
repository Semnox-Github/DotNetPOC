using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    [Serializable]
    public class TransactionResponse
    {
        private string merchantID;
        private string acctNo;
        private string expDate;
        private string cardType;
        private string tranCode;
        private string authCode;
        private string captureStatus;
        private string refNo;
        private string invoiceNo;
        private string operatorID;
        private string acqRefData;
        private string recordNo;
        private string entryMethod;
        private string applicationLabel;
        private string processData;
        private NetEpayAmount amount;
        private NetEpayAccount account;

        public string MerchantID
        {
            get
            {
                return merchantID;
            }
            set
            {
                merchantID = value;
            }
        }
        public string AcctNo
        {
            get
            {
                return acctNo;
            }
            set
            {
                acctNo = value;
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
        public string CaptureStatus
        {
            get
            {
                return captureStatus;
            }
            set
            {
                captureStatus = value;
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
        public string OperatorID
        {
            get
            {
                return operatorID;
            }
            set
            {
                operatorID = value;
            }
        }
        public string AcqRefData
        {
            get
            {
                return acqRefData;
            }
            set
            {
                acqRefData = value;
            }
        }
        public string RecordNo
        {
            get
            {
                return recordNo;
            }
            set
            {
                recordNo = value;
            }
        }
        public string EntryMethod
        {
            get
            {
                return entryMethod;
            }
            set
            {
                entryMethod = value;
            }
        }
        public string ApplicationLabel
        {
            get
            {
                return applicationLabel;
            }
            set
            {
                applicationLabel = value;
            }
        }
        public string ProcessData
        {
            get
            {
                return processData;
            }
            set
            {
                processData = value;
            }
        }
        public NetEpayAmount Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public NetEpayAccount Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;
            }
        }
    }
}
