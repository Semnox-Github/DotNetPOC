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
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction.TransactionFunctions
{
    class AttractionBookingSchedulesContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, AttractionBookingSchedulesContainer> attractionBookingSchedulesContainerCache = new Cache<int, AttractionBookingSchedulesContainer>();
        private static Timer refreshTimer;

        static AttractionBookingSchedulesContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = attractionBookingSchedulesContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                AttractionBookingSchedulesContainer attractionBookingSchedulesContainer;
                if (attractionBookingSchedulesContainerCache.TryGetValue(uniqueKey, out attractionBookingSchedulesContainer))
                {
                    attractionBookingSchedulesContainerCache[uniqueKey] = attractionBookingSchedulesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static AttractionBookingSchedulesContainer GetAttractionBookingSchedulesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            AttractionBookingSchedulesContainer result = attractionBookingSchedulesContainerCache.GetOrAdd(siteId, (k) => new AttractionBookingSchedulesContainer(siteId, ServerDateTime.Now));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get User Container DTO by site id and user id
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="scheduleDateTime"></param>
        /// <returns></returns>
        public static List<ScheduleDetailsDTO> GetAttractionBookingSchedules(int siteId, DateTime scheduleDateTime)
        {
            log.LogMethodEntry(siteId, scheduleDateTime);
            AttractionBookingSchedulesContainer attractionBookingSchedulesContainer = GetAttractionBookingSchedulesContainer(siteId);
            var result = attractionBookingSchedulesContainer.GetAttractionBookingSchedulesContainer(scheduleDateTime);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            AttractionBookingSchedulesContainer attractionBookingSchedulesContainer = GetAttractionBookingSchedulesContainer(siteId);
            attractionBookingSchedulesContainerCache[siteId] = attractionBookingSchedulesContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
