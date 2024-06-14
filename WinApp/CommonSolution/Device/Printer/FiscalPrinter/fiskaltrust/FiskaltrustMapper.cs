/********************************************************************************************
 * Project Name - Device
 * Description  - Business Logic to create Fiskaltrust ReceiptRequest object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
 *2.90.0     14-Jul-2020      Gururaja Kanjan    Created for fiskaltrust integration.
*2.110.0     22-Dec-2020      Girish Kundar      Modified :FiscalTrust changes - Shift open/Close/PayIn/PayOut to be fiscalized
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
//using System.Globalization;
using System.Linq;
using System.Reflection;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrinter.fiskaltrust;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class FiskaltrustMapper
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // private const string LOOKUP_NAME = "FISCALTRUST_INTEGRATION";
        //private static Dictionary<string, string> LOOKUP_DICTIONARY = new Dictionary<string, string>();
        // private const string CASH_BOX_ID_KEY = "FISCAL_CASH_REGISTER_ID";
        private static string FISKALTRUST_CASHBOX;
        // private const string FISCALIZATION_ERROR_MESSAGE_KEY = "FISCALIZATION_ERROR_MESSAGE";
        private readonly ExecutionContext executionContext;

        public FiskaltrustMapper(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            FISKALTRUST_CASHBOX = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_CASH_REGISTER_ID");
            if (string.IsNullOrEmpty(FISKALTRUST_CASHBOX))
            {
                log.Error("FISKALTRUST_CASHBOX is not set .Please check the set up");
                return;
            }
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Provides ReceiptRequest object for an intial receipt transaction.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns>ReceiptRequest</returns>
        public ReceiptRequest GetInitialRequest(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ReceiptRequest receipt = new ReceiptRequest();
            try
            {
                int siteId = executionContext.GetSiteId();
                string posId = executionContext.GetMachineId().ToString();
                log.Debug("Site ID is :" + siteId);
                log.Debug("POS  ID is :" + posId);
                receipt.ftCashBoxID = FISKALTRUST_CASHBOX;
                receipt.cbTerminalID = posId;
                Type type = Type.GetType("Semnox.Parafait.POS.POSMachines,POS");
                object posMachines = null;
                if (type != null)
                {
                    ConstructorInfo constructorN = type.GetConstructor(new Type[] { executionContext.GetType(), typeof(int), typeof(bool), typeof(bool), typeof(SqlTransaction) });
                    posMachines = constructorN.Invoke(new object[] { executionContext, Convert.ToInt32(posId), false, false, null });
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 1479, "posMachine"));
                }
                var posObject = (object)type.GetProperty("POSMachineDTO").GetValue(posMachines);
                object x = posObject.GetType().GetProperties()
                                    .Single(pi => pi.Name == "Attribute1")
                                    .GetValue(posObject, null);
                receipt.ftPosSystemId = x.ToString();

                if (string.IsNullOrWhiteSpace(receipt.ftPosSystemId))
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, "FTPosSystemId is not set up"));
                }
                //during publish of initial receipt we will the current time as receipt reference.
                DateTime currentDate = ServerDateTime.Now;
                receipt.cbReceiptMoment = currentDate;
                receipt.cbReceiptReference = currentDate.ToString("MMddyyyyHHmm");
                receipt.ftReceiptCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("CASE_OPEN_RECEIPT")); // 4919338172267102211
                receipt.cbUser = executionContext.GetUserId();
                receipt.cbChargeItems = new ChargeItem[0];
                receipt.cbPayItems = new PayItem[0];
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(receipt);
            return receipt;
        }


        /// <summary>
        /// Provides ReceiptRequest to be processed for fiskalization form FiscalizationRequest, 
        /// a wrapper for transaction object. 
        /// </summary>
        /// <param name="fiscalizationRequest"></param>
        /// <returns>ReceiptRequest</returns>
        public ReceiptRequest GetFiskaltrustRequest(FiscalizationRequest fiscalizationRequest)
        {
            log.LogMethodEntry(fiscalizationRequest);
            ReceiptRequest receipt = new ReceiptRequest();
            receipt.ftCashBoxID = FISKALTRUST_CASHBOX;
            log.Debug("ftCashBoxID : " + FISKALTRUST_CASHBOX);
            receipt.ftPosSystemId = fiscalizationRequest.ftPOSSystemId;
            if (string.IsNullOrEmpty(receipt.ftPosSystemId))
            {
                log.Debug("[possystem_id is the GUID of a POS System. Mandatory. Please add it in the Parfait defaults");
                throw new Exception(MessageContainerList.GetMessage(executionContext, "POS system id is not setup for fiskaltrust request"));
            }
            log.Debug("ftPosSystemId : " + receipt.ftPosSystemId);
            receipt.cbTerminalID = fiscalizationRequest.posId;
            log.Debug("cbTerminalID : " + receipt.cbTerminalID);
            receipt.cbReceiptReference = fiscalizationRequest.transactionId.ToString();
            log.Debug("transactionId : " + receipt.cbReceiptReference);
            receipt.cbReceiptMoment = fiscalizationRequest.receiptMoment;
            receipt.ftReceiptCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEFAULT_RECEIPT_CASE"));
            log.Debug("ftReceiptCase :" + receipt.ftReceiptCase);
            receipt.cbCustomer = fiscalizationRequest.customerEmail;
            receipt.cbUser = executionContext.GetUserId();
            // add the receipt reference for multi transaction scenario (typically reservation)
            if (fiscalizationRequest.isReversal)
            {
                receipt.cbPreviousReceiptReference = fiscalizationRequest.originalTransactionId.ToString();
            }
            TransactionLine[] transactions = fiscalizationRequest.transactionLines;
            PaymentInfo[] payments = fiscalizationRequest.payments;

            List<ChargeItem> chargeItemList = new List<ChargeItem>();
            if (transactions != null)
            {
                foreach (TransactionLine trxLine in transactions)
                {
                    ChargeItem chargeItem = new ChargeItem();
                    chargeItem.Position = trxLine.position;
                    chargeItem.Amount = trxLine.amount;
                    chargeItem.Quantity = trxLine.quantity;
                    chargeItem.Description = trxLine.description;
                    chargeItem.VATRate = trxLine.VATRate;
                    /*This is when shift Pay In Pay out open operation. The charge itemcase will be different . amount is Positive for pay in negative for Pay out  */
                    if (fiscalizationRequest.isPayIn || fiscalizationRequest.isPayOut)
                    {
                        chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("PAYIN_OUT_CHARGEITEM_CASE"));
                        receipt.cbReceiptReference = fiscalizationRequest.extReference.ToString();
                        receipt.ftReceiptCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEPOSIT_RECEIPT_CASE"));
                    }
                    /*This is when shift Shift open operation. The charge itemcase will be different.The amount is Positive  */
                    else if (fiscalizationRequest.isShiftIn)
                    {
                        chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEPOSIT_CHARGEITEM_CASE"));
                        receipt.cbReceiptReference = fiscalizationRequest.extReference.ToString();
                        receipt.ftReceiptCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEPOSIT_RECEIPT_CASE"));// 4919338167972134932 - pos-action-shift-03092020

                    }
                    /*This is when shift close operation. The charge itemcase will be different . The amount is negative  */
                    else if (fiscalizationRequest.isShiftOut)
                    {
                        chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEPOSIT_CHARGEITEM_CASE"));
                        receipt.cbReceiptReference = fiscalizationRequest.extReference.ToString();
                        receipt.ftReceiptCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEPOSIT_RECEIPT_CASE"));
                    }

                    /*This is when we need to add the advance amount . added as line item. charge itemcase will be different when amount is positive  */
                    else if (string.IsNullOrEmpty(trxLine.description) == false && trxLine.description.Contains("Down Payment"))
                    {
                        chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DOWNPAYMENT_CHARGEITEM_CASE"));
                    }

                    /*This is when we need to subtract the advance amount . added as line item. charge itemcase will be different when amount is negative  */
                    else if (string.IsNullOrEmpty(trxLine.description) == false && trxLine.description.Contains("Settlement"))
                    {
                        try
                        {
                            chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DOWNPAYMENT_CHARGEITEM_FULL_CASE"));
                        }
                        catch (FormatException)
                        {
                            chargeItem.ftChargeItemCase = Int64.Parse("4919338167972135049");
                        }
                        catch (Exception)
                        {
                            chargeItem.ftChargeItemCase = Int64.Parse("4919338167972135049");
                        }
                    }
                    else
                    {
                        if (chargeItem.VATRate == 0)  //0 % vat
                        {
                            chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("ZERO_TAX_CHARGEITEM_CASE"));
                        }
                        else if (chargeItem.VATRate == 7) // 7 % vat
                        {
                            try
                            {
                                chargeItem.ftChargeItemCase = Convert.ToInt64(Convert.ToString(FiscaltrustDefaults.GetFiscaltrustDefault("SEVEN_PERC_TAX_CHARGEITEM_CASE")));
                            }
                            catch (FormatException)
                            {
                                chargeItem.ftChargeItemCase = Int64.Parse("4919338167972134914");
                            }
                            catch (Exception)
                            {
                                chargeItem.ftChargeItemCase = Int64.Parse("4919338167972134914");
                            }
                        }
                        else if (chargeItem.VATRate == 19) // 19 % vat
                        {
                            try
                            {
                                chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("NINTEEN_PERC_TAX_CHARGEITEM_CASE"));
                            }
                            catch (FormatException)
                            {
                                chargeItem.ftChargeItemCase = Int64.Parse("4919338167972134913");
                            }
                            catch (Exception)
                            {
                                chargeItem.ftChargeItemCase = Int64.Parse("4919338167972134913");
                            }
                        }
                        else // default
                        {
                            chargeItem.ftChargeItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEFAULT_CHARGEITEM_CASE"));

                        }
                    }
                    chargeItem.VATAmount = trxLine.VATAmount;
                    chargeItem.Moment = fiscalizationRequest.receiptMoment;
                    log.Debug("ftChargeItemCase :" + chargeItem.ftChargeItemCase);
                    log.Debug("ftReceiptCase :" + receipt.ftReceiptCase);

                    chargeItemList.Add(chargeItem);
                }
                receipt.cbChargeItems = chargeItemList.ToArray();
            }


            if (payments != null)
            {
                List<PayItem> payItemList = new List<PayItem>();
                foreach (PaymentInfo payment in payments)
                {
                    // below check ensures the fiscalized receipts are not published again
                    if (String.IsNullOrEmpty(payment.reference))
                    {
                        PayItem pi = new PayItem();
                        pi.Quantity = 1;
                        pi.Amount = payment.amount;
                        pi.Moment = payment.moment;

                        //if a particular pay mode is not defined will be defaulted to cash transaction.
                        String modeKey = payment.paymentMode.ToUpper();
                        if (string.IsNullOrEmpty(FiscaltrustDefaults.GetFiscaltrustDefault(modeKey)) == false)
                        {
                            pi.ftPayItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault(modeKey));
                        }
                        else
                        {
                            pi.ftPayItemCase = Convert.ToInt64(FiscaltrustDefaults.GetFiscaltrustDefault("DEFAULT_PAYITEM_CASE"));
                        }

                        modeKey = string.Concat(modeKey, "_DESCRIPTION");
                        if (string.IsNullOrEmpty(FiscaltrustDefaults.GetFiscaltrustDefault(modeKey)) == false)
                        {
                            pi.Description = FiscaltrustDefaults.GetFiscaltrustDefault(modeKey);
                        }
                        else
                        {
                            pi.Description = FiscaltrustDefaults.GetFiscaltrustDefault("DEFAULT_PAYITEM_CASE_DESCRIPTION");
                        }
                        log.Debug("ftPayItemCase :" + pi.ftPayItemCase);
                        log.Debug("ft Description :" + pi.Description);
                        payItemList.Add(pi);
                    }
                }
                receipt.cbPayItems = payItemList.ToArray();
            }

            log.LogMethodExit(receipt);
            return receipt;
        }

        /// <summary>
        /// Returns the fiscalized signature received from fiskaltrust response
        /// </summary>
        /// <param name="receiptResponse">response from fiskaltrust</param>
        /// <returns>Signature</returns>
        public string GetSignature(ReceiptResponse receiptResponse)
        {
            log.LogMethodEntry(receiptResponse);
            string signature = string.Empty;
            SignaturItem[] ftSignatures = receiptResponse.ftSignatures;
            for (int i = 0; i < ftSignatures.Length; i++)
            {
                log.Debug("Signature Type =" + ftSignatures[i].ftSignatureType);
                log.Debug("\tData = " + ftSignatures[i].Data);
                if (ftSignatures[i].ftSignatureType.ToString().Equals(FiscaltrustDefaults.GetFiscaltrustDefault("SIGNATURE_TYPE")))
                {
                    signature = ftSignatures[i].Data;
                    break;
                }
            }
            log.LogMethodExit(signature);
            return signature;
        }

        /// <summary>
        /// Returns the fiscalized signature in the TransactionPaymentsDTO row
        /// </summary>
        /// <param name="paymentList">payment lines in transaction being processed</param>
        /// <returns>Signature</returns>
        public string GetSignature(List<TransactionPaymentsDTO> paymentList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(paymentList);
            DateTime tempDate = new DateTime(2000, 1, 1);
            DateTime creationTime = new DateTime(2000, 1, 1);
            string signature = string.Empty;
            // Need to verify 
            int paymentId = 0;
            if (paymentList != null)
            {
                log.Debug("Count of payment " + paymentList.Count);
                // Get the latest payment transaction
                TransactionPaymentsDTO transactionPaymentsDTO = paymentList.OrderByDescending(x => x.PaymentId).FirstOrDefault();
                TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, transactionPaymentsDTO.PaymentId);
                paymentId = transactionPaymentsDTO.PaymentId;
                signature = transactionPaymentsBL.TransactionPaymentsDTO.ExternalSourceReference;
                log.Debug("\t Latest transaction :" + paymentId);
                log.Debug("\t Transaction date :" + creationTime);


            }

            log.Debug("\t Latest transaction :" + paymentId);
            log.Debug("\t Transaction date :" + creationTime);
            log.LogMethodExit(signature);
            return signature;
        }

        /// <summary>
        /// Returns standard message for failed fiscalization
        /// </summary>
        /// <returns>Signature</returns>
        public string GetSingatureErrorMessage()
        {
            log.LogMethodEntry();
            string signatureMessage = FiscaltrustDefaults.GetFiscaltrustDefault("FISCALIZATION_ERROR_MESSAGE");
            log.LogMethodExit(signatureMessage);
            return signatureMessage;
        }
    }
}
