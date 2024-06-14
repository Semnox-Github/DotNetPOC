/********************************************************************************************
 * Project Name - Transaction Services - RescheduleAttractionBookingBL
 * Description  - BL to Reschedule Attractions Bookings and Attraction Schedules
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100      24-Sep-2020   Nitin Pai                Created
 *2.110      20-Jan-2021   Nitin Pai                Performance fixes. Club Speed Integration fixes.
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction.TransactionFunctions;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business Logic to Reschedule Attractions Bookings and Day Attraction Schedules
    /// </summary>
    public class RescheduleAttractionBookingsBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Dictionary<int, ProductsDTO> productsDictionary = new Dictionary<int, ProductsDTO>();
        private List<TransactionLineDTO> transactionLineDTOList = new List<TransactionLineDTO>();

        private DayAttractionScheduleDTO sourceDayAttractionScheduleDTO;
        private DayAttractionScheduleDTO targetDayAttractionScheduleDTO;

        private List<AttractionBookingDTO> sourceAttractionBookingDTOList;
        private List<AttractionBookingDTO> targetAttractionBookingDTOList;

        private static Dictionary<int, List<int>> relatedFacilityMaps = new Dictionary<int, List<int>>();

        private RescheduleAttractionBookingsBL()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sourceDayAttractionScheduleDTO">Source day attraction schedule</param>
        /// <param name="targetDayAttractionScheduleDTO">Target day attraction schedule</param>
        /// <returns></returns>
        public RescheduleAttractionBookingsBL(ExecutionContext executionContext)
            : this()
        {
            log.LogMethodEntry(executionContext, sourceDayAttractionScheduleDTO, targetDayAttractionScheduleDTO);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sourceDayAttractionScheduleDTO">Source day attraction schedule</param>
        /// <param name="targetDayAttractionScheduleDTO">Target day attraction schedule</param>
        /// <returns></returns>
        public RescheduleAttractionBookingsBL(ExecutionContext executionContext, DayAttractionScheduleDTO sourceDayAttractionScheduleDTO, DayAttractionScheduleDTO targetDayAttractionScheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, sourceDayAttractionScheduleDTO, targetDayAttractionScheduleDTO);
            this.sourceDayAttractionScheduleDTO = sourceDayAttractionScheduleDTO;
            this.targetDayAttractionScheduleDTO = targetDayAttractionScheduleDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sourceAttractionBookingDTOList">Source day attraction schedule</param>
        /// <param name="targetAttractionBookingDTOList">Target day attraction schedule</param>
        /// <returns></returns>
        public RescheduleAttractionBookingsBL(ExecutionContext executionContext, List<AttractionBookingDTO> sourceAttractionBookingDTOList, List<AttractionBookingDTO> targetAttractionBookingDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, sourceAttractionBookingDTOList, targetAttractionBookingDTOList);
            this.sourceAttractionBookingDTOList = sourceAttractionBookingDTOList;
            this.targetAttractionBookingDTOList = targetAttractionBookingDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Calculates the list of impacted schedules
        /// </summary>
        /// <param name="scheduleDate">Schedule Date</param>
        /// <param name="facilityMapId">Facility Map</param>
        /// <param name="attractionScheduleId">Id of the schedule from which the impact is calculated</param>
        /// <returns name="ScheduleDetailsDTO"></returns>
        public List<ScheduleDetailsDTO> MoveAttractionSchedulesImpactedSlots(DateTime scheduleDate, int facilityMapId, int attractionScheduleId, int sourceAttractionScheduleId)
        {
            log.LogMethodEntry(scheduleDate, facilityMapId, attractionScheduleId);
            List<ScheduleDetailsDTO> impactedScheduleDetailsDTO = new List<ScheduleDetailsDTO>();

            int businessDayStartTime = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME");

            DateTime businessDay = scheduleDate.Date;
            if (scheduleDate.Hour < businessDayStartTime)
                businessDay = businessDay.AddDays(-1);

            businessDay = businessDay.AddHours(businessDayStartTime);

            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            AttractionBookingSchedulesBL attractionBookingScheduleBL = new AttractionBookingSchedulesBL(executionContext);
            scheduleDetailsDTOList = attractionBookingScheduleBL.GetAttractionBookingSchedules(businessDay, "", facilityMapId, null, 0, 24, true);

            List<int> facilityMapList = new List<int>();
            if (relatedFacilityMaps.ContainsKey(facilityMapId))
            {
                facilityMapList = relatedFacilityMaps[facilityMapId];
            }
            else
            {
                FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                facilityMapList = facilityMapListBL.GetFacilityMapsForSameFacility(facilityMapId);
                relatedFacilityMaps.Add(facilityMapId, facilityMapList);
            }

            if (scheduleDetailsDTOList != null && scheduleDetailsDTOList.Any())
            {
                ScheduleDetailsDTO startingScheduleDTO = scheduleDetailsDTOList.Where(x => x.ScheduleId == attractionScheduleId && relatedFacilityMaps[facilityMapId].Contains(x.FacilityMapId)).FirstOrDefault();
                if(startingScheduleDTO == null)
                {
                    throw new Exception("Selected Schedule not found");
                }
                List<ScheduleDetailsDTO> availableSchedulesDTO = new List<ScheduleDetailsDTO>();
                availableSchedulesDTO.Add(startingScheduleDTO);

                availableSchedulesDTO.AddRange(scheduleDetailsDTOList.Where(x => x.ScheduleFromDate > startingScheduleDTO.ScheduleFromDate).ToList());

                if (availableSchedulesDTO == null || !availableSchedulesDTO.Any())
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2811));
                }

                List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = new List<DayAttractionScheduleDTO>();
                DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
                List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME, availableSchedulesDTO[0].ScheduleFromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME, availableSchedulesDTO[availableSchedulesDTO.Count -1].ScheduleToDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_UN_EXPIRED, ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, null);
                if (dayAttractionScheduleDTOList != null && dayAttractionScheduleDTOList.Any())
                {
                    dayAttractionScheduleDTOs.AddRange(dayAttractionScheduleDTOList);
                }

                bool finalSlotFound = false;
                foreach(ScheduleDetailsDTO scheduleDetailsDTO in availableSchedulesDTO)
                {
                    DayAttractionScheduleDTO dayAttractionScheduleDTO = dayAttractionScheduleDTOs.Where(x => x.AttractionScheduleId == scheduleDetailsDTO.ScheduleId
                        && relatedFacilityMaps[facilityMapId].Contains(x.FacilityMapId)).FirstOrDefault();

                    if (dayAttractionScheduleDTO == null)
                    {
                        impactedScheduleDetailsDTO.Add(scheduleDetailsDTO);
                        finalSlotFound = true;
                        break;
                    }
                    else
                    {
                        // a slot is found in day attraction schedule table, skip if it is a blocked or reserverd slot
                        if (dayAttractionScheduleDTO.ScheduleStatus == DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.BLOCKED)
                            || dayAttractionScheduleDTO.Source == DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION))
                            continue;
                        else if (dayAttractionScheduleDTO.ScheduleStatus == DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE)
                            || dayAttractionScheduleDTO.ScheduleStatus != DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN))
                            throw new Exception(MessageContainerList.GetMessage(executionContext, 2811));
                        else if (dayAttractionScheduleDTO.AttractionScheduleId == sourceAttractionScheduleId)
                        {
                            // this is the scenario where the customer is trying to do a flip. Move the source and target. Valid scenario as the source is not free
                            scheduleDetailsDTO.DayAttractionScheduleId = dayAttractionScheduleDTO.DayAttractionScheduleId;
                            impactedScheduleDetailsDTO.Add(scheduleDetailsDTO);
                            finalSlotFound = true;
                            break;
                        }
                        else
                        {
                            scheduleDetailsDTO.DayAttractionScheduleId = dayAttractionScheduleDTO.DayAttractionScheduleId;
                            impactedScheduleDetailsDTO.Add(scheduleDetailsDTO);
                        }
                    }
                }

                if (!finalSlotFound)
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2811));
                }
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2811));
            }

            log.LogMethodExit(impactedScheduleDetailsDTO);
            return impactedScheduleDetailsDTO;
        }

        /// <summary>
        /// Moves the schedules from source to target
        /// </summary>
        /// <param name="inSqlTransaction">SQl transaction</param>
        /// <returns></returns>
        public void MoveAttractionSchedules(SqlTransaction inSqlTransaction = null)
        {
            log.LogMethodEntry(sourceDayAttractionScheduleDTO, targetDayAttractionScheduleDTO);

            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = new List<DayAttractionScheduleDTO>();
            SqlTransaction sqlTransaction = null;
            SqlConnection conn = null;

            if (inSqlTransaction == null)
            {
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                conn = utilities.createConnection();
                sqlTransaction = conn.BeginTransaction();
            }
            else
            {
                sqlTransaction = inSqlTransaction;
            }

            try
            {
                List<ScheduleDetailsDTO> impactedSchedulesDetailsDTO = MoveAttractionSchedulesImpactedSlots(targetDayAttractionScheduleDTO.ScheduleDateTime, targetDayAttractionScheduleDTO.FacilityMapId,
                                                                            targetDayAttractionScheduleDTO.AttractionScheduleId, sourceDayAttractionScheduleDTO.AttractionScheduleId);
                if (impactedSchedulesDetailsDTO == null || !impactedSchedulesDetailsDTO.Any())
                {
                    throw new Exception(MessageContainerList.GetMessage(executionContext, 2811));
                }

                dayAttractionScheduleDTOList.Add(sourceDayAttractionScheduleDTO);
                // Now mark all impacted slots as blocked to prevent simultaneous updates
                int count = impactedSchedulesDetailsDTO.Count;
                for (int i = 0; i < count; i++)
                {
                    ScheduleDetailsDTO tempScheduleDetailsDTO = impactedSchedulesDetailsDTO[i];
                    if (tempScheduleDetailsDTO.DayAttractionScheduleId == -1)
                    {
                        DayAttractionScheduleDTO tempDASDTO = new DayAttractionScheduleDTO();
                        tempDASDTO.FacilityMapId = tempScheduleDetailsDTO.FacilityMapId;
                        tempDASDTO.AttractionPlayId = tempScheduleDetailsDTO.AttractionPlayId;
                        tempDASDTO.AttractionPlayName = tempScheduleDetailsDTO.AttractionPlayName;
                        tempDASDTO.AttractionScheduleId = tempScheduleDetailsDTO.ScheduleId;
                        tempDASDTO.AttractionScheduleName = tempScheduleDetailsDTO.ScheduleName;
                        tempDASDTO.ScheduleDate = tempScheduleDetailsDTO.ScheduleFromDate.Date;
                        tempDASDTO.ScheduleDateTime = tempScheduleDetailsDTO.ScheduleFromDate;
                        tempDASDTO.ScheduleToDateTime = tempScheduleDetailsDTO.ScheduleToDate;
                        tempDASDTO.ScheduleFromTime = tempScheduleDetailsDTO.ScheduleFromTime;
                        tempDASDTO.ScheduleToTime = tempScheduleDetailsDTO.ScheduleToTime;
                        dayAttractionScheduleDTOList.Add(tempDASDTO);
                    }
                    else
                    {
                        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, tempScheduleDetailsDTO.DayAttractionScheduleId);
                        dayAttractionScheduleDTOList.Add(dayAttractionScheduleBL.GetDayAttractionScheduleDTO);
                    }
                }

                // block schedules for concurrecy
                BlockSchedule(dayAttractionScheduleDTOList);

                Dictionary<int, double> secondsToMoveMap = new Dictionary<int, double>();
                count = dayAttractionScheduleDTOList.Count;

                // Traverse through the list and update the value of each DASDTO from successive DASDTO
                for (int i=0; i < count -1; i++)
                {
                    DayAttractionScheduleDTO source = dayAttractionScheduleDTOList[i];
                    DayAttractionScheduleDTO target = dayAttractionScheduleDTOList[i+1];
                    double secondsToMove1 = (target.ScheduleDateTime - source.ScheduleDateTime).TotalSeconds;
                    secondsToMoveMap.Add(source.DayAttractionScheduleId, secondsToMove1);
                    source.AttractionScheduleId = target.AttractionScheduleId;
                    source.ScheduleDate = target.ScheduleDate;
                    source.ScheduleDateTime = target.ScheduleDateTime;
                    source.ScheduleToDateTime = target.ScheduleToDateTime;
                    source.AttractionPlayId = target.AttractionPlayId;
                    source.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE);
                }
                // update status of the last impacted slot also
                dayAttractionScheduleDTOList[count - 1].ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE_COMPLETE);
                dayAttractionScheduleDTOList[count - 1].IsActive = false;
                dayAttractionScheduleDTOList[count - 1].Blocked = false;

                for (int i = count - 1; i >= 0; i--)
                {
                    DayAttractionScheduleDTO tempDASDTO = dayAttractionScheduleDTOList[i];
                    if (tempDASDTO.DayAttractionScheduleId == dayAttractionScheduleDTOList[0].DayAttractionScheduleId && i > 0)
                    {
                        //do nothing if this is the same as the origin sort, it is a flip scenario
                    }
                    else
                    {
                        DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, tempDASDTO);
                        try
                        {
                            dayAttractionScheduleBL.Save();
                        }
                        catch (Exception ex)
                        {
                            log.Debug("validate day attraction schedule failed." + ex.Message);
                            throw;
                        }

                        // get list of attraction bookings
                        AttractionBookingList attractionBookingList = new AttractionBookingList(executionContext);
                        List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> atbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                        atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, tempDASDTO.DayAttractionScheduleId.ToString()));
                        atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "Y"));
                        List<AttractionBookingDTO> attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(atbSearchParams, true, sqlTransaction);

                        // get seconds to move
                        double secondsToMove = 0;
                        if (secondsToMoveMap.ContainsKey(tempDASDTO.DayAttractionScheduleId))
                        {
                            secondsToMove = secondsToMoveMap[tempDASDTO.DayAttractionScheduleId];
                        }

                        // move card entitlements
                        if (attractionBookingDTOList != null && attractionBookingDTOList.Any() && secondsToMove != 0)
                            RescheduleCardEntitlements(attractionBookingDTOList, secondsToMove, tempDASDTO.ScheduleToDateTime, sqlTransaction);
                    }
                }

                if (inSqlTransaction == null)
                {
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                // un block - blocked schedules
                BlockSchedule(dayAttractionScheduleDTOList, false);

                if (inSqlTransaction == null)
                {
                    sqlTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (inSqlTransaction == null)
                {
                    conn.Close();
                }
            }
        }

        private void BlockSchedule(List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs, bool block = true)
        {
            log.LogMethodEntry(dayAttractionScheduleDTOs);
            for(int i = 0; i < dayAttractionScheduleDTOs.Count(); i++)
            {
                DayAttractionScheduleDTO dayAttractionScheduleDTO = dayAttractionScheduleDTOs[i];

                if (dayAttractionScheduleDTO.DayAttractionScheduleId == dayAttractionScheduleDTOs[0].DayAttractionScheduleId && i > 0)
                {
                    //do nothing
                }
                else
                {
                    // Update source DTO attributes to target DTO
                    dayAttractionScheduleDTO.ScheduleStatus = block ? DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.RESCHEDULE)
                                                                : DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);
                    DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, dayAttractionScheduleDTO);
                    try
                    {
                        dayAttractionScheduleBL.Save();
                    }
                    catch (RowNotInTableException ex)
                    {
                        dayAttractionScheduleDTO.Blocked = block;
                        dayAttractionScheduleDTO.ExpiryTime = DateTime.MinValue;
                        dayAttractionScheduleBL.Save();
                    }
                    catch (Exception ex)
                    {
                        log.Debug("validate day attraction schedule failed." + ex.Message);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Moves the attraction bookings from source day attraction schedule to target day attraction schedules
        /// </summary>
        /// <param name="inSqlTransaction">SQl transaction</param>
        /// <returns></returns>
        public void MoveAttractionBookings(SqlTransaction inSqlTransaction = null)
        {
            log.LogMethodEntry(sourceDayAttractionScheduleDTO, targetDayAttractionScheduleDTO);

            SqlTransaction sqlTransaction = null;
            SqlConnection conn = null;

            if (inSqlTransaction == null)
            {
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                conn = utilities.createConnection();
                sqlTransaction = conn.BeginTransaction();
            }
            else
            {
                sqlTransaction = inSqlTransaction;
            }

            try
            {
                // get list of attraction bookings
                AttractionBookingList attractionBookingList = new AttractionBookingList(executionContext);
                List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> atbSearchParams = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, sourceDayAttractionScheduleDTO.DayAttractionScheduleId.ToString()));
                atbSearchParams.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.IS_EXPIRY_DATE_EXPIRED, "Y"));
                List<AttractionBookingDTO> attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(atbSearchParams, true, sqlTransaction);
                if (attractionBookingDTOList == null || !attractionBookingDTOList.Any())
                {
                    String errorMessage = MessageContainerList.GetMessage(executionContext, 2813);
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }

                RescheduleAttractionLines(attractionBookingDTOList, sourceDayAttractionScheduleDTO, targetDayAttractionScheduleDTO, sqlTransaction);

                // Inactivate source DASDTO
                sourceDayAttractionScheduleDTO.ExpiryTime = ServerDateTime.Now;
                sourceDayAttractionScheduleDTO.IsActive = false;
                sourceDayAttractionScheduleDTO.Blocked = false;
                DayAttractionScheduleBL sourceDayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, sourceDayAttractionScheduleDTO);
                try
                {
                    List<ValidationError> validationErrors = sourceDayAttractionScheduleBL.Validate(sqlTransaction);
                    if (validationErrors == null || !validationErrors.Any())
                    {
                        sourceDayAttractionScheduleBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("validate day attraction schedule failed." + ex.Message);
                    throw;
                }

                if (inSqlTransaction == null)
                {
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                if (inSqlTransaction == null)
                {
                    sqlTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (inSqlTransaction == null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// Reschedule Attraction Bookings. 
        /// The target should have the same number of ATB as source. 
        /// Each Trx and TrxLine entry from source should have an entry in target.
        /// </summary>
        /// <param name="sourceAttractionBookingDTOList">Source ATB</param>
        /// <param name="targetAttractionBookingDTOList">Target ARB</param>
        /// <param name="inSqlTransaction">SQl transaction</param>
        /// <returns></returns>
        public void RescheduleAttractionBookings(SqlTransaction inSqlTransaction = null)
        {
            log.LogMethodEntry(sourceAttractionBookingDTOList, targetAttractionBookingDTOList);

            SqlTransaction sqlTransaction = null;
            SqlConnection conn = null;

            if (inSqlTransaction == null)
            {
                Semnox.Core.Utilities.Utilities utilities = new Semnox.Core.Utilities.Utilities();
                utilities.ParafaitEnv.User_Id = executionContext.GetUserPKId();
                utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
                utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                conn = utilities.createConnection();
                sqlTransaction = conn.BeginTransaction();
            }
            else
            {
                sqlTransaction = inSqlTransaction;
            }

            try
            {
                if (sourceAttractionBookingDTOList.Count != targetAttractionBookingDTOList.Count)
                {
                    String errorMessage = MessageContainerList.GetMessage(executionContext, 2814);
                    throw new Exception(errorMessage);
                }

                //loop till all the attractions are processed
                while (targetAttractionBookingDTOList.Any())
                {
                    // get the first attraction to process
                    AttractionBookingDTO targetATBDTO = targetAttractionBookingDTOList[0];

                    // get all attraction bookings from the target with same schedule
                    List<AttractionBookingDTO> tempSourceList = new List<AttractionBookingDTO>();
                    List<AttractionBookingDTO> tempTargetList = targetAttractionBookingDTOList.Where(x => x.DayAttractionScheduleDTO.AttractionScheduleId == targetATBDTO.DayAttractionScheduleDTO.AttractionScheduleId
                                                            && x.DayAttractionScheduleDTO.ScheduleDate == targetATBDTO.DayAttractionScheduleDTO.ScheduleDate
                                                            && x.DayAttractionScheduleDTO.FacilityMapId == targetATBDTO.DayAttractionScheduleDTO.FacilityMapId).ToList();

                    // iterate through this list and get the corresponding atb from source
                    if (tempTargetList != null && tempTargetList.Any())
                    {
                        // iterate through the source
                        foreach(AttractionBookingDTO tempDTO in tempTargetList)
                        {
                            // get the matching attraction booking from source, match is based on transaction and transaction line 
                            tempSourceList.AddRange(sourceAttractionBookingDTOList.Where(x => x.TrxId == tempDTO.TrxId && x.LineId == tempDTO.LineId).ToList());
                        }
                    }

                    if(tempSourceList.Count != tempTargetList.Count)
                    {
                        String errorMessage = MessageContainerList.GetMessage(executionContext, 2814);
                        throw new Exception(errorMessage);
                    }

                    // now we have the filtered source and target lists.
                    // All targets belong to same Schedule slot, Sources can belong to different slots
                    // Not extract distinct list of day attraction schedules from source
                    IEnumerable<int> sourceScheduleIdList = tempSourceList.Select(x => x.DayAttractionScheduleDTO.AttractionScheduleId).Distinct();
                    if(sourceScheduleIdList != null && sourceScheduleIdList.Any())
                    {
                        foreach (int sourceScheduleId in sourceScheduleIdList)
                        {
                            AttractionBookingDTO sourceATBDTO = tempSourceList.Where(x => x.DayAttractionScheduleDTO.AttractionScheduleId == sourceScheduleId).FirstOrDefault();
                            DayAttractionScheduleDTO sourceDASDTO = sourceATBDTO.DayAttractionScheduleDTO;
                            // Reschedule lines
                            RescheduleAttractionLines(tempSourceList.Where(x => x.DayAttractionScheduleDTO == sourceDASDTO).ToList(), sourceDASDTO, targetATBDTO.DayAttractionScheduleDTO, sqlTransaction);

                            // Inactivate source DASDTO
                            sourceDASDTO.ExpiryTime = ServerDateTime.Now;
                            sourceDASDTO.IsActive = false;
                            sourceDASDTO.Blocked = false;
                            DayAttractionScheduleBL sourceDayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, sourceDASDTO);
                            try
                            {
                                List<ValidationError> validationErrors = sourceDayAttractionScheduleBL.Validate(sqlTransaction);
                                if (validationErrors == null || !validationErrors.Any())
                                {
                                    sourceDayAttractionScheduleBL.Save(sqlTransaction);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Debug("validate day attraction schedule failed." + ex.Message);
                                throw;
                            }
                        }
                    }

                    // Now remove all the processed DTO from the source list
                    foreach (AttractionBookingDTO tempDTO in tempTargetList)
                    {
                        // get the matching attraction booking from source, match is based on transaction and transaction line 
                        targetAttractionBookingDTOList.Remove(tempDTO);
                    }
                }
               
                if (inSqlTransaction == null)
                {
                    sqlTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                if (inSqlTransaction == null)
                {
                    sqlTransaction.Rollback();
                }
                throw;
            }
            finally
            {
                if (inSqlTransaction == null)
                {
                    conn.Close();
                }
            }
        }

        private void RescheduleAttractionLines(List<AttractionBookingDTO> attractionBookingDTOList, DayAttractionScheduleDTO sourceDASDTO, DayAttractionScheduleDTO targetDASDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sourceDASDTO, targetDASDTO);

            try
            {
                foreach (AttractionBookingDTO attractionBookingDTO in attractionBookingDTOList)
                {
                    if (attractionBookingDTO.BookingId == -1 || attractionBookingDTO.DayAttractionScheduleId == -1 ||
                        attractionBookingDTO.TrxId == -1 || attractionBookingDTO.LineId == -1 || attractionBookingDTO.AttractionProductId == -1)
                    {
                        String error = "Invalid data for attraction with Booking Id:" + attractionBookingDTO.BookingId + ":DAS:" + attractionBookingDTO.DayAttractionScheduleId +
                            ":trx:" + attractionBookingDTO.TrxId + ":Line:" + attractionBookingDTO.LineId.ToString();
                        log.Debug(error);
                        throw new Exception(error);
                    }
                }

                // Check if the day attraction booking exists, if not create it
                DayAttractionScheduleBL dayAttractionScheduleBL = new DayAttractionScheduleBL(executionContext, targetDASDTO);
                try
                {
                    List<ValidationError> validationErrorList = dayAttractionScheduleBL.Validate(sqlTransaction);
                    if (validationErrorList.Count > 0)
                    {
                        throw new ValidationException("Validation Failed", validationErrorList);
                    }
                }
                catch (RowNotInTableException ex)
                {
                    targetDASDTO.Blocked = true;
                    targetDASDTO.ExpiryTime = DateTime.MinValue;
                    targetDASDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);
                    if (String.IsNullOrEmpty(targetDASDTO.Source))
                        targetDASDTO.Source = AttractionBookingDTO.SourceEnumToString(attractionBookingDTOList[0].Source);

                    dayAttractionScheduleBL.Save(sqlTransaction);
                    targetDASDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
                }
                catch (EntityExpiredException ex)
                {
                    // if an epired day attraction schedule exists, then create a new one as it is leftover of an attempted booking
                    if (attractionBookingDTOList[0].BookingId == -1)
                    {
                        targetDASDTO.Blocked = true;
                        targetDASDTO.ExpiryTime = DateTime.MinValue;
                        targetDASDTO.ScheduleStatus = DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN);
                        if (String.IsNullOrEmpty(targetDASDTO.Source))
                            targetDASDTO.Source = AttractionBookingDTO.SourceEnumToString(attractionBookingDTOList[0].Source);

                        dayAttractionScheduleBL.Save(sqlTransaction);
                        targetDASDTO = dayAttractionScheduleBL.GetDayAttractionScheduleDTO;
                    }
                }
                catch (Exception ex)
                {
                    log.Debug("validate day attraction schedule failed." + ex.Message);
                    throw;
                }

                // get the transaction lines to get the card number
                List<TransactionLineDTO> temptransactionLineDTOList = GetTransactionLinesDTOList(attractionBookingDTOList);

                // update attraction bookings
                foreach (AttractionBookingDTO attractionBookingDTO in attractionBookingDTOList)
                {
                    List<TransactionLineDTO> tempList = temptransactionLineDTOList.Where(x=> x.TransactionId == attractionBookingDTO.TrxId && x.LineId == attractionBookingDTO.LineId).ToList();
                    int cardId = -1;
                    if (tempList.Any())
                        cardId = tempList[0].CardId;

                    attractionBookingDTO.DayAttractionScheduleId = targetDASDTO.DayAttractionScheduleId;
                    attractionBookingDTO.DayAttractionScheduleDTO = targetDASDTO;
                    AttractionBooking atbBL = new AttractionBooking(executionContext, attractionBookingDTO);
                    atbBL.Save(cardId, sqlTransaction);
                }

                // Move card entitlements
                double secondsToAdd = (targetDASDTO.ScheduleDateTime - sourceDASDTO.ScheduleDateTime).TotalSeconds;
                RescheduleCardEntitlements(attractionBookingDTOList, secondsToAdd, targetDASDTO.ScheduleToDateTime, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Debug(ex);
                throw;
            }
        }

        private List<TransactionLineDTO> GetTransactionLinesDTOList(List<AttractionBookingDTO> attractionBookingDTOList)
        {
            log.LogMethodEntry(attractionBookingDTOList);

            List<TransactionLineDTO> temptransactionLineDTOList = new List<TransactionLineDTO>();
            IEnumerable<int> trxIdList = attractionBookingDTOList.Select(x => x.TrxId).Distinct();
            List<int> trxIdListNew = new List<int>();
            foreach (int trxId in trxIdList)
            {
                if (transactionLineDTOList.Where(x => x.TransactionId == trxId).ToList().Any())
                {
                    temptransactionLineDTOList.AddRange(transactionLineDTOList.Where(x => x.TransactionId == trxId).ToList());
                }
                else
                {
                    trxIdListNew.Add(trxId);
                }
            }
            if (trxIdListNew.Any())
            {
                String trxList = String.Join(",", trxIdListNew);
                TransactionLineListBL transactionLineListBL = new TransactionLineListBL(executionContext);
                List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionLineDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionLineDTO.SearchByParameters, string>(TransactionLineDTO.SearchByParameters.TRANSACTION_ID_LIST, trxList));
                temptransactionLineDTOList.AddRange(transactionLineListBL.GetTransactionLineDTOList(searchParameters));
                transactionLineDTOList.AddRange(temptransactionLineDTOList);
            }

            if(!temptransactionLineDTOList.Any())
            {
                throw new Exception("Transaction or Transaction Line not found.");
            }
            log.LogMethodExit(temptransactionLineDTOList);
            return temptransactionLineDTOList;
        }

        private void RescheduleCardEntitlements(List<AttractionBookingDTO> attractionBookingDTOList, double secondsToAdd, DateTime scheduleToDateTime, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attractionBookingDTOList, secondsToAdd, sqlTransaction);

            log.Debug("secondsToAdd:" + secondsToAdd);

            List<TransactionLineDTO> temptransactionLineDTOList = GetTransactionLinesDTOList(attractionBookingDTOList);
            if (temptransactionLineDTOList != null && temptransactionLineDTOList.Any())
            {
                foreach (AttractionBookingDTO attractionBookingDTO in attractionBookingDTOList)
                {
                    List<TransactionLineDTO> tempTrxLineDTOList = temptransactionLineDTOList.Where(x => x.TransactionId == attractionBookingDTO.TrxId && x.LineId == attractionBookingDTO.LineId).ToList();
                    if (tempTrxLineDTOList != null && tempTrxLineDTOList.Any())
                    {
                        int productId = attractionBookingDTO.AttractionProductId;
                        ProductsDTO productsDTO = null;
                        if (productsDictionary.ContainsKey(productId))
                        {
                            productsDTO = productsDictionary[productId];
                        }
                        else
                        {
                            Products productsBL = new Products(executionContext, productId);
                            productsDTO = productsBL.GetProductsDTO;
                            productsDictionary.Add(productId, productsDTO);
                        }

                        if(productsDTO.CardSale.Equals("Y"))
                        {
                            String tagNumber = tempTrxLineDTOList[0].CardNumber;
                            if(string.IsNullOrEmpty(tagNumber) || tempTrxLineDTOList[0].CardId == -1)
                            {
                                String errorMessage = MessageContainerList.GetMessage(executionContext, 504);
                                throw new Exception(errorMessage);
                            }

                            AccountBL accountBL = new Customer.Accounts.AccountBL(executionContext, tempTrxLineDTOList[0].CardId, sqlTransaction: sqlTransaction);
                            if (accountBL.AccountDTO.AccountGameDTOList != null)
                            {
                                List<AccountGameDTO> accountGameDTOList = accountBL.AccountDTO.AccountGameDTOList.Where(x => x.TransactionId == attractionBookingDTO.TrxId
                                                                                            && x.TransactionLineId == attractionBookingDTO.LineId).ToList();
                                if (accountGameDTOList != null && accountGameDTOList.Any())
                                {
                                    foreach (AccountGameDTO gameDTO in accountGameDTOList)
                                    {
                                        gameDTO.ExpiryDate = gameDTO.ExpiryDate == null ? gameDTO.ExpiryDate : Convert.ToDateTime(gameDTO.ExpiryDate.ToString()).AddSeconds(secondsToAdd);
                                        gameDTO.FromDate = gameDTO.FromDate == null ? gameDTO.FromDate : Convert.ToDateTime(gameDTO.FromDate.ToString()).AddSeconds(secondsToAdd);
                                    }
                                }
                            }

                            if (accountBL.AccountDTO.AccountCreditPlusDTOList != null)
                            {
                                List<AccountCreditPlusDTO> accountCreditPlusDTOList = accountBL.AccountDTO.AccountCreditPlusDTOList.Where(x => x.TransactionId == attractionBookingDTO.TrxId
                                                                                            && x.TransactionLineId == attractionBookingDTO.LineId).ToList();
                                if (accountCreditPlusDTOList != null && accountCreditPlusDTOList.Any())
                                {
                                    foreach (AccountCreditPlusDTO cpDTO in accountCreditPlusDTOList)
                                    {
                                        cpDTO.PeriodFrom = cpDTO.PeriodFrom == null ? cpDTO.PeriodFrom : Convert.ToDateTime(cpDTO.PeriodFrom.ToString()).AddSeconds(secondsToAdd);
                                        cpDTO.PeriodTo = cpDTO.PeriodTo == null ? cpDTO.PeriodTo : Convert.ToDateTime(cpDTO.PeriodTo.ToString()).AddSeconds(secondsToAdd);
                                    }
                                }
                            }

                            if (accountBL.AccountDTO.AccountDiscountDTOList != null)
                            {
                                List<AccountDiscountDTO> accountDiscountDTOList = accountBL.AccountDTO.AccountDiscountDTOList.Where(x => x.TransactionId == attractionBookingDTO.TrxId
                                                                                            && x.LineId == attractionBookingDTO.LineId).ToList();
                                if (accountDiscountDTOList != null && accountDiscountDTOList.Any())
                                {
                                    foreach (AccountDiscountDTO discountDTO in accountDiscountDTOList)
                                    {
                                        discountDTO.ExpiryDate = discountDTO.ExpiryDate == null ? discountDTO.ExpiryDate : Convert.ToDateTime(discountDTO.ExpiryDate.ToString()).AddSeconds(secondsToAdd);
                                    }
                                }
                            }

                            if(accountBL.AccountDTO.ExpiryDate != null && accountBL.AccountDTO.ExpiryDate < scheduleToDateTime)
                            {
                                accountBL.AccountDTO.ExpiryDate = Convert.ToDateTime(accountBL.AccountDTO.ExpiryDate.ToString()).AddSeconds(secondsToAdd);
                            }

                            accountBL.Save(sqlTransaction);
                        }
                    }
                    else
                    {
                        throw new Exception(MessageContainerList.GetMessage(executionContext, 2815, attractionBookingDTO.BookingId , attractionBookingDTO.TrxId , attractionBookingDTO.LineId));
                    }                
                }
            }
            else
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 2815));
            }
            log.LogMethodExit();
        }
    }
}
