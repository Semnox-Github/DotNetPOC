/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Products "RedemptionCurrencyRuleContainerController". Created to fetch, update and insert MembershipExclusionRule.
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
  *2.110.0     10-Sep-2020     Vikas Dwivedi       Created
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class RedemptionCurrencyRuleContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/RedemptionCurrencyRulesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                RedemptionCurrencyRuleContainerDTOCollection redemptionCurrencyRuleContainerDTOCollection = await
                           Task<RedemptionCurrencyRuleContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return RedemptionCurrencyRuleViewContainerList.GetRedemptionCurrencyRuleContainerDTOCollection(siteId, hash);
                           });
                log.LogMethodExit(redemptionCurrencyRuleContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionCurrencyRuleContainerDTOCollection });
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
