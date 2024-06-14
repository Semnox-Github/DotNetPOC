/********************************************************************************************
 * Project Name - eZee Inventory
 * Description  - frmReceipts in PurchaseOrder
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By     Remarks          
 *********************************************************************************************  
 *2.60        11-Apr-2019      Girish Kundar   Modified : Replced Purchase Tax 3 tier with Tax 3 tier
 *2.70.2      20-Aug-2019      Archana         Form is moved to Inventory project
 *2.70.2      20-Dec-2019      Girish Kundar   Modified : populateLocation() method Used 3 tier for loading location details
 * 2.80       18-May-2020      Laster Menezes  Exception handling while printing InventoryReceiveReceipt
 *2.100.0     07-Aug-2020      Deeksha         Added UOM field.
 *2.120       03-May-2021      Laster Menezes  Modified GetInventoryReport method to use receipt framework of reports for InventoryReceiveReceipt generation
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Inventory;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;
using System.Drawing.Printing;

namespace Semnox.Parafait.Inventory
{
    public partial class frmOrderReceipts : Form
    {
        int orderId;
        int receiptId;
        int inventoryReceiptId;
        Utilities utilities;
        DataGridViewCellStyle inventorycostCellStyle;

        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmOrderReceipts(int pOrderId)
        {
            InitializeComponent();
            

            if (Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.LoginID);
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setLanguage(this);
            inventorycostCellStyle = new DataGridViewCellStyle();
            inventorycostCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            inventorycostCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.NUMBER_FORMAT;

            utilities = new Utilities();
            orderId = pOrderId;
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 190, Screen.PrimaryScreen.WorkingArea.Height - 120);
            populateReceipts();
        }

        void populateReceipts()
        {
            log.LogMethodEntry();
            try
            {
                InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler();
                List<InventoryReceiptDTO> inventoryReceiptListDTO = new List<InventoryReceiptDTO>();
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> receiptDTOSearchParams;
                receiptDTOSearchParams = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>();
                receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                if (orderId != -1)
                {
                    receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_ID, orderId.ToString()));
                    grpSearch.Enabled = false;
                }
                if (grpSearch.Enabled)
                {
                    if (txtVendorName.Text.Trim() != "")
                        receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDORNAME, txtVendorName.Text.Trim()));
                    if (txtVendorBillNumber.Text.Trim() != "")
                        receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.VENDOR_BILL_NUMBER, txtVendorBillNumber.Text.Trim()));
                    if (txtPONumber.Text.Trim() != "")
                        receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.ORDERNUMBER, txtPONumber.Text.Trim()));
                    if (txtGRN.Text.Trim() != "")
                        receiptDTOSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.GRN, txtGRN.Text.Trim()));
                }

                inventoryReceiptListDTO = inventoryReceiptDataHandler.GetInventoryReceiptList(receiptDTOSearchParams);

                SortableBindingList<InventoryReceiptDTO> sortReceiptsDTOList = new SortableBindingList<InventoryReceiptDTO>();
                if (inventoryReceiptListDTO == null)
                {
                    sortReceiptsDTOList = new SortableBindingList<InventoryReceiptDTO>();
                }
                else
                {
                    sortReceiptsDTOList = new SortableBindingList<InventoryReceiptDTO>(inventoryReceiptListDTO);
                }
                BindingSource receiptDTOListBS = new BindingSource();
                receiptDTOListBS.DataSource = sortReceiptsDTOList;
                dgvReceipt.DataSource = null;
                dgvReceiptLines.DataSource = null;
                dgvReceipt.DataSource = receiptDTOListBS;
                if(receiptDTOListBS != null && receiptDTOListBS.Count > 0)
                {
                    dgvReceipt.Rows[0].Selected = true;
                }
                dgvReceipt.Refresh();
            }
            catch { }
            log.LogMethodExit();
        }

        void populateReceiptLines(int ReceiptId)
        {
            log.LogMethodEntry(ReceiptId);
            receiptId = ReceiptId;
            InventoryReceiveLinesDataHandler inventoryReceiveLinesDataHandler = new InventoryReceiveLinesDataHandler();
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
            List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> receiptLinesDTOSearchParams;
            receiptLinesDTOSearchParams = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
            if (ReceiptId != -1)
            {
                receiptLinesDTOSearchParams.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, ReceiptId.ToString()));
            }

            inventoryReceiveLinesListDTO = inventoryReceiveLinesDataHandler.GetInventoryReceiveLinesList(receiptLinesDTOSearchParams);

            SortableBindingList<InventoryReceiveLinesDTO> sortReceiptLinesDTOList = new SortableBindingList<InventoryReceiveLinesDTO>();
            if (inventoryReceiveLinesListDTO == null)
            {
                sortReceiptLinesDTOList = new SortableBindingList<InventoryReceiveLinesDTO>();
            }
            else
            {
                sortReceiptLinesDTOList = new SortableBindingList<InventoryReceiveLinesDTO>(inventoryReceiveLinesListDTO);
            }
            BindingSource receiptLinesDTOListBS = new BindingSource();
            receiptLinesDTOListBS.DataSource = sortReceiptLinesDTOList;
            populateTax();
            populateLocation();
            dgvReceiptLines.DataSource = receiptLinesDTOListBS;
            dgvReceiptLines.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewNumericCellStyle();
            dgvReceiptLines.Columns["Price"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle(); 
            dgvReceiptLines.Columns["UnitPrice"].DefaultCellStyle = inventorycostCellStyle;
            dgvReceiptLines.Columns["Amount"].DefaultCellStyle = inventorycostCellStyle;
            dgvReceiptLines.Columns["TaxAmount"].DefaultCellStyle = inventorycostCellStyle;
            dgvReceiptLines.Columns["TaxPercentage"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            dgvReceiptLines.Columns["SubTotal"].DefaultCellStyle = inventorycostCellStyle;
            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();
            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT;
            dgvReceiptLines.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            //dgvReceiptLines.Columns["Price"].DefaultCellStyle.Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT;
            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvReceiptLines.Columns["TaxAmount"].DefaultCellStyle = new DataGridViewCellStyle {  Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };
            dgvReceiptLines.Columns["TaxPercentage"].DefaultCellStyle = new DataGridViewCellStyle {Format = Semnox.Parafait.Inventory.CommonFuncs.ParafaitEnv.INVENTORY_COST_FORMAT };
            dgvReceiptLines.Refresh();
            for ( int i = 0; i < sortReceiptLinesDTOList.Count; i ++)
            {
                CommonFuncs.GetUOMComboboxForSelectedRows(dgvReceiptLines, i, sortReceiptLinesDTOList[i].UOMId);
            }
            log.LogMethodExit();
        }

        void populateTax()
        {
            log.LogMethodEntry();
            TaxList taxList = new TaxList(machineUserContext);
            List<TaxDTO> taxDTOList = new List<TaxDTO>();
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> taxDTOSearchParams;
            taxDTOSearchParams = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            taxDTOSearchParams.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID , machineUserContext.GetSiteId().ToString()));
            BindingSource taxBS = new BindingSource();
            taxDTOList = taxList.GetAllTaxes(taxDTOSearchParams);
            if (taxDTOList == null)
            {
                taxDTOList = new List<TaxDTO>();
            }
            taxDTOList.Insert(0, new TaxDTO()); 
            BindingSource bindingSource = new System.Windows.Forms.BindingSource();
            bindingSource.DataSource = taxDTOList;
            TaxId.DataSource = bindingSource;
            TaxId.ValueMember = "TaxId";
            TaxId.DisplayMember = "TaxName";
            log.LogMethodExit();
        }

        void populateLocation()
        {
            log.LogMethodEntry();
            List<LocationDTO> locationListDTOList = new List<LocationDTO>();
            LocationList locationList = new LocationList(machineUserContext);
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationDTOSearchParams;
            locationDTOSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            locationListDTOList = locationList.GetAllLocations(locationDTOSearchParams);
            if (locationListDTOList == null)
            {
                locationListDTOList = new List<LocationDTO>();
            }
            locationListDTOList.Insert(0, new LocationDTO());
            locationListDTOList[0].Name = "";
            locationListDTOList[0].LocationId = -1;
            BindingSource bindingSource = new System.Windows.Forms.BindingSource();

            bindingSource.DataSource = locationListDTOList;
            locationIdDataGridViewTextBoxColumn.DataSource = bindingSource;
            locationIdDataGridViewTextBoxColumn.ValueMember = "LocationId";
            locationIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            log.LogMethodExit();
        }

        private void frm_receipts_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            populateReceipts();

            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref dgvReceipt);
            Semnox.Parafait.Inventory.CommonFuncs.Utilities.setupDataGridProperties(ref dgvReceiptLines);

            dgvReceipt.Columns["receiveDate"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewDateCellStyle();
            dgvReceipt.Columns["OrderDate"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewDateCellStyle();
            dgvReceipt.Columns["ReceiptAmount"].DefaultCellStyle = Semnox.Parafait.Inventory.CommonFuncs.Utilities.gridViewAmountCellStyle();

            dgvReceipt.BackgroundColor = dgvReceiptLines.BackgroundColor = this.BackColor;
            dgvReceipt.BorderStyle = BorderStyle.FixedSingle;
            dgvReceipt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            InventoryReceiptsBL inventoryReceipts;
            try
            {
                BindingSource receiptsBS = (BindingSource)dgvReceipt.DataSource;
                var receiptListOnDisplay = (SortableBindingList<InventoryReceiptDTO>)receiptsBS.DataSource;

                foreach (InventoryReceiptDTO d in receiptListOnDisplay)
                {
                    if (d.IsChanged)
                    {
                        inventoryReceipts = new InventoryReceiptsBL(d, machineUserContext);
                        inventoryReceipts.Save(null);
                    }
                }
                btnRefresh.PerformClick();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            populateReceipts();
        }

        private void dgvReceipt_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.Row.Cells["receiptIdDataGridViewTextBoxColumn"].Value != null)
                    populateReceiptLines(Convert.ToInt32(e.Row.Cells["receiptIdDataGridViewTextBoxColumn"].Value));
            }
          
            catch
            { }
            log.LogMethodExit();
        }

        private void dgvReceipt_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int index;
                try
                {
                    index = dgvReceipt.SelectedRows[0].Index;
                }
                catch
                {
                    return;
                }
                if (index < 0) //Header clicked
                    return;
                if (dgvReceipt["receiptIdDataGridViewTextBoxColumn", index].Value != null)
                    populateReceiptLines(Convert.ToInt32(dgvReceipt["receiptIdDataGridViewTextBoxColumn", index].Value));
            }
            
            catch
            { }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgvReceipts_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Hand;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Underline);
                }
                catch { }
            }
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "LotDetails")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Hand;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Underline);
                }
                catch { }
            }
            log.LogMethodExit();

        }

        private void dgvReceipts_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Default;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Regular);
                }
                catch { }
            }
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "LotDetails")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Default;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Regular);
                }
                catch { }
            }
            log.LogMethodExit();
        }

        private void dgvReceipts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    using (frmProductActivity f = new frmProductActivity(dgvReceiptLines["ProductId", e.RowIndex].Value, dgvReceiptLines["locationIdDataGridViewTextBoxColumn", e.RowIndex].Value, utilities))
                    {
                        f.Text = "Activities for Product: " + dgvReceiptLines["productIdDataGridViewTextBoxColumn", e.RowIndex].FormattedValue.ToString() +
                                " and Location: " + dgvReceiptLines["locationIdDataGridViewTextBoxColumn", e.RowIndex].FormattedValue.ToString();
                        Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(f);//Added for GUI Design style on 23-Aug-2016
                        f.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center on 23-Aug-2016
                        f.ShowDialog();
                    }
                }
                catch { }
            }
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "LotDetails")
            {
                int receiptLineId = Convert.ToInt32(dgvReceiptLines["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn", e.RowIndex].Value);
                int productID = Convert.ToInt32(dgvReceiptLines["ProductId", e.RowIndex].Value);
                ProductList productList = new ProductList();
                ProductDTO productDTO = productList.GetProduct(productID);
                if (productDTO.LotControlled)
                {
                    InventoryLotList inventoryLotList = new InventoryLotList(machineUserContext);
                    List<InventoryLotDTO> lotsList = inventoryLotList.GetInventoryLotListByReceiveLineID(receiptLineId);
                    InventoryLotUI inventoryLotUI = new InventoryLotUI(Semnox.Parafait.Inventory.CommonFuncs.Utilities, lotsList, productDTO.ExpiryType, false, false, "");
                    inventoryLotUI.ShowDialog();
                    inventoryLotUI.Dispose();
                }
            }
            log.LogMethodExit();
        }        

        private void txtPONumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnRefresh.PerformClick();
        }

        private void txtVendorBillNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnRefresh.PerformClick();
        }

        private void txtVendorName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnRefresh.PerformClick();
        }

        private void txtGRN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnRefresh.PerformClick();
        }

        //Begin: Modification done for removing side Add Products button on Form Close on 01-Sep-2016
        private void frm_receipts_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (orderId == -1)//execute only if receipt in from ReceiveInventory Form
            {
                // ((ParentMDI)MdiParent).RemoveControl("Receipts", false);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                inventoryReceiptId = Convert.ToInt32(dgvReceipt.CurrentRow.Cells[0].Value);
                if (inventoryReceiptId != -1)
                {
                    string reportFileName = GetInventoryReport();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1819, ex.Message));//Error while printing the document. Error message: &1
            }

            log.LogMethodExit();
        }

        private string GetInventoryReport()
        {
            log.LogMethodEntry();
            List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
            reportParam.Add(new clsReportParameters.SelectedParameterValue("ReceiptId", inventoryReceiptId));
            ReceiptReports receiptReports = new ReceiptReports(machineUserContext, "InventoryReceiveReceipt", "", null, null, reportParam, "P");
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
    }
        //End: Modification done for removing side Add Products button on Form Close on 01-Sep-2016
    
}
