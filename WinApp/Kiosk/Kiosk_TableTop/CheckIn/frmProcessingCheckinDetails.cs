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
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Parafait_Kiosk
{
    public partial class frmProcessingCheckinDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = KioskStatic.Utilities;
        //private Semnox.Parafait.Transaction.Transaction currentTrx;
        private PurchaseProductDTO purchaseProductDTO; 
        private int selectedProductId;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesOfSelectedProduct;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmProcessingCheckinDetails(KioskTransaction kioskTransaction, int selectedProductId, PurchaseProductDTO purchaseProd, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesOfThisProduct)
        {
            log.LogMethodEntry("kioskTransaction", selectedProductId, purchaseProd, trxLinesOfThisProduct);
            this.kioskTransaction = kioskTransaction;
            this.selectedProductId = selectedProductId;
            this.purchaseProductDTO = purchaseProd;
            this.trxLinesOfSelectedProduct = trxLinesOfThisProduct;
            InitializeComponent();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            txtMessage.Text = "";
            KioskStatic.setDefaultFont(this);
            DisplaybtnCancel(false);
            DisplaybtnPrev(false);
            btnProceed.Visible = false;
            try
            {
                lblProcessingMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008); //Processing..Please wait...
                SetFont();
                SetCustomImages();
                SetCustomizedFontColors();
                SetKioskTimerTickValue(20);
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing frmProcessingCheckinDetails(): " + ex);
            }
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmAddingCheckinDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            try
            {
                if (purchaseProductDTO.ProductQtyMappingDTOs != null && purchaseProductDTO.ProductQtyMappingDTOs.Any())
                {
                    foreach (ProductQtyMappingDTO qtyMappingDTO in purchaseProductDTO.ProductQtyMappingDTOs)
                    {
                        if (qtyMappingDTO.ProductsContainerDTO.ProductType != ProductTypeValues.CHECKIN)
                            continue;

                        List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines = trxLinesOfSelectedProduct.FindAll(t => t.ProductID == qtyMappingDTO.ProductsContainerDTO.ProductId);
                        ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, selectedProductId);
                        if (kioskTransaction.GetActiveTransactionLines.Exists(t => t.ProductID == selectedProductId)
                            && productsContainerDTO.ProductType == ProductTypeValues.COMBO)
                        {
                            trxLines = trxLines.Where(t => t.LineCheckInDetailDTO == null).ToList();
                        }
                        else if(productsContainerDTO.ProductType == ProductTypeValues.CHECKIN)
                        {
                            trxLines = trxLines.Where(t => t.LineCheckInDetailDTO != null && t.LineCheckInDetailDTO.Name == null).ToList();
                        }
                        switch (qtyMappingDTO.ProductProfileType)
                        {
                            case ProductQtyMappingDTO.ProfileType.CHILD:
                                try
                                {
                                    using (frmSelectChild frmC = new frmSelectChild(kioskTransaction, trxLines, purchaseProductDTO))
                                    {
                                        DialogResult dr = frmC.ShowDialog();
                                        kioskTransaction = frmC.GetKioskTransaction;
                                        this.DialogResult = dr;
                                        if (dr == DialogResult.Cancel)
                                        {
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (dr == DialogResult.No)
                                        {
                                            RemoveProductFromCart();
                                            this.DialogResult = DialogResult.No;
                                            frmC.Close();
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }
                                }
                                catch (frmSelectChild.ChildRelationsNotExistException ex)
                                {
                                    log.Error(ex);
                                    using (frmEnterChildDetails frmec = new frmEnterChildDetails(kioskTransaction, trxLines, purchaseProductDTO, null))
                                    {
                                        DialogResult dr = frmec.ShowDialog();
                                        kioskTransaction = frmec.GetKioskTransaction;
                                        this.DialogResult = dr;
                                        if (dr == DialogResult.Cancel)
                                        {
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (dr == DialogResult.No)
                                        {
                                            RemoveProductFromCart();
                                            this.DialogResult = DialogResult.No;
                                            frmec.Close();
                                            log.LogMethodExit();
                                            return;
                                        }

                                    }
                                }

                                break;

                            case ProductQtyMappingDTO.ProfileType.ADULT:
                            case ProductQtyMappingDTO.ProfileType.NOT_DEFINED:
                                try
                                {
                                    using (frmSelectAdult frmA = new frmSelectAdult(kioskTransaction, trxLines, purchaseProductDTO))
                                    {
                                        DialogResult dr = frmA.ShowDialog();
                                        kioskTransaction = frmA.GetKioskTransaction;
                                        this.DialogResult = dr;
                                        if (dr == DialogResult.Cancel)
                                        {
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (dr == DialogResult.No)
                                        {
                                            RemoveProductFromCart();
                                            this.DialogResult = DialogResult.No;
                                            frmA.Close();
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }
                                }
                                catch (frmSelectAdult.RelationsNotExistException ex)
                                {
                                    log.Error(ex);
                                    using (frmEnterAdultDetails frmec = new frmEnterAdultDetails(kioskTransaction, trxLines, purchaseProductDTO, null))
                                    {
                                        DialogResult dr = frmec.ShowDialog();
                                        kioskTransaction = frmec.GetKioskTransaction;
                                        this.DialogResult = dr;
                                        if (dr == DialogResult.Cancel)
                                        {
                                            log.LogMethodExit();
                                            return;
                                        }
                                        else if (dr == DialogResult.No)
                                        {
                                            RemoveProductFromCart();
                                            this.DialogResult = DialogResult.No;
                                            frmec.Close();
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }
                                }

                                break;

                            default:
                                DialogResult = DialogResult.OK;
                                break;
                        }
                    }
                }
                else
                {
                    Close();
                }

                if (DialogResult == DialogResult.OK)
                {
                    bool showCheckInTAndC = ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "SHOW_CHECK-IN_TERMS_AND_CONDITIONS", false);
                    if (showCheckInTAndC)
                    {
                        using (frmTermsAndConditions frmRnC = new frmTermsAndConditions(KioskStatic.ApplicationContentModule.CHECKIN))
                        {
                            DialogResult dr = frmRnC.ShowDialog();
                            if (dr == System.Windows.Forms.DialogResult.Yes)
                            {
                                ShowCheckInSummary();
                            }
                            else
                            {
                                RemoveProductFromCart();
                            }
                        }
                    }
                    else
                    {
                        ShowCheckInSummary();
                    }
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing btnProceed_Click() of frmProcessingCheckinDetails: " + ex.Message);
            }
            finally
            {
                ResetKioskTimer();
            }
            log.LogMethodExit();
        }

        private void ShowCheckInSummary()
        {
            log.LogMethodEntry();
            using (frmCheckInSummary frm = new frmCheckInSummary(kioskTransaction, selectedProductId, purchaseProductDTO, trxLinesOfSelectedProduct))
            {
                DialogResult drSummary = frm.ShowDialog();
                kioskTransaction = frm.GetKioskTransaction;
                if (drSummary == DialogResult.No)
                {
                    RemoveProductFromCart();
                }
                else
                {
                    kioskTransaction = frm.GetKioskTransaction;
                    this.DialogResult = drSummary;
                    this.Close();
                }
            }
            log.LogMethodExit();
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
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                lblProcessingMsg.Font = new System.Drawing.Font(lblProcessingMsg.Font.FontFamily, 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized background images for frmProcessingCheckinDetails";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for frmProcessingCheckinDetails");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.ProcessingCheckinDetailsBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while Setting Customized background images for frmProcessingCheckinDetails";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
           //KioskStatic.logToFile("Setting customized font colors for the UI elements for frmProcessingCheckinDetails");
            try
            {
                this.lblProcessingMsg.ForeColor = KioskStatic.CurrentTheme.ChildSummaryGreetingLblTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ChildSummaryFooterTxtMsgTextForeColor;//Footer text message
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.ChildSummaryProceedButtonTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ChildSummaryBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChildSummaryHomeButtonTextForeColor;//Footer text message
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error while setting customized font colors for the UI elements for frmProcessingCheckinDetails: ";
                log.Error(msg, ex);
                KioskStatic.logToFile("ERROR: " + msg + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmProcessingCheckinDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmProcessingCheckinDetails_FormClosed()", ex);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }


        private void RemoveProductFromCart()
        {
            log.LogMethodEntry();
            try
            {
                if (kioskTransaction.ShowCartInKiosk == true)
                {
                    ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, selectedProductId);
                    if (kioskTransaction.ContainesLineForTheProduct(productsContainerDTO.ProductId))
                    {
                        foreach (ProductQtyMappingDTO qtyMappingDTO in purchaseProductDTO.ProductQtyMappingDTOs)
                        {
                            kioskTransaction.RemoveProduct(qtyMappingDTO.ProductsContainerDTO.ProductId, qtyMappingDTO.Quantity);
                        }
                        if (productsContainerDTO.ProductType == ProductTypeValues.COMBO)
                        {
                            kioskTransaction.RemoveProduct(productsContainerDTO.ProductId, 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing RemoveProductFromCart()", ex);
            }

            log.LogMethodExit();
        }
    }
}


























