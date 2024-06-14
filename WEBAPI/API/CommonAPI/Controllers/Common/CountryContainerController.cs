/********************************************************************************************
 * Project Name - CommonAPI                                                                    
 * Description  - Controller of the Country.
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.120.00    08-Jul-2021   Roshan Devadiga       Created : POS UI redesign
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.ViewContainer;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Common
{
    public class CountryContainerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/CountriesContainer")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, string hash = null, bool rebuildCache = false)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);

                CountryContainerDTOCollection countryContainerDTOCollection = await
                          Task<CountryContainerDTOCollection>.Factory.StartNew(() =>
                          {
                              return CountryViewContainerList.GetCountryContainerDTOCollection(siteId, hash, rebuildCache);
                          });

                log.LogMethodExit(countryContainerDTOCollection);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = countryContainerDTOCollection });
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
