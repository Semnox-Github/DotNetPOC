/********************************************************************************************
 * Project Name - Payment Gateway Factory                                                                     
 * Description  - Payment Gateway Factory
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         08-May-2019   Nitin Pai           Added Stripe Gateway for Guest app
 *2.70.2       20-Sep-2019   Archana             Added Freedompay Gateway 
 *2.70.2       05-Jan-2020   Jeevan              Added CCAvenue Gateway for Website/Guest app
 *2.70.3       10-Feb-2020   Jeevan              Added CorvusPay/Adyen Gateway for Website
 *2.90.0       14-Jul-2020   Jeevan              Added Payeezy/Payfort/Worldpay/Bambora Hosted Payments for Website
 *2.90.0       14-Jul-2020   Flavia              Added CardConnectHosted Payments for Website
 *2.80.0       09-Jun-2020   Jinto Thomas        Added Alipay Gateway
 *2.10.0       18-Aug-2020   Jinto Thomas        Added Ipay Gateway
 *2.10.0       22-Aug-2020   Jeevan              Added Ecom Payment Gateway
 *2.10.0       23-Sep-2020   Dakshakh            Added Mada Payment Gateway
 *2.140.0      08-Oct-2021   Jinto Thomas        Added VisaNets Hosted Payment Gateway
 *2.150.0      26-Jul-2022   Dakshakh Raj        Added Mashreq, Geidea Payment Gateway
 *2.150.1      27-Feb-2023   Muaaz Musthafa      Added Paytm Hosted Payment Gateway
 *2.150.1      26-Jan-2022   Nitin Pai           Added new angular payment gateways
 *2.150.3      3-Apr-2023    Muaaz Musthafa      Added Payfort Hosted Payment Gateway
 *2.150.4      15-Jun-2023   Muaaz Musthafa      Added new AdyenWeb Hosted Payment Gateway
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using System.Reflection;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Payment gateways supported in parafait. 
    /// </summary>
    public enum PaymentGateways
    {
        /// <summary>
        /// No Payment gateway.
        /// </summary>
        None,
        /// <summary>
        /// Tyro payment gateway
        /// </summary>
        TYRO,
        /// <summary>
        /// PCEFTPOS Payment gateway
        /// </summary>
        PCEFTPOS,
        /// <summary>
        /// Mercury Payment gateway
        /// </summary>
        Mercury,
        /// <summary>
        /// ElementExpress Payment gateway
        /// </summary>
        ElementExpress,
        /// <summary>
        /// Quest Payment gateway
        /// </summary>
        Quest,
        /// <summary>
        /// CreditCall Payment gateway
        /// </summary>
        CreditCall,
        /// <summary>
        /// FirstData Payment gateway
        /// </summary>   
        FirstData,
        /// <summary>
        /// Borica Payment gateway
        /// </summary>
        Borica,
        /// <summary>
        /// ChinaUMS Payment gateway
        /// </summary>
        ChinaUMS,
        /// <summary>
        /// NCR Payment gateway
        /// </summary>
        NCR,//Starts:ChinaICBC changes
        /// <summary>
        /// China ICBC Payment gateway
        /// </summary>
        ChinaICBC,
        //Ends:Moneris changes
        /// <summary>
        /// Moneris Payment gateway
        /// </summary>
        Moneris,//Ends:Moneris changes
        //Ends:Moneris changes
        /// <summary>
        /// WeChat Payment gateway
        /// </summary>
        WeChat,
        //Ends:WeChat changes
        /// <summary>
        /// Card Connect Payment Gateway
        /// </summary>
        CardConnect,
        /// <summary>
        /// Chase Payment Gateway
        /// </summary>
        Chase,
        /// <summary>
        /// Worldpay Payment Gateway
        /// </summary>
        Worldpay,
        /// <summary>
        /// Innoviti Payment Gateway
        /// </summary>
        Innoviti,
        /// <summary>
        /// CyberSource payment gateway
        /// </summary>
        CyberSource,
        /// <summary>
        /// TransBank payment gateway
        /// </summary>
        TransBank,
        /// <summary>
        /// Stripe Payment Gateway
        /// </summary>
        StripePayment,
        /// <summary>
        /// AEON Credit payment gateway
        /// </summary>
        AEONCredit,
        /// <summary>
        /// Clover payment gateway
        /// </summary>
        Clover,
        /// <summary>
        /// FreedomPay Payment Gateway
        /// </summary>
        FreedomPay,
        /// <summary>
        /// CCAvenueHostedPayment Payment Gateway
        /// </summary>
        CCAvenueHostedPayment,
        /// <summary>
        /// Adyen payment gateway
        /// </summary>
        Adyen,
        /// <summary>
        /// CorvusPayHostedPayment Payment Gateway
        /// </summary>
        CorvusPayHostedPayment,
        /// <summary>
        /// AdyenHostedPayment Payment Gateway
        /// </summary>
        AdyenHostedPayment,
        /// <summary>
        /// FirstDataPayeezyHostedPayment Payment Gateway
        /// </summary>
        FirstDataPayeezyHostedPayment,
        /// <summary>
        /// WorldPayHostedPayment Payment Gateway
        /// </summary>
        WorldPayHostedPayment,
        /// <summary>
        /// PayFortHostedPayment Payment Gateway
        /// </summary>
        PayFortHostedPayment,
        /// <summary>
        /// BamboraHostedPayment Payment Gateway
        /// </summary>
        BamboraHostedPayment,
        /// <summary>
        /// CardConnectHostedPayment Payment Gateway
        /// </summary>           
        CardConnectHostedPayment,
        /// <summary>
        /// Alipay Payment Gateway
        /// </summary>
        Alipay,
        /// <summary>
        /// Ipay Payment Gateway
        /// </summary>
        Ipay,
        /// <summary>
        /// Ecom Payment Gateway
        /// </summary>
        EcommpayHostedPayment,
        /// <summary>
        /// Mada Payment Gateway
        /// </summary>
        Mada,
        /// <summary>
        /// NetEpay Payment Gateway
        /// </summary>
        NetEpay,
        /// <summary>
        /// IpayHostedPayment Payment Gateway
        /// </summary>
        ipay88HostedPayment,
        /// <summary>
        /// StripetHostedPayment Payment Gateway
        /// </summary>
        StripeHostedPayment,
        /// <summary>
        /// PayNimoHostedPayment Payment Gateway
        /// </summary>
        PayNimoHostedPayment,
        /// <summary>
        /// CreditCallHostedPayment Payment Gateway
        /// </summary>
        CreditCallHostedPayment,
        /// <summary>
        /// Mashreq Kiosk Payment Gateway
        /// </summary>
        MashreqKiosk,
        /// <summary>
        /// MidtransHostedPayment Payment Gateway
        /// </summary>
        MidtransHostedPayment,
        /// <summary>
        /// VisaNetsHostedPayment Payment Gateway
        /// </summary>
        VisaNetsHostedPayment,
        /// <summary>
        /// CyberSource Payment Gateway
        /// </summary>
        CyberSourceHostedPayment,
        /// <summary>
        /// WorldPayService Payment Gateway
        /// </summary>
        WorldPayService,
        /// <summary>
        /// CloverCardConnectPaymentGateway
        /// </summary>
        CloverCardConnectPaymentGateway,
        /// <summary>
        /// Geidea Payment Gateway
        /// </summary>
        Geidea,
        /// <summary>
        /// Mashreq Payment Gateway
        /// </summary>
        Mashreq,
        /// <summary>
        /// WPCyberSourceHostedPayment Payment Gateway
        /// </summary>
        WPCyberSourceHostedPayment,
        /// <summary>
        /// CommonWebHostedPayment Payment Gateway
        /// </summary>
        CommonWebHostedPayment,
        /// <summary>
        /// ClubSpeedGiftCard Payment Gateway
        /// </summary>
        ClubSpeedGiftCard,
        /// <summary>
        /// ThawaniHostedPayment
        /// </summary>
        ThawaniHostedPayment,
        /// <summary>
        /// PaytmHostedPayment
        /// </summary>
        PaytmHostedPayment,
        /// <summary>
        /// ipay88CallbackHostedPayment
        /// </summary>
        ipay88CallbackHostedPayment,
        /// <summary>
        /// CCAvenueCallbackHostedPayment
        /// </summary>
        CCAvenueCallbackHostedPayment,
        /// <summary>
        /// WPCyberSourceCallbackHostedPayment
        /// </summary>
        WPCyberSourceCallbackHostedPayment,
        /// <summary>
        /// CardConnectCallbackHostedPayment
        /// </summary>
        CardConnectCallbackHostedPayment,
        /// <summary>
        /// MonerisbHostedPayment Payment Gateway
        /// </summary>
        MonerisHostedPayment,
        /// <summary>
        /// PineLabsQRPayment
        /// </summary>
        PineLabsQRPayment,
        /// <summary>
        /// PineLabsCardPayment
        /// </summary>
        PineLabsCardPayment,
        /// <summary>
        /// PaytmDQRPayment
        /// </summary>
        PaytmDQRPayment
    }

    /// <summary>
    /// This class is used to instantiate payment gateway class.
    /// </summary>
    public class PaymentGatewayFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Dictonary containing singleton gateways
        /// </summary>
        protected Dictionary<PaymentGateways, PaymentGateway> paymentGateways;

        private static PaymentGatewayFactory paymentGatewayFactory;

        /// <summary>
        /// Indicates whether the Payment gateway factory is initialized.
        /// </summary>
        protected bool initialized = false;

        /// <summary>
        /// Whether the payment process is supervised by an attendant.
        /// </summary>
        protected bool isUnattended = false;

        /// <summary>
        /// Parafait utilities
        /// </summary>
        protected Utilities utilities = null;

        /// <summary>
        /// Delegate instance to display message.
        /// </summary>
        protected ShowMessageDelegate showMessageDelegate;

        /// <summary>
        /// Delegate instance for writing the Log to File
        /// </summary>
        protected WriteToLogDelegate writeToLogDelegate;

        private PaymentGatewayFactory()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Returns a singleton instance of payment gateway factory.
        /// </summary>
        /// <returns></returns>
        public static PaymentGatewayFactory GetInstance()
        {
            log.LogMethodEntry();
            if (paymentGatewayFactory == null)
            {
                paymentGatewayFactory = new PaymentGatewayFactory();
            }
            log.LogMethodExit(paymentGatewayFactory);
            return paymentGatewayFactory;
        }

        /// <summary>
        /// Initializes the Payment Gateway Factory. 
        /// </summary>
        /// <param name="utilities">Parafait utilities</param>
        /// <param name="isUnattended">Whether the payment process is supervised by an attendant.</param>
        /// <param name="showMessageDelegate"> Delegate instance to display message.</param>
        /// <param name="writeToLogDelegate">Delegate instance for writing into log</param>
        public virtual void Initialize(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate = null, WriteToLogDelegate writeToLogDelegate = null)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            this.utilities = utilities;
            this.isUnattended = isUnattended;
            initialized = true;
            this.showMessageDelegate = showMessageDelegate;
            this.writeToLogDelegate = writeToLogDelegate;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Instatiates the payment gateway based on the gateway type.
        /// </summary>
        /// <param name="gateway">Payment gateway type</param>
        /// <returns></returns>
        public PaymentGateway GetPaymentGateway(PaymentGateways gateway)
        {
            log.LogMethodEntry(gateway);
            PaymentGateway paymentGateway = null;
            if (initialized)
            {

                if (utilities == null)
                {
                    log.LogMethodExit(null, "Throwing ArgumentNullException - utilities is null.");
                    throw new ArgumentNullException("utilities");
                }
                if (CanCreateMultipleInstances(gateway))
                {
                    paymentGateway = CreatePaymentGateway(gateway, utilities, isUnattended);
                }
                else
                {
                    if (paymentGateways == null)
                    {
                        paymentGateways = new Dictionary<PaymentGateways, PaymentGateway>();
                    }
                    if (paymentGateways.ContainsKey(gateway) == false)
                    {
                        paymentGateway = CreatePaymentGateway(gateway, utilities, isUnattended);
                        paymentGateways.Add(gateway, paymentGateway);
                    }
                    else
                    {
                        paymentGateway = paymentGateways[gateway];
                    }
                }
            }
            else
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayConfigurationException - Payment Gateway Factory not initialized. Please initialize before creating payment gateway.");
                throw new PaymentGatewayConfigurationException("Payment Gateway Factory not initialized. Please initialize before creating payment gateway.");
            }
            log.LogMethodExit(paymentGateway);
            return paymentGateway;
        }

        /// <summary>
        /// Instatiates the payment gateway based on the gateway type string.
        /// </summary>
        /// <param name="gatewayString"></param>
        /// <returns></returns>
        public PaymentGateway GetPaymentGateway(string gatewayString)
        {
            log.LogMethodEntry(gatewayString);
            PaymentGateways gateway;
            PaymentGateway paymentGateway = null;
            if (Enum.TryParse<PaymentGateways>(gatewayString, true, out gateway))
            {
                paymentGateway = GetPaymentGateway(gateway);
            }
            else
            {
                log.LogMethodExit(null, "Throwing Payment Gateway Configuration Exception Unable to create payment gateway of type: " + gatewayString + ".Please check the configurations.");
                throw new PaymentGatewayConfigurationException("Unable to create payment gateway of type : " + gatewayString + ". Please check the configurations.");
            }
            log.LogMethodExit(paymentGateway);
            return paymentGateway;
        }

        private PaymentGateway CreatePaymentGateway(PaymentGateways gateway, Utilities utilities, bool isUnattended)
        {
            log.LogMethodEntry(gateway, utilities, isUnattended);

            PaymentGateway paymentGateway = null;
            string paymentGatewayClassName = GetPaymentGatewayClassName(gateway);
            object paymentGatewayInstance = null;
            if (string.IsNullOrWhiteSpace(paymentGatewayClassName) == false)
            {
                try
                {
                    Type type = Type.GetType(paymentGatewayClassName);
                    if (type != null)
                    {
                        ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Utilities), typeof(bool), typeof(ShowMessageDelegate), typeof(WriteToLogDelegate) });
                        paymentGatewayInstance = constructor.Invoke(new object[] { utilities, isUnattended, showMessageDelegate, writeToLogDelegate });
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Payment Gateway Configuration Exception - Cannot create instance of type: " + paymentGatewayClassName + ". Please check whether the dll exist in application folder.");
                        throw new PaymentGatewayConfigurationException("Cannot create instance of type: " + paymentGatewayClassName + ". Please check whether the dll exist in application folder.");
                    }
                }
                catch (Exception e)
                {
                    log.Error("Error occured while creating payment gateway", e);
                    log.LogMethodExit(null, "Throwing Payment Gateway Configuration Exception - Error occured while creating the payment gateway. type: " + gateway.ToString());
                    throw new PaymentGatewayConfigurationException("Error occured while creating the payment gateway. type: " + gateway.ToString(), e);
                }
            }
            else
            {
                log.LogMethodExit(null, "Throwing Payment Gateway Configuration Exception - Payment gateway not configured for : " + gateway.ToString() + ". Please configure the same.");
                throw new PaymentGatewayConfigurationException("Payment gateway not configured for : " + gateway.ToString() + ". Please configure the same.");
            }
            if (paymentGatewayInstance != null && paymentGatewayInstance is PaymentGateway)
            {
                paymentGateway = paymentGatewayInstance as PaymentGateway;
            }
            else
            {
                StringBuilder message = new StringBuilder("Error occured while creating the payment gateway.type: " + gateway.ToString());
                if (paymentGatewayInstance == null)
                {
                    message.Append(". paymentGatewayInstance is null. please check the PaymentGatewayClassName");
                }
                else if ((paymentGatewayInstance is PaymentGateway) == false)
                {
                    message.Append(". paymentGatewayInstance is not of valid type. Type of paymentGatewayInstance is :" + paymentGatewayInstance.GetType().ToString());
                }
                log.LogMethodExit(null, "Throwing Payment Gateway Configuration Exception - " + message.ToString());
                throw new PaymentGatewayConfigurationException(message.ToString());
            }
            log.LogMethodExit(paymentGateway);
            return paymentGateway;
        }


        private string GetPaymentGatewayClassName(PaymentGateways gateway)
        {
            log.LogMethodEntry(gateway);
            string returnValue = string.Empty;
            switch (gateway)
            {
                case PaymentGateways.None:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.TYRO:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.TyroPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PCEFTPOS:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PCEFTPOSPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Mercury:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MercuryPaymentGateway, Device";
                        break;
                    }
                case PaymentGateways.ElementExpress:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.ElementExpressPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Quest:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.QuestPaymentGateway,Device";
                        break;
                    }
                //case PaymentGateways.CreditCall:
                //    {
                //        returnValue = "Semnox.Parafait.Device.PaymentGateway.CreditcallPaymentGateway,CreditCallWrapper";
                //        break;
                //    }
                case PaymentGateways.CreditCall:
                    {
                        returnValue = "Creditcall.CreditcallPaymentGateway,CreditCallWrapper";
                        break;
                    }
                case PaymentGateways.FirstData:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.FirstDataPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Borica:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.BoricaPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.ChinaUMS:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.ChinaUMSPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.NCR:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.NCRPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.ChinaICBC://Starts:ChinaICBC changes
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.ChinaICBCPaymentGateway,Device";
                        break;
                    }//Ends:ChinaICBC changes
                case PaymentGateways.Moneris://Starts:ChinaICBC changes
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MonerisPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.WeChat://Starts:ChinaICBC changes
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.WechatPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CardConnect://Starts:CardConnect changes
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CardConnectPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Chase:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.ChasePaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Worldpay:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.WorldpayPaymentGateway,Device";
                        break;
                    }
                //Ends:CardConnect changes
                case PaymentGateways.Innoviti:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.InnovitiPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CyberSource:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CyberSourcePaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.TransBank:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.TransBankPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.StripePayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.StripePaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.AEONCredit:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.AEONCreditPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Clover:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CloverPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.FreedomPay:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.FreedomPayPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CCAvenueHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CCAvenueHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CorvusPayHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CorvusPayHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.AdyenHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.Adyen.AdyenHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Adyen:
                    {
                        returnValue = "Semnox.Parafait.ThirdParty.AdyenWrapper.AdyenPaymentGateway,AdyenWrapper";
                        break;
                    }
                case PaymentGateways.FirstDataPayeezyHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.FirstDataPayeezyHostedPayment,Device";
                        break;
                    }
                case PaymentGateways.WorldPayHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.WorldPayHostedPayment,Device";
                        break;
                    }
                case PaymentGateways.BamboraHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.BamboraHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CardConnectHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CardConnectHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Alipay:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.AlipayPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Ipay:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.IpayPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.EcommpayHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.EcommpayHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Mada:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MadaPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.NetEpay:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.NetEpayPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.ipay88HostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.Ipay88HostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.StripeHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.StripeHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PayNimoHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PayNimoHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CreditCallHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CreditCallHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.MashreqKiosk:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MashreqKioskPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.MidtransHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MidtransHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.VisaNetsHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.VisaNetsHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CyberSourceHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.CyberSource.CyberSourceHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.WorldPayService:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.WorldPayServicePaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CloverCardConnectPaymentGateway:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CloverCardConnectPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Geidea:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.GeideaPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.Mashreq:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.MashreqPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.WPCyberSourceHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource.WPCyberSourceHostedPayment,Device";
                        break;
                    }
                case PaymentGateways.CommonWebHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CommonWebHostedPayment,Device";
                        break;
                    }
                case PaymentGateways.ClubSpeedGiftCard:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.ClubSpeedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.ipay88CallbackHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.Ipay88CallbackHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CCAvenueCallbackHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CCAvenueCallbackHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.WPCyberSourceCallbackHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.WPCyberSource.WPCyberSourceCallbackHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.CardConnectCallbackHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.CardConnectCallbackHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.ThawaniHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.Thawani.ThawaniPayHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PaytmHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.Paytm.PaytmHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PayFortHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.PayFort.PayFortHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.MonerisHostedPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.HostedPayment.Moneris.MonerisHostedPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PineLabsQRPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PineLabsPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PineLabsCardPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PineLabsPaymentGateway,Device";
                        break;
                    }
                case PaymentGateways.PaytmDQRPayment:
                    {
                        returnValue = "Semnox.Parafait.Device.PaymentGateway.PaytmDQRPaymentGateway,Device";
                        break;
                    }
            }

            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool CanCreateMultipleInstances(PaymentGateways gateway)
        {
            log.LogMethodEntry(gateway);
            bool returnValue = true;
            switch (gateway)
            {
                case PaymentGateways.FirstData:
                    {
                        returnValue = !isUnattended;
                        break;
                    }
                case PaymentGateways.ElementExpress:
                    {
                        returnValue = !isUnattended;
                        break;
                    }
                case PaymentGateways.Quest:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Mercury:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.NCR:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.ChinaICBC:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Moneris:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.WeChat:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.CardConnect:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Chase:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Worldpay:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Innoviti:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.CyberSource:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.TransBank:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.AEONCredit:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Clover:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.FreedomPay:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Adyen:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Alipay:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Ipay:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Mada:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.NetEpay:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.MashreqKiosk:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.WorldPayService:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.CloverCardConnectPaymentGateway:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Mashreq:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.Geidea:
                    {
                        returnValue = false;
                        break;
                    }
                case PaymentGateways.ClubSpeedGiftCard:
                    {
                        returnValue = false;
                        break;
                    }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }

    /// <summary>
    /// Represents payment gateway configuration error that occur during application execution. 
    /// </summary>
    public class PaymentGatewayConfigurationException : Exception
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of PaymentGatewayConfigurationException.
        /// </summary>
        public PaymentGatewayConfigurationException()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Initializes a new instance of PaymentGatewayConfigurationException class with a specified error message.
        /// </summary>
        /// <param name="message">message</param>
        public PaymentGatewayConfigurationException(string message)
        : base(message)
        {
            log.LogMethodEntry(message);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Initializes a new instance of PaymentGatewayConfigurationException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="inner">inner exception</param>
        public PaymentGatewayConfigurationException(string message, Exception inner)
        : base(message, inner)
        {
            log.LogMethodEntry(message, inner);
            log.LogMethodExit(null);
        }
    }
}
