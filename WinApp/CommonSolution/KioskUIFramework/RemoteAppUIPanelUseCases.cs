/********************************************************************************************
 * Project Name -KioskUIFramework
 * Description  -RemoteAppUIPanelUseCases class 
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
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskUIFamework
{
    class RemoteAppUIPanelUseCases:RemoteUseCases,IAppUIPanelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string APPUIPANEL_URL = "api/Kiosk/AppPanels";
        public RemoteAppUIPanelUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<AppUIPanelDTO>> GetAppUIPanels(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> searchParameters,
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
                List<AppUIPanelDTO> result = await Get<List<AppUIPanelDTO>>(APPUIPANEL_URL,searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<AppUIPanelDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<AppUIPanelDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case AppUIPanelDTO.SearchByParameters.UI_PANEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uiPanelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.UI_PANEL_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uiPanelIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.UI_PANEL_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uiPanelName".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.UI_PANEL_KEY:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("uiPanelKey".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.APP_SCREEN_PROFILE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("appScreenProfileId".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.APP_SCREEN_PROFILE_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("appScreenProfileIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case AppUIPanelDTO.SearchByParameters.ACTIVE_FLAG:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("activeFlag".ToString(), searchParameter.Value));
                        }
                        break;                  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveAppUIPanels(List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            log.LogMethodEntry(appUIPanelsDTOList);
            try
            {
                string responseString = await Post<string>(APPUIPANEL_URL, appUIPanelsDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> Delete(List<AppUIPanelDTO> appUIPanelsDTOList)
        {
            try
            {
                log.LogMethodEntry(appUIPanelsDTOList);
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                string content = JsonConvert.SerializeObject(appUIPanelsDTOList);
                string responseString = await Delete(APPUIPANEL_URL, content);
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
