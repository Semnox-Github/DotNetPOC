/********************************************************************************************
 * Project Name - SignagePatternListUI
 * Description  - UI for Signage Pattern List 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      14-Aug-2019   Dakshakh       Added logger methods
 *2.80.0      17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                         read only in Windows Management Studio.
 *********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Publish;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.DigitalSignage
{
    public partial class SignagePatternListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ManagementStudioSwitch managementStudioSwitch;

        /// <summary>
        /// Constructor of SignagePatternListUI class.
        /// </summary>
        /// <param name="utilities">Parafait Utilities</param>
        public SignagePatternListUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvSignagePatternDTOList);
            utilities.setLanguage(this);
            lnkPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void SignagePatternListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            patternDataGridViewTextBoxColumn.MaxInputLength = 100;
            nameDataGridViewTextBoxColumn.MaxInputLength = 100;
            RefreshData();
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadSignagePatternDTOList();
            log.LogMethodExit();
        }

        private void LoadSignagePatternDTOList()
        {
            log.LogMethodEntry();
            try
            {
                SignagePatternListBL signagePatternListBL = new SignagePatternListBL(utilities.ExecutionContext);
                List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SignagePatternDTO.SearchByParameters, string>>();
                if (chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.IS_ACTIVE, "1"));
                }
                searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.NAME, txtName.Text));
                searchParameters.Add(new KeyValuePair<SignagePatternDTO.SearchByParameters, string>(SignagePatternDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                List<SignagePatternDTO> signagePatternDTOList = signagePatternListBL.GetSignagePatternDTOList(searchParameters);
                SortableBindingList<SignagePatternDTO> signagePatternDTOSortableList;
                if (signagePatternDTOList != null)
                {
                    signagePatternDTOSortableList = new SortableBindingList<SignagePatternDTO>(signagePatternDTOList);
                }
                else
                {
                    signagePatternDTOSortableList = new SortableBindingList<SignagePatternDTO>();
                }
                signagePatternDTOListBS.DataSource = signagePatternDTOSortableList;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while loading SignagePatternDTO list", ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvSignagePatternDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageContainerList.GetMessage(utilities.ExecutionContext, 1144, MessageContainerList.GetMessage(utilities.ExecutionContext, dgvSignagePatternDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (HasChanges() &&
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 714), MessageContainerList.GetMessage(utilities.ExecutionContext, "Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    return;
                }
                chbShowActiveEntries.Checked = true;
                txtName.ResetText();
                RefreshData();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
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
            try
            {
                LoadSignagePatternDTOList();
            }
            catch(Exception ex)
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
                dgvSignagePatternDTOList.EndEdit();
                SortableBindingList<SignagePatternDTO> signagePatternDTOSortableList = signagePatternDTOListBS.DataSource as SortableBindingList<SignagePatternDTO>;
                SignagePatternBL signagePatternBL;
                bool error = false;
                if (signagePatternDTOSortableList != null && signagePatternDTOSortableList.Count > 0)
                {
                    for (int i = 0; i < signagePatternDTOSortableList.Count; i++)
                    {
                        if (signagePatternDTOSortableList[i].IsChanged)
                        {
                            try
                            {
                                signagePatternBL = new SignagePatternBL(utilities.ExecutionContext, signagePatternDTOSortableList[i]);
                                signagePatternBL.Save();
                            }
                            catch (ValidationException ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record", ex);
                                dgvSignagePatternDTOList.Rows[i].Selected = true;
                                MessageBox.Show(ex.GetAllValidationErrorMessages());
                                break;
                            }
                            catch (Exception ex)
                            {
                                error = true;
                                log.Error("Error occured while saving the record", ex);
                                dgvSignagePatternDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                }
                if (!error)
                {
                    btnSearch.PerformClick();
                }
                else
                {
                    dgvSignagePatternDTOList.Update();
                    dgvSignagePatternDTOList.Refresh();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvSignagePatternDTOList.SelectedRows.Count <= 0 && dgvSignagePatternDTOList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit(null, "No rows selected. Please select the rows you want to delete and press delete");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                bool refreshFromDB = false;
                if (this.dgvSignagePatternDTOList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in dgvSignagePatternDTOList.SelectedCells)
                    {
                        dgvSignagePatternDTOList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow row in dgvSignagePatternDTOList.SelectedRows)
                {
                    if (Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvSignagePatternDTOList.Rows.RemoveAt(row.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            SortableBindingList<SignagePatternDTO> signagePatternDTOSortableList = (SortableBindingList<SignagePatternDTO>)signagePatternDTOListBS.DataSource;
                            SignagePatternDTO signagePatternDTO = signagePatternDTOSortableList[row.Index];
                            signagePatternDTO.IsActive = false;
                            SignagePatternBL signagePatternBL = new SignagePatternBL(utilities.ExecutionContext, signagePatternDTO);
                            confirmDelete = true;
                            refreshFromDB = true;
                            try
                            {
                                signagePatternBL.Save();
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                dgvSignagePatternDTOList.Rows[row.Index].Selected = true;
                                MessageBox.Show(ex.Message);
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
                    btnSearch.PerformClick();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SignagePatternListUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (HasChanges())
            {
                if (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 127), MessageContainerList.GetMessage(utilities.ExecutionContext, "Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            log.LogMethodExit();
        }

        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool result = false;
            SortableBindingList<SignagePatternDTO> signagePatternDTOSortableList = signagePatternDTOListBS.DataSource as SortableBindingList<SignagePatternDTO>;
            if (signagePatternDTOSortableList != null && signagePatternDTOSortableList.Count > 0)
            {
                foreach (var signagePatternDTO in signagePatternDTOSortableList)
                {
                    if (signagePatternDTO.IsChanged)
                    {
                        result = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvSignagePatternDTOList.CurrentRow.Cells["signagePatternIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvSignagePatternDTOList.CurrentRow.Cells["signagePatternIdDataGridViewTextBoxColumn"].Value);

                    if (id <= 0)
                        return;
                    PublishUI publishUI = new PublishUI(utilities, id, "SignagePattern", dgvSignagePatternDTOList.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvSignagePatternDTOList.AllowUserToAddRows = true;
                dgvSignagePatternDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                lnkPublish.Enabled = true;
            }
            else
            {
                dgvSignagePatternDTOList.AllowUserToAddRows = false;
                dgvSignagePatternDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                lnkPublish.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
