/********************************************************************************************
 * Project Name - DefaultDataType
 * Description  - IDefaultDataTypeUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
   public interface IDefaultDataTypeUseCases
    {
        Task<List<DefaultDataTypeDTO>> GetDefaultDataTypes(List<KeyValuePair<DefaultDataTypeDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveDefaultDataTypes(List<DefaultDataTypeDTO> defaultDataTypeDTO);
    }
}
