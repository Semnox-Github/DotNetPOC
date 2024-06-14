/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Created to Import Machines to AssetGroupAsset
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Muhammed Mehraj          Created 
 * 2.70       9-4-2019      Rakesh Kumar             Modify Get method
 *2.80        22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;

namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class AssetGroupAssetController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for AssetGroupAsset
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/AssetGroupAssets")]
        public HttpResponseMessage Get(string isActive = null, int assetGroupAssetId = -1, int assetGroupId = -1, int assetId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, assetGroupAssetId, assetGroupId, assetId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AssetGroupAssetMapperList assetGroupAssetList = new AssetGroupAssetMapperList(executionContext);
                List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>> assetSearchParams = new List<KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>>();
                assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (assetGroupAssetId > -1)
                {
                    assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ASSET_ID, assetGroupAssetId.ToString()));
                }
                if (assetGroupId > -1)
                {
                    assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_GROUP_ID, assetGroupId.ToString()));
                }
                if (assetId > -1)
                {
                    assetSearchParams.Add(new KeyValuePair<AssetGroupAssetDTO.SearchByAssetGroupAssetParameters, string>(AssetGroupAssetDTO.SearchByAssetGroupAssetParameters.ASSET_ID, assetId.ToString()));
                }

                List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay = assetGroupAssetList.GetAllAssetGroupAsset(assetSearchParams);
                log.LogMethodExit(assetGroupAssetListOnDisplay);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = assetGroupAssetListOnDisplay });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post GenricAsset AssetGroupAsset
        /// </summary>
        /// <param name="assetListOnDisplay"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/AssetGroupAssets")]
        public HttpResponseMessage Post([FromBody] List<AssetGroupAssetDTO> assetGroupAssetListOnDisplay)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetGroupAssetListOnDisplay);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (assetGroupAssetListOnDisplay != null && assetGroupAssetListOnDisplay.Any())
                {
                    AssetGroupAssetMapperList assetGroupAssetList = new AssetGroupAssetMapperList(executionContext, assetGroupAssetListOnDisplay);
                    assetGroupAssetList.SaveAssetGroupAsset();
                    log.LogMethodExit(assetGroupAssetListOnDisplay);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetGroupAssetListOnDisplay });
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
    }
}
