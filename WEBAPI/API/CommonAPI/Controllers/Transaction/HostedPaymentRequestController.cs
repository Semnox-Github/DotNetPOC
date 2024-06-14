/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - ReservationStatusController
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.110        21-Nov-2020   Girish Kundar        Created :  Payment link enhancement
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class HostedPaymentRequestController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpDelete]
        [Route("api/Transaction/HostedPaymentRequests/{requestIdOrTransactionId}")]
        [Authorize]
        public HttpResponseMessage Delete([FromUri] string requestIdOrTransactionId)   // Format <RequestId , TransactionId>
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                log.LogMethodEntry(requestIdOrTransactionId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if(string.IsNullOrWhiteSpace(requestIdOrTransactionId))
                {
                    log.Error("Invalid request Id");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid request Id" });
                }
                /* 1. Request Id - <20>     */
                /* 2. TransactionId  - < ,102>    */
                /* 3. Both  <20,102>    */
                string requestId = string.Empty;
                string transactionId = string.Empty;
                int commaIndex = requestIdOrTransactionId.IndexOf(',');
                log.Debug("commaIndex :" + commaIndex);
                if (commaIndex == -1)
                {
                    string[] array = requestIdOrTransactionId.Split(',');
                    requestId = array[0];
                    log.Debug("requestId :" + requestId);
                }
                else
                {
                    char[] charArray = new char[] { ',' };
                    string[] array = requestIdOrTransactionId.Split(charArray, StringSplitOptions.RemoveEmptyEntries);
                    log.Debug("array :" + array);
                    if (array.Length == 1)
                    {
                         transactionId = array[0];
                         log.Debug("transactionId :" + transactionId);
                    }
                    else if(array.Length == 2)
                    {
                        requestId = array[0];
                        transactionId = array[1];
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid request Id or Transaction" });
                    }                  
                }
                CCRequestPGWDTO cCRequestPGWDTO = null; 
                CCRequestPGWListBL cCRequestPGWListBL = new CCRequestPGWListBL();
                List<System.Collections.Generic.KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParametersPGW = new List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>>();
                if(string.IsNullOrEmpty(requestId) == false)
                {
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.REQUEST_ID, requestId.ToString()));
                }
                if (string.IsNullOrEmpty(transactionId) == false)
                {
                    searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.INVOICE_NUMBER, transactionId.ToString()));
                }
                searchParametersPGW.Add(new KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>(CCRequestPGWDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<CCRequestPGWDTO> cCRequestsPGWDTOList = cCRequestPGWListBL.GetCCRequestPGWDTOList(searchParametersPGW);
                if (cCRequestsPGWDTOList != null && cCRequestsPGWDTOList.Any())
                {
                    cCRequestsPGWDTOList = cCRequestsPGWDTOList.OrderByDescending(ccReq => ccReq.RequestID).ToList(); // Check Date also if needed
                    cCRequestPGWDTO = cCRequestsPGWDTOList.FirstOrDefault();
                }
                if(cCRequestPGWDTO == null)
                {
                    log.Error("Invalid request Id");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Invalid request Id or Transaction" });
                }
                CCRequestPGWBL cCRequestPGWBL = new CCRequestPGWBL(executionContext, cCRequestPGWDTO.RequestID);
                cCRequestPGWBL.Delete();
                return Request.CreateResponse(HttpStatusCode.OK, new { data ="" });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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

