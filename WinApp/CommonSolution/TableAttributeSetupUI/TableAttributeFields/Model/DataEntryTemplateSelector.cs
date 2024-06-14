using Semnox.Parafait.CommonUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Semnox.Parafait.TableAttributeSetupUI
{
    public class DataEntryTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            DataEntryElement dataEntry = item as DataEntryElement;
            
            DataTemplate dataTemplate = element.FindResource("CustomTextBlockDataTemplate") as DataTemplate;
            if(dataEntry != null)
            { 
                switch (dataEntry.Type)
                {
                    case DataEntryType.TextBox:
                        {
                            dataTemplate = element.FindResource("CustomTextBoxDataTemplate") as DataTemplate;
                        }
                        break;
                    case  DataEntryType.ComboBox:
                        {
                            dataTemplate = element.FindResource("CustomComboBoxDataTemplate") as DataTemplate;
                        }
                        break;
                    case DataEntryType.DatePicker:
                        {
                            dataTemplate = element.FindResource("CustomDatePickerDataTemplate") as DataTemplate;
                        }
                        break;
                }
            }
            return dataTemplate;
        }
    }
}
