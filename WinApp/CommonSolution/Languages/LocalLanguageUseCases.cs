/********************************************************************************************
 * Project Name - Utilities 
 * Description  - LocalLanguageDataService class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.120.0      06-May-2021      B Mahesh Pai              Modified
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Languages
{
    public class LocalLanguageUseCases : LocalUseCases, ILanguageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LocalLanguageUseCases(ExecutionContext executionContext) 
            :base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<LanguageContainerDTOCollection> GetLanguageContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<LanguageContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hash, rebuildCache);
                if (rebuildCache)
                {
                    LanguageContainerList.Rebuild(siteId);
                }
                LanguageContainerDTOCollection result = LanguageContainerList.GetLanguageContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<List<LanguagesDTO>> GetLanguages(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<LanguagesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                Languages languages = new Languages(executionContext);
                List<LanguagesDTO> languagesDTOList = languages.GetAllLanguagesList(searchParameters);

                log.LogMethodExit(languagesDTOList);
                return languagesDTOList;
            });
        }
        public async Task<string> SaveLanguage(List<LanguagesDTO> languagesDTO)
    
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(languagesDTO);
                    if (languagesDTO == null)
                    {
                        throw new ValidationException("languagesDTO is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            LanguagesList languagesList = new LanguagesList(executionContext, languagesDTO);
                            languagesList.SaveUpdateLanguages();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                    throw ex;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
