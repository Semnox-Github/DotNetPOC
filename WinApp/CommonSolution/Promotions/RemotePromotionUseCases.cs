/********************************************************************************************
* Project Name - Promotions
* Description  - RemotePromotionUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    26-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Promotions
{
    class RemotePromotionUseCases:RemoteUseCases,IPromotionUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PROMOTION_URL = "api/Promotion/Promotions";
        public RemotePromotionUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<PromotionDTO>> GetPromotions(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters,
                                                                      bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<PromotionDTO> result = await Get<List<PromotionDTO>>(PROMOTION_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<PromotionDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<PromotionDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case PromotionDTO.SearchByParameters.PROMOTION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("promotionId".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.PROMOTION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("promotionName".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.PROMOTION_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("promotionType".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.RECUR_TYPE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("recurType".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.TIME_FROM:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("timeFrom".ToString(), searchParameter.Value));
                        }
                        break;
                    case PromotionDTO.SearchByParameters.TIME_TO:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("timeTo".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SavePromotions(List<PromotionDTO> promotionDTOList)
        {
            log.LogMethodEntry(promotionDTOList);
            try
            {
                string responseString = await Post<string>(PROMOTION_URL, promotionDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<PromotionDTO> promotionDTOList)
        {
            try
            {
                log.LogMethodEntry(promotionDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(promotionDTOList);
                string responseString = await Delete(PROMOTION_URL, content);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                log.LogMethodExit(response);
                return response;
            }
            catch (WebApiException wex)
            {
                log.Error(wex);
                throw;
            }
        }
    }
}
