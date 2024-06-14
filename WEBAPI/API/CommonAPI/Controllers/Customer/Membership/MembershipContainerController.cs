/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the MembershipContainer controller.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 2.200.0       07-Dec-2020   Vikas Dwivedi        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Membership;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Customer.Membership
{
    public class MembershipContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Membership/MembershipsContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                MembershipContainerDTOCollection membershipContainerDTOCollection = await
                           Task<MembershipContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               if (rebuildCache)
                               {
                                   MembershipViewContainerList.Rebuild(siteId);
                               }
                               return MembershipViewContainerList.GetMembershipContainerDTOCollection(siteId, hash);
                           });
                log.LogMethodExit(membershipContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = membershipContainerDTOCollection });
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
