using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace Semnox.Parafait.RedemptionUI
{
    public class AutoShowMenusConverter : IMultiValueConverter
    {
        #region Members
        private static readonly logging.Logger log = new logging.Logger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods	
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(values, targetType, parameter, culture);
            bool showMenu = true;
            if (values != null)
            {
                LeftPaneSelectedItem leftPaneSelectedItem = (LeftPaneSelectedItem)values[0];
                bool multiScreenMode = (bool)values[1];
                bool autoShowRedemptionProductMenu = (bool)values[2];
                bool autoShowLoadTicketProductMenu = (bool)values[3];
                if (!multiScreenMode)
                {
                    if (!autoShowRedemptionProductMenu &&
                        (leftPaneSelectedItem == LeftPaneSelectedItem.Redemption || leftPaneSelectedItem == LeftPaneSelectedItem.TurnIn))
                    {
                        showMenu = false;
                    }
                    else if (!autoShowLoadTicketProductMenu && leftPaneSelectedItem == LeftPaneSelectedItem.LoadTicket)
                    {
                        showMenu = false;
                    }
                }
            }
            log.LogMethodExit(showMenu);
            return showMenu;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry(value, targetTypes, parameter, culture);
            log.LogMethodExit(null);
            return null;
        }
        #endregion
    }
}
