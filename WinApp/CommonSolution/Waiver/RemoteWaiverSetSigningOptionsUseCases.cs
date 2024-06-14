/********************************************************************************************
* Project Name - Waiver
* Description  - RemoteWaiverSetSigningOptionsUseCases class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    26-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Waiver
{
    class RemoteWaiverSetSigningOptionsUseCases:RemoteUseCases,IWaiverSetSigningOptionsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string WAIVERSET_SIGNING_OPTIONS_URL = "api/Product/WaiverSigningOptions";
        public RemoteWaiverSetSigningOptionsUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<WaiverSetSigningOptionsDTO>> GetWaiverSetSigningOptions(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<WaiverSetSigningOptionsDTO> result = await Get<List<WaiverSetSigningOptionsDTO>>(WAIVERSET_SIGNING_OPTIONS_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case WaiverSetSigningOptionsDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case WaiverSetSigningOptionsDTO.SearchByParameters.WAIVER_SET_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("waiverSetId".ToString(), searchParameter.Value));
                        }
                        break;
                    case WaiverSetSigningOptionsDTO.SearchByParameters.LOOKUP_VALUE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("lookupValueId".ToString(), searchParameter.Value));
                        }
                        break;
                    case WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("waiverSetIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_SIGNING_OPTIONS_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("waiverSigningOptionList".ToString(), searchParameter.Value));
                        }
                        break;                
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveWaiverSetSigningOptions(List<WaiverSetSigningOptionsDTO> waiverSetSigningOptionsDTOs)
        {
            log.LogMethodEntry(waiverSetSigningOptionsDTOs);
            try
            {
                string responseString = await Post<string>(WAIVERSET_SIGNING_OPTIONS_URL, waiverSetSigningOptionsDTOs);
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
