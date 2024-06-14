/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - CalculateMaximumScheduledTimeConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     29-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Parafait.Transaction.KDS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.TransactionUI
{
    public class CalculateMaximumScheduledTimeConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetType, parameter, culture);
            string maxDate = string.Empty;
            List<KDSOrderLineDTO> kDSOrderLines = value as List<KDSOrderLineDTO>;
            if (kDSOrderLines != null)
            {
                DateTime? dateTime = kDSOrderLines.Max(s => s.ScheduleTime);
                if (dateTime != null)
                {
                    if (parameter != null)
                    {
                        maxDate = ((DateTime)dateTime).ToString(parameter as string);
                    }
                    else
                    {
                        maxDate = ((DateTime)dateTime).ToString();
                    }
                }
            }
            log.LogMethodExit(maxDate);
            return maxDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
