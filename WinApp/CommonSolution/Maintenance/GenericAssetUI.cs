/********************************************************************************************
 * Project Name - Generic Asset UI
 * Description  - User interface for generic asset
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jan-2016   Raghuveera          Created 
 *********************************************************************************************
 *1.00        22-Feb-2016   Suneetha.S          Modified
 *2.60        05-May-2019   Mehraj              Modified isActive column
 *2.70.2      12-Aug-2019   Deeksha             Modified logger methods.
 *2.70.3      02-Apr-2020   Girish Kundar       Modified: Do not allow duplicate  name
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/


using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// Generic Asset UI
    /// </summary>
    public partial class GenericAssetUI : Form
    {
        AssetTypeList assetTypeList;
        List<AssetTypeDTO> assetTypeListOnDisplay;
        TaxList tax;
        List<TaxDTO> taxListOnDisplay;
        BindingSource assetListBS;
        List<GenericAssetDTO> assetList;
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<LookupValuesDTO> lookupValuesDTOList;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        bool loadSearch = true;
        /// <summary>
        /// Parameterized constructor of GenericAssetUI
        /// </summary>
        /// <param name="_Utilities">Utilities object as parameter</param>
        /// <param name="IsRestricted">Is optional argument to ristrict the fields in grid</param>
        public GenericAssetUI(Utilities _Utilities, bool IsRestricted = false)
        {
            log.LogMethodEntry(_Utilities, IsRestricted);
            InitializeComponent();
            RegisterKeyDownHandlers(this);//Modification on 22-Feb-2016 to hide the search grid control

            utilities = _Utilities;
            utilities.setupDataGridProperties(ref genericAssetDataGridView);

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
            LoadAssetType();

            Loadsearch();
            tax = new TaxList(machineUserContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();//Starts:Modification on 18-Jul-2016 for publish feature
            searchByTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            taxListOnDisplay = tax.GetAllTaxes(searchByTaxParameters);//Ends:Modification on 18-Jul-2016 for publish feature
            if (taxListOnDisplay == null)
            {
                taxListOnDisplay = new List<TaxDTO>();
            }
            taxListOnDisplay.Insert(0, new TaxDTO());
            taxListOnDisplay[0].TaxName = utilities.MessageUtils.getMessage("None");
            assetTaxTypeIdDataGridViewComboBoxColumn.DataSource = taxListOnDisplay;
            assetTaxTypeIdDataGridViewComboBoxColumn.ValueMember = "TaxId";
            assetTaxTypeIdDataGridViewComboBoxColumn.ValueType = typeof(Int32);
            assetTaxTypeIdDataGridViewComboBoxColumn.DisplayMember = "TaxName";

            AssetLocation assetLocation = new AssetLocation();
            List<AssetLocationDTO> assetLocationListOnDisplay = assetLocation.GetLocation();
            if (assetLocationListOnDisplay == null)
            {
                assetLocationListOnDisplay = new List<AssetLocationDTO>();
            }
            assetLocationListOnDisplay.Insert(0, new AssetLocationDTO());
            cmbLocation.DataSource = assetLocationListOnDisplay;
            cmbLocation.DisplayMember = "Location";
            cmbLocation.ValueMember = "Location";

            LoadStatus();
            PopulateAssetGrid();
            if (IsRestricted)
            {
                purchaseDateDataGridViewTextBoxColumn.Visible =
                saleDateDataGridViewTextBoxColumn.Visible =
                scrapDateDataGridViewTextBoxColumn.Visible =
                assetTaxTypeIdDataGridViewComboBoxColumn.Visible =
                purchaseValueDataGridViewTextBoxColumn.Visible =
                saleValueDataGridViewTextBoxColumn.Visible =
                scrapValueDataGridViewTextBoxColumn.Visible = !IsRestricted;
            }
            cmbAssetType.Text = "";
            log.LogMethodExit();
        }
        /// <summary>
        /// Loading asset type
        /// </summary>
        private void LoadAssetType()
        {
            log.LogMethodEntry();
            assetTypeList = new AssetTypeList(utilities.ExecutionContext);
            List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
            assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            assetTypeListOnDisplay = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
            if (assetTypeListOnDisplay == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(960));
                assetTypeListOnDisplay = new List<AssetTypeDTO>();
            }
            assetTypeListOnDisplay.Insert(0, new AssetTypeDTO());
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = assetTypeListOnDisplay;

            cmbAssetType.DataSource = bindingSource;
            cmbAssetType.ValueMember = "AssetTypeId";
            cmbAssetType.DisplayMember = "Name";
            assetTypeIdDataGridViewComboBoxColumn.DataSource = assetTypeListOnDisplay;
            assetTypeIdDataGridViewComboBoxColumn.ValueMember = "AssetTypeId";
            assetTypeIdDataGridViewComboBoxColumn.ValueType = typeof(Int32);
            assetTypeIdDataGridViewComboBoxColumn.DisplayMember = "Name";
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>
        private void LoadStatus()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MAINT_ASSET_STATUS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValueId = -1;
                //lookupValuesDTOList[0].Description = utilities.MessageUtils.getMessage("");
                bindingSource.DataSource = lookupValuesDTOList;
                assetStatusDataGridViewTextBoxColumn.DataSource = lookupValuesDTOList;
                assetStatusDataGridViewTextBoxColumn.ValueMember = "Description";
                assetStatusDataGridViewTextBoxColumn.DisplayMember = "Description";
                cmbStatus.DataSource = bindingSource;
                cmbStatus.ValueMember = "LookupValueId";
                cmbStatus.DisplayMember = "Description";

                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadStatus() Method with an Exception:", e);
            }
        }
        /// <summary>
        /// Loads the asset records to the grid
        /// </summary>
        private void PopulateAssetGrid()
        {
            log.LogMethodEntry();
            AssetList assetList = new AssetList(machineUserContext);
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : null));
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<GenericAssetDTO> assetListOnDisplay = assetList.GetAllAssets(assetSearchParams);
            assetListBS = new BindingSource();
            if (assetListOnDisplay != null)
            {
                this.assetList = assetListOnDisplay;
                SortableBindingList<GenericAssetDTO> genericAssetDTOSortList = new SortableBindingList<GenericAssetDTO>(assetListOnDisplay);
                assetListBS.DataSource = genericAssetDTOSortList;
            }
            else
                assetListBS.DataSource = new SortableBindingList<GenericAssetDTO>();
            assetListBS.AddingNew += genericAssetDataGridView_BindingSourceAddNew;
            genericAssetDataGridView.DataSource = assetListBS;
            genericAssetDataGridView.DataError += new DataGridViewDataErrorEventHandler(genericAssetDataGridView_ComboDataError);
            loadAssetSearch();
            log.LogMethodExit();
        }

        private void genericAssetDataGridView_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if (genericAssetDataGridView.Rows.Count == assetListBS.Count)
            {
                assetListBS.RemoveAt(assetListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        private void genericAssetDataGridView_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex == genericAssetDataGridView.Columns["assetTypeIdDataGridViewComboBoxColumn"].Index)
            {
                if (assetTypeListOnDisplay != null)
                    genericAssetDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = assetTypeListOnDisplay[0].AssetTypeId;
            }
            else if (e.ColumnIndex == genericAssetDataGridView.Columns["assetTaxTypeIdDataGridViewComboBoxColumn"].Index)
            {
                if (taxListOnDisplay != null)
                    genericAssetDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = taxListOnDisplay[0].TaxId;
            }
            else if (e.ColumnIndex == genericAssetDataGridView.Columns["assetStatusDataGridViewTextBoxColumn"].Index)
            {
                if (lookupValuesDTOList != null)
                    genericAssetDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValueId;
            }

            log.LogMethodExit();
        }

        private void assetCloseBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void assetRefreshBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbAssetType.SelectedValue = -1;
            txtName.Text = "";
            txtURN.Text = "";
            cmbStatus.SelectedValue = -1;
            cmbLocation.SelectedValue = -1;
            chbShowActiveEntries.Checked = true;
            PopulateAssetGrid();
            log.LogMethodExit();
        }

        private void assetSaveBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindingSource assetListBS = (BindingSource)genericAssetDataGridView.DataSource;
            var assetListOnDisplay = (SortableBindingList<GenericAssetDTO>)assetListBS.DataSource;
            List<GenericAssetDTO> tempList = new List<GenericAssetDTO>(assetListOnDisplay);
            if (tempList != null && tempList.Count > 0)
            {
                var query = tempList.GroupBy(x => new { x.Name ,x.AssetTypeId })
               .Where(g => g.Count() > 1)
               .Select(y => y.Key)
               .ToList();
                if (query.Count > 0)
                {
                    log.Debug("Duplicate entries detail : " + query[0]);
                    MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "Asset type / asset"), "Validation Error");
                    return;
                }
            }
            if (assetListOnDisplay.Count > 0)
            {
                foreach (GenericAssetDTO assetDTO in assetListOnDisplay)
                {
                    if (assetDTO.IsChanged)
                    {
                        if (string.IsNullOrEmpty(assetDTO.Name.Trim()))
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(961), "Validation Error");
                            log.LogMethodExit();
                            return;
                        }
                        if (assetDTO.AssetTypeId == -1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(962));
                            log.LogMethodExit();
                            return;
                        }
                        if (!string.IsNullOrEmpty(assetDTO.PurchaseDate))
                        {
                            try
                            {
                                DateTime.Parse(assetDTO.PurchaseDate);
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error while executing Parse PurchaseDate " + ex.Message);
                                MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("PurchaseDate"));
                                log.LogMethodExit();
                                return;
                            }


                        }
                        if (!string.IsNullOrEmpty(assetDTO.SaleDate))
                        {
                            try
                            {
                                DateTime.Parse(assetDTO.SaleDate);
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error while executing Parse SaleDate " + ex.Message);
                                MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("SaleDate"));
                                log.LogMethodExit();
                                return;
                            }
                        }
                        if (!string.IsNullOrEmpty(assetDTO.ScrapDate))
                        {
                            try
                            {
                                DateTime.Parse(assetDTO.ScrapDate);
                            }
                            catch(Exception ex)
                            {
                                log.Error("Error while executing Parse Scrapdata " + ex.Message);
                                MessageBox.Show(utilities.MessageUtils.getMessage(15) + " : " + utilities.MessageUtils.getMessage("ScrapDate"));
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }
                    GenericAsset asset = new GenericAsset(machineUserContext, assetDTO);
                    asset.Save();
                }
                btnSearch.PerformClick();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void assetDeleteBtn_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (this.genericAssetDataGridView.SelectedRows.Count <= 0 && this.genericAssetDataGridView.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-assetDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.genericAssetDataGridView.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.genericAssetDataGridView.SelectedCells)
                {
                    genericAssetDataGridView.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow assetRow in this.genericAssetDataGridView.SelectedRows)
            {
                if (assetRow.Cells[0].Value == null)
                {
                    log.LogMethodExit();
                    return;
                }
                if (Convert.ToInt32(assetRow.Cells[0].Value.ToString()) <= 0)
                {
                    genericAssetDataGridView.Rows.RemoveAt(assetRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource genericAssetDTOListDTOBS = (BindingSource)genericAssetDataGridView.DataSource;
                        var genericAssetDTOList = (SortableBindingList<GenericAssetDTO>)genericAssetDTOListDTOBS.DataSource;
                        GenericAssetDTO genericAssetDTO = genericAssetDTOList[assetRow.Index];
                        genericAssetDTO.IsActive = false;
                        GenericAsset assetType = new GenericAsset(machineUserContext, genericAssetDTO);
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
            if (string.IsNullOrEmpty(txtAssetTypeSearch.Text))
            {
                if (assetTypeListOnDisplay != null)
                {
                    cmbAssetType.SelectedValue = -1;
                }
            }
            List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : "N"));
            if (string.IsNullOrEmpty(txtName.Text) == false)
            {
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_NAME, txtName.Text));
            }
            if (string.IsNullOrEmpty(cmbStatus.Text) == false)
            {
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_STATUS, cmbStatus.Text.ToString()));
            }
            if (string.IsNullOrEmpty(txtURN.Text) == false)
            {
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.URN, txtURN.Text));
            }
            if (!cmbAssetType.SelectedValue.ToString().Equals("-1"))
            {
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_TYPE_ID, (cmbAssetType.SelectedValue.ToString().Equals("-1")) ? "" : cmbAssetType.SelectedValue.ToString()));
            }
            if (cmbLocation.SelectedValue != null && !cmbLocation.SelectedValue.ToString().Equals("-1"))
            {
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.LOCATION, (cmbLocation.SelectedValue == null || cmbLocation.SelectedValue.ToString().Equals("-1")) ? "" : cmbLocation.SelectedValue.ToString()));
            }
            assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            AssetList assetList = new AssetList(machineUserContext);
            List<GenericAssetDTO> genericAssetDTOList = assetList.GetAllAssets(assetSearchParams);
            BindingSource genericAssetDTOListBS = new BindingSource();
            if (genericAssetDTOList != null)
            {
                genericAssetDTOListBS.DataSource = new SortableBindingList<GenericAssetDTO>(genericAssetDTOList);
            }
            else
            {
                genericAssetDTOListBS.DataSource = new SortableBindingList<GenericAssetDTO>();
            }
            genericAssetDataGridView.DataSource = genericAssetDTOListBS;
            purchaseDateDataGridViewTextBoxColumn.DefaultCellStyle =
            saleDateDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            log.LogMethodExit();
        }

        private void genericAssetDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ", " + utilities.MessageUtils.getMessage("Column") + " " + genericAssetDataGridView.Columns[e.ColumnIndex].DataPropertyName + ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void txtAssetTypeSearch_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (loadSearch)
            {
                if (txtAssetTypeSearch.Text.Length > 0)
                {
                    if (assetTypeListOnDisplay != null)
                    {
                        List<AssetTypeDTO> assetTypeDTOList = assetTypeListOnDisplay.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtAssetTypeSearch.Text.ToLower()))).ToList<AssetTypeDTO>();
                        if (assetTypeDTOList.Count > 0)
                        {
                            dgvAssetTypeSearch.Visible = true;
                            dgvAssetTypeSearch.DataSource = assetTypeDTOList;
                        }
                        else
                        {
                            dgvAssetTypeSearch.Visible = false;
                        }
                    }
                }
                else
                {
                    cmbAssetType.Text = "";
                    dgvAssetTypeSearch.Visible = false;
                }
            }
            else
            {
                loadSearch = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads Serached Records
        /// </summary>
        private void Loadsearch()
        {
            log.LogMethodEntry();

            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = assetTypeListOnDisplay;
            dgvAssetTypeSearch.DataSource = bindingSource;
            for (int i = 0; i < dgvAssetTypeSearch.Columns.Count; i++)
            {
                if (!dgvAssetTypeSearch.Columns[i].Name.Equals("Name"))
                {
                    dgvAssetTypeSearch.Columns[i].Visible = false;
                }
                else
                {
                    dgvAssetTypeSearch.Columns[i].Width = dgvAssetTypeSearch.Width;
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetTypeSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtAssetTypeSearch.Text = dgvAssetTypeSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                cmbAssetType.Text = txtAssetTypeSearch.Text;
                dgvAssetTypeSearch.Visible = false;
            }
            catch(Exception ex)
            {
                log.Error("Error while executing dgvAssetTypeSearch_CellClick()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void cmbAssetType_SelectedValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbAssetType.Text.Equals("Semnox.Parafait.Maintenance.AssetTypeDTO"))
            {
                txtAssetTypeSearch.Text = "";
            }
            else
            {
                txtAssetTypeSearch.Text = cmbAssetType.Text;
            }
            loadSearch = false;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the search grid for asset name option
        /// </summary>
        private void loadAssetSearch()
        {
            log.LogMethodEntry();
            //BindingSource assetListBindSouce = (BindingSource)genericAssetDataGridView.DataSource;
            //assetList = (List<GenericAssetDTO>)assetListBindSouce.DataSource;
            if (assetList != null)
            {
                if (assetList.Count > 0)
                {
                    dgvNameSearch.DataSource = assetList;
                }
                for (int i = 0; i < dgvNameSearch.Columns.Count; i++)
                {
                    if (!dgvNameSearch.Columns[i].Name.Equals("Name"))
                    {
                        dgvNameSearch.Columns[i].Visible = false;
                    }
                    else
                    {
                        dgvNameSearch.Columns[i].Width = dgvNameSearch.Width;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtName.Text.Length > 0)
            {
                if (assetList != null)
                {
                    List<GenericAssetDTO> assetDTOList = assetList.Where(x => (bool)((string.IsNullOrEmpty(x.Name) ? "" : x.Name.ToLower()).Contains(txtName.Text.ToLower()))).ToList<GenericAssetDTO>();
                    if (assetDTOList.Count > 0)
                    {
                        dgvNameSearch.Visible = true;
                        dgvNameSearch.DataSource = assetDTOList;
                    }
                    else
                    {
                        dgvNameSearch.Visible = false;
                    }
                }
            }
            else
            {
                dgvNameSearch.Visible = false;
            }
            log.LogMethodExit();
        }

        private void dgvNameSearch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                txtName.Text = dgvNameSearch.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                dgvNameSearch.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing dgvNameSearch_CellClick()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnImportMachine_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int counter;
                bool searchStatus = false;
                MachineList machineList = new MachineList();
                List<Semnox.Parafait.Game.MachineDTO> machineDTOList;
                List<KeyValuePair<Semnox.Parafait.Game.MachineDTO.SearchByMachineParameters, string>> searchByMachineParameters = new List<KeyValuePair<Semnox.Parafait.Game.MachineDTO.SearchByMachineParameters, string>>();
                machineDTOList = machineList.GetMachineList(searchByMachineParameters);

                AssetTypeDTO assetTypeDTO = new AssetTypeDTO();
                List<AssetTypeDTO> assetTypeDTOList = new List<AssetTypeDTO>();
                AssetTypeList assetTypeList = new AssetTypeList(utilities.ExecutionContext);
                List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, "Machine"));
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                assetTypeDTOList = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
                if (assetTypeDTOList == null)
                {
                    assetTypeDTO.Name = "Machine";
                    assetTypeDTO.IsActive = true;
                    AssetType assetType = new AssetType(utilities.ExecutionContext,assetTypeDTO);
                    assetType.Save();
                }
                else
                {
                    assetTypeDTO = assetTypeDTOList[0];
                }
                List<GenericAssetDTO> genericAssetDTOList;
                GenericAsset genericAsset;
                AssetList assetList = new AssetList(machineUserContext);
                List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>();
                assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                genericAssetDTOList = assetList.GetAllAssets(assetSearchParams);
                counter = 0;
                if (machineDTOList != null)
                {
                    foreach (Semnox.Parafait.Game.MachineDTO machineDTO in machineDTOList)
                    {
                        searchStatus = false;
                        if (genericAssetDTOList != null)
                        {
                            foreach (GenericAssetDTO assetDTO in genericAssetDTOList)
                            {
                                if (assetDTO.Machineid == machineDTO.MachineId)
                                {
                                    searchStatus = true;
                                    assetDTO.Name = machineDTO.MachineName;
                                    assetDTO.AssetTypeId = assetTypeDTO.AssetTypeId;
                                    assetDTO.IsActive = machineDTO.IsActive == "Y" ? true : false;
                                    genericAsset = new GenericAsset(machineUserContext, assetDTO);
                                    genericAsset.Save();
                                    counter++;
                                    continue;
                                }
                            }
                        }
                        if (!searchStatus)
                        {
                            GenericAssetDTO assetDTO = new GenericAssetDTO();
                            assetDTO.Name = machineDTO.MachineName;
                            assetDTO.AssetTypeId = assetTypeDTO.AssetTypeId;
                            assetDTO.IsActive = machineDTO.IsActive == "Y" ? true : false;
                            assetDTO.Machineid = machineDTO.MachineId;
                            genericAsset = new GenericAsset(machineUserContext, assetDTO);
                            genericAsset.Save();
                            counter++;
                        }
                    }
                    if (counter > 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(964));
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(965));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(966));
                log.Fatal("Ends-btnImportMachine_Click() event with exception:" + ex.Message + ex.StackTrace);
            }
            LoadAssetType();
            btnSearch.PerformClick();
            log.LogMethodExit();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string reportName = "Asset_" + utilities.ParafaitEnv.SiteName;
            utilities.ExportToExcel(genericAssetDataGridView, reportName + " " + System.DateTime.Now.ToString("dd-MMM-yyyy"), reportName);
            log.LogMethodExit();
        }

        private void GenericAssetUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnImportMachine.Size = new Size(127, 23);
            btnPublishToSite.Size = new Size(127, 23);//Modification on 14-Jul-2016 for adding publish to site feature
            btnExport.Size = new Size(127, 23);
            log.LogMethodExit();
        }

        private void txtAssetTypeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Down)
            {
                if (dgvAssetTypeSearch.Rows.Count > 0)
                {
                    dgvAssetTypeSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvAssetTypeSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtAssetTypeSearch.Text = dgvAssetTypeSearch.SelectedCells[0].Value.ToString();
                    cmbAssetType.Text = txtAssetTypeSearch.Text;
                    dgvAssetTypeSearch.Visible = false;
                    txtAssetTypeSearch.Focus();
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing dgvAssetTypeSearch_KeyDown()" + ex.Message);
                }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtAssetTypeSearch.Focus();
            }
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
            dgvAssetTypeSearch.Visible = dgvNameSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        private void GenericAssetUI_MouseClick(object sender, MouseEventArgs e)//Modification on 22-Feb-2016 to hide the search grid control
        {
            log.LogMethodEntry();
            dgvAssetTypeSearch.Visible = dgvNameSearch.Visible = false;
            loadSearch = false;
            log.LogMethodExit();
        }//Ends modification on 22-Feb-2016 to hide the search grid control

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Down)
            {
                if (dgvNameSearch.Rows.Count > 0)
                {
                    dgvNameSearch.Focus();
                }
            }
            log.LogMethodExit();
        }

        private void dgvNameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtName.Text = dgvAssetTypeSearch.SelectedCells[0].Value.ToString();
                    dgvNameSearch.Visible = false;
                    txtName.Focus();
                }
                catch (Exception ex)
                {
                    log.Error("Error while executing dgvNameSearch_KeyDown()" + ex.Message);
                }
            }
            else if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up)
            {
                txtName.Focus();
            }
            log.LogMethodExit();
        }

        private void btnPublishToSite_Click(object sender, EventArgs e)//Starts: Modification on 14-Jul-2016 for adding publish to site feature 
        {
            log.LogMethodEntry();
            try
            {
                if (genericAssetDataGridView.SelectedRows != null && genericAssetDataGridView.SelectedRows.Count > 0)
                {
                    int assetIdSelected = -1;
                    if (genericAssetDataGridView.SelectedRows[0].Cells[0].Value != null)
                    {
                        int.TryParse(genericAssetDataGridView.SelectedRows[0].Cells[0].Value.ToString(), out assetIdSelected);
                    }
                    if (assetIdSelected > 0)
                    {
                        Publish.PublishUI publishUI = new Publish.PublishUI(utilities, assetIdSelected, "Asset", genericAssetDataGridView.SelectedRows[0].Cells[1].Value.ToString());
                        publishUI.ShowDialog();
                    }
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Asset Publish");
                log.Fatal("Ends-btnPublishToSite_Click() Event with exception. Exception: " + ex.ToString());
            }
        }//Ends: Modification on 14-Jul-2016 for adding publish to site feature

    }
}
