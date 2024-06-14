/********************************************************************************************
* Project Name - CommonAPI
* Description  - RecipeEstimationController - Created to get the estimation for recipe
*  
**************
**Version Log
**************
*Version     Date             Modified By           Remarks          
*********************************************************************************************
*2.110.00     06-Nov-2020     Abhishek              Created : As part of Inventory UI Redesign
      
 2.120.00     21-Apr-2021     Deeksha               Modified to add get for Build Forecast data
 2.120.00     28-Apr-2021     Mushahid Faizan       Modified - Return DTOList in Put/Post response.
 2.130.0     15-Jun-2021     Mushahid Faizan       Modified - Web Inventory UI changes
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory.Recipe;

namespace Semnox.CommonAPI.Inventory.Recipe
{
    public class RecipeEstimationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of RecipeEstimationHeaderDTO
        /// </summary>
        /// <param name="recipeEstimationHeaderId">recipeEstimationHeaderId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeEstimations")]
        public async Task<HttpResponseMessage> Get(int recipeEstimationHeaderId = -1, DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, bool isActive = true, string generateForecastData = null, int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                log.LogMethodEntry(recipeEstimationHeaderId, isActive, fromDate, toDate, aspirationalPerc, seasonalPerc, isEvent, historicalDataInDays, eventOffset, isFinishedItem,
                    isSemiFinishedItem, generateForecastData, currentPage, pageSize);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                IRecipeEstimationUseCases recipeEstimationUseCases = RecipeUseCaseFactory.GetRecipeEstimationUseCases(executionContext);
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = await recipeEstimationUseCases.GetRecipeForecastingSummary(fromDate, toDate, aspirationalPerc,
                                                 seasonalPerc, isEvent, historicalDataInDays, eventOffset, isFinishedItem,
                                                  isSemiFinishedItem, generateForecastData);
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = recipeEstimationHeaderDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeEstimations")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeEstimationHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Any())
                {
                    IRecipeEstimationUseCases recipeEstimationUseCases = RecipeUseCaseFactory.GetRecipeEstimationUseCases(executionContext);
                    await recipeEstimationUseCases.SaveRecipeEstimations(recipeEstimationHeaderDTOList);
                    log.LogMethodExit(recipeEstimationHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = recipeEstimationHeaderDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeEstimations")]
        public async Task<HttpResponseMessage> Put([FromBody] List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeEstimationHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Any(x => x.RecipeEstimationHeaderId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Any())
                {
                    IRecipeEstimationUseCases recipeEstimationUseCases = RecipeUseCaseFactory.GetRecipeEstimationUseCases(executionContext);
                    await recipeEstimationUseCases.SaveRecipeEstimations(recipeEstimationHeaderDTOList);
                    log.LogMethodExit(recipeEstimationHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = recipeEstimationHeaderDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Delete the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeEstimations")]
        public async Task<HttpResponseMessage> Delete([FromBody] List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeEstimationHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Any(x => x.RecipeEstimationHeaderId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Any())
                {
                    IRecipeEstimationUseCases recipeEstimationUseCases = RecipeUseCaseFactory.GetRecipeEstimationUseCases(executionContext);
                    await recipeEstimationUseCases.DeleteRecipeEstimations(recipeEstimationHeaderDTOList);
                    log.LogMethodExit(recipeEstimationHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
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
