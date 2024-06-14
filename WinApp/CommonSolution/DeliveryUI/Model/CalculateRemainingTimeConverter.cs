/********************************************************************************************
 * Project Name - Delivery UI
 * Description  - CalculateRemainingTimeConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     29-Jun-2021    Raja Uthanda                  Created 
 ********************************************************************************************/
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.DeliveryUI
{
    public class CalculateRemainingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string remainingTimeText = string.Empty;
            if (value != null)
            {
                TransactionOrderDispensingDTO orderDispensingDTO = value as TransactionOrderDispensingDTO;
                if (orderDispensingDTO != null)
                {
                    TimeSpan remainingTime = (DateTime)orderDispensingDTO.ScheduledDispensingTime - DateTime.Now;
                    if (remainingTime.Seconds < 0 || remainingTime.Minutes < 0 || remainingTime.Hours < 0 || remainingTime.Days < 0)
                    {
                        remainingTimeText = "0 sec";
                    }
                    else
                    { 
                        char maximumTime = 'S';
                        remainingTimeText = remainingTime.Seconds.ToString() + " sec";
                        if (remainingTime.Minutes > 0)
                        {
                            maximumTime = 'M';
                            remainingTimeText = remainingTime.Minutes.ToString() + " mins";
                        }
                        if(remainingTime.Hours > 0)
                        {
                            if(maximumTime == 'M')
                            {
                                remainingTimeText = string.Format("{0:0.00}",remainingTime.Hours + (remainingTime.Minutes / (double)60), 2).ToString() + " hrs ";
                            }
                            else
                            {
                                remainingTimeText = string.Format("{0:0.00}", remainingTime.Hours).ToString() + " hrs";
                            }
                            maximumTime = 'H';
                        }
                        if(remainingTime.Days > 0)
                        {
                            if (maximumTime == 'H')
                            {
                                remainingTimeText = string.Format("{0:0.00}", remainingTime.Hours + remainingTime.Hours + (remainingTime.Minutes / (double)60), 2).ToString() + " hrs ";
                            }
                            else
                            {
                                remainingTimeText = string.Format("{0:0.00}", remainingTime.Days * 24).ToString() + " hrs";
                            }
                        }
                    }
                }
            }
            return remainingTimeText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
