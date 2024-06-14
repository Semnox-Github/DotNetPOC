using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.mobile88.com/epayment/webservice")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.mobile88.com/epayment/webservice", IsNullable = false)]
    public partial class TxDetailsInquiryCardInfo
    {

        private string merchantCodeField;

        private string referenceNoField;

        private string amountField;

        private string versionField;

        /// <remarks/>
        public string MerchantCode
        {
            get
            {
                return this.merchantCodeField;
            }
            set
            {
                this.merchantCodeField = value;
            }
        }

        /// <remarks/>
        public string ReferenceNo
        {
            get
            {
                return this.referenceNoField;
            }
            set
            {
                this.referenceNoField = value;
            }
        }

        /// <remarks/>
        public string Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        public override string ToString()
        {
            StringBuilder returnValue = new StringBuilder("\n----------------------TxDetailsInquiryCardInfo-----------------------------\n");
            returnValue.Append(" MerchantCode : " + MerchantCode);
            returnValue.Append(" RefNo : " + ReferenceNo);
            returnValue.Append(" Amount : " + Amount);
            returnValue.Append(" Version : " + Version);
            returnValue.Append("\n-------------------------------------------------------------\n");
            return returnValue.ToString();
        }

    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.mobile88.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.mobile88.com", IsNullable = false)]
    public partial class VoidTransaction
    {

        private string merchantcodeField;

        private string cctransidField;

        private string amountField;

        private string currencyField;

        private string signatureField;

        /// <remarks/>
        public string merchantcode
        {
            get
            {
                return this.merchantcodeField;
            }
            set
            {
                this.merchantcodeField = value;
            }
        }

        /// <remarks/>
        public string cctransid
        {
            get
            {
                return this.cctransidField;
            }
            set
            {
                this.cctransidField = value;
            }
        }

        /// <remarks/>
        public string amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }
    }

    public class IPay88RefundRequestDTO
    {
        public string MerchantCode { get; set; }
        public string REFUND_CASH_AMOUNT { get; set; }
        public string PayeeBank { get; set; }
        public string PayeeName { get; set; }
        public string PayeeACNo { get; set; }
        public string Signature { get; set; }
        public string TransId { get; set; }
    }
}
