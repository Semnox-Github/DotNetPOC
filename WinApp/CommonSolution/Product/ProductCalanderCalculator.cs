/********************************************************************************************
 * Project Name - Product
 * Description  - class of to calculate the product menu calendar
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      06-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class ProductCalendarCalculator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<ProductsCalenderDTO> productsCalenderDTOList;

        public ProductCalendarCalculator(List<ProductsCalenderDTO> productsCalenderDTOList)
        {
            log.LogMethodEntry(productsCalenderDTOList);
            this.productsCalenderDTOList = productsCalenderDTOList;
            log.LogMethodExit();
        }

        public bool IsProductAvailableOn(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool result = false;
            if(productsCalenderDTOList == null || productsCalenderDTOList.Any(x => x.IsActive) == false)
            {
                result = true;
                log.LogMethodExit(result, "productsCalenderDTOList is empty");
                return result;
            }
            double nowHour = GetNowHour(dateTime);
            string day = GetDay(dateTime);
            string weekDay = GetWeekDay(dateTime);
            ProductsCalenderDTO applicableProductsCalenderDTO = productsCalenderDTOList
                                                               .OrderByDescending(x => x.ProductCalendarId)
                                                               .Where(x => x.Date == dateTime.Date || x.Day == day || x.Day == weekDay || x.Day == "-1")
                                                               .OrderByDescending(x => x.Date)
                                                               .OrderByDescending(x => string.IsNullOrWhiteSpace(x.Day)? -1 : Convert.ToInt32(x.Day))
                                                               .OrderBy(x => GetFromTimeToTimeSortOrder(x, nowHour))
                                                               .FirstOrDefault();
            if (applicableProductsCalenderDTO == null)
            {
                log.LogMethodExit(result, "applicableProductsCalenderDTO is empty");
                return result;
            }
            bool nowHourFallsWithinFromTimeAndToTime = IsNowHourWithinFromTimeAndToTime(applicableProductsCalenderDTO, nowHour);
            result = nowHourFallsWithinFromTimeAndToTime && applicableProductsCalenderDTO.ShowHide || 
                    !nowHourFallsWithinFromTimeAndToTime && !applicableProductsCalenderDTO.ShowHide;
            log.LogMethodExit(result);
            return result;
        }

        private int GetFromTimeToTimeSortOrder(ProductsCalenderDTO productsCalenderDTO, double nowHour)
        {
            int result = IsNowHourWithinFromTimeAndToTime(productsCalenderDTO, nowHour) ? 0 : 1;
            return result;
        }

        private bool IsNowHourWithinFromTimeAndToTime(ProductsCalenderDTO productsCalenderDTO, double nowHour)
        {
            double fromTime = productsCalenderDTO.FromTime.HasValue ? productsCalenderDTO.FromTime.Value : nowHour;
            double toTime = productsCalenderDTO.ToTime.HasValue ? productsCalenderDTO.ToTime.Value == 0 ? 24 : productsCalenderDTO.ToTime.Value : nowHour;
            bool result = nowHour >= fromTime && nowHour <= toTime;
            return result;
        }

        private string GetWeekDay(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            string result = "-1";
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday: result = "0"; break;
                case DayOfWeek.Monday: result = "1"; break;
                case DayOfWeek.Tuesday: result = "2"; break;
                case DayOfWeek.Wednesday: result = "3"; break;
                case DayOfWeek.Thursday: result = "4"; break;
                case DayOfWeek.Friday: result = "5"; break;
                case DayOfWeek.Saturday: result = "6"; break;
            }
            log.LogMethodExit(result);
            return result;
        }

        private string GetDay(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            string result = (dateTime.Day + 1000).ToString();
            log.LogMethodExit(result);
            return result;
        }

        private double GetNowHour(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            double result = dateTime.Hour + dateTime.Minute / 100.0d;
            log.LogMethodExit(result);
            return result;
        }
    }
}
