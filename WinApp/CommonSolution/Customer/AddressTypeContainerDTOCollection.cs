/********************************************************************************************
* Project Name - Customer
* Description  - AddressTypeContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
    public class AddressTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<AddressTypeContainerDTO> addressTypeContainerDTOList;
        private string hash;

        public AddressTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            addressTypeContainerDTOList = new List<AddressTypeContainerDTO>();
            log.LogMethodExit();
        }
        public AddressTypeContainerDTOCollection(List<AddressTypeContainerDTO> addressTypeContainerDTOList)
        {
            log.LogMethodEntry(addressTypeContainerDTOList);
            this.addressTypeContainerDTOList = addressTypeContainerDTOList;
            if (addressTypeContainerDTOList == null)
            {
                addressTypeContainerDTOList = new List<AddressTypeContainerDTO>();
            }
            hash = new DtoListHash(addressTypeContainerDTOList);
            log.LogMethodExit();
        }

        public List<AddressTypeContainerDTO> AddressTypeContainerDTOList
        {
            get
            {
                return addressTypeContainerDTOList;
            }

            set
            {
                addressTypeContainerDTOList = value;
            }
        }

        public string Hash
        {
            get
            {
                return hash;
            }
            set
            {
                hash = value;
            }
        }
    }
}
