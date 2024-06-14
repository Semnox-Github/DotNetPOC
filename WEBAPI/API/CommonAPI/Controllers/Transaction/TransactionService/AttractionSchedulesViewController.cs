/********************************************************************************************
 * Project Name - Products Controller/AttractionMasterSchedulesViewController
 * Description  - Created to Get, Post and Delete Attraction Master Schedule for Products-> Set Up -> Attraction Schedule  
 *  
 **************
 **Version Log
 **************
 *Version     Date            Created By          Remarks          
 ***************************************************************************************************
 *2.110        20-Jan-2020     Nitin               Created.
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.TransactionFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Transaction.TransactionService
{
    public class AttractionSchedulesViewController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        /// <summary>
        /// Gets the AttractionSchedule Collection based on isActive,scheduleDate,productsList,facilityMapId
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TransactionService/AttractionSchedulesView")]
        public HttpResponseMessage Get(DateTime scheduleDate, string productsList = null, int facilityMapId = -1,
                                         bool? fixedSchedule = null, decimal? scheduleFromTime = null, decimal? scheduleToTime = null,
                                         bool? includePast = false, bool? filterProducts = true, bool? isBooking = false, bool? isRescheduleSlot = false, 
                                         int pageNumber = 0, int pageSize = 20, bool? filterFacility = false, bool? removeUnavailable = true)
        {
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            SecurityTokenDTO securityTokenDTO;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(scheduleDate, productsList, facilityMapId, fixedSchedule, scheduleFromTime, scheduleToTime, includePast);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                AttractionBookingSchedulesBL attractionBookingScheduleBL = new AttractionBookingSchedulesBL(executionContext);

                scheduleDate = scheduleDate.Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 0));
                List<ScheduleDetailsViewDTO> scheduleDetailsDTOList = attractionBookingScheduleBL.BuildScheduleDetailsForView(scheduleDate, productsList, facilityMapId, fixedSchedule,
                    scheduleFromTime, scheduleToTime, null, includePast != null ? Convert.ToBoolean(includePast.ToString()) : false, filterProducts != null ? Convert.ToBoolean(filterProducts.ToString()) : true,
                    true, -1, isBooking != null ? Convert.ToBoolean(isBooking.ToString()) : true, isRescheduleSlot != null ? Convert.ToBoolean(isRescheduleSlot.ToString()) : false, pageNumber, pageSize, 
                    filterFacility : filterFacility, removeUnavailable: removeUnavailable);

                log.LogMethodExit(scheduleDetailsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = scheduleDetailsDTOList });
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