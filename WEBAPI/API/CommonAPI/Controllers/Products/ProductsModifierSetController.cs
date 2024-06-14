/********************************************************************************************
 * Project Name - Products Controller
 * Description  - Created to fetch, update and insert ProductModifiers entity.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        23-Jan-2019   Muhammed Mehraj         Created to get, insert, update and Delete Methods.
 *2.60        26-Apr-2019   Akshay G                modified  
 *2.70        29-Jun-2019   Akshay G                modified  Delete method
 *2.110.0     10-Sep-2020   Girish Kundar           Modified :  REST API Standards.
 *2.120.00   08-Mar-2021   Roshan Devadiga          Modified Get,Post Delete and Added Put method
 ********************************************************************************************/

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
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class ProductsModifierSetController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the Modifiersets
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Product/ProductsModifierSets")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int modifierSetId = -1, bool buildChildRecords = false, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(modifierSetId, buildChildRecords, loadActiveChild, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchByParameters = new List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>>();
                searchByParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                bool loadActiveChildRecords = false;
                if (isActive.ToString() == "1")
                {
                    loadActiveChildRecords = true;
                    searchByParameters.Add(new KeyValuePair<ModifierSetDTO.SearchByParameters, string>(ModifierSetDTO.SearchByParameters.ISACTIVE, isActive));
                }
                IModifierSetUseCases modifierSetUseCases = ProductsUseCaseFactory.GetModifierSetUseCases(executionContext);
                List<ModifierSetDTO> modifierSetDTOList = await modifierSetUseCases.GetModifierSets(searchByParameters, buildChildRecords, loadActiveChildRecords);
                log.LogMethodExit(modifierSetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = modifierSetDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
        /// <summary>
        /// Posts the Modifiersets
        /// </summary>
        /// <param name="modifierSetDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/ProductsModifierSets")]
        [Authorize]
        public async Task<HttpResponseMessage> Post(List<ModifierSetDTO> modifierSetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(modifierSetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (modifierSetDTOList == null )
                {
                    log.LogMethodExit(modifierSetDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IModifierSetUseCases modifierSetUseCases = ProductsUseCaseFactory.GetModifierSetUseCases(executionContext);
                await modifierSetUseCases.SaveModifierSets(modifierSetDTOList);
                log.LogMethodExit(modifierSetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }


        /// <summary>
        /// Deletes the Modifiersets
        /// </summary>
        /// <param name="modifierSetDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/ProductsModifierSets")]
        [Authorize]
        public HttpResponseMessage Delete(List<ModifierSetDTO> modifierSetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(modifierSetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (modifierSetDTOList != null && modifierSetDTOList.Count != 0)
                {
                    IModifierSetUseCases modifierSetUseCases = ProductsUseCaseFactory.GetModifierSetUseCases(executionContext);
                    modifierSetUseCases.Delete(modifierSetDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the ModifierSetDTOList collection
        /// <param name="modifierSetDTOList">ModifierSetDTOList</param>
        [HttpPut]
        [Route("api/Product/ProductsModifierSets")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ModifierSetDTO> modifierSetDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(modifierSetDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (modifierSetDTOList == null || modifierSetDTOList.Any(a => a.ModifierSetId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IModifierSetUseCases modifierSetUseCases = ProductsUseCaseFactory.GetModifierSetUseCases(executionContext);
                await modifierSetUseCases.SaveModifierSets(modifierSetDTOList);
                log.LogMethodExit(modifierSetDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
