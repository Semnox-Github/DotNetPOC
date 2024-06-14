/********************************************************************************************
* Project Name - Utilities
* Description  - Specification of the Department use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   25-Feb-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IDepartmentUseCases
    {
        Task<List<DepartmentDTO>> GetDepartments(List<KeyValuePair<DepartmentDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveDepartments(List<DepartmentDTO> departmentDTOList);
        //Task<DepartmentContainerDTOCollection> GetDepartmentContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> Delete(List<DepartmentDTO> departmentDTOList);
    }
       
}
