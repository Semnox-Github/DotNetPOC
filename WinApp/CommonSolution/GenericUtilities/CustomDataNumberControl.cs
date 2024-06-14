using Semnox.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Custom data number text box control
    /// </summary>
    public class CustomDataNumberControl : TextBox, ICustomDataControl
    {
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomDataDTO customDataDTO;
        private CustomAttributesDTO customAttributesDTO;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">customAttributesDTO</param>
        public CustomDataNumberControl(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            Anchor = AnchorStyles.Left;
            this.customAttributesDTO = customAttributesDTO;
            Width = 120;
            this.Tag = customAttributesDTO.Name;
            KeyPress += CustomDataNumberTextBox_KeyPress;
            log.LogMethodExit(null);
        }

        private void CustomDataNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            char decimalChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && !(e.KeyChar == decimalChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Set the custom data dto
        /// </summary>
        /// <param name="customDataDTO"></param>
        public void SetCustomDataDTO(CustomDataDTO customDataDTO)
        {
            log.LogMethodEntry(customDataDTO);
            this.customDataDTO = customDataDTO;
            Text = string.Empty;
            if(customDataDTO.CustomDataNumber != null && customDataDTO.CustomDataNumber.HasValue)
            {
                Text = customDataDTO.CustomDataNumber.ToString();
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// saves the number value to the custom data dto
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (customDataDTO != null)
            {
                try
                {
                    if(string.IsNullOrWhiteSpace(Text) == false)
                    {
                        decimal number = decimal.Parse(Text);
                        if(customDataDTO.CustomDataNumber != number)
                        {
                            customDataDTO.CustomDataNumber = decimal.Parse(Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while parsing the number", ex);
                    customDataDTO.CustomDataNumber = null;
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
        /// Display error state
        /// </summary>
        public void ShowErrorState()
        {
            log.LogMethodEntry();
            BackColor = Color.OrangeRed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear error state
        /// </summary>
        public void ClearErrorState()
        {
            log.LogMethodEntry();
            BackColor = Color.White;
            log.LogMethodExit();
        }

        /// <summary>
        /// changes enabled flag
        /// </summary>
        /// <param name="value"></param>
        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            Enabled = value;
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the font
        /// </summary>
        /// <param name="font"></param>
        public void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            Font = new Font(font.FontFamily, font.Size, font.Style);
            log.LogMethodExit();
        }
    }
}
