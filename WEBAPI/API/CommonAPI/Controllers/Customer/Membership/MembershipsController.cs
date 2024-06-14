/********************************************************************************************
 * Project Name - MembershipController
 * Description  - Created to fetch, update and insert Membership.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.70.2      06-Mar-2020   Nitin Pai                Created.
 *2.130.3     16-Dec-2021   Abhishek                 WMS fix : Added two parameters loadChildRecords,loadActiveChildRecords
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Membership;

namespace Semnox.CommonAPI.Controllers.Customer.Membership
{
    public class MembershipsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON Object Membership Collections.
        /// </summary>       
        [HttpGet]
        [Authorize]
        [Route("api/Customer/Membership/Memberships")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int membershipId = -1, int siteId = -1, string membershipName = null, bool loadChildRecords = false, bool loadActiveChildRecords = false)
        {
            try
            {
                log.LogMethodEntry(isActive, membershipId, siteId ,membershipName, loadChildRecords, loadActiveChildRecords);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<MembershipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MembershipDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.SITE_ID, siteId == -1 ? executionContext.GetSiteId().ToString() : siteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }

                if (membershipId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.MEMBERSHIP_ID, membershipId.ToString()));
                }
                if (!String.IsNullOrEmpty(membershipName))
                {
                    searchParameters.Add(new KeyValuePair<MembershipDTO.SearchByParameters, string>(MembershipDTO.SearchByParameters.MEMBERSHIP_NAME, membershipName));
                }
                IMembershipUseCases membershipUseCases = CustomerUseCaseFactory.GetMembershipUseCases(executionContext);
                List<MembershipDTO> content = await membershipUseCases.GetAllMemberships(searchParameters, loadChildRecords, loadActiveChildRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }

        /// <summary>
        /// Post the JSON Object Memberships
        /// </summary>
        /// <param name="membershipDTOList">MembershipsList</param>
        [HttpPost]
        [Route("api/Customer/Membership/Memberships")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<MembershipDTO> membershipDTOList)
        {
            try
            {
                log.LogMethodEntry(membershipDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (membershipDTOList != null && membershipDTOList.Any())
                {
                    IMembershipUseCases membershipUseCases = CustomerUseCaseFactory.GetMembershipUseCases(executionContext);
                    await membershipUseCases.SaveAllMembership(membershipDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""});
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid Input"});
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException});
            }
        }
    }
}
