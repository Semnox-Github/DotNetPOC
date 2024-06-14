/*  Date          Modification                                          Done by         Version
 *  10-Sep-2018   Redemption Reversal changes                           Guru S A        2.4.0
 * 1-Jul-2019     Modified to add support for ULC cards                 Lakshminarayana 2.70
 * 21-Aug-2019    Reprint Ticket Receipt change                         Archana         2.80.0
 * 26-Sep-2019    Redemption currency rule enhancement                  Dakshakh        2.8.0
 * 22-Feb-2022    Modified DateTime to ServerDateTime                   Mathew Ninan    2.130.4
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Redemption;
using Logger = Semnox.Core.Utilities.Logger;
using System.Globalization;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public partial class frm_redemption : Form
    {
        DataTable DT_Search = new DataTable();
        DataTable DT_Gifts = new DataTable();
        bool fireevent = true;

        string SwipedCardNumber = "";
        double RedemptionDiscount = 0;

        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        bool _viewRedemptionOnly = false;
        bool _viewTurnInsOnly = false;
        string loginID = ""; //Added 29-May-2017
        private readonly TagNumberParser tagNumberParser;

        DeviceClass cardReader, barcodeScanner;

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal delegate void SetLastActivityTimeDelegate();
        internal SetLastActivityTimeDelegate SetLastActivityTime;
        private string parentScreenNumber = string.Empty;
        
        private VirtualKeyboardController virtualKeyboard; 
        internal string GetParentScreenNumber { get { return parentScreenNumber; } }
        public frm_redemption(Utilities utilities, DeviceClass CardReader, DeviceClass BarcodeScanner, string LoginID, string ScreenNumber, bool ViewRedemptionOnly = false)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016
            log.LogMethodEntry(utilities, CardReader,  BarcodeScanner, LoginID, ScreenNumber, ViewRedemptionOnly);
            this.Utilities = utilities;
            Utilities.setLanguage();
            InitializeComponent();
            InitializeVariables();
            initializeTicketLabelVariants();
            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            _viewRedemptionOnly = ViewRedemptionOnly;
            loginID = LoginID;
            parentScreenNumber = ScreenNumber;
            if (_viewRedemptionOnly)
            {
                tcRedemption.SelectedTab = tpViewRedemptions;
                tcRedemption.TabPages.Remove(tpTurnInGift);                
            }
            else
            {
                string eTicketsTICardHeaderText = "e -$$$";
                lblLoginId.Text = loginID;
                lblScreenNumber.Text = ScreenNumber;
                tcRedemption.SelectedTab = tpTurnInGift;
                eTicketsTICard.HeaderText = eTicketsTICardHeaderText.Replace("$$$", POSStatic.TicketTermVariant);
                tcRedemption.TabPages.Remove(tpViewRedemptions);
            }

            cardReader = CardReader;
            barcodeScanner = BarcodeScanner;

            if (BarcodeScanner != null)
            {
                barcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            else
            {
                initializeBarcodePort();
            }

            if (CardReader != null)
            {
                CardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            }

            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad, btnTurnInKeyPad, btnTurnInSearchKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }
        //End update 29-May-2017

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                FireSetLastActivityTime();
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                string CardNumber = tagNumber.Value;

                try
                {
                    HandleCardRead(CardNumber);
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                FireSetLastActivityTime();
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Utilities.ProcessScannedBarCode(checkScannedEvent.Message, ParafaitEnv.LEFT_TRIM_BARCODE, ParafaitEnv.RIGHT_TRIM_BARCODE);
                try
                {
                    //Thread error fix by threading 15-May-2016
                    this.Invoke((MethodInvoker)delegate
                    {
                        HandleBarcodeRead(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    log.Fatal("Ends-BarCodeScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void initializeTicketLabelVariants()
        {
            log.Debug("Starts-initializeTicketLabelVariants()");
            //eTicketsTICard.HeaderText = eTokens.HeaderText.Replace("$$$", POSStatic.TicketTermVariant);
            log.Debug("Ends-initializeTicketLabelVariants()");
        }

        private void InitializeVariables()
        {
            log.Debug("Starts-InitializeVariables()");
            fireevent = false;
            fireevent = true;

            log.Debug("Ends-InitializeVariables()");
        }

        private void frm_redemption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            if (_viewRedemptionOnly)
            {
                LoadSearchFilterDefaults();
                populateRedemptions();
                Utilities.setLanguage(dgvRedemptionHeader);
                Utilities.setLanguage(dgvRedemptions);
            }
            else
            {
                cmbTurninFromLocation.DataSource = Utilities.executeDataTable(@"select Name, LocationId
                                                                                from location l, LocationType lt
                                                                                where IsTurnInLocation = 'Y'
															                        and l.IsActive = 'Y'
															                        and l.LocationTypeID = lt.LocationTypeId
															                        and LocationType in ('Store', 'Department')");
                cmbTurninFromLocation.DisplayMember = "Name";
                cmbTurninFromLocation.ValueMember = "LocationId";
                cmbTurninFromLocation.SelectedIndex = -1;

                cmbTargetLocation.DataSource = Utilities.executeDataTable("select name, locationId from location where isActive = 'Y' order by 1");
                cmbTargetLocation.DisplayMember = "name";
                cmbTargetLocation.ValueMember = "locationId";
                cmbTargetLocation.SelectedIndex = -1;

                btnTurnInProductSearch.PerformClick();
            }
            FireSetLastActivityTime();
            log.LogMethodExit();
        }

        COMPort BarcodePort;
        void initializeBarcodePort()
        {
            log.Debug("Starts-initializeBarcodePort()");
            // To get the card number from COM.
            int portNumber = Properties.Settings.Default.BarcodeCOMPortNumber;
            if (portNumber <= 0)
            {
                log.Info("Ends-initializeBarcodePort() as portNumber <= 0");
                return;
            }
            BarcodePort = new COMPort(portNumber);
            if (!BarcodePort.Open())
            {
                log.Info("Ends-initializeBarcodePort() as BarcodePort is not open");
                return;
            }

            BarcodePort.comPort.ReceivedBytesThreshold = 3;
            BarcodePort.setReceiveAction = serialDataReceived;
            log.Debug("Ends-initializeBarcodePort()");
        }

        private void serialDataReceived()
        {
            log.Debug("Starts-serialDataReceived()");
            string scannedBarcode = Utilities.ProcessScannedBarCode(BarcodePort.ReceivedData, ParafaitEnv.LEFT_TRIM_BARCODE, ParafaitEnv.RIGHT_TRIM_BARCODE);
            Invoke((MethodInvoker)delegate
            {
                HandleBarcodeRead(scannedBarcode);
            });
            log.Debug("Ends-serialDataReceived()");
        }

        //For Card reading
        public void HandleCardRead(string CardNumber)
        {
            log.Debug("Starts-HandleCardRead(" + CardNumber + ")");
            SwipedCardNumber = CardNumber;
            FireSetLastActivityTime();
            if (SwipedCardNumber != "")
            {
                if (tcRedemption.SelectedTab.Equals(tpViewRedemptions))
                {
                    txtRedemptionCard.Text = SwipedCardNumber;
                    SwipedCardNumber = "";
                    populateRedemptions();
                }
                else
                {
                    DataTable DT = GetCardDetails();
                    string tempCard = SwipedCardNumber;
                    SwipedCardNumber = "";
                    if (DT.Rows.Count > 0)
                    {
                        if (DT.Rows[0]["site_id"] != DBNull.Value && Convert.ToInt32(DT.Rows[0]["site_id"]) != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,110),
                                                         MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                            log.Info("HandleCardRead(" + CardNumber + ") as Card not found. Please issue the card before proceeding");
                            return;
                        }

                        if (DT.Rows[0]["technician_card"].ToString() == "Y")
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,197, tempCard),
                                                         MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                            log.Info("Ends-HandleCardRead(" + CardNumber + ") as Technician Card (" + tempCard + ") not allowed for Transaction");
                            return;
                        }

                        object[] row0 = { DT.Rows[0]["card_number"].ToString(),
                                DT.Rows[0]["customer_name"].ToString(),
                                DT.Rows[0]["vip_customer"].ToString(),
                                DT.Rows[0]["ticket_count"].ToString(),
                                DT.Rows[0]["card_id"],
                                DT.Rows[0]["customer_id"]
                            };
                        dgvTurnInCard.Rows.Clear();
                        dgvTurnInCard.Rows.Add(row0);
                    }
                    else
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,110, tempCard), 
                                                    MessageContainerList.GetMessage(Utilities.ExecutionContext,"Card Scan") +
                                                    MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                    log.Warn("HandleCardRead(" + CardNumber + ") as Card " + tempCard + " not found. Please issue the card before proceeding");
                }
            }
            log.Debug("Ends-HandleCardRead(" + CardNumber + ")");
        }

        //For Barcode
        List<string> ManualTicketsArray = new List<string>();
        public void HandleBarcodeRead(string scannedBarcode)
        {
            log.LogMethodEntry(scannedBarcode);
            FireSetLastActivityTime();
            if (scannedBarcode != "")
            {
                if (tcRedemption.SelectedTab.Equals(tpViewRedemptions))
                {
                    txtProdInfo.Text = scannedBarcode;
                    populateRedemptions();
                }
                else //turn-ins
                {
                    switch (scannedBarcode)
                    {
                        case "SAVER": btnTurnInSave.PerformClick(); return;
                        case "PRINT": btnPrintTurnIn.PerformClick(); return;
                        case "NEWRD": btnTurnInClear.PerformClick(); return;
                    }

                    DataTable dt = getTurnInGifts(scannedBarcode, "%", 'B');
                    if (dt.Rows.Count == 0)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,111), MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                        log.Info("Ends-HandleBarcodeRead(" + scannedBarcode + ") as Product not found");
                    }
                    else
                        dgvTurnInProducts.DataSource = dt;
                }
            }
            log.LogMethodExit();
        }

        private DataTable GetCardDetails()
        {
            log.LogMethodEntry();

            string cmdText = "";
            cmdText = @"SELECT card_id, card_number, customer_name, vip_customer, ticket_count, c.site_id, isnull(m.RedemptionDiscount, 0) redemption_discount,
                                       m.membershipName, technician_card, customer_id
                                  FROM (select card_id, card_number, customer_name, vip_customer, 
                                               isnull(ticket_count, 0) ticket_count, c.site_id,  technician_card, c.customer_id, MembershipId
                                          from cards c left outer join CustomerView(@PassPhrase) cu on c.customer_id = cu.customer_id  
                                         where card_number = @cardno 
                                           and valid_flag = 'Y'
                                       ) c left outer join Membership m on m.MembershipId= c.MembershipId 
                                  WHERE card_number = @cardno ";
            DataTable DT = new DataTable();
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@cardno", SwipedCardNumber);
            sqlParams[1] = new SqlParameter("@PassPhrase", ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CUSTOMER_ENCRYPTION_PASS_PHRASE"));
            DT = Utilities.executeDataTable(cmdText, sqlParams);
            if (DT.Rows.Count > 0)
            {
                CreditPlus creditPlus = new CreditPlus(Utilities);
                DT.Rows[0]["ticket_count"] = Convert.ToInt32(DT.Rows[0]["ticket_count"]) + creditPlus.getCreditPlusTickets(Convert.ToInt32(DT.Rows[0]["card_id"]));
            }
            log.LogMethodExit(DT);
            return DT;
        }

        private void frm_redemption_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            UnregisterDevices();
            if (BarcodePort != null)
                BarcodePort.Close();

            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;

        private void btnSearchRedemptions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                FireSetLastActivityTime();
                if (ValidSearchParams())
                {
                    populateRedemptions();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void populateRedemptions()
        {
            log.LogMethodEntry();
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RedemptionListBL redemptionListBl = new RedemptionListBL();
                List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FETCH_GIFT_REDEMPTIONS_ONLY, "Y"));
                if (String.IsNullOrEmpty(txtRedemptionCard.Text) && String.IsNullOrEmpty(txtRedemptionId.Text) && String.IsNullOrEmpty(txtRedemptionNo.Text))
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, dtpRedeemedFromDate.Value.Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, dtpRedeemedToDate.Value.Date.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (!String.IsNullOrEmpty(txtRedemptionId.Text))
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, txtRedemptionId.Text));
                }

                if (cmbRedemptionStatus.SelectedIndex > 0)
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, cmbRedemptionStatus.SelectedValue.ToString()));
                }
                else
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS_NOT_IN, RedemptionDTO.RedemptionStatusEnum.NEW.ToString() + "," + RedemptionDTO.RedemptionStatusEnum.ABANDONED.ToString() + "," + RedemptionDTO.RedemptionStatusEnum.SUSPENDED.ToString()));
                }

                if (!String.IsNullOrEmpty(txtRedemptionNo.Text))
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO_LIKE, txtRedemptionNo.Text));
                }

                if (!String.IsNullOrEmpty(txtRedemptionCard.Text))
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.PRIMARY_CARD, txtRedemptionCard.Text));
                }

                if (!String.IsNullOrEmpty(txtProdInfo.Text))
                {
                    searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE, txtProdInfo.Text));
                }

                searchParam.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));

                List<RedemptionDTO> redemptionDTOList = redemptionListBl.GetRedemptionDTOList(searchParam);
                redemptionDTOBindingSource.DataSource = redemptionDTOList;
                dgvRedemptionHeader.Columns["redeemedDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                dgvRedemptionHeader.Columns["orderCompletedDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                dgvRedemptionHeader.Columns["orderDeliveredDateDataGridViewTextBoxColumn"].DefaultCellStyle = POSStatic.Utilities.gridViewDateTimeCellStyle();

                if (redemptionDTOList == null || redemptionDTOList.Count == 0)
                {
                    populateRedemptionDetails(-1);
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Cursor.Current = Cursors.Default;
            }

            log.LogMethodExit();
        }

        void populateRedemptionDetails(int redemptionId)
        {
            log.LogMethodEntry(redemptionId);
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                RedemptionGiftsListBL redemptionGiftsListBL = new RedemptionGiftsListBL(Utilities.ExecutionContext);
                List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>> giftSearchParam = new List<KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>>();
                giftSearchParam.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.REDEMPTION_ID, redemptionId.ToString()));
                giftSearchParam.Add(new KeyValuePair<RedemptionGiftsDTO.SearchByParameters, string>(RedemptionGiftsDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                List<RedemptionGiftsDTO> redemptionGiftsDTOList = redemptionGiftsListBL.GetRedemptionGiftsDTOList(giftSearchParam);

                dgvRedemptions.DataSource = redemptionGiftsDTOList;
                dgvRedemptions.ColumnHeadersHeight = 34;
                dgvRedemptions.Columns["giftCode"].Width = 100;
                dgvRedemptions.Columns["productDescription"].Width = 150;
                dgvRedemptions.Columns["tickets"].Width = 50;
                //dgvRedemptions.BackgroundColor = this.BackColor;
                dgvRedemptions.Columns["tickets"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                CreateHeaderCheckBox();
                dgvRedemptions.Refresh();

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                Cursor.Current = Cursors.Default;
            }

            log.LogMethodExit();
        }

        void ReverseRedemption()
        {
            log.LogMethodEntry();
            if (dgvRedemptionHeader.CurrentRow != null)
            {
                RedemptionDTO redemptionDTOForReversal = (RedemptionDTO)dgvRedemptionHeader.CurrentRow.DataBoundItem;
                List<RedemptionGiftsDTO> selectedGiftLinesForReversal = new List<RedemptionGiftsDTO>();
                int totalTicketsForReversal = 0;
                if (redemptionDTOForReversal.OrigRedemptionId > -1)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,128),
                                                MessageContainerList.GetMessage(Utilities.ExecutionContext, "Reversal")
                                                + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber), MessageBoxButtons.OK);
                    log.LogMethodExit("reverseRedemption() - Selected Redemption is a reversal, not allowed to proceed.");
                    return;
                }
                else
                {
                    foreach (DataGridViewRow dgvRedemptionLine in dgvRedemptions.Rows)
                    {
                        if (dgvRedemptionLine.Cells["ReverseGiftLine"].Value != null && dgvRedemptionLine.Cells["ReverseGiftLine"].Value.ToString() == "True"
                            && dgvRedemptionLine.Cells["GiftLineIsReversed"].Value.ToString() == "False")
                        {
                            selectedGiftLinesForReversal.Add((RedemptionGiftsDTO)dgvRedemptionLine.DataBoundItem);
                            totalTicketsForReversal = totalTicketsForReversal + Convert.ToInt32(dgvRedemptionLine.Cells["Tickets"].Value);
                        }
                    }

                    if (selectedGiftLinesForReversal.Count == 0)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2666),
                                                   MessageContainerList.GetMessage(Utilities.ExecutionContext,"Reversal")
                                                   + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber), MessageBoxButtons.OK);//Please select Gifts for reversal
                        log.LogMethodExit("No rows selected for reversal");
                        return;
                    }

                }
                try
                {
                    RedemptionReversalTurnInLimitCheck(totalTicketsForReversal);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(Utilities.ExecutionContext,"Reversal Approval") 
                                                  + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                    log.Error(ex);
                    log.LogMethodExit("Reversal approval required");
                    return;
                }

                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,130),
                                                MessageContainerList.GetMessage(Utilities.ExecutionContext, "Reversal Confirmation")
                                                + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    string trxRemarks = "";
                    GenericDataEntry trxRemarksForm = new GenericDataEntry(1);
                    trxRemarksForm.StartPosition = FormStartPosition.CenterParent;
                    trxRemarksForm.Owner = this;
                    trxRemarksForm.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext,131);
                    trxRemarksForm.DataEntryObjects[0].mandatory = true;
                    trxRemarksForm.DataEntryObjects[0].label = MessageContainerList.GetMessage(Utilities.ExecutionContext,132);
                    if (trxRemarksForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        trxRemarks = trxRemarksForm.DataEntryObjects[0].data;
                        if (string.Empty.Equals(trxRemarks))
                        {
                            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,134),
                                                        MessageContainerList.GetMessage(Utilities.ExecutionContext,132) +MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                            log.LogMethodExit("Ends-reverseRedemption() as Remarks was not entered for Reversal");
                            return;
                        }
                    }
                    else
                    {
                        log.LogMethodExit("Ends-reverseRedemption() as Remarks dialog  was cancelled");
                        return;
                    }

                    SqlCommand cmd = Utilities.getCommand(Utilities.createConnection().BeginTransaction());
                    SqlTransaction cmdTrx = cmd.Transaction;
                    try
                    {
                        RedemptionBL reversalRedemptionBL = new RedemptionBL(Utilities.ExecutionContext);
                        reversalRedemptionBL.CreateReversalRedemption(redemptionDTOForReversal, selectedGiftLinesForReversal, totalTicketsForReversal, Utilities, trxRemarks, cmdTrx);
                        if (reversalRedemptionBL.RedemptionDTO.CardId == -1 && totalTicketsForReversal > 0
                            && (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,138, totalTicketsForReversal, POSStatic.TicketTermVariant),
                                                            MessageContainerList.GetMessage(Utilities.ExecutionContext, "Reversed ") + POSStatic.TicketTermVariant 
                                                            + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber),
                                                           MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes))
                        {
                            Semnox.Parafait.Redemption.PrintRedemptionReceipt printRedemptionReceipt = new Semnox.Parafait.Redemption.PrintRedemptionReceipt(ExecutionContext.GetExecutionContext(), Utilities);
                            printRedemptionReceipt.CreateManualTicketReceipt(totalTicketsForReversal, reversalRedemptionBL, cmdTrx);
                        }
                        cmdTrx.Commit();
                        cmdTrx.Dispose();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                        cmdTrx.Rollback();
                        cmdTrx.Dispose(); ;
                    }
                    populateRedemptions();
                }
            }
            log.LogMethodExit();
        }


        private void btnReverseRedemption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                FireSetLastActivityTime();
                ReverseRedemption();
                FireSetLastActivityTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnPrintRedemption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                FireSetLastActivityTime();
                if (dgvRedemptionHeader.CurrentRow != null)
                {
                    int redemption_id = Convert.ToInt32(dgvRedemptionHeader.CurrentRow.Cells["redemptionIdDataGridViewTextBoxColumn"].Value);
                    PrintRedemptionReceipt.Print(redemption_id);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frm_redemption_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.Debug("Starts-frm_redemption_FormClosed() ");
            if (keypad != null && !keypad.IsDisposed && !keypad.Disposing)
                keypad.Close();

            log.Debug("Ends-frm_redemption_FormClosed() ");
        }
        public void RedemptionReversalTurnInLimitCheck(int ticketsToBeReversed)
        {
            log.LogMethodEntry(ticketsToBeReversed);
            int mgrApprovalLimit = 0;
            try
            {
                mgrApprovalLimit = Convert.ToInt32(Utilities.getParafaitDefaults("REDEMPTION_REVERSAL_LIMIT_FOR_MANAGER_APPROVAL"));
            }
            catch { mgrApprovalLimit = 0; }
            if ((ticketsToBeReversed > mgrApprovalLimit && mgrApprovalLimit != 0) || mgrApprovalLimit == 0)
            {
                int mgrId = -1;                 
                if (!DoManagerAuthenticationCheck(ref mgrId))/*!Authenticate.Manager(ref mgrId))*/
                {
                    log.LogMethodExit("Authentication Error");
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext,268));
                }
            }
            log.LogMethodExit();
        }
        private bool ValidSearchParams()
        {
            log.LogMethodEntry();

            if (dtpRedeemedFromDate.Value > dtpRedeemedToDate.Value)
            {
                //From Date is greater than To Date
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 724), MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                return false;
            }

            if ((dtpRedeemedToDate.Value - dtpRedeemedFromDate.Value).Days >= 365 && String.IsNullOrEmpty(txtRedemptionCard.Text) && String.IsNullOrEmpty(txtRedemptionNo.Text))
            {
                //"Date range provided is greater than a year. Do you want to proceed with search?"
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2667), 
                                                MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber), MessageBoxButtons.YesNo) == DialogResult.No)
                { return false; }
            }

            if (!String.IsNullOrEmpty(txtRedemptionCard.Text) && txtRedemptionCard.Text.Length < 4)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2256), MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                return false;
            }

            log.LogMethodExit(true);
            return true;

        }

        private void btnEditRedemption_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                FireSetLastActivityTime();
                if (dgvRedemptionHeader.CurrentRow != null)
                {
                    if (dgvRedemptionHeader.CurrentRow.Cells["origRedemptionIdDataGridViewTextBoxColumn"].Value.ToString() != "-1"
                        || (dgvRedemptionHeader.CurrentRow.Cells["RedemptionStatus"].Value == DBNull.Value ||
                        dgvRedemptionHeader.CurrentRow.Cells["RedemptionStatus"].Value.ToString() == "DELIVERED"))
                    {
                        //Status update is not allowed for Delivered or Reversed Redemptions
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext,2668), 
                                                    MessageContainerList.GetMessage(Utilities.ExecutionContext,"Update Status") 
                                                    + MessageContainerList.GetMessage(Utilities.ExecutionContext, 2693, parentScreenNumber));
                        return;
                    }
                    else
                    {
                        Redemption.frmEditRedemption frm = new Redemption.frmEditRedemption(Utilities, Convert.ToInt32(dgvRedemptionHeader.CurrentRow.Cells["redemptionIdDataGridViewTextBoxColumn"].Value));
                        frm.Owner = this;
                        frm.SetLastActivityTime += new Redemption.frmEditRedemption.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                        frm.ShowDialog();

                        populateRedemptions();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnExit_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            btnExit.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodExit();
        }

        private void btnExit_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            btnExit.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodExit();
        }

        private void btnReverseRedemption_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnReverseRedemption.BackgroundImage = Properties.Resources.TurnInNormal;
            log.LogMethodExit();
        }

        private void btnReverseRedemption_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            btnReverseRedemption.BackgroundImage = Properties.Resources.TurnInPressed;
            log.LogMethodExit();
        }

        private void btnPrintRedemption_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPrintRedemption.BackgroundImage = Properties.Resources.PrintTrx;
            log.LogMethodExit();
        }

        private void btnPrintRedemption_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            btnPrintRedemption.BackgroundImage = Properties.Resources.PrintTrxPressed;
            log.LogMethodExit();

        }

        private void btnEditRedemption_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnEditRedemption.BackgroundImage = Properties.Resources.OrderSave;
            log.LogMethodExit();
        }

        private void btnEditRedemption_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            btnEditRedemption.BackgroundImage = Properties.Resources.OrderSavePressed;
            log.LogMethodExit();
        }

        private void btnSearchRedemptions_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            btnSearchRedemptions.BackgroundImage = Parafait_POS.Properties.Resources.Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnSearchRedemptions_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnSearchRedemptions.BackgroundImage = Parafait_POS.Properties.Resources.Search_Btn_Normal;
            log.LogMethodExit();
        }

        private void redemptionDTOBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (redemptionDTOBindingSource.Current != null && redemptionDTOBindingSource.Current is RedemptionDTO)
            {
                RedemptionDTO redemptionDTO = redemptionDTOBindingSource.Current as RedemptionDTO;
                populateRedemptionDetails(redemptionDTO.RedemptionId);
            }

            log.LogMethodExit();
        }

        private void dgvRedemptions_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {             //if(dgvRedemptions.Columns[e.ColumnIndex].Name == "ReverseGiftLine")
                {
                    if (dgvRedemptions["GiftLineIsReversed", e.RowIndex].Value.ToString() == "True")
                    {
                        dgvRedemptions["ReverseGiftLine", e.RowIndex].ReadOnly = true;
                    }
                    else
                    {
                        dgvRedemptions["ReverseGiftLine", e.RowIndex].ReadOnly = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvRedemptions_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvRedemptions.Columns[e.ColumnIndex].Name == "ReverseGiftLine")
            {
                if (dgvRedemptions["GiftLineIsReversed", e.RowIndex].Value.ToString() == "True")
                {
                    //dgvRedemptions.Rows[e.RowIndex].DefaultCellStyle = Utilities.gridViewCustomColorRowStyle(System.Drawing.Color.Orange);  
                    dgvRedemptions["ReverseGiftLine", e.RowIndex].Style.BackColor =
                        dgvRedemptions["giftCode", e.RowIndex].Style.BackColor =
                        dgvRedemptions["productDescription", e.RowIndex].Style.BackColor =
                        dgvRedemptions["tickets", e.RowIndex].Style.BackColor = System.Drawing.Color.Orange;

                    dgvRedemptions["ReverseGiftLine", e.RowIndex].Style.SelectionBackColor =
                        dgvRedemptions["giftCode", e.RowIndex].Style.SelectionBackColor =
                        dgvRedemptions["productDescription", e.RowIndex].Style.SelectionBackColor =
                        dgvRedemptions["tickets", e.RowIndex].Style.SelectionBackColor = System.Drawing.Color.Orange;
                }
                else
                {
                    dgvRedemptions["ReverseGiftLine", e.RowIndex].Style.BackColor =
                       dgvRedemptions["giftCode", e.RowIndex].Style.BackColor =
                       dgvRedemptions["productDescription", e.RowIndex].Style.BackColor =
                       dgvRedemptions["tickets", e.RowIndex].Style.BackColor = dgvRedemptions["redemptionGiftsId", e.RowIndex].Style.BackColor;

                    dgvRedemptions["ReverseGiftLine", e.RowIndex].Style.SelectionBackColor =
                       dgvRedemptions["giftCode", e.RowIndex].Style.SelectionBackColor =
                       dgvRedemptions["productDescription", e.RowIndex].Style.SelectionBackColor =
                       dgvRedemptions["tickets", e.RowIndex].Style.SelectionBackColor = dgvRedemptions["redemptionGiftsId", e.RowIndex].Style.SelectionBackColor;
                }
            }
            log.LogMethodExit();
        }


        private void dgvRedemptions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                FireSetLastActivityTime();
                if (e.ColumnIndex > -1)
                {
                    log.LogMethodEntry(sender, e);
                    if (dgvRedemptions.Columns[e.ColumnIndex].Name == "ReverseGiftLine")
                    {
                        if (dgvRedemptions["GiftLineIsReversed", e.RowIndex].Value.ToString() != "True")
                        {
                            DataGridViewCheckBoxCell checkBox = (dgvRedemptions["ReverseGiftLine", e.RowIndex] as DataGridViewCheckBoxCell);
                            if (Convert.ToBoolean(checkBox.Value))
                            {
                                checkBox.Value = false;
                            }
                            else
                            {
                                checkBox.Value = true;
                            }
                            dgvRedemptions.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ReverseGiftLineHeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                FireSetLastActivityTime();
                dgvRedemptions.EndEdit();
                CheckBox headerCheckBox = (sender as CheckBox);
                bool hasLinesForReversal = false;

                foreach (DataGridViewRow row in dgvRedemptions.Rows)
                {
                    if (row.Cells["GiftLineIsReversed"].Value.ToString() != "True")
                    {
                        DataGridViewCheckBoxCell checkBox = (row.Cells["ReverseGiftLine"] as DataGridViewCheckBoxCell);
                        checkBox.Value = headerCheckBox.Checked;
                        hasLinesForReversal = true;
                    }
                }

                if (!hasLinesForReversal)
                {
                    headerCheckBox.Checked = false;
                }

                if (headerCheckBox.Checked)
                {
                    headerCheckBox.Image = Properties.Resources.CheckedNew;
                }
                else
                {
                    headerCheckBox.Image = Properties.Resources.UncheckedNew;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CreateHeaderCheckBox()
        {
            log.LogMethodEntry();
            if (!dgvRedemptions.Controls.ContainsKey("ReverseGiftHeaderCheckBox"))
            {
                CheckBox headerCheckBox = new CheckBox();
                headerCheckBox.Name = "ReverseGiftHeaderCheckBox";
                headerCheckBox.Appearance = System.Windows.Forms.Appearance.Button;
                headerCheckBox.FlatAppearance.BorderSize = 0;
                headerCheckBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
                headerCheckBox.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                headerCheckBox.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                headerCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                headerCheckBox.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
                headerCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
                headerCheckBox.UseVisualStyleBackColor = true;
                headerCheckBox.Image = Parafait_POS.Properties.Resources.UncheckedNew;
                headerCheckBox.ImageAlign = ContentAlignment.BottomCenter;
                headerCheckBox.Font = dgvRedemptions.Font;
                headerCheckBox.Location = new Point(dgvRedemptions.Columns["ReverseGiftLine"].HeaderCell.ContentBounds.X + 40, dgvRedemptions.Columns["ReverseGiftLine"].HeaderCell.ContentBounds.Y - 13);
                headerCheckBox.BackColor = Color.Transparent;
                headerCheckBox.Size = new Size(49, 39);
                headerCheckBox.Click += new EventHandler(ReverseGiftLineHeaderCheckBox_Clicked);
                dgvRedemptions.Controls.Add(headerCheckBox);
            }
            else
            {
                CheckBox headerCheckBox = dgvRedemptions.Controls.Find("ReverseGiftHeaderCheckBox", false)[0] as CheckBox;
                headerCheckBox.Checked = false;
                headerCheckBox.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
            }
            log.LogMethodExit();
        }


        void LoadSearchFilterDefaults()
        {
            log.LogMethodEntry();
            List<KeyValuePair<string, string>> redemptionOrderStatusList = new List<KeyValuePair<string, string>>();
            redemptionOrderStatusList.Add(new KeyValuePair<string, string>("ALL", "All"));
            redemptionOrderStatusList.Add(new KeyValuePair<string, string>("OPEN", "Open"));
            redemptionOrderStatusList.Add(new KeyValuePair<string, string>("PREPARED", "Prepared"));
            redemptionOrderStatusList.Add(new KeyValuePair<string, string>("DELIVERED", "Delivered"));
            cmbRedemptionStatus.DataSource = new BindingSource(redemptionOrderStatusList, null);
            cmbRedemptionStatus.ValueMember = "Key";
            cmbRedemptionStatus.DisplayMember = "Value";

            dtpRedeemedFromDate.Value = ServerDateTime.Now.Date;
            dtpRedeemedToDate.Value = ServerDateTime.Now.Date.AddDays(1);
            log.LogMethodExit();
        }

        private bool DoManagerAuthenticationCheck(ref int managerId)
        {
            log.LogMethodEntry();
            bool returnValue = false;
            string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;//hold pos static manager flag value
            int savRoleId = POSStatic.ParafaitEnv.RoleId;
            try
            {  //pass current redemption user manager flag value
                POSStatic.ParafaitEnv.Manager_Flag = Utilities.ParafaitEnv.Manager_Flag;
                POSStatic.ParafaitEnv.RoleId = Utilities.ParafaitEnv.RoleId;
                returnValue = RedemptionAuthentication.RedemptionAuthenticateManger(cardReader, ref managerId);
            }
            finally
            {
                //restore pos static manager flag value
                POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                POSStatic.ParafaitEnv.RoleId = savRoleId;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        internal void UnregisterDevices()
        {
            log.LogMethodEntry();
            if (barcodeScanner != null)
                barcodeScanner.UnRegister();
            if (cardReader != null)
                cardReader.UnRegister();
            log.LogMethodExit();
        }
        
        internal void RegisterDevices()
        {
            log.LogMethodEntry();
            if (barcodeScanner != null)
            {
                barcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            if (cardReader != null)
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));

            log.LogMethodExit();
        }

        private void SearchFieldKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        }
        private void SearchFieldEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime(); 
            log.LogMethodExit();
        }

        private void SearchFieldMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        }

        private void dtpRedeemedFromDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }

        private void dgvRedemptionHeader_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime(); 
            log.LogMethodExit();
        }

        private void dgvRedemptionHeader_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        }

        private void dgvRedemptions_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        }

        private void dtpRedeemedToDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }

        private void dgvTurnInProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        }


        private void cmbTargetLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastActivityTime();
            log.LogMethodExit();
        } 
    }
}
