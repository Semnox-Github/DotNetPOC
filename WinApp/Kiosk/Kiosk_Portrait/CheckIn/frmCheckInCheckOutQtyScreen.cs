/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  -CheckIn CheckOut Qty Screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.150.0.0   22-Feb-2022      Sathyavathi        Created : Check-In Check-Out Phase-2
 *2.150.0.0   02-Dec-2022      Sathyavathi        Check-In feature Phase-2 Additional features
 *2.150.1     22-Feb-2023      Sathyavathi        Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using System.Drawing;
using Semnox.Parafait.Customer.Accounts;

namespace Parafait_Kiosk
{
    public partial class frmCheckInCheckOutQtyScreen : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ProductsContainerDTO> childProductsContainerDTOList;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private Card parentCard;
        private usrCtrlCheckinCheckoutProductsQty selectedUsrCtrlProduct;
        private ProductsContainerDTO comboProductsContainerDTO;
        private const int MAX_QUANTITY = 999;
        private const string ERROR = "ERROR";
        private int selectedProductId;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmCheckInCheckOutQtyScreen(KioskTransaction kioskTransaction, int productId, string cardNum)
        {
            log.LogMethodEntry("kioskTransaction", productId, cardNum);
            KioskStatic.logToFile("In frmCheckInCheckOutQtyScreen()");
            this.kioskTransaction = kioskTransaction;
            this.selectedProductId = productId;
            try
            {
                this.comboProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, productId);
                if (comboProductsContainerDTO == null)
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4810); //ERROR: Failed to get product details
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    return;
                }
                parentCard = null;
                if (!string.IsNullOrEmpty(cardNum))
                {
                    parentCard = new Card(cardNum, "Kiosk", KioskStatic.Utilities);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error getting parent card in frmCheckInCheckOutQtyScreen" + ex.Message);
            }
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            string productName = KioskHelper.GetProductName(comboProductsContainerDTO.ProductId);
            this.lblGreeting.Text = txtMessage.Text = MessageContainerList.GetMessage(executionContext, 4283, productName);

            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            SetKioskTimerTickValue(30);
            SetCustomImages();
            lblGreeting.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Errow in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }

        private void frmCheckInCheckOutQtyScreen_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                childProductsContainerDTOList = new List<ProductsContainerDTO>(comboProductsContainerDTO.ProductId);
                if (comboProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    if (comboProductsContainerDTO.ComboProductContainerDTOList != null && comboProductsContainerDTO.ComboProductContainerDTOList.Any())
                    {
                        foreach (ComboProductContainerDTO comboProductContainerDTO in comboProductsContainerDTO.ComboProductContainerDTOList)
                        {
                            ProductsContainerDTO childProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext, comboProductContainerDTO.ChildProductId);
                            childProductsContainerDTOList.Add(childProductsContainerDTO);
                        }
                    }
                }
                else
                {
                    childProductsContainerDTOList.Add(comboProductsContainerDTO);
                }

                InitializeProductTab(childProductsContainerDTOList);
                //SetCustomImages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmCheckInCheckOutQtyScreen_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            StopKioskTimer();
            try
            {
                //Processing..Please wait...
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1008);
                Dictionary<int, int> productIdQtyPairs = new Dictionary<int, int>();
                foreach (usrCtrlCheckinCheckoutProductsQty usrCtrl in tlpComboProducts.Controls)
                {
                    if (usrCtrl.QtySelected > 0)
                    {
                        productIdQtyPairs.Add(usrCtrl.ComboChildProductContainerDTO.ProductId, usrCtrl.QtySelected);
                    }
                }
                if (productIdQtyPairs.Count == 0)
                {
                    //Error: Please select the quantity for the product/s
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4381);
                    frmOKMsg.ShowUserMessage(msg);
                    log.LogMethodExit();
                    return;
                }

                //if (productIdQtyPairs.Values.Distinct().Count() == 1
                //    && productIdQtyPairs.ContainsKey(childProductsContainerDTOList[0].ProductId)
                //    && productIdQtyPairs[childProductsContainerDTOList[0].ProductId] == 0)
                //{
                //    //Product setup Error: configuration value for THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN must be lesser than  THRESHOLD_AGE_CHECK_IN_ADULT_SCREEN
                //    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                //        + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4819);
                //    log.Error(errMsg);
                //    KioskStatic.logToFile(errMsg);
                //    frmOKMsg.ShowUserMessage(errMsg);
                //    txtMessage.Text = errMsg;
                //    return;
                //}

                decimal thresholdAgeOfChild = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN", -1);
                decimal thresholdAgeOfAdult = ParafaitDefaultContainerList.GetParafaitDefault<decimal>(KioskStatic.Utilities.ExecutionContext, "THRESHOLD_AGE_CHECK_IN_ADULT_SCREEN", -1);

                //by default both the values are different. When both values are same, application can't decide whether to show child/adult. Assumption is thresholdAgeOfAdult is greater than thresholdAgeOfChild
                if (thresholdAgeOfChild >= thresholdAgeOfAdult)
                {
                    //Product setup Error: configuration value for THRESHOLD_AGE_CHECK_IN_CHILD_SCREEN must be lesser than  THRESHOLD_AGE_CHECK_IN_ADULT_SCREEN
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4466)
                        + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4819);
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    frmOKMsg.ShowUserMessage(msg);
                    log.LogMethodExit();
                    txtMessage.Text = msg;
                    return;
                }

                bool isShowCartInKiosk = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_CART_IN_KIOSK", false);
                PurchaseProductDTO purchaseProductDTO = new PurchaseProductDTO(childProductsContainerDTOList, productIdQtyPairs, parentCard.CardNumber, parentCard.customerDTO.Id);
                if (isShowCartInKiosk == false && purchaseProductDTO.ProductQtyMappingDTOs != null && purchaseProductDTO.ProductQtyMappingDTOs.Any() &&
                    !purchaseProductDTO.ProductQtyMappingDTOs.Exists(p => p.ProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN))
                {
                    //case when min qty is not set and user skip selecting qty for checkin product
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4818); //Combo without quantity selection for check-in product is currently not supported
                    KioskStatic.logToFile(msg);
                    log.Error(msg);
                    frmOKMsg.ShowUserMessage(msg);
                    log.LogMethodExit();
                    txtMessage.Text = msg;
                    return;
                }

                bool applyCardCreditPlusConsumption = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "AUTO_APPLY_CARD_CREDITPLUS_CONSUMPTION", false);
                int availableCoupons = kioskTransaction.GetNumOfCardCreditPlusBalanceAvailable(childProductsContainerDTOList, purchaseProductDTO.ParentCardNumber);
                if (!applyCardCreditPlusConsumption && availableCoupons > 0
                    && purchaseProductDTO.ProductQtyMappingDTOs != null && purchaseProductDTO.ProductQtyMappingDTOs.Any())
                {
                    int screenTimeout = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(KioskStatic.Utilities.ExecutionContext, "BALANCE_SCREEN_TIMEOUT", 30); //30 seconds
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4343, availableCoupons); //You have &1 voucher free of charge. Would you like to use for this purchase?
                    using (frmYesNo frmYesNo = new frmYesNo(msg, string.Empty, screenTimeout))
                    {
                        DialogResult dr = frmYesNo.ShowDialog();
                        if (dr == DialogResult.Yes)
                        {
                            applyCardCreditPlusConsumption = true;
                        }
                    }
                }
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesOfSelectedProduct;
                try
                {
                    trxLinesOfSelectedProduct = kioskTransaction.AddCheckinCheckOutProduct(comboProductsContainerDTO.ProductId, purchaseProductDTO, parentCard, applyCardCreditPlusConsumption);
                }
                catch (Exception ex)
                {   //"ERROR: Failed to create trx line";
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4292);
                    log.Error("AddCheckinCheckOutProduct: " + errMsg, ex);
                    txtMessage.Text = errMsg;
                    KioskStatic.logToFile("AddCheckinCheckOutProduct: " + errMsg + ex.Message);
                    frmOKMsg.ShowUserMessage(errMsg);
                    log.LogMethodExit();
                    return;
                }

                using (frmProcessingCheckinDetails frm = new frmProcessingCheckinDetails(kioskTransaction, selectedProductId, purchaseProductDTO, trxLinesOfSelectedProduct))
                {
                    DialogResult dr = frm.ShowDialog();
                    kioskTransaction = frm.GetKioskTransaction;
                    if (dr != System.Windows.Forms.DialogResult.No) //back button pressed
                    {
                        DialogResult = dr;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("btnProceed_Click() in CheckInCheckOutQtyScreen : " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void vScrollBarProducts_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (tlpComboProducts.Top + tlpComboProducts.Height > 3)
                {
                    tlpComboProducts.Top = tlpComboProducts.Top - value;
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            try
            {
                ResetKioskTimer();
                if (tlpComboProducts.Top < 0)
                {
                    tlpComboProducts.Top = Math.Min(0, tlpComboProducts.Top + value);
                }
            }
            catch { }
            log.LogMethodExit();
        }

        private void InitializeProductTab(List<ProductsContainerDTO> productsContainerList)
        {
            log.LogMethodEntry(productsContainerList);
            this.tlpComboProducts.Controls.Clear();
            try
            {
                foreach (ProductsContainerDTO childProductsContainerDTO in productsContainerList)
                {
                    ResetKioskTimer();
                    int minQty = GetMinQuantity(childProductsContainerDTO);
                    int maxQty = GetMaxQuantity(childProductsContainerDTO);
                    usrCtrlCheckinCheckoutProductsQty usrCtrlComboChildProductsQty = CreateUsrCtlElement(childProductsContainerDTO.ProductId, minQty, maxQty);
                    usrCtrlComboChildProductsQty.Width = tlpComboProducts.Width - 2;
                    this.tlpComboProducts.Controls.Add(usrCtrlComboChildProductsQty);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in initializeProductTab() of frmCheckInCheckOutQtyScreen: " + ex);
            }
            log.LogMethodExit();
        }

        private int GetMinQuantity(ProductsContainerDTO prodsContainerDTO)
        {
            log.LogMethodEntry(prodsContainerDTO);
            ResetKioskTimer();

            int minQuantity = -1;
            try
            {
                decimal Qty = 0;

                if (comboProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    Qty = comboProductsContainerDTO.ComboProductContainerDTOList.Where(x => x.ChildProductId == prodsContainerDTO.ProductId).FirstOrDefault().Quantity;
                    minQuantity = (Qty > 0) ? (Convert.ToInt32(Qty)) : ((prodsContainerDTO.MinimumQuantity > 0) ? prodsContainerDTO.MinimumQuantity : 0);
                }
                else
                {
                    //set min Qty as 1 by default when it is not set in product setup. 
                    minQuantity = (comboProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN && prodsContainerDTO.MinimumQuantity <= 0) ? 1 : prodsContainerDTO.MinimumQuantity;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error getting minimum quantity in Quantity screen: " + ex);
            }
            log.LogMethodExit(minQuantity);
            return minQuantity;
        }

        private int GetMaxQuantity(ProductsContainerDTO prodsContainerDTO)
        {
            log.LogMethodEntry(prodsContainerDTO);
            ResetKioskTimer();

            int? maxQuantity = null;
            try
            {
                if (comboProductsContainerDTO.ProductType == ProductTypeValues.COMBO)
                {
                    var Qty = comboProductsContainerDTO.ComboProductContainerDTOList.Where(x => x.ChildProductId == prodsContainerDTO.ProductId).FirstOrDefault().MaximumQuantity;
                    maxQuantity = (Qty > 0) ? Convert.ToInt32(Qty) : prodsContainerDTO.MaximumQuantity;
                }
                else
                {
                    if (prodsContainerDTO.MaximumQuantity > 0)
                    {
                        maxQuantity = prodsContainerDTO.MaximumQuantity;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error getting maximum quantity in Quantity screen: " + ex);
            }

            log.LogMethodExit(maxQuantity);
            return (maxQuantity == null ? MAX_QUANTITY : (int)maxQuantity);
        }

        private void SelectedQuantity(usrCtrlCheckinCheckoutProductsQty usrCtrlCombo)
        {
            log.LogMethodEntry(usrCtrlCombo);
            ResetKioskTimer();
            try
            {
                selectedUsrCtrlProduct = usrCtrlCombo;
                txtMessage.Text = selectedUsrCtrlProduct.ErrMsg;

                if (!string.IsNullOrEmpty(selectedUsrCtrlProduct.ErrMsg))
                {
                    frmOKMsg.ShowUserMessage(selectedUsrCtrlProduct.ErrMsg);
                }
                if (selectedUsrCtrlProduct.ShowKeypad == true)
                {
                    double newQty = Semnox.Core.Utilities.KeyPads.Kiosk.NumberPadForm.ShowNumberPadForm("Enter Quantity", usrCtrlCombo.QtySelected.ToString(), KioskStatic.Utilities);

                    if (newQty < 0)
                        return;

                    //if min qty is set and user has selected qty lesser than that
                    if (((usrCtrlCombo.MinQuantity > 0) && (newQty < usrCtrlCombo.MinQuantity))
                        || ((usrCtrlCombo.MaxQuantity > 0) && (newQty > usrCtrlCombo.MaxQuantity)))
                    {
                        selectedUsrCtrlProduct.QtySelected = usrCtrlCombo.QtySelected; //when user enters incorrect qty, it should not be taken into considerations.
                        string errMsg = MessageContainerList.GetMessage(executionContext, 4136, usrCtrlCombo.MinQuantity, usrCtrlCombo.MaxQuantity); //'Quantity entered must be between &1 and &2'
                        txtMessage.Text = errMsg;
                        frmOKMsg.ShowUserMessage(errMsg);
                    }
                    else
                    {
                        selectedUsrCtrlProduct.QtySelected = Convert.ToInt32(newQty);
                        txtMessage.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error in SelctedQuantity() of frmCheckInCheckOutQtyScreen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private usrCtrlCheckinCheckoutProductsQty CreateUsrCtlElement(int productId, int minQty, int maxQty)
        {
            log.LogMethodEntry(productId, minQty, maxQty);
            ResetKioskTimer();

            usrCtrlCheckinCheckoutProductsQty usrCtrlChildProductsQty = null;
            try
            {
                usrCtrlChildProductsQty = new usrCtrlCheckinCheckoutProductsQty(productId, minQty, maxQty);
                usrCtrlChildProductsQty.selctedQuantity += new usrCtrlCheckinCheckoutProductsQty.SelctedQuantity(SelectedQuantity);
            }
            catch (Exception ex)
            {
                string msg = "Error in CreateUsrCtlElement() of quantity screen";
                log.Error(msg);
                KioskStatic.logToFile(msg + " : " + ex.Message);
            }
            log.LogMethodExit(usrCtrlChildProductsQty);
            return usrCtrlChildProductsQty;
        }

        private void HighlightTxtMessage(string err)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                txtMessage.BackColor = err.Equals(ERROR) ? KioskStatic.CurrentTheme.FooterMsgErrorHighlightBackColor : Color.Transparent;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting customized background images for Plaayground Quantity Screen");
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CheckInCheckOutQtyScreenBackgroundImage); //background image
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnPrev.BackgroundImage = btnCancel.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized background images for frmCheckInCheckOutQtyScreen", ex);
                KioskStatic.logToFile("Error while setting customized background images for frmCheckInCheckOutQtyScreen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                tlpComboProducts.SuspendLayout();
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyHomeButtonTextForeColor;//home button
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyGreetingLblTextForeColor;//Greeting message
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyBackButtonTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyCancelButtonTextForeColor;//Back button
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyProceedButtonTextForeColor;//Proceed button
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.ComboChildProductsQtyFooterTxtMsgTextForeColor; //footer message 
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for frmCheckInCheckOutQtyScreen", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of frmCheckInCheckOutQtyScreen: " + ex.Message);
            }
            tlpComboProducts.ResumeLayout(true);
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
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmCheckInCheckOutQtyScreen_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmCheckInCheckOutQtyScreen_FormClosed()", ex);
            }

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

    }
}
