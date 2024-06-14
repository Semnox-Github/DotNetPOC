/********************************************************************************************
 * Project Name - Redemption
 * Description  - TicketReciept UI
 * 
 **************
 **Version Log
 **************
 *Version       Date                 Modified By          Remarks          
 *********************************************************************************************
 *2.70.2          12-Aug-2019          Deeksha              Modified logger methods.
 *2.70.3          17-Apr-2020          Archana              Added keyboard button and clear button
 *2.80.0          20-Apr-2020          Guru A               Modified for redemption UI enhancements 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    /// <summary>
    /// Ticket Reciept UI
    /// </summary>
    public partial class TicketReceiptUI : Form
    {
        private Utilities utilities;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VirtualKeyboardController virtualKeyboard;
        public delegate bool DoManagerAuthenticationCheckDelegate(ref int managerId);
        public DoManagerAuthenticationCheckDelegate DoManagerAuthenticationCheck;
        int redemptionId = -1;
        public delegate void SetLastActivityTimeDelegate();
        public SetLastActivityTimeDelegate SetLastActivityTime;

        /// <summary>
        /// Ticket Receipt UI constructor
        /// </summary>
        public TicketReceiptUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;            
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(machineUserContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }
        /// <summary>
        /// Ticket Receipt UI constructor
        /// </summary>
        public TicketReceiptUI(Utilities _Utilities, string receiptNo, int redemptionId = -1, bool isSuspect = true)
            : this(_Utilities)
        {
            log.LogMethodEntry(_Utilities, receiptNo, redemptionId, isSuspect);
            txtReceiptNo.Text = receiptNo;
            this.redemptionId = redemptionId;
            chbFlagged.Checked = isSuspect;
            PopulateTicketReceipt();
            log.LogMethodExit();
        }
        private void PopulateTicketReceipt()
        {
            log.LogMethodEntry();
            TicketReceiptList ticketReceiptList = new TicketReceiptList(machineUserContext);
            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> ticketReceiptSearchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
            ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED, ((chbFlagged.Checked) ? "1" : "0")));
            ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            if (!string.IsNullOrEmpty(txtReceiptNo.Text))
            {
                ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, txtReceiptNo.Text));
            }
            if (!string.IsNullOrEmpty(txtBalanceTicketFrom.Text))
            {
                ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_FROM, txtBalanceTicketFrom.Text));
            }
            if (!string.IsNullOrEmpty(txtBalanceTicketTo.Text))
            {
                ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_TO, txtBalanceTicketTo.Text));
            }
            if (!string.IsNullOrEmpty(txtFromdate.Text))
            {
                try
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_FROM_TIME, Convert.ToDateTime(txtFromdate.Text).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                catch(Exception ex)
                {
                    log.Error("Error while executing PopulateTicketReceipt()" + ex.Message);
                    MessageBox.Show(utilities.MessageUtils.getMessage("Invalid From Date"));
                    log.LogMethodExit();
                    return;
                }
            }
            if (!string.IsNullOrEmpty(txtTodate.Text))
            {
                try
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.UPDATED_TO_TIME, Convert.ToDateTime(txtTodate.Text).ToString("yyyy-MM-dd HH:mm:ss")));
                }
                catch(Exception ex)
                {
                    log.Error("Error while executing PopulateTicketReceipt()" + ex.Message);
                    MessageBox.Show(utilities.MessageUtils.getMessage("Invalid To Date"));
                    log.LogMethodExit();
                    return;
                }
            }
            List<TicketReceiptDTO> ticketReceiptListOnDisplay = ticketReceiptList.GetAllTicketReceipt(ticketReceiptSearchParams);
            BindingSource ticketReceiptListBS = new BindingSource();
            if (ticketReceiptListOnDisplay != null)
            {
                SortableBindingList<TicketReceiptDTO> TicketReceiptDTOSortList = new SortableBindingList<TicketReceiptDTO>(ticketReceiptListOnDisplay);
                ticketReceiptListBS.DataSource = TicketReceiptDTOSortList;
            }
            else
                ticketReceiptListBS.DataSource = new SortableBindingList<TicketReceiptDTO>();
            dgvManualTicket.DataSource = ticketReceiptListBS;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
               FireSetLastAcitivityTime();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
               FireSetLastAcitivityTime();
                lblMessage.Text = "";
                ValidateSearchFields();
                PopulateTicketReceipt();
               FireSetLastAcitivityTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(); 
            lblMessage.Text = "";
            bool isManagerApproved = false;
            try
            {
               FireSetLastAcitivityTime();
                CommonFuncs.Utilities = utilities;
                CommonFuncs.ParafaitEnv = utilities.ParafaitEnv;
                if (dgvManualTicket.Rows.Count > 0)
                {
                    BindingSource ticketReceiptListBS = (BindingSource)dgvManualTicket.DataSource;
                    var ticketReceiptListOnDisplay = (SortableBindingList<TicketReceiptDTO>)ticketReceiptListBS.DataSource;
                    if (ticketReceiptListOnDisplay.Count > 0)
                    {
                        foreach (TicketReceiptDTO ticketReceiptDTO in ticketReceiptListOnDisplay)
                        {
                            if (ticketReceiptDTO.IsChanged)
                            {
                                if (!isManagerApproved)
                                {
                                    int mgrId = -1;
                                    
                                    if (DoManagerAuthenticationCheck != null)
                                    {
                                        isManagerApproved = DoManagerAuthenticationCheck(ref mgrId);
                                    }
                                    else
                                    {
                                        isManagerApproved = Authenticate.Manager();
                                    }
                                    if (!isManagerApproved)
                                    {
                                        break;
                                    }
                                }
                                ApplicationRemarksUI applicationRemarksUI = new ApplicationRemarksUI("", "ManualTicketReceipts", ticketReceiptDTO.Guid, utilities);
                                applicationRemarksUI.setCallBack = this.SaveTicket;
                                applicationRemarksUI.SetLastActivityTime += new ApplicationRemarksUI.SetLastActivityTimeDelegate(SetLastActivityTime);
                                applicationRemarksUI.Owner = this;
                                DialogResult dresult = applicationRemarksUI.ShowDialog();
                                if (dresult == DialogResult.OK)
                                {
                                    TicketReceipt ticketReceipt = new TicketReceipt(machineUserContext, ticketReceiptDTO);
                                    ticketReceipt.Save(null);
                                    lblMessage.Text = utilities.MessageUtils.getMessage(122);
                                }
                                else if (dresult == DialogResult.Cancel)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("Save Error."));
                                    log.LogMethodExit();
                                    return;
                                }

                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Save Error." + ex.Message));
                log.Error("Ends-btnSave_Click() Event" + ex.ToString());
            }
            log.LogMethodExit();
        }

        private void SaveTicket(string guid, ApplicationRemarksDTO applicationRemarkDTO)
        {
            log.LogMethodEntry(guid, applicationRemarkDTO);
           FireSetLastAcitivityTime();
            if (!string.IsNullOrEmpty(applicationRemarkDTO.Remarks))
            {
                ApplicationRemarks applicationRemarks = new ApplicationRemarks(machineUserContext,applicationRemarkDTO);
                applicationRemarks.Save(null);
            }
            else
            {
                throw new Exception(utilities.MessageUtils.getMessage(201));
            }
            log.LogMethodExit();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtTodate.Text = dtpToDate.Text;
           FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtFromdate.Text = dtpFromDate.Text;
           FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void dgvManualTicket_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
               FireSetLastAcitivityTime();
                if (dgvManualTicket.Columns[e.ColumnIndex].Name.Equals("btnRowRemarks"))
                {
                    BindingSource ticketReceiptListBS = (BindingSource)dgvManualTicket.DataSource;
                    var ticketReceiptListOnDisplay = (SortableBindingList<TicketReceiptDTO>)ticketReceiptListBS.DataSource;
                    if (ticketReceiptListOnDisplay.Count >= e.RowIndex)
                    {
                        if (!string.IsNullOrEmpty(ticketReceiptListOnDisplay[e.RowIndex].Guid))
                        {
                            ApplicationRemarksUI applicationRemarksUI = new ApplicationRemarksUI("", "ManualTicketReceipts", ticketReceiptListOnDisplay[e.RowIndex].Guid, utilities);
                            applicationRemarksUI.setCallBack = this.SaveTicket;
                            applicationRemarksUI.SetLastActivityTime += new ApplicationRemarksUI.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                            applicationRemarksUI.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void TicketReceiptUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
           FireSetLastAcitivityTime();
            if (this.Owner == null)// this code will set the form style to the Parafait POS form incase if the form is used in other application please set the owner 
            {
                this.dgvManualTicket.Refresh();
                utilities.setupDataGridProperties(ref this.dgvManualTicket);
                this.dgvManualTicket.BackgroundColor = SystemColors.Control;
                this.BackColor = SystemColors.Control;
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
               FireSetLastAcitivityTime();
                ClearSearchFields();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void ClearSearchFields()
        {
            log.LogMethodEntry();
            txtReceiptNo.Text = String.Empty;
            txtBalanceTicketFrom.Text = string.Empty;
            txtBalanceTicketTo.Text = string.Empty;
            txtFromdate.Text = string.Empty;
            txtTodate.Text = string.Empty;
            chbFlagged.Checked = false;
            dgvManualTicket.Rows.Clear();
            log.LogMethodExit();
        }

        private void txtBalanceTicketFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
           FireSetLastAcitivityTime();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            log.LogMethodExit();
        }

        void ValidateSearchFields()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            int fromBalanceTicketValue = 0;
            int toBalanceTicketValue = 0;
            if (!string.IsNullOrWhiteSpace(txtBalanceTicketFrom.Text))
            {
                fromBalanceTicketValue = Convert.ToInt32(txtBalanceTicketFrom.Text);
            }

            if (!string.IsNullOrWhiteSpace(txtBalanceTicketTo.Text))
            {
                toBalanceTicketValue = Convert.ToInt32(txtBalanceTicketTo.Text);
            }

            if (fromBalanceTicketValue > toBalanceTicketValue)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(machineUserContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(machineUserContext, "Balance Ticket Between"),
                                                            MessageContainerList.GetMessage(machineUserContext, 2639)));//To Balance Ticket must be greater than From Balance Ticket Value
            }


            if (!string.IsNullOrWhiteSpace(txtFromdate.Text))
            {
                if (DateTime.TryParse(txtFromdate.Text, out fromDate) == false)
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(machineUserContext, "Validation Error"),
                                                          MessageContainerList.GetMessage(machineUserContext, "From Date"),
                                                          MessageContainerList.GetMessage(machineUserContext, 15)));
                }

            }

            if (!string.IsNullOrWhiteSpace(txtTodate.Text))
            {
                if (DateTime.TryParse(txtTodate.Text, out toDate) == false)
                {
                    validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(machineUserContext, "Validation Error"),
                                                          MessageContainerList.GetMessage(machineUserContext, "To Date"),
                                                          MessageContainerList.GetMessage(machineUserContext, 15)));
                }
            }

            if (toDate < fromDate)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(machineUserContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(machineUserContext, "Date"),
                                                            MessageContainerList.GetMessage(machineUserContext, 724)));

            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                log.LogMethodExit(validationErrorList);
                throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, "Validation Error"), validationErrorList);
            }
            log.LogMethodExit();

        }

        private void SearchFieldEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
           FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void SearchFieldKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
           FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void chbFlagged_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void dgvManualTicket_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
           FireSetLastAcitivityTime();
            log.LogMethodExit();
        }

        private void FireSetLastAcitivityTime()
        {
            log.LogMethodEntry();
            if (SetLastActivityTime != null)
            {
                SetLastActivityTime();
            }
            log.LogMethodExit();

        }
    }
}
