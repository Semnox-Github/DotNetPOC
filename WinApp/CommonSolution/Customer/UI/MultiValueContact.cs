/********************************************************************************************
 * Project Name - MultiValueContact
 * Description  - User Control for view Multi Value Contact like Facebook, Twitter
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2       01-Sept-2019      Girish kundar    Created .                                                  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Class for MultiValueContact user control.
    /// </summary>
    public partial class MultiValueContact : UserControl, IContactInfo
    {
        private readonly ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContactDTO contactDTO = new ContactDTO();
        [field: NonSerialized]
        public event EventHandler<ContactDeleteEventArgs> DeleteContactView;
        public event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;

        /// <summary>
        /// Default Constructor for MultiValueContact.
        /// </summary>
        public MultiValueContact()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }



        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public ContactDTO ContactDTO
        {
            get
            {
                return contactDTO;
            }

            set
            {
                contactDTO = value;
                UpdateMultiValueContactUI();
            }
        }
        public string Username
        {
            get
            {
                return txtUsername.Text.ToString();
            }

        }
        public string Password
        {
            get
            {
                return txtPassword.Text.ToString();
            }

        }

        /// <summary>
        /// This method sets the Contact details to the textBox.
        /// </summary>
        public void UpdateMultiValueContactUI()
        {
            log.LogMethodEntry();
            txtUsername.Text = string.IsNullOrEmpty(contactDTO.Attribute1) ? string.Empty : contactDTO.Attribute1;
            txtPassword.Text = string.IsNullOrEmpty(contactDTO.Attribute2) ? string.Empty : contactDTO.Attribute2;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method shows the Validation error.
        /// </summary>
        /// <param name="contactDTO">contactDTO</param>
        public void ShowValidationError(ContactDTO contactDTO)
        {
            log.LogMethodEntry();
            if (this.contactDTO == contactDTO)
            {
                BackColor = Color.OrangeRed;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method invokes the ContactDeleteEventArgs event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (DeleteContactView != null)
            {
                DeleteContactView.Invoke(this, new ContactDeleteEventArgs(this.ContactDTO));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method Clears the Validation Error.
        /// </summary>
        public void ClearValdationError()
        {
            log.LogMethodEntry();
            BackColor = Color.White;
            log.LogMethodExit();
        }
        /// <summary>
        /// This method updates the Current ContactDTO.
        /// </summary>
        public void UpdateContactDTO()
        {
            log.LogMethodEntry();
            if (contactDTO.Attribute1 != txtUsername.Text)
            {
                contactDTO.Attribute1 = txtUsername.Text;
            }
            if (contactDTO.Attribute2 != txtPassword.Text)
            {
                contactDTO.Attribute2 = txtPassword.Text;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Tis method invokes the CustomerContactInfoEnteredEventArgs event .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (contactDTO.Attribute1 != txtUsername.Text.ToString())
            {
                if (CustomerContactInfoEntered != null && string.IsNullOrWhiteSpace(txtUsername.Text) == false)
                {
                    CustomerContactInfoEntered.Invoke(this, new CustomerContactInfoEnteredEventArgs(contactDTO.ContactType, txtUsername.Text.ToString()));
                }
            }
            contactDTO.Attribute1 = txtUsername.Text.ToString();
            contactDTO.Attribute2 = txtPassword.Text.ToString();
            log.LogMethodExit();
        }

        public void TextBoxFocus()
        {
            log.LogMethodEntry();
            txtUsername.Focus();
            log.LogMethodExit();
        }

        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            txtUsername.ReadOnly = !value;
            txtPassword.ReadOnly = !value;
            btnDelete.Visible = value;
            log.LogMethodExit();
        }
    }
}
