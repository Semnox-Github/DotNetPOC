using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.TransactionUI
{ 
    public class RoleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;
            List<UserRoleContainerDTO> userRoles = parameter as List<UserRoleContainerDTO>;
            UserRoleContainerDTO userRole = userRoles.FirstOrDefault(s => s.RoleId == (Int32)value);
            return userRole != null ? userRole.Role : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
