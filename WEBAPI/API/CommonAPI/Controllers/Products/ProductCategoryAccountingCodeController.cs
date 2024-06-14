/********************************************************************************************
 * Project Name - Accounting Code Controller
 * Description  - Created to fetch, update and insert Accounting Code Controller in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        24-Jan-2019   Indrajeet Kumar          Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        20-Mar-2019   Akshay Gulaganji         Added customGenericException, passing executionContext 
 *2.70.0      20-Jun-2019   Nagesh Badiger           Modified Delete method for hard deletions.
 *2.80        05-Apr-2020    Girish Kundar           Modified: API end point and removed token from the body 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class ProductCategoryAccountingCodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON AccountingCode
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductCategoryAccountingCodes")]
        public HttpResponseMessage Get(string isActive, string categoryId)
        {
            try
            {
                log.LogMethodEntry(isActive, categoryId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>> searchParameters = new List<KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.SITEID, Convert.ToString(securityTokenDTO.SiteId)));
                if (!string.IsNullOrEmpty(categoryId))
                {
                    searchParameters.Add(new KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.OBJECTID, categoryId));
                }
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters, string>(AccountingCodeCombinationDTO.SearchByAccountingCodeCombinationParameters.ISACTIVE, isActive));
                }
                AccountingCodeCombinationList accountingCodeCombinationList = new AccountingCodeCombinationList(executionContext);
                var content = accountingCodeCombinationList.GetAllAccountingCode(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
        /// <summary>
        /// Post the JSON Object Accounting Code
        /// </summary>
        /// <param name="accountingCodeCombinationList">accountingCodeCombinationList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductCategoryAccountingCodes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<AccountingCodeCombinationDTO> accountingCodeCombinationList)
        {
            try
            {
                log.LogMethodEntry(accountingCodeCombinationList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (accountingCodeCombinationList != null || accountingCodeCombinationList.Count != 0)
                {
                    AccountingCodeCombinationList accountingCodeCombination = new AccountingCodeCombinationList(executionContext, accountingCodeCombinationList);
                    accountingCodeCombination.SaveUpdateAccountingCodeList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Delete the JSON Accounting Code
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductCategoryAccountingCodes")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<AccountingCodeCombinationDTO> accountingCodeCombinationList)
        {
            try
            {
                log.LogMethodEntry(accountingCodeCombinationList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (accountingCodeCombinationList != null || accountingCodeCombinationList.Count != 0)
                {
                    AccountingCodeCombinationList accountingCodeCombination = new AccountingCodeCombinationList(executionContext, accountingCodeCombinationList);
                    accountingCodeCombination.DeleteAccountingCodeList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
