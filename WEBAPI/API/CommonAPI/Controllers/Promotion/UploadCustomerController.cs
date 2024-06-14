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
 *            22-Apr-2020   Mushahid Faizan Renamed ControllerName from CustomerServiceController to UploadCustomerController  Removed Token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Promotion
{
    public class UploadCustomerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Converts Customer to Sheet object response
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Promotion/UploadCustomers")]
        [Authorize]
        public HttpResponseMessage Get(string firstName = null, string middleName = null, string lastName = null, string uniqueIdentifier = null,
                                       string phone = null, string eMail = null, int memebershipId = -1, int siteId = -1)
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

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                CustomerSearchCriteria customerSearchCriteria = new CustomerSearchCriteria();
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
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet });
                }
                else
                {
                    customerListBL = new CustomerListBL(executionContext);
                    Sheet sheet = customerListBL.BuildTemplate();
                    log.LogMethodExit("Empty sheet ");
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet });
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
        /// Post the JSON String
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Promotion/UploadCustomers")]
        public HttpResponseMessage Post([FromBody] Sheet sheet)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(sheet);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                var resultSheet = customerListBL.BulkUpload(sheet);
                if (resultSheet.Item1.Rows.Count > 1)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = resultSheet });
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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

