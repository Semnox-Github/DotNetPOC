using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class NetEpayRequestDTO
    {
        private string hostOrIP;
        private string ipPort;
        private string merchantID;
        private string pOSPackageID;
        private string operatorID;
        private string userTrace;
        private string tranType;
        private string tranCode;
        private string secureDevice;
        private string comPort;
        private string pinPadIpAddress;
        private string pinPadIpPort;
        private string invoiceNo;
        private string refNo;
        private string collectData;
        private string authCode;
        private string sequenceNo;
        private string okAmount;
        private string duplicate;
        private string partialAuth;
        private string recordNo;
        private string frequency;
        private string acqRefData;
        private string processData;
        private string displayTextHandle;
        private string returnCustomerData;
        private string forceOffline;
        private string maxTransactions;
        private string offlineTransactionPurchaseLimit;
        private string cmdStatus;
        private string question;
        private string questionTimeout;
        private string answerYes;
        private string answerNo;
        NetEpayAmount amount;
        NetEpayAccount account;

        //    public NetEpayRequestDTO(string hostOrIP, string ipPort, string merchantID, string pOSPackageID, string operatorID, string userTrace, string tranType,string tranCode,
        //                                string secureDevice, string comPort, string pinPadIpAddress, string pinPadIpPort, string invoiceNo, string refNo, string collectData,
        //                                string authCode, string sequenceNo, string okAmount, string duplicate, string partialAuth, string recordNo, string frequency, string acqRefData,
        //                                string processData, string displayTextHandle, string returnCustomerData, string forceOffline, string maxTransactions, string offlineTransactionPurchaseLimit,
        //                                string cmdStatus, string question, string questionTimeout, string answerYes, string answerNo, NetEpayAmount amount, NetEpayAccount account)
        //    {
        //        this.hostOrIP = hostOrIP;
        //        this.ipPort = ipPort;
        //        this.merchantID = merchantID;
        //        this.pOSPackageID = pOSPackageID;
        //        this.operatorID = operatorID;
        //        this.userTrace = userTrace;
        //        this.tranType = tranType;
        //        this.tranCode = tranCode;
        //        this.secureDevice = secureDevice;
        //        this.comPort = comPort;
        //        this.pinPadIpAddress = pinPadIpAddress;
        //        this.pinPadIpPort = pinPadIpPort;
        //        this.invoiceNo = invoiceNo;
        //        this.refNo = refNo;
        //        this.collectData = collectData;
        //        this.authCode = authCode;
        //        this.sequenceNo = sequenceNo;
        //        this.okAmount = okAmount;
        //        this.duplicate = duplicate;
        //        this.partialAuth = partialAuth;
        //        this.recordNo = recordNo;
        //        this.frequency = frequency;
        //        this.acqRefData = acqRefData;
        //        this.processData = processData;
        //        this.displayTextHandle = displayTextHandle;
        //        this.returnCustomerData = returnCustomerData;
        //        this.forceOffline = forceOffline;
        //        this.maxTransactions = maxTransactions;
        //        this.offlineTransactionPurchaseLimit = offlineTransactionPurchaseLimit;
        //        this.cmdStatus = cmdStatus;
        //        this.question = question;
        //        this.questionTimeout = questionTimeout;
        //        this.answerYes = answerYes;
        //        this.answerNo = answerNo;
        //        this.amount = amount;
        //        this.account = account;
        //}
        public NetEpayRequestDTO()
        {
            this.hostOrIP = null;
            this.ipPort = null;
            this.merchantID = null;
            this.pOSPackageID = null;
            this.operatorID = null;
            this.userTrace = null;
            this.tranType = null;
            this.tranCode = null;
            this.secureDevice = null;
            this.comPort = null;
            this.pinPadIpAddress = null;
            this.pinPadIpPort = null;
            this.invoiceNo = null;
            this.refNo = null;
            this.collectData = null;
            this.authCode = null;
            this.sequenceNo = null;
            this.okAmount = null;
            this.duplicate = null;
            this.partialAuth = null;
            this.recordNo = null;
            this.frequency = null;
            this.acqRefData = null;
            this.processData = null;
            this.displayTextHandle = null;
            this.returnCustomerData = null;
            this.forceOffline = null;
            this.maxTransactions = null;
            this.offlineTransactionPurchaseLimit = null;
            this.cmdStatus = null;
            this.question = null;
            this.questionTimeout = null;
            this.answerYes = null;
            this.answerNo = null;
            this.amount = null;
            this.account = null;
        }
        public NetEpayRequestDTO(string hostOrIP, string ipPort, string merchantID, string secureDevice, string comPort, string pinPadIpAddress, string pinPadIpPort)
        {
            this.hostOrIP = hostOrIP;
            this.ipPort = ipPort;
            this.merchantID = merchantID;
            this.secureDevice = secureDevice;
            this.comPort = comPort;
            this.pinPadIpAddress = pinPadIpAddress;
            this.pinPadIpPort = pinPadIpPort;
            this.sequenceNo = "0010010010";
            //this.POSPackageID = "Parafait:2.100";
            this.pOSPackageID = Assembly.GetEntryAssembly().GetName().Name+":"+ Assembly.GetEntryAssembly().GetName().Version;
        }
        public string HostOrIP
        {
            get
            {
                return hostOrIP;
            }
            set
            {
                hostOrIP = value;
            }
        }
        public string IpPort
        {
            get
            {
                return ipPort;
            }
            set
            {
                ipPort = value;
            }
        }
        public string MerchantID
        {
            get
            {
                return merchantID;
            }
            set
            {
                merchantID = value;
            }
        }
        public string POSPackageID
        {
            get
            {
                return pOSPackageID;
            }
            set
            {
                pOSPackageID = value;
            }
        }
        public string OperatorID
        {
            get
            {
                return operatorID;
            }
            set
            {
                operatorID = value;
            }
        }
        public string UserTrace
        {
            get
            {
                return userTrace;
            }
            set
            {
                userTrace = value;
            }
        }
        public string TranType
        {
            get
            {
                return tranType;
            }
            set
            {
                tranType = value;
            }
        }
        public string TranCode
        {
            get
            {
                return tranCode;
            }
            set
            {
                tranCode = value;
            }
        }
        public string SecureDevice
        {
            get
            {
                return secureDevice;
            }
            set
            {
                secureDevice = value;
            }
        }
        public string ComPort
        {
            get
            {
                return comPort;
            }
            set
            {
                comPort = value;
            }
        }
        public string PinPadIpAddress
        {
            get
            {
                return pinPadIpAddress;
            }
            set
            {
                pinPadIpAddress = value;
            }
        }
        public string PinPadIpPort
        {
            get
            {
                return pinPadIpPort;
            }
            set
            {
                pinPadIpPort = value;
            }
        }
        public string InvoiceNo
        {
            get
            {
                return invoiceNo;
            }
            set
            {
                invoiceNo = value;
            }
        }

        public string CollectData
        {
            get
            {
                return collectData;
            }
            set
            {
                collectData = value;
            }
        }
        public string RefNo
        {
            get
            {
                return refNo;
            }
            set
            {
                refNo = value;
            }
        }
        public string AuthCode
        {
            get
            {
                return authCode;
            }
            set
            {
                authCode = value;
            }
        }
        public string SequenceNo
        {
            get
            {
                return sequenceNo;
            }
            set
            {
                sequenceNo = value;
            }
        }
        public string OKAmount
        {
            get
            {
                return okAmount;
            }
            set
            {
                okAmount = value;
            }
        }
        public string Duplicate
        {
            get
            {
                return duplicate;
            }
            set
            {
                duplicate = value;
            }
        }
        public string PartialAuth
        {
            get
            {
                return partialAuth;
            }
            set
            {
                partialAuth = value;
            }
        }
        public string RecordNo
        {
            get
            {
                return recordNo;
            }
            set
            {
                recordNo = value;
            }
        }
        public string Frequency
        {
            get
            {
                return frequency;
            }
            set
            {
                frequency = value;
            }
        }
        public string AcqRefData
        {
            get
            {
                return acqRefData;
            }
            set
            {
                acqRefData = value;
            }
        }
        public string ProcessData
        {
            get
            {
                return processData;
            }
            set
            {
                processData = value;
            }
        }
        public string DisplayTextHandle
        {
            get
            {
                return displayTextHandle;
            }
            set
            {
                displayTextHandle = value;
            }
        }
        public string ReturnCustomerData
        {
            get
            {
                return returnCustomerData;
            }
            set
            {
                returnCustomerData = value;
            }
        }
        public string ForceOffline
        {
            get
            {
                return forceOffline;
            }
            set
            {
                forceOffline = value;
            }
        }
        public string MaxTransactions
        {
            get
            {
                return maxTransactions;
            }
            set
            {
                maxTransactions = value;
            }
        }
        public string OfflineTransactionPurchaseLimit
        {
            get
            {
                return offlineTransactionPurchaseLimit;
            }
            set
            {
                offlineTransactionPurchaseLimit = value;
            }
        }

        public string CmdStatus
        {
            get
            {
                return cmdStatus;
            }
            set
            {
                cmdStatus = value;
            }
        }
        public NetEpayAmount Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
            }
        }
        public NetEpayAccount Account
        {
            get
            {
                return account;
            }
            set
            {
                account = value;
            }
        }
        public string Question
        {
            get
            {
                return question;
            }
            set
            {
                question = value;
            }
        }
        public string QuestionTimeout
        {
            get
            {
                return questionTimeout;
            }
            set
            {
                questionTimeout = value;
            }
        }
        public string AnswerYes
        {
            get
            {
                return answerYes;
            }
            set
            {
                answerYes = value;
            }
        }
        public string AnswerNo
        {
            get
            {
                return answerNo;
            }
            set
            {
                answerNo = value;
            }
        }
    }

    [Serializable]
    public class TStream
    {
        private NetEpayRequestDTO transaction;
        private NetEpayRequestDTO admin;

        public TStream()
        {
            this.transaction = null;
            this.admin = null;
        }
        public TStream(NetEpayRequestDTO transaction, NetEpayRequestDTO admin)
        {
            this.transaction = transaction;
            this.admin = admin;
        }
        public NetEpayRequestDTO Transaction
        {
            get
            {
                return transaction;
            }
            set
            {
                transaction = value;
            }
        }
        public NetEpayRequestDTO Admin
        {
            get
            {
                return admin;
            }
            set
            {
                admin = value;
            }
        }
    }
    class NetEpayRequestBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DSIEMVXLib.DsiEMVX dsiEMVX;
        bool isManual;
        private readonly ExecutionContext executionContext;

        enum TranCode
        {
            AdjustByRecordNo,
            PreAuthCaptureByRecordNo,
            SaleByRecordNo,
            EMVSale,
            PreAuthByRecordNo,
            EMVPreAuth,
            EMVZeroAuth,
            EMVReturn,
            ReturnByRecordNo,
            VoidSaleByRecordNo,
            GetAnswer,
            BatchReportTransaction
        }
        public NetEpayRequestBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="cCTransactionsPGWDTO"></param>
        /// <param name="amount"></param>
        /// <param name="tips"></param>
        /// <param name="capture"></param>
        /// <returns></returns>
        public void CreatePaymentAdjustRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, string amount, string tips)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, null, (tips == "0.00") ? null : tips, null, null);
            
           
            netEpayRequestDTO.TranCode = TranCode.PreAuthCaptureByRecordNo.ToString();
            
            
            netEpayRequestDTO.TranType = "Credit";
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.InvoiceNo = cCTransactionsPGWDTO.InvoiceNo;
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            netEpayRequestDTO.ProcessData = cCTransactionsPGWDTO.ProcessData;
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit();
            
        }

        public void CreateTipAdjustRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, double amount, double tips)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            //string currencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_SYMBOL", "Rs");
            string amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT","N2");
            //string currencySymbolWithAmountFormat = currencySymbol + amountFormat;
            NetEpayAmount Amount = new NetEpayAmount(amount.ToString(amountFormat), null, tips.ToString(amountFormat), null, cCTransactionsPGWDTO.Authorize);

            if (tips < 0)
            {
                netEpayRequestDTO.TranCode = TranCode.ReturnByRecordNo.ToString();
                tips = tips * -1;
                Amount.Gratuity = null;
                Amount.Purchase = tips.ToString(amountFormat);
                Amount.OriginalAuthorized = null;
            }
            else
            {
                netEpayRequestDTO.TranCode = TranCode.AdjustByRecordNo.ToString();
            }

            netEpayRequestDTO.TranType = "Credit";
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            netEpayRequestDTO.ProcessData = cCTransactionsPGWDTO.ProcessData;
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit();
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="cCTransactionsPGWDTO"></param>
        /// <param name="amount"></param>
        /// <param name="tips"></param>
        /// <param name="capture"></param>
        /// <returns></returns>

        public NetEpayRequestDTO CreateAdjustTipRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, double amount, double tips, bool capture = false)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(String.Format("{0:0.00}", amount), null, String.Format("{0:0.00}", tips), null, (!capture) ? null : cCTransactionsPGWDTO.Authorize);
            netEpayRequestDTO.TranType = "Credit";
            if (!capture)
            {
                netEpayRequestDTO.TranCode = TranCode.PreAuthCaptureByRecordNo.ToString();
            }
            else
            {
                netEpayRequestDTO.TranCode = TranCode.AdjustByRecordNo.ToString();
                //netEpayRequestDTO.Amount.OriginalAuthorized = cCTransactionsPGWDTO.Authorize;
            }
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="amount"></param>
        /// <param name="zeroAuth"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreatePaymentRequest(NetEpayRequestDTO netEpayRequestDTO, string amount, bool zeroAuth = false)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, null, null, null, null);

            if (zeroAuth)
            {
                netEpayRequestDTO.TranCode = TranCode.SaleByRecordNo.ToString();
                netEpayRequestDTO.TranType = "Credit";
            }
            else
            {
                netEpayRequestDTO.TranCode = TranCode.EMVSale.ToString();
            }
            
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            //netEpayRequestDTO.DisplayTextHandle = "00101484"; //8 digit hexadecimal string representing the window handle of a POS control capable of displaying text
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="amount"></param>
        /// <param name="zeroAuth"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreatePreAuthRequest(NetEpayRequestDTO netEpayRequestDTO, string amount, bool zeroAuth = false)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, amount, null, null, null);
            
            if (zeroAuth)
            {
                netEpayRequestDTO.TranCode = TranCode.PreAuthByRecordNo.ToString();
                netEpayRequestDTO.TranType = "Credit";
            }
            else
            {
                netEpayRequestDTO.TranCode = TranCode.EMVPreAuth.ToString();
            }
            
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.OKAmount = "DisAllow";
            netEpayRequestDTO.Duplicate = "None";
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            //netEpayRequestDTO.DisplayTextHandle = "00101484";//8 digit hexadecimal string representing the window handle of a POS control capable of displaying text
            //netEpayRequestDTO.ReturnCustomerData = "Payment";
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";


            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreateZeroAuthRequest(NetEpayRequestDTO netEpayRequestDTO, string amount)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, null, null, null, null);
            
            netEpayRequestDTO.Amount = Amount;
            netEpayRequestDTO.TranCode = TranCode.EMVZeroAuth.ToString();
            netEpayRequestDTO.OKAmount = "DisAllow";
            netEpayRequestDTO.PartialAuth = "PartialAuth";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            //netEpayRequestDTO.DisplayTextHandle = "00101484";//8 digit hexadecimal string representing the window handle of a POS control capable of displaying text


            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="cCTransactionsPGWDTO"></param>
        /// <param name="amount"></param>
        /// <param name="tip"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreateVoidRequest(NetEpayRequestDTO netEpayRequestDTO, CCTransactionsPGWDTO cCTransactionsPGWDTO, string amount, string tip)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, null, (tip == "0.00")? null : tip, null, null);
            
            netEpayRequestDTO.TranCode = TranCode.VoidSaleByRecordNo.ToString();
            netEpayRequestDTO.RefNo = cCTransactionsPGWDTO.RefNo;
            netEpayRequestDTO.InvoiceNo = cCTransactionsPGWDTO.InvoiceNo;
            netEpayRequestDTO.AuthCode = cCTransactionsPGWDTO.AuthCode;
            netEpayRequestDTO.RecordNo = cCTransactionsPGWDTO.TokenID;
            netEpayRequestDTO.Frequency = "OneTime";
            netEpayRequestDTO.TranType = "Credit";
            netEpayRequestDTO.AcqRefData = cCTransactionsPGWDTO.AcqRefData;
            netEpayRequestDTO.ProcessData = cCTransactionsPGWDTO.ProcessData;
            //netEpayRequestDTO.ForceOffline = "N";
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";
            netEpayRequestDTO.Amount = Amount;

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreateReturnRequest(NetEpayRequestDTO netEpayRequestDTO, string amount, bool noCard = true)
        {
            log.LogMethodEntry(netEpayRequestDTO);

            
            string resultString = string.Empty;
            NetEpayAmount Amount = new NetEpayAmount(amount, null, null, null, null);
            
            netEpayRequestDTO.Amount = Amount;
            if (noCard)
            {
                netEpayRequestDTO.TranCode = TranCode.EMVReturn.ToString();
            }
            else
            {
                netEpayRequestDTO.TranCode = TranCode.ReturnByRecordNo.ToString();
                netEpayRequestDTO.TranType = "Credit";
            }
            netEpayRequestDTO.CollectData = "CardholderName";
            netEpayRequestDTO.Duplicate = "None";
            netEpayRequestDTO.OKAmount = "DisAllow";
            netEpayRequestDTO.PartialAuth = "Allow";
            netEpayRequestDTO.RecordNo = "RecordNumberRequested";
            netEpayRequestDTO.Frequency = "OneTime";
            //netEpayRequestDTO.DisplayTextHandle = "00101484";//8 digit hexadecimal string representing the window handle of a POS control capable of displaying text
            //netEpayRequestDTO.MaxTransactions = "0";
            //netEpayRequestDTO.OfflineTransactionPurchaseLimit = "0.00";

            log.LogMethodExit(resultString);
            return netEpayRequestDTO;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="cardReadCheckRequestDTO"></param>
        ///// <param name="utilities"></param>
        ///// <returns></returns>
        //public bool isManualEntry(NetEpayRequestDTO cardReadCheckRequestDTO, Utilities utilities)
        //{
        //    log.LogMethodEntry();

        //    isManual = false;
        //    cardReadCheckRequestDTO.TranCode = TranCode.GetAnswer.ToString();
        //    cardReadCheckRequestDTO.Question = utilities.MessageUtils.getMessage(2859);
        //    cardReadCheckRequestDTO.QuestionTimeout = "60";
        //    cardReadCheckRequestDTO.AnswerYes = "Yes";
        //    cardReadCheckRequestDTO.AnswerNo = "No";
        //    dsiEMVX = new DSIEMVXLib.DsiEMVX();
        //    string resultString = string.Empty;
        //    TStream tStream = new TStream(cardReadCheckRequestDTO, null);
        //    tStream.Transaction = cardReadCheckRequestDTO;

        //    XmlSerializer requestXml = new XmlSerializer(tStream.GetType());
        //    MemoryStream requestStream = new MemoryStream();

        //    requestXml.Serialize(requestStream, tStream);
        //    requestStream.Position = 0;
        //    using (StreamReader reader = new StreamReader(requestStream))
        //    {
        //        String requestString = reader.ReadToEnd();
        //        resultString = dsiEMVX.ProcessTransaction(requestString);
        //    }

        //    XElement printData = XElement.Parse(resultString).Element("PrintData");
        //    RStream rStream = new RStream();
        //    XmlSerializer serializer = new XmlSerializer(rStream.GetType());
        //    using (StringReader sr = new StringReader(resultString))
        //    {
        //        rStream = (RStream)serializer.Deserialize(sr);
        //    }
        //    if (rStream.CmdResponse.CmdStatus.Equals("Success"))
        //    {
        //        isManual = true;
        //    }
        //    log.LogMethodExit(isManual);
        //    return isManual;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netEpayRequestDTO"></param>
        /// <param name="cCRequestPGWDTO"></param>
        /// <returns></returns>
        public NetEpayRequestDTO CreateTransactionStatusEnquiry(NetEpayRequestDTO netEpayRequestDTO, CCRequestPGWDTO cCRequestPGWDTO)
        {
            log.LogMethodEntry();

            netEpayRequestDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
            netEpayRequestDTO.TranType = "Administrative";
            netEpayRequestDTO.TranCode = TranCode.BatchReportTransaction.ToString();
            netEpayRequestDTO.InvoiceNo = cCRequestPGWDTO.RequestID.ToString();
            netEpayRequestDTO.CmdStatus = "Approve";

            log.LogMethodExit(netEpayRequestDTO);
            return netEpayRequestDTO;
        }

    }
}
