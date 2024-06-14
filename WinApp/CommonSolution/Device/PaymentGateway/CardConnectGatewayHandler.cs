/*******************************************************************************************************************************
 * Project Name - CardConnect Data Handler
 * Description  - Data handler of the CardConnect class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************************************************
 *2.110.0     08-Dec-2020   Guru S A       Subscription changes           
 *2.140.5     16-Jan-2023   Mathew Ninan   Reduced timeout period as per CardConnect 
 *                                         documentation - https://developer.cardpointe.com/guides/terminal#accepting-pin-debit-cards
 *******************************************************************************************************************************/
//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    public abstract class CardConnectGatewayHandler : CardConnectCommandHandler
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected string username;
        protected string password;
        public CardConnectGatewayHandler(Utilities utilities, TransactionPaymentsDTO transactionPaymentsDTO, string url, string username, string password, string merchantId)
            : base(utilities, transactionPaymentsDTO, merchantId)
        {
            log.LogMethodEntry(utilities, transactionPaymentsDTO);
            this.url = url;
            this.username = username;
            this.password = password;
            webRequestHandler = new WebRequestHandler();
            webRequestHandler.ReadWriteTimeout = 60000 * 3;
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
    public class AuthorizationResponse
    {
        public string Amount;
        public string Resptext;
        public string CVVresp;
        public string AVSresp;
        public string Respcode;
        public string Merchid;
        public string Token;
        public string Authcode;
        public string Respproc;
        public EMVTagData EmvTagData;
        public string Retref;
        public string Respstat;
        public string Account;
        public string ProfileId;
    }
    public class InquireResponse
    {
        public string Amount;
        public string Resptext;
        public string OrderId;
        public string Setlstat;
        public string Respcode;
        public string Capturedate;
        public string Batchid;
        public string Merchid;
        public string Entrymode;
        public string Token;
        public string Authcode;
        public string Respproc;
        public EMVTagData EmvTagData;
        public string Retref;
        public string Respstat;
        public string Account;
        public string Voidable;
        public string Refundable;
        public string Customphone;
        public string Custommerchant;
    }
    public class EMVTagData
    {
        public string TVR;
        public string ARC;
        public string TSI;
        public string AID;
        public string IAD;
        public string EntryMethod;
    }

    public class BinData
    {
        public string Country;
        public string Product;
        public string CardUseString;
        public bool Gsa;
        public string Corporate;
        public bool Fsa;
        public string SubType;
        public bool Purchase;
        public string Prepaid;
        public string CardNo;
        public string Issuer;
        public string AccountNo;//binhi accont no        
    }

    public class CaptureData
    {
        public string MerchantID;
        public string Account;
        public string Amount;
        public string Retref;
        public string Setlstat;
        public string Resptext;
        public string CVVresp;
        public string AVSresp;
        public string Respcode;
        public string Token;
        public string Authcode;
        public string Respproc;
        public string Respstat;
        public string BatchId;
    }
    public class CaptureDataL2
    {
        public string PONumber;
        public string TaxAmnt;        
    }
    public class CaptureDataL3
    {
        /// <summary>
        /// Total order freight amount
        /// </summary>
        public string Frtamnt;
        /// <summary>
        /// Total order duty amount
        /// </summary>
        public string DutyAmnt;
        /// <summary>
        /// Order date in YYYYMMDD format
        /// </summary>
        public string Orderdate;
        /// <summary>
        /// Destination Zip code
        /// </summary>
        public string ShiptoZip;
        /// <summary>
        /// Source zip code
        /// </summary>
        public string ShipfromZip;
        /// <summary>
        /// Destinition country
        /// </summary>
        public string ShiptoCountry;
        /// <summary>
        /// Item List
        /// </summary>
        public List<String> Items;
    }


    public class VoidResponse
    {
        public string MerchantId;
        public string Amount;
        public string Currency;
        public string Retref;
        public string Authcode;
        public string Respcode;
        public string Respproc;
        public string Respstat;
        public string Resptext;
    }

    public class RefundResponse
    {
        public string MerchantId;
        public string Amount;
        public string Retref;
        public string Authcode;
        public string Respcode;
        public string Respproc;
        public string Respstat;
        public string Resptext;
    }
}
