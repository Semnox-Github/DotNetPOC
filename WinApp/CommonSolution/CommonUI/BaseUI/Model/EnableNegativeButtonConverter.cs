using System;
using System.Reflection;
using System.Windows.Data;
using System.Globalization;

namespace Semnox.Parafait.CommonUI
{
    public class EnableNegativeButtonConverter : IValueConverter
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods	
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetType, parameter, culture);
            bool isEnabled = false;
            if(value != null)
            {
                NumberKeyboardType numberKeyboardType = (NumberKeyboardType)value;
                if(NumberKeyboardType.Both == numberKeyboardType)
                {
                    isEnabled = true;
                }
            }
            log.LogMethodExit(isEnabled);
            return isEnabled;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetType, parameter, culture);
            log.LogMethodExit();
            return null;
        }
        #endregion
    }
}
