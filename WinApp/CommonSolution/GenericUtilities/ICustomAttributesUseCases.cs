/********************************************************************************************
 * Project Name -CustomAttributes
 * Description  -ICustomAttributesUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
 2.130.0         27-Jul-2021       Mushahid Faizan    Modified :- POS UI redesign changes.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
   public interface ICustomAttributesUseCases
    {
        Task<List<CustomAttributesDTO>> GetCustomAttributes(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters,
            bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SaveCustomAttributes(List<CustomAttributesDTO> customAttributesDTOList);

        Task<CustomAttributeContainerDTOCollection> GetCustomAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
