/********************************************************************************************
* Project Name - JobTask
* Description  - IJobTaskUseCases class
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
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public interface IJobTaskUseCases
    {
        Task<List<JobTaskDTO>> GetJobTasks(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveJobTasks(List<JobTaskDTO>  jobTaskDTOList);
    }
}
