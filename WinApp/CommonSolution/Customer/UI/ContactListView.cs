/********************************************************************************************
 * Project Name - ContactListView
 * Description  -ContactListView is a User Control component to display Contact details in the form.
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
    /// <summary>
    /// This is the class for ContactListView user control to display contact types.
    /// </summary>
    public partial class ContactListView : UserControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ContactDTO> contactDTOList;
        private bool showActiveEntry = true;
        private List<ContactTypeDTO> contactTypeDTOList;
        private ExecutionContext executionContext;
        public event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;
        [field: NonSerialized]
        public event EventHandler RefreshVirtualKeyBoard;
        [field: NonSerialized]
        public event EventHandler RefreshContactDTOList;
        private bool controlsEnabled = true;
        public ContactListView()
        {
            InitializeComponent();

        }


        [Browsable(false)]
        [DefaultValue(false)]
        public List<ContactDTO> ContactDTOList
        {
            get
            {
                return contactDTOList;
            }

            set
            {
                contactDTOList = value;
                flpContactListView.Controls.Clear();
                UpdateContactListUI();
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public List<ContactTypeDTO> ContactTypeDTOList
        {
            get
            {
                return contactTypeDTOList;
            }

            set
            {
                contactTypeDTOList = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(false)]
        public ExecutionContext ExecutionContext
        {
            get
            {
                return executionContext;
            }

            set
            {
                executionContext = value;
            }
        }


        /// <summary>
        ///  This method returns ContactGroupView user control.
        /// </summary>
        /// <param name="type">ContactType</param>
        /// <returns>ContactGroupView</returns>
        private ContactGroupView GetContactView(ContactType type)
        {
            ContactGroupView result = null;
            if (type != ContactType.NONE)
            {
                result = new ContactGroupView();
            }
            else
            {
                throw new Exception("Invalid contact type");
            }
            result.SetContactType(type);
            return result;
        }

        /// <summary>
        /// This method returns the ContactDTOList based on the Contact Type.
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="contactDTOList">contactDTOList</param>
        /// <returns> List<ContactDTO></returns>
        private List<ContactDTO> GetFilteredContactDTOList(ContactType type, List<ContactDTO> contactDTOList)
        {
            log.LogMethodEntry(type, contactDTOList);
            if (contactDTOList == null) return new List<ContactDTO>();
            IEnumerable<ContactDTO> filteredDTOList = chbShowActiveContactEntries.Checked ? contactDTOList.Where(x => x.IsActive) : contactDTOList;
            List<ContactDTO> filteredContactDTOList = filteredDTOList.Where(x => x.ContactType == type).ToList();
            log.LogMethodExit(filteredContactDTOList);
            return filteredContactDTOList;
        }

        /// <summary>
        /// This method returns true if Contact Type is Mandatory else returns False.
        /// </summary>
        /// <param name="type">ContactType</param>
        /// <returns>bool</returns>
        private bool IsContactTypeMandatory(ContactType type)
        {
            string parafaitDefaultValueName = GetParafaitDefaultValueNameForContactType(type);
            return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, parafaitDefaultValueName) == "M";
        }


        /// <summary>
        /// This method returns true if Contact Type is Optional else returns False.
        /// </summary>
        /// <param name="type">ContactType</param>
        /// <returns>bool</returns>
        private bool IsContactTypeOptional(ContactType type)
        {
            string parafaitDefaultValueName = GetParafaitDefaultValueNameForContactType(type);
            if (string.IsNullOrEmpty(parafaitDefaultValueName))
            {
                return false;
            }
            else
            { 
                return ParafaitDefaultContainerList.GetParafaitDefault(executionContext, parafaitDefaultValueName) == "O";
            }
        }


        /// <summary>
        /// This method Gets the Parafait Default ValueName For ContactType.
        /// </summary>
        /// <param name="type">ContactType</param>
        /// <returns>string</returns>
        private static string GetParafaitDefaultValueNameForContactType(ContactType type)
        {
            string parafaitDefaultValueName = string.Empty;
            switch (type)
            {
                case ContactType.PHONE:
                    {
                        parafaitDefaultValueName = "CONTACT_PHONE";
                        break;
                    }
                case ContactType.EMAIL:
                    {
                        parafaitDefaultValueName = "EMAIL";
                        break;
                    }
                case ContactType.FACEBOOK:
                    {
                        parafaitDefaultValueName = "FBUSERID";
                        break;
                    }
                case ContactType.TWITTER:
                    {
                        parafaitDefaultValueName = "TWACCESSTOKEN";
                        break;
                    }
                case ContactType.WECHAT:
                    {
                        parafaitDefaultValueName = "WECHAT_ACCESS_TOKEN";
                        break;
                    }
            }

            return parafaitDefaultValueName;
        }


        /// <summary>
        /// Get/Set for ShowActiveContactEntries
        /// </summary>
        public bool ShowActiveContactEntries
        {
            get
            {
                return chbShowActiveContactEntries.Checked ? true : false;
            }
            set
            {
                showActiveEntry = value;
                if (showActiveEntry) chbShowActiveContactEntries.CheckState = CheckState.Checked;
                else
                    chbShowActiveContactEntries.CheckState = CheckState.Unchecked;
            }

        }

        /// <summary>
        /// This method Updates the ContactListUI. 
        /// </summary>
        public void UpdateContactListUI()
        {
            log.LogMethodEntry();
            flpContactListView.Controls.Clear();
            if (contactDTOList == null)
            {
                return;//throw new Exception("Invalid operation contactDTOList is empty.");
            }
            foreach (ContactTypeDTO contactTypeDTO in contactTypeDTOList)
            {
                List<ContactDTO> filteredContactDTOList = GetFilteredContactDTOList(contactTypeDTO.ContactType, contactDTOList);
                if (filteredContactDTOList == null)
                {
                    filteredContactDTOList = new List<ContactDTO>();
                }
                if (IsContactTypeMandatory(contactTypeDTO.ContactType) && filteredContactDTOList.Any() == false)
                {
                    ContactDTO contactDTO = new ContactDTO() { ContactType = contactTypeDTO.ContactType };
                    filteredContactDTOList.Add(contactDTO);
                    contactDTOList.Add(contactDTO);
                }
                if (IsContactTypeOptional(contactTypeDTO.ContactType) || filteredContactDTOList.Any())
                {
                    ContactGroupView contactView = GetContactView(contactTypeDTO.ContactType);
                    contactView.ShowActiveEntriesOnly = showActiveEntry;
                    contactView.ContactDTOList = contactDTOList;
                    contactView.DeleteContactView += ContactView_OnDelete;
                    contactView.CustomerContactInfoEntered += ContactView_OnContactInfoEntered;
                    contactView.RefreshVirtualKeyBoard += VirtualKeyBoard_OnRefresh;
                    contactView.SetControlsEnabled(controlsEnabled);
                    flpContactListView.Controls.Add(contactView as Control);

                }
            }
            flpContactListView.Refresh();

            log.LogMethodExit();

        }


        /// <summary>
        /// This method removes the ContactDTO on delete button clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContactView_OnDelete(Object sender, ContactDeleteEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ContactDTO.Id < 0)
            {
                // remove DTO with Id =-1 ,
                contactDTOList.Remove(e.ContactDTO);
            }
            else
            {
                ContactDTO contactDTO = contactDTOList.Where(x => x.Id == e.ContactDTO.Id).FirstOrDefault();
                if (contactDTO != null)
                {
                    contactDTO.IsActive = false;
                }
            }
            UpdateContactListUI();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method refreshed the Control list for the Virtual KeyBaord.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void VirtualKeyBoard_OnRefresh(Object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (RefreshVirtualKeyBoard != null)
            {
                RefreshVirtualKeyBoard.Invoke(this, EventArgs.Empty);
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// This method triggers the event for CustomerContactInfoEnteredEventArgs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ContactView_OnContactInfoEntered(Object sender, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry();
            CustomerContactInfoEntered.Invoke(this, new CustomerContactInfoEnteredEventArgs(e.ContactType, e.ContactValue));
            log.LogMethodExit();
        }

        /// <summary>
        /// This method sets the showActiveEntry field value based on the chbShowActiveContactEntries CheckState
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkShowActive_CheckedChanged(object sender, EventArgs e)
        {

            log.LogMethodEntry();
            showActiveEntry = chbShowActiveContactEntries.CheckState == CheckState.Checked ? true : false;

            if (RefreshContactDTOList != null)
            {
                RefreshContactDTOList.Invoke(this, EventArgs.Empty);
            }
            UpdateContactListUI();
            log.LogMethodExit();
        }


        /// <summary>
        /// This method shows the Validation errors.
        /// </summary>
        /// <param name="contactValidationErrors"></param>
        public void ShowValidationErrors(List<ValidationError> contactValidationErrors)
        {
            log.LogMethodEntry(contactValidationErrors);
            foreach (var validationError in contactValidationErrors)
            {

                if (validationError.RecordIndex < 0 ||
                    contactDTOList.Count <= validationError.RecordIndex)
                {
                    continue;
                }
                ContactDTO contactDTO = contactDTOList[validationError.RecordIndex];
                foreach (Control c in flpContactListView.Controls)
                {
                    ContactGroupView contactView = c as ContactGroupView;
                    if (contactView != null)
                    {
                        contactView.ShowValidationError(contactDTO);
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method clears the Validation errors.
        /// </summary>
        internal void ClearValdationErrors()
        {
            foreach (Control c in flpContactListView.Controls)
            {
                ContactGroupView contactView = c as ContactGroupView;
                if (contactView != null)
                {
                    contactView.ClearValdationErrors();
                }
            }
        }

        /// <summary>
        /// This method Updates the ContactDTOList.
        /// </summary>
        public void UpdateContactDTOList()
        {
            foreach (Control c in flpContactListView.Controls)
            {
                ContactGroupView contactView = c as ContactGroupView;
                if (contactView != null)
                {
                    contactView.UpdateContactDTOList();
                }
            }
        }

        internal void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            controlsEnabled = value;
            foreach (Control c in flpContactListView.Controls)
            {
                ContactGroupView contactView = c as ContactGroupView;
                if (contactView != null)
                {
                    contactView.SetControlsEnabled(value);
                }
            }
            log.LogMethodExit();
        }
    }
}
