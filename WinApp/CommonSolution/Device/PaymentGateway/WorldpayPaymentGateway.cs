using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class WorldpayPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string deviceUrl;
        private int devicePortNumber;
        private int receiptPortNumber;
        private bool isAuthEnabled;
        private bool isDeviceBeepSoundRequired;
        private bool isAddressValidationRequired;
        private bool isCustomerAllowedToDecideEntryMode;
        private bool isSignatureRequired;
        private int receiptSocketTimeout;
        public WorldpayPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            string value;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
            deviceUrl = utilities.getParafaitDefaults("CREDIT_CARD_DEVICE_URL");
            value = utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO");
            devicePortNumber = (string.IsNullOrEmpty(value)) ? -1 : Convert.ToInt32(value);
            value = utilities.getParafaitDefaults("CREDIT_CARD_RECEIPT_PORT");
            receiptPortNumber = (string.IsNullOrEmpty(value)) ? -1 : Convert.ToInt32(value);
            isAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            isDeviceBeepSoundRequired = utilities.getParafaitDefaults("ENABLE_CREDIT_CARD_DEVICE_BEEP_SOUND").Equals("Y");
            isAddressValidationRequired = utilities.getParafaitDefaults("ENABLE_ADDRESS_VALIDATION").Equals("Y");
            isCustomerAllowedToDecideEntryMode = utilities.getParafaitDefaults("ALLOW_CUSTOMER_TO_DECIDE_ENTRY_MODE").Equals("Y");
            isSignatureRequired = !utilities.getParafaitDefaults("ENABLE_SIGNATURE_VERIFICATION").Equals("N");
            value = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "FIRST_DATA_CLIENT_TIMEOUT");
            receiptSocketTimeout = (string.IsNullOrEmpty(value)) ? -1 : ((Convert.ToInt32(value) < 60) ? -1 : Convert.ToInt32(value));
            receiptSocketTimeout = (receiptSocketTimeout > 0) ? receiptSocketTimeout * 1000 : -1;
            isSignatureRequired = (isSignatureRequired) ? !isUnattended : false;
            log.LogVariableState("receiptPortNumber", receiptPortNumber);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("deviceUrl", deviceUrl);
            log.LogVariableState("devicePortNumber", devicePortNumber);
            log.LogVariableState("authorization", isAuthEnabled);
            if (devicePortNumber <= 0)
            {
                log.Error("Please enter CREDIT_CARD_TERMINAL_PORT_NO value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_TERMINAL_PORT_NO value in configuration."));
            }
            if (receiptPortNumber <= 0)
            {
                log.Error("Please enter CREDIT_CARD_RECEIPT_PORT value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_RECEIPT_PORT value in configuration."));
            }
            if (string.IsNullOrEmpty(deviceUrl))
            {
                log.Error("Please enter CREDIT_CARD_DEVICE_URL value in configuration.");
                throw new Exception(utilities.MessageUtils.getMessage("Please enter CREDIT_CARD_DEVICE_URL value in configuration."));
            }
            log.LogMethodExit(null);
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            worldpayRequest.TransactionType = PaymentGatewayTransactionType.SALE;
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                worldpayIPP350Handler = new WorldpayIPP350Handler(deviceUrl, devicePortNumber, receiptPortNumber, isUnattended, receiptSocketTimeout);
                worldpayIPP350Handler.PrintCCReceipt = Print;
                if (!isUnattended)
                {
                    if (isAuthEnabled)
                    {
                        frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate);
                        if (frmTranType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            if (frmTranType.TransactionType.Equals("Authorization") || frmTranType.TransactionType.Equals("Sale"))
                            {
                                if (frmTranType.TransactionType.Equals("Authorization"))
                                {
                                    worldpayRequest.TransactionType = PaymentGatewayTransactionType.AUTHORIZATION;
                                }
                                else
                                {
                                    worldpayRequest.TransactionType = PaymentGatewayTransactionType.SALE;
                                }
                            }
                        }
                        else
                        {
                            throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                        }
                    }
                }
                worldpayRequest.EntryMode = "SWIPED";
                if (isCustomerAllowedToDecideEntryMode)
                {
                    frmEntryMode entryMode = new frmEntryMode();
                    utilities.setLanguage(entryMode);
                    if (entryMode.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (entryMode.IsManual)
                        {
                            worldpayRequest.EntryMode = "KEYED";
                        }
                    }
                    else
                    {
                        throw new Exception(utilities.MessageUtils.getMessage("Operation cancelled."));
                    }
                }
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                worldpayRequest.ReferenceNumber = ccRequestPGWDTO.RequestID.ToString();
                worldpayRequest.Amount = transactionPaymentsDTO.Amount;
                worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                log.LogVariableState("worldpayResponse", worldpayResponse);
                if (worldpayResponse == null)
                {
                    log.LogMethodExit("Invalid response.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                }
                else
                {
                    if (worldpayResponse.ccTransactionsPGWDTO != null)
                    {
                        worldpayResponse.ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(worldpayResponse.ccTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("1")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("2")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("3")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("10")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("20"))
                        {
                            transactionPaymentsDTO.CreditCardNumber = worldpayResponse.ccTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.CreditCardName = worldpayResponse.ccTransactionsPGWDTO.UserTraceData;
                            transactionPaymentsDTO.CCResponseId = worldpayResponse.ccTransactionsPGWDTO.ResponseID;
                            if (Convert.ToInt32(worldpayResponse.ccTransactionsPGWDTO.TipAmount) > 0)
                            {
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.TipAmount) / 100;
                            }
                            transactionPaymentsDTO.Amount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.Authorize) / 100;
                            if (!string.IsNullOrEmpty(worldpayResponse.CardHolderReceipt))
                            {
                                transactionPaymentsDTO.Memo = worldpayResponse.CardHolderReceipt;
                            }
                            if (!string.IsNullOrEmpty(worldpayResponse.MerchantReceipt))
                            {
                                transactionPaymentsDTO.Memo += worldpayResponse.MerchantReceipt;
                            }
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            return transactionPaymentsDTO;
                        }
                        else
                        {
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            throw new Exception(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                        }
                    }
                    else
                    {
                        log.LogMethodExit("worldpayResponse.ccTransactionsPGWDTO is null");
                        throw new Exception("Invalid response");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString(), ex);
                throw ex;
            }
        }
        private CCTransactionsPGWDTO GetPreAuthorizationCCTransactionsPGWDTO(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            CCTransactionsPGWDTO preAuthorizationCCTransactionsPGWDTO = null;
            if (utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
            {
                CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRANSACTION_ID, transactionPaymentsDTO.TransactionId.ToString()));
                if (transactionPaymentsDTO.SplitId != -1)
                {
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SPLIT_ID, transactionPaymentsDTO.SplitId.ToString()));
                }
                searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.TATokenRequest.ToString()));
                List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                {
                    preAuthorizationCCTransactionsPGWDTO = cCTransactionsPGWDTOList[0];
                }
            }
            log.LogMethodExit(preAuthorizationCCTransactionsPGWDTO);
            return preAuthorizationCCTransactionsPGWDTO;
        }
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            worldpayRequest.TransactionType = PaymentGatewayTransactionType.REFUND;
            try
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                worldpayIPP350Handler = new WorldpayIPP350Handler(deviceUrl, devicePortNumber, receiptPortNumber, isUnattended, receiptSocketTimeout);
                worldpayIPP350Handler.PrintCCReceipt = Print;
                worldpayRequest.OrgTransactionReference = ccOrigTransactionsPGWDTO.RefNo;
                worldpayRequest.MaskedCardNumber = ccOrigTransactionsPGWDTO.AcctNo;
                worldpayRequest.ExpiryDate = ccOrigTransactionsPGWDTO.ResponseOrigin;
                worldpayRequest.EntryMode = ccOrigTransactionsPGWDTO.CaptureStatus;
                worldpayRequest.ReferenceNumber = ccRequestPGWDTO.RequestID.ToString();
                worldpayRequest.TokenId = ccOrigTransactionsPGWDTO.TokenID;
                if ((transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount) == (Convert.ToDouble(ccOrigTransactionsPGWDTO.Authorize) / 100))
                {
                    worldpayRequest.Amount = transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount;
                }
                else
                {
                    worldpayRequest.Amount = transactionPaymentsDTO.Amount;
                }
                if (ccOrigTransactionsPGWDTO.TranCode.Equals(PaymentGatewayTransactionType.AUTHORIZATION.ToString()))
                {
                    throw new Exception(utilities.MessageUtils.getMessage(1665));
                }
                worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                log.LogVariableState("worldpayResponse", worldpayResponse);
                if (worldpayResponse == null)
                {
                    log.LogMethodExit("Invalid response.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                }
                else
                {
                    if (worldpayResponse.ccTransactionsPGWDTO != null)
                    {
                        if (string.IsNullOrEmpty(worldpayResponse.ccTransactionsPGWDTO.AcctNo))
                        {
                            worldpayResponse.ccTransactionsPGWDTO.AcctNo = ccOrigTransactionsPGWDTO.AcctNo;
                        }
                        worldpayResponse.ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                        worldpayResponse.ccTransactionsPGWDTO.ProcessData = ccOrigTransactionsPGWDTO.ProcessData;
                        worldpayResponse.ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(worldpayResponse.ccTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("1")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("2")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("3")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("10")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("20"))
                        {
                            transactionPaymentsDTO.CreditCardNumber = worldpayResponse.ccTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.CreditCardName = worldpayResponse.ccTransactionsPGWDTO.UserTraceData;
                            transactionPaymentsDTO.CCResponseId = worldpayResponse.ccTransactionsPGWDTO.ResponseID;
                            if (Convert.ToInt32(worldpayResponse.ccTransactionsPGWDTO.TipAmount) > 0)
                            {
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.TipAmount) / 100;
                            }
                            transactionPaymentsDTO.Amount = (Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.Authorize) / 100) - transactionPaymentsDTO.TipAmount;
                            if (!string.IsNullOrEmpty(worldpayResponse.CardHolderReceipt))
                            {
                                transactionPaymentsDTO.Memo = worldpayResponse.CardHolderReceipt;
                            }
                            if (!string.IsNullOrEmpty(worldpayResponse.MerchantReceipt))
                            {
                                transactionPaymentsDTO.Memo += worldpayResponse.MerchantReceipt;
                            }
                            if (!string.IsNullOrEmpty(worldpayResponse.ccTransactionsPGWDTO.RefNo))
                            {
                                transactionPaymentsDTO.Reference = worldpayResponse.ccTransactionsPGWDTO.RefNo;
                            }
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            return transactionPaymentsDTO;
                        }
                        else
                        {
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            throw new Exception(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                        }
                    }
                    else
                    {
                        log.LogMethodExit("worldpayResponse.ccTransactionsPGWDTO is null");
                        throw new Exception("Invalid response");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Refunding the Amount", ex);
                log.Fatal("Ends -RefundAmount(transactionPaymentsDTO) method " + ex.ToString());
                log.LogMethodExit(null, "throwing Exception");
                throw ex;
            }
        }
        /// <summary>
        /// Returns boolean based on whether payment requires a settlement to be done.
        /// </summary>
        /// <param name = "transactionPaymentsDTO" ></ param >
        /// < returns ></ returns >
        public override bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            bool returnValue = false;
            if (isAuthEnabled)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.AUTHORIZATION.ToString()));
                    List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
                    if (cCTransactionsPGWDTOList != null && cCTransactionsPGWDTOList.Count > 0)
                    {
                        returnValue = true;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "transactionPaymentsDTO" ></ param >
        /// < param name="IsForcedSettlement"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            worldpayRequest.TransactionType = PaymentGatewayTransactionType.CAPTURE;
            try
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                worldpayIPP350Handler = new WorldpayIPP350Handler(deviceUrl, devicePortNumber, receiptPortNumber, isUnattended, receiptSocketTimeout);
                worldpayIPP350Handler.PrintCCReceipt = Print;
                worldpayRequest.ReferenceNumber = ccRequestPGWDTO.RequestID.ToString();
                worldpayRequest.MaskedCardNumber = ccOrigTransactionsPGWDTO.AcctNo;
                worldpayRequest.OrgTransactionReference = ccOrigTransactionsPGWDTO.RefNo;
                worldpayRequest.ExpiryDate = ccOrigTransactionsPGWDTO.ResponseOrigin;
                worldpayRequest.Amount = transactionPaymentsDTO.Amount;
                worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                log.LogVariableState("worldpayResponse", worldpayResponse);
                if (worldpayResponse == null)
                {
                    log.LogMethodExit("Invalid response.");
                    throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                }
                else
                {
                    if (worldpayResponse.ccTransactionsPGWDTO != null)
                    {
                        worldpayResponse.ccTransactionsPGWDTO.TokenID = ccOrigTransactionsPGWDTO.TokenID;
                        worldpayResponse.ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                        worldpayResponse.ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                        worldpayResponse.ccTransactionsPGWDTO.ProcessData = ccOrigTransactionsPGWDTO.ProcessData;
                        CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(worldpayResponse.ccTransactionsPGWDTO);
                        cCTransactionsPGWBL.Save();
                        if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("1")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("2")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("3")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("10")
                            || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("20"))
                        {
                            transactionPaymentsDTO.CreditCardNumber = worldpayResponse.ccTransactionsPGWDTO.AcctNo;
                            transactionPaymentsDTO.CreditCardName = worldpayResponse.ccTransactionsPGWDTO.UserTraceData;
                            transactionPaymentsDTO.CCResponseId = worldpayResponse.ccTransactionsPGWDTO.ResponseID;
                            if (Convert.ToInt32(worldpayResponse.ccTransactionsPGWDTO.TipAmount) > 0)
                            {
                                transactionPaymentsDTO.TipAmount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.TipAmount) / 100;
                            }
                            transactionPaymentsDTO.Amount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.Authorize) / 100;
                            if (!string.IsNullOrEmpty(worldpayResponse.CardHolderReceipt))
                            {
                                transactionPaymentsDTO.Memo = worldpayResponse.CardHolderReceipt;
                            }
                            if (!string.IsNullOrEmpty(worldpayResponse.MerchantReceipt))
                            {
                                transactionPaymentsDTO.Memo += worldpayResponse.MerchantReceipt;
                            }
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            return transactionPaymentsDTO;
                        }
                        else
                        {
                            log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                            throw new Exception(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                        }
                    }
                    else
                    {
                        log.LogMethodExit("worldpayResponse.ccTransactionsPGWDTO is null");
                        throw new Exception("Invalid response");
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }
        /// <summary>
        /// Returns list of CCTransactionsPGWDTO's  pending for settelement. 
        /// </summary>
        /// <returns></returns>
        public override List<CCTransactionsPGWDTO> GetAllUnsettledCreditCardTransactions()
        {
            log.LogMethodEntry();
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = null;
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, PaymentGatewayTransactionType.AUTHORIZATION.ToString()));
            cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetNonReversedCCTransactionsPGWDTOList(searchParameters);
            log.LogMethodExit(cCTransactionsPGWDTOList);
            return cCTransactionsPGWDTOList;
        }

        /// <summary>
        /// Settle Transaction Payment
        /// </summary>
        public override TransactionPaymentsDTO SettleTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            try
            {
                if (transactionPaymentsDTO != null)
                {
                    WorldpayRequest worldpayRequest = new WorldpayRequest();
                    WorldpayResponse worldpayResponse;
                    WorldpayIPP350Handler worldpayIPP350Handler;
                    worldpayRequest.TransactionType = PaymentGatewayTransactionType.CAPTURE;
                    try
                    {
                        CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                        CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                        CCTransactionsPGWDTO ccOrigTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                        worldpayIPP350Handler = new WorldpayIPP350Handler(deviceUrl, devicePortNumber, receiptPortNumber, isUnattended, receiptSocketTimeout);
                        worldpayIPP350Handler.PrintCCReceipt = Print;
                        worldpayRequest.ReferenceNumber = ccRequestPGWDTO.RequestID.ToString();
                        worldpayRequest.MaskedCardNumber = ccOrigTransactionsPGWDTO.AcctNo;
                        worldpayRequest.OrgTransactionReference = ccOrigTransactionsPGWDTO.RefNo;
                        worldpayRequest.ExpiryDate = ccOrigTransactionsPGWDTO.ResponseOrigin;
                        worldpayRequest.Amount = transactionPaymentsDTO.Amount;
                        worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                        log.LogVariableState("worldpayResponse", worldpayResponse);
                        if (worldpayResponse == null)
                        {
                            log.LogMethodExit("Invalid response.");
                            throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                        }
                        else
                        {
                            if (worldpayResponse.ccTransactionsPGWDTO != null)
                            {
                                worldpayResponse.ccTransactionsPGWDTO.TokenID = ccOrigTransactionsPGWDTO.TokenID;
                                worldpayResponse.ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                                worldpayResponse.ccTransactionsPGWDTO.UserTraceData = ccOrigTransactionsPGWDTO.UserTraceData;
                                worldpayResponse.ccTransactionsPGWDTO.ProcessData = ccOrigTransactionsPGWDTO.ProcessData;
                                CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(worldpayResponse.ccTransactionsPGWDTO);
                                cCTransactionsPGWBL.Save();
                                if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("1")
                                    || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("2")
                                    || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("3")
                                    || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("10")
                                    || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("20"))
                                {
                                    transactionPaymentsDTO.CreditCardNumber = worldpayResponse.ccTransactionsPGWDTO.AcctNo;
                                    transactionPaymentsDTO.CreditCardName = worldpayResponse.ccTransactionsPGWDTO.UserTraceData;
                                    transactionPaymentsDTO.CCResponseId = worldpayResponse.ccTransactionsPGWDTO.ResponseID;
                                    if (Convert.ToInt32(worldpayResponse.ccTransactionsPGWDTO.TipAmount) > 0)
                                    {
                                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.TipAmount) / 100;
                                    }
                                    transactionPaymentsDTO.Amount = Convert.ToDouble(worldpayResponse.ccTransactionsPGWDTO.Authorize) / 100;
                                    if (!string.IsNullOrEmpty(worldpayResponse.CardHolderReceipt))
                                    {
                                        transactionPaymentsDTO.Memo = worldpayResponse.CardHolderReceipt;
                                    }
                                    if (!string.IsNullOrEmpty(worldpayResponse.MerchantReceipt))
                                    {
                                        transactionPaymentsDTO.Memo += worldpayResponse.MerchantReceipt;
                                    }
                                    log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                                    return transactionPaymentsDTO;
                                }
                                else
                                {
                                    log.LogMethodExit(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                                    throw new Exception(worldpayResponse.ccTransactionsPGWDTO.TextResponse);
                                }
                            }
                            else
                            {
                                log.LogMethodExit("worldpayResponse.ccTransactionsPGWDTO is null");
                                throw new Exception("Invalid response");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while performing settlement", ex);
                        log.LogMethodExit(null, "Throwing Exception " + ex);
                        throw (ex);
                    }
                }
                else
                {
                    log.LogMethodExit("Transaction payment info is missing");
                    throw new Exception("Transaction payment info is missing");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }

        public override void CanAdjustTransactionPayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            if (transactionPaymentsDTO != null)
            {
                string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
                long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
                if (tipLimit > 0 && ((transactionPaymentsDTO.Amount * tipLimit) / 100) < transactionPaymentsDTO.TipAmount)
                {
                    throw new Exception(utilities.MessageUtils.getMessage("Tip limit exceeded"));
                }
            }
            else
            {
                throw new Exception("Transaction payment info is missing");
            }
            log.LogMethodExit();
        }
    }
}
