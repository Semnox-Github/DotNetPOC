/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Game Plays Card Activty Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        12-Sept-2018   Jagan          Created 
 *2.60        07-May-2019    Nitin Pai      Updated for Guest App
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System.Linq;
using Semnox.Parafait.Customer.Accounts;

namespace Semnox.CommonAPI.CommonServices
{
    public class GamePlaysCardActivityController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/CommonServices/GamePlaysCardActivity")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(String accountId, String startDate = "", String endDate = "", int numberOfDays = 30, int numberOfRows = 100)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, numberOfRows);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DateTime fromDate = DateTime.Now;
                DateTime toDate = fromDate.AddDays(30);
                int dateAdd = numberOfDays > 0 && numberOfDays < 180 ? numberOfDays : 30;
                int topRows = numberOfRows > 0 && numberOfRows < 100 ? numberOfRows : 25;
                bool dateBasedSearch = false;

                List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AccountActivityDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.ACCOUNT_ID, accountId));
                if (!String.IsNullOrEmpty(startDate))
                {
                    dateBasedSearch = true;
                    try
                    {
                        fromDate = Convert.ToDateTime(startDate);
                    }
                    catch
                    { }

                    
                    toDate = fromDate.AddDays(dateAdd);
                    if (!String.IsNullOrEmpty(endDate))
                    {
                        try
                        {
                            toDate = Convert.ToDateTime(endDate);
                        }
                        catch
                        { }
                    }

                    if(numberOfDays < 0 || numberOfDays > 180 || toDate.CompareTo(fromDate) >= 0)
                    {
                        log.LogMethodExit("Wrong input given " + ":" + dateAdd + ":" + toDate + ":" + fromDate);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong inputs", token = securityTokenDTO.Token });
                    }

                    log.LogVariableState("from date", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    log.LogVariableState("to date", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParameters.Add(new KeyValuePair<AccountActivityDTO.SearchByParameters, string>(AccountActivityDTO.SearchByParameters.TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                AccountActivityViewListBL accountActivityViewListBL = new AccountActivityViewListBL(executionContext);

                // DTO at position 0 is the summary DTO. Remove the DTO from this position, sort the list and add it back at position 0
                List<AccountActivityDTO> accountActivityDTOs = accountActivityViewListBL.GetAccountActivityDTOList(searchParameters, true);
                if (accountActivityDTOs != null && accountActivityDTOs.Count > 0)
                {
                    AccountActivityDTO summaryDTO = accountActivityDTOs[0];
                    accountActivityDTOs.RemoveAt(0);
                    accountActivityDTOs = accountActivityDTOs.OrderByDescending(x => x.Date).ThenBy(x => x.Site).ToList();
                    if(!dateBasedSearch)
                    {
                        accountActivityDTOs = accountActivityDTOs.Take(topRows).ToList();
                    }
                    accountActivityDTOs.Insert(0, summaryDTO);
                    log.LogMethodExit(accountActivityDTOs);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = accountActivityDTOs, token = securityTokenDTO.Token });
                }
                else
                {
                    accountActivityDTOs = new List<AccountActivityDTO>();                                       
                    log.LogMethodExit(accountActivityDTOs);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = accountActivityDTOs, token = securityTokenDTO.Token });
                }
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}
