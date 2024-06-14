/*************************************************************************************************************************************
* Project Name - Semnox.Parafait.Device.PaymentGateway - CyberSourceHandler
* Description  - CyberSource Handler class
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
**************************************************************************************************************************************
*2.50.0       25-Jan-2019      Archana            Created
 *2.130.4     22-Feb-2022   Mathew Ninan           Modified DateTime to ServerDateTime 
 *2.130.10    16-Sep-2022    Mathew Ninan         Added few fixes based on device testing. 
 *                                                 Also, changed the DLL provided by Cybersource 
*************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FABPOSLINK;
using FABECR;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal interface ICyberSourceHandler
    {
        CCTransactionsPGWDTO ProcessTransaction(CyberSourceRequest cyberSourceRequest);
        CCTransactionsPGWDTO VoidTransaction(CyberSourceRequest cyberSourceRequest);

    }

    internal class CyberSourceKioskHandler : ICyberSourceHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal FABPOSLINK.comFABPOSLINK POSLINK;
        internal CyberSourceKioskHandler()
        {
            log.LogMethodEntry();
            POSLINK = new comFABPOSLINK();
            log.LogMethodExit();
        }
        public CCTransactionsPGWDTO ProcessTransaction(CyberSourceRequest cyberSourceRequest)
        {
            log.LogMethodEntry(cyberSourceRequest);
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                if (!POSLINK.Initializer())
                {
                    log.LogMethodExit(null, "Failed to initialize device");
                    throw new Exception("Failed to initialize device");
                }

                POSLINK.VFI_GetAuth(FABPOSLINK.TransactionType.Sale, cyberSourceRequest.Amount, cyberSourceRequest.ReferenceNumber.ToString());
                cCTransactionsPGWDTO = ConvertReponseToTransactionPGWDTO(cyberSourceRequest);
                log.LogMethodExit();
                return cCTransactionsPGWDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null);
                throw ex;
            }
        }


        public CCTransactionsPGWDTO VoidTransaction(CyberSourceRequest cyberSourceRequest)
        {
            log.LogMethodEntry(cyberSourceRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                if (!POSLINK.Initializer())
                {
                    log.LogMethodExit(null, "Failed to initialize device");
                    throw new Exception("Failed to initialize device");
                }
                POSLINK.VFI_GetAuth(FABPOSLINK.TransactionType.Void, cyberSourceRequest.Amount, cyberSourceRequest.ReferenceNumber.ToString(), cyberSourceRequest.OriginalTrxNumber);
                cCTransactionsPGWDTO = ConvertReponseToTransactionPGWDTO(cyberSourceRequest);
                log.LogMethodExit();
                return cCTransactionsPGWDTO;
            }
            catch(Exception ex)
            {
                log.LogMethodExit("Void transaction failed" + ex.Message);
                log.Error(ex);
                throw ex;
            }
           
        }

        private CCTransactionsPGWDTO ConvertReponseToTransactionPGWDTO(CyberSourceRequest cyberSourceRequest)
        {
            log.LogMethodEntry(cyberSourceRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cCTransactionsPGWDTO.DSIXReturnCode = string.IsNullOrEmpty(POSLINK.GetSet_VFI_RespCode) ? "" : POSLINK.GetSet_VFI_RespCode;
            cCTransactionsPGWDTO.InvoiceNo = string.IsNullOrEmpty(POSLINK.GetSet_VFI_STAN) ? "0" : POSLINK.GetSet_VFI_STAN;
            cCTransactionsPGWDTO.TextResponse = string.IsNullOrEmpty(POSLINK.GetSet_VFI_EMV_AC) ? "" : POSLINK.GetSet_VFI_EMV_AC;//AC
            cCTransactionsPGWDTO.TokenID = string.IsNullOrEmpty(POSLINK.GetSet_VFI_EMV_AID) ? "" : POSLINK.GetSet_VFI_EMV_AID;//AID
            cCTransactionsPGWDTO.AcctNo = string.IsNullOrEmpty(POSLINK.GetSet_VFI_CardNum) ? "" : POSLINK.GetSet_VFI_CardNum;
            if (!string.IsNullOrEmpty(POSLINK.GetSet_VFI_TransType) && POSLINK.GetSet_VFI_TransType == "01")
                cCTransactionsPGWDTO.TranCode = "SALE";
            else
                cCTransactionsPGWDTO.TranCode = "VOID";
            cCTransactionsPGWDTO.ProcessData = string.IsNullOrEmpty(POSLINK.GetSet_VFI_EMV_TVR) ? "" : POSLINK.GetSet_VFI_EMV_TVR;//TVR
            cCTransactionsPGWDTO.Purchase = (Convert.ToDouble(cyberSourceRequest.Amount) / 100).ToString("##0");
            cCTransactionsPGWDTO.Authorize = (Convert.ToDouble(cyberSourceRequest.Amount) / 100).ToString("##0");//string.IsNullOrEmpty(POSLINK.GetSet_VFI_Amount) ? "0" : (Convert.ToDouble(POSLINK.GetSet_VFI_Amount)/100).ToString("##0");
            cCTransactionsPGWDTO.RecordNo = string.IsNullOrEmpty(POSLINK.GetSet_VFI_TID) ? "0": POSLINK.GetSet_VFI_TID;//TID
            cCTransactionsPGWDTO.AcqRefData = string.IsNullOrEmpty(POSLINK.GetSet_VFI_MID) ? "" : POSLINK.GetSet_VFI_MID;//MID
            cCTransactionsPGWDTO.UserTraceData = string.IsNullOrEmpty(POSLINK.GetSet_VFI_CHName) ? "" : POSLINK.GetSet_VFI_CHName;
            cCTransactionsPGWDTO.CardType = string.IsNullOrEmpty(POSLINK.GetSet_VFI_CardName) ? "" : POSLINK.GetSet_VFI_CardName;
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                cCTransactionsPGWDTO.TransactionDatetime = (string.IsNullOrEmpty(POSLINK.GetSet_VFI_DateTime)) ? ServerDateTime.Now : DateTime.ParseExact(POSLINK.GetSet_VFI_DateTime, "yyyyMMdd HHmmss", provider);
            }
            catch
            {
                cCTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
            }
            log.LogMethodExit();
            return cCTransactionsPGWDTO;
        }
    }

    internal class CyberSourcePOSHandler : ICyberSourceHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDisplayStatusUI showStatus;
        internal FABECR.ComFABECR ecr;
        internal CyberSourcePOSHandler()
        {
            log.LogMethodEntry();
            ecr = new FABECR.ComFABECR();
            log.LogMethodExit();
        }
        public CCTransactionsPGWDTO ProcessTransaction(CyberSourceRequest cyberSourceRequest)
        {
            log.LogMethodEntry(cyberSourceRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                //if (!ecr.VFI_InitializeAll())
                //{
                //    log.LogMethodExit(null, "Failed to initialize device");
                //    throw new Exception("Failed to initialize device");
                //}
                string portName = "COM8";
                int baudRate = 115200;
                short timeOut = 90;
                int trxTimeout = 120;
                int uploadTimeout = 100;
                string TracePath = "";
                bool flgTrace = true;
                bool flgWaitRes = true;
                bool delay = true;
                int delayTimeout = 1;
                if (!ecr.GetSettings(ref portName, ref baudRate, ref timeOut, ref trxTimeout, ref uploadTimeout, ref TracePath, ref flgTrace, ref flgWaitRes, ref delay, ref delayTimeout))
                {
                    log.LogMethodExit(null, "Failed to initialize port");
                    throw new Exception("Failed to initialize port");
                }
                log.Debug("Values - Port: " + portName + ". BaudRate: " + baudRate);
                string responseCode = string.Empty;
                string responseMessage = string.Empty;
                ecr.VFI_GetAuth(cyberSourceRequest.EcrTransactionType, cyberSourceRequest.EcrTid, cyberSourceRequest.Amount, cyberSourceRequest.EcrTrxCompletionAmount, cyberSourceRequest.EcrOriginalReceiptNo,
                    cyberSourceRequest.EcrReceiptNumber, ref responseCode, ref responseMessage);

                log.Debug("Response Value: " + responseCode + ". Response Message: " + responseMessage);
                cCTransactionsPGWDTO = ConvertReponseToTransactionPGWDTO(cyberSourceRequest, responseCode, responseMessage);
                log.LogMethodExit(cCTransactionsPGWDTO);
                return cCTransactionsPGWDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null);
                throw ex;
            }
        }

        public CCTransactionsPGWDTO VoidTransaction(CyberSourceRequest cyberSourceRequest)
        {
            log.LogMethodEntry(cyberSourceRequest);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
            try
            {
                if (!ecr.VFI_InitializeAll())
                {
                    log.LogMethodExit(null, "Failed to initialize device");
                    throw new Exception("Failed to initialize device");
                }
                string responseCode = string.Empty;
                string responseMessage = string.Empty;
                ecr.VFI_VoidTrans(cyberSourceRequest.EcrTid, cyberSourceRequest.Amount, cyberSourceRequest.EcrOriginalReceiptNo,
                    cyberSourceRequest.EcrReceiptNumber, ref responseCode, ref responseMessage);
               
                cCTransactionsPGWDTO = ConvertReponseToTransactionPGWDTO(cyberSourceRequest, responseCode, responseMessage);
                return cCTransactionsPGWDTO;
            }
            catch(Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private CCTransactionsPGWDTO ConvertReponseToTransactionPGWDTO(CyberSourceRequest cyberSourceRequest,string responseCode, string responseMessage)
        {
            log.LogMethodEntry(cyberSourceRequest, responseCode, responseMessage);
            CCTransactionsPGWDTO cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
            cCTransactionsPGWDTO.DSIXReturnCode = string.IsNullOrEmpty(ecr.GetSet_VFI_RespCode) ? "" : ecr.GetSet_VFI_RespCode;
            cCTransactionsPGWDTO.InvoiceNo = string.IsNullOrEmpty(ecr.GetSet_VFI_MessNum) ? "-1" : ecr.GetSet_VFI_MessNum;
            cCTransactionsPGWDTO.TextResponse = responseMessage;//ecr.GetSet_VFI_EMV_AC;//AC
            cCTransactionsPGWDTO.TokenID = string.IsNullOrEmpty(ecr.GetSet_VFI_EMV_AID) ? "" : ecr.GetSet_VFI_EMV_AID;//AID
            cCTransactionsPGWDTO.AcctNo = string.IsNullOrEmpty(ecr.GetSet_VFI_CardNum) ? "" : ecr.GetSet_VFI_CardNum;
            //cCTransactionsPGWDTO.TranCode = string.IsNullOrEmpty(ecr.GetSet_VFI_EMV_TSI) ? "" : ecr.GetSet_VFI_EMV_TSI;//TSI
            if (cyberSourceRequest.EcrTransactionType == "1")
            {
                cCTransactionsPGWDTO.TranCode = "SALE";
            }
            else if (cyberSourceRequest.EcrTransactionType == "5")
            {
                cCTransactionsPGWDTO.TranCode = "AUTH";
            }
            else if(cyberSourceRequest.EcrTransactionType == "6")
            {
                cCTransactionsPGWDTO.TranCode = "AUTH_COMPLETE";
            }
            else if (cyberSourceRequest.EcrTransactionType == "2")
            {
                cCTransactionsPGWDTO.TranCode = "REFUND";
            }
            else
            {
                cCTransactionsPGWDTO.TranCode = "VOID";
            }
            cCTransactionsPGWDTO.ProcessData = string.IsNullOrEmpty(ecr.GetSet_VFI_EMV_TVR) ? "" : ecr.GetSet_VFI_EMV_TVR;//TVR
            cCTransactionsPGWDTO.Purchase = (Convert.ToDouble(cyberSourceRequest.Amount) / 100).ToString();
            cCTransactionsPGWDTO.Authorize = string.IsNullOrEmpty(ecr.GetSet_VFI_Amount) ? "0" : (Convert.ToDouble(ecr.GetSet_VFI_Amount)/ 100).ToString("##0");
            cCTransactionsPGWDTO.RecordNo = string.IsNullOrEmpty(ecr.GetSet_VFI_TID) ? "0" : ecr.GetSet_VFI_TID;//TID
                                                               //cCTransactionsPGWDTO.ResponseOrigin = POSLINK.;
            cCTransactionsPGWDTO.AcqRefData = string.IsNullOrEmpty(ecr.GetSet_VFI_MID) ? "" : ecr.GetSet_VFI_MID;//MID
            cCTransactionsPGWDTO.UserTraceData = string.IsNullOrEmpty(ecr.GetSet_VFI_CHName) ? "" : ecr.GetSet_VFI_CHName;
            cCTransactionsPGWDTO.CardType = string.IsNullOrEmpty(ecr.GetSet_VFI_CardSchemeName) ? "" : ecr.GetSet_VFI_CardSchemeName;
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                cCTransactionsPGWDTO.TransactionDatetime = (string.IsNullOrEmpty(ecr.GetSet_VFI_DateTime)) ? ServerDateTime.Now : DateTime.ParseExact(ecr.GetSet_VFI_DateTime, "yyyyMMdd HHmmss", provider);
            }
            catch
            {
                cCTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
            }
            log.LogMethodExit(cCTransactionsPGWDTO);
            return cCTransactionsPGWDTO;
        }
    }

    internal class CyberSourceRequest
    {
        private string amount;
        private int referenceNumber;
        private string originalTrxNumber;
        private TransactionType transactionType;
        private string ecrTransactionType;
        private string ecrTid;
        private string ecrTrxCompletionAmount;
        private string ecrReceiptNumber;
        private string ecrOriginalReceiptNo;
        private string ecrResponseCode;
        private string ecrReponseMessage;

        internal int ReferenceNumber
        {
            get { return referenceNumber; }
            set { referenceNumber = value; }
        }

        internal string Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        internal string OriginalTrxNumber
        {
            get { return originalTrxNumber; }
            set { originalTrxNumber = value; }
        }

        internal TransactionType TransactionType
        {
            get { return transactionType; }
            set { transactionType = value; }
        }
        internal string EcrTransactionType
        {
            get { return ecrTransactionType; }
            set { ecrTransactionType = value; }
        }

        internal string EcrTid
        {
            get { return ecrTid; }
            set { ecrTid = value; }
        }

        internal string EcrTrxCompletionAmount
        {
            get { return ecrTrxCompletionAmount; }
            set { ecrTrxCompletionAmount = value; }
        }

        internal string EcrReceiptNumber
        {
            get { return ecrReceiptNumber; }
            set { ecrReceiptNumber = value; }
        }

        internal string EcrOriginalReceiptNo
        {
            get { return ecrOriginalReceiptNo; }
            set { ecrOriginalReceiptNo = value; }
        }

        internal string EcrResponseCode
        {
            get { return ecrResponseCode; }
            set { ecrResponseCode = value; }
        }

        internal string EcrReponseMessage
        {
            get { return ecrReponseMessage; }
            set { ecrReponseMessage = value; }
        }
    }
}
