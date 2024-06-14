/********************************************************************************************
 * Project Name - LoggingController                                                                         
 * Description  - Controller for the Logging the all log exceptions coming from UI.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.50.0        03-Dec-2018    Jagan Mohana Rao          Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Linq;

namespace Semnox.CommonAPI.CommonServices
{
    public class LoggingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Post method to log all exceptions
        /// </summary>
        /// <param name="logMessage"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Logging/Log/")]
        public HttpResponseMessage Post([FromBody] string logMessage)
        {
            try
            {
                log.LogMethodEntry(logMessage);
                log.Info("Logging controller log " + logMessage);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
            catch (Exception ex)
            {
                log.LogMethodEntry(ex.Message);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
        }
    }
}
