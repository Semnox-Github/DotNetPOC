/********************************************************************************************
* Project Name - JobUtils
* Description  - Specification of the ConcurrentProgramsUseCases . 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   27-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// IConcurrentProgramsUseCases
    /// </summary>
    public interface IConcurrentProgramsUseCases
    {
        Task<List<ConcurrentProgramsDTO>> GetConcurrentPrograms(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null);
        Task<string> SaveConcurrentPrograms(List<ConcurrentProgramsDTO> concurrentProgramsDTOList);
    }
}
