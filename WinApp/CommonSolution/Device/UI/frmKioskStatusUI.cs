//using Semnox.Parafait.PaymentGateway.Properties;
using Semnox.Core.Utilities;
using System;
/********************************************************************************************
 * Project Name - frmKioskStatic
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.80.1      02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.150.1     22-Feb-2023      Guru S A             Kiosk Cart Enhancements
 *2.150.3     12-May-2023      Vignesh Bhat         TableTop Kiosk Changes
 ********************************************************************************************/
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// This ui is used for kiosk
    /// </summary>
    public partial class frmKioskStatusUI : Form, IDisplayStatusUI
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        delegate void DisplayWindow();
        delegate void DisplayTextBox(string text);
        delegate void EnableCancel(bool isEnable);
        delegate void EnableCheckNow(bool isEnable);
        delegate void CloseWindow();
        private ExecutionContext executionContext;
        private System.Drawing.Imaging.ImageFormat fileType;
        private List<LookupValuesContainerDTO> kioskScreenLookupSetupList;
        private int timerCountDown = 90;
        private int timerCounter = 90;
        private const string KIOSK_LOOKUP_FOR_SETUP = "KIOSK_SCREEN_IMAGE";
        private const string BACKGROUND_SCREEN_IMAGE = "BackgroundScreenImage";
        private const string YES = "YES";
        private const string ENABLE_STATUS_UI_TIMER = "EnableStatusUITimer";
        private const string STATUS_UI_TIMEOUT_VALUE = "StatusUITimeoutValue";
        private const string STATUS_UI_TEXT_BG_IMAGE = "StatusUITextBgImage";
        private const string STATUS_UI_TEXT_COLOR = "StatusUITextColor";
        private const string STATUS_UI_FONT_NAME = "KioskFontFamilyName";
        private string kioskType;
        private const string TABLETOP = "TABLETOP";
        private const string PORTRAIT = "PORTRAIT";
        private const string CANCEL_BUTTON_WIDTH = "CancelButtonWidth";
        private const string CANCEL_BUTTON_HEIGHT = "CancelButtonHeight";
        private const string STATUS_UI_BTNCANCEL_TEXT_COLOR = "StatusUIBtnCancelTextForeColor";
        private const string STATUS_UI_LBLMESSAGE_TEXT_COLOR = "StatusUILblMessageTextForeColor";
        private const string STATUS_UI_LBLTITLE_TEXT_COLOR = "StatusUILblTitleTextForeColor";
        private const string STATUS_UI_LBLCARDCHARGED_TEXT_COLOR = "StatusUILblCardChargedTextForeColor";
        private const int TABLETOP_HEIGHT = 1000;

        /// <summary>
        /// Cancel clicked
        /// </summary>
        public event EventHandler CancelClicked;
        public event EventHandler CheckNowClicked;
        /// <summary>
        /// Kiosk UI
        /// </summary>
        public frmKioskStatusUI(ExecutionContext executionContext, string text, string amountChargeText)
        {
            log.LogMethodEntry(text, amountChargeText);
            InitializeComponent();
            this.executionContext = executionContext;
            SetKioskType();
            kioskScreenLookupSetupList = GetLookupSetupForKiosk();
            SetBackgrounImageAndSize();
            AdjustSizeAndLocation(this.Height);
            lblTitle.Text = text;
            if (string.IsNullOrEmpty(amountChargeText))
            {
                lblCardCharged.Visible = false;
            }
            lblCardCharged.Text = amountChargeText;
            //this.lblMessage.Size = new Size(this.Width - 100, lblMessage.Height);
            //this.lblTitle.Size = new Size(this.Width - 100, lblTitle.Height);
            //this.lblCardCharged.Size = new Size(this.Width - 20, lblCardCharged.Height);
            this.btnCancel.Location = new Point((this.Width / 2) - (btnCancel.Width / 2), btnCancel.Location.Y);
            SetFont();
            SetTextForeColorAndSize();
            this.TopMost = true;
            log.LogMethodExit();
        }

        private void frmKioskStatusUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            EnableTimer();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CancelClicked(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //this.Close();
            log.LogMethodExit();
        }
        private void btnCheckNow_Click(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            CheckNowClicked(sender, e);
            //this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //this.Close();
            //log.LogMethodExit(null);
        }
        /// <summary>
        /// Load the form
        /// </summary>
        public void ShowStatusWindow()
        {
            log.LogMethodEntry();
            try
            {
                if (this.InvokeRequired)
                {
                    DisplayWindow delegateFunction = new DisplayWindow(ShowStatusWindow);
                    Invoke(delegateFunction, new object[] { });
                }
                else
                {
                    this.ShowDialog();
                }
            }
            catch { }
            log.LogMethodExit();
        }
        /// <summary>
        /// Display the text
        /// </summary>
        /// <param name="text"></param>
        public void DisplayText(string text)
        {
            try
            {
                if (lblMessage.InvokeRequired)
                {
                    DisplayTextBox delegateFunction = new DisplayTextBox(DisplayText);
                    Invoke(delegateFunction, new object[] { text });
                }
                else
                {
                    lblMessage.Text = text;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Enable cancel button
        /// </summary>
        /// <param name="isEnable"></param>
        public void EnableCancelButton(bool isEnable)
        {
            log.LogMethodEntry();
            try
            {
                if (btnCancel.InvokeRequired)
                {
                    EnableCancel delegateFunction = new EnableCancel(EnableCancelButton);
                    Invoke(delegateFunction, new object[] { isEnable });
                }
                else
                {
                    btnCancel.Visible = isEnable;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        public void EnableCheckNowButton(bool isEnable)
        {
            if (btnCheckNow.InvokeRequired)
            {
                EnableCheckNow delegateFunction = new EnableCheckNow(EnableCheckNowButton);
                BeginInvoke(delegateFunction, new object[] { isEnable });
            }
            else
            {
                btnCheckNow.Visible = isEnable;
                //if (isEnable)
                //{
                //    btnCheckNow.Location = new System.Drawing.Point(344, 80);
                //    btnCancel.Location = new System.Drawing.Point(144, 80);
                //}
                //else
                //{
                //    btnCancel.Location = new System.Drawing.Point(244, 80);
                //}
            }
        }
        /// <summary>
        /// Close window
        /// </summary>        
        public void CloseStatusWindow()
        {
            log.LogMethodEntry();
            try
            {
                if (this.InvokeRequired)
                {
                    CloseWindow delegateFunction = new CloseWindow(CloseStatusWindow);
                    Invoke(delegateFunction, new object[] { });
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetBackgrounImageAndSize()
        {
            log.LogMethodEntry();
            Image backgroundImage = Semnox.Parafait.Device.Properties.Resources.PaymentScreen;

            string themeFolderFullPath = Application.StartupPath + @"\Media\Images\";
            try
            {
                this.BackgroundImage = backgroundImage;
                this.Size = this.BackgroundImage.Size;
                if (kioskType == TABLETOP)
                {
                    this.Size = new Size(this.BackgroundImage.Width, TABLETOP_HEIGHT);
                }
                //no animation
                this.pBxbackGroundPicureBox.Visible = false;
                this.pnlbackGroundPanel.Visible = false;
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == BACKGROUND_SCREEN_IMAGE).ToList();
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description))
                        {
                            string fileNameWithPath = themeFolderFullPath + lookupValuesDTOList[0].Description.Trim();
                            if (File.Exists(fileNameWithPath) == false)
                            {
                                fileNameWithPath = lookupValuesDTOList[0].Description.Trim(); //Assuming that full path is provided
                            }

                            backgroundImage = Image.FromFile(fileNameWithPath);
                            fileType = backgroundImage.RawFormat;
                            if (fileType.Equals(System.Drawing.Imaging.ImageFormat.Gif))
                            {
                                this.pBxbackGroundPicureBox.Visible = true;
                                this.pnlbackGroundPanel.Visible = true;
                                this.pBxbackGroundPicureBox.Location = new Point(0, 0);
                                this.pnlbackGroundPanel.Location = new Point(0, 0);
                                this.pBxbackGroundPicureBox.Image = backgroundImage;
                                this.pBxbackGroundPicureBox.Size = new Size(backgroundImage.Width, backgroundImage.Height);
                                this.pBxbackGroundPicureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                                this.pnlbackGroundPanel.Size = new Size(backgroundImage.Width, backgroundImage.Height);
                                this.BackgroundImage = backgroundImage;
                                this.BackgroundImageLayout = ImageLayout.Stretch;
                                this.Size = backgroundImage.Size;
                            }
                            else
                            {
                                this.BackgroundImage = backgroundImage;
                                this.Size = this.BackgroundImage.Size;
                            }
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

        private List<LookupValuesContainerDTO> GetLookupSetupForKiosk()
        {
            log.LogMethodEntry();
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(-1, KIOSK_LOOKUP_FOR_SETUP);
            log.LogMethodExit(lookupsContainerDTO.LookupValuesContainerDTOList);
            return lookupsContainerDTO.LookupValuesContainerDTOList;
        }

        private void EnableTimer()
        {
            log.LogMethodEntry();
            try
            {
                int xLocation = 100;
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == ENABLE_STATUS_UI_TIMER).ToList();
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description))
                        {
                            if (lookupValuesDTOList[0].Description.Trim().ToUpper() == YES)
                            {
                                timerCountDown = timerCounter = SetTimerCOuntDownValue();
                                int lblTimeoutHeight = lblTimeOut.Height;
                                lblTimeOut.BackgroundImage = SetTimeoutLabelBackgroundImage();
                                lblTimeOut.ForeColor = SetTimerTextCOlor();
                                lblTimeOut.Visible = true;
                                lblTimeOut.Size = new Size(xLocation, lblTimeoutHeight);
                                this.statusUITimer.Enabled = true;
                                this.statusUITimer.Start();
                            }
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
        private int SetTimerCOuntDownValue()
        {
            log.LogMethodEntry();
            int retValue = 90;
            try
            {
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_TIMEOUT_VALUE).ToList();
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description) == false)
                        {
                            retValue = Convert.ToInt32(lookupValuesDTOList[0].Description.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        private Image SetTimeoutLabelBackgroundImage()
        {

            log.LogMethodEntry();
            Image retValue = global::Semnox.Parafait.Device.Properties.Resources.timer_SmallBox;
            try
            {
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_TEXT_BG_IMAGE).ToList();
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description) == false)
                        {
                            string themeFolderFullPath = Application.StartupPath + @"\Media\Images\";
                            string fileNameWithPath = themeFolderFullPath + lookupValuesDTOList[0].Description.Trim();
                            retValue = Image.FromFile(fileNameWithPath);
                            retValue = Image.FromFile(fileNameWithPath);
                            if (retValue.Width > lblTimeOut.Width)
                            {
                                lblTimeOut.Size = new Size(retValue.Width, retValue.Height);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }


        private Color SetTimerTextCOlor()
        {
            log.LogMethodEntry();
            Color retValue = Color.White;
            try
            {
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_TEXT_COLOR).ToList();
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description) == false)
                        {
                            retValue = Color.FromName(lookupValuesDTOList[0].Description.Trim());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
        private void StatusUITimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                statusUITimer.Stop();
                timerCounter--;
                if (timerCounter > 0)
                {
                    lblTimeOut.Text = timerCounter.ToString("#0");
                }
                else
                {
                    lblTimeOut.Text = "";
                }
            }
            finally
            {
                if (timerCounter > -1)
                {
                    statusUITimer.Start();
                }
            }
            log.LogMethodExit();
        }

        private void frmKioskStatusUI_Closed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (statusUITimer != null)
                {
                    statusUITimer.Stop();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void AdjustSizeAndLocation(int height)
        {
            log.LogMethodEntry();
            try
            {
                int yDiffSmall = 12;
                int xFactor = 100;
                int yFactor = 70;
                double lblCardChargedPercentage = 0.47;
                double lblMessagePercentage = 0.6;
                double btnCancelPercentage = 0.9;
                double PercentagelblCardCharged = 0.40;
                double PercentagelblTimeOut = (this.Height < 1300)? 0.865: 0.89;
                double PercentagebtnCancel = 0.87;

                if (height < 1300)
                {
                    this.lblTitle.Font = new System.Drawing.Font(this.lblTitle.Font.FontFamily, 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.lblCardCharged.Font = new System.Drawing.Font(this.lblCardCharged.Font.FontFamily, 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.lblMessage.Font = new System.Drawing.Font(this.lblMessage.Font.FontFamily, 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
                this.lblTitle.Size = new Size(this.Width - xFactor, this.lblTitle.Height - yDiffSmall);
                this.lblTitle.Location = new Point(xFactor / 2, this.lblTitle.Location.Y);
                this.lblCardCharged.Size = new Size(this.Width - xFactor, this.lblCardCharged.Height * 2);
                int lblCardChargedLocY = (int)(this.Height * lblCardChargedPercentage);
                this.lblCardCharged.Location = new Point(xFactor / 2, lblCardChargedLocY);
                this.lblMessage.Size = new Size(this.Width - xFactor, this.lblMessage.Height);
                int lblMessageLocY = (int)(this.Height * lblMessagePercentage);
                this.lblMessage.Location = new Point(xFactor / 2, lblMessageLocY);
                int lblTimeOutLocX = (int)(this.Width * PercentagelblTimeOut); 
                this.lblTimeOut.Size = new Size(xFactor, this.lblTimeOut.Height);
                int lblTimeOutLocY = this.lblTitle.Location.Y + this.lblTitle.Height / 2 - lblTimeOut.Height / 2;
                this.lblTimeOut.Location = new Point(lblTimeOutLocX, lblTimeOutLocY);
                int btnCancelLocY = (int)(this.Height * btnCancelPercentage);
                this.btnCancel.Location = new Point(this.btnCancel.Location.X, btnCancelLocY);
                if (kioskType == TABLETOP)
                {
                    yDiffSmall = (this.lblTimeOut.BackgroundImage.Height <= 107) ? 0 : 12;
                    this.lblTitle.Size = new Size(this.Width - xFactor, this.lblTitle.Height - yDiffSmall);
                    int lblCardChargedHeight = (int)(this.Height * PercentagelblCardCharged);
                    this.lblCardCharged.Location = new Point(xFactor / 2, lblCardChargedHeight);
                    this.lblTimeOut.Location = new Point(lblTimeOutLocX, this.lblTimeOut.Location.Y - yDiffSmall);
                    this.lblMessage.Size = new Size(this.Width - xFactor, this.lblMessage.Height - yFactor);
                    int btnCancelLocHeight = (int)(this.Height * PercentagebtnCancel);
                    this.btnCancel.Location = new Point(this.btnCancel.Location.X, btnCancelLocHeight);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            try
            {
                string fontFamilyName = string.Empty;

                LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(-1, KIOSK_LOOKUP_FOR_SETUP);
                List<LookupValuesContainerDTO> lookupValuesDTOList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_FONT_NAME).ToList();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    if (!string.IsNullOrWhiteSpace(lookupValuesDTOList[0].Description))
                    {
                        fontFamilyName = lookupValuesDTOList[0].Description;
                        this.btnCancel.Font = new System.Drawing.Font(fontFamilyName, this.btnCancel.Font.Size, this.btnCancel.Font.Style, this.btnCancel.Font.Unit, this.btnCancel.Font.GdiCharSet);
                        this.btnCheckNow.Font = new System.Drawing.Font(fontFamilyName, this.btnCheckNow.Font.Size, this.btnCheckNow.Font.Style, this.btnCheckNow.Font.Unit, this.btnCheckNow.Font.GdiCharSet);
                        this.lblMessage.Font = new System.Drawing.Font(fontFamilyName, this.lblMessage.Font.Size, this.lblMessage.Font.Style, this.lblMessage.Font.Unit, this.lblMessage.Font.GdiCharSet);
                        this.lblTitle.Font = new System.Drawing.Font(fontFamilyName, this.lblTitle.Font.Size, this.lblTitle.Font.Style, this.lblTitle.Font.Unit, this.lblTitle.Font.GdiCharSet);
                        this.lblCardCharged.Font = new System.Drawing.Font(fontFamilyName, this.lblCardCharged.Font.Size, this.lblCardCharged.Font.Style, this.lblCardCharged.Font.Unit, this.lblCardCharged.Font.GdiCharSet);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetKioskType()
        {
            log.LogMethodEntry();
            kioskType = PORTRAIT;
            string kioskTypeValue = ConfigurationManager.AppSettings["KIOSK_TYPE"];
            if (string.IsNullOrWhiteSpace(kioskTypeValue) == false)
            {
                if (kioskTypeValue == TABLETOP)
                {
                    kioskType = TABLETOP;
                }
            }
            log.LogMethodExit();
        }

        private void SetTextForeColorAndSize()
        {
            log.LogMethodEntry();
            try
            {
                if (kioskScreenLookupSetupList != null && kioskScreenLookupSetupList.Any())
                {
                    List<LookupValuesContainerDTO> btnCancelList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_BTNCANCEL_TEXT_COLOR).ToList();
                    if (btnCancelList != null && btnCancelList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(btnCancelList[0].Description) == false)
                        {
                            try
                            {
                                this.btnCancel.ForeColor = Color.FromName(btnCancelList[0].Description.Trim());
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    List<LookupValuesContainerDTO> lblCardChargedList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_LBLCARDCHARGED_TEXT_COLOR).ToList();
                    if (lblCardChargedList != null && lblCardChargedList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lblCardChargedList[0].Description) == false)
                        {
                            try
                            {
                                this.lblCardCharged.ForeColor = Color.FromName(lblCardChargedList[0].Description.Trim());

                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    List<LookupValuesContainerDTO> lblMessageList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_LBLMESSAGE_TEXT_COLOR).ToList();
                    if (lblMessageList != null && lblMessageList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lblMessageList[0].Description) == false)
                        {
                            try
                            {
                                this.lblMessage.ForeColor = Color.FromName(lblMessageList[0].Description.Trim());
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    List<LookupValuesContainerDTO> lblTitleList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == STATUS_UI_LBLTITLE_TEXT_COLOR).ToList();
                    if (lblTitleList != null && lblTitleList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(lblTitleList[0].Description) == false)
                        {
                            try
                            {
                                this.lblTitle.ForeColor = Color.FromName(lblTitleList[0].Description.Trim());
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    List<LookupValuesContainerDTO> btnCancelWidthList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == CANCEL_BUTTON_WIDTH).ToList();
                    if (btnCancelWidthList != null && btnCancelWidthList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(btnCancelWidthList[0].Description) == false)
                        {
                            try
                            {
                                int xFactor = 300;
                                int btnCancelWidth = Convert.ToInt32(btnCancelWidthList[0].Description.Trim());
                                if (btnCancelWidth <= (this.Width - xFactor))
                                {
                                    this.btnCancel.Width = btnCancelWidth;
                                }
                                else
                                {
                                    log.Info("Cancel Button Width should not be more than " + (this.Width - xFactor));
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    List<LookupValuesContainerDTO> btnCancelHeightList = kioskScreenLookupSetupList.Where(lv => lv.LookupValue == CANCEL_BUTTON_HEIGHT).ToList();
                    if (btnCancelHeightList != null && btnCancelHeightList.Any())
                    {
                        if (string.IsNullOrWhiteSpace(btnCancelHeightList[0].Description) == false)
                        {
                            try
                            {
                                int yFactor = 120;
                                int btnCancelHeight = Convert.ToInt32(btnCancelHeightList[0].Description.Trim());
                                if (btnCancelHeight <= yFactor)
                                {
                                    this.btnCancel.Height = btnCancelHeight;
                                }
                                else
                                {
                                    log.Info("Cancel Button Height should not be more than 120");
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                    this.btnCancel.Location = new Point((this.Width / 2) - (btnCancel.Width / 2), btnCancel.Location.Y);
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
