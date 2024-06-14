/********************************************************************************************
 * Project Name - Parafait_POS.Redemption
 * Description  -frmRedemptionAdmin
 * 
 **************
 **Version Log
 **************
 *Version      Date                   Modified By                 Remarks             
 *********************************************************************************************
 *2.80         28-Aug-2019            Archana                     Ticket Receipt reprint changes and  
                                                                  Manager approval form card tap issue fix 
 *2.80         16-Sep-2019            Dakshakh raj                Redemption currency rule enhancement   
 *2.100        11-Oct-2020            Deeksha                     Issue-Fix : From Redemption admin screen, able to add manual tickets 
 *                                                                            even though enable manual ticket option is disabled.
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Parafait_POS.Redemption
{
    public partial class frmRedemptionAdmin : Form
    {
        clsRedemption _redemption;
        DeviceClass cardReader, _barcodeScanner;
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        //Start update 30-May-2017
        string _loginID = "";
        string _screenNumber = "";
        //End update 30-May-2017
        string mgrApprovalRequired = "";
        int mgrApprovalLimit = 0;
        Utilities _utilities; 

        public delegate void SetCurrencyRuleCheckFlagDelegate();
        public SetCurrencyRuleCheckFlagDelegate setCurrencyRuleCallBack;

        public delegate void RefreshMethodDelegate();
        public RefreshMethodDelegate setRefreshCallBack;

        public delegate void SetLastActivityTimeDelegate();
        public SetLastActivityTimeDelegate SetLastActivityTime;

        internal string GetParentScreenNumber { get { return _screenNumber; } }

        /// <summary>
        /// frmRedemptionAdmin
        /// </summary>
        /// <param name="cardReader">cardReader</param>
        /// <param name="barcodeScanner">barcodeScanner</param>
        /// <param name="inRedemption">inRedemption</param>
        /// <param name="LoginID">LoginID</param>
        /// <param name="ScreenNumber">ScreenNumber</param>
        /// <param name="utilities">utilities</param>
        public frmRedemptionAdmin(DeviceClass cardReader, DeviceClass barcodeScanner, clsRedemption inRedemption, string LoginID, string ScreenNumber, Utilities utilities)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry(cardReader, barcodeScanner, inRedemption, LoginID, ScreenNumber, utilities);
            InitializeComponent();
            _redemption = inRedemption;
            this.cardReader = cardReader;
            _barcodeScanner = barcodeScanner;
            _loginID = LoginID;
            _screenNumber = ScreenNumber; 
            _utilities = new Utilities();
            _utilities = utilities;
            log.LogMethodExit();
        }

        /// <summary>
        /// btnAdvanced_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnAdvanced_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e); 
            try
            {
                SetLastActivityTime();
                frm_redemption f = new frm_redemption(_utilities, cardReader, _barcodeScanner, _loginID, _screenNumber, true);
                //Start update 29-May-2017
                f.Location = this.Location;
                f.Location = this.Location;
                f.Width = this.Width;
                f.Height = this.Height;
                f.AutoScroll = true;
                f.Owner = this;
                f.SetLastActivityTime += new frm_redemption.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                f.Show();
                //End update 29-May-2017
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                log.Error(ex);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// frmRedemptionAdmin_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmRedemptionAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetDGVFormat(); 
            btnAdvanced.Focus();
            if (Owner != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(Owner.Location.X + Owner.Width / 2 - Width / 2,
                                        Owner.Location.Y + Owner.Height / 2 - Height / 2);
            }
            else
                this.StartPosition = FormStartPosition.CenterScreen;

            dgvVouchers.AutoSize = dgvCards.AutoSize = dgvCurrencies.AutoSize = true;
            refreshScreen();
            string mgtLimitValue = _utilities.getParafaitDefaults("ADD_TICKET_LIMIT_FOR_MANAGER_APPROVAL_REDEMPTION");
            try
            {
                if (mgtLimitValue != "")
                    mgrApprovalLimit = Convert.ToInt32(mgtLimitValue);
                else
                    mgrApprovalLimit = 0;
            }
            catch { mgrApprovalLimit = 0; }
            mgrApprovalRequired = _utilities.getParafaitDefaults("MANAGER_APPROVAL_TO_ADD_MANUAL_TICKET");
            HideReprintTicketButtonBasedOnUserRoleAccess(_loginID);
            SetLastActivityTime();
            txtManualTickets.Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
            lblManualTickets.Visible = POSStatic.ENABLE_MANUAL_TICKET_IN_REDEMPTION;
            log.LogMethodExit();
        }

        /// <summary>
        /// refreshScreen
        /// </summary>
        void refreshScreen()
        {
            log.LogMethodEntry();
            if (_redemption != null)
            {
                dgvCards.Rows.Clear();
                dgvVouchers.Rows.Clear();
                dgvCurrencies.Rows.Clear();
                foreach (clsRedemption.clsCards item in _redemption.cardList)
                {
                    dgvCards.Rows.Add("X", item.cardNumber, item.Tickets);
                }

                foreach (clsRedemption.clsScanTickets item in _redemption.scanTicketList)
                {
                    dgvVouchers.Rows.Add("X", item.barCode, item.Tickets);
                }

                foreach (clsRedemption.clsRedemptionCurrency item in _redemption.currencyList)
                    if (item.redemptionCurrencyRuleId == -1)
                    {
                        dgvCurrencies.Rows.Add("X", item.redemptionCurrencyRuleName, item.currencyId, item.currencyName, item.ValueInTickets, item.quantity, item.ValueInTickets * item.quantity,  "-");
                    }
                    else
                    {
                        dgvCurrencies.Rows.Add("X" , item.redemptionCurrencyRuleName, item.currencyId, item.currencyName, item.redemptionRuleTicketDelta, item.quantity, item.redemptionRuleTicketDelta * item.quantity, "-");

                    }


                //nudManualTickets.Value = _redemption.getManualTickets();
                txtManualTickets.Text = Convert.ToString(_redemption.getManualTickets());

                int total = _redemption.getCurrencyTickets() + _redemption.getManualTickets() + _redemption.getPhysicalTickets();
                lblTotalPhysicalTickets.Text = total.ToString();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// ProcessCmdKey
        /// </summary>
        /// <param name="msg">msg</param>
        /// <param name="keyData">keyData</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            bool retValue = false;
            try
            {
                SetLastActivityTime();
                if (keyData == Keys.Enter)
                { 
                    retValue= true;
                }
                else
                {
                    retValue = base.ProcessCmdKey(ref msg, keyData);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(retValue); 
            return retValue;
        }
        

        /// <summary>
        /// frmRedemptionAdmin_FormClosed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void frmRedemptionAdmin_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            FireCallBackEvents();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnClose_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvCards_CellClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvCards_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    string cardNumber = dgvCards["dcCardNumber", e.RowIndex].Value.ToString();
                    _redemption.removeCard(cardNumber);
                    refreshScreen();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// txtManualTickets_KeyPress
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void txtManualTickets_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (!char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// txtManualTickets_Enter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void txtManualTickets_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (mgrApprovalRequired == "Y")
                {
                    //if (mgrApprovalLimit ==0)
                    //{
                    //int mgrId = -1;
                    //string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
                    //POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                    if (!DoManagerAuthenticationCheck(ref _utilities.ParafaitEnv.ManagerId))/*!Authenticate.Manager(ref _utilities.ParafaitEnv.ManagerId))*/
                    {
                        //POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                        POSUtils.ParafaitMessageBox(_utilities.MessageUtils.getMessage(1217), MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                        log.Warn("btnAddManual_Click() as Manager Approval Required for this Task -Add Manual Ticket");
                        this.ActiveControl = null;
                        return;
                    }
                    //}
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(_utilities.MessageUtils.getMessage("Requires Manager Approval"), MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
            }
            finally
            {
                _utilities.ParafaitEnv.ManagerId = -1;
            }
            ShowKeyPad();
            log.LogMethodExit();
        }

        NumberPad numPad = null;
        Panel NumberPadVarPanel;

        /// <summary>
        /// ShowKeyPad
        /// </summary>
        private void ShowKeyPad()
        {
            log.LogMethodEntry();
            if (numPad == null)
            {
                numPad = new NumberPad("#0");
                NumberPadVarPanel = numPad.NumPadPanel();
                //this.Controls.Add(NumberPadVarPanel);
                panel1.Controls.Add(NumberPadVarPanel);

                numPad.setReceiveAction = EventnumPadOKReceived;
                numPad.setKeyAction = EventnumPadKeyPressReceived;

                this.KeyPreview = true;

                this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
                NumberPadVarPanel.VisibleChanged += NumberPadVarPanel_VisibleChanged;
            }




            NumberPadVarPanel.Location = new System.Drawing.Point((this.panel1.Width - NumberPadVarPanel.Width) / 2, (this.panel1.Height - NumberPadVarPanel.Height) / 2);
            numPad.GetKey('0');
            numPad.NewEntry = true;
            NumberPadVarPanel.Visible = true;
            NumberPadVarPanel.BringToFront();
            //}
            log.LogMethodExit();
        }

        /// <summary>
        /// NumberPadVarPanel_VisibleChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void NumberPadVarPanel_VisibleChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (NumberPadVarPanel.Visible == false)
            //    lastScanObject.isManualTicket = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// FormNumPad_KeyPress
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (e.KeyChar == (char)Keys.Escape)
                    NumberPadVarPanel.Visible = false;
                else
                {
                    numPad.GetKey(e.KeyChar);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// EventnumPadOKReceived
        /// </summary>
        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();
            try
            {
                SetLastActivityTime();
                string message = "";
                if (numPad.ReturnNumber > 0)
                {
                    //if (mgrApprovalRequired == "Y")
                    // {
                    if (mgrApprovalLimit > 0 && numPad.ReturnNumber > mgrApprovalLimit && _utilities.ParafaitEnv.ManagerId == -1)
                    {
                        //int mgrId = -1;
                        //string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
                        //POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                        if (!DoManagerAuthenticationCheck(ref _utilities.ParafaitEnv.ManagerId))/*!Authenticate.Manager(ref _utilities.ParafaitEnv.ManagerId))*/
                        {
                            //POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                            POSUtils.ParafaitMessageBox(_utilities.MessageUtils.getMessage(1217), MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                            log.Warn("btnAddManual_Click() as Manager Approval Required for this Task -Add Manual Ticket");
                            return;
                        }
                        _utilities.ParafaitEnv.ManagerId = -1; //since there is not transaction level check for add manual we can reset here. Else reset it after transaction
                    }
                    //}

                    if (!_redemption.addManualTickets((int)numPad.ReturnNumber, ref message))
                    {
                        POSUtils.ParafaitMessageBox(message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                        log.Error("EventnumPadOKReceived() -addManualTickets- error: " + message);
                    }
                    else
                    {
                        refreshScreen();
                        POSUtils.ParafaitMessageBox(message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                        log.Warn("EventnumPadOKReceived() -addManualTickets- " + message);
                    }
                }


                NumberPadVarPanel.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void EventnumPadKeyPressReceived()
        {
        }
        /// <summary>
        /// dgvVouchers_CellClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvVouchers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    string voucherCode = dgvVouchers["dcVoucher", e.RowIndex].Value.ToString();
                    _redemption.removeVoucher(voucherCode);
                    refreshScreen();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnRefresh_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                refreshScreen();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvCurrencies_CellClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvCurrencies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                if (e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == 0)
                    {
                        int currencyId = Convert.ToInt32(dgvCurrencies["dcCurrencyId", e.RowIndex].Value);
                        if (currencyId != -1)
                        {
                            _redemption.removeCurrency(currencyId);
                            FireCallBackEvents();
                            refreshScreen();
                        }
                    }
                    else if (e.ColumnIndex == dcReduceCurrency.Index)
                    {
                        int currencyId = Convert.ToInt32(dgvCurrencies["dcCurrencyId", e.RowIndex].Value);
                        if (currencyId != -1)
                        {
                            _redemption.reduceCurrency(currencyId);
                            FireCallBackEvents();
                            refreshScreen();
                        }
                    }
                    if (e.ColumnIndex == dcIIncreaseCurrency.Index)
                    {
                        int currencyId = Convert.ToInt32(dgvCurrencies["dcCurrencyId", e.RowIndex].Value);
                        if (currencyId != -1)
                        {
                            _redemption.IncreaseCurrency(currencyId);
                            FireCallBackEvents();
                            refreshScreen();
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


        private void btnReprintTicket_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                frmReprintTicketReceipt frmReprintTicket = new frmReprintTicketReceipt(cardReader, _utilities);                
                frmReprintTicket.Location = this.Location;
                frmReprintTicket.Width = this.Width;
                frmReprintTicket.Height = this.Height;
                frmReprintTicket.AutoScroll = true;
                frmReprintTicket.Owner = this;
                frmReprintTicket.SetLastActivityTime += new frmReprintTicketReceipt.SetLastActivityTimeDelegate(this.SetLastActivityTime);
                frmReprintTicket.Show();

            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void HideReprintTicketButtonBasedOnUserRoleAccess(string loginId)
        {
            log.LogMethodEntry();
            Users users = new Users(_utilities.ExecutionContext, loginId);
            UsersDTO usersDTO = users.UserDTO;
            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(_utilities.ExecutionContext);
            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, "Parafait POS"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, "1"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FORM_NAME, "Reprint Ticket Receipt"));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, usersDTO.RoleId.ToString()));
            searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
            List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
            if (managementFormAccessDTOList == null)
            {
                btnReprintTicket.Visible = false;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// lnkPrintTotalPhysial_LinkClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void lnkPrintTotalPhysial_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                SetLastActivityTime();
                _redemption.RedeemTicketReceipt();
                refreshScreen();
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(_utilities.ExecutionContext, 2693, _screenNumber));
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        bool DoManagerAuthenticationCheck(ref int managerId)
        {
            log.LogMethodEntry();
            string savMgrFlag = POSStatic.ParafaitEnv.Manager_Flag;
            int savRoleId = POSStatic.ParafaitEnv.RoleId;
            bool returnValue = false;
            try
            {
                POSStatic.ParafaitEnv.Manager_Flag = _utilities.ParafaitEnv.Manager_Flag;
                POSStatic.ParafaitEnv.RoleId = _utilities.ParafaitEnv.RoleId;
                returnValue = RedemptionAuthentication.RedemptionAuthenticateManger(cardReader, ref _utilities.ParafaitEnv.ManagerId);
            }
            finally
            {
                //restore pos static manager flag value
                POSStatic.ParafaitEnv.Manager_Flag = savMgrFlag;
                POSStatic.ParafaitEnv.RoleId = savRoleId;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// dgvCurrencies_CellEnter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void dgvCurrencies_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex > -1)
            {
                dgvCurrencies[e.ColumnIndex, e.RowIndex].ReadOnly = true;
            }
            log.LogMethodExit();
        }

      

        private void FireCallBackEvents()
        {
            log.LogMethodEntry();
            setCurrencyRuleCallBack();
            setRefreshCallBack();
            log.LogMethodExit();
        }
         

        private void dgvCurrencies_Scroll(object sender, ScrollEventArgs e)
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

        private void dgvVouchers_Scroll(object sender, ScrollEventArgs e)
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

        private void dgvCards_Scroll(object sender, ScrollEventArgs e)
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

        private void SetDGVFormat()
        {
            log.LogMethodEntry();
            dgvCurrencies.Columns["dcRate"].DefaultCellStyle =
            dgvCurrencies.Columns["dcCurQuantity"].DefaultCellStyle =
            dgvCurrencies.Columns["dataGridViewTextBoxColumn3"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            dgvCards.Columns["dcCardTickets"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            dgvVouchers.Columns["DataGridViewTextBoxColumn2"].DefaultCellStyle = _utilities.gridViewNumericCellStyle();
            log.LogMethodExit();
        }
         
    }
}
