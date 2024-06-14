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
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Customer.Accounts;
using System.Drawing;

namespace Parafait_Kiosk
{
    public partial class frmCheckInSummary : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private const string MESSAGE = "MESSAGE";
        //private string Function = KioskTransaction.GETCHECKINCHECKOUTTYPE;
        private decimal AmountRequired;
        private string couponNumber = "";
        private Card parentCard;

        private Utilities Utilities = KioskStatic.Utilities;
        private const string selectedEntitlementType = "CheckInCheckOut";
        private DiscountsSummaryDTO discountSummaryDTO = null;
        private CustomerDTO parentCustomerDTO;
        private DataRow selectedProductRow;
        //ProductsContainerDTO selectedProductsContainerDTO;
        private ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        //private clsKioskTransaction kioskTrx;
        private Semnox.Parafait.Transaction.Transaction.TransactionLine parentLine;
        private PurchaseProductDTO purchaseProductDTO;
        private string amountFormat;
        private string amountFormatWithCurrencySymbol;
        private string numberFormat;
        private int selectedProductId;
        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesOfSelectedProduct;

        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmCheckInSummary(KioskTransaction kioskTransaction, int selectedProductId, PurchaseProductDTO purchaseProd, List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesOfThisProduct)
        {
            log.LogMethodEntry("kioskTransaction", selectedProductId, purchaseProd, trxLinesOfThisProduct);
            InitializeComponent();
            this.purchaseProductDTO = purchaseProd;
            this.kioskTransaction = kioskTransaction;
            this.selectedProductId = selectedProductId;
            this.trxLinesOfSelectedProduct = trxLinesOfThisProduct;
            try
            {
                KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
                KioskStatic.setDefaultFont(this);
                panelDiscount.Visible = false;
                panelDiscountLine.Visible = false;
                panelDeposit.Visible = false;
                panelDepositLine.Visible = false;
                lblDiscount.Visible = false;
                panelPassportCoupon.Visible = false;

                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                SetStyle(ControlStyles.DoubleBuffer, true);
                //this.ShowInTaskbar = true;
                displayMessageLine("");
                //lblSiteName.Visible = false;
                //lblSiteName.Text = KioskStatic.SiteHeading;
                DisplaybtnCancel(true);
                DisplaybtnPrev(true);
                lblGreeting.Text = MessageContainerList.GetMessage(executionContext, 4399);//Purchase Summary
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_DISCOUNTS_IN_POS").Equals("N") || kioskTransaction.ShowCartInKiosk == true)
                {
                    btnApplyCoupon.Text = MessageContainerList.GetMessage(executionContext, btnApplyCoupon.Text);
                    btnApplyCoupon.Visible = false;
                }
                SetCustomImages();
                SetFont();
                SetCustomizedFontColors();
                Utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected Error in frmCheckInSummary(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmCheckInSummary_Load(System.Object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                amountFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
                amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
                numberFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");

                InitializeFrmCheckInSummary();

                Application.DoEvents();
                lblPassportCoupon.Text = MessageContainerList.GetMessage(executionContext, 4820); //Passport Coupon applied for below product/s
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error in frmCheckInSummary_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the screen flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            try
            {
                if (btnApplyCoupon.Tag.ToString() == "applyDiscount")
                {
                    couponNumber = string.Empty;
                    //currentTrx.EntitlementReferenceDate = KioskStatic.Utilities.getServerTime();
                    //kioskTrx = new clsKioskTransaction(this, Function, selectedProductRow, parentCard, parentCustomerDTO, null, -1, selectedEntitlementType, "", discountSummaryDTO, couponNumber, currentTrx, null);
                    using (frmScanCoupon frm = new frmScanCoupon(kioskTransaction))
                    {
                        DialogResult dr = frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                        if (dr != System.Windows.Forms.DialogResult.Cancel)
                        {
                            couponNumber = frm.CodeScaned;
                            List<DiscountsSummaryDTO> appliedDisSummary = kioskTransaction.GetTransactionDiscountsSummaryList;
                            if (appliedDisSummary != null && appliedDisSummary.Any())
                            {
                                DiscountsSummaryDTO discountCouponDTO = kioskTransaction.GetAppliedDiscountSummaryForCoupon(couponNumber);
                                lblDiscount.Visible = true;
                                lblDiscount.Text = discountCouponDTO.DiscountAmount.ToString(amountFormatWithCurrencySymbol) +
                                        Environment.NewLine + "(" + discountCouponDTO.DiscountName.ToString() + ")";
                                decimal trxNetAmount = kioskTransaction.GetTrxNetAmount();
                                lblTotalToPayValue.Text = trxNetAmount.ToString(amountFormat);
                                btnApplyCoupon.Text = MessageContainerList.GetMessage(executionContext, "Remove Coupon");
                                btnApplyCoupon.Tag = "cancelDiscount";
                            }
                        }
                        frm.Dispose();
                    }
                }
                else if (btnApplyCoupon.Tag.ToString() == "cancelDiscount")
                {
                    decimal trxNetAmount = kioskTransaction.GetTrxNetAmount();
                    lblTotalToPayValue.Text = trxNetAmount.ToString(amountFormat);
                    try
                    {
                        kioskTransaction.RemoveDiscountCoupon(couponNumber);
                        couponNumber = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing cancelDiscount" + ex.Message);
                        KioskStatic.logToFile(ex.Message);
                    }
                    lblDiscount.Visible = false;
                    trxNetAmount = kioskTransaction.GetTrxNetAmount();
                    lblTotalToPayValue.Text = (trxNetAmount == 0 ? trxNetAmount.ToString("N2") : trxNetAmount.ToString(amountFormat));
                    btnApplyCoupon.Text = MessageContainerList.GetMessage(executionContext, "Apply Coupon");
                    btnApplyCoupon.Tag = "applyDiscount";
                }
                //currentTrx.EntitlementReferenceDate = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnDiscount_Click()" + ex);
                KioskStatic.logToFile("Error occurred while executing btnDiscount_Click(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            try
            {
                string screen = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "home");
                string msg = (kioskTransaction.ShowCartInKiosk == false) ? 
                            MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4287, screen) //"This will abondon the transaction and take you back to the quantity screen"
                            : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4850, screen) //"This action will remove the product from the Cart and take you back to the quantity screen"
                            + "." + Environment.NewLine
                            + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4291);//"Are you sure you want to go back?"

                using (frmYesNo yesNo = new frmYesNo(msg))
                {
                    DialogResult dr = yesNo.ShowDialog();
                    if (dr == DialogResult.Yes)
                    {
                        DialogResult = DialogResult.No;
                        Close();
                    }
                    else if (dr == DialogResult.No)
                    {
                        this.DialogResult = DialogResult.None;
                        yesNo.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnCancel_Click() of checkin summary screen: " + ex.Message);
            }
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            try
            {
                frmChooseProduct.AlertUser(selectedProductId, kioskTransaction, ProceedActionImpl);
                if (kioskTransaction.ShowCartInKiosk == false)
                {
                    if (kioskTransaction != null)
                    {
                        ProceedActionImpl(kioskTransaction);
                    }
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                    KioskStatic.logToFile("frmCheckInSummary: " + msg); 
                    displayMessageLine(msg);
                    Close();
                }
            }
            catch (Exception ex)
            {
                string msg = "Unexpected error occurred while executing btnProceed_Click() in check-in summary : ";
                log.Error(msg + ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void ProceedActionImpl(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            try
            {
                using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                {
                    DialogResult dr = frpm.ShowDialog();
                    kioskTransaction = frpm.GetKioskTransaction;
                    if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
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
                this.Show();
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                txtMessage.Text = ex.Message;
                this.Close();
            }
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
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        }

        private void InitializeFrmCheckInSummary()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                parentCard = new Card(purchaseProductDTO.ParentCardNumber, "", KioskStatic.Utilities);
                if (parentCard == null && kioskTransaction.HasCheckinProducts() == true)
                {
                    string msg = "ParentCard can't be null, exiting...";
                    log.LogVariableState("ParentCard: ", parentCard);
                    KioskStatic.logToFile(msg);
                    return;
                }
                parentCustomerDTO = parentCard.customerDTO;
                if (parentCustomerDTO == null && kioskTransaction.HasCheckinProducts() == true)
                {
                    string msg = "parentCustomerDTO can't be null, exiting...";
                    log.LogVariableState("Parent CustomerDTO: ", parentCustomerDTO);
                    KioskStatic.logToFile(msg);
                    return;
                }
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines = kioskTransaction.GetActiveTransactionLines;
                DataTable dt = KioskStatic.getProductDetails(selectedProductId);
                selectedProductRow = dt.Rows[0];
                //selectedProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, selectedProductId);
                if (dt.Rows.Count < 0)
                {
                    string msg = "Could not retrieve product Row in checkin summary screen";
                    log.Error(msg);
                    KioskStatic.logToFile(msg);
                    return;
                }
                parentLine = allTrxLines.Find(tl => tl.LineValid && tl.ProductID == selectedProductId);
                if (kioskTransaction.ShowCartInKiosk == true)
                {
                    btnProceed.Text = MessageContainerList.GetMessage(executionContext, "Proceed");
                    lblGreeting.Text = MessageContainerList.GetMessage(executionContext, 4861, selectedProductRow["product_name"].ToString()); //Check-In Summary for &1
                }
                AmountRequired = (decimal)kioskTransaction.GetTrxNetAmount();

                UpdateSummaryPanelForCombo();
                UpdateDiscountPanel();
                UpdatePassportCouponPanel();

                lblTotalToPayValue.Text = AmountRequired.ToString(amountFormatWithCurrencySymbol);
                DisplayValidtyMsg(Convert.ToInt32(selectedProductRow["product_id"]));
                SetKioskTimerTickValue(30);
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing InitializeFrmCheckInSummary(): " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdatePassportCouponPanel()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines = trxLinesOfSelectedProduct;
                ProductsContainerDTO mainProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, selectedProductId);
                if (allTrxLines != null && allTrxLines.Exists(t => t.CreditPlusConsumptionId > -1))
                {
                    panelPassportCoupon.Visible = true;
                    this.lblStar.ForeColor = lblPackageTotal.ForeColor;
                }
                else
                {
                    flpPassportCoupon.Visible = false;
                }

                flpPassportCoupon.SuspendLayout();
                if (panelPassportCoupon.Visible == true)
                {
                    foreach (ProductQtyMappingDTO product in purchaseProductDTO.ProductQtyMappingDTOs)
                    {
                        Semnox.Parafait.Transaction.Transaction.TransactionLine tl = allTrxLines.Where(t => t.ProductID == product.ProductsContainerDTO.ProductId).FirstOrDefault();
                        int qty = allTrxLines.Count(t => t.CreditPlusConsumptionId > -1 && t.ProductID == product.ProductsContainerDTO.ProductId);
                        if (qty > 0 && tl != null)
                        {
                            ProductsContainerDTO pCAppliedproductsContainerDTO = product.ProductsContainerDTO;

                            UsrCtrlCheckInSummary usrCtrlCheckInSummary = CreateUsrCtlElement(executionContext, pCAppliedproductsContainerDTO);
                            usrCtrlCheckInSummary.Width = panelPassportCoupon.Width;
                            string productName = KioskHelper.GetProductName(pCAppliedproductsContainerDTO.ProductId);
                            usrCtrlCheckInSummary.LblPackage = productName;
                            int couponDiscPercentage = GetCardCreditPlusConsumptionDiscountPercentage(tl.CreditPlusConsumptionId);
                            usrCtrlCheckInSummary.LblDiscountPerc = couponDiscPercentage.ToString();
                            usrCtrlCheckInSummary.LblQuantity = qty.ToString();
                            usrCtrlCheckInSummary.LblPrice = pCAppliedproductsContainerDTO.Price.ToString(amountFormat);
                            if (tl.LineAmount > 0)
                            {
                                usrCtrlCheckInSummary.LblTax = tl.tax_amount.ToString(amountFormat);
                            }
                            else
                            {
                                if (kioskTransaction.GetActiveTransactionLines.Exists(t => t.ProductID == product.ProductsContainerDTO.ProductId && t.LineAmount > 0))
                                {
                                    usrCtrlCheckInSummary.LblTax = kioskTransaction.GetActiveTransactionLines.Where(t => t.ProductID == product.ProductsContainerDTO.ProductId
                                                                    && t.LineAmount > 0).FirstOrDefault().tax_amount.ToString(amountFormat);
                                }
                                else
                                {
                                    decimal zero = 0;
                                    usrCtrlCheckInSummary.LblTax = zero.ToString(amountFormat);
                                }
                            }
                            usrCtrlCheckInSummary.LblPrice = (pCAppliedproductsContainerDTO.Price - Convert.ToDecimal(usrCtrlCheckInSummary.LblTax)).ToString(amountFormat);
                            usrCtrlCheckInSummary.LblTotal = (pCAppliedproductsContainerDTO.Price * qty).ToString(amountFormat);
                            this.flpPassportCoupon.Controls.Add(usrCtrlCheckInSummary);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while UpdatePassportCouponPanel(): " + ex.Message);
            }
            finally
            {
                flpPassportCoupon.ResumeLayout(true);
            }
            log.LogMethodExit();
        }

        private int GetCardCreditPlusConsumptionDiscountPercentage(int creditPlusConsumptionId)
        {
            log.LogMethodEntry(creditPlusConsumptionId);
            ResetKioskTimer();
            int discPercentage = 0;
            try
            {
                AccountBL accountBL = new AccountBL(Utilities.ExecutionContext, purchaseProductDTO.ParentCardNumber);
                if (accountBL != null && accountBL.AccountDTO != null)
                {
                    discPercentage = accountBL.GetCardCPConsumptionDiscPercentage(creditPlusConsumptionId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while Getting percentage details in GetPassportCouponDiscountPercentage(): " + ex.Message);
            }
            log.LogMethodExit(discPercentage);
            return discPercentage;
        }

        private void UpdateSummaryPanelForCombo()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.flpSummaryDetails.SuspendLayout();
                this.flpCheckInSummary.SuspendLayout();
                lblPackageName.Text = selectedProductRow["product_name"].ToString();
                ProductsContainerDTO selectedProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(KioskStatic.Utilities.ExecutionContext.SiteId, selectedProductId);
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines = trxLinesOfSelectedProduct;
                lblPackageCardnumber.Text = (kioskTransaction.HasCheckinProducts() == true) ?
                   KioskHelper.GetMaskedCardNumber(purchaseProductDTO.ParentCardNumber) : string.Empty;
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> applicableLineList = GetApplicableTrxLines(allTrxLines);
                lblPackageQty.Text = (parentLine.ProductTypeCode == ProductTypeValues.COMBO) ? "" : applicableLineList.Count.ToString();
                lblPackagePrice.Text = (parentLine.ProductTypeCode == ProductTypeValues.COMBO) ? "" : Convert.ToDecimal(selectedProductRow["ProductPrice"]).ToString(amountFormat);
                lblPackageTax.Text = (parentLine.ProductTypeCode == ProductTypeValues.COMBO) ? "" : (Convert.ToDecimal(selectedProductRow["Price"]) - Convert.ToDecimal(selectedProductRow["ProductPrice"])).ToString(amountFormat);
                lblPackageTotal.Text = (parentLine.ProductTypeCode == ProductTypeValues.COMBO) ? "" : (Convert.ToDecimal(selectedProductRow["Price"]) * applicableLineList.Count).ToString(amountFormat);
                if (selectedProductsContainerDTO.ProductType == ProductTypeValues.CHECKIN || selectedProductsContainerDTO.ProductType == ProductTypeValues.ATTRACTION)
                {
                    if (allTrxLines.Exists(t => t.ProductID == selectedProductId && t.CreditPlusConsumptionId > -1))
                    {
                        lblPackageTotal.Text += "*";
                    }
                }
                decimal productPrice = 0;
                if (purchaseProductDTO.ProductQtyMappingDTOs != null && parentLine.ProductTypeCode == ProductTypeValues.COMBO)
                {
                    foreach (ProductQtyMappingDTO productQtyMapping in purchaseProductDTO.ProductQtyMappingDTOs)
                    {
                        Semnox.Parafait.Transaction.Transaction.TransactionLine productTrxLine = allTrxLines.Where(t => t.ProductID == productQtyMapping.ProductsContainerDTO.ProductId).FirstOrDefault();
                        //if quantity > 0 then create usrCntrl for that product
                        UsrCtrlCheckInSummary usrCtrlCheckInSummary = CreateUsrCtlElement(executionContext, productQtyMapping.ProductsContainerDTO);
                        usrCtrlCheckInSummary.Width = panelSummary.Width;
                        string productName = KioskHelper.GetProductName(productQtyMapping.ProductsContainerDTO.ProductId);
                        usrCtrlCheckInSummary.LblPackage = productQtyMapping.ProductsContainerDTO.Price.ToString(amountFormatWithCurrencySymbol)
                            + " ("
                            + productName + ")";
                        usrCtrlCheckInSummary.LblQuantity = productQtyMapping.Quantity.ToString();
                        usrCtrlCheckInSummary.LblPrice = (Math.Truncate(productTrxLine.LineAmount * 100) / 100).ToString(amountFormat); //Math.Truncate() is used not to round off the price. This will be problamatic in cases where, price=9.99 and tax=0.48. Total does not match
                        usrCtrlCheckInSummary.LblDiscountPerc = "";

                        if (productTrxLine.LineAmount > 0)
                        {
                            usrCtrlCheckInSummary.LblTax = productTrxLine.tax_amount.ToString(amountFormat);
                        }
                        else
                        {
                            if (kioskTransaction.GetActiveTransactionLines.Exists(t => t.ProductID == productQtyMapping.ProductsContainerDTO.ProductId && t.LineAmount > 0))
                            {
                                usrCtrlCheckInSummary.LblTax = kioskTransaction.GetActiveTransactionLines.Where(t => t.ProductID == productQtyMapping.ProductsContainerDTO.ProductId
                                                                && t.LineAmount > 0).FirstOrDefault().tax_amount.ToString(amountFormat);
                            }
                            else
                            {
                                decimal zero = 0;
                                usrCtrlCheckInSummary.LblTax = zero.ToString(amountFormat);
                            }
                        }

                        usrCtrlCheckInSummary.LblTotal = Convert.ToDecimal(allTrxLines.Where(t => t.ProductID == productQtyMapping.ProductsContainerDTO.ProductId).ToList().Sum(y => y.LineAmount)).ToString(amountFormat);
                        if (allTrxLines.Exists(t => t.ProductID == productQtyMapping.ProductsContainerDTO.ProductId && t.CreditPlusConsumptionId > -1))
                        {
                            usrCtrlCheckInSummary.LblTotal += "*";
                        }
                        productPrice += (productQtyMapping.ProductsContainerDTO.Price + Convert.ToDecimal(usrCtrlCheckInSummary.LblTax)) * (productQtyMapping.Quantity);
                        this.flpSummaryDetails.Controls.Add(usrCtrlCheckInSummary);
                    }
                    //lblPackageTotal.Text = productPrice.ToString(amountFormatWithCurrencySymbol);
                    lblPackageTotal.Text = string.Empty; //combo price is currently not supported
                }
                lblTotalToPayValue.Text = kioskTransaction.GetTrxNetAmount().ToString(amountFormatWithCurrencySymbol);
                if (kioskTransaction.ShowCartInKiosk == true)
                {
                    panelTotalToPay.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing UpdateSummaryPanelForCombo(): " + ex.Message);
            }
            finally
            {
                this.flpSummaryDetails.ResumeLayout(true);
                this.flpCheckInSummary.ResumeLayout(true);
            }

            log.LogMethodExit();
        }

        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> GetApplicableTrxLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> allTrxLines)
        {//get lines belonging to non-combo checkin lines
            log.LogMethodEntry();
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> applicableLineList = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();

            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> tLineList = null;
            tLineList = allTrxLines.Where(tl => tl.LineValid && tl.ProductID == selectedProductId && tl.ComboChildLine == false && tl.ComboproductId == -1).ToList();

            if (tLineList != null && tLineList.Any())
            {
                applicableLineList.AddRange(tLineList);
            }

            log.LogMethodExit(applicableLineList);
            return applicableLineList;
        }

        private UsrCtrlCheckInSummary CreateUsrCtlElement(ExecutionContext executionContext, ProductsContainerDTO childProdsContainerDTO)
        {
            log.LogMethodEntry(executionContext, childProdsContainerDTO);
            ResetKioskTimer();
            UsrCtrlCheckInSummary purchaseSummary = null;
            try
            {
                purchaseSummary = new UsrCtrlCheckInSummary(executionContext, childProdsContainerDTO, flpSummaryDetails.Width, lblGreeting.Font);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error while executing CreateUsrCtlElement(): " + ex.Message);
            }
            log.LogMethodExit(purchaseSummary);
            return purchaseSummary;
        }

        private void UpdateDiscountPanel()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                List<DiscountsSummaryDTO> discountsSummaryDTOList = kioskTransaction.GetTransactionDiscountsSummaryList;
                if (discountsSummaryDTOList != null && discountsSummaryDTOList.Any() && discountsSummaryDTOList.Exists(tds => tds.CouponNumbers != null && tds.CouponNumbers.Any()))
                {
                    discountSummaryDTO = discountsSummaryDTOList.Find(tds => tds.CouponNumbers != null && tds.CouponNumbers.Any());
                }
                if (discountSummaryDTO != null)
                {
                    panelDiscount.Visible = true;
                    panelDiscountLine.Visible = true;
                    txtDiscountName.Text = discountSummaryDTO.DiscountName;
                    txtDiscAmount.Text = discountSummaryDTO.DiscountAmount.ToString(amountFormat);

                    using(UnitOfWork unitOfWork = new UnitOfWork())
                    {
                        //DiscountsBL discountsBL = DiscountMasterList.GetDiscountsBL(executionContext, discountSummaryDTO.DiscountId);
                        DiscountsBL discountsBL = new DiscountsBL(executionContext, unitOfWork, discountSummaryDTO.DiscountId, false, false);
                        txtDiscPerc.Text = (discountsBL.DiscountsDTO.DiscountPercentage == null) ? "" : Convert.ToInt32(discountSummaryDTO.DiscountPercentage).ToString() + "%";
                    }
                    
                }
                else
                {
                    panelDiscount.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error setting custom images for Purchase Summary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void displayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            Application.DoEvents();
            txtMessage.Text = message;
            log.LogMethodExit();
        }

        private void DisplayValidtyMsg(int productId)
        {
            log.LogMethodEntry(productId);
            ResetKioskTimer();
            try
            {
                string validityMsg = "";
                validityMsg = KioskStatic.GetProductCreditPlusValidityMessage(productId, selectedEntitlementType);
                int lineCount = Regex.Split(validityMsg, "\r\n").Count();
                if (lineCount > 2)
                {
                    lblProductCPValidityMsg.AutoEllipsis = true;
                    string[] validityMsgList = Regex.Split(validityMsg, "\r\n");
                    lblProductCPValidityMsg.Text = validityMsgList[0] + "\r\n" + validityMsgList[1];
                    System.Windows.Forms.ToolTip validtyMsgToolTip = new ToolTip();
                    validtyMsgToolTip.SetToolTip(lblProductCPValidityMsg, validityMsg);
                }
                else
                {
                    lblProductCPValidityMsg.Text = validityMsg;
                }
            }
            catch (Exception ex)
            {
                log.Error("Unexpected error in DisplayValidtyMsg() of Purchase Summary", ex);
                KioskStatic.logToFile("Unexpected error in DisplayValidtyMsg() of Purchase Summary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CheckInSummaryBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;

                this.panelSummary.BackgroundImage =
                    this.panelSummaryHeader.BackgroundImage =
                    this.panelDiscountLine.BackgroundImage =
                    this.panelDepositLine.BackgroundImage = ThemeManager.CurrentThemeImages.PurchaseSummaryTableImage;

                this.btnApplyCoupon.BackgroundImage = ThemeManager.CurrentThemeImages.DiscountButton;
                this.btnCancel.BackgroundImage =
                this.btnPrev.BackgroundImage =
                this.btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Unexpected error setting custom images for Purchase Summary: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFont()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            try
            {
                foreach (Control c in this.Controls["flpCheckInSummary"].Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.Font = new Font(lblGreeting.Font.FontFamily, c.Font.Size, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFont() of Check-In summary screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements of check in Summary");
            try
            {
                foreach (Control c in panelSummaryHeader.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryDetailsTextForeColor;//Summary Panel header
                    }
                }
                foreach (Control c in panelSummary.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryPanelTextForeColor;//Summary Panel main product
                    }
                }
                foreach (Control c in passportCouponHeader.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryDetailsTextForeColor;//Summary Panel header
                    }
                }
                foreach (Control c in panelDeposit.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryDetailsTextForeColor;//Deposite panel header
                    }
                }
                foreach (Control c in panelDiscount.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryDetailsTextForeColor;//Discount panel header
                    }
                }

                foreach (Control c in panelTotalToPay.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryTotalToPayTextForeColor;//Donation panel header
                    }
                }

                this.btnHome.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryHomeButtonTextForeColor;//Home button
                this.lblDiscount.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryDiscountLabelTextForeColor;//Discount Coupon Label
                this.btnApplyCoupon.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryApplyDiscountBtnTextForeColor;//Apply discount Coupon button
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryCPValidityTextForeColor;//back button
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryBackButtonTextForeColor;//proceed button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryBackButtonTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryProceedButtonTextForeColor;//product cp validity
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryFooterMessageTextForeColor;//Footer message
                //this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.lblPassportCoupon.ForeColor = KioskStatic.CurrentTheme.CheckInSummaryLblPassportCouponTextForeColor;//passport coupon label
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors for Purchase Summary", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements of check in Summary: " + ex.Message);
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
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmCheckInSummary_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //Cursor.Hide();
                log.Info(this.Name + ": Form closed");
                StopKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmPurchaseSummary_FormClosed()", ex);
                KioskStatic.logToFile("Error occurred while executing frmPurchaseSummary_FormClosed(): " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
