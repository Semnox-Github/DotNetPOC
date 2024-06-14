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
    public partial class TxDetailsInquiryCardInfoResponse
    {

        private TxDetailsInquiryCardInfoResponseTxDetailsInquiryCardInfoResult txDetailsInquiryCardInfoResultField;

        /// <remarks/>
        public TxDetailsInquiryCardInfoResponseTxDetailsInquiryCardInfoResult TxDetailsInquiryCardInfoResult
        {
            get
            {
                return this.txDetailsInquiryCardInfoResultField;
            }
            set
            {
                this.txDetailsInquiryCardInfoResultField = value;
            }
        }

        public override string ToString()
        {
            StringBuilder returnValue = new StringBuilder("\n----------------------TxDetailsInquiryCardInfoResponse-----------------------------\n");
            returnValue.Append(" MerchantCode : " + TxDetailsInquiryCardInfoResult.MerchantCode);
            returnValue.Append(" PaymentId : " + TxDetailsInquiryCardInfoResult.PaymentId);
            returnValue.Append(" RefNo : " + TxDetailsInquiryCardInfoResult.RefNo);
            returnValue.Append(" Amount : " + TxDetailsInquiryCardInfoResult.Amount);
            returnValue.Append(" Currency : " + TxDetailsInquiryCardInfoResult.Currency);
            returnValue.Append(" Remark : " + TxDetailsInquiryCardInfoResult.Remark);
            returnValue.Append(" TransId : " + TxDetailsInquiryCardInfoResult.TransId);
            returnValue.Append(" AuthCode : " + TxDetailsInquiryCardInfoResult.AuthCode);
            returnValue.Append(" Status : " + TxDetailsInquiryCardInfoResult.Status);
            returnValue.Append(" Errdesc : " + TxDetailsInquiryCardInfoResult.Errdesc);
            returnValue.Append("\n-------------------------------------------------------------\n");
            return returnValue.ToString();
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.mobile88.com/epayment/webservice")]
    public partial class TxDetailsInquiryCardInfoResponseTxDetailsInquiryCardInfoResult
    {

        private string merchantCodeField;

        private string paymentIdField;

        private string refNoField;

        private string amountField;

        private string currencyField;

        private string remarkField;

        private string transIdField;

        private string authCodeField;

        private string statusField;

        private string errdescField;

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
        public string PaymentId
        {
            get
            {
                return this.paymentIdField;
            }
            set
            {
                this.paymentIdField = value;
            }
        }

        /// <remarks/>
        public string RefNo
        {
            get
            {
                return this.refNoField;
            }
            set
            {
                this.refNoField = value;
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
        public string Currency
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
        public string Remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }

        /// <remarks/>
        public string TransId
        {
            get
            {
                return this.transIdField;
            }
            set
            {
                this.transIdField = value;
            }
        }

        /// <remarks/>
        public string AuthCode
        {
            get
            {
                return this.authCodeField;
            }
            set
            {
                this.authCodeField = value;
            }
        }

        /// <remarks/>
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public string Errdesc
        {
            get
            {
                return this.errdescField;
            }
            set
            {
                this.errdescField = value;
            }
        }
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.mobile88.com")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.mobile88.com", IsNullable = false)]
    public partial class VoidTransactionResponse
    {

        private string voidTransactionResultField;

        /// <remarks/>
        public string VoidTransactionResult
        {
            get
            {
                return this.voidTransactionResultField;
            }
            set
            {
                this.voidTransactionResultField = value;
            }
        }
    }

    public class IPay88RefundResponseDTO
    {
        public string MerchantCode { get; set; }
        public string RefNo { get; set; }
        public string Status { get; set; }
        public string ErrDesc { get; set; }
        public string TransId { get; set; }
        public string XField1 { get; set; }
        public long TransactionDate { get; set; }

        public override string ToString()
        {
            StringBuilder returnValue = new StringBuilder("\n----------------------IPay88RefundResponseDTO-----------------------------\n");
            returnValue.Append(" MerchantCode : " + MerchantCode);
            returnValue.Append(" RefNo : " + RefNo);
            returnValue.Append(" Status : " + Status);
            returnValue.Append(" ErrDesc : " + ErrDesc);
            returnValue.Append(" TransId : " + TransId);
            returnValue.Append(" XField1 : " + XField1);
            returnValue.Append(" TransactionDate : " + TransactionDate);
            returnValue.Append("\n-------------------------------------------------------------\n");
            return returnValue.ToString();
        }
    }

}
