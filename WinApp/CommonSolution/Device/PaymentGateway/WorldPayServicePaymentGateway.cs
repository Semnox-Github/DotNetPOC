/********************************************************************************************
 * Project Name - CardConnectPaymentGateway
 * Description  - CardConnectPaymentGateway class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.130.8     06-Jun-2022      Guru S A       Worldpay service should be restarted by kiosk if it is unable to connect service socket
 *2.150.1     22-Feb-2023      Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using Semnox.Parafait.Languages;
using Semnox.Parafait.JobUtils; 

namespace Semnox.Parafait.Device.PaymentGateway
{
    class WorldPayServicePaymentGateway : PaymentGateway
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
        private int messgePortNo;
        private bool paymentProcess;
        IDisplayStatusUI statusDisplayUi;
        public delegate void DisplayWindow();
        Socket socketMessage;
        private const string WORLDPAYSERVICENAME = "IPCService";
        internal enum CommandTransactionType
        {
            SIGNATURE,
            REFERRAL,
            FALLBACK,
            CASHBACK,
            CNPCONFIRMATION,
            AVSCONFIRMATION
        }

        public WorldPayServicePaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
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
            messgePortNo = string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["WorldPayServiceMessagePortNo"]) ? 8000 : Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WorldPayServiceMessagePortNo"]);
            paymentProcess = false;
            log.LogVariableState("receiptPortNumber", receiptPortNumber);
            log.LogVariableState("isUnattended", isUnattended);
            log.LogVariableState("deviceUrl", deviceUrl);
            log.LogVariableState("devicePortNumber", devicePortNumber);
            log.LogVariableState("authorization", isAuthEnabled);
            log.LogVariableState("messgePortNo", messgePortNo);
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

        private void StatusDisplayUi_CancelClicked(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (statusDisplayUi != null)
            {
                statusDisplayUi.DisplayText("Cancelling...");
            }
            CancellProcess();
            log.LogMethodExit(null);
        }
        private void CancellProcess()
        {
            log.LogMethodEntry();
            try
            {
                statusDisplayUi.CloseStatusWindow();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //dsiEMVX.CancelRequest();
            log.LogMethodExit(null);
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry();

            //Form activeForm = GetActiveForm();
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            worldpayRequest.TransactionType = PaymentGatewayTransactionType.SALE;
            paymentProcess = true;
            using (statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "WorldPay Payment Gateway"))
            {
                statusDisplayUi.CancelClicked += StatusDisplayUi_CancelClicked;
                statusDisplayUi.EnableCancelButton(false); 
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                //Thread thr = new Thread();

                try
                {
                    StandaloneRefundNotAllowed(transactionPaymentsDTO);
                    worldpayIPP350Handler = new WorldpayIPP350Handler(deviceUrl, devicePortNumber, receiptPortNumber, isUnattended, receiptSocketTimeout);
                    worldpayIPP350Handler.PrintCCReceipt = Print;


                    if (!isUnattended)
                    {
                        if (isAuthEnabled)
                        {
                            using (frmTransactionTypeUI frmTranType = new frmTransactionTypeUI(utilities, "Authorization", transactionPaymentsDTO.Amount, showMessageDelegate))
                            {
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
                    }
                    worldpayRequest.EntryMode = "SWIPED";
                    //thr.Start();
                    //Form form = statusDisplayUi as Form;
                    //form.Show(activeForm);
                    Application.DoEvents();
                    //SetNativeEnabled(activeForm, false);
                    if (isCustomerAllowedToDecideEntryMode)
                    {
                        using (frmEntryMode entryMode = new frmEntryMode())
                        {
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
                    }


                    thr.Start();
                    statusDisplayUi.DisplayText(utilities.MessageUtils.getMessage(1008));
                    CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                    worldpayRequest.ReferenceNumber = ccRequestPGWDTO.RequestID.ToString();
                    worldpayRequest.Amount = transactionPaymentsDTO.Amount;
                    //DisplayStatus(deviceUrl, messgePortNo); 
                    new Task(() => { DisplayStatus(deviceUrl, messgePortNo, statusDisplayUi); }).Start();
                    Task<WorldpayResponse> task = new Task<WorldpayResponse>(() => { return worldpayIPP350Handler.ProcessTransaction(worldpayRequest); });
                    task.Start();
                    while (task.IsCompleted == false)
                    {
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    worldpayResponse = task.Result;
                    statusDisplayUi.DisplayText("Payment In Process");
                    paymentProcess = false;
                    log.LogVariableState("worldpayResponse", worldpayResponse);
                    if (worldpayResponse == null)
                    {
                        log.LogMethodExit("Invalid response.");
                        throw new Exception(utilities.MessageUtils.getMessage("Invalid response."));
                    }
                    else
                    {
                        log.Debug("valid response");
                        if (worldpayResponse.ccTransactionsPGWDTO != null)
                        {
                            worldpayResponse.ccTransactionsPGWDTO.InvoiceNo = ccRequestPGWDTO.RequestID.ToString();
                            CCTransactionsPGWBL cCTransactionsPGWBL = new CCTransactionsPGWBL(worldpayResponse.ccTransactionsPGWDTO);
                            cCTransactionsPGWBL.Save();
                            log.Debug("DSIXReturnCode: " + worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode);
                            if (worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("1")
                                || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("2")
                                || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("3")
                                || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("10")
                                || worldpayResponse.ccTransactionsPGWDTO.DSIXReturnCode.Equals("20"))
                            {
                                log.Debug("not cancelled");
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
                catch (AggregateException ex)
                {
                    log.Error(ex.ToString(), ex);
                    paymentProcess = false;
                    CloseSocket(socketMessage);
                    throw ex.InnerException;
                }
                //catch (Exception ex)
                //{
                //    log.Error(ex.ToString(), ex);
                //    paymentProcess = false;
                //    if (socketMessage != null && socketMessage.Connected)
                //    {
                //        log.Debug("Closing Message socket normally");
                //        socketMessage.Shutdown(SocketShutdown.Both);
                //        socketMessage.Close();
                //    }
                //    throw ex;
                //}

                finally
                {
                    //SetNativeEnabled(activeForm, true);
                    if (statusDisplayUi != null)
                        statusDisplayUi.CloseStatusWindow();
                }
            }
        }

        //public static Form GetActiveForm()
        //{
        //    // Returns null for an MDI app
        //    Form activeForm = Form.ActiveForm;
        //    if (activeForm == null)
        //    {
        //        FormCollection openForms = Application.OpenForms;
        //        for (int i = 0; i < openForms.Count && activeForm == null; ++i)
        //        {
        //            Form openForm = openForms[i];
        //            if (openForm.IsMdiContainer)
        //            {
        //                activeForm = openForm.ActiveMdiChild;
        //            }
        //        }
        //    }
        //    if (activeForm == null)
        //    {
        //        activeForm = Application.OpenForms[Application.OpenForms.Count - 1];
        //    }
        //    return activeForm;
        //}

        //const int GWL_STYLE = -16; const int WS_DISABLED = 0x08000000;
        //[DllImport("user32.dll")]
        //static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        //[DllImport("user32.dll")]
        //static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //void SetNativeEnabled(Form form, bool enabled) { SetWindowLong(form.Handle, GWL_STYLE, GetWindowLong(form.Handle, GWL_STYLE) & ~WS_DISABLED | (enabled ? 0 : WS_DISABLED)); }

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
            //Form activeForm = GetActiveForm();
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            paymentProcess = true;
            using (statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(4202, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "WorldPay Payment Gateway"))
            {
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
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
                    //Form form = statusDisplayUi as Form;
                    //form.Show(activeForm);
                    Application.DoEvents();
                    //SetNativeEnabled(activeForm, false);
                    //DisplayStatus(deviceUrl, messgePortNo); 
                    thr.Start();
                    new Task(() => { DisplayStatus(deviceUrl, messgePortNo, statusDisplayUi); }).Start();
                   
                    Task<WorldpayResponse> task = new Task<WorldpayResponse>(() => { return worldpayIPP350Handler.ProcessTransaction(worldpayRequest); });
                    task.Start();
                    while (task.IsCompleted == false)
                    {
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    worldpayResponse = task.Result;
                    //worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                    paymentProcess = false;
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
                    paymentProcess = false;
                    CloseSocket(socketMessage);
                    log.LogMethodExit(null, "throwing Exception");
                    throw ex;
                }
                finally
                {
                    //SetNativeEnabled(activeForm, true);
                    if (statusDisplayUi != null)
                        statusDisplayUi.CloseStatusWindow();
                }
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
            //Form activeForm = GetActiveForm();
            WorldpayRequest worldpayRequest = new WorldpayRequest();
            WorldpayResponse worldpayResponse;
            WorldpayIPP350Handler worldpayIPP350Handler;
            worldpayRequest.TransactionType = PaymentGatewayTransactionType.CAPTURE;
            paymentProcess = true;
            using (statusDisplayUi = DisplayUIFactory.GetStatusUI(utilities.ExecutionContext, isUnattended, utilities.MessageUtils.getMessage(1839, transactionPaymentsDTO.Amount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)), "WorldPay Payment Gateway"))
            {
                statusDisplayUi.EnableCancelButton(false);
                Thread thr = new Thread(statusDisplayUi.ShowStatusWindow);
                try
                {
                    thr.Start();
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
                    //DisplayStatus(deviceUrl, messgePortNo); 
                    new Task(() => { DisplayStatus(deviceUrl, messgePortNo, statusDisplayUi); }).Start();
                    Task<WorldpayResponse> task = new Task<WorldpayResponse>(() => { return worldpayIPP350Handler.ProcessTransaction(worldpayRequest); });
                    task.Start();
                    while (task.IsCompleted == false)
                    {
                        Thread.Sleep(500);
                        Application.DoEvents();
                    }
                    worldpayResponse = task.Result;
                    worldpayResponse = worldpayIPP350Handler.ProcessTransaction(worldpayRequest);
                    paymentProcess = false;
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
                    paymentProcess = false;
                    CloseSocket(socketMessage); 
                    log.LogMethodExit(null, "Throwing Exception " + ex);
                    throw (ex);
                }
                finally
                {
                    if (statusDisplayUi != null)
                        statusDisplayUi.CloseStatusWindow();
                }
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

        private void DisplayStatus(string deviceIpAddress, int portNumber, IDisplayStatusUI statusDisplayUi)
        {
            log.LogMethodEntry(deviceIpAddress, portNumber);
            try
            {
                IPEndPoint remoteEP;

                IPHostEntry host = Dns.GetHostEntry(deviceIpAddress);
                IPAddress ipAddress = host.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, portNumber);
                socketMessage = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketMessage.Connect(remoteEP);
                //socketMessage.ReceiveTimeout = 3000;// from existing setup
                try
                {
                    byte[] transactionStatus = new byte[1024];
                    int loop = 0;
                    int bytesRec;
                    while (paymentProcess)
                    {
                        loop++;
                        log.Debug("looping: " + loop);
                        Array.Clear(transactionStatus, 0, transactionStatus.Length);
                        bytesRec = socketMessage.Receive(transactionStatus);
                        string responseString;
                        if (bytesRec > 0)
                        {
                            responseString = Encoding.ASCII.GetString(transactionStatus, 0, bytesRec);
                            log.Debug("message responseString: " + responseString);
                            string message = responseString.Substring(0, responseString.IndexOf("99"));
                            log.Debug("message: " + message);
                            string[] responseArray = message.Split(':');
                            if (responseArray[1].Contains("ClearInstruction"))
                            {
                                log.Debug("contain ClearInstruction");
                                message = responseArray[1].Substring(0, responseArray[1].IndexOf("ClearInstruction"));
                            }
                            else if (responseArray[0].StartsWith("2=400"))
                            {
                                log.Debug("Requesting Referral. Respond with default as Reject");
                                ProcessRequest(socketMessage, CommandTransactionType.REFERRAL, message);
                            }
                            else if (responseArray[0].StartsWith("2=401"))
                            {
                                log.Debug("Requesting Signature Verification. Respond with default as Accept");
                                ProcessRequest(socketMessage, CommandTransactionType.SIGNATURE, message);
                            }
                            else if (responseArray[0].StartsWith("2=402"))
                            {
                                log.Debug("Requesting Fallback Verification. Respond with default as Accept");
                                ProcessRequest(socketMessage, CommandTransactionType.FALLBACK, message);
                            }
                            else if (responseArray[0].StartsWith("2=403"))
                            {
                                log.Debug("Requesting CNP Verification. Respond with default as Accept");
                                ProcessRequest(socketMessage, CommandTransactionType.CNPCONFIRMATION, message);
                            }
                            else if (responseArray[0].StartsWith("2=405"))
                            {
                                log.Debug("Requesting Cashback. Respond with default as Reject");
                                ProcessRequest(socketMessage, CommandTransactionType.CASHBACK, message);
                            }
                            else if (responseArray[0].StartsWith("2=407"))
                            {
                                log.Debug("Requesting AVS. Respond with default as Accept");
                                ProcessRequest(socketMessage, CommandTransactionType.AVSCONFIRMATION, message);
                            }
                            else
                            {
                                log.Debug("not contain ClearInstruction");
                                message = responseArray[1];
                            }
                            log.Debug("final message: " + message);
                            message = message.Replace("\r\n", " ");
                            message = message.Replace("\r", " ");
                            message = message.Replace("\n", " ");
                            message = Regex.Replace(message, "[ ]{2,}", " ");
                            message = TransalteMessage(message.Trim());
                            log.Debug("TransalteMessage: " + message);
                            if (statusDisplayUi != null)
                            {
                                log.Debug("statusDisplayUi: not null");
                                statusDisplayUi.DisplayText(message);
                            }
                            else
                            {
                                log.Debug("statusDisplayUi: null");
                            }

                        }
                        else
                        {
                            throw (new Exception("Invalid response."));
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception at DisplayStatus:", ex);
                }
                finally
                {
                    CloseSocket(socketMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private string TransalteMessage(string message)
        {
            log.LogMethodEntry(message);
            string response = string.Empty;
            switch (message)
            {
                case "insertCtlsCard":
                    response = "PLEASE INSERT CARD";
                    break;
                case "insertSwipeCard":
                    response = "PLEASE INSERT/SWIPE CARD";
                    break;
                case "insertSwipeCardctls":
                    response = "PLEASE INSERT CARD";
                    break;
                case "chipCardInserted":
                    response = "CARD INSERTED";
                    break;
                case "ChpWt":
                    response = "PLEASE WAIT";
                    break;
                case "ENTER PIN":
                    response = "PLEASE ENTER PIN";
                    break;
                case "EnterPin":
                    response = "PLEASE ENTER PIN";
                    break;
                case "EnterPin  PrsEnt":
                    response = "PLEASE ENTER PIN AND PRESS ENTER";
                    break;
                case "pleaseWait":
                    response = "PLEASE WAIT";
                    break;
                case "PlsWt":
                    response = "PLEASE WAIT";
                    break;
                case "PIN OK":
                    response = "PIN OK PLEASE WAIT";
                    break;
                case "PinOk":
                    response = "PIN OK PLEASE WAIT";
                    break;
                case "pressEnter":
                    response = "THEN PRESS ENTER";
                    break;
                case "firstPinRetry":
                    response = "FIRST PIN RE-TRY";
                    break;
                case "lastPinRetry":
                    response = "LAST PIN RE-TRY";
                    break;
                case "swipeCard":
                    response = "PLEASE SWIPE CARD";
                    break;
                case "iccFallBack":
                    response = "ICC FALL BACK";
                    break;
                case "removeCardwithAwaitCheck":
                    response = "REMOVE CARD (AWAITING CHECK)";
                    break;
                case "cardSwiped":
                    response = "SWIPED : PLEASE WAIT";
                    break;
                case "swipedOk":
                    response = "SWIPED OK";
                    break;
                case "reinsertCardtry1":
                    response = "RE-INSERT OR SWIPE CARD(TRY # 1)";
                    break;
                case "reinsertCardtry2":
                    response = "RE-INSERT OR SWIPE CARD(TRY # 2)";
                    break;
                case "cardNotAccepted":
                    response = "CARD NOT ACCEPTED";
                    break;
                case "insertCard_T":
                    response = "PLEASE INSERT CARD";
                    break;
                case "connectingMsg":
                    response = "CONNECTING...";
                    break;
                case "authorisingMsg":
                    response = "AUTHORISING...";
                    break;
                case "transactionRefered":
                    response = "TRANSACTION REFERRED";
                    break;
                case "finalisingMsg":
                    response = "PIN OK PLEASE WAIT";
                    break;
                case "plsRemoveCard":
                    response = "PLEASE REMOVE CARD";
                    break;
                case "cardRemoved":
                    response = "CARD REMOVED";
                    break;
                case "waitingMsg":
                    response = "PIN OK PLEASE WAIT";
                    break;
                case "approvedOnline":
                    response = "APPROVED ONLINE";
                    break;
                case "declined_T":
                    response = "DECLINED (OFFLINE)";
                    break;
                case "approvedOffline":
                    response = "APPROVED OFFLINE";
                    break;
                default:
                    response = "PAYMENT IN PROCESS";
                    break;
            }
            log.LogMethodExit(response);
            return response;
        }

        private string BuildMessageCommand(CommandTransactionType CommandType, string message)
        {
            log.LogMethodEntry(CommandType);
            string command = "";
            log.LogVariableState("CommandType", CommandType.ToString());
            switch (CommandType.ToString())
            {

                case "REFERRAL":
                    command = "2=400"
                        + "\r\n3=1"//Referral rejected
                        + "\r\n99=0";
                    break;
                case "SIGNATURE":
                    command = "2=401"
                        + "\r\n3=0"//Signature Accepted
                        + "\r\n99=0";
                    break;
                case "FALLBACK":
                    command = "2=402"
                        + "\r\n3=0"//Fallback accepted
                        + "\r\n99=0";
                    break;
                case "CNPCONFIRMATION":
                    command = "2=403"
                        + "\r\n3=0"//CNP Confirmation accepted
                        + "\r\n99=0";
                    break;
                case "CASHBACK":
                    command = "2=405"
                        + "\r\n3=1"//Cashback rejected
                        + "\r\n99=0";
                    break;
                case "AVSCONFIRMATION"://AVS confirmation prompt 
                    command = "2=407"
                        + "\r\n3=0"//CNP Confirmation accepted
                        + ((message.Contains("3=Address") || message.Contains("3=Address,ZipCode")) ?
                          "\r\n7=" : "")//7=Address
                        + ((message.Contains("3=ZipCode") || message.Contains("3=Address,ZipCode")) ?
                          "\r\n8=" : "")//8=Zipcode
                        + "\r\n99=0";
                    break;
            }
            return command;
        }

        /// <summary>
        /// Convert string request to byte[] to be sent to Worldpay device
        /// </summary>
        /// <param name="command">Command to be sent</param>
        /// <returns>Byte[] to be sent to device</returns>
        private byte[] CreateByteCommand(string command)
        {
            log.LogMethodEntry(command);
            byte[] request;
            request = Encoding.ASCII.GetBytes(command);
            log.LogMethodExit(request);
            return request;
        }

        void ProcessRequest(Socket socketObj, CommandTransactionType commandType, string message)
        {
            log.LogMethodEntry("socketObj", commandType, message);
            byte[] byteRequestCommand = null;
            string command;
            if (socketObj != null)
            {
                command = BuildMessageCommand(commandType, message);
                log.LogVariableState("command", command);
                byteRequestCommand = CreateByteCommand(command);
                if (byteRequestCommand != null && byteRequestCommand.Length > 0)
                {
                    socketObj.SendTimeout = 10000;//10 second time out for sending command
                    log.LogVariableState("command in bytes", byteRequestCommand);
                    int bytesSent = socketObj.Send(byteRequestCommand);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// GatewayComponentNeedsRestart
        /// </summary>
        /// <returns></returns>
        public override bool GatewayComponentNeedsRestart()
        {
            log.LogMethodEntry();
            bool needsRestart = false;
            try
            {
                IPEndPoint remoteEP;
                IPHostEntry host = Dns.GetHostEntry(deviceUrl);
                IPAddress ipAddress = host.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, messgePortNo);
                socketMessage = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketMessage.Connect(remoteEP);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                needsRestart = true;
            }
            finally
            {
                if (socketMessage != null)
                {
                    CloseSocket(socketMessage);
                    try { socketMessage.Dispose(); } catch { }
                }
            }
            log.LogMethodExit(needsRestart);
            return needsRestart;
        }

        /// <summary>
        /// RestartPaymentGatewayService
        /// </summary>
        /// <param name="forceRestart"></param> 
        public override void RestartPaymentGatewayComponent(bool forceRestart = false)
        {
            log.LogMethodEntry(forceRestart);
            try
            {
                if (ApplicationLoader.IsAdministrator())
                {
                    RestartService(WORLDPAYSERVICENAME, true);
                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 4524));
                    // "Application should be Run as administrator to access service details"
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }
        private void CloseSocket(Socket socketMessage)
        {
            log.LogMethodEntry();
            if (socketMessage != null && socketMessage.Connected)
            {
                log.Debug("Closing Message socket normally");
                try
                {
                    socketMessage.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    log.Error("socketMessage.Shutdown", ex);
                }
                try
                {
                    socketMessage.Close();
                }
                catch (Exception ex)
                {
                    log.Error("socketMessage.Close", ex);
                }
            }
            log.LogMethodExit();
        }
    }
}
