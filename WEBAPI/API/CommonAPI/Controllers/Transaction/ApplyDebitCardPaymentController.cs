/**************************************************************************************************
 * Project Name - Transaction 
 * Description  - ApplyDebitCardPaymentController applies the debit card payment to the transaction
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.152.0     08-Mar-2024       Nitin                     Created
 **************************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Transaction
{
    public class ApplyDebitCardPaymentController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of ApplyDebitCardPaymentController
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/ApplyDebitCardPayment")]
        public HttpResponseMessage PostTransaction(TransactionDTO transactionDTO)
        {

            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(transactionDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (transactionDTO != null)
                {
                    TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTO);
                    TransactionDTO TransactionDTO = transactionBL.ApplyDebitCardPayment();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = TransactionDTO });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong input" });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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

