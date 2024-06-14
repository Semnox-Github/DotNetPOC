/********************************************************************************************
 * Project Name - Customer
 * Description  - frmCardCountBasic.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80         3-Sep-2019      Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
 *2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
 ********************************************************************************************/
using System;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk.Home
{
    public partial class frmCardCountBasic : BaseFormKiosk
    {
        //Timer timeout = new Timer();
        DataRow _productRow;
        CommonBillAcceptor billAcceptor;//Added 7-Dec-2017
        
        string selectedEntitlementType;
        string selectedLoyaltyCardNo = "";
        public frmCardCountBasic(DataRow productRow, int maxCards, string entitlementType)
        {
            log.LogMethodEntry(productRow, maxCards, entitlementType);
            KioskStatic.logToFile("frmCardCountBasic:Constructor");
            InitializeComponent();
            KioskStatic.setDefaultFont(this);

            _productRow = productRow;
            selectedEntitlementType = entitlementType;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.BackgroundImage = KioskStatic.CurrentTheme.CardCountBackgroundImage;
            WindowState = FormWindowState.Maximized;

            if (maxCards == 0)
                maxCards = 3;

            btnOne.Visible = true;
            if (maxCards > 1)
                btnTwo.Visible = true;

            if (maxCards > 2)
                btnThree.Visible = true;

            KioskStatic.Utilities.setLanguage(this);

            billAcceptor = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;//Added 7-Dec-2017
            lblHeading.Text = KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + billAcceptor.GetAcceptance().totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + lblHeading.Text;
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        //Begin Timer Cleanup
        //protected override CreateParams CreateParams
        //{
        //    //this method is used to avoid the table layout flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //}
        //End Timer Cleanup

        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmCardCount_Load");
            lblSiteName.Text = KioskStatic.SiteHeading;
            //KioskStatic.setFieldLabelForeColor(lblHeading);

            if (selectedEntitlementType == "Time")//Start:Added 7-Dec-2017
                lblPoints.Text = KioskStatic.GetCreditsOnSplitVariableProduct((double)billAcceptor.GetAcceptance().totalValue, selectedEntitlementType, null).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + KioskStatic.Utilities.MessageUtils.getMessage("MINUTES");
            else
                lblPoints.Text = KioskStatic.GetCreditsOnSplitVariableProduct((double)billAcceptor.GetAcceptance().totalValue, selectedEntitlementType, null).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) + " " + lblPoints.Text;
            //End:Added 7-Dec-2017
            //timeout.Interval = 1000;
            //timeout.Tick += timeout_Tick;
            //timeout.Start();
            displaybtnCancel(false);
            KioskStatic.logToFile("frmCardCount_Load:Closed");
            log.LogMethodExit();
        }

        //int secondsRemaining = 30;
        //void timeout_Tick(object sender, EventArgs e)
        //{
        //    secondsRemaining--;
        //    if (secondsRemaining == 10)
        //    {
        //        if (TimeOut.AbortTimeOut(this))
        //        {
        //            secondsRemaining = 30;
        //        }
        //        else
        //            secondsRemaining = 0;
        //    }

        //    if (secondsRemaining <= 0)
        //    {
        //        DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //        Close();
        //    }
        //}

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
            Button b = sender as Button;
            int CardCount = 1;
            if (b.Name.Equals("btnOne"))
                CardCount = 1;
            else
                CardCount = 2;

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
            //timeout.Stop();
            log.LogMethodEntry(CardCount);
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
            }

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
            List<PaymentModeDTO> paymentModeDTOList = null;
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

            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnThree_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //timeout.Stop();
            StopKioskTimer();
            frmCardCount frm = new frmCardCount(_productRow, selectedEntitlementType, 6);
            DialogResult dr = frm.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.No) // back button
            {
                //secondsRemaining = 30;
                //timeout.Start();
                ResetKioskTimer();
                StartKioskTimer();
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader1TextForeColor;//(GIVES YOU)
                this.lblPoints.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader2TextForeColor;//(POINTS)
                this.label1.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsHeader3TextForeColor;//(Please choose an option from below) 
                this.btnOne.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnOneTextForeColor;
                this.btnTwo.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnTwoTextForeColor;
                this.btnThree.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnThreeTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CardCountBasicsBtnPrevTextForeColor;
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
