using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    ///  Class WeChatPayDTO
    /// </summary>
    public class WeChatPayDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Enum  PaymentType
        /// </summary>
        public enum  PaymentType
        {

            /// <summary>
            /// PAY
            /// </summary>
            PAY = 0,
            /// <summary>
            /// REFUND
            /// </summary>
            REFUND = 1,
            /// <summary>
            /// CANCEL
            /// </summary>
            CANCEL = 2,
            /// <summary>
            /// QUERY_ORDER
            /// </summary>
            QUERY_ORDER = 3
        }
        
        string appId;
        string vendorId;
        string subVendorId;
        string appKey;
        string appSecrekKey;
        string randomString;
        string signature;
        string signatureType;
        string currencyType;
        string itemDescription;
        string itemDetails;
        string vendorOrderNumber;
        double amount;
        string itemLabel;
        string authorizationCode;
        string notifyUrl;
        string terminalIP;
        string tradeType;  //MICROPAY
        PaymentType paymentTypeSelected;

        double refundAmount;
        string refundCurrencyType;
        string refundVendorId;
        string wechatRefundNumber;
        DateTime requestBegintime;
        DateTime requestEndtime;
        string wechatTransactionId;

        private const string AUTHORIZATION_CODE = "authorizationCode";

        /// <summary>
        /// Default constructor
        /// </summary>
        public WeChatPayDTO()
        {
            log.LogMethodEntry();
            this.appId="";
            this.vendorId="";
            this.subVendorId = "";
            this.appKey="";
            this.appSecrekKey="";
            this.randomString="";
            this.signature="";
            this.signatureType="";
            this.currencyType="";
            this.itemDescription="";
            this.itemDetails="";
            this.vendorOrderNumber="";
            this.amount=0.0;
            this.itemLabel="";
            this.authorizationCode="";
            this.notifyUrl="";
            this.terminalIP="";
            this.wechatRefundNumber = "";
            this.tradeType = "";
            this.paymentTypeSelected = PaymentType.PAY;
            this.refundAmount = 0.0;
            this.refundCurrencyType="";
            this.refundVendorId="";
            this.wechatTransactionId = "";
            //this.requestBegintime;
            //this.requestEndtime;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the AppId field
        /// </summary>
        public string AppId { get { return appId; } set { appId = value; } }

        /// <summary>
        /// Get/Set method of the VendorId field
        /// </summary>
        public string VendorId { get { return vendorId; } set { vendorId = value; } }

        /// <summary>
        /// Get/Set method of the AppKey field
        /// </summary>
        public string AppKey { get { return appKey; } set { appKey = value; } }

        /// <summary>
        /// Get/Set method of the AppSecrekKey field
        /// </summary>
        public string AppSecrekKey { get { return appId; } set { appSecrekKey = value; } }

        /// <summary>
        /// Get/Set method of the RandomString field
        /// </summary>
        public string RandomString { get { return randomString; } set { randomString = value; } }

        /// <summary>
        /// Get/Set method of the SubVendorId field
        /// </summary>
        public string SubVendorId { get { return subVendorId; } set { subVendorId = value; } }
        
        /// <summary>
        /// Get/Set method of the Signature field
        /// </summary>
        public string Signature { get { return signature; } set { signature = value; } }

        /// <summary>
        /// Get/Set method of the SignatureType field
        /// </summary>
        public string SignatureType { get { return signatureType; } set { signatureType = value; } }

        /// <summary>
        /// Get/Set method of the CurrencyType field
        /// </summary>
        public string CurrencyType { get { return currencyType; } set { currencyType = value; } }

        /// <summary>
        /// Get/Set method of the ItemDescription field
        /// </summary>
        public string ItemDescription { get { return itemDescription; } set { itemDescription = value; } }

        /// <summary>
        /// Get/Set method of the ItemDetails field
        /// </summary>
        public string ItemDetails { get { return itemDetails; } set { itemDetails = value; } }

        /// <summary>
        /// Get/Set method of the VendorOrderNumber field
        /// </summary>
        public string VendorOrderNumber { get { return vendorOrderNumber; } set { vendorOrderNumber = value; } }

        /// <summary>
        /// Get/Set method of the Amount field
        /// </summary>
        public double Amount { get { return amount; } set { amount = value; } }

       
        /// <summary>
        /// Get/Set method of the ItemLabel field
        /// </summary>
        public string ItemLabel { get { return itemLabel; } set { itemLabel = value; } }

        /// <summary>
        /// Get/Set method of the AuthorizationCode field
        /// </summary>
        public string AuthorizationCode { get { return authorizationCode; } set { authorizationCode = value; } }

        /// <summary>
        /// Get/Set method of the NotifyUrl field
        /// </summary>
        public string NotifyUrl { get { return notifyUrl; } set { notifyUrl = value; } }


        /// <summary>
        /// Get/Set method of the TerminalIP field
        /// </summary>
        public string TerminalIP { get { return terminalIP; } set { terminalIP = value; } }


        /// <summary>
        /// Get/Set method of the TerminalIP field
        /// </summary>
        public string TradeType { get { return tradeType; } set { tradeType = value; } }

        /// <summary>
        /// Get/Set method of the PaymentTypeSelected field
        /// </summary>
        public PaymentType PaymentTypeSelected { get { return paymentTypeSelected; } set { paymentTypeSelected = value; } }

        /// <summary>
        /// Get/Set method of the RefundAmount field
        /// </summary>
        public double RefundAmount { get { return refundAmount; } set { refundAmount = value; } }


        /// <summary>
        /// Get/Set method of the RefundCurrencyType field
        /// </summary>
        public string RefundCurrencyType { get { return refundCurrencyType; } set { refundCurrencyType = value; } }



        /// <summary>
        /// Get/Set method of the RefundVendorId field
        /// </summary>
        public string RefundVendorId { get { return refundVendorId; } set { refundVendorId = value; } }



        /// <summary>
        /// Get/Set method of the RequestBegintime field
        /// </summary>
        public DateTime RequestBegintime { get { return requestBegintime; } set { requestBegintime = value; } }

        /// <summary>
        /// Get/Set method of the RequestEndtime field
        /// </summary>
        public DateTime RequestEndtime { get { return requestEndtime; } set { requestEndtime = value; } }

        /// <summary>
        /// Get/Set method of the WechatRefundNumber field
        /// </summary>
        public string WechatRefundNumber { get { return wechatRefundNumber; } set { wechatRefundNumber = value; } }


        /// <summary>
        /// Get/Set method of the WechatTransactionId field
        /// </summary>
        public string WechatTransactionId { get { return wechatTransactionId; } set { wechatTransactionId = value; } }



        public Dictionary<string, string> MessageList()
        {
            log.LogMethodEntry();
            Dictionary<string, string> messageList = new Dictionary<string, string>();
            log.LogMethodExit(messageList);
            return messageList;
        }

    }
}
