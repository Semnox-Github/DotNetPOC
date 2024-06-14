/********************************************************************************************
 * Project Name - Utilities
 * Description  - RemoteLanguageUseCases class to get the data  from API by doing remote call  
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
    public class RemoteLanguageUseCases : RemoteUseCases, ILanguageUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string LANGUAGE_URL = "api/SiteSetup/Languages";
        private const string LANGUAGE_CONTAINER_URL = "api/Configuration/LanguageContainer";

        public RemoteLanguageUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<LanguageContainerDTOCollection> GetLanguageContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            LanguageContainerDTOCollection result = await Get<LanguageContainerDTOCollection>(LANGUAGE_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
        public async Task<List<LanguagesDTO>> GetLanguages(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<LanguagesDTO> result = await Get<List<LanguagesDTO>>(LANGUAGE_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<LanguagesDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case LanguagesDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case LanguagesDTO.SearchByParameters.LANGUAGE_CODE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("LanguageCode".ToString(), searchParameter.Value));
                        }
                        break;
                    case LanguagesDTO.SearchByParameters.LANGUAGE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("LanguageId".ToString(), searchParameter.Value));
                        }
                        break;
                    case LanguagesDTO.SearchByParameters.LANGUAGE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("LanguageName".ToString(), searchParameter.Value));
                        }
                        break;
                    case LanguagesDTO.SearchByParameters.READER_LANGUAGE_NO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ReaderLanguageNo".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveLanguage(List<LanguagesDTO> languagesDTO)
        {
            log.LogMethodEntry(languagesDTO);
            try
            {
                string responseString = await Post<string>(LANGUAGE_URL, languagesDTO);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
