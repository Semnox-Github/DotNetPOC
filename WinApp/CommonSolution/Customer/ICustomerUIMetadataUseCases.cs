/********************************************************************************************
* Project Name - Customer
* Description  - Specification of the ICustomerUIMetadataUseCases use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   09-Jul-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Customer
{
   public  interface ICustomerUIMetadataUseCases
    {
       Task<List<CustomerUIMetadataDTO>> GetCustomerUIMetadatas(int siteId);
       Task<CustomerUIMetadataContainerDTOCollection> GetCustomerUIMetadataContainerDTOCollection(int siteId, string hash, bool rebuildCache);


    }
}
