/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Token Card Inventory Controller
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     25-May-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Task
{
    public class TokenInventoryController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        [HttpGet]
        [Authorize]
        [Route("api/Task/TokenCardInventory")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, DateTime? date = null, int cardInventoryKey = -1, DateTime? fromDate = null, DateTime? toDate = null, string action = null, int tagType = -1, int activityType = -1, int machineType = -1, int masterEntityId = -1, string addCardKey = null)
        {
            executionContext = null;
            List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>> searchParameters = new List<KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>>();
            searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.SITE_ID, siteId.ToString()));
            if(date != null)
            {
                DateTime lastSundayDate = Convert.ToDateTime(date);
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.DATE, lastSundayDate.ToString()));
            }
            if(fromDate != null)
            {
                DateTime dateFrom = Convert.ToDateTime(fromDate);
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.FROM_DATE, dateFrom.ToString("MM-dd-yyyy",CultureInfo.InvariantCulture)));
            }
            if(toDate != null)
            {
                DateTime dateTo = Convert.ToDateTime(toDate);
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TO_DATE, dateTo.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture)));
            }
            if(cardInventoryKey > 0)
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.CARD_INVENTORY_KEY, cardInventoryKey.ToString()));
            }
            if (tagType > 0)
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.TAG_TYPE, tagType.ToString()));
            }
            if(machineType > 0)
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MACHINE_TYPE, machineType.ToString()));
            }
            if (activityType > 0)
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTIVITY_TYPE, activityType.ToString()));
            }
            if(masterEntityId > 0)
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.MASTER_ENTITY_ID, masterEntityId.ToString()));
            }
            if (!string.IsNullOrEmpty(action))
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ACTION, action));
            }
            if (!string.IsNullOrEmpty(addCardKey))
            {
                searchParameters.Add(new KeyValuePair<TokenCardInventoryDTO.SearchByTokenCardInventoryParameters, string>(TokenCardInventoryDTO.SearchByTokenCardInventoryParameters.ADDCARD_KEY, addCardKey));
            }
            
            
            try
            {
                log.LogMethodEntry(siteId, date);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITokenCardInventoryUseCases tokenCardInventoryUseCases = TokenCardInventoryUseCaseFactory.GetTokenCardInventoryUseCases(executionContext);
                List<TokenCardInventoryDTO> tokenCardInventoryDTOList = await tokenCardInventoryUseCases.GetAllTokenCardInventoryDTOsList(searchParameters);
                log.LogMethodExit(tokenCardInventoryDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = tokenCardInventoryDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        [HttpPost]
        [Route("api/Task/TokenCardInventory")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            executionContext = null;           
            try
            {
                log.LogMethodEntry(tokenCardInventoryDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITokenCardInventoryUseCases tokenCardInventoryUseCases = TokenCardInventoryUseCaseFactory.GetTokenCardInventoryUseCases(executionContext);
                if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                {
                    tokenCardInventoryUseCases.SaveCardInventory(tokenCardInventoryDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = tokenCardInventoryDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.LogMethodExit(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        [HttpPut]
        [Route("api/Task/TokenCardInventory")]
        [Authorize]
        public HttpResponseMessage Put([FromBody] List<TokenCardInventoryDTO> tokenCardInventoryDTOList)
        {
            executionContext = null;
            try
            {
                log.LogMethodEntry(tokenCardInventoryDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                ITokenCardInventoryUseCases tokenCardInventoryUseCases = TokenCardInventoryUseCaseFactory.GetTokenCardInventoryUseCases(executionContext);
                if (tokenCardInventoryDTOList != null && tokenCardInventoryDTOList.Count > 0)
                {
                    tokenCardInventoryUseCases.UpdateCardInventory(tokenCardInventoryDTOList);
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = tokenCardInventoryDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.LogMethodExit(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}