/********************************************************************************************
 * Project Name -ApplicationContent
 * Description  -IApplicationContentUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
*2.130.0     21-Jul-2021       Mushahid Faizan     Modified : POS UI Redesign changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
   public interface IApplicationContentUseCases
    {
        Task<List<ApplicationContentDTO>> GetApplicationContents(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null);
        Task<string> SaveApplicationContents(List<ApplicationContentDTO> applicationContentDTOList);

        Task<ApplicationContentContainerDTOCollection> GetApplicationContentContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache);
    }
}
