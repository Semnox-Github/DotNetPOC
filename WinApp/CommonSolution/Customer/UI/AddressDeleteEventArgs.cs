/********************************************************************************************
 * Project Name - AddressDeleteEventArgs
 * Description  -Class for  Handling Delete Event for the Address Controls.
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       15-Oct-2019       Girish kundar    Created .                                                  
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class AddressDeleteEventArgs : EventArgs
    {
        private AddressDTO addressDTO;
        public AddressDeleteEventArgs(AddressDTO addressDTO)
        {
            if (addressDTO == null)
            {
                throw new Exception("");
            }
            this.addressDTO = addressDTO;
        }
        public AddressDTO AddressDTO
        {
            get
            {
                return addressDTO;
            }
        }
    }
}
