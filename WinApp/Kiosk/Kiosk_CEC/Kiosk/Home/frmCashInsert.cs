/********************************************************************************************
 * Project Name - Customer
 * Description  - frmCashInsert.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By      Remarks          
 *********************************************************************************************
 *2.70.3      3-Sep-2019      Deeksha          Added logger methods.
 *2.70.3     14-Nov-2019      Girish Kundar    Modified: As part of ticket printer integration
 *2.70.3     30-Jan-2020      Archana          Modified: btnNewCard_Click() to send card count as 1 during direct cash
 *2.80.1     02-Feb-2021      Deeksha          Theme changes to support customized Images/Font
 *2.90.0     30-Jun-2020      Dakshakh raj     Dynamic Payment Modes based on set up
 *2.140.0    22-Oct-2021      Sathyavathi      CEC enhancement - Fund Raiser and Donations
 *********************************************************************************************/
using Semnox.Parafait.KioskCore.BillAcceptor;
using System;
using System.Data;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Product;
using Semnox.Parafait.Category;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;

namespace Parafait_Kiosk
{
    public partial class frmCashInsert : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        CommonBillAcceptor billAcceptor;

        string selectedEntitlementType;//Added 7-Dec-2017
        string selectedLoyaltyCardNo = "";

        Utilities Utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        ParafaitEnv ParafaitEnv = KioskStatic.Utilities.ParafaitEnv;

        List<PaymentModeDTO> paymentModeDTOList = null;
        private Semnox.Parafait.Transaction.Transaction _currentTrx;
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsList = new List<KeyValuePair<string, ProductsDTO>>();

        public frmCashInsert(string entitlementType)
        {
            log.LogMethodEntry(entitlementType);
            Utilities.setLanguage();
            InitializeComponent();
            initializeForm();
            Utilities.setLanguage(this);
            selectedEntitlementType = entitlementType;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            this.ShowInTaskbar = false;

            billAcceptor = (Application.OpenForms[0] as FSKCoverPage).commonBillAcceptor;
            billAcceptor.setReceiveAction = handleBillAcceptorEvent;

            lblCashInserted.Text = ParafaitEnv.CURRENCY_SYMBOL + billAcceptor.GetAcceptance().totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            KioskStatic.logToFile("Entering direct cash insert...");
            log.LogMethodExit();
        }
        void handleBillAcceptorEvent(KioskStatic.acceptance ac)
        {
            log.LogMethodEntry(ac);
            lblCashInserted.Text = ParafaitEnv.CURRENCY_SYMBOL + ac.totalValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            log.LogMethodExit();
        }

