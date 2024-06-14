/********************************************************************************************
 * Project Name - RemoteParafaitFunctionEventUseCases
 * Description  - RemoteParafaitFunctionEventUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.110.0      14-Dec-2020       Deeksha            Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Communication
{
    /// <summary>
    /// RemoteParafaitFunctionEventUseCases
    /// </summary>
    public class RemoteParafaitFunctionEventUseCases : RemoteUseCases, IParafaitFunctionEventUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PARAFAIT_FUNCTION_EVENTS_URL = "api/Communication/ParafaitFunctionEvents";
        /// <summary>
        /// RemoteParafaitFunctionEventUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteParafaitFunctionEventUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetParafaitFunctionEvent
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<ParafaitFunctionEventDTO>> GetParafaitFunctionEvent(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ParafaitFunctionEventDTO> result = await Get<List<ParafaitFunctionEventDTO>>(PARAFAIT_FUNCTION_EVENTS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }         

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string>> searchParams)
        {
            log.LogMethodEntry(searchParams);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ParafaitFunctionEventDTO.SearchByParameters, string> searchParameter in searchParams)
            {
                switch (searchParameter.Key)
                {
                    case ParafaitFunctionEventDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break; 
                    case ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitFunctionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitFunctionEventId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ParafaitFunctionEventDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("parafaitFunctionEventName".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        ///// <summary>
        ///// SaveParafaitFunctionEvent
        ///// </summary>
        ///// <param name="parafaitFunctionsDTOList"></param>
        ///// <returns></returns>
        //public async Task<string> SaveParafaitFunctionEvent(List<ParafaitFunctionEventDTO> parafaitFunctionsDTOList)
        //{
        //    log.LogMethodEntry(parafaitFunctionsDTOList);
        //    try
        //    {
        //        string result = await Post<string>(PARAFAIT_FUNCTION_URL, parafaitFunctionsDTOList);
        //        log.LogMethodExit(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error(ex);
        //        throw ex;
        //    }
        //}
         
    }
}
