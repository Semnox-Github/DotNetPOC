/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - ReservationStatusController
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.110        21-Nov-2020   Girish Kundar        Created :  Payment link enhancement
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class ReservationStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [HttpPost]
        [Route("api/Transaction/Reservation/{bookingId}/Status/{status}")]
        [Authorize]
        public HttpResponseMessage Post([FromUri] int bookingId, string status)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {

                log.LogMethodEntry(bookingId, status);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                Utilities Utilities = new Utilities();
                Users user = new Users(executionContext, executionContext.GetUserId(), securityTokenDTO.SiteId);
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
                if (bookingId < 0 || string.IsNullOrEmpty(status))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }

                ReservationBL reservationBL = new ReservationBL(executionContext, Utilities, bookingId);
                reservationBL.ConfirmReservation(null);
                log.LogMethodExit(reservationBL.GetReservationDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = reservationBL.GetReservationDTO });
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

