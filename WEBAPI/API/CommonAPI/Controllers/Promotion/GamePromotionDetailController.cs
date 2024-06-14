/********************************************************************************************
 * Project Name - Promotions
 * Description  - Controller to get the Game Promotions.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.150.2     18-Jan-2022      Abhishek          Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Promotions;
using Semnox.Parafait.ServerCore;

namespace Semnox.CommonAPI.Promotion
{
    public class GamePromotionDetailController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object of Game Promotion Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/GamePromotionDetails")]
        public async Task<HttpResponseMessage> Get(int gameId = -1, int gameProfileId = -1, int membershipId = -1)
        {
            log.LogMethodEntry(gameId, gameProfileId, membershipId);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                List<KeyValuePair<PromotionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PromotionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PromotionDTO.SearchByParameters, string>(PromotionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                PromotionViewDTO promotionViewDTO = await gameTransactionUseCases.GetGamePromotionDetailDTO(gameId, gameProfileId, membershipId);
                log.LogMethodExit(promotionViewDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = promotionViewDTO });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Get the JSON Object of Machine Promotion Details
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/MachinePromotionDetails")]
        public async Task<HttpResponseMessage> Get(string machineIdList = null, int membershipId = -1)
        {
            log.LogMethodEntry(machineIdList, membershipId);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {

                IGameTransactionUseCases gameTransactionUseCases = GameTransactionUseCaseFactory.GetGameTransactionUseCases(executionContext);
                List<GameMachinePromotion> gameMachinePromotionDTOList = await gameTransactionUseCases.GetMachinePromotionDetailDTOList(machineIdList, membershipId);
                log.LogMethodExit(gameMachinePromotionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = gameMachinePromotionDTOList });
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
