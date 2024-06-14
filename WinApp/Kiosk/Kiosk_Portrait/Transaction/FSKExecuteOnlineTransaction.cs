/********************************************************************************************
* Project Name - Parafait_Kiosk - FSKExecuteOnlineTransaction
* Description  - FSKExecuteOnlineTransaction 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A            Created for Online Transaction in Kiosk changes 
*2.6.0       07-Mar-2019      Guru S A            OTP validation check updates *
* 2.70       01-Jul-2019      Lakshminarayana     Modified to add support for ULC cards
* 2.80       09-Sep-2019      Deeksha             Added logger methods.
* 2.70.2     13-Nov-2019      Jinto Thomas        Modified SetBarCodeScanner method
*2.80        06-Nov-2019      Girish Kundar       Ticket printer integration enhancement
*2.90.0      23-Jul-2020      Jinto Thomas        Modified: Sending Purchase notification by creating messaing request
*2.100       25-Sep-2020      Nitin Pai           Modified: Messaging Client Id instead of name
*2.100       23-Oct-2020      Dakshakh Raj        Modified: Kiosk auto refund of issued cards in the "Retrieve My Purchase" section   
*2.100       01-jan-2021      Guru S A            Allow wristband print when card dispener is not enabled
*2.120       17-Apr-2021      Guru S A            Wristband printing flow enhancements
*2.130.0     09-Jul-2021      Dakshak             Theme changes to support customized Font ForeColor
*2.130.9     16-Jun-2022      Guru S A            Execute online transaction changes in Kiosk
*2.150.1     22-Feb-2023      Vignesh Bhat        Kiosk Cart Enhancements
*2.150.5     28-Sep-2023      Vignesh Bhat        Modified: Printing Execute online receipts based on "EXECUTE_ONLINE_RECEIPT_DELIVERY_MODE" configuration 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Semnox.Parafait.KioskCore.CardDispenser;
using Semnox.Parafait.logger;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Communication;
using System.Threading;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device;
using Semnox.Parafait.Languages;
using System.Threading.Tasks;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using System.Reflection;
using System.Security.Principal;

namespace Parafait_Kiosk
{
    public partial class FSKExecuteOnlineTransaction : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CardDispenser cardDispenser;
        string dispenserMessage = "";
        bool isDispenserCardReaderValid = false;
        DeviceClass barcodeScannerDevice = null;
        //Semnox.Parafait.logger.Monitor kioskMonitor;
        Semnox.Parafait.logger.Monitor cardDispenserMonitor;
        Semnox.Core.Utilities.ExecutionContext machineUserContext;
        Utilities utilities;
        //int tickCount = 0;
        //int homeAudioInterval = 0;
        bool showTransactionIdField;
        TextBox currentTextBox;
        string previousCardNumber;
        public delegate void IsHomeForm(bool isHome);
        public IsHomeForm isHomeForm;
        private readonly TagNumberParser tagNumberParser;
        //private bool singleFunctionMode;
        private POSPrinterDTO rfidPrinterDTO = null;
        private int kioskGlobalTrxId = -1;
        private string[] displayCOlumnList = new string[] { "productName", "lineQty", "lineAmount", "lineCardNo", "lineProductDetails", "Selected" };
        private FiscalPrinter fiscalPrinter = null;
        private string numberFormat;
        private List<KeyValuePair<string, List<TransactionLineDTO>>> printableLineDetails;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        private KioskTransaction onlineTransaction = null;

        public FSKExecuteOnlineTransaction(Semnox.Core.Utilities.ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            KioskStatic.logToFile("Launching Execute Online transaction form");
            machineUserContext = executionContext;
            GetExecuteTransactionConfig();
            utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            lblStatus.Text = string.Empty;
            //GetCoverPageConfigs();
            SetTextForFormCOntrols();
            kioskGlobalTrxId = -1;
            SetStyles();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            rfidPrinterDTO = KioskStatic.GetRFIDPrinter(machineUserContext, utilities.ParafaitEnv.POSMachineId);
            InitForm();
            txtMessage.AutoSize = false;
            txtMessage.AutoEllipsis = true;
            tagNumberParser = new TagNumberParser(KioskStatic.Utilities.ExecutionContext);
            InitializeKeyboard();
            SetCustomizedFontColors();
            numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "NUMBER_FORMAT");
            utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void GetExecuteTransactionConfig()
        {
            log.LogMethodEntry();
            showTransactionIdField = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CONSIDER_ID_FOR_EXECUTE_TRANSACTION") == "Y");
            KioskStatic.logToFile("Show Transaction Id Field: " + showTransactionIdField);
            log.LogMethodExit();
        }
        private void SetClientLogo()
        {
            log.LogMethodEntry();
            try
            {
                if (pbClientLogo.Image == null &&
                    System.IO.File.Exists(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png"))
                {
                    pbClientLogo.Image = Image.FromFile(Application.StartupPath + @"\Media\Images\HomeScreenClientLogo.png");
                }
                if (pbClientLogo.Image != null)
                {
                    pbClientLogo.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }
        private void SetStyles()
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            log.LogMethodExit();
        }
        void InitForm()
        {
            log.LogMethodEntry();
            StopKioskTimer();
            SetTheme();
            SetupDevices();
            StartKioskTimer();
            this.Activate();
            //if (KioskStatic.debugMode == false) && singleFunctionMode)
            //{
            //    Cursor.Hide();
            //}
            lblSiteName.Text = KioskStatic.SiteHeading;
            if (string.IsNullOrWhiteSpace(lblSiteName.Text))
            {
                lblSiteName.Visible = false;
            }
            else
            {
                lblSiteName.Visible = true;
            }
            SetDGVStyle();
            log.LogMethodExit();
        }
        private void SetTheme()
        {
            log.LogMethodEntry();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.FSKExecuteOnlineTransactionBackgroundImage);
            //pbClientLogo.Image = ThemeManager.CurrentThemeImages.HomeScreenClientLogo;
            btnGetTransactionDetails.BackgroundImage = ThemeManager.CurrentThemeImages.GetTransactionDetailsButton;
            btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            btnExecute.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            panelPurchase.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            //btnPrint.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            mainPanel.Location = new Point(31, 293);
            if (ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo != null)
            {
                pbSemnox.Image = ThemeManager.CurrentThemeImages.HomeScreenSemnoxLogo;
            }
            else
            {
                pbSemnox.Image = Properties.Resources.semnox_logo;
            }
            KioskStatic.setDefaultFont(this);
            if (KioskStatic.CurrentTheme == null ||
                (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                txtTransactionOTP.ForeColor = Color.Black;
                txtTransactionId.ForeColor = Color.Black;
                dgvTransactionHeader.ForeColor = Color.Black;
                dgvTransactionLines.ForeColor = Color.Black;
            }
            else
            {
                txtTransactionOTP.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                txtTransactionId.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                dgvTransactionHeader.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                dgvTransactionLines.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            log.LogMethodExit();
        }
        private void SetupDevices()
        {
            log.LogMethodEntry();
            SetupTopUpReader();
            SetupDispenserReader();
            if (KioskStatic.config.dispport > 0)
            {
                InitiateCardDispenser();
            }
            SetBarCodeScanner();
            log.LogMethodExit();
        }
        private void SetupTopUpReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.TopUpReaderDevice != null)
            {
                KioskStatic.TopUpReaderDevice.Register(TopUpCardScanCompleteEventHandle);
                List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                KioskStatic.logToFile(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
                log.Info(this.Name + ": Register topup card reader device. Call back list count: " + listCount.ToString());
            }
            log.LogMethodExit();
        }
        private void SetupDispenserReader()
        {
            log.LogMethodEntry();
            if (KioskStatic.DispenserReaderDevice != null && !KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
                KioskStatic.logToFile(this.Name + ": " + KioskStatic.CardDispenserModel.ToString() + " dispenser");
                log.Info(this.Name + ": " + KioskStatic.CardDispenserModel.ToString() + " dispenser");
            }
            else if (KioskStatic.CardDispenserModel.Equals(CardDispenser.Models.SCT0M0))
            {
                isDispenserCardReaderValid = true;
                KioskStatic.logToFile(this.Name + ": Sankyo dispenser");
                log.Info(": Sankyo dispenser");
            }
            log.LogMethodExit();
        }
        private void InitiateCardDispenser()
        {
            log.LogMethodEntry();
            cardDispenser = KioskStatic.getCardDispenser(KioskStatic.config.dispport.ToString());
            //log.Info("singleFunctionMode: " + singleFunctionMode.ToString());
            //if (singleFunctionMode)
            //{
            cardDispenserMonitor = new Semnox.Parafait.logger.Monitor(Semnox.Parafait.logger.Monitor.MonitorAppModule.CARD_DISPENSER);
            //}
            KioskStatic.logToFile("Card Dispenser is initiated. Port is " + KioskStatic.config.dispport.ToString());
            log.LogMethodExit();
        }
        private void SetBarCodeScanner()
        {
            log.LogMethodEntry();
            string barcardPID = KioskStatic.Utilities.getParafaitDefaults("USB_BARCODE_READER_PID");
            string barcardVID = KioskStatic.Utilities.getParafaitDefaults("USB_BARCODE_READER_VID");
            log.LogVariableState("barcardPID", barcardPID);
            log.LogVariableState("barcardVID", barcardVID);

            if (string.IsNullOrEmpty(barcardPID) == false && string.IsNullOrEmpty(barcardVID) == false)
            {
                try
                {
                    barcodeScannerDevice = DeviceContainer.RegisterBarcodeScanner(KioskStatic.Utilities.ExecutionContext, Application.OpenForms["frmHome"], BarCodeScanCompleteEventHandle);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    //Unable to Activate Barcode Scanner. Please Contact Staff
                    ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 4845));
                    KioskStatic.logToFile("Error registering barcode device. VID is " + barcardPID + " . PID is " + barcardPID + " . Error: " + ex.Message);
                }
            }
            else
            {
                log.Debug("Skipping barcode scanner registration as VID/PID details are not set in the configuration for the kiosk");
                KioskStatic.logToFile("Skipping barcode scanner registration as VID/PID details are not set in the configuration for the kiosk");
            }
            log.LogMethodExit();
        }
        private async void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    string scannedBarcode = KioskStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, KioskStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, KioskStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                    if (currentTextBox != null)
                    {
                        currentTextBox.Text = scannedBarcode;
                    }
                    else
                    {
                        txtTransactionOTP.Text = scannedBarcode;
                    }
                    ClosePopUps();
                    if (String.IsNullOrEmpty(txtTransactionOTP.Text) == false || String.IsNullOrEmpty(txtTransactionId.Text) == false)
                    {
                        bool transactionProcessingIsComplete = await PerformGetTransactionDetails();
                        if (transactionProcessingIsComplete)
                        {
                            string msg = MessageContainerList.GetMessage(machineUserContext, 4556, scannedBarcode);
                            //Transaction with &1 is already Issued. Please retry with a different OTP or Id.
                            ShowUserAlert(msg);
                            ClearScreen();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile("Error in BarCodeScanCompleteEventHandle. Error: " + ex.Message);
            }
            log.LogMethodEntry();
        }
        private void TopUpCardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, KioskStatic.Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        KioskStatic.logToFile(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    KioskStatic.logToFile(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string lclCardNumber = tagNumber.Value;
                if ((sender as DeviceClass).GetType().ToString().Contains("KeyboardWedge"))
                    lclCardNumber = KioskStatic.ReverseTopupCardNumber(lclCardNumber);
                HandleTopUpCardRead(lclCardNumber, sender as DeviceClass);
            }
            log.LogMethodExit();
        }
        private void ClosePopUps()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                for (int i = Application.OpenForms.Count - 1; i > 1; i--)
                {

                    if (Application.OpenForms[i].Name == "frmOKSMsg"
                          || Application.OpenForms[i].Name == "frmTimeOutCounter"
                          || Application.OpenForms[i].Name == "frmTimeOut")
                    {
                        Application.OpenForms[i].Visible = false;
                        Application.OpenForms[i].Close();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
            }
            log.LogMethodExit();
        }
        void HandleTopUpCardRead(string inCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(inCardNumber, readerDevice);
            try
            {
                KioskStatic.logToFile("Execute Online Transaction Page Tap: " + inCardNumber);
                log.LogVariableState("Execute Online Transaction Page Tap: ", inCardNumber);
                //if (Application.OpenForms.Count > 1)
                //{
                //    KioskStatic.logToFile("Not in Execute Online Transaction Page. Ignored.");
                //    log.LogVariableState("Not in Execute Online Transaction Page. Ignored.", Application.OpenForms.Count);
                //    log.LogMethodExit();
                //    return;
                //}
                //Card card = null;
                //if (KioskStatic.Utilities.ParafaitEnv.MIFARE_CARD)
                //{
                //    try
                //    {
                //        if (KioskStatic.cardAcceptor != null)
                //        {
                //            KioskStatic.cardAcceptor.EjectCardFront();
                //            KioskStatic.cardAcceptor.BlockAllCards();
                //        }
                //    }
                //    catch (Exception ex) { log.Error(ex); }
                //}
                //else
                //{
                //    card = new Card(readerDevice, inCardNumber, "", utilities);
                //}
                //if (card != null)
                //{
                //    if (card.CardStatus == "NEW")
                //    {
                //        KioskStatic.logToFile("NEW Card tapped. Ignore");
                //        log.LogVariableState("NEW Card tapped. Ignored.", card.CardStatus);
                //        log.LogMethodExit();
                //        return;
                //    }

                //    if (card.customerDTO != null)
                //    {
                //        StopKioskTimer();
                //        txtMessage.Text = utilities.MessageUtils.getMessage(463, card.customerDTO.FirstName);
                //    }
                //    else
                //    {
                //        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
                //    }

                //    KioskStatic.logToFile(txtMessage.Text);

                //    if (card.vip_customer == 'N')
                //    {
                //        card.getTotalRechargeAmount();
                //        if ((card.credits_played >= utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS && utilities.ParafaitEnv.MINIMUM_SPEND_FOR_VIP_STATUS > 0)
                //             || (card.TotalRechargeAmount >= utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS && utilities.ParafaitEnv.MINIMUM_RECHARGE_FOR_VIP_STATUS > 0))
                //        {
                //            using (frmOKMsg fok = new frmOKMsg(utilities.MessageUtils.getMessage(451, KioskStatic.VIPTerm)))
                //            {
                //                fok.ShowDialog();
                //            }
                //        }
                //    }
                //    bool isManagerCard = frmHome.CheckIsThisCardIsManagerCard(machineUserContext, inCardNumber);
                //    if (card.technician_card == 'Y' || isManagerCard == true)
                //    {
                //        KioskStatic.logToFile("Tech card OR isManagerCard: " + card.technician_card + " isManagerCard: "+ (isManagerCard? "Yes": "No"));
                //        log.LogVariableState("Tech Card", card.technician_card);
                //        log.LogVariableState("isManagerCard", isManagerCard);
                //        using (frmAdmin adminForm = new frmAdmin(machineUserContext, isManagerCard))
                //        {
                //            if (adminForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //            {
                //                KioskStatic.logToFile("Exit from Admin screen");
                //                log.LogVariableState("Exit from Admin screen", card);
                //                CloseApplication();
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        void CloseApplication()
        {
            log.LogMethodEntry();
            for (int openForm = Application.OpenForms.Count - 1; openForm >= 0; openForm--)
            {
                Application.OpenForms[openForm].Visible = false;
                Application.OpenForms[openForm].Close();
            }
            log.LogMethodExit();
        }

        private void SetDGVStyle()
        {
            log.LogMethodEntry();
            dgvTransactionHeader.Columns["trxDate"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvTransactionLines.Columns["lineQty"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvTransactionLines.Columns["lineAmount"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            SetDGVTransactionHeaderRowHeight();
            SetDGVTransactionLineRowHeight();
            log.LogMethodExit();
        }
        private void FSKExecuteOnlineTransaction_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetFocus();
            HideKeyboardObject();
            DisplayAssemblyVersion();
            //if (singleFunctionMode) //This is the home
            //{
            //    if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)
            //    {
            //        if (KioskStatic.CurrentTheme.SplashAfterSeconds == 0)//Playpass1:Starts
            //        {
            //            ResetKioskTimer();
            //        }
            //        else
            //        {
            //            SetKioskTimerTickValue(KioskStatic.CurrentTheme.SplashAfterSeconds);
            //            ResetKioskTimer();
            //        }
            //    }
            //    else
            //        ResetKioskTimer();

            //    if (!KioskStatic.questStatus)
            //    {
            //        string msg = MessageContainerList.GetMessage(machineUserContext, 1692);
            //        KioskStatic.logToFile(msg);
            //        DisplayMessageLine(msg);
            //    }
            //    kioskMonitor = new Semnox.Parafait.logger.Monitor();
            //    kioskMonitor.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1693));
            //}
            ShowHideTransactionIDField();
            AdjustButtonLocation();
            previousCardNumber = "";
            txtTransactionOTP.Text = "";
            txtTransactionId.Text = "";
            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
            log.LogMethodExit();
        }
        private void ShowHideTransactionIDField()
        {
            log.LogMethodEntry();
            if (!showTransactionIdField)
            {
                this.lblTransactionId.Visible = false;
                this.txtTransactionId.Visible = false;
                this.pnlTrasactionId.Visible = false;
                this.btnGetTransactionDetails.Location = new Point(this.pnlTransactionOTP.Location.X + this.pnlTransactionOTP.Width + this.btnCancel.Width / 4, this.btnGetTransactionDetails.Location.Y);
                this.btnShowKeyPad.Location = new Point(this.btnGetTransactionDetails.Location.X + this.btnGetTransactionDetails.Width + 70, this.btnGetTransactionDetails.Location.Y - 20);
            }
            log.LogMethodExit();
        }
        private void AdjustButtonLocation()
        {
            log.LogMethodEntry();
            int widthFactor = this.Width / 2;
            int adjustmentFactor = 106;
            int xPoint = widthFactor - this.btnCancel.Width - adjustmentFactor;
            this.btnCancel.Location = new Point(xPoint, this.btnCancel.Location.Y);
            adjustmentFactor = 140;
            xPoint = xPoint + this.btnCancel.Width + adjustmentFactor;
            this.btnExecute.Location = new Point(xPoint, this.btnExecute.Location.Y);
            //xPoint = xPoint + btnCancel.Width + sizeFactor;
            //this.btnPrint.Location = new Point(xPoint, this.btnPrint.Location.Y);
            log.LogMethodExit();
        }
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //if (singleFunctionMode)
                //{
                //    if ((ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || KioskStatic.CurrentTheme.SplashScreenImage != null))
                //    {
                //        int tickSecondsRemaining = GetKioskTimerSecondsValue();
                //        tickSecondsRemaining--;
                //        setKioskTimerSecondsValue(tickSecondsRemaining);

                //        if (tickSecondsRemaining <= 0)
                //        {
                //            StopKioskTimer();
                //            KioskStatic.logToFile("Calling splash");
                //            using (frmSplashScreen splashScreen = new frmSplashScreen())
                //            {
                //                splashScreen.ShowDialog();
                //            }
                //        }
                //    }
                //    if (this.Equals(Form.ActiveForm) || (Form.ActiveForm != null && Form.ActiveForm.Name != null && Form.ActiveForm.Name.Equals("frmSplashScreen")))
                //    {
                //        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
                //        tickCount++;
                //        if (tickCount % 300 == 0) // refresh variable every 5 minutes
                //        {
                //            kioskMonitor.Post(Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO, MessageContainerList.GetMessage(machineUserContext, 1694));
                //            KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                //            KioskStatic.logToFile("Refresh defaults: getParafaitDefaults()");
                //            log.LogVariableState("Kiosk app running.., Refresh defaults: getParafaitDefaults()", null);
                //            KioskStatic.getParafaitDefaults();
                //            tickCount = 0;
                //        }
                //    }
                //}
                //else
                //{
                int tickSecondsRemaining = GetKioskTimerSecondsValue();
                tickSecondsRemaining--;
                setKioskTimerSecondsValue(tickSecondsRemaining);
                if (tickSecondsRemaining == 10)
                {
                    if (TimeOut.AbortTimeOut(this))
                    {
                        ResetKioskTimer();
                    }
                    else
                    {
                        tickSecondsRemaining = 0;
                    }
                }
                if (tickSecondsRemaining <= 0)
                {
                    this.Close();
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in KioskTimer_Tick: " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }
        public override void Form_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //if (singleFunctionMode)
                //{
                //    if (Application.OpenForms.Count > 1)
                //    {
                //        try
                //        {
                //            Application.OpenForms[Application.OpenForms.Count - 1].Focus();
                //        }
                //        catch (Exception ex)
                //        {
                //            log.Error(ex);
                //        }
                //        log.LogMethodExit();
                //        return;
                //    }
                //    if (ThemeManager.CurrentThemeImages.SplashScreenBackgroundImage != null || ThemeManager.CurrentThemeImages.SplashScreenImage != null)//Modification on 17-Dec-2015 for introducing new theme
                //    {
                //        if (KioskStatic.CurrentTheme.SplashAfterSeconds == 0)
                //        {
                //            ResetKioskTimer();
                //        }
                //        else
                //        {
                //            SetKioskTimerTickValue(KioskStatic.CurrentTheme.SplashAfterSeconds);
                //            ResetKioskTimer();
                //        }
                //    }//Playpass1:Ends
                //    this.TopMost = true;
                //    this.TopMost = false;
                //    this.Focus();
                //    this.Activate();
                //    StartKioskTimer();
                //    ResetKioskTimer();
                //    KioskStatic.logToFile(MessageContainerList.GetMessage(machineUserContext, 1694));
                //    log.LogVariableState(MessageContainerList.GetMessage(machineUserContext, 1694), null);
                //    if (Application.OpenForms.Count == 2)
                //    {
                //        if (Audio.soundPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                //            PlayHomeScreenAudio();
                //    }
                //}
                //else
                //{
                StartKioskTimer();
                ResetKioskTimer();
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("FSKExecuteOnlineTransaction_Activated: " + ex.Message);
                StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void FSKExecuteOnlineTransaction_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                //if (singleFunctionMode)
                //{
                //    if ((int)e.KeyChar == 3)
                //    {
                //        Cursor.Show();
                //    }
                //    else if ((int)e.KeyChar == 8)
                //    {
                //        Cursor.Hide();
                //    }
                //    else
                //    {
                //        e.Handled = true;
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in FSK Execute Online Transaction screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(mainPanel.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in FSK Execute Online Transaction screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(mainPanel.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblSiteName.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in FSK Execute Online Transaction screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Disposing keyboard in FSK Execute Online Transaction screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void FSKExecuteOnlineTransaction_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (barcodeScannerDevice != null)
                {
                    barcodeScannerDevice.UnRegister();
                    barcodeScannerDevice.Dispose();
                }
                if (KioskStatic.TopUpReaderDevice != null)
                {
                    KioskStatic.TopUpReaderDevice.UnRegister();
                    List<EventHandler> eventList = (KioskStatic.TopUpReaderDevice != null ? KioskStatic.TopUpReaderDevice.GetCallBackList : new List<EventHandler>());
                    int listCount = eventList != null && eventList.Any() ? eventList.Count : 0;
                    KioskStatic.logToFile(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                    log.Info(this.Name + ": UnRegister topup card reader device. Call back list count: " + listCount.ToString());
                }
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //if (singleFunctionMode)
            //{
            //    isHomeForm(true);
            //}
            KioskStatic.logToFile(this.Name + ": Form closed");
            log.LogMethodExit();
        }
        private void ShowUserAlert(string messageForUser, bool enableTimeOut = true)
        {
            log.LogMethodEntry(messageForUser, enableTimeOut);
            try
            {
                ResetKioskTimer();
                using (frmOKMsg frm = new frmOKMsg(messageForUser, enableTimeOut))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            try
            {
                txtMessage.Text = message;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void TxtTrasactionOTP_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        private void TxtTrasactionId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        private void BtnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetFocus()
        {
            log.LogMethodEntry();
            try
            {
                if (String.IsNullOrEmpty(txtTransactionOTP.Text))
                {
                    txtTransactionOTP.Focus();
                }
                else if (showTransactionIdField && String.IsNullOrEmpty(txtTransactionId.Text))
                {
                    txtTransactionId.Focus();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetTextForFormCOntrols()
        {
            log.LogMethodEntry();
            btnGetTransactionDetails.Text = MessageContainerList.GetMessage(machineUserContext, "Search");
            btnCancel.Text = MessageContainerList.GetMessage(machineUserContext, "Back");//((singleFunctionMode) ? MessageContainerList.GetMessage(machineUserContext, "Clear") : MessageContainerList.GetMessage(machineUserContext, "Back"));
            btnExecute.Text = MessageContainerList.GetMessage(machineUserContext, 4559);//Issue
            //btnPrint.Text = MessageContainerList.GetMessage(machineUserContext, "Print");
            lblTransactionOTP.Text = MessageContainerList.GetMessage(machineUserContext, "Transaction OTP") + ":";
            lblTransactionId.Text = MessageContainerList.GetMessage(machineUserContext, "Transaction Id") + ":";
            lblCustomerName.Text = MessageContainerList.GetMessage(machineUserContext, "Customer");
            lblTrxDate.Text = MessageContainerList.GetMessage(machineUserContext, "Date");
            lblTrxNo.Text = MessageContainerList.GetMessage(machineUserContext, "Transaction No");
            lblTrxReferenceNo.Text = MessageContainerList.GetMessage(machineUserContext, "Transaction Id");
            lblTransactionDetails.Text = MessageContainerList.GetMessage(machineUserContext, "Transaction Details");
            lblLineCardNo.Text = MessageContainerList.GetMessage(machineUserContext, "Card Number");
            lblLineAmount.Text = MessageContainerList.GetMessage(machineUserContext, "Amount");
            lblLineQuantity.Text = MessageContainerList.GetMessage(machineUserContext, "Qty");
            lblLineProduct.Text = MessageContainerList.GetMessage(machineUserContext, "Product");
            lblProductDetail.Text = MessageContainerList.GetMessage(machineUserContext, "Detail");
            lblStatus.Text = String.Empty;
            log.LogMethodExit();
        }
        //private void GetCoverPageConfigs()
        //{
        //    log.LogMethodEntry();
        //    bool fskSalesEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_SALES_OPTION") == "Y");
        //    bool fskExecuteOnlineTrxEnabled = (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "KIOSK_ENABLE_EXECUTE_TRANSACTION_OPTION") == "Y");
        //    singleFunctionMode = !(fskSalesEnabled && fskExecuteOnlineTrxEnabled);
        //    log.LogMethodExit();
        //}
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in FSKExecuteOnlineTransaction");
            try
            {
                foreach (Control c in panelPurchase.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.FskExecutePnlPurchaseDetailsHeadersTextForeColor;
                    }
                    if (c.Name.Equals("lblTransactionDetails"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.FskExecutePnlPurchaseHeaderTextForeColor;
                    }
                }

                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineLblSiteNameTextForeColor;
                this.lblTransactionOTP.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineLblTransactionOTPTextForeColor;
                this.txtTransactionOTP.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTxtTransactionOTPTextForeColor;
                this.lblTransactionId.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineLblTransactionIdTextForeColor;
                this.txtTransactionId.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlinePnlTrasactionIdTextForeColor;
                this.btnGetTransactionDetails.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineBtnGetTransactionDetailsTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineBtnCancelTextForeColor;
                this.btnExecute.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineBtnCancelTextForeColor;
                //this.btnPrint.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTxtMessageTextForeColor;
                this.lblStatus.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineBtnGetTransactionDetailsTextForeColor;
                this.lblAppVersion.ForeColor = KioskStatic.CurrentTheme.ApplicationVersionForeColor;
                this.bigVerticalScrollTransactionLines.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);

                if (KioskStatic.CurrentTheme == null ||
                (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
                {
                    dgvTransactionHeader.ForeColor = Color.Black;
                    dgvTransactionLines.ForeColor = Color.Black;
                }
                else
                {
                    dgvTransactionHeader.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTransactionHeaderInfoTextForeColor;
                    this.dgvTransactionHeader.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTransactionHeaderInfoTextForeColor;
                    dgvTransactionLines.ForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTransactionLinesInfoTextForeColor;
                    this.dgvTransactionLines.RowsDefaultCellStyle.SelectionForeColor = KioskStatic.CurrentTheme.FskExecuteOnlineTransactionLinesInfoTextForeColor;
                }
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in FSKExecuteOnlineTransactions: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private async void BtnGetTransactionDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Get Transaction Details button clicked");
                bool transactionProcessingIsComplete = await PerformGetTransactionDetails();
                if (transactionProcessingIsComplete)
                {
                    string code = string.IsNullOrWhiteSpace(txtTransactionOTP.Text) == false ? txtTransactionOTP.Text : txtTransactionId.Text;
                    string msg = MessageContainerList.GetMessage(machineUserContext, 4556, code);
                    //Transaction with &1 is already Issued. Please retry with a different OTP or Id.
                    ShowUserAlert(msg);
                    ClearScreen();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUserAlert(ex.Message);
            }
            log.LogMethodExit();
        }

        private async Task<bool> PerformGetTransactionDetails()
        {
            log.LogMethodEntry();
            bool transactionProcessingIsComplete = false;
            bool inputValidationError = false;
            try
            {
                KioskStatic.logToFile("Calling PerformGetTransactionDetails");
                BeforeBackgroundOperation();
                this.Cursor = Cursors.WaitCursor;
                kioskGlobalTrxId = -1;
                printableLineDetails = new List<KeyValuePair<string, List<TransactionLineDTO>>>();
                ResetKioskTimer();
                StopKioskTimer();
                //SetEnableButtonState(false);
                //btnPrint.Enabled = false;
                previousCardNumber = "";
                CardDispenserStatusCheck();
                StopKioskTimer();
                CheckCardDispenserDependency();
                try
                {
                    ValidateSearchParamters();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    inputValidationError = true;
                    throw;
                }
                List<TransactionDTO> onlineTransactionDTOList = await GetTransactionDetails();
                if (onlineTransactionDTOList == null || onlineTransactionDTOList.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1670));
                }
                else
                {
                    StopKioskTimer();
                    printableLineDetails = await GetPrintableLineDetails(onlineTransactionDTOList[0]);
                    IEnumerable<TransactionLineDTO> displayableTransactionLines = GetDisplayableTransactionLines(onlineTransactionDTOList);
                    if (IsVirtualStore() == false)
                    {
                        this.kioskTransaction = new KioskTransaction(KioskStatic.Utilities, onlineTransactionDTOList[0].TransactionId);
                        kioskTransaction.IsOnlineTransaction = true;
                        if (kioskTransaction != null && kioskTransaction.WaiverMappingIsPending())
                        {
                            CompleteWaiverSigningProcess();
                            bool isPending = kioskTransaction.WaiverMappingIsPending(); //confirm again if waiver signing is pending
                            if (isPending)
                            {
                                //1507 - Waiver Signing Pending., 441 - Please contact our staff
                                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 1507) + " " + MessageContainerList.GetMessage(machineUserContext, 441));
                            }
                        }
                    }
                    StopKioskTimer();
                    LoadDataGrid(onlineTransactionDTOList);
                    SelectEligibleLines();
                    //if (dgvTransactionLines.Rows.Count > 0)
                    //{
                    //    btnPrint.Enabled = true;
                    //}
                    SetStatusLabelText(displayableTransactionLines);
                    List<TransactionLineDTO> pendingLines = displayableTransactionLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false
                                                                && ((x.CardNumber.StartsWith("T") == false
                                                                     && x.ReceiptPrinted == false && IsTransactionLineSelectable(x))
                                                                    || x.CardNumber.StartsWith("T"))).ToList();
                    transactionProcessingIsComplete = (pendingLines != null && pendingLines.Any()) ? false : true;
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 4554,
                                                    MessageContainerList.GetMessage(machineUserContext, "Issue")));
                    //'Click on &1 button to proceed.'
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile("Error in PerformGetTransactionDetails: " + ex.Message);
                if (inputValidationError == false)
                { ClearScreen(); }
                else
                {
                    SetFocus();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                this.kioskTransaction = null;
                kioskGlobalTrxId = -1;
                StartKioskTimer();
                ResetKioskTimer();
                AfterBackgroundOperation();
            }
            log.LogMethodExit(transactionProcessingIsComplete);
            return transactionProcessingIsComplete;
        }


        private void CardDispenserStatusCheck()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                StopKioskTimer();
                if (cardDispenser != null)
                {
                    string mes = "";
                    if (isDispenserCardReaderValid == false)
                    {
                        cardDispenser.dispenserWorking = false;
                        dispenserMessage = MessageContainerList.GetMessage(machineUserContext, 1696);
                    }
                    else
                    {
                        dispenserMessage = string.Empty;
                        int cardPosition = -1;
                        this.Cursor = Cursors.WaitCursor;
                        bool suc = BackgroundProcessRunner.Run<bool>(() =>
                        {
                            return InvokeCardDispenserCheckStatus(ref cardPosition, ref mes);
                        }
                        );
                        this.Cursor = Cursors.WaitCursor;
                        //bool suc = cardDispenser.checkStatus(ref cardPosition, ref mes);
                        log.LogVariableState("cardPosition", cardPosition);
                        log.LogVariableState("mes", mes);
                        log.LogVariableState("suc", suc);
                        KioskStatic.logToFile("cardPosition code:" + cardPosition.ToString());
                        if (suc)
                        {
                            if (cardPosition == 3)
                            {
                                dispenserMessage = mes;
                                cardDispenser.dispenserWorking = false;
                                KioskStatic.logToFile("Card at mouth positon. Please remove card.");
                                log.LogVariableState("Card at mouth positon. Please remove card.", cardDispenser.dispenserWorking);
                            }
                            else if (cardPosition == 2)
                            {
                                dispenserMessage = mes;
                                cardDispenser.dispenserWorking = false;
                                string message = "";
                                KioskStatic.logToFile("Card at read positon. Ejecting.");
                                cardDispenser.ejectCard(ref message);
                                KioskStatic.logToFile(message);
                                log.LogVariableState(message, cardDispenser.dispenserWorking);
                            }
                        }
                        else
                        {
                            KioskStatic.logToFile(mes);
                            dispenserMessage = MessageContainerList.GetMessage(machineUserContext, 377) + ". " + MessageContainerList.GetMessage(machineUserContext, 441);
                        }
                    }
                    if (cardDispenserMonitor != null)
                    {
                        cardDispenserMonitor.Post((cardDispenser.dispenserWorking ? Semnox.Parafait.logger.Monitor.MonitorLogStatus.INFO : Semnox.Parafait.logger.Monitor.MonitorLogStatus.ERROR), string.IsNullOrEmpty(dispenserMessage) ? "Dispenser working" : dispenserMessage + ": " + mes);
                    }
                }
                if (dispenserMessage != "")
                {
                    DisplayMessageLine(dispenserMessage);
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
                }

                log.LogVariableState(txtMessage.Text, null);
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile("Error in CardDispenserStatusCheck : " + ex.Message);
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                //StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void CheckCardDispenserDependency()
        {
            log.LogMethodEntry();
            //string errorMessage = "";
            log.Debug("Disport: " + KioskStatic.config.dispport);
            if (KioskStatic.config.dispport == -1)
            {
                string mes = MessageContainerList.GetMessage(machineUserContext, 2384);
                KioskStatic.logToFile(mes);
                log.LogMethodExit("Disabled the card dispenser by setting card dispenser port = -1");
                return;
            }
            if (cardDispenser == null)
            {
                //txtMessage.Text = errorMessage = MessageContainerList.GetMessage(machineUserContext, 460);
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 460));
            }
            else if (cardDispenser.dispenserWorking == false)
            {
                //txtMessage.Text = errorMessage = string.IsNullOrEmpty(dispenserMessage) ? MessageContainerList.GetMessage(machineUserContext, 460) : dispenserMessage;
                throw new Exception(string.IsNullOrEmpty(dispenserMessage) ? MessageContainerList.GetMessage(machineUserContext, 460) : dispenserMessage);
            }
            else if (KioskStatic.DISABLE_PURCHASE_ON_CARD_LOW_LEVEL && cardDispenser.cardLowlevel)
            {
                //txtMessage.Text = errorMessage = MessageContainerList.GetMessage(machineUserContext, 378) + ". " + MessageContainerList.GetMessage(machineUserContext, 441);
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 378) + ". " + MessageContainerList.GetMessage(machineUserContext, 441));
            }
            //if (!String.IsNullOrEmpty(errorMessage))
            //{
            //    log.LogVariableState("errorMessage:", errorMessage);
            //    KioskStatic.logToFile(errorMessage);
            //    throw new Exception(errorMessage);
            //}
            log.LogMethodExit();
        }
        private void ValidateSearchParamters()
        {
            log.LogMethodEntry();
            string errorMessage = "";
            if (String.IsNullOrEmpty(txtTransactionOTP.Text) && String.IsNullOrEmpty(txtTransactionId.Text))
            {
                errorMessage = MessageContainerList.GetMessage(machineUserContext, 1671);

                throw new Exception(errorMessage);
            }
            else if (showTransactionIdField)
            {
                if (String.IsNullOrEmpty(txtTransactionOTP.Text))
                {
                    errorMessage = MessageContainerList.GetMessage(machineUserContext, 1672);
                    //txtTransactionOTP.Focus();
                    throw new Exception(errorMessage);
                }
                if (String.IsNullOrEmpty(txtTransactionId.Text))
                {
                    errorMessage = MessageContainerList.GetMessage(machineUserContext, 1673);
                    //txtTransactionId.Focus();
                    //errorMessage = MessageContainerList.GetMessage(machineUserContext, 1671);
                    throw new Exception(errorMessage);
                }
            }
            log.LogMethodExit();
        }
        private async Task<List<TransactionDTO>> GetTransactionDetails()
        {
            log.LogMethodEntry();
            List<TransactionDTO> onlineTransactionDTOList = new List<TransactionDTO>();
            string transactionId = txtTransactionId.Text.Trim();
            string transactionOTP = txtTransactionOTP.Text.Trim();
            onlineTransactionDTOList = await Task<List<TransactionDTO>>.Factory.StartNew(() => LoadOnlineTransactionDTOList(transactionId, transactionOTP));
            log.LogMethodExit(onlineTransactionDTOList);
            return onlineTransactionDTOList;
        }

        private async Task<List<KeyValuePair<string, List<TransactionLineDTO>>>> GetPrintableLineDetails(TransactionDTO transactionDTO)
        {

            log.LogMethodEntry("transactionDTO");
            List<KeyValuePair<string, List<TransactionLineDTO>>> printableLines = new List<KeyValuePair<string, List<TransactionLineDTO>>>();
            printableLines = await Task<List<KeyValuePair<string, List<TransactionLineDTO>>>>.Factory.StartNew(() => LoadPrintableOnlineTransactionLineList(transactionDTO));
            log.LogMethodExit(printableLines);
            return printableLines;
        }

        private List<KeyValuePair<string, List<TransactionLineDTO>>> LoadPrintableOnlineTransactionLineList(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(transactionId);
            TransactionBL transactionBL = new TransactionBL(machineUserContext, transactionDTO);
            string printerTypeList = PrinterDTO.PrinterTypes.CardPrinter.ToString() + "|" + PrinterDTO.PrinterTypes.RFIDWBPrinter.ToString() + "|" + PrinterDTO.PrinterTypes.TicketPrinter.ToString();
            List<KeyValuePair<string, List<TransactionLineDTO>>> result = transactionBL.GetPrintableLinesForOnlineTransaction(printerTypeList);
            log.LogMethodExit(result);
            return result;
        }

        private void BeforeBackgroundOperation()
        {
            log.LogMethodEntry();
            lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 1448);
            btnCancel.Enabled = false;
            btnExecute.Enabled = false;
            //btnPrint.Enabled = false;
            btnGetTransactionDetails.Enabled = false;
            btnShowKeyPad.Enabled = false;
            Application.DoEvents();
            log.LogMethodExit();
        }
        private static IEnumerable<TransactionLineDTO> GetDisplayableTransactionLines(List<TransactionDTO> onlineTransactionDTOList)
        {
            log.LogMethodEntry();
            IEnumerable<TransactionLineDTO> retValue = onlineTransactionDTOList[0].TransactionLinesDTOList.Where(x => x.ProductTypeCode != "LOCKERDEPOSIT"
                                                                                                                   && x.ProductTypeCode != "DEPOSIT"
                                                                                                                   && x.ProductTypeCode != "CARDDEPOSIT" && x.ProductTypeCode != "LOYALTY");
            log.LogMethodExit(retValue);
            return retValue;
        }
        private bool IsVirtualStore()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            string virtualStoreSiteId = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "VIRTUAL_STORE_SITE_ID");
            log.LogVariableState("virtualStoreSiteId", virtualStoreSiteId);
            bool retValue = string.IsNullOrWhiteSpace(virtualStoreSiteId) == false;
            log.LogMethodExit(retValue);
            return retValue;
        }
        private List<TransactionDTO> LoadOnlineTransactionDTOList(string transactionId, string transactionOTP)
        {
            log.LogMethodEntry(transactionId, transactionOTP);
            TransactionListBL transactionListBL = new TransactionListBL(machineUserContext);
            List<TransactionDTO> result = transactionListBL.GetOnlineTransactionDTOList(transactionId, transactionOTP, utilities);
            log.LogMethodExit(result);
            return result;
        }

        private void SetStatusLabelText(IEnumerable<TransactionLineDTO> displayableTransactionLines)
        {
            log.LogMethodEntry();
            var cardLines = displayableTransactionLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false)
                                    .ToList();
            int notPrintedCount = cardLines.Count(x => x.CardNumber.StartsWith("T") == false
                                                    && x.ReceiptPrinted == false && IsTransactionLineSelectable(x));
            int notIssuedCount = cardLines.Count(x => x.CardNumber.StartsWith("T"));
            int issuedCount = cardLines.Count - notIssuedCount - notPrintedCount;
            int pendingCount = notIssuedCount + notPrintedCount;
            lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 4553,
                            (issuedCount != 0 ? issuedCount.ToString(numberFormat) : "0"),
                            (pendingCount != 0 ? pendingCount.ToString(numberFormat) : "0"));
            //'Issued Count: &1        Remaining Count: &2'
            log.LogMethodExit();
        }
        private void AfterBackgroundOperation()
        {
            log.LogMethodEntry();
            try
            {
                //lblStatus.Text = string.Empty;
                btnCancel.Enabled = true;
                btnExecute.Enabled = true;
                //btnPrint.Enabled = true;
                btnGetTransactionDetails.Enabled = true;
                btnShowKeyPad.Enabled = true;
                //if (dgvTransactionLines.Rows.Count > 0)
                //{ btnPrint.Enabled = true; }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void ClearScreen()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                kioskGlobalTrxId = -1;
                txtTransactionOTP.Text = "";
                txtTransactionId.Text = "";
                lblStatus.Text = string.Empty;
                dgvTransactionHeader.Rows.Clear();
                transactionLineDTOBS.DataSource = new List<TransactionLineDTO>();
                HideDGVTransactionLinesColumns();
                SetSelectAllCheckBox(false);
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
                StartKioskTimer();
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void LoadDataGrid(List<TransactionDTO> onlineTransactionDTOList)
        {
            log.LogMethodEntry(onlineTransactionDTOList);
            ResetKioskTimer();
            LoadDGVTransactionHeaderData(onlineTransactionDTOList);
            LoadDGVTransactionLinesData(onlineTransactionDTOList);
            if (dgvTransactionLines.Rows.Count < 1)
            {
                log.LogVariableState("onlineTransactionDTOList", onlineTransactionDTOList);
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1688));
            }
            log.LogMethodExit();
        }
        private void LoadDGVTransactionHeaderData(List<TransactionDTO> onlineTransactionDTOList)
        {
            log.LogMethodEntry();
            dgvTransactionHeader.Rows.Clear();
            TransactionDTO transactionDTO = onlineTransactionDTOList[0];
            string trxId = string.IsNullOrWhiteSpace(transactionDTO.OriginalSystemReference)
                            ? transactionDTO.TransactionId.ToString() : transactionDTO.OriginalSystemReference;
            dgvTransactionHeader.Rows.Add(trxId, transactionDTO.TransactionNumber, transactionDTO.TransactionDate, transactionDTO.CustomerName);
            dgvTransactionHeader.Rows[0].Tag = transactionDTO;
            dgvTransactionHeader.EndEdit();
            SetDGVTransactionHeaderRowHeight();
            log.LogMethodExit();
        }
        private void LoadDGVTransactionLinesData(List<TransactionDTO> onlineTransactionDTOList)
        {
            log.LogMethodEntry();
            transactionLineDTOBS.DataSource = new List<TransactionLineDTO>();
            IEnumerable<TransactionLineDTO> displayableTransactionLines = displayableTransactionLines = GetDisplayableTransactionLines(onlineTransactionDTOList);
            if (onlineTransactionDTOList[0].TransactionLinesDTOList != null)
            {
                transactionLineDTOBS.DataSource = displayableTransactionLines;
            }
            HideDGVTransactionLinesColumns();
            if (displayableTransactionLines != null && displayableTransactionLines.Any())
            {
                foreach (DataGridViewRow tl in dgvTransactionLines.Rows)
                {
                    TransactionLineDTO result = tl.DataBoundItem as TransactionLineDTO;
                    tl.Cells["Selected"].Tag = false;
                    if (IsTransactionLineSelectable(tl))
                    {
                        tl.Cells["Selected"].Value = Properties.Resources.NewUnTickedCheckBox;
                    }
                    else
                    {
                        tl.Cells["Selected"].Value = Properties.Resources.NewDisabledCheckBox;
                    }
                    if (result != null)
                    {
                        result.Selected = false;
                    }
                }
            }
            SetDGVTransactionLineRowHeight();
            log.LogMethodExit();
        }
        private void SetDGVTransactionHeaderRowHeight()
        {
            log.LogMethodEntry();
            try
            {
                if (dgvTransactionHeader.RowCount > 0)
                { dgvTransactionHeader.BeginEdit(true); }
                dgvTransactionHeader.RowTemplate.MinimumHeight = Properties.Resources.NewUnTickedCheckBox.Height + 2;
                dgvTransactionHeader.RowTemplate.Height = Properties.Resources.NewUnTickedCheckBox.Height + 2;
                dgvTransactionHeader.EndEdit();
                dgvTransactionHeader.Refresh();
                dgvTransactionHeader.ReadOnly = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private void SetDGVTransactionLineRowHeight()
        {
            log.LogMethodEntry();
            try
            {
                if (dgvTransactionLines.RowCount > 0)
                { dgvTransactionLines.BeginEdit(true); }
                dgvTransactionLines.RowTemplate.MinimumHeight = Properties.Resources.NewUnTickedCheckBox.Height + 2;
                dgvTransactionLines.RowTemplate.Height = Properties.Resources.NewUnTickedCheckBox.Height + 2;
                dgvTransactionLines.EndEdit();
                dgvTransactionLines.Refresh();
                dgvTransactionLines.ReadOnly = true;
                if (dgvTransactionLines.RowCount > 0)
                {  //set focus on second column to avoid check box display issue
                    dgvTransactionLines.CurrentCell = dgvTransactionLines.Rows[0].Cells["productName"];
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        private void HideDGVTransactionLinesColumns()
        {
            log.LogMethodEntry();
            foreach (DataGridViewColumn column in dgvTransactionLines.Columns)
            {
                if (displayCOlumnList.Contains(column.Name) == false)
                {
                    column.Visible = false;
                }
            }
            log.LogMethodExit();
        }
        private void pbxSelectAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Select all check box is clicked");
                ResetKioskTimer();
                StopKioskTimer();
                this.Cursor = Cursors.WaitCursor;
                bool flag = SetSelectAllCheckBox();
                SetTransactionLineCheckBox(flag);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            finally
            {
                StartKioskTimer();
                ResetKioskTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private bool SetSelectAllCheckBox()
        {
            log.LogMethodEntry();
            bool flag = (pbxSelectAll.Tag != null ? Convert.ToBoolean(pbxSelectAll.Tag) : false);
            SetSelectAllCheckBox(!(flag));
            log.LogMethodExit(flag);
            return flag;
        }

        private void SetSelectAllCheckBox(bool flag)
        {
            log.LogMethodEntry(flag);
            if (flag)
            {
                pbxSelectAll.BackgroundImage = Properties.Resources.NewTickedCheckBox;
                pbxSelectAll.Tag = true;
            }
            else
            {
                pbxSelectAll.BackgroundImage = Properties.Resources.NewUnTickedCheckBox;
                pbxSelectAll.Tag = false;
            }
            log.LogMethodExit();
        }

        private void SetTransactionLineCheckBox(bool flag)
        {
            log.LogMethodEntry(flag);
            this.Cursor = Cursors.WaitCursor;
            if (dgvTransactionLines.Rows.Count > 0)
            {
                dgvTransactionLines.BeginEdit(true);
                for (int i = 0; i < dgvTransactionLines.Rows.Count; i++)
                {
                    TransactionLineDTO result = dgvTransactionLines.Rows[i].DataBoundItem as TransactionLineDTO;
                    DataGridViewImageCell cell = (dgvTransactionLines.Rows[i].Cells[Selected.Index] as DataGridViewImageCell);
                    if (IsTransactionLineSelectable(dgvTransactionLines.Rows[i]))
                    {
                        if (!(flag))
                        {
                            cell.Value = Properties.Resources.NewTickedCheckBox;
                            cell.Tag = true;
                            result.Selected = true;
                        }
                        else
                        {
                            cell.Value = Properties.Resources.NewUnTickedCheckBox;
                            cell.Tag = false;
                            result.Selected = false;
                        }
                    }
                    else
                    {
                        cell.Value = Properties.Resources.NewDisabledCheckBox;
                        cell.Tag = false;
                        result.Selected = false;
                    }
                }
                dgvTransactionLines.EndEdit();
                dgvTransactionLines.Refresh();
                dgvTransactionLines.CurrentCell = dgvTransactionLines.Rows[0].Cells["productName"];
            }
            log.LogMethodExit();
        }


        private void SelectEligibleLines()
        {
            log.LogMethodEntry();
            bool setFlag = true;
            SetSelectAllCheckBox(setFlag);
            bool setLineFlag = false;
            SetTransactionLineCheckBox(setLineFlag);
            log.LogMethodExit();
        }
        private void vScrollTransactionLines_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try { ResetKioskTimer(); } catch { }
            log.LogMethodExit();
        }
        private void vScrollTransactionLines_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                if (dgvTransactionLines.Rows.Count > 0)
                {
                    dgvTransactionLines.FirstDisplayedScrollingRowIndex = e.NewValue;
                }
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void dgvTransactionLines_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                this.Cursor = Cursors.WaitCursor;
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.LogMethodExit(null, "No valid cell clicked.");
                    return;
                }
                if (Selected.Index == e.ColumnIndex)
                {
                    DataGridViewImageCell cell = (dgvTransactionLines.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewImageCell);
                    DataGridViewTextBoxCell cardNumberCell = (dgvTransactionLines.Rows[e.RowIndex].Cells["lineCardNo"] as DataGridViewTextBoxCell);
                    if (cell == null)
                    {
                        log.LogMethodExit(null, "No valid cell clicked.");
                        return;
                    }
                    bool cellValue = (cell.Tag != null ? ((bool)cell.Tag) : false);
                    DataGridViewRow row = dgvTransactionLines.Rows[e.RowIndex];
                    if (IsTransactionLineSelectable(row))
                    {
                        if (string.IsNullOrWhiteSpace(cardNumberCell.Value.ToString()) == false)
                        {
                            bool mapAll = false;
                            if (cardNumberCell.Value.ToString().StartsWith("T") == false)
                            {
                                int numberOfLinesWithSameCard = 0;
                                foreach (DataGridViewRow rowsSelected in dgvTransactionLines.Rows)
                                {
                                    TransactionLineDTO transactionLineDTO = rowsSelected.DataBoundItem as TransactionLineDTO;
                                    bool selectable = IsTransactionLineSelectable(transactionLineDTO);
                                    if (transactionLineDTO.CardNumber != null && selectable
                                        && transactionLineDTO.CardNumber.Equals(cardNumberCell.Value.ToString()))
                                    {
                                        numberOfLinesWithSameCard++;
                                    }
                                }
                                //if there are more than line with same card to print, print all of them
                                if (numberOfLinesWithSameCard > 1)
                                {
                                    mapAll = true;
                                }
                            }
                            else
                            {
                                mapAll = true;
                            }
                            if (mapAll)
                            {
                                foreach (DataGridViewRow rowsSelected in dgvTransactionLines.Rows)
                                {
                                    TransactionLineDTO transactionLineDTO = rowsSelected.DataBoundItem as TransactionLineDTO;
                                    if (transactionLineDTO.CardNumber != null
                                        && transactionLineDTO.CardNumber.Equals(cardNumberCell.Value.ToString()))
                                    {
                                        SetFlag(e.ColumnIndex, cellValue, rowsSelected);
                                    }
                                }
                            }
                            else
                            {
                                DataGridViewRow rowsSelected = dgvTransactionLines.Rows[e.RowIndex];
                                SetFlag(e.ColumnIndex, cellValue, rowsSelected);
                            }
                        }
                        else
                        {
                            DataGridViewRow rowsSelected = dgvTransactionLines.Rows[e.RowIndex];
                            SetFlag(e.ColumnIndex, cellValue, rowsSelected);
                        }
                        dgvTransactionLines.EndEdit();
                        dgvTransactionLines.Refresh();
                        dgvTransactionLines.CurrentCell = dgvTransactionLines.Rows[e.RowIndex].Cells["productName"];
                    }
                    else
                    {
                        cell.Tag = false;
                        cell.Value = Properties.Resources.NewDisabledCheckBox;
                        //DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, ))
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
                ResetKioskTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private static void SetFlag(int columnIndex, bool cellValue, DataGridViewRow rowsSelected)
        {
            TransactionLineDTO result = rowsSelected.DataBoundItem as TransactionLineDTO;
            if (!(cellValue))
            {
                rowsSelected.Cells[columnIndex].Value = Properties.Resources.NewTickedCheckBox;
                rowsSelected.Cells[columnIndex].Tag = true;
                result.Selected = true;
            }
            else
            {
                rowsSelected.Cells[columnIndex].Value = Properties.Resources.NewUnTickedCheckBox;
                rowsSelected.Cells[columnIndex].Tag = false;
                result.Selected = false;
            }
        }

        private bool IsTransactionLineSelectable(DataGridViewRow row)
        {
            log.LogMethodEntry();
            TransactionLineDTO transactionLineDTO = row.DataBoundItem as TransactionLineDTO;
            bool selectable = IsTransactionLineSelectable(transactionLineDTO);
            log.LogMethodExit(selectable);
            return selectable;
        }

        private bool IsTransactionLineSelectable(TransactionLineDTO transactionLineDTO)
        {
            log.LogMethodEntry(transactionLineDTO);
            //line has card which is either temp or yet to be printed
            bool selectable = transactionLineDTO != null && string.IsNullOrWhiteSpace(transactionLineDTO.CardNumber) == false
                                 && (transactionLineDTO.CardNumber.StartsWith("T")
                                     || (transactionLineDTO.ReceiptPrinted == false
                                        && LineProductBelongsToCardRFIDTicketPrinter(transactionLineDTO.Guid)));
            log.LogMethodExit(selectable);
            return selectable;
        }

        private bool LineProductBelongsToCardRFIDTicketPrinter(string lineGuid)
        {
            log.LogMethodEntry(lineGuid);
            bool belongs = false;
            if (printableLineDetails != null)
            {
                for (int i = 0; i < printableLineDetails.Count; i++)
                {
                    if (printableLineDetails[i].Value != null && printableLineDetails[i].Value.Exists(tl => tl.Guid == lineGuid))
                    {
                        belongs = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(belongs);
            return belongs;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Cancel button is clicked");
                ResetKioskTimer();
                bool proceedWithCancel = true;
                if (dgvTransactionLines != null && dgvTransactionLines.Rows.Count > 0)
                {
                    using (frmYesNo frm = new frmYesNo(MessageContainerList.GetMessage(machineUserContext, 1695)))
                    {
                        if (frm.ShowDialog() != DialogResult.Yes)
                        {
                            proceedWithCancel = false;
                            ResetKioskTimer();
                        }
                    }
                }
                if (proceedWithCancel)
                {
                    ClearScreen();
                    //if (!singleFunctionMode)
                    //{
                    this.Close();
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowUserAlert(ex.Message);
                KioskStatic.logToFile("Error in BtnCancel_Click: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private TransactionDTO GetCurrentTransactionDTO()
        {
            log.LogMethodEntry();
            if (dgvTransactionHeader.RowCount <= 0)
            {
                log.LogMethodExit("No transactions selected.");
                return null;
            }
            TransactionDTO result = dgvTransactionHeader.Rows[0].Tag as TransactionDTO;
            log.LogMethodExit(result);
            return result;
        }
        private async void btnExecute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                KioskStatic.logToFile("Execute button is clicked");
                this.Cursor = Cursors.WaitCursor;
                ResetKioskTimer();
                StopKioskTimer();
                TransactionDTO transactionDTO = GetCurrentTransactionDTO();
                log.LogVariableState("transactionDTO", transactionDTO);
                if (transactionDTO == null)
                {
                    log.LogMethodExit("No transactions selected.");
                    KioskStatic.logToFile("No transactions selected.");
                    return;
                }
                if (transactionDTO.Status == Semnox.Parafait.Transaction.Transaction.TrxStatus.OPEN.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 5580));//"Your transaction is under process! Please retry after some time."
                }
                else if (transactionDTO.Status == Semnox.Parafait.Transaction.Transaction.TrxStatus.CANCELLED.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2409));//"Transaction is already cancelled"
                }
                else if (transactionDTO.Status == Semnox.Parafait.Transaction.Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2410)); //"Transaction is already abandoned by the system"
                }
                if (transactionDTO.TransactionDate.Date > ServerDateTime.Now.Date)
                {
                    string warningMsg = MessageContainerList.GetMessage(machineUserContext, 1129, transactionDTO.TransactionDate.Date);
                    using (frmYesNo frm = new frmYesNo(warningMsg))
                    {
                        if (frm.ShowDialog() != DialogResult.Yes)
                        {
                            log.LogMethodExit("Trx date is greater than current date, user decided not to proceed");
                            KioskStatic.logToFile("Trx date is greater than current date, user decided not to proceed");
                            return;
                        }
                    }
                }
                List<TransactionLineDTO> allSelectedLines = transactionDTO.TransactionLinesDTOList.Where(x => x.Selected).ToList();
                if (allSelectedLines.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2460));
                    //Please select a record to proceed
                }
                try
                {
                    BeforeBackgroundOperation();
                    onlineTransaction = new ExecuteOnlineTransaction(utilities, transactionDTO, IsVirtualStore());
                    SetReceiptPrintOptions(onlineTransaction);
                    await IssueNewCards(transactionDTO, onlineTransaction);
                    IssuePrints(transactionDTO);
                    lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 1448);
                    DisplayMessageLine(lblStatus.Text);
                }
                finally
                {
                    AfterBackgroundOperation();
                }
                bool transactionProcessingIsComplete = await PerformGetTransactionDetails();
                if (transactionProcessingIsComplete)
                {
                    ClearScreen();
                }
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1672));
            }
            catch (Exception ex)
            {
                //lblStatus.Text = string.Empty;
                log.Error(ex);
                string msg = MessageContainerList.GetMessage(machineUserContext, ex.Message);
                ShowUserAlert(msg);
                DisplayMessageLine(msg);
                KioskStatic.logToFile("Error in Execute: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private int GetKioskGlobalTransactionId()
        {
            log.LogMethodEntry();
            if (kioskGlobalTrxId == -1)
            {
                kioskGlobalTrxId = KioskStatic.GetKioskGlobalTransactionId(null);
            }
            log.LogMethodExit(kioskGlobalTrxId);
            return kioskGlobalTrxId;
        }
        private async Task IssueNewCards(TransactionDTO transactionDTO, KioskTransaction onlineTransaction)
        {
            log.LogMethodEntry("transactionDTO", "onlineTransaction");
            bool hasTempCards = TransactionHasTempCards(transactionDTO);
            log.LogVariableState("hasTempCards", hasTempCards);
            KioskStatic.logToFile("Transaction has temp cards: " + hasTempCards.ToString());
            if (hasTempCards)
            {
                List<TransactionLineDTO> allSelectedLines = transactionDTO.TransactionLinesDTOList.Where(x => x.Selected).ToList();
                if (allSelectedLines.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1703));//No TEMP cards to convert to physical cards
                }
                List<TransactionLineDTO> selectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && x.CardNumber.StartsWith("T")).ToList();
                if (selectedLines.Any() == false)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1703));//No TEMP cards to convert to physical cards
                }
                if (KioskStatic.config.dispport == -1)
                {
                    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2384) + ". " + MessageContainerList.GetMessage(machineUserContext, 441));
                    // Card dispenser is Disabled.Sorry you cannot proceed 
                }
                CardDispenserStatusCheck();
                StopKioskTimer();
                CheckCardDispenserDependency();
                //List<TransactionLineDTO> nonTempSelectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && x.CardNumber.StartsWith("T") == false).ToList();
                //if (nonTempSelectedLines.Any() == true)
                //{
                //    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2835));
                //    //"Can issue Temp cards only. Please uncheck issued cards to proceed"
                //}
                Semnox.Parafait.Transaction.Transaction trx = null;
                //try
                //{
                if (IsVirtualStore() == false)
                {
                    //TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    trx = onlineTransaction.KioskTransactionObject;
                    string waiverMsg = string.Empty;
                    foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in trx.TrxLines)
                    {
                        if (string.IsNullOrWhiteSpace(tl.CardNumber) == false &&
                            tl.CardNumber.StartsWith("T"))
                        {
                            foreach (TransactionLineDTO selectedTrxLine in selectedLines)
                            {
                                //if (dr.Cells["CardNumber"].Value != null && dr.Cells["CardNumber"].Value.Equals(tl.CardNumber))
                                if (string.IsNullOrEmpty(selectedTrxLine.CardNumber) == false && selectedTrxLine.CardNumber == tl.CardNumber)
                                {
                                    int lineId = trx.TrxLines.IndexOf(tl);
                                    //tl.LineValid = dr.Cells["issueCardColumn"].Value.Equals(true);
                                    if (selectedTrxLine.Selected && trx.IsWaiverSignaturePending(lineId))
                                    {
                                        waiverMsg = waiverMsg + MessageContainerList.GetMessage(machineUserContext, 2353, tl.CardNumber) + Environment.NewLine;
                                        //Waiver signing is pending for transaction line with card number &1. 
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (string.IsNullOrEmpty(waiverMsg) == false)
                    {
                        //Waiver signing is pending do not proceed
                        waiverMsg = waiverMsg + MessageContainerList.GetMessage(machineUserContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                        throw new Exception(waiverMsg);
                    }
                }
                //int numberOfCardsToBeIssued = selectedLines.Count;
                //bool proceedWithIssueCard = false;
                // proceedWithIssueCard = GetUserConfirmationToProceed(transactionDTO.TransactionDate, numberOfCardsToBeIssued);
                //if (proceedWithIssueCard)
                //{
                this.Cursor = Cursors.WaitCursor;
                //await Task.Factory.StartNew(() => { IssueCards(transactionDTO, selectedLines, trx); });
                List<string> listCards = new List<string>();
                if (selectedLines != null && selectedLines.Any())
                {
                    if (cardDispenser != null)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1674));
                        lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 1674);
                        int cardCount = 0;
                        Dictionary<string, string> cardList = new Dictionary<string, string>();
                        //TaskProcs taskProcObj = new TaskProcs(KioskStatic.Utilities);
                        int cardDispenseRetryCount = 3;
                        string cardDispenserMessage = "";
                        try
                        {
                            foreach (TransactionLineDTO unIssuedCardLine in selectedLines)
                            {
                                if (cardList.Count > 0 && cardList.ContainsKey(unIssuedCardLine.CardNumber))
                                {
                                    continue;
                                }
                                string oldCardNumber = unIssuedCardLine.CardNumber;
                                string newCardNumber = "";
                                bool yetToDispenseCard = true;
                                cardCount++;
                                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1675, cardCount));
                                lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 1675, cardCount);
                                cardDispenserMessage = "";
                                cardDispenseRetryCount = 3;
                                while (yetToDispenseCard)
                                {
                                    if (cardDispenser.doDispenseCard(ref newCardNumber, ref cardDispenserMessage))
                                    {
                                        if (KioskStatic.CardDispenserModel.Equals(Semnox.Parafait.KioskCore.CardDispenser.CardDispenser.Models.SCT0M0) == false)
                                        {
                                            int counter = 0;
                                            while (string.IsNullOrWhiteSpace(newCardNumber))
                                            {
                                                string newCardNumberValue = KioskStatic.DispenserReaderDevice.readValidatedCardNumber();
                                                newCardNumber = KioskHelper.ValidateDispenserCardNumber(machineUserContext, newCardNumberValue);
                                                if (string.IsNullOrWhiteSpace(newCardNumber) == false)
                                                {
                                                    break;
                                                }
                                                string msg1 = MessageContainerList.GetMessage(machineUserContext,
                                                                                       "Unable to read card number. Retrying...");
                                                KioskStatic.logToFile(msg1);
                                                log.Error(msg1);
                                                counter++;
                                                if (counter > 2)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        if (String.IsNullOrEmpty(newCardNumber))
                                        {
                                            cardDispenseRetryCount = CardDispenserRejectCardWithRetry(cardDispenserMessage, cardDispenseRetryCount);
                                        }
                                        else
                                        {
                                            if (CardNumberLengthIsValid(newCardNumber))
                                            {
                                                List<TransactionLineDTO> tempCardLines = new List<TransactionLineDTO>();
                                                tempCardLines.Add(unIssuedCardLine);
                                                List<string> tagNumbers = new List<string>();
                                                tagNumbers.Add(newCardNumber);
                                                await Task.Factory.StartNew(() =>
                                                {
                                                    RemotingClient cardRoamingRemotingClient = KioskHelper.GetRoamingClient();
                                                    TransactionBL transactionBL = new TransactionBL(machineUserContext, transactionDTO);
                                                    transactionBL.ExecuteOnlineTransaction(utilities, cardRoamingRemotingClient, tempCardLines, tagNumbers);
                                                });
                                                UpdateKioskActivityLog(unIssuedCardLine, newCardNumber);
                                                CardDispenserEjectCard(cardDispenserMessage);
                                                StopKioskTimer();
                                                yetToDispenseCard = false;
                                            }
                                            else
                                            {
                                                cardDispenserMessage = MessageContainerList.GetMessage(machineUserContext, 1697, newCardNumber);
                                                cardDispenseRetryCount = CardDispenserRejectCardWithRetry(cardDispenserMessage, cardDispenseRetryCount);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        CheckDispenserError(cardDispenseRetryCount, cardDispenserMessage);
                                    }
                                }
                                cardList.Add(oldCardNumber, newCardNumber);
                                //unIssuedCardLine.Cells[3].Value = newCardNumber;
                                //(unIssuedCardLine.Cells[4] as DataGridViewImageCell).Value = Properties.Resources.OK;
                            }
                            log.Debug("Card Count : " + cardCount);
                            string msg = MessageContainerList.GetMessage(machineUserContext, 1676, cardCount);
                            ShowUserAlert(msg);
                            DisplayMessageLine(msg);
                            lblStatus.Text = msg;
                            KioskStatic.logToFile(msg);
                            UpdateLocalLineCopyWithNewCardInfo(transactionDTO, cardList);
                            //ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 1702));
                            GenerateExecuteOnlineReceipt(onlineTransaction, cardCount);
                            SendSuccessEmail(onlineTransaction, transactionDTO, cardCount);
                        }
                        catch (Exception ex)
                        {
                            bool rejectCard = false;
                            while (rejectCard == false)
                            {
                                rejectCard = CardDispenserRejectCard(cardDispenserMessage);
                            }
                            log.Error(ex);
                            //DisplayMessageLine(ex.Message);
                            KioskStatic.logToFile("Error while issuing cards: " + ex.Message);
                            ShowUserAlert(ex.Message);
                            GenerateExecuteOnlineErrorReceipt(onlineTransaction, cardCount - 1);
                            SendErrorEmail(onlineTransaction, cardCount - 1);
                        }
                    }
                    else
                    {
                        log.LogVariableState("cardDispenser", cardDispenser);
                        log.Info("cardDispenser is not initialized");
                        string msg = MessageContainerList.GetMessage(machineUserContext, 1677) + MessageContainerList.GetMessage(machineUserContext, 441);
                        ShowUserAlert(msg);
                        KioskStatic.logToFile("Error while issuing cards: " + msg);
                    }
                }
                //btnPrint.Enabled = true;
                //btnGetTransactionDetails.PerformClick();
                log.Info("TEMP cards converted to physical cards successfully ");
            }
            //}
            log.LogMethodExit();
        }

        private bool TransactionHasTempCards(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry(transactionDTO);
            bool hasTempCards = false;
            if (transactionDTO != null && transactionDTO.TransactionLinesDTOList != null)
            {
                List<TransactionLineDTO> tCardLines = transactionDTO.TransactionLinesDTOList.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false
                                                                                                         && x.CardNumber.StartsWith("T") && x.CancelledTime == null).ToList();
                if (tCardLines != null && tCardLines.Any())
                {
                    hasTempCards = true;
                }
            }
            log.LogMethodExit(hasTempCards);
            return hasTempCards;
        }

        private int CardDispenserRejectCardWithRetry(string cardDispenserMessage, int cardDispenseRetryCount)
        {
            log.LogMethodEntry(cardDispenserMessage, cardDispenseRetryCount);
            ResetKioskTimer();
            ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 1680));
            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1680));
            KioskStatic.logToFile(txtMessage.Text);
            log.Info(txtMessage.Text);
            Thread.Sleep(300);
            if (!cardDispenser.doRejectCard(ref cardDispenserMessage))
            {
                DisplayMessageLine(cardDispenserMessage);
                log.Info(MessageContainerList.GetMessage(machineUserContext, 1681, txtMessage.Text, cardDispenserMessage));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1681, txtMessage.Text, cardDispenserMessage));
            }

            cardDispenseRetryCount--;
            if (cardDispenseRetryCount > 0)
            {
                Thread.Sleep(2000);
                Application.DoEvents();
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1682));
                throw new Exception(txtMessage.Text);
            }
            log.LogMethodExit(cardDispenseRetryCount);
            return cardDispenseRetryCount;
        }
        private bool CardDispenserRejectCard(string cardDispenserMessage)
        {
            log.LogMethodEntry(cardDispenserMessage);
            bool returnValue = false;
            ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 1683));
            DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1683));
            KioskStatic.logToFile(txtMessage.Text);
            log.Info(txtMessage.Text);
            Thread.Sleep(300);
            if (!cardDispenser.doRejectCard(ref cardDispenserMessage))
            {
                returnValue = false;
                DisplayMessageLine(cardDispenserMessage);
                log.Info(MessageContainerList.GetMessage(machineUserContext, 1681, txtMessage.Text, cardDispenserMessage));
                //throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1681, txtMessage.Text, cardDispenserMessage));
            }
            else
            {
                returnValue = true;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        private int CheckDispenserError(int cardDispenseRetryCount, string cardDispenserMessage)
        {
            log.LogMethodEntry(cardDispenseRetryCount, cardDispenserMessage);
            KioskStatic.logToFile(cardDispenserMessage);
            ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 441) + Environment.NewLine + MessageContainerList.GetMessage(machineUserContext, "Dispenser Error") + ": " + cardDispenserMessage + Environment.NewLine
                          + MessageContainerList.GetMessage(machineUserContext, 1686), false);
            cardDispenseRetryCount--;
            if (cardDispenseRetryCount > 0)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1687, (3 - cardDispenseRetryCount).ToString()));
                KioskStatic.logToFile(txtMessage.Text);
                log.Info(txtMessage.Text);
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 1682));
                throw new Exception(txtMessage.Text + " " + cardDispenserMessage);
            }
            log.LogMethodExit(cardDispenseRetryCount);
            return cardDispenseRetryCount;
        }
        private bool CardNumberLengthIsValid(string newCardNumber)
        {
            log.LogMethodEntry(newCardNumber);
            bool result = TagNumber.IsValid(machineUserContext, newCardNumber);
            log.LogMethodExit(result);
            return result;
        }
        private void CardDispenserEjectCard(string cardDispenserMessage)
        {
            log.LogMethodEntry(cardDispenserMessage);
            try
            {
                StopKioskTimer();
                Thread.Sleep(300);
                cardDispenser.doEjectCard(ref cardDispenserMessage);
                while (true)
                {
                    Thread.Sleep(300);
                    int cardPosition = 0;
                    if (cardDispenser.checkStatus(ref cardPosition, ref cardDispenserMessage))
                    {
                        if (cardPosition >= 2)
                        {
                            ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 393));
                            KioskStatic.logToFile("Card dispensed. Waiting to be removed.");
                            log.Info("Card dispensed. Waiting to be removed.");
                        }
                        else
                        {
                            KioskStatic.logToFile("Card removed.");
                            log.Info("Card removed.");
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            finally
            {
                //StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void UpdateKioskActivityLog(TransactionLineDTO trxLineRec, string newCardNumber)
        {
            log.LogMethodEntry(trxLineRec, newCardNumber);
            ResetKioskTimer();
            log.LogVariableState("trxLineRec", trxLineRec);
            int kioskTrxId = GetKioskGlobalTransactionId();
            KioskActivityLogDTO KioskActivityLogDTO = new KioskActivityLogDTO("", ServerDateTime.Now, "NEWCARD", (double)trxLineRec.Amount, newCardNumber,
                                                                              "transferCard " + trxLineRec.CardNumber + " to " + newCardNumber, machineUserContext.GetMachineId(),
                                                                             utilities.ParafaitEnv.POSMachine, "", false, trxLineRec.TransactionId, -1, kioskTrxId, -1);

            KioskActivityLogBL kioskActivityLogBL = new KioskActivityLogBL(machineUserContext, KioskActivityLogDTO);
            kioskActivityLogBL.Save();
            log.LogMethodExit();
        }
        private void SendSuccessEmail(KioskTransaction onlineTransaction, TransactionDTO transactionDTO, int cardCount)
        {
            log.LogMethodEntry("onlineTransaction", transactionDTO, cardCount);
            try
            {
                ResetKioskTimer();
                StopKioskTimer();
                this.Cursor = Cursors.WaitCursor;
                if (onlineTransaction.RececiptDeliveryMode == KioskTransaction.KioskReceiptDeliveryMode.EMAIL)
                {
                    TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                    Semnox.Parafait.Transaction.Transaction transactionRec = TransactionUtils.CreateTransactionFromDB(onlineTransaction.KioskTransactionObject.Trx_id, utilities);
                    if (transactionRec != null && transactionRec.customerIdentifier != null && String.IsNullOrEmpty(transactionRec.customerIdentifier) == false)
                    {
                        TransactionEventsBL transactionEventsBL = new TransactionEventsBL(machineUserContext, utilities, ParafaitFunctionEvents.EXECUTE_ONLINE_TRANSACTION_EVENT, transactionRec, null, cardCount, null);
                        transactionEventsBL.SendMessage(MessagingClientDTO.MessagingChanelType.NONE);
                    }
                    else
                    {
                        log.Info("Unable to send email as customer email information is not available");
                        KioskStatic.logToFile("Unable to send email as customer email information is not available");
                        //Dear customer, &1Your transaction with OTP: &2 is processed successfully. &3Total cards issued today: &4
                        string message = MessageContainerList.GetMessage(machineUserContext, 5254, Environment.NewLine,
                            transactionDTO.TransactionOTP, Environment.NewLine, cardCount);
                        KioskStatic.logToFile(message);
                        frmOKMsg.ShowUserMessage(message);
                    }
                }
                if (IsVirtualStore())
                {
                    //Dear customer, &1Your transaction with OTP: &2 is processed successfully. &3Total cards issued today: &4
                    string message = MessageContainerList.GetMessage(machineUserContext, 5254, Environment.NewLine,
                        transactionDTO.TransactionOTP, Environment.NewLine, cardCount);
                    KioskStatic.logToFile(message);
                    frmOKMsg.ShowUserMessage(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                StartKioskTimer();
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }
        private void SendErrorEmail(KioskTransaction onlineTransaction, int cardCount)
        {
            log.LogMethodEntry("onlineTransaction", cardCount);
            try
            {
                ResetKioskTimer();
                StopKioskTimer();
                this.Cursor = Cursors.WaitCursor;
                if (onlineTransaction.RececiptDeliveryMode == KioskTransaction.KioskReceiptDeliveryMode.EMAIL)
                {
                    Semnox.Parafait.Transaction.Transaction transactionRec = onlineTransaction.KioskTransactionObject;
                    string firstName = MessageContainerList.GetMessage(machineUserContext, "Customer");
                    string msgPiece = " ";
                    if (cardCount > 0)
                    {
                        msgPiece = MessageContainerList.GetMessage(machineUserContext, 1690, cardCount);
                    }
                    if (transactionRec != null && transactionRec.customerDTO != null)
                    {
                        firstName = transactionRec.customerDTO.FirstName;
                    }
                    string body = MessageContainerList.GetMessage(machineUserContext, 1691, firstName, Environment.NewLine, transactionRec.transactionOTP, msgPiece, utilities.ParafaitEnv.SiteName);

                    if (transactionRec != null && transactionRec.customerIdentifier != null && String.IsNullOrEmpty(transactionRec.customerIdentifier) == false)
                    {
                        Messaging msg = new Messaging(utilities);
                        msg.SendEMailSynchronous(transactionRec.customerIdentifier, "", utilities.ParafaitEnv.SiteName + MessageContainerList.GetMessage(machineUserContext, " - Error while issuing cards"), body);
                    }
                    else
                    {
                        //We are unable to process your transaction due to an error. Please contact our staff for the assistance. Total cards issued before the error: &1
                        string message = MessageContainerList.GetMessage(machineUserContext, 5239, cardCount);
                        KioskStatic.logToFile(message);
                        frmOKMsg.ShowUserMessage(message);
                    }
                }
                if (IsVirtualStore())
                {
                    //We are unable to process your transaction due to an error. Please contact our staff for the assistance. Total cards issued before the error: &1
                    string message = MessageContainerList.GetMessage(machineUserContext, 5239, cardCount);
                    KioskStatic.logToFile(message);
                    frmOKMsg.ShowUserMessage(message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            finally
            {
                StartKioskTimer();
                ResetKioskTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        private void UpdateLocalLineCopyWithNewCardInfo(TransactionDTO transactionDTO, Dictionary<string, string> cardList)
        {
            log.LogMethodEntry("transactionDTO", cardList);
            if (cardList != null && cardList.Any() && transactionDTO != null && transactionDTO.TransactionLinesDTOList != null)
            {
                Dictionary<string, string> cardListFinal = cardList.Where(x => string.IsNullOrWhiteSpace(x.Value) == false).ToDictionary(x => x.Key, x => x.Value);
                List<string> newCardNumberList = cardListFinal.Select(k => k.Value).ToList();
                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                AccountListBL accountListBL = new AccountListBL(machineUserContext);
                accountDTOList = accountListBL.CardNumberInfoForExecuteProcess(newCardNumberList);
                log.LogVariableState("accountDTOList", accountDTOList);
                if (accountDTOList != null)
                {
                    foreach (KeyValuePair<string, string> item in cardListFinal)
                    {
                        AccountDTO accountDTO = accountDTOList.Find(x => x.TagNumber == item.Value);
                        for (int i = 0; i < transactionDTO.TransactionLinesDTOList.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(transactionDTO.TransactionLinesDTOList[i].CardNumber) == false && transactionDTO.TransactionLinesDTOList[i].CardNumber == item.Key)
                            {
                                transactionDTO.TransactionLinesDTOList[i].CardId = accountDTO.AccountId;
                                transactionDTO.TransactionLinesDTOList[i].CardNumber = accountDTO.TagNumber;
                                transactionDTO.TransactionLinesDTOList[i].CardGuid = accountDTO.Guid;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void IssuePrints(TransactionDTO transactionDTO)
        {
            log.LogMethodEntry("transactionDTO");
            List<TransactionLineDTO> selectedLines = null;
            List<TransactionLineDTO> allSelectedLines = transactionDTO.TransactionLinesDTOList.Where(x => x.Selected).ToList();
            if (allSelectedLines.Any())
            {
                lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, "Printing...") + " " +
                                 MessageContainerList.GetMessage(machineUserContext, "Please Wait");
                DisplayMessageLine(lblStatus.Text);
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "USE_FISCAL_PRINTER"))
                {
                    string msg = MessageContainerList.GetMessage(machineUserContext, 4557)
                                       + ". " + MessageContainerList.GetMessage(machineUserContext, 441);
                    KioskStatic.logToFile(msg);
                    ShowUserAlert(msg);
                    // "Partial print is not allowed when Fiscal Printer is enabled. 
                    log.LogMethodExit("Partial print is not allowed when Fiscal Printer is enabled");
                    return;
                }
                //List<TransactionLineDTO> nonIssuedSelectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false
                //                                                                              && x.CardNumber.StartsWith("T")).ToList();
                //if (nonIssuedSelectedLines.Any() == true)
                //{
                //    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2833));
                //    // "Can print issued cards only. Please uncheck temp cards to proceed"
                //}
                selectedLines = allSelectedLines.Where(x => string.IsNullOrWhiteSpace(x.CardNumber) == false && (x.CardNumber.StartsWith("T") == false)).ToList();
                //if (selectedLines.Any() == false)
                //{
                //    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2834));// "Please select issued cards to print"
                //}
                if (selectedLines.Any())
                {
                    TransactionBL transactionBL = new TransactionBL(machineUserContext, transactionDTO);

                    if (transactionBL.IsPrintableOnlineTransactionLinesExists() == false)
                    {
                        ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, 1705));
                        log.LogMethodExit("No printable transaction line exists.");
                        return;
                    }

                    if (KioskStatic.POSMachineDTO == null || KioskStatic.POSMachineDTO.PosPrinterDtoList == null || KioskStatic.POSMachineDTO.PosPrinterDtoList.Any() == false)
                    {
                        POSMachines posMachine = new POSMachines(machineUserContext, utilities.ParafaitEnv.POSMachineId);
                        KioskStatic.POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
                    }
                    List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                    if (IsVirtualStore() == false)
                    {
                        TransactionUtils TransactionUtils = new TransactionUtils(utilities);
                        Semnox.Parafait.Transaction.Transaction trx = TransactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                        string waiverMsg = string.Empty;
                        foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine tl in trx.TrxLines)
                        {
                            if (tl.CardNumber != null && (tl.CardNumber.StartsWith("T") == false)
                               && (selectedLines == null || selectedLines.Exists(stl => stl.Guid == tl.guid)))
                            {
                                foreach (DataGridViewRow dr in dgvTransactionLines.Rows)
                                {
                                    if (dr.Cells["lineCardNo"].Value != null
                                        && dr.Cells["lineCardNo"].Value.Equals(tl.CardNumber))
                                    {
                                        int lineId = trx.TrxLines.IndexOf(tl);
                                        if ((!tl.CardNumber.StartsWith("T") && !tl.ReceiptPrinted) && trx.IsWaiverSignaturePending(lineId))
                                        {
                                            waiverMsg = waiverMsg + MessageContainerList.GetMessage(machineUserContext, 2353, tl.CardNumber) + Environment.NewLine;
                                            //Waiver signing is pending for transaction line with card number &1.
                                            break;
                                        }

                                    }
                                }
                            }
                        }
                        if (string.IsNullOrEmpty(waiverMsg) == false)
                        {   //Waiver signing is pending do not proceed
                            waiverMsg = waiverMsg + MessageContainerList.GetMessage(machineUserContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                            ShowUserAlert(MessageContainerList.GetMessage(machineUserContext, waiverMsg));
                            log.LogMethodExit(waiverMsg);
                            return;
                        }
                    }
                    ValidateRFIDPrinter(selectedLines);
                    //try
                    //{
                    //    BeforeBackgroundOperation();
                    if (POSPrintersDTOList != null && POSPrintersDTOList.Any())
                    {
                        //remove receipt printer
                        POSPrinterListBL posPrinterBL = new POSPrinterListBL(machineUserContext);
                        POSPrintersDTOList = posPrinterBL.RemovePrinterType(POSPrintersDTOList, PrinterDTO.PrinterTypes.ReceiptPrinter);
                    }
                    bool success = false;
                    if (POSPrintersDTOList != null && POSPrintersDTOList.Any())
                    {
                        success = transactionBL.PrintOnlineTransaction(utilities, fiscalPrinter, POSPrintersDTOList, selectedLines, false);
                    }
                    if (success)
                    {
                        //Your tickets printed successfully
                        lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 4555);
                        DisplayMessageLine(lblStatus.Text);
                    }
                    else
                    {
                        lblStatus.Text = MessageContainerList.GetMessage(machineUserContext, 145);
                        DisplayMessageLine(lblStatus.Text);
                    }
                    //}
                    //finally
                    //{
                    //    AfterBackgroundOperation();
                    //}
                }
            }
            //else
            //{
            //    throw new Exception(MessageContainerList.GetMessage(machineUserContext, 2834));// "Please select issued cards to print"
            //}
            log.LogMethodExit();
        }


        private void ValidateRFIDPrinter(List<TransactionLineDTO> transactionLineDTOList)
        {
            log.LogMethodEntry(transactionLineDTOList);
            if (transactionLineDTOList != null && transactionLineDTOList.Any())
            {
                rfidPrinterDTO = KioskStatic.GetRFIDPrinter(machineUserContext, KioskStatic.Utilities.ParafaitEnv.POSMachineId);
                foreach (TransactionLineDTO trxLine in transactionLineDTOList)
                {
                    if (trxLine.ReceiptPrinted == false && string.IsNullOrWhiteSpace(trxLine.CardNumber) == false
                        && !trxLine.ProductTypeCode.Equals("LOCKERDEPOSIT")
                        && !trxLine.ProductTypeCode.Equals("DEPOSIT") && !trxLine.ProductTypeCode.Equals("CARDDEPOSIT"))
                    {
                        bool wristBandPrintTag = KioskStatic.IsWristBandPrintTag(trxLine.ProductId, rfidPrinterDTO);
                        if (wristBandPrintTag)
                        {
                            KioskStatic.ValidateRFIDPrinter(machineUserContext, utilities.ParafaitEnv.POSMachineId, trxLine.ProductId);
                            break;
                        }
                    }
                }
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

        private void DisplayAssemblyVersion()
        {
            log.LogMethodEntry();

            try
            {
                string version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                lblAppVersion.Text = "v" + version;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while getting App Version:" + ex.Message);
            }

            log.LogMethodExit();
        }

        private bool InvokeCardDispenserCheckStatus(ref int cardPosition, ref string message)
        {
            log.LogMethodEntry(cardPosition, message);
            bool suc = false;
            try
            {
                suc = cardDispenser.checkStatus(ref cardPosition, ref message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in InvokeCardDispenserCheckStatus(): " + ex.Message);
            }
            log.LogMethodExit(suc);
            return suc;
        }


        private void SetReceiptPrintOptions(KioskTransaction onlineTransaction)
        {
            log.LogMethodEntry("onlineTransaction");
            try
            {
                string deliveryMode = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "EXECUTE_ONLINE_RECEIPT_DELIVERY_MODE");
                onlineTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.NONE;
                if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.ASK.ToString())
                {
                    using (frmReceiptDeliveryModeOptions frdmo = new frmReceiptDeliveryModeOptions(machineUserContext, onlineTransaction, IsVirtualStore()))
                    {
                        frdmo.ShowDialog();
                        onlineTransaction = frdmo.GetKioskTransaction;
                    }
                }
                else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.PRINT.ToString())
                {
                    onlineTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.PRINT;
                }
                else if (deliveryMode == KioskTransaction.KioskReceiptDeliveryMode.EMAIL.ToString())
                {
                    if (IsVirtualStore() == false)
                    {
                        using (frmGetEmailDetails fged = new frmGetEmailDetails(machineUserContext, onlineTransaction))
                        {
                            if (fged.ShowDialog() != DialogResult.OK)
                            {
                                //Time scenario. Procced as per config and it transaction has customer email id, email will be sent
                                //kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.EMAIL;
                            }
                        }
                    }
                    else
                    {
                        KioskStatic.logToFile("No email option for Virtual Store");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetReceiptPrintOptions(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void GenerateExecuteOnlineErrorReceipt(KioskTransaction onlineTransaction, int cardCount)
        {
            log.LogMethodEntry("onlineTransaction", cardCount);
            try
            {
                if ((onlineTransaction.RececiptDeliveryMode == KioskTransaction.KioskReceiptDeliveryMode.PRINT)
                    || (onlineTransaction.RececiptDeliveryMode == KioskTransaction.KioskReceiptDeliveryMode.NONE))
                {
                    if (KioskStatic.POSMachineDTO == null || KioskStatic.POSMachineDTO.PosPrinterDtoList == null || KioskStatic.POSMachineDTO.PosPrinterDtoList.Any() == false)
                    {
                        POSMachines posMachine = new POSMachines(machineUserContext, utilities.ParafaitEnv.POSMachineId);
                        KioskStatic.POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
                    }
                    List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                    POSPrinterDTO receiptPOSPrinterDTO = POSPrintersDTOList.Find(p => p.PrinterDTO != null && p.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                    ReceiptClass receiptClass = onlineTransaction.GetExecuteOnlineErrorReceipt(receiptPOSPrinterDTO, cardCount);
                    string documentName = MessageContainerList.GetMessage(machineUserContext, "Execute Online Error Receipt");
                    PrintReceipt(documentName, receiptPOSPrinterDTO, receiptClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GenerateExecuteOnlineErrorReceipt(): " + ex.Message);
            }
            log.LogMethodExit();
        }


        private void GenerateExecuteOnlineReceipt(KioskTransaction onlineTransaction, int cardCount)
        {
            log.LogMethodEntry("onlineTransaction", cardCount);
            try
            {
                if (onlineTransaction.RececiptDeliveryMode == KioskTransaction.KioskReceiptDeliveryMode.PRINT)
                {
                    if (KioskStatic.POSMachineDTO == null || KioskStatic.POSMachineDTO.PosPrinterDtoList == null || KioskStatic.POSMachineDTO.PosPrinterDtoList.Any() == false)
                    {
                        POSMachines posMachine = new POSMachines(machineUserContext, utilities.ParafaitEnv.POSMachineId);
                        KioskStatic.POSMachineDTO.PosPrinterDtoList = posMachine.PopulatePrinterDetails();
                    }
                    List<POSPrinterDTO> POSPrintersDTOList = new List<POSPrinterDTO>(KioskStatic.POSMachineDTO.PosPrinterDtoList);
                    POSPrinterDTO receiptPOSPrinterDTO = POSPrintersDTOList.Find(p => p.PrinterDTO != null && p.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                    ReceiptClass receiptClass = onlineTransaction.GetExecuteOnlineReceipt(receiptPOSPrinterDTO, cardCount);
                    string documentName = MessageContainerList.GetMessage(machineUserContext, "Execute Online Receipt");
                    PrintReceipt(documentName, receiptPOSPrinterDTO, receiptClass);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in GenerateExecuteOnlineReceipt(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void PrintReceipt(string documentName, POSPrinterDTO receiptPOSPrinterDTO, ReceiptClass receiptClass)
        {
            log.LogMethodEntry(documentName, receiptPOSPrinterDTO, receiptClass);
            try
            {
                if (receiptClass == null || receiptClass.TotalLines == 0)
                {
                    KioskStatic.logToFile("Unable to generate Execute Online Receipt. Check the setup");
                }
                else
                {
                    PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext, receiptPOSPrinterDTO.PrinterDTO);
                    System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();

                    bool setupPrintStatus =
                        printerBL.SetupThePrinting(printDocument, utilities.ParafaitEnv.IsClientServer, documentName);
                    if (setupPrintStatus)
                    {
                        int receiptLineIndex = 0;
                        printDocument.PrintPage += (sender, e) =>
                        {
                            e.HasMorePages = printerBL.PrintReceiptToPrinter(receiptClass, ref receiptLineIndex, e);
                        };
                        using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                        {
                            //code to send print document to the printer
                            printDocument.Print();
                        }
                        //MyPrintDocument.Print();
                        if (utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                            printerBL.CutPrinterPaper(printDocument.PrinterSettings.PrinterName);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in PrintReceipt(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void HideKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.HideKeyboard();
                }
                else
                {
                    customKeyboardController.HideKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in HideKeyboardObject() in  Execute Online Transaction screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void CompleteWaiverSigningProcess()
        {
            log.LogMethodEntry(kioskTransaction);
            ResetKioskTimer();
            if (this.kioskTransaction != null && this.kioskTransaction.GetTransactionId > 0)
            {
                using (frmWaiverSigningAlert frmSA = new frmWaiverSigningAlert(this.kioskTransaction))
                {
                    DialogResult dr = frmSA.ShowDialog();
                    kioskTransaction = frmSA.GetKioskTransaction;
                    if (dr == DialogResult.Cancel)
                    {
                        //timeout happened. close current form
                        this.DialogResult = DialogResult.Cancel;
                        Close();
                    }
                }
            }
            else
            {
                log.Info("Skipping CompleteWaiverSigningProcess as kioskTransaction is empty or not having valid transacttion id");
                KioskStatic.logToFile("Skipping CompleteWaiverSigningProcess as kioskTransaction is empty or not having valid transacttion id");
            }
            log.LogMethodExit();
        }
    }
}
