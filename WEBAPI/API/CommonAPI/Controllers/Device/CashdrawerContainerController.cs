/********************************************************************************************
 * Project Name - Device                                                                    
 * Description  - Controller of the Cashdrawer
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.0    11-Aug-2021   Girish Kundar         Created : Multi Cashdrawer to POS  
 ********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.ViewContainer;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Device
{
    public class CashdrawerContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Device/CashdrawerContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                CashdrawerContainerDTOCollection locationContainerDTOCollection = await
                          Task<CashdrawerContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return CashdrawerViewContainerList.GetCashdrawerContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(locationContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = locationContainerDTOCollection });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    } 
}
