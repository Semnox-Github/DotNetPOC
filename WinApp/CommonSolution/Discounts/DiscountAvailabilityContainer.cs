/********************************************************************************************
 * Project Name - Discount
 * Description  - DiscountAvailabilityContainer class to get the data
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      05-May-2021      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// Class holds the available discount values.
    /// </summary>
    public class DiscountAvailabilityContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DateTimeRange dateTimeRange;
        private readonly Dictionary<int, DiscountAvailabilityContainerDTO> discountIdDiscountAvailabilityDTODictionary = new Dictionary<int, DiscountAvailabilityContainerDTO>();
        private DiscountAvailabilityContainerDTOCollection discountAvailabilityDTOCollection;
        private TimeSpan increment;
        private int siteId;

        public DiscountAvailabilityContainer(int siteId, List<DiscountsDTO> discountsDTOList, DateTimeRange dateTimeRange, TimeSpan increment)
        {
            log.LogMethodEntry(discountsDTOList,dateTimeRange,increment);
            this.dateTimeRange = dateTimeRange;
            this.increment = increment;
            this.siteId = siteId;
            List<DiscountAvailabilityContainerDTO> discountAvailabilityDTOList = new List<DiscountAvailabilityContainerDTO>();
            foreach (var discountsDTO in discountsDTOList)
            {
                if(discountIdDiscountAvailabilityDTODictionary.ContainsKey(discountsDTO.DiscountId))
                {
                    continue;
                }
                
                DiscountAvailabilityContainerDTO discountAvailabilityDTO = GetDiscountAvailabilityDTO(discountsDTO);
                discountAvailabilityDTOList.Add(discountAvailabilityDTO);
                discountIdDiscountAvailabilityDTODictionary.Add(discountsDTO.DiscountId, discountAvailabilityDTO);
            }
            discountAvailabilityDTOCollection = new DiscountAvailabilityContainerDTOCollection(discountAvailabilityDTOList);
            log.Info("Number of items loaded by DiscountAvailabilityContainer for site " + siteId + ":" + discountsDTOList.Count);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the discount available
        /// </summary>
        /// <param name="discountsDTO">discountsDTO</param>
        /// <returns></returns>
        private DiscountAvailabilityContainerDTO GetDiscountAvailabilityDTO(DiscountsDTO discountsDTO)
        {
            log.LogMethodEntry(discountsDTO);
            if (discountsDTO.ScheduleId == -1 ||
                    discountsDTO.ScheduleCalendarDTO == null ||
                    discountsDTO.ScheduleCalendarDTO.IsActive == false)
            {
                List<DiscountAvailabilityDetailContainerDTO> defaultDiscountAvailabilityDetailDTOList = new List<DiscountAvailabilityDetailContainerDTO>();
                DiscountAvailabilityDetailContainerDTO defaultDiscountAvailabilityDetailDTO = new DiscountAvailabilityDetailContainerDTO(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime, true);
                defaultDiscountAvailabilityDetailDTOList.Add(defaultDiscountAvailabilityDetailDTO);
                DiscountAvailabilityContainerDTO defaultDiscountAvailabilityDTO = new DiscountAvailabilityContainerDTO(discountsDTO.DiscountId, defaultDiscountAvailabilityDetailDTOList);
                return defaultDiscountAvailabilityDTO;
            }
            ScheduleCalendarCalculator scheduleCalendarCalculator = new ScheduleCalendarCalculator(discountsDTO.ScheduleCalendarDTO);
            List<DiscountAvailabilityDetailContainerDTO> discountAvailabilityDetailDTOList = new List<DiscountAvailabilityDetailContainerDTO>();
            List<DateTime> dateTimesInRange = dateTimeRange.GetDateTimesInRange(increment).ToList();
            List<bool> availabilityInRange = new List<bool>();
            foreach (var value in dateTimesInRange)
            {
                availabilityInRange.Add(scheduleCalendarCalculator.IsRelevant(value));
            }
            DateTime startDateTime = dateTimesInRange[0];
            DateTime endDateTime;
            bool available = availabilityInRange[0];
            for (int i = 1; i < dateTimesInRange.Count; i++)
            {
                if (availabilityInRange[i] != available)
                {
                    DiscountAvailabilityDetailContainerDTO discountAvailabilityDetailDTO = new DiscountAvailabilityDetailContainerDTO(startDateTime, dateTimesInRange[i], available);
                    discountAvailabilityDetailDTOList.Add(discountAvailabilityDetailDTO);
                    startDateTime = dateTimesInRange[i];
                    available = availabilityInRange[i];
                }
            }
            endDateTime = dateTimesInRange.Last();
            DiscountAvailabilityDetailContainerDTO lastDiscountAvailabilityDetailDTO = new DiscountAvailabilityDetailContainerDTO(startDateTime, endDateTime, available);
            discountAvailabilityDetailDTOList.Add(lastDiscountAvailabilityDetailDTO);
            DiscountAvailabilityContainerDTO result = new DiscountAvailabilityContainerDTO(discountsDTO.DiscountId, discountAvailabilityDetailDTOList);
            log.LogMethodExit(result);
            return result;
        }

        public DiscountAvailabilityContainerDTOCollection GetDiscountAvailabilityDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(discountAvailabilityDTOCollection);
            return discountAvailabilityDTOCollection;
        }

        public bool IsDiscountAvailable(int discountId, DateTime dateTime)
        {
            log.LogMethodEntry(discountId, dateTime);
            if(discountIdDiscountAvailabilityDTODictionary.ContainsKey(discountId) == false)
            {
                string message = MessageContainerList.GetMessage(siteId, -1, 2196, "Discount", discountId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            DiscountAvailabilityContainerDTO discountAvailabilityDTO = discountIdDiscountAvailabilityDTODictionary[discountId];
            foreach (var discountAvailabilityDetailDTO in discountAvailabilityDTO.DiscountAvailabilityDetailDTOList)
            {
                if(dateTime >= discountAvailabilityDetailDTO.StartDateTime && dateTime < discountAvailabilityDetailDTO.EndDateTime)
                {
                    log.LogMethodExit(discountAvailabilityDetailDTO.Available);
                    return discountAvailabilityDetailDTO.Available;
                }
            }
            log.LogMethodExit(false, "Unable to find discountAvailabilityDetailDTO");
            return false;
        }
    }
}
