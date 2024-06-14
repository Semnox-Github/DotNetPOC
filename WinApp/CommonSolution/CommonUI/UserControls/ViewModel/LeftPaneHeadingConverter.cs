/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Heading converter for leftpane menu
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Globalization;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class LeftPaneHeadingConverter : IValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            string heading = value as String;

            if (heading != null)
            {
                if (!string.IsNullOrEmpty(heading) && heading.Contains(" "))
                    heading = heading.Replace(" ", Environment.NewLine);

            }
            log.LogMethodExit(heading);
            return heading;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
