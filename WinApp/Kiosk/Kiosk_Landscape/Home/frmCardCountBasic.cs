/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCardCountBasic.cs
* 
**************
**Version Log
**************
*Version        Date             Modified By        Remarks          
*********************************************************************************************
 *2.80          4-Sep-2019       Deeksha            Added logger methods.
 *2.90.0        30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
 *2.150.1       22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Core.Utilities;

namespace Parafait_Kiosk.Home
{
    public partial class frmCardCountBasic : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Timer timeout = new Timer();
        DataRow _productRow;
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        Semnox.Parafait.KioskCore.BillAcceptor.CommonBillAcceptor billAcceptor;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmCardCountBasic(KioskTransaction kioskTransaction, DataRow productRow, int maxCards, string entitlementType)
        {
            log.LogMethodEntry(productRow, maxCards, entitlementType);
            InitializeComponent();
            selectedEntitlementType = entitlementType;

            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme

            _productRow = productRow;
            this.kioskTransaction = kioskTransaction;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            btnOne.BackgroundImage = ThemeManager.CurrentThemeImages.NoOfCardButton;
            if (ThemeManager.CurrentThemeImages.NoOfCardButton != null)
                btnOne.Size = ThemeManager.CurrentThemeImages.NoOfCardButton.Size;
            btnTwo.BackgroundImage = btnThree.BackgroundImage = ThemeManager.CurrentThemeImages.PassesTwo;
            if (ThemeManager.CurrentThemeImages.PassesTwo != null)
                btnTwo.Size = btnThree.Size = ThemeManager.CurrentThemeImages.PassesTwo.Size;
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Ends:Modification on 17-Dec-2015 for introducing new theme
            WindowState = FormWindowState.Maximized;

            if (maxCards == 0)
                maxCards = 3;

            btnOne.Visible = true;
            if (maxCards > 1)
                btnTwo.Visible = true;

            if (maxCards > 2)
                btnThree.Visible = true;

            KioskStatic.Utilities.setLanguage(this);

            billAcceptor = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            string amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");
            decimal insertedValue = kioskTransaction.GetTotalPaymentsReceived();

            lblHeading.Text = KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + insertedValue.ToString("N0") + " " + lblHeading.Text;

            btnOne.Text = ThemeManager.InsertNewline(btnOne.Text, 13);//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnTwo.Text = ThemeManager.InsertNewline(btnTwo.Text, 13);
            btnThree.Text = ThemeManager.InsertNewline(btnThree.Text, 13);//Ends:Modification on 17-Dec-2015 for introducing new theme
            log.LogMethodExit();
        }


        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblSiteName.Text = KioskStatic.SiteHeading;
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
        {//Starts:Modification on 17-Dec-2015 for introducing new theme
         // Button b = sender as Button;
         // b.BackgroundImage = Properties.Resources.CardCountPressed;
         // btnOne.BackgroundImage = Properties.Resources.CardCountNormal;
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {//Starts:Modification on 17-Dec-2015 for introducing new theme
         // Button b = sender as Button;
         // b.BackgroundImage = Properties.Resources.CardCountNormal;
         // btnOne.BackgroundImage = Properties.Resources.CardCountPressed;
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            int CardCount = 1;
            if (b.Name.Equals("btnOne"))
                CardCount = 1;
            else
                CardCount = 2;
            log.LogVariableState("CardCount", CardCount);
            callTransactionForm(CardCount);
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        void callTransactionForm(int CardCount)
        {
            log.LogMethodEntry(CardCount);
            //timeout.Stop();
            StopKioskTimer();
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
                if (customerDTO != null)
                {
                    kioskTransaction.SetTransactionCustomer(customerDTO);
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
                    kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                    frmChooseProduct.AlertUserIfWaiverSigningIsRequired(productId, kioskTransaction);
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
        private void btnThree_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
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
            log.LogMethodExit();
        }
    }
}
