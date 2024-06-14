/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCardTransaction
* Description  - frmCardTransaction 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint, Timeout crash, Validty msg changes & tax display
*2.6.0       30-Apr-2019      Nitin Pai          Adding deposit to Kiosk Screens
*2.70        1-Jul-2019       Lakshminarayana    Modified to add support for ULC cards 
*2.80        5-Sep-2019       Deeksha            Added logger methods.
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.100.0     05-Aug-2020      Guru S A           Kiosk activity log changes
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 *******************************************************************************************/

using System;
using System.Data;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
//using ParafaitAlohaIntegration;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.KioskCore.CoinAcceptor;
using Semnox.Parafait.Transaction;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmCardTransaction : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private decimal ProductPrice;
        private decimal AmountRequired;
        private decimal productTaxAmount;
        private decimal productPriceWithOutTax;
        private decimal totalCardDeposit;
        private Semnox.Parafait.Transaction.Transaction.TransactionLine firstLine = null;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = null;
        private bool printReceipt = false;
        private string imageFolder;
        private bool cancelButtonPressed = false; 
        private decimal depositTaxAmount;
        private decimal depositPriceWithOutTax;
        private CoinAcceptor coinAcceptor;
        private BillAcceptor billAcceptor;
        private CardDispenser cardDispenser; 
        private KioskStatic.configuration config = KioskStatic.config;
        private PaymentModeDTO _PaymentModeDTO;
        public TransactionPaymentsDTO trxPaymentDTO = null;
        private int billAcceptorTimeout = 0;
        private int coinAcceptorTimeout = 0;
        private Utilities Utilities = KioskStatic.Utilities;
        private readonly TagNumberParser tagNumberParser;
        private Font savTimeOutFont;
        private Font timeoutFont;
        private string previousCardNumber = "";
        private string AMOUNTFORMAT;
        private string AMOUNTFORMATWITHCURRENCYSYMBOL;
        private string NUMBERFORMAT;
        private System.Windows.Forms.Timer noteCoinActionTimer;
        //private bool canPerformNoteCoinReceivedAction = false;
        private List<ProductsDTO> activeFunds;
        private List<ProductsDTO> activeDonations;
        private Semnox.Core.Utilities.ExecutionContext executionContext;
        private const int flpCartPHeight = 585;
        private const int pnlProdSummaryHeight = 659;
        private int orginalXValueForFlpCartProducts;
        private double serviceCharges = 0;
        private double gratuityCharges = 0;
        private double totalCharges = 0;
        private string defaultMsg = string.Empty;
        private const bool cartElementsInReadMode = true;
        private const int middleAlignmentLocationX = 166;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmCardTransaction(KioskTransaction kioskTransaction, PaymentModeDTO selectedPaymentModeDTO)
        {
            log.LogMethodEntry("kioskTransaction", selectedPaymentModeDTO);
            this.executionContext = KioskStatic.Utilities.ExecutionContext;
            InitializeComponent(); 
            this.kioskTransaction = kioskTransaction;
            activeFunds = KioskHelper.GetActiveFundRaiserProducts(executionContext);
            activeDonations = KioskHelper.GetActiveDonationProducts(executionContext);
            tagNumberParser = new TagNumberParser(executionContext);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.setDefaultFont(this);
            AMOUNTFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            AMOUNTFORMATWITHCURRENCYSYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            NUMBERFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            imageFolder = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY");
            InitializeFrmCardTransaction(selectedPaymentModeDTO);
            log.LogMethodExit();
        }

        private void InitializeFrmCardTransaction(PaymentModeDTO selectedPaymentModeDTO)
        {
            log.LogMethodEntry(selectedPaymentModeDTO);
            Audio.Stop();  
            if (selectedPaymentModeDTO != null && selectedPaymentModeDTO.GatewayLookUp.Equals(PaymentGateways.NCR.ToString()))
            {
                try
                {
                    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(selectedPaymentModeDTO.GatewayLookUp);
                    paymentGateway.BeginOrder();
                }
                catch (Exception ex)
                {
                    log.Error("Payment Gateway error :" + ex.Message);
                }
            }
            firstLine = null;
            trxLines = kioskTransaction.GetActiveTransactionLines;
            if (trxLines != null && trxLines.Any())
            {
                firstLine = kioskTransaction.GetFirstNonDepositAndFnDTransactionLine; 
                if (firstLine == null)
                {
                    firstLine = trxLines[0];
                }
            }
            int productId = -1;
            if (firstLine != null)
            {
                productId = firstLine.ProductID;
            }
            _PaymentModeDTO = selectedPaymentModeDTO;
            TimerMoney.Enabled = false;
            KioskTimerSwitch(false);
            Utilities.setLanguage(this);
            lblTimeOut.Text = KioskStatic.MONEY_SCREEN_TIMEOUT.ToString("#0");
            savTimeOutFont = lblTimeOut.Font;
            timeoutFont = lblTimeOut.Font = new System.Drawing.Font(lblTimeOut.Font.FontFamily, 50, FontStyle.Bold);
            UpdatePaymentmodeAnimationVisibility();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            DisplayMessageLine(""); 
            DisplayValidtyMsg(firstLine.ProductID, kioskTransaction.GetSelectedEntitlementType);
            orginalXValueForFlpCartProducts = this.flpCartProducts.Location.X;
            RefreshCartData();
            SetDiscountButtonText();
            DisplaybtnCancel(true); 

            if (firstLine != null)
            {
                AmountRequired = (decimal)kioskTransaction.GetTrxNetAmount();
                lblBal.Text = (AmountRequired).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL); 
                lblPaid.Text = (0).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            } 
            RefreshTotals();  
            log.LogMethodExit();
        }         

        private void frmNewcard_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            Application.DoEvents(); 
            if (kioskTransaction.GetTotalPaymentsReceived() < kioskTransaction.GetTrxNetAmount()
                && kioskTransaction.GetTotalPaymentsReceived() > 0)
            {
                e.Cancel = true;
                KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit. restartValidators");
                RestartValidators();
                log.LogMethodExit("FormClosing: Inserted value > 0. Cannot exit.");
                return;
            }
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                totSecs = 0;
                e.Cancel = true;
                KioskStatic.logToFile("Closing form: billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
                return;
            }
            KioskStatic.logToFile("FormClosing: True");
            this.Hide();
            TimerMoney.Stop();
            if (noteCoinActionTimer != null)
            {
                noteCoinActionTimer.Stop();
            }
            if (billAcceptor != null)
                billAcceptor.disableBillAcceptor();
            if (coinAcceptor != null)
                coinAcceptor.disableCoinAcceptor();

            KioskStatic.baReceiveAction = null;
            KioskStatic.caReceiveAction = null;
            KioskStatic.billAcceptorDatareceived = false;
            KioskStatic.coinAcceptorDatareceived = false;

            decimal receivedAmount = kioskTransaction.GetTotalPaymentsReceived();
            decimal requiredAmount = kioskTransaction.GetTrxNetAmount();
            if (receivedAmount >= kioskTransaction.GetTrxNetAmount())
            {
                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, "Thank You"));
            }
            else
            {
                if (receivedAmount > 0)
                {
                    Application.DoEvents();
                    e.Cancel = true;
                    KioskStatic.logToFile("FormClosing: Inserted value > 0. Cannot exit. restartValidators");
                    RestartValidators();
                    log.LogMethodExit("FormClosing: Inserted value > 0. Cannot exit.");
                    return;
                }
            }

            if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD && KioskStatic.cardAcceptor != null)//GGG to be tested
            {
                KioskStatic.cardAcceptor.EjectCardFront();
                KioskStatic.cardAcceptor.BlockAllCards();
            }
            
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("StopKioskTimer Error:", ex);
            }
            KioskStatic.logToFile("Exiting money screen...");

            Audio.Stop();
            log.LogMethodExit("Exiting money screen...");
        }
         
        private void frmCardTransaction_Load(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            FrmCardTransactionLoad(true);
            log.LogMethodExit();
        }

        private void FrmCardTransactionLoad(bool firstLoad)
        {
            log.LogMethodEntry(firstLoad);
            KioskStatic.logToFile("Enter Money screen");
            log.Info("Enter Money screen");
            SetCustomizedFontColors();
            AdjustflpLocation();
            Application.DoEvents();
            KioskStatic.logToFile("Amount required: " + AmountRequired.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
            log.Info("Amount required: " + AmountRequired.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
            SetReceiptPrintOptions();

            if (firstLoad)
            {
                InitializeBillAndNoteAcceptors(); 
                InitiateNoteCoinActionTimer();
            }
            TimerMoney.Start();
            RefreshTotals();
            Audio.PlayAudio(Audio.InsertExactAmount);
            log.Info("_PaymentMode: " + _PaymentModeDTO);
            if (_PaymentModeDTO != null)
            {
                Button btnPaymentMode = new Button();
                btnPaymentMode.Tag = _PaymentModeDTO;
                TimerMoney.Start(); 
                btnCancel.Enabled = true;
                btnHome.Enabled = true;   
                btnPaymentMode_Click(btnPaymentMode, null);
                 
            }
            else
            { 
                btnCancel.Enabled = true;
                btnHome.Enabled = true; //need to be verified
            }
            previousCardNumber = "";
            log.LogMethodExit();
        } 
        private void SetReceiptPrintOptions()
        {
            log.LogMethodEntry();
            string option = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TRX_AUTO_PRINT_AFTER_SAVE"); 
            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.NONE;
            if (option.Equals("A"))
            {
                string message;
                message = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Would you like a receipt for your purchase?");
                using (frmYesNo fyn = new frmYesNo(message))
                {
                    if (fyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        printReceipt = true;
                    }
                    else
                    {
                        printReceipt = false;
                    }
                }
            }
            else if (option.Equals("Y"))
            {
                printReceipt = true;
            }
            if (printReceipt)
            {
                //if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.ASK.ToString())
                //{
                //    using (frmReceiptDeliveryModeOptions frdmo = new frmReceiptDeliveryModeOptions(executionContext, kioskTransaction))
                //    {
                //        frdmo.ShowDialog();
                //        kioskTransaction = frdmo.GetKioskTransaction;
                //    }
                //}
                //else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.PRINT.ToString())
                //{
                    kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.PRINT;
                //}
                //else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.EMAIL.ToString())
                //{
                //    using (frmGetEmailDetails fged = new frmGetEmailDetails(executionContext, kioskTransaction))
                //    {
                //        if (fged.ShowDialog() != DialogResult.OK)
                //        {
                //            //Time scenario. Procced as per config and it transaction has customer email id, email will be sent
                //            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.EMAIL;
                //        }
                //    }
                //}
            }
            log.LogMethodExit();
        }

        private void InitializeBillAndNoteAcceptors()
        {
            log.LogMethodEntry();
            try
            {
                bool enableAcceptors = true;
                CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
                if (commonBA != null && kioskTransaction.GetTotalPaymentsReceived() > 0)
                {
                    if (firstLine != null && firstLine.ProductTypeCode == ProductTypeValues.VARIABLECARD)
                    {
                        AmountRequired = ProductPrice = (decimal)kioskTransaction.GetTotalPaymentsReceived();
                    }
                    enableAcceptors = false;
                }
                decimal pendingAmount = (kioskTransaction.GetTrxNetAmount() - kioskTransaction.GetTotalPaymentsReceived());
                if (pendingAmount > 0)
                {
                    enableAcceptors = true;
                }
                else
                {
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1008);//Processing..Please wait...
                }
                if (_PaymentModeDTO != null && _PaymentModeDTO.IsCash == true && enableAcceptors)

                {
                    if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                    {
                        billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString());
                        ((NV9USB)billAcceptor).dataReceivedEvent = BillAcceptorDataReceived;
                        billAcceptor.initialize();
                        billAcceptor.AmountRemainingToBeCollected = (double)(pendingAmount > 0 ? pendingAmount : 0);
                    }
                    else if (config.baport != 0)
                    {
                        billAcceptor = KioskStatic.getBillAcceptor(KioskStatic.config.baport.ToString());
                        KioskStatic.baReceiveAction = BillAcceptorDataReceived;
                        if (billAcceptor.spBillAcceptor != null)
                        {
                            billAcceptor.spBillAcceptor.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(spBillAcceptor_DataReceived);
                        }
                        billAcceptor.initialize();
                        billAcceptor.AmountRemainingToBeCollected = (double)(pendingAmount > 0 ? pendingAmount : 0);
                        billAcceptor.requestStatus();
                    }
                    if (config.coinAcceptorport != 0 && config.coinAcceptorport > -1)
                    {
                        try
                        {
                            coinAcceptor = KioskStatic.getCoinAcceptor(KioskStatic.config.coinAcceptorport);
                            //coinAcceptor.spCoinAcceptor = KioskStatic.spCoinAcceptor;
                            coinAcceptor.AmountRemainingToBeCollected = (double)(pendingAmount > 0 ? pendingAmount : 0);
                            if (coinAcceptor.set_acceptance())
                            {
                                if (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.MICROCOIN_SP))
                                {
                                    coinAcceptor.dataReceivedEvent = CoinAcceptorDataReceived;
                                }
                                else
                                {
                                    KioskStatic.caReceiveAction = CoinAcceptorDataReceived;
                                    log.Info("KioskStatic.caReceiveAction = coinAcceptorDataReceived");
                                }
                            }
                            else
                            {
                                log.Info("KioskStatic.caReceiveAction = null");
                                KioskStatic.caReceiveAction = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            KioskStatic.logToFile("Error while Initializing Coin Acceptor: " + ex.Message);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in InitializeBillAndNoteAcceptors: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void InitiateNoteCoinActionTimer()
        {
            log.LogMethodEntry();
            noteCoinActionTimer = new System.Windows.Forms.Timer(this.components);
            noteCoinActionTimer.Interval = 100;
            noteCoinActionTimer.Tick += new EventHandler(NoteCoinActionTimerTick);
            noteCoinActionTimer.Start();
            log.LogMethodExit();
        }
        private void NoteCoinActionTimerTick(object sender, EventArgs e)
        {
            try
            {
                noteCoinActionTimer.Stop();
                RefreshTotals();
                bool hasCashToProcess = kioskTransaction.HasMoneyToProcess();
                if (hasCashToProcess)
                {
                    kioskTransaction.ProcessReceivedMoney(null);
                    CheckMoneyStatus();
                    KioskStatic.logToFile("Performing NoteCoinActionTimer action");
                    log.Debug("Performing NoteCoinActionTimer action");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in NoteCoinActionTimerTick: " + ex.Message);
            }
            finally
            {
                noteCoinActionTimer.Start();
            }
        } 
        private void btnPaymentMode_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button btn = (Button)sender;
            PaymentModeDTO paymentModeDTO = (PaymentModeDTO)btn.Tag;
            if (paymentModeDTO != null)
            {
                if (paymentModeDTO.IsCreditCard == true)
                {
                    if (kioskTransaction.GetTotalPaymentsReceived() >= kioskTransaction.GetTrxNetAmount())
                    {
                        DisplayMessageLine("Nothing to pay for");
                        CheckMoneyStatus();
                        log.LogMethodExit();
                        return;
                    }

                    KioskStatic.logToFile("Credit card clicked");
                    log.Info("Credit card clicked");
                    try
                    {
                        KioskStatic.logToFile("btnCreditCard_Click Event. ");
                        string qrCodeScanned = string.Empty;
                        TimerMoney.Stop();
                        Audio.Stop();
                        btnHome.Enabled = btnCancel.Enabled = false;// fLPPModes.Enabled = false;

                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, "Credit Card Payment.") + " " + MessageContainerList.GetMessage(executionContext, "Please wait..."));
                        Application.DoEvents();
                        if (paymentModeDTO.GatewayLookUp.Equals(PaymentGateways.NCR.ToString()))
                        {
                            try
                            {
                                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModeDTO.GatewayLookUp);
                                paymentGateway.BeginOrder();
                            }
                            catch (Exception ex)
                            {
                                log.Error("Payment Gateway error :" + ex.Message);
                            }
                            //KioskStatic.NCRAdapter.BeginOrder();
                        }
                        if (paymentModeDTO.IsQRCode.Equals("Y"))
                        {
                            using (frmScanCoupon scanCoupon = new frmScanCoupon(paymentModeDTO.GatewayLookUp.ToString(), "Scan QRCode", "QR Code", "OK", "Cancel"))
                            {
                                while (true)
                                {
                                    if (scanCoupon.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        if (!string.IsNullOrEmpty(scanCoupon.CodeScaned))
                                        {
                                            qrCodeScanned = scanCoupon.CodeScaned;
                                            break;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    } 
                                    else
                                    {
                                        qrCodeScanned = string.Empty;
                                        DialogResult = DialogResult.Cancel;
                                        this.Close();
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            qrCodeScanned = string.Empty;
                        }
                        bool isDebitCard = (sender as Control).Equals(paymentModeDTO.IsCreditCard) ? true : false;
                        kioskTransaction.ApplyCreditCardPayment(isDebitCard, _PaymentModeDTO, qrCodeScanned, printReceipt, AbortTransactionTriggered, frmOKMsg.ShowUserMessage);
                        totSecs = 0;
                        decimal totalAmountReceived = kioskTransaction.GetTotalPaymentsReceived();
                        CheckMoneyStatus();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex); 
                        DisplayMessageLine(ex.Message);
                        string errMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4650);
                        KioskStatic.logToFile(ex.Message + errMsg);
                        frmOKMsg.ShowOkMessage(ex.Message + Environment.NewLine + errMsg, true);
                       }
                    finally
                    {
                        btnHome.Enabled = true;
                        btnCancel.Enabled = true;
                        TimerMoney.Start();
                    }
                }
                else if (paymentModeDTO.IsDebitCard == true)
                {
                    if (kioskTransaction.GetTotalPaymentsReceived() >= kioskTransaction.GetTrxNetAmount())
                    {
                        DisplayMessageLine("Nothing to pay for");
                        CheckMoneyStatus();
                        log.LogMethodExit();
                        return;
                    }

                    KioskStatic.logToFile("Debit card clicked");
                    log.Info("Debit card clicked");
                    try
                    {
                        TimerMoney.Stop();
                        Audio.Stop();
                        btnHome.Enabled = btnCancel.Enabled = false;
                        DisplayMessageLine(MessageContainerList.GetMessage(executionContext, "Game Card Payment.") + " " + MessageContainerList.GetMessage(executionContext, "Please wait..."));
                        Application.DoEvents();
                        if (trxPaymentDTO != null)
                        {
                            if (trxPaymentDTO.PaymentModeId != -1)
                            {
                                if ((sender as Control).Equals(btn) ? true : false)
                                {
                                    kioskTransaction.ProcessReceivedMoney(trxPaymentDTO);
                                    totSecs = 0;
                                    CheckMoneyStatus();
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error(ex); 
                        DisplayMessageLine(ex.Message);
                    }
                    finally
                    {
                        btnHome.Enabled = true;
                        btnCancel.Enabled = true;
                        TimerMoney.Start();
                    } 
                }
                else
                {
                    CheckMoneyStatus();
                }
            }
            log.LogMethodExit();
        } 
        private void spBillAcceptor_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry();
            if (billAcceptor.spBillAcceptor != null)
            {
                System.Threading.Thread.Sleep(20);
                billAcceptor.spBillAcceptor.Read(KioskStatic.billAcceptorRec, 0, billAcceptor.spBillAcceptor.BytesToRead);
                KioskStatic.billAcceptorDatareceived = true;

                if (KioskStatic.baReceiveAction != null)
                    KioskStatic.baReceiveAction.Invoke();
            }
            log.LogMethodExit();
        }  
        private void BillAcceptorDataReceived()
        {
            log.LogMethodEntry();
            try
            {
                billMessage = "";
                KioskStatic.billAcceptorDatareceived = true;
                bool noteRecd = billAcceptor.ProcessReceivedData(KioskStatic.billAcceptorRec, ref billMessage);
                if (noteRecd)
                {

                    totSecs = 0;
                    int denomination = billAcceptor.ReceivedNoteDenomination; 
                    log.LogVariableState("BillAcceptor.ReceivedNoteDenomination", denomination);
                    kioskTransaction.AddCashNotePayment(denomination);
                    billAcceptor.ReceivedNoteDenomination = 0;
                    decimal amountReceived = kioskTransaction.GetTotalPaymentsReceived();
                    decimal netTrxAmount = kioskTransaction.GetTrxNetAmount();
                    decimal balance = netTrxAmount - amountReceived;
                    billAcceptor.AmountRemainingToBeCollected = (double)balance;
                    Card currentCard = kioskTransaction.GetTransactionPrimaryCard;
                    KioskStatic.ReceivedDenominationToActivityLog(executionContext, kioskTransaction.GetTransactionId, (currentCard == null ? "" : currentCard.CardNumber),
                                                                    denomination, KioskTransaction.GETBILLIN, KioskTransaction.GETBILLINMSG, KioskStatic.NOTE);
                    log.Info("After calling updateKioskActivityLog");
                    System.Threading.Thread.Sleep(300);
                    billAcceptor.requestStatus();
                    //canPerformNoteCoinReceivedAction = true;
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    //KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in billAcceptorDataReceived: " + ex.Message);
                log.Error(ex); 
            }
            log.LogMethodExit();
        } 
        private void CoinAcceptorDataReceived()
        {
            log.LogMethodEntry();
            try
            {
                coinMessage = "";
                KioskStatic.coinAcceptorDatareceived = true;
                bool coinRecd = coinAcceptor.ProcessReceivedData(KioskStatic.coinAcceptor_rec, ref coinMessage);
                if (coinRecd)
                {
                    totSecs = 0;
                    int denomination = coinAcceptor.ReceivedCoinDenomination; 
                    log.LogVariableState("CoinAcceptor.ReceivedCoinDenomination", denomination);
                    kioskTransaction.AddCoinPayment(denomination);
                    coinAcceptor.ReceivedCoinDenomination = 0;
                    decimal netTrxAmount = kioskTransaction.GetTrxNetAmount();
                    decimal amountReceived = kioskTransaction.GetTotalPaymentsReceived();
                    decimal balance = netTrxAmount - amountReceived;
                    coinAcceptor.AmountRemainingToBeCollected = (double)balance;
                    Card currentCard = kioskTransaction.GetTransactionPrimaryCard;
                    KioskStatic.ReceivedDenominationToActivityLog(executionContext, kioskTransaction.GetTransactionId,
                        (currentCard == null ? "" : currentCard.CardNumber),
                        denomination, KioskTransaction.GETCOININ, KioskTransaction.GETCOININMSG, KioskStatic.COIN);
                    System.Threading.Thread.Sleep(20);
                    //canPerformNoteCoinReceivedAction = true;
                    KioskStatic.logToFile("IsHandleCreated: " + this.IsHandleCreated.ToString());
                    //KioskStatic.logToFile("Set canPerformNoteCoinReceivedAction as true");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in coinAcceptorDataReceived: " + ex.Message);
            }
            log.LogMethodExit();
        } 

        private void RefreshTotals()
        {
            log.LogMethodEntry();
            try
            {
                decimal totalPaymentReceived = kioskTransaction.GetTotalPaymentsReceived();
                decimal netAmount = kioskTransaction.GetTrxNetAmount();
                lblPaid.Text = (totalPaymentReceived != 0 ? totalPaymentReceived.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL) 
                                      : (0).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                double balVal = -1;
                double.TryParse(lblBal.Text, out balVal);
                if (netAmount > totalPaymentReceived)
                {
                    lblBal.Text = (netAmount - totalPaymentReceived).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                }
                else
                {
                    lblBal.Text = (0).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                    if (balVal != 0)
                    {
                        txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1008);//Processing..Please wait...
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in refreshTotals: " + ex.Message);
            }
            log.LogMethodExit();
        }

        int toggleAcceptorMessage = 0;
        string billMessage = "";
        string coinMessage = "";
        double totSecs = 0;
        private void TimerMoney_Tick(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            TimerMoney.Stop();

            try
            {
                if (billAcceptor != null && (KioskStatic.billAcceptorDatareceived == false || billAcceptor.Working == false))
                {
                    billAcceptorTimeout++;
                    if (billAcceptorTimeout > 30)
                    {
                        billMessage = MessageContainerList.GetMessage(executionContext, 423);
                        KioskStatic.logToFile("Bill acceptor timeout / offline");
                        log.Info("Bill acceptor timeout / offline");
                    }

                    if (billAcceptorTimeout % 5 == 0)
                    {
                        billAcceptor.requestStatus();
                        if (billAcceptor.Working == false)
                        { RestartValidators(); }
                    }
                }

                //bool microSPDevice = (KioskStatic.CoinAcceptorModel.Equals(CoinAcceptor.Models.MICROCOIN_SP));
                //if (!KioskStatic.coinAcceptorDatareceived && coinAcceptor != null
                //&& (KioskStatic.caReceiveAction != null || microSPDevice))
                //{
                //    coinAcceptorTimeout++;
                //    if (coinAcceptorTimeout > 30
                //        && ((KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen)
                //              || microSPDevice))
                //    {
                //        coinMessage = MessageContainerList.GetMessage(Utilities.ExecutionContext, 424);
                //        KioskStatic.logToFile("Coin acceptor timeout / offline");
                //        log.Info("Coin acceptor timeout / offline");
                //    }

                //    if (coinAcceptorTimeout % 5 == 0)
                //    {
                //        log.Info("KioskStatic.spCoinAcceptor.IsOpen : "
                //            + (KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen));
                //        if ((KioskStatic.spCoinAcceptor != null && KioskStatic.spCoinAcceptor.IsOpen)
                //             || microSPDevice)
                //        {
                //            if (coinAcceptor != null)
                //                coinAcceptor.checkStatus();
                //        }
                //    }
                //}

                decimal moneyReceived = kioskTransaction.GetTotalPaymentsReceived();
                if (moneyReceived <=0)
                {
                    if ((billAcceptor == null || billAcceptor.ReceivedNoteDenomination == 0)
                        && (coinAcceptor == null || coinAcceptor.ReceivedCoinDenomination == 0))
                    {
                        totSecs += TimerMoney.Interval / 1000.0;
                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            if (billAcceptor != null && billAcceptor.StillProcessing())
                            {
                                totSecs = 0;
                                KioskStatic.logToFile("billAcceptor stillProcessing. reset timer clock");
                                log.Info("billAcceptor stillProcessing. reset timer cock");
                                TimerMoney.Start();
                                log.LogMethodExit();
                                return;
                            }
                            if (billAcceptor != null)
                                billAcceptor.disableBillAcceptor();
                            if (coinAcceptor != null)
                                coinAcceptor.disableCoinAcceptor(); 
                        }
                    }

                    Application.DoEvents();
                    moneyReceived = kioskTransaction.GetTotalPaymentsReceived();
                    if (moneyReceived <= 0)
                    {
                        int secondsRemaining = KioskStatic.MONEY_SCREEN_TIMEOUT - (int)totSecs;
                        if (secondsRemaining == 10)
                        {

                            lblTimeOut.Text = secondsRemaining.ToString("#0");
                            if (TimeOut.AbortTimeOut(this))
                            {
                                totSecs = 0;
                            }
                            else
                            {
                                totSecs = KioskStatic.MONEY_SCREEN_TIMEOUT - 3;
                            }
                        }

                        if (totSecs > KioskStatic.MONEY_SCREEN_TIMEOUT)
                        {
                            if (billAcceptor != null && billAcceptor.StillProcessing())
                            {
                                totSecs = 0;
                                KioskStatic.logToFile("billAcceptor stillProcessing. reset timer clock");
                                log.Info("billAcceptor stillProcessing. reset timer cock");
                                log.LogMethodExit();
                                return;
                            }
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 425));
                            lblTimeOut.Font = savTimeOutFont;
                            lblTimeOut.Text = "Time Out";
                            btnHome.Enabled = false;
                            btnCancel.Enabled = false;
                            KioskTimerSwitch(true);
                            StartKioskTimer();
                            KioskStatic.logToFile("Operation Timed out...");
                            log.LogMethodExit("Operation Timed out...");
                            return;
                        }
                        else
                        {
                            if (secondsRemaining > 0)
                            {
                                lblTimeOut.Font = timeoutFont;
                                lblTimeOut.Text = secondsRemaining.ToString("#0");
                            }
                        }
                    }
                    else
                    {
                        KioskStatic.logToFile("Timer Money Tick: ac.totalValue <= 0 is false, restartValidators");
                        RestartValidators();
                        log.LogMethodExit("ac.totalValue > 0");
                        return;
                    }
                }
                else
                {
                    if (kioskTransaction.SelectedProductType == KioskTransaction.GETNEWCARDTYPE)
                    {
                        lblTimeOut.Text = "";
                    }
                    else if (kioskTransaction.SelectedProductType == KioskTransaction.GETCHECKINCHECKOUTTYPE)//if (Function == "C")
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = "Transaction in Progress";
                    }
                    else
                    {
                        lblTimeOut.Font = savTimeOutFont;
                        if (ServerDateTime.Now.Millisecond / 500 > 0)
                            lblTimeOut.Text = "";
                        else
                            lblTimeOut.Text = MessageContainerList.GetMessage(executionContext, 427);
                    }
                }

                if (billMessage.EndsWith("inserted") || billMessage.EndsWith("accepted") || billMessage.EndsWith("rejected"))
                {
                    log.LogVariableState("billMessage", billMessage);
                    DisplayMessageLine(billMessage.Replace("inserted", MessageContainerList.GetMessage(executionContext,"inserted"))
                                                  .Replace("accepted", MessageContainerList.GetMessage(executionContext,"accepted"))
                                                  .Replace("rejected", MessageContainerList.GetMessage(executionContext,"rejected"))
                                                  .Replace("Bill", MessageContainerList.GetMessage(executionContext,"Bill"))
                                                  .Replace("Denomination", MessageContainerList.GetMessage(executionContext,"Denomination")));
                    Audio.Stop();
                }
                else if (coinMessage.EndsWith("accepted") || coinMessage.EndsWith("rejected"))
                {
                    log.LogVariableState("coinMessage", coinMessage);
                    DisplayMessageLine(coinMessage.Replace("accepted", MessageContainerList.GetMessage(executionContext,"accepted"))
                                                  .Replace("rejected", MessageContainerList.GetMessage(executionContext,"rejected"))
                                                  .Replace("Denomination", MessageContainerList.GetMessage(executionContext,"Denomination")));
                    Audio.Stop();
                }
                else if (billMessage.StartsWith("Insert") && coinMessage.StartsWith("Insert"))
                {
                    log.Info(billMessage + " " + MessageContainerList.GetMessage(executionContext, 428));
                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 428)); //"Insert Bank Notes and Coins"
                }
                else
                {
                    if (toggleAcceptorMessage > 0)
                    {
                        if (billAcceptor != null)
                        {
                            if (billMessage != "")
                            {
                                if (billAcceptor.criticalError)
                                {
                                    log.Info("billAcceptor.criticalError:" + billMessage);
                                    using (frmOKMsg f = new frmOKMsg(MessageContainerList.GetMessage(executionContext, billMessage) + ". " + MessageContainerList.GetMessage(executionContext, 441)))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else if (billAcceptor.overpayReject)
                                {
                                    log.Info("billAcceptor.overpayReject:" + billMessage);
                                    billAcceptor.overpayReject = false;
                                    using (frmOKMsg f = new frmOKMsg("Bill refused. Please insert exact amount"))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else
                                {
                                    log.Info(billMessage);
                                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, billMessage));
                                }
                            }
                            else
                            {
                                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 419, AmountRequired.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL)));
                            }
                        }
                    }
                    else
                    {
                        if (coinAcceptor != null)
                        {
                            if (coinMessage != "")
                            {
                                if (coinAcceptor.criticalError)
                                {
                                    log.Info("coinAcceptor.criticalError:" + coinMessage);
                                    using (frmOKMsg f = new frmOKMsg(MessageContainerList.GetMessage(executionContext, coinMessage) + ". " + MessageContainerList.GetMessage(executionContext, 441)))
                                    {
                                        f.ShowDialog();
                                    }
                                }
                                else
                                {
                                    log.Info(coinMessage);
                                    DisplayMessageLine(MessageContainerList.GetMessage(executionContext, coinMessage));
                                }
                            }
                            else
                            {
                                DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 419, AmountRequired.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL)));
                            }
                        }
                    }

                    toggleAcceptorMessage++;
                    if (toggleAcceptorMessage > 40)
                        toggleAcceptorMessage = -40;
                }

                if (kioskTransaction.GetTotalPaymentsReceived() < kioskTransaction.GetTrxNetAmount())
                {
                    TimerMoney.Start();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                TimerMoney.Start();
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                btnHome.Enabled = true;
                btnCancel.Enabled = true;
            }
            log.LogMethodExit();
        }

        private Object thisLock = new Object();
        private void CheckMoneyStatus()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("checkMoneyStatus() - enter");
            TimeOut.Abort();
            KioskStatic.logToFile("checkMoneyStatus() -Abort TimeOut");
            lock (thisLock)
            {
                try
                {
                    KioskStatic.logToFile("checkMoneyStatus()- before checking TotalValue greater than or equals to AmountRequired");
                    log.Info("Before checking TotalValue greater than or equals to AmountRequired");
                    if (kioskTransaction.GetTotalPaymentsReceived() >= kioskTransaction.GetTrxNetAmount())
                    {
                        string msg = "checkMoneyStatus: Total Payment Received = " + kioskTransaction.GetTotalPaymentsReceived().ToString(AMOUNTFORMAT) + ", AmountRequired = " + kioskTransaction.GetTrxNetAmount().ToString(AMOUNTFORMAT);
                        KioskStatic.logToFile(msg);
                        log.Info(msg);
                        TimerMoney.Stop();

                        //if (kioskTransaction.cancelPressed)
                        if (cancelButtonPressed)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - Cancel button Pressed");
                            log.Info("Cancel button Pressed");
                            log.LogMethodExit();
                            return;
                        }


                        btnHome.Enabled = false;
                        btnCancel.Enabled = false;
                        Application.DoEvents();

                        if (billAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling billAcceptor");
                            log.Info("Calling billAcceptor.disableBillAcceptor");
                            System.Threading.Thread.Sleep(300);
                            billAcceptor.disableBillAcceptor();
                        }
                        if (coinAcceptor != null)
                        {
                            KioskStatic.logToFile("checkMoneyStatus() - disabling coinAcceptor");
                            log.Info("Calling coinAcceptor.disableBillAcceptor");
                            System.Threading.Thread.Sleep(300);
                            coinAcceptor.disableCoinAcceptor();
                        }

                        if (kioskTransaction.HasTempCards())
                        {
                            if (KioskStatic.DispenserReaderDevice != null)
                            {
                                KioskStatic.logToFile("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                                log.Info("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                            }
                            else
                            {
                                KioskStatic.logToFile("Dispenser Reader not present");
                                log.Info("Dispenser Reader not present");
                            }
                            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                            KioskStatic.logToFile("Dispenser Type: " + cardDispenser.GetType().ToString());
                            log.Info("Dispenser Type: " + cardDispenser.GetType().ToString());
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 429, ""));
                            Application.DoEvents();
                        }
                        kioskTransaction.ExecuteTransaction(cardDispenser, DisplayMessageLine, frmOKMsg.ShowUserMessage, ShowThankYou, AbortTransactionTriggered, printReceipt);
                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        string msg = "checkMoneyStatus: Total Payment Received = " + kioskTransaction.GetTotalPaymentsReceived().ToString(AMOUNTFORMAT) + " and it is less than AmountRequired = " + kioskTransaction.GetTrxNetAmount().ToString(AMOUNTFORMAT);
                        KioskStatic.logToFile(msg);
                        log.Info(msg);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    DisplayMessageLine(ex.Message);
                    KioskStatic.logToFile(ex.Message + "-" + ex.StackTrace);
                    btnHome.Enabled = true;
                    btnCancel.Enabled = true;
                }
                finally
                {
                    KioskStatic.logToFile("checkMoneyStatus() - exit");
                }
            }
            log.LogMethodExit();
        } 
        private void RestartValidators()
        {
            log.LogMethodEntry();
            log.Info("_PaymentMode: " + _PaymentModeDTO);
            decimal netAmount = kioskTransaction.GetTrxNetAmount();
            decimal amountReceived = kioskTransaction.GetTotalPaymentsReceived();
            if (_PaymentModeDTO != null && _PaymentModeDTO.IsCreditCard == false)
            {
                if (netAmount > amountReceived)
                {
                    if (billAcceptor != null)
                    {
                        if (KioskStatic.BillAcceptorModel.Equals(BillAcceptor.Models.NV9USB))
                        {
                            KioskStatic.logToFile("RestartValidators - DisableBillAcceptor");
                            billAcceptor.disableBillAcceptor();
                            billAcceptor.initialize();
                            billAcceptor.AmountRemainingToBeCollected = (double)(netAmount - amountReceived);
                        }
                        else
                        {
                            billAcceptor.requestStatus();
                        }
                    }

                    if (coinAcceptor != null)
                    {
                        coinAcceptor.set_acceptance();
                        coinAcceptor.AmountRemainingToBeCollected = (double)(netAmount - amountReceived);
                    }
                }
                else
                {
                    KioskStatic.logToFile("RestartValidators - skip restart, balance amount is 0");
                }
            } 
            btnHome.Enabled = true;
            btnCancel.Enabled = true;
            TimerMoney.Start();
            log.LogMethodExit();
        }

        int cancelPressCount = 0;
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool cancelButtonClicked = false;
            CancelAction(cancelButtonClicked);
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool cancelButtonClicked = true;
            CancelAction(cancelButtonClicked);
            log.LogMethodExit();
        }
        private void CancelAction(bool cancelButtonClicked)
        {
            log.LogMethodEntry(cancelButtonClicked);
            try
            {
                TimerMoney.Stop(); 
                cancelButtonPressed = true;
                btnHome.Enabled = false;
                btnCancel.Enabled = false; 

                Audio.Stop();
                int iters = 100;
                while (iters-- > 0)
                {
                    Thread.Sleep(20);
                    Application.DoEvents();
                }

                KioskStatic.logToFile("Cancel pressed");
                log.Info("Cancel pressed");

                if ((billAcceptor != null && billAcceptor.ReceivedNoteDenomination > 0) // in the process of accepting
                    || (coinAcceptor != null && coinAcceptor.ReceivedCoinDenomination > 0))
                { 
                    cancelButtonPressed = false;
                    btnHome.Enabled = true;
                    btnCancel.Enabled = true;
                    if (cancelPressCount++ < 3)
                    {
                        TimerMoney.Start();
                        log.LogMethodExit();
                        return;
                    }
                }

                if (billAcceptor != null && billAcceptor.StillProcessing())
                { 
                    cancelButtonPressed = false;
                    btnHome.Enabled = true;
                    btnCancel.Enabled = true;
                    DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4682));
                    log.Info("billAcceptor.StillProcessing");
                    KioskStatic.logToFile("billAcceptor.StillProcessing");
                    TimerMoney.Start();
                    log.LogMethodExit();
                    return;
                }
                KioskStatic.logToFile("Cancel button Disabling bill acceptor ");

                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();
                if (coinAcceptor != null)
                    coinAcceptor.disableCoinAcceptor(); 

                TimerMoney.Stop();
                Application.DoEvents();

                if (kioskTransaction.GetTrxNetAmount() > kioskTransaction.GetTotalPaymentsReceived()
                    && kioskTransaction.GetTotalPaymentsReceived() > 0)
                {
                    string message;
                    int abort = 0;
                    if (kioskTransaction.SelectedProductType == KioskTransaction.GETNEWCARDTYPE)
                    {
                        ////You have inserted &1. Do you want to continue?
                        //message = MessageContainerList.GetMessage(executionContext, 442,
                        //    kioskTransaction.GetTotalPaymentsReceived().ToString(amountFormatWithCurrencySymbol));
                        message = MessageContainerList.GetMessage(executionContext, 4197,
                              kioskTransaction.GetTotalPaymentsReceived().ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                        //You have inserted &1. Do you want to proceed with Abort?
                        using (frmYesNo f = new frmYesNo(message))
                        {
                            if (f.ShowDialog() == System.Windows.Forms.DialogResult.No)
                            {
                                abort = AttemptToIssueNewVariableCardForPartialAmount();
                            }
                            else if (f.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                            {
                                abort = 1;
                            }
                            else
                            {
                                abort = 2; 
                            }
                        } 
                    }
                    else if (kioskTransaction.SelectedProductType == KioskTransaction.GETCHECKINCHECKOUTTYPE)
                    {                        
                        message = MessageContainerList.GetMessage(executionContext, 4197,
                              kioskTransaction.GetTotalPaymentsReceived().ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                        //You have inserted &1. Do you want to proceed with Abort?
                        using (frmYesNo fop = new frmYesNo(message))
                        {
                            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                            {
                                abort = 1; 
                            }
                            else if (fop.ShowDialog() == System.Windows.Forms.DialogResult.No)
                            {
                                abort = 0;
                            }
                            else
                            {
                                abort = 2; 
                            }
                        } 
                    }
                    else
                    {
                        abort = 0;
                        Card card = kioskTransaction.GetTransactionPrimaryCard;
                        if (card != null && card.CardStatus == "ISSUED" && card.CardNumber.StartsWith("T") == false)
                        {
                            //You have inserted &1. Do you want to Top-up with this amount? 
                            decimal amount = kioskTransaction.GetTotalPaymentsReceived() - kioskTransaction.GetCCSurchargeAmount;
                            message = MessageContainerList.GetMessage(executionContext, 443, amount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));

                            using (frmYesNo fyesNo = new frmYesNo(message))
                            {
                                DialogResult dr = fyesNo.ShowDialog();
                                if (dr == System.Windows.Forms.DialogResult.Yes)
                                {
                                    if (KioskStatic.DispenserReaderDevice != null)
                                    {
                                        KioskStatic.logToFile("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                                        log.Info("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                                    }
                                    else
                                    {
                                        KioskStatic.logToFile("Dispenser Reader not present");
                                        log.Info("Dispenser Reader not present");
                                    }
                                    kioskTransaction.CancelExistingProductsAndAddVariableRechargeCard();
                                    kioskTransaction.ExecuteTransaction(cardDispenser, DisplayMessageLine, frmOKMsg.ShowUserMessage, ShowThankYou, AbortTransactionTriggered, printReceipt);
                                    abort = 2;
                                }
                                else if (dr == DialogResult.No)
                                {
                                    message = "Are you sure you want to exit?";
                                    using (frmYesNo fop = new frmYesNo(message))
                                    {
                                        if (fop.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                        {
                                            abort = 1;
                                        }
                                        else //No or timeout
                                        {
                                            abort = 2;
                                        }
                                    }
                                }
                                else //timeout
                                {
                                    abort = 2; 
                                }
                            }
                        }
                        else if (card != null && card.CardStatus == "ISSUED" && card.CardNumber.StartsWith("T") == true)
                        {
                            abort = AttemptToIssueNewVariableCardForPartialAmount();
                        }  
                        else if (card != null && card.CardStatus == "NEW")
                        {
                            message = "Are you sure you want to exit?";
                            using (frmYesNo fop = new frmYesNo(message))
                            {
                                if (fop.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                                {
                                    abort = 1;
                                }
                                else //No or timeout
                                {
                                    abort = 2;
                                }
                            }
                        }
                    }
                    if (abort == 1)
                    { 
                        string abortMsg = (kioskTransaction.SelectedProductType == KioskTransaction.GETRECHAREGETYPE
                                           ? KioskTransaction.GETABORTRECHAREGE : KioskTransaction.GETABORTTRANSACTION);

                        Card card = kioskTransaction.GetTransactionPrimaryCard;
                        if (card != null && card.CardNumber.StartsWith("T"))
                        {
                            card = null;
                        }
                        string cardNo = (card != null ? card.CardNumber : string.Empty);
                        KioskStatic.UpdateKioskActivityLog(executionContext, KioskTransaction.GETABORTTRANSACTION, abortMsg, cardNo, kioskTransaction.GetTransactionId, kioskTransaction.GetGlobalKioskTrxId);
                        string msg = "Abort recharge... Money entered: " + kioskTransaction.GetTotalPaymentsReceived().ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                        KioskStatic.logToFile(msg);
                        log.Info(msg); 
                        kioskTransaction.ClearTransaction(frmOKMsg.ShowUserMessage);
                        frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(executionContext, 441));

                        cancelButtonClicked = false;
                        closeForm(cancelButtonClicked);
                    }
                    else
                    {
                        cancelButtonPressed = false;
                        RestartValidators();
                    }
                }
                else
                {
                    closeForm(cancelButtonClicked);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnCancel_Click(): " + ex.Message + ":" + ex.StackTrace);
            } 
            log.LogMethodExit();
        }

        private int AttemptToIssueNewVariableCardForPartialAmount()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            int abort = 0;
            if (Utilities.getParafaitDefaults("ALLOW_VARIABLE_NEW_CARD_ISSUE").Equals("Y"))
            {
                int CardCount = 1;//GGG
                                  //Do you want to purchase new card(s) with the inserted amount?
                message = MessageContainerList.GetMessage(executionContext, 934,
                    (kioskTransaction.GetTotalPaymentsReceived() - kioskTransaction.GetCCSurchargeAmount).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL));
                using (frmYesNo ff = new frmYesNo(message))
                {
                    DialogResult dr = ff.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.Cancel) // time out
                    {
                        abort = 2; 
                    }
                    else if (dr == System.Windows.Forms.DialogResult.No)
                    {
                        abort = 1;
                    }
                    else
                    {
                        //kioskTransaction.cancelPressed = false;
                        cancelButtonPressed = false;
                        if (firstLine.MultiPointConversionRequired)
                        {
                            object o = Utilities.executeScalar(@"select top 1 isnull(CardCount, 1)
                                                                    from products p, product_type pt 
                                                                    where p.product_type_id = pt.product_type_id 
                                                                    and pt.product_type in ('NEW', 'CARDSALE')
                                                                    and price <= @amount 
                                                                    order by price desc",
                                    new SqlParameter("@amount", (kioskTransaction.GetTotalPaymentsReceived() - kioskTransaction.GetCCSurchargeAmount)));

                            int savCardCount = CardCount;
                            if (o == null)
                                CardCount = 1;
                            else
                                CardCount = Math.Min(CardCount, Convert.ToInt32(o));
 
                            if (CardCount != savCardCount)
                            {
                                //You will receive &1 Cards instead of &2
                                using (frmOKMsg fom = new frmOKMsg(MessageContainerList.GetMessage(executionContext, 935, CardCount, savCardCount)))
                                {
                                    fom.ShowDialog();
                                }
                            }
                        }
                        else
                        {
                            if (Math.Ceiling((kioskTransaction.GetTotalPaymentsReceived() - kioskTransaction.GetCCSurchargeAmount) / CardCount) < ProductPrice)
                            {
                                CardCount = 1;
                            }
                        }
                        //GGG
                        kioskTransaction.CancelExistingProductsAndAddNewVariableCard();
                        firstLine = kioskTransaction.GetFirstNonDepositAndFnDTransactionLine;
                        if (firstLine != null)
                        {
                            DisplayValidtyMsg(firstLine.ProductID, (firstLine.MultiPointConversionRequired
                                             ? KioskTransaction.TIME_ENTITLEMENT
                                             : KioskTransaction.CREDITS_ENTITLEMENT));
                        }
                        if (kioskTransaction.HasTempCards())
                        {
                            if (KioskStatic.DispenserReaderDevice != null)
                            {
                                KioskStatic.logToFile("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                                log.Info("Dispenser Reader is present: " + KioskStatic.DispenserReaderDevice.GetType().ToString());
                            }
                            else
                            {
                                KioskStatic.logToFile("Dispenser Reader not present");
                                log.Info("Dispenser Reader not present");
                            }
                            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
                            KioskStatic.logToFile("Dispenser Type: " + cardDispenser.GetType().ToString());
                            log.Info("Dispenser Type: " + cardDispenser.GetType().ToString());
                            DisplayMessageLine(MessageContainerList.GetMessage(executionContext, 429, ""));
                            Application.DoEvents();
                        }
                        kioskTransaction.ExecuteTransaction(cardDispenser, DisplayMessageLine, frmOKMsg.ShowUserMessage, ShowThankYou, AbortTransactionTriggered, printReceipt);
                        abort = 2; 
                    }
                }
            }
            else
            {
                abort = 1;
            }
            log.LogMethodExit(abort);
            return abort;
        }

        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            Application.DoEvents();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Application.DoEvents();
            StopKioskTimer(); 
            if (kioskTransaction.GetTotalPaymentsReceived() > 0 && kioskTransaction.GetTrxNetAmount() > kioskTransaction.GetTotalPaymentsReceived())
            {
                KioskStatic.logToFile("KioskTimer_Tick, ac.totalValue > 0, restartValidators");
                RestartValidators();
                log.LogMethodExit();
                return;
            }

            TimerMoney.Stop();
            KioskStatic.logToFile("Exit Timer ticked");
            log.Info("Exit Timer ticked");
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                totSecs = 0;
                KioskStatic.logToFile("Timer Money: billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
                return;
            } 
            bool cancelBtnClicked = false;
            closeForm(cancelBtnClicked);
            log.LogMethodExit();
        }

        void ShowThankYou(bool receiptPrinted, bool receiptEmailed)
        {
            log.LogMethodEntry(receiptPrinted, receiptEmailed);
            try
            {
                string trxNumber = kioskTransaction.GetTransactionNumber;
                string message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4121);//Transaction Successful. Thank You.
                string printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(executionContext, 498) //PLEASE COLLECT THE RECEIPT
                            : MessageContainerList.GetMessage(Utilities.ExecutionContext, 5000); //Please note down Trx Number on screen for future reference
                string source = kioskTransaction.SelectedProductType;
                if (kioskTransaction.SelectedProductType == KioskTransaction.GETCHECKINCHECKOUTTYPE)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine checkInLine = trxLines.Find(tl => tl.LineValid && tl.ProductTypeCode == ProductTypeValues.CHECKIN);
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4149,
                                                                   (checkInLine != null ? checkInLine.ProductName : ""));
                    printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 4148) : "";
                 }
                else if (kioskTransaction.SelectedProductType == KioskTransaction.GETNEWCARDTYPE
                         || kioskTransaction.SelectedProductType == KioskTransaction.GETRECHAREGETYPE)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine cardLine = trxLines.Find(tl => tl.LineValid && tl.card != null
                    && tl.ProductTypeCode != ProductTypeValues.CARDDEPOSIT);
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 952,
                                                                   (cardLine != null ? cardLine.ProductName : ""));
                    printMsg = (receiptPrinted) ? MessageContainerList.GetMessage(Utilities.ExecutionContext, 498) //PLEASE COLLECT THE RECEIPT
                        : MessageContainerList.GetMessage(Utilities.ExecutionContext, 5000); //Please note down Trx Number on screen for future reference
                }
                if (receiptEmailed)//override printMsg
                {
                    printMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Transaction receipt is emailed to you");
                }

                bool isWaiverSigningPending = kioskTransaction.WaiverMappingIsPending();
                if (isWaiverSigningPending)//override message
                {
                    //In order to complete the transaction, please visit the POS counter to finish waiver signing requirements
                    message = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4839); 
                }
                using (frmTransactionSuccess frm = new frmTransactionSuccess(message, printMsg, trxNumber, source, isWaiverSigningPending))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in ShowThankYou: " + ex.Message);
            }
            KioskStatic.logToFile("Exit money screen");
            log.Info("Exit money screen");
            bool cancelBtnClicked = false;
            closeForm(cancelBtnClicked);
            log.LogMethodExit();
        }
        void showOK(string message, bool enableTimeOut = true)
        {
            log.LogMethodEntry(message, enableTimeOut);
            KioskStatic.logToFile(message);
            using (frmOKMsg frm = new frmOKMsg(message, enableTimeOut))
            {
                frm.ShowDialog();
            }
            log.LogMethodExit();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            log.LogMethodExit(true);
            return true;
        }
        private void closeForm(bool cancelButtonClicked)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            if (billAcceptor != null && billAcceptor.StillProcessing())
            {
                totSecs = 0;
                DisplayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4682));
                KioskStatic.logToFile("Timer Money: billAcceptor stillProcessing. reset timer clock");
                log.Info("billAcceptor stillProcessing. reset timer cock");
                log.LogMethodExit();
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            if (cancelButtonClicked)
            {
                this.Close();
            }
            else
            {
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                }
            }
            log.LogMethodExit();
        }
        private void frmCardTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Card Transaction form closed event");
            if (_PaymentModeDTO != null && _PaymentModeDTO.GatewayLookUp.Equals(PaymentGateways.NCR.ToString()))
            {
                try
                {
                    PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(_PaymentModeDTO.GatewayLookUp);
                    paymentGateway.EndOrder();
                }
                catch (Exception ex)
                {
                    log.Error("Payment Gateway error :" + ex.Message);
                }
            }
            try
            {
                if (billAcceptor != null)
                    billAcceptor.disableBillAcceptor();
                if (coinAcceptor != null)
                    coinAcceptor.disableCoinAcceptor();
            }
            catch (Exception ex)
            {
                log.Error("Bill/Coin Acceptor disable Error:", ex);
            }
            try
            {
                StopKioskTimer();
            }
            catch(Exception ex)
            {
                log.Error("StopKioskTimer Error:", ex);
            }
            log.LogMethodExit();
        }         
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCardTransaction");
            try
            {
                foreach (Control c in pnlProductSummary.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardTransactionSummaryHeaderTextForeColor;//Summary Panel header
                    }
                } 
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CardTransactionFooterTextForeColor;
                //this.lblTotalToPayText.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayHeaderTextForeColor;//Total to pay header label
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.CardTransactionCPVlaiditTextForeColor;//product cp validity
                //this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.CardTransactionTotalToPayInfoTextForeColor;//Total to pay info label
                this.lblPaidText.ForeColor = KioskStatic.CurrentTheme.CardTransactionAmountPaidHeaderTextForeColor;//Total to pay info label
                this.lblPaid.ForeColor = KioskStatic.CurrentTheme.CardTransactionAmountPaidInfoTextForeColor;//Total to pay info label
                this.lblBalanceToPayText.ForeColor = KioskStatic.CurrentTheme.CardTransactionBalanceToPayHeaderTextForeColor;//Total to pay info label
                this.lblBal.ForeColor = KioskStatic.CurrentTheme.CardTransactionBalanceToPayInfoTextForeColor;//Total to pay info label
                //this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CardTransactionBtnHomeTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CardTransactionCancelButtonTextForeColor;//Cancel button  
                this.lblTimeOut.ForeColor = KioskStatic.CurrentTheme.CardTransactionLblTimeOutTextForeColor;

                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage; 
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton; 
                lblTimeOut.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall; 
                pbxPaymentModeAnimation.Image = ThemeManager.CurrentThemeImages.InsertCashAnimation; 
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.pnlProductSummary.BackgroundImage = ThemeManager.CurrentThemeImages.TablePurchaseSummary;
                this.bigVerticalScrollCartProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);

                this.lblTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblChargeAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblTaxesHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblDiscountAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblGrandTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;

                this.lblDiscountInfo.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblCharges.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblDiscount.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblGrandTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTax.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;

            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCardTransaction: " + ex.Message);
            }
            log.LogMethodExit();
        }    
        private void UpdatePaymentmodeAnimationVisibility()
        {
            log.LogMethodEntry();
            if (_PaymentModeDTO != null && _PaymentModeDTO.IsCash)
            {
                pbxPaymentModeAnimation.Visible = true;
            }
            else
            {
                pbxPaymentModeAnimation.Visible = false;
            }
            log.LogMethodExit();
        }   
        private void AbortTransactionTriggered()
        {
            log.LogMethodEntry();
            bool cancelButtonClicked = false;
            closeForm(cancelButtonClicked);
            log.LogMethodExit();
        }

        public void GroupAndDisplayTrxLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            try
            {
                this.pnlProductSummary.SuspendLayout();
                this.flpCartProducts.SuspendLayout();
                this.flpCartProducts.Controls.Clear();
                //List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> displayTrxLines = KioskHelper.GroupDisplayTrxLines(trxLines);
                List<List<KioskHelper.LineData>> displayTrxLineData = KioskHelper.GroupDisplayTrxLinesNew(trxLines);
                List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> displayTrxLines = KioskHelper.ConvertTrxLineData(displayTrxLineData);
                AddLinesToDisplay(displayTrxLines);
                //AddNonCardLinesToDisplay(displayTrxLines);
                AddChargesToDisplay();
                // display trx total
                lblTotal.Text = (kioskTransaction.GetTransactionAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                // display tax 
                lblTax.Text = (kioskTransaction.GetTrxTaxAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                AddDiscountDetailsToDisplay();
                // display grand total
                lblGrandTotal.Text = (kioskTransaction.GetTrxNetAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                //AdjustflpSize();
                AdjustflpLocation();
            }
            finally
            {
                this.flpCartProducts.ResumeLayout(true);
                this.pnlProductSummary.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void AddLinesToDisplay(List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> groupedTrxLines)
        {
            log.LogMethodEntry();
            for (int i = 0; i < groupedTrxLines.Count; i++) // display card lines
            {
                if (groupedTrxLines[i].Exists(tl => tl.LineValid))
                {
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxCardProducts = groupedTrxLines[i];
                    usrCtrlCart usrCtrlCart = CreateUsrCtlElement(trxCardProducts);
                    this.flpCartProducts.Controls.Add(usrCtrlCart);
                }
            }
            log.LogMethodExit();
        }
        private void AdjustflpLocation()
        {
            log.LogMethodEntry();
            int xVal = orginalXValueForFlpCartProducts;
            if (bigVerticalScrollCartProducts.Visible == false)
            {
                int adjustFact = (bigVerticalScrollCartProducts.Width / 2) + 4;
                xVal = orginalXValueForFlpCartProducts + adjustFact;
            }
            this.lblCartProductName.Location = new Point(xVal, lblCartProductName.Location.Y);
            this.lblCartProductTotal.Location = new Point(xVal + this.lblCartProductName.Width + 2, lblCartProductTotal.Location.Y);
            this.flpCartProducts.Location = new Point(xVal, flpCartProducts.Location.Y);
            if ((_PaymentModeDTO != null && _PaymentModeDTO.IsCash) == false)
            {
                this.pnlProductSummary.Location = new Point(middleAlignmentLocationX, this.pnlProductSummary.Location.Y);
            }
            log.LogMethodExit();
        }
        private void AddChargesToDisplay()
        {
            log.LogMethodEntry();
            // display charges
            serviceCharges = kioskTransaction.GetTransactionServiceCharges();
            gratuityCharges = kioskTransaction.GetTransactionGratuityAmount();
            totalCharges = 0;
            totalCharges = serviceCharges + gratuityCharges;
            lblCharges.Text = totalCharges.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            log.LogMethodExit();
        }
        private void AddDiscountDetailsToDisplay()
        {
            log.LogMethodEntry();
            double discountAmount = kioskTransaction.GetTransactionDiscountsAmount();
            lblDiscount.Text = discountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            log.LogMethodExit();
        }
        private void SetDiscountButtonText()
        {
            log.LogMethodEntry();
            DiscountsSummaryDTO discountsSummaryDTO = kioskTransaction.GetAppliedCouponDiscountSummary();
            SetBtnDiscountText(discountsSummaryDTO);
            log.LogMethodExit();
        }
        private void SetBtnDiscountText(DiscountsSummaryDTO discountsSummaryDTO)
        {
            log.LogMethodEntry();
            this.lblDiscountInfo.Text = string.Empty;
            if (discountsSummaryDTO != null)
            {
                lblDiscount.Text = discountsSummaryDTO.DiscountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                decimal finalPrice = kioskTransaction.GetTrxNetAmount();
                lblTotal.Text = finalPrice.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                //btnDiscount.Text = MessageContainerList.GetMessage(executionContext, "Remove Coupon");
                //btnDiscount.Tag = "cancelDiscount";
                //couponNumber = discountsSummaryDTO.CouponNumbers.First();
                string part1 = MessageContainerList.GetMessage(executionContext, "Discount") + ": ";
                string part2 = " " + MessageContainerList.GetMessage(executionContext, "applied");
                lblDiscountInfo.Text = part1 + discountsSummaryDTO.DiscountName + part2;
            }
            log.LogMethodExit();
        }

        private usrCtrlCart CreateUsrCtlElement(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            ResetKioskTimer();
            usrCtrlCart usrCtrlElement = null;
            try
            {
                usrCtrlElement = new usrCtrlCart(executionContext, trxLines, cartElementsInReadMode);
                //usrCtrlElement.CancelSelectedLines += new usrCtrlCart.cancelSelectedLines(CancelSelectedLines);
                usrCtrlElement.RefreshCartData += new usrCtrlCart.refreshCartData(RefreshCartData);
            }
            catch (Exception ex)
            {
                string msg = "Error in CreateUsrCtlElement() of cart screen";
                log.Error(msg);
                KioskStatic.logToFile(msg + " : " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit(usrCtrlElement);
            return usrCtrlElement;
        }  
        private void DisplayValidtyMsg(int productId, string selectedEntitlementType)
        {
            log.LogMethodEntry(productId, selectedEntitlementType);
            //if (kioskTransaction.ShowCartInKiosk == false)
            //{
                string validityMsg = "";
                int lineCount = 0;
                if (productId > -1)
                {
                    validityMsg = KioskStatic.GetProductCreditPlusValidityMessage(productId, selectedEntitlementType);
                    lineCount = Regex.Split(validityMsg, "\r\n").Count();
                }
                if (lineCount > 2)
                {
                    lblProductCPValidityMsg.AutoEllipsis = true;
                    string[] validityMsgList = Regex.Split(validityMsg, "\r\n");
                    lblProductCPValidityMsg.Text = validityMsgList[0] + "\r\n" + validityMsgList[1];
                    System.Windows.Forms.ToolTip validtyMsgToolTip = new ToolTip();
                    validtyMsgToolTip.SetToolTip(lblProductCPValidityMsg, validityMsg);
                }
                else
                {
                    lblProductCPValidityMsg.Text = validityMsg;
                }
                AdjustProductSummaryPanel();
            //}
            //else
            //{
            //    lblProductCPValidityMsg.Visible = false;
            //}
            log.LogMethodExit();
        }
        private void AdjustProductSummaryPanel()
        {
            log.LogMethodEntry();
            //int heightAdjFactor = lblProductCPValidityMsg.Height + 3;
            //pnlProductSummary.SuspendLayout();
            //flpCartProducts.SuspendLayout();
            //flpCartProducts.Size = new Size(flpCartProducts.Width, flpCartPHeight - heightAdjFactor);
            //bigVerticalScrollCartProducts.Size = new Size(bigVerticalScrollCartProducts.Width, flpCartPHeight - heightAdjFactor);
            //pnlProductSummary.Size = new Size(pnlProductSummary.Width, pnlProdSummaryHeight - heightAdjFactor);
            //flpCartProducts.ResumeLayout(true);
            //pnlProductSummary.ResumeLayout(true);
            log.LogMethodExit();
        } 
        private void RefreshCartData()
        {
            log.LogMethodEntry();
            try
            {
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> transactionLines = kioskTransaction.GetActiveTransactionLines;
                GroupAndDisplayTrxLines(transactionLines);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Refreshing Cart Data", ex);
                KioskStatic.logToFile("Errow while Refreshing Cart Data: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void DisplaybtnCancel(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnCancel.Visible = switchValue;
            log.LogMethodExit();
        }
    }
}
