using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RBA_SDK;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.ElementExpress;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class ElementPSCore : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IngenicoIUP250 pinPad;
        ElementExpress.Card card;
        
        ElementTransactionRequest _TrxRequest;
        /// <summary>
        /// ElementExpressResponse
        /// </summary>
        public ElementExpress.Response ElementExpressResponse;
        /// <summary>
        /// object CCResponseId
        /// </summary>
        public object CCResponseId;
        Utilities _utilities;
        string DeviceSerialNumber = "";
        Semnox.Core.Utilities.Logger logger = new Semnox.Core.Utilities.Logger(Semnox.Core.Utilities.Logger.LogType.Kiosk, "ElementPS " + DateTime.Now.ToString("yyyy-MM-dd"));
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="ingenicoTerminal"></param>
        /// <param name="TrxRequest"></param>
        /// <param name="inUtilities"></param>
        public ElementPSCore(IngenicoIUP250 ingenicoTerminal, ElementTransactionRequest TrxRequest, Utilities inUtilities)
        {
            log.LogMethodEntry(ingenicoTerminal, TrxRequest, inUtilities);
            InitializeComponent();

            txtStatus.Text = "";

            _utilities = inUtilities;
            _TrxRequest = TrxRequest;
            pinPad = ingenicoTerminal;
            if (pinPad != null)
                pinPad.setReceiveHandler = pinPadMessageHandler;
            btnCancel.Text = _utilities.MessageUtils.getMessage("Cancel");
            lblCardCharged.Text = _utilities.MessageUtils.getMessage(1839, TrxRequest.TransactionAmount.ToString(_utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            logger.WriteLog("Element adapter initiaized");
            log.LogMethodExit(null);
        }

        private void frmUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Timer timer = new Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            timer.Start();
            log.LogMethodExit(null);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            (sender as Timer).Stop();

            switch (_TrxRequest.TransactionType)
            {
                case "PAYMENT":
                    {
                        MakePayment();
                        break;
                    }
                case "VOID":
                    {
                        VoidSale(_TrxRequest.OrigElementTransactionId);
                        break;
                    }
                case "RETURN":
                    {
                        Return(_TrxRequest.TransactionAmount, _TrxRequest.OrigElementTransactionId);
                        break;
                    }
                case "PARTIAL REVERSAL":
                    {
                        PartialReversal(_TrxRequest.TransactionAmount, _TrxRequest.OrigTransactionAmount, _TrxRequest.OrigElementTransactionId);
                        break;
                    }
                case "FULL REVERSAL":
                    {
                        FullReversal(_TrxRequest.OrigTransactionAmount, _TrxRequest.OrigElementTransactionId);
                        break;
                    }
                case "HEALTH CHECK":
                    {
                        HealthCheck();
                        break;
                    }
            }
            log.LogMethodExit(null);
        }

        Timer TimeOutTimer;
        void MakePayment()
        {
            log.LogMethodEntry();
            try
            {
                //    displayStatus("CHECKING TERMINAL CONNECTION");

                //    try
                //    {
                //        try
                //        {
                //            // retry 
                //            pinPad.UnitSerialNumberReq();
                //        }
                //        catch
                //        {
                //            pinPad.UnitSerialNumberReq();
                //        }
                //    }
                //    catch
                //    {
                //        displayStatus("UNABLE TO GET STATUS FROM TERMINAL");
                //        return;
                //    }

                logger.WriteLog("MakePayment begin");

                label1.Text = _utilities.MessageUtils.getMessage("CREDIT CARD PAYMENT");
                label2.Text = _utilities.MessageUtils.getMessage("PLEASE WAIT...");
                string s1 = _utilities.MessageUtils.getMessage("PLEASE INSERT CREDIT / DEBIT CARD");
                string s2 = _utilities.MessageUtils.getMessage("WAIT FOR GREEN LIGHT IN CARD READER");
                string s3 = _utilities.MessageUtils.getMessage("REMOVE CARD QUICKLY");
                displayStatus(Environment.NewLine + s1 + Environment.NewLine + Environment.NewLine +
                            s2 + Environment.NewLine + Environment.NewLine +
                            s3,
                            true);

                try
                {
                    pinPad.CardReadRequest();
                    logger.WriteLog("Sent card read request");
                }
                catch (Exception ex)
                {
                    log.Error("Error because its UNABLE TO GET STATUS FROM TERMINAL", ex);
                    displayStatus("UNABLE TO GET STATUS FROM TERMINAL");
                    log.LogMethodExit(null);
                    return;
                }

                TimeOutTimer = new Timer();
                TimeOutTimer.Interval = 500;
                TimeOutTimer.Tick += TimeOutTimer_Tick;
                TimeOutTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error occured while making payment", ex);
                displayStatus(ex.Message);
            }
            log.LogMethodExit(null);
        }

        double totSecs = 0;
        int timeOut = 45;
        bool firstTimeOut = false;
        bool secondTimeOut = false;
        void TimeOutTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            totSecs += TimeOutTimer.Interval / 1000.0;
            if (totSecs > timeOut)
            {
                if (firstTimeOut == false || secondTimeOut == false)
                {
                    string s1 = _utilities.MessageUtils.getMessage("CARD READ TIMED OUT. PLEASE RETRY..");
                    string s2 = _utilities.MessageUtils.getMessage("INSERT CARD AND WAIT FOR GREEN LIGHT");
                    string s3 = _utilities.MessageUtils.getMessage("REMOVE CARD QUICKLY");
                    displayStatus(Environment.NewLine + s1 + Environment.NewLine + Environment.NewLine +
                                s2 + Environment.NewLine + Environment.NewLine +
                                s3,
                                true);

                    try
                    {
                        logger.WriteLog("Resending card read request on timeout");
                        pinPad.CardReadRequest();
                        logger.WriteLog("Sent card read request");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occured while Card Read Request", ex);
                        displayStatus(ex.Message);
                    }
                }

                if (firstTimeOut == false)
                {
                    logger.WriteLog("First Timeout");
                    firstTimeOut = true;
                    totSecs = 0;
                    timeOut = 20;
                    log.LogMethodExit(null);
                    return;
                }

                if (secondTimeOut == false)
                {
                    logger.WriteLog("Second Timeout");
                    secondTimeOut = true;
                    totSecs = 0;
                    log.LogMethodExit(null);
                    return;
                }

                TimeOutTimer.Stop();
                ElementExpressResponse = new ElementExpress.Response();
                ElementExpressResponse.ExpressResponseMessage = "TIME OUT";
                displayStatus("TIME OUT");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            else
            {
                int sec = timeOut - (int)totSecs;
                if (sec > 0)
                {
                    lblTimeOut.Text = sec.ToString("#0");
                }
            }
            log.LogMethodExit(null);
        }

        void VoidSale(object OrigElementTransactionId)
        {
            log.LogMethodEntry(OrigElementTransactionId);
            ElementExpress.Application application = getApplication();
            ElementExpress.Credentials credentials = getCredentials();
            ElementExpress.Terminal terminal = getTerminal();
            terminal.CardInputCode = ElementExpress.CardInputCode.ManualKeyed;
            terminal.TerminalEnvironmentCode = ElementExpress.TerminalEnvironmentCode.LocalAttended;

            ElementExpress.Transaction transaction = new ElementExpress.Transaction();
            transaction.MarketCode = ElementExpress.MarketCode.Default;
            transaction.ClerkNumber = _utilities.ParafaitEnv.LoginID.Substring(0, Math.Min(_utilities.ParafaitEnv.LoginID.Length, 10));
            transaction.TransactionID = OrigElementTransactionId.ToString();
            transaction.ReferenceNumber = transaction.TicketNumber = _TrxRequest.TransactionId.ToString();

            ElementExpress.Response response = new ElementExpress.Response();

            ElementExpress.ExpressSoapClient esc = getSoapClient();

            try
            {
                response = esc.CreditCardVoid(credentials, application, terminal, transaction, null);

                UpdateResponseMessageDetails(response);
                EndProcess(response);
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of  error in response", ex);
                displayStatus(ex.Message);
            }
            log.LogMethodExit(null);
        }

        void SystemReversal(double Amount)
        {
            log.LogMethodEntry(Amount);
            logger.WriteLog("SYSTEM REVERSAL");

            _TrxRequest.TransactionType = "SYSTEM REVERSAL";
            Reversal(ElementExpress.ReversalType.System, Amount, Amount, null);
            log.LogMethodExit(null);
        }

        void FullReversal(double ReversalAmount, object OrigElementTransactionId)
        {
            log.LogMethodEntry(ReversalAmount, OrigElementTransactionId);
            Reversal(ElementExpress.ReversalType.Full, ReversalAmount, ReversalAmount, OrigElementTransactionId);
            log.LogMethodExit(null);
        }

        void PartialReversal(double ReversalAmount, double OrigAuthorizedAmount, object OrigElementTransactionId)
        {
            log.LogMethodEntry(ReversalAmount, OrigAuthorizedAmount, OrigElementTransactionId);
            Reversal(ElementExpress.ReversalType.Full, ReversalAmount, OrigAuthorizedAmount, OrigElementTransactionId);
            log.LogMethodExit(null);
        }

        void Reversal(ElementExpress.ReversalType ReversalType, double ReversalAmount, double OrigAuthorizedAmount, object OrigElementTransactionId)
        {
            log.LogMethodEntry(ReversalType, ReversalAmount, OrigAuthorizedAmount, OrigElementTransactionId);
            ElementExpress.Application application = getApplication();
            ElementExpress.Credentials credentials = getCredentials();
            ElementExpress.Terminal terminal = getTerminal();

            ElementExpress.Transaction transaction = new ElementExpress.Transaction();
            transaction.MarketCode = ElementExpress.MarketCode.Default;
            transaction.ClerkNumber = _utilities.ParafaitEnv.LoginID.Substring(0, Math.Min(_utilities.ParafaitEnv.LoginID.Length, 10));
            transaction.ReversalType = ReversalType;
            transaction.ReferenceNumber = transaction.TicketNumber = _TrxRequest.TransactionId.ToString();

            if (ReversalType != ElementExpress.ReversalType.System)
                transaction.TransactionID = OrigElementTransactionId.ToString();

            transaction.TransactionAmount = ReversalAmount.ToString("N2");

            if (ReversalType == ElementExpress.ReversalType.Partial)
                transaction.TotalAuthorizedAmount = OrigAuthorizedAmount.ToString("N2");

            if (ReversalType != ElementExpress.ReversalType.System)
            {
                terminal.CardInputCode = ElementExpress.CardInputCode.ManualKeyed;
                terminal.TerminalEnvironmentCode = ElementExpress.TerminalEnvironmentCode.LocalAttended;
            }

            ElementExpress.Response response = new ElementExpress.Response();

            ElementExpress.ExpressSoapClient esc = getSoapClient();

            try
            {
                if (ReversalType == ElementExpress.ReversalType.System)
                {
                    if (_TrxRequest.DebitCardSale)
                        response = esc.DebitCardReversal(credentials, application, terminal, card, transaction, null);
                    else
                        response = esc.CreditCardReversal(credentials, application, terminal, card, transaction, null);
                }
                else
                {

                    ElementExpress.Card dummy = new ElementExpress.Card();
                    if (_TrxRequest.DebitCardSale)
                    {
                        if (ReversalType == ElementExpress.ReversalType.Partial)
                        {
                            log.LogMethodExit(null, "Throwing application exception-Cannot partially reverse Debit Card payment");
                            throw new ApplicationException("Cannot partially reverse Debit Card payment");
                        }
                        else
                            response = esc.DebitCardReversal(credentials, application, terminal, dummy, transaction, null);
                    }
                    else
                        response = esc.CreditCardReversal(credentials, application, terminal, dummy, transaction, null);
                }
                UpdateResponseMessageDetails(response);

                if (ReversalType != ElementExpress.ReversalType.System)
                    EndProcess(response);
                else
                {
                    if (response.ExpressResponseCode == "0")
                        displayStatus("System Reversal: Success. Please Retry.");
                    else
                        displayStatus("System Reversal: " + response.ExpressResponseMessage + ". Please Retry.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in system reversal", ex);
                enableCancel();
                displayStatus(ex.Message);
                if (ReversalType == ElementExpress.ReversalType.System)
                {
                    log.LogMethodExit(null, "Throwing Exception" + ex);
                    throw ex;
                }

            }
            log.LogMethodExit(null);
        }

        void Return(double Amount, object OrigElementTransactionId)
        {
            log.LogMethodEntry(Amount, OrigElementTransactionId);
            ElementExpress.Application application = getApplication();
            ElementExpress.Credentials credentials = getCredentials();
            ElementExpress.Terminal terminal = getTerminal();
            terminal.CardInputCode = ElementExpress.CardInputCode.ManualKeyed;
            terminal.TerminalEnvironmentCode = ElementExpress.TerminalEnvironmentCode.LocalAttended;

            ElementExpress.Transaction transaction = new ElementExpress.Transaction();
            transaction.MarketCode = ElementExpress.MarketCode.Default;
            transaction.ClerkNumber = _utilities.ParafaitEnv.LoginID.Substring(0, Math.Min(_utilities.ParafaitEnv.LoginID.Length, 10));
            transaction.TransactionID = OrigElementTransactionId.ToString();
            transaction.TransactionAmount = Amount.ToString("N2");
            transaction.ReferenceNumber = transaction.TicketNumber = _TrxRequest.TransactionId.ToString();

            ElementExpress.Response response = new ElementExpress.Response();

            ElementExpress.ExpressSoapClient esc = getSoapClient();

            try
            {
                if (_TrxRequest.DebitCardSale)
                {
                    log.LogMethodExit(null, "Throwing application exception-Debit card return is not supported. Use reversal instead.");
                    throw new ApplicationException("Debit card return is not supported. Use reversal instead.");
                }

                else
                    response = esc.CreditCardReturn(credentials, application, terminal, transaction, null);

                UpdateResponseMessageDetails(response);
                EndProcess(response);
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of TRXRequest", ex);
                enableCancel();
                displayStatus(ex.Message);
            }
            log.LogMethodExit(null);
        }

        void CreditDebitCardSale(ElementExpress.Card card)
        {
            log.LogMethodEntry(card);
            logger.WriteLog("CreditDebitCardSale begin");

            ElementExpress.Application application = getApplication();
            ElementExpress.Credentials credentials = getCredentials();
            ElementExpress.Terminal terminal = getTerminal();

            ElementExpress.Transaction transaction = new ElementExpress.Transaction();
            transaction.MarketCode = ElementExpress.MarketCode.Default;
            transaction.ClerkNumber = _utilities.ParafaitEnv.LoginID.Substring(0, Math.Min(_utilities.ParafaitEnv.LoginID.Length, 10));
            transaction.TransactionAmount = _TrxRequest.TransactionAmount.ToString("N2");
            transaction.TicketNumber = transaction.ReferenceNumber = _TrxRequest.TransactionId.ToString();
            transaction.DuplicateOverrideFlag = ElementExpress.BooleanType.False;
            transaction.DuplicateCheckDisableFlag = ElementExpress.BooleanType.True;
            transaction.PartialApprovedFlag = ElementExpress.BooleanType.False; //In Unattended POS, making this flag False to avoid Partial Approvals

            if (card.EncryptedTrack2Data.Contains(':'))
            {
                card.CardDataKeySerialNumber = card.EncryptedTrack2Data.Split(':')[1];
                card.EncryptedTrack1Data = null;
                card.EncryptedTrack2Data = card.EncryptedTrack2Data.Split(':')[0];
                card.EncryptedFormat = ElementExpress.EncryptionFormat.Format4;
            }
            else
            {
                enableCancel();
                displayStatus("Invalid EncryptedTrackData");
            }

            Response response = new Response();

            ElementExpress.ExpressSoapClient esc = getSoapClient();
            string partialAmount = string.Empty; //used to reverse in case of partial payment
            try
            {
                if (_TrxRequest.DebitCardSale)
                {
                    card.KeySerialNumber = card.PINBlock.Substring(16);
                    card.PINBlock = card.PINBlock.Substring(0, 16);
                }

                try
                {
                    logger.WriteLog("Calling web service");
                    if (_TrxRequest.DebitCardSale)
                        response = esc.DebitCardSale(credentials, application, terminal, card, transaction, null);
                    else
                        response = esc.CreditCardSale(credentials, application, terminal, card, transaction, null, null);

                    logger.WriteLog("Web service call success");
                    UpdateResponseMessageDetails(response);
                }
                catch (Exception ex)
                {
                    log.Error("Error occured in payment process", ex);
                    displayStatus("REVERSING: " + ex.Message);
                    try
                    {
                        SystemReversal(_TrxRequest.TransactionAmount);
                    }
                    catch (Exception rex)
                    {
                        log.Error("Error occured in systemReversal", rex);
                        enableCancel();
                        displayStatus(rex.Message);
                        log.LogMethodExit(null);
                        return;
                    }
                }

                if (response.ExpressResponseCode == "1001"
                    || response.ExpressResponseCode == "1002"
                    || response.ExpressResponseCode == "1009")
                {
                    displayStatus(response.ExpressResponseMessage);
                    try
                    {
                        SystemReversal(_TrxRequest.TransactionAmount);
                    }
                    catch (Exception rex)
                    {
                        log.Error("Error occured due to system reversal", rex);
                        displayStatus(rex.Message);
                    }
                }

                EndProcess(response);
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of Credit Debit Card Sale", ex);
                enableCancel();
                displayStatus(ex.Message);
            }
            finally
            {
                card.EncryptedTrack1Data = card.EncryptedTrack2Data = "";
                card.KeySerialNumber = "";
                card.PINBlock = "";
                card = null;
            }
            log.LogMethodExit(null);
        }

        void HealthCheck()
        {
            log.LogMethodEntry();
            ElementExpress.Application application = getApplication();
            Credentials credentials = getCredentials();
            Response response = new Response();

            ElementExpress.ExpressSoapClient esc = getSoapClient();

            try
            {
                response = esc.HealthCheck(credentials, application);
                UpdateResponseMessageDetails(response);
                ElementExpressResponse = response;

                displayStatus("Express Message: " + response.ExpressResponseMessage);
                if (!string.IsNullOrEmpty(response.ExpressResponseCode) && response.ExpressResponseCode.Equals("0"))//Starts:Modification on 21-07-2017 for kiosk diagnostics
                {
                    this.DialogResult = DialogResult.OK;
                    Close();
                }//ends:Modification on 21-07-2017 for kiosk diagnostics
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of Kiosk diagnostics", ex);
                displayStatus(ex.Message);
            }
            finally
            {
                btnCancel.Enabled = true;
            }
            log.LogMethodExit(null);
        }

        ElementExpress.Application getApplication()
        {
            log.LogMethodEntry();
            ElementExpress.Application application = new ElementExpress.Application();
            application.ApplicationID = _utilities.getParafaitDefaults("ELEMENT_EXPRESS_APPLICATION_ID");
            application.ApplicationName = "Parafait";
            application.ApplicationVersion = "1.5.0.0";
            log.LogMethodExit(application);
            return application;
        }

        ElementExpress.Credentials getCredentials()
        {
            log.LogMethodEntry();
            ElementExpress.Credentials credentials = new ElementExpress.Credentials();
            credentials.AcceptorID = _utilities.getParafaitDefaults("ELEMENT_EXPRESS_ACCEPTOR_ID");
            credentials.AccountID = _utilities.getParafaitDefaults("ELEMENT_EXPRESS_ACCOUNT_ID");
            credentials.AccountToken = _utilities.getParafaitDefaults("ELEMENT_EXPRESS_ACCOUNT_TOKEN");
            log.LogMethodExit(credentials);
            return credentials;
        }

        Terminal getTerminal()
        {
            log.LogMethodEntry();
            Terminal terminal = new Terminal();
            terminal.TerminalID = _utilities.getParafaitDefaults("ELEMENT_TERMINAL_ID");
            terminal.CardholderPresentCode = CardholderPresentCode.Present;
            terminal.CardInputCode = CardInputCode.MagstripeRead;
            terminal.CardPresentCode = CardPresentCode.Present;
            terminal.TerminalCapabilityCode = TerminalCapabilityCode.MagstripeReader;
            terminal.TerminalEnvironmentCode = TerminalEnvironmentCode.LocalUnattended;
            terminal.TerminalType = TerminalType.PointOfSale;
            terminal.MotoECICode = MotoECICode.NotUsed;
            terminal.TerminalSerialNumber = DeviceSerialNumber;
            log.LogMethodExit(terminal);
            return terminal;
        }

        ExpressSoapClient getSoapClient()
        {
            log.LogMethodEntry();
            displayStatus(_utilities.MessageUtils.getMessage(1008));
            System.Windows.Forms.Application.DoEvents();

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(_utilities.getParafaitDefaults("ELEMENT_EXPRESS_URL"));
            ExpressSoapClient returnvalue = (new ExpressSoapClient(binding, address));
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        void pinPadMessageHandler(RBA_SDK.MESSAGE_ID msgId, PinPadResponseAttributes PinpadResponse)
        {
            log.LogMethodEntry(msgId, PinpadResponse);
            logger.WriteLog("pinPadMessageHandler received message: " + msgId.ToString());
            try
            {
                switch (msgId)
                {
                    case MESSAGE_ID.M07_UNIT_DATA:
                        {
                            if (!string.IsNullOrEmpty(PinpadResponse.DeviceSerialNumber))
                                DeviceSerialNumber = PinpadResponse.DeviceSerialNumber;
                            break;
                        }
                    case MESSAGE_ID.M11_STATUS:
                        {
                            if (PinpadResponse.DeviceStatus == "12" && PinpadResponse.DisplayText.ToLower().Contains("input"))
                                displayStatus("PLEASE REMOVE CARD");

                            break;
                        }
                    case MESSAGE_ID.M31_PIN_ENTRY:
                        {
                            TimeOutTimer.Stop();
                            logger.WriteLog("Received M31_PIN_ENTRY");
                            pinPad.MessageOffline();
                            if (PinpadResponse.PinEntryStatus == "0")
                            {
                                card.PINBlock = PinpadResponse.PinData;
                                disableCancel();
                                CreditDebitCardSale(card);
                            }
                            else
                            {
                                displayStatus("PIN ENTRY ERROR: " + PinpadResponse.PinEntryStatus);
                                totSecs = 0;
                                TimeOutTimer.Start();
                            }
                            break;
                        }
                    case MESSAGE_ID.M41_CARD_READ:
                        {
                            TimeOutTimer.Stop();
                            logger.WriteLog("Received M41_CARD_READ");
                            if (this.IsDisposed)
                            {
                                log.LogMethodExit(null);
                                return;
                            }


                            if (PinpadResponse.CardReadStatus == "0")
                            {
                                card = new Card();
                                card.EncryptedTrack1Data = PinpadResponse.Track1;
                                card.EncryptedTrack2Data = PinpadResponse.Track2;
                                card.CardNumber = PinpadResponse.MaskedCardNumber;
                                card.ExpirationYear = PinpadResponse.ExpirationDate.Substring(0, 2);
                                card.ExpirationMonth = PinpadResponse.ExpirationDate.Substring(2);

                                if (_TrxRequest.DebitCardSale == false)
                                {
                                    pinPad.MessageOffline();
                                    disableCancel();
                                    CreditDebitCardSale(card);
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(card.EncryptedTrack1Data))
                                    {
                                        displayStatus(_utilities.MessageUtils.getMessage("INVALID DEBIT CARD. PLEASE RETRY.."));
                                        totSecs = 0;
                                        firstTimeOut = secondTimeOut = false;
                                        logger.WriteLog("Sending retry card read request");
                                        pinPad.CardReadRequest();
                                        txtStatus.Invoke(new Action(() => TimeOutTimer.Start()));
                                        logger.WriteLog("Sent retry card read request");
                                    }
                                    else
                                    {
                                        displayStatus(_utilities.MessageUtils.getMessage("PLEASE ENTER YOUR PIN"));
                                        totSecs = 0;
                                        timeOut = 45;
                                        firstTimeOut = secondTimeOut = true;
                                        logger.WriteLog("Sending pin entry request");
                                        pinPad.PinEntryRequest(card.EncryptedTrack1Data);
                                        TimeOutTimer.Start();
                                        logger.WriteLog("Sent pin entry request");
                                    }
                                }
                            }
                            else
                            {
                                string s1 = _utilities.MessageUtils.getMessage("CARD READ ERROR. PLEASE RETRY..");
                                string s2 = _utilities.MessageUtils.getMessage("INSERT CARD AND WAIT FOR GREEN LIGHT");
                                string s3 = _utilities.MessageUtils.getMessage("REMOVE CARD QUICKLY");

                                displayStatus(Environment.NewLine + s1 + Environment.NewLine + Environment.NewLine +
                                                s2 + Environment.NewLine + Environment.NewLine +
                                                s3,
                                                true);

                                totSecs = 0;
                                firstTimeOut = secondTimeOut = false;
                                logger.WriteLog("Sending retry card read request");
                                pinPad.CardReadRequest();
                                txtStatus.Invoke(new Action(() => TimeOutTimer.Start()));
                                logger.WriteLog("Sent retry card read request");
                            }

                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in pin Pad MessageHandler", ex);
                enableCancel();
                displayStatus(ex.Message);
            }
            log.LogMethodExit(null);
        }

        object isnull(object o)
        {
            log.LogMethodEntry(o);
            object returnValue = o == null ? DBNull.Value : o;
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        object isnull(object o, object value)
        {
            log.LogMethodEntry(o, value);
            object returnvalue = o == null ? value : o;
            return returnvalue;
        }

        /// <summary>
        /// Update Response Message Details method
        /// </summary>
        /// <param name="objResponseMessage"></param>
        public void UpdateResponseMessageDetails(Response objResponseMessage)
        {
            List<SqlParameter> ParameterList = new List<SqlParameter>();
            string receipt = "";
            logger.WriteLog("UpdateResponseMessageDetails: checking for approved transaction to print the receipt.");
                ParameterList.Add(new SqlParameter("@TokenId", DBNull.Value));
            ParameterList.Add(new SqlParameter("@CmdStatus", objResponseMessage.ExpressResponseMessage));
            ParameterList.Add(new SqlParameter("@TextResponse", objResponseMessage.ExpressResponseMessage));
            ParameterList.Add(new SqlParameter("@AcctNo", isnull(objResponseMessage.Card.CardNumber)));
            ParameterList.Add(new SqlParameter("@CardType", _TrxRequest.DebitCardSale ? "DEBIT" : "CREDIT"));
            ParameterList.Add(new SqlParameter("@TranCode", _TrxRequest.TransactionType.Equals("PAYMENT") ? "Sale" : (_TrxRequest.TransactionType.Equals("RETURN") ? "Return" : _TrxRequest.TransactionType)));
            ParameterList.Add(new SqlParameter("@RefNo", isnull(objResponseMessage.Transaction.TransactionID)));
            ParameterList.Add(new SqlParameter("@InvoiceNo", _TrxRequest.TransactionId.ToString()));
            ParameterList.Add(new SqlParameter("@Purchase", isnull(_TrxRequest.TransactionAmount.ToString("N2"))));
            ParameterList.Add(new SqlParameter("@Authorize", isnull(objResponseMessage.Transaction.ApprovedAmount)));
            ParameterList.Add(new SqlParameter("@TransactionDatetime", DateTime.Now));
            ParameterList.Add(new SqlParameter("@AuthCode", isnull(objResponseMessage.Transaction.ApprovalNumber)));
            ParameterList.Add(new SqlParameter("@AcqRefData", isnull(objResponseMessage.HostResponseMessage)));
            ParameterList.Add(new SqlParameter("@customerCopy", receipt));
            SqlParameter result = new SqlParameter("@Result", -1);
            log.LogVariableState("@TokenId", DBNull.Value);
            log.LogVariableState("@CmdStatus", objResponseMessage.ExpressResponseMessage);
            log.LogVariableState("@TextResponse", objResponseMessage.ExpressResponseMessage);
            log.LogVariableState("@AcctNo", isnull(objResponseMessage.Card.CardNumber));
            log.LogVariableState("@CardType", _TrxRequest.DebitCardSale ? "DEBIT" : "CREDIT");
            log.LogVariableState("@TranCode", _TrxRequest.TransactionType.Equals("PAYMENT") ? "Sale" : (_TrxRequest.TransactionType.Equals("RETURN") ? "Return" : _TrxRequest.TransactionType));
            log.LogVariableState("@RefNo", isnull(objResponseMessage.Transaction.TransactionID));
            log.LogVariableState("@InvoiceNo", _TrxRequest.TransactionId.ToString());
            log.LogVariableState("@Purchase", isnull(_TrxRequest.TransactionAmount.ToString("N2")));
            log.LogVariableState("@Authorize", isnull(objResponseMessage.Transaction.ApprovedAmount));
            log.LogVariableState("@TransactionDatetime", DateTime.Now);
            log.LogVariableState("@AuthCode", isnull(objResponseMessage.Transaction.ApprovalNumber));
            log.LogVariableState("@AcqRefData", isnull(objResponseMessage.HostResponseMessage));
            log.LogVariableState("@Result", -1);
            result.Direction = ParameterDirection.Output;
            ParameterList.Add(result);

            ParameterList.Add(new SqlParameter("@MerchantID", _utilities.ParafaitEnv.POSMachine));
            ParameterList.Add(new SqlParameter("@RecordNo", ""));
            ParameterList.Add(new SqlParameter("@DSIXReturnCode", DBNull.Value));
            ParameterList.Add(new SqlParameter("@ProcessData", DBNull.Value));
            ParameterList.Add(new SqlParameter("@ResponseOrigin", DBNull.Value));
            ParameterList.Add(new SqlParameter("@UserTraceData", DBNull.Value));
            ParameterList.Add(new SqlParameter("@CaptureStatus", isnull(objResponseMessage.Card.CardLogo)));


            log.LogVariableState("@MerchantID", _utilities.ParafaitEnv.POSMachine);
            log.LogVariableState("@RecordNo", "");
            log.LogVariableState("@DSIXReturnCode", DBNull.Value);
            log.LogVariableState("@ProcessData", DBNull.Value);
            log.LogVariableState("@ResponseOrigin", DBNull.Value);
            log.LogVariableState("@UserTraceData", DBNull.Value);
            log.LogVariableState("@CaptureStatus", isnull(objResponseMessage.Card.CardLogo));

            SqlConnection sqlconn = _utilities.createConnection();
            try
            {
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.Connection = sqlconn;
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "SPInsertResponseMesage";
                sqlcommand.Parameters.AddRange(ParameterList.ToArray());
                sqlcommand.Parameters["@Result"].Direction = ParameterDirection.Output;
                sqlcommand.ExecuteNonQuery();
                _utilities.logSQLCommand("ElementPSGateway", sqlcommand);
                sqlcommand.Dispose();
                CCResponseId = sqlcommand.Parameters["@Result"].Value;
                logger.WriteLog("UpdateResponseMessageDetails: Response saved.");
            }
            catch (Exception ex)
            {
                logger.WriteLog("UpdateResponseMessageDetails: Response not saved.");
                log.Error("Error occured while inserting the respose message", ex);
                log.LogMethodExit(null, "Throwing Exception" + ex);
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
            log.LogMethodExit(null);
        }

        void EndProcess(ElementExpress.Response response)
        {
            log.LogMethodEntry(response);
            try
            {
                logger.WriteLog("EndProcess");
                ElementExpressResponse = response;

                if (response.ExpressResponseCode == "0")
                {
                    displayStatus(_utilities.MessageUtils.getMessage("APPROVED"));
                    System.Windows.Forms.Application.DoEvents();
                    print(response);
                    System.Threading.Thread.Sleep(3000);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else if (response.ExpressResponseCode == "5")
                {
                    displayStatus(_utilities.MessageUtils.getMessage("PARTIALLY APPROVED"));
                    System.Windows.Forms.Application.DoEvents();
                    print(response);
                    System.Threading.Thread.Sleep(5000);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
                else if (response.ExpressResponseCode == "20")
                {
                    displayStatus(_utilities.MessageUtils.getMessage("DECLINED"));
                    System.Windows.Forms.Application.DoEvents();
                    print(response);
                    System.Threading.Thread.Sleep(5000);
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
                else
                {
                    displayStatus(response.ExpressResponseMessage);
                    enableCancel();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in end of the process", ex);
                enableCancel();
                log.LogMethodExit(null, "Throwing Exception" + ex);
                throw ex;
            }
            finally
            {
                Close();
            }
            log.LogMethodExit(null);
        }

        void displayStatus(string text, bool leftAlign = false)
        {
            log.LogMethodEntry(text, leftAlign);
            if (txtStatus.InvokeRequired)
            {
                if (leftAlign)
                    txtStatus.Invoke(new Action(() => txtStatus.TextAlign = ContentAlignment.MiddleLeft));
                else
                    txtStatus.Invoke(new Action(() => txtStatus.TextAlign = ContentAlignment.MiddleCenter));

                txtStatus.Invoke(new Action(() => txtStatus.Text = text));
            }
            else
            {
                if (leftAlign)
                    txtStatus.TextAlign = ContentAlignment.MiddleLeft;
                else
                    txtStatus.TextAlign = ContentAlignment.MiddleCenter;

                txtStatus.Text = text;
            }

            logger.WriteLog(text);
            log.LogMethodExit(null);
        }

        void disableCancel()
        {
            log.LogMethodEntry();
            if (btnCancel.InvokeRequired)
                btnCancel.Invoke(new Action(() => btnCancel.Enabled = false));
            else
                btnCancel.Enabled = false;
            log.LogMethodExit(null);
        }

        void enableCancel()
        {
            log.LogMethodEntry();
            if (btnCancel.InvokeRequired)
                btnCancel.Invoke(new Action(() => btnCancel.Enabled = true));
            else
                btnCancel.Enabled = true;
            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            logger.WriteLog("Cancel pressed");
            pinPad.setReceiveHandler = null;
            ElementExpressResponse = new Parafait.Device.ElementExpress.Response();
            ElementExpressResponse.ExpressResponseMessage = "CANCELLED";
            displayStatus(_utilities.MessageUtils.getMessage("CANCELLED"));
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit(null);
        }

        void print(Parafait.Device.ElementExpress.Response response)
        {
            log.LogMethodEntry(response);
            string cardId = "";
            if (card != null)
                cardId = card.CardNumber;
            logger.WriteLog("print: Print method starts");
            if (!string.IsNullOrEmpty(cardId))
                cardId = new String('X', 12) + cardId.Substring(12);

            ElementExpressResponse.Card.CardNumber = cardId;
            if (card != null)
            {
                ElementExpressResponse.Card.ExpirationMonth = card.ExpirationMonth;
                ElementExpressResponse.Card.ExpirationYear = card.ExpirationYear;
            }

            if (_TrxRequest.PrintReceipt)
            {
                try
                {
                    using (System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument())
                    {
                        pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 700);
                        pd.PrintPage += (sender, e) =>
                        {
                            logger.WriteLog("PrintPage: PrintPage event starts");
                            printReceiptText(e, response, false);
                        };
                            pd.Print();
                        }
                    }
                catch(Exception ex)
                {
                    _utilities.EventLog.logEvent("PaymentGateway", 'I', "Receipt print failed.", "CC receipt", "Element", 2, "", "", _utilities.ParafaitEnv.LoginID, _utilities.ParafaitEnv.POSMachine, null);
                    logger.WriteLog("Element cc receipt print failed.");
                    log.Error("Element receipt print failed", ex);
                }
            }
            log.LogMethodExit();
        }

        void printReceiptText(System.Drawing.Printing.PrintPageEventArgs e, Response response, bool pMerchantReceipt = true)
        {
            log.LogMethodEntry(e, response, pMerchantReceipt);
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            logger.WriteLog("printReceiptText: Starts");
            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;

            string FieldValue = "";
            int x = 10;
            int y = 10;
            Font f = new Font("Courier New", 9);
            Graphics g = e.Graphics;
            int yinc = (int)g.MeasureString("SITE", f).Height;
            int pageWidth = e.PageBounds.Width - x * 2;

            FieldValue = _utilities.ParafaitEnv.SiteName;
            g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
            y += yinc;

            FieldValue = _utilities.ParafaitEnv.SiteAddress;
            if (!string.IsNullOrEmpty(FieldValue))
            {
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
                y += yinc * 3;
            }

            FieldValue = "Date      : " + DateTime.Now.ToString("MM-dd-yyyy H:mm:ss tt");
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            FieldValue = "Terminal  : " + _utilities.ParafaitEnv.POSMachine;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            FieldValue = "Invoice No: " + response.Transaction.ReferenceNumber;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            string cardId = "";
            if (card != null)
                cardId = card.CardNumber;

            if (!string.IsNullOrEmpty(cardId))
                cardId = new String('X', 12) + cardId.Substring(12);

            FieldValue = "Account   : " + cardId;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            FieldValue = "Card      : " + response.Card.CardLogo;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            FieldValue = "Entry     : SWIPE";
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;

            string amount = ElementExpressResponse.Transaction.ApprovedAmount;
            if (string.IsNullOrEmpty(amount))
                amount = (_TrxRequest.TransactionAmount == 0 ? _TrxRequest.OrigTransactionAmount : _TrxRequest.TransactionAmount).ToString("N2");

            if (_TrxRequest.DebitCardSale)
                FieldValue = "DEBIT Purchase: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + amount;
            else
                FieldValue = "CREDIT Purchase: " + _utilities.ParafaitEnv.CURRENCY_SYMBOL + amount;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc * 2;

            if (response.ExpressResponseCode == "0")
                FieldValue = "Approval Code : " + response.Transaction.ApprovalNumber;
            else if (response.ExpressResponseCode == "20")
                FieldValue = "Approval Code : " + "DECLINED";
            else
                FieldValue = "Approval Code : " + response.ExpressResponseCode + "/" + response.ExpressResponseMessage;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc;
            FieldValue = "Tran Id       : " + response.Transaction.TransactionID;
            g.DrawString(FieldValue, f, Brushes.Black, x, y);
            y += yinc * 2;

            if (_TrxRequest.TransactionType != "PAYMENT")
            {
                FieldValue = " **  " + _TrxRequest.TransactionType + "  **";
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc;
                FieldValue = "AMOUNT: " + (_TrxRequest.TransactionAmount == 0 ? _TrxRequest.OrigTransactionAmount : _TrxRequest.TransactionAmount).ToString("N2");
                g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
                y += yinc * 2;
            }

            FieldValue = @"I AGREE TO PAY ABOVE TOTAL AMOUNT ACCORDING TO CARD ISSUER AGREEMENT";
            g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc * 3), sfCenter);
            y += yinc * 4;

            FieldValue = "___________________________";
            g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
            y += yinc * 2;

            FieldValue = "THANK YOU";
            g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
            y += yinc;

            if (pMerchantReceipt)
                FieldValue = "* MERCHANT RECEIPT *";
            else
                FieldValue = "* CUSTOMER RECEIPT *";
            g.DrawString(FieldValue, f, Brushes.Black, new Rectangle(x, y, pageWidth, yinc), sfCenter);
            logger.WriteLog("printReceiptText: Ends");
            log.LogMethodExit(g);
        }


        private void ElementPSCore_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            logger.WriteLog("FormClosing");

            if (TimeOutTimer != null)
                TimeOutTimer.Stop();

            if (pinPad != null)
                pinPad.MessageOffline();
            log.LogMethodExit(null);
        }
    }
}
