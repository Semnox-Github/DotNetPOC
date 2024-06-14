/******************************************************************************************************************************
*Project Name -                                                                          
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
********************************************************************************************************************************
 *1.00       10-Jun-2016            Soumya                     Added screen for return/exchange
 *                                                             process
 *********************************************************************************************
 *1.00       18-Oct-2016            Soumya                     Updated method dgvExchangeProducts_CellClick
 *                                                             to enable price update for exchange
 *                                                             products.
 *                                                             Added method updateExchangeProductPrice
 *                                                             to handle exchange product price updates
 *********************************************************************************************
 *1.00       21-Oct-2016            Soumya                     Updated methods 
 *                                                             updateExchangeReturnProductQuantity &
                                                               updateReturnProductQuantity to fix issues 
 *                                                             with amount values shown in grid on quantity update
 *2.50.0     11-Dec-2018            Mathew Ninan               Deprecated StaticDataExchange and used TransactionPaymentsDTO                                                             
 *2.60.2     29-May-2019            Mathew Ninan               Fixed some of the payment specific calls and printing approach
 *2.80       18-Dec-2019            Jinto Thomas               Added parameter execution context for userbl declaration with userid
 *2.140      14-Oct-2021            Guru S A                   Modified for Payment Settlements 
 *****************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Parafait.Device;
using Semnox.Parafait.Transaction;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;
using ParafaitPOS;
using Semnox.Parafait.TableAttributeSetupUI;

namespace Parafait_POS.SalesReturn
{
    public partial class frmSalesReturn : Form
    {
        Utilities Utilities = POSStatic.Utilities;
        MessageUtils MessageUtils = POSStatic.MessageUtils;
        TaskProcs TaskProcs = POSStatic.TaskProcs;
        ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        string FORFEIT_BALANCE_RETURN_AMOUNT;
        int RETURN_WITHIN_DAYS;
        public static Transaction NewTrx, SalesReturnTrx;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        TextBox CurrentTextBox;
        TextBox CurrentAlphanumericTextBox;

        double businessDayStartTime;

        public string ReturnMessage = "";
        public int SelectedTransactionId;
        DateTime TrxDate;
        string Status;

        DateTime fromdate;
        DateTime todate;
        bool managerApprovalRequired;

        public frmSalesReturn()
        {
            log.Debug("Starts-frmSalesReturn()");
            try
            {
                InitializeComponent();

                Utilities = POSStatic.Utilities;
                ParafaitEnv = Utilities.ParafaitEnv;
                MessageUtils = Utilities.MessageUtils;
                SelectedTransactionId = -1;
                Utilities.setLanguage(this);
                managerApprovalRequired = false;

                if (Common.Devices.PrimaryBarcodeScanner != null)
                {
                    Common.Devices.PrimaryBarcodeScanner.Register(new EventHandler(BarCodeScanCompleteEventHandle));
                }

                try
                {
                    businessDayStartTime = Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"));
                }
                catch
                {
                    businessDayStartTime = 6;
                }
                try
                {
                    RETURN_WITHIN_DAYS = Convert.ToInt32(Utilities.getParafaitDefaults("RETURN_WITHIN_DAYS"));
                }
                catch
                {
                    RETURN_WITHIN_DAYS = 30;
                }
                try
                {
                    FORFEIT_BALANCE_RETURN_AMOUNT = Utilities.getParafaitDefaults("FORFEIT_BALANCE_RETURN_AMOUNT");
                }
                catch
                {
                    FORFEIT_BALANCE_RETURN_AMOUNT = "N";
                }
            }
            catch
            {

            }
            log.Debug("Ends-frmSalesReturn()");
        }

        private void frmSalesReturn_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmSalesReturn_Load()");
            this.BackColor = Color.Linen;
            tpTrxLookup.BackColor = this.BackColor;
            tpProductLookup.BackColor = this.BackColor;
            try
            {
                cleanUpOnNullTrx();
            }
            catch
            { }
            log.Debug("Ends-frmSalesReturn_Load()");
        }

        private void BarCodeScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.Debug("Starts-BarCodeScanCompleteEventHandle()");
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                string scannedBarcode = Utilities.ProcessScannedBarCode(checkScannedEvent.Message, ParafaitEnv.LEFT_TRIM_BARCODE, ParafaitEnv.RIGHT_TRIM_BARCODE);
                try
                {
                    //Thread error fix by threading 15-May-2016
                    this.Invoke((MethodInvoker)delegate
                    {
                        HandleBarcodeRead(scannedBarcode);
                    });
                }
                catch (Exception ex)
                {
                    displayMessageLine(ex.Message);
                }
            }
            log.Debug("Ends-BarCodeScanCompleteEventHandle()");
        }

        public void HandleBarcodeRead(string scannedBarcode)
        {
            log.Debug("Starts-HandleBarcodeRead()");
            if (scannedBarcode != "")
            {
                if (tcReturnExchange.SelectedTab.Equals(tpTrxLookup))
                {
                    txtTrxID.Text = scannedBarcode;
                    btnSearchTrx.PerformClick();
                }
                else if (tcReturnExchange.SelectedTab.Equals(tpProductLookup))
                {
                    txtProduct.Text = scannedBarcode;
                    btnProductSearch.PerformClick();
                }
                else if (tcReturnExchange.SelectedTab.Equals(tpExchange))
                {
                    txtExchangeProduct.Text = scannedBarcode;
                    btnExchangeProductSearch.PerformClick();
                }
            }
            log.Debug("Ends-HandleBarcodeRead()");
        }

        #region trxLookup
        private void dtFromTrxDate_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtFromTrxDate_ValueChanged()");
            displayMessageLine("");
            txtFromTrxDate.Text = dtFromTrxDate.Value.ToString("dd-MMM-yyyy");
            log.Debug("Ends-dtFromTrxDate_ValueChanged()");
        }

        private void dtToTrxDate_ValueChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-dtToTrxDate_ValueChanged()");
            displayMessageLine("");
            txtToTrxDate.Text = dtToTrxDate.Value.ToString("dd-MMM-yyyy");
            log.Debug("Ends-dtToTrxDate_ValueChanged()");
        }

        private void btnSearchTrx_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearchTrx_Click()");
            try
            {
                displayMessageLine("");
                dgvTransaction.Rows.Clear();
                if (txtFromTrxDate.Text == "" && txtToTrxDate.Text == "" && txtTrxID.Text == "")
                {
                    displayMessageLine(MessageUtils.getMessage(1049));
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                fromdate = dtFromTrxDate.Value.Date.AddHours(businessDayStartTime);
                todate = dtToTrxDate.Value.Date.AddHours(businessDayStartTime);
                SqlCommand cmd = Utilities.getCommand();
                string dateFilter = "";
                if (txtFromTrxDate.Text != "")
                {
                    dateFilter = " and trxdate >= @fromDate and trxdate < @toDate";
                    cmd.Parameters.AddWithValue("@fromDate", fromdate);
                    cmd.Parameters.AddWithValue("@toDate", todate);
                }
                string commandtext = @"select [TrxId] as ID
	                                    ,[Trx_no] 
	                                    ,[TrxDate] as Date
	                                    ,[TrxAmount] as trxAmount
	                                    ,[TaxAmount] as trxTax
	                                    ,[TrxNetAmount] as Net_amount 
	                                    ,(select sum(DiscountAmount) discount_amount
		                                    from TrxDiscounts
		                                    where TrxId = h.TrxId) as trxdiscount_amount
	                                    ,Status trxStatus
	                                    ,PaymentReference trxRemarks
                                    from trx_header h
                                    where (@TrxId != -1 and h.TrxId = @TrxId " + dateFilter + ") " +
                                        " or (@TrxId = -1 " + dateFilter + ") " +
                                    "Order by date desc ";

                cmd.CommandText = commandtext;
                if (txtTrxID.Text == "")
                    cmd.Parameters.AddWithValue("@TrxId", -1);
                else
                    cmd.Parameters.AddWithValue("@TrxId", txtTrxID.Text);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvTransaction.Rows.Clear();
                if (dt.Rows.Count > 0)
                {
                    dgvTransaction.AutoGenerateColumns = false;
                    foreach (DataRow dr in dt.Rows)
                    {
                        dgvTransaction.Rows.Add(dr.ItemArray);
                    }
                    refreshdgvTransaction();

                    if (dt.Rows.Count == 1)
                    {
                        dgvTransaction.Rows[0].Selected = true;
                    }

                    this.ActiveControl = txtTrxID;
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    displayMessageLine(MessageUtils.getMessage(1022));
                }
                cmd.Dispose();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnSearchTrx_Click()");
        }

        private void dgvTransaction_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvTransaction_CellContentClick()");
            try
            {
                displayMessageLine("");
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvTransaction.CurrentRow == null || dgvTransaction.CurrentRow.Cells["ID"].Value == null)
                        return;
                }

                if (dgvTransaction.CurrentRow != null)
                {
                    int TrxId = Convert.ToInt32(dgvTransaction.CurrentRow.Cells["ID"].Value);
                    if (dgvTransaction.CurrentRow.Cells["trxStatus"].Value != null && dgvTransaction.CurrentRow.Cells["trxStatus"].Value != DBNull.Value)
                    {
                        string status = dgvTransaction.CurrentRow.Cells["trxStatus"].Value.ToString();
                        if (status == "CANCELLED")
                        {
                            displayMessageLine(MessageUtils.getMessage(1023, status.ToLower()));
                            return;
                        }
                        if (dgvTransaction.CurrentRow.Cells["trxRemarks"].Value != null && dgvTransaction.CurrentRow.Cells["trxRemarks"].Value != DBNull.Value)
                        {
                            string remarks = dgvTransaction.CurrentRow.Cells["trxRemarks"].Value.ToString().ToLower();
                            if (remarks.Contains("reversal") || remarks.Equals("return"))
                            {
                                displayMessageLine(MessageUtils.getMessage(1023, status.ToLower()));
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvTransaction_CellContentClick()");
        }

        //Start update 2-Aug-2016
        //Added method to check if the product is of type manual
        public bool isManualProduct(int ProductID)
        {
            log.Debug("Ends-getTransactionStatus()");
            try
            {
                displayMessageLine("");
                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"select isnull(product_type, '')
                                    from products p, product_type pt
                                    where p.product_type_id = pt.product_type_id
                                        and product_id = @productID";
                cmd.Parameters.AddWithValue("@productID", ProductID);
                object o = cmd.ExecuteScalar();
                if (o != null && o != DBNull.Value)
                {
                    if (o.ToString() == "MANUAL")
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                return false;
            }
        }
        //End update 2-Aug-2016

        public string getTransactionStatus(int TrxId, int TrxLineId, ref string Message)
        {
            log.Debug("Ends-getTransactionStatus()");
            try
            {
                displayMessageLine("");
                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"select top 1 1 
                                from trx_header h
                                where (@lineId != -1 and PaymentReference like '%Trx Id/Line Id: ' + @trxIdStr + '/' + @lineIdStr + ':%')
                                   or (@lineId = -1 and PaymentReference like '%Trx Id% ' + @trxIdStr + '%')
                                   or (@lineId != -1 and PaymentReference like '%Trx Id:% ' + @trxIdStr + ':%')
                                   or (TrxId = @TrxId and PaymentReference like 'Reversal of Trx Id%')";
                cmd.Parameters.AddWithValue("@trxIdStr", TrxId.ToString());
                cmd.Parameters.AddWithValue("@lineIdStr", TrxLineId.ToString());
                cmd.Parameters.AddWithValue("@trxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                if (cmd.ExecuteScalar() != null)
                {
                    cmd.Dispose();
                    displayMessageLine(MessageUtils.getMessage(335));
                    log.Debug("Ends-getTransactionStatus()");
                    return "Reversed";
                }

                cmd.CommandText = @"select top 1 1 
                                    from trx_header h, trx_lines l
                                    where h.originaltrxid = @TrxId
                                        and h.trxid = l.trxid
                                        and originallineid = @lineId
                                        --and h.paymentreference is null
                                        ";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                if (cmd.ExecuteScalar() != null)
                {
                    cmd.Dispose();
                    displayMessageLine(Utilities.MessageUtils.getMessage(335));
                    log.Debug("Ends-getTransactionStatus()");
                    return "Returned";
                }

                cmd.CommandText = @"select top 1 1 
                                    from trx_header h, trx_lines l
                                    where h.trxid = @TrxId
                                        and h.trxid = l.trxid
										and (h.paymentreference in ('Exchange', 'Return') or h.paymentreference like '%reversal%')
										and quantity < 1
										and lineid = @lineId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TrxId", TrxId);
                cmd.Parameters.AddWithValue("@lineId", TrxLineId);
                if (cmd.ExecuteScalar() != null)
                {
                    cmd.Dispose();
                    displayMessageLine(MessageUtils.getMessage(1050));
                    log.Debug("Ends-getTransactionStatus()");
                    return "Return/Exchange Transaction";
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.Debug("Ends-getTransactionStatus()");
                return "";
            }
            log.Debug("Ends-getTransactionStatus()");
            return "";
        }

        private void buttonTLNext_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-buttonTLNext_Click()");
            try
            {
                displayMessageLine("");
                managerApprovalRequired = false;

                if (dgvTransaction.SelectedRows.Count != 0)
                {
                    if (dgvTransaction.SelectedRows[0].Cells["trxStatus"].Value != null && dgvTransaction.SelectedRows[0].Cells["trxStatus"].Value != DBNull.Value)
                    {
                        Status = dgvTransaction.SelectedRows[0].Cells["trxStatus"].Value.ToString();
                        if (Status == "CANCELLED")
                        {
                            displayMessageLine(MessageUtils.getMessage(1023, Status.ToLower()));
                            return;
                        }
                    }
                    if (dgvTransaction.SelectedRows[0].Cells["trxRemarks"].Value != null && dgvTransaction.SelectedRows[0].Cells["trxRemarks"].Value != DBNull.Value)
                    {
                        string remarks = dgvTransaction.SelectedRows[0].Cells["trxRemarks"].Value.ToString().ToLower();
                        if (remarks == "return" || remarks.Contains("reversal"))
                        {
                            displayMessageLine(MessageUtils.getMessage(1023, Status.ToLower()));
                            return;
                        }
                    }
                    SelectedTransactionId = Convert.ToInt32(dgvTransaction.SelectedRows[0].Cells["ID"].Value);
                    TrxDate = Convert.ToDateTime(dgvTransaction.SelectedRows[0].Cells["Date"].Value);
                }
                else
                    SelectedTransactionId = -1;
                dgvProductDetails.Rows.Clear();
                if (SelectedTransactionId == -1)
                {
                    gbPLSearchCriteria.Enabled = true;
                    managerApprovalRequired = true;
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                    NewTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
                    NewTrx = TransactionUtils.CreateTransactionFromDB(SelectedTransactionId, Utilities);

                    dgvProductDetails.Rows.Clear();
                    int i = 0;
                    foreach (Transaction.TransactionLine tl in NewTrx.TrxLines)
                    {
                        DataTable dtPrice = getPrice(tl.ProductID);
                        double discountAmount = 0;
                        double Pre_TaxAmount = 0;
                        double Tax_Amount = 0;
                        string remarks = "";
                        string message = "";
                        double preTaxLineAmount = tl.LineAmount = tl.Price * (double)tl.quantity;
                        Pre_TaxAmount += preTaxLineAmount;
                        tl.tax_amount = preTaxLineAmount * tl.tax_percentage / 100.0;
                        Tax_Amount += tl.tax_amount;
                        tl.LineAmount += tl.tax_amount;
                        tl.Discount_Percentage = 0;
                        remarks = getTransactionStatus(SelectedTransactionId, tl.DBLineId, ref message);
                        if (tl.TransactionDiscountsDTOList != null && tl.TransactionDiscountsDTOList.Count > 0)
                        {
                            foreach (var transactionDiscountsDTO in tl.TransactionDiscountsDTOList)
                            {
                                discountAmount += transactionDiscountsDTO.DiscountAmount == null ? 0 : (double)transactionDiscountsDTO.DiscountAmount;
                            }
                        }
                        if (tl.TaxInclusivePrice == "Y")
                            dgvProductDetails.Rows.Add("N", tl.ProductID, tl.ProductName, tl.quantity, tl.Price + tl.tax_amount, tl.LineAmount - discountAmount, tl.tax_amount, "Y", discountAmount, Convert.ToDouble(dtPrice.Rows[0]["sale_price"]), i, tl.DBLineId, remarks);
                        else
                            dgvProductDetails.Rows.Add("N", tl.ProductID, tl.ProductName, tl.quantity, tl.Price, tl.LineAmount - discountAmount, tl.tax_amount, "N", discountAmount, Convert.ToDouble(dtPrice.Rows[0]["sale_price"]), i, tl.DBLineId, remarks);
                        i = i + 1;
                    }

                    refreshdgvProductDetails();
                    foreach (DataGridViewRow dr in dgvProductDetails.Rows)
                    {
                        dr.ReadOnly = false;

                        if (dr.Cells["Remarks"].Value != null)
                        {
                            if (dr.Cells["Remarks"].Value.ToString().ToLower().Contains("reverse") || dr.Cells["Remarks"].Value.ToString().ToLower().Contains("reversal") || dr.Cells["Remarks"].Value.ToString().ToLower().Contains("return"))
                            {
                                dr.DefaultCellStyle.BackColor = Color.Silver;
                                dr.Cells["chkSelect"].ReadOnly = true;
                            }
                        }

                        if (dr.Cells["quantity"].Value != null)
                        {
                            if (Convert.ToInt32(dr.Cells["quantity"].Value) <= 0)
                            {
                                dr.DefaultCellStyle.BackColor = Color.Silver;
                                dr.Cells["chkSelect"].ReadOnly = true;
                            }
                        }

                        //Start Update 2-Aug-2016
                        //Added to see that if the product type is not manual, row should be disabled
                        if (dr.Cells["product_id"].Value != null)
                        {
                            if (!isManualProduct(Convert.ToInt32(dr.Cells["product_id"].Value)))
                            {
                                dr.DefaultCellStyle.BackColor = Color.Silver;
                                dr.Cells["chkSelect"].ReadOnly = true;
                            }
                        }
                        //End Update 2-Aug-2016
                    }
                    gbPLSearchCriteria.Enabled = false;
                }
                tcReturnExchange.SelectTab(tpProductLookup);
                displayMessageLine(MessageUtils.getMessage(1040));
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-buttonTLNext_Click()");
        }

        private DataTable getPrice(int productID)
        {
            log.Debug("Starts-getPrice()");
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = Utilities.getCommand();

                cmd.CommandText = @"select p.price current_price,
	                                    (select top 1 case p.taxinclusiveprice when 'Y' then (l.price*(1 + isnull((l.tax_percentage/100), 0))) else l.price end price
                                         from trx_lines l
	                                     where l.product_id = p.product_id
		                                    and quantity > 0
		                                    and price = (select min(price)
						                                    from trx_lines
						                                    where product_id = l.product_id
						                                    and quantity > 0)
	                                    ) sale_price
                                    from products p 
                                    where product_id = @product_id";
                cmd.Parameters.AddWithValue("@product_id", productID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-getPrice()");
            return dt;
        }

        private void btnTLClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnTLClose_Click()");
            SalesReturnTrx = null;
            NewTrx = null;
            this.DialogResult = DialogResult.Cancel;
            log.Debug("Ends-btnTLClose_Click()");
            this.Close();
        }

        #endregion trxLookup

        #region productlookup 

        frmSKUSearch frmSKUSearch;

        private void btnPLSKUSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnPLSKUSearch_Click()");
            displayMessageLine("");
            frmSKUSearch = new frmSKUSearch(txtProduct.Text);
            if (frmSKUSearch.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.Cursor = Cursors.WaitCursor;
                    btnProductSearch.PerformClick();
                    this.Cursor = Cursors.Default;
                }
                catch (SqlException ex)
                {
                    this.Cursor = Cursors.Default;
                    displayMessageLine(ex.Message);
                }
            }
            log.Debug("Ends-btnPLSKUSearch_Click()");
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnProductSearch_Click()");
            string skuSearchString = "";

            try
            {
                displayMessageLine("");
                this.Cursor = Cursors.WaitCursor;
                if (frmSKUSearch != null)
                {
                    if (!string.IsNullOrEmpty(frmSKUSearch.searchString))
                    {
                        if (frmSKUSearch.searchString != "-1")
                        {
                            if (frmSKUSearch.searchString.Split(',').Length > 100)
                            {
                                displayMessageLine(MessageUtils.getMessage(1058));
                                this.Cursor = Cursors.Default;
                                return;
                            }
                            skuSearchString = " and p.segmentcategoryid in (" + frmSKUSearch.searchString + ") ";
                        }
                        else
                            skuSearchString = " ";
                    }
                }

                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"select 'N' [Select],
                                        product_id, 
	                                    p.product_name Product,
                                        1 quantity,
	                                    --case taxinclusiveprice when 'Y' then (price/(1 + isnull((tax_percentage/100), 0))) else p.price end price,
                                        p.price,
                                        case taxinclusiveprice when 'Y' then (price/(1 + isnull((tax_percentage/100), 0))) + (price/(1 + (tax_percentage/100)) * (tax_percentage/100)) else price + (price * isnull(tax_percentage/100, 0) ) end amount,
	                                    case taxinclusiveprice when 'Y' then price/(1 + isnull((tax_percentage/100), 0)) * (tax_percentage/100) else price * isnull(tax_percentage/100, 0) end tax,
                                        p.taxinclusiveprice as tax_inclusive_price, 
	                                    0 discount_amount,
	                                    (select top 1 case p.taxinclusiveprice when 'Y' then (l.price*(1 + isnull((l.tax_percentage/100), 0))) else l.price end price
									     from trx_lines l
									     where l.product_id = p.product_id
										    and quantity > 0
										    and trxid = (select max(trxid)
														    from trx_lines
														    where product_id = l.product_id
														    and quantity > 0)) last_sale_price,
                                        0 Line,
                                        null Remarks
                                        
                                    from products p left outer join tax t on p.tax_id = t.tax_id, product rp left outer join (select * 
												                                    from ( 
														                                    select *, row_number() over(partition by productid order by productid) as num 
														                                    from productbarcode 
                                                                                            where barcode like @product and isactive = 'Y')v 
                                                                                    where num = 1) b on rp.productid = b.productid, product_type pt
                                    where p.product_id = rp.manualproductId 
                                        and (b.barcode like @product or p.product_name like @product or p.description like @product
			                                     or rp.code like @product or rp.description like @product)
                                        and p.product_type_id = pt.product_type_id
                                        and pt.product_type = 'MANUAL'
                                        and p.active_flag = 'Y' "
                                        + skuSearchString;
                cmd.Parameters.AddWithValue("@product", "%" + txtProduct.Text + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!productExists(dgvProductDetails, Convert.ToInt32(dr["product_id"]), "product_id"))
                            dgvProductDetails.Rows.Add(dr.ItemArray);
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    displayMessageLine(MessageUtils.getMessage(1024));
                }
                frmSKUSearch = null;
                refreshdgvProductDetails();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnProductSearch_Click()");
        }

        bool productExists(DataGridView dgv, int product_id, string productIDColumnName)
        {
            try
            {
                if (dgv.Rows.Count <= -1)
                    return false;
                foreach (DataGridViewRow dr in dgv.Rows)
                {
                    if (Convert.ToInt32(dr.Cells[productIDColumnName].Value) == product_id)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return false;
            }
        }

        private void btnClearProductSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClearProductSearch_Click()");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                displayMessageLine("");
                txtProduct.Text = "";
                frmSKUSearch = null;
                refreshdgvProductDetails();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnClearProductSearch_Click()");
        }

        private void dgvProductDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvProductDetails_CellContentClick()");
            try
            {
                displayMessageLine("");
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvProductDetails.CurrentRow == null || dgvProductDetails.CurrentRow.Cells["product_id"].Value == null)
                        return;
                }

                if (dgvProductDetails.CurrentRow.Cells["Remarks"].Value != null)
                {
                    if (dgvProductDetails.CurrentRow.Cells["Remarks"].Value.ToString().ToLower().Contains("reverse") || dgvProductDetails.CurrentRow.Cells["Remarks"].Value.ToString().ToLower().Contains("reversal") || dgvProductDetails.CurrentRow.Cells["Remarks"].Value.ToString().ToLower().Contains("return"))
                    {
                        dgvProductDetails.CurrentRow.ReadOnly = true;
                        displayMessageLine(MessageUtils.getMessage(1025));
                        return;
                    }
                }
                if (dgvProductDetails.CurrentRow.Cells["quantity"].Value != null)
                {
                    if (Convert.ToInt32(dgvProductDetails.CurrentRow.Cells["quantity"].Value) <= 0)
                    {
                        dgvProductDetails.CurrentRow.ReadOnly = true;
                        displayMessageLine(MessageUtils.getMessage(1038));
                        return;
                    }
                }
                //Start update 2-Aug-2016
                //Added to check if the product type is manual
                if (dgvProductDetails.CurrentRow.Cells["product_id"].Value != null)
                {
                    if (!isManualProduct(Convert.ToInt32(dgvProductDetails.CurrentRow.Cells["product_id"].Value)))
                    {
                        dgvProductDetails.CurrentRow.ReadOnly = true;
                        displayMessageLine(MessageUtils.getMessage(1057));
                        return;
                    }
                }
                //End update 2-Aug-2016
                if (dgvProductDetails.Columns[e.ColumnIndex].Name == "MoreInfo")
                {
                    if (dgvProductDetails["product_id", e.RowIndex].Value != null && dgvProductDetails["product", e.RowIndex].Value != null)
                    {
                        frmMoreProductDetails f = new frmMoreProductDetails(Convert.ToInt32(dgvProductDetails["product_id", e.RowIndex].Value), dgvProductDetails["product", e.RowIndex].Value.ToString());
                        f.ShowDialog();
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvProductDetails_CellContentClick()");
        }

        private DataTable exchangeProductsSelectedRows(ref DataGridView dgv)
        {
            log.Debug("Starts-getSelectedProducts()");
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("Product Id", typeof(Int32));
                dt.Columns.Add("Product", typeof(string));
                dt.Columns.Add("Qty", typeof(float));
                dt.Columns.Add("Price", typeof(float));
                dt.Columns.Add("Amount", typeof(float));
                dt.Columns.Add("Tax", typeof(float));
                dt.Columns.Add("Tax Incl.", typeof(string));
                dt.Columns.Add("Last Sale Price", typeof(float));
                dt.Columns.Add("Line", typeof(Int32));
                if (dgv.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgv.Rows)
                    {
                        if (dr.Cells["SelectProduct"].Value.ToString() == "Y")
                        {
                            if (dr.Cells["ExchangeProductID"].Value != null)
                            {
                                DataRow row = dt.NewRow();
                                row["Product Id"] = dr.Cells["ExchangeProductID"].Value;
                                row["Product"] = dr.Cells["ExchangeProduct"].Value;
                                row["Qty"] = dr.Cells["ExchangeQuantity"].Value;
                                row["Price"] = dr.Cells["ExchangePrice"].Value;
                                row["Amount"] = dr.Cells["ExchangeAmount"].Value;
                                row["Tax"] = dr.Cells["ExchangeTax"].Value;
                                row["Tax Incl."] = dr.Cells["TaxInclv"].Value;
                                row["Last Sale Price"] = dr.Cells["ExchangeLastSalePrice"].Value;
                                if (dr.Cells["ExchangePdtLine"].Value == null || dr.Cells["ExchangePdtLine"].Value == DBNull.Value)
                                    row["Line"] = DBNull.Value;
                                else
                                    row["Line"] = dr.Cells["ExchangePdtLine"].Value;
                                dt.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-getSelectedProducts()");
            return dt;
        }

        private double getExchangeAmount()
        {
            double exchangeAmount = 0;
            try
            {
                if (dgvExchangeProducts.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgvExchangeProducts.Rows)
                    {
                        if (dr.Cells["SelectProduct"].Value.ToString() == "Y")
                        {
                            if (dr.Cells["ExchangeProductID"].Value != null)
                            {
                                if (dr.Cells["ExchangeAmount"].Value != null)
                                    exchangeAmount += Convert.ToDouble(dr.Cells["ExchangeAmount"].Value);
                            }
                        }
                    }
                }
                return exchangeAmount;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return exchangeAmount;
            }
        }

        private bool checkExchangeProductsSelected()
        {
            try
            {
                if (dgvExchangeProducts.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgvExchangeProducts.Rows)
                    {
                        if (dr.Cells["SelectProduct"].Value.ToString() == "Y")
                        {
                            if (dr.Cells["ExchangeProductID"].Value != null)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return false;
            }
        }

        private DataTable getSelectedProducts(ref DataGridView dgv)
        {
            log.Debug("Starts-getSelectedProducts()");
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("Product Id", typeof(Int32));
                dt.Columns.Add("Product", typeof(string));
                dt.Columns.Add("Qty", typeof(float));
                dt.Columns.Add("Price", typeof(float));
                dt.Columns.Add("Amount", typeof(float));
                dt.Columns.Add("Tax", typeof(float));
                dt.Columns.Add("Tax Incl.", typeof(string));
                dt.Columns.Add("Discount Amount", typeof(float));
                dt.Columns.Add("Last Sale Price", typeof(float));
                dt.Columns.Add("Line", typeof(Int32));
                dt.Columns.Add("OriginalLine", typeof(Int32));
                if (dgv.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in dgv.Rows)
                    {
                        if (dr.Cells["chkSelect"].Value.ToString() == "Y")
                        {
                            if (dr.Cells["product_id"].Value != null)
                            {
                                DataRow row = dt.NewRow();
                                row["Product Id"] = dr.Cells["product_id"].Value;
                                row["Product"] = dr.Cells["Product"].Value;
                                row["Qty"] = -1;
                                row["Price"] = dr.Cells["Price"].Value;
                                row["Amount"] = dr.Cells["amount"].Value;
                                row["Tax"] = dr.Cells["Tax"].Value;
                                row["Tax Incl."] = dr.Cells["tax_inclusive_price"].Value;
                                row["Discount Amount"] = dr.Cells["discount_amount"].Value;
                                row["Last Sale Price"] = dr.Cells["last_sale_price"].Value;
                                row["Line"] = dr.Cells["Line"].Value;
                                row["OriginalLine"] = dr.Cells["OriginalLine"].Value;
                                dt.Rows.Add(row);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-getSelectedProducts()");
            return dt;
        }

        private void btnInitiateReturn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnInitiateReturn_Click()");
            try
            {
                displayMessageLine("");
                dgvReturnProducts.Rows.Clear();
                this.Cursor = Cursors.WaitCursor;
                if (SelectedTransactionId == -1)
                    managerApprovalRequired = true;
                //Check if transaction was carried out more than RETURN_WITHIN_DAYS days ago
                if (SelectedTransactionId != -1)
                {
                    double numberOfDaysAfterTransaction = (DateTime.Today - TrxDate).TotalDays;
                    if (numberOfDaysAfterTransaction > RETURN_WITHIN_DAYS)
                        managerApprovalRequired = true;
                }

                DataTable dt = getSelectedProducts(ref dgvProductDetails);

                if (dt.Rows.Count == 0)
                {
                    displayMessageLine(MessageUtils.getMessage(1026));
                    this.Cursor = Cursors.Default;
                    return;
                }
                SalesReturnTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);

                string message = "";
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    int product_id = Convert.ToInt32(row["Product Id"]);
                    DataTable dtPrice = getPrice(product_id);
                    DataRow ProductDetails = SalesReturnTrx.getProductDetails(product_id);
                    //Transaction is not identified
                    if (SelectedTransactionId == -1)
                    {

                        if (0 != SalesReturnTrx.createTransactionLine(null, product_id, -1, -1, ref message))
                        {
                            displayMessageLine(message);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        double qty = 1;
                        double price = 0;
                        string taxInclusive = ProductDetails["TaxInclusivePrice"].ToString();
                        if (taxInclusive == "Y")
                        {
                            price = Convert.ToDouble(ProductDetails["Price"]) / (1.0 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100.0);
                        }
                        else
                        {
                            price = Convert.ToDouble(ProductDetails["Price"]);
                        }
                        double line_amt = price * qty;
                        double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                        line_amt += taxamt;
                        dgvReturnProducts.Rows.Add(product_id, row["Product"], -1, row["Price"], -1 * line_amt, -1 * taxamt, ProductDetails["TaxInclusivePrice"], 0, dtPrice.Rows[0]["sale_price"], i, row["Line"], row["Price"]);
                        i = i + 1;
                    }
                    else //Transaction is identified
                    {
                        SalesReturnTrx.OriginalTrxId = NewTrx.Trx_id;
                        if (0 != CloneFromOriginalTransaction(NewTrx, SalesReturnTrx, Convert.ToInt32(row["OriginalLine"]), ref message))
                        {
                            displayMessageLine(message);
                            return;
                        }
                        dgvReturnProducts.Rows.Add(product_id, row["Product"], -1, row["Price"], -1 * Convert.ToDouble(row["Amount"]), -1 * Convert.ToDouble(row["Tax"]), row["Tax Incl."], row["Discount Amount"], dtPrice.Rows[0]["sale_price"], i, row["Line"], row["Price"]);
                        i = i + 1;
                    }
                    SalesReturnTrx.TransactionPaymentsDTOList.Clear();
                }

                tcReturnExchange.TabPages.Add(tpReturn);
                tcReturnExchange.SelectTab(tpReturn);
                displayMessageLine("");
                tpReturn.BackColor = this.BackColor;
                btnInitiateExchange.Enabled = false;
                btnInitiateReturn.Enabled = false;

                refreshdgvReturnProducts();
                for (i = 0; i < dgvReturnProducts.Columns.Count; i++)
                {
                    dgvReturnProducts.Columns[i].ReadOnly = true;
                }
                if (SelectedTransactionId == -1)
                {
                    dgvReturnProducts.Columns["Qty"].ReadOnly = false;
                    dgvReturnProducts.Columns["ReturnPrice"].ReadOnly = false;
                    //Start update 20-Oct-2016
                    //Added to see that only Qty and price columns have a different backcolor
                    foreach (DataGridViewRow dr in dgvReturnProducts.Rows)
                    {
                        dr.Cells["Qty"].Style.BackColor = Color.PowderBlue;
                        dr.Cells["Qty"].Style.SelectionBackColor = Color.PowderBlue;
                        dr.Cells["ReturnPrice"].Style.BackColor = Color.PowderBlue;
                        dr.Cells["ReturnPrice"].Style.SelectionBackColor = Color.PowderBlue;
                    }
                    //End update 20-Oct-2016
                }

                dgvReturnProducts.Columns["ReturnLineRemarks"].ReadOnly = false;

                if (SelectedTransactionId != -1)
                    label10.Text = MessageUtils.getMessage(1028) + " " + SelectedTransactionId.ToString();
                else
                    label10.Text = MessageUtils.getMessage(1027);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnInitiateReturn_Click()");
        }

        int CloneFromOriginalTransaction(Transaction OriginalTrx, Transaction SalesReturnTrx, int OriginalLineID, ref string message)
        {
            try
            {
                if (NewTrx == null)
                    return 1;
                int Trx_id = OriginalTrx.Trx_id;
                foreach (Transaction.TransactionLine tl in OriginalTrx.TrxLines)
                {
                    if (tl.DBLineId == OriginalLineID)
                    {
                        Transaction.TransactionLine tlNew = new Transaction.TransactionLine();
                        double price;
                        if (tl.TaxInclusivePrice == "Y")
                        {
                            price = tl.Price / (1.0 + tl.Price / 100.0);
                            price = tl.Price + tl.tax_amount;
                        }
                        else
                            price = tl.Price;
                        if (0 != SalesReturnTrx.createTransactionLine(null, tl.ProductID, price, -1, ref message)) // trx amount exceeded limit
                        {
                            return 1;
                        }
                        int i = 0;
                        foreach (Transaction.TransactionLine t in SalesReturnTrx.TrxLines)
                        {
                            if (i == SalesReturnTrx.TrxLines.Count - 1)
                            {
                                if (t.LineValid)
                                {
                                    //Update original Line ID
                                    t.OriginalLineID = OriginalLineID;
                                    break;
                                }
                            }
                            i = i + 1;
                        }
                        break;
                    }
                }

                //invalidateDiscountLines(SalesReturnTrx);
                return 0;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return 1;
            }
        }

        private void btnInitiateExchange_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnInitiateExchange_Click()");
            try
            {
                displayMessageLine("");
                dgvExchangeReturnProducts.Rows.Clear();
                dgvExchangeProducts.Rows.Clear();
                this.Cursor = Cursors.WaitCursor;
                if (SelectedTransactionId == -1)
                    managerApprovalRequired = true;
                //If transaction is identified
                //Check if transaction was carried out more than RETURN_WITHIN_DAYS days ago
                if (SelectedTransactionId != -1)
                {
                    double numberOfDaysAfterTransaction = (DateTime.Today - TrxDate).TotalDays;
                    if (numberOfDaysAfterTransaction > RETURN_WITHIN_DAYS)
                        managerApprovalRequired = true;
                }

                //Get list of products that were selected for return from Product details tab
                DataTable dt = getSelectedProducts(ref dgvProductDetails);

                if (dt.Rows.Count == 0)
                {
                    displayMessageLine(MessageUtils.getMessage(1026));
                    this.Cursor = Cursors.Default;
                    return;
                }
                //Create new object for sales return
                SalesReturnTrx = new Transaction(POSStatic.POSPrintersDTOList, Utilities);
                displayMessageLine("");
                string message = "";
                foreach (DataRow row in dt.Rows)
                {
                    int product_id = Convert.ToInt32(row["Product Id"]);
                    DataTable dtPrice = getPrice(product_id);
                    if (SelectedTransactionId == -1)
                    {
                        DataRow ProductDetails = SalesReturnTrx.getProductDetails(product_id);
                        if (0 != SalesReturnTrx.createTransactionLine(null, product_id, -1, -1, ref message))
                        {
                            displayMessageLine(message);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        double qty = 1;
                        double price = 0;

                        if (ProductDetails["TaxInclusivePrice"].ToString() == "Y")
                        {
                            price = Convert.ToDouble(ProductDetails["Price"]) / (1.0 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100.0);
                        }
                        else
                        {
                            price = Convert.ToDouble(ProductDetails["Price"]);
                        }
                        double line_amt = price * qty;
                        double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                        line_amt += taxamt;
                        dgvExchangeReturnProducts.Rows.Add(product_id, row["Product"], -1, Convert.ToDouble(ProductDetails["Price"]), -1 * line_amt, -1 * taxamt, ProductDetails["TaxInclusivePrice"], 0, dtPrice.Rows[0]["sale_price"], row["Line"]);
                    }
                    else
                    {
                        SalesReturnTrx.OriginalTrxId = NewTrx.Trx_id;
                        if (0 != CloneFromOriginalTransaction(NewTrx, SalesReturnTrx, Convert.ToInt32(row["OriginalLine"]), ref message))
                        {
                            displayMessageLine(message);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        dgvExchangeReturnProducts.Rows.Add(product_id, row["Product"], -1, row["Price"], -1 * Convert.ToDouble(row["Amount"]), -1 * Convert.ToDouble(row["Tax"]), row["Tax Incl."], row["Discount Amount"], dtPrice.Rows[0]["sale_price"], row["Line"]);
                    }
                }

                tcReturnExchange.TabPages.Add(tpExchange);
                tcReturnExchange.SelectTab(tpExchange);
                displayMessageLine(MessageUtils.getMessage(1041));
                tpExchange.BackColor = this.BackColor;
                btnInitiateExchange.Enabled = false;
                btnInitiateReturn.Enabled = false;

                refreshdgvExchangeReturnProducts();
                for (int i = 0; i < dgvExchangeReturnProducts.Columns.Count; i++)
                {
                    dgvExchangeReturnProducts.Columns[i].ReadOnly = true;
                }
                if (SelectedTransactionId == -1)
                {
                    dgvExchangeReturnProducts.Columns["ReturnQuantity"].ReadOnly = false;
                    //Start update 20-Oct-2016
                    //Added to see that only Qty and price columns have a different backcolor
                    foreach (DataGridViewRow dr in dgvExchangeReturnProducts.Rows)
                    {
                        dr.Cells["ReturnQuantity"].Style.BackColor = Color.PowderBlue;
                        dr.Cells["ReturnQuantity"].Style.SelectionBackColor = Color.PowderBlue;
                    }
                    //End update 20-Oct-2016
                }

                dgvExchangeReturnProducts.Columns["ExchangeReturnLineRemarks"].ReadOnly = false;

                if (SelectedTransactionId != -1)
                    label2.Text = MessageUtils.getMessage(1028) + " " + SelectedTransactionId.ToString();
                else
                    label2.Text = MessageUtils.getMessage(1027);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnInitiateExchange_Click()");
        }

        private void btnPLBack_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Starts-btnPLBack_Click()");
                this.Cursor = Cursors.WaitCursor;
                displayMessageLine("");
                tcReturnExchange.SelectTab(tpTrxLookup);
                this.Cursor = Cursors.Default;
                log.Debug("Ends-btnPLBack_Click()");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
        }

        private void btnPLClose_Click(object sender, EventArgs e)
        {
            try
            {
                log.Debug("Starts-btnPLClose_Click()");
                displayMessageLine("");
                SalesReturnTrx = null;
                NewTrx = null;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
                log.Debug("Ends-btnPLClose_Click()");
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
        }

        #endregion ProductLookup

        #region Return

        private void btnCancelReturn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancelReturn_Click()");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SalesReturnTrx = null;
                //SalesReturnTrx.Trx_id = -1;
                //NewTrx = null;
                managerApprovalRequired = false;
                displayMessageLine(MessageUtils.getMessage(1042));
                tcReturnExchange.TabPages.Remove(tpReturn);
                tcReturnExchange.SelectTab(tpProductLookup);
                btnInitiateExchange.Enabled = true;
                btnInitiateReturn.Enabled = true;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnCancelReturn_Click()");
        }

        private void btnConfirmReturn_Click(object sender, EventArgs e)
        {
            displayMessageLine("");
            int managerID = -1;
            string payMode = "CREDITCARD";
            string message = "";

            log.Debug("Starts-btnConfirmReturn_Click()");
            //string message = "";
            try
            {
                if (NewTrx != null)
                    applyDiscountsToReturnProduct(NewTrx, SalesReturnTrx, ref message);
                this.Cursor = Cursors.WaitCursor;
                if (managerApprovalRequired)//Check if any update is carried out during the process
                {
                    if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                    {
                        displayMessageLine(MessageUtils.getMessage(268));
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else
                    {
                        managerID = Utilities.ParafaitEnv.ManagerId;

                    }
                    Users users = new Users(Utilities.ExecutionContext, Utilities.ParafaitEnv.ManagerId);
                    Utilities.ParafaitEnv.ApproverId = users.UserDTO.LoginId;
                    Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
                }

                if (SalesReturnTrx == null)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                //string message = "";
                Utilities.ParafaitEnv.SalesReturnType = "Return";
                SalesReturnTrx.PaymentReference = "RETURN";
                if (SalesReturnTrx.Net_Transaction_Amount != SalesReturnTrx.TotalPaidAmount)
                {
                    if (SalesReturnTrx.TotalPaidAmount != 0.00) // payment details exist so re-show it
                    {
                        if (!PaymentDetails())
                        {
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    else
                    {
                        message = "";
                        SalesReturnTrx.TransactionPaymentsDTOList.Clear();
                        if (!SaveReturnExchangeTransaction(SelectedTransactionId, "Return", SalesReturnTrx, ref message, ref payMode))
                        {
                            displayMessageLine(message);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    ReturnMessage = MessageUtils.getMessage(1029);
                    string userinfo = "";
                    if (SalesReturnTrx.Net_Transaction_Amount != 0)
                    {
                        if (payMode == "CASH")
                        {
                            double cashAmount = 0;
                            List<TransactionPaymentsDTO> originalTrxPaymentDTOList = getOriginalTrxPaymentDetails(SelectedTransactionId);
                            if (originalTrxPaymentDTOList != null && originalTrxPaymentDTOList.Exists(x => x.paymentModeDTO != null
                                                                                && x.paymentModeDTO.IsRoundOff))
                            {
                                cashAmount = POSStatic.CommonFuncs.RoundOff(SalesReturnTrx.Net_Transaction_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                            }
                            else
                                cashAmount = -1 * SalesReturnTrx.Net_Transaction_Amount;
                            userinfo = MessageUtils.getMessage(1051, (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                            userinfo += " " + MessageUtils.getMessage(30) + " " + (cashAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                        }
                        if (payMode == "CREDITCARD")
                        {
                            userinfo = MessageUtils.getMessage(1051, (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                            userinfo += MessageUtils.getMessage(1052, (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        }
                    }
                    else
                    {
                        userinfo = MessageUtils.getMessage(1051, (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                    }
                    //If any update was carried out, create an eventlog corresponding to manager approval
                    if (managerApprovalRequired)
                        Utilities.EventLog.logEvent("Return", 'D', ParafaitEnv.LoginID, "OriginalTrxId: " + SelectedTransactionId.ToString() + "; ReturnTrxId: " + SalesReturnTrx.Trx_id.ToString(), "Return", 0, "Manager Id", managerID.ToString(), null);
                    POSUtils.ParafaitMessageBox(userinfo, MessageUtils.getMessage(MessageUtils.getMessage(1044)));
                    btnConfirmReturn.Enabled = false;
                    btnCancelReturn.Enabled = false;
                    this.Cursor = Cursors.Default;
                    printTransaction(SalesReturnTrx.Trx_id);
                    SalesReturnTrx.TransactionPaymentsDTOList.Clear();
                    cleanUpOnNullTrx();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    if (message != string.Empty)
                    {
                        displayMessageLine(message);
                    }
                    else
                    {
                        displayMessageLine(MessageContainerList.GetMessage(Utilities.ExecutionContext, 2617));
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            finally
            {
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                Utilities.ParafaitEnv.ManagerId = -1;
                Utilities.ParafaitEnv.SalesReturnType = "";
            }
            log.Debug("Ends-btnConfirmReturn_Click()");
        }

        //Get original transaction payment details
        List<TransactionPaymentsDTO> getOriginalTrxPaymentDetails(int trxID)
        {
            log.LogMethodEntry(trxID);
            displayMessageLine("");
            try
            {
                TransactionPaymentsListBL trxPaymentsListBL = new TransactionPaymentsListBL();
                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                trxPaymentSearchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, trxID.ToString()));
                List<TransactionPaymentsDTO> transactionPaymentDTOList = trxPaymentsListBL.GetNonReversedTransactionPaymentsDTOListForReversal(trxPaymentSearchParameters);
                log.LogMethodExit(transactionPaymentDTOList);
                return transactionPaymentDTOList;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                log.LogMethodExit(null);
                return null;
            }
        }

        DateTime lastTrxActivityTime;
        bool PaymentDetails()
        {
            log.Debug("Starts-PaymentDetails()");
            try
            {
                if (SalesReturnTrx == null)
                {
                    return false;
                }
                lastTrxActivityTime = DateTime.Now;
                SalesReturnTrx.ClearRoundOffPayment(); // clear round off

                updatePaymentAmounts();
                //double savPaidAmount = SalesReturnTrx.TotalPaidAmount;

                if (SalesReturnTrx.isSavedTransaction())
                {
                    string lmessage = "";
                    SalesReturnTrx.TransactionPaymentsDTOList.Clear();
                    if (0 != SalesReturnTrx.SaveOrder(ref lmessage))
                    {
                        displayMessageLine(lmessage);
                        return false;
                    }
                }
                log.Debug("Ends-PaymentDetails()");
                return true;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return false;
            }
        }

        void updatePaymentAmounts()
        {
            log.Debug("Starts-updatePaymentAmounts()");
            if (SalesReturnTrx == null)
            {
                return;
            }

            if (SalesReturnTrx.TotalPaidAmount != SalesReturnTrx.Net_Transaction_Amount)
            {
                double balanceAmount = Math.Max(SalesReturnTrx.Net_Transaction_Amount - SalesReturnTrx.TotalPaidAmount, 0);
                PaymentModeDTO paymentModeDTO = new PaymentModeDTO();
                PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                if (POSStatic.AUTO_DEBITCARD_PAYMENT_POS || Utilities.ParafaitEnv.ALLOW_ONLY_GAMECARD_PAYMENT_IN_POS == "Y")
                {
                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                    searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                    List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                    if (paymentModeDTOList != null)
                    {
                        paymentModeDTO = paymentModeDTOList[0];
                    }
                }
                else
                {
                    if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 1)
                    {
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            paymentModeDTO = paymentModeDTOList[0];
                        }
                    }
                    else if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 2)
                    {
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCREDITCARD, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            paymentModeDTO = paymentModeDTOList[0];
                        }
                    }
                    else if (ParafaitEnv.PREFERRED_NON_CASH_PAYMENT_MODE == 3)
                    {
                        List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                        searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISDEBITCARD, "Y"));
                        List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                        if (paymentModeDTOList != null)
                        {
                            paymentModeDTO = paymentModeDTOList[0];
                        }
                    }
                }
                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTO.PaymentModeId, balanceAmount,
                                                                                      "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                      Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                trxPaymentDTO.paymentModeDTO = paymentModeDTO;
                App.machineUserContext = Utilities.ExecutionContext;
                App.EnsureApplicationResources();
                trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
            }
            log.Debug("Ends-updatePaymentAmounts()");
        }

        #endregion Return

        #region Exchange

        private void btnExchangeSKUSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnExchangeSKUSearch_Click()");
            displayMessageLine("");
            frmSKUSearch = new frmSKUSearch(txtExchangeProduct.Text);
            if (frmSKUSearch.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    btnExchangeProductSearch.PerformClick();
                }
                catch (SqlException ex)
                {
                    displayMessageLine(ex.Message);
                    this.Cursor = Cursors.Default;
                }
            }
            log.Debug("Ends-btnExchangeSKUSearch_Click()");
        }

        private int getForefeitProductId()
        {
            log.Debug("Starts-getForefeitProductId()");
            try
            {
                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"select top 1 product_id
                                    from products
                                    where product_name = 'Forfeit'";
                object o = cmd.ExecuteScalar();
                if (o != null && o != DBNull.Value)
                {
                    log.Debug("End-getForefeitProductId()");
                    return Convert.ToInt32(o);
                }
                log.Debug("End-getForefeitProductId()");
                return -1;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                log.Debug("End-getForefeitProductId()");
                return -1;
            }
        }

        private void btnExchangeProductSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnExchangeProductSearch_Click()");
            string skuSearchString = "";
            displayMessageLine("");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (frmSKUSearch != null)
                {
                    if (!string.IsNullOrEmpty(frmSKUSearch.searchString))
                    {
                        if (frmSKUSearch.searchString != "-1")
                        {
                            if (frmSKUSearch.searchString.Split(',').Length > 100)
                            {
                                displayMessageLine(MessageUtils.getMessage(1058));
                                this.Cursor = Cursors.Default;
                                return;
                            }
                            skuSearchString = " and p.segmentcategoryid in (" + frmSKUSearch.searchString + ") ";
                        }
                        else
                            skuSearchString = " ";
                    }
                }

                SqlCommand cmd = Utilities.getCommand();
                cmd.CommandText = @"select 'N' [SelectProduct],
                                        product_id ExchangeProductID, 
	                                    p.product_name ExchangeProduct,
                                        1 ExchangeQuantity,
	                                    p.price price,
                                        case taxinclusiveprice when 'Y' then (price/(1 + isnull((tax_percentage/100), 0))) + (price/(1 + (tax_percentage/100)) * (tax_percentage/100)) else price + (price * isnull(tax_percentage/100, 0) ) end amount,
	                                    case taxinclusiveprice when 'Y' then price/(1 + isnull((tax_percentage/100), 0)) * (tax_percentage/100) else price * isnull(tax_percentage/100, 0) end tax,
                                        p.taxinclusiveprice as tax_inclusive_price, 
	                                    (select top 1 case p.taxinclusiveprice when 'Y' then (l.price*(1 + isnull((l.tax_percentage/100), 0))) else l.price end price
									     from trx_lines l
									     where l.product_id = p.product_id
										    and quantity > 0
										    and trxid = (select max(trxid)
														    from trx_lines
														    where product_id = l.product_id
														    and quantity > 0)) last_sale_price,
                                        0 ExchangePdtLine
                                    from products p left outer join tax t on p.tax_id = t.tax_id
                                        , product rp left outer join (select * 
												                                    from ( 
														                                    select *, row_number() over(partition by productid order by productid) as num 
														                                    from productbarcode 
                                                                                            where barcode like @product and isactive = 'Y')v 
                                                                                    where num = 1) b on rp.productid = b.productid, product_type pt
                                        
                                     where p.product_id = rp.manualproductId 
                                        and (b.barcode like @product or p.product_name like @product or p.description like @product
			                                     or rp.code like @product or rp.description like @product)
                                        and p.active_flag = 'Y' 
                                        and pt.product_type_id = p.product_type_id
                                        and pt.product_type = 'MANUAL' "
                                    + skuSearchString +
                                    " order by 2";
                cmd.Parameters.AddWithValue("@product", "%" + txtExchangeProduct.Text + "%");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmd.Dispose();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!productExists(dgvExchangeProducts, Convert.ToInt32(dr["ExchangeProductID"]), "ExchangeProductID"))
                            dgvExchangeProducts.Rows.Add(dr.ItemArray);
                    }
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    displayMessageLine(MessageUtils.getMessage(1024));
                }

                frmSKUSearch = null;
                refreshdgvExchangeProducts();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-btnExchangeProductSearch_Click()");
        }

        private void btnClearExchangeProductSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClearExchangeProductSearch_Click()");
            displayMessageLine("");
            txtExchangeProduct.Text = "";
            frmSKUSearch = null;
            log.Debug("Ends-btnClearExchangeProductSearch_Click()");
        }



        private void btnConfirmExchange_Click(object sender, EventArgs e)
        {
            int managerID = -1;
            string payMode = "CREDITCARD";
            string message = "";
            double exchangeAmount = 0;

            log.Debug("Starts-btnConfirmExchange_Click()");
            if (!checkExchangeProductsSelected())
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(MessageUtils.getMessage(1033));
                return;
            }

            if (NewTrx != null)
            {
                //SalesReturnTrx.OriginalTrxId = NewTrx.Trx_id;
                applyDiscountsToReturnProduct(NewTrx, SalesReturnTrx, ref message);
                displayMessageLine("");
            }

            if (dgvExchangeProducts.Rows.Count > 0)
            {
                exchangeAmount = getExchangeAmount();
            }

            if (managerApprovalRequired)//Check if any update is carried out during the process
            {
                if (!Authenticate.Manager(ref Utilities.ParafaitEnv.ManagerId))
                {
                    displayMessageLine(MessageUtils.getMessage(268));
                    this.Cursor = Cursors.Default;
                    return;
                }
                else
                {
                    managerID = Utilities.ParafaitEnv.ManagerId;
                }
                Users users = new Users(Utilities.ExecutionContext, Utilities.ParafaitEnv.ManagerId);
                Utilities.ParafaitEnv.ApproverId = users.UserDTO.LoginId;
                Utilities.ParafaitEnv.ApprovalTime = Utilities.getServerTime();
            }

            try
            {
                //string message = "";
                string userinfo = "";
                //Get selected products list from exchange products section of exchange tab
                DataTable dt = exchangeProductsSelectedRows(ref dgvExchangeProducts);
                if (dt.Rows.Count == 0)
                {
                    displayMessageLine(MessageUtils.getMessage(1033));
                    return;
                }
                if (SalesReturnTrx == null)
                {
                    return;
                }

                Utilities.ParafaitEnv.SalesReturnType = "Exchange";
                SalesReturnTrx.PaymentReference = "EXCHANGE";
                if (SalesReturnTrx.Net_Transaction_Amount == 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (!SaveReturnExchangeTransaction(SelectedTransactionId, "Exchange", SalesReturnTrx, ref message, ref payMode))
                    {
                        this.Cursor = Cursors.Default;
                        displayMessageLine(message);
                        return;
                    }
                    this.Cursor = Cursors.Default;
                    userinfo = MessageUtils.getMessage(1053, exchangeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                    if (managerApprovalRequired)
                        Utilities.EventLog.logEvent("Exchange", 'D', ParafaitEnv.LoginID, "OriginalTrxId: " + SelectedTransactionId.ToString() + "; ReturnTrxId: " + SalesReturnTrx.Trx_id.ToString(), "Return", 0, "Manager Id", managerID.ToString(), null);
                    POSUtils.ParafaitMessageBox(userinfo, MessageUtils.getMessage(MessageUtils.getMessage(1044)));
                }
                if (SalesReturnTrx.Net_Transaction_Amount != SalesReturnTrx.TotalPaidAmount)
                {
                    if (SalesReturnTrx.TotalPaidAmount != 0.00) // payment details exist so re-show it
                    {
                        if (!PaymentDetails())
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (SalesReturnTrx.Net_Transaction_Amount < 0)
                        {
                            List<TransactionPaymentsDTO> originalTrxPaymentDTOList = getOriginalTrxPaymentDetails(SelectedTransactionId);
                            if (FORFEIT_BALANCE_RETURN_AMOUNT == "Y")
                            {
                                double forfeitedAmount = 0;
                                forfeitedAmount = SalesReturnTrx.Net_Transaction_Amount;
                                int forfeitProductID = getForefeitProductId();
                                if (forfeitProductID != -1 && forfeitedAmount != 0)
                                {
                                    if (0 != SalesReturnTrx.createTransactionLine(null, forfeitProductID, -1 * forfeitedAmount, 1, ref message))
                                    {
                                        displayMessageLine(message);
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }
                                }
                                else
                                {
                                    POSUtils.ParafaitMessageBox("Forfeit product not defined. Please create Forfeit product to continue.", MessageUtils.getMessage(MessageUtils.getMessage(1044)));
                                    return;
                                }
                                this.Cursor = Cursors.WaitCursor;
                                if (!SaveReturnExchangeTransaction(SelectedTransactionId, "Exchange", SalesReturnTrx, ref message, ref payMode))
                                {
                                    this.Cursor = Cursors.Default;
                                    displayMessageLine(message);
                                    return;
                                }
                                this.Cursor = Cursors.Default;
                                if (originalTrxPaymentDTOList != null && originalTrxPaymentDTOList.Exists(x => x.paymentModeDTO != null
                                                                                && x.paymentModeDTO.IsRoundOff))
                                {
                                    forfeitedAmount = POSStatic.CommonFuncs.RoundOff(forfeitedAmount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                }
                                userinfo = MessageUtils.getMessage(1054, SalesReturnTrx.Trx_id.ToString(), (-1 * forfeitedAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                            }
                            else
                            {
                                this.Cursor = Cursors.WaitCursor;
                                if (!SaveReturnExchangeTransaction(SelectedTransactionId, "Exchange", SalesReturnTrx, ref message, ref payMode))
                                {
                                    this.Cursor = Cursors.Default;
                                    displayMessageLine(message);
                                    return;
                                }
                                this.Cursor = Cursors.Default;
                                double cashAmount = 0;
                                if (originalTrxPaymentDTOList != null && originalTrxPaymentDTOList.Exists(x => x.paymentModeDTO != null
                                                                                && x.paymentModeDTO.IsRoundOff))
                                {
                                    cashAmount = POSStatic.CommonFuncs.RoundOff(SalesReturnTrx.Net_Transaction_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                }
                                else
                                    cashAmount = -1 * SalesReturnTrx.Net_Transaction_Amount;
                                userinfo = MessageUtils.getMessage(1053, (exchangeAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                                userinfo += " " + MessageUtils.getMessage(30) + " " + (cashAmount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                            }
                        }
                        else
                        {
                            using (PaymentDetails frmPayment = new PaymentDetails(SalesReturnTrx))
                            {
                                DialogResult dr = frmPayment.ShowDialog();
                                if (dr != DialogResult.OK)
                                {
                                    SalesReturnTrx.DeleteTransactionDiscounts();
                                    SalesReturnTrx.ClearTransactionDiscounts();
                                    SalesReturnTrx.UpdateDiscountsSummary();
                                    SalesReturnTrx.TransactionPaymentsDTOList.RemoveAll(x => x.PaymentId == -1);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                this.Cursor = Cursors.WaitCursor;
                                SalesReturnTrx.PaymentCreditCardSurchargeAmount = frmPayment.PaymentCreditCardSurchargeAmount;
                            }
                            SalesReturnTrx.PaymentReference = "EXCHANGE";
                            int retcode = SalesReturnTrx.SaveTransacation(ref message);
                            if (retcode != 0)
                            {
                                this.Cursor = Cursors.Default;
                                displayMessageLine(message);
                                return;
                            }
                            this.Cursor = Cursors.Default;
                            userinfo = MessageUtils.getMessage(1053, exchangeAmount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL), SalesReturnTrx.Trx_id.ToString());
                        }
                        if (managerApprovalRequired)
                            Utilities.EventLog.logEvent("Exchange", 'D', ParafaitEnv.LoginID, "OriginalTrxId: " + SelectedTransactionId.ToString() + "; ReturnTrxId: " + SalesReturnTrx.Trx_id.ToString(), "Return", 0, "Manager Id", managerID.ToString(), null);
                        POSUtils.ParafaitMessageBox(userinfo, MessageUtils.getMessage(MessageUtils.getMessage(1044)));
                    }
                }
                btnConfirmExchange.Enabled = false;
                btnCancelExchange.Enabled = false;
                this.Cursor = Cursors.Default;
                printTransaction(SalesReturnTrx.Trx_id);
                cleanUpOnNullTrx();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                this.Cursor = Cursors.Default;
            }
            finally
            {
                Utilities.ParafaitEnv.ApproverId = "-1";
                Utilities.ParafaitEnv.ApprovalTime = null;
                Utilities.ParafaitEnv.ManagerId = -1;
                Utilities.ParafaitEnv.SalesReturnType = "";
            }
            log.Debug("Ends-btnConfirmExchange_Click()");
        }

        private void btnCancelExchange_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCancelExchange_Click()");
            displayMessageLine("");
            btnClearExchangeProductSearch.PerformClick();
            dgvExchangeProducts.Rows.Clear();
            dgvExchangeReturnProducts.Rows.Clear();
            SalesReturnTrx = null;
            managerApprovalRequired = false;
            tcReturnExchange.TabPages.Remove(tpExchange);
            tcReturnExchange.SelectTab(tpProductLookup);
            btnInitiateReturn.Enabled = true;
            btnInitiateExchange.Enabled = true;
            displayMessageLine(MessageUtils.getMessage(1043));
            log.Debug("Ends-btnCancelExchange_Click()");
        }

        #endregion Exchange

        private void displayMessageLine(string message)
        {
            log.Debug("Starts-displayMessageLine()");
            textBoxMessageLine.Text = message;
            log.Debug("Ends-displayMessageLine()");
        }

        private void dgvReturnProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvReturnProducts_CellClick()");
            try
            {
                this.Cursor = Cursors.WaitCursor;
                displayMessageLine("");
                //If transaction is identified, quantity or price should not be updateable
                if (SelectedTransactionId != -1)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (dgvReturnProducts.Columns[e.ColumnIndex].Name == "Qty" || dgvReturnProducts.Columns[e.ColumnIndex].Name == "ReturnPrice")
                    //    updateReturnProductDetails(e.RowIndex, dgvReturnProducts.Columns[e.ColumnIndex].Name);
                    updateReturnProductQuantityPrice(e.RowIndex, dgvReturnProducts.Columns[e.ColumnIndex].Name);
                if (dgvReturnProducts.Columns[e.ColumnIndex].Name == "ReturnLineRemarks")
                {
                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage(201);
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage(1047);
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                        SalesReturnTrx.TrxLines[e.RowIndex].Remarks = TrxRemarks;
                        dgvReturnProducts.Rows[e.RowIndex].Cells["ReturnLineRemarks"].Value = TrxRemarks;
                    }
                    else
                    {
                        log.Info("Ends-CreateProduct() as Transaction Line Remarks dialog was cancelled");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                refreshdgvReturnProducts();
                for (int i = 0; i < dgvReturnProducts.Columns.Count; i++)
                {
                    dgvReturnProducts.Columns[i].ReadOnly = true;
                }
                if (SelectedTransactionId == -1)
                {
                    dgvReturnProducts.Columns["Qty"].ReadOnly = false;
                    dgvReturnProducts.Columns["ReturnPrice"].ReadOnly = false;
                    //Start update 20-Oct-2016
                    //Added to see that only Qty and price columns have a different backcolor
                    foreach (DataGridViewRow dr in dgvReturnProducts.Rows)
                    {
                        dr.Cells["Qty"].Style.BackColor = Color.PowderBlue;
                        dr.Cells["Qty"].Style.SelectionBackColor = Color.PowderBlue;
                        dr.Cells["ReturnPrice"].Style.BackColor = Color.PowderBlue;
                        dr.Cells["ReturnPrice"].Style.SelectionBackColor = Color.PowderBlue;
                    }
                    //End update 20-Oct-2016
                }
                dgvReturnProducts.Columns["ReturnLineRemarks"].ReadOnly = false;

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvReturnProducts_CellClick()");
        }

        void updateReturnProductQuantityPrice(int RowIndex, string Type)
        {
            log.Debug("Starts-updateReturnProductQuantityPrice(RowIndex,Type)");
            displayMessageLine("");
            if (RowIndex < 0 || SalesReturnTrx == null)
            {
                log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) as RowIndex < 0 || NewTrx == null");
                return;
            }

            if (dgvReturnProducts["ReturnLine", RowIndex].Value == null)
            {
                log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) as LineId == null");
                return;
            }

            int lineId = (int)dgvReturnProducts["ReturnLine", RowIndex].Value;
            if (Type == "Qty")
            {
                log.Info("updateReturnProductQuantityPrice(RowIndex,Type) - Quantity");//Added for logger function on 08-Mar-2016
                decimal quantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Quantity"), '-', Utilities);
                if (quantity > 0)
                {
                    log.Info("updateReturnProductQuantityPrice(RowIndex,Type) - Quantity > 0");//Added for logger function on 08-Mar-2016
                    string message = "";
                    updateReturnProductQuantity(RowIndex, -1 * quantity, ref message);
                    managerApprovalRequired = true;
                }
                else
                {
                    log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) - Quantity is less than 0");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            else if (Type == "ReturnPrice")
            {
                log.Info("updateReturnProductQuantityPrice(RowIndex,Type) - Price");//Added for logger function on 08-Mar-2016
                double Price = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Price"), '-', Utilities);
                if (Price >= 0)
                {
                    if (Price == 0 && POSUtils.ParafaitMessageBox(MessageUtils.getMessage(485), MessageUtils.getMessage("User Price"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) - Price is 0");//Added for logger function on 08-Mar-2016
                        return;
                    }
                    if (Price > Convert.ToDouble(dgvReturnProducts["OriginalPrice", RowIndex].Value))
                    {
                        displayMessageLine(MessageUtils.getMessage(1037));
                        return;
                    }

                    string message = SalesReturnTrx.TrxLines[lineId].Remarks;
                    updateReturnProductPrice(lineId, Price);
                    managerApprovalRequired = true;
                }
                else
                {
                    log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) - Price is less than 0");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            SalesReturnTrx.updateAmounts();
            log.Debug("Ends-updateReturnProductQuantityPrice(RowIndex,Type)");//Added for logger function on 08-Mar-2016
        }

        void updateReturnProductPrice(int LineId, double Price)
        {
            double lineAmt = 0;

            log.Debug("Starts-updateReturnProductPrice()");
            try
            {
                displayMessageLine("");
                int productID = Convert.ToInt32(SalesReturnTrx.TrxLines[LineId].ProductID);
                double taxAmt = 0;
                double selectedPrice = Price;

                foreach (Transaction.TransactionLine l in SalesReturnTrx.TrxLines)
                {
                    if (l.ProductID == productID)
                    {
                        l.UserPrice = true;
                        if (l.TaxInclusivePrice == "Y")
                        {
                            Price = selectedPrice / (1 + l.tax_percentage / 100.0);
                        }
                        l.Price = Price;

                        SalesReturnTrx.updateAmounts();
                        lineAmt = -1 * l.LineAmount;
                        taxAmt = -1 * l.tax_amount;
                    }
                    if (l.TransactionDiscountsDTOList != null &&
                        l.TransactionDiscountsDTOList.Count > 0)
                    {
                        foreach (var transactionDiscountsDTO in l.TransactionDiscountsDTOList)
                        {
                            SalesReturnTrx.cancelDiscountLine(transactionDiscountsDTO.DiscountId);
                            updatePaymentAmounts();
                            SalesReturnTrx.updateAmounts();
                        }
                    }
                }
                dgvReturnProducts["ReturnPrice", LineId].Value = selectedPrice;
                if (SalesReturnTrx.TrxLines[LineId].TaxInclusivePrice == "Y")
                    dgvReturnProducts["ReturnAmount", LineId].Value = (Price + taxAmt) * Convert.ToDouble(dgvReturnProducts["Qty", LineId].Value);
                else
                    dgvReturnProducts["ReturnAmount", LineId].Value = lineAmt * Convert.ToDouble(dgvReturnProducts["Qty", LineId].Value);
                dgvReturnProducts["ReturnTax", LineId].Value = taxAmt * Convert.ToDouble(dgvReturnProducts["Qty", LineId].Value);
                dgvReturnProducts["DiscAmount", LineId].Value = 0;
                updatePaymentAmounts();
                SalesReturnTrx.updateAmounts();
                displayMessageLine(MessageUtils.getMessage(243));
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-updateReturnProductPrice()");//Added for logger function on 08-Mar-2016
        }

        //Updated the method completely 21-Oct-2016
        //Old method would not show proper amount values on quantity update
        void updateReturnProductQuantity(int RowIndex, decimal Quantity, ref string message)
        {
            try
            {
                log.Debug("Starts-updateReturnProductQuantity()");
                displayMessageLine("");
                decimal savQty = Convert.ToDecimal(dgvReturnProducts["Qty", RowIndex].Value);
                if (savQty == Quantity)
                    return;

                bool retVal = true;
                this.Cursor = Cursors.WaitCursor;
                int LineId = (int)dgvReturnProducts["ReturnLine", RowIndex].Value;
                int productId = Convert.ToInt32(dgvReturnProducts["ProductId", RowIndex].Value); //Update 21-Oct-2016
                DataRow ProductDetails = SalesReturnTrx.getProductDetails(productId);
                double price = 0;
                string taxInclusive = ProductDetails["TaxInclusivePrice"].ToString();
                double selectedPrice = Convert.ToInt32(dgvReturnProducts["ReturnPrice", RowIndex].Value);
                if (taxInclusive == "Y")
                    price = selectedPrice / (1 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                else
                    price = selectedPrice;

                double line_amt = price * Convert.ToDouble(Quantity);
                double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                line_amt += taxamt;

                decimal diffQty = (-1 * Quantity) - (-1 * savQty);

                if (diffQty < 0)
                {
                    //List<int> cancelLines;
                    //if (dgvReturnProducts.Rows[RowIndex].Tag == null)
                    //{
                    //    cancelLines = new List<int>();
                    //    cancelLines.Add(LineId);
                    //}
                    //else
                    //    cancelLines = dgvReturnProducts.Rows[RowIndex].Tag as List<int>;

                    int lineCnt = 0;
                    foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                    {
                        if (tl.ProductID == productId)
                        {
                            tl.CancelledLine = true; //Added 18-Oct-2016
                            SalesReturnTrx.cancelLine(lineCnt);
                            SalesReturnTrx.updateAmounts();
                            diffQty++;
                        }
                        lineCnt = lineCnt + 1;
                        if (diffQty == 0)
                            break;
                    }
                }
                else if (diffQty > 0)
                {
                    for (int i = 0; i < diffQty; i++)
                    {
                        if (0 != SalesReturnTrx.createTransactionLine(null, productId, selectedPrice, -1, ref message))
                        {
                            retVal = false;
                        }

                        SalesReturnTrx.updateAmounts();
                        displayMessageLine(MessageUtils.getMessage(242));
                        log.Info("updateReturnProductQuantity() - Quantity updated");
                        if (!retVal)
                            break;
                    }
                }
                SalesReturnTrx.updateAmounts();
                dgvReturnProducts["Qty", RowIndex].Value = Quantity;
                //if (taxInclusive == "Y")
                //    dgvReturnProducts["ReturnAmount", RowIndex].Value = Convert.ToDouble(Quantity) * price;
                //else
                dgvReturnProducts["ReturnAmount", RowIndex].Value = line_amt;
                dgvReturnProducts["ReturnTax", RowIndex].Value = taxamt;
                updatePaymentAmounts();
                displayMessageLine(MessageUtils.getMessage(242));
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                log.Info("updateReturnProductQuantity() -" + ex.Message);//Added for logger function on 21-Apr-2016
                return;
            }
        }


        private void dgvReturnProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvReturnProducts_CellDoubleClick()");
            displayMessageLine("");
            double ProductQuantity = 1;
            ProductQuantity = (int)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage(479), '-', Utilities);
            if (ProductQuantity <= 0)
            {
                return;
            }
            else if (ProductQuantity >= 1)
            {
                dgvReturnProducts["Qty", e.RowIndex].Value = -1 * ProductQuantity;
            }
            log.Debug("Ends-dgvReturnProducts_CellDoubleClick()");
        }

        private void dgvReturnProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.Debug("Starts-dataGridViewTransaction_CellMouseClick()");
            try
            {
                displayMessageLine("");
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as e.ColumnIndex < 0 || e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
                if (SalesReturnTrx == null)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as NewTrx == null");//Added for logger function on 08-Mar-2016
                    return;
                }

                dgvReturnProducts.CurrentCell = dgvReturnProducts[e.ColumnIndex, e.RowIndex];
                int line = Convert.ToInt32(dgvReturnProducts["ReturnLine", dgvReturnProducts.CurrentRow.Index].Value);
                if (line < 0)
                {
                    log.Info("Ends-dataGridViewTransaction_CellMouseClick() as line < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
        }

        private void btnCloseReturn_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCloseReturn_Click()");
            if (SalesReturnTrx.Trx_id != -1 && SalesReturnTrx != null)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
            SalesReturnTrx = null;
            NewTrx = null;
            this.Close();
            log.Debug("Ends-btnCloseReturn_Click()");
        }

        AlphaNumericKeyPad keypad;
        void showAlphaNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showAlphaNumberPadForm()");
            if (CurrentAlphanumericTextBox != null)
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, CurrentAlphanumericTextBox);
                    if (this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60 + keypad.Height < Screen.PrimaryScreen.WorkingArea.Height)
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y + 60);
                    else
                        keypad.Location = new Point(this.Location.X, this.PointToScreen(CurrentAlphanumericTextBox.Location).Y - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.Show();
                }
            }
            log.Debug("Ends-showAlphaNumberPadForm()");
        }

        private void btnKeyboardProductLookup_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnKeyboardProductLookup_Click()");
            CurrentAlphanumericTextBox = txtProduct;
            showAlphaNumberPadForm('-');
            log.Debug("Ends-btnKeyboardProductLookup_Click()");
        }

        private void btnExchangeProductLookup_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnExchangeProductLookup_Click()");
            CurrentAlphanumericTextBox = txtExchangeProduct;
            showAlphaNumberPadForm('-');
            log.Debug("Ends-btnExchangeProductLookup_Click()");
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnShowNumPad_Click()");//Added for logger function on 08-Mar-2016
            CurrentTextBox = txtTrxID;
            if (txtTrxID.Equals(CurrentAlphanumericTextBox))
            {
                showAlphaNumberPadForm('-');
            }
            else
            {
                if (CurrentTextBox != null)
                    try
                    {
                        this.ActiveControl = CurrentTextBox;
                    }
                    catch { }

                showNumberPadForm('-');
            }
            log.Debug("Ends-btnShowNumPad_Click()");
        }

        void showNumberPadForm(char firstKey)
        {
            log.Debug("Starts-showNumberPadForm()");
            double varAmount = NumberPadForm.ShowNumberPadForm("Enter TrxID", firstKey, Utilities);
            if (varAmount >= 0)
            {
                TextBox txtBox = null;
                try
                {
                    if (this.ActiveControl.GetType().ToString().ToLower().Contains("textbox"))
                        txtBox = this.ActiveControl as TextBox;
                }
                catch { }

                if (txtBox != null && !txtBox.ReadOnly)
                {
                    txtBox.Text = varAmount.ToString();
                    this.ValidateChildren();
                    btnSearchTrx.PerformClick();
                }
            }
            log.Debug("Ends-showNumberPadForm()");
        }

        private void dgvReturnProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvReturnProducts_CellContentClick()");
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvReturnProducts.CurrentRow == null || dgvReturnProducts.CurrentRow.Cells["product_id"].Value == null)
                        return;
                }

                if (dgvReturnProducts.Columns[e.ColumnIndex].Name == "ProductDetails")
                {
                    if (dgvReturnProducts["ProductId", e.RowIndex].Value != null && dgvReturnProducts["ReturnProduct", e.RowIndex].Value != null)
                    {
                        frmMoreProductDetails f = new frmMoreProductDetails(Convert.ToInt32(dgvReturnProducts["ProductId", e.RowIndex].Value), dgvReturnProducts["ReturnProduct", e.RowIndex].Value.ToString());
                        f.ShowDialog();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvReturnProducts_CellContentClick()");
        }

        private void btnCloseExchange_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnCloseExchange_Click()");
            if (SalesReturnTrx.Trx_id != -1 && SalesReturnTrx != null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            SalesReturnTrx = null;
            NewTrx = null;
            log.Debug("Ends-btnCloseExchange_Click()");
            this.Close();
        }

        private void dgvExchangeReturnProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvExchangeReturnProducts_CellContentClick()");
            try
            {
                displayMessageLine("");
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvExchangeReturnProducts.CurrentRow == null || dgvExchangeReturnProducts.CurrentRow.Cells["ReturnProductID"].Value == null)
                        return;
                }

                if (dgvExchangeReturnProducts.Columns[e.ColumnIndex].Name == "MoreInformation")
                {
                    if (dgvExchangeReturnProducts["ReturnProductID", e.RowIndex].Value != null && dgvExchangeReturnProducts["ReturnProductName", e.RowIndex].Value != null)
                    {
                        frmMoreProductDetails f = new frmMoreProductDetails(Convert.ToInt32(dgvExchangeReturnProducts["ReturnProductID", e.RowIndex].Value), dgvExchangeReturnProducts["ReturnProductName", e.RowIndex].Value.ToString());
                        f.ShowDialog();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvExchangeReturnProducts_CellContentClick()");
        }

        private void dgvExchangeProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "SelectProduct")
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvExchangeProducts.CurrentRow == null || dgvExchangeProducts.CurrentRow.Cells["SelectProduct"].Value == null)
                        return;
                }
                updateExchangeProductSelection(e.RowIndex);
            }
        }

        private void dgvExchangeProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvExchangeProducts_CellContentClick()");
            try
            {
                displayMessageLine("");
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;

                if (e.ColumnIndex == 0 || e.ColumnIndex == 1)
                {
                    if (dgvExchangeProducts.CurrentRow == null || dgvExchangeProducts.CurrentRow.Cells["ExchangeProductID"].Value == null)
                        return;
                }

                if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "ExchangeMoreInfo")
                {
                    if (dgvExchangeProducts["ExchangeProductID", e.RowIndex].Value != null && dgvExchangeProducts["ExchangeProduct", e.RowIndex].Value != null)
                    {
                        frmMoreProductDetails f = new frmMoreProductDetails(Convert.ToInt32(dgvExchangeProducts["ExchangeProductID", e.RowIndex].Value), dgvExchangeProducts["ExchangeProduct", e.RowIndex].Value.ToString());
                        f.ShowDialog();
                    }
                    else
                    {
                        return;
                    }
                }
                else if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "SelectProduct")
                {
                    dgvExchangeProducts.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvExchangeProducts_CellContentClick()");
        }

        private void txtTrxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            displayMessageLine("");
            if (e.KeyChar == 13)
                btnSearchTrx.PerformClick();
        }

        private void txtProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            displayMessageLine("");
            if (e.KeyChar == 13)
                btnProductSearch.PerformClick();
        }

        private void txtExchangeProduct_KeyPress(object sender, KeyPressEventArgs e)
        {
            displayMessageLine("");
            if (e.KeyChar == 13)
                btnExchangeProductSearch.PerformClick();
        }

        private void dgvExchangeReturnProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvExchangeReturnProducts_CellClick()");
            displayMessageLine("");
            try
            {
                //If transaction is identified, quantity should not be updateable
                if (SelectedTransactionId != -1)
                    return;
                if (dgvExchangeReturnProducts.Columns[e.ColumnIndex].Name == "ReturnQuantity")
                    updateExchangeReturnProductQuantity(e.RowIndex);
                if (dgvExchangeReturnProducts.Columns[e.ColumnIndex].Name == "ExchangeReturnLineRemarks")
                {
                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage(201);
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage(1047);
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                        SalesReturnTrx.TrxLines[e.RowIndex].Remarks = TrxRemarks;
                        dgvExchangeReturnProducts.Rows[e.RowIndex].Cells["ExchangeReturnLineRemarks"].Value = TrxRemarks;
                    }
                    else
                    {
                        log.Info("Ends-CreateProduct() as Transaction Line Remarks dialog was cancelled");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                refreshdgvExchangeReturnProducts();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvExchangeReturnProducts_CellClick()");
        }

        //Updated the method completely 21-Oct-2016
        //Old method would not show proper amount values on quantity update
        bool updateExchangeReturnProductQuantity(int RowIndex)
        {
            string message = "";
            try
            {
                displayMessageLine("");
                log.Debug("Starts-updateExchangeReturnProductQuantity(RowIndex)");
                if (RowIndex < 0 || SalesReturnTrx == null)
                {
                    log.Info("Ends-updateExchangeReturnProductQuantity(RowIndex) as RowIndex < 0 || NewTrx == null");
                    return false;
                }

                if (dgvExchangeReturnProducts["ExchangeLine", RowIndex].Value == null)
                {
                    log.Info("Ends-updateExchangeReturnProductQuantity(RowIndex) as LineId == null");
                    return false;
                }

                int lineId = (int)dgvExchangeReturnProducts["ExchangeLine", RowIndex].Value;

                log.Info("updateExchangeReturnProductQuantity(RowIndex) - Quantity");//Added for logger function on 08-Mar-2016
                decimal quantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Quantity"), '-', Utilities);
                if (quantity <= 0)
                {
                    log.Info("Ends-updateReturnProductQuantityPrice(RowIndex,Type) - Quantity is less than 0");//Added for logger function on 08-Mar-2016
                    return false;
                }
                this.Cursor = Cursors.WaitCursor;
                quantity = quantity * -1;
                if (quantity == 0 || quantity == Convert.ToDecimal(dgvExchangeReturnProducts["ReturnQuantity", RowIndex].Value))
                {
                    this.Cursor = Cursors.Default;
                    log.Info("Ends-updateExchangeReturnProductQuantity(RowIndex) - Quantity is less than 0");//Added for logger function on 08-Mar-2016
                    return false;
                }

                log.Debug("Starts-updateExchangeReturnProductQuantity()");//Added for logger function on 08-Mar-2016

                decimal savQty = Convert.ToDecimal(dgvExchangeReturnProducts["ReturnQuantity", RowIndex].Value);
                if (savQty == quantity)
                {
                    this.Cursor = Cursors.Default;
                    return true;
                }

                bool retVal = true;

                int LineId = (int)dgvExchangeReturnProducts["ExchangeLine", RowIndex].Value;
                int productId = (int)dgvExchangeReturnProducts["ReturnProductID", RowIndex].Value;
                DataRow ProductDetails = SalesReturnTrx.getProductDetails(productId);
                double newAmount = 0;
                double price = 0;
                double selectedPrice = Convert.ToDouble(dgvExchangeReturnProducts["ReturnPdtPrice", RowIndex].Value);
                if (ProductDetails["TaxInclusivePrice"].ToString() == "Y")
                    price = selectedPrice / (1 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                else
                    price = selectedPrice;

                double line_amt = price * Convert.ToDouble(quantity);
                double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                line_amt += taxamt;
                managerApprovalRequired = true;
                decimal diffQty = (-1 * quantity) - (-1 * savQty);

                if (diffQty < 0)
                {
                    int lineCnt = 0;
                    foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                    {
                        if (tl.ProductID == productId && tl.quantity < 0)
                        {
                            SalesReturnTrx.TrxLines[lineCnt].CancelledLine = true; //Added 20-Oct-2016
                            SalesReturnTrx.cancelLine(lineCnt);
                            SalesReturnTrx.updateAmounts();
                            diffQty++;
                        }
                        lineCnt = lineCnt + 1;
                        if (diffQty == 0)
                            break;
                    }
                }
                else if (diffQty > 0)
                {
                    for (int i = 0; i < diffQty; i++)
                    {
                        Transaction.TransactionLine newLine = new Transaction.TransactionLine();
                        newLine.ComboChildLine = SalesReturnTrx.TrxLines[LineId].ComboChildLine;
                        newLine.ModifierLine = SalesReturnTrx.TrxLines[LineId].ModifierLine;
                        message = SalesReturnTrx.TrxLines[LineId].Remarks;

                        if (SalesReturnTrx.createTransactionLine(null, productId, price, -1, ref message, newLine) == 8) // trx amount exceeded limit
                        {
                            retVal = false;
                        }

                        if (message != "")
                        {
                            displayMessageLine(message);
                            log.Error("updateExchangeReturnProductQuantity() error: " + message);//Added for logger function on 08-Mar-2016
                        }
                        else
                        {
                            SalesReturnTrx.updateAmounts();
                            newAmount = newLine.LineAmount;
                            displayMessageLine(MessageUtils.getMessage(242));
                            log.Info("updateExchangeReturnProductQuantity() - Quantity updated");//Added for logger function on 08-Mar-2016
                        }
                        if (!retVal)
                            break;
                    }
                }
                SalesReturnTrx.updateAmounts();
                dgvExchangeReturnProducts["ReturnQuantity", RowIndex].Value = quantity;
                dgvExchangeReturnProducts["ReturnAmt", RowIndex].Value = line_amt;
                dgvExchangeReturnProducts["ReturnTaxAmt", RowIndex].Value = taxamt;
                updatePaymentAmounts();
                displayMessageLine(MessageUtils.getMessage(242));
                this.Cursor = Cursors.Default;
                return retVal;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                log.Info("updateReturnProductQuantity() -" + ex.Message);//Added for logger function on 21-Apr-2016
                return false;
            }
        }

        private void dgvExchangeReturnProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.Debug("Starts-dgvExchangeReturnProducts_CellMouseClick()");
            displayMessageLine("");
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.Info("Ends-dgvExchangeReturnProducts_CellMouseClick() as e.ColumnIndex < 0 || e.RowIndex < 0");
                    return;
                }
                if (SalesReturnTrx == null)
                {
                    log.Info("Ends-dgvExchangeReturnProducts_CellMouseClick() as NewTrx == null");
                    return;
                }

                dgvExchangeReturnProducts.CurrentCell = dgvExchangeReturnProducts[e.ColumnIndex, e.RowIndex];
                int line = Convert.ToInt32(dgvExchangeReturnProducts["ExchangeLine", dgvExchangeReturnProducts.CurrentRow.Index].Value);
                if (line < 0)
                {
                    log.Info("Ends-dgvExchangeReturnProducts_CellMouseClick() as line < 0");
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                return;
            }
        }

        void updateExchangeProductQuantity(int RowIndex)
        {
            try
            {
                log.Debug("Starts-updateExchangeProductQuantity(RowIndex)");
                displayMessageLine("");
                if (RowIndex < 0 || SalesReturnTrx == null)
                {
                    log.Info("Ends-updateExchangeProductQuantity(RowIndex) as RowIndex < 0 || NewTrx == null");
                    return;
                }

                if (dgvExchangeProducts["ExchangePdtLine", RowIndex].Value == null)
                {
                    log.Info("Ends-updateExchangeProductQuantity(RowIndex) as LineId == null");
                    return;
                }

                int lineId = (int)dgvExchangeProducts["ExchangePdtLine", RowIndex].Value;

                log.Info("updateExchangeProductQuantity(RowIndex) - Quantity");//Added for logger function on 08-Mar-2016
                decimal Quantity = (decimal)NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Quantity"), '-', Utilities);
                this.Cursor = Cursors.WaitCursor;
                if (Quantity < 1 || Quantity == Convert.ToDecimal(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value))
                {
                    this.Cursor = Cursors.Default;
                    log.Info("Ends-updateExchangeReturnProductQuantity(RowIndex) - Quantity is less than 0");//Added for logger function on 08-Mar-2016
                    return;
                }

                int LineId = (int)dgvExchangeProducts["ExchangePdtLine", RowIndex].Value;
                int productId = (int)dgvExchangeProducts["ExchangeProductID", RowIndex].Value;

                DataRow ProductDetails = SalesReturnTrx.getProductDetails(productId);

                double price = 0;
                //Added Oct-18-2016
                double selectedPrice = Convert.ToDouble(dgvExchangeProducts["ExchangePrice", RowIndex].Value);

                if (ProductDetails["TaxInclusivePrice"].ToString() == "Y")
                    price = selectedPrice / (1 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                else
                    price = selectedPrice;
                double line_amt = price;
                double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);

                if (Quantity < 0 || Quantity == Convert.ToDecimal(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value))
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                string message = "";
                bool retVal = true;
                decimal savQty = Convert.ToDecimal(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value);
                if (dgvExchangeProducts["SelectProduct", RowIndex].Value.ToString() == "Y")
                {
                    decimal diffQty = (Quantity) - (savQty);

                    if (diffQty < 0)
                    {
                        int i = 0;
                        int lineCnt = 0;
                        foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                        {
                            if (tl.ProductID == productId && tl.quantity > 0)
                            {
                                tl.CancelledLine = true; //Added 18-Oct-2016
                                SalesReturnTrx.cancelLine(lineCnt);
                                SalesReturnTrx.updateAmounts();
                                i++;
                            }
                            lineCnt = lineCnt + 1;
                            if (i == (diffQty * -1))
                                break;
                        }
                    }
                    else if (diffQty > 0)
                    {
                        for (int i = 0; i < diffQty; i++)
                        {
                            Transaction.TransactionLine newLine = new Transaction.TransactionLine();
                            newLine.ComboChildLine = SalesReturnTrx.TrxLines[LineId].ComboChildLine;
                            newLine.ModifierLine = SalesReturnTrx.TrxLines[LineId].ModifierLine;
                            message = SalesReturnTrx.TrxLines[LineId].Remarks;

                            //Updated condition to consider user price instead of product price 18-Oct-2016
                            if (SalesReturnTrx.createTransactionLine(null, productId, selectedPrice, 1, ref message, newLine) == 8) // trx amount exceeded limit
                            {
                                retVal = false;
                            }

                            if (message != "")
                            {
                                displayMessageLine(message);
                                log.Error("updateExchangeReturnProductQuantity() error: " + message);//Added for logger function on 08-Mar-2016
                            }
                            else
                            {
                                //invalidateDiscountLines(SalesReturnTrx);
                                SalesReturnTrx.updateAmounts();
                                //line_amt = newLine.LineAmount;
                                displayMessageLine(MessageUtils.getMessage(242));
                                log.Info("updateExchangeReturnProductQuantity() - Quantity updated");//Added for logger function on 08-Mar-2016
                            }
                            if (!retVal)
                                break;
                        }
                    }
                    SalesReturnTrx.updateAmounts();
                    //End
                }

                dgvExchangeProducts["ExchangeQuantity", RowIndex].Value = Quantity;
                dgvExchangeProducts["ExchangeAmount", RowIndex].Value = Convert.ToDouble(Quantity) * (line_amt + taxamt);
                dgvExchangeProducts["ExchangeTax", RowIndex].Value = Convert.ToDouble(Quantity) * taxamt;
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                log.Info("updateReturnProductQuantity() -" + ex.Message);//Added for logger function on 21-Apr-2016
                return;
            }
        }

        //Start update 18-Oct-2016
        //Added method to handle exchange product price update
        void updateExchangeProductPrice(int RowIndex)
        {
            try
            {
                log.Debug("Starts-updateExchangeProductPrice(RowIndex)");
                displayMessageLine("");
                if (RowIndex < 0 || SalesReturnTrx == null)
                {
                    log.Info("Ends-updateExchangeProductPrice(RowIndex) as RowIndex < 0 || NewTrx == null");
                    return;
                }

                if (dgvExchangeProducts["ExchangePdtLine", RowIndex].Value == null)
                {
                    log.Info("Ends-updateExchangeProductPrice(RowIndex) as LineId == null");
                    return;
                }

                int LineId = (int)dgvExchangeProducts["ExchangePdtLine", RowIndex].Value;
                int productId = (int)dgvExchangeProducts["ExchangeProductID", RowIndex].Value;

                log.Info("updateExchangeProductPrice(RowIndex) - Price");//Added for logger function on 08-Mar-2016
                double Price = NumberPadForm.ShowNumberPadForm(MessageUtils.getMessage("Change Price"), '-', Utilities);
                if (Price >= 0)
                {
                    if (Price == 0 && POSUtils.ParafaitMessageBox(MessageUtils.getMessage(485), MessageUtils.getMessage("User Price"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                    {
                        log.Info("Ends-updateExchangeProductPrice(RowIndex) - Price is 0");//Added for logger function on 08-Mar-2016
                        return;
                    }

                    log.Debug("Starts-updateExchangeProductPrice(RowIndex)");
                    try
                    {
                        displayMessageLine("");
                        double selectedPrice = Price;

                        DataRow ProductDetails = SalesReturnTrx.getProductDetails(productId);
                        if (ProductDetails["TaxInclusivePrice"].ToString() == "Y")
                            Price = selectedPrice / (1 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                        else
                            Price = selectedPrice;

                        double line_amt = Price;
                        double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                        foreach (Transaction.TransactionLine l in SalesReturnTrx.TrxLines)
                        {
                            if (l.ProductID == productId && l.quantity > 0)
                            {
                                l.UserPrice = true;
                                l.Price = Price;

                                SalesReturnTrx.updateAmounts();
                            }
                        }
                        dgvExchangeProducts["ExchangePrice", RowIndex].Value = selectedPrice;
                        dgvExchangeProducts["ExchangeAmount", RowIndex].Value = (line_amt + taxamt) * Convert.ToDouble(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value);
                        dgvExchangeProducts["ExchangeTax", RowIndex].Value = taxamt * Convert.ToDouble(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value);
                        updatePaymentAmounts();
                        SalesReturnTrx.updateAmounts();
                        displayMessageLine(MessageUtils.getMessage(243));
                    }
                    catch (Exception ex)
                    {
                        displayMessageLine(ex.Message);
                    }
                    log.Debug("Ends-updateReturnProductPrice()");//Added for logger function on 08-Mar-2016

                    if (dgvExchangeProducts["SelectProduct", RowIndex].Value.ToString() == "Y")
                        managerApprovalRequired = true;
                }
                else
                {
                    log.Info("Ends-updateExchangeProductPrice(RowIndex) - Price is less than 0");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            catch { }
        }
        //End update 18-Oct-2016

        private void dgvExchangeProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvExchangeProducts_CellClick()");
            try
            {
                displayMessageLine("");
                if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "ExchangeQuantity")
                    updateExchangeProductQuantity(e.RowIndex);
                //Start update 18-Oct-2016
                //Added to see that price is updatable
                if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "ExchangePrice")
                    updateExchangeProductPrice(e.RowIndex);
                //End update 18-Oct-2016
                if (dgvExchangeProducts.Columns[e.ColumnIndex].Name == "ExchangeProductRemarks")
                {
                    string TrxRemarks = "";
                    GenericDataEntry trxRemarks = new GenericDataEntry(1);
                    trxRemarks.Text = MessageUtils.getMessage(201);
                    trxRemarks.DataEntryObjects[0].mandatory = true;
                    trxRemarks.DataEntryObjects[0].label = MessageUtils.getMessage("Remarks");
                    if (trxRemarks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        TrxRemarks = trxRemarks.DataEntryObjects[0].data;
                        SalesReturnTrx.TrxLines[e.RowIndex].Remarks = TrxRemarks;
                        dgvExchangeProducts.Rows[e.RowIndex].Cells["ExchangeProductRemarks"].Value = TrxRemarks;
                    }
                    else
                    {
                        log.Info("Ends-CreateProduct() as Transaction Line Remarks dialog was cancelled");
                        return;
                    }
                }
                refreshdgvExchangeProducts();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-dgvExchangeProducts_CellClick()");
        }

        private void applyDiscountsToReturnProduct(Transaction OriginalTrx, Transaction SalesReturnTrx, ref string message)
        {
            log.Debug("Starts-applyDiscountsToReturnProduct()");
            try
            {
                foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                {
                    if (tl.quantity > 0)
                        tl.LineValid = false;
                }
                foreach (Transaction.TransactionLine tl in OriginalTrx.TrxLines)
                {
                    foreach (Transaction.TransactionLine tlc in SalesReturnTrx.TrxLines)
                    {
                        if (tlc.OriginalLineID == tl.DBLineId)
                        {
                            double Pre_TaxAmount = 0;
                            double Tax_Amount = 0;
                            message = "";
                            double preTaxLineAmount = tl.LineAmount = tl.Price * (double)tl.quantity;
                            Pre_TaxAmount += preTaxLineAmount;
                            tl.tax_amount = preTaxLineAmount * tl.tax_percentage / 100.0;
                            Tax_Amount += tl.tax_amount;
                            tl.LineAmount += tl.tax_amount;
                            tl.Discount_Percentage = 0;

                            if (tlc.TransactionDiscountsDTOList != null)
                            {
                                foreach (var transactionDiscountsDTO in tlc.TransactionDiscountsDTOList)
                                {
                                    if (transactionDiscountsDTO.TransactionDiscountId != -1)
                                    {
                                        TransactionDiscountsBL transactionDiscountsBL = new TransactionDiscountsBL(Utilities.ExecutionContext, transactionDiscountsDTO);
                                        transactionDiscountsBL.Delete();
                                    }
                                }
                            }
                            if (tl.TransactionDiscountsDTOList != null &&
                                tl.TransactionDiscountsDTOList.Count > 0)
                            {
                                if (tlc.TransactionDiscountsDTOList == null)
                                {
                                    tlc.TransactionDiscountsDTOList = new List<TransactionDiscountsDTO>();
                                }
                                foreach (var transactionDiscountsDTO in tl.TransactionDiscountsDTOList)
                                {
                                    TransactionDiscountsDTO reversedTransactionDiscountsDTO = new TransactionDiscountsDTO();
                                    reversedTransactionDiscountsDTO.ApprovedBy = transactionDiscountsDTO.ApprovedBy;
                                    reversedTransactionDiscountsDTO.Remarks = transactionDiscountsDTO.Remarks;
                                    reversedTransactionDiscountsDTO.DiscountPercentage = transactionDiscountsDTO.DiscountPercentage;
                                    reversedTransactionDiscountsDTO.DiscountAmount = -1 * transactionDiscountsDTO.DiscountAmount;
                                    reversedTransactionDiscountsDTO.TransactionId = SalesReturnTrx.Trx_id;
                                    reversedTransactionDiscountsDTO.LineId = tlc.TransactionLineDTO.LineId;
                                    reversedTransactionDiscountsDTO.DiscountId = transactionDiscountsDTO.DiscountId;
                                    tlc.TransactionDiscountsDTOList.Add(reversedTransactionDiscountsDTO);
                                }
                            }
                        }
                    }
                }

                foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                {
                    //Updated condition to see that only the line that is not cancelled is made valid 18-Oct-2016
                    if (tl.quantity > 0 && tl.CancelledLine == false)
                        tl.LineValid = true;
                }
                SalesReturnTrx.updateAmounts(false);
                log.Debug("End-applyDiscountsToReturnProduct()");
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                message = ex.Message;
                log.Debug("End-applyDiscountsToReturnProduct()");
                return;
            }
        }

        private void updateExchangeProductSelection(int RowIndex)
        {
            try
            {
                log.Debug("Starts-updateExchangeProductQuantity(RowIndex)");
                this.Cursor = Cursors.WaitCursor;
                displayMessageLine("");
                if (RowIndex < 0 || SalesReturnTrx == null)
                {
                    log.Info("Ends-updateExchangeProductQuantity(RowIndex) as RowIndex < 0 || NewTrx == null");
                    return;
                }

                if (dgvExchangeProducts["ExchangePdtLine", RowIndex].Value == null)
                {
                    log.Info("Ends-updateExchangeProductQuantity(RowIndex) as LineId == null");
                    return;
                }

                int lineId = (int)dgvExchangeProducts["ExchangePdtLine", RowIndex].Value;

                log.Info("updateExchangeProductQuantity(RowIndex) - Quantity");//Added for logger function on 08-Mar-2016
                decimal savQty = Convert.ToDecimal(dgvExchangeProducts["ExchangeQuantity", RowIndex].Value);
                if (savQty < 1)
                {
                    log.Info("Ends-updateExchangeReturnProductQuantity(RowIndex) - Quantity is less than 0");//Added for logger function on 08-Mar-2016
                    return;
                }

                int LineId = (int)dgvExchangeProducts["ExchangePdtLine", RowIndex].Value;
                int productId = (int)dgvExchangeProducts["ExchangeProductID", RowIndex].Value;

                DataRow ProductDetails = SalesReturnTrx.getProductDetails(productId);

                double price = 0;
                //Added 19-Oct-2016
                double selectedPrice = Convert.ToDouble(dgvExchangeProducts["ExchangePrice", RowIndex].Value);
                if (ProductDetails["TaxInclusivePrice"].ToString() == "Y")
                    price = selectedPrice / (1 + Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                else
                    price = selectedPrice;
                double line_amt = price * Convert.ToDouble(savQty);
                double taxamt = (line_amt * Convert.ToDouble(ProductDetails["tax_percentage"]) / 100);
                bool retVal = true;
                string message = "";

                if (savQty < 0)
                {
                    return;
                }

                if (dgvExchangeProducts["SelectProduct", RowIndex].Value.ToString() == "N")
                {
                    int i = 0;
                    foreach (Transaction.TransactionLine tl in SalesReturnTrx.TrxLines)
                    {
                        if (tl.ProductID == productId && tl.quantity > 0)
                        {
                            SalesReturnTrx.TrxLines[i].CancelledLine = true; //Added 21-Oct-2016
                            SalesReturnTrx.cancelLine(i);
                            SalesReturnTrx.updateAmounts();
                        }
                        i = i + 1;
                    }
                }
                else if (dgvExchangeProducts["SelectProduct", RowIndex].Value.ToString() == "Y")
                {
                    for (int i = 0; i < savQty; i++)
                    {
                        //Updated to see that user price is passed instead of product price 20-Oct-2016
                        if (0 != SalesReturnTrx.createTransactionLine(null, productId, selectedPrice, 1, ref message))
                        {
                            retVal = false;
                        }

                        if (message != "")
                        {
                            displayMessageLine(message);
                            log.Error("updateExchangeReturnProductQuantity() error: " + message);//Added for logger function on 08-Mar-2016
                        }
                        else
                        {
                            SalesReturnTrx.updateAmounts();
                            log.Info("updateExchangeReturnProductQuantity() - Quantity updated");//Added for logger function on 08-Mar-2016
                        }
                        if (!retVal)
                            break;

                        //Start update 20-Oct-2016
                        //Manager approval is required in case price is different from product price
                        if (Convert.ToDouble(ProductDetails["Price"]) != selectedPrice)
                            managerApprovalRequired = true;
                        //End update 20-Oct-2016
                    }
                }

                SalesReturnTrx.updateAmounts();
                this.Cursor = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                displayMessageLine(ex.Message);
                log.Info("updateReturnProductQuantity() -" + ex.Message);
                return;
            }
        }

        private void dgvExchangeProducts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.Debug("Starts-dgvExchangeProducts_CellMouseClick()");
            displayMessageLine("");
            try
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    log.Info("Ends-dgvExchangeProducts_CellMouseClick() as e.ColumnIndex < 0 || e.RowIndex < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
                if (SalesReturnTrx == null)
                {
                    log.Info("Ends-dgvExchangeProducts_CellMouseClick() as NewTrx == null");//Added for logger function on 08-Mar-2016
                    return;
                }

                dgvExchangeProducts.CurrentCell = dgvExchangeProducts[e.ColumnIndex, e.RowIndex];
                int line = Convert.ToInt32(dgvExchangeProducts["ExchangePdtLine", dgvExchangeProducts.CurrentRow.Index].Value);
                if (line < 0)
                {
                    log.Info("Ends-dgvExchangeProducts_CellMouseClick() as line < 0");//Added for logger function on 08-Mar-2016
                    return;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
        }

        void refreshdgvTransaction()
        {
            dgvTransaction.Columns["Date"].DefaultCellStyle = Utilities.gridViewDateTimeCellStyle();
            dgvTransaction.Columns["trxAmount"].DefaultCellStyle
                = dgvTransaction.Columns["trxTax"].DefaultCellStyle
                = dgvTransaction.Columns["Net_amount"].DefaultCellStyle
                = dgvTransaction.Columns["trxdiscount_amount"].DefaultCellStyle
                = Utilities.gridViewAmountCellStyle();
            dgvTransaction.BackgroundColor = Color.White;
            dgvTransaction.Refresh();
            dgvTransaction.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransaction.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
        }

        void refreshdgvProductDetails()
        {
            try
            {
                dgvProductDetails.Columns["quantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvProductDetails.Columns["Amount"].DefaultCellStyle
                            = dgvProductDetails.Columns["tax"].DefaultCellStyle
                            = dgvProductDetails.Columns["discount_amount"].DefaultCellStyle
                            = dgvProductDetails.Columns["Price"].DefaultCellStyle
                            = dgvProductDetails.Columns["last_sale_price"].DefaultCellStyle
                            = Utilities.gridViewAmountCellStyle();
                dgvProductDetails.BackgroundColor = Color.White;
                dgvProductDetails.Refresh();
                chkSelect.DisplayIndex = 0;

                dgvProductDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvProductDetails.DefaultCellStyle.SelectionBackColor = Color.SteelBlue;
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }

        void refreshdgvReturnProducts()
        {
            try
            {
                dgvReturnProducts.Columns["Qty"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvReturnProducts.Columns["ReturnPrice"].DefaultCellStyle
                            = dgvReturnProducts.Columns["ReturnAmount"].DefaultCellStyle
                            = dgvReturnProducts.Columns["ReturnTax"].DefaultCellStyle
                            = dgvReturnProducts.Columns["DiscAmount"].DefaultCellStyle
                            = dgvReturnProducts.Columns["LastSalePrice"].DefaultCellStyle
                            = Utilities.gridViewAmountCellStyle();
                dgvReturnProducts.BackgroundColor = Color.White;
                dgvReturnProducts.Refresh();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }

        void refreshdgvExchangeReturnProducts()
        {
            try
            {
                dgvExchangeReturnProducts.Columns["ReturnQuantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvExchangeReturnProducts.Columns["ReturnPdtPrice"].DefaultCellStyle
                            = dgvExchangeReturnProducts.Columns["ReturnAmt"].DefaultCellStyle
                            = dgvExchangeReturnProducts.Columns["ReturnTaxAmt"].DefaultCellStyle
                            = dgvExchangeReturnProducts.Columns["discountAmt"].DefaultCellStyle
                            = dgvExchangeReturnProducts.Columns["SalePrice"].DefaultCellStyle
                            = Utilities.gridViewAmountCellStyle();
                dgvExchangeReturnProducts.BackgroundColor = Color.White;
                dgvExchangeReturnProducts.Refresh();
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }

        void refreshdgvExchangeProducts()
        {
            try
            {
                dgvExchangeProducts.Columns["ExchangeQuantity"].DefaultCellStyle = Utilities.gridViewNumericCellStyle();
                dgvExchangeProducts.Columns["ExchangePrice"].DefaultCellStyle
                            = dgvExchangeProducts.Columns["ExchangeAmount"].DefaultCellStyle
                            = dgvExchangeProducts.Columns["ExchangeTax"].DefaultCellStyle
                            = dgvExchangeProducts.Columns["ExchangeLastSalePrice"].DefaultCellStyle
                            = Utilities.gridViewAmountCellStyle();
                dgvExchangeProducts.BackgroundColor = Color.White;
                dgvExchangeProducts.Refresh();

                foreach (DataGridViewColumn dc in dgvExchangeProducts.Columns)
                {
                    dc.ReadOnly = true;
                }
                dgvExchangeProducts.Columns["SelectProduct"].ReadOnly = false;
                dgvExchangeProducts.Columns["ExchangeQuantity"].ReadOnly = false;
                dgvExchangeProducts.Columns["ExchangePrice"].ReadOnly = false; //Added to enable price editing 18-Oct-2016 

                //Start update 20-Oct-2016
                //Added to see that only Qty and price columns have a different backcolor
                foreach (DataGridViewRow dr in dgvExchangeProducts.Rows)
                {
                    dr.Cells["ExchangeQuantity"].Style.BackColor = Color.PowderBlue;
                    dr.Cells["ExchangeQuantity"].Style.SelectionBackColor = Color.PowderBlue;
                    dr.Cells["ExchangePrice"].Style.BackColor = Color.PowderBlue;
                    dr.Cells["ExchangePrice"].Style.SelectionBackColor = Color.PowderBlue;
                }
                //End update 20-Oct-2016
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
                this.Cursor = Cursors.Default;
                return;
            }
        }

        bool SaveReturnExchangeTransaction(int OriginalTrxID, string SalesReturnType, Transaction SalesReturnTrx, ref string message, ref string payMode)
        {
            try
            {
                App.machineUserContext = Utilities.ExecutionContext;
                App.EnsureApplicationResources();
                using (SqlConnection cnn = Utilities.createConnection())
                {
                    SqlCommand cmd = new SqlCommand("", cnn);
                    SqlTransaction sqlTrx = cnn.BeginTransaction();
                    SalesReturnTrx.PaymentReference = SalesReturnType;
                    if (0 != SalesReturnTrx.SaveOrder(ref message, sqlTrx))
                    {
                        displayMessageLine(message);
                        sqlTrx.Rollback();
                        return false;
                    }
                    List<TransactionPaymentsDTO> originalTrxPaymentDTOList = getOriginalTrxPaymentDetails(OriginalTrxID);
                    if (originalTrxPaymentDTOList != null)
                    {
                        if (OriginalTrxID == -1 || originalTrxPaymentDTOList.Count > 1 || SalesReturnType == "Exchange")
                        {
                            decimal roundOffCashValue = (decimal)POSStatic.CommonFuncs.RoundOff(SalesReturnTrx.Net_Transaction_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                            decimal roundOffValue = (decimal)SalesReturnTrx.Net_Transaction_Amount - roundOffCashValue;
                            decimal cashAmount = roundOffValue != 0 ? roundOffCashValue : (decimal)SalesReturnTrx.Net_Transaction_Amount;
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, (double)cashAmount,
                                                                                                  "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                  Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                                trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                                SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                            }
                            payMode = "CASH";
                            if (roundOffValue != 0)
                            {
                                PaymentModeList roundOffPaymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> roundoffSearchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                roundoffSearchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                roundoffSearchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISROUNDOFF, "Y"));
                                List<PaymentModeDTO> roundoffPaymentModeDTOList = roundOffPaymentModeListBL.GetPaymentModeList(roundoffSearchParameters);
                                if (roundoffPaymentModeDTOList != null)
                                {
                                    TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, roundoffPaymentModeDTOList[0].PaymentModeId, (double)roundOffValue,
                                                                                                      "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                      Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                    trxPaymentDTO.paymentModeDTO = roundoffPaymentModeDTOList[0];

                                    trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                                    SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                }
                            }
                        }
                        else if (originalTrxPaymentDTOList.Count == 1)
                        {
                            if (originalTrxPaymentDTOList[0].paymentModeDTO.IsCreditCard)
                            {
                                TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                                originalTrxPaymentDTOList[0].Amount = SalesReturnTrx.Net_Transaction_Amount;
                                originalTrxPaymentDTOList[0].PaymentId = -1;
                                originalTrxPaymentDTOList[0].TransactionId = SalesReturnTrx.Trx_id;
                                if (originalTrxPaymentDTOList[0].GatewayPaymentProcessed == false)
                                {
                                    TransactionPaymentsBL trxPaymentBL = new TransactionPaymentsBL(Utilities.ExecutionContext, originalTrxPaymentDTOList[0]);
                                    trxPaymentBL.Save(sqlTrx);
                                    SalesReturnTrx.TransactionPaymentsDTOList.Add(originalTrxPaymentDTOList[0]);
                                    if (!SalesReturnTrx.refundCreditCardPayments(OriginalTrxID, SalesReturnTrx.Trx_id, sqlTrx, ref message))
                                    {
                                        displayMessageLine(message);
                                        sqlTrx.Rollback();
                                        return false;
                                    }
                                }
                                payMode = "CREDITCARD";
                            }
                            else
                            {
                                payMode = "CASH";
                                decimal roundOffCashValue = (decimal)POSStatic.CommonFuncs.RoundOff(SalesReturnTrx.Net_Transaction_Amount, Utilities.ParafaitEnv.RoundOffAmountTo, Utilities.ParafaitEnv.RoundingPrecision, Utilities.ParafaitEnv.RoundingType);
                                decimal roundOffValue = (decimal)SalesReturnTrx.Net_Transaction_Amount - roundOffCashValue;
                                decimal cashAmount = roundOffValue != 0 ? roundOffCashValue : (decimal)SalesReturnTrx.Net_Transaction_Amount;
                                PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                                List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                                if (paymentModeDTOList != null)
                                {
                                    TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, (double)cashAmount,
                                                                                                      "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                      Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                    trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                                    trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                                    SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                }
                                displayMessageLine(MessageUtils.getMessage(30) + " " + (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                                if (roundOffValue != 0)
                                {
                                    PaymentModeList roundOffPaymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                                    List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> roundoffSearchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                                    roundoffSearchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                                    roundoffSearchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISROUNDOFF, "Y"));
                                    List<PaymentModeDTO> roundoffPaymentModeDTOList = roundOffPaymentModeListBL.GetPaymentModeList(roundoffSearchParameters);
                                    if (roundoffPaymentModeDTOList != null)
                                    {
                                        TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, roundoffPaymentModeDTOList[0].PaymentModeId, (double)roundOffValue,
                                                                                                          "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                          Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                        trxPaymentDTO.paymentModeDTO = roundoffPaymentModeDTOList[0];

                                        trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                                        SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                                    }
                                }
                            }
                        }
                        else
                        {
                            payMode = "CASH";
                            PaymentModeList paymentModeListBL = new PaymentModeList(Utilities.ExecutionContext);
                            List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            searchParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISCASH, "Y"));
                            List<PaymentModeDTO> paymentModeDTOList = paymentModeListBL.GetPaymentModeList(searchParameters);
                            if (paymentModeDTOList != null)
                            {
                                TransactionPaymentsDTO trxPaymentDTO = new TransactionPaymentsDTO(-1, -1, paymentModeDTOList[0].PaymentModeId, SalesReturnTrx.Net_Transaction_Amount,
                                                                                                  "", "", "", "", "", -1, "", -1, 0, -1, "", "", false, -1, -1, "", Utilities.getServerTime(),
                                                                                                  Utilities.ParafaitEnv.LoginID, -1, null, 0, -1, Utilities.ParafaitEnv.POSMachine, -1, "", null);
                                trxPaymentDTO.paymentModeDTO = paymentModeDTOList[0];

                                trxPaymentDTO = TableAttributesUIHelper.GetEnabledAttributeDataForPaymentMode(Utilities.ExecutionContext, trxPaymentDTO);

                                SalesReturnTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                            }
                            displayMessageLine(MessageUtils.getMessage(30) + " " + (-1 * SalesReturnTrx.Net_Transaction_Amount).ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        }
                    }
                    message = "";

                    if (!SalesReturnTrx.CreatePaymentInfo(sqlTrx, ref message))
                    {
                        displayMessageLine(message);
                        sqlTrx.Rollback();
                        return false;
                    }
                    message = "";
                    if (!SalesReturnTrx.CompleteTransaction(sqlTrx, ref message))
                    {
                        displayMessageLine(message);
                        sqlTrx.Rollback();
                        return false;
                    }

                    sqlTrx.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        FiscalPrinter FiscalPrinter;
        void printTransaction(int TransactionId)
        {
            try
            {
                FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);

                string Message = "";
                if (ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "Y")
                {
                    if (POSStatic.USE_FISCAL_PRINTER != "Y")
                        PrintSpecificTransaction(TransactionId, false);
                    else
                    {
                        if (fiscalPrinter.PrintReceipt(TransactionId, ref Message) == false)
                        {
                            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                // Non fiscal type for type 'D' taxed products
                                PrintSpecificTransaction(TransactionId, false);
                            }
                        }
                        endPrintAction();
                    }

                    TransactionId = -1;
                }
                else if (ParafaitEnv.TRX_AUTO_PRINT_AFTER_SAVE == "A")
                {
                    if (POSStatic.USE_FISCAL_PRINTER == "Y")
                    {
                        if (fiscalPrinter.PrintReceipt(TransactionId, ref Message) == false)
                        {
                            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                // Non fiscal type for type 'D' taxed products
                                PrintSpecificTransaction(TransactionId, false);
                            }
                        }
                        endPrintAction();
                    }
                    else
                    {
                        if (POSUtils.ParafaitMessageBox(MessageUtils.getMessage(205), ParafaitEnv.SalesReturnType + " Print", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            PrintSpecificTransaction(TransactionId, false);
                    }
                    TransactionId = -1;
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
        }

        void PrintSpecificTransaction(int TrxId, bool rePrint)
        {
            log.Debug("Starts-PrintSpecificTransaction(" + TrxId + ",rePrint)");
            try
            {
                Utilities.ParafaitEnv.ClearSpecialPricing();

                List<int> trxIdList = new List<int>();

                trxIdList.Add(TrxId);

                string message = "";

                PrintMultipleTransactions printMultipleTransactions = new PrintMultipleTransactions(Utilities);
                if (!printMultipleTransactions.Print(trxIdList, rePrint, ref message))
                {
                    displayMessageLine(message);
                    log.Warn("PrintSpecificTransaction(" + TrxId + ",rePrint) - Unable to Print Transaction error: " + message);//Added for logger function on 08-Mar-2016
                }
            }
            catch (Exception ex)
            {
                displayMessageLine(ex.Message);
            }
            log.Debug("Ends-PrintSpecificTransaction(" + TrxId + ",rePrint)");//Added for logger function on 08-Mar-2016
        }

        void endPrintAction()
        {
            log.Debug("Starts-endPrintAction()");//Added for logger function on 08-Mar-2016
            if (ParafaitEnv.CLEAR_TRX_AFTER_PRINT == "Y" && NewTrx == null)
                cleanUpOnNullTrx(); // clear out the Trx display so that it is not reprinted

            log.Debug("Ends-endPrintAction()");//Added for logger function on 08-Mar-2016
        }

        void cleanUpOnNullTrx()
        {
            log.Debug("Starts-cleanUpOnNullTrx()");//Added for logger function on 08-Mar-2016
            Utilities.ParafaitEnv.SalesReturnType = "";
            managerApprovalRequired = false;
            SalesReturnTrx = null;
            NewTrx = null;
            SelectedTransactionId = -1;
            //Return and exchange tabs should not be visible on form load
            tcReturnExchange.TabPages.Remove(tpReturn);
            tcReturnExchange.TabPages.Remove(tpExchange);
            txtTrxID.Text = "";
            txtFromTrxDate.Text = "";
            txtToTrxDate.Text = "";
            dgvTransaction.Rows.Clear();
            gbPLSearchCriteria.Enabled = true;
            btnClearProductSearch.PerformClick();
            btnInitiateReturn.Enabled = true;
            btnInitiateExchange.Enabled = true;
            dgvProductDetails.Rows.Clear();
            dgvReturnProducts.Rows.Clear();
            dgvExchangeReturnProducts.Rows.Clear();
            dgvExchangeProducts.Rows.Clear();
            btnConfirmExchange.Enabled = true;
            btnCancelExchange.Enabled = true;
            btnConfirmReturn.Enabled = true;
            btnCancelReturn.Enabled = true;
            btnClearExchangeProductSearch.PerformClick();
            tcReturnExchange.SelectTab(tpTrxLookup);
            txtTrxID.Focus();
            displayMessageLine(MessageUtils.getMessage(234));
            log.Debug("Ends-cleanUpOnNullTrx()");
        }

        private void frmSalesReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Common.Devices.PrimaryBarcodeScanner != null)
                Common.Devices.PrimaryBarcodeScanner.UnRegister();
        }
    }
}
