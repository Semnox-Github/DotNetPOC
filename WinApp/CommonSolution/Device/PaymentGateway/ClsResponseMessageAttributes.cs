using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Class ClsResponseMessageAttributes
    /// </summary>
    public class ClsResponseMessageAttributes
    {
        /// <summary>
        /// Get/set For TokenID
        /// </summary>
        public string TokenID { get; set; }
        /// <summary>
        /// Get/set For ResponseOrigin
        /// </summary>
        public string ResponseOrigin { get; set; }
        /// <summary>
        /// Get/set For DSIXReturnCode
        /// </summary>
        public string DSIXReturnCode { get; set; }
        /// <summary>
        /// Get/set For CmdStatus
        /// </summary>
        public string CmdStatus { get; set; }
        /// <summary>
        /// Get/set For TextResponse
        /// </summary>
        public string TextResponse { get; set; }
        /// <summary>
        /// Get/set For UserTraceData
        /// </summary>
        public string UserTraceData { get; set; }
        /// <summary>
        /// Get/set For MerchantID
        /// </summary>
        public string MerchantID { get; set; }
        /// <summary>
        /// Get/set For AcctNo
        /// </summary>
        public string AcctNo { get; set; }
        /// <summary>
        /// Get/set For CardType
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// Get/set For TranCode
        /// </summary>
        public string TranCode { get; set; }
        /// <summary>
        /// Get/set For AuthCode
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// Get/set For CaptureStatus
        /// </summary>
        public string CaptureStatus { get; set; }
        /// <summary>
        /// Get/set For RefNo
        /// </summary>
        public string RefNo { get; set; }
        /// <summary>
        /// Get/set For InvoiceNo
        /// </summary>
        public string InvoiceNo { get; set; }
        /// <summary>
        /// Get/set For Memo
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// Get/set For Purchase
        /// </summary>
        public string Purchase { get; set; }
        /// <summary>
        /// Get/set For Gratuity
        /// </summary>
        public string Gratuity { get; set; }//Modification on 09-Nov-2015:Tip Feature
        /// <summary>
        /// Get/set For Authorize
        /// </summary>
        public string Authorize { get; set; }
        /// <summary>
        /// Get/set For AcqRefData
        /// </summary>
        public string AcqRefData { get; set; }
        /// <summary>
        /// Get/set For RecordNo
        /// </summary>
        public string RecordNo { get; set; }
        /// <summary>
        /// Get/set For ProcessData
        /// </summary>
        public string ProcessData { get; set; }
        /// <summary>
        /// Get/set For ErrorMessage
        /// </summary>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Get/set For ResponseId
        /// </summary>
        public object ResponseId { get; set; }
    }
}
