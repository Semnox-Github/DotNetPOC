/**************************************************************************************************
 * Project Name - Reports 
 * Description  - Controller for ViewTransactions - TransactionDiscounts
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
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Reports
{
    public class TransactionDiscountReportsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of TransactionDiscountsDTO
        /// </summary>
        /// <returns>HttpResponseMessage</returns>

        [HttpGet]
        [Authorize]
        [Route("api/Report/TransactionDiscountReports")]
        public HttpResponseMessage Get(int transactionDiscountId = -1, int transactionId = -1, int lineId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionDiscountId, transactionId, lineId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>> trxDiscountsSearchParameter = new List<KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>>();
                trxDiscountsSearchParameter.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (transactionDiscountId > -1)
                {
                    trxDiscountsSearchParameter.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.TRANSACTION_DISCOUNT_ID, Convert.ToString(transactionDiscountId)));
                }
                if (transactionId > -1)
                {
                    trxDiscountsSearchParameter.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.TRANSACTION_ID, Convert.ToString(transactionId)));
                }
                if (lineId > -1)
                {
                    trxDiscountsSearchParameter.Add(new KeyValuePair<TransactionDiscountsDTO.SearchByParameters, string>(TransactionDiscountsDTO.SearchByParameters.LINE_ID, Convert.ToString(lineId)));
                }
                TransactionDiscountsListBL transactionDiscountsListBL = new TransactionDiscountsListBL(executionContext);
                List<TransactionDiscountsDTO> transactionDiscountsDTOList = transactionDiscountsListBL.GetTransactionDiscountsDTOList(trxDiscountsSearchParameter);

                log.LogMethodExit(transactionDiscountsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionDiscountsDTOList });
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
