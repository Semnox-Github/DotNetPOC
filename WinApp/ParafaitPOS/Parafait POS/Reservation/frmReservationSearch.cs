/********************************************************************************************
 * Project Name - Reservations
 * Description  - UI for reservation search
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.50.0      28-Nov-2018      Guru S A       UI enhancements
 *2.70.0      01-Feb-2019      Guru S A       Booking Phase 2 changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmReservationSearch : Form
    {
        private Utilities utilities = POSStatic.Utilities;
        private TaskProcs taskProcs = POSStatic.TaskProcs;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VirtualKeyboardController virtualKeyboard;
        public delegate void PerformActivityTimeOutChecksdelegate(int inactivityPeriodSec);
        public PerformActivityTimeOutChecksdelegate PerformActivityTimeOutChecks; 

        public frmReservationSearch()
        {
            log.LogMethodEntry();
            Logger.setRootLogLevel(log);
            utilities.setLanguage();
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            ((Parafait_POS.POS)Application.OpenForms["POS"]).LoadMasterScheduleBLList();
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }

        private void frmReservations_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoodBookingProducts();
            DefaultStatusList();
            LoadFacilityMap();
            LoadCheckListAssigneeDropdown();
            SetupDay();
            SetupWeek();
            RefreshGrids();
            utilities.setLanguage(this); 
            StartInActivityTimeoutTimer();
            log.LogMethodExit();
        }

        private void LoodBookingProducts()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            ProductsList productsListBL = new ProductsList(utilities.ExecutionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, ProductTypeValues.BOOKINGS));
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.SITEID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<ProductsDTO> bookinProductsDTOLIst = productsListBL.GetProductsDTOList(searchParams);
            if (bookinProductsDTOLIst == null)
            {
                bookinProductsDTOLIst = new List<ProductsDTO>();
            }
            else
            {
                bookinProductsDTOLIst = bookinProductsDTOLIst.OrderBy(prod => prod.ProductName).ToList();
            }
            ProductsDTO productsALLDTO = new ProductsDTO
            {
                ProductName = "- All -"
            };
            bookinProductsDTOLIst.Insert(0, productsALLDTO);
            cmbBookingProducts.DataSource = bookinProductsDTOLIst;
            cmbBookingProducts.DisplayMember = "ProductName";
            cmbBookingProducts.ValueMember = "ProductId";
            cmbBookingProducts.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadFacilityMap()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(utilities.ExecutionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, ProductTypeValues.BOOKINGS));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(facilitySearcParm);
            FacilityMapDTO firstFacilityMapDTO = new FacilityMapDTO
            {
                FacilityMapName = "- All -"
            };
            if (facilityMapDTOList == null || facilityMapDTOList.Count == 0)
            {
                facilityMapDTOList = new List<FacilityMapDTO>();
            }
            List<FacilityMapDTO> dupNameList = facilityMapDTOList.Where(f => facilityMapDTOList.Where(ff => ff.FacilityMapName == f.FacilityMapName).Count() > 1).ToList();
            if (dupNameList != null && dupNameList.Any())
            {
                for (int i = 0; i < facilityMapDTOList.Count; i++)
                {
                    if (dupNameList.Exists(f => f.FacilityMapId == facilityMapDTOList[i].FacilityMapId))
                    {
                        facilityMapDTOList[i].FacilityMapName = facilityMapDTOList[i].FacilityMapName + " [" + facilityMapDTOList[i].FacilityMapId + "]";
                    }
                }
            }
            facilityMapDTOList.Insert(0, firstFacilityMapDTO);

            cmbFacility.DataSource = facilityMapDTOList;
            cmbFacility.DisplayMember = "FacilityMapName";
            cmbFacility.ValueMember = "FacilityMapId";
            cmbFacility.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private void LoadCheckListAssigneeDropdown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            UsersList usersListBL = new UsersList(utilities.ExecutionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            //searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_NOT_IN, excludedUserRoleList));
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<UsersDTO> eventHostUsersDTOList = usersListBL.GetAllUsers(searchParameters);
            UsersDTO usersDTO = new UsersDTO();
            usersDTO.LoginId = "- None -";
            if (eventHostUsersDTOList == null)
            {
                eventHostUsersDTOList = new List<UsersDTO>();
            }
            eventHostUsersDTOList.Insert(0, usersDTO);
            cmbCheckListAssignee.DataSource = eventHostUsersDTOList;
            cmbCheckListAssignee.DisplayMember = "UserName";
            cmbCheckListAssignee.ValueMember = "UserId";
            cmbCheckListAssignee.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void DefaultStatusList()
        {
            log.LogMethodEntry();
            cbxBlockedStatus.Checked = cbxBookedStatus.Checked = cbxConfirmedStatus.Checked = cbxCompleteStatus.Checked = cbxWIPStatus.Checked = true;
            cbxCancelledStatus.Checked = false;
            log.LogMethodExit();
        }

        private bool AnyStatusIncludedForSearch()
        {
            log.LogMethodEntry();
            bool checkBoxIsChecked = false;
            if (cbxBlockedStatus.Checked || cbxBookedStatus.Checked || cbxCancelledStatus.Checked || cbxConfirmedStatus.Checked || cbxCompleteStatus.Checked || cbxWIPStatus.Checked)
            {
                checkBoxIsChecked = true;
            }
            log.LogMethodExit(checkBoxIsChecked);
            return checkBoxIsChecked;
        }


        private bool AllStatusInclcudedForSearch()
        {
            log.LogMethodEntry();
            bool checkBoxIsChecked = false;
            if (cbxBlockedStatus.Checked && cbxBookedStatus.Checked && cbxCancelledStatus.Checked && cbxConfirmedStatus.Checked && cbxCompleteStatus.Checked && cbxWIPStatus.Checked)
            {
                checkBoxIsChecked = true;
            }
            log.LogMethodExit(checkBoxIsChecked);
            return checkBoxIsChecked;
        }


        private string GetSelectedBookingStatus()
        {
            log.LogMethodEntry();
            StringBuilder statusValues = new StringBuilder();
            if (AllStatusInclcudedForSearch())
            {
                statusValues.Append("ALL");
                statusValues.Append(",");
            }
            else
            {
                if (cbxBlockedStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.BLOCKED.ToString());
                    statusValues.Append("',");
                }
                if (cbxBookedStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.BOOKED.ToString());
                    statusValues.Append("',");
                }
                if (cbxCancelledStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.CANCELLED.ToString());
                    statusValues.Append("',");
                }
                if (cbxConfirmedStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.CONFIRMED.ToString());
                    statusValues.Append("',");
                }
                if (cbxCompleteStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.COMPLETE.ToString());
                    statusValues.Append("',");
                }
                if (cbxWIPStatus.Checked)
                {
                    statusValues.Append("'" + ReservationDTO.ReservationStatus.WIP.ToString());
                    statusValues.Append("',");
                }
            }
            string statusValue = statusValues.ToString();
            statusValue = statusValue.Remove(statusValue.LastIndexOf(","), 1);

            log.LogMethodExit(statusValue);
            return statusValue;
        }

        void RefreshList()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<ReservationDTO> reservationDTOList = GetBookingRecords();
            dgvAll.DataSource = reservationDTOList;

            utilities.setupDataGridProperties(ref dgvAll);

            dgvAll.Columns["recurEndDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvAll.Columns["fromDateDataGridViewTextBoxColumn"].DefaultCellStyle =
            dgvAll.Columns["toDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();

            SetDGVCellFont(dgvAll);

            dgvAll.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            if (dgvAll.Rows.Count > 0)
            {
                dgvAll.Rows[0].Selected = true;
            }
            else
            {
                ResetDGVBookingDetailsStyle();
            }
            calendarAllHScrollBar.Visible = true;
            calendarAllHScrollBar.Update();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                ValidateSearchParameters();
                SetSuitableGridTabAndRefresh();
            }
            catch (ValidationException ex)
            {
                string validationMsg = ex.GetAllValidationErrorMessages();
                string errorMsg = String.IsNullOrEmpty(validationMsg) == true ? ex.Message : validationMsg;
                POSUtils.ParafaitMessageBox(errorMsg);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void ValidateSearchParameters()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            List<ValidationError> searchParamValidationErrors = new List<ValidationError>();
            if (dtpfromDate.Value.Date > dtpToDate.Value)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainerList.GetMessage(utilities.ExecutionContext, "To Date")
                                                                                    , MessageContainerList.GetMessage(utilities.ExecutionContext, 724));
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (cmbBookingProducts.SelectedValue == null)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Product")
                                                                                    , MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Product") + ": " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1787));
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (cmbFacility.SelectedValue == null)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainerList.GetMessage(utilities.ExecutionContext, "Facility")
                                                                                    , MessageContainerList.GetMessage(utilities.ExecutionContext, "Facility") + ": " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1787)); // "Please select a valid option from the list"
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (cmbCheckListAssignee.SelectedValue == null)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainerList.GetMessage(utilities.ExecutionContext, "Assignee")
                                                                                    , MessageContainerList.GetMessage(utilities.ExecutionContext, "Assignee") + ": " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1787)); // "Please select a valid option from the list"
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (AnyStatusIncludedForSearch() == false)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainerList.GetMessage(utilities.ExecutionContext, "Status")
                                                                                    , MessageContainerList.GetMessage(utilities.ExecutionContext, "Status") + ": " + MessageContainerList.GetMessage(utilities.ExecutionContext, 1836)); // "Please select at least one booking status entry"
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (searchParamValidationErrors.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Validation"), searchParamValidationErrors);
            }
            log.LogMethodExit();
        }
        private void SetSuitableGridTabAndRefresh()
        {
            log.LogMethodEntry();
            bool callRefresh = true; //tab selection also fires refresh grid
            if (String.IsNullOrEmpty(txtReservationCode.Text.Trim()) == false || String.IsNullOrEmpty(txtCardNumber.Text.Trim()) == false
                 || String.IsNullOrEmpty(txtCustomerName.Text.Trim()) == false)
            {

                if (tabCalendar.SelectedTab.Equals(tabPageList) == false)
                {
                    tabCalendar.SelectedTab = tabPageList;
                    callRefresh = false;
                }
            }
            else if (dtpfromDate.Value.Date.AddDays(1) == dtpToDate.Value.Date)
            {
                if (tabCalendar.SelectedTab.Equals(tabPageDay) == false)
                {
                    tabCalendar.SelectedTab = tabPageDay;
                    callRefresh = false;
                }
            }
            else if (GetFirstDayOfWeek() == dtpfromDate.Value.Date && dtpfromDate.Value.Date.AddDays(7) == dtpToDate.Value.Date)
            {

                if (tabCalendar.SelectedTab.Equals(tabPageWeek) == false)
                {
                    tabCalendar.SelectedTab = tabPageWeek;
                    callRefresh = false;
                }
            }
            else if (tabCalendar.SelectedTab.Equals(tabPageList) == false)
            {
                tabCalendar.SelectedTab = tabPageList;
                callRefresh = false;
            }

            if (callRefresh)
            {
                RefreshGrids();
            }
            log.LogMethodExit();
        }
        private void btnNewReservation_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                StopInActivityTimeoutTimer();
                using (frmReservationUI fm = new frmReservationUI(DateTime.Now.Date, (decimal)DateTime.Now.Hour))
                {
                    fm.PerformActivityTimeOutChecks += new frmReservationUI.PerformActivityTimeOutChecksdelegate(POSUtils.CallPerformActivityTimeOutChecks);
                    fm.ShowDialog(); 
                    fm.Dispose();
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Close();
            log.LogMethodExit();
        }

        private void SetupDay()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            SetupTime(ref dgvDay);
            SetupDGVs(ref dgvDay);
            SetupDayHeader();
            UpdateDayColumnHeaders();
            log.LogMethodExit();
        }

        private void UpdateDayColumnHeaders()
        {
            log.LogMethodEntry();
            dgvDay.Columns[0].HeaderText = "";
            dgvDayHeader.Columns[0].HeaderText = dtpfromDate.Value.ToString("ddd, MMM dd");
            log.LogMethodExit();
        }

        private void SetupWeek()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            SetupTime(ref dgvWeek);
            SetupDGVs(ref dgvWeek);
            SetupWeekHeader();
            UpdateWeekColumnHeaders();
            log.LogMethodExit();
        }

        private void UpdateWeekColumnHeaders()
        {
            log.LogMethodEntry();
            DateTime firstDayOfWeek;
            firstDayOfWeek = GetFirstDayOfWeek();
            foreach (DataGridViewColumn dc in dgvWeek.Columns)
            {
                dc.HeaderText = "";
                dgvWeekHeader.Columns[dc.Index].HeaderText = firstDayOfWeek.AddDays(dc.Index).ToString("ddd, MMM dd");
            }
            log.LogMethodExit();
        }


        private void SetupTime(ref DataGridView dgv)
        {
            log.LogMethodEntry();
            dgv.Rows.Clear();
            dgv.RowTemplate.Height = 12;
            for (int i = 0; i < 96; i++)
            {
                dgv.Rows.Add(new object[] { "", "", "", "", "", "", "" });

                if (i == 0)
                    dgv.Rows[i].HeaderCell.Value = "12:AM";
                else
                {
                    if (i == 48)
                    {
                        dgv.Rows[i].HeaderCell.Value = "12:PM";
                    }
                    else
                    {
                        if (i % 4 == 0)
                        {
                            if (i < 48)
                                dgv.Rows[i].HeaderCell.Value = (i / 4).ToString() + ":00";
                            else
                                dgv.Rows[i].HeaderCell.Value = ((i - 48) / 4).ToString() + ":00";
                        }
                        else if (i % 2 == 0)
                            dgv.Rows[i].HeaderCell.Value = ":30";
                        else
                            dgv.Rows[i].HeaderCell.Value = "";
                    }
                }
            }
            log.LogMethodExit();
        }

        private void SetupDGVs(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.ScrollBars = ScrollBars.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.Size = new Size(dgv.Width, dgv.Rows.Count * dgv.Rows[0].Height + dgv.ColumnHeadersHeight);
            dgv.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.BackgroundColor = this.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 8);
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn dc in dgv.Columns)
                dc.SortMode = DataGridViewColumnSortMode.NotSortable;

            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = false;
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgv.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgv.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            dgv.RowHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.RowHeadersWidth = 50;

            dgv.GridColor = dgv.DefaultCellStyle.BackColor = Color.LightYellow;
            dgv.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgv.MultiSelect = false;
            log.LogMethodExit();
        }

        void SetupWeekHeader()
        {
            log.LogMethodEntry();
            dgvWeekHeader.EnableHeadersVisualStyles = false;
            dgvWeekHeader.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvWeekHeader.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvWeekHeader.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvWeekHeader.RowHeadersWidth = 50;
            dgvWeekHeader.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 8);
            dgvWeekHeader.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvWeekHeader.RowTemplate.Height = 12;

            foreach (DataGridViewColumn dc in dgvWeekHeader.Columns)
            { dc.SortMode = DataGridViewColumnSortMode.NotSortable; }

            log.LogMethodExit();
        }

        void SetupDayHeader()
        {
            log.LogMethodEntry();
            dgvDayHeader.EnableHeadersVisualStyles = false;
            dgvDayHeader.RowHeadersDefaultCellStyle.BackColor = Color.LightGray;
            dgvDayHeader.RowHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvDayHeader.RowHeadersDefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvDayHeader.RowHeadersWidth = 50;
            dgvDayHeader.ColumnHeadersDefaultCellStyle.Font = new Font("arial", 8);
            dgvDayHeader.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDayHeader.RowTemplate.Height = 12;

            foreach (DataGridViewColumn dc in dgvDayHeader.Columns)
            { dc.SortMode = DataGridViewColumnSortMode.NotSortable; }

            log.LogMethodExit();
        }

        private void dgvWeek_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
            }
            log.LogMethodExit();
        }

        bool CodeChangingSelection = false;
        private void dgvWeek_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (CodeChangingSelection)
            {
                log.LogMethodExit("CodeChangingSelection is true");
                return;
            }
            CodeChangingSelection = true;

            int minColumn = 100, maxColumn = -1;
            int minColRow = 100, maxColRow = -1;

            foreach (DataGridViewCell c in dgvWeek.SelectedCells)
            {
                if (c.ColumnIndex < minColumn)
                    minColumn = c.ColumnIndex;

                if (c.ColumnIndex > maxColumn)
                    maxColumn = c.ColumnIndex;
            }

            foreach (DataGridViewCell c in dgvWeek.SelectedCells)
            {
                if (c.ColumnIndex == minColumn)
                    if (c.RowIndex < minColRow)
                        minColRow = c.RowIndex;

                if (c.ColumnIndex == maxColumn)
                    if (c.RowIndex > maxColRow)
                        maxColRow = c.RowIndex;
            }

            int MAXROWS = dgvWeek.Rows.Count - 1;
            if (minColumn < maxColumn)
            {
                for (int i = minColumn; i <= maxColumn; i++)
                {
                    if (i == minColumn)
                    {
                        for (int j = minColRow; j <= MAXROWS; j++)
                            dgvWeek.Rows[j].Cells[i].Selected = true;
                    }
                    else
                        if (i == maxColumn)
                    {
                        for (int j = 0; j <= maxColRow; j++)
                            dgvWeek.Rows[j].Cells[i].Selected = true;
                    }
                    else
                    {
                        for (int j = 0; j <= MAXROWS; j++)
                            dgvWeek.Rows[j].Cells[i].Selected = true;
                    }
                }
            }
            else
            {
                for (int i = minColumn; i <= maxColumn; i++)
                {
                    for (int j = minColRow; j <= maxColRow; j++)
                        dgvWeek.Rows[j].Cells[i].Selected = true;
                }
            }
            CodeChangingSelection = false;
            log.LogMethodExit();
        }

        private void GetSelectedTimeSlotOfWeek(DataGridView dgv, DateTime RefDate, ref DateTime FromTime, ref DateTime ToTime)
        {
            log.LogMethodEntry(dgv, RefDate, FromTime, ToTime);
            int minColumn = 100, maxColumn = -1;
            int minColRow = 100, maxColRow = -1;

            if (dgv.SelectedCells.Count <= 0)
                return;
            foreach (DataGridViewCell c in dgv.SelectedCells)
            {
                if (c.ColumnIndex < minColumn)
                    minColumn = c.ColumnIndex;

                if (c.ColumnIndex > maxColumn)
                    maxColumn = c.ColumnIndex;
            }

            foreach (DataGridViewCell c in dgv.SelectedCells)
            {
                if (c.ColumnIndex == minColumn)
                    if (c.RowIndex < minColRow)
                        minColRow = c.RowIndex;

                if (c.ColumnIndex == maxColumn)
                    if (c.RowIndex > maxColRow)
                        maxColRow = c.RowIndex;
            }

            FromTime = RefDate.Date;
            FromTime = FromTime.AddDays(minColumn);
            int hour = Convert.ToInt32(minColRow * 15 / 60);
            int mins = minColRow % 4 * 15;
            FromTime = FromTime.AddHours(hour);
            FromTime = FromTime.AddMinutes(mins);

            ToTime = RefDate.Date;
            ToTime = ToTime.AddDays(maxColumn);
            maxColRow++;
            hour = Convert.ToInt32(maxColRow * 15 / 60);
            mins = maxColRow % 4 * 15;
            ToTime = ToTime.AddHours(hour);
            ToTime = ToTime.AddMinutes(mins);
            log.LogMethodExit();
        }

        private void dgvWeek_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            cellPaint(sender, e);
            log.LogMethodExit();
        }

        private void cellPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Value != null)
            {
                if (e.Value.ToString().Contains(":")) // header
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentForeground);
                    if (e.Value.ToString() == ":30")
                        e.Graphics.DrawLine(Pens.Gray, e.CellBounds.X + 30, e.CellBounds.Y, e.CellBounds.Right - 10, e.CellBounds.Y);
                    else
                        e.Graphics.DrawLine(Pens.Gray, e.CellBounds.X + 10, e.CellBounds.Y, e.CellBounds.Right - 10, e.CellBounds.Y);
                    string hour = e.Value.ToString();
                    string mins = hour.Substring(hour.IndexOf(':') + 1);
                    hour = hour.Substring(0, hour.IndexOf(':'));

                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Far;

                    Brush brush = Brushes.Black;
                    if (e.RowIndex >= 48)
                        brush = Brushes.DarkGoldenrod;
                    e.Graphics.DrawString(hour, new Font(this.Font.FontFamily, 9, FontStyle.Regular)
                            , brush,
                            new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 30, e.CellBounds.Y),
                            sf);

                    int fontSize = 9;
                    brush = Brushes.Black;
                    if (mins == "30")
                        fontSize = 8;
                    if (e.RowIndex >= 48)
                        brush = Brushes.DarkGoldenrod;

                    e.Graphics.DrawString(mins, new Font(this.Font.FontFamily, fontSize, FontStyle.Regular)
                            , brush,
                            new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width - 10, e.CellBounds.Y),
                            sf);

                    e.Handled = true;
                }
                else if (e.ColumnIndex >= 0 && e.RowIndex >= 0) // cells
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    if (e.RowIndex % 4 == 0)
                    {
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    else
                    {
                        e.Graphics.DrawLine(Pens.Khaki, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Right, e.CellBounds.Y);
                        e.Graphics.DrawLine(Pens.DarkKhaki, e.CellBounds.X, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                    }
                    e.Graphics.DrawLine(Pens.Black, e.CellBounds.X, e.CellBounds.Y, e.CellBounds.X, e.CellBounds.Bottom);

                    e.Handled = true;
                }
            }
            log.LogMethodExit();
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            //refreshGrids();
            log.LogMethodExit();
        }

        private void RefreshWeekGrid()
        {
            log.LogMethodEntry();
            UpdateWeekColumnHeaders();

            DateTime reservationFromDate, reservationToDate;
            DateTime origreservationFromDate, origreservationToDate;

            long reservation_id;
            string status;
            string recur_flag, recur_frequency;
            string reservation_name;
            DateTime? recur_end_date = null;

            int minRow = -1;
            int maxRow = 100;
            int numrows;
            DateTime cellTime;

            // scroll to top of tab page
            ScrollableControl scrCtl = panelDGV as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvWeek);
            List<ReservationDTO> reservationDTOList = GetBookingRecords();
            for (int i = 0; i < panelDGV.Controls.Count; i++)
            {
                Control c = panelDGV.Controls[i];
                if (c.Name.Contains("reservationDisplay"))
                {
                    panelDGV.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }

            Label lbl = null;
            DateTime prevDate = DateTime.MinValue;
            int offset = 0;

            for (int rows = 0; rows < reservationDTOList.Count; rows++)
            {
                reservation_id = reservationDTOList[rows].BookingId;
                reservation_name = reservationDTOList[rows].BookingName;
                reservationFromDate = reservationDTOList[rows].FromDate;
                reservationToDate = reservationDTOList[rows].ToDate;
                status = reservationDTOList[rows].Status;
                recur_flag = reservationDTOList[rows].RecurFlag;
                recur_frequency = reservationDTOList[rows].RecurFrequency;
                recur_end_date = reservationDTOList[rows].RecurEndDate;
                if (prevDate != reservationFromDate.Date)
                {
                    offset = 0;
                    prevDate = reservationFromDate.Date;
                }
                else
                {
                    offset += 30;
                    if (offset > 70)
                        offset = 10;
                }

                reservationFromDate = reservationFromDate.Date.AddHours(reservationFromDate.Hour).AddMinutes(reservationFromDate.Minute);
                reservationToDate = reservationToDate.Date.AddHours(reservationToDate.Hour).AddMinutes(reservationToDate.Minute);

                origreservationFromDate = reservationFromDate;
                origreservationToDate = reservationToDate;

                int numIterations = 0;

                if (recur_flag == "Y")
                    numIterations = dgvWeek.Columns.Count; // repeat for each day of week to check if recur applicable
                else
                    numIterations = 1;

                for (int recurDate = 0; recurDate < numIterations; recurDate++)
                {
                    if (recur_flag == "Y")
                    {
                        DateTime cellTimeRecur = GetGridCellDateTime(0, recurDate); // get date on day of week

                        if (recur_frequency == "Weekly")
                        {
                            if (cellTimeRecur.DayOfWeek != origreservationFromDate.DayOfWeek)
                                continue;
                        }

                        if ((recur_end_date != null && recur_end_date >= cellTimeRecur.Date) && origreservationFromDate.Date <= cellTimeRecur.Date) // check if recur has ended
                        {
                            TimeSpan ts = reservationToDate.Date - reservationFromDate.Date; // used to get number of days the reservation spans over. change reservation from and to days as per week day date
                            reservationFromDate = cellTimeRecur.Date.AddHours(reservationFromDate.Hour).AddMinutes(reservationFromDate.Minute);
                            reservationToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(reservationToDate.Hour).AddMinutes(reservationToDate.Minute);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    foreach (DataGridViewColumn dc in dgvWeek.Columns)
                    {
                        if (!dc.Visible)
                            continue;
                        minRow = -1;
                        maxRow = 100;
                        for (int i = 0; i < dgvWeek.RowCount; i++)
                        {
                            cellTime = GetGridCellDateTime(i, dc.Index);

                            if (cellTime >= reservationFromDate && cellTime < reservationToDate)
                            {
                                if (minRow == -1)
                                    minRow = i;
                                maxRow = i;
                            }
                        }

                        if (minRow != -1)
                        {
                            Label reservationDisplay = new Label();
                            reservationDisplay.Name = "reservationDisplay" + rows.ToString();
                            reservationDisplay.Font = new Font("arial", 8);
                            reservationDisplay.BorderStyle = BorderStyle.FixedSingle;

                            ToolTip tp = new ToolTip();
                            tp.IsBalloon = true;
                            reservationDisplay.Text = reservation_name + " - " + origreservationFromDate.ToString("h:mm tt") + " to " + origreservationToDate.ToString("h:mm tt");

                            if (recur_flag == "Y")
                            {
                                reservationDisplay.Text += " Recurs every " + (recur_frequency == "Daily" ? "day " : "week ");
                                reservationDisplay.Text += "until " + (recur_end_date != null ? Convert.ToDateTime(recur_end_date).ToString(utilities.ParafaitEnv.DATE_FORMAT) : "");
                            }
                            if (status.ToUpper() == ReservationDTO.ReservationStatus.CANCELLED.ToString())
                            {
                                tp.SetToolTip(reservationDisplay, "CANCELLED-" + reservationDisplay.Text);
                                reservationDisplay.BackColor = Color.LightCoral;
                                reservationDisplay.Font = new Font(reservationDisplay.Font, FontStyle.Strikeout);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightBlue;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.COMPLETE.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightGreen;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.BOOKED.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightPink;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
                            {
                                reservationDisplay.BackColor = Color.Aquamarine;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }

                            reservationDisplay.Tag = reservation_id;
                            reservationDisplay.Click += new EventHandler(ReservationDisplay_Click);
                            reservationDisplay.DoubleClick += new EventHandler(ReservationDisplay_DoubleClick);

                            if (maxRow == minRow)
                                numrows = 1;
                            else
                                numrows = maxRow - minRow + 1;

                            reservationDisplay.Size = new Size(dgvWeek.Columns[0].Width - 70 - (numrows * 3), dgvWeek.Rows[0].Height * numrows);
                            reservationDisplay.Location = new Point(offset + dgvWeek.Columns[0].Width * dc.Index + dgvWeek.RowHeadersWidth + 1, dgvWeek.Rows[0].Height * minRow + dgvWeek.ColumnHeadersHeight + 1);
                            panelDGV.Controls.Add(reservationDisplay);
                            reservationDisplay.BringToFront();

                            lbl = reservationDisplay;
                        }
                    }
                }
            }

            this.ActiveControl = dgvWeek;
            if (lbl != null)
            {
                ReservationDisplay_Click(lbl, null);
                scrCtl.ScrollControlIntoView(lbl);
            }
            else
            {
                ResetDGVBookingDetailsStyle();
                using (Control c = new Control() { Parent = panelDGV, Height = 1, Top = 1000 })
                {
                    scrCtl.ScrollControlIntoView(c);
                }
            }
            calendarAllHScrollBar.Visible = false;
            log.LogMethodExit();
        }

        private List<ReservationDTO> GetBookingRecords()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            string bookingStatusValue = GetSelectedBookingStatus();
            ReservationListBL reservationListBL = new ReservationListBL(utilities.ExecutionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            if (String.IsNullOrEmpty(txtReservationCode.Text.Trim()) == true && String.IsNullOrEmpty(txtCardNumber.Text.Trim()) == true && String.IsNullOrEmpty(txtCustomerName.Text.Trim()) == true)
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, dtpfromDate.Value.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_TO_DATE, dtpToDate.Value.Date.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
            }

            if (cmbBookingProducts.SelectedValue.ToString() != "-1")
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.BOOKING_PRODUCT_ID, cmbBookingProducts.SelectedValue.ToString()));
            }
            if (bookingStatusValue != "ALL")
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_IN, bookingStatusValue));
            }
            searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, "'SYSTEMABANDONED'"));
            if (String.IsNullOrEmpty(txtReservationCode.Text.Trim()) == false)
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_CODE_LIKE, txtReservationCode.Text));
            }
            if (String.IsNullOrEmpty(txtCardNumber.Text.Trim()) == false)
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CARD_NUMBER_LIKE, txtCardNumber.Text));
            }
            if (String.IsNullOrEmpty(txtCustomerName.Text.Trim()) == false)
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CUSTOMER_NAME_LIKE, txtCustomerName.Text));
            }
            if (cmbFacility.SelectedValue.ToString() != "-1")
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.FACILITY_MAP_ID, cmbFacility.SelectedValue.ToString()));
            }

            if (cmbCheckListAssignee.SelectedValue.ToString() != "-1")
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CHECKLIST_TASK_ASSIGNEE_ID, cmbCheckListAssignee.SelectedValue.ToString()));
            }
            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParams);

            if (reservationDTOList != null && reservationDTOList.Any())
            {
                reservationDTOList = reservationDTOList.OrderBy(res => res.FromDate).ThenBy(res => res.ToDate).ToList();
            }
            log.LogVariableState("@fromDate", dtpfromDate.Value.Date);
            log.LogVariableState("@toDate", dtpToDate.Value.Date);
            log.LogVariableState("@BookingProductId", cmbBookingProducts.SelectedValue);
            log.LogVariableState("@RCode", txtReservationCode.Text.Trim() == "" ? DBNull.Value : (object)txtReservationCode.Text);
            log.LogVariableState("@Status", bookingStatusValue);
            log.LogVariableState("@customer", txtCustomerName.Text.Trim() == "" ? DBNull.Value : (object)("%" + txtCustomerName.Text + "%"));
            log.LogVariableState("@cardNumber", txtCardNumber.Text.Trim() == "" ? DBNull.Value : (object)("%" + txtCardNumber.Text + "%"));
            log.LogVariableState("@FacilityMapId", cmbFacility.SelectedValue);
            log.LogVariableState("@CheckListAssigneeId", cmbCheckListAssignee.SelectedValue);

            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(reservationDTOList);
            return reservationDTOList;
        }
        void ReservationDisplay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                Label lbl = sender as Label;

                GetBookingDetails(lbl.Tag);

                Panel panel;
                if (tabCalendar.SelectedTab.Equals(tabPageWeek))
                    panel = panelDGV;
                else
                    panel = panelDay;

                for (int i = 0; i < panel.Controls.Count; i++)
                {
                    Control c = panel.Controls[i];
                    if (c.Name.Contains("reservationDisplay"))
                    {
                        Label label = c as Label;
                        if (label.ForeColor != Color.Black)
                        {
                            label.ForeColor = Color.Black;
                        }
                    }
                }
                lbl.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        void GetBookingDetails(object BookingId)
        {
            log.LogMethodEntry(BookingId);
            List<Tuple<string, string>> bookingDetails = new List<Tuple<string, string>>();
            try
            {
                POSUtils.SetLastActivityDateTime();
                Semnox.Parafait.Transaction.ReservationBL reservationBL = new Semnox.Parafait.Transaction.ReservationBL(utilities.ExecutionContext, utilities, Convert.ToInt32(BookingId));
                if (dgvBookingDetails.Columns.Count == 0)
                {
                    dgvBookingDetails.Columns.Add("Detail", "Code");
                    dgvBookingDetails.Columns[0].DataPropertyName = "Item1";
                    dgvBookingDetails.Columns.Add("Data", "Data");
                    dgvBookingDetails.Columns[1].DataPropertyName = "Item2";
                    SetDGVBookingDetailsStyle();
                }

                dgvBookingDetails.DataSource = new List<Tuple<string, string>>();
                if (reservationBL != null && reservationBL.GetReservationDTO != null)
                {
                    dgvBookingDetails.Tag = BookingId;

                    //FacilityMapBL facilityMapBL = new FacilityMapBL(utilities.ExecutionContext, reservationBL.GetReservationDTO.FacilityMapId);
                    dgvBookingDetails.Columns[1].HeaderText = reservationBL.GetReservationDTO.ReservationCode;
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Status"), reservationBL.GetReservationDTO.Status));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Date"), reservationBL.GetReservationDTO.FromDate.Date.ToString(utilities.ParafaitEnv.DATE_FORMAT)));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Facility Name"), reservationBL.GetReservationDTO.FacilityName));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Name"), reservationBL.GetReservationDTO.CustomerName));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Name"), reservationBL.GetReservationDTO.BookingName));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "From Time"), reservationBL.GetReservationDTO.FromDate.ToString("h:mm tt")));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "To Time"), reservationBL.GetReservationDTO.ToDate.ToString("h:mm tt")));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Product"), reservationBL.GetReservationDTO.BookingProductName.ToString()));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Amount Paid"), reservationBL.BookingTransaction.TotalPaidAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Guests"), reservationBL.GetReservationDTO.Quantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Contact No"), reservationBL.GetReservationDTO.ContactNo));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Email"), reservationBL.GetReservationDTO.Email));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Remarks"), reservationBL.GetReservationDTO.Remarks));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Card"), reservationBL.GetReservationDTO.CardNumber));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Channel"), reservationBL.GetReservationDTO.Channel));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Expires"), (reservationBL.GetReservationDTO.ExpiryTime == null ? "" : Convert.ToDateTime(reservationBL.GetReservationDTO.ExpiryTime).ToString(utilities.ParafaitEnv.DATETIME_FORMAT))));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Alternate No"), reservationBL.GetReservationDTO.AlternateContactNo));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Email Sent?"), reservationBL.GetReservationDTO.IsEmailSent > 0 ? "Yes" : "No"));

                    POSUtils.SetLastActivityDateTime();

                    List<Transaction.TransactionLine> bookingPackageLines = reservationBL.GetPurchasedPackageProducts();
                    if (bookingPackageLines != null && bookingPackageLines.Count > 0)
                    {
                        List<Transaction.TransactionLine> parentPackageLines = bookingPackageLines.Where(tl => tl.ComboproductId > -1 && tl.LineValid == true).ToList();
                        if (parentPackageLines != null && parentPackageLines.Count > 0)
                        {
                            List<int> disinctParentPackageProductId = parentPackageLines.Select(tl => tl.ProductID).Distinct().ToList();
                            if (disinctParentPackageProductId != null && disinctParentPackageProductId.Count > 0)
                            {
                                for (int i = 0; i < disinctParentPackageProductId.Count; i++)
                                {
                                    decimal productQty = parentPackageLines.Where(tl => tl.ProductID == disinctParentPackageProductId[i]).Sum(tlin => tlin.quantity);
                                    Transaction.TransactionLine productLine = parentPackageLines.Find(tl => tl.ProductID == disinctParentPackageProductId[i]);
                                    bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Product Contents"), productLine.ProductName + "( " + productQty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT) + " )"));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //dgvBookingDetails.Rows.Add("Exception", ex.Message);
                bookingDetails.Add(new Tuple<string, string>(MessageContainerList.GetMessage(utilities.ExecutionContext, "Exception"), ex.Message));
                log.Error(ex);
            }
            POSUtils.SetLastActivityDateTime();
            dgvBookingDetails.DataSource = bookingDetails.ToList();
            SetDGVBookingDetailsStyle();
            log.LogMethodExit();
        }

        void ReservationDisplay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (Convert.ToInt32(((Label)sender).Tag) == -1)
                {
                    contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
                }
                else
                {
                    LoadBookingForm(Convert.ToInt32(((Label)sender).Tag));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private DateTime GetFirstDayOfWeek()
        {
            log.LogMethodEntry();
            int daysToAdd = 0;
            switch (dtpfromDate.Value.DayOfWeek)
            {
                case DayOfWeek.Monday: daysToAdd = 0; break;
                case DayOfWeek.Tuesday: daysToAdd = -1; break;
                case DayOfWeek.Wednesday: daysToAdd = -2; break;
                case DayOfWeek.Thursday: daysToAdd = -3; break;
                case DayOfWeek.Friday: daysToAdd = -4; break;
                case DayOfWeek.Saturday: daysToAdd = -5; break;
                case DayOfWeek.Sunday: daysToAdd = -6; break;
                default: daysToAdd = 0; break;
            }
            log.LogMethodExit(dtpfromDate.Value.Date.AddDays(daysToAdd));
            return (dtpfromDate.Value.Date.AddDays(daysToAdd));
        }

        private DateTime GetGridCellDateTime(int rowIndex, int colIndex)
        {
            log.LogMethodEntry(rowIndex, colIndex);
            DateTime CellTime;
            if (tabCalendar.SelectedTab.Equals(tabPageWeek))
                CellTime = GetFirstDayOfWeek();
            else
                CellTime = dtpfromDate.Value.Date;

            CellTime = CellTime.AddDays(colIndex);
            int hour = Convert.ToInt32(rowIndex * 15 / 60);
            int mins = rowIndex % 4 * 15;
            CellTime = CellTime.AddHours(hour);
            CellTime = CellTime.AddMinutes(mins);
            log.LogMethodExit(CellTime);
            return CellTime;
        }

        private void tabCalendar_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            RefreshGrids();
            log.LogMethodExit();
        }

        void RefreshGrids()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                UpdateCalendarConfi();
                ValidateSearchParameters();
                if (tabCalendar.SelectedTab.Equals(tabPageWeek))
                {
                    log.Info("Week Tab is Selected");
                    RefreshWeekGrid();
                }
                else if (tabCalendar.SelectedTab.Equals(tabPageDay))
                {
                    log.Info("Day Tab is Selected");
                    RefreshDayGrid();
                }
                else
                {
                    log.Info("List Tab is Selected");
                    RefreshList();
                }
            }
            catch (Exception ex)
            {
                //string validationMsg = ex.Message;
                string errorMsg = ex.Message;
                POSUtils.ParafaitMessageBox(errorMsg);
            }
            finally
            { 
                StartInActivityTimeoutTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void dgvWeek_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            CreateNew();
            log.LogMethodExit();
        }

        private void dgvAll_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadBookingForm(Convert.ToInt32(dgvAll.CurrentRow.Cells["bookingIdDataGridViewTextBoxColumn"].Value));
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            //refreshList();
            log.LogMethodExit();
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (e.ClickedItem == menuNewBooking)
            {
                CreateNew();
            }
            log.LogMethodExit();
        }

        private void CreateNew()
        {
            log.LogMethodEntry();
            try
            {
                DateTime FromTime = DateTime.MinValue, ToTime = DateTime.MinValue;

                if (tabCalendar.SelectedTab.Equals(tabPageWeek))
                {
                    //DateTime RefDate = GetFirstDayOfWeek();
                    GetSelectedTimeSlotOfWeek(dgvWeek, dtpfromDate.Value, ref FromTime, ref ToTime);
                    if (FromTime == DateTime.MinValue)
                    {
                        log.LogMethodExit("Page Week: FromTime == DateTime.MinValue");
                        return;
                    }
                }
                else if (tabCalendar.SelectedTab.Equals(tabPageDay))
                {
                    GetSelectedTimeSlotOfWeek(dgvDay, dtpfromDate.Value, ref FromTime, ref ToTime);
                    if (FromTime == DateTime.MinValue)
                    {
                        log.LogMethodExit("Page Day: FromTime == DateTime.MinValue");
                        return;
                    }
                }
                //if (FromTime.Date >= DateTime.Now.Date && (FromTime.Hour + (decimal)FromTime.Minute / 100) >= ((decimal)DateTime.Now.Hour + (decimal)(DateTime.Now.Minute - 5) / 100))
                if (FromTime > DateTime.Now)
                {
                    try
                    {
                        this.Cursor = Cursors.WaitCursor;
                        POSUtils.SetLastActivityDateTime();
                        StopInActivityTimeoutTimer();
                        using (frmReservationUI fm = new frmReservationUI(FromTime.Date, FromTime.Hour + (decimal)FromTime.Minute / 100))
                        {
                            fm.PerformActivityTimeOutChecks += new frmReservationUI.PerformActivityTimeOutChecksdelegate(POSUtils.CallPerformActivityTimeOutChecks);
                            fm.ShowDialog(); 
                        } 
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    finally
                    {
                        POSUtils.SetLastActivityDateTime();
                        StartInActivityTimeoutTimer();
                    }
                    RefreshGrids();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnPrevWeek_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (tabCalendar.SelectedTab.Equals(tabPageDay))
                {
                    dtpToDate.Value = dtpfromDate.Value;
                    dtpfromDate.Value = dtpfromDate.Value.AddDays(-1);
                }
                else
                {
                    dtpfromDate.Value = GetFirstDayOfWeek().AddDays(-7);
                    dtpToDate.Value = dtpfromDate.Value.AddDays(7);

                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (tabCalendar.SelectedTab.Equals(tabPageDay))
                {
                    dtpfromDate.Value = dtpfromDate.Value.AddDays(1);
                    dtpToDate.Value = dtpfromDate.Value.AddDays(1);
                }
                else
                {
                    dtpfromDate.Value = GetFirstDayOfWeek().AddDays(7);
                    dtpToDate.Value = dtpfromDate.Value.AddDays(7);
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void dgvAll_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvAll.CurrentRow != null)
                {
                    GetBookingDetails(dgvAll.CurrentRow.Cells["bookingIdDataGridViewTextBoxColumn"].Value);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                txtCardNumber.Text = txtCustomerName.Text = txtReservationCode.Text = "";
                //cmbStatus.SelectedIndex = 0;
                DefaultStatusList();
                cmbBookingProducts.SelectedIndex = 0;
                cmbFacility.SelectedIndex = 0;
                cmbCheckListAssignee.SelectedIndex = 0;
                dtpfromDate.Value = DateTime.Now.Date;
                dtpToDate.Value = dtpfromDate.Value.AddDays(1);
                tabCalendar.SelectedTab = tabPageDay;
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void lnkExcelDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                StopInActivityTimeoutTimer();
                utilities.ExportToExcel(dgvAll, "Bookings for " + dtpfromDate.Value.Date.ToString("dd-MMM-yyyy"), "Bookings for " + dtpfromDate.Value.Date.ToString("dd-MMM-yyyy"), utilities.ParafaitEnv.SiteName, dtpfromDate.Value.Date, dtpToDate.Value.Date);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                StartInActivityTimeoutTimer();
            }
            log.LogMethodExit();
        }

        private void dgvDay_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            cellPaint(sender, e);
            log.LogMethodExit();
        }
        internal class RservationDateRange
        {
            DateTime fromDate;
            DateTime toDate;
            internal RservationDateRange(DateTime fromDate, DateTime toDate)
            {
                this.fromDate = fromDate;
                this.toDate = toDate;
            }
            internal DateTime FromDate { get { return fromDate; } }
            internal DateTime ToDate { get { return toDate; } }
        }
        private void RefreshDayGrid()
        {
            log.LogMethodEntry();
            UpdateDayColumnHeaders();

            DateTime reservationFromDate, reservationToDate;
            DateTime origreservationFromDate, origreservationToDate;

            long reservation_id;
            string status;
            string recur_flag, recur_frequency;
            string reservation_name;
            DateTime? recur_end_date = DateTime.MinValue;

            int minRow = -1;
            int maxRow = 100;
            int numrows;
            DateTime cellTime;

            // scroll to top of tab page
            ScrollableControl scrCtl = panelDay as ScrollableControl;
            scrCtl.ScrollControlIntoView(dgvDay);

            List<ReservationDTO> reservationDTOList = GetBookingRecords();
            //DataTable dtDay = GetBookingRecords();

            for (int i = 0; i < panelDay.Controls.Count; i++)
            {
                Control c = panelDay.Controls[i];
                if (c.Name.Contains("reservationDisplay"))
                {
                    panelDay.Controls.Remove(c);
                    c.Dispose();
                    i = -1;
                }
            }

            Label lbl = null;
            List<RservationDateRange> prevReservationDates = new List<RservationDateRange>();
            int offset = 0;
            int rowRecordCount = 0;
            int labelWidth = 150;
            int offSetValue = 0;
            for (int rows = 0; rows < reservationDTOList.Count; rows++)
            {
                reservation_id = reservationDTOList[rows].BookingId;
                reservation_name = reservationDTOList[rows].BookingName;
                reservationFromDate = reservationDTOList[rows].FromDate;
                reservationToDate = reservationDTOList[rows].ToDate;
                status = reservationDTOList[rows].Status;
                recur_flag = reservationDTOList[rows].RecurFlag;
                recur_frequency = reservationDTOList[rows].RecurFrequency;
                recur_end_date = reservationDTOList[rows].RecurEndDate;

                if (prevReservationDates.Any() == false ||
                    (prevReservationDates.Exists(d => reservationFromDate >= d.FromDate && reservationFromDate < d.ToDate) == false
                    && prevReservationDates.Exists(d => reservationToDate >= d.FromDate && reservationToDate <= d.ToDate) == false)
                    )
                {
                    offset = 0;
                    prevReservationDates.Add(new RservationDateRange(reservationFromDate, reservationToDate));
                    //prevFromDate = reservationFromDate;
                    //prevToDate = reservationToDate;
                    rowRecordCount = GetSameDateTimeReservationCount(reservationFromDate, reservationToDate, reservationDTOList);
                    labelWidth = GetLabelWidth(rowRecordCount);
                    offSetValue = GetOffSetValue(rowRecordCount) + 1;
                }
                else
                {
                    //offset += 150;
                    offset += offSetValue;
                    //if (offset > dgvDay.Columns[0].Width - 150)
                    //    offset = 50;
                }

                reservationFromDate = reservationFromDate.Date.AddHours(reservationFromDate.Hour).AddMinutes(reservationFromDate.Minute);
                reservationToDate = reservationToDate.Date.AddHours(reservationToDate.Hour).AddMinutes(reservationToDate.Minute);

                origreservationFromDate = reservationFromDate;
                origreservationToDate = reservationToDate;

                int numIterations = 0;

                if (recur_flag == "Y")
                    numIterations = dgvDay.Columns.Count; // repeat for each day of week to check if recur applicable
                else
                    numIterations = 1;

                for (int recurDate = 0; recurDate < numIterations; recurDate++)
                {
                    if (recur_flag == "Y")
                    {
                        DateTime cellTimeRecur = GetGridCellDateTime(0, recurDate); // get date on day of week

                        if (recur_frequency == "Weekly")
                        {
                            if (cellTimeRecur.DayOfWeek != origreservationFromDate.DayOfWeek)
                                continue;
                        }

                        if ((recur_end_date != null && recur_end_date >= cellTimeRecur.Date) && origreservationFromDate.Date <= cellTimeRecur.Date) // check if recur has ended
                        {
                            TimeSpan ts = reservationToDate.Date - reservationFromDate.Date; // used to get number of days the reservation spans over. change reservation from and to days as per week day date
                            reservationFromDate = cellTimeRecur.Date.AddHours(reservationFromDate.Hour).AddMinutes(reservationFromDate.Minute);
                            reservationToDate = cellTimeRecur.Date.AddDays(ts.Days).AddHours(reservationToDate.Hour).AddMinutes(reservationToDate.Minute);
                        }
                        else
                        {
                            continue;
                        }
                    }

                    foreach (DataGridViewColumn dc in dgvDay.Columns)
                    {
                        if (!dc.Visible)
                            continue;
                        minRow = -1;
                        maxRow = 100;
                        for (int i = 0; i < dgvDay.RowCount; i++)
                        {
                            cellTime = GetGridCellDateTime(i, dc.Index);

                            if (cellTime >= reservationFromDate && cellTime < reservationToDate)
                            {
                                if (minRow == -1)
                                    minRow = i;
                                maxRow = i;
                            }
                        }

                        if (minRow != -1)
                        {
                            Label reservationDisplay = new Label();
                            reservationDisplay.Name = "reservationDisplay" + rows.ToString();
                            reservationDisplay.Font = new Font("arial", 8);
                            reservationDisplay.BorderStyle = BorderStyle.FixedSingle;

                            ToolTip tp = new ToolTip();
                            tp.IsBalloon = true;

                            reservationDisplay.Text = reservation_name + " - " + origreservationFromDate.ToString("h:mm tt") + " to " + origreservationToDate.ToString("h:mm tt");

                            if (recur_flag == "Y")
                            {
                                reservationDisplay.Text += " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "Recurs every") + " " + (recur_frequency == "Daily" ? MessageContainerList.GetMessage(utilities.ExecutionContext, "Day") + " " : MessageContainerList.GetMessage(utilities.ExecutionContext, "Week") + " ");
                                reservationDisplay.Text += MessageContainerList.GetMessage(utilities.ExecutionContext, "until") + " " + (recur_end_date != null ? Convert.ToDateTime(recur_end_date).ToString(utilities.ParafaitEnv.DATE_FORMAT) : "");
                            }

                            if (status.ToUpper() == ReservationDTO.ReservationStatus.CANCELLED.ToString())
                            {
                                tp.SetToolTip(reservationDisplay, "CANCELLED-" + reservationDisplay.Text);
                                reservationDisplay.BackColor = Color.LightCoral;
                                reservationDisplay.Font = new Font(reservationDisplay.Font, FontStyle.Strikeout);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.CONFIRMED.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightBlue;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.COMPLETE.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightGreen;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.BOOKED.ToString())
                            {
                                reservationDisplay.BackColor = Color.LightPink;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else if (status.ToUpper() == ReservationDTO.ReservationStatus.WIP.ToString())
                            {
                                reservationDisplay.BackColor = Color.Aquamarine;
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }
                            else
                            {
                                tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            }

                            reservationDisplay.Tag = reservation_id;
                            reservationDisplay.Click += new EventHandler(ReservationDisplay_Click);
                            reservationDisplay.DoubleClick += new EventHandler(ReservationDisplay_DoubleClick);

                            if (maxRow == minRow)
                                numrows = 1;
                            else
                                numrows = maxRow - minRow + 1;

                            reservationDisplay.Size = new Size(labelWidth, dgvDay.Rows[0].Height * numrows);
                            reservationDisplay.Location = new Point(offset + dgvDay.Columns[0].Width * dc.Index + dgvDay.RowHeadersWidth + 1, dgvDay.Rows[0].Height * minRow + dgvDay.ColumnHeadersHeight + 1);
                            panelDay.Controls.Add(reservationDisplay);
                            reservationDisplay.BringToFront();

                            lbl = reservationDisplay;
                        }
                    }
                }
            }

            this.ActiveControl = dgvDay;
            if (lbl != null)
            {
                ReservationDisplay_Click(lbl, null);
                scrCtl.ScrollControlIntoView(lbl);
            }
            else
            {
                ResetDGVBookingDetailsStyle();
                using (Control c = new Control() { Parent = panelDay, Height = 1, Top = 1000 })
                {
                    scrCtl.ScrollControlIntoView(c);
                }
            }
            calendarAllHScrollBar.Visible = false;
            log.LogMethodExit();
        }

        private void dgvDay_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (e.Button == MouseButtons.Right)
                {
                    contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void dgvDay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                CreateNew();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void SetDGVBookingDetailsStyle()
        {
            log.LogMethodEntry();
            dgvBookingDetails.RowHeadersVisible = false;
            dgvBookingDetails.Columns[0].HeaderText = MessageContainerList.GetMessage(utilities.ExecutionContext, "Code");
            dgvBookingDetails.MultiSelect = false;
            dgvBookingDetails.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvBookingDetails.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvBookingDetails.Columns[0].DefaultCellStyle.BackColor = Color.Black;
            dgvBookingDetails.Columns[0].DefaultCellStyle.ForeColor = Color.White;
            //dgvBookingDetails.ColumnHeadersVisible = false; 
            dgvBookingDetails.Columns[0].HeaderCell.Style.BackColor = Color.Black;
            dgvBookingDetails.Columns[0].HeaderCell.Style.ForeColor = Color.White;
            dgvBookingDetails.Columns[1].HeaderCell.Style.BackColor = Color.White;
            dgvBookingDetails.Columns[1].HeaderCell.Style.ForeColor = Color.Black;
            dgvBookingDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvBookingDetails.EnableHeadersVisualStyles = false;
            dgvBookingDetails.ColumnHeadersHeight = 22;
            dgvBookingDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            if (dgvBookingDetails.Rows.Count > 1)
            {   //Toggle selection to clear the highlight color from first record
                dgvBookingDetails[0, 0].Selected = false;
            }
            log.LogMethodExit();
        }

        private void ResetDGVBookingDetailsStyle()
        {
            log.LogMethodEntry();
            dgvBookingDetails.DataSource = new List<Tuple<string, string>>();
            dgvBookingDetails.Tag = -1;
            dgvBookingDetails.RowHeadersVisible = false;
            dgvBookingDetails.Columns[0].HeaderText = "";
            dgvBookingDetails.Columns[1].HeaderText = "";
            dgvBookingDetails.Columns[0].HeaderCell.Style.BackColor = Color.White;
            dgvBookingDetails.Columns[0].HeaderCell.Style.ForeColor = Color.Black;
            dgvBookingDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            log.LogMethodExit();
        }

        private void dgvBookingDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == 0 && dgvBookingDetails.ColumnCount > 0 && dgvBookingDetails.Rows.Count > 0)
            {
                dgvBookingDetails[0, e.RowIndex].Style.SelectionBackColor = Color.Transparent;
                dgvBookingDetails[0, e.RowIndex].Style.SelectionForeColor = Color.Transparent;
                dgvBookingDetails[1, e.RowIndex].Style.SelectionBackColor = Color.Transparent;
                dgvBookingDetails[1, e.RowIndex].Style.SelectionForeColor = Color.Transparent;
            }
            log.LogMethodExit();
        }

        private void UpdateCalendarConfi()
        {
            log.LogMethodEntry();
            UpdateDateParameters();
            if (tabCalendar.SelectedTab.Equals(tabPageList))
            {
                btnPrevWeek.Enabled = false;
                btnNextWeek.Enabled = false;
                btnNextWeek.Visible = false;
                btnPrevWeek.Visible = false;
                lnkExcelDownload.Visible = true;
            }
            else
            {
                btnPrevWeek.Enabled = true;
                btnNextWeek.Enabled = true;
                btnNextWeek.Visible = true;
                btnPrevWeek.Visible = true;
                lnkExcelDownload.Visible = false;
            }
            log.LogMethodExit();
        }

        private void UpdateDateParameters()
        {
            log.LogMethodEntry();
            if (tabCalendar.SelectedTab.Equals(tabPageWeek))
            {
                dtpfromDate.Value = GetFirstDayOfWeek();
                dtpToDate.Value = dtpfromDate.Value.AddDays(7);
            }
            else if (tabCalendar.SelectedTab.Equals(tabPageDay))
            {
                dtpToDate.Value = dtpfromDate.Value.AddDays(1);
            }
            log.LogMethodExit();
        }
        private void dtpfromDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void dtpToDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
                POSUtils.SetLastActivityDateTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }


        private void BlackButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_black_normal;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void BlackButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_black_pressed;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void BlueButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.normal2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void BlueButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.pressed2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void dgvBookingDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvBookingDetails != null && Convert.ToInt32(dgvBookingDetails.Tag) != -1)
                {
                    LoadBookingForm(Convert.ToInt32(dgvBookingDetails.Tag));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvBookingDetails_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvBookingDetails != null && Convert.ToInt32(dgvBookingDetails.Tag) != -1)
                {
                    LoadBookingForm(Convert.ToInt32(dgvBookingDetails.Tag));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadBookingForm(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                StopInActivityTimeoutTimer();
                using (frmReservationUI f = new frmReservationUI(bookingId))
                {
                    f.PerformActivityTimeOutChecks += new frmReservationUI.PerformActivityTimeOutChecksdelegate(POSUtils.CallPerformActivityTimeOutChecks);
                    f.ShowDialog(); 
                    f.Dispose();
                }
                RefreshGrids();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
                StartInActivityTimeoutTimer();
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void SetDGVCellFont(DataGridView dgvInput)
        {
            log.LogMethodEntry(dgvInput);
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

        private void frmReservations_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Parafait_POS.POSUtils.AttachFormEvents();
            log.LogMethodExit();
        }

        private void btnForwardMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_Forward_Btn_Hover;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void btnForwardMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_Forward_Btn;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void btnBackwardMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_Backward_Btn_Hover;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void btnBackwardMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button selectedButton = (Button)sender;
                selectedButton.BackgroundImage = Properties.Resources.R_Backward_Btn;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void frmReservationSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.inActivityTimerClock.Stop();
                this.inActivityTimerClock.Tick += null;
                this.inActivityTimerClock.Dispose();
                this.PerformActivityTimeOutChecks += null;
                this.Dispose();
            }catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private int GetSameDateTimeReservationCount(DateTime reservationFromDate, DateTime reservationToDate, List<ReservationDTO> reservationDTOList)
        {
            log.LogMethodEntry(reservationFromDate, reservationToDate);
            int sameDatetimeRecord = reservationDTOList.Where(res => res.FromDate == reservationFromDate && res.ToDate == reservationToDate).Count();
            log.LogMethodExit(sameDatetimeRecord);
            return sameDatetimeRecord;
        }
        private int GetOffSetValue(int rowRecordCount)
        {
            log.LogMethodEntry(rowRecordCount);
            int xCoordinateValue = 150;
            try
            {
                if (rowRecordCount > 5)
                {
                    xCoordinateValue = (dgvDay.Width - calendarDayVScrollBarView.Width - -calendarDayVScrollBarView.Width) / rowRecordCount;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                xCoordinateValue = 150;
            }
            log.LogMethodExit(xCoordinateValue);
            return xCoordinateValue;
        }

        private int GetLabelWidth(int rowRecordCount)
        {
            log.LogMethodEntry(rowRecordCount);
            int labelWidth = 150;
            try
            {
                if (rowRecordCount > 5)
                {
                    labelWidth = (dgvDay.Width - calendarDayVScrollBarView.Width - calendarDayVScrollBarView.Width) / rowRecordCount;
                    if (labelWidth < 40)
                    {
                        labelWidth = 40;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                labelWidth = 150;
            }
            log.LogMethodExit(labelWidth);
            return labelWidth;
        } 
        private void inActivityTimerClock_Tick(object sender, EventArgs e)
        {
            inActivityTimerClock.Stop();
            try
            { 
                DateTime lastActivityTimeValue = POSUtils.GetPOSLastTrxActivityTime(); 
                int inactivityPeriodSec = (int)(DateTime.Now - (DateTime)lastActivityTimeValue).TotalSeconds;

                if (inactivityPeriodSec > POSStatic.POS_INACTIVE_TIMEOUT)//inactivityPeriodSec > 60
                {
                    CallPerformActivityTimeOutChecks(inactivityPeriodSec);
                }
                else
                {
                    //EnableFormElements();
                    StartInActivityTimeoutTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            } 
        }

        private void CallPerformActivityTimeOutChecks(int inactivityPeriodSec)
        {
            log.LogMethodEntry();
            if (PerformActivityTimeOutChecks != null)
            {
                PerformActivityTimeOutChecks(inactivityPeriodSec);
                StartInActivityTimeoutTimer();
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, "Timeout occured, closing the form"),
                                                        MessageContainerList.GetMessage(utilities.ExecutionContext, "Time Out"));
                POSUtils.ForceCloseCurrentScreen(this);
            }
            log.LogMethodExit();
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void cbxStatus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(); 
        }

        private void ScrollBar_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void ScrollBarView_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void StartInActivityTimeoutTimer()
        {
            log.LogMethodEntry();
            try
            {
                //if (isFormClosed == false)
                //{
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "RELOGIN_USER_AFTER_INACTIVE_TIMEOUT", false))
                {
                    inActivityTimerClock.Start();
                }
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void StopInActivityTimeoutTimer()
        {
            log.LogMethodEntry();
            try
            {
                inActivityTimerClock.Stop();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
