/********************************************************************************************
* Project Name - Parafait_Kiosk - frmTextBox
* Description  - Generic textbox that can be called from any form.  
* 
**************
**Version Log
**************
*Version     Date             Modified By            Remarks          
*********************************************************************************************
*2.150.3.0   04-Apr-2023      Sathyavathi            Created
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmTextBox: BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private VirtualWindowsKeyboardController virtualKeyboardController;
        private VirtualKeyboardController customKeyboardController;

        public string TextBoxData { get { return txtBoxData.Text; } }
        private bool isMandatory;
        private string txtBoxName ;
        private ExecutionContext executionContext;
        public frmTextBox(ExecutionContext executionContext, string message, string txtboxData, string fieldName, bool isDataMandatory)
        {
            log.LogMethodEntry(executionContext, message, txtboxData, fieldName, isDataMandatory);
            this.executionContext = executionContext;
            InitializeComponent();
            this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgBox);
            btnConfirm.BackgroundImage = btnClose.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.OkMsgButtons);//Modification on 17-Dec-2015 for introducing new theme
            this.Size = this.BackgroundImage.Size;
            KioskStatic.setDefaultFont(this);

            lblmsg.Text = message;
            txtBoxData.Text = txtboxData;
            isMandatory = isDataMandatory;
            txtBoxName = fieldName;
            InitializeKeyboard();

            SetKioskTimerTickValue(30);
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmTextBox_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            SetFocus();
            HideKeyboardObject();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = DialogResult.No;
            log.LogMethodExit();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (isMandatory)
                {
                    if (string.IsNullOrWhiteSpace(txtBoxData.Text))
                    {
                        string errMsg = MessageContainerList.GetMessage(executionContext, 249, txtBoxName);
                        //&1 is mandatory. Please enter a value.
                        ValidationException validationException = new ValidationException(errMsg);
                        throw validationException;
                    }
                }
                DialogResult = DialogResult.Yes;
                log.LogVariableState("TEXTBOX DATA: ", txtBoxData.Text);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                frmOKMsg.ShowUserMessage(ex.Message);
                KioskStatic.logToFile("Error in frmTextBox confirm action: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void InitializeKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                KioskStatic.logToFile("Error Initializing keyboard in  frmTextBox screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                customKeyboardController = new VirtualKeyboardController(880);
                bool showKeyboardOnTextboxEntry = true;
                customKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, showKeyboardOnTextboxEntry, null, lblmsg.Font.FontFamily.Name);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Custom Keyboard in  frmTextBox screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetVirtualKeyboard()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                virtualKeyboardController = new VirtualWindowsKeyboardController(880);
                bool popupOnScreenKeyBoard = true;
                virtualKeyboardController.Initialize(this, new List<Control>() { btnShowKeyPad }, popupOnScreenKeyBoard);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error Initializing Windows Keyboard in  frmTextBox screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnShowKeyPad_Click_1(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
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
                KioskStatic.logToFile("Error Disposing keyboard in  frmTextBox screen: " + ex.Message);
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
                base.CloseForms();
                Dispose();
            }
            log.LogMethodExit();
        }

        private void frmTextBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DisposeKeyboardObject();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing frmTextBox_FormClosing()", ex);
            }
            log.Info(this.Name + ": Form closed");
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.TextBoxLblMsgTextForeColor;
                this.txtBoxData.ForeColor = KioskStatic.CurrentTheme.TextBoxTxtDataTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.YesNoScreenBtnNoTextForeColor;
                this.btnConfirm.ForeColor = KioskStatic.CurrentTheme.YesNoScreenBtnYesTextForeColor;
                
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmTextBox: " + ex.Message);
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
                KioskStatic.logToFile("Error in HideKeyboardObject() in TextBox screen: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetFocus()
        {
            log.LogMethodEntry();
            try
            {
                txtBoxData.Focus();
                txtBoxData.Select();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in SetFocus(): " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
