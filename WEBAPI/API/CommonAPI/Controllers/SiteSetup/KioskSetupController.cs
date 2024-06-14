/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the kiosk setup details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         18-Mar-2019   Jagan Mohana         Created 
 *2.60         23-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
                                                  Added HttpDeleteMethod, Modified HttpPost Method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using System.IO;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.SiteSetup
{
    public class KioskSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object KioskSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/KioskSetup/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                KioskSetupList kioskSetupList = new KioskSetupList(executionContext);
                List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KioskSetupDTO.SearchByParameters, string>>();

                searchParameters.Add(new KeyValuePair<KioskSetupDTO.SearchByParameters, string>(KioskSetupDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<KioskSetupDTO.SearchByParameters, string>(KioskSetupDTO.SearchByParameters.ISACTIVE, isActive));
                }
                var content = kioskSetupList.GetAllKioskSetupsList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on KioskSetupDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/KioskSetup/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<KioskSetupDTO> kioskSetupDTOList)
        {
            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (kioskSetupDTOList != null)
                {
                    // if kioskSetupDTOList.Id is less than zero then insert or else update
                    KioskSetupList kioskSetupList = new KioskSetupList(executionContext, kioskSetupDTOList);
                    kioskSetupList.SaveUpdateKioskSetupsList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on KioskSetupDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/KioskSetup/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<KioskSetupDTO> kioskSetupDTOList)
        {
            try
            {
                log.LogMethodEntry(kioskSetupDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (kioskSetupDTOList != null)
                {
                    // if kioskSetupDTOList.Id is less than zero then insert or else update
                    KioskSetupList kioskSetupList = new KioskSetupList(executionContext, kioskSetupDTOList);
                    kioskSetupList.Delete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}



//var httpRequest = HttpContext.Current.Request;

//MemoryStream ms = new MemoryStream();
//httpRequest.Files[0].InputStream.CopyTo(ms);
//byte[] fileContent = ms.ToArray();
//kioskSetupDTOList.Image = fileContent;

//var formData = httpRequest.Form["KioskSetupForm"];
//KioskSetupDTO kioskSetupDTOs = JsonConvert.DeserializeObject<KioskSetupDTO>(formData);
// if kioskSetupDTOList.Id is less than zero then insert or else update
//KioskSetupList kioskSetupList = new KioskSetupList(kioskSetupDTOList, executionContext);
//kioskSetupList.SaveUpdateKioskSetupsList();
