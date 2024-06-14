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
    /// Custom data datetime control
    /// </summary>
    public class CustomDataDateTimeControl:FlowLayoutPanel, ICustomDataControl
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomDataDTO customDataDTO;
        private CustomAttributesDTO customAttributesDTO;
        private TextBox textBox;
        private DateTimePicker dateTimePicker;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">customAttributesDTO</param>
        public CustomDataDateTimeControl(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            Margin = new Padding(0);
            Anchor = AnchorStyles.Left;
            AutoSize = true;
            FlowDirection = FlowDirection.LeftToRight;
            this.customAttributesDTO = customAttributesDTO;
            textBox = new TextBox();
            textBox.Width = 120;
            textBox.Margin = new Padding(3, 3, 0, 3);
            textBox.Tag = customAttributesDTO.Name;
            Controls.Add(textBox);
            dateTimePicker = new DateTimePicker();
            dateTimePicker.Width = 17;
            dateTimePicker.Margin = new Padding(0, 3, 3, 3);
            Controls.Add(dateTimePicker);
            dateTimePicker.ValueChanged += dateTimePicker_ValueChanged;
            log.LogMethodExit(null);
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            textBox.Text = dateTimePicker.Value.ToString("dd-MMM-yyyy");
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
            if(customDataDTO.CustomDataDate != null)
            {
                textBox.Text = customDataDTO.CustomDataDate.Value.ToString("dd-MMM-yyyy");
                dateTimePicker.Value = customDataDTO.CustomDataDate.Value;
            }
            log.LogMethodExit(null);
        }

        /// <summary>
        /// saves the datetime value to the custom data dto
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            if (customDataDTO != null)
            {
                DateTime? dateTimeValue = null;
                if (string.IsNullOrWhiteSpace(textBox.Text) == false)
                {
                    DateTime dateTime;
                    if (DateTime.TryParse(textBox.Text, out dateTime))
                    {
                        dateTimeValue = dateTime;
                    }
                }
                if(customDataDTO.CustomDataDate != dateTimeValue)
                {
                    customDataDTO.CustomDataDate = dateTimeValue;
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
                if(customAttributesDTO != null)
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
            textBox.BackColor = Color.OrangeRed;
            log.LogMethodExit();
        }

        /// <summary>
        /// Clear error state
        /// </summary>
        public void ClearErrorState()
        {
            log.LogMethodEntry();
            textBox.BackColor = Color.White;
            log.LogMethodExit();
        }

        /// <summary>
        /// changes enabled flag
        /// </summary>
        /// <param name="value"></param>
        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            textBox.Enabled = value;
            dateTimePicker.Enabled = value;
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the font
        /// </summary>
        /// <param name="font"></param>
        public void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            textBox.Font = new Font(font.FontFamily, font.Size,font.Style);
            dateTimePicker.Font = new Font(font.FontFamily, font.Size, font.Style);
            log.LogMethodExit();
        }
    }
}
