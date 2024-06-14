/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - CustomDataListRadioButtonControl  
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Modified logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Custom data list radio button control
    /// </summary>
    public class CustomDataListRadioButtonControl : AbstractCustomDataListControl
    {
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<RadioButton> radioButtonList;
        private int radioButtonCount;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="customAttributesDTO">custom attributes dto</param>
        public CustomDataListRadioButtonControl(CustomAttributesDTO customAttributesDTO):base(customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            InitializeComponents();
            log.LogMethodExit();
        }

        public int RadioButtonCount
        {
            get { return customAttributesDTO.CustomAttributeValueListDTOList.Count; }
        }
        private void InitializeComponents()
        {
            log.LogMethodEntry();
            radioButtonList = new List<RadioButton>();
            foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.AutoSize = true;
                radioButton.Text = customAttributeValueListDTO.Value;
                radioButton.Tag = customAttributeValueListDTO.ValueId;
                radioButton.Click += radioButton_Click;
                Controls.Add(radioButton);
                radioButtonList.Add(radioButton);
            }
            if (ContainsDefaultValue())
            {
                int defaultValueId = GetDefaultValueId();
                foreach (var radioButton in radioButtonList)
                {
                    if (Convert.ToInt32(radioButton.Tag) == defaultValueId)
                    {
                        radioButton.Checked = true;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void radioButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (var radioButton in radioButtonList)
            {
                if (radioButton != sender)
                {
                    radioButton.Checked = false;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the current selected value
        /// </summary>
        /// <returns>value</returns>
        protected override int GetSelectedValue()
        {
            log.LogMethodEntry();
            int selectedValueId = -1;
            foreach (var radioButton in radioButtonList)
            {
                if (radioButton.Checked)
                {
                    selectedValueId = Convert.ToInt32(radioButton.Tag);
                }
            }
            log.LogMethodExit(selectedValueId);
            return selectedValueId;
        }

        /// <summary>
        /// sets the current selected value
        /// </summary>
        /// <param name="valueId">value id</param>
        protected override void SetSelectedValue(int valueId)
        {
            log.LogMethodEntry("valueId");
            foreach (var radioButton in radioButtonList)
            {
                if (Convert.ToInt32(radioButton.Tag) == valueId)
                {
                    radioButton.Checked = true;
                }
                else
                {
                    radioButton.Checked = false;
                }
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
            foreach (var radioButton in radioButtonList)
            {
                radioButton.Enabled = value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// sets the font
        /// </summary>
        /// <param name="font"></param>
        public override void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            foreach (var radioButton in radioButtonList)
            {
                radioButton.Font = new Font(font.FontFamily, font.Size, font.Style);
            }
            log.LogMethodExit();
        }
    }
}
