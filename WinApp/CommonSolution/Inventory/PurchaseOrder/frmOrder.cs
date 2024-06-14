/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
*.00        23-Mar-2016            Soumya                      Updated queries to fix issues
*                                                              due to queries related to 
*                                                              search by barcode.
*
********************************************************************************************
*.00        20-May-2016            Soumya                      Updated queries and code to
*                                                              allow quantity update in case of 
*                                                              inprogress orders.
*********************************************************************************************
*.00         17-Jun-2016            Soumya                     Updated to fix order printing
*                                                              and email issue.
*********************************************************************************************
*1.00        08-Feb-2019            Archana                    Redemption gift search and
*                                                              inventory UI changees
*                                                              
*********************************************************************************************
* 2.60       01-April-2019          Girish Kundar              Adding MultipleTax Structure for Purchase Order 
* 2.70       28-Jun-2019            Archana                    Modified: Inventory stock and vendor search in PO
*                                                              and receive screen change 
* 2.70.2       20-Aug-2019            Archana                    Form is moved to Inventory project
*                                                              Auto PO changes   
*2.70.2        15-Nov-2019            Archana                    Modified to add stock link when form opens in management studio
*2.70.2        18-Dec-2019            Jinto Thomas            Added parameter execution context for userrolrbl declaration with userid 
*2.70.2        28-Dec-2019            Deeksha                 Inventory Next -Rel Enhancements changes.
* 2.80        18-May-2020             Laster Menezes          Exception handling while printing InventoryPurchaseOrderReceipt
* 2.90.0      19-Aug-2020             Deeksha                 Date validation not required for Auto PO
*2.100.0      27-Jul-2020             Deeksha                 Modified :Added UOM drop down field to change related UOM's
*2.110.0      29-Dec-2020             Mushahid Faizan         Modified :Added IsActive field to PurchaseOrderDTO.
*2.120        03-May-2021             Laster Menezes          Modified GetInventoryReport method to use receipt framework of reports for InventoryPurchaseOrderReceipt generation
*2.120.1      19-Jul-2021             Deeksha                 Modified :Inventory PO screen resolution Issue Fix
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing.Printing;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Vendor;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.Inventory
{
    public partial class frmOrder : Form
    {
        frmInventoryNotes inventoryNotesUI;
        PurchaseOrderDTO purchaseOrderDTO;
        PurchaseOrderDataHandler purchaseOrderDataHandler;
        PurchaseOrderLineDTO purchaseOrderLineDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        List<TaxDTO> taxDTOList;
        DataTable DT_order = new DataTable();
        DataTable DT_search = new DataTable();
        int dgvselectedindex;
        bool fireCellValueChange = true;
        DataGridView printdgv = new DataGridView();
        string scannedBarcode = "";
        string orderStatus = "";
        DateTime toDate;
        DateTime fromDate;
        Utilities utilities;
        int roundingPrecision = 5;
        bool isAutoPOMode = false;
        private int orderId = -1;
        DataGridViewCellStyle inventorycostCellStyle;
        private const string RegularPurchaseOrderType = "Regular Purchase Order";
        private const string ContractPurchaseOrderType = "Contract Purchase Order";
        private const string AutoPOText = " Auto";
        const string FinalStatus = "F";
        decimal cost;
        decimal qty;
        string reportFileName;
        double ticketCost = 0;

        public frmOrder(Utilities utilities = null, bool autoPOMode = false)
        {
            log.LogMethodEntry(utilities, autoPOMode);
            InitializeComponent();
            initializeVariables();
            txtProductPOSearch.Focus();
            orderStatus = tb_status.Text;
            UserRoles userRoles = new UserRoles(utilities.ExecutionContext, Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.RoleId);
            bool isEnable = userRoles.IsEditable("PO");
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.SetupAccess(gb_order, isEnable);
            btnClose.Enabled = true;
            if (utilities != null)
            {
                this.utilities = utilities;
            }
            else
            {
                this.utilities = new Utilities();
            }
            inventorycostCellStyle = new DataGridViewCellStyle();
            inventorycostCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            inventorycostCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.NUMBER_FORMAT;
            isAutoPOMode = autoPOMode;
            log.LogMethodExit();
        }
        private void PrepareForAutoPOMode()
        {
            log.LogMethodEntry();
            PrepareForNewPO();
            SetDefaultValuesForAutoPOMode();
            log.LogMethodExit();
        }
        private void SetDefaultValuesForAutoPOMode()
        {
            log.LogMethodEntry();
            cmbDocumentType.Text = RegularPurchaseOrderType;
            cmbDocumentType.Enabled = false;
            dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.CustomFormat = "dd-MMM-yyyy";
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.Value = dtpToDate.Value = utilities.getServerTime();
            dtpFromDate.Enabled = dtpToDate.Enabled = false;
            chkIsCreditPO.Enabled = false;
            log.LogMethodExit();
        }

        private void NewPOCreationMode()
        {
            log.LogMethodEntry();
            PrepareForNewPO();
            log.LogMethodExit();

        }
        void populateDocumentType()
        {
            log.LogMethodEntry();
            DataTable DT = new DataTable();
            string condition = "";
            SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
            if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
            {
                condition = " and (site_id = @site_id or @site_id = -1)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
            }
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.LoginID);
            cmd.CommandText = @"select -1 ID, null Name
                                union all
                                select DocumentTypeId ID, Name
                                from inventorydocumenttype 
                                where Name in ('Contract Purchase Order', 'Regular Purchase Order')  " + condition;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();
            cmbDocumentType.DataSource = DT;
            cmbDocumentType.ValueMember = "ID";
            cmbDocumentType.DisplayMember = "Name";
            log.LogMethodExit();
        }
        public void CalculateTotalTaxAmount()
        {
            log.LogMethodEntry();
            if (order_dgv.Rows.Count > 0)
            {
                decimal Totaltax = 0;
                for (int i = 0; i < order_dgv.Rows.Count; i++)
                {
                    if (order_dgv["TaxAmount", i].Value != null && !String.IsNullOrEmpty(order_dgv["TaxAmount", i].Value.ToString()))
                    {
                        Decimal quantity = 0.0M;
                        try
                        {
                            quantity = Convert.ToDecimal(order_dgv["Quantity", i].Value.ToString());
                        }
                        catch (Exception ex) { log.Error(ex); }
                        Totaltax += quantity * Math.Round(Convert.ToDecimal(order_dgv["TaxAmount", i].Value.ToString()), roundingPrecision, MidpointRounding.AwayFromZero);
                    }
                }
                txtTotalTax.Text = Totaltax.ToString();
                txtTotalTax.Text = String.Format("{0:N3}", Totaltax);
                lblViewTotalTax.Enabled = true;
            }
            log.LogMethodExit();

        }
        private void initializeVariables()
        {
            log.LogMethodEntry();
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref order_dgv);
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref searchorder_dgv);
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref Rsearch_dgv);
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.IsCorporate && Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.IsMasterSite)
            {
                lblTosite.Visible = cmbToSite.Visible = true;
                Semnox.Parafait.Inventory.CommonFuncs.LoadToSite(cmbToSite, false);
            }
            else
            {
                lblTosite.Visible = cmbToSite.Visible = false;
            }
            populateDocumentType();
            order_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            searchorder_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Rsearch_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            searchorder_dgv.BackgroundColor = Color.LightSlateGray;
            Rsearch_dgv.BackgroundColor = Color.LightSlateGray;
            searchorder_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateGray;
            Rsearch_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateGray;
            order_dgv.BackgroundColor = Color.White;
            tb_date.Text = System.DateTime.Now.ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.DATETIME_FORMAT);
            tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
            tb_cancelledDate.Text = "";
            cb_vendor.DropDownStyle = ComboBoxStyle.DropDown;
            cmbDocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
            DT_order.Columns.Add("Code");
            DT_order.Columns.Add("Description");
            DT_order.Columns.Add("Quantity");
            DT_order.Columns.Add("RequiredByDate", System.Type.GetType("System.DateTime"));
            DT_order.Columns.Add("Cost");
            DT_order.Columns.Add("UnitLogisticsCost");
            DT_order.Columns.Add("TaxAmount");
            DT_order.Columns.Add("DiscountPercentage");
            DT_order.Columns.Add("SubTotal");
            DT_order.Columns.Add("LineId");
            DT_order.Columns.Add("CancelledDate", System.Type.GetType("System.DateTime"));
            DT_order.Columns.Add("isActive");
            DT_order.Columns.Add("ReceiveQuantity");//Added 20-May-2016
            DT_order.Columns.Add("MarketListItem");
            DT_order.Columns.Add("ProductId");
            DT_order.Columns.Add("RequisitionId");
            DT_order.Columns.Add("RequisitionLineId");
            DT_order.Columns.Add("PriceInTickets");
            DT_order.Columns.Add("taxInclusiveCost");
            DT_order.Columns.Add("taxPercentage");
            DT_order.Columns.Add("TaxCode");
            DT_order.Columns.Add("UOMId");
            lb_orderid.Text = "";
            lblEmail.Text = "";
            log.LogMethodExit();
        }

        private void frm_Order_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateTax();
            PopulateVendor();
            cb_vendor.SelectedValue = -1;
            cb_searchvendor.SelectedIndex = -1;
            cb_searchstatus.SelectedIndex = 1;
            POSearch();
            tb_searchorder.Focus();
            order_dgv.Columns["Quantity"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewNumericCellStyle();
            order_dgv.Columns["Quantity"].DefaultCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            order_dgv.Columns["RequiredByDate"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewDateCellStyle();
            order_dgv.Columns["CancelledDate"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewDateCellStyle();
            order_dgv.Columns["Cost"].DefaultCellStyle =
            order_dgv.Columns["UnitLogisticsCost"].DefaultCellStyle =
            order_dgv.Columns["DiscountPercentage"].DefaultCellStyle =
            order_dgv.Columns["SubTotal"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            order_dgv.Columns["ReceiveQuantity"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            SetOrderDGVColumnReadStatus();
            order_dgv.Columns["TaxAmount"].DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Blue, Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font(order_dgv.Font, FontStyle.Underline), Format = CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };
            fireCellValueChange = false; //this is set to false,bcz setlanguage function causes grid events to fire when there are no records.
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setLanguage(this);
            fireCellValueChange = true; //set the variable true after language change is applied
            log.LogVariableState("AutoPOModeVariable", isAutoPOMode);
            if (isAutoPOMode)
            {
                PrepareForAutoPOMode();
            }
            log.LogMethodExit();
        }

        private void PopulateTax()
        {
            log.LogMethodEntry();
            TaxList taxList = new TaxList(executionContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.ACTIVE_FLAG, "1"));
            BindingSource taxBS = new BindingSource();
            taxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);
            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }
            TaxDTO defaultTax = new TaxDTO();
            taxDTOList.Insert(0, defaultTax);
            taxBS.DataSource = taxDTOList;
            TaxCode.DataSource = taxBS;
            TaxCode.ValueMember = "TaxId";
            TaxCode.DisplayMember = "TaxName";
            log.LogMethodExit();
        }
        void PopulateVendor()
        {
            log.LogMethodEntry();
            List<VendorDTO> vendorDTOList;
            VendorList vendorList = new VendorList(executionContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorDTOSearchParams;
            vendorDTOSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));
            BindingSource vendorBS = new BindingSource();
            BindingSource searchVendorBS = new BindingSource();
            vendorDTOList = vendorList.GetAllVendors(vendorDTOSearchParams);
            if (vendorDTOList == null)
            {
                vendorDTOList = new List<VendorDTO>();
            }
            vendorDTOList.Insert(0, new VendorDTO());
            vendorBS.DataSource = vendorDTOList.OrderBy(vendor => vendor.Name);
            searchVendorBS.DataSource = vendorDTOList.OrderBy(ven => ven.Name);
            cb_vendor.DataSource = vendorBS;
            cb_vendor.ValueMember = "VendorId";
            cb_vendor.DisplayMember = "Name";

            cb_searchvendor.DataSource = searchVendorBS;
            cb_searchvendor.ValueMember = "VendorId";
            cb_searchvendor.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void cb_vendor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int vendor_id = -1;

            try
            {
                vendor_id = Convert.ToInt32(cb_vendor.SelectedValue);
            }
            catch (Exception ex) { log.Error(ex); }
            DataTable DT = new DataTable();
            string condition = "";
            SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
            if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
            {
                condition = " and (site_id = @site_id or @site_id = -1)";
                cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
            }
            cmd.CommandText = "select  ContactName, Phone, Address1,Address2,City,State,Country,PostalCode, TaxRegistrationNumber, Email " +
                    " from Vendor " +
                    "where VendorId = @vendor AND IsActive = 'Y' " + condition;
            cmd.Parameters.AddWithValue("@vendor", vendor_id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();

            if (DT.Rows.Count == 0)
            {
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(857), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Vendor Status"));
                cb_vendor.SelectedValue = -1;
                return;
            }
            tb_contact.Text = DT.Rows[0]["ContactName"].ToString();
            tb_phone.Text = DT.Rows[0]["Phone"].ToString();
            lblEmail.Text = DT.Rows[0]["Email"].ToString();
            cb_vendor.Enabled = false;
            cb_vendor.SelectedValue = vendor_id;
            log.LogMethodExit();
        }


        private void frm_Order_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if ((((lb_orderid.Text != "") || (order_dgv.RowCount > 1)) && !isAutoPOMode)
                || (isAutoPOMode && orderId == -1))
            {
                if (MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(865), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Exit Order Form"), MessageBoxButtons.YesNo) == DialogResult.No)
                    e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        //Added barcode search
        private DataTable fetchProductDetails(String prod_code, string description, bool marketListOrder)
        {
            log.LogMethodEntry(prod_code, description, marketListOrder);
            DataTable DT = new DataTable();
            SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
            string condition = "";

            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_PRODUCT_SEARCH_BY_VENDOR").Equals("Y"))
            {
                if (cb_vendor.SelectedIndex == 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(850));
                    return DT;
                }
                condition = " and product.DefaultVendorId = @vendorId";
                cmd.Parameters.AddWithValue("@vendorId", cb_vendor.SelectedValue);
            }
            if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
            {
                condition = " and (Product.site_id = @site_id or @site_id = -1)";
                cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
            }

            if (marketListOrder)
            {
                condition += " and isnull(MarketListItem, 0) = 1 ";
            }
            else
            {
                if (order_dgv["SubTotal", 0].Value != null)
                    condition += " and isnull(MarketListItem, 0) = 0 ";
            }
            //updated query to search barcode in productbarcode table
            //23-Mar-2016
            cmd.CommandText = "select Code, Description, ReorderQuantity, isnull(Marketlistitem, 0) Marketlistitem, " +
                                "case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, " +
                                "null UnitLogisticsCost," +
                                "case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/100 * isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else 0 end as taxAmount, " +
                                "(case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) * ReorderQuantity as SubTotal, " +
                                "(case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) as CPOSubTotal, " +
                                "LowerLimitCost, UpperLimitCost, CostVariancePercentage, product.ProductId, uom, product.UomId,PriceInTickets, taxInclusiveCost, tax_percentage ,tax_name ,Product.PurchaseTaxId as TaxCode " +
                      "from Product left outer join Tax on Tax.tax_id = Product.PurchaseTaxId left outer join uom on uom.uomid = product.uomid " +
                                    "left outer join (select * " +
                                    "from (" +
                                            "select *, row_number() over(partition by productid order by productid) as num " +
                                                                 "from productbarcode " +
                                                                 "where BarCode like '%' + @prod_code + '%' and isactive = 'Y')v " +
                                    "where num = 1)b on Product.productid = b.productid " +
                    "where (Code like @prod_code or ProductName like N'%' + @prod_code + '%' or description like '%' + @desc + '%' or b.BarCode like '%' + @prod_code + '%') AND Product.IsActive = 'Y' and isPurchaseable = 'Y' " + condition;
            //23-Mar-2016
            cmd.Parameters.AddWithValue("@prod_code", prod_code);
            cmd.Parameters.AddWithValue("@desc", description);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();
            log.LogMethodExit(DT);
            return DT;
        }



        private void recalculateTotal()
        {
            log.LogMethodEntry();
            decimal total = 0;
            decimal totalLogisticsCost = 0;
            decimal totalLandedCost = 0;
            decimal totalQuantity = 0;
            decimal salePrice = 0;
            decimal totalRetail = 0;
            decimal markupPercent = 0;
            try
            {
                ticketCost = Convert.ToDouble(Semnox.Parafait.Inventory.CommonFuncs.Utilities.getParafaitDefaults("TICKET_COST"));
            }
            catch
            {
                ticketCost = 0;
            }
            for (int i = 0; i < order_dgv.RowCount - 1; i++)
            {
                decimal subTotal = 0;
                decimal quantity = 0;
                if (order_dgv["ProductId", i].Value != null && order_dgv["ProductId", i].Value != DBNull.Value)
                {
                    SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
                    //cmd.CommandText = @"select pdt.price Saleprice
                    cmd.CommandText = @"select CASE WHEN (p.IsRedeemable = 'Y') THEN 
                                                      CASE WHEN ISNULL(pdt.price,0) = 0 THEN p.PriceInTickets * @ticketCost  ELSE  pdt.price END
	                                               ELSE pdt.price END as Saleprice
                                from product p left outer join products pdt on p.manualproductId = pdt.product_Id
                                where productid = @productid";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@productid", order_dgv["ProductId", i].Value);
                    cmd.Parameters.AddWithValue("@ticketCost", ticketCost);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    DataTable DT1 = new DataTable();
                    da1.Fill(DT1);
                    da1.Dispose();
                    if (DT1.Rows.Count > 0 && DT1.Rows[0]["Saleprice"] != null && DT1.Rows[0]["Saleprice"] != DBNull.Value)
                    {
                        salePrice = Convert.ToDecimal(DT1.Rows[0]["Saleprice"]);
                    }
                }

                if (order_dgv["isActive", i].Value == null || order_dgv["isActive", i].Value == DBNull.Value)
                {
                    if (order_dgv["SubTotal", i].Value != null && order_dgv["SubTotal", i].Value != DBNull.Value)
                    {
                        total += Convert.ToDecimal(order_dgv["SubTotal", i].Value);
                        subTotal = Convert.ToDecimal(order_dgv["SubTotal", i].Value);
                    }
                    if (order_dgv["Quantity", i].Value != null && order_dgv["Quantity", i].Value != DBNull.Value)
                    {
                        quantity = Convert.ToDecimal(order_dgv["Quantity", i].Value);
                        totalQuantity += quantity;
                    }
                    if (order_dgv["UnitLogisticsCost", i].Value != null && order_dgv["UnitLogisticsCost", i].Value != DBNull.Value)
                    {
                        totalLogisticsCost += (quantity * Convert.ToDecimal(order_dgv["UnitLogisticsCost", i].Value));
                        totalLandedCost += (subTotal + (quantity * Convert.ToDecimal(order_dgv["UnitLogisticsCost", i].Value)));
                    }
                    else
                    {
                        totalLandedCost += subTotal;
                    }
                    totalRetail += salePrice * quantity;
                }

                if (order_dgv["isActive", i].Value != null && order_dgv["isActive", i].Value != DBNull.Value)
                {
                    if (order_dgv["isActive", i].Value.ToString() == "Y")
                    {
                        if (order_dgv["SubTotal", i].Value != null && order_dgv["SubTotal", i].Value != DBNull.Value)
                        {
                            total += Convert.ToDecimal(order_dgv["SubTotal", i].Value);
                            subTotal = Convert.ToDecimal(order_dgv["SubTotal", i].Value);
                        }
                        if (order_dgv["Quantity", i].Value != null && order_dgv["Quantity", i].Value != DBNull.Value)
                        {
                            quantity = Convert.ToDecimal(order_dgv["Quantity", i].Value);
                            totalQuantity += quantity;
                        }
                        if (order_dgv["UnitLogisticsCost", i].Value != null && order_dgv["UnitLogisticsCost", i].Value != DBNull.Value)
                        {
                            totalLogisticsCost += (quantity * Convert.ToDecimal(order_dgv["UnitLogisticsCost", i].Value));
                            totalLandedCost += (subTotal + (quantity * Convert.ToDecimal(order_dgv["UnitLogisticsCost", i].Value)));
                        }
                        else
                        {
                            totalLandedCost += subTotal;
                        }
                        totalRetail += salePrice * quantity;
                    }
                }
            }
            tb_total.Text = String.Format("{0:N2}", total);
            tb_TotalLandedCost.Text = String.Format("{0:N2}", totalLandedCost);
            tb_totalLogisticsCost.Text = String.Format("{0:N2}", totalLogisticsCost);
            tb_TotalQuantity.Text = String.Format("{0:N2}", totalQuantity);
            tb_TotalRetail.Text = String.Format("{0:N2}", totalRetail);
            decimal totalCost = (tb_total.Text == "" ? 0 : Convert.ToDecimal(tb_total.Text));
            try { markupPercent = (totalRetail == 0 ? 0 : (totalRetail - totalCost) / totalCost) * 100; }
            catch (Exception ex) { markupPercent = 0; log.Error(ex); }
            tb_MarkupPercent.Text = String.Format("{0:N2}", markupPercent);
            CalculateTotalTaxAmount();
            log.LogMethodExit();
        }

        bool marketListItemExists()
        {
            log.LogMethodEntry();
            bool returnVal = false;
            try
            {
                for (int i = 0; i < order_dgv.RowCount - 1; i++)
                {
                    if (order_dgv["SubTotal", i].Value != null && order_dgv["SubTotal", i].Value != DBNull.Value)
                    {
                        if (order_dgv["isActive", i].Value != null && order_dgv["isActive", i].Value != DBNull.Value)
                        {
                            if (order_dgv["isActive", i].Value.ToString() == "Y")
                            {
                                if (order_dgv["MarketListItem", i].Value != null && order_dgv["MarketListItem", i].Value != DBNull.Value)
                                {
                                    if (Convert.ToBoolean(order_dgv["MarketListItem", i].Value))
                                    {
                                        returnVal = true;
                                        break;
                                    }
                                    else
                                    {
                                        returnVal = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    returnVal = false;
                                    break;
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (order_dgv["isActive", i].Value == null || order_dgv["isActive", i].Value == DBNull.Value)
                        {
                            if (order_dgv["MarketListItem", i].Value != null && order_dgv["MarketListItem", i].Value != DBNull.Value)
                            {
                                if (Convert.ToBoolean(order_dgv["MarketListItem", i].Value))
                                {
                                    returnVal = true;
                                    break;
                                }
                                else
                                {
                                    returnVal = false;
                                    break;
                                }
                            }
                            else
                            {
                                returnVal = false;
                                break;
                            }
                        }
                    }
                }
                log.LogMethodExit(returnVal);
                return returnVal;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
                log.LogMethodExit(returnVal);
                return returnVal;
            }
        }


        private void order_dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool marketListOrder = false;
            try
            {
                marketListOrder = marketListItemExists();
                int taxId = -1;
                if (order_dgv.Columns[e.ColumnIndex].Name == "SubTotal")
                {
                    recalculateTotal();
                }
                //Start update 22-Feb-2016
                if (order_dgv.Columns[e.ColumnIndex].Name == "UnitLogisticsCost")
                {
                    recalculateTotal();
                }
                //End update 22-Feb-2016
                if (order_dgv.Columns[e.ColumnIndex].Name == "Code")
                {
                    if (fireCellValueChange)
                    {
                        DataTable DT = new DataTable();
                        DT = fetchProductDetails(order_dgv[e.ColumnIndex, e.RowIndex].Value.ToString() + "%", order_dgv[e.ColumnIndex, e.RowIndex].Value.ToString(), marketListOrder);
                        order_dgv.Columns[1].ReadOnly = true;
                        if (DT.Rows.Count < 1)
                        {
                            MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(846), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Find Products"));
                            dgvselectedindex = e.RowIndex;
                            order_dgv.Rows.Remove(order_dgv.Rows[dgvselectedindex]);

                        }
                        else if (DT.Rows.Count == 1)
                        {
                            dgvselectedindex = e.RowIndex;
                            fireCellValueChange = false;
                            order_dgv["Code", dgvselectedindex].Value = DT.Rows[0]["Code"];
                            fireCellValueChange = true;
                            order_dgv["Description", dgvselectedindex].Value = DT.Rows[0]["Description"];
                            //order_dgv["cmbUOM", e.RowIndex].Value = DT.Rows[0]["cmbUOM"];

                            order_dgv["cmbUOM", dgvselectedindex].Value = DT.Rows[0]["UOMId"];
                            int uomId = Convert.ToInt32(order_dgv["cmbUOM", dgvselectedindex].Value);
                            CommonFuncs.GetUOMComboboxForSelectedRows(order_dgv, dgvselectedindex, uomId);
                            taxId = DT.Rows[0]["TaxCode"] == DBNull.Value ? -1 : Convert.ToInt32(DT.Rows[0]["TaxCode"]);
                            if (taxId != -1 && taxDTOList.Exists(x => x.TaxId == taxId) == false)
                            {
                                // Tax is inactive Please update the tax in the Product set up
                                MessageBox.Show(utilities.MessageUtils.getMessage(2461), "Validation Error");
                                order_dgv["TaxCode", dgvselectedindex].Value = -1;
                            }
                            else
                            {
                                order_dgv["TaxCode", dgvselectedindex].Value = (DT.Rows[0]["TaxCode"] == DBNull.Value ? -1 : DT.Rows[0]["TaxCode"]);
                            }
                            if (cmbDocumentType.Text == ContractPurchaseOrderType)
                            {
                                order_dgv["Quantity", dgvselectedindex].Value = 1;
                                order_dgv["Quantity", dgvselectedindex].ReadOnly = true;
                                order_dgv["SubTotal", dgvselectedindex].Value = DT.Rows[0]["CPOSubTotal"];
                            }
                            else
                            {
                                order_dgv["Quantity", dgvselectedindex].Value = DT.Rows[0]["ReorderQuantity"];
                                order_dgv["SubTotal", dgvselectedindex].Value = DT.Rows[0]["SubTotal"];
                            }
                            order_dgv["Cost", dgvselectedindex].Value = DT.Rows[0]["Cost"];
                            order_dgv["UnitLogisticsCost", dgvselectedindex].Value = DT.Rows[0]["UnitLogisticsCost"];
                            order_dgv["TaxAmount", dgvselectedindex].Value = DT.Rows[0]["TaxAmount"];
                            order_dgv["LowerLimitCost", dgvselectedindex].Value = DT.Rows[0]["LowerLimitCost"];
                            order_dgv["UpperLimitCost", dgvselectedindex].Value = DT.Rows[0]["UpperLimitCost"];
                            order_dgv["CostVariancePercent", dgvselectedindex].Value = DT.Rows[0]["CostVariancePercentage"];
                            order_dgv["OrigPrice", dgvselectedindex].Value = DT.Rows[0]["Cost"];
                            order_dgv["MarketListItem", dgvselectedindex].Value = DT.Rows[0]["MarketListItem"];
                            order_dgv["ProductId", dgvselectedindex].Value = DT.Rows[0]["ProductId"];
                            order_dgv["PriceInTickets", dgvselectedindex].Value = DT.Rows[0]["PriceInTickets"];
                            InventoryList inventoryList = new InventoryList();
                            order_dgv["stockLink", dgvselectedindex].Value = inventoryList.GetProductStockQuantity(Convert.ToInt32(DT.Rows[0]["ProductId"]));

                            order_dgv["taxInclusiveCost", dgvselectedindex].Value = "N"; // we get unit price in the query so this is N DT.Rows[0]["taxInclusiveCost"];
                            order_dgv["taxPercentage", dgvselectedindex].Value = (DT.Rows[0]["tax_percentage"] == DBNull.Value ? string.Empty : DT.Rows[0]["tax_percentage"]);
                            if (order_dgv["TaxCode", dgvselectedindex].Value != null && order_dgv["TaxAmount", dgvselectedindex].Value != null)
                            {
                                order_dgv["TaxAmount", dgvselectedindex].Style = new DataGridViewCellStyle { ForeColor = Color.Blue, Font = new Font(order_dgv.Font, FontStyle.Underline), Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };

                            }
                            if (order_dgv["TaxCode", dgvselectedindex].Value == DBNull.Value || Convert.ToInt32(order_dgv.Rows[dgvselectedindex].Cells["TaxCode"].Value) == 0)
                            {
                                order_dgv.Rows[dgvselectedindex].Cells["TaxAmount"].ReadOnly = false;
                            }
                        }
                        else
                        {
                            dgvselectedindex = e.RowIndex;
                            Panel pnlMultiple_dgv = new Panel();
                            this.Controls.Add(pnlMultiple_dgv);
                            DataGridView multiple_dgv = new DataGridView();
                            pnlMultiple_dgv.Controls.Add(multiple_dgv);
                            multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                            multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                            multiple_dgv.Focus();
                            multiple_dgv.DataSource = DT;
                            multiple_dgv.Refresh();
                            multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                        }
                    }
                }

                else if (order_dgv.Columns[e.ColumnIndex].Name == "DiscountPercentage")
                {
                    marketListOrder = marketListItemExists();
                    if (fireCellValueChange)
                    {
                        decimal disc = 0;
                        try
                        {
                            disc = Convert.ToDecimal(order_dgv[e.ColumnIndex, e.RowIndex].Value) / 100;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            disc = 0;
                            order_dgv[e.ColumnIndex, e.RowIndex].Value = null;
                        }
                        DataTable dt = fetchProductDetails(order_dgv["Code", e.RowIndex].Value.ToString(), "!@#!@$@#%", marketListOrder);
                        if (dt != null)
                        {
                            order_dgv["Cost", e.RowIndex].Value = Convert.ToDecimal(dt.Rows[0]["Cost"]) * (1 - disc);
                            order_dgv["TaxAmount", e.RowIndex].Value = Math.Round(Convert.ToDecimal(dt.Rows[0]["TaxAmount"]) * (1 - disc), roundingPrecision, MidpointRounding.AwayFromZero);
                            order_dgv["TaxAmount", e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Blue, Font = new Font(order_dgv.Font, FontStyle.Underline), Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                fireCellValueChange = true;
                log.Error(ex);
            }
            log.LogMethodExit();

        }

        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DataGridView dg = (DataGridView)sender;
                fireCellValueChange = false;
                order_dgv["Code", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Code"].Value;
                order_dgv["Description", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Description"].Value;
                int uomId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["UOMId"].Value);
                CommonFuncs.GetUOMComboboxForSelectedRows(order_dgv, dgvselectedindex, uomId);
                if (cmbDocumentType.Text == ContractPurchaseOrderType)
                {
                    order_dgv["Quantity", dgvselectedindex].Value = 1;
                    order_dgv["Quantity", dgvselectedindex].ReadOnly = true;
                    order_dgv["TaxAmount", dgvselectedindex].ReadOnly = true;
                    order_dgv["TaxCode", dgvselectedindex].ReadOnly = true;
                    order_dgv["Cost", dgvselectedindex].ReadOnly = order_dgv["UnitLogisticsCost", dgvselectedindex].ReadOnly = true;
                    order_dgv["RequiredByDate", dgvselectedindex].Value = null;
                    order_dgv["SubTotal", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["CPOSubTotal"].Value;
                }
                else
                {
                    order_dgv["Quantity", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ReorderQuantity"].Value;
                    order_dgv["SubTotal", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["SubTotal"].Value;
                }
                order_dgv["Cost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Cost"].Value;
                order_dgv["UnitLogisticsCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["UnitLogisticsCost"].Value;
                order_dgv["TaxAmount", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["TaxAmount"].Value;
                order_dgv["LowerLimitCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["LowerLimitCost"].Value;
                order_dgv["UpperLimitCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["UpperLimitCost"].Value;
                order_dgv["CostVariancePercent", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["CostVariancePercentage"].Value;
                order_dgv["MarketListItem", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["MarketListItem"].Value;
                order_dgv["ProductId", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ProductId"].Value;
                order_dgv["RequisitionId", dgvselectedindex].Value = null;
                order_dgv["RequisitionLineId", dgvselectedindex].Value = null;
                order_dgv["PriceInTickets", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["PriceInTickets"].Value;
                order_dgv["taxInclusiveCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["taxInclusiveCost"].Value;
                order_dgv["taxPercentage", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["tax_percentage"].Value;
                int taxId = dg.Rows[dg.CurrentRow.Index].Cells["TaxCode"].Value == DBNull.Value ? -1 : Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["TaxCode"].Value);
                if (taxId != -1 && taxDTOList.Exists(x => x.TaxId == taxId) == false)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(2461), "Validation Error");
                    order_dgv["TaxCode", dgvselectedindex].Value = -1;
                    return;
                }
                else
                {
                    order_dgv["TaxCode", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["TaxCode"].Value;
                }

                InventoryList inventoryList = new InventoryList();
                decimal stock = inventoryList.GetProductStockQuantity(Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["ProductId"].Value));
                order_dgv["stockLink", dgvselectedindex].Value = stock.ToString();
                fireCellValueChange = true;
                dg.Parent.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                fireCellValueChange = true;
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            try
            {
                log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
                pnlMultiple_dgv.Size = new Size(300, (order_dgv.Rows[0].Cells[0].Size.Height * 10) - 3); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(150 + gb_order.Location.X + order_dgv.RowHeadersWidth + order_dgv.CurrentRow.Cells["Code"].ContentBounds.Location.X, order_dgv.Location.Y + order_dgv.ColumnHeadersHeight);
                pnlMultiple_dgv.BringToFront();
                pnlMultiple_dgv.BorderStyle = BorderStyle.None;
                pnlMultiple_dgv.BackColor = Color.White;
                multiple_dgv.Width = 300;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.AllowUserToAddRows = false;
                multiple_dgv.BackgroundColor = Color.White;

                for (int i = 2; i < multiple_dgv.Columns.Count; i++)
                    multiple_dgv.Columns[i].Visible = false;

                multiple_dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                multiple_dgv.Font = new Font("Arial", 8, FontStyle.Regular);
                multiple_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                multiple_dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                multiple_dgv.ReadOnly = true;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.RowHeadersVisible = false;
                multiple_dgv.ColumnHeadersVisible = false;
                multiple_dgv.AllowUserToResizeColumns = false;
                multiple_dgv.MultiSelect = false;
                multiple_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                multiple_dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Wheat;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                if (dg.SelectedRows.Count == 0)
                {
                    order_dgv.Rows.Remove(order_dgv.Rows[dgvselectedindex]);
                }
                dg.Visible = false;
                dg.Parent.Visible = false;
                gb_order.Controls.Remove(dg.Parent);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing multiple_dgv_LostFocus()", ex);
            }
            log.LogMethodExit();
        }

        private void cb_prodsearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ProductSearch();
            log.LogMethodExit();
        }

        private void ProductSearch()
        {
            log.LogMethodEntry();
            try
            {
                String prod_code = "";
                int quantity = 0;
                String qtyOperator = ">";
                String qtyString = "";
                SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
                string condition = "";
                if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
                {
                    condition = " and (Product.site_id = @site_id or @site_id = -1)";
                    cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
                }
                qtyOperator = dd_qty.Text;
                if (txt_qty.Text == "")
                {
                    qtyString = "";
                }
                else
                {
                    quantity = Convert.ToInt32(txt_qty.Text);
                    qtyString = " having sum(case isavailabletosell when 'N' then 0 when null then 0 else isnull(Inventory.Quantity, 0) end) " + qtyOperator + " " + quantity.ToString();

                }
                //Updated query to search barcode from productbarcode table
                //23-Mar-2016
                cmd.CommandText = "select  Code, sum(case isavailabletosell when 'N' then 0 when null then 0 else isnull(Inventory.Quantity, 0) end) quantity, Product.ProductId " +
                                    "from Product left outer join Inventory on Inventory.ProductId = Product.ProductId " +
                                    "left outer join Location " +
                                    "on Inventory.LocationId = Location.LocationId " +
                                    "left outer join (select * " +
                                                "from (select *, row_number() over(partition by productid order by productid) as num " +
                                                    "from productbarcode " +
                                                    "where BarCode like '%' + @product_code and isactive = 'Y')v " +
                                            "where num = 1)b on Product.productid = b.productid " +
                                    "where (Code like @product_code  or ProductName like N'%' + @product_code or description like '%' + @product_code or b.BarCode like '%' + @product_code)" +
                                    " AND Product.IsActive = 'Y' and IsPurchaseable = 'Y' " + condition +
                                    "group by code, Product.ProductId " + qtyString;
                //23-Mar-2016
                prod_code = txt_prodcode.Text + "%";
                cmd.Parameters.AddWithValue("@product_code", prod_code);
                cmd.Parameters.AddWithValue("@qty", quantity.ToString());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DT_search.Clear();
                da.Fill(DT_search);
                Rsearch_dgv.DataSource = DT_search;
                Rsearch_dgv.Refresh();
                Rsearch_dgv.Columns["AvailQty"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewNumericCellStyle();
                da.Dispose();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void order_dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (order_dgv.CurrentCell != null)
                {
                    if ((order_dgv.CurrentCell.ColumnIndex == 3 //Qty
                        || order_dgv.CurrentCell.ColumnIndex == 5 //cost
                        || order_dgv.CurrentCell.ColumnIndex == 9
                        || order_dgv.CurrentCell.ColumnIndex == 10) && (order_dgv["Description", order_dgv.CurrentCell.RowIndex].Value != null))
                    {
                        if (order_dgv["Description", order_dgv.CurrentCell.RowIndex].Value.ToString() != "")
                        {
                            Decimal product = 0;
                            decimal ltaxAmount = 0;
                            string pTaxInclusiveCost = null;
                            decimal pTaxPercent = 0;
                            try
                            {
                                if (order_dgv.Columns[e.ColumnIndex].DataPropertyName == "Cost")
                                {
                                    try { pTaxInclusiveCost = order_dgv["taxInclusiveCost", order_dgv.CurrentCell.RowIndex].Value.ToString(); } catch (Exception ex) { log.Error(ex); }
                                    try { pTaxPercent = Convert.ToDecimal(order_dgv["taxPercentage", order_dgv.CurrentCell.RowIndex].Value); } catch (Exception ex) { log.Error(ex); }
                                    if (pTaxInclusiveCost != null)
                                    {
                                        //if (pTaxInclusiveCost == "Y")
                                        //{
                                        //    ltaxAmount = Math.Round((pTaxPercent / 100) * (Convert.ToDecimal(order_dgv["Cost", order_dgv.CurrentCell.RowIndex].Value) / (1 + (pTaxPercent / 100))), roundingPrecision, MidpointRounding.AwayFromZero);
                                        //}
                                        //else
                                        //{
                                        ltaxAmount = Math.Round(Convert.ToDecimal(order_dgv["Cost", order_dgv.CurrentCell.RowIndex].Value) * (pTaxPercent / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                                        //}
                                        order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value = ltaxAmount;
                                        order_dgv["TaxAmount", e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Blue, Font = new Font(order_dgv.Font, FontStyle.Underline) };
                                    }
                                }
                                else
                                    ltaxAmount = Math.Round((order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value == null ? 0 : Convert.ToDecimal(order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value)), roundingPrecision, MidpointRounding.AwayFromZero);
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }

                            if ((decimal.TryParse((order_dgv["Quantity", order_dgv.CurrentCell.RowIndex].Value).ToString(), out qty) && (decimal.TryParse((order_dgv["Cost", order_dgv.CurrentCell.RowIndex].Value).ToString(), out cost))))
                            {
                                product = Math.Round(qty * cost, roundingPrecision, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(2360, (e.RowIndex + 1).ToString(), order_dgv.Columns[e.ColumnIndex].DataPropertyName));
                                order_dgv["Quantity", order_dgv.CurrentCell.RowIndex].Value = 0;
                                return;
                            }
                            order_dgv["SubTotal", order_dgv.CurrentCell.RowIndex].Value = product;
                        }

                    }
                    if (order_dgv.Columns[e.ColumnIndex].Name == "TaxAmount")
                    {
                        CalculateTotalTaxAmount();
                        recalculateTotal();
                    }
                    if (order_dgv.Columns[e.ColumnIndex].Name == "TaxCode")
                    {
                        if (order_dgv["TaxCode", e.RowIndex].Value == DBNull.Value || Convert.ToInt32(order_dgv.Rows[e.RowIndex].Cells["TaxCode"].Value) == 0)
                        {
                            order_dgv.Rows[e.RowIndex].Cells["TaxAmount"].Value = 0.00;
                            order_dgv.Rows[e.RowIndex].Cells["TaxAmount"].ReadOnly = false;
                        }

                        else
                        {
                            Decimal product = 0;
                            decimal ltaxAmount = 0;
                            string pTaxInclusiveCost = null;
                            decimal pTaxPercent = 0;
                            int taxID = Convert.ToInt32(order_dgv.Rows[e.RowIndex].Cells["TaxCode"].Value);
                            List<TaxDTO> selectedTax = taxDTOList.Where(x => x.TaxId == taxID).ToList();
                            if (selectedTax != null && selectedTax.Count > 0)
                            {
                                pTaxPercent = Convert.ToDecimal(selectedTax[0].TaxPercentage.ToString());
                            }

                            try
                            {
                                try { pTaxInclusiveCost = order_dgv["taxInclusiveCost", order_dgv.CurrentCell.RowIndex].Value.ToString(); } catch (Exception ex) { log.Error(ex); }
                                if (pTaxInclusiveCost != null)
                                {
                                    //if (pTaxInclusiveCost == "Y")
                                    //{

                                    //    ltaxAmount = Math.Round(pTaxPercent / 100 * Convert.ToDecimal(order_dgv["Cost", order_dgv.CurrentCell.RowIndex].Value) / (1 + pTaxPercent / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                                    //}
                                    //else
                                    //{
                                    ltaxAmount = Math.Round(Convert.ToDecimal(order_dgv["Cost", order_dgv.CurrentCell.RowIndex].Value) * (pTaxPercent / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                                    //}
                                    order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value = ltaxAmount;
                                }
                                else
                                {
                                    ltaxAmount = Math.Round((order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value == null ? 0 : Convert.ToDecimal(order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].Value)), roundingPrecision, MidpointRounding.AwayFromZero);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                            product = Math.Round(qty * cost, roundingPrecision, MidpointRounding.AwayFromZero);
                            order_dgv["SubTotal", order_dgv.CurrentCell.RowIndex].Value = product;
                            order_dgv["TaxAmount", order_dgv.CurrentCell.RowIndex].ReadOnly = true;
                            order_dgv["TaxAmount", e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Blue, Font = new Font(order_dgv.Font, FontStyle.Underline), Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };
                            order_dgv["taxPercentage", e.RowIndex].Value = pTaxPercent; //adding percentage to order grid
                            CalculateTotalTaxAmount();
                            recalculateTotal();

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

        private void cb_submit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (cb_vendor.Text == "")
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(850), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save"));
                    cb_vendor.Focus();
                }
                else if (order_dgv.Rows[0].Cells["ProductId"].Value == null || order_dgv.Rows[0].Cells["ProductId"].Value == DBNull.Value)
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(371), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save"));
                    return;
                }
                else
                {
                    if (transferGrid())
                    {
                        if (isAutoPOMode)
                        {
                            if (MessageBox.Show(utilities.MessageUtils.getMessage(2245), utilities.MessageUtils.getMessage("Auto PO"), MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                log.LogMethodExit();
                                return;
                            }
                        }
                        if (UpdateDatabase())
                        {
                            if (!isAutoPOMode)
                            {
                                POSearch();
                            }
                            else
                            {
                                MarkPOAsFinal();
                                foreach (Form child_form in Application.OpenForms)
                                {
                                    if (child_form.Name == "frmReceiveInventory")
                                    {
                                        child_form.Close();
                                        break;
                                    }
                                }
                                frmReceiveInventory frmReceive = new frmReceiveInventory(utilities, null, orderId);
                                CommonUIDisplay.setupVisuals(frmReceive);
                                //frmReceive.MdiParent = this.MdiParent;
                                //frmReceive.Width = this.Width;
                                //frmReceive.Height = this.Height;
                                CommonUIDisplay.SetFormLocationAndSize(this.MdiParent, "Inventory Receive", frmReceive);
                                frmReceive.Location = new Point(this.Location.X, this.Location.Y);
                                this.Close();
                                frmReceive.Show();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void MarkPOAsFinal()
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
            {
                dBTransaction.BeginTransaction();
                try
                {
                    purchaseOrderDTO = new PurchaseOrderDTO();
                    PurchaseOrder purchaseOrder = new PurchaseOrder(Convert.ToInt32(lb_orderid.Text), executionContext);
                    purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
                    purchaseOrderDTO.OrderNumber = purchaseOrderDTO.OrderNumber + AutoPOText;
                    purchaseOrderDTO.DocumentStatus = FinalStatus;
                    purchaseOrder.Save(dBTransaction.SQLTrx);
                    if (dBTransaction != null)
                    {
                        dBTransaction.EndTransaction();
                        dBTransaction.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    if (dBTransaction != null)
                    {
                        dBTransaction.RollBack();
                        dBTransaction.Dispose();
                    }
                    log.LogMethodExit();
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private bool transferGrid()
        {
            log.LogMethodEntry();
            try
            {
                DT_order.Rows.Clear();
                for (int i = 0; i < order_dgv.Rows.Count; i++)
                {
                    if (order_dgv["Code", i].Value != null && order_dgv["Quantity", i].Value != null)
                    {
                        if (order_dgv["Code", i].Value.ToString() != "")
                        {
                            if (order_dgv["RequiredByDate", i].Value != DBNull.Value)
                            {
                                try
                                {
                                    Convert.ToDateTime(order_dgv["RequiredByDate", i].Value);
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2613));
                                }
                            }
                            if ((cmbDocumentType.Text != ContractPurchaseOrderType && Convert.ToDecimal(order_dgv["Quantity", i].Value) >= Convert.ToDecimal(order_dgv["ReceiveQuantity", i].Value)) || cmbDocumentType.Text == ContractPurchaseOrderType)
                            {
                                DT_order.Rows.Add(order_dgv["Code", i].Value.ToString(),
                                    order_dgv["Description", i].Value.ToString(),
                                    order_dgv["Quantity", i].Value,
                                    order_dgv["RequiredByDate", i].Value,
                                    order_dgv["Cost", i].Value,
                                    order_dgv["UnitLogisticsCost", i].Value,
                                    order_dgv["TaxAmount", i].Value,
                                    order_dgv["DiscountPercentage", i].Value,
                                    order_dgv["SubTotal", i].Value,
                                    order_dgv["LineId", i].Value,
                                    order_dgv["CancelledDate", i].Value,
                                    order_dgv["isActive", i].Value,
                                    order_dgv["ReceiveQuantity", i].Value,
                                    order_dgv["MarketListItem", i].Value,
                                    order_dgv["ProductId", i].Value,
                                    order_dgv["RequisitionId", i].Value,
                                    order_dgv["RequisitionLineId", i].Value,
                                    order_dgv["PriceInTickets", i].Value,
                                    order_dgv["taxInclusiveCost", i].Value,
                                    order_dgv["taxPercentage", i].Value,
                                    order_dgv["TaxCode", i].Value,
                                    order_dgv["cmbUOM", i].Value
                                    );//Added 20-May-2016

                            }
                            else
                            {
                                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1020));//Updated message number 17-Jun-2016
                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(true);
            return true;
        }

        private bool UpdateDatabase()
        {
            log.LogMethodEntry();
            if (cb_submit.Tag != null && cb_submit.Tag.ToString().Contains("Save"))
            {
                // New order to be inserted.
                int return_id;
                //  int returnPurchaseOrderLineId = -1;
                string PO_number;

                SqlTransaction SQLTrx = null;
                SqlConnection TrxCnn = null;

                if (SQLTrx == null)
                {
                    TrxCnn = Semnox.Parafait.Inventory.CommonFuncs.Utilities.createConnection();
                    SQLTrx = TrxCnn.BeginTransaction();
                }

                try
                {
                    purchaseOrderDTO = new PurchaseOrderDTO();
                    purchaseOrderDTO.OrderStatus = tb_status.Text;//"Open";
                    purchaseOrderDTO.OrderDate = ServerDateTime.Now;
                    purchaseOrderDTO.VendorId = Convert.ToInt32(cb_vendor.SelectedValue);
                    purchaseOrderDTO.LastModDttm = ServerDateTime.Now;
                    purchaseOrderDTO.CancelledDate = DateTime.MinValue;
                    purchaseOrderDTO.RequestShipDate = DateTime.MinValue;
                    purchaseOrderDTO.ReceivedDate = DateTime.MinValue;
                    if (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) > -1)
                    {
                        purchaseOrderDTO.FromSiteId = Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.SiteId;
                        purchaseOrderDTO.ToSiteId = Convert.ToInt32(cmbToSite.SelectedValue);
                    }

                    if (cmbDocumentType.SelectedIndex == -1)
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1542));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                        purchaseOrderDTO.DocumentTypeID = Convert.ToInt32(cmbDocumentType.SelectedValue);
                    if (dtpFromDate.CustomFormat == " ")
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1097));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                        purchaseOrderDTO.Fromdate = dtpFromDate.Value.Date;
                    if (dtpToDate.CustomFormat == " ")
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1098));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                        purchaseOrderDTO.ToDate = dtpToDate.Value.Date;

                    if (isAutoPOMode == false && dtpToDate.Value <= dtpFromDate.Value)
                    {
                        dtpToDate.Value = toDate;
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1093), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                        log.LogMethodExit();
                        return false;
                    }

                    if (chkIsCreditPO.Checked)
                        purchaseOrderDTO.IsCreditPO = "Y";
                    else
                        purchaseOrderDTO.IsCreditPO = "N";
                    purchaseOrderDTO.OrderTotal = Convert.ToDouble(tb_total.Text);
                    purchaseOrderDTO.AmendmentNumber = -1;
                    purchaseOrderDTO.DocumentStatus = "";
                    purchaseOrderDTO.ReprintCount = -1;
                    purchaseOrderDTO.OrderNumber = string.Empty;
                    PurchaseOrder purchaseOrderBL = new PurchaseOrder(purchaseOrderDTO, executionContext);
                    purchaseOrderBL.Save(SQLTrx);
                    return_id = purchaseOrderDTO.PurchaseOrderId;
                    lb_orderid.Text = Convert.ToString(return_id);
                    orderId = return_id;
                    PO_number = purchaseOrderDTO.OrderNumber;
                    tb_order.Text = PO_number;
                    chkIsCreditPO.Enabled = false;
                    if (cmbDocumentType.Text == "")
                    {
                        cmbDocumentType.Text = RegularPurchaseOrderType;
                    }
                    if (cmbDocumentType.Text == RegularPurchaseOrderType)
                    {
                        tb_status.Enabled = true;
                        cmbDocumentType.Enabled = false;
                    }

                    bool validRowsExist = false;
                    //Insert into detail table. 
                    if (DT_order.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT_order.Rows.Count; j++)
                        {
                            if (Convert.ToDouble(DT_order.Rows[j]["Quantity"]) <= 0)
                                continue;
                            purchaseOrderLineDTO = new PurchaseOrderLineDTO();
                            purchaseOrderLineDTO.PurchaseOrderId = return_id;
                            purchaseOrderLineDTO.ItemCode = DT_order.Rows[j]["Code"].ToString();
                            purchaseOrderLineDTO.Description = DT_order.Rows[j]["Description"].ToString();
                            purchaseOrderLineDTO.RequiredByDate = (DT_order.Rows[j]["RequiredByDate"] == null || DT_order.Rows[j]["RequiredByDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(DT_order.Rows[j]["RequiredByDate"]);
                            purchaseOrderLineDTO.Quantity = Convert.ToDouble(DT_order.Rows[j]["Quantity"]);
                            purchaseOrderLineDTO.UnitPrice = DT_order.Rows[j]["Cost"] == null || DT_order.Rows[j]["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(DT_order.Rows[j]["Cost"]);
                            purchaseOrderLineDTO.UnitLogisticsCost = (DT_order.Rows[j]["UnitLogisticsCost"] == null || DT_order.Rows[j]["UnitLogisticsCost"] == DBNull.Value) ? 0 : Convert.ToDouble(DT_order.Rows[j]["UnitLogisticsCost"]);
                            purchaseOrderLineDTO.TaxAmount = Convert.ToDouble(order_dgv["TaxAmount", j].Value);
                            purchaseOrderLineDTO.SubTotal = Convert.ToDouble(DT_order.Rows[j]["SubTotal"]);
                            purchaseOrderLineDTO.DiscountPercentage = (DT_order.Rows[j]["DiscountPercentage"] == null || DT_order.Rows[j]["DiscountPercentage"] == DBNull.Value) ? 0 : Convert.ToDouble(DT_order.Rows[j]["DiscountPercentage"]);
                            purchaseOrderLineDTO.ProductId = Convert.ToInt32(DT_order.Rows[j]["ProductId"]);
                            purchaseOrderLineDTO.RequisitionId = (DT_order.Rows[j]["RequisitionId"] == null || DT_order.Rows[j]["RequisitionId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_order.Rows[j]["RequisitionId"]);
                            purchaseOrderLineDTO.RequisitionLineId = (DT_order.Rows[j]["RequisitionLineId"] == null || DT_order.Rows[j]["RequisitionLineId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_order.Rows[j]["RequisitionLineId"]);
                            purchaseOrderLineDTO.PriceInTickets = (DT_order.Rows[j]["PriceInTickets"].ToString() == "NaN" || DT_order.Rows[j]["PriceInTickets"] == null || DT_order.Rows[j]["PriceInTickets"] == DBNull.Value) ? Double.NaN : Convert.ToDouble(DT_order.Rows[j]["PriceInTickets"]);
                            purchaseOrderLineDTO.CancelledDate = DateTime.MinValue;
                            purchaseOrderLineDTO.RequiredByDate = DateTime.MinValue;
                            purchaseOrderLineDTO.isActive = "Y";
                            purchaseOrderLineDTO.PurchaseTaxId = (order_dgv["TaxCode", j].Value == null || order_dgv["TaxCode", j].Value == DBNull.Value) ? -1 : Convert.ToInt32(order_dgv["TaxCode", j].Value);//Convert.ToInt32(order_dgv["TaxCode", j].Value); //Convert.ToInt32(DT_order.Rows[j]["TaxCode"]);
                            purchaseOrderLineDTO.UOMId = (order_dgv["cmbUOM", j].Value == null || order_dgv["cmbUOM", j].Value == DBNull.Value) ? ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(order_dgv["ProductId", j].Value)).UomId : Convert.ToInt32(order_dgv["cmbUOM", j].Value);
                            PurchaseOrderLine purchaseOrderLine = new PurchaseOrderLine(purchaseOrderLineDTO, executionContext);
                            purchaseOrderLine.Save(SQLTrx);

                            validRowsExist = true;
                        }
                        log.LogMethodExit();
                        if (validRowsExist)
                        {
                            SQLTrx.Commit();
                            if (!isAutoPOMode)
                            {
                                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(867, PO_number), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save Order"));
                            }
                            log.LogMethodExit(true);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1105), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save Order"));
                            SQLTrx.Rollback();
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1105), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save Order"));
                        SQLTrx.Rollback();
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                catch (Exception Ex)
                {
                    log.Error(Ex);
                    SQLTrx.Rollback();
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(868) + Ex.Message); 
                    log.LogMethodExit(false);
                    return false;
                }
            }
            else //cb_submit.Text = "Update"
            {
                String POnumber = tb_order.Text;
                SqlTransaction SQLTrx = null;
                SqlConnection TrxCnn = null;

                if (SQLTrx == null)
                {
                    TrxCnn = Semnox.Parafait.Inventory.CommonFuncs.Utilities.createConnection();
                    SQLTrx = TrxCnn.BeginTransaction();
                }

                try
                {
                    recalculateTotal();

                    int POId = Convert.ToInt32(lb_orderid.Text);
                    decimal receiveQuantity = 0;

                    purchaseOrderDTO = new PurchaseOrderDTO();
                    purchaseOrderDataHandler = new PurchaseOrderDataHandler(SQLTrx);
                    purchaseOrderDTO = purchaseOrderDataHandler.GetPurchaseOrder(POId, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (lblDocumentStatus.Text == FinalStatus)
                    {
                        PurchaseOrderDTO amendmentCopyDTO = new PurchaseOrderDTO(-1, purchaseOrderDTO.OrderStatus, purchaseOrderDTO.OrderNumber, purchaseOrderDTO.OrderDate, purchaseOrderDTO.VendorId,
                                                            purchaseOrderDTO.ContactName, purchaseOrderDTO.Phone, purchaseOrderDTO.VendorAddress1, purchaseOrderDTO.VendorAddress2, purchaseOrderDTO.VendorCity, purchaseOrderDTO.VendorState,
                                                            purchaseOrderDTO.VendorCountry, purchaseOrderDTO.VendorPostalCode, purchaseOrderDTO.ShipToAddress1
                                                            , purchaseOrderDTO.ShipToAddress2, purchaseOrderDTO.ShipToCity, purchaseOrderDTO.ShipToState, purchaseOrderDTO.ShipToCountry,
                                                            purchaseOrderDTO.ShipToPostalCode, purchaseOrderDTO.ShipToAddressRemarks,
                                                            purchaseOrderDTO.RequestShipDate, purchaseOrderDTO.OrderTotal, purchaseOrderDTO.LastModUserId, purchaseOrderDTO.LastModDttm,
                                                            purchaseOrderDTO.ReceivedDate, purchaseOrderDTO.ReceiveRemarks, purchaseOrderDTO.site_id, purchaseOrderDTO.Guid, purchaseOrderDTO.SynchStatus, purchaseOrderDTO.CancelledDate,
                                                            purchaseOrderDTO.MasterEntityId, purchaseOrderDTO.IsCreditPO, purchaseOrderDTO.DocumentTypeID, purchaseOrderDTO.Fromdate,
                                                            purchaseOrderDTO.ToDate, purchaseOrderDTO.OrderRemarks, purchaseOrderDTO.ExternalSystemReference, purchaseOrderDTO.ReprintCount, purchaseOrderDTO.AmendmentNumber,
                                                            purchaseOrderDTO.DocumentStatus, purchaseOrderDTO.FromSiteId, purchaseOrderDTO.ToSiteId, purchaseOrderDTO.OriginalReferenceGUID,
                                                            null, null, executionContext.GetUserId(), ServerDateTime.Now, purchaseOrderDTO.IsActive, false);
                        PurchaseOrder amendementPOBL = new PurchaseOrder(amendmentCopyDTO, executionContext);
                        amendementPOBL.Save(SQLTrx);
                        lblDocumentStatus.Text = "";
                        lblAmendmentNumber.Text = (purchaseOrderDTO.AmendmentNumber == -1) ? "1" : (Convert.ToInt32(purchaseOrderDTO.AmendmentNumber) + 1).ToString();
                    }

                    string orderStatus = tb_status.Text;
                    decimal orderQuantity = 0;
                    decimal activeQuantity = 0;
                    decimal inactiveQuantity = 0;

                    if (cmbDocumentType.Text != ContractPurchaseOrderType)
                    {
                        SqlCommand select_cmd = new SqlCommand();
                        select_cmd.Connection = TrxCnn;
                        select_cmd.Transaction = SQLTrx;
                        select_cmd.CommandText = "if exists(select * from purchaseorderreceive_line where purchaseorderid = @PurchaseOrderId) select sum(quantity) from purchaseorderreceive_line where purchaseorderid = @PurchaseOrderId else select 0";
                        select_cmd.Parameters.AddWithValue("@PurchaseOrderId", POId);
                        DataTable dt_receiveTotal = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(select_cmd);
                        da.Fill(dt_receiveTotal);
                        receiveQuantity = Convert.ToInt32(dt_receiveTotal.Rows[0][0]);

                        //check if atleast one line is received
                        for (int i = 0; i < order_dgv.RowCount - 1; i++)
                        {
                            if (order_dgv["isActive", i].Value == null)
                            {
                                if (order_dgv["Quantity", i].Value != null)
                                {
                                    activeQuantity += Convert.ToDecimal(order_dgv["Quantity", i].Value);
                                }
                            }

                            if (order_dgv["isActive", i].Value != null)
                            {
                                if (order_dgv["isActive", i].Value.ToString() == "Y")
                                {
                                    if (order_dgv["Quantity", i].Value != null)
                                    {
                                        activeQuantity += Convert.ToDecimal(order_dgv["Quantity", i].Value);
                                    }
                                }
                                else if (order_dgv["isActive", i].Value.ToString() == "N")
                                {
                                    if (order_dgv["Quantity", i].Value != null)
                                    {
                                        inactiveQuantity += Convert.ToDecimal(order_dgv["Quantity", i].Value);
                                    }
                                }
                            }
                            if (order_dgv["Quantity", i].Value != null)
                            {
                                orderQuantity += Convert.ToDecimal(order_dgv["Quantity", i].Value);
                            }
                        }

                        if (receiveQuantity > 0) //order total = receive total
                        {
                            if (activeQuantity - receiveQuantity == 0)
                            {
                                orderStatus = PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED;
                            }

                        }
                        else if (orderQuantity == inactiveQuantity)
                            orderStatus = PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED;
                    }

                    purchaseOrderDTO.OrderStatus = orderStatus;
                    purchaseOrderDTO.OrderNumber = POnumber;
                    purchaseOrderDTO.VendorId = Convert.ToInt32(cb_vendor.SelectedValue);
                    purchaseOrderDTO.OrderTotal = Convert.ToDouble(tb_total.Text);
                    purchaseOrderDTO.LastModDttm = ServerDateTime.Now;
                    purchaseOrderDTO.PurchaseOrderId = Convert.ToInt32(lb_orderid.Text);
                    if (orderStatus == PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED)
                    {
                        purchaseOrderDTO.CancelledDate = ServerDateTime.Now;
                    }
                    else
                    {
                        purchaseOrderDTO.CancelledDate = DateTime.MinValue;
                    }
                    purchaseOrderDTO.DocumentTypeID = Convert.ToInt32(cmbDocumentType.SelectedIndex) == -1 ? -1 : Convert.ToInt32(cmbDocumentType.SelectedValue);
                    if (dtpFromDate.CustomFormat == " ")
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1097));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                        purchaseOrderDTO.Fromdate = dtpFromDate.Value.Date;
                    if (dtpToDate.CustomFormat == " ")
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1098));
                        log.LogMethodExit(false);
                        return false;
                    }
                    else
                        purchaseOrderDTO.Fromdate = dtpFromDate.Value.Date;
                    if (orderStatus == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED)
                        purchaseOrderDTO.ReceivedDate = ServerDateTime.Now;
                    else
                        purchaseOrderDTO.ReceivedDate = DateTime.MinValue;
                    purchaseOrderDTO.AmendmentNumber = Convert.ToInt32(lblAmendmentNumber.Text);
                    purchaseOrderDTO.DocumentStatus = "";

                    purchaseOrderDTO.ReprintCount = -1;
                    try
                    {
                        PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                        purchaseOrder.Save(SQLTrx);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        SQLTrx.Rollback();
                        tb_cancelledDate.Text = "";
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(869), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Update Order"));
                        POSearch();
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (DT_order.Rows.Count > 0)
                    {
                        for (int j = 0; j < DT_order.Rows.Count; j++)
                        {
                            purchaseOrderLineDTO = new PurchaseOrderLineDTO();
                            purchaseOrderLineDTO.PurchaseOrderId = Convert.ToInt32(POId);
                            purchaseOrderLineDTO.ItemCode = DT_order.Rows[j]["Code"].ToString();
                            purchaseOrderLineDTO.Description = DT_order.Rows[j]["Description"].ToString();
                            purchaseOrderLineDTO.RequiredByDate = (DT_order.Rows[j]["RequiredByDate"] == null || DT_order.Rows[j]["RequiredByDate"] == DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(DT_order.Rows[j]["RequiredByDate"]);
                            purchaseOrderLineDTO.Quantity = Convert.ToDouble(DT_order.Rows[j]["Quantity"]);
                            purchaseOrderLineDTO.UnitPrice = DT_order.Rows[j]["Cost"] == null || DT_order.Rows[j]["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(DT_order.Rows[j]["Cost"]);
                            purchaseOrderLineDTO.UnitLogisticsCost = (DT_order.Rows[j]["UnitLogisticsCost"] == null || DT_order.Rows[j]["UnitLogisticsCost"] == DBNull.Value) ? 0 : Convert.ToDouble(DT_order.Rows[j]["UnitLogisticsCost"]);
                            purchaseOrderLineDTO.TaxAmount = Convert.ToDouble(order_dgv["TaxAmount", j].Value);// (DT_order.Rows[j]["TaxAmount"] == null || DT_order.Rows[j]["TaxAmount"] == DBNull.Value) ? -1 : Convert.ToDouble(DT_order.Rows[j]["TaxAmount"]);  
                            purchaseOrderLineDTO.SubTotal = Convert.ToDouble(DT_order.Rows[j]["SubTotal"]);
                            purchaseOrderLineDTO.DiscountPercentage = (DT_order.Rows[j]["DiscountPercentage"] == null || DT_order.Rows[j]["DiscountPercentage"] == DBNull.Value) ? 0 : Convert.ToDouble(DT_order.Rows[j]["DiscountPercentage"]);
                            purchaseOrderLineDTO.ProductId = Convert.ToInt32(DT_order.Rows[j]["ProductId"]);
                            purchaseOrderLineDTO.RequisitionId = (DT_order.Rows[j]["RequisitionId"] == null || DT_order.Rows[j]["RequisitionId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_order.Rows[j]["RequisitionId"]);
                            purchaseOrderLineDTO.RequisitionLineId = (DT_order.Rows[j]["RequisitionLineId"] == null || DT_order.Rows[j]["RequisitionLineId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_order.Rows[j]["RequisitionLineId"]);
                            purchaseOrderLineDTO.PriceInTickets = (DT_order.Rows[j]["PriceInTickets"].ToString() == "NaN" || DT_order.Rows[j]["PriceInTickets"] == null || DT_order.Rows[j]["PriceInTickets"] == DBNull.Value) ? Double.NaN : Convert.ToDouble(DT_order.Rows[j]["PriceInTickets"]);

                            purchaseOrderLineDTO.PurchaseTaxId = (order_dgv["TaxCode", j].Value == null || order_dgv["TaxCode", j].Value == DBNull.Value) ? -1 : Convert.ToInt32(order_dgv["TaxCode", j].Value);
                            purchaseOrderLineDTO.UOMId = (DT_order.Rows[j]["UOMId"] == null || DT_order.Rows[j]["UOMId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_order.Rows[j]["UOMId"]);

                            if (DT_order.Rows[j]["isActive"] == null || DT_order.Rows[j]["isActive"] == DBNull.Value)
                            {
                                purchaseOrderLineDTO.isActive = "Y";
                                purchaseOrderLineDTO.CancelledDate = DateTime.MinValue;
                            }
                            else
                            {
                                if (DT_order.Rows[j]["isActive"].ToString() == "Y")
                                {
                                    purchaseOrderLineDTO.isActive = "Y";
                                    purchaseOrderLineDTO.CancelledDate = DateTime.MinValue;
                                }
                                else
                                {
                                    purchaseOrderLineDTO.CancelledDate = ServerDateTime.Now;
                                    purchaseOrderLineDTO.isActive = DT_order.Rows[j]["isActive"].ToString();
                                }
                            }

                            if (DT_order.Rows[j]["LineId"] != DBNull.Value)
                            {
                                purchaseOrderLineDTO.PurchaseOrderLineId = Convert.ToInt32(DT_order.Rows[j]["LineId"]);
                            }
                            purchaseOrderLineDTO.UOMId = (order_dgv["cmbUOM", j].Value == null || order_dgv["cmbUOM", j].Value == DBNull.Value) ? -1 : Convert.ToInt32(order_dgv["cmbUOM", j].Value);
                            PurchaseOrderLine purchaseOrderLine = new PurchaseOrderLine(purchaseOrderLineDTO, executionContext);
                            purchaseOrderLine.Save(SQLTrx);
                        }
                        log.LogMethodExit();
                        SQLTrx.Commit();
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(870, tb_order.Text), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Update Order"));
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1105), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Save Order"));
                        SQLTrx.Rollback();
                        log.LogMethodExit(false);
                        return false;
                    }

                }
                catch (Exception Ex)
                {
                    SQLTrx.Rollback();
                    log.Error(Ex);
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(868) + Ex.Message); 
                    log.LogMethodExit(false);
                    return false;
                }
            }

        }
        private void ClearFields()
        {
            log.LogMethodEntry();
            cmbDocumentType.Enabled = true;
            chkIsCreditPO.Enabled = true;
            chkIsCreditPO.Checked = true;
            cmbDocumentType.SelectedIndex = -1;
            cb_vendor.SelectedValue = -1;
            tb_order.Text = "";
            tb_contact.Text = "";
            tb_phone.Text = "";
            tb_MarkupPercent.Text =
            tb_TotalRetail.Text =
            tb_TotalLandedCost.Text =
            tb_totalLogisticsCost.Text =
            tb_TotalQuantity.Text =
            tb_total.Text = "0";

            DT_order.Rows.Clear();
            order_dgv.Rows.Clear();
            tb_date.Text = System.DateTime.Now.ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.DATETIME_FORMAT);
            tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
            tb_status.Enabled = false;
            tb_cancelledDate.Text = "";
            cb_amendment.Enabled = false;
            lblAmendmentNumber.Text = "";
            lb_orderid.Text = "";
            lblDocumentStatus.Text = "";
            order_dgv.Enabled = true;
            cmbDocumentType.Enabled = true;
            order_dgv.Columns["Quantity"].ReadOnly = false;
            lblEmail.Text = "";
            dtpFromDate.Enabled = true;
            dtpToDate.Enabled = true;
            dtpFromDate.CustomFormat = " ";
            dtpToDate.CustomFormat = " ";
            dtpFromDate.Value = DateTime.Now.AddDays(-1).Date; //DateTime.Now.Date;
            dtpToDate.Value = DateTime.Now.Date;//DateTime.Now.AddDays(1).Date;
            toDate = dtpToDate.Value;
            fromDate = dtpFromDate.Value;
            cmbToSite.SelectedValue = -1;
            //toDate = DateTime.Now.Date;
            //fromDate = DateTime.Now.AddDays(1).Date;
            lnkRemarks = null;
            inventoryNotesUI = null;
            lblViewRequisitions.Tag = null;
            //purchaseTaxLineGridView.Visible = false;
            txtTotalTax.Text = "0";
            lblViewTotalTax.Enabled = false;
            dgvselectedindex = 0;
            log.LogMethodExit();
        }

        private void cb_search_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSearch();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void POSearch()
        {
            log.LogMethodEntry();
            String order_number = "";
            String order_status = "";
            string prodSearch = "";
            string condition = "";
            int vendor_id = -1;
            string order_type = "";

            SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
            order_number = tb_searchorder.Text.Trim() + "%";
            order_status = "('" + cb_searchstatus.Text.Trim() + (cb_searchstatus.Text.Trim().Equals(PurchaseOrderDTO.PurchaseOrderStatus.OPEN) ? "', 'Drop Ship" : "") + "')";
            order_type = cmbOrderType.Text.Trim() + "%";

            if (txtProductPOSearch.Text.Trim() != "")
                //23-Mar-2016
                prodSearch = " and exists (select 1 from product p left outer join (select * " +
                                "from (" +
                                "select *, row_number() over(partition by productid order by productid) as num " +
                             "from productbarcode " +
                             "where BarCode like @prod and isactive = 'Y')v " +
                             "where num = 1)b on p.productid = b.productid, PurchaseOrder_Line l where (code like @prod or p.description like @prod or ProductName like @prod or b.BarCode like @prod) and l.ItemCode = p.Code and l.PurchaseOrderId = o.PurchaseOrderId) ";
            //23-Mar-2016
            else
                prodSearch = "";
            if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
            {
                condition = " and (o.site_id = @site_id or @site_id = -1) ";
                cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
            }

            if (cb_searchvendor.SelectedIndex == -1)// || (cb_searchvendor.SelectedIndex == 0))
            {
                cmd.CommandText = "exec dbo.SetContextInfo @loginUserId ;select PurchaseOrderID, OrderNumber, OrderStatus " +//, PurchaseOrderId , amendmentnumber, num "+
                                    "from( " +
                                            "select PurchaseOrderId, OrderNumber, OrderStatus, isnull(amendmentnumber, 0) amendmentnumber, ROW_NUMBER() over (partition by OrderNumber order by isnull(amendmentnumber, 0) desc) num, orderdate " +
                                            "from PurchaseOrder o left outer join inventorydocumenttype on o.documenttypeid = inventorydocumenttype.documenttypeid " +
                                            "where (OrderNumber like @vorder_number AND  OrderStatus in " + order_status + " and isnull(inventorydocumenttype.name, '') like @type) " +
                                            @"  and isnull(AmendmentNumber, 0) = (select max(isnull(AmendmentNumber, 0))
											  from purchaseorder 
											  where ordernumber =  o.ordernumber and LastModUserId in (select loginId "
                                                              + @" from DataAccessView dav
                                                               where
                                                            ((Entity = 'PO' and dav.DataAccessRuleId is not null)
						                                    OR
                                                           dav.DataAccessRuleId is null
						                                    )  ))" +
                                            condition +
                                            prodSearch +
                                    ")v " +
                                    "where num = 1 " +
                                    "order by orderdate desc";
            }
            else
            {
                vendor_id = Convert.ToInt32(cb_searchvendor.SelectedValue);
                cmd.CommandText = "exec dbo.SetContextInfo @loginUserId ;select PurchaseOrderID, OrderNumber, OrderStatus " +//, PurchaseOrderId , amendmentnumber, num "+
                                    "from( " +
                                            "select PurchaseOrderId, OrderNumber, OrderStatus, isnull(amendmentnumber, 0) amendmentnumber, ROW_NUMBER() over (partition by OrderNumber order by isnull(amendmentnumber, 0) desc) num, orderdate " +
                                              "from PurchaseOrder o left outer join inventorydocumenttype on o.documenttypeid = inventorydocumenttype.documenttypeid " +
                                              "where (OrderNumber like @vorder_number AND  OrderStatus in " + order_status + " AND VendorId = @vvendorid  and isnull(inventorydocumenttype.name, '') like @type) " +
                                              @"  and isnull(AmendmentNumber, 0) = (select max(isnull(AmendmentNumber, 0))
											  from purchaseorder
											  where ordernumber =  o.ordernumber and LastModUserId in (select loginId "
                                                              + @" from DataAccessView dav
                                                               where
                                                            ((Entity = 'PO' and dav.DataAccessRuleId is not null)
						                                    OR
                                                           dav.DataAccessRuleId is null
						                                    ) ))" +
                                                condition +
                                               prodSearch +
                                    ")v " +
                                    "where num = 1 " +
                                  "order by orderdate desc";
                cmd.Parameters.AddWithValue("@vvendorid", vendor_id);
            }

            cmd.Parameters.AddWithValue("@loginUserId", Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.User_Id);
            cmd.Parameters.AddWithValue("@prod", txtProductPOSearch.Text + "%");
            cmd.Parameters.AddWithValue("@vorder_number", order_number);
            cmd.Parameters.AddWithValue("@type", order_type);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable DT_POSearch = new DataTable();

            DT_POSearch.Clear();
            cb_search.Enabled = false;
            da.Fill(DT_POSearch);

            cb_search.Enabled = true;
            searchorder_dgv.DataSource = DT_POSearch;
            searchorder_dgv.Refresh();
            if (DT_POSearch.Rows.Count == 0)
                ClearFields();
            da.Dispose();
            log.LogMethodExit();
        }

        private void searchorder_dgv_selectionChanged()
        {
            log.LogMethodEntry();
            int index;
            try
            {
                index = searchorder_dgv.SelectedRows[0].Index;
            }
            catch (Exception ex)
            {
                ClearFields();
                enableDisableFields("enable");
                cb_submit.Text = MessageContainerList.GetMessage(executionContext, "Save PO");
                cb_submit.Tag = "Save PO";
                log.Error(ex);
                log.LogMethodExit();
                return;
            }
            if (index < 0) //Header clicked
            {
                log.LogMethodExit();
                return;
            }

            if (searchorder_dgv["PurchaseOrderID", index].Value.ToString() == lb_orderid.Text)
            {
                log.LogMethodExit();
                return;
            }

            try
            {
                ClearFields();
                String ponumber = searchorder_dgv["OrderNumber", index].Value.ToString();
                int orderID = Convert.ToInt32(searchorder_dgv["PurchaseOrderId", index].Value);
                string condition = "";
                SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
                if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
                {
                    condition = " and (p.site_id = @site_id or @site_id = -1) ";
                    cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
                }
                cmd.CommandText = "select PurchaseOrderId,OrderStatus,OrderNumber,OrderDate,VendorId,ContactName,Phone, DocumentType,  " +
                                    "OrderRemarks,OrderTotal,Email, cancelleddate, isCreditPO, amendmentNumber, DocumentStatus, reprintcount, ToSiteId " +
                                    " ,fromdate, todate " +
                                    "from ( " +
                                                "select PurchaseOrderId,OrderStatus,OrderNumber,OrderDate,p.VendorId,v.ContactName,v.Phone, d.Name DocumentType, p.ToSiteId,  " +
                                                "OrderRemarks,OrderTotal,v.TaxRegistrationNumber, v.Email, p.cancelleddate, isCreditPO, isnull(amendmentNumber, 0) amendmentNumber, isnull(DocumentStatus, '') DocumentStatus, isnull(reprintcount, 0) reprintcount, " +
                                                " fromdate, todate " +
                                                "from vendor v, PurchaseOrder p left outer join inventorydocumenttype d on p.documenttypeid = d.documenttypeid " +
                                                "where (PurchaseOrderID = @OrderID) " +
                                                condition +
                                                "and v.vendorId = p.VendorId " +
                                    ")v";

                cmd.Parameters.AddWithValue("@OrderID", orderID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable DT = new DataTable();
                da.Fill(DT);
                cmd.Dispose();
                da.Dispose();
                lb_orderid.Text = DT.Rows[0]["PurchaseOrderId"].ToString();
                lblAmendmentNumber.Text = DT.Rows[0]["amendmentNumber"].ToString();
                lblDocumentStatus.Text = DT.Rows[0]["DocumentStatus"].ToString().Trim();
                lblReprintCount.Text = DT.Rows[0]["reprintcount"].ToString().Trim();
                tb_order.Text = DT.Rows[0]["OrderNumber"].ToString();

                tb_contact.Text = DT.Rows[0]["ContactName"].ToString();
                DateTime shortdate = Convert.ToDateTime(DT.Rows[0]["OrderDate"]);
                tb_date.Text = shortdate.ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.DATETIME_FORMAT);
                if (DT.Rows[0]["CancelledDate"] != null && DT.Rows[0]["CancelledDate"] != DBNull.Value)
                    tb_cancelledDate.Text = Convert.ToDateTime(DT.Rows[0]["CancelledDate"]).ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.DATETIME_FORMAT);
                tb_phone.Text = DT.Rows[0]["Phone"].ToString();
                tb_total.Text = string.Format("{0:N3}", DT.Rows[0]["OrderTotal"]);
                tb_status.Text = DT.Rows[0]["OrderStatus"].ToString();
                orderStatus = DT.Rows[0]["OrderStatus"].ToString();
                if (orderStatus.Equals("Drop Ship"))
                {
                    tb_status.Enabled = false;
                }

                cb_vendor.SelectedValue = Convert.ToInt32(DT.Rows[0]["VendorId"].ToString());
                lblEmail.Text = (DT.Rows[0]["Email"] == null || DT.Rows[0]["Email"] == DBNull.Value) ? "" : DT.Rows[0]["Email"].ToString();

                //Start update 18-Aug-2016
                if (DT.Rows[0]["isCreditPO"] == DBNull.Value)
                    chkIsCreditPO.Checked = false;
                else if (DT.Rows[0]["isCreditPO"].ToString() == "Y")
                {
                    chkIsCreditPO.Checked = true;
                }
                else
                    chkIsCreditPO.Checked = false;

                tb_status.Text = DT.Rows[0]["OrderStatus"].ToString();
                if (DT.Rows[0]["DocumentType"] == DBNull.Value)
                    cmbDocumentType.Text = RegularPurchaseOrderType;
                else
                    cmbDocumentType.Text = DT.Rows[0]["DocumentType"].ToString();
                chkIsCreditPO.Enabled = false;
                cmbDocumentType.Enabled = false;

                dtpFromDate.Enabled = false;
                //End update 18-Aug-2016
                dtpFromDate.CustomFormat = "dd-MMM-yyyy";
                dtpFromDate.Format = DateTimePickerFormat.Custom;
                dtpToDate.CustomFormat = "dd-MMM-yyyy";
                dtpToDate.Format = DateTimePickerFormat.Custom;
                dtpFromDate.Value = Convert.ToDateTime(DT.Rows[0]["FromDate"]);
                dtpToDate.Value = Convert.ToDateTime(DT.Rows[0]["ToDate"]);
                toDate = Convert.ToDateTime(DT.Rows[0]["ToDate"]);
                fromDate = Convert.ToDateTime(DT.Rows[0]["FromDate"]);
                cmbToSite.SelectedValue = ((DT.Rows[0]["ToSiteId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT.Rows[0]["ToSiteId"]));
                if (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) != -1)
                {
                    cmbToSite.Enabled = false;
                }
                getLineDetails(Convert.ToInt32(lb_orderid.Text));
                recalculateTotal();

                cb_submit.Text = MessageContainerList.GetMessage(executionContext, "Update");
                cb_submit.Tag = "Update";

                if (searchorder_dgv["Status", index].Value.ToString() == PurchaseOrderDTO.PurchaseOrderStatus.OPEN)
                {
                    enableDisableFields("enable");
                    order_dgv.Enabled = true;
                    order_dgv.Columns["Description"].ReadOnly = true;
                    order_dgv.Columns["TaxAmount"].ReadOnly = true;
                    //order_dgv.ReadOnly = false;
                }
                else if (searchorder_dgv["Status", index].Value.ToString() == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS)
                {
                    enableDisableFields("enable");
                    order_dgv.ReadOnly = true;
                    order_dgv.Enabled = true;
                }
                else
                {
                    enableDisableFields("disable");
                }
                if (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.SHORTCLOSE || tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED || tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED || tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.DROPSHIP || fromDate > ServerDateTime.Now || toDate.Date < ServerDateTime.Now.Date)
                {
                    tb_status.Enabled = false;
                    cb_amendment.Enabled = false;
                    cb_submit.Enabled = false;
                    order_dgv.ReadOnly = true;
                    order_dgv.Enabled = true;
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    cb_vendor.Enabled = false;
                    lblViewRequisitions.Enabled = false;
                    return;
                }
                else
                {
                    tb_status.Enabled = true;
                    cb_amendment.Enabled = true;
                    cb_submit.Enabled = true;
                    lblViewRequisitions.Enabled = true;
                }

                if (!(ServerDateTime.Now > dtpFromDate.Value.Date) || !(ServerDateTime.Now < dtpToDate.Value.Date.AddDays(1)))
                {
                    tb_status.Enabled = false;
                    cb_amendment.Enabled = false;
                    cb_submit.Enabled = false;
                    order_dgv.Enabled = false;
                    dtpFromDate.Enabled = false;
                    dtpToDate.Enabled = false;
                    lblViewRequisitions.Enabled = false;
                    return;
                }

                if (lblDocumentStatus.Text == FinalStatus)
                {
                    cb_amendment.Enabled = true;
                    lblViewRequisitions.Enabled = false;


                    dtpToDate.Enabled = false;
                    tb_status.Enabled = true;
                }
                else
                {
                    cb_amendment.Enabled = false;
                    order_dgv.Enabled = true;
                    order_dgv.ReadOnly = false;
                    lblViewRequisitions.Enabled = true;
                    order_dgv.Columns["CancelledDate"].ReadOnly = true;
                    if (cmbDocumentType.Text == ContractPurchaseOrderType)
                    {
                        order_dgv.Columns["RequiredByDate"].ReadOnly = true;
                        order_dgv.Columns["Quantity"].ReadOnly = true;
                        tb_status.Enabled = false;
                        lblViewRequisitions.Enabled = false;
                    }
                    else
                    {
                        order_dgv.Columns["RequiredByDate"].ReadOnly = false;
                    }
                    dtpToDate.Enabled = true;
                    tb_status.Enabled = true;
                    orderStatus = tb_status.Text;
                }
                cb_vendor.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void getLineDetails(int OrderId)
        {
            log.LogMethodEntry(OrderId);
            try
            {
                SqlCommand cmd1 = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
                //Start update 20-May-2016
                //Updated query to add column received quantity in select statement
                cmd1.CommandText = @"select ItemCode,l.Description,l.Quantity,UnitPrice,UnitLogisticsCost,
                                        RequiredByDate,taxAmount,SubTotal,DiscountPercentage, 
                                    case when p.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' 
                                    then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) 
                                        else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, 
                                    LowerLimitCost, UpperLimitCost, CostVariancePercentage, l.PurchaseOrderLineId,
                                    isnull(l.isActive, 'Y') isActive, CancelledDate, r.purchaseorderlineid, 
                                    case  when r.purchaseorderlineid is null then 'Y' else 'N' end canInactivate, 
                                    isnull(ReceiveQuantity, 0) ReceiveQuantity, p.ProductId, isnull(p.MarketListItem, 0) MarketListItem,
                                    uom,isnull(l.UOMId, p.UOMId) as UOMId ,requisitionid, requisitionlineid, l.PriceInTickets
                                    ,l.PurchaseTaxId as TaxCode, p.taxInclusiveCost, tax_Percentage
                                from Product p 
                                 left outer join uom u on p.uomid = u.uomid
                                 left outer join Tax on Tax.tax_id = p.PurchaseTaxId, PurchaseOrder_Line l
                                 left outer join (select rl.purchaseorderlineid, sum(rl.quantity) ReceiveQuantity
												  from purchaseorderreceive_line rl
												  group by rl.purchaseorderlineid)r on l.purchaseorderlineid = r.purchaseorderlineid
                                where (l.PurchaseOrderId =@vorder_id) 
                                and p.ProductId = l.ProductId";
                //End update 20-May-2016  
                cmd1.Parameters.AddWithValue("@vorder_id", OrderId);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable DT1 = new DataTable();
                da1.Fill(DT1);
                da1.Dispose();

                order_dgv.Rows.Clear();
                for (int i = 0; i < DT1.Rows.Count; i++)
                {
                    InventoryList inventoryList = new InventoryList();
                    order_dgv.Rows.Add(DT1.Rows[i]["ItemCode"].ToString(),
                                        DT1.Rows[i]["Description"],
                                        DT1.Rows[i]["UOMId"],
                                        DT1.Rows[i]["Quantity"],
                                        DT1.Rows[i]["RequiredByDate"],
                                        DT1.Rows[i]["UnitPrice"],
                                        DT1.Rows[i]["UnitLogisticsCost"],
                                        DT1.Rows[i]["TaxCode"],
                                        DT1.Rows[i]["TaxAmount"],
                                        DT1.Rows[i]["DiscountPercentage"],
                                        DT1.Rows[i]["SubTotal"],
                                        DT1.Rows[i]["LowerLimitCost"],
                                        DT1.Rows[i]["UpperLimitCost"],
                                        DT1.Rows[i]["CostVariancePercentage"],
                                        DT1.Rows[i]["Cost"],
                                        DT1.Rows[i]["PurchaseOrderLineId"],
                                        DT1.Rows[i]["ReceiveQuantity"],
                                        inventoryList.GetProductStockQuantity(Convert.ToInt32(DT1.Rows[i]["ProductId"])),
                                        DT1.Rows[i]["CancelledDate"],
                                        DT1.Rows[i]["isActive"],
                                        DT1.Rows[i]["MarketListItem"],
                                        DT1.Rows[i]["ProductId"],
                                        DT1.Rows[i]["RequisitionId"],
                                        DT1.Rows[i]["RequisitionLineId"],
                                        DT1.Rows[i]["PriceInTickets"],
                                        DT1.Rows[i]["taxInclusiveCost"],  // from existing PO product price may be TaxInclusive
                                        DT1.Rows[i]["tax_Percentage"]
                                       );
                    if (cmbDocumentType.Text == ContractPurchaseOrderType)
                    {
                        order_dgv.Columns["Quantity"].ReadOnly = true;
                        order_dgv.Columns["RequiredByDate"].ReadOnly = true;
                    }
                    if (tb_status.Text != PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED && DT1.Rows[i]["RequiredByDate"] != DBNull.Value)
                        if (Convert.ToDateTime(DT1.Rows[i]["RequiredByDate"]) <= ServerDateTime.Now.Date)
                            order_dgv["RequiredByDate", i].Style.BackColor = Color.OrangeRed;

                    bool isLineActive = true;
                    if (order_dgv["isActive", i].Value != null && order_dgv["isActive", i].Value != DBNull.Value)
                    {
                        if (order_dgv["isActive", i].Value.ToString() == "N")
                            isLineActive = false;
                    }

                    if (!isLineActive)
                    {
                        order_dgv.Rows[i].ReadOnly = true;
                    }
                    else if (DT1.Rows[i]["canInactivate"].ToString() == "Y")
                    {
                        if (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS)
                        {
                            //Start Update 20-May-2016
                            //Added condition to check if whole line is received
                            //If order qty is greater than recv qty, qty update should be allowed
                            if (Convert.ToInt32(DT1.Rows[i]["ReceiveQuantity"]) == 0)
                            {
                                order_dgv["Code", i].ReadOnly = true;
                                order_dgv["isActive", i].ReadOnly = false;
                                order_dgv["Quantity", i].ReadOnly = false;
                                order_dgv["RequiredByDate", i].ReadOnly = false;
                                order_dgv["Cost", i].ReadOnly = false;
                                order_dgv["UnitLogisticsCost", i].ReadOnly = false;
                                order_dgv["TaxAmount", i].ReadOnly = false;
                                order_dgv["DiscountPercentage", i].ReadOnly = false;

                            }
                            else
                            {
                                order_dgv["isActive", i].ReadOnly = false;
                            }
                            //End Update 20-May-2016
                        }
                        if (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.OPEN)
                        {
                            order_dgv["isActive", i].ReadOnly = false;
                        }
                    }
                    else
                    {
                        if (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS)
                        {
                            //Start Update 20-May-2016
                            //Added condition to check if whole line is received
                            //If order qty is greater than recv qty, qty update should be allowed
                            if (Convert.ToInt32(DT1.Rows[i]["Quantity"]) > Convert.ToInt32(DT1.Rows[i]["ReceiveQuantity"]))
                            {
                                order_dgv["Code", i].ReadOnly = true;
                                order_dgv["isActive", i].ReadOnly = true;
                                order_dgv["Quantity", i].ReadOnly = false;
                                order_dgv["RequiredByDate", i].ReadOnly = false;
                                order_dgv["Cost", i].ReadOnly = false;
                                order_dgv["UnitLogisticsCost", i].ReadOnly = false;
                                order_dgv["TaxAmount", i].ReadOnly = false;
                                order_dgv["DiscountPercentage", i].ReadOnly = false;

                            }
                            else
                            {
                                order_dgv.Rows[i].ReadOnly = true;
                            }
                            //End Update 20-May-2016
                        }

                    }

                    int uomId = -1;
                    try
                    {
                        if (order_dgv["cmbUOM", i].Value != null)
                        {
                            uomId = Convert.ToInt32(order_dgv["cmbUOM", i].Value);
                        }
                        else
                        {
                            uomId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(order_dgv["ProductId", i].Value)).UomId;
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(order_dgv, i, uomId);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void enableDisableFields(String mode)
        {
            log.LogMethodEntry(mode);
            if (mode == "disable")
            {
                cb_vendor.Enabled = false;
                cb_autoorder.Enabled = false;
                order_dgv.ReadOnly = true;
                cb_amendment.Enabled = false;
                cb_submit.Enabled = false;
            }
            else //enable
            {
                cb_vendor.Enabled = true;
                cb_autoorder.Enabled = true;
                cb_submit.Enabled = true;
            }
            log.LogMethodExit();
        }

        private void cb_create_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if ((order_dgv.Rows.Count > 1))
                {
                    if (MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(856), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Select Order"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                if (isAutoPOMode)
                {
                    isAutoPOMode = false;
                }
                PrepareForNewPO();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void order_dgv_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                recalculateTotal();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void cb_print_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SqlTransaction SQLTrx = null;
                SqlConnection TrxCnn = null;


                if (tb_order.Text == "")
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(875));
                    log.LogMethodExit();
                    return;
                }

                purchaseOrderDTO = new PurchaseOrderDTO();
                PurchaseOrder purchaseOrder = new PurchaseOrder(Convert.ToInt32(lb_orderid.Text), executionContext);
                purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
                if (lblDocumentStatus.Text == "" || lblDocumentStatus.Text == "D")
                {
                    using (frmOrderPrint frm = new frmOrderPrint())
                    {
                        Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frm);
                        frm.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 25-Aug-2016
                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            if (SQLTrx == null)
                            {
                                TrxCnn = Semnox.Parafait.Inventory.CommonFuncs.Utilities.createConnection();
                                SQLTrx = TrxCnn.BeginTransaction();
                            }
                            try
                            {
                                if (frm.documentStatus == FinalStatus)
                                {
                                    purchaseOrderDTO.AmendmentNumber = lblAmendmentNumber.Text == "" ? 0 : Convert.ToInt32(lblAmendmentNumber.Text);
                                    purchaseOrderDTO.DocumentStatus = frm.documentStatus == null ? FinalStatus : frm.documentStatus;
                                    purchaseOrderDTO.ReprintCount = 0;
                                    lblDocumentStatus.Text = frm.documentStatus;
                                    bool isSaveSuccessful = false;
                                    try
                                    {
                                        //PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                                        purchaseOrder.Save(SQLTrx);
                                        isSaveSuccessful = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                        tb_cancelledDate.Text = "";
                                        MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(869), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Update Order"));
                                        POSearch();
                                    }
                                    if (isSaveSuccessful)
                                    {
                                        if (purchaseOrderDTO.OrderStatus.Equals("Drop Ship") && purchaseOrderDTO.FromSiteId == Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.SiteId)
                                        {
                                            executionContext.SetSiteId(purchaseOrderDTO.ToSiteId);
                                            PurchaseOrderLineList purchaseOrderLineList = new PurchaseOrderLineList();
                                            List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>> poLineSearchParams;
                                            poLineSearchParams = new List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>>();
                                            poLineSearchParams.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_IDS, purchaseOrderDTO.PurchaseOrderId.ToString()));
                                            poLineSearchParams.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.IS_ACTIVE, "Y"));
                                            List<PurchaseOrderLineDTO> purchaseOrderLineListDTO = purchaseOrderLineList.GetAllPurchaseOrderLine(poLineSearchParams, SQLTrx);
                                            if (purchaseOrderDTO.PurchaseOrderId > -1)
                                            {
                                                purchaseOrderDTO.PurchaseOrderLineListDTO = purchaseOrderLineListDTO;
                                                PurchaseOrder purchaseOrderBL = new PurchaseOrder(purchaseOrderDTO, executionContext);
                                                purchaseOrderBL.ProcessDropShipRequest(Semnox.Parafait.Inventory.CommonFuncs.Utilities, SQLTrx);
                                            }
                                        }
                                    }
                                    order_dgv.Enabled = false;
                                    dtpToDate.Enabled = false;
                                    lblViewRequisitions.Enabled = false;
                                    cb_amendment.Enabled = true;
                                }
                                SQLTrx.Commit();
                            }
                            catch (SqlException ex)
                            {
                                log.Error(ex);
                                SQLTrx.Rollback();
                                MessageBox.Show(ex.Message);
                                log.LogMethodExit();
                                return;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                                SQLTrx.Rollback();
                                throw;
                            }
                        }
                        else
                        {
                            log.LogMethodExit();
                            return;
                        }
                    }
                }
                else
                {
                    try
                    {
                        purchaseOrderDTO.AmendmentNumber = lblAmendmentNumber.Text == "" ? 0 : Convert.ToInt32(lblAmendmentNumber.Text);
                        purchaseOrderDTO.DocumentStatus = lblDocumentStatus.Text;
                        purchaseOrderDTO.ReprintCount = Convert.ToInt32(lblReprintCount.Text == "" ? "0" : lblReprintCount.Text) + 1;
                        try
                        {
                            purchaseOrder.Save(null);
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            tb_cancelledDate.Text = "";
                            MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(869), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Update Order"));
                            POSearch();
                        }
                        lblReprintCount.Text = (Convert.ToInt32(lblReprintCount.Text == "" ? "0" : lblReprintCount.Text) + 1).ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        log.LogMethodExit();
                        return;
                    }
                }
                try
                {
                    reportFileName = GetInventoryReport();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(1819, ex.Message));//Error while printing the document. Error message: &1
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
                log.LogMethodExit();
                return;
            }
        }

        private string GetInventoryReport()
        {
            log.LogMethodEntry();
            List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
            reportParam.Add(new clsReportParameters.SelectedParameterValue("PurchaseOrderId", Convert.ToInt32(lb_orderid.Text)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("TicketCost", Convert.ToString(ticketCost)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("TotalCost", Convert.ToString(tb_total.Text)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("TotalRetail", Convert.ToString( tb_TotalRetail.Text)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("TotalLandedCost", Convert.ToString(tb_TotalLandedCost.Text)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("TotalLogisticsCost", Convert.ToString(tb_totalLogisticsCost.Text)));
            reportParam.Add(new clsReportParameters.SelectedParameterValue("MarkupPercent", Convert.ToString(tb_MarkupPercent.Text)));
            ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryPurchaseOrderReceipt", "", null, null, reportParam, "P");
            reportFileName = receiptReports.GenerateAndPrintReport(backgroundMode);
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }

        private bool SetupThePrinting(PrintDocument MyPrintDocument)
        {
            log.LogMethodEntry(MyPrintDocument);
            try
            {
                PrintDialog MyPrintDialog = new PrintDialog();
                MyPrintDialog.AllowCurrentPage = false;
                MyPrintDialog.AllowPrintToFile = false;
                MyPrintDialog.AllowSelection = false;
                MyPrintDialog.AllowSomePages = false;
                MyPrintDialog.PrintToFile = false;
                MyPrintDialog.ShowHelp = false;
                MyPrintDialog.ShowNetwork = false;
                MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;
                MyPrintDocument.DocumentName = utilities.MessageUtils.getMessage("PurchaseOrderDetails" + "-" + lb_orderid.Text);
                MyPrintDialog.UseEXDialog = true;
                if (backgroundMode)
                {
                    log.Debug("Email Mode");
                }
                else if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
                MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
                MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(true);
            return true;
        }

        private void frm_Order_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            txtProductPOSearch.Focus();
            log.LogMethodExit();
        }

        //For Barcode
        private void serialbarcodeDataReceived()
        {
            log.LogMethodEntry();
            try
            {
                scannedBarcode = BarcodeReader.Barcode;
                if ((scannedBarcode != "") && (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.OPEN))
                {
                    if (txtProductPOSearch.Focused)
                    {
                        txtProductPOSearch.Text = scannedBarcode;
                        cb_search.PerformClick();
                    }
                    else if (txt_prodcode.Focused)
                    {
                        txt_prodcode.Text = scannedBarcode;
                        cb_prodsearch.PerformClick();
                    }
                    else
                    {
                        DataTable DT = new DataTable();
                        SqlCommand cmd = Semnox.Parafait.Inventory.CommonFuncs.Utilities.getCommand();
                        string condition = "";
                        if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.IsCorporate)
                        {
                            condition = " and (Product.site_id = @site_id or @site_id = -1)";
                            cmd.Parameters.AddWithValue("@site_id", Semnox.Parafait.Inventory.CommonFuncs.getSiteid());
                        }
                        //Update query to search barcode rom productbarcode table
                        //23-Mar-2016
                        cmd.CommandText = "select  Code, Description, ReorderQuantity, " +
                                        "case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, " +
                                        "case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/100 * isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else 0 end as taxAmount, " +
                                        "(case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then cost else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) * ReorderQuantity as SubTotal, " +
                                        "LowerLimitCost, UpperLimitCost, CostVariancePercentage, Product.ProductId, uom ,product.PurchaseTaxId as TaxCode" +
                                " from Product left outer join Tax on Tax.tax_Id = product.PurchaseTaxId " +
                                " left outer join uom on product.uomid = uom.uomid " +
                                " left outer join (select * " +
                                                    "from (" +
                                                            "select *, row_number() over(partition by productid order by productid) as num " +
                                                                                 "from productbarcode " +
                                                                                 "where BarCode = @barcode and isactive = 'Y')v " +
                                                    "where num = 1) b on Product.productid = b.productid " +
                                "where b.BarCode = @barcode AND Product.IsActive = 'Y' and isPurchaseable = 'Y' " + condition;
                        //23-Mar-2016
                        cmd.Parameters.AddWithValue("@barcode", scannedBarcode);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(DT);
                        da.Dispose();

                        if (DT.Rows.Count < 1)
                        {
                            MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(877), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Error"));
                        }
                        else if (DT.Rows.Count == 1)
                        {
                            fireCellValueChange = false;
                            int rowindex = order_dgv.Rows.Add(DT.Rows[0]["Code"],
                                                DT.Rows[0]["Description"],
                                                DT.Rows[0]["uom"],
                                                DT.Rows[0]["ReorderQuantity"],
                                                DBNull.Value,
                                                DT.Rows[0]["Cost"],
                                                DT.Rows[0]["TaxCode"],
                                                DT.Rows[0]["TaxAmount"],
                                                0,
                                                DT.Rows[0]["SubTotal"],
                                                DT.Rows[0]["LowerLimitCost"],
                                                DT.Rows[0]["UpperLimitCost"],
                                                DT.Rows[0]["CostVariancePercentage"],
                                                DT.Rows[0]["Cost"],
                                                DT.Rows[0]["ProductId"],
                                                DBNull.Value,
                                                DBNull.Value
                                );
                            order_dgv["Code", rowindex].ReadOnly = true;
                            fireCellValueChange = true;
                        }
                        else
                        {
                            MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(859), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Error"));
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

        private void btnReceipts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (lb_orderid.Text == "")
            {
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(835), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("View Receipts"));
                tb_searchorder.Focus();
                log.LogMethodExit();
                return;
            }
            int order_id = Convert.ToInt32(lb_orderid.Text);

            frmOrderReceipts frm = new frmOrderReceipts(order_id);
            Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frm);//Added to style GUI on 09-Sep-2016
            frm.Text = frm.Text + " " + tb_order.Text;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.ShowDialog();
            log.LogMethodExit();
        }


        bool backgroundMode = false;
        private void btnEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (tb_order.Text == "")
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(665));
                    log.LogMethodExit();
                    return;
                }
                backgroundMode = true;
                cb_print.PerformClick();
                string subject = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.SiteName + Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Purchase Order No") + " " + tb_order.Text + " for " + cb_vendor.Text;
                backgroundMode = false;
                string body = "Hi " + tb_contact.Text + "," + Environment.NewLine + Environment.NewLine;
                body += "Please find attached Purchase Order number " + tb_order.Text + " for your immediate processing." + Environment.NewLine + Environment.NewLine;
                body += "Regards," + Environment.NewLine + Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.Username;
                SendEmailUI emailF = new SendEmailUI(lblEmail.Text, "", "", subject, body, reportFileName, null, false, Semnox.Parafait.Inventory.CommonFuncs.Utilities);
                Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(emailF);//Added to style GUI on 09-Sep-2016
                emailF.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                emailF.BringToFront();
                emailF.ShowDialog();

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            try
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(reportFileName);
                fi.Delete();
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void searchorder_dgv_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            searchorder_dgv_selectionChanged();
            log.LogMethodExit();
        }

        private void order_dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex == -1 || e.RowIndex == -1)
                    return;
                if (order_dgv.Columns[e.ColumnIndex].Name == "Cost" || order_dgv.Columns[e.ColumnIndex].Name == "DiscountPercentage")
                {
                    decimal lowerLimit = Convert.ToDecimal(order_dgv["LowerLimitCost", e.RowIndex].Value == DBNull.Value ? 0 : order_dgv["LowerLimitCost", e.RowIndex].Value);
                    decimal upperLimit = Convert.ToDecimal(order_dgv["UpperLimitCost", e.RowIndex].Value == DBNull.Value ? decimal.MaxValue : order_dgv["UpperLimitCost", e.RowIndex].Value);
                    decimal costVariance = Convert.ToDecimal(order_dgv["CostVariancePercent", e.RowIndex].Value == DBNull.Value ? -1 : order_dgv["CostVariancePercent", e.RowIndex].Value);
                    decimal cost;
                    if (order_dgv.Columns[e.ColumnIndex].Name == "Cost")
                        cost = Convert.ToDecimal((e.FormattedValue == null || e.FormattedValue.ToString() == "" || e.FormattedValue == DBNull.Value) ? 0 : e.FormattedValue);
                    else
                        cost = Convert.ToDecimal(order_dgv["Cost", e.RowIndex].Value == DBNull.Value ? -1 : order_dgv["Cost", e.RowIndex].Value);
                    decimal origcost = Convert.ToDecimal(order_dgv["OrigPrice", e.RowIndex].Value == DBNull.Value ? 0 : order_dgv["OrigPrice", e.RowIndex].Value);
                    bool marketListItem = Convert.ToBoolean(order_dgv["MarketListItem", e.RowIndex].Value == DBNull.Value ? false : order_dgv["MarketListItem", e.RowIndex].Value);


                    if (lowerLimit == 0 && upperLimit == decimal.MaxValue)
                    {
                        if (costVariance != -1)
                        {
                            if (cost < origcost * (1 - costVariance / 100) || cost > origcost * (1 + costVariance / 100))
                            {
                                if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.Manager_Flag == "Y")
                                {
                                    if (MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(860, costVariance.ToString(), origcost.ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT)), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                                        e.Cancel = true;
                                }
                                else
                                {
                                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(861, costVariance.ToString(), origcost.ToString(Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT)), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    if (!Authenticate.Manager())
                                        e.Cancel = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (lowerLimit > 0 && upperLimit > 0)
                        {
                            if (cost < lowerLimit || cost > upperLimit)
                            {
                                if (Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.Manager_Flag == "Y")
                                {
                                    if (MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(862, lowerLimit.ToString(), upperLimit.ToString()), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                                        e.Cancel = true;
                                }
                                else
                                {
                                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(863, lowerLimit.ToString(), upperLimit.ToString()), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    if (!Authenticate.Manager())
                                        e.Cancel = true;
                                }
                            }
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

        private void order_dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Row.Cells[12].Value == null || e.Row.Cells[12].Value == DBNull.Value)
                return;

            try
            {
                Semnox.Parafait.Inventory.CommonFuncs.Utilities.executeNonQuery("delete from PurchaseOrder_line where PurchaseOrderLineId = @id",
                                                        new SqlParameter[] { new SqlParameter("@id", e.Row.Cells[12].Value) });
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        //Start update 21-Oct-2016
        //Added method
        bool POLinesExist()
        {
            log.LogMethodEntry();
            bool retVal = false;
            try
            {
                if (order_dgv.Rows.Count > 0)
                {
                    for (int j = 0; j < order_dgv.Rows.Count; j++)
                    {
                        if (order_dgv.Rows[0].Cells["Code"].Value != null && order_dgv.Rows[0].Cells["Code"].Value != DBNull.Value)
                        {
                            retVal = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retVal);
            return retVal;
        }
        //End update 21-Oct-2016

        private void cmbDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int index = Convert.ToInt32(cmbDocumentType.SelectedIndex);
                order_dgv["TaxAmount", dgvselectedindex].ReadOnly = false;
                order_dgv["TaxCode", dgvselectedindex].ReadOnly = false;
                order_dgv["Quantity", dgvselectedindex].ReadOnly = false;
                order_dgv["RequiredByDate", dgvselectedindex].ReadOnly = false;
                order_dgv["Cost", dgvselectedindex].ReadOnly =
                    order_dgv["TaxAmount", dgvselectedindex].ReadOnly =
                    order_dgv["DiscountPercentage", dgvselectedindex].ReadOnly =
                    order_dgv["UnitLogisticsCost", dgvselectedindex].ReadOnly = false;
                if (index > 0)
                {
                    cmbDocumentType.Enabled = false;
                    if (cmbDocumentType.Text == ContractPurchaseOrderType)
                    {
                        tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
                        tb_status.Enabled = false;
                        lblViewRequisitions.Enabled = false;
                        order_dgv["TaxAmount", dgvselectedindex].ReadOnly = true;
                        order_dgv["TaxCode", dgvselectedindex].ReadOnly = true;
                        order_dgv["Quantity", dgvselectedindex].ReadOnly = true;
                        order_dgv["RequiredByDate", dgvselectedindex].ReadOnly = true;
                        order_dgv["Cost", dgvselectedindex].ReadOnly =
                            order_dgv["TaxAmount", dgvselectedindex].ReadOnly =
                            order_dgv["DiscountPercentage", dgvselectedindex].ReadOnly =
                            order_dgv["UnitLogisticsCost", dgvselectedindex].ReadOnly = true;
                    }
                    dtpFromDate.Enabled = true;
                    dtpToDate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void tb_status_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ComboBox cb = (ComboBox)sender;
                if (!cb.Focused)
                {
                    log.LogMethodExit();
                    return;
                }
                string selectedStatus = tb_status.Text;
                if (orderStatus == PurchaseOrderDTO.PurchaseOrderStatus.OPEN && (selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.SHORTCLOSE || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED))
                {
                    tb_status.Text = orderStatus;
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1060, orderStatus, selectedStatus), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Order status update"));
                    log.LogMethodExit();
                    return;
                }
                if (orderStatus == PurchaseOrderDTO.PurchaseOrderStatus.DROPSHIP && (selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.OPEN || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.SHORTCLOSE || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED))
                {
                    tb_status.Text = orderStatus;
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1060, orderStatus, selectedStatus), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Order status update"));
                    log.LogMethodExit();
                    return;
                }
                if (orderStatus == PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS && (selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.OPEN || selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED))
                {
                    tb_status.Text = orderStatus;
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1060, orderStatus, selectedStatus), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Order status update"));
                    log.LogMethodExit();
                    return;
                }
                if (orderStatus != PurchaseOrderDTO.PurchaseOrderStatus.DROPSHIP && (selectedStatus == PurchaseOrderDTO.PurchaseOrderStatus.DROPSHIP))
                {
                    tb_status.Text = orderStatus;
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1060, orderStatus, selectedStatus), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Order status update"));
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void cb_amendment_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                cb_submit.Enabled = true;
                order_dgv.Enabled = true;
                lblViewRequisitions.Enabled = true;
                tb_status.Enabled = true;
                if (lblDocumentStatus.Text == FinalStatus)
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1059), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage("Amendment"));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        PurchaseOrderDTO getPODetails(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            try
            {
                PurchaseOrderList purchaseOrderList = new PurchaseOrderList(executionContext);
                List<PurchaseOrderDTO> purchaseOrderListDTO = new List<PurchaseOrderDTO>();
                List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> POSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                POSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, lb_orderid.Text));
                purchaseOrderListDTO = purchaseOrderList.GetAllPurchaseOrder(POSearchParams, executionContext.GetUserId(), SQLTrx);//purchaseOrderDataHandler.GetPurchaseOrderList(POSearchParams, executionContext.GetUserId())[0];
                return purchaseOrderListDTO[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
                log.LogMethodExit(purchaseOrderDTO);
                return purchaseOrderDTO;
            }
        }

        private void lnkRemarks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (lb_orderid.Text == "")
            {
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1061));
                log.LogMethodExit();
                return;
            }
            else
            {
                inventoryNotesUI = new frmInventoryNotes(Convert.ToInt32(lb_orderid.Text), "PurchaseOrder", Semnox.Parafait.Inventory.CommonFuncs.Utilities);
                Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(inventoryNotesUI);//Added to style GUI on 09-Sep-2016
                inventoryNotesUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                inventoryNotesUI.ShowDialog();
            }
            log.LogMethodExit();
        }

        //Begin: Modification done for removing side Order button on Form Close on 25-Aug-2016
        private void frm_Order_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }
        //End: Modification done for removing side Order button on Form Close on 25-Aug-2016
        private void searchorder_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                searchorder_dgv_selectionChanged();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lblViewRequisitions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            bool marketListOrder = marketListItemExists();
            if (marketListOrder)
            {
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1100));
                log.LogMethodExit();
                return;
            }
            frmListRequisitions pr = new frmListRequisitions(Semnox.Parafait.Inventory.CommonFuncs.Utilities, "PO", ((cmbToSite.SelectedValue == null) ? -1 : Convert.ToInt32(cmbToSite.SelectedValue)));
            Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(pr);//Added to Style GUI Design on 15-Sep-2016
            pr.StartPosition = FormStartPosition.CenterScreen;//Added to show at center of screen on 15-Sep-2016

            if (pr.SelectedRequisitionId != -1 || pr.ShowDialog() != DialogResult.Cancel)
            {
                int requisitionId = (int)(pr.SelectedRequisitionId);
                PurchaseOrderDTO purchaseOrderDTO = new PurchaseOrderDTO();
                PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                DataTable DT = new DataTable();
                InventoryDocumentTypeList inventoryDocumentTypeList = null;
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = null;
                if (pr.SelectedRequisitionDTO != null)
                {
                    inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                    inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(pr.SelectedRequisitionDTO.RequisitionType);
                }
                if (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) != -1 && Convert.ToInt32(cmbToSite.SelectedValue) != pr.SelectedRequisitionDTO.FromSiteId)
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1543));
                    log.LogMethodExit();
                    return;
                }
                if (lblViewRequisitions.Tag != null && !lblViewRequisitions.Tag.Equals(inventoryDocumentTypeDTO.Code))
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1544));
                    log.LogMethodExit();
                    return;
                }
                lblViewRequisitions.Tag = inventoryDocumentTypeDTO.Code;
                if (inventoryDocumentTypeDTO != null && inventoryDocumentTypeDTO.Code.Equals("ITRQ"))
                {
                    DT = purchaseOrder.GetPORecord(requisitionId, -1, "ITRQ", null);
                    cmbToSite.SelectedValue = pr.SelectedRequisitionDTO.FromSiteId;
                    cmbToSite.Enabled = false;
                }
                else
                {
                    DT = purchaseOrder.GetPORecord(requisitionId, ((Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.IsCorporate) ? Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.SiteId : -1), "", null);

                }
                if (DT.Rows.Count == 0)
                {
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1101));
                    log.LogMethodExit();
                    return;
                }

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    fireCellValueChange = false;
                    int rowindex = order_dgv.Rows.Add();
                    order_dgv["Code", rowindex].Value = DT.Rows[i]["Code"];
                    order_dgv["Description", rowindex].Value = DT.Rows[i]["Description"];
                    //order_dgv["uom", rowindex].Value = DT.Rows[i]["uom"];
                    order_dgv["cmbUOM", rowindex].Value = DT.Rows[i]["UOMId"];
                    int uomId = Convert.ToInt32(order_dgv["cmbUOM", rowindex].Value);
                    if (cmbToSite.SelectedValue != null && Convert.ToInt32(Convert.ToInt32(cmbToSite.SelectedValue)) != CommonFuncs.Utilities.ParafaitEnv.SiteId)
                    {
                        List<int> childUOMIdList = new List<int>();
                        string uomIdList = string.Empty;
                        UOMConversionFactorListBL uOMConversionListBL = new UOMConversionFactorListBL(executionContext);
                        List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> searchByParams = new List<System.Collections.Generic.KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
                        searchByParams.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.SITE_ID, cmbToSite.SelectedValue.ToString()));
                        List<UOMConversionFactorDTO> uomConversionDTOList = uOMConversionListBL.GetUOMConversionFactorDTOList(searchByParams);
                        if (uomConversionDTOList == null || uomConversionDTOList.Any() == false)
                        {
                            childUOMIdList.Add(uomId);
                            uomIdList = childUOMIdList[0].ToString();
                        }
                        else
                        {
                            List<UOMConversionFactorDTO> childDTOList = uomConversionDTOList.FindAll(x => x.BaseUOMId == uomId | x.UOMId == uomId).ToList();
                            childUOMIdList = childDTOList.Select(id => id.UOMId).ToList();
                            childUOMIdList.Add(uomId);
                            childUOMIdList.AddRange(childDTOList.Select(id => id.BaseUOMId).ToList());
                            childUOMIdList = childUOMIdList.Distinct().ToList();
                            uomIdList = string.Join(",", childUOMIdList);
                        }
                        //lotIdList = string.Join(",", uomConversionDTOList.Select(x => x.BaseUOMId));
                        UOMList uOMList = new UOMList(executionContext);
                        List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchbyUOMParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                        searchbyUOMParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.UOMID_LIST, uomIdList));
                        searchbyUOMParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, cmbToSite.SelectedValue.ToString()));
                        List<UOMDTO> uomDTOList = uOMList.GetAllUOMs(searchbyUOMParams);

                        if (uomDTOList != null && uomDTOList.Any())
                        {
                            cmbUOM.DataSource = uomDTOList;
                            cmbUOM.ValueMember = "UOMId";
                            cmbUOM.DisplayMember = "UOM";
                            order_dgv.Rows[rowindex].Cells["cmbUOM"].Value = uomId;
                        }
                    }
                    else
                    {
                        CommonFuncs.GetUOMComboboxForSelectedRows(order_dgv, rowindex, uomId);
                    }
                    if (cmbDocumentType.Text == ContractPurchaseOrderType)
                    {
                        order_dgv["Quantity", rowindex].Value = 1;
                        order_dgv["Quantity", rowindex].ReadOnly = true;
                        order_dgv["RequiredByDate", rowindex].Value = null;
                        order_dgv["SubTotal", rowindex].Value = DT.Rows[i]["CPOSubTotal"];
                    }
                    else
                    {
                        order_dgv["Quantity", rowindex].Value = DT.Rows[i]["RequestedQuantity"];
                        order_dgv["Quantity", rowindex].ReadOnly = true;
                        order_dgv["SubTotal", rowindex].Value = DT.Rows[i]["SubTotal"];
                    }
                    order_dgv["Cost", rowindex].Value = DT.Rows[i]["Cost"];
                    order_dgv["TaxAmount", rowindex].Value = DT.Rows[i]["TaxAmount"];
                    order_dgv["LowerLimitCost", rowindex].Value = DT.Rows[i]["LowerLimitCost"];
                    order_dgv["UpperLimitCost", rowindex].Value = DT.Rows[i]["UpperLimitCost"];
                    order_dgv["CostVariancePercent", rowindex].Value = DT.Rows[i]["CostVariancePercentage"];
                    order_dgv["MarketListItem", rowindex].Value = DT.Rows[i]["MarketListItem"];
                    order_dgv["ProductId", rowindex].Value = DT.Rows[i]["ProductId"];
                    order_dgv["RequisitionId", rowindex].Value = DT.Rows[i]["RequisitionId"];
                    order_dgv["RequisitionLineId", rowindex].Value = DT.Rows[i]["RequisitionLineId"];
                    fireCellValueChange = true;
                }
            }
            log.LogMethodExit();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DateTimePicker dtp = (DateTimePicker)sender;
                if (!dtp.Focused)
                {
                    log.LogMethodExit();
                    return;
                }

                dtpToDate.CustomFormat = "dd-MMM-yyyy";
                dtpToDate.Format = DateTimePickerFormat.Custom;
                if (dtpToDate.Value < toDate)
                {
                    //dtpToDate.Value = toDate;
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1094), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                    return;
                }
                if (dtpToDate.Value <= dtpFromDate.Value)
                {
                    //dtpToDate.Value = toDate; // message displays twice since values change triggers event again
                    MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1093), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                    log.LogMethodExit();
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DateTimePicker dtp = (DateTimePicker)sender;
            if (!dtp.Focused)
            {
                log.LogMethodExit();
                return;
            }

            dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            dtpFromDate.Format = DateTimePickerFormat.Custom;


            if (dtpToDate.Value < fromDate)
            {
                dtpToDate.Value = fromDate;
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1094), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                log.LogMethodExit();
                return;
            }
        }

        private void dtpFromDate_CloseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtpFromDate.CustomFormat = "dd-MMM-yyyy";
            dtpFromDate.Format = DateTimePickerFormat.Custom;

            if (dtpToDate.Value < fromDate)
            {
                dtpToDate.Value = fromDate;
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1094), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                log.LogMethodExit();
                return;
            }
        }

        private void dtpToDate_CloseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DateTimePicker dtp = (DateTimePicker)sender;
            if (!dtp.Focused)
            {
                log.LogMethodExit();
                return;
            }

            dtpToDate.CustomFormat = "dd-MMM-yyyy";
            dtpToDate.Format = DateTimePickerFormat.Custom;
            if (dtpToDate.Value < toDate)
            {
                dtpToDate.Value = toDate;
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1094), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                log.LogMethodExit();
                return;
            }
            if (dtpToDate.Value <= dtpFromDate.Value)
            {
                dtpToDate.Value = toDate;
                MessageBox.Show(Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1093), Semnox.Parafait.Inventory.CommonFuncs.Utilities.MessageUtils.getMessage(1095));
                log.LogMethodExit();
                return;
            }
        }

        private void cmbToSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (!cmbToSite.Text.Equals("Semnox.Parafait.Site.SiteDTO"))
                {
                    if (lblViewRequisitions.Tag != null && !lblViewRequisitions.Tag.Equals("ITRQ") && (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) > -1))
                    {
                        cmbToSite.SelectedValue = -1;
                    }
                    if (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) > -1)
                    {
                        tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.DROPSHIP;
                    }
                    else
                    {
                        tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void lblViewTotalTax_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                CalculateTotalTaxAmount();
                int i = 0;
                if (order_dgv.Rows.Count >= 0 && Convert.ToDecimal(txtTotalTax.Text) > 0) //show only when tax is >0
                {

                    PurchaseOrderLine purchaseOrderLine = new PurchaseOrderLine();
                    PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(executionContext);
                    List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                    string description = "Total Tax";
                    for (i = 0; i < order_dgv.Rows.Count - 1; i++) //  count -1 : To skip the last empty row of order_dgv grid 
                    {
                        if (order_dgv["TaxAmount", i].Value != null && !String.IsNullOrEmpty(order_dgv["TaxAmount", i].Value.ToString()))
                        {
                            int taxId = (order_dgv.Rows[i].Cells["TaxCode"].Value == DBNull.Value ? 0 : Convert.ToInt32(order_dgv.Rows[i].Cells["TaxCode"].Value));
                            decimal taxAmount = Convert.ToDecimal(order_dgv["Quantity", i].Value.ToString()) * Convert.ToDecimal(order_dgv["TaxAmount", i].Value.ToString());
                            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOListForLine = purchaseOrderTaxLineBL.GetTaxLines(taxId, 0, 0, taxAmount, 0);
                            if (purchaseOrderTaxLineDTOListForLine != null && purchaseOrderTaxLineDTOListForLine.Count > 0)
                                purchaseOrderTaxLineDTOList.AddRange(purchaseOrderTaxLineDTOListForLine);
                        }
                    }
                    using (frmTaxPopUp taxPopup = new frmTaxPopUp(description, Semnox.Parafait.Inventory.CommonFuncs.Utilities, purchaseOrderTaxLineDTOList))
                    {
                        taxPopup.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void order_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.ColumnIndex == -1 || e.RowIndex == -1)
                    return;
                if (e.ColumnIndex == 8 && order_dgv["TaxCode", e.RowIndex].Value != null && !String.IsNullOrEmpty(order_dgv["TaxCode", e.RowIndex].Value.ToString()))
                {
                    int taxId = Convert.ToInt32(order_dgv.Rows[e.RowIndex].Cells["TaxCode"].Value);
                    decimal taxamount = (order_dgv["TaxAmount", e.RowIndex].Value == null ? 0 : Convert.ToDecimal(order_dgv["TaxAmount", e.RowIndex].Value));
                    if (taxId >= 0 && order_dgv.Rows.Count > 0 && order_dgv["TaxAmount", e.RowIndex].Value != null)
                    {
                        string description = order_dgv["Code", e.RowIndex].Value.ToString();
                        PurchaseOrderLine purchaseOrderLine = new PurchaseOrderLine();
                        PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(executionContext);
                        List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                        purchaseOrderTaxLineDTOList = purchaseOrderTaxLineBL.GetTaxLines(taxId, 0, 0, taxamount, 0);
                        using (frmTaxPopUp taxPopup = new frmTaxPopUp(description, Semnox.Parafait.Inventory.CommonFuncs.Utilities, purchaseOrderTaxLineDTOList))
                        {
                            taxPopup.ShowDialog();
                        }
                    }

                }
                else
                {
                    order_dgv["TaxAmount", e.RowIndex].ReadOnly = false;
                    order_dgv["TaxAmount", e.RowIndex].ReadOnly = false;
                }

                if (order_dgv.Columns[e.ColumnIndex].Name == "stockLink")
                {
                    using (Semnox.Parafait.Inventory.frmInventoryStockDetails frmStockDetails = new Semnox.Parafait.Inventory.frmInventoryStockDetails(executionContext, Convert.ToInt32(order_dgv["ProductId", order_dgv.CurrentRow.Index].Value)))
                    {
                        Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frmStockDetails);
                        frmStockDetails.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void PrepareForNewPO()
        {
            log.LogMethodEntry();
            ClearFields();
            enableDisableFields("enable");
            cb_submit.Tag = "Save";
            cb_submit.Text = MessageContainerList.GetMessage(executionContext, "Save PO");
            cb_amendment.Enabled = false;
            order_dgv.ReadOnly = false;
            SetOrderDGVColumnReadStatus();
            cmbToSite.Enabled = true;
            lblViewRequisitions.Enabled = true;
            log.LogMethodExit();
        }

        private void order_dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Exception != null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(585, Name, e.RowIndex + 1, order_dgv.Columns[e.ColumnIndex].DataPropertyName) + ": ");
            }
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void SetOrderDGVColumnReadStatus()
        {
            log.LogMethodEntry();
            order_dgv.Columns["TaxAmount"].ReadOnly = true;
            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities.getParafaitDefaults("ALLOW_PRICE_UPDATE_IN_PO") == "N")
            {
                order_dgv.Columns["Cost"].ReadOnly = order_dgv.Columns["TaxAmount"].ReadOnly = order_dgv.Columns["TaxCode"].ReadOnly = true;
            }
            log.LogMethodExit();
        }

    }
}
