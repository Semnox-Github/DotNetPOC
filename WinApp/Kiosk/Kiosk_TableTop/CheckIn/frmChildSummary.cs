/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - Handles Playground Entry menu
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmChildSummary : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> childTrxLines;
        private PurchaseProductDTO purchaseProductDTO;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmChildSummary(KioskTransaction kioskTransaction, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> inTrxLines, PurchaseProductDTO purchaseProd)
        {
            log.LogMethodEntry("kioskTransaction", inTrxLines,purchaseProd);
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;
            this.childTrxLines = inTrxLines;
            this.purchaseProductDTO = purchaseProd;

            KioskStatic.setDefaultFont(this);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = "";

            try
            {
                DisplaybtnCancel(false);
                DisplaybtnPrev(true);
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                SetKioskTimerTickValue(20);
                ResetKioskTimer();
                utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmChildSummary(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmChildSummary_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                lblChildSummaryMsg.Text = GetMessageToDisplay();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmChildSummary_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private string GetMessageToDisplay()
        {
            log.LogMethodEntry();
            string message = string.Empty;
            try
            {
                int index = purchaseProductDTO.ProductQtyMappingDTOs.IndexOf(purchaseProductDTO.ProductQtyMappingDTOs.Where(p => p.ProductsContainerDTO.ProductId == childTrxLines[0].ProductID).FirstOrDefault());
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
                lblChildSummaryMsg.Font = new System.Drawing.Font(lblChildSummaryMsg.Font.FontFamily, 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error while Setting Customized background images for frmChildSummary", ex);
                KioskStatic.logToFile("Unexpected error while setting customized background images for frmChildSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for frmChildSummary");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.ChildSummaryBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error while Setting Customized background images for frmChildSummary", ex);
                KioskStatic.logToFile("Unexpected error while setting customized background images for frmChildSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements for frmChildSummary");
            try
            {
                this.lblChildSummaryMsg.ForeColor = KioskStatic.CurrentTheme.ChildSummaryGreetingLblTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ChildSummaryFooterTxtMsgTextForeColor;//Footer text message
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.ChildSummaryProceedButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ChildSummaryBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChildSummaryHomeButtonTextForeColor;//Footer text message
            }
            catch (Exception ex)
            {
                log.Error("Unexpected errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Unexpected error while setting customized font colors for the UI elements for frmChildSummary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmChildSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmChildSummaryDetails_FormClosed()", ex);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}


























