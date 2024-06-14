/********************************************************************************************
* Project Name - User
* Description  - IEntityOverrideDatesUseCases class
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Apr-2021      B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public interface IEntityOverrideDatesUseCases
    {
        Task<List<EntityOverrideDatesDTO>> GetEntityOverrideDates(List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveEntityOverrideDates(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList);
        Task<string> Delete(List<EntityOverrideDatesDTO> entityOverrideDatesDTOList);
    }
}
