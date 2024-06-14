using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
//using Semnox.Mercury.PaymentGateway;
//using ParafaitQuestEFTPOS;
using System.Windows.Forms;//Quest:starts,Ends
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Communication;
using ExecutionContext = Semnox.Core.Utilities.ExecutionContext;
using Semnox.Parafait.Languages;
using System.Configuration;

namespace Semnox.Parafait.Transaction
{
    public static class CreditCardPaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool MakePayment(TransactionPaymentsDTO trxPaymentDTO, Utilities utilities, ref string message)
        {
            bool canRunInThreadModeForCCRun = CanRunInThreadModeForCCRun();
            bool retValue;
            if (canRunInThreadModeForCCRun)
            {
                string msgValue = message;
                KeyValuePair<bool, string> makePaymentResult = Core.GenericUtilities.BackgroundProcessRunner.Run<KeyValuePair<bool, string>>(() => { return MakePayment(trxPaymentDTO, utilities, msgValue); });
                retValue = makePaymentResult.Key;
                message = makePaymentResult.Value;
                return retValue;
            }
            else
            {
                retValue = MakePayment(trxPaymentDTO, utilities.ExecutionContext, ref message);
            }
            return retValue;
        }

        private static KeyValuePair<bool, string> MakePayment(TransactionPaymentsDTO trxPaymentDTO, Utilities utilities, string message)
        {

            KeyValuePair<bool, string> retValue;
            bool noError = false;
            try
            {
                bool makePaymentResult = MakePayment(trxPaymentDTO, utilities.ExecutionContext, ref message);
                retValue = new KeyValuePair<bool, string>(makePaymentResult, message);
                noError = true;
            } 
            finally
            {
                if (noError == false)
                {
                    retValue = new KeyValuePair<bool, string>(false, message);
                }
            }
            return retValue;
        }
        private static bool MakePayment(TransactionPaymentsDTO trxPaymentDTO, Core.Utilities.ExecutionContext executionContext, ref string message)
        {
            log.LogMethodEntry(trxPaymentDTO, executionContext, message);

            Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO = new Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO();
            transactionPaymentsDTO = trxPaymentDTO;
            //transactionPaymentsDTO.TransactionId = PaymentDetail.TrxId;
            //transactionPaymentsDTO.Amount = (PaymentDetail.Amount - PaymentDetail.TipAmount);
            //transactionPaymentsDTO.PaymentModeId = Convert.ToInt32(PaymentDetail.PaymentModeId);
            //transactionPaymentsDTO.CreditCardNumber = PaymentDetail.CreditCardNumber;
            //transactionPaymentsDTO.CreditCardExpiry = PaymentDetail.CreditCardExpiry;
            //transactionPaymentsDTO.CreditCardAuthorization = PaymentDetail.CreditCardAuthorization;
            //transactionPaymentsDTO.NameOnCreditCard = PaymentDetail.NameOnCard;
            //transactionPaymentsDTO.CreditCardName = PaymentDetail.CreditCardName;
            //transactionPaymentsDTO.Reference = PaymentDetail.Reference;
            //transactionPaymentsDTO.TipAmount = PaymentDetail.TipAmount;
            //transactionPaymentsDTO.SplitId = PaymentDetail.SplitId;
            //transactionPaymentsDTO.Memo = PaymentDetail.Memo;
            transactionPaymentsDTO.CurrencyCode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_CODE");
            try
            {
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.paymentModeDTO.GatewayLookUp); 
                transactionPaymentsDTO = paymentGateway.MakePayment(transactionPaymentsDTO);
                if (transactionPaymentsDTO != null)
                {
                    trxPaymentDTO.CreditCardAuthorization = transactionPaymentsDTO.CreditCardAuthorization;
                    trxPaymentDTO.Reference = transactionPaymentsDTO.Reference;
                    if (transactionPaymentsDTO.CCResponseId != -1)
                    {
                        trxPaymentDTO.CCResponseId = transactionPaymentsDTO.CCResponseId;
                    }
                    else
                    {
                        trxPaymentDTO.CCResponseId = -1;
                    }
                    trxPaymentDTO.CreditCardNumber = transactionPaymentsDTO.CreditCardNumber;
                    trxPaymentDTO.CreditCardExpiry = transactionPaymentsDTO.CreditCardExpiry;
                    trxPaymentDTO.NameOnCreditCard = transactionPaymentsDTO.NameOnCreditCard;
                    trxPaymentDTO.CreditCardName = transactionPaymentsDTO.CreditCardName;
                    trxPaymentDTO.Amount = transactionPaymentsDTO.Amount;
                    trxPaymentDTO.TipAmount = transactionPaymentsDTO.TipAmount;
                    trxPaymentDTO.Memo = transactionPaymentsDTO.Memo;
                    trxPaymentDTO.CustomerCardProfileId = transactionPaymentsDTO.CustomerCardProfileId;
                    trxPaymentDTO.GatewayPaymentProcessed = true;
                }
                else
                {
                    //This part executes for only if PaymentGateway is None  
                    log.LogVariableState("Message ", message);
                    log.LogMethodEntry(true);                 
                    return true;
                }
            }
            catch (Exception e)
            {
                log.Error("Error occured while processing credit card payment ", e);
                trxPaymentDTO.GatewayPaymentProcessed = false;
                message = e.Message;

                log.LogVariableState("PaymentDetail.GatewayPaymentProcessed ", trxPaymentDTO.GatewayPaymentProcessed);
                log.LogVariableState("Message ", message);
                log.LogMethodExit(false);
                return false;
            }

