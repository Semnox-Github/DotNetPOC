/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the ULC Keys in site Setup module.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.0      29-Aug-2019   Mushahid Faizan     Created 
 *2.90.0      14-Jun-2020   Girish Kundar       Modified : REST API phase 2 changes/standard   
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class ULCKeyController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Product Key Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ULCKeys")]
        public HttpResponseMessage Get()
        {

            log.LogMethodEntry();
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                string pipeSeparatedUlcKeysString = string.Empty;
                SystemOptionsBL systemOptionsBl = new SystemOptionsBL(executionContext, "Parafait Keys", "CustomerMifareUltraLightCKey");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    pipeSeparatedUlcKeysString = Encryption.Decrypt(encryptedOptionValue);
                }

                UlcKeyCollection ulcKeyCollection = new UlcKeyCollection(pipeSeparatedUlcKeysString);
                var content = ulcKeyCollection.Values;
                log.LogMethodExit();
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
        /// Post the JSON Object systemOptionsDTOList
        /// </summary>
        /// <param name="systemOptionsDTOList">systemOptionsDTOList</param>
        [HttpPost]
        [Route("api/Configuration/ULCKeys")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<UlcKeyDTO> ulcKeyDtoList)
        {

            log.LogMethodEntry(ulcKeyDtoList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                UlcKeyCollection ulcKeyCollection = new UlcKeyCollection(ulcKeyDtoList);
                SystemOptionsBL systemOptionsBl = new SystemOptionsBL(executionContext, "Parafait Keys", "CustomerMifareUltraLightCKey");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    systemOptionsBl.GetSystemOptionsDTO.OptionValue = Encryption.Encrypt(ulcKeyCollection.PipeSeparatedUlcKeysString);
                    SystemOptionsDTO systemOptionsDTO = systemOptionsBl.GetSystemOptionsDTO;
                    systemOptionsBl = new SystemOptionsBL(executionContext, systemOptionsDTO);
                    systemOptionsBl.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Delete the JSON Object systemOptionsDTOList
        /// </summary>
        /// <param name="systemOptionsDTOList">systemOptionsDTOList</param>
        [HttpDelete]
        [Route("api/Configuration/ULCKeys")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]  List<UlcKeyDTO> ulcKeyDtoList)
        {

            log.LogMethodEntry(ulcKeyDtoList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                string pipeSeparatedUlcKeysString = string.Empty;
                SystemOptionsBL systemOptionsBl = new SystemOptionsBL(executionContext, "Parafait Keys", "CustomerMifareUltraLightCKey");
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    string encryptedOptionValue = systemOptionsBl.GetSystemOptionsDTO.OptionValue;
                    pipeSeparatedUlcKeysString = Encryption.Decrypt(encryptedOptionValue);
                }

                UlcKeyCollection ulcKeyCollection = new UlcKeyCollection(pipeSeparatedUlcKeysString);
                List<UlcKeyDTO> ulcKeyList = ulcKeyCollection.Values;

                UlcKeyDTO deletedKey = ulcKeyList[0];
                if (systemOptionsBl.GetSystemOptionsDTO != null)
                {
                    if (ulcKeyList.Contains(deletedKey))
                    {
                        ulcKeyList.Remove(deletedKey);
                    }
                    systemOptionsBl.GetSystemOptionsDTO.OptionValue =
                        Encryption.Encrypt(ulcKeyCollection.PipeSeparatedUlcKeysString);
                    systemOptionsBl.Save();

                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException ex)
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

        private bool ValidateUlcKeyDtoList(List<UlcKeyDTO> ulcKeyDtoList)
        {
            bool error = false;
            for (int i = 0; i < ulcKeyDtoList.Count; i++)
            {
                if (UlcKey.IsValidKeyString(ulcKeyDtoList[i].Key)) continue;
                error = true;
                UlcKeyDTO deletedKey = ulcKeyDtoList[i];
                //MessageContainer.GetMessage(utilities.ExecutionContext, "Invalid key"));
                break;
            }
            return error;
        }
    }
}
