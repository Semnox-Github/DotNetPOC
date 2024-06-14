/********************************************************************************************
 * Project Name - IContactInfo
 * Description  - Interface for Contact Controls.
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    interface IContactInfo
    {
        /// <summary>
        /// Get / Set property for Contact Custom controls
        /// </summary>
        ContactDTO ContactDTO
        {
            get;
            set;
        }
        /// <summary>
        /// Generates the Delete event for the controls
        /// </summary>
        [field: NonSerialized]
        event EventHandler<ContactDeleteEventArgs> DeleteContactView;

        /// <summary>
        /// Generates the  event when user enters the contact details.
        /// </summary>
        event CustomerContactInfoEnteredEventHandler CustomerContactInfoEntered;

        /// <summary>
        /// Sets the Contact Type for Contact View Control
        /// </summary>
        void ShowValidationError(ContactDTO contactDTO);
        void ClearValdationError();
        void UpdateContactDTO();
        void TextBoxFocus();
        void SetControlsEnabled(bool value);
    }
}
