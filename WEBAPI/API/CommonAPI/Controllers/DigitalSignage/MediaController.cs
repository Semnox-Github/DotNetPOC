/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Media
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.50       27-Sept-2018   Jagan Mohana Rao        Created 
 *2.90        29-Jul-2020    Mushahid Faizan        Modified : Renamed Controller from ContentMediaController to MediaController
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.DigitalSignage
{

    public class MediaController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Media Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/Media")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, string fileName = null, string mediaName = null, int mediaTypeId = -1, int mediaId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>> searchParameters = new List<KeyValuePair<MediaDTO.SearchByMediaParameters, string>>();
                searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.IS_ACTIVE, isActive));
                    }
                }

                if (!string.IsNullOrEmpty(fileName))
                {
                    searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.FILE_NAME, fileName));
                }
                if (!string.IsNullOrEmpty(mediaName))
                {
                    searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.NAME, mediaName));
                }
                if (mediaTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.TYPE_ID, mediaTypeId.ToString()));
                }
                if (mediaId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MediaDTO.SearchByMediaParameters, string>(MediaDTO.SearchByMediaParameters.MEDIA_ID, mediaId.ToString()));
                }

                MediaList mediaList = new MediaList(executionContext);
                var content = mediaList.GetAllMedias(searchParameters);
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


        [HttpPost]
        [Route("api/DigitalSignage/Media")]
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
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1812));
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }

        }
    }
}