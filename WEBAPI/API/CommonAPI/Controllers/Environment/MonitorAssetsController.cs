/********************************************************************************************
 * Project Name - Tools Controller
 * Description  - Created to fetch, update and insert monitor assets
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.90        18-Jun-2020   Mushahid Faizan          Created
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.CommonAPI.Environments
{
    public class MonitorAssetsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON monitor assets list
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Environment/MonitorAssets")]
        public HttpResponseMessage Get(string isActive = null, int assetId = -1, int assetTypeId = -1, string hostName = null, string ipAddress = null, string macAddress = null, string assetName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, assetId, assetTypeId, hostName, ipAddress, macAddress);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext);
                List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters = new List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>>();
                searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (assetId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_ID, assetId.ToString()));
                }
                if (assetTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.ASSET_TYPE_ID, assetTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(hostName))
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.HOST_NAME, hostName.ToString()));
                }
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.IP_ADDRESS, ipAddress.ToString()));
                }
                if (!string.IsNullOrEmpty(macAddress))
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.MAC_ADDRESS, macAddress.ToString()));
                }
                if (!string.IsNullOrEmpty(assetName))
                {
                    searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.NAME, assetName.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>(MonitorAssetDTO.SearchByMonitorAssetParameters.ISACTIVE, isActive));
                    }
                }

                var content = monitorAssetList.GetAllMonitorAssets(searchParameters);
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
        /// Post the JSON monitor assets
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Environment/MonitorAssets")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MonitorAssetDTO> monitorAssetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(monitorAssetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (monitorAssetDTOList != null && monitorAssetDTOList.Any())
                {
                    MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext, monitorAssetDTOList);
                    monitorAssetList.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = monitorAssetDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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
        /// Delete monitor assets Record
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Environment/MonitorAssets")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MonitorAssetDTO> monitorAssetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(monitorAssetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                if (monitorAssetDTOList != null && monitorAssetDTOList.Any())
                {
                    MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext, monitorAssetDTOList);
                    monitorAssetList.Delete();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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