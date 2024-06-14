/**************************************************************************************************
* Project Name - VirtualKeyboardController
* Description  - VirtualKeyboardController 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*************************************************************************************************** 
 *2.130.8     26-Jun-2022      Guru S A         OSK usage in Kiosk
 *2.150.0.0   10-Oct-2022      Sathyavathi      Added Constructor to take Y location
****************************************************************************************************/
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Semnox.Core.Utilities
{
    public class VirtualWindowsKeyboardController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Control form;
        private AlphaNumericKeyPad keypad;
        private Control currentActiveTextBox;
        private DataGridView dataGridView;
        private DataGridViewCell cell;
        private bool showKeyboardOnTextboxEntry;
        List<Control> exclusionList;
        List<Control> showVirtualKeyboardButtonList;
        private Process keyBoardProcess;
        private bool windowsKeyBoardIsHidden;
        private GlobalKeyboardHook globalKeyboardHook;
        private bool canMoveKeyboardAsPerUIElementPosition = false;
        private int locationY = -1;
        #region Constants
        private static Keys[] notAllowedKeys = { Keys.LControlKey, Keys.RControlKey, Keys.LMenu, Keys.RMenu, Keys.LWin, Keys.RWin, Keys.Apps,
            Keys.Control, Keys.F1, Keys.F2, Keys.F3, Keys.F4, Keys.F5, Keys.F6, Keys.F7, Keys.F8, Keys.F9, Keys.F10, Keys.F11, Keys.F12,
            Keys.ControlKey, Keys.Help, Keys.Home, Keys.PrintScreen, Keys.Menu, Keys.Print, Keys.Pause, Keys.LaunchMail, Keys.Modifiers,
            Keys.Zoom
        //        ,Keys.Alt
        };
        private const string REG_ENTRY_HELPPANE_WIN32 = @"Software\Classes\TypeLib\{8cec5860-07a1-11d9-b15e-000d56bfe6ee}\1.0\0\win32\";
        private const string REG_ENTRY_HELPPANE_WIN64 = @"Software\Classes\TypeLib\{8cec5860-07a1-11d9-b15e-000d56bfe6ee}\1.0\0\win64\";
        private const string HELPPANE_WIN32_CONFIG = @"HELPPANE_WIN32_CONFIG";
        private const string HELPPANE_WIN64_CONFIG = @"HELPPANE_WIN64_CONFIG";
        private const int SC_CLOSE = 0xF060;
        private const int SWP_NOZORDER = 0x4;
        private const int SWP_HIDEWINDOW = 0x80;
        private const int SWP_SHOWWINDOW = 0x40;
        private const int SWP_NOCOPYBITS = 0x100;
        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, int flags);
        [DllImport("kernel32.dll")]
        private static extern bool Wow64EnableWow64FsRedirection(bool set);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string className, string windowTitle);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hwnd, int command);
        #endregion
        private VirtualWindowsKeyboardController()
        {
            log.LogMethodEntry();
            this.currentActiveTextBox = null;
            log.LogMethodExit();
        } 
        /// <summary>
        /// VirtualWindowsKeyboardController
        /// </summary>
        public VirtualWindowsKeyboardController(bool canMoveKeyboardAsPerUIElementPosition) :base()
        {
            log.LogMethodEntry(canMoveKeyboardAsPerUIElementPosition);
            this.canMoveKeyboardAsPerUIElementPosition = canMoveKeyboardAsPerUIElementPosition;
            log.LogMethodExit();
        }
        /// <summary>
        /// VirtualWindowsKeyboardController
        /// </summary>
        /// <param name="locationY"></param>
        public VirtualWindowsKeyboardController(int locationY) : base()
        {
            log.LogMethodEntry(locationY);
            this.locationY = locationY;
            log.LogMethodExit();
        }
        /// <summary>
        /// Initialize
        /// </summary> 
        public void Initialize(Control form, List<Control> showVirtualKeyboardButtonList, bool showKeyboardOnTextboxEntry = true, List<Control> exclusionList = null)
        {
            log.LogMethodEntry(form);
            windowsKeyBoardIsHidden = true;
            this.form = form;
            this.showKeyboardOnTextboxEntry = showKeyboardOnTextboxEntry;
            this.showVirtualKeyboardButtonList = showVirtualKeyboardButtonList;
            if (showVirtualKeyboardButtonList == null)
            {
                showVirtualKeyboardButtonList = new List<Control>();
            }
            foreach (var showVirtualKeyboardButton in showVirtualKeyboardButtonList)
            {
                showVirtualKeyboardButton.Click += ShowVirtualKeyboardButton_Click;
            }
            this.exclusionList = exclusionList;
            if (exclusionList == null)
            {
                this.exclusionList = new List<Control>();
            }
            AddEventListener(form);
            SetGlobalKeyboardHook();
            log.LogMethodExit();
        }
        /// <summary>
        /// ShowKeyboardOnTextboxEntry
        /// </summary>
        public bool ShowKeyboardOnTextboxEntry
        {
            get
            {
                return showKeyboardOnTextboxEntry;
            }
            set
            {
                showKeyboardOnTextboxEntry = value;
            }
        }

        private void AddEventListener(Control control)
        {
            if (exclusionList.Contains(control))
            {
                return;
            }
            if (control is TextBox)
            {
                control.Enter += Control_Enter;
                control.Click += Control_Enter;
                control.Leave += Control_Leave;
            }
            else if (control is MaskedTextBox)
            {
                control.Enter += Control_Enter;
                control.Click += Control_Enter;
                control.Leave += Control_Leave;
            }
            else if (control is ComboBox)
            {
                ComboBox comboBox = control as ComboBox;
                if (comboBox.AutoCompleteMode == AutoCompleteMode.SuggestAppend &&
                    comboBox.AutoCompleteSource == AutoCompleteSource.ListItems)
                {
                    control.Enter += Control_Enter;
                    control.Click += Control_Enter;
                    control.Leave += Control_Leave;
                }
            }
            else if (control is DataGridView)
            {
                (control as DataGridView).EditingControlShowing += dgv_EditingControlShowing;
            }
            else if (control.Controls != null && control.Controls.Count > 0)
            {
                foreach (Control childControl in control.Controls)
                {
                    AddEventListener(childControl);
                }
            }
        }

        /// <summary>
        /// Remove Event Listeners from the Controls
        /// </summary>
        /// <param name="control">control</param>
        public void RemoveEventListener(Control control)
        {

            if (exclusionList != null && exclusionList.Contains(control))
            {
                return;
            }
            if (control is TextBox)
            {
                control.Enter -= Control_Enter;
                control.Click -= Control_Enter;
                control.Leave -= Control_Leave;
            }
            else if (control is MaskedTextBox)
            {
                control.Enter -= Control_Enter;
                control.Click -= Control_Enter;
                control.Leave -= Control_Leave;
            }
            else if (control is ComboBox)
            {
                ComboBox comboBox = control as ComboBox;
                if (comboBox.AutoCompleteMode == AutoCompleteMode.SuggestAppend &&
                    comboBox.AutoCompleteSource == AutoCompleteSource.ListItems)
                {
                    control.Enter -= Control_Enter;
                    control.Click -= Control_Enter;
                    control.Leave -= Control_Leave;
                }
            }
            else if (control is DataGridView)
            {
                (control as DataGridView).EditingControlShowing -= dgv_EditingControlShowing;
            }
            else if (control.Controls != null && control.Controls.Count > 0)
            {
                foreach (Control childControl in control.Controls)
                {
                    RemoveEventListener(childControl);
                }
            }
        }

        /// <summary>
        /// Refreshes the KeyBoard control List
        /// </summary>
        public void Refresh()
        {
            log.LogMethodEntry();
            if (form != null)
            {
                RemoveEventListener(form);
                AddEventListener(form);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// HideKeyboard
        /// </summary>
        public void HideKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                if (keypad != null && keypad.Visible)
                {
                    keypad.Hide();
                }
                if (keyBoardProcess != null)
                {
                    HideOnScreenKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Hiding.", ex);
            }
            log.LogMethodExit();
        }
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry();
            if (e.Control is TextBox)
            {
                Control_Enter(e.Control, null);
                dataGridView = sender as DataGridView;
                cell = dataGridView.CurrentCell;
            }
            log.LogMethodExit();
        }

        private void Control_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Control control = (sender) as Control;
                if (keypad != null && keypad.Visible && !control.Capture)
                {
                    keypad.Hide();
                }
                if (keyBoardProcess != null)
                {
                    HideOnScreenKeyboard();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Hiding.", ex);
            }
            log.LogMethodExit();
        }
        private void Control_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dataGridView = null;
                currentActiveTextBox = sender as TextBox;
                if (currentActiveTextBox == null)
                    currentActiveTextBox = sender as MaskedTextBox;
                if (keypad != null && !keypad.IsDisposed && currentActiveTextBox != null)
                {
                    keypad.currentTextBox = currentActiveTextBox;
                }
                else if (keyBoardProcess != null && currentActiveTextBox != null)
                {
                    //Keyboard.Focus(currentActiveTextBox); 
                    currentActiveTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while setting the active control.", ex);
            }
            try
            {
                if (showKeyboardOnTextboxEntry)
                {
                    if (!(keypad == null || keypad.IsDisposed) && keypad.Visible)
                    {
                        keypad.Hide();
                    }
                    else if (keyBoardProcess != null)
                    {
                        HideOnScreenKeyboard();
                    }
                    ShowKeyBoard();
                }
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
                throw;
            }
            log.LogMethodExit();
        }

        private void ShowVirtualKeyboardButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ShowKeyBoard();
            log.LogMethodExit();
        }

        private void ShowKeyBoard()
        {
            log.LogMethodEntry();
            if (currentActiveTextBox != null)
            {
                try
                {
                    TextBoxBase textBoxTypeObject = null;
                    if (currentActiveTextBox is TextBoxBase)
                    {
                        textBoxTypeObject = (TextBoxBase)currentActiveTextBox;
                        if (textBoxTypeObject.ReadOnly == true || textBoxTypeObject.Enabled == false)
                        {
                            if (keypad != null && keypad.Visible)
                            { keypad.Hide(); }
                            if (keyBoardProcess != null)
                            {
                                HideOnScreenKeyboard();
                            }
                            return;
                        }

                    }
                    if (keyBoardProcess == null || keyBoardProcess.HasExited)
                    {
                        if (ShowOnScreenKeyboard())
                        {
                            if (!showKeyboardOnTextboxEntry)
                            {
                                //virtualbuttonclicked = true;
                            }
                            if (currentActiveTextBox != null)
                            {
                                currentActiveTextBox.Focus();
                            }
                            //System.Windows.Input.Keyboard.Focus(currentActiveTextBox);
                            return;
                        }
                        else
                        {
                            if (keypad == null || keypad.IsDisposed)
                            {
                                keypad = new AlphaNumericKeyPad(FindParentForm(form), currentActiveTextBox);
                                try
                                {
                                    if (dataGridView != null)
                                    {
                                        //dataGridView.CurrentCell = cell;
                                        //dataGridView.BeginEdit(true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error occurred while selecting the current cell", ex);
                                }

                                //May 23 2016
                                //keypad.Location = new Point(tabPageCardCustomer.PointToScreen(btnShowKeyPad.Location).X - keypad.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                                // keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                                keypad.Show();
                            }
                        }
                    }
                    else
                    {
                        if (keyBoardProcess == null && keypad != null)
                        {
                            if (keypad.Visible)
                            {
                                keypad.Hide();
                            }
                            else
                            {
                                keypad.Show();
                            }
                        }
                        else
                        {
                            if (windowsKeyBoardIsHidden == false)
                            {
                                HideOnScreenKeyboard();
                            }
                            else
                            {
                                ShowOnScreenKeyboard();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while showing virtual keyboard", ex);
                }
            }
            log.LogMethodExit();
        }

        private Form FindParentForm(Control childControl)
        {
            log.LogMethodEntry(childControl);
            Form parent = null;
            if (childControl is Form)
            {
                parent = childControl as Form;
            }
            else
            {
                parent = FindParentForm(childControl.Parent);
            }
            log.LogMethodExit(parent);
            return parent;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            log.LogMethodEntry("starts Dispose method");
            try
            {
                if (keypad != null)
                {
                    keypad.Close();
                    keypad.Dispose();
                }
                Kill();
                if (globalKeyboardHook != null)
                {
                    globalKeyboardHook.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while disposing the virtual keyboard", ex);
            }
            log.LogMethodExit("Ends -Dispose() method");
        }
        private bool ShowOnScreenKeyboard()
        {
            log.LogMethodEntry();
            bool oskOpened = false;
            if (this.form != null)
            {
                if (keyBoardProcess == null)
                {
                    keyBoardProcess = new Process();
                    keyBoardProcess.StartInfo = new ProcessStartInfo();
                    keyBoardProcess.StartInfo.FileName = "osk.exe";
                }
                //else
                //{
                //    HideOnScreenKeyboard();
                //}
                //bool validWindow = OnScreenWindow();
                //if (validWindow)
                {
                    try
                    {
                        Wow64EnableWow64FsRedirection(false);
                        keyBoardProcess.Start();
                        oskOpened = true;
                        windowsKeyBoardIsHidden = false;
                        Wow64EnableWow64FsRedirection(true);
                        SetOSKKey();
                        keyBoardProcess.WaitForInputIdle();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        if (!oskOpened)
                        {
                            try
                            {
                                keyBoardProcess.Start();
                                oskOpened = true;
                                windowsKeyBoardIsHidden = false;
                                SetOSKKey();
                            }
                            catch (Exception innerEx)
                            {
                                log.Error(innerEx);
                            }
                        }

                    }
                }
            }
            log.LogMethodExit(oskOpened);
            return oskOpened;
        }
        private void SetOSKKey()
        {
            log.LogMethodEntry();
            if (keyBoardProcess != null)
            {
                int widthalue = (this.form != null && this.form.Width > 930 ? this.form.Width : 930);
                System.Windows.Rect rect = new Rect(0, 0, widthalue, 300);
                System.Drawing.Point position = GetWindowPosition(rect.Width, rect.Height);

                if (locationY != -1)
                {
                    rect = new Rect(position.X, locationY - rect.Height, rect.Width, rect.Height);
                }
                else
                {
                    if (canMoveKeyboardAsPerUIElementPosition == false)
                    {
                        rect = new Rect(position.X, position.Y, rect.Width, rect.Height);
                    }
                    else if (currentActiveTextBox != null)
                    {
                        System.Drawing.Point loc = currentActiveTextBox.PointToScreen(System.Drawing.Point.Empty);
                        double yLoc = loc.Y;
                        if (loc.Y + 5 > rect.Height)
                        {
                            yLoc = loc.Y - rect.Height - 5;
                        }
                        else if (loc.Y + 5 + rect.Height < widthalue)
                        {
                            yLoc = loc.Y + rect.Height + 5;
                        }
                        else
                        {
                            loc = GetWindowPosition(rect.Width, rect.Height);
                            yLoc = loc.Y;
                        }
                        rect = new Rect(position.X, yLoc, rect.Width, rect.Height);
                    }
                }
                //HideToolbars();
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Osk", true);
                if (myKey != null)
                {
                    myKey.SetValue("WindowLeft", rect.X, RegistryValueKind.DWord);
                    myKey.SetValue("WindowTop", rect.Y, RegistryValueKind.DWord);
                    myKey.SetValue("WindowWidth", rect.Width, RegistryValueKind.DWord);
                    myKey.SetValue("WindowHeight", rect.Height, RegistryValueKind.DWord);
                }
            }
            log.LogMethodExit();
        }
        private System.Drawing.Point GetWindowPosition(double width, double height)
        {
            log.LogMethodEntry();
            System.Drawing.Point position = new System.Drawing.Point();
            if (currentActiveTextBox != null)
            {
                System.Drawing.Point p = currentActiveTextBox.PointToScreen(new System.Drawing.Point(0, currentActiveTextBox.Height));
                position.X = (int)(this.form.Width - width) / 3;
                double windowHeight = this.form.Height - height - (height / 5);
                if (p.Y >= windowHeight)
                {
                    position.Y = 20;
                }
                else
                {
                    position.Y = (int)windowHeight;
                }
            }
            log.LogMethodExit(position);
            return position;
        }
        private void HideOnScreenKeyboard()
        {
            log.LogMethodEntry();
            try
            {
                if (keyBoardProcess != null)
                {
                    uint WM_SYSCOMMAND = 0x0112;
                    IntPtr iHandle = FindWindow("OSKMainClass", null);
                    int n = SendMessage(iHandle, WM_SYSCOMMAND, SC_CLOSE, 0);
                    windowsKeyBoardIsHidden = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void Kill()
        {
            log.LogMethodEntry();
            try
            {
                if (keyBoardProcess != null)
                {
                    currentActiveTextBox = null;
                    keyBoardProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private GlobalKeyboardHook SetGlobalKeyboardHook()
        {
            log.LogMethodEntry();
            globalKeyboardHook = new GlobalKeyboardHook();
            globalKeyboardHook.HookedKeys.AddRange(notAllowedKeys);
            globalKeyboardHook.KeyDown += new System.Windows.Forms.KeyEventHandler(GlobalKeyboardHookKeyDown);
            globalKeyboardHook.KeyUp += new System.Windows.Forms.KeyEventHandler(GlobalKeyboardHookKeyUp);
            log.LogMethodExit();
            return globalKeyboardHook;
        }
        private void GlobalKeyboardHookKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            log.LogMethodEntry();
            e.Handled = true;
            log.LogMethodExit();
        }
        private void GlobalKeyboardHookKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            log.LogMethodEntry();
            e.Handled = true;
            log.LogMethodExit();
        }
        //Windows toolbar hide and show. Not used as of now
        private void HideToolbars()
        {
            log.LogMethodEntry();
            IntPtr ihWnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(ihWnd, SW_HIDE);
            log.LogMethodExit();
        }
        private void ShowToolbars()
        {
            log.LogMethodEntry();
            IntPtr ihWnd = FindWindow("Shell_TrayWnd", "");
            ShowWindow(ihWnd, SW_SHOW);
            log.LogMethodExit();
        }
        /// <summary>
        /// Clear Helppane Settings
        /// </summary>
        public static void ClearHelpPaneSettings()
        {
            try
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(REG_ENTRY_HELPPANE_WIN32, true);
                if (myKey != null)
                {
                    Object o = myKey.GetValue("");
                    if (o != null)
                    {
                        string keyValue = (o as String);
                        if (string.IsNullOrWhiteSpace(keyValue) == false)
                        {
                            UpdateAppConfig(HELPPANE_WIN32_CONFIG, keyValue);
                        }
                    }
                    myKey.SetValue("", "", RegistryValueKind.String);
                }
                RegistryKey myKeyWin64 = Registry.CurrentUser.OpenSubKey(REG_ENTRY_HELPPANE_WIN64, true);
                if (myKeyWin64 != null)
                {
                    Object o = myKeyWin64.GetValue("");
                    if (o != null)
                    {
                        string keyValue = (o as String);
                        if (string.IsNullOrWhiteSpace(keyValue) == false)
                        {
                            UpdateAppConfig(HELPPANE_WIN64_CONFIG, keyValue);
                        }
                    }
                    myKeyWin64.SetValue("", "", RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        /// <summary>
        /// Restore Helppane Settings
        /// </summary>
        public static void RestoreHelpPaneSettings()
        {
            log.LogMethodEntry();
            try
            {
                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(REG_ENTRY_HELPPANE_WIN32, true);
                if (myKey != null)
                {
                    string helpPaneSettingsWin32 = ReadAppConfigSetting(HELPPANE_WIN32_CONFIG);
                    myKey.SetValue("", helpPaneSettingsWin32, RegistryValueKind.String);
                }
                RegistryKey myKeyWin64 = Registry.CurrentUser.OpenSubKey(REG_ENTRY_HELPPANE_WIN64, true);
                if (myKeyWin64 != null)
                {
                    string helpPaneSettingsWin64 = ReadAppConfigSetting(HELPPANE_WIN64_CONFIG);
                    myKeyWin64.SetValue("", helpPaneSettingsWin64, RegistryValueKind.String);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Update App Config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateAppConfig(string key, string value)
        {
            log.LogMethodEntry(key, value);
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                bool modifiedKeyValue = false;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                    modifiedKeyValue = true;
                }
                else
                {
                    if (settings[key].Value != value)
                    {
                        settings[key].Value = value;
                        modifiedKeyValue = true;
                    }
                }
                if (modifiedKeyValue)
                {
                    configFile.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Read App Config Setting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ReadAppConfigSetting(string key)
        {
            log.LogMethodEntry(key);
            string result = string.Empty;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                result = appSettings[key];
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
            return result;
        }
    }
}
