using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Factory class to create custom data UI controls
    /// </summary>
    public class CustomDataControlFactory
    {
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the custom data control based on the attribute type
        /// </summary>
        /// <param name="customAttributesDTO"></param>
        /// <returns></returns>
        public static ICustomDataControl GetCustomDataControl(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            ICustomDataControl customDataControl = null;
            switch(customAttributesDTO.Type)
            {
                case "TEXT":
                    {
                        customDataControl = new CustomDataTextControl(customAttributesDTO);
                        break;
                    }
                case "NUMBER":
                    {
                        customDataControl = new CustomDataNumberControl(customAttributesDTO);
                        break;
                    }
                case "DATE":
                    {
                        customDataControl = new CustomDataDateTimeControl(customAttributesDTO);
                        break;
                    }
                case "LIST":
                    {
                        if (IsBooleanList(customAttributesDTO))
                        {
                            customDataControl = new CustomDataListCheckBoxControl(customAttributesDTO);
                        }
                        else if (IsRadioButtonList(customAttributesDTO))
                        {
                            customDataControl = new CustomDataListRadioButtonControl(customAttributesDTO);
                        }
                        else
                        {
                            customDataControl = new CustomDataListComboBoxControl(customAttributesDTO);
                        }
                        break;
                    }
                default:
                    {
                        customDataControl = new CustomDataTextControl(customAttributesDTO);
                        break;
                    }
            }
            log.LogMethodExit(customDataControl);
            return customDataControl;
        }

        private static bool IsRadioButtonList(CustomAttributesDTO customAttributesDTO)
        {
            bool isRadiobuttonList = false;
            log.LogMethodEntry(customAttributesDTO);
            if (customAttributesDTO.CustomAttributeValueListDTOList.Count <= 3)
            {
                isRadiobuttonList = true;
            }
            log.LogMethodExit(isRadiobuttonList);
            return isRadiobuttonList;
        }

        private static bool IsBooleanList(CustomAttributesDTO customAttributesDTO)
        {
            bool isBooleanList = false;
            log.LogMethodEntry(customAttributesDTO);
            if (customAttributesDTO.CustomAttributeValueListDTOList.Count == 2 &&
                (customAttributesDTO.CustomAttributeValueListDTOList[0].Value == "0" ||
                customAttributesDTO.CustomAttributeValueListDTOList[0].Value == "1") &&
                (customAttributesDTO.CustomAttributeValueListDTOList[1].Value == "0" ||
                customAttributesDTO.CustomAttributeValueListDTOList[1].Value == "1"))
            {
                isBooleanList = true;
            }
            log.LogMethodExit(isBooleanList);
            return isBooleanList;
        }
    }
}
