/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for Game Play Info
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80        30-Sep-2019   Nitin          Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Globalization;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Customer.Accounts;


namespace Semnox.CommonAPI.Games
{
    public class GamePlayInfoController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Post the JSON Object Game Play
        /// </summary>
        /// <param name="machinesDTOList">machineDtoList</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/GamePlayInfo")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]GamePlayInfoDTO gamePlayInfoDTO, String machineReference = null, String cardNumber = null, int siteId = -1, DateTime? playDate = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(gamePlayInfoDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                GamePlayInfoBL gamePlayInfoBL = null;

                if (gamePlayInfoDTO == null)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }

                if(gamePlayInfoDTO.GameplayId == -1)
                {
                    int machineId = -1;

                    if (!String.IsNullOrEmpty(machineReference))
                    {
                        MachineList machineList = new MachineList(executionContext);
                        List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                        if (siteId > -1)
                            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, siteId.ToString()));

                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, machineReference));
                        List<MachineDTO> machineDTOList = machineList.GetMachineList(searchParameters);
                        if (machineDTOList != null && machineDTOList.Any())
                        {
                            machineId = machineDTOList[0].MachineId;
                        }
                    }

                    int accountId = -1;
                    if(!string.IsNullOrEmpty(cardNumber))
                    {
                        AccountBL accountBL = new AccountBL(executionContext, cardNumber);
                        accountId = accountBL.AccountDTO.AccountId;
                    }

                    if(machineId == -1 && accountId == -1)
                    {
                        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "GamePlayId NotFound" });
                    }

                    DateTime playDateFrom = DateTime.MaxValue;
                    if (playDate == null)
                    {
                        int startTime = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME");
                        LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                        DateTime currentServerTime = lookupValuesList.GetServerDateTime();
                        playDateFrom = currentServerTime.Date.AddHours(startTime);
                    }
                    else
                    {
                        playDateFrom = Convert.ToDateTime(playDate);
                    }

                    GamePlayListBL gamePlayListBL = new GamePlayListBL(executionContext);
                    List<KeyValuePair<GamePlayDTO.SearchByParameters, string>> gpSearchParameters = new List<KeyValuePair<GamePlayDTO.SearchByParameters, string>>();
                    if (siteId > -1)
                        gpSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.SITE_ID, siteId.ToString()));
                    if (machineId > -1)
                        gpSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.MACHINE_ID, machineId.ToString()));
                    if (accountId > -1)
                        gpSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.CARD_ID, accountId.ToString()));
                    gpSearchParameters.Add(new KeyValuePair<GamePlayDTO.SearchByParameters, string>(GamePlayDTO.SearchByParameters.FROM_DATE, playDateFrom.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)));

                    List<GamePlayDTO> gamePlayDTOList = gamePlayListBL.GetGamePlayDTOList(gpSearchParameters);
                    if(gamePlayDTOList != null && gamePlayDTOList.Any())
                    {
                        gamePlayInfoDTO.GameplayId = gamePlayDTOList[gamePlayDTOList.Count - 1].GameplayId;
                    }
                    else
                    {
                        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "GamePlayId NotFound" });
                    }

                }

                GamePlayBL gamePlayBL = new GamePlayBL(executionContext, gamePlayInfoDTO.GameplayId);
                if (gamePlayBL.GamePlayDTO == null)
                {
                    string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(new InvalidOperationException("Invalid Input"), executionContext);
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "GamePlayId NotFound" });
                }
                gamePlayInfoBL = new GamePlayInfoBL(executionContext, gamePlayInfoDTO);
                gamePlayInfoBL.Save();
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
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
