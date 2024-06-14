/********************************************************************************************
 * Project Name - POS
 * Description  - Created to fetch, update and insert in the Override Items.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    30-Dec-2020   Dakshakh Raj             Created as part of Peru Invoice changes
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Parafait.POS;
using Semnox.Core.GenericUtilities;
using System.Threading.Tasks;
using System.Linq;
using Semnox.CommonAPI.Helpers;

namespace Semnox.CommonAPI.POS
{
    public class OverrideItemsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON POS Printer Override Rules.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/POS/OverrideItems")]
        public async Task<HttpResponseMessage> Get(string optionItemCode = null, string optionItemName = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(optionItemCode, optionItemName);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>>();
                if (!string.IsNullOrEmpty(optionItemCode))
                {
                    searchParameters.Add(new KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>(OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_CODE, optionItemCode.ToString()));
                }
                if (!string.IsNullOrEmpty(optionItemName))
                {
                    searchParameters.Add(new KeyValuePair<OverrideOptionItemDTO.SearchByParameters, string>(OverrideOptionItemDTO.SearchByParameters.OPTION_ITEM_NAME, optionItemName.ToString()));
                }
                IOverrideItemUseCases overrideItemUseCases = POSUseCaseFactory.GetOverrideItemsUseCases(executionContext);
                List<OverrideOptionItemDTO> OverrideOptionItemDTOList = await overrideItemUseCases.GetOverrideOptionItems(searchParameters, null);

                log.LogMethodExit(OverrideOptionItemDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = OverrideOptionItemDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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