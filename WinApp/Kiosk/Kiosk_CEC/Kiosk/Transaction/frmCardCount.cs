/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - frmcardCount.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 *2.80.1      02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
 *2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
 *2.120       17-Apr-2021      Guru S A           Wristband printing flow enhancements
 *2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
  *********************************************************************************************/
using System;
using System.Data;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk
{
    public partial class frmCardCount : BaseFormKiosk
    {
        //Timer timeout = new Timer();
        int _MaxCards = 0;
        DataRow _productRow;
        //string selecteEntitlementType;
        List<PaymentModeDTO> paymentModeDTOList = null;
        private int maxCardsAllowed = 6;
        private const string KIOSKSETUP = "KIOSK_SETUP";
        private const string MAXCARDSINCARDCOUNTUI = "MAX_CARDS_IN_CARD_COUNT_UI";


        string selectedEntitlementType = "Credits";
        string selectedLoyaltyCardNo = "";
        public frmCardCount(DataRow productRow, string entitlementType, int maxCards = 0)
        {
            log.LogMethodEntry(productRow, entitlementType, maxCards);
            InitializeComponent();
            this.BackgroundImage = KioskStatic.CurrentTheme.CardCountBackgroundImage;

            KioskStatic.setDefaultFont(this);
            selectedEntitlementType = entitlementType;

            KioskStatic.Utilities.setLanguage(this);

            _productRow = productRow;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            WindowState = FormWindowState.Maximized;

            _MaxCards = maxCards;
            GetMaxCardsAllowed();
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
            KioskStatic.logToFile("Loading Card Count UI");
            log.Info("Loading Card Count UI");
            log.LogMethodExit();
        }


        private void GetMaxCardsAllowed()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(KioskStatic.Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, KIOSKSETUP));
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, MAXCARDSINCARDCOUNTUI));
                searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, KioskStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParameters);
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    maxCardsAllowed = Convert.ToInt32(lookupValuesDTOList[0].Description);
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

        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
            //KioskStatic.setFieldLabelForeColor(lblHeading);
            displaybtnCancel(true);
            lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
            if (selectedEntitlementType == "Credits")
            {
                if (Convert.ToInt32(_productRow["CardCount"]) <= 1 && _productRow["QuantityPrompt"].ToString().Equals("Y"))
                {
                    lblPoints.Text = KioskStatic.Utilities.MessageUtils.getMessage(1726, Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                }
                else
                {
                    lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, "");
                    lblPoints.Text = KioskStatic.Utilities.MessageUtils.getMessage(1727);
                }
            }
            else
            {
                if (Convert.ToInt32(_productRow["CardCount"]) <= 1 && _productRow["QuantityPrompt"].ToString().Equals("Y"))
                {
                    lblPoints.Text = KioskStatic.Utilities.MessageUtils.getMessage(1728, Convert.ToDouble(_productRow["price"].ToString()).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                }
                else
                {
                    lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(1844, "");
                    lblPoints.Text = KioskStatic.Utilities.MessageUtils.getMessage(1729);
                }
            }
            SetCustomizedFontColors();
            setButtonBackGroundImage();
            log.LogMethodExit();            
        }

        private void setButtonBackGroundImage()
        {
           log.LogMethodEntry();
           btnOne.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnTwo.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnThree.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnFour.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnFive.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnSix.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnSeven.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnEight.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnNine.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            btnTen.BackgroundImage = KioskStatic.CurrentTheme.NoOfCardsButtonImage;
            log.LogMethodExit();
        }
       

        private void frmCardCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timeout.Stop();
        }

       

        private void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            int CardCount = Convert.ToInt32(b.Text);

            callTransactionForm(CardCount);
            log.LogMethodExit();
        }

        //private void btnPrev_Click(object sender, EventArgs e)
        //{
        //    DialogResult = System.Windows.Forms.DialogResult.No;
        //    Close();
        //}

        void callTransactionForm(int CardCount)
        {
            log.LogMethodEntry(CardCount);
            //timeout.Stop();
            //secondsRemaining = 30;
            StopKioskTimer();
            ResetKioskTimer();
            CustomerDTO customerDTO = null;
            if (KioskStatic.RegisterBeforePurchase)
            {
                if (_productRow["RegisteredCustomerOnly"].ToString() == "Y")
                {
                    customerDTO = CustomerStatic.ShowCustomerScreen();
                    if (customerDTO == null)
                    {
                        DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
                else
                {
                    Audio.PlayAudio(Audio.RegisterCardPrompt);
                    if (new frmYesNo(KioskStatic.Utilities.MessageUtils.getMessage(758), KioskStatic.Utilities.MessageUtils.getMessage(759)).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        customerDTO = CustomerStatic.ShowCustomerScreen();
                    }
                }
            }

            CommonBillAcceptor commonBA = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;
            if (commonBA != null && commonBA.GetAcceptance().totalValue > 0)
            {
                if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_ALOHA_LOYALTY_INTERFACE") == "Y")
                {
                    using (frmLoyalty fm = new frmLoyalty())
                    {
                        if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            selectedLoyaltyCardNo = fm.selectedLoyaltyCardNo;
                        if (fm != null)
                            fm.Dispose();
                    }
                }
                paymentModeDTOList = KioskStatic.paymentModeDTOList;
                if (paymentModeDTOList != null && paymentModeDTOList.Any())
                {
                    PaymentModeDTO paymentModeDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                    if (paymentModeDTO != null)
                    {
                        using (frmCardTransaction frm = new frmCardTransaction("I", _productRow, null, customerDTO, paymentModeDTO, CardCount, selectedEntitlementType, selectedLoyaltyCardNo))
                        {
                            frm.ShowDialog();
                        }
                    }
                }
                Close();
                log.LogMethodExit();
                return;
            }
            else
            {
                frmPaymentMode frpm = new frmPaymentMode("I", _productRow, null, customerDTO, selectedEntitlementType, CardCount);
                DialogResult dr = frpm.ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                {
                    DialogResult = dr;
                    this.Close();
                    log.LogMethodExit();
                    return;
                }
            }
            //timeout.Start();
            StartKioskTimer();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
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
                this.lblPoints.ForeColor = KioskStatic.CurrentTheme.CardCountScreenHeader2TextForeColor;//Points will be divided onto each card
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardCountScreenBackBtnTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.CardCountScreenCancelBtnTextForeColor;//Cancel button
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
