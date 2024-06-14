/********************************************************************************************
 * Project Name - Asset Group UI
 * Description  - User interface for asset group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *2.70.2      12-Jul-2019   Deeksha             Added logger methods.
 2.70.3       02-Apr-2020   Girish Kundar       Modified: Do not allow duplicate  name
 *2.80        10-May-2020   Girish Kundar       Modified: REST API Changes   
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Asset Group UI
    /// </summary>
    public partial class AssetGroupUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public AssetGroupUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref assetGroupsDataGridView);
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
            utilities.setLanguage(this);
            PopulateAssetGroupGrid();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the asset group record to the grid
        /// </summary>
        private void PopulateAssetGroupGrid()
        {
            log.LogMethodEntry();
            AssetGroupList assetGroupList = new AssetGroupList(machineUserContext);
            List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
            assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : ""));
            assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            List<AssetGroupDTO> assetGroupListOnDisplay = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
            BindingSource assetGroupListBS = new BindingSource();
            if (assetGroupListOnDisplay != null)
            {
                SortableBindingList<AssetGroupDTO> assetGroupDTOSortList = new SortableBindingList<AssetGroupDTO>(assetGroupListOnDisplay);
                assetGroupListBS.DataSource = assetGroupDTOSortList;
            }
            else
                assetGroupListBS.DataSource = new SortableBindingList<AssetGroupDTO>();
            assetGroupsDataGridView.DataSource = assetGroupListBS;
            log.LogMethodExit();
        }

        private void assetGroupSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource assetGroupBS = (BindingSource)assetGroupsDataGridView.DataSource;
            var assetGroupListOnDisplay = (SortableBindingList<AssetGroupDTO>)assetGroupBS.DataSource;
          
            if (assetGroupListOnDisplay.Count > 0)
            {
                List<AssetGroupDTO> tempList = new List<AssetGroupDTO>(assetGroupListOnDisplay);
                var isNull = tempList.Any(item => item.AssetGroupName == null);
                if (isNull)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "asset group"), "Validation Error");
                    return;
                }
                List<string> nameList = tempList.Select(x => x.AssetGroupName.Trim().ToLower()).ToList();
                if (nameList.Count != nameList.Distinct().Count())
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "asset group"), "Validation Error");
                    return;
                }
                foreach (AssetGroupDTO assetGroupDTO in assetGroupListOnDisplay)
                {
                    if (string.IsNullOrEmpty(assetGroupDTO.AssetGroupName.Trim()))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(969));
                        return;
                    }
                    AssetGroup assetGroup = new AssetGroup(assetGroupDTO);
                    assetGroup.Save();
                }
                btnSearch.PerformClick();
                log.LogMethodExit();
            }
        }

        private void assetGroupCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void assetGroupRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            PopulateAssetGroupGrid();
            log.LogMethodExit();
        }

        private void assetGroupDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.assetGroupsDataGridView.SelectedRows.Count <= 0 && this.assetGroupsDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-assetGroupDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message .");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.assetGroupsDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.assetGroupsDataGridView.SelectedCells)
                {
                    assetGroupsDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow assetGroupRow in this.assetGroupsDataGridView.SelectedRows)
            {
                if (assetGroupRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(assetGroupRow.Cells[0].Value.ToString()) <= 0)
                {
                    assetGroupsDataGridView.Rows.RemoveAt(assetGroupRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource assetGroupDTOListDTOBS = (BindingSource)assetGroupsDataGridView.DataSource;
                        var assetGroupDTOList = (SortableBindingList<AssetGroupDTO>)assetGroupDTOListDTOBS.DataSource;
                        AssetGroupDTO assetGroupDTO = assetGroupDTOList[assetGroupRow.Index];
                        assetGroupDTO.IsActive = false;
                        AssetGroup assetGroup = new AssetGroup(assetGroupDTO);
                        assetGroup.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            btnSearch.PerformClick();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG, "Y"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME, txtName.Text));
            }
            assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            AssetGroupList assetGroupList = new AssetGroupList(machineUserContext);
            List<AssetGroupDTO> assetGroupDTOList = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
            BindingSource bindingsource = new BindingSource();
            if (assetGroupDTOList != null)
            {
                SortableBindingList<AssetGroupDTO> assetGroupDTOSortList = new SortableBindingList<AssetGroupDTO>(assetGroupDTOList);
                bindingsource.DataSource = assetGroupDTOSortList;
            }
            else
            {
                bindingsource.DataSource = new SortableBindingList<AssetGroupDTO>();
            }
            assetGroupsDataGridView.DataSource = bindingsource;
            log.LogMethodExit();
        }

        private void assetGroupsDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + assetGroupsDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            try
            {
                if (assetGroupsDataGridView.SelectedRows != null && assetGroupsDataGridView.SelectedRows.Count > 0)
                {
                    int groupIdSelected = -1;
                    if (assetGroupsDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(assetGroupsDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out groupIdSelected);
                    }
                    if (groupIdSelected > 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, groupIdSelected, "AssetGroup", assetGroupsDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    }
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Asset Group Publish");
                log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
            }
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature

        private void AssetGroupUI_Load(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            btnPublishToSite.Size = new System.Drawing.Size(126, 23);
            log.LogMethodExit();
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature 
    }
}
