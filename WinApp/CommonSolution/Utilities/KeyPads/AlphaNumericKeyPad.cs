/********************************************************************************************
* Project Name - Alphanumeric keyboard
* Description  - 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.60.0      13-Mar-2019      Raghuveera         Added on screen lable to display typed text.
*2.70.2.0    23-Sep-2019     Girish Kundar       Modified : CustomerUI Changes 
*2.140.0     18-Oct-2021     Sathyavathi         Added a Delegate to reset timer 
*2.150.1     22-Feb-2023     Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Semnox.Core.Utilities
{
    public partial class AlphaNumericKeyPad : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        bool blnCaps = true;
        public Panel PanelKeyPad;
        Form baseForm;
        private bool isPasswordText = false;
        private Control selectedTextBox;
        public delegate void ResetTimerDelegate();
        public ResetTimerDelegate resetTimer;

        public Control currentTextBox { get { return selectedTextBox; }
            set { selectedTextBox = value;
                isPasswordText = ((selectedTextBox.GetType().ToString().Contains("TextBox"))?(((TextBox)selectedTextBox).PasswordChar != '\0'): false);
               // lblText.Text = (isPasswordText) ? "".PadRight(selectedTextBox.Text.Length, '*') : selectedTextBox.Text;
            } }
        public bool FirstKeyPressed = false;
        //int _sizePercent;
        public AlphaNumericKeyPad(int sizePercentage = 100, string fontName = null)
        {
            log.LogMethodEntry(sizePercentage);
            InitializeComponent();
            SetFont(fontName);
            PanelKeyPad = pnlKeyBoard;
            baseForm = this;
            selectedTextBox = txtDummy;
            isPasswordText = (txtDummy.PasswordChar != '\0');
           // lblText.Text = (isPasswordText)?"".PadRight(selectedTextBox.Text.Length, '*'): selectedTextBox.Text;
            //_sizePercent = sizePercentage;
            resize(sizePercentage);
            log.LogMethodExit();
        }

        public AlphaNumericKeyPad(Form BaseForm, Control FocusTextBox, int sizePercentage = 100, string fontName = null)
        {
            log.LogMethodEntry(BaseForm, FocusTextBox, sizePercentage);
            InitializeComponent();
            SetFont(fontName);
            PanelKeyPad = pnlKeyBoard;
            baseForm = BaseForm;
            if (FocusTextBox != null)
            {
                selectedTextBox = FocusTextBox;

                if (FocusTextBox.GetType().ToString().Contains("MaskedTextBox") == false &&
                    FocusTextBox.GetType().ToString().Contains("TextBox"))
                {
                    isPasswordText = (((TextBox)FocusTextBox).PasswordChar != '\0');
                }
                else
                {
                    isPasswordText = false;
                }

                // lblText.Text = (isPasswordText) ? "".PadRight(selectedTextBox.Text.Length, '*') : selectedTextBox.Text;
                //_sizePercent = sizePercentage;
                resize(sizePercentage);
                Point textBoxLocationOnScreen = FocusTextBox.PointToScreen(FocusTextBox.Location);
                if (textBoxLocationOnScreen.Y >= (Screen.PrimaryScreen.WorkingArea.Height - this.Height))
                {
                    Rectangle rcScreen = Screen.PrimaryScreen.WorkingArea;
                    this.Location = new Point(((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2), 20);
                }
                else
                {
                    this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - this.Height);
                }
            }
            log.LogMethodExit();          
        }

        void resize(int sizePercent)
        {
            log.LogMethodEntry(sizePercent);
            //if (sizePercent == 100)
            //    return;

            //double factor = Convert.ToDouble(sizePercent / 100.0);
            //this.Size = new Size((int)(this.Width + btnQ.Width * 13 * (factor - 1.0)), (int)(this.Height + btnQ.Height * 4 * (factor - 1.0)));

            //foreach(Control c in flpKeys.Controls)
            //{
            //    c.Size = new Size((int)(c.Width * factor), (int)(c.Height * factor));
            //}

            //btnCaps.Width = btnBack.Width = btnDotCom.Width = btnW.Right - btnQ.Left;
            //btnSpace.Width = btnE.Right - btnQ.Left;
            log.LogMethodExit();
        }

        void writeText(string key)
        {
            log.LogMethodEntry(key);
            if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
            {
                //lblText.Text += (isPasswordText) ? "".PadRight(key.Length, '*') : key;
                baseForm.ActiveControl = selectedTextBox;
                selectedTextBox.Text += key;                
            }
            log.LogMethodExit();
        }

        // virtual Key board events
        private void btnNumberKey_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button objBtn = (Button)sender;
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                    baseForm.ActiveControl = selectedTextBox;
                baseForm.Activate();
                if (baseForm.ActiveControl != null)
                {
                    baseForm.ActiveControl.Focus();
                }
                SendKeys.Send(objBtn.Text.ToLower());
                InvokeResetTimer();
                // lblText.Text += (isPasswordText) ? "".PadRight(objBtn.Text.Length, '*') : objBtn.Text.ToLower();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred", ex);
            }
            log.LogMethodExit();
        }

        private void btnAlphabetKey_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                Button objBtn = (Button)sender;
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                {
                    baseForm.ActiveControl = selectedTextBox;
                }
                baseForm.Activate();
                if (baseForm.ActiveControl != null)
                {
                    baseForm.ActiveControl.Focus();
                }
                if (blnCaps)
                {
                    //  lblText.Text += (isPasswordText) ? "".PadRight(objBtn.Text.Length, '*') : objBtn.Text.ToUpper();
                    SendKeys.Send(objBtn.Text.ToUpper());
                    InvokeResetTimer();
                }
                else
                {
                    //  lblText.Text += (isPasswordText)?"".PadRight(objBtn.Text.Length,'*'): objBtn.Text.ToLower();
                    SendKeys.Send(objBtn.Text.ToLower());
                    InvokeResetTimer();
                }
                if (!FirstKeyPressed)
                {
                    FirstKeyPressed = true;
                    blnCaps = true;
                    toggleCaps();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred", ex);
            }
            log.LogMethodExit();
        }

        private void btnCaps_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            toggleCaps();
            FirstKeyPressed = true;
            log.LogMethodExit();
        }

        public void UpperCase()
        {
            log.LogMethodEntry();
            blnCaps = false;
            toggleCaps();
            log.LogMethodExit();
        }

        public void LowerCase()
        {
            log.LogMethodEntry();
            blnCaps = true;
            toggleCaps();
            log.LogMethodExit();
        }

        void toggleCaps()
        {
            log.LogMethodEntry();
            try
            {
                blnCaps = !blnCaps;

                foreach (Control c in flpKeys.Controls)
                {
                    if (c.Name == "btnCaps" || c.Name == "btnBack" || c.Name == "btnSpace" || c.Name == "btnEnter")
                        continue;
                    if (blnCaps)
                        c.Text = c.Text.ToUpper();
                    else
                        c.Text = c.Text.ToLower();
                }

                if (blnCaps)
                {
                    btnCaps.Image = Semnox.Core.Utilities.Properties.Resources.KB_Shift_Button_Normal;
                }
                else
                {
                    btnCaps.Image = Semnox.Core.Utilities.Properties.Resources.KB_Shift_Button_Pressed;
                }
                InvokeResetTimer();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred", ex);
            }
            
            log.LogMethodExit();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                    baseForm.ActiveControl = selectedTextBox;
                baseForm.Activate();
                if (baseForm.ActiveControl != null)
                {
                    baseForm.ActiveControl.Focus();
                }
                SendKeys.Send("{BKSP}");
                InvokeResetTimer();
                // lblText.Text = (isPasswordText)?"".PadRight((string.IsNullOrEmpty(lblText.Text) ? "" : lblText.Text.Substring(0, lblText.Text.Length - 1)).Length,'*') :string.IsNullOrEmpty(lblText.Text) ? "" : lblText.Text.Substring(0, lblText.Text.Length - 1);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred", ex);
            }
            
            log.LogMethodExit();
        }

        private void btnSpace_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                {
                    baseForm.ActiveControl = selectedTextBox;
                    baseForm.Activate();
                    if (baseForm.ActiveControl != null)
                    {
                        baseForm.ActiveControl.Focus();
                    }
                    SendKeys.Send(" ");
                    InvokeResetTimer();
                    //  lblText.Text += (isPasswordText)?"*":" ";
                }
            }
            catch (Exception ex)
            {

                log.Error("Error occurred", ex);
            }
            
            log.LogMethodExit();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                    baseForm.ActiveControl = selectedTextBox;
                baseForm.Activate();
                if (baseForm.ActiveControl != null)
                {
                    baseForm.ActiveControl.Focus();
                }
                SendKeys.Send("{+}");
                InvokeResetTimer();
                //  lblText.Text += (isPasswordText) ? "*" : "+";
            }
            catch (Exception ex) 
            {

                log.Error("Error occurred", ex);
            }
            
            log.LogMethodExit();
        }

        private void AlphaNumericKeyPad_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Timer delayedStart = new Timer();
            delayedStart.Interval = 10;
            delayedStart.Tick += new EventHandler(delayedStart_Tick);
            delayedStart.Start();
            log.LogMethodExit();
        }

        void delayedStart_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                (sender as Timer).Stop();

                baseForm.Activate();
                if (selectedTextBox != null && selectedTextBox.Visible && selectedTextBox.Enabled)
                {
                    baseForm.ActiveControl = selectedTextBox;
                    selectedTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while focusing the textbox", ex);
                
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
            log.LogMethodExit();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            Close();
            log.LogMethodExit();
        }

        private void btnDotCom_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (selectedTextBox != null)
                {
                    baseForm.ActiveControl = selectedTextBox;
                    baseForm.Activate();
                    if (baseForm.ActiveControl != null)
                    {
                        baseForm.ActiveControl.Focus();
                    }
                    //  lblText.Text += (isPasswordText) ? "".PadRight(btnDotCom.Text.Length,'*') : btnDotCom.Text;
                    SendKeys.Send(btnDotCom.Text);
                    InvokeResetTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while sending text", ex);
            }
            
            log.LogMethodExit();
        }

        private void InvokeResetTimer()
        {
            log.LogMethodEntry();
            try
            {
                if (resetTimer != null)
                {
                    resetTimer();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void SetFont(string fontName)
        {
            log.LogMethodEntry();
            if (string.IsNullOrWhiteSpace(fontName) == false)
            {
                Font defaultFont = this.btn1.Font;
                Font keyBoardFont = new Font(fontName, defaultFont.Size, defaultFont.Style, defaultFont.Unit, defaultFont.GdiCharSet);
                pnlKeyBoard.SuspendLayout();
                for (int i = 0; i < pnlKeyBoard.Controls.Count; i++)
                {
                    Control flpCtrlElement = pnlKeyBoard.Controls[i];
                    flpCtrlElement.Font = keyBoardFont;
                    if (flpCtrlElement.Controls != null && flpCtrlElement.Controls.Count > 0)
                    {
                        for (int j = 0; j < flpCtrlElement.Controls.Count; j++)
                        {
                            Control ctrlElement = flpCtrlElement.Controls[j];
                            ctrlElement.Font = keyBoardFont;
                        }
                    }
                }
                pnlKeyBoard.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
    }
}
