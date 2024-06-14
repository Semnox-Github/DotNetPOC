/********************************************************************************************
* Project Name - Product
* Description  - UpsellOfferUseCases class 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    class RemoteUpsellOfferUseCases:RemoteUseCases,IUpsellOfferUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string UPSELLOFFER_URL = "api/Product/ProductUpsellOffers";
        public RemoteUpsellOfferUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UpsellOffersDTO>> GetUpsellOffers(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>>
                         parameters)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<UpsellOffersDTO> result = await Get<List<UpsellOffersDTO>>(UPSELLOFFER_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<UpsellOffersDTO.SearchByUpsellOffersParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("offerId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UpsellOffersDTO.SearchByUpsellOffersParameters.PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("productId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UpsellOffersDTO.SearchByUpsellOffersParameters.SALE_GROUP_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("saleGroupId".ToString(), searchParameter.Value));
                        }
                        break;
                    case UpsellOffersDTO.SearchByUpsellOffersParameters.OFFER_PRODUCT_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("offerProductId".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveUpsellOffers(List<UpsellOffersDTO> upsellOffersList)
        {
            log.LogMethodEntry(upsellOffersList);
            try
            {
                string responseString = await Post<string>(UPSELLOFFER_URL, upsellOffersList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<UpsellOffersDTO> upsellOffersDTOList)
        {
            try
            {
                log.LogMethodEntry(upsellOffersDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(upsellOffersDTOList);
                string responseString = await Delete(UPSELLOFFER_URL, content);
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
