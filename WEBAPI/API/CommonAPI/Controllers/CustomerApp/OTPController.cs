/********************************************************************************************
 * Project Name - OTPController
 * Description  - Controller for Getting the Configuration Setting for the Customer App
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        07-May-2019      Nitin Pai      Initial Version 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.CustomerApp;
using Semnox.Parafait.Site;

namespace Semnox.CommonAPI.CustomerApp
{
    public class OTPController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Get CustomerAppVerificationOTPDTO object - contains the id of the otp sent to the user
        /// </summary>  
        [HttpGet]
        [Route("api/CustomerApp/OTP/{contactNumber}")]
        public HttpResponseMessage Get(String contactNumber)
        {
            SecurityTokenDTO securityTokenDTO = null;
            Utilities utilities = new Utilities();

            try
            {
                log.LogMethodEntry(contactNumber);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                int siteId = securityTokenDTO.SiteId;
                SiteList siteList = new SiteList(executionContext);
                SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                if (HQSite != null && HQSite.SiteId != -1)
                {
                    siteId = HQSite.SiteId;
                }
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, siteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                CustomerSearchCriteria searchCriteria = new CustomerSearchCriteria(CustomerSearchByParameters.ISACTIVE, Operator.EQUAL_TO, 1);
                searchCriteria.And(CustomerSearchByParameters.CONTACT_CONTACT_TYPE, Operator.EQUAL_TO, ContactType.PHONE.ToString());
                searchCriteria.And(CustomerSearchByParameters.CONTACT_ATTRIBUTE1, Operator.LIKE, contactNumber);
                List<CustomerDTO> customersList = customerListBL.GetCustomerDTOList(searchCriteria, true, true);

                CustomerDTO customer;
                if (customersList != null && customersList.Count > 0)
                {
                    if (customersList.Count > 1)
                        customersList = customersList.OrderByDescending(x => x.Id).ToList();

                    customer = customersList[0];
                    CustomerAppOTPVerificationDTO customerOTPDTO = null;
                    CustomerAppOTPVerificationBL verificationOTPBL = new CustomerAppOTPVerificationBL(executionContext);

                    if (!string.IsNullOrEmpty(customer.Email))
                    {
                        customerOTPDTO = verificationOTPBL.GenerateAndSendVerificationCode(customer, true, false);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerOTPDTO, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch(Exception ex)
            {
                log.LogMethodExit(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Get CustomerAppVerificationOTPDTO object - contains the id of the otp sent to the user
        /// </summary>  
        [HttpGet]
        [Route("api/CustomerApp/ResendOTP/{otpId}")]
        public HttpResponseMessage ResendOTP(int otpId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            Utilities utilities = new Utilities();

            try
            {
                log.LogMethodEntry(otpId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                int siteId = securityTokenDTO.SiteId;
                SiteList siteList = new SiteList(executionContext);
                SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                if (HQSite != null && HQSite.SiteId != -1)
                {
                    siteId = HQSite.SiteId;
                }
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, siteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (otpId > 0)
                {
                    CustomerAppOTPVerificationDTO customerOTPDTO = null;
                    CustomerAppOTPVerificationBL verificationOTPBL = new CustomerAppOTPVerificationBL(executionContext);
                    customerOTPDTO = verificationOTPBL.ResendOTP(otpId, true, false);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerOTPDTO, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Validate the OTP sent by the user with the OTP generated by the system
        /// </summary>  
        [HttpPost]
        [Route("api/CustomerApp/OTP")]
        public HttpResponseMessage Post([FromBody]CustomerAppOTPVerificationDTO verificationOTPDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            Utilities utilities = new Utilities();

            try
            {
                log.LogMethodEntry(verificationOTPDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                int siteId = securityTokenDTO.SiteId;
                SiteList siteList = new SiteList(executionContext);
                SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                if (HQSite != null && HQSite.SiteId != -1)
                {
                    siteId = HQSite.SiteId;
                }
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, siteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                CustomerAppOTPVerificationBL appVerificationOTPBL = new CustomerAppOTPVerificationBL(executionContext);

                int customerCode = appVerificationOTPBL.ValidateOTP(verificationOTPDTO);
                if (customerCode > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = customerCode.ToString(), token = securityTokenDTO.Token });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "0", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
            }
        }

    }
}
