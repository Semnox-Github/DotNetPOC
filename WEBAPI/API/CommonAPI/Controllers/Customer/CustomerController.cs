/********************************************************************************************
 * Project Name - Customer Controller
 * Description  - Controller for Customer Resource
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version
 *2.80        15-Oct-2019      Nitin Pai      Customer registration related changes
 *2.80        20-Dec-2019      Mushahid Faizan Modified for Customer registration related changes
 *2.80        15-Apr-2019      Nitin Pai      Fun City App fixes 
 *2.80        05-Jun-2020      Girish Kundar  Modified : Replaced the search by parameters object with search criteria to build search query 
 *2.100       12-Oct-2020      Nitin Pai      Modified : Nick China Enhancement, link cutomer to card
 *2.120.0     15-Mar-2021      Prajwal S      Modified: Added Get for CustomerSummaryDTO. 
 *2.130.7     23-Apr-2022      Nitin Pai      Modified: Adding DB sync Entries for customer object after SQL transaction is commited
 *2.130.10    02-Sep-2022      Nitin Pai      Modified: Phone or email will go by Hash Column Value. (Reverted)Address field is truncated to 50 chars
 *2.130.11    08-Sep-2022      Nitin Pai      Modified: Added customer delete end point. Modified others for customer activity user log enhancement.
 *2.150.0    12-Dec-2022       Abhishek       Modified : Added parameters waivercode, source, customerIdList for waiver 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Transaction;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.User;
using Semnox.Parafait.Languages;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Semnox.Core.Utilities.Utilities utilities;

        /// <summary>   
        /// Get the Customer JSON by Customer Id.
        /// </summary>       
        [HttpGet]
        [Route("api/Customer/Customers")]
        public async Task<HttpResponseMessage> Get(int customerId = -1, String contactNumber = null, String emailId = null, String firstName = null, String lastName = null, DateTime? fromDate = null, DateTime? toDate = null,
                                       int pageNumber = 0, int pageSize = 0, bool buildChildRecords = false, bool activeRecordsOnly = false, bool profilePic = false, bool idPic = false, bool buildActiveCampaignActivity = false,
                                       bool buildLastVistitedDate = false, string phoneOrEmail = null, bool loadAdultOnly = false, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false, //string registrationToken = null,
                                       string customerGUID = null, int customerMembershipId = -1, string uniqueIdentifier = null, string middleName = null, 
                                       string exactContactNumber = null, string exactEmailId = null, string waiverCode = null, string source = null, string customerIdList = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(customerId, contactNumber, emailId, firstName, lastName, fromDate, toDate, pageNumber, pageSize, buildChildRecords, activeRecordsOnly, profilePic, 
                    idPic, buildActiveCampaignActivity, buildLastVistitedDate, phoneOrEmail, loadAdultOnly, loadSignedWaivers, loadSignedWaiverFileContent, customerGUID , customerMembershipId, 
                    uniqueIdentifier , middleName ,  exactContactNumber , exactEmailId, waiverCode, source, customerIdList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria();
                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);
                DateTime lastUpdateDate = DateTime.Now;

                bool getCustomerCount = true;

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }
                else
                {
                    endDate = utilities.getServerTime();
                }

                if (activeRecordsOnly)
                {
                    searchCriteria.And(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, "1");
                }


                if (!String.IsNullOrEmpty(phoneOrEmail))
                {
                    phoneOrEmail = phoneOrEmail.Trim();
                    int phoneNumberWidth = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "CUSTOMER_PHONE_NUMBER_WIDTH", 0);
                    if (phoneNumberWidth > 0 && phoneOrEmail.Length > phoneNumberWidth && phoneOrEmail.IndexOf("@") == -1)
                    {
                        log.Error("uncorrected phone number " + phoneOrEmail);
                        phoneOrEmail = phoneOrEmail.Substring(phoneOrEmail.Length - phoneNumberWidth);
                        log.Error("corrected phone number " + phoneOrEmail);
                    }
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    if (phoneOrEmail.IndexOf("@") == -1)
                        searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                    else
                        searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString());

                    searchCriteria.And(CustomerSearchByParameters.CONTACT_PHONE_OR_EMAIL, Operator.EQUAL_TO, phoneOrEmail);
                    getCustomerCount = false;
                }

                if (!String.IsNullOrEmpty(contactNumber))
                {
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, contactNumber);
                }

                if (!String.IsNullOrEmpty(emailId))
                {
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString());
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, emailId);
                }

                if (customerId > -1)
                {
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_ID, Operator.EQUAL_TO, customerId.ToString());
                }

                if (!String.IsNullOrEmpty(firstName))
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_FIRST_NAME, Operator.LIKE, firstName.ToString());
                }

                if (!String.IsNullOrEmpty(lastName))
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_LAST_NAME, Operator.LIKE, lastName.ToString());
                }
                if (!String.IsNullOrEmpty(middleName))
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_MIDDLE_NAME, Operator.LIKE, middleName.ToString());
                }

                if (loadAdultOnly)
                {
                    log.Debug("Adding adult check");
                    String dobMandatory = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE", "");

                    LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                    DateTime majorityAge = serverTimeObject.GetServerDateTime().AddYears(-1 * ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AGE_OF_MAJORITY", 0));
                    if (!dobMandatory.Equals("M"))
                    {
                        log.Debug("DOB is not mandatory " + majorityAge.ToString("yyyy-MM-dd HH:mm:ss") + ":" + serverTimeObject.GetServerDateTime().Date.ToString("yyyy-MM-dd HH:mm:ss"));
                        searchCriteria.And(new CustomerSearchCriteria(
                                CustomerSearchByParameters.IS_ADULT, Operator.LESSER_THAN_OR_EQUAL_TO, majorityAge)
                                .Or(CustomerSearchByParameters.IS_ADULT, Operator.EQUAL_TO, serverTimeObject.GetServerDateTime().Date));
                    }
                    else
                    {
                        log.Debug("DOB is mandatory " + majorityAge.ToString("yyyy-MM-dd HH:mm:ss"));
                        searchCriteria.And(CustomerSearchByParameters.IS_ADULT, Operator.LESSER_THAN_OR_EQUAL_TO, majorityAge);
                    }
                }

                if (!String.IsNullOrEmpty(customerGUID))
                {
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_GUID, Operator.EQUAL_TO, customerGUID.ToString());
                }
                if (customerMembershipId > -1)
                {
                    searchCriteria.And(CustomerSearchByParameters.CUSTOMER_MEMBERSHIP_ID, Operator.EQUAL_TO, customerMembershipId.ToString());
                }

                if (!string.IsNullOrWhiteSpace(uniqueIdentifier))
                {
                    searchCriteria.And(CustomerSearchByParameters.PROFILE_UNIQUE_IDENTIFIER, Operator.EQUAL_TO, uniqueIdentifier.ToString());
                }

                if (!String.IsNullOrEmpty(exactContactNumber))
                {
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, exactContactNumber);
                }

                if (!String.IsNullOrEmpty(exactEmailId))
                {
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.EMAIL.ToString());
                    searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.EQUAL_TO, exactEmailId);
                }
                if (!String.IsNullOrEmpty(waiverCode))
                {
                    searchCriteria.And(CustomerSearchByParameters.WAIVER_CODE, Operator.EQUAL_TO, waiverCode.ToString());
                }
                if (!String.IsNullOrEmpty(source))
                {
                    searchCriteria.And(CustomerSearchByParameters.CHANNEL, Operator.EQUAL_TO, source.ToString());
                }
                IList<CustomerDTO> customers = null;
                if (!string.IsNullOrEmpty(customerIdList))
                {
                    List<int> customerList = customerIdList.Split(',').Select(int.Parse).ToList();
                    customers = customerListBL.GetCustomerDTOList(customerList, buildChildRecords, activeRecordsOnly, loadSignedWaivers,
                         null, loadSignedWaiverFileContent, utilities);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customers});
                }
                int totalNoOfPages = 0;
                int totalNoOfCustomers = 0;
                // Perf Change: get the customer count only for WMS for other scenarios this is not required
                if (getCustomerCount)
                    totalNoOfCustomers = await Task<int>.Factory.StartNew(() => { return customerListBL.GetCustomerCount(searchCriteria, null); });
                else
                    totalNoOfCustomers = 1;

               
                if (totalNoOfCustomers > 0)
                {
                    log.LogVariableState("totalNoOfCustomer", totalNoOfCustomers);

                    pageSize = pageSize > 500 || pageSize == 0 ? 500 : pageSize;
                    totalNoOfPages = (totalNoOfCustomers / pageSize) + ((totalNoOfCustomers % pageSize) > 0 ? 1 : 0);
                    pageNumber = pageNumber < -1 || pageNumber > totalNoOfPages ? 0 : pageNumber;

                    customers = await Task<List<CustomerDTO>>.Factory.StartNew(() =>
                    {
                        return customerListBL.GetCustomerDTOList(searchCriteria, pageNumber, pageSize, buildChildRecords, activeRecordsOnly, loadSignedWaivers, null, loadSignedWaiverFileContent, utilities, buildActiveCampaignActivity: buildActiveCampaignActivity,
                            buildLastVistitedDate: buildLastVistitedDate, fromDate: startDate, toDate: endDate);
                    });

                    if (customers != null)
                    {
                        String json = JsonConvert.SerializeObject(customers);
                        customers = new SortableBindingList<CustomerDTO>(customers);
                        customers = customers.OrderBy(x => x.Id).ToList();
                        json = JsonConvert.SerializeObject(customers);
                    }

                }

                if (customers != null && customers.Count > 0)
                {
                    string customerProfileImage = "";
                    string customerIdImage = "";
                    if (customerId > -1 || customers.Count == 1)
                    {
                        CustomerDTO customerDTO = customers[0];
                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO);
                        try
                        {
                            customerProfileImage = customerBL.GetCustomerImageBase64();
                        }
                        catch (Exception ex)
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
                        log.LogMethodExit(customerDTO);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customers, customersImage = customerProfileImage, customersIdImage = customerIdImage, totalPages = totalNoOfCustomers });
                }
                else
                {
                    log.LogMethodExit();
                    if (phoneOrEmail != null)
                    {
                        log.Error("Customer not found " + phoneOrEmail);
                        // Commenting this as this is causing deadlock issues when website triggers parallel Customer search using phone and email
                        //CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, -1, phoneOrEmail, "INVALIDSEARCH", "invalid search for " + phoneOrEmail, DateTime.Now);
                        //CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                        //customerActivityUserLogBL.Save(null);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
        /// Post the JSON Object Customer Collections.
        /// </summary>
        /// <param name="cutomerDTO"></param>
        [HttpPost]
        [Route("api/Customer/Customers")]
        public HttpResponseMessage Post([FromBody]CustomerDTO customer)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(customer);

                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                bool newCustomer = false;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        if (customer != null)
                        {
                            if (customer.SiteId != executionContext.SiteId)
                            {
                                String errorMessage = "";
                                Users user = new Users(executionContext, executionContext.GetUserId(), customer.SiteId);
                                if (String.IsNullOrEmpty(user.UserDTO.LoginId))
                                {
                                    errorMessage = "Please setup the user " + this.executionContext.GetUserId() + ":" + this.executionContext.GetSiteId();
                                    log.Error(errorMessage);
                                    throw new ValidationException(errorMessage);
                                }

                                bool isCorporate = this.executionContext.GetIsCorporate();
                                this.executionContext = new ExecutionContext(user.UserDTO.LoginId, user.UserDTO.SiteId, -1, -1, isCorporate, -1);
                            }

                            log.Debug("Contact List " + (customer.ContactDTOList != null ? customer.ContactDTOList.Count.ToString() : "No Contacts count "));

                            // remove an empty verification dto
                            if (customer != null && customer.CustomerVerificationDTO != null)
                            {
                                if (customer.CustomerVerificationDTO.Id == -1 && string.IsNullOrWhiteSpace(customer.CustomerVerificationDTO.VerificationCode))
                                    customer.CustomerVerificationDTO = null;
                            }

                            CustomerBL customerBL = new CustomerBL(executionContext, customer);
                            var validationError = customerBL.Validate();

                            if (validationError != null && validationError.Count > 0)
                            {
                                throw new ValidationException("", validationError);
                            }
                            else
                            {
                                if (customerBL.CustomerDTO.Id == -1)
                                {
                                    newCustomer = true;
                                    if (customerBL.CustomerDTO.ProfileDTO.ContactDTOList != null && customerBL.CustomerDTO.ProfileDTO.ContactDTOList.Any())
                                    {
                                        foreach (ContactDTO contact in customerBL.CustomerDTO.ProfileDTO.ContactDTOList)
                                        {
                                            log.Debug("Contact details:" + contact.Attribute1 + ":" + contact.IsActive);
                                            contact.IsActive = true;
                                        }
                                    }
                                }

                                customerBL.Save(parafaitDBTrx.SQLTrx);

                                if (newCustomer)
                                {
                                    AccountServiceDTO accountServiceDTO = new AccountServiceDTO();
                                    accountServiceDTO.CustomerDTO = customerBL.CustomerDTO;
                                    LinkAccountToCustomerBL linkAccountToCustomerBL = new LinkAccountToCustomerBL(this.executionContext, accountServiceDTO);
                                    linkAccountToCustomerBL.LinkAccount(parafaitDBTrx.SQLTrx);
                                }
                                parafaitDBTrx.EndTransaction();

                                try
                                {
                                    // create roaming data for customer outside of transaction so that the upload job can pick up the full update in one batch
                                    CustomerBL customerBLForDBSync = new CustomerBL(this.executionContext, customerBL.CustomerDTO.Id);
                                    customerBLForDBSync.CreateRoamingDataForCustomer();
                                }
                                catch (Exception ex)
                                {
                                    log.Error("Error while creating DB synch entries for Customer " + ex);
                                }

                                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerBL.CustomerDTO });
                            }
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "" });
                        }
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
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
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
        /// Post the JSON Agents
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Customer/customers/{id}/Address")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<AddressDTO> addresssDTOList, [FromUri] int id = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(addresssDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                CustomerDTO customerDTO = await customerUseCases.SaveCustomerAddress(addresssDTOList, id);
                if (customerDTO != null)
                {
                    log.LogMethodExit(addresssDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerDTO });
                }
                else
                {
                    log.LogMethodExit(addresssDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>   
        /// Delete the customer
        /// </summary>       
        [HttpPost]
        [Route("api/Customer/Customers/{customerId}/Delete")]
        public async Task<HttpResponseMessage> DeleteCustomer(int customerId)
        {
            log.LogMethodEntry(customerId);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                if (customerId < 0)
                {
                    String message = "Invalid Customer " + customerId;
                    log.Error(message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = message });
                }

                log.Debug("Initiating customer delete");
                ICustomerUseCases customerUseCases = CustomerUseCaseFactory.GetCustomerUseCases(executionContext);
                await customerUseCases.DeleteCustomer(customerId);
                log.Debug("Completed customer delete");

                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
