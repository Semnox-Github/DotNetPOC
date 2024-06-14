using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.InventoryUI
{
    public class ProductIdNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int productId = (Int32)value;
            List<object> collection = parameter as List<object>;
            ProductsContainerDTO product = (collection[0] as List<ProductsContainerDTO>).FirstOrDefault(p => p.InventoryItemContainerDTO.ProductId == productId);
            if (product != null)
            {
                return product.ProductName;
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
