/********************************************************************************************
 * Project Name - Sales Offer Group UI
 * Description  - User interface for Sales Offer Group UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
* 2.80        28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Product
{
    public partial class SalesOfferGroupUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private ManagementStudioSwitch managementStudioSwitch;
        public SalesOfferGroupUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvDisplayData);

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
            PopulateSalesOfferGroupsGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        private void PopulateSalesOfferGroupsGrid()
        {
            log.LogMethodEntry();
            SalesOfferGroupList salesOfferGroupList = new SalesOfferGroupList(machineUserContext);
            List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> salesOfferGroupSearchParams = new List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>>();
            if(chbShowActiveEntries.Checked)
            salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.IS_ACTIVE, (chbShowActiveEntries.Checked) ? "1" : "0"));
            salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (!string.IsNullOrEmpty(txtName.Text))
                salesOfferGroupSearchParams.Add(new KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>(SalesOfferGroupDTO.SearchBySalesOfferGroupParameters.NAME, txtName.Text));

            List<SalesOfferGroupDTO> salesOfferGroupListOnDisplay = salesOfferGroupList.GetAllSalesOfferGroups(salesOfferGroupSearchParams);
            BindingSource salesOfferGroupListBS = new BindingSource();
            if (salesOfferGroupListOnDisplay != null)
            {
                SortableBindingList<SalesOfferGroupDTO> salesOfferGroupDTOSortList = new SortableBindingList<SalesOfferGroupDTO>(salesOfferGroupListOnDisplay);
                salesOfferGroupListBS.DataSource = salesOfferGroupDTOSortList;
            }
            else
                salesOfferGroupListBS.DataSource = new SortableBindingList<SalesOfferGroupDTO>();
            dgvDisplayData.DataSource = salesOfferGroupListBS;
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource salesOfferGroupListBS = (BindingSource)dgvDisplayData.DataSource;
            var salesOfferGroupListOnDisplay = (SortableBindingList<SalesOfferGroupDTO>)salesOfferGroupListBS.DataSource;
            if (salesOfferGroupListOnDisplay.Count > 0)
            {
                foreach (SalesOfferGroupDTO salesOfferGroupDTO in salesOfferGroupListOnDisplay)
                {
                    if (string.IsNullOrEmpty(salesOfferGroupDTO.Name))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1249));
                        return;
                    }
                    SalesOfferGroup salesOfferGroup = new SalesOfferGroup(machineUserContext, salesOfferGroupDTO);
                    salesOfferGroup.Save(null);
                }
                PopulateSalesOfferGroupsGrid();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            PopulateSalesOfferGroupsGrid();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.dgvDisplayData.SelectedRows.Count <= 0 && this.dgvDisplayData.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-salesOfferGroupDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.dgvDisplayData.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.dgvDisplayData.SelectedCells)
                {
                    dgvDisplayData.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow salesOfferGroupRow in this.dgvDisplayData.SelectedRows)
            {
                if (salesOfferGroupRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(salesOfferGroupRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvDisplayData.Rows.RemoveAt(salesOfferGroupRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource salesOfferGroupDTOListDTOBS = (BindingSource)dgvDisplayData.DataSource;
                        var salesOfferGroupDTOList = (SortableBindingList<SalesOfferGroupDTO>)salesOfferGroupDTOListDTOBS.DataSource;
                        SalesOfferGroupDTO salesOfferGroupDTO = salesOfferGroupDTOList[salesOfferGroupRow.Index];
                        salesOfferGroupDTO.IsActive = false;
                        SalesOfferGroup salesOfferGroup = new SalesOfferGroup(machineUserContext, salesOfferGroupDTO);
                        salesOfferGroup.Save(null);
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            PopulateSalesOfferGroupsGrid();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateSalesOfferGroupsGrid();
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDisplayData.AllowUserToAddRows = true;
                dgvDisplayData.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDisplayData.AllowUserToAddRows = false;
                dgvDisplayData.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
