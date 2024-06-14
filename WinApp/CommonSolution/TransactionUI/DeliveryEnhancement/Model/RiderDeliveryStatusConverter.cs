/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - RiderDeliveryStatusConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     30-Jun-2021    Fiona                  Created 
 ********************************************************************************************/

using Semnox.Core.Utilities; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq; 
using System.Windows.Data;

namespace Semnox.Parafait.TransactionUI
{
    public class RiderDeliveryStatusConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            List<LookupValuesContainerDTO> lookupValuesContainerDTOList = parameter as List<LookupValuesContainerDTO>;
            if (lookupValuesContainerDTOList != null && value != null)
            {
                LookupValuesContainerDTO lookup = lookupValuesContainerDTOList.FirstOrDefault(l => l.LookupValueId == (int)value);
                if (lookup != null)
                {
                    return lookup;
                }
            }
            log.LogMethodEntry();
            return value;


            //Dictionary<int, string> keyValues = new Dictionary<int, string>();

            //foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupValuesContainerDTOList)
            //{
            //    keyValues.Add(lookupValuesContainerDTO.LookupValueId, lookupValuesContainerDTO.LookupValue);
            //}
            //if(keyValues.ContainsKey(riderDeliveryStatusId))
            //{
            //    riderDeliveryStatus = keyValues[riderDeliveryStatusId];
            //}
            //log.LogMethodExit(riderDeliveryStatus);
            //return riderDeliveryStatus;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LookupValuesContainerDTO lookupValuesContainerDTO = value as LookupValuesContainerDTO;
            if (lookupValuesContainerDTO != null)
            {
                int lookupid = lookupValuesContainerDTO.LookupValueId;
                if (lookupValuesContainerDTO != null)
                {
                    return lookupid;
                }
            }
            return value;
        }
    }
}
