/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the AssetTechnicianMapping
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.100.0    24-Sept-2020    Mushahid Faizan Created
 ********************************************************************************************/
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.Maintenance;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class AssetTechnicianMappingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for AssetTechnicianMapping
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/AssetTechnicianMappings")]
        public HttpResponseMessage Get(string requestType="ASSET",string isActive = null, int mapId = -1, int assetId = -1, int assetTypeId = -1, int userId = -1, int siteId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, mapId, assetId, assetTypeId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext);
                List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (mapId > -1)
                {
                    searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.MAP_ID, mapId.ToString()));
                }
                if (assetId > -1)
                {
                    searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.ASSET_ID, assetId.ToString()));
                }
                if (assetTypeId > -1)
                {
                    searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.ASSET_TYPE_ID, assetTypeId.ToString()));
                }
                if (userId > -1)
                {
                    searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.USER_ID, userId.ToString()));
                }

                if (siteId > -1 && executionContext.SiteId> -1) // search parameter is added only for HQ scenario
                {
                    searchParams.Add(new KeyValuePair<AssetTechnicianMappingDTO.SearchByParameters, string>(AssetTechnicianMappingDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                }

                List<AssetTechnicianMappingDTO> assetTechnicianMappingDTOList = new List<AssetTechnicianMappingDTO>();

				if (requestType.ToUpper().ToString() == "ASSET") {
					assetTechnicianMappingDTOList = assetTechnicianMappingListBL.GetAllAssetTechnicianMappingList(searchParams);
				} else if (requestType.ToUpper().ToString() == "ASSETTYPE") {
					assetTechnicianMappingDTOList = assetTechnicianMappingListBL.GetAllAssetTypeTechnicianMappingList(searchParams);
				} else {
					assetTechnicianMappingDTOList = assetTechnicianMappingListBL.GetAllSiteTechnicianMappingList(searchParams);
				}
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = assetTechnicianMappingDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post request for assetTechnicianMappingDTO
        /// </summary>
        /// <param name="assetTechnicianMappingDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/AssetTechnicianMappings")]
        public HttpResponseMessage Post([FromBody] List<AssetTechnicianMappingDTO> assetTechnicianMappingDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetTechnicianMappingDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (assetTechnicianMappingDTOList != null && assetTechnicianMappingDTOList.Any())
                {
                    AssetTechnicianMappingListBL assetTechnicianMappingListBL = new AssetTechnicianMappingListBL(executionContext, assetTechnicianMappingDTOList);
                    assetTechnicianMappingListBL.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetTechnicianMappingDTOList });
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
