/********************************************************************************************
 * Project Name - Customer Device verification Controller
 * Description  - Controller for Customer Device UUID Resource
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
 *2.80        15-Oct-2019      Nitin Pai      Renamed
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
    public class CustomerDeviceVerificationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Post the JSON Object Cards Customer Collections.
        /// </summary>
        /// <param name="cutomerDTO"></param>
        [HttpPost]
        [Route("api/Customer/CustomerDeviceVerifications")]
        public HttpResponseMessage Post([FromBody]CustomerDTO customerDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(customerDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<ContactDTO> contactDTOList = customerDTO.ContactDTOList;
                String phoneOrEmail = "";
                String UUID = "";
                bool isAppValid = false;
                string message = "User is not registered on this device";

                // Get the UUID of the device from the Contact DTO attribute of customerDTO
                if (contactDTOList != null && contactDTOList.Count > 0)
                {
                    foreach (ContactDTO contact in contactDTOList)
                    {
                        if (!String.IsNullOrEmpty(contact.Attribute1) && !String.IsNullOrEmpty(contact.UUID))
                        {
                            phoneOrEmail = contact.Attribute1;
                            UUID = contact.UUID;
                            break;
                        }
                    }
                }

                log.Debug("In device verification");
                log.Debug(UUID + ":" + phoneOrEmail + ":" + customerDTO.Id);
                // Check if the UUID of the registered number and current UUID matches
                if (UUID != null && !String.IsNullOrEmpty(UUID) && phoneOrEmail != null && !String.IsNullOrWhiteSpace(phoneOrEmail))
                {
                    CustomerActivityUserLogDTO customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerDTO.Id, UUID,
                                    "CUSTOMERLOGIN", "Login from " + phoneOrEmail, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.INFO));
                    CustomerActivityUserLogBL customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                    customerActivityUserLogBL.Save();

                    if (customerDTO.Id > -1)
                    {
                        CustomerBL customerBL = new CustomerBL(executionContext, customerDTO.Id, true, false);
                        if (customerBL.CustomerDTO == null || customerBL.CustomerDTO.Id <= 0)
                        {
                            log.Debug("No Customer found");
                            message = "This device is unauthorized for use. Please contact site to reset this device.";
                            isAppValid = false;
                            contactDTOList = null;
                        }
                        else
                        {
                            log.Debug("Customer found");
                            contactDTOList = customerBL.CustomerDTO.ContactDTOList;
                        }
                    }
                    else
                    {
                        ContactListBL contactListBL = new ContactListBL(executionContext);
                        List<KeyValuePair<ContactDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ContactDTO.SearchByParameters, string>>();
                        if (phoneOrEmail.Contains("@"))
                            searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.CONTACT_TYPE, ContactType.EMAIL.ToString()));
                        else
                            searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.CONTACT_TYPE, ContactType.PHONE.ToString()));

                        searchByParameters.Add(new KeyValuePair<ContactDTO.SearchByParameters, string>(ContactDTO.SearchByParameters.ATTRIBUTE1, phoneOrEmail));
                        contactDTOList = contactListBL.GetContactDTOList(searchByParameters);
                    }


                    if (contactDTOList != null && contactDTOList.Any())
                    {
                        log.Debug("contacts found " + contactDTOList);

                        List<ContactDTO> inactiveContactList = contactDTOList.Where(x => x.UUID.Equals(UUID) && !x.IsActive).ToList();
                        List<ContactDTO> activeContactList = contactDTOList.Where(x => x.UUID.Equals(UUID) && x.IsActive).ToList();
                        if (inactiveContactList != null && inactiveContactList.Any())
                        {
                            log.Debug("inactive contacts found " + inactiveContactList);
                            CustomerActivityUserLogDTO errorcustomerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, customerDTO.Id, UUID,
                                    "CUSTOMERLOGIN", "Unauthorized device for " + phoneOrEmail, ServerDateTime.Now,
                                    "POS " + executionContext.GetPosMachineGuid(), this.Request.Content.ToString(),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivityCategory), CustomerActivityUserLogDTO.ActivityCategory.SMARTFUN),
                                    Enum.GetName(typeof(CustomerActivityUserLogDTO.ActivitySeverity), CustomerActivityUserLogDTO.ActivitySeverity.ERROR));
                            CustomerActivityUserLogBL errorcustomerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, errorcustomerActivityUserLogDTO);
                            errorcustomerActivityUserLogBL.Save();

                            message = "This device is unauthorized for use. Please contact site to reset this device.";
                            log.Debug(message);
                            isAppValid = false;
                        }
                        else if (activeContactList != null)
                        {
                            log.Debug("active contacts found " + inactiveContactList);
                            isAppValid = true;
                            message = "";
                        }
                        else
                        {
                            log.Debug("no active or inactive contacts found. New device.");
                            //contactDTOList = contactDTOList.Where(x => x.IsActive).OrderByDescending(x => x.ProfileId).ToList();
                            //ContactDTO contact = contactDTOList[0];
                            //isAppValid = true;
                            //message = "";
                            //contact.UUID = UUID;
                            //ContactBL primaryContact = new ContactBL(executionContext, contact);
                            //primaryContact.Save();
                            //isAppValid = true;
                            //message = "";
                            //log.Debug("Registring device " + UUID + " for contact " + contact.Attribute1 + ":Profile:" + contact.ProfileId);
                            //customerActivityUserLogDTO = new CustomerActivityUserLogDTO(-1, -1, UUID, "CUSTOMERLOGIN", "Registered new device for " + contactNumber, DateTime.Now);
                            //customerActivityUserLogBL = new CustomerActivityUserLogBL(executionContext, customerActivityUserLogDTO);
                            //customerActivityUserLogBL.Save(null);

                            //foreach (ContactDTO contact in contactDTOList)
                            //{
                            //    if (contact.IsActive && contact.Attribute1.Equals(contactNumber)
                            //        && !String.IsNullOrEmpty(contact.UUID) && !contact.UUID.Equals(UUID))
                            //    {
                            //        message = "User login required.";
                            //        isAppValid = false;
                            //        ****IMPORTANT * **** : comment before check -in, temp to allow single device development
                            //        {
                            //            isAppValid = true;
                            //            message = "";
                            //            contact.UUID = UUID;
                            //            ContactBL primaryContact = new ContactBL(executionContext, contact);
                            //            primaryContact.Save();
                            //            isAppValid = true;
                            //            message = "";
                            //            log.Debug("Registring device " + UUID + " for contact " + contact.Attribute1 + ":Profile:" + contact.ProfileId);
                            //        }
                            //        break;
                            //    }
                            //    else if (contact.IsActive && contact.Attribute1.Equals(contactNumber)
                            //        && String.IsNullOrEmpty(contact.UUID))
                            //    {
                            //        this will happen for first login

                            //        update only

                            //       contact.UUID = UUID;
                            //        ContactBL primaryContact = new ContactBL(executionContext, contact);
                            //        primaryContact.Save();
                            //        isAppValid = true;
                            //        message = "";
                            //        log.Debug("Registring device " + UUID + " for contact " + contact.Attribute1 + ":Profile:" + contact.ProfileId);
                            //        break;
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        log.Debug("no contacts found ");
                        // there are no numbers in db. All the user to proceed to register
                        isAppValid = true;
                        message = "";
                    }
                }

                if (!isAppValid)
                {
                    log.Debug("invalid app " + message);
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = message });
                }
                else
                {
                    log.Debug("valid app " + message);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = message });
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
