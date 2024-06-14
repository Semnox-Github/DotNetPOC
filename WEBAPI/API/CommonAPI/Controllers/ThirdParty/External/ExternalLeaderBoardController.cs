/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch Leader board details.
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
    public class ExternalLeaderBoardController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON LeaderBoardDTO
        /// </summary>     
        /// <param name="gameMachineLevelId">gameMachineLevelId</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Game/LeaderBoard")]
        public async Task<HttpResponseMessage> Get(int gameMachineLevelId = -1)
        {
            log.LogMethodEntry(gameMachineLevelId);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (gameMachineLevelId < 0)
                {
                    string customException = "Please enter valid game machine level id";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<LeaderBoardDTO> leaderBoardDTOList = await customerGamePlayLevelResultUseCase.GetLeaderBoard(gameMachineLevelId);
                List<ExternalLeaderBoardDTO> externalLeaderBoardList = new List<ExternalLeaderBoardDTO>();
                ExternalLeaderBoardDTO externalLeaderBoard = new ExternalLeaderBoardDTO();
                if (leaderBoardDTOList != null && leaderBoardDTOList.Any())
                {
                    foreach (LeaderBoardDTO leaderBoardDTO in leaderBoardDTOList)
                    {
                        externalLeaderBoard = new ExternalLeaderBoardDTO(leaderBoardDTO.Name, leaderBoardDTO.Photo, leaderBoardDTO.Rank, leaderBoardDTO.Game,
                            leaderBoardDTO.LevelName, leaderBoardDTO.Score);
                        externalLeaderBoardList.Add(externalLeaderBoard);
                    }
                }
                log.LogMethodExit(externalLeaderBoardList);
                return Request.CreateResponse(HttpStatusCode.OK, externalLeaderBoardList );
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