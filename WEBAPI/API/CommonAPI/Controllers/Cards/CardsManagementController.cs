/********************************************************************************************
 * Project Name - Cards
 * Description  - API for the Cards "CardsManagement(Inventory)" entity. 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        22-Feb-2019     Nagesh Badiger      Created
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.Parafait.Cards
{
    public class CardsManagementController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the CardsManagement.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Cards/CardsManagement/")]
        public HttpResponseMessage Get(string tabName, string fromDate = null, string toDate = null)
        {
            int cardInventoryStock = 0;
            int cardInventoryIssuedStock = 0;
            TokenCardInventoryDTO tokenCardInventoryDTO = new TokenCardInventoryDTO();
            List<TokenCardInventoryDTO> tokenCardInventoryDTOList = null;            
            
            try
            {
                log.LogMethodEntry(tabName,fromDate,toDate);                
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                
                TokenCardInventory tokenCardInventory = new TokenCardInventory(executionContext);
                cardInventoryStock = tokenCardInventory.GetCardStock(securityTokenDTO.SiteId);
                cardInventoryIssuedStock = tokenCardInventory.GetCardsIssued(securityTokenDTO.SiteId);

                TokenCardInventoryList tokenCardInventoryList = new TokenCardInventoryList(executionContext);
                if (tabName.ToUpper().ToString() == "ADDCARDS" || tabName.ToUpper().ToString() == "DELETECARDS")
                {
                    // returns empty tokenCardInventoryDTO
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { cardInventoryDTO = tokenCardInventoryDTO, cardInventoryStock = cardInventoryStock, cardInventoryIssuedStock = cardInventoryIssuedStock, token = securityTokenDTO.Token });
                }
                else if (tabName.ToUpper().ToString() == "TOKENCARDINVENTORY")
                {
                    // To get last sunday date
                    DateTime lastSundayDate;
                    Utilities utilities = new Utilities();
                    string businessDayStartTime = utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME");
                    int businessHour = 6;

                    if (!string.IsNullOrEmpty(businessDayStartTime))
                    {
                        try
                        {
                            businessHour = Convert.ToInt32(businessDayStartTime);
                        }
                        catch
                        {
                            businessHour = 6;
                        }
                    }
                    //check today is monday and buisness hour not crossed the get last 2 sunday date
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Monday && DateTime.Now.Hour < businessHour)
                    {
                        lastSundayDate = DateTime.Today.Date.AddDays(-8).AddHours(businessHour);
                    }
                    else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)//If Today is sunday consider last sunday
                    {
                        lastSundayDate = DateTime.Today.Date.AddDays(-7).AddHours(businessHour);
                    }
                    else
                    {
                        DateTime input = DateTime.Today.Date;
                        int days = DayOfWeek.Sunday - input.DayOfWeek;
                        lastSundayDate = input.AddDays(days).AddHours(businessHour);
                    }
                    
                    // Token/Card Inventory idetails will fetch the week end records                                                                    
                    searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE, lastSundayDate.ToString("MM-dd-yyyy hh")));
                    tokenCardInventoryDTOList = tokenCardInventoryList.GetAllTokenCardInventoryDTOsList(searchParameters);
                    log.LogMethodExit(tokenCardInventoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { cardInventoryDTO = tokenCardInventoryDTOList, actionDate = lastSundayDate,  token = securityTokenDTO.Token });
                }
                else if (tabName.ToUpper().ToString() == "VIEWINVENTORY" && !string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
                {
                    // View Inventory it will fetch the till date records
                    DateTime viewInventoryFromDate = Convert.ToDateTime(fromDate).Date;
                    DateTime viewInventoryToDate = Convert.ToDateTime(toDate).Date.AddDays(1);
                    tokenCardInventoryDTOList = tokenCardInventoryList.GetReportTokenInventoryList(searchParameters);
                    if (tokenCardInventoryDTOList != null)
                    {
                        tokenCardInventoryDTOList = tokenCardInventoryDTOList.FindAll(x => x.Actiondate >= viewInventoryFromDate && x.Actiondate < viewInventoryToDate);
                    }
                    log.LogMethodExit(tokenCardInventoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { cardInventoryDTO = tokenCardInventoryDTOList, cardInventoryStock = cardInventoryStock, cardInventoryIssuedStock = cardInventoryIssuedStock, token = securityTokenDTO.Token });
                }
                else
                {
                    // View Inventory it will fetch the till date records
                    tokenCardInventoryDTOList = tokenCardInventoryList.GetReportTokenInventoryList(searchParameters);
                    log.LogMethodExit(tokenCardInventoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { cardInventoryDTO = tokenCardInventoryDTOList, cardInventoryStock = cardInventoryStock, cardInventoryIssuedStock = cardInventoryIssuedStock, token = securityTokenDTO.Token });
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
        /// Posts the JSON Object for TokenCardInventory
        /// </summary>
        /// <param name="tokenCardInventoryDTOList">TokenCardInventoryDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Cards/CardsManagement/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            try
            {
                log.LogMethodEntry(tokenCardInventoryDTOList);               
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (tokenCardInventoryDTOList != null || tokenCardInventoryDTOList.Count != 0)
                {
                    TokenCardInventoryList tokenCardInventoryList = new TokenCardInventoryList(tokenCardInventoryDTOList, executionContext);
                    tokenCardInventoryList.SaveUpdateCardInventory();
                    log.LogMethodEntry(tokenCardInventoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry(tokenCardInventoryDTOList);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.LogMethodExit(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the JSON Object for TokenCardInventory
        /// </summary>
        /// <param name="tokenCardInventoryDTOList">TokenCardInventoryDTOList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Cards/CardsManagement/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            try
            {
                log.LogMethodEntry(tokenCardInventoryDTOList);                
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (tokenCardInventoryDTOList != null || tokenCardInventoryDTOList.Count != 0)
                {
                    TokenCardInventoryList tokenCardInventoryList = new TokenCardInventoryList(tokenCardInventoryDTOList, executionContext);
                    tokenCardInventoryList.SaveUpdateCardInventory();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}