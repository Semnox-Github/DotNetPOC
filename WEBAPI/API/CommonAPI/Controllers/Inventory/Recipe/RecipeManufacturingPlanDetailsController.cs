/********************************************************************************************
* Project Name - CommonAPI
* Description  - RecipeManufacturingPlanDetailsController - Created for create KPN button in the Recipe Production Plan screen.
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.130.0     19-Jun-2021   Mushahid Faizan         Created.     
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class RecipeManufacturingPlanDetailsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Object of RecipePlanDetailsDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipePlans/{planHeaderId}/PlanDetails")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RecipePlanDetailsDTO> recipePlanDetailsDTOList, [FromUri] int planHeaderId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipePlanDetailsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipePlanDetailsDTOList == null || recipePlanDetailsDTOList.Any(x => x.RecipePlanHeaderId != planHeaderId) || planHeaderId < -1)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (recipePlanDetailsDTOList != null && recipePlanDetailsDTOList.Any())
                {
                    IRecipePlanUseCases recipePlanUseCases = RecipeUseCaseFactory.GetRecipePlanUseCases(executionContext);
                    await recipePlanUseCases.CreateKPN(recipePlanDetailsDTOList, planHeaderId);
                    log.LogMethodExit(recipePlanDetailsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = recipePlanDetailsDTOList });
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
