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
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Booking;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmReservations : Form
    {
        Utilities utilities = POSStatic.Utilities;
        MessageUtils messageUtils = POSStatic.MessageUtils;
        TaskProcs taskProcs = POSStatic.TaskProcs;
        ParafaitEnv parafaitEnv = POSStatic.ParafaitEnv;
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmReservations()
        {
            log.LogMethodEntry();
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            utilities.setLanguage();
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            log.LogMethodExit();
        }

        private void frmReservations_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoodBookingProducts();
            LoadBookingStatusList();
            LoadFacility();
            SetupDay();
            SetupWeek();
            RefreshGrids();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void LoodBookingProducts()
        {
            log.LogMethodEntry();
            ProductsList productsListBL = new ProductsList(utilities.ExecutionContext);
            List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductsDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductsDTO.SearchByProductParameters, string>(ProductsDTO.SearchByProductParameters.PRODUCT_TYPE_NAME, "BOOKINGS"));
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
            log.LogMethodExit();
        }

        private void LoadFacility()
        {
            log.LogMethodEntry();
            FacilityList facilityListBL = new FacilityList(utilities.ExecutionContext);
            List<KeyValuePair<FacilityDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityDTO.SearchByParameters, string>>();
            facilitySearcParm.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            facilitySearcParm.Add(new KeyValuePair<FacilityDTO.SearchByParameters, string>(FacilityDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            List<FacilityDTO> facilityDTOList = facilityListBL.GetFacilityDTOList(facilitySearcParm);
            FacilityDTO firstfacilityDTO = new FacilityDTO
            {
                FacilityName = "- All -"
            };
            if (facilityDTOList != null && facilityDTOList.Count > 0)
            {
                facilityDTOList.Insert(0, firstfacilityDTO);
            }
            else
            {
                facilityDTOList = new List<FacilityDTO>();
                facilityDTOList.Add(firstfacilityDTO);
            }
            cmbFacility.DataSource = facilityDTOList;
            cmbFacility.DisplayMember = "FacilityName";
            cmbFacility.ValueMember = "FacilityId";
            cmbFacility.SelectedIndex = 0;
            log.LogMethodExit();
        }

        private void LoadBookingStatusList()
        {
            log.LogMethodEntry();
            List<Tuple<string, string>> bookingStatusList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>(ReservationDTO.ReservationStatus.BOOKED.ToString(), "Booked"),
                new Tuple<string, string>(ReservationDTO.ReservationStatus.BLOCKED.ToString(), "Blocked"),
                new Tuple<string, string>(ReservationDTO.ReservationStatus.CANCELLED.ToString(), "Cancelled"),
                new Tuple<string, string>(ReservationDTO.ReservationStatus.COMPLETE.ToString(), "Complete"),
                new Tuple<string, string>(ReservationDTO.ReservationStatus.CONFIRMED.ToString(), "Confirmed"),
                new Tuple<string, string>(ReservationDTO.ReservationStatus.WIP.ToString(), "Wip")
            };

            lbxBookingStatus.DataSource = bookingStatusList;
            lbxBookingStatus.DisplayMember = "Item2";
            lbxBookingStatus.ValueMember = "Item1";
            SelectAllForStatusList();
            log.LogMethodExit();
        }

        private void SelectAllForStatusList()
        {
            log.LogMethodEntry();
            for (int i = lbxBookingStatus.Items.Count - 1; i >= 0; i--)
            {
                lbxBookingStatus.SetSelected(i, true);
            }
            log.LogMethodExit();
        }

        void RefreshList()
        {
            log.LogMethodEntry();
            List<ReservationDTO> reservationDTOList = GetBookingRecords();
            dgvAll.DataSource = reservationDTOList;

            utilities.setupDataGridProperties(ref dgvAll);

            dgvAll.Columns["recurEndDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateCellStyle();
            dgvAll.Columns["fromDateDataGridViewTextBoxColumn"].DefaultCellStyle =
            dgvAll.Columns["toDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle(); 


            //select rr.BookingId, rr.BookingName, rr.FromDate as time_from, rr.ToDate as time_to, rr.status, 
            //                           case rr.recur_flag when 'Y' then 'Yes' else 'No' end ""Recurring?"",
            //                           case rr.recur_frequency when 'D' then 'Daily' when 'W' then 'Weekly' else '' end as recur_frequency,
            //                           rr.recur_end_date , rr.Remarks, rr.TrxId, rr.Quantity, rr.ReservationCode, rr.CardNumber, 
            //                           isnull(c.customer_name + isnull(c.last_name, ''), customerName) Customer, th.Status TrxStatus, th.TrxNetAmount
            
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

            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
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
            log.LogMethodExit();
        }

        private void ValidateSearchParameters()
        {
            log.LogMethodEntry();
            List<ValidationError> searchParamValidationErrors = new List<ValidationError>();
            if (dtpfromDate.Value.Date > dtpToDate.Value)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainer.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainer.GetMessage(utilities.ExecutionContext, "To Date")
                                                                                    , MessageContainer.GetMessage(utilities.ExecutionContext, 724));
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (cmbBookingProducts.SelectedValue == null)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainer.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Product")
                                                                                    , MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Product") + ": " + MessageContainer.GetMessage(utilities.ExecutionContext, 1787));
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (cmbFacility.SelectedValue == null)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainer.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainer.GetMessage(utilities.ExecutionContext, "Facility")
                                                                                    , MessageContainer.GetMessage(utilities.ExecutionContext, "Facility") + ": " + MessageContainer.GetMessage(utilities.ExecutionContext, 1787)); // "Please select a valid option from the list"
                searchParamValidationErrors.Add(toDateValidation);
            }
            if(lbxBookingStatus.SelectedItems.Count == 0)
            {
                ValidationError toDateValidation = new ValidationError(MessageContainer.GetMessage(utilities.ExecutionContext, "Reservation"), MessageContainer.GetMessage(utilities.ExecutionContext, "Status")
                                                                                    , MessageContainer.GetMessage(utilities.ExecutionContext, "Status") + ": " + MessageContainer.GetMessage(utilities.ExecutionContext, 1836)); // "Please select at least one booking status entry"
                searchParamValidationErrors.Add(toDateValidation);
            }
            if (searchParamValidationErrors.Count > 0)
            {
                throw new ValidationException(MessageContainer.GetMessage(utilities.ExecutionContext, "Validation"), searchParamValidationErrors);
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
            using (frmMakeReservation fm = new frmMakeReservation(DateTime.Now.Date, (decimal)DateTime.Now.Hour))
            {
                fm.ShowDialog();
            }
            RefreshGrids();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void SetupDay()
        {
            log.LogMethodEntry();
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
                reservationToDate = reservationDTOList[rows].ToDate;//(DateTime)dtWeek.Rows[rows]["time_to"];
                status = reservationDTOList[rows].Status;// dtWeek.Rows[rows]["status"].ToString();
                recur_flag = reservationDTOList[rows].RecurFlag;// dtWeek.Rows[rows]["Recurring?"].ToString();
                recur_frequency = reservationDTOList[rows].RecurFrequency;// dtWeek.Rows[rows]["recur_frequency"].ToString();
                //if (dtWeek.Rows[rows]["recur_end_date"] != DBNull.Value)
                //    recur_end_date = Convert.ToDateTime(dtWeek.Rows[rows]["recur_end_date"]);
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

                            //Begin: Commented on 18-Oct-2016
                            //Color backColor;
                            //switch (rows % 8)
                            //{
                            //    case 1: backColor = Color.LightBlue; break;
                            //    case 2: backColor = Color.LightCoral; break;
                            //    case 3: backColor = Color.LightCyan; break;
                            //    case 4: backColor = Color.LightGreen; break;
                            //    case 5: backColor = Color.LightPink; break;
                            //    case 6: backColor = Color.LightSalmon; break;
                            //    case 7: backColor = Color.LightSkyBlue; break;
                            //    default: backColor = Color.LightSteelBlue; break;
                            //}
                            //End: Commented on 18-Oct-2016

                            ToolTip tp = new ToolTip();
                            tp.IsBalloon = true;

                            //reservationDisplay.BackColor = backColor;//Commented on 18-Oct-2016
                            reservationDisplay.Text = reservation_name + " - " + origreservationFromDate.ToString("h:mm tt") + " to " + origreservationToDate.ToString("h:mm tt");

                            if (recur_flag == "Y")
                            {
                                reservationDisplay.Text += " Recurs every " + (recur_frequency == "Daily" ? "day " : "week ");
                                reservationDisplay.Text += "until " + (recur_end_date != null? Convert.ToDateTime( recur_end_date).ToString(parafaitEnv.DATE_FORMAT): "");
                            }
                            //Begin: Commented on 18-Oct-2016
                            //if (status == "CANCELLED")
                            //{
                            //    tp.SetToolTip(reservationDisplay, "CANCELLED-" + reservationDisplay.Text);
                            //    reservationDisplay.BackColor = Color.Gray;
                            //    reservationDisplay.Font = new Font(reservationDisplay.Font, FontStyle.Strikeout);
                            //}
                            //else
                            //    tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            //End: Commented on 18-Oct-2016
                            //Begin: Modified on 18-Oct-2016
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
                            //End: Modified on 18-Oct-2016

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
            log.LogMethodExit();
        }

        private List<ReservationDTO> GetBookingRecords()
        {
            log.LogMethodEntry();

            string bookingStatusValue = GetSelectedBookingStatus(); 
            ReservationListBL reservationListBL = new ReservationListBL(utilities.ExecutionContext);
            List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
            if (String.IsNullOrEmpty(txtReservationCode.Text.Trim()) == true && String.IsNullOrEmpty(txtCardNumber.Text.Trim()) == true && String.IsNullOrEmpty(txtCustomerName.Text.Trim()) == true)
            {
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, dtpfromDate.Value.Date.ToString()));
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_TO_DATE, dtpToDate.Value.Date.ToString()));
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
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.FACILITY_ID, cmbFacility.SelectedValue.ToString()));
            }
            List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParams);

            log.LogVariableState("@fromDate", dtpfromDate.Value.Date);
            log.LogVariableState("@toDate", dtpToDate.Value.Date);
            log.LogVariableState("@BookingProductId", cmbBookingProducts.SelectedValue);
            log.LogVariableState("@RCode", txtReservationCode.Text.Trim() == "" ? DBNull.Value : (object)txtReservationCode.Text);
            log.LogVariableState("@Status", bookingStatusValue);
            log.LogVariableState("@customer", txtCustomerName.Text.Trim() == "" ? DBNull.Value : (object)("%" + txtCustomerName.Text + "%"));
            log.LogVariableState("@cardNumber", txtCardNumber.Text.Trim() == "" ? DBNull.Value : (object)("%" + txtCardNumber.Text + "%"));
            log.LogVariableState("@FacilityId", cmbFacility.SelectedValue);

            // SqlDataAdapter daBookings = new SqlDataAdapter(cmd);
            //DataTable dtBookings = new DataTable();
            //daBookings.Fill(dtBookings);
            log.LogMethodExit(reservationDTOList);
            return reservationDTOList;
        }
        void ReservationDisplay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            log.LogMethodExit();
        }

        //Modified the query to get the booking details//
        void GetBookingDetails(object BookingId)
        {
            log.LogMethodEntry(BookingId);
            List<Tuple<string, string>> bookingDetails = new List<Tuple<string, string>>();
            try
            {
                ReservationListBL reservationListBL = new ReservationListBL(utilities.ExecutionContext);
                List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.BOOKING_ID, BookingId.ToString()));
                searchParams.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, "'SYSTEMABANDONED'"));
                List<ReservationDTO> reservationDTOList = reservationListBL.GetReservationDTOList(searchParams);
                if (dgvBookingDetails.Columns.Count == 0)
                {
                    // dgvBookingDetails.DataSource = bookingDetails;
                    dgvBookingDetails.Columns.Add("Detail", "Code");
                    dgvBookingDetails.Columns[0].DataPropertyName = "Item1";
                    dgvBookingDetails.Columns.Add("Data", "Data");
                    dgvBookingDetails.Columns[1].DataPropertyName = "Item2";
                    SetDGVBookingDetailsStyle();
                }

                dgvBookingDetails.DataSource = new List<Tuple<string, string>>();
                if (reservationDTOList.Count > 0)
                {
                    dgvBookingDetails.Tag = BookingId;

                    dgvBookingDetails.Columns[1].HeaderText = reservationDTOList[0].ReservationCode;// dt.Rows[0]["ReservationCode"].ToString();
                    //dgvBookingDetails.Rows.Add("Code", dt.Rows[0]["ReservationCode"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Status"), reservationDTOList[0].Status));
                    //dgvBookingDetails.Rows.Add("Status", dt.Rows[0]["Status"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Date"), reservationDTOList[0].FromDate.Date.ToString(parafaitEnv.DATE_FORMAT)));
                    //dgvBookingDetails.Rows.Add("Booking Date", Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString(ParafaitEnv.DATE_FORMAT));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Facility Name"), reservationDTOList[0].FacilityName));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Customer Name"), reservationDTOList[0].CustomerName));
                    //dgvBookingDetails.Rows.Add("Customer Name", dt.Rows[0]["CustomerName"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Name"), reservationDTOList[0].BookingName));
                    //dgvBookingDetails.Rows.Add("Booking Name", dt.Rows[0]["BookingName"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "From Time"), reservationDTOList[0].FromDate.ToString("h:mm tt")));
                    //dgvBookingDetails.Rows.Add("From Time", Convert.ToDateTime(dt.Rows[0]["FromDate"]).ToString("h:mm tt"));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "To Time"), reservationDTOList[0].ToDate.ToString("h:mm tt")));
                    //dgvBookingDetails.Rows.Add("To Time", Convert.ToDateTime(dt.Rows[0]["ToDate"]).ToString("h:mm tt"));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Product"), reservationDTOList[0].BookingProductName.ToString()));
                    //dgvBookingDetails.Rows.Add("Booking Class", dt.Rows[0]["product_name"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Guests"), reservationDTOList[0].Quantity.ToString(parafaitEnv.NUMBER_FORMAT)));
                    //dgvBookingDetails.Rows.Add("Guests", dt.Rows[0]["Quantity"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Contact No"), reservationDTOList[0].ContactNo));
                    //dgvBookingDetails.Rows.Add("Contact No", dt.Rows[0]["ContactNo"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Email"), reservationDTOList[0].Email));
                    //dgvBookingDetails.Rows.Add("Email", dt.Rows[0]["Email"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Remarks"), reservationDTOList[0].Remarks));
                    //dgvBookingDetails.Rows.Add("Remarks", dt.Rows[0]["Remarks"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Card"), reservationDTOList[0].CardNumber));
                    //dgvBookingDetails.Rows.Add("Card", dt.Rows[0]["CardNumber"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Channel"), reservationDTOList[0].Channel));
                    //dgvBookingDetails.Rows.Add("Channel", dt.Rows[0]["Channel"]);
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Expires"), (reservationDTOList[0].ExpiryTime == null ? "" : Convert.ToDateTime(reservationDTOList[0].ExpiryTime).ToString(parafaitEnv.DATETIME_FORMAT))));
                    //dgvBookingDetails.Rows.Add("Expires", dt.Rows[0]["ExpiryTime"] == DBNull.Value ? "" : Convert.ToDateTime(dt.Rows[0]["ExpiryTime"]).ToString(ParafaitEnv.DATETIME_FORMAT));
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Alternate No"), reservationDTOList[0].AlternateContactNo));
                    //dgvBookingDetails.Rows.Add("Alternate No", dt.Rows[0]["AlternateContactNo"]); 
                    //dgvBookingDetails.Rows.Add("Recurs?", dt.Rows[0]["recur_flag"].ToString() == "Y" ? "Yes" : "No");
                    //dgvBookingDetails.Rows.Add("Recur Frequency", dt.Rows[0]["recur_frequency"].ToString() == "W" ? "Weekly" : "Daily");
                    //dgvBookingDetails.Rows.Add("Recur End Date", dt.Rows[0]["recur_end_date"] == DBNull.Value ? (object)"" : Convert.ToDateTime(dt.Rows[0]["recur_end_date"]).ToString(ParafaitEnv.DATE_FORMAT));
                    //dgvBookingDetails.Rows.Add("Email Sent?", dt.Rows[0]["isEmailSent"].ToString() == "Y" ? "Yes" : "No");
                    bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Email Sent?"), reservationDTOList[0].IsEmailSent > 0 ? "Yes" : "No"));
                    Semnox.Parafait.Transaction.Reservation reservation = new Semnox.Parafait.Transaction.Reservation(Convert.ToInt32(BookingId), utilities);

                    if (reservation.Transaction != null)
                    {
                        //dgvBookingDetails.Rows.Add("Estimated Amount", reservation.Transaction.Net_Transaction_Amount.ToString(ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL));
                        bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Estimated Amount"), reservation.Transaction.Net_Transaction_Amount.ToString(parafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)));
                    } 
                    DataTable dtp = reservation.GetPurchasedPackageProducts(Convert.ToInt32(BookingId));
                    if (dtp.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtp.Rows.Count; i++)
                        {
                            //dgvBookingDetails.Rows.Add("Booking Product Contents", dtp.Rows[i]["product_name"].ToString());
                            bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Booking Product Contents"), dtp.Rows[i]["product_name"].ToString() + "( " + Convert.ToInt32(dtp.Rows[i]["quantity"]).ToString(parafaitEnv.NUMBER_FORMAT) + " )"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //dgvBookingDetails.Rows.Add("Exception", ex.Message);
                bookingDetails.Add(new Tuple<string, string>(MessageContainer.GetMessage(utilities.ExecutionContext, "Exception"), ex.Message));
                log.Error(ex);
            }
            dgvBookingDetails.DataSource = bookingDetails.ToList();
            SetDGVBookingDetailsStyle();
            log.LogMethodExit();
        }

        void ReservationDisplay_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (Convert.ToInt32(((Label)sender).Tag) == -1)
            {
                contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
            }
            else
            {
                LoadBookingForm(Convert.ToInt32(((Label)sender).Tag));
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
            RefreshGrids();
            log.LogMethodExit();
        }

        void RefreshGrids()
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
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
            catch (ValidationException ex)
            {
                string validationMsg = ex.GetAllValidationErrorMessages();
                string errorMsg = String.IsNullOrEmpty(validationMsg) == true ? ex.Message : validationMsg;
                POSUtils.ParafaitMessageBox(errorMsg);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void dgvWeek_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CreateNew();
            log.LogMethodExit();
        }

        private void dgvAll_DoubleClick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
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
            if (e.ClickedItem == menuNewBooking)
            {
                CreateNew();
            }
            log.LogMethodExit();
        }

        private void CreateNew()
        {
            log.LogMethodEntry();
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
            if (FromTime.Date >= DateTime.Now.Date && (FromTime.Hour + (decimal)FromTime.Minute / 100) >= ((decimal)DateTime.Now.Hour + (decimal)(DateTime.Now.Minute - 5) / 100))
            {
                using (Form f = new frmMakeReservation(FromTime.Date, FromTime.Hour + (decimal)FromTime.Minute / 100))
                {
                    f.ShowDialog();
                }
                RefreshGrids();
            }
            log.LogMethodExit();
        }

        private void btnPrevWeek_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            log.LogMethodExit();
        }

        private void btnNextWeek_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
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
            log.LogMethodExit();
        }

        private void dgvAll_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvAll.CurrentRow != null)
            {
                GetBookingDetails(dgvAll.CurrentRow.Cells["bookingIdDataGridViewTextBoxColumn"].Value);
            }
            log.LogMethodExit();
        }

        private void lnkClearSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtCardNumber.Text = txtCustomerName.Text = txtReservationCode.Text = "";
            //cmbStatus.SelectedIndex = 0;
            SelectAllForStatusList();
            cmbBookingProducts.SelectedIndex = 0;
            cmbFacility.SelectedIndex = 0;
            dtpfromDate.Value = DateTime.Now.Date;
            dtpToDate.Value = dtpfromDate.Value.AddDays(1);
            tabCalendar.SelectedTab = tabPageDay;
            RefreshGrids();
            log.LogMethodExit();
        }

        private void lnkExcelDownload_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            utilities.ExportToExcel(dgvAll, "Bookings for " + dtpfromDate.Value.Date.ToString("dd-MMM-yyyy"), "Bookings for " + dtpfromDate.Value.Date.ToString("dd-MMM-yyyy"), parafaitEnv.SiteName, dtpfromDate.Value.Date, dtpToDate.Value.Date);
            log.LogMethodExit();
        }

        private void dgvDay_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            cellPaint(sender, e);
            log.LogMethodExit();
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
            DateTime prevDate = DateTime.MinValue;
            int offset = 0;

            for (int rows = 0; rows < reservationDTOList.Count; rows++)
            {
                reservation_id = reservationDTOList[rows].BookingId;// Convert.ToInt64(dtDay.Rows[rows]["BookingId"]);
                reservation_name = reservationDTOList[rows].BookingName;// dtDay.Rows[rows]["BookingName"].ToString();
                reservationFromDate = reservationDTOList[rows].FromDate;// (DateTime)dtDay.Rows[rows]["time_from"];
                reservationToDate = reservationDTOList[rows].ToDate;// (DateTime)dtDay.Rows[rows]["time_to"];
                status = reservationDTOList[rows].Status;// dtDay.Rows[rows]["status"].ToString();
                recur_flag = reservationDTOList[rows].RecurFlag;// dtDay.Rows[rows]["Recurring?"].ToString();
                recur_frequency = reservationDTOList[rows].RecurFrequency;// dtDay.Rows[rows]["recur_frequency"].ToString();
                //if (dtDay.Rows[rows]["recur_end_date"] != DBNull.Value)
                //    recur_end_date = Convert.ToDateTime(dtDay.Rows[rows]["recur_end_date"]);
                recur_end_date = reservationDTOList[rows].RecurEndDate;

                if (prevDate != reservationFromDate.Date)
                {
                    offset = 0;
                    prevDate = reservationFromDate.Date;
                }
                else
                {
                    offset += 150;
                    if (offset > dgvDay.Columns[0].Width - 150)
                        offset = 50;
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

                        if ((recur_end_date != null && recur_end_date >= cellTimeRecur.Date )&& origreservationFromDate.Date <= cellTimeRecur.Date) // check if recur has ended
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

                            //Begin: Commented on 18-Oct-2016
                            // Color backColor;
                            //switch (rows % 8)
                            //{
                            //    case 1: backColor = Color.LightBlue; break;
                            //    case 2: backColor = Color.LightCoral; break;
                            //    case 3: backColor = Color.LightCyan; break;
                            //    case 4: backColor = Color.LightGreen; break;
                            //    case 5: backColor = Color.LightPink; break;
                            //    case 6: backColor = Color.LightSalmon; break;
                            //    case 7: backColor = Color.LightSkyBlue; break;
                            //    default: backColor = Color.LightSteelBlue; break;
                            //}
                            //End: Commented on 18-Oct-2016

                            ToolTip tp = new ToolTip();
                            tp.IsBalloon = true;

                            //Begin: Commented on 18-Oct-2016
                            //reservationDisplay.BackColor = backColor;x
                            reservationDisplay.Text = reservation_name + " - " + origreservationFromDate.ToString("h:mm tt") + " to " + origreservationToDate.ToString("h:mm tt");

                            if (recur_flag == "Y")
                            {
                                reservationDisplay.Text += " " + MessageContainer.GetMessage(utilities.ExecutionContext, "Recurs every") + " " + (recur_frequency == "Daily" ? MessageContainer.GetMessage(utilities.ExecutionContext, "Day") + " " : MessageContainer.GetMessage(utilities.ExecutionContext, "Week") + " ");
                                reservationDisplay.Text += MessageContainer.GetMessage(utilities.ExecutionContext, "until") + " " + (recur_end_date != null? Convert.ToDateTime(recur_end_date).ToString(parafaitEnv.DATE_FORMAT): "");
                            }

                            //Begin: Commented on 18-Oct-2016
                            //if (status == "CANCELLED")
                            //{
                            //    tp.SetToolTip(reservationDisplay, "CANCELLED-" + reservationDisplay.Text);
                            //    reservationDisplay.BackColor = Color.Gray;
                            //    reservationDisplay.Font = new Font(reservationDisplay.Font, FontStyle.Strikeout);
                            //}
                            //else 
                            //    tp.SetToolTip(reservationDisplay, reservationDisplay.Text);
                            //End: Commented on 18-Oct-2016
                            //Begin: Modified on 18-Oct-2016
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
                            //End: Modified on 18-Oct-2016    

                            reservationDisplay.Tag = reservation_id;
                            reservationDisplay.Click += new EventHandler(ReservationDisplay_Click);
                            reservationDisplay.DoubleClick += new EventHandler(ReservationDisplay_DoubleClick);

                            if (maxRow == minRow)
                                numrows = 1;
                            else
                                numrows = maxRow - minRow + 1;

                            reservationDisplay.Size = new Size(150, dgvDay.Rows[0].Height * numrows);
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
            log.LogMethodExit();
        }

        private void dgvDay_MouseClick(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
            }
            log.LogMethodExit();
        }

        private void dgvDay_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            CreateNew();
            log.LogMethodExit();
        }

        private void SetDGVBookingDetailsStyle()
        {
            log.LogMethodEntry();
            dgvBookingDetails.RowHeadersVisible = false;
            dgvBookingDetails.Columns[0].HeaderText = MessageContainer.GetMessage(utilities.ExecutionContext, "Code");
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
                lnkExcelDownload.Visible = true;
            }
            else
            {
                btnPrevWeek.Enabled = true;
                btnNextWeek.Enabled = true;
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
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }

        private void dtpToDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            log.LogMethodExit();
        }


        private void BlackButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal1;
            log.LogMethodExit();
        }

        private void BlackButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed1;
            log.LogMethodExit();
        }

        private void BlueButtonMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void BlueButtonMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private string GetSelectedBookingStatus()
        {
            log.LogMethodEntry();
            StringBuilder statusValues = new StringBuilder();
            if (lbxBookingStatus.SelectedItems.Count == lbxBookingStatus.Items.Count)
            {
                statusValues.Append("ALL");
                statusValues.Append(",");
            }
            else
            {
                foreach (Tuple<string, string> element in lbxBookingStatus.SelectedItems)
                {
                    statusValues.Append("'"+element.Item1);
                    statusValues.Append("',");
                }
            }
            string statusValue = statusValues.ToString();
            statusValue = statusValue.Remove(statusValue.LastIndexOf(","), 1);

            log.LogMethodExit(statusValue);
            return statusValue;
        }

        private void dgvBookingDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvBookingDetails != null && Convert.ToInt32(dgvBookingDetails.Tag) != -1)
            {
                LoadBookingForm(Convert.ToInt32(dgvBookingDetails.Tag));
            }
            log.LogMethodExit();
        }

        private void dgvBookingDetails_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvBookingDetails != null && Convert.ToInt32(dgvBookingDetails.Tag) != -1)
            {
                LoadBookingForm(Convert.ToInt32(dgvBookingDetails.Tag));
            }
            log.LogMethodExit();
        }

        private void LoadBookingForm(int bookingId)
        {
            log.LogMethodEntry(bookingId);
            using (frmMakeReservation f = new frmMakeReservation(bookingId))
            {
                f.ShowDialog();
            }
            RefreshGrids();
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
            Parafait_POS.POSUtils.AttachFormEvents();
            log.LogMethodExit(); 
        }

        private void btnForwardMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Forward_Btn_Hover;
            log.LogMethodExit();
        }

        private void btnForwardMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.ForwardBtn;
            log.LogMethodExit();
        }

        private void btnBackwardMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.Backward_Btn_Hover;
            log.LogMethodExit();
        }

        private void btnBackwardMouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.BackwardBtn;
            log.LogMethodExit();
        }
    }
}
