/********************************************************************************************
 * Project Name - Media UI
 * Description  - User interface for media
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        07-Jan-2017   Raghuveera     Created
 *2.70.2      14-Aug-2019   Dakshakh       Added logger methods
 *2.80.0      17-Feb-2019   Deeksha        Modified to Make DigitalSignage module as
 *                                         read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// Media UI
    /// </summary>
    public partial class frmMediaListUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private List<LookupValuesDTO> lookupValuesDTOList;
        private BindingSource mediaListBS;
        private int allMediaTypeLookupValueId = -1;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// 
        /// </summary>
        public frmMediaListUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvMediaList);

            if(utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            lnkPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublish.Visible = true;
            }
            LoadMediaType();
            PopulateMediaGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the status to the combo boxes
        /// </summary>
        private void LoadMediaType()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "MEDIA_TYPE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if(lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                foreach(var item in lookupValuesDTOList)
                {
                    if(string.Equals(item.LookupValue, "ALL"))
                    {
                        allMediaTypeLookupValueId = item.LookupValueId;
                        break;
                    }
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValueId = -1;
                lookupValuesDTOList[0].LookupValue = "<SELECT>";
                //lookupValuesDTOList[0].Description = utilities.MessageUtils.getMessage("");
                bindingSource.DataSource = lookupValuesDTOList;
                cmbType.DataSource = bindingSource;
                cmbType.ValueMember = "LookupValueId";
                cmbType.DisplayMember = "LookupValue";

                typeIdDataGridViewTextBoxColumn.DataSource = lookupValuesDTOList;
                typeIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                typeIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";

                log.LogMethodExit();
            }
            catch(Exception e)
            {
                log.Error(e.Message);
            }
        }

        /// <summary>
        /// Loads the media to grid
        /// </summary>
        private void PopulateMediaGrid()
        {
            log.LogMethodEntry();
            MediaList mediaList = new MediaList(machineUserContext);
            List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> mediaSearchParams = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
            if (chbShowActiveEntries.Checked)
            {
                mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, "1"));
            }
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.NAME, txtName.Text));
            if(cmbType.SelectedValue != null && (int)cmbType.SelectedValue != -1 && (int)cmbType.SelectedValue != allMediaTypeLookupValueId)
            {
                mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.TYPE_ID, cmbType.SelectedValue.ToString()));
            }
            mediaSearchParams.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<MediaDTO> mediaListOnDisplay = mediaList.GetAllMedias(mediaSearchParams);
            mediaListBS = new BindingSource();
            if(mediaListOnDisplay != null)
            {
                SortableBindingList<MediaDTO> mediaDTOSortList = new SortableBindingList<MediaDTO>(mediaListOnDisplay);
                mediaListBS.DataSource = mediaDTOSortList;
            }
            else
                mediaListBS.DataSource = new SortableBindingList<MediaDTO>();
            mediaListBS.AddingNew += dgvMediaList_BindingSourceAddNew;
            dgvMediaList.DataSource = mediaListBS;
            dgvMediaList.DataError += new DataGridViewDataErrorEventHandler(dgvMediaList_ComboDataError);
            log.LogMethodExit();
        }

        private void dgvMediaList_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry();
            if(dgvMediaList.Rows.Count == mediaListBS.Count)
            {
                mediaListBS.RemoveAt(mediaListBS.Count - 1);
            }
            log.LogMethodExit();
        }

        private void dgvMediaList_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            if(e.ColumnIndex == dgvMediaList.Columns["typeIdDataGridViewTextBoxColumn"].Index)
            {
                if(lookupValuesDTOList != null)
                    dgvMediaList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDTOList[0].LookupValueId;
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                chbShowActiveEntries.Checked = true;
                txtName.ResetText();
                LoadMediaType();
                PopulateMediaGrid();
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
                if (this.dgvMediaList.SelectedRows.Count <= 0 && this.dgvMediaList.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvMediaList.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvMediaList.SelectedCells)
                    {
                        dgvMediaList.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow mediaRow in this.dgvMediaList.SelectedRows)
                {
                    if (Convert.ToInt32(mediaRow.Cells[0].Value.ToString()) < 0)
                    {
                        dgvMediaList.Rows.RemoveAt(mediaRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource mediaDTOListDTOBS = (BindingSource)dgvMediaList.DataSource;
                            var mediaDTOList = (SortableBindingList<MediaDTO>)mediaDTOListDTOBS.DataSource;
                            MediaDTO mediaDTO = mediaDTOList[mediaRow.Index];
                            mediaDTO.IsActive = false;
                            Media media = new Media(machineUserContext, mediaDTO);
                            try
                            {
                                media.Save();
                            }
                            catch (ForeignKeyException)
                            {
                                dgvMediaList.Rows[mediaRow.Index].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch (Exception ex)
                            {
                                dgvMediaList.Rows[mediaRow.Index].Selected = true;
                                MessageBox.Show(ex.Message);
                                break;
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                btnSearch.PerformClick();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            long mediaID;
            try
            {
                mediaID = Convert.ToInt64(dgvMediaList.CurrentRow.Cells["mediaIdDataGridViewTextBoxColumn"].Value);
                MediaListDetails mediaListDetails = new MediaListDetails(utilities, mediaID);
                mediaListDetails.ShowDialog();
                btnRefresh.PerformClick();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MediaListDetails mediaListDetails = new MediaListDetails(utilities, -1);
                mediaListDetails.ShowDialog();
                btnRefresh.PerformClick();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateMediaGrid();
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lnkPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvMediaList.CurrentRow.Cells["mediaIdDataGridViewTextBoxColumn"].Value != null)
                {
                    int id = Convert.ToInt32(dgvMediaList.CurrentRow.Cells["mediaIdDataGridViewTextBoxColumn"].Value);

                    if (id <= 0)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    PublishUI publishUI = new PublishUI(utilities, id, "Media", dgvMediaList.CurrentRow.Cells["mediaNameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableDigitalSignageModule)
            {
                dgvMediaList.AllowUserToAddRows = true;
                dgvMediaList.ReadOnly = false;
                btnNew.Enabled = true;
                btnDelete.Enabled = true;
                btnEdit.Enabled = true;
                lnkPublish.Enabled = true;
            }
            else
            {
                dgvMediaList.AllowUserToAddRows = false;
                dgvMediaList.ReadOnly = true;
                btnNew.Enabled = false;
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                lnkPublish.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
