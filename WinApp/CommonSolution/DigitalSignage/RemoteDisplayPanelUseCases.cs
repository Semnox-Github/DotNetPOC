/********************************************************************************************
 * Project Name -Digital Signage
 * Description  -DisplayPanelUseCases class 
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
    public class RemoteDisplayPanelUseCases:RemoteUseCases,IDisplayPanelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DISPLAYPANEL_URL = "api/DigitalSignage/DisplayPanels";
        private const string STARTPC_URL = "api/DigitalSignage/StartPC";
        private const string SHUTDOWNPC_URL = "api/DigitalSignage/ShutdownPC";
        private const string DISPLAY_PANEL_CONTAINER_URL = "api/DigitalSignage/DisplayPanelContainer";

        public RemoteDisplayPanelUseCases(ExecutionContext executionContext)
           : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public async Task<List<DisplayPanelDTO>> GetDisplayPanels(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>
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
                List<DisplayPanelDTO> result = await Get<List<DisplayPanelDTO>>(DISPLAYPANEL_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DisplayPanelDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DisplayPanelDTO.SearchByParameters.PANEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("panelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.PANEL_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("panelName".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.PC_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pCName".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.PC_NAME_EXACT:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("pCNameExact".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.DISPLAY_GROUP:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("displayGroup".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.MAC_ADDRESS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("mACAddress".ToString(), searchParameter.Value));
                        }
                        break;
                    case DisplayPanelDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveDisplayPanels(List<DisplayPanelDTO> displayPanelDTOList)
        {
            log.LogMethodEntry(displayPanelDTOList);
            try
            {
                string responseString = await Post<string>(DISPLAYPANEL_URL, displayPanelDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> SaveStartPCs(List<DisplayPanelDTO> displayPanelDTOList)
        {
            log.LogMethodEntry(displayPanelDTOList);
            try
            {
                string responseString = await Post<string>(STARTPC_URL, displayPanelDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        public async Task<string> SaveShutdownPCs(List<DisplayPanelDTO> displayPanelDTOList)
        {
            log.LogMethodEntry(displayPanelDTOList);
            try
            {
                string responseString = await Post<string>(SHUTDOWNPC_URL, displayPanelDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetDisplayPanelContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns>DisplayPanelContainerDTOCollection</returns>
        public async Task<DisplayPanelContainerDTOCollection> GetDisplayPanelContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId,hash, rebuildCache);
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("siteId", siteId.ToString()));
            if (string.IsNullOrWhiteSpace(hash) == false)
            {
                parameters.Add(new KeyValuePair<string, string>("hash", hash));
            }
            parameters.Add(new KeyValuePair<string, string>("rebuildCache", rebuildCache.ToString()));
            DisplayPanelContainerDTOCollection result = await Get<DisplayPanelContainerDTOCollection>(DISPLAY_PANEL_CONTAINER_URL, parameters);
            log.LogMethodExit(result);
            return result;
        }
    }
}
