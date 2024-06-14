/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Login
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        23-Sep-2018   Manoj          Created
 *2.60        14-Mar-2019   Jagan Mohan    Implemented Roles Authorization for Form Access
 *2.70        27-Jul-2019   Nitin Pai      Implemented Anonymous Login for non userid\pwd loging
 *2.80        25-Nov-2019   Mushahid Faizan Implemented AnonymousGuestLogin for anonymous Guest .
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Site;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Games
{
    public class CustomerLoginController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        [HttpPost]
        [Route("api/Customer/CustomerLogin")]
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
                        
                        CustomerListBL customerListBL = new CustomerListBL(executionContext);
                        List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_USER_NAME, customerDTO.UserName));
                        searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.ISACTIVE, "1"));

                        List<CustomerDTO> customerList = customerListBL.GetCustomerDTOList(searchParameters,true);
                        if (customerList != null && customerList.Any())
                        {
                            CustomerBL customerBL = new CustomerBL(executionContext, customerList[0]);
                            if (customerBL.Authenticate(customerDTO.Password))
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerList[0]});
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = MessageContainerList.GetMessage(executionContext, 24) });
                            }
                        }
                        log.LogMethodExit(null, "Customer not found ");
                        return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = MessageContainerList.GetMessage(executionContext, 24)});
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""});
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = valEx.Message});
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message});
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
