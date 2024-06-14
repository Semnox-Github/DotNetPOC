/********************************************************************************************
 * Project Name - Product Controller
 * Description  - Created to uploadfiles to server.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        25-Feb-2019   Muhammed Mehraj          Created 
 **********************************************************************************************
 *2.50        19-Mar-2019   Akshay Gulaganji         Added customGenericException 
 *2.100       09-Sep-2020    Girish Kundar             Modified : Attendance pay rate changes
 ********************************************************************************************/

using Semnox.CommonAPI.Helpers;

using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using System;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.CommonServices
{
    public class FileUploadController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();

        [HttpPost]
        [Route("api/CommonServices/FileUpload/")]
        [Authorize]
        public HttpResponseMessage Post()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                HttpRequest httpRequest = HttpContext.Current.Request;
                log.LogMethodEntry(httpRequest);
                if (httpRequest.Files.Count > 0)
                {
                    FileUploadHelper fileUploadHelper = new FileUploadHelper(executionContext);
                    fileUploadHelper.FileUpload();
                    log.LogMethodExit();
                    if (httpRequest.Form["EntityName"].ToString() == "DPLFILE")
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = MessageContainerList.GetMessage(executionContext, 1150) });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                    }
                }
                else
                {
                    log.LogMethodExit(null);
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

        [HttpGet]
        [Route("api/CommonServices/FileUpload")]
        [Authorize]
        public HttpResponseMessage Get(FileUploadHelper.FileType fileType, String fileName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (fileType != FileUploadHelper.FileType.BLANKACTIVITY)
                {
                    FileUploadHelper fileUploadHelper = new FileUploadHelper(executionContext);
                    var content = fileUploadHelper.DownloadImagesAsBase64Strings(fileType, fileName);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
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
