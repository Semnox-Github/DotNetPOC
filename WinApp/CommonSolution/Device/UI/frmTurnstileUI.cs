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

namespace Semnox.Parafait.Device.Turnstile
{
    /// <summary>
    /// Used for storing the details of the Turnstile
    /// </summary>
    public partial class frmTurnstileUI : Form
    {
        Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        //List<LookupValuesDTO> invoiceTypeLookUpValueList;
        SortableBindingList<TurnstileDTO> tunrstileSetupSortableList;
        /// <summary>
        /// Constructor of frmTurnstileUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public frmTurnstileUI(Utilities utilities)
        {
            log.Debug("Starts-frmTurnstileUI(utilities) parameterized constructor.");
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvTurnstile);
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
            dgvTurnstile.BorderStyle = BorderStyle.FixedSingle;
            log.Debug("Ends-frmTurnstileUI(utilities) parameterized constructor.");
        }

        private void frmTurnstileUI_Load(object sender, EventArgs e)
        {
            RefreshData();
        }
        private void RefreshData()
        {
            log.Debug("Starts-RefreshData() method.");            
            LoadGameProfiles();
            LoadTurnstileMake();
            LoadTurnstileType();
            LoadTurnstileModel();
            LoadTurnstileDTOList();
            log.Debug("Ends-RefreshData() Method");
        }

