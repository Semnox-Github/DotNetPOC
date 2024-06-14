/********************************************************************************************
 * Project Name - Reservation
 * Description  - Basic Reservation Inputs form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      30-Jul-2019   Guru S A                Created for Booking phase 2 enhancement changes  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Parafait_POS.Reservation
{
    public partial class frmBasicReservationInputs : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable dtFromTime = new DataTable();
        private DataTable dtToTime = new DataTable();
        private ExecutionContext executionContext = null;
        private Utilities utilities = null;
        private bool fixedSchedule = false;
        private DateTime selectedScheduleDate;
        private decimal scheduleFromTime = -1;
        private decimal scheduleToTime = -1;
        private int facilityMapId = -1;
        private int totalUnits = 0;
        private bool userAction = false;
        private VirtualKeyboardController virtualKeyboard;
        internal delegate void GetBasicReservationInputs(string bookingName, int guestQty, decimal selectedFromTime, decimal selectedToTime);
        internal GetBasicReservationInputs getBasicReservationInputs;

        public frmBasicReservationInputs(ExecutionContext executionContext, Utilities utilities,
            DataTable dtFromTime, DataTable dtToTime, bool fixedSchedule, DateTime scheduleDate, decimal scheduleFromTime, decimal scheduleToTime, string bookingName, int guestQty, int facilityMapId, int totalUnits)
        {
            log.LogMethodEntry(dtFromTime, dtToTime, fixedSchedule, scheduleDate, scheduleFromTime, scheduleToTime, bookingName,  guestQty, facilityMapId, totalUnits);
            InitializeComponent();
            this.executionContext = executionContext;
            this.utilities = utilities;
            utilities.setLanguage();
            this.dtFromTime = dtFromTime;
            this.dtToTime = dtToTime;
            this.fixedSchedule = fixedSchedule;
            this.selectedScheduleDate = scheduleDate;
            this.scheduleFromTime = scheduleFromTime;
            this.scheduleToTime = scheduleToTime;
            this.facilityMapId = facilityMapId;
            this.totalUnits = totalUnits;
            if (string.IsNullOrEmpty(bookingName) == false)
            {
                this.txtBookingName.Text = bookingName;
            }
            if (guestQty > 0)
            {
                this.txtGuestQty.Text = guestQty.ToString(this.utilities.ParafaitEnv.NUMBER_FORMAT);
            }
            LoadTimeDropdownDropDown();
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            this.userAction = true;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private void LoadTimeDropdownDropDown()
        {
            log.LogMethodEntry();
            this.userAction = false;
            cmbFromTime.DataSource = dtFromTime;
            cmbFromTime.DisplayMember = "Display";
            cmbFromTime.ValueMember = "Value";

            cmbToTime.DataSource = dtToTime;
            cmbToTime.DisplayMember = "Display";
            cmbToTime.ValueMember = "Value";
            this.userAction = true;
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmBasicReservationInputs_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            DateTime fromDate = this.selectedScheduleDate.Date.Date.AddMinutes((int)scheduleFromTime * 60 + (double)scheduleFromTime % 1 * 100);
            DateTime toDate = this.selectedScheduleDate.Date.Date.AddMinutes((int)scheduleToTime * 60 + (double)scheduleToTime % 1 * 100);
            SetAvailableQtyLabel(fromDate, toDate);

            if (fixedSchedule)
            {
                lblFromTime.Visible = lblToTime.Visible = cmbFromTime.Visible = cmbToTime.Visible = false;
            }
            else
            {
                lblFromTime.Visible = lblToTime.Visible = cmbFromTime.Visible = cmbToTime.Visible = true;
                SetFromNToTimeDropDownListValues();
            }
            utilities.setLanguage(this);
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetFromNToTimeDropDownListValues()
        {
            log.LogMethodEntry();
            this.userAction = false;
            DataRow foundRows = GetMatchingRow(this.scheduleFromTime);
            if (foundRows != null)
            {
                cmbFromTime.SelectedValue = foundRows["Value"];
                foundRows = null;
            }
            foundRows = GetMatchingRow(this.scheduleToTime);
            if (foundRows != null)
            {
                cmbToTime.SelectedValue = foundRows["Value"];
                foundRows = null;
            }
            this.userAction = true;
            log.LogMethodExit();
        }

        private void SetAvailableQtyLabel(DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(fromDate, toDate);
            if (this.facilityMapId > -1)
            {
                FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, this.facilityMapId);
                int bookedUnits = facilityMapBL.GetTotalBookedUnitsForReservation(fromDate, toDate);
                int latestTotalUnits = this.totalUnits - bookedUnits;
                if (latestTotalUnits < 0)
                {
                    latestTotalUnits = 0;
                }

                lblAvailableUnitValue.Text = (latestTotalUnits == 0? "0": latestTotalUnits.ToString(this.utilities.ParafaitEnv.NUMBER_FORMAT));
            } 
            log.LogMethodExit();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                ValidateUserInput();

                decimal selectedFromTime = -1;
                if (fixedSchedule == false && cmbFromTime.SelectedValue != null
                    && decimal.TryParse(cmbFromTime.SelectedValue.ToString(), out selectedFromTime) == false)
                {
                    log.LogMethodExit("invalid from time");
                    return;
                }

                decimal selectedToTime = -1;
                if (fixedSchedule == false && cmbToTime.SelectedValue != null
                    && decimal.TryParse(cmbToTime.SelectedValue.ToString(), out selectedToTime) == false)
                {
                    log.LogMethodExit("invalid to time");
                    return;
                }

                getBasicReservationInputs(txtBookingName.Text, Convert.ToInt32(txtGuestQty.Text), selectedFromTime, selectedToTime);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
                return;
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void ValidateUserInput()
        {
            log.LogMethodEntry();
            string specialChars = @"[-+=@]";
            if (string.IsNullOrEmpty(txtBookingName.Text))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 302));
            }
            else if (Regex.IsMatch(txtBookingName.Text.Substring(0,1), specialChars))
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2265 , MessageContainerList.GetMessage(executionContext,"Booking Name"),  specialChars));
            }
            int guestQty = 0;
            if (string.IsNullOrEmpty(txtGuestQty.Text) || int.TryParse(txtGuestQty.Text, out guestQty) == false)
            {
                txtGuestQty.Text = string.Empty;
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1834));
            }

            ValidateFreeScheduleRangeValues();
            log.LogMethodExit();
        }

        private void ValidateFreeScheduleRangeValues()
        {
            log.LogMethodEntry();
            if (fixedSchedule == false)
            {
                decimal selectedFromTime;
                if (cmbFromTime.SelectedValue == null
                    || decimal.TryParse(cmbFromTime.SelectedValue.ToString(), out selectedFromTime) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2091));//"Please enter valid time in Search From Time field"
                }

                decimal selectedToTime;
                if (cmbToTime.SelectedValue == null
                    || decimal.TryParse(cmbToTime.SelectedValue.ToString(), out selectedToTime) == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2092)); //"Please enter valid time in Search To Time field"
                }

                if (selectedFromTime < this.scheduleFromTime || selectedFromTime >= this.scheduleToTime
                    || selectedToTime <= this.scheduleFromTime || selectedToTime > this.scheduleToTime)
                {

                    string fromTimeString = "";
                    string toTimeString = "";
                    DataRow foundRows = GetMatchingRow(this.scheduleFromTime);
                    if (foundRows != null)
                        fromTimeString = foundRows["Display"].ToString();

                    foundRows = GetMatchingRow(this.scheduleToTime);
                    if (foundRows != null)
                        toTimeString = foundRows["Display"].ToString();

                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2105, fromTimeString, toTimeString));//"Time should be with in " + fromTimeString + " and " + toTimeString

                }

                if (selectedFromTime > selectedToTime)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 305));
                }
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void cmbTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            POSUtils.SetLastActivityDateTime();
            if (userAction)
            {
                try
                {
                    ValidateFreeScheduleRangeValues();
                    decimal selectedFromTime = -1;
                    if (fixedSchedule == false && cmbFromTime.SelectedValue != null
                        && decimal.TryParse(cmbFromTime.SelectedValue.ToString(), out selectedFromTime) == false)
                    {
                        log.LogMethodExit("invalid from time");
                        return;
                    }

                    decimal selectedToTime = -1;
                    if (fixedSchedule == false && cmbToTime.SelectedValue != null
                        && decimal.TryParse(cmbToTime.SelectedValue.ToString(), out selectedToTime) == false)
                    {
                        log.LogMethodExit("invalid to time");
                        return;
                    }
                    DateTime fromDate = this.selectedScheduleDate.Date.Date.AddMinutes((int)selectedFromTime * 60 + (double)selectedFromTime % 1 * 100);
                    DateTime toDate = this.selectedScheduleDate.Date.Date.AddMinutes((int)selectedToTime * 60 + (double)selectedToTime % 1 * 100);
                    SetAvailableQtyLabel(fromDate, toDate);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                    return;
                }
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime(); 
            log.LogMethodExit();
        }
    }
}
