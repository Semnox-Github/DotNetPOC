/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Created to insert and update AssetGroup
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Apr-2019   Muhammed Mehraj          Created 
 * 2.70       9-4-2019      Rakesh Kumar             Modify Get method
 *2.80        22-Apr-2020   Mushahid Faizan          Modified end points and Removed token from response body.
 *2.80        22-Apr-2020   Girish Kundar            Modified : Added asset group id and group name to the Get parameter
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
    public class AssetGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///  Get request for AssetGroup
        /// </summary>
        /// <param name="activeRecordsOnly"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Maintenance/AssetGroups")]
        public HttpResponseMessage Get(string isActive = null, int assetGroupId =-1,string groupName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, assetGroupId, groupName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                AssetGroupList assetGroupList = new AssetGroupList(executionContext);
                List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>> assetGroupSearchParams = new List<KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>>();
                assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (assetGroupId > -1)
                {
                    assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_ID, assetGroupId.ToString()));
                }
                if (string.IsNullOrEmpty(groupName) == false)
                {
                    assetGroupSearchParams.Add(new KeyValuePair<AssetGroupDTO.SearchByAssetGroupParameters, string>(AssetGroupDTO.SearchByAssetGroupParameters.ASSETGROUP_NAME, groupName));
                }
                List<AssetGroupDTO> assetGroupDTOList = assetGroupList.GetAllAssetGroups(assetGroupSearchParams);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = assetGroupDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the AssetGroup collection
        /// </summary>
        /// <param name="assetGroupListOnDisplay"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Maintenance/AssetGroups")]
        public HttpResponseMessage Post([FromBody] List<AssetGroupDTO> assetGroupListOnDisplay)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(assetGroupListOnDisplay);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (assetGroupListOnDisplay != null && assetGroupListOnDisplay.Any())
                {
                    AssetGroupList assetGroupList = new AssetGroupList(assetGroupListOnDisplay, executionContext);
                    assetGroupList.SaveAssetGroups();
                    log.LogMethodExit(assetGroupListOnDisplay);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = assetGroupListOnDisplay });
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
