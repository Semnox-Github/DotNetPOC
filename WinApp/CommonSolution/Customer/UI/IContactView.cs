using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// interface for custom data control
    /// </summary>
   public  interface IContactView
    {
        /// <summary>
        /// Get / Set property for Contact Custom controls
        /// </summary>
        List<ContactDTO> ContactDTOList
        {
            get;
            set;
        }

        bool ShowActiveEntriesOnly
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
        /// Sets the Contact Type for Contact View Control
        /// </summary>
        void SetContactType(ContactType contactType);
        void ShowValidationError(ContactDTO contactDTO);
        void ClearValdationErrors();
        void UpdateContactDTOList();
    }
}
