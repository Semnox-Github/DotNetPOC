/********************************************************************************************
 * Project Name - Product UOM
 * Description  - Created to fetch, update and insert in the Product UOM .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60.3     14-Jun-2019   Nagesh Badiger           Created 
 *2.100.0     04-Oct-2020  Mushahid Faizan         Modified: as per API Standards, namespace changes, endPoint Changes, added searchParameters in get(),
 *                                                 Renamed Controller from ProductUOMController to UOMController and Removed Delete().
 *2.110.0    23-Nov-2020   Mushahid Faizan         Web Inventory UI resdesign changes with REST API.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class UOMController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Product UOM.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/UOM")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int uOMId = -1, bool loadChildRecords = false, bool activeChildRecords = true ,string uOM = null, 
                                                     int currentPage = 0, int pageSize = 10)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(uOM))
                {
                    searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.UOM, uOM));
                }
                if (uOMId > -1)
                {
                    searchParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.UOMID, uOMId.ToString()));
                }
                UOMList uomList = new UOMList(executionContext);


                int totalNoOfPages = 0;
                int totalNoOfUOM = await Task<int>.Factory.StartNew(() => { return uomList.GetUOMCount(searchParameters, null); });
                log.LogVariableState("totalNoOfUOM", totalNoOfUOM);
                totalNoOfPages = (totalNoOfUOM / pageSize) + ((totalNoOfUOM % pageSize) > 0 ? 1 : 0);

                IUOMUseCases uomUseCases = InventoryUseCaseFactory.GetUOMUseCases(executionContext);
                List<UOMDTO> uomDTOList = await uomUseCases.GetUOMs(searchParameters, loadChildRecords,activeChildRecords, currentPage, pageSize);

                log.LogMethodExit(uomDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = uomDTOList, currentPageNo = currentPage, TotalCount = totalNoOfUOM });

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
        /// Post the JSON Object Product UOM
        /// </summary>
        /// <param name="uomDTOList">uOMDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Inventory/UOM")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<UOMDTO> uomDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(uomDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (uomDTOList == null || uomDTOList.Any(a => a.UOMId > 0))
                {
                    log.LogMethodExit(uomDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUOMUseCases uomUseCases = InventoryUseCaseFactory.GetUOMUseCases(executionContext);
                await uomUseCases.SaveUOMs(uomDTOList);
                log.LogMethodExit(uomDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = uomDTOList });
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
        /// Post the JSON Object Product UOM
        /// </summary>
        /// <param name="uomDTOList">uOMDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPut]
        [Route("api/Inventory/UOM")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<UOMDTO> uomDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(uomDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (uomDTOList == null || uomDTOList.Any(a => a.UOMId < 0))
                {
                    log.LogMethodExit(uomDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IUOMUseCases uomUseCases = InventoryUseCaseFactory.GetUOMUseCases(executionContext);
                await uomUseCases.SaveUOMs(uomDTOList);
                log.LogMethodExit(uomDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = uomDTOList });
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
