/********************************************************************************************
 * Project Name -GenericUtilities
 * Description  -FileResourceHashController to get file information 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00     21-Sep-2021       Lakshminarayana           Created
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.FileResources;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Games.Controllers.Common
{
    public class FileResourceHashController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpGet]
        [Route("api/Common/FileResourceHash")]
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
                string hash = await fileResource.GetHash();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = hash });
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