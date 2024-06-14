/********************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert Ads
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        02-May-2019   Jagan Mohana Rao          Created to Get and Post Methods.
 *2.90        18-May-2020   Mushahid Faizan           Modified :As per Rest API standard, Added SearchParams and Renamed controller from AdManagement to AdsController
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ServerCore;
namespace Semnox.CommonAPI.GameServer
{
    public class AdsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Ads
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/GameServer/Ads")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int adId = -1, string adName = null, string adType = null, bool loadActiveChild = false, bool buildChildRecords = false,
                                            bool buildImage = false)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, adId, adName, adType, loadActiveChild, buildChildRecords, buildImage);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AdsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AdsDTO.SearchByParameters, string>(AdsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        searchParameters.Add(new KeyValuePair<AdsDTO.SearchByParameters, string>(AdsDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(adName))
                {
                    searchParameters.Add(new KeyValuePair<AdsDTO.SearchByParameters, string>(AdsDTO.SearchByParameters.AD_NAME, adName.ToString()));
                }
                if (!string.IsNullOrEmpty(adType))
                {
                    searchParameters.Add(new KeyValuePair<AdsDTO.SearchByParameters, string>(AdsDTO.SearchByParameters.AD_TYPE, adType.ToString()));
                }
                if (adId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AdsDTO.SearchByParameters, string>(AdsDTO.SearchByParameters.AD_ID, adId.ToString()));
                }

                IAdUseCases adUseCases = ServerCoreUseCaseFactory.GetAdUseCases(executionContext);
                List<AdsDTO> adsDTOList = await adUseCases.GetAds(searchParameters, buildChildRecords, loadActiveChild, buildImage);
                log.LogMethodExit(adsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = adsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Ads
        /// </summary>
        /// <param name="adsDTOList">adsDTOList</param>
        /// <returns>HttpMessgae</returns>       
        [HttpPost]
        [Route("api/GameServer/Ads")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AdsDTO> adsDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(adsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (adsDTOList != null && adsDTOList.Any())
                {
                    IAdUseCases adUseCases = ServerCoreUseCaseFactory.GetAdUseCases(executionContext);
                    List<AdsDTO> adDTOList = await adUseCases.SaveAds(adsDTOList);
                    log.LogMethodExit(adDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = adDTOList });
                }
                else
                {
                    log.LogMethodExit();
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the  adsDTOList
        /// </summary>    
        /// <param name="adsDTOList">adsDTOList</param>
        /// <returns>HttpResponseMessage</returns>        
        [HttpDelete]
        [Route("api/GameServer/Ads")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<AdsDTO> adsDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(adsDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (adsDTOList != null && adsDTOList.Any())
                {
                    AdsList adsList = new AdsList(executionContext, adsDTOList);
                    adsList.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}