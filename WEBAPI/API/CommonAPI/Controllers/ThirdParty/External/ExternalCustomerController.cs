/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch card details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
 *2.150.5    31-Oct-2023   Abhishek                 Modified : Return CustomerImage as Base64 string.
 *2.151.1    19-Feb-2023   Abhishek                 Modified : Addition of site id parameter on Get customers.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalCustomerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.Utilities utilities;

        /// <summary>
        /// Gets the JSON Object Customers
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Customers")]
        public async Task<HttpResponseMessage> Get(int customerId = -1, string phone = null, String emailId = null, String firstName = null, String lastName = null, DateTime? fromDate = null, DateTime? toDate = null,
                                       int pageNumber = -1, int pageSize = -1, bool buildChildRecords = false, bool activeRecordsOnly = false, bool profilePic = false, bool idPic = false, bool buildActiveCampaignActivity = false,
                                       bool buildLastVistitedDate = false, string phoneOrEmail = null, bool loadAdultOnly = false, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false, //string registrationToken = null,
                                       string customerGUID = null, int customerMembershipId = -1, string uniqueIdentifier = null, string middleName = null,
                                       string exactContactNumber = null, string exactEmailId = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(customerId, phone, emailId, firstName, lastName, fromDate, toDate, pageNumber, pageSize, buildChildRecords, activeRecordsOnly, profilePic,
                    idPic, buildActiveCampaignActivity, buildLastVistitedDate, phoneOrEmail, loadAdultOnly, loadSignedWaivers, loadSignedWaiverFileContent, customerGUID, customerMembershipId, uniqueIdentifier, middleName, exactContactNumber, exactEmailId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                IList<CustomerDTO> customerDTOList = null;
                string customerProfileImage = string.Empty;
                string customerIdImage = string.Empty;

                customerDTOList = GetCustomerDTOList(executionContext, customerId, phone, emailId, firstName, lastName, fromDate, toDate,
                                       pageNumber, pageSize, buildChildRecords, activeRecordsOnly, profilePic, idPic, buildActiveCampaignActivity,
                                       buildLastVistitedDate, phoneOrEmail, loadAdultOnly, loadSignedWaivers, loadSignedWaiverFileContent,
                                       customerGUID, customerMembershipId, uniqueIdentifier, middleName);
                List<ExternalCustomersDTO> externalCustomersDTOList = new List<ExternalCustomersDTO>();
                ExternalCustomersDTO externalCustomers = new ExternalCustomersDTO();
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    foreach (CustomerDTO customerDTO in customerDTOList)
                    {
                        List<Address> addressDTOList = new List<Address>();
                        if (customerDTO.ProfileDTO.AddressDTOList != null && customerDTO.ProfileDTO.AddressDTOList.Any())
                        {
                            foreach (AddressDTO addressDTO in customerDTO.ProfileDTO.AddressDTOList)
                            {
                                Address address = new Address(addressDTO.AddressType.ToString(), addressDTO.Line1, addressDTO.Line2, addressDTO.Line3, addressDTO.City,
                                    addressDTO.StateId, addressDTO.CountryId, addressDTO.PostalCode, addressDTO.StateCode, addressDTO.StateName, addressDTO.CountryName);
                                addressDTOList.Add(address);
                            }
                        }
                        List<Contact> contactDTOList = new List<Contact>();
                        if (customerDTO.ProfileDTO.ContactDTOList != null && customerDTO.ProfileDTO.ContactDTOList.Any())
                        {
                            foreach (ContactDTO contactDTO in customerDTO.ContactDTOList)
                            {
                                Contact contact = new Contact();
                                if (contactDTO.ContactType.ToString() == "EMAIL")
                                {
                                    contact = new Contact(contactDTO.Attribute1, null);
                                }
                                else if (contactDTO.ContactType.ToString() == "PHONE")
                                {
                                    contact = new Contact(null, contactDTO.Attribute1);
                                }
                                contactDTOList.Add(contact);
                            }
                        }
                        string customerImage = string.Empty;
                        if (!string.IsNullOrEmpty(customerDTO.PhotoURL))
                        {
                            try
                            {
                                CustomerBL customerBL = new CustomerBL(executionContext, customerDTO.Id);
                                customerImage = await Task<String>.Factory.StartNew(() =>
                                {
                                    return customerBL.GetCustomerImageBase64();
                                });
                            }
                            catch (Exception ex)
                            {
                                string message = MessageContainerList.GetMessage(executionContext, 2405);
                                log.Error(message, ex);
                            }
                        }
                        externalCustomers = new ExternalCustomersDTO(customerDTO.Id, customerDTO.MembershipId, customerDTO.Title, customerDTO.FirstName,
                            customerDTO.MiddleName, customerDTO.LastName, customerDTO.TaxCode, customerDTO.DateOfBirth, customerDTO.Gender, customerDTO.Anniversary,
                            customerDTO.CardNumber, customerDTO.SiteId, customerDTO.ProfileDTO.NickName, customerImage, addressDTOList, contactDTOList);
                        externalCustomersDTOList.Add(externalCustomers);
                    }
                }
                else
                {

                    string customException = "Customer Not Found";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                return Request.CreateResponse(HttpStatusCode.OK, externalCustomersDTOList);
            }
            catch (ValidationException valex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(valex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        private List<CustomerDTO> GetCustomerDTOList(ExecutionContext executionContext, int customerId = -1, string phone = null, String emailId = null, String firstName = null, String lastName = null, DateTime? fromDate = null, DateTime? toDate = null,
                                       int pageNumber = -1, int pageSize = -1, bool buildChildRecords = false, bool activeRecordsOnly = false, bool profilePic = false, bool idPic = false, bool buildActiveCampaignActivity = false,
                                       bool buildLastVistitedDate = false, string phoneOrEmail = null, bool loadAdultOnly = false, bool loadSignedWaivers = false, bool loadSignedWaiverFileContent = false, //string registrationToken = null,
                                       string customerGUID = null, int customerMembershipId = -1, string uniqueIdentifier = null, string middleName = null,
                                       string exactContactNumber = null, string exactEmailId = null)
        {
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
                    throw new ValidationException(customException);
                }
            }
            if (toDate != null)
            {
                endDate = Convert.ToDateTime(toDate.ToString());
                if (endDate == DateTime.MinValue)
                {
                    string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                    log.Error(customException);
                    throw new ValidationException(customException);
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

            if (!String.IsNullOrEmpty(phone))
            {
                searchCriteria.And(CustomerSearchByParameters.CONTACT_IS_ACTIVE, Operator.EQUAL_TO, "1");
                searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, phone);
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
            int totalNoOfCustomers = 0;
            // Perf Change: get the customer count only for WMS for other scenarios this is not required
            if (getCustomerCount)
                totalNoOfCustomers = customerListBL.GetCustomerCount(searchCriteria, null);
            else
                totalNoOfCustomers = 1;

            List<CustomerDTO> customers = null;
            if (totalNoOfCustomers > 0)
            {
                log.LogVariableState("totalNoOfCustomer", totalNoOfCustomers);



                customers = customerListBL.GetCustomerDTOList(searchCriteria, pageNumber, pageSize, buildChildRecords, activeRecordsOnly, loadSignedWaivers, null, loadSignedWaiverFileContent, utilities, buildActiveCampaignActivity: buildActiveCampaignActivity,
                        buildLastVistitedDate: buildLastVistitedDate, fromDate: startDate, toDate: endDate);


            }
            log.LogMethodExit(customers);
            return customers;
        }

        /// <summary>
        /// Post the JSON Object Customers
        /// </summary>
        /// <param name="externalCustomersDTO">externalCustomersDTO</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/External/Customers")]
        public HttpResponseMessage Post([FromBody]ExternalCustomersDTO externalCustomersDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ExternalCustomersDTO externalCustomerDTO = new ExternalCustomersDTO();
                CustomerDTO customerDTO = new CustomerDTO();
                if (externalCustomersDTO == null)
                {
                    string customException = "Customer data cannot be null.Please enter the customer Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                customerDTO = new CustomerDTO(externalCustomersDTO.Id, -1, externalCustomersDTO.MembershipId, string.Empty, -1, true, string.Empty,
                    true, externalCustomersDTO.CardNumber, CustomerType.REGISTERED);
                ProfileDTO profileDTO = new ProfileDTO();
                profileDTO.Title = externalCustomersDTO.Title;
                profileDTO.FirstName = externalCustomersDTO.FirstName;
                profileDTO.MiddleName = externalCustomersDTO.MiddleName;
                profileDTO.LastName = externalCustomersDTO.LastName;
                profileDTO.DateOfBirth = externalCustomersDTO.DateOfBirth;
                profileDTO.TaxCode = externalCustomersDTO.TaxCode;
                profileDTO.Gender = externalCustomersDTO.Gender;
                profileDTO.Anniversary = externalCustomersDTO.Anniversary;
                profileDTO.NickName = externalCustomersDTO.NickName;
                profileDTO.PhotoURL = externalCustomersDTO.CustomerImage;
                profileDTO.AddressDTOList = new List<AddressDTO>();
                profileDTO.ContactDTOList = new List<ContactDTO>();
                if (externalCustomersDTO.Address != null && externalCustomersDTO.Address.Any())
                {
                    foreach (Address address in externalCustomersDTO.Address)
                    {
                        string type = address.AddressType;
                        AddressType addressType = AddressType.NONE;
                        if (type == "WORK")
                        {
                            addressType = AddressType.WORK;
                        }
                        else if (type == "HOME")
                        {
                            addressType = AddressType.HOME;
                        }
                        AddressDTO addressDTO = new AddressDTO(-1, -1, -1, addressType, address.Line1, address.Line2, address.Line3, address.City,
                            address.StateId, address.PostalCode, address.CountryId, address.StateCode, address.StateName, string.Empty, false, true);
                        profileDTO.AddressDTOList.Add(addressDTO);
                    }
                }
                if (externalCustomersDTO.Contact != null && externalCustomersDTO.Contact.Any())
                {
                    foreach (Contact contact in externalCustomersDTO.Contact)
                    {
                        ContactDTO contactDTO = new ContactDTO();
                        if (!string.IsNullOrEmpty(contact.Email))
                        {
                            contactDTO.ContactType = ContactType.EMAIL;
                            contactDTO.Attribute1 = contact.Email;
                        }
                        else if (!string.IsNullOrEmpty(contact.Phone))
                        {
                            contactDTO.ContactType = ContactType.PHONE;
                            contactDTO.Attribute1 = contact.Phone;
                        }
                        else
                        {
                            contactDTO.ContactType = ContactType.NONE;
                        }
                        profileDTO.ContactDTOList.Add(contactDTO);
                    }
                }
                customerDTO.ProfileDTO = profileDTO;
                CustomerBL customerBL = null;
                //bool newCustomer = false;
                if (customerDTO != null)
                {
                    log.Debug("Contact List " + (customerDTO.ContactDTOList != null ? customerDTO.ContactDTOList.Count.ToString() : "No Contacts count "));

                    // remove an empty verification dto
                    if (customerDTO != null && customerDTO.CustomerVerificationDTO != null)
                    {
                        if (customerDTO.CustomerVerificationDTO.Id == -1 && string.IsNullOrWhiteSpace(customerDTO.CustomerVerificationDTO.VerificationCode))
                            customerDTO.CustomerVerificationDTO = null;
                    }

                    customerBL = new CustomerBL(executionContext, customerDTO);
                    var validationError = customerBL.Validate();

                    if (validationError != null && validationError.Count > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationError });
                    }
                    else
                    {
                        if (customerBL.CustomerDTO.Id == -1)
                        {
                            //newCustomer = true;
                            if (customerBL.CustomerDTO.ProfileDTO.ContactDTOList != null && customerBL.CustomerDTO.ProfileDTO.ContactDTOList.Any())
                            {
                                foreach (ContactDTO contact in customerBL.CustomerDTO.ProfileDTO.ContactDTOList)
                                {
                                    log.Debug("Contact details:" + contact.Attribute1 + ":" + contact.IsActive);
                                    contact.IsActive = true;
                                }
                            }
                        }

                        customerBL.Save(null);
                    }
                }
                string customerImage = string.Empty;
                if (!string.IsNullOrEmpty(customerDTO.PhotoURL))
                {
                    try
                    {
                        customerBL = new CustomerBL(executionContext, customerDTO.Id);
                        customerImage = customerBL.GetCustomerImageBase64();
                    }
                    catch (Exception ex)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2405);
                        log.Error(message, ex);
                    }
                }
                List<Address> addressDTOList = new List<Address>();
                if (customerDTO.ProfileDTO.AddressDTOList != null && customerDTO.ProfileDTO.AddressDTOList.Any())
                {
                    foreach (AddressDTO addressDTO in customerDTO.ProfileDTO.AddressDTOList)
                    {
                        Address address = new Address(addressDTO.AddressType.ToString(), addressDTO.Line1, addressDTO.Line2, addressDTO.Line3, addressDTO.City,
                            addressDTO.StateId, addressDTO.CountryId, addressDTO.PostalCode, addressDTO.StateCode, addressDTO.StateName, addressDTO.CountryName);
                        addressDTOList.Add(address);
                    }
                }
                List<Contact> contactDTOList = new List<Contact>();
                if (customerBL.CustomerDTO.ProfileDTO.ContactDTOList != null && customerBL.CustomerDTO.ProfileDTO.ContactDTOList.Any())
                {
                    foreach (ContactDTO contactDTO in customerDTO.ContactDTOList)
                    {
                        Contact contact = new Contact();
                        if (contactDTO.ContactType.ToString() == "EMAIL")
                        {
                            contact = new Contact(contactDTO.Attribute1, null);
                        }
                        else if (contactDTO.ContactType.ToString() == "PHONE")
                        {
                            contact = new Contact(null, contactDTO.Attribute1);
                        }
                        contactDTOList.Add(contact);
                    }
                }

                externalCustomerDTO = new ExternalCustomersDTO(customerBL.CustomerDTO.Id, customerBL.CustomerDTO.MembershipId, customerBL.CustomerDTO.ProfileDTO.Title, customerBL.CustomerDTO.ProfileDTO.FirstName,
                    customerBL.CustomerDTO.ProfileDTO.MiddleName, customerBL.CustomerDTO.ProfileDTO.LastName, customerBL.CustomerDTO.ProfileDTO.TaxCode, customerBL.CustomerDTO.ProfileDTO.DateOfBirth, customerBL.CustomerDTO.ProfileDTO.Gender,
                    customerBL.CustomerDTO.ProfileDTO.Anniversary, customerBL.CustomerDTO.CardNumber, customerBL.CustomerDTO.SiteId, customerBL.CustomerDTO.ProfileDTO.NickName, customerImage, addressDTOList, contactDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalCustomerDTO);
            }
            catch (ValidationException valex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = customException,
                    exception = ExceptionSerializer.Serialize(valex)
                });
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