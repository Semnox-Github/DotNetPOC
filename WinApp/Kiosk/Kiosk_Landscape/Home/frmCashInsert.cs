/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmCashInsert.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        4-Sep-2019       Deeksha            Added logger methods.
*2.80        14-Nov-2019       Girish Kundar      Modified: As part of ticket printer integration
*2.90.0      30-Jun-2020       Dakshakh raj       Dynamic Payment Modes based on set up
*2.150.1     22-Feb-2023       Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.KioskCore.BillAcceptor;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk
{
    public partial class frmCashInsert : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KioskTransaction kioskTransaction; 
        private string amountFormatWithCurrencySymbol;
        private int noteDenominatorReceived;

        private CommonBillAcceptor billAcceptor;
        private string selectedEntitlementType = KioskTransaction.CREDITS_ENTITLEMENT;
        private Utilities Utilities = KioskStatic.Utilities; 
        private List<PaymentModeDTO> paymentModeDTOList = null; 
        
        public frmCashInsert(string entitlementType, int noteDenominatorReceived)
        {
            log.LogMethodEntry(entitlementType, noteDenominatorReceived);
            this.noteDenominatorReceived = noteDenominatorReceived;
            Utilities.setLanguage();
            InitializeComponent();
            Utilities.setLanguage(this);
            selectedEntitlementType = entitlementType;
            kioskTransaction = new KioskTransaction(KioskStatic.Utilities);
            kioskTransaction.AddCashNotePayment(noteDenominatorReceived);
            amountFormatWithCurrencySymbol = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL");

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme

            this.ShowInTaskbar = false;

            billAcceptor = (Application.OpenForms[0] as frmHome).commonBillAcceptor;
            billAcceptor.setReceiveAction = HandleBillAcceptorEvent;
            lblCashInserted.Text = kioskTransaction.GetCashPaymentAmount().ToString(amountFormatWithCurrencySymbol);            

            KioskStatic.logToFile("Entering direct cash insert...");
            log.LogMethodExit();
        }

        private void HandleBillAcceptorEvent(int noteDenominatorReceived)
        {
            log.LogMethodEntry(noteDenominatorReceived);
            kioskTransaction.AddCashNotePayment(noteDenominatorReceived);
            lblCashInserted.Text = kioskTransaction.GetCashPaymentAmount().ToString(amountFormatWithCurrencySymbol);
            log.LogMethodExit();
        }

        private void frmCashInsert_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry(e); 
                billAcceptor.setReceiveAction = null;

                KioskStatic.logToFile("Exiting direct cash insert...");

                Audio.Stop(); 
            log.LogMethodExit();
        }

        void initializeForm()
        {
            log.LogMethodEntry();
            try
            {
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;//Starts:Modification on 17-Dec-2015 for introducing new theme
                btnNewCard.BackgroundImage = ThemeManager.CurrentThemeImages.NewCardButtonSmall;
                btnRecharge.BackgroundImage = ThemeManager.CurrentThemeImages.LoadExistingImage;
                btnNewCard.Size = btnNewCard.BackgroundImage.Size;
                btnRecharge.Size = btnRecharge.BackgroundImage.Size; panel2.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnHome.Size = btnHome.BackgroundImage.Size;
                panelGrid.BackgroundImage = ThemeManager.CurrentThemeImages.RedeemTable;
                panel2.BackgroundImage = ThemeManager.CurrentThemeImages.TextEntryBox;
                // lblCashInserted.ForeColor = KioskStatic.CurrentTheme.TextForeColor;//Ends:Modification on 17-Dec-2015 for introducing new theme
                lblCashInserted.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing initializeForm()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmCashInsert_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;
            //Starts:Modification on 17-Dec-2015 for introducing new theme
            //    lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;//NewTheme102015
            //    lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;
            //Ends:Modification on 17-Dec-2015 for introducing new theme
            Application.DoEvents();

            KioskStatic.logToFile("Enter CashInsert screen");
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private void btnNewCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnNewCard_Click");
            try
            {


                billAcceptor.Stop();
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    KioskStatic.logToFile("TIME_IN_MINUTES_PER_CREDIT is greater than 0");
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                    }
                    else
                    {
                        using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                        {
                            if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                frmEntitle.Dispose();
                                log.LogMethodExit();
                                return;
                            }
                            selectedEntitlementType = frmEntitle.selectedEntitlement;
                            frmEntitle.Dispose();
                        }
                    }
                }
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                bool autoGeneratedCardNumber = false;
                DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["price"] = kioskTransaction.GetTotalPaymentsReceived();//billAcceptor.GetAcceptance().totalValue;
                    dt.Rows[0]["CardCount"] = 2; // more than 1 to specify that MultipleCardsInSingleProduct to true in frmCardTransaction
                    autoGeneratedCardNumber = dt.Rows[0]["AutoGenerateCardNumber"].ToString() == "N" || dt.Rows[0]["AutoGenerateCardNumber"] == DBNull.Value ? false : true;
                }
                else
                {
                    KioskStatic.logToFile("uanble to fetch record for variable product id: " + KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId.ToString());
                    throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1669));
                }

                if (KioskStatic.config.dispport == -1 && autoGeneratedCardNumber == false)
                {
                    log.Info("Card dispenser is disabled , dispport == -1");
                    KioskStatic.logToFile("Card dispenser is disabled , dispport == -1");
                    frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(2384));
                    f.ShowDialog();
                    KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                    return;
                }
                paymentModeDTOList = KioskStatic.paymentModeDTOList;
                 if (kioskTransaction.GetTotalPaymentsReceived() < 2 && paymentModeDTOList != null && paymentModeDTOList.Any())
                {
                    PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                    kioskTransaction = KioskHelper.AddNewCardProduct(kioskTransaction, dt.Rows[0], 1, selectedEntitlementType);
                    using (frmCardTransaction frm = new frmCardTransaction(kioskTransaction, paymentModeCDTO))
                    {
                        frm.ShowDialog();
                        kioskTransaction = frm.GetKioskTransaction;
                    }
                }
                else
                {
                    using (Home.frmCardCountBasic ccb = new Home.frmCardCountBasic(kioskTransaction, dt.Rows[0], kioskTransaction.GetTotalPaymentsReceived() >= 10 ? 3 : 2, selectedEntitlementType))
                    {
                        ccb.ShowDialog();
                        kioskTransaction = ccb.GetKioskTransaction;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                (new frmOKMsg(ex.Message)).ShowDialog();
            }
            finally
            {
                if (kioskTransaction.GetTotalPaymentsReceived() <= 0)
                    Close();
                else
                {
                    btnRecharge.Enabled = btnNewCard.Enabled = true;
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }

        private void btnRecharge_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                billAcceptor.Stop();
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                Audio.PlayAudio(Audio.RedeemTokenTapCard);
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = KioskTransaction.TIME_ENTITLEMENT;
                    }
                    else
                    {
                        using (frmEntitlement frmEntitle = new frmEntitlement(KioskStatic.Utilities.MessageUtils.getMessage(1345)))
                        {
                            if (frmEntitle.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                frmEntitle.Dispose();
                                log.LogMethodExit();
                                return;
                            }
                            selectedEntitlementType = frmEntitle.selectedEntitlement;
                            frmEntitle.Dispose();
                        }
                    }
                }
                using (frmTapCard ftc = new frmTapCard())
                {
                    DialogResult dr = ftc.ShowDialog();
                    Audio.Stop();

                    if (ftc.Card != null) // valid card tapped
                    {
                        if (ftc.Card.technician_card.Equals('Y'))
                        {
                            (new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(197, ftc.Card.CardNumber))).ShowDialog();
                            return;
                        }
                        DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                        if (dt.Rows.Count > 0)
                        {
                            dt.Rows[0]["price"] = kioskTransaction.GetTotalPaymentsReceived(); //billAcceptor.GetAcceptance().totalValue;
                        }
                        else
                        {
                            KioskStatic.logToFile("uanble to fetch record for variable product id: " + KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId.ToString());
                            throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1669));
                        }

                        paymentModeDTOList = KioskStatic.paymentModeDTOList;
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true); 
                            kioskTransaction = KioskHelper.AddRechargeCardProduct(kioskTransaction, ftc.Card, dt.Rows[0], 1, selectedEntitlementType);
                            using (frmCardTransaction fct = new frmCardTransaction(kioskTransaction, paymentModeCDTO))
                            {
                                fct.ShowDialog();
                                kioskTransaction = fct.GetKioskTransaction;
                            }
                        }
                    }
                    else // time out
                    {
                        ftc.Dispose();
                        log.LogMethodExit();
                        return;
                    }

                    ftc.Dispose();
                }
            }
            catch (Exception ex)
            {
                (new frmOKMsg(ex.Message)).ShowDialog();
            }
            finally
            {
                if (kioskTransaction.GetTotalPaymentsReceived() <= 0)
                    Close();
                else
                {
                    btnRecharge.Enabled = btnNewCard.Enabled = true;
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }

        private void btnHome_Click(object sender, EventArgs e)//Starts:Modification on 17-Dec-2015 for introducing new theme
        {
            log.LogMethodEntry();
            bool formClosed = false;
            try
            {
                billAcceptor.Stop();
                btnRecharge.Enabled = btnNewCard.Enabled = false; 
                if (billAcceptor != null)
                {
                    BaseForm.DirectCashAbortAction(kioskTransaction);
                }
                DialogResult = System.Windows.Forms.DialogResult.Cancel; 
                for (int i = Application.OpenForms.Count - 1; i > 0; i--)
                {
                    Application.OpenForms[i].Visible = false;
                    Application.OpenForms[i].Close();
                    formClosed = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("In direct cash form. Cannot go back to home page due to " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            finally
            {
                if (formClosed == false)
                {
                    btnRecharge.Enabled = btnNewCard.Enabled = true;
                    billAcceptor.Start();
                }
            }
            log.LogMethodExit();
        }
    }
}

