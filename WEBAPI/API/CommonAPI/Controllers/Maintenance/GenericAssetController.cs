/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Created to insert update Asset Details  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Muhammed Mehraj          Created 
 * 2.70       09-Aug-2019   Rakesh Kumar             Delete method added
 *2.80        22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class GenericAssetController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/GenericAssets")]
        public HttpResponseMessage Get(string isActive = null, int assetType = -1,
                                       string firstName = null, string urn = null, string assetStatus = null, string location = null, bool buildAssetDetailed = false, bool buildAssetLimited = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, firstName, urn, assetStatus, assetType, location);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                AssetList assetList = new AssetList(executionContext);
                List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>> assetSearchParams = new List<KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>>
                {
                    new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.SITE_ID, executionContext.GetSiteId().ToString())
                };
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                List<GenericAssetDTO> genericAssetDTOList = new List<GenericAssetDTO>();
                if (!string.IsNullOrEmpty(firstName))
                {
                    assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_NAME, firstName));
                }
                if (!string.IsNullOrEmpty(urn))
                {
                    assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.URN, urn));
                }
                if (!string.IsNullOrEmpty(assetStatus))
                {
                    assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_STATUS, assetStatus));
                }
                if (assetType > 0)
                {
                    assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.ASSET_TYPE_ID, Convert.ToString(assetType)));
                }
                if (!string.IsNullOrEmpty(location))
                {
                    assetSearchParams.Add(new KeyValuePair<GenericAssetDTO.SearchByAssetParameters, string>(GenericAssetDTO.SearchByAssetParameters.LOCATION, location));
                }
                genericAssetDTOList = assetList.GetAllAssets(assetSearchParams);
                Sheet sheet = new Sheet();
                if (buildAssetDetailed || buildAssetLimited)
                {
                    assetList = new AssetList(executionContext, genericAssetDTOList);
                    sheet = assetList.BuildTemplate(buildAssetLimited);
                }

                log.LogMethodExit(genericAssetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = genericAssetDTOList, Sheet = sheet });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post GenricAsset collection
        /// </summary>
        /// <param name="assetListOnDisplay"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/GenericAssets")]
        public HttpResponseMessage Post([FromBody] List<GenericAssetDTO> assetListOnDisplay)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetListOnDisplay);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (assetListOnDisplay != null && assetListOnDisplay.Any())
                {
                    AssetList assetList = new AssetList(executionContext, assetListOnDisplay);
                    assetList.SaveAssetList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetListOnDisplay });
                }
                else
                {
                    log.LogMethodExit(null);
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
        /// Delete GenricAsset collection
        /// </summary>
        /// <param name="assetListOnDisplay"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Maintenance/GenericAssets")]
        public HttpResponseMessage Delete([FromBody] List<GenericAssetDTO> assetListOnDisplay)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetListOnDisplay);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (assetListOnDisplay != null && assetListOnDisplay.Any())
                {
                    AssetList assetList = new AssetList(executionContext, assetListOnDisplay);
                    assetList.SaveAssetList();
                    log.LogMethodExit(assetListOnDisplay);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetListOnDisplay });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }


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
