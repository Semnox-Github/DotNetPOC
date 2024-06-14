/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the DisplayPanelContainer
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *2.150.2    06-Dec-2022    Abhishek         Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.GenericUtilities;
using Semnox.CommonAPI.Helpers;
using System.Threading.Tasks;
using Semnox.Parafait.ViewContainer;

namespace Semnox.CommonAPI.DigitalSignage
{
    public class DisplayPanelContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Get the JSON Object Display Panel List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/DigitalSignage/DisplayPanelContainer")]
        [Authorize]
        public async Task<HttpResponseMessage> GetAsync(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;

            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                log.LogMethodEntry(siteId, rebuildCache, hash);
                DisplayPanelContainerDTOCollection displayPanelContainerDTOCollection = await
                          Task<DisplayPanelContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return DisplayPanelViewContainerList.GetDisplayPanelContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(displayPanelContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = displayPanelContainerDTOCollection });
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