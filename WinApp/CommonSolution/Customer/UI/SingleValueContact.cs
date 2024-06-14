/********************************************************************************************
 * Project Name - SingleValueContact
 * Description  - User Control for view Single Value Contact like Phone, Email.WeChat
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       01-Sept-2019      Girish kundar    Created .                                                  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Customer
{
    public partial class SingleValueContact : UserControl, IContactInfo
    {
        private readonly ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string txtContactValue = string.Empty;
        private ContactDTO contactDTO = new ContactDTO();  
        [field: NonSerialized]
        public event EventHandler<ContactDeleteEventArgs> DeleteContactView;
        public event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public SingleValueContact(ContactType type)
        {
            log.LogMethodEntry();
            InitializeComponent();
            if(type == ContactType.PHONE && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY_CODE") != "N")
            {
                cmbCountryCode.Visible = true;
            }
            LoadCountryCode();
            log.LogMethodExit();
        }

        private void LoadCountryCode()
        {
            log.LogMethodEntry();
            List<CountryContainerDTO> countryDTOList = CountryContainerList.GetCountryContainerDTOList(executionContext.GetSiteId());
            if (countryDTOList != null && countryDTOList.Any())
            {
                List<CountryContainerDTO>  activeCountryDTOList = countryDTOList.FindAll(x => x.IsActive == true).ToList();
                //activeCountryDTOList.Insert(0, new CountryContainerDTO());
                activeCountryDTOList[0].CountryCode = "";
                activeCountryDTOList[0].CountryId = -1;
                List<CountryContainerDTO> countryList = activeCountryDTOList.Where(c => !string.IsNullOrEmpty(c.CountryCode)).Select(c => { c.CountryCode = c.CountryCode; return c; }).ToList();
                cmbCountryCode.DataSource = countryList;
                cmbCountryCode.DisplayMember = "CountryCode";
                cmbCountryCode.ValueMember = "CountryId";
                cmbCountryCode.SelectedValue = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE_LOOKUP_FOR_COUNTRY", -1);
            }
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
                UpdateContactValue();
            }
        }
        public string ContactValue
        {
            get
            {
                return txtContact.Text.ToString();
            }

        }

        /// <summary>
        /// This methods sets the contact value to the textBox.
        /// </summary>
        public void UpdateContactValue()
        {
            log.LogMethodEntry();
            txtContact.Text = string.IsNullOrEmpty(contactDTO.Attribute1) ? string.Empty : contactDTO.Attribute1;
            if(contactDTO.CountryId != -1)
                cmbCountryCode.SelectedValue = contactDTO.CountryId;
            log.LogMethodExit();
        }

        /// <summary>
        /// This methods invokes the ContactDeleteEventArgs event.
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
        /// Shows the Validation Error 
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

        public void ClearValdationError()
        {
            log.LogMethodEntry();
            BackColor = Color.White;
            log.LogMethodExit();
        }

        /// <summary>
        /// This methods Updates the current ContactDTO.
        /// </summary>
        public void UpdateContactDTO()
        {
            log.LogMethodEntry();
            if (contactDTO.Attribute1 != txtContact.Text)
            {
                contactDTO.Attribute1 = txtContact.Text;
                if (contactDTO.ContactType == ContactType.PHONE && cmbCountryCode.SelectedValue != null)
                {
                    contactDTO.CountryId = Convert.ToInt32(cmbCountryCode.SelectedValue);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This event fires when textBox looses focus and triggers CustomerContactInfoEnteredEventArgs event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContact_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (contactDTO.Attribute1 != txtContact.Text.ToString())
            {
                if (CustomerContactInfoEntered != null && string.IsNullOrWhiteSpace(txtContact.Text) == false)
                {
                    CustomerContactInfoEntered.Invoke(this, new CustomerContactInfoEnteredEventArgs(contactDTO.ContactType, txtContact.Text.ToString()));
                }
            }
            contactDTO.Attribute1 = txtContact.Text.ToString();
            log.LogMethodExit();
        }

        public void TextBoxFocus()
        {
            log.LogMethodEntry();
            txtContact.Focus();
            log.LogMethodExit();
        }

        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            txtContact.ReadOnly = !value;
            btnDelete.Visible = value;
            log.LogMethodExit();
        }

        private void cmbCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (contactDTO.ContactType == ContactType.PHONE && cmbCountryCode.SelectedValue != null)
            {
                contactDTO.CountryId = Convert.ToInt32(cmbCountryCode.SelectedValue);
            }
            log.LogMethodExit();
        }
    }
}

