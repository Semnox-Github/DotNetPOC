/********************************************************************************************
 * Project Name - POS- frmScanAndRedeem
 * Description  - POS redemption class 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.3.0       25-Jun-2018      Guru S A            Modifications for redemption currency shortcut keys 
 *2.5.0       12-Feb-2019      Archana             Redemption gift search changes
 *2.6.0       11-Apr-2019      Archana             Include/Exclude for redeemable products
 *2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards  
 *2.70.3      28-Aug-2019      Dakshakh            Redemption currency rule enhancement
 *2.80.0      22-Apr-2020      Guru S A            Modified for Redemption UI enhancement changes.
 *2.80.1      23-Oct-2020      Guru S A            Modified for Redemption UI enhancement changes.
 *2.100.0     27-Nov-2020      Guru S A            Enable/Disable manual ticket button issue Fix 
 *2.110.0     11-Jan-2021      Guru S A            Currency Shortcut keys dont work unless user clicks somewhere on screen
 ********************************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient; //Added 25-May-2017
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Transaction; 

namespace Parafait_POS.Redemption
{
    public partial class frmScanAndRedeem : Form
    {
        Utilities _utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        clsRedemption _redemption;
        public string LoggedInId;
        public int deviceAddress = -1;
        Security.User _user;
        bool enableSingleUserMultiScreen = false;
        double RedemptionDiscount = 0;
        DataTable DT_Search = new DataTable();
        DataTable DT_Gifts = new DataTable();
        int mgrApprovalLimit = 0;
        private readonly TagNumberParser tagNumberParser;
        int bannerHeight;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        List<RedemptionCurrencyRuleBL> redemptionCurrencyRuleBLList;
        bool checkApplicableCurrenyRule;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private bool enableMultiUserMultiScreen = false;
        private bool showAllRedemptionScreens = false;
        private bool isFormClosed = false;
        private Panel reloginPanel = new Panel();
        private const string SCANCODEADDUSER = "ADDUSER";
        private const string SCANCODEADDSCRN = "ADDSCRN";
        private const string SCANCODECLOSESCRN = "CLOSESCRN";
        private const string SCANCODESCREEN1 = "SCREEN1";
        private const string SCANCODESCREEN2 = "SCREEN2";
        private const string SCANCODESCREEN3 = "SCREEN3";
        private const string SCANCODESCREEN4 = "SCREEN4";
        private const string SCANCODESCREEN5 = "SCREEN5";
        private const string SCANCODESCREEN6 = "SCREEN6";
        private const string SCANCODESCREEN7 = "SCREEN7";
        private const string SCANCODESCREEN8 = "SCREEN8";
        private const string MDIFORM = "frmScanAndRedeemMDI";
        private VirtualKeyboardController virtualKeyboard;
        private BackgroundWorker worker;
        /// <summary>
        /// clsLastScanObject
        /// </summary>
        class clsLastScanObject
        {
            public bool isManualTicket = false;
            public bool isCurrency = false;
            public string BarCode;
        }
        clsLastScanObject lastScanObject = new clsLastScanObject();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string shortCutKeyValues;
        bool enableRCShortCutKeyFeature;
        List<Tuple<Byte[], string>> redemptionCurrencyShortCuts;
        public string GetCurrentScreenNumber { get { return lblScreenNumber.Text; } }
        public string GetScreenUserLoginId { get { return _user.LoginId; } }
        public bool IsThisFormClosed { get { return isFormClosed; } }
        /// <summary>
        /// frmScanAndRedeem
        /// </summary>
        /// <param name="User"></param>
        public frmScanAndRedeem(Security.User User)
        {
            log.LogMethodEntry(User);
            Semnox.Core.GenericUtilities.Logger.setRootLogLevel(log);
            InitializeComponent();
            SetLastActivityTime();
            _user = User;
            lblCardNumber.Text = "";
            POSStatic.Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            mdiParent.childFormIndex++;

            lblPriceInTickets.Text = lblPriceInTickets.Text.Replace("$$$", POSStatic.TicketTermVariant); //Added 30-May-2017
            enableSingleUserMultiScreen = ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "ENABLE_SINGLE_USER_MULTI_SCREEN");
            enableMultiUserMultiScreen = ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "ENABLE_MULTI_USER_MULTI_SCREEN");
            showAllRedemptionScreens = ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "SHOW_ALL_REDEMPTION_SCREENS");

            SetScreenNumber();
            SetScreenUserAndColor();
            deviceAddress = mdiParent.GetDeviceAddress(_user.LoginId);
            bannerHeight = (Application.OpenForms["frmRedemptionScreenBanner"] != null ? Application.OpenForms["frmRedemptionScreenBanner"].Height : 0);
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private List<RedemptionCurrencyRuleBL> LoadRedemptionCurrencyRules()
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(machineUserContext);
            List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> redemptionCurrencyRuleSearchParams = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
            redemptionCurrencyRuleSearchParams.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            redemptionCurrencyRuleSearchParams.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));
            List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(redemptionCurrencyRuleSearchParams, true);
            if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Count > 0)
            {
                redemptionCurrencyRuleDTOList = redemptionCurrencyRuleDTOList.OrderBy(rcr => rcr.Priority).ToList();
                redemptionCurrencyRuleBLList = new List<RedemptionCurrencyRuleBL>();
                foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                {
                    RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(machineUserContext, redemptionCurrencyRuleDTO);
                    redemptionCurrencyRuleBLList.Add(redemptionCurrencyRuleBL);
                }
            }

            log.LogMethodExit(redemptionCurrencyRuleBLList);
            return redemptionCurrencyRuleBLList;
        }

        /// <summary>
        /// get Screen Number
        /// </summary>
        internal int GetScreenNumber(bool yetToOpen)
        {
            log.LogMethodEntry(yetToOpen);
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = (yetToOpen ? (mdiParent.GetOpenScanRedeemChildFormCount() + 1) : mdiParent.GetOpenScanRedeemChildFormCount());
            int lclScreenNumber = 1;
            int screenNumber = 1;

            // look for an available screen, by scanning through the forms
            while (lclScreenNumber <= childCount)
            {
                bool found = false;
                foreach (frmScanAndRedeem f in mdiParent.GetOpenScanRedeemChildForms())
                {
                    if (lclScreenNumber.Equals(Convert.ToInt32(f.Controls.Find("lblScreenNumber", true)[0].Text)))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    screenNumber = lclScreenNumber;
                    break;
                }
                else
                    lclScreenNumber++;
            }

            return screenNumber;
        }

        /// <summary>
        /// get Screen Number
        /// </summary>
        void SetScreenNumber()
        {
            log.LogMethodEntry();
            int screenNo = GetScreenNumber(true);
            log.Info("screenNo" + screenNo.ToString());
            lblScreenNumber.Text = screenNo.ToString();
            log.LogMethodExit();
        }

        private void SetScreenUserAndColor()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            SetColorTheme(mdiParent);
            mdiParent.SetScreenUser(_user, lblScreenNumber.Text, this.panelTop.BackgroundImage, this.BackColor);
            log.LogMethodExit();
        }

        private void SetColorTheme(frmScanAndRedeemMDI mdiParent)
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI.ColorThemes colorTheme = mdiParent.GetUserColorTheme(_user.LoginId);
            this.panelTop.BackgroundImage = colorTheme.TopPanelBackgroundImage;
            this.BackColor = colorTheme.BackColor;

            log.LogMethodExit();
        }

        private int GetScanAndRedeemFormCount(frmScanAndRedeemMDI mdiParent)
        {
            log.LogMethodEntry();
            int childCount = 1;

            if (showAllRedemptionScreens)
            {
                childCount = mdiParent.GetOpenScanRedeemChildFormCount();
            }
            else
            {
                childCount = (mdiParent.RedemptionUserDetailsList != null && mdiParent.RedemptionUserDetailsList.Any() ? mdiParent.RedemptionUserDetailsList.Count : 0);
            }
            if (childCount == 0)
            {
                childCount = 1;
            }
            log.LogMethodExit(childCount);
            return childCount;
        }

        private void CreateMenuDetails(int childCount)
        {
            log.LogMethodEntry(childCount);
            int panelWidth = 152;
            if (childCount > 6)
            {
                panelWidth = 228;
            }
            fpnlMoreMenu = new FlowLayoutPanel
            {
                MinimumSize = new Size(panelWidth, 0),
                MaximumSize = new Size(panelWidth, 0),
                Size = new Size(panelWidth, 60),
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.White,
                Name = "fpnlMoreMenuSubPanel",
                Margin = new Padding(2),
                BackgroundImage = Properties.Resources.whiteBackground,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            fpnlMoreMenu.Leave += new EventHandler(FPnlMoreMenuLostFocus);
            if (fpnlMoreMenu != null)
            {
                fpnlMoreMenu.SuspendLayout();
                // 
                // btnAddTicketMenuItem
                // 
                this.btnAddTicketMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.AddTicketNormal;
                this.btnAddTicketMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnAddTicketMenuItem.FlatAppearance.BorderSize = 0;
                this.btnAddTicketMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnAddTicketMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnAddTicketMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnAddTicketMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnAddTicketMenuItem.ForeColor = System.Drawing.Color.White;
                //this.btnAddManual.Location = new System.Drawing.Point(353, 5);
                this.btnAddTicketMenuItem.Name = "btnAddTicketMenuItem";
                this.btnAddTicketMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnAddTicketMenuItem.Text = "Add";
                this.btnAddTicketMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnAddTicketMenuItem.UseVisualStyleBackColor = true;
                this.btnAddTicketMenuItem.Click += new System.EventHandler(this.btnAddManual_Click);
                this.btnAddTicketMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddManual_MouseDown);
                this.btnAddTicketMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddManual_MouseUp);
                //this.btnAddTicketMenuItem.Visible = false;
                // 
                // btnLoadTicketMenuItem
                // 
                this.btnLoadTicketMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.LoadTicket_Normal;
                this.btnLoadTicketMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnLoadTicketMenuItem.FlatAppearance.BorderSize = 0;
                this.btnLoadTicketMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnLoadTicketMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnLoadTicketMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnLoadTicketMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnLoadTicketMenuItem.ForeColor = System.Drawing.Color.White;
                //this.btnLoadTicketMenuItem.Location = new System.Drawing.Point(423, 5);
                this.btnLoadTicketMenuItem.Name = "btnLoadTicketMenuItem";
                this.btnLoadTicketMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnLoadTicketMenuItem.Text = "Load Tickets";
                this.btnLoadTicketMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnLoadTicketMenuItem.UseVisualStyleBackColor = true;
                this.btnLoadTicketMenuItem.Click += new System.EventHandler(this.btnLoadTickets_Click);
                this.btnLoadTicketMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnLoadTickets_MouseDown);
                this.btnLoadTicketMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnLoadTickets_MouseUp);
                //this.btnLoadTicketMenuItem.Visible = false;
                // 
                // btnTurnInMenuItem
                // 
                this.btnTurnInMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.TurnInNormal;
                this.btnTurnInMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnTurnInMenuItem.FlatAppearance.BorderSize = 0;
                this.btnTurnInMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnTurnInMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnTurnInMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnTurnInMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnTurnInMenuItem.ForeColor = System.Drawing.Color.White;
                //this.btnTurnInMenuItemMenuItem.Location = new System.Drawing.Point(493, 5);
                this.btnTurnInMenuItem.Name = "btnTurnInMenuItem";
                this.btnTurnInMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnTurnInMenuItem.Text = "Turn-In";
                this.btnTurnInMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnTurnInMenuItem.UseVisualStyleBackColor = true;
                this.btnTurnInMenuItem.Click += new System.EventHandler(this.btnTurnIn_Click);
                this.btnTurnInMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTurnIn_MouseDown);
                this.btnTurnInMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTurnIn_MouseUp);
                //this.btnTurnInMenuItem.Visible = false;
                // 
                // btnProductSearchMenuItem
                // 
                this.btnProductSearchMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.Product_Search_Btn_Normal;
                this.btnProductSearchMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnProductSearchMenuItem.FlatAppearance.BorderSize = 0;
                this.btnProductSearchMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnProductSearchMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnProductSearchMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnProductSearchMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
                this.btnProductSearchMenuItem.ForeColor = System.Drawing.Color.White;
                //this.btnProductSearchMenuItem.Location = new System.Drawing.Point(563, 5);
                this.btnProductSearchMenuItem.Name = "btnProductSearchMenuItem";
                this.btnProductSearchMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnProductSearchMenuItem.Text = "Gift";
                this.btnProductSearchMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnProductSearchMenuItem.UseVisualStyleBackColor = true;
                this.btnProductSearchMenuItem.Click += new System.EventHandler(this.btnProductSearch_Click);
                this.btnProductSearchMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnProductSearch_MouseDown);
                this.btnProductSearchMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnProductSearch_MouseUp);
                //this.btnProductSearchMenuItem.Visible = false;
                // 
                // btnFlagTicketReceiptMenuItem
                // 
                this.btnFlagTicketReceiptMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.FlagReceipt_Normal;
                this.btnFlagTicketReceiptMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnFlagTicketReceiptMenuItem.FlatAppearance.BorderSize = 0;
                this.btnFlagTicketReceiptMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnFlagTicketReceiptMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnFlagTicketReceiptMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnFlagTicketReceiptMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnFlagTicketReceiptMenuItem.ForeColor = System.Drawing.Color.White;
                // this.btnFlagTicketReceiptMenuItem.Location = new System.Drawing.Point(633, 5);
                this.btnFlagTicketReceiptMenuItem.Name = "btnFlagTicketReceiptMenuItem";
                this.btnFlagTicketReceiptMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnFlagTicketReceiptMenuItem.Text = "Flag Voucher";
                this.btnFlagTicketReceiptMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnFlagTicketReceiptMenuItem.UseVisualStyleBackColor = true;
                this.btnFlagTicketReceiptMenuItem.Click += new System.EventHandler(this.btnFlagTicketReceipt_Click);
                this.btnFlagTicketReceiptMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnFlagTicketReceipt_MouseDown);
                this.btnFlagTicketReceiptMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnFlagTicketReceipt_MouseUp);
                //this.btnFlagTicketReceiptMenuItem.Visible = false;
                // 
                // btnScanTicketOrGiftMenuItem
                // 
                this.btnScanTicketOrGiftMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.ScanGift;
                this.btnScanTicketOrGiftMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnScanTicketOrGiftMenuItem.FlatAppearance.BorderSize = 0;
                this.btnScanTicketOrGiftMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnScanTicketOrGiftMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnScanTicketOrGiftMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnScanTicketOrGiftMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnScanTicketOrGiftMenuItem.ForeColor = System.Drawing.Color.White;
                //this.btnScanTicketOrGiftMenuItem.Location = new System.Drawing.Point(283, 5);
                this.btnScanTicketOrGiftMenuItem.Name = "btnScanTicketOrGiftMenuItem";
                this.btnScanTicketOrGiftMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnScanTicketOrGiftMenuItem.Text = "Scan Gift";
                this.btnScanTicketOrGiftMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnScanTicketOrGiftMenuItem.UseVisualStyleBackColor = true;
                this.btnScanTicketOrGiftMenuItem.Click += new System.EventHandler(this.btnScanTicketOrGift_Click);
                this.btnScanTicketOrGiftMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnScanTicketOrGift_MouseDown);
                this.btnScanTicketOrGiftMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnScanTicketOrGift_MouseUp);
                //this.btnScanTicketOrGiftMenuItem.Visible = false;
                // 
                // btnSuspendMenuItem
                // 
                this.btnSuspendMenuItem.BackgroundImage = global::Parafait_POS.Properties.Resources.OrderSuspend;
                this.btnSuspendMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                this.btnSuspendMenuItem.FlatAppearance.BorderSize = 0;
                this.btnSuspendMenuItem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                this.btnSuspendMenuItem.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                this.btnSuspendMenuItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                this.btnSuspendMenuItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.btnSuspendMenuItem.ForeColor = System.Drawing.Color.White;
                // this.btnSuspendMenuItem.Location = new System.Drawing.Point(213, 5);
                this.btnSuspendMenuItem.Name = "btnSuspendMenuItem";
                this.btnSuspendMenuItem.Size = new System.Drawing.Size(70, 45);
                this.btnSuspendMenuItem.Text = "Suspend";
                this.btnSuspendMenuItem.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.btnSuspendMenuItem.UseVisualStyleBackColor = true;
                this.btnSuspendMenuItem.Click += new System.EventHandler(this.btnSuspend_Click);
                this.btnSuspendMenuItem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSuspend_MouseDown);
                this.btnSuspendMenuItem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnSuspend_MouseUp);
                //this.btnSuspendMenuItem.Visible = false;
                fpnlMoreMenu.Controls.Add(btnScanTicketOrGiftMenuItem);
                fpnlMoreMenu.Controls.Add(btnSuspendMenuItem);
                fpnlMoreMenu.Controls.Add(btnAddTicketMenuItem);
                fpnlMoreMenu.Controls.Add(btnLoadTicketMenuItem);
                fpnlMoreMenu.Controls.Add(btnProductSearchMenuItem);
                fpnlMoreMenu.Controls.Add(btnTurnInMenuItem);
                fpnlMoreMenu.Controls.Add(btnFlagTicketReceiptMenuItem);
                //pnlMoreMenu.Controls.Add(fpnlMoreMenuSubPanel);
                this.panel1.Controls.Add(fpnlMoreMenu);
                // pnlMoreMenu.ResumeLayout(true);
                fpnlMoreMenu.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void FPnlMoreMenuLostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (fpnlMoreMenu != null)
            {
                fpnlMoreMenu.Visible = false;
                panelAddClose.BringToFront();
            }
            log.LogMethodExit();
        }

        private void UpdateMoreMenuDetails(int childCount)
        {
            log.LogMethodEntry(childCount);
            CreateMenuDetails(childCount);
            if (fpnlMoreMenu != null)
            {
                fpnlMoreMenu.SuspendLayout();
                btnAddTicketMenuItem.Visible = false;
                btnLoadTicketMenuItem.Visible = false;
                btnProductSearchMenuItem.Visible = false;
                btnTurnInMenuItem.Visible = false;
                btnFlagTicketReceiptMenuItem.Visible = false;
                btnScanTicketOrGiftMenuItem.Visible = false;
                btnSuspendMenuItem.Visible = false;
                if (childCount >= 2 && childCount <= 4)
                {
                    btnLoadTicketMenuItem.Visible = true;
                    btnProductSearchMenuItem.Visible = true;
                    btnTurnInMenuItem.Visible = true;
                    btnFlagTicketReceiptMenuItem.Visible = true;
                }
                else if (childCount > 4 && childCount <= 6)
                {
                    btnAddTicketMenuItem.Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
                    btnLoadTicketMenuItem.Visible = true;
                    btnProductSearchMenuItem.Visible = true;
                    btnTurnInMenuItem.Visible = true;
                    btnFlagTicketReceiptMenuItem.Visible = true;
                }
                else if (childCount > 6)
                {
                    btnAddTicketMenuItem.Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
                    btnLoadTicketMenuItem.Visible = true;
                    btnProductSearchMenuItem.Visible = true;
                    btnTurnInMenuItem.Visible = true;
                    btnFlagTicketReceiptMenuItem.Visible = true;
                    btnScanTicketOrGiftMenuItem.Visible = true;
                    btnSuspendMenuItem.Visible = true;
                }
                fpnlMoreMenu.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        #region USBListeners

        DeviceClass cardReader;

        private void RegisterDevices()
        {
            log.LogMethodEntry();
            SetLastActivityTime(); 
            bool newEntry = false;

            if (cardReader != null)
            {
                cardReader.Dispose();
            }

            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = mdiParent.GetOpenScanRedeemChildFormCount(); 

            cardReader = mdiParent.GetCardReaderForUser(_user.LoginId);

            if (cardReader == null)
            {
                newEntry = true;
                deviceAddress = mdiParent.GetDeviceAddress(_user.LoginId);
                cardReader = mdiParent.RegisterCardDevice(_user.LoginId);
                 
            }

             
            if (cardReader != null)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Card reader is registered"));
                log.Info("Card reader is registered");
                if (newEntry)
                {
                    mdiParent.AddCardReaderForUser(_user.LoginId, cardReader, deviceAddress);
                }
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Unable to register card reader"));
                log.Info("Unable to register card reader");
            }

            SetLastActivityTime();
            log.LogMethodExit(); 
        }


        /// <summary>
        /// Device
        /// </summary>
        public class Device
        {
            internal string DeviceName;
            internal string DeviceType;
            internal string DeviceSubType;
            internal string VID, PID, OptString;
        }

        /// <summary>
        /// registerUSBDevice
        /// </summary>
        /// <returns></returns>
        private bool RegisterUSBDevice(frmScanAndRedeemMDI mdiParent, bool newEntry)
        {
            log.LogMethodEntry(newEntry);
            SetLastActivityTime();

            if (newEntry)
            {
                List<Device> deviceList = new List<Device>();

                PeripheralsListBL peripheralsListBL = new PeripheralsListBL(_utilities.ExecutionContext);
                List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
                searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "CardReader"));
                searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (_utilities.ParafaitEnv.POSMachineId).ToString()));
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

                string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_READER_VID");
                string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_READER_PID");
                string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_READER_OPT_STRING");

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

                int childCount = mdiParent.GetOpenScanRedeemChildFormCount();
                if (deviceList.Count >= childCount)
                {
                    int index = mdiParent.GetDeviceAddress(_user.LoginId);
                    Device deviceSelected = deviceList[index];
                    bool flag = usbCardReader.InitializeUSBReader(this.MdiParent, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                    if (usbCardReader.isOpen)
                    {
                        cardReader = usbCardReader;
                        mdiParent.AddCardReaderForUser(_user.LoginId, cardReader, deviceAddress);
                        SetLastActivityTime();
                        log.Debug("Ends-registerUSBDevice()");
                        return true;
                    }
                }

                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 281));
                log.Info("Ends-registerUSBDevice() as Unable to find USB card reader");
                SetLastActivityTime();
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Retrieved USB card reader"));
                log.Info("Retrived USB card reader");
                log.LogMethodExit(true);
                SetLastActivityTime();
                return true;
            }
        }


        private void CardScanCompleteEventHandle(object sender, EventArgs e)
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
                        DisplayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, _utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        DisplayMessageLine(ex.Message, ERROR);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        DisplayMessageLine(ex.Message, ERROR);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    DisplayMessageLine(message);
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;

                try
                {
                    HandleCardRead(CardNumber, sender as DeviceClass);
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                    log.Fatal("Ends-CardScanCompleteEventHandle() due to exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        DeviceClass barcodeScanner;
        private bool RegisterBarcodeScanner()
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            if (barcodeScanner != null)
            {
                barcodeScanner.Dispose();
            }

            barcodeScanner = mdiParent.GetBarCodeScannerForUser(_user.LoginId);

            if (barcodeScanner == null)
            {
                List<Device> deviceList = new List<Device>();
                PeripheralsListBL peripheralsListBL = new PeripheralsListBL(_utilities.ExecutionContext);
                List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
                searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "BarcodeReader"));
                searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (_utilities.ParafaitEnv.POSMachineId).ToString()));
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

                string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_BARCODE_READER_VID");
                string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_BARCODE_READER_PID");
                string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "USB_BARCODE_READER_OPT_STRING");

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

                USBDevice barcodeListener;
                if (IntPtr.Size == 4) //32 bit
                    barcodeListener = new KeyboardWedge32();
                else
                    barcodeListener = new KeyboardWedge64();

                int index = mdiParent.GetDeviceAddress(_user.LoginId);
                if (deviceList.Count > index)
                {
                    Device deviceSelected = deviceList[index];
                    bool flag = barcodeListener.InitializeUSBReader(this.MdiParent, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                    if (barcodeListener.isOpen)
                    {
                        barcodeScanner = barcodeListener;
                        mdiParent.AddBarCodeScannerForUser(_user.LoginId, barcodeScanner, index);
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2689));//"USB Bar Code scanner is added");
                        log.Info("USB Bar Code scanner is added");
                        SetLastActivityTime();
                        log.LogMethodExit(true);
                        return true;
                    }
                }
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2688));// "Unable to find USB Bar Code scanner");
                log.Info("Ends-registerBarcodeScanner() as Unable to find USB Bar Code scanner");
                SetLastActivityTime();
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2687));// "Retrived USB Bar Code scanner details");
                log.Info("Retrived USB Bar Code scanner details");
                log.LogMethodExit(true);
                SetLastActivityTime();
                return true;
            }
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs && _utilities != null)
            {
                try
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    string scannedBarcode = _utilities.ProcessScannedBarCode(checkScannedEvent.Message, _utilities.ParafaitEnv.LEFT_TRIM_BARCODE, _utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);

                    //Thread error fix by threading 15-May-2016
                    this.Invoke((MethodInvoker)delegate
                    {
                        ProcessBarcode(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                    log.Fatal("Exception " + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        #endregion USBListeners 

        private void frmScanAndRedeem_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetReloginPanel();
            lblMessage.Text = "";
            (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;

            ResizeMdiChildren();
            dgvRedemption.BackgroundColor = this.BackColor;
            btnAddManual.Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;

            _utilities = new Utilities();
            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch
            {
                log.Fatal("Unable to get ipaddress due to exception ");
            }

            _utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            Authenticate.loginUser(_user, _utilities.ParafaitEnv);
            _utilities.ParafaitEnv.Initialize();
            SetLastActivityTime();

           
            RegisterBarcodeScanner();
            RegisterDevices();
            
            this.Activated += frmScanAndRedeem_Activated;
            this.Deactivate += frmScanAndRedeem_Deactivate;

            _utilities.setLanguage();

            LoggedInId = lblLoginId.Text = _utilities.ParafaitEnv.LoginID;
            SetGiftGridColumnSizeAndStyle();

            string savMes = lblMessage.Text;
            LoadRedemptionCurrencyRules();
            NewRedemptionCheck();

            if (!string.IsNullOrEmpty(savMes))
                DisplayMessageLine(savMes, WARNING);
            RefreshSuspended();
            SetManagerApprovalLimitForManualTicketAddition();
            EnableDisableCurrencyShortCutKeys();
            SetLastActivityTime();
            SetTimerClock();
            SetFocusOnDataGrid();
            log.LogMethodExit();
        }

        private void SetTimerClock()
        {
            log.LogMethodEntry();
            this.timerClock.Interval = 1000;
            this.timerClock.Tick += new System.EventHandler(this.timerClock_Tick);
            log.LogMethodExit();
        }

        private void SetGiftGridColumnSizeAndStyle()
        {
            log.LogMethodEntry();
            dcPrice.DefaultCellStyle = dcQuantity.DefaultCellStyle = dcTotal.DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            this.dcProductName.MinimumWidth = 60;
            this.dcPrice.MinimumWidth = 30;
            this.dcQuantity.MinimumWidth = 30;
            this.dcTotal.MinimumWidth = 60;
            this.dcPrice.Width = 60;
            this.dcQuantity.Width = 60;
            this.dcTotal.Width = 80;
            log.LogMethodExit();
        }

        private void SetManagerApprovalLimitForManualTicketAddition()
        {
            log.LogMethodEntry();
            string mgtLimitValue = ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToInt32(mgtLimitValue);
                else
                    mgrApprovalLimit = 0;
            }
            catch
            { mgrApprovalLimit = 0; }
            log.LogMethodExit();
        }

        private void EnableDisableCurrencyShortCutKeys()
        {
            log.LogMethodEntry();
            try
            {
                redemptionCurrencyShortCuts = new List<Tuple<Byte[], string>>();
                enableRCShortCutKeyFeature = false;
                if (ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "ENABLE_REDEMPTION_CURRENCY_SHORTCUT_KEYS").ToString() == "Y")
                {
                    enableRCShortCutKeyFeature = true;
                    redemptionCurrencyShortCuts = _redemption.PopulateRCShortCutKeys();
                    if (redemptionCurrencyShortCuts == null || redemptionCurrencyShortCuts.Count == 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1585));
                    }
                }
            }
            catch
            { enableRCShortCutKeyFeature = false; }
            log.LogMethodExit();
        }

        /// <summary>
        /// frmScanAndRedeem_Deactivate
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void frmScanAndRedeem_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (enableSingleUserMultiScreen == true
            //    || (enableSingleUserMultiScreen == false && enableMultiUserMultiScreen == true)
            //  )
            //{
            //    if (GetReloginAuthenticationInitiated() == false)
            //    {
            //        log.Info("Unregister on deactivation " + this.GetCurrentScreenNumber + " : " + this.GetScreenUserLoginId);
            //        if (cardReader != null)
            //            cardReader.UnRegister();
            //        if (barcodeScanner != null)
            //            barcodeScanner.UnRegister();
            //    }
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// frmScanAndRedeem_Activated
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void frmScanAndRedeem_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (GetReloginAuthenticationInitiated() == false && isFormClosed == false)
            {
                RefreshScreen();
                StopTimerCloseChildFormsOnScreensForTheUser(_user.LoginId);

                frmRedemptionScreenBanner frmScreenBanner = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
                if (frmScreenBanner != null)
                {
                    frmScreenBanner.SetSelectedScreenTileColor(_user.LoginId, lblScreenNumber.Text);
                }
                SetUIBackgroundColor(_user.LoginId);
                StartScreenTimer();
                ShowHideAddScreenButton();
                frmScanAndRedeemMDI mdiParent = Application.OpenForms["frmScanAndRedeemMDI"] as frmScanAndRedeemMDI;
                if (mdiParent.IsActiveScreen(this.GetScreenUserLoginId, this.GetCurrentScreenNumber) == false)
                {
                    StopDevicesOnAllScreens();
                    mdiParent.SetAsActiveScreen(GetScreenUserLoginId, this.GetCurrentScreenNumber);
                    ActivateDevicesOnAllActiveScreens();
                }
            }
            log.LogMethodExit();
        }

        private void StopTimerCloseChildFormsOnScreensForTheUser(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (string.IsNullOrEmpty(loginId) == false)
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                foreach (frmScanAndRedeem f in mdiParent.GetOpenScanRedeemChildForms(loginId))
                {
                    if (f.GetCurrentScreenNumber != this.GetCurrentScreenNumber)
                    {
                        f.CloseChildAndRelatedForms();
                    }
                    f.StopScreenTimer();
                }
            }
            log.LogMethodExit();
        }

        private void SetUIBackgroundColor(string loginId)
        {
            log.LogMethodEntry(loginId);
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            Color backColor;
            Image pnlToBackGroundImage = null;
            foreach (frmScanAndRedeem f in mdiParent.GetOpenScanRedeemChildForms(loginId))
            {
                if (f.lblScreenNumber.Text == this.lblScreenNumber.Text)
                {
                    backColor = mdiParent.GetBackgroundColor(loginId);
                    pnlToBackGroundImage = mdiParent.GetPanelTopBackgroundImage(loginId);
                    if (backColor != null)
                    {
                        f.dgvRedemption.BackgroundColor = f.BackColor = backColor;
                    }
                    if (pnlToBackGroundImage != null)
                        f.panelTop.BackgroundImage = pnlToBackGroundImage;
                }
                else
                {
                    f.Tag = f.BackColor;
                    f.panelTop.Tag = f.panelTop.BackgroundImage;
                    f.panelTop.BackgroundImage = null;
                    f.dgvRedemption.BackgroundColor = f.BackColor = Color.Gray;
                }
            }

            log.LogMethodExit();
        }

        private void ShowHideAddScreenButton()
        {
            log.LogMethodEntry();
            string screenNumber = GetCurrentScreenNumber;
            if (showAllRedemptionScreens)
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                if (mdiParent != null)
                {
                    foreach (frmScanAndRedeem frm in mdiParent.GetOpenScanRedeemChildForms(this.GetScreenUserLoginId))
                    {
                        ShowHideAddScreenButton(frm, screenNumber);
                    }
                }
            }
            else
            {
                ShowHideAddScreenButton(this, screenNumber);
            }
            log.LogMethodExit();
        }

        private Control ShowHideAddScreenButton(frmScanAndRedeem frm, string screenNUmber)
        {
            log.LogMethodEntry(screenNUmber);
            Control c = frm.Controls.Find("btnAddScreen", true)[0];
            if ((string.IsNullOrEmpty(frm.lblScreenNumber.Text) == false && frm.lblScreenNumber.Text == screenNUmber)
                && (enableSingleUserMultiScreen || enableMultiUserMultiScreen) 
                )
            {
                c.Visible = true;
                c.Show();
            }
            else
            {
                c.Visible = false;
                c.Hide();
            }
            log.LogMethodExit();
            return c;
        }

        /// <summary>
        /// newRedemptionCheck
        /// </summary>
        private void NewRedemptionCheck()
        {
            log.LogMethodEntry();

            if (string.IsNullOrEmpty(LoggedInId))
            {
                CheckLoginRequired();
                throw new ApplicationException(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2686));// "Security Error. Log in to continue.");
            }

            if (_redemption == null || _redemption._RedemptionId > 0)
            {
                _redemption = new clsRedemption(_utilities, lblScreenNumber.Text);
                _redemption.authenticateManager += new clsRedemption.AuthenticateManager(DoManagerAuthenticationCheck);
                _redemption.launchFlagTicketReceiptUI += new clsRedemption.LaunchFlagTicketReceiptUI(LaunchFlagTicketReceiptUI);
                RefreshScreen();
                btnScanTicketOrGift.Tag = null;
                btnScanTicketOrGift.PerformClick();
            }

            DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "New Redemption"));

            log.LogMethodExit();
        }

        /// <summary>
        /// HandleCardRead
        /// </summary>
        /// <param name="cardNumber">cardNumber</param>
        /// <param name="readerDevice">readerDevice</param>
        private void HandleCardRead(string cardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(cardNumber, readerDevice);
            if (GetReloginAuthenticationInitiated() == false)
            {
                SetLastActivityTime();
                Card swipedCard = new Card(readerDevice, cardNumber, _utilities.ParafaitEnv.LoginID, _utilities);

                string message = "";
                if (!POSUtils.refreshCardFromHQ(ref swipedCard, ref message))
                {
                    DisplayMessageLine(message);
                    log.Info("Ends-HandleCardRead(" + cardNumber + ",readerDevice) as unable to refresh card from HQ error:" + message);
                    return;
                }

                DataTable dt = _utilities.executeDataTable(@"select * from SuspendedRedemption where Category = 'REDEMPTION-SUSPEND' and Value like @cardNum ",
                                                          new SqlParameter("@cardNum", "%<" + cardNumber + ">%"));
                if (dt.Rows.Count > 0)
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2685, cardNumber));// "Suspended Redemptions exist for the card: " + CardNumber + " redirecting ...");
                    LoadSuspendedRedemption(cardNumber);
                }
                else
                {
                    if (swipedCard != null)
                    {
                        AddCard(swipedCard);
                    }
                    else
                    {
                        message = MessageContainerList.GetMessage(_utilities.ExecutionContext, 110, cardNumber);
                        DisplayMessageLine(message, ERROR);
                    }
                }
                log.Debug("Ends-HandleCardRead(" + cardNumber + ",readerDevice)");
                SetLastActivityTime();
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2673), WARNING);//"Please unlock the screen first"
            }
            log.LogMethodExit();
        }
         
        private void AddCard(Card swipedCard)
        {
            log.LogMethodEntry(swipedCard);

            try
            {
                NewRedemptionCheck();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message);
                return;
            }

            string message = "";
            if (swipedCard != null && swipedCard.technician_card != 'Y')
            {
                if (_redemption.addCard(swipedCard.CardNumber, ref message))
                {
                    RefreshScreen();
                }
                if (_redemption.getETickets() > 0)
                {
                    btnScanTicketOrGift.Tag = "T";
                    btnScanTicketOrGift.PerformClick();
                }
                DisplayMessageLine(message, WARNING);
            }
            else
            {
                LauncNewUserForStaffCard(swipedCard);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// process Barcode
        /// </summary>
        /// <param name="barCode">barCode</param>
        private void ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);
            if (GetReloginAuthenticationInitiated() == false)
            {
                string screenNumber = "0";
                SetLastActivityTime();
                switch (barCode)
                {
                    case "NEWRD": btnNew.PerformClick(); return;
                    case "SCTKT": btnScanTicketOrGift.Tag = null; btnScanTicketOrGift.PerformClick(); return;
                    case "SCGFT": btnScanTicketOrGift.Tag = "T"; btnScanTicketOrGift.PerformClick(); return;
                    case "MNLTK": btnAddManual.PerformClick(); return;
                    case "CHQTY": ChangeQuantity(); return;
                    case "SAVER": btnSave.PerformClick(); return;
                    case "SPNDR": btnSuspend.PerformClick(); return;
                    case "PRINT": btnPrint.PerformClick(); return;
                    case "TRNIN": btnTurnIn.PerformClick(); return;
                    case "SERCH": btnSearch.PerformClick(); return;
                    case "LDTKT": btnLoadTickets.PerformClick(); return;
                    case "FLGTR": btnFlagTicketReceipt.PerformClick(); return;
                    case "OKKEY":
                        {
                            if (NumberPadVarPanel != null && NumberPadVarPanel.Visible)
                            {
                                numPad.handleaction("OK");
                            }
                        }
                        return;
                    case "CANCL":
                        {
                            if (NumberPadVarPanel != null && NumberPadVarPanel.Visible)
                            {
                                numPad.handleaction("Cancel");
                                NumberPadVarPanel.Visible = false;
                            }
                        }
                        return;
                    case "0DGIT":
                    case "1DGIT":
                    case "2DGIT":
                    case "3DGIT":
                    case "4DGIT":
                    case "5DGIT":
                    case "6DGIT":
                    case "7DGIT":
                    case "8DGIT":
                    case "9DGIT":
                        {
                            if (NumberPadVarPanel != null && NumberPadVarPanel.Visible)
                            {
                                numPad.GetKey(barCode[0]);
                            }
                        }
                        return;
                    case SCANCODEADDUSER:
                        AddNewUser();
                        return;
                    case SCANCODEADDSCRN:
                        AddScreenForCurrentUser();
                        return;
                    case SCANCODECLOSESCRN:
                        if (OkayToDiscardUnsavedRedemption(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Close Redemption")))
                        {
                            CloseCurrentForm();
                        }
                        return;
                    case SCANCODESCREEN1:
                        screenNumber = "1";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN2:
                        screenNumber = "2";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN3:
                        screenNumber = "3";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN4:
                        screenNumber = "4";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN5:
                        screenNumber = "5";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN6:
                        screenNumber = "6";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN7:
                        screenNumber = "7";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                    case SCANCODESCREEN8:
                        screenNumber = "8";
                        SetAsActiveScreenForCurrentUser(screenNumber);
                        return;
                }

                SetLastActivityTime();
                if (barCode.StartsWith("RDSPND"))
                {
                    if (_redemption.productList.Count > 0 || _redemption.getTotalTickets() > 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2681), WARNING);//"REDEMPTION IN PROGRESS. CLEAR OR SAVE TO PROCEED"
                        log.Warn("(" + barCode + ") as REDEMPTION IN PROGRESS. CLEAR OR SAVE TO PROCEED");
                        return;
                    }

                    _redemption = new clsRedemption(_utilities, lblScreenNumber.Text);
                    _redemption.authenticateManager += new clsRedemption.AuthenticateManager(DoManagerAuthenticationCheck);
                    _redemption.launchFlagTicketReceiptUI += new clsRedemption.LaunchFlagTicketReceiptUI(LaunchFlagTicketReceiptUI);
                    RefreshScreen();
                    string lclMessage = "";
                    if (!_redemption.retrieveSuspended(barCode, ref lclMessage))
                    {
                        RefreshSuspended();
                        DisplayMessageLine(lclMessage, ERROR);
                        log.Error("(" + barCode + ") -RDSPND- unable to retrieveSuspended  error " + lclMessage);
                        return;
                    }
                    checkApplicableCurrenyRule = true;
                    RefreshScreen();
                    log.Info("(" + barCode + ") -RDSPND- ");
                    return;
                }
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmRedemptionCurrency" && f.Owner == this)
                    {
                        f.Close();
                        break;
                    }
                }

                try
                {
                    NewRedemptionCheck();
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                    return;
                }

                string message = "";

                SetLastActivityTime();
                if (btnScanTicketOrGift.Tag != null) // ticket scan mode
                {
                    bool isTicket = false;
                    bool showTicketReceiptError = false;
                    try
                    {
                        TicketStationFactory ticketStationFactory = new TicketStationFactory();
                        TicketStationBL ticketStationBL = ticketStationFactory.GetTicketStationObject(barCode);
                        if (ticketStationBL != null)
                        {
                            if (ticketStationBL.BelongsToThisStation(barCode) && ticketStationBL.ValidCheckBit(barCode))
                            {
                                isTicket = true;
                            }
                        }
                        else
                        {
                            showTicketReceiptError = true;
                            //log.Error("Unable to find the matching station");
                        }

                    }
                    catch (Exception ex)
                    {
                        POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));
                        isTicket = false;
                    }

                    if (isTicket)
                    {
                        if (_redemption.addScanTickets(barCode, ref message))
                        {
                            RefreshScreen();
                            DisplayMessageLine(message);
                            log.Info("(" + barCode + ") -addScanTickets- ticket scan mode" + message);
                        }
                        else
                            DisplayMessageLine(message, WARNING);
                        log.Warn("(" + barCode + ") -addScanTickets- ticket scan mode unable to add scanned ticket error " + message);
                    }
                    else
                    {
                        try
                        {
                            checkApplicableCurrenyRule = false;
                            if (_redemption.addRedemptionCurrency(barCode, "", ref message)) // check if it is redemption currency
                            {
                                showTicketReceiptError = false;
                                DisplayRedemptionCurrency(barCode, "", ref message);
                            }
                            else
                            {
                                // neither ticket receipt nor currency
                                if (_redemption.addGift(barCode, 'B', ref message))
                                {
                                    showTicketReceiptError = false;
                                    RefreshScreen();
                                    DisplayMessageLine(message);
                                    log.Info("(" + barCode + ") -addGift- ticket scan mode " + message);
                                    lastScanObject.BarCode = barCode;
                                    lastScanObject.isCurrency = false;
                                }
                                else
                                {
                                    DisplayMessageLine(message, WARNING);
                                    log.Warn("(" + barCode + ") -addGift- ticket scan mode error " + message);
                                }
                            }

                            if (showTicketReceiptError)
                            {
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(2321),
                                                            MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));
                                log.Error("Unable to find the matching station");
                            }
                        }
                        catch (Exception ex)
                        {
                            RefreshScreen();
                            DisplayMessageLine(ex.Message, WARNING);
                        }
                    }
                }
                else // gift scan mode
                {
                    SetLastActivityTime();
                    if (_redemption.addGift(barCode, 'B', ref message))
                    {
                        RefreshScreen();
                        DisplayMessageLine(message);
                        log.Info("(" + barCode + ") -addGift- gift scan mode " + message);
                        lastScanObject.BarCode = barCode;
                        lastScanObject.isCurrency = false;
                    }
                    else
                    {
                        try
                        {
                            checkApplicableCurrenyRule = false;
                            if (_redemption.addRedemptionCurrency(barCode, "", ref message)) // check if it is redemption currency
                            {
                                DisplayRedemptionCurrency(barCode, "", ref message);
                            }
                            else
                            {
                                DisplayMessageLine(message, WARNING);
                                log.Warn("(" + barCode + ") -addGift- gift scan mode error " + message);
                            }
                        }
                        catch (Exception ex)
                        {
                            DisplayMessageLine(ex.Message, WARNING);
                            log.Warn("(" + barCode + ") -error " + message);
                        }
                    }
                }
                SetLastActivityTime();
            }
            else
            {
                if (barCode == SCANCODEADDUSER)
                {
                    AddNewUser();
                }
                else if (barCode == SCANCODECLOSESCRN)
                {
                    ForceCloseCurrentUserScreens();
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2673), WARNING);//"Please unlock the screen first"
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Display Redemption Currency
        /// </summary>
        /// <param name="barCode">barCode</param>
        /// <param name="currencyName">currencyName</param>
        /// <param name="message">message</param>
        private void DisplayRedemptionCurrency(string barCode, string currencyName, ref string message)
        {
            log.LogMethodEntry(barCode, currencyName, message);
            SetLastActivityTime();
            checkApplicableCurrenyRule = true;
            RefreshScreen();
            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(machineUserContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> searchParam = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            if (!string.IsNullOrEmpty(barCode))
            {
                searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.BARCODE, barCode));
            }
            if (!string.IsNullOrEmpty(currencyName))
            {
                searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.CURRENCY_NAME, currencyName));
            }

            searchParam.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            List<RedemptionCurrencyDTO> redemptionCurrencyDTOList = redemptionCurrencyList.GetAllRedemptionCurrency(searchParam);
            if (redemptionCurrencyDTOList != null && redemptionCurrencyDTOList.Count > 0)
            {
                if (String.IsNullOrEmpty(barCode))
                {
                    barCode = redemptionCurrencyDTOList[0].BarCode;
                }
                if (redemptionCurrencyDTOList[0].ShowQtyPrompt)
                {
                    frmRedemptionCurrency redemptionCurrencyDetails;
                    Boolean isCurrencyFormOpen = false;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f.Name == "frmRedemptionCurrency" && f.Owner == this)
                        {
                            f.Visible = false;
                            f.Visible = true;
                            isCurrencyFormOpen = true;
                            break;
                        }
                    }
                    if (!isCurrencyFormOpen)
                    {
                        redemptionCurrencyDetails = new frmRedemptionCurrency(_utilities, _redemption.currencyList);
                        redemptionCurrencyDetails.Owner = this;
                        redemptionCurrencyDetails.setCurrencyRuleCallBack = new frmRedemptionCurrency.SetCurrencyRuleCheckFlagDelegate(this.SetCheckApplicationCurrencyRule);
                        redemptionCurrencyDetails.setRefreshCallBack = new frmRedemptionCurrency.RefreshMethodDelegate(this.RefreshScreen);
                        redemptionCurrencyDetails.setProcessCmdKeyCallBack = new frmRedemptionCurrency.ProcessCmdKeyMethodDelegate(this.ProcessCmdKey);
                        redemptionCurrencyDetails.setShortCutKeyCallBack = new frmRedemptionCurrency.ShortCutKeyMethodDelegate(this.FrmScanAndRedeem_KeyUp);
                        redemptionCurrencyDetails.SetLastActivityTime += new frmRedemptionCurrency.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                        redemptionCurrencyDetails.Show();
                        if ((Application.OpenForms[MDIFORM].MdiChildren.Length - 1) == 1)
                        {
                            redemptionCurrencyDetails.StartPosition = FormStartPosition.CenterScreen;
                            redemptionCurrencyDetails.Location = new Point(redemptionCurrencyDetails.Location.X, this.dgvRedemption.Location.Y);
                        }
                        else
                            redemptionCurrencyDetails.Location = new Point((this.Location.X + (this.panel1.Width - redemptionCurrencyDetails.Width)), this.dgvRedemption.Location.Y);

                    }
                }
            }
            DisplayMessageLine(message);
            log.Info("(" + barCode + ") " + message);
            lastScanObject.BarCode = barCode;
            lastScanObject.isCurrency = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// SetCheckApplicationCurrencyRule
        /// </summary>
        private void SetCheckApplicationCurrencyRule()
        {
            log.LogMethodEntry(checkApplicableCurrenyRule);
            checkApplicableCurrenyRule = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// btnAddScreen_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnAddScreen_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AddScreenForCurrentUser();
            log.LogMethodExit();
        }

        private void AddNewUser()
        {
            log.LogMethodEntry();
            if (enableSingleUserMultiScreen == false)
            {
                if (Application.OpenForms["formLogin"] != null)
                {
                    log.LogMethodExit("login form is already launched");
                    return;
                }
                frmRedemptionScreenBanner frm = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
                frm.AddUser();
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2692), WARNING);//"Sorry, cannot add new user in single user mode"
            }

            log.LogMethodExit();
        }
        private void AddScreenForCurrentUser()
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            HideNumberPad();
            try
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                int childCount = mdiParent.GetOpenScanRedeemChildFormCount();
                if (childCount == 8)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2671),
                                               MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));// "Cannot launch more than 8 screens");
                    log.Info("Cannot launch more than 8 screens ");
                    return;
                }

                Security.User user = null;
                if (enableSingleUserMultiScreen == false)
                {
                    if (enableMultiUserMultiScreen == false)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2684),
                                                    MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));// "Cannot launch new screen, multi user, multi screen is not enabled");
                        log.LogMethodExit("Cannot launch new screen, multi user, multi screen is not enabled");
                        return;
                    }
                    else
                    {
                        user = _user;
                    }
                }
                else
                {
                    user = POSStatic.LoggedInUser;
                }
                SetNewScreenForLoggerdInUser(user.LoginId);
                frmScanAndRedeem frs = new frmScanAndRedeem(user);
                frs.MdiParent = mdiParent;
                frs.Show();
                frs.StopDevicesOnAllScreens();
                frs.ActivateDevicesOnAllActiveScreens();
                //frs.Visible = false;
                //frs.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1824, ex.Message,
                                            MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber)));
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        internal void ResizeMdiChildren()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = GetScanAndRedeemFormCount(mdiParent);
            int width, height;
            height = GetScreenHeight(childCount);
            width = GetScreenWidth(childCount);
            this.Size = new Size(width, height);
            int i = 0;
            List<frmScanAndRedeem> openForms = mdiParent.GetOpenScanRedeemChildForms();
            if (openForms != null && openForms.Any())
            {
                openForms = (from list in openForms orderby list.deviceAddress, list.GetCurrentScreenNumber select list).ToList();// openForms.OrderBy(frm => frm.deviceAddress).ThenBy(frm =>frm.GetCurrentScreenNumber).ToList();
            }
            string previousUser = string.Empty;
            foreach (frmScanAndRedeem f in openForms)
            {
                f.Size = new Size(width, height);
                string screenNumber = mdiParent.GetActiveScreenNumber(f._user.LoginId);
                //if (showAllRedemptionScreens == false)
                //{
                //    i = mdiParent.GetUserScreenPosition(f._user.LoginId);
                //}
                Control c = ShowHideAddScreenButton(f, screenNumber);

                if (childCount > 4)
                {
                    f.Controls.Add(c.Parent);
                    c.Parent.Top = height - panelButtons.Height - lblMessage.Height - c.Parent.Height - 10;
                    c.Parent.Left = width - c.Parent.Width - 2;
                    c.Parent.BringToFront();
                    f.Controls.Find("btnTurnIn", true)[0].Visible = false;
                    f.Controls.Find("btnAddManual", true)[0].Visible = false;
                    f.Controls.Find("btnProductSearch", true)[0].Visible = false;
                    f.Controls.Find("btnLoadTickets", true)[0].Visible = false;
                    f.Controls.Find("btnFlagTicketReceipt", true)[0].Visible = false;
                    f.Controls.Find("btnMoreOptions", true)[0].Visible = true;
                    if (childCount > 6)
                    {
                        f.Controls.Find("btnScanTicketOrGift", true)[0].Visible = false;
                        f.Controls.Find("btnSuspend", true)[0].Visible = false;
                        f.Controls.Find("btnMoreOptions", true)[0].Location = f.Controls.Find("btnSuspend", true)[0].Location;

                        Control lblBalanceCtrl = f.Controls.Find("lblBalance", true)[0];
                        lblBalanceCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        Control lblRedeemedCtrl = f.Controls.Find("lblRedeemed", true)[0];
                        lblRedeemedCtrl.Font = lblBalanceCtrl.Font;

                        Control lblTotalTicketsCtrl = f.Controls.Find("lblTotalTickets", true)[0];
                        lblTotalTicketsCtrl.Font = lblBalanceCtrl.Font;
                    }
                    else
                    {
                        f.Controls.Find("btnScanTicketOrGift", true)[0].Visible = true;
                        f.Controls.Find("btnSuspend", true)[0].Visible = true;
                        f.Controls.Find("btnMoreOptions", true)[0].Location = f.Controls.Find("btnAddManual", true)[0].Location;

                        Control lblBalanceCtrl = f.Controls.Find("lblBalance", true)[0];
                        lblBalanceCtrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                        Control lblRedeemedCtrl = f.Controls.Find("lblRedeemed", true)[0];
                        lblRedeemedCtrl.Font = lblBalanceCtrl.Font;

                        Control lblTotalTicketsCtrl = f.Controls.Find("lblTotalTickets", true)[0];
                        lblTotalTicketsCtrl.Font = lblBalanceCtrl.Font;
                    }
                    f.lblCardNumber.Visible = false;
                }
                else
                {
                    f.lblCardNumber.Visible = true;
                    f.Controls.Find("panelButtons", true)[0].Controls.Add(c.Parent);
                    c.Parent.Top = 3;
                    c.Parent.Left = width - c.Parent.Width - 2;
                    if (childCount == 1)
                    {
                        f.Controls.Find("btnTurnIn", true)[0].Visible = true;
                        //f.Controls.Find("btnAddManual", true)[0].Visible = true;
                        f.Controls.Find("btnAddManual", true)[0].Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
                        f.Controls.Find("btnProductSearch", true)[0].Visible = true;
                        f.Controls.Find("btnLoadTickets", true)[0].Visible = true;
                        f.Controls.Find("btnFlagTicketReceipt", true)[0].Visible = true;
                        f.Controls.Find("btnMoreOptions", true)[0].Visible = false;
                    }
                    else if (childCount >= 2 && childCount <= 4)
                    {
                        f.Controls.Find("btnTurnIn", true)[0].Visible = false;
                        //f.Controls.Find("btnAddManual", true)[0].Visible = true;
                        f.Controls.Find("btnAddManual", true)[0].Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
                        f.Controls.Find("btnLoadTickets", true)[0].Visible = false;
                        f.Controls.Find("btnFlagTicketReceipt", true)[0].Visible = false;
                        f.Controls.Find("btnProductSearch", true)[0].Visible = false;
                        f.Controls.Find("btnMoreOptions", true)[0].Visible = true;
                        f.Controls.Find("btnMoreOptions", true)[0].Location = f.Controls.Find("btnLoadTickets", true)[0].Location;
                    }
                }

                f.panel4.Location = new Point(f.panelTop.Location.X + (f.panelTop.Width - f.panel4.Width - 4), f.panel4.Location.Y);
                f.lblScreenNumber.Location = new Point(f.panel4.Location.X - f.lblScreenNumber.Width - 2, f.lblScreenNumber.Location.Y);

                f.UpdateMoreMenuDetails(childCount);
                if (showAllRedemptionScreens)
                {
                    i++;
                }
                else
                {
                    if (f.GetScreenUserLoginId != previousUser)
                    {
                        previousUser = f.GetScreenUserLoginId;
                        //    f.Focus();
                        i++;
                    }
                    if (mdiParent.IsActiveScreen(f.GetScreenUserLoginId, f.GetCurrentScreenNumber))
                    {
                        f.Show();
                        //f.Focus();
                    }
                }
                f.SetLocationAndChildSize(i, childCount, width, height);

            }
            log.LogMethodExit();
        }

        private void SetLocationAndChildSize(int i, int childCount, int width, int height)
        {
            log.LogMethodEntry(i, childCount, width, height);
            int screenYCoordinates = bannerHeight + 5;
            if (i == 1)
            {
                this.Location = new Point(2, screenYCoordinates);
            }
            else if (i == 2)
            {
                this.Location = new Point(width + 4, screenYCoordinates);
            }
            else if (i == 3)
            {
                if (childCount <= 4)
                    this.Location = new Point(2, height + screenYCoordinates + 2);
                else
                    this.Location = new Point(width * 2 + 4, screenYCoordinates);
            }
            else if (i == 4)
            {
                if (childCount <= 4)
                    this.Location = new Point(width + 4, (height + screenYCoordinates) + 2);
                else if (childCount <= 6)
                {
                    this.Location = new Point(2, (height + screenYCoordinates) + 2);
                }
                else
                {
                    this.Location = new Point(width * 3 + 4, screenYCoordinates);
                }
            }
            else if (i == 5)
            {
                if (childCount <= 6)
                {
                    this.Location = new Point(width + 4, (height + screenYCoordinates) + 2);
                }
                else
                {
                    this.Location = new Point(2, (height + screenYCoordinates) + 2);
                }
            }
            else if (i == 6)
            {
                if (childCount <= 6)
                {
                    this.Location = new Point(width * 2 + 4, (height + screenYCoordinates) + 2);
                }
                else
                {
                    this.Location = new Point(width + 4, (height + screenYCoordinates) + 2);
                }
            }
            else if (i == 7)
            {
                this.Location = new Point(width * 2 + 4, (height + screenYCoordinates) + 2);
            }
            else if (i == 8)
            {
                this.Location = new Point(width * 3 + 4, (height + screenYCoordinates) + 2);
            }
            if (this.OwnedForms.Count() > 0)
            {
                foreach (Form ownedForm in this.OwnedForms)
                {
                    ownedForm.Location = this.Location;
                    ownedForm.AutoScroll = true;
                    //if (ownedForm.Name == "frm_redemption")
                    ownedForm.Size = this.Size;

                    if (ownedForm.OwnedForms.Count() > 0)
                    {
                        foreach (Form ownedChildForm in ownedForm.OwnedForms)
                        {
                            ownedChildForm.Location = ownedForm.Location;
                            ownedChildForm.AutoScroll = true;
                            ownedChildForm.Size = ownedForm.Size;
                        }
                    }
                }
            }
            if (this.reloginPanel.Visible)
            {
                this.reloginPanel.Location = new Point(this.GetFormCenterX() - this.reloginPanel.Width / 2, this.GetFormCenterY() - this.reloginPanel.Height / 2);
            }
            log.LogMethodExit();
        }

        private int GetScreenWidth(int childCount)
        {
            log.LogMethodEntry(childCount);
            int width = 0;
            if (childCount == 1)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width - 10;
            }
            else if (childCount == 2)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 2 - 5;
            }
            else if (childCount == 3)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 2 - 5;
            }
            else if (childCount == 4)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 2 - 5;
            }
            else if (childCount == 5)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 3 - 5;
            }
            else if (childCount == 6)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 3 - 5;
            }
            else if (childCount == 7)
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 4 - 2;
            }
            else
            {
                width = Screen.PrimaryScreen.WorkingArea.Width / 4 - 2;
            }
            log.LogMethodExit(width);
            return width;
        }

        private int GetScreenHeight(int childCount)
        {
            log.LogMethodEntry(childCount);
            int heightAdjuster = 4;
            int height = 0;
            if (childCount == 1)
            {
                height = Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - 10;
            }
            else if (childCount == 2)
            {
                height = Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - 10;
            }
            else if (childCount == 3)
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            else if (childCount == 4)
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            else if (childCount == 5)
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            else if (childCount == 6)
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            else if (childCount == 7)
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            else
            {
                height = (Screen.PrimaryScreen.WorkingArea.Height - bannerHeight - heightAdjuster) / 2 - 5;
            }
            log.LogMethodExit(height);
            return height;
        }

        private void btnNew_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnNew.BackgroundImage = Properties.Resources.NewTrxPressed;
            log.LogMethodExit();
        }

        private void btnNew_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnNew.BackgroundImage = Properties.Resources.NewTrx;
            log.LogMethodExit();
        }

        private void btnSave_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnSave.BackgroundImage = Properties.Resources.OrderSavePressed;
            log.LogMethodExit();
        }

        private void btnSave_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnSave.BackgroundImage = Properties.Resources.OrderSave;
            log.LogMethodExit();
        }

        private void btnSuspend_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.OrderSuspendPressed;
            log.LogMethodExit();
        }

        private void btnSuspend_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.OrderSuspend;
            log.LogMethodExit();
        }

        private void btnScanTicketOrGift_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            if (senderBtn.Tag == null)
            {
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanGiftPreseed;
            }
            else
            {
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanTicketPressed;
                senderBtn.Tag = "T";
                btnScanTicketOrGift.Tag = "T";
            }
            log.LogMethodExit();
        }

        private void btnScanTicketOrGift_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            if (senderBtn.Tag == null)
            {
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanGift;
            }
            else
            {
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanTicket;
                senderBtn.Tag = "T";
                btnScanTicketOrGift.Tag = "T";
            }
            log.LogMethodExit();
        }

        private void btnScanTicketOrGift_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            Button senderBtn = (Button)sender;
            if (senderBtn.Tag == null)
            {
                btnScanTicketOrGift.Tag = "T";
                senderBtn.Tag = "T";
                senderBtn.Text = btnScanTicketOrGift.Text = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Scan Ticket");
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanTicket;
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Scanning Tickets"));
                log.Info("btnScanTicketOrGift_Click() - Scanning Tickets");
            }
            else
            {
                btnScanTicketOrGift.Tag = null;
                senderBtn.Tag = null;
                senderBtn.Text = btnScanTicketOrGift.Text = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Scan Gift");
                senderBtn.BackgroundImage = btnScanTicketOrGift.BackgroundImage = Properties.Resources.ScanGift;
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Scanning Gifts"));
                log.Info("btnScanTicketOrGift_Click() - Scanning Gifts");
            }
            log.LogMethodExit();
        }
        private void btnLoadTickets_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            int ticketsToLoad = 0;
            try
            {
                NewRedemptionCheck();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, WARNING);
                return;
            }
            if (_redemption.cardList.Count <= 0)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 459));
                return;
            }

            if (_redemption.cardList.Count > 1)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1376));//message=Tickets can be loaded to the single card at a time
                return;
            }
            if (_redemption.productList.Count > 0)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1377));//message=load ticket cannot be performed during gift redemption
                return;
            }

            ticketsToLoad = _redemption.getManualTickets() + _redemption.getCurrencyTickets();
            foreach (clsRedemption.clsScanTickets item in _redemption.scanTicketList)
            {
                if (_redemption.GetScanedReciptStatus(item.barCode))
                {
                    ticketsToLoad += item.Tickets;
                }
                else
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 112));
                    log.Debug("Ends LoadTickets_Click (" + item.barCode + ")");
                    return;
                }
            }

            if (ticketsToLoad > 0)
            {
                //&1 Tickets will be loaded to the card
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2683, ticketsToLoad.ToString()),
                                                    MessageContainerList.GetMessage(_utilities.ExecutionContext, "Load Tickets")
                                                    + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber),
                                                    System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                {
                    log.Debug("Ends LoadTickets_Click");
                    return;
                }

                try
                {
                    _redemption.LoadTicketsToCard(ticketsToLoad);
                    DisplayMessageLine(ticketsToLoad.ToString() + " " + MessageContainerList.GetMessage(_utilities.ExecutionContext, 1381));//message = Tickets loaded successfully
                    RefreshScreen();
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1390) + ": " + ticketsToLoad.ToString() + " - " + ex.Message);//message = Error loading Tickets
                }
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2691));//Please add tickets to proceed with Load Ticket
                return;
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        /// <summary>
        /// refreshScreen
        /// </summary>
        private void RefreshScreen()
        {
            log.LogMethodEntry();
            lblBalance.Text = lblRedeemed.Text = lblTotalTickets.Text = "0";
            lblCardNumber.Text = "";
            ApplyCurrencyRules();

            int total = _redemption.getETickets() + _redemption.getManualTickets() + _redemption.getPhysicalTickets() + _redemption.getCurrencyTickets();
            int redeemed = _redemption.getTotalRedeemed();
            lblBalance.Text = (total - redeemed).ToString();
            lblRedeemed.Text = redeemed.ToString();
            lblTotalTickets.Text = total.ToString();

            if (_redemption.cardList.Count > 0)
            {
                lblCardNumber.Text = string.IsNullOrEmpty(_redemption.cardList[0].customerName) ? _redemption.cardList[0].cardNumber : _redemption.cardList[0].customerName;
            }
            frmRedemptionScreenBanner mdiParent = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
            if (mdiParent != null)
            {
                mdiParent.UpdateScreenTileDetails(_user.LoginId, GetCurrentScreenNumber, (_redemption.cardList != null && _redemption.cardList.Any() ? _redemption.cardList[0].cardNumber : ""),
                                                                                        (_redemption.cardList != null && _redemption.cardList.Any() ? _redemption.cardList[0].customerName : ""),
                                                                                        (total - redeemed));
            }

            dgvRedemption.Rows.Clear();

            int i = 0;
            foreach (clsRedemption.clsProducts item in _redemption.productList)
            {
                dgvRedemption.Rows.Add("", 'X', item.productId, item.Quantity, item.productName, item.priceInTickets, item.Quantity * item.priceInTickets);
                dgvRedemption.Rows[i++].ReadOnly = true;
            }

            log.LogMethodExit();
        }

        private void ApplyCurrencyRules()
        {
            log.LogMethodEntry();
            try
            {
                if (checkApplicableCurrenyRule == true)
                {
                    if (redemptionCurrencyRuleBLList != null && redemptionCurrencyRuleBLList.Count > 0)
                    {
                        _redemption.ApplyCurrencyRule(redemptionCurrencyRuleBLList);
                        checkApplicableCurrenyRule = false;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// btnNew_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            if (OkayToDiscardUnsavedRedemption(MessageContainerList.GetMessage(_utilities.ExecutionContext, "New Redemption")))
            {
                _redemption = null;

                try
                {
                    NewRedemptionCheck();
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                }
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }


        private void CheckLoginRequired()
        {
            log.LogMethodEntry();

            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "REQUIRE_LOGIN_FOR_EACH_TRX", false) == true)
            {
                DisableFormElements();
                StopScreenTimer();
                int frmCenterX = GetFormCenterX();
                int frmCenterY = GetFormCenterY();
                if (cardReader != null)
                {
                    cardReader.UnRegister();
                }
                Login.frmReloginUser loginCheck = new Login.frmReloginUser(_utilities, _user, cardReader, false);
                loginCheck.UserEventHandler += loginCheck_UserEventHandler;
                this.reloginPanel.Visible = true;
                this.reloginPanel.SuspendLayout();
                this.reloginPanel.Size = new Size(this.dgvRedemption.Width, this.dgvRedemption.Height);
                this.reloginPanel.Controls.Add(loginCheck);
                loginCheck.Show();
                SetReloginAuthenticationInitiated(true);
                DisableFormElements();
                this.reloginPanel.ResumeLayout(true);
                this.reloginPanel.Location = new Point(frmCenterX - this.reloginPanel.Width / 2, frmCenterY - this.reloginPanel.Height / 2);
                this.reloginPanel.BringToFront();
            }

            log.LogMethodExit();
        }

        void loginCheck_UserEventHandler(Security.User User)
        {
            log.LogMethodEntry(User);
            try
            {
                if (User == null)
                {
                    log.Info("User authentication failed");
                    LoggedInId = "";
                }
                else
                {
                    log.Info("User " + User.LoginId + " logged in");

                    if (!this.LoggedInId.Equals(User.LoginId))
                    {
                        _user = User;
                        DoUserLogin(ref _user);
                        _utilities.ParafaitEnv.Initialize();

                        LoggedInId = lblLoginId.Text = _utilities.ParafaitEnv.LoginID;

                        DisplayMessageLine("");
                    }
                }
                this.reloginPanel.Visible = false;
                if (cardReader != null)
                {
                    cardReader.Register(CardScanCompleteEventHandle);
                }
                SetReloginAuthenticationInitiated(false);
                StartScreenTimer();
                EnableFormElementsForSuccessfulLoginIfTimerIsNotOn();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message, ERROR);
                StartScreenTimer();
                SetReloginAuthenticationInitiated(false);
            }
            log.LogMethodExit(); ;
        }

        /// <summary>
        /// frmScanAndRedeem_FormClosed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmScanAndRedeem_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopScreenTimer();
            string screenNumber = lblScreenNumber.Text;
            string userLoginId = _user.LoginId;
            try
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                if (enableSingleUserMultiScreen == false)
                {
                    log.Info("Unregsiter on close " + screenNumber + " : " + userLoginId);
                    if (cardReader != null)
                    {
                        cardReader.UnRegister();
                        if (mdiParent != null && mdiParent.GetOpenScanRedeemChildFormCount(userLoginId) <= 1)
                        {
                            cardReader.Dispose();
                        }
                    }
                    if (barcodeScanner != null)
                    {
                        barcodeScanner.UnRegister();
                        if (mdiParent != null && mdiParent.GetOpenScanRedeemChildFormCount(userLoginId) <= 1)
                        {
                            barcodeScanner.Dispose();
                        }
                    }
                }
                else
                {
                    if (mdiParent != null && mdiParent.GetOpenScanRedeemChildFormCount(userLoginId) <= 1)
                    {
                        if (cardReader != null)
                        {
                            cardReader.UnRegister();
                            cardReader.Dispose();
                        }
                        if (barcodeScanner != null)
                        {
                            barcodeScanner.UnRegister();
                            barcodeScanner.Dispose();
                        }
                    }
                }

                this.isFormClosed = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                try
                {
                    frmRedemptionScreenBanner frmScreenBanner = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
                    if (frmScreenBanner != null)
                    {
                        frmScreenBanner.RemoveScreenNumberTile(userLoginId, screenNumber);
                        frmScreenBanner.SetAsActiveScreen(userLoginId, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (OkayToDiscardUnsavedRedemption(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Close Redemption")))
            {
                CloseCurrentForm();
            }
            log.LogMethodExit();
        }

        internal void CloseCurrentForm()
        {
            log.LogMethodEntry();
            string screenNumber = lblScreenNumber.Text;
            string userLoginId = _user.LoginId;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            mdiParent.RemoveUserScreenEntry(userLoginId, screenNumber);
            StopScreenTimer();
            Close();
            if (mdiParent.GetOpenScanRedeemChildFormCount() == 0)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false)
                    && this.LoggedInId != POSStatic.LoggedInUser.LoginId)
                {
                    mdiParent.forceLoginForTimeOut = true;
                }
                mdiParent.CloseAllChildForms();
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            try
            {
                NewRedemptionCheck();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, WARNING);
                return;
            }

            string message = "";
            if (!_redemption.redeemGifts(ref message))
            {
                DisplayMessageLine(message, ERROR);
                log.Error("error " + message);
            }
            else
            {
                DisplayMessageLine(message, WARNING);
                log.Info(message);
                RefreshSuspended();
                CheckLoginRequired();
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void dgvRedemption_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            if (e.RowIndex < 0)
            {
                log.Info("E.RowIndex < 0");
                return;
            }

            if (dgvRedemption.Columns[e.ColumnIndex].Name.Equals("dcRemove"))
            {
                _redemption.removeGift(dgvRedemption["dcProductId", e.RowIndex].Value);
                RefreshScreen();
            }
            else if (dgvRedemption.Columns[e.ColumnIndex].Name.Equals("dcQuantity"))
            {
                ChangeQuantity(e.RowIndex);
            }

            log.LogMethodExit();
        }

        void ChangeQuantity(int RowIndex = -1)
        {
            log.LogMethodEntry(RowIndex);
            if (RowIndex != -1 )
            {
                clsRedemption.clsProducts checkProd = _redemption.productList.Find(delegate (clsRedemption.clsProducts item) { return (item.productId == Convert.ToInt32(dgvRedemption["dcProductId", RowIndex].Value)); });
                if (checkProd != null)
                {
                    lastScanObject.BarCode = (checkProd.barCode == null || checkProd.barCode.ToString() == "") ? checkProd.code : checkProd.barCode;
                    lastScanObject.isCurrency = false;
                    ShowKeyPad();
                }
            }
            else
                ShowKeyPad();

            log.LogMethodExit();
        }

        NumberPad numPad = null;
        Panel NumberPadVarPanel;
        /// <summary>
        /// ShowKeyPad
        /// </summary>
        private void ShowKeyPad()
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            if (numPad == null)
            {
                numPad = new NumberPad("#0");
                NumberPadVarPanel = numPad.NumPadPanel();
                this.Controls.Add(NumberPadVarPanel);

                numPad.setReceiveAction = EventnumPadOKReceived;
                numPad.setKeyAction = EventnumPadKeyPressReceived;

                this.KeyPreview = true;

                this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
                NumberPadVarPanel.VisibleChanged += NumberPadVarPanel_VisibleChanged;
            }

            NumberPadVarPanel.Location = new Point((this.Width - NumberPadVarPanel.Width) / 2, (this.Height - NumberPadVarPanel.Height) / 2);
            numPad.GetKey('0');
            numPad.NewEntry = true;
            NumberPadVarPanel.Visible = true;
            NumberPadVarPanel.BringToFront();

            SetLastActivityTime();
            log.LogMethodExit();
        }


        void NumberPadVarPanel_VisibleChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (NumberPadVarPanel.Visible == false)
                lastScanObject.isManualTicket = false;
            log.LogMethodExit();
        }

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (GetReloginAuthenticationInitiated() == false)
            {
                SetLastActivityTime();
            }
            if (e.KeyChar == (char)Keys.Escape)
                NumberPadVarPanel.Visible = false;
            else
            {
                numPad.GetKey(e.KeyChar);
            }
            log.LogMethodExit();
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            string message = "";
            try
            {
                NewRedemptionCheck();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, WARNING);
                return;
            }

            if (lastScanObject.isManualTicket)
            {
                log.Info("lastScanObject is ManualTicket ");
                if (numPad.ReturnNumber > 0)
                {
                    //if (mgrApprovalRequired == "Y")
                    //{
                    if (mgrApprovalLimit > 0 && numPad.ReturnNumber > mgrApprovalLimit && _utilities.ParafaitEnv.ManagerId == -1)
                    {
                        //int mgrId = -1;
                        //string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
                        //POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                        if (!DoManagerAuthenticationCheck(ref _utilities.ParafaitEnv.ManagerId))//!Authenticate.Manager(ref _utilities.ParafaitEnv.ManagerId))
                        {
                            //POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                            DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1217));
                            log.Warn("As Manager Approval Required for this Task -Add Manual Ticket");
                            return;
                        }
                        _redemption.AddManualTicketMangerApprovalDetails = new Tuple<int, int, int>(POSStatic.ParafaitEnv.User_Id, Convert.ToInt32(numPad.ReturnNumber), _utilities.ParafaitEnv.ManagerId);
                        _utilities.ParafaitEnv.ManagerId = -1; //since there is not transaction level check for add manual we can reset here. Else reset it after transaction
                    }
                    //}
                    if (!_redemption.addManualTickets((int)numPad.ReturnNumber, ref message))
                    {
                        DisplayMessageLine(message, ERROR);
                        log.Error("AddManualTickets- error: " + message);
                    }
                    else
                    {
                        RefreshScreen();
                        DisplayMessageLine(message, WARNING);
                        log.Warn("AddManualTickets- " + message);
                    }
                }
            }
            else
            {
                log.Info("LastScanObject is Currency ");
                if (lastScanObject.isCurrency)
                    _redemption.updateCurrencyQuantity(lastScanObject.BarCode, (int)numPad.ReturnNumber, ref message);
                else
                    _redemption.updateQuantity(lastScanObject.BarCode, (int)numPad.ReturnNumber, ref message);

                RefreshScreen();
                DisplayMessageLine(message);
            }

            NumberPadVarPanel.Visible = false;
            SetLastActivityTime();
            log.LogMethodExit();
        }

        void EventnumPadKeyPressReceived()
        {
        }

        private void dgvRedemption_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            try
            {
                if (e.RowIndex >= 0
                         && dgvRedemption["dcProductId", e.RowIndex].Value == null
                         && dgvRedemption.Columns[e.ColumnIndex].Name.Equals("dcProductName")
                         && dgvRedemption["dcProductName", e.RowIndex].Value.ToString().Trim() != "")
                {
                    string message = "";

                    try
                    {
                        NewRedemptionCheck();
                    }
                    catch (Exception ex)
                    {
                        DisplayMessageLine(ex.Message);
                        return;
                    }

                    DataTable dt = _redemption.GetGiftDetails(dgvRedemption["dcProductName", e.RowIndex].Value.ToString(), 'D');
                    if (dt.Rows.Count == 0)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 111), ERROR);
                        log.Info("Product not found ");
                        return;
                    }
                    else
                    {
                        DataGridView dgv = new DataGridView();

                        dgv.LostFocus += (object s, EventArgs ea) =>
                            {
                                dgv.Visible = false;
                                this.Controls.Remove(dgv);
                            };

                        dgv.CellClick += (object se, DataGridViewCellEventArgs eva) =>
                            {
                                if (eva.RowIndex < 0)
                                {
                                    log.Info("Ends-dgvRedemption_CellValueChanged() as eva.RowIndex < 0");
                                    return;
                                }
                                string code = dgv.CurrentRow.Cells["Code"].Value.ToString();
                                dgv.Visible = false;
                                this.Controls.Remove(dgv);

                                if (!_redemption.addGift(code, 'C', ref message))
                                {
                                    DisplayMessageLine(message, ERROR);
                                    log.Info("AddGift with code" + code + " ,error" + message);
                                    return;
                                }
                                else
                                    RefreshScreen();
                                log.Info("AddGift Sucessfull with code " + code + "");
                            };

                        dgv.DataSource = dt;
                        this.Controls.Add(dgv);
                        dgv.BringToFront();
                        dgv.Focus();

                        dgv.BorderStyle = BorderStyle.None;
                        dgv.AllowUserToAddRows = false;
                        dgv.BackgroundColor = Color.White;
                        dgv.Columns["ProductId"].Visible =
                            dgv.Columns["Quantity"].Visible = false;
                        dgv.Columns["selectGiftMain"].Visible = false;
                        dgv.Columns["OriginalPriceInTickets"].Visible = false;
                        dgv.Columns["PriceInTickets"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
                        dgv.Font = new Font("Arial", 10, FontStyle.Regular);
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                        dgv.ReadOnly = true;
                        dgv.RowHeadersVisible = false;
                        dgv.AllowUserToResizeColumns = false;
                        dgv.MultiSelect = false;
                        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                        dgv.AutoSize = true;
                        int wid = dgv.Width;
                        dgv.AutoSize = false;
                        dgv.Size = new Size(wid, (dgv.Rows[0].Cells[0].Size.Height * (dgv.Rows.Count)) + dgv.ColumnHeadersHeight);

                        dgv.Location = new Point(dgvRedemption[0, 0].Size.Width, (dgvRedemption.Location.Y + (dgvRedemption.CurrentRow.Index + 2) * dgvRedemption.CurrentRow.Height));
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, WARNING);
                log.Fatal("Exception " + ex.Message);
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            try
            {
                NewRedemptionCheck();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message, WARNING);
                return;
            }

            string message = "";
            if (_redemption.suspend(ref message))
            {
                btnNew.PerformClick();
                DisplayMessageLine(message);
                log.Info("Redemption suspended " + message);
            }
            else
            {
                DisplayMessageLine(message, ERROR);
                log.Error("Suspend redemption error: " + message);
            }
            RefreshSuspended();
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            if (_redemption == null)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1410), WARNING);
                log.Warn("Please save before printing");
                return;
            }
            if (_redemption != null && _redemption._RedemptionId > 0)
            {
                PrintRedemptionReceipt.Print(_redemption._RedemptionId, lblScreenNumber.Text);
                log.Info("Print with Id" + _redemption._RedemptionId.ToString());
            }
            else if (_redemption != null && _redemption.productList.Count == 0)
            {
                try
                {
                    _redemption.RedeemTicketReceipt();
                    RefreshScreen();
                    _redemption = null;
                    btnNew.PerformClick();
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 122));
                }
                catch (Exception ex)
                {
                    DisplayMessageLine(ex.Message);
                    log.Fatal("Exception " + ex.Message);
                }
            }
            else
            {
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1410), WARNING);
                log.Warn("Please save before printing");
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void RefreshSuspended()
        {
            log.LogMethodEntry();
            btnShowSuspended.Text = _utilities.executeScalar("select count(1) from SuspendedRedemption where Category = 'REDEMPTION-SUSPEND' and Username = @loginId",
                                                              new System.Data.SqlClient.SqlParameter("@loginId", _utilities.ParafaitEnv.LoginID)).ToString();
            log.LogMethodExit();
        }

        private void btnSuspended_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            RefreshSuspended();
            frmSuspendedRedemptions f = new frmSuspendedRedemptions(_utilities, redemptionCurrencyRuleBLList);
            f.Location = this.Location;
            f.Width = this.Width;
            f.Height = this.Height;
            f.AutoScroll = true;
            f.Owner = this;
            f.setRetrievedParamsCallback = new frmSuspendedRedemptions.RetrievedIDDelegate(this.RetrieveRedemptionDetails);
            f.SetLastActivityTime += new frmSuspendedRedemptions.SetLastActivityTimeDelegate(this.SetLastActivityTime);
            f.Show();
            log.LogMethodExit();
        }

        /// <summary>
        /// loadSuspendedRedemption
        /// </summary>
        /// <param name="cardNumber"></param>
        private void LoadSuspendedRedemption(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            SetLastActivityTime();
            RefreshSuspended();
            frmSuspendedRedemptions f = new frmSuspendedRedemptions(_utilities, cardNumber, redemptionCurrencyRuleBLList);
            f.Location = this.Location;
            f.Width = this.Width;
            f.Height = this.Height;
            f.AutoScroll = true;
            f.Owner = this;
            f.setRetrievedParamsCallback = new frmSuspendedRedemptions.RetrievedIDDelegate(this.RetrieveRedemptionDetails);
            f.SetLastActivityTime += new frmSuspendedRedemptions.SetLastActivityTimeDelegate(this.SetLastActivityTime);
            f.Show();
            log.LogMethodExit();
        }

        /// <summary>
        /// retrieve Redemption Details
        /// </summary>
        /// <param name="id">id</param>
        private void RetrieveRedemptionDetails(object id)
        {
            log.LogMethodEntry(id);
            SetLastActivityTime();
            if (id != null)
            {
                if (_redemption._RedemptionId <= 0 && (_redemption.productList.Count > 0 || _redemption.getTotalTickets() > 0))
                {
                    lblMessage.Text = MessageContainerList.GetMessage(_utilities.ExecutionContext, 2681);// "REDEMPTION IN PROGRESS. CLEAR OR SAVE TO PROCEED";
                    log.Warn("REDEMPTION IN PROGRESS. CLEAR OR SAVE TO PROCEED");
                    return;
                }

                _redemption = new clsRedemption(_utilities, lblScreenNumber.Text);
                _redemption.authenticateManager += new clsRedemption.AuthenticateManager(DoManagerAuthenticationCheck);
                _redemption.launchFlagTicketReceiptUI += new clsRedemption.LaunchFlagTicketReceiptUI(LaunchFlagTicketReceiptUI);
                RefreshScreen();
                string lclMessage = "";
                if (!_redemption.retrieveSuspended(id.ToString(), ref lclMessage))
                {
                    RefreshSuspended();
                    DisplayMessageLine(lclMessage, ERROR);
                    log.Error("Unable to retrieve Suspended error: " + lclMessage);
                    return;
                }
                else
                {
                    DisplayMessageLine(lclMessage);
                    log.Info("Retrieved Suspended " + lclMessage);
                    checkApplicableCurrenyRule = true;
                    RefreshScreen();
                }
            }
            RefreshSuspended();
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnAddManual_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            try
            {
                if (string.IsNullOrEmpty(LoggedInId))
                {
                    CheckLoginRequired();
                    log.LogMethodExit();
                    return;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET").Equals("Y"))
                {
                    //string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
                    //POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                    if (!DoManagerAuthenticationCheck(ref _utilities.ParafaitEnv.ManagerId))
                    {
                        // POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 268));
                        log.LogMethodExit("Manager Approval Required for this Task -Add Manual Ticket");
                        return;
                    }
                    //POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                }

                lastScanObject.isManualTicket = true;
                ShowKeyPad();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message);
            }
            finally
            {
                _utilities.ParafaitEnv.ManagerId = -1;
                SetLastActivityTime();
            }
            log.LogMethodExit();
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
            log.LogMethodExit(message + "," + msgType);
        }

        private void btnAddManual_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Parafait_POS.Properties.Resources.AddTicketNormal;
            log.LogMethodExit();
        }

        private void btnAddManual_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Parafait_POS.Properties.Resources.AddTicketPressed;
            log.LogMethodExit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            if (keyData == Keys.F1)
            {
                frmRedemptionAdmin fra = new frmRedemptionAdmin(cardReader, barcodeScanner, _redemption, LoggedInId, lblScreenNumber.Text, _utilities);
                fra.Location = this.Location;
                fra.Width = this.Width;
                fra.Height = this.Height;
                fra.AutoScroll = true;
                fra.Owner = this;
                fra.setRefreshCallBack = new frmRedemptionAdmin.RefreshMethodDelegate(this.RefreshScreen);
                fra.setCurrencyRuleCallBack = new frmRedemptionAdmin.SetCurrencyRuleCheckFlagDelegate(this.SetCheckApplicationCurrencyRule);
                fra.SetLastActivityTime += new frmRedemptionAdmin.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                fra.Show();
                return true;
            }
            else if (keyData.Equals(Keys.Enter))
            { return true; }

            if (keyData.Equals(Keys.ControlKey | Keys.Control))
            { shortCutKeyValues = ""; }
            // Call the base class]
            log.LogMethodExit(base.ProcessCmdKey(ref msg, keyData));
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void lnkTotalTickets_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            try
            {
                if (string.IsNullOrEmpty(LoggedInId))
                {
                    CheckLoginRequired();
                    log.LogMethodExit();
                    return;
                }
                frmRedemptionAdmin fra = new frmRedemptionAdmin(cardReader, barcodeScanner, _redemption, LoggedInId, lblScreenNumber.Text, _utilities);
                fra.Location = this.Location;
                fra.Width = this.Width;
                fra.Height = this.Height;
                fra.AutoScroll = true;
                fra.Owner = this;
                fra.setRefreshCallBack = new frmRedemptionAdmin.RefreshMethodDelegate(this.RefreshScreen);
                fra.setCurrencyRuleCallBack = new frmRedemptionAdmin.SetCurrencyRuleCheckFlagDelegate(this.SetCheckApplicationCurrencyRule);
                fra.SetLastActivityTime += new frmRedemptionAdmin.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                fra.Show();
            }
            catch (Exception ex)
            {
                DisplayMessageLine(ex.Message);
                return;
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnTurnIn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            try
            {
                if (string.IsNullOrEmpty(LoggedInId))
                {
                    CheckLoginRequired();
                    log.LogMethodExit();
                    return;
                }
                frm_redemption f = new frm_redemption(_utilities, cardReader, barcodeScanner, LoggedInId, lblScreenNumber.Text);
                f.Location = this.Location;
                f.Width = this.Width;
                f.Height = this.Height;
                f.AutoScroll = true;
                f.Owner = this;
                f.SetLastActivityTime += new frm_redemption.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                f.Show();
                log.Debug("Ends-btnTurnIn_Click() ");
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-btnTurnIn_Click() due to exception " + ex.Message);
                DisplayMessageLine(ex.Message);
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnTurnIn_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.TurnInPressed;
            log.LogMethodExit();
        }

        private void btnTurnIn_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.TurnInNormal;
            log.LogMethodExit();
        }


        private void btnLoadTickets_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.LoadTicket_Pressed;
            log.LogMethodExit();
        }


        private void btnLoadTickets_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.LoadTicket_Normal;
            log.LogMethodExit();
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            fpnlMoreMenu.Visible = false;
            panelAddClose.Visible = false;
            pnlProductLookup.Visible = true;
            txt_proddesc.Clear();
            txtPriceInTicketMoreThan.Clear(); 
            txtPriceInTicketLessThan.Clear(); 
            txt_prodcode.Clear();
            txt_prodcode.Focus();
            SetHintTextForLessThanField();
            SetHintTextForMoreThanField();
            btnSearch_Click(null, null);
            log.LogMethodExit();
        }

        private void btnExitProductLookup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            pnlProductLookup.Visible = false;
            panelAddClose.Visible = true;
            log.LogMethodExit();
        }

        private void btnMoreOptions_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            HideNumberPad();
            Button btnMoreOptionObject = (Button)sender;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = GetScanAndRedeemFormCount(mdiParent);

            if (childCount <= 4)
            {
                fpnlMoreMenu.Location = new Point(btnMoreOptionObject.Location.X, this.dgvRedemption.Height - 10);
            }
            else if (childCount <= 6)
            {
                panelAddClose.SendToBack();
                fpnlMoreMenu.Location = new Point(btnMoreOptionObject.Location.X - (btnMoreOptionObject.Width), this.dgvRedemption.Height - btnMoreOptionObject.Height - 15);
            }
            else  //> 6
            {
                panelAddClose.SendToBack();
                fpnlMoreMenu.Location = new Point(btnSave.Location.X + (btnSave.Width / 3), this.dgvRedemption.Height - btnSave.Height - 15);
            }
            fpnlMoreMenu.Show();
            fpnlMoreMenu.Focus();
            fpnlMoreMenu.BringToFront();
            SetLastActivityTime();
            log.LogMethodExit();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                HideNumberPad();
                GiftSearch();
                PopulateGiftSearchDGV();
                SetLastActivityTime();
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message, WARNING);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message, ERROR);
            }
            log.LogMethodExit(); 
        }

        int GetGraceTickets(int Tickets)
        {
            log.LogMethodEntry(Tickets);
            int graceTicket = 0;
            if (POSStatic.REDEMPTION_GRACE_TICKETS > 0)
            {
                graceTicket = POSStatic.REDEMPTION_GRACE_TICKETS;
            }
            else if (POSStatic.REDEMPTION_GRACE_TICKETS_PERCENTAGE > 0)
            {
                graceTicket = Convert.ToInt32(POSStatic.REDEMPTION_GRACE_TICKETS_PERCENTAGE * Tickets / 100);
            }
            log.LogMethodExit(graceTicket);
            return graceTicket;
        }

        private void GiftSearch()
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            String prod_desc = "";
            String prod_code = "";

            string cmdText = "";
            cmdText = @"select top 500 Code, P.Description, round(PriceInTickets * @disc, 0) PriceInTickets, 'N' as selectGift, isnull(Quantity, 0) Quantity, P.ProductId
                              from Product P join products ps on p.ManualProductId = ps.product_id  
                                        left outer join Inventory I 
                                        on P.productId = I.ProductId
                                       and I.LocationId = (select isnull(pos.InventoryLocationId, p.outboundLocationId) 
                                                           from (select 1 a) v left outer join POSMachines pos
                                                           on POSMachineId = @posName)
									left outer join (select * 
													 from (
															select *, row_number() over(partition by productid order by productid) as num 
															from productbarcode 
															where BarCode = @bar_code and isactive = 'Y')v 
													 where num = 1) b on p.productid = b.productid 
                              where ((b.BarCode = @bar_code OR Code like @product_code OR P.ProductName like @product_code) 
                                      AND P.Description like @product_desc 
                                      AND ((@priceLessThan is null AND @priceMoreThan is null)
									        OR
                                           (@priceLessThan is not null AND @priceMoreThan is null AND PriceInTickets <= @priceLessThan)
                                            OR 
                                           (@priceLessThan is null AND @priceMoreThan is not null AND PriceInTickets >= @priceMoreThan)
                                            OR
                                           (@priceLessThan is not null AND @priceMoreThan is not null AND PriceInTickets >= @priceMoreThan and PriceInTickets <= @priceLessThan) 
										  )) 
                              and not exists (select 1 from redemptionCurrency rc where rc.productId = p.productId) 
                              AND (Quantity > 0 or @ignoreQtyCheck = 'Y') AND P.IsActive = 'Y' 
                              and P.IsRedeemable = 'Y'
							  and not exists (select 1 
											  from ProductsDisplayGroup pd , 
														   ProductDisplayGroupFormat pdgf,
														   POSProductExclusions ppe 
											  where ps.product_id = pd.ProductId 
											  and pd.DisplayGroupId = pdgf.Id 
											  and ppe.ProductDisplayGroupFormatId = pdgf.Id
											  and ppe.POSMachineId = @posName ) 
									and not exists (select 1 
													from ProductsDisplayGroup pd , 
														     ProductDisplayGroupFormat pdgf,
														     UserRoleDisplayGroupExclusions urdge , 
														     users u
													where  ps.product_id = pd.ProductId 
													and pd.DisplayGroupId = pdgf.Id 
													and urdge.ProductDisplayGroupId = pdgf.Id
                                                    and urdge.role_id = u.role_id
                                                    and u.loginId = @loginId)
                                  Order by PriceInTickets";

            prod_code = txt_prodcode.Text + "%";
            prod_desc = "%" + txt_proddesc.Text + "%";
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            sqlParamList.Add(new SqlParameter("@posName", _utilities.ParafaitEnv.POSMachineId));
            sqlParamList.Add(new SqlParameter("@disc", 1.0 - RedemptionDiscount));
            sqlParamList.Add(new SqlParameter("@bar_code", txt_prodcode.Text));
            sqlParamList.Add(new SqlParameter("@product_code", prod_code));
            sqlParamList.Add(new SqlParameter("@ignoreQtyCheck", _utilities.ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK));
            sqlParamList.Add(new SqlParameter("@product_desc", prod_desc));
            sqlParamList.Add(new SqlParameter("@loginId", _utilities.ParafaitEnv.LoginID));
            string sortOrder = string.Empty;
            int priceLessThan = 0;
            int priceMoreThan = 0;
            try
            {
                string spriceMoreThan = txtPriceInTicketMoreThan.Text.Trim().Replace(MessageContainerList.GetMessage(_utilities.ExecutionContext,"More than"),"");
                priceMoreThan = string.IsNullOrEmpty(spriceMoreThan) ? 0 : Convert.ToInt32(spriceMoreThan);
                string spriceLessThan = txtPriceInTicketLessThan.Text.Trim().Replace(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Less than"), "");
                priceLessThan = string.IsNullOrEmpty(spriceLessThan) ? 0 : Convert.ToInt32(spriceLessThan);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ValidationException(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1144, POSStatic.TicketTermVariant));
            }
            if (priceLessThan < 0 || priceMoreThan < 0)
            { 
                throw new ValidationException(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1144, POSStatic.TicketTermVariant));
            }
            if (priceLessThan != 0 && priceMoreThan != 0 && priceMoreThan > priceLessThan)
            {
                //'Please enter valid range for &1
                throw new ValidationException(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2831, POSStatic.TicketTermVariant));
            }
            try
            {
                if (priceMoreThan == 0)
                {
                    sqlParamList.Add(new SqlParameter("@priceMoreThan", DBNull.Value));
                }
                else
                { 
                    sqlParamList.Add(new SqlParameter("@priceMoreThan", priceMoreThan));
                    sortOrder = " desc";
                }

                if (priceLessThan == 0)
                {
                    sqlParamList.Add(new SqlParameter("@priceLessThan", DBNull.Value));
                }
                else
                {
                    priceLessThan = priceLessThan + GetGraceTickets(priceLessThan);
                    sqlParamList.Add(new SqlParameter("@priceLessThan", priceLessThan));
                    sortOrder = " desc";
                }                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                sqlParamList.Add(new SqlParameter("@price", DBNull.Value)); 
                sqlParamList.Add(new SqlParameter("@priceUpper", DBNull.Value)); 
            } 

            cmdText += sortOrder;
            DT_Search.Clear();
            DisplayMessageLine("");
            if (!String.IsNullOrEmpty(txt_prodcode.Text.Trim()) || !String.IsNullOrEmpty(txt_proddesc.Text.Trim()) 
                || !String.IsNullOrEmpty(txtPriceInTicketMoreThan.Text.Trim().Replace(MessageContainerList.GetMessage(_utilities.ExecutionContext, "More than"), ""))
                || !String.IsNullOrEmpty(txtPriceInTicketLessThan.Text.Trim().Replace(MessageContainerList.GetMessage(_utilities.ExecutionContext, "Less than"), "")))
            {
                DT_Search = _utilities.executeDataTable(cmdText, sqlParamList.ToArray());
                if (DT_Search.Rows.Count == 0)
                {
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1710), WARNING);
                }
            }
            else
            {
                DT_Search = _utilities.executeDataTable("Select Code, Description, 0 PriceInTickets, 'N' as selectGift, 0 as Quantity, P.ProductId from Product p where 1 = 2");
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1711), WARNING);
            }

            SetLastActivityTime();
            log.LogMethodExit();
        }

        /// <summary>
        /// populateGiftSearchDGV
        /// </summary>
        private void PopulateGiftSearchDGV()
        {

            log.LogMethodEntry();
            search_dgv.DataSource = DT_Search;
            search_dgv.Refresh();
            search_dgv.Columns["selectGift"].Visible = false;
            search_dgv.Columns["Code"].ReadOnly = true;
            search_dgv.Columns["Description"].ReadOnly = true;
            search_dgv.Columns["PriceInTickets"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            search_dgv.Columns["Quantity"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            search_dgv.Columns["PriceInTickets"].ReadOnly = true;
            search_dgv.Columns["PriceInTickets"].HeaderText = "Price In " + POSStatic.TicketTermVariant;
            search_dgv.Columns["Quantity"].ReadOnly = true;
            search_dgv.Columns["Quantity"].HeaderText = "Avl. Qty";
            search_dgv.Columns["productId"].Visible = false;
            search_dgv.Columns["Code"].DisplayIndex = 0;
            search_dgv.Columns["Description"].DisplayIndex = 1;
            search_dgv.Columns["PriceInTickets"].DisplayIndex = 2;
            search_dgv.Columns["Quantity"].DisplayIndex = 3;
            search_dgv.Columns["imgViewInventoryLocations"].DisplayIndex = 4;

            _utilities.setLanguage(search_dgv); 
            flpProducts.Controls.Clear();
            foreach (DataRow dr in DT_Search.Rows)
            {
                Button btnProduct = new Button();
                btnProduct.Name = dr["Code"].ToString();
                btnProduct.Text = dr["Description"].ToString();
                btnProduct.Click += btnProduct_Click;
                btnProduct.FlatStyle = FlatStyle.Flat;
                btnProduct.BackColor = Color.Transparent;
                btnProduct.ForeColor = btnSampleProduct.ForeColor;
                btnProduct.FlatAppearance.MouseDownBackColor = btnProduct.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnProduct.FlatAppearance.BorderSize = 0;
                btnProduct.BackgroundImage = btnSampleProduct.BackgroundImage;
                btnProduct.BackgroundImageLayout = btnSampleProduct.BackgroundImageLayout;
                btnProduct.MouseDown += btnProduct_MouseDown;
                btnProduct.MouseUp += btnProduct_MouseUp;
                btnProduct.Size = btnSampleProduct.Size;

                btnProduct.Tag = dr;

                flpProducts.Controls.Add(btnProduct);
            } 
            log.LogMethodExit();
        }

        /// <summary>
        /// btnProduct_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void btnProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button btn = sender as Button;
            DataRow dr = btn.Tag as DataRow;
            bool gift_availability = CheckGiftAvailability(btn.Name, true);
            string message = "";
            if (!gift_availability)
            {
                log.Info("Ends-btnProduct_Click() as Not enough quantity available for " + btn.Name + ". Please correct before saving, Insufficient Inventory");
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 116, btn.Name), WARNING);
            }
            else
            {
                string code = btn.Name;

                if (!_redemption.addGift(code, 'C', ref message))
                {
                    DisplayMessageLine(message, ERROR);
                    log.LogMethodExit("AddGift with code" + code + " ,error" + message);
                    return;
                }
                else
                {
                    RefreshScreen();
                    //"Redemption gift " + btn.Name + " successfully added."
                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2674, btn.Name), MESSAGE);
                }
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void UpdateRedeemed()
        {
            log.LogMethodEntry();
            int count = 0;
            int giftCount = 0;
            for (int i = 0; i < dgvRedemption.Rows.Count - 1; i++)
            {
                if (dgvRedemption["PriceInTickets", i].Value != null)
                {
                    if (dgvRedemption["PriceInTickets", i].Value.ToString() != "")
                    {
                        count += Convert.ToInt32(dgvRedemption.Rows[i].Cells["PriceInTickets"].Value);
                        giftCount++;
                    }
                }
            }
            log.LogMethodExit();
        }

        private bool CheckGiftAvailability(String prod_id, bool BeforeAdding)
        {
            log.LogMethodEntry(prod_id, BeforeAdding);
            SetLastActivityTime();
            if (_utilities.ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK.Equals("Y"))
            {
                log.LogMethodExit("ALLOW_TRANSACTION_ON_ZERO_STOCK == Y");
                return true;
            }
            int numberOfGiftsAvailable = 0;
            int numberOfGiftsNeeded = 0;

            DataTable dt = GiftSearch(prod_id, 1);
            numberOfGiftsAvailable = Convert.ToInt32(dt.Rows[0]["quantity"]);

            if (dgvRedemption.Rows.Count > 0)
            {
                for (int i = 0; i < dgvRedemption.Rows.Count - 1; i++)
                {
                    if ((dgvRedemption.Rows[i].Cells["dcProductId"].Value != null) && (dgvRedemption.Rows[i].Cells["dcProductId"].Value.ToString() != ""))
                    {
                        if (prod_id == dgvRedemption.Rows[i].Cells["dcProductId"].Value.ToString())
                        {
                            numberOfGiftsNeeded++;
                        }
                    }
                }
            }

            if (BeforeAdding)
            {
                if (numberOfGiftsAvailable > numberOfGiftsNeeded)
                {
                    SetLastActivityTime();
                    log.LogMethodExit("(" + prod_id + "," + BeforeAdding + ") as numberOfGiftsAvailable > numberOfGiftsNeeded ");
                    return true;
                }
                else
                {
                    SetLastActivityTime();
                    log.LogMethodExit("(" + prod_id + "," + BeforeAdding + ") as numberOfGiftsAvailable < numberOfGiftsNeeded");
                    return false;
                }
            }
            else
            {
                if (numberOfGiftsAvailable >= numberOfGiftsNeeded)
                {
                    SetLastActivityTime();
                    log.LogMethodExit("(" + prod_id + "," + BeforeAdding + ") as numberOfGiftsAvailable >= numberOfGiftsNeeded");
                    return true;
                }
                else
                {
                    SetLastActivityTime();
                    log.LogMethodExit("(" + prod_id + "," + BeforeAdding + ") as numberOfGiftsAvailable <= numberOfGiftsNeeded");
                    return false;
                }
            }
        }

        private DataTable GiftSearch(String prod_code, int mode)
        {
            log.LogMethodEntry(prod_code, mode);
            SetLastActivityTime();
            string cmdText = "";
            if (mode == 1)
            {
                cmdText = @"select Code, Description, round(PriceInTickets * @disc, 0) PriceInTickets, isnull(Quantity, 0) Quantity, P.ProductId, 
                                           PriceInTickets as OriginalPriceInTickets
                                    from Product P left outer join Inventory I 
                                            on P.productId = I.ProductId 
                                            and I.LocationId = (select isnull(pos.InventoryLocationId, p.outboundLocationId) 
                                                                from (select 1 a) v left outer join POSMachines pos 
                                                                        on POSMachineId = @posName) 
                                    where (Code like @product_code or ProductName like @product_code)
                                    and not exists (select 1 from redemptionCurrency rc where rc.productId = p.productId) 
                                    AND (Quantity > 0 or @ignoreQtyCheck = 'Y') AND P.IsActive = 'Y'
                                    and P.IsRedeemable = 'Y'";
            }
            else if (mode == 2)
            {
                cmdText = @"select Code, Description, round(PriceInTickets * @disc, 0) PriceInTickets, 'N' as selectGiftMain, isnull(Quantity, 0) Quantity, P.ProductId,
                                           PriceInTickets as OriginalPriceInTickets
                                    from Product P left outer join Inventory I 
                                                    on P.productId = I.ProductId 
                                                    and I.LocationId = (select isnull(pos.InventoryLocationId, p.outboundLocationId) 
                                                                            from (select 1 a) v left outer join POSMachines pos 
                                                                                on POSMachineId = @posName) 
                                    where (Code like @product_code or ProductName like @product_code)
                                    and not exists (select 1 from redemptionCurrency rc where rc.productId = p.productId) 
                                    AND (Quantity > 0 or @ignoreQtyCheck = 'Y') AND P.IsActive = 'Y' 
                                    and P.IsRedeemable = 'Y' ";
            }
            prod_code = prod_code + "%";
            SqlParameter[] sqlParam = new SqlParameter[4];
            sqlParam[0] = new SqlParameter("@product_code", prod_code);
            sqlParam[1] = new SqlParameter("@disc", 1.0 - _redemption.getDiscount());
            sqlParam[2] = new SqlParameter("@ignoreQtyCheck", _utilities.ParafaitEnv.ALLOW_TRANSACTION_ON_ZERO_STOCK);
            sqlParam[3] = new SqlParameter("@posName", _utilities.ParafaitEnv.POSMachineId);
            DataTable DT = new DataTable();
            DT = _utilities.executeDataTable(cmdText, sqlParam);
            SetLastActivityTime();
            log.LogMethodExit(DT);
            return DT;
        }

        void btnProduct_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            (sender as Button).BackgroundImage = btnSampleProduct.BackgroundImage;
            log.LogMethodExit();
        }

        void btnProduct_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            (sender as Button).BackgroundImage = Properties.Resources.ProductPressed;
            log.LogMethodExit();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            txt_proddesc.Clear();
            txt_prodcode.Clear();
            txtPriceInTicketMoreThan.Clear();
            txtPriceInTicketLessThan.Clear();
            SetHintTextForLessThanField();
            SetHintTextForMoreThanField();
            DT_Search.Clear();
            search_dgv.Refresh();
            flpProducts.Controls.Clear();
            this.ActiveControl = txt_prodcode;
            btnSearch_Click(null, null);
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnProductSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.Product_Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnProductSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.Product_Search_Btn_Normal;
            log.LogMethodExit();
        }

        private void btnMoreOptions_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnMoreOptions.BackgroundImage = Properties.Resources.More_Options_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnMoreOptions_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnMoreOptions.BackgroundImage = Properties.Resources.More_Options_Btn_Normal;
            log.LogMethodExit();
        }

        private void search_dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            try
            {
                if (this.search_dgv.Columns[e.ColumnIndex].Name == "imgViewInventoryLocations")
                {
                    log.Debug("Ends-search_dgv_CellDoubleClick()");
                    return;
                }

                if (e.RowIndex >= 0 && search_dgv.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < search_dgv.SelectedRows.Count; i++)
                    {
                        search_dgv.SelectedRows[i].Cells["selectGift"].Value = "Y";
                    }
                    AddtoDgvRedemption();
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Ends-search_dgv_CellDoubleClick() due to exception: " + ex.Message);
                //"Error adding redemption gift. Error: " + ex.Message, search_dgv.SelectedRows[0].Cells["Code"].Value.ToString()
                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2675, ex.Message + " " + search_dgv.SelectedRows[0].Cells["Code"].Value.ToString()), MESSAGE);
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }


        private void AddtoDgvRedemption()
        {
            log.LogMethodEntry();
            bool gift_availability = true;
            for (int i = 0; i < DT_Search.Rows.Count; i++)
            {
                if (DT_Search.Rows[i]["selectGift"].ToString() == "Y")
                {
                    DT_Search.Rows[i]["selectGift"] = "N";
                    gift_availability = CheckGiftAvailability(DT_Search.Rows[i]["Code"].ToString(), true);
                    if (!gift_availability)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 116, DT_Search.Rows[i]["Code"]), ERROR);
                        log.Info("Ends-AddtoGiftDGV() as Not enough quantity available for " + DT_Search.Rows[i]["Code"] + ". Please correct before saving, Insufficient Inventory");
                    }
                    else
                    {
                        string message = "";
                        string code = DT_Search.Rows[i]["Code"].ToString();
                        if (!_redemption.addGift(code, 'C', ref message))
                        {
                            DisplayMessageLine(message, ERROR);
                            log.Info("Ends-dgvRedemption_CellValueChanged() as addGift with code" + code + " ,error" + message);
                            return;
                        }
                        else
                        {
                            RefreshScreen();
                            //"Product " + DT_Search.Rows[i]["Code"].ToString() + " added successfully."
                            DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2676, DT_Search.Rows[i]["Code"].ToString()), MESSAGE);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnExitProductLookup_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnExitProductLookup.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodExit();
        }

        private void btnExitProductLookup_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnExitProductLookup.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodExit();

        }

        private void btnClearSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnClearSearch.BackgroundImage = Properties.Resources.ClearTrx;
            log.LogMethodExit();
        }

        private void btnClearSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnClearSearch.BackgroundImage = Properties.Resources.ClearTrxPressed;
            log.LogMethodExit();
        }

        private void btnSearch_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnSearch.BackgroundImage = Properties.Resources.Search_Btn_Normal;
            log.LogMethodExit();

        }

        private void btnSearch_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnSearch.BackgroundImage = Properties.Resources.Search_Btn_Pressed;
            log.LogMethodExit();
        }

        private void btnFlagTicketReceipt_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.FlagReceipt_Pressed;
            log.LogMethodExit();
        }

        private void btnFlagTicketReceipt_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            Button senderBtn = (Button)sender;
            senderBtn.BackgroundImage = Properties.Resources.FlagReceipt_Normal;
            log.LogMethodExit();
        }

        private void btnFlagTicketReceipt_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LaunchFlagTicketReceiptUI();
            log.LogMethodExit();
        }

        private void productInventoryInfo_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            productInventoryInfo.Visible = false;
            log.LogMethodExit();
        }

        private void btnProdInvInfoClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            productInventoryInfo.Visible = false;
            log.LogMethodExit();
        }

        private void search_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            if (e.RowIndex < 0)
            {
                log.LogMethodExit("E.RowIndex < 0");
                return;
            }

            if (this.search_dgv.Columns[e.ColumnIndex].Name == "imgViewInventoryLocations")
            {
                InventoryList inventoryList = new InventoryList();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParm;
                searchParm = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                searchParm.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, search_dgv["productId", e.RowIndex].Value.ToString()));

                List<InventoryDTO> inventoryDTOList = inventoryList.GetAllInventory(searchParm);
                if (inventoryDTOList != null)
                {
                    SortableBindingList<InventoryDTO> prodInvList = new SortableBindingList<InventoryDTO>(inventoryDTOList);
                    productInvDgv.DataSource = prodInvList;
                    LocationList locationList = new LocationList(POSStatic.Utilities.ExecutionContext);
                    List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locSearchParm = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                    locSearchParm.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "Y"));
                    List<LocationDTO> locationDTOList = locationList.GetAllLocations(locSearchParm);
                    SortableBindingList<LocationDTO> locList = new SortableBindingList<LocationDTO>(locationDTOList);
                    _utilities.setLanguage(productInvDgv);
                    locationIdDataGridViewTextBoxColumn.DataSource = locList;
                    quantityDataGridViewTextBoxColumn.DefaultCellStyle = _utilities.gridViewNumericCellStyle();

                    frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                    if (mdiParent.GetOpenScanRedeemChildFormCount() > 1)
                    {
                        productInventoryInfo.Location = new Point(search_dgv[0, 0].Size.Width, (search_dgv.Location.Y + (search_dgv.CurrentRow.Index)));
                        if (mdiParent.GetOpenScanRedeemChildFormCount() > 2)
                        {
                            productInventoryInfo.Width = 624;
                            productInventoryInfo.Height = 149;
                            productInvDgv.Size = new Size(617, productInvDgv.Height);
                            if (this.MdiParent.MdiChildren.Length > 4)
                            {
                                productInvDgv.Size = new Size(350, productInvDgv.Height);
                                productInventoryInfo.Width = 400;

                            }
                        }
                    }
                    else
                    {
                        productInventoryInfo.Location = new Point(search_dgv[1, 0].Size.Width, (search_dgv.Location.Y + (search_dgv.CurrentRow.Index)));
                        productInventoryInfo.Width = 627;
                        productInventoryInfo.Height = 152;
                    }
                    btnProdInvInfoClose.Location = new Point((productInventoryInfo.Width - btnProdInvInfoClose.Width) / 2, btnProdInvInfoClose.Location.Y);
                    productInventoryInfo.Visible = true;
                    productInventoryInfo.Focus();
                }
            }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void btnProdInvInfoClose_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnProdInvInfoClose.BackgroundImage = Properties.Resources.CancelLine;
            log.LogMethodEntry();

        }

        private void btnProdInvInfoClose_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            btnProdInvInfoClose.BackgroundImage = Properties.Resources.CancelLinePressed;
            log.LogMethodEntry();
        }


        private void tpProductList_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                search_dgv.Columns["Code"].Width = 300;
                search_dgv.Columns["PriceInTickets"].Width = 140;
                search_dgv.Columns["Quantity"].Width = 140;
            }
            catch { }
            log.LogMethodExit();
        }

        private void tcProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            try
            {
                if (tcProducts.SelectedTab.Name == "tpProductList")
                {
                    search_dgv.Columns["Code"].Width = 300;
                    search_dgv.Columns["PriceInTickets"].Width = 140;
                    search_dgv.Columns["Quantity"].Width = 140;
                }
            }
            catch
            { }
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void FrmScanAndRedeem_KeyUp(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (enableRCShortCutKeyFeature && (NumberPadVarPanel == null || NumberPadVarPanel.Visible == false) && pnlProductLookup.Visible == false)
                {
                    if (e.KeyCode == Keys.ControlKey)
                    {
                        if (redemptionCurrencyShortCuts.Exists(rcEntry => (StructuralComparisons.StructuralEqualityComparer.Equals(rcEntry.Item1, Encoding.Unicode.GetBytes(shortCutKeyValues.ToUpper())) == true)))
                        {
                            List<Tuple<Byte[], string>> keyMatchList = redemptionCurrencyShortCuts.Where(rcEntry => (StructuralComparisons.StructuralEqualityComparer.Equals(rcEntry.Item1, Encoding.Unicode.GetBytes(shortCutKeyValues.ToUpper())) == true)).ToList();

                            string currencyName = keyMatchList[0].Item2;
                            string message = "";
                            try
                            {
                                if (_redemption.addRedemptionCurrency("", currencyName, ref message))
                                {
                                    DisplayRedemptionCurrency("", currencyName, ref message);
                                }
                                else
                                {
                                    DisplayMessageLine(message, WARNING);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                DisplayMessageLine(ex.Message, WARNING);
                            }
                        }
                        else
                        {
                            DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 1584, shortCutKeyValues), WARNING);
                        }

                        shortCutKeyValues = "";
                    }
                    else
                    {
                        string tempKey = "";
                        switch (e.KeyCode)
                        {
                            case Keys.D0:
                            case Keys.D1:
                            case Keys.D2:
                            case Keys.D3:
                            case Keys.D4:
                            case Keys.D5:
                            case Keys.D6:
                            case Keys.D7:
                            case Keys.D8:
                            case Keys.D9:
                                tempKey = e.KeyCode.ToString().Replace("D", "");
                                break;
                            case Keys.NumPad0:
                            case Keys.NumPad1:
                            case Keys.NumPad2:
                            case Keys.NumPad3:
                            case Keys.NumPad4:
                            case Keys.NumPad5:
                            case Keys.NumPad6:
                            case Keys.NumPad7:
                            case Keys.NumPad8:
                            case Keys.NumPad9:
                                tempKey = e.KeyCode.ToString().Replace("NumPad", "");
                                break;
                            default:
                                tempKey = e.KeyCode.ToString();
                                break;
                        }
                        shortCutKeyValues = shortCutKeyValues + tempKey;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message, WARNING);
            }
            log.LogMethodExit();
        }

        private bool DoUserLogin(ref Security.User user, bool isBasicCheck = false)
        {
            log.LogMethodEntry(isBasicCheck);
            bool returnValue = RedemptionAuthentication.RedemptionLoginUser(_utilities, cardReader, ref user, isBasicCheck);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool DoManagerAuthenticationCheck(ref int managerId)
        {
            log.LogMethodEntry();
            bool returnValue = false;
            string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;//hold pos static manager flag value
            int savRoleId = POSStatic.ParafaitEnv.RoleId;
            try
            {
                //pass current redemption user manager flag value
                POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                POSStatic.ParafaitEnv.RoleId = _utilities.ParafaitEnv.RoleId;
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



        private void SetNewScreenForLoggerdInUser(string loginId)
        {
            log.LogMethodEntry(loginId);
            SetLastActivityTime();
            frmRedemptionScreenBanner frmBanner = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
            if (frmBanner != null && string.IsNullOrEmpty(loginId) == false)
            {
                frmBanner.SetNewScreenTileForLoggedInUser(loginId);
            }
            log.LogMethodExit();
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            timerClock.Stop();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            if (mdiParent != null && _utilities != null)
            {
                DateTime lastActivityTimeValue = mdiParent.GetLastActivityDateTime(_user.LoginId);


                int inactivityPeriodSec = (int)(DateTime.Now - lastActivityTimeValue).TotalSeconds;

                if (inactivityPeriodSec > POSStatic.POS_INACTIVE_TIMEOUT)//inactivityPeriodSec > 60
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                    {
                        DisplayMessageLine("");
                        VerifyUserRelogin();
                    }
                    else
                    {
                        SetLastActivityTime();
                        EnableFormElements();
                        StartScreenTimer();
                    }
                }
                else
                {
                    EnableFormElements();
                    StartScreenTimer();
                }
            }
            else
            {
                EnableFormElements();
                StartScreenTimer();
            }

        }

        public int GetFormCenterX()
        {
            log.LogMethodEntry();
            int x = this.Width / 2;
            log.LogMethodExit(x);
            return x;
        }
        public int GetFormCenterY()
        {
            log.LogMethodEntry();
            int y = this.Height / 2;
            log.LogMethodExit(y);
            return y;

        }
        private void VerifyUserRelogin()
        {
            log.LogMethodEntry();
            if (GetReloginAuthenticationInitiated() == false)
            {
                HideNumberPad();
                int frmCenterX = GetFormCenterX();
                int frmCenterY = GetFormCenterY();
                HideChildAndRelatedForms();
                if (cardReader != null)
                {
                    cardReader.UnRegister();
                }
                Login.frmReloginUser frmReloginUser = new Login.frmReloginUser(this._utilities, this._user, cardReader);
                frmReloginUser.TopLevel = false;
                frmReloginUser.GetFormUpdates += new Login.frmReloginUser.GetFormUpdatesDeletegate(GetReloginFormUpdates);
                this.reloginPanel.Visible = true;
                this.reloginPanel.SuspendLayout();
                this.reloginPanel.Size = new Size(this.dgvRedemption.Width, this.dgvRedemption.Height);
                this.reloginPanel.Controls.Add(frmReloginUser);
                frmReloginUser.Show();
                SetReloginAuthenticationInitiated(true);
                DisableFormElements();
                this.reloginPanel.ResumeLayout(true);
                this.reloginPanel.Location = new Point(frmCenterX - this.reloginPanel.Width / 2, frmCenterY - this.reloginPanel.Height / 2);
                this.reloginPanel.BringToFront();

            }
            log.LogMethodExit();
        }


        private void GetReloginFormUpdates(bool isVerifiedUser)
        {
            log.LogMethodEntry(isVerifiedUser);
            try
            {
                if (cardReader != null)
                {
                    cardReader.Register(CardScanCompleteEventHandle);
                }
                if (isVerifiedUser)
                {
                    SetLastActivityTime();
                    ShowChildAndRelatedForms();
                    DisplayMessageLine("");
                }
                else
                {
                    //"Do you want to close screens for " + GetScreenUserLoginId + " ?"
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2677, GetScreenUserLoginId),
                                                MessageContainerList.GetMessage(_utilities.ExecutionContext, "User Login") + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber)
                                                , MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        ForceCloseCurrentUserScreens();
                    }
                }
                SetReloginAuthenticationInitiated(false);
                this.reloginPanel.Visible = false;

                timerClock.Start();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void CloseCurrentUserScreens()
        {
            log.LogMethodEntry();
            StopScreenTimer();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            List<frmScanAndRedeem> childformList = mdiParent.GetOpenScanRedeemChildForms(this.GetScreenUserLoginId);
            if (childformList != null && childformList.Any())
            {
                for (int i = 0; i < childformList.Count; i++)
                {
                    if (childformList[i].GetCurrentScreenNumber != this.GetCurrentScreenNumber)
                    {
                        childformList[i].StopScreenTimer();
                        childformList[i].CloseCurrentForm(); //Always call CloseCurrentForm() to programtically close  frmScanAndRedeem
                        childformList.RemoveAt(i);
                        i = i - 1;
                    }
                }
            }
            CloseCurrentForm(); 
            log.LogMethodExit();
        }

        private void ForceCloseCurrentUserScreens()
        {
            log.LogMethodEntry(); 
            CloseCurrentUserScreens();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            if (mdiParent.GetOpenScanRedeemChildFormCount() == 0)
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                {
                    mdiParent.forceLoginForTimeOut = true;
                }
                mdiParent.Close();
            }
            log.LogMethodExit();
        }

        internal void SetLastActivityTime()
        {
            //log.LogMethodEntry();
            try
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                if (mdiParent != null && _user != null)
                {
                    mdiParent.SetLastActivityDateTime(_user.LoginId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //log.LogMethodExit(lastActivityTime)
        }

        private void SetReloginAuthenticationInitiated(bool reLogInitiated)
        {
            log.LogMethodEntry(reLogInitiated);
            try
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                mdiParent.SetReloginAuthenticationInitiated(_user.LoginId, reLogInitiated);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        internal bool GetReloginAuthenticationInitiated()
        {
            log.LogMethodEntry();
            bool reLogInitiated = false;
            try
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                reLogInitiated = mdiParent.GetReloginAuthenticationInitiated(_user.LoginId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(reLogInitiated);
            return reLogInitiated;
        }


        internal void StopScreenTimer()
        {
            log.LogMethodEntry();
            try
            {
                timerClock.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        internal void StartScreenTimer()
        {
            log.LogMethodEntry();
            try
            {
                if (isFormClosed == false)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                    {
                        timerClock.Start();
                    } 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void EnableFormElements()
        {
            log.LogMethodEntry();
            if (this.panel1.Enabled == false)
            {
                if (GetReloginAuthenticationInitiated() == false)
                {
                    frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                    if (mdiParent != null)
                    {
                        foreach (frmScanAndRedeem childForm in mdiParent.GetOpenScanRedeemChildForms(_user.LoginId))
                        {
                            childForm.panel1.Enabled = true;
                            childForm.panelAddClose.Visible = true;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void DisableFormElements()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem childForm in mdiParent.GetOpenScanRedeemChildForms(_user.LoginId))
            {
                childForm.panel1.Enabled = false;
                childForm.panelAddClose.Visible = false;
            }
            log.LogMethodExit();
        }
        private void SetReloginPanel()
        {
            log.LogMethodEntry();
            this.reloginPanel = new Panel();
            this.reloginPanel.AutoSize = true;
            this.reloginPanel.Anchor = (AnchorStyles.Left | AnchorStyles.Top);
            this.reloginPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.reloginPanel);
            this.reloginPanel.Location = new Point(this.panel1.Location.X, this.panel1.Location.Y + panelTop.Height);
            this.reloginPanel.Size = new Size(this.dgvRedemption.Width, this.dgvRedemption.Height);
            this.reloginPanel.Margin = new Padding(0);
            this.reloginPanel.Visible = false;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            this.reloginPanel.BackColor = mdiParent.GetBackgroundColor(GetScreenUserLoginId);
            log.LogMethodExit();
        }

        internal void CloseChildAndRelatedForms()
        {
            log.LogMethodEntry();
            for (int i = 0; i < this.OwnedForms.Count(); i++)
            {
                this.OwnedForms[i].Close();
            }
            log.LogMethodExit();
        }


        private void HideChildAndRelatedForms()
        {
            log.LogMethodEntry();
            for (int i = 0; i < this.OwnedForms.Count(); i++)
            {
                SetFormVisibility(this.OwnedForms[i], false);
            }
            log.LogMethodExit();
        }


        private void ShowChildAndRelatedForms()
        {
            log.LogMethodEntry();
            for (int i = 0; i < this.OwnedForms.Count(); i++)
            {
                SetFormVisibility(this.OwnedForms[i], true);
            }
            log.LogMethodExit();
        }

        private void SetFormVisibility(Form frm, bool visibilityValue)
        {
            try
            {
                log.LogMethodEntry(frm.Name, visibilityValue);
                if (frm.Name == "frm_redemption")
                {

                    frm_redemption frmR = frm as frm_redemption;
                    if (visibilityValue)
                    {
                        frmR.RegisterDevices();
                    }
                    else
                    {
                        frmR.UnregisterDevices();
                    }
                }
                if (frm.Name == "frmReprintTicketReceipt")
                {

                    frmReprintTicketReceipt frmRPT = frm as frmReprintTicketReceipt;
                    if (visibilityValue)
                    {
                        frmRPT.RegisterDevices();
                    }
                    else
                    {
                        frmRPT.UnregisterDevices();
                    }
                }
                for (int i = 0; i < frm.OwnedForms.Count(); i++)
                {
                    SetFormVisibility(frm.OwnedForms[i], visibilityValue);
                }
                frm.Visible = visibilityValue;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void SetAsActiveScreenForCurrentUser(string screenNumber)
        {
            log.LogMethodEntry(screenNumber);
            try
            {
                if (screenNumber != "0")
                {
                    frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                    if (mdiParent != null)
                    {
                        if (mdiParent.ScreenIsAlredyOpened(screenNumber) == false)
                        {
                            //Sorry, screen number &1 is not yet launched
                            throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2678, screenNumber));
                        }
                        else if (mdiParent.IsScreenOwnedByUser(GetScreenUserLoginId, screenNumber) == false)
                        {
                            //"Sorry, screen number &1 does not belongs to &2"
                            throw new Exception(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2679, screenNumber, GetScreenUserLoginId));
                        }
                        else
                        {
                            frmRedemptionScreenBanner bannerScreen = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
                            //SetAsActiveScreen
                            bannerScreen.SetAsActiveScreen(GetScreenUserLoginId, screenNumber);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber));
            }
            log.LogMethodExit();
        }

        private void SearchFieldEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            log.LogMethodExit();
        }
        private void SearchFieldKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            log.LogMethodExit();
        }

        private void search_dgv_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            log.LogMethodExit();
        }


        private void HideNumberPad()
        {
            log.LogMethodEntry();
            if (NumberPadVarPanel != null && NumberPadVarPanel.Visible)
            {
                NumberPadVarPanel.Visible = false;
            }
            log.LogMethodExit();
        }

        private void LaunchFlagTicketReceiptUI(string barCode = null)
        {
            log.LogMethodEntry(barCode);
            SetLastActivityTime();
            HideNumberPad();
            TicketReceiptUI ticketReceiptUI = (barCode == null ? new TicketReceiptUI(_utilities) : new TicketReceiptUI(_utilities, barCode));
            ticketReceiptUI.DoManagerAuthenticationCheck += new TicketReceiptUI.DoManagerAuthenticationCheckDelegate(DoManagerAuthenticationCheck);
            ticketReceiptUI.FormBorderStyle = FormBorderStyle.None;
            ticketReceiptUI.Location = this.Location;
            //ticketReceiptUI.WindowState = FormWindowState.Maximized;
            ticketReceiptUI.Width = this.Width;
            ticketReceiptUI.Height = this.Height;
            ticketReceiptUI.AutoScroll = true;
            ticketReceiptUI.Owner = this;
            ticketReceiptUI.SetLastActivityTime += new TicketReceiptUI.SetLastActivityTimeDelegate(this.SetLastActivityTime);
            ticketReceiptUI.Show();
            log.LogMethodExit();
        }

        internal void ActivateDevicesOnAllActiveScreens()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem openFrm in mdiParent.GetOpenScanRedeemChildForms())
            {
                log.Info("Register loop " + openFrm.GetCurrentScreenNumber + " : " + openFrm.GetScreenUserLoginId);
                if (mdiParent.IsActiveScreen(openFrm.GetScreenUserLoginId, openFrm.GetCurrentScreenNumber))
                {
                    openFrm.ActivateDevices();
                }
            }
            log.LogMethodExit();
        }

        private void ActivateDevices()
        {
            log.LogMethodEntry();
            if (GetReloginAuthenticationInitiated() == false)
            {
                log.Info("Register devices on " + GetCurrentScreenNumber + " : " + GetScreenUserLoginId);
                if (cardReader != null)
                {
                    cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
                }
                if (barcodeScanner != null)
                {
                    barcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
                }
            }
            log.LogMethodExit();
        }

        internal void StopDevicesOnAllScreens()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem openFrm in mdiParent.GetOpenScanRedeemChildForms())
            {
                log.Info("unregister loop " + openFrm.GetCurrentScreenNumber + " : " + openFrm.GetScreenUserLoginId);
                if (mdiParent.IsActiveScreen(openFrm.GetScreenUserLoginId, openFrm.GetCurrentScreenNumber))
                {
                    openFrm.InActivateDevices();
                }
            }
            log.LogMethodExit();
        }

        private void InActivateDevices()
        {
            log.LogMethodEntry();
            if (GetReloginAuthenticationInitiated() == false)
            {
                log.Info("unregister devices on " + GetCurrentScreenNumber + " : " + GetScreenUserLoginId);
                if (cardReader != null)
                {
                    cardReader.UnRegister();
                }
                if (barcodeScanner != null)
                {
                    barcodeScanner.UnRegister();
                }
            }
            log.LogMethodExit();
        }

        private void flpProducts_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            log.LogMethodExit();
        }
        

        private void flpProducts_MouseWheel(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            log.LogMethodExit();
        }


        private void LauncNewUserForStaffCard(Card swipedCard)
        {
            log.LogMethodEntry();
            if (swipedCard != null && swipedCard.technician_card == 'Y')
            {
                Security sec = new Security(_utilities);
                Security.User newloginUser = sec.Login(string.Empty, -1, swipedCard.CardNumber);
                if (newloginUser != null)
                {
                    if (newloginUser.LoginId == this.LoggedInId)
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 197, swipedCard.CardNumber), ERROR);
                        //Staff card can not be used for transaction
                    }
                    else if(enableSingleUserMultiScreen == false)
                    {
                        frmRedemptionScreenBanner frmScreenBanner = Application.OpenForms["frmRedemptionScreenBanner"] as frmRedemptionScreenBanner;
                        if (frmScreenBanner.CanAddThisUser(newloginUser.LoginId))
                        {
                            if (Application.OpenForms["formLogin"] == null)
                            {//"Tapped card belongs to user &1, Do you want to launch new screen for the user?"
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2749, newloginUser.LoginId)
                                                                , MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber), MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {

                                    if (frmScreenBanner != null)
                                    {
                                        frmScreenBanner.LaunchNewUserScreen(newloginUser);
                                    }
                                }
                                else
                                {
                                    DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 197, swipedCard.CardNumber), ERROR);
                                    //Staff card can not be used for transaction
                                }
                            }
                            else
                            {
                                DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 197, swipedCard.CardNumber), ERROR);
                            }
                        }
                    }
                    else
                    {
                        DisplayMessageLine(MessageContainerList.GetMessage(_utilities.ExecutionContext, 197, swipedCard.CardNumber), ERROR);
                    }
                }
            }
            log.LogMethodExit();
        }


        private bool OkayToDiscardUnsavedRedemption(string tileText)
        {
            log.LogMethodEntry();
            bool isOkayToDiscard = true;
            if (_redemption != null && _redemption._RedemptionId <= 0 && (_redemption.getTotalTickets() > 0 || _redemption.productList.Count > 0))
            {
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(_utilities.ExecutionContext, 2740),
                                              tileText + MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, GetCurrentScreenNumber)
                                               , MessageBoxButtons.YesNo, MessageContainerList.GetMessage(_utilities.ExecutionContext, "Discard"),
                                               MessageContainerList.GetMessage(_utilities.ExecutionContext, "Cancel")) == System.Windows.Forms.DialogResult.No)
                {
                    isOkayToDiscard = false;
                }
            }
            log.LogMethodExit(isOkayToDiscard);
            return isOkayToDiscard;
        }
        private void SetFocusOnDataGrid()
        {
            log.LogMethodEntry();
            worker = new BackgroundWorker();
            worker.DoWork += StartCounting;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
            log.LogMethodExit();
        }

        private void StartCounting(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(200);
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            dgvRedemption.Focus();
            dgvRedemption.CurrentCell = dgvRedemption.Rows[0].Cells["dcProductName"];
            log.LogMethodExit();
        }
        private void txtPriceInTicketMoreThan_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetHintTextForMoreThanField();            
            log.LogMethodExit();
        }
  
        private void SetHintTextForMoreThanField()
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(txtPriceInTicketMoreThan.Text))
            {
                txtPriceInTicketMoreThan.Text = MessageContainerList.GetMessage(_utilities.ExecutionContext, "More than");
                txtPriceInTicketMoreThan.ForeColor = SystemColors.GrayText;
            }
            log.LogMethodExit();
        }
        private void txtPriceInTicketMoreThan_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            if (txtPriceInTicketMoreThan.Text == MessageContainerList.GetMessage(_utilities.ExecutionContext, "More than"))
            {
                txtPriceInTicketMoreThan.Clear();
                txtPriceInTicketMoreThan.ForeColor = txt_prodcode.ForeColor;
            }
            log.LogMethodExit();
        }
        

        private void txtPriceInTicketLessThan_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetLastActivityTime();
            if (txtPriceInTicketLessThan.Text == MessageContainerList.GetMessage(_utilities.ExecutionContext, "Less than"))
            {
                txtPriceInTicketLessThan.Clear();
                txtPriceInTicketLessThan.ForeColor = txt_prodcode.ForeColor;
            }
            log.LogMethodExit();
        }        

        private void txtPriceInTicketLessThan_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetHintTextForLessThanField();
            log.LogMethodExit();
        }
        private void SetHintTextForLessThanField()
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(txtPriceInTicketLessThan.Text))
            {
                txtPriceInTicketLessThan.Text = MessageContainerList.GetMessage(_utilities.ExecutionContext, "Less than");
                txtPriceInTicketLessThan.ForeColor = SystemColors.GrayText;
            }
            log.LogMethodExit();
        }

        internal void EnableFormElementsForSuccessfulLoginIfTimerIsNotOn()
        {
            log.LogMethodEntry();
            try
            {
                if (isFormClosed == false)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false) == false 
                        && ParafaitDefaultContainerList.GetParafaitDefault<bool>(_utilities.ExecutionContext, "REQUIRE_LOGIN_FOR_EACH_TRX", false))
                    {
                        if (string.IsNullOrEmpty(LoggedInId))
                        {
                            CheckLoginRequired();
                        }
                        else
                        {
                            EnableFormElements();
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

    }
}

