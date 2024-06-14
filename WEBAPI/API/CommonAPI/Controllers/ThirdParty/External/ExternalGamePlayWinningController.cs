/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch game play winnings.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.7    11-Apr-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.VirtualArcade;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalGamePlayWinningController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON ExternalGamePlayWinnings
        /// </summary>    
        /// <param name="customerId">customerId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Game/GamePlayWinnings")]
        public async Task<HttpResponseMessage> Get(int customerId = -1)
        {
            log.LogMethodEntry(customerId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (customerId < 0)
                {
                    string customException = "Please enter valid customer Id";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<GamePlayWinningsDTO> gamePlayWinningsDTOList = await customerGamePlayLevelResultUseCase.GetCustomerGamePlayWinnings(customerId);
                List<ExternalGamePlayWinningsDTO> externalGamePlayWinningsList = new List<ExternalGamePlayWinningsDTO>();
                ExternalGamePlayWinningsDTO externalGamePlayWinnings = new ExternalGamePlayWinningsDTO();
                if (gamePlayWinningsDTOList != null && gamePlayWinningsDTOList.Any())
                {
                    foreach (GamePlayWinningsDTO gamePlayWinningsDTO in gamePlayWinningsDTOList)
                    {
                        List<Winnings> winningDTOList = new List<Winnings>();
                        Winnings winnings = new Winnings();
                        if (gamePlayWinningsDTO.Winnings != null && gamePlayWinningsDTO.Winnings.Any())
                        {
                            foreach (GamePlayWinningDetailDTO gamePlayWinningDetailDTO in gamePlayWinningsDTO.Winnings)
                            {
                                winnings.Type = gamePlayWinningDetailDTO.Type;
                                winnings.Value = gamePlayWinningDetailDTO.Value;
                                winningDTOList.Add(winnings);
                            }
                        }
                        externalGamePlayWinnings = new ExternalGamePlayWinningsDTO(gamePlayWinningsDTO.AccountNumber, gamePlayWinningsDTO.LevelName,
                            gamePlayWinningsDTO.CustomerXP, gamePlayWinningsDTO.LevelId, winningDTOList);
                    }
                }
                log.LogMethodExit(externalGamePlayWinnings);
                return Request.CreateResponse(HttpStatusCode.OK,  externalGamePlayWinnings);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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