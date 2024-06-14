/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Handles Playground Entry menu 
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.150.0.0    20-Sep-2021      Sathyavathi        Created for Check-In feature Phase-2
*2.150.1      22-Feb-2023      Sathyavathi        Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using System.Globalization;
using System.Threading;
using Semnox.Parafait.Product;
using System.Linq;

namespace Parafait_Kiosk
{
    public partial class frmAdultSummary : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities = KioskStatic.Utilities;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines;
        private PurchaseProductDTO purchaseProductDTO;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmAdultSummary(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines, purchaseProd);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.trxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd;

            try
            {
                KioskStatic.setDefaultFont(this);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                txtMessage.Text = "";
                SetKioskTimerTickValue(20);
                ResetKioskTimer();
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while frmAdultSummary(): " + ex.Message);
            }

            log.LogMethodExit();
        }

        private void frmAdultSummary_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                lblAdultSummaryMsg.Text = GetMessageToDisplay();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmChildSummary_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private string GetMessageToDisplay()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            try
            {
                int index = purchaseProductDTO.ProductQtyMappingDTOs.IndexOf(purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductId == trxLines[0].ProductID).FirstOrDefault());
                string productName = KioskHelper.GetProductName(purchaseProductDTO.ProductQtyMappingDTOs[index].ProductsContainerDTO.ProductId);
                message = MessageContainerList.GetMessage(utilities.ExecutionContext, 4382, productName)
                        + Environment.NewLine
                        + Environment.NewLine;

                List<ProductQtyMappingDTO> productsInQueue = purchaseProductDTO.ProductQtyMappingDTOs.GetRange(index + 1, purchaseProductDTO.ProductQtyMappingDTOs.Count - (index + 1));
                ProductQtyMappingDTO prodQtyMappingDTO = productsInQueue.Where(p => p.ProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN).FirstOrDefault();
                string productNameValue = KioskHelper.GetProductName(prodQtyMappingDTO.ProductsContainerDTO.ProductId);
                if (productsInQueue != null && productsInQueue.Any() && prodQtyMappingDTO != null)
                {
                    message += MessageContainerList.GetMessage(utilities.ExecutionContext, 4383, productNameValue);
                }
                else
                {
                    bool isShowCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_CART_IN_KIOSK", false);
                    //4876 - Please click on proceed to add the product to the Cart
                    //4293 - Please click on proceed to complete transaction 
                    message += MessageContainerList.GetMessage(utilities.ExecutionContext, (isShowCartInKiosk == true) ? 4876 : 4293);
                }
            }
            catch (Exception ex)
            {
                string errMsg = "ERROR: Failed to get stringToAppend in Child Summary Screen";
                log.Error(errMsg + ex);
                KioskStatic.logToFile(errMsg + ex.Message);
            }
            log.LogMethodExit(message);
            return message;
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                Application.DoEvents();
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                lblAdultSummaryMsg.Font = new System.Drawing.Font(lblAdultSummaryMsg.Font.FontFamily, 50F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            catch (Exception ex)
            {
                string msg = "Unexpected Error in SetFont() of Adult Summary Screen: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.AdultSummaryBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected Error while Setting Customized background images for Adult Summary Screen: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements of frmAdultSummary");
            try
            {
                this.lblAdultSummaryMsg.ForeColor = KioskStatic.CurrentTheme.AdultSummaryGreetingLblTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.AdultSummaryFooterTxtMsgTextForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.AdultSummaryProceedButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.AdultSummaryBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.AdultSummaryHomeButtonTextForeColor;//Footer text message
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while setting customized font colors for the UI elements of frmAdultSummary: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex);
            }
            log.LogMethodExit();
        }

        private void frmAdultSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error occurred while executing frmChildSummaryDetails_FormClosed()", ex);
            }
            //Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}


























