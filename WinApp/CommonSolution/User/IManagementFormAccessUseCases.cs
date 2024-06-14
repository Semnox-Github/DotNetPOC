/********************************************************************************************
* Project Name - User
* Description  - Interface for ManagementFormAccess Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IManagementFormAccessUseCases
    {
        Task<List<ManagementFormAccessDTO>> GetManagementFormAccessDTOList(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        // Task<int> GetManagementFormAccessCount(List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<ManagementFormAccessDTO>> SaveManagementFormAccess(List<ManagementFormAccessDTO> ManagementFormAccessDTOList);

    }
}
