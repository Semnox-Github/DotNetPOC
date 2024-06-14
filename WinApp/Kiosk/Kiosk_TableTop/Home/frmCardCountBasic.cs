/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCardCount.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat       Created: TableTop Kiosk Changes
*2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk.Home
{
    public partial class frmCardCountBasic : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Timer timeout = new Timer();
        private DataRow _productRow;
        private string selectedEntitlementType;
        private Semnox.Parafait.KioskCore.BillAcceptor.CommonBillAcceptor billAcceptor;
        private string selectedLoyaltyCardNo = "";
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmCardCountBasic(KioskTransaction kioskTransaction, DataRow productRow, int maxCards, string entitlementType)
        {
            log.LogMethodEntry(productRow, maxCards, entitlementType);
            selectedEntitlementType = entitlementType;
            this.kioskTransaction = kioskTransaction;
            InitializeComponent();
            KioskStatic.setDefaultFont(this);

            _productRow = productRow;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CardCountBasicBackgroundImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnOne.BackgroundImage = ThemeManager.CurrentThemeImages.NoOfCardButton;
            //btnOne.Size = ThemeManager.CurrentThemeImages.NoOfCardButton.Size;
            btnTwo.BackgroundImage = btnThree.BackgroundImage = ThemeManager.CurrentThemeImages.PassesTwo;
            //btnTwo.Size = btnThree.Size = ThemeManager.CurrentThemeImages.PassesTwo.Size;
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            WindowState = FormWindowState.Maximized;

            if (maxCards == 0)
            {
                maxCards = 3;
            }
            btnOne.Visible = true;
            if (maxCards > 1)
            {
                btnTwo.Visible = true;
            }
            if (maxCards > 2)
            {
                btnThree.Visible = true;
            }
            if (maxCards <= 2)
            {
                this.flpCardCount.Location = new System.Drawing.Point(550, 408);
            }
            log.LogVariableState("MaxCard", maxCards);
            billAcceptor = (Application.OpenForms[0] as frmHome).commonBillAcceptor;

            string amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            lblHeading.Text = kioskTransaction.GetTotalPaymentsReceived().ToString(amountFormatWithCurrencySymbol) + " " + lblHeading.Text;

            btnOne.Text = ThemeManager.InsertNewline(btnOne.Text, 13);
            btnTwo.Text = ThemeManager.InsertNewline(btnTwo.Text, 13);
            btnThree.Text = ThemeManager.InsertNewline(btnThree.Text, 13);
            btnOne.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.BasicCardCountButtonTextAlignment);
            btnTwo.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.BasicCardCountButtonTextAlignment);
            btnThree.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.BasicCardCountButtonTextAlignment);
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1256);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                KioskStatic.logToFile("Back button pressed");
                DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }
        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmCardCount_Load");
            string numFormat = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "NUMBER_FORMAT");
            double totalAmountReceived = (double)kioskTransaction.GetTotalPaymentsReceived();
            if (selectedEntitlementType == KioskTransaction.TIME_ENTITLEMENT)
                lblPoints.Text = KioskStatic.GetCreditsOnSplitVariableProduct(totalAmountReceived, selectedEntitlementType, null).ToString(numFormat) + " " + KioskStatic.Utilities.MessageUtils.getMessage("MINUTES");
            else
                lblPoints.Text = KioskStatic.GetCreditsOnSplitVariableProduct(totalAmountReceived, selectedEntitlementType, null).ToString(numFormat) + " " + lblPoints.Text;
            log.LogMethodExit();
        }


        private void frmCardCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timeout.Stop();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            // Button b = sender as Button;
            // b.BackgroundImage = Properties.Resources.CardCountPressed;
            // btnOne.BackgroundImage = Properties.Resources.CardCountNormal;
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            // Button b = sender as Button;
            // b.BackgroundImage = Properties.Resources.CardCountNormal;
            // btnOne.BackgroundImage = Properties.Resources.CardCountPressed;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                Button b = sender as Button;
                int CardCount = 1;
                if (b.Name.Equals("btnOne"))
                    CardCount = 1;
                else
                    CardCount = 2;
                log.LogVariableState("CardCount", CardCount);
                callTransactionForm(CardCount);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }


        void callTransactionForm(int CardCount)
        {
            //timeout.Stop();
            log.LogMethodEntry(CardCount);
            StopKioskTimer();
            CustomerDTO customerDTO = null;
            txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1256);

            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_ALOHA_LOYALTY_INTERFACE") == "Y"
                && string.IsNullOrWhiteSpace(kioskTransaction.LoyaltyCardNumber) == true)
            {
                using (frmLoyalty fm = new frmLoyalty())
                {
                    if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        selectedLoyaltyCardNo = fm.selectedLoyaltyCardNo;
                    if (fm != null)
                        fm.Dispose();
                }
                if (string.IsNullOrWhiteSpace(selectedLoyaltyCardNo) == false)
                {
                    kioskTransaction.LoyaltyCardNumber = selectedLoyaltyCardNo;
                }
            }
            int productId = Convert.ToInt32(_productRow["product_id"]);
            List<PaymentModeDTO> paymentModeDTOList = null;
            paymentModeDTOList = KioskStatic.paymentModeDTOList;
            if (paymentModeDTOList != null && paymentModeDTOList.Any())
            {
                PaymentModeDTO paymentModeDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                if (paymentModeDTO != null)
                {
                    kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, _productRow, CardCount, selectedEntitlementType);
                    //frmChooseProduct.AlertUser(productId, kioskTransaction, ProceedActionImpl);
                    if (kioskTransaction.ShowCartInKiosk == false)
                    {
                        kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                    }
                    else
                    {
                        string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                        KioskStatic.logToFile("frmCardCountBasic: " + msg);
                        //frmOKMsg.ShowUserMessage(msg);
                        txtMessage.Text = msg;
                    }
                    using (frmCardTransaction frm = new frmCardTransaction(kioskTransaction, paymentModeDTO))
                    {
                        frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                    }
                }
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
                log.LogMethodExit();
            }
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
        private void btnThree_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                // timeout.Stop();
                StopKioskTimer();
                List<PaymentModeDTO> paymentModeDTOList = null;
                paymentModeDTOList = KioskStatic.paymentModeDTOList;
                if (paymentModeDTOList != null && paymentModeDTOList.Any())
                {
                    PaymentModeDTO paymentModeDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                    if (paymentModeDTO != null)
                    {
                        using (frmCardCount frm = new frmCardCount(kioskTransaction, _productRow, selectedEntitlementType, 6))
                        {
                            DialogResult dr = frm.ShowDialog();
                            kioskTransaction = frm.GetKioskTransaction;
                            if (dr == System.Windows.Forms.DialogResult.No) // back button
                            {
                                //    secondsRemaining = 30;
                                //    timeout.Start();
                                ResetKioskTimer();
                                StartKioskTimer();
                            }
                            else
                            {
                                using (frmCardTransaction frmCT = new frmCardTransaction(kioskTransaction, paymentModeDTO))
                                {
                                    frmCT.ShowDialog();
                                    kioskTransaction = frm.GetKioskTransaction;
                                }
                                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in frmCardCountBasic-btnThreeClick : " + ex.Message);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCardCountBasics");
            try
            {
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader1TextForeColor;//(GIVES YOU)
                this.lblPoints.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader2TextForeColor;//(POINTS)
                this.label1.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader3TextForeColor;//(Please choose an option from below) 
                this.btnOne.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnOneTextForeColor;
                this.btnTwo.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnTwoTextForeColor;
                this.btnThree.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnThreeTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnPrevTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.HomeScreenBtnHomeTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCardCountBasics: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnPrev.Enabled = true;
                this.btnOne.Enabled = true;
                this.btnTwo.Enabled = true;
                this.btnThree.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in EnableButtons() in Card Count Basic screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                this.btnPrev.Enabled = false;
                this.btnOne.Enabled = false;
                this.btnTwo.Enabled = false;
                this.btnThree.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in DisableButtons() in Card Count Basic screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}
