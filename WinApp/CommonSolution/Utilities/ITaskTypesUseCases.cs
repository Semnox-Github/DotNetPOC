/********************************************************************************************
* Project Name - Utilities
* Description  - Specification of the TaskTypes use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   02-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public interface ITaskTypesUseCases
    {
        Task<List<TaskTypesDTO>> GetTaskTypes(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>> parameters);
        Task<string> SaveTaskTypes(List<TaskTypesDTO> taskTypeDTOList);
        Task<TaskTypesContainerDTOCollection> GetTaskTypesContainerDTOCollection(int siteId, string hash, bool rebuildCache);

    }
}
