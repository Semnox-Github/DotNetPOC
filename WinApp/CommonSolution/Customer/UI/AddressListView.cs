/********************************************************************************************
 * Project Name - AddressListView
 * Description  -AddressListView is a User Control component to display address details in the form.
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 2.70.2        18-Aug-2019       Girish Kundar   Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{

    public partial class AddressListView : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AddressDTO> addressDTOList;
        private bool showActiveEntries;
        private bool showKeyboardOnTextboxEntry;
        private ExecutionContext executionContext;
        [field: NonSerialized]
        public event EventHandler RefreshAddressDTOList;
        private bool controlsEnabled = true;
        public ExecutionContext SetExecutionContext { set { executionContext = value; } }
        /// <summary>
        /// Constructors for AddressListView
        /// </summary>
        public AddressListView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            executionContext = null;
            log.LogMethodExit();
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public List<AddressDTO> AddressDTOList
        {
            get
            {
                return addressDTOList;
            }

            set
            {
                addressDTOList = value;
                UpdateAddressListUI();
            }
        }

        /// <summary>
        /// Get set for ShowActiveAddressEntries 
        /// </summary>
        public bool ShowActiveAddressEntries
        {
            get
            {
                return chbShowActiveAddressEntries.Checked ? true : false;
            }
            set
            {
                showActiveEntries = value;
                if (showActiveEntries) chbShowActiveAddressEntries.CheckState = CheckState.Checked;
                else
                    chbShowActiveAddressEntries.CheckState = CheckState.Unchecked;
            }

        }


        public bool ShowKeyboardOnTextboxEntry
        {
            get { return showKeyboardOnTextboxEntry; }
            set { showKeyboardOnTextboxEntry = value; }
        }

        /// <summary>
        /// Updates the Address List view 
        /// </summary>

        public void UpdateAddressListUI()
        {
            log.LogMethodEntry();
            flpAddressListView.Controls.Clear();
            flpAddressListView.HorizontalScroll.Enabled = false;
            if (addressDTOList == null || addressDTOList.Any() == false)
            {
                log.LogMethodExit(null, "AddressDTOListEmpty");
                return;
            }
            IEnumerable<AddressDTO> filteredDTOList = chbShowActiveAddressEntries.Checked ? addressDTOList.Where(x => x.IsActive) : addressDTOList;
            foreach (AddressDTO addressDTO in filteredDTOList)
            {
                AddressView addressView = new AddressView(showKeyboardOnTextboxEntry);
                addressView.SetControlsEnabled(controlsEnabled);
                addressView.AddressDTO = addressDTO;
                addressView.DeleteAddressView += AddressView_OnDelete;
                addressView.UpdateList += AddressView_OnUpdate;
                flpAddressListView.Controls.Add(addressView);
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Called when User modified the Address information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddressView_OnUpdate(Object sender, EventArgs e)
        {
            log.LogMethodEntry();
            UpdateAddressListUI();
            log.LogMethodExit();
        }

        /// <summary>
        /// Called when User deletes the AddressView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddressView_OnDelete(Object sender, AddressDeleteEventArgs e)
        {
            log.LogMethodEntry();
            if (e.AddressDTO.Id < 0)
            {
                // remove DTO with Id =-1 ,
                addressDTOList.Remove(e.AddressDTO);
            }
            else
            {
                AddressView addressView = (AddressView)sender as AddressView;
                AddressDTO addressDTO = addressDTOList.Where(x => x.Id == e.AddressDTO.Id).FirstOrDefault();
                if (addressDTO != null)
                {
                    addressDTO.IsActive = false;
                }
            }
            UpdateAddressListUI();
            log.LogMethodExit();
        }

        /// <summary>
        ///  Opens the frmCustomerAddress form to add new Address Info
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddControl_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            AddressDTO addressDTO = new AddressDTO();
            bool showOnScreenKeyBoard = ParafaitDefaultContainerList.GetParafaitDefault<bool>(ExecutionContext.GetExecutionContext(), "AUTO_POPUP_ONSCREEN_KEYBOARD");
            using (frmCustomerAddress frm = new frmCustomerAddress(addressDTO, showOnScreenKeyBoard))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    this.addressDTOList.Add(frm.addressDTO);
                    UpdateAddressListUI();
                }
            }
            flpAddressListView.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// When Active flag is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowActive_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ShowActiveAddressEntries = chbShowActiveAddressEntries.CheckState == CheckState.Checked ? true : false;
            // Raise the event for get records from DB
            if (RefreshAddressDTOList != null)
            {
                RefreshAddressDTOList.Invoke(this, EventArgs.Empty);
            }
            UpdateAddressListUI();
            log.LogMethodExit();
        }

        /// <summary>
        /// Shows the Address validation errors 
        /// </summary>
        /// <param name="addressValidationErrors">addressValidationErrors</param>
        public void ShowValidationErrors(List<ValidationError> addressValidationErrors)
        {
            log.LogMethodEntry(addressValidationErrors);
            foreach (var validationError in addressValidationErrors)
            {

                if (validationError.RecordIndex < 0 ||
                    addressDTOList.Count <= validationError.RecordIndex)
                {
                    continue;
                }
                AddressDTO addressDTO = addressDTOList[validationError.RecordIndex];
                foreach (Control c in flpAddressListView.Controls)
                {
                    AddressView addressView = c as AddressView;
                    if (addressView != null)
                    {
                        addressView.ShowValidationError(addressDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the error indication colors on the controls.
        /// </summary>
        public void ClearValdationErrors()
        {
            log.LogMethodEntry();
            foreach (Control c in flpAddressListView.Controls)
            {
                AddressView addressView = c as AddressView;
                if (addressView != null)
                {
                    addressView.ClearValdationError();
                }
            }
            log.LogMethodExit();
        }

        internal void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            controlsEnabled = value;
            btnAddControl.Visible = value;
            foreach (Control control in flpAddressListView.Controls)
            {
                AddressView addressView = control as AddressView;
                if(addressView == null)
                {
                    continue;
                }
                addressView.SetControlsEnabled(value);
            }
            log.LogMethodExit();
        }
    }
}
