/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Cards module "Card CustomerDetails" entity. Created to fetch, update and insert the Cards Customer.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        04-Mar-2019     Mushahid Faizan     Created
 ********************************************************************************************/

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.Parafait.Cards
{
    public class CustomerDetailsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object Cards Customer Collections.
        /// </summary>       
        [HttpGet]
        [Route("api/Cards/CustomerDetails/")]
        public HttpResponseMessage Get(string isActive, string customerId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(isActive, customerId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.CUSTOMER_ID, customerId));

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                var customerContent = customerListBL.GetCustomerDTOList(searchParameters, true, true);
                string customerProfileImage = string.Empty;
                string customerIdImage = string.Empty;
                if (customerContent != null)
                {
                    foreach (CustomerDTO customerDTO in customerContent)
                    {
                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                        try
                        {
                            customerProfileImage = customerBL.GetCustomerImageBase64();
                        }
                        catch(Exception ex)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2405);
                            log.Error(message, ex);
                        }
                        try
                        {
                            customerIdImage = customerBL.GetIdImageBase64();
                        }
                        catch (Exception ex)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 2405);
                            log.Error(message, ex);
                        }
                    }
                }
                log.LogMethodExit(customerContent);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerContent, customersImage = customerProfileImage, customersIdImage = customerIdImage, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Post the JSON Object Cards Customer Collections.
        /// </summary>
        /// <param name="customerList"></param>
        [HttpPost]
        [Route("api/Cards/CustomerDetails/")]
        [Authorize]
        public HttpResponseMessage Post(JObject[] customerjObject)
        {
            try
            {
                log.LogMethodEntry(customerjObject);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CustomerDTO customerDTO = new CustomerDTO();
                AccountDTO accountDTO = new AccountDTO();
                List<CustomerDTO> customerDTOList = new List<CustomerDTO>();
                List<AccountDTO> accountDTOList = new List<AccountDTO>();
                if (customerjObject != null)
                {
                    /// This below logic is to get the customer and accounts details 
                    foreach (JObject jObject in customerjObject)
                    {
                        /// Get the JToken based on the jObject key
                        var jTokenCustomerObj = jObject.SelectTokens("customerDTO").ToList();
                        var jtokenAccountObj = jObject.SelectTokens("accountDTO").ToList();
                        if (jTokenCustomerObj != null)
                        {
                            JObject jobjectCustomerDTO = jTokenCustomerObj[0] as JObject;
                            // Copy to a static CustomerDTO instance
                            customerDTO = jobjectCustomerDTO.ToObject<CustomerDTO>();
                        }
                        if (jtokenAccountObj != null)
                        {
                            JObject jobjectAccountDTO = jtokenAccountObj[0] as JObject;
                            // Copy to a static AccountDTO instance
                            accountDTO = jobjectAccountDTO.ToObject<AccountDTO>();
                        }

                    }
                    if (customerDTO != null)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                                customerBL.Save(parafaitDBTrx.SQLTrx);
                                if (accountDTO != null)
                                {
                                    AccountBL accountBL = new AccountBL(executionContext, accountDTO.AccountId);
                                    if (accountBL.AccountDTO != null)
                                    {
                                        accountBL.AccountDTO.CustomerId = customerDTO.Id;
                                        accountBL.Save(parafaitDBTrx.SQLTrx);
                                    }
                                }
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (ValidationException valEx)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(valEx);
                                throw valEx;
                            }
                            catch (Exception ex)
                            {
                                parafaitDBTrx.RollBack();
                                log.Error(ex);
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw new Exception(ex.Message, ex);
                            }
                        }
                    }
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Delete the JSON Object Cards Customer Collections.
        /// </summary>
        /// <param name="customerList"></param>
        [HttpDelete]
        [Route("api/Cards/CustomerDetails/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<CustomerDTO> customerList)
        {
            try
            {
                log.LogMethodEntry(customerList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerList != null || customerList.Count != 0)
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext, customerList);
                    customerListBL.SaveUpdateCustomerList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
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
