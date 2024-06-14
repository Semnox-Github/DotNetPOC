/********************************************************************************************
 * Project Name - CommnonAPI - Transaction Module                                                                     
 * Description  - Controller for posting trx external system reference and Remarks
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.140.6      30-May-2023   Deeksha              Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System.Globalization;
using System.Threading.Tasks;
using Semnox.Parafait.Customer;
using System.Linq;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TrxExtSysRefController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Transfer entitlements from parent card to child card
        /// </summary>
        [HttpPost]
        [Route("api/Transaction/{transactionId}/ExternalSysReference")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int transactionId, string externalSystemReference, string remarks)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalSystemReference, remarks);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (transactionId != -1)
                {
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionId);
                    TransactionDTO transactionDTO = transactionBL.TransactionDTO;
                    if(transactionDTO != null)
                    {
                        transactionBL.UpdateTransactionRemarksAndExtReference(transactionDTO.Guid,
                            externalSystemReference, remarks,null);
                    }
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty});
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input"});
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = customException});
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
