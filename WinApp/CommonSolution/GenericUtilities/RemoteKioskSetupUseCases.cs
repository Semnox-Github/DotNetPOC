/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -RemoteKioskSetupUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    27-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteKioskSetupUseCases:RemoteUseCases,IKioskSetupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string KIOSK_SETUP_URL = "api/Kiosk/KioskCurrencies";
        public RemoteKioskSetupUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<KioskSetupDTO>> GetKioskSetups(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<KioskSetupDTO> result = await Get<List<KioskSetupDTO>>(KIOSK_SETUP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<KioskSetupDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case KioskSetupDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case KioskSetupDTO.SearchByParameters.NOTE_COIN_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("noteCoinFlag".ToString(), searchParameter.Value));
                        }
                        break;
                    case KioskSetupDTO.SearchByParameters.ISACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("active".ToString(), searchParameter.Value));
                        }
                        break;                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveKioskSetups(List<KioskSetupDTO> kioskSetupDTOList)
        {
            log.LogMethodEntry(kioskSetupDTOList);
            try
            {
                string responseString = await Post<string>(KIOSK_SETUP_URL, kioskSetupDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<KioskSetupDTO> kioskSetupDTOList)
        {
            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(kioskSetupDTOList);
                string responseString = await Delete(KIOSK_SETUP_URL, content);
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
