/********************************************************************************************
 * Project Name - Reservation
 * Description  - Booking CheckList form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      12-Nov-2019   Guru S A                Created for Waiver phase 2 enhancement changes  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Maintenance;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmBookingCheckList : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReservationBL reservationBL;
        private ExecutionContext executionContext = null;
        private Utilities utilities;
        public frmBookingCheckList(ExecutionContext executionContext, Utilities utilities, ReservationBL reservationBL)
        {
            log.LogMethodEntry(executionContext, reservationBL);
            InitializeComponent();
            this.executionContext = executionContext;
            this.utilities = utilities;
            this.reservationBL = reservationBL;
            SetUIElements();
            utilities.setLanguage();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void frmBookingCheckList_Load(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadEventHostDropdown();
                LoadChecklistDropdown();
                LoadReservationName();
                LoadBookingCheckListDetails();
                SetStyleForDGV();
                utilities.setLanguage(this);
                this.ActiveControl = btnCancel;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                this.Close();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void LoadEventHostDropdown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            string excludedUserRoleList = GetExcludedUserRoleList();
            UsersList usersListBL = new UsersList(executionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_NOT_IN, excludedUserRoleList));
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<UsersDTO> eventHostUsersDTOList = usersListBL.GetAllUsers(searchParameters);
            UsersDTO usersDTO = new UsersDTO();
            usersDTO.LoginId = "- None -";
            if (eventHostUsersDTOList == null)
            {
                eventHostUsersDTOList = new List<UsersDTO>();
            }
            eventHostUsersDTOList.Insert(0, usersDTO);
            eventHostUserId.DataSource = eventHostUsersDTOList;
            eventHostUserId.DisplayMember = "UserName";
            eventHostUserId.ValueMember = "UserId";
            //eventHostUserId.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadChecklistDropdown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            JobTaskGroupList jobTaskGroupListBL = new JobTaskGroupList(executionContext);
            List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParameters = new List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>>();
            searchParameters.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.HAS_ACTIVE_TASKS, ""));
            searchParameters.Add(new KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>(JobTaskGroupDTO.SearchByJobTaskGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<JobTaskGroupDTO> jobTaskGroupDTOList = jobTaskGroupListBL.GetAllJobTaskGroups(searchParameters);
            JobTaskGroupDTO jobTaskGroupDTO = new JobTaskGroupDTO();
            jobTaskGroupDTO.TaskGroupName = "- None -";
            if (jobTaskGroupDTOList == null)
            {
                jobTaskGroupDTOList = new List<JobTaskGroupDTO>();
            }
            jobTaskGroupDTOList.Insert(0, jobTaskGroupDTO);
            checklistTaskGroupId.DataSource = jobTaskGroupDTOList;
            checklistTaskGroupId.DisplayMember = "TaskGroupName";
            checklistTaskGroupId.ValueMember = "JobTaskGroupId";
            //checklistTaskGroupId.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private string GetExcludedUserRoleList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            string excludedUserRoleList = "";
            LookupValuesList lookupValuesListBL = new LookupValuesList(executionContext);
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "EXCLUDED_USER_ROLES_FOR_HOST_LIST"));
            searchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<LookupValuesDTO> lookupValuesDTOList = lookupValuesListBL.GetAllLookupValues(searchParameters);
            if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
            {
                int count = 0;
                foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDTOList)
                {
                    if (count < lookupValuesDTOList.Count - 1)
                    {
                        excludedUserRoleList = excludedUserRoleList + lookupValuesDTO.Description + ",";
                    }
                    else
                    {
                        excludedUserRoleList = excludedUserRoleList + lookupValuesDTO.Description;
                    }
                    count++;
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(excludedUserRoleList);
            return excludedUserRoleList;
        }

        private void LoadReservationName()
        {
            log.LogMethodEntry();
            txtBookingName.Clear();
            if (reservationBL != null && reservationBL.GetReservationDTO != null)
            {
                txtBookingName.Text = reservationBL.GetReservationDTO.BookingName;
            }
            log.LogMethodExit();
        }

        private void LoadBookingCheckListDetails()
        {
            log.LogMethodEntry();
            if (reservationBL != null && reservationBL.GetReservationDTO != null)
            {
                SortableBindingList<BookingCheckListDTO> checkListDTOList = new SortableBindingList<BookingCheckListDTO>();
                if(reservationBL.GetReservationDTO.BookingCheckListDTOList != null && reservationBL.GetReservationDTO.BookingCheckListDTOList.Any())
                {
                    checkListDTOList = new SortableBindingList<BookingCheckListDTO>(reservationBL.GetReservationDTO.BookingCheckListDTOList);
                }
                dgvBookingCheckList.DataSource = checkListDTOList;
            }
            log.LogMethodExit();
        }

        private void SetUIElements()
        {
            log.LogMethodEntry();
            bool isEditMode = BookingIsInEditMode();
            if (isEditMode)
            {
                dgvBookingCheckList.ReadOnly = false;
                this.btnSave.Enabled = this.btnDelete.Enabled  = btnRefresh.Enabled = true;
            }
            else
            {
                dgvBookingCheckList.ReadOnly = true;
                this.btnSave.Enabled = this.btnDelete.Enabled = btnRefresh.Enabled = false;
            }
            log.LogMethodExit();
        }

        private void SetStyleForDGV()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvBookingCheckList);
            dgvBookingCheckList.Columns["lastUpdateDateDataGridViewTextBoxColumn"].DefaultCellStyle =
                dgvBookingCheckList.Columns["creationDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle(); 
            // dgvBookingCheckList.Columns["creationDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewNumericCellStyle(); 
            dgvBookingCheckList.Columns["lastUpdateDateDataGridViewTextBoxColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            SetDGVCellFont(dgvBookingCheckList); 
            log.LogMethodExit();
        }


        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry();
            System.Drawing.Font font;
            try
            {
                font = new Font(utilities.ParafaitEnv.DEFAULT_GRID_FONT, 15, FontStyle.Regular);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while applying new font", ex);
                font = new Font("Tahoma", 15, FontStyle.Regular);
            }
            foreach (DataGridViewColumn c in dgvInput.Columns)
            {
                c.DefaultCellStyle.Font = new Font(font.FontFamily, 15F, FontStyle.Regular);
            }
            log.LogMethodExit();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (HasChanges())
                {
                    SortableBindingList<BookingCheckListDTO> bookingCheckListDTOList = dgvBookingCheckList.DataSource as SortableBindingList<BookingCheckListDTO>;
                    if(bookingCheckListDTOList != null && bookingCheckListDTOList.Any())
                    {
                        for (int i = 0; i < bookingCheckListDTOList.Count; i++)
                        {
                            POSUtils.SetLastActivityDateTime();
                            BookingCheckListBL bookingCheckListBL = new BookingCheckListBL(executionContext, bookingCheckListDTOList[i]);
                            List<ValidationError> validationErrorList =  bookingCheckListBL.Validate();
                            if(validationErrorList != null && validationErrorList.Any())
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext,"Validation Error"), validationErrorList);
                            }
                            IsDuplicateEntry(bookingCheckListDTOList[i], i); 
                        }
                    }
                    this.reservationBL.GetReservationDTO.BookingCheckListDTOList = new List<BookingCheckListDTO>(bookingCheckListDTOList);
                    this.reservationBL.SaveReservationCheckListOnly();
                    RefreshDetails();
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();

        }

        private void IsDuplicateEntry(BookingCheckListDTO bookingCheckListDTO, int rowIndex)
        {
            log.LogMethodEntry(bookingCheckListDTO, rowIndex);
            SortableBindingList<BookingCheckListDTO> bookingCheckListDTOList = dgvBookingCheckList.DataSource as SortableBindingList<BookingCheckListDTO>;
            if (bookingCheckListDTOList != null && bookingCheckListDTOList.Any())
            {
                List<BookingCheckListDTO> matchingRecords = bookingCheckListDTOList.Where(bcl => bcl.EventHostUserId == bookingCheckListDTO.EventHostUserId 
                                                                                                     && bcl.ChecklistTaskGroupId == bookingCheckListDTO.ChecklistTaskGroupId).ToList();
                if(matchingRecords != null && matchingRecords.Count > 1)
                { 
                    dgvBookingCheckList.Rows[rowIndex].Cells["eventHostUserId"].Selected = true;
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
            }
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool hasChanges = false;
            SortableBindingList<BookingCheckListDTO> bookingCheckListDTOList = dgvBookingCheckList.DataSource as SortableBindingList<BookingCheckListDTO>; 
            if (bookingCheckListDTOList != null && bookingCheckListDTOList.Any())
            {
                hasChanges = bookingCheckListDTOList.Any(bcl => bcl.IsChanged == true);
            }
            log.LogMethodExit(hasChanges);
            return hasChanges;
        }

        private bool BookingIsInEditMode()
        {
            log.LogMethodEntry();
            bool inEditMode = ((reservationBL.GetReservationDTO == null || (reservationBL.GetReservationDTO != null
                                                                                                      && (reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.NEW.ToString()
                                                                                                         || reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.WIP.ToString()
                                                                                                         || reservationBL.GetReservationDTO.Status == ReservationDTO.ReservationStatus.BLOCKED.ToString())
                                                                                                     )));
            log.LogMethodExit(inEditMode);
            return inEditMode;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvBookingCheckList.Rows.Count > 0)
                {
                    if (dgvBookingCheckList.SelectedRows != null && dgvBookingCheckList.SelectedRows.Count > 0)
                    {
                        for(int i = 0; i <dgvBookingCheckList.SelectedRows.Count; i++)
                        {
                            POSUtils.SetLastActivityDateTime();
                            DataGridViewCheckBoxCell checkBox = (dgvBookingCheckList.SelectedRows[i].Cells["isActiveDataGridViewCheckBoxColumn"] as DataGridViewCheckBoxCell);
                            if (Convert.ToBoolean(checkBox.Value))
                            {
                                int bclId = -1;
                                int.TryParse(dgvBookingCheckList.SelectedRows[i].Cells["bookingCheckListIdDataGridViewTextBoxColumn"].Value.ToString(), out bclId);
                                if (bclId > -1)
                                {
                                    checkBox.Value = false;
                                }
                                else
                                {
                                    dgvBookingCheckList.Rows.Remove(dgvBookingCheckList.SelectedRows[i]);
                                    i = i - 1;
                                }
                            }
                        }
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 959));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824,ex.Message));
            }
            log.LogMethodExit();
        }

        private void dgvBookingCheckList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (e.RowIndex > 0)
            {
               if( dgvBookingCheckList.Columns[e.ColumnIndex].Name == "eventHostUserId" 
                    || dgvBookingCheckList.Columns[e.ColumnIndex].Name == "checklistTaskGroupId")
                {
                    dgvBookingCheckList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = -1;
                }
            }
            log.LogMethodExit();
        }

        private void dgvBookingCheckList_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["isActiveDataGridViewCheckBoxColumn"].Value = "True";
            e.Row.Cells["bookingIdDataGridViewTextBoxColumn"].Value = reservationBL.GetReservationDTO.BookingId;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            RefreshDetails();
            log.LogMethodExit();
        }

        private void RefreshDetails()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (HasChanges())
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2107), MessageContainerList.GetMessage(executionContext, "Close"), MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        log.LogMethodExit();
                        return;
                    }
                }
                if (reservationBL != null && reservationBL.GetReservationDTO != null)
                {
                    reservationBL.LoadCheckListDetails();
                    LoadBookingCheckListDetails();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void dgvBookingCheckList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (e.RowIndex > 0)
            {
                if(dgvBookingCheckList.Columns[e.ColumnIndex].Name == "isActiveDataGridViewCheckBoxColumn")
                {
                    DataGridViewCheckBoxCell checkBox = (dgvBookingCheckList["isActiveDataGridViewCheckBoxColumn", e.RowIndex] as DataGridViewCheckBoxCell);
                    if (Convert.ToBoolean(checkBox.Value))
                    {
                        checkBox.Value = false;
                    }
                    else
                    {
                        checkBox.Value = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void Scroll_ButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
    }
}
