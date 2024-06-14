/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmRentalReturn 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.130.11    13-Oct-2022     Vignesh Bhat        Added check box Include Returned in rental return UI.
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
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Logger = Semnox.Core.Utilities.Logger;

namespace Parafait_POS
{
    public partial class frmRentalReturn : Form
    {
        Utilities Utilities = POSStatic.Utilities;

        private readonly TagNumberParser tagNumberParser;

        public Dictionary<int, double> rentalReturnList = new Dictionary<int, double>();

        public class RentalProduct
        {
            public int trxId;
            public int productId;
            public string productName;
            public double depositAmount;
            public int returnQuantity;
            public string trxNo;
        }
        public List<RentalProduct> lstRentalProduct = new List<RentalProduct>();
        public bool qtyValidate = false;
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmRentalReturn()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmRentalReturn()");//Added for logger function on 08-Mar-2016
            InitializeComponent();
            Utilities.setLanguage(this);
            tagNumberParser = new TagNumberParser(Utilities.ExecutionContext);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            log.Debug("Ends-frmRentalReturn()");//Added for logger function on 08-Mar-2016
        }

        public frmRentalReturn(string cardNum) : this()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmRentalReturn(" + cardNum + ")");//Added for logger function on 08-Mar-2016
            Populategrid(cardNum, -1, string.Empty);
            log.Debug("Ends-frmRentalReturn(" + cardNum + ")");//Added for logger function on 08-Mar-2016
        }

        private void frmRentalReturn_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmRentalReturn_Load()");//Added for logger function on 08-Mar-2016
            Utilities.setupDataGridProperties(ref dgvRentalReturn);
            DataGridViewCellStyle Style = new DataGridViewCellStyle();
            Style.Font = new System.Drawing.Font(dgvRentalReturn.DefaultCellStyle.Font.Name, 10.0F, FontStyle.Bold);
            dgvRentalReturn.DefaultCellStyle = Style;
            txtCardNumber.ReadOnly = true;

            dgvRentalReturn.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRentalReturn.RowTemplate.Height = 30;
            log.Debug("Ends-frmRentalReturn_Load()");//Added for logger function on 08-Mar-2016
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e is DeviceScannedEventArgs)
                {
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
                            POSUtils.ParafaitMessageBox(ex.Message);
                            return;
                        }
                        try
                        {
                            scannedTagNumber = tagNumberParser.ValidateDecryptedTag(decryptedTagNumber, Utilities.ParafaitEnv.SiteId);
                        }
                        catch (ValidationException ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            POSUtils.ParafaitMessageBox(ex.Message);
                            return;
                        }
                        catch (Exception ex)
                        {
                            log.LogVariableState("Decrypted Tag Number validation: ", ex);
                            POSUtils.ParafaitMessageBox(ex.Message);
                            return;
                        }
                    }
                    if (tagNumberParser.TryParse(scannedTagNumber, out tagNumber) == false)
                    {
                        string message = tagNumberParser.Validate(scannedTagNumber);
                        POSUtils.ParafaitMessageBox(message);
                        log.LogMethodExit(null, "Invalid Tag Number.");
                        return;
                    }

                    string CardNumber = tagNumber.Value;
                    cardSwiped(CardNumber);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in CardScanCompleteEventHandle() - " + ex.Message);
            }
            log.LogMethodExit();
        }

        public void cardSwiped(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            txtCardNumber.Text = cardNumber;
            SearchRentals();
            log.LogMethodExit();
        }

        private void Populategrid(string cardNumber, int transactionId, string transactionNumber, bool includeReturned = false, int customerId = -1)
        {
            log.Debug("Starts-Populategrid(" + cardNumber + "," + transactionId + ")");//Added for logger function on 08-Mar-2016
            StringBuilder rentalReturnQuery = new StringBuilder();
            DataTable dt = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                rentalReturnQuery.Append(@"SELECT R.TrxId [Trx Id], h.trx_no [Trx No], R.CardNumber [Card Number], R.ProductId, P.product_name [Product Name],
                                                  min(R.DepositAmount) [Deposit Amount], count(r.id) [Issued Count], 
                                                  count(case when refunded = 1 then r.Id
                                                          else NULL
                                                 end) [Already Returned]
                                             FROM RentalAllocation R INNER JOIN products P ON P.product_id = R.ProductId INNER JOIN trx_header h on r.trxid = h.trxid");

                if (customerId != -1)
                {
                    rentalReturnQuery.Append(" and (h.customerId = @customerId or  @customerId = (select top 1 customer_id from cards where card_id = h.PrimaryCardId and customer_id is not null ))");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@customerId", customerId));
                }
                else if (!string.IsNullOrEmpty(cardNumber) && transactionId != -1 && !string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.CardNumber = @cardNumber and R.TrxId = @trxId and R.TrxId = (select TOP 1 TrxId from trx_header where trxNetAmount > 0 and trx_no = @transactionNumber AND (POSMachineId = @POSMachineId or @POSMachineId = -1) and CONVERT(DATE,trxDate) = CONVERT(DATE,getdate()) ORDER BY trxDate DESC) ");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@cardNumber", cardNumber));
                    parameters.Add(new SqlParameter("@trxId", transactionId));
                    parameters.Add(new SqlParameter("@transactionNumber", transactionNumber));
                    parameters.Add(new SqlParameter("@POSMachineId", Utilities.ParafaitEnv.POSMachineId));
                }
                else if (string.IsNullOrEmpty(cardNumber) && transactionId != -1 && string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.TrxId = @trxId ");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@trxId", transactionId));
                }
                else if (!string.IsNullOrEmpty(cardNumber) && transactionId == -1 && string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.CardNumber = @cardNumber");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@cardNumber", cardNumber));
                }
                else if (string.IsNullOrEmpty(cardNumber) && transactionId == -1 && !string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.TrxId = (select TOP 1 TrxId from trx_header where trxNetAmount > 0 and trx_no = @transactionNumber AND (POSMachineId = @POSMachineId or @POSMachineId = -1) and CONVERT(DATE,trxDate) = CONVERT(DATE,getdate()) ORDER BY trxDate DESC)");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@transactionNumber", transactionNumber));
                    parameters.Add(new SqlParameter("@POSMachineId", Utilities.ParafaitEnv.POSMachineId));
                }

                else if (!string.IsNullOrEmpty(cardNumber) && transactionId == -1 && !string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.CardNumber = @cardNumber and R.TrxId = (select TOP 1 TrxId from trx_header where trxNetAmount > 0 and trx_no = @transactionNumber AND (POSMachineId = @POSMachineId or @POSMachineId = -1) and CONVERT(DATE,trxDate) = CONVERT(DATE,getdate()) ORDER BY trxDate DESC)");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@cardNumber", cardNumber));
                    parameters.Add(new SqlParameter("@transactionNumber", transactionNumber));
                    parameters.Add(new SqlParameter("@POSMachineId", Utilities.ParafaitEnv.POSMachineId));
                }

                else if (string.IsNullOrEmpty(cardNumber) && transactionId != -1 && !string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.TrxId = @trxId and R.TrxId = (select TOP 1 TrxId from trx_header where trxNetAmount > 0 and trx_no = @transactionNumber AND (POSMachineId = @POSMachineId or @POSMachineId = -1) and CONVERT(DATE,trxDate) = CONVERT(DATE,getdate()) ORDER BY trxDate DESC)");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@trxId", transactionId));
                    parameters.Add(new SqlParameter("@transactionNumber", transactionNumber));
                    parameters.Add(new SqlParameter("@POSMachineId", Utilities.ParafaitEnv.POSMachineId));
                }

                else if (!string.IsNullOrEmpty(cardNumber) && transactionId != -1 && string.IsNullOrEmpty(transactionNumber))
                {
                    rentalReturnQuery.Append(" and R.CardNumber = @cardNumber and R.TrxId = @trxId");
                    rentalReturnQuery.Append(" GROUP BY R.TrxId, h.trx_no, CardNumber, product_name, ProductId");
                    parameters.Add(new SqlParameter("@trxId", transactionId));
                    parameters.Add(new SqlParameter("@cardNumber", cardNumber));
                }

                dt = Utilities.executeDataTable(rentalReturnQuery.ToString(), parameters.ToArray());

                if (dt != null)
                {
                    if (dt.Rows.Count == 0)
                    {
                        dgvRentalReturn.Columns["RefundCount"].Visible = false;
                        if (!String.IsNullOrEmpty(cardNumber) || transactionId != -1 || !string.IsNullOrEmpty(transactionNumber) || customerId != -1)
                        {
                            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("No records found for search criteria"), "Rental Return");
                        }
                        dgvRentalReturn.DataSource = null;
                        log.Info("Ends-Populategrid(" + cardNumber + "," + transactionId + ") as No records found for search criteria");//Added for logger function on 08-Mar-2016
                        return;
                    }
                    dt = CheckForIncludeReturned(dt, includeReturned);
                    dgvRentalReturn.DataSource = dt;

                    for (int i = 0; i < dgvRentalReturn.Columns.Count; i++)
                    {
                        dgvRentalReturn.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    dgvRentalReturn.Columns["Trx Id"].DefaultCellStyle.Alignment =
                    dgvRentalReturn.Columns["Card Number"].DefaultCellStyle.Alignment =
                    dgvRentalReturn.Columns["Product Name"].DefaultCellStyle.Alignment =
                    dgvRentalReturn.Columns["Deposit Amount"].DefaultCellStyle.Alignment =
                    dgvRentalReturn.Columns["Issued Count"].DefaultCellStyle.Alignment =
                    dgvRentalReturn.Columns["Already Returned"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    for (int i = 0; i < dgvRentalReturn.Columns.Count; i++)
                    {
                        dgvRentalReturn.Columns[i].ReadOnly = true;
                    }
                    dgvRentalReturn.Columns["RefundCount"].ReadOnly = false;


                    //dgvRentalReturn.RowTemplate.Height = 30;
                    dgvRentalReturn.RowsDefaultCellStyle.Font = new Font("Arial", 9.5F);

                    dgvRentalReturn.EditMode = DataGridViewEditMode.EditOnEnter; //Enable edit on first click

                    dgvRentalReturn.Columns["ProductId"].Visible = false;
                    dgvRentalReturn.Columns["Trx No"].Visible = false;
                    dgvRentalReturn.Columns["RefundCount"].Visible = true;
                    dgvRentalReturn.Columns["RefundCount"].DisplayIndex = dgvRentalReturn.Columns.Count - 1;

                    dgvRentalReturn.Columns["Product Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgvRentalReturn.Columns["Product Name"].Width = 250;

                    dgvRentalReturn.Columns["Trx Id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvRentalReturn.Columns["Trx Id"].Width = 75;

                    dgvRentalReturn.Columns["Card Number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgvRentalReturn.Columns["Card Number"].Width = 100;
                    //Begin: Modification for pointing cursor to Enter Return Count on 05-Apr-2016
                    DataGridViewRow selectRow = this.dgvRentalReturn.CurrentRow;
                    selectRow.Selected = true;
                    selectRow.Cells[0].Selected = true;
                    this.dgvRentalReturn.CurrentCell = selectRow.Cells[0];
                    this.dgvRentalReturn.BeginEdit(true);
                    //End: Modification for pointing cursor to Enter Return Count on 05-Apr-2016
                }
                else
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("No records found for search criteria"), "Rental Return");
                    log.Info("Populategrid(" + cardNumber + "," + transactionId + ") - In Rental Return No records found for search criteria");//Added for logger function on 08-Mar-2016
                }
            }
            catch(Exception ex)
            {
                log.Fatal("Ends-Populategrid(" + cardNumber + "," + transactionId + ") due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
            }
            log.Debug("Ends-Populategrid(" + cardNumber + "," + transactionId + ")");//Added for logger function on 08-Mar-2016
        }

        private DataTable CheckForIncludeReturned(DataTable dt, bool includeReturned)
        {
            log.LogMethodEntry();
            DataTable selectedTable = null;
            if (includeReturned == false)
            {
                if (dt.Rows.Count > 0)
                {
                    selectedTable = dt.AsEnumerable()
                            .Where(r => r.Field<int>("Issued Count") != r.Field<int>("Already Returned"))
                            .CopyToDataTable();
                }
            }
            else
            {
                selectedTable = dt;
            }
            log.LogMethodExit(selectedTable);
            return selectedTable;
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SearchRentals();
            log.LogMethodExit();
        }
        private void SearchRentals()
        {
            log.LogMethodEntry();
            string cardNo;
            int transactionId;
            string transactionNumber;
            bool includeReturned = false;
            if (cbxIncludeReturned.Checked)
            {
                includeReturned = true;
            }
            if ((string.IsNullOrEmpty(txtTransactionId.Text.Trim())) && (string.IsNullOrEmpty(txtCardNumber.Text.Trim())) && (string.IsNullOrEmpty(txtTransactionNumber.Text.Trim())))
            {
                POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Enter any one search parameter"), "Rental Return");
                log.Info("Ends-btnGo_Click() as Transaction id and card number was not entered");//Added for logger function on 08-Mar-2016
                return;
            }

            if (!(string.IsNullOrEmpty(txtTransactionNumber.Text.Trim())))
            {
                transactionNumber = txtTransactionNumber.Text.ToString();
            }
            else
            {
                transactionNumber = string.Empty;
            }

            if (!(string.IsNullOrEmpty(txtCardNumber.Text.Trim())))
            {
                cardNo = txtCardNumber.Text.ToString();
            }
            else
            {
                cardNo = string.Empty;
            }

            if (!(string.IsNullOrEmpty(txtTransactionId.Text.Trim())))
            {
                transactionId = Convert.ToInt32(txtTransactionId.Text.ToString());
            }
            else
            {
                transactionId = -1;
            }

            if (dgvRentalReturn.DataSource != null)
                dgvRentalReturn.DataSource = null;
            else
                dgvRentalReturn.Rows.Clear();
            Populategrid(cardNo, transactionId, transactionNumber, includeReturned);
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click()");//Added for logger function on 08-Mar-2016
            this.Close();
            log.Debug("Ends-btnClose_Click()");//Added for logger function on 08-Mar-2016
        }

        private void txtTransactionId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnReturn_Click()");//Added for logger function on 08-Mar-2016
            foreach (DataGridViewRow row in dgvRentalReturn.Rows)
            {
                if (Convert.ToInt32(row.Cells["RefundCount"].Value) > 0)
                {
                    RentalProduct rp = new RentalProduct();
                    rp.trxId = Convert.ToInt32(row.Cells["Trx Id"].Value);
                    rp.productId = Convert.ToInt32(row.Cells["ProductId"].Value);
                    rp.productName = row.Cells["Product Name"].Value.ToString();
                    rp.depositAmount = Convert.ToDouble(row.Cells["Deposit Amount"].Value);
                    rp.returnQuantity = Convert.ToInt32(row.Cells["RefundCount"].Value);
                    rp.trxNo = row.Cells["Trx No"].Value.ToString();
                    lstRentalProduct.Add(rp);
                }
            }

            if (this.Validate() && this.ValidateChildren())
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            log.Debug("Ends-btnReturn_Click()");//Added for logger function on 08-Mar-2016
        }

        private void frmRentalReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Debug("Starts-frmRentalReturn_FormClosing()");//Added for logger function on 08-Mar-2016
            Common.Devices.UnregisterCardReaders(); //Unregister the card reader so that main application takes over 
            log.Debug("Ends-frmRentalReturn_FormClosing()");//Added for logger function on 08-Mar-2016
        }

        private void dgvRentalReturn_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvRentalReturn_CellValidated()");//Added for logger function on 08-Mar-2016
            if (e.RowIndex < 0)
            {
                log.Info("Ends-dgvRentalReturn_CellValidated() as e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                return;
            }
            if (dgvRentalReturn.Columns[e.ColumnIndex].Name == "RefundCount")
            {
                int qty;
                try
                {
                    qty = Convert.ToInt32(dgvRentalReturn["RefundCount", e.RowIndex].Value);
                }
                catch
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(34, "Refund Count")); //New Message Box UI - 05-Mar-2016
                    dgvRentalReturn[e.ColumnIndex, e.RowIndex].Value = 0;
                    log.Fatal("Ends-dgvRentalReturn_CellValidated() due to exception in Refundcount");//Added for logger function on 08-Mar-2016
                    return;
                }
                if (qty > Convert.ToInt32(dgvRentalReturn["Issued Count", e.RowIndex].Value) - Convert.ToInt32(dgvRentalReturn["Already Returned", e.RowIndex].Value))
                {
                    POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage("Return quantity should be less than remaining Rental count"), "Rental Return"); //New Message Box UI - 05-Mar-2016
                    dgvRentalReturn[e.ColumnIndex, e.RowIndex].Value = Convert.ToInt32(dgvRentalReturn["Issued Count", e.RowIndex].Value) - Convert.ToInt32(dgvRentalReturn["Already Returned", e.RowIndex].Value);
                    log.Info("Ends-dgvRentalReturn_CellValidated() as Return quantity should be less than remaining Rental count");//Added for logger function on 08-Mar-2016
                    return;
                }
                qtyValidate = true;
            }
            else
                return;
            log.Debug("Ends-dgvRentalReturn_CellValidated()");//Added for logger function on 08-Mar-2016
        }

        private void dgvRentalReturn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void dgvRentalReturn_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            POSUtils.ParafaitMessageBox(Utilities.MessageUtils.getMessage(3, e.RowIndex + 1, e.Exception.Message));  //New Message Box UI - 05-Mar-2016
            e.Cancel = true;
        }

        private void btnRentalTrx_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRentalTrx_Click()");//Added for logger function on 08-Mar-2016
            double varAmount = (int)NumberPadForm.ShowNumberPadForm(txtTransactionId.Text, '-', Utilities);
            if (varAmount >= 0)
            {
                txtTransactionId.Text = varAmount.ToString();
                ValidateChildren();
            }
            log.Debug("Ends-btnRentalTrx_Click()");//Added for logger function on 08-Mar-2016
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClear_Click()");//Added for logger function on 08-Mar-2016
            txtTransactionId.Text = "";
            txtCardNumber.Text = "";
            txtTransactionNumber.Text = "";
            cbxIncludeReturned.CheckState = CheckState.Unchecked;
            this.ActiveControl = txtTransactionId;
            dgvRentalReturn.DataSource = null;
            dgvRentalReturn.Columns["RefundCount"].Visible = false;
            log.Debug("Ends-btnClear_Click()");//Added for logger function on 08-Mar-2016
        }

        private void txtTransactionId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGo_Click(sender, e);
            }
        }

        private void txtTransactionNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnGo_Click(sender, e);
            }
        }

        private void dgvRentalReturn_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is DataGridViewTextBoxEditingControl)
            {
                DataGridViewTextBoxEditingControl tb = e.Control as DataGridViewTextBoxEditingControl;
                tb.KeyDown -= dgvRentalReturn_KeyDown;
                tb.PreviewKeyDown -= dgvRentalReturn_PreviewKeyDown;
                tb.KeyDown += dgvRentalReturn_KeyDown;
                tb.PreviewKeyDown += dgvRentalReturn_PreviewKeyDown;
            }
        }

        //If enter key is pressed in last row of data grid view, Return button click
        private void dgvRentalReturn_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (dgvRentalReturn.CurrentRow.Index < 0)
                    return;
                if (dgvRentalReturn.CurrentRow.Index == dgvRentalReturn.Rows.Count - 1)
                {
                    ValidateChildren();
                    if (qtyValidate)
                        btnReturn_Click(sender, e);
                    else
                        return;
                }
            }
        }

        private void dgvRentalReturn_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerLookupUI customerLookupUI = new CustomerLookupUI(Utilities);
                if (customerLookupUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (customerLookupUI.SelectedCustomerDTO != null)
                    {
                        Populategrid("", -1, "", false, customerLookupUI.SelectedCustomerDTO.Id);
                    }
                    else
                    {
                        btnClear.PerformClick();
                    }
                }
                else
                {
                    btnClear.PerformClick();
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
        }
    }
}
