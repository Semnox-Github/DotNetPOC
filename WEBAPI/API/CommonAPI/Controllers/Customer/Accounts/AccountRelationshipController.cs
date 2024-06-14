/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Controller for AccountRelationships
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.Controllers.Customer.Accounts
{
    public class AccountRelationshipController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpGet]
        [Authorize]
        [Route("api/Customer/Account/AccountRelationships")]
        public async Task<HttpResponseMessage> Get(int accountId, int isActive)
        {
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>>();
                if(accountId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.ACCOUNT_ID, accountId.ToString()));
                }
                if (isActive == 0)
                {
                    searchParameters.Add(new KeyValuePair<AccountRelationshipDTO.SearchByParameters, string>(AccountRelationshipDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountRelationshipDTO> result = await accountUseCases.GetAccountRelationships(searchParameters);
                if (result != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = new List<AccountRelationshipDTO>() });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }          
        }

        [HttpPut]
        [Authorize]
        [Route("api/Customer/Account/AccountRelationships")]
        public async Task<HttpResponseMessage> Put([FromBody] List<AccountRelationshipDTO> accountRelationshipDTOs)
        {
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<AccountRelationshipDTO> result = await accountUseCases.UpdateAccountRelationships(accountRelationshipDTOs);
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Customer/Account/AccountRelationships")]
        public async Task<HttpResponseMessage> Post([FromBody] List<LinkNewCardDTO> linkNewCardDTOs)
        {
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IAccountUseCases accountUseCases = AccountUseCaseFactory.GetAccountUseCases(executionContext);
                List<LinkNewCardDTO> result = await accountUseCases.CreateAccountRelationships(linkNewCardDTOs);
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

    }
}