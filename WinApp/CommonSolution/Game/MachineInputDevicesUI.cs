/********************************************************************************************
 * Project Name - Parafait Machine Input Devices 
 * Description  - MachineInputDevicesUI form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        04-May-2019      Guru S A       Make game module as readonly in Windows Management Studio 
 *2.70.2        12-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Game
{
    public partial class MachineInputDevicesUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();

        private const string MACHINE_INPUT_DEVICE_TYPE = "MACHINE_INPUT_DEVICE_TYPE";
        private const string MACHINE_INPUT_DEVICE_MODEL = "MACHINE_INPUT_DEVICE_MODEL";
        private const string FP_TEMPLATE_FORMAT = "FP_TEMPLATE_FORMAT";

        BindingSource machineInputDevicesBS = new BindingSource();
        List<MachineInputDevicesDTO> machineInputDevicesDTOList;
        MachineDTO machineDTO = null;
        private ManagementStudioSwitch managementStudioSwitch;
        public MachineInputDevicesUI(Utilities _Utilities, int machineId)
        {

            log.LogMethodEntry(_Utilities, machineId);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgMachineInputDevices);

            machineUserContext.SetSiteId(utilities.ParafaitEnv.IsCorporate == true ? utilities.ParafaitEnv.SiteId : -1);
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);

            utilities.setLanguage(this);

            machineDTO = LoadMachines(machineId);
            if (machineDTO == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1199));
                dgMachineInputDevices.DataSource = null;
                log.LogMethodExit();
                return;
            }
            else
            {
                this.Text = machineDTO.MachineName;
                LoadMachineInputGrid();
                LoadDeviceType();
                LoadDeviceModel();
                LoadFPTemplateFormat();
            }
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void LoadDeviceType()
        {
            try
            {
                log.LogMethodEntry();
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName(MACHINE_INPUT_DEVICE_TYPE, machineUserContext.GetSiteId());

                if (LookupValuesDTOList == null)
                {
                    LookupValuesDTOList = new List<LookupValuesDTO>();
                }
                LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
                lookupValuesDTO.LookupValue = "--Select--";

                LookupValuesDTOList.Insert(0, lookupValuesDTO);
                drpDeviceTypeId.DataSource = LookupValuesDTOList;
                drpDeviceTypeId.ValueMember = "LookupValueId";
                drpDeviceTypeId.DisplayMember = "LookupValue";

            }
            catch (Exception ex)
            {
                log.Error("Error while excuting LoadDeviceType()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadDeviceModel()
        {
            try
            {
                log.LogMethodEntry();
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName(MACHINE_INPUT_DEVICE_MODEL, machineUserContext.GetSiteId());


                if (LookupValuesDTOList == null)
                {
                    LookupValuesDTOList = new List<LookupValuesDTO>();
                }
                LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
                lookupValuesDTO.LookupValue = "--Select--";

                LookupValuesDTOList.Insert(0, lookupValuesDTO);
                drpDeviceModelId.DataSource = LookupValuesDTOList;
                drpDeviceModelId.ValueMember = "LookupValueId";
                drpDeviceModelId.DisplayMember = "LookupValue";

            }
            catch (Exception ex)
            {
                log.Error("Error while executing LoadDeviceModel()" + ex.Message);
            }
            log.LogMethodExit();
        }


        private void LoadFPTemplateFormat()
        {
            try
            {
                log.LogMethodEntry();
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<LookupValuesDTO> LookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName(FP_TEMPLATE_FORMAT, machineUserContext.GetSiteId());

                if (LookupValuesDTOList == null)
                {
                    LookupValuesDTOList = new List<LookupValuesDTO>();
                }
                LookupValuesDTO lookupValuesDTO = new LookupValuesDTO();
                lookupValuesDTO.LookupValue = "--Select--";

                LookupValuesDTOList.Insert(0, lookupValuesDTO);
                drpFPTemplateFormat.DataSource = LookupValuesDTOList;
                drpFPTemplateFormat.ValueMember = "LookupValueId";
                drpFPTemplateFormat.DisplayMember = "LookupValue";

            }
            catch (Exception ex)
            {
                log.Error("Error while executing LoadFPTemplateFormat()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private MachineDTO LoadMachines(int machineId)
        {
            log.LogMethodEntry(machineId);
            MachineDTO machineDTO = new MachineDTO(); ;
            try
            {
                Machine machine = new Machine(machineId);
                machineDTO = machine.GetMachineDTO;

                log.LogMethodExit();

                if (machineDTO == null || machineDTO.MachineId <= 0)
                {
                    log.LogMethodExit();
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error("Error while executing LoadMachines()" + ex.Message);
            }
            log.LogMethodExit(machineDTO);
            return machineDTO;
        }


        private void LoadMachineInputGrid()
        {
            try
            {
                log.LogMethodEntry();
                MachineInputDevicesList machineInputDevicesList = new MachineInputDevicesList(machineUserContext);
                List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineInputDevicesDTO.SearchByParameters, string>(MachineInputDevicesDTO.SearchByParameters.MACHINE_ID, machineDTO.MachineId.ToString()));
                machineInputDevicesDTOList = machineInputDevicesList.GetAllMachineInputDevicesList(searchParameters);

                if (machineInputDevicesDTOList != null)
                {
                    SortableBindingList<MachineInputDevicesDTO> sortableMachineInputDevicesDTOList = new SortableBindingList<MachineInputDevicesDTO>(machineInputDevicesDTOList);
                    machineInputDevicesBS.DataSource = sortableMachineInputDevicesDTOList;
                }
                else
                    machineInputDevicesBS.DataSource = new SortableBindingList<MachineInputDevicesDTO>();

                machineInputDevicesBS.AddingNew += dgMachineInputDevices_BindingSourceAddNew;
                dgMachineInputDevices.DataSource = machineInputDevicesBS;
                dgMachineInputDevices.DataError += new DataGridViewDataErrorEventHandler(dgMachineInputDevices_ComboDataError);
            }
            catch (Exception ex)
            {
                log.Error("Error while executing LoadMachineInputGrid()" + ex.Message);
            }
            log.LogMethodExit();
        }
        void dgMachineInputDevices_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgMachineInputDevices.Rows.Count == machineInputDevicesBS.Count)
            {
                machineInputDevicesBS.RemoveAt(machineInputDevicesBS.Count - 1);
            }
            log.LogMethodExit();
        }

        void dgMachineInputDevices_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgMachineInputDevices.Columns["DeviceName"].Index)
            {
                if (machineInputDevicesDTOList != null)
                    dgMachineInputDevices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";// machineInputDevicesDTOList[0].DeviceName;
            }
            else if (e.ColumnIndex == dgMachineInputDevices.Columns["drpDeviceTypeId"].Index)
            {
                if (machineInputDevicesDTOList != null)
                    dgMachineInputDevices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0; // machineInputDevicesDTOList[0].DeviceTypeId;
            }
            else if (e.ColumnIndex == dgMachineInputDevices.Columns["drpDeviceModelId"].Index)
            {
                if (machineInputDevicesDTOList != null)
                    dgMachineInputDevices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;//machineInputDevicesDTOList[0].DeviceModelId;
            }
            else if (e.ColumnIndex == dgMachineInputDevices.Columns["drpFPTemplateFormat"].Index)
            {
                if (machineInputDevicesDTOList != null)
                    dgMachineInputDevices.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 0;// machineInputDevicesDTOList[0].FPTemplateFormat;
            }          
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                MachineInputDevices machineInputDevices;

                BindingSource machineInputDevicesListBS = (BindingSource)dgMachineInputDevices.DataSource;
                var machineInputDevicesListOnDisaply = (SortableBindingList<MachineInputDevicesDTO>)machineInputDevicesListBS.DataSource;
                foreach (MachineInputDevicesDTO machineInputDevicesDTO in machineInputDevicesListOnDisaply)
                {

                    machineInputDevicesDTO.MachineId = machineDTO.MachineId;
                    if (string.IsNullOrEmpty(machineInputDevicesDTO.DeviceName.Trim()))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1200));
                        log.LogMethodExit();
                        return;
                    }
                    if (machineInputDevicesDTO.DeviceTypeId <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1201));
                        log.LogMethodExit();
                        return;
                    }
                    if (machineInputDevicesDTO.DeviceModelId <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1202));
                        log.LogMethodExit();
                        return;
                    }
                    if (machineInputDevicesDTO.IPAddress.Any(x => char.IsLetter(x)))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1203));
                        log.LogMethodExit();
                        return;
                    }
                    //if (machineInputDevicesDTO.FPTemplateFormat>0)
                    //{
                    //    MessageBox.Show("Enter FPTemplateFormat");
                    //    return;
                    //}
                    //if (machineInputDevicesDTO.MachineId <= 0)
                    //{
                    //    MessageBox.Show("Enter Mahine Name");
                    //    //    MessageBox.Show(utilities.MessageUtils.getMessage(1181));
                    //    return;
                    //}

                    machineInputDevices = new MachineInputDevices(machineInputDevicesDTO,machineUserContext);
                    machineInputDevices.Save();
                }
                LoadMachineInputGrid();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadMachineInputGrid();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgMachineInputDevices_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + dgMachineInputDevices.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
            return;
        }


        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableGameModule)
            {
                dgMachineInputDevices.AllowUserToAddRows = true;
                dgMachineInputDevices.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true; 
            }
            else
            {
                dgMachineInputDevices.AllowUserToAddRows = false;
                dgMachineInputDevices.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false; 
            }
            log.LogMethodExit();
        }
    }
}
