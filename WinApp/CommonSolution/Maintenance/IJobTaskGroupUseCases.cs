/********************************************************************************************
* Project Name - JobTaskGroup
* Description  - IJobTaskGroupUseCases class
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   22-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
   public interface IJobTaskGroupUseCases
    {
        Task<List<JobTaskGroupDTO>> GetJobTaskGroups(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParameters);
        Task<string> SaveJobTaskGroups(List<JobTaskGroupDTO> jobTaskGroupDTOList);
    }
}
