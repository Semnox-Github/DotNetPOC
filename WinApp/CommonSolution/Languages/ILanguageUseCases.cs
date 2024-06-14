/********************************************************************************************
 * Project Name - Utilities  
 * Description  - ILanguageUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.120.0       06-May-2021     B Mahesh Pai             Modified
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Languages
{
    public interface ILanguageUseCases
    {
        Task<LanguageContainerDTOCollection> GetLanguageContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<List<LanguagesDTO>> GetLanguages(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveLanguage(List<LanguagesDTO> languagesDTO);
    }
}
