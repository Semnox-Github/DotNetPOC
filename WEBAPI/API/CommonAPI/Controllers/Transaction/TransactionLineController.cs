/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for getting the transaction line
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.100.0      10-Oct-2020   Nitin                Created
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
using System.Threading.Tasks;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionLineController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get Transaction Lines Object.
        /// </summary>
        [HttpGet]
        [Route("api/Transaction/TransactionLines")]
        public async Task<HttpResponseMessage> Get(int transactionId = -1, int lineNumber = -1, string transactionIdList = null, int pageNumber = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(transactionId, lineNumber, transactionIdList, pageNumber, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                TransactionLineListBL transactionLineListBL = new TransactionLineListBL(executionContext);
                List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>>();

                if (!string.IsNullOrEmpty(transactionIdList))
                {
                    searchParameters.Add(new KeyValuePair<TransactionLineDTO.SearchByParameters, string>(TransactionLineDTO.SearchByParameters.TRANSACTION_ID_LIST, transactionIdList));
                }

                if (transactionId != -1)
                {
                    searchParameters.Add(new KeyValuePair<TransactionLineDTO.SearchByParameters, string>(TransactionLineDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                    if (lineNumber != -1)
                    {
                        searchParameters.Add(new KeyValuePair<TransactionLineDTO.SearchByParameters, string>(TransactionLineDTO.SearchByParameters.LINE_ID, lineNumber.ToString()));
                    }
                }

                List<TransactionLineDTO> transactionLineDTOList = transactionLineListBL.GetTransactionLineDTOList(searchParameters);

                log.LogMethodExit(transactionLineDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionLineDTOList});
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
