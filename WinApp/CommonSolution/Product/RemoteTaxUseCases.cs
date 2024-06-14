/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteTaxUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0         09-Nov-2020       Mushahid Faizan         Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public class RemoteTaxUseCases : RemoteUseCases, ITaxUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string TAX_URL = "api/Inventory/Taxes";

        public RemoteTaxUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<TaxDTO>> GetTaxes(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>
                          parameters, bool buildChildRecords, bool loadActiveChild, SqlTransaction sqlTransaction = null, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("buildChildRecords".ToString(), buildChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                List<TaxDTO> result = await Get<List<TaxDTO>>(TAX_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> lookupSearchParams)
        {

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<TaxDTO.SearchByTaxParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case TaxDTO.SearchByTaxParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaxDTO.SearchByTaxParameters.TAX_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taxName".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaxDTO.SearchByTaxParameters.TAX_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taxId".ToString(), searchParameter.Value));
                        }
                        break;
                    case TaxDTO.SearchByTaxParameters.TAX_PERCENTAGE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("taxPercentage".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveTaxes(List<TaxDTO> taxDTOList)
        {
            log.LogMethodEntry(taxDTOList);
            try
            {
                string responseString = await Post<string>(TAX_URL, taxDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> DeleteTaxes(List<TaxDTO> taxDTOList)
        {
            log.LogMethodEntry(taxDTOList);
            try
            {
                string response = await Delete<string>(TAX_URL, taxDTOList);
                log.LogMethodExit(response);
                return response;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
