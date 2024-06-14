/********************************************************************************************
 * Project Name - Inventory
 * Description  - Location UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha          Added logger methods.
 *2.70.2        23-Dec-2019      Girish Kundar    Modified : btnSave_Click() method for restricting user to create location of type Wastage 
 *2.80          01-Apr-2020      jinto Thomas     Modified : PopulateLocation() method for load location list according to search filter
 *                                                added PopulateLocationSearchType() method and btnSearch_Click() 
 *2.110.0       11-Jan-2020      Girish Kundar    Modified : Location UU add fix
 *2.110.0       05-Feb-2020      Mushahid Faizan  Modified : Location UI add/Update fix
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.BarcodeUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;


namespace Semnox.Parafait.Inventory
{
    public partial class LocationUI : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        BindingSource locationListBS;
        //BindingSource locationTypeListBS;
        //BindingSource machineListBS;

        public LocationUI()
        { }

        public LocationUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                utilities.setLanguage(this);
                utilities.setupDataGridProperties(ref dgvLocation);
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkPublishToSite.Visible = true;
                }
                else
                {
                    lnkPublishToSite.Visible = false;
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                MessageBox.Show(ex.Message);
            }
        }

        private void LocationUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateLocationType();
            PopulateLocationSearchType();
            PopulateLocation();
            log.LogMethodExit();
        }

        private void PopulateLocation()
        {
            log.LogMethodEntry();
            try
            {
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> inventoryLocationSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                if (cbLocationType.SelectedValue != null && Convert.ToInt32(cbLocationType.SelectedValue) != -1)
                {
                    inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_TYPE_ID, cbLocationType.SelectedValue.ToString()));
                }
                if(txtLocationName.Text != string.Empty)
                {
                    inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.LOCATION_NAME, txtLocationName.Text));
                }
                if (cbxActive.Checked)
                {
                    inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "1"));
                }
                LocationList inventoryLocationList = new LocationList(machineUserContext);
               
                
                inventoryLocationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LocationDTO> inventoryLocationListOnDisplay = inventoryLocationList.GetAllLocations(inventoryLocationSearchParams);

                locationListBS = new BindingSource();
                if (inventoryLocationListOnDisplay != null)
                {
                    SortableBindingList<LocationDTO> inventoryLocationDTOSortList = new SortableBindingList<LocationDTO>(inventoryLocationListOnDisplay);
                    locationListBS.DataSource = inventoryLocationDTOSortList;
                }
                else
                {
                    locationListBS.DataSource = new SortableBindingList<LocationDTO>();
                }

                locationListBS.AddingNew += dgvLocation_BindingSourceAddNew;
                dgvLocation.DataSource = locationListBS;
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvLocation_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvLocation.Rows.Count == locationListBS.Count)
                {
                    locationListBS.RemoveAt(locationListBS.Count - 1);
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void PopulateLocationType()
        {
            log.LogMethodEntry();
            try
            {
                LocationTypeList locationTypeList = new LocationTypeList(machineUserContext);
                List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> inventoryLocationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "1"));
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LocationTypeDTO> inventoryLocationsListOnDisplay = locationTypeList.GetAllLocationType(inventoryLocationTypeSearchParams);

                BindingSource bindingSourceLocationTypes = new BindingSource();
                if (inventoryLocationsListOnDisplay != null)
                {
                    inventoryLocationsListOnDisplay.Insert(0, new LocationTypeDTO());
                    SortableBindingList<LocationTypeDTO> inventoryDocumentTypeDTOSortList = new SortableBindingList<LocationTypeDTO>(inventoryLocationsListOnDisplay);
                    bindingSourceLocationTypes.DataSource = inventoryDocumentTypeDTOSortList.OrderBy(loc => loc.LocationType).ToList(); 
                }
                else
                {
                    bindingSourceLocationTypes.DataSource = new SortableBindingList<LocationTypeDTO>();
                }
                locationTypeIdDataGridViewTextBoxColumn.DataSource = bindingSourceLocationTypes;
                locationTypeIdDataGridViewTextBoxColumn.ValueMember = "LocationTypeId";
                locationTypeIdDataGridViewTextBoxColumn.DisplayMember = "LocationType";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int isStoreLocationID = -1;
                InventoryList inventoryList = new InventoryList();
                List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                LocationList locationList = new LocationList(machineUserContext);
                List<LocationDTO> locationDTOList = new List<LocationDTO>();
                List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> locationSearchParams = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.ISSTORE, "Y"));
                locationSearchParams.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "Y"));
                locationDTOList = locationList.GetAllLocations(locationSearchParams);
                if (locationDTOList != null && locationDTOList.Any())
                    isStoreLocationID = locationDTOList[0].LocationId;

                BindingSource locationListBS = (BindingSource)dgvLocation.DataSource;
                var locationListOnDisplay = (SortableBindingList<LocationDTO>)locationListBS.DataSource;
                List<LocationDTO> tempList = new List<LocationDTO>(locationListOnDisplay);
                if (tempList != null && tempList.Count > 0)
                {
                    var query = tempList.GroupBy(x => new { x.Name ,x.LocationTypeId})
                   .Where(g => g.Count() > 1)
                   .Select(y => y.Key)
                   .ToList();
                    if (query.Count > 0)
                    {
                        log.Debug("Duplicate entries detail : " + query[0]);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "location"), "Validation Error");
                        return;
                    }
                }
                if (locationListOnDisplay.Count > 0)
                {
                    foreach (LocationDTO locationDTO in locationListOnDisplay)
                    {
                        if (locationDTO.IsChanged)
                        {
                            if (locationDTO.LocationId > -1 && locationDTO.IsActive.Equals("N"))
                            {
                                inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationDTO.LocationId.ToString()));
                                inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);
                                if (inventoryDTOList != null && inventoryDTOList.Exists(x => (x.Quantity > 0)))
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1536, locationDTO.Name));
                                    return;
                                }
                            }
                            string locationType = "";
                            if (isStoreLocationID != -1 && locationDTO.LocationId != isStoreLocationID && locationDTO.IsStore == "Y")
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("Store already exists. There cannot be more than one store in a site."));
                                PopulateLocation();
                                return;
                            }
                            if (string.IsNullOrEmpty(locationDTO.Name) || string.IsNullOrWhiteSpace(locationDTO.Name))
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "location"), "Validation Error");
                                return;
                            }
                            if (locationDTO.LocationTypeId == -1)
                            {
                                MessageBox.Show("Please select Location type.");
                                return;
                            }
                            LocationType LocationType = new LocationType(machineUserContext, locationDTO.LocationTypeId);
                            LocationTypeDTO locationTypeDTO = LocationType.LocationTypeDTO;
                            locationType = locationTypeDTO.LocationType;
                            if (locationType == "Receive" || locationType == "Purchase" || locationType == "Adjustment" || locationType == "Wastage")
                            {
                                MessageBox.Show("Location of type " + locationType + " already exists.");
                                log.LogMethodExit();
                                return;
                            }
                            //To ensures that No one should add a location called 'Wastage' of type Wastage.
                            LocationDTO wastageLocationDTO = locationList.GetWastageLocationDTO();
                            if (locationDTO.Name.Equals(wastageLocationDTO.Name, StringComparison.CurrentCultureIgnoreCase) 
                                && locationType == "Wastage")
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2454), MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation Error"));
                                log.LogMethodExit();
                                return;
                            }
                            LocationBL Location = new LocationBL(machineUserContext, locationDTO);
                            Location.Save();
                        }
                    }
                    PopulateLocation();
                }
                else
                {
                    MessageBox.Show("Nothing to save");
                    log.Debug("Nothing to save");
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message);
                log.LogMethodExit();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateLocationType();
            PopulateLocationSearchType();
            cbxActive.Checked = true;
            txtLocationName.Text = string.Empty;
            PopulateLocation();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            InventoryList inventoryList = new InventoryList();
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            try
            {
                if (this.dgvLocation.SelectedRows.Count <= 0 && this.dgvLocation.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvLocation.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvLocation.SelectedCells)
                    {
                        dgvLocation.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow locationRow in this.dgvLocation.SelectedRows)
                {
                    if (locationRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(locationRow.Cells[0].Value) < 0)
                        {
                            dgvLocation.Rows.RemoveAt(locationRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource locationDTOListDTOBS = (BindingSource)dgvLocation.DataSource;
                                var locationDTOList = (SortableBindingList<LocationDTO>)locationDTOListDTOBS.DataSource;
                                LocationDTO locationDTO = locationDTOList[locationRow.Index];

                                inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationDTO.LocationId.ToString()));
                                inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams);
                                if (inventoryDTOList == null || (inventoryDTOList != null && !inventoryDTOList.Exists(x => (x.Quantity > 0))))
                                {
                                    locationDTO.IsActive = false;
                                    LocationBL location = new LocationBL(machineUserContext, locationDTO);
                                    location.Save();
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1536, locationDTO.Name));
                                }
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateLocation();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnDelete_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Dispose();
            log.LogMethodExit();
        }

        private void btnImportMachines_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ImportMachines f = new ImportMachines(utilities);
                f.StartPosition = FormStartPosition.CenterScreen;//Added for showing at center on 23-Sep-2016
                f.ShowDialog();
                PopulateLocation();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnImportMachines_LinkClicked() event with exception: " + ex.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvLocation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                int locationId = Convert.ToInt32(dgvLocation.CurrentRow.Cells["locationIdDataGridViewTextBoxColumn"].Value);

                if (dgvLocation.Columns[e.ColumnIndex].Name == "GenerateBarcode" && e.RowIndex > -1)
                {
                    if (locationId > -1)
                    {
                        int currentIndex = dgvLocation.CurrentRow.Index;
                        frm_barcode f = new frm_barcode(dgvLocation.CurrentRow.Cells["barcodeDataGridViewTextBoxColumn"].Value.ToString() == "" ? dgvLocation.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString() : dgvLocation.CurrentRow.Cells["barcodeDataGridViewTextBoxColumn"].Value.ToString(), utilities);
                        f.StartPosition = FormStartPosition.CenterScreen;//Added for showing at center on 23-Sep-2016
                        if (f.ShowDialog() == DialogResult.OK)
                            dgvLocation.Rows[currentIndex].Cells["barcodeDataGridViewTextBoxColumn"].Value = BarcodeReader.Barcode;
                    }
                }
                if (dgvLocation.Columns[e.ColumnIndex].Name == "Custom" && e.RowIndex > -1)
                {
                    if (locationId > -1)
                    {
                        int currentIndex = dgvLocation.CurrentRow.Index;
                        CustomDataUI f = new CustomDataUI("LOCATION", locationId, "Location", utilities);
                        f.StartPosition = FormStartPosition.CenterScreen;//Added for showing at center on 23-Sep-2016
                        f.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing dgvLocation_CellContentClick()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvLocation_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvLocation.CurrentRow == null)
                return;
            if (dgvLocation.CurrentRow.Cells["locationIdDataGridViewTextBoxColumn"].Value != DBNull.Value && dgvLocation.CurrentRow.Cells["locationIdDataGridViewTextBoxColumn"].Value != null)
            {
                string locationType = dgvLocation.CurrentRow.Cells["locationTypeIdDataGridViewTextBoxColumn"].FormattedValue.ToString();
                if (locationType == "Receive" || locationType == "Purchase" || locationType == "Adjustment")
                {
                    dgvLocation.CurrentRow.ReadOnly = true;
                }
                else
                {
                    dgvLocation.CurrentRow.ReadOnly = false;
                }
            }
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            Publish.PublishUI publishUI;
            if (dgvLocation.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvLocation.SelectedCells)
                {
                    dgvLocation.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvLocation.SelectedRows.Count > 0)
            {
                if (dgvLocation.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new Publish.PublishUI(utilities, Convert.ToInt32(dgvLocation.SelectedRows[0].Cells["locationIdDataGridViewTextBoxColumn"].Value), "Location", dgvLocation.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void PopulateLocationSearchType()
        {
            log.LogMethodEntry();
            try
            {
                LocationTypeList locationTypeList = new LocationTypeList(machineUserContext);
                List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> inventoryLocationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.IS_ACTIVE, "1"));
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LocationTypeDTO> locationTypeDTOList = locationTypeList.GetAllLocationType(inventoryLocationTypeSearchParams);

                BindingSource bsLocationTypeSearchList = new BindingSource();
                
                if (locationTypeDTOList != null)
                {
                    locationTypeDTOList.Insert(0, new LocationTypeDTO());
                    SortableBindingList<LocationTypeDTO> locationTypeSortList = new SortableBindingList<LocationTypeDTO>(locationTypeDTOList);
                    locationTypeSortList[0].LocationType = " All";
                    bsLocationTypeSearchList.DataSource = locationTypeSortList.OrderBy(loc => loc.LocationType).ToList(); ;
                }
                else
                {
                    bsLocationTypeSearchList.DataSource = new SortableBindingList<LocationTypeDTO>();
                }
                
                cbLocationType.DataSource = bsLocationTypeSearchList;
                cbLocationType.ValueMember = "LocationTypeId";
                cbLocationType.DisplayMember = "LocationType";
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateLocation();
            log.LogMethodExit();
        }
    }
}
