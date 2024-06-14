/********************************************************************************************
 * Project Name -Product
 * Description  -AttractionPlaysUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    09-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteAttractionPlaysUseCases:RemoteUseCases, IAttractionPlaysUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ATTRACTIONPLAYS_URL = "api/Product/AttractionPlays";
        public RemoteAttractionPlaysUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<AttractionPlaysDTO>> GetAttractionPlays(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>>
                      searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<AttractionPlaysDTO> result = await Get<List<AttractionPlaysDTO>>(ATTRACTIONPLAYS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AttractionPlaysDTO.SearchByAttractionPlaysParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("attractionPlayId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttractionPlaysDTO.SearchByAttractionPlaysParameters.PLAYNAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("playName".ToString(), searchParameter.Value));
                        }
                        break;
                    case AttractionPlaysDTO.SearchByAttractionPlaysParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAttractionPlays(List<AttractionPlaysDTO> attractionPlaysDTOList)
        {
            log.LogMethodEntry(attractionPlaysDTOList);
            try
            {
                string responseString = await Post<string>(ATTRACTIONPLAYS_URL, attractionPlaysDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<AttractionPlaysDTO> attractionPlaysDTOList)
        {
            try
            {
                log.LogMethodEntry(attractionPlaysDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(attractionPlaysDTOList);
                string responseString = await Delete(ATTRACTIONPLAYS_URL, content);
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
