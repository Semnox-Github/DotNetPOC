/********************************************************************************************
 * Project Name - Reservation
 * Description  - BookingAttendees form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.0      26-Mar-2019   Guru S A                Booking phase 2 enhancement changes 
 *2.80.0      26-Oct-2019   Guru S A                Waiver phase 2 enhancement changes 
 *2.80.0      22-Apr-2020   Guru S A                Send sign waiver email changes
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Communication;

namespace Parafait_POS.Reservation
{
    public partial class frmBookingAttendees : Form
    {
        private Utilities utilities = POSStatic.Utilities;
        private int bookingId;
        private int trxId;
        private int guestCustomerId;
        private CustomCheckBox cbxHeaderSelectRecord;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ReservationBL reservationBL;
        private List<KeyValuePair<int, CustomerBL>> customerBLList = new List<KeyValuePair<int, CustomerBL>>();
        private bool initialLoad = true;
        private List<WaiversDTO> trxWaiversDTOList;
        private const string WAIVERSETUP = "WAIVER_SETUP";
        private const string BASEURLFORSIGNWAIVER = "BASE_URL_FOR_SIGN_WAIVER";
        private const string RESERVATIONWAIVERURL = "RESERVATION_WAIVER_URL";
        private string attachFile = null;
        private Semnox.Parafait.Communication.SendEmailUI semail = null;
        public ReservationBL GetReservationBL { get { return reservationBL; } }
        public frmBookingAttendees(ReservationBL pReservationBL, string CustomerName)
        {
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            log.LogMethodEntry();
            utilities.setLanguage();
            InitializeComponent();
            reservationBL = pReservationBL;
            bookingId = reservationBL.GetReservationDTO.BookingId;
            this.trxId = reservationBL.GetReservationDTO.TrxId;
            this.Text += MessageContainerList.GetMessage(utilities.ExecutionContext, " for ") + reservationBL.GetReservationDTO.BookingName + "/" + CustomerName;
            this.guestCustomerId = CustomerListBL.GetGuestCustomerId(utilities.ExecutionContext);
            utilities.setupDataGridProperties(ref dgvAttendees);
            this.DateofBirth.DefaultCellStyle = utilities.gridViewDateCellStyle();
            this.signWaiverEmailLastSentOn.DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            this.signWaiverEmailSentCount.DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvAttendees.BorderStyle = BorderStyle.FixedSingle;
            trxWaiversDTOList = reservationBL.BookingTransaction.GetWaiversDTOList();
            UpdateUIElements();
            LoadDataSource();
            SetWaiverRelatedUI();
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void frmBookingAttendees_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (bgwSetWaiverSigned.IsBusy == false)
            {
                bgwSetWaiverSigned.WorkerReportsProgress = true;
                bgwSetWaiverSigned.RunWorkerAsync();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void LoadDGVAttendees()
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                this.Cursor = Cursors.WaitCursor;
                if (reservationBL != null && reservationBL.GetReservationDTO != null)
                {
                    LoadDataSource();
                    if (bgwSetWaiverSigned.IsBusy == false)
                    {
                        bgwSetWaiverSigned.WorkerReportsProgress = true;
                        bgwSetWaiverSigned.RunWorkerAsync();
                    }
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }

        private void LoadDataSource()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (reservationBL != null)
            {
                reservationBL.LoadAttendeeDetails();
                SortableBindingList<BookingAttendeeDTO> bookingAttendeeDTOSortList = new SortableBindingList<BookingAttendeeDTO>();
                List<BookingAttendeeDTO> bookingAttendees = reservationBL.GetReservationAttendeeList();
                if (bookingAttendees != null && bookingAttendees.Any())
                {
                    //List<BookingAttendeeDTO> activeAttendees = bookingAttendees;//.Where(attendee => attendee.IsActive == true).ToList();
                    bookingAttendeeDTOSortList = new SortableBindingList<BookingAttendeeDTO>(bookingAttendees);
                }
                BindingSource bookingAttendeeBS = new BindingSource();
                bookingAttendeeBS.DataSource = bookingAttendeeDTOSortList;
                dgvAttendees.DataSource = bookingAttendeeBS;
                dgvAttendees.EndEdit();
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void SetWaiverSignedStatus()
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            BuildCustomerBLList();
            if (customerBLList != null && customerBLList.Any())
            {
                for (int i = 0; i < dgvAttendees.Rows.Count; i++)
                {
                    POSUtils.SetLastActivityDateTime();
                    try
                    {
                        if (dgvAttendees["nameDataGridViewTextBoxColumn", i].Value != null)
                        {
                            bool signedWaivers = false;
                            if (dgvAttendees["CustomerId", i].Value != null)
                            {
                                CustomerBL customerBL = null;
                                int customerId = -1;
                                var objId = this.dgvAttendees["CustomerId", i];
                                int.TryParse(objId.Value.ToString(), out customerId);
                                if (customerId > -1)
                                {
                                    if (customerBLList.Exists(blKeyValue => blKeyValue.Key == customerId))
                                    {
                                        KeyValuePair<int, CustomerBL> custKeyValueInfo = customerBLList.FirstOrDefault(keyValue => keyValue.Key == customerId);
                                        customerBL = custKeyValueInfo.Value;
                                    }
                                    if (customerBL != null)
                                    {
                                        if (customerBL.CustomerDTO != null && customerBL.CustomerDTO.Id > -1 && customerBL.CustomerDTO.Id != guestCustomerId)
                                        {
                                            if (customerBL.HasSigned(trxWaiversDTOList, reservationBL.BookingTransaction.TrxDate))
                                            {
                                                signedWaivers = true;
                                            }
                                        }
                                        else if (guestCustomerId > -1 && customerBL.CustomerDTO != null && guestCustomerId == customerBL.CustomerDTO.Id)
                                        {
                                            //guest customer and no line is pending for signature
                                            if (reservationBL.BookingTransaction.IsWaiverSignaturePending() == false)
                                            {
                                                signedWaivers = true;
                                            }
                                        }
                                    }
                                }
                                bgwSetWaiverSigned.ReportProgress(i, new Tuple<int, bool, CustomerBL>(i, signedWaivers, customerBL));
                            }
                        }
                    }
                    catch { }
                }
            }
            log.LogMethodExit();
        }

        private void BuildCustomerBLList()
        {
            log.LogMethodEntry();
            List<int> custIdList = new List<int>();
            customerBLList = new List<KeyValuePair<int, CustomerBL>>();
            if (lblWaiverStatusMessageTwo.Visible)
            {
                for (int i = 0; i < dgvAttendees.Rows.Count; i++)
                {
                    POSUtils.SetLastActivityDateTime();
                    try
                    {
                        if (dgvAttendees["nameDataGridViewTextBoxColumn", i].Value != null)
                        {
                            if (dgvAttendees["CustomerId", i].Value != null)
                            {
                                int customerId = -1;
                                var objId = this.dgvAttendees["CustomerId", i];
                                int.TryParse(objId.Value.ToString(), out customerId);
                                if (customerId > -1)
                                {
                                    if (customerBLList.Exists(blKeyValue => blKeyValue.Key == customerId) == false)
                                    {
                                        custIdList.Add(customerId);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                if (custIdList != null && custIdList.Any())
                {
                    CustomerListBL customerListBL = new CustomerListBL(utilities.ExecutionContext);
                    List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(custIdList, true, true, true);
                    if (customerDTOList != null && customerDTOList.Any())
                    {
                        for (int i = 0; i < customerDTOList.Count; i++)
                        {
                            POSUtils.SetLastActivityDateTime();
                            CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerDTOList[i]);
                            customerBLList.Add(new KeyValuePair<int, CustomerBL>(customerDTOList[i].Id, customerBL));
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    bookingAttendeeDTOBindingSource = (BindingSource)dgvAttendees.DataSource;
                    SortableBindingList<BookingAttendeeDTO> bookingAttendeeRecords = (SortableBindingList<BookingAttendeeDTO>)bookingAttendeeDTOBindingSource.DataSource;

                    if (bookingAttendeeRecords != null && bookingAttendeeRecords.Count > 0)
                    {
                        int lineIndex = 0;
                        try
                        {
                            foreach (BookingAttendeeDTO bookingAttendeeDTO in bookingAttendeeRecords)
                            {
                                POSUtils.SetLastActivityDateTime();
                                if (bookingAttendeeDTO.IsChanged)
                                {
                                    reservationBL.AddUpdateReservationAttendees(bookingAttendeeDTO);
                                }
                                lineIndex++;
                            }

                            reservationBL.SaveReservationAttendeesOnly(null);
                            dgvAttendees.EndEdit();
                            //LoadDataSource();
                            //dgvAttendees.Refresh();
                        }
                        catch (Exception ex)
                        {
                            POSUtils.ParafaitMessageBox(ex.Message);
                        }
                        LoadDGVAttendees();
                    }
                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    if (dgvAttendees.CurrentRow == null || dgvAttendees.CurrentRow.IsNewRow || dgvAttendees.CurrentRow.Cells[0].Value == DBNull.Value)
                    {
                        log.LogMethodExit("Nothing to delete");
                        return;
                    }
                    else if (dgvAttendees.SelectedRows == null || dgvAttendees.SelectedRows.Count == 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, "Please select a record to delete"));
                    }
                    else
                    {
                        DialogResult result1 = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1766),
                            MessageContainerList.GetMessage(utilities.ExecutionContext, "Booking Attendees"), MessageBoxButtons.YesNo);
                        if (result1 == DialogResult.Yes)
                        {
                            // int id = Convert.ToInt32(dgvAttendees.CurrentRow.Cells["idDataGridViewTextBoxColumn"].Value);
                            //BookingAttendeeDTO bookingAttendeeDTO = (BookingAttendeeDTO)dgvAttendees.CurrentRow; 
                            try
                            {
                                List<int> removeFromGridList = new List<int>();
                                BindingSource dgvAttendeesBS = (BindingSource)dgvAttendees.DataSource;
                                var dgvAttendeesDTOList = (SortableBindingList<BookingAttendeeDTO>)dgvAttendeesBS.DataSource;
                                foreach (DataGridViewRow dataRow in dgvAttendees.SelectedRows)
                                {
                                    POSUtils.SetLastActivityDateTime();
                                    BookingAttendeeDTO removeBookingAttendeeDTO = dgvAttendeesDTOList[dataRow.Index];
                                    List<BookingAttendeeDTO> bookingAttendeeDTOList = reservationBL.GetReservationAttendeeList();
                                    if (bookingAttendeeDTOList != null && bookingAttendeeDTOList.Contains(removeBookingAttendeeDTO))
                                    {
                                        reservationBL.RemoveReservationAttendees(removeBookingAttendeeDTO);
                                        reservationBL.SaveReservationAttendeesOnly(null);
                                        //dgvAttendees.Rows[dataRow.Index].Cells["IsActive"].Value = false;
                                    }
                                    else
                                    {
                                        removeFromGridList.Add(1);
                                        //dgvAttendees.Rows.RemoveAt(dgvAttendees.CurrentRow.Index);
                                    }
                                }
                                if (removeFromGridList.Any())
                                {
                                    for (int i = 0; i < removeFromGridList.Count; i++)
                                    {
                                        dgvAttendees.Rows.RemoveAt(removeFromGridList[i]);
                                    }
                                }
                                dgvAttendees.EndEdit();
                                //LoadDataSource();
                                //dgvAttendees.Refresh();
                            }
                            catch (Exception expn)
                            {
                                log.Error(expn);
                                POSUtils.ParafaitMessageBox(expn.Message.ToString());
                            }
                            LoadDGVAttendees();
                        }
                    }
                }
            }
            catch (Exception expn)
            {
                log.Error(expn);
                POSUtils.ParafaitMessageBox(expn.Message.ToString());
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    //if (bgwSetWaiverSigned.IsBusy)
                    //{
                    //    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2398), MessageContainerList.GetMessage(utilities.ExecutionContext, "Refresh"));//Please wait, form is loading waiver signed details to the data grid
                    //    return;
                    //}
                    if (HasModifiedRecords())
                    {
                        //"Any unsaved changes will be cleared"
                        if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2126), "Confirm Refresh", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                        {
                            log.Info("There are unsaved Transactions");
                            return;
                        }
                    }
                    LoadDGVAttendees();
                }
            }
            catch (Exception expn)
            {
                log.Error(expn);
                POSUtils.ParafaitMessageBox(expn.Message.ToString());
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    if (HasModifiedRecords() == false)
                    {
                        if (reservationBL.ReservationHasActiveAttendeeList())
                            utilities.ExportToExcel(dgvAttendees, reservationBL.GetReservationDTO.BookingName, this.Text, utilities.ParafaitEnv.SiteName, reservationBL.GetReservationDTO.FromDate, reservationBL.GetReservationDTO.ToDate);
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 2127), MessageContainerList.GetMessage(utilities.ExecutionContext, "Excel Export"));//There are unsaved records, Please save the booking first
                    }
                }
            }
            catch (Exception expn)
            {
                log.Error(expn);
                POSUtils.ParafaitMessageBox(expn.Message.ToString());
            }
            POSUtils.SetLastActivityDateTime();
            log.LogMethodExit();
        }

        private bool HasModifiedRecords()
        {
            log.LogMethodEntry();
            bool hasModifiedRecords = false;
            bookingAttendeeDTOBindingSource = (BindingSource)dgvAttendees.DataSource;
            SortableBindingList<BookingAttendeeDTO> bookingAttendeeRecords = (SortableBindingList<BookingAttendeeDTO>)bookingAttendeeDTOBindingSource.DataSource;

            if (bookingAttendeeRecords != null && bookingAttendeeRecords.Count > 0)
            {
                foreach (BookingAttendeeDTO bookingAttendeeDTO in bookingAttendeeRecords)
                {
                    if (bookingAttendeeDTO.IsChanged)
                    {
                        hasModifiedRecords = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(hasModifiedRecords);
            return hasModifiedRecords;
        }

        private void dgvAttendees_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 3, e.RowIndex + 1, e.Exception.Message));
                log.Error("Error in grid data at row " + e.RowIndex + 1 + " : " + e.Exception.Message + "");
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvAttendees_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            e.Row.Cells["bookingIdDataGridViewTextBoxColumn"].Value = bookingId;
            e.Row.Cells["TrxId"].Value = trxId;
            e.Row.Cells["Party_In_Name_Of"].Value = false;
            log.LogMethodExit();
        }
        private void dgvAttendees_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1)
            {
                if (dgvAttendees.CurrentCell != null)
                {
                    if (dgvAttendees.Columns[e.ColumnIndex].Name == "IsActive")
                    {
                        DataGridViewCheckBoxCell checkBox = (dgvAttendees["IsActive", e.RowIndex] as DataGridViewCheckBoxCell);
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            dgvAttendees["IsActive", e.RowIndex].ReadOnly = true;
                        }
                        else
                        {
                            dgvAttendees["IsActive", e.RowIndex].ReadOnly = false;
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvAttendees_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if(dgvAttendees.CurrentCell != null && e.RowIndex > -1)
            //{
            //    if(dgvAttendees.Columns[e.ColumnIndex].Name == "nameDataGridViewTextBoxColumn")
            //    {
            //        BindingSource dgvAttendeesBS = (BindingSource)dgvAttendees.DataSource;
            //        var dgvAttendeesDTOList = (SortableBindingList<BookingAttendeeDTO>)dgvAttendeesBS.DataSource;
            //        BookingAttendeeDTO updatedBookingAttendeeDTO = dgvAttendeesDTOList[dgvAttendees.CurrentRow.Index];
            //        List<BookingAttendeeDTO> bookingAttendeeDTOList = reservationBL.GetReservationAttendeeList();
            //        if (bookingAttendeeDTOList != null && bookingAttendeeDTOList.Contains(updatedBookingAttendeeDTO))
            //        {
            //            //AddUpdateAttendee(dgvAttendees.CurrentRow.Index, updatedBookingAttendeeDTO);
            //        } 

            //    }
            //}
            log.LogMethodExit();
        }

        private void dgvAttendees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if (dgvAttendees.Columns[e.ColumnIndex].Name == "selectRecord")
                {
                    DataGridViewCheckBoxCell checkBox = (dgvAttendees["selectRecord", e.RowIndex] as DataGridViewCheckBoxCell);
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

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (BookingIsInEditMode() == false)
            {
                dgvAttendees.ReadOnly = true;
                dgvAttendees.AllowUserToAddRows = false;
                btnDelete.Enabled = false;
                btnSave.Enabled = false;
            }
            else
            {
                dgvAttendees.ReadOnly = false;
                dgvAttendees.AllowUserToAddRows = true;
                btnDelete.Enabled = true;
                btnRefresh.Enabled = true;
                btnSave.Enabled = true;
            }
            log.LogMethodExit();
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

        private void bgwSetWaiverSigned_DoWork(object sender, DoWorkEventArgs e)
        {
            log.LogMethodEntry();
            SetWaiverSignedStatus();
            log.LogMethodExit();
        }

        private void bgwSetWaiverSigned_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.LogMethodEntry();
            if (e.UserState != null)
            {
                this.Cursor = Cursors.WaitCursor;
                POSUtils.SetLastActivityDateTime();
                Tuple<int, bool, CustomerBL> dataTuple = (Tuple<int, bool, CustomerBL>)e.UserState;
                if (dataTuple != null)
                {
                    dgvAttendees["WaiversSigned", dataTuple.Item1].Value = dataTuple.Item2;
                    //dgvAttendees["CustomerId", dataTuple.Item1].Tag = dataTuple.Item3;
                    if (dataTuple.Item3 != null)
                    {
                        if (customerBLList.Exists(keyVal => keyVal.Key == dataTuple.Item3.CustomerDTO.Id) == false)
                        {
                            customerBLList.Add(new KeyValuePair<int, CustomerBL>(dataTuple.Item3.CustomerDTO.Id, dataTuple.Item3));
                        }
                    }

                }
            }

            log.LogMethodExit();
        }

        private void bgwSetWaiverSigned_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            this.Cursor = Cursors.Default;
            this.dgvAttendees.EndEdit();
            this.dgvAttendees.Refresh();
            if (initialLoad)
            {
                utilities.setLanguage(this);
                initialLoad = false;
            }
            SetWaiverMesage();
            log.LogMethodExit();
        }

        private bool IsBackgroundJobRunning(bool supressMsg = false)
        {
            log.LogMethodEntry(supressMsg);
            POSUtils.SetLastActivityDateTime();
            bool jobRunning = false;
            if (bgwSetWaiverSigned.IsBusy)
            {
                jobRunning = true;
                if (supressMsg == false)
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 1448));//Loading... Please wait...
                }
            }
            log.LogMethodExit(jobRunning);
            return jobRunning;
        }

        private void frmBookingAttendees_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (bgwSetWaiverSigned.IsBusy)
            {
                bgwSetWaiverSigned.WorkerSupportsCancellation = true;
                bgwSetWaiverSigned.CancelAsync();
                while (bgwSetWaiverSigned.CancellationPending)
                {
                    this.Cursor = Cursors.WaitCursor;
                }
                this.Cursor = Cursors.Default;
            }

            //if (HasModifiedRecords())
            //{
            //    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 203), "Confirm Refresh", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            //    {
            //        log.Info("There are unsaved Transactions");
            //        e.Cancel = true;
            //    }
            //} 
            log.LogMethodExit();
        }

        private void SetWaiverRelatedUI()
        {
            log.LogMethodEntry();
            if (this.reservationBL != null)
            {
                POSUtils.SetLastActivityDateTime();

                this.lblWaiverStatusMessageOne.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Reservation Guest Count:");
                this.lblWaiverStatusMessageOneVal.Text = reservationBL.GetReservationDTO.Quantity.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);

                if (this.reservationBL.BookingTransaction != null
                    && this.reservationBL.BookingTransaction.TrxLines != null
                    && this.reservationBL.BookingTransaction.TrxLines.Exists(tl => tl.LineValid && tl.CancelledLine == false
                                                                                && tl.WaiverSignedDTOList != null
                                                                                && tl.WaiverSignedDTOList.Any()
                                                                                && tl.WaiverSignedDTOList.Exists(ws => ws.IsActive)))
                {
                    this.lblWaiverStatusMessageTwo.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Waiver Signed Attendee Count:");
                    CreateHeaderCheckBox();
                }
                else
                {
                    this.remarksDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                    this.lblWaiverStatusMessageTwo.Visible = false;
                    this.lblWaiverStatusMessageTwoVal.Visible = false;
                    this.selectRecord.Visible = false;
                    this.WaiversSigned.Visible = false;
                    this.signWaiverEmailLastSentOn.Visible = false;
                    this.signWaiverEmailSentCount.Visible = false;
                    //this.dgvAttendees.Location = new Point(this.dgvAttendees.Location.X, lblWaiverStatusMessage.Location.Y);
                    this.btnSendWaiverEmail.Visible = false;
                    this.btnExcel.Location = new Point(this.btnSendWaiverEmail.Location.X, this.btnExcel.Location.Y);
                }
            }
            log.LogMethodExit();
        }

        private void SetWaiverMesage()
        {
            log.LogMethodEntry();
            if (lblWaiverStatusMessageTwo.Visible)
            {
                //List<Transaction.TransactionLine> waiverReqLines = this.reservationBL.BookingTransaction.GetWaiverTransactionLines();
                //int waiverRequiredProductQty = ((waiverReqLines != null && waiverReqLines.Any()) ? waiverReqLines.Count : 0);
                // if (guestQty > 0)
                //{
                //int waiverPendingProductQty = 0;
                //if (waiverReqLines != null && waiverReqLines.Any())
                //{
                //    List<Transaction.TransactionLine> waiverPendingLines = waiverReqLines.Where(tl => tl.LineValid && tl.WaiverSignedDTOList != null
                //                                                                                && tl.WaiverSignedDTOList.Exists(ws => ws.IsActive &&
                //                                                                                                                      ws.CustomerSignedWaiverId == -1)).ToList();
                //    waiverPendingProductQty = (waiverPendingLines != null && waiverPendingLines.Any() ? waiverPendingLines.Count : 0);
                //}

                int attendeeCount = 0;
                //List<BookingAttendeeDTO> bookingAttendees = reservationBL.GetReservationAttendeeList();
                if (dgvAttendees.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvAttendees.RowCount; i++)
                    {
                        if (dgvAttendees["CustomerId", i].Value != null
                            && Convert.ToInt32(dgvAttendees["CustomerId", i].Value) > -1
                            && dgvAttendees["WaiversSigned", i].Value != null
                            && Convert.ToBoolean(dgvAttendees["WaiversSigned", i].Value) == true)
                        {
                            attendeeCount++;
                        }
                    }
                }
                if (attendeeCount > 0)
                {
                    lblWaiverStatusMessageTwoVal.Text = attendeeCount.ToString(utilities.ParafaitEnv.NUMBER_FORMAT);
                }
                else
                {
                    lblWaiverStatusMessageTwoVal.Text = "0";
                }

                //"&1 attendees have signed required waivers so far. Total quantity of waiver required products is &2"; 
            }
            log.LogMethodExit();
        }

        private void btnSendWaiverEmail_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                POSUtils.SetLastActivityDateTime();
                if (IsBackgroundJobRunning() == false)
                {
                    this.Cursor = Cursors.WaitCursor;
                    List<BookingAttendeeDTO> selectedBookingAttendeeDTOList = GetSelectedDTOList();
                    //if (selectedBookingAttendeeDTOList != null && selectedBookingAttendeeDTOList.Any())
                    //{
                    SendEmailUI(selectedBookingAttendeeDTOList);
                    DeleteAttachmentFile();
                    //}
                    //else
                    //{
                    //    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Please select attendee records to proceed"));
                    //}

                }
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                POSUtils.SetLastActivityDateTime();
            }
            log.LogMethodExit();
        }


        private List<BookingAttendeeDTO> GetSelectedDTOList()
        {
            log.LogMethodEntry();
            List<BookingAttendeeDTO> bookingAttendeeDTOList = new List<BookingAttendeeDTO>();
            if (dgvAttendees != null && dgvAttendees.Rows.Count > 0)
            {
                bookingAttendeeDTOBindingSource = (BindingSource)dgvAttendees.DataSource;
                SortableBindingList<BookingAttendeeDTO> bookingAttendeeRecords = (SortableBindingList<BookingAttendeeDTO>)bookingAttendeeDTOBindingSource.DataSource;

                if (bookingAttendeeRecords != null)
                {
                    for (int rowIndex = 0; rowIndex < dgvAttendees.Rows.Count; rowIndex++)
                    {
                        DataGridViewCheckBoxCell checkBox = (dgvAttendees["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            BookingAttendeeDTO bookingAttendeeDTO = bookingAttendeeRecords[rowIndex];
                            bookingAttendeeDTOList.Add(bookingAttendeeDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(bookingAttendeeDTOList);
            return bookingAttendeeDTOList;
        }
        private void SendEmailUI(List<BookingAttendeeDTO> selectedBookingAttendeeDTOList)
        {
            log.LogMethodEntry();
            POSUtils.SetLastActivityDateTime();
            if (reservationBL != null && reservationBL.BookingTransaction != null && reservationBL.BookingTransaction.WaiverSignatureRequired())
            {
                LookupValuesList lookupValuesList = new LookupValuesList(this.utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, BASEURLFORSIGNWAIVER));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam);
                string baseURLWaiver = string.Empty;
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    baseURLWaiver = lookupValuesDTOList[0].Description;
                }
                if (string.IsNullOrEmpty(baseURLWaiver) || string.IsNullOrEmpty(baseURLWaiver.Trim()))
                {
                    string missingParamName = MessageContainerList.GetMessage(utilities.ExecutionContext, BASEURLFORSIGNWAIVER) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "parameter");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2654, missingParamName));//'&1 details is missing in sign waiver URL setup'
                }
                POSUtils.SetLastActivityDateTime();
                SignWaiverEmail signWaiverEmail = new SignWaiverEmail(this.utilities.ExecutionContext, reservationBL.BookingTransaction, this.utilities);
                List<ValidationError> validationErrorList = signWaiverEmail.CanSendSignWaiverEmail(null);

                if (reservationBL.GetReservationDTO.ToDate.AddDays(1) < utilities.getServerTime())
                {
                    if (validationErrorList == null) { validationErrorList = new List<ValidationError>(); }
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(utilities.ExecutionContext, "Waiver"), MessageContainerList.GetMessage(utilities.ExecutionContext, "Email"),
                                                                MessageContainerList.GetMessage(utilities.ExecutionContext, 2657, reservationBL.GetReservationDTO.ToDate.ToString(utilities.ParafaitEnv.DATETIME_FORMAT))));//'Sorry unable to proceed. Transaction Date (&1) is over'
                }
                if (validationErrorList != null && validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(this.utilities.ExecutionContext, "Validation Error"), validationErrorList);
                }
                POSUtils.SetLastActivityDateTime();
                lookupValuesList = new LookupValuesList(this.utilities.ExecutionContext);
                lookupValuesDTOList = null;
                searchParam = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, WAIVERSETUP));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, RESERVATIONWAIVERURL));
                searchParam.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, this.utilities.ExecutionContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParam);
                POSUtils.SetLastActivityDateTime();
                string signWaiverLinkReservation = string.Empty;
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    signWaiverLinkReservation = lookupValuesDTOList[0].Description;
                }
                if (string.IsNullOrEmpty(signWaiverLinkReservation) || string.IsNullOrEmpty(signWaiverLinkReservation.Trim()))
                {
                    string missingParamName = MessageContainerList.GetMessage(utilities.ExecutionContext, RESERVATIONWAIVERURL) + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "parameter");
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2654, missingParamName));//'&1 details is missing in sign waiver URL setup'
                }

                semail = null;
                string message = string.Empty;
                string template = string.Empty;
                string contentID = string.Empty;
                attachFile = null;
                string emailIDList = GetEmailIdList(selectedBookingAttendeeDTOList);
                POSUtils.SetLastActivityDateTime();
                if (utilities.ParafaitEnv.CompanyLogo != null)
                {
                    contentID = "ParafaitBookingLogo" + Guid.NewGuid().ToString() + ".jpg";//Content Id is the identifier for the image
                    attachFile = POSStatic.GetCompanyLogoImageFile(contentID);
                    if (string.IsNullOrWhiteSpace(attachFile))
                    {
                        contentID = "";
                    }
                }

                //Get the email template//
                string emailTemplate = string.Empty;
                emailTemplate = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "RESERVATION_WAIVER_SIGNATURE_EMAIL_TEMPLATE");
                EmailTemplateDTO emailTemplateDTO = new EmailTemplate(utilities.ExecutionContext).GetEmailTemplate(emailTemplate, utilities.ExecutionContext.GetSiteId());
                string emailContent = string.Empty;
                if (emailTemplateDTO != null && emailTemplateDTO.EmailTemplateId > 0)
                {
                    emailContent = emailTemplateDTO.EmailTemplate;

                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(utilities.ExecutionContext, utilities, emailTemplateDTO.EmailTemplateId, reservationBL.BookingTransaction, reservationBL.GetReservationDTO);
                    emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();

                }
                else
                {
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2649));//'Email template for Send sign waiver with URL is not defined'
                }
                POSUtils.SetLastActivityDateTime();
                if (emailTemplateDTO != null)
                {
                    string emailSubject = emailTemplateDTO.Description;
                    semail = new Semnox.Parafait.Communication.SendEmailUI(emailIDList,
                                                        "", "", emailSubject, emailContent, null, "", attachFile, contentID, false, utilities, true);

                    semail.ShowDialog();
                    if (semail.EmailSentSuccessfully)
                    {
                        POSUtils.SetLastActivityDateTime();
                        log.Info("Email Send Successfully");
                        UpdateEmailSentStatusInfo(selectedBookingAttendeeDTOList);
                        LoadDGVAttendees();
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2660));//"Waiver Signature is not Required"
            }
            log.LogMethodExit();
        }

        private void UpdateEmailSentStatusInfo(List<BookingAttendeeDTO> selectedBookingAttendeeDTOList)
        {
            log.LogMethodEntry(selectedBookingAttendeeDTOList);
            if (selectedBookingAttendeeDTOList != null && selectedBookingAttendeeDTOList.Any())
            {
                for (int i = 0; i < selectedBookingAttendeeDTOList.Count; i++)
                {
                    if (string.IsNullOrEmpty(selectedBookingAttendeeDTOList[i].Email) == false)
                    {
                        selectedBookingAttendeeDTOList[i].SignWaiverEmailSentCount = selectedBookingAttendeeDTOList[i].SignWaiverEmailSentCount + 1;
                        selectedBookingAttendeeDTOList[i].SignWaiverEmailLastSentOn = utilities.getServerTime();
                        BookingAttendee bookingAttendee = new BookingAttendee(utilities.ExecutionContext, selectedBookingAttendeeDTOList[i]);
                        bookingAttendee.Save();
                    }
                }
            }
            log.LogMethodExit();
        }

        private string GetEmailIdList(List<BookingAttendeeDTO> selectedBookingAttendeeDTOList)
        {
            log.LogMethodEntry(selectedBookingAttendeeDTOList);
            StringBuilder stringBuilder = new StringBuilder("");
            if (selectedBookingAttendeeDTOList != null && selectedBookingAttendeeDTOList.Any())
            {
                for (int i = 0; i < selectedBookingAttendeeDTOList.Count; i++)
                {
                    if (string.IsNullOrEmpty(selectedBookingAttendeeDTOList[i].Email) == false)
                    {
                        stringBuilder.Append(selectedBookingAttendeeDTOList[i].Email + ",");
                    }
                }
            }
            string emailIDList = stringBuilder.ToString();
            if (string.IsNullOrEmpty(emailIDList) == false)
            {
                emailIDList = emailIDList.Substring(0, emailIDList.Length - 1);
            }
            log.LogMethodExit(emailIDList);
            return emailIDList;
        }


        private void CreateHeaderCheckBox()
        {
            log.LogMethodEntry();
            //if (!dgvRedemptions.Controls.ContainsKey("ReverseGiftHeaderCheckBox"))
            //{
            cbxHeaderSelectRecord = new CustomCheckBox();
            //cbxHeaderSelectRecord.Name = "HeaderCheckBox";
            //cbxHeaderSelectRecord.Appearance = System.Windows.Forms.Appearance.Button;
            cbxHeaderSelectRecord.FlatAppearance.BorderSize = 0;
            //cbxHeaderSelectRecord.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
            //headerCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            //headerCheckBox.UseVisualStyleBackColor = true;
            //headerCheckBox.Image = Parafait_POS.Properties.Resources.UncheckedNew;
            //headerCheckBox.ImageAlign = ContentAlignment.BottomCenter;
            cbxHeaderSelectRecord.ImageAlign = ContentAlignment.MiddleCenter;
            cbxHeaderSelectRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            cbxHeaderSelectRecord.Text = string.Empty;
            cbxHeaderSelectRecord.Font = dgvAttendees.Font;
            cbxHeaderSelectRecord.Location = new Point(dgvAttendees.Columns["selectRecord"].HeaderCell.ContentBounds.X + 26, dgvAttendees.Columns["selectRecord"].HeaderCell.ContentBounds.Y + 1);
            cbxHeaderSelectRecord.BackColor = Color.Transparent;
            cbxHeaderSelectRecord.Size = new Size(60, 28);
            cbxHeaderSelectRecord.Click += new EventHandler(HeaderCheckBox_Clicked);
            dgvAttendees.Controls.Add(cbxHeaderSelectRecord);
            //}
            //else
            //{
            //    CheckBox headerCheckBox = dgvRedemptions.Controls.Find("ReverseGiftHeaderCheckBox", false)[0] as CheckBox;
            //    headerCheckBox.Checked = false;
            //    headerCheckBox.Image = global::Parafait_POS.Properties.Resources.UncheckedNew;
            //}
            log.LogMethodExit();
        }


        private void HeaderCheckBox_Clicked(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                POSUtils.SetLastActivityDateTime();
                dgvAttendees.EndEdit();
                CheckBox headerCheckBox = (sender as CheckBox);

                for (int rowIndex = 0; rowIndex < dgvAttendees.Rows.Count; rowIndex++)
                {
                    DataGridViewCheckBoxCell checkBox = (dgvAttendees["selectRecord", rowIndex] as DataGridViewCheckBoxCell);
                    checkBox.Value = headerCheckBox.Checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void DeleteAttachmentFile()
        {
            log.LogMethodEntry();
            if (attachFile != null && string.IsNullOrEmpty(attachFile) == false)
            {
                //Delete the image created in the image folder once Email is sent successfully//
                FileInfo file = new FileInfo(attachFile);
                if (file.Exists)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
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
