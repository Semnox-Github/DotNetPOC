/********************************************************************************************************************************************
 * Project Name - FormCardTask
 * Description  - UI for various Task Types
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad Created 
 *2.00        26-Sep-2018      Mathew Ninan   Modified OpenCashDrawer call in Refund Card
 *                                            to use PrinterBL
 *2.40.0      28-Sep-2018      Guru S A           Modified for MulitPoint changes
 *2.50.0      30-Nov-2018      Mathew Ninan   Deprecating StaticDataExchange
 *2.60.0      19-Apr-2019      Nitin Pai      Log manager approval
 *2.70        1-Jul-2019      Lakshminarayana Modified to add support for ULC cards 
 *2.80        22-Aug-2019     Jinto Thomas    Modified consolidate and balance transfer
 *2.80        10-Oct-2019    Girish Kundar   Modified : part of Ticket station enhancement.
 *2.80        07-Apr-2020     Jinto Thomas   Modified: ManagerApprovalCheck() part refund games manager approval
 *                                           Added product name when load multiple is triggered from quantity prompt
 *            25-May-2020     Mathew Ninan   Updated option for Multiple card load to allow issued cards as well in
 *                                           case of CARDSALE products
 *2.90.0      23-Jun-2020      Raghuveera      Variable refund changes to redirect the refund to payment detail screen for supporting multple payment modes
 *2.90.0      19-Aug-2020      Nitin Pai     Card Balance available for balance transfer should include purchase credits also
 *2.100.0     20-Oct-2020     Mathew Ninan   Fixed UI issue related to redeem loyalty points. In case of multiple
 *                                           redeem options, changing redeem value would default the selected value to first row
 *2.120.0     12-Mar-2021     Prajwal        Modified :  Hold card task enhancement
 *2.130.4     22-Feb-2022     Mathew Ninan    Modified DateTime to ServerDateTime 
 *********************************************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing; //-Added on 25-06-2015 for printing bonus details
using System.Linq;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Product;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.POS;

namespace Parafait_POS
{
    partial class FormCardTasks : Form
    {
        string TaskID;
        public Card CurrentCard;
        Card TransferToCard;
        //Card[] ConsolidateCards;
        List<Card> ConsolidateCards;
        public List<Card> LoadMultipleCards;
        public int[] LoadMultipleProducts;
        public List<int> ListTrxId;
        private readonly TagNumberParser tagNumberParser;
        private readonly UlcKeyStore ulcKeyStore;

        DataGridView FromCardDGV;
        DataGridView HoldEntDGV;
        DataGridView ToCardDGV;
        DataGridView LoadTicketDGV;
        DataGridView LoadBonusDGV;
        DataGridView RefundCardDGV;
        DataGridView RealETicketDGV;
        DataGridView ExchangeDGV;
        //DataGridView[] ConsolidateDGV;
        List<DataGridView> ConsolidateDGV;

        const int MAXCONSCARDS = 6;
        const int MAXLOADMULTIPLECARDS = 1000;
        const int MAXLOADMULTIPLEPRODUCTS = 30;
        int ConsCardCount;

        string CardNumber;
        public string ReturnMessage = "";

        byte[] purseDataBuffer;

        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;

        //int Attribute1 = -1;
        int Attribute2 = -1;
        object _Parameter;

        TextBox CurrentTextBox;
        TextBox CurrentAlphanumericTextBox;

        PrintDocument MyPrintDocument; //Added on 25-06-2015    
        int managerId = -1;
        double timeRefundAmount = 0;
        double gameRefundAmount = 0;
        string previousTransactionId = "";
        string previousLineId = "";
        decimal multiplicationFactor = 1;
        int selectedTransferCardProductId = -1;
        int damagedTransferCardProductId = -1;
        bool displayGroupExists = false;
        bool productExists = false;
        bool isDamaged = false;
        string transferCardType = string.Empty;
        Dictionary<string, Image> btnImageDictionary = new Dictionary<string, Image>();
        private Transaction newTrx;
        private Dictionary<string, ApprovalAction> transferCardOTPApprovals = new Dictionary<string, ApprovalAction>();
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        //bool mgrApprovalRequired;

        public FormCardTasks(string inTaskID, Card incard, Utilities pUtilities, object Parameter = null)
        {
            log.LogMethodEntry(inTaskID, incard, pUtilities, Parameter);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            Utilities = pUtilities;
            TaskID = inTaskID;
            CurrentCard = incard;
            Utilities.setLanguage();
            InitializeComponent();

            Utilities.setLanguage(this);

            ulcKeyStore = new UlcKeyStore();
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            _Parameter = Parameter;
            //-following section of code was added on 25-06-2015
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
            managerId = pUtilities.ParafaitEnv.ManagerId;
            //changes on 25-06-2015 end

            multiplicationFactor = 1;
            try
            {
                multiplicationFactor = Convert.ToDecimal(Utilities.getParafaitDefaults("TIME_IN_MINUTES_PER_CREDIT"));
                if (multiplicationFactor == 0)
                {
                    multiplicationFactor = 1;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            log.LogMethodExit();
        }

        //Added new constructor to accept the list of cards for refund multiple cards on 4-sep-2017
        public FormCardTasks(string inTaskID, List<Card> listcards, Utilities pUtilities, object Parameter = null) //Added for accept the list of cards for multiple cards refund
        {
            log.LogMethodEntry(inTaskID, listcards, pUtilities);
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            if (LoadMultipleCards == null)
                LoadMultipleCards = new List<Card>();
            LoadMultipleCards.Clear();
            ListTrxId = new List<int>();
            TaskID = inTaskID;
            LoadMultipleCards = listcards;
            Utilities.setLanguage();
            InitializeComponent();
            Utilities = pUtilities;

            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            _Parameter = Parameter;
            //-following section of code was added on 25-06-2015
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
            managerId = pUtilities.ParafaitEnv.ManagerId;
            //changes on 25-06-2015 end
            log.Debug("Ends-FormCardTasks(" + inTaskID + "," + listcards + "," + pUtilities + "," + Parameter + ")");
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-CardScanCompleteEventHandle()");
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
                        displayMessageLine(ex.Message);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        displayMessageLine(ex.Message);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    displayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                try
                {
                    CardSwiped(tagNumber.Value, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception" + ex.Message);
                }
            }
            log.Debug("Ends-CardScanCompleteEventHandle()");
        }

        private void FormCardTasks_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-FormCardTasks_Load()");
            this.BackColor = Color.Linen;
            btnShowNumPad.BackColor = this.BackColor;
            FillDropDownAttributes();//to load Attributes

            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            // this is a single form with multiple tabs for all card tasks
            // each task is loaded in a tab
            // other tabs are hidden, and the tab corresponding to the task is shown
            while (tabControlTasks.TabPages.Count > 1)
            {
                for (int i = 0; i < tabControlTasks.TabPages.Count; i++)
                {
                    if (tabControlTasks.TabPages[i].Tag != null && tabControlTasks.TabPages[i].Tag.ToString() != TaskID)
                        tabControlTasks.TabPages.RemoveAt(i);
                }
            }

            tabControlTasks.TabPages[0].BackColor = this.BackColor;

            //Begin: Added to retain the form width for all forms except for Refund tasks on July 29 2015
            if (TaskID != TaskProcs.REFUNDCARD)
            {
                this.Width = 772;
                CenterToScreen();
                buttonOK.Location = new Point(200, 441);
                buttonCancel.Location = new Point(450, 441);
                txtRemarks.Width = 600;
                btnAlphanumericCancelndarTransferCard.Location = new Point(472, 27);
            }
            //End: Added to retain the form width for all forms except for Refund tasks on July 29 2015

            switch (TaskID)
            {
                case TaskProcs.TRANSFERCARD:
                    {
                        FromCardDGV = new DataGridView();
                        ToCardDGV = new DataGridView();

                        FromCardDGV.Location = new Point(20, 100);
                        FromCardDGV.BackgroundColor = this.BackColor;
                        FromCardDGV.BorderStyle = BorderStyle.None;
                        FromCardDGV.Height = 10;
                        tabPageTransferCard.Controls.Add(FromCardDGV);

                        CurrentCard = null;
                        this.ActiveControl = textBoxTransferCardNumber;
                        SetupTransferProducts();
                        break;
                    }

                case TaskProcs.HOLDENTITLEMENTS:
                    {
                        HoldEntDGV = new DataGridView();

                        HoldEntDGV.Location = new Point(20, 100);
                        HoldEntDGV.BackgroundColor = this.BackColor;
                        HoldEntDGV.BorderStyle = BorderStyle.None;
                        HoldEntDGV.Height = 10;
                        buttonOK.Text = "HOLD";
                        tabPageHoldEntitlement.Controls.Add(HoldEntDGV);
                        CurrentCard = null;
                        this.ActiveControl = textBoxHoldEntitlementNumber;

                        break;
                    }
                case TaskProcs.LOADTICKETS:
                    {
                        LoadTickets(); break;
                    }
                case TaskProcs.REFUNDCARD:
                    {
                        RefundCard(); break;
                    }
                case TaskProcs.REALETICKET:
                    {
                        RealETicket(); break;
                    }
                case TaskProcs.EXCHANGETOKENFORCREDIT:
                    {
                        ExchangeTokenForCredit(); break;
                    }
                case TaskProcs.EXCHANGECREDITFORTOKEN:
                    {
                        ExchangeCreditForToken(); break;
                    }
                case TaskProcs.CONSOLIDATE:
                    {
                        // initialize the Cards array that will hold the cards to consolidate
                        //ConsolidateCards = new Card[MAXCONSCARDS];
                        ConsolidateCards = new List<Card>();
                        // each card is displayed in a grid. initialize the grid array
                        //ConsolidateDGV = new DataGridView[MAXCONSCARDS];
                        ConsolidateDGV = new List<DataGridView>();
                        displayMessageLine(MessageUtils.getMessage(51));
                        break;
                    }
                case TaskProcs.BALANCETRANSFER:
                    {
                        lblTransfererCardDetails.Text = Utilities.MessageUtils.getMessage(756);
                        lblTransfereeCardDetails.Text = Utilities.MessageUtils.getMessage(757);
                        // initialize the Cards array that will hold the from and to cards
                        //ConsolidateCards = new Card[20];
                        ConsolidateCards = new List<Card>();
                        // each card is displayed in a grid. initialize the grid array
                        ConsolidateDGV = new List<DataGridView>();
                        Utilities.setLanguage(dgvBalanceTransferee);
                        dcTrToCredits.DefaultCellStyle =
                            dcTrToBonus.DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                        dcTrToTickets.DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                        displayMessageLine(MessageUtils.getMessage(749));
                        break;
                    }
                case TaskProcs.LOADMULTIPLE:
                    {
                        // multiple cards are loaded with common products with ease
                        LoadMultipleCards = new List<Card>();
                        LoadMultipleProducts = new int[MAXLOADMULTIPLEPRODUCTS];
                        for (int i = 0; i < MAXLOADMULTIPLEPRODUCTS; i++)
                            LoadMultipleProducts[i] = -1;
                        dgvMultipleCards.BackgroundColor = this.BackColor;
                        dgvProductsAdded.BackgroundColor = this.BackColor;
                        dgvMultipleCards.AllowUserToAddRows = false;
                        dgvMultipleCards.AllowUserToDeleteRows = false;
                        dgvProductsAdded.AllowUserToAddRows = false;
                        dgvProductsAdded.AllowUserToDeleteRows = false;
                        dgvProductsAdded.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgvMultipleCards.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                        displayMessageLine(MessageUtils.getMessage(257));

                        if (_Parameter != null)
                        {
                            groupBox2.Location = new System.Drawing.Point(185, 49);
                            groupBox2.Size = new System.Drawing.Size(503, 267);
                            dgvProductsAdded.Size = new Size(498, 196);

                            groupBox3.Location = new System.Drawing.Point(3, 49);
                            groupBox3.Size = new System.Drawing.Size(178, 267);
                            dgvMultipleCards.Size = new System.Drawing.Size(171, 247);

                            object[] parameters = _Parameter as object[];
                            string productName = Utilities.executeScalar(@"SELECT PRODUCT_NAME
                                                                             FROM PRODUCTS WHERE PRODUCT_ID = @productId",
                                                                          new SqlParameter("@productId", Convert.ToInt32(parameters[0]))).ToString();
                            dgvProductsAdded.Rows.Add(new object[] { Convert.ToInt32(parameters[0]),
                                                         productName,
                                                         null,
                                                         null,
                                                         null,
                                                         null,
                                                         null,
                                                         "CARDSALE" });
                            btnChooseProduct.Enabled = btnRemoveProduct.Enabled = false;
                            if (Convert.ToBoolean(parameters[2]) == false)
                                lblEntitlement.Text = "Points will be added to each card";
                        }
                        else
                        {
                            lblEntitlement.Visible = false;
                        }

                        break;
                    }
                case TaskProcs.LOADBONUS:
                    {
                        LoadBonus(); break;
                    }
                case TaskProcs.DISCOUNT:
                    {
                        populateDiscounts();
                        break;
                    }
                case TaskProcs.REDEEMLOYALTY:
                    {
                        txtLoyaltyRedeemPoints.Text =
                            txtLoyaltyPoints.Text = (CurrentCard.loyalty_points + CurrentCard.RedeemableCreditPlusLoyaltyPoints).ToString(Utilities.getAmountFormat());
                        populateRedemptionRule();
                        PoleDisplay.writeSecondLine("Points: " + txtLoyaltyPoints.Text);
                        break;
                    }
                case TaskProcs.REDEEMVIRTUALPOINTS:
                    {
                        txtVirtualRedeemPoints.Text =
                            txtVirtualPoints.Text = (CurrentCard.CreditPlusVirtualPoints).ToString(Utilities.getAmountFormat());
                        populateRedemptionRule(true);
                        PoleDisplay.writeSecondLine("Points: " + txtVirtualPoints.Text);
                        break;
                    }
                case TaskProcs.SPECIALPRICING:
                    {
                        specialPricing();
                        break;
                    }
                case TaskProcs.REDEEMTICKETSFORBONUS:
                    {
                        txtTicketsAvailable.Text = (CurrentCard.ticket_count + CurrentCard.CreditPlusTickets).ToString(ParafaitEnv.NUMBER_FORMAT);

                        txtTicketsToRedeem.Text = Convert.ToDecimal(CurrentCard.ticket_count + CurrentCard.CreditPlusTickets).ToString(ParafaitEnv.NUMBER_FORMAT);
                        txtBonusEligible.Text = ((CurrentCard.ticket_count + CurrentCard.CreditPlusTickets) / ParafaitEnv.TICKETS_TO_REDEEM_PER_BONUS).ToString(ParafaitEnv.AMOUNT_FORMAT);

                        tpRedeemTicketsForBonus.Text = tpRedeemTicketsForBonus.Text.Replace("Tickets", POSStatic.TicketTermVariant);
                        lblTicketsAvlbl.Text = lblTicketsAvlbl.Text.Replace("Tickets", POSStatic.TicketTermVariant);
                        lblTicketsToRedeem.Text = lblTicketsToRedeem.Text.Replace("Tickets", POSStatic.TicketTermVariant);
                        PoleDisplay.writeSecondLine("Bonus Eligible: " + txtBonusEligible.Text);

                        break;
                    }
                case TaskProcs.SETCHILDSITECODE:
                    {
                        if (CurrentCard == null)
                            displayMessageLine(MessageUtils.getMessage(257));
                        SqlCommand cmd = Utilities.getCommand();
                        cmd.CommandText = @"Select LookupValue, LookupValue + ' - ' + Description as ChildSiteCode from LookupView where LookupName = @LookupName order by 2";
                        cmd.Parameters.AddWithValue("@LookupName", "CHILD_SITES");
                        DataTable dt = new DataTable();
                        try
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                cbSiteCode.DataSource = dt;
                                cbSiteCode.DisplayMember = "ChildSiteCode";
                                cbSiteCode.ValueMember = "LookupValue";
                            }
                        }
                        catch
                        {
                            displayMessageLine("No child site codes set up in the system");
                        }
                        break;
                    }
                case TaskProcs.GETMIFAREGAMEPLAY:
                    {
                        if (CurrentCard == null)
                            displayMessageLine(MessageUtils.getMessage(257));
                        SqlCommand cmd = Utilities.getCommand();
                        break;
                    }
                //Added on 16-jun-2016 for REDEEMBONUSFORTICKET
                case TaskProcs.REDEEMBONUSFORTICKET:
                    {
                        txtBonusAvailable.Text = (CurrentCard.bonus + CurrentCard.CreditPlusBonus).ToString(ParafaitEnv.AMOUNT_FORMAT);
                        txtBonusToRedeem.Text = (CurrentCard.bonus + CurrentCard.CreditPlusBonus).ToString(ParafaitEnv.AMOUNT_FORMAT);
                        txtElgibleTickets.Text = (ParafaitEnv.TICKETS_TO_REDEEM_PER_BONUS * (CurrentCard.bonus + CurrentCard.CreditPlusBonus)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                        PoleDisplay.writeSecondLine("Tickets Eligible: " + txtElgibleTickets.Text);
                        break;
                    }
                case TaskProcs.PAUSETIMEENTITLEMENT:
                    {
                        this.ActiveControl = txtRemarks;
                        textCardNo.Text = CurrentCard.CardNumber;
                        textTimeRemaining.Text = (CurrentCard.time + CurrentCard.CreditPlusTime).ToString() + " " + MessageUtils.getMessage("Minutes");
                        if ((CurrentCard.CreditPlusTickets + CurrentCard.ticket_count) > 0)
                        {
                            txteTicket.Text = (CurrentCard.CreditPlusTickets + CurrentCard.ticket_count).ToString() + " " + MessageUtils.getMessage("Tickets");
                        }
                        else
                        {
                            lblPausetimeEticketBal.Visible = txteTicket.Visible = false;
                        }
                        displayMessageLine(MessageUtils.getMessage("Balance Time will be paused"));
                        break;
                    }
                case TaskProcs.EXCHANGECREDITFORTIME:
                    {
                        ExchangeCreditsForTime();
                        break;
                    }
                case TaskProcs.EXCHANGETIMEFORCREDIT:
                    {
                        ExchangeTimeForCredits();
                        break;
                    }
                //end
                default: break;
            }



            log.Debug("Ends-FormCardTasks_Load()");
        }

        private void CardSwiped(string inCardNumber, DeviceClass readerDevice)
        {
            log.Debug("Starts-CardSwiped(" + inCardNumber + ",readerDevice)");
            // this is triggered when card is swiped when this form is in focus
            CardNumber = inCardNumber;
            switch (TaskID)
            {
                case TaskProcs.TRANSFERCARD:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - TRANSFERCARD");
                        TransferCard();
                        break;
                    }
                case TaskProcs.LOADBONUS:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - LOADBONUS");
                        getLoadBonusCardDetails(CardNumber);
                        break;
                    }
                case TaskProcs.CONSOLIDATE:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - CONSOLIDATE");
                        Consolidate();
                        break;
                    }
                case TaskProcs.BALANCETRANSFER:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - BALANCETRANSFER");
                        BalanceTransfer();
                        break;
                    }
                case TaskProcs.LOADMULTIPLE:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - LOADMULTIPLE");
                        LoadMultiple();
                        break;
                    }
                case TaskProcs.SETCHILDSITECODE:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - SETCHILDSITECODE");
                        //if (miFareReaderObject.s70Card)
                        //{
                        //    displayMessageLine(MessageUtils.getMessage(53));
                        //    return;
                        //}
                        GetSiteCode(readerDevice);
                        break;
                    }
                case TaskProcs.GETMIFAREGAMEPLAY:
                    {
                        //if (miFareReaderObject.s70Card)
                        //{
                        //    displayMessageLine(MessageUtils.getMessage(54));
                        //    clearCardDetails();
                        //    return;
                        //}
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - GETMIFAREGAMEPLAY");
                        int site;
                        purseDataBuffer = new byte[16];
                        txtCardNumberMiFare.Text = CardNumber;

                        CurrentCard = new MifareCard(readerDevice, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                        site = CurrentCard.getChildSiteCode(ref purseDataBuffer, ref ReturnMessage);
                        if (site <= -1)
                            displayMessageLine(ReturnMessage);
                        else
                        {
                            if (ReturnMessage == "NEW")
                            {
                                displayMessageLine(MessageUtils.getMessage(55));
                                log.Info("Ends-CardSwiped(" + inCardNumber + ",readerDevice) as New card. Please issue card and retry");
                                clearCardDetails();
                                return;
                            }
                            else if (ReturnMessage == "INVALID")
                            {
                                displayMessageLine(MessageUtils.getMessage(56));
                                log.Info("Ends-CardSwiped(" + inCardNumber + ",readerDevice) as Invalid card, please check");
                                clearCardDetails();
                                return;
                            }
                            else
                            {
                                displayMessageLine(ReturnMessage);
                                log.Info("CardSwiped(" + inCardNumber + ",readerDevice) as card is " + ReturnMessage);
                            }
                            displayCardDetails();
                        }
                        break;
                    }

                case TaskProcs.HOLDENTITLEMENTS:
                    {
                        log.Info("CardSwiped(" + inCardNumber + ",readerDevice) - HOLDENTITLEMENTS");
                        LoadHoldEntitlements();
                        break;
                    }
            }
            log.Debug("Ends-CardSwiped(" + inCardNumber + ",readerDevice)");
        }

        private void GetSiteCode(DeviceClass readerDevice)
        {
            log.Debug("Starts-GetSiteCode(readerDevice)");
            int siteCode;
            purseDataBuffer = new byte[16];
            txtCardNumber.Text = CardNumber;
            btnChangeSiteCode.Enabled = false;
            CurrentCard = new MifareCard(readerDevice, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            siteCode = CurrentCard.getChildSiteCode(ref purseDataBuffer, ref ReturnMessage);
            if (siteCode <= -1)
            {
                displayMessageLine(ReturnMessage);
                log.Warn("GetSiteCode(readerDevice) - unable to getChildSiteCode error :" + ReturnMessage);
            }
            else
            {
                if (ReturnMessage == "NEW")
                {
                    displayMessageLine(MessageUtils.getMessage(57));
                    log.Warn("GetSiteCode(readerDevice) - Site cannot be set on a New Card, Please issue the card and retry ");
                }
                else if (ReturnMessage == "INVALID")
                    displayMessageLine(MessageUtils.getMessage(56));
                else
                {
                    btnChangeSiteCode.Enabled = true;
                    displayMessageLine(ReturnMessage);
                }
                txtCurrentSiteCode.Text = String.Format("{0:D}", siteCode);

                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"Select Description from LookupView where LookupName = 'CHILD_SITES' and  LookupValue = @LookupValue";
                cmd.Parameters.AddWithValue("@LookupValue", siteCode);
                string SiteDesc = "";
                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    try
                    {
                        SiteDesc = o.ToString();
                    }
                    catch
                    {
                        SiteDesc = "";
                        log.Fatal("Ends-GetSiteCode(readerDevice) due to exception in SiteDesc");
                    }
                }

                if (SiteDesc != "")
                {
                    string siteCodeValue = String.Format("{0:D}", siteCode) + " - " + SiteDesc;
                    txtCurrentSiteCode.Text = siteCodeValue;
                    log.Info("GetSiteCode(readerDevice) - SiteCode:" + siteCodeValue);
                }
            }
            log.Debug("Ends-GetSiteCode(readerDevice)");
        }

        void specialPricing()
        {
            log.Debug("Starts-specialPricing()");
            dgvSpecialPricing.BackgroundColor = this.BackColor;
            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select pricingId, pricingName, Percentage, RequiresManagerApproval " +
                                "from specialPricing where activeflag = 'Y'";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dgvSpecialPricing.Rows.Add();
                dgvSpecialPricing[0, i].Value = dt.Rows[i][0];
                dgvSpecialPricing[1, i].Value = dt.Rows[i][1];
                dgvSpecialPricing[2, i].Value = dt.Rows[i][2];
                dgvSpecialPricing[3, i].Value = dt.Rows[i][3];
            }
            log.Debug("Ends-specialPricing()");
        }

        void populateRedemptionRule(bool isVirtualLoyaltyPoint = false)
        {
            log.LogMethodEntry(isVirtualLoyaltyPoint);
            SqlCommand cmd = Utilities.getCommand();
            if (isVirtualLoyaltyPoint)
            {
                cmd.CommandText = "select DBColumnName, attribute, convert(varchar, RedemptionValue) + ' for ' + convert(varchar, VirtualLoyaltyPoints) as \"Rule\", " +
                                @"RedemptionValue * (case MultiplesOnly 
                                                when 'Y' then Convert(int, ((case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) / (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end))) * (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end)
                                                else (case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) end)
                                                / case VirtualLoyaltyPoints when 0 then null else VirtualLoyaltyPoints end Redemption_value, " +
                                 "MinimumPoints \"Min Points\", MultiplesOnly \"Multiples Only\", RedemptionValue Rate, VirtualLoyaltyPoints " +
                                 "from LoyaltyRedemptionRule lrr, LoyaltyAttributes la " +
                                 "where lrr.LoyaltyAttributeId = la.LoyaltyAttributeId " +
                                 "and lrr.activeflag = 'Y' " +
                                 "and (lrr.ExpiryDate is null or lrr.ExpiryDate >= getdate())";
                try
                {
                    cmd.Parameters.AddWithValue("@loyaltyPoints", Convert.ToDecimal(txtVirtualRedeemPoints.Text));
                }
                catch
                {
                    cmd.Parameters.AddWithValue("@loyaltyPoints", 0);
                    log.Fatal("Ends-populateRedemptionRule() due to exception in txtLoyaltyRedeemPoints");
                }
            }
            else
            {
                cmd.CommandText = "select DBColumnName, attribute, convert(varchar, RedemptionValue) + ' for ' + convert(varchar, LoyaltyPoints) as \"Rule\", " +
                                 @"RedemptionValue * (case MultiplesOnly 
                                                when 'Y' then Convert(int, ((case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) / (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end))) * (case when isnull(MinimumPoints, 1) = 0 then 1 else MinimumPoints end)
                                                else (case when @loyaltyPoints < isnull(MinimumPoints, 0) then 0 else @loyaltyPoints end) end)
                                                / case LoyaltyPoints when 0 then null else LoyaltyPoints end Redemption_value, " +
                                  "MinimumPoints \"Min Points\", MultiplesOnly \"Multiples Only\", RedemptionValue Rate, LoyaltyPoints " +
                                  "from LoyaltyRedemptionRule lrr, LoyaltyAttributes la " +
                                  "where lrr.LoyaltyAttributeId = la.LoyaltyAttributeId " +
                                  "and lrr.activeflag = 'Y' " +
                                  "and (lrr.ExpiryDate is null or lrr.ExpiryDate >= getdate())";
                try
                {
                    cmd.Parameters.AddWithValue("@loyaltyPoints", Convert.ToDecimal(txtLoyaltyRedeemPoints.Text));
                }
                catch
                {
                    cmd.Parameters.AddWithValue("@loyaltyPoints", 0);
                    log.Fatal("Ends-populateRedemptionRule() due to exception in txtLoyaltyRedeemPoints");
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (isVirtualLoyaltyPoint)
            {
                dgvVirtualPointRedemption.DataSource = dt;
                dgvVirtualPointRedemption.Columns[1].Visible = false;
                dgvVirtualPointRedemption.Columns[dgvVirtualPointRedemption.Columns.Count - 1].Visible = false;
                dgvVirtualPointRedemption.Columns[dgvVirtualPointRedemption.Columns.Count - 2].Visible = false;
                Utilities.setupDataGridProperties(ref dgvVirtualPointRedemption);
                dgvVirtualPointRedemption.BackgroundColor = this.BackColor;
                dgvVirtualPointRedemption.Columns[4].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvVirtualPointRedemption.Columns[5].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                if (dgvVirtualPointRedemption.Rows.Count > 0)
                {
                    dgvVirtualPointRedemption.Rows[0].Selected = true;
                }
                DataGridViewCellStyle Style = new DataGridViewCellStyle();
                Style.Font = new System.Drawing.Font(dgvVirtualPointRedemption.DefaultCellStyle.Font.Name, 12.0F, FontStyle.Regular);
                dgvVirtualPointRedemption.DefaultCellStyle = Style;
                Utilities.setLanguage(dgvVirtualPointRedemption);
            }
            else
            {
                dgvLoyaltyRedemption.DataSource = dt;
                dgvLoyaltyRedemption.Columns[1].Visible = false;
                dgvLoyaltyRedemption.Columns[dgvLoyaltyRedemption.Columns.Count - 1].Visible = false;
                dgvLoyaltyRedemption.Columns[dgvLoyaltyRedemption.Columns.Count - 2].Visible = false;
                Utilities.setupDataGridProperties(ref dgvLoyaltyRedemption);
                dgvLoyaltyRedemption.BackgroundColor = this.BackColor;
                dgvLoyaltyRedemption.Columns[4].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                dgvLoyaltyRedemption.Columns[5].DefaultCellStyle = Utilities.gridViewAmountCellStyle();
                if (dgvLoyaltyRedemption.Rows.Count > 0)
                {
                    dgvLoyaltyRedemption.Rows[0].Selected = true;
                }
                DataGridViewCellStyle Style = new DataGridViewCellStyle();
                Style.Font = new System.Drawing.Font(dgvLoyaltyRedemption.DefaultCellStyle.Font.Name, 12.0F, FontStyle.Regular);
                dgvLoyaltyRedemption.DefaultCellStyle = Style;
                Utilities.setLanguage(dgvLoyaltyRedemption);
            }
            log.LogMethodExit();
        }

        private void populateDiscounts()
        {
            log.Debug("Starts-populateDiscounts()");
            List<DiscountsDTO> discountsDTOList = new List<DiscountsDTO>();
            List<DiscountsDTO> transactionDiscountsDTOList = null;
            using(UnitOfWork unitOfWork = new UnitOfWork())
            {
                DiscountsListBL discountsListBL = new DiscountsListBL(Utilities.ExecutionContext, unitOfWork);
                SearchParameterList<DiscountsDTO.SearchByParameters> searchDiscountsParameters = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, (POSStatic.ParafaitEnv.IsCorporate ? POSStatic.ParafaitEnv.SiteId : -1).ToString()));
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_TRANSACTION));
                transactionDiscountsDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParameters);

                if (transactionDiscountsDTOList != null)
                {
                    discountsDTOList.AddRange(transactionDiscountsDTOList);
                }
                searchDiscountsParameters.Clear();
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, (POSStatic.ParafaitEnv.IsCorporate ? POSStatic.ParafaitEnv.SiteId : -1).ToString()));
                searchDiscountsParameters.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_GAMEPLAY));
                List<DiscountsDTO> gamePlayDiscountsDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParameters);
                if (gamePlayDiscountsDTOList != null)
                {
                    discountsDTOList.AddRange(gamePlayDiscountsDTOList);
                }
            }


            /*discountsDTOList.Insert(0, new DiscountsDTO());
            discountsDTOList[0].DiscountId = -1;
            discountsDTOList[0].DiscountName = "<SELECT>";*/

            foreach (var discountsDTO in discountsDTOList)
            {
                if (discountsDTO.DiscountId != -1)
                {
                    string discountPercentage = discountsDTO.DiscountPercentage == null ? "0" : discountsDTO.DiscountPercentage.ToString();
                    string discountType = discountsDTO.DiscountType == DiscountsBL.DISCOUNT_TYPE_TRANSACTION ? "Trx" : "Game Play";
                    discountsDTO.DiscountName = discountsDTO.DiscountName + " (" + discountPercentage + "% - " + discountType + ")";
                }
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = discountsDTOList;
            cmbDiscount.DisplayMember = "DiscountName";
            cmbDiscount.ValueMember = "DiscountId";
            cmbDiscount.DataSource = bs;

            bs = new BindingSource();
            bs.DataSource = discountsDTOList;
            dgvCardDiscountsDTOListDiscountIdComboBoxColumn.DataSource = bs;
            dgvCardDiscountsDTOListDiscountIdComboBoxColumn.ValueMember = "DiscountId";
            dgvCardDiscountsDTOListDiscountIdComboBoxColumn.DisplayMember = "DiscountName";

            CardDiscountsListBL cardDiscountsListBL = new CardDiscountsListBL();
            List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>> searchCardDiscountsParameters = new List<KeyValuePair<CardDiscountsDTO.SearchByParameters, string>>();
            searchCardDiscountsParameters.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
            searchCardDiscountsParameters.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.CARD_ID, CurrentCard.card_id.ToString()));
            searchCardDiscountsParameters.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.SITE_ID, (POSStatic.ParafaitEnv.IsCorporate ? POSStatic.ParafaitEnv.SiteId : -1).ToString()));
            searchCardDiscountsParameters.Add(new KeyValuePair<CardDiscountsDTO.SearchByParameters, string>(CardDiscountsDTO.SearchByParameters.EXPIRY_DATE_GREATER_THAN, POSStatic.Utilities.getServerTime().ToString("yyyy/MM/dd HH:mm:ss")));
            List<CardDiscountsDTO> cardDiscountsDTOList = cardDiscountsListBL.GetCardDiscountsDTOList(searchCardDiscountsParameters);
            SortableBindingList<CardDiscountsDTO> cardDiscountsDTOSortableList;
            if (cardDiscountsDTOList != null)
            {
                cardDiscountsDTOSortableList = new SortableBindingList<CardDiscountsDTO>(cardDiscountsDTOList);
            }
            else
            {
                cardDiscountsDTOSortableList = new SortableBindingList<CardDiscountsDTO>();
            }
            cardDiscountsDTOListBS.DataSource = cardDiscountsDTOSortableList;


            dtpDiscountExpiryDate.Value = ServerDateTime.Now.Date.AddDays(1);
            rbNever.Checked = true;
            dtpDiscountExpiryDate.Enabled = false;
            Utilities.setupDataGridProperties(ref dgvCardDiscountsDTOList);
            dgvCardDiscountsDTOList.Columns[dgvCardDiscountsDTOListExpiryDateTextBoxColumn.Index].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvCardDiscountsDTOList.BackgroundColor = tabPageDiscount.BackColor;
            Utilities.setLanguage(dgvCardDiscountsDTOList);


            /*SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select cd.discount_id, discount_name, expiry_date " + 
                                "from CardDiscounts cd, discounts d " + 
                                "where card_id = @card_id " +
                                " AND ISNULL(cd.IsActive, 'Y') = 'Y' " +
                                "and d.discount_id = cd.discount_id";
            cmd.Parameters.AddWithValue("@card_id", CurrentCard.card_id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dgvCardDiscounts.Rows.Add(new object[] {dt.Rows[i]["discount_id"],
                                                        dt.Rows[i]["discount_name"],
                                                        dt.Rows[i]["expiry_date"]});
            }
            Utilities.setupDataGridProperties(ref dgvCardDiscounts);
            dgvCardDiscounts.Columns["expiry_date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvCardDiscounts.BackgroundColor = tabPageDiscount.BackColor;

            cmd.CommandText = "select discount_name + ' (' + convert(varchar, isnull(discount_percentage, 0)) + '% - ' + case discount_type when 'T' then 'Trx' else 'Game Play' end + ')' discount_name, " + 
                                "discount_id from discounts where active_flag = 'Y' and discount_type in ('T', 'G') order by discount_type, discount_name";
            da.SelectCommand = cmd;
            DataTable dt1 = new DataTable();
            da.Fill(dt1);
            cmbDiscount.DataSource = dt1;
            cmbDiscount.DisplayMember = "discount_name";
            cmbDiscount.ValueMember = "discount_id";*/

            log.Debug("Ends-populateDiscounts()");
        }

        //Begin Modificatoion: Added to load game amd time Drop down with credit, Card Balance and Bonus attributes//
        void FillDropDownAttributes()
        {
            log.Debug("Starts-FillDropDownAttributes()");
            Dictionary<string, string> cmbAttributes = new Dictionary<string, string>();
            cmbAttributes.Add("", "");
            cmbAttributes.Add("Credits", "C");
            cmbAttributes.Add("Card Balance", "A");
            cmbAttributes.Add("Bonus", "");
            cmbLoadTimeAttributes.DataSource = new BindingSource(cmbAttributes, null);
            cmbLoadTimeAttributes.DisplayMember = "Key";
            cmbLoadTimeAttributes.ValueMember = "Value";
            cmbLoadGameAttributes.DataSource = new BindingSource(cmbAttributes, null);
            cmbLoadGameAttributes.DisplayMember = "Key";
            cmbLoadGameAttributes.ValueMember = "Value";
            log.Debug("Ends-FillDropDownAttributes()");
        }
        //End : Added to load game amd time Drop down with credit, Card Balance and Bonus attributes//

        /// <summary>
        /// Get/Set method of the NewTrx field
        /// </summary>
        public Transaction NewTrx
        {
            get { return newTrx; }
        }
        
        /// <summary>
        /// Get/Set method of the NewTrx field
        /// </summary>
        public string TransferCardType
        {
            get { return transferCardType; }
        }

        /// <summary>
        /// Get/Set method of the TransferCardOTPApprovals field
        /// </summary>
        public Dictionary<string, ApprovalAction> TransferCardOTPApprovals
        {
            get { return transferCardOTPApprovals; }
        }

        private void SetupTransferProducts()
        {
            log.LogMethodEntry();
            double cardButtonSize = 1;
            try
            {
                cardButtonSize = Convert.ToDouble(Utilities.getParafaitDefaults("CARD_PRODUCT_BUTTON_SIZE")) / 100.0;
            }
            //catch { }            
            catch (Exception ex)
            {
                log.Fatal("Ends-initializeProductTab() cardProductButtonSize due to exception " + ex.Message);//Added for logger function on 29-Feb-2016
            }
            string displayGroupGuid = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "LOST_CARD_PRODUCTS_DISPLAY_GROUP");
            string damagedCardProductGuid = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DAMAGED_CARD_TRANSFER_PRODUCT");
            List<int> displayGroupIdList = new List<int>();
            if (string.IsNullOrEmpty(displayGroupGuid) == false)
            {
                ProductDisplayGroupFormatContainerDTO productDisplayGroupFormatContainerDTO = ProductDisplayGroupFormatContainerList.GetProductDisplayGroupFormatContainerDTOCollection(Utilities.ExecutionContext.GetSiteId()).ProductDisplayGroupFormatContainerDTOList.FirstOrDefault(x => x.Guid.Equals(displayGroupGuid, StringComparison.InvariantCultureIgnoreCase));
                if (productDisplayGroupFormatContainerDTO != null)
                {
                    displayGroupIdList.Add(productDisplayGroupFormatContainerDTO.Id);
                }
            }
            List<ProductsContainerDTO> productsContainerDTOList = new List<ProductsContainerDTO>();
            if (displayGroupIdList.Any())
            {
                displayGroupExists = true;
                productsContainerDTOList = ProductsContainerList.GetProductContainerDTOListOfDisplayGroups(Utilities.ExecutionContext.SiteId, displayGroupIdList).Where(x => x.IsTransferCard == true && x.ProductType == "CARDSALE").ToList();
            }
            if (string.IsNullOrEmpty(damagedCardProductGuid) == false)
            {
                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext, damagedCardProductGuid);
                //productsContainerDTOList.Add(productsContainerDTO);
                damagedTransferCardProductId = productsContainerDTO.ProductId;
            }
            flowLayoutPanel1.Controls.Clear();
            if (productsContainerDTOList != null && productsContainerDTOList.Any())
            {
                productExists = true;
                productsContainerDTOList = productsContainerDTOList.OrderBy(x => (x.SortOrder == -1 ? 99999999 : x.SortOrder)).ThenBy(x => x.ProductName).ToList();
                for (int i = 0; i < productsContainerDTOList.Count; i++)
                {
                    Button ProductButton = new Button();
                    ProductButton.Click += new EventHandler(MyToggleButton_Click);
                    //ProductButton.MouseDown += ProductButton_MouseDown;
                    //ProductButton.MouseUp += ProductButton_MouseUp;
                    ProductButton.Name = "ProductButton" + productsContainerDTOList[i].ProductId.ToString();
                    ProductButton.Text = productsContainerDTOList[i].ProductName;
                    ProductButton.Tag = productsContainerDTOList[i].ProductId;
                    ProductButton.Font = sampleButtonCardSaleProduct.Font;
                    ProductButton.Size = sampleButtonCardSaleProduct.Size;
                    ProductButton.FlatStyle = sampleButtonCardSaleProduct.FlatStyle;
                    ProductButton.FlatAppearance.BorderColor = sampleButtonCardSaleProduct.FlatAppearance.BorderColor;
                    ProductButton.FlatAppearance.BorderSize = sampleButtonCardSaleProduct.FlatAppearance.BorderSize;
                    ProductButton.BackgroundImageLayout = ImageLayout.Zoom;
                    ProductButton.BackColor = Color.Transparent;
                    ProductButton.BackgroundImage = sampleButtonCardSaleProduct.BackgroundImage;
                    ProductButton.ForeColor = sampleButtonCardSaleProduct.ForeColor;

                    // Create the ToolTip and associate with the Form container.
                    ToolTip toolTip = new ToolTip();

                    // Set up the delays for the ToolTip.
                    toolTip.AutoPopDelay = 5000;
                    toolTip.InitialDelay = 1000;
                    toolTip.ReshowDelay = 500;
                    // Force the ToolTip text to be displayed whether or not the form is active.
                    toolTip.ShowAlways = true;

                    // Set up the ToolTip text for the Button
                    toolTip.SetToolTip(ProductButton, productsContainerDTOList[i].Description);
                    Size s = new Size((int)(ProductButton.Width * cardButtonSize), (int)(ProductButton.Height * cardButtonSize));
                    if (productsContainerDTOList[i].ButtonColor.ToString().Trim() != "")
                    {
                        if (btnImageDictionary.ContainsKey(productsContainerDTOList[i].ButtonColor.ToString().Trim()))
                            ProductButton.BackgroundImage = btnImageDictionary[productsContainerDTOList[i].ButtonColor.ToString().Trim()];
                        else
                        {
                            setProductButtonColor(ProductButton, productsContainerDTOList[i].ButtonColor.ToString());
                            btnImageDictionary.Add(productsContainerDTOList[i].ButtonColor.ToString(), ProductButton.BackgroundImage);
                        }
                        //setProductButtonColor(ProductButton, ProductTbl.Rows[i]["ButtonColor"].ToString());
                    }

                    if (productsContainerDTOList[i].TextColor.ToString().Trim() != "")
                    {
                        setProductButtonTextColor(ProductButton, productsContainerDTOList[i].TextColor.ToString());
                    }
                    else if (productsContainerDTOList[i].ButtonColor.ToString().Trim() != "")
                    {
                        Color bColor = POSUtils.getColor(productsContainerDTOList[i].ButtonColor.ToString().Trim());
                        if (bColor.ToArgb() != 0)
                            ProductButton.ForeColor = Color.FromArgb(255 - bColor.R, 255 - bColor.G, 255 - bColor.B);
                    }

                    ProductButton.BackgroundImage.Tag = ProductButton.BackgroundImage;

                    if (productsContainerDTOList[i].Font.ToString() != "")
                    {
                        setProductButtonFont(ProductButton, productsContainerDTOList[i].Font.ToString());
                    }
                    ProductButton.Size = s;
                    flowLayoutPanel1.Controls.Add(ProductButton);
                    if (i == 0 && productsContainerDTOList.Count == 1)
                    {
                        MyToggleButton_Click(ProductButton, new EventArgs());
                    }
                }
            }
            else
            {
                if (displayGroupIdList.Any() == false)
                {
                    flowLayoutPanel1.Enabled = false;
                    flowLayoutPanel1.Visible = false;
                }
                else
                {
                    displayGroupExists = true;
                    Label noItemsLabel = new Label();
                    noItemsLabel.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4498, MessageContainerList.GetMessage(Utilities.ExecutionContext, "Lost Card"));
                    noItemsLabel.AutoSize = true;
                    noItemsLabel.Dock = DockStyle.Fill;
                    noItemsLabel.TextAlign = ContentAlignment.MiddleCenter;
                    // Add the Label to the FlowLayoutPanel
                    flowLayoutPanel1.Controls.Add(noItemsLabel);
                }
            }
            if (rb10HFormat.Checked)
            {
                isDamaged = true;
            }
            if (isDamaged)
            {
                flowLayoutPanel1.Enabled = false;
                flowLayoutPanel1.Visible = false;
            }
            log.LogMethodExit();
        }

        public static void CreateTrxUsrLogEntryForTransferType(string transferCardType, Transaction trx, string loginId, ExecutionContext executionContext, SqlTransaction sqlTrx = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(transferCardType) && transferCardType == "D" || transferCardType == "L")
                {
                    if (trx.TrxLines.Count > 1)
                    {
                        for (int i = 0; i < trx.TrxLines.Count; i++)
                        {
                            for (int j = 0; j < trx.TrxLines.Count; j++)
                            {
                                if (trx.TrxLines[j].ParentLine != null && trx.TrxLines[i].DBLineId == trx.TrxLines[j].ParentLine.DBLineId && trx.TrxLines[i].ProductID == trx.TrxLines[j].ProductID
                                    && trx.TrxLines[i].ProductTypeCode == "CARDSALE" && trx.TrxLines[j].CardNumber != trx.TrxLines[i].CardNumber)
                                {
                                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, trx.TrxLines[i].ProductID);
                                    if (productsContainerDTO.IsTransferCard)
                                    {
                                        List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>>();
                                        searchParameters.Add(new KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>(TrxUserLogsDTO.SearchByParameters.TRX_ID, trx.Trx_id.ToString()));
                                        TrxUserLogsList trxUserLogsList = new TrxUserLogsList(executionContext);
                                        List<TrxUserLogsDTO> trxUserLogsDTOList = trxUserLogsList.GetAllTrxUserLogs(searchParameters, sqlTrx);
                                        if (trxUserLogsDTOList != null && trxUserLogsDTOList.Any(x => x.TrxId == trx.Trx_id && x.LineId == trx.TrxLines[i].DBLineId && x.Action == "TRANSFERCARDTYPE") == false)
                                        {
                                            trx.InsertTrxLogs(trx.Trx_id, trx.TrxLines[i].DBLineId, loginId, "TRANSFERCARDTYPE", transferCardType == "D" ? "Damaged" : "Lost", sqlTrx);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        bool setProductButtonColor(Button button, string bgcolor)
        {
            log.Debug("Starts-setProductButtonColor(button," + bgcolor + ")");//Added for logger function on 29-Feb-2016
            Color BGcolor = POSUtils.getColor(bgcolor);
            if (BGcolor.ToArgb() == 0)
            {
                log.Info("Ends-setProductButtonColor(button," + bgcolor + ") as BGcolor.ToArgb() == 0");//Added for logger function on 29-Feb-2016
                return false;
            }

            button.BackgroundImage = POSUtils.ChangeColor((Bitmap)button.BackgroundImage, BGcolor);
            log.Debug("Ends-setProductButtonColor(button," + bgcolor + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        bool setProductButtonTextColor(Button button, string fcolor)
        {
            log.Debug("Starts-setProductButtonTextColor(button," + fcolor + ")");//Added for logger function on 29-Feb-2016
            Color foreColor = POSUtils.getColor(fcolor);
            if (foreColor.ToArgb() == 0)
            {
                log.Info("Ends-setProductButtonTextColor(button," + fcolor + ") as foreColor.ToArgb() == 0");//Added for logger function on 29-Feb-2016
                return false;
            }

            button.ForeColor = foreColor;
            log.Debug("Ends-setProductButtonTextColor(button," + fcolor + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        bool setProductButtonFont(Button button, string font)
        {
            log.Debug("Starts-setProductButtonFont(button," + font + ")");//Added for logger function on 29-Feb-2016
            try
            {
                if (string.IsNullOrEmpty(font))
                    button.Font = new Font("Tahoma", 9);
                else
                {
                    button.Font = CustomFontConverter.ConvertStringToFont(Utilities.ExecutionContext, font);
                }
            }
            //catch { }            
            catch (Exception exp)
            {
                log.Fatal("Ends-setProductButtonFont(button," + font + ") due to exception " + exp.Message);//Added for logger function on 29-Feb-2016
            }

            log.Debug("Ends-setProductButtonFont(button," + font + ")");//Added for logger function on 29-Feb-2016
            return true;
        }

        private void MyToggleButton_Click(object sender, EventArgs e)
        {
            // Set all Buttons in the Panel to their 'default' appearance.
            var panelButtons = flowLayoutPanel1.Controls.OfType<Button>();
            var clickedButton = (Button)sender;
            foreach (Button button in flowLayoutPanel1.Controls.OfType<Button>())
            {
                button.BackgroundImage = sampleButtonCardSaleProduct.BackgroundImage;
                //button.BackColor = sampleButtonCardSaleProduct.BackColor;
                if (button.Name == clickedButton.Name)
                {
                    button.BackgroundImage = global::Parafait_POS.Properties.Resources.Attraction;
                    //button.BackColor = Color.Red;
                    selectedTransferCardProductId = Convert.ToInt32(button.Tag);
                }
                // Other changes...
            }
            // Now set the appearance of the Button that was clicked.
            // Other changes...
        }

        private void IsLostCardButton_click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            flowLayoutPanel1.Enabled = true;
            flowLayoutPanel1.Visible = true;
            isDamaged = false;
            if(displayGroupExists == false)
            {
                flowLayoutPanel1.Enabled = false;
                flowLayoutPanel1.Visible = false;
            }
            log.LogMethodExit();
        }

        private void IsDamagedCardButton_click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            flowLayoutPanel1.Enabled = false;
            flowLayoutPanel1.Visible = false;
            isDamaged = true;
            log.LogMethodExit();
        }

        private GenericOTPDTO PerformgenericOTPValidation(TaskTypesContainerDTO taskTypesContainerDTO, int cardId)
        {
            log.LogMethodEntry(taskTypesContainerDTO);
            GenericOTPDTO paymentModeOTPValue = null;
            if (taskTypesContainerDTO != null && taskTypesContainerDTO.EnableOTPValidation)
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                KeyValuePair<GenericOTPDTO, ApprovalAction> keyValuePair = frmVerifyTaskOTP.PerformGenericOTPValidation(Utilities, taskTypesContainerDTO, cardId);
                paymentModeOTPValue = keyValuePair.Key;
                if (keyValuePair.Value != null)
                {
                    string keyValue = keyValuePair.Key.Id + "-" + ServerDateTime.Now.ToString("yyyyMMddHHmmss");
                    transferCardOTPApprovals.Add(keyValue, keyValuePair.Value);
                }
            }
            log.LogMethodExit(paymentModeOTPValue);
            return paymentModeOTPValue;
        }

        private void TransferCard()
        {
            log.Debug("Starts-TransferCard()");
            Card tappedCard;
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                tappedCard = new MifareCard(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            else
                tappedCard = new Card(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);

            string message = "";
            if (!POSUtils.refreshCardFromHQ(ref tappedCard, ref message))
            {
                displayMessageLine(message);
                log.Debug("Ends-TransferCard() as unable to refresh card from HQ");
                return;
            }

            if (tappedCard.siteId > 0 && tappedCard.siteId != POSStatic.ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Info("Ends-TransferCard() as Roaming cards not allowed");
                return;
            }

            if (FromCardDGV.Rows.Count == 0 && tappedCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(459));
                log.Info("Ends-TransferCard() as New Card was Tapped");
                return;
            }

            if (FromCardDGV.Rows.Count > 0 && tappedCard.CardStatus != "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(58));
                log.Info("Ends-TransferCard() as New Card Should be Tapped to Transfer");
                return;
            }

            if (tappedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(MessageUtils.getMessage(1161, CardNumber));
                log.Info("Ends-TransferCard() as a Technician Card (" + CardNumber + ") not allowed for Transfer");
                return;
            }

            if (tappedCard.CardStatus != "NEW")
            {
                CurrentCard = tappedCard;
                textBoxTransferCardNumber.Text = CurrentCard.CardNumber;
                TaskProcs.getCardDetails(CurrentCard, ref FromCardDGV);
                Utilities.setLanguage(FromCardDGV);
                log.Info("TransferCard() - Tapped card not a New Card");
            }
            else
            {
                TransferToCard = tappedCard;
                textBox1.Text = TransferToCard.CardNumber;
                log.Info("TransferCard() - Tapped a New Card");
            }
            /* End: Tapping of card - Till here on July 9, 2015*/
            this.Refresh();

            displayMessageLine("");
            log.Debug("Ends-TransferCard()");
        }
        private void LoadHoldEntitlements()
        {
            log.Debug("Starts-LoadHoldEntitlements()");
            Card tappedCard;
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                tappedCard = new MifareCard(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            else
                tappedCard = new Card(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            bool flag = false;
            bool isCardOnHold = false;
            string message = "";
            if (!POSUtils.refreshCardFromHQ(ref tappedCard, ref message))
            {
                displayMessageLine(message);
                log.Debug("Ends-LoadHoldEntitlements() as unable to refresh card from HQ");
                return;
            }

            if (tappedCard.siteId > 0 && tappedCard.siteId != POSStatic.ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Info("Ends-LoadHoldEntitlements() as Roaming cards not allowed");
                return;
            }

            if (HoldEntDGV.Rows.Count == 0 && tappedCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(172));
                log.Info("Ends-LoadHoldEntitlements() as New Card was Tapped");
                return;
            }

            if (tappedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(MessageUtils.getMessage(1161, CardNumber));
                log.Info("Ends-LoadHoldEntitlements() as a Technician Card (" + CardNumber + ") not allowed for Transfer");
                return;
            }

            if (tappedCard.CardStatus != "NEW")
            {
                CurrentCard = tappedCard;
                textBoxHoldEntitlementNumber.Text = CurrentCard.CardNumber;
                TaskProcs.getCardDetails(CurrentCard, ref HoldEntDGV);
                Utilities.setLanguage(HoldEntDGV);
                flag = true;
                log.Info("LoadHoldEntitlements() - Tapped card not a New Card");
            }
            AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, tappedCard.card_id, true, true);
            AccountDTO accountDTO = accountBL.AccountDTO;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
            if (flag == true)
            {
                if ((accountDTO.AccountCreditPlusDTOList == null || accountDTO.AccountCreditPlusDTOList.Count == 0) && (accountDTO.AccountGameDTOList == null || accountDTO.AccountGameDTOList.Count == 0) && (accountDTO.AccountDiscountDTOList == null || accountDTO.AccountDiscountDTOList.Count == 0))
                {
                    displayMessageLine(MessageUtils.getMessage(12557));
                    log.Debug("Ends-LoadHoldEntitlements() as card has no entitlements");
                    return;
                }
                if (accountDTO != null
                && accountDTO.AccountCreditPlusDTOList != null
                && accountDTO.AccountCreditPlusDTOList.Count > 0
                && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountCreditPlusDTO> accountCreditPlusHoldList = accountDTO.AccountCreditPlusDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                          && (x.SubscriptionBillingScheduleId == -1
                                                                                                                             || (x.SubscriptionBillingScheduleId != -1
                                                                                                                                    && subscriptionBillingScheduleDTOList != null
                                                                                                                                    && subscriptionBillingScheduleDTOList.Any()
                                                                                                                                    && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                                   && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountCreditPlusHoldList != null && accountCreditPlusHoldList.Any())
                    {
                        isCardOnHold = true;
                    }
                }
                if (accountDTO != null
                && accountDTO.AccountGameDTOList != null
                && accountDTO.AccountGameDTOList.Count > 0
                && accountDTO.AccountGameDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountGameDTO> accountGameHoldList = accountDTO.AccountGameDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                        && (x.SubscriptionBillingScheduleId == -1
                                                                                                            || (x.SubscriptionBillingScheduleId != -1
                                                                                                                && subscriptionBillingScheduleDTOList != null
                                                                                                                && subscriptionBillingScheduleDTOList.Any()
                                                                                                                && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountGameHoldList != null && accountGameHoldList.Any())
                    {
                        if (!isCardOnHold)
                            isCardOnHold = true;
                    }
                }
                if (accountDTO != null
                && accountDTO.AccountDiscountDTOList != null
                && accountDTO.AccountDiscountDTOList.Count > 0
                && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountDiscountDTO> accountDiscountHoldList = accountDTO.AccountDiscountDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                    && (x.SubscriptionBillingScheduleId == -1
                                                                                                                        || (x.SubscriptionBillingScheduleId != -1
                                                                                                                            && subscriptionBillingScheduleDTOList != null
                                                                                                                            && subscriptionBillingScheduleDTOList.Any()
                                                                                                                            && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                             && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountDiscountHoldList != null && accountDiscountHoldList.Any())
                    {
                        if (!isCardOnHold)
                            isCardOnHold = true;
                    }

                }
                if (isCardOnHold)
                {
                    buttonOK.Text = "UNHOLD";
                }
                else
                {
                    buttonOK.Text = "HOLD";
                }
            }

            displayMessageLine("");
            log.Debug("Ends-LoadHoldEntitlements()");
        }
        private void LoadTickets()
        {
            log.Debug("Starts-LoadTickets()");
            lblScannedTickets.Text = "";
            tabPageLoadTickets.Text = tabPageLoadTickets.Text.Replace("Tickets", POSStatic.TicketTermVariant);
            lblEnterTicketsToLoad.Text = lblEnterTicketsToLoad.Text.Replace("Tickets", POSStatic.TicketTermVariant);
            LoadTicketDGV = new DataGridView();
            TaskProcs.getCardDetails(CurrentCard, ref LoadTicketDGV);
            Utilities.setLanguage(LoadTicketDGV);
            LoadTicketDGV.Location = new Point(20, 10);
            LoadTicketDGV.BackgroundColor = this.BackColor;
            tabPageLoadTickets.Controls.Add(LoadTicketDGV);
            redemption = new clsRedemption(Utilities, null);
            redemption.ticketSourceInfoObj = new List<clsRedemption.TicketSourceInfo>();
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            log.Debug("Ends-LoadTickets()");
        }
        private void ExchangeCreditsForTime()
        {
            dcAdditionalTime.DefaultCellStyle =
                        dcAdditionalPoints.DefaultCellStyle =
                        dcBalancePoints.DefaultCellStyle = Utilities.gridViewNumericCellStyle();

            if (dgvTappedCard.Rows.Count == 0) // parent card
            {
                dgvTappedCard.Rows.Add(CurrentCard.CardNumber,
                                       CurrentCard.customerDTO == null ? "" : CurrentCard.customerDTO.FirstName + " " + CurrentCard.customerDTO.LastName,
                                       CurrentCard.time + CurrentCard.CreditPlusTime,
                                       CurrentCard.credits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits,
                                       CurrentCard.card_id);

                dgvPointsToConvert.Rows.Add("",
                                       CurrentCard.time + CurrentCard.CreditPlusTime,
                                       CurrentCard.credits + CurrentCard.CreditPlusCardBalance + CurrentCard.CreditPlusCredits);
            }
            else
            {
                if (dgvTappedCard[0, 0].Value.ToString().Equals(CardNumber))
                {
                    displayMessageLine("Parent Card");
                    return;
                }

            }
            dgvPointsToConvert["dcAdditionalPoints", dgvPointsToConvert.Rows.Count - 1].Style.BackColor = Color.PaleTurquoise;
            displayMessageLine(Utilities.MessageUtils.getMessage(1382));
        }
        private void ExchangeTimeForCredits()
        {
            if (CurrentCard.time + CurrentCard.CreditPlusTime <= 0)
            {
                displayMessageLine(Utilities.MessageUtils.getMessage(1466));//Message= "No active time available"
                return;
            }
            dcNewTimeToConvert.DefaultCellStyle =
                        dcNewPoints.DefaultCellStyle =
                        dcNewTimeBalance.DefaultCellStyle = Utilities.gridViewNumericCellStyle();

            if (dgvCardInfo.Rows.Count == 0) // parent card
            {
                dgvCardInfo.Rows.Add(CurrentCard.CardNumber,
                                       CurrentCard.customerDTO == null ? "" : CurrentCard.customerDTO.FirstName + " " + CurrentCard.customerDTO.LastName,
                                       CurrentCard.credits,
                                       CurrentCard.time + CurrentCard.CreditPlusTime,
                                       CurrentCard.card_id);

                dgvTimeToConvert.Rows.Add("",
                                       CurrentCard.credits,
                                       CurrentCard.time + CurrentCard.CreditPlusTime);

            }
            else
            {
                if (dgvCardInfo[0, 0].Value.ToString().Equals(CardNumber))
                {
                    displayMessageLine("Parent Card");
                    return;
                }

            }
            dgvTimeToConvert["dcNewTimeToConvert", dgvTimeToConvert.Rows.Count - 1].Style.BackColor = Color.PaleTurquoise;
            displayMessageLine(Utilities.MessageUtils.getMessage(1442));
        }
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-BarCodeScanCompleteEventHandle()");
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Utilities.ProcessScannedBarCode(checkScannedEvent.Message, ParafaitEnv.LEFT_TRIM_BARCODE, ParafaitEnv.RIGHT_TRIM_BARCODE);
                Invoke((MethodInvoker)delegate
                {
                    HandleBarcodeRead(scannedBarcode);
                });
            }
            log.Debug("Ends-BarCodeScanCompleteEventHandle()");
        }

        //For Barcode
        // List<string> ManualTicketsArray = new List<string>();
        //List<Tuple<string, int, DateTime?>> manualTicketsArray = new List<Tuple<string, int, DateTime?>>();
        clsRedemption redemption;
        private void HandleBarcodeRead(string scannedBarcode)
        {
            log.Debug("Starts-HandleBarcodeRead(" + scannedBarcode + ")");
            if (scannedBarcode != "")
            {
                string localBarcode = scannedBarcode;
                try
                {
                    // if (manualTicketsArray.Contains(localBarcode))
                    //if (manualTicketsArray.Exists(t => t.Item1== localBarcode))
                    if (redemption.ticketSourceInfoObj != null && redemption.ticketSourceInfoObj.Exists(t => t.receiptNo == localBarcode))
                    {
                        displayMessageLine(MessageUtils.getMessage(113) + ": " + MessageUtils.getMessage(112));
                        log.Info("Ends-HandleBarcodeRead(" + scannedBarcode + ") as Duplicate Receipt/Receipt already used");
                        Application.DoEvents();
                        return;
                    }

                    ////SqlCommand cmd = Utilities.getCommand();
                    ////cmd.CommandText = "select top 1 1 from ManualTicketReceipts where ManualTicketReceiptNo = @receiptNo and BalanceTickets = 0";
                    ////cmd.Parameters.AddWithValue("@receiptNo", localBarcode);
                    //if (cmd.ExecuteScalar() != null)
                    TicketReceipt ticketReceipt = new TicketReceipt(Utilities.ExecutionContext, localBarcode);
                    if (ticketReceipt.IsUsedTicketReceipt(null))
                    {
                        displayMessageLine(MessageUtils.getMessage(113) + ": " + MessageUtils.getMessage(112));
                        log.Info("Ends-HandleBarcodeRead(" + scannedBarcode + ") as Duplicate Receipt/Receipt already used");
                        Application.DoEvents();
                        return;
                    }
                    if (ticketReceipt.IsFlaggedTicketReceipt())
                    {
                        ApplicationRemarksList applicationRemarksList = new ApplicationRemarksList(Utilities.ExecutionContext);
                        List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>> applicationRemarksSearchParams = new List<KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>>();
                        applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.ACTIVE_FLAG, "1"));
                        applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SITE_ID, ((Utilities.ParafaitEnv.IsCorporate && Utilities.ParafaitEnv.IsMasterSite) ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                        applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_NAME, "ManualTicketReceipts"));
                        applicationRemarksSearchParams.Add(new KeyValuePair<ApplicationRemarksDTO.SearchByApplicationRemarksParameters, string>(ApplicationRemarksDTO.SearchByApplicationRemarksParameters.SOURCE_GUID, ticketReceipt.TicketReceiptDTO.Guid));
                        List<ApplicationRemarksDTO> applicationRemarksListOnDisplay = applicationRemarksList.GetAllApplicationRemarks(applicationRemarksSearchParams);
                        if (MessageBox.Show(MessageUtils.getMessage(1395, ": " + ((applicationRemarksListOnDisplay != null && applicationRemarksListOnDisplay.Count > 0) ? applicationRemarksListOnDisplay[0].Remarks : " " + MessageUtils.getMessage("unknown") + ".") + "\n"), "Ticket receipt flagged.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            TicketReceiptUI ticketReceiptUI = new TicketReceiptUI(Utilities, localBarcode);
                            ticketReceiptUI.Show();
                        }
                        log.Info("Ends-HandleBarcodeRead(" + localBarcode + ") as ticket receipt is flagged for the reason :" + ((applicationRemarksListOnDisplay != null && applicationRemarksListOnDisplay.Count > 0) ? applicationRemarksListOnDisplay[0].Remarks : " unknown."));
                        return;
                    }

                    TicketStationBL ticketStationBL = null;
                    bool valid = false;
                    string stationId = string.Empty;
                    try
                    {
                        TicketStationFactory ticketStationFactory = new TicketStationFactory();
                        ticketStationBL = ticketStationFactory.GetTicketStationObject(localBarcode);
                        if (ticketStationBL == null)
                        {
                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(2321, "Ticket Station"));
                        }
                        else
                        {
                            if (ticketStationBL.BelongsToThisStation(localBarcode) && ticketStationBL.ValidCheckBit(localBarcode))
                            {
                                valid = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        valid = false;
                    }
                    if (!valid)
                    {
                        string message = MessageUtils.getMessage(115, POSStatic.TicketTermVariant);
                        displayMessageLine(message);
                        log.Info("Ends-HandleBarcodeRead(" + scannedBarcode + ") is invalid");
                        return;
                    }

                    int scanTicks;
                    try
                    {
                        scanTicks = ticketStationBL.GetTicketValue(localBarcode);
                    }
                    catch
                    {
                        displayMessageLine(MessageUtils.getMessage(115, POSStatic.TicketTermVariant));
                        log.Fatal("Ends-addScanTickets(" + localBarcode + "," + localBarcode + ") due to exception  Scanned Ticket Receipt is invalid");
                        return;
                    }

                    // ManualTicketsArray.Add(localBarcode);
                    //manualTicketsArray.Add(new Tuple<string, int, DateTime?>(localBarcode, scanTicks, null));
                    clsRedemption.TicketSourceInfo ticketSourceobj = new clsRedemption.TicketSourceInfo();
                    ticketSourceobj.ticketSource = "Receipt";
                    ticketSourceobj.ticketValue = scanTicks;
                    ticketSourceobj.receiptNo = localBarcode;
                    ticketSourceobj.sourceCurrencyRuleId = -1;
                    redemption.ticketSourceInfoObj.Add(ticketSourceobj);

                    if (lblScannedTickets.Text == "")
                        lblScannedTickets.Text = scanTicks.ToString();
                    else
                        lblScannedTickets.Text += " + " + scanTicks.ToString();
                    int totalTickets = 0;
                    if (textBoxLoadTickets.Enabled == false)
                    {
                        totalTickets = Convert.ToInt32(textBoxLoadTickets.Text);
                    }
                    else
                        textBoxLoadTickets.Enabled = false;

                    totalTickets += scanTicks;
                    textBoxLoadTickets.Text = totalTickets.ToString();

                    displayMessageLine(MessageUtils.getMessage(114, scanTicks.ToString(), POSStatic.TicketTermVariant));
                }
                catch
                {
                    displayMessageLine(MessageUtils.getMessage(115, POSStatic.TicketTermVariant));
                    log.Debug("Ends-HandleBarcodeRead(" + scannedBarcode + ") due to exception  Scanned Ticket Receipt is invalid ");
                }
            }
            log.Debug("Ends-HandleBarcodeRead(" + scannedBarcode + ")");
        }

        //Begin Modification: Added a new method to refund balance time for creidits/card Balance / Bonus on July 9, 2015//
        private void RefundTime()
        {
            log.Debug("Starts-RefundTime()");
            previousTransactionId = "";
            previousLineId = "";
            timeRefundAmount = 0;
            DataTable dtRefundTime = Utilities.executeDataTable(@"SELECT a.TrxId,
                                                                   a.LineId,
                                                                   a.CreditPlus[Loaded Time],
                                                                   a.CreditPlusBalanceTime [Balance Time],
                                                                   a.Card_id
                                                                FROM
                                                                    (SELECT cp.TrxId,
                                                                      cp.LineId,
                                                                      cp.CreditPlus,
                                                                      isnull(CASE CreditPlusType WHEN 'M' 
				                                                            THEN (CASE WHEN PlayStartTime IS NULL 
					                                                               THEN CreditPlusBalance 
						                                                            ELSE 
							                                                            CASE WHEN datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime)) < 0 
							                                                            THEN 0 ELSE datediff(MI, getdate(), dateadd(MI, CreditPlusBalance, PlayStartTime)) END END) 
				                                                            ELSE 0 END, 0) CreditPlusBalanceTime,
                                                                      Card_id
                                                                    FROM CardCreditPlus cp
                                                                    WHERE (PeriodFrom IS NULL
                                                                          OR PeriodFrom <= GETDATE())
                                                                     AND (PeriodTo IS NULL
                                                                          OR PeriodTo >= GETDATE())
                                                                     AND CreditPlusBalance >0
                                                                     AND Card_id = @cardId
                                                                     AND CreditPlusType ='M'
                                                                     AND ISNULL(CP.ValidityStatus,'Y') != 'H'
	                                                             UNION ALL 
	                                                             SELECT			NULL,
                                                                                NULL,
                                                                                cv.time,
                                                                                cv.BalanceTime,
                                                                                cv.card_id
                                                               FROM cardview cv
                                                               WHERE card_id = @cardId AND cv.BalanceTime > 0)a
                                                              JOIN cards c ON c.card_id = a.card_id",
                                      new SqlParameter("@cardId", CurrentCard.card_id));
            dgvRefundTime.DataSource = dtRefundTime;
            Utilities.setupDataGridProperties(ref dgvRefundTime);
            dgvRefundTime.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRefundTime.BackgroundColor = this.BackColor;
            dgvRefundTime.Columns[0].SortMode =
                dgvRefundTime.Columns[1].SortMode =
                dgvRefundTime.Columns[2].SortMode =
                dgvRefundTime.Columns[3].SortMode =
                dgvRefundTime.Columns[4].SortMode =
                dgvRefundTime.Columns[5].SortMode =
                DataGridViewColumnSortMode.NotSortable;

            dgvRefundTime.Columns[3].DefaultCellStyle = dgvRefundTime.Columns[4].DefaultCellStyle = Utilities.gridViewAmountCellStyle(); ;

            dgvRefundTime.ColumnHeadersDefaultCellStyle = RefundCardDGV.ColumnHeadersDefaultCellStyle;
            dgvRefundTime.Columns[2].Visible = dgvRefundTime.Columns[5].Visible = false;
            Utilities.setLanguage(dgvRefundTime);

            if (dtRefundTime.Rows.Count > 0)
            {
                dgvRefundTime.Rows[0].Selected = false;
            }
            log.Debug("Ends-RefundTime()");
        }
        //End: Added a new method to refund balance time for creidits/card Balance / Bonus on July 9, 2015//

        private void RefundCard()
        {
            log.Debug("Starts-RefundCard()");
            //CenterToParent();
            //this.Width = 1180;
            List<Card> newCardList = new List<Card>();//Added to load the cards to a list for refunding multiple cards
            GetRefundGrid();
            RefundGameBalance();
            if (CurrentCard != null)
            {
                newCardList.Add(CurrentCard);
                GetRefundAmountGrid(newCardList);
            }
            else
            {
                for (int i = 0; i < LoadMultipleCards.Count; i++)
                {
                    newCardList.Add(LoadMultipleCards[i]);
                }

                GetRefundAmountGrid(newCardList);
                grpRefundTime.Visible = false;
                this.Width = 790;
                CenterToScreen();
                buttonOK.Location = new Point(200, 441);
                buttonCancel.Location = new Point(450, 441);
                txtRemarks.Width = 600;
            }
            Utilities.setLanguage(dgvRefundCardData);
            PoleDisplay.writeSecondLine("Refund: " + lblRefundAmount.Text);
            displayMessageLine("Pay Customer " + lblRefundAmount.Text);
            if (CurrentCard != null)
                //Begin: Added to load Time Grid on July 29, 2015
                RefundTime();
            //End: Added to load Time 

            log.Info("RefundCard() - Total Refund Amount:" + lblRefundAmount.Text);

            log.Debug("Ends-RefundCard()");
        }
        private void GetRefundGrid()
        {
            if (RefundCardDGV == null)
            {
                RefundCardDGV = new DataGridView();
                TaskProcs.getCardDetails(CurrentCard, ref RefundCardDGV);
                Utilities.setLanguage(RefundCardDGV);
                RefundCardDGV.Location = new Point(20, 10);
                RefundCardDGV.BackgroundColor = this.BackColor;
                tabPageRefundCard.Controls.Add(RefundCardDGV);
                chkMakeCardNewOnFullRefund.Checked = POSStatic.MAKE_CARD_NEW_ON_FULL_REFUND;
            }
            //Begin: Added to hide the Time group box and reduce the form width if time component is 0 on July 30, 2015//
            if (CurrentCard != null)
            {
                if (CurrentCard.time.CompareTo(0.0) == 0 && CurrentCard.CreditPlusTime.CompareTo(0.0) == 0)
                {
                    grpRefundTime.Visible = false;
                    this.Width = 790;
                    CenterToScreen();
                    buttonOK.Location = new Point(200, 441);
                    buttonCancel.Location = new Point(450, 441);
                    txtRemarks.Width = 600;
                }
                else
                {
                    this.AutoScroll = true;
                    this.Width = 990;
                    this.Height = 580;
                    CenterToScreen();
                    buttonOK.Location = new Point(200, 441);
                    buttonCancel.Location = new Point(450, 441);
                }
            }

            else    //Added to display the empty Card information grid while refunding multiple cards
            {
                if (RefundCardDGV != null && RefundCardDGV.Columns.Count <= 0)
                {
                    RefundCardDGV.Columns.Add("Card_Number", "Card Number");
                    RefundCardDGV.Columns.Add("Issue_Date", "Issue Date");
                    RefundCardDGV.Columns.Add("Credits", "Credits");
                    RefundCardDGV.Columns.Add("Courtesy", "Courtesy");
                    RefundCardDGV.Columns.Add("Bonus", "Bonus");
                    RefundCardDGV.Columns.Add("Time", "Time");
                    RefundCardDGV.Columns.Add("Tickets", Utilities.ParafaitEnv.REDEMPTION_TICKET_NAME_VARIANT);
                    RefundCardDGV.Columns.Add("Used_Credits", "Spent");


                    for (int i = 0; i < RefundCardDGV.Columns.Count; i++)
                    {
                        RefundCardDGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        RefundCardDGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    RefundCardDGV.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                    RefundCardDGV.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    RefundCardDGV.DefaultCellStyle.BackColor = Color.White;
                    RefundCardDGV.DefaultCellStyle.ForeColor = Color.Black;
                    RefundCardDGV.DefaultCellStyle.SelectionBackColor = Color.White;
                    RefundCardDGV.DefaultCellStyle.SelectionForeColor = Color.Black;

                    RefundCardDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(Utilities.getFont(), System.Drawing.FontStyle.Bold);
                    RefundCardDGV.DefaultCellStyle.Font = Utilities.getFont();
                    RefundCardDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
            //End : Added to hide the Time group box and reduce the form width if time component is 0 on July 30, 2015//
        }
        private void RefundGameBalance()
        {
            txtRefundGameAmount.Enabled = false;
            txtRefundTime.Enabled = false;
            cmbLoadGameAttributes.Enabled = false;
            cmbLoadTimeAttributes.Enabled = false;
            if (CurrentCard != null)
            {
                DataTable dt = Utilities.executeDataTable(@"select cg.TrxId,cg.TrxLineId, isnull(gp.profile_name, case when g.game_name is null then ' - All -' else null end) Profile, 
	                                                    isnull(g.game_name, case when gp.profile_name is null then ' - All -' else null end) Game,
                                                        sum(cg.Quantity) [Games Loaded],
                                                        sum(cg.BalanceGames) [Balance Games]
                                                        from CardGames cg 
                                                            left outer join game_profile gp on gp.game_profile_id = cg.game_profile_id
                                                            left outer join games g on g.game_id = cg.game_id
                                                        where cg.card_id = @cardId
                                                        and isnull(cg.Frequency, 'N') = 'N'
                                                        and isnull(cg.validityStatus, 'Y') != 'H'
                                                        and quantity > 0
                                                        and isnull(ExpiryDate, getdate()) >= getdate()
                                                        group by cg.TrxId,cg.TrxLineId, gp.profile_name, g.game_name
                                                        having sum(cg.BalanceGames) > 0",
                                                          new SqlParameter("@cardId", CurrentCard.card_id));
                dgvRefundBalanceGames.DataSource = dt;
                Utilities.setupDataGridProperties(ref dgvRefundBalanceGames);
                dgvRefundBalanceGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvRefundBalanceGames.BackgroundColor = this.BackColor;
                dgvRefundBalanceGames.Columns[0].SortMode =
                    dgvRefundBalanceGames.Columns[1].SortMode =
                    dgvRefundBalanceGames.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

                dgvRefundBalanceGames.Columns[2].DefaultCellStyle = Utilities.gridViewNumericCellStyle();

                dgvRefundBalanceGames.ColumnHeadersDefaultCellStyle = RefundCardDGV.ColumnHeadersDefaultCellStyle;

                Utilities.setLanguage(dgvRefundBalanceGames);

                if (dt.Rows.Count > 0)
                {
                    dgvRefundBalanceGames.Rows[0].Selected = false;
                }
            }
            else   //Added to display the empty Refund game grid while refunding multiple cards
            {
                if (dgvRefundBalanceGames != null && dgvRefundBalanceGames.Columns.Count <= 1)
                {
                    dgvRefundBalanceGames.Columns.Add("TrxId", "Trx Id");
                    dgvRefundBalanceGames.Columns.Add("TrxLineId", "TrxLineId");
                    dgvRefundBalanceGames.Columns.Add("Profile", "Profile");
                    dgvRefundBalanceGames.Columns.Add("Game", "Game");
                    dgvRefundBalanceGames.Columns.Add("Games Loaded", "Games Loaded");
                    dgvRefundBalanceGames.Columns.Add("Balance Games", "Balance Games");

                    Utilities.setupDataGridProperties(ref dgvRefundBalanceGames);
                    for (int i = 0; i < dgvRefundBalanceGames.Columns.Count; i++)
                    {
                        dgvRefundBalanceGames.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        dgvRefundBalanceGames.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }

                    dgvRefundBalanceGames.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
                    dgvRefundBalanceGames.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

                    dgvRefundBalanceGames.DefaultCellStyle.BackColor = Color.White;
                    dgvRefundBalanceGames.DefaultCellStyle.ForeColor = Color.Black;
                    dgvRefundBalanceGames.DefaultCellStyle.SelectionBackColor = Color.White;
                    dgvRefundBalanceGames.DefaultCellStyle.SelectionForeColor = Color.Black;

                    dgvRefundBalanceGames.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(Utilities.getFont(), System.Drawing.FontStyle.Bold);
                    dgvRefundBalanceGames.DefaultCellStyle.Font = Utilities.getFont();
                    dgvRefundBalanceGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                }
            }
        }
        private void GetRefundAmountGrid(List<Card> inCard)
        {
            double creditsRefund = 0, creditPlusAmount = 0, creditPlusRefund = 0;
            double refundCardDeposit = 0;

            CreditPlus creditPlus = new CreditPlus(Utilities);

            foreach (var item in inCard)
            {
                creditPlusAmount = creditPlus.getCreditPlusRefund(item.card_id);
                if (ParafaitEnv.ALLOW_REFUND_OF_CARD_DEPOSIT == "Y" && chkMakeCardNewOnFullRefund.Checked)
                {
                    refundCardDeposit += item.face_value;
                    log.Info("RefundCard() - Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + refundCardDeposit);
                }
                else
                {
                    refundCardDeposit = 0;
                    log.Info("RefundCard() - Not Allowed to Refund Card Deposit/Make Card New on Full Refund, CardDeposit:" + refundCardDeposit);
                }
                if (ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y")
                {
                    creditsRefund += item.credits;
                    log.Info("RefundCard() - Allowed to Refund Card Credits:" + creditsRefund);
                }

                if (ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y")
                {
                    creditPlusRefund += creditPlusAmount;
                    log.Info("RefundCard() - Allowed to Refund Credits Plus:" + creditPlusRefund);
                }
            }

            if (ParafaitEnv.MIFARE_CARD)
                grpRefundGames.Enabled = false;

            dgvRefundCardData.Columns.Clear();
            dgvRefundCardData.Columns.Add("Credit Type", "Credit Type");
            dgvRefundCardData.Columns.Add("Value", "Value");
            dgvRefundCardData.Columns.Add("Refund Amount", "Refund Amount");
            dgvRefundCardData.Columns.Add("RefundableAmount", "RefundableAmount");
            dgvRefundCardData.Columns[3].Visible = false;

            dgvRefundCardData.Rows.Add(new object[] { "Card Deposit", refundCardDeposit, refundCardDeposit, refundCardDeposit });
            dgvRefundCardData.Rows.Add(new object[] { "Credits", creditsRefund, creditsRefund, creditsRefund });
            dgvRefundCardData.Rows.Add(new object[] { "Credit Plus", creditPlusRefund, creditPlusRefund, creditPlusRefund });

            Utilities.setupDataGridProperties(ref dgvRefundCardData);
            dgvRefundCardData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRefundCardData.BackgroundColor = this.BackColor;
            dgvRefundCardData.Columns[0].SortMode =
                dgvRefundCardData.Columns[1].SortMode =
                dgvRefundCardData.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;

            dgvRefundCardData.Columns[1].DefaultCellStyle =
            dgvRefundCardData.Columns[2].DefaultCellStyle = Utilities.gridViewAmountCellStyle();

            dgvRefundCardData.Columns[0].DefaultCellStyle.Font =
                dgvRefundCardData.Columns[1].DefaultCellStyle.Font =
                dgvRefundCardData.Columns[2].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 10, FontStyle.Regular);

            dgvRefundCardData.ColumnHeadersDefaultCellStyle = RefundCardDGV.ColumnHeadersDefaultCellStyle;

            dgvRefundCardData.Rows[0].Selected = false;

            txtRefundAmount.Enabled = false; // not to fire the text changed event
            txtRefundAmount.Tag = Math.Round(refundCardDeposit + creditsRefund + creditPlusRefund, 4, MidpointRounding.AwayFromZero);
            txtRefundAmount.Text = (refundCardDeposit + creditsRefund + creditPlusRefund).ToString(ParafaitEnv.AMOUNT_FORMAT);

            calculateRefundAmounts();

            if ((ParafaitEnv.ALLOW_REFUND_OF_CARD_CREDITS == "Y" || ParafaitEnv.ALLOW_REFUND_OF_CREDITPLUS == "Y") && ParafaitEnv.ALLOW_PARTIAL_REFUND == "Y")
            {
                if (CurrentCard != null)
                {
                    txtRefundAmount.Enabled = true;
                    this.ActiveControl = txtRefundAmount;
                    txtRefundAmount.SelectAll();
                }
                else
                {
                    txtRefundAmount.Enabled = false;
                }
            }
            else
            {
                txtRefundAmount.Enabled = false;
            }
        }
        private void RealETicket()
        {
            log.Debug("Starts-RealETicket()");
            tabPageRealETicket.Text = tabPageRealETicket.Text.Replace("Ticket", POSStatic.TicketTermVariant.TrimEnd('s'));
            grpRealEticket.Text = grpRealEticket.Text.Replace("Ticket", POSStatic.TicketTermVariant.TrimEnd('s'));
            radioButtoneTicket.Text = radioButtoneTicket.Text.Replace("Ticket", POSStatic.TicketTermVariant.TrimEnd('s'));
            radioButtonRealTicket.Text = radioButtonRealTicket.Text.Replace("Ticket", POSStatic.TicketTermVariant.TrimEnd('s'));

            RealETicketDGV = new DataGridView();
            TaskProcs.getCardDetails(CurrentCard, ref RealETicketDGV);
            Utilities.setLanguage(RealETicketDGV);
            RealETicketDGV.Location = new Point(20, 10);
            RealETicketDGV.BackgroundColor = this.BackColor;
            tabPageRealETicket.Controls.Add(RealETicketDGV);
            if (CurrentCard.real_ticket_mode == 'Y')
            {
                radioButtonRealTicket.Checked = true;
            }
            else
            {
                radioButtoneTicket.Checked = true;
            }
            log.Debug("Ends-RealETicket()");
        }
        private void ExchangeTokenForCredit()
        {
            log.Debug("Starts-ExchangeTokenForCredit()");
            ExchangeDGV = new DataGridView();
            TaskProcs.getCardDetails(CurrentCard, ref ExchangeDGV);
            Utilities.setLanguage(ExchangeDGV);
            ExchangeDGV.Location = new Point(20, 10);
            ExchangeDGV.BackgroundColor = this.BackColor;
            tabPageExchangeTokenForCredit.Controls.Add(ExchangeDGV);
            log.Debug("Ends-ExchangeTokenForCredit()");
        }
        private void ExchangeCreditForToken()
        {
            log.Debug("Starts-ExchangeCreditForToken()");
            ExchangeDGV = new DataGridView();
            TaskProcs.getCardDetails(CurrentCard, ref ExchangeDGV);
            Utilities.setLanguage(ExchangeDGV);
            ExchangeDGV.Location = new Point(20, 10);
            ExchangeDGV.BackgroundColor = this.BackColor;
            tabPageExchangeCreditForToken.Controls.Add(ExchangeDGV);
            log.Debug("Ends-ExchangeCreditForToken()");
        }
        private void Consolidate()
        {
            log.Debug("Starts-Consolidate()");
            //if (ConsCardCount == MAXCONSCARDS)
            //{
            //    displayMessageLine("You can consolidate at most " + MAXCONSCARDS.ToString() + " Cards at a time");
            //    log.Info("Ends-Consolidate() as ConsCardCount == MAXCONSCARDS"); 
            //    return;
            //}

            Card swipedCard;
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                swipedCard = new MifareCard(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            else
                swipedCard = new Card(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);

            if (swipedCard.CardStatus == "NEW") // only existing / issued cards can be consolidated
            {
                displayMessageLine(MessageUtils.getMessage(472));
                log.Info("Ends-Consolidate() as no valid Card was Tapped to consolidate");
                return;
            }
            else if (swipedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(MessageUtils.getMessage(197, CardNumber));
                log.Info("Ends-Consolidate() as a Technician Card (" + CardNumber + ") not allowed for Transaction");
                return;
            }
            else if (swipedCard.siteId != -1 && swipedCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Info("Ends-Consolidate() as Roaming cards are not allowed");
                return;
            }

            bool cardFound = false;
            for (int i = 0; i < ConsCardCount; i++) // cannot have duplicate cards
            {
                if (swipedCard.CardNumber == ConsolidateCards[i].CardNumber)
                {
                    cardFound = true;
                    break;
                }
            }
            if (cardFound)
            {
                displayMessageLine(MessageUtils.getMessage(59));
                log.Info("Ends-Consolidate() as the Card is already added");
                return;
            }

            // if found valid, display card details in a new DGV
            //ConsolidateCards[ConsCardCount] = swipedCard;
            ConsolidateCards.Add(swipedCard);
            //ConsolidateDGV[ConsCardCount] = new DataGridView();
            ConsolidateDGV.Add(new DataGridView());
            ConsolidateDGV[ConsCardCount].BackgroundColor = this.BackColor;
            ConsolidateDGV[ConsCardCount].BorderStyle = BorderStyle.None;
            DataGridView cardGrid = ConsolidateDGV[ConsCardCount];
            TaskProcs.getCardDetails(ConsolidateCards[ConsCardCount], ref cardGrid);
            Utilities.setLanguage(ConsolidateDGV[ConsCardCount]);
            if (ConsCardCount == 0)
            {
                ConsolidateDGV[ConsCardCount].Location = new Point(20, 5 + (10 + ConsolidateDGV[ConsCardCount].Height) * ConsCardCount);
            }
            else
            {
                ConsolidateDGV[ConsCardCount].Location = new Point(20, (10 + ConsolidateDGV[ConsCardCount - 1].Location.Y + ConsolidateDGV[ConsCardCount].Height));
            }
            tabPageConsolidate.Controls.Add(ConsolidateDGV[ConsCardCount]);
            ConsolidateDGV[ConsCardCount].BringToFront();
            ConsolidateDGV[ConsCardCount].Refresh();
            ConsolidateDGV[ConsCardCount].Show();
            this.Refresh();
            ConsCardCount++;

            // last card entered is the final destination of consolidation
            // if (ConsCardCount == MAXCONSCARDS)
            //     displayMessageLine(MessageUtils.getMessage(60));
            // else if (ConsCardCount == MAXCONSCARDS - 1)
            //      displayMessageLine(MessageUtils.getMessage(61));
            if (ConsCardCount == 1)
            {
                displayMessageLine(MessageUtils.getMessage(2221));
            }
            else
                displayMessageLine(MessageUtils.getMessage(2222));

            log.Debug("Ends-Consolidate()");
        }
        private void BalanceTransfer()
        {
            log.Debug("Starts-BalanceTransfer()");
            Card swipedCard;
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                swipedCard = new MifareCard(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            else
                swipedCard = new Card(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);

            if (swipedCard.CardStatus == "NEW") // only existing / issued cards can be consolidated
            {
                displayMessageLine(MessageUtils.getMessage(459));
                log.Info("Ends-BalanceTransfer() as Tapped card was a New Card");
                return;
            }
            else if (swipedCard.technician_card.Equals('Y'))
            {
                displayMessageLine(MessageUtils.getMessage(197, CardNumber));
                log.Info("Ends-BalanceTransfer() as Tapped a Technician Card (" + Card_Number + ")");
                return;
            }
            else if (swipedCard.siteId != -1 && swipedCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Info("Ends-BalanceTransfer() as Tapped a Roaming cards");
                return;
            }

            bool cardFound = false;
            for (int i = 0; i < ConsCardCount; i++) // cannot have duplicate cards
            {
                if (swipedCard.CardNumber == ConsolidateCards[i].CardNumber)
                {
                    cardFound = true;
                    break;
                }
            }
            if (cardFound)
            {
                displayMessageLine(MessageUtils.getMessage(59));
                log.Info("Ends-BalanceTransfer() as Tapped cards is already added");
                return;
            }

            ConsolidateCards.Add(swipedCard);
            if (ConsCardCount == 0)
            {
                ConsolidateDGV.Add(new DataGridView());
                ConsolidateDGV[ConsCardCount].BackgroundColor = this.BackColor;
                ConsolidateDGV[ConsCardCount].BorderStyle = BorderStyle.None;
                DataGridView cardGrid = ConsolidateDGV[ConsCardCount];
                TaskProcs.getCardDetails(ConsolidateCards[ConsCardCount], ref cardGrid);
                Utilities.setLanguage(ConsolidateDGV[ConsCardCount]);
                int y = lblTransfererCardDetails.Location.Y + 15;
                ConsolidateDGV[ConsCardCount].Location = new Point(20, y);
                tabPageBalanceTransfer.Controls.Add(ConsolidateDGV[ConsCardCount]);
                ConsolidateDGV[ConsCardCount].BringToFront();
                ConsolidateDGV[ConsCardCount].Refresh();
                ConsolidateDGV[ConsCardCount].Show();
            }
            else
            {
                dgvBalanceTransferee.Rows.Add(swipedCard.CardNumber, swipedCard.credits + swipedCard.CreditPlusCardBalance + swipedCard.CreditPlusCredits + swipedCard.creditPlusItemPurchase, swipedCard.bonus + swipedCard.CreditPlusBonus,
                    swipedCard.ticket_count + swipedCard.CreditPlusTickets, 0, 0, 0, swipedCard.card_id);
            }
            this.Refresh();
            ConsCardCount++;

            if (ConsCardCount == 1)
                displayMessageLine(MessageUtils.getMessage(750));
            else
                displayMessageLine("");

            log.Debug("Ends-BalanceTransfer()");
        }
        private void LoadMultiple()
        {
            log.Debug("Starts-LoadMultiple()");
            // only NEW cards can be issued in bulk. Recharge is not allowed.
            // all cards are swiped in one go, and then products picked
            // these products will be applied to all cards in a single transaction on return

            int CardCount = dgvMultipleCards.Rows.Count;
            if (_Parameter != null)
            {
                object[] parameters = _Parameter as object[];
                if (CardCount == Convert.ToInt32(parameters[1]))
                {
                    displayMessageLine(POSStatic.MessageUtils.getMessage(32, CardCount));
                    log.Info("Ends-CardSwiped(" + CardNumber + ") as can load at most " + CardCount + " Cards");
                    return;
                }
            }
            if (CardCount == MAXLOADMULTIPLECARDS)
            {
                displayMessageLine(MessageUtils.getMessage(32, MAXLOADMULTIPLECARDS));
                log.Info("Ends-LoadMultiple() as CardCount == MAXLOADMULTIPLECARDS");
                return;
            }

            Card swipedCard = new Card(CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            if (swipedCard.technician_card.Equals('Y') == true)
            {
                displayMessageLine(MessageUtils.getMessage(197, CardNumber));
                log.Error("LoadMultiple() as Technician Card (" + Card_Number + ") not allowed for Transaction");
                return;
            }
            //Commented to allow issued cards to be loaded using Multiple load option
            //if (swipedCard.CardStatus != "NEW")
            //{
            //    displayMessageLine(MessageUtils.getMessage(63));
            //    log.Info("Ends-LoadMultiple() as tapped card was not a NEW Card to Load"); 
            //    return;
            //}
            bool cardFound = false;
            for (int i = 0; i < CardCount; i++)
            {
                if (swipedCard.CardNumber == LoadMultipleCards[i].CardNumber)
                {
                    cardFound = true;
                    break;
                }
            }
            if (cardFound)
            {
                displayMessageLine(MessageUtils.getMessage(59));
                log.Info("Ends-LoadMultiple() as tapped card is already added");
                return;
            }

            // add it to dgv for display
            LoadMultipleCards.Add(swipedCard);
            dgvMultipleCards.Rows.Add(new object[] { ++CardCount, swipedCard.CardNumber });
            dgvMultipleCards.Refresh();
            dgvMultipleCards.FirstDisplayedScrollingRowIndex = dgvMultipleCards.RowCount - 1;
            this.Refresh();

            log.Debug("Ends-LoadMultiple()");
        }
        private void LoadBonus()
        {
            log.Debug("Starts-LoadBonus()");
            int yLocation = 10;
            if (POSStatic.ALLOW_MANUAL_CARD_IN_LOAD_BONUS)
            {
                panelLoadBonusManualCard.Visible = true;
                yLocation = 50;
            }
            else
                panelLoadBonusManualCard.Visible = false;

            LoadBonusDGV = new DataGridView();
            Utilities.setLanguage(LoadBonusDGV);
            LoadBonusDGV.Location = new Point(20, yLocation);
            LoadBonusDGV.BackgroundColor = this.BackColor;
            tabPageLoadBonus.Controls.Add(LoadBonusDGV);

            if (CurrentCard != null)
                TaskProcs.getCardDetails(CurrentCard, ref LoadBonusDGV);
            else
                LoadBonusDGV.BorderStyle = BorderStyle.None;

            SqlCommand cmd = Utilities.getCommand();
            cmd.CommandText = "select attribute, CreditPlusType from LoyaltyAttributes where CreditPlusType in ('A', 'G', 'B', 'L')";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                RadioButton rbLoadBonusType = new RadioButton();
                rbLoadBonusType.Name = dt.Rows[i][0].ToString();
                rbLoadBonusType.Text = rbLoadBonusType.Name;
                rbLoadBonusType.Tag = dt.Rows[i][1];
                rbLoadBonusType.AutoSize = true;

                rbLoadBonusType.Font = new System.Drawing.Font(rbLoadBonusType.Font.FontFamily, 12.0F, FontStyle.Bold);
                flpLoadBonusTypes.Controls.Add(rbLoadBonusType);
                Utilities.setLanguage(rbLoadBonusType);
            }
            string loadBonusType = Utilities.getParafaitDefaults("LOAD_BONUS_DEFAULT_ENT_TYPE");
            if (string.IsNullOrEmpty(loadBonusType) || (POSStatic.ParafaitEnv.MIFARE_CARD && loadBonusType == "G"))
                loadBonusType = "B";

            foreach (Control c in flpLoadBonusTypes.Controls)
            {
                if (POSStatic.ParafaitEnv.MIFARE_CARD && ((RadioButton)c).Tag.ToString() == "G")
                    c.Enabled = false;

                if (((RadioButton)c).Tag.ToString() == loadBonusType)
                    ((RadioButton)c).Checked = true;

                if (((RadioButton)c).Tag.ToString() != loadBonusType && _Parameter != null)
                    ((RadioButton)c).Enabled = false;
            }

            // disable bonus and gameplay credits in case card balance is default type and credits are loaded instead. code change done for CEC
            if (loadBonusType == "A" && Utilities.getParafaitDefaults("LOAD_CREDITS_INSTEAD_OF_CARD_BALANCE").Equals("Y"))
            {
                foreach (Control c in flpLoadBonusTypes.Controls)
                {
                    if (((RadioButton)c).Tag.ToString() != loadBonusType)
                        c.Enabled = false;

                    if (((RadioButton)c).Tag.ToString() == loadBonusType)
                        ((RadioButton)c).Checked = true;
                }
            }

            if (_Parameter != null)
            {
                object[] pars = _Parameter as object[];
                txtLoadBonus.Text = Convert.ToDecimal(pars[0]).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtLoadBonus.ReadOnly = true;
                txtRemarks.Text = "Refund Gameplay";

                Attribute2 = Convert.ToInt32(pars[1]);
            }

            log.Debug("Ends-LoadBonus()");
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Utilities.ParafaitEnv.ManagerId = -1;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.Debug("Ends-buttonCancel_Click()");
        }

        bool calculateRefundAmounts()
        {
            log.LogMethodEntry();
            double totalAmount = 0, totalCredits = 0, totalCreditPlus, cardDeposit;
            cardDeposit = Convert.ToDouble(dgvRefundCardData["RefundableAmount", 0].Value);
            totalCredits = Convert.ToDouble(dgvRefundCardData["RefundableAmount", 1].Value);
            totalCreditPlus = Convert.ToDouble(dgvRefundCardData["RefundableAmount", 2].Value);
            double refundTaxAmount = 0, taxAmount = 0;
            totalAmount = Math.Round(totalCredits + totalCreditPlus + cardDeposit, 4, MidpointRounding.AwayFromZero);

            displayMessageLine("");
            double refundAmount = 0;
            try
            {
                refundAmount = Math.Round(Convert.ToDouble(txtRefundAmount.Tag), 4, MidpointRounding.AwayFromZero);
                if (refundAmount > totalAmount)
                {
                    displayMessageLine(MessageUtils.getMessage(64));
                    log.Info("Ends-calculateRefundAmounts() as Refund amount exceeds Total Balance on card");
                    return false;
                }
                else if (refundAmount > totalCredits + totalCreditPlus && refundAmount < totalAmount)
                {
                    displayMessageLine(MessageUtils.getMessage(65));
                    log.Info("Ends-calculateRefundAmounts() as Cannot refund Card Deposit partially");
                    return false;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(66));
                log.Fatal("Ends-calculateRefundAmounts() as Entered refund amount was not valid");
                return false;
            }

            if (refundAmount <= totalCreditPlus)
            {
                dgvRefundCardData["Refund Amount", 2].Value = refundAmount;
                refundAmount = 0;
                log.Info("calculateRefundAmounts() - Refund amount:" + refundAmount);
            }
            else
            {
                dgvRefundCardData["Refund Amount", 2].Value = totalCreditPlus;
                refundAmount = refundAmount - totalCreditPlus;
                log.Info("calculateRefundAmounts() - Refund amount:" + refundAmount);
            }

            if (refundAmount <= totalCredits)
            {
                dgvRefundCardData["Refund Amount", 1].Value = refundAmount;
                refundAmount = 0;
                log.Info("calculateRefundAmounts() as Refund amount:" + refundAmount);
            }
            else
            {
                dgvRefundCardData["Refund Amount", 1].Value = totalCredits;
                refundAmount = refundAmount - totalCredits;
                log.Info("calculateRefundAmounts() - Refund amount:" + refundAmount);
            }

            if (refundAmount <= cardDeposit)
            {
                dgvRefundCardData["Refund Amount", 0].Value = refundAmount;
                refundAmount = 0;
                log.Info("calculateRefundAmounts() - Refund amount:" + refundAmount);
            }
            else
            {
                dgvRefundCardData["Refund Amount", 0].Value = cardDeposit;
                refundAmount = refundAmount - totalCredits;
                log.Info("calculateRefundAmounts() - Refund amount:" + refundAmount);
            }
            double refAmount = Convert.ToDouble(txtRefundAmount.Tag);
            //Begin: Calculate refund Tax 25-sept-2015

            DataTable dtRefundTax = Utilities.executeDataTable(@"select top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                 from Products p, product_type pt ,tax t
                                                                 where product_type = 'REFUND' 
                                                                 and p.product_type_id = pt.product_type_id
                                                                 and t.tax_id = p.tax_id");

            if (dtRefundTax.Rows.Count > 0)
            {
                if (dtRefundTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    refundTaxAmount = refAmount - cardDeposit;
                    taxAmount = (refundTaxAmount - refundTaxAmount / (1 + Convert.ToDouble(dtRefundTax.Rows[0]["taxPercentage"]) / 100));
                    lblTaxAmount.Text = taxAmount.ToString();

                }
                else
                {
                    refundTaxAmount = refAmount - cardDeposit;
                    taxAmount = (refundTaxAmount * ParafaitEnv.RefundCardTaxPercent / 100);
                    lblTaxAmount.Text = taxAmount.ToString();
                    refAmount = (refundTaxAmount * ParafaitEnv.RefundCardTaxPercent / 100) + refAmount;
                }
            }
            //End Calculate refund Tax 25-sept-2015

            //Begin Calculate Deposit Tax 01-Mar-2016
            DataTable dtRefundDepositTax = Utilities.executeDataTable(@"select top 1 t.tax_id taxId ,TaxInclusivePrice, t.tax_percentage taxPercentage
                                                                 from Products p, product_type pt ,tax t
                                                                 where product_type = 'REFUNDCARDDEPOSIT' 
                                                                 and p.product_type_id = pt.product_type_id
                                                                 and t.tax_id = p.tax_id");

            if (dtRefundDepositTax.Rows.Count > 0)
            {
                if (dtRefundDepositTax.Rows[0]["TaxInclusivePrice"].ToString() == "Y")
                {
                    taxAmount = (taxAmount + (cardDeposit - cardDeposit / (1 + Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100)));
                    lblTaxAmount.Text = taxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                }
                else
                {
                    taxAmount = (taxAmount + (cardDeposit * Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100));
                    lblTaxAmount.Text = taxAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                    refAmount = (cardDeposit * Convert.ToDouble(dtRefundDepositTax.Rows[0]["taxPercentage"]) / 100) + refAmount;//
                }
            }
            lblRefundAmount.Text = refAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            log.Info("calculateRefundAmounts() - Total Refund amount: " + refAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            log.Debug("Ends-calculateRefundAmounts()");
            return true;
        }

        bool validateIfValueLessOrEqual(object reference, object compare)
        {
            log.Debug("Starts-validateIfValueLessOrEqual()");
            decimal value = 0;
            if (compare == null || string.IsNullOrEmpty(compare.ToString().Trim()))
                return true;
            if (reference == null)
                return false;

            value = Convert.ToDecimal(compare);
            if (Convert.ToDecimal(reference) < value)
            {
                value = 0;
                log.Debug("Ends-validateIfValueLessOrEqual()");
                return false;
            }
            else
            {
                log.Debug("Ends-validateIfValueLessOrEqual()");
                return true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonOK_Click()");
            // perform the task on OK press
            string message = "";
            switch (TaskID)
            {
                case TaskProcs.TRANSFERCARD:
                    {
                        log.Info("buttonOK_Click() - TRANSFERCARD");
                        if (CurrentCard == null || CurrentCard.CardStatus == "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(67));
                            log.Warn("buttonOK_Click() -TRANSFERCARD- Please enter a valid From Card Number");
                        }
                        else
                        if (TransferToCard == null || TransferToCard.CardStatus != "NEW")
                        {
                            displayMessageLine(MessageUtils.getMessage(58));
                            log.Warn("buttonOK_Click() -TRANSFERCARD- Please tap a NEW Card to transfer");
                        }
                        else
                        {
                            try
                            {
                                List<Card> cards = new List<Card>();
                                cards.Add(CurrentCard);
                                cards.Add(TransferToCard);
                                if (isDamaged == false)
                                {
                                    transferCardType = "L";
                                    if (selectedTransferCardProductId < 0)
                                    {
                                        if (displayGroupExists && productExists)
                                        {
                                            string errorMessage = MessageUtils.getMessage(5613);
                                            log.Error(errorMessage);
                                            throw new Exception(errorMessage);
                                        }
                                        if (displayGroupExists && productExists == false)
                                        {
                                            string errorMessage = MessageUtils.getMessage(5614);
                                            log.Error(errorMessage);
                                            throw new Exception(errorMessage);
                                        }
                                        selectedTransferCardProductId = damagedTransferCardProductId;
                                        if (selectedTransferCardProductId < 0)
                                        {
                                            string errorMessage = MessageUtils.getMessage(5615);
                                            log.Error(errorMessage);
                                            throw new Exception(errorMessage);
                                        }
                                    }
                                    TaskTypesContainerDTO taskTypesContainerDTO = TaskTypesContainerList.GetTaskTypesContainerDTOList(Utilities.ExecutionContext.SiteId).Where(x => x.TaskType == TaskProcs.TRANSFERCARD).FirstOrDefault();
                                    PerformgenericOTPValidation(taskTypesContainerDTO, CurrentCard.card_id);
                                    newTrx = TaskProcs.TransferCardTransaction(selectedTransferCardProductId, cards, POSStatic.POSPrintersDTOList, txtRemarks.Text, ref message);
                                    if (newTrx.Trx_id > 0)
                                    {
                                        ReturnMessage = MessageUtils.getMessage(33);
                                        if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                                        {
                                            frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, newTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                                            transferCardOTPApprovals = null;
                                        }
                                        if (!string.IsNullOrEmpty(transferCardType))
                                        {
                                            FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, newTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                                            transferCardType = string.Empty;
                                        }
                                    }
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                else
                                {
                                    transferCardType = "D";
                                    if (damagedTransferCardProductId < 0)
                                    {
                                        throw new Exception("Please set up damaged transfer card product");
                                    }
                                    TaskTypesContainerDTO taskTypesContainerDTO = TaskTypesContainerList.GetTaskTypesContainerDTOList(Utilities.ExecutionContext.SiteId).Where(x => x.TaskType == TaskProcs.TRANSFERCARD).FirstOrDefault();
                                    PerformgenericOTPValidation(taskTypesContainerDTO, CurrentCard.card_id);
                                    newTrx = TaskProcs.TransferCardTransaction(damagedTransferCardProductId, cards, POSStatic.POSPrintersDTOList, txtRemarks.Text, ref message);
                                    if (newTrx.Trx_id > 0)
                                    {
                                        ReturnMessage = MessageUtils.getMessage(33);
                                        if (transferCardOTPApprovals != null && transferCardOTPApprovals.Any())
                                        {
                                            frmVerifyTaskOTP.CreateTrxUsrLogEntryForGenricOTPValidationOverride(transferCardOTPApprovals, newTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext, null);
                                            transferCardOTPApprovals = null;
                                        }
                                        if (!string.IsNullOrEmpty(transferCardType))
                                        {
                                            FormCardTasks.CreateTrxUsrLogEntryForTransferType(transferCardType, newTrx, Utilities.ParafaitEnv.LoginID, Utilities.ExecutionContext);
                                            transferCardType = string.Empty;
                                        }
                                    }
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                if (message != "")
                                {
                                    displayMessageLine(message);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                this.Cursor = Cursors.Default;
                                displayMessageLine(ex.Message);
                                return;
                            }
                        }

                        break;
                    }
                case TaskProcs.LOADTICKETS:
                    {
                        log.Info("buttonOK_Click() - LOADTICKETS");
                        int tickets;
                        try
                        {
                            tickets = Convert.ToInt32(textBoxLoadTickets.Text);
                            if (tickets <= 0)
                            {
                                displayMessageLine(MessageUtils.getMessage(34, POSStatic.TicketTermVariant));
                                log.Warn("buttonOK_Click() -LOADTICKETS- Please enter an Integer value (>0)");
                                return;
                            }
                            // if tickets are entered manually, they should be limited
                            if (textBoxLoadTickets.Enabled && tickets > ParafaitEnv.LOAD_TICKETS_LIMIT)
                            {
                                //Sorry, cannot proceed with &1 &2. Load ticket limit is &3  
                                string msgValue = MessageContainerList.GetMessage(Utilities.ExecutionContext, 2830, tickets, POSStatic.TicketTermVariant, ParafaitEnv.LOAD_TICKETS_LIMIT.ToString());
                                // displayMessageLine(MessageUtils.getMessage(35, ParafaitEnv.LOAD_TICKETS_LIMIT.ToString(), POSStatic.TicketTermVariant));
                                displayMessageLine(msgValue);
                                log.Warn(msgValue);
                                return;
                            }
                        }
                        catch
                        {
                            displayMessageLine(MessageUtils.getMessage(34, POSStatic.TicketTermVariant));
                            log.Fatal("Ends-buttonOK_Click() -LOADTICKETS- Please enter an valid Integer value (>0)");
                            return;
                        }
                        displayMessageLine("");

                        if (!ManagerApprovalCheck(tickets))
                            return;


                        SqlConnection cnn = Utilities.createConnection();
                        SqlTransaction sqlTrx = cnn.BeginTransaction();
                        SqlCommand cmd = Utilities.getCommand(sqlTrx);
                        try
                        {
                            List<TicketReceiptDTO> ticketReceiptDTOList = new List<TicketReceiptDTO>();
                            if (redemption.ticketSourceInfoObj != null && redemption.ticketSourceInfoObj.Count == 0)
                            {
                                clsRedemption.TicketSourceInfo ticketSourceobj = new clsRedemption.TicketSourceInfo();
                                ticketSourceobj.ticketSource = "Manual";
                                ticketSourceobj.ticketValue = tickets;
                                ticketSourceobj.sourceCurrencyRuleId = -1;
                                redemption.ticketSourceInfoObj.Add(ticketSourceobj);
                            }
                            else
                            {
                                foreach (clsRedemption.TicketSourceInfo item in redemption.ticketSourceInfoObj)
                                {
                                    if (item.ticketSource == "Receipt")
                                    {
                                        TicketReceiptList ticketreceiptList = new TicketReceiptList(Utilities.ExecutionContext);
                                        List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParam = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                                        searchParam.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, item.receiptNo));
                                        List<TicketReceiptDTO> ticketReceiptDTOListTemp = ticketreceiptList.GetAllTicketReceipt(searchParam);
                                        if (ticketReceiptDTOListTemp != null && ticketReceiptDTOListTemp.Count > 0)
                                        {
                                            ticketReceiptDTOList.Add(ticketReceiptDTOListTemp[0]);
                                        }
                                        else
                                        {
                                            TicketReceiptDTO ticketReceiptDTO = new TicketReceiptDTO(-1, -1, item.receiptNo, (POSStatic.ParafaitEnv.IsCorporate ? POSStatic.ParafaitEnv.SiteId : -1), "", false, -1, item.ticketValue, item.ticketValue, POSStatic.ParafaitEnv.LoginID, ServerDateTime.Now, false, -1, ServerDateTime.Now, POSStatic.ParafaitEnv.LoginID, ServerDateTime.Now);
                                            TicketReceipt newTicketReceipt = new TicketReceipt(Utilities.ExecutionContext, ticketReceiptDTO);
                                            newTicketReceipt.Save(sqlTrx);
                                            ticketReceiptDTOList.Add(newTicketReceipt.TicketReceiptDTO);
                                        }
                                    }
                                }
                            }
                            try
                            {
                                int manualTicketCount = 0;
                                manualTicketCount = redemption.ticketSourceInfoObj.FindAll(x => x.ticketSource == "Manual").Sum(y => y.ticketValue);
                                ManualTicketApprovalCheck(manualTicketCount);
                            }
                            catch (Exception exp)
                            {
                                log.Error(exp);
                                if (redemption.ticketSourceInfoObj != null && redemption.ticketSourceInfoObj.Count > 0)
                                {
                                    redemption.ticketSourceInfoObj.Clear();
                                }
                                if (sqlTrx != null)
                                {
                                    sqlTrx.Rollback();
                                }
                                if (cnn != null)
                                {
                                    cnn.Close();
                                }
                                displayMessageLine(MessageUtils.getMessage(exp.Message));
                                return;
                            }
                            int redemptionId = redemption.CreateLoadTicketRedemptionOrder(CurrentCard, redemption.ticketSourceInfoObj, sqlTrx);
                            //try
                            //{
                            //    redemptionId = 
                            //}
                            //catch(Exception ex)
                            //{
                            //    log.Error(ex);
                            //    displayMessageLine(ex.Message);
                            //    sqlTrx.Rollback();
                            //    cnn.Close();
                            //}
                            foreach (TicketReceiptDTO updateTicketReceiptDTO in ticketReceiptDTOList)
                            {
                                // TicketReceipt updateTicketReceipt = new TicketReceipt(item.receiptNo);
                                updateTicketReceiptDTO.RedemptionId = redemptionId;
                                updateTicketReceiptDTO.BalanceTickets = 0;
                                TicketReceipt updateTicketReceipt = new TicketReceipt(Utilities.ExecutionContext, updateTicketReceiptDTO);
                                updateTicketReceipt.Save(sqlTrx);
                            }
                            // This check is done upfront here to skip it in the loadticket method
                            RedemptionBL redemptionBL = new RedemptionBL(redemptionId, Utilities.ExecutionContext, sqlTrx);
                            bool managerApprovalReceived = (Utilities.ParafaitEnv.ManagerId != -1);
                            int totalTickets = redemption.ticketSourceInfoObj.Sum(y => y.ticketValue);
                            redemptionBL.ManualTicketLimitChecks(managerApprovalReceived, totalTickets);
                            //if (!TaskProcs.loadTickets(CurrentCard, tickets, txtRemarks.Text, manualTicketsArray, ref message))
                            if (!TaskProcs.loadTickets(CurrentCard, totalTickets, txtRemarks.Text, redemptionId, ref message, sqlTrx))
                            {
                                displayMessageLine(message);
                                if (redemption.ticketSourceInfoObj != null && redemption.ticketSourceInfoObj.Count > 0)
                                {
                                    redemption.ticketSourceInfoObj.Clear();
                                }
                                log.Error("Ends-buttonOK_Click() -LOADTICKETS- unable to loadTickets as error " + message);
                                sqlTrx.Rollback();
                                cnn.Close();
                            }
                            else
                            {
                                sqlTrx.Commit();
                                cnn.Close();

                                PoleDisplay.writeSecondLine(totalTickets.ToString() + " " + POSStatic.TicketTermVariant + " Loaded");
                                log.Info("buttonOK_Click() -LOADTICKETS- " + POSStatic.TicketTermVariant + " loaded successfully to  " + CurrentCard);

                                if (Utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "Y")
                                {
                                    PrintTaskDetails();
                                    managerId = -1; //set manager id to default value
                                }
                                else if (Utilities.getParafaitDefaults("AUTO_PRINT_LOAD_TICKETS") == "A")
                                {
                                    if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(484), "Print Receipt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        PrintTaskDetails();
                                        managerId = -1; //set manager id to default value
                                    }
                                }
                                ReturnMessage = MessageUtils.getMessage(36, POSStatic.TicketTermVariant);
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            if (redemption.ticketSourceInfoObj != null && redemption.ticketSourceInfoObj.Count > 0)
                            {
                                redemption.ticketSourceInfoObj.Clear();
                            }
                            displayMessageLine(ex.Message);
                            sqlTrx.Rollback();
                            cnn.Close();
                        }
                        break;
                    }
                case TaskProcs.REFUNDCARD:
                    {
                        log.Info("buttonOK_Click() - REFUNDCARD");
                        if (txtRemarks.Text.Trim() == "" && ParafaitEnv.REFUND_REMARKS_MANDATORY == "Y")
                        {
                            displayMessageLine(MessageUtils.getMessage(69));
                            log.Info("Ends-buttonOK_Click() -REFUNDCARD- Enter remarks for Card Refund");
                            this.ActiveControl = txtRemarks;
                            return;
                        }

                        decimal refundGameAmount = 0;
                        decimal refundCardAmount = 0;
                        decimal refundTimeAmount = 0;

                        //Begin Modification: Added to convert refund amount to decimal for validations//
                        if (txtRefundGameAmount.Text.Trim().Equals("") == false)
                        {
                            refundGameAmount = Convert.ToDecimal(txtRefundGameAmount.Text);
                        }
                        if (txtRefundAmount.Text.Trim().Equals("") == false)
                        {
                            refundCardAmount = Convert.ToDecimal(txtRefundAmount.Text);
                        }
                        if (txtRefundTime.Text.Trim().Equals("") == false)
                        {
                            refundTimeAmount = Convert.ToDecimal(txtRefundTime.Text);
                        }
                        //validate for Manager approval and Limit
                        if (!ManagerApprovalCheck(refundCardAmount))
                            return;

                        //Refund games as Credits, Card Balance or Bonus. on July 24, 2015
                        if (Convert.ToDouble(refundGameAmount) > gameRefundAmount)
                        {
                            POSUtils.ParafaitMessageBox("Amount to refund is greater than the total Transaction Amount");
                            log.Info("Ends-buttonOK_Click() -REFUNDCARD- Amount to refund is greater than the total Transaction Amount");
                            this.ActiveControl = txtRefundGameAmount;
                            txtRefundGameAmount.Text = gameRefundAmount.ToString();
                            return;
                        }
                        else
                        {
                            if ((refundGameAmount > 0) && (((KeyValuePair<string, string>)cmbLoadGameAttributes.SelectedItem).Key == ""))
                            {
                                if (!ManagerApprovalCheck(refundGameAmount, TaskProcs.LOADBONUS.ToString()))
                                {
                                    log.LogMethodExit("Manager Approval check failed");
                                    return;
                                }
                                if (!TaskProcs.RefundCardGames(CurrentCard, Convert.ToDouble(txtRefundGameAmount.Text), (int)txtRefundGameAmount.Tag, txtRemarks.Text, ref message))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -REFUNDCARD- unable to Refund card as error " + message);
                                }
                                else
                                {
                                    ReturnMessage = MessageUtils.getMessage(37);
                                    log.Info("Ends-buttonOK_Click() -REFUNDCARD- Card Refunded successfully");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                break;
                            }
                            else if ((refundGameAmount > 0) && (((KeyValuePair<string, string>)cmbLoadGameAttributes.SelectedItem).Key != ""))
                            {
                                string loadBonusType = ((KeyValuePair<string, string>)cmbLoadGameAttributes.SelectedItem).Value;
                                double bonus = Convert.ToDouble(refundGameAmount);
                                if (!ManagerApprovalCheck(refundGameAmount, TaskProcs.LOADBONUS.ToString()))
                                {
                                    log.LogMethodExit("Manager Approval check failed");
                                    return;
                                }
                                try
                                {
                                    //Do not proceed if there is geniune hold
                                    AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, CurrentCard.card_id, true, true);
                                    IsCardOnHold(accountBL.AccountDTO);
                                }
                                catch (Exception ex)
                                {
                                    displayMessageLine(ex.Message);
                                    log.Error(ex);
                                    return;
                                }
                                if (!TaskProcs.loadBonus(CurrentCard, bonus, TaskProcs.getEntitlementType(loadBonusType), false, Attribute2, txtRemarks.Text, ref message))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -REFUNDCARD- unable to loadBonus as error " + message);
                                }
                                else
                                {
                                    Utilities.executeNonQuery(@"update cardGames set BalanceGames = 0 where TrxId = @TrxId and BalanceGames > 0",
                                                                new SqlParameter("@TrxId", (int)txtRefundGameAmount.Tag));
                                    ReturnMessage = MessageUtils.getMessage(44);
                                    log.Info("buttonOK_Click() -REFUNDCARD- Bonus loaded successfully");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                break;
                            }
                        }
                        //End: Till here Refunding of games as Credits, Card Balance or Bonus.on July 24, 2015, //

                        //Begin: Refund time  as credits, Card Balance or Bonus.on July 24, 2015//
                        if (timeRefundAmount > 0 && Convert.ToDouble(txtRefundTime.Text) > timeRefundAmount)
                        {
                            POSUtils.ParafaitMessageBox("Amount to refund is greater than the total Transaction Amount");
                            log.Info("buttonOK_Click() -REFUNDCARD- Amount to refund is greater than the total Transaction Amount");
                            this.ActiveControl = txtRefundTime;
                            txtRefundTime.Text = timeRefundAmount.ToString();
                            return;
                        }
                        else
                        {
                            if ((refundTimeAmount > 0) && (((KeyValuePair<string, string>)cmbLoadTimeAttributes.SelectedItem).Key == ""))
                            {
                                //Here the refund value is refunded as 
                                if (!TaskProcs.RefundTime(CurrentCard, Convert.ToDouble(refundTimeAmount), txtRefundTime.Tag == System.DBNull.Value ? -1 : (int)txtRefundTime.Tag, txtRemarks.Text, ref message))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -REFUNDCARD- unable to RefundTime as error " + message);
                                }
                                else
                                {
                                    ReturnMessage = MessageUtils.getMessage(37);
                                    log.Info("buttonOK_Click() -REFUNDCARD- Card Refunded successfully ");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                break;
                            }
                            else if ((refundTimeAmount > 0) && (((KeyValuePair<string, string>)cmbLoadTimeAttributes.SelectedItem).Key != ""))
                            {
                                string loadBonusType = ((KeyValuePair<string, string>)cmbLoadTimeAttributes.SelectedItem).Value;
                                double bonus = Convert.ToDouble(refundTimeAmount);

                                try
                                {
                                    //Do not proceed if there is geniune hold
                                    AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, CurrentCard.card_id, true, true);
                                    IsCardOnHold(accountBL.AccountDTO);
                                }
                                catch (Exception ex)
                                {
                                    displayMessageLine(ex.Message);
                                    log.Error(ex);
                                    return;
                                }

                                if (!TaskProcs.loadBonus(CurrentCard, bonus, TaskProcs.getEntitlementType(loadBonusType), true, Attribute2, txtRemarks.Text, ref message))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -REFUNDCARD- unable to loadBonus as error " + message);
                                }
                                else
                                {
                                    if (txtRefundTime.Tag != System.DBNull.Value)
                                    {
                                        Utilities.executeNonQuery(@"update CardCreditPlus set CreditPlusBalance = 0
                                                                            where CreditPlusType ='M' 
                                                                            and CreditPlusBalance >0 and TrxId = @TrxId and Card_id = @cardId",
                                                                    new SqlParameter("@TrxId", (int)txtRefundTime.Tag),
                                                                    new SqlParameter("@cardId", CurrentCard.card_id));
                                    }
                                    else
                                    {
                                        Utilities.executeNonQuery(@"update cards
                                                                set time = 0 ,start_time =null
                                                                where time > 0 and card_id = @cardId",
                                                                    new SqlParameter("@cardId", CurrentCard.card_id));
                                    }
                                    ReturnMessage = MessageUtils.getMessage(44);
                                    log.Info("buttonOK_Click() -REFUNDCARD- Bonus loaded successfully");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                                break;
                            }
                        }
                        //End: Till here Refunding time  as credits, Card Balance or Bonus.on July 24, 2015//
                        if (!calculateRefundAmounts())
                        {
                            log.Info("Ends-dgvMultipleCards_CellClick() as !calculateRefundAmounts()");
                            return;
                        }
                        if (refundCardAmount >= 0)
                        {
                            if (CurrentCard != null)
                            {
                                LoadMultipleCards = new List<Card>();
                                LoadMultipleCards.Add(CurrentCard);
                            }
                            Transaction transaction = null;
                            if (!ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "DEFAULT_PAY_MODE").Equals("1") && CurrentCard != null)
                            {
                                transaction = new Transaction(Utilities);
                                transaction.Transaction_Amount = transaction.Net_Transaction_Amount = Convert.ToDouble(txtRefundAmount.Text) * -1;
                                if (transaction.Order == null)
                                {
                                    transaction.Order = new OrderHeaderBL(Utilities.ExecutionContext, new OrderHeaderDTO());
                                }
                                transaction.Order.OrderHeaderDTO.TransactionOrderTypeId = transaction.LoadTransactionOrderType()["Refund"];
                                ProductsList productsList = new ProductsList(Utilities.ExecutionContext);
                                List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
                                if (Convert.ToDouble(dgvRefundCardData["Refund Amount", 0].Value) > 0
                                    && Convert.ToDouble(dgvRefundCardData["Refund Amount", 1].Value) <= 0
                                    && Convert.ToDouble(dgvRefundCardData["Refund Amount", 2].Value) <= 0)
                                    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.REFUNDCARDDEPOSIT));
                                else if (Convert.ToDouble(dgvRefundCardData["Refund Amount", 0].Value) <= 0
                                    && (Convert.ToDouble(dgvRefundCardData["Refund Amount", 1].Value) > 0
                                        || Convert.ToDouble(dgvRefundCardData["Refund Amount", 2].Value) > 0))
                                    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.REFUND));
                                else
                                    searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME_LIST,
                             "'" + ProductTypeValues.REFUNDCARDDEPOSIT + "','" + ProductTypeValues.REFUND + "'"));

                                searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, Utilities.ExecutionContext.GetSiteId().ToString()));
                                List<ProductsDTO> productsDTOList = productsList.GetProductsDTOList(searchParams);
                                if (productsDTOList == null || (productsDTOList != null && productsDTOList.Count == 0))
                                {
                                    message = Utilities.MessageUtils.getMessage("Refund product not found");
                                    log.LogMethodExit("Refund product not found");
                                    return;
                                }
                                else
                                {
                                    if (productsDTOList.Count > 1)
                                    {
                                        foreach (ProductsDTO productsDTO in productsDTOList)
                                        {
                                            switch (productsDTO.ProductType)
                                            {
                                                case ProductTypeValues.REFUNDCARDDEPOSIT:
                                                    {
                                                        transaction.createTransactionLine(CurrentCard, productsDTO.ProductId, Convert.ToDouble(dgvRefundCardData["Refund Amount", 0].Value) * -1, 1, ref message);
                                                        transaction.TransactionLineList[transaction.TransactionLineList.Count - 1].Price = Convert.ToDouble(dgvRefundCardData["Refund Amount", 0].Value) * -1;
                                                        transaction.updateAmounts(false);
                                                        break;
                                                    }
                                                case ProductTypeValues.REFUND:
                                                    {
                                                        double totalRefundValue = Convert.ToDouble(dgvRefundCardData["Refund Amount", 1].Value) //Credits
                                                                                    + Convert.ToDouble(dgvRefundCardData["Refund Amount", 2].Value);//Credit plus
                                                        transaction.createTransactionLine(CurrentCard, productsDTO.ProductId, totalRefundValue * -1, 1, ref message);
                                                        transaction.TransactionLineList[transaction.TransactionLineList.Count - 1].Price = totalRefundValue * -1;
                                                        transaction.updateAmounts(false);
                                                        break;
                                                    }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        transaction.createTransactionLine(CurrentCard, productsDTOList[0].ProductId, Convert.ToDouble(txtRefundAmount.Text) * -1, 1, ref message);
                                        foreach (var transactionLine in transaction.TransactionLineList)
                                        {
                                            if (transactionLine.ProductID == productsDTOList[0].ProductId)
                                            {
                                                transactionLine.Price = Convert.ToDouble(txtRefundAmount.Text) * -1;
                                                transaction.updateAmounts();
                                            }
                                        }
                                    }
                                }
                                PaymentDetails paymentDetails = new PaymentDetails(transaction);
                                while (transaction.Net_Transaction_Amount != transaction.TotalPaidAmount)
                                {
                                    paymentDetails.ShowDialog();
                                    transaction.getTotalPaidAmount(null);
                                    transaction.updateAmounts(false);
                                    CurrentCard.last_update_time = Utilities.getServerTime();
                                    if (transaction.TotalPaidAmount == transaction.Net_Transaction_Amount)
                                    {
                                        log.Debug("Refund task - Payment completed via payment form");
                                        break;
                                    }
                                    else if (transaction.TotalPaidAmount == 0)
                                    {
                                        log.Debug("Refund Task - Payment form was cancelled");
                                        return;
                                    }
                                }
                            }
                            if (!TaskProcs.RefundCard(LoadMultipleCards,
                                                        Convert.ToDouble(dgvRefundCardData["Refund Amount", 0].Value),
                                                        Convert.ToDouble(dgvRefundCardData["Refund Amount", 1].Value),
                                                        Convert.ToDouble(dgvRefundCardData["Refund Amount", 2].Value),
                                                        txtRemarks.Text, ref message, chkMakeCardNewOnFullRefund.Checked, null, transaction))
                            {
                                displayMessageLine(message);
                                log.Error("buttonOK_Click() -REFUNDCARD- unable to RefundCard as error " + message);
                            }
                            else
                            {
                                if (chkMakeCardNewOnFullRefund.Checked)
                                {
                                    log.LogVariableState("Card to be refunded and keys changed to default: ", CurrentCard);
                                    ChangeAuthenticationKeyOfCard(CurrentCard);
                                }

                                string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                                log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
                                bool cashdrawerMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CASHDRAWER_ASSIGNMENT_MANDATORY_FOR_TRX");
                                log.Debug("cashdrawerMandatory :" + cashdrawerMandatory);
                                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(Utilities.ExecutionContext.SiteId,
                                                                                   ParafaitEnv.POSMachine, "", -1);

                                if (cashdrawerInterfaceMode != "NONE")
                                {
                                    if (pOSMachineContainerDTO.POSCashdrawerContainerDTOList == null ||
                                                                pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any() == false)
                                    {
                                        log.Error("cashdrawer is not mapped to the POS");
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Validate Cashdrawer")); // New message
                                        return;
                                    }
                                    int shiftId = POSUtils.GetOpenShiftId(ParafaitEnv.LoginID);
                                    log.Debug("Open ShiftId :" + shiftId);
                                    if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE)
                                        || cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                                    {
                                        if (pOSMachineContainerDTO != null && pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                                                                    pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                                        {
                                            var posCashdrawerDTO = pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Where(x => x.IsActive == true).FirstOrDefault();
                                            if (posCashdrawerDTO != null && posCashdrawerDTO.CashdrawerId > -1)
                                            {
                                                CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, posCashdrawerDTO.CashdrawerId);
                                                cashdrawerBL.OpenCashDrawer();
                                            }
                                        }
                                    }
                                    else if (shiftId > -1)
                                    {
                                        ShiftBL shiftBL = new ShiftBL(Utilities.ExecutionContext, shiftId);
                                        if (shiftBL.ShiftDTO.CashdrawerId > 0)
                                        {
                                            log.Debug("Open CashdrawerId :" + shiftBL.ShiftDTO.CashdrawerId);
                                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftBL.ShiftDTO.CashdrawerId);
                                            cashdrawerBL.OpenCashDrawer();
                                        }
                                    }
                                }


                                //////Added on 8th March 2017 for opening the cashdrawer
                                //if (ParafaitEnv.OPEN_CASH_DRAWER == "Y")
                                //{
                                //    PrintTransaction pt = new PrintTransaction(POSStatic.POSPrintersDTOList);

                                //    if (ParafaitEnv.CASH_DRAWER_INTERFACE == "Serial Port")
                                //    {
                                //        if (POSStatic.CashDrawerSerialPort != null && POSStatic.CashDrawerSerialPort.comPort.IsOpen)
                                //        {
                                //            POSStatic.CashDrawerSerialPort.comPort.Write(ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING, 0, ParafaitEnv.CASH_DRAWER_SERIALPORT_STRING.Length);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext);
                                //        printerBL.OpenCashDrawer();
                                //    }

                                //}//end
                                if (CurrentCard == null)
                                {
                                    if (TaskProcs.TransactionId != -1)
                                    {
                                        ListTrxId.Add(TaskProcs.TransactionId);
                                    }
                                }

                                ReturnMessage = MessageUtils.getMessage(37);
                                log.Info("buttonOK_Click() -REFUNDCARD- " + CurrentCard + " Card Refunded successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        break;
                    }
                case TaskProcs.REALETICKET:
                    {
                        log.Info("buttonOK_Click() - REALETICKET");
                        displayMessageLine("");
                        bool realTicket;
                        if (radioButtonRealTicket.Checked)
                            realTicket = true;
                        else
                            realTicket = false;

                        if (!TaskProcs.RealETicket(CurrentCard, realTicket, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -REALETICKET-  has error " + message);
                        }
                        else
                        {
                            ReturnMessage = MessageUtils.getMessage(38, POSStatic.TicketTermVariant.TrimEnd('s'));
                            log.Info("buttonOK_Click() -REALETICKET-  Card Mode changed successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.EXCHANGETOKENFORCREDIT:
                    {
                        log.Info("buttonOK_Click() - EXCHANGETOKENFORCREDIT");
                        int tokens; double credits;
                        try
                        {
                            tokens = Convert.ToInt32(txtTokensExchanged.Text);
                        }
                        catch
                        {
                            displayMessageLine(MessageUtils.getMessage(70));
                            log.Fatal("Ends-buttonOK_Click() -EXCHANGETOKENFORCREDIT- Please enter an Integer value (>0) for Tokens");
                            return;
                        }

                        credits = tokens * ParafaitEnv.CREDITS_PER_TOKEN;
                        txtCreditsAdded.Text = credits.ToString();

                        displayMessageLine("");
                        if (!ManagerApprovalCheck(tokens))
                            return;
                        if (!TaskProcs.exchangeTokenForCredit(CurrentCard, tokens, credits, txtRemarks.Text, ref message, -1))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -EXCHANGETOKENFORCREDIT-  has error " + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(tokens.ToString() + " Tokens = " + credits.ToString() + " Credits");
                            log.Info("buttonOK_Click() -EXCHANGETOKENFORCREDIT-  Tokens exchanged successfully");
                            ReturnMessage = MessageUtils.getMessage(39);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.EXCHANGECREDITFORTOKEN:
                    {
                        log.Info("buttonOK_Click() - EXCHANGECREDITFORTOKEN");
                        int tokens; double credits;
                        try
                        {
                            tokens = Convert.ToInt32(txtTokensBought.Text);
                        }
                        catch
                        {
                            displayMessageLine(MessageUtils.getMessage(70));
                            log.Fatal("Ends-buttonOK_Click() -EXCHANGECREDITFORTOKEN- Please enter an Integer value (>0) for Tokens");
                            return;
                        }

                        credits = tokens * ParafaitEnv.CREDITS_PER_TOKEN;
                        txtCreditsRequired.Text = credits.ToString();

                        displayMessageLine("");
                        if (!TaskProcs.exchangeCreditForToken(CurrentCard, tokens, credits, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -EXCHANGECREDITFORTOKEN-  has error " + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(tokens.ToString() + " Tokens = " + credits.ToString() + " Credits");
                            log.Info("buttonOK_Click() -EXCHANGECREDITFORTOKEN-  Credits exchanged successfully");
                            ReturnMessage = MessageUtils.getMessage(40);
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.CONSOLIDATE:
                    {
                        log.Info("buttonOK_Click() - CONSOLIDATE");
                        displayMessageLine("");
                        if (ConsCardCount <= 1)
                        {
                            displayMessageLine(MessageUtils.getMessage(71));
                            log.Info("Ends-buttonOK_Click() -CONSOLIDATE-  as Consolidation requires at least 2 cards");
                            return;
                        }

                        if (!TaskProcs.Consolidate(ConsolidateCards, ConsCardCount, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -CONSOLIDATE-  has error " + message);
                        }
                        else
                        {
                            ReturnMessage = MessageUtils.getMessage(41);
                            log.Info("buttonOK_Click() -CONSOLIDATE-  as Cards consolidated successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.BALANCETRANSFER:
                    {
                        log.Info("buttonOK_Click() - BALANCETRANSFER");
                        displayMessageLine("");
                        if (ConsCardCount == 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(749));

                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  as Please Tap the Card that Credits will be deducted from");
                            return;
                        }
                        else if (ConsCardCount == 1)
                        {
                            displayMessageLine(MessageUtils.getMessage(750));
                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  as Please Tap the Card that Credits will be added to");
                            return;
                        }

                        decimal credits = 0, bonus = 0, courtesy = 0, tickets = 0;
                        decimal trcredits = 0, trbonus = 0, trcourtesy = 0, trtickets = 0;

                        foreach (DataGridViewRow dr in dgvBalanceTransferee.Rows)
                        {
                            trcredits += Convert.ToDecimal(dr.Cells["dcTransferCredits"].Value);
                            trbonus += Convert.ToDecimal(dr.Cells["dcTransferBonus"].Value);
                            trtickets += Convert.ToDecimal(dr.Cells["dcTransferTickets"].Value);
                        }

                        if (!validateIfValueLessOrEqual(ConsolidateCards[0].credits + ConsolidateCards[0].CreditPlusCardBalance, trcredits))
                        {
                            displayMessageLine(MessageUtils.getMessage(746));
                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  credits part as Cannot transfer more than available");
                            return;
                        }
                        if (!validateIfValueLessOrEqual(ConsolidateCards[0].bonus + ConsolidateCards[0].CreditPlusBonus, trbonus))
                        {
                            displayMessageLine(MessageUtils.getMessage(746));
                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  bonus part as Cannot transfer more than available");
                            return;
                        }
                        if (!validateIfValueLessOrEqual(ConsolidateCards[0].ticket_count + ConsolidateCards[0].CreditPlusTickets, trtickets))
                        {
                            displayMessageLine(MessageUtils.getMessage(746));
                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  ticket count part as Cannot transfer more than available");
                            return;
                        }

                        if (trcredits + trcourtesy + trbonus + trtickets <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(747));
                            log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  as Please enter a value to transfer ");
                            return;
                        }

                        foreach (DataGridViewRow dr in dgvBalanceTransferee.Rows)
                        {
                            credits = Convert.ToDecimal(dr.Cells["dcTransferCredits"].Value);
                            bonus = Convert.ToDecimal(dr.Cells["dcTransferBonus"].Value);
                            tickets = Convert.ToDecimal(dr.Cells["dcTransferTickets"].Value);

                            if (credits + bonus + tickets > 0)
                            {
                                if (!TaskProcs.BalanceTransfer(ConsolidateCards[0].card_id, Convert.ToInt32(dr.Cells["dcTrToCardId"].Value), credits, bonus, courtesy, tickets, txtRemarks.Text, ref message))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -BALANCETRANSFER-  has error " + message);
                                }
                                else
                                {
                                    ReturnMessage = MessageUtils.getMessage(798);
                                    log.Info("Ends-buttonOK_Click() -BALANCETRANSFER-  Credits Transferred Successfully ");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                            }
                        }
                        break;
                    }
                case TaskProcs.LOADMULTIPLE:
                    {
                        log.Info("buttonOK_Click() - LOADMULTIPLE");
                        displayMessageLine("");
                        if (LoadMultipleCards.Count == 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(72));
                            log.Info("Ends-buttonOK_Click() -LOADMULTIPLE-  No Card entered");
                            return;
                        }

                        if (_Parameter != null)
                        {
                            object[] parameters = _Parameter as object[];

                            if (LoadMultipleCards.Count < Convert.ToInt32(parameters[1]))
                            {
                                displayMessageLine("At least " + parameters[1].ToString() + " Cards required");
                                log.Info("Ends-buttonOK_Click() -LOADMULTIPLE-  as At least " + parameters[1].ToString() + " Cards required ");
                                return;
                            }
                        }

                        // check if a NEW card product is chosen. a NEW product is mandatory for issuing NEW cards
                        bool newProductFound = false;
                        int newProductIndex = -1;
                        for (int i = 0; i < dgvProductsAdded.Rows.Count; i++)
                        {
                            string prodType = dgvProductsAdded.Rows[i].Cells["ProductType"].Value.ToString();
                            if (prodType == "NEW" && !LoadMultipleCards.Exists(x => x.CardStatus == "NEW"))
                            {
                                displayMessageLine(MessageUtils.getMessage(63));
                                log.Info("Ends-LoadMultiple() as tapped card was not a NEW Card to Load");
                                return;
                            }
                        }
                        for (int i = 0; i < dgvProductsAdded.Rows.Count; i++)
                        {
                            string prodType = dgvProductsAdded.Rows[i].Cells["ProductType"].Value.ToString();
                            if (prodType == "NEW" || prodType == "GAMETIME" || prodType == "CARDSALE")
                            {
                                newProductFound = true;
                                newProductIndex = i;
                                break;
                            }
                        }
                        if (!newProductFound)
                        {
                            displayMessageLine(MessageUtils.getMessage(73));
                            log.Info("Ends-buttonOK_Click() -LOADMULTIPLE- as need to choose a NEW, CARDSALE or GAMETIME Card Product");
                            return;
                        }

                        // new product should be the first product 
                        LoadMultipleProducts[0] = Convert.ToInt32(dgvProductsAdded.Rows[newProductIndex].Cells["ProductID"].Value);

                        int index = 1;
                        for (int i = 0; i < dgvProductsAdded.Rows.Count; i++)
                        {
                            if (i != newProductIndex) // skip new product
                            {
                                LoadMultipleProducts[index++] = Convert.ToInt32(dgvProductsAdded.Rows[i].Cells["ProductID"].Value);
                            }
                        }

                        ReturnMessage = MessageUtils.getMessage(42);
                        log.Info("Ends-buttonOK_Click() -LOADMULTIPLE- Multiple Card Issue");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }
                case TaskProcs.LOADBONUS:
                    {
                        log.Info("buttonOK_Click() - LOADBONUS");
                        if (CurrentCard == null)
                        {
                            displayMessageLine(Utilities.MessageUtils.getMessage(172));
                            log.Info("Ends-buttonOK_Click() -LOADBONUS- as Invalid Card");
                            return;
                        }

                        double bonus;
                        try
                        {
                            bonus = Convert.ToDouble(txtLoadBonus.Text);
                            if (bonus <= 0)
                            {
                                displayMessageLine(MessageUtils.getMessage(74));
                                log.Info("Ends-buttonOK_Click() -LOADBONUS- as Bonus Should be a positive value (>0) ");
                                txtLoadBonus.Focus();
                                return;
                            }
                            if (bonus > (double)ParafaitEnv.LOAD_BONUS_LIMIT)
                            {
                                displayMessageLine(MessageUtils.getMessage(43, ParafaitEnv.LOAD_BONUS_LIMIT.ToString(ParafaitEnv.AMOUNT_FORMAT)));
                                log.Info("Ends-buttonOK_Click() -LOADBONUS-  as entered a value less than or equal to " + ParafaitEnv.LOAD_BONUS_LIMIT.ToString(ParafaitEnv.AMOUNT_FORMAT) + " for Bonus");
                                txtLoadBonus.Focus();
                                return;
                            }
                        }
                        catch
                        {
                            displayMessageLine(MessageUtils.getMessage(74));
                            log.Info("Ends-buttonOK_Click() -LOADBONUS- as Bonus Should be a positive value (>0) ");
                            txtLoadBonus.Focus();
                            return;
                        }

                        txtRemarks.Text = txtRemarks.Text.Trim();
                        if (txtRemarks.Text == "" && Utilities.getParafaitDefaults("LOAD_BONUS_REMARKS_MANDATORY") == "Y")
                        {
                            displayMessageLine(MessageUtils.getMessage(201));
                            log.Info("Ends-buttonOK_Click() -LOADBONUS- as Remarks was not entered ");
                            txtRemarks.Focus();
                            return;
                        }

                        displayMessageLine("");
                        string loadBonusType = "B";
                        foreach (Control c in flpLoadBonusTypes.Controls)
                        {
                            if (((RadioButton)c).Checked)
                            {
                                loadBonusType = c.Tag.ToString();
                                break;
                            }
                        }
                        if (!ManagerApprovalCheck(bonus))
                            return;
                        if (!TaskProcs.loadBonus(CurrentCard, bonus, TaskProcs.getEntitlementType(loadBonusType), false, Attribute2, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -LOADBONUS-  has error " + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(bonus.ToString() + " Bonus Loaded");
                            ReturnMessage = MessageUtils.getMessage(44);
                            //the following section of code was added on 25-06-2015 to print load bonus details
                            if (Utilities.getParafaitDefaults("AUTO_PRINT_LOAD_BONUS") == "Y")
                            {
                                PrintTaskDetails();
                                managerId = -1; //set manager id to default value
                            }
                            else if (Utilities.getParafaitDefaults("AUTO_PRINT_LOAD_BONUS") == "A")
                            {
                                if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(484), "Print Receipt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    PrintTaskDetails();
                                    managerId = -1; //set manager id to default value
                                }
                            }
                            //-code change on 25-06-2015 end
                            log.Info("Ends-buttonOK_Click() -LOADBONUS- as Bonus loaded successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }

                case TaskProcs.DISCOUNT:
                    {
                        log.Info("buttonOK_Click() - DISCOUNT");
                        SortableBindingList<CardDiscountsDTO> cardDiscountsDTOList = null;
                        if (cardDiscountsDTOListBS.DataSource != null && cardDiscountsDTOListBS.DataSource is SortableBindingList<CardDiscountsDTO>)
                        {
                            cardDiscountsDTOList = cardDiscountsDTOListBS.DataSource as SortableBindingList<CardDiscountsDTO>;
                        }
                        if (!TaskProcs.applyDiscount(CurrentCard, cardDiscountsDTOList, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -DISCOUNT- has error " + message);
                        }
                        else
                        {
                            ReturnMessage = MessageUtils.getMessage(45);
                            log.Info("Ends-buttonOK_Click() -DISCOUNT- as Discount applied successfully ");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }

                case TaskProcs.REDEEMLOYALTY:
                    {
                        log.Info("buttonOK_Click() - REDEEMLOYALTY");
                        double redeemPoints = Convert.ToDouble(txtLoyaltyRedeemPoints.Text);
                        if (redeemPoints <= 0)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            break;
                        }
                        if (!ManagerApprovalCheck(redeemPoints))
                            return;
                        if (!TaskProcs.RedeemLoyalty(CurrentCard, redeemPoints, dgvLoyaltyRedemption, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -REDEEMLOYALTY- has error " + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(redeemPoints.ToString() + " points redeemed");
                            ReturnMessage = MessageUtils.getMessage(46);
                            log.Info("Ends-buttonOK_Click() -REDEEMLOYALTY- as Loyalty points redeemed successfully ");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.REDEEMVIRTUALPOINTS:
                    {
                        log.Info("REDEEMVIRTUALLOYALTY");
                        double redeemPoints = Convert.ToDouble(txtVirtualRedeemPoints.Text);
                        if (redeemPoints <= 0)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            break;
                        }
                        if (!ManagerApprovalCheck(redeemPoints))
                        {
                            log.Debug("ManagerApprovalCheck Failed");
                            return;
                        }
                        if (!TaskProcs.RedeemVirtualPoints(CurrentCard, redeemPoints, dgvVirtualPointRedemption, txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("REDEEMVIRTUALPOINTS error " + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(redeemPoints.ToString() + " points redeemed");
                            ReturnMessage = MessageUtils.getMessage("Virtual Points ") + MessageUtils.getMessage(797);
                            log.Info("REDEEMVIRTUALPOINTS- as Virtual points redeemed successfully ");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.SPECIALPRICING:
                    {
                        log.Info("buttonOK_Click() - SPECIALPRICING");
                        displayMessageLine("");

                        if (dgvSpecialPricing.CurrentRow == null)
                        {
                            displayMessageLine(MessageUtils.getMessage(75));
                            log.Info("Ends-buttonOK_Click() -SPECIALPRICING- as Special Pricing option was not selected");
                            return;
                        }
                        else
                        {
                            if (dgvSpecialPricing.CurrentRow.Cells["RequiresManagerApproval"].Value.ToString() == "Y")
                            {
                                if (Utilities.ParafaitEnv.ManagerId == -1 && !Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                                {
                                    displayMessageLine(MessageUtils.getMessage(76));
                                    log.Info("Ends-buttonOK_Click() -SPECIALPRICING- as Manager approval required for using this special pricing option");
                                    return;
                                }
                            }
                        }

                        if (txtRemarks.Text.Trim() == "")
                        {
                            displayMessageLine(MessageUtils.getMessage(77));
                            log.Info("Ends-buttonOK_Click() -SPECIALPRICING- as remarks was not entered for Special Pricing");
                            this.ActiveControl = txtRemarks;
                            return;
                        }

                        Utilities.ParafaitEnv.specialPricingId = Convert.ToInt32(dgvSpecialPricing.CurrentRow.Cells["PricingId"].Value);
                        Utilities.ParafaitEnv.specialPricingRemarks = txtRemarks.Text;
                        PoleDisplay.writeFirstLine(dgvSpecialPricing.CurrentRow.Cells["PricingName"].Value.ToString());
                        ReturnMessage = dgvSpecialPricing.CurrentRow.Cells["PricingName"].Value.ToString() + " Special pricing applied successfully";
                        log.Info("buttonOK_Click() -SPECIALPRICING- as Special pricing applied successfully");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    }

                case TaskProcs.REDEEMTICKETSFORBONUS:
                    {
                        log.Info("buttonOK_Click() - REDEEMTICKETSFORBONUS");
                        double bonusEligible = Convert.ToDouble(txtBonusEligible.Text);
                        if (bonusEligible <= 0)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            break;
                        }

                        int redeemTickets = Convert.ToInt32(txtTicketsToRedeem.Text);

                        if (!ManagerApprovalCheck(redeemTickets))
                            return;
                        #region Added on April-26-2017 code for redeeming tickets for credits 
                        if (rbCardBalance.Checked)
                        {
                            if (!TaskProcs.RedeemTicketsForCredit(CurrentCard, redeemTickets, bonusEligible, txtRemarks.Text, ref message))
                            {
                                displayMessageLine(message);
                                log.Error("buttonOK_Click() -REDEEMTICKETSFORCREDITS- unable to RedeemTicketsForCredits as error" + message);
                                return;
                            }
                            else
                            {
                                PoleDisplay.writeSecondLine(redeemTickets.ToString() + " " + POSStatic.TicketTermVariant + " redeemed");
                                ReturnMessage = MessageUtils.getMessage(1171, POSStatic.TicketTermVariant);
                                log.Info("Ends-buttonOK_Click() -REDEEMTICKETSFORBONUS- " + POSStatic.TicketTermVariant + "redeemed for Credits successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        #endregion
                        else
                        {
                            //modification added on 30-May-2017
                            //checks Bonus load limit 
                            if (bonusEligible > (double)ParafaitEnv.LOAD_BONUS_LIMIT)
                            {
                                displayMessageLine(MessageUtils.getMessage(43, ParafaitEnv.LOAD_BONUS_LIMIT.ToString(ParafaitEnv.AMOUNT_FORMAT)));
                                log.Info("Ends-buttonOK_Click() -LOADBONUS-  as entered a value less than or equal to " + ParafaitEnv.LOAD_BONUS_LIMIT.ToString(ParafaitEnv.AMOUNT_FORMAT) + " for Bonus");
                                txtLoadBonus.Focus();
                                return;
                            }
                            //end modification added on 30-May-2017

                            if (!TaskProcs.RedeemTicketsForBonus(CurrentCard, redeemTickets, bonusEligible, txtRemarks.Text, ref message))
                            {
                                displayMessageLine(message);
                                log.Error("buttonOK_Click() - REDEEMTICKETSFORBONUS - unable to RedeemTicketsForCredits as error" + message);
                                return;
                            }
                            else
                            {
                                PoleDisplay.writeSecondLine(redeemTickets.ToString() + " " + POSStatic.TicketTermVariant + " redeemed");
                                ReturnMessage = MessageUtils.getMessage(47, POSStatic.TicketTermVariant);
                                log.Info("Ends-buttonOK_Click() -REDEEMTICKETSFORBONUS- " + POSStatic.TicketTermVariant + "redeemed for Bonus successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        break;
                    }
                case TaskProcs.SETCHILDSITECODE:
                    {
                        log.Info("buttonOK_Click() - SETCHILDSITECODE");
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        ReturnMessage = MessageUtils.getMessage(48);
                        log.Info("Ends-buttonOK_Click() -SETCHILDSITECODE- as Task completed successfully");
                        this.Close();
                        break;
                    }
                case TaskProcs.GETMIFAREGAMEPLAY:
                    {
                        log.Info("buttonOK_Click() - GETMIFAREGAMEPLAY");
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        ReturnMessage = MessageUtils.getMessage(48);
                        log.Info("Ends-buttonOK_Click() -GETMIFAREGAMEPLAY- as Task completed successfully");
                        this.Close();
                        break;
                    }
                //Added for REDEEMBONUSFORTICKET on 16-Jun-2017
                case TaskProcs.REDEEMBONUSFORTICKET:
                    {
                        log.Info("buttonOK_Click() - REDEEMBONUSFORTICKET");
                        double ticketsEligible = Convert.ToDouble(txtElgibleTickets.Text);
                        if (ticketsEligible <= 0)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                            break;
                        }
                        double redeemBonus = Convert.ToDouble(txtBonusToRedeem.Text);
                        string minimumBonus = Utilities.getParafaitDefaults("MINIMUM_BONUS_VALUE_FOR_TICKET_REDEMPTION");

                        #region Check minimum Bonus value
                        if (!string.IsNullOrEmpty(minimumBonus))
                        {
                            try
                            {
                                double minumumBonusValue = Convert.ToDouble(minimumBonus);
                                if (minumumBonusValue > redeemBonus)
                                {
                                    displayMessageLine(MessageUtils.getMessage(1196, minumumBonusValue));
                                    break;
                                }
                            }
                            catch (Exception ex)
                            {
                                displayMessageLine(ex.Message);
                                break;
                            }
                        }
                        #endregion
                        if (!ManagerApprovalCheck(redeemBonus))
                            return;
                        if (!TaskProcs.RedeemBonusForTicket(CurrentCard, redeemBonus, Convert.ToInt32(ticketsEligible), txtRemarks.Text, ref message))
                        {
                            displayMessageLine(message);
                            log.Error("buttonOK_Click() -REDEEMBONUSFORTICKET- unable to RedeemBonusForTickets as error" + message);
                        }
                        else
                        {
                            PoleDisplay.writeSecondLine(redeemBonus.ToString() + " Bonus redeemed");
                            ReturnMessage = MessageUtils.getMessage(1194, POSStatic.TicketTermVariant);
                            log.Info("Ends-buttonOK_Click() -REDEEMBONUSFORTICKET Bonus redeemed for ticket successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                //Added 22-Dec-2017
                case TaskProcs.EXCHANGECREDITFORTIME:
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(dgvPointsToConvert["dcAdditionalPoints", 0].Value.ToString()) ||
                        Convert.ToDecimal(dgvPointsToConvert["dcAdditionalPoints", 0].Value) <= 0)
                            {
                                displayMessageLine(Utilities.MessageUtils.getMessage(1382));
                                return;
                            }

                            decimal TotalPoints = Convert.ToDecimal(dgvTappedCard["ParentCredits", 0].Value);

                            decimal additionalPoints = Convert.ToDecimal(dgvPointsToConvert["dcAdditionalPoints", 0].Value);

                            if (additionalPoints > TotalPoints)
                            {
                                displayMessageLine(Utilities.MessageUtils.getMessage(1383, TotalPoints));
                                return;
                            }

                            TaskProcs taskProcs = new TaskProcs(Utilities);

                            Card updateCard = new Card(dgvTappedCard["ParentCardNumber", 0].Value.ToString(), ParafaitEnv.LoginID, Utilities);

                            //string message = "";
                            bool succTransfer = true;
                            succTransfer = taskProcs.ConvertCreditsForTime(updateCard, Convert.ToDouble(additionalPoints), -1, -1, true, txtRemarks.Text, ref message);
                            if (!succTransfer)
                                displayMessageLine(message);
                            else
                            {
                                PoleDisplay.writeSecondLine(MessageUtils.getMessage(1384));
                                ReturnMessage = MessageUtils.getMessage(1384);
                                log.Info("Ends-buttonOK_Click() -EXCHANGECREDITFORTIME Points converted to Time successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            displayMessageLine(ex.Message);
                        }
                        break;
                    }
                case TaskProcs.PAUSETIMEENTITLEMENT:
                    {

                        bool succPauseTime = true;
                        succPauseTime = TaskProcs.PauseTimeEntitlement(CurrentCard.card_id, txtRemarks.Text, ref message);
                        if (!succPauseTime)
                            displayMessageLine(message);
                        else
                        {
                            //PoleDisplay.writeSecondLine(additionalPoints.ToString() + " Points converted to Time");
                            ReturnMessage = MessageUtils.getMessage(1388);
                            log.Info("Ends-buttonOK_Click() -PAUSETIMEENTITLEMENT Time Paused successfully");
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        break;
                    }
                case TaskProcs.EXCHANGETIMEFORCREDIT:
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(dgvTimeToConvert["dcNewTimeToConvert", 0].Value.ToString()) ||
                        Convert.ToDecimal(dgvTimeToConvert["dcNewTimeToConvert", 0].Value) <= 0)
                            {
                                displayMessageLine(Utilities.MessageUtils.getMessage(1442));
                                return;
                            }

                            double TotalTime = Convert.ToDouble(dgvCardInfo["dcParentTime", 0].Value);

                            double additionalTime = Convert.ToDouble(dgvTimeToConvert["dcNewTimeToConvert", 0].Value);

                            if (additionalTime > TotalTime)
                            {
                                displayMessageLine(Utilities.MessageUtils.getMessage(1441, TotalTime));
                                return;
                            }

                            TaskProcs taskProcs = new TaskProcs(Utilities);

                            Card updateCard = new Card(dgvCardInfo["dcParentCardNumber", 0].Value.ToString(), ParafaitEnv.LoginID, Utilities);

                            //string message = "";
                            bool succTransfer = true;
                            succTransfer = taskProcs.ConvertTimeForCredit(updateCard, additionalTime, true, txtRemarks.Text, ref message);
                            if (!succTransfer)
                                displayMessageLine(message);
                            else
                            {
                                PoleDisplay.writeSecondLine(MessageUtils.getMessage(1443));
                                ReturnMessage = MessageUtils.getMessage(1443);
                                log.Info("Ends-buttonOK_Click() -EXCHANGETIMEFORCREDIT Time converted to points successfully");
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            displayMessageLine(ex.Message);
                        }
                        break;
                    }
                case TaskProcs.HOLDENTITLEMENTS:
                    {
                        if (HoldEntDGV.Rows.Count == 0 && string.IsNullOrEmpty(textBoxHoldEntitlementNumber.Text) == false)
                        {
                            this.getHoldCardDetails.PerformClick();
                        }
                        else
                        {
                            bool hold = true;
                            string displayText = string.Empty;
                            if (buttonOK.Text == "HOLD")
                            {
                                hold = true;
                            }
                            if (buttonOK.Text == "UNHOLD")
                            {
                                hold = false;
                            }
                            log.Info("buttonOK_Click() - HOLDENTITLEMENTS");
                            if (CurrentCard == null || CurrentCard.CardStatus == "NEW")
                            {
                                displayMessageLine(MessageUtils.getMessage(776));
                                log.Debug("HOLDENTITLEMENTS- Please enter a valid Card Number");
                            }
                            else
                            {
                                displayMessageLine("");
                                if (!TaskProcs.HoldEntitlements(CurrentCard, txtRemarks.Text, ref message, hold, null))
                                {
                                    displayMessageLine(message);
                                    log.Error("buttonOK_Click() -HOLDENTITLEMENTS- Hold entitlements operation failed " + message);
                                }
                                else
                                {
                                    if (hold)
                                    {
                                        displayText = "hold";
                                    }
                                    else
                                    {
                                        displayText = "unhold";
                                    }
                                    ReturnMessage = MessageUtils.getMessage(2957, displayText); // Card entitlements &1 is successful.
                                    log.Info("HOLDENTITLEMENTS- Hold entitlements successfull");
                                    this.DialogResult = DialogResult.OK;
                                    this.Close();
                                }
                            }
                        }
                        break;
                    }
                //end
                default: break;
            }
            log.Debug("Ends-buttonOK_Click()");
        }

        #region LoadBonusPrint
        //The followig section of code was added on 23-06-2015 to provide print of bonus details
        private bool SetupThePrinting()
        {
            log.Debug("Starts-SetupThePrinting()");
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDialog.UseEXDialog = true;

            if (Utilities.getParafaitDefaults("SHOW_PRINT_DIALOG_IN_POS").Equals("Y"))
            {
                if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.Debug("Ends-SetupThePrinting()");
                    return false;
                }
            }

            //Added code on 26-April-2017 to print POS Load Tickets
            if (TaskProcs.LOADTICKETS == TaskID)
            {
                MyPrintDocument.DocumentName = MessageUtils.getMessage("POS Load Tickets");
            }
            else if (TaskProcs.LOADBONUS == TaskID)
            {
                MyPrintDocument.DocumentName = MessageUtils.getMessage("POS Load Bonus");
            }
            else
            {
                log.Debug("Ends-SetupThePrinting()");
                return false;
            }

            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins =
                             new Margins(10, 10, 20, 20);

            log.Debug("Ends-SetupThePrinting()");
            return true;
        }

        #region AuthenticationChangetoDefaultKey
        private void ChangeAuthenticationKeyOfCard(Card CurrentCard)
        {
            log.LogMethodEntry(CurrentCard);
            if (CurrentCard == null || CurrentCard.ReaderDevice == null)
                return;
            byte[] customerAuthKey = new byte[6];
            int customerKey = POSStatic.Utilities.MifareCustomerKey;

            const int BLOCK_NUMBER = 4; //block number for changing auth key
            byte[] basicAuthKey = new byte[6]; //default key
            string authkey = Encryption.Decrypt(Encryption.GetParafaitKeys("MifareAuthorization"));//Encryption.Decrypt("0aNVShI2+C3Nw3yOnFGjbk+wLOV7Ia7z");
            string[] sa = authkey.Substring(0, 17).Split('-');
            int ikey = 0;
            foreach (string s in sa)
            {
                basicAuthKey[ikey++] = Convert.ToByte(s, 16);
            }

            string key = Encryption.GetParafaitKeys("NonMifareAuthorization");
            for (int i = 0; i < 5; i++)
                customerAuthKey[i] = Convert.ToByte(key[i]);
            customerAuthKey[5] = Convert.ToByte(customerKey);

            byte[] siteIdBuffer = new byte[16];
            siteIdBuffer[0] = siteIdBuffer[1] = siteIdBuffer[2] = siteIdBuffer[3] = 0xff;

            string message = "";

            try //Change authentication key to default key
            {
                if (CurrentCard.ReaderDevice.CardType == CardType.MIFARE)
                {
                    CurrentCard.ReaderDevice.change_authentication_key(BLOCK_NUMBER + 3, customerAuthKey, basicAuthKey, ref message);
                }
                else if (CurrentCard.ReaderDevice.CardType == CardType.MIFARE_ULTRA_LIGHT_C)
                {
                    CurrentCard.ReaderDevice.change_authentication_key(BLOCK_NUMBER + 3, ulcKeyStore.LatestCustomerUlcKey.Value, ulcKeyStore.DefaultUltralightCKeys[0].Value, ref message);
                }
                //readerDevice.write_data(6, 1, basicAuthKey, siteIdBuffer, ref message);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                displayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }
        #endregion AuthenticationChangetoDefaultKey

        bool PrintTaskDetails()
        {
            log.Debug("Starts-PrintTaskDetails()");
            if (SetupThePrinting())
            {
                try
                {
                    MyPrintDocument.Print();
                    log.Debug("Ends-PrintTaskDetails()");
                    return true;
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message, MessageUtils.getMessage("Print Error"));
                    log.Fatal("Ends-PrintTaskDetails() due to exception " + ex.Message);
                    return false;
                }
            }
            else
            {
                log.Debug("Ends-PrintTaskDetails()");
                return false;
            }
        }

        void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            log.Debug("Starts-MyPrintDocument_PrintPage()");
            int col1x = 10;
            int yLocation = 40;
            int yIncrement = 20;
            string bonusType = "";
            string authorizedBy = "";

            Font defaultFont = new System.Drawing.Font("courier narrow", 7.5f);

            //Modifed code on April 25th 2017 for printing POS Load Tickets
            string siteName = ParafaitEnv.SiteName;

            e.Graphics.DrawString(siteName, new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;

            if (TaskProcs.LOADTICKETS == TaskID)
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Load Tickets"), new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            }
            else if (TaskProcs.LOADBONUS == TaskID)
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Load Bonus"), new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            }
            else
            {
                log.Debug("Ends-MyPrintDocument_PrintPage()");
                return;
            }
            //end

            yLocation += 30;
            e.Graphics.DrawString(MessageUtils.getMessage("POS Name") + ": " + ParafaitEnv.POSMachine, new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;

            //Modifed code on April 25th 2017 for assiging managerName to authorizedBy variable
            //Check the CurrentUser is Manager
            if (ParafaitEnv.User_Id == managerId)
            {
                authorizedBy = ParafaitEnv.Username;
            }
            else if (managerId != -1)
            {
                object objAuthBy = Utilities.executeScalar(@"SELECT username 
                                                             FROM users 
                                                             WHERE user_id=@userid",
                                                             new SqlParameter("@userid", managerId));
                authorizedBy = Convert.ToString(objAuthBy);
            }//end

            e.Graphics.DrawString(MessageUtils.getMessage("Date") + ": " + ServerDateTime.Now.ToString(ParafaitEnv.DATETIME_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;

            e.Graphics.DrawString(MessageUtils.getMessage("Cashier") + ": " + ParafaitEnv.Username, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;

            if (!string.IsNullOrEmpty(authorizedBy.Trim()))
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Authorized By") + ": " + authorizedBy, defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
            }

            e.Graphics.DrawString(MessageUtils.getMessage("Card") + ": " + CurrentCard.CardNumber, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;

            foreach (Control c in flpLoadBonusTypes.Controls)
            {
                if (((RadioButton)c).Checked == true)
                {
                    bonusType = ((RadioButton)c).Text.ToString();
                    break;
                }
            }

            //Modifed code on April 25th 2017 for printing POS Load Tickets
            if (TaskProcs.LOADTICKETS == TaskID)
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Tickets Loaded") + ": " + (Convert.ToDecimal(textBoxLoadTickets.Text)).ToString(ParafaitEnv.AMOUNT_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
            }
            else
            {
                e.Graphics.DrawString(MessageUtils.getMessage("Bonus Type") + ": " + bonusType, defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageUtils.getMessage("Bonus Amount") + ": " + (Convert.ToDecimal(txtLoadBonus.Text)).ToString(ParafaitEnv.AMOUNT_FORMAT), defaultFont, Brushes.Black, col1x, yLocation);
                yLocation += yIncrement;
            }//end

            e.Graphics.DrawString(MessageUtils.getMessage("Remarks") + ": " + txtRemarks.Text, defaultFont, Brushes.Black, col1x, yLocation);


            log.Debug("Ends-MyPrintDocument_PrintPage()");
        }
        //Changes on 23-06-2015 end
        #endregion

        private void buttonTransferCardGetDetails_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonTransferCardGetDetails_Click()");
            if (string.IsNullOrEmpty(textBoxTransferCardNumber.Text.Trim()))
            {
                log.Info("Ends-buttonTransferCardGetDetails_Click() as TransferCardNumber is null ");
                return;
            }

            string lclCardNumber = "";
            if (rb10DFormat.Checked)
            {
                try
                {
                    byte[] b = BitConverter.GetBytes(Convert.ToInt32(textBoxTransferCardNumber.Text));
                    string Code = BitConverter.ToString(b).Replace("-", "");
                    char[] arr = Code.ToCharArray();

                    for (int i = 0; i < Code.Length / 2; i += 2)
                    {
                        char x = arr[i];
                        char y = arr[i + 1];

                        arr[i] = arr[Code.Length - i - 2];
                        arr[i + 1] = arr[Code.Length - i - 1];

                        arr[Code.Length - i - 2] = x;
                        arr[Code.Length - i - 1] = y;
                    }
                    lclCardNumber = new string(arr);
                    object o = Utilities.executeScalar("select top 1 card_number from cards where card_number like '%" + lclCardNumber + "' and valid_flag = 'Y'");
                    if (o != null)
                        lclCardNumber = o.ToString();
                }
                catch { }
            }
            else
                lclCardNumber = textBoxTransferCardNumber.Text;

            //CurrentCard = new Card(lclCardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            CurrentCard = new Card(Common.Devices.PrimaryCardReader, lclCardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            string message = string.Empty;
            if (!POSUtils.refreshCardFromHQ(ref CurrentCard, ref message))
            {
                displayMessageLine(message);
                log.Debug("Ends-TransferCard() as unable to refresh card from HQ");
                return;
            }
            if (CurrentCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(172));
                log.Warn("buttonTransferCardGetDetails_Click() as Invalid Card");
            }
            else if (CurrentCard.siteId != -1 && CurrentCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Warn("buttonTransferCardGetDetails_Click() as Roaming cards not allowed ");
            }
            else
            {
                TaskProcs.getCardDetails(CurrentCard, ref FromCardDGV);
                Utilities.setLanguage(FromCardDGV);
            }
            log.Debug("Ends-buttonTransferCardGetDetails_Click()");
        }

        private void ButtonGetCardDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(textBoxHoldEntitlementNumber.Text.Trim()))
            {
                log.Info("CardNumber is null ");
                return;
            }
            bool isCardOnHold = false;
            string lclCardNumber = "";
            lclCardNumber = textBoxHoldEntitlementNumber.Text;
            bool flag = false;
            CurrentCard = new Card(Common.Devices.PrimaryCardReader, lclCardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            string message = string.Empty;
            if (!POSUtils.refreshCardFromHQ(ref CurrentCard, ref message))
            {
                displayMessageLine(message);
                log.Debug("unable to refresh card from HQ");
                return;
            }
            if (CurrentCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(172));
                log.Debug("Invalid Card");
            }
            else if (CurrentCard.siteId != -1 && CurrentCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Debug("Roaming cards not allowed ");
            }
            else
            {
                TaskProcs.getCardDetails(CurrentCard, ref HoldEntDGV);
                Utilities.setLanguage(HoldEntDGV);
                flag = true;
            }
            if (flag == true)
            {
                AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, CurrentCard.card_id, true, true);
                AccountDTO accountDTO = accountBL.AccountDTO;
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
                if ((accountDTO.AccountCreditPlusDTOList == null || accountDTO.AccountCreditPlusDTOList.Count == 0) && (accountDTO.AccountGameDTOList == null || accountDTO.AccountGameDTOList.Count == 0) && (accountDTO.AccountDiscountDTOList == null || accountDTO.AccountDiscountDTOList.Count == 0))
                {
                    displayMessageLine(MessageUtils.getMessage(12557));
                    log.Debug("card has no entitlements");
                    return;
                }
                if ((accountDTO.AccountCreditPlusDTOList == null || accountDTO.AccountCreditPlusDTOList.Count == 0) && (accountDTO.AccountGameDTOList == null || accountDTO.AccountGameDTOList.Count == 0) && (accountDTO.AccountDiscountDTOList == null || accountDTO.AccountDiscountDTOList.Count == 0))
                {
                    displayMessageLine(MessageUtils.getMessage(12557));
                    log.Debug("Ends-LoadHoldEntitlements() as card has no entitlements");
                    return;
                }
                if (accountDTO != null
                && accountDTO.AccountCreditPlusDTOList != null
                && accountDTO.AccountCreditPlusDTOList.Count > 0
                && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountCreditPlusDTO> accountCreditPlusHoldList = accountDTO.AccountCreditPlusDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                          && (x.SubscriptionBillingScheduleId == -1
                                                                                                                             || (x.SubscriptionBillingScheduleId != -1
                                                                                                                                    && subscriptionBillingScheduleDTOList != null
                                                                                                                                    && subscriptionBillingScheduleDTOList.Any()
                                                                                                                                    && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                                   && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountCreditPlusHoldList != null)
                    {
                        isCardOnHold = true;
                    }
                }
                if (accountDTO != null
                && accountDTO.AccountGameDTOList != null
                && accountDTO.AccountGameDTOList.Count > 0
                && accountDTO.AccountGameDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountGameDTO> accountGameHoldList = accountDTO.AccountGameDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                        && (x.SubscriptionBillingScheduleId == -1
                                                                                                           || (x.SubscriptionBillingScheduleId != -1
                                                                                                                && subscriptionBillingScheduleDTOList != null
                                                                                                                && subscriptionBillingScheduleDTOList.Any()
                                                                                                                && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                   && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountGameHoldList != null)
                    {
                        if (!isCardOnHold)
                            isCardOnHold = true;
                    }
                }
                if (accountDTO != null
                && accountDTO.AccountDiscountDTOList != null
                && accountDTO.AccountDiscountDTOList.Count > 0
                && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountDiscountDTO> accountDiscountHoldList = accountDTO.AccountDiscountDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                   && (x.SubscriptionBillingScheduleId == -1
                                                                                                                      || (x.SubscriptionBillingScheduleId != -1
                                                                                                                            && subscriptionBillingScheduleDTOList != null
                                                                                                                            && subscriptionBillingScheduleDTOList.Any()
                                                                                                                            && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                            && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountDiscountHoldList != null)
                    {
                        if (!isCardOnHold)
                            isCardOnHold = true;
                    }

                }
                if (isCardOnHold)
                {
                    buttonOK.Text = "UNHOLD";
                }
                else
                {
                    buttonOK.Text = "HOLD";
                }
            }
            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.Debug("Starts-displayMessageLine(" + message + ")");
            textBoxMessageLine.Text = message;
            log.Debug("Ends-displayMessageLine(" + message + ")");
        }

        private void FormCardTasks_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-FormCardTasks_FormClosing()");
            Common.Devices.UnregisterCardReaders();
            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();

            if (keypad != null)
                keypad.Close();
        }

        private void FormCardTasks_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void radioButtonRealTicket_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-radioButtonRealTicket_CheckedChanged()");
            if (radioButtonRealTicket.Checked)
            {
                radioButtonRealTicket.BackColor = Color.SkyBlue;
                radioButtoneTicket.BackColor = Color.Transparent;
            }
            else
            {
                radioButtonRealTicket.BackColor = Color.Transparent;
                radioButtoneTicket.BackColor = Color.SkyBlue;
            }
            log.Debug("Ends-radioButtonRealTicket_CheckedChanged()");
        }

        private void txtTokensExchanged_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtTokensExchanged_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtTokensExchanged_Validating()");
            int tokens; double credits;

            displayMessageLine("");

            if (string.IsNullOrEmpty(txtTokensExchanged.Text.Trim()))
            {
                log.Debug("Ends-txtTokensExchanged_Validating() as txtTokensExchanged is not entered");
                return;
            }

            try
            {
                tokens = Convert.ToInt32(txtTokensExchanged.Text);
                if (tokens <= 0)
                {
                    displayMessageLine(MessageUtils.getMessage(70));
                    log.Info("Ends-txtTokensExchanged_Validating() as Tokens should be an Integer value (>0) ");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(70));
                log.Fatal("Ends-txtTokensExchanged_Validating() as Tokens should be an Integer value (>0) ");
                e.Cancel = true;
                return;
            }

            credits = tokens * ParafaitEnv.CREDITS_PER_TOKEN;
            txtCreditsAdded.Text = credits.ToString();
            log.Debug("Ends-txtTokensExchanged_Validating()");
        }

        private void txtTokensBought_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void txtTokensBought_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtTokensBought_KeyPress()");
            int tokens; double credits;

            displayMessageLine("");

            if (string.IsNullOrEmpty(txtTokensBought.Text.Trim()))
            {
                log.Debug("Ends-txtTokensBought_Validating() as txtTokensBought is not entered");
                return;
            }

            try
            {
                txtTokensBought.Text = Convert.ToInt32(double.Parse(txtTokensBought.Text.Trim())).ToString();
                tokens = Convert.ToInt32(txtTokensBought.Text);
                if (tokens <= 0)
                {
                    displayMessageLine(MessageUtils.getMessage(70));
                    log.Info("Ends-txtTokensBought_Validating() as Tokens entered should be an Integer value (>0)");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(70));
                log.Fatal("Ends-txtTokensBought_Validating() as Tokens entered should be an Integer value (>0)");
                e.Cancel = true;
                return;
            }

            credits = tokens * ParafaitEnv.CREDITS_PER_TOKEN;
            txtCreditsRequired.Text = credits.ToString();

            if (credits > CurrentCard.credits + CurrentCard.CreditPlusCardBalance)
            {
                displayMessageLine(MessageUtils.getMessage(49, credits.ToString(), (CurrentCard.credits + CurrentCard.CreditPlusCardBalance).ToString()));
                log.Info("Ends-txtTokensBought_Validating() as Insufficient Credits: Required: " + credits.ToString() + "; Available: " + (CurrentCard.credits + CurrentCard.CreditPlusCardBalance).ToString() + "");
                e.Cancel = true;
            }
            log.Debug("Ends-txtTokensBought_Validating()");
        }

        private void textBoxLoadTickets_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void btnChooseProduct_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnChooseProduct_Click()");
            // called from Load multiple Task when choose product is pressed
            // a new dynamic form is shown with NEW and RECHARGE products
            // NEW products are disabled if a NEW product is already picked

            Form ProductForm = new Form();
            ProductForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ProductForm.Size = new Size(500, 600);
            ProductForm.StartPosition = FormStartPosition.CenterScreen;
            ProductForm.Text = "Load Multiple - Choose Card Product";

            FlowLayoutPanel flpProducts = new FlowLayoutPanel();
            flpProducts.FlowDirection = FlowDirection.LeftToRight;
            flpProducts.WrapContents = true;
            flpProducts.Size = new Size(ProductForm.Width - 7, ProductForm.Height - 80);
            flpProducts.BorderStyle = BorderStyle.FixedSingle;
            ProductForm.BackColor = flpProducts.BackColor = Color.Gray;
            flpProducts.AutoScroll = true;
            ProductForm.Controls.Add(flpProducts);

            SqlCommand Productcmd = Utilities.getCommand();
            DateTime serverTime = Utilities.getServerTime();
            Productcmd.CommandText = @"select p.product_id, isnull(case when p.description='' then null else p.description end, p.product_name) as product_name, 
                                     pt.product_type  
                                     from products p
                                        left outer join ProductsDisplayGroup pdg
                                         on pdg.ProductId = p.product_id
                                         left outer join ProductDisplayGroupFormat pdf
                                           on pdf.Id = pdg.DisplayGroupId, product_type pt  
                                     where p.product_type_id = pt.product_type_id and 
                                     p.active_flag = 'Y' 
                                     and pt.product_type in ('NEW', 'CARDSALE', 'GAMETIME', 'RECHARGE') 
                                     and p.DisplayInPOS = 'Y' 
                                     and (p.POSTypeId = @Counter or @Counter = -1 or p.POSTypeId is null) 
                                     and (p.expiryDate >= getdate() or p.expiryDate is null) 
                                     and (p.StartDate <= getdate() or p.StartDate is null) 
                                     and not exists (select 1 
                                                     from POSProductExclusions e  
                                                     where e.POSMachineId = @POSMachine 
                                                     and e.ProductDisplayGroupFormatId = pdf.Id) 
                                     and not exists (select 1 
                                                       from UserRoleDisplayGroupExclusions urdge , 
                                                            users u
                                                      where urdge.ProductDisplayGroupId = pdf.Id
                                                        and urdge.role_id = u.role_id
                                                        and u.loginId = @loginId )
                                     and (not exists (select 1 
                                                     from ProductCalendar pc 
                                                     where pc.product_id = p.product_id) 
                                           or exists (select 1 from  
                                                        (select top 1 date, day, -- select in the order of specific date, day of month, weekday, every day. if there are multiple slots on same day, take the one which is in current hour 
                                                                case when @nowHour between isnull(FromTime, @nowHour) and isnull(case ToTime when 0 then 24 else ToTime end, @nowHour) then 0 else 1 end sort,  
                                                                FromTime, ToTime, ShowHide  
                                                        from ProductCalendar pc 
                                                         where pc.product_id = p.product_id 
                                                         and (Date = @today -- specific day 
                                                            or Day = @DayNumber -- day number 1001 - 1031 
                                                            or Day = @weekDay -- week day 0-6 
                                                            or Day = -1) -- everyday 
                                                         order by 1 desc, 2 desc, 3) inView  
                                                         where (ShowHide = 'Y'  
                                                                and (@nowHour >= isnull(FromTime, 0) and @nowHour <= case isnull(ToTime, 0) when 0 then 24 else ToTime end)) 
                                                            or (ShowHide = 'N' 
                                                                and (@nowHour < isnull(FromTime, 0) or @nowHour > case isnull(ToTime, 0) when 0 then 24 else ToTime end)))) 
                                                order by product_type, sort_order";
            Productcmd.Parameters.AddWithValue("@POSMachine", Utilities.ParafaitEnv.POSMachineId);
            Productcmd.Parameters.AddWithValue("@Counter", Utilities.ParafaitEnv.POSTypeId);
            Productcmd.Parameters.AddWithValue("@today", serverTime.Date);
            Productcmd.Parameters.AddWithValue("@nowHour", serverTime.Hour + serverTime.Minute / 100.0);
            Productcmd.Parameters.AddWithValue("@DayNumber", serverTime.Day + 1000); // day of month stored as 1000 + day of month
            Productcmd.Parameters.AddWithValue("@loginId", Utilities.ParafaitEnv.LoginID);

            int dayofweek = -1;
            switch (serverTime.DayOfWeek)
            {
                case DayOfWeek.Sunday: dayofweek = 0; break;
                case DayOfWeek.Monday: dayofweek = 1; break;
                case DayOfWeek.Tuesday: dayofweek = 2; break;
                case DayOfWeek.Wednesday: dayofweek = 3; break;
                case DayOfWeek.Thursday: dayofweek = 4; break;
                case DayOfWeek.Friday: dayofweek = 5; break;
                case DayOfWeek.Saturday: dayofweek = 6; break;
                default: break;
            }
            Productcmd.Parameters.AddWithValue("@weekDay", dayofweek);

            DataTable ProductTbl = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(Productcmd);
            da.Fill(ProductTbl);

            bool newProductFound = false;
            for (int i = 0; i < dgvProductsAdded.Rows.Count; i++)
            {
                if (dgvProductsAdded.Rows[i].Cells["ProductType"].Value.ToString() == "NEW")
                {
                    newProductFound = true;
                    break;
                }
            }

            for (int i = 0; i < ProductTbl.Rows.Count; i++)
            {
                Button ProductButton = new Button();
                ProductButton.Click += new EventHandler(ProductButton_Click);
                ProductButton.Name = "ProductButton" + i.ToString();
                ProductButton.Text = ProductTbl.Rows[i]["product_name"].ToString();
                ProductButton.Tag = ProductTbl.Rows[i]["product_id"];
                ProductButton.Font = new Font("arial", 10);
                ProductButton.ForeColor = Color.White;
                ProductButton.Size = new Size(150, 80);
                ProductButton.FlatStyle = FlatStyle.Flat;
                ProductButton.FlatAppearance.BorderColor = Color.White;

                if (ProductTbl.Rows[i]["product_type"].ToString() == "CARDSALE")
                    ProductButton.BackColor = Color.MidnightBlue;
                else if (ProductTbl.Rows[i]["product_type"].ToString() == "RECHARGE")
                    ProductButton.BackColor = Color.Olive;
                else if (ProductTbl.Rows[i]["product_type"].ToString() == "GAMETIME")
                    ProductButton.BackColor = Color.Tan;
                else // NEW
                {
                    ProductButton.BackColor = Color.SteelBlue;
                    if (newProductFound)
                        ProductButton.Enabled = false;
                }

                flpProducts.Controls.Add(ProductButton);
            }
            flpProducts.Refresh();

            Button CancelButton = new Button();
            CancelButton.Click += new EventHandler(CancelButton_Click);
            CancelButton.Name = "CancelButton";
            CancelButton.Text = "Cancel";
            CancelButton.Font = new Font("arial", 10, FontStyle.Bold);
            CancelButton.ForeColor = Color.Black;
            CancelButton.Size = new Size(100, 36);
            CancelButton.Location = new Point(ProductForm.Width / 2 - CancelButton.Width / 2, flpProducts.Bottom + 2);
            CancelButton.BackColor = Color.White;
            ProductForm.Controls.Add(CancelButton);

            ProductForm.CancelButton = CancelButton;

            // when a product is selected, display its details on form grid
            if (ProductForm.ShowDialog() == DialogResult.OK)
            {
                int productId = Utilities.ParafaitEnv.LoadMultipleProductPicked;
                SqlCommand cmd = Utilities.getCommand();
                //Added on 08-Jan-2016- Modified the Query to add ManagerApprovalRequired field//
                cmd.CommandText = "select p.product_name, p.price, " +
                                    "p.credits, p.bonus, p.courtesy, " +
                                    "pt.product_type, p.time, p.ManagerApprovalRequired " +
                                    "from products p, product_type pt " +
                                    "where product_id = @product_id " +
                                    "and p.product_type_id = pt.product_type_id";
                cmd.Parameters.AddWithValue("@product_id", productId);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable DT = new DataTable();
                dap.Fill(DT);
                //Begin Modification 08-Jan-2016 to check for Manager Approval//
                if (DT.Rows[0]["ManagerApprovalRequired"].ToString() == "Y" && ParafaitEnv.Manager_Flag == "N")
                {
                    if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(219));
                        log.Info("btnChooseProduct_Click() as Manager Approval required to use this Product");
                        return;
                    }
                }
                //End Modification 08-Jan-2016 to check for Manager Approval//
                dgvProductsAdded.Rows.Add(new object[] { productId,
                                                         DT.Rows[0]["product_name"],
                                                         DT.Rows[0]["price"],
                                                         DT.Rows[0]["credits"],
                                                         DT.Rows[0]["Bonus"],
                                                         DT.Rows[0]["courtesy"],
                                                         DT.Rows[0]["time"],
                                                         DT.Rows[0]["product_type"] });
            }
            log.Debug("Ends-btnChooseProduct_Click() ");
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-CancelButton_Click() ");
            ((Button)sender).FindForm().DialogResult = DialogResult.Cancel;
            log.Debug("Ends-CancelButton_Click() ");
        }

        void ProductButton_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-ProductButton_Click() ");
            Form f = ((Button)sender).FindForm();
            f.DialogResult = DialogResult.OK;
            Utilities.ParafaitEnv.LoadMultipleProductPicked = Convert.ToInt32(((Button)sender).Tag);
            f.Close();
            log.Debug("Ends-ProductButton_Click() ");
        }

        private void btnRemoveProduct_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRemoveProduct_Click() ");
            if (dgvProductsAdded.CurrentRow != null)
                dgvProductsAdded.Rows.RemoveAt(dgvProductsAdded.CurrentRow.Index);

            log.Debug("Ends-btnRemoveProduct_Click() ");
        }

        private void textBoxTransferCardNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
            else
                e.Handled = true;
        }

        private void textBoxHoldEntitlementNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsControl(e.KeyChar))
                e.KeyChar = char.ToUpper(e.KeyChar);
            else
                e.Handled = true;
        }

        private void rbNever_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNever.Checked)
                dtpDiscountExpiryDate.Enabled = false;
            else
                dtpDiscountExpiryDate.Enabled = true;
        }

        private void btnAddDiscount_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAddDiscount_Click() ");
            displayMessageLine("");
            int discountId = Convert.ToInt32(cmbDiscount.SelectedValue);
            if (discountId != -1)
            {
                if (cardDiscountsDTOListBS.DataSource != null && cardDiscountsDTOListBS.DataSource is SortableBindingList<CardDiscountsDTO>)
                {
                    SortableBindingList<CardDiscountsDTO> cardDiscountsDTOList = cardDiscountsDTOListBS.DataSource as SortableBindingList<CardDiscountsDTO>;
                    bool found = false;
                    foreach (var cardDiscountsDTO in cardDiscountsDTOList)
                    {
                        if (cardDiscountsDTO.DiscountId == discountId)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found == false)
                    {
                        CardDiscountsDTO cardDiscountsDTO = new CardDiscountsDTO();
                        cardDiscountsDTO.DiscountId = discountId;
                        cardDiscountsDTO.CardId = CurrentCard.card_id;
                        if (rbExpires.Checked)
                        {
                            cardDiscountsDTO.ExpiryDate = dtpDiscountExpiryDate.Value;
                        }
                        cardDiscountsDTOList.Add(cardDiscountsDTO);
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(78));
                    }
                }

            }
            log.Debug("Ends-btnAddDiscount_Click() ");
        }

        private void txtLoyaltyRedeemPoints_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtLoyaltyRedeemPoints_Validating() ");
            displayMessageLine("");
            try
            {
                double redeemPoints = Convert.ToDouble(txtLoyaltyRedeemPoints.Text);
                if (redeemPoints > (CurrentCard.loyalty_points + CurrentCard.RedeemableCreditPlusLoyaltyPoints))
                {
                    displayMessageLine(MessageUtils.getMessage(79));
                    log.Info("Ends-txtLoyaltyRedeemPoints_Validating() as Redemption Points cannot be more than available points");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(80));
                log.Info("Ends-txtLoyaltyRedeemPoints_Validating() as Enter valid value for Redemption Points");
                e.Cancel = true;
                return;
            }

            populateRedemptionRule();
            log.Debug("Ends-txtLoyaltyRedeemPoints_Validating()");
        }
        private void txtVirtualLoyaltyRedeemPoints_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtLoyaltyRedeemPoints_Validating() ");
            displayMessageLine("");
            try
            {
                double redeemPoints = Convert.ToDouble(txtVirtualRedeemPoints.Text);
                if (redeemPoints > (CurrentCard.CreditPlusVirtualPoints))
                {
                    displayMessageLine(MessageUtils.getMessage(79));
                    log.Info("Ends-txtLoyaltyRedeemPoints_Validating() as Redemption Points cannot be more than available points");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(80));
                log.Info("Ends-txtLoyaltyRedeemPoints_Validating() as Enter valid value for Redemption Points");
                e.Cancel = true;
                return;
            }

            populateRedemptionRule(true);
            log.Debug("Ends-txtLoyaltyRedeemPoints_Validating()");
        }

        private void btnGetRedemptionValues_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGetRedemptionValues_Click()");
            populateRedemptionRule();
            log.Debug("Ends-btnGetRedemptionValues_Click()");
        }
        private void btnVirtualGetRedemptionValues_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGetRedemptionValues_Click()");
            populateRedemptionRule(true);
            log.Debug("Ends-btnGetRedemptionValues_Click()");
        }

        private void dgvLoyaltyRedemption_SelectionChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dgvLoyaltyRedemption_SelectionChanged()");
            for (int i = 0; i < dgvLoyaltyRedemption.Rows.Count; i++)
            {
                dgvLoyaltyRedemption["Selected", i].Value = "N";
            }
            try
            {
                if (dgvLoyaltyRedemption.SelectedRows.Count > 0)
                    dgvLoyaltyRedemption.SelectedRows[0].Cells["Selected"].Value = "Y";
            }
            catch
            {
                log.Debug("Ends-dgvLoyaltyRedemption_SelectionChanged() due to exception in dgvLoyaltyRedemption SelectedRows value");
            }

            log.Debug("Ends-dgvLoyaltyRedemption_SelectionChanged()");
        }
        private void dgvVirtualLoyaltyRedemption_SelectionChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dgvVirtualLoyaltyRedemption_SelectionChanged()");
            for (int i = 0; i < dgvVirtualPointRedemption.Rows.Count; i++)
            {
                dgvVirtualPointRedemption["SelectedVirtualPoint", i].Value = "N";
            }
            try
            {
                if (dgvVirtualPointRedemption.SelectedRows.Count > 0)
                    dgvVirtualPointRedemption.SelectedRows[0].Cells["SelectedVirtualPoint"].Value = "Y";
            }
            catch
            {
                log.Debug("Ends-dgvVirtualLoyaltyRedemption_SelectionChanged() due to exception in dgvVirtualLoyaltyRedemption SelectedRows value");
            }

            log.Debug("Ends-dgvVirtualLoyaltyRedemption_SelectionChanged()");
        }

        private void btnClearPricing_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClearPricing_Click()");
            if (_Parameter != null)
            {
                Utilities.executeNonQuery(@"delete from tasks 
                                        where task_type_id = (select ty.task_type_id 
                                                                  from task_type ty 
                                                                  where ty.task_type = 'SPECIALPRICING')
                                        and value_loaded = @pricingId 
                                        and Attribute1 = @trxId",
                                            new SqlParameter("@pricingId", Utilities.ParafaitEnv.specialPricingId),
                                            new SqlParameter("@trxId", (_Parameter as Transaction).Trx_id));
            }

            Utilities.ParafaitEnv.specialPricingId = -1;
            Utilities.ParafaitEnv.specialPricingRemarks = "";
            PoleDisplay.writeLines("Normal Pricing", "");

            this.DialogResult = DialogResult.OK;
            this.Close();
            log.Debug("Ends-btnClearPricing_Click()");
        }

        private void txtTicketsToRedeem_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtTicketsToRedeem_Validating()");
            displayMessageLine("");
            int redeemTickets;

            if (string.IsNullOrEmpty(txtTicketsToRedeem.Text.Trim()))
            {
                log.Debug("Ends-txtTicketsToRedeem_Validating() as txtTicketsToRedeem is not entered");
                txtBonusEligible.Text = "0";
                return;
            }

            try
            {
                redeemTickets = Convert.ToInt32(txtTicketsToRedeem.Text);
                if (redeemTickets > CurrentCard.ticket_count + CurrentCard.CreditPlusTickets)
                {
                    displayMessageLine(MessageUtils.getMessage(50, POSStatic.TicketTermVariant, POSStatic.TicketTermVariant));
                    log.Info("Ends-txtTicketsToRedeem_Validating() as Redemption " + POSStatic.TicketTermVariant + " cannot be more than available Tickets");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(473, POSStatic.TicketTermVariant));
                log.Fatal("Ends-txtTicketsToRedeem_Validating() due to exception Enter a valid value for " + POSStatic.TicketTermVariant + " to Redeem");
                e.Cancel = true;
                return;
            }

            UpdateTicketsToRedeem();
            log.Debug("Ends-txtTicketsToRedeem_Validating()");
        }

        private void btnRefreshBonus_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefreshBonus_Click()");
            try
            {
                UpdateTicketsToRedeem();
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(473, POSStatic.TicketTermVariant));
                log.Fatal("Ends-btnRefreshBonus_Click() as Enter valid value for " + POSStatic.TicketTermVariant + " to Redeem");
            }
            log.Debug("Ends-btnRefreshBonus_Click()");
        }

        private void txtRefundAmount_TextChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-txtRefundAmount_TextChanged()");
            if (txtRefundAmount.Enabled)
            {
                txtRefundAmount.Tag = txtRefundAmount.Text;
                calculateRefundAmounts();
            }
            log.Debug("Ends-txtRefundAmount_TextChanged()");
        }

        private void btnChangeSiteCode_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnChangeSiteCode_Click()");
            int siteCode = 0;
            bool response;
            displayMessageLine("");
            try
            {
                siteCode = Convert.ToInt32(cbSiteCode.SelectedValue);
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(81));
                log.Fatal("Ends-btnChangeSiteCode_Click() due to exception Invalid site code. Should be an Integer.");
                return;
            }

            try
            {
                response = CurrentCard.setChildSiteCode(ref purseDataBuffer, siteCode, ref ReturnMessage);
            }
            catch
            {
                response = false;
                log.Fatal("Ends-btnChangeSiteCode_Click() due to exception in setChildSiteCode.");
            }

            if (response)
            {
                string message = "";
                if (TaskProcs.SetChildSiteCode(CurrentCard, siteCode, ref message))
                {
                    GetSiteCode(CurrentCard.ReaderDevice);
                    this.Refresh();
                    displayMessageLine(MessageUtils.getMessage(82));
                    log.Info("btnChangeSiteCode_Click() - Successfully changed the site code");
                }
                else
                {
                    displayMessageLine(message);
                    log.Info("btnChangeSiteCode_Click() -Unable to change the site code error " + message);
                }
            }
            else
            {
                displayMessageLine(MessageUtils.getMessage(83));
                log.Error("Ends-btnChangeSiteCode_Click() - Failed to change the site code, please retry");
                return;
            }
            log.Debug("Ends-btnChangeSiteCode_Click()");
        }

        #region Get MiFare Gameplay details

        private const string COMPLETE = "Completed";
        private const string ERROR_UPLOADING = "Error Uploading";
        private const string INVALID_DATA = "Invalid Value";

        int gamePlayCount = 0;

        int MAX_PROGRESS_VAL = 100;

        public class GamePlaysClass
        {
            public int siteCode;
            public int machineId;
            public double startingBalance;
            public double endingBalance;
        }

        string gamePlayReturned = "";
        GamePlaysClass[] currCardGamePlays;
        int siteCode = 0;
        int machineId = 0;
        double startingBalance = 0;
        double endingBalance = 0;

        private void btnValidate_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnValidate_Click()");
            updateProgressBar(-1);

            try
            {
                displayMessageLine(MessageUtils.getMessage(84));
                log.Info("btnValidate_Click() - Validating data. Please wait...");
                updateProgressBar(1);

                if (CurrentCard.CardNumber != null)
                {
                    if (CurrentCard.checkCardExists())
                    {
                        displayCardDetails();
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(85));
                        log.Info("Ends-btnValidate_Click() as Card read failed");
                        clearCardDetails();
                        return;
                    }
                    updateProgressBar(3);
                }
                updateProgressBar(MAX_PROGRESS_VAL);
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-btnValidate_Click() due to exception " + ex.Message);
                clearCardDetails();
                return;
            }
            log.Debug("Ends-btnValidate_Click()");
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnUpload_Click()");
            int response;
            updateProgressBar(-1);

            try
            {
                if (CurrentCard != null)
                {
                    if (CurrentCard.checkCardExists())
                    {
                        if (gamePlayCount <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(86));
                            log.Info("Ends-btnUpload_Click() as No data to upload ");
                            btnValidate.Enabled = false;
                            btnUpload.Enabled = false;
                            return;
                        }
                        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(474), "Confirm upload", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            displayMessageLine(MessageUtils.getMessage(84));
                            log.Info("btnUpload_Click() - Validating data. Please wait...");
                            updateProgressBar(1);

                            displayMessageLine(MessageUtils.getMessage(88));
                            log.Info("btnUpload_Click() - Uploading data. Please do not remove card");
                            this.btnUpload.Enabled = false;
                            this.btnValidate.Enabled = false;

                            int uploadedDataCount = 0;
                            string machineAddr = "";
                            for (int i = 0; i < gamePlayCount; i++)
                            {
                                if (CurrentCard.checkCardExists())
                                {
                                    if (dgvCardDetails.Rows[i].Cells["dcComments"].Value.ToString() == "Valid")
                                    {
                                        log.Info("btnUpload_Click() - has Valid data");
                                        siteCode = Convert.ToInt32(dgvCardDetails.Rows[i].Cells["dcSiteId"].Value);
                                        machineId = Convert.ToInt32(dgvCardDetails.Rows[i].Cells["dcMachineAddress"].Value);
                                        machineAddr = dgvCardDetails.Rows[i].Cells["dcSiteId"].Value.ToString() + dgvCardDetails.Rows[i].Cells["dcMachineAddress"].Value.ToString();
                                        startingBalance = long.Parse(dgvCardDetails.Rows[i].Cells["dcStartBalance"].Value.ToString());
                                        endingBalance = long.Parse(dgvCardDetails.Rows[i].Cells["dcEndBalance"].Value.ToString());

                                        response = createGamePlay(i, ref siteCode, ref machineAddr, ref startingBalance, ref endingBalance);

                                        if (response > 0)
                                        {
                                            dgvCardDetails.Rows[i].Cells["dcStatus"].Value = COMPLETE;
                                            log.Info("btnUpload_Click() - createGamePlay is Completed");
                                            uploadedDataCount++;
                                        }
                                        else
                                        {
                                            log.Info("btnUpload_Click() - createGamePlay has ERROR_UPLOADING");
                                            dgvCardDetails.Rows[i].Cells["dcStatus"].Value = ERROR_UPLOADING;
                                        }
                                    }
                                    else
                                    {
                                        log.Info("btnUpload_Click() - has InValid data");
                                        dgvCardDetails.Rows[i].Cells["dcStatus"].Value = INVALID_DATA;
                                    }
                                }
                                else
                                {
                                    displayMessageLine(MessageUtils.getMessage(89));
                                    log.Info("Ends-btnUpload_Click() as Card reading failed");
                                    return;
                                }
                                updateProgressBar(3);
                            }
                            displayMessageLine(uploadedDataCount + " gameplay records added to the system.");
                            log.Info("btnUpload_Click() " + uploadedDataCount + " gameplay records added to the system.");
                            updateProgressBar(MAX_PROGRESS_VAL);
                        }
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(89));
                        log.Info("Ends-btnUpload_Click() as Card reading failed");
                        clearCardDetails();
                        return;
                    }
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(90));
                    log.Info("Ends-btnUpload_Click() as a Card needs to be tapped");
                    clearCardDetails();
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-btnUpload_Click() due to exception" + ex.Message);
                clearCardDetails();
                return;
            }
            log.Debug("Ends-btnUpload_Click()");
        }

        private int createGamePlay(int gamePlayNumber, ref int siteCode, ref string machineAddr, ref double startingBalance, ref double endingBalance)
        {
            log.Debug("Starts-createGamePlay(" + gamePlayNumber + "sitecode,machineAddr,startingBalance,endingBalance)");
            SqlCommand insertCMD = Utilities.getCommand();
            int response = 0;
            try
            {
                insertCMD.CommandText = @"Insert into gameplay (machine_id, card_id, credits, courtesy, bonus, time, guid, play_date)
                                      (select top 1 machine_id, card_id, @startingBalance - @endingBalance, 0, 0, 0, newid(), getdate() 
                                        from cards c, machines m where card_number = @cardNumber
                                        and valid_flag = 'Y' 
                                        and m.machine_address = @machineAddress
                                        ) ";
                insertCMD.Parameters.AddWithValue("@machineAddress", machineAddr);
                insertCMD.Parameters.AddWithValue("@cardNumber", "FFFFFFFFFF");
                insertCMD.Parameters.AddWithValue("@startingBalance", startingBalance);
                insertCMD.Parameters.AddWithValue("@endingBalance", endingBalance);
                response = insertCMD.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-createGamePlay(" + gamePlayNumber + "sitecode,machineAddr,startingBalance,endingBalance) due to exception " + ex.Message);
                clearCardDetails();
                return 0;
            }
            log.Debug("Ends-createGamePlay(" + gamePlayNumber + "sitecode,machineAddr,startingBalance,endingBalance)");
            return response;
        }
        private void btnExit_click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnExit_click()");
            this.Close();
            log.Debug("Ends-btnExit_click()");
        }
        private void displayCardDetails()
        {
            log.Debug("Starts-displayCardDetails()");
            updateProgressBar(-1);
            clearCardDetails();

            try
            {
                displayMessageLine(MessageUtils.getMessage(91));
                log.Info("displayCardDetails() - Loading card details. Please wait...");
                updateProgressBar(1);

                if (CurrentCard.CardNumber != null)
                {
                    clearCardDetails();

                    CardNumber = CurrentCard.CardNumber;
                    txtCardNumberMiFare.Text = CardNumber;
                    gamePlayCount = CurrentCard.getPlayCount();
                    txtNoOfGamePlays.Text = gamePlayCount.ToString();

                    if (CurrentCard.checkCardExists())
                    {
                        if (gamePlayCount <= 0)
                        {
                            displayMessageLine(MessageUtils.getMessage(93));
                            log.Info("Ends-displayCardDetails() as There is no data to be displayed");
                            return;
                        }

                        currCardGamePlays = new GamePlaysClass[gamePlayCount];
                        SqlCommand cmd = Utilities.getCommand();
                        string machineAddress = "";

                        for (int i = 0; i < gamePlayCount; i++)
                        {
                            this.dgvCardDetails.Rows.Add();
                            GamePlaysClass thisGameDetails = new GamePlaysClass();

                            gamePlayReturned = CurrentCard.getPlayDetails(i, ref siteCode, ref machineId, ref startingBalance, ref endingBalance);

                            if (gamePlayReturned == "")
                            {
                                thisGameDetails.siteCode = siteCode;
                                thisGameDetails.machineId = machineId;
                                thisGameDetails.startingBalance = startingBalance;
                                thisGameDetails.endingBalance = endingBalance;
                                currCardGamePlays[i] = thisGameDetails;

                                if (siteCode >= 10)
                                    this.dgvCardDetails.Rows[i].Cells[0].Value = siteCode.ToString();
                                else
                                    this.dgvCardDetails.Rows[i].Cells[0].Value = "0" + siteCode.ToString();
                                if (machineId >= 10)
                                    this.dgvCardDetails.Rows[i].Cells[1].Value = machineId.ToString();
                                else
                                    this.dgvCardDetails.Rows[i].Cells[1].Value = "0" + machineId.ToString();
                                this.dgvCardDetails.Rows[i].Cells[2].Value = startingBalance.ToString();
                                this.dgvCardDetails.Rows[i].Cells[3].Value = endingBalance.ToString();

                                cmd.Parameters.Clear();
                                cmd.CommandText = @"Select * from lookupView where LookupValue = @lookupvalue";
                                cmd.Parameters.AddWithValue("@lookupvalue", siteCode);
                                DataTable dt = new DataTable();
                                try
                                {
                                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                                    da.Fill(dt);
                                    if (dt.Rows.Count > 0)
                                    {
                                        log.Info("displayCardDetails() - Valid Site");
                                        machineAddress = this.dgvCardDetails.Rows[i].Cells[0].Value.ToString() + this.dgvCardDetails.Rows[i].Cells[1].Value.ToString();
                                        cmd.Parameters.Clear();
                                        cmd.CommandText = @"Select * from machines where machine_address = @machineAddress";
                                        cmd.Parameters.AddWithValue("@machineAddress", machineAddress);

                                        try
                                        {
                                            SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                                            dt.Clear();
                                            da1.Fill(dt);

                                            if (dt.Rows.Count > 0)
                                            {
                                                this.dgvCardDetails.Rows[i].Cells[4].Value = "Valid";
                                                log.Info("displayCardDetails() - Valid machines detail");
                                            }
                                            else
                                            {
                                                this.dgvCardDetails.Rows[i].Cells[4].Value = "Invalid machine ID";
                                                log.Info("displayCardDetails() - Invalid machine ID");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            displayMessageLine(ex.Message);
                                            log.Fatal("Ends-displayCardDetails() due to exception " + ex.Message);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        this.dgvCardDetails.Rows[i].Cells[4].Value = "Invalid Site";
                                        log.Info("displayCardDetails() - Invalid Site");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    displayMessageLine(ex.Message);
                                    log.Fatal("Ends-displayCardDetails() due to exception " + ex.Message);
                                    clearCardDetails();
                                    return;
                                }
                            }
                            else
                            {
                                displayMessageLine(MessageUtils.getMessage(89));
                                log.Info("Ends-displayCardDetails() as Card reading failed");
                                clearCardDetails();
                                return;
                            }
                            updateProgressBar(3);
                        }

                        displayMessageLine(MessageUtils.getMessage(92));
                        log.Info("displayCardDetails() - Data load complete...");
                        btnValidate.Enabled = true;
                        btnUpload.Enabled = true;
                        updateProgressBar(MAX_PROGRESS_VAL);
                    }
                    else
                    {
                        displayMessageLine(MessageUtils.getMessage(89));
                        log.Info("Ends-displayCardDetails() as Card reading failed");
                        clearCardDetails();
                        return;
                    }
                }
                else
                {
                    displayMessageLine(MessageUtils.getMessage(90));
                    log.Info("Ends-displayCardDetails() as no Cards are tapped ");
                    clearCardDetails();
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Fatal("Ends-displayCardDetails() due to excception " + ex.Message);
                clearCardDetails();
                return;
            }
            log.Debug("Ends-displayCardDetails()");
        }

        private void updateProgressBar(int value)
        {
            log.Debug("Starts-updateProgressBar(" + value + ")");
            if (value == -1)
                progressBar.Value = 0;
            else if (value == MAX_PROGRESS_VAL)
                progressBar.Value = value;
            else if (progressBar.Value < 90)
                progressBar.Value += value;
            this.Refresh();
            log.Debug("Ends-updateProgressBar(" + value + ")");
        }

        void clearCardDetails()
        {
            log.Debug("Starts-clearCardDetails()");
            dgvCardDetails.Rows.Clear();
            txtNoOfGamePlays.Text = "";
            btnValidate.Enabled = false;
            btnUpload.Enabled = false;
            log.Debug("Ends-clearCardDetails()");
        }
        #endregion

        private void FormCardTasks_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.Debug("Starts-FormCardTasks_KeyPress()");
            try
            {
                (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now; //ServerDateTime.Now;
            }
            catch { }

            if (this.ActiveControl.Name != "txtRemarks"
                && this.ActiveControl.Name != "textBoxTransferCardNumber"
                && this.ActiveControl.Name != "txtLoadBonusManualCardNumber"
                && this.ActiveControl.Name != "txtFromCardSerialNumber"
               && this.ActiveControl.Name != "txtToCardSerialNumber"
                && this.ActiveControl.Name != "textBoxHoldEntitlementNumber")
            {
                if (char.IsNumber(e.KeyChar) || e.KeyChar == POSStatic.decimalChar)
                {
                    e.Handled = true;
                    showNumberPadForm(e.KeyChar);
                }
                else if (!char.IsControl(e.KeyChar))
                {
                    showNumberPadForm('-');
                }
            }
            log.Debug("Ends-FormCardTasks_KeyPress()");
        }

        void showNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showNumberPadForm()");
            double varAmount = NumberPadForm.ShowNumberPadForm("Enter Amount", firstKey, Utilities);
            if (varAmount >= 0)
            {
                TextBox txtBox = null;
                try
                {
                    if (this.ActiveControl.GetType().ToString().ToLower().Contains("textbox"))
                        txtBox = this.ActiveControl as TextBox;
                }
                catch { }

                if (txtBox != null && !txtBox.ReadOnly)
                {
                    txtBox.Text = varAmount.ToString();
                    this.ValidateChildren();
                }
            }
            log.Debug("Ends-showNumberPadForm()");
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurrentTextBox == null && txtRemarks.Equals(CurrentAlphanumericTextBox))
            {
                showAlphaNumberPadForm('-');
            }
            else
            {
                if (CurrentTextBox != null)
                    try
                    {
                        this.ActiveControl = CurrentTextBox;
                    }
                    catch { }

                showNumberPadForm('-');
            }
            log.LogMethodExit();
        }

        private void txtTokensExchanged_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtTokensBought_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void textBoxLoadTickets_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtRefundAmount_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtLoadBonus_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtLoyaltyRedeemPoints_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }
        private void txtVirtualLoyaltyRedeemPoints_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtTicketsToRedeem_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void textBoxTransferCardNumber_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = null;
        }
        private void textBoxHoldEntitlementNumber_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = null;
        }

        private void txtRemarks_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = null;
            CurrentAlphanumericTextBox = txtRemarks;
        }

        private void chkMakeCardNewOnFullRefund_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-chkMakeCardNewOnFullRefund_CheckedChanged()");
            if (dgvRefundCardData.Rows.Count > 0)
                RefundCard();

            log.Debug("Ends-chkMakeCardNewOnFullRefund_CheckedChanged()");
        }

        private void buttonOK_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.normal2;
        }

        private void buttonOK_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.pressed2;
        }

        private void txtRefundGameAmount_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void txtRefundTime_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = (sender) as TextBox;
        }

        private void dgvRefundBalanceGames_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gameRefundAmount = 0;
            previousLineId = "";
            previousTransactionId = "";
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.Info("Ends-dgvRefundBalanceGames_CellContentClick() as e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }

            if (e.ColumnIndex == 0)
            {
                int totalGames = 0;
                object trxId = dgvRefundBalanceGames["TrxId", e.RowIndex].Value;
                if (dgvRefundBalanceGames[0, e.RowIndex].Value == null || 0.Equals(dgvRefundBalanceGames[0, e.RowIndex].Value))
                {
                    foreach (DataGridViewRow dr in dgvRefundBalanceGames.Rows)
                    {
                        if (dr.Cells["TrxId"].Value.Equals(trxId))
                        {
                            dr.Cells[0].Value = 1;
                            totalGames += Convert.ToInt32(dr.Cells["Balance Games"].Value);
                            //Begin: Added the below code to calculate the total transaction amount//
                            SqlCommand cmdRefundAmount = Utilities.getCommand();
                            cmdRefundAmount.CommandText = "select amount from trx_lines where TrxId = @TrxId and LineId = @LineId";

                            for (int l = 0; l < dgvRefundBalanceGames.Rows.Count; l++)
                            {
                                if (previousLineId == dr.Cells["TrxLineId"].Value.ToString() && previousTransactionId == dr.Cells["TrxId"].Value.ToString())
                                    continue;
                                cmdRefundAmount.Parameters.Clear();
                                cmdRefundAmount.Parameters.AddWithValue("@TrxId", dr.Cells["TrxId"].Value);
                                cmdRefundAmount.Parameters.AddWithValue("@LineId", dr.Cells["TrxLineId"].Value);
                                previousTransactionId = dr.Cells["TrxId"].Value.ToString();
                                previousLineId = dr.Cells["TrxLineId"].Value.ToString();
                                object o = cmdRefundAmount.ExecuteScalar();
                                gameRefundAmount += Math.Round(Convert.ToDouble(o));
                            }
                            txtRefundGameAmount.Text = gameRefundAmount.ToString();
                            gameRefundAmount = Math.Round(gameRefundAmount, 4, MidpointRounding.AwayFromZero);
                            txtRefundGameAmount.Text = (gameRefundAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                            //End: till here Added the below code to calculate the total transaction amount
                        }
                        else
                            dr.Cells[0].Value = 0;
                    }
                    //Begin: to enable and disable controls based on selection
                    txtRefundGameAmount.Tag = trxId;
                    grpRefundGames.Enabled = true;
                    txtRefundGameAmount.Enabled = true;
                    cmbLoadGameAttributes.Enabled = true;
                    grpRefundCredits.Enabled = false;
                    cmbLoadTimeAttributes.Enabled = false;//Added to disable combo, if games were selected to refund//
                    txtRefundTime.Enabled = false;//Added to disable textbox for time refund, if games were selected to refund//
                    grpRefundTime.Enabled = false;//Added to disable group box for time, if games were selected to refund//
                    //End: to enable and disable controls based on selection
                }
                else
                {
                    foreach (DataGridViewRow dr in dgvRefundBalanceGames.Rows)
                    {
                        if (dr.Cells["TrxId"].Value.Equals(trxId))
                        {
                            dr.Cells[0].Value = 0;
                        }
                    }
                    //Begin: to enable and disable controls based on selection
                    totalGames = 0;
                    txtRefundGameAmount.Clear();
                    txtRefundGameAmount.Tag = null;
                    txtRefundGameAmount.Clear();
                    grpRefundCredits.Enabled = true;
                    grpRefundTime.Enabled = true;
                    txtRefundTime.Enabled = true;
                    cmbLoadTimeAttributes.Enabled = true;//Added to disable combo, if games were selected to refund//
                    txtRefundGameAmount.Enabled = false;
                    cmbLoadGameAttributes.Enabled = false;
                    //End: to enable and disable controls based on selection
                }
                lblRefundGameCount.Text = totalGames.ToString();
            }
            log.Debug("Ends-dgvRefundBalanceGames_CellContentClick()");
        }

        private void btnAlphanumericCancelndarTransferCard_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAlphanumericCancelndarTransferCard_Click()");
            CurrentAlphanumericTextBox = textBoxTransferCardNumber;
            showAlphaNumberPadForm('-');
            log.Debug("Ends-btnAlphanumericCancelndarTransferCard_Click()");
        }

        private void btnAlphanumericCancelndarHoldEntitlement_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAlphanumericCancelndarHoldEntitlement_Click()");
            CurrentAlphanumericTextBox = textBoxHoldEntitlementNumber;
            showAlphaNumberPadForm('-');
            log.Debug("Ends-btnAlphanumericCancelndarHoldEntitlement_Click()");
        }

        AlphaNumericKeyPad keypad;
        void showAlphaNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showAlphaNumberPadForm()");
            if (CurrentAlphanumericTextBox != null)
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, CurrentAlphanumericTextBox);
                    if (this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60 + keypad.Height < Screen.PrimaryScreen.WorkingArea.Height)
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60);
                    else
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
            log.Debug("Ends-showAlphaNumberPadForm()");
        }

        private void btnGetLoadBonusManualCard_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnGetLoadBonusManualCard_Click()");
            if (string.IsNullOrEmpty(txtLoadBonusManualCardNumber.Text.Trim()))
            {
                log.Info("Ends-btnGetLoadBonusManualCard_Click() as LoadBonusManualCardNumber is empty");
                return;
            }

            getLoadBonusCardDetails(txtLoadBonusManualCardNumber.Text);

            log.Debug("Ends-btnGetLoadBonusManualCard_Click()");
        }

        void getLoadBonusCardDetails(string CardNumber)
        {
            log.Debug("Starts-getLoadBonusCardDetails(" + CardNumber + ")");
            displayMessageLine("");
            if (POSStatic.ParafaitEnv.MIFARE_CARD)
                CurrentCard = new MifareCard(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            else
                CurrentCard = new Card(Common.Devices.PrimaryCardReader, CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
            if (CurrentCard.CardStatus == "NEW")
            {
                displayMessageLine(MessageUtils.getMessage(172));
                CurrentCard = null;
                log.Warn("getLoadBonusCardDetails(" + CardNumber + ") as Invalid Card");
            }
            else if (CurrentCard.siteId != -1 && CurrentCard.siteId != ParafaitEnv.SiteId && ParafaitEnv.ALLOW_ROAMING_CARDS == "N")
            {
                displayMessageLine(MessageUtils.getMessage(133));
                log.Warn("getLoadBonusCardDetails(" + CardNumber + ") as Roaming cards not allowed");
                CurrentCard = null;
            }
            else
            {
                TaskProcs.getCardDetails(CurrentCard, ref LoadBonusDGV);
                Utilities.setLanguage(FromCardDGV);
            }
            log.Debug("Ends-getLoadBonusCardDetails(" + CardNumber + ")");
        }

        private void btnLoadBonusAlphaKeypad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnLoadBonusAlphaKeypad_Click()");
            CurrentAlphanumericTextBox = txtLoadBonusManualCardNumber;
            showAlphaNumberPadForm('-');
            log.Debug("Ends-btnLoadBonusAlphaKeypad_Click()");
        }

        private void dgvBalanceTransferee_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvBalanceTransferee_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            CurrentTextBox = e.Control as TextBox;
        }

        //Load Amount as credits, Card Balance or Bonus.Select from the drop down on July 24, 2015
        private void cmbLoadGameAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gameAttribute = ((KeyValuePair<string, string>)cmbLoadGameAttributes.SelectedItem).Value;
        }

        private void cmbLoadTimeAttributes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string timeAttribute = ((KeyValuePair<string, string>)cmbLoadTimeAttributes.SelectedItem).Value;
        }
        //End

        //Begin: Added the event to enable selection of Time transactions on cell click on july 29,2015//
        private void dgvRefundTime_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvRefundTime_CellContentClick()");
            timeRefundAmount = 0;
            previousTransactionId = "";
            previousLineId = "";
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.Info("Ends-dgvRefundTime_CellContentClick() as e.ColumnIndex < 0 || e.RowIndex < 0");
                return;
            }

            if (e.ColumnIndex == 0)
            {
                int totalGames = 0;
                object trxId = dgvRefundTime["TrxId", e.RowIndex].Value;
                if (dgvRefundTime[0, e.RowIndex].Value == null || 0.Equals(dgvRefundTime[0, e.RowIndex].Value))
                {
                    foreach (DataGridViewRow dr in dgvRefundTime.Rows)
                    {
                        if (dr.Cells["TrxId"].Value.Equals(trxId))
                        {
                            dr.Cells[0].Value = 1;
                            totalGames += Convert.ToInt32(dr.Cells["Balance Time"].Value);
                            SqlCommand cmdRefundAmount = Utilities.getCommand();
                            cmdRefundAmount.CommandText = "select amount from trx_lines where TrxId = @TrxId and LineId = @LineId";
                            for (int i = 0; i < dgvRefundTime.Rows.Count; i++)
                            {
                                if (previousLineId == dr.Cells["LineId"].Value.ToString() && previousTransactionId == dr.Cells["TrxId"].Value.ToString())
                                    continue;
                                cmdRefundAmount.Parameters.Clear();
                                cmdRefundAmount.Parameters.AddWithValue("@TrxId", dr.Cells["TrxId"].Value);
                                cmdRefundAmount.Parameters.AddWithValue("@LineId", dr.Cells["LineId"].Value);
                                previousTransactionId = dr.Cells["TrxId"].Value.ToString();
                                previousLineId = dr.Cells["LineId"].Value.ToString();
                                object o = cmdRefundAmount.ExecuteScalar();
                                timeRefundAmount += Math.Round(Convert.ToDouble(o));
                            }
                            txtRefundTime.Text = timeRefundAmount.ToString();
                            timeRefundAmount = Math.Round(timeRefundAmount, 4, MidpointRounding.AwayFromZero);
                            txtRefundTime.Text = (timeRefundAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                        }
                        else
                            dr.Cells[0].Value = 0;
                    }
                    txtRefundTime.Tag = trxId;
                    grpRefundCredits.Enabled = false;
                    grpRefundGames.Enabled = false;
                    txtRefundGameAmount.Enabled = false;
                    cmbLoadGameAttributes.Enabled = false;
                    cmbLoadTimeAttributes.Enabled = true;
                    txtRefundTime.Enabled = true;
                    grpRefundTime.Enabled = true;
                    //Enabled active control to Time text box - 26-Jan-2016
                    this.ActiveControl = txtRefundTime;
                    txtRefundTime.SelectAll();
                    //End Modification
                }
                else
                {
                    foreach (DataGridViewRow dr in dgvRefundTime.Rows)
                    {
                        if (dr.Cells["TrxId"].Value.Equals(trxId))
                        {
                            dr.Cells[0].Value = 0;
                        }
                    }
                    txtRefundTime.Clear();
                    txtRefundTime.Tag = null;
                    txtRefundTime.Clear();
                    grpRefundCredits.Enabled = true;
                    grpRefundGames.Enabled = true;
                    txtRefundGameAmount.Enabled = true;
                    cmbLoadGameAttributes.Enabled = true;
                    txtRefundTime.Enabled = false;
                    cmbLoadTimeAttributes.Enabled = false;
                }
            }
            log.Debug("Ends-dgvRefundTime_CellContentClick()");
        }

        //End Added the event to enable selection of Time transactions on cell click
        private void btnLoadCardSerialMapping_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnLoadCardSerialMapping_Click()");

            displayMessageLine("");

            if (string.IsNullOrEmpty(txtFromCardSerialNumber.Text.Trim())
                || string.IsNullOrEmpty(txtToCardSerialNumber.Text.Trim()))
            {
                displayMessageLine("From Serial Number and To Serial Number cannot be blank");
                return;
            }

            int totalCount = (int)Utilities.executeScalar(@"select count(1)
                                                        from CardSerialMapping csm 
                                                        where not exists 
                                                            (select 1 
                                                                from cards c 
                                                               where c.card_number = csm.cardNumber) 
                                                        and SerialNumber between @fromSerial and @toSerial",
                                                                            new SqlParameter("@fromSerial", txtFromCardSerialNumber.Text.Trim()),
                                                                            new SqlParameter("@toSerial", txtToCardSerialNumber.Text.Trim()));
            if (MAXLOADMULTIPLECARDS < totalCount)
            {
                displayMessageLine("Entered series has " + totalCount.ToString() + " mappings. Maximum allowed is:" + MAXLOADMULTIPLECARDS.ToString());
                return;
            }

            DataTable dt = Utilities.executeDataTable(@"select top " + MAXLOADMULTIPLECARDS.ToString() + @" cardNumber, SerialNumber
                                                        from CardSerialMapping csm 
                                                        where not exists 
                                                            (select 1 
                                                                from cards c 
                                                               where c.card_number = csm.cardNumber) 
                                                        and SerialNumber between @fromSerial and @toSerial 
                                                        order by CardNumber",
                                                                            new SqlParameter("@fromSerial", txtFromCardSerialNumber.Text.Trim()),
                                                                            new SqlParameter("@toSerial", txtToCardSerialNumber.Text.Trim()));

            if (dt.Rows.Count == 0)
            {
                displayMessageLine("No serial-mapped cards found for the given series");
                return;
            }

            displayMessageLine("Processing... Please wait.");
            int CardCount = 0;
            dgvMultipleCards.Rows.Clear();
            BulkSerialNumber.Visible = true;
            LoadMultipleCards.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                CardNumber = dr["CardNumber"].ToString();
                if (TagNumber.IsValid(Utilities.ExecutionContext, CardNumber))
                {
                    Card swipedCard = new Card(CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                    LoadMultipleCards.Add(swipedCard);
                    dgvMultipleCards.Rows.Add(new object[] { ++CardCount, swipedCard.CardNumber, dr["SerialNumber"].ToString() });
                }
            }
            dgvMultipleCards.FirstDisplayedScrollingRowIndex = dgvMultipleCards.RowCount - 1;
            displayMessageLine(dgvMultipleCards.Rows.Count.ToString() + " cards loaded");

            log.Debug("Ends-btnLoadCardSerialMapping_Click()");
        }
        private void txtBonusToRedeem_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtBonusToRedeem_Validating()");
            displayMessageLine("");
            double redeemBonus;
            try
            {
                redeemBonus = Convert.ToDouble(txtBonusToRedeem.Text);
                if (redeemBonus > Convert.ToDouble(CurrentCard.bonus + CurrentCard.CreditPlusBonus))
                {
                    txtElgibleTickets.Text = "0";
                    displayMessageLine(MessageUtils.getMessage(1195));
                    log.Info("Ends-txtBonusToRedeem_Validating() as Redemption " + POSStatic.TicketTermVariant + " cannot be more than available Tickets");
                    e.Cancel = true;
                    return;
                }
            }
            catch
            {
                txtElgibleTickets.Text = "0";
                displayMessageLine(MessageUtils.getMessage(473, MessageUtils.getMessage("Bonus")));
                log.Fatal("Ends-txtBonusToRedeem_Validating() due to exception Enter a valid value for " + POSStatic.TicketTermVariant + " to Redeem");
                e.Cancel = true;
                return;
            }

            txtElgibleTickets.Text = (redeemBonus * ParafaitEnv.TICKETS_TO_REDEEM_PER_BONUS).ToString(ParafaitEnv.AMOUNT_FORMAT);
            log.Debug("Ends-txtBonusToRedeem_Validating()");
        }

        private void btnBonusRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnBonusRefresh_Click()");
            double redeemBonus;
            try
            {
                redeemBonus = Convert.ToDouble(txtBonusToRedeem.Text);
                txtElgibleTickets.Text = (redeemBonus * ParafaitEnv.TICKETS_TO_REDEEM_PER_BONUS).ToString(ParafaitEnv.AMOUNT_FORMAT);
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(473, MessageUtils.getMessage("Bonus")));
                log.Fatal("Ends-btnBonusRefresh_Click() as Enter valid value for Bonus to Redeem");
            }
            log.Debug("Ends-btnBonusRefresh_Click()");
        }

        //Added on 15-May-2017 for Redeem Tickets for credit enahancement
        #region Added code to RedeemTickets for credits enahancement
        private void rbBonus_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-rbBonus_CheckedChanged() event");
            txtRemarks.Text = "";
            lblBonusEligible.Text = "Bonus Eligible:";
            UpdateTicketsToRedeem();
            log.Debug("Ends-rbBonus_CheckedChanged() event");
        }

        private void rbCardBalance_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-rbCardBalance_CheckedChanged() event");
            txtRemarks.Text = "";
            lblBonusEligible.Text = "Card Balance Eligible:";
            UpdateTicketsToRedeem();
            log.Debug("Ends-rbCardBalance_CheckedChanged() event");
        }

        void UpdateTicketsToRedeem()
        {
            log.Debug("Starts-UpdateTicketsToRedeem() Method");
            displayMessageLine("");
            int redeemTickets;
            double denominator;
            try
            {
                //Check setup issue for cardbalance
                if (rbCardBalance.Checked)
                {
                    if (string.IsNullOrEmpty(Utilities.getParafaitDefaults("TICKETS_TO_REDEEM_PER_CREDIT")))
                    {
                        displayMessageLine(MessageUtils.getMessage(1170, POSStatic.TicketTermVariant));
                        txtBonusEligible.Text = "0";
                        log.Debug("Ends-UpdateTicketsToRedeem() as tickets to credits rate is not configured");
                        return;
                    }

                    try
                    {
                        denominator = Convert.ToInt32(Utilities.getParafaitDefaults("TICKETS_TO_REDEEM_PER_CREDIT"));

                        if (denominator < 1)
                        {
                            displayMessageLine(MessageUtils.getMessage(1170, POSStatic.TicketTermVariant));
                            txtBonusEligible.Text = "0";
                            log.Debug("Ends-UpdateTicketsToRedeem() as tickets to credits rate is not configured");
                            return;
                        }
                    }
                    catch
                    {
                        displayMessageLine(MessageUtils.getMessage(1170, POSStatic.TicketTermVariant));
                        txtBonusEligible.Text = "0";
                        log.Debug("Ends-UpdateTicketsToRedeem() as tickets to credits rate is not configured");
                        return;
                    }
                }
                else //bonus checked
                {
                    denominator = ParafaitEnv.TICKETS_TO_REDEEM_PER_BONUS;
                }

                if (string.IsNullOrEmpty(txtTicketsToRedeem.Text.Trim()))
                {
                    txtBonusEligible.Text = "0";
                    log.Debug("Ends-UpdateTicketsToRedeem() as txtTicketsToRedeem is not entered");
                    return;
                }
                redeemTickets = Convert.ToInt32(txtTicketsToRedeem.Text);
            }
            catch
            {
                displayMessageLine(MessageUtils.getMessage(473, POSStatic.TicketTermVariant));
                log.Fatal("Ends-UpdateTicketsToRedeem() as Enter valid value for " + POSStatic.TicketTermVariant + " to Redeem");
                return;
            }

            txtBonusEligible.Text = (redeemTickets / denominator).ToString(ParafaitEnv.AMOUNT_FORMAT);
            log.Debug("Ends-UpdateTicketsToRedeem() Method");
        }
        #endregion

        private string FetchManagerApprovalLimit(string taskName)
        {
            //if (mgrApprovalRequired)
            //{
            switch (taskName)
            {

                case TaskProcs.EXCHANGETOKENFORCREDIT:
                    return Utilities.getParafaitDefaults("TOKEN_FOR_CREDIT_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.LOADTICKETS:
                    return Utilities.getParafaitDefaults("LOAD_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.LOADBONUS:
                    return Utilities.getParafaitDefaults("LOAD_BONUS_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMTICKETSFORBONUS:
                    return Utilities.getParafaitDefaults("REDEEM_TICKET_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMBONUSFORTICKET:
                    return Utilities.getParafaitDefaults("REDEEM_BONUS_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REDEEMLOYALTY:
                    return Utilities.getParafaitDefaults("REDEEM_LOYALTY_LIMIT_FOR_MANAGER_APPROVAL_IN_POS");
                case TaskProcs.REDEEMVIRTUALPOINTS:
                    return Utilities.getParafaitDefaults("REDEEM_VIRTUAL_POINT_LIMIT_FOR_MANAGER_APPROVAL");
                case TaskProcs.REFUNDCARD:
                    return Utilities.getParafaitDefaults("REFUND_AMOUNT_LIMIT_FOR_MANAGER_APPROVAL");
                default:
                    return "";
            }
            //}
            // return "";
        }

        private bool ManagerApprovalCheck(int itemCount)
        {
            int mgrApprovalLimit = 0;
            string mgtLimitValue = FetchManagerApprovalLimit(TaskID);
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToInt32(mgtLimitValue);
            }
            catch { mgrApprovalLimit = 0; }

            //if (mgrApprovalRequired)
            //{
            if (mgrApprovalLimit > 0 && itemCount > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268));
                    return false;
                }
            }
            //}
            return true;
        }
        private bool ManagerApprovalCheck(decimal itemCount, string taskName = null)
        {
            log.LogMethodEntry(itemCount, taskName);
            decimal mgrApprovalLimit = 0;
            string mgtLimitValue = (taskName != null ? FetchManagerApprovalLimit(taskName) : FetchManagerApprovalLimit(TaskID));
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToDecimal(mgtLimitValue);
            }
            catch { mgrApprovalLimit = 0; }

            //if (mgrApprovalRequired)
            //{
            if (mgrApprovalLimit > 0 && itemCount > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268));
                    return false;
                }
                Users approveUser = new Users(Utilities.ExecutionContext, ParafaitEnv.ManagerId);
                Utilities.ParafaitEnv.ApproverId = approveUser.UserDTO.LoginId;
                Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
            }
            //}
            log.LogMethodExit(true);
            return true;
        }

        private bool ManagerApprovalCheck(double itemCount)
        {
            double mgrApprovalLimit = 0;
            string mgtLimitValue = FetchManagerApprovalLimit(TaskID);
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToDouble(mgtLimitValue);
            }
            catch { mgrApprovalLimit = 0; }

            //if (mgrApprovalRequired)
            //{
            if (mgrApprovalLimit > 0 && itemCount > mgrApprovalLimit && Utilities.ParafaitEnv.ManagerId == -1)
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268));
                    return false;
                }
            }
            //}
            return true;
        }

        private void dgvPointsToConvert_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvPointsToConvert.Columns[e.ColumnIndex].Name.Equals("dcAdditionalPoints") ||
                 String.IsNullOrEmpty(dgvPointsToConvert["dcAdditionalPoints", e.RowIndex].Value.ToString()))
                return;
            dgvPointsToConvert["dcBalancePoints", e.RowIndex].Value =
                (Convert.ToDecimal(dgvTappedCard["ParentCredits", 0].Value) -
                Convert.ToDecimal(dgvPointsToConvert["dcAdditionalPoints", e.RowIndex].Value));


            dgvPointsToConvert["dcAdditionalTime", e.RowIndex].Value =
                (Convert.ToDecimal(dgvTappedCard["ParentTime", 0].Value) +
                                                                      Convert.ToInt32(Convert.ToDecimal(dgvPointsToConvert["dcAdditionalPoints", e.RowIndex].Value) * multiplicationFactor));
        }

        private void dgvPointsToConvert_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvPointsToConvert.Columns[e.ColumnIndex].Name.Equals("dcAdditionalPoints"))
            {
                double varAmount = NumberPadForm.ShowNumberPadForm("Convert Points", '-', Utilities);
                if (varAmount >= 0)
                {
                    dgvPointsToConvert[e.ColumnIndex, e.RowIndex].Value = Convert.ToInt32(varAmount);
                    this.ValidateChildren();
                }
            }
        }

        private void btnClearPointsToConvert_Click(object sender, EventArgs e)
        {
            dgvPointsToConvert.Rows.Clear();
        }

        private void dgvTimeToConvert_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (!dgvTimeToConvert.Columns[e.ColumnIndex].Name.Equals("dcNewTimeToConvert") ||
                String.IsNullOrEmpty(dgvTimeToConvert["dcNewTimeToConvert", e.RowIndex].Value.ToString()))
                return;
            dgvTimeToConvert["dcNewTimeBalance", e.RowIndex].Value =
                (Convert.ToDecimal(dgvCardInfo["dcParentTime", 0].Value) -
                Convert.ToDecimal(dgvTimeToConvert["dcNewTimeToConvert", e.RowIndex].Value));
            dgvTimeToConvert["dcNewPoints", e.RowIndex].Value =
                (Convert.ToDecimal(dgvCardInfo["dcParentPoints", 0].Value) +
                                                                       (Convert.ToDecimal(dgvTimeToConvert["dcNewTimeToConvert", e.RowIndex].Value) / multiplicationFactor));
        }

        private void dgvTimeToConvert_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvTimeToConvert.Columns[e.ColumnIndex].Name.Equals("dcNewTimeToConvert"))
            {
                double varAmount = NumberPadForm.ShowNumberPadForm("Convert Time", '-', Utilities);
                if (varAmount >= 0)
                {
                    dgvTimeToConvert[e.ColumnIndex, e.RowIndex].Value = Convert.ToInt32(varAmount);
                    this.ValidateChildren();
                }
            }
        }

        private void btnCustomerLookUp_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities);
            if (customerLookupUI.ShowDialog() == DialogResult.OK)
            {
                textBoxTransferCardNumber.Text = customerLookupUI.CardNumber;
            }
            log.LogMethodExit();
        }

        private void ManualTicketApprovalCheck(int manualTickets)
        {
            log.LogMethodEntry(manualTickets);
            int managerApprovalLimitForManualTicket;
            try
            {
                managerApprovalLimitForManualTicket = ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
            }
            catch
            {
                managerApprovalLimitForManualTicket = 0;
            }
            if ((managerApprovalLimitForManualTicket != 0 && manualTickets > managerApprovalLimitForManualTicket) || ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET") == true)
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Manager approval required to add manual ticket"));
                    log.LogMethodExit("Manager approval is required to proceed further to add manual ticket ");
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Manager approval required to add manual tickets"));
                }

            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tabPageHoldEntitlement.Controls.Remove(HoldEntDGV);
            HoldEntDGV = new DataGridView();
            HoldEntDGV.Location = new Point(20, 100);
            HoldEntDGV.BackgroundColor = this.BackColor;
            HoldEntDGV.BorderStyle = BorderStyle.None;
            HoldEntDGV.Height = 10;
            buttonOK.Text = "HOLD";
            tabPageHoldEntitlement.Controls.Add(HoldEntDGV);
            textBoxHoldEntitlementNumber.Clear();
            CurrentCard = null;
            this.ActiveControl = textBoxHoldEntitlementNumber;
            displayMessageLine("");
        }

        private List<SubscriptionBillingScheduleDTO> GetSubscriptionBillingSchedules(AccountDTO accountDTO, SqlTransaction inSQLTrx)
        {
            log.LogMethodEntry((accountDTO != null ? accountDTO.AccountId : -1), inSQLTrx);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
            if (accountDTO != null)
            {
                List<int> subsriptionBillingCycleIdList = new List<int>();
                if (accountDTO.AccountCreditPlusDTOList != null && accountDTO.AccountCreditPlusDTOList.Any()
                    && accountDTO.AccountCreditPlusDTOList.Exists(cp => cp.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountCreditPlusDTOList.Where(cp => cp.SubscriptionBillingScheduleId > -1).Select(cp => cp.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountGameDTOList != null && accountDTO.AccountGameDTOList.Any()
                    && accountDTO.AccountGameDTOList.Exists(cg => cg.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountGameDTOList.Where(cg => cg.SubscriptionBillingScheduleId > -1).Select(cg => cg.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null && accountDTO.AccountDiscountDTOList.Any()
                   && accountDTO.AccountDiscountDTOList.Exists(cd => cd.SubscriptionBillingScheduleId > -1))
                {
                    List<int> tempIdList = accountDTO.AccountDiscountDTOList.Where(cd => cd.SubscriptionBillingScheduleId > -1).Select(cd => cd.SubscriptionBillingScheduleId).ToList();
                    if (tempIdList != null && tempIdList.Any())
                    {
                        tempIdList = tempIdList.Distinct().ToList();
                        subsriptionBillingCycleIdList.AddRange(tempIdList);
                    }
                }

                if (subsriptionBillingCycleIdList != null && subsriptionBillingCycleIdList.Any())
                {
                    SubscriptionBillingScheduleListBL subscriptionBillingScheduleListBL = new SubscriptionBillingScheduleListBL(Utilities.ExecutionContext);
                    subscriptionBillingScheduleDTOList = subscriptionBillingScheduleListBL.GetSubscriptionBillingScheduleDTOListById(subsriptionBillingCycleIdList, inSQLTrx);
                }
            }
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;
        }

        private void IsCardOnHold(AccountDTO accountDTO)
        {
            log.LogMethodEntry();
            if (accountDTO != null)
            {
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = GetSubscriptionBillingSchedules(accountDTO, null);
                if (accountDTO.AccountCreditPlusDTOList != null
                   && accountDTO.AccountCreditPlusDTOList.Count > 0
                   && accountDTO.AccountCreditPlusDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountCreditPlusDTO> accountCreditPlusHoldList = accountDTO.AccountCreditPlusDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                          && (x.SubscriptionBillingScheduleId == -1
                                                                                                                             || (x.SubscriptionBillingScheduleId != -1
                                                                                                                                    && subscriptionBillingScheduleDTOList != null
                                                                                                                                    && subscriptionBillingScheduleDTOList.Any()
                                                                                                                                    && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                                   && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountCreditPlusHoldList != null && accountCreditPlusHoldList.Any())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2610));
                    }
                }
                if (accountDTO.AccountGameDTOList != null
                && accountDTO.AccountGameDTOList.Count > 0
                && accountDTO.AccountGameDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountGameDTO> accountGameHoldList = accountDTO.AccountGameDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                        && (x.SubscriptionBillingScheduleId == -1
                                                                                                            || (x.SubscriptionBillingScheduleId != -1
                                                                                                                && subscriptionBillingScheduleDTOList != null
                                                                                                                && subscriptionBillingScheduleDTOList.Any()
                                                                                                                && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                 && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountGameHoldList != null && accountGameHoldList.Any())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2610));
                    }
                }
                if (accountDTO.AccountDiscountDTOList != null
                && accountDTO.AccountDiscountDTOList.Count > 0
                && accountDTO.AccountDiscountDTOList.Exists(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold))
                {
                    List<AccountDiscountDTO> accountDiscountHoldList = accountDTO.AccountDiscountDTOList.Where(x => x.ValidityStatus == AccountDTO.AccountValidityStatus.Hold
                                                                                                                    && (x.SubscriptionBillingScheduleId == -1
                                                                                                                        || (x.SubscriptionBillingScheduleId != -1
                                                                                                                            && subscriptionBillingScheduleDTOList != null
                                                                                                                            && subscriptionBillingScheduleDTOList.Any()
                                                                                                                            && subscriptionBillingScheduleDTOList.Exists(sbs => sbs.SubscriptionBillingScheduleId == x.SubscriptionBillingScheduleId
                                                                                                                                                                             && sbs.TransactionId != -1 && sbs.IsActive)))).ToList();
                    //Ignore subscription hold
                    if (accountDiscountHoldList != null && accountDiscountHoldList.Any())
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2610));
                    }
                }
            }
        }
    }
}
