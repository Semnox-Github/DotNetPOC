/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - lower case converter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.130.0     07-July-2021  Raja                   Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Globalization;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class LowerCaseConverter : IValueConverter
    {
        #region Members
        #endregion

        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is string)
            {
                return value.ToString().ToLower();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
