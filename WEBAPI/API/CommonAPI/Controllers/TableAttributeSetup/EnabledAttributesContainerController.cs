/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the EnabledAttributesContainer.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    20-Aug-2021     Fiona               Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using Semnox.Parafait.TableAttributeSetup;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.TableAttributeSetup
{
    public class EnabledAttributesContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Authorize]
        [Route("api/TableAttributeSetup/EnabledAttributesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            ExecutionContext executionContext = null;
            try
            {
                EnabledAttributesContainerDTOCollection EnabledAttributesContainerDTOCollection = await
                          Task<EnabledAttributesContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return EnabledAttributesViewContainerList.GetEnabledAttributesContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(EnabledAttributesContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = EnabledAttributesContainerDTOCollection });
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