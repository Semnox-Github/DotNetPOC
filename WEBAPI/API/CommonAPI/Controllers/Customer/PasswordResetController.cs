/********************************************************************************************
 * Project Name - Forgot Password Controller
 * Description  - Controller for sending customer password reset link
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.80       08-Apr-2020      Nitin Pai      Cobra changes for Waiver, Customer Registration and Online Sales
 *2.150.2     25-Jul-2023      Nitin Pai      SISA Security fixes - Do not confirm if the email is existing email
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
    public class ForgotPasswordController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpPost]
        [Route("api/Customer/PasswordReset")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] CustomerDTO customerDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                try
                {
                    if (customerDTO != null)
                    {
                        CustomerListBL customerListBL = new CustomerListBL(this.executionContext);
                        List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_USER_NAME, customerDTO.UserName));
                        searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.ISACTIVE, "1"));

                        List<CustomerDTO> customerList = customerListBL.GetCustomerDTOList(searchParameters, true);
                        if (customerList != null && customerList.Any())
                        {
                            log.Debug("Customer Found");
                            CustomerBL customerBL = new CustomerBL(executionContext, customerList[0]);
                            customerBL.ForgotPassword();
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                        }
                        else
                        {
                            // SISA Security recommentation - Do not confirm if the user id is valid or not.
                            log.Debug("Customer not found");
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                            //return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "", token = securityTokenDTO.Token });
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = valEx.Message, token = securityTokenDTO.Token });
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message, token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
