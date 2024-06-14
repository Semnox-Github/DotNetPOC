/********************************************************************************************
* Project Name - ProgramParameterValue
* Description  - IProgramParameterValueUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.1    18-May-2021   B Mahesh Pai             Created 
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
   public  interface IProgramParameterValueUseCases
    {
        Task<List<ProgramParameterValueDTO>> GetProgramParameterValues(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters,
                                       bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);

        Task<string> SaveProgramParameterValues(List<ProgramParameterValueDTO> programParameterValueDTOList);
        Task<string> Delete(List<ProgramParameterValueDTO> programParameterValueDTOList);

    }
}
