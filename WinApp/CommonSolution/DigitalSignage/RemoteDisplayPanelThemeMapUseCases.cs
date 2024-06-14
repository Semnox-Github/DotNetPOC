/********************************************************************************************
 * Project Name - DigitalSignage
 * Description  - DisplayPanelThemeMapUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    21-Apr-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
    class RemoteDisplayPanelThemeMapUseCases:RemoteUseCases,IDisplayPanelThemeMapUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DISPLAYPANELTHEMEMAP_URL = "api/DigitalSignage/DisplayPanelThemeMaps";
        public RemoteDisplayPanelThemeMapUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<DisplayPanelThemeMapDTO>> GetDisplayPanelThemeMaps(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>>
                        searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();

            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<DisplayPanelThemeMapDTO> result = await Get<List<DisplayPanelThemeMapDTO>>(DISPLAYPANELTHEMEMAP_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DisplayPanelThemeMapDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DisplayPanelThemeMapDTO.SearchByParameters.ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("id".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelThemeMapDTO.SearchByParameters.PANEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("panelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelThemeMapDTO.SearchByParameters.THEME_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("themeId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelThemeMapDTO.SearchByParameters.SCHEDULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("scheduleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelThemeMapDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;  
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDisplayPanelThemeMaps(List<DisplayPanelThemeMapDTO> displayPanelThemeDTOList)
        {
            log.LogMethodEntry(displayPanelThemeDTOList);
            try
            {
                string responseString = await Post<string>(DISPLAYPANELTHEMEMAP_URL, displayPanelThemeDTOList);
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
