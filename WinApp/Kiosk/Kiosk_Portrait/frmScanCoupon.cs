/********************************************************************************************
 * Project Name - Parafait Kiosk - frmScanCoupon 
 * Description  - frmScanCoupon
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       05-Sep-2018      Guru S A           Modified for device container changes
 *2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmScanCoupon : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string codeScaned;
        public string CodeScaned { get { return codeScaned; } }
        DeviceClass barcodeScannerDevice = null;
        string applicability;
        string headerText;
        string lableText;
        string okButtonText;
        string cancelButtonText;
        private bool enableKeyboard = false;
        delegate void DisplayCoupon(string couponNumber);
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmScanCoupon(KioskTransaction kioskTransactionIn)
        {
            log.LogMethodEntry("kioskTransactionIn");
            InitializeComponent();
            this.kioskTransaction = kioskTransactionIn;
            btnShowKeyPad.Visible = false;
            KioskStatic.setDefaultFont(this);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        public frmScanCoupon(string applicability, string headerText, string lableText, string okButtonText, string cancelButtonText)
        {
            log.LogMethodEntry(applicability, headerText, lableText, okButtonText, cancelButtonText);
            InitializeComponent();

            this.applicability = applicability;
            this.headerText = headerText;
            this.lableText = lableText;
            this.okButtonText = okButtonText;
            this.cancelButtonText = cancelButtonText;
            btnShowKeyPad.Visible = false;
            KioskStatic.setDefaultFont(this);
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void frmScanCoupon_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.ActiveControl = txtCouponNo;
            btnShowKeyPad.Visible = false;
            ShowhideKeypad('H');
            enableKeyboard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_KEYBOARD_IN_COUPON_SCREEN", false);
            if (enableKeyboard)
            {
                btnShowKeyPad.Visible = true;
            }
            if (!string.IsNullOrEmpty(applicability))
            {
                if (!string.IsNullOrEmpty(headerText))
                {
                    lblHeader.Text = KioskStatic.Utilities.MessageUtils.getMessage(headerText);
                }
                if (!string.IsNullOrEmpty(lableText))
                {
                    lblCoupon.Text = KioskStatic.Utilities.MessageUtils.getMessage(lableText) + ": ";
                }
                if (!string.IsNullOrEmpty(okButtonText))
                {
                    btnApply.Text = KioskStatic.Utilities.MessageUtils.getMessage(okButtonText);
                }
                if (!string.IsNullOrEmpty(cancelButtonText))
                {
                    btnClose.Text = KioskStatic.Utilities.MessageUtils.getMessage(cancelButtonText);
                }
                if (applicability.Equals("Ipay"))
                {
                    btnShowKeyPad.Visible = false;
                }
                //ShowhideKeypad('H');
            }
            else
            {
                lblCoupon.Text = KioskStatic.Utilities.MessageUtils.getMessage(lblCoupon.Text);
                btnApply.Text = KioskStatic.Utilities.MessageUtils.getMessage(btnApply.Text);
                btnClose.Text = KioskStatic.Utilities.MessageUtils.getMessage(btnClose.Text);
            }

            //if (!RegisterBarcodeScanner())
            //    this.Close();
            try
            {
                barcodeScannerDevice = DeviceContainer.RegisterBarcodeScanner(KioskStatic.Utilities.ExecutionContext, Application.OpenForms["frmHome"], BarCodeScanCompleteEventHandle);
            }

            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile(ex.Message);
                if (enableKeyboard == false)
                {
                    frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4845));  //Unable to Activate Barcode Scanner. Please Contact Staff
                    this.Close();
                }
                else
                {
                    ShowhideKeypad('S');
                }
            }
            log.LogMethodExit();
        }
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ShowhideKeypad('H');
                if (e is DeviceScannedEventArgs)
                {
                    DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                    string scannedBarcode = KioskStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, KioskStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, KioskStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                    DisplayScanedText(scannedBarcode);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodEntry();
        }
        private void DisplayScanedText(string text)
        {
            log.LogMethodEntry(text);
            ResetKioskTimer();
            if (this.txtCouponNo.InvokeRequired)
            {
                DisplayCoupon delegateFunction = new DisplayCoupon(DisplayScanedText);
                this.BeginInvoke(delegateFunction, new object[] { text });
            }
            else
            {
                if (!string.IsNullOrEmpty(applicability))
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    codeScaned = text;
                    this.Close();
                }
                txtCouponNo.Text = text;
                log.Info("Coupon Number is :" + text);
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            if (tickSecondsRemaining < 20)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            else
                setKioskTimerSecondsValue(tickSecondsRemaining - 1);
            log.LogMethodExit();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if (!string.IsNullOrEmpty(applicability))
                {
                    if (string.IsNullOrEmpty(txtCouponNo.Text))
                    {
                        frmOKMsg.ShowUserMessage(KioskStatic.Utilities.MessageUtils.getMessage(1551));
                        //DialogResult = System.Windows.Forms.DialogResult.OK;
                        //this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    ApplyDiscount();
                }
                codeScaned = txtCouponNo.Text;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ShowhideKeypad('H');
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frmScanCoupon_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ShowhideKeypad('H');
                if (barcodeScannerDevice != null)
                {
                    barcodeScannerDevice.UnRegister();
                    barcodeScannerDevice.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        private void ApplyDiscount()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            string message = string.Empty;
            DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(KioskStatic.Utilities.ExecutionContext, txtCouponNo.Text);
            if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
            {
                try
                {
                    kioskTransaction.ApplyDiscountCoupon(txtCouponNo.Text);
                }
                catch (Exception ex)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Discount is not applied..");
                    log.Error(message, ex);
                    KioskStatic.logToFile(message + " :" + ex.Message);
                    frmOKMsg.ShowUserMessage(ex.Message);
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
                this.Close();
            }
            else
            {

                if (discountCouponsBL.CouponStatus == CouponStatus.EXPIRED)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Expired coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.INEFFECTIVE)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Issued coupon not yet active");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.INVALID)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Invalid coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.IN_ACTIVE)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Inactive coupon");
                }
                else if (discountCouponsBL.CouponStatus == CouponStatus.USED)
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Used coupon");
                }
                log.Error(message);
                KioskStatic.logToFile(txtCouponNo.Text + ": " + message);
                frmOKMsg.ShowUserMessage(txtCouponNo.Text + ": " + message);
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (btnShowKeyPad.Visible)
            {
                currentActiveText = this.txtCouponNo;
                if (currentActiveText.Text == "")
                {
                    if (keypad != null)
                    {
                        keypad.LowerCase();
                    }
                }
                ShowhideKeypad('S');
            }

            ResetKioskTimer();
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        public TextBox currentActiveText;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            ShowhideKeypad('T'); // toggle
            log.LogMethodExit();
        }

        void ShowhideKeypad(char mode = 'N') // T for toggle, S for show and H for hide, 'N' for nothing
        {
            log.LogMethodEntry(mode);
            int yFactorLocation = 125;
            try
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    if (currentActiveText == null)
                    {
                        currentActiveText = txtCouponNo;
                    }
                    keypad = new AlphaNumericKeyPad(this, currentActiveText, KioskStatic.CurrentTheme.KeypadSizePercentage);
                    keypad.Location = new Point(this.Location.X + (this.Width - keypad.Width) / 2, panelCouponNo.Location.Y + yFactorLocation);

                }

                if (mode == 'T')
                {
                    if (keypad.Visible)
                        keypad.Hide();
                    else
                    {
                        keypad.Location = new Point(this.Location.X + (this.Width - keypad.Width) / 2, panelCouponNo.Location.Y + yFactorLocation);
                        keypad.Show();
                        keypad.LowerCase();
                    }
                }
                else if (mode == 'S')
                {
                    keypad.Location = new Point(this.Location.X + (this.Width - keypad.Width) / 2, panelCouponNo.Location.Y + yFactorLocation);
                    keypad.Show();
                    keypad.LowerCase();
                }
                else if (mode == 'H')
                    keypad.Hide();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();

        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmScanCoupon");
            try
            {
                this.lblHeader.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenHeaderTextForeColor;
                this.lblCoupon.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenCouponHeaderTextForeColor;
                this.txtCouponNo.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenCouponInfoTextForeColor;
                this.btnApply.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenBtnApplyTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenBtnCloseTextForeColor;
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.ScanCouponBox);
                btnApply.BackgroundImage =
                    btnClose.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.ScanCouponButtons);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmScanCoupon: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