        private void LoadTurnstileType()
        {
            log.Debug("Starts-LoadTurnstileType() method.");
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TURNSTILE_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> turnstileTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (turnstileTypeLookUpValueList == null)
                {
                    turnstileTypeLookUpValueList = new List<LookupValuesDTO>();
                }
                turnstileTypeLookUpValueList.Insert(0, new LookupValuesDTO());
                turnstileTypeLookUpValueList[0].LookupValueId = -1;
                turnstileTypeLookUpValueList[0].LookupValue = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = turnstileTypeLookUpValueList;
                turnstileTypeDataGridViewTextBoxColumn.DataSource = bs;
                turnstileTypeDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                turnstileTypeDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.Debug("Ends-LoadTurnstileType() Method");
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadTurnstileType() Method with an Exception:", e);
            }
        }

        private void LoadTurnstileMake()
        {
            log.Debug("Starts-LoadTurnstileMake() method.");
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TURNSTILE_MAKE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> turnstileMakeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (turnstileMakeLookUpValueList == null)
                {
                    turnstileMakeLookUpValueList = new List<LookupValuesDTO>();
                }
                turnstileMakeLookUpValueList.Insert(0, new LookupValuesDTO());
                turnstileMakeLookUpValueList[0].LookupValueId = -1;
                turnstileMakeLookUpValueList[0].LookupValue = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = turnstileMakeLookUpValueList;
                turnstileMakeDataGridViewTextBoxColumn.DataSource = bs;
                turnstileMakeDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                turnstileMakeDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.Debug("Ends-LoadTurnstileMake() Method");
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadTurnstileMake() Method with an Exception:", e);
            }
        }
        private void LoadTurnstileModel()
        {
            log.Debug("Starts-LoadTurnstileModel() method.");
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "TURNSTILE_MODEL"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<LookupValuesDTO> turnstileModelLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (turnstileModelLookUpValueList == null)
                {
                    turnstileModelLookUpValueList = new List<LookupValuesDTO>();
                }
                turnstileModelLookUpValueList.Insert(0, new LookupValuesDTO());
                turnstileModelLookUpValueList[0].LookupValueId = -1;
                turnstileModelLookUpValueList[0].LookupValue = "<SELECT>";
                BindingSource bs = new BindingSource();
                bs.DataSource = turnstileModelLookUpValueList;
                turnstileModelDataGridViewTextBoxColumn.DataSource = bs;
                turnstileModelDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                turnstileModelDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.Debug("Ends-LoadTurnstileModel() Method");
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadTurnstileModel() Method with an Exception:", e);
            }
        }
        private void LoadTurnstileDTOList()
        {
            log.Debug("Starts-LoadTurnstileDTOList() method.");
            try
            {
                TurnstilesList turnstileSetupList = new TurnstilesList(machineUserContext);
                List<KeyValuePair<TurnstileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TurnstileDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                    searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.ACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.TURNSTILE_NAME, Convert.ToString(txtTurnstileName.Text)));

                searchParameters.Add(new KeyValuePair<TurnstileDTO.SearchByParameters, string>(TurnstileDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

                //TurnstileSearchParams searchParams = new TurnstileSearchParams();
                //searchParams.Active = Convert.ToBoolean(chbShowActiveEntries.Checked);
                //searchParams.TurnstileName = Convert.ToString(txtTurnstileName.Text);

                List<TurnstileDTO> turnstileSetupDTOList = turnstileSetupList.GetAllTurnstilesList(searchParameters);

                if (turnstileSetupDTOList != null && turnstileSetupDTOList.Count > 0)
                {
                    tunrstileSetupSortableList = new SortableBindingList<TurnstileDTO>(turnstileSetupDTOList);
                }
                else
                {
                    tunrstileSetupSortableList = new SortableBindingList<TurnstileDTO>();
                }
                TurnstileSetupDTOListBS.DataSource = tunrstileSetupSortableList;

                log.Debug("Ends-LoadTurnstileDTOList() Method");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
                log.Error("Ends-LoadTurnstileDTOList() Method with an Exception:", ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
            dgvTurnstile.EndEdit();
            lblMessage.Text = string.Empty;
            SortableBindingList<TurnstileDTO> turnstileSetupDTOSortableList = (SortableBindingList<TurnstileDTO>)TurnstileSetupDTOListBS.DataSource;
            string message;
            TurnstileBL turnstileSetupBL;
            if (turnstileSetupDTOSortableList != null)
            {
                for (int i = 0; i < turnstileSetupDTOSortableList.Count; i++)
                {
                    if (turnstileSetupDTOSortableList[i].IsChanged)
                    {
                        message = ValidateTurnstileSetupDTO(turnstileSetupDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            //if (((MessageBox.Show(utilities.MessageUtils.getMessage(566), "Confirm Save.", MessageBoxButtons.YesNo) == DialogResult.Yes)))
                            //{
                                turnstileSetupBL = new TurnstileBL(machineUserContext, turnstileSetupDTOSortableList[i]);
                                try
                                {
                                    turnstileSetupBL.Save();
                                    if (turnstileSetupDTOSortableList[i].TurnstileId > -1)
                                    {
                                        lblMessage.Text = utilities.MessageUtils.getMessage(122);
                                        //MessageBox.Show(utilities.MessageUtils.getMessage(122));
                                    }
                                    RefreshData();
                                }
                                catch (Exception)
                                {
                                    log.Error("Error while saving event.");
                                    dgvTurnstile.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            //}
                        }
                        else
                        {
                            dgvTurnstile.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            log.Debug("Ends-btnSave_Click() Event.");
        }

        private string ValidateTurnstileSetupDTO(TurnstileDTO tsSequenceSetupDTO)
        {
            log.Debug("Starts-ValidateTurnstileSetupDTO() method.");
            string message = string.Empty;
            if (string.IsNullOrWhiteSpace(tsSequenceSetupDTO.TurnstileName))
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", turnstileNameDataGridViewTextBoxColumn.HeaderText);
                return message;
            }

            if (tsSequenceSetupDTO.GameProfileId < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", gameProfileIdDataGridViewComboBoxColumn.HeaderText);
                return message;
            }

            if (tsSequenceSetupDTO.Type < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", turnstileTypeDataGridViewTextBoxColumn.HeaderText);
                return message;
            }

            if (tsSequenceSetupDTO.Make < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", turnstileMakeDataGridViewTextBoxColumn.HeaderText);
                return message;
            }
           
            if (tsSequenceSetupDTO.Model < 0)
            {
                message = utilities.MessageUtils.getMessage(1144);
                message = message.Replace("&1", turnstileModelDataGridViewTextBoxColumn.HeaderText);
            }
            log.Debug("Ends-ValidateTurnstileSetupDTO() Method");
            return message;                 
        }

        private void btnControlPanel_Click(object sender, EventArgs e)
        {
            lblMessage.Text = string.Empty;
            bool formOpen = false;
            foreach (Form f in Application.OpenForms)
            {
                if (f.Name == "frmShowTurnstiles")
                {
                    f.Focus();
                    formOpen = true;
                }
            }
            if(!formOpen)
            {
                TurnstileUI tui = new TurnstileUI();
                tui.Show();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() Event.");
            if (dgvTurnstile.SelectedRows.Count <= 0 && dgvTurnstile.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if (this.dgvTurnstile.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvTurnstile.SelectedCells)
                {
                    dgvTurnstile.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow row in dgvTurnstile.SelectedRows)
            {
                if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvTurnstile.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        SortableBindingList<TurnstileDTO> turnstileDTOSortableList = (SortableBindingList<TurnstileDTO>)TurnstileSetupDTOListBS.DataSource;
                        TurnstileDTO turnstileSetupDTO = turnstileDTOSortableList[row.Index];
                        turnstileSetupDTO.Active = false;
                        TurnstileBL turnstileSetupBL = new TurnstileBL(machineUserContext,  turnstileSetupDTO);
                        confirmDelete = true;
                        refreshFromDB = true;
                        try
                        {
                            turnstileSetupBL.Save();
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            dgvTurnstile.Rows[row.Index].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1083));
                            continue;
                        }
                    }
                }
            }
            if (rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if (refreshFromDB == true)
            {
                RefreshData();
            }
            log.Debug("Ends-btnDelete_Click() Event.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnRefresh_Click() Event.");
            lblMessage.Text = string.Empty;
            txtTurnstileName.Text = "";
            dgvTurnstile.EndEdit();
            SortableBindingList<TurnstileDTO> turnstileSetupDTOSortableList = (SortableBindingList<TurnstileDTO>)TurnstileSetupDTOListBS.DataSource;
            string message;
            TurnstileBL turnstileSetupBL;
            DialogResult dialogResult = new DialogResult() ;
            bool showDialog = true; ;
            if (turnstileSetupDTOSortableList != null)
            {
                for (int i = 0; i < turnstileSetupDTOSortableList.Count; i++)
                {
                    if (turnstileSetupDTOSortableList[i].IsChanged)
                    {
                        message = ValidateTurnstileSetupDTO(turnstileSetupDTOSortableList[i]);
                        if (string.IsNullOrEmpty(message))
                        {
                            if (showDialog)
                            {
                                showDialog = false;
                                dialogResult = MessageBox.Show(utilities.MessageUtils.getMessage(566), utilities.MessageUtils.getMessage("Refresh Turnstiles"), MessageBoxButtons.YesNoCancel);
                            }

                            if (dialogResult == DialogResult.Yes)
                            {
                                turnstileSetupBL = new TurnstileBL(machineUserContext, turnstileSetupDTOSortableList[i]);
                                try
                                {
                                    turnstileSetupBL.Save();
                                    if (turnstileSetupDTOSortableList[i].TurnstileId > -1)
                                    {
                                        lblMessage.Text = utilities.MessageUtils.getMessage(122);
                                    }
                                }
                                catch (Exception)
                                {
                                    log.Error("Error while saving event.");
                                    dgvTurnstile.Rows[i].Selected = true;
                                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                    break;
                                }
                            }
                            else if (dialogResult == DialogResult.No)
                                break;
                            else if (dialogResult == DialogResult.Cancel)
                                return;
                            else
                                break;
                            
                        }
                        else
                        {
                            dgvTurnstile.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            RefreshData();
            log.Debug("Ends-btnRefresh_Click() Event.");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnClose_Click() Event.");
            this.Close();
            log.Debug("Ends-btnClose_Click() Event.");
        }

        private void LoadGameProfiles()
        {
            log.Debug("Starts-LoadGameProfiles() Event.");
            try
            {
                DataAccessHandler dataHandler = new DataAccessHandler();

                DataTable profileDT = new DataTable();

                profileDT.Columns.Add("game_profile_id", typeof(int));
                profileDT.Columns.Add("profile_name");
                profileDT = dataHandler.executeSelectQuery(@"select game_profile_id, profile_name 
                                                                    from game_profile", null);
                
                gameProfileIdDataGridViewComboBoxColumn.DataSource = profileDT;
                gameProfileIdDataGridViewComboBoxColumn.ValueMember = "game_profile_id";
                gameProfileIdDataGridViewComboBoxColumn.DisplayMember = "profile_name";
                DataRow dr = profileDT.NewRow();
                dr["game_profile_id"] = -1;
                dr["profile_name"] = "<SELECT>";

                profileDT.Rows.InsertAt(dr, 0);

            }
            catch (Exception ex)
            {
                log.Error("Error while loading game profiles-LoadGameProfiles() Event." + ex.Message);
            }
            log.Debug("Ends-LoadGameProfiles() Event.");

        }

        private void dgvTurnstile_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.Debug("Starts-dgvTurnstile_DataError() Event.");
            MessageBox.Show("Error in Turnstile grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvTurnstile.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.Debug("Ends-dgvTurnstile_DataError() Event.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            LoadTurnstileDTOList();
            log.LogMethodExit(null);
        }

        private void chbShowActiveEntries_CheckedChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-chbShowActiveEntries_CheckedChanged() Event.");
            lblMessage.Text = string.Empty;
            RefreshData();
            log.Debug("Ends-chbShowActiveEntries_CheckedChanged() Event.");
        }
    }

       
}
