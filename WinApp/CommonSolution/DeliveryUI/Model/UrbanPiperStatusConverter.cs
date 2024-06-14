/********************************************************************************************
 * Project Name - Delivery UI
 * Description  - StatusConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     29-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.KDS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Semnox.Parafait.DeliveryUI
{
    public class UrbanPiperStatusConverter: IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetType, parameter, culture);
            string status = "None";
            object style = Application.Current.FindResource("CompleteButtonStyle");
            string transactionStatus = string.Empty;
            string transactionGuid = (string)value;
            IEnumerable<object> parameterList = (IEnumerable<object>)parameter;
            if (parameterList == null)
            {
                log.LogMethodExit(status);
                return status;
            }

            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parameterList.ElementAt(0) as List<ParafaitMessageQueueDTO>;

            if (parafaitMessageQueueDTOList != null && parafaitMessageQueueDTOList.Any() && !string.IsNullOrEmpty(transactionGuid) && parafaitMessageQueueDTOList.Exists(x => x.EntityGuid == transactionGuid && string.IsNullOrEmpty(x.Message) == false))
            {
                List<ParafaitMessageQueueDTO> parafaitMessageQueueofCurrentTransaction = parafaitMessageQueueDTOList.Where(x => x.EntityGuid == transactionGuid && string.IsNullOrEmpty(x.Message) == false).OrderByDescending(x=>x.MessageQueueId).ToList(); 
                if(parafaitMessageQueueofCurrentTransaction != null && parafaitMessageQueueofCurrentTransaction.Any())
                {
                    if (parafaitMessageQueueofCurrentTransaction[0].Status.ToString().ToUpper() == MessageQueueStatus.Read.ToString().ToUpper() 
                        || parafaitMessageQueueofCurrentTransaction[0].Attempts >= 3)
                    {
                        status = "Complete";
                        style = Application.Current.FindResource("CompleteButtonStyle");
                        
                    }
                    else if(parafaitMessageQueueofCurrentTransaction[0].Status.ToString().ToUpper() == MessageQueueStatus.UnRead.ToString().ToUpper())
                    {
                        status = "Processing";
                        style = Application.Current.FindResource("ProcessingButtonStyle");
                    }
                }
            }
            log.Debug("transactionGuid"+ transactionGuid + " MessageQueueStatus:" + status);
            log.LogMethodExit(style);
            return style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
