/********************************************************************************************
 * Project Name -  Moneris Hosted Payment Gateway                                                                     
 * Description  -  Class to handle the payment of Moneris Payment Gateway
 *
 **************
 **Version Log
  *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 *2.150.3     15-May-2023      Muaaz Musthafa             Created for Website
 ********************************************************************************************/

using Moneris;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.HostedPayment.Moneris
{
    public class MonerisHostedCommandHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string _storeId;
        private string _apiToken;
        private string _checkoutId;
        private string _monerisApiUrl;

        private bool _isTestMode;

        private const string CATEGORY_OF_ECOMMERCE = "7";
        private const string PROCESSING_COUNTRY_CODE = "CA";
        private const bool STATUS_CHECK = false;


        public MonerisHostedCommandHandler(string storeId, string apiToken, string checkoutId, string monerisApiUrl, bool isTestMode)
        {
            log.LogMethodEntry(storeId, apiToken, checkoutId, monerisApiUrl);
            _storeId = storeId;
            _apiToken = apiToken;
            _checkoutId = checkoutId;
            _monerisApiUrl = monerisApiUrl;
            _isTestMode = isTestMode;
            log.LogMethodExit();
        }

        public Receipt MakeVoid(TransactionPaymentsDTO transactionPaymentsDTO, string refundTrxId)
        {
            Receipt voidReceipt = null;
            PurchaseCorrection purchasecorrection = new PurchaseCorrection();
            purchasecorrection.SetOrderId(refundTrxId);
            purchasecorrection.SetTxnNumber(transactionPaymentsDTO.Reference);
            purchasecorrection.SetCryptType(CATEGORY_OF_ECOMMERCE);

            HttpsPostRequest mpgReq = new HttpsPostRequest();
            mpgReq.SetProcCountryCode(PROCESSING_COUNTRY_CODE);
            mpgReq.SetTestMode(_isTestMode); //false or comment out this line for production transactions
            mpgReq.SetStoreId(_storeId);
            mpgReq.SetApiToken(_apiToken);
            mpgReq.SetTransaction(purchasecorrection);
            mpgReq.SetStatusCheck(STATUS_CHECK);
            mpgReq.Send();

            voidReceipt = GetReceiptDetails(mpgReq);
            return voidReceipt;
        }

        public Receipt MakeRefund(TransactionPaymentsDTO transactionPaymentsDTO, string refundTrxId)
        {
            Receipt refundReceipt = null;
            //Perform Moneris refund
            Refund refund = new Refund();
            refund.SetTxnNumber(transactionPaymentsDTO.Reference.ToString());
            refund.SetOrderId(refundTrxId);
            refund.SetAmount(string.Format("{0:0.00}", transactionPaymentsDTO.Amount));
            refund.SetCryptType(CATEGORY_OF_ECOMMERCE);

            HttpsPostRequest mpgReq = new HttpsPostRequest();
            mpgReq.SetProcCountryCode(PROCESSING_COUNTRY_CODE);
            mpgReq.SetTestMode(_isTestMode); //false or comment out this line for production transactions
            mpgReq.SetStoreId(_storeId);
            mpgReq.SetApiToken(_apiToken);
            mpgReq.SetTransaction(refund);
            mpgReq.SetStatusCheck(STATUS_CHECK);
            mpgReq.Send();

            refundReceipt = GetReceiptDetails(mpgReq);
            return refundReceipt;
        }

        private Receipt GetReceiptDetails(HttpsPostRequest mpgReq)
        {
            log.LogMethodEntry();
            Receipt receipt = null;
            try
            {
                receipt = mpgReq.GetReceipt();
                log.Debug($"CardType: {receipt.GetCardType()} TransAmount: {receipt.GetTransAmount()} TxnNumber: {receipt.GetTxnNumber()} ReceiptId: {receipt.GetReceiptId()} TransType: {receipt.GetTransType()} ReferenceNum: {receipt.GetReferenceNum()}  ResponseCode: {receipt.GetResponseCode()} ISO: {receipt.GetISO()} BankTotals: {receipt.GetBankTotals()} Message: {receipt.GetMessage()} AuthCode: {receipt.GetAuthCode()} Complete:{receipt.GetComplete()} TransDate: {receipt.GetTransDate()} TransTime: {receipt.GetTransTime()}  Ticket: {receipt.GetTicket()} TimedOut: {receipt.GetTimedOut()}");
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                throw;
            }

            log.LogMethodExit();
            return receipt;
        }
    }
}
