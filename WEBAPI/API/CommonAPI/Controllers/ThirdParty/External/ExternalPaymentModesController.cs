/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - Created to fetch Payment Mode details.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.140.4    18-Nov-2022   Abhishek                 Created - External  REST API
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.PaymentGateway;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalPaymentModesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON PaymentModesDTO
        /// </summary>       
        /// <param name="paymentChannel">paymentChannel</param>
        /// <param name="paymentModeId">paymentModeId</param>
        /// <param name="paymentMode">paymentMode</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Transaction/PaymentModes")]
        public HttpResponseMessage Get(string paymentChannel = null, int paymentModeId = -1, string paymentMode = null, bool loadChildRecords = false, string isActive = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(paymentChannel, paymentModeId, paymentMode, loadChildRecords, isActive);
               
                List<PaymentModeDTO> paymentModeDTOList = new List<PaymentModeDTO>();
                List<int> paymentModeIdList = new List<int>();
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                
                PaymentModeList paymentModeListBL = new PaymentModeList(executionContext);
                List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>> searchPaymentModeParameters = new List<KeyValuePair<PaymentModeDTO.SearchByParameters, string>>();
                searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (string.IsNullOrWhiteSpace(paymentChannel) == false)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupSearchParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, paymentChannel));
                    lookupSearchParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupSearchParameters);
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                    {
                        PaymentChannelList paymentChannelList = new PaymentChannelList(executionContext);
                        List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>> searchChannelParameters = new List<KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>>();
                        searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.LOOKUP_VALUE_ID, lookupValuesDTOList[0].LookupValueId.ToString()));
                        searchChannelParameters.Add(new KeyValuePair<PaymentModeChannelsDTO.SearchByParameters, string>(PaymentModeChannelsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<PaymentModeChannelsDTO> paymentChannelsDTOList = paymentChannelList.GetAllPaymentChannels(searchChannelParameters);
                        if (paymentChannelsDTOList != null && paymentChannelsDTOList.Any())
                        {
                            paymentModeIdList = paymentChannelsDTOList.Select(x => x.PaymentModeId).ToList();
                        }
                        if (paymentModeIdList.Any())
                        {
                            searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID_LIST, string.Join(",", paymentModeIdList).ToString()));
                        }
                    }
                    else
                    {
                        log.Error("Invalid payment channel");
                        throw new Exception("Invalid payment channel");
                    }
                }
                if (string.IsNullOrWhiteSpace(paymentChannel) && paymentModeId > -1)
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE_ID, paymentModeId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(paymentMode))
                {
                    searchPaymentModeParameters.Add(new KeyValuePair<PaymentModeDTO.SearchByParameters, string>(PaymentModeDTO.SearchByParameters.PAYMENT_MODE, paymentMode));
                }

                paymentModeDTOList = paymentModeListBL.GetAllPaymentModeList(searchPaymentModeParameters, loadChildRecords);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = paymentModeDTOList });

            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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