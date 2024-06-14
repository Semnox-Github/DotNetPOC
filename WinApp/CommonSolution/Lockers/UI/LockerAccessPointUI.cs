/****************************************************************************************************
 * Project Name -  Locker Access  Point
 * Description  - UI of Locker Access  Point
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ****************************************************************************************************
 *2.70.2        18-Sep-2019   Dakshakh raj       Modified : Added logs
 ****************************************************************************************************/
using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// User interface for Locker Access Point UI
    /// </summary>
    public partial class LockerAccessPointUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<LookupValuesDTO> lookupValuesDTOList;
        /// <summary>
        /// LockerAccessPointUI constructor
        /// </summary>
        /// <param name="_Utilities">Parafait Utils</param>
        public LockerAccessPointUI(Utilities _Utilities)
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
            LoadGroupCode();
            PopulateAccessPointGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the Load Group Code to the comboboxes
        /// </summary>
        private void LoadGroupCode()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LOCKER_ZONE_CODE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                    lookupValuesDTOList.Add(new LookupValuesDTO());
                    lookupValuesDTOList[0].LookupValue = "0";
                }
                GroupCode.DataSource = lookupValuesDTOList;
                GroupCode.ValueMember = "LookupValue";
                GroupCode.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadGroupCode() Method with an Exception:", e);
            }
        }

        /// <summary>
        /// PopulateAccessPointGrid
        /// </summary>
        private void PopulateAccessPointGrid()
        {
            log.LogMethodEntry();
            LockerAccessPointList lockerAccessPointList = new LockerAccessPointList(machineUserContext);
            List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>> lockerAccessPointSearchParams = new List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>>();
            lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "1" : "0"));
            lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (!string.IsNullOrEmpty(txtName.Text))
                lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.NAME, txtName.Text));

            List<LockerAccessPointDTO> lockerAccessPointListOnDisplay = lockerAccessPointList.GetAllLockerAccessPoint(lockerAccessPointSearchParams);
            BindingSource lockerAccessPointListBS = new BindingSource();
            if (lockerAccessPointListOnDisplay != null && lockerAccessPointListOnDisplay.Any())
            {
                SortableBindingList<LockerAccessPointDTO> lockerAccessPointDTOSortList = new SortableBindingList<LockerAccessPointDTO>(lockerAccessPointListOnDisplay);
                lockerAccessPointListBS.DataSource = lockerAccessPointDTOSortList;
            }
            else
                lockerAccessPointListBS.DataSource = new SortableBindingList<LockerAccessPointDTO>();
            dgvDisplayData.DataError += new DataGridViewDataErrorEventHandler(dgvDisplayData_ComboDataError);
            dgvDisplayData.DataSource = lockerAccessPointListBS;
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvDisplayData_ComboDataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvDisplayData_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);           
            if (e.ColumnIndex == dgvDisplayData.Columns["GroupCode"].Index)
            {
                if (lookupValuesDTOList != null)
                    dgvDisplayData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValue;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string[] ipValidate;
            BindingSource lockerAccessPointListBS = (BindingSource)dgvDisplayData.DataSource;
            var lockerAccessPointListOnDisplay = (SortableBindingList<LockerAccessPointDTO>)lockerAccessPointListBS.DataSource;            
            if (lockerAccessPointListOnDisplay.Count > 0)
            {
                foreach (LockerAccessPointDTO lockerAccessPointDTO in lockerAccessPointListOnDisplay)
                {
                    if (string.IsNullOrEmpty(lockerAccessPointDTO.Name))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Please enter AP name."));
                        return;
                    }
                    if (string.IsNullOrEmpty(lockerAccessPointDTO.IPAddress))
                    {
                        MessageBox.Show("Please enter the IP address.");
                        return;
                    }
                    ipValidate=lockerAccessPointDTO.IPAddress.Split('.');
                    if (ipValidate.Length!=4)
                    {
                        MessageBox.Show("Please enter the IP address.");
                        return;
                    }
                    if (lockerAccessPointDTO.LockerIDFrom==-1)
                    {
                        MessageBox.Show("Please enter the locker starting range.");
                        return;
                    }
                    if (lockerAccessPointDTO.LockerIDTo == -1)
                    {
                        MessageBox.Show("Please enter the locker ending range.");
                        return;
                    }
                    LockerAccessPoint lockerAccessPoint = new LockerAccessPoint(machineUserContext, lockerAccessPointDTO);
                    if (lockerAccessPoint.IsLockerAssignedToAP())
                    {
                        MessageBox.Show("Lockers range you are entered is already exists.");
                        return;
                    }
                    else
                    {
                        lockerAccessPoint.Save();
                    }
                }
                PopulateAccessPointGrid();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtName.Text = "";
            chbShowActiveEntries.Checked = true;
            PopulateAccessPointGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.dgvDisplayData.SelectedRows.Count <= 0 && this.dgvDisplayData.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit();
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
            foreach (DataGridViewRow lockerAccessPointRow in this.dgvDisplayData.SelectedRows)
            {
                if (lockerAccessPointRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(lockerAccessPointRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvDisplayData.Rows.RemoveAt(lockerAccessPointRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource lockerAccessPointDTOListDTOBS = (BindingSource)dgvDisplayData.DataSource;
                        var lockerAccessPointDTOList = (SortableBindingList<LockerAccessPointDTO>)lockerAccessPointDTOListDTOBS.DataSource;
                        LockerAccessPointDTO lockerAccessPointDTO = lockerAccessPointDTOList[lockerAccessPointRow.Index];
                        lockerAccessPointDTO.IsActive = false;
                        LockerAccessPoint lockerAccessPoint = new LockerAccessPoint(machineUserContext, lockerAccessPointDTO);
                        lockerAccessPoint.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            PopulateAccessPointGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSearch_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateAccessPointGrid();
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvDisplayData_DataError
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvDisplayData_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",  " + utilities.MessageUtils.getMessage("Column") + " " + dgvDisplayData.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }
    }
}
