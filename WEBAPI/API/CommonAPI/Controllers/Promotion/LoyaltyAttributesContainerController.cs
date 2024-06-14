/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Controller for LoyaltyAttributesContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     20-Nov-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.Parafait.Promotions;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Promotion
{
    public class LoyaltyAttributesContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        /// 
        [HttpGet]
        [Authorize]
        [Route("api/Promotion/LoyaltyAttributesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {

            try
            {
                log.LogMethodEntry(siteId, rebuildCache, hash);
                LoyaltyAttributeContainerDTOCollection loyaltyAttributeContainerDTOCollection = await
                          Task<LoyaltyAttributeContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return LoyaltyAttributeViewContainerList.GetLoyaltyAttributeContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(loyaltyAttributeContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = loyaltyAttributeContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}