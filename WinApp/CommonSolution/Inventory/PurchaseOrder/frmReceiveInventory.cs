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
 *2.00        23-May-2016            Soumya                    Updated query to add isnull 
 *                                                             condition while considering validity 
 *                                                             condition while considering validity 
 *                                                             of a PO line.
*
 ********************************************************************************************
 *2.00        02-Jun-2016            Soumya                    Updated receive_dgv_CellContentClick 
 *                                                             to see that order receive does not 
 *                                                             happen if receive quantity is more
 *                                                             than order quantity.
********************************************************************************************
 *2.00        03-Jun-2016            Soumya                    Added if statement in 
 *                                                             receive_dgv_CellContentClick function
 *                                                             to see there should be no check on 
 *                                                             if order_qty < receive_qty in case
 *                                                             of auto order.
********************************************************************************************
 *3.00        11-Feb-2019            Archana                   Redemption gift search and Inventory UI changes.
********************************************************************************************
* 2.60        08-Apr-2019            Girish K                   Replaced PurchaseTax 3 tier with Tax 3 Tier,
*                                                               TaxAmount Column is added to the Grid.
*2.70         28-Jun-2019            Archana                    Modified: Inventory stock and vendor search in PO
*                                                               and receive screen change 
*2.70.2         20-Aug-2019            Archana                    Auto PO changes   
*2.70.2       27-Nov-2019             Girish Kundar               Modified: Issue fix - Quantity validation pop up issue fix 
*2.70.2       18-Dec-2019             Jinto Thomas                Added parameter execution context for userrolrbl declaration with userid 
*2.80         18-May-2020             Laster Menezes            Exception handling while printing InventoryReceiveReceipt
*2.90.0       20-May-2020            Deeksha                    Modified :Bulk product publish for inventory products & weighted average costing changes.
*2.100.0      17-Sep-2020            Deeksha                    Modified to add UOM dropdown field to change related UOM option.
*2.100.1      29-Jun-2021            Deeksha                    Modified :Issue Fix -Tax Inclusive products price calculation Issue.
*2.120        15-Apr-2021            Laster Menezes             Modified GetInventoryReport method to use receipt framework of reports for InventoryReceiveReceipt generation
*2.120.1      19-Jul-2021             Deeksha                  Modified :Inventory Recieve screen resolution Issue Fix
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Semnox.Parafait.Vendor;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.JobUtils;
using System.Linq;
using System.Drawing.Printing;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public partial class frmReceiveInventory : Form
    {
        PurchaseOrderDTO purchaseOrderDTO;
        PurchaseOrderLineDTO purchaseOrderLineDTO;
        PurchaseOrder purchaseOrder;
        PurchaseOrderLine purchaseOrderLine;
        InventoryReceiptDTO inventoryReceiptDTO;
        InventoryReceiptsBL inventoryReceipt;
        InventoryReceiveLinesDTO inventoryReceiveLinesDTO;
        InventoryReceiveLinesBL inventoryReceiveLines;
        InventoryDTO inventoryDTO;
        Inventory inventory;
        ProductBL productBl;
        TaxList purchaseTaxList;
        // ParafaitEnv ParafaitEnv;
        List<InventoryLotDTO> inventoryLotListDTO;
        InventoryLotDTO inventoryLotDTO;

        DataTable DT_receive = new DataTable();
        DataTable DT_search = new DataTable();
        //DataTable DT_POSearch = new DataTable();

        bool receiveValidationResult = true;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        bool fireCellValueChange = true;
        int dgvselectedindex = -1; //To store the row index for the multidgv grid
        string scannedBarcode = "";
        Semnox.Parafait.Inventory.InventoryLotUI inventoryLotUI;
        ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        string guid;
        bool isDataBound = false;
        private int inventoryReceiptId;
        private int roundingPrecision = 5;
        private int purchaseOrderId = -1;
        private bool isReceiveAutoPO = false;
        public frmReceiveInventory(Utilities utilities, string guid, int orderId = -1)
        {
            log.LogMethodEntry(utilities, guid, orderId);
            InitializeComponent();
            CommonFuncs.Utilities = CommonUIDisplay.Utilities = utilities;
            CommonFuncs.ParafaitEnv = CommonUIDisplay.ParafaitEnv = utilities.ParafaitEnv;
            this.utilities = utilities;
            this.guid = guid;
            if (orderId != -1)
            {
                isReceiveAutoPO = true;
                searchorder_dgv.Enabled = false;
                cb_POsearch.Enabled = false;
                tb_searchorder.Enabled = false;
                cb_searchstatus.Enabled = false;
                cmbSearchVendor.Enabled = false;
                txtProductCode.Enabled = false;
            }
            purchaseOrderId = orderId;
            InitializeVariables();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            UserRoles userRoles = new UserRoles(executionContext, utilities.ParafaitEnv.RoleId);
            bool isEnable = userRoles.IsEditable("Receiving");
            utilities.SetupAccess(gb_receive, isEnable);
            cb_complete.Enabled = cb_create.Enabled = btnReceipts.Enabled = isEnable;
            ProductContainer productContainer = new ProductContainer(executionContext);
            UOMContainer uomCOntainer = new UOMContainer(executionContext);
            log.LogMethodExit();

        }
        private void frm_receive_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            // TODO: This line of code loads data into the 'redemptionDataSet.Tax' table. You can move, or remove it, as needed.
            populateVendor();
            populateLocation();
            populateTax();
            cmbVendor.SelectedIndex = -1; //To set initial selection as blank.
            cmbSearchVendor.SelectedIndex = -1;
            cmbDefaultTax.SelectedIndex = -1;
            cb_complete.AutoSize = true;
            POSearch();
            tb_searchorder.Focus();
            fireCellValueChange = false; //this is set to false,bcz setlanguage function causes grid events to fire when there are no records.
            utilities.setLanguage(this);
            fireCellValueChange = true; //set the variable true after language change is applied
            receive_dgv.Columns["TaxAmount"].DefaultCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT;
            btnPrint.Enabled = false;
            if (purchaseOrderId != -1)
            {
                refreshReceiveLines(purchaseOrderId);
            }
            receive_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        void populateVendor()
        {
            log.LogMethodEntry();
            List<VendorDTO> vendorListDTO = new List<VendorDTO>();
            VendorList vendorList = new VendorList(executionContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> vendorDTOSearchParams;
            vendorDTOSearchParams = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            vendorDTOSearchParams.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.SITEID, executionContext.GetSiteId().ToString()));
            BindingSource bindingSource = new BindingSource();
            BindingSource bindingSource1 = new BindingSource();

            vendorListDTO = vendorList.GetAllVendors(vendorDTOSearchParams);
            if (vendorListDTO == null)
            {
                vendorListDTO = new List<VendorDTO>();
            }
            vendorListDTO.Insert(0, new VendorDTO());
            bindingSource.DataSource = vendorListDTO.OrderBy(ven => ven.Name);
            bindingSource1.DataSource = vendorListDTO.OrderBy(vendor => vendor.Name);

            cmbVendor.DataSource = bindingSource;
            cmbVendor.ValueMember = "VendorId";
            cmbVendor.DisplayMember = "Name";

            cmbSearchVendor.DataSource = bindingSource1;
            cmbSearchVendor.ValueMember = "VendorId";
            cmbSearchVendor.DisplayMember = "Name";
            log.LogMethodExit();
        }

        void populateTax()
        {
            log.LogMethodEntry();
            List<TaxDTO> purchaseTaxListDTO = new List<TaxDTO>();
            purchaseTaxList = new TaxList(executionContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            purchaseTaxListDTO = purchaseTaxList.GetAllTaxes(taxDTOSearchParams);
            if (purchaseTaxListDTO == null)
            {
                purchaseTaxListDTO = new List<TaxDTO>();
            }
            purchaseTaxListDTO.Insert(0, new TaxDTO());
            purchaseTaxListDTO[0].TaxName = string.Empty;
            BindingSource bindingSource = new System.Windows.Forms.BindingSource();
            BindingSource bindingSource1 = new System.Windows.Forms.BindingSource();
            bindingSource.DataSource = purchaseTaxListDTO;
            bindingSource1.DataSource = purchaseTaxListDTO;
            cmbDefaultTax.DataSource = bindingSource;
            cmbDefaultTax.ValueMember = "TaxId";
            cmbDefaultTax.DisplayMember = "TaxName";

            TaxId.DataSource = bindingSource1;
            TaxId.ValueMember = "TaxId";
            TaxId.DisplayMember = "TaxName";
            log.LogMethodExit();
        }

        void populateLocation()
        {
            log.LogMethodEntry();
            LocationList locationList = new LocationList(executionContext);
            List<LocationDTO> locationDTOList = new List<LocationDTO>();
            locationDTOList = locationList.GetAllLocations("Store");
            if (locationDTOList == null)
            {
                locationDTOList = new List<LocationDTO>();
            }
            locationDTOList.Insert(0, new LocationDTO());
            locationDTOList[0].Name = "<SELECT>";

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = locationDTOList;
            BindingSource bindingSource1 = new BindingSource();
            bindingSource1.DataSource = locationDTOList;

            bindingSource.DataSource = locationDTOList;
            bindingSource1.DataSource = locationDTOList;
            cmbLocation.DataSource = bindingSource;
            cmbLocation.ValueMember = "LocationId";
            cmbLocation.DisplayMember = "Name";

            recvLocation.DataSource = bindingSource1;
            recvLocation.ValueMember = "LocationId";
            recvLocation.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void InitializeVariables()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref receive_dgv);
            utilities.setupDataGridProperties(ref Rsearch_dgv);
            utilities.setupDataGridProperties(ref searchorder_dgv);

            RequiredByDate.DefaultCellStyle = utilities.gridViewDateCellStyle();

            receive_dgv.Columns["Qty"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            receive_dgv.Columns["Qty"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;

            Amount.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            Amount.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;

            Price.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            Price.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
            TaxPercentage.DefaultCellStyle = utilities.gridViewAmountCellStyle();

            //receive_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            searchorder_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Rsearch_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            searchorder_dgv.BackgroundColor = this.BackColor;
            Rsearch_dgv.BackgroundColor = this.BackColor;

            Rsearch_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Goldenrod;
            searchorder_dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.Goldenrod;

            receive_dgv.BackgroundColor = Color.White;
            DT_receive.Columns.Add("ProductID");
            DT_receive.Columns.Add("ProductCode");
            DT_receive.Columns.Add("Description");
            DT_receive.Columns.Add("UOM");
            DT_receive.Columns.Add("Qty", System.Type.GetType("System.Decimal"));
            DT_receive.Columns.Add("UOMId");
            DT_receive.Columns.Add("OrderQty", System.Type.GetType("System.Decimal"));
            DT_receive.Columns.Add("Price", System.Type.GetType("System.Decimal"));
            DT_receive.Columns.Add("TaxId");
            DT_receive.Columns.Add("TaxPercentage", System.Type.GetType("System.Decimal"));
            DT_receive.Columns.Add("TaxInclusive");

            DT_receive.Columns.Add("TaxAmount");

            DT_receive.Columns.Add("Amount", System.Type.GetType("System.Decimal"));
            DT_receive.Columns.Add("PurchaseOrderLineId");
            DT_receive.Columns.Add("IsLottable", System.Type.GetType("System.Boolean"));
            DT_receive.Columns.Add("ExpiryType");
            DT_receive.Columns.Add("RequisitionId");
            DT_receive.Columns.Add("RequisitionLineId");
            DT_receive.Columns.Add("LocationId");
            DT_receive.Columns.Add("LotDetails", typeof(List<InventoryLotDTO>));
            DT_receive.Columns.Add("PriceInTickets");
            // taxamount was here
            lb_orderid.Text = "";
            cb_complete.Text = MessageContainerList.GetMessage(executionContext, "Save");
            cb_complete.Tag = "Save";
            tb_date.Text = System.DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
            tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
            txtVendorBillNo.Text = "";
            log.LogMethodExit();
        }

        private void cb_search_Click(object sender, EventArgs e)
        {
            ProductSearch();
        }

        private void ProductSearch()
        {
            log.LogMethodEntry();
            String prod_code = "";
            decimal quantity = 0;
            String qtyOperator = ">";
            string condition = "";
            String qtyString = "";
            qtyOperator = dd_qty.Text;

            SqlCommand cmd = utilities.getCommand();
            if (txt_qty.Text == "")
            {
                qtyString = "";
            }
            else
            {
                if (!decimal.TryParse(txt_qty.Text, out quantity))
                    txt_qty.Text = "0";
                qtyString = " having sum(isnull(Inventory.Quantity, 0)) " + qtyOperator + " " + quantity.ToString();
            }
            if (utilities.ParafaitEnv.IsCorporate)
            {
                condition = " AND (Product.site_id = @site_id or @site_id = -1) ";
                cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
            }
            //Added serach by barcode
            //23-Mar-2016  
            cmd.CommandText = "select Product.productid, Code, Description as \"Desc.\", sum(case isavailabletosell when 'N' then 0 when null then 0 else isnull(Inventory.Quantity, 0) end) quantity " +
                                "from Product left outer join Inventory on Inventory.ProductId = Product.ProductId " +
                                "left outer join Location " +
                                "on Inventory.LocationId = Location.LocationId " +
                                " left outer join (select * " +
                                "from (" +
                                "select *, row_number() over(partition by productid order by productid) as num " +
                                    "from productbarcode " +
                             "where BarCode like '%' + @product_code and isactive = 'Y')v " +
                                "where num = 1)b on product.productid = b.productid " +
                                "where (Code like @product_code or description like '%' + @product_code or ProductName like N'%' + @product_code or b.Barcode like '%' + @product_code)" +
                                " AND Product.IsActive = 'Y' and IsPurchaseable = 'Y' " +
                                " and isnull(marketlistitem, 0) = 0 " + //Added 21-Aug-2016
                                condition +
                                "group by Product.productid, code, description " +
                                qtyString;
            //23-Mar-2016
            prod_code = txt_prodcode.Text + "%";
            cmd.Parameters.AddWithValue("@product_code", prod_code);

            cmd.Parameters.AddWithValue("@qty", quantity.ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DT_search.Clear();
            da.Fill(DT_search);
            da.Dispose();
            Rsearch_dgv.DataSource = DT_search;
            Rsearch_dgv.Refresh();
            Rsearch_dgv.Columns["Quantity"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            Rsearch_dgv.Columns["Quantity"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            da.Dispose();
            log.LogMethodExit();
        }

        //Updated query to search barcode from productbarcode
        private DataTable GetProductDetails(String prod_code)//,int purchaseOrderLineId,int poId)
        {
            log.LogMethodEntry(prod_code);
            DataTable DT = new DataTable();
            SqlCommand cmd = utilities.getCommand();
            string condition = "";
            if (utilities.ParafaitEnv.IsCorporate)
            {
                condition = " AND (p.site_id = @site_id or @site_id = -1)";
                cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
            }

            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_PRODUCT_SEARCH_BY_VENDOR").Equals("Y"))
            {
                if (cmbVendor.SelectedIndex == -1)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(850));
                    return DT;
                }
                condition += " and p.DefaultVendorId = @vendorId";
                cmd.Parameters.AddWithValue("@vendorId", cmbVendor.SelectedValue);
            }

            cmd.CommandText = "select p.productid, Code, Description, ReorderQuantity, " +
                    "case when p.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/ (1 + tax_Percentage / 100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, " +
                    "p.PurchaseTaxId, t.tax_percentage, " +
                    "p.taxinclusivecost, " +
                    "case when p.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/ 100 * isnull(LastPurchasePrice, cost) / (1 + tax_Percentage / 100) else isnull(LastPurchasePrice, cost) * tax_Percentage / 100 end else 0 end as TaxAmount" +
                                    ", LowerLimitCost, UpperLimitCost, CostVariancePercentage" +
                                    ", isnull(LotControlled, 0) LotControlled, isnull(ExpiryType, 'N') ExpiryType, p.defaultlocationid ReceiveLocation, ExpiryDays, p.PriceInTickets , p.uomId " + //Added 21-Aug-2016
                                    " from Product p left outer join Tax t on t.tax_id = p.PurchaseTaxId " +
                                    "left outer join (select * " +
                                                "from (" +
                                                "select *, row_number() over(partition by productid order by productid) as num " +
                                                "from productbarcode " +
                                                "where BarCode like '%' + @product_code and isactive = 'Y')v " +
                                                "where num = 1) b on p.productid = b.productid " +
                                    @"where (Code like @product_code or description like N'%' + @product_code or ProductName like N'%' + @product_code or b.BarCode like '%' + @product_code) 
                                        AND p.IsActive = 'Y' and IsPurchaseable = 'Y' " + condition +
                                        " and isnull(marketlistitem, 0) = 0 " + //Added 21-Aug-2016
                                        " order by 1";
            //23-Mar-2016
            cmd.Parameters.AddWithValue("@product_code", prod_code + "%");

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();
            log.LogMethodExit(DT);
            return DT;
        }

        private String GetProductId(String prod_code)
        {
            log.LogMethodEntry(prod_code);
            String return_value = "-1";
            DataTable DT = new DataTable();
            SqlCommand cmd = utilities.getCommand();
            string condition = "";
            if (utilities.ParafaitEnv.IsCorporate)
            {
                condition = " AND (site_id = @site_id or @site_id = -1)";
                cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
            }
            cmd.CommandText = "select  ProductId" +
                            " from Product " +
                            "where Code = @product_code " + condition;

            cmd.Parameters.AddWithValue("@product_code", prod_code);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();
            if (DT.Rows.Count == 1)
            {
                return_value = DT.Rows[0][0].ToString();
            }
            else
            {
                return_value = "-1";
            }
            log.LogMethodExit(return_value);
            return return_value;
        }

        private void txt_qty_KeyPress(object sender, KeyPressEventArgs e)
        {
            // This is for the quantity search textbox to accept only numbers. 
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void receive_dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (fireCellValueChange)
            {
                if (receive_dgv.Columns[e.ColumnIndex].Name == "ProductCode")
                {

                    DataTable DT = new DataTable();
                    if (receive_dgv[e.ColumnIndex, e.RowIndex].Value != null)
                        DT = GetProductDetails(receive_dgv[e.ColumnIndex, e.RowIndex].Value.ToString());//,LineId, PoId);

                    if (DT.Rows.Count < 1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(846), utilities.MessageUtils.getMessage("Find Products"));
                        dgvselectedindex = e.RowIndex;
                        BeginInvoke(new MethodInvoker(RemoveDGVRow));
                        //receive_dgv.Rows.Remove(receive_dgv.Rows[dgvselectedindex]);
                    }
                    else if (DT.Rows.Count == 1)
                    {
                        fireCellValueChange = false;
                        dgvselectedindex = e.RowIndex;
                        receive_dgv["ProductCode", e.RowIndex].Value = DT.Rows[0]["Code"];
                        receive_dgv["Description", e.RowIndex].Value = DT.Rows[0]["Description"];
                        receive_dgv["Qty", e.RowIndex].Value = DT.Rows[0]["ReorderQuantity"];
                        receive_dgv["Price", e.RowIndex].Value = DT.Rows[0]["cost"];
                        receive_dgv["TaxId", e.RowIndex].Value = (DT.Rows[0]["PurchaseTaxId"] == DBNull.Value || DT.Rows[0]["PurchaseTaxId"] == null ? -1 : DT.Rows[0]["PurchaseTaxId"]); //added GK
                        receive_dgv["TaxPercentage", e.RowIndex].Value = (DT.Rows[0]["tax_Percentage"] == DBNull.Value ? 0 : DT.Rows[0]["tax_Percentage"]); //added GK
                        receive_dgv["TaxInclusive", e.RowIndex].Value = DT.Rows[0]["taxInclusiveCost"];// make it 'N' In PO unit price is exclusive of tax to show that make taxinclusive N;
                        receive_dgv["TaxAmount", e.RowIndex].Value = (DT.Rows[0]["TaxAmount"] == DBNull.Value ? 0 : DT.Rows[0]["TaxAmount"]); // changed indexes 7- taxAmount
                        receive_dgv["Amount", e.RowIndex].Value = 0;
                        receive_dgv["RequiredByDate", e.RowIndex].Value = DBNull.Value;
                        receive_dgv["IsReceived", e.RowIndex].Value = "Receive";
                        receive_dgv["OrderedQty", e.RowIndex].Value = DBNull.Value;
                        receive_dgv["PurchaseOrderLineId", e.RowIndex].Value = DBNull.Value;
                        receive_dgv["LowerLimitCost", e.RowIndex].Value = DT.Rows[0]["LowerLimitCost"];
                        receive_dgv["UpperLimitCost", e.RowIndex].Value = DT.Rows[0]["UpperLimitCost"];
                        receive_dgv["CostVariancePercent", e.RowIndex].Value = DT.Rows[0]["CostVariancePercentage"];
                        receive_dgv["OrigPrice", e.RowIndex].Value = DT.Rows[0]["Cost"];
                        CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, dgvselectedindex, Convert.ToInt32(DT.Rows[0]["UOMId"]));
                        receive_dgv["txtUOM", e.RowIndex].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == Convert.ToInt32(DT.Rows[0]["UOMId"])).UOM;
                        receive_dgv["isLottable", e.RowIndex].Value = DT.Rows[0]["LotControlled"];
                        receive_dgv["ExpiryType", e.RowIndex].Value = DT.Rows[0]["ExpiryType"];
                        receive_dgv["ProductId", e.RowIndex].Value = DT.Rows[0]["productid"];
                        receive_dgv["PriceInTickets", e.RowIndex].Value = DT.Rows[0]["PriceInTickets"];
                        InventoryList inventoryList = new InventoryList();
                        receive_dgv["StockLink", e.RowIndex].Value = inventoryList.GetProductStockQuantity(Convert.ToInt32(DT.Rows[0]["productid"]));

                        if (cmbLocation.SelectedIndex != -1)
                        {
                            if (Convert.ToInt32(cmbLocation.SelectedValue) != -1)
                                receive_dgv["recvLocation", e.RowIndex].Value = cmbLocation.SelectedValue;
                        }

                        setupReceiveButton();
                        calculateAmount(e.RowIndex);
                        fireCellValueChange = true;
                    }
                    else
                    {
                        dgvselectedindex = e.RowIndex;
                        Panel pnlMultiple_dgv = new Panel();
                        this.Controls.Add(pnlMultiple_dgv);
                        DataGridView multiple_dgv = new DataGridView();
                        //this.Controls.Add(multiple_dgv);
                        pnlMultiple_dgv.Controls.Add(multiple_dgv);
                        multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                        multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                        multiple_dgv.Focus();
                        multiple_dgv.DataSource = DT;
                        multiple_dgv.Refresh();
                        multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv);//Added the grid in a panel to see that there is a scroll bar if list is long
                    }
                }
                else if (receive_dgv.Columns[e.ColumnIndex].Name == "Qty" || receive_dgv.Columns[e.ColumnIndex].Name == "Price")
                {
                    calculateAmount(e.RowIndex);
                }
                // Add : for cell value changed event for TaxID
                else if (receive_dgv.Columns[e.ColumnIndex].Name == "TaxId")
                {
                    if (receive_dgv["TaxId", e.RowIndex].Value == null || Convert.ToInt32(receive_dgv.Rows[e.RowIndex].Cells["TaxId"].Value) == -1)
                    {
                        receive_dgv.Rows[e.RowIndex].Cells["TaxAmount"].Value = 0.00;
                        receive_dgv.Rows[e.RowIndex].Cells["TaxAmount"].ReadOnly = false;
                        receive_dgv["TaxPercentage", e.RowIndex].Value = 0;
                    }

                    else
                    {
                        // getTax(e.RowIndex);
                        TaxList TaxList = new TaxList(executionContext);
                        List<TaxDTO> taxDTOList = new List<TaxDTO>();
                        List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
                        int taxID = Convert.ToInt32(receive_dgv.Rows[e.RowIndex].Cells["TaxId"].Value);
                        taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                        taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, taxID.ToString()));
                        taxDTOList = TaxList.GetAllTaxes(taxDTOSearchParams);
                        if (taxDTOList == null)
                        {
                            taxDTOList = new List<TaxDTO>();
                        }
                        //Decimal product = 0;
                        decimal ltaxAmount = 0;
                        string pTaxInclusiveCost = null;
                        decimal pTaxPercent = 0;

                        List<TaxDTO> selectedTax = taxDTOList.Where(x => x.TaxId == taxID).ToList();
                        if (selectedTax != null && selectedTax.Count > 0)
                        {
                            pTaxPercent = Convert.ToDecimal(selectedTax[0].TaxPercentage.ToString());
                        }
                        receive_dgv["TaxPercentage", e.RowIndex].Value = pTaxPercent;  // adding tac percentage to grid

                        try
                        {
                            try { pTaxInclusiveCost = receive_dgv["taxInclusiveCost", receive_dgv.CurrentCell.RowIndex].Value.ToString(); } catch { }
                            if (pTaxInclusiveCost != null)
                            {
                                decimal priceValue = Convert.ToDecimal(receive_dgv["Price", receive_dgv.CurrentCell.RowIndex].Value);
                                //if (pTaxInclusiveCost == "Y")
                                //{
                                //    ltaxAmount = Math.Round((pTaxPercent / 100) * Convert.ToDecimal(receive_dgv["Price", receive_dgv.CurrentCell.RowIndex].Value) / (1 + (pTaxPercent / 100)), roundingPrecision, MidpointRounding.AwayFromZero);
                                //}
                                //else
                                //{
                                ltaxAmount = Math.Round((priceValue * pTaxPercent / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                                //}
                                receive_dgv["TaxAmount", receive_dgv.CurrentCell.RowIndex].Value = ltaxAmount;
                            }
                            else
                            {
                                ltaxAmount = Math.Round(Convert.ToDecimal(receive_dgv["TaxAmount", receive_dgv.CurrentCell.RowIndex].Value), roundingPrecision, MidpointRounding.AwayFromZero);
                            }
                        }
                        catch
                        {
                        }

                        receive_dgv["TaxAmount", receive_dgv.CurrentCell.RowIndex].Value = ltaxAmount;
                        receive_dgv["TaxAmount", receive_dgv.CurrentCell.RowIndex].ReadOnly = true;
                        //receive_dgv["TaxAmount", e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Blue, Font = new Font(receive_dgv.Font, FontStyle.Underline) };
                    }
                    receive_dgv["TaxAmount", e.RowIndex].ReadOnly = false;
                    // receive_dgv["TaxAmount", e.RowIndex].Value = Convert.ToDecimal(receive_dgv["TaxAmount", receive_dgv.CurrentCell.RowIndex].Value);
                    calculateAmount(e.RowIndex);


                }
                else if (receive_dgv.Columns[e.ColumnIndex].Name == "TaxAmount")
                {
                    calculateAmount(e.RowIndex);
                }

                receive_dgv.Refresh();
                log.LogMethodExit();
            }
        }

        private void RemoveDGVRow()
        {
            log.LogMethodEntry(dgvselectedindex);
            try
            {
                if (dgvselectedindex > -1)
                {
                    receive_dgv.Rows.RemoveAt(dgvselectedindex);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void calculateAmount(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            try
            {
                int taxID = receive_dgv["TaxId", rowIndex].Value == DBNull.Value || receive_dgv["TaxId", rowIndex].Value == null ? -1 : Convert.ToInt32(receive_dgv["TaxId", rowIndex].Value);
                decimal taxPer = Convert.ToDecimal((receive_dgv["TaxPercentage", rowIndex].Value == DBNull.Value ? 0 : receive_dgv["TaxPercentage", rowIndex].Value));
                decimal price = Convert.ToDecimal((receive_dgv["Price", rowIndex].Value == DBNull.Value ? 0 : receive_dgv["Price", rowIndex].Value));
                decimal qty = Convert.ToDecimal((receive_dgv["Qty", rowIndex].Value == DBNull.Value ? 0 : receive_dgv["Qty", rowIndex].Value));
                decimal ltaxAmount = 0.0M;
                int uomId = -1;
                try
                {
                    uomId = Convert.ToInt32(receive_dgv["cmbUOM", rowIndex].Value == null ? ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", rowIndex].Value)).UomId
                        : receive_dgv["cmbUOM", rowIndex].Value);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                receive_dgv["cmbUOM", rowIndex].Value = null;
                if (taxID <= -1) // Added by GK: if  No tax selected for Auto PO
                {
                    receive_dgv["TaxAmount", rowIndex].ReadOnly = false;
                    receive_dgv["TaxId", rowIndex].ReadOnly = false;
                    if (receive_dgv["TaxInclusive", rowIndex].Value != null)
                    {
                        //if (receive_dgv["TaxInclusive", rowIndex].Value.ToString() == "Y")
                        //{
                        //    receive_dgv["Amount", rowIndex].Value = qty * price;
                        //    // if not tax structure but inclusive  then Price = Price  
                        //}
                        //else
                        //{
                        if (receive_dgv["TaxAmount", rowIndex].Value != null && !String.IsNullOrEmpty(receive_dgv["TaxAmount", rowIndex].Value.ToString()))
                        {
                            receive_dgv["Amount", rowIndex].Value = Math.Round(qty * (price + Convert.ToDecimal(receive_dgv["TaxAmount", rowIndex].Value.ToString())), roundingPrecision, MidpointRounding.AwayFromZero);
                        }
                        else
                        {
                            receive_dgv["Amount", rowIndex].Value = qty * price;
                        }
                        // if not tax structure and notinclusive  then Price = Price + tax amount 
                        //}

                    }
                }
                else if (receive_dgv["TaxInclusive", rowIndex].Value != null)
                {
                    //if (receive_dgv["TaxInclusive", rowIndex].Value.ToString() == "Y")
                    //{
                    //    receive_dgv["Amount", rowIndex].Value = qty * price;
                    //    ltaxAmount = Math.Round((taxPer / 100) * price / (1 + (taxPer / 100)), roundingPrecision, MidpointRounding.AwayFromZero);
                    //}
                    //else
                    //{
                    //receive_dgv["TaxAmount", rowIndex].ReadOnly = true;
                    receive_dgv["Amount", rowIndex].Value = Math.Round(qty * (price + price * taxPer / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                    ltaxAmount = Math.Round((price * taxPer / 100), roundingPrecision, MidpointRounding.AwayFromZero);
                    //}
                    receive_dgv["TaxAmount", rowIndex].Value = ltaxAmount;
                }
                recalculateTotal();
                CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, rowIndex, uomId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataGridView dg = (DataGridView)sender;
            fireCellValueChange = false;
            receive_dgv["ProductCode", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Code"].Value;
            receive_dgv["Description", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Description"].Value;
            receive_dgv["Qty", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ReorderQuantity"].Value;
            receive_dgv["Price", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["cost"].Value;
            receive_dgv["TaxId", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["PurchaseTaxId"].Value;
            receive_dgv["TaxPercentage", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["tax_Percentage"].Value;
            receive_dgv["TaxInclusive", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["TaxInclusiveCost"].Value;
            receive_dgv["TaxAmount", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["TaxAmount"].Value;
            receive_dgv["Amount", dgvselectedindex].Value = 0;
            receive_dgv["RequiredByDate", dgvselectedindex].Value = DBNull.Value;
            receive_dgv["IsReceived", dgvselectedindex].Value = "Receive";
            receive_dgv["PurchaseOrderLineId", dgvselectedindex].Value = DBNull.Value;
            receive_dgv["OrderedQty", dgvselectedindex].Value = DBNull.Value;
            receive_dgv["LowerLimitCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["LowerLimitCost"].Value;
            receive_dgv["UpperLimitCost", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["UpperLimitCost"].Value;
            receive_dgv["CostVariancePercent", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["CostVariancePercentage"].Value;
            receive_dgv["OrigPrice", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Cost"].Value;
            receive_dgv["isLottable", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["LotControlled"].Value;
            receive_dgv["ExpiryType", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["ExpiryType"].Value;
            receive_dgv["ProductId", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["Productid"].Value;
            receive_dgv["PriceInTickets", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["PriceInTickets"].Value;
            CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, dgvselectedindex, Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["UOMId"].Value));
            receive_dgv["txtUOM", dgvselectedindex].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["UOMId"].Value)).UOM;

            // receive_dgv["TaxAmount", dgvselectedindex].Value = dg.Rows[dg.CurrentRow.Index].Cells["TaxAmount"].Value; //added Gk
            if (cmbLocation.SelectedIndex != -1)
            {
                if (Convert.ToInt32(cmbLocation.SelectedValue) != -1)
                    receive_dgv["recvLocation", dgvselectedindex].Value = cmbLocation.SelectedValue;
            }

            InventoryList inventoryList = new InventoryList();
            receive_dgv["StockLink", dgvselectedindex].Value = inventoryList.GetProductStockQuantity(Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["Productid"].Value));

            calculateAmount(dgvselectedindex);
            fireCellValueChange = true;
            dg.Visible = false;
            dg.Parent.Visible = false;
            setupReceiveButton();
            log.LogMethodExit();
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            pnlMultiple_dgv.Size = new Size(250, (receive_dgv.Rows[0].Cells[0].Size.Height * 10) - 3);
            pnlMultiple_dgv.AutoScroll = true;
            //multiple_dgv.Size = new Size(250, (receive_dgv.Rows[0].Cells[0].Size.Height * (multiple_dgv.Rows.Count - 1)) - 3);
            //multiple_dgv.Location = new Point(100 + gb_receive.Location.X + receive_dgv.Location.X + receive_dgv.RowHeadersWidth + receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["Qty"].ContentBounds.Location.X, (receive_dgv.Location.Y + receive_dgv.ColumnHeadersHeight));
            //multiple_dgv.BringToFront();
            pnlMultiple_dgv.Location = new Point(100 + gb_receive.Location.X + receive_dgv.Location.X + receive_dgv.RowHeadersWidth + receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["Qty"].ContentBounds.Location.X, (receive_dgv.Location.Y + receive_dgv.ColumnHeadersHeight));
            pnlMultiple_dgv.BringToFront();
            pnlMultiple_dgv.BackColor = Color.White;
            pnlMultiple_dgv.BorderStyle = BorderStyle.None;
            multiple_dgv.BorderStyle = BorderStyle.None;
            multiple_dgv.AllowUserToAddRows = false;
            multiple_dgv.BackgroundColor = Color.White;
            multiple_dgv.Columns[0].Visible = false;
            for (int i = 3; i < multiple_dgv.Columns.Count; i++)
                multiple_dgv.Columns[i].Visible = false;
            multiple_dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //multiple_dgv.Columns["ReorderQuantity"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
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

        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            DataGridView dg = (DataGridView)sender;
            if (dg.SelectedRows.Count == 0)
            {
                try
                {
                    receive_dgv.Rows.Remove(receive_dgv.Rows[dgvselectedindex]);
                }
                catch { }
            }
            dg.Visible = false;
            dg.Parent.Visible = false;
            gb_receive.Controls.Remove(dg.Parent);
        }


        private void receive_dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;
            receiveValidationResult = true;
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridView dg = (DataGridView)sender;
                    if ((dg.Columns[e.ColumnIndex].Name == "Lot"))
                    {
                        int userEnteredUOMId = -1;
                        if (receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].Value != null)
                        {
                            userEnteredUOMId = Convert.ToInt32(receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].Value);
                        }
                        if (userEnteredUOMId == -1)
                        {
                            userEnteredUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                        }
                        int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                        double factor = 1;
                        if (userEnteredUOMId != inventoryUOMId)
                        {
                            factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                        }
                        if ((receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value) != null && (
                            Convert.ToBoolean(receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value) == true ||
                            receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString() == "D" ||
                            receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString() == "E"))
                        {
                            string expiryType = "N";
                            if (receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != null && receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != DBNull.Value)
                            {
                                expiryType = receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString();
                            }

                            inventoryLotListDTO = new List<InventoryLotDTO>();
                            if (receive_dgv.CurrentRow.Tag == null)
                            {
                                inventoryLotDTO = new InventoryLotDTO();
                                inventoryLotDTO.Quantity = Convert.ToDouble(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value);// * factor;
                                inventoryLotDTO.BalanceQuantity = Convert.ToDouble(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value);// * factor;
                                inventoryLotDTO.ExpiryInDays = Convert.ToInt32(receive_dgv["ExpiryDays", receive_dgv.CurrentRow.Index].Value);
                                inventoryLotDTO.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                                inventoryLotDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == userEnteredUOMId).UOM;
                                inventoryLotListDTO.Add(inventoryLotDTO);
                                inventoryLotUI = new InventoryLotUI(utilities, inventoryLotListDTO, expiryType, true, (expiryType == "N" || expiryType == "D") ? false : true);
                                CommonUIDisplay.setupVisuals(inventoryLotUI);//Added to style GUI on 09-Sep-2016
                                inventoryLotUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                                inventoryLotUI.ShowDialog();
                                inventoryLotListDTO = inventoryLotUI.inventoryLotListOnReturn;
                                if (inventoryLotListDTO != null)
                                {
                                    receive_dgv.CurrentRow.Tag = inventoryLotListDTO;
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1062));
                                    receiveValidationResult = false;
                                    return;
                                }
                            }
                            else
                            {
                                inventoryLotListDTO = (List<InventoryLotDTO>)receive_dgv.CurrentRow.Tag;
                                foreach (InventoryLotDTO lotDTO in inventoryLotListDTO)
                                {
                                    lotDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == userEnteredUOMId).UOM;
                                    //receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].FormattedValue.ToString();
                                }
                                inventoryLotUI = new InventoryLotUI(utilities, inventoryLotListDTO, expiryType, true, expiryType == "N" ? false : true);
                                CommonUIDisplay.setupVisuals(inventoryLotUI);//Added to style GUI on 09-Sep-2016
                                inventoryLotUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                                inventoryLotUI.ShowDialog();
                                if (inventoryLotListDTO != null)
                                {
                                    receive_dgv.CurrentRow.Tag = inventoryLotListDTO;
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1062));
                                    receiveValidationResult = false;
                                    return;
                                }
                            }
                        }
                    }

                    if ((dg.Columns[e.ColumnIndex].Name == "IsReceived"))
                    {
                        if ((receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value != null && receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value != DBNull.Value) || (receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != null && receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != DBNull.Value))
                        {
                            if (Convert.ToBoolean(receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value) == true || receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString() == "D" || receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString() == "E")
                            {
                                string expiryType = "N";
                                if (receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != null && receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value != DBNull.Value)
                                {
                                    expiryType = receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value.ToString();
                                }

                                inventoryLotListDTO = new List<InventoryLotDTO>();
                                int userEnteredUOMId = -1;
                                if (receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].Value != null)
                                {
                                    userEnteredUOMId = Convert.ToInt32(receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].Value);
                                }
                                if (userEnteredUOMId == -1)
                                {
                                    userEnteredUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                                }
                                int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductId", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                                double factor = 1;
                                if (userEnteredUOMId != inventoryUOMId)
                                {
                                    factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                                }
                                if (receive_dgv.CurrentRow.Tag == null)
                                {
                                    inventoryLotDTO = new InventoryLotDTO();

                                    inventoryLotDTO.Quantity = Convert.ToDouble(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value) * factor;
                                    inventoryLotDTO.BalanceQuantity = Convert.ToDouble(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value) * factor;
                                    inventoryLotDTO.ExpiryInDays = Convert.ToInt32(receive_dgv["ExpiryDays", receive_dgv.CurrentRow.Index].Value);
                                    inventoryLotDTO.UOMId = inventoryUOMId;
                                    inventoryLotListDTO.Add(inventoryLotDTO);
                                    inventoryLotUI = new InventoryLotUI(utilities, inventoryLotListDTO, expiryType, true, expiryType == "N" ? false : true);
                                    CommonUIDisplay.setupVisuals(inventoryLotUI);//Added to style GUI on 09-Sep-2016
                                    inventoryLotUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                                    inventoryLotUI.ShowDialog();
                                    inventoryLotListDTO = inventoryLotUI.inventoryLotListOnReturn;
                                    if (inventoryLotListDTO != null)
                                    {
                                        receive_dgv.CurrentRow.Tag = inventoryLotListDTO;
                                    }
                                    else
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1062));
                                        receiveValidationResult = false;
                                        return;
                                    }
                                }
                                else
                                {
                                    inventoryLotListDTO = (List<InventoryLotDTO>)receive_dgv.CurrentRow.Tag;
                                    foreach (InventoryLotDTO lotDTO in inventoryLotListDTO)
                                    {
                                        lotDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == userEnteredUOMId).UOM;
                                    }
                                    inventoryLotUI = new InventoryLotUI(utilities, inventoryLotListDTO, expiryType, true, expiryType == "N" ? false : true);
                                    CommonUIDisplay.setupVisuals(inventoryLotUI);//Added to style GUI on 09-Sep-2016
                                    inventoryLotUI.StartPosition = FormStartPosition.CenterScreen;//Added to show at center on 09-Sep-2016
                                    inventoryLotUI.ShowDialog();
                                    if (inventoryLotListDTO != null)
                                    {
                                        foreach (InventoryLotDTO lotDTO in inventoryLotListDTO)
                                        {
                                            lotDTO.Quantity = lotDTO.Quantity * factor;
                                            lotDTO.BalanceQuantity = lotDTO.BalanceQuantity * factor;
                                        }
                                        receive_dgv.CurrentRow.Tag = inventoryLotListDTO;
                                    }
                                    else
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1062));
                                        receiveValidationResult = false;
                                        return;
                                    }
                                }
                            }
                        }

                        if (receive_dgv["IsReceived", receive_dgv.CurrentRow.Index].ValueType.FullName != "System.Drawing.Image")
                        {
                            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
                            decimal OrderQty = 0;
                            if (receive_dgv["OrderedQty", receive_dgv.CurrentRow.Index].Value != DBNull.Value)
                            {
                                OrderQty = Convert.ToDecimal(receive_dgv["OrderedQty", receive_dgv.CurrentRow.Index].Value);
                            }
                            decimal recieveQty = Convert.ToDecimal(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value);
                            int userEnteredUOMId = -1;
                            if (receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["cmbUOM"].Value != null)
                            {
                                userEnteredUOMId = Convert.ToInt32(receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["cmbUOM"].Value);
                            }
                            if (userEnteredUOMId == -1)
                            {
                                userEnteredUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductID", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                            }
                            int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductID", receive_dgv.CurrentRow.Index].Value)).InventoryUOMId;
                            UOMDTO uomDTO = UOMContainer.uomDTOList.Find(x => x.UOM == receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["txtUOM"].Value.ToString());
                            int productUOMid = uomDTO.UOMId;
                            if (userEnteredUOMId != inventoryUOMId)
                            {
                                decimal factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId));
                                recieveQty = recieveQty * factor;
                                if (userEnteredUOMId == productUOMid)
                                {
                                    OrderQty = OrderQty * factor;
                                }
                                else
                                {
                                    receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value = Convert.ToDecimal(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value) * factor;
                                }
                            }
                            else if (userEnteredUOMId != productUOMid)
                            {
                                decimal factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(productUOMid, userEnteredUOMId));
                                OrderQty = OrderQty * factor;
                                receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value = Convert.ToDecimal(receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value) / factor;
                            }
                            //Start update 02-Jun-2016
                            //Added if condition to see that receive does not happen if receive quantity is more than order quantity
                            if (tb_order.Text != "" && lblOrderDocumentType.Text != "Contract Purchase Order")//03-Jun-2016 Added if statement to see that next if condition is executed only in case there is a PO
                                if (OrderQty < recieveQty)
                                {
                                    if (lblOrderDocumentType.Text != "Contract Purchase Order")
                                    {
                                        receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value = receive_dgv["OrderedQty", receive_dgv.CurrentRow.Index].Value;
                                        if (receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["txtUOM"].Value != null)
                                        {
                                            receive_dgv["cmbUOM", receive_dgv.CurrentRow.Index].Value = receive_dgv.Rows[receive_dgv.CurrentRow.Index].Cells["txtUOM"].Value;
                                        }
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1019), utilities.MessageUtils.getMessage("Receive Line Quantity"));
                                        receiveValidationResult = false;
                                        return;
                                    }
                                }
                            if (recieveQty <= 0)
                            {
                                if (lb_orderid.Text != "")
                                    receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value = Convert.ToDecimal(receive_dgv["OrderedQty", receive_dgv.CurrentRow.Index].Value);
                                MessageBox.Show(utilities.MessageUtils.getMessage(1091), utilities.MessageUtils.getMessage(1092));
                                receiveValidationResult = false;
                                return;
                            }
                            //End update 02-Jun-2016
                            if (Convert.ToDouble(receive_dgv["amount", receive_dgv.CurrentRow.Index].Value).Equals(0))
                            {
                                if (MessageBox.Show(utilities.MessageUtils.getMessage(847), utilities.MessageUtils.getMessage("Receipt Line Amount"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                                {
                                    receiveValidationResult = false;
                                    return;
                                }
                            }
                            if (Convert.ToDouble(receive_dgv["recvLocation", receive_dgv.CurrentRow.Index].Value).Equals(-1))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1107), utilities.MessageUtils.getMessage("Receipt Line Location"));
                                receiveValidationResult = false;
                                return;
                            }
                            DataGridViewImageCell ic = new DataGridViewImageCell();
                            ic.Value = Properties.Resources.status_ok;
                            ic.ImageLayout = DataGridViewImageCellLayout.Normal;
                            receive_dgv["IsReceived", receive_dgv.CurrentRow.Index] = ic;
                            receive_dgv.CurrentRow.ReadOnly = true;
                            DT_receive.Rows.Add(receive_dgv["ProductID", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["ProductCode", receive_dgv.CurrentRow.Index].Value.ToString(),
                                                receive_dgv["Description", receive_dgv.CurrentRow.Index].Value.ToString(),
                                                receive_dgv["txtUOM", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["Qty", receive_dgv.CurrentRow.Index].Value,
                                                UOMContainer.uomDTOList.Find(x => x.UOM == receive_dgv["txtUOM", receive_dgv.CurrentRow.Index].Value.ToString()).UOMId,
                                                receive_dgv["OrderedQty", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["Price", receive_dgv.CurrentRow.Index].Value == null || receive_dgv["Price", receive_dgv.CurrentRow.Index].Value == DBNull.Value ? 0 : receive_dgv["Price", receive_dgv.CurrentRow.Index].Value,
                                                (receive_dgv["TaxId", receive_dgv.CurrentRow.Index].Value == null || receive_dgv["TaxId", receive_dgv.CurrentRow.Index].Value == DBNull.Value) ? -1 : receive_dgv["TaxId", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["TaxPercentage", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["TaxInclusive", receive_dgv.CurrentRow.Index].Value.ToString(),
                                                receive_dgv["TaxAmount", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["amount", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["PurchaseOrderLineId", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["isLottable", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv["ExpiryType", receive_dgv.CurrentRow.Index].Value,
                                                (receive_dgv["RequisitionId", receive_dgv.CurrentRow.Index].Value == null || receive_dgv["RequisitionId", receive_dgv.CurrentRow.Index].Value == DBNull.Value) ? -1 : receive_dgv["RequisitionId", receive_dgv.CurrentRow.Index].Value,
                                                (receive_dgv["RequisitionLineId", receive_dgv.CurrentRow.Index].Value == null || receive_dgv["RequisitionLineId", receive_dgv.CurrentRow.Index].Value == DBNull.Value) ? -1 : receive_dgv["RequisitionLineId", receive_dgv.CurrentRow.Index].Value,
                                                (receive_dgv["recvLocation", receive_dgv.CurrentRow.Index].Value == null) || (receive_dgv["recvLocation", receive_dgv.CurrentRow.Index].Value == null) ? -1 : receive_dgv["recvLocation", receive_dgv.CurrentRow.Index].Value,
                                                receive_dgv.CurrentRow.Tag,
                                                (receive_dgv["PriceInTickets", receive_dgv.CurrentRow.Index].Value == null) || (receive_dgv["PriceInTickets", receive_dgv.CurrentRow.Index].Value.ToString() == "" || receive_dgv["PriceInTickets", receive_dgv.CurrentRow.Index].Value.ToString() == "NaN" || receive_dgv["PriceInTickets", receive_dgv.CurrentRow.Index].Value == null) ? Double.NaN : receive_dgv["PriceInTickets", receive_dgv.CurrentRow.Index].Value
                                                //added  
                                                // receive_dgv["TaxAmount", receive_dgv.CurrentRow.Index].Value
                                                );
                        }
                        bool all_complete = true;
                        for (int i = 0; i < receive_dgv.RowCount; i++)
                        {
                            try
                            {
                                DataGridViewImageCell img = new DataGridViewImageCell();
                                img = (DataGridViewImageCell)receive_dgv["IsReceived", i];
                                if (Convert.ToDecimal(receive_dgv["OrderedQty", i].Value) > Convert.ToDecimal(receive_dgv["Qty", i].Value))
                                {
                                    all_complete = false;
                                    break;
                                }
                            }
                            catch
                            {
                                all_complete = false;
                                break;
                            }
                        }
                        if (all_complete)
                        {
                            //   cb_complete.Enabled = true;
                            cb_complete.Text = MessageContainerList.GetMessage(executionContext, "     Complete PO");
                            cb_complete.Tag = "Complete";
                        }
                        else
                        {
                            cb_complete.Text = MessageContainerList.GetMessage(executionContext, "Save");
                            cb_complete.Tag = "Save";
                        }
                    }

                    if ((dg.Columns[e.ColumnIndex].Name == "StockLink"))
                    {
                        using (Semnox.Parafait.Inventory.frmInventoryStockDetails frmStockDetails = new Semnox.Parafait.Inventory.frmInventoryStockDetails(executionContext, Convert.ToInt32(receive_dgv.Rows[e.RowIndex].Cells["ProductId"].Value)))
                        {
                            Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(frmStockDetails);
                            frmStockDetails.ShowDialog();
                        }
                    }
                }
                if (DT_receive.Rows.Count > 0)
                {
                    int uomId = Convert.ToInt32(DT_receive.Rows[e.RowIndex]["UOMId"].ToString());
                    CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, e.RowIndex, uomId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void setupReceiveButton()
        {
            DataGridViewButtonCell cb = new DataGridViewButtonCell();
            cb.Value = "Receive";
            receive_dgv["IsReceived", dgvselectedindex] = cb;
            receive_dgv["Qty", dgvselectedindex].ReadOnly = false;
        }

        private void receive_dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (receive_dgv.CurrentCell.ColumnIndex == 2 || receive_dgv.CurrentCell.ColumnIndex == 3)
            {
                Control obj = receive_dgv.EditingControl;
                if (obj != null)
                {
                    DataGridViewTextBoxEditingControl tb = (DataGridViewTextBoxEditingControl)obj;
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (receive_dgv.CurrentCell.ColumnIndex == 2 || receive_dgv.CurrentCell.ColumnIndex == 3)
            {
                if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
                {
                    e.Handled = true;
                }
            }
        }

        private void cb_complete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (Validate())
                {
                    if (isReceiveAutoPO)
                    {
                        try
                        {
                            if (cb_complete.Tag.ToString() == "Save")
                            {
                                ValidateAutoPOReceive();
                            }
                        }
                        catch (ValidationException ex)
                        {
                            log.Error(ex);
                            if (MessageBox.Show(this, ex.Message, "Auto PO", MessageBoxButtons.YesNo) == DialogResult.No)//2269-Do you want to short close them and complete receive?
                            {
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }
                    if (UpdateDatabase())
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(848) + " " + tb_order.Text + ". " + utilities.MessageUtils.getMessage(849) + " " + txtReceiptId.Text, utilities.MessageUtils.getMessage("Receive PO"));
                        POSearch();
                        receive_dgv.ReadOnly = false;
                        btnPrint.Enabled = true;
                        btnRefresh.PerformClick();
                    }
                    log.LogMethodExit();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, "Error while saving" + ex.Message));
            }
        }

        private bool UpdateDatabase()
        {
            log.LogMethodEntry();
            bool manual_po = false;

            String remarks = ""; //Receive remarks field.
            remarks = txt_remarks.Text.Trim();
            DateTime HeaderDateTime = ServerDateTime.Now;

            String ordernumber;
            ordernumber = tb_order.Text;

            SqlTransaction SQLTrx = null;
            SqlConnection TrxCnn = null;

            if (SQLTrx == null)
            {
                TrxCnn = utilities.createConnection();
                SQLTrx = TrxCnn.BeginTransaction();
            }

            int return_id;
            purchaseOrderDTO = new PurchaseOrderDTO();


            try
            {
                if (tb_order.Text == "") //No PO. Insert a Manual PO record.
                {
                    // New order to be inserted.
                    manual_po = true;
                    purchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED;
                    purchaseOrderDTO.OrderDate = HeaderDateTime;
                    purchaseOrderDTO.VendorId = Convert.ToInt32(cmbVendor.SelectedValue);
                    purchaseOrderDTO.ReceivedDate = dtpReceiveDate.Value;
                    purchaseOrderDTO.ReceiveRemarks = txt_remarks.Text;
                    purchaseOrderDTO.OrderTotal = Convert.ToDouble(tb_total.Text);
                    purchaseOrderDTO.LastModDttm = ServerDateTime.Now;
                    purchaseOrderDTO.CancelledDate = DateTime.MinValue;
                    purchaseOrderDTO.DocumentTypeID = -1;
                    purchaseOrderDTO.Fromdate = ServerDateTime.Now;
                    purchaseOrderDTO.ToDate = ServerDateTime.Now;
                    purchaseOrderDTO.AmendmentNumber = 0;
                    purchaseOrderDTO.DocumentStatus = "";
                    purchaseOrderDTO.ReprintCount = 0;
                    purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                    purchaseOrder.Save(SQLTrx);
                    return_id = purchaseOrderDTO.PurchaseOrderId;
                    lb_orderid.Text = Convert.ToString(purchaseOrderDTO.PurchaseOrderId);
                    tb_order.Text = ordernumber;
                }
                else //User is adding to or updating existing PO
                {
                    return_id = Convert.ToInt32(lb_orderid.Text);
                    purchaseOrder = new PurchaseOrder(return_id, executionContext);
                    if (lblOrderDocumentType.Text != "Contract Purchase Order")
                    {
                        purchaseOrder.getPurchaseOrderDTO.LastModDttm = ServerDateTime.Now;
                        if (cb_complete.Tag.ToString() == "Save")
                        {
                            purchaseOrder.getPurchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS;
                        }
                        else
                        {
                            purchaseOrder.getPurchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED;
                        }
                        purchaseOrder.getPurchaseOrderDTO.ReceivedDate = HeaderDateTime;
                        purchaseOrder.Save(SQLTrx);
                        //purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
                    }
                    UserMessagesList userMessagesList = new UserMessagesList();
                    List<UserMessagesDTO> userMessagesDTOList;
                    List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, purchaseOrder.getPurchaseOrderDTO.Guid));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, "RGPO"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                    userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
                    if (userMessagesDTOList != null && userMessagesDTOList.Count == 1 && userMessagesDTOList[0].ApprovalRuleID == -1)
                    {
                        userMessagesDTOList[0].ActedByUser = utilities.ParafaitEnv.User_Id;
                        userMessagesDTOList[0].Status = UserMessagesDTO.UserMessagesStatus.APPROVED.ToString();
                        UserMessages userMessages = new UserMessages(userMessagesDTOList[0], executionContext);
                        userMessages.Save(SQLTrx);
                    }
                }

                int detailPurchaseOrderId = purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId;
                int ReceiptId;
                int receiveLocationID = -1;
                receiveLocationID = cmbLocation.SelectedIndex < 0 ? -1 : Convert.ToInt32(cmbLocation.SelectedValue);

                inventoryReceiptDTO = new InventoryReceiptDTO();

                inventoryReceiptDTO.VendorBillNumber = txtVendorBillNo.Text;
                inventoryReceiptDTO.GatePassNumber = txtGatePassNo.Text;
                inventoryReceiptDTO.Remarks = remarks;
                inventoryReceiptDTO.PurchaseOrderId = detailPurchaseOrderId;
                inventoryReceiptDTO.DocumentTypeID = getDocumentType();
                inventoryReceiptDTO.ReceiveDate = dtpReceiveDate.Value;
                inventoryReceiptDTO.ReceivedBy = utilities.ParafaitEnv.LoginID;
                inventoryReceiptDTO.ReceiveToLocationID = receiveLocationID;
                inventoryReceipt = new InventoryReceiptsBL(inventoryReceiptDTO, executionContext);
                inventoryReceipt.Save(SQLTrx);
                ReceiptId = inventoryReceiptDTO.ReceiptId;
                txtGRN.Text = inventoryReceiptDTO.GRN;
                inventoryReceiptId = inventoryReceiptDTO.ReceiptId;   // For Report Generation
                                                                      //Insert into detail table
                int detailProductId;
                String detailProdCode;
                String detailDescription;
                int requisitionId = -1;
                int requisitionLineId = -1;
                double detailQuantity;
                decimal receiveQty;
                String detailisReceived;
                double price;
                double ordertotal = 0;
                int LocationId;
                List<ProductDTO> productDTOList;
                ProductList productList = new ProductList();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchByProductParameters;
                int retPurchaseOrderLineId = -1;
                bool isClosed = true;
                decimal factor = 1;
                for (int i = 0; i < DT_receive.Rows.Count; i++) //The valid records have been stored in DT_Receive.
                {
                    detailQuantity = Convert.ToDouble(DT_receive.Rows[i]["Qty"]);
                    receiveQty = Convert.ToDecimal(detailQuantity);
                    int userEnteredUOMId = -1;
                    if (receive_dgv.Rows[i].Cells["cmbUOM"].Value != null)
                    {
                        userEnteredUOMId = Convert.ToInt32(receive_dgv.Rows[i].Cells["cmbUOM"].Value);
                    }
                    if (userEnteredUOMId == -1)
                    {
                        userEnteredUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductID", i].Value)).InventoryUOMId;
                    }
                    int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv["ProductID", i].Value)).InventoryUOMId;
                    if (userEnteredUOMId != inventoryUOMId)
                    {
                        factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId));
                        receiveQty = receiveQty * factor;
                    }
                    detailProdCode = DT_receive.Rows[i]["ProductCode"].ToString();
                    if (receiveQty <= 0)
                        continue;
                    searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE_EXACT_MATCH, detailProdCode));
                    productDTOList = productList.GetAllProducts(searchByProductParameters, SQLTrx);
                    if (productDTOList != null && productDTOList.Count > 0)
                    {
                        detailProductId = productDTOList[0].ProductId;
                    }
                    else
                    {
                        throw new Exception("Product not found");
                    }

                    requisitionId = (DT_receive.Rows[i]["RequisitionId"] == null || DT_receive.Rows[i]["RequisitionId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_receive.Rows[i]["RequisitionId"]);
                    requisitionLineId = (DT_receive.Rows[i]["RequisitionLineId"] == null || DT_receive.Rows[i]["RequisitionLineId"] == DBNull.Value) ? -1 : Convert.ToInt32(DT_receive.Rows[i]["RequisitionLineId"]);

                    if (requisitionLineId != -1)
                    {
                        RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionId, SQLTrx);
                        if (requisitionBL.GetRequisitionsDTO != null)
                        {
                            receiveLocationID = requisitionBL.GetRequisitionsDTO.ToDepartment;
                        }
                    }


                    price = Convert.ToDouble(DT_receive.Rows[i]["Price"]);
                    double taxPercentage = 0;
                    double taxAmount = 0;
                    // for No tax Selection
                    if (DT_receive.Rows[i]["TaxPercentage"] == DBNull.Value || Convert.ToInt32(DT_receive.Rows[i]["TaxId"]) == 0)
                    {
                        taxAmount = DT_receive.Rows[i]["TaxAmount"] == DBNull.Value ? 0 : Convert.ToDouble(DT_receive.Rows[i]["TaxAmount"]);
                        taxPercentage = 0;
                        DT_receive.Rows[i]["TaxId"] = -1; // If no tax is selected then taxid =-1 ;
                    }

                    if (DT_receive.Rows[i]["TaxPercentage"] != DBNull.Value)
                    {
                        taxPercentage = Convert.ToDouble(DT_receive.Rows[i]["TaxPercentage"]);
                        //if (DT_receive.Rows[i]["TaxInclusive"].ToString() == "Y")
                        //{
                        //    price = price / (1 + taxPercentage / 100); // Getting Unit price if TaxInclusive is Y from Query.
                        //    taxAmount = price * taxPercentage / 100;
                        //}
                        //else
                        //{
                        taxAmount = price * taxPercentage / 100;
                        //}
                    }

                    if (cmbLocation.SelectedIndex > 0)
                        LocationId = Convert.ToInt32(cmbLocation.SelectedValue);
                    else
                        LocationId = -1;
                    detailDescription = DT_receive.Rows[i]["description"].ToString();

                    detailisReceived = "Y";

                    if (manual_po)
                    {
                        //Insert into PO details table for manually created POs. 
                        purchaseOrderLineDTO = new PurchaseOrderLineDTO();

                        purchaseOrderLineDTO.PurchaseOrderId = return_id;
                        purchaseOrderLineDTO.ItemCode = detailProdCode;
                        purchaseOrderLineDTO.ProductId = detailProductId;
                        purchaseOrderLineDTO.Description = detailDescription;
                        purchaseOrderLineDTO.Quantity = detailQuantity;
                        purchaseOrderLineDTO.UnitPrice = price;
                        purchaseOrderLineDTO.SubTotal = detailQuantity * (price + taxAmount);
                        purchaseOrderLineDTO.TaxAmount = taxAmount;
                        purchaseOrderLineDTO.CancelledDate = DateTime.MinValue;
                        purchaseOrderLineDTO.RequisitionId = requisitionId;
                        purchaseOrderLineDTO.RequisitionLineId = requisitionLineId;
                        purchaseOrderLineDTO.PriceInTickets = (DT_receive.Rows[i]["PriceInTickets"].ToString() == "NaN" || DT_receive.Rows[i]["PriceInTickets"] == null || DT_receive.Rows[i]["PriceInTickets"] == DBNull.Value) ? Double.NaN : Convert.ToDouble(DT_receive.Rows[i]["PriceInTickets"]);

                        purchaseOrderLineDTO.PurchaseTaxId = Convert.ToInt32(DT_receive.Rows[i]["TaxId"]);
                        purchaseOrderLineDTO.UOMId = Convert.ToInt32(DT_receive.Rows[i]["UOMId"]);

                        purchaseOrderLine = new PurchaseOrderLine(purchaseOrderLineDTO, executionContext);
                        purchaseOrderLine.Save(SQLTrx);
                        retPurchaseOrderLineId = purchaseOrderLineDTO.PurchaseOrderLineId;
                        ordertotal += detailQuantity * (price + taxAmount);

                    }
                    else
                    {
                        retPurchaseOrderLineId = Convert.ToInt32(DT_receive.Rows[i]["PurchaseOrderLineId"]);
                    }

                    inventoryReceiveLinesDTO = new InventoryReceiveLinesDTO();

                    inventoryReceiveLinesDTO.PurchaseOrderId = detailPurchaseOrderId;
                    inventoryReceiveLinesDTO.ProductId = detailProductId;
                    inventoryReceiveLinesDTO.Description = detailDescription;
                    inventoryReceiveLinesDTO.Quantity = detailQuantity;
                    inventoryReceiveLinesDTO.VendorBillNumber = txtVendorBillNo.Text;
                    inventoryReceiveLinesDTO.ReceivedBy = executionContext.GetUserId();
                    if (receiveLocationID != -1)
                        LocationId = receiveLocationID;
                    else if (LocationId == -1 && Convert.ToInt32(DT_receive.Rows[i]["LocationId"]) != -1)
                        LocationId = Convert.ToInt32(DT_receive.Rows[i]["LocationId"]);
                    else
                    {
                        if (productDTOList != null && productDTOList.Count > 0)
                            LocationId = productDTOList[0].DefaultLocationId;
                    }
                    inventoryReceiveLinesDTO.LocationId = LocationId;
                    inventoryReceiveLinesDTO.ReceiptId = ReceiptId;
                    inventoryReceiveLinesDTO.Price = price;
                    inventoryReceiveLinesDTO.UOMId = Convert.ToInt32(DT_receive.Rows[i]["UOMId"]);

                    // inventoryReceiveLinesDTO.TaxId = -1;

                    inventoryReceiveLinesDTO.IsReceived = detailisReceived;
                    inventoryReceiveLinesDTO.TaxInclusive = DT_receive.Rows[i]["TaxInclusive"].ToString();
                    inventoryReceiveLinesDTO.TaxPercentage = taxPercentage;
                    inventoryReceiveLinesDTO.Amount = (price + taxAmount) * detailQuantity;

                    // for PurchaseOrderReceiveLineDTO -- Update PurchaseOrderReceiveLine Table
                    inventoryReceiveLinesDTO.TaxAmount = Convert.ToDecimal(taxAmount); // added gk
                    inventoryReceiveLinesDTO.PurchaseTaxId = Convert.ToInt32(DT_receive.Rows[i]["TaxId"]);

                    if (retPurchaseOrderLineId != -1)
                        inventoryReceiveLinesDTO.PurchaseOrderLineId = retPurchaseOrderLineId;
                    else
                        inventoryReceiveLinesDTO.PurchaseOrderLineId = -1;
                    inventoryReceiveLinesDTO.PriceInTickets = (DT_receive.Rows[i]["PriceInTickets"].ToString() == "NaN" || DT_receive.Rows[i]["PriceInTickets"] == null || DT_receive.Rows[i]["PriceInTickets"] == DBNull.Value) ? Double.NaN : Convert.ToDouble(DT_receive.Rows[i]["PriceInTickets"]);
                    inventoryReceiveLines = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, executionContext);
                    inventoryReceiveLines.Save(SQLTrx);
                    int receiveLineID;
                    receiveLineID = inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId;
                    RequisitionLinesDTO requisitionLinesDTO;
                    RequisitionLinesList requisitionLinesList = new RequisitionLinesList(executionContext);
                    if (requisitionLineId != -1)
                    {
                        requisitionLinesDTO = requisitionLinesList.GetRequisitionLine(requisitionLineId);

                        if (requisitionLinesDTO != null)
                        {
                            double approvedQty = (requisitionLinesDTO.ApprovedQuantity == -1) ? 0 : requisitionLinesDTO.ApprovedQuantity;
                            approvedQty = approvedQty + detailQuantity;
                            requisitionLinesDTO.ApprovedQuantity = approvedQty;
                            if (approvedQty < requisitionLinesDTO.RequestedQuantity)
                            {
                                requisitionLinesDTO.Status = PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS;
                            }
                            else if (approvedQty == requisitionLinesDTO.RequestedQuantity)
                                requisitionLinesDTO.Status = "Closed";
                            RequisitionLinesBL requisitionLinesBL = new RequisitionLinesBL(executionContext, requisitionLinesDTO);
                            requisitionLinesBL.Save(SQLTrx);
                            RequisitionDTO requisitionDTO = new RequisitionDTO();
                            RequisitionList requisitionlist = new RequisitionList(executionContext);
                            requisitionDTO = requisitionlist.GetRequisition(requisitionId);
                            List<RequisitionLinesDTO> requisitionLinesDTOList = new List<RequisitionLinesDTO>();
                            List<RequisitionDTO> requisitionDTOList = new List<RequisitionDTO>();
                            List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                            inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
                            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionId.ToString()));
                            requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams, SQLTrx);

                            if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
                            {
                                foreach (RequisitionLinesDTO rlDTO in requisitionLinesDTOList)
                                {
                                    if (requisitionLinesDTO.ApprovedQuantity < requisitionLinesDTO.RequestedQuantity)
                                    {
                                        isClosed = false;
                                    }
                                }
                            }
                            if (isClosed)
                                requisitionDTO.Status = "Closed";
                            else
                                requisitionDTO.Status = "InProgress";
                            RequisitionBL requisitionBL = new RequisitionBL(executionContext, requisitionDTO);
                            requisitionBL.Save(SQLTrx);
                        }
                    }

                    if (DT_receive.Rows[i]["LotDetails"] != null && DT_receive.Rows[i]["LotDetails"] != DBNull.Value)
                    {
                        inventoryLotListDTO = new List<InventoryLotDTO>();
                        inventoryLotListDTO = (List<InventoryLotDTO>)DT_receive.Rows[i]["LotDetails"];
                        foreach (InventoryLotDTO inventoryLotDTO in inventoryLotListDTO)
                        {
                            inventoryDTO = new InventoryDTO();
                            inventoryDTO.ProductId = detailProductId;
                            inventoryDTO.LocationId = LocationId;
                            inventoryDTO.ReceivePrice = price;
                            inventoryLotDTO.PurchaseOrderReceiveLineId = receiveLineID;
                            int lotID;
                            inventoryLotDTO.ReceivePrice = price;
                            inventoryLotDTO.OriginalQuantity = inventoryLotDTO.Quantity;
                            inventoryLotDTO.BalanceQuantity = inventoryLotDTO.Quantity;
                            inventoryLotDTO.LotId = -1;
                            inventoryLotDTO.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == detailProductId).InventoryUOMId;
                            InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                            inventoryLotBL.Save(SQLTrx);
                            lotID = inventoryLotDTO.LotId;
                            inventoryDTO.LotId = lotID;
                            inventoryDTO.Quantity = inventoryLotDTO.Quantity;
                            inventoryDTO.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == detailProductId).InventoryUOMId;
                            inventoryDTO.Timestamp = ServerDateTime.Now;
                            inventory = new Inventory(inventoryDTO, executionContext);
                            inventory.Save(SQLTrx);

                            int inventoryTransactionTypeId = -1;
                            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_TRANSACTION_TYPE"));
                            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Receipt"));
                            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                            if (lookupValuesListDTO != null)
                            {

                                inventoryTransactionTypeId = lookupValuesListDTO[0].LookupValueId;
                            }
                            InventoryTransactionDTO inventoryTransactionDTO = new InventoryTransactionDTO(-1, -1, utilities.getServerTime(),
                                                                utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, detailProductId,
                                                                LocationId, Convert.ToDouble(receiveQty), price, taxPercentage, DT_receive.Rows[i]["TaxInclusive"].ToString(),
                                                                receiveLineID, utilities.ParafaitEnv.POSMachineId, inventoryTransactionTypeId, lotID, null, null,
                                                                ProductContainer.productDTOList.Find(x => x.ProductId == detailProductId).InventoryUOMId);
                            InventoryTransactionBL inventoryTransactionBL = new InventoryTransactionBL(inventoryTransactionDTO, executionContext);
                            inventoryTransactionBL.Save(SQLTrx);
                        }
                    }
                    else
                    {
                        // Update inventory with received values.
                        InventoryList inventoryList = new InventoryList();
                        List<InventoryDTO> inventoryListDTO = new List<InventoryDTO>();
                        List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                        inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, LocationId.ToString()));
                        inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, detailProductId.ToString()));
                        inventoryListDTO = inventoryList.GetAllInventory(inventorySearchParams, false, SQLTrx);
                        inventoryDTO = new InventoryDTO();
                        double quantity = 0;
                        if (inventoryListDTO != null && inventoryListDTO.Count > 0)
                        {
                            inventoryDTO = inventoryListDTO[0];
                            quantity = inventoryDTO.Quantity + Convert.ToDouble(receiveQty);
                            inventoryDTO.Quantity = quantity;
                            inventoryDTO.Timestamp = ServerDateTime.Now;
                        }
                        else
                        {
                            inventoryDTO.ProductId = detailProductId;
                            inventoryDTO.LocationId = LocationId;
                            inventoryDTO.ReceivePrice = price;
                            inventoryDTO.Timestamp = ServerDateTime.Now;
                            inventoryDTO.Quantity = Convert.ToDouble(receiveQty);
                            inventoryDTO.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == detailProductId).InventoryUOMId;
                        }
                        inventory = new Inventory(inventoryDTO, executionContext);
                        inventory.Save(SQLTrx);

                        int inventoryTransactionTypeId = -1;
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
                        searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_TRANSACTION_TYPE"));
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Receipt"));
                        searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
                        if (lookupValuesListDTO != null)
                        {

                            inventoryTransactionTypeId = lookupValuesListDTO[0].LookupValueId;
                        }
                        InventoryTransactionDTO inventoryTransactionDTO = new InventoryTransactionDTO(-1, -1, utilities.getServerTime(),
                                                                utilities.ParafaitEnv.LoginID, utilities.ParafaitEnv.POSMachine, detailProductId,
                                                                LocationId, Convert.ToDouble(receiveQty), price, taxPercentage, DT_receive.Rows[i]["TaxInclusive"].ToString(),
                                                                receiveLineID, utilities.ParafaitEnv.POSMachineId, inventoryTransactionTypeId, -1, null, null,
                                                                ProductContainer.productDTOList.Find(x => x.ProductId == detailProductId).InventoryUOMId);

                        InventoryTransactionBL inventoryTransactionBL = new InventoryTransactionBL(inventoryTransactionDTO, executionContext);
                        inventoryTransactionBL.Save(SQLTrx);
                    }
                }

                //Update the PO table with total for a manual po
                if (manual_po && lblOrderDocumentType.Text != "Contract Purchase Order")
                {
                    purchaseOrder.getPurchaseOrderDTO.OrderTotal = ordertotal;
                    //purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                    purchaseOrder.Save(SQLTrx);
                }
                bool orderInProgress = false;

                if (!manual_po && lblOrderDocumentType.Text != "Contract Purchase Order")///* && !isReceiveAutoPO*/)
                {
                    for (int i = 0; i < DT_receive.Rows.Count; i++) //The valid records have been stored in DT_Receive.
                    {
                        if (Convert.ToDouble(DT_receive.Rows[i]["OrderQty"]) - Convert.ToDouble(DT_receive.Rows[i]["Qty"]) > 0)
                        {
                            orderInProgress = true;
                            break;
                        }
                    }
                }

                if (orderInProgress)
                {
                    purchaseOrder.getPurchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS;
                    //purchaseOrder = new PurchaseOrder(purchaseOrderDTO, executionContext);
                    purchaseOrder.Save(SQLTrx);
                }

                manual_po = false;
                productBl = new ProductBL();
                productBl.UpdateProductLastPurchasePrice(ReceiptId, SQLTrx);

                //-update price in product-code added-29-04-2015
                if (utilities.getParafaitDefaults("ENABLE_WEIGHTED_AVERAGE_COST_METHOD") == "Y")
                {
                    DataTable dtProdids = new DataTable();
                    SqlCommand cmdpr = new SqlCommand();
                    cmdpr.Connection = TrxCnn;
                    cmdpr.Transaction = SQLTrx;
                    cmdpr.CommandText = "SELECT DISTINCT  ProductId FROM PurchaseOrderReceive_line WHERE ReceiptId = @ReceiptId";
                    cmdpr.Parameters.Clear();
                    cmdpr.Parameters.AddWithValue("@ReceiptId", ReceiptId);
                    SqlDataAdapter da = new SqlDataAdapter(cmdpr);
                    da.Fill(dtProdids);
                    da.Dispose();
                    if (dtProdids.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtProdids.Rows.Count; i++)
                        {
                            productBl.UpdateProductCost(Convert.ToInt32(dtProdids.Rows[i][0]), ReceiptId, SQLTrx);
                        }
                        UpdatePITUsingReceipt(ReceiptId, "Cost", SQLTrx);
                    }
                }
                else
                {
                    UpdatePITUsingReceipt(ReceiptId, "LastPurchasePrice", SQLTrx);
                }

                if (isReceiveAutoPO && (GetNumberOfPOLinesToBeReceived() > 0 || IsAutoPOReceivedPartially(SQLTrx)))
                {
                    UpdateAutoPOStatus(SQLTrx);
                }

                txtReceiptId.Text = ReceiptId.ToString();
                //-update price in product-code ends-29-04-2015
                SQLTrx.Commit();
                if (isReceiveAutoPO)
                {
                    isReceiveAutoPO = false;
                    searchorder_dgv.Enabled = true;
                    cb_POsearch.Enabled = true;
                    tb_searchorder.Enabled = true;
                    cb_searchstatus.Enabled = true;
                    cmbSearchVendor.Enabled = true;
                    txtProductCode.Enabled = true;
                }

                guid = "";
                log.LogMethodExit();
            }
            catch (Exception Ex)
            {
                log.Error(Ex);
                SQLTrx.Rollback();
                MessageBox.Show(utilities.MessageUtils.getMessage(855) + Ex.Message, utilities.MessageUtils.getMessage("Complete")); 
                return false;
            }
            return true;
        }

        private void UpdatePITUsingReceipt(int receiptID, string CostSource, SqlTransaction SQLTrx = null)
        {
            //log.Debug("Starts-UpdatePITUsingReceipt(int receiptID, SqlTransaction SQLTrx = null) method.");
            InventoryReceiptLineList inventoryReceiptLineList = new InventoryReceiptLineList(executionContext); //added executionContext
            List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParameters;
            searchParameters = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, receiptID.ToString()));
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO = inventoryReceiptLineList.GetAllInventoryReceiveLines(searchParameters, SQLTrx);
            InventoryReceiveLinesBL inventoryReceiptLineBL;
            if (inventoryReceiveLinesListDTO != null)
            {
                foreach (InventoryReceiveLinesDTO inventoryReceiveLinesDTO in inventoryReceiveLinesListDTO)
                {
                    ProductList productList = new ProductList();
                    ProductDTO productDTO = productList.GetProduct(inventoryReceiveLinesDTO.ProductId, SQLTrx);
                    if (productDTO != null)
                    {
                        if (productDTO.IsRedeemable == "Y")
                        {
                            try
                            {
                                if (inventoryReceiveLinesDTO.UOMId == -1)
                                {
                                    inventoryReceiveLinesDTO.UOMId = ProductContainer.productDTOList.Find(x => x.ProductId == productDTO.ProductId).InventoryUOMId;
                                }
                                double newPITValue;
                                if (productDTO.AutoUpdateMarkup)
                                {
                                    if (CostSource == "Cost")
                                        newPITValue = productList.calculatePITByMarkUp(productDTO.Cost, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);
                                    else
                                        newPITValue = productList.calculatePITByMarkUp(productDTO.LastPurchasePrice, productDTO.ItemMarkupPercent, productDTO.DefaultVendorId);

                                    if (productDTO.PriceInTickets != newPITValue)
                                    {
                                        string rangeStart = utilities.getParafaitDefaults("MAINTENANCE_START_HOUR");
                                        string rangeEnd = utilities.getParafaitDefaults("MAINTENANCE_END_HOUR");
                                        double rangeStartValue = (rangeStart != "" ? Convert.ToDouble(rangeStart) : 0.0);
                                        double rangeEndValue = (rangeEnd != "" ? Convert.ToDouble(rangeEnd) : 0.0);
                                        if (GenericUtils.WithInHoursRange(rangeStartValue, rangeEndValue))
                                        {
                                            productDTO.PriceInTickets = newPITValue;
                                            Product.ProductBL productBL = new Product.ProductBL(productDTO);
                                            productBL.Save(SQLTrx);
                                        }
                                        else
                                        {
                                            AddToBatchJobRequest(productDTO, newPITValue, SQLTrx);
                                        }
                                    }
                                }
                                else
                                    newPITValue = productDTO.PriceInTickets;

                                if (inventoryReceiveLinesDTO.PriceInTickets != newPITValue)
                                {
                                    inventoryReceiveLinesDTO.PriceInTickets = newPITValue;
                                    inventoryReceiptLineBL = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, executionContext);
                                    inventoryReceiptLineBL.Save(SQLTrx);
                                }
                            }
                            catch
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1228), utilities.MessageUtils.getMessage("Price in Tickets"));
                            }
                        }
                    }
                }
            }
            //log.Debug("Ends-UpdatePITUsingReceipt(int receiptID, SqlTransaction SQLTrx = null) method.");
        }


        private void AddToBatchJobRequest(ProductDTO productDTO, double newPITValue, SqlTransaction SQLTrx = null)
        {
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            int batchJobActionId = -1;
            int batchJobModuleId = -1;
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchLVParameters;
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_ACTION"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "UPDATE"));
            List<LookupValuesDTO> lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            if (lookupValuesListDTO != null)
            {
                batchJobActionId = lookupValuesListDTO[0].LookupValueId;
            }
            searchLVParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "BATCH_JOB_MODULE"));
            searchLVParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "INVENTORY"));
            lookupValuesListDTO = lookupValuesList.GetAllLookupValues(searchLVParameters);
            if (lookupValuesListDTO != null)
            {
                batchJobModuleId = lookupValuesListDTO[0].LookupValueId;
                BatchJobActivityList batchJobActivityList = new BatchJobActivityList();
                List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>> searchBJAParameters;
                searchBJAParameters = new List<KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>>();
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.MODULE_ID, batchJobModuleId.ToString()));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ACTION_ID, batchJobActionId.ToString()));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYNAME, "Product"));
                searchBJAParameters.Add(new KeyValuePair<BatchJobActivityDTO.SearchByBatchJobActivityParameters, string>(BatchJobActivityDTO.SearchByBatchJobActivityParameters.ENTITYCOLUMN, "PriceInTickets"));
                List<BatchJobActivityDTO> batchJobActivityListDTO = batchJobActivityList.GetAlltBatchJobActivityList(searchBJAParameters);
                if (batchJobActivityListDTO != null)
                {
                    BatchJobRequest batchJobRequest;
                    BatchJobRequestDTO batchJobRequestDTO = new BatchJobRequestDTO();
                    batchJobRequestDTO.BatchJobActivityID = batchJobActivityListDTO[0].BatchJobActivityId;
                    batchJobRequestDTO.EntityGuid = productDTO.Guid;
                    batchJobRequestDTO.EntityColumnValue = newPITValue.ToString();
                    batchJobRequestDTO.ProcesseFlag = false;
                    batchJobRequest = new BatchJobRequest(batchJobRequestDTO);
                    batchJobRequest.Save(SQLTrx);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodEntry();
        }

        private void frm_receive_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((lb_orderid.Text != "") || (receive_dgv.RowCount > 1))
            {
                if (isReceiveAutoPO)
                {
                    CancelCurrentPO(e);
                }
                else
                {
                    if (MessageBox.Show(utilities.MessageUtils.getMessage(127), utilities.MessageUtils.getMessage("Exit Receive Form"), MessageBoxButtons.YesNo) == DialogResult.No)
                        e.Cancel = true;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            POSearch();
        }

        private void POSearch()
        {
            log.LogMethodEntry();
            DataTable DT_POSearch = new DataTable();
            String order_number = "";
            String order_status = "";
            String productSearch = "";
            string condition = "";
            int vendor_id = -1;
            string disAllowMarketListItems = "";
            string amendmentCondition = "";
            string contractPODateCondition = "";

            ClearFields();

            SqlCommand cmd1 = utilities.getCommand();
            order_number = tb_searchorder.Text + "%";
            if (cb_searchstatus.Text == "")
                order_status = "in ('Open', 'InProgress'" + ((!string.IsNullOrEmpty(guid)) ? ",'Received'" : "") + ")";
            else
                order_status = "like '" + cb_searchstatus.Text + "%'";

            disAllowMarketListItems = @" and not exists (select 1 
                                                        from purchaseorder_line l, product p
                                                        where l.purchaseorderid = o.purchaseorderid
                                                            and l.productid = p.productid
                                                            and isnull(p.marketlistitem, 0) = 1) ";
            amendmentCondition = @" and isnull(amendmentnumber, 0) = (select max(isnull(amendmentnumber, 0))
                                                                      from purchaseorder
                                                                      where o.ordernumber = ordernumber)";
            contractPODateCondition = " and (getdate() between cast(fromdate as date) and cast(todate + 1 as date) or (fromdate is null and todate is null)) ";


            if (txtProductCode.Text.Trim() != "")
            {
                //23-Mar-2016
                productSearch = " and exists (select 1 from product p left outer join (select * " +
                                            "from ( " +
                                            "select *, row_number() over(partition by productid order by productid) as num " +
                                            "from productbarcode " +
                                            "where BarCode like @prod and isactive = 'Y')v " +
                                    "where num = 1)b on p.productid = b.productid, PurchaseOrder_Line l where (code like @prod or p.description like @prod or b.BarCode like @prod) and l.ItemCode = p.Code and l.PurchaseOrderId = o.PurchaseOrderId)";
                //23-Mar-2016
            }

            if (cmbSearchVendor.SelectedIndex == -1) //|| (cb_searchvendor.SelectedIndex == 0))
            {
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    condition = " and (site_id = @site_id or @site_id = -1) ";
                    cmd1.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
                }
                cmd1.CommandText = "exec dbo.SetContextInfo @loginUserId ;select PurchaseorderID, OrderNumber, OrderStatus, case when isnull(DocumentStatus, 'D') = 'F' then 'Final' else 'Draft' end DocumentStatus, o.guid " +
                                    "from PurchaseOrder o " +
                                    "where (OrderNumber like  @vorder_number  AND  OrderStatus " + order_status + ") and LastModUserId in (select loginId "
                                                              + @" from DataAccessView dav
                                                               where
                                                            ((Entity = 'Receiving' and dav.DataAccessRuleId is not null)
						                                    OR
                                                           dav.DataAccessRuleId is null
						                                    ))"
                                    + amendmentCondition
                                    + productSearch
                                    + condition
                                    + disAllowMarketListItems
                                    + contractPODateCondition
                                    + "order by orderdate desc";
            }
            else
            {
                vendor_id = Convert.ToInt32(cmbSearchVendor.SelectedValue);
                cmd1.CommandText = "exec dbo.SetContextInfo @loginUserId ;select PurchaseorderID, OrderNumber, OrderStatus, case when isnull(DocumentStatus, 'D') = 'F' then 'Final' else 'Draft' end DocumentStatus, o.guid " +
                                  "from PurchaseOrder o " +
                                  " where LastModUserId in (select loginId "
                                                              + @" from DataAccessView dav
                                                               where
                                                            ((Entity = 'Receiving' and dav.DataAccessRuleId is not null)
						                                    OR
                                                          dav.DataAccessRuleId is null
						                                    )) and (OrderNumber like @vorder_number AND  OrderStatus "
                                  + order_status
                                  + " AND VendorId = @vvendorid)"
                                  + amendmentCondition
                                  + productSearch
                                  + condition
                                  + disAllowMarketListItems
                                  + contractPODateCondition
                                  + "order by orderdate desc";
                cmd1.Parameters.AddWithValue("@vvendorid", vendor_id);
            }


            cmd1.Parameters.AddWithValue("@loginUserId", utilities.ParafaitEnv.User_Id);
            cmd1.Parameters.AddWithValue("@vorder_number", order_number);
            cmd1.Parameters.AddWithValue("@vorder_status", order_status);
            cmd1.Parameters.AddWithValue("@prod", "%" + txtProductCode.Text.Trim() + "%");
            SqlDataAdapter da2 = new SqlDataAdapter(cmd1);
            da2.Fill(DT_POSearch);
            da2.Dispose();
            //ClearFields();
            isDataBound = false;
            searchorder_dgv.DataSource = DT_POSearch;
            searchorder_dgv.Refresh();
            searchorder_dgv.Columns["guid"].Visible = false;
            isDataBound = true;
            if (!string.IsNullOrEmpty(guid))
            {
                if (DT_POSearch != null && DT_POSearch.Rows.Count > 0)
                {
                    bool recordNotFound = true;
                    //searchorder_dgv.SelectedRows[0].Selected = false;
                    for (int i = 0; i < DT_POSearch.Rows.Count; i++)
                    {
                        if (DT_POSearch.Rows[i]["guid"].ToString().ToLower().Equals(guid.ToLower()))
                        {
                            recordNotFound = false;
                            searchorder_dgv.CurrentCell = searchorder_dgv.Rows[i].Cells[1];
                            break;
                        }
                    }
                    if (recordNotFound)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("PO Record not found/expired."));
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("PO Record not found/expired."));
                    this.Close();
                }
            }
            log.LogMethodExit();
        }

        int getDocumentType()
        {
            int documentType = -1;
            InventoryDocumentTypeDTO inventoryDocumentTypeDTO = new InventoryDocumentTypeDTO();
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
            inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("Receipt from Vendor");
            if (inventoryDocumentTypeDTO != null)
                documentType = inventoryDocumentTypeDTO.DocumentTypeId;
            else
                documentType = -1;
            return documentType;
        }

        private void ClearFields()
        {
            txtReceiptId.Text = "";
            cmbVendor.Enabled = true;
            cmbVendor.SelectedIndex = -1;
            tb_order.Text = "";
            tb_contact.Text = "";
            tb_phone.Text = "";
            txt_remarks.Text = "";
            DT_receive.Clear();
            receive_dgv.Rows.Clear();
            tb_date.Text = System.DateTime.Now.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
            tb_status.Text = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
            txtVendorBillNo.Text = "";
            txtGRN.Text = "";
            txtGatePassNo.Text = "";
            dtpReceiveDate.Value = DateTime.Now;
            lblOrderDocumentType.Text = "";
            lb_orderid.Text = "";
            cmbDefaultTax.SelectedIndex = -1;
            cmbLocation.SelectedIndex = -1;
            cmbVendor.SelectedIndex = -1;
            tb_total.Text = string.Empty;
            lblViewRequisitions.Visible = false;
        }

        private void cb_vendor_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int vendor_id = Convert.ToInt32(cmbVendor.SelectedValue.ToString());
            DataTable DT = new DataTable();
            SqlCommand cmd = utilities.getCommand();
            cmd.CommandText = "select  ContactName, Phone, Address1,Address2,City,State,Country,PostalCode" +
                    " from Vendor " +
                    "where VendorId = @vendor AND IsActive = 'Y'";
            cmd.Parameters.AddWithValue("@vendor", vendor_id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(DT);
            da.Dispose();

            if (DT.Rows.Count == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(857), utilities.MessageUtils.getMessage("Vendor Status"));
                cmbVendor.SelectedIndex = -1;
                return;
            }

            tb_contact.Text = DT.Rows[0]["ContactName"].ToString();
            tb_phone.Text = DT.Rows[0]["Phone"].ToString();
        }

        private void cb_create_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (receive_dgv.Rows.Count > 1 && !isReceiveAutoPO)
            {
                if (MessageBox.Show(utilities.MessageUtils.getMessage(856), utilities.MessageUtils.getMessage("Select Order"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            foreach (Form child_form in Application.OpenForms)
            {
                if (child_form.Name == "frm_Order")
                {
                    child_form.Close();
                    break;
                }
            }
            frmOrder frmOrderScreen = new frmOrder(utilities, true);
            CommonUIDisplay.setupVisuals(frmOrderScreen);
            //frmOrderScreen.MdiParent = this.MdiParent;
            //frmOrderScreen.Location = new Point(this.Location.X, this.Location.Y);
            //frmOrderScreen.Width = this.Width;
            //frmOrderScreen.Height = this.Height;
            //frmOrderScreen.AutoScroll = true;
            CommonUIDisplay.SetFormLocationAndSize(this.MdiParent, "Purchase Order", frmOrderScreen);
            this.Close();
            if (this.IsDisposed)
            {
                frmOrderScreen.Show();
            }
            log.LogMethodExit();
        }

        private void receive_dgv_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DataGridViewImageCell dc = new DataGridViewImageCell();
            if (receive_dgv.Rows[e.Row.Index].Cells["IsReceived"].GetType() == dc.GetType())
                e.Cancel = true;
        }

        private void frm_receive_Activated(object sender, EventArgs e)
        {
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            LoadUOMComboBox();
        }

        //For Barcode
        private void serialbarcodeDataReceived()
        {
            log.LogMethodEntry();
            scannedBarcode = BarcodeReader.Barcode;
            if ((scannedBarcode != "") && (tb_status.Text == PurchaseOrderDTO.PurchaseOrderStatus.OPEN))
            {
                if (txtProductCode.Focused)
                {
                    txtProductCode.Text = scannedBarcode;
                    cb_POsearch.PerformClick();
                }
                else if (txt_prodcode.Focused)
                {
                    txt_prodcode.Text = scannedBarcode;
                    cb_search.PerformClick();
                }
                else
                {
                    DataTable DT1 = new DataTable();
                    string condition = "";
                    SqlCommand cmd = utilities.getCommand();
                    if (utilities.ParafaitEnv.IsCorporate)
                    {
                        condition = " and (Product.site_id = @site_id or @site_id = -1)";
                        cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
                    }
                    //Updated query to search barcode from productbarcode
                    //Start update query
                    cmd.CommandText = "select Code, Description, ReorderQuantity, isnull(lastPurchasePrice, cost) cost, product.PurchaseTaxId, t.tax_percentage, product.taxinclusivecost, " +
                                        "LowerLimitCost, UpperLimitCost, CostVariancePercentage, isnull(prd.lotcontrolled, 0) isLottable, isnull(prd.ExpiryType, 'N') ExpiryType, ExpiryDays, DefaultLocationId ,ReceiveToLocationId " +
                        " from Product left outer join Tax t on t.tax_Id = product.PurchaseTaxId " +
                        "left outer join (select * " +
                                        "from (" +
                                                "select *, row_number() over(partition by productid order by productid) as num " +
                                                                     "from productbarcode " +
                                                                     "where BarCode = @barcode and isactive = 'Y')v " +
                                        "where num = 1) b on b.productid = product.productid " +
                            "where b.BarCode = @barcode AND product.IsActive = 'Y' and IsPurchaseable = 'Y' " + condition;
                    //End update query
                    cmd.Parameters.AddWithValue("@barcode", scannedBarcode);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(DT1);
                    da.Dispose();

                    if (DT1.Rows.Count < 1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(858), utilities.MessageUtils.getMessage("Error"));
                    }
                    else if (DT1.Rows.Count == 1)
                    {
                        int ReceiveToLocationId = Convert.ToInt32(DT1.Rows[0]["ReceiveToLocationId"]);
                        if (!isStoreLocation(ReceiveToLocationId))
                            ReceiveToLocationId = -1;

                        fireCellValueChange = false;
                        int rowindex = receive_dgv.Rows.Add(
                                    DT1.Rows[0]["Code"],
                                    DT1.Rows[0]["Description"],
                                    DT1.Rows[0]["ReorderQuantity"],
                                    DT1.Rows[0]["cost"],
                                    DT1.Rows[0]["PurchaseTaxId"] == null || DT1.Rows[0]["PurchaseTaxId"] == DBNull.Value ? -1 : DT1.Rows[0]["PurchaseTaxId"],
                                    DT1.Rows[0]["tax_Percentage"],
                                    DT1.Rows[0]["taxInclusiveCost"] == null || DT1.Rows[0]["taxInclusiveCost"] == DBNull.Value ? "N" : DT1.Rows[0]["taxInclusiveCost"],
                                    0,
                                    DBNull.Value,
                                    "Receive",
                                    DBNull.Value,
                                    DBNull.Value,
                                    DT1.Rows[0]["LowerLimitCost"],
                                    DT1.Rows[0]["UpperLimitCost"],
                                    DT1.Rows[0]["CostVariancePercentage"],
                                    DT1.Rows[0]["cost"],
                                    DT1.Rows[0]["isLottable"],
                                    DT1.Rows[0]["ExpiryType"],
                                    DT1.Rows[0]["ExpiryDays"],
                                    DT1.Rows[0]["productid"],
                                    -1,
                                    -1,
                                    null,
                                    ReceiveToLocationId,
                                    DT1.Rows[0]["PriceInTickets"]
                                    //  DT1.Rows[0]["TaxAmount"]
                                    );

                        DataGridViewButtonCell cb = new DataGridViewButtonCell();
                        cb.Value = "Receive";
                        receive_dgv["IsReceived", rowindex] = cb;
                        receive_dgv["Qty", rowindex].ReadOnly = false;
                        fireCellValueChange = true;
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(859), utilities.MessageUtils.getMessage("Error"));
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnReceipts_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            foreach (Form child_form in Application.OpenForms)
            {
                if (child_form.Name == "frm_receipts")
                {
                    child_form.Close();
                    break;
                }
            }
            frmInventoryReceipt frm = new frmInventoryReceipt(-1, utilities);
            CommonUIDisplay.setupVisuals(frm);//Setup GUI Design Style Added on 23-Aug-2016
            frm.Text = "Receipts";
            frm.MdiParent = this.MdiParent;
            frm.Location = new Point(this.Location.X, this.Location.Y);
            frm.Width = this.Width;
            frm.Height = this.Height;
            frm.Show();
            log.LogMethodExit();
        }

        private void receive_dgv_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["TaxId"].Value = cmbDefaultTax.SelectedValue;
            e.Row.Cells["TaxInclusive"].Value = "N";
            e.Row.Cells["recvLocation"].Value = -1;
        }

        private void receive_dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            receive_dgv.EndEdit();
            if (receive_dgv.Columns[e.ColumnIndex].Name == "Qty")
            {
                if (receive_dgv["ProductId", e.RowIndex].Value != DBNull.Value && receive_dgv["ProductId", e.RowIndex].Value != null)
                {
                    if (Convert.ToDecimal((receive_dgv["Qty", e.RowIndex].Value == DBNull.Value || receive_dgv["Qty", e.RowIndex].Value == null) ? 0 : receive_dgv["Qty", e.RowIndex].Value) <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1091), utilities.MessageUtils.getMessage(1092));
                        return;
                    }
                }
            }
            //There is no column in the receive_dgv grid called "Cost".
            //if (lb_orderid.Text == "")
            //{
            //    if (receive_dgv.Columns[e.ColumnIndex].Name == "Cost")
            //    {
            //        decimal lowerLimit = Convert.ToDecimal(receive_dgv["LowerLimitCost", e.RowIndex].Value == DBNull.Value ? 0 : receive_dgv["LowerLimitCost", e.RowIndex].Value);
            //        decimal upperLimit = Convert.ToDecimal(receive_dgv["UpperLimitCost", e.RowIndex].Value == DBNull.Value ? decimal.MaxValue : receive_dgv["UpperLimitCost", e.RowIndex].Value);
            //        decimal costVariance = Convert.ToDecimal(receive_dgv["CostVariancePercent", e.RowIndex].Value == DBNull.Value ? -1 : receive_dgv["CostVariancePercent", e.RowIndex].Value);
            //        decimal cost;
            //        if (receive_dgv.Columns[e.ColumnIndex].Name == "Cost")
            //            cost = Convert.ToDecimal((e.FormattedValue == null || e.FormattedValue.ToString() == "" || e.FormattedValue == DBNull.Value) ? 0 : e.FormattedValue);
            //        else
            //            cost = Convert.ToDecimal(receive_dgv["Cost", e.RowIndex].Value == DBNull.Value ? -1 : receive_dgv["Cost", e.RowIndex].Value);
            //        decimal origcost = Convert.ToDecimal(receive_dgv["OrigPrice", e.RowIndex].Value == DBNull.Value ? 0 : receive_dgv["OrigPrice", e.RowIndex].Value);

            //        if (lowerLimit == 0 && upperLimit == decimal.MaxValue)
            //        {
            //            if (costVariance != -1)
            //            {
            //                if (cost < origcost * (1 - costVariance / 100) || cost > origcost * (1 + costVariance / 100))
            //                {
            //                    if (utilities.ParafaitEnv.Manager_Flag == "Y")
            //                    {
            //                        if (MessageBox.Show(utilities.MessageUtils.getMessage(860, costVariance.ToString(), origcost.ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT)), utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
            //                            e.Cancel = true;
            //                    }
            //                    else
            //                    {
            //                        MessageBox.Show(utilities.MessageUtils.getMessage(861, costVariance.ToString(), origcost.ToString(utilities.ParafaitEnv.INVENTORY_COST_FORMAT)), utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                        if (!Semnox.Parafait.Inventory.Authenticate.Manager())
            //                            e.Cancel = true;
            //                    }
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (lowerLimit > 0 && upperLimit > 0)
            //            {
            //                if (cost < lowerLimit || cost > upperLimit)
            //                {
            //                    if (utilities.ParafaitEnv.Manager_Flag == "Y")
            //                    {
            //                        if (MessageBox.Show(utilities.MessageUtils.getMessage(862, lowerLimit.ToString(), upperLimit.ToString()), utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
            //                            e.Cancel = true;
            //                    }
            //                    else
            //                    {
            //                        MessageBox.Show(utilities.MessageUtils.getMessage(863, lowerLimit.ToString(), upperLimit.ToString()), utilities.MessageUtils.getMessage("Cost Validation"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                        if (!Semnox.Parafait.Inventory.Authenticate.Manager())
            //                            e.Cancel = true;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            log.LogMethodExit();
        }

        private void recalculateTotal()
        {
            log.LogMethodEntry();
            decimal total = 0;
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                if (receive_dgv["Amount", i].Value != null)
                    total += Convert.ToDecimal(receive_dgv["Amount", i].Value);
            }
            tb_total.Text = String.Format("{0:N2}", total);
            log.LogMethodExit();
        }

        private void dtpReceiveDate_ValueChanged(object sender, EventArgs e)
        {
            changeRequiredByDateBackColor();
        }

        void changeRequiredByDateBackColor()
        {
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                if (receive_dgv["RequiredByDate", i].Value != DBNull.Value && receive_dgv["RequiredByDate", i].Value != null)
                {
                    if (Convert.ToDateTime(receive_dgv["RequiredByDate", i].Value).Date < dtpReceiveDate.Value.Date)
                        receive_dgv["RequiredByDate", i].Style.BackColor = Color.OrangeRed;
                    else
                        receive_dgv["RequiredByDate", i].Style.BackColor = Color.LightGreen;
                }
            }
        }

        private void lnkApplyTaxToAllLines_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                if (receive_dgv["ProductCode", i].Value != null)
                    receive_dgv["TaxId", i].Value = cmbDefaultTax.SelectedValue;
            }
        }


        //Begin: Modification done for removing side Receive button on Form Close on 22-Aug-2016
        private void frm_receive_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Begin: Modification done for closing Product Tabular Form if Product Form is closed on 01-Sep-2016
            foreach (Form child_form in Application.OpenForms)
            {
                if (child_form.Name == "frm_receipts")
                {
                    child_form.Dispose();
                    break;
                }
            }
        }
        //End: Modification done for removing side Receive button on Form Close on 22-Aug-2016

        private void searchorder_dgv_SelectionChanged(object sender, EventArgs e)
        {
            searchorder_dgv_selectionChanged();
        }

        private void searchorder_dgv_selectionChanged()
        {
            log.LogMethodEntry();
            int index;
            try
            {
                index = searchorder_dgv.SelectedRows[0].Index;
            }
            catch
            {
                return;
            }
            if (index < 0) //Header clicked
                return;

            if (searchorder_dgv.RowCount < 1)
                return;
            if (searchorder_dgv["OrderStatus", searchorder_dgv.CurrentRow.Index].Value.ToString() == PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED)
                return;
            if (searchorder_dgv["OrderStatus", searchorder_dgv.CurrentRow.Index].Value.ToString() == PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED && string.IsNullOrEmpty(guid))
                return;
            if (searchorder_dgv["OrderStatus", searchorder_dgv.CurrentRow.Index].Value.ToString() == PurchaseOrderDTO.PurchaseOrderStatus.SHORTCLOSE)
                return;
            if (searchorder_dgv["DocumentStatus", searchorder_dgv.CurrentRow.Index].Value.ToString() != "Final")
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1106));
                ClearFields();
                return;
            }

            DataTable DT = new DataTable();
            SqlCommand cmd = utilities.getCommand();
            string condition = "";
            if (utilities.ParafaitEnv.IsCorporate)
            {
                condition = " and (o.site_id = @site_id or @site_id = -1)";
                cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
            }
            cmd.CommandText = "select  o.*, isnull(d.name, 'Regular Purchase Order') DocumentType, v.ContactName vContactName, v.Phone vPhone " +
                            " from PurchaseOrder o left outer join inventorydocumenttype d on o.documenttypeid = d.documenttypeid " +
                            " left outer join vendor v on o.vendorid = v.vendorid " +
                            "where PurchaseorderID = @PurchaseorderID" + condition;
            cmd.Parameters.AddWithValue("@PurchaseorderID", searchorder_dgv["PurchaseorderID", index].Value.ToString());
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DT = new DataTable();
            da.Fill(DT);
            da.Dispose();

            ClearFields();

            lblOrderDocumentType.Text = DT.Rows[0]["DocumentType"].ToString();
            lb_orderid.Text = DT.Rows[0]["PurchaseOrderId"].ToString();
            cmbVendor.SelectedValue = Convert.ToInt32(DT.Rows[0]["VendorId"].ToString());
            tb_order.Text = DT.Rows[0]["OrderNumber"].ToString();
            tb_contact.Text = DT.Rows[0]["vContactName"].ToString();
            DateTime shortdate = Convert.ToDateTime(DT.Rows[0]["OrderDate"]);
            tb_date.Text = shortdate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
            tb_phone.Text = DT.Rows[0]["vPhone"].ToString();
            tb_status.Text = DT.Rows[0]["OrderStatus"].ToString();
            if (!string.IsNullOrEmpty(guid) && tb_status.Text.Equals(PurchaseOrderDTO.PurchaseOrderStatus.RECEIVED))
            {
                cb_complete.Enabled = cb_create.Enabled = btnReceipts.Enabled = false;
            }
            cmbVendor.Enabled = false;
            if (isDataBound && !searchorder_dgv["guid", index].Value.ToString().ToLower().Equals(guid))
            {
                guid = string.Empty;
            }

            refreshReceiveLines(Convert.ToInt32(lb_orderid.Text));
            log.LogMethodExit();
        }

        private void refreshReceiveLines(int orderID)
        {
            log.LogMethodEntry(orderID);
            try
            {
                SqlCommand cmd1 = utilities.getCommand();
                // Added TaxAmount column and group by clause by GK
                if (lblOrderDocumentType.Text == "" || lblOrderDocumentType.Text == "Regular Purchase Order")
                {
                    cmd1.CommandText = @"select pol.productid, pol.itemcode, pol.description, pol.UOMId , pol.UOM, pol.ordqty - case when pol.requisitionlineid is not null then isnull(pol.ApprovedQuantity, 0) else isnull(sum(r.quantity), 0) end remqty ,
                                            pol.UnitPrice, pol.PurchaseTaxId, pol.tax_percentage, TaxInclusiveCost,pol.TaxAmount, pol.RequiredByDate, pol.PurchaseOrderLineId, 
                                            LowerLimitCost, UpperLimitCost, CostVariancePercentage, pol.isLottable, pol.ExpiryType, pol.requisitionid, pol.RequisitionLineId, ReceiveLocation, pol.ExpiryDays, pol.PriceInTickets 
                                        from (select pl.purchaseorderid, pl.PurchaseOrderLineId, prd.productid, prd.code itemcode, prd.description, isnull(pl.UOMId, (select uomId from Product where ProductId = pl.ProductId)) UOMId, u.UOM ,
		                                        pl.UnitPrice, pl.PurchaseTaxId,t.tax_Percentage, prd.TaxInclusiveCost,pl.TaxAmount, pl.RequiredByDate, case when irl.requisitionlineid is not null and pl.OriginalReferenceGUID is null then irl.RequestedQuantity else quantity end ordqty,
		                                        prd.LowerLimitCost, prd.UpperLimitCost, prd.CostVariancePercentage 
		                                        ,isnull(prd.lotcontrolled, 0) isLottable, isnull(prd.ExpiryType, 'N') ExpiryType, pl.requisitionid, pl.RequisitionLineId
		                                        ,isnull(irl.ApprovedQuantity, 0) ApprovedQuantity
		                                        ,isnull(ir.todepartment, prd.defaultlocationid) ReceiveLocation, ExpiryDays, prd.PriceInTickets
                                             from product prd   , purchaseorder_line pl left outer join Tax t on t.tax_Id= pl.purchaseTaxId left outer join UOM u on u.UOMId = pl.UOMId 
		                                        left outer join inventoryrequisition ir on pl.requisitionid = ir.requisitionid 
		                                        left outer join inventoryrequisitionlines irl on irl.requisitionid = pl.requisitionid and irl.requisitionlineid = pl.requisitionlineid and isnull(irl.IsActive, 0) = 1
                                             where pl.purchaseorderid = @vorder_id
		                                        and pl.productId = prd.productId 
                                               and isnull(pl.isactive, 'Y') = 'Y' 
                                            ) pol 
	                                        left outer join purchaseorderreceive_line r on pol.purchaseorderid = r.purchaseorderid and pol.PurchaseOrderLineId = r.PurchaseOrderLineId and pol.productid = r.productid 
                                        group by  pol.productid, pol.itemcode, pol.description, pol.ordqty, pol.PurchaseOrderLineId,  pol.UOM , pol.UOMId ,
                                            pol.UnitPrice, pol.PurchaseTaxId, pol.tax_percentage, TaxInclusiveCost, pol.RequiredByDate, 
                                            LowerLimitCost, UpperLimitCost, CostVariancePercentage, isLottable, ExpiryType, pol.requisitionid, pol.RequisitionLineId, pol.ApprovedQuantity, ReceiveLocation, ExpiryDays, pol.PriceInTickets,pol.TaxAmount "
                                            + ((string.IsNullOrEmpty(guid)) ? " having pol.ordqty - case when pol.requisitionlineid is not null then isnull(pol.ApprovedQuantity, 0) else isnull(sum(r.quantity), 0) end > 0" : "");
                }
                else
                {
                    cmd1.CommandText = @"select pol.productid, pol.itemcode, pol.description,  pol.UOMId , pol.UOM ,
	                                    pol.ordqty remqty,
                                        pol.UnitPrice, pol.PurchaseTaxId, pol.tax_Percentage, TaxInclusiveCost,pol.TaxAmount, pol.RequiredByDate, pol.PurchaseOrderLineId, 
                                        LowerLimitCost, UpperLimitCost, CostVariancePercentage, pol.isLottable, pol.ExpiryType, pol.requisitionid, 
	                                    pol.requisitionlineid, pol.ReceiveLocation, pol.ExpiryDays, pol.PriceInTickets
                                    from (select pl.purchaseorderid, pl.PurchaseOrderLineId, prd.productid, prd.code itemcode, prd.description, isnull(pl.UOMId, (select uomId from Product where ProductId = pl.ProductId)) UOMId,u.UOM ,
		                                    pl.UnitPrice, pl.PurchaseTaxId, t.tax_percentage, prd.TaxInclusiveCost,pl.TaxAmount, pl.RequiredByDate, quantity ordqty, 
		                                    prd.LowerLimitCost, prd.UpperLimitCost, prd.CostVariancePercentage 
		                                    ,isnull(prd.lotcontrolled, 0) isLottable, isnull(prd.ExpiryType, 'N') ExpiryType, 
		                                    -1 requisitionid, -1 requisitionlineid, prd.defaultlocationid ReceiveLocation, ExpiryDays, prd.PriceInTickets
	                                        from product prd , purchaseorder_line pl left outer join Tax t on t.tax_Id= pl.purchaseTaxId left outer join UOM u on u.UOMId = pl.UOMId 
		                                    where pl.purchaseorderid = @vorder_id
		                                    and pl.productId = prd.productId 
                                            and isnull(pl.isactive, 'Y') = 'Y' 
	                                        ) pol 
                                    group by  pol.productid, pol.itemcode, pol.description, pol.ordqty, pol.PurchaseOrderLineId, pol.UOM , pol.UOMId ,
		                                    pol.UnitPrice, pol.PurchaseTaxId, pol.tax_Percentage, TaxInclusiveCost, pol.RequiredByDate, 
		                                    LowerLimitCost, UpperLimitCost, CostVariancePercentage, isLottable, ExpiryType, pol.requisitionid, pol.requisitionlineid, pol.ReceiveLocation, pol.ExpiryDays, pol.PriceInTickets ,pol.TaxAmount";
                }
                cmd1.Parameters.Clear();
                cmd1.Parameters.AddWithValue("@vorder_id", orderID);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable DT1 = new DataTable();
                da1.Fill(DT1);
                da1.Dispose();

                for (int i = 0; i < DT1.Rows.Count; i++)
                {
                    int ReceiveToLocationId = Convert.ToInt32(DT1.Rows[i]["ReceiveLocation"]);
                    if (!isStoreLocation(ReceiveToLocationId))
                        ReceiveToLocationId = -1;
                    InventoryList inventoryList = new InventoryList();
                    receive_dgv.Rows.Add(DT1.Rows[i]["itemcode"],
                                        DT1.Rows[i]["description"],
                                        DT1.Rows[i]["UOM"],
                                        DT1.Rows[i]["remqty"],
                                        //-1,
                                        DT1.Rows[i]["UOMId"],
                                        DT1.Rows[i]["UnitPrice"],
                                        DT1.Rows[i]["PurchaseTaxId"],
                                        DT1.Rows[i]["tax_Percentage"],
                                         DT1.Rows[i]["TaxInclusiveCost"],
                                         DT1.Rows[i]["TaxAmount"],        //added  12-04-2019 
                                        0,
                                        DT1.Rows[i]["RequiredByDate"],
                                        "Receive",
                                        DT1.Rows[i]["remqty"],
                                        DT1.Rows[i]["PurchaseOrderLineId"],
                                        DT1.Rows[i]["LowerLimitCost"],
                                        DT1.Rows[i]["UpperLimitCost"],
                                        DT1.Rows[i]["CostVariancePercentage"],
                                        DT1.Rows[i]["UnitPrice"],
                                        DT1.Rows[i]["isLottable"],
                                        DT1.Rows[i]["ExpiryType"],
                                        DT1.Rows[i]["ExpiryDays"],
                                        DT1.Rows[i]["productid"],
                                        (DT1.Rows[i]["RequisitionId"] == null || DT1.Rows[i]["RequisitionId"] == DBNull.Value) ? -1 : DT1.Rows[i]["RequisitionId"],
                                        (DT1.Rows[i]["RequisitionLineId"] == null || DT1.Rows[i]["RequisitionLineId"] == DBNull.Value) ? -1 : DT1.Rows[i]["RequisitionLineId"],
                                        null,
                                        ReceiveToLocationId,
                                        DT1.Rows[i]["PriceInTickets"],
                                        inventoryList.GetProductStockQuantity(Convert.ToInt32(DT1.Rows[i]["productid"]))
                                        );

                    calculateAmount(i);
                }

                receive_dgv.AllowUserToAddRows = false;
                for (int j = 0; j < receive_dgv.RowCount; j++)
                {
                    if (receive_dgv["ProductId", j].Value != null && receive_dgv["ProductId", j].Value != DBNull.Value)
                    {
                        if (receive_dgv["IsReceived", j].Value.ToString() == "Y")
                        {
                            DataGridViewImageCell ic = new DataGridViewImageCell();
                            ic.Value = Properties.Resources.status_ok;
                            ic.ImageLayout = DataGridViewImageCellLayout.Normal;
                            receive_dgv["IsReceived", j] = ic;
                            receive_dgv.Rows[j].ReadOnly = true;
                        }
                        else
                        {
                            DataGridViewButtonCell cb = new DataGridViewButtonCell();
                            cb.Value = "Receive";
                            receive_dgv["IsReceived", j] = cb;
                        }
                        int uomId = -1;
                        try
                        {
                            uomId = Convert.ToInt32(DT1.Rows[j]["UOMId"].ToString());
                            if (uomId == -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(DT1.Rows[j]["productid"])).UomId;
                            }
                            CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, j, uomId);
                            receive_dgv.Rows[j].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                        }
                    }
                }

                receive_dgv.ReadOnly = false;
                receive_dgv.Columns["ProductCode"].ReadOnly = true;
                receive_dgv.Columns["Description"].ReadOnly = true;
                if (!string.IsNullOrEmpty(guid))
                {
                    receive_dgv.Columns["TaxInclusive"].ReadOnly = false;
                    receive_dgv.Columns["Price"].ReadOnly = false;
                    receive_dgv.Columns["TaxId"].ReadOnly = false;
                }
                else
                {
                    receive_dgv.Columns["TaxInclusive"].ReadOnly = true;
                    receive_dgv.Columns["Price"].ReadOnly = true;
                    receive_dgv.Columns["TaxId"].ReadOnly = true;
                    receive_dgv.Columns["TaxAmount"].ReadOnly = true; //added by GK
                }
                receive_dgv.Columns["TaxPercentage"].ReadOnly = true;
                receive_dgv.Columns["Amount"].ReadOnly = true; //added by GK
                receive_dgv.Columns["TaxAmount"].ReadOnly = true;
                receive_dgv.Columns["RequiredByDate"].ReadOnly = true;
                receive_dgv.Columns["Qty"].ReadOnly = false;

                changeRequiredByDateBackColor();
                LoadUOMComboBox();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private bool isStoreLocation(int ReceiveToLocationId)
        {
            SqlCommand cmd1 = utilities.getCommand();
            cmd1.CommandText = @"select isnull(LocationType, '') LocationType
                                    from location l left outer join locationtype t on l.locationtypeid = t.locationtypeid
                                    where locationid = @locationid";
            cmd1.Parameters.Clear();
            cmd1.Parameters.AddWithValue("@locationid", ReceiveToLocationId);
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd1);
            da2.Fill(dt2);
            if (dt2.Rows[0]["LocationType"].ToString() != "Store")
                return false;
            else
                return true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DT_receive.Rows.Clear();
            receive_dgv.Rows.Clear();
            cb_complete.Text = MessageContainerList.GetMessage(executionContext, "Save");
            cb_complete.Tag = "Save";
            refreshReceiveLines(lb_orderid.Text == "" ? -1 : Convert.ToInt32(lb_orderid.Text));
            if ((lb_orderid.Text == "" ? -1 : Convert.ToInt32(lb_orderid.Text)) == -1)
            {
                receive_dgv.ReadOnly = false;
                receive_dgv.AllowUserToAddRows = true;
                ProductCode.ReadOnly = false;
            }
            updateReceiveLineLocation();
        }

        private void lblViewRequisitions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            frmListRequisitions pr = new frmListRequisitions(utilities, "RECEIVE", -1);
            CommonUIDisplay.setupVisuals(pr);//Added to Style GUI Design on 15-Sep-2016
            pr.StartPosition = FormStartPosition.CenterScreen;//Added to show at center of screen on 15-Sep-2016

            if (pr.SelectedRequisitionId != -1 || pr.ShowDialog() != DialogResult.Cancel)
            {
                int requisitionId = (int)(pr.SelectedRequisitionId);

                DataTable DT = new DataTable();
                SqlCommand cmd = utilities.getCommand();
                string condition = "";
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    condition = " and (InventoryRequisitionLines.site_id = @site_id or @site_id = -1)";
                    cmd.Parameters.AddWithValue("@site_id", executionContext.GetSiteId());
                }
                InventoryDocumentTypeList inventoryDocumentTypeList = null;
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = null;
                if (pr.SelectedRequisitionDTO != null)
                {
                    inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                    inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType(pr.SelectedRequisitionDTO.RequisitionType);
                }
                if (inventoryDocumentTypeDTO != null && inventoryDocumentTypeDTO.Code.Equals("ITRQ"))
                {
                    //updated query to search barcode in productbarcode table
                    //23-Mar-2016
                    cmd.CommandText = @"select Code, product.Description, InventoryRequisitionLines.RequestedQuantity, isnull(Marketlistitem, 0) Marketlistitem, product.PurchaseTaxId,
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, tax_Percentage,
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/100 * isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else 0 end as taxAmount, taxInclusiveCost,
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) * ReorderQuantity as SubTotal, 
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) as CPOSubTotal, 
	                                LowerLimitCost, UpperLimitCost, CostVariancePercentage, product.ProductId, uom, InventoryRequisitionLines.requisitionid, InventoryRequisitionLines.requisitionlineid, InventoryRequisitionLines.ExpectedReceiptDate, isnull(product.lotcontrolled, 0) isLottable, isnull(product.ExpiryType, 'N') ExpiryType, ExpiryDays,  product.defaultlocationid ReceiveLocation
                                from inventoryrequisition, InventoryRequisitionLines, Product left outer join Tax on Tax.tax_Id = product.PurchaseTaxId 
	                                left outer join uom on uom.uomid = product.uomid 
                                where Product.productid  = (select prd.MasterEntityId from Product prd where prd.ProductId = InventoryRequisitionLines.productid )  
	                                AND Product.IsActive = 'Y' 
	                                and isPurchaseable = 'Y' 
                                    and inventoryrequisition.requisitionid = InventoryRequisitionLines.requisitionid
	                                and isnull(InventoryRequisitionLines.IsActive, 0) = 1
	                                and InventoryRequisitionLines.RequisitionId = @RequisitionId ";
                }
                else
                {
                    //updated query to search barcode in productbarcode table
                    //23-Mar-2016
                    cmd.CommandText = @"select Code, product.Description, InventoryRequisitionLines.RequestedQuantity, isnull(Marketlistitem, 0) Marketlistitem, product.PurchaseTaxId,
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) end else isnull(LastPurchasePrice, cost) end as cost, tax_Percentage,
	                                case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then tax_Percentage/100 * isnull(LastPurchasePrice, cost)/(1 + tax_Percentage/100) else isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else 0 end as taxAmount, taxInclusiveCost,
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) * ReorderQuantity as SubTotal, 
	                                (case when product.PurchaseTaxId is not null then case taxInclusiveCost when 'Y' then isnull(LastPurchasePrice, cost) else isnull(LastPurchasePrice, cost) + isnull(LastPurchasePrice, cost) * tax_Percentage/100 end else isnull(LastPurchasePrice, cost) end) as CPOSubTotal, 
	                                LowerLimitCost, UpperLimitCost, CostVariancePercentage, product.ProductId, uom, InventoryRequisitionLines.requisitionid, InventoryRequisitionLines.requisitionlineid, InventoryRequisitionLines.ExpectedReceiptDate, isnull(product.lotcontrolled, 0) isLottable, isnull(product.ExpiryType, 'N') ExpiryType, ExpiryDays,  product.defaultlocationid ReceiveLocation
                                from inventoryrequisition, InventoryRequisitionLines, Product left outer join   Tax on Tax.tax_Id = product.PurchaseTaxId 
	                                left outer join uom on uom.uomid = product.uomid 
                                where Product.productid = InventoryRequisitionLines.productid 
	                                AND Product.IsActive = 'Y' 
	                                and isPurchaseable = 'Y' 
                                    and inventoryrequisition.requisitionid = InventoryRequisitionLines.requisitionid
	                                and isnull(InventoryRequisitionLines.IsActive, 0) = 1
	                                and InventoryRequisitionLines.RequisitionId = @RequisitionId " + condition;
                }
                cmd.Parameters.AddWithValue("@RequisitionId", requisitionId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DT);
                if (DT.Rows.Count == 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1101));
                    return;
                }

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    int ReceiveToLocationId = Convert.ToInt32(DT.Rows[i]["ReceiveLocation"]);
                    if (!isStoreLocation(ReceiveToLocationId))
                        ReceiveToLocationId = -1;
                    fireCellValueChange = false;
                    fireCellValueChange = false;
                    int rowindex = receive_dgv.Rows.Add(
                                DT.Rows[0]["Code"],
                                DT.Rows[0]["Description"],
                                DT.Rows[0]["RequestedQuantity"],
                                DT.Rows[0]["cost"],
                                DT.Rows[0]["PurchaseTaxId"],
                                DT.Rows[0]["tax_Percentage"],
                                "N",// DT.Rows[0]["taxInclusiveCost"], getting unit price from query 
                                DT.Rows[0]["taxAmount"], // add GK
                                0,
                                DT.Rows[0]["ExpectedReceiptDate"],
                                "Receive",
                                DBNull.Value,
                                DBNull.Value,
                                DT.Rows[0]["LowerLimitCost"],
                                DT.Rows[0]["UpperLimitCost"],
                                DT.Rows[0]["CostVariancePercentage"],
                                DT.Rows[0]["cost"],
                                DT.Rows[0]["isLottable"],
                                DT.Rows[0]["ExpiryType"],
                                DT.Rows[i]["ExpiryDays"],
                                DT.Rows[0]["Productid"],
                                DT.Rows[0]["RequisitionId"],
                                DT.Rows[0]["RequisitionLineId"],
                                null,
                                ReceiveToLocationId
                                );

                    DataGridViewButtonCell cb = new DataGridViewButtonCell();
                    cb.Value = "Receive";
                    receive_dgv["IsReceived", rowindex] = cb;
                    try
                    {
                        CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, dgvselectedindex, Convert.ToInt32(DT.Rows[rowindex]["UOMId"]));
                        receive_dgv["txtUOM", dgvselectedindex].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == Convert.ToInt32(DT.Rows[rowindex]["UOMId"])).UOM;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    receive_dgv["Qty", rowindex].ReadOnly = false;
                    fireCellValueChange = true;
                    fireCellValueChange = true;
                }

            }
            log.LogMethodExit();
        }

        private void cmbLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateReceiveLineLocation();
        }

        private void updateReceiveLineLocation()
        {
            if (cmbLocation.SelectedIndex != -1)
            {
                if (receive_dgv != null && receive_dgv.Rows.Count > 0)
                {
                    for (int i = 0; i < receive_dgv.Rows.Count; i++)
                    {
                        if (receive_dgv.Rows[i].Cells["ProductCode"].Value != null)
                        {
                            if (Convert.ToInt32(cmbLocation.SelectedValue) != -1)
                                receive_dgv.Rows[i].Cells["recvLocation"].Value = cmbLocation.SelectedValue;
                        }
                    }
                }
            }
        }

        private void lnkReceiveAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                receive_dgv.CurrentCell = receive_dgv.Rows[i].Cells[0];
                if (receive_dgv.CurrentCell.Value != null &&
                    receive_dgv["IsReceived", receive_dgv.CurrentRow.Index].ValueType.FullName != "System.Drawing.Image")
                {
                    receiveValidationResult = true;
                    receive_dgv_CellContentClick(this.receive_dgv, new DataGridViewCellEventArgs(receive_dgv.Columns["IsReceived"].Index, i));
                    if (!receiveValidationResult)
                    {
                        break;
                    }
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (inventoryReceiptId != -1)
                {
                    string reportFileName = PrintInventoryReceiveReceipt();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1819, ex.Message));//Error while printing the document. Error message: &1
            }
            log.LogMethodExit();
        }

        private string PrintInventoryReceiveReceipt()
        {
            log.LogMethodEntry();
            List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
            reportParam.Add(new clsReportParameters.SelectedParameterValue("ReceiptId", inventoryReceiptId));
            ReceiptReports receiptReports = new ReceiptReports(executionContext, "InventoryReceiveReceipt", "", null, null, reportParam, "P");
            string reportFileName = receiptReports.GenerateAndPrintReport(false);
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }

        private bool SetupThePrinting(PrintDocument MyPrintDocument)
        {
            log.LogMethodEntry(MyPrintDocument);
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;
            MyPrintDocument.DocumentName = utilities.MessageUtils.getMessage("InventoryReceiptDetails" + "-" + inventoryReceiptId);
            MyPrintDialog.UseEXDialog = true;
            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);
            log.LogMethodExit(true);
            return true;
        }

        private void ValidateAutoPOReceive()
        {
            log.LogMethodEntry();
            string errorMessagePartial = string.Empty;
            string errorMessageNotReceived = string.Empty;
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                if (receive_dgv["IsReceived", i].ValueType.FullName != "System.Drawing.Image")
                {
                    errorMessageNotReceived = errorMessageNotReceived + "'" + receive_dgv["Description", i].Value.ToString() + "',";
                }
                if (Convert.ToDecimal(receive_dgv["OrderedQty", i].Value) > Convert.ToDecimal(receive_dgv["Qty", i].Value))
                {
                    int quantity = Convert.ToInt32(receive_dgv["Qty", i].Value);
                    int orderedQuantity = Convert.ToInt32(receive_dgv["OrderedQty", i].Value);
                    errorMessagePartial = errorMessagePartial + "'" + receive_dgv["Description", i].Value.ToString() + "[" +
                                              quantity.ToString(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT) + "/" + orderedQuantity.ToString(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT) + "]' ,";

                }
            }
            bool hasReceiveError = false;
            bool hasQtyError = false;
            if (!string.IsNullOrEmpty(errorMessagePartial) && errorMessagePartial.Length > 1)
            {
                hasQtyError = true;
                errorMessagePartial = errorMessagePartial.Substring(0, errorMessagePartial.Length - 1);
                errorMessagePartial = MessageContainerList.GetMessage(executionContext, 2270, errorMessagePartial);//2270
            }
            if (!string.IsNullOrEmpty(errorMessageNotReceived) && errorMessageNotReceived.Length > 1)
            {
                hasReceiveError = true;
                errorMessageNotReceived = errorMessageNotReceived.Substring(0, errorMessageNotReceived.Length - 1);
                errorMessageNotReceived = MessageContainerList.GetMessage(executionContext, 2268, errorMessageNotReceived);
            }
            if (hasQtyError || hasReceiveError)
            {
                string errorMsg = string.Empty;
                if (hasReceiveError)
                {
                    errorMsg = errorMessageNotReceived + Environment.NewLine;
                }
                if (hasQtyError)
                {
                    errorMsg = errorMsg + errorMessagePartial + Environment.NewLine;
                }
                errorMsg = errorMsg + MessageContainerList.GetMessage(executionContext, 2269);
                throw new ValidationException(errorMsg);
            }
            log.LogMethodExit();
        }

        private void CancelCurrentPO(FormClosingEventArgs e)
        {
            log.LogMethodEntry(e);
            int poLinesToBeRecieved = GetNumberOfPOLinesToBeReceived();
            if (poLinesToBeRecieved > 0 || tb_status.Text == "Open")
            {
                if (MessageBox.Show(this, MessageContainerList.GetMessage(executionContext, 2267), "Auto PO", MessageBoxButtons.YesNo) == DialogResult.No) //2267-Auto PO will be marked as cancelled. Do you want to proceed?
                {
                    log.LogMethodExit();
                    e.Cancel = true;
                    return;
                }
                UpdateAutoPOStatus();
            }
            log.LogMethodExit();
        }

        private void UpdateAutoPOStatus(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(purchaseOrderId);
            SqlConnection TrxCnn = null;
            SqlTransaction sqlTrx = null;
            if (sqlTransaction == null)
            {
                TrxCnn = Semnox.Parafait.Inventory.CommonFuncs.Utilities.createConnection();
                sqlTrx = TrxCnn.BeginTransaction();
            }
            else
            {
                sqlTrx = sqlTransaction;
            }
            try
            {
                String POnumber = tb_order.Text;
                bool isPartiallyReceived = false;
                PurchaseOrderList purchaseOrderList = new PurchaseOrderList(executionContext);
                List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                {
                    new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, purchaseOrderId.ToString())
                };
                List<PurchaseOrderDTO> purchaseOrderListDTO = purchaseOrderList.GetAllPurchaseOrder(searchParams, true, sqlTrx);
                bool isPOReceived = false;
                if (purchaseOrderListDTO != null && purchaseOrderListDTO.Any())
                {
                    isPOReceived = purchaseOrderListDTO.Exists(x => x.InventoryReceiptListDTO != null && x.InventoryReceiptListDTO.Any());
                }
                if (isPOReceived)
                {
                    isPartiallyReceived = IsAutoPOReceivedPartially(purchaseOrderListDTO);
                }
                purchaseOrderDTO = CancelOrShortAutoPO(isPOReceived, sqlTrx);
                CreateAmendmentCopy(isPOReceived, isPartiallyReceived, purchaseOrderDTO, sqlTrx);
                if (sqlTransaction == null)
                {
                    sqlTrx.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (sqlTransaction == null)
                {
                    sqlTrx.Rollback();
                }
                throw;
            }
            log.LogMethodExit();
        }

        private bool IsAutoPOReceivedPartially(List<PurchaseOrderDTO> purchaseOrderListDTO)
        {
            log.LogMethodEntry(purchaseOrderListDTO);
            bool isPartiallyReceive = false;
            double OrderQty = 0;
            double receivedQty = 0;
            if (purchaseOrderListDTO[0].PurchaseOrderLineListDTO != null && purchaseOrderListDTO[0].PurchaseOrderLineListDTO.Any())
            {
                OrderQty = purchaseOrderListDTO[0].PurchaseOrderLineListDTO.Where(pl => pl.CancelledDate == DateTime.MinValue).Sum(pl => pl.Quantity);
            }
            if (purchaseOrderListDTO[0].InventoryReceiptListDTO != null && purchaseOrderListDTO[0].InventoryReceiptListDTO.Any())
            {
                receivedQty = purchaseOrderListDTO[0].InventoryReceiptListDTO.FindAll(receipt => receipt.InventoryReceiveLinesListDTO != null && receipt.InventoryReceiveLinesListDTO.Any())
                                      .Sum(receiptQty => receiptQty.InventoryReceiveLinesListDTO.Sum(receiptLine => receiptLine.Quantity));
            }
            if (OrderQty != receivedQty)
            {
                isPartiallyReceive = true;
            }
            log.LogMethodExit(isPartiallyReceive);
            return isPartiallyReceive;
        }

        private PurchaseOrderDTO CancelOrShortAutoPO(bool isPOReceived, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(isPOReceived, sqlTransaction);
            string orderStatus = string.Empty;
            if (!isPOReceived)
            {
                orderStatus = PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED;
            }
            else
            {
                orderStatus = PurchaseOrderDTO.PurchaseOrderStatus.SHORTCLOSE;
            }
            purchaseOrderDTO = new PurchaseOrderDTO();
            PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext);
            purchaseOrderDTO = purchaseOrder.getPurchaseOrderDTO;
            purchaseOrderDTO.AmendmentNumber = (purchaseOrderDTO.AmendmentNumber == -1 ? 1 : (purchaseOrderDTO.AmendmentNumber + 1));
            purchaseOrderDTO.OrderStatus = orderStatus;
            purchaseOrderDTO.DocumentStatus = "";
            purchaseOrderDTO.ReprintCount = -1;
            // purchaseOrderDTO.OrderNumber = POnumber;
            if (purchaseOrderDTO.OrderStatus == PurchaseOrderDTO.PurchaseOrderStatus.CANCELLED)
            {
                purchaseOrderDTO.CancelledDate = ServerDateTime.Now;
            }
            else
            {
                purchaseOrderDTO.CancelledDate = DateTime.MinValue;
            }
            purchaseOrderDTO.DocumentStatus = "";
            purchaseOrderDTO.ReprintCount = -1;
            purchaseOrder.Save(sqlTransaction);
            log.LogMethodExit(purchaseOrder.getPurchaseOrderDTO);
            return purchaseOrder.getPurchaseOrderDTO;
        }

        void CreateAmendmentCopy(bool isPOReceived, bool isPartiallyReceive, PurchaseOrderDTO purchaseOrderDTO, SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(isPOReceived, isPartiallyReceive, purchaseOrderDTO, sqlTrx);
            try
            {
                purchaseOrderDTO.PurchaseOrderId = -1;
                if (!isPOReceived)
                {
                    purchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.OPEN;
                }
                else if (isPartiallyReceive)
                {
                    purchaseOrderDTO.OrderStatus = PurchaseOrderDTO.PurchaseOrderStatus.INPROGRESS;
                    purchaseOrderDTO.ReceivedDate = ServerDateTime.Now;
                }
                purchaseOrderDTO.DocumentStatus = "F";//DocumentStatus;
                purchaseOrderDTO.AmendmentNumber = 0;
                PurchaseOrder purchaseOrderNew = new PurchaseOrder(purchaseOrderDTO, executionContext);
                purchaseOrderNew.Save(sqlTrx);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        private bool Validate()
        {
            log.LogMethodEntry();
            if (cmbVendor.Text == "")
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(850), utilities.MessageUtils.getMessage("Complete"));
                cmbVendor.Focus();
                log.LogMethodExit();
                return false;
            }
            if (DT_receive.Rows.Count < 1)//&& !isReceiveAutoPO)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(852), utilities.MessageUtils.getMessage("Complete"));
                log.LogMethodExit();
                return false;
            }
            if (string.IsNullOrEmpty(txtVendorBillNo.Text))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(851), utilities.MessageUtils.getMessage("Complete"));
                txtVendorBillNo.Focus();
                log.LogMethodExit();
                return false;
            }
            log.LogMethodExit();
            return true;
        }

        private bool IsAutoPOReceivedPartially(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            bool isPartiallyReceived = false;
            PurchaseOrderList purchaseOrderList = new PurchaseOrderList(executionContext);
            List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> searchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>
                {
                    new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.PURCHASEORDERID, purchaseOrderId.ToString())
                };
            List<PurchaseOrderDTO> purchaseOrderListDTO = purchaseOrderList.GetAllPurchaseOrder(searchParams, true, sqlTransaction);

            if (purchaseOrderListDTO != null && purchaseOrderListDTO.Any())
            {
                isPartiallyReceived = purchaseOrderListDTO.FindAll(x => x.InventoryReceiptListDTO != null && x.InventoryReceiptListDTO.Any())
                                         .Exists(line => line.InventoryReceiptListDTO.FindAll(y => y.InventoryReceiveLinesListDTO != null && y.InventoryReceiveLinesListDTO.Any())
                                         .Exists(xy => xy.InventoryReceiveLinesListDTO.Any(xyz => xyz.POQuantity != xyz.Quantity)));
            }
            log.LogMethodExit(isPartiallyReceived);
            return isPartiallyReceived;
        }

        private int GetNumberOfPOLinesToBeReceived()
        {
            log.LogMethodEntry();
            int poLinesToBeRecieved = 0;
            for (int i = 0; i < receive_dgv.RowCount; i++)
            {
                if (receive_dgv["IsReceived", i].ValueType.FullName != "System.Drawing.Image")
                {
                    poLinesToBeRecieved += 1;
                }
            }
            log.LogMethodExit(poLinesToBeRecieved);
            return poLinesToBeRecieved;
        }

        private void receive_dgv_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                if (receive_dgv.RowCount > 0 && receive_dgv.DataSource != null)
                {
                    for (int i = 0; i < receive_dgv.RowCount; i++)
                    {
                        if (receive_dgv.Rows[i].Cells["cmbUOM"].Value == null)
                        {
                            int uomId = -1;
                            if (ProductContainer.productDTOList != null)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv.Rows[i].Cells["productId"].Value)).InventoryUOMId;
                                if (uomId == -1)
                                {
                                    uomId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(receive_dgv.Rows[i].Cells["productId"].Value)).UomId;
                                }
                            }
                            CommonFuncs.GetUOMComboboxForSelectedRows(receive_dgv, i, uomId);
                            receive_dgv.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
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
    }
}