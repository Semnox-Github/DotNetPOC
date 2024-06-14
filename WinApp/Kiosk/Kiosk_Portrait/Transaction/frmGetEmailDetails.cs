/********************************************************************************************
 * Project Name - Portrait Kiosk
 * Description  - user interface -frmGetEmailDetails
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.150.1      27-Dec-2022     Vignesh Bhat        Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Customer;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmGetEmailDetails : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<Panel> panelCustomerDetailsList = new List<Panel>();
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }
        private ExecutionContext executionContext;
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;
        private List<string> guestIdList = new List<string>();
        private bool hasEmailId = false;
        private bool showEmailEntryForm = true;
        private List<ContactDTO> contactDTOList;
        public frmGetEmailDetails(ExecutionContext executionContext, KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry(kioskTransaction);
            this.executionContext = executionContext;
            this.kioskTransaction = kioskTransaction;
            guestIdList = kioskTransaction.GetTransactionCustomerIdentifierList();
            kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.NONE;
            InitializeComponent();
            DisplaybtnCancel(true);
            //DisplaybtnHome(true);
            KioskStatic.setDefaultFont(this);
            txtUserEntryEmail.CharacterCasing = CharacterCasing.Lower;
            btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
            btnCancel.BackgroundImage = btnOk.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            //Please select or enter an email address to proceed
            txtMessage.Text = MessageContainerList.GetMessage(executionContext, 4860);
            this.panelCustomerDetailsList.Add(panelUserEntry);
            showEmailEntryForm = ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_EMAIL_ENTRY_FORM", true);
            string lblSkipmsg = MessageContainerList.GetMessage(executionContext, 5197); // Press ''Skip'' to proceed without providing your email 
            this.lblSkipDetails.Text = lblSkipmsg;
            string lblNotemsg = MessageContainerList.GetMessage(executionContext, 5198); // Note: if you skip, the receipt will not be emailed
            this.lblNote.Text = lblNotemsg;
            if (guestIdList != null && guestIdList.Any())
            {
                txtUserEntryEmail.Text = guestIdList[0];
                pbxUserEntry.Tag = "1";
                pbxUserEntry.Image = Properties.Resources.tick_box_checked;
            }
            LoadCustomerEmailDetails(kioskTransaction);
            lblGreeting1.Visible = KioskStatic.CurrentTheme.ShowHeaderMessage;
            SetCustomizedFontColors();
            InitializeKeyboard();
            SetKioskTimerTickValue(20);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmGetEmailDetails_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (contactDTOList != null && contactDTOList.Count == 1)
            {
                List<Panel> panelCustEmailDetails = flpCustomerDetails.Controls.OfType<Panel>().ToList();
                List<PictureBox> pbox = panelCustEmailDetails[0].Controls.OfType<PictureBox>().ToList();
                pbox[0].Tag = "1";
                pbox[0].Image = Properties.Resources.tick_box_checked;
            }
            if (showEmailEntryForm == false && contactDTOList != null && contactDTOList.Count == 1)
            {
                btnOk.PerformClick();
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }

        private void pbxCustCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            PictureBox pictureBox = sender as PictureBox;
            SetCheckBoxFlag(pictureBox);
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void SetCheckBoxFlag(PictureBox pictureBox)
        {
            log.LogMethodEntry();
            Panel parentPanel = (Panel)pictureBox.Parent;
            if (pictureBox.Tag == null || pictureBox.Tag.ToString() == "0")
            {
                pictureBox.Tag = "1";
                pictureBox.Image = Properties.Resources.tick_box_checked;
            }
            else
            {
                pictureBox.Tag = "0";
                pictureBox.Image = Properties.Resources.tick_box_unchecked;

            }
            if (pictureBox.Tag.ToString() == "1")
            {
                if (parentPanel.Name == "panelUserEntry")
                {
                    txtUserEntryEmail.Enabled = true;
                }
                if (panelCustomerDetailsList != null && panelCustomerDetailsList.Any())
                {
                    for (int i = 0; i < panelCustomerDetailsList.Count; i++)
                    {
                        Panel pnl = panelCustomerDetailsList[i];
                        if (pnl.Tag != parentPanel.Tag)
                        {
                            for (int j = 0; j < pnl.Controls.Count; j++)
                            {
                                if (pnl.Controls[j] is PictureBox)
                                {
                                    PictureBox pbx = (PictureBox)pnl.Controls[j];
                                    pbx.Image = Properties.Resources.tick_box_unchecked;
                                    pbx.Tag = "0";
                                    if (pnl.Tag != "-1")
                                    {
                                        break;
                                    }
                                }
                                if (pnl.Controls[j] is Panel)
                                {
                                    Control innerPnl = pnl.Controls[j] as Panel;
                                    for (int k = 0; k < innerPnl.Controls.Count; k++)
                                    {
                                        if (innerPnl.Controls[k].Name == "txtUserEntryEmail")
                                        {
                                            txtUserEntryEmail.Enabled = false;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
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
                Application.DoEvents();
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private bool HasSelected()
        {
            log.LogMethodEntry();
            bool hasSelected = false;
            if (panelCustomerDetailsList != null && panelCustomerDetailsList.Any())
            {
                for (int i = 0; i < panelCustomerDetailsList.Count; i++)
                {
                    Panel pnl = panelCustomerDetailsList[i];
                    for (int j = 0; j < pnl.Controls.Count; j++)
                    {
                        if (pnl.Controls[j] is PictureBox)
                        {
                            PictureBox pbx = (PictureBox)pnl.Controls[j];
                            if (pbx.Tag != null && pbx.Tag.ToString() == "1")
                            {
                                hasSelected = true;
                                break;
                            }
                        }
                    }
                    if (hasSelected)
                    {
                        break;
                    }
                }
            }
            log.LogMethodExit(hasSelected);
            return hasSelected;
        }
        private string GetSelectedEmailId()
        {
            log.LogMethodEntry();
            string emailId = string.Empty;
            bool gotSelectedEntry = false;
            if (panelCustomerDetailsList != null && panelCustomerDetailsList.Any())
            {
                for (int i = 0; i < panelCustomerDetailsList.Count; i++)
                {
                    Panel pnl = panelCustomerDetailsList[i];
                    for (int j = 0; j < pnl.Controls.Count; j++)
                    {
                        if (pnl.Controls[j] is PictureBox)
                        {
                            PictureBox pbx = (PictureBox)pnl.Controls[j];
                            if (pbx.Tag != null && pbx.Tag.ToString() == "1")
                            {
                                for (int k = 0; k < pnl.Controls.Count; k++)
                                {
                                    if (pnl.Controls[k] is Panel)
                                    {
                                        Control innerCtrl = pnl.Controls[k];
                                        for (int l = 0; l < innerCtrl.Controls.Count; l++)
                                        {
                                            if (innerCtrl.Controls[l] is TextBox)
                                            {
                                                TextBox txtBox = (TextBox)innerCtrl.Controls[l];
                                                if (txtBox != null)
                                                {
                                                    emailId = txtBox.Text;
                                                    gotSelectedEntry = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (gotSelectedEntry)
                                    {
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                        if (gotSelectedEntry)
                        {
                            break;
                        }
                    }
                    if (gotSelectedEntry)
                    {
                        break;
                    }
                }
            }
            log.LogMethodExit(emailId);
            return emailId;
        }
        private void LoadCustomerEmailDetails(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry(kioskTransaction);
            hasEmailId = false;
            //txtMessage.Text = "";
            flpCustomerDetails.Controls.Clear();
            try
            {
                this.flpCustomerDetails.SuspendLayout();
                if (kioskTransaction != null)
                {
                    CustomerDTO customerDTO = kioskTransaction.GetTransactionCustomer();
                    if (customerDTO != null)
                    {
                        contactDTOList = customerDTO.ProfileDTO.ContactDTOList;
                        if (contactDTOList != null)
                        {
                            for (int i = 0; i < contactDTOList.Count; i++)
                            {
                                if (contactDTOList[i].Attribute1 != null && contactDTOList[i].ContactType == ContactType.EMAIL)
                                {
                                    hasEmailId = true;
                                    Panel panelCustEmailDetails = new Panel();
                                    panelCustEmailDetails = BuildEmailPanel(contactDTOList[i]);
                                    flpCustomerDetails.Controls.Add(panelCustEmailDetails);
                                }
                            }
                            if (hasEmailId == false)
                            {
                                label2.Visible = false;
                                label3.Visible = false;
                                //Please provide an email address
                                string message = MessageContainerList.GetMessage(executionContext, 1929);
                                txtMessage.Text = message;
                                bigVerticalScrollCustomerDetails.Visible = false;
                            }
                        }
                        else
                        {
                            CustomerEmailNotFound();
                        }
                    }
                    else
                    {
                        CustomerEmailNotFound();
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in LoadCustomerEmailDetails() of frmGetEmailDetails screen: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                this.flpCustomerDetails.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void CustomerEmailNotFound()
        {
            log.LogMethodEntry();
            SetCheckBoxFlag(pbxUserEntry);
            txtUserEntryEmail.Focus();
            label2.Visible = false;
            label3.Visible = false;
            if (guestIdList == null || (guestIdList.Any() == false))
            {
                //Please provide an email address
                string message = MessageContainerList.GetMessage(executionContext, 1929);
                txtMessage.Text = message;
            }
            bigVerticalScrollCustomerDetails.Visible = false;
            log.LogMethodExit();
        }
        private void textBox_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                if(hasEmailId)
                {
                    //Please select or enter an email address to proceed
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 4860);
                }
                else
                {
                    //Please provide email address
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, 1929);
                }
                TextBox text = sender as TextBox;
                Panel pnl = (Panel)text.Parent;
                Panel mainpanel = (Panel)pnl.Parent;
                PictureBox pbxCheckBox = null;
                for (int j = 0; j < mainpanel.Controls.Count; j++)
                {
                    if (mainpanel.Controls[j] is PictureBox)
                    {
                        pbxCheckBox = (PictureBox)mainpanel.Controls[j];
                    }
                }
                bool hasSelected = HasSelected();
                if (pbxCheckBox != null && hasSelected == false)
                {
                    pbxCheckBox.Tag = "1";
                    pbxCheckBox.Image = Properties.Resources.tick_box_checked;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in textBox_Enter(): " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void KeyPressEvent(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                TextBox currentTextxBox = sender as TextBox;
                if (currentTextxBox != null)
                {
                    if (currentTextxBox.Name == "textBoxEmail")
                    {
                        SetTextToLowerCase(currentTextxBox);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private static void SetTextToLowerCase(TextBox currentTextxBox)
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(currentTextxBox.Text) == false)
            {
                String txtValue = currentTextxBox.Text.ToLower();
                currentTextxBox.Text = txtValue;
                currentTextxBox.Focus();
                currentTextxBox.SelectionStart = txtValue.Length;
                currentTextxBox.SelectionLength = 0;
            }
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
                KioskStatic.logToFile("Error Initializing keyboard in  Get Email Details screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(panelUserEntry.Top);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  Get Eamil Details screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                customKeyboardController = new VirtualKeyboardController(panelUserEntry.Top);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblGreeting1.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  Get Eamil Details screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void GetEmailDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing GetEmailDetails_FormClosed()", ex);
            }
            log.Info(this.Name + ": Form closed");
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
                KioskStatic.logToFile("Error Disposing keyboard in  Get Email Details screen: " + ex.Message);
            }
            log.LogMethodExit();
        }
        private Panel BuildEmailPanel(ContactDTO contactDTO)
        {
            log.LogMethodEntry("contactDTO");
            Panel panelCustomerDetails = new Panel();
            Panel panelCustEmail = new Panel();
            PictureBox pbxCheckBox = new PictureBox();
            panelCustomerDetails.Size = new System.Drawing.Size(690, 120);
            panelCustomerDetails.Location = new System.Drawing.Point(0, 0);
            panelCustEmail = BuildEmailTextBox(contactDTO);
            pbxCheckBox = BuildEmailCheckBox();
            panelCustomerDetails.Controls.Add(panelCustEmail);
            panelCustomerDetails.Controls.Add(pbxCheckBox);
            panelCustomerDetails.Tag = contactDTO.Id;
            panelCustomerDetailsList.Add(panelCustomerDetails);
            log.LogMethodExit(panelCustomerDetails);
            return panelCustomerDetails;
        }
        private Panel BuildEmailTextBox(ContactDTO contactDTO)
        {
            log.LogMethodExit(contactDTO);
            TextBox txtCustomerEmail = new TextBox();
            Panel panelCustEmail = new Panel();

            panelCustEmail.BackgroundImage = global::Parafait_Kiosk.Properties.Resources.text_entry_box;
            panelCustEmail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            panelCustEmail.Controls.Add(txtCustomerEmail);
            panelCustEmail.Location = new System.Drawing.Point(112, 18);
            panelCustEmail.Margin = new System.Windows.Forms.Padding(0);
            panelCustEmail.Name = "panelCustEmail";
            panelCustEmail.Size = new System.Drawing.Size(549, 80);
            panelCustEmail.TabIndex = 1062;

            txtCustomerEmail.BackColor = System.Drawing.Color.White;
            string fontFamName = (KioskStatic.CurrentTheme.DefaultFont != null ? KioskStatic.CurrentTheme.DefaultFont.FontFamily.Name : "Gotham Rounded Bold");
            txtCustomerEmail.Font = new System.Drawing.Font(fontFamName, 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            txtCustomerEmail.ForeColor = System.Drawing.Color.Black;
            txtCustomerEmail.Location = new System.Drawing.Point(35, 12);
            txtCustomerEmail.Name = "txtCustomerEmail";
            txtCustomerEmail.BorderStyle = BorderStyle.None;
            txtCustomerEmail.Size = new System.Drawing.Size(505, 47);
            txtCustomerEmail.MaxLength = 100;
            txtCustomerEmail.Multiline = true;
            txtCustomerEmail.TabIndex = 1046;
            txtCustomerEmail.ReadOnly = true;
            //txtCustomerEmail.Enabled = false;
            txtCustomerEmail.TextAlign = HorizontalAlignment.Left;
            txtCustomerEmail.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtCustomerEmailTextForeColor;
            txtCustomerEmail.Text = contactDTO.Attribute1;
            txtCustomerEmail.Tag = contactDTO;

            log.LogMethodExit(txtCustomerEmail);
            return panelCustEmail;
        }
        private PictureBox BuildEmailCheckBox()
        {
            log.LogMethodEntry();
            PictureBox pbxCustCheckBox = new PictureBox();
            pbxCustCheckBox.BackColor = System.Drawing.Color.Transparent;
            pbxCustCheckBox.Image = global::Parafait_Kiosk.Properties.Resources.tick_box_unchecked;
            pbxCustCheckBox.Location = new System.Drawing.Point(0, 3);
            pbxCustCheckBox.Name = "pbxCustCheckBox";
            pbxCustCheckBox.Size = new System.Drawing.Size(111, 111);
            pbxCustCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            pbxCustCheckBox.TabIndex = 1059;
            pbxCustCheckBox.TabStop = false;
            pbxCustCheckBox.Tag = "0";
            pbxCustCheckBox.Click += new System.EventHandler(pbxCustCheckBox_Click);
            log.LogMethodExit(pbxCustCheckBox);
            return pbxCustCheckBox;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StopKioskTimer();
            txtMessage.Text = "";
            try
            {
                if (HasSelected())
                {
                    string emailId = GetSelectedEmailId();
                    if (string.IsNullOrWhiteSpace(emailId))
                    {
                        //Please provide email address
                        string message = MessageContainerList.GetMessage(executionContext, 1929);
                        txtMessage.Text = message;
                    }
                    else
                    {
                        kioskTransaction.RececiptDeliveryMode = KioskTransaction.KioskReceiptDeliveryMode.EMAIL;
                        kioskTransaction.SetGuestEmailId(emailId);
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    //Please select only one option to proceed
                    string message = MessageContainerList.GetMessage(executionContext, 2599);
                    txtMessage.Text = message;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnOk_Click() of frmGetEmailDetails screen: " + ex.Message);
                txtMessage.Text = ex.Message;
            }
            finally
            {
                ResetKioskTimer();
                StartKioskTimer();
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmGetEmailDetails");
            try
            {
                this.lblGreeting1.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLblGreeting1TextForeColor;
                this.txtUserEntryEmail.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsTxtUserEntryEmailTextForeColor;
                this.label1.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel1TextForeColor;
                this.label2.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel2TextForeColor;
                this.label3.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLabel3TextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsBtnCancelTextForeColor;
                this.btnOk.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsBtnOkTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsFooterTxtMsgTextForeColor; //footer message 
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.EmailDetailsBackgroundImage);
                this.bigVerticalScrollCustomerDetails.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.lblSkipDetails.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLblSkipDetailsTextForeColor;
                this.lblNote.ForeColor = KioskStatic.CurrentTheme.FrmGetEmailDetailsLblNoteTextForeColor;
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.PaymentModeBtnHomeTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmGetEmailDetails: " + ex.Message);
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

    }
}
