/********************************************************************************************
 * Project Name - RemoteParafaitMessageQueueUseCases
 * Description  - RemoteParafaitMessageQueueUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
*2.120.0      15-Mar-2021       Prajwal S          Created :urban Pipers changes
*2.130.0      08-Feb-2022       Fiona Lishal        Added GetParafaitMessageQueueDTOList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// RemoteParafaitMessageQueueUseCases
    /// </summary>
    public class RemoteParafaitMessageQueueUseCases : RemoteUseCases, IParafaitMessageQueueUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ParafaitMessageQueue_URL = "api/Common/ParafaitMessageQueue";

        /// <summary>
        /// RemoteParafaitMessageQueueUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteParafaitMessageQueueUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueue(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<ParafaitMessageQueueDTO> result = await Get<List<ParafaitMessageQueueDTO>>(ParafaitMessageQueue_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ParafaitMessageQueueDTO.SearchByParameters.MESSAGE_QUEUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("messageQueueId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitMessageQueueDTO.SearchByParameters.ENTITY_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("entityName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitMessageQueueDTO.SearchByParameters.STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("status".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("entityGuid".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<ParafaitMessageQueueDTO>> SaveParafaitMessageQueue(List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList)
        {
            log.LogMethodEntry(parafaitMessageQueueDTOList);
            try
            {
                List<ParafaitMessageQueueDTO> responseString = await Post<List<ParafaitMessageQueueDTO>>(ParafaitMessageQueue_URL, parafaitMessageQueueDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<List<ParafaitMessageQueueDTO>> GetParafaitMessageQueueDTOList(List<string> entityGuidList, List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>
                          parameters)
        {
            log.LogMethodEntry(entityGuidList, parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            if (entityGuidList != null)
            {
                searchParameterList.Add(new KeyValuePair<string, string>("entityGuidList", entityGuidList.ToString()));
            }

            try
            {
                List<ParafaitMessageQueueDTO> result = await Get<List<ParafaitMessageQueueDTO>>(ParafaitMessageQueue_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
