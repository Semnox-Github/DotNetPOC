/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Panel
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana     Created
 *2.70        10-Sept-2019   Jagan Mohana     Controller Renamed from SetupPanelController to DisplayPanelController
 *2.80        05-Apr-2020    Girish Kundar    Modified: API end point in route 
 *2.90        28-Jul-2020    Mushahid Faizan  Modified: as per API Standards, namespace changes, added searchParameters in get(), Removed Delete().
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class DisplayPanelController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Panel List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DisplayPanels")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int panelId = -1, string panelName = null, string pcName = null, string displayGroup = null,
            string macAddress = null, string pcNameExact = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, panelId, panelName, pcName, displayGroup, macAddress, pcNameExact);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DisplayPanelDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (panelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.PANEL_ID, panelId.ToString()));
                }
                if (!string.IsNullOrEmpty(panelName))
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.PANEL_NAME, panelName));
                }
                if (!string.IsNullOrEmpty(pcName))
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.PC_NAME, pcName));
                }
                if (!string.IsNullOrEmpty(displayGroup))
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.DISPLAY_GROUP, displayGroup));
                }
                if (!string.IsNullOrEmpty(macAddress))
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.MAC_ADDRESS, macAddress));
                }
                if (!string.IsNullOrEmpty(pcNameExact))
                {
                    searchParameters.Add(new KeyValuePair<DisplayPanelDTO.SearchByParameters, string>(DisplayPanelDTO.SearchByParameters.PC_NAME_EXACT, pcNameExact));
                }

                DisplayPanelListBL panelListBL = new DisplayPanelListBL(executionContext);
                var content = panelListBL.GetDisplayPanelDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on panel details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/DisplayPanels")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DisplayPanelDTO> displayPanelDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(displayPanelDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (displayPanelDTOList != null && displayPanelDTOList.Any())
                {
                        DisplayPanelListBL displayPanelList = new DisplayPanelListBL(executionContext, displayPanelDTOList);
                    displayPanelList.Save();
                  
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = displayPanelDTOList });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}