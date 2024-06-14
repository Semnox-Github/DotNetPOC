/********************************************************************************************
 * Project Name - Parafait_POS                                                                     
 * Description  - UI class for Redemption Screen Banner
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80.0       03-Apr-2020   Guru S A             Created for redemption UI enhancements  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device;
using System.Collections.Generic;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.Peripherals;

namespace Parafait_POS.Redemption
{
    public partial class frmRedemptionScreenBanner : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private bool enableSingleUserMultiScreen;
        private bool showAllRedemptionScreens;
        private DeviceClass defaultBarcodeScanner = null;
        private const int bannerHeight = 72;
        private const int groupTileHeight = 48;
        private const int tileHeight = 44;
        private const int tileWidth = 145;
        private const int tileLabelHeight = 44;
        private const int tileNameLabelWidth = 120;
        private const int tileNumberLabelWidth = 25;
        private const string MDIFORM = "frmScanAndRedeemMDI";
        private const string SCANCODEADDUSER = "ADDUSER";
        private bool addFromMDI = false;

        public bool AddFromMDI { set { addFromMDI = value; } }
         
        public frmRedemptionScreenBanner(Utilities utilities)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.utilities = utilities;
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 5, bannerHeight);

            this.fpnlUserTiles.Location = new Point(2, 20);
            this.fpnlUserTiles.MinimumSize = new System.Drawing.Size(tileWidth, groupTileHeight);
            this.fpnlUserTiles.Size = new System.Drawing.Size(tileWidth, groupTileHeight);
            this.fpnlUserTiles.BorderStyle = BorderStyle.None;

            this.btnAddUser.Location = new Point(this.Width - 80, btnAddUser.Location.Y);
            enableSingleUserMultiScreen = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "ENABLE_SINGLE_USER_MULTI_SCREEN");
            showAllRedemptionScreens = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "SHOW_ALL_REDEMPTION_SCREENS");

            if (enableSingleUserMultiScreen)
            {
                btnAddUser.Visible = false;
            }
            defaultBarcodeScanner = null;
            log.LogMethodExit();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            AddUser();
            log.LogMethodExit();
        }

        public void AddUser()
        {
            log.LogMethodEntry();
            try
            {
                if (CanAddNewScreens())
                {
                    Security.User user = null;
                    bool auth = DoUserLogin(ref user, true);

                    if (!auth)
                    {
                        log.LogMethodExit("User authentication failed");
                        return;
                    }
                    else
                    {
                        LaunchNewUserScreen(user);
                    }

                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2671));// "Cannot launch more than 8 screens"));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private bool CanAddNewScreens()
        {
            log.LogMethodEntry();
            bool canAddNewScreen = false;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = (mdiParent.GetOpenScanRedeemChildFormCount());
            if (childCount < 8)
            {
                canAddNewScreen = true;
            }
            log.LogMethodExit(canAddNewScreen);
            return canAddNewScreen;
        }

        private bool DoUserLogin(ref Security.User user, bool isBasicCheck = false)
        {
            log.LogMethodEntry((user != null ? user.LoginId : string.Empty), isBasicCheck);
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            DeviceClass cardReader = mdiParent.RegisterCardDevice();
            //bool firstCardReaderIsSet = false;
           // if (cardReader == null)
            //{
                //cardReader = RegisterCardReaderDevice();
                if (cardReader != null)
                {
              //      firstCardReaderIsSet = true;
                    cardReader.Register(CardScanCompleteEventHandle);
                }
           // }
            //else
            //{
                //stop timer on user forms till login is complete
                StopUserScreenTimer(mdiParent);
            //}
            bool returnValue = RedemptionAuthentication.RedemptionLoginUser(utilities, cardReader, ref user, isBasicCheck);
            //if (firstCardReaderIsSet)
            // {
            if (cardReader != null)
            {
                cardReader.Dispose();
                cardReader = null;
            }
            // }
            // else
            // {
            StartUserScreenTimer(mdiParent);
           // }
            ResetBarcodeScannerIfUserScannerFound();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        
        /// <summary>
        /// CanAddThisUser
        /// </summary>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public bool CanAddThisUser(string loginId)
        {
            log.LogMethodEntry(loginId);
            bool canAddThisUser = true;

            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem f in mdiParent.GetOpenScanRedeemChildForms())
            {
                if (f.LoggedInId.Equals(loginId))
                {
                    //Sorry, f.LoggedInId is already logged in as a Redemption user
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2672, f.LoggedInId));
                    canAddThisUser = false;
                    break;
                }
            }
            log.LogMethodExit(canAddThisUser);
            return canAddThisUser;
        }

        private void CreateUserScreenTile(Security.User user)
        {
            log.LogMethodEntry(user.LoginId);
            pnlBase.SuspendLayout();
            fpnlUserTiles.SuspendLayout();
            FlowLayoutPanel userFlowPanel = new FlowLayoutPanel();
            userFlowPanel.BackColor = Color.White;
            userFlowPanel.AutoSize = true;
            userFlowPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            userFlowPanel.Name = "userFlowPanel";
            userFlowPanel.MaximumSize = new Size(0, groupTileHeight);
            userFlowPanel.Tag = user.LoginId;
            userFlowPanel.Margin = new Padding(2);
            userFlowPanel.BorderStyle = BorderStyle.FixedSingle;

            FlowLayoutPanel userGroupBox = CreateUserGroup(user.LoginId);

            Panel pnlScreenTile = CreateScreenNumberPanel();

            userGroupBox.Controls.Add(pnlScreenTile);
            userFlowPanel.Controls.Add(userGroupBox);

            fpnlUserTiles.Controls.Add(userFlowPanel);

            Label lnlUserGroupTag = new Label
            {
                Name = "lnlUserGroupTag",
                Tag = user.LoginId,
                Text = user.LoginId,
                BackColor = Color.Transparent,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold),
                Anchor = AnchorStyles.Left | AnchorStyles.Top
            };
            fpnlUserTiles.ResumeLayout(true);
            pnlBase.Controls.Add(lnlUserGroupTag);
            lnlUserGroupTag.Location = new Point(userFlowPanel.Location.X + 2, 2);
            lnlUserGroupTag.Size = new Size(userFlowPanel.Width, lnlUserGroupTag.Height);
            pnlBase.ResumeLayout(true);
            log.LogMethodExit();
        }


        private FlowLayoutPanel CreateUserGroup(string loginId)
        {
            log.LogMethodEntry(loginId);
            FlowLayoutPanel userGroupBox = new FlowLayoutPanel
            {
                AutoSize = true,
                //Text = loginId,
                MinimumSize = new Size(tileWidth, groupTileHeight),
                Size = new Size(tileWidth, groupTileHeight),
                Margin = new Padding(1, 1, 1, 1),
                BackColor = Color.Transparent,
                Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F),
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            log.LogMethodEntry(userGroupBox);
            return userGroupBox;
        }

        private Panel CreateScreenNumberPanel()
        {
            log.LogMethodEntry();
            Panel pnlScreenTile = new Panel
            {
                Size = new Size(tileWidth, tileHeight),
                Margin = new Padding(1, 0, 1, 0),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            int screenNumber = GetNextAvailableScreenNumber();
            Label lblScreenNumber = new Label
            {
                Text = screenNumber.ToString(),
                Name = "lblScreenNumber",
                //AutoEllipsis = true,
                Size = new Size(tileNumberLabelWidth, tileLabelHeight),
                Margin = new Padding(1, 0, 1, 0),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblCustomerCardInfo = new Label
            {
                Text = string.Empty,
                Name = "lblCustomerCardInfo",
                AutoEllipsis = true,
                Size = new Size(tileNameLabelWidth, (tileLabelHeight / 2) - 2),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(1, 0, 1, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label lblBalanceInfo = new Label
            {
                Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Bal: ") + "0",
                Name = "lblBalanceInfo",
                AutoEllipsis = true,
                Size = new Size(tileNameLabelWidth, (tileLabelHeight / 2)),
                Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, FontStyle.Regular),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(1, 0, 1, 0),
                TextAlign = ContentAlignment.MiddleLeft
            };

            pnlScreenTile.Controls.Add(lblScreenNumber);
            pnlScreenTile.Controls.Add(lblCustomerCardInfo);
            pnlScreenTile.Controls.Add(lblBalanceInfo);
            lblScreenNumber.Location = new Point(0, 1);
            lblCustomerCardInfo.Location = new Point(lblScreenNumber.Width + 2, 1);
            lblBalanceInfo.Location = new Point(lblScreenNumber.Width + 2, lblCustomerCardInfo.Height + 2);

            pnlScreenTile.Tag = screenNumber.ToString();
            lblCustomerCardInfo.Tag = screenNumber.ToString();
            lblScreenNumber.Tag = screenNumber.ToString();
            lblBalanceInfo.Tag = screenNumber.ToString();

            lblScreenNumber.Click += new EventHandler(ScreenTileClick);
            lblCustomerCardInfo.Click += new EventHandler(ScreenTileClick);
            lblBalanceInfo.Click += new EventHandler(ScreenTileClick);
            pnlScreenTile.Click += new EventHandler(ScreenTileClick);
            log.LogMethodExit();
            return pnlScreenTile;
        }


        private int GetNextAvailableScreenNumber()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            int childCount = mdiParent.MdiChildren.Length + 1;
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
            log.LogMethodExit(screenNumber);
            return screenNumber;
        }
        private void ScreenTileClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Control frmCtrl = (Control)sender;
            if (frmCtrl.Tag != null)
            {
                string screenNumber = frmCtrl.Tag.ToString();
                string loginId = GetScreenUserLoginId(screenNumber);
                if (ScreenNotLocked(loginId))
                {
                    SetAsActiveScreen(loginId, screenNumber);
                }
            }
            log.LogMethodExit();
        }



        private string GetScreenUserLoginId(string screenNumber)
        {
            log.LogMethodEntry(screenNumber);
            string loginID = string.Empty;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem f in mdiParent.GetOpenScanRedeemChildForms())
            {
                if (f.GetCurrentScreenNumber == screenNumber)
                {
                    loginID = f.GetScreenUserLoginId;
                    break;
                }
            }
            log.LogMethodExit(loginID);
            return loginID;
        }

        internal void SetAsActiveScreen(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);

            if (string.IsNullOrEmpty(loginId) == false)
            {
                FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginId);
                if (selectedUserPanel != null)
                {
                    pnlBase.SuspendLayout();
                    fpnlUserTiles.SuspendLayout();
                    List<FlowLayoutPanel> userGroupBoxList = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList();
                    if (userGroupBoxList != null && userGroupBoxList.Any())
                    {
                        FlowLayoutPanel userGroupBox = userGroupBoxList[0];
                        SetAsActiveScreen(userGroupBox, loginId, screenNumber);
                    }
                    fpnlUserTiles.ResumeLayout(true);
                    pnlBase.ResumeLayout();
                }
                else
                {
                    //no screens for the user. So find available user and their active screen
                    frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                    foreach (frmScanAndRedeem childForm in mdiParent.GetOpenScanRedeemChildForms())
                    {
                        if (mdiParent.IsActiveScreen(childForm.GetScreenUserLoginId, childForm.GetCurrentScreenNumber))
                        {
                            childForm.Visible = false;
                            childForm.Show();
                            childForm.WindowState = FormWindowState.Normal;
                            childForm.ResizeMdiChildren();
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetAsActiveScreen(FlowLayoutPanel userGroupBox, string loginId, string screenNumber)
        {
            log.LogMethodEntry(userGroupBox, loginId, screenNumber);
            string activeScreenNumber = string.Empty;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            foreach (frmScanAndRedeem childForm in mdiParent.GetOpenScanRedeemChildForms(loginId))
            {
                if (string.IsNullOrEmpty(screenNumber)
                    || (string.IsNullOrEmpty(screenNumber) == false && childForm.GetCurrentScreenNumber == screenNumber
                   ))
                {
                    screenNumber = childForm.GetCurrentScreenNumber;
                    childForm.StopDevicesOnAllScreens();
                    mdiParent.SetAsActiveScreen(loginId, screenNumber);
                    // SetSelectedScreenTileColor(loginId, screenNumber);
                    activeScreenNumber = screenNumber;
                    childForm.ActivateDevicesOnAllActiveScreens(); 
                }
                else
                {
                    if (showAllRedemptionScreens == false)
                    {
                        if (childForm.Visible)
                        {
                            childForm.Visible = false;
                            childForm.Hide();
                            childForm.WindowState = FormWindowState.Minimized;
                        }
                    }
                }
            }
            foreach (frmScanAndRedeem childForm in mdiParent.GetOpenScanRedeemChildForms(loginId))
            {
                if (childForm.GetScreenUserLoginId == loginId && childForm.GetCurrentScreenNumber == activeScreenNumber)
                {
                    childForm.Visible = false;
                    childForm.Show();
                    childForm.WindowState = FormWindowState.Normal;
                    childForm.ResizeMdiChildren();
                    break;
                }
            }
            log.LogMethodExit();
        }

        private FlowLayoutPanel GetUserFlowPanel(string loginId)
        {
            log.LogMethodEntry(loginId);
            List<FlowLayoutPanel> userFLowPanelList = fpnlUserTiles.Controls.OfType<FlowLayoutPanel>().ToList();
            FlowLayoutPanel selectedUserPanel = null;
            if (userFLowPanelList != null)
            {
                int userTileCount = userFLowPanelList.Count();
                for (int i = 0; i < userTileCount; i++)
                {
                    if (userFLowPanelList[i].Tag != null && userFLowPanelList[i].Tag.ToString() == loginId)
                    {
                        selectedUserPanel = userFLowPanelList[i];
                        break;
                    }
                }
            }
            log.LogMethodExit(selectedUserPanel);
            return selectedUserPanel;
        }


        private void frmRedemptionScreenBanner_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetBannerLocation();
            if (addFromMDI)
            {
                addFromMDI = false;//reset the variable
            }
            else
            {
                CreateUserScreenTile(POSStatic.LoggedInUser);
                frmScanAndRedeem frs = new frmScanAndRedeem(POSStatic.LoggedInUser)
                {
                    MdiParent = Application.OpenForms[MDIFORM]
                };
                frs.Show();
                frs.StopDevicesOnAllScreens();
                frs.ActivateDevicesOnAllActiveScreens();
            }
            log.LogMethodExit();
        }

        public void SetBannerLocation()
        {
            log.LogMethodEntry();
            this.Location = new Point(0, 0);
            log.LogMethodExit();
        }

        internal void SetNewScreenTileForLoggedInUser(string loginID)
        {
            log.LogMethodEntry(loginID);
            if (string.IsNullOrEmpty(loginID) == false)
            {
                FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginID);
                if (selectedUserPanel != null)
                {
                    pnlBase.SuspendLayout();
                    fpnlUserTiles.SuspendLayout();
                    FlowLayoutPanel userGroupBox = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList()[0];

                    Panel pnlScreenTile = CreateScreenNumberPanel();
                    int xCoordinate = userGroupBox.Width + 2;
                    userGroupBox.Controls.Add(pnlScreenTile);
                    pnlScreenTile.Location = new Point(xCoordinate, userGroupBox.Location.Y + 15);
                    SetSelectedScreenTileColor(userGroupBox, pnlScreenTile.Tag.ToString());
                    fpnlUserTiles.ResumeLayout(true);
                    pnlBase.ResumeLayout();
                }
                AdjustScreenTilePosition(loginID);
                SetUserTagLabelLocation();
            }
            log.LogMethodExit();
        }


        private void SetSelectedScreenTileColor(FlowLayoutPanel userGroupBox, string screenNumber)
        {
            log.LogMethodEntry(userGroupBox, screenNumber);
            List<Panel> userScreenPanelList = userGroupBox.Controls.OfType<Panel>().ToList();
            if (userScreenPanelList != null)
            { 
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                for (int i = 0; i < userScreenPanelList.Count; i++)
                {
                    if (userScreenPanelList[i].Tag != null
                        && userScreenPanelList[i].Tag.ToString() == screenNumber)
                    {                        
                        userScreenPanelList[i].BackColor = mdiParent.GetUserBackgroundColor(screenNumber);
                        userScreenPanelList[i].ForeColor = Color.Black;
                    }
                    else
                    {
                        userScreenPanelList[i].BackColor = Color.Gray;
                        userScreenPanelList[i].ForeColor = Color.White;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal void SetSelectedScreenTileColor(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);
            FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginId);
            if (selectedUserPanel != null)
            {
                pnlBase.SuspendLayout();
                fpnlUserTiles.SuspendLayout();
                FlowLayoutPanel userGroupBox = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList()[0];
                SetSelectedScreenTileColor(userGroupBox, screenNumber);
                fpnlUserTiles.ResumeLayout(true);
                pnlBase.ResumeLayout();
            }
            log.LogMethodExit();
        }

        internal void RemoveScreenNumberTile(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);
            FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginId);
            if (selectedUserPanel != null)
            {
                pnlBase.SuspendLayout();
                fpnlUserTiles.SuspendLayout();
                selectedUserPanel.SuspendLayout();
                FlowLayoutPanel userGroupBox = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList()[0];
                RemoveSelectedScreenTile(userGroupBox, screenNumber);
                List<Panel> userScreenPanelList = userGroupBox.Controls.OfType<Panel>().ToList();
                if (userScreenPanelList == null || userScreenPanelList.Count == 0)
                {
                    selectedUserPanel.Controls.Remove(userGroupBox);
                    List<FlowLayoutPanel> subPnlList = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList();
                    if (subPnlList == null || subPnlList.Any() == false)
                    {
                        fpnlUserTiles.Controls.Remove(selectedUserPanel);
                    }
                    List<Label> userGroupTagList = pnlBase.Controls.OfType<Label>().ToList();
                    if (userGroupTagList != null)
                    {
                        Label lblUserGroupTag = userGroupTagList.Find(lbl => lbl.Tag != null && lbl.Tag.ToString() == loginId);
                        if (lblUserGroupTag != null)
                        {
                            pnlBase.Controls.Remove(lblUserGroupTag);
                        }
                    }
                }
                selectedUserPanel.ResumeLayout(true);
                fpnlUserTiles.ResumeLayout(true);
                fpnlUserTiles.Refresh();
                pnlBase.ResumeLayout(true);
            }
            SetUserTagLabelLocation();
            SetDefaultBarCodeScanner();
            log.LogMethodExit();
        }

        private void RemoveSelectedScreenTile(FlowLayoutPanel userGroupBox, string screenNumber)
        {
            log.LogMethodEntry(userGroupBox, screenNumber);
            if (userGroupBox != null)
            {
                userGroupBox.SuspendLayout();
                List<Panel> userScreenPanelList = userGroupBox.Controls.OfType<Panel>().ToList();
                int removeSceeenIndex = -1;
                if (userScreenPanelList != null)
                {
                    for (int i = 0; i < userScreenPanelList.Count; i++)
                    {
                        if (userScreenPanelList[i].Tag != null && userScreenPanelList[i].Tag.ToString() == screenNumber)
                        {
                            removeSceeenIndex = i;
                            break;
                        }
                    }

                    if (removeSceeenIndex > -1)
                    {
                        for (int i = userScreenPanelList.Count - 1; i > removeSceeenIndex; i--)
                        {
                            userScreenPanelList[i].Location = new Point(userScreenPanelList[i - 1].Location.X, userScreenPanelList[i - 1].Location.Y);
                        }
                        userGroupBox.Controls.Remove(userScreenPanelList[removeSceeenIndex]);
                    }
                }
                userGroupBox.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private void SetUserTagLabelLocation()
        {
            log.LogMethodEntry();
            pnlBase.SuspendLayout();
            List<Label> lblUserGroupTagList = pnlBase.Controls.OfType<Label>().ToList();
            List<FlowLayoutPanel> usrGroupFlowPanelList = fpnlUserTiles.Controls.OfType<FlowLayoutPanel>().ToList();
            if (lblUserGroupTagList != null)
            {
                for (int i = 0; i < lblUserGroupTagList.Count; i++)
                {
                    if (lblUserGroupTagList[i].Tag != null)
                    {
                        FlowLayoutPanel usrGroupFlowPanel = usrGroupFlowPanelList.Find(grpPnl => grpPnl.Tag != null && grpPnl.Tag.ToString() == lblUserGroupTagList[i].Tag.ToString());
                        if (usrGroupFlowPanel != null)
                        {
                            lblUserGroupTagList[i].Location = new Point(usrGroupFlowPanel.Location.X + 2, 2);
                            lblUserGroupTagList[i].Size = new Size(usrGroupFlowPanel.Width, lblUserGroupTagList[i].Height);
                        }
                    }
                }
            }
            pnlBase.ResumeLayout();
            log.LogMethodExit();
        }

        internal void UpdateScreenTileDetails(string loginId, string screenNumber, string cardNumber, string customerName, int balanceTickets)
        {
            log.LogMethodEntry(loginId, screenNumber, cardNumber, customerName, balanceTickets);
            FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginId);
            if (selectedUserPanel != null)
            {
                frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                Color backGroundColor = mdiParent.GetBackgroundColor(loginId);
                selectedUserPanel.BackColor = backGroundColor;
                SetUserTagLabelColor(loginId, backGroundColor);
                pnlBase.SuspendLayout();
                fpnlUserTiles.SuspendLayout();
                FlowLayoutPanel userGroupBox = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList()[0];
                UpdateScreenTileDetails(userGroupBox, screenNumber, cardNumber, customerName, balanceTickets);
                fpnlUserTiles.ResumeLayout(true);
                pnlBase.ResumeLayout();
            }
            log.LogMethodExit();
        }

        private void UpdateScreenTileDetails(FlowLayoutPanel userGroupBox, string screenNumber, string cardNumber, string customerName, int ticketBalance)
        {
            log.LogMethodEntry(userGroupBox, screenNumber, cardNumber, customerName, ticketBalance);
            if (userGroupBox != null)
            {
                List<Panel> userScreenPanelList = userGroupBox.Controls.OfType<Panel>().ToList();
                if (userScreenPanelList != null)
                {
                    for (int i = 0; i < userScreenPanelList.Count; i++)
                    {
                        if (userScreenPanelList[i].Tag != null
                            && userScreenPanelList[i].Tag.ToString() == screenNumber)
                        {
                            List<Label> labelList = userScreenPanelList[i].Controls.OfType<Label>().ToList();
                            if (labelList != null)
                            {
                                for (int j = 0; j < labelList.Count; j++)
                                {
                                    if (labelList[j].Name == "lblCustomerCardInfo")
                                    {
                                        labelList[j].Text = (string.IsNullOrEmpty(customerName) == false ? customerName : string.IsNullOrEmpty(cardNumber) == false ? cardNumber : "");
                                        break;
                                    }
                                }
                                for (int j = 0; j < labelList.Count; j++)
                                {
                                    if (labelList[j].Name == "lblBalanceInfo")
                                    {
                                        labelList[j].Text = MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Bal: ") + ticketBalance.ToString();
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetUserTagLabelColor(string loginId, Color backGroundColor)
        {
            log.LogMethodEntry(loginId, backGroundColor);
            pnlBase.SuspendLayout();
            List<Label> lblUserGroupTagList = pnlBase.Controls.OfType<Label>().ToList();
            //List<FlowLayoutPanel> usrGroupFlowPanelList = fpnlUserTiles.Controls.OfType<FlowLayoutPanel>().ToList();
            if (lblUserGroupTagList != null)
            {
                for (int i = 0; i < lblUserGroupTagList.Count; i++)
                {
                    if (lblUserGroupTagList[i].Tag != null && lblUserGroupTagList[i].Tag.ToString() == loginId)
                    {

                        lblUserGroupTagList[i].BackColor = backGroundColor;
                        break;
                    }
                }
            }
            pnlBase.ResumeLayout();
            log.LogMethodExit();
        }

        private bool ScreenNotLocked(string loginId)
        {
            log.LogMethodEntry(loginId);
            bool notLocked = false;
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            notLocked = (mdiParent.GetReloginAuthenticationInitiated(loginId) == true ? false : true);
            log.LogMethodExit(notLocked);
            return notLocked;
        }

        private void btnAddUser_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnAddUser.BackgroundImage = Properties.Resources.AddRedemptionUserPressed;
            log.LogMethodExit();
        }

        private void btnAddUser_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            btnAddUser.BackgroundImage = Properties.Resources.AddRedemptionUser;
            log.LogMethodExit();
        }


        private void AdjustUserTilePositions()
        {
            log.LogMethodEntry();
            List<FlowLayoutPanel> userFLowPanelList = fpnlUserTiles.Controls.OfType<FlowLayoutPanel>().ToList();
            if (userFLowPanelList != null)
            {
                int userTileCount = userFLowPanelList.Count();
                if (userTileCount > 1)
                {
                    frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                    List<string> userNames = mdiParent.RedemptionUserDetailsList.OrderBy(rUsrDetails => rUsrDetails.DeviceAddress).Select(rUsrDetails => rUsrDetails.RedemptionUser.LoginId).ToList();
                    for (int i = 0; i < userTileCount; i++)
                    {
                        fpnlUserTiles.Controls.Remove(userFLowPanelList[i]);
                    }
                    for (int i = 0; i < userNames.Count; i++)
                    {
                        string loginId = userNames[i];
                        for (int j = 0; j < userTileCount; j++)
                        {
                            if (userFLowPanelList[j].Tag != null && userFLowPanelList[j].Tag.ToString() == loginId)
                            {
                                fpnlUserTiles.Controls.Add(userFLowPanelList[j]);
                                break;
                            }
                        }
                    }
                    SetUserTagLabelLocation();
                }
            }
            log.LogMethodExit();
        }


        private void AdjustScreenTilePosition(string loginID)
        {
            log.LogMethodEntry(loginID);
            FlowLayoutPanel selectedUserPanel = GetUserFlowPanel(loginID);
            if (selectedUserPanel != null)
            {
                selectedUserPanel.SuspendLayout();
                List<FlowLayoutPanel> userPanelList = selectedUserPanel.Controls.OfType<FlowLayoutPanel>().ToList();
                if (userPanelList != null && userPanelList.Any())
                {
                    List<Panel> userScreenPanelList = userPanelList[0].Controls.OfType<Panel>().ToList();
                    int screenTileCount = userScreenPanelList.Count();
                    if (screenTileCount > 1)
                    {
                        frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
                        frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = mdiParent.RedemptionUserDetailsList.Find(rUsrDetails => rUsrDetails.RedemptionUser != null && rUsrDetails.RedemptionUser.LoginId == loginID);
                        if (redemptionUserDetails != null && redemptionUserDetails.ScreenNumberList != null && redemptionUserDetails.ScreenNumberList.Any())
                        {
                            List<string> screenNumberList = userScreenPanelList.OrderBy(pnl => pnl.Tag).Select(pnl => pnl.Tag.ToString()).ToList();
                            for (int i = 0; i < screenTileCount; i++)
                            {
                                userPanelList[0].Controls.Remove(userScreenPanelList[i]);
                            }
                            selectedUserPanel.Refresh();
                            int xCoordinate = 0;
                            for (int i = 0; i < screenNumberList.Count; i++)
                            {
                                string screenNumber = screenNumberList[i];
                                for (int j = 0; j < screenTileCount; j++)
                                {
                                    if (userScreenPanelList[j].Tag != null && userScreenPanelList[j].Tag.ToString() == screenNumber)
                                    {
                                        userPanelList[0].Controls.Add(userScreenPanelList[j]);
                                        userScreenPanelList[j].Location = new Point(xCoordinate, selectedUserPanel.Location.Y + 15);
                                        xCoordinate = xCoordinate + userScreenPanelList[j].Width + 2;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    selectedUserPanel.ResumeLayout(true);
                }
            }
            log.LogMethodExit();
        }

        //private DeviceClass RegisterCardReaderDevice()
        //{
        //    log.LogMethodEntry();
        //    DeviceClass newCardReader = null;
        //    int deviceAddress = 0;
        //    try
        //    {
        //        string serialNumber = ParafaitDefaultContainer.GetParafaitDefault(utilities.ExecutionContext, ("CARD_READER_SERIAL_NUMBER").Trim());
        //        if (!string.IsNullOrEmpty(serialNumber))
        //        {
        //            string[] serialNumbers = serialNumber.Split('|');
        //            if (serialNumbers.Length == 2)
        //            {
        //                int index = (deviceAddress);
        //                if (index == -1)
        //                {
        //                    throw new ApplicationException("Not found");
        //                }
        //                string lclSerial = serialNumbers[index];
        //                if (string.IsNullOrEmpty(lclSerial))
        //                {
        //                    newCardReader = new ACR1252U(lclSerial);
        //                }
        //                else
        //                    throw new ApplicationException("Not found");
        //            }
        //            else
        //            {
        //                log.Error("CARD_READER_SERIAL_NUMBER is not configured properly for multi user and multi screen");
        //                newCardReader = new ACR1252U(deviceAddress);
        //            }
        //        }
        //        else
        //        {
        //            newCardReader = new ACR1252U(deviceAddress);
        //        }
        //    }
        //    catch
        //    {
        //        try
        //        {
        //            newCardReader = new ACR122U(deviceAddress);
        //        }
        //        catch
        //        {
        //            try
        //            {
        //                newCardReader = new MIBlack(deviceAddress);
        //            }
        //            catch (Exception ex)
        //            {
        //                log.Error(ex);
        //            }
        //        }
        //    }


        //    if (newCardReader == null)
        //    {
        //        newCardReader = RegisterUSBDevice();
        //    }
        //    if (newCardReader != null)
        //    {
        //        utilities.getMifareCustomerKey();
        //    }
        //    log.LogMethodExit(newCardReader);
        //    return newCardReader;
        //}
        //private DeviceClass RegisterUSBDevice()
        //{
        //    log.LogMethodEntry();

        //    DeviceClass newCardReader = null;
        //    try
        //    {
        //        List<frmScanAndRedeem.Device> deviceList = new List<frmScanAndRedeem.Device>();

        //        PeripheralsListBL peripheralsListBL = new PeripheralsListBL(utilities.ExecutionContext);
        //        List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
        //        searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "CardReader"));
        //        searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (utilities.ParafaitEnv.POSMachineId).ToString()));
        //        searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, "1"));
        //        List<PeripheralsDTO> peripheralsDTOList = peripheralsListBL.GetPeripheralsDTOList(searchPeripheralsParams);
        //        if (peripheralsDTOList != null && peripheralsDTOList.Count > 0)
        //        {

        //            foreach (PeripheralsDTO peripheralsList in peripheralsDTOList)
        //            {
        //                if (peripheralsList.Vid.ToString().Trim() == string.Empty)
        //                    continue;
        //                frmScanAndRedeem.Device device = new frmScanAndRedeem.Device();

        //                device.DeviceName = peripheralsList.DeviceName.ToString();
        //                device.DeviceType = peripheralsList.DeviceType.ToString();
        //                device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
        //                device.VID = peripheralsList.Vid.ToString().Trim();
        //                device.PID = peripheralsList.Pid.ToString().Trim();
        //                device.OptString = peripheralsList.OptionalString.ToString().Trim();
        //                deviceList.Add(device);

        //            }
        //        }

        //        string USBReaderVID = ParafaitDefaultContainer.GetParafaitDefault(utilities.ExecutionContext, "USB_READER_VID");
        //        string USBReaderPID = ParafaitDefaultContainer.GetParafaitDefault(utilities.ExecutionContext, "USB_READER_PID");
        //        string USBReaderOptionalString = ParafaitDefaultContainer.GetParafaitDefault(utilities.ExecutionContext, "USB_READER_OPT_STRING");

        //        if (USBReaderVID.Trim() != string.Empty)
        //        {
        //            string[] optStrings = USBReaderOptionalString.Split('|');
        //            foreach (string optValue in optStrings)
        //            {
        //                frmScanAndRedeem.Device device = new frmScanAndRedeem.Device();
        //                device.DeviceName = "Default";
        //                device.DeviceType = "CardReader";
        //                device.DeviceSubType = "KeyboardWedge";
        //                device.VID = USBReaderVID.Trim();
        //                device.PID = USBReaderPID.Trim();
        //                device.OptString = optValue.ToString();
        //                deviceList.Add(device);
        //            }
        //        }

        //        USBDevice usbCardReader;
        //        if (IntPtr.Size == 4) //32 bit
        //            usbCardReader = new KeyboardWedge32();
        //        else
        //            usbCardReader = new KeyboardWedge64();

        //        frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
        //        int childCount = mdiParent.GetOpenScanRedeemChildFormCount();
        //        if (deviceList.Count >= childCount)
        //        {
        //            int index = 0;
        //            frmScanAndRedeem.Device deviceSelected = deviceList[index];
        //            bool flag = usbCardReader.InitializeUSBReader(this.MdiParent, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
        //            if (usbCardReader.isOpen)
        //            {
        //                newCardReader = usbCardReader;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //    }
        //    log.LogMethodExit(newCardReader);
        //    return newCardReader;
        //}
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //No action. Just added to avoid issues with event list corruption
            log.LogMethodExit();
        }


        private void SetDefaultBarCodeScanner()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            DeviceClass userBarcodeScanner = mdiParent.GetBarCodeScannerForUser(null);
            if (userBarcodeScanner == null)
            {
                RegisterBarCodeScanner();
            }
            log.LogMethodExit();
        }


        private void ResetBarcodeScannerIfUserScannerFound()
        {
            log.LogMethodEntry();
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            DeviceClass userBarcodeScanner = mdiParent.GetBarCodeScannerForUser(null);
            if (userBarcodeScanner != null && defaultBarcodeScanner != null)
            {
                defaultBarcodeScanner.UnRegister();
                defaultBarcodeScanner.Dispose();
                defaultBarcodeScanner = null;
            }
            log.LogMethodExit();
        }

        private void RegisterBarCodeScanner()
        {
            log.LogMethodEntry();
            if (defaultBarcodeScanner == null)
            {
                List<frmScanAndRedeem.Device> deviceList = new List<frmScanAndRedeem.Device>();
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
                        frmScanAndRedeem.Device device = new frmScanAndRedeem.Device();

                        device.DeviceName = peripheralsList.DeviceName.ToString();
                        device.DeviceType = peripheralsList.DeviceType.ToString();
                        device.DeviceSubType = peripheralsList.DeviceSubType.ToString();
                        device.VID = peripheralsList.Vid.ToString().Trim();
                        device.PID = peripheralsList.Pid.ToString().Trim();
                        device.OptString = peripheralsList.OptionalString.ToString().Trim();
                        deviceList.Add(device);
                    }
                }

                string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "USB_BARCODE_READER_VID");
                string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "USB_BARCODE_READER_PID");
                string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "USB_BARCODE_READER_OPT_STRING");

                if (USBReaderVID.Trim() != string.Empty)
                {
                    string[] optStrings = USBReaderOptionalString.Split('|');
                    foreach (string optValue in optStrings)
                    {
                        frmScanAndRedeem.Device device = new frmScanAndRedeem.Device();
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

                int index = 0;
                if (deviceList.Count > index)
                {
                    frmScanAndRedeem.Device deviceSelected = deviceList[index];
                    bool flag = barcodeListener.InitializeUSBReader(this.MdiParent, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                    if (barcodeListener.isOpen)
                    {
                        defaultBarcodeScanner = barcodeListener;
                        defaultBarcodeScanner.Register(BarCodeScanCompleteEventHandle);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs && utilities != null)
            {
                try
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    string scannedBarcode = utilities.ProcessScannedBarCode(checkScannedEvent.Message, utilities.ParafaitEnv.LEFT_TRIM_BARCODE, utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);

                    this.Invoke((MethodInvoker)delegate
                    {
                        ProcessBarcode(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void ProcessBarcode(string barCode)
        {
            log.LogMethodEntry(barCode);
            switch (barCode)
            {
                case SCANCODEADDUSER:
                    AddUser();
                    break;
                default:
                    //no action for other codes
                    break;
            }
            log.LogMethodExit();
        }

        private void StopUserScreenTimer(frmScanAndRedeemMDI mdiParent)
        {
            log.LogMethodEntry();
            if (mdiParent.RedemptionUserDetailsList != null && mdiParent.RedemptionUserDetailsList.Any())
            {
                frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = mdiParent.RedemptionUserDetailsList.OrderBy(rUsrDetails => rUsrDetails.DeviceAddress).ToList()[0];
                string userLogin = redemptionUserDetails.RedemptionUser.LoginId;
                string activeScreen = redemptionUserDetails.ActiveScreenNumber;
                List<frmScanAndRedeem> frmScanAndRedeemList = mdiParent.GetOpenScanRedeemChildForms(userLogin);
                if (frmScanAndRedeemList != null && frmScanAndRedeemList.Any())
                {
                    for (int i = 0; i < frmScanAndRedeemList.Count; i++)
                    {
                        if (frmScanAndRedeemList[i].GetCurrentScreenNumber == activeScreen && frmScanAndRedeemList[i].GetReloginAuthenticationInitiated() == false)
                        {
                            frmScanAndRedeemList[i].StopScreenTimer();
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void StartUserScreenTimer(frmScanAndRedeemMDI mdiParent)
        {
            log.LogMethodEntry();
            if (mdiParent.RedemptionUserDetailsList != null && mdiParent.RedemptionUserDetailsList.Any())
            {
                frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = mdiParent.RedemptionUserDetailsList.OrderBy(rUsrDetails => rUsrDetails.DeviceAddress).ToList()[0];
                string userLogin = redemptionUserDetails.RedemptionUser.LoginId;
                string activeScreen = redemptionUserDetails.ActiveScreenNumber;
                List<frmScanAndRedeem> frmScanAndRedeemList = mdiParent.GetOpenScanRedeemChildForms(userLogin);
                if (frmScanAndRedeemList != null && frmScanAndRedeemList.Any())
                {
                    for (int i = 0; i < frmScanAndRedeemList.Count; i++)
                    {
                        if (frmScanAndRedeemList[i].GetCurrentScreenNumber == activeScreen && frmScanAndRedeemList[i].GetReloginAuthenticationInitiated() == false)
                        {
                            frmScanAndRedeemList[i].StartScreenTimer();
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// LaunchNewUserScreen
        /// </summary>
        /// <param name="user"></param>
        public void LaunchNewUserScreen(Security.User user)
        {
            log.LogMethodEntry(user.LoginId);
            if (CanAddThisUser(user.LoginId) == false)
            {
                log.LogMethodExit("CanAddThisUser(user.LoginId) == false");
                return;
            }
            CreateUserScreenTile(user);
            frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            frmScanAndRedeem frs = new frmScanAndRedeem(user)
            {
                MdiParent = mdiParent
            };
            AdjustUserTilePositions();
            frs.Show();
            frs.StopDevicesOnAllScreens();
            frs.ActivateDevicesOnAllActiveScreens();
            log.LogMethodExit();
        }
    }
}
