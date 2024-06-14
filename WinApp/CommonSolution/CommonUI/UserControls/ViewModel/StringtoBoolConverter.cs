/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Game Management - used for attributes for flag 0,1 to bool value converter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Amitha Joy              Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Globalization;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{

    class StringtoBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value =="1" ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "1" : "0";
        }
    }
}
