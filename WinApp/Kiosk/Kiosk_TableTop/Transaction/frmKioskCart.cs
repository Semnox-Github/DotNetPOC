/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmKioskCart.cs
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Product;
using Semnox.Parafait.Discounts;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk
{
    public partial class frmKioskCart : BaseFormKiosk
    {
        private string couponNumber = String.Empty;
        private string AMOUNTFORMAT;
        private string AMOUNTFORMATWITHCURRENCYSYMBOL;
        private ExecutionContext executionContext;
        private int orginalXValueForFlpCartProducts;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const bool cartElementsInReadMode = false;
        private string defaultMsg = string.Empty;
        public frmKioskCart(KioskTransaction trx)
        {
            log.LogMethodEntry("kioskTransaction");
            InitializeComponent();
            DisplaybtnCancel(true);
            DisplaybtnCart(false);
            kioskTransaction = trx;
            lblGreeting1.Visible = true;
            executionContext = KioskStatic.Utilities.ExecutionContext;
            AMOUNTFORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            AMOUNTFORMATWITHCURRENCYSYMBOL = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            orginalXValueForFlpCartProducts = this.flpCartProducts.Location.X;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            defaultMsg = MessageContainerList.GetMessage(executionContext, "Cart Items");
            txtMessage.Text = defaultMsg;
            lblGreeting1.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_DISCOUNTS_IN_POS").Equals("N"))
            {
                btnDiscount.Text = MessageContainerList.GetMessage(executionContext, btnDiscount.Text);
                btnDiscount.Visible = false;
            }
            KioskStatic.setDefaultFont(this);
            RefreshCartData();
            SetDiscountButtonText();
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public void GroupAndDisplayTrxLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            try
            {
                this.flpCartProducts.SuspendLayout();
                this.flpCartProducts.Controls.Clear();
                List<List<KioskHelper.LineData>> displayTrxLineData = KioskHelper.GroupDisplayTrxLinesNew(trxLines);
                List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> displayTrxLines = KioskHelper.ConvertTrxLineData(displayTrxLineData);
                AddLinesToDisplay(displayTrxLines);
                double totalCharges = KioskHelper.GetChargesToDisplay(kioskTransaction);
                lblCharges.Text = totalCharges.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                // display trx total
                double subTotalAmouunt = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                lblTotal.Text = (subTotalAmouunt).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                // display tax 
                lblTax.Text = (kioskTransaction.GetTrxTaxAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                AddDiscountDetailsToDisplay();
                // display grand total
                lblGrandTotal.Text = (kioskTransaction.GetTrxNetAmount()).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                //AdjustflpSize();
                AdjustflpLocation();
            }
            finally
            {
                this.flpCartProducts.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void frmKioskCart_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            AdjustflpLocation();
            log.LogMethodExit();
        }
        private void AddLinesToDisplay(List<List<Semnox.Parafait.Transaction.Transaction.TransactionLine>> groupedTrxLines)
        {
            log.LogMethodEntry();
            for (int i = 0; i < groupedTrxLines.Count; i++) // display card lines
            {
                if (groupedTrxLines[i].Exists(tl => tl.LineValid))
                {
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxCardProducts = groupedTrxLines[i];
                    usrCtrlCart usrCtrlCart = CreateUsrCtlElement(trxCardProducts);
                    this.flpCartProducts.Controls.Add(usrCtrlCart);
                }
            }
            log.LogMethodExit();
        }
        private void AdjustflpLocation()
        {
            log.LogMethodEntry();
            int xVal = orginalXValueForFlpCartProducts;
            if (bigVerticalScrollCartProducts.Visible == false)
            {
                int adjustFact = (bigVerticalScrollCartProducts.Width / 2) + 4;
                xVal = orginalXValueForFlpCartProducts + adjustFact;
            }
            this.lblCartProductName.Location = new Point(xVal, lblCartProductName.Location.Y);
            this.lblCartProductTotal.Location = new Point(xVal + this.lblCartProductName.Width + 2, lblCartProductTotal.Location.Y);
            this.flpCartProducts.Location = new Point(xVal, flpCartProducts.Location.Y);
            log.LogMethodExit();
        }
        private void AddDiscountDetailsToDisplay()
        {
            log.LogMethodEntry();
            double discountAmount = kioskTransaction.GetTransactionDiscountsAmount();
            lblDiscount.Text = discountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
            log.LogMethodExit();
        }
        private void SetDiscountButtonText()
        {
            log.LogMethodEntry();
            DiscountsSummaryDTO discountsSummaryDTO = kioskTransaction.GetAppliedCouponDiscountSummary();
            SetBtnDiscountText(discountsSummaryDTO);
            log.LogMethodExit();
        }
        private void SetBtnDiscountText(DiscountsSummaryDTO discountsSummaryDTO)
        {
            log.LogMethodEntry();
            this.lblDiscountInfo.Text = string.Empty;
            if (discountsSummaryDTO != null)
            {
                lblDiscount.Text = discountsSummaryDTO.DiscountAmount.ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                double subTotalAmount = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                lblTotal.Text = (subTotalAmount).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                btnDiscount.Text = MessageContainerList.GetMessage(executionContext, "Remove Coupon");
                btnDiscount.Tag = "cancelDiscount";
                couponNumber = discountsSummaryDTO.CouponNumbers.First();
                string part1 = MessageContainerList.GetMessage(executionContext, "Discount") + ": ";
                string part2 = " " + MessageContainerList.GetMessage(executionContext, "applied");
                lblDiscountInfo.Text = part1 + discountsSummaryDTO.DiscountName + part2;
            }
            log.LogMethodExit();
        }
        private usrCtrlCart CreateUsrCtlElement(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLines)
        {
            log.LogMethodEntry(trxLines);
            ResetKioskTimer();
            usrCtrlCart usrCtrlElement = null;
            try
            {
                usrCtrlElement = new usrCtrlCart(executionContext, trxLines, cartElementsInReadMode);
                usrCtrlElement.CancelSelectedLines += new usrCtrlCart.cancelSelectedLines(CancelSelectedLines);
                usrCtrlElement.RefreshCartData += new usrCtrlCart.refreshCartData(RefreshCartData);
            }
            catch (Exception ex)
            {
                string msg = "Error in CreateUsrCtlElement() of cart screen";
                log.Error(msg);
                KioskStatic.logToFile(msg + " : " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit(usrCtrlElement);
            return usrCtrlElement;
        }
        private void CancelSelectedLines(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> selectedTrxLines)
        {
            log.LogMethodEntry(selectedTrxLines);
            //Processing..Please wait... 
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1008);
            Application.DoEvents();
            if (selectedTrxLines != null && selectedTrxLines.Any())
            {
                selectedTrxLines = AddRelatedCardLinesForCombo(selectedTrxLines);
                for (int i = 0; i < selectedTrxLines.Count; i++)
                {
                    kioskTransaction.RemoveSpecificTransactionLine(selectedTrxLines[i]);
                }
            }
            log.LogMethodExit();
        }

        private List<Semnox.Parafait.Transaction.Transaction.TransactionLine> AddRelatedCardLinesForCombo(List<Semnox.Parafait.Transaction.Transaction.TransactionLine> selectedTrxLines)
        {
            log.LogMethodEntry(selectedTrxLines);
            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLineSet = new List<Semnox.Parafait.Transaction.Transaction.TransactionLine>();

            if (selectedTrxLines != null && selectedTrxLines.Any())
            {
                trxLineSet.AddRange(selectedTrxLines);
                for (int i = 0; i < selectedTrxLines.Count; i++)
                {
                    Semnox.Parafait.Transaction.Transaction.TransactionLine trxL = selectedTrxLines[i];
                    if (trxL.ProductTypeCode == ProductTypeValues.COMBO)
                    {
                        List<Semnox.Parafait.Transaction.Transaction.TransactionLine> cardChildren = KioskHelper.GetChildLinesWithCards(executionContext, trxL, selectedTrxLines);
                        if (cardChildren != null && cardChildren.Any())
                        {
                            List<string> cardNumberList = cardChildren.Select(tl => tl.CardNumber).Distinct().ToList();
                            List<Semnox.Parafait.Transaction.Transaction.TransactionLine> relatedComboLines = KioskHelper.GetRelatedComboLines(executionContext, kioskTransaction, trxL, cardNumberList);
                            if (relatedComboLines != null && relatedComboLines.Any())
                            {
                                int qtyCount = 1 + relatedComboLines.Count;
                                string msg = MessageContainerList.GetMessage(executionContext, 5468, qtyCount);
                                using (frmYesNo f = new frmYesNo(msg))
                                {
                                    DialogResult dr = f.ShowDialog();
                                    if (dr == DialogResult.Yes)
                                    {
                                        trxLineSet.AddRange(relatedComboLines);
                                    }
                                    else
                                    {
                                        //do not proceed if we have not received Yes as response.
                                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 5469));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(trxLineSet);
            return trxLineSet;
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Text = defaultMsg;
                ResetKioskTimer();
                StopKioskTimer();
                if (btnDiscount.Tag.ToString() == "applyDiscount")
                {
                    couponNumber = string.Empty;
                    using (frmScanCoupon frm = new frmScanCoupon(kioskTransaction))
                    {
                        DialogResult dr = frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                        if (dr != System.Windows.Forms.DialogResult.Cancel)
                        {
                            couponNumber = frm.CodeScaned;
                            DiscountsSummaryDTO discountsSummaryDTO = kioskTransaction.GetAppliedDiscountSummaryForCoupon(couponNumber);
                            if (discountsSummaryDTO != null)
                            {
                                SetBtnDiscountText(discountsSummaryDTO);
                                RefreshCartData();
                            }
                        }
                    }
                }
                else if (btnDiscount.Tag.ToString() == "cancelDiscount")
                {
                    double subTotalAmountBefore = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                    lblTotal.Text = (subTotalAmountBefore).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                    try
                    {
                        kioskTransaction.RemoveDiscountCoupon(couponNumber);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while executing btnDiscount_Click()" + ex.Message);
                        KioskStatic.logToFile(ex.Message);
                        txtMessage.Text = ex.Message;
                    }
                    double subTotalAmountAfter = KioskHelper.GetSubTotalForDisplay(kioskTransaction);
                    lblTotal.Text = (subTotalAmountAfter).ToString(AMOUNTFORMATWITHCURRENCYSYMBOL);
                    btnDiscount.Text = MessageContainerList.GetMessage(executionContext, "Apply Coupon");
                    btnDiscount.Tag = "applyDiscount";
                    lblDiscountInfo.Text = string.Empty;
                    RefreshCartData();
                }
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        private void RefreshCartData()
        {
            log.LogMethodEntry();
            try
            {
                //Processing..Please wait... 
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1008);
                Application.DoEvents();
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> transactionLines = kioskTransaction.GetActiveTransactionLines;
                GroupAndDisplayTrxLines(transactionLines);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Refreshing Cart Data", ex);
                KioskStatic.logToFile("Errow while Refreshing Cart Data: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                txtMessage.Text = defaultMsg;
            }
            log.LogMethodExit();
        }
        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtMessage.Text = defaultMsg;
                StopKioskTimer();
                kioskTransaction.TransactionHasItems();
                int itemCount = kioskTransaction.GetCartItemCount;
                if (itemCount > 0)
                {
                    using (frmPaymentMode frpm = new frmPaymentMode(kioskTransaction))
                    {
                        DialogResult dr = frpm.ShowDialog();
                        kioskTransaction = frpm.GetKioskTransaction;
                    }
                    this.Close();
                }
                else
                {
                    //Cart is Empty
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4843);
                    KioskStatic.logToFile("Skipping cart proceed action: " + errMsg);
                    Semnox.Core.Utilities.ValidationException validationException = new Semnox.Core.Utilities.ValidationException(errMsg);
                    log.Error(validationException);
                    throw validationException;
                }
            }
            catch (Exception ex)
            {
                this.Show();
                StartKioskTimer();
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                txtMessage.Text = ex.Message;
                this.Close();
            }
            log.LogMethodExit();
        }
        private void btnContinue_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            KioskStatic.logToFile("Back button pressed");
            txtMessage.Text = defaultMsg;
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in KioskCart");
            try
            {
                this.lblDiscountInfo.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblChargeAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblTaxesHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblDiscountAmountHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblGrandTotalHeader.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblHeaderTextForeColor;
                this.lblCharges.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblDiscount.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblGrandTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTax.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblAmountTextForeColor;
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblGreeting1TextForeColor;
                this.btnContinue.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartButtonTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartButtonTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartButtonTextForeColor;
                this.btnDiscount.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartDiscountButtonTextForeColor;
                this.lblCartProductName.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblProductHeaderTextForeColor;
                this.lblCartProductTotal.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartLblProductHeaderTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmKioskCartFooterTxtMsgTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor;
                btnDiscount.BackgroundImage = ThemeManager.CurrentThemeImages.DiscountButton;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CartScreenBackgroundImage);
                this.btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                this.btnContinue.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                this.pnlProductSummary.BackgroundImage = ThemeManager.CurrentThemeImages.TablePurchaseSummary;
                this.bigVerticalScrollCartProducts.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in KioskCart: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
    }
}
