/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET and POST the Game levels 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Transaction.VirtualArcade;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Games.Controllers.Games
{
    public class CustomerGamePlayLevelResultController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Product CustomerGamePlayLevelResult.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/CustomerGamePlayLevelResults")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int customerGamePlayLevelResultId = -1, int gamePlayId = -1,
                                                  int gameMachineLevelId = -1, int customerId = -1,string gameMachineLevelIdList = null)
        {

            log.LogMethodEntry(isActive, gameMachineLevelId, gameMachineLevelIdList, customerId);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }

                if (customerGamePlayLevelResultId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_LEVEL_RESULT_ID, customerGamePlayLevelResultId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(gameMachineLevelIdList) == false)
                {
                    searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID_LIST, gameMachineLevelIdList.ToString()));
                }
                if (gameMachineLevelId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_MACHINE_LEVEL_ID, gameMachineLevelId.ToString()));
                }
                if (gamePlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<CustomerGamePlayLevelResultDTO.SearchByParameters, string>(CustomerGamePlayLevelResultDTO.SearchByParameters.GAME_PLAY_ID, gamePlayId.ToString()));
                }

                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = await customerGamePlayLevelResultUseCase.GetCustomerGamePlayLevelResults(searchParameters);
                log.LogMethodExit(customerGamePlayLevelResultDTOList);
                customerGamePlayLevelResultDTOList = customerGamePlayLevelResultDTOList.OrderByDescending(x => x.GameMachineLevelId).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = customerGamePlayLevelResultDTOList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Product CustomerGamePlayLevelResult
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTOList">CustomerGamePlayLevelResultList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Game/CustomerGamePlayLevelResults")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                log.Debug("executionContext" + executionContext.GetSitePKId());
                if (customerGamePlayLevelResultDTOList == null || customerGamePlayLevelResultDTOList.Any(a => a.CustomerGamePlayLevelResultId > 0))
                {
                    log.LogMethodExit(customerGamePlayLevelResultDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<GamePlayWinningsDTO> result = await customerGamePlayLevelResultUseCase.SaveCustomerGamePlayLevelResults(customerGamePlayLevelResultDTOList);
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Product CustomerGamePlayLevelResult
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTOList">CustomerGamePlayLevelResultList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Game/CustomerGamePlayLevelResults")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList)
        {
            log.LogMethodEntry(customerGamePlayLevelResultDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                // Checks if the id is greater than to 0, If it is greater than to 0, then update 
                if (customerGamePlayLevelResultDTOList == null || customerGamePlayLevelResultDTOList.Any(a => a.CustomerGamePlayLevelResultId < 0))
                {
                    log.LogMethodExit(customerGamePlayLevelResultDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<GamePlayWinningsDTO> result = await customerGamePlayLevelResultUseCase.SaveCustomerGamePlayLevelResults(customerGamePlayLevelResultDTOList);

                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

    }
}
