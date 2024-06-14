using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
namespace Semnox.Parafait.TransactionUI
{
    public class IssuedDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int userId = (Int32)value;
            List<object> collection = parameter as List<object>;
            UsersDTO user = (collection[1] as List<UsersDTO>).FirstOrDefault(u => u.UserId == userId);
            if(user != null)
            {
                UserIdentificationTagsDTO userIdentificationTag = user.UserIdentificationTagsDTOList.FirstOrDefault();
                if (userIdentificationTag != null && userIdentificationTag.StartDate != DateTime.MinValue)
                {
                    return userIdentificationTag.StartDate.ToString(ParafaitDefaultViewContainerList.GetParafaitDefault(collection[0] as ExecutionContext, "DATE_FORMAT"));
                }
            }
            return "-";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
