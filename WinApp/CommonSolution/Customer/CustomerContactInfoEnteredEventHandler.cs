
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// CustomerIdentificationInfoEnteredEventHandler event arguments definition
    /// </summary>
    public class CustomerContactInfoEnteredEventArgs : EventArgs
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ContactType contactType;
        private string contactValue;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="contactType">contact types</param>
        /// <param name="contactValue">contact value</param>
        public CustomerContactInfoEnteredEventArgs(ContactType contactType, string contactValue)
        {
            log.LogMethodEntry(contactType, contactValue);
            this.contactType = contactType;
            this.contactValue = contactValue;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Method of contact type field
        /// </summary>
        public ContactType ContactType
        {
            get
            {
                return contactType;
            }
        }

        /// <summary>
        /// Get method of contactValue field
        /// </summary>
        public string ContactValue
        {
            get
            {
                return contactValue;
            }
        }
    }
    /// <summary>
    /// CustomerIdentificationInfoEnteredEventHandler delegate definition
    /// </summary>
    public delegate void CustomerContactInfoEnteredEventHandler(object source, CustomerContactInfoEnteredEventArgs e);
}
