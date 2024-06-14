using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Class ClsRequestMessageAttributes
    /// </summary>
    public class ClsRequestMessageAttributes
    {
        /// <summary>
        /// Get/set For MerchantID
        /// </summary>
        [XmlElement]
        public string MerchantID { get; set; }
        /// <summary>
        /// Get/set For TranType
        /// </summary>
        [XmlElement]
        public CardType TranType { get; set; }
        /// <summary>
        /// Get/set For TranCode
        /// </summary>
        [XmlElement]
        public string TranCode { get; set; }
        /// <summary>
        /// Get/set For InvoiceNo
        /// </summary>
        [XmlElement]
        public string InvoiceNo { get; set; }
        /// <summary>
        /// Get/set For RefNo
        /// </summary>
        [XmlElement]
        public string RefNo { get; set; }
        /// <summary>
        /// Get/set For Memo
        /// </summary>
        [XmlElement]
        public string Memo { get; set; }
        /// <summary>
        /// Get/set For Frequency
        /// </summary>
        [XmlElement]
        public string Frequency { get; set; }
        /// <summary>
        /// Get/set For RecordNo
        /// </summary>
        [XmlElement]
        public string RecordNo { get; set; }
        /// <summary>
        /// Get/set For PartialAuth
        /// </summary>
        [XmlElement]
        public string PartialAuth { get; set; }
        /// <summary>
        /// Get/set For EncryptedFormat
        /// </summary>
        [XmlElement]
        public string EncryptedFormat { get; set; }
        /// <summary>
        /// Get/set For AccountSource
        /// </summary>
        [XmlElement]
        public string AccountSource { get; set; }
        /// <summary>
        /// Get/set For EncryptedBlock
        /// </summary>
        [XmlElement]
        public string EncryptedBlock { get; set; }
        /// <summary>
        /// Get/set For EncryptedKey
        /// </summary>
        [XmlElement]
        public string EncryptedKey { get; set; }
        /// <summary>
        /// Get/set For Purchase
        /// </summary>
        [XmlElement]
        public string Purchase { get; set; }
        /// <summary>
        /// Get/set For TipAmount
        /// </summary>
        [XmlElement]//Begin Modification on 09-Nov-2015:Tip feature
        public string TipAmount { get; set; }//Ends Modification on 09-Nov-2015:Tip feature
        /// <summary>
        /// Get/set For TerminalName
        /// </summary>
        [XmlElement]
        public string TerminalName { get; set; }
        /// <summary>
        /// Get/set For PINBlock
        /// </summary>
        [XmlElement]
        public string PINBlock { get; set; }
        /// <summary>
        /// Get/set For DervdKey
        /// </summary>
        [XmlElement]
        public string DervdKey { get; set; }
        /// <summary>
        /// Get/set For ShiftID
        /// </summary>
        [XmlElement]
        public string ShiftID { get; set; }
        /// <summary>
        /// Get/set For OperatorID
        /// </summary>
        [XmlElement]
        public string OperatorID { get; set; }
        /// <summary>
        /// Get/set For CardType
        /// </summary>
        // copied
        [XmlElement]
        public string CardType { get; set; }
        /// <summary>
        /// Get/set For CmdStatus
        /// </summary>
        [XmlElement]
        public string CmdStatus { get; set; }
        /// <summary>
        /// Get/set For AcctNo
        /// </summary>
        [XmlElement]
        public string AcctNo { get; set; }
        /// <summary>
        /// Get/set For ExpDate
        /// </summary>
        [XmlElement]
        public string ExpDate { get; set; }
        /// <summary>
        /// Get/set For AcqRefData
        /// </summary>
        [XmlElement]
        public string AcqRefData { get; set; }
        /// <summary>
        /// Get/set For ProcessData
        /// </summary>
        [XmlElement]
        public string ProcessData { get; set; }
        /// <summary>
        /// Get/set For AuthCode
        /// </summary>
        [XmlElement]
        public string AuthCode { get; set; }
    }
}
