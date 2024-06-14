﻿/********************************************************************************************
 * Project Name - AccountSummary
 * Description  - LinkedAccountSummaryController class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.130.11    23-Sep-2022       Yashodhara C H           Created 
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.CommonAPI.Helpers;
using System.Collections.Generic;

namespace Semnox.CommonAPI.Customer.Accounts
{
    public class LinkedAccountSummaryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Cards Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Account/LinkedAccountsSummary")]
        public async Task<HttpResponseMessage> Get(int customerId)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerId);

                // Prevent a blank search of accounts
                if (customerId == -1)
                {
                    log.Fatal("Blank search encountered. Forcing an error. " + this.Request.Content);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = "Invalid Search Parameters" });
                }

                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ILinkedAccountUseCases linkedAccountUseCases = LinkedAccountUseCaseFactory.GetLinkedAccountUseCases(executionContext);
                //AccountsSummary Not View DTO
                List<AccountSummaryViewDTO> accountSummaryViewDTO = await linkedAccountUseCases.GetLinkedAccountSummaryViewDTO(customerId);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountSummaryViewDTO });
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
