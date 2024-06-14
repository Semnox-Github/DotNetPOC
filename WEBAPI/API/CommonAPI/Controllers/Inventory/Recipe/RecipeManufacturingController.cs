/********************************************************************************************
* Project Name - CommonAPI
* Description  - RecipeManufacturingController - Created to get the manufacturing details of recipe
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
    public class RecipeManufacturingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of RecipeManufacturingHeaderDTO
        /// </summary>
        /// <param name="recipeManufacturingHeaderId">recipeManufacturingHeaderId</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="activeRecordsOnly">activeRecordsOnly</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeManufacturings")]
        public async Task<HttpResponseMessage> Get(int recipeManufacturingHeaderId = -1,  bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false,
            DateTime? mfgFromDate = null, DateTime? mfgToDate = null, int recipePlanHeaderId=-1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeManufacturingHeaderId, buildChildRecords, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> recipeManufacturingSearchParameter = new List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>>();
                recipeManufacturingSearchParameter.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (recipeManufacturingHeaderId > 0)
                {
                    recipeManufacturingSearchParameter.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.RECIPE_MANUFACTURING_HEADER_ID, Convert.ToString(recipeManufacturingHeaderId)));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        recipeManufacturingSearchParameter.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (mfgFromDate != null && mfgToDate != null)
                {
                    DateTime mfg_FromDate = Convert.ToDateTime(mfgFromDate);
                    DateTime mfg_ToDate = Convert.ToDateTime(mfgToDate);
                    recipeManufacturingSearchParameter.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, mfg_FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                    recipeManufacturingSearchParameter.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, mfg_ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }
                IRecipeManufacturingUseCases recipeManufacturingUseCases = RecipeUseCaseFactory.GetRecipeManufacturingUseCases(executionContext);
                List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = await recipeManufacturingUseCases.GetRecipeManufacturings(recipeManufacturingSearchParameter, buildChildRecords, loadActiveChild);
                log.LogMethodExit(recipeManufacturingHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = recipeManufacturingHeaderDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object of RecipeManufacturingHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeManufacturings")]
        public async Task<HttpResponseMessage> Post([FromBody] List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeManufacturingHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
                {
                    IRecipeManufacturingUseCases recipeManufacturingUseCases = RecipeUseCaseFactory.GetRecipeManufacturingUseCases(executionContext);
                    await recipeManufacturingUseCases.SaveRecipeManufacturings(recipeManufacturingHeaderDTOList);
                    log.LogMethodExit(recipeManufacturingHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = recipeManufacturingHeaderDTOList });
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
        /// Post the JSON Object of RecipeManufacturingHeaderDTO List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPut]
        [Authorize]
        [Route("api/Inventory/Recipe/RecipeManufacturings")]
        public async Task<HttpResponseMessage> Put([FromBody] List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(recipeManufacturingHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (recipeManufacturingHeaderDTOList == null || recipeManufacturingHeaderDTOList.Any(x => x.RecipeManufacturingHeaderId < -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
                {
                    IRecipeManufacturingUseCases recipeManufacturingUseCases = RecipeUseCaseFactory.GetRecipeManufacturingUseCases(executionContext);
                    await recipeManufacturingUseCases.SaveRecipeManufacturings(recipeManufacturingHeaderDTOList);
                    log.LogMethodExit(recipeManufacturingHeaderDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
