/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmScanCoupon. 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
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
        //clsKioskTransaction kioskTransaction;
        string applicability;
        string headerText;
        string lableText;
        string okButtonText;
        string cancelButtonText;
        delegate void DisplayCoupon(string couponNumber); 
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmScanCoupon(KioskTransaction kioskTransactionIn)
        {
            log.LogMethodEntry("kioskTransactionIn");
            InitializeComponent();
            //this.kioskTransaction = kioskTransaction;
            this.kioskTransaction = kioskTransactionIn;
            btnShowKeyPad.Visible = false;
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
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmScanCoupon_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.ActiveControl = txtCouponNo;

            if (!string.IsNullOrEmpty(applicability))
            {
                if (!string.IsNullOrEmpty(headerText))
                {
                    lblScanCoupon.Text = KioskStatic.Utilities.MessageUtils.getMessage(headerText);
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
                ShowhideKeypad('H');
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
                //Unable to Activate Barcode Scanner. Please Contact Staff
                frmOKMsg.ShowUserMessage(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4845));
                this.Close();
            }
            log.LogMethodExit();
        }
        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = KioskStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, KioskStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, KioskStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                DisplayScanedText(scannedBarcode); 
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
            if (!string.IsNullOrEmpty(applicability))
            {
                ShowhideKeypad('H');
            }

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void frmScanCoupon_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(applicability))
            {
                ShowhideKeypad('H');
            }
            if (barcodeScannerDevice != null)
            {
                barcodeScannerDevice.UnRegister();
                barcodeScannerDevice.Dispose();
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
        public Label currentActiveText;
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
            if (keypad == null || keypad.IsDisposed)
            {
                if (currentActiveText == null)
                {
                    currentActiveText = txtCouponNo;
                }
                keypad = new AlphaNumericKeyPad(this, currentActiveText, KioskStatic.CurrentTheme.KeypadSizePercentage);
                keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height - 50);
            }

            if (mode == 'T')
            {
                if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height - 50);
                    keypad.Show();
                }
            }
            else if (mode == 'S')
            {
                keypad.Location = new Point((this.Width - keypad.Width) / 2, this.Height - keypad.Height - 50);
                keypad.Show();
            }
            else if (mode == 'H')
                keypad.Hide();
            log.LogMethodExit();
        }
    }
}
