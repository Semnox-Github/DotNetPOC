/********************************************************************************************
 * Project Name - ContactDeleteEventArgs
 * Description  -Class for  Handling Delete Event for the Contact Controls.
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
    public class ContactDeleteEventArgs : EventArgs
    {
        private ContactDTO contactDTO;
        public ContactDeleteEventArgs(ContactDTO contactDTO)
        {
            if(contactDTO == null)
            {
                throw new Exception("");
            }
            this.contactDTO = contactDTO;
        }
        public ContactDTO ContactDTO
        {
            get
            {
                return contactDTO;
            }
        }
    }
}
