/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for performing the transaction related operations
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80         26-Nov-2019    Nitin Pai            Created for Virtual store enhancement
 *2.130.9      16-Jun-2022    Guru S A             Execute online transaction changes in Kiosk
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionServicesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        [HttpGet]
        [Route("api/Transaction/TransactionServices")]
        public async Task<HttpResponseMessage> Get(PerformTransactionActivityDTO.ActivityType activityType, int transactionId = -1, string transactionOTP = null, bool onlineTransactionOnly = false,
                string originalSystemReference = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                this.Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                this.Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                this.Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                this.Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                this.Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                this.Utilities.ParafaitEnv.Initialize();

                switch (activityType)
                {
                    case PerformTransactionActivityDTO.ActivityType.VIRTUALSTORE_ONLINEPRINT:
                        {
                            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                            if (transactionId > -1)
                            {
                                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                            }

                            if (!string.IsNullOrEmpty(transactionOTP))
                            {
                                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_OTP, transactionOTP));
                            }

                            if (!string.IsNullOrEmpty(originalSystemReference))
                            {
                                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, originalSystemReference));
                            }

                            TransactionListBL transactionListBL = new TransactionListBL(executionContext);
                            IList<TransactionDTO> transactionDTOList = null;
                            transactionDTOList = await Task<List<TransactionDTO>>.Factory.StartNew(() =>
                            {
                                return transactionListBL.GetTransactionDTOList(searchParameters, Utilities, null, 0, 1, false, false, false);
                            });

                            if (transactionDTOList == null || !transactionDTOList.Any() || transactionDTOList.Count > 1)
                            {
                                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new Exception("Online Transaction Record not found."), executionContext);
                                log.Error(customException);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                            }

                            TransactionBL transactionBL = new TransactionBL(executionContext, transactionDTOList[0]);
                            transactionBL.BuildTransactionWithPrintDetails(Utilities);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = transactionBL.TransactionDTO, token = securityTokenDTO.Token });
                        }
                    default:
                        {
                            string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new Exception("Invalid Transaction Activity."), executionContext);
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                        }
                }
                //log.LogMethodExit();
                //return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });

            }
        }
        //api/Transaction/Transactions/{TransactionId}/Payments
        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        [Route("api/Transaction/{TransactionId}/TransactionServices")]
        [Authorize]
        public async Task<HttpResponseMessage> PrintVirtualStoreTransaction([FromUri] int TransactionId, [FromBody]List<Semnox.Parafait.Transaction.TransactionLineDTO> transactionListDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(TransactionId, transactionListDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITransactionUseCases transactionUseCases = TransactionUseCaseFactory.GetTransactionUseCases(executionContext);
                TransactionDTO result = await transactionUseCases.PrintVirtualStoreTransaction(TransactionId, transactionListDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
