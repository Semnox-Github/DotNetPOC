/********************************************************************************************
 * Project Name - Moneris Payment Gateway
 * Description  - This is the core class and using this the pos will comunicate with MonerisPaymentGateway
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Aug-2017   Raghuveera          Created 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
//using Semnox.Core.Languages;
//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Semnox.Parafait.Languages;
using Nst;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class MonerisPaymentGateway : PaymentGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Terminal terminal;
        private MonerisUx300 monerisUx300;
        private MonerisIPP320 monerisIPP320;
        private int englishLangId;
        private int frenchLangId;
        bool IsInitialized;
        bool IsAuthEnabled;
        int comPort;
        string host;
        string storeId;
        string tokenId;
        bool isTipEnabled = false;

        public MonerisPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            : base(utilities, isUnattended, showMessageDelegate, writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);

            comPort = Convert.ToInt32(utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
            host = utilities.getParafaitDefaults("CREDIT_CARD_HOST_URL");
            storeId = utilities.getParafaitDefaults("CREDIT_CARD_STORE_ID");
            tokenId = utilities.getParafaitDefaults("CREDIT_CARD_TOKEN_ID");
            isTipEnabled = utilities.getParafaitDefaults("SHOW_TIP_AMOUNT_KEYPAD").Equals("Y");
            IsAuthEnabled = utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y");
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate 
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (comPort <= 0)
            {
                log.LogMethodExit(null, "Throwing Exception - Moneris CREDIT_CARD_TERMINAL_PORT_NO not set.");
                throw new Exception("Moneris CREDIT_CARD_TERMINAL_PORT_NO not set.");
            }
            //if (!isUnattended)
            //{
            //    if (string.IsNullOrEmpty(host))
            //    {
            //        log.LogMethodExit(null, "Throwing Exception - Moneris CREDIT_CARD_HOST_URL not set.");
            //        throw new Exception("Moneris CREDIT_CARD_HOST_URL not set.");
            //    }
            //    if (string.IsNullOrEmpty(storeId))
            //    {
            //        log.LogMethodExit(null, "Throwing Exception - Moneris CREDIT_CARD_STORE_ID not set.");
            //        throw new Exception("Moneris CREDIT_CARD_STORE_ID not set.");
            //    }
            //    if (string.IsNullOrEmpty(tokenId))
            //    {
            //        log.LogMethodExit(null, "Throwing Exception - Moneris CREDIT_CARD_TOKEN_ID not set.");
            //        throw new Exception("Moneris CREDIT_CARD_TOKEN_ID not set.");
            //    }
            //}
            englishLangId = frenchLangId = -1;

            log.LogMethodExit(null);
        }

        ~MonerisPaymentGateway()
        {
            log.LogMethodEntry();

            if (terminal != null)
                terminal.Close();

            log.LogMethodExit(null);
        }

        public override void Initialize()
        {
            log.LogMethodEntry();

            try
            {
                Semnox.Parafait.Languages.Languages languages = new Semnox.Parafait.Languages.Languages(utilities.ExecutionContext);
                List<LanguagesDTO> languagesDTOList = new List<LanguagesDTO>();
                List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> languageSerachParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                languageSerachParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_CODE, "en-US"));
                languagesDTOList = languages.GetAllLanguagesList(languageSerachParam);
                if (languagesDTOList != null && languagesDTOList.Count > 0)
                {
                    englishLangId = languagesDTOList[0].LanguageId;
                }
                languageSerachParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
                languageSerachParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.LANGUAGE_CODE, "fr-FR"));
                languagesDTOList = languages.GetAllLanguagesList(languageSerachParam);
                if (languagesDTOList != null && languagesDTOList.Count > 0)
                {
                    frenchLangId = languagesDTOList[0].LanguageId;
                }
                //isUnattended = true; //to test Ux device
                if (comPort > 0
                     && string.IsNullOrEmpty(host) && string.IsNullOrEmpty(storeId))
                {
                    monerisUx300 = new MonerisUx300(comPort, utilities);
                    monerisUx300.setLanguage(englishLangId, frenchLangId);
                    IsInitialized = monerisUx300.ProcessTransaction(TransactionType.INIT, null, false);
                }
                else if (comPort > 0 && !string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(storeId)
                          && !string.IsNullOrEmpty(tokenId))
                {
                    monerisIPP320 = new MonerisIPP320(isUnattended, utilities);
                    monerisIPP320.setLanguage(englishLangId, frenchLangId);
                    monerisIPP320.InitializeIPP320(ref terminal, comPort, host, storeId, tokenId, isTipEnabled);
                    monerisIPP320.ProcessTransaction(-1, TransactionType.INIT, null, "");
                    IsInitialized = monerisIPP320.Isinitialized;
                }
                else
                {
                    log.Error("Either host, StoreId or TokenId is not set.");
                    throw new Exception("Moneris Payment Gateway set up incomplete.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while initialization", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }

        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            TransactionType trxType = TransactionType.SALE;
            string trxNo = "";
            try
            {
                StandaloneRefundNotAllowed(transactionPaymentsDTO);
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                if (monerisIPP320 != null)
                    IsInitialized = monerisIPP320.Isinitialized;
                if (isUnattended)
                {
                    if (IsInitialized)
                    {
                        if (monerisUx300 != null)
                        {
                            monerisUx300.ProcessTransaction(trxType, transactionPaymentsDTO, printReceipt);
                        }
                        else if (monerisIPP320 != null)
                        {
                            trxNo = (ServerDateTime.Now.Date.Year + ServerDateTime.Now.Date.Month + ServerDateTime.Now.Date.Day - 2000) + "_" + ServerDateTime.Now.ToString("HHmmss") + "_" + utilities.ParafaitEnv.POSMachineId.ToString();
                            monerisIPP320.ProcessTransaction(ccRequestPGWDTO.RequestID, trxType, transactionPaymentsDTO, trxNo);
                        }
                        else
                        {
                            log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                            throw new Exception(utilities.MessageUtils.getMessage("Transaction can not be processed without initialization. Please restart the application."));
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                        throw new Exception(utilities.MessageUtils.getMessage("Transaction can not be processed without initialization. Please restart the application."));

                    }
                }
                else
                {
                    //IsInitialized = monerisIPP320.Isinitialized;
                    if (IsInitialized)
                    {
                        if (IsAuthEnabled)
                        {

                            Semnox.Parafait.Device.PaymentGateway.Menories.ftmTransactionType ftmTranType = new Semnox.Parafait.Device.PaymentGateway.Menories.ftmTransactionType(utilities, trxType);
                            if (ftmTranType.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                trxType = ftmTranType.transactionType;
                            }


                        }
                        trxNo = (DateTime.Today.Year + DateTime.Today.Month + DateTime.Today.Day - 2000) + "_" + DateTime.Now.ToString("HHmmss") + "_" + utilities.ParafaitEnv.POSMachineId.ToString();
                        monerisIPP320.ProcessTransaction(ccRequestPGWDTO.RequestID, trxType, transactionPaymentsDTO, trxNo);
                    }
                    else
                    {
                        if (System.Windows.Forms.MessageBox.Show(utilities.MessageUtils.getMessage("Device initialization is not done on load.\n Do you want to do it now?"), "Moneris Payment Gateway", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (monerisIPP320.IsInitializationInProgress)
                            {
                                log.LogMethodExit(null, "Throwing Exception - Initialization process is in progress... Please try after some time...");
                                throw new Exception(utilities.MessageUtils.getMessage("Initialization process is in progress... Please try after some time..."));
                            }
                            if (terminal != null)
                            {
                                terminal.Close();
                            }
                            Initialize();
                            log.LogMethodExit(null, "Throwing Exception - Please wait for initialization to complete, then click apply again to do the transaction.");
                            throw new Exception(utilities.MessageUtils.getMessage("Please wait for initialization to complete, then click apply again to do the transaction."));
                        }
                        else
                        {
                            log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                            throw new Exception(utilities.MessageUtils.getMessage("Transaction can not be processed without initialization. Please restart the application."));
                        }
                    }
                }
                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making payments", ex);
                log.Fatal("Ends-MakePayment()  Exception:" + ex.ToString());
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }

        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            double tipAmount = 0;
            try
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                CCTransactionsPGWDTO ccTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                if (monerisIPP320 != null)
                    IsInitialized = monerisIPP320.Isinitialized;
                if (isUnattended)
                {
                    if (IsInitialized)
                    {
                        if (monerisUx300 != null)
                        {
                            monerisUx300.ProcessTransaction(TransactionType.REFUND, transactionPaymentsDTO, printReceipt);
                        }
                        else if (monerisIPP320 != null)
                        {
                            monerisIPP320.ProcessTransaction(Convert.ToInt32(ccTransactionsPGWDTO.RefNo), TransactionType.VOID, transactionPaymentsDTO, ccTransactionsPGWDTO.UserTraceData);
                        }
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                        throw new Exception("Transaction can not be processed without initialization. Please restart the application.");
                    }
                }
                else
                {
                    //IsInitialized = monerisIPP320.Isinitialized;
                    if (IsInitialized)
                    {
                        tipAmount = transactionPaymentsDTO.TipAmount;
                        //transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount;
                        if (ccTransactionsPGWDTO.TransactionDatetime.AddMinutes(20).CompareTo(ServerDateTime.Now) >= 0 && (transactionPaymentsDTO.Amount + transactionPaymentsDTO.TipAmount) == Convert.ToDouble(ccTransactionsPGWDTO.Authorize))
                        {
                            monerisIPP320.ProcessTransaction(Convert.ToInt32(ccTransactionsPGWDTO.RefNo), TransactionType.VOID, transactionPaymentsDTO, ccTransactionsPGWDTO.UserTraceData);

                        }
                        else
                        {
                            monerisIPP320.ProcessTransaction(Convert.ToInt32(ccTransactionsPGWDTO.RefNo), TransactionType.REFUND, transactionPaymentsDTO, ccTransactionsPGWDTO.UserTraceData);
                        }
                        transactionPaymentsDTO.TipAmount = tipAmount;
                        transactionPaymentsDTO.Amount = transactionPaymentsDTO.Amount - tipAmount;
                        transactionPaymentsDTO.Reference = "Moneris";
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                        throw new Exception("Transaction can not be processed without initialization. Please restart the application.");
                    }
                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;

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
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override bool IsSettlementPending(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            bool returnValue = false;
            if (IsAuthEnabled)
            {
                if (transactionPaymentsDTO != null && transactionPaymentsDTO.CCResponseId != -1)
                {
                    CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
                    List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.RESPONSE_ID, transactionPaymentsDTO.CCResponseId.ToString()));
                    searchParameters.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.TRAN_CODE, "01"));
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
        public override TransactionPaymentsDTO PerformSettlement(TransactionPaymentsDTO transactionPaymentsDTO, bool IsForcedSettlement = false)
        {
            log.LogMethodEntry(transactionPaymentsDTO, IsForcedSettlement);

            string trxNo = "";
            try
            {
                CCRequestPGWDTO ccRequestPGWDTO = CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
                if (monerisIPP320 != null)
                    IsInitialized = monerisIPP320.Isinitialized;
                if (IsInitialized)
                {
                    CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
                    CCTransactionsPGWDTO ccTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
                    trxNo = ccTransactionsPGWDTO.UserTraceData;
                    monerisIPP320.ProcessTransaction(Convert.ToInt32(ccTransactionsPGWDTO.RefNo), TransactionType.COMPLETION, transactionPaymentsDTO, trxNo);
                }
                else
                {
                    log.LogMethodExit(null, "Throwing Exception - Transaction can not be processed without initialization. Please restart the application.");
                    throw new Exception(utilities.MessageUtils.getMessage("Transaction can not be processed without initialization. Please restart the application."));

                }

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;

            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing settlement", ex);
                log.LogMethodExit(null, "Throwing Exception " + ex);
                throw (ex);
            }
        }
    }
}
