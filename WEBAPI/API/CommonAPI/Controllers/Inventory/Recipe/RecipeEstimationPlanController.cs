/********************************************************************************************
* Project Name - CommonAPI
* Description  - RecipeEstimationPlanController - Created for create Plan button functionality in Recipe Forecasting scree.
*  
**************
**Version Log
**************
*Version     Date             Modified By           Remarks          
*********************************************************************************************
 2.130.00     19-Jun-2021     Mushahid Faizan       Created
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

namespace Semnox.CommonAPI.Controllers.Inventory.Recipe
{
    public class RecipeEstimationPlanController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object of RecipeEstimationHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeEstimationPlans")]
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
                    await recipeEstimationUseCases.CreatePlan(recipeEstimationHeaderDTOList);
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
    }
}
