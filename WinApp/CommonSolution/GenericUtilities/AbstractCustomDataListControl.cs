using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Abstarct class of custom data list control classes
    /// </summary>
    public abstract class AbstractCustomDataListControl: FlowLayoutPanel, ICustomDataControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// CustomAttributesDTO
        /// </summary>
        protected CustomAttributesDTO customAttributesDTO;

        /// <summary>
        /// CustomDataDTO
        /// </summary>
        protected CustomDataDTO customDataDTO;

        /// <summary>
        /// Event generated when custom data text box is entered
        /// </summary>
        public event EventHandler CustomDataTextBoxEnter;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">custom attributes dto</param>
        public AbstractCustomDataListControl(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            InitializeComponents();
            this.customAttributesDTO = customAttributesDTO;
            log.LogMethodExit();
        }

        private void InitializeComponents()
        {
            Anchor = AnchorStyles.Left;
            Margin = new Padding(3);
            Padding = new Padding(0);
            FlowDirection = FlowDirection.LeftToRight;
            AutoSize = true;
        }

        /// <summary>
        /// returns the value id corresponding to the value
        /// </summary>
        /// <param name="value">value</param>
        /// <returns></returns>
        protected int GetValueId(string value)
        {
            int valueId = -1;
            log.LogMethodEntry(value);
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                {
                    if (customAttributeValueListDTO.Value == value)
                    {
                        valueId = customAttributeValueListDTO.ValueId;
                    }
                }
            }
            log.LogMethodExit(valueId);
            return valueId;
        }

        /// <summary>
        /// Returns the value corresponding to the value id
        /// </summary>
        /// <param name="valueId">value id</param>
        /// <returns></returns>
        protected string GetValue(int valueId)
        {
            log.LogMethodEntry(valueId);
            string value = "";
            if (valueId != -1)
            {
                foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                {
                    if (customAttributeValueListDTO.ValueId == valueId)
                    {
                        value = customAttributeValueListDTO.Value;
                        break;
                    }
                }
            }
            log.LogMethodExit(value);
            return value;
        }

        /// <summary>
        /// Returns whether list has any default value
        /// </summary>
        /// <returns></returns>
        protected bool ContainsDefaultValue()
        {
            bool hasDefaultValue = false;
            log.LogMethodEntry();
            foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
            {
                if (customAttributeValueListDTO.IsDefault == "Y")
                {
                    hasDefaultValue = true;
                    break;
                }
            }
            log.LogMethodExit(hasDefaultValue);
            return hasDefaultValue;

        }

        /// <summary>
        /// Returns the default value id
        /// </summary>
        /// <returns></returns>
        protected int GetDefaultValueId()
        {
            log.LogMethodEntry();
            int defaultValueId = -1;
            foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
            {
                if (customAttributeValueListDTO.IsDefault == "Y")
                {
                    defaultValueId = customAttributeValueListDTO.ValueId;
                    break;
                }
            }
            log.LogMethodExit(defaultValueId);
            return defaultValueId;
        }

        /// <summary>
        /// Returns the default value
        /// </summary>
        /// <returns></returns>
        protected string GetDefaultValue()
        {
            log.LogMethodEntry();
            string defaultValue = "";
            foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
            {
                if (customAttributeValueListDTO.IsDefault == "Y")
                {
                    defaultValue = customAttributeValueListDTO.Value;
                    break;
                }
            }
            log.LogMethodExit(defaultValue);
            return defaultValue;
        }

        /// <summary>
        /// Set the custom data dto
        /// </summary>
        /// <param name="customDataDTO"></param>
        public void SetCustomDataDTO(CustomDataDTO customDataDTO)
        {
            log.LogMethodEntry(customDataDTO);
            this.customDataDTO = customDataDTO;
            SetSelectedValue(customDataDTO.ValueId);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// saves the valueid to the custom data dto
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (customDataDTO != null)
            {
                int valueId = GetSelectedValue();
                if(customDataDTO.ValueId != valueId)
                {
                    customDataDTO.ValueId = valueId;
                }
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// returns the custom attribute id
        /// </summary>
        public int CustomAttributeId
        {
            get
            {
                int customAttributeId = -1;
                if (customAttributesDTO != null)
                {
                    customAttributeId = customAttributesDTO.CustomAttributeId;
                }
                return customAttributeId;
            }
        }

        /// <summary>
        /// Returns the current selected value
        /// </summary>
        /// <returns></returns>
        abstract protected int GetSelectedValue();

        /// <summary>
        /// sets the current selected value
        /// </summary>
        /// <param name="valueId">value id</param>
        abstract protected void SetSelectedValue(int valueId);

        /// <summary>
        /// Display error state
        /// </summary>
        abstract public void ShowErrorState();

        /// <summary>
        /// Clear error state
        /// </summary>
        abstract public void ClearErrorState();
        /// <summary>
        /// 
        /// </summary>
        abstract public void SetControlsEnabled(bool value);

        /// <summary>
        /// Sets the font size
        /// </summary>
        /// <param name="font"></param>
        abstract public void SetFont(Font font);
    }
}
