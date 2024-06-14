/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmSelectWaiverOption UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.70.2      1-Oct-2019    Dakshakh raj            Created for Waiver phase 2 enhancement changes  
 *2.120      18-May-2021     Dakshakh Raj           Handling text box fore color changes.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1     22-Feb-2023      Vignesh Bhat           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmSelectWaiverOptions : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private List<WaiverSetDTO> waiverSetDTOList;
        private List<WaiverSetDTO> selectedWaiverSetDTOList = null;
        private CustomerDTO signatoryCustomerDTO;
        private string waiverSetSelectionOption;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        public frmSelectWaiverOptions()
        {
            log.LogMethodEntry();
            this.utilities = KioskStatic.Utilities;
            signatoryCustomerDTO = null;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            DisplayReservationCodeTAndOTPOptions();
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            label1.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            //DisplaybtnCancel(true);
            DisplaybtnPrev(true);
            InitializeKeyboard();
            utilities.setLanguage(this);
            KioskStatic.logToFile("Loading select waiver options form");
            log.LogMethodExit();
        }

        private void SetTextForeColorAndFont()
        {
            log.LogMethodEntry();
            KioskStatic.setDefaultFont(this);
            SetTextBoxFontColors();
            log.LogMethodExit();
        }

        private void frmSelectWaiverOption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SetFocus();
            HideKeyboardObject();
            LoadWaiverSet();
            string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2406);
            DisplayMessageLine(msg);
            SetCustomerName();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void LoadWaiverSet()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Loading waiver set");
            ResetKioskTimer();
            waiverSetDTOList = new List<WaiverSetDTO>();
            WaiverSetContainer waiverSetContainer = WaiverSetContainer.GetInstance;
            List<WaiverSetDTO> waiverSetDTOListTemp = waiverSetContainer.GetWaiverSetDTOList(utilities.ExecutionContext.SiteId);
            if (waiverSetDTOListTemp != null && waiverSetDTOListTemp.Any())
            {
                waiverSetDTOList = RemoveManualWaiverSet(waiverSetDTOListTemp);
            }
            dgvWaiverSet.DataSource = waiverSetDTOList;
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    SetVirtualKeyboard();
                }
                else
                {
                    SetCustomKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing keyboard in Select Waiver Options screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(pnlReservationCode.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in Select Waiver Options screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(pnlReservationCode.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, label7.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Select Waiver Options screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void DisposeKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.Dispose();
                }
                else
                {
                    customKeyboardController.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Disposing keyboard in  Select Waiver Options screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private List<WaiverSetDTO> RemoveManualWaiverSet(List<WaiverSetDTO> waiverSetDTOListTemp)
        {
            log.LogMethodEntry(waiverSetDTOListTemp);
            ResetKioskTimer();
            KioskStatic.logToFile("Removing any waiver set with manual waivers");
            List<WaiverSetDTO> waiverSetDTOListLocal = new List<WaiverSetDTO>();
            for (int i = 0; i < waiverSetDTOListTemp.Count; i++)
            {
                //Skip manual sign waiver sets
                if (waiverSetDTOListTemp[i].WaiverSetSigningOptionDTOList != null
                    && waiverSetDTOListTemp[i].WaiverSetSigningOptionDTOList.Exists(signOpt => signOpt.OptionName == WaiverSetSigningOptionsDTO.WaiverSigningOptions.MANUAL.ToString()) == false)
                {
                    waiverSetDTOListLocal.Add(waiverSetDTOListTemp[i]);
                }
            }
            log.LogMethodExit(waiverSetDTOListLocal);
            return waiverSetDTOListLocal;
        }

        void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            KioskStatic.logToFile("user clicked proceed button");
            try
            {
                DisableButtons();
                txtReservationCode.BackColor = txtTrxOTP.BackColor = Color.White;
                selectedWaiverSetDTOList = new List<WaiverSetDTO>();
                string defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2406);
                DisplayMessageLine(defaultMsg);
                Validate();
                string eventCode = string.Empty;
                bool evenCodeWaiverSet = false;
                if (string.IsNullOrEmpty(txtReservationCode.Text) == false
                     || string.IsNullOrEmpty(txtTrxOTP.Text) == false)
                {
                    if (string.IsNullOrEmpty(txtReservationCode.Text) == false)
                    {
                        GetReservationCodeWaiverSetDTOList(txtReservationCode.Text);
                        eventCode = txtReservationCode.Text;
                    }
                    else if (string.IsNullOrEmpty(txtTrxOTP.Text) == false)
                    {
                        GetTrxOTPWaiverSetDTOList(txtTrxOTP.Text);
                        eventCode = txtTrxOTP.Text;
                    }
                    evenCodeWaiverSet = true;
                    selectedWaiverSetDTOList = RemoveManualWaiverSet(selectedWaiverSetDTOList);
                    if (selectedWaiverSetDTOList == null || selectedWaiverSetDTOList.Any() == false)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2423)
                                                    + ". " + MessageContainerList.GetMessage(utilities.ExecutionContext, 441));
                        //Event Code/Transaction OTP is linked with manual waiver set. pls contact staff
                    }
                }
                else
                {
                    GetSelectedWaiverSetDTOList();
                }
                if (selectedWaiverSetDTOList != null && selectedWaiverSetDTOList.Any())
                {
                    KioskStatic.logToFile("Show sign waiver UI with selected waiver sets");
                    log.Info("Show sign waiver UI with selected waiver sets");
                    using (frmSignWaivers frmSignWaivers = new frmSignWaivers(selectedWaiverSetDTOList, signatoryCustomerDTO, (evenCodeWaiverSet == true ? eventCode : string.Empty)))
                    {
                        frmSignWaivers.ShowDialog();
                        signatoryCustomerDTO = frmSignWaivers.GetSignatoryDTO;
                        SetCustomerName();
                    }
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2406);
                    throw new ValidationException(msg);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (waiverSetSelectionOption == "BOTH" || waiverSetSelectionOption == "RESERVATIONCODE")
                {
                    if (string.IsNullOrEmpty(txtReservationCode.Text) == false)
                    {
                        txtReservationCode.BackColor = Color.OrangeRed;
                    }
                }
                if (waiverSetSelectionOption == "BOTH" || waiverSetSelectionOption == "TRANSACTIONOTP")
                {
                    if (string.IsNullOrEmpty(txtTrxOTP.Text) == false)
                    {
                        txtTrxOTP.BackColor = Color.OrangeRed;
                    }
                }
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message))
                {
                    frmOK.ShowDialog();
                }
            }
            finally
            {
                EnableButtons();
                ResetKioskTimer();
            }
            log.LogMethodExit();

        }

        private void Validate()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Validate user input");
            ResetKioskTimer();
            int waiverSetCount = 0;
            int optionCount = 0;
            string msgNone = string.Empty;
            string msgMulti = string.Empty;
            foreach (DataGridViewRow row in dgvWaiverSet.Rows)
            {
                bool isSelected = Convert.ToBoolean(row.Cells["chooseWaiverSet"].Tag);
                if (isSelected)
                {
                    waiverSetCount++;
                }
            }
            if (waiverSetCount >= 1)
            {
                optionCount++;
            }
            msgMulti = MessageContainerList.GetMessage(utilities.ExecutionContext, 2599);// "Please select only one option to proceed");
            if (waiverSetSelectionOption == "BOTH")
            {

                if (string.IsNullOrEmpty(txtReservationCode.Text) == false)
                {
                    optionCount++;
                }

                if (string.IsNullOrEmpty(txtTrxOTP.Text) == false)
                {
                    optionCount++;
                }
                msgNone = MessageContainerList.GetMessage(utilities.ExecutionContext, 2600);// "Please select one of the options to proceed");

            }
            else if (waiverSetSelectionOption == "RESERVATIONCODE")
            {

                if (string.IsNullOrEmpty(txtReservationCode.Text) == false)
                {
                    optionCount++;
                }

                msgNone = MessageContainerList.GetMessage(utilities.ExecutionContext, 2601);// "Please enter Reservation Code or select waiver set(s) to proceed"); 
            }
            else if (waiverSetSelectionOption == "TRANSACTIONOTP")
            {

                if (string.IsNullOrEmpty(txtTrxOTP.Text) == false)
                {
                    optionCount++;
                }

                msgNone = MessageContainerList.GetMessage(utilities.ExecutionContext, 2602);// "Please enter Transaction OTP or select waiver set(s) to proceed");
            }

            if (optionCount == 0)
            {
                throw new ValidationException(msgNone);
            }
            if (optionCount > 1)
            {
                throw new ValidationException(msgMulti);
            }

            ResetKioskTimer();
            log.LogMethodExit();

        }

        private void GetReservationCodeWaiverSetDTOList(String eventCode)
        {
            log.LogMethodEntry(eventCode);
            ResetKioskTimer();
            KioskStatic.logToFile("Get Reservation code Waiver Set List");
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            try
            {
                selectedWaiverSetDTOList = transactionUtils.GetEventCodeWaiverSetDTOList(eventCode);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2603));
            }
            if (selectedWaiverSetDTOList == null || selectedWaiverSetDTOList.Any() == false)
            {
                //txtReservationCode.BackColor = Color.OrangeRed;
                //txtReservationCode.Focus();
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2603));
                //Please enter valid Reservation Code
            }
            ResetKioskTimer();
            log.LogMethodExit();

        }

        private void GetTrxOTPWaiverSetDTOList(String eventCode)
        {
            log.LogMethodEntry(eventCode);
            ResetKioskTimer();
            KioskStatic.logToFile("Get Transaction OTP Waiver Set List");
            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            try
            {
                selectedWaiverSetDTOList = transactionUtils.GetEventCodeWaiverSetDTOList(eventCode);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2604));
            }
            if (selectedWaiverSetDTOList == null || selectedWaiverSetDTOList.Any() == false)
            {
                //txtReservationCode.BackColor = Color.OrangeRed;
                //txtReservationCode.Focus();
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2604));
                //Please enter valid Transaction OTP
            }
            ResetKioskTimer();
            log.LogMethodExit();

        }


        private void GetSelectedWaiverSetDTOList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Get Selected Waiver Set List");
            List<WaiverSetDTO> waiverSetDTOListLocal = (List<WaiverSetDTO>)dgvWaiverSet.DataSource;
            for (int i = 0; i < dgvWaiverSet.Rows.Count; i++)
            {
                if (dgvWaiverSet.Rows[i].Cells["chooseWaiverSet"] != null
                    && dgvWaiverSet.Rows[i].Cells["chooseWaiverSet"].Tag != null)
                {
                    bool isSelected = Convert.ToBoolean(dgvWaiverSet.Rows[i].Cells["chooseWaiverSet"].Tag);
                    log.LogVariableState("isSelected", isSelected);
                    if (isSelected)
                    {
                        int waiverSetId = Convert.ToInt32(dgvWaiverSet.Rows[i].Cells["waiverSetIdDataGridViewTextBoxColumn"].Value);
                        log.LogVariableState("waiverSetId", waiverSetId);
                        selectedWaiverSetDTOList.Add(waiverSetDTOListLocal.Find(ws => ws.WaiverSetId == waiverSetId));
                    }
                }
            }
            log.LogMethodExit();

        }

        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void dgvWaiverSet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    DataGridViewRow dgvRow = dgvWaiverSet.CurrentRow;
                    dgvWaiverSet.BeginEdit(true);
                    if (dgvRow != null && dgvRow.Cells["chooseWaiverSet"].Selected)
                    {
                        if (this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag == null)
                        {
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = false;
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewUnTickedCheckBox;
                        }
                        if (this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.Equals(false))
                        {
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = true;
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewTickedCheckBox;
                        }
                        else
                        {
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag = false;
                            this.dgvWaiverSet.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = Properties.Resources.NewUnTickedCheckBox;
                        }
                    }
                    dgvWaiverSet.EndEdit();
                    dgvWaiverSet.RefreshEdit();
                }
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
            }
            log.LogMethodExit();
        }

        private void textReservationCodeBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void textTrxOTPBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }
        AlphaNumericKeyPad reservationCodekeypad;

        AlphaNumericKeyPad trxOTPkeypad;

        private void btnShowKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnShowKeypad_Click(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                DisableButtons();
                KioskStatic.logToFile("Cancel button is clicked");
                if (signatoryCustomerDTO != null)
                {
                    //This action will clear current customer session. Do you want to proceed?
                    using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                    {
                        if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }


        private void frmSelectWaiverOption_Closing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisposeKeyboardObject();
            KioskStatic.logToFile("Closing select waiver option form");
            log.LogMethodExit();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            ActiveControl = lblSelection;
            ActiveControl = txtReservationCode;
            log.LogMethodExit();
        }
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                if (signatoryCustomerDTO != null)
                {
                    //This action will clear current customer session. Do you want to proceed?
                    using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                    {
                        if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            base.btnHome_Click(sender, e);
                        }
                    }
                }
                else
                {
                    base.btnHome_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisableButtons();
                if (signatoryCustomerDTO != null)
                {
                    //This action will clear current customer session. Do you want to proceed?
                    using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                    {
                        if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                        {
                            base.btnHome_Click(sender, e);
                        }
                    }
                }
                else
                {
                    base.btnHome_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                EnableButtons();
            }
            log.LogMethodExit();
        }

        private void SetCustomerName()
        {
            log.LogMethodEntry();
            if (this.signatoryCustomerDTO != null && this.signatoryCustomerDTO.Id > -1)
            {
                lblCustomer.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer")
                                   + ": "
                                   + (string.IsNullOrEmpty(signatoryCustomerDTO.FirstName) ? string.Empty : signatoryCustomerDTO.FirstName) +
                                   " " +
                                   (string.IsNullOrEmpty(signatoryCustomerDTO.LastName) ? string.Empty : signatoryCustomerDTO.LastName);
            }
            else
            {
                lblCustomer.Text = string.Empty;
            }
            log.LogMethodExit();
        }

        private void DisplayReservationCodeTAndOTPOptions()
        {
            log.LogMethodEntry();
            int heightAdjustValueRCode = 100;
            int heightAdjustValueForOTP = 150;
            waiverSetSelectionOption = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "OPTIONS_FOR_WAIVER_SET_SELECTION");
            if (string.IsNullOrEmpty(waiverSetSelectionOption))
            {
                waiverSetSelectionOption = "BOTH";
                log.Info("OPTIONS_FOR_WAIVER_SET_SELECTION is not set. Doing with option as Both");
                KioskStatic.logToFile("OPTIONS_FOR_WAIVER_SET_SELECTION is not set. Doing with option as Both");
            }
            log.Debug("waiverSetSelectionOption: " + waiverSetSelectionOption);
            KioskStatic.logToFile("waiverSetSelectionOption: " + waiverSetSelectionOption);
            if (waiverSetSelectionOption == "RESERVATIONCODE")
            {
                lblTrxOTP.Visible = false;
                pnlTrxOTP.Visible = false;
                //btnTOTPShowKeyPad.Visible = false;
                lblReservationCodeOR.Visible = false;
                lblReservationCodeORBarOne.Visible = false;
                lblReservationCodeORBarTwo.Visible = false;
                lblReservationCode.Location = new Point(lblReservationCode.Location.X, lblReservationCode.Location.Y + heightAdjustValueRCode);
                pnlReservationCode.Location = new Point(pnlReservationCode.Location.X, pnlReservationCode.Location.Y + heightAdjustValueRCode);
                btnShowKeyPad.Location = new Point(btnShowKeyPad.Location.X, btnShowKeyPad.Location.Y + heightAdjustValueRCode);
            }
            else if (waiverSetSelectionOption == "TRANSACTIONOTP")
            {
                lblReservationCode.Visible = false;
                pnlReservationCode.Visible = false;
                btnShowKeyPad.Visible = false;
                lblReservationCodeOR.Visible = false;
                lblReservationCodeORBarOne.Visible = false;
                lblReservationCodeORBarTwo.Visible = false;
                lblTrxOTP.Location = new Point(lblTrxOTP.Location.X, lblTrxOTP.Location.Y - heightAdjustValueForOTP);
                pnlTrxOTP.Location = new Point(pnlTrxOTP.Location.X, pnlTrxOTP.Location.Y - heightAdjustValueForOTP);
                //btnTOTPShowKeyPad.Location = new Point(btnTOTPShowKeyPad.Location.X, btnTOTPShowKeyPad.Location.Y - heightAdjustValueForOTP);
            }
            log.LogMethodExit();

        }
        private void SetTextBoxFontColors()
        {
            if (KioskStatic.CurrentTheme == null ||
               (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                txtReservationCode.ForeColor = Color.Black;
                txtTrxOTP.ForeColor = Color.Black;
                foreach (Control c in panel1.Controls)
                {
                    c.ForeColor = Color.Black;
                }
                lblWaiver.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                lblSelection.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            else
            {
                txtReservationCode.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                txtTrxOTP.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                foreach (Control c in panel1.Controls)
                {
                    c.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
                }
            }
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmSelectWaiverOptions");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverBtnHomeTextForeColor;
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLabel1TextForeColor;
                this.lblCustomer.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblCustomerTextForeColor;
                this.lblReservationCode.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblReservationCodeTextForeColor;
                this.txtReservationCode.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverTxtReservationCodeTextForeColor;
                this.lblReservationCodeOR.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblReservationCodeORTextForeColor;
                this.lblTrxOTP.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblTrxOTPTextForeColor;
                this.txtTrxOTP.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverTxtTrxOTPTextForeColor;
                this.label4.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLabel4TextForeColor;
                this.label7.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLabel7TextForeColor;
                this.lblWaiver.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblWaiverTextForeColor;
                this.lblSelection.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverLblSelectionTextForeColor;
                this.dgvWaiverSet.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverDgvWaiverSetTextForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverBtnProceedTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmSelectWaiverTxtMessageTextForeColor;
                this.bigVerticalScrollWaiverSet.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.SelectWaiverOptionsBackgroundImage);
                btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                panel1.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSelectWaiverOptions: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void EnableButtons()
        {
            log.LogMethodEntry();
            try
            {
                btnPrev.Enabled = true;
                btnProceed.Enabled = true;
                btnShowKeyPad.Enabled = true;
                txtReservationCode.Enabled = true;
                txtTrxOTP.Enabled = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void DisableButtons()
        {
            log.LogMethodEntry();
            try
            {
                txtReservationCode.Enabled = false;
                txtTrxOTP.Enabled = false;
                btnPrev.Enabled = false;
                btnProceed.Enabled = false;
                btnShowKeyPad.Enabled = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void HideKeyboardObject()
        {
            log.LogMethodEntry();
            try
            {
                bool isWindowsKeyboardEnabled = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_WINDOWS_KEYBOARD", false);
                if (isWindowsKeyboardEnabled)
                {
                    virtualKeyboardController.HideKeyboard();
                }
                else
                {
                    customKeyboardController.HideKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in HideKeyboardObject() in  Select Waiver Options screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFocus()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(waiverSetSelectionOption) == false)
            {
                if (waiverSetSelectionOption == "TRANSACTIONOTP")
                {
                    txtTrxOTP.Focus(); 
                }
                else
                {
                    txtReservationCode.Focus(); 
                }
            }
            log.LogMethodExit();
        }
    }
}

