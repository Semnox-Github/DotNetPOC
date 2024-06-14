/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for Reservation
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80         16-Mar-2020   Mushahid Faizan      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using System.Linq;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System.Globalization;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class ReservationsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        Semnox.Core.Utilities.Utilities Utilities = new Semnox.Core.Utilities.Utilities();

        [HttpGet]
        [Route("api/Transaction/Reservations")]
        [Authorize]
        public HttpResponseMessage Get(string eventCode = null, int bookingId = -1, int bookingProductId = -1, int facilityMapId = -1, string reservationStatus = null,
                                              int transactionId = -1, DateTime? fromDate = null, DateTime? toDate = null, string relatedEventCode = null, string cardNumber = null,
                                              int checklistTaskAssigneeId = -1, string customerName = null, string reservationStatusNotIn = null, string transactionGuid = null)
        {
            try
            {
                log.LogMethodEntry(eventCode);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                //Utilities = new Semnox.Core.Utilities.Utilities();
                //Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                //Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                //Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                //Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                //Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                //Utilities.ParafaitEnv.Initialize();
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                List<KeyValuePair<ReservationDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReservationDTO.SearchByParameters, string>>();

                DateTime startDate = serverTimeObject.GetServerDateTime(); 
                DateTime endDate = serverTimeObject.GetServerDateTime().AddDays(1);

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = serverTimeObject.GetServerDateTime(); 
                }

                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(eventCode))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_CODE_EXACT, eventCode));
                }
                if (bookingId != -1)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.BOOKING_ID, bookingId.ToString()));
                }
                if (bookingProductId != -1)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.BOOKING_PRODUCT_ID, bookingProductId.ToString()));
                }
                if (!string.IsNullOrEmpty(cardNumber))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CARD_NUMBER_LIKE, cardNumber.ToString()));
                }
                if (!string.IsNullOrEmpty(relatedEventCode))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.RESERVATION_CODE_LIKE, relatedEventCode.ToString()));
                }
                if (checklistTaskAssigneeId != -1)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CHECKLIST_TASK_ASSIGNEE_ID, checklistTaskAssigneeId.ToString()));
                }
                if (facilityMapId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }

                if (!string.IsNullOrEmpty(customerName))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.CUSTOMER_NAME_LIKE, customerName.ToString()));
                }

                if (!string.IsNullOrEmpty(reservationStatus))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS, reservationStatus.ToString()));
                }
                if (!string.IsNullOrEmpty(reservationStatusNotIn))
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.STATUS_LIST_NOT_IN, reservationStatusNotIn.ToString()));
                }
                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRX_ID, transactionId.ToString()));
                }
                if (string.IsNullOrWhiteSpace(transactionGuid) == false)
                {
                    searchParameters.Add(new KeyValuePair<ReservationDTO.SearchByParameters, string>(ReservationDTO.SearchByParameters.TRANSACTION_GUID, transactionGuid.ToString()));
                }
                ReservationListBL reservationListBL = new ReservationListBL(executionContext);
                List<ReservationDTO> content = reservationListBL.GetReservationDTOList(searchParameters);

                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object ReservationDTO
        /// </summary>
        /// <param name="reservationDTO">ReservationDTO</param>
        [HttpPost]
        [Route("api/Transaction/Reservation")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] ReservationDTO reservationDTO)
        {
            try
            {
                log.LogMethodEntry(reservationDTO);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;

                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                Users user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
                Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
                Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
                Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
                Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                Utilities.ExecutionContext.SetUserId(user.UserDTO.LoginId);
                Utilities.ParafaitEnv.Initialize();

                if (reservationDTO != null)
                {
                    ReservationBL reservationBL = new ReservationBL(executionContext, Utilities, reservationDTO);
                    reservationBL.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = reservationDTO });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
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
