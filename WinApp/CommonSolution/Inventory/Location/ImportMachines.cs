/********************************************************************************************
 * Project Name - Inventory
 * Description  - Import Machine UI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Semnox.Parafait.Inventory
{
    public partial class ImportMachines : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;

        public ImportMachines(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);

            log.LogMethodExit();
        }

        private void ImportMachines_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateLocationType();
            PopulateMachines();
            chkImportAllMachines.Checked = true;
            log.LogMethodExit();
        }

        private void PopulateLocationType()
        {
            log.LogMethodEntry();
            try
            {
                LocationTypeList locationTypeList = new LocationTypeList(machineUserContext);
                List<LocationTypeDTO> locationTypeDTOList = new List<LocationTypeDTO>();
                List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>> inventoryLocationTypeSearchParams = new List<KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>>();
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                inventoryLocationTypeSearchParams.Add(new KeyValuePair<LocationTypeDTO.SearchByLocationTypeParameters, string>(LocationTypeDTO.SearchByLocationTypeParameters.LOCATION_TYPE, "Department,Store"));
                locationTypeDTOList = locationTypeList.GetAllLocationType(inventoryLocationTypeSearchParams);
                //locationTypeDTOList = locationTypeList.GetLocationTypeListOnType("'Department','Store'", machineUserContext.GetSiteId());

                cmbLocationType.DataSource = locationTypeDTOList;
                cmbLocationType.DisplayMember = "LocationType";
                cmbLocationType.ValueMember = "LocationTypeId";
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateMachines()
        {
            log.LogMethodEntry();

            MachineList machineList = new MachineList();
            List<MachineDTO> machineListOnDisplay = machineList.GetNonInventoryLocationMachines(machineUserContext.GetSiteId());
            BindingSource machineListBS = new BindingSource();

            if (machineListOnDisplay != null)
            {
                SortableBindingList<MachineDTO> machineDTOSortList = new SortableBindingList<MachineDTO>(machineListOnDisplay);
                machineListBS.DataSource = machineDTOSortList;
                dgvMachines.DataSource = machineListBS;
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1126), utilities.MessageUtils.getMessage(1128));
                return;
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<int> selectedMachines = getSelectedMachines();
            if (selectedMachines != null)
            {
                BindingSource machineListBS = (BindingSource)dgvMachines.DataSource;
                var machineListOnDisplay = (SortableBindingList<MachineDTO>)machineListBS.DataSource;
                if (machineListOnDisplay != null)
                {
                    if (machineListOnDisplay.Count > 0)
                    {
                        foreach (MachineDTO machineDTO in machineListOnDisplay)
                        {
                            for (int j = 0; j < selectedMachines.Count; j++)
                            {
                                if (selectedMachines[j] == machineDTO.MachineId)
                                {
                                    LocationDTO locationDTO = new LocationDTO();
                                    locationDTO.Name = machineDTO.MachineName;
                                    //locationDTO.MachineId = machineDTO.MachineId;
                                    locationDTO.LocationTypeId = Convert.ToInt32(cmbLocationType.SelectedValue);
                                    locationDTO.IsActive = true;
                                    locationDTO.IsAvailableToSell = "N";
                                    locationDTO.IsTurnInLocation = "N";
                                    locationDTO.IsStore = "N";
                                    locationDTO.MassUpdatedAllowed = "Y";
                                    locationDTO.RemarksMandatory = "N";
                                    locationDTO.Barcode = "";
                                    locationDTO.ExternalSystemReference = "";
                                    locationDTO.CustomDataSetId = -1;
                                    locationDTO.SiteId = machineUserContext.GetSiteId();
                                    LocationBL Location = new LocationBL(machineUserContext, locationDTO);
                                    Location.Save();

                                    int LocationId = locationDTO.LocationId;
                                    machineDTO.InventoryLocationId = LocationId;
                                    Machine machine = new Machine(machineDTO);
                                    machine.Save();
                                    break;
                                }
                            }
                        }
                    }
                    PopulateMachines();
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1127), utilities.MessageUtils.getMessage(1128));
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Dispose();
            log.LogMethodExit();
        }

        private void chkImportAllMachines_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (chkImportAllMachines.Checked)
            {
                selectAllMachines();
            }
            else
            {
                unSelectAllMachines();
            }
            log.LogMethodExit();
        }

        private void selectAllMachines()
        {
            log.LogMethodEntry();
            for (int i = 0; i < dgvMachines.Rows.Count; i++)
            {
                if (dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != null && dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    DataGridViewRow row = dgvMachines.Rows[i];
                    ((DataGridViewCheckBoxCell)row.Cells["SelectMachine"]).Value = true;
                }
            }
            log.LogMethodExit();
        }

        private void unSelectAllMachines()
        {
            log.LogMethodEntry();
            for (int i = 0; i < dgvMachines.Rows.Count - 1; i++)
            {
                if (dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != null && dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    DataGridViewRow row = dgvMachines.Rows[i];
                    ((DataGridViewCheckBoxCell)row.Cells["SelectMachine"]).Value = false;
                }
            }
            log.LogMethodExit();
        }

        private List<int> getSelectedMachines()
        {
            log.LogMethodEntry();
            int machineID;
            List<int> selectedMachines = new List<int>();

            for (int i = 0; i < dgvMachines.Rows.Count; i++)
            {
                if (dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != null && dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value != DBNull.Value)
                {
                    bool isMachineSelected = Convert.ToBoolean(dgvMachines.Rows[i].Cells["SelectMachine"].Value);
                    if (isMachineSelected == true)
                    {
                        machineID = Convert.ToInt32(dgvMachines.Rows[i].Cells["machineIdDataGridViewTextBoxColumn"].Value);
                        selectedMachines.Add(machineID);
                    }
                }
            }
            log.LogMethodExit(selectedMachines);
            return selectedMachines;
        }
    }
}
