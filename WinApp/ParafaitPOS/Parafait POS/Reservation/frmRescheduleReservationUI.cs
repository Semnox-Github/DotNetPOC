/* Description  - Reschedule Reservation form
*
**************
** Version Log
 **************
 * Version     Date          Modified By         Remarks
*********************************************************************************************
*2.80.0        05-Jun-2020   Guru S A            Created for reservation enhancements 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
*********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmRescheduleReservationUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReservationBL reservationBL;
        private ExecutionContext executionContext = null;
        private Utilities utilities = null;
        private bool userAction = false;
        private ReservationDefaultSetup reservationDefaultSetup;
        private decimal fromTimeForSearch;
        private decimal toTimeForSearch;
        private int facilityIdForSearch;
        private decimal lastTimeSlotForTheDay = (decimal)23.55;
        private DataTable dtFromTime = new DataTable();
        private DataTable dtToTime = new DataTable();
        private BindingSource bsFromTime = new BindingSource();
        private BindingSource bsToTime = new BindingSource();
        private Transaction.TransactionLine bookingProductScheduleTrxLine = null;
        private Transaction.TransactionLine bookingProductTrxLine = null;
        public frmRescheduleReservationUI(ExecutionContext executionContext, Utilities utilities, ReservationBL reservationBL)
        {
            log.LogMethodEntry(executionContext, utilities, reservationBL);
            this.reservationBL = reservationBL;
            this.executionContext = executionContext;
            this.utilities = utilities;
            this.reservationDefaultSetup = new ReservationDefaultSetup(executionContext);
            InitUIElements();
            SetHeaderLabelText();
            this.utilities.setLanguage();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void InitUIElements()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                userAction = false;
                InitializeComponent();
                utilities.setLanguage();
                //SetscheduleDTOMasterList(); 
                LoadFacility();
                LoadTimeDropdownDropDown();
                SetStyleForDGV();
                //virtualKeyboard = new VirtualKeyboardController();
                //virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad, btnPkgTabShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { pnlCustomerDetail, txtServiceChargeAmount, txtServiceChargePercentage });
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                userAction = true;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }


        private void LoadFacility()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
            List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
            facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.HAVING_PRODUCT_TYPES_IN, ProductTypeValues.BOOKINGS));
            List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(facilitySearcParm);
            FacilityMapDTO firstfacilityMapDTO = new FacilityMapDTO
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
            facilityMapDTOList.Insert(0, firstfacilityMapDTO);

            cmbSearchFacility.DataSource = facilityMapDTOList;
            cmbSearchFacility.DisplayMember = "FacilityMapName";
            cmbSearchFacility.ValueMember = "FacilityMapId";
            cmbSearchFacility.SelectedIndex = 0;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetStyleForDGV()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvSearchSchedules);
            dgvSearchSchedules.Columns["scheduleNameDataGridViewTextBoxColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; ;
            SetDGVCellFont(dgvSearchSchedules);
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

        private void SetHeaderLabelText()
        {
            log.LogMethodEntry();
            try
            {
                this.lblOldFacilityMapValue.Text = string.Empty;
                this.lblBookingProductName.Text = reservationBL.GetReservationDTO.BookingProductName;
                string scheduleDetails = reservationBL.GetReservationDTO.FromDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT)
                                        + " " + MessageContainerList.GetMessage(executionContext, "to")
                                        + " " + reservationBL.GetReservationDTO.ToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                this.lblBookedScheduleDetails.Text = scheduleDetails;
                bookingProductTrxLine = this.reservationBL.GetBookingProductTransactionLine();
                GetBookingProductSchedule();
                if (bookingProductScheduleTrxLine != null)
                {
                    decimal? guestQty = bookingProductScheduleTrxLine.GetLineReservationGuestQuantity();
                    if (guestQty == null)
                    {
                        guestQty = 0;
                    }
                    this.lblQtyVallue.Text = (guestQty == 0 ? "0" : ((decimal)guestQty).ToString(utilities.ParafaitEnv.NUMBER_FORMAT));
                    this.lblOldFacilityMapValue.Text = bookingProductScheduleTrxLine.GetCurrentTransactionReservationScheduleDTO().FacilityMapName;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void GetBookingProductSchedule()
        {
            log.LogMethodEntry();
            List<Transaction.TransactionLine> transactionScheduleLines = this.reservationBL.GetScheduleTransactionLines();
            if (transactionScheduleLines != null && transactionScheduleLines.Any())
            {
                bookingProductScheduleTrxLine = transactionScheduleLines.Find(tl => tl.LineValid && tl.CancelledLine == false && tl == bookingProductTrxLine.ParentLine);
            }
            log.LogMethodExit();
        }

        private void frmRescheduleReservationUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            SetDefaultSearchParameters();
            LoadSchedules();
            this.utilities.setLanguage(this);
            ActiveControl = btnCancel;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
        private void dtpSearchDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void dtpSearchDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void cmbSearchFacilityProd_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                LoadSchedules();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void SetDefaultSearchParameters()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                userAction = false;
                DateTime startDate = ServerDateTime.Now;
                if (this.reservationBL.GetReservationDTO != null)
                {
                    startDate = this.reservationBL.GetReservationDTO.FromDate;
                }
                DateTime pDate = startDate.Date;
                decimal pfromTime = startDate.Hour;
                int minTimeSlot = this.reservationDefaultSetup.GetCalendarTimeSlotGap;
                dtpSearchDate.Value = pDate;
                if (minTimeSlot <= 15)
                {
                    if (pfromTime > ((decimal)(23.60) - (decimal)(minTimeSlot * 3) / 100) && pfromTime <= (decimal)24)
                        pfromTime = ((decimal)(23.60) - (decimal)(minTimeSlot * 3) / 100);
                }
                else
                {
                    if (pfromTime > ((decimal)(23.60) - (decimal)(minTimeSlot) / 100) && pfromTime <= (decimal)24)
                        pfromTime = GetLastFromHourSlot(pfromTime, minTimeSlot);
                }
                pfromTime = GetFromHourSlot(pfromTime, minTimeSlot);
                cmbSearchFromTime.SelectedValue = pfromTime;
                //Load from time + 2 hrs to  totime
                decimal fromTime = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);
                int hourSlot = GetHourSlot(minTimeSlot);
                int totToMins = (int)(Math.Floor(fromTime) * 60 + (fromTime - Math.Floor(fromTime)) * 100 + hourSlot);
                decimal toTime = totToMins / 60 + (decimal)((totToMins % 60) / 100.0);
                if (toTime > ((decimal)23.60 - (decimal)(minTimeSlot) / 100))
                    toTime = GetLastToHourSlot(pfromTime, minTimeSlot);

                cmbSearchToTime.SelectedValue = toTime;
                fromTime = pfromTime;
                SetFacilityMapDropdownList();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                userAction = true;
            }
            log.LogMethodExit();
        }

        private void SetFacilityMapDropdownList()
        {
            log.LogMethodEntry();
            if (bookingProductScheduleTrxLine != null)
            {
                TransactionReservationScheduleDTO transactionReservationScheduleDTO = bookingProductScheduleTrxLine.GetCurrentTransactionReservationScheduleDTO();
                if (transactionReservationScheduleDTO != null)
                {
                    cmbSearchFacility.SelectedValue = transactionReservationScheduleDTO.FacilityMapId;
                }
            }
            log.LogMethodExit();
        }

        private void LoadTimeDropdownDropDown()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            dtFromTime.Columns.Add("Display");
            dtFromTime.Columns.Add("Value");
            dtFromTime.Columns.Add(new DataColumn("Compare", typeof(DateTime)));

            dtToTime.Columns.Add("Display");
            dtToTime.Columns.Add("Value");

            DateTime startTime = utilities.getServerTime().Date;
            while (startTime < utilities.getServerTime().Date.AddDays(1))
            {
                dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
            }
            cmbSearchFromTime.DisplayMember = "Display";
            cmbSearchFromTime.ValueMember = "Value";
            cmbSearchFromTime.DataSource = dtFromTime;
            cmbSearchToTime.DisplayMember = "Display";
            cmbSearchToTime.ValueMember = "Value";
            cmbSearchToTime.DataSource = dtToTime;

            cmbFromTime.DisplayMember = "Display";
            cmbFromTime.ValueMember = "Value";
            bsFromTime.DataSource = dtFromTime;
            cmbFromTime.DataSource = bsFromTime;
            cmbToTime.DisplayMember = "Display";
            cmbToTime.ValueMember = "Value";
            bsToTime.DataSource = dtToTime;
            cmbToTime.DataSource = bsToTime;

            lastTimeSlotForTheDay = Convert.ToDecimal(dtToTime.Rows[dtToTime.Rows.Count - 1]["Value"]);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private decimal GetLastFromHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = pfromTime;
            DateTime startTime = utilities.getServerTime().Date.AddMinutes((int)pfromTime * 60 + (double)pfromTime % 1 * 100);
            while (startTime.Date == utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            if (startTime.Date > utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }


        private decimal GetFromHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = 0;
            DateTime startTime = utilities.getServerTime().Date;
            while (hourSlot < pfromTime)
            {
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            if (hourSlot > pfromTime)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }

        private int GetHourSlot(int minTimeSlot)
        {
            log.LogMethodEntry(minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            int hourSlot = 0;
            while (hourSlot < 120)
            {
                hourSlot = hourSlot + minTimeSlot;
            }
            if (hourSlot > 120)
            {
                hourSlot = hourSlot - minTimeSlot;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }
        private decimal GetLastToHourSlot(decimal pfromTime, int minTimeSlot)
        {
            log.LogMethodEntry(pfromTime, minTimeSlot);
            POSUtils.SetLastActivityDateTime();
            decimal hourSlot = pfromTime;
            DateTime startTime = utilities.getServerTime().Date.AddMinutes((int)pfromTime * 60 + (double)pfromTime % 1 * 100);
            while (startTime.Date == utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            if (startTime.Date > utilities.getServerTime().Date)
            {
                startTime = startTime.AddMinutes(-this.reservationDefaultSetup.GetCalendarTimeSlotGap);
                hourSlot = (decimal)(startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit(hourSlot);
            return hourSlot;
        }


        private void LoadSchedules()
        {
            log.LogMethodEntry(userAction);
            try
            {
                if (userAction)
                {
                    POSUtils.SetLastActivityDateTime();
                    this.Cursor = Cursors.WaitCursor;
                    ValidateSearchParameters();
                    SetSearchDateParameters();
                    facilityIdForSearch = (int)cmbSearchFacility.SelectedValue;
                    List<ScheduleDetailsDTO> scheduleDetailsDTOList = GetElligileSchedules();
                    dgvSearchSchedules.DataSource = scheduleDetailsDTOList;
                    //if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0 && dgvSearchSchedules.CurrentRow != null)
                    //{
                    //    UpdateDGVSearchScheduleAvailableUnit();
                    //    int rowIndex = dgvSearchSchedules.CurrentRow.Index;
                    //    if (rowIndex >= 0)
                    //    {
                    //        DoDGVSearchScheduleRowEnterOperation(rowIndex);
                    //    }
                    //}
                    SetCellReadOnlyPropertyForFromAndToTime();
                }
            }
            catch (ValidationException ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.GetAllValidationErrorMessages());
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void SetCellReadOnlyPropertyForFromAndToTime()
        {
            log.LogMethodEntry();
            if (dgvSearchSchedules.Rows.Count > 0)
            {
                try
                {
                    userAction = false;
                    for (int i = 0; i < dgvSearchSchedules.Rows.Count; i++)
                    {

                        decimal scheduleFromDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[i].Cells["ScheduleFromTime"].Value);
                        decimal scheduleToDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[i].Cells["ScheduleToTime"].Value);
                        bool fixedSchedule = Convert.ToBoolean(dgvSearchSchedules.Rows[i].Cells["FixedSchedule"].Value);
                        dgvSearchSchedules.Rows[i].Cells["cmbFromTime"].Value = scheduleFromDateTime;
                        dgvSearchSchedules.Rows[i].Cells["cmbToTime"].Value = scheduleToDateTime;
                        if (fixedSchedule)
                        {
                            dgvSearchSchedules.Rows[i].Cells["cmbFromTime"].ReadOnly = true;
                            dgvSearchSchedules.Rows[i].Cells["cmbToTime"].ReadOnly = true;
                        }
                        else
                        {
                            dgvSearchSchedules.Rows[i].Cells["cmbFromTime"].ReadOnly = false;
                            dgvSearchSchedules.Rows[i].Cells["cmbToTime"].ReadOnly = false;
                        }
                    }
                }
                finally
                {
                    userAction = true;
                }
            }
            log.LogMethodExit();
        }

        private void ValidateSearchParameters()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (cmbSearchFacility.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "Facility"),
                                                            MessageContainerList.GetMessage(executionContext, 694)));
            }
            if (cmbSearchFromTime.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                             MessageContainerList.GetMessage(executionContext, "From Time"),
                                                             MessageContainerList.GetMessage(executionContext, "Select From Time")));
            }
            if (cmbSearchToTime.SelectedValue == null)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                             MessageContainerList.GetMessage(executionContext, "To Time"),
                                                             MessageContainerList.GetMessage(executionContext, "Select To Time")));
            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            log.LogMethodExit();
        }

        private void SetSearchDateParameters()
        {
            log.LogMethodEntry();
            bool fromDateIsSet = false;
            if (cbxEarlyMorningSlot.Checked)
            {
                fromDateIsSet = true;
                fromTimeForSearch = 0;
                toTimeForSearch = 6;
            }
            if (cbxMorningSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 6;
                }
                toTimeForSearch = 12;
            }
            if (cbxAfternoonSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 12;
                }
                toTimeForSearch = 18;
            }
            if (cbxNightSlot.Checked)
            {
                if (fromDateIsSet == false)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = 18;
                }
                toTimeForSearch = 24;
            }
            if (fromDateIsSet == false)
            {
                if (cmbSearchFromTime.SelectedValue != null && cmbSearchFromTime.SelectedIndex != 0)
                {
                    fromDateIsSet = true;
                    fromTimeForSearch = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);

                    if (Convert.ToDouble(cmbSearchToTime.SelectedValue) > Convert.ToDouble(cmbSearchFromTime.SelectedValue))
                    {
                        toTimeForSearch = Convert.ToDecimal(cmbSearchToTime.SelectedValue);
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 305),
                                                      MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                      MessageContainerList.GetMessage(executionContext, "From Time"),
                                                      MessageContainerList.GetMessage(executionContext, 305));
                    }
                }
            }
            if (fromDateIsSet == false)
            {
                fromTimeForSearch = utilities.getServerTime().Hour;
                toTimeForSearch = 24;
            }
            log.LogMethodExit();
        }


        private List<ScheduleDetailsDTO> GetElligileSchedules()
        {
            log.LogMethodEntry(facilityIdForSearch);
            POSUtils.SetLastActivityDateTime();
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = ((Parafait_POS.POS)Application.OpenForms["POS"]).GetEligibleSchedules(dtpSearchDate.Value.Date,
                                                                                                                                     fromTimeForSearch, toTimeForSearch,
                                                                                                                                     facilityIdForSearch, this.bookingProductTrxLine.ProductID, -1);

            POSUtils.SetLastActivityDateTime();
            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
                {
                    if (scheduleDetailsDTOList[i].ScheduleFromDate < utilities.getServerTime().AddMinutes(-this.reservationDefaultSetup.GetGracePeriodForFixedSchedule) && scheduleDetailsDTOList[i].FixedSchedule == true
                        || scheduleDetailsDTOList[i].ScheduleFromDate < utilities.getServerTime().Date && scheduleDetailsDTOList[i].FixedSchedule == false)
                    {
                        scheduleDetailsDTOList.RemoveAt(i);
                        i = -1;
                    }
                }
            }

            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Count > 0)
            {
                for (int i = 0; i < scheduleDetailsDTOList.Count; i++)
                {
                    POSUtils.SetLastActivityDateTime();
                    double Price = (scheduleDetailsDTOList[i].Price == null ? 0 : (double)scheduleDetailsDTOList[i].Price);
                    if (Price == 0)
                    {
                        Price = (scheduleDetailsDTOList[i].AttractionPlayPrice == null ? 0 : (double)scheduleDetailsDTOList[i].AttractionPlayPrice);
                    }

                    scheduleDetailsDTOList[i].Price = Price;

                    int? totalUnits = 0;
                    totalUnits = scheduleDetailsDTOList[i].TotalUnits;

                    int bookedUnits = 0;
                    FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, scheduleDetailsDTOList[i].FacilityMapDTO);
                    if (scheduleDetailsDTOList[i].FixedSchedule)
                    {
                        bookedUnits = facilityMapBL.GetTotalBookedUnitsForReservation(scheduleDetailsDTOList[i].ScheduleFromDate, scheduleDetailsDTOList[i].ScheduleToDate);
                        scheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                    }
                    else
                    {
                        scheduleDetailsDTOList[i].AvailableUnits = totalUnits - bookedUnits;
                    }
                    if (scheduleDetailsDTOList[i].AvailableUnits < 1)
                    {
                        scheduleDetailsDTOList.RemoveAt(i);
                        i = -1;
                    }
                }
            }
            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        private void cbxTimeSlots_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            SetTimeSearchListValues();
            LoadSchedules();
            log.LogMethodExit();
        }

        private void cmbSearchFromTimeORToTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            ResetTimeSlots();
            LoadSchedules();
            log.LogMethodExit();
        }

        private void SetTimeSearchListValues()
        {
            log.LogMethodEntry();
            if (userAction)
            {
                bool fromDateIsSet = false;
                decimal fromTimeSlot = 0;
                decimal toTimeSlot = lastTimeSlotForTheDay;
                decimal currentFromTimeSlot = 0;
                decimal currentToTimeSlot = lastTimeSlotForTheDay;
                if (cmbSearchFromTime.SelectedValue != null)
                {
                    fromTimeSlot = Convert.ToDecimal(cmbSearchFromTime.SelectedValue);
                    currentFromTimeSlot = fromTimeSlot;
                    if (cmbSearchToTime.SelectedValue != null && Convert.ToDouble(cmbSearchToTime.SelectedValue) > Convert.ToDouble(cmbSearchFromTime.SelectedValue))
                    {
                        toTimeSlot = Convert.ToDecimal(cmbSearchToTime.SelectedValue);
                        currentToTimeSlot = toTimeSlot;
                    }
                }
                userAction = false;
                try
                {

                    if (cbxEarlyMorningSlot.Checked)
                    {
                        fromTimeSlot = 0;
                        fromDateIsSet = true;
                        toTimeSlot = 6;
                    }
                    if (cbxMorningSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 6;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = 12;
                    }
                    if (cbxAfternoonSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 12;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = 18;
                    }
                    if (cbxNightSlot.Checked)
                    {
                        if (fromDateIsSet == false)
                        {
                            fromTimeSlot = 18;
                            fromDateIsSet = true;
                        }
                        toTimeSlot = lastTimeSlotForTheDay;
                    }

                    if (fromTimeSlot != currentFromTimeSlot || toTimeSlot != currentToTimeSlot)
                    {
                        cmbSearchFromTime.SelectedValue = fromTimeSlot;
                        cmbSearchToTime.SelectedValue = toTimeSlot;
                    }
                }
                finally
                {
                    userAction = true;
                }
            }
            log.LogMethodExit();

        }
        private void ResetTimeSlots()
        {
            log.LogMethodEntry();
            if (userAction)
            {
                userAction = false;
                try
                {
                    if (cbxEarlyMorningSlot.Checked)
                        cbxEarlyMorningSlot.Checked = false;
                    if (cbxMorningSlot.Checked)
                        cbxMorningSlot.Checked = false;
                    if (cbxAfternoonSlot.Checked)
                        cbxAfternoonSlot.Checked = false;
                    if (cbxNightSlot.Checked)
                        cbxNightSlot.Checked = false;
                }
                finally
                {
                    userAction = true;
                }
            }
            log.LogMethodExit();
        }

        private void dgvSearchSchedules_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (dgvSearchSchedules.CurrentCell != null & e.Value != null)
                {
                    if (dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbToTime")
                    {
                        decimal decimalCheckValue = 0;
                        if (decimal.TryParse(e.Value.ToString(), out decimalCheckValue))
                        {
                            string selectCondtion = "value = " + e.Value.ToString();
                            DataRow foundRows = GetMatchingRow(e.Value);
                            if (foundRows != null)
                                e.Value = foundRows["Display"];
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        private void dgvSearchSchedules_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (dgvSearchSchedules.CurrentCell != null & userAction && e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbToTime")
                    {
                        bool fromTimeField = (dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbFromTime");
                        if (ValidateUserSelectedTime(e, fromTimeField) == false)
                        {
                            ResetUserSelectedTime(e, fromTimeField);
                            dgvSearchSchedules.CurrentCell = dgvSearchSchedules.Rows[e.RowIndex].Cells[e.ColumnIndex];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private bool ValidateUserSelectedTime(DataGridViewCellEventArgs e, bool fromTimeField)
        {
            log.LogMethodEntry(e, fromTimeField);
            bool validTimeValue = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int trsScheduleId = Convert.ToInt32(dgvSearchSchedules.Rows[e.RowIndex].Cells["scheduleIdDataGridViewTextBoxColumn"].Value);
                decimal scheduleFromDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["ScheduleFromTime"].Value);
                decimal scheduleToDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["ScheduleToTime"].Value);
                decimal fromtimeValue = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbFromTime"].Value);
                decimal toTimeValue = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbToTime"].Value);
                bool fixedSchedule = Convert.ToBoolean(dgvSearchSchedules.Rows[e.RowIndex].Cells["FixedSchedule"].Value);

                if (fromTimeField)
                {
                    validTimeValue = (fixedSchedule == false && fromtimeValue >= scheduleFromDateTime && fromtimeValue <= scheduleToDateTime);
                }
                else
                {
                    validTimeValue = (fixedSchedule == false && toTimeValue >= scheduleFromDateTime && toTimeValue <= scheduleToDateTime);
                }
                if (validTimeValue)
                {
                    if (fromtimeValue > toTimeValue)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 305));
                    }
                }
                else
                {
                    string fromTimeString = "";
                    string toTimeString = "";
                    DataRow foundRows = GetMatchingRow(scheduleFromDateTime);
                    if (foundRows != null)
                        fromTimeString = foundRows["Display"].ToString();

                    foundRows = GetMatchingRow(scheduleToDateTime);
                    if (foundRows != null)
                        toTimeString = foundRows["Display"].ToString();

                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2105, fromTimeString, toTimeString));
                    //"Time should be with in " + fromTimeString + " and " + toTimeString
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
                validTimeValue = false;
            }
            this.Cursor = Cursors.Default;
            log.LogMethodExit(validTimeValue);
            return validTimeValue;
        }


        private void ResetUserSelectedTime(DataGridViewCellEventArgs e, bool fromTimeField)
        {
            log.LogMethodEntry(e, fromTimeField);
            this.Cursor = Cursors.WaitCursor;
            try
            {
                userAction = false;
                if (fromTimeField)
                {
                    decimal scheduleFromDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["ScheduleFromTime"].Value);
                    dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbFromTime"].Value = scheduleFromDateTime;
                }
                else
                {
                    decimal scheduleToDateTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["ScheduleToTime"].Value);
                    dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbToTime"].Value = scheduleToDateTime;
                }
            }
            finally
            {
                userAction = true;
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }
        private void dgvSearchSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            TransactionReservationScheduleDTO transactionReservationScheduleDTO = null;
            POSUtils.SetLastActivityDateTime();
            try
            { 
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvSearchSchedules.CurrentCell != null && dgvSearchSchedules.Columns[e.ColumnIndex].Name == "SelectSchedule")
                    {
                        this.Cursor = Cursors.WaitCursor; 
                        List<ScheduleDetailsDTO> ScheduleDetailsDTOList = (List<ScheduleDetailsDTO>)dgvSearchSchedules.DataSource;
                        ScheduleDetailsDTO scheduleDetailsDTO = ScheduleDetailsDTOList[e.RowIndex];
                        decimal selectedFromTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbFromTime"].Value);
                        decimal selectedToTime = Convert.ToDecimal(dgvSearchSchedules.Rows[e.RowIndex].Cells["cmbToTime"].Value);
                        if (scheduleDetailsDTO.FixedSchedule == false
                            && scheduleDetailsDTO.ScheduleFromTime == selectedFromTime
                            && scheduleDetailsDTO.ScheduleToTime == selectedToTime)
                        {
                            TimeSpan tspan = scheduleDetailsDTO.ScheduleToDate - scheduleDetailsDTO.ScheduleFromDate;
                            if (tspan.Hours > 2)
                            {

                                string fromTimeString = "";
                                string toTimeString = "";
                                DataRow foundRows = GetMatchingRow(selectedFromTime);
                                if (foundRows != null)
                                    fromTimeString = foundRows["Display"].ToString();

                                foundRows = GetMatchingRow(selectedToTime);
                                if (foundRows != null)
                                    toTimeString = foundRows["Display"].ToString();

                                //Do you want reschedule the reservation to &1 to &2 ?
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2702, fromTimeString, toTimeString),
                                                                MessageContainerList.GetMessage(executionContext, "Reschedule"), MessageBoxButtons.YesNo) == DialogResult.No)
                                {
                                    log.LogMethodExit("User does not want to proceed");
                                    return;
                                }
                            }

                        }
                        DateTime selecteFromDate = scheduleDetailsDTO.ScheduleFromDate.Date.Date.AddMinutes((int)selectedFromTime * 60 + (double)selectedFromTime % 1 * 100);
                        DateTime selectedToDate = scheduleDetailsDTO.ScheduleFromDate.Date.Date.AddMinutes((int)selectedToTime * 60 + (double)selectedToTime % 1 * 100);

                        int lineIndex = reservationBL.BookingTransaction.TrxLines.IndexOf(bookingProductScheduleTrxLine);
                        POSUtils.SetLastActivityDateTime();
                        if (lineIndex > -1)
                        {
                            TransactionReservationScheduleDTO trsDTO = bookingProductScheduleTrxLine.GetCurrentTransactionReservationScheduleDTO();
                            if (trsDTO.ScheduleFromDate == selecteFromDate && trsDTO.ScheduleToDate == selectedToDate)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2714));
                                //Selected schedule for reschedule is same as current reservation schedule!
                            }
                            else
                            {
                                transactionReservationScheduleDTO = new TransactionReservationScheduleDTO(-1, -1,
                                                       (this.bookingProductScheduleTrxLine.DBLineId == 0 ? lineIndex + 1 : this.bookingProductScheduleTrxLine.DBLineId),
                                                       trsDTO.GuestQuantity,
                                                       scheduleDetailsDTO.ScheduleId, scheduleDetailsDTO.ScheduleName, selecteFromDate,
                                                       selectedToDate, false, null, null, null, ServerDateTime.Now, null, ServerDateTime.Now, executionContext.GetSiteId(),
                                                       false, -1, trsDTO.TrxLineProductId, trsDTO.TrxLineProductName,
                                                       scheduleDetailsDTO.FacilityMapId, scheduleDetailsDTO.FacilityMapName, null);

                                TransactionReservationScheduleBL transactionReservationScheduleBL  = new TransactionReservationScheduleBL(executionContext, transactionReservationScheduleDTO);
                                transactionReservationScheduleBL.Save();
                                transactionReservationScheduleDTO = transactionReservationScheduleBL.TransactionReservationScheduleDTO;
                                reservationBL.BookingTransaction.TrxLines[lineIndex].SetTransactionReservationScheduleDTO(executionContext, transactionReservationScheduleDTO);
                                List<KeyValuePair<Transaction.TransactionLine, List<ValidationError>>> validationList = reservationBL.ValidateRescheduleReservation(false);
                                POSUtils.SetLastActivityDateTime();
                                using (frmRescheduleSummaryUI frmRS = new frmRescheduleSummaryUI(utilities, executionContext, this.reservationBL, validationList))
                                {
                                    if (frmRS.ShowDialog() == DialogResult.OK)
                                    {
                                        POSUtils.SetLastActivityDateTime();
                                        this.Cursor = Cursors.WaitCursor;
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                    }
                                    else
                                    {
                                        POSUtils.SetLastActivityDateTime();
                                        this.Cursor = Cursors.WaitCursor;
                                        ClearUnSavedSchedule(transactionReservationScheduleDTO);
                                        reservationBL = new ReservationBL(utilities.ExecutionContext, utilities, reservationBL.GetReservationDTO.BookingId);
                                        bookingProductTrxLine = this.reservationBL.GetBookingProductTransactionLine();
                                        GetBookingProductSchedule();
                                    }
                                }
                            } 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ClearUnSavedSchedule(transactionReservationScheduleDTO);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void ClearUnSavedSchedule(TransactionReservationScheduleDTO transactionReservationScheduleDTO)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.reservationBL.BookingTransaction.ClearUnSavedReservationSchedules(null);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }

        private DataRow GetMatchingRow(object value)
        {
            log.LogMethodEntry(value);
            DataRow selectedRow = null;
            for (int i = 0; i < dtFromTime.Rows.Count; i++)
            {
                if (Convert.ToDecimal(value) == Convert.ToDecimal(dtFromTime.Rows[i]["value"]))
                {
                    selectedRow = dtFromTime.Rows[i];
                    break;
                }
            }
            log.LogMethodExit(selectedRow);
            return selectedRow;
        }

        private void dgvSearchSchedules_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvSearchSchedules.CurrentCell != null)
            {
                if (dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbFromTime" || dgvSearchSchedules.Columns[e.ColumnIndex].Name == "cmbToTime")
                {

                }
            }
            log.LogMethodExit();
        }

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.pressed2;
            log.LogMethodExit();
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            Button selectedButton = (Button)sender;
            selectedButton.BackgroundImage = Properties.Resources.normal2;
            log.LogMethodExit();
        }

        private void frmRescheduleReservationUI_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //Parafait_POS.POSUtils.AttachFormEvents();
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
