/********************************************************************************************
* Project Name - Tools Controller
* Description  - Created to fetch partnersRevenue records.
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.80        10-Jul-2020   Mushahid Faizan           Created
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Organization
{
    public class PartnerRevenueController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Partner
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Organization/PartnerRevenues")]
        public HttpResponseMessage Get(DateTime? fromDate = null, DateTime? toDate = null, string transactions = null, bool credits = false,
                                        bool courtesy = false, bool bonus = false, bool time = false)
        {
            log.LogMethodEntry(fromDate, toDate, transactions, credits, courtesy, bonus, time);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

            try
            {
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime startDate = serverTimeObject.GetServerDateTime();
                DateTime endDate = startDate.AddDays(1);

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = serverTimeObject.GetServerDateTime();
                }

                PartnerRevenueShareList partnerRevenueShareList = new PartnerRevenueShareList(executionContext);
                List<string> gamePlays = new List<string>();
                // Below condition is used to add the columns to build the query.
                if (credits)
                {
                    gamePlays.Add("Credits");
                }
                if (courtesy)
                {
                    gamePlays.Add("Courtesy");
                }
                if (bonus)
                {
                    gamePlays.Add("Bonus");
                }
                if (time)
                {
                    gamePlays.Add("Time");
                }
                // Check for transaction methods, Default it should be Card Payment or else 'All' need to be passed 
                if (string.IsNullOrEmpty(transactions))
                {
                    transactions = "CardPayment";
                }
                List<PartnerRevenueShareDTO> content = partnerRevenueShareList.GetPartnerRevenuesTable(startDate, endDate, transactions, gamePlays);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
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
