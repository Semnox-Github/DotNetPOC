using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Semnox.Parafait.TransactionPayments;
//using Semnox.Parafait.PaymentGateway;
using System.Data.SqlClient;
using System.Windows.Forms;
 using Semnox.Core.Utilities;
using System.Net;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Mercury Payment Gateway
    /// </summary>
    public class MercuryPaymentGateway : Semnox.Parafait.Device.PaymentGateway.PaymentGateway
    {
        static SemnoxMercuryAdapter objSMAdapter = null;
        string Message;
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///Constructor of MercuryPaymentGateway
        /// </summary>
        public MercuryPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate) : base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; 
            if (System.Windows.Forms.Application.OpenForms != null && System.Windows.Forms.Application.OpenForms.Count > 0)
            {
                objSMAdapter = new SemnoxMercuryAdapter(System.Windows.Forms.Application.OpenForms[0].Handle, utilities);
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes payment.
        /// </summary>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            StandaloneRefundNotAllowed(transactionPaymentsDTO);
            if (objSMAdapter == null)
            {
                objSMAdapter = new SemnoxMercuryAdapter(System.Windows.Forms.Application.OpenForms[0].Handle, utilities);
            }
            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_PURCHASE);

            objSMAdapter.MakePayment(transactionPaymentsDTO.TransactionId.ToString(),
                (transactionPaymentsDTO.Amount).ToString("0.00"),
                transactionPaymentsDTO.TipAmount.ToString("0.00"),
                (transactionPaymentsDTO.CCResponseId == -1 ? (object)DBNull.Value : transactionPaymentsDTO.CCResponseId));

            bool timeOut = !objSMAdapter.mre.WaitOne(180000);
            if (timeOut)
            {
                Message = "Timeout Occured";

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }

            if (objSMAdapter.RetrunResponseMessage.CmdStatus != "Approved")
            {
                Message = objSMAdapter.RetrunResponseMessage.CmdStatus;

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }
            else
            {
                if (objSMAdapter.RetrunResponseMessage.TextResponse == "AP*")

                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();

                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, objSMAdapter.RetrunResponseMessage.InvoiceNo));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.REF_NO, objSMAdapter.RetrunResponseMessage.RefNo));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.AUTH_CODE, objSMAdapter.RetrunResponseMessage.AuthCode));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(searchParameters);

                    if (cCTransactionsPGWDTOList != null)
                    {
                        TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
                        foreach (var response in cCTransactionsPGWDTOList)
                        {
                            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchTransactionParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                            searchTransactionParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, response.ResponseID.ToString()));
                            searchTransactionParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(searchTransactionParameters);
                            if (transactionPaymentsDTOList != null)
                            {
                                Message = "Duplicate Mercury Payment";

                                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                                throw new PaymentGatewayException(Message);
                            }
                        }
                    }
                }

                transactionPaymentsDTO.Amount = Convert.ToDouble(objSMAdapter.RetrunResponseMessage.Authorize) * Math.Sign(transactionPaymentsDTO.Amount);
                transactionPaymentsDTO.TipAmount = Convert.ToDouble(string.IsNullOrEmpty(objSMAdapter.RetrunResponseMessage.Gratuity) ? "0.00" : objSMAdapter.RetrunResponseMessage.Gratuity) * Math.Sign(transactionPaymentsDTO.Amount);
                transactionPaymentsDTO.CCResponseId = Convert.ToInt32(objSMAdapter.RetrunResponseMessage.ResponseId);
                transactionPaymentsDTO.CreditCardAuthorization = objSMAdapter.RetrunResponseMessage.AuthCode;
                transactionPaymentsDTO.Reference = objSMAdapter.RetrunResponseMessage.RefNo;

                transactionPaymentsDTO.CreditCardNumber = objSMAdapter.RetrunResponseMessage.AcctNo;
                if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                    transactionPaymentsDTO.CreditCardName = objSMAdapter.RetrunResponseMessage.CardType;

                try
                {
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    PrintDocument myprnt = new PrintDocument(objSMAdapter.RetrunResponseMessage, utilities);
                    if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                    {
                        //myprnt.PrintReport(true);
                        transactionPaymentsDTO.Memo = myprnt.getReceiptText(true);
                        cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = transactionPaymentsDTO.Memo;
                    }
                    if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                    {
                        //myprnt.PrintReport(false);
                        cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = myprnt.getReceiptText(false);
                        transactionPaymentsDTO.Memo += cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy;

                    }
                    cCTransactionsPGWBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error("Unable to print the transaction");
                    utilities.EventLog.logEvent(PaymentGateways.Mercury.ToString(), 'D', ex.Message, ex.Message, CREDIT_CARD_PAYMENT, 3);

                    log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                    throw new PaymentGatewayException(ex.Message);
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
        }

        /// <summary>
        /// Reverts the payment.
        /// </summary>

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            if (objSMAdapter == null)
            {
                objSMAdapter = new SemnoxMercuryAdapter(System.Windows.Forms.Application.OpenForms[0].Handle, utilities);
            }
            CCRequestPGWDTO cCRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CCREQUEST_TRANSACTION_TYPE_REFUND);
            objSMAdapter.VoidTransaction(transactionPaymentsDTO.CCResponseId, transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount);//Modification on 09-Nov-2015:TipFeature

            bool timeOut = !objSMAdapter.mre.WaitOne(180000);
            if (timeOut)
            {
                Message = "Timeout Occured";

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }

            if (objSMAdapter.RetrunResponseMessage.CmdStatus != "Approved")
            {
                Message = objSMAdapter.RetrunResponseMessage.CmdStatus;

                log.LogMethodExit(null, "Throwing PaymentGatewayException - " + Message);
                throw new PaymentGatewayException(Message);
            }
            else
            {
                transactionPaymentsDTO.CCResponseId = Convert.ToInt32(objSMAdapter.RetrunResponseMessage.ResponseId);
                transactionPaymentsDTO.CreditCardAuthorization = objSMAdapter.RetrunResponseMessage.AuthCode;
                transactionPaymentsDTO.Reference = objSMAdapter.RetrunResponseMessage.RefNo;
                transactionPaymentsDTO.CreditCardNumber = objSMAdapter.RetrunResponseMessage.AcctNo;
                if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardName))
                    transactionPaymentsDTO.CreditCardName = objSMAdapter.RetrunResponseMessage.CardType;

                try
                {
                    CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    PrintDocument myprnt = new PrintDocument(objSMAdapter.RetrunResponseMessage, utilities);
                    if (utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y")
                    {
                        //myprnt.PrintReport(true);
                        transactionPaymentsDTO.Memo = myprnt.getReceiptText(true);
                        cCTransactionsPGWBL.CCTransactionsPGWDTO.MerchantCopy = transactionPaymentsDTO.Memo;
                    }
                    if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                    {
                        //myprnt.PrintReport(false);
                        cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy = myprnt.getReceiptText(false);
                        transactionPaymentsDTO.Memo += cCTransactionsPGWBL.CCTransactionsPGWDTO.CustomerCopy;

                    }
                    cCTransactionsPGWBL.Save();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while printing the credit card payment receipt", ex);
                    utilities.EventLog.logEvent(PaymentGateways.Mercury.ToString(), 'D', ex.Message, ex.Message, CREDIT_CARD_REFUND, 3);
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
        }

        /// <summary>
        /// Returns whether tip is allowed for the payment.
        /// </summary>
        public override bool IsTipAllowed(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            bool isTipAllowed = true;
            if (transactionPaymentsDTO == null || transactionPaymentsDTO.CreditCardName == null || transactionPaymentsDTO.CreditCardName.Contains("DEBIT"))
            {
                isTipAllowed = false;
            }
            if (transactionPaymentsDTO.CCResponseId == -1)
            {
                isTipAllowed = false;
            }

            log.LogMethodExit(isTipAllowed);
            return isTipAllowed;
        }

        /// <summary>
        /// Returns theTransactionDTO after tip payment.
        /// </summary>
        public override TransactionPaymentsDTO PayTip(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            TransactionPaymentsDTO returnTransactionPaymentsDTO = null;

            DateTime dt = Convert.ToDateTime(transactionPaymentsDTO.PaymentDate);
            int compare = dt.CompareTo(ServerDateTime.Now.AddDays(-1));
            if (compare < 0)
            {
                log.LogMethodExit(null, "Throwing PaymentGatewayException - Tip adjustment should be done with in 24 hours");
                throw new PaymentGatewayException("Tip adjustment should be done with in 24 hours.");
            }
            string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
            long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
            if (tipLimit > 0 && ((transactionPaymentsDTO.Amount * tipLimit) / 100) < transactionPaymentsDTO.TipAmount)
            {
                if (showMessageDelegate == null)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                }
                else
                {
                    showMessageDelegate(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                }                
                log.LogMethodExit();
                return null;
            }
            returnTransactionPaymentsDTO = MakePayment(transactionPaymentsDTO);
            if (returnTransactionPaymentsDTO != null)
            {
                transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount - transactionPaymentsDTO.TipAmount;
            }

            log.LogMethodExit(returnTransactionPaymentsDTO);
            return returnTransactionPaymentsDTO;
        }
    }
}

