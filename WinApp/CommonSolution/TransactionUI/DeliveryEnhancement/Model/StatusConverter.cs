/********************************************************************************************
 * Project Name - Transaction UI
 * Description  - StatusConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     29-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Parafait.Transaction;
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
    public class StatusConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetType, parameter, culture);
            string status = string.Empty;
            int transactionLineId = (int)value;
            List<TransactionLineDTO> transactionLineDTOList = parameter as List<TransactionLineDTO>;
            TransactionLineDTO selectedTransactionLineDTO = transactionLineDTOList.Where(trx => trx.LineId == transactionLineId).FirstOrDefault();
            if (selectedTransactionLineDTO.KDSOrderLineDTOList != null && selectedTransactionLineDTO.KDSOrderLineDTOList.Any())
            {
                KDSOrderLineDTO defaultKDSOrderLineDTO = selectedTransactionLineDTO.KDSOrderLineDTOList.OrderByDescending(x => x.Id).FirstOrDefault();
                //if (defaultKDSOrderLineDTO.EntryType == KDSOrderLineDTO.KDSKOTEntryType.KDS)
                //{
                if (defaultKDSOrderLineDTO.ScheduleTime != null && defaultKDSOrderLineDTO.OrderedTime == null && defaultKDSOrderLineDTO.PrepareStartTime == null && defaultKDSOrderLineDTO.PreparedTime == null && defaultKDSOrderLineDTO.DeliveredTime == null)
                {
                    status = "Scheduled";
                }
                else if (defaultKDSOrderLineDTO.OrderedTime != null && defaultKDSOrderLineDTO.PrepareStartTime == null && defaultKDSOrderLineDTO.PreparedTime == null && defaultKDSOrderLineDTO.DeliveredTime == null)
                {
                    status = "Ordered";
                }
                else if (defaultKDSOrderLineDTO.PrepareStartTime != null && defaultKDSOrderLineDTO.PreparedTime == null && defaultKDSOrderLineDTO.DeliveredTime == null)
                {
                    status = "WIP";
                }
                else if (defaultKDSOrderLineDTO.PreparedTime != null && defaultKDSOrderLineDTO.DeliveredTime == null)
                {
                    status = "Prepared";
                }
                else if (defaultKDSOrderLineDTO.DeliveredTime != null)
                {
                    status = "Delivered";
                }
                //}

            }

            log.LogMethodExit(status);
            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
