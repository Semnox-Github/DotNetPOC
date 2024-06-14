/********************************************************************************************
* Project Name - Parafait_Kiosk - CardCount.cs
* Description  - CardCount.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        06-Sep-2019      Deeksha            Added logger methods.
*2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
*2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.140.0     18-Oct-2021      Sathyavathi        Check-In Check-Out feature in Kiosk
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.0.0   23-Sep-2022      Sathyavathi        Check-In feature Phase-2
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
*2.150.7     10-Nov-2023      Sathyavathi        Customer Lookup Enhancement
 ********************************************************************************************/
using System;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.POS;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmCardCount : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Timer timeout = new Timer();
        private int _MaxCards = 0;
        private DataRow _productRow;
        private string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        private List<PaymentModeDTO> paymentModeDTOList = null;
        private int maxCardsAllowed = 6;
        private const string KIOSKSETUP = "KIOSK_SETUP";
        private const string MAXCARDSINCARDCOUNTUI = "MAX_CARDS_IN_CARD_COUNT_UI";
        private string selectedLoyaltyCardNo = string.Empty;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        private ExecutionContext executionContext;

        public frmCardCount(KioskTransaction kioskTransaction, DataRow productRow, string entitlementType, int maxCards = 0)
        {
            log.LogMethodEntry(productRow, entitlementType, maxCards);
            InitializeComponent();
            this.executionContext = KioskStatic.Utilities.ExecutionContext;
            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme
            selectedEntitlementType = entitlementType;
            pbClientLogo.Visible = false;
            _productRow = productRow;
            this.kioskTransaction = kioskTransaction;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetTheme();
            WindowState = FormWindowState.Maximized;
            _MaxCards = maxCards;
            GetMaxCardsAllowed();
            SetCardCountButtons();
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1256);
            KioskStatic.logToFile("Loading Card Count UI");
            log.Info("Loading Card Count UI");
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }
        private void GetMaxCardsAllowed()
        {
            log.LogMethodEntry();
            try
            {
                LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.SiteId, KIOSKSETUP, executionContext);
                LookupValuesContainerDTO valuesContainerDTO = null;
                if (lookupsContainerDTO != null && lookupsContainerDTO.LookupValuesContainerDTOList != null && lookupsContainerDTO.LookupValuesContainerDTOList.Any())
                {
                    valuesContainerDTO = lookupsContainerDTO.LookupValuesContainerDTOList.Find(luv => luv.LookupValue == MAXCARDSINCARDCOUNTUI);
                }
                if (valuesContainerDTO != null)
                {
                    maxCardsAllowed = Convert.ToInt32(valuesContainerDTO.Description);
                }
                else
                {
                    KioskStatic.logToFile("MAX_CARDS_IN_CARD_COUNT_UI is not defined in KIOSK_SETUP look up. Going with default value 6");
                    log.Info("MAX_CARDS_IN_CARD_COUNT_UI is not defined in KIOSK_SETUP look up. Going with default value 6");
                    maxCardsAllowed = 6;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                maxCardsAllowed = 6;
                KioskStatic.logToFile("Error while fetching MAX_CARDS_IN_CARD_COUNT_UI from KIOSK_SETUP look up. Going with default value 6");
                log.Info("Error while fetching MAX_CARDS_IN_CARD_COUNT_UI from KIOSK_SETUP look up. Going with default value 6");

            }
            KioskStatic.logToFile("maxCardsAllowed: " + maxCardsAllowed.ToString());
            log.LogMethodExit(maxCardsAllowed);
        }

        private void SetCardCountButtons()
        {
            log.LogMethodEntry();
            log.LogVariableState("MaxCard", _MaxCards);
            if (_MaxCards == 0)
                _MaxCards = maxCardsAllowed;

            btnOne.Visible = true;
            if (_MaxCards > 1)
                btnTwo.Visible = true;

            if (_MaxCards > 2)
                btnThree.Visible = true;

            if (_MaxCards > 3)
                btnFour.Visible = true;

            if (_MaxCards > 4)
                btnFive.Visible = true;

            if (_MaxCards > 5)
                btnSix.Visible = true;

            if (_MaxCards > 6)
                btnSeven.Visible = true;

            if (_MaxCards > 7)
                btnEight.Visible = true;

            if (_MaxCards > 8)
                btnNine.Visible = true;

            if (_MaxCards > 9)
                btnTen.Visible = true;

            if (_MaxCards < 5)
            {
                flpCardCount.Top += 100;
            }
            else if (_MaxCards > 6)
            {
                flpCardCount.Width = 1050;
                flpCardCount.Top += 100;
                foreach (Control c in flpCardCount.Controls)
                {
                    c.Width = flpCardCount.Width / 2 - c.Margin.Left - c.Margin.Right;
                    c.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            flpCardCount.Left = (this.Width - flpCardCount.Width) / 2;
            log.LogMethodExit();
        }
        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            SetlblBelowText();
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            log.LogMethodExit();
        }

        private void SetlblBelowText()
        {
            log.LogMethodEntry();
            string prodText = string.Empty;
            if (string.IsNullOrWhiteSpace(_productRow["price"].ToString()) == false)
            {
                prodText = Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + " (" + _productRow["product_name"].ToString() + ")";
            }
            else
            {
                prodText = _productRow["product_name"].ToString();
            }
            lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, prodText);
            if (selectedEntitlementType == KioskTransaction.CREDITS_ENTITLEMENT)
            {
                if (Convert.ToInt32(_productRow["CardCount"]) <= 1 && _productRow["QuantityPrompt"].ToString().Equals("Y"))
                {
                    lblBelow.Text = KioskStatic.Utilities.MessageUtils.getMessage(1726, Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    //Points will not be split and &1 will be added to each card.
                }
                else
                {
                    prodText = "(" + _productRow["product_name"].ToString() + ")";
                    lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, prodText);
                    lblBelow.Text = KioskStatic.Utilities.MessageUtils.getMessage(1727);
                    //"Points will be divided onto each card");
                }
            }
            else
            {
                if (Convert.ToInt32(_productRow["CardCount"]) <= 1 && _productRow["QuantityPrompt"].ToString().Equals("Y"))
                {
                    lblBelow.Text = KioskStatic.Utilities.MessageUtils.getMessage(1728, Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                    //Time will not be split and &1 will be added to each card.
                }
                else
                {
                    prodText = "(" + _productRow["product_name"].ToString() + ")";
                    lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, prodText);
                    lblBelow.Text = KioskStatic.Utilities.MessageUtils.getMessage(1729);
                    //"Time will be divided onto each card");
                }
            }
            log.LogMethodExit();
        }

        private void frmCardCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timeout.Stop();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            //Button b = sender as Button;//Starts:Modification on 17-Dec-2015 for introducing new theme
            // b.BackgroundImage = Properties.Resources.CardCountPressed;
            // btnOne.BackgroundImage = Properties.Resources.CardCountNormal;//Ends:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            // Button b = sender as Button;//Starts:Modification on 17-Dec-2015 for introducing new theme
            // b.BackgroundImage = Properties.Resources.CardCountNormal;
            // btnOne.BackgroundImage = Properties.Resources.CardCountPressed;//Ends:Modification on 17-Dec-2015 for introducing new theme
        }

        private void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button b = sender as Button;
                b.Enabled = false;
                DisableButtons();
                KioskStatic.logToFile("Card Count Button Pressed");
                int CardCount = Convert.ToInt32(b.Text);
                CallTransactionForm(CardCount);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
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

        private void CallTransactionForm(int CardCount)
        {
            log.LogMethodEntry(CardCount);
            StopKioskTimer();
            ResetKioskTimer();
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1256);
            int productId = Convert.ToInt32(_productRow["product_id"]);
            CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            if (commonBA != null && kioskTransaction.GetTotalPaymentsReceived()> 0) //here check if paymentModeDTOList has data
            {
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
                            using (frmCardTransaction frm = new frmCardTransaction(kioskTransaction, paymentModeDTO))
                            {
                                frm.ShowDialog();
                                kioskTransaction = frm.GetKioskTransaction;
                            }
                        }
                        else
                        {
                            string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                            KioskStatic.logToFile("frmCardCount: " + msg);
                            //frmOKMsg.ShowUserMessage(msg); 
                            txtMessage.Text = msg;
                        }
                    }
                }
                Close();
                log.LogMethodExit();
                return;
            }
            else
            {
                kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, _productRow, CardCount, selectedEntitlementType);
                frmChooseProduct.AlertUser(productId, kioskTransaction, ProceedActionImpl);
                if (kioskTransaction.ShowCartInKiosk == false)
                {
                    kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                    ProceedActionImpl(kioskTransaction);
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4842);
                    KioskStatic.logToFile("frmCardCount: " + msg);
                    //frmOKMsg.ShowUserMessage(msg);
                    txtMessage.Text = msg;
                    this.Close();
                }
            }
            //timeout.Start();
            StartKioskTimer();
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
        private void SetTheme()
        {
            log.LogMethodEntry();
            this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.CardCountBackgroundImage);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnOne.BackgroundImage = ThemeManager.CurrentThemeImages.NoOfCardButton;
            btnTwo.BackgroundImage = btnThree.BackgroundImage = btnFour.BackgroundImage =
            btnFive.BackgroundImage = btnSix.BackgroundImage = btnSeven.BackgroundImage =
            btnEight.BackgroundImage = btnNine.BackgroundImage = btnTen.BackgroundImage = ThemeManager.CurrentThemeImages.PassesTwo;

            btnOne.TextAlign = btnTwo.TextAlign = btnThree.TextAlign =
            btnFour.TextAlign = btnFive.TextAlign = btnSix.TextAlign =
            btnSeven.TextAlign = btnEight.TextAlign = btnNine.TextAlign = btnTen.TextAlign = KioskStatic.GetContextAligment(KioskStatic.CurrentTheme.BasicCardCountButtonTextAlignment);

            btnPrev.BackgroundImage = btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            SetCustomizedFontColors();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCardCount");
            try
            {
                foreach (Control c in flpCardCount.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.CardCountScreenCardBtnsTextForeColor;//Card count button
                    }
                }
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.CardCountScreenHeader1TextForeColor;//How many Cards?
                //this.lblPoints.ForeColor = KioskStatic.CurrentTheme.CardCountScreenHeader2TextForeColor;//Points will be divided onto each card
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardCountScreenBackBtnTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CardCountScreenCancelBtnTextForeColor;//Cancel button
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor;//Footer text message
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.HomeScreenFooterTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCardCount: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Disable card count Buttons");
                flpCardCount.SuspendLayout();
                List<Button> buttonList = flpCardCount.Controls.OfType<Button>().ToList();
                if (buttonList != null && buttonList.Any())
                {
                    for (int j = buttonList.Count - 1; j >= 0; j--)
                    {
                        buttonList[j].Enabled = false;
                    }
                }
                this.btnHome.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnCancel.Enabled = false; 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                flpCardCount.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Enable card count Buttons");
                flpCardCount.SuspendLayout();
                List<Button> buttonList = flpCardCount.Controls.OfType<Button>().ToList();
                if (buttonList != null && buttonList.Any())
                {
                    for (int j = buttonList.Count - 1; j >= 0; j--)
                    {
                        buttonList[j].Enabled = true;
                    }
                }
                this.btnHome.Enabled = true;
                this.btnPrev.Enabled = true;
                this.btnCancel.Enabled = true; 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                flpCardCount.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
    }
}
