/********************************************************************************************
 * Project Name - Inventory
 * Description  - IVendorUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Vendor
{
    public interface IVendorUseCases
    {
        Task<List<VendorDTO>> GetVendors(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
             searchParameters, int currentPage = 0, int pageSize = 0);
        Task<string> SaveVendors(List<VendorDTO> locationDTOList);
        Task<int> GetVendorCount(List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>
             searchParameters);
    }
}
