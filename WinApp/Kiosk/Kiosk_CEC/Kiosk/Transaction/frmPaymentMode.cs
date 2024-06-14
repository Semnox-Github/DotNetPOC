/********************************************************************************************
* Project Name - Parafait_Kiosk.frmPaymentMode
* Description  - frmPaymentMode 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Tax display
*2.80        4-Sep-2019       Deeksha            Added logger methods.
*2.80.1      10-Nov-2020      Deeksha            Modified to display entitlement information message for payment mode / transaction screen
 *2.80.1     02-Feb-2021      Deeksha            Theme changes to support customized Images/Font
*2.90.0      23-Jun-2020      Dakshakh raj       Payment Modes based on Kiosk Configuration set up
*2.100.0     27-Oct-2020      Dakshakh Raj       Payment Buttons image issue fix
*2.110.0     09-Dec-2020      Mushahid Faizan    Handled execution Context for Tax
*2.130.0     30-Jun-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.140.0     22-Oct-2021      Sathyavathi        CEC enhancement - Fund Raiser and Donations
*2.150.0.0   13-Oct-2022      Sathyavathi        Mask card number
********************************************************************************************/
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.KioskCore;
using System.Text.RegularExpressions;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;
using System.Data.SqlClient;
using Parafait_Kiosk.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmPaymentMode : BaseFormKiosk
    {
        //Timer timeout = new Timer();
        string _pFunction; DataRow _prowProduct; Card _rechargeCard; CustomerDTO customerDTO; int _cardCount;
        string selectedEntitlementType = "Credits";
        string selectedLoyaltyCardNo = "";
        DiscountsSummaryDTO discountSummaryDTO = null;
        public string couponNumber = "";
        List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = null;
        //SqlTransaction sqlTransaction;
        ExecutionContext executionContext = KioskStatic.Utilities.ExecutionContext;
        private List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = new List<KeyValuePair<string, ProductsDTO>>();
        private Semnox.Parafait.Transaction.Transaction _currentTrx;

        public frmPaymentMode(string pFunction, DataRow prowProduct, Card rechargeCard, CustomerDTO customerDTO, string entitlementType, int CardCount = 1, Semnox.Parafait.Transaction.Transaction inTrx = null)
        {
            log.LogMethodEntry(pFunction, prowProduct, rechargeCard, customerDTO, entitlementType, CardCount);
            _pFunction = pFunction;
            _rechargeCard = rechargeCard;
            _prowProduct = prowProduct;
            this.customerDTO = customerDTO;
            _cardCount = CardCount == 0 ? 1 : CardCount;
            _currentTrx = inTrx;

            InitializeComponent();
            this.BackgroundImage = KioskStatic.CurrentTheme.PaymentModeBackgroundImage;
            KioskStatic.Utilities.setLanguage(this);
            selectedEntitlementType = entitlementType;

            SetBackgroundImages();
            KioskStatic.setDefaultFont(this);
            WindowState = FormWindowState.Maximized;


            // if multiple cards are there, then add deposit value
            decimal cardDeposits = 0.0M;
            decimal faceValue = 0.0M;
            // get depot product details depositTaxAmount && depositPriceWithOutTax
            decimal depositTaxAmount = 0;
            decimal depositPriceWithOutTax = faceValue;
            try
            {
                faceValue = cardDeposits = Math.Round(Convert.ToDecimal(prowProduct["face_value"] == DBNull.Value ? 0 : prowProduct["face_value"]), 4, MidpointRounding.AwayFromZero);

                if (pFunction.Equals("R"))
                {
                    // deposit will be zero on recharge
                    faceValue = cardDeposits = 0;
                }

                if (cardDeposits > 0)
                {
                    // if this is a "NEW" card sale or gametime product
                    // the deposit is not a part of the product component and should be added to it
                    if (pFunction.Equals("I") && (prowProduct["product_type"].ToString().Equals("CARDSALE") || prowProduct["product_type"].ToString().Equals("GAMETIME")))
                    {
                        decimal tempPrice = Math.Round(Convert.ToDecimal(prowProduct["price"] == DBNull.Value ? 0 : prowProduct["price"]), 4, MidpointRounding.AwayFromZero);
                        decimal tempPPrice = Math.Round(Convert.ToDecimal(prowProduct["ProductPrice"] == DBNull.Value ? 0 : prowProduct["ProductPrice"]), 4, MidpointRounding.AwayFromZero);
                        prowProduct["price"] = tempPrice + cardDeposits;
                        prowProduct["ProductPrice"] = tempPPrice + cardDeposits;
                    }

                    if (CardCount > 1)
                    {
                        cardDeposits = cardDeposits * (_cardCount - 1);
                    }

                    ProductsDTO depositProduct = KioskStatic.GetDepositProduct();
                    if (depositProduct != null && depositProduct.Tax_id != -1)
                    {
                        Tax depositTax = new Tax(executionContext, depositProduct.Tax_id);
                        if (depositTax.GetTaxDTO() != null && depositTax.GetTaxDTO().TaxPercentage > 0)
                        {
                            if (depositProduct.TaxInclusivePrice.Equals("Y"))
                            {
                                depositPriceWithOutTax = Math.Round(Convert.ToDecimal(Convert.ToDouble(faceValue) / (1.0 + Convert.ToDouble(depositTax.GetTaxDTO().TaxPercentage) / 100.0)), 4, MidpointRounding.AwayFromZero);
                                depositTaxAmount = faceValue - depositPriceWithOutTax;
                            }
                            else
                            {
                                depositTaxAmount = Math.Round(Convert.ToDecimal((Convert.ToDouble(faceValue) * Convert.ToDouble(depositTax.GetTaxDTO().TaxPercentage)) / 100.0), 4, MidpointRounding.AwayFromZero);
                                depositPriceWithOutTax = faceValue;
                                faceValue += depositTaxAmount;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmPaymentMode()" + ex.Message);
            }


            decimal ProductPrice = Math.Round(Convert.ToDecimal(prowProduct["price"] == DBNull.Value ? 0 : prowProduct["price"]), 4, MidpointRounding.AwayFromZero);
            decimal productPriceWithOutTax = Math.Round(Convert.ToDecimal(prowProduct["ProductPrice"] == DBNull.Value ? 0 : prowProduct["ProductPrice"]), 4, MidpointRounding.AwayFromZero);
            bool multipleCardsInSingleProduct = (Convert.ToInt32(prowProduct["CardCount"]) > 1 ? true : false);
            decimal productTaxAmount = Math.Round(Convert.ToDecimal(prowProduct["taxAmount"] == DBNull.Value ? 0 : prowProduct["taxAmount"]), 4, MidpointRounding.AwayFromZero);
            string TaxInclusivePrice = prowProduct["TaxInclusivePrice"] == DBNull.Value ? "N" : prowProduct["TaxInclusivePrice"].ToString();
            Decimal taxPercentage = Math.Round(Convert.ToDecimal(prowProduct["taxPercentage"] == DBNull.Value ? 0 : prowProduct["taxPercentage"]), 4, MidpointRounding.AwayFromZero);

            // The tax calculated needs to be exclusive of the face value
            if (faceValue > 0)
            {
                if (TaxInclusivePrice.Equals("N"))
                {
                    productTaxAmount = Math.Round(((ProductPrice - faceValue) * (taxPercentage / 100)), 4, MidpointRounding.AwayFromZero);
                    productPriceWithOutTax = (ProductPrice - faceValue);
                }
                else
                {
                    productPriceWithOutTax = Math.Round(((ProductPrice - faceValue) / (1 + taxPercentage / 100)), 4, MidpointRounding.AwayFromZero);
                    productTaxAmount = (ProductPrice - faceValue - productPriceWithOutTax);
                }
            }

            if (productPriceWithOutTax == 0 && prowProduct["product_type"].ToString() == "VARIABLECARD")
            {
                productPriceWithOutTax = ProductPrice;
            }

            lblPackage.Text = KioskStatic.Utilities.ParafaitEnv.CURRENCY_SYMBOL + ProductPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT) +
                                Environment.NewLine + "(" +
                                prowProduct["product_name"].ToString() + ")";
            if (pFunction == "I")
            {
                lblCardnumber.Text = KioskStatic.Utilities.MessageUtils.getMessage("NEW");
            }
            else
            {
                lblCardnumber.Text = KioskHelper.GetMaskedCardNumber(rechargeCard.CardNumber);
            }
            DisplayValidtyMsg(Convert.ToInt32(_prowProduct["product_id"]));

            //int credits = 0;
            string message = KioskStatic.GetEntitlementsMessage(_prowProduct, CardCount, selectedEntitlementType,
                                                                multipleCardsInSingleProduct, _rechargeCard);
            //displayCreditsDetails(_prowProduct, CardCount);
            lblQuantity.Text = message;
            BuildToolTipForlblQuantity(message);

            //lblTotal.Text = ProductPrice.ToString("N2");
            bool qtyPromptCheck = (prowProduct["QuantityPrompt"].ToString().Equals("Y"));
            if (!multipleCardsInSingleProduct && qtyPromptCheck)
            {
                lblTotal.Text = ((ProductPrice - faceValue) * (CardCount == 0 ? 1 : CardCount)).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            }
            else
            {
                lblTotal.Text = (ProductPrice - faceValue).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            }
            lblPrice.Text = productPriceWithOutTax.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            lblTaxAmount.Text = productTaxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);

            // if (Convert.ToInt32(prowProduct["CardCount"]) > 1 && cardDeposits > 0.0M && _cardCount > 1)
            if (cardDeposits > 0.0M)
            {
                panelDeposit.Visible = true;
                lblDepositName.Text = String.IsNullOrEmpty(KioskStatic.GetDepositProduct().TranslatedProductName) ? KioskStatic.GetDepositProduct().ProductName : KioskStatic.GetDepositProduct().TranslatedProductName;
                lblDepositQty.Text = _cardCount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                lblDepositPrice.Text = faceValue.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                lblDepositTotal.Text = (faceValue * _cardCount).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                lblDepositTax.Text = depositTaxAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
            }
            else
            {
                panelDeposit.Visible = false;
            }
            if (_currentTrx != null)
            {
                lblTotalToPay.Text = _currentTrx.Net_Transaction_Amount.ToString(ParafaitDefaultContainerList.GetParafaitDefault(KioskStatic.Utilities.ExecutionContext, "AMOUNT_WITH_CURRENCY_SYMBOL"));
            }
            else
            {
                if (Convert.ToInt32(prowProduct["CardCount"]) <= 1 || _cardCount == 1)
                    lblTotalToPay.Text = (ProductPrice * _cardCount).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                else
                    lblTotalToPay.Text = (ProductPrice + cardDeposits).ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }
            //this.btnCash.Text = KioskStatic.Utilities.MessageUtils.getMessage("Cash") + Environment.NewLine +
            //KioskStatic.Utilities.MessageUtils.getMessage("No Change Made"); 
            log.LogMethodExit();
        }

        private void SetBackgroundImages()
        {
            log.LogMethodEntry();
            btnDiscount.BackgroundImage = KioskStatic.CurrentTheme.DiscountButton;
            //KioskStatic.setDefaultFont(this);
            //foreach (Control c in flowLayoutPanel1.Controls)
            //{
            //    string type = c.GetType().ToString().ToLower();
            //    if (type.Contains("label"))
            //    {
            //        c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            //    }
            //}
            //foreach (Control c in panel3.Controls)
            //{
            //    string type = c.GetType().ToString().ToLower();
            //    if (type.Contains("label"))
            //    {
            //        c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            //    }
            //}
            //foreach (Control c in flowLayoutPanel5.Controls)
            //{
            //    string type = c.GetType().ToString().ToLower();
            //    if (type.Contains("label"))
            //    {
            //        c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            //    }
            //}
            log.LogMethodExit();
        }

        private void frmPaymentMode_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            lblSiteName.Text = KioskStatic.SiteHeading;

            //timeout.Interval = 1000;
            //timeout.Tick += timeout_Tick;
            //timeout.Start();
            displaybtnCancel(true);
            panelDiscount.Visible = false;
            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_DISCOUNTS_IN_POS").Equals("N"))
            {
                btnDiscount.Text = KioskStatic.Utilities.MessageUtils.getMessage(btnDiscount.Text);
                btnDiscount.Visible = false;
                btnDiscount.Location = new Point(panelSummary.Bottom + 5, 255);
            }
            //PaymentGateway enhancement6/11/2020
            pOSPaymentModeInclusionDTOList = KioskStatic.pOSPaymentModeInclusionDTOList;
            List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
            string imageFolder = KioskStatic.Utilities.getParafaitDefaults("IMAGE_DIRECTORY");
            bool temp = true;
            bool found = false;

            if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Count > 0)
            {
                if (pOSPaymentModeInclusionDTOList.Exists(p => p.PaymentModeDTO.IsCash == true))
                {
                    if (KioskStatic.config.baport > 0)
                    {

                        foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Notes)
                        {
                            if (ai != null)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            foreach (KioskStatic.configuration.acceptorInfo ai in KioskStatic.config.Coins)
                            {
                                if (ai != null)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                        if (!found)
                        {
                            pOSPaymentModeInclusionDTOList.RemoveAll(p => p.PaymentModeDTO.IsCash == true);
                            if (pOSPaymentModeInclusionDTOList.Count == 1)
                            {
                                //txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(423);
                            }
                        }
                    }
                    else
                    {
                        pOSPaymentModeInclusionDTOList.RemoveAll(p => p.PaymentModeDTO.IsCash == true);
                    }
                    if (pOSPaymentModeInclusionDTOList.Count == 1 && found == false)
                    {
                        //txtMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(423);
                    }
                }
                if (KioskStatic.Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON") == "N" && pOSPaymentModeInclusionDTOList.Exists(p => p.PaymentModeDTO.IsDebitCard == true))
                {
                    pOSPaymentModeInclusionDTOList.RemoveAll(p => p.PaymentModeDTO.IsDebitCard == true);
                }
                if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any())
                {
                    foreach (POSPaymentModeInclusionDTO pOSPaymentModeInclusionDTO in pOSPaymentModeInclusionDTOList)
                    {
                        //Image plainImage = ThemeManager.CurrentThemeImages.CreditCardButton;//Modification on 17-Dec-2015 for introducing new theme
                        PaymentModeDTO paymentModeDTO = new PaymentModeDTO();
                        paymentModeDTO = pOSPaymentModeInclusionDTO.PaymentModeDTO;
                        Button btnPayment = new Button();
                        String btnText = "";
                        if (String.IsNullOrEmpty(pOSPaymentModeInclusionDTO.FriendlyName))
                        {
                            btnText = paymentModeDTO.PaymentMode;
                        }
                        else
                        {
                            btnText = pOSPaymentModeInclusionDTO.FriendlyName;
                        }
                        if (paymentModeDTO.IsCash)
                        {
                            btnPayment.Text = "Cash\r\nNo Change Made";
                        }
                        else
                        {
                            btnPayment.Text = btnText;
                        }

                        if (pOSPaymentModeInclusionDTOList.Count > 2 && pOSPaymentModeInclusionDTOList.Count % 2 == 0)
                        {
                            btnPayment.Margin = new System.Windows.Forms.Padding(15, 5, 5, 5);
                            btnPayment.Size = new System.Drawing.Size(480, 198);
                        }
                        else if (pOSPaymentModeInclusionDTOList.Count <= 2)
                        {
                            btnPayment.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                            btnPayment.Size = new System.Drawing.Size(1015, 197);
                        }
                        else
                        {
                            if (temp == true)
                            {
                                btnPayment.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                                btnPayment.Size = new System.Drawing.Size(1015, 197);
                                temp = false;
                            }
                            else
                            {
                                btnPayment.Margin = new System.Windows.Forms.Padding(15, 5, 5, 5);
                                btnPayment.Size = new System.Drawing.Size(480, 198);
                            }

                        }
                        if (String.IsNullOrEmpty(paymentModeDTO.ImageFileName))
                        {
                            if (pOSPaymentModeInclusionDTOList.Count <= 2)
                            {
                                btnPayment.BackgroundImage = KioskStatic.CurrentTheme.CashButtonSmall;
                            }
                            else
                            {
                                btnPayment.BackgroundImage = KioskStatic.CurrentTheme.CreditCardButtonBig;
                            }
                            btnPayment.Text = btnText;
                        }
                        else
                        {
                            try
                            {
                                object o = KioskStatic.Utilities.executeScalar("exec ReadBinaryDataFromFile @FileName",
                                                        new SqlParameter("@FileName", imageFolder + "\\" + paymentModeDTO.ImageFileName));

                                btnPayment.BackgroundImage = KioskStatic.Utilities.ConvertToImage(o);
                                btnText = "";
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                KioskStatic.logToFile(ex.Message + ": " + imageFolder + "\\" + paymentModeDTO.ImageFileName);
                                btnPayment.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.credit_debit_button;
                            }
                        }


                        btnPayment.BackColor = System.Drawing.Color.Transparent;
                        btnPayment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                        btnPayment.FlatAppearance.BorderColor = System.Drawing.Color.DarkSlateGray;
                        btnPayment.FlatAppearance.BorderSize = 0;
                        btnPayment.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
                        btnPayment.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
                        btnPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btnPayment.Font = new System.Drawing.Font(KioskStatic.CurrentTheme.DefaultFont.Name, 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        btnPayment.ForeColor = KioskStatic.CurrentTheme.PaymentModeButtonsTextForeColor;
                        btnPayment.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
                        //btn.Location = new System.Drawing.Point(10, 10);
                        //btn.Margin = new System.Windows.Forms.Padding(10, 10, 0, 10);
                        btnPayment.Name = "btn" + btnText;
                        //btn.Size = new System.Drawing.Size(1015, 197);
                        btnPayment.TabIndex = 12;
                        btnPayment.UseVisualStyleBackColor = false;
                        btnPayment.Tag = paymentModeDTO;
                        btnPayment.Click += new System.EventHandler(this.btnPayment_Click);
                        flpPaymentOptions.Controls.Add(btnPayment);
                    }
                }
            }
            else
            {
                //Takes care of default payment mode as cash
                System.IO.Ports.SerialPort spBillAcceptor = new System.IO.Ports.SerialPort();
                spBillAcceptor.PortName = "COM" + KioskStatic.config.baport.ToString();
                spBillAcceptor.BaudRate = 9600;
                spBillAcceptor.DataBits = 8;
                spBillAcceptor.StopBits = System.IO.Ports.StopBits.One;
                spBillAcceptor.Parity = System.IO.Ports.Parity.Even;
                spBillAcceptor.RtsEnable = true;
                spBillAcceptor.WriteTimeout = spBillAcceptor.ReadTimeout = 1000;
                try
                {
                    if (!spBillAcceptor.IsOpen)
                    {
                        spBillAcceptor.Open();
                        KioskStatic.logToFile("Bill acceptor port " + KioskStatic.config.baport.ToString() + " opened");
                    }
                    if (spBillAcceptor.IsOpen)
                    {
                        spBillAcceptor.Close();
                    }
                    //changes for kiosk dynamic Payemnet modes
                    List<PaymentModeDTO> cashPaymentModeDTOList = null;
                    cashPaymentModeDTOList = KioskStatic.paymentModeDTOList;
                    if (cashPaymentModeDTOList != null && cashPaymentModeDTOList.Any())
                    {
                        PaymentModeDTO paymentModeCDTO = cashPaymentModeDTOList.FirstOrDefault(p => p.IsCash == true);
                        if (paymentModeCDTO != null)
                        {
                            callTransactionForm(paymentModeCDTO, selectedFundsAndDonationsList);
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                            Close();
                        }
                    }
                }
                catch
                {
                    KioskStatic.logToFile("Error Opening Bill acceptor port " + KioskStatic.config.baport.ToString());
                    frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(423) + ". " + KioskStatic.Utilities.MessageUtils.getMessage(441));
                    f.ShowDialog();
                    this.Close();
                }
            }


            //flpPaymentOptions.Refresh();
            //if (flpPaymentOptions.Height > panelPaymentModes.Height)
            //{
            //    vScrollBarPaymentModes.Visible = true;
            //    vScrollBarPaymentModes.Maximum = (int)((flpPaymentOptions.Height - panelPaymentModes.Height) * 1.3);
            //    vScrollBarPaymentModes.SmallChange = Math.Max(1, vScrollBarPaymentModes.Maximum / 10);
            //    vScrollBarPaymentModes.LargeChange = Math.Max(1, vScrollBarPaymentModes.Maximum / 4);
            //}
            //else
            //{
            //    vScrollBarPaymentModes.Visible = false;
            //}


            //Reconciliation of landscape change to default cash as payment.
            if (KioskStatic.config.baport <= 0 && pOSPaymentModeInclusionDTOList == null && pOSPaymentModeInclusionDTOList.Count <= 0)// KioskStatic.ccPaymentModeDetails == null)
            {
                frmOKMsg f = new frmOKMsg(KioskStatic.Utilities.MessageUtils.getMessage(1257) + ". " + KioskStatic.Utilities.MessageUtils.getMessage(441));
                f.ShowDialog();
                this.Close();
                log.LogMethodExit();
                return;
            }

            //else if (KioskStatic.config.baport <= 0 && KioskStatic.Utilities.getParafaitDefaults("SHOW_DEBIT_CARD_BUTTON") == "N")
            //{
            //    callTransactionForm("CREDITCARD");
            //    DialogResult = System.Windows.Forms.DialogResult.OK;
            //    Close();
            //}
            SetCustomizedFontColors();
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
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }
        //End Timer Cleanup

        private void frmCardCount_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timeout.Stop();
        }



        PaymentModeDTO selectedPaymentModeDTO = new PaymentModeDTO();
        private void btnPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (KioskStatic.Utilities.getParafaitDefaults("ENABLE_LOYALTY_INTERFACE") == "Y")
            {
                StopKioskTimer();
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
            ResetKioskTimer();
            Button b = sender as Button;
            b.Enabled = false;
            btnCancel.Visible = true;
            KioskStatic.logToFile(b.Name + "(" + b.Text + ")" + " Button clicked. ");
            selectedPaymentModeDTO = (PaymentModeDTO)b.Tag;
            selectedFundsAndDonationsList = GetSelectedFundsAndDonationsList();

            string message = "";
            if (selectedFundsAndDonationsList != null && selectedFundsAndDonationsList.Any())
            {
                if (_currentTrx == null)
                {
                    _currentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                    _currentTrx.PaymentReference = "Kiosk Transaction";
                    KioskStatic.logToFile("New Trx object created for fund raiser and donation products");
                }

                foreach (KeyValuePair<string, ProductsDTO> keyValuePair in selectedFundsAndDonationsList)
                {
                    if (_currentTrx.createTransactionLine(_rechargeCard, keyValuePair.Value.ProductId, Convert.ToDouble(keyValuePair.Value.Price), 1, ref message) != 0)
                    {
                        message = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Error") + ": " + message;
                        KioskStatic.logToFile("Error TrxLine1: " + message);
                    }
                }
            }
            callTransactionForm(selectedPaymentModeDTO, selectedFundsAndDonationsList);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            b.Enabled = true;
            Close();
            log.LogMethodExit();
        }

        void callTransactionForm(PaymentModeDTO paymentModeDTO, List<KeyValuePair<string, ProductsDTO>> activeFundsDonationsDTO)
        {
            //secondsRemaining = 30;
            //if (timeout != null)
            //{
            //    timeout.Stop();
            //}
            log.LogMethodEntry(paymentModeDTO);
            ResetKioskTimer();
            StopKioskTimer();
            if (paymentModeDTO.IsDebitCard == true)
            {
                TransactionPaymentsDTO transactionPaymentsDTO = null;
                using (frmPaymentGameCard frmg = new frmPaymentGameCard(_pFunction, _prowProduct, _rechargeCard, customerDTO, paymentModeDTO,
                                                                        _cardCount, selectedEntitlementType,
                                                                        discountSummaryDTO == null ? null : discountSummaryDTO,
                                                                        couponNumber == string.Empty ? null : couponNumber, activeFundsDonationsDTO
                                                                        , _currentTrx))
                {
                    frmg.ShowDialog();
                    if (frmg.debitTrxPaymentDTO != null)
                    {
                        transactionPaymentsDTO = frmg.debitTrxPaymentDTO;
                    }
                }
            }
            else
            {
                using (frmCardTransaction frm = new frmCardTransaction(_pFunction, _prowProduct, _rechargeCard, customerDTO, paymentModeDTO,
                                                                       _cardCount, selectedEntitlementType, selectedLoyaltyCardNo,
                                                                       discountSummaryDTO == null ? null : discountSummaryDTO,
                                                                       couponNumber == string.Empty ? null : couponNumber, activeFundsDonationsDTO
                                                                       , _currentTrx))
                {
                    frm.ShowDialog();
                }
            }
            log.LogMethodExit();
        }
        clsKioskTransaction kioskTrx;
        private void btnDiscount_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (btnDiscount.Tag.ToString() == "applyDiscount")
            {
                kioskTrx = new clsKioskTransaction(this, _pFunction, _prowProduct, _rechargeCard, customerDTO, selectedPaymentModeDTO, _cardCount, selectedEntitlementType, selectedLoyaltyCardNo, discountSummaryDTO, couponNumber, _currentTrx, selectedFundsAndDonationsList);
                StopKioskTimer();
                using (frmScanCoupon frm = new frmScanCoupon(kioskTrx))
                {
                    if (frm.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    {
                        couponNumber = kioskTrx.CouponNumber;
                        if (kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList.Count != 0)
                        {
                            discountSummaryDTO = kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0];
                            panelDiscount.Visible = true;
                            txtDiscountName.Text = kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0].DiscountName.ToString();
                            txtDiscPerc.Text = (kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0].DiscountPercentage.ToString() + "%");
                            txtDiscAmount.Text = kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0].DiscountAmount.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_FORMAT);
                            decimal finalPrice = kioskTrx.AmountRequired - kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0].DiscountAmount;
                            lblTotalToPay.Text = finalPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                            btnDiscount.Text = KioskStatic.Utilities.MessageUtils.getMessage("Remove Coupon");
                            btnDiscount.Tag = "cancelDiscount";
                        }
                    }
                    if (frm != null)
                        frm.Dispose();
                }
            }
            else if (btnDiscount.Tag.ToString() == "cancelDiscount")
            {
                decimal finalPrice = kioskTrx.AmountRequired;
                lblTotalToPay.Text = finalPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                try
                {
                    kioskTrx.DummyCurrentTrx.cancelDiscountLine(kioskTrx.DummyCurrentTrx.DiscountsSummaryDTOList[0].DiscountId);
                    discountSummaryDTO = null;
                }
                catch (Exception ex)
                {
                    KioskStatic.logToFile(ex.Message);
                }
                panelDiscount.Visible = false;
                lblTotalToPay.Text = finalPrice.ToString(KioskStatic.Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                btnDiscount.Text = KioskStatic.Utilities.MessageUtils.getMessage("Apply Coupon");
                btnDiscount.Tag = "applyDiscount";
            }
            StartKioskTimer();
            log.LogMethodExit();
        }
        //private void btnPrev_Click(object sender, EventArgs e)
        //{
        //    DialogResult = System.Windows.Forms.DialogResult.No;
        //    Close();
        //}

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //    Close();
        //}

        private void DisplayValidtyMsg(int productId)
        {
            log.LogMethodEntry(productId);
            string validityMsg = "";
            validityMsg = KioskStatic.GetProductCreditPlusValidityMessage(productId, selectedEntitlementType);
            int lineCount = Regex.Split(validityMsg, "\r\n").Count();
            if (lineCount > 2)
            {
                string[] validityMsgList = Regex.Split(validityMsg, "\r\n");
                lblProductCPValidityMsg.Text = validityMsgList[0] + "\r\n" + validityMsgList[1];
                System.Windows.Forms.ToolTip validtyMsgToolTip = new ToolTip();
                validtyMsgToolTip.SetToolTip(lblProductCPValidityMsg, validityMsg);
            }
            else
            {
                lblProductCPValidityMsg.Text = validityMsg;
            }

            log.LogMethodExit();
        }

        private void vScrollBarPaymentModes_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                if (e.NewValue > e.OldValue)
                    scrollDown(e.NewValue - e.OldValue);
                else if (e.NewValue < e.OldValue)
                    scrollUp(e.OldValue - e.NewValue);
            }
            catch { }
            log.LogMethodExit();
        }
        void scrollDown(int value = 10)
        {
            log.LogMethodEntry(value);
            //if (flpPaymentOptions.Top + flpPaymentOptions.Height > panelPaymentModes.Height)
            //{
            //    flpPaymentOptions.Top = flpPaymentOptions.Top - value;
            //}
            log.LogMethodExit();
        }

        void scrollUp(int value = 10)
        {
            log.LogMethodEntry(value);
            if (flpPaymentOptions.Top < 0)
                flpPaymentOptions.Top = Math.Min(0, flpPaymentOptions.Top + value);
            log.LogMethodExit();
        }
        private void BuildToolTipForlblQuantity(string message)
        {
            log.LogMethodEntry(message);
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.SetToolTip(lblQuantity, message);
            log.LogMethodExit();
        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.Show(lblQuantity.Text, lblQuantity);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void lblProductCPValidityMsg_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ToolTip msgToolTip = new ToolTip();
            msgToolTip.Show(lblProductCPValidityMsg.Text, lblProductCPValidityMsg);
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                foreach (Control c in flpPaymentOptions.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("button"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeBtnTextForeColor;//Payment options buttons
                    }
                }
                foreach (Control c in panelSummary.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeSummaryHeaderTextForeColor;//Summary Panel header
                    }
                }
                foreach (Control c in panelDeposit.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeDepositeHeaderTextForeColor;//Deposite panel header
                    }
                }
                foreach (Control c in panelDiscount.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeDiscountHeaderTextForeColor;//Discount panel header
                    }
                }
                foreach (Control c in flowLayoutPanel1.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeSummaryInfoTextForeColor;//PaymentMode Summary Info 
                    }
                }
                foreach (Control c in panel3.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeDepositeInfoTextForeColor;//PaymentMode Deposite Info
                    }
                }
                foreach (Control c in panel2.Controls)
                {
                    string type = c.GetType().ToString().ToLower();
                    if (type.Contains("label"))
                    {
                        c.ForeColor = KioskStatic.CurrentTheme.PaymentModeDiscountInfoTextForeColor;//PaymentMode Discount Info 
                    }
                }
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.PaymentModeBackButtonTextForeColor;//Back button
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.PaymentModeCancelButtonTextForeColor;//Cancel button
                this.label1.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayHeaderTextForeColor;//Total to pay header label
                this.btnDiscount.ForeColor = KioskStatic.CurrentTheme.PaymentModeDiscountBtnTextForeColor;//Coupon button
                this.lblProductCPValidityMsg.ForeColor = KioskStatic.CurrentTheme.PaymentModeCPValidityTextForeColor;//product cp validity
                this.lblTotalToPay.ForeColor = KioskStatic.CurrentTheme.PaymentModeTotalToPayInfoTextForeColor;//Total to pay info label
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets active Fund and Donation products DTO.
        /// </summary>
        /// <param name=""></param> None
        /// <returns = List<KeyValuePair<string, ProductsDTO>> </returns> dictionary holding the fund/donation products DTO List
        public List<KeyValuePair<string, ProductsDTO>> GetSelectedFundsAndDonationsList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            StopKioskTimer();
            KioskStatic.logToFile("Fetching user selected fund/donation products");
            List<KeyValuePair<string, ProductsDTO>> selectedFundsAndDonationsList = new List<KeyValuePair<string, ProductsDTO>>();
            try
            {
                List<ProductsDTO> activeFunds = KioskHelper.GetActiveFundRaiserProducts(executionContext);
                if (activeFunds != null && activeFunds.Any())
                {
                    frmFundRaiserAndDonation frmFandD = new frmFundRaiserAndDonation(activeFunds, true);
                    DialogResult dr = frmFandD.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        for (int i = 0; i < frmFandD.selectedProductsDTOList.Count; i++)
                        {
                            selectedFundsAndDonationsList.Add(new KeyValuePair<string, ProductsDTO>(KioskStatic.FUND_RAISER_LOOKUP_VALUE, frmFandD.selectedProductsDTOList[i]));
                        }
                    }
                }

                List<ProductsDTO> activeDonations = KioskHelper.GetActiveDonationProducts(executionContext);
                if (activeDonations != null && activeDonations.Any())
                {
                    frmFundRaiserAndDonation frmFandD = new frmFundRaiserAndDonation(activeDonations, false);
                    DialogResult dr = frmFandD.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        for (int i = 0; i < frmFandD.selectedProductsDTOList.Count; i++)
                        {
                            selectedFundsAndDonationsList.Add(new KeyValuePair<string, ProductsDTO>(KioskStatic.DONATIONS_LOOKUP_VALUE, frmFandD.selectedProductsDTOList[i]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while fetching getting selected fund/donation products : " + ex.Message);
            }

            log.LogMethodExit();
            return selectedFundsAndDonationsList;
        }
    }
}
