/********************************************************************************************
 * Project Name - Parafait_POS                                                                     
 * Description  - Mdi form for redemption UI
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80.0       03-Apr-2020   Guru S A             Modified for redemption UI enhancements  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device;
using Semnox.Parafait.Device.Peripherals;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    public partial class frmScanAndRedeemMDI : Form
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RedemptionUserDetails> redemptionUserDetailsList = new List<RedemptionUserDetails>();

        public int childFormIndex = 0;
        public class RedemptionUserDetails
        {
            private Security.User redemptionUser;
            private DeviceClass cardReaderDevice;
            private int deviceAddress = -1;
            //private bool isUsbCardReader;
            private DeviceClass barcodeScannerDevice;
            private List<string> screenNumberList = new List<string>();
            private Image panelTopBackGroundImage;
            private Color backGroundColor;
            private string activeScreenNumber;
            private DateTime lastActivityDateTime;
            private bool reloginAuthenticationInitiated;

            public Security.User RedemptionUser { get { return redemptionUser; } set { redemptionUser = value; } }
            public DeviceClass CardReaderDevice { get { return cardReaderDevice; } set { cardReaderDevice = value; } }
            public DeviceClass BarcodeScannerDevice { get { return barcodeScannerDevice; } set { barcodeScannerDevice = value; } }

            public List<string> ScreenNumberList { get { return screenNumberList; } set { screenNumberList = value; } }
            public Image PanelTopBackGroundImage { get { return panelTopBackGroundImage; } set { panelTopBackGroundImage = value; } }
            public Color BackGroundColor { get { return backGroundColor; } set { backGroundColor = value; } }
            public string ActiveScreenNumber { get { return activeScreenNumber; } set { activeScreenNumber = value; } }
            //public bool IsUsbCardReader { get { return isUsbCardReader; } set { isUsbCardReader = value; } }
            public DateTime LastActivityDateTime { get { return lastActivityDateTime; } set { lastActivityDateTime = value; } }
            public bool ReloginAuthenticationInitiated { get { return reloginAuthenticationInitiated; } set { reloginAuthenticationInitiated = value; } }
            public int DeviceAddress { get { return deviceAddress; } set { deviceAddress = value; } }
        }
        public List<RedemptionUserDetails> RedemptionUserDetailsList { get { return redemptionUserDetailsList; } set { redemptionUserDetailsList = value; } }

        public class ColorThemes
        {
            private Color backColor;
            private Image topPanelBackgroundImage;
            private bool isInUse;
            public ColorThemes(Color backGroundColor, Image topPanelBackgroundImage)
            {
                this.backColor = backGroundColor;
                this.topPanelBackgroundImage = topPanelBackgroundImage;
                this.isInUse = false;
            }
            public Image TopPanelBackgroundImage { get { return topPanelBackgroundImage; } }
            public Color BackColor { get { return backColor; } }
            public bool IsInUse { get { return isInUse; } set { isInUse = value; } }
        }

        public List<ColorThemes> ColorThemeList = new List<ColorThemes>();
        public bool forceLoginForTimeOut = false;
        /// <summary>
        /// frmScanAndRedeemMDI
        /// </summary>
        public frmScanAndRedeemMDI()
        {
            log.LogMethodEntry();
            Logger.setRootLogLevel(log);
            InitializeComponent();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            SetDefaultColorThemes();
            POSStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }


        /// <summary>
        /// frmScanAndRedeemMDI_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmScanAndRedeemMDI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.AutoScroll = false;
            btnPanel.Location = new Point((this.Width - btnPanel.Width) / 2, (this.Height - btnPanel.Height) / 2);
            LaunchChildForm();
            log.LogMethodExit();
        }

        /// <summary>
        /// frmScanAndRedeemMDI_FormClosing
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmScanAndRedeemMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
            {
                forceLoginForTimeOut = true;
            }
            Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnStart_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            btnPanel.Visible = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false) )
            {
                LaunchChildFormWithNewUser();
            }
            else
            {
                LaunchChildForm();
            }
            ShowHideBtnPanel();
            log.LogMethodExit();
        }

        /// <summary>
        /// frmScanAndRedeemMDI_Activated
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmScanAndRedeemMDI_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ShowHideBtnPanel();
            log.LogMethodExit();
        }

        private void ShowHideBtnPanel()
        {
            log.LogMethodEntry();
            this.btnPanel.Visible = true;
            if (this.GetOpenScanRedeemChildFormCount() > 0)
            {
                this.btnPanel.Visible = false;
            }
            log.LogMethodExit();
        }

        private void LaunchChildForm()
        {
            log.LogMethodEntry();
            frmRedemptionScreenBanner frmRemScreenBanner = new frmRedemptionScreenBanner(POSStatic.Utilities)
            {
                MdiParent = this
            };
            frmRemScreenBanner.Show();
            log.LogMethodExit();
        }

        internal void SetScreenUser(Security.User user, string screenNumber, Image panelTopBackGroundImage, Color backGroundColor)
        {
            log.LogMethodEntry(user.LoginId, screenNumber, panelTopBackGroundImage, backGroundColor);
            if (RedemptionUserDetailsList != null
               && redemptionUserDetailsList.Any()
               && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser == user))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser == user)
                    {
                        if (redemptionUserDetails.ScreenNumberList != null
                            && redemptionUserDetails.ScreenNumberList.Exists(screen => screen == screenNumber) == false)
                        {
                            redemptionUserDetails.ScreenNumberList.Add(screenNumber);
                            redemptionUserDetails.ActiveScreenNumber = screenNumber;
                            redemptionUserDetails.LastActivityDateTime = DateTime.Now;
                            redemptionUserDetails.DeviceAddress = GetDeviceAddress(user.LoginId);
                        }
                        else
                        {
                            redemptionUserDetails.ActiveScreenNumber = screenNumber;
                            redemptionUserDetails.LastActivityDateTime = DateTime.Now;
                        }
                        break;
                    }
                }
            }
            else
            {
                RedemptionUserDetails redemptionUserDetails = new RedemptionUserDetails();
                redemptionUserDetails.ActiveScreenNumber = screenNumber;
                redemptionUserDetails.RedemptionUser = user;
                redemptionUserDetails.LastActivityDateTime = DateTime.Now;
                redemptionUserDetails.ScreenNumberList.Add(screenNumber);
                redemptionUserDetails.PanelTopBackGroundImage = panelTopBackGroundImage;
                redemptionUserDetails.BackGroundColor = backGroundColor;
                redemptionUserDetailsList.Add(redemptionUserDetails);
                redemptionUserDetails.DeviceAddress = GetDeviceAddress(user.LoginId);
            }
            log.LogMethodExit();
        }


        internal void AddCardReaderForUser(string loginId, DeviceClass cardReader, int deviceAddress)
        {
            log.LogMethodEntry(loginId, cardReader, deviceAddress);
            if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any())
            {
                for (int i = 0; i < RedemptionUserDetailsList.Count; i++)
                {
                    if (RedemptionUserDetailsList[i].RedemptionUser.LoginId == loginId)
                    {
                        RedemptionUserDetailsList[i].CardReaderDevice = cardReader;
                        RedemptionUserDetailsList[i].LastActivityDateTime = DateTime.Now;
                        if (RedemptionUserDetailsList[i].DeviceAddress == -1)
                        {
                            RedemptionUserDetailsList[i].DeviceAddress = deviceAddress;
                        }
                        log.Debug("Card reader device added to" + loginId);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal DeviceClass GetCardReaderForUser(string loginId)
        {
            log.LogMethodExit(loginId);
            DeviceClass cardReader = null;
            if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any())
            {
                frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = null;
                if (string.IsNullOrEmpty(loginId) == false)
                {
                    redemptionUserDetails = redemptionUserDetailsList.Find(uiUser => uiUser.RedemptionUser.LoginId == loginId
                                                                                     && uiUser.CardReaderDevice != null);
                }
                else
                {
                    redemptionUserDetails = redemptionUserDetailsList.OrderBy(rUsrDetails => rUsrDetails.DeviceAddress).ToList()[0];
                }
                if (redemptionUserDetails != null)
                {
                    cardReader = redemptionUserDetails.CardReaderDevice;
                    log.Debug("Getting card reader device info for " + loginId);
                }
            }
            log.LogMethodExit();
            return cardReader;
        }

        //internal bool GetCardReaderTypeForUser(string loginId)
        //{
        //    log.LogMethodExit(loginId);
        //    bool usbCardReader = false;
        //    if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any())
        //    {
        //        frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = redemptionUserDetailsList.Find(uiUser => uiUser.RedemptionUser.LoginId == loginId
        //                                                                                                                   && uiUser.CardReaderDevice != null);
        //        if (redemptionUserDetails != null)
        //        {
        //            usbCardReader = redemptionUserDetails.IsUsbCardReader;
        //            log.Debug("Getting card reader device type info for " + loginId);
        //        }
        //    }
        //    log.LogMethodExit(usbCardReader);
        //    return usbCardReader;
        //}


        internal int GetDeviceAddress(string loginId = null)
        {
            log.LogMethodEntry(loginId);
            int deviceAddress = 0;
            if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any() && string.IsNullOrWhiteSpace(loginId) == false)
            {
                RedemptionUserDetails redemptionUserDetails = RedemptionUserDetailsList.Find(rUsrDetails => rUsrDetails.RedemptionUser != null && rUsrDetails.RedemptionUser.LoginId == loginId);
                if (redemptionUserDetails != null && redemptionUserDetails.DeviceAddress > -1)
                {
                    deviceAddress = redemptionUserDetails.DeviceAddress;
                    log.LogMethodExit(deviceAddress);
                    return deviceAddress;
                }
            }
            int userCount = (string.IsNullOrWhiteSpace(loginId) ? ((RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any()) ? RedemptionUserDetailsList.Count+1 : 0) 
                                                                : ((RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any()) ? RedemptionUserDetailsList.Count : 0));
            int lcldeviceAddress = 0;
            //look for an available device, by scanning through user details
            while (lcldeviceAddress < userCount)
            {
                bool found = false;
                foreach (RedemptionUserDetails rUsrDetails in RedemptionUserDetailsList)
                {
                    if (lcldeviceAddress.Equals(rUsrDetails.DeviceAddress))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    deviceAddress = lcldeviceAddress;
                    break;
                }
                else
                    lcldeviceAddress++;
            }
            log.LogMethodExit(deviceAddress);
            return deviceAddress;
        }

        internal void AddBarCodeScannerForUser(string loginId, DeviceClass barCodeScanner, int deviceAddress)
        {
            log.LogMethodEntry(loginId, barCodeScanner, deviceAddress);
            if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any())
            {
                for (int i = 0; i < RedemptionUserDetailsList.Count; i++)
                {
                    if (RedemptionUserDetailsList[i].RedemptionUser.LoginId == loginId)
                    {
                        RedemptionUserDetailsList[i].BarcodeScannerDevice = barCodeScanner;
                        RedemptionUserDetailsList[i].LastActivityDateTime = DateTime.Now;
                        if (RedemptionUserDetailsList[i].DeviceAddress == -1)
                        {
                            RedemptionUserDetailsList[i].DeviceAddress = deviceAddress;
                        }
                        log.Debug("Barcode scanner device added to" + loginId);
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        internal DeviceClass GetBarCodeScannerForUser(string loginId)
        {
            log.LogMethodExit(loginId);
            DeviceClass barcodeScanner = null;
            if (RedemptionUserDetailsList != null && RedemptionUserDetailsList.Any())
            {
                frmScanAndRedeemMDI.RedemptionUserDetails redemptionUserDetails = null;
                if (string.IsNullOrEmpty(loginId) == false)
                {
                    redemptionUserDetails = redemptionUserDetailsList.Find(uiUser => uiUser.RedemptionUser.LoginId == loginId
                                                                                      && uiUser.BarcodeScannerDevice != null);
                }
                else
                {
                    redemptionUserDetails = redemptionUserDetailsList.OrderBy(rUsrDetails => rUsrDetails.DeviceAddress).ToList()[0];
                }
                if (redemptionUserDetails != null)
                {
                    barcodeScanner = redemptionUserDetails.BarcodeScannerDevice;
                    log.Debug("Getting card reader device info for " + loginId);
                }
            }
            log.LogMethodExit();
            return barcodeScanner;
        }

        internal void RemoveUserScreenEntry(string userLoginId, string screenNumber)
        {
            log.LogMethodEntry(userLoginId, screenNumber);
            bool removeUserEntry = false;
            Object backColorCode = null;
            if (RedemptionUserDetailsList != null
               && redemptionUserDetailsList.Any()
               && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser.LoginId == userLoginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser.LoginId == userLoginId)
                    {
                        if (redemptionUserDetails.ScreenNumberList != null
                            && redemptionUserDetails.ScreenNumberList.Exists(screen => screen == screenNumber))
                        {
                            //if (screenNumberColorTheme != null)
                            {
                                if (redemptionUserDetails.ScreenNumberList != null && redemptionUserDetails.ScreenNumberList.Count == 1)
                                {
                                    backColorCode = redemptionUserDetails.BackGroundColor;
                                }
                                redemptionUserDetails.ScreenNumberList.Remove(screenNumber);
                                if (redemptionUserDetails.ScreenNumberList != null
                                    && redemptionUserDetails.ScreenNumberList.Any())
                                {
                                    redemptionUserDetails.ActiveScreenNumber = redemptionUserDetails.ScreenNumberList[0];
                                }
                                else
                                {
                                    removeUserEntry = true;
                                }
                            }
                        }
                        break;
                    }
                }
                if (removeUserEntry)
                {
                    RedemptionUserDetails redemptionUserDetails = redemptionUserDetailsList.Find(usrDtl => usrDtl.RedemptionUser.LoginId == userLoginId);
                    if (backColorCode != null)
                    {
                        ReleaseColorTheme((Color)backColorCode);
                    }
                    redemptionUserDetailsList.Remove(redemptionUserDetails);
                }
            }
        }

        internal bool IsActiveScreen(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);
            bool isActiveScreen = false;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        if (redemptionUserDetails.ActiveScreenNumber == screenNumber)
                        {
                            isActiveScreen = true;
                            break;
                        }
                    }
                }
            }
            log.LogMethodExit(isActiveScreen);
            return isActiveScreen;
        }
        internal void SetAsActiveScreen(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);
            if (redemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        redemptionUserDetails.ActiveScreenNumber = screenNumber;
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal string GetActiveScreenNumber(string loginId)
        {
            log.LogMethodEntry(loginId);
            string activeScreenNumber = "-1";
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        activeScreenNumber = redemptionUserDetails.ActiveScreenNumber;
                        break;
                    }
                }
            }
            log.LogMethodExit(activeScreenNumber);
            return activeScreenNumber;
        }

        internal int GetUserScreenPosition(string loginId)
        {
            log.LogMethodEntry(loginId);
            int userPosition = 1;
            if (redemptionUserDetailsList != null
              && redemptionUserDetailsList.Any())
            {
                RedemptionUserDetails redemptionUserDetails = redemptionUserDetailsList.Find(usrDtl => usrDtl.RedemptionUser != null && usrDtl.RedemptionUser.LoginId == loginId);
                if (redemptionUserDetails != null)
                {
                    userPosition = redemptionUserDetails.DeviceAddress + 1;
                }
            }
            log.LogMethodExit(userPosition);
            return userPosition;
        }

        internal Image GetPanelTopBackgroundImage(string loginId)
        {
            log.LogMethodEntry(loginId);
            Image panelTopBackgroundImage = null;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId
                        && redemptionUserDetails.ScreenNumberList != null && redemptionUserDetails.ScreenNumberList.Any())
                    {
                        panelTopBackgroundImage = redemptionUserDetails.PanelTopBackGroundImage;
                        break;
                    }
                }
            }
            log.LogMethodExit(panelTopBackgroundImage);
            return panelTopBackgroundImage;
        }

        internal Color GetBackgroundColor(string loginId)
        {
            log.LogMethodEntry(loginId);
            Color backGroundColor = ColorTranslator.FromHtml("#beebfc");
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId
                        && redemptionUserDetails.ScreenNumberList != null && redemptionUserDetails.ScreenNumberList.Any())
                    {
                        backGroundColor = redemptionUserDetails.BackGroundColor;
                        break;
                    }
                }
            }
            log.LogMethodExit(backGroundColor);
            return backGroundColor;
        }

        internal Color GetUserBackgroundColor(string screenNumber)
        {
            log.LogMethodEntry(screenNumber);
            Color backGroundColor = ColorTranslator.FromHtml("#beebfc");
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any())
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.ScreenNumberList != null && redemptionUserDetails.ScreenNumberList.Exists(scr => scr == screenNumber))
                    {
                        backGroundColor = redemptionUserDetails.BackGroundColor;
                        break;
                    }
                }
            }
            log.LogMethodExit(backGroundColor);
            return backGroundColor;
        }

        //internal Color GetBackgroundColor(string loginId)
        //{
        //    log.LogMethodEntry(loginId);
        //    Color backGroundColor = ColorTranslator.FromHtml("#beebfc");
        //    if (RedemptionUserDetailsList != null
        //      && redemptionUserDetailsList.Any()
        //      && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
        //    {
        //        foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
        //        {
        //            if (redemptionUserDetails.RedemptionUser != null
        //                && redemptionUserDetails.RedemptionUser.LoginId == loginId
        //                && redemptionUserDetails.ScreenNumberColorThemeList != null)
        //            {
        //                RedemptionUserDetails.ScreenNumberColorTheme screenNumberColorTheme = redemptionUserDetails.ScreenNumberColorThemeList[0];
        //                if (screenNumberColorTheme != null)
        //                {
        //                    backGroundColor = screenNumberColorTheme.BackGroundColor;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    log.LogMethodExit(backGroundColor);
        //    return backGroundColor;
        //}

        internal int GetOpenScanRedeemChildFormCount()
        {
            log.LogMethodEntry();
            int childCount = 0;
            List<frmScanAndRedeem> openFormList = this.MdiChildren.OfType<frmScanAndRedeem>().ToList();
            if (openFormList != null && openFormList.Any())
            {
                childCount = openFormList.Count(frm => frm.IsThisFormClosed == false);
            }
            log.LogMethodExit(childCount);
            return childCount;
        }

        internal int GetOpenScanRedeemChildFormCount(string loginId)
        {
            log.LogMethodEntry(loginId);
            int childCount = 0;
            List<frmScanAndRedeem> openFormList = this.MdiChildren.OfType<frmScanAndRedeem>().ToList();
            if (openFormList != null && openFormList.Any())
            {
                childCount = openFormList.Count(frm => frm.IsThisFormClosed == false && frm.GetScreenUserLoginId == loginId);
            }
            log.LogMethodExit(childCount);
            return childCount;
        }
        internal List<frmScanAndRedeem> GetOpenScanRedeemChildForms()
        {
            log.LogMethodEntry();
            List<frmScanAndRedeem> openFormList = this.MdiChildren.OfType<frmScanAndRedeem>().ToList();
            if (openFormList != null && openFormList.Any())
            {
                openFormList = openFormList.Where(frm => frm.IsThisFormClosed == false).ToList();

            }
            if (openFormList == null)
            {
                openFormList = new List<frmScanAndRedeem>();
            }
            log.LogMethodExit(openFormList);
            return openFormList;
        }

        internal List<frmScanAndRedeem> GetOpenScanRedeemChildForms(string loginId)
        {
            log.LogMethodEntry(loginId);
            List<frmScanAndRedeem> openFormList = this.MdiChildren.OfType<frmScanAndRedeem>().ToList();
            if (openFormList != null && openFormList.Any())
            {
                openFormList = openFormList.Where(frm => frm.IsThisFormClosed == false && frm.GetScreenUserLoginId == loginId).ToList();

            }
            if (openFormList == null)
            {
                openFormList = new List<frmScanAndRedeem>();
            }
            log.LogMethodExit(openFormList);
            return openFormList;
        }

        internal void CloseAllChildForms()
        {
            log.LogMethodEntry();
            foreach (frmScanAndRedeem childForm in this.MdiChildren.OfType<frmScanAndRedeem>())
            {
                childForm.StopScreenTimer();
                childForm.MdiParent = null;
                childForm.Close();
            }
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.MdiParent = null;
                childForm.Close();
            }
            this.Focus();
            log.LogMethodExit();
        }

        internal void SetLastActivityDateTime(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        redemptionUserDetails.LastActivityDateTime = DateTime.Now;
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal DateTime GetLastActivityDateTime(string loginId)
        {
            log.LogMethodEntry(loginId);
            DateTime lastActivityDateTimeValue = DateTime.MinValue;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        lastActivityDateTimeValue = redemptionUserDetails.LastActivityDateTime;
                        break;
                    }
                }
            }
            log.LogMethodExit(lastActivityDateTimeValue);
            return lastActivityDateTimeValue;
        }

        internal void SetReloginAuthenticationInitiated(string loginId, bool reloginAuthenticationInitiatedValue)
        {
            log.LogMethodEntry(loginId, reloginAuthenticationInitiatedValue);
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        redemptionUserDetails.ReloginAuthenticationInitiated = reloginAuthenticationInitiatedValue;
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }
        internal bool GetReloginAuthenticationInitiated(string loginId)
        {
            log.LogMethodEntry(loginId);
            bool reloginAuthenticationInitiatedValue = false;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null && usr.RedemptionUser.LoginId == loginId))
            {
                foreach (RedemptionUserDetails redemptionUserDetails in redemptionUserDetailsList)
                {
                    if (redemptionUserDetails.RedemptionUser != null
                        && redemptionUserDetails.RedemptionUser.LoginId == loginId)
                    {
                        reloginAuthenticationInitiatedValue = redemptionUserDetails.ReloginAuthenticationInitiated;
                        break;
                    }
                }
            }
            log.LogMethodExit(reloginAuthenticationInitiatedValue);
            return reloginAuthenticationInitiatedValue;
        }

        internal bool ScreenIsAlredyOpened(string screenNumber)
        {
            log.LogMethodEntry(screenNumber);
            bool screenIsAlreadyOpened = false;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
               && redemptionUserDetailsList.Exists(usr => usr.ScreenNumberList != null
                                                    && usr.ScreenNumberList.Exists(scr => scr == screenNumber)))
            {
                screenIsAlreadyOpened = true;
            }
            log.LogMethodExit(screenIsAlreadyOpened);
            return screenIsAlreadyOpened;
        }

        internal bool IsScreenOwnedByUser(string loginId, string screenNumber)
        {
            log.LogMethodEntry(loginId, screenNumber);
            bool isScreenOwnedByUser = false;
            if (RedemptionUserDetailsList != null
              && redemptionUserDetailsList.Any()
              && redemptionUserDetailsList.Exists(usr => usr.RedemptionUser != null
                                                        && usr.RedemptionUser.LoginId == loginId
                                                        && usr.ScreenNumberList != null
                                                        && usr.ScreenNumberList.Exists(scr => scr == screenNumber)))
            {
                isScreenOwnedByUser = true;
            }
            log.LogMethodExit(isScreenOwnedByUser);
            return isScreenOwnedByUser;
        }


        private void SetDefaultColorThemes()
        {
            log.LogMethodEntry();
            ColorThemeList = new List<ColorThemes>();
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#beebfc"), Properties.Resources.blueGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#feedc6"), Properties.Resources.brownGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#f7feb2"), Properties.Resources.greenGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#fdd3c5"), Properties.Resources.pinkGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#fffce1"), Properties.Resources.yellowGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#f9d7ff"), Properties.Resources.lavenderGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#ffe2eb"), Properties.Resources.darkpinkGradient));
            ColorThemeList.Add(new ColorThemes(ColorTranslator.FromHtml("#eafffe"), Properties.Resources.turquoiseGradient));
            log.LogMethodExit();
        }

        private void ReleaseColorTheme(Color backColorCode)
        {
            log.LogMethodEntry(backColorCode);
            int index = ColorThemeList.IndexOf(ColorThemeList.Find(ct => ct.BackColor == backColorCode));
            if (index > -1)
            {
                ColorThemeList[index].IsInUse = false;
            }
            log.LogMethodExit();
        }

        private ColorThemes GetColorTheme(Color backColorCode)
        {
            log.LogMethodEntry(backColorCode);
            ColorThemes colorThemes = ColorThemeList.Find(ct => ct.BackColor == backColorCode);
            log.LogMethodExit(colorThemes);
            return colorThemes;
        }
        internal ColorThemes GetUserColorTheme(string loginId)
        {
            log.LogMethodEntry(loginId);
            ColorThemes colorThemes = null;
            if (redemptionUserDetailsList != null && redemptionUserDetailsList.Any())
            {
                RedemptionUserDetails rdUser = redemptionUserDetailsList.Find(rdu => rdu.RedemptionUser != null && rdu.RedemptionUser.LoginId == loginId);
                if (rdUser != null && rdUser.ScreenNumberList != null && rdUser.ScreenNumberList.Any())
                {
                    colorThemes = GetColorTheme(rdUser.BackGroundColor);
                }
            }
            if (colorThemes == null)
            {
                int index = ColorThemeList.IndexOf(ColorThemeList.Find(ct => ct.IsInUse == false));
                if (index > -1)
                {
                    ColorThemeList[index].IsInUse = true;
                    colorThemes = ColorThemeList[index];
                }
            }
            log.LogMethodExit(colorThemes);
            return colorThemes;
        }

        internal DeviceClass RegisterCardDevice(string loginId = null)
        {
            log.LogMethodEntry(loginId);

            DeviceClass cardReader = null;
            //SetLastActivityTime();
            //bool response;
            // bool newEntry = false;
            int deviceAddress = 0;
            //if (cardReader != null)
            //{
            //    cardReader.Dispose();
            //}

            //frmScanAndRedeemMDI mdiParent = Application.OpenForms[MDIFORM] as frmScanAndRedeemMDI;
            //int childCount = GetOpenScanRedeemChildFormCount();
            //bool response = true;

            //cardReader = GetCardReaderForUser();

            //if (cardReader == null)
            // {
            // newEntry = true;
            deviceAddress = GetDeviceAddress(loginId);
            try
            {
                string serialNumber = ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "CARD_READER_SERIAL_NUMBER").Trim();
                if (!string.IsNullOrEmpty(serialNumber))
                {
                    string[] serialNumbers = serialNumber.Split('|');
                    if (serialNumbers.Length > 1)
                    {
                        int index = (deviceAddress);
                        if (index == -1 || index >= serialNumbers.Length)
                        {
                            throw new ApplicationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Not found"));
                        }
                        string lclSerial = serialNumbers[index];
                        if (!string.IsNullOrEmpty(lclSerial))
                        {
                            cardReader = new ACR1252U(lclSerial);
                        }
                        else
                            throw new ApplicationException(MessageContainerList.GetMessage(POSStatic.Utilities.ExecutionContext, "Not found"));
                    }
                    else
                    {
                        log.Error("CARD_READER_SERIAL_NUMBER is not configured properly for multi user and multi screen");
                        cardReader = new ACR1252U(deviceAddress);
                    }
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
                        cardReader = null;
                    }
                }
            }
            // }


            if (cardReader == null)
            {
                //if (newEntry)
                //{
                //    mdiParent.AddCardReaderForUser(_user.LoginId, cardReader, deviceAddress);
                //}
                //}
                //else
                cardReader = RegisterUSBCardDevice(loginId);
            }

            if (cardReader != null)
                POSStatic.Utilities.getMifareCustomerKey();
            log.LogMethodExit(cardReader);
            return cardReader;
        }
         
        private DeviceClass RegisterUSBCardDevice(string loginId = null)
        {
            log.LogMethodEntry(loginId);
            DeviceClass cardReader = null;
            // if (newEntry)
            //{
            List<frmScanAndRedeem.Device> deviceList = new List<frmScanAndRedeem.Device>();

            PeripheralsListBL peripheralsListBL = new PeripheralsListBL(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchPeripheralsParams = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.DEVICE_TYPE, "CardReader"));
            searchPeripheralsParams.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, (POSStatic.Utilities.ParafaitEnv.POSMachineId).ToString()));
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

            string USBReaderVID = ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "USB_READER_VID");
            string USBReaderPID = ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "USB_READER_PID");
            string USBReaderOptionalString = ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "USB_READER_OPT_STRING");

            if (USBReaderVID.Trim() != string.Empty)
            {
                string[] optStrings = USBReaderOptionalString.Split('|');
                foreach (string optValue in optStrings)
                {
                    frmScanAndRedeem.Device device = new frmScanAndRedeem.Device();
                    device.DeviceName = "Default";
                    device.DeviceType = "CardReader";
                    device.DeviceSubType = "KeyboardWedge";
                    device.VID = USBReaderVID.Trim();
                    device.PID = USBReaderPID.Trim();
                    device.OptString = optValue.ToString();
                    deviceList.Add(device);
                }
            }

            //EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);

            if (cardReader != null)
                cardReader.Dispose();

            USBDevice usbCardReader;
            if (IntPtr.Size == 4) //32 bit
                usbCardReader = new KeyboardWedge32();
            else
                usbCardReader = new KeyboardWedge64();

            int childCount = GetOpenScanRedeemChildFormCount();
            if (deviceList.Count >= childCount)
            {
                int index = GetDeviceAddress(loginId);
                if (index < deviceList.Count)
                {
                    frmScanAndRedeem.Device deviceSelected = deviceList[index];
                    bool flag = usbCardReader.InitializeUSBReader(this, deviceSelected.VID, deviceSelected.PID, deviceSelected.OptString.Trim());
                    if (usbCardReader.isOpen)
                    {
                        cardReader = usbCardReader;
                        //mdiParent.AddCardReaderForUser(_user.LoginId, cardReader, deviceAddress);
                        //SetLastActivityTime();
                        log.LogMethodExit(cardReader);
                        return cardReader;
                    }
                }
            }

            log.Info("Unable to find USB card reader");
            log.LogMethodExit(cardReader);
            return cardReader;
        }

        private void LaunchChildFormWithNewUser()
        {
            log.LogMethodEntry();
            frmRedemptionScreenBanner frmRemScreenBanner = new frmRedemptionScreenBanner(POSStatic.Utilities)
            {
                MdiParent = this
            };
            frmRemScreenBanner.AddFromMDI = true;
            frmRemScreenBanner.Show();
            frmRemScreenBanner.AddUser();
            int childFrmCount = GetOpenScanRedeemChildFormCount();
            if (childFrmCount == 0)
            {
                frmRemScreenBanner.Close();
            }
            log.LogMethodExit();
        }
    }
}
