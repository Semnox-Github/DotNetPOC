/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Created to insert update Assettypes  
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Muhammed Mehraj          Created 
 *2.80        22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 *2.80        22-Apr-2020   Girish Kundar            Modified : Added Get parameter  assetTypeId =-1 ,  assetTypeName = null ,  LastUpdatedDate = null.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Maintenance;
namespace Semnox.CommonAPI.Controllers.Maintenance
{
    public class AssetTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get request for AssetType
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/AssetTypes")]
        public HttpResponseMessage Get(string isActive = null, int assetTypeId =-1 , string assetTypeName = null , DateTime? lastUpdatedDate = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, assetTypeId , assetTypeName, lastUpdatedDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                AssetTypeList assetTypeList = new AssetTypeList(executionContext);
                List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>> assetTypeSearchParams = new List<KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>>();
                assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (assetTypeId > -1)
                {
                    assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_ID, assetTypeId.ToString()));
                }
                if (string.IsNullOrEmpty(assetTypeName) == false)
                {
                    assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.ASSETTYPE_NAME, assetTypeName));
                }
                if (lastUpdatedDate != null)
                {
                    DateTime dateTime = Convert.ToDateTime(lastUpdatedDate);
                    if (dateTime == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    assetTypeSearchParams.Add(new KeyValuePair<AssetTypeDTO.SearchByAssetTypeParameters, string>(AssetTypeDTO.SearchByAssetTypeParameters.LASTUPDATEDDATE, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                List<AssetTypeDTO> assetTypeListOnDisplay = assetTypeList.GetAllAssetTypes(assetTypeSearchParams);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = assetTypeListOnDisplay });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Post request for AssetType
        /// </summary>
        /// <param name="assetTypeListOnDisplay"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/AssetTypes")]
        public HttpResponseMessage Post([FromBody] List<AssetTypeDTO> assetTypeListOnDisplay)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetTypeListOnDisplay);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (assetTypeListOnDisplay != null && assetTypeListOnDisplay.Any())
                {
                    AssetTypeList assetTypeList = new AssetTypeList(executionContext, assetTypeListOnDisplay);
                    assetTypeList.SaveAssetTypes();
                    log.LogMethodExit(assetTypeListOnDisplay);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetTypeListOnDisplay });
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
