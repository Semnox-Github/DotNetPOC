using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class WeChatPayResponseDTO :WeChatPayDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string serviceResult;

        string errorCode;
        string errorCodeDescription;

        string resultCode;
        string returnCode;
        string resultCodeDescription;

        //openid 
        string userTag;
        bool isFollowsOfficialPay;
        string wechatOrderNumber;

        string paymentBank;

        string exchangeRate;
    
        int refundCount;


        
        //Query
        string refundChannel;
        string refundStatus;
        string accountReceivedRefund;
        
        //refund
        string recall;
        string tradeState;

        string xmlData;

        public WeChatPayResponseDTO():base()
        {
            log.LogMethodEntry();
            this.serviceResult="";
            this.errorCode="";
            this.errorCodeDescription="";
            this.resultCode="";
            this.resultCodeDescription="";
            this.userTag="";
            this.isFollowsOfficialPay=false;
            this.paymentBank="";
            this.exchangeRate="";
           
            this.refundCount=0;
            this.refundChannel="";
            this.refundStatus="";
            this.accountReceivedRefund="";
            this.recall = "N";
            this.wechatOrderNumber = "";
            this.returnCode = "";
            this.tradeState = "";
            this.xmlData = "";

            log.LogMethodExit(null);
        }

        //string weChatRefund;

        /// <summary>
        /// Get/Set method of the ServiceResult field
        /// </summary>
        public string ServiceResult { get { return serviceResult; } set { serviceResult = value; } }

        /// <summary>
        /// Get/Set method of the ErrorCode field
        /// </summary>
        public string ErrorCode { get { return errorCode; } set { errorCode = value; } }

        /// <summary>
        /// Get/Set method of the ErrorCodeDescription field
        /// </summary>
        public string ErrorCodeDescription { get { return errorCodeDescription; } set { errorCodeDescription = value; } }

        /// <summary>
        /// Get/Set method of the ResultCode field
        /// </summary>
        public string ResultCode { get { return resultCode; } set { resultCode = value; } }


        /// <summary>
        /// Get/Set method of the ReturnCode field
        /// </summary>
        public string ReturnCode { get { return returnCode; } set { returnCode = value; } }


        /// <summary>
        /// Get/Set method of the ResultCodeDescription field
        /// </summary>
        public string ResultCodeDescription { get { return resultCodeDescription; } set { resultCodeDescription = value; } }

        /// <summary>
        /// Get/Set method of the UserTag field
        /// </summary>
        public string UserTag { get { return userTag; } set { userTag = value; } }

        /// <summary>
        /// Get/Set method of the IsFollowsOfficialPay field
        /// </summary>
        public bool IsFollowsOfficialPay { get { return isFollowsOfficialPay; } set { isFollowsOfficialPay = value; } }

        

        /// <summary>
        /// Get/Set method of the PaymentBank field
        /// </summary>
        public string PaymentBank { get { return paymentBank; } set { paymentBank = value; } }

        /// <summary>
        /// Get/Set method of the ExchangeRate field
        /// </summary>
        public string ExchangeRate { get { return exchangeRate; } set { exchangeRate = value; } }

      
        /// <summary>
        /// Get/Set method of the RefundCount field
        /// </summary>
        public int RefundCount { get { return refundCount; } set { refundCount = value; } }

        /// <summary>
        /// Get/Set method of the RefundChannel field
        /// </summary>
        public string RefundChannel { get { return refundChannel; } set { refundChannel = value; } }

        /// <summary>
        /// Get/Set method of the RefundStatus field
        /// </summary>
        public string RefundStatus { get { return refundStatus; } set { refundStatus = value; } }

        /// <summary>
        /// Get/Set method of the AccountReceivedRefund field
        /// </summary>
        public string AccountReceivedRefund { get { return accountReceivedRefund; } set { accountReceivedRefund = value; } }

        /// <summary>
        /// Get/Set method of the Recall field
        /// </summary>
        public string Recall { get { return recall; } set { recall = value; } }


        /// <summary>
        /// Get/Set method of the WechatOrderNumber field
        /// </summary>
        public string  WechatOrderNumber { get { return wechatOrderNumber; } set { wechatOrderNumber = value; } }


        /// <summary>
        /// Get/Set method of the TradeState field
        /// </summary>
        public string TradeState { get { return tradeState; } set { tradeState = value; } }

        /// <summary>
        /// Get/Set method of the XmlData field
        /// </summary>
        public string XmlData { get { return xmlData; } set { xmlData = value; } }
        

    }
}
