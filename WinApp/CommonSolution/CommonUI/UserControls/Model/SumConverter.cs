using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class SumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CustomDataGridColumnElement dataEntryElement = parameter as CustomDataGridColumnElement;
            int result = 0;
            if (value is IEnumerable<object>)
            {
                IEnumerable<object> list = (IEnumerable<object>)value;
                result = list.Sum(x => (int)x.GetType().GetProperty(dataEntryElement.ChildOrSecondarySourcePropertyName).GetValue(x));
            }
            if (result.ToString() == "-1")
            {
                return "-";
            }
            else if (!string.IsNullOrEmpty(dataEntryElement.DataGridColumnStringFormat) &&
               !string.IsNullOrWhiteSpace(dataEntryElement.DataGridColumnStringFormat))
            {
                return result.ToString(dataEntryElement.DataGridColumnStringFormat);
            }
            else
            {
                return result.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
