/********************************************************************************************
 * Project Name - Lookups
 * Description  - ILookupsUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         06-May-2021       B Mahesh Pai       Modified
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public interface ILookupsUseCases
    {
        Task<LookupsContainerDTOCollection> GetLookupsContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<List<LookupsDTO>> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> searchParameters, bool loadChild = false,
                                              bool loadActiveChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SaveLookups(List<LookupsDTO> lookupsDTOList);
    }
}
