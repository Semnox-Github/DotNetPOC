/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - FileServiceController used to get and save files.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.0      08-Sep-2021      Abhishek                  Created
 ********************************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.FileResources;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Games.Controllers.Common
{
    public class FileResourceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Route("api/Common/FileResource")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string defaultValueName = null, string fileName = null, bool secure = false)
        {
            log.LogMethodEntry(defaultValueName, fileName);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (string.IsNullOrWhiteSpace(defaultValueName) ||
                    string.IsNullOrWhiteSpace(fileName))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid default value name or file name" });
                }
                FileResource fileResource = FileResourceFactory.GetFileResource(executionContext, defaultValueName, fileName, secure);
                Stream inputStream = await fileResource.Get();
                var response = Request.CreateResponse();
                response.Content = new PushStreamContent(async (outputStream, httpContent, transportContext) => 
                                                          {
                                                              try
                                                              {
                                                                  await inputStream.CopyToAsync(outputStream);
                                                              }
                                                              catch (HttpException ex)
                                                              {
                                                                  return;
                                                              }
                                                              finally
                                                              {
                                                                  outputStream.Dispose();
                                                                  inputStream.Dispose();
                                                              }
                                                          }, new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(fileName)));
                log.LogMethodExit();
                return response;
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = ex.Message;
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        [HttpPost]
        [Route("api/Common/FileResource")]
        [Authorize]
        public async Task<HttpResponseMessage> POST(string defaultValueName, string fileName, bool secure = false)
        {
            log.LogMethodEntry(defaultValueName, fileName, secure);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                FileResource fileResource = FileResourceFactory.GetFileResource(executionContext, defaultValueName, fileName, secure);
                bool result = await fileResource.Save(HttpContext.Current.Request.GetBufferlessInputStream(true));
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = ex.Message;
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}