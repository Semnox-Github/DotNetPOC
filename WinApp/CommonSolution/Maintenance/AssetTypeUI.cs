/********************************************************************************************
 * Project Name - Asset Type UI
 * Description  - User interface for asset type
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *2.70.2      12-Aug-2019   Deeksha             Modified logger methods.
 *2.70.3       02-Apr-2020  Girish Kundar       Modified: Do not allow duplicate  name
 *2.80        10-May-2020   Girish Kundar       Modified: REST API Changes merge from WMS  
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
    /// Asset type UI
    /// </summary>
    public partial class AssetTypeUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public AssetTypeUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref assetTypeDataGridView);

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
            PopulateAssetTypeGrid();
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the asset type to grid
        /// </summary>
        private void PopulateAssetTypeGrid()
        {
            log.LogMethodEntry();
            AssetTypeList assetTypeList = new AssetTypeList(utilities.ExecutionContext);
            List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
            assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : ""));
            assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<AssetTypeDTO> assetTypeListOnDisplay = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
            BindingSource assetTypeListBS = new BindingSource();
            if (assetTypeListOnDisplay != null)
            {
                SortableBindingList<AssetTypeDTO> assetTypeDTOSortList = new SortableBindingList<AssetTypeDTO>(assetTypeListOnDisplay);
                assetTypeListBS.DataSource = assetTypeDTOSortList;
            }
            else
                assetTypeListBS.DataSource = new SortableBindingList<AssetTypeDTO>();
            assetTypeDataGridView.DataSource = assetTypeListBS;
            log.LogMethodExit();
        }

        private void assetTypeCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void assetTypeRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            PopulateAssetTypeGrid();
            log.LogMethodExit();
        }

        private void assetTypeSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource assetTypeListBS = (BindingSource)assetTypeDataGridView.DataSource;
            var assetTypeListOnDisplay = (SortableBindingList<AssetTypeDTO>)assetTypeListBS.DataSource;
            if (assetTypeListOnDisplay.Count > 0)
            {
                List<AssetTypeDTO> tempList = new List<AssetTypeDTO>(assetTypeListOnDisplay);
                var isNull = tempList.Any(item => item.Name == null);
                if (isNull)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "asset type"), "Validation Error");
                    return;
                }
                List<string> nameList = tempList.Select(x => x.Name.Trim().ToLower()).ToList();
                if (nameList.Count != nameList.Distinct().Count())
                {
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "asset type"), "Validation Error");
                    return;
                }
                foreach (AssetTypeDTO assetTypeDTO in assetTypeListOnDisplay)
                {
                    if (string.IsNullOrEmpty(assetTypeDTO.Name.Trim()))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(962), "Validation Error");
                        log.LogMethodExit();
                        return;
                    }

                    AssetType assetType = new AssetType(utilities.ExecutionContext, assetTypeDTO);
                    assetType.Save();
                }
                btnSearch.PerformClick();

            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void assetTypeDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.assetTypeDataGridView.SelectedRows.Count <= 0 && this.assetTypeDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-assetTypeDeleteBtn_Click() event by showing \"No rows selected. Please select the rows you want to delete and press delete..\" message");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.assetTypeDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.assetTypeDataGridView.SelectedCells)
                {
                    assetTypeDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow assetTypeRow in this.assetTypeDataGridView.SelectedRows)
            {
                if (assetTypeRow.Cells[0].Value == null)
                {
                    log.LogMethodExit();
                    return;
                }
                if (Convert.ToInt32(assetTypeRow.Cells[0].Value.ToString()) <= 0)
                {
                    assetTypeDataGridView.Rows.RemoveAt(assetTypeRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource assetTypeDTOListDTOBS = (BindingSource)assetTypeDataGridView.DataSource;
                        var assetTypeDTOList = (SortableBindingList<AssetTypeDTO>)assetTypeDTOListDTOBS.DataSource;
                        AssetTypeDTO assetTypeDTO = assetTypeDTOList[assetTypeRow.Index];
                        assetTypeDTO.IsActive = false;
                        AssetType assetType = new AssetType(utilities.ExecutionContext,assetTypeDTO);
                        assetType.Save();
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
            List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG, "Y"));
            }
            if (!string.IsNullOrEmpty(txtName.Text))
            {
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, txtName.Text));
            }
            assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            AssetTypeList assetTypeList = new AssetTypeList(utilities.ExecutionContext);
            List<AssetTypeDTO> assetTypeDTOList = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
            BindingSource assetTypeDTOListBS = new BindingSource();
            if (assetTypeDTOList != null)
            {
                SortableBindingList<AssetTypeDTO> assetTypeDTOSortList = new SortableBindingList<AssetTypeDTO>(assetTypeDTOList);
                assetTypeDTOListBS.DataSource = assetTypeDTOSortList;
            }
            else
            {
                assetTypeDTOListBS.DataSource = new SortableBindingList<AssetTypeDTO>();
            }
            assetTypeDataGridView.DataSource = assetTypeDTOListBS;
            log.LogMethodExit();
        }

        private void assetTypeDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + assetTypeDataGridView.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void AssetTypeUI_Load(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            btnPublishToSite.Size = new System.Drawing.Size(126, 23);
            log.LogMethodExit();
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature 

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            try
            {
                if (assetTypeDataGridView.SelectedRows != null && assetTypeDataGridView.SelectedRows.Count > 0)
                {
                    int typeIdSelected = -1;
                    if (assetTypeDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(assetTypeDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out typeIdSelected);
                    }
                    if (typeIdSelected > 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, typeIdSelected, "AssetType", assetTypeDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    }
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Asset Type Publish");
                log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
            }
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature
    }
}
