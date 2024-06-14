/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Selected Item converter for combogroup
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
    public class SelectedItemConverter : IMultiValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            return new ComboBoxGroupSelectedItem()
            {
                ComboBoxGroupUserControl = values[0] as ComboGroupUserControl,
                ComboBoxField = values[1] as ComboBoxField
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
