/********************************************************************************************
 * Project Name - Inventory
 * Description  - RemoteUOMUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         30-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Inventory
{
    public class RemoteUOMUseCases : RemoteUseCases, IUOMUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string UOM_URL = "api/Inventory/UOMs";
        private const string UOM_COUNT_URL = "api/Inventory/UOMCounts";

        public RemoteUOMUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<UOMDTO>> GetUOMs(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                                 parameters, bool loadChildRecords, bool loadActiveChild, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(parameters);

            List<UOMDTO> result = null;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("currentPage".ToString(), currentPage.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("pageSize".ToString(), pageSize.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChild".ToString(), loadActiveChild.ToString()));
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                string responseString = await Get(UOM_URL, searchParameterList);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<List<UOMDTO>>(data.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        public async Task<int> GetUOMCounts(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>
                                                 parameters)
        {
            log.LogMethodEntry(parameters);

            int result = 0;
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(parameters));
            }
            try
            {
                string responseString = await Get(UOM_COUNT_URL, searchParameterList);
                dynamic response = JsonConvert.DeserializeObject(responseString);
                if (response != null)
                {
                    object data = response["data"];
                    result = JsonConvert.DeserializeObject<int>(data.ToString());
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<UOMDTO.SearchByUOMParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case UOMDTO.SearchByUOMParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case UOMDTO.SearchByUOMParameters.UOM:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uOM".ToString(), searchParameter.Value));
                        }
                        break;
                    case UOMDTO.SearchByUOMParameters.UOMID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uOMId".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<string> SaveUOMs (List<UOMDTO> uomDTOList)
        {
            log.LogMethodEntry(uomDTOList);
            try
            {
                string responseString = await Post<string>(UOM_URL, uomDTOList);
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
