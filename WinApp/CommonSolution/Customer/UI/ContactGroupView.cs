/********************************************************************************************
 * Project Name - ContactGroupView
 * Description  - User Control for ContactGroupView
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       01-Sept-2019      Girish kundar    Created .                                                  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Drawing.Drawing2D;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is class for ContactGroupView User control which displays the contact details.
    /// </summary>
    public partial class ContactGroupView : UserControl
    {
        private readonly ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContactType contactType;
        private bool showActiveEntriesOnly;
        private List<ContactDTO> contactDTOList;
        [field: NonSerialized]
        public event EventHandler<ContactDeleteEventArgs> DeleteContactView;
        [field: NonSerialized]
        public event EventHandler RefreshVirtualKeyBoard;
        private VirtualKeyboardController virtualKeyboardController;
        /// <summary>
        /// Event generated when customer contact information is entered
        /// </summary>
        public event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;

        /// <summary>
        /// Default Constructor for ContactGroupView.
        /// </summary>
        public ContactGroupView()
        {
            log.LogMethodEntry();
            InitializeComponent();
            virtualKeyboardController = new VirtualKeyboardController();
            RecreateRegion();
            log.LogMethodExit();
        }

        public bool ShowActiveEntriesOnly
        {
            get
            {
                return showActiveEntriesOnly;
            }
            set
            {
                showActiveEntriesOnly = value;
            }
        }

        [Browsable(true)]
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
                UpdateContactGroupUI();

            }
        }


        /// <summary>
        /// Sets the Contact Type for the ContactGroupView user control.
        /// </summary>
        /// <param name="type"></param>
        public void SetContactType(ContactType type)
        {
            log.LogMethodEntry(type);
            contactType = type;
            lblContactType.Text = type.ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method returns the IContactInfo object based on the ContactType
        /// </summary>
        /// <param name="type">ContactType</param>
        /// <returns>IContactInfo</returns>
        private IContactInfo GetContactView(ContactType type)
        {
            log.LogMethodEntry(type);
            IContactInfo result = null;
            if (type == ContactType.PHONE || type == ContactType.EMAIL || type == ContactType.WECHAT)
            {
                result = new SingleValueContact(type);
            }
            else if (type == ContactType.TWITTER || type == ContactType.FACEBOOK)
            {
                result = new MultiValueContact();
            }
            else
            {
                throw new Exception("Invalid contact type");
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This method Updates the ContactGroupView display.
        /// </summary>
        public void UpdateContactGroupUI()
        {
            log.LogMethodEntry();
            var contactViewList = flpContactGroup.Controls.OfType<IContactInfo>().ToList();
            foreach (Control contactView in contactViewList)
            {
                flpContactGroup.Controls.Remove(contactView);
            }
            flpContactGroup.Refresh();
            var filteredContactDTOList = contactDTOList.Where(x => x.ContactType == contactType && (showActiveEntriesOnly == false || x.IsActive));
            foreach (ContactDTO contactDTO in filteredContactDTOList)
            {
                // Based on type create contact view
                IContactInfo contactInfo = GetContactView(contactType);
                contactInfo.ContactDTO = contactDTO;
                contactInfo.DeleteContactView += ContactView_OnDelete;
                contactInfo.CustomerContactInfoEntered += CustomerContactInfo_OnEnter;
                flpContactGroup.Controls.Add(contactInfo as Control);
                flpContactGroup.Refresh();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is invoked when contact delete button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactView_OnDelete(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (DeleteContactView != null)
            {
                IContactInfo contactInfo = (IContactInfo)sender as IContactInfo;
                DeleteContactView.Invoke(this, new ContactDeleteEventArgs(contactInfo.ContactDTO));
            }
            if (RefreshVirtualKeyBoard != null)
            {
                RefreshVirtualKeyBoard.Invoke(this, EventArgs.Empty);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method invokes the CustomerContactInfoEnteredEventArgs event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerContactInfo_OnEnter(object sender, CustomerContactInfoEnteredEventArgs e)
        {
            log.LogMethodEntry();
            if (CustomerContactInfoEntered != null)
            {
                CustomerContactInfoEntered.Invoke(this, new CustomerContactInfoEnteredEventArgs(e.ContactType, e.ContactValue));
            }
            log.LogMethodExit();
        }


        private int radius = 10;
        public int Radius
        {
            get { return radius; }
            set
            {
                radius = value;
                this.RecreateRegion();
            }
        }

        /// <summary>
        /// This method is used to Draw curved edges for the rectangle view .
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        /// <returns>GraphicsPath</returns>
        private GraphicsPath GetRoundRectangle(Rectangle bounds, int radius)
        {
            log.LogMethodEntry();
            GraphicsPath path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, radius, radius, 180, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y, radius, radius, 270, 90);
            path.AddArc(bounds.X + bounds.Width - radius, bounds.Y + bounds.Height - radius,
                        radius, radius, 0, 90);
            path.AddArc(bounds.X, bounds.Y + bounds.Height - radius, radius, radius, 90, 90);
            path.CloseAllFigures();
            log.LogMethodExit(path);
            return path;
        }

        /// <summary>
        /// This method re-creates the Region after edit. 
        /// </summary>
        private void RecreateRegion()
        {
            log.LogMethodEntry();
            var bounds = ClientRectangle;
            bounds.Width--; bounds.Height--;
            using (var path = GetRoundRectangle(bounds, this.Radius))
                this.Region = new Region(path);
            this.Invalidate();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method is invokes when user clicks on the Add(Plus) Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Create new Contact DTO for new Number   // Based on type create contact view
            log.LogMethodEntry();
            ContactDTO newContactDTO = new ContactDTO();
            newContactDTO.ContactType = contactType;
            IContactInfo contactInfo = GetContactView(contactType);
            contactInfo.ContactDTO = newContactDTO;
            contactInfo.DeleteContactView += ContactView_OnDelete;
            flpContactGroup.Controls.Add(contactInfo as Control);
            contactDTOList.Add(newContactDTO);
            log.LogMethodExit();
        }

        /// <summary>
        /// This method shows the Validation error in the Contact Group View .
        /// </summary>
        /// <param name="contactDTO">contactDTO</param>
        public void ShowValidationError(ContactDTO contactDTO)
        {
            log.LogMethodEntry("contactDTO");
            foreach (Control c in flpContactGroup.Controls)
            {
                IContactInfo contactInfo = c as IContactInfo;
                if (contactInfo != null)
                {
                    contactInfo.ShowValidationError(contactDTO);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method clears the Validation errors.
        /// </summary>
        public void ClearValdationErrors()
        {
            log.LogMethodEntry();
            foreach (Control c in flpContactGroup.Controls)
            {
                IContactInfo contactInfo = c as IContactInfo;
                if (contactInfo != null)
                {
                    contactInfo.ClearValdationError();
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method Update the ContactDTOList.
        /// </summary>
        public void UpdateContactDTOList()
        {
            log.LogMethodEntry();
            foreach (Control c in flpContactGroup.Controls)
            {
                IContactInfo contactInfo = c as IContactInfo;
                if (contactInfo != null)
                {
                    contactInfo.UpdateContactDTO();
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// This method adjust the Height of User control based on the Height of flpContactGroup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flpContactGroup_SizeChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Height = flpContactGroup.Height + 2;
            RecreateRegion();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method adjust the Height of User control based on the Height of flpContactGroup on load .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContactGroupView_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Height = flpContactGroup.Height + 5;
            RecreateRegion();
            log.LogMethodExit();
        }

        /// <summary>
        /// This method Refreshes the Virtual keyBoard Control List.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flpContactGroup_ControlAdded(object sender, ControlEventArgs e)
        {
            log.LogMethodEntry();
            // keyBoard to be refreshed.
            if (RefreshVirtualKeyBoard != null)
            {
                RefreshVirtualKeyBoard.Invoke(this, EventArgs.Empty);
            }
            IContactInfo contactInfo = e.Control as IContactInfo;
            contactInfo.TextBoxFocus();
            log.LogMethodExit();
        }

        internal void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            btnAdd.Visible = value;
            foreach (Control c in flpContactGroup.Controls)
            {
                IContactInfo contactInfo = c as IContactInfo;
                if (contactInfo != null)
                {
                    contactInfo.SetControlsEnabled(value);
                }
            }
            log.LogMethodExit();
        }
    }
}
