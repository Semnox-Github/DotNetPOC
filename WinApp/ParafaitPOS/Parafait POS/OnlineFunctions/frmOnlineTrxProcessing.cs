/********************************************************************************************
* Project Name - Parafait_POS - frmOnlineTrxProcessing
* Description  - frmOnlineTrxProcessing 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online transaction in Kiosk changes 
*2.50.0      18-Dec-2018      Mathew Ninan       Modified to remove Staticdataexchange as its deprecated.
*                                                Used TransactionPayments where applicable
*2.60.0      22-Feb-2019      Guru S A           Product details column is added to transaction line data grid and OTP validation changes
*2.60.0      21-Mar-2019      Iqbal              Explorium changes
*2.80.0      21-Oct-2019      Guru S A           Waiver phase 2 changes
*2.150.9     22-Mar-2024      Vignesh Bhat       Modified: Remove  Waiver validation for past transaction date
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Device;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Parafait_POS.Waivers;
using Semnox.Parafait.Waiver;

namespace Parafait_POS
{
    public partial class frmOnlineTrxProcessing : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        int trxId = 0;
        TextBox currentTextBox = null;
        public int ReopenedTrxId = -1;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        int checkboxXLocation;
        FiscalPrinter fiscalPrinter;
        public frmOnlineTrxProcessing(FiscalPrinter _FiscalPrinter)
        {
            log.LogMethodEntry(_FiscalPrinter);
            fiscalPrinter = _FiscalPrinter;
            Logger.setRootLogLevel(log);

            InitializeComponent();

            dgvTrxLines.Columns["LineAmount"].DefaultCellStyle =
                dgvTrxHeader.Columns["Amount"].DefaultCellStyle = POSStatic.Utilities.gridViewAmountWithCurSymbolCellStyle();
            dgvTrxHeader.Columns["Date"].DefaultCellStyle = POSStatic.Utilities.gridViewDateTimeCellStyle();
            dgvTrxLines.Columns["Quantity"].DefaultCellStyle =
                dgvTrxHeader.Columns["TrxId"].DefaultCellStyle = POSStatic.Utilities.gridViewNumericCellStyle();
            Product.ReadOnly = Quantity.ReadOnly = Amount.ReadOnly = CardNumber.ReadOnly = true;
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
            }
            checkboxXLocation = ChkBxSelectAll.Location.X;
            log.LogMethodExit();
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Utilities.ProcessScannedBarCode(checkScannedEvent.Message, Utilities.ParafaitEnv.LEFT_TRIM_BARCODE, Utilities.ParafaitEnv.RIGHT_TRIM_BARCODE);
                //Invoke assignment in same UI thread 29-Mar-2016
                this.Invoke((MethodInvoker)delegate
                {
                    if (currentTextBox != null)
                    {
                        currentTextBox.Text = scannedBarcode;
                    }
                    else
                    {
                        txtTransactionOTP.Text = scannedBarcode;
                    }
                    btnGetDetails.PerformClick();

                });
            }
            log.LogMethodExit();
        }
        Transaction Trx = null;

        private void btnGetDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            object o = null;
            try
            {
                if ((string.IsNullOrEmpty(txtTransactionId.Text.Trim())) && (string.IsNullOrEmpty(txtTransactionOTP.Text.Trim())))
                {
                    log.LogMethodExit("OTP and TransactionId is null ");
                    return;
                }
                //Added  the below conditions  to accept either Reference Number or Transaction Id  for printing Online reservation cards on May 22, 2015//
                else if ((string.IsNullOrEmpty(txtTransactionId.Text.Trim())) && (!string.IsNullOrEmpty(txtTransactionOTP.Text.Trim())))
                {
                    o = Utilities.executeScalar("select trxId from trx_header where TransactionOTP = @trxOTP and TrxDate >= Cast(getdate()-300 as date) and Status not in ('CANCELLED','SYSTEMABANDONED')  ", new SqlParameter("@trxOTP", txtTransactionOTP.Text));
                    trxId = Convert.ToInt32(o);
                }
                else if ((!string.IsNullOrEmpty(txtTransactionId.Text.Trim())) && (string.IsNullOrEmpty(txtTransactionOTP.Text.Trim())))
                {
                    o = Utilities.executeScalar("select trxId from trx_header where Original_System_Reference = @trxId", new SqlParameter("@trxId", txtTransactionId.Text));
                    trxId = Convert.ToInt32(o);
                }
                else if ((!string.IsNullOrEmpty(txtTransactionId.Text.Trim())) && (!string.IsNullOrEmpty(txtTransactionOTP.Text.Trim())))
                {
                    o = Utilities.executeScalar("select trxId  from trx_header where TransactionOTP = @trxOTP and Original_System_Reference = @trxId", new SqlParameter("@trxId", txtTransactionId.Text), new SqlParameter("@trxOTP", txtTransactionOTP.Text));
                    trxId = Convert.ToInt32(o);
                }

                if (o == null)
                {
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, "No Transaction exists with the entered OTP/Transaction Ref Id"));
                    log.LogMethodExit("No Transaction exists with the entered OTP/Transaction Ref Id");
                    return;
                }

                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                Trx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);

                dgvTrxHeader.Rows.Clear();
                string customerName = "";
                if (Trx.PrimaryCard != null && Trx.PrimaryCard.customerDTO != null)
                    customerName = Trx.PrimaryCard.customerDTO.FirstName + " " + Trx.PrimaryCard.customerDTO.LastName;
                else if (Trx.customerDTO != null && Trx.customerDTO.Id >= 0)
                    customerName = Trx.customerDTO.FirstName + " " + Trx.customerDTO.LastName;
                dgvTrxHeader.Rows.Add(Trx.Trx_id, Trx.Trx_No, Trx.TransactionDate, customerName, Trx.Net_Transaction_Amount, Trx.Status, Trx.PaymentReference);

                dgvTrxLines.Rows.Clear();
                foreach (Transaction.TransactionLine tl in Trx.TrxLines)
                {
                    //dgvTrxLines.Rows.Add(tl.ProductName, tl.quantity, tl.LineAmount, tl.CardNumber, (!string.IsNullOrEmpty(tl.CardNumber) && tl.CardNumber.StartsWith("T")));//Starts:Modification on 29-Sep-2016 for adding check box
                    if (!tl.ProductTypeCode.Equals("LOCKERDEPOSIT") && !tl.ProductTypeCode.Equals("DEPOSIT") && !tl.ProductTypeCode.Equals("CARDDEPOSIT"))
                    {
                        string productDetails = null;
                        if (tl.ProductTypeCode.Equals("ATTRACTION"))
                        {
                            productDetails = tl.AttractionDetails;
                        }
                        else if (tl.ProductTypeCode.Equals("LOCKER"))
                        {
                            productDetails = tl.LockerName + " : " + tl.LockerNumber + " : " + (tl.lockerAllocationDTO != null ? tl.lockerAllocationDTO.ValidToTime.ToString(Utilities.ParafaitEnv.DATETIME_FORMAT) : "");
                        }
                        dgvTrxLines.Rows.Add(tl.ProductName, tl.quantity, tl.LineAmount, tl.CardNumber, productDetails, false);//Starts:Modification on 29-Sep-2016 for adding check box
                        dgvTrxLines.Rows[dgvTrxLines.Rows.Count - 1].Cells["issueCardColumn"].ReadOnly = !(!string.IsNullOrEmpty(tl.CardNumber) && tl.CardNumber.StartsWith("T"));//Ends:Modification on 29-Sep-2016 for adding check box
                    }
                }
                if (dgvTrxLines.Rows.Count > 0)
                    btnPrint.Enabled = true;

                if (Trx != null && Trx.WaiverSignatureRequired())
                {
                    btnMapWaiver.Enabled = true;
                }
                else
                {
                    btnMapWaiver.Enabled = false;
                }

                //Start Modification : Added code on 6-feb-2016 to dispaly the status
                #region Added code to display the status of issued cards
                if (dgvTrxLines.Rows.Count > 0)
                {
                    try
                    {
                        lblStatus.Visible = true;
                        ChkBxSelectAll.Checked = false;

                        if (dgvTrxLines.Height > dgvTrxLines.Rows.GetRowsHeight(DataGridViewElementStates.Visible))
                        {
                            //scroll not visible
                            ChkBxSelectAll.Location = new Point(checkboxXLocation, ChkBxSelectAll.Location.Y);
                        }
                        else
                        {
                            //scroll visible
                            if (ChkBxSelectAll.Location.X == checkboxXLocation)
                            {
                                ChkBxSelectAll.Location = new Point(ChkBxSelectAll.Location.X - 17, ChkBxSelectAll.Location.Y);
                            }
                        }

                        List<Transaction.TransactionLine> TrxCardLines = new List<Transaction.TransactionLine>();
                        TrxCardLines = Trx.TrxLines.FindAll(x => (x.CardNumber != null && x.LineValid && (!x.ProductTypeCode.Equals("LOCKERDEPOSIT") && !x.ProductTypeCode.Equals("DEPOSIT") && !x.ProductTypeCode.Equals("CARDDEPOSIT"))));
                        int notIssuedcardCount = TrxCardLines.FindAll(x => (x.CardNumber.StartsWith("T") && (!x.ProductTypeCode.Equals("LOCKERDEPOSIT") && !x.ProductTypeCode.Equals("DEPOSIT") && !x.ProductTypeCode.Equals("CARDDEPOSIT")))).Count;
                        int issuedCardCount = TrxCardLines.Count - notIssuedcardCount;
                        lblStatus.Text = MessageContainer.GetMessage(Utilities.ExecutionContext, "Printed card count : " + issuedCardCount + "        Remaining temp card count : " + notIssuedcardCount);
                    }
                    catch (Exception ex) { log.Error(ex); }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1824, ex.Message));
            }
            #endregion
            log.LogMethodExit();
        }

        bool executeTransaction(Dictionary<string, string> cardList, ref string message)
        {
            log.LogMethodEntry(cardList, message);
            SqlTransaction SQLTrx = POSStatic.Utilities.createConnection().BeginTransaction();
            SqlConnection cnn = SQLTrx.Connection;
            try
            {
                if (cardList.Count > 0)
                {
                    TaskProcs tp = new TaskProcs(Utilities);

                    foreach (KeyValuePair<string, string> keyv in cardList)
                    {
                        if (keyv.Key.Equals(keyv.Value))
                            continue;
                        Card tempCard = new Card(keyv.Key, "", Utilities);
                        if (tempCard.CardStatus.Equals("NEW"))
                        {
                            message = MessageContainer.GetMessage(Utilities.ExecutionContext, 1684, keyv.Key);
                            SQLTrx.Rollback();
                            log.LogMethodExit(false, message);
                            return false;
                        }
                        Card card = new Card(keyv.Value, "", Utilities);
                        if (card.CardStatus == "ISSUED")
                        {
                            btnPrint.Enabled = true;
                            List<Card> cards = new List<Card>();
                            cards.Add(tempCard);
                            cards.Add(card);

                            if (!tp.Consolidate(cards, 2, "Execute Transaction", ref message, SQLTrx, true))
                            {
                                if (SQLTrx.Connection != null)
                                    SQLTrx.Rollback();

                                log.LogMethodExit(false, message);
                                return false;
                            }
                        }
                        else
                        {
                            if (!tp.transferCard(tempCard, card, "Execute Transaction", ref message, SQLTrx))
                            {
                                if (SQLTrx.Connection != null)
                                    SQLTrx.Rollback();
                                log.LogMethodExit(false, message);
                                return false;
                            }
                        }
                    }
                }

                SQLTrx.Commit();
            }
            catch (Exception ex)
            {
                if (SQLTrx.Connection != null)
                    SQLTrx.Rollback();
                message = ex.Message;
                log.Error(ex);
                log.LogMethodExit(false);
                return false;
            }
            finally
            {
                cnn.Close();
            }

            log.LogMethodExit();
            return true;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Trx == null)
            {
                log.LogMethodExit(Trx, " Trx == null");
                return;
            }
            else if (Trx.Status == Transaction.TrxStatus.CANCELLED)
            {
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction is already cancelled"));
                log.LogMethodExit(Trx, "Trx.Status == Transaction.TrxStatus.CANCELLED");
                return;
            }
            else if (Trx.Status == Transaction.TrxStatus.SYSTEMABANDONED)
            {
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction is already abandoned by the system"));
                log.LogMethodExit(Trx, "Trx.Status == Transaction.TrxStatus.SYSTEMABANDONED");
                return;
            }

            //Start Modification on 25-Jan-2016 for avoiding the complete transaction when trx date is greater than current date
            if (Trx.TransactionDate.Date > DateTime.Now.Date)
            {
                if (DialogResult.Yes != POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1129, Trx.TransactionDate.Date), "Warning", MessageBoxButtons.YesNo))
                    return;
            }
            //End Modification on 25-Jan-2016 for avoiding the complete transaction when trx date is greater than current date
            try
            {
                string waiverMsg = string.Empty;
                foreach (Transaction.TransactionLine tl in Trx.TrxLines)//Starts:Modification on 29-Sep-2016 for adding check box
                {
                    foreach (DataGridViewRow dr in dgvTrxLines.Rows)
                    {
                        if (dr.Cells["CardNumber"].Value != null && dr.Cells["CardNumber"].Value.Equals(tl.CardNumber))
                        {
                            int lineId = Trx.TrxLines.IndexOf(tl);
                            tl.LineValid = dr.Cells["issueCardColumn"].Value.Equals(true);
                            if (tl.LineValid && Trx.IsWaiverSignaturePending(lineId))
                            {
                                waiverMsg = waiverMsg + MessageContainer.GetMessage(Utilities.ExecutionContext, 2353, tl.CardNumber) + Environment.NewLine;
                                //Waiver signing is pending for transaction line with card number &1. 
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(waiverMsg) == false)
                {
                    //Waiver signing is pending do not proceed
                    waiverMsg = waiverMsg + MessageContainer.GetMessage(Utilities.ExecutionContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, waiverMsg));
                    log.LogMethodExit(waiverMsg);
                    return;
                }
                Reservation.frmInputPhysicalCards fin = new Reservation.frmInputPhysicalCards(Trx);

                if (fin.cardList.Count > 0) // card lines found
                {
                    if (fin.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    {
                        log.LogMethodExit(" Dialog is Cancelled ");
                        return;
                    }

                    string message = "";
                    if (executeTransaction(fin.cardList, ref message))
                    {
                        btnPrint.Enabled = true;
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1702));
                        log.Info("TEMP cards converted to physical cards successfully ");
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(message);
                        log.Error(" Cannot convert TEMP cards to physical cards error " + message);
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1703));
                    log.Info("btnExecute_Click() - No TEMP cards to convert to physical cards");
                }
                btnGetDetails.PerformClick();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void frmOnlineTrxProcessing_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.ActiveControl = txtTransactionOTP;
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                bool IsRecordToPrint = false;
                string message = "";
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                Trx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER").Equals("Y"))//Begin: Modified for adding fiscal print option on 09-MAY-2016
                {
                    if (fiscalPrinter.PrintReceipt(trxId, ref message, null, 0, false, false))
                    {
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1704));
                        log.Info("Physical cards printed successfully");
                    }
                }
                else
                {
                    string waiverMsg = string.Empty;
                    foreach (Transaction.TransactionLine tl in Trx.TrxLines)//Starts:Modification on 29-Sep-2016 for adding check box
                    {
                        foreach (DataGridViewRow dr in dgvTrxLines.Rows)
                        {
                            if (dr.Cells["CardNumber"].Value != null && dr.Cells["CardNumber"].Value.Equals(tl.CardNumber))
                            {
                                int lineId = Trx.TrxLines.IndexOf(tl);
                                tl.LineValid = (!tl.CardNumber.StartsWith("T") && !tl.ReceiptPrinted); //|| (dr.Cells["issueCardColumn"].Value.Equals(true) && !tl.ReceiptPrinted && tl.CardNumber.StartsWith("T")));
                                if (tl.LineValid && Trx.IsWaiverSignaturePending(lineId))
                                {
                                    waiverMsg = waiverMsg + MessageContainer.GetMessage(Utilities.ExecutionContext, 2353, tl.CardNumber) + Environment.NewLine;
                                    //Waiver signing is pending for transaction line with card number &1.
                                }

                            }
                            else if (string.IsNullOrEmpty(tl.CardNumber))
                            {
                                tl.LineValid = !tl.ReceiptPrinted;
                            }
                        }
                        if (tl.LineValid)
                        {
                            IsRecordToPrint = true;
                        }
                    }
                    if (string.IsNullOrEmpty(waiverMsg) == false)
                    {   //Waiver signing is pending do not proceed
                        waiverMsg = waiverMsg + MessageContainer.GetMessage(Utilities.ExecutionContext, 2354);//Please complete the waiver signature formalities or uncheck the line entry to proceed with rest
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, waiverMsg));
                        log.LogMethodExit(waiverMsg);
                        return;
                    }
                    PrintTransaction printTransaction = new PrintTransaction(POSStatic.POSPrintersDTOList);
                    if (IsRecordToPrint)
                    {
                        if (printTransaction.Print(Trx, ref message, false, false))
                        {
                            POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1704));
                            log.Info("Physical cards printed successfully");
                        }
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1705));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1824, ex.Message));
            }

            log.LogMethodExit();
        }

        private void btnReopen_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            DialogResult dr = POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 2071), MessageContainer.GetMessage(Utilities.ExecutionContext, "Trx Reopen"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //You can reschedule the transaction if you intend to only change the transaction date. Do you want to reschedule instead of reopening?
            if (dr == DialogResult.Cancel)
            {
                log.LogMethodExit("dr == DialogResult.Cancel");
                return;
            }
            else if (dr == DialogResult.Yes)
            {
                if (Trx != null && Trx.Status == Transaction.TrxStatus.CLOSED)
                {
                    OnlineFunctions.frmRescheduleTrx rescheduleForm = new OnlineFunctions.frmRescheduleTrx(Trx);
                    rescheduleForm.StartPosition = FormStartPosition.Manual;
                    rescheduleForm.Location = PointToScreen(btnChooseProduct.Location);
                    rescheduleForm.ShowDialog();
                }
                else
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 2072));//"Transaction cannot be rescheduled"

                log.LogMethodExit();
                return;
            }

            try
            {
                bool reopenAllowed = false;
                if (Trx != null && Trx.Status == Transaction.TrxStatus.CLOSED && !String.IsNullOrEmpty(Trx.originalSystemReference))
                {
                    for (int i = 0; i < Trx.TrxLines.Count; i++)
                    {
                        if (Trx.TrxLines[i].CardNumber != null && Trx.TrxLines[i].CardNumber.StartsWith("T"))
                        {
                            reopenAllowed = true;
                            break;
                        }
                    }

                    if (reopenAllowed)
                    {
                        SqlTransaction sqlTrxn = POSStatic.Utilities.createConnection().BeginTransaction();
                        SqlConnection cnn = sqlTrxn.Connection;

                        try
                        {
                            string Message = "";
                            int reverseTrxId = 0;
                            int OldtrxId = 0;

                            //Get payment details from original transactions before reversal
                            TransactionPaymentsListBL trxPaymentsListBL = new TransactionPaymentsListBL();
                            List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                            trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, Trx.Trx_id.ToString()));
                            List<TransactionPaymentsDTO> transactionPaymentsDTOList = trxPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(trxPaymentSearchParameters, null, sqlTrxn);

                            SqlCommand cmd = Utilities.getCommand(cnn);
                            cmd.Transaction = sqlTrxn;
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@OldTrxId", Trx.Trx_id);
                            cmd.Parameters.AddWithValue("@lastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                            cmd.CommandText = "Update trx_header set status='CANCELLED', LastUpdateTime = getdate(), LastUpdatedBy = @lastUpdatedBy where trxId = @OldTrxId";
                            cmd.ExecuteNonQuery();

                            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                            if (!TransactionUtils.ReverseTransactionEntity(Trx.Trx_id, -1, Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.User_Id, null, "Reversing the transaction for reopening.", ref Message, ref reverseTrxId, sqlTrxn, cnn))
                            {
                                if (sqlTrxn.Connection != null)
                                    sqlTrxn.Rollback();
                                btnGetDetails.PerformClick();
                                POSUtils.ParafaitMessageBox(Message);
                                log.Error("ReverseTransactionEntity has error: " + Message);
                                log.LogMethodExit();
                                return;
                            }
                            log.LogVariableState("reverseTrxId", reverseTrxId);
                            OldtrxId = Trx.Trx_id;
                            log.LogVariableState("OldtrxId", OldtrxId);
                            if (TransactionUtils.ReverseCard(OldtrxId, -1, Utilities.ParafaitEnv.LoginID, true, ref Message, reverseTrxId, sqlTrxn, cnn) == false)
                            {
                                if (sqlTrxn.Connection != null)
                                    sqlTrxn.Rollback();
                                btnGetDetails.PerformClick();
                                POSUtils.ParafaitMessageBox(Message);
                                log.Error("ReverseCard has error: " + Message);
                                log.LogMethodExit();
                                return;
                            }
                            Trx.Trx_id = 0;
                            for (int i = 0; i < Trx.TrxLines.Count; i++)
                            {
                                Trx.TrxLines[i].DBLineId = 0;
                                Trx.TrxLines[i].LineValid = true;
                            }
                            Trx.GameCardReadTime = Utilities.getServerTime();
                            Trx.Status = Transaction.TrxStatus.OPEN;
                            Trx.Utilities.ParafaitEnv.POSTypeId = Trx.POSTypeId;
                            int retcode = Trx.SaveOrder(ref Message, sqlTrxn);
                            if (retcode != 0)
                            {
                                if (sqlTrxn.Connection != null)
                                    sqlTrxn.Rollback();
                                POSUtils.ParafaitMessageBox(Message);
                                btnGetDetails.PerformClick();
                                log.Error("SaveOrder as retcode != 0 error: " + Message);
                                log.LogMethodExit();
                                return;
                            }
                            else
                            {
                                if (transactionPaymentsDTOList != null)
                                {
                                    foreach (TransactionPaymentsDTO trxPaymentDTO in transactionPaymentsDTOList)
                                    {
                                        trxPaymentDTO.TransactionId = Trx.Trx_id;
                                        trxPaymentDTO.PaymentId = -1;
                                        TransactionPaymentsBL trxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, trxPaymentDTO);
                                        trxPaymentBL.Save(sqlTrxn);
                                    }
                                }
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@TrxId", Trx.Trx_id);
                                cmd.Parameters.AddWithValue("@OldTrxId", OldtrxId);
                                cmd.Parameters.AddWithValue("@lastUpdatedBy", Utilities.ParafaitEnv.LoginID);
                                cmd.CommandText = @"Update trx_header set Original_System_Reference=(select top 1 Original_System_Reference from trx_header where TrxId = @OldTrxId),
                                                                      transactionOTP = (select top 1 transactionOTP from trx_header where TrxId = @OldTrxId),
                                                                      CreatedBy = (select top 1 CreatedBy from trx_header where TrxId = @OldTrxId) ,
                                                                      LastUpdateTime = getdate(), LastUpdatedBy = @lastUpdatedBy
                                                   where trxId = @TrxId";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "Update trx_header set Original_System_Reference=NULL, TransactionOTP= null, LastUpdateTime = getdate(), LastUpdatedBy = @lastUpdatedBy where trxId = @OldTrxId";
                                cmd.ExecuteNonQuery();
                                ReopenedTrxId = Trx.Trx_id;
                                sqlTrxn.Commit();
                                this.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (sqlTrxn.Connection != null)
                                sqlTrxn.Rollback();
                            log.Error(ex);
                            POSUtils.ParafaitMessageBox(ex.Message);
                            btnGetDetails.PerformClick();
                        }
                        finally
                        {
                            cnn.Close();
                        }
                    }
                }
                else
                    POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 1416));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmOnlineTrxProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Common.Devices.PrimaryBarcodeScanner != null)
            {
                Common.Devices.PrimaryBarcodeScanner.UnRegister();
            }
            log.LogMethodExit();
        }

        private void ChkBxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool flag = false;
            if (ChkBxSelectAll.Checked)
            {
                flag = true;
            }
            if (dgvTrxLines.Rows.Count > 0)
            {
                try
                {
                    for (int i = 0; i < dgvTrxLines.Rows.Count; i++)
                    {
                        if (!dgvTrxLines.Rows[i].Cells["issueCardColumn"].ReadOnly)
                        {
                            dgvTrxLines.Rows[i].Cells["issueCardColumn"].Value = flag;
                        }
                    }
                }
                catch (Exception ex) { log.Error(ex); }
            }
            log.LogMethodExit();
        }

        private void btnChooseProduct_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<ItemPanel> itemPanelList = new List<ItemPanel>();
            ItemPanel itemPanel;
            if (dgvTrxLines.Rows.Count > 0)
            {
                for (int i = 0; i < dgvTrxLines.Rows.Count; i++)
                {
                    if (!dgvTrxLines.Rows[i].Cells["issueCardColumn"].ReadOnly)
                    {
                        dgvTrxLines.Rows[i].Cells["issueCardColumn"].Value = false;
                        if (itemPanelList.Exists(x => (bool)(x.ProdName.Equals(dgvTrxLines.Rows[i].Cells["Product"].Value.ToString()))))
                        {
                            itemPanelList.Where(x => (bool)(x.ProdName == dgvTrxLines.Rows[i].Cells["Product"].Value.ToString())).ToList<ItemPanel>()[0].ProdCount++;
                        }
                        else
                        {
                            itemPanel = new ItemPanel();
                            itemPanel.ProdName = dgvTrxLines.Rows[i].Cells["Product"].Value.ToString();
                            itemPanel.ProdCount++;
                            itemPanelList.Add(itemPanel);
                        }
                    }
                }
                if (itemPanelList.Count > 0)
                {
                    int count = 0;
                    using (frmChooseItem chooseItem = new frmChooseItem(Utilities, itemPanelList))
                    {
                        if (chooseItem.ShowDialog() == DialogResult.OK)
                        {
                            foreach (ItemPanel itp in itemPanelList)
                            {
                                count = itp.ProdCount;
                                for (int j = 0; j < dgvTrxLines.Rows.Count && count > 0; j++)
                                {
                                    if (dgvTrxLines.Rows[j].Cells["Product"].Value.ToString().Equals(itp.ProdName) && !dgvTrxLines.Rows[j].Cells["issueCardColumn"].ReadOnly)
                                    {
                                        dgvTrxLines.Rows[j].Cells["issueCardColumn"].Value = true;
                                        count--;
                                    }
                                }
                            }
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ShowNumberPadForm('-');
            log.LogMethodExit();
        }
        void ShowNumberPadForm(char firstKey)
        {
            log.LogMethodEntry(firstKey);
            double varAmount = NumberPadForm.ShowNumberPadForm(Utilities.MessageUtils.getMessage("Enter Amount"), firstKey, Utilities);
            if (varAmount >= 0)
            {
                if (currentTextBox != null && !currentTextBox.ReadOnly)
                {
                    currentTextBox.Text = Convert.ToInt32(varAmount).ToString();
                }
            }
            log.LogMethodExit();
        }

        private void txtTransactionId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentTextBox = txtTransactionId;
            log.LogMethodExit();
        }

        private void txtTransactionOTP_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            currentTextBox = txtTransactionOTP;
            log.LogMethodExit();
        }

        private void btnFindCustomer_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities);
                if (customerLookupUI.ShowDialog() == DialogResult.OK)
                {
                    DataTable dt = Utilities.executeDataTable(@"select TrxId [TrxId], Trx_No [TrxNo], Original_System_Reference [TrxRefId], TransactionOTP [OTP], TrxDate [Date], TrxNetAmount Amount
                                                                from trx_header 
                                                                where customerId = @custId
                                                                and (Original_System_Reference is not null
                                                                    or TransactionOTP is not null)",
                                                               new SqlParameter("@custId", customerLookupUI.SelectedCustomerDTO.Id));
                    if (dt.Rows.Count > 0)
                    {
                        DataGridView dgv = new DataGridView();
                        dgv.DataSource = dt;
                        dgv.ReadOnly = true;
                        dgv.AllowUserToAddRows = dgv.AllowUserToDeleteRows = false;
                        dgv.Width = 530;
                        dgv.Height = 200;
                        dgv.ScrollBars = ScrollBars.Both;
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                        dgv.MultiSelect = false;
                        dgv.Left = 10;
                        dgv.BorderStyle = BorderStyle.None;
                        dgv.RowHeadersVisible = false;

                        using (Form f = new Form())
                        {
                            f.Font = this.Font;
                            f.Width = dgv.Width + 20;
                            f.Height = dgv.Height + 100;
                            f.StartPosition = FormStartPosition.CenterScreen;
                            f.FormBorderStyle = FormBorderStyle.FixedToolWindow;

                            Button btnSelect = new Button();
                            btnSelect.FlatStyle = FlatStyle.Flat;
                            btnSelect.FlatAppearance.BorderSize = 0;
                            btnSelect.FlatAppearance.MouseDownBackColor = btnSelect.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            btnSelect.BackgroundImage = btnFindCustomer.BackgroundImage;
                            btnSelect.Size = btnPrint.Size;
                            btnSelect.BackgroundImageLayout = ImageLayout.Stretch;
                            btnSelect.Location = new Point(100, dgv.Bottom + 10);
                            btnSelect.Text = "Select";
                            btnSelect.ForeColor = btnPrint.ForeColor;
                            btnSelect.DialogResult = DialogResult.OK;

                            Button btnCancel = new Button();
                            btnCancel.FlatStyle = FlatStyle.Flat;
                            btnCancel.FlatAppearance.BorderSize = 0;
                            btnCancel.FlatAppearance.MouseDownBackColor = btnCancel.FlatAppearance.MouseOverBackColor = Color.Transparent;
                            btnCancel.BackgroundImage = btnFindCustomer.BackgroundImage;
                            btnCancel.Size = btnPrint.Size;
                            btnCancel.BackgroundImageLayout = ImageLayout.Stretch;
                            btnCancel.Location = new Point(btnSelect.Right + 40, btnSelect.Top);
                            btnCancel.Text = "Cancel";
                            btnCancel.ForeColor = btnPrint.ForeColor;
                            btnCancel.DialogResult = DialogResult.Cancel;
                            f.CancelButton = btnCancel;

                            f.Controls.Add(dgv);
                            f.Controls.Add(btnSelect);
                            f.Controls.Add(btnCancel);

                            dgv.BackgroundColor = f.BackColor;

                            f.Load += (object o, EventArgs ev) =>
                            {
                                dgv.Columns["Amount"].DefaultCellStyle = POSStatic.Utilities.gridViewAmountWithCurSymbolCellStyle();
                                dgv.Columns["Date"].DefaultCellStyle = POSStatic.Utilities.gridViewDateTimeCellStyle();
                            };

                            if (f.ShowDialog() == DialogResult.OK)
                            {
                                if (dgv.SelectedRows.Count > 0)
                                {
                                    txtTransactionOTP.Text = dgv.SelectedRows[0].Cells["OTP"].Value.ToString();
                                    txtTransactionId.Text = dgv.SelectedRows[0].Cells["TrxRefId"].Value.ToString();

                                    btnGetDetails.PerformClick();
                                }
                            }
                        }
                    }
                    else
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 2073));// "No online transactions found for this customer");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnMapWaiver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (Trx == null)
                {
                    log.LogMethodExit(Trx, " Trx == null");
                    return;
                }
                else if (Trx.Status == Transaction.TrxStatus.CANCELLED)
                {
                    throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction is already cancelled"));
                }
                else if (Trx.Status == Transaction.TrxStatus.SYSTEMABANDONED)
                {
                    throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction is already abandoned by the system"));
                }
                //else if (Trx.TransactionDate < Utilities.getServerTime().Date)
                //{
                //    throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, 2355)); //Cannot map waivers for past date transaction 
                //}
                else
                {

                    if (Trx.WaiverSignatureRequired())
                    {
                        List<WaiversDTO> trxWaiversDTOList = Trx.GetWaiversDTOList();
                        if (trxWaiversDTOList == null || trxWaiversDTOList.Any() == false)
                        {
                            log.LogVariableState("trxWaiversDTOList", trxWaiversDTOList);
                            throw new ValidationException(MessageContainer.GetMessage(Utilities.ExecutionContext, 2317,
                                 MessageContainer.GetMessage(Utilities.ExecutionContext, "Transaction") + " " + MessageContainer.GetMessage(Utilities.ExecutionContext, "Waivers")));
                        }
                        using (frmMapWaiversToTransaction frm = new frmMapWaiversToTransaction(POSStatic.Utilities, Trx))
                        {
                            if (frm.Width > Application.OpenForms["POS"].Width + 28)
                            {
                                frm.Width = Application.OpenForms["POS"].Width - 30;
                            }
                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                string msg = string.Empty;

                                this.Cursor = Cursors.WaitCursor;
                                int retcode = Trx.SaveOrder(ref msg);
                                if (retcode != 0)
                                {
                                    POSUtils.ParafaitMessageBox(msg);
                                    //reload transaction details from db
                                    TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                                    Trx = TransactionUtils.CreateTransactionFromDB(Trx.Trx_id, Utilities);
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainer.GetMessage(Utilities.ExecutionContext, 2357));//Transaction does not require waiver mapping
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {

                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
    }
}
