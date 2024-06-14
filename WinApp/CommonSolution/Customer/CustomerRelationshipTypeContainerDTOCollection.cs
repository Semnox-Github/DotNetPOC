/********************************************************************************************
* Project Name - Customer
* Description  - CustomerRelationshipTypeContainerDTOCollection class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    31-Aug-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    public class CustomerRelationshipTypeContainerDTOCollection
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList;
        private string hash;

        public CustomerRelationshipTypeContainerDTOCollection()
        {
            log.LogMethodEntry();
            customerRelationshipTypeContainerDTOList = new List<CustomerRelationshipTypeContainerDTO>();
            log.LogMethodExit();
        }
        public CustomerRelationshipTypeContainerDTOCollection(List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList)
        {
            log.LogMethodEntry(customerRelationshipTypeContainerDTOList);
            this.customerRelationshipTypeContainerDTOList = customerRelationshipTypeContainerDTOList;
            if (customerRelationshipTypeContainerDTOList == null)
            {
                customerRelationshipTypeContainerDTOList = new List<CustomerRelationshipTypeContainerDTO>();
            }
            hash = new DtoListHash(GetDTOList(customerRelationshipTypeContainerDTOList));
            log.LogMethodExit();
        }

        private IEnumerable<object> GetDTOList(object customerRelationshipTypeContainerDTOList)
        {
            throw new NotImplementedException();
        }
        private IEnumerable<object> GetDTOList(List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList)
        {
            log.LogMethodEntry(customerRelationshipTypeContainerDTOList);
            foreach (CustomerRelationshipTypeContainerDTO customerRelationshipTypeContainerDTO in customerRelationshipTypeContainerDTOList.OrderBy(x => x.CustomerRelationshipTypeId))
            {
                yield return customerRelationshipTypeContainerDTO;
            }
            log.LogMethodExit();
        }
        public List<CustomerRelationshipTypeContainerDTO> CustomerRelationshipTypeContainerDTOList
        {
            get
            {
                return customerRelationshipTypeContainerDTOList;
            }

            set
            {
                customerRelationshipTypeContainerDTOList = value;
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
