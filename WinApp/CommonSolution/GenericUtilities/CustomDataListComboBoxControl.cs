using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Custom data list combobox control
    /// </summary>
    public class CustomDataListComboBoxControl : AbstractCustomDataListControl
    {
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ComboBox comboBox;
        
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">custom attributes dto</param>
        public CustomDataListComboBoxControl(CustomAttributesDTO customAttributesDTO) : base(customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            InitializeComponents();
            log.LogMethodExit();
        }

        private void InitializeComponents()
        {
            log.LogMethodEntry();
            comboBox = new ComboBox();
            comboBox.Margin = new Padding(1);
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "ValueId";
            comboBox.DataSource = GetDataSource();
            comboBox.SelectedValue = GetDefaultValueId();
            comboBox.Tag = customAttributesDTO.Name;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            Controls.Add(comboBox);
            log.LogMethodExit();
        }

        private List<CustomAttributeValueListDTO> GetDataSource()
        {
            log.LogMethodEntry();
            List<CustomAttributeValueListDTO> customAttributeValueListDTOList = new List<CustomAttributeValueListDTO>();
            if (ContainsDefaultValue() == false)
            {
                customAttributeValueListDTOList.Add(new CustomAttributeValueListDTO());
                customAttributeValueListDTOList[0].Value = "Select";
            }
            customAttributeValueListDTOList.AddRange(customAttributesDTO.CustomAttributeValueListDTOList);
            log.LogMethodExit(customAttributeValueListDTOList);
            return customAttributeValueListDTOList;
        }

        /// <summary>
        /// Returns the current selected value
        /// </summary>
        /// <returns>value</returns>
        protected override int GetSelectedValue()
        {
            log.LogMethodEntry();
            int selectedValueId = -1;
            log.LogMethodEntry();
            selectedValueId = Convert.ToInt32(comboBox.SelectedValue);
            log.LogMethodExit(selectedValueId);
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
            comboBox.SelectedValue = valueId;
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
            comboBox.Enabled = value;
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the font
        /// </summary>
        /// <param name="font"></param>
        public override void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            comboBox.Font = new Font(font.FontFamily, font.Size, font.Style);
            log.LogMethodExit();
        }
    }
}
