using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    ///  class NCRResponse
    /// </summary>
    public class NCRResponse
    {
        /// <summary>
        /// Sequence No
        /// </summary>
        public StringBuilder SequenceNo = new StringBuilder();
        /// <summary>
        /// PosTransaction no= Trxid
        /// </summary>
        public StringBuilder POSTransactionNo = new StringBuilder();
        /// <summary>
        /// Approved amount
        /// </summary>
        public int ApprovedAmount;
        /// <summary>
        /// Authorization number
        /// </summary>
        public StringBuilder AuthCode = new StringBuilder(8);
        /// <summary>
        /// Authorization number
        /// </summary>
        public StringBuilder HostReferenceNumber = new StringBuilder(10);
        /// <summary>
        /// Response Code
        /// </summary>
        public StringBuilder ResponseCode = new StringBuilder(4);
        /// <summary>
        /// Customer Name
        /// </summary>
        public StringBuilder CustomerName = new StringBuilder(40);
        /// <summary>
        /// Track2Data
        /// </summary>
        public StringBuilder CardNo = new StringBuilder(80);
        /// <summary>
        /// Account
        /// </summary>
        public StringBuilder AccountNumber = new StringBuilder(22);
        /// <summary>
        /// Transaction Date in MMDDYY
        /// </summary>
        public StringBuilder TrnsactionDate = new StringBuilder(6);
        /// <summary>
        /// Transaction Time in HHMMSS
        /// </summary>
        public StringBuilder TrnsactionTime = new StringBuilder(6);
        /// <summary>
        /// ResponseText
        /// </summary>
        public string ResponseText;
        /// <summary>
        /// TenderType
        /// </summary>
        public eTenderType TenderType;
        /// <summary>
        /// Customer Receipt Text
        /// </summary>
        public string CustomerReceiptText;
        /// <summary>
        /// ccResponseId
        /// </summary>
        public int ccResponseId = -1;
        /// <summary>
        /// Merchant Receipt Text
        /// </summary>
        public string MerchantReceiptText;
        /// <summary>
        /// TokenData
        /// </summary>
        public StringBuilder TokenData = new StringBuilder(500);
        /// <summary>
        /// CardType
        /// </summary>
        public StringBuilder CardType = new StringBuilder(18);
        /// <summary>
        /// CashBackAmount
        /// </summary>
        public int CashBackAmount;
    }
}
