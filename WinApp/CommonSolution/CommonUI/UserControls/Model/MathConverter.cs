using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class MathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            CustomDataGridColumnElement dataEntryElement = parameter as CustomDataGridColumnElement;
            int result = 0;
            if (values != null && values[0] != null)
            {
                if (dataEntryElement.Properties.Values.All(x => x == ArthemeticOperationType.Multiply))
                {
                    result = 1;
                }
                foreach (KeyValuePair<string, ArthemeticOperationType> property in dataEntryElement.Properties)
                {
                    switch (property.Value)
                    {
                        case ArthemeticOperationType.Add:
                            {
                                object value = values[0].GetType().GetProperty(property.Key) != null ?
                                    values[0].GetType().GetProperty(property.Key).GetValue(values[0]) : null;
                                result += value != null ? (int)value : 0;
                            };
                            break;
                        case ArthemeticOperationType.Subtract:
                            {
                                object value = values[0].GetType().GetProperty(property.Key) != null ?
                                    values[0].GetType().GetProperty(property.Key).GetValue(values[0]) : null;
                                result -= value != null ? (int)value : 0;
                            }
                            break;
                        case ArthemeticOperationType.Multiply:
                            {
                                object value = values[0].GetType().GetProperty(property.Key) != null ?
                                   values[0].GetType().GetProperty(property.Key).GetValue(values[0]) : null;
                                result *= value != null ? (int)value : 1;
                            }
                            break;
                    }
                }
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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
