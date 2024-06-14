/********************************************************************************************
 * Project Name - Products Controller/AttractionMasterScheduleController
 * Description  - Created to Get, Post and Delete Attraction Master Schedule for Products-> Set Up -> Attraction Schedule  
 *  
 **************
 **Version Log
 **************
 *Version     Date            Created By          Remarks          
 ***************************************************************************************************
 *2.80        20-Apr-2020     Girish Kundar       Created.
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
    public class AttractionSchedulesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Gets the AttractionSchedule Collection based on isActive,scheduleDate,productsList,facilityMapId
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TransactionService/AttractionSchedules")]
        public HttpResponseMessage Get(DateTime scheduleDate, string productsList = null, int facilityMapId = -1,
                                         bool? fixedSchedule = null, decimal? scheduleFromTime = null, decimal? scheduleToTime = null,
                                         bool? includePast = false, bool? filterProducts = true, bool? isBooking = false, bool? isRescheduleSlot = false,
                                         int pageNumber = 0, int pageSize = 20, bool? removeUnavailable = true)
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

                // Website is sending hour as 0, this is causing the schedule date to go to previous date
                if (scheduleDate.Hour == 0 && scheduleDate.Minute == 0)
                {
                    scheduleDate = scheduleDate.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 6));
                }

                AttractionBookingSchedulesBL attractionBookingScheduleBL = new AttractionBookingSchedulesBL(executionContext);
                List<ScheduleDetailsDTO> scheduleDetailsDTOList = attractionBookingScheduleBL.GetAttractionBookingSchedules(scheduleDate, productsList, facilityMapId, fixedSchedule, scheduleFromTime, scheduleToTime,
                    includePast != null ? Convert.ToBoolean(includePast.ToString()) : false, filterProducts != null ? Convert.ToBoolean(filterProducts.ToString()) : false, removeUnavailable: removeUnavailable);

                List<int> inputproductsList = new List<int>();
                if (!String.IsNullOrEmpty(productsList))
                {
                    String[] products = productsList.Split(',');
                    if (products != null && products.Count() > 0)
                    {
                        foreach (string product in products)
                            inputproductsList.Add(Convert.ToInt32(product));
                    }
                }

                if (scheduleDetailsDTOList != null && (filterProducts != null ? Convert.ToBoolean(filterProducts) : false) && inputproductsList.Any())
                {
                    log.Debug("filtering products : count before filtering " + scheduleDetailsDTOList.Count);
                    List<ScheduleDetailsDTO> filteredSchedulesList = new List<ScheduleDetailsDTO>();
                    foreach (ScheduleDetailsDTO existingSchedule in scheduleDetailsDTOList)
                    {
                        FacilityMapDTO facilityMapDTO = new FacilityMapDTO(
                            existingSchedule.FacilityMapDTO.FacilityMapId,
                            existingSchedule.FacilityMapDTO.FacilityMapName,
                            existingSchedule.FacilityMapDTO.MasterScheduleId,
                            existingSchedule.FacilityMapDTO.CancellationProductId,
                            existingSchedule.FacilityMapDTO.GraceTime,
                            existingSchedule.FacilityMapDTO.IsActive
                            );

                        facilityMapDTO.FacilityMapDetailsDTOList = existingSchedule.FacilityMapDTO.FacilityMapDetailsDTOList;
                        facilityMapDTO.ProductsAllowedInFacilityDTOList = existingSchedule.FacilityMapDTO.ProductsAllowedInFacilityDTOList.Where(x => inputproductsList.Any(y => y == x.ProductsId)).ToList();

                        ScheduleDetailsDTO scheduleDetailsDTO = new ScheduleDetailsDTO(
                            existingSchedule.FacilityMapId,
                            existingSchedule.FacilityMapName,
                            existingSchedule.MasterScheduleId,
                            existingSchedule.MasterScheduleName,
                            existingSchedule.ScheduleId,
                            existingSchedule.ScheduleName,
                            existingSchedule.ScheduleFromDate,
                            existingSchedule.ScheduleToDate,
                            existingSchedule.ScheduleFromTime,
                            existingSchedule.ScheduleToTime,
                            existingSchedule.FixedSchedule,
                            existingSchedule.AttractionPlayId,
                            existingSchedule.AttractionPlayName,
                            existingSchedule.ProductId,
                            existingSchedule.ProductName,
                            existingSchedule.Price,
                            existingSchedule.FacilityCapacity,
                            existingSchedule.RuleUnits,
                            existingSchedule.TotalUnits,
                            existingSchedule.BookedUnits,
                            existingSchedule.AvailableUnits,
                            existingSchedule.DesiredUnits,
                            existingSchedule.ExpiryDate,
                            existingSchedule.CategoryId,
                            existingSchedule.PromotionId,
                            existingSchedule.Seats,
                            existingSchedule.SiteId,
                            existingSchedule.AttractionPlayPrice,
                            facilityMapDTO
                            );

                        filteredSchedulesList.Add(scheduleDetailsDTO);
                    }

                    scheduleDetailsDTOList = filteredSchedulesList;
                    log.Debug("completed filtering products : count after filtering " + scheduleDetailsDTOList.Count);
                }

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