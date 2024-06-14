/********************************************************************************************
 * Project Name - Transaction Services - AttractionBookingSchedulesContainer
 * Description  - Container of attraction schedules
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.130.04   17-Feb-2022   Nitin Pai               Creating Attraction Schedule Container so that schedules can be refreshed without restart of API or POS
 ***************************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction.TransactionFunctions
{
    class AttractionBookingSchedulesContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Cache<DateTime, List<ScheduleDetailsDTO>> attractionSchedulesContainerCache = new Cache<DateTime, List<ScheduleDetailsDTO>>();
        private readonly DateTime? attractionSchedulesLastUpdateTime;
        private readonly int siteId;

        public AttractionBookingSchedulesContainer(int siteId, DateTime scheduleDateTime) :
            this(siteId, scheduleDateTime, GetAttractionSchedulesLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public AttractionBookingSchedulesContainer(int siteId, DateTime scheduleDateTime, DateTime? attractionSchedulesLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.attractionSchedulesLastUpdateTime = attractionSchedulesLastUpdateTime;
            // Only the date part is the key
            scheduleDateTime = GetBusinessDate(siteId, scheduleDateTime);

            // Build the schedules here
            attractionSchedulesContainerCache.GetOrAdd(scheduleDateTime, (k) => { return CreateScheduleDetailsDTOList(scheduleDateTime); });
            log.LogMethodExit();
        }

        private static DateTime GetBusinessDate(int siteId, DateTime scheduleDate)
        {
            log.LogMethodEntry(scheduleDate);
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(siteId, "BUSINESS_DAY_START_TIME");

            if (scheduleDate.Hour < businessEndHour)
                scheduleDate = scheduleDate.AddDays(-1);

            DateTime businessDate = scheduleDate.Date;
            businessDate = businessDate.AddHours(businessEndHour);

            log.LogMethodExit(businessDate);
            return businessDate;
        }

        private static DateTime? GetAttractionSchedulesLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList();
                DateTime? updateTime = masterScheduleList.GetAttractionSchedulesLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the user module last update time.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public AttractionBookingSchedulesContainer Refresh()
        {
            log.LogMethodEntry();

            var dateRangeList = attractionSchedulesContainerCache.Keys;
            DateTime serverNow = ServerDateTime.Now;
            foreach (var dateTimeRange in dateRangeList)
            {
                if (dateTimeRange < serverNow)
                {
                    List<ScheduleDetailsDTO> value;
                    if (attractionSchedulesContainerCache.TryRemove(dateTimeRange, out value))
                    {
                        log.Debug("Removing attractionSchedulesContainerCache of date range" + dateTimeRange);
                    }
                    else
                    {
                        log.Debug("Unable to remove attractionSchedulesContainerCache of date range" + dateTimeRange);
                    }
                }
            }

            DateTime? updateTime = null;

            try
            {
                MasterScheduleList masterScheduleList = new MasterScheduleList();
                updateTime = masterScheduleList.GetAttractionSchedulesLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the user module last update time.", ex);
            }

            if (attractionSchedulesLastUpdateTime.HasValue
                && attractionSchedulesLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in attraction schedules since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }

            AttractionBookingSchedulesContainer result = new AttractionBookingSchedulesContainer(siteId, ServerDateTime.Now);
            log.LogMethodExit(result);
            return result;
        }

        public List<ScheduleDetailsDTO> GetAttractionBookingSchedulesContainer(DateTime scheduleDateTime)
        {
            log.LogMethodEntry(scheduleDateTime);
            List<ScheduleDetailsDTO> result = new List<ScheduleDetailsDTO>();
            List<ScheduleDetailsDTO> tempList = null;
            // Only the date part is the key
            scheduleDateTime = GetBusinessDate(siteId, scheduleDateTime);

            // Build the schedules here
            tempList = attractionSchedulesContainerCache.GetOrAdd(scheduleDateTime, (k) => { return CreateScheduleDetailsDTOList(scheduleDateTime); }); 

            foreach (ScheduleDetailsDTO existingSchedule in tempList)
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
                facilityMapDTO.ProductsAllowedInFacilityDTOList = existingSchedule.FacilityMapDTO.ProductsAllowedInFacilityDTOList;

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
                result.Add(scheduleDetailsDTO);
            }

            log.LogMethodExit(result);
            return result;
        }

        private List<ScheduleDetailsDTO>  CreateScheduleDetailsDTOList(DateTime scheduleDateTime)
        {
            log.LogMethodEntry(scheduleDateTime);
            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();


            decimal startTime = 0;
            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(siteId, "BUSINESS_DAY_START_TIME");
            if (businessEndHour > 0)
                startTime = Convert.ToDecimal(businessEndHour);
            decimal endTime = 24;

            DateTime businessDate = GetBusinessDate(siteId, scheduleDateTime);

            // Nitin To Do: This needs to be checked
            ExecutionContext executionContext = new ExecutionContext("", siteId, -1, -1, true, -1);

            MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
            List<MasterScheduleBL> masterScheduleBLList = masterScheduleList.GetAllMasterScheduleBLList();
            scheduleDetailsDTOList = masterScheduleList.GetEligibleSchedules(masterScheduleBLList, businessDate, startTime, endTime, -1, -1, ProductTypeValues.ATTRACTION);
            if (businessEndHour > 0)
                scheduleDetailsDTOList.AddRange(masterScheduleList.GetEligibleSchedules(masterScheduleBLList, businessDate.AddDays(1), 0, businessEndHour, -1, -1, ProductTypeValues.ATTRACTION));

            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }
    }
}
