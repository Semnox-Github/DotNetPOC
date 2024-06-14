/********************************************************************************************
 * Project Name - Delivery UI
 * Description  - RiderNameConverter 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     29-Jun-2021    Fiona                  Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using Semnox.Parafait.ViewContainer;
using System; 
using System.Globalization; 
using System.Windows.Data;

namespace Semnox.Parafait.DeliveryUI
{
    public class RiderNameConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            string name = "-";
            ExecutionContext executionContext = parameter as ExecutionContext;
            int riderId = (int)value;
            UserContainerDTO userContainerDTO = UserViewContainerList.GetUserContainerDTOList(parameter as ExecutionContext).Find(user => user.UserId == riderId);
            //string role = UserRoleViewContainerList.GetUserRoleContainerDTO(executionContext.GetSiteId(), userContainerDTO.RoleId).Role;
            if(userContainerDTO!=null)
            {
                name = userContainerDTO.UserName;
            }
            log.LogMethodExit(name);
            return name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
