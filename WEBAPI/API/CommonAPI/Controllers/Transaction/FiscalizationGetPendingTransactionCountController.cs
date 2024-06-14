/********************************************************************************************
 * Project Name - CommnonAPI - Transaction Module                                                                     
 * Description  - Controller for Fiscalization Get Pending Transaction Count
 *
 **************
 **Version Log
  *Version         Date            Modified By          Remarks          
 *********************************************************************************************
 *2.155.1.0         11-Aug-20232   Guru S A            Created for Chile fiscalization
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Fiscalization;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Globalization;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.Transaction  
{
    /// <summary>
    /// FiscalizationGetPendingTransactionCountController
    /// </summary>
    public class FiscalizationGetPendingTransactionCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get FiscalizationPendingTransaction count.
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/Fiscalization/GetPendingTransactionsCount")]
        public async Task<HttpResponseMessage> Get(string fiscalization, DateTime? transactionFromDate = null, DateTime? tranasctionToDate = null, int transactionId = -1,
                                                    bool ignoreWIPTransactions = false)
        {
            log.LogMethodEntry(fiscalization, transactionFromDate, tranasctionToDate, transactionId, ignoreWIPTransactions);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null; 
            try
            {
                securityTokenDTO = GetSecurityToken();
                executionContext = SetExecutionContext(securityTokenDTO);
                Utilities utilities = SetUtilities(executionContext);  
                List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>>();
                DateTime startDate = DateTime.Now.Date;
                DateTime endDate = DateTime.Now;
                ParafaitFiscalizationNames parafaitFiscalizationListValue = FiscalizationFactory.GetParafaitFiscalizationNames(fiscalization);
                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>(FiscalizationPendingTransactionDTO.SearchParameters.TRX_ID, transactionId.ToString()));
                }
                else
                {
                    if (transactionFromDate != null)
                    { 
                        startDate = Convert.ToDateTime(transactionFromDate);
                        if (startDate == DateTime.MinValue)
                        {
                            string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                        }
                    }
                    else
                    {
                        double businessDayStartTime = !String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME")) ? ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME") : 6;
                        startDate = ServerDateTime.Now.Date.AddHours(businessDayStartTime);
                    }
                    if (tranasctionToDate != null)
                    { 
                        endDate = Convert.ToDateTime(tranasctionToDate);
                        if (endDate == DateTime.MinValue)
                        {
                            string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                            log.Error(customException);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                        }
                    }
                    else
                    {
                        endDate = ServerDateTime.Now;
                    }
                    if (startDate != null || endDate != null)
                    {
                        searchParameters.Add(new KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>(FiscalizationPendingTransactionDTO.SearchParameters.TRX_FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>(FiscalizationPendingTransactionDTO.SearchParameters.TRX_TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    }
                }

                IParafaitFiscalizationUseCases fiscalizationUseCases = FiscalizationUseCaseFactory.GetParafaitFiscalizationUseCases(executionContext, utilities);
                int totalNoOfTransactions = await fiscalizationUseCases.GetPendingTransactionCount(parafaitFiscalizationListValue, searchParameters); 
                log.LogMethodExit(totalNoOfTransactions);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfTransactions, token = securityTokenDTO.Token }); 
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });

            }
        }
        private static SecurityTokenDTO GetSecurityToken()
        {
            log.LogMethodEntry();
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO tokenDTO = securityTokenBL.GetSecurityTokenDTO;
            log.LogMethodExit("tokenDTO");
            return tokenDTO;
        }
        private static ExecutionContext SetExecutionContext(SecurityTokenDTO securityTokenDTO)
        {
            log.LogMethodEntry("securityTokenDTO");
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            log.LogMethodExit(executionContext);
            return executionContext;
        }
        private static Utilities SetUtilities(ExecutionContext executionContext)
        {
            log.LogMethodEntry("executionContext");
            Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
            Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
            Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
            Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
            log.LogMethodExit("Utilities");
            return Utilities;
        }
    }
}