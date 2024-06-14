/********************************************************************************************
 * Project Name - ImagesController
 * Description  - API to return images
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Controllers.Common
{
    public class ImagesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of MachineDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/Images")]
        public HttpResponseMessage Get(string imageType = null, string imageName = null, DateTime? lastModifiedDate = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(imageType);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<String> imageList = new List<string>();
                String sharedFolderPath = "";
                try
                {
                    if(!string.IsNullOrEmpty(imageType) && string.IsNullOrEmpty(imageName))
                    {
                        sharedFolderPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, imageType);
                        try
                        {
                            foreach (string filePath in Directory.GetFiles(sharedFolderPath, "*.*"))
                            {
                                if (filePath.Contains(".bmp") || filePath.Contains(".jpeg") || filePath.Contains(".png") || filePath.Contains(".jpg"))
                                {
                                    if (lastModifiedDate != null)
                                    {
                                        DateTime lastWriteTime = System.IO.File.GetLastWriteTime(filePath);
                                        if(lastWriteTime > Convert.ToDateTime(lastModifiedDate))
                                        {
                                            imageList.Add(Path.GetFileName(filePath));
                                        }
                                    }
                                    else
                                    {
                                        imageList.Add(Path.GetFileName(filePath));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Debug(ex.Message);
                            throw;
                        }
                    }
                    else if(!string.IsNullOrEmpty(imageName))
                    {
                        byte[] buffer;

                        if(!string.IsNullOrEmpty(imageType))
                            sharedFolderPath = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, imageType);

                        String fullFileName = sharedFolderPath + '\\' + imageName;
                        using (FileStream fileStream = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
                        {
                            try
                            {
                                int length = (int)fileStream.Length;  // get file length
                                buffer = new byte[length];            // create buffer
                                int count;                            // actual number of bytes read
                                int sum = 0;                          // total number of bytes read

                                // read until Read method returns 0 (end of the stream has been reached)
                                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                                    sum += count;  // sum is a buffer offset for next reading
                            }
                            finally
                            {
                                fileStream.Close();
                            }
                        }

                        String base64 = Convert.ToBase64String(buffer);
                        imageList.Add(base64);
                    }
                    else
                    {
                        string customException = "Invalid Inputs";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = customException });
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    throw new Exception(ex.Message);
                }


                log.LogMethodExit(imageList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = imageList });
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
