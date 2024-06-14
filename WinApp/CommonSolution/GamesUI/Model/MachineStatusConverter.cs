using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.ViewContainer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Semnox.Parafait.GamesUI
{
    public class MachineStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<MachineAttributeDTO> GameMachineAttributes = value as List<MachineAttributeDTO>;
            return GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).
                                  Select(x => x.AttributeValue == "0" ? MessageViewContainerList.GetMessage((parameter as ExecutionContext), "In Service") : MessageViewContainerList.GetMessage((parameter as ExecutionContext), "Out of Service")).FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
