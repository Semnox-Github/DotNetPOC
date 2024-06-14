/********************************************************************************************
 * Project Name - ProductsEntityExclusionController
 * Description  - Created to fetch, update and insert for Include / Exclude Days for Product Games and Extended Credits
 * in the product games entitlement.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        24-Jan-2019   Jagan Mohana Rao        Created to get, insert, update and Delete Methods.
 *2.60        07-May-2019   Akshay Gulaganji        Added isActive Parameter for Get method* 
 ********************************************************************************************
 *2.70        29-June-2019  Indrajeet Kumar         Modified Delete Implemented - Hard Deletion
 *2.110.0     21-Nov-2019   Girish Kundar           Modified :  REST API changes for Inventory UI redesign
 *2.120.00    08-Apr-2021   B Mahesh Pai            Modified Get and post ,Delete , Added Put method
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using System.Linq;
using Semnox.Parafait.User;
using System.Threading.Tasks;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class ProductsEntityExclusionController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Entity Exclusion Dates.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductEntityExclusions")]
        public async Task<HttpResponseMessage> Get(string isActive, string entityName, string entityGUID)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, entityName, entityGUID);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>> searchParameters = new List<KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>>();
                if (!string.IsNullOrEmpty(entityName))
                {
                    searchParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_NAME, entityName));
                }
                if (!string.IsNullOrEmpty(entityGUID))
                {
                    searchParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.ENTITY_GUID, entityGUID));
                }
                searchParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<EntityOverrideDatesDTO.SearchByEntityOverrideParameters, string>(EntityOverrideDatesDTO.SearchByEntityOverrideParameters.IS_ACTIVE, isActive));
                }
                IEntityOverrideDatesUseCases entityOverrideDatesUseCases = GenericUtilitiesUseCaseFactory.GetEntityOverrideDates(executionContext);
                List<EntityOverrideDatesDTO> entityOverrideDatesList = await entityOverrideDatesUseCases.GetEntityOverrideDates(searchParameters);
                log.LogMethodExit(entityOverrideDatesList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = entityOverrideDatesList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the JSON Object Entity Exclusion
        /// </summary>
        /// <param name="entityOverrideDatesList">entityOverrideDatesList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductEntityExclusions")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody]List<EntityOverrideDatesDTO> entityOverrideDatesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(entityOverrideDatesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (entityOverrideDatesList == null )
                {
                    log.LogMethodExit(entityOverrideDatesList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IEntityOverrideDatesUseCases entityOverrideDatesUseCases = GenericUtilitiesUseCaseFactory.GetEntityOverrideDates(executionContext);
                await entityOverrideDatesUseCases.SaveEntityOverrideDates(entityOverrideDatesList);
                log.LogMethodExit();
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
        /// Post the EntityOverrideDatesDTO collection
        /// <param name="entityOverrideDatesDTO">EntityOverrideDatesDTO</param>
        [HttpPut]
        [Route("api/Product/ProductEntityExclusions")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<EntityOverrideDatesDTO> entityOverrideDatesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(entityOverrideDatesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (entityOverrideDatesList == null || entityOverrideDatesList.Any(a => a.ID < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IEntityOverrideDatesUseCases entityOverrideDatesUseCases = GenericUtilitiesUseCaseFactory.GetEntityOverrideDates(executionContext);
                await entityOverrideDatesUseCases.SaveEntityOverrideDates(entityOverrideDatesList);
                log.LogMethodExit(entityOverrideDatesList);
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
        /// Delete the Product Entity Exclusion
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/ProductEntityExclusions")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<EntityOverrideDatesDTO> entityOverrideDatesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(entityOverrideDatesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (entityOverrideDatesList != null || entityOverrideDatesList.Any())
                {
                    IEntityOverrideDatesUseCases entityOverrideDatesUseCases = GenericUtilitiesUseCaseFactory.GetEntityOverrideDates(executionContext);
                    entityOverrideDatesUseCases.Delete(entityOverrideDatesList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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