using System;
using System.Globalization;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class CheckDateMinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CustomDataGridColumnElement dataGridColumnElement = parameter as CustomDataGridColumnElement;
            if (value != null && (value.GetType() == typeof(DateTime) || value.GetType() == typeof(DateTime?)))
            {  
                if (value.ToString() == DateTime.MinValue.ToString())
                { 
                    return "-";
                }
                else if (dataGridColumnElement != null && !string.IsNullOrEmpty(dataGridColumnElement.DataGridColumnStringFormat)
                   && !string.IsNullOrWhiteSpace(dataGridColumnElement.DataGridColumnStringFormat))
                {
                    value = ((DateTime)value).ToString(dataGridColumnElement.DataGridColumnStringFormat);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
