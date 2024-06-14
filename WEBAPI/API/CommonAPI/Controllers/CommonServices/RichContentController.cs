/********************************************************************************************
 * Project Name - RichContentController
 * Description  - Controller for Getting the Terms and Conditions for the Customer App
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
 *2.80        15-Oct-2019      Nitin Pai      Guest App Phase 2 changes
 *2.90        01-Jun-2020      Girish Kundar  Modified : Added POST method to save rich content
 *2.131.0     01-Nov-2021      Amitha Joy     Modified : Added save of rich content file to image directory in server
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;


namespace Semnox.CommonAPI.CommonServices
{

    public class RichContentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Terms and Conditions as Base 64 string.
        /// </summary>       
        [HttpGet]
        [Route("api/CommonServices/RichContents")]
        public HttpResponseMessage Get(string isActive = null, int contendId = -1, String contentName = null)
        {
            log.LogMethodEntry(isActive, contendId, contentName);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                List<KeyValuePair<RichContentDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RichContentDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ACTIVE_FLAG, isActive.ToString()));
                    }
                }
                if (contendId > -1)
                {
                    searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.ID, contendId.ToString()));
                }
                if (string.IsNullOrEmpty(contentName) == false)
                {
                    searchParameters.Add(new KeyValuePair<RichContentDTO.SearchByParameters, string>(RichContentDTO.SearchByParameters.CONTENT_NAME, contentName.ToString()));
                }
                RichContentListBL richContentListBL = new RichContentListBL(executionContext);
                List<RichContentDTO> richContentDTOList = richContentListBL.GetRichContentDTOList(searchParameters);
                log.LogMethodExit(richContentDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = richContentDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }


        /// <summary>
        /// Performs a Post operation on richContentDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/CommonServices/RichContents")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<RichContentDTO> richContentDTOList)
        {
            log.LogMethodEntry(richContentDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                if (richContentDTOList != null && richContentDTOList.Any())
                {
                    RichContentListBL richContentListBL = new RichContentListBL(executionContext, richContentDTOList);
                    richContentListBL.Save();
                    if (!string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY")))
                    {
                        foreach (RichContentDTO richcontentDTO in richContentDTOList)
                        {

                            using (Stream file = File.OpenWrite(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY")+ (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IMAGE_DIRECTORY").EndsWith("\\") ?"":"\\" )+ Path.GetFileName(richcontentDTO.FileName)))
                            {
                                await file.WriteAsync(richcontentDTO.Data, 0, richcontentDTO.Data.Length);
                            }
                        }
                    }
                    log.LogMethodExit(richContentDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = richContentDTOList });
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
