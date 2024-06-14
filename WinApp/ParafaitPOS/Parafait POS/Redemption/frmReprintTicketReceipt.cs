/********************************************************************************************
*Project Name -  frmReprintTicketReceipt                                                                         
*Description  -  Form to reprint ticket receipt
*************
**Version Log
*************
*Version      Date                   Modified By                 Remarks          
*********************************************************************************************
*2.80         20-Aug-2019            Archana                     Created  
*2.80         18-Dec-2019            Jinto Thomas        Added parameter execution context for userbl declaration with userid 
**********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.User;

namespace Parafait_POS.Redemption
{
    public partial class frmReprintTicketReceipt : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DeviceClass cardReader;
        private ExecutionContext executionContext;
        private readonly TagNumberParser tagNumberParser;
        private DataTable dtFromTime = new DataTable();
        private DataTable dtToTime = new DataTable();
        private BindingSource bsFromTime = new BindingSource();
        private BindingSource bsToTime = new BindingSource();
        private Double fromTimeForSearch;
        private Double toTimeForSearch;
        private VirtualKeyboardController virtualKeyboard;
        private string parentScreenNumber = string.Empty;
        private Utilities utilities;

        internal delegate void SetLastActivityTimeDelegate();
        internal SetLastActivityTimeDelegate SetLastActivityTime;
        public frmReprintTicketReceipt(DeviceClass cardReader, Utilities utilities)
        {
            log.LogMethodEntry(cardReader, utilities);
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            InitializeComponent();
            this.cardReader = cardReader;
            LoadTimeDropdownDropDown();
            tagNumberParser = new TagNumberParser(executionContext);
            RegisterDevices();
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnKeyPad }, ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"));
            log.LogMethodExit();
        }

       

        private void frmReprintTicketReceipt_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetLastActivityTime();
            ClearSearchFields();
            SetParentScreenNumber();
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                SearchTicketReceipts();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SearchTicketReceipts()
        {
            log.LogMethodEntry();
            try
            {
                dgvTicketDetails.Rows.Clear();
                ValidateSearchFields();
                TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                List<TicketReceiptDTO> ticketReceiptDTOList = null;
                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                if (string.IsNullOrEmpty(txtCardNumber.Text) && string.IsNullOrEmpty(txtTicketNumber.Text))
                {
                    DateTime issueFromDate = dtpIssueDate.Value.Date.AddHours(fromTimeForSearch);
                    DateTime issueToTime = dtpIssueDate.Value.Date.AddHours(toTimeForSearch);
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE, issueFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture).ToString()));
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE, issueToTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(txtCardNumber.Text))
                {
                    searchParams = GetSearchParamsForCard(txtCardNumber.Text);
                }
                if (!string.IsNullOrEmpty(txtTicketNumber.Text))
                {
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO_LIKE, txtTicketNumber.Text.ToString()));
                }
                if (searchParams.Count > 0)
                {
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS_FROM, "1"));
                    searchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    ticketReceiptDTOList = ticketReceiptList.GetAllTicketReceipt(searchParams);

                    if (ticketReceiptDTOList != null && ticketReceiptDTOList.Count > 0)
                    {
                        RefreshTicketDetailsGrid(ticketReceiptDTOList);
                    }
                    else
                    {
                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1670), MessageContainerList.GetMessage(executionContext, 2693, parentScreenNumber));
                    }
                }
                else
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1670), MessageContainerList.GetMessage(executionContext, 2693, parentScreenNumber));
                    log.LogMethodExit();
                    return;
                }
            } 
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(2254), MessageContainerList.GetMessage(executionContext, 2693, parentScreenNumber));
            }
            log.LogMethodExit();
        }
        
        private void ValidateDateField()
        {
            log.LogMethodEntry();
            fromTimeForSearch = 0;
            toTimeForSearch = 0;
            if (cmbSearchFromTime.SelectedValue != null && cmbSearchFromTime.SelectedIndex != 0)
            {
                fromTimeForSearch = Convert.ToDouble(cmbSearchFromTime.SelectedValue);
            }
            if (Convert.ToDouble(cmbSearchToTime.SelectedValue) > Convert.ToDouble(cmbSearchFromTime.SelectedValue))
            {
                toTimeForSearch = Convert.ToDouble(cmbSearchToTime.SelectedValue);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 305),
                                              MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                              MessageContainerList.GetMessage(executionContext, "From Time"),
                                              MessageContainerList.GetMessage(executionContext, 305));
            }
            log.LogMethodExit();
        }

        private void RefreshTicketDetailsGrid(List<TicketReceiptDTO> ticketReceiptListDTO)
        {
            log.LogMethodEntry(ticketReceiptListDTO);
            ticketReceiptDTOBindingSource.DataSource = ticketReceiptListDTO;
            dgvTicketDetails.Columns["issueDateDataGridViewTextBoxColumn"].DefaultCellStyle = POSStatic.Utilities.gridViewDateTimeCellStyle();
            dgvTicketDetails.Columns["balanceTicketsDataGridViewTextBoxColumn"].DefaultCellStyle = POSStatic.Utilities.gridViewNumericCellStyle();
            
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (dgvTicketDetails != null && dgvTicketDetails.Rows.Count > 0 && dgvTicketDetails.CurrentRow.Cells["idDataGridViewTextBoxColumn"].Value != null)
                {
                    int selectedTicketReceiptId = Convert.ToInt32(dgvTicketDetails.CurrentRow.Cells["idDataGridViewTextBoxColumn"].Value);
                    RedemptionUserLogsDTO redemptionUserLogsDTO = new RedemptionUserLogsDTO();
                    redemptionUserLogsDTO.Action = RedemptionUserLogsDTO.RedemptionAction.REPRINT_TICKET.ToString();
                    redemptionUserLogsDTO.LoginId = POSStatic.ParafaitEnv.LoginID;
                    redemptionUserLogsDTO.TicketReceiptId = selectedTicketReceiptId;

                    if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(POSStatic.Utilities.ExecutionContext, "MANAGER_APPROVAL_TO_REPRINT_TICKET_RECEIPT"))
                    {
                        int managerId = -1;
                        if (DoManagerAuthenticationCheck(ref managerId))
                        {
                            Users approveUser = new Users(executionContext, managerId);
                            redemptionUserLogsDTO.ApproverId = approveUser.UserDTO.LoginId;
                            redemptionUserLogsDTO.ApprovalTime = POSStatic.Utilities.getServerTime();
                        }
                        else
                        {
                            throw new ApprovalMandatoryException(POSStatic.MessageUtils.getMessage("Manager approval required"));
                        }
                    }
                    Semnox.Parafait.Redemption.PrintRedemptionReceipt printRedemptionReceipt = new Semnox.Parafait.Redemption.PrintRedemptionReceipt(POSStatic.Utilities.ExecutionContext, POSStatic.Utilities);
                    printRedemptionReceipt.ReprintManualTicketReceipt(selectedTicketReceiptId, redemptionUserLogsDTO);
                }
                else
                {
                    log.LogMethodExit();
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(145), MessageContainerList.GetMessage(executionContext, 2693, parentScreenNumber));
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(executionContext, 2693, parentScreenNumber));
            }
            log.LogMethodExit();
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e is DeviceScannedEventArgs)
            {
                SetLastActivityTime();
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;

                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                string CardNumber = tagNumber.Value;

                try
                {
                    HandleCardRead(CardNumber);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void HandleCardRead(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            SetLastActivityTime();
            if (!string.IsNullOrEmpty(cardNumber))
            {
                txtCardNumber.Text = cardNumber;
                SearchTicketReceipts();
            }
            log.LogMethodExit();
        }

        private List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> GetSearchParamsForCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
            if (cardNumber != "")
            {
                RedemptionListBL redemptionListBL = new RedemptionListBL();
                List<RedemptionDTO> redemptionListDTO = new List<RedemptionDTO>();
                List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> searchRedemptionParams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>
                {
                    new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.PRIMARY_CARD, cardNumber),
                    new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString())
                };
                redemptionListDTO = redemptionListBL.GetRedemptionDTOList(searchRedemptionParams);
                string redemptionIdList = string.Empty;
                if (redemptionListDTO != null && redemptionListDTO.Count > 0)
                {
                    foreach (RedemptionDTO item in redemptionListDTO)
                    {
                        redemptionIdList = redemptionIdList + item.RedemptionId + ",";
                    }
                }
                if (string.IsNullOrEmpty(redemptionIdList) == false)
                {
                    redemptionIdList = redemptionIdList.Substring(0, redemptionIdList.Length - 1);
                    TicketReceiptList ticketReceiptList = new TicketReceiptList(executionContext);
                    searchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>
                    {
                        new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID_LIST, redemptionIdList)
                    };                    
                }
            }
            log.LogMethodExit(searchParams);
            return searchParams;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                ClearSearchFields();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void ClearSearchFields()
        {
            log.LogMethodEntry();
            txtCardNumber.Text = string.Empty;
            txtTicketNumber.Text = string.Empty;
            dtpIssueDate.CustomFormat = "ddd, dd - MMM - yyyy";
            dtpIssueDate.Value = POSStatic.Utilities.getServerTime().Date;
            cmbSearchFromTime.SelectedValue = 9;
            cmbSearchToTime.SelectedValue = 18;
            dgvTicketDetails.Rows.Clear();
            log.LogMethodExit();
        }

        private bool DoManagerAuthenticationCheck(ref int managerId)
        {
            log.LogMethodEntry();
            string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
            int savRoleId = POSStatic.ParafaitEnv.RoleId;
            bool returnValue = false;
            try
            {
                POSStatic.ParafaitEnv.Manager_Flag = utilities.ParafaitEnv.Manager_Flag;
                POSStatic.ParafaitEnv.RoleId = utilities.ParafaitEnv.RoleId;
                returnValue = RedemptionAuthentication.RedemptionAuthenticateManger(cardReader, ref managerId);
            }
            finally
            {
                POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                POSStatic.ParafaitEnv.RoleId = savRoleId;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        void ValidateSearchFields()
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (string.IsNullOrEmpty(txtCardNumber.Text) && string.IsNullOrEmpty(txtTicketNumber.Text))
            {
                ValidateDateField();
            }
            if (!string.IsNullOrEmpty(txtCardNumber.Text) && txtCardNumber.Text.Length < 4)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "CardNumber"),
                                                            POSStatic.Utilities.MessageUtils.getMessage(2256)));
            }
            else if (!string.IsNullOrEmpty(txtTicketNumber.Text)  && txtTicketNumber.Text.Length < 10)
            {
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Validation Error"),
                                                            MessageContainerList.GetMessage(executionContext, "Ticket Receipt No"),
                                                            POSStatic.Utilities.MessageUtils.getMessage(2255)));
            }
            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                log.LogMethodExit(validationErrorList);
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
            }
            log.LogMethodExit();
        }

        private void LoadTimeDropdownDropDown()
        {
            log.LogMethodEntry();
            dtFromTime.Columns.Add("Display");
            dtFromTime.Columns.Add("Value");
            dtFromTime.Columns.Add(new DataColumn("Compare", typeof(DateTime)));

            dtToTime.Columns.Add("Display");
            dtToTime.Columns.Add("Value");

            DateTime startTime = POSStatic.Utilities.getServerTime().Date;
            while (startTime < POSStatic.Utilities.getServerTime().Date.AddDays(1))
            {
                dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
                dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));
                startTime = startTime.AddMinutes(60);
            }
            //final entry for 11:59 PM
            startTime = startTime.AddMinutes(-1);
            dtFromTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2), startTime);
            dtToTime.Rows.Add(startTime.ToString("h:mm tt"), startTime.Hour + Math.Round(startTime.Minute / 100.0, 2));

            cmbSearchFromTime.DisplayMember = "Display";
            cmbSearchFromTime.ValueMember = "Value";
            cmbSearchFromTime.DataSource = dtFromTime;
            cmbSearchToTime.DisplayMember = "Display";
            cmbSearchToTime.ValueMember = "Value";
            cmbSearchToTime.DataSource = dtToTime;
            log.LogMethodExit();
        }

        private void dtpIssueDate_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        internal void RegisterDevices()
        {
            log.LogMethodEntry();
            if (cardReader != null)
            {
                cardReader.Register(CardScanCompleteEventHandle);
            }
            log.LogMethodExit();
        }

        internal void UnregisterDevices()
        {
            log.LogMethodEntry(); 
            if (cardReader != null)
                cardReader.UnRegister();
            log.LogMethodExit();
        }

        private void frmReprintTicketReceipt_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            UnregisterDevices();
            log.LogMethodExit();
        }

        private void dgvTicketDetails_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime(); 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvTicketDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void SetParentScreenNumber()
        {
            log.LogMethodEntry();
            try
            {
                frmRedemptionAdmin parentForm = this.Owner as frmRedemptionAdmin;
                if (parentForm != null)
                {
                    parentScreenNumber = parentForm.GetParentScreenNumber;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
