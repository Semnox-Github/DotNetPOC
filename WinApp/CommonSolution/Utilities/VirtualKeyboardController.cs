/**************************************************************************************************
* Project Name - VirtualKeyboardController
* Description  - VirtualKeyboardController 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*************************************************************************************************** 
*2.70        26-Mar-2019      Guru S A           Booking phase 2 enhancement changes     
*2.70.2      08-Sep-2019      Girish Kundar      Added RemoveEventListner() method , get/set for showKeyboardOnTextboxEntry
*2.70.2      07-Nov-2022      Sathyavathi        Added Contructor that takes Y position
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ****************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.Utilities
{
    public class VirtualKeyboardController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Control form;
        AlphaNumericKeyPad keypad;
        private Control currentActiveTextBox;
        private DataGridView dataGridView;
        private DataGridViewCell cell;
        private bool showKeyboardOnTextboxEntry;
        List<Control> exclusionList;
        List<Control> showVirtualKeyboardButtonList;
        private int locationY = -1;
        private string keyboardFontName;
        /// <summary>
        /// VirtualKeyboardController
        /// </summary>
        public VirtualKeyboardController()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// VirtualKeyboardController
        /// </summary>
        /// <param name="locationY"></param>
        public VirtualKeyboardController(int locationY) : base()
        {
            log.LogMethodEntry(locationY);
            this.locationY = locationY;
            log.LogMethodExit();
        }
        /// <summary>
        /// Initialize
        /// </summary> 
        public void Initialize(Control form, List<Control> showVirtualKeyboardButtonList, bool showKeyboardOnTextboxEntry = true, List<Control> exclusionList = null, string keyboardFontName = null)
        {
            log.LogMethodEntry(form);
            this.form = form;
            this.keyboardFontName = keyboardFontName;
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Hiding.", ex);
            }
            log.LogMethodExit();
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
                if (keypad.Visible && !control.Capture)
                {
                    keypad.Hide();
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
                    keypad.currentTextBox = currentActiveTextBox;
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
                        if (textBoxTypeObject.ReadOnly == true)
                        {
                            if (keypad.Visible)
                            { keypad.Hide(); }
                            return;
                        }

                    }
                    if (keypad == null || keypad.IsDisposed)
                    {
                        keypad = new AlphaNumericKeyPad(FindParentForm(form), currentActiveTextBox, 100, keyboardFontName);
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
                        if(locationY > -1)
                        {
                            keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, locationY - keypad.Height);
                        }
                        keypad.Show();
                    }
                    else if (keypad.Visible)
                    {
                        keypad.Hide();
                    }
                    else
                    {
                        keypad.Show();
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
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while disposing the virtual keyboard", ex);
            }
            log.LogMethodExit("Ends -Dispose() method");
        }
    }
}
