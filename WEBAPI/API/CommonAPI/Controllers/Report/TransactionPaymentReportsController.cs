/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for ViewTransactions - TransactionPayments
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By               Remarks          
 **************************************************************************************************
 *2.90        07-Jun-2020       Vikas Dwivedi             Created to Get Methods.
 **************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;
namespace Semnox.CommonAPI.Reports
{
    public class TransactionPaymentReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of TransactionPaymentsDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Report/TransactionPaymentReports")]
        public HttpResponseMessage Get(int paymentId = -1, int transactionId = -1, int paymentModeId = -1, int cardId = -1, int orderId = -1, int cCResponseId = -1, int parentPaymentId = -1, int splitId = -1, string posMachine = null, string lastUpdatedUser = null, string creditCardAuthorization = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(paymentId, transactionId, paymentModeId, cardId, orderId, cCResponseId, parentPaymentId, splitId, posMachine, lastUpdatedUser, creditCardAuthorization);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> trxPaymentSearchParameter = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (paymentId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_ID, Convert.ToString(paymentId)));
                }
                if (transactionId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, Convert.ToString(transactionId)));
                }
                if (paymentModeId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, Convert.ToString(paymentModeId)));
                }
                if (cardId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CARD_ID, Convert.ToString(cardId)));
                }
                if (orderId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.ORDER_ID, Convert.ToString(orderId)));
                }
                if (cCResponseId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CCRESPONSE_ID, Convert.ToString(cCResponseId)));
                }
                if (parentPaymentId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PARENT_PAYMENT_ID, Convert.ToString(parentPaymentId)));
                }
                if (splitId > -1)
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.SPLIT_ID, Convert.ToString(splitId)));
                }
                if (!string.IsNullOrEmpty(posMachine))
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.POS_MACHINE, posMachine.ToString()));
                }
                if (!string.IsNullOrEmpty(lastUpdatedUser))
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.LAST_UPDATED_USER, lastUpdatedUser.ToString()));
                }
                if (!string.IsNullOrEmpty(creditCardAuthorization))
                {
                    trxPaymentSearchParameter.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.CREDIT_CARD_AUTHORIZATION, creditCardAuthorization.ToString()));
                }
                TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(executionContext);
                List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetTransactionPaymentsDTOList(trxPaymentSearchParameter);

                log.LogMethodExit(transactionPaymentsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionPaymentsDTOList });
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
