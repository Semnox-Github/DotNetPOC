/********************************************************************************************
 * Project Name - POS - frmTransferLegacyTickets
 * Description  - frmTransferLegacyTickets 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       06-Sep-2018      Guru S A           Modified for redemption reversal changes
 *2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 *2.80         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using LegacyTicketsMigration;
using LegacyTicketsMigration.LegacyTicketsDTO;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction;

namespace Parafait_POS
{
    public partial class frmTransferLegacyTickets : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        ParafaitEnv parafaitEnv;
        private int deviceAddress = -1;
        //RemotingClient CardRoamingRemotingClient = null;
        Card currentCard;
        LegacyTicketsDTO legacyTicketsDTO = null;
        //bool fireEvent = true;
        bool newTransaction;
        private readonly TagNumberParser tagNumberParser;
        class clsCards
        {
            internal string cardNumber;
            internal string customerName;
            internal int customerId;
            internal int cardId;
            internal int tickets;
            internal int totalTickets;
        }
        List<clsCards> cardList = new List<clsCards>();
        public frmTransferLegacyTickets(Utilities utilities)
        {
            log.LogMethodEntry();
            this.utilities = utilities;
            tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
            parafaitEnv = utilities.ParafaitEnv;
            InitializeComponent();
            this.BackColor = utilities.ParafaitEnv.getPOSBackgroundColor();
            StartPosition = FormStartPosition.CenterScreen;
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmTransferLegacyTickets_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RegisterBarcodeScanner();
            RegisterDevices();
            ClearFields();
            newTransaction = true;
            log.LogMethodExit();
        }

        DeviceClass cardReader;
        private bool RegisterDevices()
        {
            log.LogMethodEntry();
            bool response;
            EventHandler mifareCardScanCompleteEvent = new EventHandler(CardScanCompleteEventHandle);
            if (cardReader != null)
            {
                cardReader.Dispose();
            }
            int lcldeviceAddress = 0;
            deviceAddress = lcldeviceAddress;
            response = true;

            try
            {
                string serialNumber = utilities.getParafaitDefaults("CARD_READER_SERIAL_NUMBER").Trim();
                if (!string.IsNullOrEmpty(serialNumber))
                {
                    string[] serialNumbers = serialNumber.Split('|');

                    string lclSerial = serialNumbers[0];
                    if (string.IsNullOrEmpty(lclSerial))
                    {
                        cardReader = new ACR1252U(lclSerial);
                    }
                    else
                        throw new ApplicationException("Not found");
                }
                else
                {
                    cardReader = new ACR1252U(deviceAddress);
                }
            }
            catch
            {
                try
                {
                    cardReader = new ACR122U(deviceAddress);
                }
                catch
                {
                    try
                    {
                        cardReader = new MIBlack(deviceAddress);
                    }
                    catch
                    {
                        response = false;
                    }
                }
            }

            if (response)
                cardReader.Register(mifareCardScanCompleteEvent);
            else
                response = RegisterUSBDevice();

            if (response)
                utilities.getMifareCustomerKey();

            log.LogMethodExit();
            return response;
        }

        class Device
        {
            internal string DeviceName;
            internal string DeviceType;
            internal string DeviceSubType;
            internal string VID, PID, OptString;
        }
        private bool RegisterUSBDevice()
        {
            log.LogMethodEntry();
            List<Device> deviceList = new List<Device>();

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(utilities.ExecutionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "CardReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (utilities.ParafaitEnv.POSMachineId).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {
                foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
                {
                    if (peripheralsList.Vid.ToString().Trim() == string.Empty)
                        continue;
                    Device device = new Device();
                    device.DeviceName = peripheralsList.DeviceName.ToString();
                    device.DeviceType = peripheralsList.DeviceType.ToString();
                    device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                    device.VID = peripheralsList.Vid.ToString().Trim();
                    device.PID = peripheralsList.Pid.ToString().Trim();
                    device.OptString = peripheralsList.OptionalString.ToString().Trim();
                    deviceList.Add(device);
                }
            }

            string USBReaderVID = utilities.getParafaitDefaults("USB_READER_VID");
            string USBReaderPID = utilities.getParafaitDefaults("USB_READER_PID");
            string USBReaderOptionalString = utilities.getParafaitDefaults("USB_READER_OPT_STRING");
            if (USBReaderVID.Trim() != string.Empty)
            {
                string[] optStrings = USBReaderOptionalString.Split('|');
                foreach (string optValue in optStrings)
                {
                    Device device = new Device();
                    device.DeviceName = "Default";
                    device.DeviceType = "CardReader";
                    device.DeviceSubType = "KeyboardWedge";
                    device.VID = USBReaderVID.Trim();
                    device.PID = USBReaderPID.Trim();
                    device.OptString = optValue.ToString();
                    deviceList.Add(device);
                }
            }

            EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);
            if (cardReader != null)
                cardReader.Dispose();
            USBDevice usbCardReader;
            if (IntPtr.Size == 4) //32 bit
                usbCardReader = new KeyboardWedge32();
            else
                usbCardReader = new KeyboardWedge64();
            if (deviceList != null && deviceList.Count > 0)
            {
                Device deviceSelected = deviceList[0];
                bool flag = usbCardReader.InitializeUSBReader(this.MdiParent, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                if (usbCardReader.isOpen)
                {
                    cardReader = usbCardReader;
                    cardReader.Register(currEventHandler);
                    log.LogMethodExit(true);
                    return true;
                }
            }
            DisplayMessageLine(utilities.MessageUtils.getMessage(281), "WARNING");
            log.Info(utilities.MessageUtils.getMessage(281));
            log.LogMethodExit(false);
            return false;
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    DisplayMessageLine(message, "ERROR");
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                try
                {
                    HandleCardRead(tagNumber.Value, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message, "ERROR");
                    log.Error("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void HandleCardRead(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry();
            if (!newTransaction)
            {
                ClearFields();
                newTransaction = true;
            }
            DisplayMessageLine("");
            Card swipedCard = new Card(readerDevice, CardNumber, utilities.ParafaitEnv.LoginID, utilities);

            string message = "";
            if (!POSUtils.refreshCardFromHQ(ref swipedCard, ref message))
            {
                DisplayMessageLine(message, "ERROR");
                log.Error(CardNumber + " unable to refresh card from HQ error:" + message);
                return;
            }
            if (swipedCard.CardStatus != "ISSUED")
            {
                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 459);
                DisplayMessageLine(message, "WARNING");
                log.Error(message);
                return;
            }
            AddCard(swipedCard);
            log.LogMethodExit(CardNumber);
        }

        void AddCard(Card swipedCard)
        {
            log.LogMethodEntry(swipedCard);

            if (cardList.Find(delegate (clsCards item) { return (item.cardNumber == swipedCard.CardNumber); }) != null)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 59));
                return;
            }

            if (swipedCard != null)
            {
                if (cardList.Count > 0)
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 42), "WARNING");
                    log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 42));
                    return;
                }

                if (swipedCard.site_id != DBNull.Value && Convert.ToInt32(swipedCard.site_id) != parafaitEnv.SiteId && parafaitEnv.ALLOW_ROAMING_CARDS == "N")
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 133), "WARNING");
                    log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 133));
                    return;
                }

                if (swipedCard.technician_card.ToString() == "Y")
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 197, swipedCard.CardNumber), "WARNING");
                    log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 197, swipedCard.CardNumber));
                    return;
                }

                clsCards item = new clsCards();
                item.cardNumber = swipedCard.CardNumber;
                item.customerId = swipedCard.customer_id;
                if (swipedCard.customerDTO != null)
                    item.customerName = swipedCard.customerDTO.FirstName + " " + swipedCard.customerDTO.LastName;
                item.cardId = swipedCard.card_id;
                item.tickets = swipedCard.ticket_count;
                item.totalTickets = swipedCard.ticket_count + swipedCard.CreditPlusTickets;
                cardList.Add(item);
                currentCard = swipedCard;
                txtCardNumber.Text = item.cardNumber;
                txtCardTickets.Text = item.totalTickets.ToString();

                DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, "Card") + ": " + swipedCard.CardNumber, "");
                log.LogMethodExit(cardList);
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 110, swipedCard.CardNumber), "WARNING");
                log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 110, swipedCard.CardNumber));
                log.LogMethodExit();
            }
        }
        private void DisplayMessageLine(string message, string msgType = null)
        {
            log.LogMethodEntry(message, msgType);
            switch (msgType)
            {
                case "WARNING": lblMessage.BackColor = Color.Yellow; lblMessage.ForeColor = Color.Black; break;
                case "ERROR": lblMessage.BackColor = Color.Red; lblMessage.ForeColor = Color.White; break;
                case "MESSAGE": lblMessage.BackColor = Color.LightGray; lblMessage.ForeColor = Color.Black; break;
                default: lblMessage.BackColor = Color.LightGray; lblMessage.ForeColor = Color.Black; break;
            }
            lblMessage.Text = message;
            log.LogMethodExit();
        }

        //internal bool RefreshCardFromHQ(ref Card CurrentCard, ref string message)
        //{
        //    log.LogMethodEntry(CurrentCard, message);
        //    try
        //    {
        //        CardUtils cardUtils = new CardUtils(utilities);
        //        return cardUtils.getCardFromHQ(CardRoamingRemotingClient, ref CurrentCard, ref message);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.ToLower().Contains("fault"))
        //        {
        //            message = MessageContainerList.GetMessage(utilities.ExecutionContext,216);
        //            try
        //            {
        //                CardRoamingRemotingClient = new RemotingClient();
        //            }
        //            catch (Exception exe)
        //            {
        //                // POSUtils.ParafaitMessageBox(exe.Message);
        //                log.Error(exe);
        //                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 217);
        //            }
        //        }
        //        else
        //            message = "On-Demand Roaming: " + ex.Message;
        //        return false;
        //    }
        //}

        DeviceClass barcodeScanner;
        private bool RegisterBarcodeScanner()
        {
            log.LogMethodEntry();

            List<Device> deviceList = new List<Device>();

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(utilities.ExecutionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "BarcodeReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (utilities.ParafaitEnv.POSMachineId).ToString()));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
            List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
            if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
            {
                foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
                {
                    if (peripheralsList.Vid.ToString().Trim() == string.Empty)
                        continue;
                    Device device = new Device();
                    device.DeviceName = peripheralsList.DeviceName.ToString();
                    device.DeviceType = peripheralsList.DeviceType.ToString();
                    device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                    device.VID = peripheralsList.Vid.ToString().Trim();
                    device.PID = peripheralsList.Pid.ToString().Trim();
                    device.OptString = peripheralsList.OptionalString.ToString().Trim();
                    deviceList.Add(device);
                }
            }
            string USBReaderVID = utilities.getParafaitDefaults("USB_BARCODE_READER_VID");
            string USBReaderPID = utilities.getParafaitDefaults("USB_BARCODE_READER_PID");
            string USBReaderOptionalString = utilities.getParafaitDefaults("USB_BARCODE_READER_OPT_STRING");

            if (USBReaderVID.Trim() != string.Empty)
            {
                string[] optStrings = USBReaderOptionalString.Split('|');
                foreach (string optValue in optStrings)
                {
                    Device device = new Device();
                    device.DeviceName = "Default";
                    device.DeviceType = "BarcodeReader";
                    device.DeviceSubType = "KeyboardWedge";
                    device.VID = USBReaderVID.Trim();
                    device.PID = USBReaderPID.Trim();
                    device.OptString = optValue.ToString();
                    deviceList.Add(device);
                }
            }
            EventHandler currEventHandler = new EventHandler(BarCodeScanCompleteEventHandle);
            if (barcodeScanner != null)
                barcodeScanner.Dispose();

            USBDevice barcodeListener;
            if (IntPtr.Size == 4) //32 bit
                barcodeListener = new KeyboardWedge32();
            else
                barcodeListener = new KeyboardWedge64();
            if (deviceList != null && deviceList.Count > 0)
            {
                Device deviceSelected = deviceList[0];
                bool flag = barcodeListener.InitializeUSBReader(Application.OpenForms["POS"], deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                if (barcodeListener.isOpen)
                {
                    barcodeListener.Register(currEventHandler);
                    barcodeScanner = barcodeListener;
                    log.LogMethodExit(barcodeScanner);
                    return true;
                }
            }
            DisplayMessageLine("Unable to find USB Bar Code scanner", "ERROR");
            log.LogMethodExit();
            return false;
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = utilities.ProcessScannedBarCode(checkScannedEvent.Message, utilities.ParafaitEnv.LEFT_TRIM_BARCODE, utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                try
                {
                    this.Invoke((MethodInvoker)delegate
                     {
                         ProcessBarcode(scannedBarcode);
                     });
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message, "ERROR");
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }


        void ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);
            DisplayMessageLine("");
            if (!newTransaction)
            {
                ClearFields();
                newTransaction = true;
            }

            legacyTicketsDTO = null;
            txtTicketReceiptNo.Text = "";
            txtTickets.Text = "";
            dtpPicker.Value = (DateTime.Now);
            string message = "";
            LegacyTicketsListBL legacyTicketsListBL = new LegacyTicketsListBL(utilities.ExecutionContext);
            List<KeyValuePair<LegacyTicketsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<LegacyTicketsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<LegacyTicketsDTO.SearchByParameters, string>(LegacyTicketsDTO.SearchByParameters.RECEIPT_NO, barCode));
            List<LegacyTicketsDTO> legacyTicketsDTOList = legacyTicketsListBL.GetLegacyTicketsDTOList(searchParameters);
            if (legacyTicketsDTOList != null && legacyTicketsDTOList.Count > 0)
            {
                if (legacyTicketsDTOList[0].BalanceTickets != 0)
                {
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1525, barCode);
                }
                else if (!legacyTicketsDTOList[0].ActiveFlag)
                {
                    string message2 = "";
                    if (legacyTicketsDTOList[0].LoadToCard == false)
                        message2 = MessageContainerList.GetMessage(utilities.ExecutionContext, 1493, legacyTicketsDTOList[0].ParafaitReceiptNo); //" new receipt " + legacyTicketsDTOList[0].ParafaitReceiptNo + " was issued";
                    else
                    {
                        Card customerCard = null;
                        if (legacyTicketsDTOList[0].ParafaitCardId != -1)
                        {
                            customerCard = new Card(legacyTicketsDTOList[0].ParafaitCardId, utilities.ParafaitEnv.LoginID, utilities);
                        }
                        string custCardNo = "";
                        if (customerCard != null)
                            custCardNo = customerCard.CardNumber;
                        message2 = MessageContainerList.GetMessage(utilities.ExecutionContext, 1494) + " " + custCardNo;// "Receipt Tickets are already transfered to Customer card";
                    }

                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1495, barCode) + "." + message2;
                }
                else if (legacyTicketsDTOList[0].ExpiredWithExpiryDate && legacyTicketsDTOList[0].ExpireDate < DateTime.Now)
                {
                    message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1496, barCode, Convert.ToDateTime(legacyTicketsDTOList[0].ExpireDate).ToString(utilities.ParafaitEnv.DATETIME_FORMAT));
                }
                else
                {
                    txtTicketReceiptNo.Text = legacyTicketsDTOList[0].ReceiptNo;
                    txtTickets.Text = legacyTicketsDTOList[0].Tickets.ToString();
                    legacyTicketsDTO = legacyTicketsDTOList[0];
                    //string dateText = (legacyTicketsDTOList[0].CreationDate).ToString(utilities.ParafaitEnv.DATE_FORMAT);
                    dtpPicker.Value = legacyTicketsDTOList[0].CreationDate;
                    if (legacyTicketsDTOList[0].SuspectFlag)
                    {
                        message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1526, barCode);
                        //message = "Receipt "+ barCode +" is marked as suspect";
                    }
                    else
                        message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1497, barCode);
                }
            }
            else
            {
                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 1498, barCode);
            }
            DisplayMessageLine(message);
            log.LogMethodExit();
        }

        void ClearFields()
        {
            log.LogMethodEntry();
            txtTicketReceiptNo.Text = "";
            txtTickets.Text = "";
            txtCardNumber.Text = "";
            txtCardTickets.Text = "";
            dtpPicker.Value = DateTime.Now;
            //dtpPicker.Enabled = false;
            currentCard = null;
            legacyTicketsDTO = null;
            cardList.Clear();
            btnPrintTicket.Enabled = true;
            btnLoadTicket.Enabled = true;
            DisplayMessageLine("");

            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            newTransaction = true;
            ClearFields();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void frmTransferLegacyTickets_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (cardReader != null)
                cardReader.Dispose();
            if (barcodeScanner != null)
                barcodeScanner.Dispose();
            log.LogMethodExit();
        }

        private void btnPrintTicket_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine("");
            if (legacyTicketsDTO != null && newTransaction == true)
            {
                int tickets;
                clsRedemption redemption = new clsRedemption(utilities, null);
                try
                {
                    tickets = Convert.ToInt32(txtTickets.Text);
                    if (tickets <= 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 34, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT), "WARNING");
                        return;
                    }
                }
                catch
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 34, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT), "WARNING");
                    return;
                }
                if (!ManagerApprovalCheck(tickets))
                    return;
                utilities.ParafaitEnv.ManagerId = -1;

                DateTime oldTicketIssueDate;
                //if (legacyTicketsDTO.UserUpdatedIssueDate != null && legacyTicketsDTO.UserUpdatedIssueDate != legacyTicketsDTO.CreationDate)
                //    oldTicketIssueDate = Convert.ToDateTime(legacyTicketsDTO.UserUpdatedIssueDate);
                //else
                oldTicketIssueDate = legacyTicketsDTO.CreationDate;

                SqlConnection cnn = utilities.createConnection();
                SqlTransaction sqlTrx = cnn.BeginTransaction();
                try
                {
                    SqlCommand cmd = utilities.getCommand(sqlTrx);
                    clsRedemption.clsCards cardEntry = null;
                    if (currentCard != null)
                    {
                        cardEntry = new clsRedemption.clsCards();
                        cardEntry.cardId = cardList[0].cardId;
                        cardEntry.cardNumber = cardList[0].cardNumber;
                        cardEntry.customerName = cardList[0].customerName;
                        cardEntry.customerId = cardList[0].customerId;
                        cardEntry.Tickets = cardList[0].tickets;
                    }
                    int newReceiptId = redemption.TransferLegacyTicketReceipt(legacyTicketsDTO.ReceiptNo, tickets, oldTicketIssueDate, cardEntry, sqlTrx);

                    if (newReceiptId > -1)
                    {
                        TicketReceiptList ticketReceiptList = new TicketReceiptList(utilities.ExecutionContext);
                        List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParam = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                        searchParam.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ID, newReceiptId.ToString()));
                        List<TicketReceiptDTO> ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParam, sqlTrx);
                        if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                        {
                            legacyTicketsDTO.ParafaitReceiptNo = ticketReceiptDTOList[0].ManualTicketReceiptNo;
                            if (legacyTicketsDTO.SuspectFlag)
                            {
                                ticketReceiptDTOList[0].IsSuspected = true;
                                TicketReceipt newTicketReceipt = new TicketReceipt(utilities.ExecutionContext,ticketReceiptDTOList[0]);
                                newTicketReceipt.Save(sqlTrx);
                                ApplicationRemarksDTO applicationRemarksDTO = new ApplicationRemarksDTO(-1, "", "ManualTicketReceipts", ticketReceiptDTOList[0].Guid, legacyTicketsDTO.SuspectReason, true, utilities.ExecutionContext.GetUserId(), DateTime.Now, utilities.ExecutionContext.GetUserId(), DateTime.Now, "", utilities.ExecutionContext.GetSiteId(), false, -1);
                                ApplicationRemarks applicationRemarks = new ApplicationRemarks(utilities.ExecutionContext, applicationRemarksDTO);
                                applicationRemarks.Save(sqlTrx);
                            }
                        }
                        legacyTicketsDTO.LoadToCard = false;
                        if (currentCard != null)
                            legacyTicketsDTO.ParafaitCardId = currentCard.card_id;
                        legacyTicketsDTO.ActiveFlag = false;
                        LegacyTicketsBL legacyTicketsBL = new LegacyTicketsBL(utilities.ExecutionContext, legacyTicketsDTO);
                        legacyTicketsBL.Save(sqlTrx);
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 143, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT));
                        sqlTrx.Commit();
                        cnn.Close();
                        newTransaction = false;
                        btnPrintTicket.Enabled = false;
                    }
                    else
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 1499));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 1499), "ERROR");
                    sqlTrx.Rollback();
                    cnn.Close();
                }
            }
            log.LogMethodExit();
        }

        private void btnLoadTicket_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DisplayMessageLine("");
            int tickets;
            if (currentCard != null && legacyTicketsDTO != null && newTransaction == true)
            {
                try
                {
                    tickets = Convert.ToInt32(txtTickets.Text);
                    if (tickets <= 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 34, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT), "WARNING");
                        return;
                    }

                    if (legacyTicketsDTO.SuspectFlag)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 1527), "WARNING");
                        //DisplayMessageLine("This Ticket Recepipt is marked as suspect. Can not load tickets to the card", "WARNING");
                        return;
                    }
                }
                catch
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 34, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT), "WARNING");
                    return;
                }

                if (!ManagerApprovalCheck(tickets))
                    return;
                string message = "";

                TaskProcs TaskProcs = new TaskProcs(utilities);
                string newTicketBarCode = CreateBarCode(tickets);

                DateTime oldTicketIssueDate;
                //if (legacyTicketsDTO.UserUpdatedIssueDate != null && legacyTicketsDTO.UserUpdatedIssueDate != legacyTicketsDTO.CreationDate)
                //    oldTicketIssueDate = Convert.ToDateTime(legacyTicketsDTO.UserUpdatedIssueDate);
                //else
                oldTicketIssueDate = legacyTicketsDTO.CreationDate;

                SqlConnection cnn = utilities.createConnection();
                SqlTransaction sqlTrx = cnn.BeginTransaction();
                SqlCommand cmd = utilities.getCommand(sqlTrx);
                try
                {
                    TicketReceiptDTO newTicketReceiptDTO = new TicketReceiptDTO(-1, -1, newTicketBarCode, -1, "", false, -1, tickets, tickets, parafaitEnv.LoginID, DateTime.Now, false, -1, oldTicketIssueDate, parafaitEnv.LoginID, DateTime.Now);
                    TicketReceipt newTicketReceipt = new TicketReceipt(utilities.ExecutionContext,newTicketReceiptDTO);
                    newTicketReceipt.Save(sqlTrx);
                    //List<Tuple<string, int, DateTime?>> manualTicketList = new List<Tuple<string, int, DateTime?>>();
                    //manualTicketList.Add(new Tuple<string, int, DateTime?>(newTicketBarCode, tickets, oldTicketIssueDate));
                    clsRedemption redemption = new clsRedemption(utilities, null);
                    redemption.ticketSourceInfoObj = new List<clsRedemption.TicketSourceInfo>();
                    clsRedemption.TicketSourceInfo ticketSourceobj = new clsRedemption.TicketSourceInfo();
                    ticketSourceobj.ticketSource = "Receipt";
                    ticketSourceobj.ticketValue = tickets;
                    ticketSourceobj.receiptNo = newTicketBarCode;
                    ticketSourceobj.sourceCurrencyRuleId = -1;
                    redemption.ticketSourceInfoObj.Add(ticketSourceobj);
                    int redemptionId = redemption.CreateLoadTicketRedemptionOrder(currentCard, redemption.ticketSourceInfoObj, sqlTrx);
                    newTicketReceiptDTO = newTicketReceipt.TicketReceiptDTO;
                    newTicketReceiptDTO.RedemptionId = redemptionId;
                    newTicketReceiptDTO.BalanceTickets = 0;
                    newTicketReceipt = new TicketReceipt(utilities.ExecutionContext,newTicketReceiptDTO);
                    newTicketReceipt.Save(sqlTrx);
                    // This check is done upfront here to skip it in the loadticket method
                    RedemptionBL redemptionBL = new RedemptionBL(redemptionId, utilities.ExecutionContext, sqlTrx);
                    bool managerApprovalReceived = (utilities.ParafaitEnv.ManagerId != -1);
                    redemptionBL.ManualTicketLimitChecks(managerApprovalReceived,tickets);
                    if (!TaskProcs.loadTickets(currentCard, tickets, "Legacy Ticket Migration", redemptionId, ref message, sqlTrx))
                    {
                        utilities.ParafaitEnv.ManagerId = -1;
                        DisplayMessageLine(message, "ERROR");
                        log.Error("loadTickets" + message);
                        sqlTrx.Rollback();
                        cnn.Close();
                    }
                    else
                    {
                        utilities.ParafaitEnv.ManagerId = -1;
                        legacyTicketsDTO.LoadToCard = true;
                        legacyTicketsDTO.ParafaitCardId = currentCard.card_id;
                        legacyTicketsDTO.ActiveFlag = false;
                        legacyTicketsDTO.ParafaitReceiptNo = newTicketBarCode;
                        LegacyTicketsBL legacyTicketsBL = new LegacyTicketsBL(utilities.ExecutionContext, legacyTicketsDTO);
                        legacyTicketsBL.Save(sqlTrx);
                        DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 36, utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT));
                        sqlTrx.Commit();
                        cnn.Close();
                        newTransaction = false;
                        btnLoadTicket.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 1500), "ERROR");
                    sqlTrx.Rollback();
                    cnn.Close();
                }
            }
            log.LogMethodExit();
        }

        string CreateBarCode(int tickets)
        {
            log.LogMethodEntry(tickets);
            string barCode = string.Empty;
            if (tickets > 0)
            {
                TicketStationFactory ticketStationFactory = new TicketStationFactory();
                POSCounterTicketStationBL posCounterTicketStationBL = ticketStationFactory.GetPosCounterTicketStationObject();
                if (posCounterTicketStationBL == null)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2322), "Ticket Station");
                }
                else
                {
                    barCode = posCounterTicketStationBL.GenerateBarCode(tickets);
                    return barCode;
                }
            }
            log.LogMethodExit();
            return barCode;
        }

        private bool ManagerApprovalCheck(int itemCount)
        {
            log.LogMethodEntry(itemCount);
            int mgrApprovalLimit = 0;
            string mgtLimitValue = FetchManagerApprovalLimit();
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToInt32(mgtLimitValue);
            }
            catch { mgrApprovalLimit = 0; }

            if (mgrApprovalLimit > 0 && itemCount > mgrApprovalLimit && utilities.ParafaitEnv.ManagerId == -1)
            {

                if (!Authenticate.Manager(ref utilities.ParafaitEnv.ManagerId))
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, 268), "WARNING");
                    log.LogMethodExit("Manager Authentication Failed");
                    return false;
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        private string FetchManagerApprovalLimit()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
        }

        //private void dtpPicker_ValueChanged(object sender, EventArgs e)
        //{
        //    if (fireEvent)
        //    {
        //        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext,1501));    
        //        if(legacyTicketsDTO != null)
        //        {
        //            legacyTicketsDTO.UserUpdatedIssueDate = dtpPicker.Value.Date;
        //        }           
        //    }
        //}

        private void btnPrintTicket_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.PrintTrxPressed;
            log.LogMethodExit();
        }

        private void btnPrintTicket_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.PrintTrx;
            log.LogMethodExit();
        }

        private void btnLoadTicket_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.LoadTicket_Normal;
            log.LogMethodExit();
        }

        private void btnLoadTicket_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.LoadTicket_Pressed;
            log.LogMethodExit();
        }

        private void btnRefresh_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.ClearTrxPressed;
            log.LogMethodExit();
        }

        private void btnRefresh_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.ClearTrx;
            log.LogMethodExit();
        }

        private void btnClose_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodExit();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodExit();
        }

        private void btnGetReceipt_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string barCode = txtTicketReceiptNo.Text;
            if (!string.IsNullOrEmpty(barCode))
                ProcessBarcode(barCode);
            log.LogMethodExit();
        }

        private void btnGetReceipt_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnGetReceipt_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            b.FlatAppearance.BorderColor = this.BackColor;
            b.BackgroundImage = Properties.Resources.Search_Btn_Normal;
            log.LogMethodExit();
        }

        private void txtTicketReceiptNo_Validating(object sender, CancelEventArgs e)
        {
            log.LogMethodEntry();
            string barCode = txtTicketReceiptNo.Text;
            if (!string.IsNullOrEmpty(barCode))
                ProcessBarcode(barCode);
            log.LogMethodExit();
        }
    }
}
