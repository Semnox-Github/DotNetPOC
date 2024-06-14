/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Home page UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 *********************************************************************************************
 *2.3.0       05-Jun-2018      Archana/Guru S A     Created 
 *2.4.0       14-Sep-2018      Archana              Modified to support customer registration changes
 *2.70        1-Jul-2019       Lakshminarayana      Modified to add support for ULC cards 
 *2.130.7.0   26-Apr-2022      Guru S A             Enable remote debug option    
 ********************************************************************re************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Languages;

namespace Redemption_Kiosk
{
    public partial class frmRedemptionKioskHomeScreen : frmRedemptionKioskBaseForm
    {
        Utilities utilities = Common.utils;
        DeviceClass cardReaderDevice = null;
        DeviceClass barcodeScannerDevice = null;
        ExecutionContext machineUserContext;
        private bool registerCardDevice = false;
        private bool registerBarCodeDevice = false;
        private readonly TagNumberParser tagNumberParser; 

        public frmRedemptionKioskHomeScreen()
        {
            log.LogMethodEntry();
            InitializeComponent();
            utilities.setLanguage(this);
            Init();
            redemptionOrder = null;
            tagNumberParser = new TagNumberParser(Common.utils.ExecutionContext);
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskHomeScreen_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (redemptionOrder == null)
                    redemptionOrder = new RedemptionBL(Common.utils.ExecutionContext);
                RefreshScreen();
                base.RenderPanelContent(_screenModel, panelHeader, 1);
                base.RenderPanelContent(_screenModel, flpOptions, 2);
                this.KeyPreview = true;
                this.KeyPress += FrmHome_KeyPress;
                RedemptionKioskHelper.SetRemoteDebugIsEnabled(machineUserContext);
                log.LogVariableState("RemoteDebugIsEnabled", RedemptionKioskHelper.RemoteDebugIsEnabled);
                utilities.setLanguage(this);
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
                Close();
            }
            log.LogMethodExit();
        }
        void Init()
        {
            log.LogMethodEntry();
            machineUserContext = Common.utils.ExecutionContext;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ExecutionContext.GetUserId());

