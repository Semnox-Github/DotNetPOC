/********************************************************************************************
 * Project Name - Customer
 * Description  - ICustomerRelationshipTypeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version        Date             Modified By        Remarks          
 *********************************************************************************************
 2.130.0         31-Aug-2021      Mushahid Faizan    Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Semnox.Parafait.Customer
{
    public interface ICustomerRelationshipTypeUseCases
    {
        Task<List<CustomerRelationshipTypeDTO>> GetCustomerRelationshipType(List<KeyValuePair<CustomerRelationshipTypeDTO.SearchByParameters, string>> searchParameters);
        Task<CustomerRelationshipTypeContainerDTOCollection> GetCustomerRelationshipTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
