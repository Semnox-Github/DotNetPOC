/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Media
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50       27-Sept-2018   Jagan Mohana Rao          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.DigitalSignage;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.DigitalSignage
{
    [Route("api/[controller]")]
    public class ContentMediaController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Media Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/ContentMedia/")]
        [Authorize]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionCcontext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchParameters = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, isActive));
                }
                searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));

                MediaList mediaList = new MediaList(executionCcontext);
                var content = mediaList.GetAllMedias(searchParameters);
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
        /// Performs a Post operation on media details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/ContentMedia/")]
        [Authorize]
        public HttpResponseMessage Post()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(HttpContext.Current.Request.Files, HttpContext.Current.Request.Form["MediaForm"]);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                var httpRequest = HttpContext.Current.Request;
                var formData = httpRequest.Form["MediaForm"];
                MediaDTO mediaDTOs = JsonConvert.DeserializeObject<MediaDTO>(formData);

                if (httpRequest.Files.Count > 0)
                {
                    var docfiles = new List<string>();
                    string filePath = string.Empty;
                    HttpPostedFile postedFile;
                    foreach (string file in httpRequest.Files)
                    {
                        postedFile = httpRequest.Files[file];
                        Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
                        string sharedFolderPath = utils.getParafaitDefaults("UPLOAD_DIRECTORY");

                        bool fileExist = true;
                        filePath = sharedFolderPath + '\\' + postedFile.FileName;
                        if (File.Exists(filePath))
                        {
                            fileExist = false;
                        }
                        if (fileExist == false && mediaDTOs.IsOverrideFile == false)
                        {
                            // The file already exists.Do you want replace a file?
                            throw new ValidationException(MessageContainer.GetMessage(executionContext, 1812));
                        }
                        else if (mediaDTOs.IsOverrideFile == true && mediaDTOs.IsChanged == true)
                        {
                            File.Delete(filePath);
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                        }
                        else
                        {
                            postedFile.SaveAs(filePath);
                            docfiles.Add(filePath);
                        }
                    }
                }
                if (mediaDTOs != null)
                {
                    Media media = new Media(executionContext, mediaDTOs);
                    // if mediaDTOs.MediaId is less than zero then insert or else update                   
                    media.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentMediaController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Conflict, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on media details.
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        //DELETE: api/Subscriber/
        [HttpDelete]
        [Route("api/DigitalSignage/ContentMedia/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MediaDTO> mediaListDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(mediaListDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (mediaListDTOs.Count != 0)
                {
                    MediaList media = new MediaList(mediaListDTOs, executionContext);
                    media.DeleteMediaList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.Debug("ContentMediaController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}