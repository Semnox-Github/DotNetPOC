/********************************************************************************************
* Project Name - CommonAPI
* Description  - RecipePlanController - Created to get the plan for recipe
*  
**************
**Version Log
**************
*Version     Date          Modified By               Remarks          
*********************************************************************************************
*2.110.00     06-Nov-2020     Abhishek              Created : As part of Inventory UI Redesign
*2.130.0      03-May-2021   Mushahid Faizan         Added Search Params in Get().      
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

namespace Semnox.CommonAPI.Inventory.Recipe
{
    public class RecipePlanController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object of RecipePlanHeaderDTO
        /// </summary>
        /// <param name="recipePlanHeaderId">recipePlanHeaderId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipePlans")]
        public async Task<HttpResponseMessage> Get(int recipePlanHeaderId = -1, bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false,
            DateTime? planFromDate=null, DateTime? planToDate=null, DateTime? fromDate = null, DateTime? toDate = null, DateTime? recurEndDate=null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipePlanHeaderId, buildChildRecords, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> recipePlanHeaderSearchParameter = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (recipePlanHeaderId > 0)
                {
                    recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.RECIPE_PLAN_HEADER__ID, Convert.ToString(recipePlanHeaderId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (fromDate != null && toDate != null)
                {
                    DateTime pFromDate = Convert.ToDateTime(fromDate);
                    DateTime pToDate = Convert.ToDateTime(toDate);
                    recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, pFromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                    recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, pToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }
                if (planFromDate != null && planToDate != null)
                {
                    DateTime plFromDate = Convert.ToDateTime(planFromDate);
                    DateTime plToDate = Convert.ToDateTime(planToDate);
                    recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_FROM_DATE, plFromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                    recipePlanHeaderSearchParameter.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_TO_DATE, plToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }
                IRecipePlanUseCases recipePlanUseCases = RecipeUseCaseFactory.GetRecipePlanUseCases(executionContext);
                List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = await recipePlanUseCases.GetRecipePlans(recipePlanHeaderSearchParameter, buildChildRecords, loadActiveChild);
                log.LogMethodExit(recipePlanHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = recipePlanHeaderDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RecipePlanHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipePlans")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RecipePlanHeaderDTO> recipePlanHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipePlanHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
                {
                    IRecipePlanUseCases recipePlanUseCases = RecipeUseCaseFactory.GetRecipePlanUseCases(executionContext);
                    await recipePlanUseCases.SaveRecipePlans(recipePlanHeaderDTOList);
                    log.LogMethodExit(recipePlanHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = recipePlanHeaderDTOList });
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
        /// Post the JSON Object of RecipePlanHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipePlans")]
        public async Task<HttpResponseMessage> Put([FromBody] List<RecipePlanHeaderDTO> recipePlanHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipePlanHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipePlanHeaderDTOList == null || recipePlanHeaderDTOList.Any(x => x.RecipePlanHeaderId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                IRecipePlanUseCases recipePlanUseCases = RecipeUseCaseFactory.GetRecipePlanUseCases(executionContext);
                await recipePlanUseCases.SaveRecipePlans(recipePlanHeaderDTOList);
                log.LogMethodExit(recipePlanHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = recipePlanHeaderDTOList });

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
