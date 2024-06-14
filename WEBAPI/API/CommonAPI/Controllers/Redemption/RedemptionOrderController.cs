/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the RedemptionOrderController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     20-Nov-2020     Vikas Dwivedi       Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class RedemptionOrderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/RedemptionOrders")]
        public async Task<HttpResponseMessage> Get(int orderId = -1, int posMachineId = -1, string orderNumber = null, string cardNumber = null, DateTime? fromDate = null, DateTime? toDate = null,
            string status = null, string giftBarCode = null, string productCode = null, string description = null, string isActive = null, bool loadAChildRecords = false, bool activeChildRecords = false,
            bool buildReceipt = false, string loadGiftCardTicketAllocationDetails = null)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderId, posMachineId, orderNumber, cardNumber, fromDate, toDate, status, giftBarCode, productCode, description, isActive, activeChildRecords, buildReceipt);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<RedemptionDTO.SearchByParameters, string>> redemptionOrderSeacrhParams = new List<KeyValuePair<RedemptionDTO.SearchByParameters, string>>();
                redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (orderId > 0)
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEPTION_ID, orderId.ToString()));
                }
                if (posMachineId > 0)
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));

                }
                if (!string.IsNullOrEmpty(orderNumber))
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_ORDER_NO, orderNumber));
                }
                if (!string.IsNullOrEmpty(cardNumber))
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.CARD_NUMBER, cardNumber));
                }
                if (!string.IsNullOrEmpty(status))
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.REDEMPTION_STATUS, status));
                }
                if (!string.IsNullOrEmpty(giftBarCode))
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.GIFT_CODE_DESC_BARCODE, giftBarCode));
                }
                if (!string.IsNullOrEmpty(loadGiftCardTicketAllocationDetails))
                {
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.LOAD_GIFT_CARD_TICKET_ALLOCATION_DETAILS, loadGiftCardTicketAllocationDetails));
                }

                if (fromDate != null)
                {
                    DateTime dateFrom = Convert.ToDateTime(fromDate);
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.FROM_REDEMPTION_DATE, dateFrom.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (toDate != null)
                {
                    DateTime dateTo = Convert.ToDateTime(toDate);
                    redemptionOrderSeacrhParams.Add(new KeyValuePair<RedemptionDTO.SearchByParameters, string>(RedemptionDTO.SearchByParameters.TO_REDEMPTION_DATE, dateTo.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                List<RedemptionDTO> redemptionDTOList = await redemptionUseCases.GetRedemptionOrders(redemptionOrderSeacrhParams);
                log.LogMethodExit(redemptionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionDTOList });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new{ data = customException });
            }
        }

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Redemption/RedemptionOrders")]
        public async Task<HttpResponseMessage> Post(List<RedemptionDTO> redemptionDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(redemptionDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                IRedemptionUseCases redemptionUseCases = RedemptionUseCaseFactory.GetRedemptionUseCases(executionContext);
                var content = await redemptionUseCases.SaveRedemptionOrders(redemptionDTOList);
                log.LogMethodExit(redemptionDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = redemptionDTOList });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
