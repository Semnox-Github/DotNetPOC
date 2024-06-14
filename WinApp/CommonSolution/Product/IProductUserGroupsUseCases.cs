/********************************************************************************************
 * Project Name - Product
 * Description  - IProductUserGroupsUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00     17-Nov-2020         Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IProductUserGroupsUseCases
    {
        Task<List<ProductUserGroupsDTO>> GetProductUserGroups(List<KeyValuePair<ProductUserGroupsDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false);
        Task<string> SaveProductUserGroups(List<ProductUserGroupsDTO> productUserGroupsDTOList);
    }
}
