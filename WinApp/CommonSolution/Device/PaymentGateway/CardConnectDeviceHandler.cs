//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public abstract class CardConnectDeviceHandler:CardConnectCommandHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string sessionKey;
        protected string deviceId;
        protected string authorization;
        public CardConnectDeviceHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string sessionKey, string merchantId, string url, string deviceId, string authorization)
            : base(utilities, transactionPaymentsDTO, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO);
            this.sessionKey = sessionKey;
            this.url = url;
            this.deviceId = deviceId;
            this.authorization = authorization;
            webRequestHandler = new WebRequestHandler();
            webRequestHandler.ReadWriteTimeout = 60000 * 2;
            log.LogMethodExit(null);
        }

        public override void CreateCommand(object data)
        {
            log.LogMethodEntry(data);
            log.LogMethodExit(null);
        }
        public override HttpWebResponse Sendcommand()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }

        public override object GetResponse(HttpWebResponse webResponse)
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }

        public override CCTransactionsPGWDTO ParseResponse(object response)
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
            return null;
        }
    }
    public class ReadCardResponse
    {
        public string TokenId;
        public string Name;
        public string ExpDate;
        public string ZipCode;
        public string Signature;
        public string orderId;
        public bool includePIN = false;
        public string aid = "credit";
    }
    public class AuthCardResponse
    {
        public string TokenId;
        public string Name;
        public string ExpDate;
        public string Batchid;
        public string Retref;
        public string Avsresp;
        public string Respproc;
        public string Amount;
        public string Resptext;
        public string Authcode;
        public string Respcode;
        public string MerchentId;
        public string CvvResp;
        public string Respstat;
    }


    public class ReadCardRequest
    {
        public bool IsBeepSoundRequired;
        public bool IsZipValidationRequired;
        public bool IsCaptureRequired;
        public bool IsSignatureRequired;
        public bool includePIN = false;
        public string aid = "credit";
    }
    public enum InputType{
        ALPHA_NUMERIC,
        NUMERIC,
        PHONE_NUMBER,
        MMYY,
        AMOUNT
    }
    public class ReadInputRequest
    {
        public string DisplayMessage;
        public InputType Format;
        /// <summary>
        /// This is mandatory for Numeric and Alpha numeric
        /// </summary>
        public int  MinLength;
        public int MaxLength;
    }
    
}
