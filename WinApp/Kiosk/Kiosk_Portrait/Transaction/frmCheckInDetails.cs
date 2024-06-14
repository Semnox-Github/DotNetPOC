/********************************************************************************************
* Project Name - Parafait_Kiosk -frmCheckInDetails.cs
* Description  - frmCheckInDetails.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 *2.130.1    20-Sep-2021      Sathyavathi        Created
 *2.140.0    20-Sep-2021      Sathyavathi        Created
 *2.150.0.0  21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Transaction;
using System.Globalization;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Device.PaymentGateway;

namespace Parafait_Kiosk
{
    public partial class frmCheckInDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities = KioskStatic.Utilities;
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        private int guestCount;
        private CustomerDTO parentCustomerDTO;
        Card parentCard;
        DataRow selectedProductRow;
        private Semnox.Parafait.Transaction.Transaction currentTrx;
        private List<CustomCustomerDTO> customCustomerDTOList = new List<CustomCustomerDTO>();
        private CheckInDTO checkInDTO;
        SortableBindingList<CustomCheckInDetailDTO> customCheckInDetailDTOList = new SortableBindingList<CustomCheckInDetailDTO>();
        string message = "";
        Font savTimeOutFont;
        Font TimeOutFont;
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        private string checkIn = "C";
        private string selectedEntitlementType = "CheckInCheckOut";

        public Semnox.Parafait.Transaction.Transaction CheckinTransaction
        {
            get { return currentTrx; }
        }

        public frmCheckInDetails(DataRow productRow, string parentCardNumber, CustomerDTO customerDTO, int cardCount = 1)
        {
            log.LogMethodEntry(productRow, parentCardNumber, customerDTO, cardCount);
            InitializeComponent();
            utilities.setLanguage(this);
            float dgvHeaderFontSize = this.dgvSelectRelations.ColumnHeadersDefaultCellStyle.Font.Size;
            parentCustomerDTO = customerDTO;
            guestCount = cardCount;
            selectedProductRow = productRow;
            if (!string.IsNullOrEmpty(parentCardNumber))
            {
                txtCardNumber.Text = parentCardNumber;
                txtCardNumber.Enabled = false;

                parentCard = new Card(parentCardNumber, "Kiosk", utilities);
            }
            else
            {
                lblCardNumber.Visible = false;
                txtCardNumber.Visible = false;
            }
            txtCustomerName.Text = parentCustomerDTO.FirstName;
            txtCustomerName.Enabled = false;

            savTimeOutFont = lblTimeRemaining.Font;
            TimeOutFont = lblTimeRemaining.Font = new System.Drawing.Font(lblTimeRemaining.Font.FontFamily, 50, FontStyle.Bold);
            SetKioskTimerTickValue(60);
            ResetKioskTimer();
            lblTimeRemaining.Text = GetKioskTimerTickValue().ToString("#0");
            KioskStatic.setDefaultFont(this);
            dgvSelectRelations.Visible = true;
            lblSelectRelations.Visible = true;
            lblNoRelations.Visible = false;
            lblGuestsCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4136) + customCheckInDetailDTOList.Count.ToString() + " of " + guestCount.ToString();
            lblGreeting.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4134, guestCount);

            lblNoRelations.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "No Relationship linked to the member card.")
                                                + "\n" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Link Relationships to speed up the check-in process in the future.")
                                                + "\n" + MessageContainerList.GetMessage(utilities.ExecutionContext, "Link Relationship via Register option in the Home Screen or go to Cashier Counter.");
            lblSelectRelations.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Your Family");
            lblGuestEntry.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4119);//Guest Details Form
            dgvGuestEntry_ClearData();

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            KioskStatic.formatMessageLine(textBoxMessageLine, 26, Properties.Resources.bottom_bar);
            textBoxMessageLine.Text = "";

            dgvSelectRelations.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(dgvSelectRelations_DefaultValuesNeeded);
            btnProceed.Enabled = false;
            try
            {
                lblTimeRemaining.BackgroundImage = ThemeManager.CurrentThemeImages.TimerBoxSmall;
                this.BackgroundImage = ThemeManager.CurrentThemeImages.HomeScreenBackgroundImage;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                panelRelations.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
                this.btnPrev.BackgroundImage = btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                panelGuestEntry.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;

                dgvSelectRelations.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvGuestEntry.Font = new System.Drawing.Font("Gotham Rounded Bold", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvSelectRelations.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvGuestEntry.DefaultCellStyle.Font = new System.Drawing.Font("Gotham Rounded Bold", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dgvSelectRelations.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvSelectRelations.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvHeaderFontSize);
                dgvGuestEntry.ColumnHeadersDefaultCellStyle.Font = new Font(this.dgvSelectRelations.ColumnHeadersDefaultCellStyle.Font.FontFamily, dgvHeaderFontSize);
                dgvGuestEntry.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while frmCheckInDetails constructor: " + ex.Message);
            }
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            log.LogMethodExit();
        }
        private void frmCheckInDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("Loading frmCheckInDetails");
            try
            {
                ResetKioskTimer();
                lblSiteName.Text = KioskStatic.SiteHeading;
                if (checkInDTO == null)
                {
                    checkInDTO = new CheckInDTO(-1, parentCustomerDTO.Id, null, string.Empty, null,
                              null, (parentCard == null ? -1 : parentCard.card_id), -1, -1, Convert.ToInt32(selectedProductRow["CheckInFacilityId"]),
                             -1, -1, parentCustomerDTO, true);
                }

                dgvGuestEntry_GetFieldsToDisplay();
                dgvGuestEntrySetColumnSize();

                deleteDataGridViewImageColumn.FillWeight = checkboxDataGridViewImageColumn.FillWeight;
                deleteDataGridViewImageColumn.MinimumWidth = checkboxDataGridViewImageColumn.MinimumWidth;
                deleteDataGridViewImageColumn.Width = checkboxDataGridViewImageColumn.Width;

                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle = gridViewDateOfBirthCellStyle();
                dgvSelectRelations_PopulateLinkedCustomerRelations();

                dgvGuestEntry.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(CustomCheckInDetailDTOBindingSource_DataError);// Attach an event handler for the DataError event.

                dgvSelectRelations.DataSource = customCustomerDTOBindingSource;  // control's DataSource.
                customCustomerDTOBindingSource.DataSource = customCustomerDTOList;  // Bind the BindingSource to the DataGridView
                for (int i = 0; i < guestCount; i++)
                {
                    InsertNewCustomCheckInDetailDTO();
                }
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>(customCheckInDetailDTOList);  // Bind the BindingSource to the DataGridView
                dgvGuestEntry.DataSource = customCheckInDetailDTOBindingSource;  // control's DataSource.
                nameDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateFormat;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                displayMessageLine(ex.Message, ERROR);
                StopKioskTimer();
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
                ResetKioskTimer();
                StartKioskTimer();
                KioskStatic.logToFile("Error while frmCheckInDetails_Load(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("frmCheckInDetails btnPrev Click");
            ResetKioskTimer();
            ShowHideKeypad('H');
            this.DialogResult = DialogResult.No;
            Close();
            log.LogMethodExit();
        }
        private void btnProceed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("frmCheckInDetails btnProceed Click");
            ResetKioskTimer();
            ShowHideKeypad('H');
            bool canProceed = false;

            //CheckInDetails can not be saved without a Name
            for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
            {
                if (!string.IsNullOrEmpty(customCheckInDetailDTOList[i].Name))
                {
                    canProceed = true;
                }
                else
                {
                    dgvGuestEntry.Rows[i].DefaultCellStyle.BackColor = Color.OrangeRed;
                    string message = MessageUtils.getMessage(249, dgvGuestEntry.Columns["nameDataGridViewTextBoxColumn"].HeaderText); //&1 is mandatory. Please enter a value.
                    displayMessageLine(message, ERROR);
                }
            }

            if (canProceed == true)
            {
                CheckInBL checkInBL = new CheckInBL(utilities.ExecutionContext, checkInDTO);
                checkInDTO.CheckInDetailDTOList.Clear();  //CheckInDTO will be only have header checkin
                try
                {
                    if (currentTrx == null)
                    {
                        currentTrx = new Semnox.Parafait.Transaction.Transaction(KioskStatic.Utilities);
                        currentTrx.PaymentReference = "Kiosk Transaction";
                        KioskStatic.logToFile("New Trx object created");
                    }
                    if (parentCard != null)
                    {
                        currentTrx.PrimaryCard = parentCard;
                    }

                    Semnox.Parafait.Transaction.Transaction.TransactionLine checkInParentLine = new Semnox.Parafait.Transaction.Transaction.TransactionLine();
                    for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
                    {
                        //CheckInDTO only in first line. Other lines have only CheckInDetailDTO. Link all lines using parent line id (of first line)
                        if (i == 0)
                        {
                            if (0 == currentTrx.createTransactionLine(null, Convert.ToInt32(selectedProductRow["product_id"]), checkInDTO, customCheckInDetailDTOList[i].CheckInDetailDTO, selectedProductRow["price"] == null ? 0 : Convert.ToDouble(selectedProductRow["price"]), 1, ref message, null))
                            {
                                checkInParentLine = currentTrx.TrxLines[currentTrx.TrxLines.Count - 1];
                                currentTrx.TrxLines[currentTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                            }
                            else
                            {
                                displayMessageLine(MessageContainerList.GetMessage(utilities.ExecutionContext, "Error") + ": " + message, ERROR);
                                KioskStatic.logToFile("Error TrxLine4: " + message);
                                log.LogMethodExit(false);
                                this.DialogResult = DialogResult.Cancel;
                            }
                        }
                        else
                        {
                            currentTrx.createTransactionLine(null, Convert.ToInt32(selectedProductRow["product_id"]), null, customCheckInDetailDTOList[i].CheckInDetailDTO, selectedProductRow["price"] == null ? 0 : Convert.ToDouble(selectedProductRow["price"]), 1, ref message, null);
                            currentTrx.TrxLines[currentTrx.TrxLines.Count - 1].ParentLine = checkInParentLine;
                            currentTrx.TrxLines[currentTrx.TrxLines.Count - 1].ProductName = checkInParentLine.ProductName;
                        }
                    }

                    RichContentDTO richContentDTO = KioskStatic.CheckInRichContentDTO;
                    if (richContentDTO != null)
                    {
                        frmRegisterTnC frmRnC = new frmRegisterTnC();
                        frmRnC.LoadRichContent(richContentDTO, "C");
                        KioskStatic.logToFile("Terms and Conditions file for Playground Entry Present");
                        DialogResult dr = frmRnC.ShowDialog();
                        if (dr != System.Windows.Forms.DialogResult.Yes)
                        {
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                        else
                        {
                            callTransactionForm(guestCount);
                        }
                    }
                    else
                    {
                        callTransactionForm(guestCount);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    KioskStatic.logToFile("Error while processing frmCheckInDetails btnProceed_Click:" + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        void callTransactionForm(int GuestCount)
        {
            log.LogMethodEntry(GuestCount);
            StopKioskTimer();
            ResetKioskTimer();

            if (currentTrx != null && currentTrx.Net_Transaction_Amount > 0)
            {
                using (frmPaymentMode frpm = new frmPaymentMode(checkIn, selectedProductRow, parentCard, parentCustomerDTO, selectedEntitlementType, guestCount, currentTrx))
                {
                    DialogResult dr = frpm.ShowDialog();
                    if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                    {
                        DialogResult = dr;
                        this.Close();
                        log.LogMethodExit();
                        return;
                    }
                }
            }
            else
            {
                List<POSPaymentModeInclusionDTO> pOSPaymentModeInclusionDTOList = KioskStatic.pOSPaymentModeInclusionDTOList;
                if (pOSPaymentModeInclusionDTOList != null && pOSPaymentModeInclusionDTOList.Any()
                     && pOSPaymentModeInclusionDTOList.Exists(x => x.PaymentModeDTO.IsDebitCard))
                {
                    PaymentModeDTO paymentModeDTO = pOSPaymentModeInclusionDTOList.Where(p => p.PaymentModeDTO.IsDebitCard == true).FirstOrDefault().PaymentModeDTO;
                    if (paymentModeDTO != null)
                    {
                        TransactionPaymentsDTO debitTrxPaymentDTO = new TransactionPaymentsDTO();
                        debitTrxPaymentDTO.PaymentModeId = paymentModeDTO.PaymentModeId;
                        debitTrxPaymentDTO.paymentModeDTO = paymentModeDTO;
                        debitTrxPaymentDTO.Amount = 0;
                        debitTrxPaymentDTO.CardId = parentCard.card_id;
                        debitTrxPaymentDTO.CardEntitlementType = "C";
                        debitTrxPaymentDTO.PaymentUsedCreditPlus = 0;
                        debitTrxPaymentDTO.PaymentCardNumber = parentCard.CardNumber;
                        using (frmCardTransaction frms = new frmCardTransaction(checkIn, selectedProductRow, parentCard, parentCustomerDTO, paymentModeDTO, guestCount, selectedEntitlementType, null, null, currentTrx))
                        {
                            frms.trxPaymentDTO = debitTrxPaymentDTO;
                            DialogResult dr = frms.ShowDialog();
                            if (dr != System.Windows.Forms.DialogResult.No) // back button pressed
                            {
                                DialogResult = dr;
                                this.Close();
                                log.LogMethodExit();
                                return;
                            }
                        }
                    }
                }
            }
            StartKioskTimer();
            log.LogMethodExit();
        }

        AlphaNumericKeyPad keypad;
        public TextBox CurrentActiveTextBox;
        private void btnShowKeypad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            ShowHideKeypad('T'); // toggle
            log.LogMethodExit();
        }
        void ShowHideKeypad(char mode = 'N') // T for toggle, S for show and H for hide, 'N' for nothing
        {
            log.LogMethodEntry(mode);
            ResetKioskTimer();
            if (CurrentActiveTextBox == null)
            {
                CurrentActiveTextBox = txtCustomerName;
            }
            if (keypad == null || keypad.IsDisposed)
            {
                if (CurrentActiveTextBox == null)
                    CurrentActiveTextBox = new TextBox();
                keypad = new AlphaNumericKeyPad(this, CurrentActiveTextBox, KioskStatic.CurrentTheme.KeypadSizePercentage);
                keypad.Location = new Point((this.Width - keypad.Width) / 2, panelGuestEntry.Top - keypad.Height);
                keypad.resetTimer += new AlphaNumericKeyPad.ResetTimerDelegate(ResetKioskTimer);
            }
            if (mode == 'T')
            {
                if (keypad.Visible)
                    keypad.Hide();
                else
                {
                    keypad.currentTextBox = CurrentActiveTextBox;
                    keypad.Location = new Point((this.Width - keypad.Width) / 2, panelGuestEntry.Top - keypad.Height);
                    keypad.Show();
                }
            }
            else if (mode == 'S')
            {
                keypad.currentTextBox = CurrentActiveTextBox;
                keypad.Location = new Point((this.Width - keypad.Width) / 2, panelGuestEntry.Top - keypad.Height);
                keypad.Show();
            }
            else if (mode == 'H')
                keypad.Hide();

            log.LogMethodExit();
        }
        private void dgvSelectRelations_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            (dgvSelectRelations.CurrentRow.Cells["lblCheckbox"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
            log.LogMethodExit();
        }
        private void dgvSelectRelations_PopulateLinkedCustomerRelations()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                KioskStatic.logToFile("Getting Customer Linked Relations");
                List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>> searchCustomerRelationshipParams = new List<KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>>();
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.IS_ACTIVE, "1"));
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.CUSTOMER_ID, parentCustomerDTO.Id.ToString()));
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EFFECTIVE_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchCustomerRelationshipParams.Add(new KeyValuePair<CustomerRelationshipDTO.SearchByParameters, string>(CustomerRelationshipDTO.SearchByParameters.EXPIRY_DATE, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<CustomerRelationshipDTO> customerRelationshipDTOList = null;
                CustomerRelationshipListBL customerRelationshipListBL = new CustomerRelationshipListBL(utilities.ExecutionContext);
                customerRelationshipDTOList = customerRelationshipListBL.GetCustomerRelationshipDTOList(searchCustomerRelationshipParams);

                if ((customerRelationshipDTOList != null) && customerRelationshipDTOList.Any())
                {
                    lblSelectRelations.Visible = true;
                    dgvSelectRelations.Visible = true;
                    lblNoRelations.Visible = false;

                    foreach (CustomerRelationshipDTO customerRelationshipDTO in customerRelationshipDTOList)
                    {
                        CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, customerRelationshipDTO.RelatedCustomerId);
                        CustomCustomerDTO customCustomerDTO = new CustomCustomerDTO(customerRelationshipDTO, customerBL.CustomerDTO);
                        customCustomerDTOList.Add(customCustomerDTO);
                    }
                }
                else
                {
                    lblSelectRelations.Visible = false;
                    dgvSelectRelations.Visible = false;
                    lblNoRelations.Visible = true;
                    verticalScrollBarView1.Visible = false;
                    ShowHideKeypad('S');

                    // Attach an event handler for the DefaultValuesNeeded event.
                    dgvGuestEntry.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(dgvGuestEntry_DefaultValuesNeeded);
                }
                ShowHideKeypad('H');
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while getting populating Linked Relations for dgvSelectRelations: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvSelectRelations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();

            try
            {
                for (int i = 0; i < dgvSelectRelations.RowCount; i++)
                {
                    dgvSelectRelations.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    dgvSelectRelations.Rows[i].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                }

                Bitmap x = (System.Drawing.Bitmap)(dgvSelectRelations.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value;
                Bitmap y = (System.Drawing.Bitmap)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;
                int index = dgvSelectRelations.CurrentRow.Index;
                if (CompareImages(x, y))
                {
                    (dgvSelectRelations.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;

                    for (int i = 0; i < dgvGuestEntry.RowCount; i++)
                    {
                        if (Convert.ToInt32(dgvGuestEntry.Rows[i].Cells["customerIdDataGridViewTextBoxColumn"].Value) == Convert.ToInt32(dgvSelectRelations.CurrentRow.Cells["relatedCustomerIdDataGridViewTextBoxColumn"].Value))
                        {
                            customCheckInDetailDTOList.RemoveAt(i);
                            InsertNewCustomCheckInDetailDTO();
                            dgvGuestEntry_UpdateBindDataSource();
                            dgvGuestEntry_UpdateCheckInGuestsCount();
                            textBoxMessageLine.Text = "Removed Child Detail";
                            break;
                        }
                    }
                }
                else
                {
                    (dgvSelectRelations.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Ticked;

                    CustomCheckInDetailDTO editCustomCheckInDetailDTO = new CustomCheckInDetailDTO();

                    if (!string.IsNullOrEmpty(customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].RelatedCustomerName))
                    {
                        editCustomCheckInDetailDTO.Name = customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].RelatedCustomerName;
                    }

                    string DOB = customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].DOB;
                    if (!string.IsNullOrEmpty(DOB) && DOB != KioskStatic.DateFormat)
                    {
                        editCustomCheckInDetailDTO.DateOfBirth = customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].DOB;

                        if (customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].Age > 0)
                        {
                            editCustomCheckInDetailDTO.Age = customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].Age;
                        }
                        else
                        {
                            editCustomCheckInDetailDTO.Age = 0;
                        }
                    }

                    if (customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].RelatedCustomerId != -1)
                    {
                        editCustomCheckInDetailDTO.CustomerId = customCustomerDTOList[dgvSelectRelations.CurrentRow.Index].RelatedCustomerId;
                    }
                    int editIndex = -1;
                    var dto = customCheckInDetailDTOList.Where(k => k.CustomerId == -1 && (k.Name == MessageContainerList.GetMessage(utilities.ExecutionContext, 4135) || string.IsNullOrWhiteSpace(k.Name))).FirstOrDefault();
                    if (dto != null)
                    {
                        editIndex = customCheckInDetailDTOList.IndexOf(customCheckInDetailDTOList.Where(k => k.CustomerId == -1 && (k.Name == MessageContainerList.GetMessage(utilities.ExecutionContext, 4135) || string.IsNullOrWhiteSpace(k.Name))).FirstOrDefault());
                        if (editIndex > -1)
                            customCheckInDetailDTOList.RemoveAt(editIndex);
                    }
                    if ((customCheckInDetailDTOList.Count < guestCount) && (editIndex > -1))
                    {
                        customCheckInDetailDTOList.Insert(editIndex, editCustomCheckInDetailDTO);
                        dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle = gridViewDateOfBirthCellStyle();
                        dgvGuestEntry_UpdateBindDataSource();
                        dgvGuestEntry_UpdateCheckInGuestsCount();
                        textBoxMessageLine.Text = "Added Child Detail";
                    }
                    else
                    {
                        (dgvSelectRelations.CurrentRow.Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                        textBoxMessageLine.Text = "You have already entered all the child details";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while processing checkboxDataGridViewImageColumn of dgvSelectRelations: " + ex.Message);
            }
            //}
            log.LogMethodExit();
        }
        private void InsertNewCustomCheckInDetailDTO()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                customCheckInDetailDTO.DateOfBirth = KioskStatic.DateFormat;
                customCheckInDetailDTOList.Add(customCheckInDetailDTO);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while InsertNewCustomCheckInDetailDTO(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvSelectRelation_verticalScrollBar_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void dgvGuestEntry_verticalScrollBar_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void CustomCheckInDetailDTOBindingSource_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            e.Cancel = true;
            log.LogMethodExit(null);
        }
        private void dgvGuestEntry_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void dgvGuestEntry_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (e.Control is TextBox)
            {
                CurrentActiveTextBox = e.Control as TextBox;
                ShowHideKeypad('S');
            }
            log.LogMethodExit();
        }
        private void dgvGuestEntry_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            dgvGuestEntry.EndEdit();
            try
            {
                if ((dgvGuestEntry.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value) != null)
                {
                    if (string.IsNullOrWhiteSpace(dgvGuestEntry.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                    {
                        customCheckInDetailDTOList[e.RowIndex].Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                    }
                    dgvGuestEntry.CurrentCell.Style.BackColor = Color.White;
                    dgvGuestEntry_UpdateCheckInGuestsCount();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvGuestEntry_RowLeave(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvGuestEntry_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var addImage = Properties.Resources.Add;
            var deleteImage = Properties.Resources.Delete;

            if (dgvGuestEntry.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
            {
                if ((dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null)
                     && (dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() != MessageContainerList.GetMessage(utilities.ExecutionContext, 4135))
                     && !string.IsNullOrWhiteSpace(dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString()))
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var w = deleteImage.Width;
                    var h = deleteImage.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(deleteImage, new Rectangle(x, y, w, h));
                    e.Handled = true;
                }
                else
                {
                    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                    var w = addImage.Width;
                    var h = addImage.Height;
                    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                    e.Graphics.DrawImage(addImage, new Rectangle(x, y, w, h));
                    e.Handled = true;
                }
            }
        }

        private void dgvGuestEntry_UpdateCheckInGuestsCount()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();

                int selectedGuestCount = 0;
                foreach (CustomCheckInDetailDTO customCheckInDetailDTO in customCheckInDetailDTOList)
                {
                    if (!string.IsNullOrWhiteSpace(customCheckInDetailDTO.Name)
                        && customCheckInDetailDTO.Name != MessageContainerList.GetMessage(utilities.ExecutionContext, 4135))
                    {
                        selectedGuestCount++;

                        for (int i = 0; i < customCheckInDetailDTOList.Count; i++)
                        {
                            dgvGuestEntry.Rows[i].Selected = false;
                        }
                    }
                }
                lblGuestsCount.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 4136) + selectedGuestCount.ToString() + " of " + guestCount.ToString();
                if (selectedGuestCount > guestCount)
                {
                    btnProceed.Enabled = false;
                    displayMessageLine(MessageUtils.getMessage(2921), WARNING); //Exceeded maximum value
                }
                else if (selectedGuestCount < guestCount)
                {
                    btnProceed.Enabled = false;
                    textBoxMessageLine.Text = "";
                }
                else
                {
                    btnProceed.Enabled = true;
                    this.btnProceed.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                    textBoxMessageLine.Text = "";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvGuestEntry_UpdateCheckInGuestsCount() : " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void HighlightRow(int rowIndex, bool selected)
        {
            log.LogMethodEntry();

            if (rowIndex < 0)
                return;

            try
            {
                for (int i = 0; i < dgvSelectRelations.RowCount; i++)
                {
                    dgvSelectRelations.Rows[i].Selected = false;
                    dgvSelectRelations.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    dgvSelectRelations.Rows[i].DefaultCellStyle.ForeColor = SystemColors.ControlText;
                }

                if (selected)
                {
                    dgvSelectRelations.Rows[rowIndex].DefaultCellStyle.BackColor = SystemColors.Highlight;
                    dgvSelectRelations.Rows[rowIndex].DefaultCellStyle.ForeColor = SystemColors.HighlightText;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while HighlightRow() : " + ex.Message);
            }

            log.LogMethodExit();
        }
        private void dgvGuestEntry_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            dgvGuestEntry.EndEdit();
            try
            {
                if (e.ColumnIndex == dgvGuestEntry.Columns["dateOfBirthDataGridViewTextBoxColumn"].Index)
                {
                    if (dgvGuestEntry.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value == null)
                    {
                        dgvGuestEntry.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].Value = string.Empty;
                    }
                }

                if (e.ColumnIndex == dgvGuestEntry.Columns["nameDataGridViewTextBoxColumn"].Index)
                {
                    if (dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value != null
                        && dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value.ToString() == MessageContainerList.GetMessage(utilities.ExecutionContext, 4135))
                    {
                        dgvGuestEntry.Rows[e.RowIndex].Cells["nameDataGridViewTextBoxColumn"].Value = string.Empty;
                    }
                }

                int selectedCustomerId = Convert.ToInt32(dgvGuestEntry.CurrentRow.Cells["customerIdDataGridViewTextBoxColumn"].Value);
                if (selectedCustomerId != -1)
                {
                    dgvSelectRelations.CurrentCell.ReadOnly = true;
                    ShowHideKeypad('H');
                }
                if ((dgvGuestEntry.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn")
                    && (!string.IsNullOrEmpty(dgvGuestEntry.CurrentRow.Cells["nameDataGridViewTextBoxColumn"].Value.ToString())))
                {
                    ShowHideKeypad('H');

                    for (int i = 0; i < customCustomerDTOList.Count; i++)
                    {
                        if (Convert.ToInt32(dgvSelectRelations.Rows[i].Cells["relatedCustomerIdDataGridViewTextBoxColumn"].Value) == selectedCustomerId)
                        {
                            (dgvSelectRelations.Rows[i].Cells["checkboxDataGridViewImageColumn"] as DataGridViewImageCell).Value = (System.Drawing.Image)Parafait_Kiosk.Properties.Resources.Check_Box_Empty;
                            HighlightRow(dgvSelectRelations.Rows[i].Index, true);
                            break;
                        }
                    }
                    CustomCheckInDetailDTO customCheckInDetailDTO = new CustomCheckInDetailDTO();
                    customCheckInDetailDTO.Name = MessageContainerList.GetMessage(utilities.ExecutionContext, 4135);
                    customCheckInDetailDTO.DateOfBirth = KioskStatic.DateFormat;
                    customCheckInDetailDTOList.RemoveAt(dgvGuestEntry.CurrentRow.Index);
                    customCheckInDetailDTOList.Add(customCheckInDetailDTO);
                    dgvGuestEntry_UpdateBindDataSource();
                }
                if (!(dgvGuestEntry.Columns[e.ColumnIndex].Name == "deleteDataGridViewImageColumn"))
                {
                    ShowHideKeypad('S');
                }
                dgvGuestEntry_UpdateCheckInGuestsCount();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while processing deleteDataGridViewImageColumn of dgvGuestEntry: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvGuestEntry_UpdateBindDataSource()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateFormat;
                if (customCheckInDetailDTOList.Count > 0)
                {
                    dgvGuestEntry.Rows[customCheckInDetailDTOList.Count - 1].Selected = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvGuestEntry_UpdateBindDataSource() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvGuestEntry_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetKioskTimer();
                customCheckInDetailDTOList = (SortableBindingList<CustomCheckInDetailDTO>)customCheckInDetailDTOBindingSource.DataSource;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while dgvGuestEntry_DataBindingComplete() : " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvGuestEntry_ClearData()
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                customCheckInDetailDTOBindingSource.DataSource = new SortableBindingList<CustomCheckInDetailDTO>();
                dgvGuestEntry.DataSource = customCheckInDetailDTOBindingSource;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error while dgvGuestEntry_ClearData() : " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void dgvGuestEntry_GetFieldsToDisplay()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Fetching Lookup values for dgvGuestEntry column display fields");
            try
            {
                LookupsContainerDTO CheckinFieldsLookupValuesList = LookupsContainerList.GetLookupsContainerDTO(-1, "KIOSK_CHECKIN_DISPLAY_FIELDS_CONFIG");
                if (CheckinFieldsLookupValuesList != null)
                {
                    nameDataGridViewTextBoxColumn.Visible = false;
                    vehicleNumberDataGridViewTextBoxColumn.Visible = false;
                    vehicleModelDataGridViewTextBoxColumn.Visible = false;
                    vehicleColorDataGridViewTextBoxColumn.Visible = false;
                    dateOfBirthDataGridViewTextBoxColumn.Visible = false;
                    ageDataGridViewTextBoxColumn.Visible = false;
                    specialNeedsDataGridViewTextBoxColumn.Visible = false;
                    allergiesDataGridViewTextBoxColumn.Visible = false;

                    for (int i = 0; i < CheckinFieldsLookupValuesList.LookupValuesContainerDTOList.Count; i++)
                    {
                        switch (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].LookupValue)
                        {
                            case "Name":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    nameDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "VehicleNumber":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    vehicleNumberDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "VehicleModel":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    vehicleModelDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "VehicleColour":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    vehicleColorDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "DateOfBirth":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    dateOfBirthDataGridViewTextBoxColumn.Visible = true;
                                    dateOfBirthDataGridViewTextBoxColumn.DefaultCellStyle.NullValue = KioskStatic.DateFormat;
                                }
                                break;
                            case "Age":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    ageDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "SpecialNeeds":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    specialNeedsDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            case "Allergies":
                                if (CheckinFieldsLookupValuesList.LookupValuesContainerDTOList[i].Description == "Y")
                                {
                                    allergiesDataGridViewTextBoxColumn.Visible = true;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Fetching Lookup values for dgvGuestEntry column display fields: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvGuestEntrySetColumnSize()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            KioskStatic.logToFile("Setting column size: dgvGuestEntrySetColumnSize()");
            try
            {
                int visibleColumnsCount = 0;
                for (int i = 0; i < dgvGuestEntry.ColumnCount; i++)
                {
                    if (dgvGuestEntry.Columns[i].Visible == true)
                    {
                        visibleColumnsCount++;
                    }
                }
                dgvGuestEntry.Columns[visibleColumnsCount - 1].AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Fetching Lookup values for dgvGuestEntry column display fields: " + ex.Message);
            }
            log.LogMethodExit();
        }

        public DataGridViewCellStyle gridViewDateOfBirthCellStyle()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            DataGridViewCellStyle style = new DataGridViewCellStyle();

            style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            style.Format = KioskStatic.DateFormat;
            log.LogMethodExit(style);
            return style;
        }
        /// <summary>
        /// Comparse the two images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        private static bool CompareImages(Bitmap image1, Bitmap image2)
        {
            log.LogMethodEntry(image1, image2);

            if (image1.Width == image2.Width && image1.Height == image2.Height)
            {
                for (int i = 0; i < image1.Width; i++)
                {
                    for (int j = 0; j < image1.Height; j++)
                    {
                        if (image1.GetPixel(i, j) != image2.GetPixel(i, j))
                        {
                            log.LogMethodExit();
                            return false;
                        }
                    }
                }
                log.LogMethodExit();
                return true;
            }
            else
            {
                log.LogMethodExit();
                return false;
            }
        }

        private void dgvGuestEntry_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex == dgvGuestEntry.Columns["dateOfBirthDataGridViewTextBoxColumn"].Index)
                {
                    if (!string.IsNullOrWhiteSpace(dgvGuestEntry.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].EditedFormattedValue.ToString())
                        && dgvGuestEntry.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].EditedFormattedValue.ToString() != KioskStatic.DateFormat)
                    {
                        ValidateDateOfBirth(e.RowIndex, dgvGuestEntry.Rows[e.RowIndex].Cells["dateOfBirthDataGridViewTextBoxColumn"].EditedFormattedValue.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("dgvGuestEntry_CellLeave: Error while handling wrong Date Of Birth : " + ex.Message);
            }
            try
            {
                if (keypad != null)
                {
                    keypad.FirstKeyPressed = false;
                    keypad.UpperCase();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("dgvGuestEntry_CellLeave: Error while doing Keypad InitCap : " + ex.Message);
            }
            log.LogMethodExit();
        }

        void ValidateDateOfBirth(int index, string value)
        {
            log.LogMethodEntry();
            string dateformat = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "DATE_FORMAT");
            if (!string.IsNullOrEmpty(value.Replace(" ", "").Replace("-", "")))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    DateTime DateOfBirthValue = DateTime.ParseExact(value, dateformat, provider);
                    if (DateOfBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[index].DateOfBirth))
                    {
                        customCheckInDetailDTOList[index].DateOfBirth = DateOfBirthValue.ToString(KioskStatic.DateFormat);
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        DateTime DateOfBirthValue = Convert.ToDateTime(value, provider);
                        if (DateOfBirthValue != Convert.ToDateTime(customCheckInDetailDTOList[index].DateOfBirth))
                        {
                            customCheckInDetailDTOList[index].DateOfBirth = DateOfBirthValue.ToString(KioskStatic.DateFormat);
                        }
                    }
                    catch (Exception exp)
                    {
                        string msg = dgvGuestEntry.Rows[index].Cells["dateOfBirthDataGridViewTextBoxColumn"].EditedFormattedValue.ToString();
                        msg += " is ";
                        msg += MessageContainerList.GetMessage(utilities.ExecutionContext, 449, KioskStatic.DateFormat, Convert.ToDateTime("23-Feb-1982").ToString(KioskStatic.DateFormat));
                        displayMessageLine(msg, ERROR);
                        ShowHideKeypad('H');
                        StopKioskTimer();
                        using (frmOKMsg frmOK = new frmOKMsg(msg, true))
                        {
                            frmOK.ShowDialog();
                        }
                        ResetKioskTimer();
                        StartKioskTimer();
                        log.Error(exp);
                        log.Error(MessageContainerList.GetMessage(utilities.ExecutionContext, 449, KioskStatic.DateFormat, Convert.ToDateTime("23-Feb-1982").ToString(KioskStatic.DateFormat)));
                    }
                    log.Error(ex);
                }
            }
            else
            {
                if (customCheckInDetailDTOList[index].DateOfBirth != null)
                {
                    customCheckInDetailDTOList[index].DateOfBirth = null;
                }
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining <= 60)
            {
                lblTimeRemaining.Font = TimeOutFont;
                lblTimeRemaining.Text = tickSecondsRemaining.ToString("#0");
            }
            else
            {
                lblTimeRemaining.Font = savTimeOutFont;
                lblTimeRemaining.Text = (tickSecondsRemaining / 60).ToString() + ":" + (tickSecondsRemaining % 60).ToString().PadLeft(2, '0');
            }

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                displayMessageLine(MessageUtils.getMessage(457), WARNING);
                Application.DoEvents();
                this.Close();
                Dispose();
            }
            log.LogMethodExit();
        }
        void displayMessageLine(string message, string msgType)
        {
            log.LogMethodEntry(message, msgType);
            ResetKioskTimer();
            textBoxMessageLine.Text = message;
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            ResetKioskTimer();

            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblGreeting.SuspendLayout();
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblGreetingTxtForeColor;
                lblNoRelations.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblNoRelationsTxtForeColor;
                lblGuestEntry.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblAddGuestsTxtForeColor;
                lblSelectRelations.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblSelectRelationsTextForeColor;
                dgvSelectRelations.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsDGVRelationsTextForeColor;
                txtCardNumber.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsTxtCardNumberTextForeColor;
                txtCustomerName.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsTxtFirstNameTextForeColor;
                dgvGuestEntry.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsDGVGuestEntryTextForeColor;//Footer text message
                this.textBoxMessageLine.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsTxtMessageTxtForeColor;//Footer text message
                this.lblSiteName.ForeColor = KioskStatic.CurrentTheme.SiteHeadingForeColor;
                this.btnProceed.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsBtnProceedTxtForeColor;
                lblGuestsCount.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblGuestsCountTxtForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsBackButtonTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.ChooseProductsBtnHomeTextForeColor;//Footer text message
                this.lblTimeRemaining.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblTimeRemainingTextForeColor;
                this.lblCardNumber.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblCardNumberTextForeColor;
                this.lblFirstname.ForeColor = KioskStatic.CurrentTheme.CheckInDetailsLblFirstNameForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            this.lblGreeting.ResumeLayout(true);
            log.LogMethodExit();
        }
        private void frmCheckIndetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                keypad.Close();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing Customer_FormClosed()", ex);
            }
            Cursor.Hide();

            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }
    }
}


























