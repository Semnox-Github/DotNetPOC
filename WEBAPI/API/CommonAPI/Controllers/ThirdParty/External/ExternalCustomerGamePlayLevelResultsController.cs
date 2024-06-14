/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to save game play winnings.
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
    public class ExternalCustomerGamePlayLevelResultsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON CustomerGamePlayLevelResult
        /// </summary>
        /// <param name="customerGamePlayLevelResultDTOList">CustomerGamePlayLevelResultList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/External/Game/CustomerGamePlayLevelResults")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<ExternalCustomerGamePlayLevelResultsDTO> externalCustomerGamePlayLevelResultsDTOList)
        {
            log.LogMethodEntry(externalCustomerGamePlayLevelResultsDTOList);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log.Debug("executionContext" + executionContext.GetSitePKId());
                List<CustomerGamePlayLevelResultDTO> customerGamePlayLevelResultDTOList = new List<CustomerGamePlayLevelResultDTO>();
                CustomerGamePlayLevelResultDTO customerGamePlayLevelResultDTO = new CustomerGamePlayLevelResultDTO();
                if (externalCustomerGamePlayLevelResultsDTOList == null || externalCustomerGamePlayLevelResultsDTOList.Any(a => a.CustomerGamePlayLevelResultId > 0))
                {
                    log.LogMethodExit(customerGamePlayLevelResultDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                foreach (ExternalCustomerGamePlayLevelResultsDTO externalCustomerGamePlayLevelResults in externalCustomerGamePlayLevelResultsDTOList)
                {
                    customerGamePlayLevelResultDTO.CustomerGamePlayLevelResultId = externalCustomerGamePlayLevelResults.CustomerGamePlayLevelResultId;
                    customerGamePlayLevelResultDTO.GamePlayId = externalCustomerGamePlayLevelResults.GamePlayId;
                    customerGamePlayLevelResultDTO.GameMachineLevelId = externalCustomerGamePlayLevelResults.GameMachineLevelId;
                    customerGamePlayLevelResultDTO.CustomerId = externalCustomerGamePlayLevelResults.CustomerId;
                    Parafait.Game.VirtualArcade.Points points = new Parafait.Game.VirtualArcade.Points(externalCustomerGamePlayLevelResults.Points.Type,
                        Convert.ToInt32(externalCustomerGamePlayLevelResults.Points.Value));
                    customerGamePlayLevelResultDTO.Points = points;
                    customerGamePlayLevelResultDTOList.Add(customerGamePlayLevelResultDTO);
                }

                ICustomerGamePlayLevelResultUseCases customerGamePlayLevelResultUseCase = TransactionUseCaseFactory.GetCustomerGamePlayLevelResultUseCases(executionContext);
                List<GamePlayWinningsDTO> result = await customerGamePlayLevelResultUseCase.SaveCustomerGamePlayLevelResults(customerGamePlayLevelResultDTOList);
                ExternalGamePlayWinningsDTO externalGamePlayWinnings = new ExternalGamePlayWinningsDTO();
                if (result != null && result.Any())
                {
                    foreach (GamePlayWinningsDTO gamePlayWinningsDTO in result)
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
                return Request.CreateResponse(HttpStatusCode.OK, externalGamePlayWinnings);
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}