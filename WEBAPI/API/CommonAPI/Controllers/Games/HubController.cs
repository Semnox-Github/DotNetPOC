/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Hubs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        30-Aug-2018   Jagan          Created
 *2.80        16-Oct-2019   Jagan Mohana   Implemented the Ebyte Configuration in Post() and added multiple catch blocks for exceptions
 *2.80        12-May-2020   Girish Kundar   Modified :Added HubName,HubId and isActive type as string
 *2.90        12-Aug-2020   Girish Kundar   Modified :Added loadEbyteConfig to Get method
*2.100       27-Oct-2020    Girish Kundar        Modified : Implemented factory class to get/save the data
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Games
{

    public class HubController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/Hubs")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int hubId = -1, string hubName = null, string isRadian = null, string restartAP = null, int currentPage = 0, int pageSize = 0, bool loadChildRecords = false,
                                       bool loadMachineCount = false, bool loadAllEBYTEConfig = false)
        {
            log.LogMethodEntry(isActive, hubId, hubName, isRadian, restartAP);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>();
                searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, isActive));
                    }
                }
                if (hubId > -1)
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_ID, hubId.ToString()));
                }
                if (string.IsNullOrEmpty(hubName) == false)
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.HUB_NAME, hubName));
                }
                if (!string.IsNullOrEmpty(isRadian))
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_RADIAN, isRadian.ToString()));
                }
                if (!string.IsNullOrEmpty(restartAP))
                {
                    searchParameters.Add(new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.RESTART_AP, restartAP.ToString()));
                }
                IHubUseCases hubDataService = GameUseCaseFactory.GetHubUseCases(executionContext);
                var response = await hubDataService.GetHubs(searchParameters, loadMachineCount, loadChildRecords, currentPage, pageSize);
                log.LogMethodExit(response);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = response });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Hub Details
        /// </summary>
        /// <param name="List<HubDTO>">hublistdto</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Game/Hubs")]
        [Authorize]
        public async Task<HttpResponseMessage> Post(string activityType, [FromBody]List<HubDTO> hubsList)
        {
            log.LogMethodEntry(activityType,hubsList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (hubsList != null && hubsList.Any())
                {
                    if (activityType.ToUpper().ToString() == "HUB")
                    {
                        IHubUseCases hubDataService = GameUseCaseFactory.GetHubUseCases(executionContext);
                        var response = await hubDataService.SaveHubs(hubsList);
                    }
                    else if (activityType.ToUpper().ToString() == "HUBCONFIGURE")
                    {
                        HubDTO hubDTO = hubsList[0];
                        if (hubDTO.ConfigureHubType == "NRF")
                        {
                            ConfigureHubBLNRF configureHubBL = new ConfigureHubBLNRF(executionContext, hubDTO);
                            configureHubBL.ConfigureHub();
                        }
                        else if (hubDTO.ConfigureHubType == "SP1ML")
                        {
                            ConfigureHubBLSP1ML configureHubBLSP1ML = new ConfigureHubBLSP1ML(executionContext, hubDTO);
                            configureHubBLSP1ML.ConfigureSpirit();
                        }
                        else if (hubDTO.ConfigureHubType == "SetRegister")
                        {
                            ConfigureHubBLSP1ML configureHubBLSP1ML = new ConfigureHubBLSP1ML(executionContext, hubDTO);
                            configureHubBLSP1ML.SetRegister();
                        }
                        else if (hubDTO.ConfigureHubType == "ReadConfig")
                        {
                            ConfigureHubBLSP1ML configureHubBLSP1ML = new ConfigureHubBLSP1ML(executionContext, hubDTO);
                            configureHubBLSP1ML.ReadConfig();
                        }
                        else if (hubDTO.ConfigureHubType == "EBYTEConfig")
                        {
                            ConfigEBYTEHubBL configEBYTEHub = new ConfigEBYTEHubBL(executionContext, hubDTO);
                            configEBYTEHub.ConfigureEbyte();
                        }
                        //else if (hubDTO.ConfigureHubType == "EBYTEReadAllConfig")
                        //{
                        //    ConfigEBYTEHubBL configEBYTEHub = new ConfigEBYTEHubBL(executionContext, hubDTO);
                        //    configEBYTEHub.ReadAllConfigs();
                        //}
                    }
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (ApplicationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }

        }

        /// <summary>
        /// Delte the Hub Details
        /// </summary>
        /// <param name="List<HubDTO>">masterId</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Game/Hubs")]
        [Authorize]
        public async Task<HttpResponseMessage> Delete(List<HubDTO> hubsList)
        {
            log.LogMethodEntry(hubsList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (hubsList != null && hubsList.Any())
                {
                    IHubUseCases hubDataService = GameUseCaseFactory.GetHubUseCases(executionContext);
                    var  response = await hubDataService.DeleteHubs(hubsList);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}