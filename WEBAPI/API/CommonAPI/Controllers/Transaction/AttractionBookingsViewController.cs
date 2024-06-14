/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the AttractionBookings
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By                 Remarks          
 *********************************************************************************************
 *2.80        08-Apr-2020           Girish Kundar               Created 
 *2.110       20-Jan-2021           Nitin                       Fix: Consider schedule date and not from date in DB
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.TransactionFunctions;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class AttractionBookingsViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        /// <summary>
        /// Gets the AttractionBookings Collection based on Get parameters.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/AttractionBookingsView")]
        public HttpResponseMessage Get(int bookingId = -1, int scheduleId = -1, bool loadChildRecords = false,
                                       int attractionPlayId = -1, int trxId = -1, int facilityMapId = -1, string externalReference = null,
                                       DateTime? attractionDate = null, string cardNumber = null, int cardId = -1, int customerId = -1)
        {
            log.LogMethodEntry(facilityMapId, bookingId, scheduleId, loadChildRecords, attractionPlayId, trxId, externalReference, attractionDate);
            try
            {
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<AttractionBookingViewDTO> attractionBookingViewDTOList = new List<AttractionBookingViewDTO>();
                AttractionBookingList attractionBookingList = new AttractionBookingList(executionContext);
                List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_UNEXPIRED, "Y"));
                if (bookingId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_BOOKING_ID, bookingId.ToString()));
                }
                if (trxId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.TRX_ID, trxId.ToString()));
                }
                if (facilityMapId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                if (attractionPlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_PLAY_ID, attractionPlayId.ToString()));
                }
                if (scheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (attractionDate != null)
                {
                    DateTime dateTime = Convert.ToDateTime(attractionDate);
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_DATE, dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                }
                if (string.IsNullOrEmpty(externalReference) == false)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, externalReference));
                }
                if (!string.IsNullOrEmpty(cardNumber) && cardId == -1)
                {
                    AccountBL accountBL = new AccountBL(executionContext, cardNumber);
                    if (accountBL.AccountDTO != null && accountBL.AccountDTO.AccountId > -1)
                        cardId = accountBL.AccountDTO.AccountId;
                }
                if (cardId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.CARD_ID, cardId.ToString()));
                }
                if (customerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                attractionBookingViewDTOList = attractionBookingList.GetAttractionBookingViewDTOList(searchParameters, loadChildRecords);
                log.LogMethodExit(attractionBookingViewDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = attractionBookingViewDTOList });

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
