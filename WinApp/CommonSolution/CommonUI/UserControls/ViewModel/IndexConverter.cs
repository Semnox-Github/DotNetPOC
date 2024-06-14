/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - IndexConverter for comboboxuser control
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
    public class IndexConverter : IMultiValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            log.LogMethodEntry();
            ComboGroupVM comboBoxGroupVM = values[0] as ComboGroupVM;

            ComboBoxField comboBoxField = values[1] as ComboBoxField;

            if (comboBoxGroupVM != null && comboBoxField != null && comboBoxGroupVM.ComboList != null && comboBoxGroupVM.ComboList.Count > 0
                && comboBoxGroupVM.ComboList.IndexOf(comboBoxField) == comboBoxGroupVM.ComboList.Count - 1)
            {
                return true;
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
