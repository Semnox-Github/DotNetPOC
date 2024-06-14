/********************************************************************************************
* Project Name -Inventory Lot UI
* Description  -User interface inventory Lot 
* 
**************
**Version Log
**************
*Version     Date          Modified By          Remarks          
*********************************************************************************************
*1.00        15-Aug-2016    Raghuveera          Created
*2.70.2      06-Aug-2019    Deeksha             Added log() methods.
*2.70.2      18-Dec-2019    Jinto Thomas        Added parameter execution context for userrolrbl declaration with userid 
*2.100.0     14-Aug-2020    Deeksha             Modified for Recipe Management Enhancement
********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logging;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// User interface for inventorylot
    /// </summary>
    public partial class InventoryLotUI : Form
    {
        Utilities utilities;
        double TotalQuantity = 0;
        double TotalReturnQuantity = 0;
        string expiryType = "N";
        string applicability = "";
        /// <summary>
        /// On press of Ok the grid content is available in the form of InventoryLotDTO List
        /// </summary>
        public List<InventoryLotDTO> inventoryLotListOnReturn = null;
        logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private List<EntityExclusionDetailDTO> entityExclusionDetailDTOList;
        private int poUOMId = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Utilities"></param>
        public InventoryLotUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            TotalQuantity = 0;
            utilities.setupDataGridProperties(ref dgvInventoryLot);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            inventoryLotListOnReturn = null; 
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = utilities;
            CommonUIDisplay.ParafaitEnv = utilities.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen;
            UserRoles userRoles = new UserRoles(machineUserContext, Semnox.Parafait.Inventory.CommonFuncs.Utilities.ParafaitEnv.RoleId);
            entityExclusionDetailDTOList = userRoles.GetUIFieldsToHide("InventoryLot");
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Utilities"></param> 
        /// <param name="lotId"></param>
        /// <param name="productId"></param>
        /// <param name="expiryType"></param>
        public InventoryLotUI(Utilities _Utilities, int lotId, int productId = -1, string expiryType = "N")
            : this(_Utilities)
        {
            log.LogMethodEntry(_Utilities, lotId, productId, expiryType);
            populateInventoryLot(lotId);
            this.expiryType = expiryType;
            if (expiryType.Equals("D") || expiryType.Equals("E"))
            {
                expiryInDaysDataGridViewTextBoxColumn.Visible = false;
                expirydateDataGridViewTextBoxColumn.Visible = true;
                expirydateDataGridViewTextBoxColumn.ReadOnly = true;
            }
            else
            {
                expiryInDaysDataGridViewTextBoxColumn.Visible = false;
                expirydateDataGridViewTextBoxColumn.Visible = false;
            }
            //Added update 23-Feb-2017
            selectExpiryDate.Visible = false; 
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = utilities;
            CommonUIDisplay.ParafaitEnv = utilities.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen; 
            log.LogMethodExit();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_Utilities"></param>
        /// <param name="inventoryLotList"></param>
        /// <param name="expiryType"></param>
        /// <param name="IsQuantityEditable"></param>
        /// <param name="IsExpiryEditable"></param>
        /// <param name="applicability"></param>
        public InventoryLotUI(Utilities _Utilities, List<InventoryLotDTO> inventoryLotList, string expiryType = "N", bool IsQuantityEditable = false, bool IsExpiryEditable = false, string applicability = "")
            : this(_Utilities)
        {
            log.LogMethodEntry(_Utilities, inventoryLotList, expiryType, IsQuantityEditable, IsExpiryEditable, applicability);
            this.expiryType = expiryType;
            this.applicability = applicability;
            if (expiryType.Equals("D") || expiryType.Equals("E"))
            {
                //    expiryInDaysDataGridViewTextBoxColumn.Visible = true;
                //    expirydateDataGridViewTextBoxColumn.Visible = false;
                //    expiryInDaysDataGridViewTextBoxColumn.ReadOnly = !IsExpiryEditable;
                //}
                //else if ()
                //{
                expiryInDaysDataGridViewTextBoxColumn.Visible = false;
                expirydateDataGridViewTextBoxColumn.Visible = true;
                //expirydateDataGridViewTextBoxColumn.ReadOnly = !IsExpiryEditable;
                expirydateDataGridViewTextBoxColumn.ReadOnly = true; //Updated 23-Feb-2017
            }
            else
            {
                expiryInDaysDataGridViewTextBoxColumn.Visible = false;
                expirydateDataGridViewTextBoxColumn.Visible = false;
                selectExpiryDate.Visible = false; //Added 23-Feb-2017
            }
            if (IsQuantityEditable)
            {

                if (applicability.Equals("VendorReturn"))
                {
                    originalQuantityDataGridViewTextBoxColumn.Visible = false;
                    selectExpiryDate.Visible = false; //Added 23-Feb-2017
                }
                else
                {
                    //Start update 23-Feb-2017
                    if (expiryType.Equals("E"))
                    {
                        selectExpiryDate.Visible = true;
                    }
                    else
                    {
                        selectExpiryDate.Visible = false;
                    }
                    //End update 23-Feb-2017
                    btnSplit.Visible = btnDelete.Visible = true;
                    balanceQuantityDataGridViewTextBoxColumn.Visible = false;
                    receivePriceDataGridViewTextBoxColumn.ReadOnly = false;
                }
                quantityDataGridViewTextBoxColumn.Visible = true;
                quantityDataGridViewTextBoxColumn.ReadOnly = false;
                //originalQuantityDataGridViewTextBoxColumn.ReadOnly = false;
            }
            else
            {
                btnSplit.Visible = btnDelete.Visible = false;
                balanceQuantityDataGridViewTextBoxColumn.Visible = true;
                quantityDataGridViewTextBoxColumn.Visible = false;
                quantityDataGridViewTextBoxColumn.ReadOnly = true;
                receivePriceDataGridViewTextBoxColumn.ReadOnly = true;
                selectExpiryDate.Visible = false; //24-Feb-2017
            }
            if (applicability.Equals("AdjustmentLotConvertion"))
            {
                receivePriceDataGridViewTextBoxColumn.ReadOnly = false;
            }

            foreach (InventoryLotDTO inventoryLotDTO in inventoryLotList)
            {
                if (!applicability.Equals("VendorReturn"))
                {
                    inventoryLotDTO.Quantity = inventoryLotDTO.BalanceQuantity;
                }
                TotalQuantity += inventoryLotDTO.BalanceQuantity;
                try
                {
                    poUOMId = UOMContainer.uomDTOList.Find(x => x.UOM == inventoryLotDTO.UOM).UOMId;
                }
                catch(Exception ex) { log.Error(ex); };
            }
            PopulateGrid(inventoryLotList);
            //cmbUOM.ReadOnly = false;
            btnCancel.Visible = btnOK.Visible = true;
            //txtUOM.Visible = true;
            dgvInventoryLot.Height = btnOK.Top - 20; 
            CommonUIDisplay.setupVisuals(this);
            CommonUIDisplay.Utilities = utilities;
            CommonUIDisplay.ParafaitEnv = utilities.ParafaitEnv;
            this.StartPosition = FormStartPosition.CenterScreen; 
            LoadUOMComboBox();
            log.LogMethodExit();
        }


        void populateInventoryLot(int LotId, int ProductId = -1)
        {
            log.LogMethodEntry(LotId, ProductId);
            InventoryLotList inventoryLotList = new InventoryLotList(machineUserContext);
            List<InventoryLotDTO> inventoryLotListOnDisplay = new List<InventoryLotDTO>();

            List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> inventoryLotSearchParams = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>();
            inventoryLotSearchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.IS_ACTIVE, "1"));
            inventoryLotSearchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryLotSearchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID, LotId.ToString()));
            inventoryLotListOnDisplay = inventoryLotList.GetAllInventoryLot(inventoryLotSearchParams);
            PopulateGrid(inventoryLotListOnDisplay);
            log.LogMethodExit();
        }
        void PopulateGrid(List<InventoryLotDTO> inventoryLotListOnDisplay)
        {
            log.LogMethodEntry(inventoryLotListOnDisplay);
            SortableBindingList<InventoryLotDTO> sortInventoryLotDTOList;
            if (inventoryLotListOnDisplay == null)
            {
                sortInventoryLotDTOList = new SortableBindingList<InventoryLotDTO>();
            }
            else
            {
                foreach (InventoryLotDTO inventoryLotDTO in inventoryLotListOnDisplay)
                {
                    if (inventoryLotDTO.Expirydate.Equals(DateTime.MinValue) && inventoryLotDTO.ExpiryInDays > 0)
                    {
                        inventoryLotDTO.Expirydate = DateTime.Today.AddDays(inventoryLotDTO.ExpiryInDays);
                    }
                }
                sortInventoryLotDTOList = new SortableBindingList<InventoryLotDTO>(inventoryLotListOnDisplay);
            }
            BindingSource InventoryLotDTOListBS = new BindingSource();
            InventoryLotDTOListBS.DataSource = sortInventoryLotDTOList;
            dgvInventoryLot.DataSource = InventoryLotDTOListBS;
            dgvInventoryLot.Columns["expirydateDataGridViewTextBoxColumn"].DefaultCellStyle.Format = utilities.getDateFormat();
            dgvInventoryLot.Columns["txtUOM"].Visible = false;
            log.LogMethodExit();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            log.LogMethodEntry(sender, e);
            BindingSource inventoryLotBS = (BindingSource)dgvInventoryLot.DataSource;
            var inventoryLotListOnDisplay = (SortableBindingList<InventoryLotDTO>)inventoryLotBS.DataSource;
            inventoryLotListOnReturn = new List<InventoryLotDTO>();
            TotalReturnQuantity = 0;
            InventoryLotDTO inventoryLotDTO = new InventoryLotDTO();
            for (int i = 0; i < inventoryLotListOnDisplay.Count; i++)
            {
                inventoryLotDTO = inventoryLotListOnDisplay[i];
                if (!quantityDataGridViewTextBoxColumn.ReadOnly)
                {
                    if (!applicability.Equals("VendorReturn"))
                    {
                        //Updated condition to check for applicablity in addition to quantity 21-Feb-2017
                        if (inventoryLotDTO.Quantity == 0)
                        {
                            MessageBox.Show("Quantity can not be 0.");
                            return;
                        }
                        //int userEnteredUOMId = Convert.ToInt32(dgvInventoryLot.Rows[i].Cells["cmbUOM"].Value);
                        //if (userEnteredUOMId != inventoryLotDTO.UOMId)
                        //{
                        //    double factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryLotDTO.UOMId);
                        //    inventoryLotDTO.Quantity = inventoryLotDTO.Quantity * factor;
                        //}
                        if (inventoryLotDTO.Quantity > 0)
                        {
                            inventoryLotDTO.BalanceQuantity = inventoryLotDTO.Quantity;
                        }
                    }
                }
                if (inventoryLotDTO.BalanceQuantity == 0)
                {
                    MessageBox.Show("Balance quantity can not be 0.");
                    return;
                }

                if (inventoryLotDTO.Quantity > inventoryLotDTO.BalanceQuantity)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("Cannot allow to return quantity more than balance quantity"));
                    return;
                }

                if (!expiryInDaysDataGridViewTextBoxColumn.ReadOnly)
                {
                    if (inventoryLotDTO.ExpiryInDays > 0)
                    {
                        inventoryLotDTO.Expirydate = ServerDateTime.Now.Date.AddDays(inventoryLotDTO.ExpiryInDays);
                    }
                    else
                    {
                        MessageBox.Show("Expiry in days should be greater than 0.");
                        return;
                    }
                }

                if (inventoryLotDTO.Expirydate.Date < ServerDateTime.Now.Date && !expiryType.Equals("N"))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage("Cannot allow to return quantity Expired items"));
                    return;
                }

                TotalReturnQuantity += inventoryLotDTO.BalanceQuantity;
                inventoryLotListOnReturn.Add(inventoryLotDTO);
            }
            if (!applicability.Equals("VendorReturn"))
            {
                if (TotalReturnQuantity != TotalQuantity)
                {
                    MessageBox.Show("Entered quantity is not matching with total quantity.");
                    inventoryLotListOnReturn = null;
                    return;
                }
            }
            this.Close();
            log.LogMethodExit();
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvInventoryLot.CurrentRow != null)
            {
                InventoryLotDTO inventoryLotDTO = new InventoryLotDTO();
                BindingSource inventoryLotBS = (BindingSource)dgvInventoryLot.DataSource;
                var inventoryLotListOnDisplay = (SortableBindingList<InventoryLotDTO>)inventoryLotBS.DataSource;
                inventoryLotListOnReturn = new List<InventoryLotDTO>();
                foreach (InventoryLotDTO gridInventoryLotDTO in inventoryLotListOnDisplay)
                {
                    inventoryLotListOnReturn.Add(gridInventoryLotDTO);
                }
                inventoryLotDTO.BalanceQuantity = 0;
                inventoryLotDTO.Expirydate = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].Expirydate;
                inventoryLotDTO.IsActive = true;
                inventoryLotDTO.OriginalQuantity = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].OriginalQuantity;
                inventoryLotDTO.PurchaseOrderReceiveLineId = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].PurchaseOrderReceiveLineId;
                inventoryLotDTO.ReceivePrice = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].ReceivePrice;
                inventoryLotDTO.UOM = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].UOM;
                inventoryLotDTO.UOMId = inventoryLotListOnDisplay[dgvInventoryLot.CurrentRow.Index].UOMId;
                inventoryLotListOnReturn.Add(inventoryLotDTO);
                PopulateGrid(inventoryLotListOnReturn);
                LoadUOMComboBox();
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            inventoryLotListOnReturn = null;
            this.Close();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (this.dgvInventoryLot.SelectedRows.Count <= 0 && this.dgvInventoryLot.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                if (this.dgvInventoryLot.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvInventoryLot.SelectedCells)
                    {
                        dgvInventoryLot.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow InventoryLotRow in this.dgvInventoryLot.SelectedRows)
                {
                    if (InventoryLotRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(InventoryLotRow.Cells[0].Value) <= 0)
                        {
                            dgvInventoryLot.Rows.RemoveAt(InventoryLotRow.Index);
                            rowsDeleted = true;
                        }

                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvInventoryLot_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(3, e.RowIndex, dgvInventoryLot.Columns[e.ColumnIndex].HeaderText));
            log.LogMethodExit();
        }

        //Start update 23-Feb-2017
        DateTimePicker dtp;
        private void dgvInventoryLot_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvInventoryLot.Columns[e.ColumnIndex].Name == "selectExpiryDate" && e.RowIndex > -1)
            {
                dtp = new DateTimePicker();
                dgvInventoryLot.Controls.Add(dtp);
                dtp.Format = DateTimePickerFormat.Short;
                Rectangle Rectangle = dgvInventoryLot.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(Rectangle.Width, Rectangle.Height);
                dtp.Location = new Point(Rectangle.X, Rectangle.Y);

                dtp.CloseUp += new EventHandler(dtp_CloseUp);
                dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                dtp.Visible = true;
            }
            log.LogMethodExit();
        }

        private void dtp_OnTextChange(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dtp.Value.Date < ServerDateTime.Now.Date)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Expiry date cannot be less than current date"), utilities.MessageUtils.getMessage("Select Expiry date"));
                return;
            }
            dgvInventoryLot.Rows[dgvInventoryLot.CurrentRow.Index].Cells["expirydateDataGridViewTextBoxColumn"].Value = dtp.Text.ToString();
            log.LogMethodExit();
        }

        void dtp_CloseUp(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dtp.Visible = false;
            log.LogMethodExit();
        }

        private void dgvInventoryLot_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvInventoryLot.CurrentCell != null && e.RowIndex > -1)
            {
                if (entityExclusionDetailDTOList != null && entityExclusionDetailDTOList.Count > 0)
                {
                    foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in entityExclusionDetailDTOList)
                    {
                        if (entityExclusionDetailDTO.FieldName == dgvInventoryLot.Columns[e.ColumnIndex].DataPropertyName && dgvInventoryLot.Columns[e.ColumnIndex].Visible == true)
                        { 
                            e.Value = new String('\u25CF', 6);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                int rowIndex = dgvInventoryLot.CurrentRow.Index;
                BindingSource bindingSource = (BindingSource)dgvInventoryLot.DataSource;
                SortableBindingList<InventoryLotDTO> lotDTOList = (SortableBindingList<InventoryLotDTO>)bindingSource.DataSource;
                if (dgvInventoryLot.Rows[rowIndex].Cells["cmbUOM"].Value == null)
                {
                    for (int i = 0; i < lotDTOList.Count; i++)
                    {
                        int uomId = lotDTOList[i].UOMId;
                        if(string.IsNullOrEmpty(lotDTOList[i].UOM))
                        {
                            lotDTOList[i].UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvInventoryLot, i, uomId);
                        if (poUOMId > -1)
                        {
                            dgvInventoryLot.Rows[i].Cells["cmbUOM"].Value = poUOMId;
                        }
                        else
                        {
                            dgvInventoryLot.Rows[i].Cells["cmbUOM"].Value = lotDTOList[i].UOMId;
                        }
                        dgvInventoryLot.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == lotDTOList[i].UOMId).UOM;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void InventoryLotUI_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }

        private void dgvInventoryLot_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                LoadUOMComboBox();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
