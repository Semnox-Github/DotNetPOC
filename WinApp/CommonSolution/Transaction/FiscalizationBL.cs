/********************************************************************************************
 * Project Name - Transaction
 * Description  - Helper for Fiskaltrust
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
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.POS;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    ///  GermanFiscalizationBL - Handles business logic of German fiskaltrust printer
    /// </summary>
    public class FiscalizationBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private Transaction transactionForFiscalization;

        /// <summary>
        /// Default constructor for FiscalizationBL class
        /// </summary>
        /// <param name="executionContext"></param>
        public FiscalizationBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="transaction"></param>
        public FiscalizationBL(ExecutionContext executionContext, Transaction transaction)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transaction);
            if (transaction == null || transaction.Trx_id < 0)
            {
                log.Error("transaction == null || transaction.Trx_id < 0");
                throw new Exception("Cannot fiscalize the transaction. ");
            }
            this.transactionForFiscalization = transaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// checks if transaction needs to be fiscalized or not.
        /// </summary>
        /// <returns>bool</returns>
        public bool NeedsFiscalization()
        {
            log.LogMethodEntry();
            bool needFiscalization = false;
            if (transactionForFiscalization.Status == Transaction.TrxStatus.CLOSED ||
                transactionForFiscalization.Status == Transaction.TrxStatus.RESERVED)
            {
                needFiscalization = true;
            }
            List<TransactionPaymentsDTO> paymentList = transactionForFiscalization.TransactionPaymentsDTOList;
            if (paymentList == null || paymentList.Any() == false)
            {
                log.Debug("No payment details found. Transaction will not be fiscalized");
                needFiscalization = false;
                return needFiscalization;
            }
            Boolean isReservation = transactionForFiscalization.IsReservationTransaction(null);
            log.Debug("isResevation :" + isReservation);
            if (isReservation)
            {
                needFiscalization = false;
                if (paymentList != null)
                {
                    foreach (TransactionPaymentsDTO paymentDTO in paymentList)
                    {
                        string paymentReference = paymentDTO.ExternalSourceReference;
                        if (string.IsNullOrWhiteSpace(paymentReference))
                        {
                            needFiscalization = true;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(needFiscalization);
            return needFiscalization;
        }

        /// <summary>
        /// Creates a FiscalizationRequest object from transaction
        /// </summary>
        /// <returns></returns>
        public FiscalizationRequest BuildFiscalizationRequest()
        {
            log.LogMethodEntry();
            bool result = ValidateTransactionPayments();
            string RoundingType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ROUNDING_TYPE");
            decimal RoundOffAmountTo = 0;
            try
            {
                RoundOffAmountTo = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ROUND_OFF_AMOUNT_TO", 100); //to be edited
                if (RoundOffAmountTo <= 0)
                    RoundOffAmountTo = 100;
            }
            catch
            {
                RoundOffAmountTo = 100;
            }
            if (result == false)
            {
                log.Debug("Transaction is already fiscalized. Two receipt printer might assigned to the POS");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Transaction is already fiscalized. TrnsactionId " + transactionForFiscalization.Trx_id));
            }
            POSMachineContainerDTO posMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), executionContext.POSMachineName, "", -1);
            if (posMachineContainerDTO == null || string.IsNullOrWhiteSpace(posMachineContainerDTO.Attribute1))
            {
                log.Debug("FTPOSSystemId is not set for this POS. Please check the configuration");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "FTPOSSystemId is not set for this POS. Please check the configuration")); // message needed
            }
            FiscalizationRequest receipt = new FiscalizationRequest();
            double downPayment = 0;
            if (transactionForFiscalization.OriginalTrxId > 0)
            {
                //handle scenario of full reversal where there is a seperate transaction header.
                receipt.isReversal = true;
                receipt.originalTransactionId = transactionForFiscalization.OriginalTrxId;
                log.Debug("Transaction is a full reversal");
            }

            receipt.transactionId = transactionForFiscalization.Trx_id;
            receipt.receiptMoment = transactionForFiscalization.TrxDate;
            receipt.extReference = transactionForFiscalization.externalSystemReference;
            receipt.posId = transactionForFiscalization.POSMachineId.ToString();
            receipt.ftPOSSystemId = posMachineContainerDTO.Attribute1;

            if (transactionForFiscalization.customerDTO != null && transactionForFiscalization.customerDTO.Email != null)
            {
                receipt.customerEmail = transactionForFiscalization.customerDTO.Email;
            }

            // add the receipt reference for multi transaction scenario (typically reservation)
            Boolean reservationCheck = transactionForFiscalization.IsReservationTransaction(null);
            if (reservationCheck)
            {
                if (transactionForFiscalization.Net_Transaction_Amount != transactionForFiscalization.TotalPaidAmount && transactionForFiscalization.TotalPaidAmount > 0)
                {
                    receipt.isReservation = true;
                    downPayment = transactionForFiscalization.TotalPaidAmount;
                }
            }
            else
            {
                receipt.isReservation = false;
            }

            //set closed status
            if (transactionForFiscalization.Status == Transaction.TrxStatus.CLOSED || transactionForFiscalization.Status == Transaction.TrxStatus.CANCELLED)
            {
                receipt.isClosed = true;
            }

            List<Transaction.TransactionLine> lineDTOList = transactionForFiscalization.TrxLines;
            List<TransactionLine> transactionLineList = new List<TransactionLine>();
            if (lineDTOList != null)
            {
                log.Debug("Count of lines " + lineDTOList.Count);
                // Add the trxlines as chargeItem line for advance amount - Do not add the trxlines for fiscalization
                if (receipt.isReservation && downPayment > 0 && !receipt.isClosed)
                {
                    decimal vatRate = 0;
                    string productDescription = string.Empty;
                    Transaction.TransactionLine lineDTO = lineDTOList.Where(x => x.ProductTypeCode == ProductTypeValues.BOOKINGS ||
                                                                             x.ProductType == ProductTypeValues.ATTRACTION).FirstOrDefault();
                    if (lineDTO != null)
                    {
                        vatRate = Convert.ToDecimal(lineDTO.tax_percentage);
                        productDescription = lineDTO.ProductName;
                    }
                    TransactionLine transactionLine = new TransactionLine();
                    transactionLine.position = 0;
                    transactionLine.amount = Convert.ToDecimal(downPayment);
                    transactionLine.quantity = 1;
                    transactionLine.description = MessageContainerList.GetMessage(executionContext, productDescription + "- Down Payment");
                    transactionLine.VATRate = vatRate;
                    transactionLineList.Add(transactionLine);
                }
                else
                {
                    decimal discountAmount = 0;
                    foreach (Transaction.TransactionLine lineDTO in lineDTOList)
                    {
                        TransactionLine transactionLine = new TransactionLine();
                        if (lineDTO.TransactionDiscountsDTOList != null &&
                                          lineDTO.TransactionDiscountsDTOList.Any())
                        {
                            foreach (TransactionDiscountsDTO transactionDiscountsDTO in lineDTO.TransactionDiscountsDTOList)
                            {
                                discountAmount += transactionDiscountsDTO.DiscountAmount.HasValue ? Convert.ToDecimal(transactionDiscountsDTO.DiscountAmount) : 0;
                            }
                        }
                        transactionLine.position = lineDTO.TransactionLineDTO.LineId;
                        transactionLine.description = lineDTO.TransactionLineDTO.ProductName;
                        decimal trxLineAmount = Convert.ToDecimal(lineDTO.LineAmount) - discountAmount;
                        transactionLine.amount = Math.Round(trxLineAmount / RoundOffAmountTo, 2, MidpointRounding.AwayFromZero) * RoundOffAmountTo;
                        transactionLine.quantity = lineDTO.TransactionLineDTO.Quantity ?? 0;
                        if (lineDTO.TransactionLineDTO.ProductsDTO != null)
                        {
                            transactionLine.description = lineDTO.TransactionLineDTO.ProductsDTO.Description ?? "No Description";
                        }
                        transactionLine.VATRate = Convert.ToDecimal(lineDTO.tax_percentage);
                        transactionLine.VATAmount = Math.Round(Convert.ToDecimal(lineDTO.tax_amount) / RoundOffAmountTo, 2, MidpointRounding.AwayFromZero) * RoundOffAmountTo;
                        transactionLineList.Add(transactionLine);
                    }

                }
            }
            List<PaymentInfo> payItemList = new List<PaymentInfo>();
            // Get the trx payment details for the advance payments made. The reference will be the fiscalized signature
            // If the reference is not null then it is been sent to fiscalize 
            // Assuming that previous trx payments are the advance payments
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionForFiscalization.TransactionPaymentsDTOList.Where(x => x.ExternalSourceReference != "").ToList();
            if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Any())
            {
                if (reservationCheck)
                {
                    decimal vatRate = 0;
                    string productDescription = string.Empty;
                    Transaction.TransactionLine lineDTO = lineDTOList.Where(x => x.ProductTypeCode == ProductTypeValues.BOOKINGS ||
                                                                             x.ProductType == ProductTypeValues.ATTRACTION).FirstOrDefault();
                    if (lineDTO != null)
                    {
                        vatRate = Convert.ToDecimal(lineDTO.tax_percentage);
                        productDescription = lineDTO.ProductName;
                    }
                    double totalAmountPaid = transactionPaymentsDTOList.Select(x => x.Amount).Sum();
                    TransactionLine transactionLine = new TransactionLine();
                    transactionLine.position = 0;
                    transactionLine.amount = Math.Round(Convert.ToDecimal(totalAmountPaid * -1) / RoundOffAmountTo, 2, MidpointRounding.AwayFromZero) * RoundOffAmountTo;
                    transactionLine.quantity = 1;
                    transactionLine.description = MessageContainerList.GetMessage(executionContext, productDescription + "- Settlement");
                    transactionLine.VATRate = vatRate;
                    transactionLineList.Add(transactionLine);
                }
            }

            receipt.transactionLines = transactionLineList.ToArray();
            List<TransactionPaymentsDTO> paymentList = transactionForFiscalization.TransactionPaymentsDTOList;

            if (paymentList != null)
            {

                log.Debug("Count of payment " + paymentList.Count);
                foreach (TransactionPaymentsDTO paymentDTO in paymentList)
                {
                    log.Debug("\t Amount :" + paymentDTO.Amount);
                    log.Debug("\t Mode :" + paymentDTO.paymentModeDTO.PaymentMode);
                    log.Debug("\t\t Reference value (signature) :" + paymentDTO.ExternalSourceReference);

                    // below check ensures the fiscalized receipts are not published again
                    string paymentReference = paymentDTO.ExternalSourceReference;
                    if (string.IsNullOrWhiteSpace(paymentReference))
                    {
                        PaymentInfo paymentInfo = new PaymentInfo();
                        paymentInfo.quantity = 1;
                        paymentInfo.amount = Math.Round(Convert.ToDecimal(paymentDTO.Amount) / RoundOffAmountTo, 2, MidpointRounding.AwayFromZero) * RoundOffAmountTo;  // In case of reservation :  amount = amount - advance amount
                        if (paymentDTO.paymentModeDTO.IsCreditCard)
                        {
                            paymentInfo.paymentMode = "CREDITCARD";
                        }
                        else if (paymentDTO.paymentModeDTO.IsDebitCard)
                        {
                            paymentInfo.paymentMode = "DEBITCARD";
                        }
                        else if (paymentDTO.paymentModeDTO.IsCash)
                        {
                            paymentInfo.paymentMode = "CASH";
                        }
                        else
                        {
                            paymentInfo.paymentMode = paymentDTO.paymentModeDTO.PaymentMode;
                        }
                        paymentInfo.reference = paymentDTO.Reference;
                        paymentInfo.paymentID = paymentDTO.PaymentId;
                        payItemList.Add(paymentInfo);
                    }

                }
            }
            receipt.payments = payItemList.ToArray();
            log.LogMethodExit(receipt);
            return receipt;
        }

        private bool ValidateTransactionPayments()
        {
            log.LogMethodEntry();
            bool shouldFiscalize = false;
            List<TransactionPaymentsDTO> paymentList = transactionForFiscalization.TransactionPaymentsDTOList;
            if (paymentList != null && paymentList.Any())
            {
                foreach (TransactionPaymentsDTO paymentDTO in paymentList)
                {
                    log.Debug("\t Amount :" + paymentDTO.PaymentId);
                    log.Debug("\t Mode :" + paymentDTO.paymentModeDTO.PaymentMode);
                    log.Debug("\t\t Reference value (signature) :" + paymentDTO.ExternalSourceReference);
                    string paymentReference = paymentDTO.ExternalSourceReference;
                    string existingReference = paymentDTO.ExternalSourceReference;
                    TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, paymentDTO.PaymentId);
                    if (transactionPaymentsBL.TransactionPaymentsDTO != null)
                    {
                        existingReference = transactionPaymentsBL.TransactionPaymentsDTO.ExternalSourceReference;
                    }
                    if (string.IsNullOrWhiteSpace(paymentReference))
                    {
                        shouldFiscalize = true;
                    }
                    if (string.IsNullOrWhiteSpace(existingReference) == false && existingReference != paymentReference)
                    {
                        shouldFiscalize = true;
                    }
                }
            }
            log.LogMethodExit(shouldFiscalize);
            return shouldFiscalize;
        }

        /// <summary>
        /// BuildNonTransactionalFiscalRequest - This method builds the fiscal requests for Shift in/out and PayIn PayOut
        /// </summary>
        /// <param name="shiftLogDTO"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public FiscalizationRequest BuildShiftFiscalizationRequest(ShiftLogDTO shiftLogDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(shiftLogDTO);
            try
            {
                ShiftBL shiftBL = new ShiftBL(executionContext, shiftLogDTO.ShiftKey, true, false, sqlTransaction);
                if (shiftBL.ShiftDTO == null)
                {
                    log.Debug("No data found in the shift for the shiftId = " + shiftLogDTO.ShiftKey);
                    return null;
                }
                ShiftDTO shiftDTO = shiftBL.ShiftDTO;
                FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
                fiscalizationRequest.shiftLogId = shiftLogDTO.ShiftLogId;  // used for updating the payment signature in the externalReference column
                fiscalizationRequest.shiftId = shiftLogDTO.ShiftKey;          // used for updating the payment signature in the externalReference column
                fiscalizationRequest.receiptMoment = shiftBL.ShiftDTO.ShiftTime;
                fiscalizationRequest.posId = executionContext.GetMachineId().ToString();
                fiscalizationRequest.extReference = "pos-action-shift-" + shiftDTO.ShiftTime;
                POSMachineContainerDTO posMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(executionContext.GetSiteId(), executionContext.POSMachineName, "", -1);
                if (posMachineContainerDTO == null || string.IsNullOrWhiteSpace(posMachineContainerDTO.Attribute1))
                {
                    log.Debug("FTPOSSystemId is not set for this POS. Please check the configuration");
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "FTPOSSystemId is not set for this POS. Please check the configuration")); // message needed
                }   //Create the payment information . By default Cash
                fiscalizationRequest.ftPOSSystemId = posMachineContainerDTO.Attribute1;

                PaymentInfo paymentInfo = new PaymentInfo();
                paymentInfo.quantity = 1;
                paymentInfo.description = "Cash";
                paymentInfo.moment = shiftDTO.ShiftTime;
                paymentInfo.paymentMode = "CASH";

                // Create charge Item information through transaction
                TransactionLine transactionLine = new TransactionLine();
                transactionLine.position = 1;
                transactionLine.quantity = 1;
                transactionLine.VATRate = 0;
                transactionLine.VATAmount = 0;
                if (shiftLogDTO.ShiftAction == "Open")
                {
                    transactionLine.amount = Convert.ToDecimal(shiftDTO.ShiftAmount);
                    paymentInfo.amount = Convert.ToDecimal(transactionLine.amount);
                    transactionLine.description = "Cash transfer to till";
                    transactionLine.VATAmount = null;
                    fiscalizationRequest.isShiftIn = true;
                }
                else if (shiftLogDTO.ShiftAction == "Close")
                {
                    transactionLine.amount = -Convert.ToDecimal(shiftDTO.ShiftAmount);
                    paymentInfo.amount = Convert.ToDecimal(transactionLine.amount);
                    transactionLine.description = "Cash transfer from till";
                    fiscalizationRequest.isShiftOut = true;
                }

                else if (shiftLogDTO.ShiftAction == "Paid In")
                {
                    transactionLine.amount = Convert.ToDecimal(shiftLogDTO.ShiftAmount);
                    paymentInfo.amount = Convert.ToDecimal(shiftLogDTO.ShiftAmount);
                    transactionLine.description = "Cash transfer to till";
                    transactionLine.VATAmount = null;
                    fiscalizationRequest.isPayIn = true;
                }
                else if (shiftLogDTO.ShiftAction == "Paid Out")
                {
                    transactionLine.amount = Convert.ToDecimal(shiftLogDTO.ShiftAmount * -1);
                    paymentInfo.amount = -Convert.ToDecimal(shiftLogDTO.ShiftAmount);
                    transactionLine.description = "Cash transfer from till";
                    fiscalizationRequest.isPayOut = true;
                    transactionLine.VATAmount = null;
                }
                fiscalizationRequest.payments = new PaymentInfo[] { paymentInfo };
                //if(shiftLogDTO.ShiftAction == "Close")
                //{
                //    fiscalizationRequest.payments = new PaymentInfo[] {  };
                //}
                fiscalizationRequest.transactionLines = new TransactionLine[] { transactionLine };
                log.LogMethodExit(fiscalizationRequest);
                return fiscalizationRequest;
            }

            catch (Exception ex)
            {
                log.Debug(ex);
                return null;
            }
        }

        /// <summary>
        /// UpdatePaymentReference -  Updates the signature in the trxPayments table
        /// </summary>
        /// <param name="fiscalizationRequest"></param>
        /// <param name="signature"></param>
        public void UpdatePaymentReference(FiscalizationRequest fiscalizationRequest, String signature, SqlTransaction transaction = null)
        {
            log.LogMethodEntry(fiscalizationRequest, signature, transaction);
            log.Debug("Signature : " + signature);

            PaymentInfo[] payments = fiscalizationRequest.payments;

            if (payments != null)
            {
                log.Debug("Count of payment " + payments.Length);
                foreach (PaymentInfo paymentInfo in payments)
                {
                    log.Debug("\t Payment ID :" + paymentInfo.paymentID);
                    log.Debug("\t Start update of reference ...");
                    TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, paymentInfo.paymentID);
                    transactionPaymentsBL.TransactionPaymentsDTO.ExternalSourceReference = signature;
                    transactionPaymentsBL.TransactionPaymentsDTO.IsChanged = true;
                    transactionPaymentsBL.Save(transaction);
                    log.Debug("\t Reference updated sucessfully ... ");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// UpdateShiftPaymentReference - updates the signature in the 
        /// </summary>
        /// <param name="fiscalizationRequest"></param>
        /// <param name="signature"></param>
        public void UpdateShiftPaymentReference(FiscalizationRequest fiscalizationRequest, string signature, SqlTransaction transaction = null)
        {
            log.LogMethodEntry(fiscalizationRequest, signature, transaction);
            log.Debug("Signature : " + signature);
            log.Debug("\t Shift ID :" + fiscalizationRequest.shiftId);
            log.Debug("\t Shift log ID :" + fiscalizationRequest.shiftLogId);
            log.Debug("\t Start update of reference ...");
            ShiftBL shiftBL = new ShiftBL(executionContext, fiscalizationRequest.shiftId, true, false, transaction);
            var shiftLogDTO = shiftBL.ShiftDTO.ShiftLogDTOList.Where(x => x.ShiftLogId == fiscalizationRequest.shiftLogId).FirstOrDefault();
            if (shiftLogDTO != null)
            {
                shiftLogDTO.ExternalReference = signature;
            }
            shiftBL.ShiftDTO.IsChanged = true;
            shiftBL.Save(transaction);
            log.Debug("\t Reference updated sucessfully ... ");
            log.LogMethodExit();
        }
    }


}
