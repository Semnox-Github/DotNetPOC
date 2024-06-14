/********************************************************************************************
* Project Name - Customer
* Description  - CustomerUIMetadataContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    09-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
   public class CustomerUIMetadataContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTOList;
        private string hash;

        public CustomerUIMetadataContainerDTOCollection()
        {
            log.LogMethodEntry();
            customerUIMetadataContainerDTOList = new List<CustomerUIMetadataContainerDTO>();
            log.LogMethodExit();
        }
        public CustomerUIMetadataContainerDTOCollection(List<CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTOList)
        {
            log.LogMethodEntry(customerUIMetadataContainerDTOList);
            this.customerUIMetadataContainerDTOList = customerUIMetadataContainerDTOList;
            if (customerUIMetadataContainerDTOList == null)
            {
                customerUIMetadataContainerDTOList = new List<CustomerUIMetadataContainerDTO>();
            }
            hash = new DtoListHash(customerUIMetadataContainerDTOList);
            log.LogMethodExit();
        }

        public List<CustomerUIMetadataContainerDTO> CustomerUIMetadataContainerDTOList
        {
            get
            {
                return customerUIMetadataContainerDTOList;
            }

            set
            {
                customerUIMetadataContainerDTOList = value;
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

