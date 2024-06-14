/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Content List
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.50        28-Sept-2018   Jagan Mohana Rao        Created 
 *2.90        07-Aug-2020    Mushahid Faizan        Modified : Renamed Controller from ContentListController to DSLookUpController, Changed end points,
 *                                                  Added search parameters in get, Removed Delete() and removed token from response body.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Web;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class DSLookUpController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Content List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DSLookUps")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, int dsLookupId = -1, string dsLookupName = null, bool loadActiveChild = false, bool buildChildRecords = false)
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

                List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DSLookupDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (!string.IsNullOrEmpty(dsLookupName))
                {
                    searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.DSLOOKUP_NAME, dsLookupName));
                }
                if (dsLookupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DSLookupDTO.SearchByParameters, string>(DSLookupDTO.SearchByParameters.DSLOOKUP_ID, dsLookupId.ToString()));
                }

                DSLookupListBL lookupListBL = new DSLookupListBL(executionContext);
                var content = lookupListBL.GetDSLookupDTOList(searchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on dslookup details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        // POST: api/Subscriber
        [HttpPost]
        [Route("api/DigitalSignage/DSLookUps")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DSLookupDTO> lookupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(lookupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (lookupDTOList != null && lookupDTOList.Any())
                {
                    // if lookupDTOs.DSLookupID is less than zero then insert or else update
                    DSLookupListBL dsLookupListBL = new DSLookupListBL(executionContext, lookupDTOList);
                    dsLookupListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = lookupDTOList });
                }
                else
                {
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
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }

        }
    }
}
