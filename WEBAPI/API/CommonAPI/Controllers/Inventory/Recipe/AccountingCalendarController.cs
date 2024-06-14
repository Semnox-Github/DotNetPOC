/********************************************************************************************
* Project Name - CommonAPI
* Description  - AccountingCalendarController - Created to get the Accounting Calendar details
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00     06-Nov-2020     Abhishek              Created : As part of Inventory UI Redesign
      
********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Inventory.Recipe
{
    public class AccountingCalendarController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object of AccountingCalendarMasterDTO
        /// </summary>
        /// <param name="accountingCalendarMasterId">accountingCalendarMasterId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Recipe/AccountingCalendars")]
        public async Task<HttpResponseMessage> Get(int accountingCalendarMasterId = -1, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                log.LogMethodEntry(accountingCalendarMasterId, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>> accountingCalendarSearchParameter = new List<KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>>();
                accountingCalendarSearchParameter.Add(new KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>(AccountingCalendarMasterDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (accountingCalendarMasterId > 0)
                {
                    accountingCalendarSearchParameter.Add(new KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>(AccountingCalendarMasterDTO.SearchByParameters.ACCOUNTING_CALENDAR_MASTER_ID, Convert.ToString(accountingCalendarMasterId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {

                        accountingCalendarSearchParameter.Add(new KeyValuePair<AccountingCalendarMasterDTO.SearchByParameters, string>(AccountingCalendarMasterDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                IAccountingCalendarUseCases accountingCalendarUseCases = RecipeUseCaseFactory.GetAccountingCalendarUseCases(executionContext);
                List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList = await accountingCalendarUseCases.GetAccountingCalendars(accountingCalendarSearchParameter);
                log.LogMethodExit(accountingCalendarMasterDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = accountingCalendarMasterDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of AccountingCalendarMasterDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/AccountingCalendars")]
        public async Task<HttpResponseMessage> Post([FromBody] List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountingCalendarMasterDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (accountingCalendarMasterDTOList != null && accountingCalendarMasterDTOList.Any())
                {
                    IAccountingCalendarUseCases accountingCalendarUseCases = RecipeUseCaseFactory.GetAccountingCalendarUseCases(executionContext);
                    await accountingCalendarUseCases.SaveAccountingCalendars(accountingCalendarMasterDTOList);
                    log.LogMethodExit(accountingCalendarMasterDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
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
        /// <summary>
        /// Post the JSON Object of AccountingCalendarMasterDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Recipe/AccountingCalendars")]
        public async Task<HttpResponseMessage> Put([FromBody] List<AccountingCalendarMasterDTO> accountingCalendarMasterDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(accountingCalendarMasterDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (accountingCalendarMasterDTOList == null || accountingCalendarMasterDTOList.Any(x => x.AccountingCalendarMasterId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                if (accountingCalendarMasterDTOList != null && accountingCalendarMasterDTOList.Any())
                {
                    IAccountingCalendarUseCases accountingCalendarUseCases = RecipeUseCaseFactory.GetAccountingCalendarUseCases(executionContext);
                    await accountingCalendarUseCases.SaveAccountingCalendars(accountingCalendarMasterDTOList);
                    log.LogMethodExit(accountingCalendarMasterDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = accountingCalendarMasterDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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
