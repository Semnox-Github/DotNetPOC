/********************************************************************************************
 * Project Name - Common API
 * Description  - POSMachineContainerController class to get the List of 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 0.1         07-Dec-2020       Vikas Dwivedi             Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Parafait.POS;
using System.Threading.Tasks;
using Semnox.Parafait.ViewContainer;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.POS
{
    public class POSMachineContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON POS Management Machines.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/POSMachinesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                POSMachineContainerDTOCollection posMachineContainerDTOCollection = await
                           Task<POSMachineContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   POSMachineViewContainerList.Rebuild(siteId);
                               }
                               return POSMachineViewContainerList.GetPOSMachineContainerDTOCollection(siteId, hash);
                           });
                log.LogMethodExit(posMachineContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = posMachineContainerDTOCollection });
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