/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - LookupContainerController
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *0.0        02-Sep-2020   Girish Kundar   Created : POSUI redesign using REST API 
 *2.120.00   30-Mar-2021   Prajwal         Container design change
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class LookupContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpGet]
        [Route("api/Lookups/LookupsContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            try
            {
                log.LogMethodEntry(rebuildCache, hash);
                LookupsContainerDTOCollection lookupsContainerDTOCollection = await
                           Task<LookupsContainerDTOCollection>.Factory.StartNew(() =>
                           {
                               return lookupsContainerDTOCollection = LookupsViewContainerList.GetLookupsContainerDTOCollection(siteId, hash, rebuildCache); //chnage later
                           });
                log.LogMethodExit(lookupsContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = lookupsContainerDTOCollection });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }
    }
}
