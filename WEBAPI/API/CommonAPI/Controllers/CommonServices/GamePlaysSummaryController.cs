/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Machines Game Plays Summary Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        12-Sept-2018   Jagan          Created 
 *2.60        07-May-2019    Nitin Pai      Modified for guest app
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Parafait.Game;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.Customer.Accounts;
using System.Linq;

namespace Semnox.CommonAPI.CommonServices
{
    public class GamePlaysSummaryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Game Plays List
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/CommonServices/GamePlaysSummary")]
        [Authorize]
        [HttpGet]
        public HttpResponseMessage Get(string accountId, String startDate = "", String endDate = "", int numberOfDays = 180, int numberOfRows = 100)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(accountId, startDate, endDate, numberOfDays, numberOfRows);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executioncontext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DateTime fromDate = DateTime.Now;
                DateTime toDate = fromDate.AddDays(30);
                int dateAdd = numberOfDays > 0 && numberOfDays < 180 ? numberOfDays : 30;
                int topRows = numberOfRows > 0 && numberOfRows < 100 ? numberOfRows : 25;
                bool dateBasedSearch = false;

                List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId));
                if (!String.IsNullOrEmpty(startDate))
                {
                    dateBasedSearch = true;
                    fromDate = DateTime.Now;
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

                    if (numberOfDays < 0 || numberOfDays > 180 || toDate.CompareTo(fromDate) >= 0)
                    {
                        log.LogMethodExit("Wrong input given " + ":" + dateAdd + ":" + toDate + ":" + fromDate);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Wrong inputs", token = securityTokenDTO.Token });
                    }

                    log.LogVariableState("from date", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                    log.LogVariableState("to date", toDate.ToString("yyyy-MM-dd HH:mm:ss"));

                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    searchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.TO_DATE, fromDate.AddDays(-180).ToString("yyyy-MM-dd HH:mm:ss")));
                }

                GamePlaySummaryListBL gamePlaySummaryListBL = new GamePlaySummaryListBL(executioncontext);                
                List<GamePlayDTO> gamePlayDTOList = gamePlaySummaryListBL.GetGamePlayDTOList(searchParameters, true);
                if (gamePlayDTOList != null && gamePlayDTOList.Count > 0)
                {
                    GamePlayDTO summaryDTO = gamePlayDTOList[0];
                    gamePlayDTOList.RemoveAt(0);
                    gamePlayDTOList = gamePlayDTOList.OrderByDescending(x => x.PlayDate).ThenBy(x => x.Site).ToList();
                    if (!dateBasedSearch)
                    {
                        gamePlayDTOList = gamePlayDTOList.Take(topRows).ToList();
                    }
                    gamePlayDTOList.Insert(0, summaryDTO);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = gamePlayDTOList, token = securityTokenDTO.Token });
                }
                else
                {
                    gamePlayDTOList = new List<GamePlayDTO>();
                    log.LogMethodExit(gamePlayDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = gamePlayDTOList, token = securityTokenDTO.Token });
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
