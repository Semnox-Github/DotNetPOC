/********************************************************************************************
 * Project Name - Product
 * Description  - ProductGamesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    06-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteProductGamesUseCases:RemoteUseCases,IProductGamesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PRODUCTGAMES_URL = "api/Product/ProductGames";    

        public RemoteProductGamesUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<ProductGamesDTO>> GetProductGames(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>>
                         parameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<ProductGamesDTO> result = await Get<List<ProductGamesDTO>>(PRODUCTGAMES_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("product_id".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_GAME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("product_game_id".ToString(), searchParameter.Value));
                        }
                        break;
                    case ProductGamesDTO.SearchByProductGamesParameters.PRODUCT_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("product_id_List".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveProductGames(List<ProductGamesDTO> productGamesDTOList)
        {
            log.LogMethodEntry(productGamesDTOList);
            try
            {
                string responseString = await Post<string>(PRODUCTGAMES_URL, productGamesDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        } 
        public async Task<string> Delete(List<ProductGamesDTO> productGamesDTOList)
        {
            try
            {
                log.LogMethodEntry(productGamesDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(productGamesDTOList);
                string responseString = await Delete(PRODUCTGAMES_URL, content);
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
