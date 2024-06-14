using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Semnox.Parafait.User;

namespace Semnox.Parafait.TransactionUI
{
    public class CardConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<UserIdentificationTagsDTO> userIdentificationTags = value as List<UserIdentificationTagsDTO>;
            if(userIdentificationTags.Count > 0)
            {
                return userIdentificationTags.FirstOrDefault().CardNumber;
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
