using Semnox.Parafait.Inventory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.InventoryUI
{
    public class LotIdToExpiryDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int lotId = (Int32)value;
            if (lotId != -1)
            {
                List<object> collection = parameter as List<object>;
                InventoryLotDTO inventoryLotDTO = (collection[0] as List<InventoryLotDTO>).FirstOrDefault(p => p.LotId == lotId);
                string dateTimeFormat = collection[1] as string;
                if (inventoryLotDTO != null)
                {
                    if (inventoryLotDTO.Expirydate.Equals(DateTime.MinValue))
                    {
                        return "";
                    }
                    else
                    {
                        return inventoryLotDTO.Expirydate.ToString(dateTimeFormat);
                    }
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