        private void frmCashInsert_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            log.LogMethodEntry();
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
                this.BackgroundImage = KioskStatic.CurrentTheme.DefaultBackgroundImage;
                btnNewCard.BackgroundImage = KioskStatic.CurrentTheme.NewPlayPassButtonSmall;
                btnRecharge.BackgroundImage = KioskStatic.CurrentTheme.RechargePlayPassButtonSmall;
            }
            catch { }
            log.LogMethodExit();
        }

        private void frmCashInsert_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            initializeForm();

            lblSiteName.Text = KioskStatic.SiteHeading;

            lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            lblGreeting1.ForeColor = KioskStatic.CurrentTheme.ScreenHeadingForeColor;

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
                log.LogMethodExit(CP);
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
                //Added:Added 7-Dec-2017 for time_split Entitlement Changes
                if (KioskStatic.TIME_IN_MINUTES_PER_CREDIT > 0)//Checks whether credits to time transfer concepts is used
                {
                    KioskStatic.logToFile("TIME_IN_MINUTES_PER_CREDIT is greater than 0");
                    if (KioskHelper.isTimeEnabledStore() == true)
                    {
                        selectedEntitlementType = "Time";
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
                }//End:Added 7-Dec-2017
                btnRecharge.Enabled = btnNewCard.Enabled = false;
                bool autoGeneratedCardNumber = false;
                DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[0]["price"] = billAcceptor.GetAcceptance().totalValue;
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
                    using (frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(2384)))
                    {
                        f.ShowDialog();
                    }
                    KioskStatic.logToFile("Card dispenser is disabled and product with auto generated card number set to Y is exists");
                    return;
                }


                //if (billAcceptor.GetAcceptance().totalValue < 2) // less than 2$ inserted, go to transaction with cardcount = 1
                //{
                if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_ALOHA_LOYALTY_INTERFACE") == "Y")
                {
                    using (frmLoyalty fm = new frmLoyalty())
                    {
                        if (fm.DialogResult != DialogResult.Cancel)
                        {
                            if (fm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                selectedLoyaltyCardNo = fm.selectedLoyaltyCardNo;
                        }
                        if (fm != null)
                            fm.Dispose();
                    }
                }
                paymentModeDTOList = KioskStatic.paymentModeDTOList;
                if (paymentModeDTOList != null && paymentModeDTOList.Any()) // less than 2$ inserted, go to transaction with cardcount = 1
                {
                    PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                    selectedFundsList = GetSelectedFundsList();
                    CreateTrxLinesForFundProduct(null);
                    using (frmCardTransaction frm = new frmCardTransaction("I", dt.Rows[0], null, null, paymentModeCDTO, 1, selectedEntitlementType, selectedLoyaltyCardNo, null, null, selectedFundsList, _currentTrx))
                    {
                        frm.ShowDialog();
                    }
                }
                //}
                //else
                //{
                //    KioskStatic.logToFile("before calling frCardCountBasic");
                //    Home.frmCardCountBasic ccb = new Home.frmCardCountBasic(dt.Rows[0], billAcceptor.GetAcceptance().totalValue >= 10 ? 3 : 2, selectedEntitlementType);
                //    ccb.ShowDialog();
                //}
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg fom = new frmOKMsg(ex.Message))
                {
                    fom.ShowDialog();
                }
                log.Error(ex.Message);
            }
            finally
            {
                if (billAcceptor.GetAcceptance().totalValue <= 0)
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
                        selectedEntitlementType = "Time";
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
                            using (frmOKMsg fom = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(197, ftc.Card.CardNumber)))
                            {
                                fom.ShowDialog();
                            }
                            log.LogMethodExit();
                            return;
                        }
                        DataTable dt = KioskStatic.getProductDetails(KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId);
                        if (dt.Rows.Count > 0)
                        {
                            dt.Rows[0]["price"] = billAcceptor.GetAcceptance().totalValue;
                        }
                        else
                        {
                            KioskStatic.logToFile("unable to fetch record for variable product id: " + KioskStatic.Utilities.ParafaitEnv.VariableRechargeProductId.ToString());
                            throw new Exception(KioskStatic.Utilities.MessageUtils.getMessage(1669));
                        }
                        if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_ALOHA_LOYALTY_INTERFACE") == "Y")
                        {
                            using (frmLoyalty frm = new frmLoyalty())
                            {
                                if (frm.DialogResult != DialogResult.Cancel)
                                {
                                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                        selectedLoyaltyCardNo = frm.selectedLoyaltyCardNo;
                                }
                                if (frm != null)
                                    frm.Dispose();
                            }
                        }

                        paymentModeDTOList = KioskStatic.paymentModeDTOList;
                        if (paymentModeDTOList != null && paymentModeDTOList.Any())
                        {
                            PaymentModeDTO paymentModeCDTO = paymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                            selectedFundsList = GetSelectedFundsList();
                            CreateTrxLinesForFundProduct(ftc.Card);
                            using (frmCardTransaction fct = new frmCardTransaction("R", dt.Rows[0], ftc.Card, null, paymentModeCDTO, 1, selectedEntitlementType, selectedLoyaltyCardNo, null, null, selectedFundsList, _currentTrx))
                            {
                                fct.ShowDialog();
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
                log.Error(ex.Message);
            }
            finally
            {
                if (billAcceptor.GetAcceptance().totalValue <= 0)
                    Close();
                else
                    btnRecharge.Enabled = btnNewCard.Enabled = true;
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader1TextForeColor;//( Insert more cash) - #CashInsertScreenHeader1ForeColor
                this.label1.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader2TextForeColor;//(Cash Inserted Header) - #CashInsertScreenHeader2ForeColor
                this.panel2.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenCashInsertedInfoTextForeColor;//(Cash Inserted Info) - #CashInsertScreenCashInsertedInfoForeColor
                this.btnNewCard.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenBtnNewCardTextForeColor;//#CashInsertScreenBtnNewCardForeColor
                this.btnRecharge.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenBtnRechargeTextForeColor;//# CashInsertScreenBtnRechargeForeColor
                this.label5.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenHeader3TextForeColor;//(more cash at any time) - #CashInsertScreenHeader3ForeColor
                this.label2.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenDenominationHeaderTextForeColor;//(Header for Denomination) -#CashInsertScreenDenominationHeaderForeColor
                this.label3.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenQuantityHeaderTextForeColor;//(Header for Quantity) -#CashInsertScreenQuantityHeaderForeColor
                this.label4.ForeColor = KioskStatic.CurrentTheme.CashInsertScreenPointsHeaderTextForeColor;//(Header for points) -#CashInsertScreenPointsHeaderForeColor
                lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets active Fund products DTO.
        /// </summary>
        /// <param name=""></param> None
        /// <returns = List<KeyValuePair<string, ProductsDTO>> </returns> dictionary holding the fund products DTO List
        /// 
        private List<KeyValuePair<string, ProductsDTO>> GetSelectedFundsList()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Fetching user selected fund products");
            List<KeyValuePair<string, ProductsDTO>> selectedFundsList = new List<KeyValuePair<string, ProductsDTO>>();
            try
            {
                List<ProductsDTO> activeFunds = KioskHelper.GetActiveFundRaiserProducts(KioskStatic.Utilities.ExecutionContext);
                if (activeFunds != null && activeFunds.Any())
                {
                    frmFundRaiserAndDonation frmFandD = new frmFundRaiserAndDonation(activeFunds, true);
                    if (frmFandD.ShowDialog() == DialogResult.OK)
                    {
                        for (int i = 0; i < frmFandD.selectedProductsDTOList.Count; i++)
                        {
                            selectedFundsList.Add(new KeyValuePair<string, ProductsDTO>(KioskStatic.FUND_RAISER_LOOKUP_VALUE, frmFandD.selectedProductsDTOList[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while fetching getting selected fund products : " + ex.Message);
            }

            log.LogMethodExit();
            return selectedFundsList;
        }

        /// <summary>
        /// Creates Transaction Line for the fund/s product selected 
        /// </summary>
        /// <param name="rechargeCard"></param>
        private void CreateTrxLinesForFundProduct(Card rechargeCard = null)
        {
            log.LogMethodEntry();
            try
            {
                if (_currentTrx == null)
                {
                    _currentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                    _currentTrx.PaymentReference = "Kiosk Transaction";
                    KioskStatic.logToFile("New Trx object created for fund raiser product");
                }
                string message = "";
                if (selectedFundsList != null && selectedFundsList.Any())
                {
                    foreach (KeyValuePair<string, ProductsDTO> keyValuePair in selectedFundsList)
                    {
                        if (_currentTrx.createTransactionLine(rechargeCard == null ? null : rechargeCard, keyValuePair.Value.ProductId, Convert.ToDouble(keyValuePair.Value.Price), 1, ref message) != 0)
                        {
                            message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Error") + ": " + message;
                            KioskStatic.logToFile("Error Creating TrxLine for fund raiser: " + message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Exception occured while Creating TrxLine for fund raiser: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