            machineUserContext.SetMachineId(utilities.ParafaitEnv.POSMachineId);
            RegisterDevices();
            string message = "";
            try
            {
                try
                {
                    //RedemptionKioskHelper.PrinterStatus();
                    DeviceContainer.PrinterStatus(Common.utils.ExecutionContext);
                }
                catch (Exception ex)
                {
                    log.Info("Printer status Error:" + message);
                    Common.ShowMessage(ex.Message);
                    Application.Exit();
                }
                if (Common.utils.getParafaitDefaults("ENABLE_RDS_SYSTEM").Equals("N"))
                {
                   
                    POSMachines posMachine = new POSMachines(utilities.ExecutionContext, Common.utils.ParafaitEnv.POSMachineId);
                    List<POSPrinterDTO> posPrinterDTOList = posMachine.PopulatePrinterDetails();
                    List<POSPrinterDTO> posRDSPrinterDTOList;
                    if (posPrinterDTOList != null && posPrinterDTOList.Count > 0)
                    {
                        posRDSPrinterDTOList = posPrinterDTOList.Where(printerEntry => printerEntry.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RDSPrinter).ToList();
                        if(posRDSPrinterDTOList == null || posRDSPrinterDTOList.Count == 0)
                        {
                            Common.ShowMessage(utilities.MessageUtils.getMessage(1618));
                            log.Error(utilities.MessageUtils.getMessage(1618));
                            //"RDS Printer is not setup"
                            Application.Exit();
                        }
                        else
                        {
                            int rdsReceiptTemplateId = -1;
                            if (posRDSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO != null)
                            {
                                rdsReceiptTemplateId = posRDSPrinterDTOList[0].ReceiptPrintTemplateHeaderDTO.TemplateId;
                            }
                            if (rdsReceiptTemplateId == -1)
                            {
                                Common.ShowMessage(utilities.MessageUtils.getMessage(1619));
                                //"Redemption Receipt template is not setup for the RDS Printer"
                                log.Error(utilities.MessageUtils.getMessage(1619));
                            }
                            try
                            {
                                //RedemptionKioskHelper.PrinterStatus(printer); 
                                DeviceContainer.PrinterStatus(Common.utils.ExecutionContext, posRDSPrinterDTOList[0].PrinterDTO); 
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                Common.ShowMessage(ex.Message);
                                Application.Exit();
                            }
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(ex.Message);
                UnRegisterDevices();
                Close();
                throw new Exception(ex.Message);

            }
            log.LogMethodExit();
        }




        void RegisterDevices()
        {
            log.LogMethodEntry();
            LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "REDEMPTION_KIOSK_DEVICE"));

            List<LookupValuesDTO> redemptionDeviceValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
            if (redemptionDeviceValueList != null && redemptionDeviceValueList[0].LookupValue == "DeviceToEnable")
            {
                if (redemptionDeviceValueList[0].Description == "CARD")
                {
                    cardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, CardScanCompleteEventHandle);
                    registerCardDevice = true;
                }
                else if (redemptionDeviceValueList[0].Description == "BARCODE")
                {
                    barcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, BarCodeScanCompleteEventHandle);
                    registerBarCodeDevice = true;
                }
                else if (redemptionDeviceValueList[0].Description == "BOTH")
                {
                    cardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, CardScanCompleteEventHandle);
                    barcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, BarCodeScanCompleteEventHandle);
                    registerCardDevice = true;
                    registerBarCodeDevice = true;
                }
                else
                {
                    Common.ShowMessage(Common.utils.MessageUtils.getMessage(1620, "REDEMPTION_KIOSK_DEVICE" + Common.utils.MessageUtils.getMessage(441))); //"Lookup value is not defined for REDEMPTION_KIOSK_DEVICE" + Common.utils.MessageUtils.getMessage(441)));
                }
            }
            log.LogMethodExit();
        }
        internal void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                    ReceivedCardTap(checkScannedEvent.Message, sender as DeviceClass);
                    
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ReceivedCardTap(string eventMessage, DeviceClass deviceClass)
        {
            log.LogMethodEntry();
            ResetTimeOut();
            Card card = null;
            TagNumber tagNumber;
            if (tagNumberParser.TryParse(eventMessage, out tagNumber) == false)
            {
                string message = tagNumberParser.Validate(eventMessage);
                Common.ShowMessage(message);
                log.LogMethodExit(null, "Invalid Tag Number. " + message);
                return;
            }

            string lclCardNumber = tagNumber.Value;
            lclCardNumber = RedemptionKioskHelper.ReverseTopupCardNumber(lclCardNumber);
            try
            {
                card = RedemptionKioskHelper.HandleCardRead(lclCardNumber, deviceClass);
                if (card != null)
                {
                    string mes = redemptionOrder.AddCard(card.CardNumber);
                    RefreshScreen();
                }
            }
            catch (ValidationException ex)
            {
                log.Info(ex.Message);
                try
                {
                    UnRegisterDevices();
                    using (FrmRedemptionKioskAdmin frmRedemptionKioskAdmin = new FrmRedemptionKioskAdmin(Common.utils.ExecutionContext))
                    {
                        frmRedemptionKioskAdmin.ShowDialog();
                        frmRedemptionKioskAdmin.BringToFront();
                    }
                    card = null;
                    ReRegisterDevices();
                }
                catch (Exception exx)
                {
                    log.Error(exx);
                    Common.ShowMessage(exx.Message);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
            }
            ResetTimeOut();
            log.LogMethodExit();
        }

        void RefreshScreen()
        {
            log.LogMethodEntry();
            if (redemptionOrder != null)
            {
                int? totalTickets = 0;
                if (redemptionOrder.RedemptionDTO != null)
                {
                    totalTickets = redemptionOrder.GetTotalTickets();
                }
                btnTicketCount.Text = totalTickets.ToString();
                if (totalTickets == 0 && (redemptionOrder.RedemptionHasCards() == false))
                {
                    btnAddToCard.Enabled = false;
                    btnConsolidate.Enabled = false;
                    btnChooseGift.Enabled = false;
                    btnStartOver.Enabled = false;
                }
                else
                {
                    btnAddToCard.Enabled = true;
                    btnConsolidate.Enabled = true;
                    btnChooseGift.Enabled = true;
                    btnStartOver.Enabled = true;
                }
            }
            else
            {
                redemptionOrder = new RedemptionBL(Common.utils.ExecutionContext);
                btnTicketCount.Text = "0";
                btnAddToCard.Enabled = false;
                btnConsolidate.Enabled = false;
                btnChooseGift.Enabled = false;
                btnStartOver.Enabled = false;
            }
            log.LogMethodExit();
        }
        internal void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    ProcessBarCodeReceived(checkScannedEvent.Message);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ProcessBarCodeReceived(string barcodeNumber)
        {
            log.LogMethodEntry(barcodeNumber);
            ResetTimeOut();
            string scannedBarcode = Common.utils.ProcessScannedBarCode(barcodeNumber, Common.utils.ParafaitEnv.LEFT_TRIM_BARCODE, Common.utils.ParafaitEnv.RIGHT_TRIM_BARCODE);

            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    AddScanedTicketToOrder(RedemptionKioskHelper.ProcessBarcode(scannedBarcode, redemptionOrder));
                    RefreshScreen();
                });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            ResetTimeOut();
            log.LogMethodExit();
        }

        void AddScanedTicketToOrder(TicketReceipt ticketReceipt)
        {
            log.LogMethodEntry(ticketReceipt);
            try
            {
                ResetTimeOut();
                redemptionOrder.AddTicket(ticketReceipt);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Common.ShowMessage(ex.Message);
                return;
            }
            ResetTimeOut();
            log.LogMethodExit();
        }

        void FrmHome_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            base.ResetTimeOut();

            if ((int)e.KeyChar == 3)
            {
                Cursor.Show();
            }
            else if ((int)e.KeyChar == 8)
            {
                Cursor.Hide();
            }
            else if ((int)e.KeyChar == 18)
            {
                Application.Restart();
            }
            else if ((int)e.KeyChar == 5) // ctrl e
            {
                log.Info("Ctrl-E pressed");
                Application.Exit();
            }
            else
            {
                e.Handled = true;
            }

            log.LogMethodExit();
        }


        internal override bool ValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            if (element.ActionScreenId > 0)
            {
                ScreenModel screen = new ScreenModel(element.ActionScreenId);
                if (screen.CodeObjectName == "frmLoadTickets")
                {
                    ResetTimeOut();
                    if (AskForTapCard())
                    {
                        AddToCard();
                    }
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmConsolidate")
                {
                    ResetTimeOut();
                    if (AskForTapCard())
                    {
                        if (ValidateForCustomerRegistration())
                        {
                            ConsolidateTicketReceipt();
                        }
                        else
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmStartOver")
                {
                    ResetTimeOut();
                    StartOver();
                    log.LogMethodExit(false);
                    return false;
                }
                if (screen.CodeObjectName == "frmRedemptionKioskChooseGiftScreen")
                {
                    ResetTimeOut();
                    if (AskForTapCard())
                    {
                        if (ValidateForCustomerRegistration())
                        {
                            return InvokeChooseGift();
                        }
                        else
                        {
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    log.LogMethodExit(false);
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        bool InvokeChooseGift()
        {
            log.LogMethodEntry();
            this.Cursor = Cursors.WaitCursor;
            bool tapCardEventSucces = false;
            tapCardEventSucces = AskForTapCard();
            if(tapCardEventSucces)
            {
                UnRegisterDevices();
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(tapCardEventSucces);
            return tapCardEventSucces; 
        }

        bool AskForTapCard()
        {
            log.LogMethodEntry();
            if (RedemptionBL.IsCardRequiredForRedemption(machineUserContext) && !redemptionOrder.RedemptionHasCards())
            {
                try
                {
                    UnRegisterDevices();
                    using (frmRedemptionKioskTapCard frm = new frmRedemptionKioskTapCard())
                    {
                        frm.ShowDialog();
                        if (frm.CardNumber != null && frm.DialogResult == DialogResult.OK)
                        {
                            try
                            {
                                string mes = redemptionOrder.AddCard(frm.CardNumber);
                                frm.Close();
                                log.LogMethodExit(true);
                                return true;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                Common.ShowMessage(ex.Message + ". " + utilities.MessageUtils.getMessage(441));
                            }
                        }
                        if (frm != null)
                        {
                            frm.Close();
                        }
                    }
                    log.LogMethodExit(false);
                    return false;
                }
                finally
                {
                    ReRegisterDevices();
                }
            }
            else
            {
                log.LogMethodExit(true);
                return true;
            }
        }

        void ConsolidateTicketReceipt()
        {
            log.LogMethodEntry();
            try
            {
                ResetTimeOut();
                UnRegisterDevices();
                BasicValidationsForTicketReceiptConsoliadtion();
                if (Common.ShowDialog(utilities.MessageUtils.getMessage(1626)) == System.Windows.Forms.DialogResult.Yes) //"Do you want to consolidate scanned ticket receipt tickets?"
                {

                    SqlCommand cmd = utilities.getCommand(utilities.createConnection().BeginTransaction());
                    SqlTransaction sqlTrx = cmd.Transaction;
                    try
                    {
                        bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                        TicketReceipt newTicketReceipt = redemptionOrder.ConsolidateTicketReceipts(managerApprovalReceived, "Redemption Kiosk", sqlTrx);
                        PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(machineUserContext, utilities);
                        printRedemptionReceipt.PrintManualTicketReceipt(newTicketReceipt, redemptionOrder, sqlTrx);
                        sqlTrx.Commit();
                        sqlTrx.Dispose();
                        cmd.Dispose();
                        ClearOrderProcess();
                        RefreshScreen();
                    }
                    catch (Exception ex)
                    {
                        Common.ShowMessage(utilities.MessageUtils.getMessage(1627) + ". " + ex.Message);
                        log.Error(ex);
                        sqlTrx.Rollback();
                        sqlTrx.Dispose();
                        cmd.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.ShowMessage(utilities.MessageUtils.getMessage(1627) + ". " + ex.Message);
                log.Error(ex);
            }
            finally
            {
                ResetTimeOut();
                ReRegisterDevices();
            }
            log.LogMethodExit();
        }

        private void BasicValidationsForTicketReceiptConsoliadtion()
        {
            log.LogMethodEntry();
            if (redemptionOrder != null && redemptionOrder.RedemptionDTO != null && (redemptionOrder.RedemptionDTO.TicketReceiptListDTO.Count < 2))
            {
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1623));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1623));
                //Scan multiple tickets receipts for consolidation
            }
            if (redemptionOrder != null && redemptionOrder.RedemptionDTO != null && redemptionOrder.RedemptionDTO.RedemptionGiftsListDTO.Count > 0)
            {
                log.Error(MessageContainerList.GetMessage(machineUserContext, 1624));
                throw new Exception(MessageContainerList.GetMessage(machineUserContext, 1624));
                //Gifts are selected for Redemption. Can not proceed with ticket receipt consolidation
            }
            log.LogMethodExit();
        }

        void AddToCard()
        {
            log.LogMethodEntry();
            try
            {
                ResetTimeOut();
                UnRegisterDevices();
                if (!redemptionOrder.RedemptionHasCards())
                {
                    using (frmRedemptionKioskTapCard frm = new frmRedemptionKioskTapCard())
                    {
                        frm.ShowDialog();
                        if (frm.CardNumber != null && frm.DialogResult == DialogResult.OK)
                        {
                            try
                            {
                                string mes = redemptionOrder.AddCard(frm.CardNumber);
                                frm.Close();
                            }
                            catch (Exception ex)
                            {
                                Common.ShowMessage(ex.Message);
                                return;
                            }
                        }
                        else
                        {
                            if (frm != null)
                            {
                                frm.Close();
                            }
                            return;
                        }
                    }
                }

                try
                {
                    if (ValidateForCustomerRegistration())
                    {
                        redemptionOrder.LoadTicketsToCard(Common.utils, "Redemption Kiosk");

                        if (utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "Y")
                        {
                            PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(machineUserContext, utilities);
                            printRedemptionReceipt.PrintRedemption(redemptionOrder);
                        }
                        else if (utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "A")
                        {
                            if (Common.ShowDialog(utilities.MessageUtils.getMessage(484)) == System.Windows.Forms.DialogResult.Yes)
                            {
                                PrintRedemptionReceipt printRedemptionReceipt = new PrintRedemptionReceipt(machineUserContext, utilities);
                                printRedemptionReceipt.PrintRedemption(redemptionOrder);
                            }
                        }
                        Common.ShowMessage(utilities.MessageUtils.getMessage(1381));
                        ClearOrderProcess();
                        RefreshScreen();
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    Common.ShowMessage(ex.Message);
                }
            }
            finally
            {
                ResetTimeOut();
                ReRegisterDevices();
            }
        }
        public override void StartOver()
        {
            log.LogMethodEntry();
            base.StartOver();
            RefreshScreen();
            log.LogMethodExit();
        }



        private void FrmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UnRegisterDevices();
            log.LogMethodExit();
        }

        private void FrmHome_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //UnRegisterDevices();
            log.LogMethodExit();
        }


        private void UnRegisterDevices()
        {
            log.LogMethodEntry();
            if (cardReaderDevice != null)
            {
                cardReaderDevice.UnRegister();
                cardReaderDevice.Dispose();
                cardReaderDevice = null;
            }

            if (barcodeScannerDevice != null)
            {
                barcodeScannerDevice.UnRegister();
                barcodeScannerDevice.Dispose();
                barcodeScannerDevice = null;
            }
            log.LogMethodExit();
        }

        private void ReRegisterDevices()
        {
            log.LogMethodEntry(); 
            if(registerCardDevice && cardReaderDevice == null)
            {
                cardReaderDevice = RedemptionKioskHelper.RegisterCardReader(this, CardScanCompleteEventHandle);
            }
            if(registerBarCodeDevice && barcodeScannerDevice == null)
            {
                barcodeScannerDevice = RedemptionKioskHelper.RegisterBarCodeScanner(this, BarCodeScanCompleteEventHandle);
            }
            log.LogMethodExit(); 
        }

        private void RegisterCustomer()
        {
            log.LogMethodEntry();
            UnRegisterDevices();
            InactivityTimerSwitch(false);
            if (ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "SHOW_REGISTRATION_AGE_GATE").Equals("Y"))
            {
                using (FrmRedemptionKioskAgeGate frmRedemptionAgeGate = new FrmRedemptionKioskAgeGate(machineUserContext, utilities, redemptionOrder, redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Count == 0 ? null : redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[0].CardNumber))
                {
                    frmRedemptionAgeGate.ageGateStartOver += new FrmRedemptionKioskAgeGate.AgeGateStartOver(StartOver);
                    frmRedemptionAgeGate.ageGateUpdateCustomer += new FrmRedemptionKioskAgeGate.AgeGateUpdateCustomer(UpdateRedemptionOrderCustomer);
                    frmRedemptionAgeGate.BringToFront();
                    frmRedemptionAgeGate.ShowDialog();
                }
            }
            else
            {
                using (FrmRedemptionKioskCustomer frmRedemptionCustomer = new FrmRedemptionKioskCustomer(machineUserContext, utilities, redemptionOrder, redemptionOrder.RedemptionDTO.RedemptionCardsListDTO.Count == 0 ? null : redemptionOrder.RedemptionDTO.RedemptionCardsListDTO[0].CardNumber))
                {
                    frmRedemptionCustomer.startOver += new FrmRedemptionKioskCustomer.StartOver(StartOver);
                    frmRedemptionCustomer.updateCustomer += new FrmRedemptionKioskCustomer.UpdateCustomer(UpdateRedemptionOrderCustomer);
                    frmRedemptionCustomer.BringToFront();
                    frmRedemptionCustomer.ShowDialog();
                }
            }
            InactivityTimerSwitch(true);
            ResetTimeOut();
            ReRegisterDevices();
            log.LogMethodExit();
        }
        void UpdateRedemptionOrderCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            redemptionOrder.UpdateCustomer(customerId);
            log.LogMethodExit();
        }

        private bool ValidateForCustomerRegistration()
        {
            log.LogMethodEntry();
            bool isCustomerRegistered = false;
            if (!redemptionOrder.HasCustomerDetails() && ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "CUSTOMER_REGISTRATION_IS_MANDATORY").Equals("Y"))
            {
                Common.ShowMessage(MessageContainerList.GetMessage(machineUserContext, 1664));
                RegisterCustomer();
                if (redemptionOrder.HasCustomerDetails())
                {
                    log.LogMethodExit(true);
                    isCustomerRegistered = true;
                }
            }
            else
            {
                isCustomerRegistered = true;
            }
            log.LogMethodExit(false);
            return isCustomerRegistered;
        }

        private void frmRedemptionKioskHomeScreen_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            RefreshScreen();
            ReRegisterDevices();
            log.LogMethodExit();
        }


        private void btnTapCardText_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
                //bool remoteDebugIsEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "ENABLE_REMOTE_DEBUG", false);
                log.LogVariableState("RemoteDebugIsEnabled", RedemptionKioskHelper.RemoteDebugIsEnabled);
                if (RedemptionKioskHelper.RemoteDebugIsEnabled)
                {
                    LaunchRemoteDebugMenu();
                } 
            }
            catch (Exception ex)
            { 
                log.Error(ex);
            }
            ResetTimeOut();
            log.LogMethodExit();
        }

        private void LaunchRemoteDebugMenu()
        {
            log.LogMethodEntry();
            try
            {
                using (frmRemoteDebugInput frmRBI = new frmRemoteDebugInput(machineUserContext))
                {
                    if (frmRBI.ShowDialog() == DialogResult.OK)
                    {
                        string cardNumber = frmRBI.GetEnteredCardNumber();
                        string ticketReceiptNumber = frmRBI.GetEnteredTicketReceiptNumber();
                        if (string.IsNullOrWhiteSpace(cardNumber) == false)
                        {
                            ReceivedCardTap(cardNumber, null);
                        }
                        if (string.IsNullOrWhiteSpace(ticketReceiptNumber) == false)
                        {
                            ProcessBarCodeReceived(ticketReceiptNumber);  
                        }
                    }
                    DisableRemoteDebugOption();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void DisableRemoteDebugOption()
        {
            log.LogMethodEntry();
            try
            {
                ParafaitDefaultsListBL parafaitDefaultsListBL = new ParafaitDefaultsListBL(machineUserContext);
                List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.DEFAULT_VALUE_NAME, "ENABLE_REMOTE_DEBUG"));
                searchParameters.Add(new KeyValuePair<ParafaitDefaultsDTO.SearchByParameters, string>(ParafaitDefaultsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<ParafaitDefaultsDTO> parafaitDefaultsDTOList = parafaitDefaultsListBL.GetParafaitDefaults(searchParameters, true, true);
                if (parafaitDefaultsDTOList != null && parafaitDefaultsDTOList.Any())
                {
                    log.LogVariableState("machineUserContext.GetMachineId()", machineUserContext.GetMachineId());
                    for (int i = 0; i < parafaitDefaultsDTOList.Count; i++)
                    {
                        ParafaitDefaultsDTO defaultsDTO = parafaitDefaultsDTOList[i];
                        log.LogVariableState("defaultsDTO", defaultsDTO);
                        if (defaultsDTO != null && defaultsDTO.ParafaitOptionValuesDTOList != null)
                        {
                            bool machineLevelConfigNotUpdated = true;
                            for (int j = 0; j < defaultsDTO.ParafaitOptionValuesDTOList.Count; j++)
                            {
                                ParafaitOptionValuesDTO valuesDTO = defaultsDTO.ParafaitOptionValuesDTOList[j];
                                log.LogVariableState("valuesDTO", valuesDTO);
                                if (valuesDTO.PosMachineId == machineUserContext.GetMachineId())
                                {
                                    valuesDTO.OptionValue = "N"; 
                                    ParafaitOptionValuesBL optionValuesBL = new ParafaitOptionValuesBL(machineUserContext, valuesDTO);
                                    optionValuesBL.Save();
                                    machineLevelConfigNotUpdated = false;
                                    break;
                                }
                            }
                            log.LogVariableState("machineLevelConfigNotUpdated", machineLevelConfigNotUpdated);
                            if (machineLevelConfigNotUpdated)
                            {
                                defaultsDTO.DefaultValue = "N";
                                defaultsDTO.IsChanged = true;
                                ParafaitDefaultsBL defaultsBL = new ParafaitDefaultsBL(machineUserContext, defaultsDTO);
                                defaultsBL.Save();
                                break;
                            }
                        }
                    }
                    RedemptionKioskHelper.RemoteDebugIsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}





