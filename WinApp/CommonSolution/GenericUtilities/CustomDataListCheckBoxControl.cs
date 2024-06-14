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
    /// Custom data list check box control
    /// </summary>
    public class CustomDataListCheckBoxControl: AbstractCustomDataListControl
    {
        private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CustomCheckBox checkBox;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">custom attributes dto</param>
        public CustomDataListCheckBoxControl(CustomAttributesDTO customAttributesDTO):base(customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            if(customAttributesDTO.CustomAttributeValueListDTOList != null)
            {
                InitializeCmponents();
            }
            log.LogMethodExit();
        }

        private void InitializeCmponents()
        {
            log.LogMethodEntry();
            checkBox = new CustomCheckBox();
            Controls.Add(checkBox);
            checkBox.Tag = customAttributesDTO.Name;
            if (ContainsDefaultValue() && GetDefaultValue() == "Y")
            {
                checkBox.Checked = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the current selected value
        /// </summary>
        /// <returns>value</returns>
        protected override int GetSelectedValue()
        {
            int selectedValueId = -1;
            log.LogMethodEntry();
            selectedValueId = GetValueId(checkBox.Checked ? "1" : "0");
            log.LogMethodExit(selectedValueId);
            return selectedValueId;
        }

        /// <summary>
        /// sets the current selected value
        /// </summary>
        /// <param name="valueId">value id</param>
        protected override void SetSelectedValue(int valueId)
        {
            log.LogMethodEntry(valueId);
            if (GetValue(valueId) == "1")
            {
                checkBox.Checked = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Display error state
        /// </summary>
        public override void ShowErrorState()
        {
            log.LogMethodEntry();
            BackColor = Color.OrangeRed;
            log.LogMethodExit();    
        }

        /// <summary>
        /// Clear error state
        /// </summary>
        public override void ClearErrorState()
        {
            log.LogMethodEntry();
            BackColor = Color.Transparent;
            log.LogMethodExit();
        }

        /// <summary>
        /// changes enabled flag
        /// </summary>
        /// <param name="value"></param>
        public override void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            checkBox.Enabled = value;
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the font
        /// </summary>
        /// <param name="font"></param>
        public override void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            checkBox.Font = new Font(font.FontFamily, font.Size, font.Style);
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                checkBox.FlatAppearance.MouseDownBackColor = value;
                checkBox.BackColor = value;
                checkBox.FlatAppearance.MouseOverBackColor = value;
                checkBox.FlatAppearance.CheckedBackColor = value;
            }
        }
    }
}
