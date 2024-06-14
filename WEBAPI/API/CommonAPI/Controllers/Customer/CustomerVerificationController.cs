/********************************************************************************************
 * Project Name - Verify customer data for login
 * Description  - Controller for Verifying customer code and update the customer verification flag
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        10-Oct-2019      Nitin Pai      Initial Version 
 * 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.80        05-Apr-2020      Girish Kundar  Modified: API end point and removed token from the body 
 *2.130.10    08-Sep-2022      Nitin Pai      Enhanced customer activity user log table
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
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class CustomerVerificationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Validate the new customer registration data.
        /// Validation fails if
        ///     The contact number is duplicate
        ///     Customer data fails the generic validation of the Customer 3 tier rules
        /// </summary>       
        [HttpPost]
        [Route("api/Customer/CustomerVerifications")]
        public HttpResponseMessage Post([FromBody]CustomerVerificationDTO customerVerificationDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;

            try
            {
                log.LogMethodEntry(customerVerificationDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerVerificationDTO != null && customerVerificationDTO.CustomerId != -1)
                {
                    CustomerVerificationBL customerVerificationBL = new CustomerVerificationBL(executionContext, customerVerificationDTO);
                    List<ValidationError> validationError = new List<ValidationError>();

                    try
                    {
                        // If the customer verification DTO does not contain a verification code, it indicates that the customer has requested for a new verification code
                        if (String.IsNullOrEmpty(customerVerificationDTO.VerificationCode) && customerVerificationDTO.Id > -1 && !customerVerificationDTO.ResendEmailToken)
                        {
                            return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = "Invalid PIN" });
                        }
                        if (customerVerificationDTO.ResendEmailToken)
                        {
                            CustomerActivityUserLogDTO errorcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerVerificationDTO.CustomerId, "",
                                    "CUSTOMERVERIFICATION", "RESEND VERIFICATION CODE", ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                            CustomerActivityUserLogBL errorcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, errorcustomerActivityUserLogDTO);
                            errorcustomerActivityUserLogBL.Save();

                            // If resendEmailToken is true, it indicates that the customer has requested for a new verification email
                            CustomerBL customerBL = new CustomerBL(executionContext, customerVerificationDTO.CustomerId, true);
                            customerBL.SendRegistrationLink(null);
                        }
                        else if (String.IsNullOrEmpty(customerVerificationDTO.VerificationCode))
                        {
                            CustomerBL customerBL = new CustomerBL(executionContext, customerVerificationDTO.CustomerId);
                            CustomerDTO customerDTO = customerBL.CustomerDTO;

                            if (customerDTO == null || customerDTO.Id == -1)
                                throw new Exception("Customer Id is InValid");

                            CustomerVerificationListBL customerVerificationListBL = new CustomerVerificationListBL(executionContext);
                            List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>>();
                            searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.PROFILE_ID, customerDTO.ProfileId.ToString()));
                            searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.IS_ACTIVE, "1"));
                            List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationListBL.GetCustomerVerificationDTOList(searchParameters, false, false, null);
                            if (customerVerificationDTOList != null && customerVerificationDTOList.Any())
                            {
                                customerVerificationDTOList = customerVerificationDTOList.Where(x => x.CustomerId == -1).ToList();
                                if (customerVerificationDTOList != null && customerVerificationDTOList.Any())
                                {
                                    customerVerificationDTOList = customerVerificationDTOList.OrderByDescending(x => x.CreationDate).ToList();
                                    DateTime createdTime = customerVerificationDTOList[0].CreationDate;
                                    DateTime localServerTime = new LookupValuesList(null).GetServerDateTime();
                                    // if the code was generated within one minute, return the same, do not resend as it is blocked
                                    if ((localServerTime - createdTime).TotalSeconds < 60)
                                    {
                                        customerVerificationDTOList[0].CustomerId = customerVerificationDTO.CustomerId;
                                        log.Debug("Customer requested for OTP within a minute, return the same object");
                                        log.Debug(customerVerificationDTOList[0].Id + ":" + customerVerificationDTOList[0].VerificationCode);
                                        return Request.CreateResponse(HttpStatusCode.OK, new { data = customerVerificationDTOList[0], token = securityTokenDTO.Token });
                                    }
                                }
                            }

                            customerVerificationBL.GenerateVerificationRecord(customerVerificationDTO.CustomerId, securityTokenDTO.LoginId, Environment.MachineName, true);
                            // blank out the verification code before sending it to client
                            customerVerificationBL.CustomerVerificationDTO.VerificationCode = "";
                            if (customerVerificationBL.CustomerVerificationDTO.CustomerId == -1)
                                customerVerificationBL.CustomerVerificationDTO.CustomerId = customerVerificationDTO.CustomerId;

                            CustomerActivityUserLogDTO errorcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerVerificationDTO.CustomerId, "",
                                    "CUSTOMERVERIFICATION", "SENT VERIFICATION CODE", ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                            CustomerActivityUserLogBL errorcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, errorcustomerActivityUserLogDTO);
                            errorcustomerActivityUserLogBL.Save();
                        }
                        else
                        {
                            CustomerActivityUserLogDTO errorcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerVerificationDTO.CustomerId, "",
                                    "CUSTOMERVERIFICATION", "VALIDATING VERIFICATION CODE", ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                            CustomerActivityUserLogBL errorcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, errorcustomerActivityUserLogDTO);
                            errorcustomerActivityUserLogBL.Save();

                            customerVerificationBL.Save();
                        }
                    }
                    catch (ValidationException vex)
                    {
                        validationError.AddRange(vex.ValidationErrorList);
                    }
                    catch (Exception ex)
                    {
                        ValidationError error = new ValidationError("CustomerVerification", "DetailsNotFound", ex.Message);
                        validationError.Add(error);
                    }

                    if (validationError != null && validationError.Count > 0)
                    {
                        String error = "Error in OTP" + customerVerificationDTO.Id.ToString() + ":profile:" + customerVerificationDTO.CustomerId.ToString() + ":code" + customerVerificationDTO.VerificationCode;
                        log.Debug(error);
                        log.Debug(String.Join(":", validationError));
                        CustomerActivityUserLogDTO errorcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerVerificationDTO.CustomerId, "",
                                    "CUSTOMERVERIFICATION", error, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                        CustomerActivityUserLogBL errorcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, errorcustomerActivityUserLogDTO);
                        errorcustomerActivityUserLogBL.Save();

                        return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = validationError, token = securityTokenDTO.Token });
                    }
                    else
                    {
                        String message = "Generated/Validated OTP" + customerVerificationDTO.Id.ToString() + ":profile:" + customerVerificationDTO.CustomerId.ToString() + ":code" + customerVerificationDTO.VerificationCode;

                        CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerVerificationDTO.CustomerId, "",
                                    "CUSTOMERVERIFICATION", message, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                        CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                        customerActivityUserLogBL.Save();

                        return Request.CreateResponse(HttpStatusCode.OK, new { data = customerVerificationBL.CustomerVerificationDTO });
                    }
                }
                else
                {
                    log.LogMethodExit();
                    ValidationError duplicate = new ValidationError("Customer", "CustomerId", "Object Not Found");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = new List<ValidationError>() { duplicate } });
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
        /// Gets the Customer Verification DTO based on the passed CustomerId.
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Customer/CustomerVerifications")]
        [Authorize]
        public HttpResponseMessage Get(int customerId = -1, int verificationId = -1, int profileId = -1, string verificationCode = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(customerId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL(); // Prevents creating new token, it will update the existing token.
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customerId == -1 && verificationId == -1 && profileId == -1 && String.IsNullOrEmpty(verificationCode))
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = "No search parameters found." });

                CustomerVerificationListBL customerVerificationListBL = new CustomerVerificationListBL(executionContext);
                List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.IS_ACTIVE, "1"));

                if(customerId > -1)
                    searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));

                if (verificationId > -1)
                    searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.ID, verificationId.ToString()));

                if (profileId > -1)
                    searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.PROFILE_ID, profileId.ToString()));

                if (!String.IsNullOrEmpty(verificationCode))
                    searchParameters.Add(new KeyValuePair<CustomerVerificationDTO.SearchByParameters, string>(CustomerVerificationDTO.SearchByParameters.VERIFICATION_CODE, verificationCode.ToString()));

                List<CustomerVerificationDTO> customerVerificationDTOList = customerVerificationListBL.GetCustomerVerificationList(searchParameters, false);

                if (customerVerificationDTOList != null && customerVerificationDTOList.Any())
                {
                    customerVerificationDTOList = customerVerificationDTOList.OrderByDescending(x => x.CreationDate).Take(1).ToList();
                }

                log.LogMethodExit(customerVerificationDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerVerificationDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }
    }
}
