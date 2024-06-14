/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - AdvancedSearch 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Modified logger methods.
 ********************************************************************************************/

using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Helper class to build the custom data list user interface
    /// </summary>
    public class CustomDataTableLayoutPanel : TableLayoutPanel
    {
        private List<CustomAttributesDTO> customAttributesDTOList;
        private CustomDataSetDTO customDataSetDTO;
         private static readonly Semnox.Parafait.logging.Logger  log =  new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ICustomDataControl> customDataControlList;
        private Dictionary<int, CustomDataDTO> customDataDTODictionary;
        private Dictionary<string, ICustomDataControl> attributeNameCustomDataControlDictionary;
        private ExecutionContext executionContext;
        private Applicability applicability;
        private bool customDataControlsEnabled = true;

        /// <summary>
        /// Parameterized constructor of CustomDataListUIBuilder
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="applicability">applicability</param>

        public CustomDataTableLayoutPanel(ExecutionContext executionContext, Applicability applicability)
        {
            log.LogMethodEntry(executionContext, applicability);
            this.executionContext = executionContext;
            this.applicability = applicability;
            customDataControlList = new List<ICustomDataControl>();
            attributeNameCustomDataControlDictionary = new Dictionary<string, ICustomDataControl>();
            InitializeComponenets();
            LoadCustomAttributes();
            log.LogMethodExit();
        }

        private void InitializeComponenets()
        {
            log.LogMethodEntry();
            Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top)
            | System.Windows.Forms.AnchorStyles.Left)
            )));
            ColumnCount = 2;
            Padding = new Padding(0,0,10,0);
            AutoScroll = false;
            ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            AutoScroll = true;
            //CellBorderStyle = TableLayoutPanelCellBorderStyle.Outset;
            log.LogMethodExit();
        }

        private void LoadCustomAttributes()
        {
            log.LogMethodEntry();
            List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.APPLICABILITY, applicability.ToString()));
            searchParameters.Add(new KeyValuePair<CustomAttributesDTO.SearchByParameters, string>(CustomAttributesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            OnLoadCustomAttributes(GetCustomAttributesDTOList(searchParameters));
            log.LogMethodExit();
        }

        private void OnLoadCustomAttributes(List<CustomAttributesDTO> customAttributesDTOList)
        {
            log.LogMethodEntry(customAttributesDTOList);
            this.customAttributesDTOList = customAttributesDTOList;
            CreateCustomDataControls();
            customDataDTODictionary = GetAttributeIdCustomDataDTODictionary();
            AssignToCustomDataControls();
            log.LogMethodExit();
        }

        private List<CustomAttributesDTO> GetCustomAttributesDTOList(List<KeyValuePair<CustomAttributesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<CustomAttributesDTO> customAttributesDTOList = null;
            CustomAttributesListBL customAttributesListBL = new CustomAttributesListBL(executionContext);
            try
            {
                customAttributesDTOList = customAttributesListBL.GetCustomAttributesDTOList(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while retrieveing CustomAttributesDTO List", ex);
            }
            if (customAttributesDTOList == null)
            {
                customAttributesDTOList = new List<CustomAttributesDTO>();
            }
            else
            {
                customAttributesDTOList = customAttributesDTOList.OrderBy((x) => x.Sequence).ToList();
            }
            log.LogMethodExit(customAttributesDTOList);
            return customAttributesDTOList;
        }

        private void CreateCustomDataControls()
        {
            log.LogMethodEntry();
            try
            {
                RowCount = customAttributesDTOList.Count + 1;
                for (int i = 0; i < customAttributesDTOList.Count; i++)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, customAttributesDTOList[i].Name) != "N")
                    {
                        if (!attributeNameCustomDataControlDictionary.ContainsKey(customAttributesDTOList[i].Name))
                        {
                            RowStyles.Add(new System.Windows.Forms.RowStyle());
                            Label label = GetLabel(customAttributesDTOList[i]);
                            ICustomDataControl customDataControl = CustomDataControlFactory.GetCustomDataControl(customAttributesDTOList[i]);
                            customDataControlList.Add(customDataControl);
                            attributeNameCustomDataControlDictionary.Add(customAttributesDTOList[i].Name, customDataControl);
                            Controls.Add(label, 0, i);
                            Controls.Add(customDataControl as Control, 1, i);
                        }
                    }
                }
                RowStyles.Add(new System.Windows.Forms.RowStyle());
                SetControlsEnabledStatus();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private Label GetLabel(CustomAttributesDTO customAttributesDTO)
        {
            log.LogMethodEntry(customAttributesDTO);
            Label label = new Label();
            label.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(0, 0);
            label.Margin = new Padding(5);
            label.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label.Name = customAttributesDTO.Name + "Label";
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            label.Text = customAttributesDTO.Name + ":";
            //label.Text = textInfo.ToTitleCase(label.Text.ToLower());
            label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label.Height = 25;
            log.LogMethodExit(label);
            return label;
        }

        /// <summary>
        /// Displays the custom data set
        /// </summary>
        /// <param name="customDataSetDTO"></param>
        public void SetCustomDataSetDTO(CustomDataSetDTO customDataSetDTO)
        {
            log.LogMethodEntry(customDataSetDTO);
            ClearErrorState();
            this.customDataSetDTO = customDataSetDTO;
            if(customAttributesDTOList != null)
            {
                customDataDTODictionary = GetAttributeIdCustomDataDTODictionary();
                AssignToCustomDataControls();
            }
            log.LogMethodExit();
        }

        private Dictionary<int, CustomDataDTO> GetAttributeIdCustomDataDTODictionary()
        {
            log.LogMethodEntry();
            Dictionary<int, CustomDataDTO> attributeIdCustomDataDTODictionary = new Dictionary<int, CustomDataDTO>();
            if (customDataSetDTO != null && customDataSetDTO.CustomDataDTOList != null)
            {
                foreach (var customDataDTO in customDataSetDTO.CustomDataDTOList)
                {
                    if (attributeIdCustomDataDTODictionary.ContainsKey(customDataDTO.CustomAttributeId) == false)
                    {
                        attributeIdCustomDataDTODictionary.Add(customDataDTO.CustomAttributeId, customDataDTO);
                    }
                }
            }
            log.LogMethodExit(attributeIdCustomDataDTODictionary);
            return attributeIdCustomDataDTODictionary;
        }

        /// <summary>
        /// display validation error
        /// </summary>
        /// <param name="validationErrorList"></param>
        public void HandleValidationErrors(List<ValidationError> validationErrorList)
        {
            log.LogMethodEntry(validationErrorList);
            foreach (var validationError in validationErrorList)
            {
                if(attributeNameCustomDataControlDictionary.ContainsKey(validationError.FieldName))
                {
                    attributeNameCustomDataControlDictionary[validationError.FieldName].ShowErrorState();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Clears the UI from validation error state
        /// </summary>
        public void ClearErrorState()
        {
            log.LogMethodEntry();
            foreach (var customDataControl in customDataControlList)
            {
                customDataControl.ClearErrorState();
            }
            log.LogMethodExit();
        }

        private void AssignToCustomDataControls()
        {
            log.LogMethodEntry();
            foreach (var customDataControl in customDataControlList)
            {
                CustomDataDTO customDataDTO = null;
                if (customDataDTODictionary.ContainsKey(customDataControl.CustomAttributeId))
                {
                    customDataDTO = customDataDTODictionary[customDataControl.CustomAttributeId];
                }
                else
                {
                    customDataDTO = new CustomDataDTO();
                    customDataDTO.CustomAttributeId = customDataControl.CustomAttributeId;
                    customDataDTO.ValueId = GetDefaultValueId(customDataControl.CustomAttributeId);
                    if (customDataSetDTO != null)
                    {
                        if (customDataSetDTO.CustomDataDTOList == null)
                        {
                            customDataSetDTO.CustomDataDTOList = new List<CustomDataDTO>();
                        }
                        customDataSetDTO.CustomDataDTOList.Add(customDataDTO);
                    }
                    customDataDTODictionary.Add(customDataControl.CustomAttributeId, customDataDTO);
                }
                customDataControl.SetCustomDataDTO(customDataDTO);
            }
            log.LogMethodExit();
        }

        private int GetDefaultValueId(int customAttributeId)
        {
            log.LogMethodEntry(customAttributeId);
            int result = -1;
            if(customAttributesDTOList != null && customAttributeId > -1)
            {
                foreach (var customAttributesDTO in customAttributesDTOList)
                {
                    if(customAttributesDTO.CustomAttributeId == customAttributeId)
                    {
                        if(customAttributesDTO.Type == "LIST" && customAttributesDTO.CustomAttributeValueListDTOList != null)
                        {
                            foreach (var customAttributeValueListDTO in customAttributesDTO.CustomAttributeValueListDTOList)
                            {
                                if(customAttributeValueListDTO.IsDefault == "Y")
                                {
                                    result = customAttributeValueListDTO.ValueId;
                                    return result;
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// saves the custom data to the custom data dto
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            foreach (var customDataControl in customDataControlList)
            {
                customDataControl.Save();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Changes whether the ui is enabled
        /// </summary>
        /// <param name="value"></param>
        public void SetControlsEnabled(bool value)
        {
            log.LogMethodEntry(value);
            customDataControlsEnabled = value;
            SetControlsEnabledStatus();
            log.LogMethodExit();
        }

        private void SetControlsEnabledStatus()
        {
            log.LogMethodEntry();
            foreach (var customDataControl in customDataControlList)
            {
                customDataControl.SetControlsEnabled(customDataControlsEnabled);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Whether Contains CustomData Controls
        /// </summary>
        /// <returns></returns>
        public bool ContainsCustomDataControls()
        {
            log.LogMethodEntry();
            bool result = false;
            if(customAttributesDTOList != null && customAttributesDTOList.Count > 0 &&
                customDataControlList != null && customDataControlList.Count > 0)
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// sets the font size for all the controls
        /// </summary>
        /// <param name="font"></param>
        public void SetFont(Font font)
        {
            log.LogMethodEntry(font);
            foreach (var customDataControl in customDataControlList)
            {
                customDataControl.SetFont(font);
            }
            log.LogMethodExit();
        }
    }
}
