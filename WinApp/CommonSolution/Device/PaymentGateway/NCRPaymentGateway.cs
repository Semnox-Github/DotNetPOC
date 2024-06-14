//using Semnox.Parafait.PaymentGateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
//using Semnox.Parafait.TransactionPayments;
using Semnox.Parafait.Site;

using Semnox.Parafait.logging;
namespace Semnox.Parafait.Device.PaymentGateway
{
    class NCRPaymentGateway : PaymentGateway
    {
        NCRAdapter nCRAdapter;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public NCRPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

            if (!isUnattended)
            {
                string Message = "";
                //object o = Utilities.executeScalar("select version from site where site_id=@siteid", new SqlParameter("@siteid", Utilities.ParafaitEnv.SiteId));
                string version = "Parafait:000.000.000.001";
                SiteList siteList = new SiteList(executionContext);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchVersionParameter = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchVersionParameter.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, utilities.ParafaitEnv.SiteId.ToString()));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchVersionParameter);

                if (siteDTOList != null && siteDTOList.Count > 0 && !string.IsNullOrEmpty(siteDTOList[0].Version))
                {
                    version = "Parafait:" + siteDTOList[0].Version;                    
                }
                nCRAdapter = new NCRAdapter(utilities.ParafaitEnv.LoginID, utilities.getParafaitDefaults("NCR_LANE_NO"), version, ref Message, utilities);
            }
            else
            {
                string Message = "";
                string version = "Kiosk:000.000.000.001";
                SiteList siteList = new SiteList(executionContext);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchVersionParameter = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchVersionParameter.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, utilities.ParafaitEnv.SiteId.ToString()));
                List<SiteDTO> siteDTOList = siteList.GetAllSites(searchVersionParameter);

                if (siteDTOList != null && siteDTOList.Count > 0 && !string.IsNullOrEmpty(siteDTOList[0].Version))
                {
                    version = "Kiosk" + siteDTOList[0].Version;
                }
                nCRAdapter = new NCRAdapter(utilities.getParafaitDefaults("ALOHA_USER_ID"), utilities.getParafaitDefaults("NCR_LANE_NO"), version, ref Message, utilities);
                
                if(Message != null && writeToLogDelegate != null)
                {
                    writeToLogDelegate(Convert.ToInt32(-1), "NCR initialization failed", Convert.ToInt32(-1), 0, Message, utilities.ParafaitEnv.POSMachineId, utilities.ParafaitEnv.POSMachine);
                }
            }

            log.LogMethodExit(null);
        }

        public override void BeginOrder()
        {
            log.LogMethodEntry();
            nCRAdapter.BeginOrder();
            log.LogMethodExit(null);
        }

        public override void EndOrder()
        {
            log.LogMethodEntry();
            nCRAdapter.EndOrder();
            log.LogMethodExit(null);
        }

        ///<summary>
        ///Makes Payment.
        ///</summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string Message = "";
            NCRRequest ncrRequest = new NCRRequest();
            NCRResponse ncrResponse = new NCRResponse();
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            int.TryParse((transactionPaymentsDTO.Amount * 100).ToString(), out ncrRequest.Amount);
            ncrRequest.TransactionType = eTransactionType.PURCHASE;
            ncrRequest.PostTransactionNumber = transactionPaymentsDTO.TransactionId.ToString();
            ncrResponse = nCRAdapter.ProcessTransaction(ncrRequest, ref Message);
            if (ncrResponse != null && !string.IsNullOrEmpty(ncrResponse.ResponseText))
            {
                transactionPaymentsDTO.CCResponseId = ncrResponse.ccResponseId;
                transactionPaymentsDTO.CreditCardAuthorization = (ncrResponse.AuthCode == null) ? "" : ncrResponse.AuthCode.ToString();
                transactionPaymentsDTO.NameOnCreditCard = (ncrResponse.CustomerName == null) ? "" : ncrResponse.CustomerName.ToString();
                transactionPaymentsDTO.CreditCardNumber = (ncrResponse.CardNo == null) ? "" : ncrResponse.CardNo.ToString();
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    //NCRAdapter.PrintCCReceipt(ncrResponse.CustomerReceiptText);
                    transactionPaymentsDTO.Memo = ncrResponse.CustomerReceiptText;
                }
                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !NCRAdapter.IsUnattended)
                {
                    //NCRAdapter.PrintCCReceipt(ncrResponse.MerchantReceiptText);
                    transactionPaymentsDTO.Memo += ncrResponse.MerchantReceiptText;
                }
                if (!ncrResponse.ResponseText.Contains("Approved") || !ncrRequest.TransactionType.Equals(eTransactionType.PURCHASE))
                {
                    //transactionPaymentsDTO.GatewayPaymentProcessed = false;
                    //return false;
                    log.LogMethodExit(null, Message);
                    throw new Exception(Message);
                }
            }
            else
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException");
                throw new PaymentGatewayException();
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
        ///<summary>
        ///Reverts the Payment.
        ///</summary>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            string Message = "";
            NCRRequest ncrRequest = new NCRRequest();
            NCRResponse ncrResponse = new NCRResponse();
            int.TryParse((transactionPaymentsDTO.Amount * 100).ToString(), out ncrRequest.Amount);
            if (NCRAdapter.IsUnattended)
            {
                ncrRequest.TransactionType = eTransactionType.VOID_BY_MTX;
            }
            else
            {
                ncrRequest.TransactionType = eTransactionType.RETURN;
            }
            transactionPaymentsDTO.CCResponseId = ncrResponse.ccResponseId;
            ncrRequest.PostTransactionNumber = transactionPaymentsDTO.TransactionId.ToString();
            ncrResponse = nCRAdapter.ProcessTransaction(ncrRequest, ref Message);
            if (ncrResponse != null && !string.IsNullOrEmpty(ncrResponse.ResponseText))
            {
                transactionPaymentsDTO.CCResponseId = ncrResponse.ccResponseId;
                transactionPaymentsDTO.CreditCardAuthorization = (ncrResponse.AuthCode == null) ? "" : ncrResponse.AuthCode.ToString();
                transactionPaymentsDTO.NameOnCreditCard = (ncrResponse.CustomerName == null) ? "" : ncrResponse.CustomerName.ToString();
                transactionPaymentsDTO.CreditCardNumber = (ncrResponse.CardNo == null) ? "" : ncrResponse.CardNo.ToString();
                if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                {
                    //nCRAdapter.PrintCCReceipt(ncrResponse.CustomerReceiptText);
                    transactionPaymentsDTO.Memo = ncrResponse.CustomerReceiptText;
                }
                if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !NCRAdapter.IsUnattended)
                {
                    //nCRAdapter.PrintCCReceipt(ncrResponse.MerchantReceiptText);
                    transactionPaymentsDTO.Memo += ncrResponse.MerchantReceiptText;
                }
                if (!ncrResponse.ResponseText.Contains("Approved") || !ncrRequest.TransactionType.Equals(eTransactionType.RETURN))
                {
                    //transactionPaymentsDTO.GatewayPaymentProcessed = false;
                    //return false;
                    throw new PaymentGatewayException();
                }
            }
            else
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException");
                throw new PaymentGatewayException();
            }

            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }
    }
}
