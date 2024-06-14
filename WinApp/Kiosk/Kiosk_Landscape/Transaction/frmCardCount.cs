/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCardCount.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        5-Sep-2019       Deeksha            Added logger methods.
* 2.90.0      30-Jun-2020      Dakshakh raj       Dynamic Payment Modes based on set up
* 2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System.Data;
using System.Windows.Forms;
using Semnox.Parafait.Device.PaymentGateway;
using System.Collections.Generic;
using System.Linq;

namespace Parafait_Kiosk
{
    public partial class frmCardCount : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Timer timeout = new Timer();
        int _MaxCards = 0;
        DataRow _productRow;
        string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        private List<PaymentModeDTO> paymentModeDTOList = null;
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        public frmCardCount(KioskTransaction kioskTransaction, DataRow productRow, string entitlementType, int maxCards = 0)
        {
            log.LogMethodEntry("kioskTransaction", productRow, entitlementType, maxCards);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            selectedEntitlementType = entitlementType;
            this.kioskTransaction = kioskTransaction;


            KioskStatic.Utilities.setLanguage(this);

            _productRow = productRow;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnHome.Size = btnHome.BackgroundImage.Size;
            btnOne.BackgroundImage = ThemeManager.CurrentThemeImages.NoOfCardButton;
            btnTwo.BackgroundImage = btnThree.BackgroundImage = btnFour.BackgroundImage =
            btnFive.BackgroundImage = btnSix.BackgroundImage = ThemeManager.CurrentThemeImages.PassesTwo;
            //btnPrev.BackgroundImage = btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;//Ends:Modification on 17-Dec-2015 for introducing new theme
            WindowState = FormWindowState.Maximized;

            _MaxCards = maxCards;
            if (_MaxCards == 0)
                _MaxCards = 6;

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
            log.LogMethodExit();
        }


        private void frmCardCount_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmCardCount_Load");
            lblSiteName.Text = KioskStatic.SiteHeading;
            //KioskStatic.setFieldLabelForeColor(lblHeading);//Starts:Modification on 17-Dec-2015 for introducing new theme
            //btnOne.BackgroundImage = Properties.Resources.CardCountPressed;//Ends:Modification on 17-Dec-2015 for introducing new theme

            //timeout.Interval = 1000;
            //timeout.Tick += timeout_Tick;
            //timeout.Start();

            btnPrev.Top = lblSiteName.Bottom + 20;//playpass:Starts

            if (_MaxCards < 3)
                flpCardCount.Width = flpCardCount.Width * 2 / 3;
            flpCardCount.Left = (this.Width / 2) - (flpCardCount.Width / 2);
            flpCardCount.Top = (this.Height / 2) - (flpCardCount.Height / 2);
            log.LogMethodExit();
        }

        private void frmCardCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("frmCardCount_FormClosing");
            // timeout.Stop();
            log.LogMethodExit();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            //Button b = sender as Button;//NewTheme102015:
            //b.BackgroundImage = Properties.Resources.CardCountPressed;
            //btnOne.BackgroundImage = Properties.Resources.CardCountNormal;
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btn_MouseUp(object sender, MouseEventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            //Button b = sender as Button;//NewTheme102015:
            //b.BackgroundImage = Properties.Resources.CardCountNormal;
            //btnOne.BackgroundImage = Properties.Resources.CardCountPressed;
        }//Ends:Modification on 17-Dec-2015 for introducing new theme

        private void btn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Button b = sender as Button;
            int CardCount = Convert.ToInt32(b.Text);
            log.LogVariableState("CardCount", CardCount);
            callTransactionForm(CardCount);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        void callTransactionForm(int CardCount)
        {
            log.LogMethodEntry(CardCount);
            KioskStatic.logToFile(System.Reflection.MethodBase.GetCurrentMethod().Name);

            //timeout.Stop();
            //secondsRemaining = 30;
            StopKioskTimer();
            ResetKioskTimer();
            CustomerDTO customerDTO = null;
            if (kioskTransaction.HasCustomerRecord() == false)
            {
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
            }
            int productId = Convert.ToInt32(_productRow["product_id"]);
            CommonBillAcceptor commonBA = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            if (commonBA != null && kioskTransaction.GetTotalPaymentsReceived() > 0)
            {
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
                }
                Close();
                log.LogMethodExit();
                return;
            }
            else
            {
                kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, _productRow, CardCount, selectedEntitlementType);
                kioskTransaction.SelectedProductType = KioskTransaction.GETNEWCARDTYPE;
                frmChooseProduct.AlertUserIfWaiverSigningIsRequired(productId, kioskTransaction);
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
            StartKioskTimer();
            log.LogMethodExit();
        }

        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile(System.Reflection.MethodBase.GetCurrentMethod().Name);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            for (int i = Application.OpenForms.Count - 1; i > 0; i--)
            {
                Application.OpenForms[i].Close();
            }
            log.LogMethodExit();
        }
    }
}
