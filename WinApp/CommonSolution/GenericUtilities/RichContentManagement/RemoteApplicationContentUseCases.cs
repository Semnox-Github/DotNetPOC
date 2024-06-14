/********************************************************************************************
 * Project Name -ApplicationContent
 * Description  -RemoteApplicationContentUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         12-May-2021       B Mahesh Pai       Created
 2.130.0         20-Jul-2021       Mushahid Faizan    Modified - POS UI redesign changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteApplicationContentUseCases: RemoteUseCases, IApplicationContentUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string APPLICATIONCONTENT_URL = "api/Common/ApplicationContents";
        private const string APPLICATIONCONTENT_CONTAINER_URL = "api/Common/ApplicationContentContainer";
        public RemoteApplicationContentUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ApplicationContentDTO>> GetApplicationContents(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveRecords, sqlTransaction);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveRecords".ToString(), loadActiveRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ApplicationContentDTO> result = await Get<List<ApplicationContentDTO>>(APPLICATIONCONTENT_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ApplicationContentDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ApplicationContentDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ApplicationContentDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Active".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationContentDTO.SearchByParameters.APPLICATION:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Description".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationContentDTO.SearchByParameters.APP_CONTENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MOduleID".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationContentDTO.SearchByParameters.CHAPTER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Active".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationContentDTO.SearchByParameters.CONTENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("Description".ToString(), searchParameter.Value));
                        }
                        break;
                    case ApplicationContentDTO.SearchByParameters.MODULE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("MOduleID".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveApplicationContents(List<ApplicationContentDTO> applicationContentDTOList)
        {
            log.LogMethodEntry(applicationContentDTOList);
            try
            {
                string responseString = await Post<string>(APPLICATIONCONTENT_URL, applicationContentDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }


        public async Task<ApplicationContentContainerDTOCollection> GetApplicationContentContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("languageId", languageId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            ApplicationContentContainerDTOCollection result = await Get<ApplicationContentContainerDTOCollection>(APPLICATIONCONTENT_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}

    
