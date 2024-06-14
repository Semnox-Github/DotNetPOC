/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmScsnCoupon.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80.2     02-May-2021      Dakshakh             Theme changes to support customized Font ForeColor
 *2.130.0    30-Jun-2021      Dakshak              Theme changes to support customized Font ForeColor
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmScanCoupon : BaseForm
    {
        //private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string codeScaned;
        public string CodeScaned { get { return codeScaned; } }
        DeviceClass barcodeScannerDevice = null;
        clsKioskTransaction kioskTransaction;
        string applicability;
        string headerText;
        string lableText;
        string okButtonText;
        string cancelButtonText;
        delegate void DisplayCoupon(string couponNumber);

        public frmScanCoupon(clsKioskTransaction kioskTransaction)
        {
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
        }
        public frmScanCoupon(string applicability, string headerText, string lableText, string okButtonText, string cancelButtonText)
        {
            InitializeComponent();
            this.applicability = applicability;
            this.headerText = headerText;
            this.lableText = lableText;
            this.okButtonText = okButtonText;
            this.cancelButtonText = cancelButtonText;
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
        }

        private void frmScanCoupon_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (!string.IsNullOrEmpty(applicability))
            {
                if (!string.IsNullOrEmpty(headerText))
                {
                    lblHeader.Text = KioskStatic.Utilities.MessageUtils.getMessage(headerText);
                }
                if (!string.IsNullOrEmpty(lableText))
                {
                    lblCoupon.Text = KioskStatic.Utilities.MessageUtils.getMessage(lableText)+": ";
                }
                if (!string.IsNullOrEmpty(okButtonText))
                {
                    btnApply.Text = KioskStatic.Utilities.MessageUtils.getMessage(okButtonText) ;
                }
                if (!string.IsNullOrEmpty(cancelButtonText))
                {
                    btnClose.Text = KioskStatic.Utilities.MessageUtils.getMessage(cancelButtonText);
                }
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
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                { frm.ShowDialog(); }
                this.Close();
            }
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                string scannedBarcode = KioskStatic.Utilities.ProcessScannedBarCode(checkScannedEvent.Message, KioskStatic.Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, KioskStatic.Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                DisplayScanedText(scannedBarcode);
                //this.Invoke((MethodInvoker)delegate
                //{
                //    txtCouponNo.Text = scannedBarcode;
                //    log.Info("Coupon Number is :" + txtCouponNo.Text);
                //});
            }
            log.LogMethodEntry();
        }
        private void DisplayScanedText(string text)
        {
            log.Debug("starts-DisplayScanedText() method.");

            if (this.txtCouponNo.InvokeRequired)
            {
                DisplayCoupon delegateFunction = new DisplayCoupon(DisplayScanedText);
                this.BeginInvoke(delegateFunction, new object[] { text });
            }
            else
            {
                txtCouponNo.Text = text;
                log.Info("Coupon Number is :" + text);
            }

            log.Debug("Ends-DisplayScanedText() method.");
        }
        //private bool RegisterBarcodeScanner()
        //{
        //    log.LogMethodEntry();
        //    log.Debug("Starts-registerBarcodeScanner()");
        //    string USBReaderVID = KioskStatic.Utilities.getParafaitDefaults("USB_BARCODE_READER_VID");
        //    string USBReaderPID = KioskStatic.Utilities.getParafaitDefaults("USB_BARCODE_READER_PID");
        //    string USBReaderOptionalString = KioskStatic.Utilities.getParafaitDefaults("USB_BARCODE_READER_OPT_STRING");

        //    EventHandler currEventHandler = new EventHandler(BarCodeScanCompleteEventHandle);

        //    USBDevice barcodeListener;
        //    if (IntPtr.Size == 4) //32 bit
        //        barcodeListener = new KeyboardWedge32();
        //    else
        //        barcodeListener = new KeyboardWedge64();

        //    foreach (string optString in USBReaderOptionalString.Split('|'))
        //    {
        //        if (string.IsNullOrEmpty(optString.Trim()))
        //            continue;

        //        bool flag = barcodeListener.InitializeUSBReader(Application.OpenForms["frmHome"], USBReaderVID, USBReaderPID, optString.Trim());
        //        if (barcodeListener.isOpen)
        //        {
        //            barcodeListener.Register(currEventHandler);
        //            barcodeScannerDevice = barcodeListener;
        //            log.Debug("Ends-registerBarcodeScanner()");
        //            return true;
        //        }
        //    }
        //    string mes = "Unable to find USB card reader for Top-up. VID: " + USBReaderVID + ", PID: " + USBReaderPID + ", OPT: " + USBReaderOptionalString + " POS: " + KioskStatic.Utilities.ParafaitEnv.POSMachine;
        //    frmOKMsg frm = new frmOKMsg(mes);
        //    frm.ShowDialog();
        //    log.Info("Ends-registerBarcodeScanner() as Unable to find USB Bar Code scanner");
        //    log.LogMethodExit();
        //    return false;
        //}

        void applyDiscount()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(KioskStatic.Utilities.ExecutionContext, txtCouponNo.Text);
            if (discountCouponsBL.CouponStatus == CouponStatus.ACTIVE)
            {
                if (!kioskTransaction.ApplyCoupon(txtCouponNo.Text))
                {
                    message = KioskStatic.Utilities.MessageUtils.getMessage("Discount is not applied..");
                    frmOKMsg frm = new frmOKMsg(message);
                    frm.ShowDialog();
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
                
                frmOKMsg frm = new frmOKMsg(message);
                frm.ShowDialog();
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            log.LogMethodExit(null);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (!string.IsNullOrEmpty(applicability))
            {
                if(applicability.Equals("WeChat"))
                {
                    if(string.IsNullOrEmpty(txtCouponNo.Text))
                    {
                        frmOKMsg frm = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1551));
                        frm.ShowDialog();
                        return;
                    }
                }

            }
            else
            {
                applyDiscount();
            }
            codeScaned = txtCouponNo.Text;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void frmScanCoupon_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (barcodeScannerDevice != null)
            {
                barcodeScannerDevice.UnRegister();
                barcodeScannerDevice.Dispose();
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblHeader.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenHeaderTextForeColor;
                this.lblCoupon.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenCouponHeaderTextForeColor;
                this.txtCouponNo.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenCouponInfoTextForeColor;
                this.btnApply.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenBtnApplyTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.ScanCouponScreenBtnCloseTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
