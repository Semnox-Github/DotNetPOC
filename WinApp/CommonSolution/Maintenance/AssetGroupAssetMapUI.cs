/********************************************************************************************
 * Project Name - Asset Group Asset Map UI
 * Description  - User interface for asset group asset map
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        22-Feb-2016   Suneetha.S          Modified 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
 *2.70.2      12-Aug-2019   Deeksha             Added logger methods.
 *2.70.3      02-Apr-2020   Girish Kundar       Modified : frmLoad() method  and refresh() to get active and inavtive records in UI
 *2.80        10-May-2020   Girish Kundar       Modified: REST API Changes    
 ********************************************************************************************/

using System;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Group Asset Map UI
    /// </summary>
    public partial class AssetGroupAssetMapUI : Form
    {
        Utilities utilities;
        AssetList assetList;
        List<GenericAssetDTO> assetListOnDisplay;
        AssetGroupList assetGroupList;
        List<AssetGroupDTO> assetGroupListOnDisplay;
        BindingSource assetGroupAssetListBS;
        bool loadSearch = true;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public AssetGroupAssetMapUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            RegisterKeyDownHandlers(this);//Modification on 22-Feb-2016 to hide the search grid control
            utilities = _Utilities;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
            {
                btnPublishToSite.Visible = true;
            }
            else
            {
                btnPublishToSite.Visible = false;
            }//Ends: Modification on 14-Jul-2016 for adding publish to site feature 
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            _Utilities.setupDataGridProperties(ref assetGroupAssetDataGridView);
            utilities.setLanguage(this);
            PopulateAssets();
            LoadAssetsearch();
            PopulateAssetGroup();
            LoadGroupsearch();
            PopulateAssetMapGrid();
            log.LogMethodExit();
        }

        private void PopulateAssets()
        {
            log.LogMethodEntry();
            AssetList assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> searchByAssetParameters = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            searchByAssetParameters.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, "Y"));
            assetListOnDisplay = assetList.GetAllAssets(searchByAssetParameters);
            if (assetListOnDisplay == null)
            {
                assetListOnDisplay = new List<GenericAssetDTO>();
            }
            assetListOnDisplay.Insert(0, new GenericAssetDTO());
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = assetListOnDisplay;

            assetIdDataGridViewTextBoxColumn.DataSource = assetListOnDisplay;
            assetIdDataGridViewTextBoxColumn.ValueMember = "AssetId";
            assetIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            assetIdDataGridViewTextBoxColumn.DisplayMember = "Name";

            cmbAssetName.DataSource = bindingSource;
            cmbAssetName.ValueMember = "AssetId";
            cmbAssetName.DisplayMember = "Name";
            log.LogMethodExit();
        }
        private void PopulateAssetGroup()
        {
            log.LogMethodEntry();
            AssetGroupList assetGroupList = new AssetGroupList(machineUserContext);
            List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
            assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG, "Y"));
            assetGroupListOnDisplay = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
            if (assetGroupListOnDisplay == null)
            {
                assetGroupListOnDisplay = new List<AssetGroupDTO>();
            }
            assetGroupListOnDisplay.Insert(0, new AssetGroupDTO());
            BindingSource bindingSource1 = new BindingSource();
            bindingSource1.DataSource = assetGroupListOnDisplay;
            assetGroupIdDataGridViewTextBoxColumn.DataSource = assetGroupListOnDisplay;
            assetGroupIdDataGridViewTextBoxColumn.ValueMember = "AssetGroupId";
            assetGroupIdDataGridViewTextBoxColumn.ValueType = typeof(Int32);
            assetGroupIdDataGridViewTextBoxColumn.DisplayMember = "AssetGroupName";
            cmbGroupName.DataSource = bindingSource1;
            cmbGroupName.ValueMember = "AssetGroupId";
            cmbGroupName.DisplayMember = "AssetGroupName";
            log.LogMethodExit();
        }


        /// <summary>
        /// Loads assetMap records to the grid
        /// </summary>
        private void PopulateAssetMapGrid()
        {
            log.LogMethodEntry();
            AssetGroupAssetMapperList assetGroupAssetList = new AssetGroupAssetMapperList(utilities.ExecutionContext);
            List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> assetSearchParams = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : "N"));
            assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay = assetGroupAssetList.GetAllAssetGroupAsset(assetSearchParams);
            assetGroupAssetListBS = new BindingSource();
            if(assetGroupAssetListOnDisplay == null)
            {
                assetGroupAssetListOnDisplay = new List<AssetGroupAssetDTO>();
            }
            if (assetGroupAssetListOnDisplay != null)
            {
                SortableBindingList<AssetGroupAssetDTO> assetGroupAssetDTOSortList = new SortableBindingList<AssetGroupAssetDTO>(assetGroupAssetListOnDisplay);
                assetGroupAssetListBS.DataSource = assetGroupAssetDTOSortList;
            }
            else
                assetGroupAssetListBS.DataSource = new SortableBindingList<AssetGroupAssetDTO>();
            assetGroupAssetListBS.AddingNew += assetGroupAssetDataGridView_BindingSourceAddNew;
            assetGroupAssetDataGridView.DataSource = assetGroupAssetListBS;
            assetGroupAssetDataGridView.DataError += new DataGridViewDataErrorEventHandler(assetGroupAssetDataGridView_ComboDataError);
            log.LogMethodExit();
        }

        void assetGroupAssetDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (assetGroupAssetDataGridView.Rows.Count == assetGroupAssetListBS.Count)
            {
                assetGroupAssetListBS.RemoveAt(assetGroupAssetListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        void assetGroupAssetDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == assetGroupAssetDataGridView.Columns["assetIdDataGridViewTextBoxColumn"].Index)
            {
                if (assetListOnDisplay != null)
                    assetGroupAssetDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = assetListOnDisplay[0].AssetId;
            }
            else if (e.ColumnIndex == assetGroupAssetDataGridView.Columns["assetGroupIdDataGridViewTextBoxColumn"].Index)
            {
                if (assetGroupListOnDisplay != null)
                    assetGroupAssetDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = assetGroupListOnDisplay[0].AssetGroupId;
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> assetSearchParams = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG, "Y"));
            }
            if (cmbAssetName.SelectedValue != null && !cmbAssetName.SelectedValue.ToString().Equals("-1"))
            {
                assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID, (cmbAssetName.SelectedValue == null) ? "" : cmbAssetName.SelectedValue.ToString().Equals("-1") ? "" : cmbAssetName.SelectedValue.ToString()));
            }
            if (cmbGroupName.SelectedValue != null && !cmbGroupName.SelectedValue.ToString().Equals("-1"))
            {
                assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID, (cmbGroupName.SelectedValue == null) ? "" : cmbGroupName.SelectedValue.ToString().Equals("-1") ? "" : cmbGroupName.SelectedValue.ToString()));
            }
            assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            AssetGroupAssetMapperList assetGroupAssetMapperList = new AssetGroupAssetMapperList(utilities.ExecutionContext);
            List<AssetGroupAssetDTO> assetGroupAssetDTOList = assetGroupAssetMapperList.GetAllAssetGroupAsset(assetSearchParams);
            BindingSource assetGroupAssetDTOListBS = new BindingSource();
            if (assetGroupAssetDTOList != null)
            {
                SortableBindingList<AssetGroupAssetDTO> assetGroupAssetDTOSortList = new SortableBindingList<AssetGroupAssetDTO>(assetGroupAssetDTOList);
                assetGroupAssetDTOListBS.DataSource = assetGroupAssetDTOSortList;
            }
            else
            {
                assetGroupAssetDTOListBS.DataSource = new SortableBindingList<AssetGroupAssetDTO>();
            }
            assetGroupAssetDataGridView.DataSource = assetGroupAssetDTOListBS;
            log.LogMethodExit();
        }

        private void assetGroupSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource assetGroupAssetListBS = (BindingSource)assetGroupAssetDataGridView.DataSource;
            var assetGroupAssetListOnDisplay = (SortableBindingList<AssetGroupAssetDTO>)assetGroupAssetListBS.DataSource;
            if (assetGroupAssetListOnDisplay.Count > 0)
            {
                foreach (AssetGroupAssetDTO assetGroupAssetDTO in assetGroupAssetListOnDisplay)
                {
                    if (assetGroupAssetDTO.AssetId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(961));
                        log.LogMethodExit();
                        return;
                    }
                    if (assetGroupAssetDTO.AssetGroupId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(967));
                        log.LogMethodExit();
                        return;
                    }
                    try
                    {
                        AssetGroupAssetMapper assetGroupAsset = new AssetGroupAssetMapper(utilities.ExecutionContext,assetGroupAssetDTO);
                        assetGroupAsset.Save();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        log.Error("Ends-assetGroupSaveBtn_Click() event with exception : ", ex);
                        if (ex.Number == 2601 || ex.Number == 2627)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(998), utilities.MessageUtils.getMessage("Database Save"));
                        }
                        else
                        {
                            MessageBox.Show(ex.Number + " : " + ex.Message, utilities.MessageUtils.getMessage("Database Save"));
                        }
                        log.LogMethodExit();
                        return;
                    }
                    catch (Exception ex)
                    {
                        string message = utilities.MessageUtils.getMessage(968) + " \"" + assetGroupListOnDisplay.Where(x => (bool)(x.AssetGroupId == assetGroupAssetDTO.AssetGroupId)).ToList<AssetGroupDTO>()[0].AssetGroupName + "\" And \"" + assetListOnDisplay.Where(x => (bool)(x.AssetId == assetGroupAssetDTO.AssetId)).ToList<GenericAssetDTO>()[0].Name + "\". \n" + ex.Message;
                        MessageBox.Show(message);
                        log.Error("Ends-assetGroupSaveBtn_Click() event with exception : ", ex);
                        log.LogMethodExit();
                        return;
                    }
                }
                btnSearch.PerformClick();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void assetGroupRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbAssetName.SelectedValue = -1;
            cmbGroupName.SelectedValue = -1;
            chbShowActiveEntries.Checked = true;
            PopulateAssets();
            PopulateAssetGroup();
            PopulateAssetMapGrid();
            log.LogMethodExit();
        }

        private void assetGroupDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.assetGroupAssetDataGridView.SelectedRows.Count <= 0 && this.assetGroupAssetDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-assetGroupDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message.");
                log.LogMethodExit();
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.assetGroupAssetDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.assetGroupAssetDataGridView.SelectedCells)
                {
                    assetGroupAssetDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow assetRow in this.assetGroupAssetDataGridView.SelectedRows)
            {
                if (assetRow.Cells[0].Value == null)
                {
                    log.LogMethodExit();
                    return;
                }
                if (Convert.ToInt32(assetRow.Cells[0].Value.ToString()) <= 0)
                {
                    assetGroupAssetDataGridView.Rows.RemoveAt(assetRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource assetGroupAssetDTOListDTOBS = (BindingSource)assetGroupAssetDataGridView.DataSource;
                        var assetGroupAssetDTOList = (SortableBindingList<AssetGroupAssetDTO>)assetGroupAssetDTOListDTOBS.DataSource;
                        AssetGroupAssetDTO assetGroupAssetDTO = assetGroupAssetDTOList[assetRow.Index];
                        assetGroupAssetDTO.IsActive = false;
                        AssetGroupAssetMapper assetGroupAsset = new AssetGroupAssetMapper(utilities.ExecutionContext,assetGroupAssetDTO);
                        assetGroupAsset.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            btnSearch.PerformClick();
            log.LogMethodExit();
        }

        private void assetGroupCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void assetGroupAssetDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + assetGroupAssetDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads Serached Records
        /// </summary>
        private void LoadAssetsearch()
        {
            log.LogMethodEntry();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = assetListOnDisplay;
            dgvAssetSearch.DataSource = bindingSource;
            for (int i = 0; i < dgvAssetSearch.Columns.Count; i++)
            {
                if (!dgvAssetSearch.Columns[i].Name.Equals("Name"))
                {
                    dgvAssetSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvAssetSearch.Columns[i].Width = dgvAssetSearch.Width;
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads Serached Records
        /// </summary>
        private void LoadGroupsearch()
        {
            log.LogMethodEntry();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = assetGroupListOnDisplay;
            dgvAssetGroupSearch.DataSource = bindingSource;
            for (int i = 0; i < dgvAssetGroupSearch.Columns.Count; i++)
            {
                if (!dgvAssetGroupSearch.Columns[i].Name.Equals("AssetGroupName"))
                {
                    dgvAssetGroupSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvAssetGroupSearch.Columns[i].Width = dgvAssetSearch.Width;
                }
            }
            log.LogMethodExit();
        }

        private void txtAssetSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtAssetSearch.Text.Length > 0)
                {
                    if (assetListOnDisplay != null)
                    {
                        List<GenericAssetDTO> assetDTOList = assetListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtAssetSearch.Text.ToLower()))).ToList<GenericAssetDTO>();
                        if (assetDTOList.Count > 0)
                        {
                            dgvAssetSearch.Visible = true;
                            dgvAssetSearch.DataSource = assetDTOList;
                        }
                        else
                        {
                            dgvAssetSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssetName.Text = "";
                    dgvAssetSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void txtAssetGroupSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtAssetGroupSearch.Text.Length > 0)
                {
                    if (assetGroupListOnDisplay != null)
                    {
                        List<AssetGroupDTO> assetTypeDTOList = assetGroupListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.AssetGroupName) ? "" : x.AssetGroupName.ToLower()).Contains(txtAssetGroupSearch.Text.ToLower()))).ToList<AssetGroupDTO>();
                        if (assetTypeDTOList.Count > 0)
                        {
                            dgvAssetGroupSearch.Visible = true;
                            dgvAssetGroupSearch.DataSource = assetTypeDTOList;
                        }
                        else
                        {
                            dgvAssetGroupSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbGroupName.Text = "";
                    dgvAssetGroupSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        private void dgvAssetSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtAssetSearch.Text = dgvAssetSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssetName.Text = txtAssetSearch.Text;
                dgvAssetSearch.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing dgvAssetSearch_CellClick()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvAssetGroupSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtAssetGroupSearch.Text = dgvAssetGroupSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbGroupName.Text = txtAssetGroupSearch.Text;
                dgvAssetGroupSearch.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing dgvAssetGroupSearch_CellClick()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void cmbAssetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbAssetName.Text.Equals("Semnox.Parafait.Maintenance.GenericAssetDTO"))
            {
                txtAssetSearch.Text = "";
            }
            else
            {
                txtAssetSearch.Text = cmbAssetName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        private void cmbGroupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbGroupName.Text.Equals("Semnox.Parafait.Maintenance.AssetGroupDTO"))
            {
                txtAssetGroupSearch.Text = "";
            }
            else
            {
                txtAssetGroupSearch.Text = cmbGroupName.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// To fix the issue hiding the List on click of any controls in the form
        /// </summary>
        /// <param name="control"></param>
        private void RegisterKeyDownHandlers(Control control)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry(control);
            foreach (Control ctl in control.Controls)
            {
                ctl.Click += MyKeyPressEventHandler;
                RegisterKeyDownHandlers(ctl);
            }
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        public void MyKeyPressEventHandler(Object sender, EventArgs e)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry();
            dgvAssetGroupSearch.Visible = dgvAssetSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry();
            dgvAssetGroupSearch.Visible = dgvAssetSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        private void txtAssetSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssetSearch.Rows.Count > 0)
                {
                    dgvAssetSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtAssetSearch.Text = dgvAssetSearch.SelectedCells[0].Value.ToString();
                    cmbAssetName.Text = txtAssetSearch.Text;
                    dgvAssetSearch.Visible = false;
                    txtAssetSearch.Focus();
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing dgvAssetSearch_KeyDown()" + ex.Message);
                }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtAssetSearch.Focus();
            }
            log.LogMethodExit();
        }

        private void txtAssetGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssetSearch.Rows.Count > 0)
                {
                    dgvAssetSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetGroupSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtAssetGroupSearch.Text = dgvAssetGroupSearch.SelectedCells[0].Value.ToString();
                    cmbGroupName.Text = txtAssetGroupSearch.Text;
                    dgvAssetGroupSearch.Visible = false;
                    txtAssetGroupSearch.Focus();
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing dgvAssetGroupSearch_KeyDown()" + ex.Message);
                }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtAssetGroupSearch.Focus();
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            try
            {
                if (assetGroupAssetDataGridView.SelectedRows != null && assetGroupAssetDataGridView.SelectedRows.Count > 0)
                {
                    int assetIdSelected = -1;
                    if (assetGroupAssetDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(assetGroupAssetDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out assetIdSelected);
                    }
                    if (assetIdSelected >= 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, assetIdSelected, "AssetGroupAsset", assetGroupAssetDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    }
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Asset group asset Publish");
                log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
            }
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature

        private void AssetGroupAssetMapUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnPublishToSite.Size = new Size(127, 23);//Modification on 14-Jul-2016 for adding publish to site feature
            log.LogMethodExit();
        }
    }
}
