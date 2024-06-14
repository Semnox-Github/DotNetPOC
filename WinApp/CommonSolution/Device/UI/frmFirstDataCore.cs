using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Printing;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Net.Security;
using System.Net;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Instance to frmFirstDataCore Form
    /// </summary>
    public partial class frmFirstDataCore : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        FirstDataTransactionRequest FDTransctionRequest;
        /// <summary>
        /// Instance to FirstDataResponse Class
        /// </summary>
        public FirstDataResponse firstdataResponse = new FirstDataResponse();

        VerifoneHandler VrifoneHndlr;
        Utilities _utilities;
        string CardName = "";
        bool returnStatus = false;
        #region Parafait configurations
        //The following values are coming from configuration
        string TPPID = "";
        string TermID = "";
        string GroupID = "";
        string MerchantID = "";
        string DID = "";
        string ServiceID = "";
        string Version = "";
        //string SRSAPP = "";
        string TrxAPP = "";
        string SRSURL = "";
        string TrxURL = "";
        string ClientTimeOut = "";
        string CurrencyCode = "";
        string TokenType = "";
        string Domain = "";
        string Brand = "";
        string MCC = "";
        #endregion
        ShowMessageDelegate showMessageDelegate;
        bool isPrint = false;
        private enum Alignment
        {
            Left,
            Right,
            Center
        }
        //Firstdata Request objects
        CreditRequestDetails creditRequest = new CreditRequestDetails();
        DebitRequestDetails debitRequest = new DebitRequestDetails();
        VoidTOReversalRequestDetails VoidTorRequest = new VoidTOReversalRequestDetails();
        TARequestDetails taRequestDetails = new TARequestDetails();

        //firstdata Response objects
        DebitResponseDetails DebitResponse = null;
        CreditResponseDetails CreditResponse = null;
        VoidTOReversalResponseDetails VoidTorResponse = null;
        TAResponseDetails taResponseDetails = null;
        RejectResponseDetails RejectResp = null;

        //Schema object
        GMFMessageVariants gmfMsgVar = new GMFMessageVariants();

        #region Firstdata Groups
        //Firstdata groups
        CommonGrp cmngrp = new CommonGrp();
        CardGrp crdGrp = new CardGrp();
        PINGrp pingrp = new PINGrp();
        AddtlAmtGrp addamtGrp = new AddtlAmtGrp();
        OrigAuthGrp orgGrp = new OrigAuthGrp();
        ProdCodeGrp prodcodegp = new ProdCodeGrp();
        ProdCodeDetGrp prdctCodeDetGp = new ProdCodeDetGrp();
        TAGrp taGrp = new TAGrp();
        MCGrp mastCrdGrp = new MCGrp();
        VisaGrp visaGrp = new VisaGrp();
        DSGrp discGrp = new DSGrp();
        AmexGrp amxGrp = new AmexGrp();
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="FDTranRequest"></param>
        /// <param name="Verifone"></param>
        /// <param name="utilities"></param>
        public frmFirstDataCore(FirstDataTransactionRequest FDTranRequest, VerifoneHandler Verifone, Utilities utilities, ShowMessageDelegate showMessageDelegate)
        {
            log.LogMethodEntry(FDTranRequest, Verifone, utilities);

            InitializeComponent();
            FDTransctionRequest = FDTranRequest;
            VrifoneHndlr = Verifone;
            _utilities = utilities;
            this.showMessageDelegate = showMessageDelegate;
            try
            {
                //fetching values from configuration
                TPPID = "RSE015"; //_utilities.getParafaitDefaults("FIRST_DATA_TPPID");
                TermID = _utilities.getParafaitDefaults("FIRST_DATA_TERMINAL_ID");
                GroupID = _utilities.getParafaitDefaults("FIRST_DATA_GROUP_ID");
                MerchantID = _utilities.getParafaitDefaults("FIRST_DATA_MERCHANT_ID");
                DID = _utilities.getParafaitDefaults("FIRST_DATA_DID");
                ServiceID = _utilities.getParafaitDefaults("FIRST_DATA_SERVICE_ID");
                Version = _utilities.getParafaitDefaults("FIRST_DATA_VERSION");
                //SRSAPP = "RAPIDCONNECTSRS"; //_utilities.getParafaitDefaults("FIRST_DATA_SRS_APP");
                TrxAPP = "RAPIDCONNECTSRS"; // _utilities.getParafaitDefaults("FIRST_DATA_TRANSACTION_APP");
                SRSURL = _utilities.getParafaitDefaults("FIRST_DATA_SRS_URL");
                TrxURL = _utilities.getParafaitDefaults("FIRST_DATA_TRANSACTION_URL");
                ClientTimeOut = _utilities.getParafaitDefaults("FIRST_DATA_CLIENT_TIMEOUT");
                CurrencyCode = _utilities.getParafaitDefaults("GATEWAY_CURRENCY_CODE");
                TokenType = _utilities.getParafaitDefaults("FIRST_DATA_TOKEN_TYPE");
                Domain = _utilities.getParafaitDefaults("FIRST_DATA_DOMAIN");
                Brand = _utilities.getParafaitDefaults("FIRST_DATA_BRAND");
                MCC = _utilities.getParafaitDefaults("FIRST_DATA_MCC");
            }
            catch (Exception e)
            {
                log.Error("Error while getting First Data Core", e);
                firstdataResponse.responseMessage = e.ToString();
                this.Dispose();
            }

            log.LogMethodExit(null);
        }
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private void frmFirstDataCore_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            timer.Interval = 2000;
            timer.Tick += timer_Tick;
            timer.Start();

            log.LogMethodExit(null);
        }
        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                timer.Stop();
                returnStatus = DoTransaction(FDTransctionRequest.TransactionType);
                if (returnStatus)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while calculating timer tick", ex);
                firstdataResponse.responseMessage = ex.Message;
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            displayMessage("Transaction Cancelled.");
            Thread.Sleep(3000);
            firstdataResponse.responseMessage = "Transaction Cancelled.";
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            timer.Stop();
            this.Close();

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Do Transaction method
        /// </summary>
        /// <param name="TranType"></param>
        /// <returns></returns>
        public bool DoTransaction(string TranType)
        {
            log.LogMethodEntry(TranType);

            //This  is the main function which is called from firstdata adapter class
            int counter = 3;
            string message = "";
            string Response = "";
            string readCardStatus = "";
            string tranTypeForSelection = "";
            if (TranType.Equals("Sale"))//2017-05-13
            {
                if (string.IsNullOrEmpty(FDTransctionRequest.OrigToken))//2017-05-13
                {
                    readCardStatus = ReadCard();
                    if (!readCardStatus.Equals("00"))
                    {
                        message = readCardStatus;
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                displayMessage("Processing... Please wait...");
                VrifoneHndlr.displayMessages("Processing...", "Please wait...", "", "", 300);
                if (FDTransctionRequest.isCredit && !FirstDataAdapter.IsUnattended && string.IsNullOrEmpty(FDTransctionRequest.OrigTransactionType) && _utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y") && _utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y"))
                {
                    log.Debug("Creditcard auto authorization is enabled");
                    FDTransctionRequest.TransactionType = TranType = "Authorization";
                }
                else
                {
                    if (FDTransctionRequest.isCredit && !FirstDataAdapter.IsUnattended && _utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
                    {
                        if (!string.IsNullOrEmpty(FDTransctionRequest.OrigTransactionType))
                        {
                            tranTypeForSelection = ((FDTransctionRequest.OrigTransactionType.Equals("TATokenRequest")) ? "Authorization" : ((FDTransctionRequest.OrigTransactionType.Equals("Authorization"))) ? "Completion" : "TATokenRequest");
                        }
                        else
                        {
                            tranTypeForSelection = "TATokenRequest";
                        }
                        if (_utilities.getParafaitDefaults("ENABLE_AUTO_CREDITCARD_AUTHORIZATION").Equals("Y") && (tranTypeForSelection.Equals("TATokenRequest") || tranTypeForSelection.Equals("Sale")))
                        {
                            tranTypeForSelection = "Authorization";
                        }
                        else
                        {
                            Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI frmTranType = new Semnox.Parafait.Device.PaymentGateway.frmTransactionTypeUI(_utilities, tranTypeForSelection, FDTransctionRequest.TransactionAmount, showMessageDelegate);
                            if (frmTranType.ShowDialog() != DialogResult.Cancel)
                            {
                                if (!string.IsNullOrEmpty(frmTranType.TransactionType))
                                {
                                    FDTransctionRequest.TransactionType = TranType = frmTranType.TransactionType;
                                    FDTransctionRequest.TipAmount = frmTranType.TipAmount;
                                    if (TranType.Equals("TATokenRequest"))
                                    {
                                        FDTransctionRequest.TransactionAmount = 0;
                                    }
                                }
                            }
                            else
                            {
                                while (!VrifoneHndlr.SendCommand(VerifoneHandler.CANCELL_SESSION_REQUEST_72) && counter > 0)//clear screen command    
                                {
                                    Thread.Sleep(250);
                                    counter--;
                                }
                                throw new Exception("CANCELLED");
                            }
                        }
                    }
                }
            }
            switch (TranType)
            {
                case "Sale":
                    message = Makepayment(ref Response);
                    break;
                case "Return":
                    message = RefundTransaction(ref Response);
                    break;
                case "Void":
                    message = Reversal("Void", FDTransctionRequest.OrigTransactionType, FDTransctionRequest.isCredit, ref Response);
                    break;
                case "Authorization":
                    message = PerformAuthorization(ref Response);
                    break;
                case "Completion":
                    message = CompleteTransaction(ref Response);
                    break;
                case "TATokenRequest":
                    message = PerformTokenRequest(ref Response);
                    break;
                default:
                    message = "Invalid TranType!!...";
                    log.LogMethodExit(false);
                    return false;
            }

            if (!string.IsNullOrEmpty(message))
            {
                string message1 = "";
                message = message.Replace("APPROVAL", "APPROVED")
                                 .Replace("APPRV", "APPROVED")
                                 .Replace("-", " ")
                                 .Replace(".", " ");//the firstdata will return APPROVAL & APPRV these words replacing with APPROVED
                displayMessage(message);
                if (message.Length > 16)
                {//To the verifone device we can send only 16 character max in one line.
                    message1 = message.Substring(16);
                    message = message.Substring(0, 16);
                }
                VrifoneHndlr.displayBankResponse(message, message1);
            }
            //try
            //{
            //    if (isPrint && FDTransctionRequest.isPrintReceipt && !FDTransctionRequest.TransactionType.Equals("Completion"))
            //    {
            //        if (_utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y" || FirstDataAdapter.IsUnattended)
            //        {
            //            print(false);//Reciept printing
            //        }
            //        if (_utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !FirstDataAdapter.IsUnattended)
            //        {
            //            print(true);//Reciept printing
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Print Error!!!..", ex);
            //    displayMessage("Print Error!!!..");
            //}
            Thread.Sleep(3000);            
            while (!VrifoneHndlr.SendCommand(VerifoneHandler.CANCELL_SESSION_REQUEST_72) && counter > 0)//clear screen command    
            {
                Thread.Sleep(250);
                counter--;
            }
            // VrifoneHndlr.SendCommand(VerifoneHandler.CANCELL_SESSION_REQUEST_72);        
            VrifoneHndlr.closeVerifonePort();//Closing the verifone port before exit.            
            bool status = false;
            if (Response.Contains("000") || Response.Contains("002"))//000 is approved and 002 is partialy approved 
            {
                status = true;
            }

            log.LogMethodExit(status);
            return status;
        }

        private void GroupInitialize(bool isCredit, bool isReversal = false)
        {
            log.LogMethodEntry(isCredit, isReversal);

            #region Common Group
            //initializing common values for common group
            cmngrp.PymtTypeSpecified = true;
            cmngrp.TxnTypeSpecified = true;
            cmngrp.LocalDateTime = FDTransctionRequest.LocalDateTime;//Local time
            cmngrp.TrnmsnDateTime = DateTime.ParseExact(FDTransctionRequest.LocalDateTime, "yyyyMMddHHmmss", null).ToUniversalTime().ToString("yyyyMMddHHmmss");//converting time to GMT/UTC
            cmngrp.STAN = FDTransctionRequest.STAN;//This STAN is assigned in poscore for the first time and this is identity field of ccrequestpgw
            if (string.IsNullOrEmpty(cmngrp.STAN))
            {
                cmngrp.STAN = getSTAN();
            }
            cmngrp.RefNum = FDTransctionRequest.ReferenceNo;//This is the trxid from ccrequestpgw with left pading zeros.
            cmngrp.TPPID = TPPID;
            cmngrp.TermID = TermID;
            cmngrp.GroupID = GroupID;
            cmngrp.MerchID = MerchantID;
            //differentiating for manual entry and swiped
            if (!FDTransctionRequest.TransactionType.Equals("TATokenRequest"))
            {
                //if (VrifoneHndlr.customerAttribute.isSwipe || (FDTransctionRequest.wasOrigSwiped && !FDTransctionRequest.TransactionType.Equals("Return") && !FDTransctionRequest.TransactionType.Equals("Authorization") && !FDTransctionRequest.TransactionType.Equals("Sale")) || ((FDTransctionRequest.TransactionType.Equals("Authorization") || FDTransctionRequest.TransactionType.Equals("Sale")) && (VrifoneHndlr.customerAttribute.isSwipe || FDTransctionRequest.wasOrigSwiped) && string.IsNullOrEmpty(FDTransctionRequest.OrigToken)))//&& !FDTransctionRequest.TransactionType.Equals("Completion")
                if (VrifoneHndlr.customerAttribute.isSwipe || (FDTransctionRequest.wasOrigSwiped && (FDTransctionRequest.TransactionType.Equals("Void") || FDTransctionRequest.TransactionType.Equals("Authorization") || FDTransctionRequest.TransactionType.Equals("Completion")) && !FDTransctionRequest.OrigTransactionType.Equals("TATokenRequest")))
                {
                    // swiped
                    cmngrp.POSEntryMode = "901";
                    cmngrp.POSCondCode = POSCondCodeType.Item00;
                }
                else
                {
                    //Manual entry
                    cmngrp.POSEntryMode = "011";
                    cmngrp.POSCondCode = POSCondCodeType.Item71;
                }
                cmngrp.POSCondCodeSpecified = true;
                if (FirstDataAdapter.IsUnattended)//Differentiating pos and Kiosk.
                {
                    cmngrp.TermCatCode = TermCatCodeType.Item06;
                }
                else
                {
                    cmngrp.TermCatCode = TermCatCodeType.Item01;
                }
                cmngrp.MerchCatCode = MCC;
                cmngrp.TermCatCodeSpecified = true;
                cmngrp.TermEntryCapablt = TermEntryCapabltType.Item03;
                cmngrp.TermEntryCapabltSpecified = true;
                cmngrp.TxnAmt = (FDTransctionRequest.TransactionAmount * 100).ToString();//Amount 12.34 need to send as 1234 
                if (FDTransctionRequest.TransactionType.Equals("Completion"))
                {
                    cmngrp.TxnAmt = FDTransctionRequest.OrigTransactionAmount.ToString();
                }
                cmngrp.TxnCrncy = CurrencyCode;//It is a number for $->840 from umf document given by firstdata
                cmngrp.TermLocInd = TermLocIndType.Item0;
                cmngrp.TermLocIndSpecified = true;
                cmngrp.CardCaptCap = CardCaptCapType.Item1;
                cmngrp.CardCaptCapSpecified = true;
                //cmngrp.SettleInd = SettleIndType.Item1;
            }
            #endregion
            #region Card Group
            //if (!FDTransctionRequest.TransactionType.Equals("TATokenRequest"))
            //{
            if (!isReversal)
            {
                CardName = (((FDTransctionRequest.TransactionType.Equals("Authorization") || FDTransctionRequest.TransactionType.Equals("Sale")) && !string.IsNullOrEmpty(FDTransctionRequest.OrigToken)) || FDTransctionRequest.TransactionType.Equals("Completion")) ? FDTransctionRequest.OrigCardName : getCardName(VrifoneHndlr.customerAttribute.CardNumber);
            }
            if (string.IsNullOrEmpty(CardName))
            {
                //cardname is empty iff transaction is reverse type.
                CardName = FDTransctionRequest.OrigCardName;
            }
            switch (CardName)
            {
                //Amex,Diners,Discover,JCB,MasterCard,Visa
                case "Visa":
                    crdGrp.CardType = CardTypeType.Visa;
                    break;
                case "MasterCard":
                    crdGrp.CardType = CardTypeType.MasterCard;
                    break;
                case "Amex":
                    crdGrp.CardType = CardTypeType.Amex;
                    break;
                case "Diners":
                    crdGrp.CardType = CardTypeType.Diners;
                    break;
                case "Discover":
                    crdGrp.CardType = CardTypeType.Discover;
                    break;
                case "JCB":
                    crdGrp.CardType = CardTypeType.JCB;
                    break;
            }

            crdGrp.CardTypeSpecified = true;
            if ((FDTransctionRequest.TransactionType.Equals("Authorization") || FDTransctionRequest.TransactionType.Equals("Sale")) && !string.IsNullOrEmpty(FDTransctionRequest.OrigToken))
            {
                crdGrp.CardExpiryDate = FDTransctionRequest.OrigCardExpiryDate;
            }
            //else if (FDTransctionRequest.TransactionType.Equals("TATokenRequest"))
            //{
            //    crdGrp.CardExpiryDate = VrifoneHndlr.customerAttribute.ExpDate;
            //}
            //}
            #endregion
            if (!FDTransctionRequest.TransactionType.Equals("TATokenRequest"))
            {
                #region Pin Group
                //assigning Pin data 
                pingrp.PINData = VrifoneHndlr.customerAttribute.PinBlock;
                if (!string.IsNullOrEmpty(VrifoneHndlr.customerAttribute.PinSerial))
                {
                    pingrp.KeySerialNumData = VrifoneHndlr.customerAttribute.PinSerial.PadLeft(20, 'F');
                }
                #endregion
                #region Visa Group
                //This is the mandatory field for Visa cards 
                if (!FDTransctionRequest.TransactionType.Equals("Completion"))
                {
                    visaGrp.ACI = ACIType.Y;
                    visaGrp.ACISpecified = true;
                }
                #endregion
                #region Additional Group
                //set comman values
                if (!FDTransctionRequest.TransactionType.Equals("Completion"))
                {
                    addamtGrp.PartAuthrztnApprvlCapablt = PartAuthrztnApprvlCapabltType.Item1;
                    addamtGrp.PartAuthrztnApprvlCapabltSpecified = true;
                }
                if (FDTransctionRequest.TransactionType.Equals("Completion"))
                {
                    addamtGrp.AddAmtType = AddAmtTypeType.FirstAuthAmt;
                    addamtGrp.AddAmt = FDTransctionRequest.OrigTransactionAmount.ToString();
                    addamtGrp.AddAmtTypeSpecified = true;
                    addamtGrp.AddAmtCrncy = CurrencyCode;
                }
                #endregion
                #region original group
                orgGrp.OrigSTAN = FDTransctionRequest.OrigSTAN;
                orgGrp.OrigLocalDateTime = FDTransctionRequest.OrigDatetime;
                orgGrp.OrigTranDateTime = FDTransctionRequest.OrigGMTDatetime;
                if (FDTransctionRequest.TransactionType.Equals("Completion"))
                {
                    orgGrp.OrigRespCode = FDTransctionRequest.OrigResponseCode;
                    orgGrp.OrigAuthID = FDTransctionRequest.OrigAuthID;
                }
                #endregion
            }
            #region TA Group

            taGrp.SctyLvl = SctyLvlType.EncrptTknizatn;
            taGrp.SctyLvlSpecified = true;
            taGrp.TknType = TokenType;
            if (!isReversal && !((FDTransctionRequest.TransactionType.Equals("Authorization") || FDTransctionRequest.TransactionType.Equals("Completion") || FDTransctionRequest.TransactionType.Equals("Sale")) && !string.IsNullOrEmpty(FDTransctionRequest.OrigToken)))
            {
                //if the transaction is not a reversal type then need to send the following details
                taGrp.EncrptType = EncrptTypeType.Verifone;
                taGrp.EncrptTypeSpecified = true;
                if (VrifoneHndlr.customerAttribute.isSwipe)
                {
                    taGrp.EncrptTrgt = EncrptTrgtType.Track2;
                }
                else
                {
                    taGrp.EncrptTrgt = EncrptTrgtType.PAN;
                }
                taGrp.EncrptTrgtSpecified = true;
                taGrp.KeyID = Domain + ";" + Brand;
                taGrp.EncrptBlock = VrifoneHndlr.customerAttribute.Track2data;

            }
            else
            {
                //if the transaction is reversal type thenonly token is enough with SctyLvl and TknType .
                taGrp.Tkn = FDTransctionRequest.OrigToken;
            }

            #endregion

            log.LogMethodExit(null);
        }
        private string getCardName(string cardNumber)
        {
            log.LogMethodExit(cardNumber);

            //Base on the bin range this function returns the card name
            string Visa = "^4[0-9]{12}(?:[0-9]{3})?$";
            string MasterCard = "^5[0-5][0-9]{14}$";
            string Amex = "^3[47][0-9]{13}$";
            string DCI = "^3(?:0[0-5]|[68][0-9])[0-9]{11}$";
            string Discover = "(^6(?:(0|2)(1|2)1|5[0-9]{2})[0-9]{12}$)|^8[0-9]*";
            string JCB = "^(?:2131|1800|35\\d{3})\\d{11}$";
            // Amex,Diners,Discover,JCB,MasterCard,Visa
            Regex regex = new Regex(Visa);
            Match match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Visa");
                return "Visa";
            }
            regex = new Regex(MasterCard);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("MasterCard");
                return "MasterCard";
            }
            regex = new Regex(Amex);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Amex");
                return "Amex";
            }
            regex = new Regex(DCI);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Diners");
                return "Diners";
            }
            regex = new Regex(Discover);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("Discover");
                return "Discover";
            }
            regex = new Regex(JCB);
            match = regex.Match(cardNumber);
            if (match.Success)
            {
                log.LogMethodExit("JCB");
                return "JCB";
            }

            log.LogMethodExit("Unknown");
            return "Unknown";
        }
        private string Reversal(string ReversalType, string TranType, bool isCredit, ref string RespCode)
        {
            log.LogMethodEntry(ReversalType, TranType, isCredit, RespCode);

            //this function will handle void,Timeout,Partial and Timeout of void and partial reversal.
            string xml;
            string clientRef = "";
            string returnString = "";
            int counter = 3;
            object Response;
            AddtlAmtGrp admgrp = new AddtlAmtGrp();
            if (!ReversalType.Equals("Timeout"))
            {
                //Incase of timeout reversal the reversal message is not neccessary to display.
                this.Show();
                displayMessage(ReversalType + " Reversal is processing... Please wait...");
            }

            cmngrp.ReversalIndSpecified = true;
            if (isCredit)//Checking for credit or debit transaction
            {
                cmngrp.PymtType = PymtTypeType.Credit;
            }
            else
            {
                cmngrp.PymtType = PymtTypeType.Debit;
            }
            if (TranType.Equals("Sale"))
            {
                cmngrp.TxnType = TxnTypeType.Sale;
            }
            else if (TranType.Equals("Refund"))
            {
                cmngrp.TxnType = TxnTypeType.Refund;
            }
            else if (TranType.Equals("Authorization"))
            {
                cmngrp.TxnType = TxnTypeType.Authorization;
            }
            else if (TranType.Equals("Completion"))
            {
                cmngrp.TxnType = TxnTypeType.Completion;
            }
            cmngrp.TxnTypeSpecified = true;

            if (ReversalType.Equals("Timeout"))
            {
                isPrint = false;
                GroupInitialize(isCredit);
                orgGrp.OrigSTAN = cmngrp.STAN;
                orgGrp.OrigLocalDateTime = cmngrp.LocalDateTime;
                orgGrp.OrigTranDateTime = cmngrp.TrnmsnDateTime;
                cmngrp.ReversalInd = ReversalIndType.Timeout;//seting reversal Type
                if (TranType.Equals("Completion"))
                {
                    orgGrp.OrigAuthID = null;
                    orgGrp.OrigRespCode = null;
                }

            }
            else
            {
                GroupInitialize(isCredit, true);
                addamtGrp.AddAmt = FDTransctionRequest.OrigTransactionAmount.ToString();
                addamtGrp.AddAmtCrncy = CurrencyCode;
                addamtGrp.AddAmtType = AddAmtTypeType.TotalAuthAmt;
                addamtGrp.AddAmtTypeSpecified = true;
                addamtGrp.PartAuthrztnApprvlCapabltSpecified = false;
                cmngrp.ReversalInd = ReversalIndType.Void;
                VoidTorRequest.AddtlAmtGrp = new AddtlAmtGrp[1];
                VoidTorRequest.AddtlAmtGrp[0] = addamtGrp;
                if (isCredit)
                {
                    VoidTorRequest.Item = setGroupData(crdGrp.CardType.ToString());
                }
                #region original group                
                orgGrp.OrigAuthID = FDTransctionRequest.OrigAuthID;
                orgGrp.OrigRespCode = FDTransctionRequest.OrigResponseCode;
                #endregion
            }
            //if (FDTransctionRequest.wasOrigSwiped)
            //{
            //    crdGrp.CardExpiryDate = FDTransctionRequest.OrigCardExpiryDate;
            //}
            //else
            //{
            crdGrp.CardExpiryDate = null;
            //}
            if (isCredit)
            {
                VoidTorRequest.CardGrp = crdGrp;
            }
            while (counter > 0 && string.IsNullOrEmpty(returnString))
            {
                if (ReversalType.Equals("Timeout"))
                {
                    cmngrp.STAN = getSTAN();//for all the subsequent transactions STAN should be Unique.
                    cmngrp.LocalDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                    cmngrp.TrnmsnDateTime = DateTime.Now.ToUniversalTime().ToString("yyyyMMddHHmmss");
                }
                VoidTorRequest.TAGrp = taGrp;
                VoidTorRequest.OrigAuthGrp = orgGrp;
                VoidTorRequest.CommonGrp = cmngrp;
                gmfMsgVar = new GMFMessageVariants();
                gmfMsgVar.Item = VoidTorRequest;
                xml = GetXMLData();
                clientRef = VoidTorRequest.CommonGrp.STAN + "V" + VoidTorRequest.CommonGrp.TPPID;
                clientRef = clientRef.PadLeft(14, '0');
                Response = ProcessTransaction(xml, clientRef);
                try
                {
                    //This 'Response' object type variable may contain 3 types of values 
                    //i)VoidTOReversalResponseDetails Type 
                    //ii)RejectResponseDetails Type
                    //iii) return codes like 203,204,205 etc
                    VoidTorResponse = (VoidTOReversalResponseDetails)Response;
                }
                catch (Exception ex)
                {
                    log.Error("Error while Void to response", ex);

                    try
                    {
                        RejectResp = (RejectResponseDetails)Response;
                        returnString = "Request Rejected.";
                        isPrint = false;
                    }
                    catch (Exception e)
                    {
                        log.Error("Error while rejecting request", e);
                    }
                }
                if (VoidTorResponse != null)
                {
                    //If recieved Response
                    if (!string.IsNullOrEmpty(VoidTorResponse.RespGrp.RespCode))
                    {
                        if (ReversalType.Equals("Void"))
                        {
                            if (VoidTorResponse.RespGrp.RespCode.Equals("000") || VoidTorResponse.RespGrp.RespCode.Equals("002"))//Partial and full approval
                            {
                                isPrint = true;
                                returnString = VoidTorResponse.RespGrp.AddtlRespData + ".";
                                RespCode = VoidTorResponse.RespGrp.RespCode;
                                CreateResponse("Reversal", 1);
                                _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + VoidTorResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + returnString, ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            }
                            else if (VoidTorResponse.RespGrp.RespCode.Equals("500"))//declined
                            {
                                isPrint = true;
                                returnString = VoidTorResponse.RespGrp.AddtlRespData + ".";
                                RespCode = VoidTorResponse.RespGrp.RespCode;
                                CreateResponse("Reversal", 2);
                                _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + VoidTorResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + returnString, ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            }
                            else if (!string.IsNullOrEmpty(VoidTorResponse.RespGrp.AddtlRespData))//Other status
                            {
                                isPrint = true;
                                returnString = VoidTorResponse.RespGrp.AddtlRespData + ".";
                                RespCode = VoidTorResponse.RespGrp.RespCode;
                                CreateResponse("Reversal", 3);
                                _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + VoidTorResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + returnString, ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            }
                            else if (!string.IsNullOrEmpty(VoidTorResponse.RespGrp.ErrorData))//Error 
                            {
                                returnString = VoidTorResponse.RespGrp.ErrorData + ".";
                                RespCode = VoidTorResponse.RespGrp.RespCode;
                                CreateResponse("Reversal", 3);
                                _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + VoidTorResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + returnString, ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            }
                        }
                        else if (VoidTorResponse.RespGrp.RespCode.Equals("000"))//in case of timeout reversal checking for approval
                        {
                            CreateResponse("Reversal", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + VoidTorResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + returnString, ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            firstdataResponse.responseMessage = "Response Timeout!!!... Please Retry...";
                            returnString = "Response Timeout!!!... Please Retry...";
                        }
                    }
                }
                else if (!ReversalType.Equals("Timeout"))//Timeout Reversal for void and partial
                {
                    if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008"))
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response.ToString() + " Response:", ReversalType + " Reversal", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        returnString = Reversal("Timeout", cmngrp.TxnType.ToString(), isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            returnString = Response.ToString();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while converting Response to string and hence fetching a valid value for returnString", ex);
                        }
                    }
                }


                counter--;
            }
            if (counter == 0)
            {
                isPrint = false;
                returnString = "Response Timeout!!!... Please Retry...";
            }

            log.LogMethodExit(returnString);
            return returnString;
        }
        private void displayMessage(string msg)
        {
            log.LogMethodEntry(msg);

            //display message
            txtStatus.Text = msg;
            txtStatus.Refresh();

            log.LogMethodExit(null);
        }
        private string ReadCard()
        {
            log.LogMethodEntry();

            bool status = false;
            if (VerifoneHandler.portResponse == -1)
            {
                status = VrifoneHndlr.openVerifonePort(VerifoneHandler.verifonePort);
                if (!status)
                {
                    log.LogMethodEntry("Pinpad Communication Error!!!...");
                    return "Pinpad Communication Error!!!...";
                }
            }
            if (VerifoneHandler.portResponse != -1)
            {
                VrifoneHndlr.customerAttribute = new VerifoneHandler.CustomerAttribute();
                if (!FirstDataAdapter.IsUnattended)
                {
                    string response = "";
                    displayMessage("Select Transaction Type.");
                    response = VrifoneHndlr.UserOptions("Select", "Debit", "TransactionType", "Credit");
                    if (string.IsNullOrEmpty(response))
                    {
                        log.LogMethodEntry("Invalid Option!..");
                        return "Invalid Option!..";
                    }
                    else
                    {
                        if (response.Substring(1, 2).Equals("10"))
                        {
                            //if (FDTransctionRequest.TransactionType.Equals("TATokenRequest")||FDTransctionRequest.TransactionType.Equals("Authorization"))//2017-05-13
                            //{
                            //    return "Pre-Authorization or Authorization is not allowed for Debit cards.";
                            //}
                            //else
                            //{
                            FDTransctionRequest.isCredit = false;
                            //}
                        }
                        else if (response.Substring(1, 2).Equals("11"))
                        { FDTransctionRequest.isCredit = true; }
                        else if (response.Substring(1, 2).Equals("03"))
                        {
                            log.LogMethodEntry("Transaction Type Selection Cancelled.");
                            return "Transaction Type Selection Cancelled.";
                        }
                        else if (response.Substring(1, 2).Equals("02"))
                        {
                            log.LogMethodEntry("Transaction Type Selection Timeout.");
                            return "Transaction Type Selection Timeout.";
                        }
                        else
                        {
                            log.LogMethodEntry("Invalid Option!..");
                            return "Invalid Option!..";
                        }
                    }

                }
                if (FDTransctionRequest.isCredit)
                {
                    displayMessage("Please Swipe your card Or Enter the card details on the pin pad.");
                    status = VrifoneHndlr.ReadCardEntryorSwipe(60);//swipe or manual entry command with delay 60 sec   
                }
                else
                {
                    displayMessage("Please Swipe your card.");
                    status = VrifoneHndlr.ReadCard(FDTransctionRequest.TransactionAmount * 100, 60);
                }
                if (VrifoneHndlr.customerAttribute.statusCode.Equals("00") && status)
                {
                    if (!string.IsNullOrEmpty(VrifoneHndlr.customerAttribute.CardNumber) && VrifoneHndlr.customerAttribute.CardNumber.Length >= 14)//card no should be greated than 8 digit while passing to pin request.
                    {
                        if (!FDTransctionRequest.isCredit)
                        {
                            displayMessage("Please enter pin for Debit and press enter.");
                            status = VrifoneHndlr.PinRequest(VrifoneHndlr.customerAttribute.CardNumber, false, 4, 12, "Enter PIN", "", 60);//pin request
                            if (status)
                            {
                                if (!string.IsNullOrEmpty(VrifoneHndlr.customerAttribute.PinBlock) && !VrifoneHndlr.customerAttribute.isSwipe)//Checking for is debit
                                {
                                    VrifoneHndlr.customerAttribute.CardNumber = null;
                                    displayMessage("Manual Entry not allowed for debit card... Please swipe again...");
                                    status = VrifoneHndlr.ReadCard(FDTransctionRequest.TransactionAmount * 100, 60);
                                    if (status)
                                    {
                                        if (VrifoneHndlr.customerAttribute.statusCode.Equals("00"))
                                        {
                                            try
                                            {
                                                if (VrifoneHndlr.customerAttribute.CardNumber.Length < 8)
                                                {
                                                    VrifoneHndlr.customerAttribute.CardNumber = null;

                                                    log.LogMethodExit("Read card Error!!!...Try again...");
                                                    return "Read card Error!!!...Try again...";
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                log.Error("Card Number length is less than zero", ex);

                                                log.LogMethodExit("Read card Error!!!...Try again...");
                                                return "Read card Error!!!...Try again...";
                                            }
                                        }
                                        else if (VrifoneHndlr.customerAttribute.statusCode.Equals("01"))
                                        {
                                            isPrint = false;

                                            log.LogMethodExit("Read card Unsuccessful!.. Try again...");
                                            return "Read card Unsuccessful!.. Try again...";
                                        }
                                        else if (VrifoneHndlr.customerAttribute.statusCode.Equals("02"))
                                        {
                                            isPrint = false;

                                            log.LogMethodExit("Read card Timeout occurred!.. Try again...");
                                            return "Read card Timeout occurred!.. Try again...";
                                        }
                                        else if (VrifoneHndlr.customerAttribute.statusCode.Equals("03"))
                                        {
                                            isPrint = false;

                                            log.LogMethodExit("Read card Cancelled.");
                                            return "Read card Cancelled.";
                                        }
                                    }
                                    else
                                    {
                                        log.LogMethodExit("Pinpad Communication Error!!!...");
                                        return "Pinpad Communication Error!!!...";
                                    }
                                }
                            }
                            else
                            {
                                if (VrifoneHndlr.customerAttribute.statusCode.Equals("01"))
                                {
                                    log.LogMethodExit("Pin Entry Cancelled.");
                                    return "Pin Entry Cancelled.";
                                }
                                else if (VrifoneHndlr.customerAttribute.statusCode.Equals("02"))
                                {
                                    log.LogMethodExit("Pin Entry Timeout!!!...");
                                    return "Pin Entry Timeout!!!...";
                                }
                                else
                                {
                                    log.LogMethodExit("Pinpad Communication Error!!!...");
                                    return "Pinpad Communication Error!!!...";
                                }
                            }
                        }
                        displayMessage("Processing... Please wait...");
                    }
                    else
                    {
                        isPrint = false;

                        log.LogMethodExit("Read card error!.. Try again...");
                        return "Read card error!.. Try again...";
                    }
                }
                else if (VrifoneHndlr.customerAttribute.statusCode.Equals("01"))
                {
                    isPrint = false;

                    log.LogMethodExit("Read card Unsuccessful!.. Try again...");
                    return "Read card Unsuccessful!.. Try again...";
                }
                else if (VrifoneHndlr.customerAttribute.statusCode.Equals("02"))
                {
                    isPrint = false;

                    log.LogMethodExit("Read card Timeout occurred!.. Try again...");
                    return "Read card Timeout occurred!.. Try again...";
                }
                else if (VrifoneHndlr.customerAttribute.statusCode.Equals("03"))
                {
                    isPrint = false;

                    log.LogMethodExit("Read card Cancelled.");
                    return "Read card Cancelled.";
                }
                else
                {
                    log.LogMethodExit("Pinpad Communication Error!!!...");
                    return "Pinpad Communication Error!!!...";
                }
                if (string.IsNullOrEmpty(VrifoneHndlr.customerAttribute.PinData))
                {
                    FDTransctionRequest.isCredit = true;
                }
                else
                {
                    FDTransctionRequest.isCredit = false;
                }
            }
            else
            {
                isPrint = false;

                log.LogMethodExit(",Verifone COM Port is not opened!!!...");
                return ",Verifone COM Port is not opened!!!...";
            }

            log.LogMethodExit("00");
            return "00";
        }
        private string Makepayment(ref string RespCode)
        {
            log.LogMethodEntry(RespCode);

            string responseString = "";
            string xml;
            object Response;
            string clientRef = string.Empty;
            //string status;//2017-05-13
            creditRequest = new CreditRequestDetails();
            debitRequest = new DebitRequestDetails();
            //if (string.IsNullOrEmpty(FDTransctionRequest.OrigToken))//2017-05-13
            //{
            //    status = ReadCard();
            //    if (!status.Equals("00"))
            //    {
            //        return status;
            //    }
            //}


            try
            {
                GroupInitialize(FDTransctionRequest.isCredit);
                cmngrp.TxnType = TxnTypeType.Sale;
                if (FDTransctionRequest.isCredit)
                {
                    creditRequest.TAGrp = taGrp;
                    creditRequest.CardGrp = crdGrp;
                    creditRequest.AddtlAmtGrp = new AddtlAmtGrp[1];
                    creditRequest.AddtlAmtGrp[0] = addamtGrp;
                    if (CardName.Equals("Visa"))
                    {
                        creditRequest.Item = visaGrp;
                    }
                    cmngrp.PymtType = PymtTypeType.Credit;
                    creditRequest.CommonGrp = cmngrp;
                    gmfMsgVar.Item = creditRequest;
                    creditRequest = new CreditRequestDetails();
                    creditRequest = gmfMsgVar.Item as CreditRequestDetails;
                }
                else
                {
                    debitRequest.TAGrp = taGrp;
                    cmngrp.PymtType = PymtTypeType.Debit;
                    debitRequest.CommonGrp = cmngrp;
                    debitRequest.PINGrp = pingrp;
                    debitRequest.AddtlAmtGrp = new AddtlAmtGrp[1];
                    debitRequest.AddtlAmtGrp[0] = addamtGrp;
                    gmfMsgVar.Item = debitRequest;
                    debitRequest = new DebitRequestDetails();
                    debitRequest = gmfMsgVar.Item as DebitRequestDetails;
                }
                clientRef = cmngrp.STAN + "V" + cmngrp.TPPID;//Should be unique for each Transaction
                clientRef = clientRef.PadLeft(14, '0');
                xml = GetXMLData();
                Response = ProcessTransaction(xml, clientRef);
                if (FDTransctionRequest.isCredit)
                {
                    try
                    {
                        CreditResponse = (CreditResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error when fetching a valid value for CreditResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e)
                        {
                            log.Error("Error when rejecting the request", e);
                        }
                    }
                    if (CreditResponse != null)
                    {
                        if (CreditResponse.RespGrp.RespCode.Equals("000") || CreditResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (CreditResponse.RespGrp.RespCode.Equals("500"))//declined
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            CreateResponse("Credit", 3);
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.ErrorData))//error in transaction
                        {
                            CreateResponse("Credit", 3);
                            responseString = CreditResponse.RespGrp.ErrorData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Sale", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while converting Response to string and hence fetching a valid value for returnString", ex);
                        }
                    }
                }
                else
                {
                    try
                    {
                        DebitResponse = (DebitResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error when fetching a valid value for DebitResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e)
                        {
                            log.Error("Error while rejecting the request", e);
                        }
                    }

                    if (DebitResponse != null)
                    {
                        if (DebitResponse.RespGrp.RespCode.Equals("000") || DebitResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (DebitResponse.RespGrp.RespCode.Equals("500"))//Declined
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.ErrorData))//Error
                        {
                            responseString = DebitResponse.RespGrp.ErrorData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Sale", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while converting Response to string and hence fetching a valid value for returnString", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Processing...", ex);
                isPrint = false;
                responseString = "Error in Processing...";
            }

            log.LogVariableState("RespCode", RespCode);
            log.LogMethodExit(responseString);
            return responseString;
        }
        private string RefundTransaction(ref string RespCode)
        {
            log.LogMethodEntry(RespCode);

            string responseString = "";
            string clientRef = "";
            string xml = "";
            object Response = null;
            string status;
            creditRequest = new CreditRequestDetails();
            debitRequest = new DebitRequestDetails();

            status = ReadCard();
            if (!status.Equals("00"))
            {
                log.LogVariableState("RespCode", RespCode);
                log.LogMethodExit(status);
                return status;
            }

            VrifoneHndlr.displayMessages("Processing...", "Please wait...", "", "", 300);
            try
            {
                GroupInitialize(FDTransctionRequest.isCredit);
                cmngrp.TxnType = TxnTypeType.Refund;
                if (FDTransctionRequest.isCredit) //is credit card
                {
                    creditRequest.TAGrp = taGrp;
                    creditRequest.CardGrp = crdGrp;
                    cmngrp.PymtType = PymtTypeType.Credit;
                    creditRequest.CommonGrp = cmngrp;
                    gmfMsgVar.Item = creditRequest;
                    creditRequest = new CreditRequestDetails();
                    creditRequest = gmfMsgVar.Item as CreditRequestDetails;
                }
                else//is debitcard
                {
                    debitRequest.TAGrp = taGrp;
                    cmngrp.PymtType = PymtTypeType.Debit;
                    debitRequest.CommonGrp = cmngrp;
                    debitRequest.PINGrp = pingrp;
                    gmfMsgVar.Item = debitRequest;
                    debitRequest = new DebitRequestDetails();
                    debitRequest = gmfMsgVar.Item as DebitRequestDetails;

                }
                clientRef = cmngrp.STAN + "V" + cmngrp.TPPID;
                clientRef = clientRef.PadLeft(14, '0');
                xml = GetXMLData();
                Response = ProcessTransaction(xml, clientRef);
                if (FDTransctionRequest.isCredit)
                {
                    try
                    {
                        CreditResponse = (CreditResponseDetails)Response;
                    }//in case of timeout the response is just a return code
                    catch (Exception e1)
                    {
                        log.Error("Error while calculating a valid value for CreditResponse", e1);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e2)
                        {
                            log.Error("Error while rejecting the reuest", e2);
                        }
                    }
                    if (CreditResponse != null)
                    {
                        if (CreditResponse.RespGrp.RespCode.Equals("000") || CreditResponse.RespGrp.RespCode.Equals("002"))//Full and partial approval
                        {
                            CreateResponse("Credit", 1);
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            isPrint = true;
                        }
                        else if (CreditResponse.RespGrp.RespCode.Equals("500"))//Declined
                        {
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            isPrint = true;
                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.AddtlRespData))//Other status
                        {
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                            isPrint = true;
                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.ErrorData))//error
                        {
                            responseString = CreditResponse.RespGrp.ErrorData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Timeout
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response.ToString() + " Response:", "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Refund", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while fetching a valid value for responseString", e1);
                        }
                    }
                }
                else
                {
                    try
                    {
                        DebitResponse = (DebitResponseDetails)Response;
                    }//in case of timeout the response is just a return code
                    catch (Exception ex)
                    {
                        log.Error("Error while fetching a valid value for DebitResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e)
                        {
                            log.Error("Error while Rejecting the request", e);
                        }
                    }
                    if (DebitResponse != null)
                    {
                        if (DebitResponse.RespGrp.RespCode.Equals("000") || DebitResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            CreateResponse("Debit", 1);
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (DebitResponse.RespGrp.RespCode.Equals("500"))//Declined
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.ErrorData))//Error data
                        {
                            responseString = DebitResponse.RespGrp.ErrorData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response.ToString() + " Response:", "Refund", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Refund", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Unable to get a valid value for responseString", e1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Processing...", ex);

                isPrint = false;
                responseString = "Error in Processing...";
            }

            log.LogVariableState("RespCode", RespCode);
            log.LogMethodExit(responseString);
            return responseString;
        }
        private object ProcessTransaction(string gmfRequest, string clientRef)
        {
            log.LogMethodEntry(gmfRequest, clientRef);


            Parafait.Device.FirstDataTrxn.StatusType stType = null; ;
            try
            {
                /* Create the instance of the  RequestType that is a class generated from the Rapid connect Transaction 
                 * Service WSDL file [rc.wsdl]*/

                Parafait.Device.FirstDataTrxn.RequestType requestType = new Parafait.Device.FirstDataTrxn.RequestType();
                /* Set Client timeout*/
                requestType.ClientTimeout = ClientTimeOut;

                /* Create the instance of the RequestType that is a class generated from the Rapid connect Transaction 
                 * Service WSDL file [rc.wsdl]*/
                Parafait.Device.FirstDataTrxn.ReqClientIDType reqClientIDType = new Parafait.Device.FirstDataTrxn.ReqClientIDType();
                /* Set App value*/
                reqClientIDType.App = TrxAPP;
                /* Set Auth value*/
                reqClientIDType.Auth = GroupID + MerchantID + "|" + TermID.PadLeft(8, '0');
                /* Set clientRef value*/
                reqClientIDType.ClientRef = clientRef;
                /* Set DID value*/
                reqClientIDType.DID = DID;

                /* Set requestclienttype ojbect to request type*/
                requestType.ReqClientID = reqClientIDType;
                /* Create the instance of the TransactionType that is a class generated from the Rapid connect Transaction 
                 * Service WSDL file [rc.wsdl]*/
                Parafait.Device.FirstDataTrxn.TransactionType transactionType = new Parafait.Device.FirstDataTrxn.TransactionType();

                /* Create the instance of the PayloadType that is a class generated from the Rapid connect Transaction 
                 * Service WSDL file [rc.wsdl]*/
                Parafait.Device.FirstDataTrxn.PayloadType payloadType = new Parafait.Device.FirstDataTrxn.PayloadType();
                /* Set pay load data*/
                payloadType.Encoding = Parafait.Device.FirstDataTrxn.PayloadTypeEncoding.xml_escape;

                /* Set pay load type as the actual XML request*/
                payloadType.Value = gmfRequest; //Set payload - actual xml request
                                                /*set pay load of the transaction type object */
                transactionType.Payload = payloadType;
                /* Set Service ID of the tranasction type object*/
                transactionType.ServiceID = ServiceID;
                /* Set version of the request type object */
                requestType.Version = Version;
                requestType.Item = transactionType;
                //BasicHttpBinding binding = new BasicHttpBinding();
                //binding.OpenTimeout = new TimeSpan(0, 0, 65);
                ////binding.ReceiveTimeout = new TimeSpan(0, 0, 65);
                //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
                //binding.Security.Mode = BasicHttpSecurityMode.Transport;                
                //EndpointAddress address = new EndpointAddress(TrxURL);//url comes from parafait default

                X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2 cert = null;
                //finding certificate in the store
                X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, "VeriSign Class 3 Public Primary Certification Authority - G5", false);
                if (certs.Count == 1)
                {
                    cert = certs[0];
                }
                //FirstDataTrxn.rcPortTypeClient rcClentPortType = new rcPortTypeClient(binding, address);                
                //rcClentPortType.ClientCredentials.ClientCertificate.Certificate = cert;
                Parafait.Device.FirstDataTrxn.rcService service = new Parafait.Device.FirstDataTrxn.rcService();
                service.Url = TrxURL;
                service.Timeout = 65000;
                service.UserAgent = "Parafait v1.5.0.0";
                service.ClientCertificates.Add(cert);
                //Perform certificate validation by performing call back. 
                System.Net.ServicePointManager.ServerCertificateValidationCallback = RemoteServerCertificateValidationCallback;
                Parafait.Device.FirstDataTrxn.ResponseType rspGrp = service.rcTransaction(requestType);//sending request to the first data
                stType = (Parafait.Device.FirstDataTrxn.StatusType)rspGrp.Status;
                Parafait.Device.FirstDataTrxn.TransactionResponseType tRt = (Parafait.Device.FirstDataTrxn.TransactionResponseType)rspGrp.Item;
                string ReturnCode = tRt.ReturnCode; //This should be "000" for successful transaction                
                string StatusCode = stType.StatusCode;//This should be "OK" for sucessful transaction   
                string xml = tRt.Payload.Value;
                object response = null;
                if (string.IsNullOrEmpty(xml))
                {
                    response = (object)ReturnCode;
                    Thread.Sleep(35000);//in case of timeout before requesting time out need to wait 35 sec
                }
                else
                {
                    getResponse(xml, ref response);
                }

                log.LogMethodExit(response);
                return response;
            }
            catch (WebException ex)
            {
                log.Error("Caught a WebException - Error occured while Processing the transaction", ex);
                displayMessage("Error: " + ex.Message);
                firstdataResponse.responseMessage = ex.Message;

                log.LogMethodExit(null, "Throwing Exception");
                throw ex;
            }
            catch (Exception e)
            {
                log.Error("Error occured while Processing the transaction", e);

                if (stType == null)
                {
                    displayMessage("Transaction request Failed!!!...");
                    firstdataResponse.responseMessage = "Transaction request Failed!!!...";
                    _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", e.ToString(), "Transaction Request", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                }
                else
                {
                    displayMessage("Status Code:" + stType.StatusCode + "  StatusValue:" + stType.Value);
                    firstdataResponse.responseMessage = "Status Code:" + stType.StatusCode + "  StatusValue:" + stType.Value;
                    _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "Status Code:" + stType.StatusCode + " StatusValue:" + stType.Value, "Transaction Request", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                }
                // return firstdataResponse.responseMessage;

                log.LogMethodExit(null, "Throwing Exception" + e);
                throw e;
            }
        }


        /// <summary>
        /// Method to check for ssL errors. In case of CertificateNameMismatch, throw error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        public static bool RemoteServerCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            log.LogMethodEntry(sender, certificate, chain, sslPolicyErrors);

            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                log.LogMethodExit(true);
                return true;
            }

            // if got an cert auth error
            if (sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        void print(bool pMerchantReceipt)
        {
            log.LogMethodEntry(pMerchantReceipt);

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);
            pd.PrintPage += (sender, e) =>
            {
                printReceiptText(e, pMerchantReceipt);
            };
            //pd.Print();

            log.LogMethodExit(null);
        }
        private void printReceiptText(System.Drawing.Printing.PrintPageEventArgs e, bool pMerchantReceipt = true)
        {
            log.LogMethodEntry(e, pMerchantReceipt);
            try
            {
                StringFormat sfCenter = new StringFormat();
                sfCenter.Alignment = StringAlignment.Center;

                StringFormat sfRight = new StringFormat();
                sfRight.Alignment = StringAlignment.Far;


                string FieldValue = "";
                int x = 10;
                int y = 10;
                Font f = new Font("Arial", 9);
                Graphics g = e.Graphics;
                int yinc = (int)g.MeasureString("SITE", f).Height;
                int pageWidth = e.PageBounds.Width - x * 2;
                firstdataResponse.ReceiptText += Environment.NewLine + Environment.NewLine;
                FieldValue = _utilities.ParafaitEnv.SiteName;
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                FieldValue = _utilities.ParafaitEnv.SiteAddress;
                if (!string.IsNullOrEmpty(FieldValue))
                {
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                    y += yinc * 3;
                }

                FieldValue = "";
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "Date      : " + DateTime.Now.ToString("MM-dd-yyyy H:mm:ss tt");
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "MID       : " + MerchantID;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "TID       : " + TermID;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                FieldValue = "Terminal  : " + _utilities.ParafaitEnv.POSMachine;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;


                FieldValue = "Invoice No: " + firstdataResponse.ReferenceNo;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;

                string cardId = "";
                cardId = firstdataResponse.CardNo;

                if (!string.IsNullOrEmpty(cardId))
                    cardId = new String('X', 12) + cardId.Substring(12);

                FieldValue = "Account   : " + cardId;
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                y += yinc;
                if (FDTransctionRequest.isCredit)
                {
                    FieldValue = "Card Type : " + CardName;
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                    g.DrawString(FieldValue, f, Brushes.Black, x, y);
                    y += yinc;
                }
                if (VrifoneHndlr.customerAttribute.isSwipe || FDTransctionRequest.wasOrigSwiped)
                    FieldValue = "Entry     : SWIPE";
                else
                    FieldValue = "Entry     : KEYED";
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                y += yinc;


                if (FDTransctionRequest.TransactionType.Equals("Return") && FDTransctionRequest.isCredit)
                {
                    if (!string.IsNullOrEmpty(FDTransctionRequest.OrigDatetime))
                    {
                        FieldValue = "Original Transaction Date:";
                        g.DrawString(FieldValue, f, Brushes.Black, x, y);
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                        y += yinc;
                        FieldValue = DateTime.ParseExact(FDTransctionRequest.OrigDatetime, "yyyyMMddHHmmss", null).ToString("dd/MM/yyyy");
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                        g.DrawString(FieldValue, f, Brushes.Black, x, y);
                        y += yinc;
                        FieldValue = "Return Reason:Transaction Aborted.";//This is Hardcoded in case of Kiosk.
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                        g.DrawString(FieldValue, f, Brushes.Black, x, y);
                        y += yinc;
                    }
                }
                if (!FDTransctionRequest.isCredit)
                    FieldValue = "DEBIT Purchase: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + FDTransctionRequest.TransactionAmount.ToString("0.00");
                else
                    FieldValue = "CREDIT Purchase: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + FDTransctionRequest.TransactionAmount.ToString("0.00");
                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                y += yinc;

                if (firstdataResponse.responseCode == "000" || firstdataResponse.responseCode == "002")
                {
                    FieldValue = "Approval Code : " + firstdataResponse.responseAuthId;
                    g.DrawString(FieldValue, f, Brushes.Black, x, y);
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                    y += yinc * 2;
                    FieldValue = "Status    : ** " + firstdataResponse.responseMessage + " **";
                }
                else if (firstdataResponse.responseCode == "500")
                    FieldValue = "Approval Code : " + "DECLINED";
                else
                    FieldValue = "Approval Code : " + firstdataResponse.responseCode + "/" + firstdataResponse.responseMessage;

                g.DrawString(FieldValue, f, Brushes.Black, x, y);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                y += yinc * 2;


                FieldValue = " **  " + FDTransctionRequest.TransactionType.Replace("Return", "Refund") + "  **";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc;
                if (firstdataResponse.responseCode == "000" || firstdataResponse.responseCode == "002")
                {
                    FieldValue = "AMOUNT: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + firstdataResponse.TransAmount.ToString("0.00");
                }
                else
                {
                    FieldValue = "AMOUNT: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + "0.00";
                }
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                y += yinc * 2;

                if (FDTransctionRequest.TransactionType.Equals("Authorization") && FDTransctionRequest.isCredit)
                {
                    FieldValue = "Tip   : ___________________";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                    y += yinc * 2;

                    FieldValue = "Total : ___________________";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                    y += yinc * 2;
                }

                if (!pMerchantReceipt && FDTransctionRequest.isCredit)
                {
                    FieldValue = _utilities.MessageUtils.getMessage(1175);//Retain this copy for statement validation
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3));
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                    y += yinc * 4;
                }
                else
                {
                    if ((FDTransctionRequest.TransactionType.Equals("Sale") || FDTransctionRequest.TransactionType.Equals("Authorization")) && FDTransctionRequest.isCredit)
                    {
                        FieldValue = _utilities.MessageUtils.getMessage(1176) + _utilities.MessageUtils.getMessage(1180);
                        g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 7));
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine;
                        y += yinc * 8;
                    }
                    else if ((FDTransctionRequest.TransactionType.Replace("Return", "Refund").Equals("Refund") || FDTransctionRequest.TransactionType.Equals("Void")) && FDTransctionRequest.isCredit)
                    {
                        FieldValue = _utilities.MessageUtils.getMessage(1177) + _utilities.MessageUtils.getMessage(1179);
                        g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 10));
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Left) + Environment.NewLine + Environment.NewLine;
                        y += yinc * 10;

                        FieldValue = "Merchant Signature";
                        g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine + Environment.NewLine;
                        y += yinc * 3;

                        FieldValue = "___________________________";
                        g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                        firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine + Environment.NewLine;
                        y += yinc * 2;
                    }
                }

                if (pMerchantReceipt && FDTransctionRequest.isCredit)
                {
                    FieldValue = "Cardholder Signature";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine + Environment.NewLine;
                    y += yinc * 3;

                    FieldValue = "___________________________";
                    g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                    firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine + Environment.NewLine;
                    y += yinc * 2;
                }
                FieldValue = "THANK YOU";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;
                y += yinc;

                if (pMerchantReceipt)
                    FieldValue = "* MERCHANT RECEIPT *";
                else
                    FieldValue = "* CUSTOMER RECEIPT *";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                firstdataResponse.ReceiptText += AllignText(FieldValue, Alignment.Center) + Environment.NewLine;

                log.LogMethodExit(null);
            }
            catch(Exception ex)
            {
                log.Error(ex.ToString());
            }
        }
        private string AllignText(string text, Alignment align)
        {
            log.LogMethodEntry(text, align);

            int pageWidth = 40;
            string res;
            if (align.Equals(Alignment.Right))
            {
                string returnValueNew = text.PadLeft(pageWidth, ' ');
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else if (align.Equals(Alignment.Center))
            {
                int len = (pageWidth - text.Length);
                int len2 = len / 2;
                len2 = len2 + text.Length;
                res = text.PadLeft(len2);
                if (res.Length > 40 && res.Length > text.Length)
                {
                    res = res.Substring(res.Length - 40);
                }

                log.LogMethodExit(res);
                return res;
            }
            else
            {
                //res= text.PadLeft(5 + text.Length);         
                log.LogMethodExit(text);
                return text;
            }
        }
        private void getResponse(string xml, ref object respnse)
        {
            log.LogMethodEntry(xml, respnse);

            //this function deserialize the result xml
            GMFMessageVariants gmf = new GMFMessageVariants();
            var reader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(GMFMessageVariants));
            gmf = (GMFMessageVariants)serializer.Deserialize(reader);
            respnse = (object)gmf.Item;

            log.LogMethodExit(null);
        }
        private String GetXMLData()
        {
            log.LogMethodEntry();

            //serializing xml
            string xmlString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(gmfMsgVar.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, System.Text.Encoding.UTF8);
            xs.Serialize(xmlTextWriter, gmfMsgVar);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            xmlString = encoding.GetString(memoryStream.ToArray());
            xmlString = xmlString.Substring(1, xmlString.Length - 1);

            log.LogMethodExit(xmlString);
            return xmlString;
        }
        private string getSTAN()
        {
            log.LogMethodEntry();

            //Getting new stan
            string STAN = "";
            DataTable dTable;
            int nCounter = 10;
            while (string.IsNullOrEmpty(STAN) && nCounter != 0)
            {
                try
                {
                    dTable = _utilities.executeDataTable(@"insert into CCRequestPGW 
                                               (InvoiceNo, RequestDateTime, POSAmount, TransactionType, StatusId, MerchantId)
                                        values (@TrxId, getdate(), @Amount, @Type, 7, @MerchantId) select @@identity",
                                             new SqlParameter("@TrxId", FDTransctionRequest.ReferenceNo),
                                             new SqlParameter("@Amount", FDTransctionRequest.TransactionAmount.ToString("0.00")),
                                             new SqlParameter("@Type", FDTransctionRequest.TransactionType),
                                             new SqlParameter("@MerchantId", _utilities.ParafaitEnv.POSMachine));

                    log.LogVariableState("@TrxId", FDTransctionRequest.ReferenceNo);
                    log.LogVariableState("@Amount", FDTransctionRequest.TransactionAmount.ToString("0.00"));
                    log.LogVariableState("@Type", FDTransctionRequest.TransactionType);
                    log.LogVariableState("@MerchantId", _utilities.ParafaitEnv.POSMachine);


                    if (dTable.Rows.Count > 0)
                    {
                        STAN = dTable.Rows[0][0].ToString();
                        STAN = STAN.PadLeft(6, '0');
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while inserting values into CCRequestPGW", ex);
                    STAN = "";
                }
                nCounter--;
            }
            if (nCounter == 0)
            {
                txtStatus.Text = "STAN Error!...";
                Thread.Sleep(3000);
                this.Close();
            }

            log.LogMethodExit(STAN);
            return STAN;
        }
        private bool CreateResponse(string Type, int statusID)
        {
            log.LogMethodEntry(Type, statusID);

            //adding response to CCTransactionsPGW
            DataTable dTable = new DataTable();
            string TokenID = "";
            string CardType = "";
            string AuthCode = "";
            string TransDatetime = "";
            string RtnCode = "";
            string TextResponse = "";
            string Stan = "";
            string AccNo = "";
            string TranAmt = "";
            string tranType = "";
            string CardExpiryDate = "";
            string isSwiped = "0";
            string respone = "";
            string tipAmount = "";
            try
            {
                if (Type.Equals("Credit") || Type.Equals("Completion"))
                {
                    if (CreditResponse.TAGrp != null)
                    {
                        TokenID = CreditResponse.TAGrp.Tkn;
                        if (!string.IsNullOrEmpty(CreditResponse.TAGrp.TAExpDate))
                        { CardExpiryDate = CreditResponse.TAGrp.TAExpDate; }
                    }
                    else
                    {
                        TokenID = "";
                        CardExpiryDate = "";
                    }
                    CardType = "Credit";
                    AuthCode = CreditResponse.RespGrp.AuthID;
                    TransDatetime = CreditResponse.CommonGrp.LocalDateTime;
                    RtnCode = CreditResponse.RespGrp.RespCode;
                    TextResponse = CreditResponse.RespGrp.AddtlRespData.Replace("APPROVAL", "APPROVED").Replace("APPRV", "APPROVED");
                    if (string.IsNullOrEmpty(TextResponse))
                    {
                        TextResponse = CreditResponse.RespGrp.ErrorData;
                    }
                    Stan = CreditResponse.CommonGrp.STAN;
                    TranAmt = CreditResponse.CommonGrp.TxnAmt;
                    tranType = CreditResponse.CommonGrp.TxnType.ToString();
                    AccNo = VrifoneHndlr.customerAttribute.CardNumber;
                    tipAmount = (FDTransctionRequest.TipAmount * 100).ToString();
                }
                else if (Type.Equals("Debit") || Type.Equals("Completion"))
                {
                    TokenID = DebitResponse.TAGrp.Tkn;
                    if (DebitResponse.TAGrp != null)
                    {
                        if (!string.IsNullOrEmpty(DebitResponse.TAGrp.TAExpDate))
                        {
                            CardExpiryDate = DebitResponse.TAGrp.TAExpDate;
                        }
                    }
                    else
                    {
                        TokenID = "";
                        CardExpiryDate = "";
                    }
                    CardType = "Debit";
                    AuthCode = DebitResponse.RespGrp.AuthID;
                    TransDatetime = DebitResponse.CommonGrp.LocalDateTime;
                    RtnCode = DebitResponse.RespGrp.RespCode;
                    TextResponse = DebitResponse.RespGrp.AddtlRespData.Replace("APPROVAL", "APPROVED").Replace("APPRV", "APPROVED");
                    if (string.IsNullOrEmpty(TextResponse))
                    {
                        TextResponse = DebitResponse.RespGrp.ErrorData;
                    }
                    TranAmt = DebitResponse.CommonGrp.TxnAmt;
                    Stan = DebitResponse.CommonGrp.STAN;
                    tranType = DebitResponse.CommonGrp.TxnType.ToString();
                    AccNo = VrifoneHndlr.customerAttribute.CardNumber;
                    tipAmount = (FDTransctionRequest.TipAmount * 100).ToString();
                }
                else if (Type.Equals("Reversal"))
                {
                    TokenID = VoidTorResponse.TAGrp.Tkn;

                    if (!string.IsNullOrEmpty(VoidTorResponse.TAGrp.TAExpDate))
                    {
                        CardExpiryDate = VoidTorResponse.TAGrp.TAExpDate;
                    }

                    CardType = VoidTorResponse.CommonGrp.PymtType.ToString(); ;
                    AuthCode = VoidTorResponse.RespGrp.AuthID;
                    TransDatetime = VoidTorResponse.CommonGrp.LocalDateTime;
                    RtnCode = VoidTorResponse.RespGrp.RespCode;
                    TextResponse = VoidTorResponse.RespGrp.AddtlRespData.Replace("APPROVAL", "APPROVED").Replace("APPRV", "APPROVED");
                    if (string.IsNullOrEmpty(TextResponse))
                    {
                        TextResponse = VoidTorResponse.RespGrp.ErrorData;
                    }
                    TranAmt = VoidTorResponse.CommonGrp.TxnAmt;
                    Stan = VoidTorResponse.CommonGrp.STAN;
                    tranType = VoidTorRequest.CommonGrp.ReversalInd.ToString() + " " + VoidTorResponse.CommonGrp.TxnType.ToString();
                    if (string.IsNullOrEmpty(VrifoneHndlr.customerAttribute.CardNumber))
                    {
                        AccNo = FDTransctionRequest.OrigAccNo;
                    }
                    else
                    {
                        AccNo = VrifoneHndlr.customerAttribute.CardNumber;
                    }

                }
                else if (Type.Equals("TATokenRequest"))
                {
                    TokenID = taResponseDetails.TAGrp.Tkn;
                    if (!string.IsNullOrEmpty(taResponseDetails.TAGrp.TAExpDate))
                    { CardExpiryDate = taResponseDetails.TAGrp.TAExpDate; }
                    else
                    {
                        CardExpiryDate = VrifoneHndlr.customerAttribute.ExpDate;
                    }
                    CardType = (FDTransctionRequest.isCredit) ? "Credit" : "Debit";
                    AuthCode = taResponseDetails.RespGrp.AuthID;
                    TransDatetime = taResponseDetails.CommonGrp.LocalDateTime;
                    RtnCode = taResponseDetails.RespGrp.RespCode;
                    TextResponse = taResponseDetails.RespGrp.AddtlRespData.Replace("APPROVAL", "APPROVED").Replace("APPRV", "APPROVED");
                    if (string.IsNullOrEmpty(TextResponse))
                    {
                        TextResponse = taResponseDetails.RespGrp.ErrorData;
                    }
                    Stan = taResponseDetails.CommonGrp.STAN;
                    TranAmt = "0";
                    tranType = taResponseDetails.CommonGrp.TxnType.ToString();
                    AccNo = VrifoneHndlr.customerAttribute.CardNumber;
                }

                if (VrifoneHndlr.customerAttribute.isSwipe || FDTransctionRequest.wasOrigSwiped)
                {
                    isSwiped = "1";
                }
                if (string.IsNullOrEmpty(AuthCode))
                    AuthCode = "";
                TransDatetime = DateTime.ParseExact(TransDatetime, "yyyyMMddHHmmss", null).ToString("yyyy-MM-dd HH:mm:ss");
                respone = genarateGroupData(CardName, Type);
                firstdataResponse.responseCode = RtnCode;
                firstdataResponse.responseMessage = TextResponse;
                firstdataResponse.responseAuthId = AuthCode;
                firstdataResponse.CardNo = AccNo;
                firstdataResponse.CardName = crdGrp.CardType.ToString();
                firstdataResponse.ReferenceNo = FDTransctionRequest.ReferenceNo;
                firstdataResponse.TransAmount = double.Parse(TranAmt) / 100;
                firstdataResponse.CardExpiryDate = CardExpiryDate;
                if (!string.IsNullOrEmpty(CardExpiryDate))
                {
                    CardExpiryDate = DateTime.Today.Year.ToString().Substring(0, 2) + CardExpiryDate.Substring(CardExpiryDate.Length - 2, 2) + CardExpiryDate.Substring(0, 2);
                }
                //else if (Type.Equals("TATokenRequest"))
                //{
                //    CardExpiryDate="20" + CardExpiryDate;
                //}
                try
                {
                    if (isPrint && FDTransctionRequest.isPrintReceipt && !FDTransctionRequest.TransactionType.Equals("Completion"))
                    {
                        if (_utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y" || FirstDataAdapter.IsUnattended)
                        {
                            print(false);//Reciept printing
                        }
                        if (_utilities.getParafaitDefaults("PRINT_MERCHANT_RECEIPT") == "Y" && !FirstDataAdapter.IsUnattended)
                        {
                            print(true);//Reciept printing
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Print Error!!!..", ex);
                    displayMessage("Print Error!!!..");
                }
                dTable = _utilities.executeDataTable(@"insert into CCTransactionsPGW 
                                               (InvoiceNo,ProcessData,TokenID,CardType,AuthCode,TransactionDatetime,site_id, StatusId,DSIXReturnCode, TextResponse,AcctNo,TranCode,Authorize,UserTraceData,ResponseOrigin,CaptureStatus,AcqRefData, RefNo,TipAmount,CustomerCopy)
                                        values (@TrxId,@Stan,@Tocken,@CardType,@AuthCode,@TranDatetime,@site,@StatusId,@RtnCode,@Response,@AccNo,@TranType,@Authorize,@Card,@expdate,@swiped,@Acqdata, @refNo,@tipAmount,@customerCopy) select @@identity",
                                        new SqlParameter("@TrxId", FDTransctionRequest.ReferenceNo),
                                        new SqlParameter("@Stan", Stan),
                                        new SqlParameter("@Tocken", TokenID),
                                        new SqlParameter("@CardType", CardType),
                                        new SqlParameter("@AuthCode", AuthCode),
                                        new SqlParameter("@site", _utilities.ParafaitEnv.SiteId),
                                        new SqlParameter("@StatusId", statusID),
                                        new SqlParameter("@RtnCode", RtnCode),
                                        new SqlParameter("@TranDatetime", TransDatetime),
                                        string.IsNullOrEmpty(AccNo) ? (new SqlParameter("@AccNo", DBNull.Value)) : new SqlParameter("@AccNo", AccNo),
                                        new SqlParameter("@TranType", tranType),
                                        new SqlParameter("@Authorize", TranAmt),
                                        new SqlParameter("@Card", CardName),
                                        new SqlParameter("@expdate", CardExpiryDate),
                                        new SqlParameter("@swiped", isSwiped),
                                        new SqlParameter("@Acqdata", respone),
                                        new SqlParameter("@Response", TextResponse),
                                        (string.IsNullOrEmpty(tipAmount) ? new SqlParameter("@tipAmount", DBNull.Value) : new SqlParameter("@tipAmount", tipAmount)),
                                        (FDTransctionRequest.OrigPaymentId <= 0) ? new SqlParameter("@refNo", DBNull.Value) : new SqlParameter("@refNo", FDTransctionRequest.OrigPaymentId),
                                        new SqlParameter("@customerCopy", firstdataResponse.ReceiptText));

                log.LogVariableState("@TrxId", FDTransctionRequest.ReferenceNo);
                log.LogVariableState("@Stan", Stan);
                log.LogVariableState("@Tocken", TokenID);
                log.LogVariableState("@CardType", CardType);
                log.LogVariableState("@AuthCode", AuthCode);
                log.LogVariableState("@site", _utilities.ParafaitEnv.SiteId);
                log.LogVariableState("@StatusId", statusID);
                log.LogVariableState("@RtnCode", RtnCode);
                log.LogVariableState("@TranDatetime", TransDatetime);
                log.LogVariableState("@TranType", tranType);
                log.LogVariableState("@Authorize", TranAmt);
                log.LogVariableState("@Card", CardName);
                log.LogVariableState("@expdate", CardExpiryDate);
                log.LogVariableState("@swiped", isSwiped);
                log.LogVariableState("@Acqdata", respone);
                log.LogVariableState("@Response", TextResponse);
                log.LogVariableState("@tipAmount", DBNull.Value);
                log.LogVariableState("@tipAmount", tipAmount);
                log.LogVariableState("@refNo", DBNull.Value);
                log.LogVariableState("@refNo", FDTransctionRequest.OrigPaymentId);

                if (dTable.Rows.Count > 0)
                {
                    firstdataResponse.ccResponseId = dTable.Rows[0][0].ToString();
                }

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Response is not recorded due to some problem.\n Please note the details displayed", ex);
                //MessageBox.Show("Response is not recorded due to some problem.\n Please note the details displayed and contact semnox: \n Transaction status is :" + TextResponse + "\n Reference no : " + FDTransctionRequest.ReferenceNo + "\n Auth Code:" + AuthCode + "\n Transaction amount :" + (double.Parse(string.IsNullOrEmpty(TranAmt) ? "0" : TranAmt) / 100));
                // MessageBox.Show("Error:"+ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        private string genarateGroupData(string cardType, string tranType)
        {
            log.LogMethodEntry(cardType, tranType);

            //This function is to genrate the card group data which is mandatory for void transaction
            string response = "";

            try
            {

                switch (cardType)
                {
                    case "Visa":
                        if (tranType.Equals("Credit"))
                        {
                            try
                            {
                                visaGrp = (VisaGrp)CreditResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for visa group", ex);
                            }
                        }
                        else if (tranType.Equals("Reversal"))
                        {
                            try
                            {
                                visaGrp = (VisaGrp)VoidTorResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for visa group", ex);
                            }
                        }
                        if (visaGrp != null)
                        {
                            if (!string.IsNullOrEmpty(visaGrp.ACI.ToString()))
                            {
                                response = visaGrp.ACI.ToString();
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(visaGrp.TransID.ToString()))
                            {
                                response += visaGrp.TransID;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(visaGrp.CardLevelResult.ToString()))
                            {
                                response += visaGrp.CardLevelResult;
                            }
                        }
                        break;
                    case "MasterCard":
                        if (tranType.Equals("Credit"))
                        {
                            try
                            {
                                mastCrdGrp = (MCGrp)CreditResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for master card group", ex);

                            }
                        }
                        else if (tranType.Equals("Reversal"))
                        {
                            try
                            {
                                mastCrdGrp = (MCGrp)VoidTorResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for master card group", ex);
                            }
                        }
                        if (mastCrdGrp != null)
                        {
                            if (!string.IsNullOrEmpty(mastCrdGrp.BanknetData.ToString()))
                            {
                                response = mastCrdGrp.BanknetData.ToString();
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(mastCrdGrp.CCVErrorCode.ToString()))
                            {
                                response += mastCrdGrp.CCVErrorCode;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(mastCrdGrp.POSEntryModeChg.ToString()))
                            {
                                response += mastCrdGrp.POSEntryModeChg;
                            }
                        }
                        break;

                    case "Amex":
                        if (tranType.Equals("Credit"))
                        {
                            try
                            {
                                amxGrp = (AmexGrp)CreditResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for amex group", ex);
                            }
                        }
                        else if (tranType.Equals("Reversal"))
                        {
                            try
                            {
                                amxGrp = (AmexGrp)VoidTorResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for amex group", ex);
                            }
                        }
                        if (amxGrp != null)
                        {
                            if (!string.IsNullOrEmpty(amxGrp.AmExPOSData.ToString()))
                            {
                                response = amxGrp.AmExPOSData.ToString();
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(amxGrp.AmExTranID.ToString()))
                            {
                                response += amxGrp.AmExTranID;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(amxGrp.GdSoldCd.ToString()))
                            {
                                response += amxGrp.GdSoldCd;
                            }
                        }
                        break;
                    case "JCB":
                    case "Diners":
                    case "Discover":
                        if (tranType.Equals("Credit"))
                        {
                            try
                            {
                                discGrp = (DSGrp)CreditResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for discover group", ex);
                            }
                        }
                        else if (tranType.Equals("Reversal"))
                        {
                            try
                            {
                                discGrp = (DSGrp)VoidTorResponse.Item;
                            }
                            catch (Exception ex)
                            {
                                log.Error("Unable to get a valid value for discover group", ex);
                            }
                        }
                        if (discGrp != null)
                        {
                            if (!string.IsNullOrEmpty(discGrp.DiscProcCode.ToString()))
                            {
                                response = discGrp.DiscProcCode.ToString();
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(discGrp.DiscPOSEntry.ToString()))
                            {
                                response += discGrp.DiscPOSEntry;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(discGrp.DiscRespCode.ToString()))
                            {
                                response += discGrp.DiscRespCode;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(discGrp.DiscPOSData.ToString()))
                            {
                                response += discGrp.DiscPOSData;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(discGrp.DiscTransQualifier.ToString()))
                            {
                                response += discGrp.DiscTransQualifier;
                            }
                            response += "|";
                            if (!string.IsNullOrEmpty(discGrp.DiscNRID.ToString()))
                            {
                                response += discGrp.DiscNRID;
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while genarating Group Data", ex);
            }

            log.LogMethodExit(response);
            return response;
        }
        private object setGroupData(string cardType)
        {
            log.LogMethodEntry(cardType);

            //This function is to genrate the card group  based on the origianl response for void transaction
            string[] groupData;
            object returnGroupdata = null;
            groupData = FDTransctionRequest.OrigGroupData.Split('|');
            switch (cardType)
            {
                case "Visa":
                    switch (groupData[0])
                    {
                        case "A":
                            visaGrp.ACI = ACIType.A;
                            visaGrp.ACISpecified = true;
                            break;
                        case "B":
                            visaGrp.ACI = ACIType.B;
                            visaGrp.ACISpecified = true;
                            break;
                        case "C":
                            visaGrp.ACI = ACIType.C;
                            visaGrp.ACISpecified = true;
                            break;
                        case "E":
                            visaGrp.ACI = ACIType.E;
                            visaGrp.ACISpecified = true;
                            break;
                        case "F":
                            visaGrp.ACI = ACIType.F;
                            visaGrp.ACISpecified = true;
                            break;
                        case "I":
                            visaGrp.ACI = ACIType.I;
                            visaGrp.ACISpecified = true;
                            break;
                        case "J":
                            visaGrp.ACI = ACIType.J;
                            visaGrp.ACISpecified = true;
                            break;
                        case "K":
                            visaGrp.ACI = ACIType.K;
                            visaGrp.ACISpecified = true;
                            break;
                        case "N":
                            visaGrp.ACI = ACIType.N;
                            visaGrp.ACISpecified = true;
                            break;
                        case "P":
                            visaGrp.ACI = ACIType.P;
                            visaGrp.ACISpecified = true;
                            break;
                        case "R":
                            visaGrp.ACI = ACIType.R;
                            visaGrp.ACISpecified = true;
                            break;
                        case "S":
                            visaGrp.ACI = ACIType.S;
                            visaGrp.ACISpecified = true;
                            break;
                        case "T":
                            visaGrp.ACI = ACIType.T;
                            visaGrp.ACISpecified = true;
                            break;
                        case "U":
                            visaGrp.ACI = ACIType.U;
                            visaGrp.ACISpecified = true;
                            break;
                        case "V":
                            visaGrp.ACI = ACIType.V;
                            visaGrp.ACISpecified = true;
                            break;
                        case "W":
                            visaGrp.ACI = ACIType.W;
                            visaGrp.ACISpecified = true;
                            break;
                        case "Y":
                            visaGrp.ACI = ACIType.Y;
                            visaGrp.ACISpecified = true;
                            break;
                    }

                    if (!string.IsNullOrEmpty(groupData[1]))
                    {
                        visaGrp.TransID = groupData[1];
                    }
                    if (!string.IsNullOrEmpty(groupData[2]) && !FDTransctionRequest.TransactionType.Equals("Void"))
                    {
                        visaGrp.CardLevelResult = groupData[2];
                    }
                    returnGroupdata = visaGrp;
                    break;
                case "MasterCard":
                    if (!string.IsNullOrEmpty(groupData[0]))
                    {
                        mastCrdGrp.BanknetData = groupData[0];
                    }
                    if (!string.IsNullOrEmpty(groupData[1]))
                    {
                        mastCrdGrp.CCVErrorCode = CCVErrorCodeType.Y;
                    }
                    if (!string.IsNullOrEmpty(groupData[2]))
                    {
                        mastCrdGrp.POSEntryModeChg = POSEntryModeChgType.Y;
                    }
                    returnGroupdata = mastCrdGrp;
                    break;
                case "Amex":
                    if (!string.IsNullOrEmpty(groupData[0]))
                    {
                        amxGrp.AmExPOSData = groupData[0];
                    }

                    if (!string.IsNullOrEmpty(groupData[1]))
                    {
                        amxGrp.AmExTranID = groupData[1];
                    }
                    if (!string.IsNullOrEmpty(groupData[2]))
                    {
                        amxGrp.GdSoldCd = GdSoldCdType.Item1000;
                    }
                    returnGroupdata = amxGrp;
                    break;
                case "JCB":
                case "Discover":
                case "Diners":
                    if (!string.IsNullOrEmpty(groupData[0]))
                    {
                        discGrp.DiscProcCode = groupData[0];
                    }

                    if (!string.IsNullOrEmpty(groupData[1]))
                    {
                        discGrp.DiscPOSEntry = groupData[1];
                    }

                    if (!string.IsNullOrEmpty(groupData[2]))
                    {
                        discGrp.DiscRespCode = groupData[2];
                    }

                    if (!string.IsNullOrEmpty(groupData[3]))
                    {
                        discGrp.DiscPOSData = groupData[3];
                    }

                    if (!string.IsNullOrEmpty(groupData[4]))
                    {
                        discGrp.DiscTransQualifier = groupData[4];
                    }

                    if (!string.IsNullOrEmpty(groupData[5]))
                    {
                        discGrp.DiscNRID = groupData[5];
                    }
                    returnGroupdata = discGrp;
                    break;

            }

            log.LogMethodExit(returnGroupdata);
            return returnGroupdata;
        }

        private string PerformAuthorization(ref string RespCode)//Starts modification on 18-Oct-2016 for adding tip feature
        {
            log.LogMethodEntry(RespCode);

            string responseString = "";
            string xml;
            object Response;
            string clientRef = string.Empty;
            //string status;2017-05-13
            creditRequest = new CreditRequestDetails();
            debitRequest = new DebitRequestDetails();

            //if (string.IsNullOrEmpty(FDTransctionRequest.OrigToken))//2017-05-13
            //{
            //    status = ReadCard();
            //    if (!status.Equals("00"))
            //    {
            //        return status;
            //    }
            //}
            //displayMessage("Processing... Please wait...");
            //VrifoneHndlr.displayMessages("Processing...", "Please wait...", "", "", 300);
            try
            {
                cmngrp.TxnType = TxnTypeType.Authorization;
                GroupInitialize(FDTransctionRequest.isCredit);
                if (FDTransctionRequest.isCredit)
                {
                    creditRequest.TAGrp = taGrp;
                    creditRequest.CardGrp = crdGrp;
                    creditRequest.AddtlAmtGrp = new AddtlAmtGrp[1];
                    //addamtGrp.AddAmtType = AddAmtTypeType.PreAuthAmt;
                    //addamtGrp.AddAmtTypeSpecified = true;
                    //addamtGrp.AddAmt = cmngrp.TxnAmt;
                    //addamtGrp.AddAmtCrncy = cmngrp.TxnCrncy;
                    creditRequest.AddtlAmtGrp[0] = addamtGrp;

                    if (CardName.Equals("Visa"))
                    {
                        creditRequest.Item = visaGrp;
                    }
                    cmngrp.PymtType = PymtTypeType.Credit;
                    creditRequest.CommonGrp = cmngrp;
                    gmfMsgVar.Item = creditRequest;
                    creditRequest = new CreditRequestDetails();
                    creditRequest = gmfMsgVar.Item as CreditRequestDetails;
                }
                else
                {
                    debitRequest.TAGrp = taGrp;
                    cmngrp.PymtType = PymtTypeType.Debit;
                    debitRequest.CommonGrp = cmngrp;
                    debitRequest.PINGrp = pingrp;
                    debitRequest.AddtlAmtGrp = new AddtlAmtGrp[1];
                    debitRequest.AddtlAmtGrp[0] = addamtGrp;
                    gmfMsgVar.Item = debitRequest;
                    debitRequest = new DebitRequestDetails();
                    debitRequest = gmfMsgVar.Item as DebitRequestDetails;
                }
                clientRef = cmngrp.STAN + "V" + cmngrp.TPPID;//Should be unique for each Transaction
                clientRef = clientRef.PadLeft(14, '0');
                xml = GetXMLData();
                Response = ProcessTransaction(xml, clientRef);
                if (FDTransctionRequest.isCredit)
                {
                    try
                    {
                        CreditResponse = (CreditResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while fetching a valid value for CreditResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while Rejecting the request", e1);
                        }
                    }
                    if (CreditResponse != null)
                    {
                        if (CreditResponse.RespGrp.RespCode.Equals("000") || CreditResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (CreditResponse.RespGrp.RespCode.Equals("500"))//declined
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Credit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            CreateResponse("Credit", 3);
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.ErrorData))//error in transaction
                        {
                            CreateResponse("Credit", 3);
                            responseString = CreditResponse.RespGrp.ErrorData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Authorization", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Unable to get a valid value for responseString", e1);
                        }
                    }
                }
                else
                {
                    try
                    {
                        DebitResponse = (DebitResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception e1)
                    {
                        log.Error("Error occured while fetching a valid value for DebitResponse", e1);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e2)
                        {
                            log.Error("error while rejecting the request", e2);
                        }
                    }

                    if (DebitResponse != null)
                    {
                        if (DebitResponse.RespGrp.RespCode.Equals("000") || DebitResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (DebitResponse.RespGrp.RespCode.Equals("500"))//Declined
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.ErrorData))//Error
                        {
                            responseString = DebitResponse.RespGrp.ErrorData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Debit", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Authorization", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error whilefetching a valid value for responseString", e1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while performing authoriztion", ex);
                isPrint = false;
                responseString = "Error in Processing...";
            }

            log.LogMethodExit(responseString);
            return responseString;
        }//Ends modification on 18-Oct-2016 for adding tip feature

        private string PerformTokenRequest(ref string RespCode)//Starts modification on 18-Oct-2016 for adding tip feature
        {
            log.LogMethodEntry(RespCode);

            string responseString = "";
            string xml;
            object Response;
            string clientRef = string.Empty;
            //string status;//2017-05-13
            taRequestDetails = new TARequestDetails();

            //status = ReadCard();//2017-05-13
            //if (!status.Equals("00"))
            //{
            //    return status;
            //}

            //VrifoneHndlr.displayMessages("Processing...", "Please wait...", "", "", 300);
            try
            {
                cmngrp.TxnType = TxnTypeType.TATokenRequest;
                GroupInitialize(FDTransctionRequest.isCredit);
                taRequestDetails.TAGrp = taGrp;
                taRequestDetails.CardGrp = crdGrp;
                cmngrp.PymtType = (FDTransctionRequest.isCredit) ? PymtTypeType.Credit : PymtTypeType.Debit;
                taRequestDetails.CommonGrp = cmngrp;
                gmfMsgVar.Item = taRequestDetails;
                taRequestDetails = new TARequestDetails();
                taRequestDetails = gmfMsgVar.Item as TARequestDetails;

                clientRef = cmngrp.STAN + "V" + cmngrp.TPPID;//Should be unique for each Transaction
                clientRef = clientRef.PadLeft(14, '0');
                xml = GetXMLData();
                Response = ProcessTransaction(xml, clientRef);

                try
                {
                    taResponseDetails = (TAResponseDetails)Response;//in case of timeout the response is just a return code
                }
                catch (Exception e1)
                {
                    log.Error("Error occured while fetching a valid value for taResponseDetails", e1);

                    try
                    {
                        RejectResp = (RejectResponseDetails)Response;
                        responseString = "Request Rejected.";
                        isPrint = false;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error while rejecting the request", ex);
                    }
                }
                if (taResponseDetails != null)
                {
                    if (taResponseDetails.RespGrp.RespCode.Equals("000"))//Full or partial approval
                    {
                        isPrint = false;
                        responseString = taResponseDetails.RespGrp.AddtlRespData;
                        RespCode = taResponseDetails.RespGrp.RespCode;
                        CreateResponse("TATokenRequest", 1);
                        _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + taResponseDetails.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                    }
                    else if (taResponseDetails.RespGrp.RespCode.Equals("500"))//declined
                    {
                        isPrint = true;
                        responseString = taResponseDetails.RespGrp.AddtlRespData;
                        RespCode = taResponseDetails.RespGrp.RespCode;
                        CreateResponse("TATokenRequest", 2);
                        _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + taResponseDetails.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                    }
                    else if (!string.IsNullOrEmpty(taResponseDetails.RespGrp.AddtlRespData))//Other status
                    {
                        isPrint = true;
                        CreateResponse("TATokenRequest", 3);
                        responseString = taResponseDetails.RespGrp.AddtlRespData;
                        RespCode = taResponseDetails.RespGrp.RespCode;
                        _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + taResponseDetails.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                    }
                    else if (!string.IsNullOrEmpty(taResponseDetails.RespGrp.ErrorData))//error in transaction
                    {
                        CreateResponse("TATokenRequest", 3);
                        responseString = taResponseDetails.RespGrp.ErrorData;
                        RespCode = taResponseDetails.RespGrp.RespCode;
                        _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + taResponseDetails.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                    }
                }
                else
                {
                    try
                    {
                        responseString = Response.ToString();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while fetching a valid value for responseString", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while performing Token Request", ex);
                isPrint = false;
                responseString = "Error in Processing...";
            }

            log.LogVariableState("RespCode", RespCode);
            log.LogMethodExit(responseString);
            return responseString;
        }//Ends modification on 18-Oct-2016 for adding tip feature

        private string CompleteTransaction(ref string RespCode)//Starts modification on 18-Oct-2016 for adding tip feature
        {
            log.LogMethodEntry(RespCode);

            string responseString = "";
            string xml;
            object Response;
            string clientRef = string.Empty;
            creditRequest = new CreditRequestDetails();
            debitRequest = new DebitRequestDetails();
            displayMessage("Processing... Please wait...");
            VrifoneHndlr.displayMessages("Processing...", "Please wait...", "", "", 300);
            try
            {
                GroupInitialize(FDTransctionRequest.isCredit);
                cmngrp.TxnType = TxnTypeType.Completion;
                cmngrp.TxnAmt = (Convert.ToInt64(cmngrp.TxnAmt) + (FDTransctionRequest.TipAmount * 100)).ToString();
                if (FDTransctionRequest.isCredit)
                {
                    creditRequest.TAGrp = taGrp;
                    creditRequest.CardGrp = crdGrp;
                    creditRequest.AddtlAmtGrp = new AddtlAmtGrp[2];
                    creditRequest.AddtlAmtGrp[0] = addamtGrp;
                    creditRequest.AddtlAmtGrp[1] = new AddtlAmtGrp();
                    creditRequest.AddtlAmtGrp[1].AddAmtType = AddAmtTypeType.TotalAuthAmt;
                    creditRequest.AddtlAmtGrp[1].AddAmt = (FDTransctionRequest.OrigTransactionAmount + (FDTransctionRequest.TipAmount * 100)).ToString();
                    creditRequest.AddtlAmtGrp[1].AddAmtTypeSpecified = true;
                    creditRequest.AddtlAmtGrp[1].AddAmtCrncy = CurrencyCode;

                    //if (CardName.Equals("Visa"))
                    //{
                    //    creditRequest.Item = visaGrp;
                    //}
                    cmngrp.PymtType = PymtTypeType.Credit;
                    creditRequest.CommonGrp = cmngrp;
                    creditRequest.OrigAuthGrp = orgGrp;
                    creditRequest.Item = setGroupData(crdGrp.CardType.ToString());
                    gmfMsgVar.Item = creditRequest;
                    creditRequest = new CreditRequestDetails();
                    creditRequest = gmfMsgVar.Item as CreditRequestDetails;
                }
                else
                {
                    debitRequest.TAGrp = taGrp;
                    cmngrp.PymtType = PymtTypeType.Debit;
                    debitRequest.CommonGrp = cmngrp;
                    //debitRequest.PINGrp = pingrp;
                    debitRequest.AddtlAmtGrp = new AddtlAmtGrp[2];
                    debitRequest.AddtlAmtGrp[0] = addamtGrp;
                    debitRequest.AddtlAmtGrp[1] = new AddtlAmtGrp();
                    debitRequest.AddtlAmtGrp[1].AddAmtType = AddAmtTypeType.TotalAuthAmt;
                    debitRequest.AddtlAmtGrp[1].AddAmt = (FDTransctionRequest.OrigTransactionAmount + (FDTransctionRequest.TipAmount * 100)).ToString();
                    debitRequest.AddtlAmtGrp[1].AddAmtTypeSpecified = true;
                    debitRequest.AddtlAmtGrp[1].AddAmtCrncy = CurrencyCode;
                    debitRequest.OrigAuthGrp = orgGrp;
                    //prodcodegp = new ProdCodeGrp();
                    //prodcodegp.ServLvl = ServLvlType.F;
                    //prodcodegp.ServLvlSpecified = true;
                    //prodcodegp.NumOfProds = NumOfProdsType.Item01;
                    //prodcodegp.NumOfProdsSpecified = true;
                    //prdctCodeDetGp = new ProdCodeDetGrp();
                    //prdctCodeDetGp.NACSProdCode = "2703";
                    //prdctCodeDetGp.UnitOfMsure = UnitOfMsureType.C;
                    //prdctCodeDetGp.Qnty = "1";
                    //prdctCodeDetGp.UnitOfMsureSpecified = true;
                    //prdctCodeDetGp.UnitPrice = cmngrp.TxnAmt;
                    //prdctCodeDetGp.ProdAmt = cmngrp.TxnAmt;
                    //debitRequest.ProdCodeGrp = prodcodegp;
                    //debitRequest.ProdCodeDetGrp =new ProdCodeDetGrp[1];
                    //debitRequest.ProdCodeDetGrp[0] = prdctCodeDetGp;
                    gmfMsgVar.Item = debitRequest;
                    debitRequest = new DebitRequestDetails();
                    debitRequest = gmfMsgVar.Item as DebitRequestDetails;
                }
                clientRef = cmngrp.STAN + "V" + cmngrp.TPPID;//Should be unique for each Transaction
                clientRef = clientRef.PadLeft(14, '0');
                xml = GetXMLData();
                Response = ProcessTransaction(xml, clientRef);
                if (FDTransctionRequest.isCredit)
                {
                    try
                    {
                        CreditResponse = (CreditResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception ex)
                    {
                        log.Error("Erroe while fetching a valid value for CreditResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while rejecting the request", e1);
                        }
                    }
                    if (CreditResponse != null)
                    {
                        if (CreditResponse.RespGrp.RespCode.Equals("000") || CreditResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (CreditResponse.RespGrp.RespCode.Equals("500"))//declined
                        {
                            isPrint = true;
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            CreateResponse("Completion", 3);
                            responseString = CreditResponse.RespGrp.AddtlRespData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);

                        }
                        else if (!string.IsNullOrEmpty(CreditResponse.RespGrp.ErrorData))//error in transaction
                        {
                            CreateResponse("Completion", 3);
                            responseString = CreditResponse.RespGrp.ErrorData;
                            RespCode = CreditResponse.RespGrp.RespCode;
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + CreditResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Completion", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while fetching a valid value for responseString", e1);
                        }
                    }
                }
                else
                {
                    try
                    {
                        DebitResponse = (DebitResponseDetails)Response;//in case of timeout the response is just a return code
                    }
                    catch (Exception ex)
                    {

                        log.Error("Error while fetching a valid value for DebitResponse", ex);

                        try
                        {
                            RejectResp = (RejectResponseDetails)Response;
                            responseString = "Request Rejected.";
                            isPrint = false;
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while rejecting the request", e1);
                        }
                    }

                    if (DebitResponse != null)
                    {
                        if (DebitResponse.RespGrp.RespCode.Equals("000") || DebitResponse.RespGrp.RespCode.Equals("002"))//Full or partial approval
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 1);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (DebitResponse.RespGrp.RespCode.Equals("500"))//Declined
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 2);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.AddtlRespData))//Other status
                        {
                            isPrint = true;
                            responseString = DebitResponse.RespGrp.AddtlRespData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                        else if (!string.IsNullOrEmpty(DebitResponse.RespGrp.ErrorData))//Error
                        {
                            responseString = DebitResponse.RespGrp.ErrorData;
                            RespCode = DebitResponse.RespGrp.RespCode;
                            CreateResponse("Completion", 3);
                            _utilities.EventLog.logEvent("FirstData", 'I', "FirstData", "STAN:" + DebitResponse.CommonGrp.STAN + " Status Code:" + RespCode + " Response:" + responseString, "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        }
                    }
                    else if (Response == null || Response.ToString().Equals("203") || Response.ToString().Equals("204") || Response.ToString().Equals("205") || Response.ToString().Equals("206") || Response.ToString().Equals("405") || Response.ToString().Equals("505") || Response.ToString().Equals("008") || Response.ToString().Substring(0, 1).Equals("5"))//Time out
                    {
                        _utilities.EventLog.logEvent("FirstData", 'E', "FirstData", "STAN:" + cmngrp.STAN + " Return Code:" + Response + " Response:", "Payment", 1, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                        responseString = Reversal("Timeout", "Completion", FDTransctionRequest.isCredit, ref RespCode);
                    }
                    else
                    {
                        try
                        {
                            responseString = Response.ToString();
                        }
                        catch (Exception e1)
                        {
                            log.Error("Error while fetching a valid value for responseString", e1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in Processing...", ex);
                isPrint = false;
                responseString = "Error in Processing...";
            }

            log.LogVariableState("RespCode", RespCode);
            log.LogMethodExit(responseString);
            return responseString;
        }//Ends modification on 18-Oct-2016 for adding tip feature
    }
}
