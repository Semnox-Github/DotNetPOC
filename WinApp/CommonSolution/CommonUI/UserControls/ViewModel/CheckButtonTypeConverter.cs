/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - check button type converter
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.110.0     25-Nov-2020   Raja Uthanda            Created for POS UI Redesign 
 ********************************************************************************************/

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Semnox.Parafait.CommonUI
{
    public class CheckButtonTypeConverter : IValueConverter
    {
        #region Members
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<DisplayTag> displayTags = value as ObservableCollection<DisplayTag>;
            bool isButtonType = false;
            if (displayTags != null && displayTags.Any(d => d.Type == DisplayTagType.Button))
            {
                isButtonType = true;
            }
            return isButtonType;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
