using Semnox.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// UI class to display custom data
    /// </summary>
    public partial class CustomDataListUI : Form
    {
        private Utilities utilities;
        private CustomDataSetDTO customDataSetDTO;
        private Applicability applicability;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private CustomDataTableLayoutPanel customDataTableLayoutPanel;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="utilities">Parafait utilities</param>
        /// <param name="customDataSetDTO">CustomDataSet DTO </param>
        /// <param name="applicability">Applicability</param>
        public CustomDataListUI(Utilities utilities, CustomDataSetDTO customDataSetDTO, Applicability applicability)
        {
            log.LogMethodEntry(utilities, customDataSetDTO, applicability);
            this.utilities = utilities;
            this.customDataSetDTO = customDataSetDTO;
            this.applicability = applicability;
            InitializeComponent();
            executionContext = utilities.ExecutionContext;
            utilities.setLanguage(this);
            System.Globalization.CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.TextInfo textInfo = cultureInfo.TextInfo;
            lblHeading.Text = utilities.MessageUtils.getMessage("Custom Attribute Data for ") + utilities.MessageUtils.getMessage(applicability.ToString());
            lblHeading.Text = textInfo.ToTitleCase(lblHeading.Text.ToLower());
            log.LogMethodExit();
        }

        private void CustomDataListUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            customDataTableLayoutPanel = new CustomDataTableLayoutPanel(executionContext, applicability);
            customDataTableLayoutPanel.Location = new Point(0, 0);
            customDataTableLayoutPanel.Height = customAttributesPanel.ClientSize.Height;
            customDataTableLayoutPanel.Width = customAttributesPanel.ClientSize.Width;
            customAttributesPanel.Controls.Add(customDataTableLayoutPanel);
            customDataTableLayoutPanel.SetCustomDataSetDTO(customDataSetDTO);
            log.LogMethodExit();
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            customDataTableLayoutPanel.ClearErrorState();
            customDataTableLayoutPanel.Save();
            if(customDataSetDTO != null && 
                customDataSetDTO.CustomDataDTOList != null && 
                customDataSetDTO.CustomDataDTOList.Count > 0)
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetDTO);
                try
                {
                    customDataSetBL.Save();
                }
                catch (ValidationException ex)
                {
                    log.Error("validation failed", ex);
                    customDataTableLayoutPanel.HandleValidationErrors(ex.ValidationErrorList);
                    StringBuilder errorMessageBuilder = new StringBuilder("");
                    foreach (var validationError in ex.ValidationErrorList)
                    {
                        errorMessageBuilder.Append(validationError.Message);
                        errorMessageBuilder.Append(Environment.NewLine);
                    }
                    MessageBox.Show(errorMessageBuilder.ToString());
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while saving the customdataset", ex);
                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                }
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(customDataSetDTO.CustomDataSetId == -1)
            {
                customDataSetDTO.CustomDataDTOList = new List<CustomDataDTO>();
            }
            else
            {
                CustomDataSetBL customDataSetBL = new CustomDataSetBL(executionContext, customDataSetDTO.CustomDataSetId);
                customDataSetDTO.CustomDataDTOList = customDataSetBL.CustomDataSetDTO.CustomDataDTOList;
            }
            customDataTableLayoutPanel.SetCustomDataSetDTO(customDataSetDTO);
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.DialogResult = DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
    }
}
