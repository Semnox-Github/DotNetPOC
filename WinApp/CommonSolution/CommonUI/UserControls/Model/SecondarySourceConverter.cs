/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - Common - Custom combo box
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.100.0     25-Sep-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class SecondarySourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object result = null;
            IEnumerable<object> parameterList = (IEnumerable<object>)parameter;
            CustomDataGridColumnElement dataEntryElement = parameterList.ElementAt(1) as CustomDataGridColumnElement;
            if (parameterList.ElementAt(0) is IEnumerable<object> && dataEntryElement.SecondarySource is IEnumerable<object>)
            {
                IEnumerable<object> sourceList = (IEnumerable<object>)parameterList.ElementAt(0);
                if (sourceList != null && dataEntryElement.SecondarySource != null)
                {
                    int index = sourceList.ToList().IndexOf(values[0]);
                    if (index != -1)
                    {
                        object element = sourceList.ElementAt(index);
                        if (dataEntryElement.SourcePropertyName.Contains("."))
                        {
                            result = element.GetType().GetProperty(dataEntryElement.SourcePropertyName.Substring(0, dataEntryElement.SourcePropertyName.IndexOf("."))).GetValue(element);
                            if (result != null)
                            {
                                result = result.GetType().GetProperty(dataEntryElement.SourcePropertyName.Substring(dataEntryElement.SourcePropertyName.IndexOf(".") + 1)).GetValue(result);
                            }
                        }
                        else
                        {
                            result = element.GetType().GetProperty(dataEntryElement.SourcePropertyName).GetValue(sourceList.ElementAt(index));
                        }
                        if (result != null)
                        {
                            object secondarySourceElement = dataEntryElement.SecondarySource.FirstOrDefault(x => x.GetType().
                            GetProperty(dataEntryElement.ChildOrSecondarySourcePropertyName).GetValue(x).ToString().ToLower()
                            == result.ToString().ToLower());
                            if (secondarySourceElement != null)
                            {
                                result = secondarySourceElement.GetType().GetProperty(parameterList.ElementAt(2).ToString()).GetValue(secondarySourceElement);
                            }
                        }
                    }
                }
            }
            if (result == null || result.ToString() == "-1")
            {
                return string.Empty;
            }
            else if (dataEntryElement != null && !string.IsNullOrEmpty(dataEntryElement.DataGridColumnStringFormat) &&
              !string.IsNullOrWhiteSpace(dataEntryElement.DataGridColumnStringFormat))
            {
                if (result.GetType() is IEnumerable<object>)
                {
                    return (result as IEnumerable<object>).Count().ToString(dataEntryElement.DataGridColumnStringFormat);
                }
                else if (result.GetType() == typeof(DateTime) || result.GetType() == typeof(DateTime?))
                {
                    if (((DateTime)result).ToString() != DateTime.MinValue.ToString())
                    {
                        return ((DateTime)result).ToString(dataEntryElement.DataGridColumnStringFormat);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else if (result.GetType() == typeof(Int32))
                {
                    return ((Int32)result).ToString(dataEntryElement.DataGridColumnStringFormat);
                }
                else if (result.GetType() == typeof(Double))
                {
                    return ((double)result).ToString(dataEntryElement.DataGridColumnStringFormat);
                }
                else
                {
                    return result.ToString();
                }
            }
            else
            {
                return result.ToString();
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
