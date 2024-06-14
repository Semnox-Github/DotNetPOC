/********************************************************************************************
 * Project Name - UOMCount Controller
 * Description  - Created UOMCount Controller
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1   11-feb-2021   Likhitha Reddy            created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class UOMCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Product UOM.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/UOMCounts")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int uOMId = -1, bool loadChildRecords = false, bool activeChildRecords = true, string uOM = null,
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
                IUOMUseCases uomUseCases = InventoryUseCaseFactory.GetUOMUseCases(executionContext);
                int totalNoOfPages = 0;
                int totalNoOfUOM = await uomUseCases.GetUOMCounts(searchParameters); 
                log.LogVariableState("totalNoOfUOM", totalNoOfUOM);
                totalNoOfPages = (totalNoOfUOM / pageSize) + ((totalNoOfUOM % pageSize) > 0 ? 1 : 0);
                log.LogMethodExit(totalNoOfUOM);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalNoOfUOM, currentPageNo = currentPage, TotalNoOfPages = totalNoOfPages });

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}