/********************************************************************************************
 * Project Name - Transaction Services - AttractionBookingScheduleBL
 * Description  - BL to Fetch schedules for attractions and reservation bookings
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100      24-Sep-2020   Nitin Pai                Created
 *2.110      20-Jan-2021   Nitin Pai                Performance improvements changes. Returning View DTO    
 *2.120.0    04-Mar-2021   Sathyavathi              Enabling option nto decide ''Multiple-Booking at Facility level 
 *2.110      31-Mar-2021   Nitin Pai                GRS Prod Fix: Schedule was 
 *2.130      07-Jun-2021   Nitin Pai                Funstasia Fix: Master schedules did not consider product start date
 *2.130.1    01-Dec-2021   Nitin Pai                Fetch product price from container to avoid applying discount multiple times.
 *2.130.04   17-Feb-2022   Nitin Pai                Creating Attraction Schedule Container so that schedules can be refreshed without restart of API or POS
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction.TransactionFunctions
{
    /// <summary>
    /// BL to get eligible achedules for attractions and bookings
    /// </summary>
    public class AttractionBookingSchedulesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private static Dictionary<DateTime, List<MasterScheduleBL>> masterScheduleBLListMap = new Dictionary<DateTime, List<MasterScheduleBL>>();
        private static Dictionary<int, List<int>> relatedFacilityMaps = new Dictionary<int, List<int>>();
        private static Dictionary<DateTime, Dictionary<int, List<ScheduleDetailsDTO>>> schedulesForFacilityMaps = new Dictionary<DateTime, Dictionary<int, List<ScheduleDetailsDTO>>>();
        private static Dictionary<string, string> statusCellColorMap = new Dictionary<string, string>();
        List<DayAttractionScheduleDTO> dayAttractionSchedulesList = null;

        private AttractionBookingSchedulesBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public AttractionBookingSchedulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="selectedDate"></param>
        /// <param name="masterScheduleBLList"></param>
        public AttractionBookingSchedulesBL(ExecutionContext executionContext, DateTime selectedDate, List<MasterScheduleBL> masterScheduleBLList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            if (!AttractionBookingSchedulesBL.masterScheduleBLListMap.ContainsKey(selectedDate.Date))
            {
                AttractionBookingSchedulesBL.masterScheduleBLListMap.Add(selectedDate.Date, masterScheduleBLList);
            }
            log.LogMethodExit();
        }



        /// <summary>
        /// Gets the AttractionSchedule Collection based on isActive,scheduleDate,productsList,facilityMapId
        /// </summary>       
        /// <param name="scheduleDate"></param>
        /// <param name="productsList"></param>
        /// <param name="facilityMapId"></param>
        /// <param name="fixedSchedule"></param>
        /// <param name="scheduleFromTime"></param>
        /// <param name="scheduleToTime"></param>
        /// <param name="includePast"></param>
        /// <param name="filterProducts"></param>
        /// <param name="buildDetails"></param>
        /// <param name="removeUnavailable"></param>
        /// <returns>List ScheduleDetailsDTO </returns>
        public List<ScheduleDetailsDTO> GetAttractionBookingSchedules(DateTime scheduleDate, string productsList = null, int facilityMapId = -1,
            bool? fixedSchedule = null, decimal? scheduleFromTime = null, decimal? scheduleToTime = null, bool includePast = false, bool filterProducts = false, bool buildDetails = true,
            bool? removeUnavailable = false)
        {
            log.LogMethodEntry(scheduleDate, productsList, facilityMapId, fixedSchedule, scheduleFromTime, scheduleToTime);

            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();

            decimal startTime = 0;
            if (scheduleFromTime != null)
                startTime = Convert.ToDecimal(scheduleFromTime.ToString());

            decimal endTime = 24;
            if (scheduleToTime != null)
                endTime = Convert.ToDecimal(scheduleToTime.ToString());

            char[] arrayOfCharacters = new Char[] { ',' };
            List<int> productIdList = new List<int>();
            if (!string.IsNullOrEmpty(productsList))
                productIdList = productsList.Split(arrayOfCharacters, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            log.LogVariableState("Getting details for products ", string.Join(",", productIdList) + " Input dates " + scheduleDate + ":" + scheduleDate.Hour);

            int businessEndHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME");
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            if (currentTime.Hour < businessEndHour)
                currentTime = currentTime.AddDays(-1);
            if (scheduleDate.Date == currentTime.Date && !includePast)
            {
                startTime = currentTime.Hour;
            }
            else
            {
                startTime = businessEndHour;
            }

            DateTime businessDate = scheduleDate.Date;
            // avoid this if the input has come as 00:00:00
            if (scheduleDate.Date == currentTime.Date && scheduleDate.Hour < businessEndHour && (scheduleDate.Hour != 0 && scheduleDate.Minute != 0 && scheduleDate.Second != 0))
            {
                log.Debug("Rolling back as the current time is less than business day start time");
                businessDate = businessDate.AddDays(-1).Date;
            }
            else
            {
                log.Debug("Skiping the rollover part");
            }
            businessDate = businessDate.AddHours(businessEndHour);
            log.LogVariableState("Final calculated dates ", businessDate);

            string gracePeriod = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTRACTION_BOOKING_GRACE_PERIOD");
            int gracePeriodMin = 0;
            if (!int.TryParse(gracePeriod, out gracePeriodMin))
                gracePeriodMin = 0;

            List<ScheduleDetailsDTO> scheduleDetailsDTOList = new List<ScheduleDetailsDTO>();
            List<int> facilityMapIdList = new List<int>();

            if (facilityMapId == -1 && !String.IsNullOrEmpty(productsList))
            {
                FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> facilitySearcParm = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
                facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                facilitySearcParm.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_IDS_IN, productsList));
                List<FacilityMapDTO> facilityMapDTOList = facilityMapListBL.GetFacilityMapDTOList(facilitySearcParm);
                if (facilityMapDTOList != null && facilityMapDTOList.Any())
                {
                    foreach (FacilityMapDTO tempFMDTO in facilityMapDTOList)
                        facilityMapIdList.Add(tempFMDTO.FacilityMapId);
                }
                else
                {
                    facilityMapIdList.Add(-1);
                }
            }
            else
            {
                facilityMapIdList.Add(facilityMapId);
            }

            List<int> facilityIdMap = new List<int>();
            DateTime scheduleStartTime = DateTime.MaxValue, scheduleEndTime = DateTime.MinValue;
            List<ScheduleDetailsDTO> tempScheduleDetailsList1 = new List<ScheduleDetailsDTO>();
            List<ScheduleDetailsDTO> containerScheduleDetailsList1 = AttractionBookingSchedulesContainerList.GetAttractionBookingSchedules(executionContext.GetSiteId(), businessDate);

            if (facilityMapIdList.Contains(-1))
            {
                tempScheduleDetailsList1.AddRange(containerScheduleDetailsList1.ToList());
            }
            else
            {
                foreach (int tempFacilityMapId in facilityMapIdList)
                {
                    tempScheduleDetailsList1.AddRange(containerScheduleDetailsList1.Where(x => x.FacilityMapId == tempFacilityMapId).ToList());
                }
            }

            if (tempScheduleDetailsList1 != null && tempScheduleDetailsList1.Any())
            {
                log.Info("Schedule Count before filtering :" + tempScheduleDetailsList1.Count);
                List<Tuple<int, DateTime, int>> offsetData = new List<Tuple<int, DateTime, int>>();
                int offsetTimeSecs = GetOffSet(offsetData, timeZoneUtil, executionContext.GetSiteId(), businessDate);
                log.Debug("Offset time " + offsetTimeSecs);

                List<ScheduleDetailsDTO> tempScheduleDetailsList = new List<ScheduleDetailsDTO>();
                foreach (ScheduleDetailsDTO scheduleDetailsDTO in tempScheduleDetailsList1)
                {
                    log.Debug("Schedule details" + scheduleDetailsDTO.ScheduleId + ":" + scheduleDetailsDTO.FacilityMapId + ":" + scheduleDetailsDTO.ScheduleFromDate);
                    log.Debug(scheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList == null ? string.Join(",", scheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList) : "no products");

                    if (tempScheduleDetailsList.FirstOrDefault(x => x.ScheduleId == scheduleDetailsDTO.ScheduleId && x.FacilityMapId == scheduleDetailsDTO.FacilityMapId) != null)
                        continue;

                    if (!String.IsNullOrEmpty(productsList) &&
                        (scheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList == null
                        || !scheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList.Any(x => productIdList.Any(y => x.ProductsId.Equals(y)))))
                        continue;

                    if (scheduleDetailsDTO.ScheduleFromDate < scheduleStartTime)
                        scheduleStartTime = scheduleDetailsDTO.ScheduleFromDate;

                    if (scheduleDetailsDTO.ScheduleToDate > scheduleEndTime)
                        scheduleEndTime = scheduleDetailsDTO.ScheduleToDate;

                    if (!facilityIdMap.Contains(scheduleDetailsDTO.FacilityMapId))
                        facilityIdMap.Add(scheduleDetailsDTO.FacilityMapId);

                    scheduleDetailsDTO.ProductId = -1;
                    scheduleDetailsDTO.DayAttractionScheduleId = -1;
                    scheduleDetailsDTO.BookedUnits = null;
                    tempScheduleDetailsList.Add(scheduleDetailsDTO);
                }

                if (tempScheduleDetailsList == null || !tempScheduleDetailsList.Any())
                {
                    log.LogMethodExit(scheduleDetailsDTOList);
                    return scheduleDetailsDTOList;
                }

                dayAttractionSchedulesList = GetDayAttractionSchedules(scheduleStartTime, scheduleEndTime);
                log.Info("Schedule Count after filtering :" + tempScheduleDetailsList.Count);

                Utilities parafaitUtility = new Semnox.Core.Utilities.Utilities();
                parafaitUtility.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
                parafaitUtility.ExecutionContext.SetSiteId(executionContext.GetSiteId());
                parafaitUtility.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
                parafaitUtility.ParafaitEnv.SiteId = executionContext.GetSiteId();

                List<AttractionBookingDTO> bookedUnitMap = new List<AttractionBookingDTO>();
                List<int> promoProducts = new List<int>();
                if (productIdList.Any())
                {
                    foreach (int productId in productIdList)
                    {
                        ProductsContainerDTO products = ProductsContainerList.GetProductsContainerDTO(executionContext.GetSiteId(), productId);
                        if (products != null && products.ProductId > -1)
                        {
                            Double price = Convert.ToDouble(products.Price.ToString());
                            bool promotionExits = Promotions.CheckProductPromotionExitsForDay(null, products.ProductId, products.CategoryId, parafaitUtility, scheduleStartTime.Date);

                            if (promotionExits)
                            {
                                promoProducts.Add(productId);
                            }
                            else
                            {
                                if (scheduleStartTime.Date != scheduleEndTime.Date)
                                {
                                    promotionExits = Promotions.CheckProductPromotionExitsForDay(null, products.ProductId, products.CategoryId, parafaitUtility, scheduleEndTime.Date);

                                    if (promotionExits)
                                    {
                                        promoProducts.Add(productId);
                                    }
                                }
                            }

                            log.LogVariableState("Promo exists for the day", promotionExits);
                            log.LogVariableState("Promo products for the day", string.Join(",", promoProducts));

                            AttractionBookingList attractionBooking = new AttractionBookingList(executionContext);
                            log.Debug("Get total booked units for " + String.Join(",", facilityIdMap) + ":" + scheduleStartTime.AddSeconds(offsetTimeSecs) + ":" + scheduleEndTime.AddSeconds(offsetTimeSecs) + ":" + productId);
                            List<AttractionBookingDTO> tempMap = attractionBooking.GetTotalBookedUnitsForAttractionBySchedule(facilityIdMap, scheduleStartTime.AddSeconds(offsetTimeSecs), scheduleEndTime.AddSeconds(offsetTimeSecs), productId, -1, -1 * offsetTimeSecs);
                            foreach (AttractionBookingDTO tempATB in tempMap)
                            {
                                log.Debug("Booked map " + String.Join(",", tempMap));

                                if (bookedUnitMap.FirstOrDefault(x => x.AttractionScheduleId == tempATB.AttractionScheduleId && x.FacilityMapId == tempATB.FacilityMapId) != null)
                                    continue;

                                bookedUnitMap.Add(tempATB);
                            }
                        }
                    }
                }
                else
                {
                    AttractionBookingList attractionBooking = new AttractionBookingList(executionContext);
                    log.Debug("Get total booked units for " + String.Join(",", facilityIdMap) + ":" + scheduleStartTime.AddSeconds(offsetTimeSecs) + ":" + scheduleEndTime.AddSeconds(offsetTimeSecs) + ":" + -1);
                    List<AttractionBookingDTO> tempMap = attractionBooking.GetTotalBookedUnitsForAttractionBySchedule(facilityIdMap, scheduleStartTime.AddSeconds(offsetTimeSecs), scheduleEndTime.AddSeconds(offsetTimeSecs), -1, -1, -1 * offsetTimeSecs);
                    foreach (AttractionBookingDTO tempATBDTO in tempMap)
                    {
                        log.Debug("Booked map " + String.Join(",", tempMap));
                        if (bookedUnitMap.FirstOrDefault(x => x.AttractionScheduleId == tempATBDTO.AttractionScheduleId && x.FacilityMapId == tempATBDTO.FacilityMapId) != null)
                            continue;

                        bookedUnitMap.Add(tempATBDTO);
                    }
                }
                DateTime serverTime = ServerDateTime.Now;
                foreach (ScheduleDetailsDTO tempScheduleDetailsDTO in tempScheduleDetailsList)
                {
                    if (tempScheduleDetailsDTO.FacilityMapDTO != null &&
                        tempScheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList != null &&
                        tempScheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList.Any())
                    {
                        offsetTimeSecs = GetOffSet(offsetData, timeZoneUtil, tempScheduleDetailsDTO.SiteId, tempScheduleDetailsDTO.ScheduleFromDate.Date);
                        log.Info("Server Time plus gracetime" + serverTime.AddMinutes(gracePeriodMin) + " | gracetime (min) : " + gracePeriodMin);
                        log.Info("schedule from date with offset" + tempScheduleDetailsDTO.ScheduleFromDate.AddSeconds(offsetTimeSecs));

                        if (tempScheduleDetailsDTO.ScheduleFromDate.AddSeconds(offsetTimeSecs) < serverTime.AddMinutes(gracePeriodMin * (-1)) && !includePast)
                        {
                            continue;
                        }

                        List<int> facilityMapList = new List<int>();
                        if (relatedFacilityMaps.ContainsKey(tempScheduleDetailsDTO.FacilityMapId))
                        {
                            facilityMapList = relatedFacilityMaps[tempScheduleDetailsDTO.FacilityMapId];
                        }
                        else
                        {
                            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext);
                            facilityMapList = facilityMapListBL.GetFacilityMapsForSameFacility(tempScheduleDetailsDTO.FacilityMapId);
                            relatedFacilityMaps.Add(tempScheduleDetailsDTO.FacilityMapId, facilityMapList);
                        }

                        List<DayAttractionScheduleDTO> dsaForSchedule = dayAttractionSchedulesList.Where(x => x.AttractionScheduleId == tempScheduleDetailsDTO.ScheduleId &&
                                                                        relatedFacilityMaps[tempScheduleDetailsDTO.FacilityMapId].Contains(x.FacilityMapId) &&
                                                                        (x.ExpiryTime == DateTime.MinValue || x.ExpiryTime > parafaitUtility.getServerTime())).ToList();
                        if (dsaForSchedule != null && dsaForSchedule.Any())
                        {
                            tempScheduleDetailsDTO.DayAttractionScheduleId = dsaForSchedule[0].DayAttractionScheduleId;
                        }
                        else if (tempScheduleDetailsDTO.DayAttractionScheduleId > -1)
                        {
                            tempScheduleDetailsDTO.DayAttractionScheduleId = -1;
                        }

                        int? totalUnits = 0;
                        totalUnits = tempScheduleDetailsDTO.TotalUnits;

                        int bookedUnits = 0;
                        List<AttractionBookingDTO> existingBooking = bookedUnitMap.Where(x => (
                                                                        (x.ScheduleFromDate <= tempScheduleDetailsDTO.ScheduleFromDate && x.ScheduleToDate > tempScheduleDetailsDTO.ScheduleFromDate) ||
                                                                        (x.ScheduleFromDate < tempScheduleDetailsDTO.ScheduleToDate && x.ScheduleToDate >= tempScheduleDetailsDTO.ScheduleToDate) ||
                                                                        (tempScheduleDetailsDTO.ScheduleFromDate < x.ScheduleFromDate && tempScheduleDetailsDTO.ScheduleToDate > x.ScheduleToDate)) &&
                                                                        (x.FacilityMapId == tempScheduleDetailsDTO.FacilityMapId) &&
                                                                        (x.ExpiryDate == DateTime.MinValue || x.ExpiryDate > currentTime)).ToList();

                        if (existingBooking.Any())
                        {
                            log.Debug("Existing bookings found");
                            bookedUnits = existingBooking.Sum(x => x.BookedUnits);
                        }
                        log.Debug("bookedUnits " + bookedUnits);

                        if (bookedUnits > 0)
                        {
                            tempScheduleDetailsDTO.BookedUnits = bookedUnits;
                        }
                        else
                        {
                            // Nitin - remove comment if tested
                            tempScheduleDetailsDTO.BookedUnits = 0;
                        }

                        if (totalUnits < 0)
                        {
                            tempScheduleDetailsDTO.AvailableUnits = -1;
                        }
                        else
                        {
                            tempScheduleDetailsDTO.AvailableUnits = totalUnits - bookedUnits;
                        }

                        // reset the price so that it does not carry from previous price
                        tempScheduleDetailsDTO.Price = null;

                        List<ProductsAllowedInFacilityMapDTO> tempAllowedProductsList = new List<ProductsAllowedInFacilityMapDTO>();
                        foreach (ProductsAllowedInFacilityMapDTO tempAllowedProd in tempScheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList)
                        {
                            if (productIdList.Any(y => y == tempAllowedProd.ProductsId))
                            {
                                if (promoProducts.Contains(tempAllowedProd.ProductsId))
                                {
                                    // get a new product dto from DB to avoid cyclic application of promo price on a cached ProductDTO
                                    //Products products = new Products(executionContext, tempAllowedProd.ProductsDTO.ProductId);

                                    ProductsContainerDTO tempProductsContainerDTO = ProductsContainerList.GetProductsContainerDTO(executionContext.GetSiteId(), tempAllowedProd.ProductsDTO.ProductId);
                                    double promotionPrice = (double)tempProductsContainerDTO.Price;
                                    int PromotionId = Promotions.getProductPromotionPrice(null, tempAllowedProd.ProductsDTO.ProductId, tempAllowedProd.ProductsDTO.CategoryId, promotionPrice.ToString(),
                                                                          tempAllowedProd.ProductsDTO.TaxPercentage, ref promotionPrice, parafaitUtility, tempScheduleDetailsDTO.ScheduleFromDate.AddSeconds(offsetTimeSecs));
                                    tempScheduleDetailsDTO.PromotionId = PromotionId;
                                    tempScheduleDetailsDTO.Price = promotionPrice;
                                    tempAllowedProd.ProductsDTO.Price = (decimal)promotionPrice;
                                    log.LogVariableState("promotionPrice applied", promotionPrice);
                                }

                                if (tempScheduleDetailsDTO.Price == null || tempScheduleDetailsDTO.Price == -1)
                                {
                                    log.LogVariableState(" Non promo price applier ", tempAllowedProd.ProductsDTO.Price);
                                    tempScheduleDetailsDTO.Price = tempAllowedProd.ProductsDTO.Price == -1 ? (tempScheduleDetailsDTO.AttractionPlayPrice == null ? -1 : Convert.ToDouble(tempScheduleDetailsDTO.AttractionPlayPrice.ToString())) : (double)tempAllowedProd.ProductsDTO.Price;
                                }
                                // Changed - Filtering in calling function to avoid changing the cached value
                                tempAllowedProductsList.Add(tempAllowedProd);
                            }
                            else if (String.IsNullOrEmpty(productsList))
                            {
                                // Changed - Filtering in calling function to avoid changing the cached value
                                tempAllowedProductsList.Add(tempAllowedProd);
                            }
                            //// Changed - Filtering in calling function to avoid changing the cached value
                            //tempAllowedProductsList.Add(tempAllowedProd);
                        }

                        if (tempAllowedProductsList != null && tempAllowedProductsList.Any())
                        {
                            //tempScheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList = tempAllowedProductsList.OrderBy(x => x.ProductsDTO.SortOrder)
                            //                                                                                                .ThenBy(x => x.ProductsId).ToList();
                            //if (filterProducts)
                            {
                                tempScheduleDetailsDTO.FacilityMapDTO.ProductsAllowedInFacilityDTOList = tempAllowedProductsList;
                            }
                            scheduleDetailsDTOList.Add(tempScheduleDetailsDTO);
                        }
                    }
                }

                // sort by ascending order of time
                scheduleDetailsDTOList = scheduleDetailsDTOList.OrderBy(x => x.ScheduleFromDate).ToList();
                // remove unavailable units if not required. This will be used by websites and app
                if (scheduleDetailsDTOList != null && removeUnavailable != null && removeUnavailable == true)
                {
                    scheduleDetailsDTOList = scheduleDetailsDTOList.Where(x => x.AvailableUnits > 0).OrderBy(x => x.ScheduleFromDate).ToList();
                }
            }

            log.LogMethodExit(scheduleDetailsDTOList);
            return scheduleDetailsDTOList;
        }

        private List<DayAttractionScheduleDTO> GetDayAttractionSchedules(DateTime fromDate, DateTime toDate)
        {
            // if multiple bookings are allowed, then do not prevent from being booked
            log.LogMethodEntry(fromDate, toDate);
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOs = new List<DayAttractionScheduleDTO>();
            DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
            List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME, toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, "Y"));
            List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, null);
            if (dayAttractionScheduleDTOList != null && dayAttractionScheduleDTOList.Any())
            {
                dayAttractionScheduleDTOs.AddRange(dayAttractionScheduleDTOList);
            }
            log.LogMethodExit(dayAttractionScheduleDTOs);
            return dayAttractionScheduleDTOs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduleDate"></param>
        /// <param name="productsList"></param>
        /// <param name="facilityMapId"></param>
        /// <param name="fixedSchedule"></param>
        /// <param name="scheduleFromTime"></param>
        /// <param name="scheduleToTime"></param>
        /// <param name="selectedToDateTime"></param>
        /// <param name="includePast"></param>
        /// <param name="filterProducts"></param>
        /// <param name="buildDetails"></param>
        /// <param name="masterScheduleId"></param>
        /// <param name="isBooking"></param>
        /// <param name="isRescheduleSlot"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="filterFacility"></param>
        /// <param name="removeUnavailable"></param>
        /// <returns></returns>
        public List<ScheduleDetailsViewDTO> BuildScheduleDetailsForView(DateTime scheduleDate, string productsList = null, int facilityMapId = -1, bool? fixedSchedule = null,
            decimal? scheduleFromTime = null, decimal? scheduleToTime = null, DateTime? selectedToDateTime = null, bool includePast = false, bool filterProducts = false,
            bool buildDetails = true, int masterScheduleId = -1, bool isBooking = false, bool isRescheduleSlot = false, int pageNumber = 0, int pageSize = 20, bool? filterFacility = false,
            bool? removeUnavailable = false)
        {
            log.LogMethodEntry();

            Dictionary<int, string> facilityMapNames = new Dictionary<int, string>();
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime currentTime = serverTimeObject.GetServerDateTime();
            Dictionary<int, FacilityMapBL> facilityIdAndMapBLList = new Dictionary<int, FacilityMapBL>();

            List<ScheduleDetailsViewDTO> scheduleDetailsViewDTOList = new List<ScheduleDetailsViewDTO>();

            int startHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 0);
            int graceMinutes = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "ATTRACTION_BOOKING_GRACE_PERIOD", 0);
            bool isAttractionsWithinReservationPeriodOnly = ParafaitDefaultContainerList.GetParafaitDefault<bool>(this.executionContext, "ATTRACTIONS_WITHIN_RESERVATION_PERIOD_ONLY", false);

            int currentFacilityMapSelection = facilityMapId;// set as this is reset when the facility maps are added
            int productId = -1;
            List<int> inputproductsList = new List<int>();
            if (!String.IsNullOrEmpty(productsList))
            {
                String[] products = productsList.Split(',');
                if (products != null && products.Count() > 0)
                {
                    foreach (string product in products)
                        inputproductsList.Add(Convert.ToInt32(product));

                    productId = Convert.ToInt32(products[0]);
                }
            }

            List<ScheduleDetailsDTO> schedules = GetAttractionBookingSchedules(scheduleDate, productsList, //productId != -1 ? productId.ToString() : "",
                 string.IsNullOrWhiteSpace(productsList) ? facilityMapId : -1, null, scheduleFromTime, scheduleToTime, true);

            if (schedules != null && removeUnavailable != null && removeUnavailable == true)
            {
                schedules = schedules.Where(x => x.AvailableUnits > 0).OrderBy(x => x.ScheduleFromDate).ToList();
            }

            if (schedules != null && productId > -1)
            {
                foreach (ScheduleDetailsDTO scheduleDetailsDTO in schedules)
                    scheduleDetailsDTO.ProductId = productId;
            }

            if (schedules != null && schedules.Any() && !includePast)
            {
                string gracePeriod = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ATTRACTION_BOOKING_GRACE_PERIOD");
                int gracePeriodMin = 0;
                if (!int.TryParse(gracePeriod, out gracePeriodMin))
                    gracePeriodMin = 0;

                log.Debug("Filtering for offset corporate flag " + executionContext.IsCorporate + " current time " + currentTime);
                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                int offsetTimeSecs = timeZoneUtil.GetOffSetDuration(schedules[0].SiteId, schedules[0].ScheduleFromDate.Date);
                log.Debug("After offset : offset time " + offsetTimeSecs + "first schedule time " + schedules[0].ScheduleFromDate.AddSeconds(offsetTimeSecs) + " current time " + currentTime.AddMinutes(-1 * graceMinutes));
                schedules = schedules.Where(x => x.ScheduleFromDate.AddSeconds(offsetTimeSecs) >= (isBooking ? scheduleDate : currentTime.AddMinutes(-1 * graceMinutes))).ToList();
            }

            if (isBooking && isAttractionsWithinReservationPeriodOnly)
            {
                Decimal from = (decimal)(scheduleDate.Hour + Math.Round(scheduleDate.Minute / 100.0, 2));
                DateTime date = selectedToDateTime != null ? Convert.ToDateTime(selectedToDateTime) : DateTime.MinValue;
                Decimal to = selectedToDateTime == DateTime.MinValue ? 24 : (decimal)(date.Hour + Math.Round(date.Minute / 100.0, 2));
                schedules = schedules.Where(sch => sch.ScheduleFromTime >= from && sch.ScheduleToTime <= to).ToList();
            }

            if (schedules != null && schedules.Any())
            {
                schedules = schedules.OrderBy(x => x.ScheduleFromDate).ToList();

                if (schedules != null && removeUnavailable != null && removeUnavailable == true)
                {
                    schedules = schedules.Where(x => x.AvailableUnits > 0).OrderBy(x => x.ScheduleFromDate).ToList();
                }

                List<int> facilityIdList = schedules.Select(x => x.FacilityMapId).Distinct().ToList();
                facilityIdAndMapBLList = LoadFacilityMapDetails(facilityMapNames, facilityIdAndMapBLList, facilityIdList);

                List<DayAttractionScheduleDTO> dayAttractionSchedulesList = GetDayAttractionSchedules(schedules[0].ScheduleFromDate, schedules[schedules.Count - 1].ScheduleToDate);
                if (dayAttractionSchedulesList != null && dayAttractionSchedulesList.Any())
                {
                    facilityIdList = dayAttractionSchedulesList.Select(x => x.FacilityMapId).Distinct().ToList();
                    List<int> newMapIdList = new List<int>();
                    foreach (int tempFMId in facilityIdList)
                    {
                        if (!facilityMapNames.ContainsKey(tempFMId) && tempFMId > -1 && newMapIdList.Exists(id => id == tempFMId) == false)
                        {
                            newMapIdList.Add(tempFMId);
                        }
                    }
                    facilityIdAndMapBLList = LoadFacilityMapDetails(facilityIdAndMapBLList, newMapIdList);
                }
                facilityMapNames = LoadfacilityMapNames(facilityMapNames, facilityIdAndMapBLList);

                foreach (ScheduleDetailsDTO scheduleDetailsDTO in schedules)
                {
                    ScheduleDetailsViewDTO scheduleDetailsViewDTO = new ScheduleDetailsViewDTO();

                    scheduleDetailsViewDTO.ScheduleDetailsDTO = scheduleDetailsDTO;
                    bool multiBookingAllowed = true;
                    try
                    {
                        if (scheduleDetailsViewDTO.ScheduleDetailsDTO.FacilityMapId > -1)
                        {
                            facilityIdAndMapBLList[scheduleDetailsViewDTO.ScheduleDetailsDTO.FacilityMapId].CanAllowMulitpleBookingsForTheSlot(null);
                            multiBookingAllowed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        multiBookingAllowed = false;
                    }

                    String backColor = "255,255,255";
                    bool pastSchedule = false;
                    bool visible = false;

                    if (scheduleDetailsDTO.ScheduleFromDate < (isBooking ? scheduleDate : currentTime.AddMinutes(-1 * graceMinutes)))
                    {
                        backColor = GetBackPanelColor("UNAVAILABLE", "");
                        pastSchedule = true;
                    }

                    // Booked units is send as null, reset to 0
                    if (scheduleDetailsDTO.BookedUnits == null)
                        scheduleDetailsDTO.BookedUnits = 0;

                    if (scheduleDetailsDTO.DayAttractionScheduleId > -1)
                    {
                        List<DayAttractionScheduleDTO> dsaForSchedule = dayAttractionSchedulesList.Where(x => x.DayAttractionScheduleId == scheduleDetailsDTO.DayAttractionScheduleId).ToList();
                        if (dsaForSchedule.Count > 0)
                        {
                            scheduleDetailsViewDTO.DayAttractionScheduleDTO = dsaForSchedule[0];

                            if (facilityMapNames.ContainsKey(dsaForSchedule[0].FacilityMapId))
                            {
                                scheduleDetailsDTO.FacilityMapName = facilityMapNames[dsaForSchedule[0].FacilityMapId];
                            }
                            else
                            {
                                FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, dsaForSchedule[0].FacilityMapId);
                                facilityMapNames.Add(dsaForSchedule[0].FacilityMapId, facilityMapBL.FacilityMapDTO.FacilityMapName);
                                scheduleDetailsDTO.FacilityMapName = facilityMapBL.FacilityMapDTO.FacilityMapName;
                            }

                            if (!dsaForSchedule[0].ScheduleStatus.Equals(DayAttractionScheduleDTO.ScheduleStatusEnumToString(DayAttractionScheduleDTO.ScheduleStatusEnum.OPEN)))
                            {
                                backColor = GetBackPanelColor(dsaForSchedule[0].ScheduleStatus, "");

                                if (!isRescheduleSlot)
                                    pastSchedule = true;
                            }
                            else if (dsaForSchedule[0].FacilityMapId != scheduleDetailsDTO.FacilityMapId)
                            {
                                if (!multiBookingAllowed)
                                    backColor = GetBackPanelColor("", "DIFFERENT_FACILITY");

                                if (!multiBookingAllowed && !isRescheduleSlot)
                                    pastSchedule = true;

                                if (dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                                {
                                    backColor = GetBackPanelColor("", "RESERVATION");
                                    if (isRescheduleSlot)
                                        pastSchedule = true;
                                }

                            }
                            else if ((isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                                && dsaForSchedule[0].Blocked && (dsaForSchedule[0].ExpiryTime == DateTime.MinValue || dsaForSchedule[0].ExpiryTime > currentTime))
                            {
                                backColor = GetBackPanelColor("", "RESERVATION");
                                // allow the user to select the slot, do not block but give a different color to indicate that it is a reservation slot
                                //pastSchedule = true;
                            }
                            else if (!isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                            {
                                if (!multiBookingAllowed)
                                {
                                    backColor = GetBackPanelColor("", "RESERVATION");
                                    // do not allow the user to select the slot
                                    // if (!isRescheduleSlot)
                                    pastSchedule = true;
                                }
                            }
                            else if (isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.WALK_IN)))
                            {
                                if (!multiBookingAllowed)
                                {
                                    backColor = GetBackPanelColor("", "DIFFERENT_FACILITY");
                                    // do not allow the user to select the slot
                                    pastSchedule = true;
                                }
                            }
                            else if (scheduleDetailsDTO.AvailableUnits == 0)
                            {
                                backColor = GetBackPanelColor("SOLDOUT", "");
                            }
                        }
                    }
                    else
                    {
                        // need to check if the slot exists for a different FM
                        List<DayAttractionScheduleDTO> dsaForSchedule = dayAttractionSchedulesList.Where(x => (
                                                                        (x.ScheduleDateTime <= scheduleDetailsDTO.ScheduleFromDate && x.ScheduleToDateTime >= scheduleDetailsDTO.ScheduleToDate) ||
                                                                        (x.ScheduleDateTime <= scheduleDetailsDTO.ScheduleFromDate && x.ScheduleToDateTime <= scheduleDetailsDTO.ScheduleToDate && x.ScheduleToDateTime > scheduleDetailsDTO.ScheduleFromDate) ||
                                                                        (x.ScheduleDateTime >= scheduleDetailsDTO.ScheduleFromDate && x.ScheduleDateTime < scheduleDetailsDTO.ScheduleToDate && x.ScheduleToDateTime >= scheduleDetailsDTO.ScheduleToDate)) &&
                                                                        relatedFacilityMaps[scheduleDetailsDTO.FacilityMapId].Contains(x.FacilityMapId) &&
                                                                        (x.ExpiryTime == DateTime.MinValue || x.ExpiryTime > currentTime)).ToList();
                        if (dsaForSchedule != null && dsaForSchedule.Any())
                        {

                            if ((isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                                && dsaForSchedule[0].Blocked && (dsaForSchedule[0].ExpiryTime == DateTime.MinValue || dsaForSchedule[0].ExpiryTime > currentTime))
                            {
                                backColor = GetBackPanelColor("", "RESERVATION");
                            }
                            else if (!isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                            {
                                if (!multiBookingAllowed)
                                {
                                    backColor = GetBackPanelColor("", "RESERVATION");
                                    // do not allow the user to select the slot
                                    pastSchedule = true;
                                }
                            }
                            else if (isBooking && dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.WALK_IN)))
                            {
                                if (!multiBookingAllowed)
                                {
                                    backColor = GetBackPanelColor("", "DIFFERENT_FACILITY");
                                    // do not allow the user to select the slot
                                    pastSchedule = true;
                                }
                            }
                            else if (dsaForSchedule[0].FacilityMapId != scheduleDetailsDTO.FacilityMapId)
                            {
                                if (!multiBookingAllowed)
                                    backColor = GetBackPanelColor("", "DIFFERENT_FACILITY");

                                if (!multiBookingAllowed && !isRescheduleSlot)
                                    pastSchedule = true;

                                if (dsaForSchedule[0].Source.Equals(DayAttractionScheduleDTO.SourceEnumToString(DayAttractionScheduleDTO.SourceEnum.RESERVATION)))
                                {
                                    backColor = GetBackPanelColor("", "RESERVATION");
                                    if (isRescheduleSlot)
                                        pastSchedule = true;
                                }
                            }
                        }

                        if (isRescheduleSlot)
                            scheduleDetailsDTO.FacilityMapName = String.Empty;
                        else
                        {
                            if (facilityMapNames.ContainsKey(scheduleDetailsDTO.FacilityMapId))
                            {
                                scheduleDetailsDTO.FacilityMapName = facilityMapNames[scheduleDetailsDTO.FacilityMapId];
                            }
                            else
                            {
                                FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, scheduleDetailsDTO.FacilityMapId);
                                facilityMapNames.Add(scheduleDetailsDTO.FacilityMapId, facilityMapBL.FacilityMapDTO.FacilityMapName);
                                scheduleDetailsDTO.FacilityMapName = facilityMapBL.FacilityMapDTO.FacilityMapName;
                            }
                        }
                    }

                    if (isBooking && isAttractionsWithinReservationPeriodOnly)
                    {
                        if (scheduleDetailsDTO.ScheduleFromDate > selectedToDateTime)
                        {
                            backColor = GetBackPanelColor("UNAVAILABLE", "");
                            pastSchedule = true;
                        }
                    }

                    if (scheduleDetailsDTO.AvailableUnits == 0 && scheduleDetailsDTO.TotalUnits == 0)
                    {
                        backColor = GetBackPanelColor("UNAVAILABLE", "");
                        pastSchedule = true;
                    }

                    if (currentFacilityMapSelection < 0)
                    {
                        visible = true;
                    }
                    else
                    {
                        if (scheduleDetailsDTO.FacilityMapId == currentFacilityMapSelection)
                        {
                            visible = true;
                        }
                        else
                        {
                            visible = false;
                        }
                    }

                    scheduleDetailsViewDTO.BackColor = backColor;
                    scheduleDetailsViewDTO.PastSchedule = pastSchedule;
                    scheduleDetailsViewDTO.Visible = visible;
                    scheduleDetailsViewDTOList.Add(scheduleDetailsViewDTO);
                }

                if (currentFacilityMapSelection > -1 && filterFacility != null && Convert.ToBoolean(filterFacility) == true)
                {
                    scheduleDetailsViewDTOList = scheduleDetailsViewDTOList.Where(x => x.ScheduleDetailsDTO.FacilityMapId == currentFacilityMapSelection).ToList();
                }
            }
            log.LogMethodExit(scheduleDetailsViewDTOList);
            return scheduleDetailsViewDTOList;
        }

        private String GetBackPanelColor(String status, String origin)
        {
            if (statusCellColorMap == null || !statusCellColorMap.Any())
            {
                LookupValuesList lookUpList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookUpValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookUpValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "ATTRACTION_SCHEDULE_BACKCOLORS"));
                List<LookupValuesDTO> lookUpValuesList = lookUpList.GetAllLookupValues(lookUpValuesSearchParams);
                if ((lookUpValuesList != null) && (lookUpValuesList.Any()))
                {
                    foreach (LookupValuesDTO lookupValueDTO in lookUpValuesList)
                    {
                        if (!statusCellColorMap.ContainsKey(lookupValueDTO.LookupValue))
                        {
                            statusCellColorMap.Add(lookupValueDTO.LookupValue, lookupValueDTO.Description);
                        }
                    }
                }
            }

            String colorLabel = "";
            if (!String.IsNullOrEmpty(status))
            {
                switch (status.ToUpper())
                {
                    case "OPEN":
                        colorLabel = "OPEN";
                        break;
                    case "UNAVAILABLE":
                        colorLabel = "UNAVAILABLE";
                        break;
                    case "BLOCKED":
                        colorLabel = "BLOCKED";
                        break;
                    case "SOLDOUT":
                        colorLabel = "SOLDOUT";
                        break;
                    case "RACE_IN_PROGRESS":
                    case "RACING":
                    case "FINISHED":
                    case "ABORTED":
                    case "CLOSED":
                        colorLabel = "RACE_IN_PROGRESS";
                        break;
                    case "RESCHEDULE_IN_PROGRESS":
                    case "RESCHEDULE":
                        colorLabel = "RESCHEDULE_IN_PROGRESS";
                        break;
                    default:
                        colorLabel = "OPEN";
                        break;
                };
            }
            else
            {
                switch (origin)
                {
                    case "PARTY_RESERVATION":
                    case "RESERVATION":
                        colorLabel = "PARTY_RESERVATION";
                        break;
                    case "DIFFERENT_FACILITY":
                        colorLabel = "UNAVAILABLE";
                        break;
                    default:
                        colorLabel = "OPEN";
                        break;
                };
            }

            if (statusCellColorMap.ContainsKey(colorLabel))
                return statusCellColorMap[colorLabel];
            else
                return "255,255,255";
        }

        public ScheduleDetailsDTO GetScheduleDetailsById(DateTime businessDateTime, int facilityMapId, int attractionScheduleId, int siteId)
        {
            log.LogMethodEntry(businessDateTime, facilityMapId, attractionScheduleId);

            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
            int offSetDuration = 0;
            offSetDuration = timeZoneUtil.GetOffSetDuration(siteId, businessDateTime);
            log.Debug("Before offset " + businessDateTime);
            businessDateTime = businessDateTime.AddSeconds(offSetDuration * -1);
            log.Debug("After  offset " + businessDateTime);
            int businessStartHour = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "BUSINESS_DAY_START_TIME", 6);
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            if (businessDateTime.Hour < businessStartHour)
                businessDateTime = businessDateTime.AddDays(-1).Date.AddHours(businessStartHour);
            else
                businessDateTime = businessDateTime.Date.AddHours(businessStartHour);

            log.Debug("Final " + businessDateTime);
            ScheduleDetailsDTO scheduleDetailsDTO = null;
            List<ScheduleDetailsDTO> schedules = GetAttractionBookingSchedules(businessDateTime, null, facilityMapId, null, 0, 24, true, false, true, false);
            if (schedules != null && schedules.Any())
            {
                scheduleDetailsDTO = schedules.FirstOrDefault(x => x.FacilityMapId == facilityMapId && x.ScheduleId == attractionScheduleId);
            }

            log.LogMethodExit(scheduleDetailsDTO);
            return scheduleDetailsDTO;
        }


        private Dictionary<int, FacilityMapBL> LoadFacilityMapDetails(Dictionary<int, string> facilityMapNames, Dictionary<int, FacilityMapBL> facilityIdAndMapBLList, List<int> facilityIdList)
        {
            log.LogMethodEntry(facilityMapNames, "facilityIdAndMapBLList", facilityIdList);
            if (facilityIdList != null && facilityIdList.Any())
            {
                FacilityMapListBL listBL = new FacilityMapListBL(executionContext);
                List<FacilityMapDTO> mapDTOList = listBL.GetFacilityMapDTOList(facilityIdList, true, true);
                if (mapDTOList != null && mapDTOList.Any())
                {
                    foreach (FacilityMapDTO tempFMDTO in mapDTOList)
                    {
                        if (!facilityMapNames.ContainsKey(tempFMDTO.FacilityMapId) && tempFMDTO.FacilityMapId > -1)
                        {
                            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, tempFMDTO);
                            facilityMapNames.Add(tempFMDTO.FacilityMapId, facilityMapBL.FacilityMapDTO.FacilityMapName);
                            if (facilityIdAndMapBLList.ContainsKey(tempFMDTO.FacilityMapId) == false)
                            {
                                facilityIdAndMapBLList.Add(tempFMDTO.FacilityMapId, facilityMapBL);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(facilityIdAndMapBLList);
            return facilityIdAndMapBLList;
        }

        private Dictionary<int, FacilityMapBL> LoadFacilityMapDetails(Dictionary<int, FacilityMapBL> facilityIdAndMapBLList, List<int> facilityIdList)
        {
            log.LogMethodEntry("facilityIdAndMapBLList", facilityIdList);
            if (facilityIdList != null && facilityIdList.Any())
            {
                FacilityMapListBL listBL = new FacilityMapListBL(executionContext);
                List<FacilityMapDTO> mapDTOList = listBL.GetFacilityMapDTOList(facilityIdList, true, true);
                if (mapDTOList != null && mapDTOList.Any())
                {
                    foreach (FacilityMapDTO tempFMDTO in mapDTOList)
                    {
                        if (facilityIdAndMapBLList.ContainsKey(tempFMDTO.FacilityMapId) == false)
                        {
                            FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, tempFMDTO);
                            facilityIdAndMapBLList.Add(tempFMDTO.FacilityMapId, facilityMapBL);
                        }
                    }
                }
            }
            log.LogMethodExit(facilityIdAndMapBLList);
            return facilityIdAndMapBLList;
        }

        private Dictionary<int, string> LoadfacilityMapNames(Dictionary<int, string> facilityMapNames, Dictionary<int, FacilityMapBL> facilityIdAndMapBLList)
        {
            log.LogMethodEntry(facilityMapNames, "facilityIdAndMapBLList");
            foreach (var item in facilityIdAndMapBLList)
            {
                if (facilityMapNames.ContainsKey(item.Key) == false)
                {
                    string facName = item.Value.FacilityMapDTO.FacilityMapName;
                    facilityMapNames.Add(item.Key, facName);
                }
            }
            log.LogMethodExit(facilityMapNames);
            return facilityMapNames;
        }

        private int GetOffSet(List<Tuple<int, DateTime, int>> offsetData, TimeZoneUtil timeZoneUtil, int siteId, DateTime selectedDate)
        {
            log.LogMethodEntry(offsetData, "timeZoneUtil", siteId, selectedDate);
            int offsetValue = 0;
            Tuple<int, DateTime, int> data = offsetData.Find(od => od.Item1 == siteId && od.Item2 == selectedDate);
            if (data != null)
            {
                offsetValue = data.Item3;
            }
            else
            {
                offsetValue = timeZoneUtil.GetOffSetDuration(siteId, selectedDate);
                offsetData.Add(new Tuple<int, DateTime, int>(siteId, selectedDate, offsetValue));
            }
            log.LogMethodExit(offsetValue);
            return offsetValue;
        }
    }
}