            bool returnValueNew = trxPaymentDTO.GatewayPaymentProcessed;
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
        public static bool RefundAmount(TransactionPaymentsDTO trxPaymentDTO, Utilities utilities, ref string message)
        {
            log.LogMethodEntry(trxPaymentDTO, utilities, message);
            try
            {
                bool canRunInThreadModeForCCRun = CanRunInThreadModeForCCRun();
                bool retValue;
                if (canRunInThreadModeForCCRun)
                {
                    string msgValue = message;
                    KeyValuePair<bool, string> makeRefundResult = Core.GenericUtilities.BackgroundProcessRunner.Run<KeyValuePair<bool, string>>(() => { return RefundCreditCardAmount(trxPaymentDTO, utilities, msgValue); });
                    retValue = makeRefundResult.Key;
                    message = makeRefundResult.Value;
                }
                else
                {
                    retValue = RefundCreditCardAmount(trxPaymentDTO, utilities, ref message);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while refunding credit card payment", ex);
                trxPaymentDTO.GatewayPaymentProcessed = true;
                message = ex.Message; 
                log.LogVariableState("Message ", message);
                log.LogMethodExit(false);
                return false;
            }
        }
        private static KeyValuePair<bool, string> RefundCreditCardAmount(TransactionPaymentsDTO trxPaymentDTO, Utilities utilities, string message)
        {

            KeyValuePair<bool, string> retValue;
            bool noError = false;
            try
            {
                bool makePaymentResult = RefundCreditCardAmount(trxPaymentDTO, utilities, ref message);
                retValue = new KeyValuePair<bool, string>(makePaymentResult, message);
                noError = true;
            }
            finally
            {
                if (noError == false)
                {
                    retValue = new KeyValuePair<bool, string>(false, message);
                }
            }
            return retValue;
        }
        private static bool RefundCreditCardAmount(TransactionPaymentsDTO trxPaymentDTO, Utilities utilities, ref string Message)
        {
            log.LogMethodEntry(trxPaymentDTO, utilities, Message);
            try
            { 
                if (trxPaymentDTO.CCResponseId > -1)
                {
                    CCTransactionsPGWBL ccOrigTransactionsPGWBL = new CCTransactionsPGWBL(trxPaymentDTO.CCResponseId);
                    if (Convert.ToDouble(ccOrigTransactionsPGWBL.CCTransactionsPGWDTO.Authorize) == 0 && (ccOrigTransactionsPGWBL.CCTransactionsPGWDTO.TranCode == PaymentGatewayTransactionType.TATokenRequest.ToString()))
                    {
                        return true;
                    }
                }
                trxPaymentDTO.Amount = Math.Abs(trxPaymentDTO.Amount);
                trxPaymentDTO.TipAmount = Math.Abs(trxPaymentDTO.TipAmount);
                TransactionPaymentsDTO sendTransactionPaymentDTO = trxPaymentDTO;
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(trxPaymentDTO.paymentModeDTO.GatewayLookUp);
                Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO reversedTransactionPaymentsDTO = paymentGateway.RefundAmount(sendTransactionPaymentDTO);
                if (reversedTransactionPaymentsDTO != null)
                {
                    trxPaymentDTO.CreditCardAuthorization = reversedTransactionPaymentsDTO.CreditCardAuthorization;
                    trxPaymentDTO.Reference = reversedTransactionPaymentsDTO.Reference;

                    if (reversedTransactionPaymentsDTO.CCResponseId != -1)
                    {
                        trxPaymentDTO.CCResponseId = reversedTransactionPaymentsDTO.CCResponseId;
                    }
                    else
                    {
                        trxPaymentDTO.CCResponseId = -1;
                    }

                    trxPaymentDTO.CreditCardNumber = reversedTransactionPaymentsDTO.CreditCardNumber;
                    trxPaymentDTO.CreditCardExpiry = reversedTransactionPaymentsDTO.CreditCardExpiry;
                    trxPaymentDTO.NameOnCreditCard = reversedTransactionPaymentsDTO.NameOnCreditCard;
                    trxPaymentDTO.Amount = Math.Abs(reversedTransactionPaymentsDTO.Amount);
                    trxPaymentDTO.TipAmount = Math.Abs(reversedTransactionPaymentsDTO.TipAmount);
                    trxPaymentDTO.CreditCardName = reversedTransactionPaymentsDTO.CreditCardName;
                    trxPaymentDTO.Memo = reversedTransactionPaymentsDTO.Memo;
                    trxPaymentDTO.GatewayPaymentProcessed = false;

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    trxPaymentDTO.GatewayPaymentProcessed = false;

                    log.LogVariableState("Message ", Message);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while refunding credit card payment", ex);
                trxPaymentDTO.GatewayPaymentProcessed = true;
                Message = ex.Message;

                log.LogVariableState("Message ", Message);
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Print the cc receipt 
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        public static void PrintCCReceipt(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            bool canRunInThreadModeForCCRun = CanRunInThreadModeForCCRun(); 
            if (canRunInThreadModeForCCRun)
            {
                Core.GenericUtilities.BackgroundProcessRunner.Run(() => { PrintCreditCardReceipt(transactionPaymentsDTO); });
            }
            else
            {
                PrintCreditCardReceipt(transactionPaymentsDTO);
            }
            log.LogMethodExit();
        }

        private static void PrintCreditCardReceipt(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);  
            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.paymentModeDTO.GatewayLookUp);
            List<TransactionPaymentsDTO> transactionPaymentsDTOs = new List<TransactionPaymentsDTO>();
            transactionPaymentsDTOs.Add(transactionPaymentsDTO);
            paymentGateway.PrintCCReceipt(transactionPaymentsDTOs);
            log.LogMethodExit();
        }

        /// <summary>
        /// MakePaymentForRecurringBilling
        /// </summary>
        /// <param name="TrxPaymentDTO"></param>
        /// <param name="executionContext"></param>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static TransactionPaymentsDTO MakePaymentForRecurringBilling(TransactionPaymentsDTO TrxPaymentDTO, Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry(TrxPaymentDTO, executionContext);

            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
            transactionPaymentsDTO = TrxPaymentDTO;
            transactionPaymentsDTO.CurrencyCode = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_CODE");
            try
            {
                //PaymentGatewayFactory.GetInstance().Initialize(, true);
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionPaymentsDTO.paymentModeDTO.GatewayLookUp);
                transactionPaymentsDTO = paymentGateway.MakePaymentForRecurringBilling(transactionPaymentsDTO);
                if (transactionPaymentsDTO != null)
                {
                    TrxPaymentDTO.CreditCardAuthorization = transactionPaymentsDTO.CreditCardAuthorization;
                    TrxPaymentDTO.Reference = transactionPaymentsDTO.Reference;
                    if (transactionPaymentsDTO.CCResponseId != -1)
                    {
                        TrxPaymentDTO.CCResponseId = transactionPaymentsDTO.CCResponseId;
                    }
                    else
                    {
                        TrxPaymentDTO.CCResponseId = -1;
                    }
                    TrxPaymentDTO.CreditCardNumber = transactionPaymentsDTO.CreditCardNumber;
                    TrxPaymentDTO.CreditCardExpiry = transactionPaymentsDTO.CreditCardExpiry;
                    TrxPaymentDTO.NameOnCreditCard = transactionPaymentsDTO.NameOnCreditCard;
                    TrxPaymentDTO.CreditCardName = transactionPaymentsDTO.CreditCardName;
                    TrxPaymentDTO.Amount = transactionPaymentsDTO.Amount;
                    TrxPaymentDTO.TipAmount = transactionPaymentsDTO.TipAmount;
                    TrxPaymentDTO.Memo = transactionPaymentsDTO.Memo;
                    TrxPaymentDTO.GatewayPaymentProcessed = true;
                    TrxPaymentDTO.CustomerCardProfileId = transactionPaymentsDTO.CustomerCardProfileId;
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Unexpected error in payment gateway"));
                }
            }
            catch (Exception e)
            {
                log.Error("Error occured while processing credit card payment ", e);
                TrxPaymentDTO.GatewayPaymentProcessed = false;
                log.LogVariableState("PaymentDetail.GatewayPaymentProcessed ", TrxPaymentDTO.GatewayPaymentProcessed);
                throw;
            }

            log.LogMethodExit(TrxPaymentDTO);
            return TrxPaymentDTO;
        }
        /// <summary>
        /// GetCreditCardExpiryYear
        /// </summary>
        /// <param name="utilities"></param> 
        /// <param name="paymentModeDTO"></param>
        /// <param name="cardExpiryData"></param>
        /// <returns></returns>
        public static int GetCreditCardExpiryYear(Utilities utilities, PaymentModeDTO paymentModeDTO, string cardExpiryData)
        {
            log.LogMethodEntry(paymentModeDTO, cardExpiryData);
            int yearValue = 0;
            try
            {
                PaymentGatewayFactory.GetInstance().Initialize(utilities, true);
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModeDTO.GatewayLookUp);
                yearValue = paymentGateway.GetCreditCardExpiryYear(cardExpiryData);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

            log.LogMethodExit(yearValue);
            return yearValue;
        }
        /// <summary>
        /// GetCreditCardExpiryMonth
        /// </summary>
        /// <param name="utilities"></param> 
        /// <param name="paymentModeDTO"></param>
        /// <param name="cardExpiryData"></param>
        /// <returns></returns>
        public static int GetCreditCardExpiryMonth(Utilities utilities, PaymentModeDTO paymentModeDTO, string cardExpiryData)
        {
            log.LogMethodEntry(paymentModeDTO, cardExpiryData);
            int monthValue = 0;
            try
            {
                PaymentGatewayFactory.GetInstance().Initialize(utilities, true);
                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModeDTO.GatewayLookUp);
                monthValue = paymentGateway.GetCreditCardExpiryMonth(cardExpiryData);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }

            log.LogMethodExit(monthValue);
            return monthValue;
        }

        private static bool CanRunInThreadModeForCCRun()
        {
            log.LogMethodEntry();
            bool canRunInThreadModeForCCRun = true;
            string runInThreadModeForCCRun = ConfigurationManager.AppSettings["RUN_IN_THREAD_MODE_FOR_CC_CALL"];
            if (string.IsNullOrWhiteSpace(runInThreadModeForCCRun) == 
                false && (runInThreadModeForCCRun.ToUpper() == "NO" || runInThreadModeForCCRun.ToUpper() == "N"))
            {
                canRunInThreadModeForCCRun = false;
            }
            log.LogMethodExit(canRunInThreadModeForCCRun);
            return canRunInThreadModeForCCRun;
        }
    }
}

