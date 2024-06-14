/********************************************************************************************
 * Project Name -KioskUIFramework
 * Description  -RemoteAppScreenUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    27-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    class RemoteAppScreenUseCases:RemoteUseCases,IAppScreenUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string APPSCREEN_URL = "api/Kiosk/AppScreens";
        public RemoteAppScreenUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AppScreenDTO>> GetAppScreens(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> searchParameters,
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
                List<AppScreenDTO> result = await Get<List<AppScreenDTO>>(APPSCREEN_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AppScreenDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AppScreenDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AppScreenDTO.SearchByParameters.SCREEN_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("screenId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.SCREEN_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("screenIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.SCREEN_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("screenName".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.APP_SCREEN_PROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("appScreenProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.APP_SCREEN_PROFILE_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("appScreenProfileIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.SCREEN_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("screenKey".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppScreenDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAppScreens(List<AppScreenDTO> appScreenDTOList)
        {
            log.LogMethodEntry(appScreenDTOList);
            try
            {
                string responseString = await Post<string>(APPSCREEN_URL, appScreenDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<AppScreenDTO> appScreenDTOList)
        {
            try
            {
                log.LogMethodEntry(appScreenDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(appScreenDTOList);
                string responseString = await Delete(APPSCREEN_URL, content);
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
