/********************************************************************************************
 * Project Name - Customer Services Controller
 * Description  - Controller for Customer Services
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        15-Oct-2019      Nitin Pai      Initial Version
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using Semnox.Parafait.Customer;
using System.Linq;

namespace Semnox.CommonAPI.Controllers.Customer
{
    public class PerformCustomerServiceDTO
    {
        public enum ActivityType
        {
            /// <summary>
            /// To return Active Customers
            /// </summary>
            ACTIVE_CUSTOMERS,
            /// <summary>
            /// To validate customer and return Validation Error list
            /// </summary>
            VALIDATE_CUSTOMER
        }

        public ActivityType activityType;
        public CustomerDTO customerDTO;
    }
    public class CustomerServicesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.Utilities utilities;

        /// <summary>
        /// Customer Services Controller
        /// </summary>
        [HttpGet]
        [Route("api/Customer/CustomerServices")]
        [Authorize]
        public HttpResponseMessage Get(PerformCustomerServiceDTO.ActivityType activityType, int customerId = -1, DateTime? fromDate = null, DateTime? toDate = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                String message = "";
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                utilities.ParafaitEnv.Initialize();

                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);

                if (fromDate != null)
                {
                    //DateTime.TryParseExact(fromDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is " + utilities.getParafaitDefaults("DATE_FORMAT");
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }

                if (toDate != null)
                {
                    //DateTime.TryParseExact(toDate, utilities.getParafaitDefaults("DATE_FORMAT"), CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is " + utilities.getParafaitDefaults("DATE_FORMAT");
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }
                else
                {
                    endDate = utilities.getServerTime();
                }

                switch (activityType)
                {
                    case PerformCustomerServiceDTO.ActivityType.ACTIVE_CUSTOMERS:
                        {
                            if (fromDate == null || toDate == null)
                            {
                                return Request.CreateResponse(HttpStatusCode.OK, new { data = "Dates are mandatory", token = securityTokenDTO.Token });
                            }

                            var content = customerListBL.GetActiveCustomersInDateRangeList(startDate, endDate, null);
                            log.LogMethodExit(message);
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
                        }
                    default:
                        {
                            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Activity Type not found", token = securityTokenDTO.Token });
                        }
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
        /// Customer Services Controller
        /// </summary>
        [HttpPost]
        [Route("api/Customer/CustomerServices")]
        [Authorize]
        public HttpResponseMessage Post(PerformCustomerServiceDTO customerServiceDTO)
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
                switch (customerServiceDTO.activityType)
                {
                    case PerformCustomerServiceDTO.ActivityType.VALIDATE_CUSTOMER:
                        CustomerBL customerBL = new CustomerBL(executionContext, customerServiceDTO.customerDTO);
                        List<ValidationError> validationErrorList = customerBL.Validate();
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = validationErrorList, token = securityTokenDTO.Token });
                    default:
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Activity type not found", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex, token = securityTokenDTO.Token });
            }
        }
    }
}
