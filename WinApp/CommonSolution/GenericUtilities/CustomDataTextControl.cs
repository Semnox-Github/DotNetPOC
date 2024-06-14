using Semnox.Core;
//using Semnox.Core.GenericUtilities.Validation;
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
    /// Custom data text box control
    /// </summary>
    public class CustomDataTextControl : TextBox, ICustomDataControl
    {
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomDataDTO customDataDTO;
        private CustomAttributesDTO customAttributesDTO;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">customAttributesDTO</param>
        public CustomDataTextControl(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            Anchor = AnchorStyles.Left;
            this.Tag = customAttributesDTO.Name;
            this.customAttributesDTO = customAttributesDTO;
            Width = 120;
            Location = new System.Drawing.Point(5, 0);
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
            if(string.IsNullOrWhiteSpace(customDataDTO.CustomDataText) == false)
            {
                Text = customDataDTO.CustomDataText;
            }
            else
            {
                Text = "";
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// saves the number value to the custom data dto
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (customDataDTO != null && customDataDTO.CustomDataText != Text)
            {
                customDataDTO.CustomDataText = Text;
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
