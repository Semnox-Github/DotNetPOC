/********************************************************************************************
 * Project Name - PaymentDetails
 * Description  - Handle Payment UI for POS
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad   Created 
 *2.50.0      12-Dec-2018      Mathew Ninan     Remove staticDataExchange from calls as Static data exchange
 *                                              is deprecated. Replace paymentDetails with TransactionPayments
 *2.60.0      29-Apr-2019      Mathew Ninan     Rounding to be done upfront in case of lesser values. This is based
 *                                              on tendered value and round off amount configuration.
 * 2.70        1-Jul-2019      Lakshminarayana  Modified to add support for ULC cards 
 * 2.80.0     10-Mar-2020      Mathew Ninan     Avoiding saving transaction before performing credit card payment.
 *                                              Transaction will be saved only if Payment is successful
 *                                              Credit plus validity status check for daily limit method
 *2.90.0       23-Jun-2020     Raghuveera       Variable refund changes btnPaymentMode_click() for allowing -ve amount
 *2.130.0      08-Aug-2021	   Mathew Ninan     Modified logic for Parent account transfer to child account
 *2.130.4      08-Feb-2022	   Girish Kundar    Modified: Smartro Fiscalization
 *2.130.4      22-Feb-2022     Mathew Ninan     Modified DateTime to ServerDateTime 
 *2.130.4      11-Apr-2022	   Girish Kundar    Modified: Smartro Fiscalization Issue Fixes
 *2.130.7      13-Apr-2022     Guru S A         Payment mode OTP validation changes
 *2.140.0      08-Feb-2022	   Girish Kundar  Modified: Smartro Fiscalization
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Device;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.CardCore;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Currency;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using Logger = Semnox.Core.Utilities.Logger;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Device.Printer.FiscalPrint.Smartro;
using Semnox.Parafait.POS;
using Semnox.Parafait.CommonUI;
using System.Collections.ObjectModel;
using System.Windows.Interop;
using System.Configuration;
using Semnox.Parafait.Customer;
using System.Text.RegularExpressions;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Device.Printer.FiscalPrint.Smartro;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.PrintUI;
using System.Collections.Concurrent;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Parafait.TableAttributeSetupUI;
using ParafaitPOS;

//using Semnox.Parafait.Tags;
//using Semnox.Parafait.Tags.CardActivityLog;

namespace Parafait_POS
{
    public partial class PaymentDetails : Form
    {
        private Utilities Utilities = POSStatic.Utilities;
        private MessageUtils MessageUtils = POSStatic.MessageUtils;
        private TaskProcs TaskProcs = POSStatic.TaskProcs;
        private ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        private readonly TagNumberParser tagNumberParser;
        //Added on 9-may-2016 
        bool isCouponPayment = false;
        int paymentModeId = 0;
        string couponUsed = string.Empty;
        int paymentCouponSetId = -1;
        //end

        //Added on 9-july-2016 
        bool isPointsPayment = false;
        string validationCode = string.Empty;
        //end

        double tobePaidAmount = 0;
        double balanceAmount = 0;
        double cashAmount = 0;
        decimal roundedCashAmount = 0;
        double creditCardAmount = 0;
        double OtherPaymentAmount = 0;
        double gameCardAmount = 0;
        double paidAmount = 0;
        double creditPlusAmount = 0;
        string currencyCode = "";
        double? currencyRate = null;

        public double CashTip = 0;
        public double CreditTip = 0;
        public double TotalAmount = 0;
        public bool IsTipEnabled = false;

        public double TenderedAmount = 0;
        public double CashAmount = 0;
        public double PaymentCreditCardSurchargeAmount = 0;

        public bool TrxStatusChanged = false;
        private string QRCode = "";//ChinaICBC changes
        string PrimaryCardNumber = "";
        string AMOUNT_FORMAT;
        Transaction Transaction;
        int creditCardModeApprovedBy = -1;
        int otherModeApprovedBy = -1;
        TextBox CurrentTextBox;
        public List<int> lstUserId = new List<int>();

        int PaymentSplitId = -1;

        double TrxSplitDiff = 0; //New property for storing Split difference between Original Trx Amt and Split Amts       
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool PaymentReferenceMandatory = false;
        private Dictionary<string, ApprovalAction> paymentModeOTPApprovals = new Dictionary<string, ApprovalAction>();
        private ConcurrentQueue<KeyValuePair<int, string>> statusProgressMsgQueue;

        private KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>> preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
        private int onLoadselectedPaymentModeId = -1;

        private string giftCardNumber = "";//added for clubspeed
        private string giftCardPIN="";//added for quikcilver
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        private TransactionUtils TransactionUtils;

        public PaymentDetails(Transaction Trx) : this(Trx, -1, 0)
        {
            log.LogMethodEntry("Trx");
            Logger.setRootLogLevel(log);

            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext,
                "DISABLE_SPLIT_PAYMENTS"))
            {
                btnSplit.Visible = false;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        public PaymentDetails(Transaction Trx, int paymentSplitId, double trxDiff)
        {
            log.LogMethodEntry("Trx", paymentSplitId, trxDiff);
            Logger.setRootLogLevel(log);
            POSUtils.SetLastActivityDateTime();
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            TransactionUtils = new TransactionUtils(Utilities);

            PaymentSplitId = paymentSplitId;
            TrxSplitDiff = trxDiff;
            Utilities.setLanguage();
            InitializeComponent();
            Transaction = Trx;

            dgvGameCards.Columns["Amount"].DefaultCellStyle =
            dgvGameCards.Columns["UsedAmount"].DefaultCellStyle =
                dgvGameCards.Columns["Balance"].DefaultCellStyle =
               paidPaymentAmount.DefaultCellStyle = Utilities.gridViewAmountCellStyle();

            paymentDate.DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_GAMECARD_KEYIN_FOR_PAYMENT", false))
            {
                dgvGameCards.Columns[1].HeaderCell.Style.ForeColor = Color.Blue;
            }

            dgvCouponValue.Columns[1].HeaderCell.Style.ForeColor = Color.Blue;
            dgvCouponValue.AllowUserToAddRows = false;

            dgvGameCards.AllowUserToAddRows = false;

            txtMessage.Text = "";
            AMOUNT_FORMAT = ParafaitEnv.AMOUNT_FORMAT;

            if (Trx.PrimaryCard != null)
                PrimaryCardNumber = Trx.PrimaryCard.CardNumber;

            CurrentTextBox = txtCashAmount;

            POSUtils.SetLastActivityDateTime();
            createPaymentModeButtons();

            setupCreditCards();
            setupOtherPaymentModes();

            foreach (DataGridViewColumn dc in dgvGameCards.Columns)
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;
            foreach (DataGridViewColumn dc in dgvSavedPayments.Columns)
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;

            txtChangeBack.BorderStyle = txtTotalAmount.BorderStyle = txtPaidAmount.BorderStyle = txtBalanceAmount.BorderStyle = BorderStyle.None;

            txtChangeBack.ForeColor = Color.DarkGreen;

            TrxStatusChanged = false;

            IsTipEnabled = Utilities.getParafaitDefaults("SHOW_TIP_AMOUNT_KEYPAD").Equals("Y"); //Modification on 09-Nov-2015: Tip amount feature

            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext,
                "DISABLE_SPLIT_PAYMENTS"))
            {
                btnSplit.Visible = false;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                POSUtils.SetLastActivityDateTime();
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                string scannedTagNumber = checkScannedEvent.Message;
                DeviceClass encryptedTagDevice = sender as DeviceClass;
                if (tagNumberParser.IsTagDecryptApplicable(encryptedTagDevice, checkScannedEvent.Message.Length))
                {
                    string decryptedTagNumber = string.Empty;
                    try
                    {
                        decryptedTagNumber = tagNumberParser.GetDecryptedTagData(encryptedTagDevice, checkScannedEvent.Message);
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number result: ", ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit(txtMessage.Text);
                        return;
                    }
                    try
                    {
                        scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                    }
                    catch (ValidationException ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit(txtMessage.Text);
                        return;
                    }
                    catch (Exception ex)
                    {
                        log.LogVariableState("Decrypted Tag Number validation: ", ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit(txtMessage.Text);
                        return;
                    }
                }
                if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(scannedTagNumber);
                    txtMessage.Text = message;
                    log.LogMethodExit(null, "Invalid Tag Number. " + message);
                    return;
                }

                string CardNumber = tagNumber.Value;

                //Start: Modidfication on 10-may-2016 for controling the tap in other mode
                if (isCouponPayment)
                {
                    cardSwiped(CardNumber);
                }
                else
                    CardSwiped(CardNumber, sender as DeviceClass);
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string CardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(CardNumber, readerDevice);
            if (!POSStatic.ParafaitEnv.MIFARE_CARD && dgvGameCards.Rows.Count == 4)
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Text = MessageUtils.getMessage(171);
                log.Info("Ends-CardSwiped() as MIFARE_CARD && dgvGameCards.Rows.Count == 4");
                log.LogMethodExit(txtMessage.Text);
                return;
            }
            // this is triggered when card is swiped when this form is in focus
            InsertCardDetails(CardNumber, readerDevice);
            log.LogMethodExit();
        }


        public void cardSwiped(string couponNumber)
        {
            log.LogMethodEntry(couponNumber);

            POSUtils.SetLastActivityDateTime();
            DataTable dt = ValidateCoupon(paymentModeId, couponNumber);

            if (dt != null && dt.Rows.Count > 0)
            {
                dgvCouponValue.Rows.Clear();
                dgvCouponValue.Rows.Add(new object[] { dt.Rows[0]["CouponSetId"], couponNumber, dt.Rows[0]["CouponValue"] });
            }
            else
            {
                txtMessage.Text = "Invalid or used Coupon Number";
            }
            log.LogMethodExit();
        }
        //end of modification 

        bool InsertCardDetails(string SwipedCardNumber, DeviceClass readerDevice)
        {
            log.LogMethodEntry(SwipedCardNumber, readerDevice);
            log.Debug(SwipedCardNumber);
            Card card;
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                    card = new MifareCard(readerDevice, SwipedCardNumber, ParafaitEnv.LoginID, Utilities);
                else
                    card = new Card(readerDevice, SwipedCardNumber, ParafaitEnv.LoginID, Utilities);

                string message = "";
                if (!POSUtils.refreshCardFromHQ(ref card, ref message))
                {
                    txtMessage.Text = message;
                    log.Info("Ends-InsertCardDetails(" + SwipedCardNumber + ",readerDevice) as  unable to refresh Card From HQ ");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Fatal("Ends-InsertCardDetails(" + SwipedCardNumber + ",readerDevice) as exception " + ex.ToString());
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (card.CardStatus != "ISSUED")
            {
                txtMessage.Text = MessageUtils.getMessage(172);
                PoleDisplay.writeLines(txtMessage.Text, SwipedCardNumber);
                log.Info("Ends-InsertCardDetails(" + SwipedCardNumber + ",readerDevice) as card.CardStatus != ISSUED");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }
            else if (card.technician_card.Equals('N') == false)
            {
                txtMessage.Text = MessageUtils.getMessage(197, SwipedCardNumber);
                PoleDisplay.writeLines(txtMessage.Text, SwipedCardNumber);
                log.Info(SwipedCardNumber + " is tech card");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }
            else
            {
                double credits = card.credits;
                creditPlusAmount = 0;
                if (POSStatic.ParafaitEnv.MIFARE_CARD)
                    creditPlusAmount = card.CreditPlusCardBalance;
                else
                {
                    CreditPlus creditPlus = new CreditPlus(Utilities);
                    creditPlusAmount = creditPlus.getCreditPlusForPOS(card.card_id, Utilities.ParafaitEnv.POSTypeId, Transaction);
                }

                dgvGameCards.Rows.Clear();
                dgvGameCards.Rows.Add(new object[] { card.card_id, SwipedCardNumber, credits + creditPlusAmount, null, null, credits, 0, creditPlusAmount, 0, card.last_update_time });
                if (ValidateGameCards() != 0)
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(183));

                Transaction.TransactionInfo.PrimaryPaymentCardNumber = SwipedCardNumber;
                PoleDisplay.writeLines("Card: " + SwipedCardNumber, "Balance: " + (credits + creditPlusAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));

                if ((credits + creditPlusAmount) < tobePaidAmount && Utilities.getParafaitDefaults("ALLOW_PARENTCARD_PAYMENT_WITHOUT_CARD").Equals("Y"))
                {
                    ParentChildCardsListBL parentChildCardsListBL = new ParentChildCardsListBL(POSStatic.Utilities.ExecutionContext);
                    List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.CHILD_CARD_ID, card.card_id.ToString()));
                    searchParameters.Add(new KeyValuePair<ParentChildCardsDTO.SearchByParameters, string>(ParentChildCardsDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                    List<ParentChildCardsDTO> parentChildCardsDTOList = parentChildCardsListBL.GetParentChildCardsDTOList(searchParameters);
                    if (parentChildCardsDTOList != null && parentChildCardsDTOList.Count > 0)
                    {
                        if (getParentChildCardEntitlementBalance(parentChildCardsDTOList[0], tobePaidAmount))
                        {
                            InsertCardDetails(card.CardNumber, readerDevice);
                        }
                        Transaction.GameCardReadTime = Utilities.getServerTime();
                        //Check parent card account balance
                        //Transfer Balance from Parent Card to child card -- Call another method
                        //if Daily Perc define, transfer defined perc else transfer only required balance
                        //Create method in AccountBL to take entitlement to transfer and destination child card
                        //Call this method after checking perc check. Send AmttoTransfer
                    }
                    //object parentCardNumber = Utilities.executeScalar(@"select top 1 card_number 
                    //                                                 from cards c, ParentChildCards pcc 
                    //                                                where pcc.ParentCardId = c.card_id 
                    //                                                and pcc.ChildCardId = @cardId
                    //                                                and pcc.ActiveFlag = 1 
                    //                                                and c.valid_flag = 'Y'",
                    //                                                new SqlParameter("@cardId", card.card_id));
                    //if (parentCardNumber != null)
                    //    InsertCardDetails(parentCardNumber.ToString(), readerDevice);
                }
                POSUtils.SetLastActivityDateTime();
                log.LogMethodExit();
                return true;
            }
        }

        void setupCreditCards()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            DataTable paymentCreditCards = new DataTable();
            paymentCreditCards = POSUtils.dtCreditCards.Copy();
            cmbCreditCards.DataSource = paymentCreditCards;
            //cmbCreditCards.DataSource = POSUtils.dtCreditCards;
            cmbCreditCards.DisplayMember = "PaymentMode";
            cmbCreditCards.ValueMember = "PaymentModeId";
            log.LogMethodExit();
        }

        void setupOtherPaymentModes()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            DataTable paymentOtherModes = new DataTable();
            paymentOtherModes = POSUtils.dtOtherPaymentModes.Copy();
            cmbOtherModes.DataSource = paymentOtherModes;
            //cmbOtherModes.DataSource = POSUtils.dtOtherPaymentModes;
            cmbOtherModes.DisplayMember = "PaymentMode";
            cmbOtherModes.ValueMember = "PaymentModeId";
            log.LogMethodExit();
        }

        void CalculateBalances()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            txtMessage.Text = "";
            try
            {
                cashAmount = Math.Round(Convert.ToDouble((string.IsNullOrEmpty(txtCashAmount.Text) ? "0" : txtCashAmount.Text)), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            }
            catch
            {
                cashAmount = 0;
            }

            try
            {
                creditCardAmount = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtCreditCardAmount.Text) ? "0" : txtCreditCardAmount.Text), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            }
            catch
            {
                creditCardAmount = 0;
            }
            if (creditCardAmount == 0)
            {
                cmbCreditCards.SelectedIndex = -1;
                cmbCreditCards.Tag = null;
                txtCCSurchPercent.Clear();
                txtCCSurchAmt.Clear();
                txtTotalCCAmountWithSurch.Clear();
            }

            try
            {
                gameCardAmount = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtGameCardAmount.Text) ? "0" : txtGameCardAmount.Text), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            }
            catch
            {
                gameCardAmount = 0;
            }

            try
            {
                OtherPaymentAmount = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtOtherPaymentAmount.Text) ? "0" : txtOtherPaymentAmount.Text), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            }
            catch
            {
                OtherPaymentAmount = 0;
            }

            if (OtherPaymentAmount == 0)
                cmbOtherModes.SelectedIndex = -1;
            balanceAmount = Math.Round(Math.Round(tobePaidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) - cashAmount - creditCardAmount - gameCardAmount - OtherPaymentAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
            displayAmounts();

            if (balanceAmount != 0)
            {
                txtBalanceAmount.ForeColor = Color.OrangeRed;
            }
            else
            {
                txtBalanceAmount.ForeColor = Color.LimeGreen;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        void displayAmounts()
        {
            log.LogMethodEntry();
            txtCashAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", cashAmount);
            txtCreditCardAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", creditCardAmount);
            txtGameCardAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", gameCardAmount);
            txtOtherPaymentAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", OtherPaymentAmount);
            txtBalanceAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", balanceAmount);
            txtTipAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", CashTip + CreditTip);//Begin Modification on 09-Nov-2015:Tip feature
            log.LogMethodExit();
        }

        double getDebitCardPaymentAmount()
        {
            log.Debug("Starts-getDebitCardPaymentAmount()");
            double d = 0;
            POSUtils.SetLastActivityDateTime();
            foreach (DataGridViewRow r in dgvGameCards.Rows)
            {
                try
                {
                    if (!string.IsNullOrEmpty(r.Cells["UsedAmount"].Value.ToString()))
                        d += Convert.ToDouble(r.Cells["UsedAmount"].Value);
                }
                catch
                {
                    log.Fatal("Ends-getDebitCardPaymentAmount() due to exception in r.Cells[UsedAmount].Value ");
                }
            }
            log.Debug("Ends-getDebitCardPaymentAmount()");
            return d;
        }

        double getOtherPaymentAmount()
        {
            log.Debug("Starts-getOtherPaymentAmount()");
            double d = 0;
            POSUtils.SetLastActivityDateTime();
            if (cmbOtherModes.SelectedIndex >= 0)
            {
                double.TryParse(txtOtherPaymentAmount.Text, out d);
            }
            log.Debug("Ends-getOtherPaymentAmount()");
            return d;
        }

        double getCreditCardPaymentAmount(ref double SurchargeAmount)
        {
            log.Debug("Starts-getCreditCardPaymentAmount()");
            POSUtils.SetLastActivityDateTime();
            double d = 0;
            SurchargeAmount = 0;

            try
            {
                if (cmbCreditCards.SelectedIndex >= 0)
                {
                    double camt = 0;
                    if (Double.TryParse(txtCreditCardAmount.Text, out camt))
                    {
                        double surcharge = 0;
                        double.TryParse(txtCCSurchPercent.Text, out surcharge);
                        {
                            surcharge = camt * surcharge / 100.0;
                            if (surcharge < 0)
                                surcharge = 0;
                            txtCCSurchAmt.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_FORMAT + "}", surcharge);
                        }
                        d = camt + surcharge;
                        txtTotalCCAmountWithSurch.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_FORMAT + "}", d);
                        SurchargeAmount = surcharge;
                    }
                }
            }
            catch
            {
                log.Fatal("Ends-getCreditCardPaymentAmount() due to exception while calculating surcharge ");
            }
            POSUtils.SetLastActivityDateTime();

            log.Debug("Ends-getCreditCardPaymentAmount()");
            return d;
        }

        void GetCreditCardSurcharge(int paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);
            DataTable dt = Utilities.executeDataTable(@"select CreditCardSurchargePercentage, LookupValue, ManagerApprovalRequired
                                                                    from PaymentModes p left outer join LookupValues l
                                                                    on p.Gateway = l.LookupValueId
                                                                    where PaymentModeId = @id",
                                                                   new SqlParameter[] { new SqlParameter("@id", paymentModeId) });

            if (dt.Rows.Count == 0)
            {
                txtCCSurchPercent.Clear();
                cmbCreditCards.Tag = null;
            }
            else
            {
                txtCCSurchPercent.Text = dt.Rows[0]["CreditCardSurchargePercentage"].ToString();
                cmbCreditCards.Tag = dt.Rows[0]["LookupValue"]; // gateway
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void txtCashAmount_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtCashAmount_Validating()");
            POSUtils.SetLastActivityDateTime();
            CalculateBalances();
            log.Debug("Ends-txtCashAmount_Validating()");
        }

        private void PaymentDetails_Load(object sender, EventArgs e)
        {
            try
            {
                POSUtils.SetLastActivityDateTime();
                flpPaymentModes.Enabled = false;
                onLoadselectedPaymentModeId = -1;
                log.Debug("Starts-PaymentDetails_Load()");//Modified for Adding logger feature on 08-Mar-2016
                Utilities.setLanguage(this);
                load();
                this.TopMost = false;
                if (POSStatic.HIDE_CC_DETAILS_IN_PAYMENT_SCREEN)
                    panelCCDetails.Visible = false;
                txtChangeBack.Clear();
                log.Debug("Ends-PaymentDetails_Load()");
            }
            finally
            {
                flpPaymentModes.Enabled = true;
                try
                {
                    SetPaymentModeOnAttributeButton(onLoadselectedPaymentModeId);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                POSUtils.SetLastActivityDateTime();
            }
        }

        void load()
        {
            log.Debug("Starts-load()");//Modified for Adding logger feature on 08-Mar-2016
            POSUtils.SetLastActivityDateTime();
            preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
            cashAmount = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                           && x.paymentModeDTO.IsCash).Sum(x => x.Amount); //0;
            creditCardAmount = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                           && x.paymentModeDTO.IsCreditCard).Sum(x => x.Amount); //0;
            gameCardAmount = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                           && x.paymentModeDTO.IsDebitCard).Sum(x => x.Amount); //0;
            OtherPaymentAmount = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                           && !x.paymentModeDTO.IsCash
                                                                           && !x.paymentModeDTO.IsCreditCard
                                                                           && !x.paymentModeDTO.IsDebitCard).Sum(x => x.Amount); //0;

            displayAmounts();
            if (cashAmount > 0)
            {
                onLoadselectedPaymentModeId = GetSelectedIdforMode("CASH");
            }
            else if (creditCardAmount > 0)
            {
                onLoadselectedPaymentModeId = GetSelectedIdforMode("CREDITCARD");
            }
            else if (gameCardAmount != 0)
            {
                onLoadselectedPaymentModeId = GetSelectedIdforMode("GAMECARD");
            }
            else if (OtherPaymentAmount > 0)
            {
                onLoadselectedPaymentModeId = GetSelectedIdforMode("OTHERS");
            }
           
            Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
            if (TrxStatusChanged)
            {
                Transaction = TransactionUtils.CreateTransactionFromDB(Transaction.Trx_id, Transaction.Utilities);
            }
            cmbCreditCards.SelectedIndex = -1;
            cmbOtherModes.SelectedIndex = -1;
            cmbCreditCards.Tag = null;

            QRCode = txtCCAuthorization.Text = txtCCExpiry.Text = txtCCName.Text = txtCCNumber.Text = txtNameOnCC.Text = txtOtherPaymentReference.Text = txtPaymentReference.Text = "";

            giftCardNumber = "";
            giftCardPIN = "";

            paidAmount = 0;
            double cashPaid = 0, creditCardPaid = 0, otherPaid = 0, debitCardPaid = 0;
            double trxPaidAmount = 0;
            dgvSavedPayments.Rows.Clear();
            if (Transaction.Trx_id > 0)
            { //Begin Modification on 09-Nov-2015 :Tip feature
                //13-mar-2016 split
                DataTable paidDT = Utilities.executeDataTable(@"select tp.*, c.card_number,
                                                                pm.isCash, pm.isCreditCard, pm.isDebitCard, 
                                                                pm.PaymentMode, pm.PaymentModeId,  
                                                                    (select LookupValue 
                                                                        from LookupValues l
                                                                        where tp.PaymentModeId = pm.PaymentModeId 
                                                                        and pm.Gateway = l.LookupValueId) as Gateway 
                                                                  from trxPayments tp left outer join cards c on c.card_id = tp.cardId, 
                                                                        paymentModes pm 
                                                                 where tp.trxId = @trxId and pm.paymentModeId = tp.paymentModeId
                                                                 and (tp.splitId = @splitId or @splitId = -1)",
                                                              new SqlParameter("@trxId", Transaction.Trx_id),
                                                              new SqlParameter("@splitId", PaymentSplitId));
                foreach (DataRow dr in paidDT.Rows)
                {
                    double amount = Convert.ToDouble(dr["Amount"]);
                    double Tips = Convert.ToDouble(dr["TipAmount"] == DBNull.Value ? 0.00 : dr["TipAmount"]);
                    if (dr["isCash"].ToString()[0].Equals('Y'))
                        cashPaid += amount;
                    else if (dr["isCreditCard"].ToString()[0].Equals('Y'))
                        creditCardPaid += amount;
                    else if (dr["isDebitCard"].ToString()[0].Equals('Y'))
                        debitCardPaid += amount;
                    else
                        otherPaid += amount;

                    dgvSavedPayments.Rows.Add("X", dr["PaymentMode"],
                                             (dr["card_number"] == DBNull.Value ? (dr["CreditCardNumber"] == DBNull.Value ? dr["Reference"] : dr["CreditCardNumber"]) : dr["card_number"]),
                                             dr["PaymentDate"],
                                             amount, Tips.ToString("0.00"), "...");
                    dgvSavedPayments.Rows[dgvSavedPayments.Rows.Count - 1].Tag = dr;
                    paidAmount += amount + Tips;//Ends Modification on 09-Nov-2015
                    trxPaidAmount += amount; // 13-mar-2016
                }
            }

            foreach (DataGridViewRow dr in dgvSavedPayments.Rows)
            {
                DataRow drp = dr.Tag as DataRow;
                bool readOnly = !(drp["ParentPaymentId"] == DBNull.Value);
                if (!readOnly)
                {
                    if (IsPaymentReversed(drp["PaymentId"]))
                    {
                        readOnly = true;
                    }
                    //foreach (DataGridViewRow dr1 in dgvSavedPayments.Rows)
                    //{
                    //    DataRow drp1 = dr1.Tag as DataRow;
                    //    if(drp1["ParentPaymentId"].Equals(drp["PaymentId"]))
                    //    {
                    //        readOnly = true;
                    //        break;
                    //    }
                    //}
                }
                dr.Cells["deleteAllowed"].Value = (readOnly ? 0 : 1);
                if (readOnly)
                {
                    dr.DefaultCellStyle.BackColor = Color.LightGray;
                }
            }

            txtTotalCashPayment.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", cashPaid);
            txtTotalDebitCardPayment.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", debitCardPaid);
            txtTotalOtherPayment.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", otherPaid);
            txtCCTotalAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", creditCardPaid);
            txtTotalPaidAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", paidAmount);

            if (PaymentSplitId != -1)
                Transaction.updateAmounts(false); // refresh the Transaction.Net_Transaction_Amount from trx lines of split trx
            tobePaidAmount = Transaction.Net_Transaction_Amount - trxPaidAmount;

            if (PrimaryCardNumber != "" && POSStatic.ParafaitEnv.MIFARE_CARD == false
                 && POSStatic.Utilities.getParafaitDefaults("AUTOLOAD_GAMECARD_IN_PAYMENT_SCREEN") == "Y")
                InsertCardDetails(PrimaryCardNumber, Common.Devices.PrimaryCardReader);
            else if (dgvGameCards.Rows.Count > 0 && dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString() != "")
                InsertCardDetails(dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString(), Common.Devices.PrimaryCardReader);

            //Begin Modification on 09-Nov-2015 :Tip feature
            txtTotalAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", Transaction.Net_Transaction_Amount + Transaction.Tip_Amount + CashTip + CreditTip);
            TotalAmount = Transaction.Net_Transaction_Amount + Transaction.Tip_Amount + CashTip + CreditTip;
            txtTrxAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", Transaction.Net_Transaction_Amount);
            txtTipAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", Transaction.Tip_Amount);
            //Ends Modification on 09-Nov-2015

            if (Math.Round((trxPaidAmount + Transaction.Tip_Amount), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round(paidAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero))//Modification on 18-Nov-2015:Tip feature
            {
                txtMessage.Text = "CHECK TOTAL PAID AMOUNT";
                Application.DoEvents();
                System.Threading.Thread.Sleep(3000);
            }

            POSUtils.SetLastActivityDateTime();
            CalculateBalances();

            Transaction.GameCardReadTime = Utilities.getServerTime();

            if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
            {
                if (ApplyPayment())
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                else
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

                log.Info("Ends-load() as AUTO_DEBITCARD_PAYMENT_POS - apply() ");
                return;
            }

            if (Utilities.ParafaitEnv.ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS == "Y")
            {
                txtCashAmount.Enabled = btnCashPayment.Enabled =
                    txtOtherPaymentAmount.Enabled = btnOtherPayment.Enabled =
                    txtCreditCardAmount.Enabled = btnCreditCardPayment.Enabled = false;
            }

            if (cashAmount > 0)
            {
                tcPaymentModes.TabPages.Clear();
                tcPaymentModes.TabPages.Add(tpCash);
                this.ActiveControl = txtCashAmount;
            }
            else if (creditCardAmount > 0)
            {
                tcPaymentModes.TabPages.Clear();
                tcPaymentModes.TabPages.Add(tpCreditCard);
                this.ActiveControl = txtCreditCardAmount;
            }
            else if (gameCardAmount != 0)
            {
                tcPaymentModes.TabPages.Clear();
                tcPaymentModes.TabPages.Add(tpDebitCard);
                this.ActiveControl = txtGameCardAmount;
            }
            else if (OtherPaymentAmount > 0)
            {
                tcPaymentModes.TabPages.Clear();
                tcPaymentModes.TabPages.Add(tpOtherMode);
                this.ActiveControl = txtOtherPaymentAmount;
            }

            txtPaidAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", paidAmount);

            if (PaymentSplitId == -1)
            {
                if (Utilities.executeScalar(@"select top 1 1 from trxSplitPayments where trxId = @trxId",
                                             new SqlParameter("@trxId", Transaction.Trx_id)) != null)
                {
                    btnSave.Enabled = false;
                }
                else
                {

                }
            }
            else
            {
                btnSplit.Enabled = false;
            }

            try
            {
                if (POSUtils.dtAllPaymentModes != null)
                {
                    if (!POSUtils.dtAllPaymentModes.AsEnumerable().Any(drow => drow.Field<string>("IsCash") == "Y"))
                        tcPaymentModes.Controls.Remove(tpCash);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit();
            }
            POSUtils.SetLastActivityDateTime();
        }
        public bool IsPaymentReversed(object paymentId)
        {
            bool found = false;
            POSUtils.SetLastActivityDateTime();

            if (paymentId.Equals(DBNull.Value))
                return false;

            foreach (DataGridViewRow dr1 in dgvSavedPayments.Rows)
            {
                DataRow drp1 = dr1.Tag as DataRow;
                if (drp1["ParentPaymentId"].Equals(paymentId))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click()");
            POSUtils.SetLastActivityDateTime();
            if (ApplyPayment())
                load();

            log.Debug("Ends-btnSave_Click()");
        }

        //Added on 27-june-2016 for redemptionDiscount
        public void UpdatePointsAmount(double pointsAmount, double tobepaid, string messageCode)
        {
            log.LogMethodEntry(pointsAmount, tobepaid, messageCode);
            POSUtils.SetLastActivityDateTime();
            int pointsPaymentId = -1;
            int pointIndex = -1;

            foreach (DataRow row in POSUtils.dtOtherPaymentModes.Rows)
            {
                pointIndex++;
                if (row["PaymentMode"].ToString() == "Loyalty Points")
                {
                    pointsPaymentId = Convert.ToInt32(row["PaymentModeId"]);
                    break;
                }
            }

            cmbOtherModes.SelectedValue = pointsPaymentId;
            cmbOtherModes.SelectedIndex = pointIndex;

            if (pointIndex > -1 && pointsPaymentId > 0) //PointsPayment Exist
            {
                txtOtherPaymentAmount.Text = pointsAmount.ToString();
                tobePaidAmount = tobepaid;
                isPointsPayment = true;
                validationCode = messageCode;
                ApplyPayment();
                load();
                isPointsPayment = false;
                validationCode = string.Empty;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        bool ApplyPayment()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            bool result = apply();
            QRCode = string.Empty;
            log.LogMethodExit(result);
            return result;
        }

        bool apply()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            paymentModeOTPApprovals = new Dictionary<string, ApprovalAction>();
            if (PaymentReferenceMandatory && String.IsNullOrEmpty(txtPaymentReference.Text))
            {
                MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, MessageUtils.getMessage(1767)));
                POSUtils.SetLastActivityDateTime();
                this.ActiveControl = txtPaymentReference;
                return false;
            }
            //start: Modification on 12-may-2016 for Adding the coupon used details
            if (isCouponPayment && dgvCouponValue.RowCount == 0)
            {
                POSUtils.SetLastActivityDateTime();
                GenericDataEntry gde = new GenericDataEntry(1);
                gde.Text = "Enter Coupon Number";

                gde.DataEntryObjects[0].mandatory = true;
                gde.DataEntryObjects[0].allowMinusSign = false;
                gde.DataEntryObjects[0].label = "Coupon Number";
                gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                gde.DataEntryObjects[0].width = 150;
                gde.DataEntryObjects[0].uppercase = true;

                if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(gde.DataEntryObjects[0].data))
                    {
                        string couponNumber = gde.DataEntryObjects[0].data.ToString();
                        DataTable dt = ValidateCoupon(paymentModeId, couponNumber);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dgvCouponValue.Rows.Clear();
                            dgvCouponValue.Rows.Add(new object[] { dt.Rows[0]["CouponSetId"], couponNumber, dt.Rows[0]["CouponValue"] });
                            isCouponPayment = false;
                        }
                        else
                        {
                            dgvCouponValue.Rows.Clear();
                            txtMessage.Text = "Invalid or Used Coupon Number";
                            log.LogMethodExit(txtMessage.Text);
                            return false;
                        }
                    }
                }
                else
                {
                    log.LogMethodExit("gde.ShowDialog()!= System.Windows.Forms.DialogResult.OK");
                    return false;
                }
            }

            if (dgvCouponValue.RowCount > 0 & !string.IsNullOrEmpty(txtCashAmount.Text))
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvCouponValue.Rows[0].Cells["couponAmount"].Value == DBNull.Value)
                {
                    txtMessage.Text = "Coupon value not specified.";
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }
                if (Convert.ToDouble(dgvCouponValue.Rows[0].Cells["couponAmount"].Value) < Convert.ToDouble(txtOtherPaymentAmount.Text))
                    txtOtherPaymentAmount.Text = Convert.ToDouble(dgvCouponValue.Rows[0].Cells["couponAmount"].Value).ToString();
                couponUsed = dgvCouponValue.Rows[0].Cells["couponNumber"].Value == null ? string.Empty : dgvCouponValue.Rows[0].Cells["couponNumber"].Value.ToString();
                paymentCouponSetId = dgvCouponValue.Rows[0].Cells["couponSetId"].Value == null ? -1 : Convert.ToInt32(dgvCouponValue.Rows[0].Cells["couponSetId"].Value);
                txtCouponValue.Text = Convert.ToDouble(dgvCouponValue.Rows[0].Cells["couponAmount"].Value).ToString();
                dgvCouponValue.Rows.Clear();
            }
            else
            {
                couponUsed = string.Empty;
                paymentCouponSetId = -1;
            }
            //end

            if (!this.ValidateChildren())
            {
                log.LogMethodExit("ValidateChildren() failed");
                return false;
            }
            POSUtils.SetLastActivityDateTime();

            txtMessage.Text = "";
            //Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
            txtBalanceAmount.ForeColor = txtTotalAmount.ForeColor;

            // allow any amount for advance payment during reservation - Aug 20, 2018
            //if (Transaction.Status != Transaction.TrxStatus.RESERVED)
            //{
            //    if (balanceAmount < 0 && (Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId != POSStatic.transactionOrderTypes["Item Refund"]&& Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId != POSStatic.transactionOrderTypes["Refund"]))
            //    {
            //        txtMessage.Text = MessageUtils.getMessage(173);
            //        txtBalanceAmount.ForeColor = Color.OrangeRed;
            //        log.Info("Ends-apply() as Balance amount should be 0 to complete payment. ");
            //        return false;
            //    }
            //}

            txtCreditCardAmount.BackColor = txtGameCardAmount.BackColor = txtCouponValue.BackColor = txtOtherPaymentAmount.BackColor = txtCashAmount.BackColor;

            if (dgvGameCards.Rows.Count > 0)
            {
                POSUtils.SetLastActivityDateTime();
                // iqbal added on march 11, 2016
                decimal preAmount = (decimal)Transaction.Net_Transaction_Amount;

                double creditamount = Convert.ToDouble(dgvGameCards.Rows[0].Cells["CreditPlus"].Value);

                #region Credit Limit Implementation
                //Begin Modification : 25-OCt-2016 for implementing creditpllus under daialy limit
                if (creditPlusAmount > 0)
                {
                    try
                    {
                        //modified code on -25-Oct-2016
                        int cardId = Convert.ToInt32(dgvGameCards.Rows[0].Cells["CardId"].Value);

                        DataTable dtByProductId = GetAvailableCreditPlusLimit(cardId);

                        if (dtByProductId != null && dtByProductId.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtByProductId.Rows.Count; i++)
                            {
                                string query = string.Empty;
                                DataTable productsByCategory = null;
                                if (!DBNull.Value.Equals(dtByProductId.Rows[i]["ProductId"]))
                                {
                                    query = "and l.product_id in (@productId)";
                                }
                                else if (!DBNull.Value.Equals(dtByProductId.Rows[i]["CategoryId"]))
                                {
                                    query = "and l.product_id in (select product_id from products where CategoryId in (SELECT c.categoryId FROM category c LEFT OUTER JOIN category PC on C.ParentCategoryId = pc.CategoryId where c.CategoryId = @categryId and c.IsActive = 'Y')) ";
                                    productsByCategory = Utilities.executeDataTable(@"select product_id from products where CategoryId in (SELECT c.categoryId FROM category c LEFT OUTER JOIN category PC on C.ParentCategoryId = pc.CategoryId where c.CategoryId = @categryId and c.IsActive = 'Y')",
                                                                                        new SqlParameter("@categryId", Convert.ToInt32(dtByProductId.Rows[i]["CategoryId"])));
                                }

                                DataTable usedCreditplusDT = Utilities.executeDataTable(@"select sum(quantity) QuantityUsed, l.product_id
                                                                                from trx_lines l, trx_header h
                                                                                where h.trxid = l.trxid
                                                                                and trxdate between DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE()))) and 1 + DATEADD(HOUR, 6, DATEADD(D, 0, DATEDIFF(D, 0, GETDATE())))
                                                                                " + query +
                                                                            " and CancelledTime is null and exists (select 'x' from trxPayments tp, CardCreditPlus CCP, CardCreditPlusConsumption CCPC " +
                                                                                                                    "where TP.CardCreditPlusId = CCP.CardCreditPlusId " +
                                                                                                                    "and tp.TrxId = l.TrxId " +
                                                                                                                    "and CCPC.CardCreditPlusId = CCP.CardCreditPlusId " +
                                                                                                                    "and CCP.Card_id = @cardId) group by l.product_id ",
                                                                                                new SqlParameter("@productId", DBNull.Value.Equals(dtByProductId.Rows[i]["ProductId"]) ? -1 : Convert.ToInt32(dtByProductId.Rows[i]["ProductId"])),
                                                                                                new SqlParameter("@cardId", cardId),
                                                                                                new SqlParameter("@categryId", DBNull.Value.Equals(dtByProductId.Rows[i]["CategoryId"]) ? -1 : Convert.ToInt32(dtByProductId.Rows[i]["CategoryId"])));
                                int usedQnty = 0;
                                int availableLimit = -1;
                                if (usedCreditplusDT != null && usedCreditplusDT.Rows.Count > 0)
                                {
                                    foreach (DataRow row in usedCreditplusDT.Rows)
                                    {
                                        usedQnty += Convert.ToInt32(row["QuantityUsed"]);
                                    }

                                    if (usedQnty >= 0)
                                    {
                                        if (!DBNull.Value.Equals(dtByProductId.Rows[i]["QuantityLimit"]))
                                            availableLimit = Math.Max(Convert.ToInt32(dtByProductId.Rows[i]["QuantityLimit"]) - usedQnty, 0);
                                        else
                                            availableLimit = -1;//If QuantityLimit (daily limit) is not set, reset the available limit to -1
                                    }
                                }
                                else
                                {
                                    if (!DBNull.Value.Equals(dtByProductId.Rows[i]["QuantityLimit"]))
                                        availableLimit = Convert.ToInt32(dtByProductId.Rows[i]["QuantityLimit"]);
                                    else
                                        availableLimit = -1;
                                }

                                List<Transaction.TransactionLine> trxlineslist = null;
                                bool isLimitExceed = false;
                                //check by products
                                if (availableLimit != -1) //Begin Modification: on 23-01-2017 for checking if the available limit is null then considered there is not daily limit applied
                                {
                                    if (!DBNull.Value.Equals(dtByProductId.Rows[i]["ProductId"]))
                                    {
                                        trxlineslist = Transaction.TrxLines.FindAll(delegate (Transaction.TransactionLine trxObj)
                                        {
                                            return (trxObj.ProductID == Convert.ToInt32(dtByProductId.Rows[i]["ProductId"]) && (trxObj.LineValid));
                                        });
                                        if (trxlineslist.Count > 0)
                                        {
                                            if (trxlineslist.Count > availableLimit)
                                            {
                                                isLimitExceed = true;
                                            }
                                        }
                                    }
                                    else
                                    {//check by category
                                        if (productsByCategory != null && productsByCategory.Rows.Count > 0)
                                        {
                                            int totalTrxLines = 0;
                                            for (int j = 0; j < productsByCategory.Rows.Count; j++)
                                            {
                                                trxlineslist = Transaction.TrxLines.FindAll(delegate (Transaction.TransactionLine trxObj)
                                                {
                                                    return (trxObj.ProductID == (DBNull.Value.Equals(productsByCategory.Rows[j]["product_id"]) ? -1 : Convert.ToInt32(productsByCategory.Rows[j]["product_id"])) && (trxObj.LineValid));
                                                });
                                                if (trxlineslist.Count > 0)
                                                {
                                                    totalTrxLines += trxlineslist.Count;
                                                    if (totalTrxLines > availableLimit)
                                                    {
                                                        isLimitExceed = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //display warning message if the Daily Limit Exeeded
                                if (isLimitExceed)
                                {
                                    log.Info("Ends-apply() as " + MessageUtils.getMessage(1123, availableLimit));
                                    txtMessage.Text = MessageUtils.getMessage(1123, availableLimit);
                                    log.LogMethodExit(txtMessage.Text);
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error in -apply() while checking credit plus limit " + ex.Message);
                    }
                }
                //End Modification : 25-OCt-2016 for implementing creditplus under daily limit

                //end modfied code
                #endregion
                string message = "";
                bool voucherApplied = Transaction.ApplyVoucher(new Card(dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString(), "", Utilities), ref message);
                if (voucherApplied)
                {
                    POSUtils.SetLastActivityDateTime();
                    decimal postAmount = (decimal)Transaction.Net_Transaction_Amount;
                    POSUtils.ParafaitMessageBox("Voucher applied successfully. Transaction amount has changed from " + preAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT) + " to " + postAmount.ToString(POSStatic.ParafaitEnv.AMOUNT_FORMAT) + ". Please proceed with balance payment.");
                    log.Info("apply() as voucher Applied Successfully");

                    string lmessage = "";
                    if (0 != Transaction.SaveOrder(ref lmessage))
                    {
                        txtMessage.Text = lmessage;
                        log.Info("Ends-apply() as 0 != Transaction.SaveOrder error:" + lmessage);
                        log.LogMethodExit(txtMessage.Text);
                        return false;
                    }

                    TrxStatusChanged = true;
                    log.Info("Ends-apply() as voucherApplied");
                    log.LogMethodExit();
                    return true;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(message) == false)
                    {
                        POSUtils.ParafaitMessageBox(message);
                    }
                }
            }

            // default to first cc payment mode incase PREFERRED_NON_CASH_PAYMENT_MODE = cc payment
            if (creditCardAmount != 0
                && cmbCreditCards.SelectedIndex < 0
                && ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 2)
            {
                if (cmbCreditCards.Items.Count > 0)
                {
                    cmbCreditCards.SelectedIndex = 0;
                }
                else
                {
                    txtMessage.Text = MessageUtils.getMessage(178);
                    log.Info("Ends-apply() as Credit Card Type not Selected )");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }
            }

            POSUtils.SetLastActivityDateTime();
            double ccSurcharge = 0;
            double ccAmount = getCreditCardPaymentAmount(ref ccSurcharge);
            ccAmount = ccAmount - ccSurcharge;

            if (creditCardAmount > 0 && ccAmount == 0)
            {
                txtMessage.Text = MessageUtils.getMessage(178);
                txtCreditCardAmount.BackColor = Color.OrangeRed;
                log.Info("Ends-apply() as creditCardAmount > 0 && ccAmount == 0");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (Math.Round(creditCardAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round(ccAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero))
            {
                txtMessage.Text = MessageUtils.getMessage(174);
                txtCreditCardAmount.BackColor = Color.OrangeRed;
                log.Info("Ends-apply() as Credit Card amount mismatch. Click Auto Balance to correct.");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            txtOtherPaymentAmount.BackColor = txtCashAmount.BackColor;
            double otherAmount = getOtherPaymentAmount();

            POSUtils.SetLastActivityDateTime();
            if (OtherPaymentAmount > 0 && otherAmount == 0)
            {
                txtMessage.Text = MessageUtils.getMessage(182);
                txtOtherPaymentAmount.BackColor = Color.OrangeRed;
                log.Info("Ends-apply() as Other Payment Mode not Selected");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (Math.Round(OtherPaymentAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero) != Math.Round(otherAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero))
            {
                txtMessage.Text = MessageUtils.getMessage(175);
                txtOtherPaymentAmount.BackColor = Color.OrangeRed;
                log.Info("Ends-apply() as Other Payment amount mismatch. Click Auto Balance to correct.");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (ValidateGameCards() != 0)
            {
                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                    this.Close();
                    log.Info("Ends-apply() as ValidateGameCards Dialog is aborted");
                    log.LogMethodExit();
                    return false;
                }

                txtGameCardAmount.BackColor = Color.OrangeRed;
                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(183));

                log.Info("Ends-apply() as Insufficient Credits on Game Card(s)");
                log.LogMethodExit(MessageUtils.getMessage(183));
                return false;
            }

            POSUtils.SetLastActivityDateTime();
            if (cmbOtherModes.SelectedIndex >= 0 && otherAmount == 0)
            {
                txtMessage.Text = MessageUtils.getMessage(176);
                txtOtherPaymentAmount.BackColor = Color.OrangeRed;
                this.ActiveControl = txtOtherPaymentAmount;
                log.Info("Ends-apply() as Other Payment Mode amount not entered");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (cmbCreditCards.SelectedIndex >= 0 && ccAmount == 0)//2017-05-13//Modification on 20-Oct-2016 for adding firstdata tip feature
            {
                txtMessage.Text = MessageUtils.getMessage(177);
                txtCreditCardAmount.BackColor = Color.OrangeRed;
                this.ActiveControl = txtCreditCardAmount;
                log.Info("Ends-apply() as Credit Card amount not entered");
                log.LogMethodExit(txtMessage.Text);
                return false;
            }

            if (creditCardAmount != 0)
            {
                POSUtils.SetLastActivityDateTime();
                if (cmbCreditCards.SelectedIndex < 0)
                {
                    txtMessage.Text = MessageUtils.getMessage(178);
                    log.Info("Ends-apply() as Credit Card Type not selected ");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }

                if (ParafaitEnv.CREDITCARD_DETAILS_MANDATORY == "Y" && (cmbCreditCards.Tag == null || cmbCreditCards.Tag.ToString() == "")) //Added check to handle empty scenario
                {
                    if (string.IsNullOrEmpty(txtCCNumber.Text.Trim()))
                    {
                        txtMessage.Text = MessageUtils.getMessage(179);
                        log.Info("Ends-apply() as Credit Card Number not Entered ");
                        log.LogMethodExit(txtMessage.Text);
                        return false;
                    }

                    if (string.IsNullOrEmpty(txtCCExpiry.Text.Trim()))
                    {
                        txtMessage.Text = MessageUtils.getMessage(180);
                        log.Info("Ends-apply() as Credit Card Expiry Month / Year not Entered ");
                        log.LogMethodExit(txtMessage.Text);
                        return false;
                    }

                    if (string.IsNullOrEmpty(txtNameOnCC.Text.Trim()))
                    {
                        txtMessage.Text = MessageUtils.getMessage(181);
                        log.Info("Ends-apply() as not Entered Name on Credit Card");
                        log.LogMethodExit(txtMessage.Text);
                        return false;
                    }
                }
            }

            if (OtherPaymentAmount > 0)
            {
                if (cmbOtherModes.SelectedIndex < 0)
                {
                    txtMessage.Text = MessageUtils.getMessage(182);
                    log.Info("Ends-apply() as Selected Other Payment Mode");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }
            }

            POSUtils.SetLastActivityDateTime();
            if (POSStatic.POPUP_FAKE_NOTE_DETECTION_ALERT
                && cashAmount > 0
                && POSUtils.ParafaitMessageBox(MessageUtils.getMessage(529), "Fake Note Detection") == System.Windows.Forms.DialogResult.No)
            {
                log.Info("Ends-apply() as Fake Note Detection verification");
                log.LogMethodExit();
                return false;
            }
            if (gameCardAmount != 0)
            {
                if (Transaction.Order != null && Transaction.Order.OrderHeaderDTO != null
                && Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == POSStatic.transactionOrderTypes["Refund"] && gameCardAmount < 0
                && Transaction.TransactionLineList.Exists(x => (bool)(x.ProductTypeCode.Equals("REFUND") && x.CardNumber.Equals(dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString()))))
                {
                    txtMessage.Text = MessageUtils.getMessage(2726);
                    log.Info("Refunding to the same card is not allowed");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }

                Card card = new Card(dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString(), Utilities.ParafaitEnv.LoginID, Utilities);
                if (Transaction.Order != null && Transaction.Order.OrderHeaderDTO != null
                    && (Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == POSStatic.transactionOrderTypes["Refund"] || Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == POSStatic.transactionOrderTypes["Item Refund"])
                    && gameCardAmount < 0 && card.technician_card.Equals("Y"))
                {
                    txtMessage.Text = MessageUtils.getMessage(2727);
                    log.Info("Refunding to the technician card is not allowed");
                    log.LogMethodExit(txtMessage.Text);
                    return false;
                }
            }
            btnSave.Enabled = btnCancel.Enabled = false;

            try
            {
                POSUtils.SetLastActivityDateTime();
                if (creditCardAmount != 0)
                {
                    string VCATAuth = string.Empty;
                    Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                    TransactionPaymentsDTO creditTransactionPaymentsDTO = new TransactionPaymentsDTO();
                    creditTransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(cmbCreditCards.SelectedValue);
                    creditTransactionPaymentsDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Convert.ToInt32(cmbCreditCards.SelectedValue)).GetPaymentModeDTO;
                    try
                    {
                        String paymentModeOTPValue = PerformPaymentModeOTPValidation(creditTransactionPaymentsDTO.paymentModeDTO);
                        creditTransactionPaymentsDTO.PaymentModeOTP = paymentModeOTPValue;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                        return false;
                    }

                    POSUtils.SetLastActivityDateTime();
                    creditTransactionPaymentsDTO = GetEnabledAttributeDataForPaymentMode(creditTransactionPaymentsDTO);
                    if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                    {
                        FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                        FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(Utilities.getParafaitDefaults("FISCAL_PRINTER"));
                        string Message = string.Empty;
                        string printOption = string.Empty;
                        FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
                        List<PaymentInfo> payItemList = new List<PaymentInfo>();
                        PaymentInfo paymentInfo = new PaymentInfo();
                        paymentInfo.amount = Convert.ToDecimal(creditCardAmount);
                        paymentInfo.paymentMode = "CreditCard";
                        payItemList.Add(paymentInfo);
                        fiscalizationRequest.payments = payItemList.ToArray();
                        int installmentMonth = GetSmartroInstallmentMonth();
                        if (installmentMonth < 0)
                        {
                            log.Debug("Customer not entered valid installment month. Cannot proceed");
                            return false;
                        }
                        paymentInfo.quantity = installmentMonth;
                        if (fiscalPrinter.IsConfirmationRequired(fiscalizationRequest))
                        {
                            printOption = GetSmartroPrintOption();
                            if (string.IsNullOrWhiteSpace(printOption))
                            {
                                log.Error("Credit card Payment Failed");
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(4244), "Payment");
                                return false;
                            }
                            payItemList.First().description = printOption;
                        }

                        POSUtils.SetLastActivityDateTime();
                        Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new TransactionLine();
                        if (Transaction != null && Transaction.TrxLines != null && Transaction.TrxLines.Any())
                        {
                            transactionLine.VATRate = Convert.ToDecimal(Transaction.TrxLines.First().tax_percentage);
                            log.Debug("VATRate :" + transactionLine.VATRate);
                            if (transactionLine.VATRate > 0)
                            {
                                //creditCardAmount is inclusive of tax amount. 
                                transactionLine.VATAmount = (Convert.ToDecimal(creditCardAmount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                                log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                                if (transactionLine.VATAmount % 1 > 0)
                                {
                                    transactionLine.VATAmount = (decimal)POSStatic.CommonFuncs.RoundOff(Convert.ToDouble(transactionLine.VATAmount), Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                    log.Debug("transactionLine.VATAmount after rounding:" + transactionLine.VATAmount);
                                }
                            }
                            else
                            {
                                transactionLine.VATAmount = 0;
                                log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                            }
                        }
                        fiscalizationRequest.transactionLines = new TransactionLine[] { transactionLine };
                        POSUtils.SetLastActivityDateTime();
                        bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
                        POSUtils.SetLastActivityDateTime();
                        if (success)
                        {
                            if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                            {
                                txtCCNumber.Text = fiscalizationRequest.payments[0].description;
                                creditTransactionPaymentsDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                                creditTransactionPaymentsDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description;
                                creditTransactionPaymentsDTO.CreditCardName = fiscalizationRequest.transactionLines.ToList().First().description;
                                creditTransactionPaymentsDTO.CreditCardExpiry = txtCCExpiry.Text;
                                if (installmentMonth.ToString().Length < 2)
                                {
                                    creditTransactionPaymentsDTO.Reference = "0" + installmentMonth;
                                    log.LogVariableState("installtionMonth after appending 0 : ", creditTransactionPaymentsDTO.Reference);
                                }
                                else
                                {
                                    creditTransactionPaymentsDTO.Reference = installmentMonth.ToString();
                                }
                                creditTransactionPaymentsDTO.CreditCardNumber = fiscalizationRequest.payments[0].description;
                            }
                            else
                            {
                                log.Error("Credit card Payment Failed");
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(Message), "Payment");
                                return false;
                            }
                        }
                        else
                        {
                            log.Error("Credit card Payment Failed");
                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(Message), "Payment");
                            return false;
                        }
                    }
                    else
                    {
                        POSUtils.SetLastActivityDateTime();
                        creditTransactionPaymentsDTO.CreditCardAuthorization = txtCCAuthorization.Text;
                        creditTransactionPaymentsDTO.CreditCardExpiry = txtCCExpiry.Text;
                        creditTransactionPaymentsDTO.CreditCardNumber = txtCCNumber.Text;
                        creditTransactionPaymentsDTO.CreditCardAuthorization = txtCCAuthorization.Text;
                        creditTransactionPaymentsDTO.Reference = txtPaymentReference.Text;
                        if (!string.IsNullOrEmpty(QRCode))
                        {
                            creditTransactionPaymentsDTO.CreditCardNumber = QRCode;
                        }
                        else if (!string.IsNullOrEmpty(giftCardNumber))
                        {
                            creditTransactionPaymentsDTO.CreditCardNumber = giftCardNumber;//clubspeed changes
                        }
                        else
                        {
                            creditTransactionPaymentsDTO.CreditCardNumber = txtCCNumber.Text;
                        }
                        //ChinaICBC changes
                         //if (!string.IsNullOrEmpty(giftCardPIN))
                         //{
                         //    creditTransactionPaymentsDTO.CreditCardAuthorization = giftCardPIN;
                         //}
                    //creditTransactionPaymentsDTO.CreditCardNumber = (!string.IsNullOrEmpty(QRCode)) ? QRCode : txtCCNumber.Text;//ChinaICBC changes
                }

                creditTransactionPaymentsDTO.CreditCardAuthorization = txtCCAuthorization.Text;
                if (!string.IsNullOrEmpty(giftCardPIN))
                {
                    creditTransactionPaymentsDTO.CreditCardAuthorization = giftCardPIN;
                }
                creditTransactionPaymentsDTO.CreditCardExpiry = txtCCExpiry.Text;
                creditTransactionPaymentsDTO.CreditCardName = txtCCName.Text;
                if (Transaction.Trx_id > 0)
                {
                    creditTransactionPaymentsDTO.TransactionId = Transaction.Trx_id;
                }
                creditTransactionPaymentsDTO.Amount = Convert.ToDouble(txtCreditCardAmount.Text);
                creditTransactionPaymentsDTO.TipAmount = CreditTip;//Modification on 09-Nov-2015: Tip Feature
                if (!string.IsNullOrEmpty(txtCCSurchPercent.Text.Trim()))
                {
                    ccSurcharge = creditTransactionPaymentsDTO.Amount * Convert.ToDouble(txtCCSurchPercent.Text.Trim()) / 100.0;
                    ccSurcharge = Math.Round(ccSurcharge, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                    if (ccSurcharge < 0)
                        ccSurcharge = 0;
                    creditTransactionPaymentsDTO.Amount += ccSurcharge;
                }
                creditTransactionPaymentsDTO.SplitId = PaymentSplitId;
                creditTransactionPaymentsDTO.NameOnCreditCard = txtNameOnCC.Text;

                if (creditCardModeApprovedBy > -1)
                {
                    Users ccApprovedBy = new Users(Utilities.ExecutionContext, creditCardModeApprovedBy);
                    creditTransactionPaymentsDTO.ApprovedBy = ccApprovedBy.UserDTO.LoginId;
                    creditCardModeApprovedBy = -1;
                }
                if (cmbCreditCards.Tag != null && Enum.IsDefined(typeof(PaymentGateways), cmbCreditCards.Tag.ToString()))
                {
                    creditTransactionPaymentsDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), cmbCreditCards.Tag.ToString(), true);
                    //TrxStatusChanged = true; //2.80.0
                }
                else
                    creditTransactionPaymentsDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;

                if (Transaction.HasSubscriptionProducts())
                {
                    creditTransactionPaymentsDTO.SubscriptionAuthorizationMode = SubscriptionAuthorizationMode.I;
                    creditTransactionPaymentsDTO.CustomerCardProfileId = GetTrxCustomerGuid();
                }
                Transaction.TransactionPaymentsDTOList.Add(creditTransactionPaymentsDTO);
            }

                POSUtils.SetLastActivityDateTime();
                if (OtherPaymentAmount != 0)
                {
                    TransactionPaymentsDTO otherTransactionPaymentsDTO = new TransactionPaymentsDTO();
                    otherTransactionPaymentsDTO.Amount = Convert.ToDouble(txtOtherPaymentAmount.Text);
                    otherTransactionPaymentsDTO.SplitId = PaymentSplitId;
                    //start: Modification on 11-may-2016 for Adding couponNumber
                    if (couponUsed != string.Empty)
                    {
                        otherTransactionPaymentsDTO.Reference = couponUsed;
                        otherTransactionPaymentsDTO.CouponSetId = paymentCouponSetId;
                        paymentCouponSetId = -1;
                        couponUsed = string.Empty;
                    }
                    else if (isPointsPayment) //start: Modification on 28-June-2016 for Adding ValidationCode
                    {
                        otherTransactionPaymentsDTO.Reference = validationCode;
                    }
                    else
                    {
                        otherTransactionPaymentsDTO.Reference = txtOtherPaymentReference.Text;
                    }
                    //end 
                    if (otherModeApprovedBy > -1)
                    {
                        Users ccApprovedBy = new Users(Utilities.ExecutionContext, otherModeApprovedBy);
                        otherTransactionPaymentsDTO.ApprovedBy = ccApprovedBy.UserDTO.LoginId;
                        otherModeApprovedBy = -1;
                    }
                    otherTransactionPaymentsDTO.PaymentModeId = Convert.ToInt32(cmbOtherModes.SelectedValue);
                    otherTransactionPaymentsDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Convert.ToInt32(cmbOtherModes.SelectedValue)).GetPaymentModeDTO;
                    try
                    {
                        String paymentModeOTPValue = PerformPaymentModeOTPValidation(otherTransactionPaymentsDTO.paymentModeDTO);
                        otherTransactionPaymentsDTO.PaymentModeOTP = paymentModeOTPValue;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                        return false;
                    }

                    otherTransactionPaymentsDTO = GetEnabledAttributeDataForPaymentMode(otherTransactionPaymentsDTO);

                    POSUtils.SetLastActivityDateTime();
                    if (string.IsNullOrWhiteSpace(txtCouponValue.Text) == false)
                    {
                        double couponValue;
                        if (double.TryParse(txtCouponValue.Text, out couponValue))
                        {
                            otherTransactionPaymentsDTO.CouponValue = couponValue;
                        }
                        else
                        {
                            otherTransactionPaymentsDTO.CouponValue = null;
                        }
                    }
                    else
                    {
                        otherTransactionPaymentsDTO.CouponValue = null;
                    }
                    if (chbIsTaxable.Checked)
                    {
                        otherTransactionPaymentsDTO.IsTaxable = true;
                    }
                    else
                    {
                        otherTransactionPaymentsDTO.IsTaxable = null;
                    }
                    if (string.IsNullOrEmpty(otherTransactionPaymentsDTO.Reference.Trim()))
                        otherTransactionPaymentsDTO.Reference = txtPaymentReference.Text.Trim();
                    Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                    Transaction.TransactionPaymentsDTOList.Add(otherTransactionPaymentsDTO);
                }

                POSUtils.SetLastActivityDateTime();
                if (cashAmount != 0)
                {
                    PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    if (paymentModeId > 0)
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeId.ToString()));
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    //In case paymentMode Id is not available, go with existing approach of top cash payment mode
                    if (paymentModeDTOList == null
                        ||
                        (paymentModeDTOList != null && paymentModeDTOList.Count <= 0)
                       )
                    {
                        searchParameters.Clear();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    }
                    if (paymentModeDTOList != null && paymentModeDTOList.Count > 0)
                    {
                        Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                        TransactionPaymentsDTO cashTrxPaymentDTO = new TransactionPaymentsDTO();
                        cashTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                        cashTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        try
                        {
                            String paymentModeOTPValue = PerformPaymentModeOTPValidation(cashTrxPaymentDTO.paymentModeDTO);
                            cashTrxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                            return false;
                        }
                        cashTrxPaymentDTO.Amount = cashAmount;
                        cashTrxPaymentDTO.SplitId = PaymentSplitId;
                        cashTrxPaymentDTO.TipAmount = CashTip;
                        cashTrxPaymentDTO.CurrencyCode = currencyCode;
                        cashTrxPaymentDTO.CurrencyRate = currencyRate;
                        cashTrxPaymentDTO.IsTaxable = null;
                        if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
                        {
                            FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                            FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(Utilities.getParafaitDefaults("FISCAL_PRINTER"));
                            string printOption = string.Empty;
                            decimal smartroAmount = decimal.Parse(cashAmount.ToString());
                            log.Debug("smartroAmount:" + smartroAmount);
                            if (smartroAmount % 1 > 0)
                            {
                                smartroAmount = (decimal)POSStatic.CommonFuncs.RoundOff(cashAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                            }
                            string Message = string.Empty;
                            FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
                            List<PaymentInfo> payItemList = new List<PaymentInfo>();
                            PaymentInfo paymentInfo = new PaymentInfo();
                            paymentInfo.amount = smartroAmount;
                            paymentInfo.paymentMode = "Cash";
                            payItemList.Add(paymentInfo);
                            fiscalizationRequest.payments = payItemList.ToArray();
                            if (fiscalPrinter.IsConfirmationRequired(fiscalizationRequest))
                            {
                                printOption = GetSmartroPrintOption();
                                if (string.IsNullOrWhiteSpace(printOption))
                                {
                                    log.Error("Cash Payment Failed");
                                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(4243), "Payment");
                                    return false;
                                }
                                payItemList.First().description = printOption;
                            }
                            Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new TransactionLine();
                            if (Transaction != null && Transaction.TrxLines != null && Transaction.TrxLines.Any())
                            {
                                transactionLine.VATRate = Convert.ToDecimal(Transaction.TrxLines.First().tax_percentage);
                                log.Debug("VATRate :" + transactionLine.VATRate);
                                if (transactionLine.VATRate > 0)
                                {
                                    //creditCardAmount is inclusive of tax amount. 
                                    transactionLine.VATAmount = (Convert.ToDecimal(smartroAmount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                                    if (transactionLine.VATAmount % 1 > 0)
                                    {
                                        transactionLine.VATAmount = (decimal)POSStatic.CommonFuncs.RoundOff(Convert.ToDouble(transactionLine.VATAmount), Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                        log.Debug("transactionLine.VATAmount after rounding:" + transactionLine.VATAmount);
                                    }
                                }
                                else
                                {
                                    transactionLine.VATAmount = 0;
                                    log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                                }
                            }
                            fiscalizationRequest.transactionLines = new TransactionLine[] { transactionLine };
                            POSUtils.SetLastActivityDateTime();
                            bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
                            POSUtils.SetLastActivityDateTime();
                            if (success)
                            {
                                if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                                {
                                    cashTrxPaymentDTO.Reference = printOption;
                                    cashTrxPaymentDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                                    cashTrxPaymentDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description;// Phone number 123456 ****
                                }
                                else
                                {
                                    log.Error("Cash Payment Failed");
                                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(Message), "Payment");
                                    return false;
                                }
                            }
                            else
                            {
                                log.Error("Cash Payment Failed");
                                POSUtils.ParafaitMessageBox(MessageUtils.getMessage(Message), "Payment");
                                return false;
                            }

                        }

                        cashTrxPaymentDTO = GetEnabledAttributeDataForPaymentMode(cashTrxPaymentDTO);

                        //TenderedAmount = Math.Max(TenderedAmount, cashAmount);//rounding fix 29-Apr-2019
                        //rounding fix 29-Apr-2019
                        if ((decimal)TenderedAmount == (decimal)POSStatic.CommonFuncs.RoundOff(cashAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType))
                        {
                            cashTrxPaymentDTO.TenderedAmount = TenderedAmount;//rounding fix 29-Apr-2019
                        }
                        else//rounding fix 29-Apr-2019
                        {
                            TenderedAmount = Math.Max(TenderedAmount, cashAmount);
                            cashTrxPaymentDTO.TenderedAmount = Math.Max(TenderedAmount, cashAmount);
                        }
                        if (string.IsNullOrWhiteSpace(cashTrxPaymentDTO.Reference))
                            cashTrxPaymentDTO.Reference = txtPaymentReference.Text.Trim();

                        Transaction.TransactionPaymentsDTOList.Add(cashTrxPaymentDTO);
                        currencyCode = string.Empty;
                        currencyRate = null;
                    }
                }

                POSUtils.SetLastActivityDateTime();
                if (gameCardAmount != 0)
                {
                    PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if (paymentModeDTOList != null)
                    {
                        PaymentModeDTO selectedPaymentModeDTO = GetSelectedPaymentMode(paymentModeDTOList);
                        Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                        TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                        debitTrxPaymentDTO.PaymentModeId = paymentModeDTOList[0].PaymentModeId;
                        debitTrxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];
                        try
                        {
                            String paymentModeOTPValue = PerformPaymentModeOTPValidation(debitTrxPaymentDTO.paymentModeDTO);
                            debitTrxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                            return false;
                        }
                        debitTrxPaymentDTO.PaymentModeId = selectedPaymentModeDTO.PaymentModeId;
                        debitTrxPaymentDTO.paymentModeDTO = selectedPaymentModeDTO;

                        debitTrxPaymentDTO = GetEnabledAttributeDataForPaymentMode(debitTrxPaymentDTO);

                        debitTrxPaymentDTO.Amount = Convert.ToDouble(dgvGameCards.Rows[0].Cells["UsedCredits"].Value);
                        debitTrxPaymentDTO.CardId = Convert.ToInt32(dgvGameCards.Rows[0].Cells["CardId"].Value);
                        debitTrxPaymentDTO.CardEntitlementType = "C";
                        debitTrxPaymentDTO.SplitId = PaymentSplitId;
                        debitTrxPaymentDTO.PaymentUsedCreditPlus = Convert.ToDouble(dgvGameCards.Rows[0].Cells["UsedCreditPlus"].Value);
                        debitTrxPaymentDTO.PaymentCardNumber = dgvGameCards.Rows[0].Cells["CardNumber"].Value.ToString();
                        debitTrxPaymentDTO.Reference = txtPaymentReference.Text.Trim();
                        Transaction.TransactionPaymentsDTOList.Add(debitTrxPaymentDTO);
                    }
                }

                POSUtils.SetLastActivityDateTime();
                Transaction.CreateRoundOffPayment();
                if (PaymentSplitId != -1
                    && Transaction.TransactionPaymentsDTOList != null
                    && Transaction.TransactionPaymentsDTOList
                                  .Exists(x => x.PaymentId == -1
                                               && x.paymentModeDTO != null
                                               && x.paymentModeDTO.IsRoundOff)
                   )
                {
                    Transaction.TransactionPaymentsDTOList
                               .Where(x => x.PaymentId == -1
                                      && x.paymentModeDTO != null
                                      && x.paymentModeDTO.IsRoundOff).LastOrDefault().SplitId = PaymentSplitId;
                }

                //08-May-2016 :: If there is difference in transaction amount then add rounding payment mode to balance it out
                if (TrxSplitDiff != 0 && (cashAmount != 0 || creditCardAmount != 0 || OtherPaymentAmount != 0))
                {
                    bool found = false;
                    foreach (TransactionPaymentsDTO roundingTrxPaymentDTO in Transaction.TransactionPaymentsDTOList)
                    {
                        if (Convert.ToInt32(roundingTrxPaymentDTO.PaymentModeId) == Utilities.ParafaitEnv.RoundOffPaymentModeId)
                        {
                            roundingTrxPaymentDTO.Amount += TrxSplitDiff;
                            found = true;
                            break;
                        }
                    }
                    if (!found && Utilities.ParafaitEnv.RoundOffPaymentModeId != -1)
                    {
                        TransactionPaymentsDTO roundingTrxPaymentDTO = new TransactionPaymentsDTO();
                        roundingTrxPaymentDTO.PaymentModeId = Utilities.ParafaitEnv.RoundOffPaymentModeId;
                        roundingTrxPaymentDTO.Reference = "";
                        roundingTrxPaymentDTO.Amount = TrxSplitDiff;
                        roundingTrxPaymentDTO.SplitId = PaymentSplitId;
                        roundingTrxPaymentDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Utilities.ParafaitEnv.RoundOffPaymentModeId).GetPaymentModeDTO;
                        try
                        {
                            POSUtils.SetLastActivityDateTime();
                            String paymentModeOTPValue = PerformPaymentModeOTPValidation(roundingTrxPaymentDTO.paymentModeDTO);
                            roundingTrxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            txtMessage.Text = ex.Message;
                            log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                            return false;
                        }

                        roundingTrxPaymentDTO = GetEnabledAttributeDataForPaymentMode(roundingTrxPaymentDTO);

                        Transaction.TransactionPaymentsDTOList.Add(roundingTrxPaymentDTO);
                    }

                    TrxSplitDiff = 0;
                }

                Transaction.PaymentReference = txtPaymentReference.Text;

                //if (!Transaction.isSavedTransaction())
                //{
                //    log.Debug("Inside apply() when trx is not saved transaction : Start ");
                //    string lmessage = "";
                //    if(0 != Transaction.SaveOrder(ref lmessage))
                //    {
                //        txtMessage.Text = lmessage;
                //        log.Info("Ends-apply() as unable to Save Order Transaction error: " + lmessage);
                //        return false;
                //    }
                //    log.Debug("Inside apply() when trx is not saved transaction : End ");
                //}
                //else
                //{
                log.Debug("Inside apply() within isSavedTransaction condition: Start ");
                string lmessage = "";
                using (SqlConnection cnn = Utilities.createConnection())
                {
                    SqlTransaction sqlTrx = cnn.BeginTransaction();
                    try
                    {
                        TransactionPaymentsDTO trxPaymentDTO = Transaction.TransactionPaymentsDTOList.FindLast(x => x.paymentModeDTO != null
                                                                                                                && x.paymentModeDTO.IsCreditCard
                                                                                                                && x.PaymentId == -1
                                                                                                                && !x.GatewayPaymentProcessed);
                        if (trxPaymentDTO != null)
                        {
                            if (trxPaymentDTO.Amount != 0)
                            {
                                string hasGratuityLine = Transaction.GetActiveLineForType(ProductTypeValues.GRATUITY, null) != null ? "G" : String.Empty;
                                trxPaymentDTO.ExternalSourceReference = hasGratuityLine;
                                try
                                {
                                    DisableButtons();
                                    if (!CreditCardPaymentGateway.MakePayment(trxPaymentDTO, Utilities, ref lmessage))
                                    {
                                        if (sqlTrx != null && sqlTrx.Connection != null)
                                            sqlTrx.Rollback();
                                        txtMessage.Text = lmessage;
                                        displayMessageLine(lmessage, ERROR);
                                        log.Info("Ends-apply() as unable to Make CC payment " + lmessage);
                                        log.LogMethodExit(txtMessage.Text);
                                        Transaction.TransactionPaymentsDTOList.Remove(trxPaymentDTO);
                                        return false;
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(txtCCSurchPercent.Text.Trim()))
                                        {
                                            if (trxPaymentDTO.Amount != 0) //Pre auth will reset amount to zero.
                                            {
                                                Transaction.PaymentCreditCardSurchargeAmount = ccSurcharge;
                                                PaymentCreditCardSurchargeAmount = ccSurcharge;
                                            }
                                        }
                                    }
                                }
                                finally
                                {
                                    EnableButtons();
                                }
                                trxPaymentDTO.ExternalSourceReference = null;
                            }
                            else
                            {
                                if (sqlTrx != null && sqlTrx.Connection != null)
                                    sqlTrx.Rollback();
                                txtMessage.Text = lmessage;
                                displayMessageLine(lmessage, ERROR);
                                log.Info("Ends-apply() as unable to Make CC payment " + lmessage);
                                log.LogMethodExit(txtMessage.Text);
                                return false;
                            }
                        }
                        POSUtils.SetLastActivityDateTime();
                        if (Transaction.isSavedTransaction() == false)
                        {
                            POSUtils.SetLastActivityDateTime();
                            if (0 != CallTransactionSaveOrder(ref lmessage, sqlTrx))
                            {
                                POSUtils.SetLastActivityDateTime();
                                if (trxPaymentDTO != null)
                                {
                                    if (!string.IsNullOrEmpty(txtCCSurchPercent.Text.Trim()) && Transaction.PaymentCreditCardSurchargeAmount != 0)
                                    {
                                        Transaction.PaymentCreditCardSurchargeAmount = 0;
                                        PaymentCreditCardSurchargeAmount = 0;
                                    }
                                    try
                                    {
                                        DisableButtons();
                                        if (!CreditCardPaymentGateway.RefundAmount(trxPaymentDTO, Utilities, ref lmessage))
                                        {
                                            if (sqlTrx != null && sqlTrx.Connection != null)
                                                sqlTrx.Rollback();
                                            POSUtils.ParafaitMessageBox(MessageUtils.getMessage(187, lmessage), "Remove Payment");
                                            displayMessageLine(lmessage, ERROR);
                                            log.Error("Cannot reverse payment as Transaction save failed: " + lmessage);
                                            log.LogMethodExit();
                                            return false;
                                        }
                                    }
                                    finally
                                    {
                                        EnableButtons();
                                    }
                                    POSUtils.SetLastActivityDateTime();
                                    if (!string.IsNullOrEmpty(trxPaymentDTO.Reference))
                                    {
                                        if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT").Equals("Y") ||
                                            ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT").Equals("Y"))
                                        {
                                            try
                                            {
                                                DisableButtons();
                                                string printOption = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CC_PAYMENT_RECEIPT_PRINT"));
                                                if (printOption == "Y")
                                                {
                                                    POSUtils.SetLastActivityDateTime();
                                                    CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO);
                                                }
                                                else if (printOption == "A")
                                                {
                                                    if (MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 484), "Credit Card Payment Receipt Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                    {
                                                        POSUtils.SetLastActivityDateTime();
                                                        CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO);
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                                EnableButtons();
                                            }
                                        }
                                    }

                                    POSUtils.SetLastActivityDateTime();
                                }
                                if (sqlTrx != null && sqlTrx.Connection != null)
                                    sqlTrx.Rollback();
                                txtMessage.Text = lmessage;
                                log.Info("Ends-apply() as unable to Save Order Transaction error: " + lmessage);
                                log.LogMethodExit(txtMessage.Text);
                                return false;
                            }
                        }
                        else
                        {
                            POSUtils.SetLastActivityDateTime();
                            if (!CallTrxCreatePaymentInfo(ref lmessage, sqlTrx))
                            {
                                if (!string.IsNullOrEmpty(txtCCSurchPercent.Text.Trim()) && Transaction.PaymentCreditCardSurchargeAmount != 0)
                                {
                                    Transaction.PaymentCreditCardSurchargeAmount = 0;
                                    PaymentCreditCardSurchargeAmount = 0;
                                }
                                POSUtils.SetLastActivityDateTime();
                                sqlTrx.Rollback();
                                txtMessage.Text = lmessage;
                                log.Info("Ends-apply() as unable to CreatePaymentInfo " + lmessage);
                                log.LogMethodExit(txtMessage.Text);
                                return false;
                            }
                            POSUtils.SetLastActivityDateTime();
                        }
                        POSUtils.SetLastActivityDateTime();
                        frmVerifyPaymentModeOTP.CreateTrxUsrLogEntryForPaymentOTPValidationOverride(paymentModeOTPApprovals, Transaction, Utilities.ParafaitEnv.LoginID, sqlTrx);
                        sqlTrx.Commit();
                        //Call CC Print method
                        if (trxPaymentDTO != null && trxPaymentDTO.Amount != 0)
                        {
                            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_MERCHANT_RECEIPT").Equals("Y") ||
                                ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "PRINT_CUSTOMER_RECEIPT").Equals("Y"))
                            {
                                string hasGratuityLine = Transaction.GetActiveLineForType(ProductTypeValues.GRATUITY, null) != null ? "G" : String.Empty;
                                trxPaymentDTO.ExternalSourceReference = hasGratuityLine;
                                try
                                {
                                    DisableButtons();
                                    string printOption = (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CC_PAYMENT_RECEIPT_PRINT"));
                                    if (printOption == "Y")
                                    {
                                        CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO); //Print the receipt after saving transaction
                                    }
                                    else if (printOption == "A")
                                    {
                                        if (MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 484), "Credit Card Payment Receipt Print", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            CreditCardPaymentGateway.PrintCCReceipt(trxPaymentDTO); //Print the receipt after saving transaction
                                        }
                                    }
                                }
                                finally
                                {
                                    EnableButtons();
                                }
                                trxPaymentDTO.ExternalSourceReference = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (sqlTrx != null && sqlTrx.Connection != null)
                            sqlTrx.Rollback();
                        txtMessage.Text = ex.Message;
                        log.Fatal("Ends-apply() due to exception " + ex.Message);
                        log.LogMethodExit(txtMessage.Text);
                        return false;
                    }
                    // }
                    log.Debug("Inside apply() within isSavedTransaction condition: End ");
                }

                POSUtils.SetLastActivityDateTime();
                TrxStatusChanged = true;
                if (isCouponPayment)
                    isCouponPayment = false; //reset isCouponPayment flag once payment is applied
                CashAmount = Transaction.TransactionPaymentsDTOList.Where(x => x.paymentModeDTO != null
                                                                            && x.paymentModeDTO.IsCash).Sum(x => x.Amount);
                QRCode = string.Empty;
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Fatal("Ends-apply() due to exception " + ex.ToString());
                log.LogMethodExit(txtMessage.Text);
                return false;
            }
            finally
            {
                btnSave.Enabled = btnCancel.Enabled = true;
                this.Activate();
                POSUtils.SetLastActivityDateTime();
            }

            log.Debug("Ends-apply()");

            CashTip = CreditTip = 0;

            if (balanceAmount == 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            log.LogMethodExit("true");
            return true;
        }

        private int GetSmartroInstallmentMonth()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            int installmentMonth = 0;
            bool enableinstallmentMonth = ParafaitDefaultViewContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_INSTALLMENT_MONTH_SELECTION");
            log.Debug("enableinstallmentMonth: " + enableinstallmentMonth);
            if (enableinstallmentMonth)
            {
                ParafaitPOS.App.machineUserContext = Utilities.ExecutionContext;
                ParafaitPOS.App.EnsureApplicationResources();
                GenericDataEntryVM dataEntryVM = new GenericDataEntryVM(Utilities.ExecutionContext)
                {
                    Heading = MessageViewContainerList.GetMessage(Utilities.ExecutionContext, "Installment Month Option"),
                    DataEntryCollections = new ObservableCollection<DataEntryElement>()
                        {
                            new DataEntryElement()
                            {
                                Heading =MessageViewContainerList.GetMessage(Utilities.ExecutionContext,"Installment Month"),
                                Type = DataEntryType.TextBox,
                                DefaultValue = "00", Text ="0",
                                IsMandatory= true,
                                ValidationType = Semnox.Parafait.CommonUI.ValidationType.NumberOnly,
                                IsEditable=true
                            },
                            new DataEntryElement()
                            {
                                Type = DataEntryType.TextBlock,IsReadOnly= true,
                                Size = Semnox.Parafait.CommonUI.Size.XSmall,
                                DefaultValue = MessageViewContainerList.GetMessage(Utilities.ExecutionContext,"Enter value between 0 to 60"),
                                Text =MessageViewContainerList.GetMessage(Utilities.ExecutionContext,"Enter value between 0 to 60"),
                                IsMandatory= false,
                                IsEditable=false
                            },
                        }
                };

                POSUtils.SetLastActivityDateTime();
                GenericDataEntryView dataEntryView = new GenericDataEntryView();
                dataEntryView.DataContext = dataEntryVM;
                dataEntryView.ShowDialog();
                if (dataEntryVM.ButtonClickType == ButtonClickType.Ok && dataEntryVM.DataEntryCollections.Any())
                {
                    string installmentMonthEntered = dataEntryVM.DataEntryCollections[0].Text;
                    if (string.IsNullOrWhiteSpace(installmentMonthEntered))
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 1773, "Installment Month"));
                        return -1;
                    }
                    if (!int.TryParse(installmentMonthEntered, out installmentMonth) || installmentMonth < 0 || installmentMonth > 60)
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 1773, "Installment Month"));
                        return -1;
                    }
                }
                else if (dataEntryVM.ButtonClickType == ButtonClickType.Cancel)
                {
                    log.Debug("Clicked cancel for installment month");
                    dataEntryView.Close();
                    return -1;
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(installmentMonth);
            return installmentMonth;
        }
        private string GetTrxCustomerGuid()
        {
            log.LogMethodEntry();
            try
            {
                string customerGuid = Transaction.customerDTO.Guid;
                log.LogMethodExit(customerGuid);
                return customerGuid;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                throw new Exception("Save Customer details before Initiating Subscription Payment");
            }
        }
        private string GetSmartroPrintOption()
        {
            log.LogMethodEntry();
            string printOption = string.Empty;
            VCATPrintOptionView vCATPrintOptionView = null;
            try
            {
                POSUtils.SetLastActivityDateTime();
                ParafaitPOS.App.machineUserContext = ParafaitEnv.ExecutionContext;
                ParafaitPOS.App.EnsureApplicationResources();
                VCATPrintOptionVM vCATPrintOptionVM = null;
                try
                {
                    vCATPrintOptionVM = new VCATPrintOptionVM(ParafaitEnv.ExecutionContext);
                }
                catch (UserAuthenticationException ue)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(ParafaitEnv.ExecutionContext, 2927, ConfigurationManager.AppSettings["SYSTEM_USER_LOGIN_ID"]));
                    throw new UnauthorizedException(ue.Message);
                }
                vCATPrintOptionView = new VCATPrintOptionView();
                vCATPrintOptionView.DataContext = vCATPrintOptionVM;
                WindowInteropHelper helper = new WindowInteropHelper(vCATPrintOptionView);
                vCATPrintOptionView.ShowDialog();
                printOption = vCATPrintOptionVM.PrintOption;
                POSUtils.SetLastActivityDateTime();
            }
            catch (UnauthorizedException ex)
            {
                log.Error(ex);
                try
                {
                    vCATPrintOptionView.Close();
                }
                catch (Exception)
                {
                    printOption = string.Empty;
                }
            }
            log.LogMethodExit(printOption);
            return printOption;
        }
        //Begin Modification - 02-Dec-2016 for Credit plus under daily limit implementation
        private DataTable GetAvailableCreditPlusLimit(int cardNumber)
        {
            DataTable cardPlusIdDT = null;
            try
            {
                POSUtils.SetLastActivityDateTime();
                //fetch all the credit plus details for card group  by CardCreditPlusId and ProductId
                cardPlusIdDT = Utilities.executeDataTable(@"select m.ProductId, m.QuantityLimit, m.CategoryId from (
                                                            select ccn.ProductId, ccn.CategoryId, 
                                                            case when ccn.ProductId is not null 
                                                              then Ceiling(cast(sum(ccn.QuantityLimit) as float)/cast(Count(ccn.ProductId) as float))  
                                                            when ccn.CategoryId is not null
                                                              then Ceiling(cast(sum(ccn.QuantityLimit) as float)/cast(Count(ccn.CategoryId) as float))  
                                                            else sum(ccn.QuantityLimit)
                                                            end as QuantityLimit 
                                                            from CardCreditPlus cp,CardCreditPlusConsumption ccn
                                                            where 
                                                            cp.CardCreditPlusId = ccn.CardCreditPlusId 
                                                            and card_id = @cardId   
                                                            and cp.CreditPlusType in ('P', 'A') 
                                                            and isnull(ValidityStatus, 'Y') != 'H'
                                                            and (cp.PeriodFrom is null or cp.PeriodFrom <= getdate()) 
                                                            and (cp.PeriodTo is null or cp.PeriodTo > getdate()) 
                                                            and isnull(cp.TimeFrom, 0) <= DATEPART(HOUR, getdate()) 
                                                            and (case isnull(cp.TimeTo, 0) when 0 then 24 else cp.TimeTo end >= DATEPART(HOUR, getdate()) + 1)  
                                                            and isnull(case DATEPART(WEEKDAY, getdate()) 
			                                                            when 1 then cp.Sunday 
			                                                            when 2 then cp.Monday 
			                                                            when 3 then cp.Tuesday 
			                                                            when 4 then cp.Wednesday 
			                                                            when 5 then cp.Thursday 
			                                                            when 6 then cp.Friday 
			                                                            when 7 then cp.Saturday 
			                                                            else 'Y' end, 'Y') = 'Y'
			                                                            group by ccn.ProductId, ccn.CategoryId)m",
                                                                        new SqlParameter("@cardId", cardNumber));

            }
            catch { }
            return cardPlusIdDT;
        }
        //end Modification - 02-Dec-2016 for Credit plus under daily limit implementation

        private void txtGameCardAmount_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtGameCardAmount_Validating()");
            CalculateBalances();
            ValidateGameCards();
            log.Debug("Ends-txtGameCardAmount_Validating()");
        }

        double ValidateGameCards()
        {
            log.Debug("Starts-ValidateGameCards()");
            double credits = 0;
            double localGameCardAmount = gameCardAmount;
            double creditAmountAvailable = 0;
            double creditPlusAvailable = 0;

            for (int i = 0; i < dgvGameCards.Rows.Count; i++)
            {
                creditPlusAvailable = Math.Round(Convert.ToDouble(dgvGameCards.Rows[i].Cells["CreditPlus"].Value), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                credits = Math.Round(Convert.ToDouble(dgvGameCards.Rows[i].Cells["Credits"].Value), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                creditAmountAvailable = credits;
                if (localGameCardAmount > creditPlusAvailable)
                {
                    dgvGameCards.Rows[i].Cells["UsedCreditPlus"].Value = creditPlusAvailable;
                    localGameCardAmount -= creditPlusAvailable;

                    if (localGameCardAmount > creditAmountAvailable)
                    {
                        dgvGameCards.Rows[i].Cells["Balance"].Value = 0;
                        dgvGameCards.Rows[i].Cells["UsedCredits"].Value = credits;
                        dgvGameCards.Rows[i].Cells["UsedAmount"].Value = creditAmountAvailable + creditPlusAvailable;
                        localGameCardAmount -= creditAmountAvailable;
                    }
                    else
                    {
                        double usedCredits = localGameCardAmount;
                        dgvGameCards.Rows[i].Cells["Balance"].Value = (credits - usedCredits);
                        dgvGameCards.Rows[i].Cells["UsedCredits"].Value = usedCredits;
                        dgvGameCards.Rows[i].Cells["UsedAmount"].Value = usedCredits + creditPlusAvailable;
                        localGameCardAmount = 0;
                    }
                }
                else
                {
                    if (localGameCardAmount >= 0)
                    {
                        dgvGameCards.Rows[i].Cells["UsedCreditPlus"].Value = localGameCardAmount;
                        dgvGameCards.Rows[i].Cells["UsedCredits"].Value = 0;
                    }
                    else
                    {
                        dgvGameCards.Rows[i].Cells["UsedCreditPlus"].Value = 0;
                        dgvGameCards.Rows[i].Cells["UsedCredits"].Value = localGameCardAmount;
                    }

                    dgvGameCards.Rows[i].Cells["Balance"].Value = credits + Convert.ToDouble(dgvGameCards.Rows[i].Cells["CreditPlus"].Value) - localGameCardAmount;
                    dgvGameCards.Rows[i].Cells["UsedAmount"].Value = localGameCardAmount;
                    localGameCardAmount = 0;
                }
            }

            txtTotalDebitCardPayment.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", getDebitCardPaymentAmount());

            txtMessage.Text = "";
            if (localGameCardAmount != 0)
            {
                txtMessage.Text = MessageUtils.getMessage(183);
                log.Debug("Ends-ValidateGameCards() as Insufficient Credits on Game Card(s)");
                return localGameCardAmount;
            }

            log.Debug("Ends-ValidateGameCards()");
            return 0;
        }

        private void PaymentDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-PaymentDetails_FormClosing()");
            Common.Devices.UnregisterCardReaders();
            if (keypad != null)
                keypad.Close();
            Transaction.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
            try
            {
                if (cmbCreditCards != null)
                {
                    cmbCreditCards.DataSource = null;
                    cmbCreditCards.DataBindings.Clear();
                    cmbCreditCards.Tag = null;
                }
                if (cmbOtherModes != null)
                {
                    cmbOtherModes.DataSource = null;
                    cmbOtherModes.DataBindings.Clear();
                    cmbOtherModes.Tag = null;
                }
                tpCreditCard.Dispose();
                dgvGameCards.Dispose();
                dgvCouponValue.Dispose();
                cmbCreditCards.Dispose();
                cmbOtherModes.Dispose();
                panelCC.Dispose();
                panelOther.Dispose();
                panelDebit.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.Debug("Ends-PaymentDetails_FormClosing()");
        }

        private void txtCashAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            POSUtils.SetLastActivityDateTime();
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
                e.Handled = true;
        }

        private void txtGameCardAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            POSUtils.SetLastActivityDateTime();
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
                e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancel_Click()");
            POSUtils.SetLastActivityDateTime();
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            log.Debug("Ends-btnCancel_Click()");
        }

        private void btnAutoBalance_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnAutoBalance_Click()");
            POSUtils.SetLastActivityDateTime();
            txtOtherPaymentAmount.BackColor = txtCreditCardAmount.BackColor = txtGameCardAmount.BackColor = txtCashAmount.BackColor;

            txtCashAmount.Focus(); // validate text boxes

            double ccSurcharge = 0;
            creditCardAmount = getCreditCardPaymentAmount(ref ccSurcharge);
            creditCardAmount -= ccSurcharge;

            OtherPaymentAmount = getOtherPaymentAmount();

            double GameCardAmountRemaining;
            gameCardAmount = tobePaidAmount - creditCardAmount - OtherPaymentAmount;
            GameCardAmountRemaining = ValidateGameCards();
            txtMessage.Text = "";

            if (GameCardAmountRemaining > 0) // balance on card not enough
            {
                gameCardAmount = ((gameCardAmount - GameCardAmountRemaining) < 0 ? 0 : (gameCardAmount - GameCardAmountRemaining));
                cashAmount = tobePaidAmount - creditCardAmount - gameCardAmount - OtherPaymentAmount;
                if (cashAmount < 0)
                    cashAmount = 0;
            }
            else
            {
                cashAmount = tobePaidAmount - creditCardAmount - gameCardAmount - OtherPaymentAmount;
                if (cashAmount < 0)
                    cashAmount = 0;
            }
            txtCashAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", cashAmount);
            txtGameCardAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", gameCardAmount);
            txtCreditCardAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", creditCardAmount);
            txtOtherPaymentAmount.Text = string.Format("{0:" + AMOUNT_FORMAT + "}", OtherPaymentAmount);

            CalculateBalances();
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-btnAutoBalance_Click()");
        }

        private void PaymentDetails_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.Debug("Starts-PaymentDetails_KeyPress()");
            POSUtils.SetLastActivityDateTime();
            bool isAlphanumeric = true;
            isAlphanumeric = CurrentTextBox.Equals(txtPaymentReference)
                | CurrentTextBox.Equals(txtCCAuthorization)
                | CurrentTextBox.Equals(txtCCExpiry)
                | CurrentTextBox.Equals(txtCCName)
                | CurrentTextBox.Equals(txtCCNumber)
                | CurrentTextBox.Equals(txtNameOnCC)
                | CurrentTextBox.Equals(txtOtherPaymentReference);
            if (CurrentTextBox != null && isAlphanumeric == false)
            {
                if (char.IsNumber(e.KeyChar) || e.KeyChar == POSStatic.decimalChar)
                {
                    e.Handled = true;
                    showNumberPadForm(e.KeyChar);
                }
                else if (!char.IsControl(e.KeyChar))
                {
                    showNumberPadForm('-');
                }
            }
            log.Debug("Ends-PaymentDetails_KeyPress()");
        }

        AlphaNumericKeyPad keypad;
        void showNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showNumberPadForm()");
            POSUtils.SetLastActivityDateTime();
            bool isAlphanumeric = true;
            isAlphanumeric = CurrentTextBox.Equals(txtPaymentReference)
                | CurrentTextBox.Equals(txtCCAuthorization)
                | CurrentTextBox.Equals(txtCCExpiry)
                | CurrentTextBox.Equals(txtCCName)
                | CurrentTextBox.Equals(txtCCNumber)
                | CurrentTextBox.Equals(txtNameOnCC)
                | CurrentTextBox.Equals(txtOtherPaymentReference);
            if (CurrentTextBox != null && isAlphanumeric == false)
            {
                double varAmount = NumberPadForm.ShowNumberPadForm("Enter Amount", firstKey, Utilities);
                if (varAmount >= 0)
                {
                    CurrentTextBox.Text = varAmount.ToString();
                    this.ValidateChildren();
                    if (CurrentTextBox.Name.Equals("txtCreditCardAmount"))
                    {
                        if (paymentModeId > -1 && cmbCreditCards != null)
                        {
                            cmbCreditCards.SelectedValue = paymentModeId;
                            PaymentMode paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModeId);
                            cmbCreditCards.Tag = paymentModesBL.Gateway;
                        }
                    }
                }
            }
            else
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, CurrentTextBox);
                    keypad.Location = new Point(this.PointToScreen(btnShowNumPad.Location).X - keypad.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-showNumberPadForm()");
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowNumPad_Click()");
            POSUtils.SetLastActivityDateTime();
            showNumberPadForm('-');
            log.Debug("Ends-btnShowNumPad_Click()");
        }

        private void txtCashAmount_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtCashAmount;
            POSUtils.SetLastActivityDateTime();
        }

        private void txtGameCardAmount_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtGameCardAmount;
            POSUtils.SetLastActivityDateTime();
        }

        private void btnCashPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
            string amount = (balanceAmount + cashAmount).ToString();
            double varAmount = NumberPadForm.ShowNumberPadForm(tpCash.Text, amount, Utilities);
            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
            {
                if (varAmount >= 0)
                {
                    TenderedAmount = varAmount;
                    cashAmount = Math.Min(varAmount, balanceAmount + cashAmount);
                    txtChangeBack.Text = Math.Max(0, (varAmount - cashAmount)).ToString(AMOUNT_FORMAT);
                }
                else
                    cashAmount = varAmount;
                displayAmounts();
                ValidateChildren();
                CalculateBalances();
            }
            log.LogMethodExit();
        }

        private void btnCardPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
            string amount = (balanceAmount + gameCardAmount).ToString();
            double varAmount = NumberPadForm.ShowNumberPadForm(tpDebitCard.Text, amount, Utilities);
            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
            {
                gameCardAmount = varAmount;
                displayAmounts();
                ValidateChildren();
                CalculateBalances();
            }
            log.LogMethodExit();
        }

        private void btnCreditCardPayment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
            string amount = (balanceAmount + creditCardAmount).ToString();
            double varAmount = NumberPadForm.ShowNumberPadForm(tpCreditCard.Text, amount, Utilities);
            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
            {
                creditCardAmount = varAmount;
                displayAmounts();
                ValidateChildren();
                CalculateBalances();
                if (paymentModeId > -1 && cmbCreditCards != null)
                {
                    cmbCreditCards.SelectedValue = paymentModeId;
                    PaymentMode paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModeId);
                    cmbCreditCards.Tag = paymentModesBL.Gateway;
                }
            }
            log.LogMethodExit();
        }

        private void btnOtherPayment_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnOtherPayment_Click()");
            POSUtils.SetLastActivityDateTime();
            preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();
            string amount = (balanceAmount + OtherPaymentAmount).ToString();
            double varAmount = NumberPadForm.ShowNumberPadForm(tpOtherMode.Text, amount, Utilities);
            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
            {
                OtherPaymentAmount = varAmount;
                displayAmounts();
                ValidateChildren();
                CalculateBalances();
            }
            log.Debug("Ends-btnOtherPayment_Click()");
        }

        private void txtPaymentReference_Enter(object sender, EventArgs e)
        {
            CurrentTextBox = txtPaymentReference;
            POSUtils.SetLastActivityDateTime();
        }

        private void cmbCreditCards_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-cmbCreditCards_Validating()");
            POSUtils.SetLastActivityDateTime();
            if (cmbCreditCards.SelectedIndex >= 0)
            {
                if (Utilities.executeScalar("select ManagerApprovalRequired from PaymentModes where PaymentModeId = @mode",
                                                            new SqlParameter[] { new SqlParameter("@mode", cmbCreditCards.SelectedValue) }).ToString() == "Y")
                {
                    int mgrId = -1;
                    if (!Authenticate.Manager(ref mgrId))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(185), "Payment Mode Approval");
                        e.Cancel = true;
                        log.Info("Ends-cmbCreditCards_Validating() as Manager approval required to use this payment mode");
                        return;
                    }
                    if (mgrId > -1)
                        creditCardModeApprovedBy = mgrId;
                }
            }
            log.Debug("Ends-cmbCreditCards_Validating()");
        }

        private void cmbCreditCards_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbCreditCards_SelectedIndexChanged()");
            POSUtils.SetLastActivityDateTime();
            if (cmbCreditCards.SelectedIndex >= 0 && creditCardAmount != 0)
            {
                int paymentModeId = -1;
                int.TryParse(cmbCreditCards.SelectedValue != null ? cmbCreditCards.SelectedValue.ToString() : "-1", out paymentModeId);
                if (paymentModeId > -1)
                {
                    GetCreditCardSurcharge(paymentModeId);
                }
                double d = 0;
                getCreditCardPaymentAmount(ref d);
                CalculateBalances();
            }
            else
            {
                txtCCSurchPercent.Clear();
                txtCCSurchAmt.Clear();
                txtTotalCCAmountWithSurch.Clear();
            }
            log.Debug("Ends-cmbCreditCards_SelectedIndexChanged()");
        }

        private void cmbOtherModes_Validated(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbOtherModes_Validated()");
            POSUtils.SetLastActivityDateTime();
            CalculateBalances();
            log.Debug("Ends-cmbOtherModes_Validated()");
        }

        private void cmbOtherModes_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-cmbOtherModes_Validating()");
            POSUtils.SetLastActivityDateTime();
            if (cmbOtherModes.SelectedIndex >= 0)
            {
                if (Utilities.executeScalar("select ManagerApprovalRequired from PaymentModes where PaymentModeId = @mode",
                                                        new SqlParameter[] { new SqlParameter("@mode", cmbOtherModes.SelectedValue) }).ToString() == "Y")
                {
                    int mgrId = -1;
                    if (!Authenticate.Manager(ref mgrId))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(185), "Payment Approval");
                        e.Cancel = true;
                        log.Info("Ends-cmbOtherModes_Validating() as Manager approval required to use this payment mode ");
                        return;
                    }
                    if (mgrId > -1)
                        otherModeApprovedBy = mgrId;
                }
            }
            log.Debug("Ends-cmbOtherModes_Validating()");
        }


        private void dgvSavedPayments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvSavedPayments_CellClick()");
            POSUtils.SetLastActivityDateTime();
            txtMessage.Text = "";
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                log.Info("Ends-dgvSavedPayments_CellClick() as e.RowIndex < 0 || e.ColumnIndex < 0");
                return;
            }

            try
            {
                if (e.ColumnIndex == 0 && dgvSavedPayments["deleteAllowed", e.RowIndex].Value.Equals(1))
                {
                    int managerId = -1;
                    if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(518), "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Ends-dgvSavedPayments_CellClick() as  deleteAllowed and payment is deleted");
                        return;
                    }
                    if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_MANAGER_APPROVAL_FOR_PAYMENT_REVERSAL").Equals("Y"))
                    {
                        if (!Authenticate.Manager(ref managerId))
                        {

                            log.LogMethodExit("Manager not approved.");
                            return;
                        }
                    }
                    Dictionary<string, int> transactionOrderType = Transaction.TransactionOrderTypes;
                    if (Transaction.Order != null && Transaction.Order.OrderHeaderDTO != null && (Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == transactionOrderType["Refund"] || Transaction.Order.OrderHeaderDTO.TransactionOrderTypeId == transactionOrderType["Item Refund"]))
                    {
                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage("Reversal is not allowed."), "Confirm", MessageBoxButtons.OK);
                        log.Info("Ends-dgvSavedPayments_CellClick() as  Reversal is not allowed.");
                        return;
                    }

                    DataRow dr = dgvSavedPayments.Rows[e.RowIndex].Tag as DataRow;
                    TransactionPaymentsBL trxPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, Convert.ToInt32(dr["PaymentId"]));
                    TransactionPaymentsDTO trxPaymentDTO = trxPaymentsBL.TransactionPaymentsDTO;
                    trxPaymentDTO.ParentPaymentId = trxPaymentDTO.PaymentId;
                    trxPaymentDTO.PaymentId = -1;
                    trxPaymentDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                    if (managerId > -1)
                    {
                        Users approveUser = new Users(Utilities.ExecutionContext, managerId);
                        trxPaymentDTO.ApprovedBy = approveUser.UserDTO.LoginId;
                    }
                    trxPaymentDTO.Amount = -1 * trxPaymentDTO.Amount;
                    if (trxPaymentDTO.TenderedAmount != null)
                        trxPaymentDTO.TenderedAmount = -1 * trxPaymentDTO.TenderedAmount;
                    trxPaymentDTO.TipAmount = -1 * trxPaymentDTO.TipAmount;
                    if (trxPaymentDTO.paymentModeDTO.IsCreditCard)
                    {
                        string message = "";
                        try
                        {
                            double paymentAmount = trxPaymentDTO.Amount;
                            if (trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString().ToUpper() != "NONE")
                            {
                                try
                                {
                                    DisableButtons();
                                    if (!CreditCardPaymentGateway.RefundAmount(trxPaymentDTO, Utilities, ref message))
                                    {
                                        POSUtils.ParafaitMessageBox(MessageUtils.getMessage(187, message), "Remove Payment");
                                        log.Info("Ends-dgvSavedPayments_CellClick() as cannot reverse payment");
                                        return;
                                    }
                                    else
                                    {
                                        trxPaymentDTO.TipAmount = -1 * Convert.ToDouble(dr["TipAmount"]);//Begin Modification on 09-Nov-2015:tip Feature
                                    }
                                }
                                finally
                                {
                                    EnableButtons();
                                }
                            }
                            trxPaymentDTO.Amount = paymentAmount;
                            // smartro reverse
                            ReverseSmartroPayment(trxPaymentDTO);
                            TransactionPaymentsBL trxCreditPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                            trxCreditPaymentsBL.Save();
                        }
                        catch (Exception ex)
                        {
                            POSUtils.ParafaitMessageBox(ex.Message, "Remove Payment");
                            log.Fatal("Ends-dgvSavedPayments_CellClick() in isCreditCard due to exception " + ex.ToString());
                            return;
                        }
                    }
                    else if (trxPaymentDTO.paymentModeDTO.IsCash)
                    {
                        ReverseSmartroPayment(trxPaymentDTO);
                        TransactionPaymentsBL trxCashPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                        trxCashPaymentsBL.Save();
                        TrxStatusChanged = true;
                    }
                    else if (trxPaymentDTO.paymentModeDTO.IsDebitCard)
                    {
                        SqlTransaction SQLTrx = Utilities.getConnection().BeginTransaction();
                        try
                        {
                            int cardId = Convert.ToInt32(dr["CardId"]);

                            if (dr["CardEntitlementType"].ToString() == "C")
                            {
                                SqlCommand PayCardTrx = Utilities.getCommand(SQLTrx);
                                PayCardTrx.CommandText = @"update cards set credits = credits - @usedCredits, 
                                                         last_update_time = getdate(), 
                                                         LastUpdatedBy = @LastUpdatedBy, 
                                                         credits_played = isnull(credits_played, 0) + @usedCredits 
                                                         where card_id = @card_Id";

                                PayCardTrx.Parameters.AddWithValue("@card_Id", dr["CardId"]);
                                PayCardTrx.Parameters.AddWithValue("@usedCredits", trxPaymentDTO.Amount);
                                PayCardTrx.Parameters.AddWithValue("@LastUpdatedBy", Transaction.Utilities.ParafaitEnv.LoginID);
                                PayCardTrx.ExecuteNonQuery();
                            }
                            else
                            {
                                SqlCommand cmd = Utilities.getCommand(SQLTrx);
                                cmd.CommandText = "update CardCreditPlus set CreditPlusBalance = CreditPlusBalance - @reduceAmount, " +
                                                        "LastupdatedDate = getdate(), LastUpdatedBy = @user " +
                                                    "where CardCreditPlusId = @CardCreditPlusId";

                                cmd.Parameters.AddWithValue("@reduceAmount", trxPaymentDTO.Amount);
                                cmd.Parameters.AddWithValue("@user", Utilities.ParafaitEnv.LoginID);
                                cmd.Parameters.AddWithValue("@CardCreditPlusId", dr["CardCreditPlusId"]);
                                cmd.ExecuteNonQuery();
                                //Update credits played value with the reversed entry
                                cmd.CommandText = @"update cards set credits_played = isnull(credits_played, 0) + @reduceAmount, 
                                                    last_update_time = getdate(), 
                                                    LastUpdatedBy = @user 
                                                    where card_id = @cardId";
                                cmd.Parameters.AddWithValue("@cardId", dr["CardId"]);
                                cmd.ExecuteNonQuery();
                            }
                            TransactionPaymentsBL trxGameCardPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                            trxGameCardPaymentsBL.Save(SQLTrx);
                            SQLTrx.Commit();
                        }
                        catch (Exception ex)
                        {
                            SQLTrx.Rollback();
                            txtMessage.Text = ex.Message;
                            log.Fatal("Ends-dgvSavedPayments_CellClick() in isDebitCard due to exception " + ex.Message);
                        }
                    }
                    else
                    {
                        trxPaymentDTO.CouponValue = trxPaymentDTO.CouponValue == (double?)null ? (double?)null : (-1 * trxPaymentDTO.CouponValue);

                        TransactionPaymentsBL trxOtherPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                        trxOtherPaymentsBL.Save();
                        if (trxPaymentDTO.CouponSetId != -1 && trxPaymentDTO.Reference != null)
                        {
                            TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                            transactionUtils.UpdateCouponUsedDetails(trxPaymentDTO.CouponSetId, trxPaymentDTO.Reference, trxPaymentDTO.TransactionId, null, trxPaymentDTO.Amount);
                        }
                    }
                    Transaction.TransactionPaymentsDTOList.Add(trxPaymentDTO);//Add to existing transaction payment dto list
                    Transaction.getTotalPaidAmount(null);
                    Transaction.updateAmounts();
                    TrxStatusChanged = true;
                    load();
                    txtMessage.Text = MessageUtils.getMessage(492);
                    log.Info("dgvSavedPayments_CellClick() as Payment cancelled");
                }
                else if (dgvSavedPayments.Columns[e.ColumnIndex].Name == "TipAmt")//Begin Modification on 09-Nov-2015:tip Feature
                {
                    DataRow dr = dgvSavedPayments.Rows[e.RowIndex].Tag as DataRow;
                    if (IsTipEnabled && !IsPaymentReversed(dr["PaymentId"]))
                    {
                        TransactionPaymentsBL trxPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, Convert.ToInt32(dr["PaymentId"]));
                        TransactionPaymentsDTO trxPaymentDTO = trxPaymentsBL.TransactionPaymentsDTO;
                        string gateway = "";
                        string paymentMode = "";
                        if (trxPaymentDTO.Amount < 0)
                        {
                            log.Info("Ends-dgvSavedPayments_CellClick() in TipAmt as pd.Amount < 0");
                            return;
                        }
                        int compare = trxPaymentDTO.PaymentDate.CompareTo(ServerDateTime.Now.AddDays(-1));
                        if (compare < 0)
                        {
                            txtMessage.Text = "Tip adjustment should be done within 24 hours.";
                            log.Info("Ends-dgvSavedPayments_CellClick() in TipAmt adjustment should be done with in 24 hours ");
                            return;
                        }
                        if (trxPaymentDTO.paymentModeDTO.IsCreditCard && !string.IsNullOrEmpty(trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString())
                             && trxPaymentDTO.paymentModeDTO.GatewayLookUp != PaymentGateways.None) //Modified on 22-jun-2017 for allowing tip to credit card which are not linked gateway
                        {
                            gateway = trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString();
                            paymentMode = "CreditCard";
                            if (!(string.IsNullOrEmpty(gateway) && trxPaymentDTO.paymentModeDTO.IsCreditCard))
                            {
                                List<int> lstUserId = new List<int>();
                                //frmTip formTip = null;
                                frmFinalizeTransaction frmFinalize;
                                int managerId = -1;
                                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(gateway);
                                TransactionPaymentsDTO transactionPaymentsDTO = trxPaymentDTO;
                                bool settlementPending = paymentGateway.IsSettlementPending(transactionPaymentsDTO);
                                if (paymentGateway.IsTipAllowed(transactionPaymentsDTO) || settlementPending)
                                {
                                    if (!settlementPending && Utilities.getParafaitDefaults("MANAGER_APPROVAL_REQUIRED_FOR_TIP_ADJUSTMENT").Equals("Y"))
                                    {
                                        if (!Authenticate.Manager(ref managerId))
                                        {
                                            log.Debug("Manager is not approved");
                                            return;
                                        }
                                        if (managerId > -1)
                                        {
                                            Users approverUserDTO = new Users(Utilities.ExecutionContext, managerId);
                                            transactionPaymentsDTO.ApprovedBy = approverUserDTO.UserDTO.LoginId;
                                        }
                                    }
                                    // TrxUserLogsBL trxUserLogsBL = new TrxUserLogsBL();                                    
                                    using (frmFinalize = new frmFinalizeTransaction(Utilities, Convert.ToDecimal(Transaction.Transaction_Amount), Convert.ToDecimal(Transaction.Tip_Amount), Convert.ToDecimal(transactionPaymentsDTO.Amount), Convert.ToDecimal(transactionPaymentsDTO.TipAmount), transactionPaymentsDTO.CreditCardNumber, POSUtils.ParafaitMessageBox))//formTip = new frmTip(Utilities, 0.00, transactionPaymentsDTO.Amount, "CreditCard", false))
                                    {
                                        if (frmFinalize.ShowDialog() == DialogResult.Cancel)
                                        {
                                            txtMessage.Text = Utilities.MessageUtils.getMessage("Cancelled");
                                            log.LogMethodExit("Cancelled");
                                            return;
                                        }
                                        if (frmFinalize.TipAmount <= 0 && !paymentGateway.IsTipAdjustmentAllowed)
                                            return;
                                        //if (MessageBox.Show(Utilities.MessageUtils.getMessage(953, string.Format("{0:" + Utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", frmFinalize.TipAmount)), "Tip Amount", MessageBoxButtons.YesNo) == DialogResult.No)//Begin Modification on 18-Nov-2015:Tip feature
                                        //{
                                        //    return;
                                        //}
                                        transactionPaymentsDTO.TipAmount = Convert.ToDouble(frmFinalize.TipAmount);
                                        trxPaymentDTO.TipAmount = Convert.ToDouble(frmFinalize.TipAmount);
                                        TransactionPaymentsDTO tipPaymentTransactionPaymentsDTO = paymentGateway.PayTip(transactionPaymentsDTO);
                                        if (tipPaymentTransactionPaymentsDTO != null)
                                        {
                                            tipPaymentTransactionPaymentsDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                                            TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                                            transactionPaymentsBL.Save();

                                            Transaction.InsertTrxLogs(tipPaymentTransactionPaymentsDTO.TransactionId, -1, Utilities.ParafaitEnv.LoginID, "CCSETTLEMENT", "Transaction id " + tipPaymentTransactionPaymentsDTO.TransactionId + "(Payment Id:" + tipPaymentTransactionPaymentsDTO.PaymentId + ") is settled with tipamount " + tipPaymentTransactionPaymentsDTO.TipAmount + ".", null, managerId.ToString(), Utilities.getServerTime());
                                            //Transaction.CreateTipPayment(transactionPaymentsDTO.PaymentId, lstUserId, null);
                                            dr["TipAmount"] = tipPaymentTransactionPaymentsDTO.TipAmount.ToString("0.00");
                                        }
                                    }
                                }
                            }
                            Transaction.getTotalPaidAmount(null);
                            CashTip = CreditTip = 0;
                            TrxStatusChanged = true;
                            load();
                        }
                        else if (trxPaymentDTO.paymentModeDTO.IsCash ||
                                    (trxPaymentDTO.paymentModeDTO.IsCreditCard &&
                                      (string.IsNullOrEmpty(trxPaymentDTO.paymentModeDTO.GatewayLookUp.ToString())
                                       || trxPaymentDTO.paymentModeDTO.GatewayLookUp == PaymentGateways.None)
                                    )
                                )
                        {
                            if (trxPaymentDTO.paymentModeDTO.IsCash)
                            {
                                paymentMode = "Cash";
                            }
                            else
                            {
                                paymentMode = "CreditCard";
                            }
                            if (dgvSavedPayments.Rows[e.RowIndex].Cells["TipAmt"].Value.Equals("0.00"))
                            {
                                using (frmTip formTip = new frmTip(Utilities, CashTip, trxPaymentDTO.Amount, paymentMode, false))
                                {
                                    formTip.ShowDialog();
                                    if (formTip.TipAmount == 0)
                                    {
                                        log.Info("Ends-dgvSavedPayments_CellClick() in Cash mode TipAmt as TipAmount == 0");
                                        return;
                                    }
                                    if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(953, string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", formTip.TipAmount)), "Tip Amount", MessageBoxButtons.YesNo) == DialogResult.No)//Begin Modification on 18-Nov-2015:Tip feature
                                    {
                                        log.Info("Ends-dgvSavedPayments_CellClick() in CreditCard mode TipAmt is been entered");
                                        return;
                                    }//End Modification on 18-Nov-2015:Tip feature
                                    if (formTip.TipAmount > 0)
                                    {
                                        trxPaymentDTO.TipAmount = formTip.TipAmount;
                                        TransactionPaymentsBL trxOtherPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                                        trxOtherPaymentsBL.Save();

                                        TransactionPaymentsDTO trxTipPaymentDTO = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == trxPaymentDTO.PaymentId).First();
                                        trxTipPaymentDTO.TipAmount = trxPaymentDTO.TipAmount;
                                        Transaction.CreateTipPayment(trxPaymentDTO.PaymentId, formTip.lstUserId, null);
                                        //formTip.Dispose();
                                    }
                                }
                                Transaction.getTotalPaidAmount(null);
                                CashTip = CreditTip = 0;
                                TrxStatusChanged = true;
                                load();
                            }
                        }
                    }//End Modification on 09-Nov-2015
                }
                else if (dgvSavedPayments.Columns[e.ColumnIndex].Name == "Attributes" )
                {
                    DataRow dr = dgvSavedPayments.Rows[e.RowIndex].Tag as DataRow;
                    ShowTableAttributeUI(dr, dgvSavedPayments["deleteAllowed", e.RowIndex].Value.Equals(1));
                }
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Fatal("Ends-dgvSavedPayments_CellClick() due to exception " + ex.Message);
            }
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-dgvSavedPayments_CellClick()");
        }

        private void ReverseSmartroPayment(TransactionPaymentsDTO trxPaymentDTO)
        {
            log.LogMethodEntry(trxPaymentDTO);
            POSUtils.SetLastActivityDateTime();
            if ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.Smartro.ToString())))
            {
                log.Debug("FiscalPrinters.SmartroKorea  Reversal");
                FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(Utilities.getParafaitDefaults("FISCAL_PRINTER"));
                string Message = string.Empty;
                FiscalizationRequest fiscalizationRequest = new FiscalizationRequest();
                List<PaymentInfo> payItemList = new List<PaymentInfo>();
                PaymentInfo paymentInfo = new PaymentInfo();
                if (trxPaymentDTO.paymentModeDTO.IsCash)
                {
                    paymentInfo.paymentMode = "Cash";
                    decimal smartroAmount = decimal.Parse(trxPaymentDTO.Amount.ToString());
                    log.Debug("smartroAmount:" + smartroAmount);
                    if (smartroAmount < 0)
                        smartroAmount = smartroAmount * -1;
                    paymentInfo.amount = Math.Round(smartroAmount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                }
                else if (trxPaymentDTO.paymentModeDTO.IsCreditCard)
                {
                    paymentInfo.paymentMode = "CreditCard";
                    paymentInfo.amount = Convert.ToDecimal(trxPaymentDTO.Amount);
                    if (paymentInfo.amount < 0)
                        paymentInfo.amount = paymentInfo.amount * -1;
                    paymentInfo.quantity = Convert.ToInt32(trxPaymentDTO.Reference);  // installment month from saved transaction
                }
                paymentInfo.reference = trxPaymentDTO.CreditCardAuthorization;
                paymentInfo.description = trxPaymentDTO.PaymentDate.ToString("yyyyMMdd");
                log.LogVariableState("paymentInfo", paymentInfo);
                payItemList.Add(paymentInfo);
                fiscalizationRequest.payments = payItemList.ToArray();
                fiscalizationRequest.isReversal = true;
                fiscalizationRequest.extReference = trxPaymentDTO.Reference;
                log.LogVariableState("fiscalizationRequest", fiscalizationRequest);
                Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine transactionLine = new Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine();
                if (Transaction != null && Transaction.TrxLines != null && Transaction.TrxLines.Any())
                {
                    transactionLine.VATRate = Convert.ToDecimal(Transaction.TrxLines.First().tax_percentage);
                    log.Debug("VATRate :" + transactionLine.VATRate);
                    if (transactionLine.VATRate > 0)
                    {
                        //creditCardAmount is inclusive of tax amount. 
                        transactionLine.VATAmount = (Convert.ToDecimal(paymentInfo.amount) * transactionLine.VATRate) / (100 + transactionLine.VATRate);
                        log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                        if (transactionLine.VATAmount % 1 > 0)
                        {
                            transactionLine.VATAmount = (decimal)(new Semnox.Core.GenericUtilities.CommonFuncs(Utilities)).RoundOff(Convert.ToDouble(transactionLine.VATAmount), Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                            log.Debug("transactionLine.VATAmount after rounding:" + transactionLine.VATAmount);
                        }
                    }
                    else
                    {
                        transactionLine.VATAmount = 0;
                        log.Debug("transactionLine.VATAmount :" + transactionLine.VATAmount);
                    }
                }
                fiscalizationRequest.transactionLines = new Semnox.Parafait.Device.Printer.FiscalPrint.TransactionLine[] { transactionLine };

                POSUtils.SetLastActivityDateTime();
                bool success = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref Message);
                POSUtils.SetLastActivityDateTime();
                if (success)
                {
                    if (fiscalPrinter != null && string.IsNullOrWhiteSpace(fiscalizationRequest.extReference) == false)
                    {
                        trxPaymentDTO.CreditCardNumber = fiscalizationRequest.payments[0].description;
                        trxPaymentDTO.CreditCardAuthorization = fiscalizationRequest.extReference;
                        trxPaymentDTO.ExternalSourceReference = fiscalizationRequest.payments[0].description;
                        if (trxPaymentDTO.paymentModeDTO.IsCreditCard)
                        {
                            trxPaymentDTO.CreditCardName = fiscalizationRequest.transactionLines.ToList().First().description;
                        }
                    }
                    else
                    {
                        log.Error("Payment reversal Failed");
                        fiscalPrinter.ClosePort();
                        throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment reversal Failed"));
                    }
                }
                else
                {
                    log.Error("Payment reversal Failed");
                    fiscalPrinter.ClosePort();
                    throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment reversal Failed"));
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void txtOtherPaymentAmount_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtOtherPaymentAmount_Enter()");
            CurrentTextBox = txtOtherPaymentAmount;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtOtherPaymentAmount_Enter()");
        }

        private void txtOtherPaymentAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            POSUtils.SetLastActivityDateTime();
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
                e.Handled = true;
        }

        private void txtCreditCardAmount_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtCreditCardAmount_Enter()");
            CurrentTextBox = txtCreditCardAmount;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCreditCardAmount_Enter()");
        }

        private void txtCreditCardAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            POSUtils.SetLastActivityDateTime();
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
                e.Handled = true;
        }

        private void txtCCNumber_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtCCNumber_Enter()");
            CurrentTextBox = txtCCNumber;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCCNumber_Enter()");
        }

        private void txtNameOnCC_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtNameOnCC_Enter()");
            CurrentTextBox = txtNameOnCC;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtNameOnCC_Enter()");
        }

        private void txtCCExpiry_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtCCExpiry_Enter()");
            CurrentTextBox = txtCCExpiry;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCCExpiry_Enter()");
        }

        private void txtCCAuthorization_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtCCAuthorization_Enter()");
            CurrentTextBox = txtCCAuthorization;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCCAuthorization_Enter()");
        }

        private void txtCCName_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtCCName_Enter()");
            CurrentTextBox = txtCCName;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCCName_Enter()");
        }

        private void txtOtherPaymentReference_Enter(object sender, EventArgs e)
        {
            log.Debug("Starts-txtOtherPaymentReference_Enter()");
            CurrentTextBox = txtOtherPaymentReference;
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtOtherPaymentReference_Enter()");
        }

        private void txtCreditCardAmount_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtCreditCardAmount_Validating()");
            CalculateBalances();
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtCreditCardAmount_Validating()");
        }

        private void txtOtherPaymentAmount_Validating(object sender, CancelEventArgs e)
        {
            log.Debug("Starts-txtOtherPaymentAmount_Validating()");
            CalculateBalances();
            POSUtils.SetLastActivityDateTime();
            log.Debug("Ends-txtOtherPaymentAmount_Validating()");
        }

        void createPaymentModeButtons()
        {
            log.Debug("Starts-createPaymentModeButtons()");
            POSUtils.SetLastActivityDateTime();
            flpPaymentModes.Controls.Clear();
            DataTable dtAllPaymentModes = new DataTable();
            dtAllPaymentModes = POSUtils.dtAllPaymentModes.Copy();
            int index = 0;
            //foreach (DataRow dr in POSUtils.dtAllPaymentModes.Rows)
            foreach (DataRow dr in dtAllPaymentModes.Rows)
            {
                Button btnPaymentMode = new Button();
                btnPaymentMode.FlatStyle = FlatStyle.Flat;
                btnPaymentMode.FlatAppearance.BorderSize = 0;
                btnPaymentMode.FlatAppearance.CheckedBackColor = Color.Transparent;
                btnPaymentMode.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnPaymentMode.FlatAppearance.MouseOverBackColor = Color.Transparent;
                btnPaymentMode.BackgroundImageLayout = ImageLayout.Zoom;
                btnPaymentMode.BackColor = Color.Transparent;
                btnPaymentMode.Tag = dr;
                btnPaymentMode.Text = dr["PaymentMode"].ToString();
                btnPaymentMode.Click += btnPaymentMode_Click;
                if (index++ < 3)
                {
                    Color bgColor;
                    switch (index)
                    {
                        case 1:
                            bgColor = Color.Turquoise;
                            break;
                        case 2:
                            bgColor = Color.LimeGreen;
                            break;
                        default:
                            bgColor = Color.OrangeRed;
                            break;
                    }

                    btnPaymentMode.Size = btnSample1.Size;
                    btnPaymentMode.BackgroundImage = POSUtils.ChangeColor((Bitmap)btnSample1.BackgroundImage, bgColor);
                    btnPaymentMode.Font = btnSample1.Font;
                    btnPaymentMode.ForeColor = btnSample1.ForeColor;
                }
                else
                {
                    btnPaymentMode.Size = btnSample2.Size;
                    btnPaymentMode.BackgroundImage = btnSample2.BackgroundImage;
                    btnPaymentMode.Font = btnSample2.Font;
                    btnPaymentMode.ForeColor = btnSample2.ForeColor;
                }

                //Added on 4-july for hiding Loyalty Points button
                if (dr["PaymentMode"].ToString() != "Loyalty Points")
                {
                    flpPaymentModes.Controls.Add(btnPaymentMode);
                }

                if (dr["isDebitCard"].ToString().Equals("Y"))
                    lblDebitCard.Text = btnPaymentMode.Text + ":";
            }
            log.Debug("Ends-createPaymentModeButtons()");
        }

        void btnPaymentMode_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Starts-btnPaymentMode_Click()");//Modified for Adding logger feature on 08-Mar-2016 
                POSUtils.SetLastActivityDateTime();
                Button payMode = (sender as Button);
                DataRow dr = payMode.Tag as DataRow;
                isCouponPayment = false;
                //start: Modification on 11-may-2016 for Assign paymentModeId and clear the couponValueGrid
                dgvCouponValue.Rows.Clear();
                PaymentMode paymentModesBL = null;
                if (dr[0] != null || dr[0] != DBNull.Value)
                {
                    paymentModeId = Convert.ToInt32(dr[0]);
                    paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModeId);
                }
                else
                    paymentModeId = 0;
                //end 

                if (dr["PaymentReferenceMandatory"] != DBNull.Value)
                    PaymentReferenceMandatory = dr["PaymentReferenceMandatory"].ToString() == "N" ? false : true;
                else
                    PaymentReferenceMandatory = false;

                string mode = "";
                preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>();

                if (dr["isCash"].ToString().Equals("Y"))
                {
                    mode = "Cash";
                    btnCashAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                }
                else if (dr["isDebitCard"].ToString().Equals("Y"))
                {
                    mode = "DebitCard";
                    btnGCAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                }
                else if (dr["isCreditCard"].ToString().Equals("Y"))
                {
                    mode = "CreditCard";
                    btnCCAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                }
                else
                {
                    mode = "Other";
                    btnOtherAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                    chbIsTaxable.Checked = false;
                    //Start: Modification on 18-may-2016 for coupon Payment
                    object couponExist = Utilities.executeScalar(@"if exists(select 1 from DiscountCoupons where PaymentModeId = @paymentModeId ) 
                                                                 select 1
                                                                 else 
                                                                 select 0", new SqlParameter("@paymentModeId", paymentModeId));
                    if (Convert.ToInt32(couponExist) == 1)
                        isCouponPayment = true;
                    else
                        isCouponPayment = false;
                    //end
                }

                cashAmount = creditCardAmount = gameCardAmount = OtherPaymentAmount = 0;
                displayAmounts();
                ValidateChildren();
                CalculateBalances();

                tcPaymentModes.TabPages.Clear();

                log.Info("btnPaymentMode_Click() - Payment Mode is " + mode);

                switch (mode)
                {
                    case "Cash":
                        {
                            tcPaymentModes.TabPages.Add(tpCash);
                            Application.DoEvents();

                            double amount = (balanceAmount + cashAmount);
                            double varAmount;
                            if (cashAmount == CashTip && balanceAmount == 0)
                            {
                                cashAmount = 0;
                            }
                            DialogResult DR;
                            POSUtils.SetLastActivityDateTime();
                            using (frmTender ft = new frmTender(amount))
                            {
                                POSUtils.SetLastActivityDateTime();
                                DR = ft.ShowDialog();
                                POSUtils.SetLastActivityDateTime();
                                //begin Modification on 18-Nov-2015:Tip feature
                                if (ft.TenderedAmount < Math.Round((tobePaidAmount + CashTip), ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero))
                                {
                                    CashTip = 0;
                                }
                                txtTipAmount.Text = CashTip.ToString(AMOUNT_FORMAT);
                                txtTotalAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", Transaction.Net_Transaction_Amount);
                                TotalAmount = Math.Round(Transaction.Net_Transaction_Amount, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                                //End Modification on 18-Nov-2015:Tip feature
                                varAmount = ft.TenderedAmount;
                                currencyCode = string.Empty;
                                currencyRate = 0;

                                //added on 03-Oct-2016 for implementing multi currency
                                int currencyTypeID = ft.CurrencyID;
                                if (currencyTypeID != -1)
                                {
                                    varAmount = ft.MultiCurrencyAmount;
                                    Currency currency = new Currency(Utilities.ExecutionContext, currencyTypeID); //// Added on 2- jul-2019 : Modified the Currency BL based on new structure. 
                                    CurrencyDTO currencyDisplay = currency.CurrencyDTO;

                                    if (currencyDisplay != null)
                                    {
                                        double saleRate = currencyDisplay.SellRate;
                                        varAmount = Math.Round(varAmount / saleRate, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                                        currencyCode = currencyDisplay.CurrencyCode;
                                        currencyRate = currencyDisplay.SellRate;
                                    }
                                }
                            }
                            //end
                            if (varAmount >= 0 || (amount < 0 && DR != DialogResult.Cancel)) //If tender form is cancelled and Item Refund scenario, ignore balance calculation
                            {
                                //Rounding Fix 29-Apr-2019
                                roundedCashAmount = (decimal)POSStatic.CommonFuncs.RoundOff(balanceAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                TenderedAmount = varAmount;
                                if ((decimal)varAmount == roundedCashAmount) //rounding fix
                                {
                                    varAmount = cashAmount;//rounding fix
                                    cashAmount = balanceAmount;//rounding fix
                                }
                                else//rounding fix
                                {
                                    if (varAmount < 0)
                                    {
                                        cashAmount = Math.Max(varAmount, balanceAmount + cashAmount);
                                    }
                                    else
                                    {
                                        cashAmount = Math.Min(varAmount, balanceAmount + cashAmount);//Begin Modification on 09-Nov-2015:Tip Feature.
                                    }
                                }
                                txtChangeBack.Text = Math.Max(0, (varAmount - cashAmount)).ToString(AMOUNT_FORMAT);
                                displayAmounts();
                                ValidateChildren();
                                CalculateBalances();
                            }
                            break;
                        }
                    case "DebitCard":
                        {
                            POSUtils.SetLastActivityDateTime();
                            tcPaymentModes.TabPages.Add(tpDebitCard);
                            Application.DoEvents();

                            string amount = (balanceAmount + gameCardAmount).ToString();
                            double varAmount = NumberPadForm.ShowNumberPadForm(payMode.Text, amount, Utilities);
                            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
                            {
                                gameCardAmount = varAmount;
                                displayAmounts();
                                ValidateChildren();
                                CalculateBalances();
                            }
                            break;
                        }
                    case "CreditCard":
                        {
                            POSUtils.SetLastActivityDateTime();
                            //The Default value FIRSTDATA_AUTHORIZATION_ENABLED will be changed to a generic PAYMENT_GATEWAY_AUTHORIZATION_ENABLED   
                            GenericDataEntry trxRemarks;
                            tcPaymentModes.TabPages.Add(tpCreditCard);
                            GetCreditCardSurcharge(paymentModeId);
                            Application.DoEvents();
                            if (paymentModesBL.GetPaymentModeDTO.IsQRCode.Equals('Y'))//Starts:ChinaICBC changes
                            {
                                if (paymentModesBL.Gateway.Equals("Ipay"))
                                {
                                    trxRemarks = new GenericDataEntry(null, true, 1);
                                    trxRemarks.Text = MessageUtils.getMessage("Enter QR Code");
                                    trxRemarks.DataEntryObjects[0].mandatory = true;
                                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Enter QR Code");
                                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        QRCode = trxRemarks.DataEntryObjects[0].data;
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }//Ends:ChinaICBC changes
                                else if (paymentModesBL.Gateway.Equals("QwikCilver"))
                                {
                                    trxRemarks = new GenericDataEntry(2);
                                    trxRemarks.Text = MessageUtils.getMessage("QwikCilver");
                                    trxRemarks.DataEntryObjects[0].mandatory = true;
                                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Enter/Swipe Card Number");
                                    trxRemarks.Enabled = true;
                                    if (string.IsNullOrWhiteSpace(trxRemarks.DataEntryObjects[0].data))
                                    {
                                        trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Card Number");
                                    }
                                    //trxRemarks.Text = MessageUtils.getMessage("QuikCilver");
                                    trxRemarks.DataEntryObjects[1].mandatory = true;
                                    trxRemarks.DataEntryObjects[1].label = MessageUtils.getMessage("Enter Card Pin");
                                    trxRemarks.Enabled = true;
                                    if (string.IsNullOrWhiteSpace(trxRemarks.DataEntryObjects[1].data))
                                    {
                                        trxRemarks.DataEntryObjects[1].label = MessageUtils.getMessage("Card Pin");
                                    }

                                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        giftCardNumber = trxRemarks.DataEntryObjects[0].data.Trim();
                                        giftCardPIN = trxRemarks.DataEntryObjects[1].data.Trim();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }//end quikcilver changes
                                else if (paymentModesBL.Gateway.Equals("ClubSpeedGiftCard"))//clubspeed changes
                                {
                                    trxRemarks = new GenericDataEntry(1);
                                    trxRemarks.Text = MessageUtils.getMessage("Club Speed Gift Card");
                                    trxRemarks.DataEntryObjects[0].mandatory = true;//mandatory to enter the number
                                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Enter/Swipe Gift Card Number");
                                    trxRemarks.Enabled = true;
                                    if (string.IsNullOrWhiteSpace(trxRemarks.DataEntryObjects[0].data))//over writing to send label as Gift Card Number is mandatory
                                    {
                                        trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Gift Card Number");
                                    }
                                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        giftCardNumber = trxRemarks.DataEntryObjects[0].data.Trim();//GIFTCardNumber=Value entered in the TextBox
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }//end clubspeed changes
                                else
                                {
                                    trxRemarks = new GenericDataEntry(1);
                                }
                            }
                            else if (paymentModesBL.GetPaymentModeDTO.IsQRCode.Equals('D'))
                            {
                                QRCode = "12345";
                            }
                            txtTotalAmount.Text = string.Format("{0:" + ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL + "}", Transaction.Net_Transaction_Amount);
                            TotalAmount = Transaction.Net_Transaction_Amount;
                            string amount = "";
                            amount = (balanceAmount + creditCardAmount).ToString();
                            double varAmount = 0.0;
                            PaymentGateways gateway;
                            if (string.IsNullOrWhiteSpace(paymentModesBL.Gateway) == false &&
                                        Enum.TryParse<PaymentGateways>(paymentModesBL.Gateway, out gateway))
                            {
                                PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(gateway);
                                paymentGateway.SetTransactionAmount(Convert.ToDecimal(Transaction.Net_Transaction_Amount));
                                paymentGateway.SetTotalTipAmountEntered(Convert.ToDecimal(Transaction.Tip_Amount));
                            }


                            /*if (Utilities.getParafaitDefaults("ALLOW_CREDIT_CARD_AUTHORIZATION").Equals("Y"))
                            {
                                try
                                {
                                    PaymentGateways gateway;
                                    if (string.IsNullOrWhiteSpace(paymentModesBL.Gateway) == false &&
                                        Enum.TryParse<PaymentGateways>(paymentModesBL.Gateway, out gateway))
                                    {
                                        PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(gateway);
                                        List<TransactionPaymentsDTO> transactionPaymentsDTOList = GetTransactionPaymentsDTOList(paymentModeId);
                                        if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
                                        {
                                            foreach (var transactionPaymentsDTO in transactionPaymentsDTOList)
                                            {
                                                if (paymentGateway.IsSettlementPending(transactionPaymentsDTO))
                                                {
                                                    if (POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(1174), paymentModesBL.GetPaymentModeDTO.PaymentMode, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                                    {
                                                        TransactionPaymentsDTO settledTransactionPaymentsDTO = paymentGateway.PerformSettlement(transactionPaymentsDTO);
                                                        if (settledTransactionPaymentsDTO != null)
                                                        {
                                                            settledTransactionPaymentsDTO.PosMachine = Utilities.ParafaitEnv.POSMachine;
                                                            TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, settledTransactionPaymentsDTO);
                                                            transactionPaymentsBL.Save();
                                                            Transaction.InsertTrxLogs(settledTransactionPaymentsDTO.TransactionId, -1, Utilities.ParafaitEnv.LoginID, "CCSETTLEMENT", "Transaction id " + settledTransactionPaymentsDTO.TransactionId + "(Payment Id:" + settledTransactionPaymentsDTO.PaymentId + ") is settled with tipamount "+ settledTransactionPaymentsDTO.TipAmount + ".");                                                        
                                                            load();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cmbCreditCards.SelectedValue = -1;
                                                    }
                                                    return;
                                                }
                                            }
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    txtMessage.Text = ex.Message;
                                    POSUtils.ParafaitMessageBox(ex.Message, "Payment Gateway");
                                    log.Info("Ends-btnPaymentMode_Click() as error occured while processing payment settlement.");
                                    return;
                                }

                            }*/
                            POSUtils.SetLastActivityDateTime();
                            varAmount = NumberPadForm.ShowNumberPadForm(payMode.Text, amount, Utilities);
                            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
                            {
                                creditCardAmount = varAmount;
                                displayAmounts();
                                ValidateChildren();
                                CalculateBalances();
                                cmbCreditCards.SelectedValue = dr["PaymentModeId"];
                                cmbCreditCards.Tag = paymentModesBL.Gateway;
                                if (paymentModesBL != null && paymentModesBL.GetPaymentModeDTO.Gateway != -1)
                                {
                                    //btnSave.PerformClick();
                                    if (ApplyPayment())
                                        load();
                                    else
                                    {
                                        if (tpCreditCard != null && !tpCreditCard.IsDisposed)
                                        {
                                            tcPaymentModes.TabPages.Clear();
                                            tcPaymentModes.TabPages.Add(tpCreditCard);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case "Other":
                        {
                            POSUtils.SetLastActivityDateTime();
                            tcPaymentModes.TabPages.Add(tpOtherMode);
                            Application.DoEvents();

                            string amount = (balanceAmount + OtherPaymentAmount).ToString();
                            double varAmount = NumberPadForm.ShowNumberPadForm(payMode.Text, amount, Utilities);
                            if (varAmount >= 0 || (Convert.ToDecimal(amount) < 0 && NumberPadForm.dialogResult != DialogResult.Cancel))
                            {
                                OtherPaymentAmount = varAmount;
                                displayAmounts();
                                ValidateChildren();
                                cmbOtherModes.SelectedValue = dr["PaymentModeId"];
                                CalculateBalances();
                            }
                            break;
                        }
                }

                if (tcPaymentModes.SelectedTab != null)
                    tcPaymentModes.SelectedTab.Text = dr["PaymentMode"].ToString();
                log.Debug("Ends-btnPaymentMode_Click()");
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
            }
        }

        private List<TransactionPaymentsDTO> GetTransactionPaymentsDTOList(int paymentModeId)
        {
            List<TransactionPaymentsDTO> transactionPaymentsDTOList = null;
            POSUtils.SetLastActivityDateTime();
            if (paymentModeId >= 0 &&
                dgvSavedPayments != null &&
                dgvSavedPayments.Rows.Count > 0)
            {
                transactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                for (int i = 0; i < dgvSavedPayments.Rows.Count; i++)
                {
                    if (dgvSavedPayments.Rows[i].Tag != null && dgvSavedPayments.Rows[i].Tag is DataRow)
                    {
                        DataRow dr = dgvSavedPayments.Rows[i].Tag as DataRow;
                        if ((dr["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dr["PaymentModeId"])) == paymentModeId)
                        {
                            transactionPaymentsDTOList.Add(GetTransactionPaymentsDTO(dr));
                        }

                    }
                }
            }
            List<TransactionPaymentsDTO> nonReversedTransactionPaymentsDTOList = null;
            if (transactionPaymentsDTOList != null && transactionPaymentsDTOList.Count > 0)
            {
                nonReversedTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
                for (int i = 0; i < transactionPaymentsDTOList.Count; i++)
                {
                    bool paymentReversed = false;
                    for (int j = 0; j < transactionPaymentsDTOList.Count; j++)
                    {
                        if (transactionPaymentsDTOList[j].ParentPaymentId == transactionPaymentsDTOList[i].PaymentId)
                        {
                            paymentReversed = true;
                        }
                    }
                    if (paymentReversed == false)
                    {
                        nonReversedTransactionPaymentsDTOList.Add(transactionPaymentsDTOList[i]);
                    }
                }
            }
            return nonReversedTransactionPaymentsDTOList;
        }

        private TransactionPaymentsDTO GetTransactionPaymentsDTO(DataRow dataRow)
        {
            log.Debug("Starts-GetTransactionPaymentsDTO(dataRow) Method.");
            POSUtils.SetLastActivityDateTime();
            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO(Convert.ToInt32(dataRow["PaymentId"]),
                                                                                        dataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["TrxId"]),
                                                                                        dataRow["PaymentModeId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["PaymentModeId"]),
                                                                                        dataRow["Amount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["Amount"]),
                                                                                        dataRow["CreditCardNumber"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardNumber"]),
                                                                                        dataRow["NameOnCreditCard"] == DBNull.Value ? "" : Convert.ToString(dataRow["NameOnCreditCard"]),
                                                                                        dataRow["CreditCardName"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardName"]),
                                                                                        dataRow["CreditCardExpiry"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardExpiry"]),
                                                                                        dataRow["CreditCardAuthorization"] == DBNull.Value ? "" : Convert.ToString(dataRow["CreditCardAuthorization"]),
                                                                                        dataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardId"]),
                                                                                        dataRow["CardEntitlementType"] == DBNull.Value ? "" : Convert.ToString(dataRow["CardEntitlementType"]),
                                                                                        dataRow["CardCreditPlusId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CardCreditPlusId"]),
                                                                                        dataRow["OrderId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["OrderId"]),
                                                                                        dataRow["Reference"] == DBNull.Value ? "" : Convert.ToString(dataRow["Reference"]),
                                                                                        dataRow["Guid"] == DBNull.Value ? "" : Convert.ToString(dataRow["Guid"]),
                                                                                        dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                                                                        dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                                                                        dataRow["CCResponseId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["CCResponseId"]),
                                                                                        dataRow["Memo"] == DBNull.Value ? "" : Convert.ToString(dataRow["Memo"]),
                                                                                        dataRow["PaymentDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["PaymentDate"]),
                                                                                        dataRow["LastUpdatedUser"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdatedUser"]),
                                                                                        dataRow["ParentPaymentId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["ParentPaymentId"]),
                                                                                        dataRow["TenderedAmount"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["TenderedAmount"]),
                                                                                        dataRow["TipAmount"] == DBNull.Value ? 0d : Convert.ToDouble(dataRow["TipAmount"]),
                                                                                        dataRow["SplitId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["SplitId"]),
                                                                                        dataRow["PosMachine"] == DBNull.Value ? "" : Convert.ToString(dataRow["PosMachine"]),
                                                                                        dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                                                                        dataRow["CurrencyCode"] == DBNull.Value ? "" : Convert.ToString(dataRow["CurrencyCode"]),
                                                                                        dataRow["CurrencyRate"] == DBNull.Value ? (double?)null : Convert.ToDouble(dataRow["CurrencyRate"])
                                                                                        );
            log.Debug("Ends-GetTransactionPaymentsDTO(dataRow) Method.");
            return transactionPaymentsDTO;
        }

        /// <summary>
        /// Transfer entitlement from parent to child card
        /// </summary>
        /// <param name="parentChildCardDTO"></param>
        /// <param name="valueRequiredInChildCard"></param>
        /// <returns>true if successful</returns>
        private bool getParentChildCardEntitlementBalance(ParentChildCardsDTO parentChildCardDTO, double valueRequiredInChildCard)
        {
            log.LogMethodEntry(parentChildCardDTO, valueRequiredInChildCard);
            POSUtils.SetLastActivityDateTime();
            AccountDTO parentAccountDTO = new AccountBL(Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true).AccountDTO;
            decimal parentCardBalance =
                Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.CreditPlusCardBalance == null ? 0 : parentAccountDTO.AccountSummaryDTO.CreditPlusCardBalance)
                + Convert.ToDecimal(parentAccountDTO.AccountSummaryDTO.CreditPlusItemPurchase == null ? 0 : parentAccountDTO.AccountSummaryDTO.CreditPlusItemPurchase)
                + Convert.ToDecimal(parentAccountDTO.Credits == null ? 0 : parentAccountDTO.Credits);
            log.LogVariableState("Parent card balance: ", parentCardBalance);
            AccountDTO childAccountDTO = new AccountBL(Utilities.ExecutionContext, parentChildCardDTO.ChildCardId, true, true).AccountDTO;
            if (parentChildCardDTO.DailyLimitPercentage == null || parentChildCardDTO.DailyLimitPercentage == 0)
            {
                decimal childCardBalance =
                    Convert.ToDecimal(childAccountDTO.AccountSummaryDTO.CreditPlusCardBalance == null ? 0 : childAccountDTO.AccountSummaryDTO.CreditPlusCardBalance)
                    + Convert.ToDecimal(childAccountDTO.AccountSummaryDTO.CreditPlusItemPurchase == null ? 0 : childAccountDTO.AccountSummaryDTO.CreditPlusItemPurchase)
                    + Convert.ToDecimal(childAccountDTO.Credits == null ? 0 : childAccountDTO.Credits);
                log.LogVariableState("Child Card Balance: ", childCardBalance);
                if (parentCardBalance >= Convert.ToDecimal(valueRequiredInChildCard)
                    && childCardBalance < Convert.ToDecimal(valueRequiredInChildCard))
                {
                    Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                    entitlements.Add(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE), Convert.ToDecimal(valueRequiredInChildCard));
                    AccountBL parentAccountBL = new AccountBL(Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true);
                    AccountBL childAccountBL = new AccountBL(Utilities.ExecutionContext, childAccountDTO.AccountId, true, true);
                    string message = string.Empty;
                    if (TaskProcs.TranferEntitlementBalance(parentAccountBL, childAccountBL, entitlements, "Parent to child card transfer", ref message, -1, -1))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.Error("failure in transferring entitlements from Parent to Child card: " + entitlements + message);
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else //Daily Limit Percentage is available
            {
                DateTime serverTime = Utilities.getServerTime();
                int businessStartTime;
                if (int.TryParse(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext,
                                "BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
                {
                    businessStartTime = 6;
                }
                if (serverTime.Hour < businessStartTime)
                {
                    serverTime = serverTime.AddDays(-1).Date;
                }
                else
                {
                    serverTime = serverTime.Date;
                }
                //Check if Child Account was loaded with balance for the day
                decimal valueToTransfer = 0;
                valueToTransfer = parentCardBalance * Convert.ToInt32(parentChildCardDTO.DailyLimitPercentage) / 100;
                log.LogVariableState("Value to be transferred from parent to child based on Daily Limit Perc: ", valueToTransfer);
                if (childAccountDTO.AccountCreditPlusDTOList != null
                    && childAccountDTO.AccountCreditPlusDTOList.Exists(x => x.CreationDate != null
                                                                       && x.CreationDate > serverTime.AddHours(6)
                                                                       && x.CreationDate < serverTime.AddDays(1).AddHours(6)
                                                                       && x.SourceCreditPlusId > -1
                                                                       && (x.CreditPlusType == CreditPlusType.COUNTER_ITEM
                                                                          || x.CreditPlusType == CreditPlusType.CARD_BALANCE)
                                                                       && x.IsActive)
                   )
                {
                    decimal childAccountBalance = Convert.ToDecimal(childAccountDTO.AccountCreditPlusDTOList.Where(x => x.CreationDate != null
                                                                       && x.CreationDate > serverTime.AddHours(6)
                                                                       && x.CreationDate < serverTime.AddDays(1).AddHours(6)
                                                                       && x.SourceCreditPlusId > -1
                                                                       && (x.CreditPlusType == CreditPlusType.COUNTER_ITEM
                                                                          || x.CreditPlusType == CreditPlusType.CARD_BALANCE)
                                                                       && x.IsActive).Sum(x => x.CreditPlus));
                    log.LogVariableState("Total CreditPlus loaded to Child Account for the business day: ", childAccountBalance);
                    if (childAccountBalance > 0 && valueToTransfer > childAccountBalance)
                    {
                        valueToTransfer = valueToTransfer - childAccountBalance;
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                valueToTransfer = Decimal.Round(valueToTransfer, ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                log.LogVariableState("Required balance to transfer from parent to child card: ", valueToTransfer);
                Dictionary<string, decimal> entitlements = new Dictionary<string, decimal>();
                entitlements.Add(CreditPlusTypeConverter.ToString(CreditPlusType.CARD_BALANCE), valueToTransfer);
                AccountBL parentAccountBL = new AccountBL(Utilities.ExecutionContext, parentChildCardDTO.ParentCardId, true, true);
                AccountBL childAccountBL = new AccountBL(Utilities.ExecutionContext, childAccountDTO.AccountId, true, true);
                string message = string.Empty;
                if (TaskProcs.TranferEntitlementBalance(parentAccountBL, childAccountBL, entitlements, "Parent to child card transfer", ref message, -1, -1))
                {
                    log.LogMethodExit(true);
                    return true;
                }
                else
                {
                    log.Error("failure in transferring entitlements from Parent to Child card: " + entitlements + message);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            POSUtils.SetLastActivityDateTime();
        }

        private void dgvSavedPayments_CellMouseEnter(object sender, DataGridViewCellEventArgs e)//Begin Modification on 09-Nov-2015:Tip Feature.
        {
            log.Debug("Starts-dgvSavedPayments_CellMouseEnter()");
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.Info("Ends-dgvSavedPayments_CellMouseEnter() as e.ColumnIndex < 0 || e.RowIndex < 0");
                    return;
                }

                DataRow dr = dgvSavedPayments.Rows[e.RowIndex].Tag as DataRow;
                if (dr == null)
                    return;

                if (IsTipEnabled && !IsPaymentReversed(dr["PaymentId"]))
                {
                    string gateway = "";
                    if (dr["isCreditCard"].ToString()[0].Equals('Y'))
                    {
                        gateway = dr["Gateway"].ToString();
                    }
                    if (dgvSavedPayments.Columns[e.ColumnIndex].Name == "TipAmt" &&
                        dgvSavedPayments.Rows[e.RowIndex].Cells["TipAmt"].Value.Equals("0.00") &&
                        !dgvSavedPayments.Rows[e.RowIndex].Cells["paidPaymentAmount"].Value.ToString().Contains("-"))
                    {
                        bool flag = false;
                        if (dr["isCash"].ToString()[0].Equals('Y'))
                        {
                            flag = true;
                        }
                        else if (!(string.IsNullOrEmpty(gateway) && dr["isCreditCard"].ToString()[0].Equals('Y')))
                        {
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(gateway);
                            TransactionPaymentsDTO transactionPaymentsDTO = GetTransactionPaymentsDTO(dr);
                            if (paymentGateway.IsTipAllowed(transactionPaymentsDTO))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            dgvSavedPayments.Cursor = Cursors.Hand;
                            try
                            {
                                dgvSavedPayments[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvSavedPayments.DefaultCellStyle.Font, FontStyle.Underline);
                            }
                            catch (Exception ex)
                            {
                                log.Fatal("Ends-dgvSavedPayments_CellMouseEnter() due to exception " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch
            {
                log.Fatal("Ends-dgvSavedPayments_CellMouseEnter() due to exception in dgvSavedPayments");
            }
            log.Debug("Ends-dgvSavedPayments_CellMouseEnter()");
        }

        private void dgvSavedPayments_CellMouseLeave(object sender, DataGridViewCellEventArgs e)//Begin Modification on 09-Nov-2015:Tip Feature.
        {
            log.Debug("Starts-dgvSavedPayments_CellMouseLeave()");
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.Info("Ends-dgvSavedPayments_CellMouseLeave() as e.ColumnIndex < 0 || e.RowIndex < 0");
                    return;
                }
                if (IsTipEnabled)
                {
                    if (dgvSavedPayments.Columns[e.ColumnIndex].Name == "TipAmt" && dgvSavedPayments.Rows[e.RowIndex].Cells["TipAmt"].Value.Equals("0.00"))
                    {
                        if (!dgvSavedPayments.Rows[e.RowIndex].Cells["paidPaymentAmount"].Value.ToString().Contains("-"))
                        {

                            dgvSavedPayments.Cursor = Cursors.Default;
                            try
                            {
                                dgvSavedPayments[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvSavedPayments.DefaultCellStyle.Font, FontStyle.Regular);
                            }
                            catch (Exception ex)
                            {
                                log.Fatal("Ends-dgvSavedPayments_CellMouseLeave() due to exception " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch
            {
                log.Fatal("Ends-dgvSavedPayments_CellMouseLeave() due to exception in dgvSavedPayments");
            }

            log.Debug("Ends-dgvSavedPayments_CellMouseLeave()");
        }


        private void dgvGameCards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtMessage.Clear();
                if (e.RowIndex <= 0 && e.ColumnIndex == 1 && ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ENABLE_GAMECARD_KEYIN_FOR_PAYMENT", false))
                {
                    GenericDataEntry gde = new GenericDataEntry(1);
                    gde.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Enter Voucher / Card Number");

                    gde.DataEntryObjects[0].mandatory = false;
                    gde.DataEntryObjects[0].allowMinusSign = false;
                    gde.DataEntryObjects[0].label = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Voucher / Card Number");
                    gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                    gde.DataEntryObjects[0].width = 150;
                    gde.DataEntryObjects[0].uppercase = true;

                    if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (!string.IsNullOrEmpty(gde.DataEntryObjects[0].data))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            InsertCardDetails(gde.DataEntryObjects[0].data, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSplit_Click()");
            POSUtils.SetLastActivityDateTime();
            if (Utilities.getParafaitDefaults("SPLIT_PAYMENT_REQUIRES_MANAGER_APPROVAL").Equals("Y"))
            {
                int mgrId = -1;
                if (!Authenticate.Manager(ref mgrId))
                {
                    POSUtils.ParafaitMessageBox(MessageUtils.getMessage(1468), "Split Payment Approval");
                    log.Debug("Manager approval not accepted for Split payments.");
                    return;
                }
            }
            using (Payment.frmSplitPayments fsp = new Payment.frmSplitPayments(Transaction))
            {
                fsp.ShowDialog();
            }
            if (Transaction.Trx_id > 0)
            {
                TrxStatusChanged = true;
            }
            //Added code for final round off after split payments are complete and variation exists
            Transaction.getTotalPaidAmount(null);
            Decimal balanceAmount = (decimal)(Transaction.Net_Transaction_Amount - Transaction.TotalPaidAmount);
            if (balanceAmount != 0 && balanceAmount < 1 && balanceAmount > -1)
            {
                if (Utilities.ParafaitEnv.RoundOffPaymentModeId != -1)
                {
                    balanceAmount = Decimal.Round(balanceAmount, POSStatic.Utilities.ParafaitEnv.RoundingPrecision, MidpointRounding.AwayFromZero);
                    TransactionPaymentsDTO roundingTrxPaymentDTO = new TransactionPaymentsDTO();
                    roundingTrxPaymentDTO.PaymentModeId = Utilities.ParafaitEnv.RoundOffPaymentModeId;
                    roundingTrxPaymentDTO.Reference = "";
                    roundingTrxPaymentDTO.Amount = Convert.ToDouble(balanceAmount);
                    roundingTrxPaymentDTO.paymentModeDTO = new PaymentMode(Utilities.ExecutionContext, Utilities.ParafaitEnv.RoundOffPaymentModeId).GetPaymentModeDTO;
                    try
                    {
                        String paymentModeOTPValue = PerformPaymentModeOTPValidation(roundingTrxPaymentDTO.paymentModeDTO);
                        roundingTrxPaymentDTO.PaymentModeOTP = paymentModeOTPValue;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        txtMessage.Text = ex.Message;
                        log.LogMethodExit("PerformPaymentModeOTPValidation failed");
                        return;
                    }
                    roundingTrxPaymentDTO = GetEnabledAttributeDataForPaymentMode(roundingTrxPaymentDTO);
                    if (Transaction.Trx_id > 0)
                    {
                        roundingTrxPaymentDTO.TransactionId = Transaction.Trx_id;
                        TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, roundingTrxPaymentDTO);
                        transactionPaymentsBL.Save();
                    }
                    Transaction.TransactionPaymentsDTOList.Add(roundingTrxPaymentDTO);
                }
            }
            load();
            log.Debug("Ends-btnSplit_Click()");
        }

        //Added on 10-may-2016 for Adding the couponNumber Details
        private void dgvCouponValue_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCouponValue_CellClick()");
            txtMessage.Text = "";
            POSUtils.SetLastActivityDateTime();
            if (e.RowIndex < 0 && e.ColumnIndex == 1)
            {
                GenericDataEntry gde = new GenericDataEntry(1);
                gde.Text = "Enter Coupon Number";

                gde.DataEntryObjects[0].mandatory = false;
                gde.DataEntryObjects[0].allowMinusSign = false;
                gde.DataEntryObjects[0].label = "Coupon Number";
                gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.String;
                gde.DataEntryObjects[0].width = 150;
                gde.DataEntryObjects[0].uppercase = true;

                if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(gde.DataEntryObjects[0].data))
                    {
                        string couponNumber = gde.DataEntryObjects[0].data.ToString();
                        DataTable dt = ValidateCoupon(paymentModeId, couponNumber);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dgvCouponValue.Rows.Clear();
                            dgvCouponValue.Rows.Add(new object[] { dt.Rows[0]["CouponSetId"], couponNumber, dt.Rows[0]["CouponValue"] });
                        }
                        else
                        {
                            txtMessage.Text = "Invalid or Used Coupon Number";
                        }
                    }
                }
            }
            log.Debug("Ends-dgvCouponValue_CellClick()");
        }

        public DataTable ValidateCoupon(int paymentModeId, string CouponNumber)
        {
            POSUtils.SetLastActivityDateTime();
            DataTable dt = Utilities.executeDataTable(@"SELECT top 1 dc.CouponValue, dc.CouponSetId
                                                          FROM DiscountCoupons dc
                                                          WHERE PaymentModeId = @paymentModeId
                                                          AND ((Tonumber is null and FromNumber = @CouponNumber)
                                                                    OR Tonumber is not null   
                                                                    AND len(@couponNumber) = len(FromNumber)                                                 
                                                                    AND @CouponNumber between isnull(FromNumber, '') 
                                                                    AND isnull(ToNumber, 'zzzzzzzzzzzzzzzzzzzz'))
                                                                    AND (isnull(Count, 0) = 0 or count > (SELECT count(*)
                                                                                                          FROM DiscountCouponsUsed u
                                                                                                          WHERE u.CouponSetId = dc.CouponSetId
                                                                                                            and isnull(u.isactive, 'Y') = 'Y'))
                                                          AND not exists (SELECT 1
                                                                        FROM DiscountCouponsUsed u
                                                                        WHERE u.CouponSetId = dc.CouponSetId
                                                                         and isnull(u.isactive, 'Y') = 'Y'
                                                                        AND u.CouponNumber = @CouponNumber)",
                                                     new SqlParameter("@paymentModeId", paymentModeId),
                                                     new SqlParameter("@CouponNumber", CouponNumber));

            if (dt.Rows.Count > 0 && dt.Rows[0]["CouponSetId"].ToString() != null && dt.Rows[0]["CouponValue"].ToString() != null)
            {
                return dt;
            }
            else
                return dt = null;
        }

        private void btnCouponValue_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            double varAmount = NumberPadForm.ShowNumberPadForm(txtCouponValue.Text, "", Utilities);
            if (varAmount >= 0)
            {
                txtCouponValue.Text = varAmount.ToString(Utilities.ParafaitEnv.AMOUNT_FORMAT);
            }
            log.LogMethodExit();
        }

        private void txtCouponValue_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            CurrentTextBox = txtCouponValue;
            log.LogMethodExit();
        }

        private void txtCouponValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
                e.Handled = true;
            log.LogMethodExit();
        }
        //end 

        private void ShowTableAttributeUI(DataRow dr, bool deleteAllowed)
        {
            log.LogMethodEntry(dr, deleteAllowed);
            POSUtils.SetLastActivityDateTime();
            App.machineUserContext = Utilities.ExecutionContext;
            App.EnsureApplicationResources();
            TransactionPaymentsBL trxPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, Convert.ToInt32(dr["PaymentId"]));
            TransactionPaymentsDTO trxPaymentDTO = trxPaymentsBL.TransactionPaymentsDTO;
            PaymentMode paymentMode = new PaymentMode(Utilities.ExecutionContext, trxPaymentDTO.PaymentModeId);

            List<TableAttributeDetailsDTO> tableAttributeDetailsDTOList = TableAttributesUIHelper.GetEnabledAttributes(Utilities.ExecutionContext, EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode, paymentMode.GetPaymentModeDTO.Guid);
            if (tableAttributeDetailsDTOList != null && tableAttributeDetailsDTOList.Any())
            {
                bool readOnly = (deleteAllowed == false);
                trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO, true, readOnly, this.Handle);
                trxPaymentsBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                trxPaymentsBL.Save();
                TrxStatusChanged = true;
            }
            else
            {
                txtMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4102, (paymentMode.GetPaymentModeDTO != null ? paymentMode.GetPaymentModeDTO.PaymentMode
                                                                                                           : EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode.ToString()));
                //Attributes are not enabled for &1;
            }
            log.LogMethodExit();
        }

        private void btnTableAttributes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                App.machineUserContext = Utilities.ExecutionContext;
                App.EnsureApplicationResources();
                txtMessage.Clear();
                if (sender != null)
                {

                    Button btnPaymentMode = (Button)sender;
                    if (btnPaymentMode != null && btnPaymentMode.Tag != null)
                    {
                        PaymentModeDTO paymentModeDTO = (PaymentModeDTO)btnPaymentMode.Tag;
                        List<TableAttributeDetailsDTO> localTableAttributeDetailsDTOList = TableAttributesUIHelper.GetEnabledAttributes(Utilities.ExecutionContext,
                                                                                     EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode, paymentModeDTO.Guid);
                        if (localTableAttributeDetailsDTOList != null && localTableAttributeDetailsDTOList.Any())
                        {
                            TransactionPaymentsDTO transactionPaymentsDTO = new TransactionPaymentsDTO();
                            transactionPaymentsDTO.paymentModeDTO = paymentModeDTO;
                            transactionPaymentsDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                            if (preSelectedTableAttributeDetails.Key == null ||
                                (preSelectedTableAttributeDetails.Key != null && preSelectedTableAttributeDetails.Key.PaymentModeId != paymentModeDTO.PaymentModeId))
                            { //reset if mode is changed
                                preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>(paymentModeDTO, localTableAttributeDetailsDTOList);
                            }
                            else
                            {
                                localTableAttributeDetailsDTOList = preSelectedTableAttributeDetails.Value;
                                transactionPaymentsDTO = TableAttributesUIHelper.LoadObjectWithAttributeValues(Utilities.ExecutionContext, transactionPaymentsDTO, localTableAttributeDetailsDTOList);
                            }
                            bool canSkip = true;

                            transactionPaymentsDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, transactionPaymentsDTO, canSkip, false, this.Handle);
                            localTableAttributeDetailsDTOList = TableAttributesUIHelper.SetAttributeValues(Utilities.ExecutionContext, transactionPaymentsDTO, localTableAttributeDetailsDTOList);

                            if (localTableAttributeDetailsDTOList != null && localTableAttributeDetailsDTOList.Any() && localTableAttributeDetailsDTOList.Exists(tad => string.IsNullOrWhiteSpace(tad.AttributeValue) == false))
                            {
                                preSelectedTableAttributeDetails = new KeyValuePair<PaymentModeDTO, List<TableAttributeDetailsDTO>>(paymentModeDTO, localTableAttributeDetailsDTOList);
                            }
                        }
                        else
                        {
                            txtMessage.Text = MessageContainerList.GetMessage(Utilities.ExecutionContext, 4102, (paymentModeDTO != null ? paymentModeDTO.PaymentMode
                                                                                                                       : EnabledAttributesDTO.TableWithEnabledAttributes.PaymentMode.ToString()));
                            //Attributes are not enabled for &1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1824, ex.Message), MessageContainerList.GetMessage(Utilities.ExecutionContext, "Payment Attributes"));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private TransactionPaymentsDTO GetEnabledAttributeDataForPaymentMode(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);
            POSUtils.SetLastActivityDateTime();
            App.machineUserContext = Utilities.ExecutionContext;
            App.EnsureApplicationResources();
            if (preSelectedTableAttributeDetails.Key != null
                && preSelectedTableAttributeDetails.Key.PaymentModeId == transactionPaymentsDTO.PaymentModeDTO.PaymentModeId
                && preSelectedTableAttributeDetails.Value.Any() && preSelectedTableAttributeDetails.Value.Exists(tad => string.IsNullOrWhiteSpace(tad.AttributeValue) == false))
            {
                transactionPaymentsDTO = TableAttributesUIHelper.LoadObjectWithAttributeValues(Utilities.ExecutionContext, transactionPaymentsDTO, preSelectedTableAttributeDetails.Value);
            }
            else
            {
                transactionPaymentsDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, transactionPaymentsDTO, false, false, this.Handle);
            }
            log.LogMethodExit(transactionPaymentsDTO);
            return transactionPaymentsDTO;
        }


        private void SetPaymentModeOnAttributeButton(int selectedPaymentModeId)
        {
            log.LogMethodEntry(selectedPaymentModeId);
            POSUtils.SetLastActivityDateTime();
            List<Button> payModeBtnList = flpPaymentModes.Controls.OfType<Button>().ToList();
            if (payModeBtnList != null)
            {
                foreach (Button paymentModeBtn in payModeBtnList)
                {
                    if (paymentModeBtn.Tag != null)
                    {
                        DataRow dr = paymentModeBtn.Tag as DataRow;
                        if (dr[0] != null || dr[0] != DBNull.Value)
                        {
                            int localPaymentModeId = Convert.ToInt32(dr[0]);
                            if (localPaymentModeId == selectedPaymentModeId)
                            {
                                //paymentModeBtn.PerformClick();
                                paymentModeId = localPaymentModeId;
                                PaymentMode paymentModesBL = new PaymentMode(Utilities.ExecutionContext, paymentModeId);
                                if (dr["isCash"].ToString().Equals("Y"))
                                {
                                    btnCashAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                                }
                                else if (dr["isDebitCard"].ToString().Equals("Y"))
                                {
                                    btnGCAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                                }
                                else if (dr["isCreditCard"].ToString().Equals("Y"))
                                {
                                    btnCCAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                                }
                                else
                                {
                                    btnOtherAttributes.Tag = (paymentModesBL != null ? paymentModesBL.GetPaymentModeDTO : null);
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private PaymentModeDTO GetSelectedPaymentMode(List<PaymentModeDTO> paymentModeDTOList)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            PaymentModeDTO selectedPaymentModeDTO = paymentModeDTOList[0];
            if (paymentModeId > 0 && paymentModeDTOList.Exists(pm => pm.PaymentModeId == paymentModeId))
            {
                selectedPaymentModeDTO = paymentModeDTOList.Find(pm => pm.PaymentModeId == paymentModeId);
            }
            log.LogMethodExit();
            return selectedPaymentModeDTO;
        }
        private int GetSelectedIdforMode(string paymentModeType)
        {
            log.LogMethodEntry(paymentModeType);
            POSUtils.SetLastActivityDateTime();
            int selectedPaymentModeId = -1;
            List<int> idList = new List<int>();
            switch (paymentModeType)
            {
                case "CASH":
                    idList= Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                                          && x.paymentModeDTO.IsCash).Select(x => x.paymentModeDTO.PaymentModeId).ToList();
                    break;
                case "CREDITCARD":
                    idList = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                                         && x.paymentModeDTO.IsCreditCard).Select(x => x.paymentModeDTO.PaymentModeId).ToList();
                    break;
                case "GAMECARD":
                    idList = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                                         && x.paymentModeDTO.IsDebitCard).Select(x => x.paymentModeDTO.PaymentModeId).ToList();
                    break;
                case "OTHERS":
                    idList = Transaction.TransactionPaymentsDTOList.Where(x => x.PaymentId == -1 && x.paymentModeDTO != null
                                                                                         && !x.paymentModeDTO.IsCash
                                                                                         && !x.paymentModeDTO.IsCreditCard
                                                                                         && !x.paymentModeDTO.IsDebitCard).Select(x => x.paymentModeDTO.PaymentModeId).ToList();
                    break;
                default:
                    break;
            }
            if (idList != null && idList.Any())
            {
                selectedPaymentModeId = idList[0];
            }
            log.LogMethodExit(selectedPaymentModeId);
            return selectedPaymentModeId;
        }
        private string PerformPaymentModeOTPValidation(PaymentModeDTO paymentModeDTO)
        {
            log.LogMethodEntry(paymentModeDTO);
            POSUtils.SetLastActivityDateTime();
            string paymentModeOTPValue = string.Empty;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                KeyValuePair<string, ApprovalAction> keyValuePair = frmVerifyPaymentModeOTP.PerformPaymentModeOTPValidation(Utilities, paymentModeDTO, this.Transaction, dgvGameCards);
                paymentModeOTPValue = keyValuePair.Key;
                if (keyValuePair.Value != null)
                {
                    string keyValue = keyValuePair.Key + "-" + ServerDateTime.Now.ToString("yyyyMMddHHmmss");
                    paymentModeOTPApprovals.Add(keyValue, keyValuePair.Value);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(paymentModeOTPValue);
            return paymentModeOTPValue;
        }

        private int CallTransactionSaveOrder(ref string lmessage, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(lmessage, sqlTrx);
            this.Cursor = Cursors.WaitCursor;
            int retValue = -1;
            UIActionStatusLauncher uiActionStatusLauncher = null;
            try
            {
                string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Saving Transaction.") + " " +
                              MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                bool showProgress = true;
                POSUtils.SetLastActivityDateTime();
                uiActionStatusLauncher = new UIActionStatusLauncher( msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                POSUtils.SetLastActivityDateTime();
                Transaction.SetStatusProgressMsgQueue = statusProgressMsgQueue;
                retValue = Transaction.SaveOrder(ref lmessage, sqlTrx);
                POSUtils.SetLastActivityDateTime();
                UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
            }
            finally
            {
                if (uiActionStatusLauncher != null)
                {
                    uiActionStatusLauncher.Dispose();
                    statusProgressMsgQueue = null;
                }
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit(retValue);
            return retValue;
        }

        private bool CallTrxCreatePaymentInfo(ref string lmessage, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(lmessage, sqlTrx);
            this.Cursor = Cursors.WaitCursor;
            bool retValue = false;
            UIActionStatusLauncher uiActionStatusLauncher = null;
            try
            {
                string msg = MessageContainerList.GetMessage(Utilities.ExecutionContext, "Saving transaction payments.") + " " +
                              MessageContainerList.GetMessage(Utilities.ExecutionContext, 684);// "Please wait..." 

                statusProgressMsgQueue = new ConcurrentQueue<KeyValuePair<int, string>>();
                bool showProgress = true;
                POSUtils.SetLastActivityDateTime();

                uiActionStatusLauncher = new UIActionStatusLauncher(msg, RaiseFocusEvent, statusProgressMsgQueue, showProgress, BackgroundProcessRunner.LaunchWaitScreenAfterXSeconds);
                Transaction.SetStatusProgressMsgQueue = statusProgressMsgQueue;
                POSUtils.SetLastActivityDateTime();
                retValue = Transaction.CreatePaymentInfo(sqlTrx, ref lmessage);
                if (retValue)
                {
                    try
                    {
                        Transaction.CreateTransactionLineForCreditCardSurcharge(sqlTrx);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        retValue = false;
                        lmessage = ex.Message;
                    }
                }
                POSUtils.SetLastActivityDateTime();
                UIActionStatusLauncher.SendMessageToStatusMsgQueue(statusProgressMsgQueue, "CLOSEFORM", 100, 100);
            }
            finally
            {
                if (uiActionStatusLauncher != null)
                {
                    uiActionStatusLauncher.Dispose();
                    statusProgressMsgQueue = null;
                }
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit(retValue);
            return retValue;
        }
        delegate void RaiseFocusEventback();
        public void RaiseFocusEvent()
        {
            log.LogMethodEntry();
            try
            {
                if (this.InvokeRequired)
                {
                    RaiseFocusEventback d = new RaiseFocusEventback(RaiseFocusEvent);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    POSUtils.SetLastActivityDateTime();
                    if (Application.OpenForms.Count > 2)
                    {
                        if (Application.OpenForms[Application.OpenForms.Count - 1].TopMost == false)
                        {
                            Application.OpenForms[Application.OpenForms.Count - 1].TopMost = true;
                            Application.OpenForms[Application.OpenForms.Count - 1].BringToFront();
                            Application.OpenForms[Application.OpenForms.Count - 1].TopMost = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void DisableButtons()
        {
            log.LogMethodEntry();
            this.Enabled = false;
            Application.DoEvents();
            log.LogMethodExit();
        }
        private void EnableButtons()
        {
            log.LogMethodEntry();
            Application.DoEvents();
            this.Enabled = true;
            Application.DoEvents();
            log.LogMethodExit();
        }
        private void displayMessageLine(string message, string msgType)
        {
            log.Debug("Starts-displayMessageLine(" + message + "," + msgType + ")");
            try
            {
                if (txtMessage.InvokeRequired)
                {
                    switch (msgType)
                    {
                        case "WARNING":
                            txtMessage.Invoke(new Action(() => txtMessage.BackColor = Color.Yellow));
                            txtMessage.Invoke(new Action(() => txtMessage.ForeColor = Color.Black));
                            break;
                        case "ERROR":
                            txtMessage.Invoke(new Action(() => txtMessage.BackColor = Color.Red));
                            txtMessage.Invoke(new Action(() => txtMessage.ForeColor = Color.White));
                            break;
                        case "MESSAGE":
                            txtMessage.Invoke(new Action(() => txtMessage.BackColor = Color.White));
                            txtMessage.Invoke(new Action(() => txtMessage.ForeColor = Color.Black));
                            break;
                        default:
                            txtMessage.ForeColor = Color.Black;
                            break;
                    }
                    txtMessage.Invoke(new Action(() => txtMessage.Text = message));
                }
                else
                {
                    switch (msgType)
                    {
                        case "WARNING":
                            txtMessage.BackColor = Color.Yellow;
                            txtMessage.ForeColor = Color.Black;
                            break;
                        case "ERROR":
                            txtMessage.BackColor = Color.Red;
                            txtMessage.ForeColor = Color.White;
                            break;
                        case "MESSAGE":
                            txtMessage.BackColor = Color.White;
                            txtMessage.ForeColor = Color.Black;
                            break;
                        default:
                            txtMessage.ForeColor = Color.Black;
                            break;
                    }

                    txtMessage.Text = message;
                }
            }
            catch
            {
                log.Error("Error in -displayMessageLine(" + message + "," + msgType + ")");//Added Error log exception on Mar-03-2017
            }
            log.Debug("Ends-displayMessageLine(" + message + "," + msgType + ")");
        }
    }
}


