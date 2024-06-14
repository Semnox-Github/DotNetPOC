
/********************************************************************************************
 * Project Name - Vendor Return UI
 * Description  -UI of Vendor Return
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        26-Aug-2016   Suneetha.S          Created 
 *2.70.2      15-Jul-2019   Girish Kundar       Modified : Added LogMethdEntry() and LogMethodExit()
*2.100.0      14-Aug-2020   Deeksha             Modified :Added UOM drop down field to change Related UOM's
*2.120.0      24-Feb-2021   Mushahid Faizan     Handled execution Context.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// frmVendorReturns 
    /// </summary>
    public partial class frmVendorReturns : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = new Utilities();
        private int documentTypeId = 0;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        /// <summary>
        /// default constructor
        /// </summary>
        public frmVendorReturns(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            InitializeComponent();
            utilities = _utilities;
            utilities.setupDataGridProperties(ref dgvReceipt);
            utilities.setupDataGridProperties(ref dgvReceiptLines);
            log.LogMethodExit();
        }


        /// <summary>
        /// form load
        /// </summary>
        private void frmVendorReturns_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            dgvReceipt.Columns["receiveDate"].DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvReceipt.Columns["PODate"].DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvReceipt.Columns["ReceiptAmount"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            ProductContainer productContainer = new ProductContainer(machineUserContext);

            dgvReceiptLines.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvReceiptLines.Columns["Price"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
            dgvReceiptLines.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            dgvReceiptLines.Columns["Price"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;

            dgvReceiptLines.Columns["CurrentStock"].DefaultCellStyle.ForeColor = Color.Blue;

            dgvReceipt.BackgroundColor = dgvReceiptLines.BackgroundColor = this.BackColor;
            dgvReceipt.BorderStyle = BorderStyle.FixedSingle;
            dgvReceipt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            LoadReceipts();
            documentTypeId = GetDocumentType();
            log.LogMethodExit();
        }

        /// <summary>
        /// populate Receipts into grid on load
        /// </summary>
        private void LoadReceipts()
        {
            log.LogMethodEntry();
            try
            {
                InventoryReceiptList inventoryReceiptsList = new InventoryReceiptList(machineUserContext);
                List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> inventoryReceiptsSearchParams = new List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>>();
                inventoryReceiptsSearchParams.Add(new KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>(InventoryReceiptDTO.SearchByInventoryReceiptParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<InventoryReceiptDTO> inventoryReceiptsListOnDisplay = inventoryReceiptsList.GetAllInventoryReceipts(inventoryReceiptsSearchParams);
                BindingSource inventoryReceiptsListBS = null;
                if (inventoryReceiptsListOnDisplay != null && inventoryReceiptsListOnDisplay.Count > 0)
                {
                    inventoryReceiptsListBS = new BindingSource();
                    SortableBindingList<InventoryReceiptDTO> inventoryReceiptsDTOSortList = new SortableBindingList<InventoryReceiptDTO>(inventoryReceiptsListOnDisplay);
                    inventoryReceiptsListBS.DataSource = inventoryReceiptsDTOSortList;
                }
                else
                    inventoryReceiptsListBS.DataSource = new SortableBindingList<InventoryReceiptDTO>();

                dgvReceipt.DataSource = inventoryReceiptsListBS;

                inventoryReceiptsListBS.Filter = string.Format("receiptIdDataGridViewTextBoxColumn = '{0}'", "0");
                dgvReceipt.ResetBindings();
                dgvReceipt.Refresh();
                dgvReceipt.Update();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in LoadReceipts() event." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate Receipts into grid on load
        /// </summary>
        private void LoadReceiptsLines(int receiptId)
        {
            log.LogMethodEntry(receiptId);
            try
            {
                InventoryReceiptLineList inventoryReceiptLinesList = new InventoryReceiptLineList(machineUserContext);
                //List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> inventoryReceiptLinesSearchParams = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
                //inventoryReceiptLinesSearchParams.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, Convert.ToString(receiptId)));
                List<InventoryReceiveLinesDTO> inventoryReceiptLineListOnDisplay = inventoryReceiptLinesList.GetNonMarketInventoryReceiveLines(receiptId);
                BindingSource inventoryReceiptLineListBS = new BindingSource();
                if (inventoryReceiptLineListOnDisplay != null && inventoryReceiptLineListOnDisplay.Count > 0)
                {
                    SortableBindingList<InventoryReceiveLinesDTO> inventoryReceiptsDTOSortList = new SortableBindingList<InventoryReceiveLinesDTO>(inventoryReceiptLineListOnDisplay);
                    inventoryReceiptLineListBS.DataSource = inventoryReceiptsDTOSortList;
                }
                else
                    inventoryReceiptLineListBS.DataSource = new SortableBindingList<InventoryReceiveLinesDTO>();

                dgvReceiptLines.DataSource = inventoryReceiptLineListBS;
                //Modified on - 03-Jul-2017 fix for displaying qty on referesh click 
                foreach (DataGridViewRow row in dgvReceiptLines.Rows)
                {
                    if (row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value != null)
                    {
                        try
                        {
                            row.Cells["VendorReturnedQuantity"].Value = Convert.ToDouble(GetReturnedQuantity(row.Cells["LocationId"].Value == null ? 0 : Convert.ToInt32(row.Cells["LocationId"].Value), Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value), Convert.ToInt32(row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value)));
                            row.Cells["ReturnQuantity"].Value = 0;
                        }
                        catch (Exception ex)
                        {
                            log.LogMethodExit(ex);
                            log.LogMethodExit("Exception Occurred");
                        }
                    }
                    LoadUOMComboBox();
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in LoadReceiptsLines() event." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method which loads related UOM to the each row in the dgv
        /// </summary>
        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                int rowIndex = dgvReceiptLines.CurrentRow.Index;
                BindingSource bindingSource = (BindingSource)dgvReceiptLines.DataSource;
                var recieveLinesDTOList = (SortableBindingList<InventoryReceiveLinesDTO>)bindingSource.DataSource;
                if (dgvReceiptLines.Rows[rowIndex].Cells["cmbUOM"].Value == null
                    && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                {
                    for (int i = 0; i < recieveLinesDTOList.Count; i++)
                    {
                        int uomId = -1;
                        if (recieveLinesDTOList[i].UOMId != -1)
                        {
                            uomId = recieveLinesDTOList[i].UOMId;
                        }
                        else
                        {
                            uomId = ProductContainer.productDTOList.Find(x => x.ProductId == recieveLinesDTOList[i].ProductId).InventoryUOMId;
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvReceiptLines, i, uomId);
                        dgvReceiptLines.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        /// <summary>
        /// populate Receipts into grid on load
        /// </summary>
        private void dgvReceipt_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (e.Row.Index < 0)
                    return;
                if (e.Row.Cells["PONumber"].Value == DBNull.Value || e.Row.Cells["PONumber"].Value == null)
                {
                    VendorReturnList returnList = new VendorReturnList();
                    VendorReturnsDTO vrReceiptData = returnList.GetReceiptsData(Convert.ToInt32(e.Row.Cells["purchaseOrderIdDataGridViewTextBoxColumn"].Value), Convert.ToInt32(e.Row.Cells["receiptIdDataGridViewTextBoxColumn"].Value));
                    if (vrReceiptData != null)
                    {
                        e.Row.Cells["PONumber"].Value = vrReceiptData.OrderNumber;
                        e.Row.Cells["PODate"].Value = vrReceiptData.PODate;
                        e.Row.Cells["VendorName"].Value = vrReceiptData.VendorName;
                        e.Row.Cells["ReceiptAmount"].Value = vrReceiptData.ReceiptAmount;
                    }
                }
                LoadUOMComboBox();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in dgvReceipt_RowStateChanged() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// populate Receipts into grid on load
        /// </summary>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ClearFields();
            LoadReceipts();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// find receipts which matches search fields
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                VendorReturnList returnList = new VendorReturnList();
                List<InventoryReceiptDTO> receipts = null;
                DataTable getIds = returnList.GetReceiptIdsForFilter(txtVendorName.Text.Trim(), txtPONumber.Text.Trim(), txtVendorBillNumber.Text.Trim(), txtGRN.Text.Trim());
                //BindingSource source = (BindingSource)dgvReceipt.DataSource;
                //var listOnDisplay = (SortableBindingList<InventoryReceiptDTO>)source.DataSource;
                //if (source != null && source.Count > 0)
                //{
                if (getIds != null)
                {
                    InventoryReceiptList inventoryReceiptsList = new InventoryReceiptList(machineUserContext);

                    List<InventoryReceiptDTO> inventoryLocationFromDTOSortList = inventoryReceiptsList.GetAllInventoryReceipts(null);
                    if (inventoryLocationFromDTOSortList != null && inventoryLocationFromDTOSortList.Count > 0)
                    {
                        receipts = new List<InventoryReceiptDTO>();
                        foreach (DataRow row in getIds.Rows)
                        {
                            InventoryReceiptDTO receiptItem = inventoryLocationFromDTOSortList.Find(delegate (InventoryReceiptDTO r) { return r.ReceiptId.Equals(Convert.ToInt32(row["ReceiptId"])); });
                            if (receiptItem != null)
                            {
                                receipts.Add(receiptItem);
                            }
                        }
                    }
                    //List<InventoryReceiptDTO> inventoryLocationFromDTOSortList = new List<InventoryReceiptDTO>(listOnDisplay);
                    //if (inventoryLocationFromDTOSortList != null && inventoryLocationFromDTOSortList.Count > 0)
                    //{
                    //    receipts = new List<InventoryReceiptDTO>();
                    //    foreach (DataRow row in getIds.Rows)
                    //    {
                    //        InventoryReceiptDTO receiptItem = inventoryLocationFromDTOSortList.Find(delegate(InventoryReceiptDTO r) { return r.ReceiptId.Equals(Convert.ToInt32(row["ReceiptId"])); });
                    //        if (receiptItem != null)
                    //        {
                    //            receipts.Add(receiptItem);
                    //        }
                    //    }
                    //}
                }
                else
                {
                    BindingSource bindingSource = new BindingSource();
                    bindingSource.DataSource = new SortableBindingList<InventoryReceiptDTO>();
                    dgvReceipt.DataSource = bindingSource;
                    dgvReceipt.Rows.Clear();

                    BindingSource inventoryReceiptLineListBS = new BindingSource();
                    inventoryReceiptLineListBS.DataSource = new SortableBindingList<InventoryReceiveLinesDTO>();
                    dgvReceiptLines.DataSource = inventoryReceiptLineListBS;
                    dgvReceiptLines.Rows.Clear();
                }
                // }
                if (receipts != null)
                {
                    BindingSource bindingSource = new BindingSource();
                    SortableBindingList<InventoryReceiptDTO> inventoryReceiptsDTOSortList = new SortableBindingList<InventoryReceiptDTO>(receipts);
                    bindingSource.DataSource = inventoryReceiptsDTOSortList;
                    dgvReceipt.DataSource = bindingSource;
                }
                //else
                //{
                //    LoadReceipts();
                //}
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in btnSearch_Click() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// clear fields on refresh click
        /// </summary>
        private void ClearFields()
        {
            txtVendorName.Text = "";
            txtPONumber.Text = "";
            txtVendorBillNumber.Text = "";
            txtVendorName.Text = "";
            txtGRN.Text = ""; //Modified on 24-Oct-2016
        }

        /// <summary>
        /// Load receipt  lines on Receipt grid selection changed
        /// </summary>
        private void dgvReceipt_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReceipt != null && dgvReceipt.Rows.Count > 0)
                {
                    int selectedIndex;
                    try
                    {
                        selectedIndex = dgvReceipt.SelectedRows[0].Index;
                    }
                    catch (Exception ex)
                    {
                        log.LogMethodExit(ex);
                        log.LogMethodExit("Exception Occurred");
                        return;
                    }
                    if (selectedIndex < 0) //Header clicked
                        return;

                    //ClearFields();
                    int receiptId = Convert.ToInt32(dgvReceipt["receiptIdDataGridViewTextBoxColumn", selectedIndex].Value);
                    LoadReceiptsLines(receiptId);
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in dgvReceipt_SelectionChanged() event." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// To Get location name based on Location Id
        /// </summary>
        private string GetLocationName(int locationId)
        {
            log.LogMethodEntry(locationId);
            string locationName = string.Empty;
            try
            {
                LocationBL locationBL = new LocationBL(machineUserContext, locationId);
                LocationDTO locationDTO = locationBL.GetLocationDTO;
                if (locationDTO != null)
                {
                    locationName = locationDTO.Name.ToString();
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in GetLocationName() method." + ex.Message);
            }
            log.LogMethodExit(locationName);
            return locationName;
        }

        /// <summary>
        /// To Returned quantities count for each receipt line
        /// </summary>
        private double GetReturnedQuantity(int locationId, int productId, int receiveLineId)
        {
            log.LogMethodEntry(locationId, productId, receiveLineId);
            double totalReturnedQnty = 0;
            try
            {
                InventoryAdjustmentsList inventoryList = new InventoryAdjustmentsList(machineUserContext);
                double totalQntyReturned = inventoryList.GetTotalAdjustments(locationId == -1 ? -1 : Convert.ToInt32(locationId), Convert.ToInt32(productId), Convert.ToInt32(receiveLineId), documentTypeId, "VendorReturn");
                if (totalQntyReturned != 0)
                {
                    totalReturnedQnty = totalQntyReturned;
                }
                //int documentTypeId = GetDocumentType();
                //InventoryAdjustmentsList inventoryAdjList= new InventoryAdjustmentsList();
                //List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>> inventoryAdjustmentSearchParams = new List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>>();
                //List<InventoryLotDTO> inventoryLotList = GetInventoryLotList(receiveLineId);
                //if(inventoryLotList != null)
                //{
                //    for (int i = 0; i < inventoryLotList.Count; i++)
                //    {
                //        inventoryAdjustmentSearchParams.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.LOT_ID, Convert.ToString(inventoryLotList[i].LotId)));
                //        inventoryAdjustmentSearchParams.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.FROM_LOCATION_ID, Convert.ToString(locationId)));
                //        inventoryAdjustmentSearchParams.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.PRODUCT_ID, Convert.ToString(productId)));
                //        inventoryAdjustmentSearchParams.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.DOCUMENT_TYPE_ID, Convert.ToString(documentTypeId)));
                //        inventoryAdjustmentSearchParams.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.ADJUSTMENT_TYPE, Convert.ToString("VendorReturn")));
                //        List<InventoryAdjustmentsDTO> adjustmentDTOList = inventoryAdjList.GetAllInventoryAdjustments(inventoryAdjustmentSearchParams);
                //        if(adjustmentDTOList != null && adjustmentDTOList.Count > 0)
                //        {
                //            for(int j=0;j<adjustmentDTOList.Count; j++)
                //            {
                //                totalReturnedQnty += adjustmentDTOList[j].AdjustmentQuantity;
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in GetReturnedQuantity() method." + ex.Message);
            }
            log.LogMethodExit(totalReturnedQnty);
            return totalReturnedQnty;
        }

        /// <summary>
        /// enable and disable Return Quantity and Select lot button if Product is lot controlled on grid row state changed
        /// </summary>
        bool enableLotbtn = true;
        private void dgvReceiptLines_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //to display location name
                if (e.Row.Index < 0)
                {
                    log.LogMethodExit();
                    return;
                }
                if (e.Row.Cells["LocationId"].Value != DBNull.Value)
                {
                    string name = GetLocationName(Convert.ToInt32(e.Row.Cells["LocationId"].Value));
                    if (!string.IsNullOrEmpty(name))
                    {
                        e.Row.Cells["LocationName"].Value = name;
                    }
                }

                //to check receive line is lot controlled or not
                if (e.Row.Index < 0)
                {
                    log.LogMethodExit();
                    return;
                }

                if (e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    bool lotCottroled = false;
                    ProductList productList = new ProductList();
                    ProductDTO productDTO = productList.GetProduct(Convert.ToInt32(e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value));
                    if (productDTO != null)
                    {
                        lotCottroled = Convert.ToBoolean(productDTO.LotControlled);

                        if (!string.IsNullOrEmpty(productDTO.Code))
                        {
                            e.Row.Cells["Code"].Value = productDTO.Code.ToString();
                        }
                    }

                    InventoryList inventorylist = new InventoryList();

                    if (lotCottroled)
                    {
                        InventoryDTO inventoryDTO = inventorylist.GetInventoryOnLots(Convert.ToInt32(e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value), e.Row.Cells["LocationId"].Value == DBNull.Value ? -1 : Convert.ToInt32(e.Row.Cells["LocationId"].Value), "LotId is not null");
                        if (inventoryDTO != null) // check receipt exist in Inventory
                        {
                            if (inventoryDTO.LotId != -1)
                            {
                                dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = true;
                                enableLotbtn = true;
                            }
                            else //if LotId null for the Inventory product then cannot allow to return
                            {
                                dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = false;
                                enableLotbtn = false;
                            }
                        }
                        else //if receipt not exist in Inventory cannot allow to return 
                        {
                            dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = true;
                            enableLotbtn = false;
                        }
                    }
                    else
                    {
                        InventoryDTO inventoryDTO = inventorylist.GetInventoryOnLots(Convert.ToInt32(e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value), e.Row.Cells["LocationId"].Value == DBNull.Value ? -1 : Convert.ToInt32(e.Row.Cells["LocationId"].Value), "LotId is null");
                        if (inventoryDTO != null) // check receipt exist in Inventory
                        {
                            if (inventoryDTO.LotId != -1)
                            {
                                dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = true;
                                enableLotbtn = true;
                            }
                            else //if LotId null for the Inventory product then cannot allow to return
                            {
                                dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = false;
                                enableLotbtn = false;
                            }
                        }
                        else //if receipt not exist in Inventory cannot allow to return 
                        {
                            dgvReceiptLines.Columns["ReturnQuantity"].ReadOnly = true;
                            enableLotbtn = false;
                        }
                    }

                }

                //GetReturned quantity
                if (e.Row.Index < 0)
                {
                    log.LogMethodExit();
                    return;
                }
                if (e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    e.Row.Cells["VendorReturnedQuantity"].Value = Convert.ToDouble(GetReturnedQuantity(e.Row.Cells["LocationId"].Value == null ? 0 : Convert.ToInt32(e.Row.Cells["LocationId"].Value), Convert.ToInt32(e.Row.Cells["productIdDataGridViewTextBoxColumn"].Value), Convert.ToInt32(e.Row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value)));
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in dgvReceiptLines_RowStateChanged() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvReceiptLines_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Hand;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Underline);
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex);
                    log.LogMethodExit("Exception Occurred");
                }
            }
            log.LogMethodExit();
        }

        private void dgvReceiptLines_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    dgvReceiptLines.Cursor = Cursors.Default;
                    dgvReceiptLines[e.ColumnIndex, e.RowIndex].Style.Font = new Font(dgvReceiptLines.DefaultCellStyle.Font, FontStyle.Regular);
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex);
                    log.LogMethodExit("Exception Occurred");
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// open InventoryLOTUI on select log button click
        /// </summary>
        private void dgvReceiptLines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (enableLotbtn)
                {
                    if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "SelectLots" && e.RowIndex > -1)
                    {
                        int receiptLineId = Convert.ToInt32(dgvReceiptLines.CurrentRow.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value);
                        List<InventoryLotDTO> lotsList = GetInventoryLotList(receiptLineId);
                        //InventoryLotUI frmInventorylot = new InventoryLotUI(utilities, lotsList, "E", true, false, "VendorReturn");
                        //Start update 14-Feb-2017
                        ProductList productList = new ProductList();
                        ProductDTO productDTO = productList.GetProduct(Convert.ToInt32(dgvReceiptLines.CurrentRow.Cells["productIdDataGridViewTextBoxColumn"].Value));
                        InventoryLotUI frmInventorylot = new InventoryLotUI(utilities, lotsList, productDTO.ExpiryType, true, false, "VendorReturn");
                        //End update 14-Feb-2017
                        frmInventorylot.StartPosition = FormStartPosition.CenterScreen;
                        frmInventorylot.ShowDialog();

                        double totalQntyReturn = 0;
                        List<InventoryLotDTO> returnedLotDetails = frmInventorylot.inventoryLotListOnReturn;
                        dgvReceiptLines.Rows[e.RowIndex].Tag = returnedLotDetails;
                        frmInventorylot.Dispose();
                        if (returnedLotDetails != null && returnedLotDetails.Count > 0)
                        {
                            for (int i = 0; i < returnedLotDetails.Count; i++)
                            {
                                if (returnedLotDetails[i].IsChanged)
                                {
                                    if (returnedLotDetails[i].Quantity > 0)
                                    {
                                        totalQntyReturn += returnedLotDetails[i].Quantity;
                                    }
                                }
                            }
                        }
                        dgvReceiptLines.Rows[e.RowIndex].Cells["ReturnQuantity"].Value = totalQntyReturn;
                    }
                }
            }
            catch (Exception ex) { log.Error("Error in dgvReceiptLines_CellContentClick() method. " + ex.Message); }

            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "CurrentStock")
            {
                try
                {
                    //frmProductActivity f = new frmProductActivity(dgvReceiptLines["productIdDataGridViewTextBoxColumn", e.RowIndex].Value, dgvReceiptLines["locationIdDataGridViewTextBoxColumn", e.RowIndex].Value);
                    //f.Text = "Activities for Product: " + dgvReceiptLines["productIdDataGridViewTextBoxColumn", e.RowIndex].FormattedValue.ToString() +
                    //        " and Location: " + dgvReceiptLines["locationIdDataGridViewTextBoxColumn", e.RowIndex].FormattedValue.ToString();
                    //f.ShowDialog();
                }
                catch (Exception ex)
                {
                    log.LogMethodExit(ex);
                    log.LogMethodExit("Exception Occurred");
                }
            }
        }

        /// <summary>
        /// Update Balance quantity into lot table on return
        /// </summary>
        private void UpdateLotOnReturn(InventoryLotDTO inventoryRetObj, string expiryType)
        {
            log.LogMethodEntry(inventoryRetObj, expiryType);
            try
            {
                if (inventoryRetObj != null)
                {
                    if (inventoryRetObj.Quantity > 0)
                    {
                        if (inventoryRetObj.Quantity > inventoryRetObj.BalanceQuantity)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage("Cannot allow to return quantity more than balance quantity"));
                            log.LogMethodExit();
                            return;
                        }

                        if (inventoryRetObj.Expirydate.Date < ServerDateTime.Now.Date && !expiryType.Equals("N"))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage("Cannot allow to return quantity Expired items"));
                            log.LogMethodExit();
                            return;
                        }

                        inventoryRetObj.BalanceQuantity = Convert.ToDouble(inventoryRetObj.BalanceQuantity) - Convert.ToDouble(inventoryRetObj.Quantity);

                        InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryRetObj, machineUserContext);
                        inventoryLotBL.Save(null);
                        //inventoryLotBL.
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in UpdateLotOnReturn() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Insert adjustments records into Inventory Adjustment
        /// </summary>
        private void InsertIntoAdjustments(int locationId, int productId, double price, double quantity, int lotId, int receiptlineid, int uomId)
        {
            log.LogMethodEntry(locationId, productId, price, quantity, lotId, receiptlineid, uomId);
            try
            {
                InventoryAdjustmentsDTO adjustmentDTO;
                adjustmentDTO = new InventoryAdjustmentsDTO(-1,
                                                            "VendorReturn",
                                                            Convert.ToDouble(quantity) * -1,
                                                            locationId,
                                                            -1,
                                                            string.Empty,
                                                            productId,
                                                            ServerDateTime.Now,
                                                            string.Empty,
                                                            -1,
                                                            string.Empty,
                                                            false,
                                                            string.Empty,
                                                            -1,
                                                            -1,
                                                            lotId,
                                                            price,
                                                            documentTypeId,
                                                            false,
                                                            -1,
                                                            string.Empty,
                                                            ServerDateTime.Now,
                                                            string.Empty,
                                                            ServerDateTime.Now,
                                                            uomId,
                                                            receiptlineid
                                                            );
                if (adjustmentDTO != null)
                {
                    if (adjustmentDTO.AdjustmentQuantity != 0)
                    {
                        InventoryAdjustmentsBL inventoryAdjBL = new InventoryAdjustmentsBL(utilities, adjustmentDTO);
                        inventoryAdjBL.Save(null);
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in InsertIntoAdjustments() method. " + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Update Inventory Table stock on vendor return
        /// </summary>
        private void UpdateInventoryStock(int locationId, int lotId, int productId, double quantity, int uomId)
        {
            log.LogMethodEntry(locationId, lotId, productId, quantity);
            try
            {
                Inventory inventoryBL;
                InventoryList inventoryList = new InventoryList();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, Convert.ToString(locationId)));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, Convert.ToString(productId)));

                if (lotId > 0)
                {
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, Convert.ToString(lotId)));
                }
                List<InventoryDTO> inventoryListOnDisplay = inventoryList.GetAllInventory(inventorySearchParams, false, null);
                if (inventoryListOnDisplay != null)
                {
                    for (int i = 0; i < inventoryListOnDisplay.Count; i++)
                    {
                        if (Convert.ToInt32(inventoryListOnDisplay[i].ProductId) >= 0)
                        {
                            inventoryListOnDisplay[i].UOMId = uomId;
                            inventoryListOnDisplay[i].Quantity = Convert.ToDouble(inventoryListOnDisplay[i].Quantity) - quantity;
                            inventoryBL = new Inventory(inventoryListOnDisplay[i], machineUserContext);
                            inventoryBL.Save(null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in InsertIntoAdjustments() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get list of lots for specific receive line
        /// </summary>
        private List<InventoryLotDTO> GetInventoryLotList(int receiptLineId)
        {
            log.LogMethodEntry();
            try
            {
                InventoryLotList inventoryLotList = new InventoryLotList(machineUserContext);
                List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> inventoryLotLinesSearchParams = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>();
                inventoryLotLinesSearchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.PURCHASEORDER_RECEIVE_LINEID, Convert.ToString(receiptLineId)));
                List<InventoryLotDTO> inventoryLotsOnDisplay = inventoryLotList.GetAllInventoryLot(inventoryLotLinesSearchParams);
                if (inventoryLotsOnDisplay != null && inventoryLotsOnDisplay.Count > 0)
                {
                    log.LogMethodExit(inventoryLotsOnDisplay);
                    return inventoryLotsOnDisplay;
                }
                else
                {
                    log.LogMethodExit();
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in GetInventoryLotList() method." + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Button click event to save returned quantity
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvReceiptLines != null && dgvReceiptLines.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvReceiptLines.Rows)
                    {
                        int userEnteredUOMId = Convert.ToInt32(dgvReceiptLines.Rows[row.Index].Cells["cmbUOM"].Value);
                        int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value)).InventoryUOMId;
                        double factor = 1;
                        if (row.Cells["ReturnQuantity"].Value != null && (userEnteredUOMId != inventoryUOMId))
                        {
                            factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                            row.Cells["ReturnQuantity"].Value = Convert.ToDouble(row.Cells["ReturnQuantity"].Value) * factor;
                        }
                        if (Convert.ToDouble(row.Cells["ReturnQuantity"].Value) > 0)
                        {
                            if (Convert.ToDouble(row.Cells["quantityDataGridViewTextBoxColumn"].Value) < Convert.ToDouble(row.Cells["ReturnQuantity"].Value))
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1081, MessageContainerList.GetMessage(utilities.ExecutionContext, "return"), MessageContainerList.GetMessage(utilities.ExecutionContext, "received quantity")), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation Error")); //The product &1 quantity should be less than or equal to &2
                                log.LogMethodExit();
                                return;
                            }
                            if (Convert.ToDouble(row.Cells["quantityDataGridViewTextBoxColumn"].Value) < (Convert.ToDouble(row.Cells["VendorReturnedQuantity"].Value) * -1) + Convert.ToDouble(row.Cells["ReturnQuantity"].Value))
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 116, MessageContainerList.GetMessage(utilities.ExecutionContext, "return"), MessageContainerList.GetMessage(utilities.ExecutionContext, "return quantity")), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation Error")); //Not enough quantity available for &1. Please correct before saving
                                log.LogMethodExit();
                                return;
                            }
                            if (row.Tag != null)
                            {
                                //loop each lots for receipt line
                                List<InventoryLotDTO> selectedLotsList = (List<InventoryLotDTO>)row.Tag;
                                if (selectedLotsList != null && selectedLotsList.Count > 0)
                                {
                                    for (int i = 0; i < selectedLotsList.Count; i++)
                                    {
                                        //Update Inventory Lot
                                        ProductList productList = new ProductList();
                                        ProductDTO productDTO = productList.GetProduct(Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value));
                                        UpdateLotOnReturn(selectedLotsList[i], productDTO.ExpiryType);
                                        //Insert into Adjustment                                            
                                        InsertIntoAdjustments(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value),
                                            row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value),
                                            row.Cells["Price"].Value == null ? -1 : Convert.ToDouble(row.Cells["Price"].Value), Convert.ToDouble(selectedLotsList[i].Quantity), Convert.ToInt32(selectedLotsList[i].LotId),
                                            row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value),
                                            inventoryUOMId);

                                        //Update Stock at Inventory
                                        UpdateInventoryStock(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value),
                                            Convert.ToInt32(selectedLotsList[i].LotId), row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 :
                                            Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value), selectedLotsList[i].Quantity,
                                             inventoryUOMId);
                                    }
                                }
                                else
                                {
                                    //Insert into Adjustment                                            
                                    InsertIntoAdjustments(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value),
                                        row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value),
                                        row.Cells["Price"].Value == null ? -1 : Convert.ToDouble(row.Cells["Price"].Value), Convert.ToDouble(row.Cells["ReturnQuantity"].Value), -1,
                                        row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value),
                                        inventoryUOMId);

                                    //Update Stock at Inventory
                                    UpdateInventoryStock(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value), -1,
                                        row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value),
                                        Convert.ToDouble(row.Cells["ReturnQuantity"].Value),
                                         inventoryUOMId);
                                }
                            }
                            else
                            {
                                //Insert into Adjustment                                            
                                InsertIntoAdjustments(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value),
                                    row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value),
                                    row.Cells["Price"].Value == null ? -1 : Convert.ToDouble(row.Cells["Price"].Value), Convert.ToDouble(row.Cells["ReturnQuantity"].Value), -1,
                                    row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value),
                                    inventoryUOMId);

                                //Update Stock at Inventory
                                UpdateInventoryStock(row.Cells["LocationId"].Value == null ? -1 : Convert.ToInt32(row.Cells["LocationId"].Value), -1,
                                    row.Cells["productIdDataGridViewTextBoxColumn"].Value == null ? -1 : Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value),
                                    Convert.ToDouble(row.Cells["ReturnQuantity"].Value),
                                     inventoryUOMId);
                            }
                        }
                        try
                        {
                            row.Cells["VendorReturnedQuantity"].Value = Convert.ToDouble(GetReturnedQuantity(row.Cells["LocationId"].Value == null ? 0 : Convert.ToInt32(row.Cells["LocationId"].Value), Convert.ToInt32(row.Cells["productIdDataGridViewTextBoxColumn"].Value), Convert.ToInt32(row.Cells["purchaseOrderReceiveLineIdDataGridViewTextBoxColumn"].Value)));
                            row.Cells["ReturnQuantity"].Value = 0;
                        }
                        catch (Exception ex)
                        {
                            log.LogMethodExit(ex);
                            log.LogMethodExit("Exception Occurred");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                log.Error("Error in btnSave_Click() method." + ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loading Requisitions type
        /// </summary>
        private int GetDocumentType()
        {
            log.LogMethodEntry();
            int typeId = -1;
            try
            {

                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
                List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "VR"));
                List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
                if (inventoryDocumentTypeListOnDisplay != null)
                {
                    typeId = Convert.ToInt32(inventoryDocumentTypeListOnDisplay[0].DocumentTypeId);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in GetDocumentType() event." + ex.Message);
            }
            log.LogMethodExit(typeId);
            return typeId;
        }

        private void dgvReceiptLines_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvReceiptLines.Columns[e.ColumnIndex].Name == "ReturnQuantity")
            {
                double verifyDouble = 0;
                if (Double.TryParse(e.FormattedValue.ToString(), out verifyDouble) == false)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(648));
                    e.Cancel = true;
                }
            }
        }

        private void dgvReceiptLines_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        private void frmVendorReturns_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }
    }
}
