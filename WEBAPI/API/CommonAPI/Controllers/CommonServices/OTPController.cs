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
 *2.80        15-Oct-2019      Nitin Pai      Converted to generic otp code generator
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.CommonServices
{
    public class OTPController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// This will generate and send the otp to the user. It also returns the OTP to requester in plain text. This should be used only in intranet and not in internet
        /// </summary>  
        [HttpGet]
        [Route("api/CommonServices/OTP")]
        public HttpResponseMessage Get(String phoneNumber = "", String emailAddress = "", bool sendSMS = false, bool sendMail = true)
        {
            SecurityTokenDTO securityTokenDTO = null;
            Utilities utilities = new Utilities();

            try
            {
                log.LogMethodEntry(phoneNumber, emailAddress, sendSMS, sendMail);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if(!String.IsNullOrEmpty(phoneNumber) || !String.IsNullOrEmpty(emailAddress))
                { 
                    Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();
                    Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    Utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                    Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
                    Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                    Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                    Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                    Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                    Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
                    Utilities.ParafaitEnv.Initialize();
                    
                    String otpCode = utilities.GenerateRandomNumber(6, Utilities.RandomNumberType.Numeric);

                    Messaging msg = new Messaging(utilities);
                    string body = "Dear Customer,";
                    body += Environment.NewLine;
                    body += "Your registration verification code is " + otpCode + ".";
                    body += Environment.NewLine;
                    body += "Thank you";

                    try
                    {
                        if (sendMail && !String.IsNullOrEmpty(emailAddress))
                        {
                            msg.SendEMailSynchronous(emailAddress, "", utilities.ParafaitEnv.SiteName + " - customer registration verification", body);
                        }
                        if (sendSMS && !String.IsNullOrEmpty(phoneNumber))
                        {
                            msg.sendSMSSynchronous(phoneNumber, utilities.ParafaitEnv.SiteName + " - customer registration verification " + body);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Send mail or sms failed:" + ex.Message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Unable to send message", token = securityTokenDTO.Token });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = otpCode, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch(Exception ex)
            {
                log.LogMethodExit(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = "", token = securityTokenDTO.Token });
            }
        }
    }
}
