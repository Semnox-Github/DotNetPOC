/********************************************************************************************
 * Project Name - Recieve                                                                          
 * Description  -  Return the sheet object for Customer
 * Entity : Promotions => Customers export import and upload
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80        19-Dec-2019   Vikas Dwivedi  Created
 *            24-Dec-2019   Jagan Mohana   Modified sheet object loop fro export customers         
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Promotions
{
    public class CustomerServiceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Converts Customer to Sheet object response
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Promotions/CustomerService/")]
        [Authorize]
        public HttpResponseMessage Get(string firstName = null, string middleName = null, string lastName = null, string uniqueIdentifier = null, string phone = null, string eMail = null, int memebershipId = -1, int siteId = -1)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
                //List<KeyValuePair<CustomerSearchCriteria.CustomerSearchByParameters, string>> purchaseOrderSearchParameter = new List<KeyValuePair<customerSearchCriteria.CustomerSearchByParameters, string>>();
                if (!string.IsNullOrEmpty(firstName))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, "%" + firstName + "%");
                }
                if (!string.IsNullOrEmpty(middleName))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_MIDDLE_NAME, Operator.LIKE, "%" + middleName + "%");
                }
                if (!string.IsNullOrEmpty(lastName))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_LAST_NAME, Operator.LIKE, "%" + lastName + "%");
                }
                if (!string.IsNullOrEmpty(uniqueIdentifier))
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.LIKE, "%" + uniqueIdentifier + "%");
                }
                if (siteId > 0)
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_SITE_ID, Operator.EQUAL_TO, siteId);
                }
                if (memebershipId > 0)
                {
                    customerSearchCriteria.And(CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID, Operator.EQUAL_TO, memebershipId);
                }
                if (!string.IsNullOrEmpty(phone))
                {
                    customerSearchCriteria.And(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "PHONE")
                                                .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, "%" + phone + "%"));
                }
                if (!string.IsNullOrEmpty(eMail))
                {
                    customerSearchCriteria.Or(new CustomerSearchCriteria(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, "EMAIL")
                                                .And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, "%" + eMail + "%"));
                }
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(customerSearchCriteria, true, true);
                if (customerDTOList != null && customerDTOList.Any())
                {
                    CustomerDTODefinition customerDTODefinition = new CustomerDTODefinition(executionContext, "");
                    foreach (CustomerDTO customerDTO in customerDTOList)
                    {
                        customerDTODefinition.Configure(customerDTO);
                    }
                    Sheet sheet = new Sheet();
                    Row headerRow = new Row();
                    customerDTODefinition.BuildHeaderRow(headerRow);
                    sheet.AddRow(headerRow);
                    foreach (CustomerDTO customerDTO in customerDTOList)
                    {
                        Row row = new Row();
                        customerDTODefinition.Serialize(row, customerDTO);
                        sheet.AddRow(row);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet, token = securityTokenDTO.Token });
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

        /// <summary>
        /// Post the JSON String
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Promotions/CustomerService/")]
        public HttpResponseMessage Post([FromBody]Sheet sheet, string activityType)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                CustomerListBL customerListBL = new CustomerListBL(executionContext);

                switch (activityType.ToUpper().ToString())
                {
                    case "TEMPLATE":
                        customerListBL.BuildTemplate();
                        break;
                    case "UPLOAD":
                        if (sheet != null)
                        {
                            customerListBL.BulkUpload(sheet);
                        }
                        break;
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", securityTokenDTO.Token });
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

